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
    [EnableCors("CorsPolicy")]
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

        [HttpGet("getUserById")]
        public IActionResult getUserById(string id)
        {
            Task<string?> user = _userLogic.GetUserById(id);
            if (user.Result == null)
            {
                return NotFound();
            }
            return Ok(user.Result);
        }

        [HttpPost("followUser")]
        public IActionResult followUser(string person1, string person2)
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
        public IActionResult getRequest(string person)
        {
            var i = _userLogic.GetRequests(person);
            return Ok(i.Result);
        }
        [HttpGet("getRecommendation")]
        public IActionResult GetRecommendation(string person)
        {
            var message = _userLogic.GetRecommendations(person);
            return Ok(message.Result);
        }

        [HttpDelete("DeleteRelation")]
        public IActionResult DeleteRelation(string person1, string person2)
        {
            if (person1 == person2)
            {
                return BadRequest("You can't delete a request from your self");
            }
            _userLogic.DeleteRelation(person1, person2);
            return Ok();
        }
        [HttpPost("AcceptRelation")]
        public IActionResult AcceptRelation(string person1, string person2)
        {
            if (person1 == person2)
            {
                return BadRequest("You can't accept a request to your self");
            }
            _userLogic.AcceptRelation(person1, person2);
            return Ok();
        }

        [HttpGet("getFollowingUsers")]
        public IActionResult GetFollowingUsers(string person)
        {
            var message = _userLogic.GetFollowingUsers(person);
            return Ok(message.Result);
        }

        [HttpGet("getAllUsers")]
        public IActionResult getAllUsers()
        {
            var i = _userLogic.GetAllUsers();
            return Ok(i.Result);
        }

        [HttpGet("dtos/emailandid/{substring}")]
        public IActionResult getAllEmailAndIdStartsWith(string substring)
        {
            var i = _userLogic.GetAllUsersEmailAndIdStartsWith(substring);
            return Ok(i.Result);
        }

    }
}
