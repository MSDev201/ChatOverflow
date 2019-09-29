using ChatOverflow.Models.DB.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Models.ResultModels
{
    public class UserDetailsResult
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

        public UserDetailsResult()
        {

        }

        public UserDetailsResult(User user, bool privacy = true)
        {
            Id = user.Id;
            UserName = user.UserName;
            DisplayName = user.UserName;
            Email = privacy ? null : user.Email;
        }
    }
}
