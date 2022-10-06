<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="SetDefaultMaster.aspx.cs" Inherits="MPages_SetDefaultMaster" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    
    


    <div class="container-fluid header_main form-group">
        <div class="col-md-2 col-sm-3"><h2>Set Defaults Master</h2></div>
        <div class="col-md-8 col-sm-7 text-center">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-2 col-sm-2 text-right">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSave" ToolTip="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="save" Text="Save" OnClick="btnSave_OnClick" />
                    <%--<asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" SkinID="Button" />  
                    <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" SkinID="Button" />     
                    <asp:Button ID="btnUpdate" ToolTip="Update" runat="server" SkinID="Button" 
                        ValidationGroup="save" Text="Update" OnClick="btnUpdate_OnClick" />--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-4 col-sm-5">
                        
                        <div class="row form-groupTop01">
                            <div class="col-md-12 col-sm-12">
                                <div class="container-fluid header_main"><div class="col-md-12 col-sm-12"><h2>Default Settings for Masters</h2></div></div>
                            </div>
                        </div>


                        <div class="border-LeftRight">

                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12"><asp:Literal ID="Litral1" runat="server" Text="Default Country"></asp:Literal></div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="ddlcountryname" AutoPostBack="true" runat="server" OnSelectedIndexChanged="LocalCountry_SelectedIndexChanged"
                                            Width="100%" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">Default State</div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="dropLState"  Width="100%" runat="server"
                                            OnSelectedIndexChanged="LocalState_SelectedIndexChanged" AutoPostBack="true"
                                            AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">Default City</div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="dropLCity" runat="server" Width="100%" AppendDataBoundItems="true"
                                            AutoPostBack="true" OnSelectedIndexChanged="LocalCity_OnSelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">Default Zip</div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="ddlZip" runat="server" Width="100%" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12"><asp:Literal ID="Literal2" runat="server" Text="Default Religion"></asp:Literal></div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="ddlDefaultReligion" runat="server" Width="100%"
                                            AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <%--<div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12"><asp:Literal ID="Literal1" runat="server" Text="Default Company"></asp:Literal></div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="ddlcompany" runat="server" SkinID="DropDown" Width="200px"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>--%>

                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6 PaddingRightSpacing">
                                    <div class="col-md-12 col-sm-12"><asp:Literal ID="Literal6" runat="server" Text="Default Nationality"></asp:Literal></div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="ddlDefaultNationality" runat="server" 
                                            AppendDataBoundItems="true" Width="100%">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row form-groupTop02 form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 PaddingRightSpacing">Default Language</div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="ddlDefaultLanguage" runat="server" 
                                            AppendDataBoundItems="true" Width="100%">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                        </div>
                        
                        
                        
                        
                        <div class="row form-groupTop01">
                            <div class="col-md-12 col-sm-12">
                                <div class="container-fluid header_main"><div class="col-md-12 col-sm-12"><h2>Default Settings for Word Processor</h2></div></div>
                            </div>
                        </div>

                        <div class="border-LeftRight">
                            
                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 PaddingRightSpacing"><asp:Literal ID="Literal3" runat="server" Text="Default Font Type"></asp:Literal></div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="ddlfonttype" runat="server" Width="100%">
                                            <%--<asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                           <asp:ListItem Text="Arial" Value="1" ></asp:ListItem>
                                            <asp:ListItem Text="Arial Black" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Arial Narrow" Value="3" ></asp:ListItem>
                                            <asp:ListItem Text="Comic Sans MS" Value="4" ></asp:ListItem>
                                           <asp:ListItem Text="Courier New" Value="5" ></asp:ListItem>
                                           <asp:ListItem Text="System" Value="6" ></asp:ListItem>
                                            <asp:ListItem Text="Tahoma" Value="7" ></asp:ListItem>
                                            <asp:ListItem Text="Times" Value="8" ></asp:ListItem>
                                            <asp:ListItem Text="Verdana" Value="9" ></asp:ListItem>
                                            <asp:ListItem Text="Wingdings" Value="10" ></asp:ListItem>--%>
                                         </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        
                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12"><asp:Literal ID="Literal4" runat="server" Text="Default Font Size"></asp:Literal></div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12"><asp:DropDownList ID="ddlfontsize" runat="server" Width="100%"></asp:DropDownList></div>
                                </div>
                            </div>

                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12 PaddingRightSpacing"><asp:Literal ID="Literal5" runat="server" Text="Default Page Size"></asp:Literal></div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:DropDownList ID="ddlpagesize" runat="server" Width="100%">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="11*17" Value="11*17"></asp:ListItem>
                                            <asp:ListItem Text="A0" Value="A0"></asp:ListItem>
                                            <asp:ListItem Text="A1" Value="A1"></asp:ListItem>
                                            <asp:ListItem Text="A10" Value="A10"></asp:ListItem>
                                            <asp:ListItem Text="A2" Value="A2"></asp:ListItem>
                                            <asp:ListItem Text="A3" Value="A3"></asp:ListItem>
                                            <asp:ListItem Text="A4" Value="A4"></asp:ListItem>
                                            <asp:ListItem Text="A5" Value="A5"></asp:ListItem>
                                            <asp:ListItem Text="A6" Value="A6"></asp:ListItem>
                                            <asp:ListItem Text="A7" Value="A7"></asp:ListItem>
                                            <asp:ListItem Text="A8" Value="A8"></asp:ListItem>
                                            <asp:ListItem Text="A9" Value="A9"></asp:ListItem>
                                            <asp:ListItem Text="ARCH_A" Value="ARCH_A"></asp:ListItem>
                                            <asp:ListItem Text="ARCH_B" Value="ARCH_B"></asp:ListItem>
                                            <asp:ListItem Text="ARCH_C" Value="ARCH_C"></asp:ListItem>
                                            <asp:ListItem Text="ARCH_D" Value="ARCH_D"></asp:ListItem>
                                            <asp:ListItem Text="ARCH_E" Value="ARCH_E"></asp:ListItem>
                                            <asp:ListItem Text="B0" Value="B0"></asp:ListItem>
                                            <asp:ListItem Text="B1" Value="B1"></asp:ListItem>
                                            <asp:ListItem Text="B10" Value="B10"></asp:ListItem>
                                            <asp:ListItem Text="B2" Value="B2"></asp:ListItem>
                                            <asp:ListItem Text="B3" Value="B3"></asp:ListItem>
                                            <asp:ListItem Text="B4" Value="B4"></asp:ListItem>
                                            <asp:ListItem Text="B5" Value="B5"></asp:ListItem>
                                            <asp:ListItem Text="B6" Value="B6"></asp:ListItem>
                                            <asp:ListItem Text="B7" Value="B7"></asp:ListItem>
                                            <asp:ListItem Text="B8" Value="B8"></asp:ListItem>
                                            <asp:ListItem Text="B9" Value="B9"></asp:ListItem>
                                            <asp:ListItem Text="CROWN_OCTAVO" Value="CROWN_OCTAVO"></asp:ListItem>
                                            <asp:ListItem Text="CROWN_QUARTO" Value="CROWN_QUARTO"></asp:ListItem>
                                            <asp:ListItem Text="DEMY_OCTAVO" Value="DEMY_OCTAVO"></asp:ListItem>
                                            <asp:ListItem Text="DEMY_QUARTO" Value="DEMY_QUARTO"></asp:ListItem>
                                            <asp:ListItem Text="EXECUTIVE" Value="EXECUTIVE"></asp:ListItem>
                                            <asp:ListItem Text="FLSA" Value="FLSA"></asp:ListItem>
                                            <asp:ListItem Text="FLSE" Value="FLSE"></asp:ListItem>
                                            <asp:ListItem Text="HALFLETTER" Value="HALFLETTER"></asp:ListItem>
                                            <asp:ListItem Text="ID_1" Value="ID_1"></asp:ListItem>
                                            <asp:ListItem Text="ID_2" Value="ID_2"></asp:ListItem>
                                            <asp:ListItem Text="ID_3" Value="ID_3"></asp:ListItem>
                                            <asp:ListItem Text="LARGE_CROWN_OCTAVO" Value="LARGE_CROWN_OCTAVO"></asp:ListItem>
                                            <asp:ListItem Text="LARGE_CROWN_OCTAVO" Value="LARGE_CROWN_OCTAVO"></asp:ListItem>
                                            <asp:ListItem Text="LEDGER" Value="LEDGER"></asp:ListItem>
                                            <asp:ListItem Text="LEGAL" Value="LEGAL"></asp:ListItem>
                                            <asp:ListItem Text="LETTER" Value="LETTER"></asp:ListItem>
                                            <asp:ListItem Text="NOTE" Value="NOTE"></asp:ListItem>
                                            <asp:ListItem Text="PENGUIN_LARGE_PAPERBACK" Value="PENGUIN_LARGE_PAPERBACK"></asp:ListItem>
                                            <asp:ListItem Text="PENGUIN_SMALL_PAPERBACK" Value="PENGUIN_SMALL_PAPERBACK"></asp:ListItem>
                                            <asp:ListItem Text="POSTCARD" Value="POSTCARD"></asp:ListItem>
                                            <asp:ListItem Text="ROYAL_OCTAVO" Value="ROYAL_OCTAVO"></asp:ListItem>
                                            <asp:ListItem Text="ROYAL_QUARTO" Value="ROYAL_QUARTO"></asp:ListItem>
                                            <asp:ListItem Text="SMALL_PAPERBACK" Value="SMALL_PAPERBACK"></asp:ListItem>
                                            <asp:ListItem Text="TABLOID" Value="TABLOID"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        
                            <div class="row form-groupTop01">
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12"></div>
                                </div>
                                <div class="col-md-6 col-sm-6">
                                    <div class="col-md-12 col-sm-12"></div>
                                </div>
                            </div>
                        </div>
                        
                    </div>


                    <div class="col-md-offset-2 col-sm-offset-2 col-sm-3 col-md-3">
                        <asp:GridView ID="gv" runat="server" SkinID="gridviewOrderNew" AutoGenerateColumns="false" Width="100%"
                            OnRowDataBound="gv_RowDataBound" OnRowCommand="gv_RowCommand" HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" 
                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" 
                            HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                            <Columns>
                                <asp:BoundField DataField="Id" />
                                <asp:BoundField DataField="HospitalLocationId" />
                                <asp:BoundField DataField="Fieldkey" HeaderText="Description" />
                                <asp:BoundField DataField="FieldDescription" HeaderText="Value" />
                                <asp:BoundField DataField="FieldValue" HeaderText="Field Value" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete1"
                                            CommandArgument='<%#Eval("FieldKey") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
            <asp:AsyncPostBackTrigger ControlID="ddlcountryname" />
            <asp:AsyncPostBackTrigger ControlID="dropLState" />
            <asp:AsyncPostBackTrigger ControlID="dropLCity" />
            <asp:AsyncPostBackTrigger ControlID="ddlZip" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
