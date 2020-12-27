using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hadouken.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }
    }
}