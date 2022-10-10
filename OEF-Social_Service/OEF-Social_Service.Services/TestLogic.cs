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
    public class testLogic : ITestLogic
    {
        private readonly IFollowService _followService;

        public testLogic(IFollowService followService)
        {
            _followService = followService; 
        }

        public void CreatePerson(Person person)
        {
            _followService.createUser(person);
        }

        public void followPerson(string person1, string person2)
        {
            _followService.sendRequest(person1, person2);
        }

        public Task<string> getRequest(string person)
        {
                return _followService.GetRequest(person);
        }

        //public async Task<List<Dictionary<string, object>>> SearchPersonsByName(string searchString)
        //{
        //    var query = @"MATCH (p:Person) WHERE toUpper(p.name) CONTAINS toUpper($searchString) 
        //                        RETURN p{ name: p.name, born: p.born } ORDER BY p.Name LIMIT 5";

        //    IDictionary<string, object> parameters = new Dictionary<string, object> { { "searchString", searchString } };

        //    var persons = await _followService.ExecuteReadDictionaryAsync(query, "p", parameters);

        //    return persons;
        //}
    }
}
