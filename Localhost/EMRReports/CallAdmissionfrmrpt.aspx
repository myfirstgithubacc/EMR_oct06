<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CallAdmissionfrmrpt.aspx.cs" Inherits="EMRReports_CallAdmissionfrmrpt" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="aspl" TagName="ComboPatientSearch" Src="~/Include/Components/PatientSearchCombo.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/jscript" language="javascript">

            $(document).ready(function() {
                if ($.browser.mozilla) {
                    try {
                        var ControlName = 'ReportViewer1';
                        var innerScript = '<scr' + 'ipt type="text/javascript">document.getElementById("' + ControlName + '_print").Controller = new ReportViewerHoverButton("' + ControlName + '_print", false, "", "", "", "#ECE9D8", "#DDEEF7", "#99BBE2", "1px #ECE9D8 Solid", "1px #336699 Solid", "1px #336699 Solid");</scr' + 'ipt>';
                        var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                        var innerTable = '<table title="Print" onmouseout="this.Controller.OnNormal();" onmouseover="this.Controller.OnHover();" onclick="PrintFunc(\'' + ControlName + '\'); return false;" id="' + ControlName + '_print" style="border: 1px solid rgb(236, 233, 216); background-color: rgb(236, 233, 216); cursor: default;">' + innerScript + innerTbody + '</table>'
                        var outerScript = '<scr' + 'ipt type="text/javascript">document.getElementById("' + ControlName + '_print").Controller.OnNormal();</scr' + 'ipt>';
                        var outerDiv = '<div style="display: inline; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + outerScript + '</td></tr></tbody></table></div>';

                        $("#" + ControlName + " > div > div").append(outerDiv);

                    }
                    catch (e) { alert(e); }
                }
            });


            function PrintFunc(ControlName) {
                setTimeout('ReportFrame' + ControlName + '.print();', 100);
            }

    
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="500px" Width="95%">
                    </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
