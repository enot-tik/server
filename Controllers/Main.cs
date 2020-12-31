using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace server.Controllers
{
    class Connect
    {
        public string ip { get; set; }
        public int id { get; set; }
        public string token { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class Main : ControllerBase
    {
        private readonly ILogger<Main> _logger;
        // private List<>

        public Main(ILogger<Main> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ActionName("get")]
        public string Get()
        {
            return Request.Host.Value;
        }

    }
}
