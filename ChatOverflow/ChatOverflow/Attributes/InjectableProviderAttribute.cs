using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Attributes
{
    public enum ProviderType
    {
        Development = 0,
        Testing = 1,
        Release = 2,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableProviderAttribute : Attribute
    {
        public ProviderType Type = ProviderType.Release;

        public InjectableProviderAttribute(ProviderType type = ProviderType.Release)
        {
            Type = type;
        }
    }
}
