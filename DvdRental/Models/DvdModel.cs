using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DvdRental.Models
{
    //model for interfacing the DVD data
    public class DvdModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Title: ")]      

        public string Title { get; set; }

        [Required]
        [Display(Name = "Description: ")]      

        public string Description { get; set; }

        public int NumLeased { get; set; }

        [Required]
        [Display(Name = "Total Stock: ")]      

        public int TotalStock { get; set; }
    }
}