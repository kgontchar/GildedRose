using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok("Home Controller");
        }
    }
}
