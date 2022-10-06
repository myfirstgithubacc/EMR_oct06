<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreView.aspx.cs" Inherits="EMRBILLING_Report_PreView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>--%>
    
    <script src="../../Include/JS/jquery1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript">

        $(document).ready(function() {
            if ($.browser.mozilla) {
                try {
                    var ControlName = 'ReportViewer1';
                    var innerScript = '<scr' + 'ipt type="text/javascript">document.getElementById("' + ControlName + '_print").Controller = new ReportViewerHoverButton("' + ControlName + '_print", false, "", "", "", "#ECE9D8", "#DDEEF7", "#99BBE2", "1px #ECE9D8 Solid", "1px #336699 Solid", "1px #336699 Solid");</scr' + 'ipt>';
                    var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&Version=9.0.30729.1&Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
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

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:Label ID="msg" runat="server"></asp:Label>
    </div>
    <div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="500px" Width="100%">
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
