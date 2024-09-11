using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Application.Interfaces
{
    public interface ICommandService
    {
        Task<(string Output, string Error)> ExecuteCommandAsync(string fileName, string arguments, bool waitForExit = true);
    }

}
