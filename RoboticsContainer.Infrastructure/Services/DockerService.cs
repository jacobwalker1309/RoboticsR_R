using RoboticsContainer.Application.Interfaces;
using System.Diagnostics;
using System.Text;
using Polly;
using RoboticsContainer.Infrastructure.Extensions;

namespace RoboticsContainer.Infrastructure.Services
{
    public class DockerService : IDockerService
    {
        private readonly string _dockerComposeFilePath;
        private readonly IAsyncPolicy<(string Output, string Error)> _retryPolicy;

        public DockerService(IPathService pathService)
        {
            _dockerComposeFilePath = pathService.GetDockerComposeFilePath();
            Console.WriteLine("Beginning of Docker Service Process");

            // Define a simple retry policy with Polly (retry 3 times with a 2-second delay)
            _retryPolicy = Policy<(string Output, string Error)>
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                    (result, timeSpan, retryCount, context) =>
                    {
                        // Log each retry
                        Console.WriteLine($"Retry {retryCount} for Docker command.");
                    });
        }

        public virtual async Task<(string Output, string Error)> RunDockerComposeUp()
        {
            Console.WriteLine("Checking for running containers...");
            var runningContainers = await GetRunningContainers();
            var definedServices = await GetDockerComposeServices();

            var stoppedServices = definedServices.Except(runningContainers).ToList();

            if (!stoppedServices.Any())
            {
                Console.WriteLine("All services are already running.");
                return ("All services are already running.", "");
            }

            Console.WriteLine($"Starting the following services: {string.Join(", ", stoppedServices)}");
            var servicesArg = string.Join(" ", stoppedServices);

            // Do not wait for the process to exit because docker-compose up is long-running

            return await _retryPolicy.ExecuteAsync(async () =>
            {

                return await RunDockerCommand($"up --build {servicesArg}", waitForExit: false);

            });
        }

        public virtual async Task<(string Output, string Error)> RunDockerComposeDown()
        {
            Console.WriteLine("Running docker-compose down...");
            // Wait for the process to exit since docker-compose down should finish
            return await RunDockerCommand("down", waitForExit: true);
        }

        // Check which containers are running
        public virtual async Task<List<string>> GetRunningContainers()
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "ps --format \"{{.Names}}\"", // Get the names of running containers
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = processStartInfo
            };

            var output = new StringBuilder();

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    output.AppendLine(args.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();

            return output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        // Get the services defined in docker-compose.yml
        public virtual async Task<List<string>> GetDockerComposeServices()
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                Arguments = $"-f \"{_dockerComposeFilePath}\" config --services", // List services in docker-compose.yml
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = processStartInfo
            };

            var output = new StringBuilder();

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    output.AppendLine(args.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();

            return output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public virtual async Task<(string Output, string Error)> RunDockerCommand(string command, bool waitForExit = true)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                Console.WriteLine($"Executing Docker Command: {command}");

                // Use the extension method to execute the docker command
                return await ProcessExtensions.ExecuteCommandAsync("docker-compose", $"-f \"{_dockerComposeFilePath}\" {command}", waitForExit);
            });
        }
    }
}
