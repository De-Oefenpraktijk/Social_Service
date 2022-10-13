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
        Task createUser(Person person);
        Task sendRequest(string person1, string person2);
        Task<string> GetRequests(string person);
        Task<List<Person>> ExecuteReadListAsync(string person);
        Task deleteFollower(string person1, string person2);
    }
}
