using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTDatabaseApplicationAppFramework.Models
{
    public class ViewModelData
    {
        //public IEnumerable<DDI_Details> DDIDetails { get; set; }
        //public IEnumerable<DDI_User_Details> DDIUserDetails { get; set; }
        //public IEnumerable<Response_Group> ResponseGroup { get; set; }
        public IEnumerable<Response_Group_Members> ResponseGroupMembers; 
        public IEnumerable<DDI_User_Details> DDIUserDetails;
        public IEnumerable<DDI_Details> DDIDetails;
        public IEnumerable<Response_Group> ResponseGroup;


        public string SearchNumber { get; set; }
    }
    
}