<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/BlankMaster.master" CodeFile="EWSGraph.aspx.cs" Inherits="EMR_Templates_EWSGraph" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
<%--    <script src="https://code.jquery.com/jquery-1.11.3.js"></script>--%>
       <script type="text/javascript" src="../../Include/JS/jquery1.6.4.min.js"></script>
    <script src="../../Include/JS/jquery.mCustomScrollbar.concat.min.js"></script>
    <script src="../../Scripts/ChartLibrary/Chart.min.js"></script>
    <script src="../../Scripts/ChartLibrary/chartjs-plugin-datalabels.js"></script>
    <script src="../../Scripts/ChartLibrary/utils.js"></script>
    <script src="../../Scripts/ChartLibrary/DashboardGraph.js"></script>
    <script>
  

        window.onload = function () {         
            bindGraph();
        }
   
        function bindGraph() {

         
            removeCanvas();
            $.ajax({
                type: "POST",
                url: "/Shared/Services/PatientDashboardasmx.asmx/GetEWSGraphData",
                 data: "{ templateId:'" + <%= hdnTemplateId.Value%> + "'}",
                //data:JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var Data = result.d;
                    var ctx = $("#myChart").get(0).getContext("2d");
                    if (Data.length > 0) {
                        var Datasets = Data[0][0]
                        var Labels = Data[0][1];
                        var ChartTitle = Data[0][2][0];

                        $('#chartDiv').css("display", "");
                        $('#divMsg').css("display", "none");
                            
                        if (Datasets.length > 0) {
                            DrawChart('line', Labels, Datasets, ChartTitle, ctx);
                        }
                        else {
                            $('#divMsg').css("display", "");
                            $('#chartDiv').css("display", "none");

                        }
                    }
                    else {
                        $('#chartDiv').css("display", "none");
                        $('#divMsg').css("display", "");


                    }
                },
                failure: function (response) {
                    alert(response.d);

                }
            });
        }

       

        
        function removeCanvas() {
            try{
                var div = document.getElementById('chartDiv');
                while (div.firstChild)
                {
                    div.removeChild(div.firstChild);
                }
                var canvas = document.createElement('canvas');
                canvas.id = 'myChart';
                canvas.style.padding = 0;
                canvas.style.margin = 'auto';
                canvas.style.display = 'block';

                div.appendChild(canvas)
                
            }
            catch(error)
            {
                console.log(error.message);
            }
            
           


        }

    </script>
    <style>
        .rcSingle .riSingle {
            width: 100px !important;
        }
    </style>
    <div class="content-wrapper form-group">
        <div class="row" style="margin-left: 5px; margin-right: 5px">
            <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
        </div>
        <div class="row">
            <asp:HiddenField ID="hdnTemplateId" runat="server" />
            <div class="col-md-12 col-sm-12 col-xs-12" style="margin-top: 5px; margin-bottom: 10px; font-size: 14px">
           
                

            </div>
        </div>
        <div class="row text-center" id="divMsg">
            <h4>Graph data not found!</h4>
        </div>
        <div class="row">
            <div class="col-md-12">
               <div class="col-md-1"></div>
               <div class="col-md-10">
                    <div id="chartDiv" style="background: #fff;">

                    <canvas id="myChart" style="padding: 0; margin: auto; display: block;"></canvas>
                </div>
               </div>
               <div class="col-md-1"></div>
               

            </div>
        </div>

    </div>

 
    
    
</asp:Content>
