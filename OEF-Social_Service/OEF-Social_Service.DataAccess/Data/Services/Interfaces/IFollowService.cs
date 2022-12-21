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
        Task SendRequest(Guid person1, Guid person2);
        Task<string> GetRequests(Guid person);
        Task<string> GetRelatedUsers(Guid person);
        Task DeleteRelation(Guid person1, Guid person2);
        Task AcceptRelation(Guid person1, Guid person2);
        Task<string> GetAllUsers(string firstname);
    }
}
