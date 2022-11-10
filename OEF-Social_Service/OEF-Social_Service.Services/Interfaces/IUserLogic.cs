using Neo4j.Driver;
using OEF_Social_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEF_Social_Service.Services.Interfaces
{
    public interface IUserLogic
    {
        void CreatePerson(Person person);
        void FollowPerson(Guid person1, Guid person2);
        Task<string> GetRequests(Guid person);
        Task<string> GetRecommendations(Guid person);
        void DeleteRelation(Guid person1, Guid person2);
        void AcceptRelation(Guid person1, Guid person2);
    }
}
