using ChatOverflow.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ChatOverflow.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static void AddDIAttributes(this IServiceCollection services)
        {
            var env = services.BuildServiceProvider().GetService<IHostingEnvironment>();

            var injectableProviders = Assembly.GetEntryAssembly()
                .DefinedTypes
                .Where(x =>
                {
                    var dipAttr = x.GetCustomAttributes().FirstOrDefault(y => y.GetType() == typeof(InjectableProviderAttribute));
                    if (dipAttr == null)
                        return false;
                    var realAttr = (InjectableProviderAttribute)dipAttr;
                    if (env.IsProduction() && realAttr.Type != ProviderType.Release)
                        return false;
                    return true;
                })
                .ToList();


            foreach (var injectProv in injectableProviders)
            {
                // Check if has Interface
                var injectProvType = injectProv.UnderlyingSystemType;
                var injectInterface = injectProvType.GetInterfaces().Where(x => x.GetCustomAttributes().FirstOrDefault(y => y.GetType() == typeof(InjectableInterfaceAttribute)) != null).SingleOrDefault();
                if (injectInterface == null)
                    continue;

                var injectInterfaceType = injectInterface.UnderlyingSystemType;
                services.Add(new ServiceDescriptor(injectInterfaceType, injectProvType, ServiceLifetime.Transient));
            }
        }

    }
}
