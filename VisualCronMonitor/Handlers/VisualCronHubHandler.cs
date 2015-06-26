using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using VisualCronMonitor.Models;

namespace VisualCronMonitor.Handlers
{
    [HubName("visualCronHub")]
    public class VisualCronHubHandler : Hub
    {
        public IHubContext VisualCronHubContext { get; set; }
        private IDbHandler DbHandler { get; set; }

        public VisualCronHubHandler(IDbHandler dbHandler)
        {
            VisualCronHubContext = GlobalHost.ConnectionManager.GetHubContext<VisualCronHubHandler>();
            DbHandler = dbHandler;
        }

        public void UpdateClients(List<Task> tasks)
        {
            VisualCronHubContext.Clients.All.broadcastMessage(tasks);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                Console.WriteLine(String.Format("Client {0} explicitly closed the connection.", Context.ConnectionId));
            }
            else
            {
                Console.WriteLine(String.Format("Client {0} timed out .", Context.ConnectionId));
            }

            return base.OnDisconnected(stopCalled);
        }

        //public void RequestTasks() //Todo couldn't get this to work
        //{
        //    var tasks = DbHandler.UpdateTask(new Task());
        //    VisualCronHubContext.Clients.All.broadcastMessage(tasks);
        //}
    }
}