using DvdRental.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DvdRental.Controllers
{
    //controller for handling user interaction while logged out
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


        //Action result for whenever the login form 
        //has been posted to the index page

        [HttpPost]
        public ActionResult Index(IndexModel vm) {

            //Check if the username and password have been entered into the form
            if(String.IsNullOrEmpty(vm.UserName))
            ModelState.AddModelError("UserName", "Enter a valid Username");
            if(String.IsNullOrEmpty(vm.Password))
            ModelState.AddModelError("Password", "Enter a valid Password");

            ViewBag.Result = String.Format("{0} {1}",vm.UserName, vm.Password);
            
            return View();
        
        }

    }
}
