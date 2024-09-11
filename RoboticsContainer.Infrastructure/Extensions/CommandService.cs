using RoboticsContainer.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Infrastructure.Extensions
{
    public class CommandService : ICommandService
    {
        public async Task<(string Output, string Error)> ExecuteCommandAsync(string fileName, string arguments, bool waitForExit = true)
        {
            return await ProcessExtensions.ExecuteCommandAsync(fileName, arguments, waitForExit);
        }
    }

}
