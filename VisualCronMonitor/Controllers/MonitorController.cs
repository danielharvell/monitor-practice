using System.Web.Mvc;

namespace VisualCronMonitor.Controllers
{
    public class MonitorController : Controller
    {
        public MonitorController()
        {
        }

        public IDbHandler DbHandler { get; set; }

        public MonitorController(IDbHandler dbHandler)
        {
            DbHandler = dbHandler;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}