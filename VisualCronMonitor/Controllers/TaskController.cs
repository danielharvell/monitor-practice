using System.Collections.Generic;
using System.Web.Http;
using VisualCronMonitor.Handlers;
using VisualCronMonitor.Models;

namespace VisualCronMonitor.Controllers
{
    public class TaskController : ApiController
    {
        public TaskController()
        {
        }

        public List<Task> Get()
        {
            var dbHandler = new DbHandler(new HistoryHandler());

            List<Task> tasks = dbHandler.GetAllTasks();
            return tasks;
        }
    }
}
