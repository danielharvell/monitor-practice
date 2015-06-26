using System;
using System.Diagnostics;
using System.Text;
using VisualCronMonitor.Handlers;
using VisualCronMonitor.Models;
using VisualCron;
using VisualCronAPI;

namespace VisualCronMonitor
{
    public class VisualCronEventHandler : IVisualCronEventHandler
    {
        public bool IsServerRunning { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        private VisualCronHubHandler VisualCronHubHandler { get; set; }
        private DbHandler DbHandler { get; set; }



        private StringBuilder Sb { get; set; }

        public VisualCronEventHandler()
        {
            StartTime = DateTime.Now;
            LastUpdateTime = StartTime;
            IsServerRunning = true;
            Tasks.TaskUpdatedStatic += OnTaskUpdated;
            Server.ServerStatusOffStatic += server_ServerStatusOff;
            DbHandler = new DbHandler(new HistoryHandler());
            VisualCronHubHandler = new VisualCronHubHandler(DbHandler);
        }

        private void server_ServerStatusOff(ref Server server)
        {
            IsServerRunning = false;
        }

        protected void OnTaskUpdated(ref Server vcServer, JobClass vcJob, TaskClass vcTask)
        {
            UpdateMonitoringTimeMessage(vcServer);


            if (vcTask.Stats.Status == TaskStatsClass.TaskStatusT.Waiting)
            {

                Debug.WriteLine(vcTask.Name);
                Debug.WriteLine("ExecutionTime: " + vcTask.Stats.ExecutionTime);
                Debug.WriteLine(vcTask.Stats.Status);


                var task = new Task(ref vcServer, vcJob, vcTask);
                var tasks = DbHandler.UpdateTask(task);
                VisualCronHubHandler.UpdateClients(tasks);
            }
        }

        public void UpdateMonitoringTimeMessage(Server server)
        {
            var now = DateTime.Now;
            var runningDuration = (now - StartTime);
            var lastUpdateDuration = (now - LastUpdateTime).TotalMinutes;

            if (lastUpdateDuration > 1)
            {
                LastUpdateTime = now;
                Debug.WriteLine("Monitoring Server: {1} for: {0:%d} days, {0:%h}:{0:%m}:{0:%s}", runningDuration, server.IP);
            }
        }
    }
}