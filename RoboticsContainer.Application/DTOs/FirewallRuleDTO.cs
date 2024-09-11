using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Application.DTOs
{
    public class FirewallRuleDTO
    {
        public string RuleName { get; set; }
        public string SourceIP { get; set; }
        public string DestinationIP { get; set; }
        public int Port { get; set; }
        public string Protocol { get; set; }
        public bool Allow { get; set; }
        public string RuleDescription { get; set; }
    }
}
