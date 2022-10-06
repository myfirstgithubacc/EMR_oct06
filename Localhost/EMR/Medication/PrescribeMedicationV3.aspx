<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PrescribeMedicationV3.aspx.cs" Inherits="EMR_Medication_PrescribeMedicationV3" %>

<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/mainNew.css" type="text/css" rel="Stylesheet" />
    <link href="../../Include/css/bootstrap.css" type="text/css" rel="Stylesheet" />
    <style>
        body {
            background: #f4f4f4;
            font-family: 'Open Sans', sans-serif !important;
        }

        p {
            font-family: 'Open Sans', sans-serif !important;
        }

        .mystyle {
            background-color: #777;
            border-radius: 10px;
            font-weight: bold;
        }

            /* :hover is a pseudo selector to use to set the mouseover attributes */
            .mystyle:hover {
                background-color: Red;
                border-radius: 10px;
                font-weight: bold;
            }

        /* commented by devendra
            #ctl00_ContentPlaceHolder1_tbDiagnosis_body {
            max-height: 420px;
            overflow-x: hidden;
            overflow-y: auto;
        }*/

        #ctl00_ContentPlaceHolder1_gvItem {
            background: #f4f4f4;
            font-family: 'Open Sans', sans-serif !important;
        }

        /*#ctl00_ContentPlaceHolder1_tbDiagnosis{
            width:100%;
            visibility: visible;
        }*/
        /*below code commented by Devendra */
        /*#ctl00_ContentPlaceHolder1_tbDiagnosis_tabFavorite {
            height: auto;
            overflow-x: hidden;
            overflow-y: auto;
            max-height: 150px;
        }*/

        .fixi {
            position: fixed;
            right: 100px;
        }

        /* commented by devendra
            #ctl00_ContentPlaceHolder1_tbDiagnosis {
            width: 1275px;
            overflow-x: hidden;
            overflow-y: auto;
            visibility: visible;
        }*/

        .label1 {
            display: inline;
            padding: .2em .6em .3em;
            font-size: 75%;
            font-weight: bold;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: 10px;
        }

        .ajax__tab_outer {
            padding-right: 7px;
            height: 25px;
        }

        .ajax__tab_default .ajax__tab_inner {
            display: -moz-inline-box;
            display: inline-block;
            height: 25px;
        }

        #ctl00_ContentPlaceHolder1_tbDiagnosis_body {
            border: 0px;
        }

        /* 09-May-2019 */


        .header_main ~ .container-fluid p, .header_main ~ .container-fluid td span {
            font-size: 12px !important;
            color: #666666;
            font-family: 'Open Sans', sans-serif !important;
        }

        .header_main ~ .container-fluid .form-group input[type='text'] {
            padding: 20px !important;
        }

        .header_main ~ .container-fluid .form-group td.rcbInputCell {
            background: none !important;
        }

        .header_main ~ .container-fluid .form-group input[type='text'] {
            padding: 5px 10px !important;
            border: 1px solid #ccc !important;
        }

        td.rcbArrowCell.rcbArrowCellRight {
            background: none !important;
        }

        .header_main ~ .container-fluid .form-group input[type='text'] {
            padding: 5px 10px !important;
            border: 1px solid red;
        }

            .header_main ~ .container-fluid .form-group input[type='text'].rcbInput {
                background: url(../../Images/DownArrow.png) no-repeat 95% center;
                background-size: 10px;
            }

        #ctl00_ContentPlaceHolder1_gvItem {
            border: 1px solid #ccc;
            margin-top: 2px;
        }

        .clsGridheader th {
            padding: 6px;
            border: none;
        }

        .clsGridRow td {
            padding: 6px;
            border: none;
        }

        .header_main ~ .container-fluid .clsGridRow td span {
            font-size: 11px !important;
        }

        .border-line {
            border-right: 1px dotted #ccc;
            padding-right: 20px;
        }

        input#ctl00_ContentPlaceHolder1_ddlStore_Input {
            background-color: #fff;
        }

        .check-all {
            float: left;
            width: 12%;
        }

        div#ctl00_ContentPlaceHolder1_UpdatePanel4 {
            float: left;
            width: 88%;
        }

        .RadPane2 td.rcbArrowCell.rcbArrowCellRight {
            display: none;
        }

        input#ctl00_ContentPlaceHolder1_ddlStore_Input {
            padding-right: 25px !important;
        }

        /* 09-May-2019 */

        .dt_btn2 input[type='submit'].btn_cus2 {
            padding: 10px 7px;
            font-size: 14px;
        }

        .btn_hand {
            width: 17px;
            height: 17px;
            color: #000;
            font-weight: bold;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0px 0px 0px 3px;
            cursor: pointer;
        }

        .prescription-section input.rcbInput {
    background: url(../../Images/down.png) no-repeat #fff !important;
    background-position: 96% center !important;
}
    </style>

    <!-- Bellow  css references added for new design by Devendra-->
    <link href="../../UI_2019_IGI/style.css" rel="stylesheet" />

    <script type="text/javascript" src="../../Include/JS/StringBuilder.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=rdoSearchMedication.ClientID%> input').change(function () {
                console.log($(this).val());
            });


        });

        function bindFavourite() {
            // alert("1");

            //$('#tbDiagnosis').empty();
            //tbDiagnosis_tabFavorite
            //$('[name=favouriteItem]').html("");
            //$("span").html("");

            var imgCntrl = document.getElementById('<%= imgMedicationPopup.ClientID %>');
            var txtFavouriteItemName = $get('<%=txtFavouriteItemName.ClientID%>').value
            imgCntrl.style.display = 'block';

            var data = new Object();
            data.itemName = '';
            $.ajax({
                type: "POST",
                url: '/Shared/Services/PrescriptionService.asmx/GetFavoriteItems',
                //data: "{itemName:''}",
                data: "{itemName:'" + txtFavouriteItemName + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var jsonData = JSON.parse(res.d);
                    var sb = new StringBuilder();
                    $('#dvFavorite').empty();
                    for (var i = 0; i < jsonData.length; i++) {
                        //  debugger;
                        if (i < 500) {

                            // sb.append("<div id='" + jsonData[i].ItemId + "' style='border-radius: 10px;display: block;float: left;margin-right: 10px;'><span id='favouriteItemId' tabindex='0'  name='favouriteItem' class='label label-default' style='background-color: #f2f2f2; color: #000000;border-top-left-radius: 3px !important;border-bottom-left-radius: 3px !important;display:inline-block;border-bottom-right-radius: 0px;border-top-right-radius: 0px;cursor: pointer; padding:6px 10px; margin-bottom:5px;' onclick='setFavorateItem(" + JSON.stringify(jsonData[i]) + "," + i + ",this)'>" + jsonData[i].ItemName + " </span><span class='label label-default'  style='cursor: pointer;background-color: #f2f2f2; color: #000000;border-top-left-radius: 0px;border-bottom-right-radius: 3px;border-bottom-right-radius: 3px;border-bottom-left-radius: 0px;padding: 5.0px 9px;' onclick='removeFavouriteItem(" + jsonData[i].ItemId + "," + i + ",this)'>x</span></div>");
                            sb.append("<div class='pre_btn' id='" + jsonData[i].FavoriteId + "'><span id='favouriteItemId' name='favouriteItem' class='btn-cus' onclick='setFavorateItem(" + JSON.stringify(jsonData[i]) + "," + i + ",this)'>" + jsonData[i].ItemName + " </span>");

                            if ('<%= common.myBool(Session["IsCIMSInterfaceActive"]) %>' == 'True') {
                                if (jsonData[i].CIMSItemId.toString().length > 10)
                                    sb.append("<span class='btn_hand' onclick='showMonograph(" + JSON.stringify(jsonData[i]) + ")'><img src='../../CIMSDatabase/monograph.png' alt='Monograph' width='16px' height='16px'></img></span>");

                                //sb.append("<span class='btn_hand' onclick='showMonograph(" + JSON.stringify(jsonData[i]) + ")'><img src='../../CIMSDatabase/interaction.png' alt='Interaction' width='16px' height='16px'></img></span>");
                            }

                            sb.append("<span class='btn_close' onclick='removeFavouriteItem(" + jsonData[i].FavoriteId + "," + jsonData[i].ItemId + "," + jsonData[i].GenericId + "," + i + ",this)'>&times;</span></div>");
                        }
                    }
                    if (jsonData.length >= 500)
                        imgCntrl.style.display = '';

                    $('#dvFavorite').append(sb.toString());
                    console.log();
                },
                error: function (res) {
                    console.log(res);
                }
            })
        }
        function bindOrderSet() {

            var imgCntrl = document.getElementById('<%= imgOrderSet.ClientID %>');
            var txtOrderSetName = $get('<%=txtOrderSetName.ClientID%>').value
            imgCntrl.style.display = 'block';

            $.ajax({
                type: "POST",
                url: '/Shared/Services/PrescriptionService.asmx/GetOrderSet',
                data: "{orderSetName:'" + txtOrderSetName + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    // console.log(res);
                    var jsonData = JSON.parse(res.d);

                    var sb = new StringBuilder();
                    $('#dvOrderSet').empty();
                    for (var i = 0; i < jsonData.length; i++) {
                        if (i < 35) {
                            // sb.append("<span id='" + jsonData[i].ItemId + "' style='border-radius: 10px;'><span class='label label-default' style='background-color: #808080;border-top-left-radius: 3px !important;border-bottom-left-radius: 3px !important;border-bottom-right-radius: 0px;border-top-right-radius: 0px;  cursor: pointer; padding:5px 10px;' onclick='setOrderSet(" + jsonData[i].SetId + "," + i + ",this)'>" + jsonData[i].SetName + " </span></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                            sb.append("<div class='pre_btn' id='" + jsonData[i].ItemId + "'><span class='btn-cus'   name='OrderSetItem' onclick='setOrderSet(" + jsonData[i].SetId + "," + i + ",this)'>" + jsonData[i].SetName + " </span></div>");
                        }
                    }
                    if (jsonData.length >= 35)
                        imgCntrl.style.display = '';
                    $('#dvOrderSet').append(sb.toString());

                },
                error: function (res) {
                    console.log(res);
                }
            });
        }

        function bindCurrentMedication() {

            var imgCntrl = document.getElementById('<%= imgCurrentMedicationPopup.ClientID %>');
            var txtCurrentItemName = $get('<%=txtCurrentItemName.ClientID%>').value
            imgCntrl.style.display = 'block';



            $.ajax({
                type: "POST",
                url: '/Shared/Services/PrescriptionService.asmx/GetCurrentMediation',
                data: "{itemName:'" + txtCurrentItemName + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    // console.log(res);
                    var jsonData = res.d;

                    var sb = new StringBuilder();
                    $('#dvCurrent').empty();
                    for (var i = 0; i < jsonData.length; i++) {
                        if (i < 100) {
                            //sb.append("<span id='" + jsonData[i].ItemId + "' style='border-radius: 10px;'><span class='label label-default' style='background-color: #20ab82;border-top-left-radius: 3px !important;border-bottom-left-radius: 3px !important;border-bottom-right-radius: 0px;border-top-right-radius: 0px;cursor: pointer;padding:4.5px 10px 5px 10px;' onclick='setCurrentMedication(" + jsonData[i].ItemId + "," + i + ",this)'>" + jsonData[i].ItemName + " </span>");
                            sb.append("<div class='pre_btn' id='" + jsonData[i].ItemId + "' style='border-radius: 10px;'><span name='CurrentMedicationItem' class='label label-default' style='background-color: #f2f2f2 ;color: #000000;border-top-left-radius: 3px !important;border-bottom-left-radius: 3px !important;display:inline-block;border-bottom-right-radius: 0px;border-top-right-radius: 0px;cursor: pointer;padding:4.5px 10px 5px 10px; margin-bottom:7px;' onclick='setCurrentMedication(" + JSON.stringify(jsonData[i]) + "," + i + ",this)'>" + jsonData[i].ItemName + " </span>");


                            if ('<%= common.myBool(Session["IsCIMSInterfaceActive"]) %>' == 'True') {
                                if (jsonData[i].CIMSItemId.toString().length > 10)
                                    sb.append("<span class='btn_hand' onclick='showMonograph(" + JSON.stringify(jsonData[i]) + ")'><img src='../../CIMSDatabase/monograph.png' alt='Monograph' width='16px' height='16px'></img></span>");

                                //sb.append("<span class='btn_hand' onclick='showMonograph(" + JSON.stringify(jsonData[i]) + ")'><img src='../../CIMSDatabase/interaction.png' alt='Interaction' width='16px' height='16px'></img></span>")
                            }
                            
                            sb.append("<span class='badge  mystyle' style='cursor: pointer;background-color: #a2a2a2 ;margin-left: 4px;margin-right: 2px;color: #fff;border-top-left-radius: 0px;border-top-right-radius: 4px;border-bottom-right-radius: 4px;border-bottom-left-radius: 0px;padding: 3.2px 9px;' onclick='stopCurrentMedication(" + Number(jsonData[i].ItemId) + "," + Number(jsonData[i].GenericId) + "," + i + ",this)'>Stop</span>");
                            sb.append("<span class='badge  mystyle' style='cursor: pointer;background-color: #a2a2a2 ;color: #fff;border-top-left-radius: 0px;border-top-right-radius: 4px;border-bottom-right-radius: 4px;border-bottom-left-radius: 0px;padding: 3.2px 9px;' onclick='PrintCurrentMedication(" + jsonData[i].ItemId + "," + i + ",this)'>Print</span>");
                            sb.append("</span></div>");
                        }
                    }
                    if (jsonData.length >= 100)
                        imgCntrl.style.display = '';

                    $('#dvCurrent').append(sb.toString());

                },
                error: function (res) {
                    console.log(res);
                }
            });
        }
        function setFavorateItem(data, index, control) {
            $('[name=favouriteItem]').css("background-color", "#f2f2f2");

            if (control != null) {
                control.style.backgroundColor = '#FFA500';
            }
            console.log(data);

            var cimsItemId = data.CIMSItemId;
            var cimsType = data.CIMSType;

            $get('<%=hdnCIMSItemId.ClientID%>').value = cimsItemId;
            $get('<%=hdnCIMSType.ClientID%>').value = cimsType;

            var ItemId = data.ItemId;
            var GenericId = data.GenericId;
            var FavoriteId = data.FavoriteId;

            //alert(Number(ItemId));
            //alert(Number(GenericId));

            <%--  var combo = $find("<%= ddlBrand.ClientID %>");
                comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text(data.ItemName);
                comboItem.set_value(data.ItemId);--%>

            if (Number(ItemId) > 0) {
                var combo = $find("<%= ddlBrand.ClientID %>");
                comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text(data.ItemName);
                comboItem.set_value(data.ItemId);
            }

            else if (Number(GenericId) > 0) {
                var combo = $find("<%= ddlGeneric.ClientID %>");
                comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text(data.GenericName);
                comboItem.set_value(data.GenericId);
            }



<%--             var combo = $find("<%= ddlGeneric.ClientID %>");
            comboItem = new Telerik.Web.UI.RadComboBoxItem();
            comboItem.set_text(data.ItemName);
            comboItem.set_value(data.GenericId);--%>

            //alert(data.ItemName);
            //  combo.trackChanges();
            combo.get_items().add(comboItem);
            //combo.commitChanges();
            comboItem.select();

            callRowCommandGeneric("selectItem", index, data.ItemId, data.GenericId, "Favourite", FavoriteId);

        }

        function showMonograph(data) {

            var cimsItemId = data.CIMSItemId;
            var cimsType = data.CIMSType;

            $get('<%=hdnCIMSItemIdClick.ClientID%>').value = cimsItemId;
            $get('<%=hdnCIMSTypeClick.ClientID%>').value = cimsType;

            $get('<%=btnMonographViewClick.ClientID%>').click();
        }

        //function executeEvent(e, data, index, control) {
        //    //See notes about 'which' and 'key'
        //    alert("1");
        //    if (e.keyCode == 13) {
        //        setFavorateItem(data, index, control);
        //        return false;
        //    }

        //}

        function setFavorateItem_OnClientClose(oWnd, args) {
            //  alert("setFavorateItem_OnClientClose");
            bindFavourite();
            var arg = args.get_argument();
            if (arg) {

                setFavorateItem(JSON.parse(arg.data), arg.index, null);

            }

        }
        function removeFavouriteItem(FavoriteId, itemId, GenericId, index, control) {
            //control.remove();
            document.getElementById(FavoriteId).remove();
            callRowCommandGeneric("ItemDelete", index, itemId, GenericId, "Favourite", FavoriteId);
            //bindFavourite();
        }

        function setOrderSet(data, index, control) {
            $('[name=OrderSetItem]').css("background-color", "#f2f2f2");
            if (control != null)
            { control.style.backgroundColor = '#FFA500'; }


            callRowCommand("SelectOrderSet", index, data, "OrderSet")
        }

        function setOrderSet_OnClientClose(oWnd, args) {
            bindOrderSet();
            var arg = args.get_argument();
            if (arg) {
                setOrderSet(arg.data, arg.index, null);

            }
        }

        function setCurrentMedication(data, index, control) {

            // alert("setCurrentMedication");
            $('[name=CurrentMedicationItem]').css("background-color", "#f2f2f2");
            if (control != null)
            { control.style.backgroundColor = '#FFA500'; }

            var cimsItemId = data.CIMSItemId;
            var cimsType = data.CIMSType;

            $get('<%=hdnCIMSItemId.ClientID%>').value = cimsItemId;
            $get('<%=hdnCIMSType.ClientID%>').value = cimsType;

            callRowCommandGeneric("selectItem", index, data.ItemId, data.GenericId, "Current", 0);

        }

        function setCurrentMedication_OnClientClose(oWnd, args) {
            bindCurrentMedication();
            var arg = args.get_argument();
            if (arg) {
                setCurrentMedication(JSON.parse(arg.data), arg.index, null);

            }
        }

        function stopCurrentMedication(itemId, GenericId, index, control) {
            //  document.getElementById(itemId).remove();
            callRowCommandGeneric("ItemStop", index, itemId, GenericId, "Current", 0);
        }

        function PrintCurrentMedication(itemId, index, control) {
            //  document.getElementById(itemId).remove();
            //   alert("PrintCurrentMedication");
            callRowCommand("PRINT", index, itemId, "Current");
        }

        function callRowCommand(commandName, index, itemId, commandType) {
            $get('<%=hdnFavouriteCommand.ClientID%>').value = commandName;
            $get('<%=hdnFavouriteIndex.ClientID%>').value = index;
            $get('<%=hdnFavouriteItemId.ClientID%>').value = itemId;
            $get('<%=hdnCommandType.ClientID%>').value = commandType;

            $get('<%=btnFavoriteRowCommand.ClientID%>').click();

        }


        function callRowCommandGeneric(commandName, index, itemId, GenericId, commandType, FavoriteId) {
            $get('<%=hdnFavouriteCommand.ClientID%>').value = commandName;
            $get('<%=hdnFavouriteIndex.ClientID%>').value = index;
            $get('<%=hdnFavouriteItemId.ClientID%>').value = itemId;
            $get('<%=hdnFavouriteGenericId.ClientID%>').value = GenericId;
            $get('<%=hdnFavoriteId.ClientID%>').value = FavoriteId;

            $get('<%=hdnCommandType.ClientID%>').value = commandType;

            $get('<%=btnFavoriteRowCommand.ClientID%>').click();

        }



        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false) {
                    return false;
                }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.className += " btn btn-primary";
                myButton.value = "Processing...";
            }
            return true;
        }

        function ddlFrequencyId_OnClientSelectedIndexChanged(sender, args) {




            ////////////////  setPrescriptionInstructoins Start //////////////

            var ddlUnitText = $find('<%=ddlUnit.ClientID %>').get_selectedItem().get_text();
            var ddlFrequencyIdText = $find('<%=ddlFrequencyId.ClientID %>').get_selectedItem().get_text();
            var ddlRouteText = $find('<%=ddlRoute.ClientID %>').get_selectedItem().get_text();
            var ddlPeriodTypeText = $find('<%=ddlPeriodType.ClientID %>').get_selectedItem().get_text();
            var txtDoseText = document.getElementById('<%= txtDose.ClientID %>').value;
            var txtDurationText = document.getElementById('<%= txtDuration.ClientID %>').value;

            $get('<%=txtInstructionsHeader.ClientID%>').value = "Take " + txtDoseText + " " + ddlUnitText + ", " + ddlFrequencyIdText + ", (" + ddlRouteText + "), For " + txtDurationText + " " + ddlPeriodTypeText;

            ////////////////  setPrescriptionInstructoins End //////////////

            ////////////////  calcTotalQtySelectedMed Start //////////////

            if (' <%= common.myBool(Session["ISCalculationRequired"]) %>') {
                var dose = document.getElementById('<%= txtDose.ClientID %>').value;

                //  var frequency = $find('<%=ddlFrequencyId.ClientID %>').get_selectedItem().get_value();
                <%--    if ($find('<%=ddlFrequencyId.ClientID %>').get_selectedItem().get_value() != 0) {--%>
                   <%--  var combo = $find("<%=ddlFrequencyId.ClientID %>");
                var frequency = combo.get_attributes().getAttribute("Frequency");--%>
                //  }

                var item = args.get_item();
                var frequency = item != null ? item.get_attributes().getAttribute("Frequency") : "0";
                $get('<%=hdnFrequencyIdAttributes.ClientID%>').value = item != null ? item.get_attributes().getAttribute("Frequency") : "0";
                // alert(frequency);
                var days = document.getElementById('<%= txtDuration.ClientID %>').value;
                var totalQty = 0;
                var unitname = '';
                unitname = $find('<%=ddlUnit.ClientID %>').get_selectedItem().get_text();

                switch ($find('<%=ddlPeriodType.ClientID %>').get_selectedItem().get_value()) {
                    case 'H':
                        days = days * 1;
                        break;
                    case 'D':
                        days = days * 1;
                        break;
                    case 'W':
                        days = days * 7;
                        break;
                    case 'M':
                        days = days * 30;
                        break;
                    case 'Y':
                        days = days * 365;
                        break;
                    default:
                        days = days * 1;
                        break;
                }

                totalQty = frequency * days * dose;
                $get('<%=txtTotQty.ClientID%>').value = totalQty;
            }
            else {
                $get('<%=txtTotQty.ClientID%>').value = '1';
            }



            ////////////////  calcTotalQtySelectedMed End //////////////

        }

        function setInstructoinsCalcTotalQty() {

            ////////////////  setPrescriptionInstructoins Start //////////////

            var ddlUnitText = $find('<%=ddlUnit.ClientID %>').get_selectedItem().get_text();
            var ddlFrequencyIdText = $find('<%=ddlFrequencyId.ClientID %>').get_selectedItem().get_text();
            var ddlRouteText = $find('<%=ddlRoute.ClientID %>').get_selectedItem().get_text();
            var ddlPeriodTypeText = $find('<%=ddlPeriodType.ClientID %>').get_selectedItem().get_text();
            var txtDoseText = document.getElementById('<%= txtDose.ClientID %>').value;
            var txtDurationText = document.getElementById('<%= txtDuration.ClientID %>').value;

            $get('<%=txtInstructionsHeader.ClientID%>').value = "Take " + txtDoseText + " " + ddlUnitText + ", " + ddlFrequencyIdText + ", (" + ddlRouteText + "), For " + txtDurationText + " " + ddlPeriodTypeText;

            ////////////////  setPrescriptionInstructoins End //////////////

            ////////////////  calcTotalQtySelectedMed Start //////////////

            if (' <%= common.myBool(Session["ISCalculationRequired"]) %>') {
                var dose = document.getElementById('<%= txtDose.ClientID %>').value;

                //  var frequency = $find('<%=ddlFrequencyId.ClientID %>').get_selectedItem().get_value();
                <%--    if ($find('<%=ddlFrequencyId.ClientID %>').get_selectedItem().get_value() != 0) {--%>
                   <%--  var combo = $find("<%=ddlFrequencyId.ClientID %>");--%>
                var frequency = $get('<%=hdnFrequencyIdAttributes.ClientID%>').value;
                //  }

                // alert(frequency);
                var days = document.getElementById('<%= txtDuration.ClientID %>').value;
                var totalQty = 0;
                var unitname = '';
                unitname = $find('<%=ddlUnit.ClientID %>').get_selectedItem().get_text();

                switch ($find('<%=ddlPeriodType.ClientID %>').get_selectedItem().get_value()) {
                    case 'H':
                        days = days * 1;
                        break;
                    case 'D':
                        days = days * 1;
                        break;
                    case 'W':
                        days = days * 7;
                        break;
                    case 'M':
                        days = days * 30;
                        break;
                    case 'Y':
                        days = days * 365;
                        break;
                    default:
                        days = days * 1;
                        break;
                }

                totalQty = frequency * days * dose;
                $get('<%=txtTotQty.ClientID%>').value = totalQty;
            }
            else {
                $get('<%=txtTotQty.ClientID%>').value = '1';
            }



            ////////////////  calcTotalQtySelectedMed End //////////////
        }


    </script>
    <script language="javascript" type="text/javascript">

        function OpenCIMSWindow() {
            var ReportContent = $get('<%=hdnCIMSOutput.ClientID%>')

            var WindowObject = window.open('', 'PrintWindow2', 'width=1250,height=585,top=72,left=30,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
            WindowObject.document.writeln(ReportContent.value);
            WindowObject.document.close();
            WindowObject.focus();
        }
        function ShowICDPanel(ctrlPanel, txt1) {
            var ICDarr = new Array();
            var txt = document.getElementById('<%=txtICDCode.ClientID%>');
            ICDarr = txt.value.split(',');

            var ICDCodes = '';
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
            var tableElement = document.getElementById('rptrICDCodes');
            if (tableElement != null) {
                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    col.checked = false;
                }

                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    var chklabel = rowElem.cells[0].childNodes[1];
                    for (var j = 0; j < ICDarr.length; j++) {
                        if (chklabel.innerText == ICDarr[j]) {
                            col.checked = true;
                        }
                    }
                }
            }
        }

        function CalcChange() {
            $get('<%=btnCalc.ClientID%>').click();
        }

        function OnClientMedicationOverrideClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsOverride = arg.IsOverride;
                var OverrideComments = arg.OverrideComments;
                var DrugAllergyScreeningResult = arg.DrugAllergyScreeningResult;

                $get('<%=hdnIsOverride.ClientID%>').value = IsOverride;
                $get('<%=hdnOverrideComments.ClientID%>').value = OverrideComments;
                $get('<%=hdnDrugAllergyScreeningResult.ClientID%>').value = DrugAllergyScreeningResult;
            }

            $get('<%=btnMedicationOverride.ClientID%>').click();
        }
        function SelectAllFavourite(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=lstFavourite.ClientID%>");
            //variable to contain the cell of the grid
            var cell;
            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

        function SelectAllCurrent(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvPrevious.ClientID%>");
            //variable to contain the cell of the grid
            var cell;
            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

        function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }

        function returnToParent() {
            var oArg = new Object();

            oArg.ControlId = $get('<%=hdnControlId.ClientID%>').value;
            oArg.ControlType = $get('<%=hdnControlType.ClientID%>').value;
            oArg.TemplateFieldId = $get('<%=hdnTemplateFieldId.ClientID%>').value;
            oArg.Sentence = $get('<%=hdnCtrlValue.ClientID%>').value;

            var oWnd = GetRadWindow();
            oWnd.close(oArg);

        }

        function OnChangeCheckboxRemoveFavourite(checkbox) {
            if (checkbox.checked) {

            }
            else {

            }
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        if (window.captureEvents) {
            window.captureEvents(Event.KeyUp);
            window.onkeyup = executeCode;
        }
        else if (window.attachEvent) {
            document.attachEvent('onkeyup', executeCode);
        }

        function OnClientFocusHandler(sender, eventArgs) {
            //if (!sender.get_dropDownVisible()) 
            {
                //sender.showDropDown();
            }
            //alert('y');
        }
        function executeCode(evt) {
            if (evt == null) {
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            var activeElement;
            switch (theKey) {
                case 114:  // F3
                    $get('<%=btnSave.ClientID%>').click();
                    break;
                case 113:  // F2
                    $get('<%=btnAlredyExistProceed.ClientID%>').click();
                    break;
                case 117:  // F6
                    $get('<%=btnAddItem.ClientID%>').click();
                    break;
                case 119:  // F8
                    $get('<%=btnclose.ClientID%>').click();
                    break;
                case 120:  // F9
                    $get('<%=btnPrint.ClientID%>').click();
                    break;
                case 13:  // Enter  document.getElementById('<%= tbDiagnosis.ClientID %>')
                    //var selectedTextArea = document.activeElement;
                    //alert(selectedTextArea);


                    //var focused = document.hasFocus();
                    //alert(focused);

                    if ($('#<%=txtFavouriteItemName.ClientID%>').is(":focus") || $('#<%=txtCurrentItemName.ClientID%>').is(":focus") || $('#<%=txtOrderSetName.ClientID%>').is(":focus")) {
                        // alert("focus");
                        var tabIndex = $find('<%= tbDiagnosis.ClientID %>'); //AdvOrBasicSearch is name of tabContainer
                        var i = tabIndex._activeTabIndex;
                        if (i == 0) {
                            // alert("enter");
                            // $('#dvFavorite').remove()
                            bindFavourite();
                            $get('<%=btnSearchFavourite.ClientID%>').click();
                        }
                        else if (i == 1) {

                            $get('<%=btnSearchCurrent.ClientID%>').click();
                            bindCurrentMedication();

                        }
                        else if (i == 2) {
                            bindOrderSet();
                            $get('<%=btnSearchOrderSet.ClientID%>').click();
                        }
                $('#dvFavorite').focus();

            }

            else {

                var tabIndex = $find('<%= tbDiagnosis.ClientID %>'); //AdvOrBasicSearch is name of tabContainer
                        var j = tabIndex._activeTabIndex;


                        activeElement = document.activeElement;
                        if (activeElement.tagName == 'SPAN')


                            //  alert(activeElement.getAttribute("onclick"));
                            var input = activeElement.getAttribute("onclick");
                        var dataJSON = input.substring(
        input.lastIndexOf("{"),
        input.lastIndexOf("}") + 1
    );
                        //  alert(dataJSON);

                        var index = input.substring(
        input.lastIndexOf("},") + 2,
        input.lastIndexOf(",this")
    );
                        // alert(index);
                        if (j == 0) {
                            setFavorateItem(JSON.parse(dataJSON), index, null);
                        }
                        else if (j == 1) // CurrentMedication
                        {
                            //alert("j1");
                            //alert(activeElement.getAttribute("onclick"));
                            var inputCurrentMedication = activeElement.getAttribute("onclick");
                            var datastring = inputCurrentMedication.substring(
        input.lastIndexOf("(") + 1,
        input.lastIndexOf(")")
    );

                            var dataarray = datastring.split(",");

                            //alert(array[1]);

                            //alert(dataarray[0]);
                            //alert(dataarray[1]);
                            setCurrentMedication(JSON.parse(dataarray[0]), dataarray[1], null);
                            //setCurrentMedication(dataarray[0], dataarray[1], null);
                        }
                        else if (j == 2) // OrderSet
                        {
                            //alert("j2");
                            //alert(activeElement.getAttribute("onclick"));
                            var inputOrderSet = activeElement.getAttribute("onclick");
                            var datastring = inputOrderSet.substring(
        input.lastIndexOf("(") + 1,
        input.lastIndexOf(")")
    );

                            var dataarray = datastring.split(",");

                            //alert(array[1]);

                            //alert(dataarray[0]);
                            //alert(dataarray[1]);
                            setOrderSet(dataarray[0], dataarray[1], null);
                        }
                        <%--  var radInput = $find("<%= ddlGeneric.ClientID %>");
                        radInput.focus();--%>

                        var comboBox = $find("<%=ddlGeneric.ClientID %>");
                        var input = comboBox.get_inputDomElement();
                        input.focus();


                    }




                    break;
                case 9:

                    //$('#favouriteItemId').focus();
                    //                    activeElement = document.activeElement;
                    //                    if (activeElement.tagName == 'SPAN')

                    //                    var input = activeElement.getAttribute("onclick");
                    //                    var dataJSON = input.substring(
                    //    input.lastIndexOf("{") ,
                    //    input.lastIndexOf("}")+1
                    //);
                    //                   // alert(dataJSON);

                    //                    var index=      input.substring(
                    //    input.lastIndexOf("},") + 2,
                    //    input.lastIndexOf(",this")
                    //);
                    //                    // alert(index);
                    //                    setFavorateItem(JSON.parse(dataJSON), index, null);

                    break;
                    //   var msg = document.getElementById('favouriteItemId');

<%--                    var msg = document.getElementById('favouriteItemId');
                    document.body.addEventListener('keydown', function (e) {
                        msg.textContent = 'keydown:' + e.keyCode;
                    });


                     var tabIndex = $find('<%= tbDiagnosis.ClientID %>'); //AdvOrBasicSearch is name of tabContainer
                    var i = tabIndex._activeTabIndex;
                    if (i == 0) {
                        bindFavourite();
                    }
                    else if (i == 1) {
                        bindCurrentMedication();
                    }
                    else if (i == 2) {
                        bindOrderSet();
                    }

                    var msg = document.getElementById('favouriteItemId');
                    document.body.addEventListener('keydown', function (e) {
                        msg.textContent = 'keydown:' + e.keyCode;
                    });--%>




            }
            evt.returnValue = false;
            return false;
        }

        function OnClientItemsRequesting(sender, eventArgs) {
            var ddlgeneric = $find('<%=ddlGeneric.ClientID %>');
            var value = ddlgeneric.get_value();
            var context = eventArgs.get_context();
            context["GenericId"] = value;
        }

        function ddlGeneric_OnClientSelectedIndexChanged(sender, args) {
            var ddlbrand = $find("<%=ddlBrand.ClientID%>");
            ddlbrand.clearItems();
            ddlbrand.set_text("");
            ddlbrand.get_inputDomElement().focus();

            var item = args.get_item();
            $get('<%=hdnGenericId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnGenericName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=hdnCIMSItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSItemId") : "";
            $get('<%=hdnCIMSType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSType") : "";

            $get('<%=hdnVIDALItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("VIDALItemId") : "";

            $get('<%=btnGetInfoGeneric.ClientID%>').click();
        }

        function ddlBrand_OnClientSelectedIndexChanged(sender, args) {

            var item = args.get_item();
            $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnItemName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=hdnCIMSItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSItemId") : "";
            $get('<%=hdnCIMSType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSType") : "";

            $get('<%=hdnVIDALItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("VIDALItemId") : "";
            $get('<%=btnGetInfo.ClientID%>').click();
        }

        function ddlBrandOnClientDropDownClosedHandler(sender, args) {

            if (sender.get_text().trim() == "") {

                $get('<%=hdnItemId.ClientID%>').value = "";
                $get('<%=hdnItemName.ClientID%>').value = "";

                $get('<%=hdnCIMSItemId.ClientID%>').value = "";
                $get('<%=hdnCIMSType.ClientID%>').value = "";
                $get('<%=hdnVIDALItemId.ClientID%>').value = "";

                $get('<%=btnGetInfo.ClientID%>').click();
            }
        }

        function OnClientCloseAIMedicineWindow(oWnd, args) {
            $get('<%=btnAddAIMedicineList.ClientID%>').click();
        }
        function OnClientClose(oWnd, args) {
            var arg = args.get_argument();

            $get('<%=hdnReturnIndentOPIPSource.ClientID%>').value = arg.IndentOPIPSource;
            $get('<%=hdnReturnIndentDetailsIds.ClientID%>').value = arg.IndentDetailsIds;
            $get('<%=hdnReturnIndentIds.ClientID%>').value = arg.IndentIds;
            $get('<%=hdnReturnItemIds.ClientID%>').value = arg.ItemIds;
            $get('<%=btnRefresh.ClientID%>').click();
        }
        function OnClientCloseFavourite(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnItemId.ClientID%>').value = arg.ItemIds;
            $get('<%=btnGetFavourite.ClientID%>').click();
        }
        function MaxLenTxt(TXT, MAX) {
            if (TXT.value.length > MAX) {
                alert("Text length should not be greater then " + MAX + " ...");

                TXT.value = TXT.value.substring(0, MAX);
                TXT.focus();
            }
        }

        function OnClientCloseVariableDose(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnXmlVariableDoseString.ClientID%>').value = arg.xmlVariableDoseString;
            $get('<%=hdnvariableDoseDuration.ClientID%>').value = arg.DoseDuration;
            $get('<%=hdnvariableDoseFrequency.ClientID%>').value = arg.DoseFrequency;
            $get('<%=hdnVariabledose.ClientID%>').value = arg.DoseValue;
            $get('<%=hdnNoOfDose.ClientID%>').value = arg.NoOfDose;
            $get('<%=hdnDateWise.ClientID%>').value = arg.DateWise;
            $get('<%=hdnXmlDoseString.ClientID%>').value = arg.XmlDoseString;
            $get('<%=txtInstructions.ClientID%>').value = arg.Instructions;
            $get('<%=btnVariableDoseClose.ClientID%>').click();
        }
        function OnClientCloseFrequencyTime(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnXmlFrequencyTime.ClientID%>').value = arg.xmlFrequencyString;
        }
        function OnClientCloseInsulingTime(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnXmlFrequencyTime.ClientID%>').value = arg.xmlFrequencyString;
        }

        function changeTab() {
            if ($find("<%=tbDiagnosis.ClientID%>").get_activeTabIndex() == 0) {

                document.getElementById('<%= txtFavouriteItemName.ClientID %>').style.display = "inline";
                document.getElementById('<%= txtCurrentItemName.ClientID %>').style.display = "none";
                document.getElementById('<%= txtOrderSetName.ClientID %>').style.display = "none";
            }
            else if ($find("<%=tbDiagnosis.ClientID%>").get_activeTabIndex() == 1) {

                document.getElementById('<%= txtFavouriteItemName.ClientID %>').style.display = "none";
                document.getElementById('<%= txtCurrentItemName.ClientID %>').style.display = "inline";
                document.getElementById('<%= txtOrderSetName.ClientID %>').style.display = "none";
            }
            else if ($find("<%=tbDiagnosis.ClientID%>").get_activeTabIndex() == 2) {

                document.getElementById('<%= txtFavouriteItemName.ClientID %>').style.display = "none";
                document.getElementById('<%= txtCurrentItemName.ClientID %>').style.display = "none";
                document.getElementById('<%= txtOrderSetName.ClientID %>').style.display = "inline";
            }
            else if ($find("<%=tbDiagnosis.ClientID%>").get_activeTabIndex() == 3) {

                document.getElementById('<%= txtFavouriteItemName.ClientID %>').style.display = "none";
                document.getElementById('<%= txtCurrentItemName.ClientID %>').style.display = "none";
                document.getElementById('<%= txtOrderSetName.ClientID %>').style.display = "none";
            }
            //var control = document.getElementById(controlId);
            //if (control.style.visibility == "visible" || control.style.visibility == "")
            //    control.style.visibility = "hidden";
            //else
            //    control.style.visibility = "visible";
                    <%-- $find("<%=tbDiagnosis.ClientID%>").set_activeTabIndex(0);--%>
        }


        //        function OnClientKeyPressFavSearch(oWnd, args) {
        //            // Get the input field
        //            var input = document.getElementById("txtFavouriteItemName");

        //// Execute a function when the user releases a key on the keyboard
        //input.addEventListener("keyup", function(event) {
        //  // Number 13 is the "Enter" key on the keyboard
        //    if (event.keyCode === 13) {
        //        alert("search");
        //    // Cancel the default action, if needed
        //    event.preventDefault();
        //    // Trigger the button element with a click

        //  }
        //});
        // }

        <%--  function OnClientKeyPressFavSearch(oWnd, args) {
             
            var input =document.getElementById('<%= txtFavouriteItemName.ClientID %>');
            input.addEventListener("keyup", function (event) {
                if (event.keyCode === 13) {
                    event.preventDefault();
                    bindFavourite();
                    // document.getElementById("myBtn").click();
                }
            });
        }--%>
    </script>

    <div class="container-fluid text-center form-group">
        <asplUD:UserDetails ID="asplUD" runat="server" />
        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" Visible="false" />
    </div>


    <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">
        <ContentTemplate>--%>
    <div class="container-fluid text-left" style="position: relative">

        <div style="position: absolute; left: 340px; top: 2px; margin-left: 227px;">
            <div class="form-group col-md-12">
                <%--<input type="text"  placeholder="Search" style="height:20px" />--%>

                <%--<asp:TextBox ID="txtFavouriteItemName"  runat="server" style="height:20px"  />--%>
                <%--<telerik:RadTextBox ID="txtSearch" EmptyMessage="Search" runat="server" Style="height: 20px" />--%>
                <asp:TextBox ID="txtFavouriteItemName" runat="server" placeholder="Search Favourite" SkinID="textbox" Style="height: 20px; width: 200px!important" />
                <%--  <asp:Panel ID="Panel7" runat="server" DefaultButton="btnSearchCurrent">--%>
                <asp:TextBox ID="txtCurrentItemName" runat="server" placeholder="Search Current Item" SkinID="textbox" Style="height: 20px; display: none; width: 200px!important" />
                <%--</asp:Panel>--%>

                <%--<asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearchOrderSet" Style="visibility: hidden;">--%>
                <asp:TextBox ID="txtOrderSetName" runat="server" SkinID="textbox" placeholder="Search Order Set" Style="height: 20px; display: none; width: 200px!important" />
                <%-- </asp:Panel>--%>
                <%--Width="467%"--%>
            </div>
        </div>


        <AJAX:TabContainer ID="tbDiagnosis" runat="server" ActiveTabIndex="0" Width="100%" OnClientActiveTabChanged="changeTab">
            <%--   <AJAX:TabContainer ID="tbDiagnosis" runat="server" ActiveTabIndex="0" Width="100%" AutoPostBack="true" OnActiveTabChanged="tbDiagnosis_ActiveTabChanged">--%>
            <AJAX:TabPanel ID="tabFavorite" runat="server" Font-Size="9px" HeaderText="Today's Diagnosis">
                <HeaderTemplate>
                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnFavourite" CssClass="nav-link" runat="server" Text="Favourites" OnClick="btnFavourite_Click" />
                            <%-- BackColor="White" ForeColor="Black" BorderColor="Blue" Font-Bold="true" --%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnFavourite" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                </HeaderTemplate>
                <ContentTemplate>
                    <%--   <button type="button" class="btn btn-primary btn-sm " id="btnTest">Referesh</button>
                    <button type="button" class="btn btn-danger  btn-sm" id="btnClear">clear</button>
                    <input class="form-control" id="myInput" type="text" placeholder="Search..">
                    --%>
                    <div class="tab_bt">
                        <div class="">
                            <div id="dvFavorite" class="d-flex">
                            </div>
                        </div>
                        <div style="display: block; float: right; display: none" class="">
                            <asp:Button ID="imgMedicationPopup" OnClick="imgMedicationPopup_Click" Text="+" runat="server" CssClass=" fixi btn btn-primary" />
                        </div>

                    </div>
                    <div class="clearfix"></div>


                </ContentTemplate>
            </AJAX:TabPanel>
            <AJAX:TabPanel ID="TabPanel1" runat="server" Height="25px" HeaderText="Current">
                <HeaderTemplate>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnCurrentMedication" runat="server" CssClass="nav-link" Text="Current" OnClick="btnCurrentMedication_Click" />

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCurrentMedication" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </HeaderTemplate>
                <ContentTemplate>
                    <%--  <button type="button" class="btn btn-primary btn-sm " id="btnTest">Referesh</button>
                    <button type="button" class="btn btn-danger  btn-sm" id="btnClear">clear</button>--%>
                    <span id="dvCurrent"></span>

                    <span style="display: block; float: right; display: none">
                        <asp:ImageButton ID="imgCurrentMedicationPopup" OnClick="imgCurrentMedicationPopup_Click" ImageUrl="~/Images/add.gif" runat="server" /></span>

                </ContentTemplate>


            </AJAX:TabPanel>
            <AJAX:TabPanel ID="TabPanel2" runat="server" Height="25px" HeaderText="Order Set">
                <HeaderTemplate>
                    <asp:UpdatePanel ID="up1" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnOrderSet" CssClass="nav-link" runat="server" Text="Order Set" OnClick="btnOrderSet_Click" />

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnOrderSet" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </HeaderTemplate>
                <ContentTemplate>
                    <%--  		
                    <button type="button" class="btn btn-primary btn-sm " id="btnTest">Referesh</button>
                    <button type="button" class="btn btn-danger  btn-sm" id="btnClear">clear</button>
                
                    --%>
                    <%--<div id="dvOrderSet" style="width: 99%; max-width: 99%; max-height: 100px; overflow-x: auto;"></div>--%>
                    <span id="dvOrderSet"></span>
                    <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>--%>
                    <span style="display: block; float: right; display: none">
                        <asp:ImageButton ID="imgOrderSet" OnClick="imgOrderSet_Click" ImageUrl="~/Images/add.gif" runat="server" />
                    </span>
                    <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </ContentTemplate>

            </AJAX:TabPanel>
            <AJAX:TabPanel ID="TabPanel3" runat="server" Height="25px" HeaderText="Previous Prescription">
                <HeaderTemplate>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnPreviousMedications" CssClass="nav-link" runat="server" SkinID="label"
                                OnClick="btnPreviousMedications_Click" Text="Previous Medications" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </HeaderTemplate>
                <ContentTemplate>
                </ContentTemplate>
            </AJAX:TabPanel>
        </AJAX:TabContainer>


    </div>
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
    <div class="clearfix"></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main hidden" style="background: #fff; padding-top: 10px;">
                <div class="row">
                    <div class="col-md-3">
                        <h2>
                            <asp:Label ID="Label1" runat="server" Text="&nbsp;Drug&nbsp;Order" SkinID="label" /></h2>
                    </div>
                    <div class="col-md-5">
                    </div>
                    <div class="col-md-4 text-right">
                        <div id="Td1" style="width: 10px" runat="server" visible="false">
                            <asp:Label ID="Label65" runat="server" Text="Prescription No" SkinID="label" />&nbsp;
                                    <telerik:RadComboBox ID="ddlPrescription" runat="server" EmptyMessage="[ Select ]"
                                        Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlPrescription_SelectedIndexChanged" />
                            &nbsp;
                                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Print (F9)" OnClick="btnPrint_Click"
                                        Visible="false" CausesValidation="false" />
                        </div>




                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="&nbsp;Order&nbsp;Priority" Style="display: none" />

                        <telerik:RadComboBox ID="ddlIndentType" runat="server" Width="100px" Filter="Contains" Style="display: none" />




                        <asp:Button ID="btnclose" Text="Close (F8)" runat="server" CssClass="btn btn-primary" Visible="false"
                            OnClick="btnClose_OnClick" />
                    </div>
                </div>
            </div>


            <div class="container-fluid">
                <div class="" style="background: #fff; padding-top: 10px; border-top: 1px solid #ccc">
                    <div class="RadPane2">
                        <div class="col-md-6">

                            <div class="border-line prescription-section">
                                <div class="form-group row">
                                    <div class="col-md-3" id="trGeneric" runat="server">
                                        <asp:Label ID="Label16" runat="server" SkinID="label" Text="Generic" Style="display: none" />

                                        <%--<telerik:RadComboBox ID="ddlGeneric" runat="server" Width="100%" Height="300px" EmptyMessage=""
                                        AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true" OnItemsRequested="ddlGeneric_OnItemsRequested"
                                        DataTextField="GenericName" DataValueField="GenericId" Skin="Office2007" ShowMoreResultsBox="true"
                                        EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlGeneric_OnClientSelectedIndexChanged" />--%>
                                        <telerik:RadComboBox ID="ddlGeneric" runat="server" DataTextField="GenericName"
                                            DataValueField="GenericId" HighlightTemplatedItems="true" Height="300px" Width="100%"
                                            DropDownWidth="450px" ZIndex="50000" EmptyMessage="Generic" AllowCustomText="true" MarkFirstMatch="true"
                                            EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true" TabIndex="1"
                                            OnItemsRequested="ddlGeneric_OnItemsRequested" OnClientSelectedIndexChanged="ddlGeneric_OnClientSelectedIndexChanged">
                                            <HeaderTemplate>
                                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 100%" align="left">
                                                            <asp:Label ID="Label28" runat="server" Text="Generic Name" />
                                                        </td>
                                                        <td style="width: 0%" align="right" visible="false">
                                                            <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSItemId']")%> />
                                                        </td>
                                                        <td style="width: 0%" align="right" visible="false">
                                                            <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSType']")%> />
                                                        </td>
                                                        <td style="width: 0%" align="right" visible="false">
                                                            <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['VIDALItemId']")%> />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 100%" align="left">
                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="check-all">
                                            <asp:CheckBox ID="chkAllbrand" runat="server" TextAlign="Right" Text="All" AutoPostBack="true" OnCheckedChanged="chkAllbrand_CheckedChanged" />
                                        </div>
                                        <asp:Label ID="lblInfoBrand" runat="server" SkinID="label" Text="Brand" Style="display: none" />

                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlBrand" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <telerik:RadComboBox ID="ddlBrand" runat="server" HighlightTemplatedItems="true"
                                                    Width="86%" DropDownWidth="450px" Height="300px" EmptyMessage="Brand"
                                                    AllowCustomText="true" EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                    MarkFirstMatch="true" ZIndex="50000" OnItemsRequested="ddlBrand_OnItemsRequested" TabIndex="2"
                                                    OnClientItemsRequesting="OnClientItemsRequesting" OnClientSelectedIndexChanged="ddlBrand_OnClientSelectedIndexChanged"
                                                    OnClientDropDownClosed="ddlBrandOnClientDropDownClosedHandler">
                                                    <HeaderTemplate>
                                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 85%" align="left">
                                                                    <asp:Label ID="Label28" runat="server" Text="Item Name" />
                                                                </td>
                                                                <td style="width: 10%" align="left">
                                                                    <asp:Label ID="Label29" runat="server" Text="Stock" />
                                                                </td>
                                                                <td style="width: 0%" align="right" visible="false"></td>
                                                                <td style="width: 0%" align="right" visible="false"></td>
                                                                <td style="width: 0%" align="right" visible="false"></td>

                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 85%" align="left">
                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                </td>
                                                                <td style="width: 10%" align="left">
                                                                    <%# DataBinder.Eval(Container, "Attributes['ClosingBalance']")%>
                                                                </td>
                                                                <td style="width: 0%" align="right" visible="false">
                                                                    <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSItemId']")%> />

                                                                </td>
                                                                <td style="width: 0%" align="right" visible="false">
                                                                    <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSType']")%> />
                                                                </td>
                                                                <td style="width: 0%" align="right" visible="false">
                                                                    <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['VIDALItemId']")%> />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-1" style="display: none">
                                        <asp:ImageButton ID="btnBrandDetailsViewOnItemBased" runat="server" Visible="false"
                                            Width="18px" Height="18px"
                                            ToolTip="View Brand Details" OnClick="btnBrandDetailsView_OnClick" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:ImageButton ID="btnMonographViewOnItemBased" runat="server"
                                            ImageUrl="~/CIMSDatabase/monograph.png" Width="18px" Height="18px"
                                            ToolTip="View Monograph" OnClick="btnMonographView_OnClick" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:ImageButton ID="btnInteractionViewOnItemBased" runat="server"
                                            ImageUrl="~/CIMSDatabase/interaction.png" Width="18px" Height="18px"
                                            ToolTip="View Interaction" OnClick="btnInteractionView_OnClick" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:ImageButton ImageUrl="~/Icons/bookmark-white.png" Width="35px" ID="btnAddtoFav" runat="server" CssClass="btn btn-primary"
                                            ToolTip="Add To Favourite" OnClick="btnAddtoFav_Click" TabIndex="-1" />
                                    </div>
                                </div>
                                <div style="visibility: hidden; display: none">
                                    <div class="col-md-12">row gap </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-3">
                                        <p style="margin-bottom: 5px">Dose</p>
                                        <asp:TextBox ID="txtDose" runat="server" Text='<%#Eval("Dose") %>' SkinID="textbox"
                                            Width="30%" MaxLength="5" Style="text-align: right; float: left" AutoPostBack="false" onchange="setInstructoinsCalcTotalQty()"
                                            TabIndex="3" />
                                        <%--TabIndex="3"  OnTextChanged="txtDose_TextChanged"--%>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose" ValidChars="." />
                                        <%--<p style="margin-top:3px;  margin-left:50px">2 tablets</p>--%>
                                        <telerik:RadComboBox ID="ddlUnit" runat="server" MarkFirstMatch="true" Filter="Contains" AutoPostBack="false" TabIndex="4"
                                            Width="60%" CssClass="pull-right" Height="250px" EmptyMessage="Unit" DropDownWidth="150px" OnClientSelectedIndexChanged="setInstructoinsCalcTotalQty">
                                        </telerik:RadComboBox>
                                        <%--  TabIndex="4" OnClientFocus="OnClientFocusHandler" OnSelectedIndexChanged="ddlUnit_OnSelectedIndexChanged"--%>
                                    </div>
                                    <div class="col-md-2">
                                        <p style="margin-bottom: 5px">Frequency</p>
                                        <%--<input type="text" style="width:40px; float:left" />--%>
                                        <telerik:RadComboBox ID="ddlFrequencyId" DropDownWidth="160px" CssClass="pull-left" runat="server" MarkFirstMatch="true" Filter="Contains"
                                            EmptyMessage="[ Select ]" Width="100%" Height="250px" AutoPostBack="false" TabIndex="5"
                                            OpenDropDownOnFocus="true" EnableVirtualScrolling="true"
                                            OnClientSelectedIndexChanged="ddlFrequencyId_OnClientSelectedIndexChanged">
                                        </telerik:RadComboBox>
                                        <%-- TabIndex="5"  OnClientFocus="OnClientFocusHandler"   OnSelectedIndexChanged="ddlFrequency_OnSelectedIndexChanged"--%>
                                    </div>
                                    <div class="col-md-4">
                                        <p style="margin-bottom: 5px; visibility: hidden; display: none">
                                            Before or After Meal
                                             <span id="spnRoute" runat="server" style="color: Red">*</span>
                                        </p>
                                        <telerik:RadComboBox ID="ddlFoodRelation" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlFoodRelation_SelectedIndexChanged" Filter="Contains" EmptyMessage="[ Select ]"
                                            Width="100%" ToolTip="Relationship to Food" Style="visibility: hidden; display: none" TabIndex="-1" />

                                        <p style="margin-bottom: 5px;">Route <span id="Span1" runat="server" style="color: Red">*</span></p>
                                        <telerik:RadComboBox ID="ddlRoute" runat="server" MarkFirstMatch="true" Filter="Contains"
                                            EmptyMessage="[ Select ]" Width="100%" Height="250px" AutoPostBack="false" TabIndex="6"
                                            OnClientSelectedIndexChanged="setInstructoinsCalcTotalQty" />
                                        <%--  TabIndex="6" OnClientFocus="OnClientFocusHandler" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged"--%>
                                    </div>
                                    <div class="col-md-3">
                                        <p style="margin-bottom: 5px">Duration</p>
                                        <asp:TextBox ID="txtDuration" runat="server" SkinID="textbox" TabIndex="7"
                                            Width="40px" MaxLength="2" Style="text-align: right" AutoPostBack="false"
                                            onchange="setInstructoinsCalcTotalQty()" />
                                        <%--OnTextChanged="txtDuration_TextChanged"--%>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDuration" ValidChars="">
                                        </AJAX:FilteredTextBoxExtender>
                                        <telerik:RadComboBox ID="ddlPeriodType" runat="server" Width="64%" AutoPostBack="false" TabIndex="8"
                                            OnClientSelectedIndexChanged="setInstructoinsCalcTotalQty">
                                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodType_OnSelectedIndexChanged"--%>
                                            <Items>
                                                <%--<telerik:RadComboBoxItem Text="Minute(s)" Value="N"  />--%>
                                                <%--<telerik:RadComboBoxItem Text="Hour(s)" Value="H" />--%>
                                                <telerik:RadComboBoxItem Text="Day(s)" Value="D" Selected="true" />
                                                <telerik:RadComboBoxItem Text="Week(s)" Value="W" />
                                                <telerik:RadComboBoxItem Text="Month(s)" Value="M" />
                                                <telerik:RadComboBoxItem Text="Year(s)" Value="Y" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <%--TabIndex="8" OnClientFocus="OnClientFocusHandler"--%>
                                    </div>

                                </div>
                                <div style="visibility: hidden; display: none">
                                    <div class="col-md-12">row gap </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-12">
                                        <p>
                                            <asp:Label ID="Label61" runat="server" Text="Special Instructions" />
                                        </p>
                                    </div>
                                    <div class="col-md-7">
                                        <div class="input_disp_co" style="margin: 1px 0px;">
                                            <p>
                                                <asp:TextBox ID="txtInstructionsHeader" runat="server" ReadOnly="true" class="form-control" style="background: #fff !important; font-size: 12px;" TabIndex="-1" />
                                            </p>
                                        </div>
                                        <%--<input type="text" class="pull-left" style="width:35%" />--%>
                                        <telerik:RadComboBox ID="ddlInstructions" runat="server" EmptyMessage="[ Select ]"
                                            Style="width: 35%; visibility: hidden; display: none;" AutoPostBack="true" OnSelectedIndexChanged="ddlInstructions_SelectedIndexChanged" />
                                        <div style="width: 100%; float: right;">
                                            <asp:TextBox ID="txtInstructions" runat="server" SkinID="textbox" TextMode="MultiLine" MaxLength="1000" CssClass="" Width="100%"
                                                onkeyup="return MaxLenTxt(this, 1000);" placeholder="Type more..." TabIndex="-1" />
                                        </div>
                                        <asp:ImageButton ID="imgSaveInstruction" runat="server" Style="visibility: hidden; display: none;"
                                            ToolTip="Click here to Save selected Instructions into your favorite instructions set"
                                            ImageUrl="~/Images/Login/orrange-arrow.GIF" Width="18px"
                                            OnClick="imgSaveInstuctions_Click" TabIndex="-1" />
                                    </div>
                                    <div class="col-md-5" style="padding: 0;">
                                        <div class="">
                                            <%--dt_btn2--%>
                                            <div class="col-md-6">
                                                <asp:Button ID="btnVariableDose" runat="server" Text="Variable Dose" CssClass="btn_cus2"
                                                    OnClick="btnVariableDose_OnClick" Visible="false" />
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Button ID="btnAddItem" runat="server" CssClass="btn_cus2" Text="Add Medicine (F6)"
                                                    OnClick="btnAddItem_OnClick" TabIndex="9" />
                                            </div>
                                            <%--<span class="glyphicon glyphicon-menu-right"></span>--%>
                                        </div>

                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <fieldset class="pt_cus_field">
                                            <div class="form-group_cu">
                                                <label>Substitute&nbsp;Not&nbsp;Allowed</label>
                                                <label class="switch">
                                                    <%--<input type="checkbox" checked>--%>
                                                    <asp:CheckBox ID="chkSubstituteNotAllow" runat="server" />
                                                    <span class="slider round"></span>
                                                </label>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="col-md-5" id="dvDrugAdmin" runat="server" visible="false">
                                        <div class="">
                                            <%--dt_btn2--%>
                                            <div class="col-md-6">
                                                <label>Drug Admin In</label>
                                            </div>
                                            <div class="col-md-6">
                                                <telerik:RadComboBox ID="ddlDrugAdminIn" runat="server" Width="100%">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="OP" Value="1" />
                                                        <telerik:RadComboBoxItem Text="Pre OP" Value="2" />
                                                        <telerik:RadComboBoxItem Text="Intra OP" Value="3" />
                                                        <telerik:RadComboBoxItem Text="PACU" Value="4" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="">
                                            <%--dt_btn2--%>
                                            <div class="col-md-6">
                                                <label>Total&nbsp;Qty</label>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtTotQty" runat="server" SkinID="textbox" Text="0" Width="70px"
                                                    MaxLength="10" Style="text-align: right" autocomplete="off" Enabled="true" />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                    FilterType="Custom,Numbers" TargetControlID="txtTotQty" ValidChars="0123456789." />
                                            </div>
                                        </div>

                                        <%-- Total Quantity :
                                        <asp:TextBox ID="txtTotalQuantity" Width="50px" runat="server"></asp:TextBox>--%>

                                        <%--<asp:Label ID="lblTotalQtySelectedMed" runat="server" Text=""></asp:Label>--%>
                                    </div>
                                </div>
                                <%-- <div class="form-group  checkboxes">
                                </div>--%>
                                <div class="form-group">
                                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                        <tr>
                                            <td>
                                                <%--#E3DCCO--%>
                                                <asp:Panel ID="Panel5" runat="server" BorderStyle="Solid" BorderWidth="2px" bgcolor="#337ab7" BorderColor="#337ab7">
                                                    <table border="0" width="100%" bgcolor="#337ab7" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="height: 22px; width: 120px" bgcolor="#337ab7">
                                                                <asp:Label ID="Label22" runat="server" SkinID="label" Text="&nbsp;Drug Attributes"
                                                                    ForeColor="white" Font-Bold="true" />
                                                            </td>
                                                            <td align="right" bgcolor="#337ab7">
                                                                <asp:LinkButton ID="lnkPharmacistInstruction" runat="server" Font-Bold="true" OnClick="lnkPharmacistInstruction_OnClick"
                                                                    Text="Instruction For Patient" ToolTip="Instruction For Patient" Font-Size="Smaller" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px;"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel4" runat="server">
                                                    <div class="row">
                                                        <div class="form-group">
                                                            <div class="col-md-6 form-group">
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="Label10" runat="server" Text="Strength" ForeColor="#666666" />
                                                                    </div>
                                                                    <div class="col-md-9">
                                                                        <asp:TextBox ID="txtStrengthValue" runat="server" SkinID="textbox"
                                                                            MaxLength="255" />
                                                                        <br />
                                                                        <asp:LinkButton ID="lnkTappereddose" runat="server" OnClick="lnkTappereddose_Click"
                                                                            SkinID="label" Text="Tapered Dose" Font-Size="Smaller" Style="display: none" />
                                                                    </div>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        <asp:Label ID="Label3" runat="server" Text="Form" ForeColor="#666666" />
                                                                    </div>
                                                                    <div class="col-md-9">
                                                                        <telerik:RadComboBox ID="ddlFormulation" runat="server" MarkFirstMatch="true" Filter="Contains"
                                                                            EmptyMessage="[ Select ]" AutoPostBack="false" OnSelectedIndexChanged="ddlFormulation_OnSelectedIndexChanged"
                                                                            Height="250px" />
                                                                    </div>

                                                                </div>
                                                            </div>

                                                        </div>

                                                    </div>

                                                    <table border="0" cellpadding="2" cellspacing="0">


                                                        <tr>
                                                            <td align="center" colspan="4" style="display: none">
                                                                <img src="../../Images/Fading-Line.jpg" height="99%" alt="line" />
                                                            </td>
                                                        </tr>

                                                        <tr style="display: none">
                                                            <td id="trStrtdtlbl" runat="server" visible="false">
                                                                <asp:Label ID="Label14" runat="server" SkinID="label" Text="Start Date" Font-Size="Smaller" Visible="false" />
                                                            </td>
                                                            <td id="trStrtdt" runat="server" visible="false">
                                                                <%--AutoPostBack="true" OnSelectedDateChanged="txtStartDate_OnSelectedDateChanged"--%>
                                                                <telerik:RadDatePicker ID="txtStartDate" runat="server" Width="100px" DbSelectedDate='<%#Eval("StartDate")%>'>
                                                                    <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy" />
                                                                </telerik:RadDatePicker>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label15" runat="server" SkinID="label" Text="End Date" Visible="false"
                                                                    Font-Size="Smaller" />
                                                                <asp:Label ID="Label21" runat="server" SkinID="label" Text="Order Type" Font-Size="Smaller" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlDoseType" runat="server" Width="110px" EmptyMessage="[ Select ]"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDoseType_OnSelectedIndexChanged" />
                                                                <telerik:RadDatePicker ID="txtEndDate" runat="server" Width="100px" Enabled="false"
                                                                    DbSelectedDate='<%#Eval("EndDate")%>' Visible="false">
                                                                    <DateInput ID="DateInput2" runat="server" DateFormat="dd/MM/yyyy" />
                                                                </telerik:RadDatePicker>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="4" style="display: none">
                                                                <img src="../../Images/Fading-Line.jpg" height="99%" alt="line" />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trLinkedServices" visible="false">
                                                            <td>
                                                                <asp:Label ID="Label5" runat="server" SkinID="label" Text="Linked Item" Font-Size="Smaller" />
                                                            </td>
                                                            <td colspan="3">
                                                                <telerik:RadComboBox ID="ddlReferanceItem" runat="server" Width="99%" EmptyMessage="[ Select ]"
                                                                    AppendDataBoundItems="true">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="" Value="-1" Selected="true" />
                                                                        <telerik:RadComboBoxItem Text="Diluents" Value="0" />
                                                                        <telerik:RadComboBoxItem Text="Normal Saline" Value="100001" />
                                                                        <telerik:RadComboBoxItem Text="Dextrose 5%" Value="100002" />
                                                                        <telerik:RadComboBoxItem Text="DNS" Value="100003" />
                                                                        <telerik:RadComboBoxItem Text="Others" Value="100004" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trVolumn" visible="false">
                                                            <td>
                                                                <asp:Label ID="Label29" runat="server" SkinID="label" Text="Volume" Font-Size="Smaller" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVolume" runat="server" Text="" SkinID="textbox" Width="35px"
                                                                    MaxLength="5" Style="text-align: right" />
                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="txtVolume" ValidChars=".">
                                                                </AJAX:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label34" runat="server" SkinID="label" Text="Volume Unit" Font-Size="Smaller" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlVolumeUnit" runat="server" MarkFirstMatch="true" EmptyMessage="[ Select ]"
                                                                    ToolTip="Volume unit" Width="110px" />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trFlowRate" visible="false">
                                                            <td>
                                                                <asp:Label ID="Label35" runat="server" SkinID="label" Text="Flow Rate" Font-Size="Smaller" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFlowRateUnit" runat="server" Text="" SkinID="textbox" Width="35px"
                                                                    MaxLength="5" Style="text-align: right" />
                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                    FilterType="Custom, Numbers" TargetControlID="txtFlowRateUnit" ValidChars=".">
                                                                </AJAX:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label36" runat="server" SkinID="label" Text="Flow&nbsp;Rate&nbsp;Unit"
                                                                    Font-Size="Smaller" />
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="1">
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="ddlFlowRateUnit" Width="70px" runat="server" MarkFirstMatch="true"
                                                                                EmptyMessage="[ Select ]" ToolTip="Flow Rate Unit" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlMinute" runat="server" SkinID="DropDown">
                                                                                <asp:ListItem Text="/Minute" Value="0" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="/Hour" Value="1"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <%--<asp:Label ID="Label20" runat="server" SkinID="label" Text="/Minute" Font-Size="Smaller" />--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trTotalVolumn" visible="false">
                                                            <td valign="top">
                                                                <asp:Label ID="Label32" runat="server" SkinID="label" Text="Total Volume" Visible="false"
                                                                    Font-Size="Smaller" />
                                                                <asp:Label ID="Label33" runat="server" SkinID="label" Text="Time" Font-Size="Smaller" />
                                                            </td>
                                                            <td colspan="3">
                                                                <table cellpadding="0" cellspacing="1">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtTotalVolumn" runat="server" Text="" SkinID="textbox" Width="80px"
                                                                                MaxLength="50" Visible="false" />
                                                                            <asp:TextBox ID="txtTimeInfusion" runat="server" Text="" SkinID="textbox" Width="35px"
                                                                                MaxLength="5" Style="text-align: right" />
                                                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                FilterType="Custom, Numbers" TargetControlID="txtTimeInfusion" ValidChars=".">
                                                                            </AJAX:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="ddlTimeUnit" Width="80px" runat="server" MarkFirstMatch="true"
                                                                                EmptyMessage="[ Select ]" ToolTip="Infusion Time unit">
                                                                                <Items>
                                                                                    <telerik:RadComboBoxItem Value="0" Text="" Selected="true" />
                                                                                    <telerik:RadComboBoxItem Value="H" Text="Hour(s)" />
                                                                                    <telerik:RadComboBoxItem Value="M" Text="Minute(s)" />
                                                                                    <telerik:RadComboBoxItem Value="S" Text="Second (S)" />
                                                                                </Items>
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="Tr1" runat="server" visible="false">
                                                            <td valign="top">
                                                                <asp:Label ID="label67" runat="server" SkinID="label" Font-Size="Smaller" Text="Special Instrucation" />
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtSpecialInstrucation" runat="server" ReadOnly="true" TextMode="MultiLine"
                                                                    Width="320px" Height="30px" />
                                                            </td>
                                                        </tr>

                                                        <tr style="display: none">
                                                            <td colspan="4">
                                                                <table cellpadding="0" cellspacing="1" width="100%">
                                                                    <tr>
                                                                        <td valign="top" colspan="2">
                                                                            <asp:CheckBox ID="chkNotToPharmacy" runat="server" SkinID="checkbox" TextAlign="Right"
                                                                                Font-Bold="true" Text="Own Medication" Font-Size="Smaller" />
                                                                        </td>
                                                                        <td>
                                                                            <%--<asp:Button ID="btnVariableDose" runat="server" CssClass="btn btn-primary  btn-xs" Text="Variable Dose"
                                                                                OnClick="btnVariableDose_OnClick" Font-Size="Smaller" Visible="false" />--%>
                                                                            <asp:Button ID="btnInsulingSliding" runat="server" CssClass="btn btn-primary  btn-xs" Text="Insuling Sliding"
                                                                                OnClick="btnInsulingSliding_Click" Font-Size="Smaller" Visible="false" />
                                                                        </td>

                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="hidden">

                                        <asp:Panel ID="pnlFavouriteDetails" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            BorderColor="SkyBlue" Height="440px" ScrollBars="Auto" Style="display: none">
                                            <asp:GridView ID="lstFavourite" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                                AllowPaging="true" OnPageIndexChanging="lstFavourite_PageIndexChanging"
                                                OnRowDataBound="lstFavourite_OnRowDataBound" OnRowCommand="lstFavourite_OnRowCommand" PageSize="500">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" SkinID="label" ForeColor="Red" Font-Bold="true" />
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                                            ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Favourite Drug(s)">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkItemName" runat="server" ToolTip="Click here to add new prescription"
                                                                CausesValidation="false" CommandName='selectItem' CommandArgument='<%#Eval("ItemId")%>'
                                                                Text='<%#Eval("ItemName")%>' Font-Size="Smaller" />
                                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                            <asp:HiddenField ID="hdnDose" runat="server" Value='<%#Eval("Dose")%>' />
                                                            <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId")%>' />
                                                            <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId")%>' />
                                                            <asp:HiddenField ID="hdnStrengthValue" runat="server" Value='<%#Eval("StrengthValue")%>' />
                                                            <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%#Eval("FormulationId")%>' />
                                                            <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId")%>' />
                                                            <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%#Eval("FrequencyId")%>' />
                                                            <asp:HiddenField ID="hdnDuration" runat="server" Value='<%#Eval("Duration")%>' />
                                                            <asp:HiddenField ID="hdnDurationType" runat="server" Value='<%#Eval("DurationType")%>' />
                                                            <asp:HiddenField ID="hdnFoodRelationshipId" runat="server" Value='<%#Eval("FoodRelationshipId")%>' />
                                                            <asp:HiddenField ID="hdnFCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                            <asp:HiddenField ID="hdnFCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                            <asp:HiddenField ID="hdnFVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                            <asp:HiddenField ID="hdnInstructions" runat="server" Value='<%#Eval("Instructions") %>' />
                                                            <asp:HiddenField ID="hdnGenericName" runat="server" Value='<%#Eval("GenericName")%>' />
                                                            <asp:HiddenField ID="hdnXmlVariableDose" runat="server" Value='<%#Eval("XmlVariableDose") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                                CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                                ImageUrl="~/Images/DeleteRow.png" Height="16px" Width="16px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlCurrentDetails" runat="server" Visible="false" BorderStyle="Solid"
                                            BorderWidth="1px" BorderColor="SkyBlue" Height="440px" ScrollBars="Auto">
                                            <asp:GridView ID="gvPrevious" runat="server" Width="100%" AllowPaging="false" SkinID="gridview"
                                                AutoGenerateColumns="False" OnRowDataBound="gvPrevious_OnRowDataBound"
                                                OnRowCommand="gvPrevious_OnRowCommand">
                                                <%--OnRowCreated="gvPrevious_OnRowCreated" --%>
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                                    ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    FooterStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndentNo" runat="server" SkinID="label" Text='<%# Eval("IndentNo") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndentDate" runat="server" SkinID="label" Text='<%# Eval("IndentDate") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Current Drug(s)">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkItemName" runat="server" ToolTip="Click here to add new prescription"
                                                                CausesValidation="false" CommandName='selectItem' CommandArgument='<%#Eval("ItemId")%>'
                                                                Text='<%#Eval("ItemName")%>' Font-Size="Smaller" />
                                                            <asp:Label ID="lblGenericName" runat="server" Style="display: none" Text='<%#Eval("GenericName")%>' />
                                                            <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                            <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                                            <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                                            <%--<asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />--%>
                                                            <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                            <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                            <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                            <%-- <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%#Eval("FrequencyId") %>' />--%>
                                                            <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                                            <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                                            <asp:HiddenField ID="hdnStrengthValue" runat="server" Value='<%#Eval("StrengthValue")%>' />
                                                            <asp:HiddenField ID="hdnXMLData" runat="server" />
                                                            <asp:HiddenField ID="hdnDose" runat="server" Value='<%#Eval("Dose") %>' />
                                                            <asp:HiddenField ID="hdnDays" runat="server" Value='<%#Eval("Days") %>' />
                                                            <asp:HiddenField ID="hdnIsInfusion" runat="server" Value='<%#Eval("IsInfusion") %>' />
                                                            <asp:HiddenField ID="hdnIndentDetailsId" runat="server" Value='<%#Eval("DetailsId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Indent Type" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndentType" runat="server" SkinID="label" Text='<%# Eval("IndentType") %>' />
                                                            <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="40px"
                                                                SkinID="label" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Prescription Detail" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>'
                                                                SkinID="label" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStartDate" runat="server" SkinID="label" Text='<%# Eval("StartDate") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEndDate" runat="server" SkinID="label" Text='<%# Eval("EndDate") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label41" runat="server" Text="BD" ToolTip="Brand Details" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnBrandDetailsCIMS" runat="server" ToolTip="Click here to view cims brand details"
                                                                CommandName="BrandDetailsCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                                Text="&nbsp;" Width="100%" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label42" runat="server" Text="MG" ToolTip="Monograph" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkBtnMonographCIMS" runat="server"
                                                                ImageUrl="~/CIMSDatabase/monograph.png" Width="18px" Height="18px"
                                                                CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                                ToolTip="Click here to view cims monograph" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label43" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkBtnInteractionCIMS" runat="server"
                                                                ImageUrl="~/CIMSDatabase/interaction.png" Width="18px" Height="18px"
                                                                CommandName="InteractionCIMS" CausesValidation="false"
                                                                ToolTip="Click here to view cims drug interaction" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label44" runat="server" Text="DH" ToolTip="Drug Health Interaction" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                                                CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                                                Text="&nbsp;" Width="100%" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label45" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnDAInteractionCIMS" runat="server" ToolTip="Click here to view cims drug allergy interaction"
                                                                CommandName="DAInteractionCIMS" CausesValidation="false" BackColor="#82CAFA"
                                                                Text="&nbsp;" Width="100%" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label46" runat="server" Text="BD" ToolTip="Brand Details" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnBrandDetailsVIDAL" runat="server" ToolTip="Click here to view vidal brand details"
                                                                CommandName="BrandDetailsVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                                Text="&nbsp;" Width="100%" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label47" runat="server" Text="MG" ToolTip="Monograph" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                                                CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                                Text="&nbsp;" Width="100%" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label48" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                                                CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                                Width="100%" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label49" runat="server" Text="DH" ToolTip="Drug Health Interaction" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                                                CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                                                Text="&nbsp;" Width="100%" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label50" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnDAInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug allergy interaction"
                                                                CommandName="DAInteractionVIDAL" CausesValidation="false" BackColor="#82CAFA"
                                                                Text="&nbsp;" Width="100%" Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stop Remarks" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" Height="20px" Width="100%"
                                                                TextMode="MultiLine" Text="" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" HeaderText="Stop">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnStop" runat="server" ToolTip="Click here to stop this drug"
                                                                CommandName="ItemStop" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                                ImageUrl="~/Images/Close.png" Height="16px" Width="16px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" HeaderText="Print">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkBtnPrint" runat="server" ToolTip="Click here to print prescription"
                                                                CommandName="PRINT" CausesValidation="false" CommandArgument='<%#Eval("IndentId")%>'
                                                                AlternateText="Print" ImageUrl="~/Images/editor/print.gif" Height="16px" Width="16px" Font-Bold="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlOrderSet" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="1px"
                                            BorderColor="SkyBlue" Height="440px" ScrollBars="Auto">
                                            <asp:GridView ID="gvOrderSet" runat="server" Width="100%" AllowPaging="false" SkinID="gridview"
                                                AutoGenerateColumns="False" OnRowCommand="gvOrderSet_OnRowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Order Set(s)">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkSetName" runat="server" ToolTip="Click here to add new order set"
                                                                CausesValidation="false" CommandName='SelectOrderSet' CommandArgument='<%#Eval("SetId")%>'
                                                                Text='<%#Eval("SetName")%>' Font-Size="Smaller" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>

                                    </div>


                                </div>
                                <table border="0" cellpadding="0" cellspacing="1" width="100%" class="form-group">

                                    <tr id="trDrugs" runat="server" class="hidden">
                                        <td colspan="2">Dose
                                        </td>
                                        <td colspan="2">Frequency
                                        </td>
                                        <td colspan="2">Duration
                                        </td>
                                        <td colspan="2">Food Relation
                                        </td>
                                    </tr>
                                    <div class="hidden">
                                        <asp:Button ID="btnRemoveItem" runat="server" CssClass="btn btn-primary" Text="Remove Item"
                                            OnClick="btnRemoveItem_OnClick" Font-Size="Smaller" />
                                    </div>
                                    <tr>
                                        <td>
                                            <%--  <asp:Label ID="Label4" runat="server" SkinID="label" Text="Frequency" Font-Size="Smaller" /><span
                                            style="color: Red">*</span>--%>
                                        </td>

                                        <td>
                                            <asp:LinkButton ID="lbtnFrequencyTime" runat="server" OnClick="lbtnFrequencyTime_OnClick"
                                                SkinID="label" Text="Dosage Time" Font-Size="Smaller" Style="display: none" />
                                            <br />
                                            <asp:LinkButton ID="lnkInsulinDose" runat="server" OnClick="lnkInsulinDose_Click"
                                                SkinID="label" Text="Insuline Dose" Font-Size="Smaller" Style="display: none" />

                                        </td>
                                    </tr>
                                    <tr id="trCustomMedication" runat="server" visible="false">
                                        <td style="width: 85px" valign="top">
                                            <asp:Label ID="Label23" runat="server" Text="Custom Medication" SkinID="label" />
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtCustomMedication" runat="server" SkinID="textbox" MaxLength="1000"
                                                TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 1000);" Style="min-height: 44px; max-height: 44px; min-width: 300px; max-width: 300px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--  <asp:Label ID="Label6" runat="server" SkinID="label" Text="Duration" Font-Size="Smaller" /><span
                                            style="color: Red">*</span>--%>
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="2">
                                                <tr>
                                                </tr>
                                            </table>
                                        </td>
                                        <td colspan="2">
                                            <table width="100%" style="display: none">
                                                <tr id="trDiagnosis" runat="server">
                                                    <td style="width: 60%">
                                                        <asp:Label ID="label64" runat="server" SkinID="label" Text="Diagnosis" />
                                                        <span style="color: Red; font-weight: bold;">*</span>
                                                    </td>
                                                    <td style="width: 40%">
                                                        <asp:UpdatePanel ID="indicationsel" runat="server">
                                                            <ContentTemplate>
                                                                <input id="hdnICDCode" value='<%# Eval("ICDCodes")%>' type="hidden" runat="server" />
                                                                <input id="hdnExitOrNot" value='<%# Eval("ExitOrNot")%>' type="hidden" runat="server" />
                                                                <asp:TextBox ID="txtICDCode" runat="server" SkinID="textbox" Wrap="true" MaxLength="200"
                                                                    Visible="true" Width="100px" AutoPostBack="True" OnTextChanged="txtICDCode_TextChanged" />
                                                                <asp:Panel ID="pnlICDCodes" BorderStyle="Solid" BorderWidth="1px" Style="visibility: hidden; position: relative;"
                                                                    BackColor="#E0EBFD" runat="server" Height="100px" ScrollBars="Auto"
                                                                    Width="400px">
                                                                    <asp:UpdatePanel ID="update" runat="server">
                                                                        <ContentTemplate>
                                                                            <aspl:ICD ID="icd" runat="server" width="400px" PanelName="ctl00_ContentPlaceHolder1_pnlICDCodes"
                                                                                ICDTextBox="ctl00_ContentPlaceHolder1_txtICDCode" />
                                                                            <asp:HiddenField ID="hdnGridClientId" runat="server" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </asp:Panel>
                                                                <AJAX:PopupControlExtender ID="PopUnit" runat="server" TargetControlID="txtICDCode"
                                                                    PopupControlID="pnlICDCodes" Position="Right" OffsetX="5" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>



                                </table>
                            </div>

                        </div>
                        <div class="col-md-6">
                            <div class="container-fluid form-group">
                                <div class="row">

                                    <div class="col-md-12" style="background: #337ab7; font-family: 'Open Sans', sans-serif !important; padding: 5px 10px;">
                                        <div style="float: left">
                                            <asp:Label ID="lblPrescribedBy" runat="server" Text="Prescribed By:" ForeColor="White" />
                                            <asp:Label ID="lblPrescribedByValue" runat="server" ForeColor="White" Font-Bold="true" />

                                        </div>
                                        <telerik:RadComboBox ID="ddlAdvisingDoctor" runat="server" EmptyMessage="[ Select ]"
                                            Width="130px" Filter="Contains" DropDownWidth="300px" Style="display: none" />

                                        <asp:Label ID="Label7" runat="server" Text="Store" SkinID="label" Style="display: none" />

                                        <telerik:RadComboBox ID="ddlStore" SkinID="DropDown" runat="server" Width="200px"
                                            Filter="Contains" DropDownWidth="200px" EmptyMessage="[ Select ]" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="pull-right prescription-section" />

                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col-md-12" style="background: white; padding: 10px; height: 5px;">
                                        <div class="col-md-9">
                                            <asp:Label ID="lblGenericName" runat="server" SkinID="label" Text="" Font-Bold="true"
                                                Font-Size="11px" ForeColor="#990066" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:LinkButton ID="lnkClearAll" runat="server" Text="Clear All" OnClick="lnkClearAll_Click" CssClass="pull-right"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="Panel3" runat="server" class="custom-scroller custom-scroller-light"  style="overflow-y: auto; max-height:180px;">
                                <asp:GridView ID="gvItem" runat="server" Width="100%" AllowPaging="false" SkinID="gridview"
                                    AutoGenerateColumns="False" OnRowCreated="gvItem_OnRowCreated" OnRowDataBound="gvItem_OnRowDataBound"
                                    OnRowCommand="gvItem_OnRowCommand" >
                                    <Columns>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                                <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                                <asp:HiddenField ID="hdnGenericName" runat="server" Value='<%# Eval("GenericName") %>' />
                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                                <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                                <%--<asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />--%>
                                                <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                <asp:HiddenField ID="hdnIsInfusion" runat="server" Value='<%#Eval("IsInfusion") %>' />
                                                <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                                <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                                <asp:HiddenField ID="hdnStrengthValue" runat="server" Value='<%#Eval("StrengthValue")%>' />
                                                <asp:HiddenField ID="hdnXMLData" runat="server" Value='<%#Eval("XMLData") %>' />
                                                <asp:HiddenField ID="hdnCustomMedication" runat="server" Value='<%#Eval("CustomMedication") %>' />
                                                <asp:HiddenField ID="hdnNotToPharmcy" runat="server" Value='<%#Eval("NotToPharmacy") %>' />
                                                <asp:HiddenField ID="hdnStartDate" runat="server" Value='<%# Eval("StartDate") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugAllergy" runat="server" Value='<%# Eval("OverrideComments") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugToDrug" runat="server" Value='<%# Eval("OverrideCommentsDrugToDrug") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugHealth" runat="server" Value='<%# Eval("OverrideCommentsDrugHealth") %>' />
                                                <asp:HiddenField ID="hdnUnAppPrescriptionId" runat="server" Value='<%#Eval("UnAppPrescriptionId") %>' />
                                                <%--<asp:HiddenField ID="hdnCustomId" runat="server" Value='<%#Eval("Id") %>'/>
                                             <asp:HiddenField ID="hdnVolume" runat="server" Value='<%#Eval("Volume") %>' />
                                             <asp:HiddenField ID="hdnVolumeUnitId" runat="server" Value='<%#Eval("VolumeUnitId") %>' />
                                             <asp:HiddenField ID="hdnInfusionTime" runat="server" Value='<%#Eval("InfusionTime") %>' />
                                             <asp:HiddenField ID="hdnTimeUnit" runat="server" Value='<%#Eval("TimeUnit") %>' />
                                             <asp:HiddenField ID="hdnTotalVolume" runat="server" Value='<%#Eval("TotalVolume") %>' />
                                             <asp:HiddenField ID="hdnFlowRateUnit" runat="server" Value='<%#Eval("FlowRateUnit") %>' />--%>
                                                <asp:HiddenField ID="hdnFrequency" runat="server" Value='<%#Eval("Frequency") %>' />
                                                <asp:HiddenField ID="hdnXmlVariableDose" runat="server" Value='<%#Eval("XmlVariableDose") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Drug Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" SkinID="label" Text='<%# Eval("ItemName") %>'
                                                    Width="100%" Font-Size="Smaller" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="60px" ItemStyle-Width="60px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartDate" runat="server" SkinID="label" Text='<%# Eval("StartDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="50px" ItemStyle-Width="50px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndDate" runat="server" SkinID="label" Text='<%# Eval("EndDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Indent Type" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentType" runat="server" SkinID="label" Text='<%# Eval("IndentType") %>' />
                                                <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotalQty" runat="server" SkinID="textbox" Width="100%" MaxLength="3"
                                                    Text='<%#Eval("Qty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prescription Detail" HeaderStyle-Width="150px" ItemStyle-Width="530px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGenericNameDetail" runat="server" Font-Bold="true" Text='<%#Eval("GenericName")%>' Font-Size="Smaller" />
                                                <asp:Label ID="lblItemNameDetail" Font-Bold="true" runat="server" SkinID="label" Text='<%# Eval("ItemName") %>'
                                                    Width="100%" Font-Size="Smaller" />
                                                <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>'
                                                    SkinID="label" Font-Size="Smaller" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label52" runat="server" Text="MG" ToolTip="Monograph" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkBtnMonographCIMS" runat="server"
                                                    ImageUrl="~/CIMSDatabase/monograph.png" Width="18px" Height="18px"
                                                    CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                    ToolTip="Click here to view cims monograph" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label53" runat="server" Text="DI" ToolTip="Drug Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkBtnInteractionCIMS" runat="server"
                                                    ImageUrl="~/CIMSDatabase/interaction.png" Width="18px" Height="18px"
                                                    CommandName="InteractionCIMS" CausesValidation="false"
                                                    ToolTip="Click here to view cims drug interaction" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="13px" HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                    CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                    ImageUrl="~/Images/DeletePresc.png" Width="16px" Height="16px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="13px" HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnEdit" runat="server" CommandName="Select" CausesValidation="false"
                                                    CommandArgument='<%#Eval("ItemId")%>' ImageUrl="~/Images/EditPresc.png" Width="16px"
                                                    Height="16px" ToolTip="Click here to edit this record" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label51" runat="server" Text="BD" ToolTip="Brand Details" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnBrandDetailsCIMS" runat="server" ToolTip="Click here to view cims brand details"
                                                    CommandName="BrandDetailsCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label52" runat="server" Text="MG" ToolTip="Monograph" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                                    CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label53" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                                    CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                    Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label54" runat="server" Text="DH" ToolTip="Drug Health Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                                    CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label55" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDAInteractionCIMS" runat="server" ToolTip="Click here to view cims drug allergy interaction"
                                                    CommandName="DAInteractionCIMS" CausesValidation="false" BackColor="#82CAFA"
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label56" runat="server" Text="BD" ToolTip="Brand Details" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnBrandDetailsVIDAL" runat="server" ToolTip="Click here to view vidal brand details"
                                                    CommandName="BrandDetailsVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label57" runat="server" Text="MG" ToolTip="Monograph" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                                    CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label58" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                                    CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                    Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label59" runat="server" Text="DH" ToolTip="Drug Health Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                                    CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label60" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDAInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug allergy interaction"
                                                    CommandName="DAInteractionVIDAL" CausesValidation="false" BackColor="#82CAFA"
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>


                            </asp:Panel>
                            <br />
                            <div class="col-md-12 text-center">
                                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary  btn-xs" Text="Print (F9)" OnClick="btnPrint_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnSave" runat="server" CausesValidation="false" Text="Generate Rx (F3)" CssClass="btn btn-primary btn-xs" OnClick="btnSave_Onclick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />

                            </div>
                            <div class="col-md-12 text-center">
                                <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" Font-Size="Small" />
                                <asp:Label ID="lblERXNo" Font-Bold="true" BackColor="Gray" ForeColor="White" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>


                    <table border="0" cellpadding="1" cellspacing="0" width="100%">

                        <tr>
                            <td valign="top" style="display: none">
                                <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                    <tr>
                                        <td valign="top" style="width: 140px">
                                            <asp:Label ID="Label19" runat="server" SkinID="label" Text="Prov.&nbsp;Diagnosis" Style="display: none" />
                                        </td>
                                        <td colspan="2" valign="top">
                                            <%--<asp:TextBox ID="txtProvisionalDiagnosis" runat="server" SkinID="textbox" Height="21px"
                                        Width="99%" TextMode="MultiLine" ReadOnly="true" />--%>
                                            <asp:TextBox ID="txtProvisionalDiagnosis" runat="server" SkinID="textbox" TextMode="MultiLine"
                                                ReadOnly="true" Style="min-height: 21px; max-height: 21px; min-width: 250px; max-width: 250px; visibility: hidden;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label30" runat="server" SkinID="label" Text="Medication&nbsp;List" Style="display: none" />
                                        </td>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:RadioButtonList ID="rdoSearchMedication" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="rdoSearchMedication_OnSelectedIndexChanged" Style="display: none">
                                                        <asp:ListItem Text="Favourite" Value="F" Selected="True" />
                                                        <asp:ListItem Text="Current" Value="C" />
                                                        <asp:ListItem Text="Order Set" Value="OS" />
                                                        <%-- <asp:ListItem Text="Previous" Value="P" />--%>
                                                    </asp:RadioButtonList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top"></td>












                            <td valign="top" align="right">
                                <%--<asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="1px" BorderColor="SkyBlue"
                                Height="441px" ScrollBars="Auto">--%>
                            
                            </td>
                            <td valign="top" style="display: none">
                                <%--<table border="1" cellpadding="0" cellspacing="1" width="100%">
                            <tr>
                                <td style="height: 22px">
                                    <asp:Label ID="Label11" runat="server" Text="Height&nbsp;(Cm) : " SkinID="label"
                                        Font-Bold="true" />
                                    <asp:Label ID="txtHeight" runat="server" SkinID="label" Font-Bold="true" BackColor="Aqua" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="&nbsp;Weight&nbsp;(Kg) : " SkinID="label"
                                        Font-Bold="true" />
                                    <asp:Label ID="lbl_Weight" runat="server" SkinID="label" Font-Bold="true" BackColor="Aqua" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 22px">
                                    <asp:Button ID="btnAddtoFav" runat="server" cssClass="btn btn-primary" Text="Add To Favourite"
                                        ToolTip="Add To Favourite" OnClick="btnAddtoFav_Click" />
                                </td>
                            </tr>
                        </table>--%>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="display: none">
                                        <td style="height: 22px">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="height: 22px">
                                                        <asp:Label ID="Label11" runat="server" Text="Height&nbsp;(Cm) : " SkinID="label"
                                                            Font-Bold="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="txtHeight" runat="server" SkinID="label" Font-Bold="true" BackColor="Aqua" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label12" runat="server" Text="&nbsp;Weight&nbsp;(Kg) : " SkinID="label"
                                                            Font-Bold="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Weight" runat="server" SkinID="label" Font-Bold="true" BackColor="Aqua" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="ChkAutoSuggestion" Text="Auto Suggession" runat="server" AutoPostBack="true" OnCheckedChanged="cbEnableSuggession_CheckedChanged" />
                                            &nbsp;<asp:LinkButton ID="lbtnShowSuggession" runat="server" Text="Show Suggession" OnClick="lbtnShowSuggession_Click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 22px">
                                            <%--<table cellpadding="2" cellspacing="0">
                                                <tr>

                                                    <td>
                                                        <asp:Button ID="btnBrandDetailsViewOnItemBased" CssClass="btn btn-primary" runat="server" Text="View Brand Details"
                                                            Visible="false" OnClick="btnBrandDetailsView_OnClick" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnMonographViewOnItemBased" CssClass="btn btn-primary" runat="server" Text="View Monograph"
                                                            Visible="false" OnClick="btnMonographView_OnClick" />
                                                    </td>
                                                </tr>
                                            </table>--%>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="tblFavouriteSearch" runat="server" border="0" cellpadding="0" cellspacing="1"
                                    width="100%">
                                    <tr>
                                        <td style="width: 140px">
                                            <asp:Label ID="Label24" runat="server" SkinID="label" Text="Search&nbsp;Drug&nbsp;Name" Style="display: none" />
                                        </td>
                                        <td>
                                            <asp:Panel ID="Panel6" runat="server" DefaultButton="btnSearchFavourite">
                                            </asp:Panel>
                                        </td>
                                        <%--<td style="width: 70px">
                                    <asp:CheckBox ID="chkRemoveFavourite" runat="server" SkinID="checkbox" Text="Remove"
                                        AutoPostBack="true" OnCheckedChanged="chkRemoveFavourite_OnCheckedChanged" />
                                </td>--%>
                                        <td style="width: 22px" align="right">
                                            <asp:ImageButton ID="btnProceedFavourite" runat="server" ToolTip="Click here to proceed selected favourite"
                                                ImageUrl="~/Images/Login/orrange-arrow.GIF" Width="18px" OnClick="btnProceedFavourite_OnClick" Style="display: none" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblCurrentSearch" runat="server" visible="false" border="0" cellpadding="0"
                                    cellspacing="1" width="100%">
                                    <tr>
                                        <td style="width: 140px">
                                            <asp:Label ID="Label27" runat="server" SkinID="label" Text="Search Drug Name" Style="display: none" />
                                        </td>
                                        <td></td>
                                        <td style="width: 22px" align="right">
                                            <asp:ImageButton ID="btnProceedCurrent" runat="server" ToolTip="Click here to proceed selected current medication"
                                                ImageUrl="~/Images/Login/orrange-arrow.GIF" Width="18px" OnClick="btnProceedCurrent_OnClick" Style="display: none" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblOrderSetSearch" runat="server" visible="false" border="0" cellpadding="0"
                                    cellspacing="1" width="100%" style="visibility: hidden; display: none;">
                                    <tr>
                                        <td style="width: 140px">
                                            <asp:Label ID="Label68" runat="server" SkinID="label" Text="Search Order Set" Style="visibility: hidden;" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 112px">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkCustomMedication" runat="server" AutoPostBack="true" Visible="false" OnCheckedChanged="chkCustomMedication_OnCheckedChanged"
                                                        Font-Size="10px" SkinID="checkbox" Text="Custom&nbsp;Medication" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right"></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" style="display: none">
                                    <tr>
                                        <td>
                                            <%--<asp:Label ID="lblPrescription" runat="server" SkinID="label" Text="Prescription"
                                        Font-Size="12px" Font-Bold="true" />--%>
                                            <asp:LinkButton ID="lnkStopMedication" runat="server" SkinID="label" Font-Size="10px"
                                                OnClick="lnkStopMedication_OnClick" Text="Stopped Medications" ToolTip="Click to see stoped medication" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCopyLastPrescription" runat="server" CssClass="btn btn-primary" Font-Size="9px"
                                                Text="Copy Last Prescription" OnClick="btnCopyLastPrescription_Click" />
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:LinkButton ID="lnkDrugAllergy" runat="server" BackColor="#82CAFA" Font-Size="10px"
                                                Text="Drug&nbsp;Allergy" ToolTip="Drug Allergy" Font-Bold="true" OnClick="lnkDrugAllergy_OnClick"
                                                Visible="false" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkShowDetails" runat="server" Font-Size="10px" Text="Show Details"
                                                AutoPostBack="true" OnCheckedChanged="chkShowDetails_OnCheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>


                        <tr>
                        </tr>
                    </table>

                </div>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div id="dvPharmacistInstruction" runat="server" visible="false" style="width: 520px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 260px; left: 400px; top: 250px">
                                <table width="100%" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label39" runat="server" SkinID="label" Font-Size="12px" Font-Bold="true"
                                                Text="Instruction For Patient" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txtPharmacistInstruction" runat="server" Font-Size="12px" Font-Bold="true"
                                                ForeColor="Navy" TextMode="MultiLine" Style="min-height: 200px; max-height: 200px; min-width: 510px; max-width: 510px;"
                                                ReadOnly="true" BackColor="#FFFFCC" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnPharmacistInstructionClose" CssClass="btn btn-primary" runat="server" Text="Close"
                                                OnClick="btnPharmacistInstructionClose_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div id="dvInteraction" runat="server" visible="false" style="width: 700px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 400px; left: 320px; top: 120px">
                                <table width="100%" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label25" runat="server" SkinID="label" Font-Size="11px" Font-Bold="true"
                                                ForeColor="Red" Text="INTERACTION ALERTS FOUND!" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <%--<asp:Label ID="lblInteractionBetweenMessage" runat="server" SkinID="label" Font-Size="10px" Font-Bold="true" ForeColor="Maroon"
                                            Text="This drug has interaction with prescribed medicines !" />--%>
                                            <asp:TextBox ID="txtInteractionBetweenMessage" runat="server" Font-Size="10px" Font-Bold="true"
                                                ForeColor="Maroon" Text="This drug has interaction with prescribed medicines !"
                                                TextMode="MultiLine" Height="200px" Style="min-width: 690px; max-width: 690px;"
                                                ReadOnly="true" BackColor="#FFFFCC" />
                                            <%--min-height: 56px; max-height: 56px;--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table cellpadding="3" cellspacing="0">
                                                <tr>
                                                    <%--<td>
                                                        <asp:Button ID="btnBrandDetailsView" CssClass="btn btn-primary" runat="server" Text="View Brand Details"
                                                            Width="150px" OnClick="btnBrandDetailsView_OnClick" />
                                                    </td>--%>
                                                    <td>
                                                        <asp:Button ID="btnMonographView" CssClass="btn btn-primary" runat="server" Text="View Monograph"
                                                            Width="150px" OnClick="btnMonographView_OnClick" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnInteractionView" CssClass="btn btn-primary" runat="server" Text="View Drug Interaction(s)"
                                                            Width="170px" OnClick="btnInteractionView_OnClick" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblIntreactionMessage" runat="server" SkinID="label" ForeColor="Red" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table cellpadding="0" cellspacing="1">
                                                <%--<tr>
                                                    <td>
                                                        <asp:Label ID="Label26" runat="server" Text="Reason to continue for Drug Allergy Interaction"
                                                            ForeColor="Gray" />
                                                        <span id="spnCommentsDrugAllergy" runat="server" style="color: Red; font-size: large;"
                                                            visible="false">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtCommentsDrugAllergy" runat="server" SkinID="textbox" MaxLength="500"
                                                            TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" />
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label37" runat="server" Text="Reason to continue for Interaction alert"
                                                            ForeColor="Gray" />
                                                        <span id="spnCommentsDrugToDrug" runat="server" style="color: Red; font-size: large;"
                                                            visible="false">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtCommentsDrugToDrug" runat="server" SkinID="textbox" MaxLength="500"
                                                            TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" />
                                                    </td>
                                                </tr>
                                                <%--<tr id="Tr2" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="Label38" runat="server" Text="Reason to continue for Drug Health Interaction"
                                                            ForeColor="Gray" />
                                                        <span id="spnCommentsDrugHealth" runat="server" style="color: Red; font-size: large;"
                                                            visible="false">*</span>
                                                    </td>
                                                </tr>
                                                <tr id="Tr3" runat="server" visible="false">
                                                    <td>
                                                        <asp:TextBox ID="txtCommentsDrugHealth" runat="server" SkinID="textbox" MaxLength="500"
                                                            TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" />
                                                    </td>
                                                </tr>--%>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnInteractionContinue" CssClass="btn btn-primary" runat="server" Text="Continue"
                                                Width="150px" OnClick="btnInteractionContinue_OnClick" />
                                            &nbsp;
                                        <asp:Button ID="btnInteractionCancel" CssClass="btn btn-primary" runat="server" Text="Cancel"
                                            Width="150px" OnClick="btnInteractionCancel_OnClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="middle">
                                            <table cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td valign="middle">
                                                        <asp:Image ID="Image1" ImageUrl="~/CIMSDatabase/CIMSLogo.PNG" Height="30px" Width="120px"
                                                            runat="server" />
                                                    </td>
                                                    <td valign="bottom">
                                                        <asp:Label ID="Label40" runat="server" SkinID="label" Font-Size="14px" Font-Bold="true"
                                                            ForeColor="Red" Text="(Powered by CIMS. Copyright MIMS Pte Ltd. All rights reserved.)" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td>
                            <div id="dvConfirmStop" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 100px; left: 520px; top: 220px">
                                <table width="100%" cellspacing="2" cellpadding="0">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="Label31" Font-Size="12px" runat="server" Font-Bold="true" Text="Stop Medication Remarks" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtStopRemarks" SkinID="textbox" runat="server" TextMode="MultiLine"
                                                Style="min-height: 45px; max-height: 45px; min-width: 390px; max-width: 390px;"
                                                MaxLength="200" onkeyup="return MaxLenTxt(this, );" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"></td>
                                        <td align="center">
                                            <asp:Button ID="btnStopMedication" CssClass="btn btn-primary" runat="server" Text="Stop" OnClick="btnStopMedication_OnClick" />
                                            &nbsp;
                                        <asp:Button ID="btnStopClose" CssClass="btn btn-primary" runat="server" Text="Close" OnClick="btnStopClose_OnClick" />
                                        </td>
                                        <td align="center"></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <%-- <table border="0" cellpadding="0" cellspacing="2">
                <tr>
                    <td>
                        <div id="Div1" runat="server" visible="false" style="width: 400px; z-index: 200;
                            border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
                            border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute;
                            bottom: 0; height: 100px; left: 520px; top: 220px">
                            <table width="100%" cellspacing="2" cellpadding="0">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="Label26" Font-Size="12px" runat="server" Font-Bold="true" Text="Stop Medication Remarks" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="TextBox1" SkinID="textbox" runat="server" TextMode="MultiLine"
                                            Style="min-height: 45px; max-height: 45px; min-width: 390px; max-width: 390px;"
                                            MaxLength="200" onkeyup="return MaxLenTxt(this, );" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="Button1" cssClass="btn btn-primary" runat="server" Text="Stop" OnClick="btnStopMedication_OnClick" />
                                        &nbsp;
                                        <asp:Button ID="Button2" cssClass="btn btn-primary" runat="server" Text="Close" OnClick="btnStopClose_OnClick" />
                                    </td>
                                    <td align="center">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>--%>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdnCIMSOutput" runat="server" />
                            <asp:HiddenField ID="hdnReturnIndentOPIPSource" runat="server" />
                            <asp:HiddenField ID="hdnReturnIndentDetailsIds" runat="server" />
                            <asp:HiddenField ID="hdnReturnIndentIds" runat="server" />
                            <asp:HiddenField ID="hdnReturnItemIds" runat="server" />
                            <asp:HiddenField ID="hdnStoreId" runat="server" />
                            <asp:HiddenField ID="hdnGenericId" runat="server" />
                            <asp:HiddenField ID="hdnGenericName" runat="server" />
                            <asp:HiddenField ID="hdnItemId" runat="server" />
                            <asp:HiddenField ID="hdnItemName" runat="server" />
                            <asp:HiddenField ID="hdnCIMSItemId" runat="server" />
                            <asp:HiddenField ID="hdnCIMSType" runat="server" />
                            <asp:HiddenField ID="hdnCIMSItemIdClick" runat="server" />
                            <asp:HiddenField ID="hdnCIMSTypeClick" runat="server" />
                            <asp:HiddenField ID="hdnVIDALItemId" runat="server" />
                            <asp:HiddenField ID="hdnTotalQty" runat="server" />
                            <asp:HiddenField ID="hdnInfusion" runat="server" />
                            <asp:HiddenField ID="hdnIsInjection" runat="server" />
                            <asp:HiddenField ID="hdnFavouriteIndex" runat="server" />
                            <asp:HiddenField ID="hdnFavouriteItemId" runat="server" />
                            <asp:HiddenField ID="hdnFavoriteId" runat="server" />
                            <asp:HiddenField ID="hdnFavouriteGenericId" runat="server" />
                            <asp:HiddenField ID="hdnFavouriteCommand" runat="server" />
                            <asp:HiddenField ID="hdnCommandType" runat="server" />

                            <asp:HiddenField ID="hdnDateWise" runat="server" />

                            <asp:HiddenField ID="hdnNoOfDose" runat="server" />
                            <asp:HiddenField ID="hdnXmlDoseString" runat="server" />


                            <%--   <asp:HiddenField ID="hdnGenericIdAddToFav" runat="server" />--%>
                            <asp:Button ID="btnMonographViewClick" runat="server" Style="visibility: hidden;" OnClick="btnMonographViewClick_OnClick" />

                            <asp:Button ID="btnFavoriteRowCommand" runat="server" Text="" CausesValidation="false" SkinID="button"
                                Style="visibility: hidden;" OnClick="btnFavoriteRowCommand_Click" />

                            <asp:Button ID="btnSearchFavourite" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                Width="1px" OnClick="btnSearchFavourite_OnClick" />
                            <asp:Button ID="btnSearchCurrent" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                Width="1px" OnClick="btnSearchCurrent_OnClick" />
                            <asp:Button ID="btnSearchOrderSet" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                Width="1px" OnClick="btnSearchOrderSet_OnClick" />
                            <asp:Button ID="btnGetInfo" runat="server" Text="" CausesValidation="false" CssClass="btn btn-primary"
                                Style="visibility: hidden;" OnClick="btnGetInfo_Click" />
                            <asp:Button ID="btnGetInfoGeneric" runat="server" Text="" CausesValidation="false"
                                CssClass="btn btn-primary" Style="visibility: hidden;" OnClick="btnGetInfoGeneric_Click" />
                            <asp:Button ID="btnRefresh" runat="server" Style="visibility: hidden;" OnClick="btnRefresh_OnClick" />
                            <asp:Button ID="btnGetFavourite" runat="server" Style="visibility: hidden;" OnClick="btnGetFavourite_OnClick" />

                            <asp:Button ID="btnVariableDoseClose" runat="server" Style="visibility: hidden;" OnClick="btnVariableDoseClose_OnClick" />

                            <asp:Button ID="btnAddAIMedicineList" runat="server" Style="visibility: hidden;" OnClick="btnAddAIMedicineList_Click" />
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" KeepInScreenBounds="true" Skin="Metro" Title="Previous Medications" AutoSizeBehaviors="HeightProportional">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Maximize,Minimize" KeepInScreenBounds="true" />
                                </Windows>
                            </telerik:RadWindowManager>
                            <asp:HiddenField ID="hdnControlId" runat="server" />
                            <asp:HiddenField ID="hdnControlType" runat="server" Value="M" />
                            <asp:HiddenField ID="hdnTemplateFieldId" runat="server" />
                            <asp:HiddenField ID="hdnCtrlValue" runat="server" />
                            <asp:HiddenField ID="hdnXmlVariableDoseString" runat="server" Value="" />
                            <asp:HiddenField ID="hdnvariableDoseDuration" runat="server" Value="" />
                            <asp:HiddenField ID="hdnvariableDoseFrequency" runat="server" Value="" />
                            <asp:HiddenField ID="hdnVariabledose" runat="server" Value="" />
                            <asp:HiddenField ID="hdnXmlFrequencyTime" runat="server" Value="" />
                            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                            <asp:HiddenField ID="hdnFrequencyIdAttributes" runat="server" />
                            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                                Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                            <asp:Button ID="btnCalc" runat="server" Style="visibility: hidden;" OnClick="btnCalc_OnClick" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td>
                            <div id="dvConfirmPrint" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 75px; left: 410px; top: 240px">
                                <table width="100%" cellspacing="2" cellpadding="0">
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to print the prescription ?" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"></td>
                                        <td align="center">
                                            <asp:Button ID="btnPrintYes" CssClass="btn btn-primary" runat="server" Text="Yes" Width="80px"
                                                OnClick="btnPrintYes_OnClick" />
                                            &nbsp;&nbsp;
                                        <asp:Button ID="btnPrintNo" CssClass="btn btn-primary" runat="server" Text="No" Width="80px"
                                            OnClick="btnPrintNo_OnClick" />
                                        </td>
                                        <td align="center"></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="dvConfirmAlreadyExistOptions" runat="server" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 150px; left: 270px; top: 200px;">
                    <table cellspacing="2" cellpadding="2" width="400px">
                        <tr>
                            <td style="width: 30%; text-align: left;">
                                <asp:Label ID="lblSn" runat="server" Text="Drug Name :" ForeColor="#990066" />
                            </td>
                            <td style="width: 70%; text-align: left;">
                                <asp:Label ID="lblItemName" runat="server" ForeColor="#990066" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%; text-align: left;">
                                <asp:Label ID="Label62" runat="server" Text="Already Order By :" ForeColor="#990066" />
                            </td>
                            <td style="width: 70%; text-align: left;">
                                <asp:Label ID="lblEnteredBy" runat="server" ForeColor="#990066" />
                            </td>
                        </tr>
                        <tr style="border-bottom-style: solid; border-bottom-width: 1px;">
                            <td style="width: 30%; text-align: left;">
                                <asp:Label ID="Label63" runat="server" Text="Order date :" ForeColor="#990066" />
                            </td>
                            <td style="width: 70%; text-align: left;">
                                <asp:Label ID="lblEnteredOn" runat="server" ForeColor="#990066" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 100%; text-align: center;">
                                <asp:Label ID="lblAlertMsg" runat="server" Font-Size="12px" Text="Do you wish to continue...?"
                                    ForeColor="#990066" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 100%; text-align: center;">
                                <asp:Button ID="btnAlredyExistProceed" runat="server" Text="Proceed(F2)" OnClick="btnAlredyExistProceed_OnClick"
                                    CssClass="btn btn-primary" />
                                &nbsp;&nbsp;
                            <asp:Button ID="btnAlredyExistCancel" runat="server" Text="Cancel" OnClick="btnAlredyExistCancel_OnClick"
                                CssClass="btn btn-primary" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="2000" DynamicLayout="true">
        <ProgressTemplate>
            <center>
                <div style="width: 154px; position: absolute; bottom: 0; height: 60px; left: 500px; top: 300px">
                    <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div id="dverx" runat="server" style="position: absolute; top: 20%; width: 100%; opacity: 0.9; background-color: Gray; height: 200px;"
        visible="false">
        <b>There is Validation Error. Please Correct</b><br />
        <div id="dvInfo" runat="server">
        </div>
        <br />
        <asp:Button ID="btnOkay" runat="server" Text="Okay" OnClick="btnOkay_Click" />
        <asp:GridView ID="dgError" runat="server" SkinID="gridview">
        </asp:GridView>
    </div>
    <asp:Button ID="btnMedicationOverride" runat="server" Text="" CausesValidation="true"
        CssClass="btn btn-primary" Style="visibility: hidden;" OnClick="btnMedicationOverride_OnClick" />
    <asp:HiddenField ID="hdnIsOverride" runat="server" Value="" />
    <asp:HiddenField ID="hdnOverrideComments" runat="server" Value="" />
    <asp:HiddenField ID="hdnDrugAllergyScreeningResult" runat="server" Value="" />
</asp:Content>
