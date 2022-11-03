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
        private readonly IUserLogic _userLogic;

        public PersonController(ILogger<PersonController> logger, IUserLogic userLogic)
        {
            _logger = logger;
            _userLogic = userLogic;
        }

        [HttpPost("createUser")]
        public IActionResult CreateUserNode(Person person)
        {
            try
            {
                _userLogic.CreatePerson(person);
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
                _userLogic.FollowPerson(person1, person2);
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
                var i = _userLogic.GetRequests(person);
                return Ok(i.Result);
        }

        [HttpDelete("DeleteRelation")]
        public IActionResult DeleteRelation(string person1, string person2)
        {
            _userLogic.DeleteRelation(person1, person2);
            return Ok();
        }
        [HttpPost("AcceptRelation")]
        public IActionResult AcceptRelation(string person1, string person2)
        {
            _userLogic.AcceptRelation(person1, person2);
            return Ok();
        }
    }
}
