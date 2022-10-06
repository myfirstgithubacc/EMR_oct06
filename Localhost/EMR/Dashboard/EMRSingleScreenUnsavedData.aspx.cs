using Newtonsoft.Json;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class EMR_Dashboard_EMRSingleScreenUnsavedData : System.Web.UI.Page
{
    // private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            dvDelete.Visible = false;
            Session["SelectedEncounterFromUnsave"] = null;

            bindDetails();
        }
    }

    private void bindDetails()
    {
        DataSet ds = new DataSet();
       // BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getEMRSingleScreenDataInTransitList";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.iFacilityId = common.myInt(Session["FacilityId"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //  ds = objEmr.getEMRSingleScreenDataInTransitList(common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]));
            gvDetails.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                //DataRow DR = ds.Tables[0].NewRow();
                //ds.Tables[0].Rows.Add(DR);
                gvDetails.DataSource = null;

            }
            
            gvDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            //objEmr = null;
        }
    }

    protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //BaseC.Patient bC = new BaseC.Patient(sConString);
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataSet dsPatientDetail = new DataSet();

        try
        {
            int TransitId = common.myInt(e.CommandArgument);
            if (TransitId > 0)
            {
                if (common.myStr(e.CommandName).ToUpper().Equals("ENCOUNTERSELECT"))
                {
                    LinkButton lbt = (LinkButton)e.CommandSource;
                    GridViewRow grvRow = (GridViewRow)((DataControlFieldCell)lbt.Parent).Parent;

                    HiddenField hdnRegistrationId = (HiddenField)grvRow.FindControl("hdnRegistrationId");
                    HiddenField hdnEncounterId = (HiddenField)grvRow.FindControl("hdnEncounterId");
                    HiddenField hdnOPIP = (HiddenField)grvRow.FindControl("hdnOPIP");
                    HiddenField hdnDoctorId = (HiddenField)grvRow.FindControl("hdnDoctorId");
                    HiddenField hdnAppointmentId = (HiddenField)grvRow.FindControl("hdnAppointmentId");
                    HiddenField hdnGender = (HiddenField)grvRow.FindControl("hdnGender");
                    HiddenField hdnEMRStatus = (HiddenField)grvRow.FindControl("hdnEMRStatus");
                    HiddenField hdnMedicalAlert = (HiddenField)grvRow.FindControl("hdnMedicalAlert");
                    HiddenField hdnAllergiesAlert = (HiddenField)grvRow.FindControl("hdnAllergiesAlert");
                    HiddenField hdnPackageName = (HiddenField)grvRow.FindControl("hdnPackageName");
                    HiddenField hdnIVFId = (HiddenField)grvRow.FindControl("hdnIVFId");
                    HiddenField hdnIVFNo = (HiddenField)grvRow.FindControl("hdnIVFNo");

                    Label lblAgeGender = (Label)grvRow.FindControl("lblAgeGender");
                    Label lblRegistrationNo = (Label)grvRow.FindControl("lblRegistrationNo");
                    Label lblEncounterNo = (Label)grvRow.FindControl("lblEncounterNo");
                    Label lblEncounterDate = (Label)grvRow.FindControl("lblEncounterDate");

                    Session["AgeGender"] = common.myStr(lblAgeGender.Text);
                    Session["RegistrationNo"] = common.myStr(lblRegistrationNo.Text);
                    Session["EncounterNo"] = common.myStr(lblEncounterNo.Text);
                    Session["EncounterDate"] = common.myStr(lblEncounterDate.Text);

                    Session["RegistrationId"] = common.myStr(hdnRegistrationId.Value);
                    Session["EncounterId"] = common.myStr(hdnEncounterId.Value);
                    Session["OPIP"] = common.myStr(hdnOPIP.Value);
                    Session["DoctorId"] = common.myStr(hdnDoctorId.Value);
                    Session["AppointmentId"] = common.myStr(hdnAppointmentId.Value);
                    Session["Gender"] = common.myStr(hdnGender.Value);
                    Session["Status"] = common.myStr(hdnEMRStatus.Value);
                    Session["MedicalAlert"] = common.myStr(hdnMedicalAlert.Value);
                    Session["AllergiesAlert"] = common.myStr(hdnAllergiesAlert.Value);
                    Session["PackageName"] = common.myStr(hdnPackageName.Value);
                    Session["IVFId"] = common.myStr(hdnIVFId.Value);
                    Session["IVFNo"] = common.myStr(hdnIVFNo.Value);

                    #region PatientDetailStringOP
                    Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                    Session["PatientDetailString"] = null;

                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getEMRPatientDetails";
                    APIRootClass.getEMRPatientDetails objRoot = new global::APIRootClass.getEMRPatientDetails();
                    objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                    objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                    objRoot.RegistrationNo = common.myStr(Session["RegistrationNo"]);
                    objRoot.EncounterNo = common.myStr(Session["EncounterNo"]);
                    objRoot.UserId = common.myInt(Session["UserId"]);
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
                    Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = objDs;

                    //Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(Session["RegistrationNo"]),
                    //    common.myStr(Session["EncounterNo"]), common.myInt(Session["UserId"]));
                    dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
                    if (dsPatientDetail.Tables.Count > 0)
                    {
                        if (dsPatientDetail.Tables[0].Rows.Count > 0)
                        {
                            string sRegNoTitle = Resources.PRegistration.regno;
                            string sDoctorTitle = Resources.PRegistration.Doctor;
                            string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                            Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + 
                                ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"]) + "</span>"
                             + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                             + "&nbsp;Enc #:" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
                             + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
                             + DateTitle + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"
                             //+ "&nbsp;Bed:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]) + "</span>"
                             //+ "&nbsp;Ward:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]) + "</span>"
                             + "&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                             + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"
                             + "</b>";
                        }
                    }

                    #endregion

                    Session["SelectedEncounterFromUnsave"] = "1";
                    if (common.myStr(Request.QueryString["FROM"]).Equals("FINDPATIENT"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
                    }
                    else
                    {
                        Response.Redirect("~/EMR/Dashboard/PatientDashboardForDoctor.aspx", false);
                    }
                }
                else if (common.myStr(e.CommandName).ToUpper().Equals("TRANSITDELETE"))
                {
                    dvDelete.Visible = true;
                    ViewState["TransiteId"] = TransitId;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            //bC = null;
            //objEMR = null;
            ds.Dispose();
            dsPatientDetail.Dispose();
        }
    }

    protected void btnYes_OnClick(object sender, EventArgs e)
    {
       // BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        int TransitId = common.myInt(ViewState["TransiteId"]);

        try
        {
            if (TransitId == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Record not selected !";
                dvDelete.Visible = false;
                return;
            }

            if (dvDelete.Visible && TransitId > 0)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/InActiveSingleScreenDataInTransit";
                APIRootClass.InActiveSingleScreenDataInTransit objRoot = new global::APIRootClass.InActiveSingleScreenDataInTransit();
                objRoot.TransitId = TransitId;
                objRoot.Userid = common.myInt(Session["UserId"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);

                int intVal =common.myInt(sValue);

               // int intVal = objEMR.InActiveSingleScreenDataInTransit(TransitId, common.myInt(Session["UserId"]));
                if (intVal.Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = "Data deleted...";

                    bindDetails();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
           // objEMR = null;
            dvDelete.Visible = false;
            ViewState["TransiteId"] = null;
        }
    }

    protected void btnNo_OnClick(object sender, EventArgs e)
    {
        dvDelete.Visible = false;
    }




}
