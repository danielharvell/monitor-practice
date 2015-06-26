using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using VisualCronMonitor;


[assembly: OwinStartup(typeof(Startup))]
namespace VisualCronMonitor
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (Debugger.IsAttached)
            {
                var hubConfiguration = new HubConfiguration();
                hubConfiguration.EnableDetailedErrors = true;
                hubConfiguration.EnableJavaScriptProxies = true;
                app.MapSignalR("/visualcronmonitor/signalr", hubConfiguration);
            }
            else
            {
                app.MapSignalR();
            }
        }
    }
}