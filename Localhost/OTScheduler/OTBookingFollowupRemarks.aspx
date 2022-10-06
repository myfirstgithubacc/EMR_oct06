<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OTBookingFollowupRemarks.aspx.cs" Inherits="OTScheduler_OTBookingFollowupRemarks" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Followup Remarks</title>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function setMaxLength(control) {
            //get the isMaxLength attribute
            var mLength = control.getAttribute ? parseInt(control.getAttribute("isMaxLength")) : ""

            //was the attribute found and the length is more than the max then trim it
            if (control.getAttribute && control.value.length > mLength) {
                control.value = control.value.substring(0, mLength);
                alert('Length exceeded');
            }

            //display the remaining characters
            var modid = control.getAttribute("id") + "_remain";
            if (document.getElementById(modid) != null) {
                document.getElementById(modid).innerHTML = mLength - control.value.length + " Remains";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-md-6 col-xs-6">
                <h2 style="color: #333;">
                    <asp:Label ID="Label3" runat="server" Text="Followup Remarks"></asp:Label></h2>
            </div>
            <div class="col-md-6 col-xs-6 text-right">
                <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-default" Text="Close" ToolTip="Close" OnClientClick="window.close();" />

            </div>
        </div>
        <div class="container-fluid">

            <table class="table table-small-font table-bordered table-striped margin_bottom01">
                <tr align="center">
                    <td colspan="1" align="left">
                        <asp:Label ID="lblOTBN" runat="server" SkinID="label" Text="Booking No" Font-Bold="true" />
                        <asp:Label ID="lblOtBookingNo" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true" />
                    </td>
                    <td colspan="1" align="left">
                        <asp:Label ID="lblOtbDt" runat="server" SkinID="label" Font-Bold="true" Text="Date:" />
                        <asp:Label ID="lblBookingDate" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true" />
                    </td>
                    <td colspan="1" align="left">
                        <asp:Label ID="lblReg" runat="server" SkinID="label" Font-Bold="true" Text='<%$ Resources:PRegistration, Regno%>' />
                        <asp:Label ID="lblregNo" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true" />
                    </td>
                </tr>
            </table>

        </div>
        <div class="container-fluid">
            <div class="row form-group">
                <div class="col-md-12 col-xs-12 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row form-group">
                <div class="col-md-12 col-xs-12 ">
                    <asp:Label ID="lblPreviousRemarks" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row form-group">
                <div class="col-md-9">
                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" onKeyUp="setMaxLength(this)" isMaxLength="100" runat="server" Text=""></asp:TextBox>
                    <span id="<%=txtRemarks.ClientID %>_remain"><%= 100 - txtRemarks.Text.Length %> Remains</span>
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" CssClass="btn btn-primary" ValidationGroup="save" Text="Save" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
