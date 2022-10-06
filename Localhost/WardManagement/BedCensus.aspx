<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="BedCensus.aspx.cs" Inherits="WardManagement_BedCensus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" /> 
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>   
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
     <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    
    <style>#ctl00_ContentPlaceHolder1_lblMessage{color:red!important}</style>
    

   
   
    <style type="text/css">
        .taborderbutton { background-image: url(/Images/orders.jpg); background-repeat: repeat-x; height: 22px; text-align: center;}
        .tabmidbuttonactive { background-image: url(/Images/Butt.png); background-repeat: no-repeat; color: Black; height: 22px; text-align: center;}
        .blink { text-decoration: blink;}
        .blinkNone { text-decoration: none;}
    </style>
    
    

     <script type="text/javascript">

      
           function SelectAllFavourite(id) {
             //get reference of GridView control
             var grid = document.getElementById("<%=gvFavorites.ClientID%>");
             //variable to contain the cell of the grid
             var cell;
             if (grid.rows.length > 0) {
                 //loop starts from 1. rows[0] points to the header.
                 for (ridx = 1; ridx < grid.rows.length; ridx++) {
                     //get the reference of first column
                     cell = grid.rows[ridx].cells[0];

                     //loop according to the number of childNodes in the cell
                     for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                         //if childNode type is CheckBox
                         if (cell.childNodes[cIdx].type == "checkbox") {
                             //assign the status of the Select All checkbox to the cell checkbox within the grid
                             cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                         }
                     }
                 }
             }
         }

        

       

    </script>


    <style type="text/css">
        .Gridheader { font-family: Verdana; background-image: url(/Images/header.gif); height: 24px; color: black; font-weight: normal; position: relative;}
    </style>
    
    <script type="text/javascript">
        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false)
                { return false; }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.className += " PatientBtn01";
                myButton.value = "Save (Ctrl+F3)";

                //display message
                //document.getElementById("message-div").style.display = "block";
                //document.getElementById("divbtnSaveConfirm").style.display = "none";

            }
            return true;
        }
    </script>
    
    
    
<%--    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
        <ContentTemplate>--%>
            
            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                    
                        <div class="col-md-2 col-sm-2"><div class="WordProcessorDivText"><h2>Ward Census</h2></div></div>
                       <div class="col-md-5 col-sm-5"><div class="WordProcessorDivText" style="color:red;"><h4 style="	color:red!important;">               <asp:Label ID="lblMessage" runat="server" SkinID="label" /></h4></div></div>
                        <div class="col-md-5 col-sm-5">
                            <span class="orderPop"><asp:CheckBox ID="chkAllergyReviewed" runat="server" TabIndex="6" Text="Allergy&nbsp;List&nbsp;Reviewed"  Visible="false"/></span>
                            
                            <asp:Button ID="btnPrint" runat="server" CssClass="PatientBtn01" OnClick="btnPrint_Click" Text="Print (Ctrl+F9)" />
                          
                          
                            <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                        </div>
                    </div>
                </div>                    
            </div>                    
            
            
            
            
            
        
                
            <%-- <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px; border-style: solid none solid none; border-width: 1px; border-color: #808080;" cellpadding="2" cellspacing="2" width="100%">
                    <tr><td><asp:Label ID="Label4" runat="server" Text="" Font-Bold="true" Visible="false"></asp:Label></td></tr>
            </table>--%>
            
            
            <asp:Panel ID="pnlAllCtrl" runat="server">
            
                <div class="ImmunizationDD-Div">
                    <div class="container-fluid">
                        <div class="row">
                        
                            <div class="col-md-4">
                                <asp:Panel ID="Panel2" runat="server">
                                
                                    <div class="orderPopLeftPart">
                                        <h2><asp:Label ID="Label4" runat="server" Text="Search" /></h2>
                                        <h3><asp:TextBox ID="txtSearchFavrioute" runat="server" OnTextChanged="txtSearchFavrioute_OnTextChanged" AutoPostBack="true" /></h3>
                                        <h4><asp:ImageButton ID="btnProceedFavourite" runat="server" ToolTip="Click here to proceed selected Ward" ImageUrl="~/Images/Login/orrange-arrow.GIF" Width="18px" Height="20px" OnClick="btnProceedFavourite_OnClick" /></h4>
                                    </div>
                                
                                
                                    <div class="orderPopLeftPart">
                                        <asp:Panel ID="pnlFavorites" runat="server" ScrollBars="Auto" Width="100%">
                                            <asp:GridView ID="gvFavorites" runat="server" AutoGenerateColumns="false" DataKeyNames="WardId" Autopostback="true" HeaderStyle-HorizontalAlign="Left" SkinID="gridviewOrder" Width="100%" Style="margin-bottom: 0px" OnRowCommand="gvFavorites_OnRowCommand" OnRowDataBound="gvFavorites_OnRowDataBound" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"  HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" PageSize="101" OnPageIndexChanging="gvFavorites_PageIndexChanging" AllowPaging="True">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top">
                                                        <HeaderTemplate><asp:CheckBox ID="chkAll" runat="server" /></HeaderTemplate>
                                                        <ItemTemplate><asp:CheckBox ID="chkRow" runat="server" /></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ward" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkFAV" runat="server" Font-Size="12px" Font-Bold="false" CommandName="FAVLIST" Text='<%#Eval("WardName")%>' />
                                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("WardId")%>'/>
                                                         </ItemTemplate>
                                                    </asp:TemplateField>
                                                   
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                
                                
                              
                                    
                                </asp:Panel>
                                
                                
                            </div>
                            
                            
                            

                                
                                
                               
                                
                                
                            </asp:Panel>
                            
                            </div>
                        
                        
                        </div>
                    </div>
                </div>
                
                
                
                
                
                
             
                
                <asp:Panel ID="Panel1" runat="server">
                    <div id="divDelete" runat="server" visible="false" style="width: 250px; z-index: 100; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; background-color: #FFF8DC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; position: absolute; bottom: 0; height: 75px; left: 470px; top: 355px">
                        <table width="100%" border="0">
                            <tr><td colspan="2" align="center"><asp:Label ID="lblTitle" runat="server" Text="Do you want to delete ?" SkinID="label" Font-Bold="true" Font-Size="Small" /></td></tr>
                            <tr><td colspan="2">&nbsp;</td></tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_OnClick" SkinID="Button" Width="60px" />
                                    <asp:HiddenField ID="hdnUpdateServiceId" runat="server" />
                                    <asp:HiddenField ID="hdnUpdateOrderDtlId" runat="server" />
                                </td>
                                <td>&nbsp;<asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnNo_OnClick" SkinID="Button" Width="60px" /></td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </asp:Panel>
     <%--   </ContentTemplate>
    </asp:UpdatePanel>--%>
    
    
   
    
                   <telerik:RadWindowManager ID="RadWindowManager" ShowContentDuringLoad="false"  EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForReport" runat="server" Behaviors="Default" />
                                        </Windows>
                                    </telerik:RadWindowManager>

    
      <asp:HiddenField ID="hdPageFlag" runat="server"  />
</asp:Content>
