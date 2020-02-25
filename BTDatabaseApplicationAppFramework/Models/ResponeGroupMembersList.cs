using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTDatabaseApplicationAppFramework.Models
{
 
    public partial class ResponseGroupMembersList
    {
        public string Response_GroupMember_PUID_Name { get; set; }
        public string Response_GroupMember_Name { get; set; }
        public string Response_GroupMember_Type { get; set; }


        public List<ResponseGroupMembersList> ResponseGroupMembersListcollection { get; set; }
    }

    }
