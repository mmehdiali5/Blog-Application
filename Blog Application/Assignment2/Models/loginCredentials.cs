using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


/*LOGIN CREDENTIALS HAS SAME ATTRIBUTES FOR BOTH USER AND ADMIN AND HAS NO REMOTE ATTRIBUTE*/
namespace Assignment2.Models
{
    public class loginCredentials
    {
        /*IF USERNAME ALREADY IN DB OR IS NULL IT IS NOT VALIDATED*/
        [Required(ErrorMessage = "Please enter a username")]
        public string username { get; set; }

        /*IF PASSWORD IS NULL IT IS NOT VALIDATED*/
        [Required(ErrorMessage = "Please enter a password")]
        public string password { get; set; }
    }
}
