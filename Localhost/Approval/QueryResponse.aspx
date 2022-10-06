<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true" CodeFile="QueryResponse.aspx.cs" Inherits="Approval_QueryResponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <%-- <link href="/Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <script src="../JS/bootstrap.bundle.min.js" type="text/javascript"></script>
    <script  type="text/javascript" src="/Include/JS/Common1.js"></script>
    <script src="/Include/jquery.min.js" type="text/javascript"></script>--%>
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
   <style>
       body { overflow: hidden;}
   </style>

    <%--<div class="container-fluid">
        <div class="row blue-headbg">
            <h2>Insurance Department Query</h2>
        </div>
    </div>--%>
   <div id="dvgvQuery" runat="server" class="contentscroll-auto">
       <asp:GridView ID="gvQuery" runat="server" AutoGenerateColumns="false" SkinID="gridview">  
       <Columns>
            <asp:BoundField DataField="UHID" HeaderText="UHID" />
            <asp:BoundField DataField="PatientName" HeaderText="PatientName" /> 
             <asp:BoundField DataField="Query" HeaderText="Query" />
            <asp:BoundField DataField="QueryBy" HeaderText="QueryBy" />
            <asp:BoundField DataField="QueryTo" HeaderText="QueryTo" />
            <asp:BoundField DataField="QueryDate" HeaderText="QueryDate" />
            <asp:BoundField DataField="Isclosed" HeaderText="Query Closed" />
            <asp:BoundField DataField="ClosedRemark" HeaderText="Closed Remark" />
            <asp:BoundField DataField="ClosedBy" HeaderText="ClosedBy" />
            <asp:BoundField DataField="ClosedDate" HeaderText="ClosedDate" />   
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button  ID="btnResponse" CommandName='<%#Eval("ID") %>' runat="server" Text="Respond" OnClick="btnResponse_Click" CssClass="btn btn-primary btn-xs"/>
                </ItemTemplate>
            </asp:TemplateField>  
       </Columns>
    </asp:GridView>
   </div> 
    
   
    <div class="row" id="dvx" runat="server" visible="false">
        <div class="response-contain">
            
            <h1>Please enter the reply here.</h1>

            <b style="color:red">Query</b> <asp:Label ID="lblQuery" runat="server" ForeColor="Red"></asp:Label> <br />
            <label>Query</label>
            <asp:TextBox ID="lblQueryText" ReadOnly="true"  runat="server"  TextMode="MultiLine" Width="100%" Height="100"></asp:TextBox>
              <label>Response</label>   
             <asp:TextBox ID="txtReply" runat="server" MaxLength="1000" TextMode="MultiLine" Height="100px" Width="100%" style="margin-bottom: 5px;"></asp:TextBox>
                <asp:Button ID="btnSave" runat="server" Text="Send" OnClick="btnSave_Click" SkinID="Button" />
                <asp:Button ID="btnClose" runat="server"  Text="Close" OnClick="btnClose_Click" SkinID="Button" />
                <asp:GridView ID="GvxConversation" runat="server" AutoGenerateColumns="false" SkinID="gridview">
                    <Columns>
                        <asp:BoundField DataField="Query" HeaderText="Query" />
                        <asp:BoundField DataField="QueryBy" HeaderText="QueryBy" />
                        <asp:BoundField DataField="QueryTo" HeaderText="QueryTo" />
                        <asp:BoundField DataField="QueryDate" HeaderText="QueryDate" />
                        <asp:BoundField DataField="Isclosed" HeaderText="Query Closed" />
                        <asp:BoundField DataField="ClosedRemark" HeaderText="Closed Remark" />
                        <asp:BoundField DataField="ClosedBy" HeaderText="ClosedBy" />
                        <asp:BoundField DataField="ClosedDate" HeaderText="ClosedDate" />
                    </Columns>
                </asp:GridView>

        </div>
    </div>
   
    <script type="text/javascript">
        function returnToParent() {
            var oArg = new Object();
            var oWnd = GetRadWindow();
            top.location.href = top.location.href;
            oWnd.close(oArg);;
        }
    </script>

</asp:Content>

