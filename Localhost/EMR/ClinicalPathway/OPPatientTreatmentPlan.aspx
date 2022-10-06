<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="OPPatientTreatmentPlan.aspx.cs" Inherits="EMR_ClinicalPathway_OPPatientTreatmentPlan" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


        <script type="text/javascript">
        window.onload = function () {
            var div = document.getElementById("dvScroll");
            var div_position = document.getElementById("div_position");
            var position = parseInt('<%=Request.Form["div_position"] %>');
           
            if (isNaN(position)) {
                position = 0;
            }
            div.scrollTop = position;
            div.onscroll = function () {
                div_position.value = div.scrollTop;
            };
        };
    </script>

    <script type="text/javascript">
        
        function OnClientEditorLoad(editor, args) {
            var style = editor.get_contentArea().style;
            style.fontFamily = 'Tahoma';
            style.fontSize = 11 + 'pt';
        }

       

        var ilimit = 40;
        function AutoChange(txtRemarks) {
            var txt = document.getElementById(txtRemarks);
            if (txt.value.length >= 10) {
                if (txt.value.length >= 40 * txt.rows) {
                    txt.rows = txt.rows + 1;
                    ilimit = 0;
                }
                else if (txt.value.length < 40 * (txt.rows - 1)) {
                    txt.rows = Math.round(txt.value.length / 40) + 1;
                }
                else if (txt.value.length >= 500) {
                    txt.value.length = txt.value.substring(0, 500)
                    return false;
                }
                else {
                    if (txt.value.length <= ilimit * txt.rows && txt.rows >= ilimit) {
                        txt.cols = (txt.cols * 1) + 1;
                    }
                }
            }
            return true;
        }

    </script>
    <script type="text/javascript">
            function OnClientSelectionChange(editor, args) {
                var tool = editor.getToolByName("RealFontSize");
                if (tool && !$telerik.isIE) {
                    setTimeout(function () {
                        var value = tool.get_value();

                        switch (value) {
                            case "11px":
                                value = value.replace("11px", "9pt");
                                break;
                            case "12px":
                                value = value.replace("12px", "9pt");
                                break;
                            case "14px":
                                value = value.replace("14px", "11pt");
                                break;
                            case "16px":
                                value = value.replace("16px", "12pt");
                                break;
                            case "15px":
                                value = value.replace("15px", "11pt");
                                break;
                            case "18px":
                                value = value.replace("18px", "14pt");
                                break;
                            case "24px":
                                value = value.replace("24px", "18pt");
                                break;
                            case "26px":
                                value = value.replace("26px", "20pt");
                                break;
                            case "32px":
                                value = value.replace("32px", "24pt");
                                break;
                            case "34px":
                                value = value.replace("34px", "26pt");
                                break;
                            case "48px":
                                value = value.replace("48px", "36pt");
                                break;
                        }
                        tool.set_value(value);
                    }, 0);
                }
            }
        </script>

    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
  
    <style type="text/css">
        .left-form-panel .form-group label {
            display: block;
        }

        .left-form-panel .form-group .RadComboBox.RadComboBox_Default {
            width: 100% !important;
        }

            .left-form-panel .form-group .RadComboBox.RadComboBox_Default .rcbInput {
                width: 100% !important;
                float: none !important;
            }

        .checkbox-panel .form-group label {
            display: flex; font-size: 12px; padding-top: 5px;
        }

        .panel-heading b {
            font-size: 15px;
            font-weight: 500;
        }


        .MPSpacingDiv03 h2 {
            color: #82d0ef;
            font-weight: 400;
            font-size: 16px;
        }

        .EMRTreatmentTop #ctl00_ContentPlaceHolder1_ddlProvider_Input {
            width: 100%;
        }

        .table-block-custom .Checkboxes td {
    width: 33.333333% !important;
    display: inline-block !important;
    margin: 0;
    padding: 0;
   
}



        /* Dont write in Stylesheet */
        div#RAD_SPLITTER_PANE_CONTENT_ctl00_RadPane2 {
            overflow: hidden !important;
        }
        /* Dont write in Stylesheet */


        .success {
            -webkit-animation: seconds 1.0s forwards;
            -webkit-animation-iteration-count: 1;
            -webkit-animation-delay: 3s;
            animation: seconds 1.0s forwards;
            animation-iteration-count: 1;
            animation-delay: 3s;
            background: #fff;
            border-radius: 5px;
            box-shadow: 0 0 5px #ccc;
            color: #1ca52d !important;
            margin: auto !important;
            left: 0;
            line-height: 40px;
            text-transform: capitalize;
            right: 0;
            position: absolute;
            display: block;
            width: 40vw;
            text-align: center;
        }

        @-webkit-keyframes seconds {
            0% {
                opacity: 1;
            }

            100% {
                opacity: 0;
                left: -9999px;
                position: absolute;
            }
        }

        @keyframes seconds {
            0% {
                opacity: 1;
            }

            100% {
                opacity: 0;
                left: -9999px;
                position: absolute;
            }
        }

        div#ctl00_ContentPlaceHolder1_viewpart .col-lg-6:nth-child(even) {
            clear: both;
        }

        .li-page .day-col tbody {
            display: block !important;
        }

        .li-page .day-col tr {
            float: left;
            margin-bottom: 5px;
        }

        .gv-selected-service tr.clsGridRow { background: none;}

        .checkbox-inline {
    margin-top: 10px;
    
}

        .checkbox-inline label {
    font-weight: normal;
    font-size: 13px;
}
.form-scroller .panel.panel-default { margin-bottom: 5px;}
/*.form-scroller .panel-body { min-height: 110px; max-height: 110px; overflow-y: auto;}*/
table#ctl00_ContentPlaceHolder1_gvSelectedServices table { width: 100% !important;}
/*table#ctl00_ContentPlaceHolder1_gvSelectedServices table td { width: 42% !important;}*/
/*.panel-body label { display: inline-block; font-size: 12px; width: 30%; }
.panel-body .RadComboBox.RadComboBox_Default { width: 55% !important;}*/
.panel-default>.panel-heading {
    padding: 2px 10px;
}
/*.form-scroller .col-md-6:nth-child(even) { clear: both;}*/
.check-panel { margin-bottom: 10px;}
.check-panel input[type="checkbox"] {
    float: left;
    margin-top: 8px;
    margin-right: 4px;
}
.check-panel label { font-weight: normal;}
.check-panel td { display: inline-block; width: 33%;}
input#ctl00_ContentPlaceHolder1_ddlPlanTemplates_Input { padding: 2px 10px;}
.check-panel td:first-child {
    width: 33%;
}

