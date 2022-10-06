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
using System.Resources;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing;

public partial class Include_Components_TopPanelNurse : System.Web.UI.UserControl
{
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DataSet dsPatientDetail = new DataSet();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //string url = common.myStr(ViewState["PatientImages"]);
        //PatientImage.Attributes.Add("onclick", "getPage(" + url + ")");
        if (!IsPostBack)
        {
            FillPatientData();
            if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "")
            {
                if (common.myInt(Session["RegistrationID"]) > 0)
                // && ((common.myStr(Session["ModuleName"])=="EMR" || common.myInt(Session["ModuleId"])==3) || (common.myStr(Session["ModuleName"])=="Nurse Workbench" || common.myInt(Session["ModuleId"])==44)))
                {
                    FillPatientData();
                    if (PatientImage.ImageUrl.Equals("~/imagesHTML/nophoto.ico"))
                    {
                        PatientImage.Enabled = false;
                    }
                }
            }
            PatientImage_Click(null, null);
        }
    }


    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }

    protected void FillPatientData()
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        try
        {
            if (Request.QueryString["AlertType"] != null)
            {
                if (common.myInt(Session["EncounterId"]) != common.myInt(Request.QueryString["EId"]) && common.myInt(Request.QueryString["EId"]) != 0)
                {
                    // dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    //common.myStr(sRegistrationNo),common.myStr(sEncounterNo), common.myInt(Session["UserId"]));
                }
                else
                {
                    //dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(txtAccountNo.Text),
                    //common.myStr (Session["EncounterNo"]), common.myInt(Session["UserId"]));

                    dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(Session["RegistrationNoReg"]),
                    common.myStr(Session["EncounterNo"]), common.myInt(Session["UserId"]), 0);
                    //if (Session["RegistrationIdReg"] != null)
                    //{
                    //    if (common.myStr(Session["RegistrationIdReg"]) != string.Empty)
                    //    {
                    //        dsPatientDetail = (DataSet)objPatient.GetPatientRecord(common.myInt(Session["RegistrationIdReg"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
                    //    }
                    //}
                }
            }
            else
            {
                dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(Session["RegistrationNoReg"]),
                    common.myStr(Session["EncounterNo"]), common.myInt(Session["UserId"]), 0);
                //if (Session["RegistrationIdReg"] != null)
                //{
                //    if (common.myStr(Session["RegistrationIdReg"]) != string.Empty)
                //    {

                //        dsPatientDetail = (DataSet)objPatient.GetPatientRecord(common.myInt(Session["RegistrationIdReg"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
                //    }
                //}
            }

            if (dsPatientDetail != null)
            {


                //dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
                if (dsPatientDetail.Tables.Count > 0)
                {
                    if (dsPatientDetail.Tables[0].Rows.Count > 0)
                    {
                        if (dsPatientDetail.Tables[0].Rows[0]["PatientImage"].ToString() != "")
                        {
                            try
                            {
                                Stream strm;
                                Object img = dsPatientDetail.Tables[0].Rows[0]["PatientImage"];
                                String FileName = dsPatientDetail.Tables[0].Rows[0]["ImageType"].ToString();
                                strm = new MemoryStream((byte[])img);
                                byte[] buffer = new byte[strm.Length];
                                string targetFile = Server.MapPath("~/PatientDocuments/PatientImages/" + FileName);
                                int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                                FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                                fs.Write(buffer, 0, byteSeq);
                                fs.Dispose();
                                GenerateThumbnails(0.0375, strm, targetFile);
                                //PatientImage.ImageUrl = "~/PatientDocuments/PatientImages/" + FileName;

                                ViewState["PatientImages"] = "/PatientDocuments/PatientImages/" + FileName;
                                ViewState["strm"] = strm;
                                ViewState["targetFile"] = targetFile;


                                PatientImage.ImageUrl = "~/imagesHTML/camera.ico";
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            PatientImage.ImageUrl = "~/imagesHTML/nophoto.ico";
                            //////PatientImage.ImageUrl = "~/Images/no_photo.jpg";
                        }
                        lblCId.Text = dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        lblDob.Text = dsPatientDetail.Tables[0].Rows[0]["DOB"].ToString();
                        //   lblAge.Text = dsPatientDetail.Tables[0].Rows[0]["Age"].ToString();
                        //  lblGender.Text = dsPatientDetail.Tables[0].Rows[0]["Gender"].ToString();
                        lblAge.Text = dsPatientDetail.Tables[0].Rows[0]["AgeGender"].ToString();

                        lblCrntEnSts.Text = dsPatientDetail.Tables[0].Rows[0]["EncounterStatus"].ToString();

                        lblPatientName.Text = dsPatientDetail.Tables[0].Rows[0]["PatientName"].ToString();
                        lblBedNo.Text = dsPatientDetail.Tables[0].Rows[0]["BedNo"].ToString();
                        lblWard.Text = dsPatientDetail.Tables[0].Rows[0]["WardName"].ToString();


                        lblEncNo.Text = lblEncNo_Resources.Text + ": " + dsPatientDetail.Tables[0].Rows[0]["EncounterNo"].ToString();
                        lblEncDate.Text = dsPatientDetail.Tables[0].Rows[0]["EncounterDate"].ToString();
                        lblVisitType.Text = dsPatientDetail.Tables[0].Rows[0]["VisitType"].ToString();
                        lblLoc.Text = dsPatientDetail.Tables[0].Rows[0]["FacilityName"].ToString();
                        lblMphone.Text = common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]).Trim();
                        lblAddress.Text = common.myStr(dsPatientDetail.Tables[0].Rows[0]["Address"]).Trim();
                        if (common.myLen(dsPatientDetail.Tables[0].Rows[0]["CityName"]) > 0)
                        {
                            lblAddress.Text += ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["CityName"]).Trim();
                        }
                        if (common.myLen(dsPatientDetail.Tables[0].Rows[0]["StateName"]) > 0)
                        {
                            lblAddress.Text += ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["StateName"]).Trim();
                        }
                        if (common.myLen(dsPatientDetail.Tables[0].Rows[0]["CountryName"]) > 0)
                        {
                            lblAddress.Text += ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["CountryName"]).Trim();
                        }
                        lblApptNote.Text = dsPatientDetail.Tables[0].Rows[0]["Note"].ToString();
                        lblVtCrPrvdr.Text = dsPatientDetail.Tables[0].Rows[0]["DoctorName"].ToString();
                        lblAcCategory.Text = dsPatientDetail.Tables[0].Rows[0]["AccountCategory"].ToString();
                        lblAcType.Text = dsPatientDetail.Tables[0].Rows[0]["AccountTypeName"].ToString();
                        lblPlnType.Text = dsPatientDetail.Tables[0].Rows[0]["PlanName"].ToString();
                        lblPayer.Text = dsPatientDetail.Tables[0].Rows[0]["Payername"].ToString();
                        Session["IsMedicalAlert"] = dsPatientDetail.Tables[0].Rows[0]["isMedicalAlert"].ToString();
                        lblPackageVisit.Visible = false;
                        lblPackageVisit.Text = "";
                        if (common.myStr(dsPatientDetail.Tables[0].Rows[0]["PackageName"]) != "")
                        {
                            lblPackageVisit.Text = "Package: " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PackageName"]);
                            lblPackageVisit.Visible = true;
                        }

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            if (dsPatientDetail != null)
            {
                dsPatientDetail.Dispose();
            }

        }
    }


    protected void bindImage()
    {

        if (Session["EMR_PatientRegistrationID"] != null && (!common.myStr(Session["EMR_PatientRegistrationID"]).Equals(string.Empty)))
        {


        }




    }
    protected void PatientImage_Click(object sender, ImageClickEventArgs e)
    {
        //string url = common.myStr(ViewState["PatientImages"]);
        //PatientImage.Attributes.Add("onclick", "getPage('" + url + "');");



        try
        {
            if (Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] != null)
            {
                dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
                if (dsPatientDetail.Tables.Count > 0)
                {
                    if (dsPatientDetail.Tables[0].Rows.Count > 0)
                    {
                        if (dsPatientDetail.Tables[0].Rows[0]["PatientImage"].ToString() != "")
                        {
                            try
                            {



                                Stream strm;
                                Object img = dsPatientDetail.Tables[0].Rows[0]["PatientImage"];
                                String FileName = dsPatientDetail.Tables[0].Rows[0]["ImageType"].ToString();
                                strm = new MemoryStream((byte[])img);
                                byte[] buffer = new byte[strm.Length];
                                string UnquieID = common.myStr(Guid.NewGuid().ToString("N"));

                                int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                                FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/PatientImages/" + UnquieID + ".jpg"), FileMode.Create, FileAccess.Write);
                                fs.Write(buffer, 0, byteSeq);
                                fs.Dispose();

                                string targetFile = Server.MapPath("~/PatientDocuments/PatientImages/" + UnquieID + ".jpg");
                                GenerateThumbnails(2, strm, targetFile);
                                //PatientImage.ImageUrl = "~/PatientDocuments/PatientImages/" + FileName;
                                // ViewState["PatientImages"] = "/PatientDocuments/PatientImages/" + FileName;
                                string url = "/PatientDocuments/PatientImages/" + UnquieID + ".jpg";
                                PatientImage.Attributes.Add("onclick", "getPage('" + url + "');");
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            PatientImage.ImageUrl = "~/imagesHTML/nophoto.ico";
                            //////PatientImage.ImageUrl = "~/Images/no_photo.jpg";
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            if (dsPatientDetail != null)
            {
                dsPatientDetail.Dispose();
            }

        }





        //  Stream strm;
        //  string targetFile;


        //  strm = (Stream)ViewState["strm"];
        //  targetFile = url + Guid.NewGuid().ToString("N");
        //   targetFile = Server.MapPath("/PatientDocuments/PatientImages/" + FileName);

        //GenerateThumbnails(1, strm, targetFile);



    }
}