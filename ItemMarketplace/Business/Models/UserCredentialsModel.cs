using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class UserCredentialsModel
    {
        public string Nickname { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
