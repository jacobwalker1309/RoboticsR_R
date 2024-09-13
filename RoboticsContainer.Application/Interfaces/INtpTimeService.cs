namespace RoboticsContainer.Application.Interfaces
{
    /// <summary>
    /// Uses the Ntpcontainer to get current time used in conjunction with SqlContainerTimeService to set the SqlContainer Db time
    /// </summary>
    public interface INtpTimeService
    {
        Task<DateTime> GetNtpTimeAsync(string ntpServer = "ntp-server");
    }
}
