using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SD200_Final_Project_Blog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /// <summary>
            ///     Just for finding which header nav to bold
            /// </summary>
            /// <variable name="CurrentControllerMethodName">Holds the method name (view name)</variable>
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Index);

            return View();
        }

        public ActionResult Blog()
        {
            /// <summary>
            ///     Just for finding which header nav to bold
            /// </summary>
            /// <variable name="CurrentControllerMethodName">Holds the method name (view name)</variable>
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Blog);

            return View();
        }

        public ActionResult Post()
        {
            /// <summary>
            ///     Just for finding which header nav to bold
            /// </summary>
            /// <variable name="CurrentControllerMethodName">Holds the method name (view name)</variable>
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Post);

            return View();
        }
    }
}