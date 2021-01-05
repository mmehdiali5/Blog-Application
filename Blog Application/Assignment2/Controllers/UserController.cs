using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Assignment2.Controllers
{
    public class UserController : Controller
    {
        /*Update the post if title and content are not null i.e modelState is validated else return the updatePost View
         with original data*/
        [HttpPost]
        public ViewResult updatePost(posts post)
        {
            if (ModelState.IsValid)
            {
                PostsDatabase.updatePost(post);
                ViewBag.SuccessMessage = "POST UPDATED SUCCESSFULLY";
                return View("UserHome", PostsDatabase.getPosts());
            }
            else
            {
                return View("UpdatePost", PostsDatabase.getPostById(post.ID));
            }
            
        }

        /*This function returns the update form for the post having given id*/
        [HttpGet]
        public ViewResult updatePost(int id)
        {
            posts post = PostsDatabase.getPostById(id);
            return View("UpdatePost", post);
        }

        /*This function receives id of post and call database delete function to delete the post*/
        public ViewResult deletePost(int id)
        {
            PostsDatabase.deletePostFromDB(id);
            ViewBag.SuccessMessage = "POST DELETED SUCCESSFULLY";
            return View("UserHome", PostsDatabase.getPosts());
        }

        /*When user clicks on a title it will send the post id to the controller. The controller will send LoggedInUser id and
         Post Id to DB which check whether this user has posted the post or not by checking condition post Id=given postId and 
        userId=given userId  if returns true it means loggedin user has posted the post so editPost View will open else display post 
        view will be opened*/
       public ViewResult ViewDetails(int postId)
        {
            /*Get id of logged In user*/
            int loggedInUserId = (int)HttpContext.Session.GetInt32("userId");
            /*get post from DB by Id*/
            posts post = PostsDatabase.getPostById(postId);
            /*Check whether the loggedIn User has posted the post or not*/
            if(PostsDatabase.isPostByLoggedInUser(postId,loggedInUserId))
            {
                return View("editPost", post);
            }
            else
            {
                return View("displayPost", post);
            }
        }

        /*Insert Post to DB*/
        [HttpPost]
        public ViewResult PosttoDB(posts p)
        {
            if (ModelState.IsValid)
            {
                /*Get Username from session stored when user logged in*/
                string username = (string)HttpContext.Session.GetString("username");
                /*Get User Id from session stored when user logged in*/
                int userId = (int)HttpContext.Session.GetInt32("userId");
                /*Get User Image Name from session stored when user logged in*/
                string userImage = (string)HttpContext.Session.GetString("userImage");

                /*Post will be inserted to DB*/
                PostsDatabase.addPost(p, username,userId,userImage);
                /*Returns to homepage*/
                return View("UserHome", PostsDatabase.getPosts());
            }
            else
            {
                /*If model not validates it will return to Create Post Page*/
                return View("createPost");
            }
            
        }

        /*Returns Create Post Page*/
        public ViewResult createPost()
        {
            return View();
        }

        [HttpPost]
        public ViewResult UpdateUser(CredentialsUserForUpdate c)
        {
            if (ModelState.IsValid)
            {
                /*If CredentialUserForUpdate Model is validated then user is updated and return to display Users View*/
                if (!UserDatabase.isUsernameMoreThanOnce(c.username, c.ID))
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
                    CredentialsUserForUpdate user = UserDatabase.getUser(c.ID);
                    return View("ProfilePreview", user);
                }
                else
                {
                    /*If Username already exist then not update*/
                    CredentialsUserForUpdate user = UserDatabase.getUser(c.ID);
                    /*Return Update User Form with original data of user*/
                    return View("UpdateUser", user);
                }
            }
            else
            {
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

        public ViewResult ProfilePreview()
        {
            /*Get user Id from session stored when user logged in*/
            int id =(int) HttpContext.Session.GetInt32("userId");
            /*get user data from DB using id*/
            CredentialsUserForUpdate user = UserDatabase.getUser(id);
            /*Return Update User Form*/
            return View("ProfilePreview",user);
        }

        /*Return About Page*/
        public ViewResult gotoAbout()
        {
            return View("About");
        }

        /*Return Home Page*/
        public ViewResult UserHome()
        {
            return View("UserHome", PostsDatabase.getPosts());
        }

        /*Reurn Login Form*/
        [HttpGet]
        public ViewResult UserLogin()
        {
            return View();
        }


        [HttpPost]
        public ViewResult UserLogin(loginCredentials c)
        {
            if (UserDatabase.authenticateUser(c))
            {
                CredentialsUserForUpdate user = UserDatabase.getUserByName(c.username);
                /*Storing user name in a session for using when user logs in*/
                HttpContext.Session.SetString("username", c.username);
                /*Storing user Id in a session for using when user logs in*/
                HttpContext.Session.SetInt32("userId",user.ID);
                /*Storing user Image in a session for using when user logs in*/
                HttpContext.Session.SetString("userImage",user.imageName);
                /*If user is authenticated then homepage is shown*/
                return View("UserHome", PostsDatabase.getPosts());
            }
            else
            {
                /*View Bag is used to alert Error Message*/
                ViewBag.ErrorMessage = "Invalid Username Or Password";
                return View();
            }
        }

        /*For Logout to Index*/
        public ViewResult logOut()
        {
            /*Remove the session when user logs out*/
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("username");
            return View("Index");
        }

        /*For redirecting to homepage*/
        public ViewResult goToHome()
        {
            return View("Index");
        }


        /*RETURNS USER SIGNUP PAGE*/
        [HttpGet]
        public ViewResult UserSignUp()
        {
            return View();
        }

        /*Add credentials entered by user to database for registering the user*/
        [HttpPost]
        public ViewResult UserSignUp(CredentialsUser c)
        {
            if (ModelState.IsValid)
            {
                UserDatabase.addUser(c);
                /*ViewBag is used to display message for success*/
                ViewBag.SuccessMessage = "User Registered Successfully";
            }
            return View("Index");
        }

        /*THIS REMOTE METHOD CHECKS WHETHER USERNAME ALREADY EXISTS IF YES THEN ERROR MESSAGE IS RETURNED AND
         * ACCEPTS GET AND POST REQUESTS
         */
        [AcceptVerbs("GET", "POST")]
        public IActionResult verifyUserName(string username)
        {
            if (UserDatabase.isUsernameExist(username))
            {
                return Json($"Username {username} is already in use.");
            }
            return Json(true);
        }


        /*THIS REMOTE METHOD CHECKS WHETHER USERNAME ALREADY EXISTS IF YES THEN ERROR MESSAGE IS RETURNED AND
         * ACCEPTS GET AND POST REQUESTS. It has additional check that allows the same username of user editting
         * the profile. So it will return false if username already exist other than same name of user. This method
         * is for usernme validation in update form.
         */
        [AcceptVerbs("GET", "POST")]
        public IActionResult isUserNameMoreThanOce(string username,int id )
        {
            if (UserDatabase.isUsernameMoreThanOnce(username,id))
            {
                return Json($"Username {username} is already in use.");
            }
            return Json(true);
        }

    }
}
