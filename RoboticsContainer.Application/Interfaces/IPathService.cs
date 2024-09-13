using System;
using System.Collections.Generic;
namespace RoboticsContainer.Application.Interfaces
{
    /// <summary>
    /// Used to get path roots for docker compose - docker commands will be done in code for granularity
    /// </summary>
    public interface IPathService
        {
            string GetProjectRoot();
            string GetDockerComposeFilePath();
            string GetFirewallRulesFilePath();
    }
}

