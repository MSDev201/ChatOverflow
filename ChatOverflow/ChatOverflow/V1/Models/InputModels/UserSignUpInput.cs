using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Models.InputModels
{
    public class UserSignUpInput
    {
        public string UserName { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
    }
}
