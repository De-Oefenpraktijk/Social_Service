using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using OEF_Social_Service.Composition;
using OEF_Social_Service.DataAccess.Data.Services.Interfaces;
using OEF_Social_Service.Models;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace OEF_Social_Service.DataAccess.Data.Services
{
    public class FollowService : IFollowService
    {
        private IAsyncSession _session;
        private string _database;
        public Person _person;
        private IAsyncSession replicaSession;

        public FollowService(IDriver driver, IOptions<ApplicationSettings> appSettingsOptions)
        {
            _database = appSettingsOptions.Value.Neo4jDatabase;
            _session = driver.AsyncSession(o => o.WithDatabase(_database));
            replicaSession = driver.AsyncSession(o => o.WithDatabase(_database));
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
            }
        }

        public async Task SendRequest(Guid person1, Guid person2)
           {
                var requester = person1.ToString();
                var requestee = person2.ToString();

                var relationExist = DoesRelationExist(requester, requestee).Result;
                if (relationExist == false)
                {
                    var statementText = new StringBuilder();
                    statementText.Append("MATCH (p1:Person), (p2:Person) WHERE p1.Id = $userId AND p2.Id = $userId2 CREATE (p1)-[p:Request_Send] ->(p2)");
                    var statementParameters = new Dictionary<string, object>
                        {
                        {"userId", requester},
                        {"userId2", requestee}
                        };
                    using (_session)
                    {
                        //_session.LastBookmark.Values(null);
                        var excecuteResult = await _session.RunAsync(statementText.ToString(), statementParameters);
                    }
                }
                if (relationExist == true)
                {
                    return;
                }
        }

        public async Task<string> GetRequests(Guid person)
        {
            var personString = person.ToString();
            var data = new List<Person>();
            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person)-[r:Request_Send]->({Id: $firstname}) Return user");
            var statementParameters = new Dictionary<string, object>
            {
                {"firstname", personString},
            };

            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
                var result = await query.ToListAsync();
                var serializedResult = JsonSerializer.Serialize(result);
                return serializedResult;
            }
        }
        public async Task<string> GetRelatedUsers(Guid person)
        {
            var requestee = person.ToString();
            var data = new List<Person>();
            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person {Id: $userId})-[r:Request_Accepted*1..3]-(b) Return b");
            //statementText.Append("Match (user:Person)-[r:Request_Send]->({Id: $firstname}) Return user");
            var statementParameters = new Dictionary<string, object>
            {
                {"userId", requestee},
            };

            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
                var result = await query.ToListAsync();
                var serializedResult = JsonSerializer.Serialize(result);
                return serializedResult;
            }
        }

        public async Task<bool> DoesRelationExist(string person1, string person2)
        {
            var statementText = new StringBuilder();
            statementText.Append("Match (p:Person {Id:$userId}), (b:Person {Id: $userId2}) RETURN EXISTS((p)-[]->(b))");
            var statementParameters = new Dictionary<string, object>
            {
                {"userId", person1},
                {"userId2", person2}
            };

            using (replicaSession)
            {
                var query = await replicaSession.RunAsync(statementText.ToString(), statementParameters);
                var result = query.ToListAsync();
                foreach (var item in result.Result[0].Values)
                {
                    if ((bool)item.Value == true)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public async Task DeleteRelation(Guid person1, Guid person2)
        {
            var requester = person1.ToString();
            var requestee = person2.ToString();

            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person)-[r]->({Id: $userId2}) Where ({Id: $userId})-[r]->() DELETE r");
            var statementParameters = new Dictionary<string, object>
            {
                {"userId", requester},
                {"userId2", requestee}
            };
            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
            }
        }
        public async Task AcceptRelation(Guid person1, Guid person2)
        {
            var requester = person1.ToString();
            var requestee = person2.ToString();

            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person {Id: $userId})-[r:Request_Send]->(m:Person {Id:$userId2}) CREATE (user)-[r2:Request_Accepted]->(m) SET r2 = r WITH r DELETE r ");
            var statementParameters = new Dictionary<string, object>
            {
                {"userId", requestee},
                {"userId2", requester}
            };
            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
            }
        }
    }
}
