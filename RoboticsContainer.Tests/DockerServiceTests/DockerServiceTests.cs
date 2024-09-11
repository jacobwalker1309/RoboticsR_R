using Moq;
using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Infrastructure.Services;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DockerServiceTests
{
    [Fact]
    public async Task RunDockerComposeUp_AllServicesAlreadyRunning_ReturnsMessage()
    {
        // Arrange
        var mockPathService = new Mock<IPathService>();
        mockPathService
            .Setup(service => service.GetDockerComposeFilePath())
            .Returns(@"C:\projects\DotNet\Robotics\docker-compose.yml");

        var mockDockerService = new Mock<DockerService>(mockPathService.Object)
        {
            CallBase = true // Use the real implementation for non-mocked methods
        };

        // Mock GetRunningContainers to return all containers
        mockDockerService
            .Setup(service => service.GetRunningContainers())
            .ReturnsAsync(new List<string> { "mssql", "redis", "ntp-server" });

        // Mock GetDockerComposeServices to return the defined services
        mockDockerService
            .Setup(service => service.GetDockerComposeServices())
            .ReturnsAsync(new List<string> { "mssql", "redis", "ntp-server" });

        // Act
        var result = await mockDockerService.Object.RunDockerComposeUp();

        // Assert
        Assert.Equal("All services are already running.", result.Output);
        Assert.Equal("", result.Error);
    }

    [Fact]
    public async Task RunDockerComposeUp_SomeServicesDown_StartsOnlyStoppedServices()
    {
        // Arrange
        var mockPathService = new Mock<IPathService>();
        mockPathService
            .Setup(service => service.GetDockerComposeFilePath())
            .Returns(@"C:\projects\DotNet\Robotics\docker-compose.yml");

        var mockDockerService = new Mock<DockerService>(mockPathService.Object)
        {
            CallBase = true // Use the real implementation for non-mocked methods
        };

        // Mock GetRunningContainers to return only some running containers
        mockDockerService
            .Setup(service => service.GetRunningContainers())
            .ReturnsAsync(new List<string> { "mssql" });

        // Mock GetDockerComposeServices to return the defined services
        mockDockerService
            .Setup(service => service.GetDockerComposeServices())
            .ReturnsAsync(new List<string> { "mssql", "redis", "ntp-server" });

        // Mock RunDockerCommand to simulate starting services
        mockDockerService
            .Setup(service => service.RunDockerCommand(It.IsAny<string>(), false))
            .ReturnsAsync(("Started redis, ntp-server", ""));

        // Act
        var result = await mockDockerService.Object.RunDockerComposeUp();

        // Assert
        Assert.Equal("Started redis, ntp-server", result.Output);
        Assert.Equal("", result.Error);
    }

    [Fact]
    public async Task RunDockerComposeUp_NoRunningServices_StartsAllServices()
    {
        // Arrange
        var mockPathService = new Mock<IPathService>();
        mockPathService
            .Setup(service => service.GetDockerComposeFilePath())
            .Returns(@"C:\projects\DotNet\Robotics\docker-compose.yml");

        var mockDockerService = new Mock<DockerService>(mockPathService.Object)
        {
            CallBase = true // Use the real implementation for non-mocked methods
        };

        // Mock GetRunningContainers to return no running containers
        mockDockerService
            .Setup(service => service.GetRunningContainers())
            .ReturnsAsync(new List<string>());

        // Mock GetDockerComposeServices to return the defined services
        mockDockerService
            .Setup(service => service.GetDockerComposeServices())
            .ReturnsAsync(new List<string> { "mssql", "redis", "ntp-server" });

        // Mock RunDockerCommand to simulate starting all services
        mockDockerService
            .Setup(service => service.RunDockerCommand(It.IsAny<string>(), false))
            .ReturnsAsync(("Started mssql, redis, ntp-server", ""));

        // Act
        var result = await mockDockerService.Object.RunDockerComposeUp();

        // Assert
        Assert.Equal("Started mssql, redis, ntp-server", result.Output);
        Assert.Equal("", result.Error);
    }

    [Fact]
    public async Task RunDockerComposeUp_RetriesOnFailure()
    {
        // Arrange
        var mockPathService = new Mock<IPathService>();
        mockPathService
            .Setup(service => service.GetDockerComposeFilePath())
            .Returns(@"C:\projects\DotNet\Robotics\docker-compose.yml");

        var retryCount = 0;

        // Mock DockerService and use CallBase to ensure non-overridden methods use the base class implementation
        var mockDockerService = new Mock<DockerService>(mockPathService.Object)
        {
            CallBase = true // Ensure the base methods are called for non-mocked methods
        };

        // Mock GetRunningContainers to simulate that no services are running
        mockDockerService
            .Setup(service => service.GetRunningContainers())
            .ReturnsAsync(new List<string>());  // No running services

        // Mock GetDockerComposeServices to return all defined services
        mockDockerService
            .Setup(service => service.GetDockerComposeServices())
            .ReturnsAsync(new List<string> { "mssql", "redis", "ntp-server" });  // Define 3 services

        // Mock the RunDockerCommand method to simulate failures and eventual success
        mockDockerService
            .Setup(service => service.RunDockerCommand(It.IsAny<string>(), false))
            .ReturnsAsync(() =>
            {
                retryCount++;
                if (retryCount < 3)
                {
                    throw new Exception("Temporary failure");  // Throwing exception to trigger Polly
                }
                return ("Success", "");  // Simulate success after retries
            });

        // Act

        (string Output, string Error) result = (string.Empty, string.Empty);

        try
        {
            result = await mockDockerService.Object.RunDockerComposeUp();
        }
        catch (Exception ex)
        {
            Assert.True(false, $"Test failed with exception: {ex.Message}");
        }

        // Assert
        Assert.Equal("Success", result.Output);  // Expect success after retries
        Assert.Equal(3, retryCount);  // Ensure it retried 3 times before succeeding

        // Verify that RunDockerCommand was called the expected number of times
        mockDockerService.Verify(service => service.RunDockerCommand(It.IsAny<string>(), false), Times.Exactly(3));
    }

}
