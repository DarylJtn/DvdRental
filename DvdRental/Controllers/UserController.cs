using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
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

            return View();
        }


        //Method for logging the user out of the website
        public ActionResult LogOut() {

            return View();
        
        }



        //method for getting the registration form
        [HttpGet]
        public ActionResult Registration() {

            return View();
        
        }

        [HttpPost]
        public ActionResult Registration(Models.UserModel user)
        {

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
                    if (user.Password == Crypto.SHA256(password)) {
                        return true;
                    }
                
                }
            }
            
            return false;


        }

        

    }
}
