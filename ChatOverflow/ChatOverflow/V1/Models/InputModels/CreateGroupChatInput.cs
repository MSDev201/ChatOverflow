using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Models.InputModels
{
    public class CreateGroupChatInput
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool CreateLink { get; set; }
        public List<string> Members { get; set; }

        public CreateGroupChatInput()
        {
            Members = new List<string>();
        }
    }
}
