using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;

public partial class EMR_Masters_ClinicalTemplate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();
    private Hashtable hshIn;
    private Hashtable hshOut;
    DAL.DAL dl;
    StringBuilder strSQL;
    BaseC.EncryptDecrypt objEncDc = new BaseC.EncryptDecrypt();

    private const string DefaultFontType = "DefaultFontType";
    private const string DefaultFontSize = "DefaultFontSize";
    private const string DefaultPageSize = "DefaultPageSize";
    private enum GridTemplate : byte
    {
        TemplateId = 0,
        SerialNo = 1,
        TemplateName = 2,
        Type = 3,
        Code = 4,
        Active = 6,
        encodedby = 5,

        Edit = 7,
        Status = 8,
        TemplateTypeID = 9,
        DisplayTitle = 10
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string TemplateListCache = "TemplateList" + common.myInt(Session["HospitalLocationId"]).ToString() + common.myInt(Session["FacilityId"]).ToString() + common.myInt(Session["UserId"]).ToString();
            if (Cache[TemplateListCache] != null)
            {
                Cache.Remove(TemplateListCache);
            }
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            //trTemplate.Disabled = true;
            GetFontsField();
            ViewState["FieldId"] = "0";
            populateTemplateView();
            DL_Funs fun = new DL_Funs();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Id, Name FROM SpecialisationMaster WITH(NOLOCK) WHERE Active = 1 ORDER BY Name");
            DataSet dsSpecialisation = new DataSet();
            if (Cache["Specialisation"] != null)
            {
                dsSpecialisation = Cache["Specialisation"] as DataSet;
            }
            else
            {
                dsSpecialisation = fun.ExecuteSql(sb);
                Cache.Insert("Specialisation", dsSpecialisation, null, System.DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            ddlSpecialisation.DataSource = dsSpecialisation;
            ddlSpecialisation.DataBind();
            ibtnTemplateSave.Visible = true;

            if (Session["HospitalLocationId"] != null)
            {
                ClinicDefaults cd = new ClinicDefaults(Page);
                ddlFontTypeTemplate.SelectedIndex = ddlFontTypeTemplate.Items.IndexOf(ddlFontTypeTemplate.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
                ddlFontSizeTemplate.SelectedIndex = ddlFontSizeTemplate.Items.IndexOf(ddlFontSizeTemplate.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));

                ddlFontTypeSections.SelectedIndex = ddlFontTypeSections.Items.IndexOf(ddlFontTypeSections.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
                ddlFontSizeSections.SelectedIndex = ddlFontSizeSections.Items.IndexOf(ddlFontSizeSections.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));

                ddlFontTypeFields.SelectedIndex = ddlFontTypeFields.Items.IndexOf(ddlFontTypeFields.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
                ddlFontSizeFields.SelectedIndex = ddlFontSizeFields.Items.IndexOf(ddlFontSizeFields.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));

                ddlFontSizeTemplate.SelectedIndex = 2;
                ddlFontSizeFields.SelectedIndex = 2;
                ddlFontSizeSections.SelectedIndex = 2;

                ddlFontTypeTemplate.SelectedIndex = 1;
                ddlFontTypeSections.SelectedIndex = 1;
                ddlFontTypeFields.SelectedIndex = 1;

                //ddlFontSizeTemplate.SelectedValue = "11pt";
                //ddlFontSizeFields.SelectedValue = "11pt";
                //ddlFontSizeSections.SelectedValue = "11pt";
            }
        }
    }

    private void GetFontsField()
    {
        try
        {
            BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
            DataSet ds = fonts.GetFontDetails("Size");
            DataSet dsFontName = fonts.GetFontDetails("Name");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlFontSizeTemplate.Items.Clear();
                ddlFontSizeTemplate.DataSource = ds;
                ddlFontSizeTemplate.DataTextField = "FontSize";
                ddlFontSizeTemplate.DataValueField = "FontSize";
                ddlFontSizeTemplate.DataBind();

                ddlFontSizeFields.Items.Clear();
                ddlFontSizeFields.DataSource = ds;
                ddlFontSizeFields.DataTextField = "FontSize";
                ddlFontSizeFields.DataValueField = "FontSize";
                ddlFontSizeFields.DataBind();

                ddlFontSizeSections.Items.Clear();
                ddlFontSizeSections.DataSource = ds;
                ddlFontSizeSections.DataTextField = "FontSize";
                ddlFontSizeSections.DataValueField = "FontSize";
                ddlFontSizeSections.DataBind();


            }
            if (dsFontName.Tables[0].Rows.Count > 0)
            {
                ddlFontTypeTemplate.Items.Clear();
                ddlFontTypeTemplate.DataSource = dsFontName;
                ddlFontTypeTemplate.DataTextField = "FontName";
                ddlFontTypeTemplate.DataValueField = "FontId";
                ddlFontTypeTemplate.DataBind();

                ddlFontTypeFields.Items.Clear();
                ddlFontTypeFields.DataSource = dsFontName;
                ddlFontTypeFields.DataTextField = "FontName";
                ddlFontTypeFields.DataValueField = "FontId";
                ddlFontTypeFields.DataBind();

                ddlFontTypeSections.Items.Clear();
                ddlFontTypeSections.DataSource = dsFontName;
                ddlFontTypeSections.DataTextField = "FontName";
                ddlFontTypeSections.DataValueField = "FontId";
                ddlFontTypeSections.DataBind();
            }
            ddlFontSizeFields.Items.Insert(0, "Select");
            ddlFontSizeFields.Items[0].Value = "0";
            ddlFontSizeSections.Items.Insert(0, "Select");
            ddlFontSizeSections.Items[0].Value = "0";
            ddlFontSizeTemplate.Items.Insert(0, "Select");
            ddlFontSizeTemplate.Items[0].Value = "0";
            ddlFontTypeTemplate.Items.Insert(0, "Select");
            ddlFontTypeTemplate.Items[0].Value = "0";
            ddlFontTypeFields.Items.Insert(0, "Select");
            ddlFontTypeFields.Items[0].Value = "0";
            ddlFontTypeSections.Items.Insert(0, "Select");
            ddlFontTypeSections.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void populateTemplateControls()
    {
        try
        {
            ddlTemplateType.Items.Clear();
            strSQL = new StringBuilder();
            //hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
            strSQL.Append("SELECT ID, TypeName FROM EMRTemplateTypes WITH(NOLOCK) ORDER BY TypeName");
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString());
            if (dr.HasRows)
            {
                ddlTemplateType.DataSource = dr;
                ddlTemplateType.DataBind();
            }

            ddlTemplateType.Items.Insert(0, " [ Select ] ");
            ddlTemplateType.Items[0].Value = "0";

            dr.Close();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void populateTemplateView()
    {
        populateTemplateControls();
    }

    protected void btnSearchTemplate_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/Masters/SearchTemplates.aspx";
        RadWindow1.Height = 600;
        RadWindow1.Width = 950;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.OnClientClose = "OnClientCloseSearchTemplate";
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
    }
    protected void btnTemplateNew_OnClick(object sender, EventArgs e)
    {
        ViewState["TemplateEdit"] = "TemplateNew";
        lblStatusT.Visible = false;
        ddlStatus.Visible = false;
        MakeEmptyTemplateInputFields("");
    }
    public void MakeEmptyTemplateInputFields(string msg)
    {
        try
        {
            ViewState["TemplateEdit"] = null;
            lblTemplateMessage.Text = msg;
            ddlTemplateType.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlSpecialisation.SelectedIndex = 0;
            ddlEntryType.SelectedValue = "V";
            rdoApplicableFor.SelectedValue = "B";
            //txtMenuSequence.Text = "";
            txtTemplateName.Text = "";
            txtAssignCode.Text = "";
            txtTemplateName.Text = "";
            txtDocumentNo.Text = string.Empty;
            chkDispInEMR.Checked = false;
            chkDispInNB.Checked = false;
            //        chkTitleNote.Checked = false;
            //if (chkTitleNote.Checked)
            //    trTemplate.Disabled = false;
            //else
            //    trTemplate.Disabled = true;
            //chkpublictemplate.Checked = false;
            chkTitleNote.Checked = false;
            chkBoldSections.Checked = true; //
            chkBoldFields.Checked = true; //
            chkBoldTemplate.Checked = false;
            chkItalicSections.Checked = false;
            chkItalicFields.Checked = false;
            chkItalicTemplate.Checked = false;
            chkUnderlineSections.Checked = true;//
            chkUnderlineFields.Checked = false;
            chkUnderlineTemplate.Checked = false;
            txtColorSections.Text = "000000";
            txtColorFields.Text = "000000";
            txtColorTemplate.Text = "000000";
            //ddlListStyleSections.SelectedIndex = 0;
            //ddlListStyleFields.SelectedIndex = 0;
            //ddlListStyleTemplate.SelectedIndex = 0;
            ddlFontTypeSections.SelectedIndex = 1;
            ddlFontTypeFields.SelectedIndex = 1;
            ddlFontTypeTemplate.SelectedIndex = 1;
            ddlFontSizeSections.SelectedIndex = 2;
            ddlFontSizeFields.SelectedIndex = 2;
            ddlFontSizeTemplate.SelectedIndex = 2;
            DdlTemSpace.SelectedValue = "1";
            chkIsConfidentail.Checked = false;
            chkIsMandatory.Checked = false;
            chkIsShowCloseEncounter.Checked = false;

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void SaveTemplate_OnClick(Object sender, EventArgs e)
    {

        if (ddlFontSizeTemplate.SelectedIndex == 0 || ddlFontSizeFields.SelectedIndex == 0 || ddlFontSizeSections.SelectedIndex == 0)
        {
            Alert.ShowAjaxMsg("Please select size", this);
            return;
        }
        if (ddlFontTypeTemplate.SelectedIndex == 0 || ddlFontTypeSections.SelectedIndex == 0 || ddlFontTypeFields.SelectedIndex == 0)
        {
            Alert.ShowAjaxMsg("Please select font", this);
            return;
        }


        SaveUpdateTemplate();

        hdnTemplateId.Value = "";
        hdnTemplateTypeId.Value = "";

        ddlFontSizeTemplate.SelectedIndex = 2;
        ddlFontSizeFields.SelectedIndex = 2;
        ddlFontSizeSections.SelectedIndex = 2;

        ddlFontTypeTemplate.SelectedIndex = 1;
        ddlFontTypeSections.SelectedIndex = 1;
        ddlFontTypeFields.SelectedIndex = 1;
    }

    private void SaveUpdateTemplate()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (txtTemplateName.Text.ToString().Trim().Length == 0)
            {
                Alert.ShowAjaxMsg("Name Cannot Be Blank...", this.Page);
                return;
            }

            lblStatusT.Visible = false;
            ddlStatus.Visible = false;
            hshIn = new Hashtable();
            hshOut = new Hashtable();

            if (chkBoldTemplate.Checked)
            {
                hshIn.Add("@TemplateBold", "1");
            }
            if (chkItalicTemplate.Checked)
            {
                hshIn.Add("@TemplateItalic", "1");
            }
            if (chkUnderlineTemplate.Checked)
            {
                hshIn.Add("@TemplateUnderline", "1");
            }
            if (common.myStr(txtColorTemplate.Text) != string.Empty)
            {
                hshIn.Add("@TemplateForecolor", common.myStr(txtColorTemplate.Text.Trim()));
            }
            //hshIn.Add("@TemplateListStyle", ddlListStyleTemplate.SelectedValue == "0" ? null : ddlListStyleTemplate.SelectedValue);
            // hshIn.Add("@TemplateListStyle", null);

            if (!common.myStr(ddlFontTypeTemplate.SelectedValue).Equals("0"))
            {
                hshIn.Add("@TemplateFontStyle", common.myInt(ddlFontTypeTemplate.SelectedValue));
            }
            if (!common.myStr(ddlFontSizeTemplate.SelectedValue).Equals("0"))
            {
                hshIn.Add("@TemplateFontSize", common.myStr(ddlFontSizeTemplate.SelectedValue));
            }

            //if (rblpagetype.SelectedValue == "1")
            {
                hshIn.Add("@chvTemplateName", bc.ParseQ(txtTemplateName.Text));
            }
            if (chkBoldSections.Checked)
            {
                hshIn.Add("@SectionsBold", "1");
            }
            if (chkItalicSections.Checked)
            {
                hshIn.Add("@SectionsItalic", "1");
            }
            if (chkUnderlineSections.Checked)
            {
                hshIn.Add("@SectionsUnderline", "1");
            }
            if (common.myStr(txtColorSections.Text) != string.Empty)
            {
                hshIn.Add("@SectionsForecolor", common.myStr(txtColorSections.Text.Trim()));
            }
            //hshIn.Add("@SectionsListStyle", ddlListStyleSections.SelectedValue == "0" ? null : ddlListStyleSections.SelectedValue);
            // hshIn.Add("@SectionsListStyle", null);
            if (!common.myStr(ddlFontTypeSections.SelectedValue).Equals("0"))
            {
                hshIn.Add("@SectionsFontStyle", common.myInt(ddlFontTypeSections.SelectedValue));
            }
            if (!common.myStr(ddlFontSizeSections.SelectedValue).Equals("0"))
            {
                hshIn.Add("@SectionsFontSize", common.myStr(ddlFontSizeSections.SelectedValue));
            }
            if (chkBoldFields.Checked)
            {
                hshIn.Add("@FieldsBold", "1");
            }
            if (chkItalicFields.Checked)
            {
                hshIn.Add("@FieldsItalic", "1");
            }
            if (chkUnderlineFields.Checked)
            {
                hshIn.Add("@FieldsUnderline", "1");
            }
            if (common.myStr(txtColorFields.Text.Trim()) != string.Empty)
            {
                hshIn.Add("@FieldsForecolor", common.myStr(txtColorFields.Text.Trim()));
            }
            //  hshIn.Add("@FieldsListStyle", null);
            if (!common.myStr(ddlFontTypeFields.SelectedValue).Equals("0"))
            {
                hshIn.Add("@FieldsFontStyle", common.myInt(ddlFontTypeFields.SelectedValue));
            }
            if (!common.myStr(ddlFontSizeFields.SelectedValue).Equals("0"))
            {
                hshIn.Add("@FieldsFontSize", common.myStr(ddlFontSizeFields.SelectedValue));
            }

            hshIn.Add("@bitDisplayTitle", chkTitleNote.Checked ? 1 : 0);


            if (chkDispInEMR.Checked == true)
                hshIn.Add("@bitDisplayInMenuEMR", 1);
            else
                hshIn.Add("@bitDisplayInMenuEMR", 0);

            if (chkDispInNB.Checked == true)
                hshIn.Add("@bitDisplayInMenuNB", 1);
            else
                hshIn.Add("@bitDisplayInMenuNB", 0);


            hshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            hshIn.Add("@TemplateSpaceNumber", DdlTemSpace.SelectedValue);

            hshIn.Add("@chrApplicableFor", common.myStr(rdoApplicableFor.SelectedValue));
            //hshIn.Add("@intMenuSequence", common.myInt(txtMenuSequence.Text));
            hshIn.Add("@intMenuSequence", 0);

            hshIn.Add("@bitIsConfidential", chkIsConfidentail.Checked);

            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            hshIn.Add("@bitIsMandatory", chkIsMandatory.Checked ? 1 : 0);
            hshIn.Add("@bitShowCloseEncounter", common.myBool(chkIsShowCloseEncounter.Checked));
            hshIn.Add("@bitIsMandatoryForMarkForDischarge", common.myBool(chkIsMandatoryForMarkForDischarge.Checked));
            if (ViewState["TemplateEdit"] != null)
            {
                if (ViewState["TemplateEdit"].ToString() == "TemplateEdit")
                {
                    int i = 0;
                    //if (rblpagetype.SelectedValue == "1")
                    //{
                    hshIn.Add("@ID", common.myInt(ViewState["TemplateId"]));

                    hshIn.Add("@chvAssignCode", bc.ParseQ(txtAssignCode.Text));
                    hshIn.Add("@chvDocumentNo", bc.ParseQ(txtDocumentNo.Text));

                    if (ddlTemplateType.SelectedValue != "0")
                    {
                        hshIn.Add("@inyTemplateTypeID", bc.ParseQ(ddlTemplateType.SelectedValue));
                    }
                    if (ddlSpecialisation.SelectedValue != "0")
                    {
                        hshIn.Add("@inySpecialisationId", bc.ParseQ(ddlSpecialisation.SelectedValue));
                    }

                    hshIn.Add("@Active", bc.ParseQ(ddlStatus.SelectedValue));

                    //hshIn.Add("@bitPublicTemplate", chkpublictemplate.Checked == true ? 1 : 0);
                    hshIn.Add("@bitPublicTemplate", 0);
                    hshIn.Add("@chrEntryType", common.myStr(ddlEntryType.SelectedValue));
                    if (chkIsMandatoryForIP.Checked)
                    {
                        hshIn.Add("@bitIsMandatoryForIP", 1);
                    }
                 
                    hshIn.Add("@intIPTATHours", common.myInt(txtIPTATHours.Text));

                    hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRUpdateTemplate", hshIn, hshOut);
                    //  i = dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRUpdateTemplate", hshIn, hshOut);
                    //}
                    //else
                    //{
                    //    hshIn.Add("@intModuleId", Convert.ToInt32(Session["ModuleId"]));
                    //    hshIn.Add("@intPageID", Convert.ToInt32(ViewState["TemplateId"]));
                    //    hshIn.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                    //    i = dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRSaveUpdateTemplateStatic", hshIn, hshOut);

                    //}
                    string sMsg = hshOut["@chvErrorStatus"].ToString();
                    if (i == 0)
                    {
                        txtIPTATHours.Text = string.Empty;
                        txtIPTATHours.Enabled = false;
                        chkIsMandatoryForIP.Checked = false;
                        //gvTemplates.EditIndex = -1;
                        // MakeEmptyTemplateInputFields("Template has been updated successfully.");
                        MakeEmptyTemplateInputFields(sMsg);
                    }
                }
            }
            else
            {
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //if (rblpagetype.SelectedValue == "1")
                //{

                if (ddlTemplateType.SelectedIndex != 0)
                {
                    hshIn.Add("@inyTemplateTypeID", ddlTemplateType.SelectedItem.Value);
                }
                hshIn.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                hshIn.Add("@chvAssignCode", bc.ParseQ(txtAssignCode.Text.ToString().Trim()));
                hshIn.Add("@chvDocumentNo", bc.ParseQ(txtDocumentNo.Text.ToString().Trim()));                

                //hshIn.Add("@bitPublicTemplate", chkpublictemplate.Checked ? 1 : 0);
                hshIn.Add("@bitPublicTemplate", 0);

                if (ddlSpecialisation.SelectedIndex != 0)
                {
                    hshIn.Add("@inySpecialisationId", ddlSpecialisation.SelectedValue);
                }
                if (common.myStr(Session["ModuleName"]) == "OT Module")
                {
                    hshIn.Add("@intModuleId", common.myInt(Session["ModuleId"]));
                }
                if(chkIsMandatoryForIP.Checked)
                {
                    hshIn.Add("@bitIsMandatoryForIP", 1);
                }
                hshIn.Add("@intIPTATHours", common.myInt(txtIPTATHours.Text));
                
                hshIn.Add("@chrEntryType", common.myStr(ddlEntryType.SelectedValue));

                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveTemplate", hshIn, hshOut);
                
                //}
                //else
                //{
                //    hshIn.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
                //    hshIn.Add("@PageID", Convert.ToInt32(ViewState["TemplateId"]));
                //    hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveTemplateStatic", hshIn, hshOut);
                //}

                MakeEmptyTemplateInputFields("Template has been saved successfully.");
            }

            ClinicDefaults cd = new ClinicDefaults(Page);

            ddlFontTypeTemplate.SelectedIndex = ddlFontTypeTemplate.Items.IndexOf(ddlFontTypeTemplate.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
            ddlFontSizeTemplate.SelectedIndex = ddlFontSizeTemplate.Items.IndexOf(ddlFontSizeTemplate.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));

            ddlFontTypeSections.SelectedIndex = ddlFontTypeSections.Items.IndexOf(ddlFontTypeSections.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
            ddlFontSizeSections.SelectedIndex = ddlFontSizeSections.Items.IndexOf(ddlFontSizeSections.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));

            ddlFontTypeFields.SelectedIndex = ddlFontTypeFields.Items.IndexOf(ddlFontTypeFields.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
            ddlFontSizeFields.SelectedIndex = ddlFontSizeFields.Items.IndexOf(ddlFontSizeFields.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));

            string TemplateListCache = "TemplateList" + common.myInt(Session["HospitalLocationId"]).ToString() + common.myInt(Session["FacilityId"]).ToString() + common.myInt(Session["UserId"]).ToString();
            chkIsConfidentail.Checked = false;
            Cache[TemplateListCache] = null;
            txtIPTATHours.Text = string.Empty;
            txtIPTATHours.Enabled = false;
            chkIsMandatoryForIP.Checked = false;
            chkIsShowCloseEncounter.Checked = false;
            chkIsMandatoryForMarkForDischarge.Checked = false;
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvTemplates_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            //if (rblpagetype.SelectedValue == "2")
            //{
            //    e.Row.Cells[3].Visible = false;
            //    e.Row.Cells[4].Visible = false;
            //    e.Row.Cells[5].Visible = false;
            //    e.Row.Cells[6].Visible = false;
            //    e.Row.Cells[11].Visible = false;
            //}

            e.Row.Cells[Convert.ToByte(GridTemplate.TemplateId)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridTemplate.Status)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridTemplate.TemplateTypeID)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridTemplate.DisplayTitle)].Visible = false;
        }
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        {
            if (Server.HtmlDecode(e.Row.Cells[Convert.ToByte(GridTemplate.SerialNo)].Text).Trim() == "")
            {
                e.Row.Visible = false;
            }
            LinkButton lblName = (LinkButton)e.Row.FindControl("lnkTname");
            Label lblEncBy = (Label)e.Row.FindControl("lblSpecialisation");

            //if (lblEncBy.Text.Length > 10)
            //    lblEncBy.Text = lblEncBy.Text.PadRight(35).Substring(0, 35) + "...";
            //else
            //    lblEncBy.Text = lblEncBy.Text.PadRight(35).Substring(0, 35);

            //ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
            //string jQuery = "var color=$get('" + e.Row.ClientID + "').style.backgroundColor;"
            //    + "$get('" + e.Row.ClientID + "').style.backgroundColor='Pink';"
            //    + "if(!confirm('Are you sure you want to De-Activate \\'" + lblName.Text + "\\' Template?'))"
            //    + "{$get('" + e.Row.ClientID + "').style.backgroundColor=color; return false;}";
            //img.Attributes.Add("onclick", jQuery);
            //if (e.Row.Cells[Convert.ToByte(GridTemplate.Status)].Text.ToString().Trim() == "0")
            //{
            //    img.Enabled = false;
            //}
        }
        //if (e.Row.RowState == DataControlRowState.Edit)
        //{
        //    DropDownList ddlTStatus = (DropDownList)e.Row.FindControl("ddlTStatus");
        //    String strStatus = e.Row.Cells[Convert.ToByte(GridTemplate.Status)].Text.ToString().Trim();

        //    if (strStatus == "0")
        //    {
        //        ddlTStatus.Enabled = true;
        //    }
        //    else
        //    {
        //        ddlTStatus.Enabled = false;
        //    }
        //}
        //if ((e.Row.RowState & DataControlRowState.Edit) > 0)
        //{
        //    DropDownList ddlTStatus = (DropDownList)e.Row.FindControl("ddlTStatus");
        //    Label lblName2 = (Label)e.Row.FindControl("LName2");
        //    String strStatus = e.Row.Cells[Convert.ToByte(GridTemplate.Status)].Text.ToString().Trim();

        //    if (strStatus == "0")
        //    {
        //        ddlTStatus.Visible = true;
        //        lblName2.Visible = false;
        //    }
        //    else
        //    {
        //        ddlTStatus.Visible = false;
        //        lblName2.Visible = true;
        //    }
        //}
    }

    //protected void lnkTname_OnClick(object sender, EventArgs e)
    //{
    //    if (rblpagetype.SelectedValue == "1")
    //    {
    //        LinkButton lnk = sender as LinkButton;
    //        //Session["TmpId"] = lnk.CommandArgument;
    //        //Session["nm"] = lnk.Text;
    //        Response.Redirect("TemplateMaster1.aspx?tempid=" + lnk.CommandArgument, false);
    //    }
    //}
    protected void chkpublictemplate_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void btnclose_OnClick(object sender, EventArgs e)
    {
    }

    protected void btnbtnTempPreview_OnClick(object sender, EventArgs e)
    {
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        try
        {
            lblTemplateMessage.Text = "";
            ViewState["TemplateId"] = common.myInt(hdnTemplateId.Value);
            if (common.myInt(hdnTemplateId.Value) == 0)
            {
                return;
            }

            DataSet ds = getTemplateData();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ClinicDefaults cd = new ClinicDefaults(Page);

                DataRow DR = ds.Tables[0].Rows[0];

                txtTemplateName.Text = common.myStr(DR["TemplateName"]).Trim();
                if (common.myInt(hdnTemplateTypeId.Value) == 1)
                {
                    ddlSpecialisation.SelectedIndex = ddlSpecialisation.Items.IndexOf(ddlSpecialisation.Items.FindByValue(common.myStr(DR["SpecialisationId"])));
                    ddlTemplateType.SelectedIndex = ddlTemplateType.Items.IndexOf(ddlTemplateType.Items.FindByValue(common.myStr(DR["TemplateTypeID"])));
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(common.myStr(DR["Status"])));

                    txtAssignCode.Text = common.myStr(DR["AssignCode"]).Trim();
                    txtDocumentNo.Text= common.myStr(DR["DocumentNo"]).Trim();
                    DdlTemSpace.SelectedIndex = DdlTemSpace.Items.IndexOf(DdlTemSpace.Items.FindByValue(common.myStr(DR["TemplateSpaceNumber"])));
                    ddlEntryType.SelectedIndex = ddlEntryType.Items.IndexOf(ddlEntryType.Items.FindByValue(common.myStr(DR["EntryType"])));
                }

                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshIn = new Hashtable();
                hshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hshIn.Add("@intTemplateId", common.myInt(hdnTemplateId.Value));

                ds = new DataSet();

                //if (rblpagetype.SelectedValue == "1")
                //{
                ds = dl.FillDataSet(CommandType.Text, "SELECT DisplayTitle, SpecialisationId, PublicTemplate, TemplateBold, TemplateItalic, TemplateUnderline, TemplateForecolor, TemplateListStyle, TemplateFontStyle, TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline, SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic, FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize, Active, ISNULL(TemplateSpaceNumber, 1) as TemplateSpaceNumber, ISNULL(DisplayInMenu,0) DisplayInMenuEMR , ISNULL(DisplayInMenuNB,0) DisplayInMenuNB, ISNULL(ApplicableFor, 'B') AS ApplicableFor, ISNULL(MenuSequence,0) as MenuSequence, IsConfidential, ISNULL(IsMandatory,0) AS IsMandatory,IsMandatoryForIP,IPTATHours,IsShowCloseEncounter,IsMandatoryForMarkForDischarge FROM EMRTemplate WITH(NOLOCK) WHERE Id=@intTemplateId AND HospitalLocationId=@inyHospitalLocationId ORDER BY MenuSequence", hshIn);
                //}
                //else
                //{
                //    ds = dl.FillDataSet(CommandType.Text, "Select TemplateDisplayTitle, TemplateBold, TemplateItalic, TemplateUnderline, TemplateForecolor, TemplateListStyle, TemplateFontStyle, TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline, SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic, FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize,isnull(TemplateSpaceNumber ,1) as TemplateSpaceNumber, isnull(DisplayInMenu,0) DisplayInMenuEMR , isnull(DisplayInMenuNB,0) DisplayInMenuNB, ISNULL(ApplicableFor, 'B') AS ApplicableFor from EMRTemplateStatic where PageId=@intTemplateId And HospitalLocationId=@inyHospitalLocationId", hshIn);
                //}

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0] as DataRow;
                    if (dr["TemplateBold"].ToString() == "True") chkBoldTemplate.Checked = true; else chkBoldTemplate.Checked = false; ;
                    if (dr["TemplateItalic"].ToString() == "True") chkItalicTemplate.Checked = true; else chkItalicTemplate.Checked = false; ;
                    if (dr["TemplateUnderline"].ToString() == "True") chkUnderlineTemplate.Checked = true; else chkUnderlineTemplate.Checked = false; ;
                    if (dr["TemplateForecolor"].ToString() == "") txtColorTemplate.Text = "000000"; else txtColorTemplate.Text = dr["TemplateForecolor"].ToString();
                    //if (dr["TemplateListStyle"].ToString() == "") ddlListStyleTemplate.SelectedIndex = 0; else ddlListStyleTemplate.SelectedValue = dr["TemplateListStyle"].ToString();
                    if (dr["TemplateFontStyle"].ToString() == "")
                        ddlFontTypeTemplate.SelectedIndex = ddlFontTypeTemplate.Items.IndexOf(ddlFontTypeTemplate.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
                    else
                        ddlFontTypeTemplate.SelectedIndex = ddlFontTypeTemplate.Items.IndexOf(ddlFontTypeTemplate.Items.FindByValue(common.myStr(dr["TemplateFontStyle"])));

                    if (dr["TemplateFontSize"].ToString() == "")
                        ddlFontSizeTemplate.SelectedIndex = ddlFontSizeTemplate.Items.IndexOf(ddlFontSizeTemplate.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));
                    else
                        ddlFontSizeTemplate.SelectedIndex = ddlFontSizeTemplate.Items.IndexOf(ddlFontSizeTemplate.Items.FindByValue(common.myStr(dr["TemplateFontSize"])));

                    //if (rblpagetype.SelectedValue == "1")
                    //{
                    if (dr["DisplayTitle"].ToString() == "True") chkTitleNote.Checked = true; else chkTitleNote.Checked = false; ;
                    //if (dr["PublicTemplate"].ToString() == "True") chkpublictemplate.Checked = true; else chkpublictemplate.Checked = false; ;
                    if (dr["IsMandatory"].ToString() == "True") chkIsMandatory.Checked = true; else chkIsMandatory.Checked = false; ;
                    //}
                    //else
                    //{
                    //    if (dr["TemplateDisplayTitle"].ToString() == "True") chkTitleNote.Checked = true; else chkTitleNote.Checked = false; ;
                    //}
                    if (dr["DisplayInMenuEMR"].ToString() == "True") chkDispInEMR.Checked = true; else chkDispInEMR.Checked = false;
                    if (dr["DisplayInMenuNB"].ToString() == "True") chkDispInNB.Checked = true; else chkDispInNB.Checked = false;

                    if (dr["SectionsBold"].ToString() == "True") chkBoldSections.Checked = true; else chkBoldSections.Checked = false; ;
                    if (dr["SectionsItalic"].ToString() == "True") chkItalicSections.Checked = true; else chkItalicSections.Checked = false; ;
                    if (dr["SectionsUnderline"].ToString() == "True") chkUnderlineSections.Checked = true; else chkUnderlineSections.Checked = false; ;
                    if (dr["SectionsForecolor"].ToString() == "") txtColorSections.Text = "000000"; else txtColorSections.Text = dr["SectionsForecolor"].ToString();
                    //if (dr["SectionsListStyle"].ToString() == "") ddlListStyleSections.SelectedIndex = 0; else ddlListStyleSections.SelectedValue = dr["SectionsListStyle"].ToString();
                    if (dr["SectionsFontStyle"].ToString() == "")
                        ddlFontTypeSections.SelectedIndex = ddlFontTypeSections.Items.IndexOf(ddlFontTypeSections.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
                    else
                        ddlFontTypeSections.SelectedValue = dr["SectionsFontStyle"].ToString();
                    if (dr["SectionsFontSize"].ToString() == "")
                        ddlFontSizeSections.SelectedIndex = ddlFontSizeSections.Items.IndexOf(ddlFontSizeSections.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));
                    else
                        ddlFontSizeSections.SelectedIndex = ddlFontSizeSections.Items.IndexOf(ddlFontSizeSections.Items.FindByValue(common.myStr(dr["SectionsFontSize"])));

                    if (dr["FieldsBold"].ToString() == "True") chkBoldFields.Checked = true; else chkBoldFields.Checked = false; ;
                    if (dr["FieldsItalic"].ToString() == "True") chkItalicFields.Checked = true; else chkItalicFields.Checked = false; ;
                    if (dr["FieldsUnderline"].ToString() == "True") chkUnderlineFields.Checked = true; else chkUnderlineFields.Checked = false; ;
                    if (dr["FieldsForecolor"].ToString() == "") txtColorFields.Text = "000000"; else txtColorFields.Text = dr["FieldsForecolor"].ToString();
                    //if (dr["FieldsListStyle"].ToString() == "") ddlListStyleFields.SelectedIndex = 0; else ddlListStyleFields.SelectedValue = dr["FieldsListStyle"].ToString();
                    if (dr["FieldsFontStyle"].ToString() == "")
                        ddlFontTypeFields.SelectedIndex = ddlFontTypeFields.Items.IndexOf(ddlFontTypeFields.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
                    else
                        ddlFontTypeFields.SelectedValue = dr["FieldsFontStyle"].ToString();
                    if (dr["FieldsFontSize"].ToString() == "")
                        ddlFontSizeFields.SelectedIndex = ddlFontSizeFields.Items.IndexOf(ddlFontSizeFields.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));
                    else
                        ddlFontSizeFields.SelectedIndex = ddlFontSizeFields.Items.IndexOf(ddlFontSizeFields.Items.FindByValue(common.myStr(dr["FieldsFontSize"])));


                    DdlTemSpace.SelectedValue = dr["TemplateSpaceNumber"].ToString();

                    rdoApplicableFor.SelectedIndex = rdoApplicableFor.Items.IndexOf(rdoApplicableFor.Items.FindByValue(common.myStr(dr["ApplicableFor"])));

                    //txtMenuSequence.Text = common.myInt(dr["MenuSequence"]).ToString();
                    chkIsConfidentail.Checked = common.myBool(dr["IsConfidential"]);
                    chkIsMandatory.Checked = common.myBool(dr["IsMandatory"]);
                    chkIsMandatoryForIP.Checked = common.myBool(dr["IsMandatoryForIP"]);
                    txtIPTATHours.Text = common.myStr(dr["IPTATHours"]);
                    if(!common.myStr(txtIPTATHours.Text).Equals(string.Empty))
                    {
                        txtIPTATHours.Enabled = true;
                    }
                    chkIsShowCloseEncounter.Checked= common.myBool(dr["IsShowCloseEncounter"]);
                    chkIsMandatoryForMarkForDischarge.Checked= common.myBool(dr["IsMandatoryForMarkForDischarge"]); 
                    //}
                }
                else
                {
                    chkTitleNote.Checked = false;
                    chkDispInNB.Checked = false;
                    chkDispInEMR.Checked = false;
                    chkBoldSections.Checked = false; //
                    chkBoldFields.Checked = false; //
                    chkBoldTemplate.Checked = false;
                    chkItalicSections.Checked = false;
                    chkItalicFields.Checked = false;
                    chkItalicTemplate.Checked = false;
                    chkUnderlineSections.Checked = false; //
                    chkUnderlineFields.Checked = false;
                    chkUnderlineTemplate.Checked = false;
                    chkIsMandatory.Checked = false;
                    txtColorSections.Text = "000000";
                    txtColorFields.Text = "000000";
                    txtColorTemplate.Text = "000000";
                    //ddlListStyleSections.SelectedIndex = 0;
                    //ddlListStyleFields.SelectedIndex = 0;
                    //ddlListStyleTemplate.SelectedIndex = 0;
                    ddlFontTypeSections.SelectedIndex = 1;
                    ddlFontTypeFields.SelectedIndex = 1;
                    ddlFontTypeTemplate.SelectedIndex = 1;
                    ddlFontSizeSections.SelectedIndex = 2;
                    ddlFontSizeFields.SelectedIndex = 2;
                    ddlFontSizeTemplate.SelectedIndex = 2;
                    DdlTemSpace.SelectedValue = "1";
                    rdoApplicableFor.SelectedValue = "B";
                    //txtMenuSequence.Text = "";
                    chkIsConfidentail.Checked = false;
                    chkIsMandatory.Checked = false;
                    chkIsShowCloseEncounter.Checked = false;
                }

                ViewState["TemplateEdit"] = "TemplateEdit";
                lblStatusT.Visible = true;
                ddlStatus.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private DataSet getTemplateData()
    {
        DataSet dsTemplate = new DataSet();
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if (common.myInt(hdnTemplateTypeId.Value) == 2)
            {
                string qry = "SELECT Row_Number() Over (ORDER BY PageName) SerialNo, PageID as TemplateId, " +
                    "PageName as TemplateName , '' as type, '' as Code ,'' as Specialisation, 0 as Active, " +
                    "0 as Status, '' as TemplateTypeID, '' as DisplayTitle, 1 as TemplateSpaceNumber, " +
                    "'V' AS EntryType, '' AssignCode, 0 SpecialisationId, '' DocumentNo " +
                    "FROM SecModulePages WITH(NOLOCK) " +
                    "WHERE ModuleID = 3 AND ISNULL(Hierarchy,0) = 0 AND ISNULL(StaticPage,0) = 0 AND Active = 1";

                dsTemplate = dl.FillDataSet(CommandType.Text, qry);
            }
            else
            {
                StringBuilder strSQL = new StringBuilder();
                Hashtable hshIn = new Hashtable();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                hshIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
                hshIn.Add("@intTemplateId", common.myInt(hdnTemplateId.Value));


                dsTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspGetTemplate", hshIn);

                if (dsTemplate.Tables[0].Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    DataColumn dcTemplateName = new DataColumn("TemplateName");
                    DataColumn dcTemplateId = new DataColumn("TemplateId");
                    DataColumn dcSpecialisation = new DataColumn("Specialisation");
                    DataColumn dcActive = new DataColumn("Active");
                    DataColumn dcSerialNo = new DataColumn("SerialNo");
                    DataColumn dcType = new DataColumn("Type");
                    DataColumn dcCode = new DataColumn("Code");
                    DataColumn dcStatus = new DataColumn("Status");
                    DataColumn dcTemplateTypeID = new DataColumn("TemplateTypeID");
                    DataColumn dcDisplayTitle = new DataColumn("DisplayTitle");
                    DataColumn dcTemplateSpace = new DataColumn("TemplateSpaceNumber");
                    DataColumn dcEntryType = new DataColumn("EntryType");

                    dt.Columns.Add(dcTemplateName);
                    dt.Columns.Add(dcTemplateId);
                    dt.Columns.Add(dcSpecialisation);
                    dt.Columns.Add(dcActive);
                    dt.Columns.Add(dcSerialNo);
                    dt.Columns.Add(dcType);
                    dt.Columns.Add(dcCode);
                    dt.Columns.Add(dcStatus);
                    dt.Columns.Add(dcTemplateTypeID);
                    dt.Columns.Add(dcDisplayTitle);
                    dt.Columns.Add(dcTemplateSpace);
                    dt.Columns.Add(dcEntryType);

                    DataRow drow = dt.NewRow();

                    drow[0] = "";
                    drow[1] = "";
                    drow[2] = "";
                    drow[3] = "";
                    drow[4] = "";
                    drow[5] = "";
                    drow[6] = "";
                    drow[7] = "";
                    drow[8] = "";
                    drow[9] = "";
                    drow[10] = "";
                    drow[11] = "V";

                    dt.Rows.Add(drow);

                    dsTemplate.Tables.Add(dt);

                }
            }

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return dsTemplate;
    }


    protected void chkIsMandatoryForIP_CheckedChanged(object sender, EventArgs e)
    {
        if(chkIsMandatoryForIP.Checked)
        {
            txtIPTATHours.Enabled = true;

        }
        else
        {
            txtIPTATHours.Text = string.Empty;
            txtIPTATHours.Enabled =false;
        }
    }
    protected void btnTagging_OnClick(object sender,EventArgs e)
    {
        Response.Redirect("FacilityDepartmentLinking.aspx?For=CM&MPG=P22115&Mpg=C22115", false);
    }
}