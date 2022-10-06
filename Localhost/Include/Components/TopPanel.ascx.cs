using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.Data;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using BaseC;

public partial class Include_Components_TopPanel : System.Web.UI.UserControl
{
    clsExceptionLog objException = new clsExceptionLog();
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    //private Hashtable hshInput;
    //BaseC.ParseData Parse = new BaseC.ParseData();
    Hashtable hsNewPage = new Hashtable();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "")
            {
                if (common.myInt(Session["RegistrationID"]) > 0)
                {
                    FillPatientData();
                    WardManagement objw = new WardManagement();
                    DataSet dsStatus = objw.GetPatientAcknowledgementStatus(common.myInt(Session["EncounterId"]));
                    //yogesh 07/09/2022
                    if(dsStatus.Tables.Count>0)
                    {

                        if (dsStatus.Tables[0].Rows.Count > 0)
                        {
                            lblpatinetAckDataTime.Text = dsStatus.Tables[0].Rows[0]["AcknowledgedDate"].ToString();
                            lblAcknowledgedBy.Text = dsStatus.Tables[0].Rows[0]["EnteredBy"].ToString();
                        }
                        dsStatus.Dispose();
                        objw = null;
                    }


                }
            }
        }
    }

    public void ShowPatientDetails(int RegId)
    {
        try
        {
            DataSet ds = new DataSet();
            Hashtable HashIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            StringBuilder strSQL = new StringBuilder();
            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmdTemp;
            SqlParameter prm2;
            strSQL.Append("uspGetPatientImage_Img");
            cmdTemp = new SqlCommand(strSQL.ToString(), con);
            cmdTemp.CommandType = CommandType.StoredProcedure;
            prm2 = new SqlParameter();
            prm2.ParameterName = "@RegistrationId";
            prm2.Value = RegId;
            prm2.SqlDbType = SqlDbType.Int;
            prm2.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm2);
            con.Open();
            SqlDataReader dr = cmdTemp.ExecuteReader();
            if (dr.HasRows == true)
            {
                dr.Read();
                Stream strm;
                Object img = dr["PatientImage"];
                String FileName = dr["ImageType"].ToString();
                if (FileName != "")
                {
                    strm = new MemoryStream((byte[])img);
                    byte[] buffer = new byte[strm.Length];
                    int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                    FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                    fs.Write(buffer, 0, byteSeq);
                    fs.Dispose();
                    PatientImage.ImageUrl = "/PatientDocuments/PatientImages/" + FileName;
                    //  File.Delete(Server.MapPath("/PatientDocuments/PatientImages/") + FileName);
                }
            }
            else
            {
                PatientImage.ImageUrl = "~/Images/patientLeft.jpg";
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            Response.Write("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
    }
    protected void FillPatientData()
    {
        DataSet ds = new DataSet();
        try
        {


            //Hashtable hshInput = new Hashtable();
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            //hshInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            //hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            //hshInput.Add("@intRegistrationId", common.myInt(Session["RegistrationID"]));
            //hshInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
            //int intPatientID = common.myInt(Session["RegistrationID"]);

            //SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "uspGetPatientDetails", hshInput);

            APIRootClass.GetPatientDetails objRoot = new global::APIRootClass.GetPatientDetails();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
            objRoot.RegistrationNo = common.myInt(Session["RegistrationNo"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            string ServiceURL = WebAPIAddress.ToString() + "api/Common/GetPatientDetails";
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

              
                // DataSet ds = new DataSet();
                Hashtable HashIn = new Hashtable();
               // DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                if (ds.Tables[0].Rows[0]["PatientImage"].ToString() != "")
                {
                    ShowPatientDetails(objRoot.RegistrationId);
                }
                    //while (dr.Read())
                    //{
                    //if (ds.Tables[0].Rows[0]["PatientImage"].ToString() != "")
                    //{
                    //    Stream strm;
                    //    Object img = ds.Tables[0].Rows[0]["PatientImage"];
                    //    String FileName = ds.Tables[0].Rows[0]["ImageType"].ToString();
                    ////strm = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(img.ToString()));
                    ////byte[] buffer = new byte[strm.Length];
                    ////Stream ms = new MemoryStream(buffer);
                    ////int byteSeq = ms.Read(buffer, 0, System.Convert.ToInt32(ms.Length));
                    ////FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                    ////fs.Write(buffer, 0, byteSeq);
                    ////fs.Dispose();
                    //strm = new MemoryStream((byte[])img);
                    //byte[] buffer = new byte[strm.Length];
                    //int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                    //FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                    //fs.Write(buffer, 0, byteSeq);
                    //fs.Dispose();
                    

                    //PatientImage.ImageUrl = "/PatientDocuments/PatientImages/" + FileName;
                    //}
                    else
                    {
                        PatientImage.ImageUrl = "~/Images/no_photo.jpg";
                    }

                //added by bhakti
                Session["PatientCompanyType"] = ds.Tables[0].Rows[0]["PatientCompanyType"].ToString();
                Session["IsOffline"] = ds.Tables[0].Rows[0]["IsOffline"].ToString();

                lblCId.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                    lblDob.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
                    lblAge.Text = ds.Tables[0].Rows[0]["Age"].ToString();
                    lblGender.Text = ds.Tables[0].Rows[0]["Gender"].ToString();

                    lblCrntEnSts.Text = ds.Tables[0].Rows[0]["EncounterStatus"].ToString();
                    lblEncNo.Text = "Encounter #" + ds.Tables[0].Rows[0]["EncounterNo"].ToString();
                    lblEncDate.Text = ds.Tables[0].Rows[0]["EncounterDate"].ToString();
                    lblVisitType.Text = ds.Tables[0].Rows[0]["VisitType"].ToString();
                    lblLoc.Text = ds.Tables[0].Rows[0]["FacilityName"].ToString();


                    lblHphone.Text = ds.Tables[0].Rows[0]["PhoneHome"].ToString();
                    lblWphome.Text = ds.Tables[0].Rows[0]["WorkNumber"].ToString();
                    lblMphone.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString();

                    //lblRefPrvdr.Text = dr["RefferingProvider"].ToString();

                    lblApptNote.Text = ds.Tables[0].Rows[0]["Note"].ToString();
                    lblVtCrPrvdr.Text = ds.Tables[0].Rows[0]["DoctorName"].ToString();
                //Change Treating Consultant
                lblTreatingConsultant.Text = ds.Tables[0].Rows[0]["TreatingConsultant"].ToString();
                //Change Treating Consultant
                    lblAcCategory.Text = ds.Tables[0].Rows[0]["AccountCategory"].ToString();
                    lblAcType.Text = ds.Tables[0].Rows[0]["AccountTypeName"].ToString();
                    lblPlnType.Text = ds.Tables[0].Rows[0]["PlanName"].ToString();
                    lblPayer.Text = ds.Tables[0].Rows[0]["Payername"].ToString();
                    lblSponsor.Text = ds.Tables[0].Rows[0]["SponsorName"].ToString();
                    lblNetworkName.Text = ds.Tables[0].Rows[0]["CardType"].ToString();
                    lblEncounterCompanyType.Text = ds.Tables[0].Rows[0]["PayType"].ToString();
                    lblRegInsCardId.Text = ds.Tables[0].Rows[0]["RegInsCardId"].ToString();
                    lblNotificationName.Text = ds.Tables[0].Rows[0]["NotificationName"].ToString();

                     
                //Session["IsMedicalAlert"] = "";
                //dr["MedicalAlert"].ToString();
                lblPackageVisit.Visible = false;
                    lblPackageVisit.Text = "";
                    if (common.myStr(ds.Tables[0].Rows[0]["PackageName"]) != "")
                    {
                        lblPackageVisit.Text = "Package Visit : " + common.myStr(ds.Tables[0].Rows[0]["PackageName"]);
                        lblPackageVisit.Visible = true;
                    }

                }
            //}
            //dr.Close();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    
}
