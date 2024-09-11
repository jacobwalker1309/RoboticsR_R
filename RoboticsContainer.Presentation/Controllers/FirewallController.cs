using Microsoft.AspNetCore.Mvc;
using RoboticsContainer.Application.Interfaces;

namespace RoboticsContainer.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirewallController : ControllerBase
    {
        private readonly IFirewallService _firewallService;

        public FirewallController(IFirewallService firewallService)
        {
            _firewallService = firewallService;
        }

        [HttpGet("rules")]
        public IActionResult GetFirewallRules()
        {
            var rules = _firewallService.GetRules();
            return Ok(rules);
        }

        [HttpPost("apply-rules")]
        public async Task<IActionResult> ApplyFirewallRules()
        {
            await _firewallService.CheckAndAddAllFirewallRulesAsync();
            return Ok("Firewall rules have been checked and applied where necessary.");
        }
    }
}
