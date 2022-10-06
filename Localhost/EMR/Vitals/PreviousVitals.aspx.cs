using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using Telerik.Web.UI;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
public partial class EMR_Vitals_PreviousVitals : System.Web.UI.Page
{
    //String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();

    clsExceptionLog objException = new clsExceptionLog();
   // DAL.DAL dl = new DAL.DAL();
    //BaseC.EMRVitals objv;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
       // Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["GridData"] = null;

            if (common.myStr(Request.QueryString["From"]) == "POPUP" || common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate")
            {
                btnBack.Visible = false;
                btnClose.Visible = true;
            }
            else
            {
                btnBack.Visible = true;
                btnClose.Visible = false;
            }

            dtpfromDate.DateInput.DateFormat = common.myStr(Session["OutputDateformat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateformat"]);

            BindVitalSignType();
            bindPreviousVitals();
        }
    }

    void BindVitalSignType()
    {
        try
        {
            //objv = new BaseC.EMRVitals(sConString);
            DataSet ds = new DataSet();
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetVitalSignType";
            APIRootClass.vital vital = new global::APIRootClass.vital();
            vital.Encounterid = common.myInt(Session["encounterid"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(vital);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            //ds = objv.GetVitalSignType();
            //  ds=objv.GetVitalSignName("", common.myInt(Session["HospitalLocationId"]));
            ddlVitalSigntype.DataSource = ds;
            ddlVitalSigntype.DataTextField = "VitalSignName";
            ddlVitalSigntype.DataValueField = "VitalId";
            ddlVitalSigntype.DataBind();
            ddlVitalSigntype.Items.Insert(0, new RadComboBoxItem("Select All", "0"));
            ddlVitalSigntype.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void bindPreviousVitals()
    {
        try
        {
            DataSet objDs = new DataSet();
            DataTable dt1 = new DataTable();

            DateTime FrmDate = common.myDate(dtpfromDate.SelectedDate);
            DateTime ToDate = common.myDate(dtpToDate.SelectedDate);

            // objv = new BaseC.EMRVitals(sConString);

            if (common.myStr(dtpfromDate.SelectedDate) != "" && common.myStr(dtpToDate.SelectedDate) != "")
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetPatientPreviousVitalHistory";
                APIRootClass.PatientPreviousVitalsHistory objRoot = new global::APIRootClass.PatientPreviousVitalsHistory();
                objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
                objRoot.FacId = common.myInt(Session["FacilityId"]);
                objRoot.RegId = common.myInt(Session["RegistrationId"]);
                objRoot.EncId = 0;
                objRoot.ViewType = 2;
                objRoot.DRange = "";
                objRoot.FDate = common.myStr(FrmDate.ToString("yyyy/MM/dd"));
                objRoot.TDate = common.myStr(ToDate.ToString("yyyy/MM/dd"));
                objRoot.VitalSignId = common.myInt(ddlVitalSigntype.SelectedValue);
                objRoot.Abnormal = common.myBool(chkAbNormal.Checked);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                //objDs = objv.GetPatientPreviousVitals(common.myInt(Session["HospitalLocationId"]),
                //                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                //                0, 2, "", common.myStr(FrmDate.ToString("yyyy/MM/dd")),
                //                common.myStr(ToDate.ToString("yyyy/MM/dd")),
                //                common.myInt(ddlVitalSigntype.SelectedValue), common.myBool(chkAbNormal.Checked));
            }
            else
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRVitals/GetPatientPreviousVitalHistory";
                APIRootClass.PatientPreviousVitalsHistory objRoot = new global::APIRootClass.PatientPreviousVitalsHistory();
                objRoot.HospId = common.myInt(Session["HospitalLocationId"]);
                objRoot.FacId = common.myInt(Session["FacilityId"]);
                objRoot.RegId = common.myInt(Session["RegistrationId"]);
                objRoot.EncId = 0;
                objRoot.ViewType = 2;
                objRoot.DRange = common.myStr(ddldateRange.SelectedValue);
                objRoot.FDate = "";
                objRoot.TDate = "";
                objRoot.VitalSignId = common.myInt(ddlVitalSigntype.SelectedValue);
                objRoot.Abnormal = common.myBool(chkAbNormal.Checked);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                //objDs = objv.GetPatientPreviousVitals(common.myInt(Session["HospitalLocationId"]),
                //                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                //                0, 2, common.myStr(ddldateRange.SelectedValue), "", "",
                //                common.myInt(ddlVitalSigntype.SelectedValue), common.myBool(chkAbNormal.Checked));
            }

            if (objDs.Tables[0].Rows.Count > 0)
            {
                lblMessage.Text = "";
                gvPrevious.DataSource = objDs;

                //ViewState["GvClumnCount"] = objDs.Tables[0].Columns.Count.ToString();//gvPrevious.Columns.Count;
            }
            else
            {
                objDs = new DataSet();
                DataTable dt = new DataTable();

                DataColumn dc1 = new DataColumn("Vital Date");
                DataColumn dc2 = new DataColumn("WT");
                DataColumn dc3 = new DataColumn("BMI");
                DataColumn dc4 = new DataColumn("BSA");
                DataColumn dc5 = new DataColumn("Enterd By");

                dt.Columns.Add(dc1);
                dt.Columns.Add(dc2);
                dt.Columns.Add(dc3);
                dt.Columns.Add(dc4);
                dt.Columns.Add(dc5);

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                objDs.Tables.Add(dt);

                gvPrevious.DataSource = objDs;
            }

            ViewState["GridData"] = objDs.Tables[0];

            gvPrevious.DataBind();

            if (objDs.Tables.Count > 0)
            {
                lblNoOfRows.Text = "Total " + objDs.Tables[0].Rows.Count.ToString() + " Record(s)";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvPrevious_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPrevious.PageIndex = e.NewPageIndex;
        bindPreviousVitals();
    }

    protected void ddldateRange_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (ddldateRange.SelectedValue == "6")
            {
                pnlDatarng.Visible = true;

                dtpfromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpfromDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpfromDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

                dtpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpToDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpToDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));
            }
            else
            {
                pnlDatarng.Visible = false;

                dtpfromDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpfromDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpfromDate.DateInput.Text = "";
                dtpfromDate.SelectedDate = null;

                dtpToDate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
                dtpToDate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
                dtpToDate.DateInput.Text = "";
                dtpToDate.SelectedDate = null;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void imgOkPrevValue_Click(object sender, EventArgs e)
    {
        bindPreviousVitals();
    }

    protected void btnVitalChart_Click(object sender, EventArgs e)
    {
        //  Response.Redirect("/EMR/Vitals/GrowthChart.aspx?MP=NO",false);
        RadWindowForNew.NavigateUrl = "/EMR/Vitals/Vitalgraph.aspx?Value=0&Name=";
        RadWindowForNew.Height = 650;
        RadWindowForNew.Width = 1000;
        RadWindowForNew.Top = 0;
        RadWindowForNew.Left = 0;

        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnVital_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EMR/Vitals/Vitals.aspx?Mpg=P14", false);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("/EMR/Vitals/Vitals.aspx");
    }

    protected void gvPrevious_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Header
                || e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int idx = 0; idx < e.Row.Cells.Count; idx++)
                {
                    if (e.Row.Cells[idx].Text.Contains("_")
                        || common.myStr(e.Row.Cells[idx].Text) == "0"
                        || common.myStr(e.Row.Cells[idx].Text) == "1"
                        || common.myStr(e.Row.Cells[idx].Text) == "VitalEntryDate1"
                        || common.myStr(e.Row.Cells[idx].Text) == "TemplateFieldId")
                    {
                        e.Row.Cells[idx].Visible = false;

                        //if (e.Row.RowType == DataControlRowType.DataRow)
                        //{
                        //    if (common.myStr(e.Row.Cells[idx].Text) == "1")
                        //    {
                        //        e.Row.Cells[idx].ForeColor = System.Drawing.Color.Red;
                        //    }
                        //}
                    }
                    else
                    {
                        e.Row.Cells[idx].Visible = true;
                        //e.Row.Cells[idx].ForeColor = System.Drawing.Color.Blue;
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {

                        e.Row.Cells[1].Visible = false;
                        if (e.Row.Cells.Count > 29)
                        {
                            e.Row.Cells[29].Visible = false;
                        }

                        if (common.myStr(this.gvPrevious.HeaderRow.Cells[idx].Text) != "Vital Date"
                            && common.myStr(this.gvPrevious.HeaderRow.Cells[idx].Text) != "Entered BY"

                            && common.myStr(e.Row.Cells[idx].Text) != "&nbsp;"
                            && common.myStr(e.Row.Cells[idx].Text).Trim() != "")
                        {
                            string HeaderColumn = common.myStr(this.gvPrevious.HeaderRow.Cells[idx].Text);
                            if (HeaderColumn.Equals("LMP"))
                            {
                                e.Row.Cells[idx].Attributes.Add("onclick", "setValue('" + e.Row.Cells[idx].Text + "','" + HeaderColumn + "','0');");
                            }
                            else
                            {
                                e.Row.Cells[idx].Attributes.Add("onclick", "setValue('" + e.Row.Cells[idx].Text + "','" + HeaderColumn + "','1');");
                            }

                            if (common.myStr(e.Row.Cells[idx].Text) == "1")
                            {
                                if (ViewState["GridData"] != null)
                                {
                                    DataTable tbl = (DataTable)ViewState["GridData"];

                                    string strCol = HeaderColumn.Substring(0, HeaderColumn.IndexOf("_"));

                                    e.Row.Cells[tbl.Columns[strCol].Ordinal].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }

                    }

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

}
