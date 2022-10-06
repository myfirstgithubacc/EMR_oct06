<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="HospitalMandatoryFieldsSetup.aspx.cs" Inherits="MPages_HospitalMandatoryFieldsSetup"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvData.ClientID%>");
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

    <script language="javascript" type="text/javascript">
        function GetSelectedRowChkEnable(ctrl) {
            //ctl00_ContentPlaceHolder1_gvData_ctl09_ChkIsPreferableField
            //ctl00_ContentPlaceHolder1_gvData_ctl09_ChkIsMandatoryField
            //ctl00_ContentPlaceHolder1_gvData_ctl09_ChkEnable
            // var customerId1 = row.cells[1].childNodes[rowIndex].id
            //document.getElementById("ctl00_ContentPlaceHolder1_gvData_ctl02_ChkIsPreferableField").checked
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").innerHTML = "";
            var row = ctrl.parentNode.parentNode;
            var rowIndex = row.rowIndex + 1;

            if (rowIndex < 10)
                rowIndex = "0" + rowIndex;

            var GridRow = "ctl00_ContentPlaceHolder1_gvData_ctl" + rowIndex + "_";
          
            var ObjChkEnable = document.getElementById("" + GridRow + "ChkEnable");
            var ObjChkIsMandatoryField = document.getElementById("" + GridRow + "ChkIsMandatoryField");
            var ObjChkIsPreferableField = document.getElementById("" + GridRow + "ChkIsPreferableField");
            if (ObjChkEnable.checked == false) {
                ObjChkIsMandatoryField.checked = false;
                ObjChkIsPreferableField.checked = false;
            }
        }

        function GetSelectedRowChkIsMandatoryField(ctrl) {
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").innerHTML = "";
            var row = ctrl.parentNode.parentNode;
            var rowIndex = row.rowIndex + 1;

            if (rowIndex < 10)
                rowIndex = "0" + rowIndex;

            var GridRow = "ctl00_ContentPlaceHolder1_gvData_ctl" + rowIndex + "_";

            var ObjChkIsMandatoryField = document.getElementById("" + GridRow + "ChkIsMandatoryField");
            var ObjChkEnable = document.getElementById("" + GridRow + "ChkEnable");
             
            if (ObjChkIsMandatoryField.checked == true) {
                ObjChkEnable.checked = true;
            }
        }

        function GetSelectedRowChkIsPreferableField(ctrl) {
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").innerHTML = "";
            var row = ctrl.parentNode.parentNode;
            var rowIndex = row.rowIndex + 1;

            if (rowIndex < 10)
                rowIndex = "0" + rowIndex;

            var GridRow = "ctl00_ContentPlaceHolder1_gvData_ctl" + rowIndex + "_";

            var ObjChkIsPreferableField = document.getElementById("" + GridRow + "ChkIsPreferableField");
            var ObjChkEnable = document.getElementById("" + GridRow + "ChkEnable");

            if (ObjChkIsPreferableField.checked == true) {
                ObjChkEnable.checked = true;
            }
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-4">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="Hospital Mandatory Fields Setup" ToolTip="" /></h2>
                </div>
                <div class="col-md-7 col-sm-5 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" ForeColor="Green" Font-Bold="true" />
                </div>
                <div class="col-md-2 col-sm-3 text-right">
                    <asp:Button ID="btnNew" runat="server" ToolTip="New Record" CssClass="btn btn-default" Text="New" OnClick="btnNew_OnClick" />
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data" CssClass="btn btn-primary" OnClick="btnSaveData_OnClick" Text="Save" />
                </div>
            </div>


            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-4 col-sm-4">
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-5 PaddingRightSpacing">
                                <asp:Label ID="Label1" runat="server" Text="Facility" /><span style="color: Red">*</span>
                            </div>
                            <div class="col-md-8 col-sm-7 PaddingRightSpacing">
                                <telerik:RadComboBox ID="ddlFacilityId" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlFacilityId_OnSelectedIndexChanged" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-5 PaddingRightSpacing">
                                <asp:Label ID="Label2" runat="server" Text="Module Name" /><span style="color: Red">*</span>
                            </div>
                            <div class="col-md-8 col-sm-7 PaddingRightSpacing">
                                <telerik:RadComboBox ID="ddlModule" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_OnSelectedIndexChanged" />
                            </div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-5 PaddingRightSpacing">
                                <asp:Label ID="Label4" runat="server" Text="Page Name" /><span style="color: Red">*</span>
                            </div>
                            <div class="col-md-8 col-sm-7 PaddingRightSpacing">
                                <telerik:RadComboBox ID="ddlPage" runat="server" Width="100%" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlPage_OnSelectedIndexChanged" />
                            </div>
                        </div>

                    </div>
                    <div class="col-md-8 col-sm-8">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvData" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="pnl2" runat="server" Height="450px" Width="100%" ScrollBars="Auto">
                                    <asp:GridView ID="gvData" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="False"
                                        BackColor="White" BorderColor="#e0e0e0" ShowFooter="false" Width="100%" OnRowDataBound="gvData_RowDataBound" OnRowCommand="gvData_OnRowCommand">
                                        <%--OnRowCommand="gvData_OnRowCommand"--%>
                                        <EmptyDataTemplate>
                                            <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="50px" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="center" Visible="false">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" Visible="false" />
                                                    <asp:Label ID="lblActive" runat="server" Text="Active" Visible="false" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" Checked='<%#Eval("IsChk")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText='Module Name'>            OnCheckedChanged="CheckBoxChanged" AutoPostBack="true"
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModuleName" runat="server" SkinID="label" Text='<%#Eval("ModuleName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Page Name'>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPageName" runat="server" SkinID="label" Text='<%#Eval("PageName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText='Field Name'>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='IsPreferable' HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkIsPreferableField" runat="server" Checked='<%#Eval("IsPreferrableField")%>' OnClick="GetSelectedRowChkIsPreferableField(this)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='IsMandatory' HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkIsMandatoryField" runat="server" Checked='<%#Eval("IsMandatoryField")%>' OnClick="GetSelectedRowChkIsMandatoryField(this)"  OnCheckedChanged="ChkIsMandatoryField_CheckedChanged"   />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Enable' HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkEnable" runat="server" Checked='<%#Eval("IsEnableField")%>' OnCheckedChanged="ChkEnable_CheckedChanged" OnClick="GetSelectedRowChkEnable(this)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" ToolTip="Click here to Edit this record"
                                                        CommandName="EditRow" CausesValidation="false" CommandArgument='<%#Eval("FieldId")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>


                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
