<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopPanel.ascx.cs" Inherits="Include_Components_TopPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
<style>
.table-padding2 th, .table-padding2 td { padding: 2px !important;}
.table-padding2 tr:first-child td:first-child { background: #4E90FE; }
.table-padding2 tr:nth-child(odd) td {  background: #f5f5f5;}
div#RAD_SLIDING_PANE_CONTENT_ctl00_Radslidingpane4 { height: auto !important; max-height: 160px; width: 100% !important; border-bottom: 1px solid #545454;
    padding: 4px;
    box-shadow: 0 0 10px #ccc;}
td#RAD_SPLITTER_SLIDING_ZONE_RESIZE_ctl00_Radslidingpane4 { display: none;}
table.rspSlideContainer {
    width: 100% !important;
}

div#ctl00_Radslidingpane4 {
    left: 0 !important;
    right: 0px !important;
    margin: auto;
    width: 96% !important;
    z-index: 9999 !important;
}
</style>

<table width="100%" class="table table-bordered table-condensed table-padding2" style="margin-bottom: 0;">
    <tr width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff;">
        <td rowspan="6" width="12%" bgcolor="#4E90FE" align="center">
            <div>
                <span style="font-size: 12px; color: #FFFFFF;">Patient Details</span>
            </div>
            <div>
            </div>
            <asp:Image ID="PatientImage" runat="server" ImageUrl="~/Images/PImageBackGround.gif"
                BorderWidth="1" BorderColor="Gray" Height="80px" Width="80px" />
        </td>
        
        <td width="22%">
            <asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration, UHID %>'></asp:Label>:
            <asp:Label ID="lblCId" runat="server"></asp:Label>
        </td>
        <td  width="22%">Current Encounter:
            <asp:Label ID="lblCrntEnSts" runat="server"></asp:Label>
        </td>
        <td  width="22%">Account Category :
            <asp:Label ID="lblAcCategory" runat="server" SkinID="label" Text="" />
        </td>
        <td  width="22%">Payer Name :
            <asp:Label ID="lblPayer" runat="server" SkinID="label" Text="" />
        </td>
    </tr>


    <tr>
        <td>DOB(Age):<asp:Label ID="lblDob" runat="server"></asp:Label>(<asp:Label ID="lblAge"
            runat="server"></asp:Label>)
        </td>
        <td style="font-weight: bold;">
            <asp:Label ID="lblEncNo" runat="server"></asp:Label>
            &nbsp;<asp:Label ID="lblEncDate" runat="server"></asp:Label>
        </td>
        <td>Account Type :
            <asp:Label ID="lblAcType" runat="server" SkinID="label" Text="" />
        </td>
        <td>Sponsor Name :
            <asp:Label ID="lblSponsor" runat="server" SkinID="label" Text="" />
        </td>
    </tr>
    <tr>
        <td>Gender:
            <asp:Label ID="lblGender" runat="server"></asp:Label>
        </td>
        <td>Visit Type:
            <asp:Label ID="lblVisitType" runat="server"></asp:Label>
        </td>
        <td>Plan Type :
            <asp:Label ID="lblPlnType" runat="server" SkinID="label" Text="" />
        </td>
        <td> Network :
            <asp:Label ID="lblNetworkName" runat="server" SkinID="label" Text="" />
        </td>
    </tr>
    <tr>
        <td>Home #:
            <asp:Label ID="lblHphone" runat="server"></asp:Label>
        </td>
        <td>Location:
            <asp:Label ID="lblLoc" runat="server"></asp:Label>
        </td>
        <td>Card Valid Date :
            <asp:Label ID="lblNotificationName" runat="server" SkinID="label" Text="" />
        </td>
        <td>Payment Type:
            <asp:Label ID="lblEncounterCompanyType" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Work #:
            <asp:Label ID="lblWphome" runat="server"></asp:Label>
        </td>
        <td>Doctor:
            <asp:Label ID="lblVtCrPrvdr" runat="server"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPackageVisit" runat="server"></asp:Label>
        </td>
        <td>Card No :
            <asp:Label ID="lblRegInsCardId" runat="server" SkinID="label" Text="" />
        </td>
    </tr>
    <tr>
        <td>Mobile #:
            <asp:Label ID="lblMphone" runat="server"></asp:Label>
        </td>
        <td>Treating Consultant:
            <asp:Label ID="lblTreatingConsultant" runat="server"></asp:Label>
        </td>
        <td>Appointment Note:
            <asp:Label ID="lblApptNote" runat="server"></asp:Label>
        </td>
        <td>Card Valid Date :
            <asp:Label ID="lblCardValidDate" runat="server" SkinID="label" Text="" />
        </td>
    </tr>
  
    <tr>
        <td colspan="1">

        </td>
        <td>
            Acknowledged Date :
             <asp:Label ID="lblpatinetAckDataTime" runat="server" SkinID="label" Text=""  />
        </td>
        <td>
            Acknowledged By :
             <asp:Label ID="lblAcknowledgedBy" runat="server" SkinID="label" Text="" />
        </td>
    </tr>

</table>
