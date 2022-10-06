<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImmunizationBaby.aspx.cs"
    Inherits="EMR_Immunization_ImmunizationBaby" MasterPageFile="~/Include/Master/EMRMaster.master" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />



    <script language="javascript" type="text/javascript">
        function NextTab() {
            if (event.keyCode == 13) {
                event.keyCode = 9;
            }
        }

        function Validation(CtrlFrom1, CtrlFrom2, CtrlTo1, CtrlTo2, CtrltxtFrom1, CtrltxtFrom2, CtrltxtTo1, CtrltxtTo2, CtrlDdlName) {



            var valCtrlFrom1 = $get(CtrlFrom1).value;
            var valCtrlFrom2 = $get(CtrlFrom2).value;
            var valCtrlTo1 = $get(CtrlTo1).value;
            var valCtrlTo2 = $get(CtrlTo2).value;

            var SelIndexCtrlFrom1 = $get(CtrlFrom1).selectedIndex;
            var SelIndexCtrlFrom2 = $get(CtrlFrom2).selectedIndex;
            var SelIndexCtrlTo1 = $get(CtrlTo1).selectedIndex;
            var SelIndexCtrlTo2 = $get(CtrlTo2).selectedIndex;

            var valCtrlDdl = $get(CtrlDdlName).selectedIndex;

            var totFromDays = 0;
            var totToDays = 0;

            if (valCtrlDdl == "0") {
                alert("Please Select Any Name..");
                $get(CtrlDdlName).focus();
                return false;
            }
            if (valCtrlFrom1 == valCtrlFrom2) {
                alert("Age From Cannot Have Both Same Selections..");
                $get(CtrlFrom1).focus();
                return false;
            }
            //            if (valCtrlTo1 == valCtrlTo2) {
            //                alert("Age To Cannot Have Both Same Selections..");
            //                $get(CtrlTo1).focus();
            //                return false;
            //            }
            if (SelIndexCtrlFrom2 > SelIndexCtrlFrom1) {
                alert("Second Selection Of From Age Cannot Be Greater Than First Selection..");
                $get(CtrlFrom2).focus();
                return false;
            }
            if (SelIndexCtrlTo2 > SelIndexCtrlTo1) {
                alert("Second Selection Of To Age Cannot Be Greater Than First Selection..");
                $get(CtrlTo2).focus();
                return false;
            }

            switch (valCtrlFrom1) {
                case (valCtrlFrom1 = "D"):
                    totFromDays = totFromDays + ($get(CtrltxtFrom1).value * 1);
                    break;
                case (valCtrlFrom1 = "W"):
                    totFromDays = totFromDays + ($get(CtrltxtFrom1).value * 7);
                    break;
                case (valCtrlFrom1 = "M"):
                    totFromDays = totFromDays + ($get(CtrltxtFrom1).value * 30);
                    break;
                case (valCtrlFrom1 = "Y"):
                    totFromDays = totFromDays + ($get(CtrltxtFrom1).value * 365);
                    break;
            }

            switch (valCtrlFrom2) {
                case (valCtrlFrom2 = "D"):
                    totFromDays = totFromDays + ($get(CtrltxtFrom2).value * 1);
                    break;
                case (valCtrlFrom2 = "W"):
                    totFromDays = totFromDays + ($get(CtrltxtFrom2).value * 7);
                    break;
                case (valCtrlFrom2 = "M"):
                    totFromDays = totFromDays + ($get(CtrltxtFrom2).value * 30);
                    break;
                case (valCtrlFrom2 = "Y"):
                    totFromDays = totFromDays + ($get(CtrltxtFrom2).value * 365);
                    break;
            }

            switch (valCtrlTo1) {
                case (valCtrlTo1 = "D"):
                    totToDays = totToDays + ($get(CtrltxtTo1).value * 1);
                    break;
                case (valCtrlTo1 = "W"):
                    totToDays = totToDays + ($get(CtrltxtTo1).value * 7);
                    break;
                case (valCtrlTo1 = "M"):
                    totToDays = totToDays + ($get(CtrltxtTo1).value * 30);
                    break;
                case (valCtrlTo1 = "Y"):
                    totToDays = totToDays + ($get(CtrltxtTo1).value * 365);
                    break;
            }

            switch (valCtrlTo2) {
                case (valCtrlTo2 = "D"):
                    totToDays = totToDays + ($get(CtrltxtTo2).value * 1);
                    break;
                case (valCtrlTo2 = "W"):
                    totToDays = totToDays + ($get(CtrltxtTo2).value * 7);
                    break;
                case (valCtrlTo2 = "M"):
                    totToDays = totToDays + ($get(CtrltxtTo2).value * 30);
                    break;
                case (valCtrlTo2 = "Y"):
                    totToDays = totToDays + ($get(CtrltxtTo2).value * 365);
                    break;
            }
            //            alert(totFromDays);
            //            alert("To" + totToDays);
            if (totToDays.value != 0) {
                if (totFromDays > totToDays) {
                    alert("From Value Cannot Be Greater Than To Value");
                    return false;
                }
            }
            totFromDays = 0;
            totToDays = 0;
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>





            <div class="container-fluid header_main">
                <div class="col-md-3">
                    <h2>Immunization Schedule</h2>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="text-center text-success relativ alert_new" /></div>
                <div class="col-md-3 text-right pull-right">
                    <asp:Button ID="btnNew" runat="server" CausesValidation="true" OnClick="btnNew_OnClick"
                        ToolTip="New" CssClass="btn btn-primary" Text="New" Visible="true" />
                    <asp:Button ID="ibtnSaveImmunizationBabySchedule" OnClientClick="return Validation('ctl00_ContentPlaceHolder1_ddlFrom1','ctl00_ContentPlaceHolder1_ddlFrom2','ctl00_ContentPlaceHolder1_ddlAgeTo1','ctl00_ContentPlaceHolder1_ddlAgeTo2','ctl00_ContentPlaceHolder1_txtAgeFrom1','ctl00$ContentPlaceHolder1$txtAgeFrom2','ctl00_ContentPlaceHolder1_txtAgeTo1','ctl00_ContentPlaceHolder1_txtAgeTo2','ctl00_ContentPlaceHolder1_ddlImmunizationName');"
                        runat="server" CssClass="btn btn-primary" CausesValidation="true" OnClick="SaveImmunizationBabySchedule_OnClick"
                        ToolTip="Save" Width="50px" ValidationGroup="Save" Text="Save" />
                    <asp:Button ID="ibtnUpdateImmunizationBabySchedule" runat="server" CssClass="btn btn-primary"
                        CausesValidation="true" OnClick="UpdateImmunizationBabySchedule_OnClick" ToolTip="Update"
                        Width="50px" ValidationGroup="Save" Text="Update" Visible="false" />
                    <asp:ValidationSummary ID="VSibtnSaveImmunizationBabySchedule" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Save" />
                </div>
            </div>



            <div class="container-fluid subheading_main">
                <div class="row">
                    <div class="col-md-3 form-group">
                        <div class="col-md-4 label1">
                            <asp:Literal ID="ltrlImmunizationName" runat="server" Text="Name"></asp:Literal><span
                                style="color: Red">*</span>
                        </div>
                        <div class="col-md-8">
                            <telerik:RadComboBox ID="ddlImmunizationName" runat="server" Width="95%" onkeydown="NextTab();"
                                EmptyMessage="Select" Filter="Contains">
                            </telerik:RadComboBox>



                        </div>
                    </div>



                    <div class="col-md-4 form-group">
                        <div class="col-md-3 label1">
                            <asp:Literal ID="ltrlAgeFrom" runat="server" Text="Age From"></asp:Literal><span
                                style="color: Red">*</span>
                        </div>


                        <div class="col-md-9">

                            <asp:TextBox ID="txtAgeFrom1" SkinID="textbox" runat="server" TextMode="SingleLine"
                                MaxLength="2" onkeydown="NextTab();" Width="30px" Style="text-align: right"></asp:TextBox>
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                TargetControlID="txtAgeFrom1" FilterType="Numbers">
                            </AJAX:FilteredTextBoxExtender>




                            <telerik:RadComboBox ID="ddlFrom1" runat="server" Width="70px" onkeydown="NextTab();"
                                EmptyMessage="Select">
                                <Items>
                                    <telerik:RadComboBoxItem Value="0" Text="Select" />
                                    <telerik:RadComboBoxItem Value="D" Text="Day(s)" />
                                    <telerik:RadComboBoxItem Value="W" Text="Week(s)" />
                                    <telerik:RadComboBoxItem Value="M" Text="Month(s)" />
                                    <telerik:RadComboBoxItem Value="Y" Text="Year(s)" />
                                </Items>
                            </telerik:RadComboBox>

                            <asp:TextBox ID="txtAgeFrom2" SkinID="textbox" runat="server" TextMode="SingleLine"
                                MaxLength="2" onkeydown="NextTab();" Width="30px" Style="text-align: right"></asp:TextBox>
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                TargetControlID="txtAgeFrom2" FilterType="Numbers">
                            </AJAX:FilteredTextBoxExtender>

                            <telerik:RadComboBox ID="ddlFrom2" runat="server" Width="70px" EmptyMessage="Select">
                                <Items>
                                    <telerik:RadComboBoxItem Value="0" Text="Select" />
                                    <telerik:RadComboBoxItem Value="D" Text="Day(s)" />
                                    <telerik:RadComboBoxItem Value="W" Text="Week(s)" />
                                    <telerik:RadComboBoxItem Value="M" Text="Month(s)" />
                                    <telerik:RadComboBoxItem Value="Y" Text="Year(s)" />
                                </Items>
                            </telerik:RadComboBox>

                        </div>
                    </div>



                    <div class="col-md-4 form-group">
                        <div class="col-md-3 label1">
                            <asp:Literal ID="ltrlAgeTo" runat="server" Text="Age To"></asp:Literal>
                        </div>
                        <div class="col-md-9">

                            <asp:TextBox ID="txtAgeTo1" SkinID="textbox" runat="server" TextMode="SingleLine"
                                MaxLength="2" onkeydown="NextTab();" Width="30px" Style="text-align: right"></asp:TextBox>
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" Enabled="True"
                                TargetControlID="txtAgeTo1" FilterType="Numbers">
                            </AJAX:FilteredTextBoxExtender>




                            <telerik:RadComboBox ID="ddlAgeTo1" runat="server" Width="70px" EmptyMessage="Select">
                                <Items>
                                    <telerik:RadComboBoxItem Value="0" Text="Select" />
                                    <telerik:RadComboBoxItem Value="D" Text="Day(s)" />
                                    <telerik:RadComboBoxItem Value="W" Text="Week(s)" />
                                    <telerik:RadComboBoxItem Value="M" Text="Month(s)" />
                                    <telerik:RadComboBoxItem Value="Y" Text="Year(s)" />
                                </Items>
                            </telerik:RadComboBox>






                            <asp:TextBox ID="txtAgeTo2" SkinID="textbox" runat="server" TextMode="SingleLine"
                                MaxLength="2" onkeydown="NextTab();" Width="30px" Style="text-align: right"></asp:TextBox>
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                TargetControlID="txtAgeTo2" FilterType="Numbers">
                            </AJAX:FilteredTextBoxExtender>




                            <telerik:RadComboBox ID="ddlAgeTo2" runat="server" Width="70px" EmptyMessage="Select">
                                <Items>
                                    <telerik:RadComboBoxItem Value="0" Text="Select" />
                                    <telerik:RadComboBoxItem Value="D" Text="Day(s)" />
                                    <telerik:RadComboBoxItem Value="W" Text="Week(s)" />
                                    <telerik:RadComboBoxItem Value="M" Text="Month(s)" />
                                    <telerik:RadComboBoxItem Value="Y" Text="Year(s)" />
                                </Items>
                            </telerik:RadComboBox>

                        </div>
                    </div>



                    <div class="col-md-1 text-left label1" style="margin-left: -51px; width: 144px;">

                        <asp:LinkButton ID="lnkImmunizationMaster" runat="server" Text="Immunization Master" OnClick="lnkImmunizationMaster_OnClick"></asp:LinkButton>

                    </div>

                </div>


            </div>





            <%--Akshay Tirathram 24-08-2022--%>
                <div class="row">
                    <div class="col-md-3 form-group">
                        <div class="col-md-4 label1">
                            <asp:Literal ID="litDoctor" runat="server" Text="Doctor"></asp:Literal><span
                                style="color: Red">*</span>
                        </div>
                        <div class="col-md-8">
                            <telerik:RadComboBox ID="ddlDoctor" runat="server" SkinID="DropDown" TabIndex="3" AutoPostBack="true" EmptyMessage="Select"
                                EnableLoadOnDemand="true" Filter="Contains" Width="100%"/>
                        </div>
                    </div>
                </div>


            </div>






































            <div style="height: 390px; width: 100%; overflow: auto;">
                <asp:GridView ID="gvImmunizationBaby" SkinID="gridview" CellPadding="4" runat="server"
                    AutoGenerateColumns="false" DataKeyNames="ScheduleID" ShowHeader="true" Width="100%"
                    PagerSettings-Mode="NumericFirstLast" ShowFooter="false" PagerSettings-Visible="true"
                    OnRowDataBound="gvImmunizationBaby_OnRowDataBound" OnRowCommand="gvImmunizationBaby_OnRowCommand"
                    OnRowEditing="gvImmunizationBaby_OnRowEditing" OnRowCancelingEdit="gvImmunizationBaby_OnRowCancelingEdit"
                    OnRowUpdating="gvImmunizationBaby_OnRowUpdating" OnSelectedIndexChanged="gvImmunizationBaby_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="ScheduleID" HeaderText="ScheduleID" ReadOnly="true" />
                        <asp:BoundField DataField="ImmunizationId" HeaderText="ImmunizationId" ReadOnly="true" />
                        <asp:TemplateField Visible="True" HeaderText="S No">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Immunization Name">
                            <ItemTemplate>
                                <asp:Label ID="lblImmunizationNameGrid" SkinID="label" runat="server" Text=' <%#Eval("ImmunizationName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--Akshay Tirathram 21-Sep-2022--%>
                        <asp:TemplateField HeaderText="Doctor Name">
                            <ItemTemplate>
                                <asp:Label ID="lblDoctorNameGrid" SkinID="label" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Age From">
                            <ItemTemplate>
                                <asp:Label ID="lblAgeFromGrid" SkinID="label" runat="server" Text=' <%#Eval("FromAge")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Age To">
                            <ItemTemplate>
                                <asp:Label ID="lblAgeToGrid" SkinID="label" runat="server" Text=' <%#Eval("ToAge")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblGridActive1" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Active" HeaderText="Active" ReadOnly="true" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                    CommandName="DeActivate" CommandArgument='<%#Eval("ScheduleID")%>' ToolTip="DeActivate"
                                    ValidationGroup="Cancel" CausesValidation="true" />
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowSelectButton="true" />
                        <asp:BoundField DataField="AgeFrom1" />
                        <asp:BoundField DataField="AgeFrom2" />
                        <asp:BoundField DataField="AgeFromType1" />
                        <asp:BoundField DataField="AgeFromType2" />
                        <asp:BoundField DataField="AgeTo1" />
                        <asp:BoundField DataField="AgeTo2" />
                        <asp:BoundField DataField="AgeToType1" />
                        <asp:BoundField DataField="AgeToType2" />
                    </Columns>
                </asp:GridView>
            </div>
            <asp:ValidationSummary ID="VSGrid" runat="server" ShowMessageBox="True" ShowSummary="False"
                ValidationGroup="Edit" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
