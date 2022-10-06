using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Telerik.Web.UI;

public partial class MRD_EMRPrintCaseSheetMRdReports : System.Web.UI.Page
{

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = DateTime.Now;
            dtpTodate.SelectedDate = DateTime.Now;
            bindMainTemplateList();
            txtRegNo.Visible = false;
            txtSearch.Visible = true;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGridView();
    }

    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRegNo.Text = "";
        txtSearch.Text = "";
        if (common.myInt(ddlSearchOn.SelectedValue) == 1)
        {
            txtRegNo.Visible = true;
            txtSearch.Visible = false;
        }
        else
        {
            txtRegNo.Visible = false;
            txtSearch.Visible = true;
        }
    }

    private void bindMainTemplateList()

    {
        clsIVF objivf = new clsIVF(sConString);
        DataSet ds = new DataSet();

        ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), "", 0, common.myInt(ViewState["ServiceId"]),
                                                common.myStr(Session["OPIP"]), common.myInt(Request.QueryString["TemplateGroupId"]), "",
                                                common.myInt(Session["FacilityID"]), common.myBool(Request.QueryString["IsAddendum"]),
                                                common.myInt(Session["InvoiceId"]), common.myInt(Session["RegistrationID"]),
                                                common.myInt(Session["encounterid"]), false, 0, common.myInt(Session["LoginDoctorId"]));


        ds.Tables[0].DefaultView.RowFilter = "TypeName = 'MRD'";
        DataTable dt = (ds.Tables[0].DefaultView).ToTable();

        ddlTemplateMain.DataSource = dt;
        ddlTemplateMain.DataBind();
    }

    private void BindGridView()
    {
        clsIVF objivf = new clsIVF(sConString);
        DataSet ds = new DataSet();

        string EncNo = "";
        string RegNo = "";
        string PatientName = "";


        switch (common.myInt(ddlSearchOn.SelectedValue))
        {
            case 1: // EncNo
                EncNo = common.myStr(txtSearch.Text);
                break;
            case 2: // RegNo
                txtSearch.Visible = false;
                txtRegNo.Visible = true;
                RegNo = common.myStr(txtRegNo.Text);
                break;
            case 4: // PatientName
                PatientName = common.myStr(txtSearch.Text);
                break;


        }

        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {

            ds = objEMR.GetEMRPrintCaseSheetMRdReports(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                txtRegNo.Text, EncNo, dtpfromdate.SelectedDate.ToString(),
                dtpTodate.SelectedDate.ToString(), "", common.myInt(ddlTemplateMain.SelectedValue), "",
                0, ShowCheckedItems(ddlTemplateColumn));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["PrintCaseSheetMRdReports"] = ds.Tables[0];
                    gvBindMRDTemplate.DataSource = ds.Tables[0];
                    gvBindMRDTemplate.DataBind();
                }

            }



        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private string ShowCheckedItems(RadComboBox comboBox)
    {
        string SelectedStatusid = string.Empty;
        var collection = comboBox.CheckedItems;


        if (collection.Count > 0)
        {
            foreach (var item in collection)
            {
                if (SelectedStatusid.Equals(string.Empty))
                {
                    SelectedStatusid = common.myStr(item.Value);
                    item.Attributes.ToString();
                }
                else
                {
                    SelectedStatusid = SelectedStatusid + "," + common.myStr(item.Value);
                }
            }
        }
        return SelectedStatusid;
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        DataTable dtRecords = new DataTable();

        foreach (GridColumn col in gvBindMRDTemplate.MasterTableView.RenderColumns)
        {
            DataColumn colString = new DataColumn(col.UniqueName);
            dtRecords.Columns.Add(colString);

        }
        foreach (GridDataItem row in gvBindMRDTemplate.Items) // loops through each rows in RadGrid
        {
            DataRow dr = dtRecords.NewRow();
            foreach (GridColumn col in gvBindMRDTemplate.MasterTableView.RenderColumns) //loops through each column in RadGrid
                dr[col.UniqueName] = row[col.UniqueName].Text;
            dtRecords.Rows.Add(dr);
        }

        ExportToExcel(dtRecords);

    }
    void ExportToExcel(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                HttpResponse response = HttpContext.Current.Response;

                // first let's clean up the response.object
                response.Clear();
                response.Charset = "";

                // set the response mime type for excel
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", "attachment;filename="+ hdnHeaderName.Value +".xls");

                // create a string writer
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        DataGrid dg = new DataGrid();
                        dt.Columns.Remove(dt.Columns[0]);
                        dt.Columns.Remove(dt.Columns[0]);

                        dg.DataSource = dt;
                        dg.DataBind();
                        dg.RenderControl(htw);
                        response.Flush();
                        response.Write(sw.ToString());
                        HttpContext.Current.Response.End();
                        response.Close();
                    }
                    Response.ClearContent();
                }

            }
            else
            {
                Alert.ShowAjaxMsg("No Record Found", Page);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


   

    protected void gvBindMRDTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            foreach (TableCell col in e.Row.Cells)
                col.Text = Context.Server.HtmlDecode(col.Text);



    }

    protected void gvBindMRDTemplate_PreRender(object sender, EventArgs e)
    {

        //BindGridView();

        gvBindMRDTemplate.DataSource = ViewState["PrintCaseSheetMRdReports"];
        gvBindMRDTemplate.DataBind();
    }

    protected void ddlTemplateMain_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        clsIVF objivf = new clsIVF(sConString);
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        ds = objEMR.GetEMRPrintCaseSheetMRDFildName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
              txtRegNo.Text, "", dtpfromdate.SelectedDate.ToString(),
              dtpTodate.SelectedDate.ToString(), "", common.myInt(ddlTemplateMain.SelectedValue), "", 0);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlTemplateColumn.DataTextField = "FieldName";
                ddlTemplateColumn.DataValueField = "FieldName";

                ddlTemplateColumn.DataSource = ds.Tables[0];
                ddlTemplateColumn.DataBind();

                foreach (RadComboBoxItem currentItem in ddlTemplateColumn.Items)
                {
                    currentItem.Checked = true;

                }


            }
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    hdnHeaderName.Value = ds.Tables[1].Rows[0]["HeadingName"].ToString();
                }
            }
        }
    }

    protected void gvBindMRDTemplate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        foreach (GridColumn column in gvBindMRDTemplate.MasterTableView.AutoGeneratedColumns)
        {

            var collection = ddlTemplateColumn.CheckedItems;


            if (collection.Count > 0)
            {
                foreach (var item in collection)
                {


                    if (column.UniqueName.Contains(item.Value))
                    {
                        column.SortExpression = item.Value;
                        column.AutoPostBackOnFilter = true;
                        column.ShowFilterIcon = false;
                        
                    }

                    else if (column.UniqueName.Contains("UHID"))
                    {
                        column.SortExpression = "UHID";
                        column.AutoPostBackOnFilter = true;
                        column.ShowFilterIcon = false;
                        
                    }
                    else if (column.UniqueName.Contains("IP No."))
                    {
                        column.SortExpression = "IP No.";
                        column.AutoPostBackOnFilter = true;
                        column.ShowFilterIcon = false;
                        
                    }

                    else if (column.UniqueName.Contains("Facility Name"))
                    {
                        column.SortExpression = "Facility Name";
                        column.AutoPostBackOnFilter = true;
                        column.ShowFilterIcon = false;
                      

                    }

                    else if (column.UniqueName.Contains("Patient Name"))
                    {
                        column.SortExpression = "Patient Name";
                        column.AutoPostBackOnFilter = true;
                        column.ShowFilterIcon = false;
                      
                    }

                    else if (column.UniqueName.Contains("Date of Admission"))
                    {
                        column.SortExpression = "Date of Admission";
                        column.AutoPostBackOnFilter = true;
                        column.ShowFilterIcon = false;
                       
                       
                    }

                    else if (column.UniqueName.Contains("Date of Discharge"))
                    {
                        column.SortExpression = "Date of Discharge";
                        column.AutoPostBackOnFilter = true;
                        column.ShowFilterIcon = false;
                        
                    }


                }
            }

        }
    }
}