using Amazon.Runtime.Internal.Transform;
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
        public Person _person;

        public FollowService(IDriver driver, ILogger<FollowService> logger, IOptions<ApplicationSettings> appSettingsOptions)
        {
            _database = appSettingsOptions.Value.Neo4jDatabase;
            _session = driver.AsyncSession(o => o.WithDatabase(_database));
        }

        public async Task createUser(Person person)
        {
            var statementText = new StringBuilder();
            statementText.Append("CREATE (n:Person {Id : $id, Firstname: $firstname, Lastname: $lastname, Username: $username, Email: $emailAddress, Password: $password, EnrollmentDate: $enrollmentDate, Role: $role, Institution: $institution, Theme: $theme, ResidencePlace: $residencePlace})");
            var statementParameters = new Dictionary<string, object>
            {
                {"id", person.Id.ToString()},
                {"firstname", person.FirstName},
                {"lastname", person.LastName},
                {"username", person.Username},
                {"emailAddress", person.EmailAddress},
                {"password", person.Password},
                {"enrollmentDate", person.EnrollmentDate.ToShortDateString()},
                {"role", person.Role},
                {"institution", person.Institution},
                {"theme", person.Theme},
                {"residencePlace", person.ResidencePlace}
            };

            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
                Console.Write(query);
                
            }
        }

        public async Task sendRequest(string person1, string person2)
        {
            var statementText = new StringBuilder();
            statementText.Append("MATCH (p1:Person), (p2:Person) WHERE p1.Firstname = $firstname AND p2.Firstname = $firstname2 CREATE (p1)-[p:Request_Send] ->(p2)");
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

        public async Task<List<Person>> GetRequest(string person)
        {
            var data = new List<Person>();
            var statementText = new StringBuilder();
            statementText.Append("MATCH (:Person {Firstname: $firstname})--(person:Person) RETURN person");
            var statementParameters = new Dictionary<string, object>
            {
                {"firstname", person},
            };
            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
                var result = await query.ToListAsync();
                foreach (var item in result)
                {
                    Console.WriteLine(item.Values);
                }
                Console.WriteLine(result);
                return null;
            }
        }

        public async Task<List<Person>> ExecuteReadListAsync(string person)
        {
            var statementText = new StringBuilder();
            statementText.Append("MATCH (:Person {Firstname: $firstname})--(person:Person) RETURN person");
            var statementParameters = new Dictionary<string, object>
            {
                {"firstname", person},
            };

            await _session.ExecuteReadAsync(async tx =>
            {
                var data = new List<Person>();
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
                var records = await query.ToListAsync();

                data = records.Select(x => (Person)x.Values).ToList();
                Console.WriteLine(data);
                return data;
            });
            return null;
        }
    }
}
