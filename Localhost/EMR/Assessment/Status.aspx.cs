using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Assessment_Status : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("/login.aspx?Logout=1", false);
        }
        if (!IsPostBack)
        {

            string pagesource = common.myStr(Request.QueryString["Source"]);
            if (pagesource == "pd" || pagesource == "COMPLAINT")
            {
                ltrName.Text = "Search keyword";
            }
            bindgridview();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsinput = new Hashtable();
            string pagesource = common.myStr(Request.QueryString["Source"]);

            string strsave = "";
            //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);

            if (pagesource == "pd") //provisional diagnosis
            {
                //strsave = objDiag.EMRSaveProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(0),
                //                        common.myStr(txtstatus.Text.Trim()), 1, common.myInt(Session["UserId"]), "DIAGNOSIS");

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProvisionalDiagnosisSearchCodes";
                APIRootClass.SaveProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.SaveProvisionalDiagnosisSearchCodes();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.DiagnosisSearchId = 0;
                objRoot.DiagnosisSearchCode = common.myStr(txtstatus.Text.Trim());
                objRoot.Active = 1;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.KeywordType = "DIAGNOSIS";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                lblMsg.Text = JsonConvert.DeserializeObject<string>(sValue);

                //lblMsg.Text = strsave;
            }
            else if (pagesource == "COMPLAINT") //COMPLAINT
            {
                //strsave = objDiag.EMRSaveProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(0),
                //                        common.myStr(txtstatus.Text.Trim()), 1, common.myInt(Session["UserId"]), "COMPLAINT");
                //lblMsg.Text = strsave;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProvisionalDiagnosisSearchCodes";
                APIRootClass.SaveProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.SaveProvisionalDiagnosisSearchCodes();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.DiagnosisSearchId = 0;
                objRoot.DiagnosisSearchCode = common.myStr(txtstatus.Text.Trim());
                objRoot.Active = 1;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.KeywordType = "COMPLAINT";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                lblMsg.Text = JsonConvert.DeserializeObject<string>(sValue);
            }
            else
            {
                //hsinput.Add("@intHospitallocationId", Session["HospitalLocationID"].ToString());
                //hsinput.Add("@chvStatusName", txtstatus.Text);
                //hsinput.Add("@intEncodedBy", Convert.ToInt32(Session["UserId"].ToString()));

                //obj.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDiagnosisStaus", hsinput);
                //lblMsg.Text = "Data Save !";

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveDiagnosisStaus";
                APIRootClass.SaveDiagnosisStaus objRoot = new global::APIRootClass.SaveDiagnosisStaus();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.StatusName = txtstatus.Text;
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                lblMsg.Text = JsonConvert.DeserializeObject<string>(sValue);

            }
            lblMsg.ForeColor = System.Drawing.Color.Green;
            bindgridview();
            txtstatus.Text = "";
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lstTypeList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int Statusid, encodedby;
            TextBox description = new TextBox();
            TextBox txtdatvisit = new TextBox();

            Statusid = Convert.ToInt32(lstTypeList.DataKeys[e.RowIndex].Value);
            description.Text = ((TextBox)lstTypeList.Rows[e.RowIndex].Cells[0].FindControl("txtstatusname")).Text.Trim();

            if (Statusid == 0)
            {
                lblMsg.Text = "No data found!";
                return;
            }
            else if (description.Text == "")
            {
                lblMsg.Text = "Please enter description!";
                return;
            }

            string pagesource = common.myStr(Request.QueryString["source"]);
            string strsave = "";
            //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);

            if (pagesource == "pd") //provisional diagnosis
            {
                //strsave = objDiag.EMRSaveProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Statusid),
                //                    common.myStr(description.Text.Trim()), 1, common.myInt(Session["UserId"]), "DIAGNOSIS");
                //lblMsg.Text = strsave;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProvisionalDiagnosisSearchCodes";
                APIRootClass.SaveProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.SaveProvisionalDiagnosisSearchCodes();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.DiagnosisSearchId = common.myInt(Statusid);
                objRoot.DiagnosisSearchCode = common.myStr(description.Text.Trim());
                objRoot.Active = 1;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.KeywordType = "DIAGNOSIS";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                lblMsg.Text = JsonConvert.DeserializeObject<string>(sValue);
            }
            else if (pagesource == "COMPLAINT") //COMPLAINT
            {
                //strsave = objDiag.EMRSaveProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Statusid),
                //                    common.myStr(description.Text.Trim()), 1, common.myInt(Session["UserId"]), "COMPLAINT");
                //lblMsg.Text = strsave;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProvisionalDiagnosisSearchCodes";
                APIRootClass.SaveProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.SaveProvisionalDiagnosisSearchCodes();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.DiagnosisSearchId = common.myInt(Statusid);
                objRoot.DiagnosisSearchCode = common.myStr(description.Text.Trim());
                objRoot.Active = 1;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.KeywordType = "COMPLAINT";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                lblMsg.Text = JsonConvert.DeserializeObject<string>(sValue);
            }
            else
            {
                encodedby = Convert.ToInt32(Session["UserId"]);

                //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //Hashtable hsinput = new Hashtable();
                //hsinput.Add("@intStatusId", Statusid);
                //hsinput.Add("@intHospitallocationId", Session["HospitalLocationID"].ToString());
                //hsinput.Add("@chvStatusName", description.Text);

                //hsinput.Add("@intEncodedBy", Convert.ToInt32(Session["UserId"].ToString()));

                //obj.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDiagnosisStaus", hsinput);
                //lblMsg.Text = "Data Updated !";
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveDiagnosisStaus";
                APIRootClass.SaveDiagnosisStaus objRoot = new global::APIRootClass.SaveDiagnosisStaus();
                objRoot.StatusId = Statusid;
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.StatusName = txtstatus.Text;
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                lblMsg.Text = JsonConvert.DeserializeObject<string>(sValue);
            }

            lstTypeList.EditIndex = -1;
            bindgridview();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lstTypeList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lstTypeList.EditIndex = e.NewEditIndex;
        bindgridview();
    }

    protected void lstTypeList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            int Statusid;
            Statusid = Convert.ToInt32(lstTypeList.DataKeys[e.RowIndex].Value);

            if (Statusid == 0)
            {
                lblMsg.Text = "No data found!";
                return;
            }

            string pagesource = common.myStr(Request.QueryString["source"]);
            //string strsave = "";
            //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);

            if (pagesource == "pd") //provisional diagnosis
            {
                //strsave = objDiag.EMRSaveProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Statusid),
                //                    "", 0, common.myInt(Session["UserId"]), "DIAGNOSIS");
                //lblMsg.Text = strsave;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProvisionalDiagnosisSearchCodes";
                APIRootClass.SaveProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.SaveProvisionalDiagnosisSearchCodes();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.DiagnosisSearchId = common.myInt(Statusid);
                objRoot.DiagnosisSearchCode = "";
                objRoot.Active = 1;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.KeywordType = "DIAGNOSIS";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                lblMsg.Text = JsonConvert.DeserializeObject<string>(sValue);
            }
            else if (pagesource == "COMPLAINT") //COMPLAINT
            {
                //strsave = objDiag.EMRSaveProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Statusid),
                //                    "", 0, common.myInt(Session["UserId"]), "COMPLAINT");
                //lblMsg.Text = strsave;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProvisionalDiagnosisSearchCodes";
                APIRootClass.SaveProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.SaveProvisionalDiagnosisSearchCodes();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.DiagnosisSearchId = common.myInt(Statusid);
                objRoot.DiagnosisSearchCode = "";
                objRoot.Active = 1;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.KeywordType = "COMPLAINT";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                lblMsg.Text = JsonConvert.DeserializeObject<string>(sValue);
            }
            else
            {
                //lblMsg.Text = "";
                //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                String strdel = "UPDATE EMRDiagnosisStatusMaster SET Active=0 WHERE StatusId=" + Statusid;
                //obj.ExecuteNonQuery(CommandType.Text, strdel);

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/ExecuteQuery";
                APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
                objRoot.Query = strdel;
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                int intResult = common.myInt(sValue);
            }

            bindgridview();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lstTypeList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void lstTypeList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lstTypeList.EditIndex = -1;
        bindgridview();
    }

    protected void lstTypeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lstTypeList.PageIndex = e.NewPageIndex;
        bindgridview();
    }

    private void bindgridview()
    {
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet objDs;
            objDs = new DataSet();
            string pagesource = common.myStr(Request.QueryString["source"]);

            //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);

            if (pagesource == "pd")
            {
                //objDs = objDiag.GetProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), "DIAGNOSIS");
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetProvisionalDiagnosisSearchCodes";
                APIRootClass.GetProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.GetProvisionalDiagnosisSearchCodes();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.UserId = common.myInt(Session["UserID"]);
                objRoot.KeywordType = "DIAGNOSIS";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
                objDs.Tables[0].Columns[0].ColumnName = "StatusId";
                objDs.Tables[0].Columns[1].ColumnName = "Description";
            }
            else if (pagesource == "COMPLAINT")
            {
                //objDs = objDiag.GetProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), "COMPLAINT");
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetProvisionalDiagnosisSearchCodes";
                APIRootClass.GetProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.GetProvisionalDiagnosisSearchCodes();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.UserId = common.myInt(Session["UserID"]);
                objRoot.KeywordType = "COMPLAINT";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
                objDs.Tables[0].Columns[0].ColumnName = "StatusId";
                objDs.Tables[0].Columns[1].ColumnName = "Description";
            }
            else
            {
                string strSql = "Select StatusId, Description From EMRDiagnosisStatusMaster Where Active=1 and HospitalLocationId IS NULL";
                //objDs = dl.FillDataSet(CommandType.Text, strSql);
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
                APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
                objRoot.Query = strSql;
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            }

            if (objDs.Tables[0].Rows.Count > 0)
            {
                lstTypeList.DataSource = objDs;
                lstTypeList.DataBind();
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindBlankGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StatusId");
            dt.Columns.Add("Description");

            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dt.NewRow();
                dr["StatusId"] = 0;
                dr["Description"] = "";
                dt.Rows.Add(dr);
            }
            lstTypeList.DataSource = dt;
            lstTypeList.DataBind();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}

