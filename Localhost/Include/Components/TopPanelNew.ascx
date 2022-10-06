<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopPanelNew.ascx.cs" Inherits="Temp_TopPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>




<!-- Patient Part Start -->
<div class="patientDiv" id="patientDiv" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="patientDiv-Photo">
                <asp:UpdatePanel ID="UpdatePanel400" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:ImageButton ID="PatientImage" runat="server" ImageUrl="~/imagesHTML/nophoto.ico" border="0"
                            BorderWidth="0" BorderColor="Gray" OnClick="PatientImage_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="patientDiv-Table" style="height: auto!important;">
                <table cellspacing="0" class="table table-small-font table-bordered table-striped" style="font-weight: bold;">
                    <tbody>
                        <tr align="center">
                            <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                                <asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration, regno%>' />:
                                        <asp:Label ID="lblCId" Font-Bold="true" runat="server" />
                            </td>
                            <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-2">Name:
                                <asp:Label ID="lblPatientName" Font-Bold="true" runat="server" /></td>
                            <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-3">Age/Gender/Blood Group: 
                                   <asp:Label ID="lblAge" Font-Bold="true" runat="server" />
                                <asp:Label ID="lblGender" Font-Bold="true" runat="server" />
                                <asp:Label ID="lblDob" Visible="false" runat="server" />
                            </td>
                            <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-5">
                                <asp:Label ID="lblEncNo_Resources" runat="server" Text='<%$ Resources:PRegistration, EncounterNo%>'>:</asp:Label>:
                                        <asp:Label ID="lblEncNo" Font-Bold="true" runat="server" />
                            </td>
                            <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6">Provider:
                                <asp:Label ID="lblVtCrPrvdr" Font-Bold="true" runat="server" /></td>
                            <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6">

                                <%--Visit--%> Date: &nbsp;<asp:Label ID="lblEncDate" Font-Bold="true" runat="server" />
                                <asp:Label ID="lblCrntEnSts" Visible="false" runat="server" />
                                <asp:Label ID="lblAcCategory" Visible="false" runat="server" SkinID="label" Text="" />
                                <asp:Label ID="lblAcType" runat="server" Visible="false" SkinID="label" Text="" />
                                <asp:Label ID="lblVisitType" runat="server" Visible="false" />
                                <asp:Label ID="lblPackageVisit" runat="server" Visible="false" />
                                <asp:Label ID="lblLoc" runat="server" Visible="false" />
                                <asp:Label ID="lblAddress" runat="server" Visible="false" />
                                <asp:Label ID="lblRefPrvdr" Visible="false" runat="server" />
                                <asp:Label ID="lblApptNote" runat="server" Visible="false" />
                            </td>
                            <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6" style="display: none;"></td>
                        </tr>
                        <tr align="center" style="display: none;">
                            <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">Bed:
                                <asp:Label ID="lblBedNo" runat="server" /></td>
                            <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-2">Ward:
                                <asp:Label ID="lblWard" runat="server" />
                            </td>
                            <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-3">Mobile: &nbsp;<asp:Label ID="lblMphone" runat="server" /></td>
                            <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-5">Company:
                                <asp:Label ID="lblPayer" runat="server" /><asp:Label ID="lblPlnType" runat="server" SkinID="label" Text="" /></td>
                            <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6"></td>
                            <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6"></td>
                        </tr>
                    </tbody>
                </table>
            </div>





        </div>
    </div>
</div>
<!-- Patient Part Ends -->
<script type="text/javascript">
    function getPage(val) {
        // this will make a child page popup
        window.open(val, "MyWindow", "height=356,width=267,left=10,top=10, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
    }
</script>
