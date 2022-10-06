<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CurrentPatientLocation.aspx.cs" Inherits="WardManagement_CurrentPatientLocation" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Current Patient Location</title>
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
<body>
    <form id="form1" runat="server">
        
    <div>
     <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>
     <div class="container-fluid header_main">
            <div class="col-md-2"><h2>Current Patient Location</h2></div>
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
                    
                    <div class="row form-groupTop01" id="trEdod" runat="server">
                        <div class="col-md-5 PaddingRightSpacing label2"><asp:Label ID="Label1" runat="server" Text="Current Location "></asp:Label><span style="color: Red">*</span></div>
                        <div class="col-md-7">

                           <b> <asp:Label ID="Label2" runat="server"></asp:Label></b>

                        </div>

                    </div>
                        <div class="row form-groupTop01" id="Div1" runat="server">
                        <div class="col-md-5 PaddingRightSpacing label2"><asp:Label ID="Label3" runat="server" Text="New Location "></asp:Label><span style="color: Red">*</span></div>
                        <div class="col-md-7">

                             <telerik:RadComboBox ID="ddlLocationName" runat="server" EmptyMessage="Select Location Name" Skin="Simple" Width="100%">
                                        </telerik:RadComboBox>

                        </div>

                    </div>
                    <div class="row margin_Top">
                        <div class="col-md-5 label2">
                            <asp:Label ID="lblentered" runat="server" Visible="false" Text="Update Location " />
                            </div>
                         <div class="col-md-7">
                      
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
                           
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
