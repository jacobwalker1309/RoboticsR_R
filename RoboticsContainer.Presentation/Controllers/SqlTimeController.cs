using Microsoft.AspNetCore.Mvc;
using RoboticsContainer.Infrastructure.Services;

namespace RoboticsContainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SqlTimeController : ControllerBase
    {
        private readonly ISqlContainerTimeService _sqlContainerTimeService;

        public SqlTimeController(ISqlContainerTimeService sqlContainerTimeService)
        {
            _sqlContainerTimeService = sqlContainerTimeService;
        }

        // GET: api/SqlTime/SetTime
        [HttpGet("SetTime")]
        public async Task<IActionResult> SetSqlServerContainerTime()
        {
            await _sqlContainerTimeService.SetSqlServerContainerTime();
            return Ok(new { Message = "SQL Server container time updated successfully" });
        }
    }
}
