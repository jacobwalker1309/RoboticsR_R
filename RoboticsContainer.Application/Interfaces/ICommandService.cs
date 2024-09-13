using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Application.Interfaces
{
    /// <summary>
    /// Wrapper for Extension Methods when testing
    /// </summary>
    public interface ICommandService
    {
        Task<(string Output, string Error)> ExecuteCommandAsync(string fileName, string arguments, bool waitForExit = true);
    }

}
