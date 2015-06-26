using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Module = Autofac.Module;

namespace VisualCronMonitor
{
    public class AutoFacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            base.Load(builder);
        }
    }
}