using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Widgets;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Ionic.Zip;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;

public partial class Annotator : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    string path = string.Empty;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hdnXmlString.Value = "";

            //yogesh 22/06/2022

            // string path1 = Page.ResolveUrl("~/medical_illustration/" + common.myStr(Session["LoginDepartmentId"]) + "/");



            // physical path yogesh
            string path1 = Server.MapPath("~/medical_illustration/" + common.myStr(Session["LoginDepartmentId"]) + "/");
            if (!Directory.Exists(path1))
            {
                path1 = Page.ResolveUrl("~/medical_illustration/");
            }
            else
            {
                // logical path
                path1 = Page.ResolveUrl("~/medical_illustration/" + common.myStr(Session["LoginDepartmentId"]) + "/");
            }


            FileExplorer1.Configuration.UploadPaths = new string[0];
            FileExplorer1.Configuration.ViewPaths = new string[] { path1 };

            //FileExplorer1.Configuration.UploadPaths = new string[0];
            //FileExplorer1.InitialPath = Page.ResolveUrl("~/medical_illustration");

            BindPatientMedicalIllustrationImage(Convert.ToInt32(Session["HospitalLocationId"]), Convert.ToInt32(Session["FacilityId"]), Convert.ToInt32(Session["RegistrationId"]), Convert.ToInt32(Session["EncounterId"]));
        }
    }
    private void BindPatientMedicalIllustrationImage(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId)
    {
        BaseC.EMR emr = new BaseC.EMR(sConString);
        DataSet ds = emr.GetPatientIllustrationImages(HospitalLocationId, FacilityId, RegistrationId, EncounterId);

        //yogesh
        //ViewState["PatientPanelImage"] = ds;
        //ViewState["RegId"] = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
        //ViewState["EncId"] = ds.Tables[0].Rows[0]["EncounterId"].ToString();
        if (ds.Tables[0].Rows.Count > 0)
        {
            string path = Page.ResolveUrl("~/PatientDocuments/" + Session["HospitalLocationId"].ToString() + "/" + Session["RegistrationId"].ToString() + "/" + Convert.ToString(Session["EncounterId"]) + "/");

            RadFileExplorer1.Configuration.UploadPaths = new string[0];
            RadFileExplorer1.Configuration.ViewPaths = new string[] { path };
        }
    }


    protected Telerik.Web.UI.FileExplorer.FileExplorerControls GetVisibleControls()
    {
        Telerik.Web.UI.FileExplorer.FileExplorerControls explorerControls = 0;

        explorerControls |= Telerik.Web.UI.FileExplorer.FileExplorerControls.AddressBox;

        explorerControls |= Telerik.Web.UI.FileExplorer.FileExplorerControls.Grid;

        explorerControls |= Telerik.Web.UI.FileExplorer.FileExplorerControls.Toolbar;

        explorerControls |= Telerik.Web.UI.FileExplorer.FileExplorerControls.TreeView;

        explorerControls |= Telerik.Web.UI.FileExplorer.FileExplorerControls.ContextMenus;
        return explorerControls;

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (hdnPatientImage.Value != "")
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            objDl.ExecuteNonQuery(CommandType.Text, "update EMRPatientDocuments set Active=0 where ImageName='" + hdnPatientImage.Value + "'");
            checkDirectory(Convert.ToString(Session["RegistrationId"]), Convert.ToString(Session["HospitalLocationID"]));

            string path = Server.MapPath("~/PatientDocuments/" + Convert.ToString(Session["HospitalLocationID"]) + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Convert.ToString(Session["EncounterId"]) + "/" + hdnPatientImage.Value);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
                Alert.ShowAjaxMsg(hdnFileName.Value + " file deleted successfully", Page);
            }
        }
        else
        {
            Alert.ShowAjaxMsg("Please select file", this);
        }
    }


    protected void savebtn_Click(object sender, EventArgs e)
    {


    }
    private void SaveImage()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshTable = new Hashtable();
        StringBuilder objStr = new StringBuilder();
        string sFileName = "";
        string fileName = "";
        string sFileSize = "";
        System.IO.MemoryStream ms;
        MemoryStream ms2 = new MemoryStream();
        System.Drawing.Image img;
        BaseC.ParseData objParse = new BaseC.ParseData();
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        FileUpload fileUpload = new FileUpload();
        if (Convert.ToString(this.txt1.Value) == "")
        {
            Alert.ShowAjaxMsg("Please Click on Capture Image to Convert to Base64 string before Saving the Image", Page);
            return;
        }
        else
        {
            String durlimg = this.txt1.Value;
            fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            String trimddurlimg = durlimg.Remove(0, 22);
            byte[] imgurl = Convert.FromBase64String(trimddurlimg);
            ms = new System.IO.MemoryStream(imgurl);
            img = System.Drawing.Image.FromStream(ms);
            sFileSize = ms.Length.ToString();

        }
        checkDirectory(Convert.ToString(Session["RegistrationId"]), Convert.ToString(Session["HospitalLocationID"]));
        sFileName = CreateFileName("1", fileName + ".jpg");
        //yogesh 
        ViewState["saveImageName"] = sFileName;
        img.Save(Server.MapPath("../PatientDocuments/") + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Convert.ToString(Session["EncounterId"]) + "/" + sFileName, System.Drawing.Imaging.ImageFormat.Png);
        ms2 = new MemoryStream(File.ReadAllBytes(Server.MapPath("../PatientDocuments/") + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Convert.ToString(Session["EncounterId"]) + "/" + sFileName));


        int extIndex = sFileName.IndexOf(".");
        string sExt = sFileName.Substring(extIndex, sFileName.Length - extIndex);

        ms.Close();
        objStr.Append("<Table1>");
        objStr.Append("<c1>");
        objStr.Append("/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Convert.ToString(Session["EncounterId"]) + "/" + sFileName);
        objStr.Append("</c1>");
        objStr.Append("<c2>");
        objStr.Append(objParse.ParseQ(hdnFileName.Value));
        objStr.Append("</c2>");
        objStr.Append("<c3>");
        objStr.Append("1");
        objStr.Append("</c3>");
        objStr.Append("<c4>");
        objStr.Append(sFileName);
        objStr.Append("</c4>");
        objStr.Append("<c5>");
        objStr.Append(objParse.ParseQ(Convert.ToString(sFileSize)));
        objStr.Append("</c5>");
        objStr.Append("<c6>");
        objStr.Append("");
        objStr.Append("</c6>");
        objStr.Append("<c7>");
        objStr.Append("Image saved from Medical Illustrations");
        objStr.Append("</c7>");
        objStr.Append("<c8>");
        objStr.Append(objPatient.FormatDateDateMonthYear(DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
        objStr.Append("</c8>");
        objStr.Append("</Table1>");
        hshTable.Add("@intRegistrationId", Session["RegistrationNo"]);
        hshTable.Add("@intEncounterId", Session["EncounterId"]);
        hshTable.Add("@intEncodedBy", Session["UserId"]);
        hshTable.Add("@xmlDocumentDetails", objStr.ToString());
        hshTable.Add("@chvThumbnail", "/Images/JPG.jpg");
        hshTable.Add("@chvDocumentType", sExt);
        hshTable.Add("@IsMedIllustration", Convert.ToBoolean(1));
        hshTable.Add("@chvImageCategoryCode", "MI");


        objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSavePatientDocuments", hshTable);

        //Alert.ShowAjaxMsg("Image Saved to Attachement.", Page);

        //string imgpth = Request.Url.Scheme + "://" + Request.Url.Authority + "/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + sFileName;
        //string scrpt = "<script language='JavaScript'> GetSavedImage('" + imgpth + "');</script> ";
        //this.Page.RegisterStartupScript("Exec Script", scrpt);
        #region save image in table 
        byte[] byteImageData;
        StringBuilder strSQL = new StringBuilder();
        // "/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Convert.ToString(Session["EncounterId"]) + "/" + sFileName);
        // String FilePath = Server.MapPath("/PatientDocuments/PatientImages/") + txtPatientImageId.Text;
        String FilePath = Server.MapPath("/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Convert.ToString(Session["EncounterId"]) + "/" + sFileName);

        FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        byte[] image = br.ReadBytes((int)fs.Length);
        br.Close();
        fs.Close();
        byteImageData = image;
        SqlConnection con = new SqlConnection(sConString);
        SqlCommand cmdTemp;
        SqlParameter prm1, prm2, prm3, prm4, prm5;



        strSQL.Append("Exec uspEMRSaveMIImage @intHospitalLocationId, @intRegistrationId, @intEncounterId,@intEncodedBy,@Image");
        cmdTemp = new SqlCommand(strSQL.ToString(), con);
        cmdTemp.CommandType = CommandType.Text;

        prm1 = new SqlParameter();
        prm1.ParameterName = "@intHospitalLocationId";
        prm1.Value = common.myInt(Session["HospitalLocationID"]);
        prm1.SqlDbType = SqlDbType.Int;
        prm1.Direction = ParameterDirection.Input;
        cmdTemp.Parameters.Add(prm1);

        prm2 = new SqlParameter();
        prm2.ParameterName = "@intRegistrationId";
        prm2.Value = common.myInt(Session["RegistrationId"]);
        prm2.SqlDbType = SqlDbType.Int;
        prm2.Direction = ParameterDirection.Input;
        cmdTemp.Parameters.Add(prm2);

        prm3 = new SqlParameter();
        prm3.ParameterName = "@intEncounterId";
        prm3.Value = common.myInt(Session["EncounterId"]);
        prm3.SqlDbType = SqlDbType.Int;
        prm3.Direction = ParameterDirection.Input;
        cmdTemp.Parameters.Add(prm3);

        prm4 = new SqlParameter();
        prm4.ParameterName = "@intEncodedBy";
        prm4.Value = common.myInt(Session["UserId"]);
        prm4.SqlDbType = SqlDbType.Int;
        prm4.Direction = ParameterDirection.Input;
        cmdTemp.Parameters.Add(prm4);

        prm5 = new SqlParameter();
        prm5.ParameterName = "@Image";
        prm5.Value = byteImageData;
        prm5.SqlDbType = SqlDbType.Image;
        prm5.Direction = ParameterDirection.Input;
        cmdTemp.Parameters.Add(prm5);



        con.Open();
        cmdTemp.ExecuteNonQuery();
        #endregion
        BindPatientMedicalIllustrationImage(Convert.ToInt32(Session["HospitalLocationId"]), Convert.ToInt32(Session["FacilityId"]), Convert.ToInt32(Session["RegistrationId"]), Convert.ToInt32(Session["EncounterId"]));
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {


        // Yogesh 08/07/2022

        if (Convert.ToString(this.txt1.Value) == "")
        {
            Alert.ShowAjaxMsg("Please Click on Capture Image to Convert to Base64 string before Inserting the Image", Page);
            return;
        }
        else
        {
            if (hdnXmlString.Value == "")
            {
                SaveImage();
                String durlimg = this.txt1.Value;
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                String trimddurlimg = durlimg.Remove(0, 22);
                byte[] imgurl = Convert.FromBase64String(trimddurlimg);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(imgurl);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                ms.Close();
                img.Save(Server.MapPath(@"Annotated Images/" + fileName + ".jpg"), System.Drawing.Imaging.ImageFormat.Png);

                string ImageLink = Request.Url.Scheme + "://" + Request.Url.Authority + "/ImageEditor/Annotated Images/" + fileName + ".jpg";
                // string str = "<script language='JavaScript'> var closeArgument = {};  closeArgument.image = '" + ImageLink + "';  getRadWindow().close(closeArgument);</script> ";

                hdnXmlString.Value = ImageLink;



                //yogesh 08/07/2022
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                hdnXmlString.Value = "";
                //ClientScript.RegisterClientScriptBlock(Page.GetType(), "InsertImage", str);


                // yogesh
               //bindEditorImage();
                //ViewState["PatientPanelImage"]
            }
        }
    }

    protected string CreateFileName(string sCategoryId, string sFileName)
    {
        string FileName = "";
        int extIndex = sFileName.IndexOf(".");
        string sExt = sFileName.Substring(extIndex, sFileName.Length - extIndex);
        BaseC.Patient objPat = new BaseC.Patient(sConString);
        string[] sTime = null;
        char[] chr = { ' ' };
        sTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt").Replace("-", "/").Split(chr);
        //sTime = DateTime.Now.ToString().Split(chr);
        FileName = objPat.FormatDateDateMonthYear(sTime[0]) + sTime[1] + sTime[2];
        FileName = FileName.Replace("/", "");
        FileName = FileName.Replace(":", "");
        //Response.Write(objPat.FormatDateDateMonthYear(DateTime.Today.ToShortDateString()) + " " + DateTime.Now.Hour.ToString() + "_" + DateTime.Today.Now.ToString() + "_" + DateTime.Now.Second.ToString());

        FileName = Convert.ToString(Session["RegistrationId"]) + "_" + FileName + "_" + sCategoryId + sExt;

        return FileName;
    }
    protected void checkDirectory(string RegistrationId, string HospitalLocationId)
    {
        DirectoryInfo objHospitalDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/" + HospitalLocationId));
        if (objHospitalDir.Exists == false)
        {
            objHospitalDir.Create();
        }
        DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/" + HospitalLocationId + "/" + RegistrationId + "/" + Session["EncounterId"].ToString() + "/"));
        if (objDir.Exists == false)
        {
            objDir.Create();
        }
    }


    protected void btnImageP_Click(object sender, EventArgs e)
    {

        string Source = "OPD";
        string b = "";
        string a = HiddenField1.Value;
        //a = a.Substring(a.LastIndexOf("224683") + 6);
        a = a.Substring(a.LastIndexOf(common.myStr(Session["EncounterId"])) + 6);
        string strImageName = "";
        strImageName = hdnPatientImage.Value;
        a = a.Replace(".jpg.jpg", ".jpg");
        ViewState["RegistrationId"] = Session["RegistrationId"];
        ViewState["EncounterId"] = Session["EncounterId"];
        string imagepath = "";

        DirectoryInfo objd = new DirectoryInfo(Server.MapPath("~/"));
        if (objd.Exists)
        {

            b = objd.FullName;

            imagepath = b + "PatientDocuments\\" + common.myInt(Session["HospitalLocationId"]) + "\\" + common.myInt(ViewState["RegistrationId"]) + "\\" + common.myInt(ViewState["EncounterId"]) + "\\" + a;
            //imagepath = @"C:\Idrish\ApolloDhaka\EMRSourceCode_11022020By_Manmohan\Localhost\PatientDocuments\1\713956\224683" + a;
        }
        DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("~/PatientDocuments/" + common.myInt(Session["HospitalLocationId"]) + "/" + common.myInt(ViewState["RegistrationId"]) + "/" + common.myInt(ViewState["EncounterId"]) + "/"));
        if (objDir.Exists)
        {

            string path = "/PatientDocuments/" + common.myInt(Session["HospitalLocationID"]).ToString() + "/" + common.myInt(ViewState["RegistrationId"]).ToString() + "/" + common.myInt(ViewState["EncounterId"]) + "/";

        }



        string Str = "HospitalLocationID=" + common.myInt(Session["HospitalLocationID"]).ToString() + "&FacilityID=" + common.myInt(Session["FacilityID"]).ToString() + "&RegistrationId=" + common.myInt(Session["RegistrationId"]).ToString() + "&EncounterId=" + common.myInt(ViewState["EncounterId"]).ToString() + "&strImageName=" + strImageName.ToString();

        Str = "PatImage:OpenForm?" + Str;

        ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);

        return;

    }

    //yogesh
    private void bindEditorImage()
    {
        string fileName = ViewState["saveImageName"].ToString();
        string imagePath = string.Empty;
        DataSet ds = (DataSet)ViewState["PatientPanelImage"];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if(common.myStr(ds.Tables[0].Rows[i]["EncounterId"]).Contains(ViewState["EncId"].ToString()) &&
               Convert.ToInt32(ds.Tables[0].Rows[i]["RegistrationId"]) ==  Convert.ToInt32(ViewState["RegId"].ToString())
               && ds.Tables[0].Rows[i]["ImageName"].ToString().Equals(fileName))
            {
                imagePath = ds.Tables[0].Rows[i]["ImagePath"].ToString();
                // imagePath = Page.ResolveUrl(imagePath);
                imagePath = Request.Url.Scheme + "://" + Request.Url.Authority + imagePath;
                break;
                // Request.Url.Scheme + "://" + Request.Url.Authority + 
            }
        }


        //yogesh new attachment
        string Source = "OPD";
        string b = "";
        string a = imagePath;
        //a = a.Substring(a.LastIndexOf("224683") + 6);
        a = a.Substring(a.LastIndexOf(common.myStr(Session["EncounterId"])) + 6);
        string strImageName = "";
        strImageName = fileName;
        a = a.Replace(".jpg.jpg", ".jpg");
        ViewState["RegistrationId"] = Session["RegistrationId"];
        ViewState["EncounterId"] = Session["EncounterId"];
        string imagepath = "";

        DirectoryInfo objd = new DirectoryInfo(Server.MapPath("~/"));
        if (objd.Exists)
        {

            b = objd.FullName;

            imagepath = b + "PatientDocuments\\" + common.myInt(Session["HospitalLocationId"]) + "\\" + common.myInt(ViewState["RegistrationId"]) + "\\" + common.myInt(ViewState["EncounterId"]) + "\\" + a;
            //imagepath = @"C:\Idrish\ApolloDhaka\EMRSourceCode_11022020By_Manmohan\Localhost\PatientDocuments\1\713956\224683" + a;
        }
        DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("~/PatientDocuments/" + common.myInt(Session["HospitalLocationId"]) + "/" + common.myInt(ViewState["RegistrationId"]) + "/" + common.myInt(ViewState["EncounterId"]) + "/"));
        if (objDir.Exists)
        {

            string path = "/PatientDocuments/" + common.myInt(Session["HospitalLocationID"]).ToString() + "/" + common.myInt(ViewState["RegistrationId"]).ToString() + "/" + common.myInt(ViewState["EncounterId"]) + "/";

        }



        string Str = "HospitalLocationID=" + common.myInt(Session["HospitalLocationID"]).ToString() + "&FacilityID=" + common.myInt(Session["FacilityID"]).ToString() + "&RegistrationId=" + common.myInt(Session["RegistrationId"]).ToString() + "&EncounterId=" + common.myInt(ViewState["EncounterId"]).ToString() + "&strImageName=" + strImageName.ToString();

        Str = "PatImage:OpenForm?" + Str;

        ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);

        return;



    }
}

