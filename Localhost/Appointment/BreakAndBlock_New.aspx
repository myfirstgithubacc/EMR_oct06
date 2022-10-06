<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="BreakAndBlock_New.aspx.cs" Inherits="Appointment_BreakAndBlock" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>

    <asp:Content ID="cntBreakAndBlock" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    
    <link href="../Include/css/mainStyle.css" rel="stylesheet">
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    
    
    
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">
            function pageLoad() {
                var recurrenceEditor = $find('<%=RadSchedulerRecurrenceEditor1.ClientID %>');
                if (recurrenceEditor.get_recurrenceRule() == null) {
                    $telerik.$(".rsAdvWeekly_WeekDays li span input").each(function(index, item) {
                        item.checked = false;
                    });
                }
                $telerik.$(".rsRecurrenceOptionList li:first-child").hide();
            }
        </script>

        <script type="text/javascript">
            function CloseScreen() {
                window.close();
            }
        </script>

    </telerik:RadScriptBlock>
    
     <asp:UpdatePanel ID="updBlock" runat="server"><ContentTemplate>
     
     <div class="GeneralDiv">
        
     </div>
     
    
    
    
	<!-- Patient Icon Part Start -->
   	<div class="BBreak-Top">
    	<div class="container-fluid">
    		<div class="row">
            	
                
                <div class="col-md-12 col-xs-12 features02">
              		<div class="BBreak-TopRight text-center">
              		
              		<span><asp:Label ID="lblHeader" Text="" runat="server" SkinID="label" /></span>
              		
                    	<h4>
                	        <asp:Button ID="btnSave" class="btn btn-default BBSaveBtn" role="button" runat="server" Text="Save" OnClick="btnSave_Click"/> 
                        
                            <asp:Button ID="btnUpdate" class="btn btn-default BBSaveBtn" role="button" runat="server" Text="Update Break" OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnUpdateAll" class="btn btn-default BBSaveBtn" role="button" runat="server" Text="Update All Break" OnClick="btnUpdateAll_Click" />
                            <asp:Button ID="btnDeleteAllBreak" class="btn btn-default BBSaveBtn"  role="button" runat="server" Text="Delete All" OnClick="btnDeleteAllBreak_Click" />
                            <asp:Button ID="btnDeleteBreak" class="btn btn-default BBSaveBtn"  role="button" runat="server" Text="Delete This" OnClick="btnDeleteBreak_Click" />
                      
                	        <asp:Button ID="btnNew" class="btn btn-default BBSaveBtn"  role="button" runat="server" Text="New" OnClick="btnNew_Click" />                        
                            
                       </h4>  
                       <asp:Button ID="btnClose" class="btn btn-default BBSaveBtn"  role="button" Text="Close" runat="server" ToolTip="Close" OnClientClick="window.close();" />
                    	
                    	
              		</div> 
                    
                      
              	</div> 
    		</div>
    	</div>
    </div>
	<!-- Patient Icon Part Ends -->

          
    
    
    <div class="GeneralDiv">
    	<div class="container-fluid">
    		
            
          
            <div class="col-md-12">
            	<div class="row">
                
                    <div class="GeneralDiv">
                    <span class="text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></span>
                    </div>
                
                	<!-- Break And Block Start -->
                	<div class="Registration-Right">
                       
                        <div class="BBGreen">
                            <h2><img src="../Images/Break-And-Block.png" border="0" width="24" height="24"></h2>
                            <h3>Break And Block</h3>
                        </div>
                        
                        <div class="BBlock-White">  
                            <div class="GeneralDiv">
                                
                               
                                	<div class="col-md-4 ABreakSpacingLeft bbTopBox">
                                	    <asp:RadioButtonList ID="rdoIsBlock" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Break" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Block" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                	</div>
                                
                                    <div class="col-md-4 ABreakSpacing bbTopBox01">
                                        <h2>Facility <span class="red">*</span></h2>
                                        <span class="FacilityInput">
                                            <telerik:RadComboBox ID="ddlFacility" runat="server" AllowCustomText="False" Skin="Outlook" MarkFirstMatch="False"  width="95%" SkinID="DropDown"></telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="rfvFacility" runat="server" ControlToValidate="ddlFacility" ErrorMessage="Please Select Facility" ValidationGroup="BreakAndBlock" Display="None"></asp:RequiredFieldValidator>
                                        </span>
                                    </div>
                                
                                    <div class="col-md-4 ABreakSpacingRight bbTopBox01">
                                        <asp:Label ID="lblProvider" runat="server" CssClass="bbTopBox01H2" Text='<%$ Resources:PRegistration, Doctor%>' /> <span class="red">*</span>
                                       <%-- <telerik:RadComboBox ID="RadComboBox1" runat="server" Skin="Outlook" 
                                        MarkFirstMatch="true" AllowCustomText="true" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged" />--%>
                                        
                                        
                                        <telerik:RadComboBox ID="ddlProvider" runat="server" Skin="Outlook"  MarkFirstMatch="true" AllowCustomText="true" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged" />
                                        <telerik:RadComboBox ID="ddlTheater" runat="server" EmptyMessage="[ Select ]" Visible="false" />
                                    </div>
                                
                                
                            </div>
                        </div>
                	</div>
                	<!-- Break And Block Ends -->     
                    
                    
                	<!-- Details Start -->
                	<div class="Registration-Right">
                       
                        <div class="BBGreen">
                            <h2><img src="../Images/InsuranceDetails.png" border="0" width="23" height="24"></h2>
                            <h3>Details</h3>
                        </div>
                        
                        <div class="BBlock-White">  
                            <div class="bBlockDiv">
                                
                                <div class="col-md-4 ABreakSpacingLeft bbTopBox02">
                                    <h3>Name <span class="red">*</span></h3>
                                    <asp:TextBox ID="txtBreakName" runat="server" SkinID="textbox"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvBreakName" runat="server" ControlToValidate="txtBreakName" ErrorMessage="Please enter Break Name" ValidationGroup="BreakAndBlock" Display="None"></asp:RequiredFieldValidator>
                                </div>
                                
                                <div class="col-md-4 ABreakSpacing bbTopBox01">
                                    <h2>Break/Block Date <span class="red">*</span></h2>
                                    <telerik:RadDatePicker ID="dtpDate" runat="server" DateInput-DateFormat="dd/MM/yyyy" MinDate="01/01/1901"></telerik:RadDatePicker>
                                </div>
                                
                                <div class="col-md-4 ABreakSpacingRight bbTopBox01">
                                  	<h2>Break/Block Time <span class="red">*</span></h2>
                                    <telerik:RadTimePicker ID="RadTimeFrom" DateInput-ReadOnly="true" AutoPostBack="true" OnSelectedDateChanged="RadTimeFrom_SelectedDateChanged" TimeView-Interval="15" PopupDirection="BottomLeft" TimeView-Columns="6" runat="server" Width="90px" />
                                    &nbsp; To &nbsp;
                                    <telerik:RadTimePicker ID="RadTimeTo" runat="server" AutoPostBack="true" TimeView-Interval="15" DateInput-ReadOnly="true" OnSelectedDateChanged="RadTimeTo_SelectedDateChanged" PopupDirection="BottomLeft" TimeView-Columns="6" Width="90px" />
                                </div>
                          
                          </div> 
                        </div>
                        
                        
                        <div class="BBlock-WhiteNone">  
                            <div class="bBlockDiv">
                                <div class="col-md-4 ABreakSpacingLeft bbTopBox03"><telerik:RadSchedulerRecurrenceEditor ID="RadSchedulerRecurrenceEditor1" runat="server" /></div>
                            </div>
                        </div>
                        
                  
                        
                        
                	</div>
                	<!-- Details Ends -->     
                
                </div>
            </div>
                   
    
    	</div>
    </div>
    


    <!-- Footer Part Start -->
   	<div class="GeneralDiv">
    	<div class="container-fluid">
    	
    	    <div class="row">
    	        <div class="col-md-12">
    	        
    	            <table id="Table1" cellpadding="0" cellspacing="0" runat="server" border="0" class="tableRecurrence">
                          <tr>
                            <td>
                                <asp:GridView ID="gvBreakAndBlockDetails" runat="server" SkinID="gridview" Width="100%"
                                    AllowPaging="true" PageSize="15" GridLines="None" DataKeyNames="ID" AutoGenerateColumns="false"
                                    OnRowDataBound="gvBreakAndBlockDetails_RowDataBound" OnRowCommand="gvBreakAndBlockDetails_RowCommand"
                                    OnSelectedIndexChanged="gvBreakAndBlockDetails_SelectedIndexChanged" OnPageIndexChanging="gvBreakAndBlockDetails_PageIndexChanging">
                                   
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate><asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField>
                                    <ItemTemplate><asp:Label ID="lblDoctorId" runat="server" Text='<%#Eval("DoctorId")%>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField>
                                    <ItemTemplate><asp:Label ID="lblFacilityId" runat="server" Text='<%#Eval("FacilityId")%>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Facility&nbsp;Name">
                                    <ItemTemplate><asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName")%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Name">
                                    <ItemTemplate><asp:Label ID="lblBreakName" runat="server" Text='<%#Eval("BreakName")%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Break&nbsp;Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBreakDate" runat="server" Text='<%#Eval("BreakDate")%>'></asp:Label>
                                        <asp:HiddenField ID="hdBdate" runat="server" Value='<%#Eval("BDate")%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Start&nbsp;Time">
                                    <ItemTemplate><asp:Label ID="lblStartTime" runat="server" Text='<%#Eval("StartTime")%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="End&nbsp;Time">
                                    <ItemTemplate><asp:Label ID="lblEndTime" runat="server" Text='<%#Eval("EndTime")%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Recurring">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecurrence" runat="server" Text='<%#Eval("RecurrenceRule")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Recurring ParentID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecurrenceParentId" runat="server" Text='<%#Eval("RecurrenceParentId")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Recurring">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecurrenceRuleType" runat="server" Text='<%#Eval("RecurrenceRuleType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="Edit" ButtonType="Link" ControlStyle-ForeColor="Blue"
                                    ControlStyle-Font-Underline="true" SelectText="Edit" CausesValidation="false"
                                    ShowSelectButton="true">
                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                </asp:CommandField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                            ToolTip="Delete" Width="13px" /></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                    </table>
    	        </div>
    	    
    	         
    	    </div>
    	
    	
        
        </div>
    </div>
    
    
	<!-- Footer Icon Part Ends -->
 </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
