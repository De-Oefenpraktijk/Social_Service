using Neo4j.Driver;
using OEF_Social_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEF_Social_Service.Services.Interfaces
{
    public interface ITestLogic
    {
        void CreatePerson(Person person);
        void followPerson(string person1, string person2);
        Task<string> getRequest(string person);
    }
}
