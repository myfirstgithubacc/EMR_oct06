<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopPanelPatientDashBoard.ascx.cs" Inherits="Temp_TopPanelPatientDashBoard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style>
.info-box {
    background: #fff none repeat scroll 0 0;
    border-radius: 2px;
    box-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);
    display: block;
    margin-bottom: 0px;
    height:45px; overflow:hidden;
    width: 100%;
}
.bg-aqua{
    background-color: #00c0ef !important;
}
.info-box-content {
    margin-left:5px;
    padding: 5px 10px;
}
.info-box-icon {
    background: rgba(0, 0, 0, 0.2) none repeat scroll 0 0;
    border-radius: 2px 0 0 2px;
    display: block;
    float: left;
    font-size:20px;
    height: 40px;
    line-height: 40px;
    text-align: center;
    width: 40px;
}
.info-box-text {
    text-transform: uppercase;
}
.progress-description, .info-box-text {
    display: inline;
    font-size: 12px;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}
.info-box-number {
    display: inline;
    font-size: 13px;
    font-weight: bold;
}
.info-box-icon .fa{ color:#fff; line-height:40px;}
.bg-green{
    background-color: #00a65a !important;
}
.bg-yellow{
    background-color: #f39c12 !important;
}
</style>
	    <!-- Patient Part Start -->
        <div class="patientDiv">
    	    <div class="container-fluid">
                <div class="row">	
        
    			    <div class="patientDiv-Photo hide">
    			        <asp:UpdatePanel ID="UpdatePanel400" UpdateMode="Conditional"  runat="server">
                            <ContentTemplate><asp:ImageButton ID="PatientImage" runat="server" ImageUrl="~/imagesHTML/camera.ico" border="0" BorderWidth="0" BorderColor="Gray" onclick="PatientImage_Click"/></ContentTemplate>
                        </asp:UpdatePanel>
    			    </div>
                    



                      <div class="container-fluid">
        <div class="col-md-12"><h3 style="margin-top: 7px;">Patient dashboard</h3></div>
    </div>



                    <div class="container-fluid">
                    
                        
                        
                        
                        <div class="col-md-2 ">
                          <div class="info-box">
                            <%--<span class="info-box-icon blueme"><i class="fa fa-search"></i></span>--%>

                                 


                            <div class="info-box-content"  data-priority="1"  data-columns="tech-companies-1-col-1">
                              <span class="info-box-text"><asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration, UHID %>' />:</span>
                              <span class="info-box-number"><asp:Label ID="lblCId" runat="server" /></span>
                            </div>
                            <!-- /.info-box-content -->
                          </div>
                          <!-- /.info-box -->
                        </div>

                        <div class="col-md-2 ">
                          <div class="info-box">
                            <%--<span class="info-box-icon redme"><i class="fa fa-user"></i></span>--%>
                             
                            <div class="info-box-content"  data-priority="3"  data-columns="tech-companies-1-col-2">
                              <span class="info-box-text">Name: </span>
                              <span class="info-box-number"><asp:Label ID="lblPatientName" runat="server" /></span>
                            </div>
                            <!-- /.info-box-content -->
                          </div>
                          <!-- /.info-box -->
                        </div>

                        <div class="col-md-2 ">
                          <div class="info-box">
                            <%--<span class="info-box-icon greenme"><i class="fa fa-info-circle"></i></span>--%>

                            <div class="info-box-content" data-priority="1"  data-columns="tech-companies-1-col-3">
                              <span class="info-box-text">Age/Gender: </span>
                              <span class="info-box-number"><asp:Label ID="lblAge" runat="server" /><asp:Label ID="lblGender" runat="server" />
                                        <asp:Label ID="lblDob" Visible="false" runat="server" /></span>
                            </div>
                            <!-- /.info-box-content -->
                          </div>
                          <!-- /.info-box -->
                        </div>

                        <div class="col-md-2 ">
                          <div class="info-box">
                           <%-- <span class="info-box-icon yellowme"><i class="fa  fa-pencil"></i></span>--%>

                            <div class="info-box-content" data-priority="3" data-columns="tech-companies-1-col-5">
                              <span class="info-box-text"><asp:Label ID="lblEncNo" runat="server" /></span>
                              <span class="info-box-number"><asp:Label ID="lblEncNo_Resources" runat="server" Text='<%$ Resources:PRegistration, EncounterNo%>' Visible="false"></asp:Label></span>
                            </div>
                            <!-- /.info-box-content -->
                          </div>
                          <!-- /.info-box -->
                        </div>

                        <div class="col-md-2 ">
                          <div class="info-box">
                           <%-- <span class="info-box-icon orangeme"><i class="fa fa-universal-access"></i></span>--%>

                            <div class="info-box-content"  data-priority="6"  data-columns="tech-companies-1-col-6">
                              <span class="info-box-text">Provider: </span>
                              <span class="info-box-number"><asp:Label ID="lblVtCrPrvdr" runat="server" /></span>
                            </div>
                            <!-- /.info-box-content -->
                          </div>
                          <!-- /.info-box -->
                        </div>

                        <div class="col-md-2 ">
                          <div class="info-box">
                          <%--  <span class="info-box-icon dgreenme"><i class="fa  fa-calendar"></i></span>--%>

                            <div class="info-box-content" data-priority="6"  data-columns="tech-companies-1-col-6">
                              <span class="info-box-text">Date: </span>
                              <span class="info-box-number"><asp:Label ID="lblEncDate" runat="server" />
                                        <asp:Label ID="lblCrntEnSts"  Visible="false" runat="server" />
                                        <asp:Label ID="lblAcCategory" Visible="false" runat="server" SkinID="label" Text="" />
                                        <asp:Label ID="lblAcType" runat="server" Visible="false" SkinID="label" Text="" />
                                        <asp:Label ID="lblVisitType" runat="server" Visible="false" />
                                        <asp:Label ID="lblPackageVisit" runat="server" Visible="false" />
                                        <asp:Label ID="lblLoc" runat="server" Visible="false"/>
                                        <asp:Label ID="lblAddress" runat="server" Visible="false"/>
                                        <asp:Label ID="lblRefPrvdr" Visible="false" runat="server" />
                                        <asp:Label ID="lblApptNote" runat="server" Visible="false" /></span>
                            </div>
                            <!-- /.info-box-content -->
                          </div>
                          <!-- /.info-box -->
                        </div>

                        
                        

                                    
                                  

                                    <span data-priority="6"  data-columns="tech-companies-1-col-6" style="display:none;"></span>
                             





                                <span style="display:none;">
                                    <span data-priority="1" data-columns="tech-companies-1-col-1">Bed: <asp:Label ID="lblBedNo" runat="server" /></span>
                                    <span data-priority="3" data-columns="tech-companies-1-col-2">Ward: <asp:Label ID="lblWard" runat="server" /> </span>
                                    <span data-priority="1" data-columns="tech-companies-1-col-3">Mobile: &nbsp;<asp:Label ID="lblMphone" runat="server" /></span>
                                    <span data-priority="3" data-columns="tech-companies-1-col-5">Company: <asp:Label ID="lblPayer" runat="server" /><asp:Label ID="lblPlnType" runat="server" SkinID="label" Text="" /></span>
                                    <span data-priority="6" data-columns="tech-companies-1-col-6"></span>
                                    <span data-priority="6" data-columns="tech-companies-1-col-6"></span>
                                </span> 
                	    
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
          
            