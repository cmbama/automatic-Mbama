



// Common function that can be used as required by any page to display an Error/Information message
// in the Master Page status area
function ShowMessage(from, text, type) {

    var ReturnValue = true;

    try {
       
        if (from == 1) {
            var txtStatusMessageRef = document.getElementById("modelStatusMessage_DDI_Last_Allocated_Date");
            document.getElementById("modelStatusMessage_DDI_Last_Allocated_Date").style.visibility = "visible";
        }

        if (from == 2) {
            var txtStatusMessageRef = document.getElementById("modelStatusMessage_DDI_To_Be_Vacated_Date");
            document.getElementById("modelStatusMessage_DDI_To_Be_Vacated_Date").style.visibility = "visible";
        }
       
        if (txtStatusMessageRef != null) {
            txtStatusMessageRef.textContent = type.toUpperCase() + "," + text;
        }
    }
    catch (ex) {
        ReturnValue = false;
        alert("JavaScript Error in ShowMessage\n\n" + ex.name + " : " + ex.message);
    }
    finally {
        //alert("ShowMessage Return Value = " + ReturnValue);
        return ReturnValue;
    }
}


function ValidateDDIDate() {

    var txtDDI_To_Be_Vacated_Date = document.getElementById("DDI_To_Be_Vacated_Date"); 
    var txtDDI_Last_Allocated_Date = document.getElementById("DDI_Last_Allocated_Date"); 

    var ReturnValue = true;

    //var FullName = /^[a-zA-Z '-]{2,100}$/;
    //var ContactNumber = /^[\s()+-]*([0-9][\s()+-]*){6,20}$/; 
    //var ContinueProcess = true;

    try {
      
        //var txtStatusMessageRef1 = txtStatusMessageRef = document.getElementById("modelStatusMessage_DDI_Last_Allocated_Date");
        //if (txtStatusMessageRef1 != null) {
          //  txtStatusMessageRef1.value  = "";
       // }

       // var txtStatusMessageRef2 = txtStatusMessageRef = document.getElementById("modelStatusMessage_DDI_To_Be_Vacated_Date");
        //if (txtStatusMessageRef2 != null) {
          //  txtStatusMessageRef2.value  = "";
        //}
       
        if (txtDDI_Last_Allocated_Date != null) {
            if (!(isValidDate(txtDDI_Last_Allocated_Date.value))) {
                ReturnValue = false;
                ShowMessage(1, "Please specify a valid DDI_Last_Allocated_Date(dd/mm/yyyy).", "Information");
            }
        }

        //ClearMessage();
        if (txtDDI_To_Be_Vacated_Date != null) {
            if (!(isValidDate(txtDDI_To_Be_Vacated_Date.value))) {
                ReturnValue = false;
                ShowMessage(2, "Please specify a valid DDI_To_Be_Vacated_Date(dd/mm/yyyy).", "Information");
            }
        }
    }
    catch (ex) {
        ReturnValue = false;
        alert("JavaScript Error in ValidateDDIDate\n\n" + ex.name + " : " + ex.message);
    }
    finally {
        //alert("GetCustomer Return Value = " + ReturnValue);
        return ReturnValue;
    }

}



function isValidDate(dateObject) {


    var datePat = /^(\d{2,2})(\/)(\d{2,2})\2(\d{4}|\d{4})$/;

    if ((dateObject == 'undefined') || (dateObject == null)) {
        return false;
    }

    var matchArray = dateObject.match(datePat); // is the format ok?
    if (matchArray == null) {
        //alert("Date must be in MM/DD/YYYY format")
        return false;
    }

    var day = matchArray[1]; // parse date into variables
    var month = matchArray[3];
    var year = matchArray[4];
    if (month < 1 || month > 12) { // check month range
        //alert("Month must be between 1 and 12");
        return false;
    }
    if (day < 1 || day > 31) {
        //alert("Day must be between 1 and 31");
        return false;
    }
    if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {
        //alert("Month "+month+" doesn't have 31 days!")
        return false;
    }
    if (month == 2) { // check for february 29th
        var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        if (day > 29 || (day == 29 && !isleap)) {
            //alert("February " + year + " doesn't have " + day + " days!");
            return false;
        }
    }

    return true;  // date is valid
}

