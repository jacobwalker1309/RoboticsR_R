using RoboticsContainer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Application.Interfaces
{
    public interface IFirewallService
    {
        IEnumerable<FirewallRule> GetRules();

        Task<bool> FirewallRuleExistsAsync(string ruleName);

        Task CheckAndAddAllFirewallRulesAsync();  // Add this method to the interface
    }
}
