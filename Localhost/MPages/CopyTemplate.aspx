<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="CopyTemplate.aspx.cs" Inherits="MPages_CopyTemplate" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link href="../Include/css/chosen.css" rel="Stylesheet" type="text/css" />
        <script src="../../Include/JS/chosen.jquery.js" type="text/javascript"></script>
        <style>
            #ctl00_ContentPlaceHolder1_gvSpecializationTemplate th{
                 background:#D8D8D8; font-size:90%;
            }

        </style>
        <script type="text/javascript">
            function openRadWindow(TemplateId, TemplateTypeId) {

                //var oWnd = radopen("/EMR/Templates/Default.aspx?TmpId=" + TemplateId + "&TmpTId=" + TemplateTypeId, "RadWindowForNew");
                // /EMR/Templates/TemplateLibraryView.aspx?DisplayMenu=1&CopyTemplate=1&IsEMRPopUp=1&TemplateId=" + e.Row.Cells[1].Text +"" + "');
                var oWnd = radopen("/EMR/Templates/TemplateLibraryView.aspx?DisplayMenu=1&CopyTemplate=1&IsEMRPopUp=1&TemplateId=" + TemplateId, "RadWindowForNew");
                oWnd.setSize(1350, 600);
                oWnd.Center();
            }



            function SelectAll(id, GridCtrl) {
                //get reference of GridView control
                var grid = document.getElementById(GridCtrl);
                //variable to contain the cell of the grid
                var cell;

                if (grid.rows.length > 0) {
                    //loop starts from 1. rows[0] points to the header.
                    for (i = 1; i < grid.rows.length; i++) {
                        //get the reference of first column
                        cell = grid.rows[i].cells[0];

                        //loop according to the number of childNodes in the cell
                        for (j = 0; j < cell.childNodes.length; j++) {
                            //if childNode type is CheckBox                 
                            if (cell.childNodes[j].type == "checkbox") {
                                //assign the status of the Select All checkbox to the cell checkbox within the grid
                                cell.childNodes[j].checked = document.getElementById(id).checked;
                            }
                        }
                    }
                }
            }

        </script>

    </telerik:RadCodeBlock>
    <%--     <asp:DropDownList ID="ddlSpecialisation" runat="server" class="chosen-select" AppendDataBoundItems="true" data-placeholder="[ Select ]"
                                 DataTextField="Name" DataValueField="Id" Width="100%">
                                <asp:ListItem Text=" [ Select ] " Value="0" ></asp:ListItem>
                            </asp:DropDownList>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
            <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
            <div class="container-fluid header_main margin_bottom">
                <div class="col-md-3">
                    <h2>Clinical Templates Library </h2>
                </div>

                <div class="col-md-5 text-center">

                    <asp:UpdatePanel ID="upNewSave" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lbl_Msg" runat="server" CssClass="relativ alert_new text-success"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>

                <div class="col-md-3 text-right pull-right">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnCopy" runat="server" Visible="false" OnClick="btnCopy_Click" Text="Copy" CssClass="btn btn-primary" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>


            <div class="container-fluid form-group">
                <div class="col-md-5">
                    <div class="row">
                        <%-- <div class="col-md-4">Public Template(s)</div>--%>
                        <%-- <div class="col-md-8"> <asp:Label ID="lblTotalCount" runat="server" Text=""></asp:Label></div>--%>
                    </div>

                </div>

            </div>

            <div class="container-fluid form-group">

                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-4">Template</div>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtTemplate" runat="server" Width="100%"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Literal ID="Literal3" runat="server" Text="Specialisation" />
                        </div>
                        <div class="col-md-8">
                            <script type="text/javascript">

                                $(document).ready(function () {
                                    InIEvent();
                                });

                                function InIEvent() {
                                    var config = {
                                        '.chosen-select': {},
                                        '.chosen-select-deselect': { allow_single_deselect: true },
                                        '.chosen-select-no-single': { disable_search_threshold: 10 },
                                        '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
                                        '.chosen-select-width': { width: "100%" }
                                    }
                                    for (var selector in config) {
                                        $(selector).chosen(config[selector]);
                                    }
                                }
                                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InIEvent);
                            </script>
                          <%--  <asp:DropDownList ID="ddlSpecialisation" runat="server" class="chosen-select" AppendDataBoundItems="true" AutoPostBack="true"
                                onkeydown="Tab();" TabIndex="2" DataTextField="Name" DataValueField="Id" Width="100%" >
                           <asp:ListItem Text=" [ Select ] " Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>--%>

                              <asp:DropDownList ID="ddlSpecialisation" runat="server" class="chosen-select" AppendDataBoundItems="true" 
                               onkeydown="Tab();"   TabIndex="2" DataTextField="Name" DataValueField="Id" Width="100%" >
                           <asp:ListItem Text=" [ Select ] " Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </div>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Literal ID="ltrlTemplateType" runat="server" Text="Template&nbsp;Type" />
                        </div>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlTemplateType" runat="server" AppendDataBoundItems="true"
                                onkeydown="Tab();" TabIndex="4" DataTextField="TypeName" DataValueField="ID" Width="100%">
                                <asp:ListItem Text=" [ Select ] " Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>













                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnRefresh" ToolTip="Refresh" CssClass="btn btn-primary" Text="Refresh"
                            OnClick="btnRefresh_OnClick" runat="server" />
                        <asp:Button ID="btnClear" ToolTip="Clear" CssClass="btn btn-primary" Text="Clear"
                            OnClick="btnClear_OnClick" runat="server" />


                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnClear" />
                        <asp:PostBackTrigger ControlID="btnRefresh" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:Panel ID="pnlTemplate" runat="server" Width="100%" Height="470px" ScrollBars="auto">
                <asp:UpdatePanel ID="UpdatePanel33" runat="server">
                    <ContentTemplate>
                       <div class="col-md-4" style="max-height:470px; overflow-y:scroll;">


                         <asp:GridView ID="gvSpecializationTemplate" runat="server" AutoGenerateColumns="false " SkinID="gridview" >
                            <Columns>
                                <asp:TemplateField HeaderText="Specialisation">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpecilization" runat="server" Text='<%#Eval("Specialisation")%>' />
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Count">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblTemplateCount" runat="server" Text='<%#Eval("")%>' />--%>
                                        <asp:LinkButton ID="lnkTemplateCount" runat="server" Text='<%#Eval("TemplateCount")%>'
                                            CssClass="text-center" OnClick="lnkTemplateCount_OnClik" />
                                    </ItemTemplate>

                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                        </div>
                        
                        <div class="col-md-8">
                        <asp:GridView ID="gvTemplates" SkinID="gridview" CellPadding="4" runat="server" AutoGenerateColumns="false" Height="400px"
                            DataKeyNames="TemplateId, TemplateTypeId" ShowHeader="true" Width="100%" AllowPaging="true"
                            ShowFooter="false" OnRowDataBound="gvTemplates_OnRowDataBound" PageSize="18" OnPageIndexChanging="gvTemplates_PageIndexChanging">
                            <SelectedRowStyle BackColor="LightPink" />
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAllItems" runat="server"></asp:CheckBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkT" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                    <HeaderStyle Width="20px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TemplateId" HeaderText="TemplateID" />

                                <%-- <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Template" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("TemplateName")%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Specialization" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpecialisation" SkinID="label" runat="server" Text='<%#Eval("Specialisation")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="Code" HeaderText="Code" HeaderStyle-HorizontalAlign="Left"
                                    ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Type" HeaderText="Type" HeaderStyle-HorizontalAlign="Left"
                                    ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtndetails" runat="server" CommandName="Details" OnClick="lbtndetails_OnClik" CommandArgument='<%# Container.DisplayIndex%>'>View</asp:LinkButton>
                                        <%--<asp:LinkButton ID="lbtndetails" runat="server">Details</asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TemplateTypeId" />
                                <asp:BoundField DataField="SpecialisationId" HeaderText="SpecialisationId" />
                            </Columns>
                        </asp:GridView>
                       </div>
                        
                         <div class="col-md-8">
                            <asp:Label ID="lblTotalCount" runat="server" Text=""></asp:Label>
                        </div>

                        <%--                                                <telerik:RadWindowManager ID="RadWindowManager" runat="server" VisibleStatusbar="false">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindowForNew" Height="1300px" Width="1000px" runat="server" VisibleStatusbar="false"
                                            Behaviors="Close,Move">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>--%>
                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Behaviors="Close,Move,Pin,Resize,Maximize">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>







            <%-- <asp:Panel ID="pnlHospitalLocation" runat="server" Width="100%" Height="450px" ScrollBars="auto">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvHospitalLocation" SkinID="gridview" CellPadding="4" runat="server"
                                        AutoGenerateColumns="false" DataKeyNames="Id" ShowHeader="true" Width="100%"
                                        AllowPaging="False" ShowFooter="false" OnRowDataBound="gvHospitalLocation_OnRowDataBound">
                                        <SelectedRowStyle BackColor="LightPink" />
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAllItems" runat="server"></asp:CheckBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkH" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Id" HeaderText="Id" />
                                            <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHospitalLocationName" runat="server" Text='<%#Eval("Name")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NPI" HeaderText="NPI" HeaderStyle-HorizontalAlign="Left"
                                                Visible="false" ReadOnly="true" />
                                            <asp:BoundField DataField="EIN" HeaderText="EIN" HeaderStyle-HorizontalAlign="Left"
                                                Visible="false" ReadOnly="true" />
                                            <asp:BoundField DataField="BillingAddress" HeaderText="Address" HeaderStyle-HorizontalAlign="Left"
                                                Visible="false" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
