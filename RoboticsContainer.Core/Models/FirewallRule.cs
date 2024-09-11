using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Core.Models
{
    public class FirewallRule
    {
        public Guid Id { get; set; }
        public string RuleName { get; set; }
        public string RuleDescription { get; set; }
        public string Action { get; set; } // Allow, Deny, etc.
        public string Protocol { get; set; }
        public string Port { get; set; }
        public string IPAddress { get; set; }
    }
}
