// JScript File

function showdialog(objS)
{
   if (objS.value!="0" && objS.value!="4")
   {
    if (window.confirm("To use this facility you have to logged in. Do you want to login?"))
        window.parent.window.__doPostBack(this.name,'');
    }
    else
        window.parent.window.__doPostBack(this.name,'');
}

function ShowPopUp()
{
    var sDiv = document.getElementById("divPopUp");
    var sFrame = document.getElementById("IfrmPop");
    if(tempS<200)
        sDiv.style.top = ((tempY+tempS)-290).toString() + 'px';
    else
        sDiv.style.top = ((tempY+tempS)+10).toString() + 'px';
    sDiv.style.height=263;
    sDiv.style.left=(tempX-30).toString() + 'px';
    sFrame.style.height=263;    
    sFrame.style.width='352px';
    if(!IE)
    {
        sDiv.style.display ="visible";
        sFrame.style.display ="visible";
    }
    else
    {
        sDiv.style.visibility ="visible";
        sFrame.style.visibility ="visible";
    }
}
function __Close_PopUp()
{
    document.getElementById("IfrmPop").style.width="0px";
    document.getElementById("IfrmPop").src="about:blank";
    document.getElementById("divPopUp").style.visibility="hidden";
    document.getElementById("IfrmPop").style.visibility="hidden";
}
function __Close_Me()
{   
    document.getElementById("framLeftMenu").style.width="0px";
    
    document.getElementById("framLeftMenu").src="about:blank";
    document.getElementById("divLeftMenu").style.visibility="hidden";
    document.getElementById("framLeftMenu").style.visibility="hidden";
}

function __Close_PatientSearch()
{  
    parent.document.getElementById("framLeftMenu").style.width="0px";
    
    parent.document.getElementById("framLeftMenu").src="about:blank";
    parent.document.getElementById("divLeftMenu").style.visibility="hidden";
    parent.document.getElementById("framLeftMenu").style.visibility="hidden";
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


function ActivateDiv(sID, iHeight, iWidth, ileftSpace)
{   

       var myWidth = 0, myHeight = 0;
      if( typeof( window.innerWidth ) == 'number' ) {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
      } else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
      } else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
      }      
      var objB;
      if (document.body.scrollTop) 
        objB = document.body;
      else if(document.documentElement.scrollTop)
        objB = document.documentElement;
      else
          objB = document.body;
 
 
         //document.getElementById("divLeftMenu").style.top = 105;
         document.getElementById("divLeftMenu").style.top = Y(sID) - iHeight - document.getElementById("pnlGd1").scrollTop + 150;
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
         document.getElementById("framLeftMenu").style.width=sWidth;
         
         if(document.getElementById("framLeftMenu").style.height=='')
         {
            var myH = myHeight-35;
            document.getElementById("divLeftMenu").style.top=  objB.scrollTop.toString() + 'px';
            document.getElementById("divLeftMenu").style.height=myH.toString() + 'px';
            document.getElementById("framLeftMenu").style.height = myH.toString() + 'px';
            
            sWidth = sWidth - 20;
            document.getElementById("divLeftMenu").style.width= sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.width= sWidth.toString() + 'px';        
            document.getElementById("framLeftMenu").style.display ="visible";
            document.getElementById("divLeftMenu").style.display ="visible";
         }
         else
         {
            document.getElementById("framLeftMenu").style.visibility ="visible";
            document.getElementById("divLeftMenu").style.visibility="visible";
         }
         if(document.getElementById("framLeftMenu").style.width=='0px')
         {
         
            var myH = myHeight-35;
            document.getElementById("divLeftMenu").style.top=  objB.scrollTop.toString() + 'px';
            document.getElementById("divLeftMenu").style.height=myH.toString() + 'px';
            document.getElementById("framLeftMenu").style.height=myH.toString() + 'px';
            sWidth = sWidth - 20;
            document.getElementById("divLeftMenu").style.width= sWidth.toString() + 'px';
            document.getElementById("framLeftMenu").style.width= sWidth.toString() + 'px';        
            document.getElementById("framLeftMenu").style.display ="visible";
            document.getElementById("divLeftMenu").style.display ="visible";
         }              
    
     }

function ActivateDiv800()
{   
    document.getElementById("divLeftMenu").style.height="200px";
    document.getElementById("framLeftMenu").style.height="200px";
    document.getElementById("divLeftMenu").style.left="400px";
    document.getElementById("framLeftMenu").style.width="300px";
     var objB;
      if (document.body.scrollTop) 
        objB = document.body;
      else if(document.documentElement.scrollTop)
        objB = document.documentElement;
      else
        objB = document.body;
    document.getElementById("divLeftMenu").style.top = objB.scrollTop + 10;
    document.getElementById("divLeftMenu").style.visibility="visible";
    document.getElementById("framLeftMenu").style.visibility ="visible";
}

function __Open_Help(sID, sTitle, sheight, swidth, sleftSpace ,sPath)
{
    document.getElementById("sideHead").innerText = sTitle;
    if(screen.width==800)        
    { 
        //document.getElementById("framLeftMenu").src = "/EMR/ProblemDetails.aspx?" + document.getElementById(sID).innerText;
        document.getElementById("framLeftMenu").src = sPath;  //+ "?" + document.getElementById(sID).innerText; 
        
        ActivateDiv800()
    }
    else    
    {
        document.getElementById("framLeftMenu").src = sPath;  //+ "?" + document.getElementById(sID).innerText;
        
        ActivateDiv(document.getElementById(sID), sheight, swidth, sleftSpace);
   
     }
 }
 
 function __Open_PatientSearch(sID, sTitle, sheight, swidth, sleftSpace ,sPath)
 {
     document.getElementById("sideHead").innerText = sTitle;
     document.getElementById("framLeftMenu").src = sPath; 
     var myWidth = 0, myHeight = 0;
    
      if( typeof( window.innerWidth ) == 'number' ) {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
      } else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
      } else if(document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
      }      
      
      var objB;
      if (document.body.scrollTop) 
        objB = document.body;
      else if(document.documentElement.scrollTop)
        objB = document.documentElement;
      else
          objB = document.body;
   
         if (document.getElementById("pnlGd1") == null)
         {
             document.getElementById("divLeftMenu").style.top = 0;
         }
         else
            document.getElementById("divLeftMenu").style.top = Y(sID) - sheight - document.getElementById("pnlGd1").scrollTop + 150;
         document.getElementById("divLeftMenu").style.height = sheight;
         document.getElementById("framLeftMenu").style.height = sheight;
              
         if (swidth > 500)
             document.getElementById("divLeftMenu").style.left = 0;
         else
             document.getElementById("divLeftMenu").style.left = 0;
         document.getElementById("framLeftMenu").style.width=swidth;
          document.getElementById("framLeftMenu").style.visibility ="visible";
            document.getElementById("divLeftMenu").style.visibility="visible";

}