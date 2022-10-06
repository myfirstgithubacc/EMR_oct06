<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DoctorWiseImage.aspx.cs" Inherits="MPages_DoctorWiseImage" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function OnClientItemSelected(sender, args) {
            var canvas = document.getElementById("canvas");
            var context = canvas.getContext("2d");
            var img = document.createElement("img");
            img.src = args.get_path();
            img.onload = function () {
                extendResize(this.width, this.height);
                context.drawImage(img, 0, 0);
                var filename = img.src.substring(img.src.lastIndexOf('/') + 1);
                $get('<%=hdnFileName.ClientID%>').value = filename;
                }
            }
    </script>


    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>


    <div class="container-fluid header_main margin_bottom">
        <div class="col-md-4">
            <h2>
                <asp:Label ID="Label1" runat="server" Text="Doctor Wise Image Tagging" /></h2>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblMessage" runat="server" CssClass="relativ alert_new text-center text-success" />
        </div>
        <div class="col-md-4"></div>
    </div>





    <div class="container-fluid">
        <div class="row">



            <div class="col-md-6 form-group">
                <div class="col-md-4">
                    <asp:Literal ID="ltrlFacility" runat="server" Text="Facility"></asp:Literal>
                    <span style="color: Red;">*</span>
                </div>
                <div class="col-md-8">
                    <telerik:RadComboBox ID="ddlFacility" runat="server" Width="100%"
                        EmptyMessage="Select Facility" AutoPostBack="true" OnSelectedIndexChanged="ddlFacility_OnSelectedIndexChanged">
                    </telerik:RadComboBox>
                </div>
            </div>

            <div class="col-md-6 form-group">
                <div class="col-md-4">
                    <asp:Label ID="lblDepartment" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, department%>' />

                </div>
                <div class="col-lg-8">
                    <telerik:RadComboBox ID="ddlDepartment" runat="server" AllowCustomText="true" Filter="StartsWith"
                        Width="100%" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true">
                        <Items>
                            <telerik:RadComboBoxItem Text="All" Value="" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
            </div>

            <div class="col-md-6 form-group">
                <div class="col-md-4">
                    <asp:Label ID="lblProOrResour" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, Doctor%>' />
                    <%--<span style="color: Red">*</span>--%>
                </div>
                <div class="col-lg-8">
                    <telerik:RadComboBox ID="ddlProvider" runat="server" AppendDataBoundItems="true" Filter="Contains"
                        AutoPostBack="true" DataTextField="DoctorName" DataValueField="DoctorId" OnSelectedIndexChanged="ddlProvider_OnSelectedIndexChanged"
                        Width="100px" EmptyMessage="Select Provider">
                        <Items>
                            <telerik:RadComboBoxItem Text="" Value="0" />
                        </Items>
                    </telerik:RadComboBox>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-6 form-group">
                <div class="col-md-4">File Upload <span style="color: Red;">*</span></div>
                <div class="col-md-8">

                    <asp:FileUpload ID="_FileUpload" runat="server" Width="160px" CssClass="button inlin-bl" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnUpload_OnClick" />&nbsp;
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_OnClick" />
                    <asp:HiddenField ID="txt1" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnFileName" runat="server"></asp:HiddenField>
                </div>

            </div>
            <div class="col-md-2"></div>
            <div class="col-md-4">
            </div>
        </div>
    </div>


    <div class="container-fluid">
        <div class="row">
            <div class="col-md-8">
                <div style="overflow: scroll; height: 480px">
                    <asp:GridView ID="gvDoctorWiseImageTagging" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                        Width="100%" OnRowCommand="gvDoctorWiseImageTagging_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Doctor Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                   <%-- <asp:HiddenField ID="hdnImageId" runat="server" Value='<%#Eval("ImageId")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnFacilityID" runat="server" Value='<%#Eval("FacilityID")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>'></asp:HiddenField>

                                    <asp:HiddenField ID="hdnImageSize" runat="server" Value='<%#Eval("ImageSize")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnImageType" runat="server" Value='<%#Eval("ImageType")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActive" runat="server" Value='<%#Eval("Active")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnExistImage" runat="server" Value='<%#Eval("ExistImage")%>'></asp:HiddenField>
                                    <asp:Image ID="imgImage" runat="server" Visible="false" ImageUrl='<%#Eval("ImagePath")%>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Deparment Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("DepartmentName")%>'></asp:Label>
                                     <asp:HiddenField ID="hdnImageId" runat="server" Value='<%#Eval("ImageId")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnFacilityID" runat="server" Value='<%#Eval("FacilityID")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnImageSize" runat="server" Value='<%#Eval("ImageSize")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnImageType" runat="server" Value='<%#Eval("ImageType")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnActive" runat="server" Value='<%#Eval("Active")%>'></asp:HiddenField>
                                    <asp:HiddenField ID="hdnExistImage" runat="server" Value='<%#Eval("ExistImage")%>'></asp:HiddenField>
                                    <asp:Image ID="imgImage" runat="server" Visible="false" ImageUrl='<%#Eval("ImagePath")%>' />
                                     <asp:HiddenField ID="hdnDeparmentid" runat="server" Value='<%#Eval("Deparmentid")%>'></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Image Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblImageName" runat="server" Text='<%#Eval("ImageName")%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            <asp:TemplateField HeaderText="Image" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtnImage" runat="server" ImageUrl="~/Icons/ViewIcon.bmp" ToolTip="Click to View" CommandArgument='<%#Eval("ImagePath")%>' Width="30px" Height="30px" OnClick="imgBtnImage_OnClick" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnDelete" CssClass="chiefDelete" runat="server" CommandName="Del"
                                        ImageUrl="~/Images/DeleteRow.png" ToolTip="Delete" Width="16px" Height="16px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>
            <div class="col-md-4">


                <asp:Image ID="imgImage" runat="server" Height="500px" Width="100%" />

            </div>
        </div>
    </div>







    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

