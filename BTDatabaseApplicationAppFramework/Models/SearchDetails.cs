using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BTDatabaseApplicationAppFramework.Models
{


    public class SearchDetails
    {
        
        public string DDINumber { get; set; }

        [Required]
        public string PuidNumber { get; set; }

       public string Location { get; set; }

     }
   }
