using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using OEF_Social_Service.Composition;
using OEF_Social_Service.DataAccess.Data.Services.Interfaces;
using OEF_Social_Service.Models;
using System.Text;
using System.Text.Json;

namespace OEF_Social_Service.DataAccess.Data.Services
{
    public class FollowService : IFollowService
    {
        private IAsyncSession _session;
        private string _database;
        public Person _person;

        public FollowService(IDriver driver, ILogger<FollowService> logger, IOptions<ApplicationSettings> appSettingsOptions)
        {
            _database = appSettingsOptions.Value.Neo4jDatabase;
            _session = driver.AsyncSession(o => o.WithDatabase(_database));
        }

        public async Task CreateUser(Person person)
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

        public async Task SendRequest(string person1, string person2)
        {
            var i = DoesRelationExist(person1, person2).Result;
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

        public async Task<string> GetRequests(string person)
        {
            var data = new List<Person>();
            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person)-[r]->({Firstname: $firstname}) Where Not ({Firstname: $firstname})-[]->(user) Return user");
            var statementParameters = new Dictionary<string, object>
            {
                {"firstname", person},
            };
            using (_session)    
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
                var result = await query.ToListAsync();
                var i = JsonSerializer.Serialize(result);
                Console.WriteLine(i);

                return i;
            }
        }
        public async Task<bool> DoesRelationExist(string person1, string person2)
        {
            var statementText = new StringBuilder();
            statementText.Append("Match (p:Person {Firstname:$firstname}), (b:Person {Firstname:$firstname2}) RETURN EXISTS((p)-[:Request_Send]-(b))");
            var statementParameters = new Dictionary<string, object>
            {
                {"firstname", person1},
                {"firstname2", person2}
            };
            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
                var result = query.ToListAsync();
                var i = JsonSerializer.Serialize(result);
                Console.WriteLine(i);
                return true;
            }
        }

        public async Task DeleteRelation(string person1, string person2)
        {
            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person)-[r]->({Firstname: $firstname2}) Where ({Firstname: $firstname})-[r]->() DELETE r");
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
        public async Task AcceptRelation(string person1, string person2)
        {
            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person {Firstname: $firstname})-[r:Request_Send]->(m:Person {Firstname:$firstname2}) CREATE (user)-[r2:Request_Accepted]->(m) SET r2 = r WITH r DELETE r ");
            var statementParameters = new Dictionary<string, object>
            {
                {"firstname", person2},
                {"firstname2", person1}
            };
            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
            }
        }
    }
}
