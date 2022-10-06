// JScript File



function doClick(buttonName,e)
    {
//the purpose of this function is to allow the enter key to 
//point to the correct button to click.
        var key;

         if(window.event)
              key = window.event.keyCode;     //IE
         else
              key = e.which;     //firefox
    
        if (key == 13 )
        {
            //Get the button the user wants to have clicked
            var btn = document.getElementById(buttonName);
            
            if (btn != null)
            { //If we find the button click it
                btn.click();
                event.keyCode = 0
            }
        }
   }
    
   
   
    //salek kumar 30-07-2007
  //maximum length validator
  
  function MaxLengthValidatorJS(txt,maxNo,btn,e)
  {
   
        var key;

        if(window.event)
              key = window.event.keyCode;     //IE
        else
              key = e.which;
        
        var Text = document.getElementById(txt).value;
        var btnn = document.getElementById(btn);
        
        if(Text.length <= maxNo)
        {
                document.getElementById(btn).disabled=false;
        }
        
        if(key  != 8)
        {
            if(Text.length > maxNo)
            {
                document.getElementById(btn).disabled=true;
                alert("You can't Enter more than " + maxNo + " characters" );
                document.getElementById(txt).focus();
            }
            
        }
        
        
        
  }
   
   
   ///salek 01-Aug-2007 
   // Validation for InstructionDischarge.aspx 
   function InstructionDischargeJS(t1,t2,t3,t4,t5,t6,t7,t8,t9,t10)
   {
       
        var txt1 = document.getElementById(t1).value;
        var txt2 = document.getElementById(t2).value;
        var txt3 = document.getElementById(t3).value;
        var txt4 = document.getElementById(t4).value;
        var txt5 = document.getElementById(t5).value;
        var txt6 = document.getElementById(t6).value;
        var txt7 = document.getElementById(t7).value;
        var txt8 = document.getElementById(t8).value;
        var txt9 = document.getElementById(t9).value;
        var txt10 = document.getElementById(t10).value;
       
       
        if(txt1.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Activity'");
            document.getElementById(t1).focus();
            return false;
        }
        
        if(txt2.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Diet'");
            document.getElementById(t2).focus();
            return false;
        }
        if(txt3.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Life - Style'");
            document.getElementById(t3).focus();
            return false;
        }
        
        if(txt4.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Wound / Dressing'");
            document.getElementById(t4).focus();
            return false;
        }
        
        if(txt5.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Special Care'");
            document.getElementById(t5).focus();
            return false;
        }
        
        if(txt6.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Diagenosis'");
            document.getElementById(t6).focus();
            return false;
        }
        
        if(txt7.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Examination1'");
            document.getElementById(t7).focus();
            return false;
        }
        if(txt8.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Examination2'");
            document.getElementById(t8).focus();
            return false;
        }
        
        if(txt9.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Other'");
            document.getElementById(t9).focus();
            return false;
        }       
        
          if(txt10.length  > 1000)
        {
            
            alert("You Can't Enter more then 1000 characters in  'Doctor's Notes'");
            document.getElementById(t10).focus();
            return false;
        }  
        
        document.Form1.Submit();
        return true;
        
   }
   
   
<!--
/*
  - Give Credit Where Its Due -
  Please acknowledge this article and its author, at
  least in code comments, when using this code.

  Author: Justin Whitford
  Source: www.evolt.org

  Thank you.
*/

/*
  filtery(pattern, list)
  pattern: a string of zero or more characters by which to filter the list
  list: reference to a form object of type, select

  Example:
  <form name="yourForm">
    <input type="text" name="yourTextField" onchange="filtery(this.value,this.form.yourSelect)">
    <select name="yourSelect">
      <option></option>
      <option value="Australia">Australia</option>
       .......
*/

function TimeValidatorJS(txtName1,txtName2,maxtime)
   {
        var sText1=document.getElementById(txtName1).value;
	    
        
	    var ValidChars="0123456789.";
        var IsNumber1=true;
        
        var Char;
       
        for(i=0 ; i<sText1.length && IsNumber1 == true ; i++)
        {
            Char=sText1.charAt(i);
            if(ValidChars.indexOf(Char) == -1)
            {
                IsNumber1 = false;
                alert(" Invalid Character ");
                document.getElementById(txtName1).focus();
                document.getElementById(txtName1).value=sText1.substring(0,i);
                
            }
        }
 
        if(sText1 > maxtime && IsNumber1 == true)
        {
            IsNumber1 = false;
            alert(" Value not be greater than " + maxtime );
            document.getElementById(txtName1).focus();
        }


       if(sText1.length == 2 && IsNumber1 == true)
        {
            document.getElementById(txtName2).focus();
        }
        
        
   }
  
  
 
   //satyendra pandit 16-07-2007
 function RequireOneField(txtName1)
   {
        
       var txtName1;
       var sText1=document.getElementById(txtName1).value;
      
        if(sText1.length == 0 )
        {
            alert("Please Enter Remarks");
            document.getElementById(txtName1).focus();
        }
        else
        {   document.Form1.Submit();
            return true;
            
        }
   }
   function CheckPatientNameORID(txtName1,txtName2)
   {
        alert("aaa");
       var txtName1;
       var sText1=document.getElementById(txtName1).value;
       var sText2=document.getElementById(txtName2).value;
        if(sText1.length == 0 && sText2.length == 0)
        {
            alert("Please Enter Either Patient Name OR Registration No");
            document.getElementById(txtName1).focus();
            return false;
        }
        else
        {   document.Form1.Submit();
            return true;
            
        }
   }
