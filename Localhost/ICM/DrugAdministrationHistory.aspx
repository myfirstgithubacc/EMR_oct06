<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="DrugAdministrationHistory.aspx.cs" Inherits="ICM_DrugAdministrationHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        body#ctl00_Body1 {
    overflow-x: hidden;
}
        .tableresponsive + div{
            overflow-x:auto;
            width:100%;
        }
        th{
            white-space:nowrap;
        }
    </style>
    <div class="container-fluid">
        <div class="row" style="background: #3b82f56e;padding: 6px;margin-bottom: 5px;">
            <div class="col-5" style="font-size: 14px;font-weight: 700;color: black;">
                Drug administration History
            </div>
            <div class="col-7 text-right">
                  <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Text="Close" OnClientClick="window.close();" />
            </div>
        </div>
    </div>
   
    <div class="row">
        <div class="col-12 tableresponsive">
            
                <asp:GridView ID="gvDrugAdministrator" runat="server" AutoGenerateColumns="false" SkinID="gridview"
                    AllowPaging="true" PageSize="20" Width="100%" OnPageIndexChanging="gvDrugAdministrator_PageIndexChanging">
                    <Columns>
                        <asp:BoundField HeaderText="Prescription Date" DataField="PrescriptionDate" SortExpression="PrescriptionDate" />
                        <asp:BoundField HeaderText="Date of administration" DataField="AdmissionDate" SortExpression="AdmissionDate" />
                        <asp:BoundField HeaderText="Time of administration" DataField="DrugAdministertime" SortExpression="DrugAdministertime" />
                        <asp:BoundField HeaderText="Name and dose of medication" DataField="ItemName" SortExpression="ItemName" />
                        <asp:BoundField HeaderText="Medication administered by" DataField="Medicationadministeredby" SortExpression="Medicationadministeredby" />
                        <asp:BoundField HeaderText="Cross checked by" DataField="CrossCheckedBy" SortExpression="CrossCheckedBy" />
                    </Columns>
                </asp:GridView>
            
        </div>
    </div>
  
</asp:Content>
