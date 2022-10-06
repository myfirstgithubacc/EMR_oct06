using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class MPage_CommonGridPage : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //DAL.DAL objDl;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (!IsPostBack)
            {
                btnNewAlert.Visible = false;
                if (common.myStr(Request.QueryString["CF"]) == "PTA" && common.myStr(Request.QueryString["PF"]).ToUpper() != "PHA")
                {
                    if (common.myStr(Session["EmployeeType"]) == "D")
                        btnNewAlert.Visible = true;
                }
                if (common.myStr(Request.QueryString["Ward"]) == "Y")
                {
                    btnNewAlert.Visible = false;
                }
                else if (common.myStr(Request.QueryString["AlertType"]) == "A")
                {
                    btnNewAlert.Text = "Patient Allergy";
                    this.Page.Title = "Patient Allergy";
                }
                else if (common.myStr(Request.QueryString["AlertType"]) == "M")
                {
                    btnNewAlert.Text = "Patient Alert";
                    this.Page.Title = "Patient Alert";
                }
                BindPatientDetail();
                fillDetails();
                //lblMsg.Text = "";
            }

            btnNewAlert.Visible = false;
        }
        catch (Exception Ex)
        {
            //lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    private void BindPatientDetail()
    {
        //BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();

        try
        {
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }

            if (common.myLen(common.myStr(Request.QueryString["PId"])) > 0)
            {
                lblPatientDetail.Text = common.setPatientDetails(common.myInt(Request.QueryString["PId"]), 0, common.myStr(Request.QueryString["EncNo"]), string.Empty);
            }
        }
        catch(Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void btnNewAlert_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["CF"]) == "PTA")
        {
            string sEncounterId = common.myStr(Request.QueryString["EId"]) == "" ? common.myStr(Session["EncounterId"]) : common.myStr(Request.QueryString["EId"]);
            string sRegistrationId = common.myStr(Request.QueryString["PId"]) == "" ? common.myStr(Session["RegistrationId"]) : common.myStr(Request.QueryString["PId"]);

            if (common.myStr(Request.QueryString["AlertType"]) == "A")
            {
                RadWindow1.NavigateUrl = "/EMR/Allergy/Allergy.aspx?From=POPUP&";
            }
            else
            {
                RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?From=POPUP&Type=PatientAlert&EncounterId=" + sEncounterId + "&RegistrationId=" + sRegistrationId;
            }
            //if (common.myStr(Request.QueryString["AlertType"]) == "M")
            //{
            //    RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?From=POPUP&Type=PatientAlert&EncounterId=" + sEncounterId + "&RegistrationId=" + sRegistrationId;
            //}
            RadWindow1.Height = 610;
            RadWindow1.Width = 880;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "SearchPatientOnClientClose"; //"SearchPatientOnClientClose";//
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;

            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        }
        else
        {
            return;
        }
    }
    protected void ibtnShowDetails_OnClick(object sender, EventArgs e)
    {
        fillDetails();
    }
    protected void gvDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (common.myStr(Request.QueryString["Ward"]) == "Y")
            {
                e.Row.Cells[0].Text = "Patient Alergy/Alert";
            }
            else if (common.myStr(Request.QueryString["AlertType"]) == "A")
            {
                e.Row.Cells[0].Text = "Patient Alergy";
            }
            else if (common.myStr(Request.QueryString["AlertType"]) == "M")
            {
                e.Row.Cells[0].Text = "Patient Alert";
            }
        }
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            if (common.myStr(Request.QueryString["AlertType"]) != "A")
            {
                e.Row.Cells[1].Visible = false;
            }
        }
    }
    protected void fillDetails()
    {
        //BaseC.EMR objE = new BaseC.EMR(sConString);
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        int iRegistrationId = common.myInt(Request.QueryString["PId"]) == 0 ? common.myInt(Session["RegistrationId"]) : common.myInt(Request.QueryString["PId"]);
        try
        {
            //ds = objE.GetEMRPatientAlerts(common.myInt(Session["HospitalLocationId"]), iRegistrationId, common.myStr(Request.QueryString["AlertType"]));

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRPatientAlerts";
            APIRootClass.GetEMRPatientAlerts objRoot = new global::APIRootClass.GetEMRPatientAlerts();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
            objRoot.AlertType = common.myStr(Request.QueryString["AlertType"]);
            objRoot.RegistrationId = iRegistrationId;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (ds != null && ds.Tables.Count > 0 &&ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds.Tables[0];
                gvDetails.DataBind();

            }
        }
        catch (Exception Ex)
        {
            //lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            //objE = null;
            dt.Dispose();
            ds.Dispose();
        }
    }
}
