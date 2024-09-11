using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Infrastructure.Configuration;
using RoboticsContainer.Infrastructure.Extensions;

namespace RoboticsContainer.Infrastructure.Services
{
    public class SqlContainerTimeService : ISqlContainerTimeService
    {
        private readonly NtpTimeService _ntpTimeService;
        private readonly ICommandService _commandService;

        public SqlContainerTimeService(NtpTimeService ntpTimeService, ICommandService commandService)
        {
            _ntpTimeService = ntpTimeService;
            _commandService = commandService;
        }

        public async Task SetSqlServerContainerTime()
        {
            // Step 1: Get the current NTP time
            DateTime ntpTime = await _ntpTimeService.GetNtpTimeAsync();

            // Step 2: Format the time for the `date` command (e.g., "11:03:00")
            string timeString = ntpTime.ToString("HH:mm:ss");

            // Step 3: Execute the command to set the container's time using the ProcessExtensions method
            string command = $"date +%T -s \"{timeString}\"";
            await RunCommandAsync(AppConstants.SQL_SERVER_CONTAINER_NAME, command);

            Console.WriteLine($"Time in SQL Server container '{AppConstants.SQL_SERVER_CONTAINER_NAME}' set to {ntpTime}");
        }

        private async Task RunCommandAsync(string containerName, string command)
        {
            // Format the full Docker exec command
            string fullCommand = $"exec -it {containerName} bash -c \"{command}\"";

            // Use the ICommandExecutor to run the command
            var result = await _commandService.ExecuteCommandAsync("docker", fullCommand);

            // Output the result
            Console.WriteLine(result.Output);

            if (!string.IsNullOrEmpty(result.Error))
            {
                throw new Exception($"Error executing command in container {containerName}: {result.Error}");
            }
        }
    }
}