.check-panel td:nth-child(2) {
    width: 33%;
}
.panel.panel-default .RadComboBox.RadComboBox_Default { max-width: 100% !important;}
.form-scroller .panel-body { padding-bottom: 0;}
.ajax__tab_container { padding: 10px;}

.ajax__tab_outer { background: none !important; height: auto !important;}
.ajax__tab_tab { background: none !important; height: auto !important;}
.ajax__tab_inner { background: none !important; height: auto !important;}
.ajax__tab_tab {
    height: auto !important;
    vertical-align: top;
    padding: 5px !important;
   
}
.ajax__tab_active { border: 2px solid #337ab7; background: #337ab7; color: #fff; border-top-left-radius: 4px; border-top-right-radius: 4px;  display: inline-block; text-align: center; font-weight: bold;}
.ajax__tab_body { display: inline-block; width: 100%; padding: 0 !important;
    border: 0 !important;}
.head-greybg td { background: #f5f5f5; padding: 2px 10px;}

.gv-selected-service .head-greybg td { padding-left: 0;}
    </style>

    <style>
        /*table#ctl00_ContentPlaceHolder1_GridViewdays, table#ctl00_ContentPlaceHolder1_GridViewdays td {
            border: none;
        }

            table#ctl00_ContentPlaceHolder1_GridViewdays td {
                display: block;
                padding-top: 5px;
            }

                table#ctl00_ContentPlaceHolder1_GridViewdays td a {
                    display: block;
                }*/

        panel .table tr td:first-child {
            width: 5% !important;
        }

        #myCarousel {
            margin-top: 30px;
        }

        .carousel-inner {
            min-height: 450px;
        }

        .carousel-caption {
            top: 0;
        }

        .li-page {
            /*position: absolute;
            margin-top: -22px;
            right: 0;
            float: right;*/
            margin: 10px auto;
            display: table;
            text-align: center;
        }

            .li-page .day-col tr {
                margin-right: 2px;
            }

            .li-page table, .li-page table td {
                border: 0;
            }

        .panel.panel-default .RadComboBox.RadComboBox_Default {
            max-width: 220px;
        }

        .panel-body GridViewDignosis {
            max-height: 200px;
            overflow-y: auto;
        }

        div#ctl00_ContentPlaceHolder1_ddlGeneric table { position: relative;}

        div#ctl00_ContentPlaceHolder1_ddlGeneric td.rcbArrowCell.rcbArrowCellRight { position: absolute; right: 0; /*background-position: -158px -88px;*/}


                
    </style>

    <style>
        table#ctl00_ContentPlaceHolder1_GridViewDayView tbody {
            display: flex;
        }

        .table-auto {
            /*height: 232px;*/
            overflow-y: auto;
        }

        /*.left-form-panel table#ctl00_ContentPlaceHolder1_GridViewdays tr {
            border: 1px solid #f5f5f5;
            display: inline-block;
            width: 50%;
        }

        .left-form-panel table#ctl00_ContentPlaceHolder1_GridViewdays {
            display: inline-block;
            max-height: 265px;
            overflow-y: auto;
        }*/

        ::-webkit-scrollbar {
            width: 6px;
            height: 6px;
        }

        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px #91acf8;
        }

        ::-webkit-scrollbar-thumb {
            background: #91acf8;
        }

            ::-webkit-scrollbar-thumb:hover {
                background: #91acf8;
            }


            .form-group.col-md-6 label span:first-child { float: left; margin-right: 5px;}
            span#ctl00_ContentPlaceHolder1_Label14 { line-height: 10px;}
            div#ctl00_ContentPlaceHolder1_ddlGeneric td.rcbArrowCell.rcbArrowCellRight { position: absolute; right: 0; /*background-position: -158px -88px;*/}
        .form-group span[style="color:Red;"] { line-height: 0.7em;}
        .day-lbl span:first-child { float: left;}
        .form-group label { line-height: 25px;}
    </style>

    <style>
        /*body {
  font-family: Arial, Helvetica, sans-serif;
  font-size: 20px;
}*/

        #myBtn {
            display: none;
            position: fixed;
            bottom: 20px;
            right: 30px;
            z-index: 99;
            font-size: 18px;
            border: none;
            outline: none;
            background-color: red;
            color: white;
            cursor: pointer;
            padding: 15px;
            border-radius: 4px;
        }

            #myBtn:hover {
                background-color: #555;
            }
             tr.rwTitleRow { display: none !important;}

            div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow1 {
    height: 98vh !important; width: 80vw !important; margin: auto; right: 0; top: 0; bottom: 0;
}

            table#ctl00_ContentPlaceHolder1_GridViewdays a { font-size: 13px; font-weight: bold;}
            span.badge { position: absolute; margin-top: -8px;}
            .day-lbl { float: left;
    margin-top: 5px;
    margin-right: 10px;
    font-weight: bold;
    margin-bottom: 10px;}

             .select-all input[type="checkbox"] {
            margin-right: 5px;
        }

            .row.bg-info b {font-size: 15px; display: inline-flex; padding-top: 4px;}
