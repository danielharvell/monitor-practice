using System.Collections.Generic;
using VisualCronMonitor.Models;
using VisualCronAPI;

namespace VisualCronMonitor
{
    public interface IVisualCronEventHandler
    {
        void UpdateMonitoringTimeMessage(Server server);
    }

    public interface IDbHandler
    {
        List<Task> UpdateTask(Task task);
        List<Task> GetAllTasks();
    }

    public interface IHistoryHandler
    {
        void UpdateTaskHistoryProperties(Task task, Task matchingDbTask);
    }
}