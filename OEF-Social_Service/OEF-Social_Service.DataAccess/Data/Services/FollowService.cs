﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using OEF_Social_Service.Composition;
using OEF_Social_Service.DataAccess.Data.Services.Interfaces;
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

        private ILogger<FollowService> _logger;

        private string _database;
        private readonly IDriver _driver;

        public FollowService(IDriver driver, ILogger<FollowService> logger, IOptions<ApplicationSettings> appSettingsOptions)
        {
            _logger = logger;
            _database = appSettingsOptions.Value.Neo4jDatabase;
            _session = driver.AsyncSession(o => o.WithDatabase(_database));
        }

        public void printGreeting(string message)
        {
            using (_session)
            {
                var greeting = _session.RunAsync($"CREATE (a:Greeting) SET a.message = \"{message}\"  RETURN a.message");
                Console.WriteLine(greeting);
            }
        }
    }
}
