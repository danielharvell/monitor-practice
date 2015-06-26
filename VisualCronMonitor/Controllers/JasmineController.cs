using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VisualCronMonitor.Controllers
{
    public class JasmineController : Controller
    {
        // GET: Jasmine
        public ActionResult Index()
        {
            return View("SpecRunner");
        }
    }
}