using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hangfire;
using System.Threading;
using WebAppHangfire.Logic;

namespace WebAppHangfire.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TrabajoLento tlento = new TrabajoLento();
            foreach (var str in new string[] { "f1", "f2", "f3" }) {
                BackgroundJob.Enqueue(() => tlento.CreacionArchivosLog(str));
            }
           return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}