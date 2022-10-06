<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DischargeOutlier.aspx.cs" Inherits="WardManagement_DischargeOutlier" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Discharge Outlier</title>
   <%-- <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />--%>

     <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />   
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
</head>

<script type="text/javascript">
    if (window.captureEvents) {
        window.captureEvents(Event.KeyUp);
        window.onkeyup = executeCode;
    }
    else if (window.attachEvent) {
        document.attachEvent('onkeyup', executeCode);
    }

    function executeCode(evt) {
        if (evt == null) {
            evt = window.event;
        }
        var theKey = parseInt(evt.keyCode, 10);
        switch (theKey) {

            case 113: //F2
                $get('<%=btnSave.ClientID%>').click();
                break;

            case 119:  // F8
                $get('<%=btnclose.ClientID%>').click();
                break;
        }
        evt.returnValue = false;
        return false;
    }
</script>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-md-2"><h2>Discharge Outlier</h2></div>
            <div class="col-md-10 text-center"><asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" /></div>
        </div>

     <%--   <table cellspacing="0" class="table table-small-font table-bordered table-striped">
            <tbody>
                <tr align="left">
                    <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                </tr>    
            </tbody>
        </table>--%>




        <div class="container-fluid" id="Table1" runat="server">
            <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
            <div class="row margin_Top">
                        </div>
                       <div class="row margin_Top">
                        </div>
                       <div class="row margin_Top">
                        </div>
                       <div class="row margin_Top">
                        </div>
                       <div class="row margin_Top">
                        </div>
                       <div class="row margin_Top">
                        </div>
                       <div class="row margin_Top">
                        </div>
            <div class="row" id="Table2" runat="server">
                <div class="col-md-offset-4 col-md-4">
                    <div class="row form-groupTop01">
                        <div class="col-md-5 label2"><asp:Label ID="Lable1" runat="server" Text="Discharge&nbsp;Outlier&nbsp;Remarks&nbsp;:" /></div>
                        <div class="col-md-7">
                            <telerik:RadComboBox ID="ddldischargeOutlierstatus" runat="server" Width="100%"></telerik:RadComboBox>
                            
                        </div>
                    </div>

                    <div class="row margin_Top">
                        <div class="col-md-5 label2">
                            <asp:Label ID="lblentered" runat="server" Visible="false" Text="Entered&nbsp;by&nbsp;:" />
                            </div>
                         <div class="col-md-7">
                        <asp:Label ID="lblenteredby" Visible="false" runat="server"></asp:Label>
                              </div>
                        </div>
                       <div class="row margin_Top">
                        </div>
                       <div class="row margin_Top">
                            
                        </div>
                      
                    <div class="row margin_Top">
                        <div class="col-md-5"></div>
                        <div class="col-md-7">
                            <asp:Button ID="btnSave" runat="server" Text="Save (Ctrl+F2)" CssClass="btn btn-primary" ValidationGroup="S" OnClick="btnSave_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close (Ctrl+F8)" CssClass="btn btn-default" CausesValidation="false" OnClientClick="window.close();" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>