// JScript File

function showdialog(objS) {
    if (objS.value != "0" && objS.value != "4") {
        if (window.confirm("To use this facility you have to logged in. Do you want to login?"))
            window.parent.window.__doPostBack(this.name, '');
    }
    else
        window.parent.window.__doPostBack(this.name, '');
}

function ShowPopUp() {
    var sDiv = document.getElementById("divPopUp");
    var sFrame = document.getElementById("IfrmPop");
    if (tempS < 200)
        sDiv.style.top = ((tempY + tempS) - 290).toString() + 'px';
    else
        sDiv.style.top = ((tempY + tempS) + 10).toString() + 'px';
    sDiv.style.height = 263;
    sDiv.style.left = (tempX - 30).toString() + 'px';
    sFrame.style.height = 263;
    sFrame.style.width = '352px';
    if (!IE) {
        sDiv.style.display = "visible";
        sFrame.style.display = "visible";
    }
    else {
        sDiv.style.visibility = "visible";
        sFrame.style.visibility = "visible";
    }
}
function __Close_PopUp() {
    document.getElementById("IfrmPop").style.width = "0px";
    document.getElementById("IfrmPop").src = "about:blank";
    document.getElementById("divPopUp").style.visibility = "hidden";
    document.getElementById("IfrmPop").style.visibility = "hidden";
}
function __Close_Me() {
    document.getElementById("framLeftMenu").style.width = "0px";
    document.getElementById("framLeftMenu").src = "about:blank";
    document.getElementById("divLeftMenu").style.visibility = "hidden";
    document.getElementById("framLeftMenu").style.visibility = "hidden";
}

function X(obj) {
    var x = obj.offsetLeft
    while (obj = obj.offsetParent) x += obj.offsetLeft
    return x
}

function Y(obj) {
    var x = obj.offsetTop;
    while (obj = obj.offsetParent) x += obj.offsetTop
    return x
}


function ActivateDiv(sID, iHeight, iWidth, ileftSpace) {
    var myWidth = 0, myHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
    }
    var objB;
    if (document.body.scrollTop)
        objB = document.body;
    else if (document.documentElement.scrollTop)
        objB = document.documentElement;
    else
        objB = document.body;

    if (objB.scrollTop < 100) {
        //document.getElementById("divLeftMenu").style.top = 105;
        document.getElementById("divLeftMenu").style.top = Y(sID) + 12;
        //document.getElementById("divLeftMenu").style.height=250;
        document.getElementById("divLeftMenu").style.height = iHeight;
        document.getElementById("framLeftMenu").style.height = iHeight;
        var sWidth;
        //         if (document.getElementById("sideHead").innerText == 'Problem Details') {
        //             sWidth = myWidth - 350;
        //         }
        //         else
        //             sWidth = myWidth - 900;
        sWidth = iWidth;
        //document.getElementById("divLeftMenu").style.left = "695px";
        if (iWidth > 500)
            document.getElementById("divLeftMenu").style.left = X(sID) + (ileftSpace * 1);
        else
            document.getElementById("divLeftMenu").style.left = X(sID) + (ileftSpace * 1);
        document.getElementById("framLeftMenu").style.width = sWidth;
        if (document.getElementById("framLeftMenu").style.height == '') {

            var myH = myHeight - 35;
            document.getElementById("divLeftMenu").style.top = objB.scrollTop.toString() + 'px';
            document.getElementById("divLeftMenu").style.height = myH.toString() + 'px';
            document.getElementById("framLeftMenu").style.height = myH.toString() + 'px';

            sWidth = sWidth - 20;
            document.getElementById("divLeftMenu").style.width = sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.width = sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.display = "visible";
            document.getElementById("divLeftMenu").style.display = "visible";
        }
        else {
            document.getElementById("framLeftMenu").style.visibility = "visible";
            document.getElementById("divLeftMenu").style.visibility = "visible";
        }
        if (document.getElementById("framLeftMenu").style.width == '0px') {
            var myH = myHeight - 35;
            document.getElementById("divLeftMenu").style.top = objB.scrollTop.toString() + 'px';
            document.getElementById("divLeftMenu").style.height = myH.toString() + 'px';
            document.getElementById("framLeftMenu").style.height = myH.toString() + 'px';
            sWidth = sWidth - 20;
            document.getElementById("divLeftMenu").style.width = sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.width = sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.display = "visible";
            document.getElementById("divLeftMenu").style.display = "visible";
        }
    }
    else if (objB.scrollTop > 100) {

        document.getElementById("divLeftMenu").style.top = Y(sID) - objB.scrollTop.toString() + ' px';
        document.getElementById("divLeftMenu").style.height = iHeight;
        document.getElementById("framLeftMenu").style.height = iHeight;
        //var sWidth = myWidth - 350;
        var sWidth = iWidth;

        document.getElementById("divLeftMenu").style.left = X(sID) - ileftSpace;
        document.getElementById("framLeftMenu").style.width = sWidth;
        if (document.getElementById("framLeftMenu").style.height == '') {
            var myH = myHeight - 35;
            document.getElementById("divLeftMenu").style.top = objB.scrollTop.toString() + 'px';
            document.getElementById("divLeftMenu").style.height = myH.toString() + 'px';
            document.getElementById("framLeftMenu").style.height = myH.toString() + 'px';
            sWidth = sWidth - 20;
            document.getElementById("divLeftMenu").style.width = sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.width = sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.display = "visible";
            document.getElementById("divLeftMenu").style.display = "visible";
        }
        else {
            document.getElementById("framLeftMenu").style.visibility = "visible";
            document.getElementById("divLeftMenu").style.visibility = "visible";
        }
        if (document.getElementById("framLeftMenu").style.width == '0px') {
            var myH = myHeight - 35;
            document.getElementById("divLeftMenu").style.top = objB.scrollTop.toString() + 'px';
            document.getElementById("divLeftMenu").style.height = myH.toString() + 'px';
            document.getElementById("framLeftMenu").style.height = myH.toString() + 'px';
            sWidth = sWidth - 20;
            document.getElementById("divLeftMenu").style.width = sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.width = sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.display = "visible";
            document.getElementById("divLeftMenu").style.display = "visible";
        }
    }
}
function ActivateDiv800() {
    document.getElementById("divLeftMenu").style.height = "200px";
    document.getElementById("framLeftMenu").style.height = "200px";
    document.getElementById("divLeftMenu").style.left = "400px";
    document.getElementById("framLeftMenu").style.width = "300px";
    var objB;
    if (document.body.scrollTop)
        objB = document.body;
    else if (document.documentElement.scrollTop)
        objB = document.documentElement;
    else
        objB = document.body;
    document.getElementById("divLeftMenu").style.top = objB.scrollTop + 10;
    document.getElementById("divLeftMenu").style.visibility = "visible";
    document.getElementById("framLeftMenu").style.visibility = "visible";
}

