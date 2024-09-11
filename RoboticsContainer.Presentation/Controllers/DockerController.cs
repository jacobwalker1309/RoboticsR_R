using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RoboticsContainer.Infrastructure.Services;
using RoboticsContainer.Application.Interfaces;

namespace RoboticsContainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DockerController : ControllerBase
    {
        private readonly IDockerService _dockerService;

        public DockerController(IDockerService dockerService)
        {
            _dockerService = dockerService;
        }

        [HttpPost("run")]
        public async Task<IActionResult> RunDockerCompose()
        {
            var result = await _dockerService.RunDockerComposeUp();
            return Ok(new { Output = result.Output, Error = result.Error });
        }

        [HttpPost("stop")]
        public async Task<IActionResult> StopDockerCompose()
        {
            var result = await _dockerService.RunDockerComposeDown();
            return Ok(result);
        }
    }
}
