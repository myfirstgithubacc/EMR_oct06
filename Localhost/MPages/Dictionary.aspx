<%@ Page Language="C#"  MasterPageFile="~/Include/Master/EMRMaster.master"  AutoEventWireup="true" CodeFile="Dictionary.aspx.cs" Inherits="Dictionary" Title="" %>
<%@ register src="~/Controls/Default.ascx" tagprefix="uc1" tagname="default" %>
<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

       <table width="100%" class="clsheader">
       <tr>
                <td style="padding-left:10px; width:250px;">Medical Dictionary
                        </td>
                    <td align="left"> 
                    <div>
                                  
                        <asp:HiddenField ID="hdnCurrentDate" runat="server" />
                                </div></td>
                                <td align="right">
                              <asp:Button ID="btnback" runat="server" Text="Back" SkinID="Button" OnClick="btnback_Click"  />&nbsp;&nbsp;
                                    </td>
                                    </tr>
   </table>
     <table >
     <tr>
     <td>
      <uc1:default runat="server" id="NavigationControl" />
        <asp:placeholder id="configuratorContents" runat="server"></asp:placeholder>
     </td>
    <%-- <td valign="top">
     <asp:LinkButton ID="lnkback" runat="server" Text="Back" 
             PostBackUrl="~/Editor/WordProcessor.aspx"></asp:LinkButton>
     </td>--%>
     
     </tr>
     </table>
       
        <script runat="server" language="C#">
        protected override void OnLoad(EventArgs args)
        {
	        string page = "Import";
	        if (Request.QueryString["Page"] != null)
		        page = Request.QueryString["Page"];

	        Control contents = LoadControl("~/Controls/" + page + ".ascx");
	        configuratorContents.Controls.Add(contents);
        }
        </script>
   </asp:Content> 