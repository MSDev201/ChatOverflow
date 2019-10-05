using ChatOverflow.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ChatOverflow.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        public static void UseHubs(this IApplicationBuilder app, string defaultRoute = "/hub/")
        {

            var injectableHubs = Assembly.GetEntryAssembly()
                .DefinedTypes
                .Where(x => x.GetCustomAttributes().FirstOrDefault(y => y.GetType() == typeof(InjectableHubAttribute)) != null)
                .ToList();


            app.UseSignalR(routes =>
            {
                foreach (var injectHub in injectableHubs)
                {
                    var realInjectHub = (InjectableHubAttribute)injectHub.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(InjectableHubAttribute));
                    var route = defaultRoute + realInjectHub.Route;

                    var routesMethod = routes.GetType().GetMethod("MapHub", new[] { typeof(PathString) });
                    var genericRoutesMethod = routesMethod.MakeGenericMethod(injectHub.UnderlyingSystemType);
                    var routePath = (PathString)route;
                    genericRoutesMethod.Invoke(routes, new object[]{ routePath });
                }
            });

            
        }
    }
}
