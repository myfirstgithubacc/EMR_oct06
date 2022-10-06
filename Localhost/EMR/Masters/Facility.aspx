<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="Facility.aspx.cs" Inherits="EMR_Masters_Facility" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />   
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/Administration-Item.css" rel="stylesheet" type="text/css" />
    
    
    <asp:UpdatePanel ID="update" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvFacility" />
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnUpdate" />
        </Triggers>
    
    
    
        <ContentTemplate>
            
            
            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-10"><div class="OrderSetDivText"><h2>Facility</h2></div></div>
                        <div class="col-md-2">
                            <asp:Button ID="btnNew" SkinID="Button" runat="server" Text="New" OnClick="btnNew_OnClick" CausesValidation="false" Visible="false" />
                            <asp:Button ID="btnSave" SkinID="Button" runat="server" Text="Save" OnClick="SaveFacility_OnClick" Visible="false"/>
                            <asp:Button ID="btnUpdate" CssClass="VisitTypeBtn" runat="server" Text="Update" OnClick="UpdateLocation_OnClick" />
                        </div>
                    </div>
                </div>
            </div>
            
            
           <div class="AuditTrailDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="AuditMessage"><asp:Label ID="lblMessage" runat="server" Text="" /></div>
                        </div>
                        
                    </div>
                </div>
           </div>  
            
            
            
            
            
            
                <div class="ItemServiceDiv">
               
                    <div class="ItemServiceDiv">
                        <asp:Panel ID="pnlFacility" runat="server" DefaultButton="btnSave">
                            
                            <div class="ItemServiceDiv">
                                <div class="container-fluid"> 
                                    
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblFacility" runat="server" Text="Name"></asp:Label> <span class="red">*</span></h2>
                                                <h3>
                                                    <asp:TextBox ID="txtFacilityName" runat="server" CssClass="FacilityDivInput" MaxLength="250"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFacilityName" Display="None" runat="server" ErrorMessage="Enter Facility Name"></asp:RequiredFieldValidator>
                                                    <asp:ValidationSummary ID="ValidationSummary1" ShowMessageBox="true" ShowSummary="false" runat="server" />
                                                </h3>
                                            </div>
                                        </div>
                                        
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblAddress" runat="server" Text="Address1"></asp:Label></h2>
                                                <h3><asp:TextBox ID="txtAddress" runat="server" CssClass="FacilityDivInput" MaxLength="249"></asp:TextBox></h3>
                                            </div>
                                        </div>
                                        
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblAddress2" runat="server" Text="Address2"></asp:Label></h2>
                                                <h3><asp:TextBox ID="txtAddress2" runat="server" CssClass="FacilityDivInput" MaxLength="249"></asp:TextBox></h3>
                                            </div>
                                        </div>
                                        
                                        
                                        
                                        
                                        
                                        
                                        <div class="col-md-4" style="display:none;">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="Label1" runat="server" SkinID="label" Text="Report To" Visible="false"></asp:Label></h2>
                                                <h3><asp:DropDownList ID="ddlReportto" runat="server" SkinID="DropDown" Width="250px" Visible="false"></asp:DropDownList></h3>
                                            </div>
                                        </div>
                                        
                                        <div class="col-md-4" style="display:none;">
                                            <div class="FacilityDiv">
                                                <asp:CheckBox ID="chkmain" runat="server" SkinID="checkbox" Text="Main Facility" Visible="false" />&nbsp;
                                                <asp:CheckBox ID="chkActive" runat="server" SkinID="checkbox" Text="Active" Visible="false" />
                                            </div>    
                                        </div>   
                                         
                                    </div>
                                    
                                    
                                    
                            
                                    <div class="row">
                                    
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label></h2>
                                                <h3><asp:DropDownList ID="dropLCountry" runat="server" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="LocalCountry_SelectedIndexChanged" CssClass="FacilityDivDown" TabIndex="16"></asp:DropDownList></h3>
                                            </div>
                                        </div>
                                    
                                        
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblState" runat="server" Text="State"></asp:Label></h2>
                                                <h3>
                                                    <asp:DropDownList ID="dropLState" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="LocalState_SelectedIndexChanged" CssClass="FacilityDivDown" TabIndex="17">
                                                        <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </h3>
                                            </div>
                                        </div>    
                                        
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblCity" runat="server" Text="City"></asp:Label></h2>
                                                <h3>
                                                    <%--<asp:DropDownList ID="ddlCity" runat="server" SkinID="DropDown" Width="220px"></asp:DropDownList>--%>
                                                    <asp:DropDownList ID="dropLCity" runat="server" CssClass="FacilityDivDown" AppendDataBoundItems="true" TabIndex="18" AutoPostBack="true" OnSelectedIndexChanged="LocalCity_OnSelectedIndexChanged">
                                                        <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </h3>
                                            </div>
                                        </div>
                                        
                                    </div>
                                    
                                    
                                    
                                    
                                    <div class="row">
                                        
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblZip" runat="server" Text="Zip"></asp:Label></h2>
                                                <h3>
                                                    <%--<asp:DropDownList ID="ddlZip" SkinID="DropDown" runat="server" Width="140px" Font-Size="11px" AppendDataBoundItems="true" TabIndex="19"><asp:ListItem Text="[Select]" Value="0"></asp:ListItem></asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtzip" runat="server" CssClass="FacilityDivInput" TabIndex="19"/>
                                                </h3>
                                            </div>
                                        </div> 
                                        
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblTelephone" runat="server" Text="Phone #"></asp:Label></h2>
                                                <h3><asp:TextBox ID="txtTelephone" CssClass="FacilityDivInput" runat="server" Columns="10"></asp:TextBox></h3>
                                                
                                            </div>
                                        </div>
                                        
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label></h2>
                                                <h3><asp:TextBox ID="txtFax" CssClass="FacilityDivInput" runat="server" Columns="10"></asp:TextBox></h3>
                                            </div>
                                        </div>
                                           
                                    </div>
                                    
                                    
                                    
                                    <div class="row">
                                        
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2>
                                                    <asp:Label ID="lblemail" runat="server" Text="Email" />
                                                    <asp:Label ID="lblTimeZone" runat="server" Text="Time Zone" Visible="false"></asp:Label>
                                                    <%-- <span style="color: Red;">*</span>--%>
                                                </h2>
                                                <h3>
                                                    <asp:TextBox ID="txtemail" runat="server" Columns="22" CssClass="FacilityDivInput" MaxLength="100"/>
                                                    <asp:DropDownList ID="ddlTimeZone" runat="server" AppendDataBoundItems="true" CssClass="FacilityDivInput"  TabIndex="19" Visible="false"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="TimeZone_RequiredFieldValidator" CssClass="FacilityDivInput" ControlToValidate="ddlTimeZone" InitialValue="0" Display="None" runat="server" ErrorMessage="Select Time Zone" Visible="false"></asp:RequiredFieldValidator>
                                                </h3>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2>
                                                    <asp:Label ID="lblwebsite" runat="server" Text="Website" />
                                                    <asp:Label ID="lblNPI" runat="server" Text="NPI" Visible="false"></asp:Label>
                                                </h2>
                                                <h3>
                                                    <asp:TextBox ID="txtwebsite" CssClass="FacilityDivInput" runat="server" MaxLength="200" />
                                                    <asp:TextBox ID="txtNPI" SkinID="textbox" runat="server" MaxLength="20" Columns="20" Visible="false"></asp:TextBox>
                                                </h3>
                                            </div>
                                        </div>    
                                    </div>
                            
                                    <div class="row" style="display:none;">
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="Label2" runat="server" SkinID="label" Text="SMS Sender" Visible="false"></asp:Label></h2>
                                                <h3><asp:TextBox ID="txtSmsSender" runat="server" SkinID="textbox" MaxLength="10" Width="240px" Visible="false"></asp:TextBox></h3>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="lblSmsAdd" runat="server" SkinID="label" Text="SMSServer Link" Visible="false"></asp:Label></h2>
                                                <h3><asp:TextBox ID="txtLinkServer" runat="server" SkinID="textbox" MaxLength="500" Width="99%" Visible="false"></asp:TextBox></h3>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="FacilityDiv">
                                                <h2><asp:Label ID="Label3" runat="server" SkinID="label" Text="CaseNote Folder" Visible="false"></asp:Label></h2>
                                                <h3><asp:TextBox ID="txtCaseNoteFolder" runat="server" SkinID="textbox" MaxLength="250" Width="240px" Visible="false"></asp:TextBox></h3>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    
                                    <div class="row" style="display:none;">
                                        <div class="col-md-12">
                                            <%--<div class="FacilityDiv">
                                                <h2><asp:Label ID="lblPOS" runat="server" SkinID="label" Text="POS"></asp:Label> <span style="color: #FF0000">  *</span></h2>
                                                <h3>
                                                    <asp:DropDownList ID="ddlPOS" runat="server" AppendDataBoundItems="true" Font-Size="11px" SkinID="DropDown" TabIndex="19" Width="250px">
                                                        <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlPOS" InitialValue="0" Display="None" runat="server" ErrorMessage="Select POS"></asp:RequiredFieldValidator>
                                                </h3>
                                            </div>--%>
                                        </div>
                                    
                                    </div>
                            
                            
                                </div>
                            </div>    

                        </asp:Panel>
                        
                        
                    </div>                                
                 
                 
                 
                    <div class="ItemServiceDiv">
                        <div class="container-fluid"> 
                            
                            <div class="row">
                                <div class="col-md-12">
                                
                                    <asp:GridView ID="gvFacility" SkinID="gridview" CellPadding="4" runat="server" AutoGenerateColumns="false" DataKeyNames="FacilityID" ShowHeader="true" Width="100%" PageSize="13" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" ShowFooter="false" PagerSettings-Visible="true"
                                        HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" 
                                        HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" 
                                        HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" 
                                    
                                        PageIndex="0" OnRowDataBound="gvFacility_OnRowDataBound" OnRowCommand="gvFacility_OnRowCommand" OnPageIndexChanging="gvFacility_OnPageIndexChanging" HeaderStyle-HorizontalAlign="Left" OnSelectedIndexChanged="gvFacility_SelectedIndexChanged">
                                       
                                        <Columns>
                                            <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" Visible="true" ReadOnly="true" HeaderStyle-Width="15%" />
                                            <asp:BoundField DataField="NPI" HeaderText="NPI" Visible="true" ReadOnly="true" HeaderStyle-Width="10%" />
                                            <asp:BoundField DataField="POSCode" HeaderText="POSId" Visible="false" ReadOnly="true" />
                                            <asp:BoundField DataField="POSName" HeaderText="POS" Visible="false" ReadOnly="true" />
                                            <asp:BoundField DataField="Phone" HeaderText="Phone" Visible="true" ReadOnly="true" HeaderStyle-Width="8%" />
                                            <asp:BoundField DataField="Fax" HeaderText="Fax" Visible="true" ReadOnly="true" HeaderStyle-Width="8%" />
                                            <asp:BoundField DataField="Address1" HeaderText="Address1" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="CityId" HeaderText="CityId" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="CityName" HeaderText="CityName" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="StateId" HeaderText="StateId" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="StateName" HeaderText="StateName" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="CountryId" HeaderText="CountryId" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="CountryName" HeaderText="CountryName" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="PinNo" HeaderText="PinNo" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="Address2" HeaderText="Address2" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="TimeZoneId" HeaderText="TimeZoneId" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="TimeZoneOffSetMinutes" HeaderText="Time Zone OffSet Minutes" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="ReferToFacility" HeaderText="Report To Facility" Visible="true" ReadOnly="true" />
                                            <asp:BoundField DataField="MainFacility" HeaderText="Main Facility" Visible="true" ReadOnly="true" />
                                            
                                            <%-- <asp:BoundField DataField="EmailId" HeaderText="EmailId" Visible="true" ReadOnly="true"  HeaderStyle-Width="10%"/>--%>
                                            <asp:TemplateField HeaderText="Email" Visible="true" HeaderStyle-Width="10%">
                                                <ItemTemplate><asp:Label ID="lblEmailId" runat="server" Text='<%#Eval("EmailId") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="WebSite" Visible="true" HeaderStyle-Width="10%">
                                                <ItemTemplate><asp:Label ID="lblWebSite" runat="server" Text='<%#Eval("WebSite") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <%-- <asp:BoundField DataField="WebSite" HeaderText="WebSite" Visible="true" ReadOnly="true"  HeaderStyle-Width="10%"/>--%>
                                            <asp:BoundField DataField="Status" HeaderText="Status" Visible="true" ReadOnly="true" HeaderStyle-Width="8%" />
                                            <%--<asp:TemplateField HeaderText="SMS SenderName" Visible="true" HeaderStyle-Width="10%">
                                                <ItemTemplate><asp:Label ID="lblSmsSender" runat="server" Text='<%#Eval("SmsSender") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>--%>
                                            
                                           <%-- <asp:TemplateField HeaderText="SMSServer Link" Visible="true" HeaderStyle-Width="30%">
                                                <ItemTemplate><asp:Label ID="lblSmsServer" runat="server" Text='<%#Eval("SmsServer") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <%--  <asp:TemplateField HeaderText="CaseNote Folder" Visible="true" HeaderStyle-Width="30%">
                                                <ItemTemplate><asp:Label ID="lblCaseNote" runat="server" Text='<%#Eval("CaseNotePath") %>'></asp:Label></ItemTemplate>
                                            </asp:TemplateField>--%>
                                            
                                           <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center" Visible="false">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnActive" runat="server" Value='<% #Eval("Active") %>' />
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png" CausesValidation="false" CommandName="DeActivate" CommandArgument='<%#Eval("FacilityID")%>'  Visible="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            <asp:CommandField ShowSelectButton="true" HeaderStyle-Width="5%" ItemStyle-ForeColor="Blue"/>
                                        </Columns>
                                    </asp:GridView>
                                    
                                </div>
                            </div>
                        </div>
                        
                        
                        
                    </div>   
                
                
               </div>         

            
            
            
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
