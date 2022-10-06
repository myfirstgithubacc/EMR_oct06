using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class EMR_Templates_T_ConsentTemplate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            BindPatientHiddenDetailsEncounterNo(common.myStr(Session["RegistrationNo"]));
            BindEmployeeSignature(common.myStr(Session["EmployeeId"]));
            ShowTemplateData(common.myInt(Request.QueryString["TemplateId"]));



        }
    }


    protected void ShowTemplateData(int TemplateId)
    {

        try
        {
            if (common.myInt(Request.QueryString["TemplateId"]) > 0)
            {
                DataSet ds = new DataSet();
                Hashtable hshIn = new Hashtable();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                //hshIn.Add("intRegistrationId", common.myInt(Session["RegistrationId"]).ToString());
                hshIn.Add("intTemplateId", common.myInt(TemplateId));

                ds = dl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetMasterConsentForm", hshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Form12.InnerHtml = common.myStr(ds.Tables[0].Rows[0]["FormatText"]);

                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindPatientHiddenDetailsEncounterNo(string RegistrationNo)
    {
        DataSet ds = new DataSet();
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);

        DataSet dsEMRBilling = new DataSet();
        try
        {
            if (RegistrationNo != "")
            {
                ds = bC.GetPatientRecord(common.myInt(RegistrationNo), common.myStr(null));
                //ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, 0, common.myInt(Session["UserId"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            hdnFormName.Value = common.myStr(Request.QueryString["TemplateName"]);

                            hdnGender.Value = common.myStr(ds.Tables[0].Rows[0]["AgeGender"]).Split('/')[1];
                            hdnAge.Value = common.myStr(ds.Tables[0].Rows[0]["AgeGender"]).Split('/')[0];
                            hdnPatientName.Value = common.myStr(ds.Tables[0].Rows[0]["FirstName"]);
                            hdnMRN.Value = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                            hdnFullPatName.Value = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
                            hdnEmergencyName.Value = common.myStr(ds.Tables[0].Rows[0]["NotificationName"]);
                            hdntelephone.Value = common.myStr(ds.Tables[0].Rows[0]["NotificationPhoneNo"]);
                            //hdnRelation.Value = common.myStr(ds.Tables[0].Rows[0]["KinName"]);
                            // Akshay_19072022_Tirathram
                            hdnRelation.Value = common.myStr(ds.Tables[0].Rows[0]["EmergencyKinRelationId"]);
                            hdnRegDate.Value = common.myStr(ds.Tables[0].Rows[0]["RegistrationDate"]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            bParse = null;
            bC = null;
            dsEMRBilling.Dispose();
        }
    }
    protected void BindEmployeeSignature(string EmployeeID)
    {
        DataSet ds = new DataSet();
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.DoctorAccounting bC = new BaseC.DoctorAccounting(sConString);

        DataSet dsEMRBilling = new DataSet();
        try
        {
            if (EmployeeID != "")
            {
                ds = bC.GetDoctorDetail(common.myInt(EmployeeID), common.myInt(Session["HospitalLocationID"]));
                //ds = bC.GetDoctorDetail(common.myInt(EmployeeID), common.myInt(Session["HospitalLocationID"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            hdnEMPID.Value = common.myStr(ds.Tables[0].Rows[0]["Id"]);
                            hdnEmployeeName.Value = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]);

                            if (!common.myStr(ds.Tables[0].Rows[0]["SignatureImage"]).Equals(""))
                            {
                                byte[] myImageByteArrayData = (byte[])ds.Tables[0].Rows[0]["SignatureImage"];
                                string myImageBase64StringData = Convert.ToBase64String(myImageByteArrayData);
                                hdnEmpSignatureImage.Value = "data:image/png;base64," + myImageBase64StringData;
                            }
                            else
                            {
                                hdnEmpSignatureImage.Value = "../../Img/NoImage.jpg";
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            bParse = null;
            bC = null;
            dsEMRBilling.Dispose();
        }
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            string sContentWordProcessor = string.Empty;
            StringBuilder sbTableBorderStyle = new StringBuilder();
            if (hdntext.Value != null)
            {
                sbTableBorderStyle.Append(hdntext.Value.ToString());
            }
            else
            {
                sContentWordProcessor = "";
            }
            sContentWordProcessor = Convert.ToString(sbTableBorderStyle);
            Hashtable hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intId", common.myInt(0));
            HshIn.Add("Finalize", common.myInt(0));
            HshIn.Add("intFacilityId", common.myInt(Session["FacilityID"]).ToString());
            HshIn.Add("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            HshIn.Add("RegistrationId", common.myInt(Session["RegistrationId"]).ToString());
            HshIn.Add("EncounterId", common.myInt(Session["EncounterId"]).ToString());
            HshIn.Add("ConsentTemplateId", common.myInt(Request.QueryString["TemplateId"]));
            HshIn.Add("ConsentFormText", common.myStr(sContentWordProcessor).Trim());
            HshIn.Add("EncodedBy", common.myInt(Session["UserId"]));
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus1", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientConsentForms", HshIn, HshOut);

            SqlConnection cn = new SqlConnection(sConString);
            cn.Open();

            string sql = "UPDATE EMRPatientConsentForms SET ConsentFormText=N'" + sContentWordProcessor + "' WHERE Id = " + HshOut["chvErrorStatus1"].ToString() + "";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertmsg", "alert('" + HshOut["chvErrorStatus"].ToString() + "');", true);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent('" + common.myStr(Request.QueryString["From"]) + "');", true);

            cn.Close();
            sql = string.Empty;
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }

    }
    protected void btnFinalized_Click(object sender, EventArgs e)
    {
        try
        {
            string sContentWordProcessor = string.Empty;
            StringBuilder sbTableBorderStyle = new StringBuilder();
            if (hdntext.Value != null)
            {
                sbTableBorderStyle.Append(hdntext.Value.ToString());
            }
            else
            {
                sContentWordProcessor = "";
            }
            sContentWordProcessor = Convert.ToString(sbTableBorderStyle);
            Hashtable hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intId", common.myInt(0));
            HshIn.Add("Finalize", common.myInt(1));
            HshIn.Add("intFacilityId", common.myInt(Session["FacilityID"]).ToString());
            HshIn.Add("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]).ToString());
            HshIn.Add("RegistrationId", common.myInt(Session["RegistrationId"]).ToString());
            HshIn.Add("EncounterId", common.myInt(Session["EncounterId"]).ToString());
            HshIn.Add("ConsentTemplateId", common.myInt(Request.QueryString["TemplateId"]));
            HshIn.Add("ConsentFormText", common.myStr(sContentWordProcessor).Trim());
            HshIn.Add("EncodedBy", common.myInt(Session["UserId"]));
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus1", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientConsentForms", HshIn, HshOut);

            SqlConnection cn = new SqlConnection(sConString);
            cn.Open();

            string sql = "UPDATE EMRPatientConsentForms SET ConsentFormText=N'" + sContentWordProcessor + "' WHERE Id = " + HshOut["chvErrorStatus1"].ToString() + "";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertmsg", "alert('" + HshOut["chvErrorStatus"].ToString() + "');", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent('" + common.myStr(Request.QueryString["From"]) + "');", true);
            cn.Close();
            sql = string.Empty;
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
}