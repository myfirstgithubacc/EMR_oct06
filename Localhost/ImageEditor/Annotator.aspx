
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Annotator.aspx.cs" Inherits="Annotator" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Annotator</title>
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
     <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="painting.ico" />
    <link rel="stylesheet" type="text/css" href="annotator.css" />

    <script type="text/javascript" src="annotator.js"></script>


    <script type="text/javascript" src="jscolor.js"></script>

    <script type="text/javascript" src="jquery.min.js"></script>

    <script type="text/javascript" src="text.js"></script>

    <telerik:RadCodeBlock ID="codeBlock1" runat="server">

        <script type="text/javascript">
            function getRadWindow() //mandatory for the RadWindow dialogs functionality
            {
                if (window.radWindow) {
                    return window.radWindow;
                }
                if (window.frameElement && window.frameElement.radWindow) {
                    return window.frameElement.radWindow;
                }
                return null;
            }
        </script>

        <script type="text/javascript">


            function returnToParent() {
                //create the argument that will be returned to the parent page
                var oArg = new Object();
                oArg.xmlString = document.getElementById("hdnXmlString").value;
                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }
            function OnClientItemSelected(sender, args) {
                var canvas = document.getElementById("canvas");
                var context = canvas.getContext("2d");
                var img = document.createElement("img");
                img.src = args.get_path();
                img.onload = function() {
                    extendResize(this.width, this.height);
                    context.drawImage(img, 0, 0);
                    var filename = img.src.substring(img.src.lastIndexOf('/') + 1);
                    $get('<%=hdnFileName.ClientID%>').value = filename;
                    GetHiddenFieldValue('<%= txt1.ClientID %>');
                    $get('<%=btnInsert.ClientID%>').click();
                }
            }
            function OnClientPatientImageItemSelected(sender, args) {
                var canvas = document.getElementById("canvas");
                var context = canvas.getContext("2d");
                var img = document.createElement("img");
                img.src = args.get_path();
                img.onload = function() {
                    extendResize(this.width, this.height);
                    context.drawImage(img, 0, 0);
                    var filename = img.src.substring(img.src.lastIndexOf('/') + 1);
                    $get('<%=hdnPatientImage.ClientID%>').value = filename;

                    var filePath = img.src;
                    $get('<%=HiddenField1.ClientID%>').value = filePath;
                    
                     $get('<%=btnImageP.ClientID%>').click();
                    
                }
            }
           
        </script>

    </telerik:RadCodeBlock>
    <style type="text/css">
        .auto-style1 {
            left: 0px;
            top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="medill">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="Server">
            </telerik:RadScriptManager>
            <table align="right">
                <tr>
                    <td rowspan="2" style="vertical-align: top;">
                        <asp:Label ID="lblPatientImage" runat="server" Text="Patient Image" SkinID="label"></asp:Label>
                        <telerik:RadFileExplorer runat="server" ID="RadFileExplorer1" Width="250px" Height="500px"
                            OnClientItemSelected="OnClientPatientImageItemSelected" ExplorerMode="Default"
                            AllowPaging="True" PageSize="18" TreePaneWidth="122px" VisibleControls="Grid"
                            ToolTip="Select or Preview Image">
                            <Configuration />
                        </telerik:RadFileExplorer>
                    </td>
                    <td rowspan="2" style="vertical-align: top;">
                        <asp:Label ID="lblSpecializationImage" runat="server" Text="Image Library" SkinID="label"></asp:Label>
                        <telerik:RadFileExplorer runat="server" ID="FileExplorer1" Width="250px" Height="500px"
                            OnClientItemSelected="OnClientItemSelected" ExplorerMode="Default" AllowPaging="True"
                            PageSize="18" TreePaneWidth="122px" VisibleControls="Grid" ToolTip="Select or Preview Image">
                            <Configuration ViewPaths="~/medical_illustration" />
                        </telerik:RadFileExplorer>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <div id="toolbar">
                <ul id="buttons">
                    <!-- Icons from http://www.iconspedia.com and http://www.findicons.com -->
                    <li id="eraser" onmousedown="buttonDown(event, this)" onclick="selectTool(this)"
                        title="Eraser">
                        <img src="buttons/eraser.ico" alt="tool" />
                    </li>
                    <li id="pencil" onmousedown="buttonDown(event, this)" onclick="selectTool(this)"
                        title="Pencil">
                        <img src="buttons/pencil1.ico" alt="tool" />
                    </li>
                    <li id="text" title="Undo" onclick="undoLoad()">
                        <img src="buttons/undo.png" alt="tool" />
                    </li>
                    <li id="line" onmousedown="buttonDown(event, this)" onclick="selectTool(this)" title="Line">
                        <img src="buttons/line.jpg" alt="tool" />
                    </li>
                    <li id="rectangle" onmousedown="buttonDown(event, this)" onclick="selectTool(this)"
                        title="Rectangle">
                        <img src="buttons/rectangle.jpg" alt="tool" />
                    </li>
                    <li id="circle" onmousedown="buttonDown(event, this)" onclick="selectTool(this)"
                        title="Circle">
                        <img src="buttons/circle.jpg" alt="tool" />
                    </li>
                </ul>
                <div id="color" title="Select Color">
                    <input class="color" type="text" size="6" maxlength="6" onclick="selectColor(this, event)"
                        style="width: 100%; font-size: 14px;" /><img src="buttons/picker.jpg" width="30px"
                            height="30px" alt="tool" /><div>
                                <b style="font-size:12px;">Select Color</b>
                            </div>
                </div>
                <div id="settings" onmousedown="event.preventDefault()">
                    <div id="line-settings" class="line" title="Select Stroke Size ">
                        <div onclick="selectSetting(this, 'context.lineWidth=1')" class="sel">
                            <div style="height: 1px; margin-top: 3px">
                            </div>
                        </div>
                        <div onclick="selectSetting(this, 'context.lineWidth=2')">
                            <div style="height: 2px; margin-top: 3px">
                            </div>
                        </div>
                        <div onclick="selectSetting(this, 'context.lineWidth=3')">
                            <div style="height: 3px; margin-top: 2px">
                            </div>
                        </div>
                        <div onclick="selectSetting(this, 'context.lineWidth=4')">
                            <div style="height: 4px; margin-top: 2px">
                            </div>
                        </div>
                        <div onclick="selectSetting(this, 'context.lineWidth=5')">
                            <div style="height: 5px; margin-top: 1px">
                            </div>
                        </div>
                        <div onclick="selectSetting(this, 'context.lineWidth=8')">
                            <div style="height: 6px; margin-top: 1px">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Text">
                    <img src="buttons/text.jpg" width="17px" height="17px" style="float:left;"/>
                    <b title="Select Size of Text" style="font-size:11px;float:left;">Size:</b>
                    <div>
                        <select id="textsize" style="font-size:12px; float:right; width:38px;" title="Select Size of Text">
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="13">13</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="17">17</option>
                            <option value="18">18</option>
                            <option value="19">19</option>
                            <option value="20">20</option>
                        </select>
                    </div>
                    <input id="txt" runat="server"
                        type="text" style="width: 100%; font-size: 14px; margin-top:5px;" value="Enable Doc" title="Enter Text to paste on Canvas"/>
                </div>
                <div id="controls">
                    <input id="B1" type="button" class="btn btn-primary" style="width: 100%; font-size: 14px;" value="New"
                        align="middle" onclick="context.clearRect(0, 0, canvas.width, canvas.height);contextrev.clearRect(0, 0, canvas.width, canvas.height)"
                        title="Clears the Contents of the Canvas" />
                    <input id="btnCaptureImage" type="button"  class="btn btn-primary" value="Capture" style="width: 100%; font-size: 14px;"
                        value="New/Clear" align="middle" onclick="GetHiddenFieldValue('<%= txt1.ClientID %>')"
                        title="Convert to Base64 string before saving the Image" />

             

                    <asp:UpdatePanel ID="up1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnImageP" runat="server" CssClass="btn btn-primary" Width="90px" Text="ImageP" OnClick="btnImageP_Click" />

                        </ContentTemplate>

                    </asp:UpdatePanel>




                    <asp:HiddenField ID="txt1" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnFileName" runat="server"></asp:HiddenField>
                    <asp:Button ID="Button1" Style="width: 100%; font-size: 14px;" align="middle" runat="server"
                        Text="Save" OnClick="savebtn_Click"  CssClass="btn btn-primary" ToolTip="Save the Annotated Image" Visible="false" />
                    <asp:Button ID="btnInsert" Style="width: 100%; font-size: 14px;" align="middle" runat="server"
                        Text="Insert" OnClick="btnInsert_Click" CssClass="btn btn-primary" ToolTip="Insert the Annotated Image" />
                    <asp:Button ID="btnDelete" Style="width: 100%; font-size: 14px;" align="middle" runat="server"
                        Text="Delete" OnClick="btnDelete_Click" CssClass="btn btn-primary" ToolTip="Delete the Annotated Image" />
                    <asp:Button ID="btnClose" Style="width: 100%; font-size: 14px;" align="middle" runat="server"
                        Text="Close" OnClientClick="window.close()" CssClass="btn btn-primary" ToolTip="Close Editor" />
                    <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                    <asp:HiddenField ID="hdnPatientImage" runat="server" Value="" />
                    <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                </div>
            </div>
            <div id="drawpanel" class="pencil" style="width: 54%;">
                <canvas id="canvas" runat="Server" width="500px" height="420px" oncontextmenu="return false" class="auto-style1">
            </canvas>
                <canvas id="canvastrans" width="500px" height="420px" oncontextmenu="return false">
            </canvas>
                <div id="canvasresize" onmousedown="canvasResize(event)">
                </div>
            </div>
            <div id="resize" onmousedown="resizeWindow(event, this)">
            </div>
            <div id="saveimage">
            </div>
            <div id="saveimage_bkg">
            </div>
            <canvas id="canvasrev" width="500px" height="420px">
        </canvas>
        </div>
    </form>
</body>
</html>
