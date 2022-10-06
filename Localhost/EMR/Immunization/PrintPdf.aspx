<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintPdf.aspx.cs" Inherits="EMR_Immunization_PrintPdf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Immunization</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:HiddenField ID="hdnDoctorImage" runat="server" />
        <asp:HiddenField ID="hdnFacilityImage" runat="server" />
          <asp:HiddenField ID="hdnFontName" runat="server" />
         <asp:HiddenField ID="hdndob" runat="server" />
    </div>
    </form>
</body>
</html>
