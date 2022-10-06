<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ViewDoctorProgressNote.aspx.cs" Inherits="EMR_Dashboard_ViewDoctorProgressNote" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div class="container-fluid1">
        <div class="row head-content1">
            <%--<div class="col-xs-12"></div>--%>

            <div class="col-xs-6">
                <asp:Label ID="lblLastEncounterDate" runat="server" Text="" Font-Bold="false" CssClass="visit-date" />
                <asp:Panel ID="Panel8" runat="server" ScrollBars="Auto">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <%--SaveToClose--%>
                            <asp:HiddenField ID="hdnSaveToClose" runat="server" Value="0" />
                          <asp:Label ID="lblDoctor" runat="server" SkinID="label"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
            <%--<div class="col-xs-4"></div>--%>
            <div class="col-xs-6 text-right">
              
                <asp:Button ID="btnClose" runat="server" Text="X" Font-Bold="true" CssClass="btn btn-primary" OnClientClick="returnToParent()" />

            </div>





        </div>
        <!--row1-->
        <hr />
        <div class="scroller-content1">
        <asp:Label ID="lblDetails" runat="server"></asp:Label>

        </div>
    </div>
     <script type="text/javascript">

          function returnToParent() {
  
            //create the argument that will be returned to the parent page
            var oArg = new Object();
             var oArg = new Object();
             oArg.SaveToClose = $get('<%=hdnSaveToClose.ClientID%>').value;
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
</asp:Content>

