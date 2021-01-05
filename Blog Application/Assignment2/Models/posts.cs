using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class posts
    {
        public int ID { get; set; }
        public string username { get; set; }
        public DateTime date { get; set; }

        public int userId { get; set; }

        [Required(ErrorMessage = "Please Enter Title")]
        public string title { get; set; }

        [Required(ErrorMessage = "Please Enter Content")]
        public string content { get; set; }

        public string imageName { get; set; }

    }
}
