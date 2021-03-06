//--//------------------------------------------------------------------------------
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

    public partial class DDI_Details
    {

        [NotMapped]
        public List<DDI_Status> DDIStatusCollection { get; set; }

        [NotMapped]
        public List<Locations> LocationCollection { get; set; }

        [NotMapped]
        public List<Response_Grp_MemberTypes> ResponseGrpMemberTypesCollection { get; set; }

        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid DDI Number.")] for fixed 11 digits. 
        [RegularExpression(@"^[0-9]{6,11}$", ErrorMessage = "Invalid DDI Number.")] // for range between 6 and 11 digits.  
        public string DDI_Number { get; set; }

        public string DDI_Status { get; set; }

        //[Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DDI_Last_Allocated_Date { get; set; }

        public string DDI_Location { get; set; }
        public string DDI_Number_Type { get; set; }
        public string DDI_Assigned_To_Puid { get; set; }

        //[Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DDI_To_Be_Vacated_Date { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid Email Address")]
        public string DDI_Email_Address { get; set; }

        public int DDI_ID { get; set; }



        //public string Total_DDI_Available { get; set; }
        //public List<DDITotalList> DDITotalListcollection { get; set; }

    }
}

