namespace RoboticsContainer.Application.Interfaces
{
    public interface INtpTimeService
    {
        Task<DateTime> GetNtpTimeAsync(string ntpServer = "ntp-server");
    }
}