// salek 14-06-2007
function EmployeeRegistrationJS(txtEName,txtAddr,txtEmail,txtPhone,txtSTDPhone)
    {
        var ENameText=document.getElementById(txtEName).value;
        var AddrText=document.getElementById(txtAddr).value;
        var EmailText=document.getElementById(txtEmail).value;
        var PhoneText=document.getElementById(txtPhone).value;
        var PhoneSTDText=document.getElementById(txtSTDPhone).value;
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
       
       if(ENameText.length == 0 )
        {
            
            alert("Please Enter Employee Name ");
            document.getElementById(txtEName).focus();
            return false;
            
            
        }
        if(AddrText.length == 0 )
        {
            
            alert("Please Enter Employee Address" );
            document.getElementById(txtAddr).focus();
            return false;
            
        }
        
        if(PhoneText.length > 0 || PhoneSTDText.length > 0 )
        {
            
            if(PhoneSTDText.length == 0 )
            {
                
                alert("Please Enter STD Code Number " );
                document.getElementById(txtSTDPhone).focus();
                return false;
                 
            }
             
            if(PhoneText.length == 0 )
            {
                
                alert("Please Enter Phone Number " );
                document.getElementById(txtPhone).focus();
                return false;
                 
            }   

                   
        }
        
        if(EmailText.length > 0)
        {
            if (EmailText.indexOf(at)==-1)
		    {
    		
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }
		    if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }

		    if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		    }

		     if (EmailText.indexOf(at,(lat+1))!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.indexOf(dot,(lat+2))==-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
    		
		     if (EmailText.indexOf(" ")!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
            
        }
        
            
           // document.getElementById(buttonName).click();
            document.Form1.Submit();
            return true;
            
       
       
       
    
    }


// salek 13-06-2007
function NumberValidatorJS(txtnum)
    {
    
    
    var sText1=document.getElementById(txtnum).value;
	    
        
	    var ValidChars="0123456789";
        var IsNumber1=true;
        var newString;
        var Char;
       
        for(i=0 ; i<sText1.length && IsNumber1 == true ; i++)
        {
            Char=sText1.charAt(i);
            if(ValidChars.indexOf(Char) == -1)
            {
                IsNumber1 = false;
                alert(" Invalid Character ");
                document.getElementById(txtnum).focus();
                document.getElementById(txtnum).value=sText1.substring(0,i);
                
                
            }
        }
        
 
    
    }

//*************************************************
  //zafar 05-07-2007
  //************************************************
   function RequiredFieldCheckBoxTextBox(txtName1,txtName2,btn,msg1,msg2)
   {
        
       var txtName1,txtName2,buttonName,msg1,msg2;
       var sText2=document.getElementById(txtName2).value;
        if( document.getElementById(txtName1).checked == false )
        {
          alert("Please Check " + msg1 );
          // document.getElementById(txtName1).focus();
         }
        else if(sText2.length == 0 )
        {
          
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
            
            
        }
        else
        {   document.Form1.Submit();
            return true;
            
        }
   }
   //*************************************************
  //zafar 05-07-2007
  //************************************************
   function RequiredFieldCheckBox2TextBox(txtName1,txtName2,txtName3,btn,msg1,msg2,msg3)
   {
        
       var txtName1,txtName2,txtName3,buttonName,msg1,msg2;
       var sText2=document.getElementById(txtName2).value;
       var sText3=document.getElementById(txtName3).value;
        if( document.getElementById(txtName1).checked == false )
        {
          alert("Please Check " + msg1 );
          // document.getElementById(txtName1).focus();
          return false;
         }
        else if(sText2.length == 0 )
        {
          
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
            return false;  
            
        }
         else if(sText3.length == 0 )
        {
          
            alert("Please Enter " + msg3 );
            document.getElementById(txtName3).focus();
           return false; 
            
        }
        
        else
        {  
         document.Form1.Submit();
            return true;
            
        }
   }

//salek kumar 11-06-2007
function RequiredFieldJS1(txtName,buttonName,msg)
   {
      
        var txtName,buttonName,msg;
        var sText=document.getElementById(txtName).value;
	   
	    if(sText.length == 0 )
        {
            
            alert("Please Enter " + msg );
            document.getElementById(txtName).focus();
                    return false;   
            
        }
        else
        {
           
            document.Form1.Submit();
            return true;
            
            
        }
   }
   // created satyendra on 02-06-07 
   
   function RequiredFieldJScmb(txtName1,txtName2,txtName3,cmbref,cmbCasetype,cmbRoom,buttonName,msg1,msg2,msg3)
   {
        
        var  txtName1,txtName2,txtName3,buttonName,cmbref,cmbCasetype,cmbRoom,msg1,msg2,msg3;
        var  sText1=document.getElementById(txtName1).value;
        var  sText2=document.getElementById(txtName2).value;
        var  sText3=document.getElementById(txtName3).value;
        var  RefText=document.getElementById(cmbref).value;
        var  CaseText=document.getElementById(cmbCasetype).value;
        var  RoomText=document.getElementById(cmbRoom).value;
       
	    if(sText1.length == 0 )
        {
            alert("Please Enter " + msg1 );
            document.getElementById(txtName1).focus();
             return false;
        }
        if(sText2.length == 0 )
        { 
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
             return false;
         }
        if(sText3.length == 0 )
        {
           alert("Please Enter " + msg3 );
           document.getElementById(txtName3).focus();
            return false;
        }
        if(RefText.length == 0 )
        {
           alert("Please Select Reffered By." );
           document.getElementById(cmbref).focus();
            return false;
        }
        if(CaseText.length == 0 )
        {   alert("Please Select Case Type");
            document.getElementById(cmbCasetype).focus();
             return false;
           
        }
       if(RoomText.length == 0 )
        {
            
            alert("Please Select Room" );
            document.getElementById(cmbRoom).focus();
             return false;
        }
      
            document.Form1.Submit();
            return true;
 }
 //created on 3-07-07 by Satyendra
 function RequiredFieldJScmb2(txtName1,txtName2,txtName3,cmbCasetype,cmbRoom,buttonName,msg1,msg2,msg3)
   {
        
        var  txtName1,txtName2,txtName3,cmbCasetype,cmbRoom,buttonName,msg1,msg2,msg3;
        var  sText1=document.getElementById(txtName1).value;
        var  sText2=document.getElementById(txtName2).value;
        var  sText3=document.getElementById(txtName3).value;
        var  CaseText=document.getElementById(cmbCasetype).value;
        var  RoomText=document.getElementById(cmbRoom).value;
       
	    if(sText1.length == 0 )
        {
            alert("Please Enter " + msg1 );
            document.getElementById(txtName1).focus();
            return false;
        }
        if(sText2.length == 0 )
        { 
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
             return false;
         }
        if(sText3.length == 0 )
        {
           alert("Please Enter " + msg3 );
           document.getElementById(txtName3).focus();
            return false;
        }
         if(CaseText.length == 0 )
        {   alert("Please Select Case Type");
            document.getElementById(cmbCasetype).focus();
             return false;
           
        }
        if(RoomText.length == 0 )
        {
            
            alert("Please Select Room" );
            document.getElementById(cmbRoom).focus();
             return false;
        }
      
            document.Form1.Submit();
            return true;
 }

