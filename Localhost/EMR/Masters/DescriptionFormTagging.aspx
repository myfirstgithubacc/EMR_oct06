<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DescriptionFormTagging.aspx.cs" Inherits="EMR_DescriptionFormTagging" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <h2 id="tdName" runat="server">Form Default Setup</h2>
                </div>
                 
                <div class="col-md-8 col-sm-8 col-xs-8 text-right">
                    <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" Text="Save" ValidationGroup="Save" ToolTip="Save"
                        CssClass="btn btn-primary" Font-Bold="false" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" ToolTip="Reset"
                        CssClass="btn btn-primary" Font-Bold="false" />
                </div>

            </div>
                 <div class="row text-center">
                <asp:Label ID="lblMessage" runat="server" Text="" />
            </div>
          
            <div class="row">
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">Form<span style="color: Red">*</span></div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <asp:DropDownList Width="100%" ID="ddlForm" runat="server"
                        SkinID="DropDown">
                    </asp:DropDownList><span style="color: Red"></span>
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">Route<span style="color: Red">*</span></div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <asp:DropDownList Width="100%" ID="ddlRoute" runat="server"
                        SkinID="DropDown">
                    </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">Dose<span style="color: Red">*</span></div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <asp:TextBox ID="txtDoes" Height="25px" onkeypress="return isNumberKey(event)" Width="100%" runat="server"
                        MaxLength="6" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">Dose Unit<span style="color: Red">*</span></div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <asp:DropDownList Width="100%" ID="ddlUnit" runat="server"
                        SkinID="DropDown">
                    </asp:DropDownList>
                        </div>
                    </div>
                </div>
                
                <div class="col-md-2 col-sm-2 col-xs-12 p-t-b-5 box-col-checkbox">
                    <span>Default</span>
                    <asp:CheckBox ID="Defaultcheck" runat="server" />
                </div>
                
              
            </div>

           
  <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                   <%-- <h2 style="font-size:16px;margin:0px;padding:5px 0px;">Description Form Listing</h2>--%>
                    <asp:GridView  ID="GridViewData"  Width="100%"

                        OnRowCommand="GridViewData_RowCommand"   runat="server" AutoGenerateColumns="false">
                        <HeaderStyle Width="100%" />
                         <RowStyle Width="100%" />
                          <Columns>
                            
                            <asp:TemplateField HeaderText="Form">
                                <ItemTemplate>
                                   <asp:Label ID="LblForm" runat="server" Width="40%"  Text='<%# Eval("FormulationName") %>'></asp:Label> 

                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Route">
                                <ItemTemplate>
                                   <asp:Label ID="LblRoute" runat="server" Width="200%" Text='<%# Eval("RouteName") %>'></asp:Label> 

                                </ItemTemplate>
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="Dose">
                                <ItemTemplate>
                                    <asp:Label ID="LblDose" runat="server" Width="200%"  Text='<%# Eval("Dose") %>'></asp:Label>     

                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Dose Unit">
                                <ItemTemplate>
                                <asp:Label ID="LblUnit" runat="server" Width="200%"  Text='<%# Eval("Description") %>'></asp:Label> 

                                </ItemTemplate>
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="Default">
                                <ItemTemplate>
                                <asp:Label ID="LblDefault" runat="server" Width="200%"  Text='<%# Eval("Defaults") %>'></asp:Label> 

                                </ItemTemplate>
                            </asp:TemplateField>
                             
                              <asp:TemplateField HeaderText="">

                                <ItemTemplate>
                                   
                                    <asp:LinkButton ID="LinkButton" CommandName="EditRow" Width="20%" CommandArgument='<%# Eval("Id") %>' runat="server">Edit</asp:LinkButton>

                                </ItemTemplate>
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="">

                                <ItemTemplate>
                                   <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                                    CommandName="Del" CommandArgument='<%#Eval("Id")%>' />
                                  

                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

               </div>
            </div>

                </div>

        </ContentTemplate>
    </asp:UpdatePanel>
   
</asp:Content>

