using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsContainer.Infrastructure.Extensions
{
    public static class ProcessExtensions
    {
        public static async Task<(string Output, string Error)> ExecuteCommandAsync(string fileName, string arguments, bool waitForExit = true)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = processStartInfo };
            var output = new StringBuilder();
            var error = new StringBuilder();

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    output.AppendLine(args.Data);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    error.AppendLine(args.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (waitForExit)
            {
                await process.WaitForExitAsync();
            }

            if (!string.IsNullOrEmpty(error.ToString()))
            {
                throw new Exception($"Command failed: {error.ToString()}");
            }

            return (output.ToString(), error.ToString());
        }
    }
}
