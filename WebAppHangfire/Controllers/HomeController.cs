using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hangfire;
using System.Threading;

namespace WebAppHangfire.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));
            BackgroundJob.Enqueue(() => Thread.Sleep(5000));
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