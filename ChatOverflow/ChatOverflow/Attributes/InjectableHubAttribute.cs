using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableHubAttribute : Attribute
    {
        public string Route { get; set; }

        public InjectableHubAttribute(string route)
        {
            Route = route;
        }

    }
}
