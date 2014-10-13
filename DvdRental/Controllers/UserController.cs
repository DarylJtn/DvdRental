using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.Security;
using System.Net;
using System.Data.Entity.Core.Objects;
using System.Data.Common;
using System.Data;
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
                return RedirectToAction("Catalog", "User");


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
                   
                    var DbUser = db.Users.FirstOrDefault(u => u.UserName == user.UserName);

                    if (DbUser==null)
                    {
                        var sysUser = db.Users.Create();

                        sysUser.UserName = user.UserName;
                        sysUser.Password = Crypto.SHA256(user.Password);
                        sysUser.StoreLoc = user.StoreLoc;

                        db.Users.Add(sysUser);
                        db.SaveChanges();

                        return RedirectToAction("index", "Home");
                    }
                    else { 
                    //handle a user feedback error advising the username has already been taken
                    
                    
                    }

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
            if (Session["UserId"] == null) 
            return RedirectToAction("LogIn", "User");


            return View(db.DvdCatalogs.ToList());
        }
        
        //Method for seatching the catalog 
        [HttpPost]
        public ActionResult Catalog(string term)
        {

            if (Session["UserId"] == null)
                return RedirectToAction("LogIn", "User");

            //create a variable for the database model
            var dvds = from m in db.DvdCatalogs select m;

            //check if a search term was used and filter the data
            if (!String.IsNullOrEmpty(term))
            {
                dvds = dvds.Where(s => s.Title.Contains(term));
            }

            return View(dvds);

        }

            
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null)
            return RedirectToAction("LogIn", "User");
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

        //Make changes to the database shwo that a DVD has been rented
        public ActionResult Lease(int? id) {
            if (Session["UserId"] == null)
            return RedirectToAction("LogIn", "User");


            using (var context = new MainDBEntities())
            {
                context.Database.ExecuteSqlCommand(
                    "Update DvdCatalog set NumLeased = NumLeased + 1WHERE id =" + id + ";");
            }

        return RedirectToAction("Catalog", "User");
        }


        public ActionResult Return(int? id)
        {
            if (Session["UserId"] == null)
            return RedirectToAction("LogIn", "User");

            using (var context = new MainDBEntities())
            {
                context.Database.ExecuteSqlCommand(
                    "Update DvdCatalog set NumLeased = NumLeased - 1 WHERE id =" + id + ";");
            }


            return RedirectToAction("Catalog", "User");

        }



        //Create a new Dvd to be added to the catalog
        public ActionResult Create() {
            if (Session["UserId"] == null)
            return RedirectToAction("LogIn", "User");

            return View();
        }

        //handle the data dvd being revieved from the completed form
        [HttpPost]
        public ActionResult Create(Models.DvdModel dvd)
        {
            if (Session["UserId"] == null)
            return RedirectToAction("LogIn", "User");



            if (ModelState.IsValid)
            {
                using (var db = new MainDBEntities())
                {

                    var sysDvd = db.DvdCatalogs.Create();

                    sysDvd.Title = dvd.Title;
                    sysDvd.Description = dvd.Description;
                    sysDvd.TotalStock = dvd.TotalStock;
                    sysDvd.NumLeased = 0;
                    db.DvdCatalogs.Add(sysDvd);
                    db.SaveChanges();

                    return RedirectToAction("Catalog", "User");
                }

            }
            
            return View();
        }

        //page for editing DVD's
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null)
            return RedirectToAction("LogIn", "User");
            ViewBag.Id = id; 
            return View();
        }

        //logic for amending the changes to the database
        [HttpPost]
        public ActionResult Edit(Models.DvdModel dvd)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("LogIn", "User");

            using (var context = new MainDBEntities())
            {
                context.Database.ExecuteSqlCommand(
                    "Update DvdCatalog set Title = '" + dvd.Title +
                     "',Description = '"+ dvd.Description + 
                     "',NumLeased ='"+ dvd.NumLeased+
                     "',TotalStock = '" + dvd.TotalStock +
                     "' WHERE id =" + dvd.Id + ";");
            }

            return RedirectToAction("Catalog", "User");
        }



      [HttpPost]
        public ActionResult Search(string term)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("LogIn", "User");

            var dvds = from m in db.DvdCatalogs select m;


            if (!String.IsNullOrEmpty(term))
            {
                dvds = dvds.Where(s => s.Title.Contains(term));
            }

            return View(dvds);

        }
      public ActionResult Delete(int? id) {

          using (var context = new MainDBEntities())
          {
              context.Database.ExecuteSqlCommand(
                  "DELETE DvdCatalog WHERE id =" + id + ";");
          }


   return RedirectToAction("Catalog", "User");


      }

    }
}