//created satyendra on 16-06-07

function RequiredAtLeastOneFieldJS2(txtName1,txtName2,msg)
   {
        
       var txtName1,txtName2,buttonName,msg1,msg2;
       var sText1=document.getElementById(txtName1).value;
       var sText2=document.getElementById(txtName2).value;
        if(sText1.length == 0 && sText2.length == 0 )
        {
            alert("Please Enter " + msg);
            document.getElementById(txtName1).focus();
            return false;
        }
        else
        {   document.Form1.Submit();
            return true;
            
        }
   }

//salek kumar 11-06-2007
function RequiredFieldJS2(txtName1,txtName2,btn,msg1,msg2)
   {
        
       var txtName1,txtName2,buttonName,msg1,msg2;
       var sText1=document.getElementById(txtName1).value;
       var sText2=document.getElementById(txtName2).value;
        if(sText1.length == 0 )
        {
          alert("Please Enter " + msg1 );
            document.getElementById(txtName1).focus();
            return false;
         }
        else if(sText2.length == 0 )
        {
          
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
            return false;
            
            
        }
        else
        {   document.Form1.Submit();
            return true;
            
        }
   }
  //****************************************
   //Created By zafar on 02-07-2007
  //**************************************** 
   function RequiredFieldRoundJS3(cmbDate,cmbRoundNo,txtHr,txtMin,buttonName,msg1,msg2,msg3,msg4)
   {
        
       var cmbRoundNo,cmbDate,txtHr,txtMin,buttonName,msg1,msg2,msg3;
       var RoundNocmb=document.getElementById(cmbRoundNo).value;
       var cmbDate=document.getElementById(cmbDate).value;
       var sText2=document.getElementById(txtHr).value;
       var sText3=document.getElementById(txtMin).value;
      
	    if(cmbDate.length == 0 )
        {
            
            alert("Please Enter " + msg1 );
            document.getElementById(cmbDate).focus();
            
            return false;
            
        }
	    else if(RoundNocmb.length == 0 )
        {
            
            alert("Please Add " + msg2 + "First");
            document.getElementById(cmbRoundNo).focus();
                     return false;
            
        }
	  
        else if(sText2.length == 0 )
        {
            
            alert("Please Enter " + msg3 );
            document.getElementById(txtHr).focus();
            return false;
            
        }
        else if(sText3.length == 0 )
        {
            
            alert("Please Enter " + msg4 );
            document.getElementById(txtMin).focus();
            
            return false;
        }
        else
        {
            
            document.Form1.Submit();
            return true;
            
        }
        
   }
//###################################################
   
   
//salek kumar 11-06-2007
function RequiredFieldJS3(txtName1,txtName2,txtName3,buttonName,msg1,msg2,msg3)
   {
        
       var txtName1,txtName2,txtName3,buttonName,msg1,msg2,msg3;
       var sText1=document.getElementById(txtName1).value;
       var sText2=document.getElementById(txtName2).value;
       var sText3=document.getElementById(txtName3).value;
       
	  
	   
	    if(sText1.length == 0 )
        {
            
            alert("Please Enter " + msg1 );
            document.getElementById(txtName1).focus();
            
            return false;
            
        }
        else if(sText2.length == 0 )
        {
            
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
            
            return false;
        }
        else if(sText3.length == 0 )
        {
            
            alert("Please Enter " + msg3 );
            document.getElementById(txtName3).focus();
            
            return false;
        }
        else
        {
            
            document.Form1.Submit();
            return true;
            
        }
        
   }
//*********************************
// Created By Zafar on 02-07-2007
function RequiredFieldRoundJS6(cmbDate,cmbRoundNo,txtName2,txtName3,txtName4,txtName5,buttonName,msg1,msg2,msg3,msg4,msg5,msg6)
   {
        
       var cmbDate,RoundNocmb,txtName2,txtName3,txtName4,txtName5,buttonName,msg1,msg2,msg3,msg4,msg5,msg6;
       var cmbDate=document.getElementById(cmbDate).value;
       var sText2=document.getElementById(txtName2).value;
       var sText3=document.getElementById(txtName3).value;
       var sText4=document.getElementById(txtName4).value;
       var sText5=document.getElementById(txtName5).value;
	   var RoundNocmb=document.getElementById(cmbRoundNo).value;
	   
	   if(cmbDate.length == 0 )
        {
            alert("Please Enter " + msg1 );
            document.getElementById(cmbDate).focus();
            return false;
        }
	    else if(RoundNocmb.length==0)
	   {
	        alert("Please Add " + msg2 + "" + "First");
            document.getElementById(cmbRoundNo).focus();
            return false;
	   }
	  
        else if(sText2.length == 0 )
        {
            
            alert("Please Enter " + msg3 );
            document.getElementById(txtName2).focus();
            return false;
            
            
        }
        else if(sText3.length == 0 )
        {
            
            alert("Please Enter " + msg4 );
            document.getElementById(txtName3).focus();
            return false;
            
        }
        else if(sText4.length == 0 )
        {
            
            alert("Please Enter " + msg5 );
            document.getElementById(txtName4).focus();
            return false;
            
        }
        else if(sText5.length == 0 )
        {
            
            alert("Please Enter " + msg6);
            document.getElementById(txtName5).focus();
            return false;
            
        }
        else
        {
            
            document.Form1.Submit();
            return true;
            
        }
   }
  //############################################ 



