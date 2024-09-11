using Polly;
using RoboticsContainer.Application.Interfaces;

public class DockerService : IDockerService
{
    private readonly string _dockerComposeFilePath;
    private readonly IAsyncPolicy<(string Output, string Error)> _retryPolicy;
    private readonly ICommandService _commandService;

    public DockerService(IPathService pathService, ICommandService commandService)
    {
        _dockerComposeFilePath = pathService.GetDockerComposeFilePath();
        _commandService = commandService;

        // Define a simple retry policy with Polly (retry 3 times with a 2-second delay)
        _retryPolicy = Policy<(string Output, string Error)>
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                (result, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} for Docker command.");
                });
    }

    public virtual async Task<(string Output, string Error)> RunDockerComposeUp()
    {
        try
        {
            Console.WriteLine("Checking for running containers...");
            var runningContainers = await GetRunningContainers();
            Console.WriteLine($"Running Containers: {string.Join(", ", runningContainers)}");

            var (servicesOutput, error) = await _commandService.ExecuteCommandAsync(
                "docker-compose",
                $"-f \"{_dockerComposeFilePath}\" config --services"
            );

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception($"Failed to get docker-compose services: {error}");
            }

            var definedServices = servicesOutput
            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(service => service.Trim())  // Ensure no extra spaces
            .ToList();
            var stoppedServices = definedServices.Except(runningContainers).ToList();

            if (!stoppedServices.Any())
            {
                Console.WriteLine("All services are already running.");
                return ("All services are already running.", "");
            }

            var servicesArg = string.Join(" ", stoppedServices);

            // Wrapping the `docker-compose up` command in Polly retry logic
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var (upOutput, upError) = await _commandService.ExecuteCommandAsync(
                    "docker-compose",
                    $"up --build {servicesArg}",
                    waitForExit: false
                );

                Console.WriteLine($"Up Output: {upOutput}, Up Error: {upError}");

                if (!string.IsNullOrEmpty(upError))
                {
                    throw new Exception($"Docker Compose up command failed: {upError}");
                }

                return (upOutput, upError);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return (null, $"Exception: {ex.Message}");
        }
    }

    public virtual async Task<(string Output, string Error)> RunDockerComposeDown()
    {
        Console.WriteLine("Running docker-compose down...");
        return await _commandService.ExecuteCommandAsync(
            "docker-compose",
            "down",
            waitForExit: true
        );
    }

    public virtual async Task<List<string>> GetRunningContainers()
    {
        var (output, error) = await _commandService.ExecuteCommandAsync(
            "docker",
            "ps --format \"{{.Names}}\""
        );

        if (!string.IsNullOrEmpty(error))
        {
            throw new Exception($"Failed to get running containers: {error}");
        }

        return output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
