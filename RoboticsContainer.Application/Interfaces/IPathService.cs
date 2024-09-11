using System;
using System.Collections.Generic;
namespace RoboticsContainer.Application.Interfaces
{
       public interface IPathService
        {
            string GetProjectRoot();
            string GetDockerComposeFilePath();
            string GetFirewallRulesFilePath();
    }
}

