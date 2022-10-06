using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EMR_Problems_ChifComplaint : System.Web.UI.Page
{
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        switch (common.myStr(Request.QueryString["From"]).ToUpper())
        {
            case "POPUP":
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
                break;
            case "STATICTEMPLATE":
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
                break;
            default:
                Page.MasterPageFile = "/Include/Master/EMRMaster.master";
                break;
        }

        if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillPatientData();
        }
    }
    protected void FillPatientData()
    {
        DataSet ds = new DataSet();
        try
        {

            APIRootClass.GetPatientDetails objRoot = new global::APIRootClass.GetPatientDetails();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
            objRoot.RegistrationNo = common.myInt(Session["RegistrationNo"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            string ServiceURL = WebAPIAddress.ToString() + "api/Common/GetEmrPatientDetail";
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //if (dr.HasRows == true)
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

               // while (dr.Read())
                //{
                    //if (ds.Tables[0].Rows[0]["PatientImage"].ToString() != "")
                    //{
                    //    Stream strm;
                    //    Object img = ds.Tables[0].Rows[0]["PatientImage"];
                    //    String FileName = ds.Tables[0].Rows[0]["ImageType"].ToString();
                    //    strm = new MemoryStream((byte[])img);
                    //    byte[] buffer = new byte[strm.Length];
                    //    int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                    //    FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                    //    fs.Write(buffer, 0, byteSeq);
                    //    fs.Dispose();
                    //    ////PatientImage.ImageUrl = "/PatientDocuments/PatientImages/" + FileName;
                    //}
                    //else
                    //{
                    //    // PatientImage.ImageUrl = "~/Images/no_photo.jpg";
                    //}
                    Session["DoctorId"] = ds.Tables[0].Rows[0]["DoctorId"].ToString();
                lblCId.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                lblPName.Text = ds.Tables[0].Rows[0]["PatientName"].ToString();

                lblGenderAge.Text = ds.Tables[0].Rows[0]["Gender1"].ToString()+ "/"+ ds.Tables[0].Rows[0]["Age"].ToString();
                // lblDob.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
                // lblAge.Text = ds.Tables[0].Rows[0]["Age"].ToString();
                //lblGender.Text = ds.Tables[0].Rows[0]["Gender"].ToString();

                //lblCrntEnSts.Text = ds.Tables[0].Rows[0]["EncounterStatus"].ToString();
                //lblEncNo.Text = "Encounter #" + ds.Tables[0].Rows[0]["EncounterNo"].ToString();
                lblEncDate.Text = ds.Tables[0].Rows[0]["EncounterDate"].ToString();
                //lblVisitType.Text = ds.Tables[0].Rows[0]["VisitType"].ToString();
                //lblLoc.Text = ds.Tables[0].Rows[0]["FacilityName"].ToString();


                //lblHphone.Text = ds.Tables[0].Rows[0]["PhoneHome"].ToString();
                //lblWphome.Text = ds.Tables[0].Rows[0]["WorkNumber"].ToString();
                //lblMphone.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString();

                //lblRefPrvdr.Text = dr["RefferingProvider"].ToString();

                // lblApptNote.Text = ds.Tables[0].Rows[0]["Note"].ToString();
                lblVtCrPrvdr.Text = "Dr." + ds.Tables[0].Rows[0]["DoctorName"].ToString();
                //lblAcCategory.Text = ds.Tables[0].Rows[0]["AccountCategory"].ToString();
                //lblAcType.Text = ds.Tables[0].Rows[0]["AccountTypeName"].ToString();
                //lblPlnType.Text = ds.Tables[0].Rows[0]["PlanName"].ToString();
                //lblPayer.Text = ds.Tables[0].Rows[0]["Payername"].ToString();
                //lblSponsor.Text = ds.Tables[0].Rows[0]["SponsorName"].ToString();
                //lblNetworkName.Text = ds.Tables[0].Rows[0]["CardType"].ToString();
                //lblEncounterCompanyType.Text = ds.Tables[0].Rows[0]["PayType"].ToString();
                //lblRegInsCardId.Text = ds.Tables[0].Rows[0]["RegInsCardId"].ToString();
                //lblNotificationName.Text = ds.Tables[0].Rows[0]["NotificationName"].ToString();


                //Session["IsMedicalAlert"] = "";
                //dr["MedicalAlert"].ToString();
                //lblPackageVisit.Visible = false;
                //lblPackageVisit.Text = "";
                //if (common.myStr(ds.Tables[0].Rows[0]["PackageName"]) != "")
                //{
                //    lblPackageVisit.Text = "Package Visit : " + common.myStr(ds.Tables[0].Rows[0]["PackageName"]);
                //    lblPackageVisit.Visible = true;
                //}

            }
            //}
            //dr.Close();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            // objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    protected void lnkChiefComplaint_OnClick(object sender, EventArgs e)
    {
        try
        {
            // ClearMessageControl();
            //autoSaveDataInTransit(true);
            RadWindow1.NavigateUrl = "~/EMR/Problems/ViewPastPatientProblems.aspx?MP=NO";
            RadWindow1.Height = 600;
            RadWindow1.Width = 990;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
}