//salek kumar 11-06-2007
function RequiredFieldJS4(txtName1,txtName2,txtName3,txtName4,buttonName,msg1,msg2,msg3,msg4)
   {
        
       var txtName1,txtName2,txtName3,txtName4,buttonName,msg1,msg2,msg3,msg4;
       var sText1=document.getElementById(txtName1).value;
       var sText2=document.getElementById(txtName2).value;
       var sText3=document.getElementById(txtName3).value;
       var sText4=document.getElementById(txtName4).value;
	  
	   
	    if(sText1.length == 0 )
        {
            
            alert("Please Enter " + msg1 );
            document.getElementById(txtName1).focus();
            
            return false;
            
        }
        else if(sText2.length == 0 )
        {
            
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
            return false;
            
        }
        else if(sText3.length == 0 )
        {
            
            alert("Please Enter " + msg3 );
            document.getElementById(txtName3).focus();
            
            return false;
        }
        else if(sText4.length == 0 )
        {
            
            alert("Please Enter " + msg4 );
            document.getElementById(txtName4).focus();
            return false;
            
        }
        else
        {
            
            document.Form1.Submit();
            return true;
            
        }
   }
   
   //salek kumar 23-06-2007
function RequiredFieldJS5(txtName1,txtName2,txtName3,txtName4,txtName5,buttonName,msg1,msg2,msg3,msg4,msg5)
   {
        
       var txtName1,txtName2,txtName3,txtName4,txtName5,buttonName,msg1,msg2,msg3,msg4,msg5;
       var sText1=document.getElementById(txtName1).value;
       var sText2=document.getElementById(txtName2).value;
       var sText3=document.getElementById(txtName3).value;
       var sText4=document.getElementById(txtName4).value;
	   var sText5=document.getElementById(txtName5).value;
	   
	    if(sText1.length == 0 )
        {
            
            alert("Please Enter " + msg1 );
            document.getElementById(txtName1).focus();
            
            return false;
            
        }
        else if(sText2.length == 0 )
        {
            
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
            
            return false;
        }
        else if(sText3.length == 0 )
        {
            
            alert("Please Enter " + msg3 );
            document.getElementById(txtName3).focus();
            return false;
            
        }
        else if(sText4.length == 0 )
        {
            
            alert("Please Enter " + msg4 );
            document.getElementById(txtName4).focus();
            return false;
            
        }
        else if(sText5.length == 0 )
        {
            
            alert("Please Enter " + msg5 );
            document.getElementById(txtName5).focus();
            
            return false;
        }
        
        else
        {
            
            document.Form1.Submit();
            return true;
            
        }
   }
//salek kumar 11-06-2007
function ChangePassword(txtName1,txtName2,txtName3,buttonName,msg1,msg2,msg3)
   {
        
       var txtName1,txtName2,txtName3,buttonName,msg1,msg2,msg3;
       var sText1=document.getElementById(txtName1).value;
       var sText2=document.getElementById(txtName2).value;
       var sText3=document.getElementById(txtName3).value;
       
	  
	   
	    if(sText1.length == 0 )
        {
            
            alert("Please Enter " + msg1 );
            document.getElementById(txtName1).focus();
            
            return false;
            
        }
        else if(sText2.length == 0 )
        {
            
            alert("Please Enter " + msg2 );
            document.getElementById(txtName2).focus();
            return false;
            
        }
        else if(sText3.length == 0 )
        {
            
            alert("Please Enter " + msg3 );
            document.getElementById(txtName3).focus();
            return false;
            
        }
        else if(sText2.length < 6 )
        {
            
            alert("Password can't be less then 6 character " );
            document.getElementById(txtName2).value = "";
            document.getElementById(txtName3).value = "";
            document.getElementById(txtName2).focus();
            return false;
            
        }
        else if(sText2 != sText3)
        {
            alert("New Password not match with Confirm Password " );
            document.getElementById(txtName2).value = "";
            document.getElementById(txtName3).value = "";
            document.getElementById(txtName2).focus();
            return false;
        }
        
        else
        {
            
            document.Form1.Submit();
            return true;
            
        }
   }
   
  // Created By Zafar on 02-07-2007 
   function EmployeeRegistration(txName,cmdTitle,msgDName)
   {
        var Name=document.getElementById(txName).value;
        var Titlecmd=document.getElementById(cmdTitle).value;
        if(Titlecmd.length == 0 )
        {
            alert("Please Select Title" );
            document.getElementById(cmdTitle).focus();
            return false;
        }
         if(Name.length == 0 )
        {
            alert("Please Enter" + msgDName );
            document.getElementById(txName).focus();
            return false;
        }
        document.Form1.Submit();
        return true;
  
  }
  
  
 // Modified by zafar on 02-07-2007 
 // Created Satyendra on 13/06/07 modified on 05-07-07  MODIFIED ON 05-07-07 removing std and phone validation and 
 // adding mobile validation that check not less than 10 digits
