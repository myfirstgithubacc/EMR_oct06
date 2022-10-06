<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PatientVitaldetails.aspx.cs" Inherits="EMR_Vitals_PatientVitaldetails"
    Title="Vital Details" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
   
    <script type="text/javascript">

        function returnToParent() {
            var oArg = new Object();

            oArg.VitalDetailsId = $get('<%=hdnVitalDetailsId.ClientID%>').value;
            oArg.VitalDate = $get('<%=hdnVitalDate.ClientID%>').value;

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>

    <script type="text/javascript" language="javascript">

        function SelectCheckBox() {
            var flag = 0;
            TotalChkBx = parseInt('<%= this.gvvitalsCancel.Rows.Count %>');
            if (TotalChkBx == 0) {
                return;
            }
            var TargetBaseControl = document.getElementById('<%= this.gvvitalsCancel.ClientID %>');
            var TargetChildControl = "chkCancel";
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var iCount = 0; iCount < Inputs.length; ++iCount) {
                //if (Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetChildControl, 0) >= 0) 
                //{
                //alert (Inputs[iCount].checked);
                if (Inputs[iCount].checked == true) {
                    flag = 1;
                    //Inputs[iCount].checked = CheckBox.checked;
                }
                //}
            }
            if (flag == 1) {

                return true;
            }
            else {
                alert("Select at least one row.")
                return false;
            }                                                                                                
        }

        // for grid row cell checkbox selection


        function SelectheaderCheckboxes(headerchk) {
            debugger
            var gvcheck = document.getElementById('gvvitalsCancel');
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
                //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        // Call Graph

        function setValue(val, valName, IsInvest) {
            $get('<%=hdnVitalvalue.ClientID%>').value = val;
            $get('<%=hdnVitalName.ClientID%>').value = valName;
            if (IsInvest == 'False') {
                var oWnd = radopen("/EMR/Vitals/Vitalgraph.aspx?Value=" + $get('<%=hdnVitalvalue.ClientID%>').value +
                                        "&Name=" + $get('<%=hdnVitalName.ClientID%>').value, "RadWindow1");

                oWnd.setSize(1000, 600)
                oWnd.center();
                oWnd.VisibleStatusbar = "false";
                oWnd.set_status(""); // would like to remove statusbar, not just blank it
                oWnd.maximize();
                oWnd.Skin = "Metro";
            }
        }
        //End Graph


    </script>

    <div class="container-fluid">
        <div class="row header_main hidden">
            <div class="col-md-9 col-sm-9 col-xs-10 text-center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-2 text-right">
                <asp:Button ID="btnclose" runat="server" CssClass="btn-primary" Text="Close" OnClientClick="window.close();" />
            </div>
        </div>

                    <div class="row" id="trPrevVitalTemplate" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnVitalDetailsId" runat="server" />
                            <asp:HiddenField ID="hdnVitalDate" runat="server" />
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-md-10 col-sm-10 col-xs-10 p-t-b-5 box-col-checkbox">
                                        <asp:RadioButtonList ID="rblCancelRemarks" runat="server" RepeatDirection="Horizontal" Width="100%">
                                            <asp:ListItem Selected="True" Text="Wrong Patient" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Wrong Date Time" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Wrong Value" Value="3"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-2 p-t-b-5 text-right">
                                        <asp:Button ID="ibtnCancelOk" CssClass="btn btn-primary" OnClick="ibtnCancelOk_Click" runat="server" Text="Remove Vitals"
                                             OnClientClick="return SelectCheckBox();" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                            <asp:GridView ID="gvvitalsCancel" runat="server" Width="100%" AutoGenerateColumns="False"
                                CellPadding="4" OnPageIndexChanging="gvvitalsCancel_PageIndexChanging" PageSize="18"
                                AllowPaging="true" SkinID="gridview" CssClass="table table-responsive table-hover table-condensed table-striped" GridLines="Both" OnRowDataBound="gvvitalsCancel_RowDataBound"
                                OnRowCommand="gvvitalsCancel_OnRowCommand" style="margin-bottom: 10px;">
                                <Columns>
                                    <asp:BoundField HeaderText="Sl. No." DataField="SlNo" Visible="false" />
                                    <asp:BoundField HeaderText="Vital Date" DataField="VitalDate" HeaderStyle-Width="130px" />
                                    <asp:BoundField HeaderText="Vital" DataField="VitalSignName" HeaderStyle-Width="180px" />
                                    <asp:BoundField HeaderText="Value" DataField="VitalValue" HeaderStyle-Width="180px" />
                                    <asp:TemplateField HeaderText="Billable">
                                        <ItemTemplate>
                                            <asp:Label ID="lblbillable" runat="server" Text='<%# Billable(Eval("Billable").ToString()) %>'></asp:Label>
                                            <asp:HiddenField ID="hdnEncodedBy" runat="server" Value='<% #Eval("EncodedBy") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Remarks" DataField="Remarks"/>
                                    <asp:BoundField HeaderText="Entered By" DataField="EnteredBy" HeaderStyle-Width="180px" />
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkheader" runat="server" AutoPostBack="true" OnCheckedChanged="chkheader_OnCheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkCancel" runat="server" />
                                            <asp:Panel ID="pnlCancelDetail" Style="visibility: hidden; position: absolute;" BorderStyle="Solid"
                                                BorderWidth="1px" BackColor="#E0EBFD" runat="server">
                                                <asp:GridView ID="gvCancelDetail" runat="server" AutoGenerateColumns="false">
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#507CD1" HorizontalAlign="Left" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Cancelled By" HeaderStyle-Width="200px" DataField="CancelledBy" />
                                                        <asp:BoundField HeaderText="Cancelled Date" HeaderStyle-Width="150px" DataField="CancelledDate" />
                                                        <asp:BoundField HeaderText="Reason" HeaderStyle-Width="150px" DataField="Reason" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hdnIsInvestigation" runat="server" Value='<%#Bind("IsInvestigation") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Id" DataField="Id" />
                                    <asp:BoundField HeaderText="VitalId" DataField="VitalId" />
                                    <asp:BoundField HeaderText="AbNormal" DataField="AbNormal" />
                                    <asp:BoundField HeaderText="Vital Sign" DataField="DisplayName" />
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnEdit" runat="server" CommandName="VitalEdit" CausesValidation="false"
                                                CommandArgument='<%#Eval("Id")%>' ImageUrl="~/Images/edit.png" Width="14px" Height="14px"
                                                ToolTip="Click here to edit this record" />
                                            <asp:HiddenField ID="hdnFVitalDate" runat="server" Value='<% #Eval("VitalDate") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                                </div>
                            

                             <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="LblVitalAbnormal1" runat="server" Text="Abnormal Value" Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                 <asp:HiddenField ID="hdnVitalvalue" runat="server" />
                            <asp:HiddenField ID="hdnVitalName" runat="server" />
                                            </div>
                                        </div>
                                        </div>

                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                         <telerik:RadWindowManager ID="RadWindowManager1" Skin="Metro" Title="Vital Graph" runat="server" AutoSizeBehaviors="HeightProportional">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1"  runat="server" Behaviors="Close,Move,Maximize" />
                                </Windows>
                            </telerik:RadWindowManager>
                                    </div>

                                    </div>
                                 </div>
                            
                           

                           

                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ibtnCancelOk" />
                        </Triggers>
                    </asp:UpdatePanel>
                        </div>
               
    </div>
</asp:Content>