function __Open_Help(sID, sTitle, sheight, swidth, sleftSpace, sPath) {
    //alert(document.getElementById(sID));
    document.getElementById("sideHead").innerText = sTitle;
    if (screen.width == 800) {
        //document.getElementById("framLeftMenu").src = "/EMR/ProblemDetails.aspx?" + document.getElementById(sID).innerText;
        document.getElementById("framLeftMenu").src = sPath;  //+ "?" + document.getElementById(sID).innerText; 
        ActivateDiv800()
    }
    else {
        document.getElementById("framLeftMenu").src = sPath;  //+ "?" + document.getElementById(sID).innerText;
        ActivateDiv(document.getElementById(sID), sheight, swidth, sleftSpace);
    }
}

function __addKeyword(sKeyword) {
    if (window.parent.document.getElementById('hdnProblems'))
        window.parent.document.getElementById('hdnProblems').value = sKeyword;
    window.parent.document.getElementById("framLeftMenu").style.width = "0px";
    window.parent.document.getElementById("framLeftMenu").src = "about:blank";
    window.parent.document.getElementById("divLeftMenu").style.visibility = "hidden";
    window.parent.document.getElementById("framLeftMenu").style.visibility = "hidden";
    window.parent.__doPostBack('hdnProblems', '');
}

function __addLocation(sKeyword) {

    if (window.parent.document.getElementById('txtLocation')) {
        if (window.parent.document.getElementById('txtLocation').value == '')
            window.parent.document.getElementById('txtLocation').value = sKeyword;
        else
            window.parent.document.getElementById('txtLocation').value = window.parent.document.getElementById('txtLocation').value + ', ' + sKeyword;
    }
}

function __addICDCodes(sKeyword, ParentTextBox) {

    if (window.parent.document.getElementById(ParentTextBox))
        window.parent.document.getElementById(ParentTextBox).value = sKeyword;

    window.parent.document.getElementById("framLeftMenu").style.width = "0px";
    window.parent.document.getElementById("framLeftMenu").src = "about:blank";
    window.parent.document.getElementById("divLeftMenu").style.visibility = "hidden";
    window.parent.document.getElementById("framLeftMenu").style.visibility = "hidden";
    //window.parent.__doPostBack(ParentTextBox, '');
}

function CheckDecimal(even, str) {
    if (window.event) {
        keynum = even.keyCode
    }
    // For Netscape/Firefox/Opera
    else if (even.which) {
        keynum = even.which
    }
    keycode = String.fromCharCode(keynum);
    numdecs = 0;

    dec_Val = document.getElementById('ctl00_hdnDec_Val').value;
    if (dec_Val == "") {
        dec_Val = 2;
    }
    if ((even.keyCode == 37) || (even.keyCode == 39))
    { return true; }

    if (str > 0) {
        if (keycode != ".") {
            if (keynum == 8) {
                return true;
            }
            if ((keynum < 48 || keynum > 57)) {
                return false;
            }
            dtValue = str.toLocaleString().lastIndexOf('.')
            if (dtValue != -1) {
                strDec = str.toLocaleString().substring(str.toLocaleString().lastIndexOf('.'), str.toLocaleString().length);
                if (strDec.length > dec_Val) {
                    return true;
                }
            }
        }
        for (i = 0; i < str.length; i++) {
            mychar = str.charAt(i);
            if ((mychar >= '0' && mychar <= '9') || (mychar == '.')) {
                if (mychar == ".")
                    numdecs++;
            }
            else {
                return false;
            }
        }
    }
    else {
        if ((keynum == 37) || (keynum == 39))
        { return true; }
        if (keynum == 8) {
            return true;
        }
        if (keycode == ".") {
            return false;
        }
        if ((keynum < 48 || keynum > 57)) {
            return false;
        }
    }
    if (numdecs == '1') {

        if (keycode == '.') {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
} 