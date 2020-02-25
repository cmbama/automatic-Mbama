

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

    public partial class Response_Group
    {
        public string Response_Group_Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{6,11}$", ErrorMessage = "Invalid DDI Number.")] // for range between 6 and 11 digits.  
        public string Response_Group_DDI_Number { get; set; }

        public string Response_Group_Owner { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid Email Address")]
        public string Response_Group_Email { get; set; }

        public int ResponseGroup_ID { get; set; }
    }
}