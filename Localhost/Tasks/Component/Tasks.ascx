<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tasks.ascx.cs" Inherits="Tasks_Component_Tasks" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="aspl" TagName="ComboPatientSearch" Src="~/Include/Components/PatientSearchCombo.ascx" %>

<link href="../../Include/EMRStyle.css" rel="stylesheet" />
<link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    </style>
<telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript" language="javascript">
        function OnClientSelectedIndexChangedEventHandler_Task(sender, args) {
            var item = args.get_item();
            $get('<%=txtAccountNumber.ClientID%>').value = item != null ? item.get_attributes().getAttribute("RegistrationNo") : sender.get_attributes().getAttribute("RegistrationNo");
            $get('<%=txttest.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=btnAssigned.ClientID%>').click();
        }

    </script>
</telerik:RadScriptBlock>
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSent" runat="server" Text="Assigned" ValidationGroup="task" SkinID="Button"
                                        OnClick="btnSent_Click" CausesValidation="False" Visible="true" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPosted" runat="server" Text="Sent" SkinID="Button" ValidationGroup="task"
                                        CausesValidation="false" OnClick="btnPosted_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnNewTask" runat="server" Text="New Task" ValidationGroup="task"
                                        SkinID="Button" OnClick="btnNewTask_Click" CausesValidation="False" />
                                </td>
                                <td>
                                    <asp:Button ID="btnAddType" runat="server" Text="Add Type" ValidationGroup="task"
                                        SkinID="Button" OnClick="btnAddType_Click" CausesValidation="False" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top" align="center" width="100%">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlPosted" BackColor="White" runat="server" Width="100%" Height="450px"
                                                Visible="false" CssClass="taskPanel">
                                                <table cellpadding="0" cellspacing="0" width="690px">
                                                    <tr>
                                                        <td align="left" class="tdbgGray">
                                                            <b><span>Sent Tasks </span></b>&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <b><span>Status </span></b>&nbsp;&nbsp;
                                                            <telerik:RadComboBox ID="ddlstatus" runat="server" CausesValidation="false" Skin="Outlook"
                                                                AutoPostBack="true" ZIndex="20001" Enabled="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                               
                                                            </telerik:RadComboBox>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnClearFilter" runat="server" Text="ClearFilter" SkinID="Button"
                                                                OnClick="btnClearFilter_Click" />
                                                            <br />
                                                             
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblGridStatus1" runat="server" Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:GridView ID="grvPosted" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                DataKeyNames="TaskId" HeaderStyle-HorizontalAlign="Left" SkinID="gridview" Style="margin-bottom: 0px"
                                                                Width="100%" OnRowDataBound="grvPosted_RowDataBound" OnPageIndexChanging="grvPosted_OnPageIndexChanging"
                                                                OnSelectedIndexChanged="grvPosted_SelectedIndexChanged" OnRowDeleting="grvPosted_RowDeleting"
                                                                OnRowCreated="grvPosted_RowCreated" OnRowCommand="gvPosted_RowCommand">
                                                                <PagerSettings Mode="NextPreviousFirstLast" PageButtonCount="6" />
                                                                <RowStyle Wrap="false" />
                                                                <Columns>
                                                                 
                                                                    <asp:BoundField DataField="TaskType" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                                        HeaderStyle-Height="20%" HeaderText="Task Type" ItemStyle-CssClass="grdBorderRight">
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CreatedDate" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                                        HeaderStyle-Height="20%" HeaderText="Posted Date" ItemStyle-CssClass="grdBorderRight">
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DueTime" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                                        HeaderStyle-Height="20%" HeaderText="Due" ItemStyle-CssClass="grdBorderRight">
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TaskStatus" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                                        HeaderStyle-Height="20%" HeaderText="Status" ItemStyle-CssClass="grdBorderRight">
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PatientName" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                                        HeaderStyle-Height="20%" HeaderText="Patient" ItemStyle-CssClass="grdBorderRight">
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TaskPriority" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                                        HeaderStyle-Height="20%" HeaderText="Priority" ItemStyle-CssClass="grdBorderRight">
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Page Url">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkOpenTask" runat="server" Text="GoTo Page" CausesValidation="false"
                                                                                CommandArgument='<%#Eval("pageurl") %>' CommandName="task"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                   <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActive1" Text='<%#Eval("Active")%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                                    <asp:CommandField ShowSelectButton="true" />
                                                                    <asp:BoundField DataField="pageurl" />
                                                                      <asp:BoundField DataField="StatusColor" HeaderText="Status Color" />
                                                                </Columns>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblPostedMsg" Font-Bold="true" runat="server" ForeColor="Green"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAssigned" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSent" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNewTask" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddType" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPosted" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlstatus" />
                                            <asp:AsyncPostBackTrigger ControlID="grvPosted" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlAssigned" BackColor="White" runat="server" Width="100%" Height="450px"
                                                Visible="true" CssClass="taskPanel">
                                                <table cellpadding="1" cellspacing="1" width="700px">
                                                    <tr>
                                                        <td class="tdbgGray" align="left">
                                                            <b><span>Assigned Tasks</span></b>&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <b><span>Status </span></b>&nbsp;&nbsp;
                                                            <telerik:RadComboBox ID="ddlStatus1" runat="server" Skin="Outlook" CausesValidation="false"
                                                                Enabled="true" AutoPostBack="true" ZIndex="20002" OnSelectedIndexChanged="ddlStatus1_SelectedIndexChanged">
                                                            </telerik:RadComboBox>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnfiltertask" runat="server" Text="ClearFilter" SkinID="Button"
                                                                 />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblGridStatus" runat="server" Font-Bold="true"></asp:Label>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:GridView ID="gvTasks" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                DataKeyNames="TaskId" AllowSorting="true" 
                                                                HeaderStyle-HorizontalAlign="Left" OnPageIndexChanging="gvTasks_OnPageIndexChanging"
                                                                OnSelectedIndexChanged="gvTasks_SelectedIndexChanged" SkinID="gridview" Style="margin-bottom: 0px"
                                                                Width="100%" OnRowCreated="gvTasks_RowCreated" OnRowDataBound="gvTasks_RowDataBound"
                                                                OnRowCommand="gvTasks_RowCommand" onsorting="gvTasks_Sorting"  >
                                                                <PagerSettings Mode="NextPreviousFirstLast" PageButtonCount="6" />
                                                                <RowStyle Wrap="false" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="TaskType" SortExpression ="TaskType" HeaderStyle-Height="20%" HeaderText="Task Type"
                                                                         ItemStyle-HorizontalAlign="Left"  
                                                                        ItemStyle-VerticalAlign="Middle" >
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle  HorizontalAlign="Left" 
                                                                            VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CreatedDate" HeaderStyle-Height="20%" HeaderText="Posted Date"
                                                                        ItemStyle-CssClass="grdBorderRight" ItemStyle-HorizontalAlign="Left" 
                                                                        ItemStyle-VerticalAlign="Middle" >
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" 
                                                                            VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DueTime" HeaderStyle-Height="20%" HeaderText="Due" ItemStyle-CssClass="grdBorderRight"
                                                                        ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" >
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" 
                                                                            VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TaskStatus" HeaderStyle-Height="20%" HeaderText="Status"
                                                                        ItemStyle-CssClass="grdBorderRight" ItemStyle-HorizontalAlign="Left" 
                                                                        ItemStyle-VerticalAlign="Middle" >
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" 
                                                                            VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PatientName" HeaderStyle-Height="20%" HeaderText="Patient"
                                                                        ItemStyle-CssClass="grdBorderRight" ItemStyle-HorizontalAlign="Left" 
                                                                        ItemStyle-VerticalAlign="Middle" >
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" 
                                                                            VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TaskPriority" SortExpression="TaskPriority" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                                        HeaderStyle-Height="20%" HeaderText="Priority" ItemStyle-CssClass="grdBorderRight">
                                                                        <HeaderStyle Height="20%" />
                                                                        <ItemStyle CssClass="grdBorderRight" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="assignedby" HeaderText="Assigned By" />
                                                                    <asp:TemplateField HeaderText="Page Url">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton CausesValidation="False" ID="lnkOpenTask" runat="server" Text="GoTo Page"
                                                                                CommandArgument='<%# Eval("pageurl") %>' CommandName="task"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField ShowSelectButton="true" ItemStyle-ForeColor="Blue" >
                                                                        <ItemStyle ForeColor="Blue" />
                                                                    </asp:CommandField>
                                                                    <asp:BoundField DataField="pageurl" HeaderText="Page Url" />
                                                                    <asp:BoundField DataField="StatusColor" HeaderText="Status Color" />
                                                                </Columns>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAssigned" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSent" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNewTask" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddType" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPosted" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlStatus1" />
                                            <asp:AsyncPostBackTrigger ControlID="grvPosted" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="upAddType" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlAddType" BackColor="White" runat="server" Width="100%" Height="450px"
                                                Visible="false" CssClass="taskPanel">
                                                <table cellpadding="0" cellspacing="0" >
                                              
                                                    <tr>
                                                        <td align="center" width="400px">
                                                            <asp:RequiredFieldValidator ID="rgvTxtType" ControlToValidate="txtNewType" runat="server"
                                                                ErrorMessage="Enter a New Type" ValidationGroup="New"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                       <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top" width="400px">
                                                            <table >
                                                                <tr>
                                                                    <td valign="top" width="20%" class="tdbgGray">
                                                                        Task&nbsp;Type<span style="color: Red">*</span>
                                                                    </td>
                                                                    <td class="tdbgGray">
                                                                        <asp:TextBox ID="txtNewType" runat="server" ValidationGroup="New" SkinID="textbox"
                                                                            MaxLength="40" Width="200px"></asp:TextBox><br />
                                                                    </td>
                                                                    <td class="tdbgGray" valign="top">
                                                                        <asp:Button ID="btnAdd" Text="Save" runat="server" SkinID="Button" ValidationGroup="New"
                                                                            OnClick="btnAdd_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:GridView ID="lstTypeList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                DataKeyNames="TaskTypeId" HeaderStyle-HorizontalAlign="Left" SkinID="gridview"
                                                                Style="margin-bottom: 0px" Width="400px" OnRowEditing="lstTypeList_RowEditing"
                                                                EditRowStyle-HorizontalAlign="Left" 
                                                                OnRowUpdating="lstTypeList_RowUpdating" OnRowCancelingEdit="lstTypeList_RowCancelingEdit"
                                                                OnRowDataBound="lstTypeList_RowDataBound" >
                                                               
                                                                <PagerSettings Mode="NextPreviousFirstLast" PageButtonCount="6" />
                                                                <RowStyle Wrap="false" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Task Type" ItemStyle-HorizontalAlign="Left"
                                                                        ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltasktype" runat="server" Text='<%# Eval("Description")%>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txttasktupe" runat="server" MaxLength="40"  Width="200px" SkinID="textbox" Text='<%# Eval("Description")%>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle Wrap="False" Width="200px" />
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="True" 
                                                                            Width="200px" />
                                                                    </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGridActive1" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                            <asp:DropDownList ID="ddlGridStatus" SkinID="DropDown" runat="server" Width="70px" SelectedValue='<%#Eval("Active")%>'>
                                                                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                                                            <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </EditItemTemplate>
                                                                                    <HeaderStyle Width="90px" />
                                                                                    <ItemStyle Width="90px" />
                                                                                </asp:TemplateField>
                                                                    <asp:CommandField ButtonType="Link" SelectText="Edit" ValidationGroup="Edit" 
                                                                        ShowEditButton="true"  >
                                                                        <HeaderStyle Width="90px" />
                                                                        <ItemStyle Width="90px" />
                                                                    </asp:CommandField>
                                                                </Columns>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <EditRowStyle HorizontalAlign="Left" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                   
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAssigned" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSent" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNewTask" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddType" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPosted" />
                                            <asp:AsyncPostBackTrigger ControlID="grvPosted" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="upNewTask" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel BackColor="White" ID="pnlNewTask" runat="server" Width="100%" Visible="false"
                                                CssClass="taskPanel">
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="padding-left: 2px;" valign="top" width="50%">
                                                            <table style="border-right-style: solid; border-right-width: 1px; border-color: Gray;"
                                                                cellpadding="1" cellspacing="1">
                                                                <tr>
                                                                    <td align="left">
                                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td colspan="3" align="left" class="tdbgGray" style="height: 23px;">
                                                                                    Task Details :
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" style="height: 2px;">
                                                                                    <asp:HiddenField ID="hdnTaskId" runat="server" Value="0" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Assigned&nbsp;By
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="txtassignedby" runat="server" SkinID="label" Width="300px"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Task&nbsp;Type<span style="color: Red">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadComboBox ID="ddlTaskType" ZIndex="20003" runat="server" Width="170px" Skin="Outlook"
                                                                                        Enabled="true">
                                                                                    </telerik:RadComboBox>
                                                                                </td>
                                                                                <td>
                                                                                   Current Task Status
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Priority&nbsp;<span style="color: Red">*</span>
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadComboBox ID="ddlPriority" ZIndex="20004" runat="server" Width="170px" Skin="Outlook"
                                                                                        Enabled="true">
                                                                                    </telerik:RadComboBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                   
                                                                                    <asp:TextBox ID="lblstatus" runat="server" SkinID="textbox" ReadOnly="True" Font-Bold="True"
                                                                                        Width="70px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height="5px" colspan="3" valign="top">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" colspan="3" align="left">
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td class="txt06" align="left" style="font-size: 10">
                                                                                                <asp:Label ID="rbEmp" Text="Assign To : Employee" Checked="true" runat="server" Font-Bold="true" />
                                                                                            </td>
                                                                                            <td class="txt06" align="left" style="font-size: 10; padding-left: 5px;">
                                                                                                <asp:Label ID="rbEmpGrp" Text="Employee Group" Font-Bold="true" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Panel ID="Panel1" Height="130px" Width="185px" BorderStyle="Solid" BorderWidth="1px"
                                                                                                    runat="server" ScrollBars="Auto">
                                                                                                    <asp:CheckBoxList ID="chlstEmployee" RepeatDirection="Vertical" runat="server">
                                                                                                    </asp:CheckBoxList>
                                                                                                </asp:Panel>
                                                                                            </td>
                                                                                            <td style="padding-left: 5px;">
                                                                                                <asp:Panel ID="pnlempgrp" Height="130px" Width="185px" BorderStyle="Solid" BorderWidth="1px"
                                                                                                    runat="server" ScrollBars="Auto">
                                                                                                    <asp:CheckBoxList ID="chklstempgrp" runat="server">
                                                                                                    </asp:CheckBoxList>
                                                                                                </asp:Panel>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-top: 5px;" height="5px" colspan="3" valign="top">
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                Due<span style="color: Red">*&nbsp;</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:RadioButton ID="rbkduenow" runat="server" Text="Now" GroupName="Due" Checked="true"
                                                                                                    OnCheckedChanged="rbkduenow_CheckedChanged" AutoPostBack="true" />
                                                                                                <asp:RadioButton ID="rbduedate" runat="server" GroupName="Due" Text="On&nbsp;" OnCheckedChanged="rbduedate_CheckedChanged"
                                                                                                    AutoPostBack="true" />
                                                                                            </td>
                                                                                            <td>
                                                                                              <telerik:RadDateTimePicker ID="RadDateTimePicker1" runat="server" AutoPostBackControl="Both"
                                                                                            ZIndex="20005" Skin="Outlook" >
                                                                                        </telerik:RadDateTimePicker>
                                                                                            <%-- <telerik:RadDatePicker ID="RadDateTimePicker1" runat="server"  AutoPostBackControl="Both" MinDate="01/01/1900" ZIndex="20005" Skin="Outlook"
                                                                                              Calendar-RangeMinDate="1/1/2007 12:00 PM" DateInput-DateFormat="dd/MM/yyyy hh:mm">
                                                                                            </telerik:RadDatePicker>--%>
                                                                                               
                                                                                            </td>
                                                                                            <td>
                                                                                                <telerik:RadComboBox ID="RadComboBox1" runat="server" Height="300px" Width="50px"
                                                                                                    Skin="Outlook" AutoPostBack="True" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged"
                                                                                                    Enabled="false" ZIndex="20006">
                                                                                                </telerik:RadComboBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                &nbsp;<asp:Label ID="lbl_hhmm" Enabled="False" runat="server" Text="HH MM" 
                                                                                                    Font-Size="9px"></asp:Label></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Description<span style="color: Red">*</span>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:RequiredFieldValidator ID="rqrtxtNote" ControlToValidate="txtNote" runat="server"
                                                                                        ValidationGroup="SaveTask" Text="Enter Task Discription"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" align="left">
                                                                                    <asp:TextBox ID="txtNote" runat="server" Rows="3" MaxLength="300" TextMode="MultiLine"
                                                                                        Width="98%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        Comments
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="50%" valign="top">
                                                            <table border="0" width="100%" style="border-color: Gray" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td colspan="2" align="left" valign="top" class="tdbgGray">
                                                                                    Patient Details :
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" valign="top">
                                                                                  
                                                                                    <aspl:ComboPatientSearch ID="ComboPatientSearch_Task" runat="server" />
                                                                                </td>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <hr />
                                                                                    </td>
                                                                                </tr>
                                                                            <tr>
                                                                                <td valign="top" align="left">
                                                                                    Account #
                                                                                </td>
                                                                                <td valign="top" align="left">
                                                                                    <asp:TextBox ID="txtAccountNumber" runat="server" SkinID="textbox" Width="165px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" align="left">
                                                                                    Patient Name
                                                                                </td>
                                                                                <td valign="top" align="left">
                                                                                    <asp:TextBox ID="txtpatientName" runat="server" ReadOnly="true" SkinID="textbox"
                                                                                        Width="165px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" align="left">
                                                                                    Telephone
                                                                                </td>
                                                                                <td valign="top" align="left">
                                                                                    <telerik:RadComboBox ID="txtpNumber" runat="server" Width="170px" Skin="Outlook"
                                                                                        Enabled="true" ZIndex="20007">
                                                                                    </telerik:RadComboBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" align="left">
                                                                                    Encounter
                                                                                </td>
                                                                                <td valign="top" align="left">
                                                                                    <telerik:RadComboBox ID="ddlEncounter" runat="server" Width="170px" Skin="Outlook" ZIndex="20008"
                                                                                        Enabled="true">
                                                                                    </telerik:RadComboBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Contact Patient &nbsp;
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:RadioButtonList ID="rblstCpatient" runat="server" RepeatColumns="2">
                                                                                        <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                                                    </asp:RadioButtonList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" align="center">
                                                                                    <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                        OR</b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Contact Person
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="txtCPerson" runat="server" SkinID="textbox" Width="165px" MaxLength="50"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Telephone
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="txtCPPhone" runat="server" SkinID="textbox" Width="165px" MaxLength="50"></asp:TextBox>
                                                                                 
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    Contact Person
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:RadioButtonList ID="rblstcPerson" runat="server" RepeatColumns="2" RepeatDirection="Vertical">
                                                                                        <asp:ListItem Text="Yes" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="No" Value="1"></asp:ListItem>
                                                                                    </asp:RadioButtonList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" align="left">
                                                                                    <asp:Button ID="btnSelectTask" runat="server" Text="Select Page" SkinID="Button"
                                                                                        OnClick="btnSelectTask_Click" CausesValidation="False" />
                                                                                    <asp:Button ID="btnRemovePageLink" runat="server" Text="Remove Page" SkinID="Button"
                                                                                        OnClick="btnRemovePageLink_Click" CausesValidation="False" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" align="left">
                                                                                    <asp:TextBox ID="ltrlLink" ReadOnly="true" runat="server" Width="350px" SkinID="textbox"></asp:TextBox>
                                                                                    <%-- <asp:Label ID="ltrlLink" runat="server"></asp:Label>--%>
                                                                                    <%--<asp:Literal ID="ltrlLink" runat="server"></asp:Literal>--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                            <td>
                                                                            <table>
                                                                            <tr>
                                                                            <td>
                       
                             <asp:Label ID="Label3" Text="Status" runat="server" /> 
                            </td>
                            <td>
                    &nbsp;
                                    <asp:DropDownList ID="ddlActive" runat="server" SkinID="dropdown" 
                                    Width="100px">
                                    
                                        <asp:ListItem Text="Active" Value="1" />
                                         <asp:ListItem Text="In-Active" Value="0" />
                                    </asp:DropDownList>
                                      
                            
                                  
                                </td>
                                                                            </tr>
                                                                           
                                                                            </table>
                                                                                </td>
                                                                                <td>
                                                                                    <input id="hdnregno" runat="server" type="hidden" />
                                                                                    <input id="hdnregno1" runat="server" type="hidden" />
                                                                                    <asp:TextBox ID="txttest" runat="server" BorderStyle="None" ForeColor="White" BackColor="White"></asp:TextBox>
                                                                                </td>
                                                                                
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="left" valign="top" style="padding-left: 3px;">
                                                            <asp:TextBox ID="TextBox1" runat="server" Rows="3" MaxLength="300" TextMode="MultiLine"
                                                                Width="99%"></asp:TextBox>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-top: 5px;" colspan="2" align="center" valign="bottom">
                                                            <table cellpadding="2" cellspacing="2" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <b>Change Task Status &nbsp;</b>
                                                                        <telerik:RadComboBox ID="ddlstatusset" CausesValidation="false" runat="server" Skin="Outlook"
                                                                            Enabled="true" OnSelectedIndexChanged="ddlstatusset_SelectedIndexChanged" AutoPostBack="false" ZIndex="20009">
                                                                        </telerik:RadComboBox>
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:Button ID="Save" Text="Save" SkinID="Button" ValidationGroup="SaveTask" runat="server"
                                                                            OnClick="Save_Click" />
                                                                        <%-- <asp:Button ID="btndelete" Text="Delete" SkinID="Button"  runat="server"  />
                                                            <asp:Button ID="btnworking" Text="Working" SkinID="Button"  runat="server"  />
                                                            <asp:Button ID="btncompleted" Text="Completed" SkinID="Button"  runat="server"  />--%>
                                                                        <asp:Label ID="lbltaskMsg" runat="server" Font-Bold="true"></asp:Label>
                                                                        <asp:Button ID="btnAssigned" runat="server" CausesValidation="False" OnClick="btnAssigned_Click"
                                                                            SkinID="Button" Text="Assigned" ValidationGroup="task" BackColor="White" ForeColor="White"
                                                                            BorderStyle="None" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="5px" colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:GridView ID="gvComments" SkinID="gridview" CellPadding="4" runat="server" DataKeyNames="ID"
                                                                AutoGenerateColumns="false" ShowHeader="true" PageSize="7" Width="100%" AllowPaging="true"
                                                                OnRowDeleting="gvComments_RowDeleting" OnRowEditing="gvComments_RowEditing" OnRowCancelingEdit="gvComments_RowCancelingEdit"
                                                                OnRowUpdating="gvComments_RowUpdating" PagerSettings-Mode="NumericFirstLast"
                                                                PageIndex="0" ShowFooter="false" PagerSettings-Visible="true" OnRowDataBound="gvComments_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        &nbsp;&nbsp;&nbsp; Comment Posted By <b>
                                                                                            <%# Eval("assignedby")%>
                                                                                        </b>&nbsp;&nbsp;&nbsp;On <b>
                                                                                            <%# Eval("EncodedDate")%>
                                                                                        </b>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <%# Eval("Remarks")%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        &nbsp;&nbsp;&nbsp; Comment Posted By <b>
                                                                                            <%# Eval("EncodedBy")%>
                                                                                        </b>&nbsp;&nbsp;&nbsp;On <b>
                                                                                            <%# Eval("EncodedDate")%>
                                                                                        </b>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="txtremarks" runat="server" Text='<%# Bind("Remarks") %>' SkinID="textbox"
                                                                                            Width="320px" Rows="2" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/Images/DeleteRow.png" />
                                                                    <asp:CommandField ShowEditButton="true" />
                                                                </Columns>
                                                                <PagerSettings PageButtonCount="6" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAssigned" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSent" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNewTask" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddType" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPosted" />
                                            <asp:AsyncPostBackTrigger ControlID="grvPosted" />
                                            <asp:AsyncPostBackTrigger ControlID="gvTasks" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="upPriorityMessage" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel BackColor="White" ID="pnlPriorityMessage" runat="server" Width="100%"
                                                Visible="false" Height="200px">
                                                <table width="100%" height="190px">
                                                    <tr>
                                                        <td width="30%">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblPriorityMessageHighPriority" runat="server" Text="" Style="color: Black;
                                                                font-size: medium; text-align: left;"></asp:Label><br />
                                                            <asp:Label ID="lblPriorityMessageCheckOutPriority" runat="server" Text="" Style="color: Black;
                                                                font-size: medium; text-align: left;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAssigned" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSent" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNewTask" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddType" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPosted" />
                                            <asp:AsyncPostBackTrigger ControlID="grvPosted" />
                                            <asp:AsyncPostBackTrigger ControlID="gvTasks" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
