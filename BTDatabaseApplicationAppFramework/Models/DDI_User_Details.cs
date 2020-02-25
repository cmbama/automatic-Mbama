//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTDatabaseApplicationAppFramework.Models
{
    using System;
    using System.Collections.Generic;

    public partial class DDI_User_Details
    {


        [NotMapped]
        public List<Locations> LocationCollection { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid DDI Number.")]
        public string User_Allocated_DDI_Number { get; set; }

        public string User_Puid_Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid Email Address")]
        public string User_SMTP_Address { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //[RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Invalid Date dd/mm/yyyy.")]
        public Nullable<System.DateTime> User_Date_DDI_Allocated { get; set; }
        public string User_Location { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid DDI Number.")]
        public string User_Overseas_DDI_Number { get; set; }
        public int User_ID { get; set; }

    }

}
