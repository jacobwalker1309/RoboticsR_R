using System.Threading.Tasks;

namespace RoboticsContainer.Infrastructure.Services
{
    public interface ISqlContainerTimeService
    {
        /// <summary>
        /// Sets the time in the SQL Server container to the current NTP time.
        /// </summary>
        Task SetSqlServerContainerTime();
    }
}