function DoctorRegistration(txtDName,cmdTitle,msgDName,txtMobile,txtEmail)
   {
        var DNameText=document.getElementById(txtDName).value;
        var Titlecmd=document.getElementById(cmdTitle).value;
        var MobileText=document.getElementById(txtMobile).value;
        var EmailText=document.getElementById(txtEmail).value;
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
        
        if(Titlecmd.length == 0 )
        {
            alert("Please Select Title" );
            document.getElementById(cmdTitle).focus();
            return false;
        }
	   
	    if(DNameText.length == 0 )
        {
            alert("Please Enter " + msgDName );
            document.getElementById(txtDName).focus();
            return false;
        }
       if(MobileText.length !=0)
       {
            if(MobileText.length < 10 )
            {
                alert("Pls Enter Your Mobile No Greater than or Equal to 10 Digit");
                document.getElementById(txtMobile).focus();
                return false;
            }
        return false;
        }
        //e-mail ID
        if(EmailText.length > 0)
        {
            if (EmailText.indexOf(at)==-1)
		    {
    		
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }
		    if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }

		    if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		    }

		     if (EmailText.indexOf(at,(lat+1))!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.indexOf(dot,(lat+2))==-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
    		
		     if (EmailText.indexOf(" ")!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
            
        }
   
        
        document.Form1.Submit();
        return true;
   }
   
   function CommonRegistrationHosp(txtDName,msgDName,txtEmail,txtHouseNo,txtStreet,txtLocality,txtCity,txtPinCode,msgAddress)
   {
        var DNameText=document.getElementById(txtDName).value;
        var EmailText=document.getElementById(txtEmail).value;
        //Address
        var HouseNoText=document.getElementById(txtHouseNo).value;
        var StreetText=document.getElementById(txtStreet).value;
        var LocalityText=document.getElementById(txtLocality).value;
        var CityText=document.getElementById(txtCity).value;
        var PinCodeText=document.getElementById(txtPinCode).value;
         
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
        
       
	   
	    if(DNameText.length == 0 )
        {
            alert("Please Enter " + msgDName );
            document.getElementById(txtDName).focus();
            return false;
        }
      
        //e-mail ID
        if(EmailText.length > 0)
        {
            if (EmailText.indexOf(at)==-1)
		    {
    		
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }
		    if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }

		    if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		    }

		     if (EmailText.indexOf(at,(lat+1))!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.indexOf(dot,(lat+2))==-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
    		
		     if (EmailText.indexOf(" ")!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
            
        }
        //address
	    if(HouseNoText.length == 0 && StreetText.length == 0 && LocalityText.length == 0 && CityText.length == 0 && PinCodeText.length == 0)
	    {
	     alert("Please Enter  " + msgAddress);
	     document.getElementById(txtHouseNo).focus();
	     return false;
	    
	    }
	   //address 
	   
        document.Form1.Submit();
        return true;
   }
   
   // Modified By Zafar on 15-07-2007 
   function CommonRegistrationPatient(txtDName,cmdTitle,msgDName,txtMobile,txtHr,txtMin,txtEmail)
   {
        var DNameText=document.getElementById(txtDName).value;
        var Titlecmd=document.getElementById(cmdTitle).value;
        var MobileText=document.getElementById(txtMobile).value;
         var HrText=document.getElementById(txtHr).value;
        var MinText=document.getElementById(txtMin).value;
        var EmailText=document.getElementById(txtEmail).value;
        //Address
//        var HouseNoText=document.getElementById(txtHouseNo).value;
//        var StreetText=document.getElementById(txtStreet).value;
//        var LocalityText=document.getElementById(txtLocality).value;
//        var CityText=document.getElementById(txtCity).value;
//        var PinCodeText=document.getElementById(txtPinCode).value;
//        
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
        
        if(Titlecmd.length == 0 )
        {
            alert("Please Select Title" );
            document.getElementById(cmdTitle).focus();
            return false;
        }
	   
	    if(DNameText.length == 0 )
        {
            alert("Please Enter " + msgDName );
            document.getElementById(txtDName).focus();
            return false;
        }
       if(MobileText.length > 0)
       {
            if(MobileText.length < 10 )
            {
                alert("Pls Enter Your Mobile No Greater than or Equal to 10 Digit");
                document.getElementById(txtMobile).focus();
                return false;
            }
           
        }
        if(HrText.length > 0 || MinText.length > 0 )
        {
            
            if(HrText.length == 0 )
            {
                
                alert("Please Enter Hours " );
                document.getElementById(txtHr).focus();
                return false;
                 
            }
             
            if(MinText.length == 0 )
            {
                
                alert("Please Enter Minutes " );
                document.getElementById(txtMin).focus();
                return false;
                 
            }   

                   
        }
        
        
        //e-mail ID
        if(EmailText.length > 0)
        {
            if (EmailText.indexOf(at)==-1)
		    {
    		
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }
		    if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }

		    if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		    }

		     if (EmailText.indexOf(at,(lat+1))!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.indexOf(dot,(lat+2))==-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
    		
		     if (EmailText.indexOf(" ")!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
            
        }
//        //address
//	    if(HouseNoText.length == 0 && StreetText.length == 0 && LocalityText.length == 0 && CityText.length == 0 && PinCodeText.length == 0)
//	    {
//	     alert("Please Enter " + msgAddress);
//	     document.getElementById(txtHouseNo).focus();
//	     return false;
//	    
//	    }
	   //address 
	   
        document.Form1.Submit();
        return true;
   }
   //satyendra  05-07-07
   function CommonRegistration(txtDName,cmdTitle,msgDName,txtMobile,txtEmail,txtHouseNo,txtStreet,txtLocality,txtCity,txtPinCode,msgAddress,cmbUserType)
   {
        var DNameText=document.getElementById(txtDName).value;
        var Titlecmd=document.getElementById(cmdTitle).value;
        var MobileText=document.getElementById(txtMobile).value;
        var EmailText=document.getElementById(txtEmail).value;
        //Address
        var HouseNoText=document.getElementById(txtHouseNo).value;
        var StreetText=document.getElementById(txtStreet).value;
        var LocalityText=document.getElementById(txtLocality).value;
        var CityText=document.getElementById(txtCity).value;
        var PinCodeText=document.getElementById(txtPinCode).value;
        var cmbIndex = document.getElementById(cmbUserType).selectedIndex;
        var TextUserType=document.getElementById(cmbUserType).options[cmbIndex].outerText;
      
        
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
        
        if(Titlecmd.length == 0 )
        {
            alert("Please Select Title" );
            document.getElementById(cmdTitle).focus();
            return false;
        }
	   
	    if(DNameText.length == 0 )
        {
            alert("Please Enter " + msgDName );
            document.getElementById(txtDName).focus();
            return false;
        }
       if(MobileText.length > 0)
       {
            if(MobileText.length < 10 )
            {
                alert("Pls Enter Your Mobile No Greater than or Equal to 10 Digit");
                document.getElementById(txtMobile).focus();
                return false;
            }
      
        }
        //e-mail ID
        if(EmailText.length > 0)
        {
            if (EmailText.indexOf(at)==-1)
		    {
    		
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }
		    if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }

		    if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		    }

		     if (EmailText.indexOf(at,(lat+1))!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.indexOf(dot,(lat+2))==-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
    		
		     if (EmailText.indexOf(" ")!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
            
        }
       
        
        //address
	    if(HouseNoText.length == 0 && StreetText.length == 0 && LocalityText.length == 0 && CityText.length == 0 && PinCodeText.length == 0)
	    {
	         alert("Please Enter  " + msgAddress);
	         document.getElementById(txtHouseNo).focus();
	         return false;
	    
	    }
	   // alert(TextUserType);
	    if(TextUserType == "--Select--")
        {
            alert("Please Select UserType"); 
            document.getElementById(cmbUserType).focus();
            return false;
	   //address 
	   }
        
        document.Form1.Submit();
        return true;
   }
   
   
   
   //created by satyendra 
   function HospitalRegistration(txtDName,msgDName,txtEmail)
   {
        var DNameText=document.getElementById(txtDName).value;
        var EmailText=document.getElementById(txtEmail).value;
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
        
	    if(DNameText.length == 0 )
        {
            alert("Please Enter " + msgDName );
            document.getElementById(txtDName).focus();
            return false;
        }
      
        //e-mail ID
        if(EmailText.length > 0)
        {
            if (EmailText.indexOf(at)==-1)
		    {
    		
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }
		    if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }

		    if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		    }

		     if (EmailText.indexOf(at,(lat+1))!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.indexOf(dot,(lat+2))==-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
    		
		     if (EmailText.indexOf(" ")!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
            
        }
   
        
        document.Form1.Submit();
        return true;
   }
 // Created Satyendra on 13/06/07 
 function EmailvalidatorJS(txtEmail)  
   {    
        
        var EmailText=document.getElementById(txtEmail).value;
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
				
		if (EmailText.indexOf(at)==-1)
		{
		
		   alert("Invalid E-mail ID")
		   document.getElementById(txtEmail).focus();
		   return false;
		}
		if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		   alert("Invalid E-mail ID")
		   document.getElementById(txtEmail).focus();
		   return false;
		}

		if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		    alert("Invalid E-mail ID")
		    document.getElementById(txtEmail).focus();
		    return false;
		}

		 if (EmailText.indexOf(at,(lat+1))!=-1){
		    alert("Invalid E-mail ID")
		    document.getElementById(txtEmail).focus();
		    return false;
		 }

		 if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		    alert("Invalid E-mail ID")
		    document.getElementById(txtEmail).focus();
		    return false;
		 }

		 if (EmailText.indexOf(dot,(lat+2))==-1){
		    alert("Invalid E-mail ID")
		    document.getElementById(txtEmail).focus();
		    return false;
		 }
		
		 if (EmailText.indexOf(" ")!=-1){
		    alert("Invalid E-mail ID")
		    document.getElementById(txtEmail).focus();
		    return false;
		 }
        
         document.Form1.Submit();
 		 return true;					
	   
 
   }
   
 // Created Satyendra on 13/06/07 
        
  
   
   
   // Modified By Zafar on 02-07-2007
   //salek kumar 12-06-2007
   function PatientRegistrationJS(txtPName,cmbTitle,txtMobile,txtHr,txtMin,txtEmail)
   {
        
       
        var PNameText=document.getElementById(txtPName).value;
        var Titlecmb=document.getElementById(cmbTitle).value;
        var MobileText=document.getElementById(txtMobile).value;
        var HrText=document.getElementById(txtHr).value;
        var MinText=document.getElementById(txtMin).value;
        var EmailText=document.getElementById(txtEmail).value;
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
       
        
	    var ValidNumber="0123456789";
        var IsNumberMobile=true;
        var IsNumberPhone=true;
        var IsNumberPhoneSTD=true;
        var Char;
          
        if(Titlecmb.length== 0)
        {
            
            alert("Please Select Title Name Title " );
            document.getElementById(cmbTitle).focus();
            return false;
             
        }  
	    if(PNameText.length == 0 )
        {
            
            alert("Please Enter Patient Name " );
            document.getElementById(txtPName).focus();
            return false;
             
        }
        
        // validate mobile no.
        if(MobileText.length > 0 )
        {
            if(MobileText.length < 10)
            {
            alert("Pls Enter Your Mobile No Greater than or Equal to 10 Digit");
            document.getElementById(txtMobile).value="";
            document.getElementById(txtMobile).focus();
            return false;
            }
         return false;   
        }
        
        if(HrText.length > 0 || MinText.length > 0 )
        {
            
            if(HrText.length == 0 )
            {
                
                alert("Please Enter Hours " );
                document.getElementById(txtHr).focus();
                return false;
                 
            }
             
            if(MinText.length == 0 )
            {
                
                alert("Please Enter Minutes " );
                document.getElementById(txtMin).focus();
                return false;
                 
            }   

                   
        }
        
        
        if(PhoneText.length > 0 || PhoneSTDText.length > 0 )
        {
            
            if(PhoneSTDText.length == 0 )
            {
                
                alert("Please Enter STD Code Number " );
                document.getElementById(txtSTDPhone).focus();
                return false;
                 
            }
             
            if(PhoneText.length == 0 )
            {
                
                alert("Please Enter Phone Number " );
                document.getElementById(txtPhone).focus();
                return false;
                 
            }   

                   
        }
        
        
        //e-mail ID
        if(EmailText.length > 0)
        {
            if (EmailText.indexOf(at)==-1)
		    {
    		
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }
		    if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }

		    if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		    }

		     if (EmailText.indexOf(at,(lat+1))!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.indexOf(dot,(lat+2))==-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
    		
		     if (EmailText.indexOf(" ")!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
            
        }
        
        document.Form1.Submit();
        return true;
            
            
        
   }
   
   
   
