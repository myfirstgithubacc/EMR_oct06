using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
public partial class EMR_Masters_ReportFormat : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    string folderPath;
    clsExceptionLog objException = new clsExceptionLog();

    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];
    string[] strdocType = { ".jpg", ".jpeg", ".png" };

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            bindControl();

            //gvReport.Columns[8].Visible = true;
            gvReport.Columns[3].Visible = false;
            gvReport.Columns[4].Visible = false;
            gvReport.Columns[5].Visible = false;

            bindReportList();
            //txtMarginBottom.Visible = false;
            //txtMarginTop.Visible = false;
            //txtMarginLeft.Visible = false;
            //txtMarginRight.Visible = false;

            //lblMarginBottom.Visible = false;
            //lblMarginTop.Visible = false;
            //lblMarginLeft.Visible = false;
            //lblMarginRight.Visible = false;

            //spMarginBottom.Visible = false;
            //spMarginTop.Visible = false;
            //spMarginLeft.Visible = false;
            //spMarginRight.Visible = false;
        }
    }
    private void bindControl()
    {
        BaseC.Hospital objH = new BaseC.Hospital(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objH.getEMRFormHeader(common.myInt(Session["HospitalLocationID"]));
            ddlClinicalHeaderId.DataSource = ds.Tables[0];
            ddlClinicalHeaderId.DataTextField = "HeaderName";
            ddlClinicalHeaderId.DataValueField = "HeaderId";
            ddlClinicalHeaderId.DataBind();

            ddlClinicalHeaderId.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlClinicalHeaderId.SelectedIndex = 0;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objH = null;
            ds.Dispose();
        }
    }
    private void bindReportList()
    {
        try
        {
            clsIVF objivf = new clsIVF(sConString);
            DataSet ds = objivf.getEMRTemplateReportSetup(0, 0, common.myInt(Session["EmployeeId"]), "D", 1,
                common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvReport.DataSource = ds;
                gvReport.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string sFileName = "";
            string sFileNameFooter = "";
            string sSavePath = "/Images/DischargeSummary/";
            if (chkPrintFooterImage.Checked)
            {
                if (common.myStr(hdnPrintFooterImagePath.Value).Equals(""))
                {
                    if (!_ImageFileUploadFooter.HasFile)
                    {
                        Alert.ShowAjaxMsg("The select  footer image.", Page);
                        return;
                    }
                }
            }

            if (chkPrintHeaderImage.Checked)
            {
                if (common.myStr(hdnPrintHeaderImagePath.Value).Equals(""))
                {
                    if (!_ImageFileUploadHeader.HasFile)
                    {
                        Alert.ShowAjaxMsg("The select header  image.", Page);
                        return;
                    }
                }
            }
            if (_ImageFileUploadHeader.HasFile)
            {
                if (!Directory.Exists(sSavePath))
                {
                    //If Directory (Folder) does not exists. Create it.
                    Directory.CreateDirectory(sSavePath);
                }

                string sFileSize;
                string sFileExtension = "";
                sFileName = CreateFileName(txtReportName.Text.Replace(" ", "") + "HeadImg", _ImageFileUploadHeader.FileName);
                sFileSize = _ImageFileUploadHeader.PostedFile.ContentLength.ToString() + " KB";
                //if (_FileUpload.PostedFile.ContentLength > 10485760)
                if (_ImageFileUploadHeader.PostedFile.ContentLength > 20971520)
                {
                    Alert.ShowAjaxMsg("The file you uplaod is too large.", Page);
                    return;
                }
                else
                {

                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath(sSavePath));
                    FileInfo[] fi_array = dir.GetFiles();
                    String FileName = "";
                    foreach (FileInfo file in fi_array)
                    {
                        FileName = file.ToString();
                        sFileExtension = System.IO.Path.GetExtension(FileName).ToLower();
                        if (FileName.ToString() == sFileName)
                        {
                            File.Delete(Server.MapPath(sSavePath) + sFileName + sFileExtension.ToLower());
                            //File.Move(Server.MapPath("/PatientDocuments/PatientImages/" + txtPatientImageId.Text), Server.MapPath("/PatientDocuments/PatientImages/" + strRegistrationId + sFileExtension.ToLower()));

                            break;
                        }
                    }

                    HttpPostedFile myFile = _ImageFileUploadHeader.PostedFile;
                    int nFileLen = myFile.ContentLength;
                    if (nFileLen == 0)
                    {
                        lblMessage.Text = "Error: The file size is zero.";
                        return;
                    }
                    byte[] myData = new Byte[nFileLen];
                    myFile.InputStream.Read(myData, 0, nFileLen);
                    sFileExtension = "";
                    sFileExtension = System.IO.Path.GetExtension(myFile.FileName).ToLower();
                    //sFileName = myFile.FileName.ToLower();
                    System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(sSavePath + sFileName), System.IO.FileMode.Create);
                    newFile.Write(myData, 0, myData.Length);
                    newFile.Close();
                }
            }

            if (_ImageFileUploadFooter.HasFile)
            {


                string sFileSize;
                string sFileExtension = "";
                sFileNameFooter = CreateFileName(txtReportName.Text.Replace(" ", "") + "FooterImg", _ImageFileUploadFooter.FileName);
                sFileSize = _ImageFileUploadFooter.PostedFile.ContentLength.ToString() + " KB";
                //if (_FileUpload.PostedFile.ContentLength > 10485760)
                if (_ImageFileUploadFooter.PostedFile.ContentLength > 20971520)
                {
                    Alert.ShowAjaxMsg("The file you uplaod is too large.", Page);
                    return;
                }
                else
                {

                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath(sSavePath));
                    FileInfo[] fi_array = dir.GetFiles();
                    String FileName = "";
                    foreach (FileInfo file in fi_array)
                    {
                        FileName = file.ToString();
                        sFileExtension = System.IO.Path.GetExtension(FileName).ToLower();
                        if (FileName.ToString() == sFileNameFooter)
                        {
                            File.Delete(Server.MapPath(sSavePath) + sFileNameFooter + sFileExtension.ToLower());
                            //File.Move(Server.MapPath("/PatientDocuments/PatientImages/" + txtPatientImageId.Text), Server.MapPath("/PatientDocuments/PatientImages/" + strRegistrationId + sFileExtension.ToLower()));

                            break;
                        }
                    }

                    HttpPostedFile myFile = _ImageFileUploadFooter.PostedFile;
                    int nFileLen = myFile.ContentLength;
                    if (nFileLen == 0)
                    {
                        lblMessage.Text = "Error: The file size is zero.";
                        return;
                    }
                    byte[] myData = new Byte[nFileLen];
                    myFile.InputStream.Read(myData, 0, nFileLen);
                    sFileExtension = "";
                    sFileExtension = System.IO.Path.GetExtension(myFile.FileName).ToLower();
                    //sFileName = myFile.FileName.ToLower();
                    System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(sSavePath + sFileNameFooter), System.IO.FileMode.Create);
                    newFile.Write(myData, 0, myData.Length);
                    newFile.Close();
                }
            }
            if (FileUpLoad1.HasFile)
            {


                folderPath = Server.MapPath("~/ReportImage/");

                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists. Create it.
                    Directory.CreateDirectory(folderPath);
                }

                //Save the File to the Directory (Folder).
                // FileUpLoad1.SaveAs(folderPath + Path.GetFileName(FileUpLoad1.FileName));
                //Display the success message.
                lblMessage.Text = Path.GetFileName(FileUpLoad1.FileName) + " has been uploaded.";
            }
            if (common.myStr(txtReportName.Text) == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Report name not entered !";
                return;
            }
            clsIVF objIVF = new clsIVF(sConString);
            DataSet ds = new DataSet();
            if (common.myInt(ViewState["ReportId"]) == 0)
            {
                ds = objIVF.CheckReportName(common.myStr(txtReportName.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Report name already exists !";
                    txtReportName.Focus();
                    ds.Dispose();
                    return;
                }
            }
            if (sFileName != "")
            {
                sFileName = sSavePath + sFileName;

            }
            else
            {
                sFileName = hdnPrintHeaderImagePath.Value;

            }
            if (sFileNameFooter != "")
            {

                sFileNameFooter = sSavePath + sFileNameFooter;
            }
            else
            {

                sFileNameFooter = hdnPrintFooterImagePath.Value;
            }


            /*yogesh*/
            // <% --yogesh 4 / 4 / 22 rollback-- %>
            //if (chkPrintDoctorSignature.Checked && signheighttxtbox.Text.Equals(""))
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Signature height required!')</script>");
            //    return;
            //}
            //else if (chkPrintDoctorSignature.Checked && signwidthtxtbox.Text.Equals(""))
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Signature width required!')</script>");
            //    return;
            //}

            string strMsg = objIVF.SaveEMRTemplateReportName(common.myInt(ViewState["ReportId"]), common.myStr(txtReportName.Text), 0,
                                        0, "", common.myInt(ddlStatus.SelectedValue), common.myBool(chkShowPageNoInPageFooter.Checked),
                                        common.myBool(chkShowPrintByInPageFooter.Checked), common.myBool(chkShowPrintDateInPageFooter.Checked),

                                        common.myInt(Session["UserID"]), txtReportHead.Text, common.myStr(ddlReportType.SelectedValue),
                                        common.myInt(ddlClinicalHeaderId.SelectedValue), common.myBool(chkPrintHospitalHeader.Checked),
                                        common.myBool(chkPrintDoctorSignature.Checked), common.myBool(chkIsShowFilledTemplates.Checked),
                                        common.myBool(chkCheckListRequired.Checked), common.myInt(txtMarginLeft.Text),
                                        common.myInt(txtMarginRight.Text), common.myInt(txtMarginTop.Text), common.myInt(txtMarginBottom.Text),
                                        common.myStr(txtReportFooterText.Text), common.myBool(chkDefaultForOP.Checked), common.myStr(ddlPageSize.SelectedValue),
                                        FileUpLoad1.FileName, common.myBool(chkPrintHeaderImage.Checked), common.myBool(chkPrintFooterImage.Checked),
                                        common.myStr(txtPrintVersionCode.Text), sFileName, sFileNameFooter
                /*yogesh 4 / 4 / 22 rollback   common.myInt(signheighttxtbox.Text), common.myInt(signwidthtxtbox.Text)*/);



            if (strMsg.Contains("Saved") || strMsg.Contains("Update"))
            {
                ViewState["ReportId"] = "";
                bindReportList();
                Clear();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;
                if (FileUpLoad1.HasFile)
                {
                    FileUpLoad1.SaveAs(folderPath + Path.GetFileName(FileUpLoad1.FileName));
                }
                btnSave.Text = "Save";
            }
            ddlReportType_SelectedIndexChanged(null, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Clear();
        ddlReportType_SelectedIndexChanged(null, null);
    }
    void Clear()
    {
        lblMessage.Text = "";
        txtReportHead.Text = "";
        txtReportName.Text = "";
        chkCheckListRequired.Checked = false;
        chkPrintHospitalHeader.Checked = false;
        chkPrintDoctorSignature.Checked = false;
        chkShowPageNoInPageFooter.Checked = false;
        chkShowPrintByInPageFooter.Checked = false;
        chkShowPrintDateInPageFooter.Checked = false;
        chkDefaultForOP.Checked = false;
        chkIsShowFilledTemplates.Checked = false;
        ViewState["ReportId"] = "0";
        ddlStatus.SelectedValue = "1";
        ddlReportType.SelectedIndex = 0;
        ddlClinicalHeaderId.SelectedIndex = 0;
        txtMarginBottom.Text = string.Empty;
        txtMarginTop.Text = string.Empty;
        txtMarginLeft.Text = string.Empty;
        txtMarginRight.Text = string.Empty;
        txtReportFooterText.Text = string.Empty;
        ddlPageSize.SelectedValue = "A4";


        /*YOGESH */

        // <% --yogesh 4 / 4 / 22 rollback-- %>
        //signheighttxtbox.Text = "";
        //signwidthtxtbox.Text = "";
        //signheighttxtbox.Visible = false;
        //signwidthtxtbox.Visible = false;
        //signheight.Visible = false;
        //signwidth.Visible = false;
        //Span1.Visible = false;
        //Span2.Visible = false;
        //Label8.Visible = false;
        //Label9.Visible = false;




        //chnage palendra

        chkPrintHeaderImage.Checked = false;
        chkPrintFooterImage.Checked = false;

    }
    protected void gvReport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        Clear();
        ViewState["ReportId"] = ((Label)gvReport.Items[e.Item.ItemIndex].FindControl("lblReportId")).Text;
        txtReportHead.Text = ((Label)gvReport.Items[e.Item.ItemIndex].FindControl("lblHeadingName")).Text;
        txtReportName.Text = ((Label)gvReport.Items[e.Item.ItemIndex].FindControl("lblReportName")).Text;
        chkPrintHospitalHeader.Checked = common.myBool(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnIsPrintHospitalHeader")).Value);
        chkPrintDoctorSignature.Checked = common.myBool(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnIsPrintDoctorSignature")).Value);
        chkShowPageNoInPageFooter.Checked = common.myBool(((Label)gvReport.Items[e.Item.ItemIndex].FindControl("lblShowPageNoInPageFooter")).Text);
        chkShowPrintByInPageFooter.Checked = common.myBool(((Label)gvReport.Items[e.Item.ItemIndex].FindControl("lblShowPrintByInPageFooter")).Text);
        chkShowPrintDateInPageFooter.Checked = common.myBool(((Label)gvReport.Items[e.Item.ItemIndex].FindControl("lblShowPrintDateInPageFooter")).Text);
        ddlStatus.SelectedValue = ((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnActive")).Value == "True" ? "1" : "0";
        ddlReportType.SelectedValue = ((Label)gvReport.Items[e.Item.ItemIndex].FindControl("lblReportType")).Text;
        chkIsShowFilledTemplates.Checked = common.myBool(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnIsShowFilledTemplates")).Value);
        chkCheckListRequired.Checked = common.myBool(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnIsCheckListRequired")).Value);
        chkDefaultForOP.Checked = common.myBool(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnIsDefaultForOp")).Value);

        HiddenField hdnPageSize = (HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnPageSize");
        ddlPageSize.SelectedIndex = ddlPageSize.Items.IndexOf(ddlPageSize.Items.FindItemByValue(common.myStr(hdnPageSize.Value)));

        HiddenField hdnHeaderId = (HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnHeaderId");
        ddlClinicalHeaderId.SelectedIndex = ddlClinicalHeaderId.Items.IndexOf(ddlClinicalHeaderId.Items.FindItemByValue(common.myInt(hdnHeaderId.Value).ToString()));

        txtMarginLeft.Text = ((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnMarginLeft")).Value;
        txtMarginRight.Text = ((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnMarginRight")).Value;
        txtMarginTop.Text = ((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnMarginTop")).Value;
        txtMarginBottom.Text = ((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnMarginBottom")).Value;
        txtReportFooterText.Text = ((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnReportFooterText")).Value;


        /*yogesh*/
        // <% --yogesh 4 / 4 / 22 rollback-- %>

        //signheighttxtbox.Text = ((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("lbldocheight")).Value;
        //signwidthtxtbox.Text = ((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("docWidth")).Value;



        //Change Palendra
        chkPrintHeaderImage.Checked = common.myBool(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnPrintHeaderImage")).Value);
        chkPrintFooterImage.Checked = common.myBool(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnPrintFooterImage")).Value);
        hdnPrintHeaderImagePath.Value = common.myStr(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnPrintHeaderImageUrl")).Value);
        hdnPrintFooterImagePath.Value = common.myStr(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnPrintFooterImageURL")).Value);

        txtPrintVersionCode.Text = common.myStr(((HiddenField)gvReport.Items[e.Item.ItemIndex].FindControl("hdnPrintVersionCode")).Value);

        //Change Palendra
        if (common.myStr(txtMarginLeft.Text).Equals("0"))
        {
            txtMarginLeft.Text = string.Empty;
        }
        if (common.myStr(txtMarginRight.Text).Equals("0"))
        {
            txtMarginRight.Text = string.Empty;
        }
        if (common.myStr(txtMarginTop.Text).Equals("0"))
        {
            txtMarginTop.Text = string.Empty;
        }
        if (common.myStr(txtMarginBottom.Text).Equals("0"))
        {
            txtMarginBottom.Text = string.Empty;
        }

        ddlReportType_SelectedIndexChanged(null, null);


    }

    protected void ddlReportType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            switch (common.myStr(ddlReportType.SelectedValue))
            {
                case "HC":
                    chkIsShowFilledTemplates.Enabled = true;
                    chkPrintDoctorSignature.Enabled = true;
                    chkPrintHospitalHeader.Enabled = true;
                    chkCheckListRequired.Enabled = true;
                    break;

                case "PR":
                case "PT":
                    chkIsShowFilledTemplates.Checked = false;
                    chkCheckListRequired.Checked = false;

                    chkIsShowFilledTemplates.Enabled = false;
                    chkPrintDoctorSignature.Enabled = true;
                    chkPrintHospitalHeader.Enabled = true;
                    chkCheckListRequired.Enabled = false;
                    break;

                default:
                    chkIsShowFilledTemplates.Checked = false;
                    if (!common.myStr(ddlReportType.SelectedValue).Equals("DI") && !common.myStr(ddlReportType.SelectedValue).Equals("DE"))
                    {
                        chkPrintDoctorSignature.Checked = false;
                    }
                    chkCheckListRequired.Checked = false;

                    chkIsShowFilledTemplates.Enabled = false;
                    chkPrintDoctorSignature.Enabled = false;
                    chkPrintHospitalHeader.Enabled = true;
                    chkCheckListRequired.Enabled = false;

                    if (common.myStr(ddlReportType.SelectedValue).Equals("DI") || common.myStr(ddlReportType.SelectedValue).Equals("CS") || common.myStr(ddlReportType.SelectedValue).Equals("DE"))
                    {
                        chkPrintDoctorSignature.Enabled = true;
                    }
                    break;
            }

            //if (common.myStr(ddlReportType.SelectedValue).Equals("DI")
            //    || common.myStr(ddlReportType.SelectedValue).Equals("PR")
            //    || common.myStr(ddlReportType.SelectedValue).Equals("PT")
            //    || common.myStr(ddlReportType.SelectedValue).Equals("DE")
            //    || common.myStr(ddlReportType.SelectedValue).Equals("HC"))
            //{
            //    txtMarginBottom.Visible = true;
            //    txtMarginTop.Visible = true;
            //    txtMarginLeft.Visible = true;
            //    txtMarginRight.Visible = true;

            //    lblMarginBottom.Visible = true;
            //    lblMarginTop.Visible = true;
            //    lblMarginLeft.Visible = true;
            //    lblMarginRight.Visible = true;

            //    spMarginBottom.Visible = true;
            //    spMarginTop.Visible = true;
            //    spMarginLeft.Visible = true;
            //    spMarginRight.Visible = true;
            //}
            //else
            //{
            //    txtMarginBottom.Text = string.Empty;
            //    txtMarginTop.Text = string.Empty;
            //    txtMarginLeft.Text = string.Empty;
            //    txtMarginRight.Text = string.Empty;

            //    txtMarginBottom.Visible = false;
            //    txtMarginTop.Visible = false;
            //    txtMarginLeft.Visible = false;
            //    txtMarginRight.Visible = false;

            //    lblMarginBottom.Visible = false;
            //    lblMarginTop.Visible = false;
            //    lblMarginLeft.Visible = false;
            //    lblMarginRight.Visible = false;

            //    spMarginBottom.Visible = false;
            //    spMarginTop.Visible = false;
            //    spMarginLeft.Visible = false;
            //    spMarginRight.Visible = false;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
            //string[] sTime = null;
            //char[] chr = { ' ' };
            //sTime = DateTime.Now.ToString().Split(chr);
            //FileName = objPat.FormatDateDateMonthYear(sTime[0]) + sTime[1] + sTime[2];
            FileName = FileName.Replace("/", "");
            FileName = FileName.Replace(":", "");
            //Response.Write(objPat.FormatDateDateMonthYear(DateTime.Today.ToShortDateString()) + " " + DateTime.Now.Hour.ToString() + "_" + DateTime.Today.Now.ToString() + "_" + DateTime.Now.Second.ToString());

            FileName = FileName.Trim() + "_" + sCategoryId + sExt;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return FileName;
    }





    // yogesh
    protected void chkPrintDoctorSignature_CheckedChanged(object sender, EventArgs e)
    {
        // checkDoctorSignature();
    }

    protected void lbtnSelect_Click(object sender, EventArgs e)
    {
        //if (chkPrintDoctorSignature.Checked)
        //{
        //    signheighttxtbox.Visible = true;
        //    signwidthtxtbox.Visible = true;
        //    signheight.Visible = true;
        //    signwidth.Visible = true;
        //    Span1.Visible = true;
        //    Span2.Visible = true;
        //    Label8.Visible = true;
        //    Label9.Visible = true;
        //}
    }


    // <% --yogesh 4 / 4 / 22 rollback-- %>
    //private void checkDoctorSignature()
    //{
    //    if (chkPrintDoctorSignature.Checked)
    //    {
    //        signheighttxtbox.Visible = true;
    //        signwidthtxtbox.Visible = true;
    //        signheight.Visible = true;
    //        signwidth.Visible = true;
    //        Span1.Visible = true;
    //        Span2.Visible = true;
    //        Label8.Visible = true;
    //        Label9.Visible = true;
    //    }
    //    else
    //    {
    //        signheighttxtbox.Visible = false;
    //        signwidthtxtbox.Visible = false;
    //        signheight.Visible = false;
    //        signwidth.Visible = false;
    //        Span1.Visible = false;
    //        Span2.Visible = false;
    //        Label8.Visible = false;
    //        Label9.Visible = false;
    //    }
    //}
    
    
}


