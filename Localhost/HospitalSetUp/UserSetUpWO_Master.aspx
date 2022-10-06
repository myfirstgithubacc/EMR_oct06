<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserSetUpWO_Master.aspx.cs" Inherits="HospitalSetUp_UserSetUpWO_Master" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
     <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="_ScriptManager" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>

                <script language="javascript" type="text/javascript">
                    function Tab() {
                        if (event.keyCode == 13)
                            event.keyCode = 9;
                    }
                    function MultilineMaxNumber(txt, maxLen) {
                        try {
                            if (txt.value.length > (maxLen - 1)) {
                                alert("Only  " + maxLen + " Characters Alowed");
                                return false;
                            }
                        } catch (e) {
                            alert(e);
                        }
                    }
                </script>

                
                
                
                
                
               
               
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="chkAccept" EventName="CheckedChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    
                                    <div class="container-fluid header_main">
	                                    <div class="col-sm-3 col-sm-3">
		                                    <h2>Organization SetUp</h2>
	                                    </div>
	                                    <div class="col-sm-5">
	                                        <asp:Label ID="lblTemplateMessage" runat="server" CssClass="relativ alert_new text-center text-success" />
	                                    
	                                    </div>
	                                    <div class="col-sm-3 col-sm-3 text-right pull-right">
                                    	    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="Save_OnClick" />
                                                <asp:ValidationSummary ID="VSHospital" runat="server" ShowMessageBox="True" ShowSummary="False" />
	                                    </div>

                                    </div>
                                    
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                
                        
                
                <div class="container-fluid subheading_main">
                    <div class="col-sm-4">Organization Details</div>
                
                </div>
                
              
                      
                      <div class="container-fluid">
                      <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkResetPassword" runat="server" CausesValidation="false" Text="Reset Password"
                                            Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                                            OnClick="lnkResetPassword_OnClick" Font-Underline="false" Visible="false" ></asp:LinkButton>
                                        <script language="JavaScript" type="text/javascript">
                                            function LinkBtnMouseOver(lnk) {
                                                document.getElementById(lnk).style.color = "red";
                                            }
                                            function LinkBtnMouseOut(lnk) {
                                                document.getElementById(lnk).style.color = "blue";
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                       <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" Text="" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <div class="row  form-group">
                               <div class="col-sm-6">
                                    <div class="col-sm-4"><asp:Literal ID="ltrlAccountName" runat="server" Text="Organization"></asp:Literal><span
                                style="color: Red">*</span></div>
                                    <div class="col-sm-8"><asp:UpdatePanel ID="updAccountName" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtAccountName" runat="server" SkinID="textbox" Width="100%" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVtxtAccountName" runat="server" ErrorMessage="Enter Organization Name"
                                        SetFocusOnError="true" ControlToValidate="txtAccountName" Display="None">
                                    </asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                               </div>   
                        </div>
                        <div class="row  form-group">
                               <div class="col-sm-6">
                                    <div class="col-sm-4"><asp:Literal ID="ltrlNPI" runat="server" Text="NPI"></asp:Literal></div>
                                    <div class="col-sm-8">  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtNPI" runat="server" SkinID="textbox" MaxLength="20"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                               </div>   
                               
                               <div class="col-sm-6">
                                    <div class="col-sm-4"><asp:Literal ID="ltrlEIN" runat="server" Text="EIN"></asp:Literal></div>
                                    <div class="col-sm-8"><asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtEIN" runat="server" SkinID="textbox" MaxLength="20"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                               </div> 
                               
                        </div>
                        
                        <div class="row form-group">
                            <div class="col-sm-6">
                                <div class="col-sm-4"><asp:Literal ID="ltrlMainNo" runat="server" Text="Phone #"></asp:Literal>
                            <span style="color: Red">*</span></div>
                                <div class="col-sm-8"> <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtMainNo" runat="server" SkinID="textbox" MaxLength="20"></asp:TextBox>
                                    <AJAX:FilteredTextBoxExtender ID="FTEtxtMainNo" runat="server" Enabled="True" TargetControlID="txtMainNo"
                                        FilterType="Custom, Numbers" ValidChars="()- _">
                                    </AJAX:FilteredTextBoxExtender>
                                 
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Enter Phone No"
                                        SetFocusOnError="true" ControlToValidate="txtMainNo" Display="None">
                                    </asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                            
                            
                            <div class="col-sm-6">
                                <div class="col-sm-4"><asp:Literal ID="ltrlFaxNo" runat="server" Text="Fax #"></asp:Literal></div>
                                <div class="col-sm-8"> <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFaxNo" runat="server" SkinID="textbox" MaxLength="20"></asp:TextBox>
                                    <AJAX:FilteredTextBoxExtender ID="FTEtxtFaxNo" runat="server" Enabled="True" TargetControlID="txtFaxNo"
                                        FilterType="Custom, Numbers" ValidChars="()- _">
                                    </AJAX:FilteredTextBoxExtender>
                                   
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                        </div>
                        
                         <div class="row form-group">
                            <div class="col-sm-6">
                                <div class="col-sm-4"><asp:Literal ID="ltrlMainAddress" runat="server" Text="Address1"></asp:Literal>
                            <span style="color: Red">*</span></div>
                                <div class="col-sm-8"><asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtAddress" runat="server" SkinID="textbox" MaxLength="250" Width="100%"
                                        onkeypress="return MultilineMaxNumber(this,250);"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVtxtAddress" runat="server" ErrorMessage="Enter Organization Address1"
                                        SetFocusOnError="true" ControlToValidate="txtAddress" Display="None">
                                    </asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                            
                            
                            <div class="col-sm-6">
                                <div class="col-sm-4"><asp:Literal ID="Literal1" runat="server" Text="Address2"></asp:Literal></div>
                                <div class="col-sm-8">  <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtAddress2" runat="server" SkinID="textbox" MaxLength="250" Width="100%"
                                        onkeypress="return MultilineMaxNumber(this,250);"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                        </div>
                         <div class="row form-group">
                            <div class="col-sm-6">
                                <div class="col-sm-4"><asp:Literal ID="ltrlCountry" runat="server" Text="Country"></asp:Literal><span style="color: Red">*</span></div>
                                <div class="col-sm-8">  <asp:UpdatePanel ID="updCountry" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCountry" SkinID="DropDown" AutoPostBack="true" runat="server"
                                        AppendDataBoundItems="true" onkeydown="Tab();" Font-Size="11px" Width="100%"
                                        OnSelectedIndexChanged="ddlCountry_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFVddlCountry" runat="server" ErrorMessage="Enter Country"
                                        SetFocusOnError="true" ControlToValidate="ddlCountry" Display="None" InitialValue="0">
                                    </asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                            
                            
                            <div class="col-sm-6">
                                <div class="col-sm-4"><asp:Literal ID="ltrlState" runat="server" Text="State"></asp:Literal><span style="color: Red">*</span></div>
                                <div class="col-sm-8"> <asp:UpdatePanel ID="updState" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlState" SkinID="DropDown" AutoPostBack="true" runat="server"
                                        AppendDataBoundItems="true" onkeydown="Tab();" Font-Size="11px" Width="100%"
                                        OnSelectedIndexChanged="ddlState_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFVddlState" runat="server" ErrorMessage="Enter State"
                                        SetFocusOnError="true" ControlToValidate="ddlState" Display="None" InitialValue="0">
                                    </asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                        </div>
                         <div class="row form-group">
                            <div class="col-sm-6">
                                <div class="col-sm-4"><asp:Literal ID="ltrlCity" runat="server" Text="City"></asp:Literal><span style="color: Red">*</span></div>
                                <div class="col-sm-8"><asp:UpdatePanel ID="updCity" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCity" SkinID="DropDown" AutoPostBack="true" runat="server"
                                        AppendDataBoundItems="true" onkeydown="Tab();" Font-Size="11px" Width="100%"
                                        OnSelectedIndexChanged="ddlCity_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Select" Value="" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFVddlCity" runat="server" ErrorMessage="Enter City"
                                        SetFocusOnError="true" ControlToValidate="ddlCity" Display="None" InitialValue="0">
                                    </asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                            
                            
                            <div class="col-sm-6">
                                <div class="col-sm-4"><asp:Literal ID="ltrlZip" runat="server" Text="ZIP"></asp:Literal><span style="color: Red">*</span></div>
                                <div class="col-sm-8"><asp:UpdatePanel ID="updZip" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlZip" SkinID="DropDown" AutoPostBack="false" runat="server"
                                        Font-Size="11px" Width="100%">
                                    </asp:DropDownList>
                                   <%-- <asp:RequiredFieldValidator ID="RFVddlZip" runat="server" ErrorMessage="Enter Zip"
                                        SetFocusOnError="true" ControlToValidate="ddlZip" Display="None" InitialValue="0">
                                    </asp:RequiredFieldValidator>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                        </div>
                         <div class="row form-group">
                            <div class="col-sm-6"  id="trposcode" runat="server">
                                <div class="col-sm-4"><asp:Label ID="lblPOS" runat="server" SkinID="label" Text="POS"></asp:Label>
                            <span style="color: #FF0000">  *</span></div>
                                <div class="col-sm-8"> <asp:DropDownList ID="ddlPOS" runat="server" AppendDataBoundItems="true" Font-Size="11px"
                                SkinID="DropDown" TabIndex="19" Width="250px">
                                <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                            </asp:DropDownList></div>
                            </div>
                            
                            
                            <div class="col-sm-6" id="tbltimezonehd" runat="server">
                                <div class="col-sm-4">Timing Zone</div>
                                <div class="col-sm-8"> <asp:Label ID="Label5" runat="server" /></div>
                            </div>
                        </div>
                         <div class="row form-group" id="tbltimezone" runat="server">
                            <div class="col-sm-6">
                                <div class="col-sm-4">  <asp:Label ID="lblTimeZone" runat="server" SkinID="label" Text="Time Zone"></asp:Label>
                            <span style="color: Red;">*</span></div>
                                <div class="col-sm-8"> <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlTimeZone" runat="server" AppendDataBoundItems="true" Font-Size="11px"
                                        SkinID="DropDown" TabIndex="19" Width="250px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="TimeZone_RequiredFieldValidator" ControlToValidate="ddlTimeZone" InitialValue="0"
                                            Display="None" runat="server" ErrorMessage="Select Time Zone"></asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                            </div>
                            
                            
                         
                        
                        
                        
                        
                      </div>
                   
                         <div class="conatainer-fluid subheading_main" id="table1" runat="server">
                            <div class="col-sm-3">Billing Address</div>
                            <div class="col-sm-3"><asp:Label ID="Label4" runat="server" /></div>
                        </div>
                    
                 
                </div>
                
                
                
                <div class="container-fluid">
                    <div class="row form-group">
                        <div class="col-sm-6">
                            <div class="col-sm-4"><asp:Literal ID="Literal2" runat="server" Text="Address1"></asp:Literal></div>
                            <div class="col-sm-8">  <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBillingaddress" runat="server" SkinID="textbox" MaxLength="250"
                                        Width="100%"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                        </div>
                        <div class="col-sm-6">
                            <div class="col-sm-4">Address2</div>
                            <div class="col-sm-8"><asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBillingaddress2" runat="server" MaxLength="250" SkinID="textbox"
                                        Width="100%"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel></div>
                        </div>    
                    </div>
                    
                     <div class="row form-group">
                        <div class="col-sm-6">
                            <div class="col-sm-4"><asp:Literal ID="Literal5" runat="server" Text="Country"></asp:Literal></div>
                            <div class="col-sm-8"> <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlbillingcountery" SkinID="DropDown" AutoPostBack="true" runat="server"
                                        AppendDataBoundItems="true" onkeydown="Tab();" Font-Size="11px" Width="100%"
                                        OnSelectedIndexChanged="ddlbillingcountery_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Enter Country"
                            SetFocusOnError="true" ControlToValidate="ddlbillingcountery" Display="None" InitialValue="0">*</asp:RequiredFieldValidator>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                        </div>
                        <div class="col-sm-6">
                            <div class="col-sm-4"><asp:Literal ID="Literal8" runat="server" Text="State"></asp:Literal></div>
                            <div class="col-sm-8"> <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlbillingcountery" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlbillingstate" SkinID="DropDown" AutoPostBack="true" runat="server"
                                        AppendDataBoundItems="true" onkeydown="Tab();" Font-Size="11px" Width="100%"
                                        OnSelectedIndexChanged="ddlbillingstate_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Enter State"
                            SetFocusOnError="true" ControlToValidate="ddlbillingstate" Display="None" InitialValue="0">*</asp:RequiredFieldValidator>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                        </div>    
                    </div>
                    
                    
                     <div class="row form-group">
                        <div class="col-sm-6">
                            <div class="col-sm-4"><asp:Literal ID="Literal6" runat="server" Text="City"></asp:Literal></div>
                            <div class="col-sm-8"><asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlbillingstate" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlbillingcity" SkinID="DropDown" AutoPostBack="true" runat="server"
                                        AppendDataBoundItems="true" onkeydown="Tab();" Font-Size="11px" Width="100%"
                                        OnSelectedIndexChanged="ddlbillingcity_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Enter City"
                            SetFocusOnError="true" ControlToValidate="ddlCity" Display="None" InitialValue="0">
                        </asp:RequiredFieldValidator>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                        </div>
                        <div class="col-sm-6">
                            <div class="col-sm-4"><asp:Literal ID="Literal9" runat="server" Text="Zip"></asp:Literal></div>
                            <div class="col-sm-8"><asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlbillingcity" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlbillingzip" SkinID="DropDown" AutoPostBack="false" runat="server"
                                        Font-Size="11px" Width="100%">
                                    </asp:DropDownList>
                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Enter Zip"
                            SetFocusOnError="true" ControlToValidate="ddlZip" Display="None" InitialValue="0">
                        </asp:RequiredFieldValidator>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel></div>
                        </div>    
                    </div>
                    
                    
                     <div class="row form-group">
                        <div class="col-sm-6">
                            <div class="col-sm-4"><asp:Literal ID="Literal4" runat="server" Text="End of Year Date"></asp:Literal>
                            <span style="color: Red">*</span></div>
                            <div class="col-sm-8"> <asp:UpdatePanel ID="updendofyear" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                   
                                                <asp:DropDownList ID="ddlmonth" runat="server" SkinID="DropDown" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged">
                                                    <asp:ListItem Value="00" Text="[ Month ]"></asp:ListItem>
                                                    <asp:ListItem Value="01" Text="January"></asp:ListItem>
                                                    <asp:ListItem Value="02" Text="February"></asp:ListItem>
                                                    <asp:ListItem Value="03" Text="March"></asp:ListItem>
                                                    <asp:ListItem Value="04" Text="April"></asp:ListItem>
                                                    <asp:ListItem Value="05" Text="May"></asp:ListItem>
                                                    <asp:ListItem Value="06" Text="June"></asp:ListItem>
                                                    <asp:ListItem Value="07" Text="July"></asp:ListItem>
                                                    <asp:ListItem Value="08" Text="August"></asp:ListItem>
                                                    <asp:ListItem Value="09" Text="September"></asp:ListItem>
                                                    <asp:ListItem Value="10" Text="October"></asp:ListItem>
                                                    <asp:ListItem Value="11" Text="November"></asp:ListItem>
                                                    <asp:ListItem Value="12" Text="December"></asp:ListItem>
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:DropDownList ID="ddldate" runat="server" SkinID="DropDown" Style="visibility: hidden;">
                                                    <asp:ListItem Value="0" Text="[ Date ]"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Select End Of Year Date"
                                                    SetFocusOnError="true" ControlToValidate="ddlmonth" Display="None" InitialValue="00">
                                                </asp:RequiredFieldValidator>
                                       
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlmonth" />
                                    <asp:AsyncPostBackTrigger ControlID="ddldate" />
                                </Triggers>
                            </asp:UpdatePanel></div>
                        </div>
                        <div class="col-sm-6">
                            <div class="col-sm-4"><asp:Literal ID="Literal3" runat="server" Text="Phone"></asp:Literal></div>
                            <div class="col-sm-8"><asp:TextBox ID="txtBillingphone" runat="server" SkinID="textbox" MaxLength="20"></asp:TextBox>
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                TargetControlID="txtBillingphone" FilterType="Custom, Numbers" ValidChars="()- _">
                            </AJAX:FilteredTextBoxExtender></div>
                        </div>    
                    </div>
                    
                    
                   
                     
                    
                    
                </div>
                
                
                    
                       
              
              
               
               
                
                
                
               
                            <asp:UpdatePanel ID="Availpanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                
                                 <div class="container-fluid margin_bottom" id="trnewuser" runat="server" visible="true">
                                 <div class="container-fluid subheading_main">
              <div id="tableUserDetails" runat="server">User Details<asp:Label ID="Label2" runat="server" /></div>
              </div>
                                        <div class="row form-group">
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">Prefix</div>
                                                <div class="col-sm-8"> <asp:DropDownList ID="ddltitle" CssClass="DropDown" runat="server" Width="100px"
                                                    DataTextField="Name" DataValueField="TitleId">
                                                </asp:DropDownList></div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">Last Name<span style="color: Red;">*</span></div>
                                                <div class="col-sm-8"> <asp:TextBox ID="txtlastname" CssClass="Textbox" runat="server" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="None" ControlToValidate="txtlastname"
                                                    runat="server" ErrorMessage="Enter Last Name"></asp:RequiredFieldValidator></div>
                                            </div>    
                                        </div>
                                        
                                        <div class="row form-group">
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">First Name<span style="color: Red;">*</span></div>
                                                <div class="col-sm-8"> <asp:TextBox ID="txtfirstname" CssClass="Textbox" runat="server" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="None" ControlToValidate="txtfirstname"
                                                    runat="server" ErrorMessage="Enter First Name"></asp:RequiredFieldValidator></div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">Middle Name</div>
                                                <div class="col-sm-8"> <asp:TextBox ID="txtmiddlename" CssClass="Textbox" runat="server" MaxLength="50"></asp:TextBox></div>
                                            </div>
                                        </div>
                                        
                                         <div class="row form-group">
                                            <div class="col-sm-6">
                                                <div class="col-sm-4"><asp:Literal ID="Literal7" runat="server" Text="Employee Type"></asp:Literal></div>
                                                <div class="col-sm-8"><asp:DropDownList ID="ddlemployeetype" runat="server" AutoPostBack="false" Font-Size="11px"
                                                    SkinID="DropDown" Width="130px">
                                                </asp:DropDownList></div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="col-sm-4"><asp:Label ID="lblEmploymentStatus" runat="server" Text="Employment Status" SkinID="label"></asp:Label><span
                                                    style="color: Red">*</span></div>
                                                <div class="col-sm-8"><asp:DropDownList ID="ddlEmploymentstatus" runat="server" SkinID="DropDown" Width="130px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="employmentvalidation" runat="server" ControlToValidate="ddlEmploymentstatus"
                                                    InitialValue="0" Display="None" ErrorMessage="Select Employment Status">*</asp:RequiredFieldValidator></div>
                                            </div>
                                        </div>
                                        
                                        
                                         <div class="row form-group">
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">User Name<span style="color: Red;">*</span></div>
                                                <div class="col-sm-8"> <asp:UpdatePanel ID="up1" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAvailability" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtusername" runat="server" SkinID="textbox" Columns="30" MaxLength="15"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="None" ControlToValidate="txtusername"
                                                            runat="server" ErrorMessage="Enter User Name"></asp:RequiredFieldValidator>
                                                        &nbsp; &nbsp;
                                                        <asp:Button ID="btnAvailability" Width="100%" runat="server" SkinID="button" Text="Check User Availability"
                                                            ValidationGroup="username" OnClick="btnAvailability_Click" Height="20px" />
                                                        &nbsp; &nbsp;
                                                        <asp:Label ID="lblAvailabilityMessage" runat="server" SkinID="label"></asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel></div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">Password<span style="color: Red;">*</span></div>
                                                <div class="col-sm-8"> <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtpassword"  MaxLength="30" TextMode="Password" Width="91%"
                                                            runat="server" ToolTip="Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&amp;=*' letters."></asp:TextBox>
                                                        <asp:Button ID="btnpasshelp" runat="server" Text="?" CausesValidation="false" SkinID="Button"
                                                            ToolTip="  Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&amp;=*' letters."
                                                            OnClick="btnpasshelp_Click" />
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Display="None" ControlToValidate="txtpassword"
                                                            runat="server" ErrorMessage="Enter Password"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="regexpName" Display="None" runat="server" ErrorMessage="Password is not in write format."
                                                            ControlToValidate="txtpassword" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^*&+=]).*$"></asp:RegularExpressionValidator>
                                                        <telerik:RadToolTip ID="RadToolTip1" runat="server" EnableShadow="true" RelativeTo="Element"
                                                            Height="40px" Width="300px" TargetControlID="txtpassword" BackColor="#FFFFD5"
                                                            Position="TopCenter">
                                                        </telerik:RadToolTip>
                                                        <AJAX:PasswordStrength ID="PasswordStrength1" DisplayPosition="BelowLeft" CalculationWeightings="50;15;15;20"
                                                            TextStrengthDescriptions="Weak;Average;Strong;Excellent" TargetControlID="txtpassword"
                                                            MinimumLowerCaseCharacters="1" MinimumNumericCharacters="1" MinimumSymbolCharacters="1"
                                                            MinimumUpperCaseCharacters="1" runat="server">
                                                        </AJAX:PasswordStrength>
                                                        <telerik:RadToolTip ID="RadToolTip5" runat="server" EnableShadow="true" RelativeTo="Element"
                                                             Width="10%" TargetControlID="btnpasshelp" BackColor="#FFFFD5"
                                                            Position="TopCenter">
                                                        </telerik:RadToolTip>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel></div>
                                            </div>
                                        </div>
                                        
                                        
                                         <div class="row form-group">
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">Confirm Password<span style="color: Red;">*</span></div>
                                                <div class="col-sm-8"><asp:TextBox ID="txtConfirmPassword"  MaxLength="30" runat="server" Width="100%"
                                                    ToolTip="Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&amp;=*' letters."
                                                    TextMode="Password"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="None" ControlToValidate="txtConfirmPassword"
                                                    runat="server" ErrorMessage="Enter Confirm Password"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Password and Confirm Password Should be Same"
                                                    Display="None" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword"
                                                    SetFocusOnError="true" Operator="Equal"></asp:CompareValidator>
                                                <telerik:RadToolTip ID="RadToolTip2" runat="server" EnableShadow="true" RelativeTo="Element"
                                                    Height="40px" Width="300px" TargetControlID="txtConfirmPassword" BackColor="#FFFFD5"
                                                    Position="TopCenter">
                                                </telerik:RadToolTip></div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">EMail</div>
                                                <div class="col-sm-8"><asp:TextBox ID="txtemailid" MaxLength="70" CssClass="Textbox" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtemailid"
                                                    runat="server" ErrorMessage="Not a valid e-Mail." Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></div>
                                            </div>
                                        </div>
                                        
                                        
                                         <div class="row form-group">
                                            <div class="col-sm-6">
                                                <div class="col-sm-4">Work #</div>
                                                <div class="col-sm-8"><asp:TextBox ID="txtworkPhone" CssClass="Textbox" Columns="15" runat="server" MaxLength="20"></asp:TextBox>
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                    TargetControlID="txtworkPhone" FilterType="Custom, Numbers" ValidChars="()- _">
                                                </AJAX:FilteredTextBoxExtender></div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="col-sm-4"> Mobile #</div>
                                                <div class="col-sm-8"> <asp:TextBox ID="txtmobileno" CssClass="Textbox" Columns="15" runat="server"></asp:TextBox>
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                    TargetControlID="txtmobileno" FilterType="Custom, Numbers" ValidChars="()- _">
                                                </AJAX:FilteredTextBoxExtender></div>
                                            </div>
                                        </div>
                                        
                                     
                                        
                                    </div>
                                
                                           
                                          
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAvailability" />
                                </Triggers>
                            </asp:UpdatePanel>
                    
                    
                    
                    
                    
                    
                    
                    
           <div class="container-fluid">
                            <asp:CheckBox ID="chkAccept" Font-Bold="true" runat="server" SkinID="checkbox" AutoPostBack="true"
                                    Text="I Accept" OnCheckedChanged="chkAccept_OnCheckedChanged" /><br />
                            <strong>Terms & Conditions</strong>
                        
                        
                            <asp:Label ID="Label3" runat="server" />
                        
                                Please read the
                                <asp:Label ID="lblTerms" runat="server" Text="Terms of V-Care Service" Font-Underline="true"
                                    Font-Bold="true" ForeColor="Blue" Style="cursor: pointer;"></asp:Label>
                                and the
                                <asp:Label ID="lblPrivacyPolicy" runat="server" Text="Privacy Policy" Font-Underline="true"
                                    Font-Bold="true" ForeColor="Blue" Style="cursor: pointer;"></asp:Label>. If
                                you agree with these terms and conditions, please accept the term below.<br />
                                <br />
                                
                                <br />
                                <br />
                                Please read this standard
                                <asp:Label ID="Label1" runat="server" Text="Business Associate Agreement" Font-Underline="true"
                                    Font-Bold="true" ForeColor="Blue" Style="cursor: pointer;"></asp:Label>.If you
                                agree with these terms and conditions,please print,sign and attach the document.<br />
                                If you have your Business Associate Agreement, please attach it or fax it to XXX-XXX-XXXX.
                                We will review the agreement and execute it or<br />
                                contact you with concerns. This can be completed at any time.
                            </p>
                       
    </div></div>
    </form>
</body>
</html>
