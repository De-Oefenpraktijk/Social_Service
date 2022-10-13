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

        [HttpPost("createUser")]
        public IActionResult CreateUserNode(Person person)
        {
            try
            {
                _testLogic.CreatePerson(person);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }

        [HttpPost("followUser")]
        public IActionResult followUser(string person1, string person2)
        {
            try
            {
                _testLogic.followPerson(person1, person2);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }
        [HttpGet("getRequest")]
        public IActionResult getRequest(string person)
        {
                var i = _testLogic.getRequests(person);
                return Ok(i.Result);
        }

        [HttpDelete("getRequest")]
        public IActionResult deleteFollower(string person1, string person2)
        {
            _testLogic.deletePerson(person1, person2);
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
