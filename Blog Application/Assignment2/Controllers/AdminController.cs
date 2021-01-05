using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2.Models;
using System.IO;

namespace Assignment2.Controllers
{
    public class AdminController : Controller
    {
       
        [HttpPost]
        public ViewResult UpdateUser(CredentialsUserForUpdate c)
        {
            if (ModelState.IsValid)
            {
                /*If CredentialUserForUpdate Model is validated then user is updated and return to display Users View*/
                if(!UserDatabase.isUsernameMoreThanOnce(c.username,c.ID))
                {
                    if (c.image != null)
                    {
                        c.imageName = DateTime.Now.ToString("yymmssfff") + c.image.FileName;
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", c.imageName);
                        c.image.CopyTo(new FileStream(imagePath, FileMode.Create));
                    }
                    else
                    {
                        c.imageName = "default.jpg";
                    }
                    UserDatabase.updateUser(c);
                    return View("displayUsers", UserDatabase.getData());
                }
                else
                {
                    /*If Username already exist then not update*/
                    CredentialsUserForUpdate user = UserDatabase.getUser(c.ID);
                    /*Return Update User Form with original data of user*/
                    return View("UpdateUser", user);
                }
            }
            else {
                /*If CredentialUserForUpdate Model is not validated we return to UpdateUser page with original data of user*/
                CredentialsUserForUpdate user = UserDatabase.getUser(c.ID);
                /*Return Update User Form with original data of user*/
                return View("UpdateUser", user);
            }

        }

        [HttpGet]
        public ViewResult UpdateUser(int id)
        {
            /*get user data from DB using id*/
            CredentialsUserForUpdate user = UserDatabase.getUser(id);
            /*Return Update User Form*/
            return View("UpdateUser", user);
        }
        
        public ViewResult deleteUser(int id)
        {
            /*Delete user from DB with given ID*/
            UserDatabase.deleteUser(id);
            /*Delete posts of user deleted by admin from database*/
            PostsDatabase.deletePostsOfUser(id);
            /*Return to display User View*/      
            return View("displayUsers", UserDatabase.getData());
        }

        public ViewResult displayUsers()
        {
            /*Display Data of all users*/
            return View("displayUsers", UserDatabase.getData());
        }

        /*RETURNS USER SIGNUP PAGE*/
        [HttpGet]
        public ViewResult UserSignUp()
        {
            return View();
        }

        /*Add credentials entered by ADMIN to database for registering the user*/
        [HttpPost]
        public ViewResult UserSignUp(CredentialsUser c)
        {
            if (ModelState.IsValid)
            {
                /*Add User to database*/
                UserDatabase.addUser(c);
                /*ViewBag is used to display message for success*/
                ViewBag.SuccessMessage = "User Registered Successfully";
                return View("displayUsers", UserDatabase.getData());
            }
            else
            {
                return View();
            }
            
        }


        public ViewResult registerUser()
        {
            /*This method will redirect to User Sign Up page for registration*/
            return View("UserSignUp");
        }

        [HttpGet]
        public ViewResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ViewResult AdminLogin(loginCredentials c)
        {
            if (AdminDatabase.authenticateAdmin(c))
            {
                /*If admin is authenticated then go to Admin Home*/
                return View("AdminHome");
            }
            else
            {
                /*View Bag is used to alert Error Message*/
                ViewBag.ErrorMessage = "Invalid Username Or Password";
                return View();
            }
        }


        /*For redirecting to homepage*/
        public ViewResult goToHome()
        {
            return View("Index");
        }

        /*For redirecting to Admin homepage*/
        public ViewResult AdminHome()
        {
            return View();
        }

        /*RETURNS ADMIN SIGNUP PAGE*/
        [HttpGet]
        public ViewResult AdminSignUp()
        {
            return View();
        }

        /*Add credentials entered by admin to database for registering admin*/
        [HttpPost]
        public ViewResult AdminSignUp(CredentialsAdmin c)
        {
            if (ModelState.IsValid) 
            {
                AdminDatabase.addAdmin(c);
                /*ViewBag is used to display message for success*/
                ViewBag.SuccessMessage = "Admin Registered Successfully";
            }
            return View();
        }

        /*THIS REMOTE METHOD CHECKS WHETHER USERNAME ALREADY EXISTS IF YES THEN ERROR MESSAGE IS RETURNED AND
         * ACCEPTS GET AND POST REQUESTS
         */
        [AcceptVerbs("GET", "POST")]
        public IActionResult verifyUserName(string username)
        {
            if (AdminDatabase.isUsernameExist(username))
            {
                return Json($"Username {username} is already in use.");
            }
            return Json(true);
        }
    }
}
