<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BedClearance.aspx.cs" Inherits="ATD_BedClearance" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ICU Clearance</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style>
        .textColor {border:solid 1px #d2d2d2; outline:none;}
    </style>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function ConfirmClearance() {
            var registrationId = document.getElementById("hdnRegistrationId1");
            var EncounterId1 = document.getElementById("hdnEncounterId1");
            var TransferRequistionId1 = document.getElementById("hdnTransferRequistionId1");
            var RegId = '', EncId = '', Id = '';
            
            if (registrationId != null ) {
                RegId = registrationId.value;
            }
            if (EncounterId1 != null) {
                EncId = EncounterId1.value;
            }
            if (TransferRequistionId1 != null) {
                Id = TransferRequistionId1.value;
            }
           // alert(Id);
            if (Id != '' || EncId != '' || RegId != '') {
                if (confirm("You want to Proceed?") == true) {
                    return true;
                } else {
                    return false;
                }
            } else {
                alert("Please select the record !");
                return false;
            }
           
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
   <div class="container-fluid header_main">
        <div class="col-md-3 col-sm-3"><h2><asp:Label ID="lblHeader" runat="server" Text="ICU Clearance" /></h2></div>
        <div class="col-md-6 col-sm-6 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
        <div class="col-md-3 col-sm-3 text-right"><asp:Button ID="btnClearance" Text="Update" runat="server" CssClass="btn btn-primary" OnClick="btnClearance_Click" OnClientClick="return ConfirmClearance();"/> </div>
    </div>

    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
     <asp:HiddenField ID="hdnRegistrationId1" runat="server" />
    <asp:HiddenField ID="hdnEncounterId1" runat="server" />
    <asp:HiddenField ID="hdnTransferRequistionId1" runat="server" /> 
            <div class="container-fluid">
                <div class="row form-group" style="overflow-y: scroll; height: 550px;">
                    <asp:GridView ID="gvBedTransferRequest" runat="server" SkinID="gridviewOrderNew" AutoGenerateColumns="false" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" OnClick="lnkSelect_OnClick"></asp:LinkButton>
                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                    <asp:HiddenField ID="hdnTransferRequistionId" runat="server" Value='<%#Eval("id") %>' />                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText='<%$ Resources:PRegistration, Regno%>' DataField="RegistrationNo" />
                            <asp:BoundField HeaderText="IP No." DataField="EncounterNo" />
                            <asp:BoundField HeaderText="Name" DataField="PatientName" />
                            <asp:BoundField HeaderText="Age/Gender" DataField="AgeGender" />
                            <asp:BoundField HeaderText="From Billing Category" DataField="FromBillingCategory" />
                            <asp:BoundField HeaderText="From Bed Category" DataField="FromBedCategory" />
                            <asp:BoundField HeaderText="From Ward" DataField="Fromward" />
                            <asp:BoundField HeaderText="To Ward" DataField="Toward" Visible="true" />
                            <asp:BoundField HeaderText="From Bed No" DataField="FromBedNo" />
                            <asp:BoundField HeaderText="Request Remarks" DataField="RequestRemarks"  />
                            <asp:BoundField HeaderText="Current Bed Status" DataField="Bedstatus" />
                            <asp:BoundField HeaderText="Transfer Request Date" DataField="RequestDt" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="container-fluid header_mainGray">
                <%--<div class="col-md-12 col-sm-12"><h2>To Bed Detail</h2></div>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvBedTransferRequest" />
            
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
