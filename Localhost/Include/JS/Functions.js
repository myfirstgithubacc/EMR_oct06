
//Satvinder Singh

function keyDown(txtCtl, btnCtl, e) {
    var unicode = e.charCode ? e.charCode : e.keyCode;
    if (unicode == 13) {
        $get(btnCtl).click();
        return false;
    }
}
function Focus(ths) {
    ths.style.backgroundColor = '#EFF3FB';
}
function Blur(ths) {
    ths.style.backgroundColor = 'white';
}
function Tab() {
    if (event.keyCode == 13)
        event.keyCode = 9;
}

//function PatientReferral() {
//    var tdate = "__/__/____";
//    if ($get('<%=ddlSpeciality.ClientID%>').options[$get('<%=ddlSpeciality.ClientID%>').selectedIndex].text == "[ Select ]") {
//        alert("Please select Speciality.");
//        $get('<%=ddlSpeciality.ClientID%>').focus();
//        return false;
//    }
//    else if ($get('<%=txtStartDate.ClientID%>').value == tdate) {
//        alert("Please select Start Date.");
//        $get('<%=txtStartDate.ClientID%>').focus();
//        return false;
//    }
//    else if ($get('<%=txtEndDate.ClientID%>').value == tdate) {
//        alert("Please select End Date.");
//        $get('<%=txtEndDate.ClientID%>').focus();
//        return false;
//    }
//    else if ($get('<%=txtRemarks.ClientID%>').value == "") {
//        alert("Please enter Remarks.");
//        $get('<%=txtRemarks.ClientID%>').focus();
//        return false;
//    }
//    else {
//        if (confirm("Are you sure? You want to save this record."))
//            return true;
//        else
//            return false;
//    }
//}
function IsNumeric(ctrl) {
    if (isNaN(ctrl.value)) {
        alert("Invalid Number.");
        ctrl.focus();
        ctrl.value = ctrl.value.substr(0, ctrl.value.length - 1);
        //ctrl.select();
    }
}
function n09(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode;
    var c = String.fromCharCode(unicode);
    var regex = /^[0-9]/;
    if (!regex.test(c))
        return false;
    else
        return true;
}
function n09Dot(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode;
    var c = String.fromCharCode(unicode);
    var regex = /^[0-9.]/;
    if (!regex.test(c))
        return false;
    else
        return true;
}
function azAZ(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode;
    var c = String.fromCharCode(unicode);
    var regex = /^[a-zA-Z -.]/;
    if (!regex.test(c))
        return false;
    else
        return true;
}
function isValidDate(sText) {
    //var reDate = /(?:0[1-9]|[12][0-9]|3[01])\/(?:0[1-9]|1[0-2])\/(?:19|20\d{2})/;
    var reDate = /^(((0[1-9]|[1-2][0-9]|3[0-1])\/(0[13578]|(10|12)))|((0[1-9]|[1-2][0-9])\/02)|((0[1-9]|[1-2][0-9]|30)\/(0[469]|11)))\/(19|20)\d\d$/;
    return reDate.test(sText);
}
function validateDate(ctrl) {
    var oInput1 = document.getElementById(ctrl);
    if (oInput1.value != "__/__/____") {
        if (isValidDate(oInput1.value)) {
            //alert("Valid");

        } else {
            alert("Invalid Date!");
            oInput1.value = "__/__/____";
            oInput1.focus();
        }
    }
}
function updateCheck(ctrl, Tdate) {
    var oInput1 = document.getElementById(ctrl);
    if (oInput1.value == "__/__/____" || oInput1.value != "__/__/____") {
        if (!confirm("Are you sure? You want to update this record!"))
            return false;
    }
    if (oInput1.value != "__/__/____") {
        if (isValidDate(oInput1.value)) {
            if (Date.parse(oInput1.value) > Date.parse(Tdate)) {
                alert("Date you selected can not be greater than the current date.");
                return false;
            }
            return true;
        }
        else {
            alert("Invalid Date!");
            return false;
        }
    }
}