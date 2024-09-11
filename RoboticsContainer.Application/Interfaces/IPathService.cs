using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Application.Interfaces
{
       public interface IPathService
        {
            string GetProjectRoot();
            string GetDockerComposeFilePath();
            string GetFirewallRulesFilePath();
    }
}

