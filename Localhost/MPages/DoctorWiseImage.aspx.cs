using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Drawing.Imaging;

public partial class MPages_DoctorWiseImage : System.Web.UI.Page
{
    clsExceptionLog objException = new clsExceptionLog();
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.EMROrders objEMR_Order = new BaseC.EMROrders(sConString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populateFacility();
            //  populateProvider();
            BindBlankGrid();
            BindDropDownServiceCategories();
            // BindDoctorTaggingImages("0",true);
            gvDoctorWiseImageTagging.Columns[1].Visible = false;
            lblProOrResour.Visible = false;
            ddlProvider.Visible = false;
        }
    }
    private void populateFacility()
    {
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
            DataView dv;
            dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "Active=1";

            ddlFacility.Items.Clear();
            ddlFacility.DataSource = dv;
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataTextField = "Name";
            ddlFacility.DataBind();
            ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void populateProvider()
    {
        //try
        //{
        //    DataSet objDs;
        //    Hashtable hshInput = new Hashtable();
        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
        //    hshInput.Add("@intFacilityId", common.myInt(ddlFacility.SelectedValue));
        //    hshInput.Add("@IsDoctor", true);
        //    objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetResourceList", hshInput);

        //    if (objDs.Tables[0].Rows.Count > 0)
        //    {
        //        ddlProvider.Items.Clear();
        //        ddlProvider.DataSource = objDs;
        //        ddlProvider.DataValueField = "DoctorId";
        //        ddlProvider.DataTextField = "DoctorName";
        //        ddlProvider.DataBind();
        //        ddlProvider.Items.Insert(0, new RadComboBoxItem("", "0"));
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}


        try
        {
            DataSet objDs = new DataSet();
            if (common.myStr(ddlDepartment.SelectedValue) == string.Empty)
            {
                ddlDepartment.SelectedValue = "0";
            }

            objDs = objEMR_Order.fillDoctorCombo(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlDepartment.SelectedValue), common.myInt(Session["FacilityID"]), 0);
            ddlProvider.DataSource = objDs.Tables[0];
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorId";
            ddlProvider.DataBind();

            if (objDs.Tables[0].Rows.Count > 1)
            {
                ddlProvider.Enabled = true;
            }
            else
            {
                ddlProvider.Enabled = false;
            }

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlProvider_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindDoctorTaggingImages(ddlProvider.SelectedValue, false);
    }
    protected void ddlFacility_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindDoctorTaggingImages("0", true);
    }
    protected void btnUpload_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            if (ddlFacility.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please select Facility", Page);
                return;
            }
            StringBuilder sb = new StringBuilder();
            string sFileName = "";
            string sFileSize = "";
            String FilePath = "";
            sFileName = _FileUpload.FileName;
            if (_FileUpload.FileName == "")
            {
                return;
            }

            CheckDirectory("0", Convert.ToString(Session["HospitalLocationID"]));

            int extIndex = sFileName.IndexOf(".");
            string sExt = sFileName.Substring(extIndex, sFileName.Length - extIndex);
            sFileSize = _FileUpload.PostedFile.ContentLength.ToString() + " KB";

            if (_FileUpload.PostedFile.ContentLength > 10485760)
            {
                Alert.ShowAjaxMsg("The file you uplaod is too large.", Page);
                return;
            }


            _FileUpload.SaveAs(Server.MapPath("/medical_illustration/") + "/" + sFileName);
            FilePath = "/medical_illustration/" + sFileName;


            sb.Append("<Table1><c1>");
            sb.Append(DBNull.Value);
            sb.Append("</c1><c2>");
            sb.Append(DBNull.Value);
            sb.Append("</c2><c3>");
            sb.Append(FilePath);
            sb.Append("</c3><c4>");
            sb.Append(sFileName);
            sb.Append("</c4><c5>");
            sb.Append(sFileSize);
            sb.Append("</c5><c6>");
            sb.Append(sExt);
            sb.Append("</c6></Table1>");

            BaseC.EMR emr = new BaseC.EMR(sConString);
            Hashtable hshOut = emr.SaveDoctorImageTagging(true, Convert.ToInt16(Session["HospitalLocationId"]),
                Convert.ToInt16(Session["FacilityId"]), sb.ToString(), Convert.ToInt32(Session["UserId"]));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = hshOut["@chvErrorStatus"].ToString();

            BindDoctorTaggingImages("0", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnUpdate_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            //if (ddlProvider.SelectedValue == "")
            //{
            //    Alert.ShowAjaxMsg("Please select Provider", Page);
            //    return;
            //}

            if (common.myStr(ddlDepartment.SelectedValue).Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Please select Department", Page);
                return;
            }
            //   CheckDirectory(ddlProvider.SelectedValue, Convert.ToString(Session["HospitalLocationID"]));
            CheckDirectory(ddlDepartment.SelectedValue, Convert.ToString(Session["HospitalLocationID"]));
            StringBuilder sb = new StringBuilder();
            foreach (GridViewRow row in gvDoctorWiseImageTagging.Rows)
            {

                Label lblDoctorName = (Label)row.FindControl("lblDoctorName");
                HiddenField hdnImageId = (HiddenField)row.FindControl("hdnImageId");
                HiddenField hdnFacilityID = (HiddenField)row.FindControl("hdnFacilityID");
                HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
                Label hdnImageName = (Label)row.FindControl("lblImageName");
                HiddenField hdnImageSize = (HiddenField)row.FindControl("hdnImageSize");
                HiddenField hdnImageType = (HiddenField)row.FindControl("hdnImageType");
                HiddenField hdnExistImage = (HiddenField)row.FindControl("hdnExistImage");
                Image imgImage = (Image)row.FindControl("imgImage");

                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                if (chkSelect.Checked == true && hdnExistImage.Value == "0")
                {
                    string sFileName = "";
                    String FilePath = "";
                    sFileName = hdnImageName.Text;

                    System.Drawing.Bitmap varBmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(Server.MapPath("~/medical_illustration/") + sFileName);
                    System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(varBmp);
                    //  varBmp.Dispose();


                    newBitmap.Save(Server.MapPath("~/medical_illustration/") + ddlDepartment.SelectedValue + "/" + sFileName);
                    FilePath = "/medical_illustration/" + ddlDepartment.SelectedValue + "/" + sFileName;


                    //using (var m = new MemoryStream())
                    //{
                    //    newBitmap.Save(m, ImageFormat.Jpeg);
                    //    var img = Image.FromStream(m);
                    //    img.Save(Server.MapPath("~/vicpic/scimages/") + imagename);
                    //}

                    //newBitmap.Save(Server.MapPath("~/medical_illustration/") + ddlProvider.SelectedValue + "/" + sFileName);
                    //FilePath = "/medical_illustration/" + ddlProvider.SelectedValue + "/" + sFileName;
                    /**************************************/
                    //string outputFileName = "...";
                    //using (MemoryStream memory = new MemoryStream())
                    //{
                    //    if (!Directory.Exists("~/medical_illustration/" + ddlDepartment.SelectedValue + "/" + sFileName))
                    //    { // if it doesn't exist, create

                    //        System.IO.Directory.CreateDirectory(("~/medical_illustration/") + ddlDepartment.SelectedValue + "/" + sFileName);
                    //    }


                    //    using (FileStream fs = new FileStream(Server.MapPath("~/medical_illustration/") + ddlDepartment.SelectedValue + "/" + sFileName, FileMode.Create, FileAccess.ReadWrite))
                    //    {
                    //        // newBitmap.Save(memory, ImageFormat.Jpeg);
                    //        newBitmap.Save(Server.MapPath("~/medical_illustration/") + ddlDepartment.SelectedValue + "/" + sFileName);
                    //        FilePath = "/medical_illustration/" + ddlDepartment.SelectedValue + "/" + sFileName;
                    //        byte[] bytes = memory.ToArray();
                    //        fs.Write(bytes, 0, bytes.Length);
                    //    }
                    //}
                    /**************************************/







                    sb.Append("<Table1><c1>");
                    sb.Append(hdnImageId.Value);
                    sb.Append("</c1><c2>");
                    sb.Append(ddlProvider.SelectedValue);
                    sb.Append("</c2><c3>");
                    sb.Append(FilePath);
                    sb.Append("</c3><c4>");
                    sb.Append(sFileName);
                    sb.Append("</c4><c5>");
                    sb.Append(DBNull.Value);
                    sb.Append("</c5><c6>");
                    sb.Append(DBNull.Value);
                    sb.Append("</c6><c7>");
                    sb.Append(ddlDepartment.SelectedValue);
                    sb.Append("</c7></Table1>");
                }
            }

            BaseC.EMR emr = new BaseC.EMR(sConString);
            if (sb.ToString() != "")
            {
                Hashtable hshOut = emr.SaveDoctorImageTagging(false, Convert.ToInt16(Session["HospitalLocationId"]),
                    Convert.ToInt16(Session["FacilityId"]), sb.ToString(), Convert.ToInt32(Session["UserId"]));
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = hshOut["@chvErrorStatus"].ToString();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Either Image already mapped Or No Image is selected";
            }
            BindDoctorTaggingImages(ddlProvider.SelectedValue, false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void CheckDirectory(string DoctorId, string HospitalLocationId)
    {
        if (DoctorId == "0")
        {
            DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/medical_illustration/"));
            if (objDir.Exists == false)
            {
                objDir.Create();
            }
        }
        else
        {
            DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/medical_illustration/" + DoctorId));
            if (objDir.Exists == false)
            {
                objDir.Create();
            }
        }
    }
    protected string CreateFileName(string sCategoryId, string sFileName)
    {
        string FileName = "";
        try
        {
            int extIndex = sFileName.IndexOf(".");
            string sExt = sFileName.Substring(extIndex, sFileName.Length - extIndex);
            BaseC.Patient objPat = new BaseC.Patient(sConString);
            string[] sTime = null;
            char[] chr = { ' ' };
            sTime = DateTime.Now.ToString().Split(chr);
            FileName = objPat.FormatDateDateMonthYear(sTime[0]) + sTime[1] + sTime[2];
            FileName = FileName.Replace("/", "");
            FileName = FileName.Replace(":", "");
            FileName = FileName + "_" + sCategoryId + sExt;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return FileName;
    }
    private void BindBlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ImageId");
        dt.Columns.Add("HospitalLocationId");
        dt.Columns.Add("FacilityId");
        dt.Columns.Add("DoctorId");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("ImageName");
        dt.Columns.Add("ImagePath");
        dt.Columns.Add("ImageSize");
        dt.Columns.Add("ImageType");
        dt.Columns.Add("Active");
        dt.Columns.Add("ExistImage");
        dt.Columns.Add("DepartmentName");
        dt.Columns.Add("Deparmentid");
        DataRow dr = dt.NewRow();
        dr["ImageId"] = null;
        dr["HospitalLocationId"] = null;
        dr["FacilityId"] = null;
        dr["DoctorId"] = null;
        dr["DoctorName"] = null;
        dr["ImageName"] = null;
        dr["ImagePath"] = null;
        dr["ImageSize"] = null;
        dr["ImageType"] = null;
        dr["Active"] = null;
        dr["ExistImage"] = null;
        dr["DepartmentName"] = null;
        dr["Deparmentid"] = null;
        dt.Rows.Add(dr);
        gvDoctorWiseImageTagging.DataSource = dt;
        gvDoctorWiseImageTagging.DataBind();

    }

    private void BindDoctorTaggingImages(string sDoctorId, bool bNewUpload)
    {
        BaseC.EMR emr = new BaseC.EMR(sConString);
        // DataSet ds = emr.GetDoctorImageTagging(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]),Convert.ToInt32(sDoctorId),bNewUpload);
        DataSet ds = emr.GetDoctorImageTagging(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]), common.myInt(sDoctorId), bNewUpload, common.myInt(ddlDepartment.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDoctorWiseImageTagging.DataSource = ds.Tables[0];
            gvDoctorWiseImageTagging.DataBind();
        }
        else
        {
            BindBlankGrid();
        }
    }
    protected void imgBtnImage_OnClick(object sender, EventArgs e)
    {
        ImageButton img = sender as ImageButton;
        string strvalue = img.CommandArgument;
        imgImage.ImageUrl = strvalue;
    }
    protected void gvDoctorWiseImageTagging_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Del"))
        {

            try
            {
                lblMessage.Text = string.Empty;
                StringBuilder sb = new StringBuilder();
                foreach (GridViewRow row in gvDoctorWiseImageTagging.Rows)
                {
                    HiddenField hdnImageId = (HiddenField)row.FindControl("hdnImageId");
                    HiddenField hdnFacilityID = (HiddenField)row.FindControl("hdnFacilityID");
                    //HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
                    HiddenField hdnDeparmentid = (HiddenField)row.FindControl("hdnDeparmentid");

                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");

                    if (chkSelect.Checked == true && common.myInt(hdnDeparmentid.Value) != 0)
                    {
                        sb.Append("<Table1><c1>");
                        sb.Append(hdnImageId.Value);
                        sb.Append("</c1><c2>");
                        //sb.Append(ddlProvider.SelectedValue);
                        sb.Append(ddlDepartment.SelectedValue);
                        sb.Append("</c2></Table1>");
                    }
                }

                if (sb.ToString() != "")
                {
                    BaseC.EMR emr = new BaseC.EMR(sConString);
                    Hashtable hshOut = emr.DeleteDoctorImageTagging(Convert.ToInt16(Session["HospitalLocationId"]),
                        Convert.ToInt16(Session["FacilityId"]), sb.ToString(), Convert.ToInt32(Session["UserId"]));
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = hshOut["@chvErrorStatus"].ToString();
                    BindDoctorTaggingImages(ddlProvider.SelectedValue, false);
                }
                else
                {




                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Either Image not tagged with Deparment Or Image not selected";
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
    private void BindDropDownServiceCategories()
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = order.GetServiceCategories(Convert.ToInt32(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDepartment.DataSource = ds.Tables[0];
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            //populateProvider();
            BindDoctorTaggingImages(ddlProvider.SelectedValue, false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
