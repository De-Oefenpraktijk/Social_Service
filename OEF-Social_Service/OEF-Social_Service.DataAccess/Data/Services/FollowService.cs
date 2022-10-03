using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using OEF_Social_Service.Composition;
using OEF_Social_Service.DataAccess.Data.Services.Interfaces;
using OEF_Social_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEF_Social_Service.DataAccess.Data.Services
{
    public class FollowService : IFollowService
    {
        private IAsyncSession _session;
        private string _database;
        private readonly IDriver _driver;

        public FollowService(IDriver driver, ILogger<FollowService> logger, IOptions<ApplicationSettings> appSettingsOptions)
        {
            _database = appSettingsOptions.Value.Neo4jDatabase;
            _session = driver.AsyncSession(o => o.WithDatabase(_database));
        }

        public async Task createUser(Person person)
        {
            var statementText = new StringBuilder();
            statementText.Append("CREATE (n:Person {firstname: $firstname, lastname: $lastname, education: $education})");
            var statementParameters = new Dictionary<string, object>
            {
                {"firstname", person.firstname},
                {"lastname", person.lastname},
                {"education", person.education}
            };

            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
            }
        }

        public async Task followUser(string person1, string person2)
        {
            var statementText = new StringBuilder();
            statementText.Append("MATCH (p1:Person), (p2:Person) WHERE p1.firstname = $firstname AND p2.firstname = $firstname2 CREATE (p1)-[p:Follows] ->(p2)");
            var statementParameters = new Dictionary<string, object>
            {
                {"firstname", person1},
                {"firstname2", person2}
            };
            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
            }
        }
    }
}
