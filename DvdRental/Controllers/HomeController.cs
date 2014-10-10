using DvdRental.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DvdRental.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            var vm = new IndexModel();
            return View(vm);
        }

        public ActionResult Info() {

            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexModel vm) {


            ViewBag.Result = String.Format("{0} {1};",vm.UserName, vm.Password);
            
            return View();
        
        }

    }
}
