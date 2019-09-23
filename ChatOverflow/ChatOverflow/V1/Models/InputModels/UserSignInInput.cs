using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Models.InputModels
{
    public class UserSignInInput
    {
        public string NameId { get; set; }
        public string Password { get; set; }
        public string TwoFactorCode { get; set; }
    }
}
