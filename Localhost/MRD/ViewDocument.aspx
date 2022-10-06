<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ViewDocument.aspx.cs" Inherits="MRD_ViewDocument" Title="View Document Refer Type" %>

<%@ Register Src="../Include/Components/PatientQView.ascx" TagName="PatientQView"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

 <link rel="stylesheet" href="../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../Include/css/mainNew.css" />
    <link rel="stylesheet" href="../Include/EMRStyle.css" />

    <script language="javascript" type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtAccountNo.ClientID%>');
            if (txt.value > 2147483647) {
                alert("Value should not be more than 2147483647.");
                txt.value = txt.value.substring(0, 9);
                txt.focus();
            }
        }
        function SearchPatientOnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.RegistrationNo;
                $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                $get('<%=txtAccountNo.ClientID%>').value = RegistrationNo;

            }
            $get('<%=btnGetInfo.ClientID%>').click();
        }

        function openRadWindow(strPageNameWithQueryString) {
            var oWnd = radopen(strPageNameWithQueryString, "RadWindow1");
        }
    </script>

    <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
            <div class="container-fluid" id="tblMain">
                <div class="row header_main">
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <asp:Label SkinID="label" runat="server" ID="lblHeader" Text="Docment Refer Type" />
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-12 text-center">
                        <asp:Label ID="lblMsg" runat="server" SkinID="label" Text="" />
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-5 col-sm-5 col-xs-12 m-t">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #dedede;">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                   <div style="width:100%;float:left;overflow:hidden;">
                                        <uc1:PatientQView ID="patientQV" runat="server" />
                                   </div>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                                        <div class="col-md-3 col-sm-3 col-xs-4">
                                            <asp:LinkButton ID="lbtnSearchPatient" runat="server" Text='<%$ Resources:PRegistration, Regno%>'
                                                        Font-Underline="false" ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-xs-8">
                                            <asp:TextBox ID="txtAccountNo" SkinID="textbox" runat="server" Width="100px" ForeColor="Maroon"
                                                        TabIndex="0" MaxLength="10" Visible="true" onkeyup="return validateMaxLength();" ></asp:TextBox> 
                    <cc1:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True"
                    FilterType="Custom" TargetControlID="txtAccountNo" ValidChars="0123456789" />
                    
                                                    <asp:Button ID="btnGetInfo" runat="server" CausesValidation="false" Enabled="true"
                                                        OnClick="btnGetInfo_Click" SkinID="button" Style="visibility: hidden;" TabIndex="103"
                                                        Text="Assign" Width="10px" />
                                        </div>
                                             </asp:Panel>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <div class="col-md-3 col-sm-3 col-xs-4">
                                            <asp:Label ID="lblEnc" runat="server" Text="Enc#" />
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-xs-8">
                                            <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row m-t" style="border-top:1px solid #dedede;border-bottom:1px solid #dedede;">
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="lblEDt" runat="server" Text="Enc. Dt." />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <asp:Label ID="lblEncDate" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                    <div class="row p-t-b-5">
                                       <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label5" runat="server" Text="Discharge On" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <asp:Label ID="lblDischargeDate" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label1" runat="server" Text="Bed#/Category" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <asp:Label ID="lblBedCategory" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label6" runat="server" Text="Payer" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <asp:Label ID="lblPayer" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                    <div class="row p-t-b-5">
                                       <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label7" runat="server" Text="Sponsor" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                             <asp:Label ID="lblSponsor" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <div class="col-md-3 col-sm-3 col-xs-4"></div>
                                        <div class="col-md-9 col-sm-9 col-xs-8"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="row m-t">
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Width="100%" Height="300px"
                                        BorderWidth="0">
                                        <asp:GridView ID="gvVisists" runat="server" SkinID="gridview" AutoGenerateColumns="false"
                                            Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Enc#" ItemStyle-Width="15%" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkShowDetails" runat="server" Text='<%#Eval("EncounterNo") %>'
                                                            OnClick="lnkShowDetails_OnClick" Width="98%" />
                                                        <asp:HiddenField ID="hdnEncId" runat="server" Value='<%#Eval("ID") %>' />
                                                        <asp:HiddenField ID="hdnRegId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                                        <asp:HiddenField ID="hdnPayer" runat="server" Value='<%#Eval("Payer") %>' />
                                                        <asp:HiddenField ID="hdnSponsor" runat="server" Value='<%#Eval("Sponsor") %>' />
                                                        <asp:HiddenField ID="hdnEncounterDate" runat="server" Value='<%#Eval("EncounterDate") %>' />
                                                        <asp:HiddenField ID="hdnDischargeDate" runat="server" Value='<%#Eval("DischargeDate") %>' />
                                                        <asp:HiddenField ID="hdnBedNo" runat="server" Value='<%#Eval("BedNo") %>' />
                                                        <asp:HiddenField ID="hdnBedCategory" runat="server" Value='<%#Eval("BedCategory") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dated On" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncDate" runat="server" Font-Size="9px" Text='<%#Eval("EncDate") %>'
                                                            Width="98%" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OP/IP" ItemStyle-Width="5%" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOPIP" runat="server" Font-Size="9px" Text='<%#Eval("OPIP") %>'
                                                            Width="98%" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doctor" HeaderStyle-Width="60%" ItemStyle-Width="60%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDoc" runat="server" Font-Size="9px" Text='<%#Eval("Doctor") %>'
                                                            Width="98%" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                    </div>
                        </div>
                    </div>

                    <div class="col-md-7 col-sm-7 col-xs-12 m-t">
                        <asp:Panel ID="pnlMain" runat="server" ScrollBars="None" Width="99%" Height="500px"
                            BorderWidth="2">
                            <%-- <iframe id="ifrmDiag" runat="server" width="100%" height="100%" frameborder="1">
                            </iframe>--%>
                            <telerik:RadListView runat="server" ID="RadListView1" OnPageIndexChanged="RadListView1_PageIndexChanged"
                                AllowPaging="true" DataKeyNames="Id" Width="100%">
                                <LayoutTemplate>
                                    <telerik:RadDataPager ID="RadDataPager1" runat="server" PagedControlID="RadListView1"
                                        Width="98%" PageSize="4">
                                        <Fields>
                                            <telerik:RadDataPagerTemplatePageField Visible="true">
                                            </telerik:RadDataPagerTemplatePageField>
                                            <telerik:RadDataPagerButtonField FieldType="FirstPrev" />
                                            <telerik:RadDataPagerButtonField FieldType="Numeric" />
                                            <telerik:RadDataPagerButtonField FieldType="NextLast" />
                                            <telerik:RadDataPagerGoToPageField CurrentPageText="Page: " TotalPageText="of" SubmitButtonText="Go"
                                                TextBoxWidth="20" />
                                            <telerik:RadDataPagerTemplatePageField>
                                                <PagerTemplate>
                                                </PagerTemplate>
                                            </telerik:RadDataPagerTemplatePageField>
                                        </Fields>
                                    </telerik:RadDataPager>
                                    <fieldset runat="server" id="itemPlaceholder" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                </LayoutTemplate>
                                <EmptyItemTemplate>
                                    <div style="float: left; width: 230px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    No item to display here.
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </EmptyItemTemplate>
                                <EmptyDataTemplate>
                                    No records for patient available.
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <fieldset style="float: left; margin: 5px 5px 5px 5px; padding: 2px 2px 2px 2px;
                                        background: #eeeeee" class="myClass">
                                        <table>
                                            <tr>
                                                <td>
                                                    <%--DataValue='<%#Eval("Data") %>'--%>
                                                    <telerik:RadBinaryImage Style="cursor: pointer;" runat="server" ID="RadBinaryImage1"
                                                        OnClick='<%#CreateWindowScript(DataBinder.Eval(Container.DataItem, "id")) %>'
                                                        ImageUrl='<%#Bind("ImagePath") %>' Height='<%#ImageHeight%>' Width="<%#ImageWidth %>"
                                                        ResizeMode="Fit" AutoAdjustImageControlSize="false" AlternateText="Thumbnail not available please download to view"
                                                        ToolTip='<%#Bind("Description")%>' />
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td>
                                                    <asp:TextBox ID="txtRemarks" Width="200px" ReadOnly="true" Rows="3" TextMode="MultiLine"
                                                        runat="server" Text='<%#Bind("Remarks") %>'></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnDelete" SkinID="Button" runat="server" Text="Delete" CommandArgument='<%#Eval("Id") %>'
                                                        OnClick="btnDelete_Click" />&nbsp;
                                                   
                                                    <asp:LinkButton ID="lbtnDownload" runat="server" Text="Download" CommandArgument='<%#Eval("ImageName") %>'
                                                        OnClick="lbtnDownload_OnClick"></asp:LinkButton>
                                                </td>
                                            </tr>--%>
                                        </table>
                                        
                                    </fieldset>
                                </ItemTemplate>
                            </telerik:RadListView>
                        </asp:Panel>
                    </div>

                </div>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
