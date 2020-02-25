
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

    public partial class Response_Group_Members
    {
        public string Response_GroupMember_Name { get; set; }
        public string Response_GroupMember_Type { get; set; }
        public int Response_GroupMember_ID { get; set; }
        public string Response_GroupMember_PUID_Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid Email Address")]
        public string Response_GroupMember_Email { get; set; }


        [NotMapped]
        public List<Response_Group> ResponseGroupCollection { get; set; }

        [NotMapped]
        public List<Response_Grp_MemberTypes> ResponseGrpMemberTypesCollection { get; set; }
    }
}

