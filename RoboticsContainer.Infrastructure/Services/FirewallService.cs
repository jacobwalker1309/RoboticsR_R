using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Core.Models;
using RoboticsContainer.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RoboticsContainer.Infrastructure.Services
{
    public class FirewallService : IFirewallService
    {
        private readonly List<FirewallRule> _predefinedRules;
        private readonly IPathService _pathService;

        public FirewallService(IPathService pathService)
        {
            _pathService = pathService;
            _predefinedRules = LoadFirewallRulesFromFile();
        }

        private List<FirewallRule> LoadFirewallRulesFromFile()
        {
            var filePath = _pathService.GetFirewallRulesFilePath();
            Console.WriteLine($"Project Root: {_pathService.GetProjectRoot()}");
            Console.WriteLine($"Full Path: {filePath}");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Firewall rules file not found.");
            }

            string jsonData = File.ReadAllText(filePath);
            var firewallRules = JsonSerializer.Deserialize<List<FirewallRule>>(jsonData);
            return firewallRules;
        }

        public IEnumerable<FirewallRule> GetRules()
        {
            return _predefinedRules;
        }

        public async Task CheckAndAddAllFirewallRulesAsync()
        {
            foreach (var rule in _predefinedRules)
            {
                string ruleName = rule.RuleName;

                // Ensure DisplayName is quoted properly, and the entire command is valid
                string checkCommand = $"Get-NetFirewallRule -DisplayName \"{ruleName}\"";
                string addCommand = $"New-NetFirewallRule -DisplayName \"{ruleName}\" -Direction Inbound -LocalPort {rule.Port} -Protocol {rule.Protocol} -Action Allow";

                // Print out the command to ensure correctness for debugging
                Console.WriteLine($"Check Command: {checkCommand}");
                Console.WriteLine($"Add Command: {addCommand}");

                if (!await FirewallRuleExistsAsync(ruleName))
                {
                    await ExecutePowerShellCommandWithScriptBlockAsync(ruleName, rule.Port, rule.Protocol);
                }
                else
                {
                    Console.WriteLine($"{ruleName} already exists.");
                }
            }
        }


        public async Task<bool> FirewallRuleExistsAsync(string ruleName)
        {
            // Ensure the rule name is properly quoted in the PowerShell command
            var command = $"Get-NetFirewallRule -DisplayName \"{ruleName}\"";

            try
            {
                var result = await ProcessExtensions.ExecuteCommandAsync("powershell.exe", $"-Command \"{command}\"", waitForExit: true);
                return !string.IsNullOrEmpty(result.Output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while checking firewall rule: {ex.Message}");
                return false;  // Rule doesn't exist or some other issue
            }
        }

        private async Task<(string Output, string Error)> ExecutePowerShellCommandWithScriptBlockAsync(string ruleName, string port, string protocol)
        {
            // Define a script block for PowerShell execution
            string scriptBlock = $@"
        New-NetFirewallRule -DisplayName '{ruleName}' -Direction Inbound -LocalPort {port} -Protocol {protocol} -Action Allow
    ";

            // Pass the script block to PowerShell
            var arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{scriptBlock}\"";

            // Execute the PowerShell command
            return await ProcessExtensions.ExecuteCommandAsync("powershell.exe", arguments, waitForExit: true);
        }
    }
}
