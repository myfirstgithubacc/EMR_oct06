<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DiagnosisDetails.aspx.cs" Inherits="EMR_Assessment_DiagnosisDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="_ScriptManager" runat="server">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
           <div class="container-fluid">
               <div class="row header_main">
                   <div class="col-md-3 col-sm-3 col-xs-4">
                  <h2>ICD Help, Version 10</h2>
                 </div>
                   <div class="col-md-9 col-sm-9 col-xs-8 text-right">
                      <asp:Button ID="btnAddToFavourite" runat="server" Text="Add To Favorite" OnClick="btnAddToFavourite_Click" CssClass="btn btn-primary" />
                      <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="window.close();"
                                                                            CssClass="btn btn-primary" />
                 </div>
               </div>
                 
                 <div class="row text-center"> <asp:Label ID="lblMessage" runat="server" /></div>
                 

                <div class="row">
            <div class="col-md-4 col-sm-4 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-2 col-sm-2 col-xs-3"> <asp:Label ID="ltrlFacility" runat="server" Text="Group" > </asp:Label></div>
                    <div class="col-md-10 col-sm-10 col-xs-9">
                        <telerik:RadComboBox ID="ddlGroup" runat="server" Width="100%" OnSelectedIndexChanged="ddlGroup_OnSelectedIndexChanged"
                                AutoPostBack="true">
                            </telerik:RadComboBox></div>
                </div>

            </div>
             <div class="col-md-4 col-sm-4 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-3 col-sm-3 col-xs-3"><asp:Label ID="Literal1" runat="server" Text="Sub Group"> </asp:Label>
</div>
                    <div class="col-md-9 col-sm-9 col-xs-9">
                        <telerik:RadComboBox ID="ddlSubGroup" runat="server" Width="100%" OnSelectedIndexChanged="ddlSubGroup_OnSelectedIndexChanged"
                                AutoPostBack="true">
                            </telerik:RadComboBox></div>
                </div>

            </div>
         
    </div>
        <div class="row">
            <div class="col-md-4 col-sm-4 col-xs-12 gridview m-t">
                <asp:Label ID="lblDiseaseId" runat="server" Text="Disease" Font-Bold="true"> </asp:Label>
                <asp:GridView ID="gvDisease" runat="server" AutoGenerateColumns="False" Width="100%"
                                DataKeyNames="DiseaseId" ForeColor="#333333" GridLines="None" PagerSettings-Visible="true"
                                OnSelectedIndexChanged="gvDisease_OnSelectedIndexChanged" OnRowDataBound="gvDisease_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiseaseId" runat="server" Text='<%# Eval("DiseaseId")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubGroupId" runat="server" Text='<%# Eval("SubGroupId")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Disease Code" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiseaseCode" runat="server" Text='<%# Eval("DiseaseCode")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Disease Name" HeaderStyle-Width="400px" ItemStyle-Width="400px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiseaseName" runat="server" Text='<%# Eval("DiseaseName")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowSelectButton="True" />
                                </Columns>
                            </asp:GridView>
            </div>
            <div class="col-md-8 col-sm-8 col-xs-12 gridview m-t">
                <asp:Label ID="Label1" runat="server" Text="Sub Disease" Font-Bold="true"> </asp:Label>
                <asp:GridView  ID="gvSubDisease" runat="server" AutoGenerateColumns="False"
                                 ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gvSubDisease_OnSelectedIndexChanged"
                                  OnRowDataBound="gvSubDisease_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubGroupId" runat="server" Text='<%# Eval("SubGroupId")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiseaseId" runat="server" Text='<%# Eval("DiseaseId")%>'> </asp:Label>
                                             <asp:Label ID="lblICDID" runat="server" Text='<%# Eval("ICDID")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ICDCode" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblICDCode" runat="server" Text='<%# Eval("ICDCode")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-Width="400px" ItemStyle-Width="400px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Valid For Clinical Use" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValidForClinicalUse" runat="server" Text='<%# Eval("ValidForClinicalUse")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valid For Primary Diagnosis" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblValidForPrimaryDiagnosis" runat="server" Text='<%# Eval("ValidForPrimaryDiagnosis")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Age Range" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgeRange" runat="server" Text='<%# Eval("AgeRange")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Gender" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGender" runat="server" Text='<%# Eval("Gender")%>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowSelectButton="True" />
                                </Columns>
                            </asp:GridView>
                                    <asp:HiddenField ID="hdnICDId" runat="server" />

            </div>
        </div>

</div>

                   
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
