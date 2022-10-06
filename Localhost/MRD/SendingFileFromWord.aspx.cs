using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class MRD_SendingFileFromWord : System.Web.UI.Page
{
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    // wcf_Service_MRD.MRDServiceClient objMRD = new wcf_Service_MRD.MRDServiceClient();
    BaseC.RestFulAPI objMRD = new BaseC.RestFulAPI(sConString);
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PageType"] = common.myStr(Request.QueryString["PT"]);

            ViewState["DSStatus"] = null;

           // setDate();

            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtFromDate.SelectedDate = DateTime.Now;

            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.SelectedDate = DateTime.Now;


            bindDetailsData();
        }
    }

     

    private void bindDetailsData()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        lblMessage.Text = "&nbsp;";

        DataSet ds = new DataSet();
        try
        {
            //objMRD = new wcf_Service_MRD.MRDServiceClient();
            objMRD = new BaseC.RestFulAPI(sConString);



            ds = objMRD.GetMRDRequestFileFromWardReports(common.myInt(Session["HospitalLocationID"]),
                           common.myInt(Session["FacilityID"]),  
                           common.myDate(txtFromDate.SelectedDate.Value), common.myDate(txtToDate.SelectedDate.Value), sConString, txtSearchOnForUHID.Text);

            //ds = objMRD.GetMRDRequestFile(common.myInt(ddlFileStatus.SelectedValue), 0, common.myInt(Session["HospitalLocationID"]),
            //                common.myInt(Session["FacilityID"]), common.myStr(ddlPatientType.SelectedValue),
            //                common.myDate(txtFromDate.SelectedDate.Value), common.myDate(txtToDate.SelectedDate.Value),sConString, txtSearchOnForUHID.Text,txtSearchOnEnc.Text,txtSearchOnPat.Text);



            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvData.DataSource = ds.Tables[0];
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void ddlFileStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindDetailsData();
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        //reBindData();
        // setDate();
    }



    protected void gvData_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                //HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
                //int QuotationId = common.myInt(((Label)row.FindControl("lblQuotationId")).Text);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        bindDetailsData();
    }

    protected void menuStatus_ItemClick(object sender, RadMenuEventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";

            //Telerik.Web.UI.RadContextMenu menuStatus = (RadContextMenu)e.Item.NamingContainer;

            //// objMRD = new wcf_Service_MRD.MRDServiceClient();
            //objMRD = new BaseC.RestFulAPI(sConString);
            //string strMsg = objMRD.UpdateMRDFileStatus(common.myInt(hdnGRequestId.Value), common.myInt(menuStatus.SelectedValue),
            //                        common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), sConString);

            //if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            //{
            //    bindDetailsData();

            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            //}
            //lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ////    HiddenField hdnDispenseStatus = (HiddenField)e.Row.FindControl("hdnDispenseStatus");

            ////    System.Drawing.Color color = System.Drawing.Color.White;

            ////    if (common.myInt(hdnDispenseStatus.Value) == 3)
            ////    {
            ////        e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
            ////    }
            ////    else if (common.myInt(hdnDispenseStatus.Value) == 2)
            ////    {
            ////        e.Row.BackColor = System.Drawing.Color.Bisque;
            ////    }

            //HiddenField hdnRequestId = (HiddenField)e.Row.FindControl("hdnRequestId");
            //HiddenField hdnMRDStatusId = (HiddenField)e.Row.FindControl("hdnMRDStatusId");
            //HiddenField hdnMRDStatusCode = (HiddenField)e.Row.FindControl("hdnMRDStatusCode");
            //ImageButton btnCategory = (ImageButton)e.Row.FindControl("btnCategory");

            //Telerik.Web.UI.RadContextMenu menuStatus = (Telerik.Web.UI.RadContextMenu)e.Row.FindControl("menuStatus");

            //DataSet ds = (DataSet)ViewState["DSStatus"];
            //DataView DV = new DataView(ds.Tables[0].Copy());

            //bool isFound = false;
            //for (int rIdx = 0; rIdx < ds.Tables[0].Rows.Count; rIdx++)
            //{
            //    DataRow DR = ds.Tables[0].Rows[rIdx];

            //    if (isFound)
            //    {
            //        DV.RowFilter = "StatusId=" + common.myInt(DR["StatusId"]);
            //        break;
            //    }
            //    if (common.myInt(DR["StatusId"]) == common.myInt(hdnMRDStatusId.Value))
            //    {
            //        isFound = true;
            //    }
            //}

            //if (common.myInt(hdnRequestId.Value) == 0
            //    || common.myInt(ddlFileStatus.SelectedValue) == 0
            //    || common.myStr(hdnMRDStatusCode.Value) == "RTN")
            //{
            //    btnCategory.Visible = false;
            //}
        }
    }

    protected void ddlSearchOn_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlSearchOn.SelectedValue == "UHID")
        {


            txtSearchOnForUHID.Visible = true;
            txtSearchOnEnc.Visible = false;
            txtSearchOnPat.Visible = false;
            txtSearchOnEnc.Text = string.Empty;
            txtSearchOnPat.Text = string.Empty;

        }
        else if (ddlSearchOn.SelectedValue == "Enc")
        {


            txtSearchOnEnc.Visible = true;
            txtSearchOnForUHID.Visible = false;
            txtSearchOnPat.Visible = false;
            txtSearchOnForUHID.Text = string.Empty;
            txtSearchOnPat.Text = string.Empty;


        }
        else
        {
            txtSearchOnPat.Visible = true;
            txtSearchOnForUHID.Visible = false;
            txtSearchOnEnc.Visible = false;
            txtSearchOnForUHID.Text = string.Empty;
            txtSearchOnEnc.Text = string.Empty;

        }

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
       
        DataSet ds = new DataSet();
        objMRD = new BaseC.RestFulAPI(sConString);
        try
        {
            if (Convert.ToDateTime(txtFromDate.SelectedDate.Value) > Convert.ToDateTime(txtToDate.SelectedDate.Value))
            {
                lblMessage.Text = "From Date Should Not be Greater Than To Date !";
                return;
            }


            ds = objMRD.GetMRDRequestFileFromWardReports(common.myInt(Session["HospitalLocationID"]),
                           common.myInt(Session["FacilityID"]),
                           common.myDate(txtFromDate.SelectedDate.Value), common.myDate(txtToDate.SelectedDate.Value), sConString, txtSearchOnForUHID.Text);


            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    HttpResponse response = HttpContext.Current.Response;

                    // first let's clean up the response.object
                    response.Clear();
                    response.Charset = "";

                    // set the response mime type for excel
                    response.ContentType = "application/vnd.ms-excel";
                    response.AddHeader("Content-Disposition", "attachment;filename=SendFilefromward.xls");

                    // create a string writer
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {
                            DataGrid dg = new DataGrid();
                            dg.DataSource = ds.Tables[0];
                            dg.DataBind();
                            dg.RenderControl(htw);
                            response.Flush();
                            response.Write(sw.ToString());
                            response.End();
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

            //string filename = "DownloadExcelFile.xls";
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    // string filename = "DownloadExcelFile.xls";
            //    System.IO.StringWriter tw = new System.IO.StringWriter();
            //    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            //    DataGrid dgGrid = new DataGrid();
            //    dgGrid.DataSource = ds.Tables[0];
            //    dgGrid.DataBind();
            //    //Get the HTML for the control.
            //    dgGrid.RenderControl(hw);
            //    //Write the HTML back to the browser.
            //    //Response.ContentType = application/vnd.ms-excel;
            //    Response.ContentType = "application/vnd.ms-excel";
            //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            //    this.EnableViewState = false;
            //    Response.Write(tw.ToString());
            //    Response.End();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }

}