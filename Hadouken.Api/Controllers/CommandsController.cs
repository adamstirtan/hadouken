using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Hadouken.Dto;

namespace Hadouken.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ILogger<CommandsController> _logger;

        public CommandsController(ILogger<CommandsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("sendmessage")]
        public Task<IActionResult> SendMessage(SendMessageRequestDto dto)
        {
            _logger.LogInformation("Received SendMessage request");

            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("speak")]
        public Task<IActionResult> SpeakMessage(SpeakMessageRequestDto dto)
        {
            _logger.LogInformation("Received SpeakMessage request");

            throw new NotImplementedException();
        }
    }
}