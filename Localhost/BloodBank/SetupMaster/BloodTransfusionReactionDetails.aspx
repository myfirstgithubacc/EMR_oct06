<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BloodTransfusionReactionDetails.aspx.cs" Inherits="BloodBank_SetupMaster_BloodTransfusionReactionDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'/>
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    
    <style type="text/css">
        th {
            background: #2b77cd;
            color: white;
            font-family: sans-serif;
            font-weight: 400;
            font-size: 14px;
        }

        table#gvTransfusionReactionDetails {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            &nbsp;<asp:GridView ID="gvTransfusionReactionDetails" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True">
                <Columns>
                    <asp:TemplateField HeaderText="Registration No" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%# Eval("RegistrationNo") %>' Width="100%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Issue No" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblIssueNo" runat="server" Text='<%# Eval("IssueNo") %>' Width="100%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bag No." HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblBagNo" runat="server" Text='<%# Eval("BagNo") %>' Width="100%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Start Time" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblStartTime" runat="server" Text='<%# Eval("StartDatetime") %>' Width="100%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="End Time" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblEndTime" runat="server" Text='<%# Eval("EndDatetime") %>' Width="100%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Blood Group" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lblBloodGroup" runat="server" Text='<%# Eval("BloodGroup") %>' Width="100%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reaction" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblReaction" runat="server" Text='<%# Eval("Reaction") %>' Width="100%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reason" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblReason" runat="server" Text='<%# Eval("Reason") %>' Width="100%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
