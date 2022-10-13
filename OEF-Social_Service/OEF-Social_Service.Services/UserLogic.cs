using Neo4j.Driver;
using OEF_Social_Service.DataAccess.Data.Services.Interfaces;
using OEF_Social_Service.Models;
using OEF_Social_Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEF_Social_Service.Services
{
    public class UserLogic : IUserLogic
    {
        private readonly IFollowService _followService;

        public UserLogic(IFollowService followService)
        {
            _followService = followService; 
        }

        public void CreatePerson(Person person)
        {
            _followService.CreateUser(person);
        }

        public void FollowPerson(string person1, string person2)
        {
            _followService.SendRequest(person1, person2);
        }

        public Task<string> GetRequests(string person)
        {
                return _followService.GetRequests(person);
        }

        public void DeleteRelation(string person1, string person2)
        {
            _followService.DeleteRelation(person1, person2);
        }
    }
}
