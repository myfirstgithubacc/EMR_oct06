<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="SMSSetup.aspx.cs" Inherits="EMR_Masters_SMSSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

  
    

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">











<style type="text/css">
    .Table
    {
        display: table;
       
    }
    .Title
    {
        display: table-caption;
        text-align: center;
        font-weight: bold;
        font-size: larger;
        
    }
    .Heading
    {
        display: table-row;
        font-weight: bold;
        text-align: center;
        
    }
    .Row
    {
        display: table-row;
      
    }
    .Cell
    {
        display: table-cell;
        border: solid;
        border-width: thin;
        padding-left: 5px;
        padding-right: 5px;
       font-size:small;
        
    }
</style>




    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />   
    
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="update" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvSMSDetails" />
           <%-- <asp:PostBackTrigger ControlID="btnSaveSMS" />--%>
            <asp:PostBackTrigger ControlID="btnUpdate" />
        </Triggers>
      <ContentTemplate>
      
      
    <table width="100%" cellpadding="0" cellspacing="0">
                <tr class="clsheader">
                    <td>
                        <table width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                </td>
                                <td align="left" style="padding-left: 10px; width: 250px;">
                                    SMS Setup
                                    
                                </td>
                                <td align="right" style="padding-right: 10px">
                                    <asp:Button ID="btnNew" SkinID="Button" runat="server" Text="New" OnClick="btnNew_OnClick"
                                        CausesValidation="false" Visible="false" />
                                   
                                    <asp:Button ID="btnUpdate" SkinID="Button" runat="server" Text="Update" OnClick="UpdateLocation_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="1" style="height: 13px; color: green; font-size: 12px;
                        font-weight: bold;">
                        <asp:Label ID="lblMessage" runat="server" Text="" />
                    </td>
                </tr>
               
                              
                                
             
            </table>
    <table cellpadding="2" cellspacing="2">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDescriptionOfUse" runat="server" SkinID="label" Text="Event Name"></asp:Label>
                                        <span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                           <asp:DropDownList ID="ddlDescriptionOfUse" runat="server" SkinID="DropDown" Width="250px" Height="25px" >
                                        </asp:DropDownList>
                                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ControlToValidate="ddlDescriptionOfUse" InitialValue="NA" ErrorMessage="Select Event Name"/>--%>
                                    </td>
                                    <td align="right" style="padding-left: 10px">
                                        <asp:Label ID="lblEventText" runat="server" SkinID="label" Text="SMS Text" ></asp:Label>
                                    </td>
                                    <td align="right" style="padding-left: 10px">
                                       <asp:TextBox ID="txtEventText" SkinID="textbox" TextMode="MultiLine" runat="server" MaxLength="250" Height="70px"
                                            Width="240px"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtEventText"
                                            Display="None" runat="server" ErrorMessage="Enter SMS Text"></asp:RequiredFieldValidator>
                                        <asp:ValidationSummary ID="ValidationSummary1" ShowMessageBox="true" ShowSummary="false" runat="server" />--%>
                                    </td>
                                    <td colspan="2" align="right" style="padding-right: 10px">
                                        <asp:Button ID="btnSaveSMS"  runat="server" Text="Save" OnClick="btnSaveSMS_OnClick" CssClass="SearchKeyBtn01" />
                                    </td>
                                     
                                    
                                    <td>
                                    <asp:Button ID="btnpasshelp" runat="server" CausesValidation="false" OnClick="btnpasshelp_Click" CssClass="QuestionBtn" Text="?" />
                                    
                                    </td>
                                </tr>
                                
                                </table>
    <table width="98%">
                <tr>
                    <td>
                    
                        <asp:GridView ID="gvSMSDetails" runat="server" AutoGenerateColumns="false" 
                                        CellPadding="4" onselectedindexchanged="gvSMSDetails_SelectedIndexChanged"  OnRowDataBound="gvSMSDetails_OnRowDataBound"
                                     OnRowCommand="gvSMSDetails_OnRowCommand"   SkinID="gridview">
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Event Name" Visible="true">
                                                <ItemTemplate>
                                                <asp:HiddenField ID="hdnIsWhiteListed" runat="server" Value='<%#Eval("IsWhiteListed") %>'  />
                                                  <asp:HiddenField ID="hdnCustomizedSMSId" runat="server" Value='<%#Eval("CustomizedSMSId") %>'  />
                                                
                                                
                                                
                                                <asp:HiddenField ID="hdnFacilityId" runat="server" Value='<%#Eval("FacilityId") %>'  />
                                                 <asp:HiddenField ID="hdnEevntName" runat="server" Value='<%#Eval("EevntName") %>'  />
                                                    <asp:Label ID="lblEventname" runat="server" Text='<%#Eval("DescriptionOfUse") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Standard SMS Text" 
                                                Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSMSStandard" runat="server" Text='<%#Eval("StandardSMSText") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Customized SMS Text" 
                                                Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSMSCostimized" runat="server" Text='<%#Eval("CustomizedSMSText") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderStyle-Width="0.3%" ItemStyle-HorizontalAlign="Center" >
                                    <ItemTemplate>
                                       <%-- <asp:HiddenField ID="hdnActive" runat="server" Value='<% #Eval("Active") %>' />--%>
                                        <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png" ToolTip="Delte Customized SMS Text"
                                            CausesValidation="false" CommandName="DeActivate" CommandArgument='<%#Eval("CustomizedSMSId")%>'  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                    </td>
                </tr>
                </table>              
    <table width="98%" cellpadding="3" cellspacing="3" >
                                    <tr>
                                        <td width="55px">
                                            <strong>Legend</strong>
                                        </td>
                                        <td id="Td1" runat="server" style="width: 40px;" bgcolor="Pink">
                                        </td>
                                        <td>
                                            <strong>&nbsp; Approval Pending</strong>
                                        </td>
                                    </tr>
                                </table>
    <div id="dvConfirmCancelOptions" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">
                        
                        <table width="100%" cellspacing="2">
                            <tr><td colspan="3" align="center"><asp:Label ID="lblConfirm" Font-Size="12px" Font-Bold="true" runat="server" Text="Do you want to delete ?"></asp:Label></td></tr>
                            <tr><td colspan="3">&nbsp;</td></tr>
                            
                            <tr>
                                <td align="center"></td>
                                <td align="center">
                                    <asp:Button ID="ButtonOk" SkinID="Button" runat="server" Text="Yes" OnClick="ButtonOk_OnClick" />&nbsp;
                                    <asp:Button ID="ButtonCancel" SkinID="Button" runat="server" Text="Cancel" OnClick="ButtonCancel_OnClick" />
                                </td>
                                <td align="center"></td>
                            </tr>
                        </table>
                    </div>
    <div   id="divtooltip" runat="server" visible="false" style="width: 400px; z-index: 2000; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 505px; left: 300px; top: 150px" >
    <div class="Title">
        <p>Key Help</p>
    </div>
    <div class="Heading">
        <div class="Cell">
            <p>Key Name</p>
        </div>
        <div class="Cell">
            <p>Description</p>
        </div>
        <div class="Cell">
            <p> Max Length</p>
        </div>
    </div>
    <div class="Row">
        <div class="Cell">
            <p><asp:Label id="lblFacilityKeyWord" runat="server"/></p>
        </div>
        <div class="Cell">
         <p><asp:Label id="lblFacilityKeyWordDescription" runat="server"/></p>
           
        </div>
        <div class="Cell">
          <p><asp:Label id="lblFacilityNameMaxLength" runat="server"/></p>
           
        </div>
    </div>
    <div class="Row">
        <div class="Cell">
         <p><asp:Label id="lblClinicPhoneKeyWord" runat="server"/></p>
           
        </div>
        <div class="Cell">
         <p><asp:Label id="lblClinicPhoneKeyWordDescription" runat="server"/></p>
           
        </div>
        <div class="Cell">
         <p><asp:Label id="lblClinicPhoneMaxLength" runat="server"/></p>
           
        </div>
    </div>
    <div class="Row">
        <div class="Cell">
         <p><asp:Label id="lblDrNameKeyWord" runat="server"/></p>
         
        </div>
        <div class="Cell">
          <p><asp:Label id="lblDrNameKeyWordDescription" runat="server"/></p>
         
        </div>
        <div class="Cell">
           <p><asp:Label id="lblDrNameMaxLength" runat="server"/></p>
            
        </div>
    </div>
    <div class="Row">
        <div class="Cell">
        <p><asp:Label id="lblDateKeyWord" runat="server"/></p>
           
        </div>
        <div class="Cell">
         <p><asp:Label id="lblDateKeyWordDescription" runat="server"/></p>
          
        </div>
        <div class="Cell">
           <p><asp:Label id="lblDateMaxLength" runat="server"/></p>
          
        </div>
    </div>
    <div class="Row">
        <div class="Cell">
         <p><asp:Label id="lblTimeKeyWord" runat="server"/></p>
          
        </div>
        <div class="Cell">
         <p><asp:Label id="lblTimeKeyWordDescription" runat="server"/></p>
            
        </div>
        <div class="Cell">
        <p><asp:Label id="lblTimeMaxLength" runat="server"/></p>
           
        </div>
    </div>
    <div class="Row">
        <div class="Cell">
         <p><asp:Label id="lblPatNameKeyWord" runat="server"/></p>
           
        </div>
        <div class="Cell">
          <p><asp:Label id="lblPatNameKeyWordDescription" runat="server"/></p>
           
        </div>
        <div class="Cell">
        <p><asp:Label id="lblPatNameMaxLength" runat="server"/></p>
         
        </div>
    </div>
    <div class="Row">
        <div class="Cell">
          <p><asp:Label id="lblRegNoKeyWord" runat="server"/></p>
          
        </div>
        <div class="Cell">
         <p><asp:Label id="lblRegNoKeyWordDescription" runat="server"/></p>
          
        </div>
        <div class="Cell">
          <p><asp:Label id="lblRegNoMaxLength" runat="server"/></p>
            
        </div>
    </div>
    
    
     <div class="Row">
        <div class="Cell">
          <p><asp:Label id="lblApmtCountKeyWord" runat="server"/></p>
          
        </div>
        <div class="Cell">
          <p><asp:Label id="lblApmtCountKeyWordDescription" runat="server"/></p>
          
          
        </div>
        <div class="Cell">
          <p><asp:Label id="lblApmtCountMaxLength" runat="server"/></p>
           
        </div>
    </div>
    
     <div class="Row">
        <div class="Cell">
          <p><asp:Label id="lblAptListKeyWord" runat="server"/></p>
          
        </div>
        <div class="Cell">
         <p><asp:Label id="lblAptListKeyWordDescription" runat="server"/></p>
          
        </div>
        <div class="Cell">
          <p><asp:Label id="lblAptListMaxLength" runat="server"/></p>
           
        </div>
    </div>
     <div class="Row">
        <div class="Cell">
          <p><asp:Label id="lblVaccinesKeyWord" runat="server"/></p>
          
        </div>
        <div class="Cell">
         <p><asp:Label id="lblVaccinesKeyWordDescription" runat="server"/></p>
          
        </div>
        <div class="Cell">
          <p><asp:Label id="lblVaccinesMaxLength" runat="server"/></p>
          
        </div>
    </div>
    
    <div >
       <table width="100%" cellspacing="2">
                          
                            <tr><td colspan="3">&nbsp;</td></tr>
                            <tr>
                                <td align="center"></td>
                                <td align="center">
                                  
                                    <asp:Button ID="btntooltipshow" CssClass="SaveBtnNew"  Width="60px"  runat="server" Text="Close" OnClick="btntooltipshow_OnClick" />
                                </td>
                             
                            </tr>
                        </table>
    </div>
   
       </div>                   
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
