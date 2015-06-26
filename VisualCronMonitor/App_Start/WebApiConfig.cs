extern alias NewtonsoftAlias;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace VisualCronMonitor
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            config.MapHttpAttributeRoutes();
            ConfigureJsonSerializer(config);
            ConfigureAutoFacResolver(config);
        }

        private static void ConfigureJsonSerializer(HttpConfiguration config)
        {
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
            //    new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new NewtonsoftAlias::Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = true });
        }

        private static void ConfigureAutoFacResolver(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            foreach (Assembly assembly in BuildManager.GetReferencedAssemblies())
            {
                builder.RegisterApiControllers(assembly);
                builder.RegisterAssemblyModules(assembly);
            }

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}