.top-error {color: green;  position: absolute; left: 0; right: 0; text-align: center; margin-top: 10px; font-size: 14px;}

    </style>


    <input type="hidden" id="div_position" name="div_position" />

    <div id="dvScroll"  style="overflow-y: scroll; height: 600px; width: 99%">
    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>
            <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <asplUD:UserDetails ID="asplUD" runat="server" />
                      
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="row header_main">
                        <div class="col-md-4 col-sm-4 col-xs-4"><b>Treatment Plan</b></div>
                        <div class="col-md-8 col-sm-8 col-xs-8 text-right">
                             <asp:LinkButton ID="btnSave" runat="server" Text="Submit" CssClass="btn btn-primary"
                                CausesValidation="false" OnClick="btnSave_Click" />
                        </div>
                </div>

                <div class="row text-center">
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="top-error" />
                </div>
                <div class="row bg-gray">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-3">
                                <asp:Label ID="lblDept" runat="server" Text="Plan Name" /><asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-9">
                                <telerik:RadComboBox ID="ddlPlanTemplates" runat="server" class="form-control" MarkFirstMatch="true" Filter="Contains" EnableLoadOnDemand="true"
                                                    EmptyMessage="[Select Care Plan]" EnableVirtualScrolling="true" OnSelectedIndexChanged="ddlPlanTemplates_SelectedIndexChanged"
                                                     AutoPostBack="true" Width="100%"  />
                            </div>
                        </div>
                    </div>
                    </div>

                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="border-radius:5px;border:1px solid #ccc;overflow:hidden;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-gray">
                                <b>Chief Complaints</b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                                <asp:TextBox ID="txtChiefComplaints" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px" 
                                        Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="border-radius:5px;border:1px solid #ccc;overflow:hidden;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-gray">
                                <b>History</b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                                <asp:TextBox ID="txtHistory" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px" 
                                        Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="border-radius:5px;border:1px solid #ccc;overflow:hidden;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-gray">
                                <b>Examination</b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                                <asp:TextBox ID="txtExamination" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px" 
                                        Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="border-radius:5px;border:1px solid #ccc;overflow:hidden;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-gray">
                                <b>Plan Of Care</b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                                <asp:TextBox ID="txtPlanOfCare" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px" 
                                        Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="border-radius:5px;border:1px solid #ccc;overflow:hidden;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-gray">
                                <b>Instructions</b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                                <asp:TextBox ID="txtFreeInstruction" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px" 
                                        Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                    
                </div>
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="border-radius:5px;border:1px solid #ccc;overflow:hidden;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-gray">
                                <b>Investigations</b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 gridview">
                                <asp:GridView ID="gvService" runat="server"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" OnRowDataBound="gvService_RowDataBound"
                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                            AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-bordered">

                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px">
                                                     <HeaderTemplate>
                                                         <asp:CheckBox ID="chkAllService" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllService_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkService" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Service Name" ItemStyle-Font-Size="Small">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                         <asp:HiddenField ID="hdnServiceId" runat="server" Value='<% #Eval("ServiceId") %>' /> 
                                                        <asp:HiddenField ID="hdnId" runat="server" Value='<% #Eval("Id") %>' />
                                                           <asp:HiddenField ID="hdnPlanId" runat="server" Value='<% #Eval("PlanId") %>' />
                                                         <asp:HiddenField ID="hdnDayId" runat="server" Value='<% #Eval("DayId") %>' />
                                                         <asp:HiddenField ID="hdnDayDetailId" runat="server" Value='<% #Eval("DayDetailId") %>' />
                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                </div>

                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="border-radius:5px;border:1px solid #ccc;overflow:hidden;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-gray">
                                <b>Medicines</b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview p-t-b-5">
                                <asp:GridView ID="gvDrugClass" runat="server" SkinID="gridviewOrderNew"  
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" OnRowDataBound="gvDrugClass_RowDataBound"
                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                            AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px">
                                                       <HeaderTemplate>
                                                         <asp:CheckBox ID="chkAllDrugClass" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllDrugClass_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDrugClass" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>'  HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Drug Class Name" ItemStyle-Font-Size="Small">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDrugClassName" runat="server" Text='<%#Eval("DrugClassName")%>' />

                                                         <asp:HiddenField ID="hdnDrugClassId" runat="server" Value='<% #Eval("DrugClassId") %>' />
                                                        <asp:HiddenField ID="hdnId"  runat="server" Value='<%# Eval("Id") %>' />
                                                           <asp:HiddenField ID="hdnPlanId" runat="server" Value='<% #Eval("PlanId") %>' />
                                                         <asp:HiddenField ID="hdnDayId" runat="server" Value='<% #Eval("DayId") %>' />
                                                         <asp:HiddenField ID="hdnDayDetailId" runat="server" Value='<% #Eval("DayDetailId") %>' />
                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        </asp:GridView>
                            </div>

                             <div class="col-md-12 text-center" style="border-bottom: 1px solid #ccc;  margin-bottom: 20px;"><span class="badge">OR</span></div>
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview p-t-b-5">
                                <asp:GridView ID="gvPrescription" runat="server" SkinID="gridviewOrderNew" 
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" OnRowDataBound="gvPrescription_RowDataBound"
                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                            AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px">
                                                    <HeaderTemplate>
                                                         <asp:CheckBox ID="chkAllPrescription" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllPrescription_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkPrescription" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>'  HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" >
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Generic Name" ItemStyle-Font-Size="Small" ItemStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGenericName" runat="server" Text='<%#Eval("GenericName")%>' />
                                                         <asp:HiddenField ID="hdnId"  runat="server"  Value='<%# Eval("Id") %>' />
                                                        <asp:HiddenField ID="hdnItemId"  runat="server"  Value='<%# Eval("ItemId") %>' />
                                                        <asp:HiddenField ID="hdnPlanId" runat="server" Value='<% #Eval("PlanId") %>' />
                                                         <asp:HiddenField ID="hdnDayId" runat="server" Value='<% #Eval("DayId") %>' />
                                                         <asp:HiddenField ID="hdnDayDetailId" runat="server" Value='<% #Eval("DayDetailId") %>' />
                                                        <asp:HiddenField ID="hdnRouteId" runat="server" Value='<% #Eval("RouteId") %>' /> 
                                                         <asp:HiddenField ID="hdnDoseUnitID" runat="server" Value='<% #Eval("DoseUnitID") %>' />
                                                       <asp:HiddenField ID="hdnGenericId" runat="server" Value='<% #Eval("GenericId") %>' /> 
                                                          <asp:HiddenField ID="hdnQty" runat="server" Value='<% #Eval("Qty") %>' />
                                                         <asp:HiddenField ID="hdnFrequencyID" runat="server" Value='<% #Eval("FrequencyID") %>' />
                                                           <asp:HiddenField ID="hdnFoodRelationId" runat="server" Value='<% #Eval("FoodRelationId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Brand Name" ItemStyle-Font-Size="Small" ItemStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dose" ItemStyle-Font-Size="Small" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDose" runat="server" Text='<%#Eval("Dose")%>' />
                                                        <asp:Label ID="lblDoseUnit" runat="server" Text='<%#Eval("DoseUnit")%>' />
                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Frequency" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFrequency" runat="server" Text='<%#Eval("Frequency")%>' />
                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Duration" ItemStyle-Font-Size="Small" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDays" runat="server" Text='<%#Eval("Days")%>' />
                                                        <asp:Label ID="lblDaysType" runat="server" Text='<%#Eval("DaysType")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Route" ItemStyle-Font-Size="Small" ItemStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRouteName" runat="server" Text='<%#Eval("RouteName")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Food Relation" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFoodName" runat="server" Text='<%#Eval("FoodName")%>' />
                                                     
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Instructions" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIntructions" runat="server" Text='<%#Eval("Intructions")%>' />

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                    
                    
                </div>
                <div class="row">
                    <div class="col-md-3 col-sm-3 col-xs-12 p-t-b-5 box-col-checkbox">
                        <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview p-t-b-5">
                        <asp:GridView ID="gvSelectedServices" SkinID="gridview" runat="server" HeaderStyle-Wrap="false"
                                                AutoGenerateColumns="False" ShowHeader="false" CellSpacing="0" CellPadding="0"
                                                Width="100%" AllowPaging="false" PagerSettings-Visible="true" OnRowDataBound="gvSelectedServices_RowDataBound" CssClass="gv-selected-service">
                                                <EmptyDataTemplate>
                                                    No Data Found.
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Values" ItemStyle-Width="100%" HeaderStyle-Width="100%"
                                                        HeaderStyle-Font-Size="10pt" ItemStyle-VerticalAlign="Top">
                                                        <ItemTemplate>
                                                           <table id="tblFieldName" cellpadding="0" cellspacing="1" border="0" runat="server" width="100%" class="head-greybg" >
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName")%>' Font-Bold="true"  ></asp:Label>
                                                                        <asp:HiddenField  ID="hdnFieldType"  runat="server" Value='<%#Eval("FieldType")%>' />
                                                                         <asp:HiddenField  ID="hdnSectionId"  runat="server" Value='<%#Eval("SectionId")%>' /> 
                                                                          <asp:HiddenField  ID="hdnFieldId"  runat="server" Value='<%#Eval("FieldID")%>' />   
                                                                        <asp:HiddenField  ID="hdnColumnNosToDisplay"  runat="server" Value='<%#Eval("ColumnNosToDisplay")%>' />   
                                                                    </td>
                                                                </tr>
                                                               </table>
                                                            <table id="tbl1" cellpadding="0" cellspacing="1" border="0" runat="server">
                                                                <tr valign="top">
                                                                    <td>
                                                                        <asp:TextBox ID="txtT" SkinID="textbox" Columns='<%#common.myInt(Eval("MaxLength"))%>'
                                                                            Visible="false" MaxLength='<%#common.myInt(Eval("MaxLength"))%>' runat="server"
                                                                            Width="100%" />
                                                                    </td>

                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="txtM" SkinID="textbox" runat="server" TextMode="MultiLine" Style="min-height: 50px; min-width: 250px; width: auto !important;"
                                                                            MaxLength="5000" onkeyup="return MaxLenTxt(this,5000);"
                                                                            Visible="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlTemplateFieldFormats" Font-Size="10pt" runat="server" OnSelectedIndexChanged="ddlTemplateFieldFormats_OnSelectedIndexChanged"
                                                                            SkinID="DropDown" Width="200px" AutoPostBack="true" Visible="false" />
                                                                        <telerik:RadEditor ID="txtW" ToolbarMode="ShowOnFocus" OnClientSelectionChange="OnClientSelectionChange"
                                                                            EnableResize="true" runat="server" Skin="Outlook"
                                                                            ToolsFile="~/Include/XML/PrescriptionRTF.xml" EditModes="Design" OnClientLoad="OnClientEditorLoad" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table id="Table1" cellpadding="0" cellspacing="1" border="0" runat="server">
                                                                <tr valign="top">
                                                                    <td>
                                                                        <asp:DropDownList ID="D" SkinID="DropDown"
                                                                            Visible="false" runat="server" Width="227px" Font-Size="10pt" AppendDataBoundItems="true">
                                                                            <asp:ListItem Text="Select" Value="0" />
                                                                        </asp:DropDownList>
                                                                        <telerik:RadComboBox ID="IM" Visible="false" runat="server" Width="227px" Font-Size="10pt"
                                                                            AppendDataBoundItems="true" Skin="Default" EnableAutomaticLoadOnDemand="True"
                                                                            EnableVirtualScrolling="true">
                                                                            <Items>
                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                            </Items>
                                                                        </telerik:RadComboBox>

                                                                        <asp:DataList ID="C" runat="server" Visible="false" CellPadding="10" CellSpacing="10" CssClass="check-panel">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnCV" runat="server" Value='<%#Eval("ValueId")%>' />
                                                                                <asp:CheckBox ID="C" Font-Size="10pt" runat="server" Text='<%#Eval("ValueName")%>' Font-Bold="false" />
                                                                                <textarea id="CT" class="Textbox" visible="false" runat="server" onkeypress="AutoChange()"
                                                                                    rows="1" cols="40"></textarea>
                                                                            </ItemTemplate>
                                                                        </asp:DataList>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                            <asp:RadioButtonList ID="B" Font-Size="10pt" Width="100px" runat="server" Visible="false" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="1" Text="Yes" />
                                                                <asp:ListItem Value="0" Text="No" />
                                                            </asp:RadioButtonList>
                                                            <asp:RadioButtonList ID="R" Font-Size="10pt" Width="100%" CssClass="FormatRadioButtonList"
                                                                runat="server" Visible="false" RepeatDirection="Horizontal" RepeatLayout="Flow" />
                                                            <table id="tblDate" runat="server" visible="false" cellpadding="0" cellspacing="0">
                                                                <tr >
                                                                    <td>
                                                                        <asp:TextBox ID="txtDate" SkinID="textbox" Font-Size="13px" Text="" Width="67px"
                                                                            runat="server" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                            vspace="0" border="0" id="imgFromDate" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDate"
                                                                            Format="dd/MM/yyyy" PopupButtonID="imgFromDate">
                                                                        </cc1:CalendarExtender>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                            TargetControlID="txtDate" FilterType="Custom, Numbers" ValidChars="_/">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                                                            CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                            Enabled="True" TargetControlID="txtDate" MessageValidatorTip="false" AcceptAMPM="true"
                                                                            AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                            ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                        </cc1:MaskedEditExtender>
                                                                        <asp:CustomValidator ID="CustomValidator" runat="server" ClientValidationFunction="isValidateDate"
                                                                            ControlToValidate="txtDate" ErrorMessage="Invalid date format." />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                        </div>
                </div>

                </div>
                      

        </ContentTemplate>
    </asp:UpdatePanel>
    
    </div>

</asp:Content>

