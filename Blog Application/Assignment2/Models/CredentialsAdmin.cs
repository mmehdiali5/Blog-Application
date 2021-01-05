using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*TO USE MODAL VALIDATION ATTRIBUTES*/
using System.ComponentModel.DataAnnotations;

/*THIS LIBRARY ENABLES TO USE REMOTE ATTRIBUTE FOR MODAL VALIDATION*/
using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Models
{
    public class CredentialsAdmin
    {
        /*IF USERNAME ALREADY IN DB OR IS NULL IT IS NOT VALIDATED*/
        [Required(ErrorMessage="Please enter a username") ]
        [Remote(action: "verifyUserName", controller: "Admin")]
        public string username { get; set; }

        /*IF PASSWORD IS NULL IT IS NOT VALIDATED*/
        [Required(ErrorMessage = "Please enter a password")]
        public string password { get; set; }
    }
}
