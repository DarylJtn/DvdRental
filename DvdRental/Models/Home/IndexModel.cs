using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DvdRental.Models.Home
{
    public class IndexModel
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }





    }
}