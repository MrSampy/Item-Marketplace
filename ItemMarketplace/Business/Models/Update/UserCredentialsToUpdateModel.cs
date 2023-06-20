using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Update
{
    public class UserCredentialsToUpdateModel
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
