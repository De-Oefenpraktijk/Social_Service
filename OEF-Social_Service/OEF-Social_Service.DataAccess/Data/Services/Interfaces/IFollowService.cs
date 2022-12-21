using Neo4j.Driver;
using OEF_Social_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEF_Social_Service.DataAccess.Data.Services.Interfaces
{
    public interface IFollowService
    {
        Task CreateUser(Person person);
        Task<string> GetUser(string username);
        Task UpdateUser(Person person);
        Task SendRequest(string person1, string person2);
        Task<string> GetRequests(string person);
        Task<string> GetRelatedUsers(string person);
        Task DeleteRelation(string person1, string person2);
        Task AcceptRelation(string person1, string person2);
        Task<string> GetFollowingUsers(string person);
        Task<string> GetAllUsers();
    }
}
