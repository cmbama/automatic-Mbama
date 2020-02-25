using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //Here Namespace used for access attributes.  
using System.Linq;
using System.Web;

namespace BTDatabaseApplicationAppFramework.Models
{
    public class UserCustomValidation
    {
        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate);
        }
    }
}
