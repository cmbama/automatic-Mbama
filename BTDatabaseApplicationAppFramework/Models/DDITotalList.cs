using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTDatabaseApplicationAppFramework.Models
{
 
    public class DDITotalList
    {
        public string DDI_Location { get; set; }
        public string DDI_Status  { get; set; }

        public string DDI_Number { get; set; }
        public string Total_DDI_Available { get; set; }

        public  List<DDITotalList> DDITotalListcollection { get; set; }
  }
}

   