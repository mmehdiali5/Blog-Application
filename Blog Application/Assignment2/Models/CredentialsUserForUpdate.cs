using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*TO USE MODAL VALIDATION ATTRIBUTES*/
using System.ComponentModel.DataAnnotations;

/*THIS LIBRARY ENABLES TO USE REMOTE ATTRIBUTE FOR MODAL VALIDATION*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


/*This Modal is responsible for "USER UPDATE" It has fields for Original Password, Old Password Input to be entered by the user
 The new password Input entered by the user. The old Password must be equal to original password so we add Compare attribute
 with old Password field otherwise user will not be updated
 */
namespace Assignment2.Models
{
    public class CredentialsUserForUpdate
    {
        public int ID { get; set; }
        /*IF USERNAME ALREADY IN DB OR IS NULL IT IS NOT VALIDATED*/
        [Required(ErrorMessage = "Please enter a username")]
        [Remote(action: "isUserNameMoreThanOce", controller: "User",AdditionalFields ="ID")]
        public string username { get; set; }

        /*IF PASSWORD IS NULL IT IS NOT VALIDATED*/
        public string password { get; set; }

        /*IF OLD PASSWORD FIELD IS NULL IN UPDATE PROFILE FORM IT IS NOT VALIDATED*/
        [Required(ErrorMessage = "Please enter a password")]
        [Compare("password", ErrorMessage ="Please Enter correct password")]
        public string oldPassword { get; set; }

        /*IF NEW PASSWORD FIELD IS NULL IN UPDATE PROFILE FORM IT IS NOT VALIDATED*/
        [Required(ErrorMessage = "Please enter a password")]
        public string newPassword { get; set; }

        /*IF EMAIL IS NULL IT IS NOT VALIDATED*/
        [Required(ErrorMessage = "Please enter an email")]
        public string email { get; set; }

        public IFormFile image { get; set; }

        public string imageName { get; set; }

    }
}
