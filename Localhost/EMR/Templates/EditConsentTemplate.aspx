<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditConsentTemplate.aspx.cs" Inherits="EMR_Templates_EditConsentTemplate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html lang="en">
<head runat="server"> 


    <title></title>
        <script src="../../Scripts/wgssSigCaptX.js" type="text/javascript"></script>
    <script src="../../Scripts/base64.js" type="text/javascript"></script>
   
<style type="text/css">
	input[type='text'],input[type='email'],input[type='date'],input[type='time'],input[type='tel'] { border: 0; border-bottom: 1px solid #ccc; padding: 5px;}
	input[type='text']:focus,input[type='date']:focus,input[type='time']:focus,input[type='tel']:focus {  outline: 0;}

    .postion_fixed { position: fixed;
    background: #fff;
    width: 100%;
    top: 0;
    right: 0;
    padding: 5px 10px;
    text-align: right;
    z-index: 99;
    }

        .divmargin {
            position: absolute; top: 40px;
        }
    .btn-blue { color: #fff; background-color: #337ab7;     display: inline-block;
    padding: 2px 12px;
    margin-bottom: 0;
    font-size: 14px;
    font-weight: 400;
    line-height: 1.42857143;
    text-align: center;
    white-space: nowrap;    
    cursor: pointer;   
    border: 1px solid transparent;
    border-radius: 4px;}

     body {font-family: Arial !important; font-size: 14px !important; line-height: 18px !important;}
   ol li { margin-top: 5px;}
   table tr td { font-size: 14px;}
   P {
    font-family: Arial, Helvetica, sans-serif;
    font-size: 14px !important;
    line-height: 16px;
    color: black !important;
    margin-top: 3pt;
    margin-bottom: 2pt;
}
   
</style>

</head>
<body>             
    <form id="form1" runat="server">
          <div class="row postion_fixed">
                        <div class="col-md-12">

                            <%--yogesh--%>
                            <br />
                            <br />
                                  <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                                 <asp:Button ID="btnSave" runat="server" Text="Save as Draft" OnClientClick="showpatient();" OnClick="btnSave_Click1" CssClass="btn-blue" />
          <asp:Button ID="btnFinalized" runat="server" Text="Save as Finalized"  OnClientClick="showpatient();" OnClick="btnFinalized_Click" CssClass="btn-blue" />
          <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn-blue" OnClientClick="printData();"/>

                            <%--yogesh--%>
                             <div id="google_translate_element"> </div>

                            </div>
         </div>
    <div id="Form12" runat="server" class="divmargin">
     
        </div>
       <textarea cols="125" rows="100" style="display:none" id="txtDisplay"></textarea>
                <asp:HiddenField ID="hdntext" runat="server" />
     
     
        <script type="text/javascript">
         
            function returnToParent(From) {
                window.opener.location = "TemplateNotesPrint.aspx";
                window.close();
            }
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }
            function printData() {
                var divToPrint = document.getElementById("Form12");
                newWin = window.open("");
                newWin.document.write('<html><head><title></title>');
                newWin.document.write('<style type="text/css">@media print { * { -webkit-print-color-adjust: exact; print-color-adjust: exact; border-collapse: collapse; } table, table td { border: 0; }  table th,  table td { padding: 5px;} input { border: 0;border-bottom: 1px solid #ccc;} span.costtext { position: absolute; margin-top: -20px;}</style>');
                newWin.document.write('</head><body >');
                newWin.document.write(divToPrint.outerHTML);
                newWin.document.write('</body></html>');
                newWin.print();
                newWin.close();
            }
            
         function showpatient() {
             var elems = document.getElementById("Form12").getElementsByTagName("input");
    for (var i = 0; i < elems.length; i++) {
        if (elems[i].type == "checkbox") {
            if (elems[i].checked) {
                elems[i].setAttribute('checked', 'checked');
            }
        }
        else {
            elems[i].setAttribute("value", elems[i].value);
        }
    }
    document.getElementById("hdntext").value = document.getElementById("Form12").innerHTML;
}

      function DisplaySignatureDetails1(id) {
            //document.getElementById("hdnTextboxId").value = id
            captureSignature1('P', 'F', id);
        }
    
        //This prints output to the text field on the page
        function print(txt) {
            var txtDisplay = document.getElementById("txtDisplay");
            if ("CLEAR" == txt) {
                txtDisplay.value = "";
            }
            else {
                txtDisplay.value += txt + "\n";
                txtDisplay.scrollTop = txtDisplay.scrollHeight; // scroll to end
            }
        }
       
        var wgssSignatureSDK;
        var sigObj = null;
        var sigCtl = null;
        var dynCapt = null;
        var l_name = null;
        var l_reason = null;
        var l_imageBox = null;

        //Assumes the default host / port for sig captX (localhost, 8000). Checks for sigcapt service.  Called from Default.aspx.cs
        function startSession() {
            print("Detecting SigCaptX");
      
            wgssSignatureSDK = new WacomGSS_SignatureSDK(onDetectRunning, 8000);

            function onDetectRunning() {
                print("SigCaptX detected");
                clearTimeout(timeout);
            }

            var timeout = setTimeout(timedDetect, 1500);
            function timedDetect() {
                if (wgssSignatureSDK.running) {
                    print("SigCaptX detected");
                }
                else {
                    if (wgssSignatureSDK.service_detected) {
                        print("SigCaptX service detected, but not the server");
                    }
                    else {
                        print("SigCaptX service not detected");
                    }
                }
            }
        }

        //Reset the session for signature capture
        function restartSession(callback) {
            //First, reset the objects 
            wgssSignatureSDK = null;
            sigCtl = null;
            sigObj = null;
            dynCapt = null;

            var timeout = setTimeout(timedDetect, 1500);

            //Startup the SDK - assumes default port
            wgssSignatureSDK = new WacomGSS_SignatureSDK(onDetectRunning, 8000);

            function timedDetect() {
                if (wgssSignatureSDK.running) {
                    print("Signature SDK Service detected.");
                    start();
                }
                else {
                    print("Signature SDK Service not detected.");
                }
            }


            function onDetectRunning() {
                if (wgssSignatureSDK.running) {
                    print("Signature SDK Service detected.");
                    clearTimeout(timeout);
                    start();
                }
                else {
                    print("Signature SDK Service not detected.");
                }
            }

            function start() {
                if (wgssSignatureSDK.running) {
                    sigCtl = new wgssSignatureSDK.SigCtl(onSigCtlConstructor);
                }
            }

            function onSigCtlConstructor(sigCtlV, status)
            {
               if(wgssSignatureSDK.ResponseStatus.OK == status)
               {
                 // Insert license here:
                 sigCtl.PutLicence("AgAkAEy2cKydAQVXYWNvbQ1TaWduYXR1cmUgU0RLAgKBAgJkAACIAwEDZQA", onSigCtlPutLicence);
               }
               else
               {
                  print("SigCtl constructor error: " + status);
               }
            }

            function onSigCtlPutLicence(sigCtlV, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    dynCapt = new wgssSignatureSDK.DynamicCapture(onDynCaptConstructor);
                }
                else {
                    print("SigCtl constructor error: " + status);
                }
            }

            function onDynCaptConstructor(dynCaptV, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    sigCtl.GetSignature(onGetSignature);
                }
                else {
                    print("DynCapt constructor error: " + status);
                }
            }

            function onGetSignature(sigCtlV, sigObjV, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    sigObj = sigObjV;
                    sigCtl.GetProperty("Component_FileVersion", onSigCtlGetProperty);
                }
                else {
                    print("SigCapt GetSignature error: " + status);
                }
            }

            function onSigCtlGetProperty(sigCtlV, property, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    print("DLL: flSigCOM.dll  v" + property.text);
                    dynCapt.GetProperty("Component_FileVersion", onDynCaptGetProperty);
                }
                else {
                    print("SigCtl GetProperty error: " + status);
                }
            }

            function onDynCaptGetProperty(dynCaptV, property, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    print("DLL: flSigCapt.dll v" + property.text);
                    print("Test application ready.");
                    print("Press 'Start' to capture a signature.");
                    if ('function' === typeof callback) {
                        callback();
                    }
                }
                else {
                    print("DynCapt GetProperty error: " + status);
                }
            }
        }

        //Capture the first signature with the specified name and reason fields
        function captureSignature1(name, reason, imageBox) {
            l_name = name;
            l_reason = reason;
            l_imageBox = imageBox;
            Capture();
        }

       
        //Capture the first signature with the specified name and reason field
        //Do the capture
        function Capture() {

            if(wgssSignatureSDK == null || !wgssSignatureSDK.running || null == dynCapt)
            {
                print("Session error. Restarting the session.");
                restartSession(window.Capture);
                return;
            }
            dynCapt.Capture(sigCtl, l_name, l_reason, null, null, onDynCaptCapture);

            function onDynCaptCapture(dynCaptV, SigObjV, status) {
                if (wgssSignatureSDK.ResponseStatus.INVALID_SESSION == status) {
                    print("Error: invalid session. Restarting the session.");
                    restartSession(window.Capture);
                }
                else {
                    if (wgssSignatureSDK.DynamicCaptureResult.DynCaptOK != status) {
                        print("Capture returned: " + status);
                    }
                    switch (status) {
                        case wgssSignatureSDK.DynamicCaptureResult.DynCaptOK:
                            sigObj = SigObjV;
                            print("Signature captured successfully");
                            //Produce a bitmap image with steganograpics
                            var flags = wgssSignatureSDK.RBFlags.RenderOutputBase64 |
                                        wgssSignatureSDK.RBFlags.RenderEncodeData |
                                        wgssSignatureSDK.RBFlags.RenderColor24BPP;

                            var imageBox = document.getElementById(l_imageBox);
                            sigObj.RenderBitmap("bmp", imageBox.clientWidth, imageBox.clientWidth, 0.7, 0x00000000, 0x00FFFFFF, flags, 0, 0, onRenderBitmap);
                            break;
                        case wgssSignatureSDK.DynamicCaptureResult.DynCaptCancel:
                            print("Signature capture cancelled");
                            break;
                        case wgssSignatureSDK.DynamicCaptureResult.DynCaptPadError:
                            print("No capture service available");
                            break;
                        case wgssSignatureSDK.DynamicCaptureResult.DynCaptError:
                            print("Tablet Error");
                            break;
                        case wgssSignatureSDK.DynamicCaptureResult.DynCaptIntegrityKeyInvalid:
                            print("The integrity key parameter is invalid (obsolete)");
                            break;
                        case wgssSignatureSDK.DynamicCaptureResult.DynCaptNotLicensed:
                            print("No valid Signature Capture licence found");
                            break;
                        case wgssSignatureSDK.DynamicCaptureResult.DynCaptAbort:
                            print("Error - unable to parse document contents");
                            break;
                        default:
                            print("Capture Error " + status);
                            break;
                    }
                }
            }
        }

        ///Called when the signature image is received 
        function onRenderBitmap(sigObjV, bmpObj, status) 
        {
            if(wgssSignatureSDK.ResponseStatus.OK == status) 
            {
                var imageBox = document.getElementById(l_imageBox);
                if(null == imageBox.firstChild)
                {
                    imageBox.appendChild(bmpObj.image);
                }
                else
                {
                    imageBox.replaceChild(bmpObj.image, imageBox.firstChild);
                }
            } 
            else 
            {
                print("Signature Render Bitmap error: " + status);
            }
      
            //And get the base64 too
            sigObj.GetSigText(onGetText);
        }

        //Generate a random file name - in production, the filename could be set to transaction ID
        function guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();
        }

        ///Called when the signature text is received uploads the FSS to the server and writes out the file
        function onGetText(sigObjV, text, status) {
            var name = guid();

            //$.ajax({
            //    type: 'POST',
            //    url: 'Default.aspx/ReceivedSignatureText',
            //    data: '{ signature: "' + text + '", guid: "' + name + '" }',
            //    contentType: 'application/json; charset=utf-8',
            //    dataType: 'json',
            //    success: function (msg) {
            //    }
            //});

            print("Sent " + name + ".txt to server as BASE64 encoded FSS");
        }

        function DisplaySignatureDetails() {
            if (!wgssSignatureSDK.running || null == sigObj) {
                print("Session error. Restarting the session.");
                restartSession(window.DisplaySignatureDetails);
                return;
            }
            sigObj.GetIsCaptured(onGetIsCaptured);

            function onGetIsCaptured(sigObj, isCaptured, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    if (!isCaptured) {
                        print("No signature has been captured yet.");
                        return;
                    }
                    sigObj.GetWho(onGetWho);
                }
                else {
                    print("Signature GetWho error: " + status);
                    if (wgssSignatureSDK.ResponseStatus.INVALID_SESSION == status) {
                        print("Session error. Restarting the session.");
                        restartSession(window.DisplaySignatureDetails);
                    }
                }
            }

            function onGetWho(sigObjV, who, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    print("  Name:   " + who);
                    var tz = wgssSignatureSDK.TimeZone.TimeLocal;
                    sigObj.GetWhen(tz, onGetWhen);
                }
                else {
                    print("Signature GetWho error: " + status);
                }
            }

            function onGetWhen(sigObjV, when, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    print("  Date:   " + when.toString());
                    sigObj.GetWhy(onGetWhy);
                }
                else {
                    print("Signature GetWhen error: " + status);
                }
            }

            function onGetWhy(sigObjV, why, status) {
                if (wgssSignatureSDK.ResponseStatus.OK == status) {
                    print("  Reason: " + why);
                }
                else {
                    print("Signature GetWhy error: " + status);
                }
            }

        }
  </script>
    </form>
    
          
    <%--yogesh--%>
            <script type="text/javascript">
function googleTranslateElementInit() {
  new google.translate.TranslateElement({pageLanguage: 'en'}, 'google_translate_element');
    }
    </script>
    



<script type="text/javascript" src="//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>

        
        
</body>
</html>
