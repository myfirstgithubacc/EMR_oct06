<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintDischargeSummary.aspx.cs" Inherits="ICM_PrintDischargeSummary" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.IO" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Discharge Summary</title>
    <script src="https://code.jquery.com/jquery-1.12.4.min.js" type="text/jscript" integrity="sha256-ZosEbRLbNQzLpnKIkEdrPv7lOy9C27hHQ+Xp8a4MxAQ=" crossorigin="anonymous"></script>
</head>
<body>
    <%--<button class="printMe">Print</button>--%>


    <form id="form1" runat="server">
    <%--   <%-- //yogesh--%>
<%--        <%foreach (DataRow drSet in dset.Tables[0].Rows)
            { %>

        <table>
            <thead>
                <tr>
                    <th>
                        <div>
                            <%=System.Web.HttpContext.Current.Server.HtmlDecode( common.myStr(drSet["HeaderHTML"]))%>
                        </div>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <div>
                            <%=drSet["PatientSummary"]%>
                        </div>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr style="vertical-align: bottom;">
                    <td style="height: 120px; overflow: hidden;">
                        <div>
                            <%=System.Web.HttpContext.Current.Server.HtmlDecode( common.myStr(drSet["FooterHTML"]))%>
                        </div>
                    </td>
                </tr>
            </tfoot>
        </table>

        <%
            }
        %>--%>

      <%--  <div>
            <asp:HiddenField ID="hdnDoctorImage" runat="server" />
            <asp:HiddenField ID="hdnFacilityImage" runat="server" />
            <asp:HiddenField ID="hdnFontName" runat="server" />
        </div>

        <script type="text/javascript">
            document.body.innerHTML = $('#form1')[0].innerHTML;
            window.print();
            $('.printMe').click(function () {
                document.body.innerHTML = $('#form1')[0].innerHTML;
                window.print();
                // $(".from").print();
                setTimeout(function () { window.close(); }, 10000);
            });

            //window.onafterprint = function () {
            //    location.reload();
            //}
        </script>--%>--%>


         <div>
            <asp:HiddenField ID="hdnDoctorImage" runat="server" />
            <asp:HiddenField ID="hdnFacilityImage" runat="server" />
            <asp:HiddenField ID="hdnFontName" runat="server" />
            <asp:HiddenField ID="hdnFontSize" runat="server" />
        </div>
    </form>
</body>
</html>