//salek kumar 11-06-2007
function CompairValidatorJS(txtName1,txtName2,msg)
{

    
    var txtName1,txtName2;
    var sText1=document.getElementById(txtName1).value;
    
    var sText2=document.getElementById(txtName2).value;
   
    if(sText1.length > 0 && sText2.length > 0)
    {
    
        if(sText1 != sText2)
        {
             alert(msg);
            document.getElementById(txtName1).focus();
            return false;
        }
    
    } 
    
    
    
}
// created satyendra 13-06-07
function HospitalRegistrationJS(txtPName,txtSTD,txtPhone,txtSTDOff,txtPhoneOff,txtEmail)
   {
        var PNameText=document.getElementById(txtPName).value;
        var STDtext=document.getElementById(txtSTD).value;
        var PhoneText=document.getElementById(txtPhone).value;
        var OfficeSTDText=document.getElementById(txtSTDOff).value;
        var OfficePhoneText=document.getElementById(txtPhoneOff).value;
        var EmailText=document.getElementById(txtEmail).value;
        var at="@"
		var dot="."
		var lat=EmailText.indexOf(at)
		var lstr=EmailText.length
		var ldot=EmailText.indexOf(dot)
                 
	    if(PNameText.length == 0 )
        {
            
            alert("Please Enter Hospital Name " );
            document.getElementById(txtPName).focus();
            return false;
             
        }
        if(STDText.length > 0 || PhoneText.length > 0 )
        {
            
            if(STDText.length == 0 )
            {
                
                alert("Please Enter STD Code Number " );
                document.getElementById(txtSTD).focus();
                return false;
                 
            }
             
            if(PhoneText.length == 0 )
            {
                
                alert("Please Enter Phone Number " );
                document.getElementById(txtPhone).focus();
                return false;
                 
            }   
   
        }
        if(OfficeSTDText.length > 0 || OfficePhoneText.length > 0 )
        {
            
            if(OfficeSTDText.length == 0 )
            {
                
                alert("Please Enter STD Code Number " );
                document.getElementById(txtSTDOff).focus();
                return false;
                 
            }
             
            if(OfficePhoneText.length == 0 )
            {
                
                alert("Please Enter Phone Number " );
                document.getElementById(txtPhoneOff).focus();
                return false;
                 
            }   
   
        }
        // Email check
        if(EmailText.length > 0)
        {
            if (EmailText.indexOf(at)==-1)
		    {
    		
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }
		    if (EmailText.indexOf(at)==-1 || EmailText.indexOf(at)==0 || EmailText.indexOf(at)==lstr){
		       alert("Invalid E-mail ID")
		       document.getElementById(txtEmail).focus();
		       return false;
		    }

		    if (EmailText.indexOf(dot)==-1 || EmailText.indexOf(dot)==0 || EmailText.indexOf(dot)==lstr){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		    }

		     if (EmailText.indexOf(at,(lat+1))!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.substring(lat-1,lat)==dot || EmailText.substring(lat+1,lat+2)==dot){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }

		     if (EmailText.indexOf(dot,(lat+2))==-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
    		
		     if (EmailText.indexOf(" ")!=-1){
		        alert("Invalid E-mail ID")
		        document.getElementById(txtEmail).focus();
		        return false;
		     }
            
        }
        
        document.Form1.Submit();
        return true;
            
            
        
   }
   
   
  // End  


function selctintoTextBox(list)
{
        var key;

         if(window.event)
              key = window.event.keyCode;     //IE
         else
              key = e.which;     //firefox
    
   
       if(key==40)
       {
        list.selectedIndex=list.selectedIndex+1;
        txtMedicine.text = list[list.selectedIndex].text;    
        } 
        if(key==38)
       {
        list.selectedIndex=list.selectedIndex-1;    
        }   
       if(key==13)
       {
        __doPostBack('__Page', '');    
        } 
        
}
function filtery(pattern, list)
{
  /*
  if the dropdown list passed in hasn't
  already been backed up, we'll do that now
  */
        
  if (!list.bak)
  {
    /*
    We're going to attach an array to the select object
    where we'll keep a backup of the original dropdown list
    */
    list.bak = new Array();
    for (n=0;n<list.length;n++)
    {
      list.bak[list.bak.length] = new Array(list[n].value, list[n].text);
    }
  }

  /*
  We're going to iterate through the backed up dropdown
  list. If an item matches, it is added to the list of
  matches. If not, then it is added to the list of non matches.
  */
  match = new Array();
  nomatch = new Array();
  for (n=0;n<list.bak.length;n++){
    if(list.bak[n][1].toLowerCase().indexOf(pattern.toLowerCase())!=-1){
      match[match.length] = new Array(list.bak[n][0], list.bak[n][1]);
    }else{
      nomatch[nomatch.length] = new Array(list.bak[n][0], list.bak[n][1]);
    }
  }

  /*
  Now we completely rewrite the dropdown list.
  First we write in the matches, then we write
  in the non matches
  */
  for (n=0;n<match.length;n++){
    list[n].value = match[n][0];
    list[n].text = match[n][1];
  }
//  for (n=0;n<nomatch.length;n++){
//    list[n+match.length].value = nomatch[n][0];
//    list[n+match.length].text = nomatch[n][1];
//  }

  /*
  Finally, we make the 1st item selected - this
  makes sure that the matching options are
  immediately apparent
  */
  
}
//



function HasClassName(objElement, strClass)
   {

   // if there is a class
   if ( objElement.className )
      {

      // the classes are just a space separated list, so first get the list
      var arrList = objElement.className.split(' ');

      // get uppercase class for comparison purposes
      var strClassUpper = strClass.toUpperCase();

      // find all instances and remove them
      for ( var i = 0; i < arrList.length; i++ )
         {

         // if class found
         if ( arrList[i].toUpperCase() == strClassUpper )
            {

            // we found it
            return true;

            }

         }

      }

   // if we got here then the class name is not there
   return false;

   }

function DoctorOneField(txtName1)
   {
        
       var txtName1;
       var sText1=document.getElementById(txtName1).value;
      
        if(sText1.length == 0 )
        {
            alert("Please Enter Doctor Name");
            document.getElementById(txtName1).focus();
            return false;
        }
        else
        {   document.Form1.Submit();
            return true;
            
        }
   }
   
    function RequiredFieldAppointment(txtpatient,txtDob,txtDate,txtDoc,lstdoc,cmbBill,cmbLedger,cmbHospital,buttonName,msg1,msg2,msg3,msg4,msg5,msg6,msg7)
   {
        
       // var  txtName1,txtName2,txtName3,buttonName,cmbref,cmbCasetype,cmbRoom,msg1,msg2,msg3;
        var  Patient=document.getElementById(txtpatient).value;
        var  DOB=document.getElementById(txtDob).value;
        var  Date=document.getElementById(txtDate).value;
        var  Doctor=document.getElementById(txtDoc).value;
        var DocName=document.getElementById(lstdoc).selectedIndex;
        //var  Department=document.getElementById(txtDep).value;
        var  Bill=document.getElementById(cmbBill).value;
        var  Ledger=document.getElementById(cmbLedger).value;
        var  Hospital=document.getElementById(cmbHospital).value;
       
	    if(Patient.length == 0 )
        {
            alert("Please Enter " + msg1);
            document.getElementById(txtpatient).focus();
            return false;
        }
        if(DOB.length == 0 )
        { 
            alert("Please Enter " + msg2);
            document.getElementById(txtDob).focus();
             return false;
         }
        if(Date.length == 0 )
        {
           alert("Please Enter " + msg3);
           document.getElementById(txtDate).focus();
            return false;
        }
        if(Doctor.length == 0 )
        {
           alert("Please Enter " + msg4);
           document.getElementById(txtDoc).focus();
            return false;
        }
        if(DocName == -1 )
        {
           alert("Please select DoctorName ");
           document.getElementById(lstdoc).focus();
            return false;
        }
//        if(Department.length == 0 )
//        {
//           alert("Please Enter " + msg5);
//           document.getElementById(txtDep).focus();
//            return false;
//        }
        if(Bill.length == 0 )
        {   alert("Please Select " + msg5);
            document.getElementById(cmbBill).focus();
             return false;
           
        }
       if(Ledger.length == 0 )
        {
            
            alert("Please Select " + msg6);
            document.getElementById(cmbLedger).focus();
             return false;
        }
        if(Hospital.length == 0 )
        {
            
            alert("Please Select " + msg7);
            document.getElementById(cmbHospital).focus();
             return false;
        }
      
            document.Form1.Submit();
            return true;
 }
   function RequiredFieldPrescription(txtpatient,txtDate,buttonName,msg1,msg2)
   {
        
       // var  txtName1,txtName2,txtName3,buttonName,cmbref,cmbCasetype,cmbRoom,msg1,msg2,msg3;
        var  Patient=document.getElementById(txtpatient).value;
        var  Date=document.getElementById(txtDate).value;
        //var Search=document.getElementById(txtSearch).value;
                      
	    if(Patient.length == 0 )
        {
            alert("Please Enter " + msg1);
            document.getElementById(txtpatient).focus();
            return false;
        }
       
        if(Date.length == 0 )
        {
           alert("Please Enter " + msg2);
           document.getElementById(txtDate).focus();
            return false;
        }
//        if(Search.length == 0 )
//        {
//           alert("Please select " + msg3);
//           document.getElementById(txtSearch).focus();
//            return false;
//        }
        
              
            document.Form1.Submit();
            return true;
 }
 
  function RequiredFieldProcedure(txtpatient,buttonName,msg1)
   {
        
       // var  txtName1,txtName2,txtName3,buttonName,cmbref,cmbCasetype,cmbRoom,msg1,msg2,msg3;
        var  Patient=document.getElementById(txtpatient).value;
        //var Search=document.getElementById(txtSearch).value;
                      
	    if(Patient.length == 0 )
        {
            alert("Please Enter " + msg1);
            document.getElementById(txtpatient).focus();
            return false;
        }
       
       
//        if(Search.length == 0 )
//        {
//           alert("Please select " + msg2);
//           document.getElementById(txtSearch).focus();
//            return false;
//        }
           document.Form1.Submit();
            return true;
 }
 
 
 //zafar 11 dec 2007
 
 function listSearch1(txt,lst,e)
           {
            
                //firefox
   
    
                Text = document.getElementById(txt).value;
                if(Text.indexOf(",") != -1)
                {
                    var arrText = Text.split(",");
                    //alert(arrText.length);
                    Text = trim(arrText[arrText.length - 1]);
                }
                              
                list1=document.getElementById(lst);
                var flag = 0;
                var pos = -1;
                
                var key;
                if(window.event)
                key = window.event.keyCode;     //IE
                else
                key = e.which; 
                
                if (key == 40)
                {
                     if(list1.selectedIndex != list1.length - 1)
                     { 
                        list1.selectedIndex = list1.selectedIndex + 1;
                     }
                }
                else if (key == 38 )
                {
                 
                     if(list1.selectedIndex != 0)
                     {
                        list1.selectedIndex = list1.selectedIndex - 1;
                     }
                }
                else if(key == 13 && list1.selectedIndex != -1)
                {
                        var str = "";
                        if(document.getElementById(txt).value.indexOf(",") != -1)
                        {
                            
                            var arrText1 = document.getElementById(txt).value.split(",");
                           
                            for(i=0 ; i<arrText1.length - 1 ; i++)
                            {
                                str = str + trim(arrText1[i]) + ", " ;
                                //alert("inside for = " + str);
                            }
                            //alert("Outside for = " + str);
                            //var Text1 = arrText[arrText.length - 1].trim();
                        }
                        
                       if(document.getElementById(txt).value.indexOf(list1.options[list1.selectedIndex].text+",") == -1 )
                       {
                            document.getElementById(txt).value = str + list1.options[list1.selectedIndex].text + ", ";
                       }
                        
                }
                else
                {
                    if(Text.length == 0 )
                    {
                        document.getElementById(lst).selectedIndex = -1; 
                        return false;
                      
                    }
               
                    for(i=0;i<list1.length;i++)
                    {   
                        var len = Text.length;
                        if(list1.options[i].text.substring(0,len) == Text)// && flag == 0)
                        {
                            document.getElementById(lst).selectedIndex = i;
                            break;
                                                                       
                        }
                        else
                        {
                          
                            document.getElementById(lst).selectedIndex = -1;
                        }               
                        
                    }
               }
                
           }
           
           
           
           // Removes leading whitespaces
function LTrim( value ) {
	
	var re = /\s*((\S+\s*)*)/;
	return value.replace(re, "$1");
	
}

// Removes ending whitespaces
function RTrim( value ) {
	
	var re = /((\s*\S+)*)\s*/;
	return value.replace(re, "$1");
	
}

// Removes leading and ending whitespaces
function trim( value ) {
	
	return LTrim(RTrim(value));
	
}

  


