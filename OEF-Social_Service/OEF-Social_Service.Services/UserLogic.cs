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
        private readonly DataAccess.Data.Services.Interfaces.IFollowService _followService;

        public UserLogic(DataAccess.Data.Services.Interfaces.IFollowService followService)
        {
            _followService = followService;
        }

        public void CreatePerson(Person person)
        {
            _followService.CreateUser(person);
        }
        public Task<string> GetUser(string username)
        {
            return _followService.GetUser(username);
        }
        public Task<string?> GetUserById(string id)
        {
            Task<string?> user = _followService.GetUserById(id);
            return user;
        }

        public void UpdatePerson(Person person)
        {
            _followService.UpdateUser(person);
        }

        public void FollowPerson(string person1, string person2)
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

        public Task<string> GetRequests(string person)
        {
            return _followService.GetRequests(person);
        }

        public void DeleteRelation(string person1, string person2)
        {
            _followService.DeleteRelation(person1, person2);
        }
        public void AcceptRelation(string person1, string person2)
        {
            _followService.AcceptRelation(person1, person2);
        }

        public Task<string> GetRecommendations(string person)
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

        public Task<string> GetFollowingUsers(string person)
        {
            try
            {
                var relatedUsers = _followService.GetFollowingUsers(person);
                return (relatedUsers);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public Task<string> GetAllUsers()
        {
            try
            {
                var allUsers = _followService.GetAllUsers();
                return (allUsers);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Task<string> GetAllUsersEmailAndIdStartsWith(string substring)
        {
            try
            {
                var allUsers = _followService.GetAllUsersEmailAndIdStartsWith(substring);
                return (allUsers);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
