using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using VisualCronMonitor.Models;

namespace VisualCronMonitor.Handlers
{
    public class DbHandler : IDbHandler
    {
        private IHistoryHandler HistoryHandler { get; set; }

        public DbHandler(IHistoryHandler historyHandler)
        {
            HistoryHandler = historyHandler;
        }

        public List<Task> GetAllTasks()
        {
            using (var monitorContext = new MonitorDbContext())
            {
                try
                {
                    return monitorContext.Tasks.Select(t => t).ToList();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return new List<Task>();
                }
            }
        }

        public List<Task> UpdateTask(Task task)
        {
            using (var monitorContext = new MonitorDbContext())
            {
                try
                {
                    var matchingDbTask = monitorContext.Tasks.FirstOrDefault(t => t.Name == task.Name);

                    if (matchingDbTask != null)
                    {
                        task.TaskId = matchingDbTask.TaskId;

                        HistoryHandler.UpdateTaskHistoryProperties(task, matchingDbTask);

                        monitorContext.Set<Task>().AddOrUpdate(task);
                        monitorContext.SaveChanges();
                    }
                    else
                    {
                        var dbTasks = monitorContext.Tasks;

                        if (dbTasks.Any())
                            task.TaskId = dbTasks.Max(t => t.TaskId) + 1;
                        else
                            task.TaskId = 1;

                        HistoryHandler.UpdateTaskHistoryProperties(task, null);
                        monitorContext.Tasks.Add(task);
                        monitorContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                }

                return new List<Task> { monitorContext.Tasks.FirstOrDefault(t => t.Name == task.Name) };
            }
        }
    }
}