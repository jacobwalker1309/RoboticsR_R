using Moq;
using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Infrastructure.Services;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit.Abstractions;

public class DockerServiceTests
{
    private readonly ITestOutputHelper _output;

    public DockerServiceTests(ITestOutputHelper output)
    {
        _output = output;  // Capture output for debugging
    }

    [Fact]
    public async Task RunDockerComposeUp_AllServicesAlreadyRunning_ReturnsMessage()
    {
        // Arrange
        var mockPathService = new Mock<IPathService>();
        var mockCommandService = new Mock<ICommandService>();

        mockPathService
            .Setup(service => service.GetDockerComposeFilePath())
            .Returns(@"C:\projects\DotNet\Robotics\docker-compose.yml");

        // Mock ExecuteCommandAsync for docker-compose services
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker-compose",
                $"-f \"{mockPathService.Object.GetDockerComposeFilePath()}\" config --services", true))
            .ReturnsAsync(("mssql\nredis\nntp-server", ""));

        // Mock GetRunningContainers to return all containers
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker", "ps --format \"{{.Names}}\"", true))
            .ReturnsAsync(("mssql\nredis\nntp-server", ""));

        // Mock `docker-compose up` to simulate that no services need to be started
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker-compose",
                It.Is<string>(cmd => cmd.Contains("up --build")), false))
            .ReturnsAsync(("All services already running.", ""));

        var dockerService = new DockerService(mockPathService.Object, mockCommandService.Object);

        // Act
        var result = await dockerService.RunDockerComposeUp();

        // Assert
        Assert.Equal("All services already running.", result.Output);
        Assert.Equal("", result.Error);

        // Verify `docker-compose up` command was called 0 times (since all services are already running)
        mockCommandService.Verify(service => service.ExecuteCommandAsync(
         It.Is<string>(cmd => cmd.Contains("up --build")),
         It.IsAny<string>(),  // Matches any string passed as the second argument
         false  // This is correct if your actual method accepts a Boolean for `waitForExit`
     ), Times.Never);
    }



    [Fact]
    public async Task RunDockerComposeUp_SomeServicesDown_StartsOnlyStoppedServices()
    {
        // Arrange
        var mockPathService = new Mock<IPathService>();
        var mockCommandService = new Mock<ICommandService>();

        mockPathService
            .Setup(service => service.GetDockerComposeFilePath())
            .Returns(@"C:\projects\DotNet\Robotics\docker-compose.yml");

        // Mock GetRunningContainers to return only "mssql" (meaning "redis" and "ntp-server" are stopped)
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker", "ps --format \"{{.Names}}\"", true))
            .ReturnsAsync(("mssql", ""));  // Simulating only "mssql" is running

        // Mock ExecuteCommandAsync for docker-compose services
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker-compose",
                $"-f \"{mockPathService.Object.GetDockerComposeFilePath()}\" config --services", true))
            .ReturnsAsync(("mssql\nredis\nntp-server", ""));  // Simulating defined services

        // Mock ExecuteCommandAsync for "docker-compose up --build"
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker-compose",
                It.Is<string>(cmd => cmd.Contains("up --build")), false))  // Matching any up command
            .ReturnsAsync(("Started redis, ntp-server", ""));  // Simulating docker-compose up success

        var dockerService = new DockerService(mockPathService.Object, mockCommandService.Object);

        // Act
        var result = await dockerService.RunDockerComposeUp();

        // Assert
        Assert.Equal("Started redis, ntp-server", result.Output);
        Assert.Equal("", result.Error);
    }

    [Fact]
    public async Task RunDockerComposeUp_NoRunningServices_StartsAllServices()
    {
        // Arrange
        var mockPathService = new Mock<IPathService>();
        var mockCommandService = new Mock<ICommandService>();

        mockPathService
            .Setup(service => service.GetDockerComposeFilePath())
            .Returns(@"C:\projects\DotNet\Robotics\docker-compose.yml");

        // Mock GetRunningContainers to return no running containers
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker", "ps --format \"{{.Names}}\"", true))
            .ReturnsAsync(("", ""));  // No running services

        // Mock ExecuteCommandAsync for docker-compose services
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker-compose",
                It.Is<string>(cmd => cmd.Contains("config --services")), true))
            .ReturnsAsync(("mssql\nredis\nntp-server", ""));  // Simulate a successful response with services

        // Mock RunDockerCommand to simulate starting all services
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker-compose",
                It.Is<string>(cmd => cmd.Contains("up --build mssql redis ntp-server")), false))
            .ReturnsAsync(("Started mssql, redis, ntp-server", ""));  // Simulate the service start

        var dockerService = new DockerService(mockPathService.Object, mockCommandService.Object);

        // Act
        var result = await dockerService.RunDockerComposeUp();

        // Assert
        Assert.Equal("Started mssql, redis, ntp-server", result.Output);
        Assert.Equal("", result.Error);
    }


    [Fact]
    public async Task RunDockerComposeUp_RetriesOnFailure()
    {
        // Arrange
        var mockPathService = new Mock<IPathService>();
        var mockCommandService = new Mock<ICommandService>();

        mockPathService
            .Setup(service => service.GetDockerComposeFilePath())
            .Returns(@"C:\projects\DotNet\Robotics\docker-compose.yml");

        var retryCount = 0;

        // Mock GetRunningContainers to return no running containers
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker", "ps --format \"{{.Names}}\"", true))
            .ReturnsAsync(("", ""));  // No running services

        // Mock ExecuteCommandAsync for docker-compose services
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker-compose",
                $"-f \"{mockPathService.Object.GetDockerComposeFilePath()}\" config --services", true))
            .ReturnsAsync(("mssql\nredis\nntp-server", ""));  // Defined services

        // Mock ExecuteCommandAsync for starting services, simulating retries
        mockCommandService
            .Setup(service => service.ExecuteCommandAsync("docker-compose",
                It.Is<string>(cmd => cmd.Contains("up --build mssql redis ntp-server")), false))
            .ReturnsAsync(() =>
            {
                retryCount++;
                if (retryCount < 3)
                {
                    throw new Exception("Temporary failure");  // Simulate failure
                }
                return ("Success", "");  // Simulate success after retries
            });

        var dockerService = new DockerService(mockPathService.Object, mockCommandService.Object);

        // Act
        var result = await dockerService.RunDockerComposeUp();

        // Assert
        Assert.Equal("Success", result.Output);  // Expect success after retries
        Assert.Equal(3, retryCount);  // Ensure it retried 3 times before succeeding

        // Verify that ExecuteCommandAsync was called 3 times
        mockCommandService.Verify(service => service.ExecuteCommandAsync("docker-compose",
            It.Is<string>(cmd => cmd.Contains("up --build mssql redis ntp-server")), false),
            Times.Exactly(3));
    }





}
