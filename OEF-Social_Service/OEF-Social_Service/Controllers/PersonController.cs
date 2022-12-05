using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OEF_Social_Service.Models;
using OEF_Social_Service.Services;
using OEF_Social_Service.Services.Interfaces;
using System.Net;

namespace OEF_Social_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("1.0")]
    [EnableCors]
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
        [HttpGet("getUser")]
        public IActionResult getUser(string username)
        {
            var i = _userLogic.GetUser(username);
            return Ok(i.Result);
        }

        [HttpPost("followUser")]
        public IActionResult followUser(Guid person1, Guid person2)
        {
            try
            {
                if (person1 == person2)
                {
                    return BadRequest("You can't send a request to your self");
                }
                _userLogic.FollowPerson(person1, person2);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }
        [HttpGet("getRequest")]
        public IActionResult getRequest(Guid person)
        {
                var i = _userLogic.GetRequests(person);
                return Ok(i.Result);
        }
        [HttpGet("getRecommendation")]
        public IActionResult GetRecommendation(Guid person)
        {
            var message = _userLogic.GetRecommendations(person);
            return Ok(message.Result);
        }

        [HttpDelete("DeleteRelation")]
        public IActionResult DeleteRelation(Guid person1, Guid person2)
        {
            if (person1 == person2)
            {
                return BadRequest("You can't delete a request from your self");
            }
            _userLogic.DeleteRelation(person1, person2);
            return Ok();
        }
        [HttpPost("AcceptRelation")]
        public IActionResult AcceptRelation(Guid person1, Guid person2)
        {
            if (person1 == person2)
            {
                return BadRequest("You can't accept a request to your self");
            }
            _userLogic.AcceptRelation(person1, person2);
            return Ok();
        }

    }
}
