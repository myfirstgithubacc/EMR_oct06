<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="Default3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="library/styles/speech-input-sdk.css" rel="stylesheet" media="all">

    <style type="text/css">
        .active {
            box-shadow: 0 2px 5px 0 rgba(0, 0, 0, .16), -8px 0px 0px 0 rgb(171 65 134);
        }

        .custom-textarea {
            width: 100%;
            height: 150px;
            border-radius: 10px;
        }

        .logo {
            width: 220px;
        }
    </style>

    <script type="text/jscript">
        speechInput​.​onLoad​(​"akhil123"​);
      
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>

    <textarea id="speech-output-text" class="p-2 custom-textarea" onfocus="onFocus(this)"></textarea>



</body>


<script>
    function myFunction(x) {
        x.style.background = "yellow";
    }
</script>
<script src="library/speech-input-sdk.min.js"></script>
<script src="library/app.js"></script>
</html>
