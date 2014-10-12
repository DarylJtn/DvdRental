using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Security;
using System.Net;
namespace DvdRental.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        //Method that recieves the data from LogIn(DvdRental.Models.UserModel user) method
        //This method will log the user in
        [HttpGet]
        public ActionResult LogIn() {


            return View();
        }


        //method for sending the log in credential to the server
        [HttpPost]
        public ActionResult LogIn(Models.UserModel user) {

            //Check if the data meets the credentials as set
            //from the methods in the UserModel.cs Class
            if (ModelState.IsValid)
            {
                if (IsValid(user.UserName, user.Password)) { 

                //create a session for authenticating the user
                Session["UserID"] = user.UserName;
                return RedirectToAction("Index","User");

                }
                else
                {
                    ModelState.AddModelError("", "Login Data is incorrect.");

                }

            }
            return View();
        }


        //Method for logging the user out of the website
        public ActionResult LogOut() {

            Session.Contents.RemoveAll();
            return RedirectToAction("Index", "Home");
          
        
        }



        //method for getting the registration form
        [HttpGet]
        public ActionResult Registration() {

            return View();
        
        }

        [HttpPost]
        public ActionResult Registration(Models.UserModel user)
        {
            
            if (ModelState.IsValid) { 
            
                using(var db = new MainDBEntities()){
                
                    var sysUser = db.Users.Create();

                    sysUser.UserName = user.UserName;
                    sysUser.Password = Crypto.SHA256(user.Password);
                    sysUser.StoreLoc = user.StoreLoc;

                    db.Users.Add(sysUser);
                    db.SaveChanges();

                    return RedirectToAction("index", "Home");
                }
            
            
            }


            return View();
        }

        //method for verifying the users credentials
        //takes in the username and password and returns true if user is legit
        private bool IsValid(string userName,string password){
            string hash = Crypto.SHA256(password);

            using (var db = new MainDBEntities()) {

                //get the user from the database from the username
                var user = db.Users.FirstOrDefault(u => u.UserName == userName);

                //check if the user exists
                if (user != null) {
                    
                    //Hash the users password and compare it to the password stored
                    //in the Database
                    if (user.Password == hash) {
                        return true;
                    }
                
                }
            }
            
            return false;
        }
            private MainDBEntities db = new MainDBEntities();

        [HttpGet]
        public ActionResult Catalog() {
            //if the user is not logged in redirect user to login page
            if (Session["UserId"] == null) {

                return RedirectToAction("LogIn", "User");

            }
          //ViewData["dvds"]= db.DvdCatalogs.);

           // return View();
           return View(db.DvdViewStocks.ToList());
        }

        public ActionResult Details(int? id)
        {  
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DvdCatalog movie = db.DvdCatalogs.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }


    }
}
