using Neo4j.Driver;
using OEF_Social_Service.DataAccess.Data.Services;
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

        public void FollowPerson(Guid person1, Guid person2)
        {
            try
            {
                _followService.SendRequest(person1, person2);

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Task<string> GetRequests(Guid person)
        {
                return _followService.GetRequests(person);
        }

        public void DeleteRelation(Guid person1, Guid person2)
        {
            _followService.DeleteRelation(person1, person2);
        }
        public void AcceptRelation(Guid person1, Guid person2)
        {
            _followService.AcceptRelation(person1, person2);
        }

        public Task<string> GetRecommendations(Guid person)
        {
            try
            {
                var relatedUsers = _followService.GetRelatedUsers(person);
                return (relatedUsers);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
