using Microsoft.AspNetCore.Mvc;
using OEF_Social_Service.Models;
using OEF_Social_Service.Services.Interfaces;
using System.Net;

namespace OEF_Social_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly ITestLogic _testLogic;

        public PersonController(ILogger<PersonController> logger, ITestLogic testLogic)
        {
            _logger = logger;
            _testLogic = testLogic;
        }

        [HttpPost("create")]
        public IActionResult Create(string message)
        {
            try
            {
                _testLogic.writeHello(message);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }
        //[HttpPost("get")]
        //public IActionResult GetUser(string name)
        //{
        //    try
        //    {
        //        _testLogic.SearchPersonsByName(name);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new { message = e.Message });
        //    }
        //    return Ok();
        //}
    }
}
