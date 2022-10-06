<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    Theme="DefaultControls" CodeFile="LinkDoctorWithVitalTemplate.aspx.cs" Inherits="EMR_Vitals_LinkDoctorWithVitalTemplate"
    Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">


        function openRadWindow(ID) {
            var oWnd = radopen("ViewDoctorVitalTemplate.aspx?TempID=" + ID, "Radwindow1");
            oWnd.Center();
        }

    </script>

    <script language="javascript" type="text/javascript" src="/Include/JS/Common.js">
    </script>
    <link href="../../Include/css/bootstrap.css" type="text/css" rel="Stylesheet" />
    <link href="../../Include/css/emr_new.css" type="text/css" rel="Stylesheet" />


        <div class="container-fluid header_main">
                 <div class="col-md-3">
                  <h2> Link Provider - Vital Templates</h2>
                 </div>
				 <asp:UpdatePanel ID="updSave" runat="server">
                     <ContentTemplate>
                 <div class="col-md-5 text-center"> <asp:Label ID="lblMessage" runat="server" Text="" /> </div>
                 
                 <div class="col-md-3 text-right pull-right"> 
                    
                        <asp:Button ID="ibtnLinkDoctorVitalTemplates" SkinID="Button" runat="server" OnClick="ibtnLinkDoctorVitalTemplates_OnClick"
                            Text="Save" />
                    </ContentTemplate>
                </asp:UpdatePanel> </div>
        </div>



    <div class="container-fluid text-right">
       <div class="">  <asp:HyperLink ID="HyperLink1" Style="text-decoration: none;" NavigateUrl="/EMR/Vitals/VitalMaster.aspx"
                    runat="server">Vital Signs</asp:HyperLink>
                |
                <asp:HyperLink ID="HyperLink2" Style="text-decoration: none;" NavigateUrl="/EMR/Vitals/VitalSignTemplate.aspx"
                    runat="server">Vital Templates</asp:HyperLink>
                |
                <asp:HyperLink ID="HyperLink3" Style="text-decoration: none;" NavigateUrl="/EMR/Vitals/LinkDoctorWithVitalTemplate.aspx"
                    runat="server">Provider - Vital Templates </asp:HyperLink>
               </div>
    </div>



    



   
    <asp:UpdatePanel ID="updLinkDoctorWithVitalTemplate" runat="server">
        <ContentTemplate>
           <div class="container-fluid">
        <div class="row">
            <div class="col-md-6">
                <asp:Literal ID="ltrlDoctor" runat="server" Text="<strong>Provider</strong>"></asp:Literal><br />
                        
                        <asp:GridView ID="gvDoctors" SkinID="gridview" CellPadding="2" runat="server" AutoGenerateColumns="false"
                            ShowHeader="true" Width="100%" PageSize="25" AllowPaging="true" OnRowDataBound="gvDoctors_OnRowDataBound"
                            OnPageIndexChanging="gvDoctors_OnPageIndexChanging" PagerSettings-Visible="true"
                            PageIndex="0" PagerSettings-Mode="NumericFirstLast">
                            <Columns>
                                <asp:BoundField DataField="DoctorID" />
                                <asp:BoundField DataField="DoctorName" HeaderText="Provider" />
                                <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ItemStyle-Width="70px"
                                    ControlStyle-Font-Underline="true" SelectText="Select" CausesValidation="false"
                                    ShowSelectButton="true">
                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lblMSG" runat="server" Font-Bold="true" ForeColor="Red" Visible="false"></asp:Label>

            </div>


            <div class="col-md-6">
                <asp:Literal ID="ltrlVitalTemplates" runat="server" Text="<strong>Vital Templates</strong>"></asp:Literal><br />
                        
                        <asp:GridView ID="gvVitalTemplates" CellPadding="2" SkinID="gridview" runat="server"
                            AutoGenerateColumns="false" ShowHeader="true" Width="100%" OnSelectedIndexChanged="gvVitalTemplates_OnSelectedIndexChanged"
                            OnRowDataBound="gvVitalTemplates_OnRowDataBound" PageSize="25" AllowPaging="true"
                            OnPageIndexChanging="gvVitalTemplates_OnPageIndexChanging" PagerSettings-Visible="true"
                            PageIndex="0" PagerSettings-Mode="NumericFirstLast">
                            <Columns>
                                <asp:BoundField DataField="ID" />
                                <asp:BoundField DataField="TemplateName" HeaderText="Vital&nbsp;Template" />
                                
                                <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ItemStyle-Width="70px"
                                    SelectText="Select" CausesValidation="false" ShowSelectButton="true">
                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                </asp:CommandField>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="47px" ItemStyle-Width="47px">
                                    <ItemTemplate>
                                        <a href="#" onclick="openRadWindow('<%# DataBinder.Eval(Container.DataItem, "ID") %>'); return false;">
                                            Details</a>
                                        <%-- <label style="color: Blue; text-decoration: underline; cursor: hand" onclick="javascript:window.open('/EMR/Vitals/ViewDoctorVitalTemplate.aspx?TempID=<%#Eval("ID")%>','mywindow', 'menubar=0,resizable=0,width=550,height=250,status=0,toolbars=0');">
                                  Details  </label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

            </div>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="550" Height="400"
        VisibleStatusbar="false" Top="40" Left="200" Behaviors="Close,Move" 
        ReloadOnShow="true">
    </telerik:RadWindowManager>
</asp:Content>
