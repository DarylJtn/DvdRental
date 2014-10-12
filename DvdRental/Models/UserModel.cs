using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DvdRental.Models
{
    public class UserModel
    {
       // [Required]       
       // public string Email { get; set; }
        [Required] 
        [Display(Name="User Name: ")]      
        public string UserName { get; set; }
      
        
        
        
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20,MinimumLength=6)]
        [Display(Name="Password: ")]
        public string Password { get; set; }
        
        
        
        [Display(Name="Store Location: ")]
        public string StoreLoc { get; set; }

       }
        
}