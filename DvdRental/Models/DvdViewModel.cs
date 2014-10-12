using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DvdRental.Models
{
    public class DvdViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }


        public string StoreLoc { get; set; }

        public int NumLeased { get; set; }

        public int TotalStock { get; set; }
    }
}