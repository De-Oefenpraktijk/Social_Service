using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using OEF_Social_Service.Composition;
using OEF_Social_Service.DataAccess.Data.Services.Interfaces;
using OEF_Social_Service.Models;
using System;
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
            statementText.Append("CREATE (n:Person {Id : $id, Firstname: $firstname, Lastname: $lastname, Username: $username, Email: $emailAddress, Password: $password, EnrollmentDate: $enrollmentDate, Role: $role, Educations: $educations, Specializations: $specializations, ResidencePlace: $residencePlace})");

            var statementParameters = new Dictionary<string, object>
            {
                {"id", person.Id},
                {"firstname", person.FirstName},
                {"lastname", person.LastName},
                {"username", person.Username},
                {"emailAddress", person.EmailAddress},
                {"password", person.Password},
                {"enrollmentDate", person.EnrollmentDate.ToShortDateString()},
                {"role", person.Role},
                {"educations", person.Educations},
                {"specializations", person.Specializations},
                {"residencePlace", person.ResidencePlace}
            };

            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
            }
        }
        public async Task<string> GetUser(string username)
        {
            var statementText = new StringBuilder();
            statementText.Append("MATCH (n) WHERE n.Username = $username Return n");
            var statementParameters = new Dictionary<string, object>
            {
                { "username", username }
            };
            using (replicaSession)
            {
                var query = await replicaSession.RunAsync(statementText.ToString(), statementParameters);
                var result = await query.ToListAsync();
                var serializedResult = JsonSerializer.Serialize(result);
                return serializedResult;
            }
        }

        public async Task UpdateUser(Person person)
        {
            var statementText = new StringBuilder();
            statementText.Append("MATCH (n:Person {Username: $username}) set n = {Id : $id, Firstname: $firstname, Lastname: $lastname, Username: $username, Email: $emailAddress, Password: $password, EnrollmentDate: $enrollmentDate, Role: $role, Educations: $educations, Specializations: $specializations, ResidencePlace: $residencePlace} return n");
            var statementParameters = new Dictionary<string, object>
            {
                {"id", person.Id},
                {"firstname", person.FirstName},
                {"lastname", person.LastName},
                {"username", person.Username},
                {"emailAddress", person.EmailAddress},
                {"password", person.Password},
                {"enrollmentDate", person.EnrollmentDate.ToShortDateString()},
                {"role", person.Role},
                {"educations", person.Educations},
                {"specializations", person.Specializations},
                {"residencePlace", person.ResidencePlace}
            };

            using (_session)
            {
                var query = await _session.RunAsync(statementText.ToString(), statementParameters);
            }
        }


        public async Task SendRequest(string person1, string person2)
           {
                var requester = person1;
                var requestee = person2;

                var relationExist = DoesRelationExist(requester, requestee).Result;
                if (relationExist == false)
                {
                    var statementText = new StringBuilder();
                    statementText.Append("MATCH (p1:Person), (p2:Person) WHERE p1.Username = $userId AND p2.Username = $userId2 CREATE (p1)-[p:Request_Send] ->(p2)");
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

        public async Task<string> GetRequests(string person)
        {
            var personString = person;
            var data = new List<Person>();
            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person)-[r:Request_Send]->({Username: $firstname}) Return user");
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
        public async Task<string> GetRelatedUsers(string person)
        {
            var requestee = person;
            var data = new List<Person>();
            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person {Username: $userId})-[r:Request_Accepted*1..3]-(b) Return b");
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

        public async Task<string> GetFollowingUsers(string person)
        {
            var requestee = person;
            var data = new List<Person>();
            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person {Username: $userId})-[r:Request_Accepted]-(b) Return b");
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
            statementText.Append("Match (p:Person {Username:$userId}), (b:Person {Username: $userId2}) RETURN EXISTS((p)-[]->(b))");
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

        public async Task DeleteRelation(string person1, string person2)
        {
            var requester = person1;
            var requestee = person2;

            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person)-[r]->({Username: $userId2}) Where ({Username: $userId})-[r]->() DELETE r");
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
        public async Task AcceptRelation(string person1, string person2)
        {
            var requester = person1;
            var requestee = person2;

            var statementText = new StringBuilder();
            statementText.Append("Match (user:Person {Username: $userId})-[r:Request_Send]->(m:Person {Username:$userId2}) CREATE (user)-[r2:Request_Accepted]->(m) SET r2 = r WITH r DELETE r ");
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
        public async Task<string> GetAllUsers()
        {
            var statementText = new StringBuilder();
            statementText.Append("MATCH (n) Return n");
            using (replicaSession)
            {
                var query = await replicaSession.RunAsync(statementText.ToString());
                var result = await query.ToListAsync();
                var serializedResult = JsonSerializer.Serialize(result);
                return serializedResult;
            }
        }
    }
}
