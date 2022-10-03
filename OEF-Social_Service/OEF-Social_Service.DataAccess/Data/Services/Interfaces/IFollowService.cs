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
        Task followUser(string person1, string person2);
    }
}
