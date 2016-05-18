using AngularMVCBoundDataExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace AngularMcvWebApiTableExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ServerGreeting = "Hello from the Server";
            return View();
        }

        public JsonResult WelcomeLetters()
        {
            WelcomeLetterModel model = new WelcomeLetterModel();
            var result = model.GetWelcomeLetters();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}