<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RisTextReport.aspx.cs" Inherits="RisTextReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RIS Report</title>
    <link href="/Include/New/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="../Include/css/mainNew.css" />
    <link rel="stylesheet" href="../Include/EMRStyle.css" />
    <style>

        * { font-family: Arial !important;}
        p, span {font-size: 12px !important;}

        .MsoNormal em { font-style: normal;}

        .MsoNoSpacing {
            text-align: left;
        }


        #ShowData h2 {    
    text-indent: 0 !important;
    margin: 0 !important;
    text-align: left !important;
    color: #0e85ea !important;
                    padding:0 10px 0 0;
                    display: block;
}

        #ShowData h2 u { text-decoration:  none; font-family: Arial; font-weight: bold;}

        #ShowData {
            background: #fbfbfb;
            margin: auto;
            font-family: Arial;
            width: 100%;
            border: 6px solid #fff;
            outline: 1px dashed #e2e2e2;
            padding: 20px;
            text-align: left;
            font-size: 14px;
            font-weight: bold;
        }

        #ShowData ul {list-style-position: inside;}

        .MsoNormal { display: block; padding: 5px 0; }

            #ShowData div {
                margin-top: 20px;
            }

            #ShowData p {
                margin: auto !important;
                font-weight: normal;
                padding: 0;
                text-align: left !important;
                text-indent: 0 !important;
            }

            p.MsoNormal br { display: none;}





                #ShowData p strong u span {
                    /*background: #0e85ea;*/
                    color: #0e85ea !important;
                    padding:0 10px 0 0;
                    display: block;
                }

                p.MsoNormal strong u span { margin-bottom: 5px;}

                #ShowData p strong u {
                    text-decoration: none;
                }

                #ShowData p u {
                    color: #fff;
                }
    </style>
    <script type="text/javascript">
        function returnToParent() {
            debugger;
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            //oArg.ServiceId = document.getElementById("hdnServiceId").value;
            oArg.RISData = document.getElementById("hdnRisdata").value;

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true" />
        <asp:UpdatePanel ID="udpdateofdeath" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <label>From Date: </label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadDatePicker ID="dtpFromdate" Width="100px" runat="server" DateInput-DateFormat="dd/MM/yyyy" DateInput-DateDisplayFormat="dd/MM/yyyy" MinDate="01/01/1900">
                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <label>To Date: </label>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadDatePicker ID="dtpTodate" Width="100px" runat="server" DateInput-DateFormat="dd/MM/yyyy" DateInput-DateDisplayFormat="dd/MM/yyyy" MinDate="01/01/1900">
                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6 col-sm-6 col-xs-12 text-right">
                            <asp:Button ID="btnGetReport" runat="server" CssClass="btn btn-primary" Text="Filtter" OnClick="btnGetReport_Click" />
                            <asp:Button ID="btnUpdateSummary" runat="server" CssClass="btn btn-primary" Text="Update Summary" OnClick="btnUpdateSummary_Click" />
                        </div>
                    </div>

                    <div runat="server" id="ShowData" class="row text-left">
                    </div>

                </div>

                <asp:HiddenField ID="hdnRisdata" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
