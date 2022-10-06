<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ChifComplaint.aspx.cs" Inherits="EMR_Problems_ChifComplaint" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<link href="../../Include/css/bootstrap.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" />



    <style>
        div#RadWindowWrapper_ctl00_RadWindowForNew {
            display: none !important;
        }

        .duration-input {
            display: inline-block;
            width: 40%;
        }

        .duration-option {
            display: inline-block;
            width: 50%;
            margin-left: 2%;
        }

        .tab-pane label.checkbox-inline {
            width: 16%;
            margin: 5px 0 !important;
        }

        .tabs-custom {
            border-bottom: none;
        }

            .tabs-custom li a {
                border-top: 4px solid #fff !important;
            }

            .tabs-custom li a, .tabs-custom li.active a {
                border: 0;
            }

            .tabs-custom li.active a {
                border-top: 4px solid #06c !important;
                border-radius: 0;
            }

                .tabs-custom li a:focus, .tabs-custom li a:hover, .tabs-custom li.active a:focus, .tabs-custom li.active a:hover {
                    background: none;
                    border: 0;
                }




        .filter-custom {
            position: absolute;
            top: 0;
            left: 15px;
            max-width: 30%;
            border-radius: 30px;
        }

            .filter-custom ~ a {
                min-height: 29px;
                line-height: 24px;
                border-radius: 0px 15px 15px 0 !important;
                margin-left: 29%;
            }

        ul.nav.nav-tabs.tabs-custom {
            float: right;
            margin-bottom: 1px;
        }

            ul.nav.nav-tabs.tabs-custom ~ .tab-content {
                clear: both;
            }


        .tab-content {
            border: 1px solid #ccc;
            margin-top: 1px;
        }

        .top-textbar * {
            font-size: 12px !important;
        }

        span.checkbox-row {
            width: 25%;
            display: inline-block;
            margin-bottom: 5px;
            font-family: Arial !important;
            font-size: 13px !important;
        }

            span.checkbox-row [type="checkbox"] {
                margin-right: 5px;
            }

        .table-data .table td {
            border: 1px solid #ccc !important;
        }

        .bs-linebreak {
            height: 10px;
        }

        tbody#tbl_ChifComplaint tr {
            font-family: Arial !important;
            font-size: 13px !important;
        }

        iframe[name="RadWindowForNew"] {
            overflow-y: hidden;
            height: 74vh !important;
        }

        div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow1 div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowForNew iframe { /*height: 80vh !important; overflow:hidden;*/
        }

        .delFav {
            margin-left: 10px;
            cursor: pointer;
        }
    </style>

    <div class="container-fluid">
        <div class="row bg-info1">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12 top-textbar p-t-b-5">
                <asp:Label runat="server" ID="lblCId" Style="border-top-left-radius: 10px; background: #0a76da; color: #fff; font-weight: 600; font-size: 14px; padding: 5px; float: left;">0000255275</asp:Label>
                <asp:Label runat="server" ID="lblPName" Style="border-top-right-radius: 10px; background: #bb0d0d; color: #fff; font-weight: 600; font-size: 14px; padding: 5px; float: left;">Ms. Ludivine Leyla Essah Sonmez</asp:Label>

                <asp:Label runat="server" ID="lblGenderAge" Style="background: #fff; color: #595a5f; font-size: 14px; padding: 5px; float: left;">Female/ 31Y 1M 15D</asp:Label>
                <asp:Label runat="server" ID="lblVtCrPrvdr" Style="border-radius: 10px; background: #668f00; color: #fff; font-size: 14px; padding: 5px; float: left;">Dr Salvin George, MD, MRCP</asp:Label>

                <span style="background: #fff; color: #595a5f; font-size: 14px; padding: 5px; float: left;">Visit Date</span>
                <asp:Label runat="server" ID="lblEncDate" Style="border-radius: 10px; background: #668f00; color: #fff; font-size: 14px; padding: 5px; float: left;">15/07/2015</asp:Label>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12 text-right p-t-b-5">
                <a id="A2" href="javascript:void(0);" onclick="A2Click();" runat="server" class="btn btn-warning" tooltip="Chief Complaint" data-toggle="modal" data-target="#myModal">Master</a>

                <a id="A1" href="javascript:void(0);" class="btn btn-success" onclick="SaveDotorFavComplaint();">Favorite</a>

                <a id="btnSave" href="javascript:void(0);" runat="server" class="btn btn-primary" tooltip="Save" onclick="SaveComplaint();">Save</a>
                <asp:Button ID="btnClose" CssClass="btn btn-danger" runat="server" Text="Close"
                    OnClientClick="window.close();" />
                <%--<asp:UpdatePanel runat="server" ID="updatepanelchiefcomplaint">
                                                <ContentTemplate>--%>
                <asp:Button ID="btnChiefComplaintHIS" runat="server" class="btn btn-primary" Style="cursor: pointer;"
                    OnClick="lnkChiefComplaint_OnClick" title="Chief Complaints HIS" Text="Complaint(s) History" />
                <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
            </div>
        </div>
        <hr style="margin: 10px 0;" />
        <div class="alert alert-success" style="display: none" id="divmessage">
            <strong>Success!</strong>
            <label id="message"></label>
            .
       
        </div>

        <div class="row">
            <div class="col-md-12">
                <ul class="nav nav-tabs tabs-custom">
                    <li class="active"><a data-toggle="tab" href="#fav">Doctor Favourite</a></li>
                    <%-- <li><a data-toggle="" href="#spec">Specialization </a></li>--%>
                    <li><a data-toggle="tab" href="#all">All</a></li>
                </ul>
                <div class="tab-content">
                    <div id="fav" class="tab-pane fade in active">
                        <input type="search" placeholder="Search Text" onkeyup="doctorFav();" class="form-control input-sm filter-custom" id="txtFevSearch" />
                        <a id="btnFevSearch" class="btn btn-info filter-custom"></a>
                        <div id="btnGroup" style="height: 200px; width: 100%; overflow-y: auto;">
                        </div>
                    </div>
                    <div id="spec" class="tab-pane fade">
                        <input type="search" placeholder="Search Text" class="form-control input-sm filter-custom" />
                        <a onclick="" class="btn btn-success filter-custom"></a>


                    </div>
                    <div id="all" class="tab-pane fade">
                        <input type="search" placeholder="Search Text" onkeyup="AllProblem();" class="form-control input-sm filter-custom" id="txtAllSearch" />
                        <a class="btn btn-info filter-custom"></a>
                        <div id="divAll" style="height: 200px; width: 100%; overflow-y: auto;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12 table-data" style="margin-top: 5px;">
                <div class="table-responsive gridview">
                    <table class="table table-condensed table-custom table-bordered table-striped" width="100%" id="tbl_PatientProblem">
                        <tr>

                            <th>Chief Complaints</th>
                            <th>Duration</th>
                            <th>Details</th>
                            <th style="display: none;">Action  </th>
                        </tr>
                        <tbody id="tbl_ChifComplaint">
                        </tbody>

                    </table>
                </div>
            </div>
        </div>
    </div>

    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Add Chief Complaint</h4>
                </div>
                <div class="modal-body">
                    <div class="alert alert-success" style="display: none" id="divmsg">
                        <strong>Success!</strong>
                        <label id="msg">Data Saved !!</label>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="alert alert-danger" style="display: none" id="diverror">
                        <strong>Required!</strong>
                        <label id="error">Please enter complaint description</label>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>


                    <label>Complaint Description</label>
                    <input type="text" id="txtproblem" class="form-control" />
                    <div class="row bs-linebreak"></div>
                    <label>
                        <input type="checkbox" id="chkFavourite" />
                        Is Favourite</label>

                </div>
                <div class="modal-footer">

                    <button type="button" class="btn btn-primary" id="SaveProblem" onclick="SaveMaster()">Save</button>
                    <button type="reset" class="btn btn-danger">Reset</button>

                </div>
            </div>

        </div>
    </div>

    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Metro" EnableViewState="false" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Pin,Move,Minimize,Maximize,Resize" />
        </Windows>
    </telerik:RadWindowManager>
    <%--        <asp:UpdatePanel ID="UpdatePanel43" runat="server">
                                                        <ContentTemplate>
                                                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Skin="Metro">
                                                                <Windows>
                                                                    <telerik:RadWindow ID="RadWindowForNew" Skin="Metro" runat="server" EnableViewState="false" Height="500" Width="650" MinWidth="650"
                                                                        ReloadOnShow="true" ShowContentDuringLoad="false"  VisibleStatusbar="false" OnClientShow="setCustomPosition" Behaviors="Close,Maximize,Minimize,Move,Pin,Resize" VisibleTitlebar="true" >
                                                                    </telerik:RadWindow>
                                                                </Windows>
                                                            </telerik:RadWindowManager>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
    <script src="/Include/JS/StringBuilder.js"></script>

    <script>

        $(document).ready(function () {
            // alert('kuldeep');
            // LanguageConnect();
            doctorFav();
            AllProblem();
        });

        function doctorFav() {
            
            let url = '<%=ConfigurationManager.AppSettings["WebAPIAddress"]%>';
            var txtsearch = $('#txtFevSearch').val();
            var obj = new Object();
            let sb = new StringBuilder();
            obj.strSearchCriteria = txtsearch;
            obj.DoctorID = parseInt(<%=Session["EmployeeId"]%>);
            obj.Type = '';
            $.ajax({
                url: url + "api/EMRAPI/BindDoctorFavouriteProblems",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {
                    // var jsonData = JSON.parse(res);
                    let jsonData = res;
                    for (var i = 0; i < jsonData.length; i++) {
                        // sb.append(" <input id='chk" + i + "' class='form-check-input' type='checkbox' onclick='updateChecbox(this," + i + "," + JSON.stringify(tableData[i]) + ")' >")
                        // sb.append("<span class='checkbox-row'><input name='fev'  id='chk" + i + "'  type='checkbox'  onclick='onFavclick(this," + i + "," + JSON.stringify(jsonData[i]) + ")'>" + jsonData[i].ProblemDescription + "</span>")
                        
                        sb.append("<span class='checkbox-row'><input name='fev'  id='chk" + i + "'  type='checkbox'  onclick='onFavclick(this," + i + "," + JSON.stringify(jsonData[i]) + ")'>" + jsonData[i].ProblemDescription + "<a name='fav'  id='delfav" + i + "'  class='delFav'  onclick='onDelFavclick(this," + i + "," + JSON.stringify(jsonData[i]) + ")'>X</a></span> ")
                    }
                    $("#btnGroup").empty();

                    $('#btnGroup').append(sb.toString());
                }

            })
        }

        function AllProblem() {
            
            let url = '<%=ConfigurationManager.AppSettings["WebAPIAddress"]%>';
            var txtsearch = $('#txtAllSearch').val();
            var obj = new Object();
            let sb = new StringBuilder();
            obj.strSearchCriteria = txtsearch;
            obj.DoctorID = parseInt(<%=Session["DoctorID"]%>);
            obj.Type = 'All';
            $.ajax({
                url: url + "api/EMRAPI/BindDoctorFavouriteProblems",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {
                    // var jsonData = JSON.parse(res);
                    let jsonData = res;
                    for (var i = 0; i < jsonData.length; i++) {
                        // sb.append(" <input id='chk" + i + "' class='form-check-input' type='checkbox' onclick='updateChecbox(this," + i + "," + JSON.stringify(tableData[i]) + ")' >")
                        sb.append("<span class='checkbox-row'><input name='all'  id='chk" + i + "'  type='checkbox'  onclick='onFavclick(this," + i + "," + JSON.stringify(jsonData[i]) + ")'>" + jsonData[i].ProblemDescription + "</a></span>")
                    }
                    $("#divAll").empty();

                    $('#divAll').append(sb.toString());
                }

            })
        }

        let FevbackList = [];
        var i = 0;
        function onFavclick(control, index, data) {
            // control.style.backgroundColor = '#00bf5b';
            //  alert(JSON.stringify(Data));
            console.log("data in favclick function :"+JSON.stringify(data));

            // yogesh new =section
            let sb = new StringBuilder();
            let obj = new Object();
            console.log("dta in obj :" + JSON.stringify(obj));
    
            obj.RowId = index;
            obj.ProblemId = data.ProblemId;
            obj.ProblemDescription = data.ProblemDescription;
            obj.InputType = data.InputType;
            obj.SelectedValue = "";//Default
            obj.Rating = 0; //Default
            obj.CheckBoxSelectedValue = control.checked;
            obj.Id = control.id;
            obj.Name = control.name;

            console.log("index "+index);
            console.log("Problem Id"+data.ProblemId);
            console.log("Input type"+data.InputType);
            console.log("Control checked"+control.checked);
            console.log("control Id"+control.id);
            console.log("Control Name="+control.name);
          



            let probDesc = JSON.stringify(obj.ProblemDescription);
            var objArr =  [];
      
            if (obj.CheckBoxSelectedValue == false)
            {
                
                //alert("yo not checked!!");
               // debugger;

               // $("tr").eq(control.id).remove();
               
            } else {
                debugger;
                var cid = "#"+control.id;
                
               // alert("CID"+cid);
                $("#"+control.id).attr('disabled', !$("#"+control.id).attr('disabled'));   
                
                sb.append('<tr> <td style="display:none;"> ' + obj.ProblemId + '</td> <td style="display:none;"> ' + obj.RowId + '</td> <td> <input type="text" id="txtPatientProblem" style="height: 43px;width: 285px;" value=' + probDesc  + ' ></td>' + '<td> <select  class="form-control input-sm duration-input" id="ddlduration' + obj.SelectedValue + '"><option  value="0" Selected>0</option><option  value="1" >1</option><option  value="2" >2</option><option  value="3" >3</option><option  value="4" >4</option><option  value="5" >5</option><option  value="6" >6</option><option  value="7" >7</option><option  value="8" >8</option><option  value="9" >9</option><option  value="10" >10</option><option  value="11" >11</option><option  value="12" >12</option><option  value="13" >13</option><option  value="14" >14</option><option  value="15" >15</option><option  value="16" >16</option><option  value="17" >17</option><option  value="18" >18</option><option  value="19" >19</option><option  value="20" >20</option>   </select><select class="form-control input-sm duration-option" id="ddldurationtype' + i + '"><option  value="D" >Days</option><option value="W">Weeks</option><option value="M">Months</option><option value="Y">Years</option></select></td>' + '<td><textarea class="form-control" id="txtremark' + i + '" style="height:30px;"></textarea></td><td style="display:none;"><a href="javascript:void(0);" class="btn btn-danger btn-sm" onclick = "DeletePayment(this);">X</a></td><td><input type="button" value="Delete Row" onclick="RemoveRow(this,\'' + cid + '\')"></td>'); 
                sb.append('</tr>'); 
                i++;
                $('#tbl_ChifComplaint ').append(sb.toString());

              
            }

           
           //removeSelectedChild(index);



            // section end





            //let sb = new StringBuilder();
            //let obj = new Object();
            
    
            //obj.RowId = index;
            //obj.ProblemId = data.ProblemId;
            //obj.ProblemDescription = data.ProblemDescription;
            //obj.InputType = data.InputType;
            //obj.SelectedValue = "";//Default
            //obj.Rating = 0; //Default
            //obj.CheckBoxSelectedValue = control.checked;
            //obj.Id = control.id;
            //obj.Name = control.name;
         //   addUpdateRadioList(FevbackList, obj);
       

             

            //console.log("Array after favclick data :"+JSON.stringify(FevbackList));

            //$("#tbl_ChifComplaint").children().remove();
            //if (FevbackList.length) {  
                
            //   //yogesh 03/08/2022
            //    for (var i = 0; i < FevbackList.length; i++) {
            //        let jsonData = FevbackList;   
            //        let pDesc = JSON.stringify(FevbackList[i]["ProblemDescription"]);
            //        sb.append('<tr> <td style="display:none;"> ' + FevbackList[i]["ProblemId"] + '</td> <td style="display:none;"> ' + FevbackList[i]["Id"] + '</td> <td> <input type="text" id="txtPatientProblem" " style="height: 43px;width: 285px;" value=' + pDesc + ' ></td>' + '<td> <select  class="form-control input-sm duration-input" id="ddlduration' + i + '"><option  value="1" >1</option><option  value="2" >2</option><option  value="3" >3</option><option  value="4" >4</option><option  value="5" >5</option><option  value="6" >6</option><option  value="7" >7</option><option  value="8" >8</option><option  value="9" >9</option><option  value="10" >10</option><option  value="11" >11</option><option  value="12" >12</option><option  value="13" >13</option><option  value="14" >4</option><option  value="15" >15</option><option  value="16" >16</option><option  value="17" >17</option><option  value="18" >18</option><option  value="19" >19</option><option  value="20" >20</option>   </select><select class="form-control input-sm duration-option" id="ddldurationtype' + i + '"><option  value="D" >Days</option><option value="W">Weeks</option><option value="M">Months</option><option value="Y">Years</option></select></td>' + '<td><textarea class="form-control" id="txtremark' + i + '" style="height:30px;"></textarea></td><td style="display:none;"><a href="javascript:void(0);" class="btn btn-danger btn-sm" onclick = "DeletePayment(this);">X</a></td>'); 
                                                                                                                                                                                             
            //      //  sb.append('<tr> <td style="display:none;"> ' + FevbackList[i]["ProblemId"] + '</td> <td style="display:none;"> ' + FevbackList[i]["Id"] + '</td> <td><textarea class="form-control" id="txtProblemDesc" style="height:30px;">'+ JSON.stringify(FevbackList[i]["ProblemDescription"]) +'</textarea></td>' + '<td> <select  class="form-control input-sm duration-input" id="ddlduration' + i + '"><option  value="1" >1</option><option  value="2" >2</option><option  value="3" >3</option><option  value="4" >4</option><option  value="5" >5</option><option  value="6" >6</option><option  value="7" >7</option><option  value="8" >8</option><option  value="9" >9</option><option  value="10" >10</option><option  value="11" >11</option><option  value="12" >12</option><option  value="13" >13</option><option  value="14" >4</option><option  value="15" >15</option><option  value="16" >16</option><option  value="17" >17</option><option  value="18" >18</option><option  value="19" >19</option><option  value="20" >20</option>   </select><select class="form-control input-sm duration-option" id="ddldurationtype' + i + '"><option  value="D" >Days</option><option value="W">Weeks</option><option value="M">Months</option><option value="Y">Years</option></select></td>' + '<td><textarea class="form-control" id="txtremark' + i + '" style="height:30px;"></textarea></td><td style="display:none;"><a href="javascript:void(0);" class="btn btn-danger btn-sm" onclick = "DeletePayment(this);">X</a></td>');

            //        sb.append('</tr>');   
            //    } 
            //}
            ////sb.append('<tr><td>Kuldeep</td></tr>');
            //$('#tbl_ChifComplaint ').append(sb.toString());
          
        }

       //yogesh 05/08/2022
        function RemoveRow (o, cid)
        {
            var p=o.parentNode.parentNode;
            p.parentNode.removeChild(p);
            $(cid).attr('disabled', !$(cid).attr('disabled')); 
            $(cid).prop("checked", false);
            i--;
        }

        function onDelFavclick(control, index, data) {
           
            let url = '<%=ConfigurationManager.AppSettings["WebAPIAddress"]%>';           
            var obj = new Object();          
           
            obj.DoctorID = parseInt(<%=Session["EmployeeId"]%>);
            obj.ProblemId = data.ProblemId;
            $.ajax({
                url: url + "api/EMRAPI/RemoveDoctorFavProblems",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {                    
                    alert(res);
                    doctorFav();
                }

            })
            

        }
        function addUpdateRadioList(arr, obj) {
           
       
            for (var i = arr.length; i--;) {
                
                if (arr[i].RowId == obj.RowId && arr[i].Name == obj.Name) {
                    if (arr[i].Rating != 0)
                        obj.Rating = arr[i].Rating;
                    arr.splice(i, 1);
                    FevbackList = $(FevbackList).not([obj.RowId]).get();

                    console.log("IN UPDATE array :"+JSON.stringify(FevbackList));
                } 

            }
            if (obj.CheckBoxSelectedValue == true)
            {
                FevbackList.push(obj);
            }
           

            // console.log(FevbackList);
        }
        function DeletePayment(Input) {
            
            var $this = $(Input);
            var c = confirm('Are you sure to remove this record?');
            if (c) {
                $this.parents('tr').fadeOut(function () {
                    var problemId = $(Input).parent().parent().find("td:eq(0)").html();
                    var chkId = $(Input).parent().parent().find("td:eq(1)").html();
                    $(chkId).trigger('click');
                    FevbackList = $(FevbackList).not([obj.problemId]).get();                   
                    $this.remove();
                });
            }

        }
        function SaveComplaint()
        {
            debugger;
            let url = '<%=ConfigurationManager.AppSettings["WebAPIAddress"]%>';
            let rowCount = $('#tbl_PatientProblem tbody tr').length;
            var Xml = new Array();
            let obj = new Object();
            if (rowCount > 0)
            {
                $('#tbl_PatientProblem tbody tr').not(':first').each(function (i, row) // .not(':first').not(':last')
                {
                    console.log("In row is "+i,row);
                    debugger;
                    Xml[i] = {
                        //yogesh
                        problemId: $(row).find("td:eq(0)").text(),  // $(row).find("td:eq(1)").text(),
                        //problemName: $(row).find("td:eq(2)").text(),
                        problemName: $(row).find("#txtPatientProblem").val(),
                        intduration:$(row).find("#ddlduration").val(),
                        duration:$(row).find("#ddldurationtype"+i).val(),
                        remarks:$(row).find('#txtremark' + i ).val() 
                    }
                });

                // alert(JSON.stringify(Xml));
                //var xmldata = JSON.stringify(Xml);
                
                obj.HospitalLocationID = <%=Session["HospitalLocationId"]%>;
                obj.FacilityId =  <%=Session["FacilityID"]%>;
                obj.RegistrationId = <%=Session["RegistrationID"]%>;
                obj.EncounterId = parseInt(<%=Session["EncounterId"]%>);
                obj.PageId = 0;
                obj.UserId = parseInt(<%=Session["UserId"]%>); 
                obj.PatientProblem = Xml;               
                obj.Remarks = "";
                obj.IsPregment = false;
                obj.IsBreastFeed = false;
                obj.IsShowNote = false;
                obj.DoctorId = parseInt(<%=Session["DoctorId"]%>); 

                if(Xml.length > 0)
                {
                    $.ajax({
                        url: url + "api/EMRAPI/SavePatientChiefComplaint",
                        type: 'post',
                        dataType: 'json',
                        data: obj,
                        success: function (res) {
                            // alert(JSON.parse(res));
                            $('#message').text(JSON.parse(res));
                            $('#divmessage').css('display','block');
                            setTimeout(function() {
                                location.reload();
                            }, 4000);

                      
                        },
                        error: function (res) {
                            toastr.error(res, "Error");
                        }

                    });
                }
                else
                {
                    alert('Please add Complaint in the list');

                }
            }
            else
            {
                alert('Please add Complaint in the list');

            }
        }

        function LanguageConnect()
        {
            debugger;
            let obj = new Object();
            obj.sourceText="Kuldeep";
            obj.sourceLanguage="EN";
            obj.targetLanguage="HI"
            $.ajax({
                type: "Get",
                url: 'http://43.242.214.195:999/LanguageService.asmx?op=Language',
                //data: "{itemName:''}",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) 
                {
                    var jsonData = JSON.parse(res.d);
                    alert(jsonData);
                    
                },
                error: function (res) {
                    console.log(res);
                }
            })
        }
        function SaveMaster()
        {
            debugger;
            let problem = $('#txtproblem').val();
            if(problem == "")
            {
                $('#diverror').css('display','block');
                return;
            }
            $('#diverror').css('display','none');     
            
            let url = '<%=ConfigurationManager.AppSettings["WebAPIAddress"]%>';           
            var obj = new Object();
            let sb = new StringBuilder();
            obj.Description = problem;
            obj.HospitalLocationId = parseInt(<%=Session["HospitalLocationId"]%>);
            obj.DoctorID = parseInt(<%=Session["EmployeeId"]%>)
            if (document.getElementById('chkFavourite').checked) {
                obj.IsFavourite = true;
            } else {
                obj.IsFavourite = false;
            }
           
            obj.EncodedBy= parseInt(<%=Session["UserId"]%>); 
            obj.specialisationid= parseInt(<%=Session["UserSpecialisationId"]%>);             
            $.ajax({
                url: url + "api/EMRAPI/SaveMasterComplaint",
                type: 'post',
                dataType: 'json',
                data: obj,
                success: function (res) {                   
                    $('#divmsg').css('display','block');
                    doctorFav();
                    AllProblem();
                  
                }

            });
        }
        function SaveDotorFavComplaint()
        {
            debugger;
            let url = '<%=ConfigurationManager.AppSettings["WebAPIAddress"]%>';
            let rowCount = $('#tbl_PatientProblem tbody tr').length;
            var Xml = new Array();
            let obj = new Object();
            if (rowCount > 0)
            {
                $('#tbl_PatientProblem tbody tr').not(':first').each(function (i, row) // .not(':first').not(':last')
                {

                    Xml[i] = {

                        problemId: $(row).find("td:eq(0)").text(),  // $(row).find("td:eq(1)").text(),                                          
                    }
                });
                
                obj.DoctorId = parseInt(<%=Session["EmployeeId"]%>); 
                obj.DoctorProblem = Xml;
                obj.UserId = parseInt(<%=Session["UserId"]%>);
                if(Xml.length > 0)
                {
                    $.ajax({
                        url: url + "api/EMRAPI/SaveDoctorFavChiefComplaint",
                        type: 'post',
                        dataType: 'json',
                        data: obj,
                        success: function (res) {   
                            
                            alert ("Doctor Favorite Added !!!")
                            setTimeout(function() {
                                location.reload();
                            }, 2000);                      
                        },
                        error: function (res) {
                            toastr.error(res, "Error");
                        }

                    });
                }
                else
                {
                    alert('Please add Complaint in the list');

                }
            }
            else
            {
                alert('Please add Complaint in the list');

            }
        }
    </script>
    <script src="../../Include/New/jquery-3.1.1.min.js"></script>
    <script src="../../Include/JS/bootstrap.min.js"></script>

</asp:Content>

