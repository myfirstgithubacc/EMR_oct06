using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net;

public partial class EMR_ClinicalPathway_CopyEMRTreatmentPlan : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    BaseC.DiagnosisDA objDiag;
    private string cCtlType = string.Empty;
    DataSet objDs = new DataSet();
    DataTable dt = new DataTable();
    clsExceptionLog objException = new clsExceptionLog();

    private enum enumNonT : byte
    {
        FieldId = 0,
        FieldName = 1,
        FieldType = 2,
        Values = 3,
        Remarks = 4,
        ParentId = 5,
        ParentValue = 6,
        Hierarchy = 7,
        SectionId = 8,
        DataObjectId = 9,
        IsMandatory = 10,
        MandatoryType = 11,
        EmployeeTypeId = 12
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["Source"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            
            if (!IsPostBack)
            {
                hdnCopyDayDetailId.Value = common.myStr(Request.QueryString["CopyDayDetailId"]);
                hdnSelectedDayDetailId.Value = common.myStr(Request.QueryString["SelectedDayDetailId"]);
                hdnSelectedPlanId.Value = common.myStr(Request.QueryString["PlanId"]);
                hdnSelectedDayId.Value = common.myStr(Request.QueryString["SelectedDayId"]);

                hdnCopyDayId.Value= common.myStr(Request.QueryString["CopyDayId"]);

                lblPlanName.Text= common.myStr(Request.QueryString["PlanName"]);

                lblCopyDayName.Text = common.myStr(Request.QueryString["CopyDayDetailName"]);
                lblSelectedDayName.Text = common.myStr(Request.QueryString["SelectedDayDetailName"]);

                GetServiceData(common.myInt(hdnSelectedPlanId.Value), common.myInt(hdnCopyDayId.Value),common.myInt(hdnCopyDayDetailId.Value), "");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
   
    protected void GetServiceData(int TemplatePlanId, int DayId, int DayDetailId, string TemplateType)
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        DataView dv = new DataView();
        try
        {
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanDetails";

            objRoot.TemplateId = common.myStr(hdnSelectedPlanId.Value);
            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;
            objRoot.TemplateType = "";
            objRoot.IsCopy = true;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            objDs = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSelectedServices.DataSource = ds.Tables[0];
                gvSelectedServices.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.InsertAt(dr, 0);

                gvSelectedServices.DataSource = ds.Tables[0];
                gvSelectedServices.DataBind();
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                gvSpecialsation.DataSource = ds.Tables[2];
                gvSpecialsation.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[2].NewRow();
                ds.Tables[2].Rows.InsertAt(dr, 0);
                gvSpecialsation.DataSource = ds.Tables[2];
                gvSpecialsation.DataBind();
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                gvService.DataSource = ds.Tables[3];
                gvService.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[3].NewRow();
                ds.Tables[3].Rows.InsertAt(dr, 0);
                gvService.DataSource = ds.Tables[3];
                gvService.DataBind();
            }
            if (ds.Tables[4].Rows.Count > 0)
            {
                gvDrugClass.DataSource = ds.Tables[4];
                gvDrugClass.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[4].NewRow();
                ds.Tables[4].Rows.InsertAt(dr, 0);

                gvDrugClass.DataSource = ds.Tables[4];
                gvDrugClass.DataBind();
            }
            if (ds.Tables[5].Rows.Count > 0)
            {
                gvPrescription.DataSource = ds.Tables[5];
                gvPrescription.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[5].NewRow();
                ds.Tables[5].Rows.InsertAt(dr, 0);

                gvPrescription.DataSource = ds.Tables[5];
                gvPrescription.DataBind();
            }
            if (ds.Tables[6].Rows.Count > 0)
            {
                gvTemplateLis.DataSource = ds.Tables[6];
                gvTemplateLis.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[6].NewRow();
                ds.Tables[6].Rows.InsertAt(dr, 0);

                gvTemplateLis.DataSource = ds.Tables[6];
                gvTemplateLis.DataBind();
            }
            if (ds.Tables[7].Rows.Count > 0)
            {
                txtChiefComplaints.Text = "";
                txtFreeInstruction.Text = "";
                txtPlanOfCare.Text = "";
                txtHistory.Text = "";
                txtExamination.Text = "";
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='CH'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtChiefComplaints.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='IN'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtFreeInstruction.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='POC'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtPlanOfCare.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='HIS'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtHistory.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='EXM'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtExamination.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
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
            client = null;
            objRoot = null;
            ds.Dispose();
            dv.Dispose();
        }
    }
    protected void gvSelectedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      
        DataView ddv = new DataView();
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                DataView objDv = null;
                DataView objDvValue;
                DataTable objDt = null;
                // HtmlTextArea txtMulti = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                HiddenField hdnFieldType = (HiddenField)e.Row.FindControl("hdnFieldType");
                HiddenField hdnSectionId = (HiddenField)e.Row.FindControl("hdnSectionId");
                HiddenField hdnFieldId = (HiddenField)e.Row.FindControl("hdnFieldId");
                HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay"); 

                if (objDs != null && objDs.Tables.Count > 1 && objDs.Tables[1].Rows.Count > 0)
                {
                    objDv = objDs.Tables[1].DefaultView;
                    objDv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                }
                ddv = new DataView(dt); // To Be Check

                if (ddv.Count > 0)
                {
                    ddv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                    objDt = ddv.ToTable();
                    if (objDt.Rows.Count > 0)
                    {
                        e.Row.Visible = true;
                    }
                }
                else
                {
                    if (objDs.Tables.Count > 1)
                    {
                        if (objDs.Tables[1].Rows.Count > 0)
                        {
                            objDvValue = objDs.Tables[1].DefaultView;
                            if (objDvValue.Table.Columns["FieldId"] != null)
                            {
                                objDvValue.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            }
                            objDt = objDvValue.ToTable();
                            if (objDt.Rows.Count > 0)
                            {
                                e.Row.Visible = true;
                            }
                        }
                    }
                }


                if (!common.myStr(hdnFieldType.Value).Trim().Equals(string.Empty))
                {

                    #region Single TextBox Type
                    if (common.myStr(hdnFieldType.Value).Equals("T"))
                    {
                        cCtlType = "T";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        txtT.Enabled = true;
                        txtW.Visible = false;

                        string maxLength = common.myStr(txtT.MaxLength);

                        txtT.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtT.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtT.ToolTip = txtT.Text;
                            }

                        }


                    }
                    #endregion
                    #region start for Field type I
                    if (common.myStr(hdnFieldType.Value).Trim().Equals("I"))
                    {
                        cCtlType = "I";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");


                        txtT.Enabled = true;
                        txtW.Visible = false;
                        txtT.Visible = true;
                    }
                    #endregion

                    #region start for Field type IS
                    if (common.myStr(hdnFieldType.Value).Trim().Equals("IS"))
                    {
                        cCtlType = "IS";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        txtT.Enabled = false;
                        txtW.Visible = false;
                        txtT.Visible = false;
                        Hy_LinkUrl.Visible = false;
                        txtM.Visible = true;

                    }
                    #endregion
                    #region Mutiple Text Type
                    else if (common.myStr(hdnFieldType.Value).Equals("M"))
                    {
                        cCtlType = "M";
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        txtM.Visible = true;

                        txtM.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txtM.ClientID + "');");
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtM.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtM.ToolTip = txtM.Text;
                            }
                        }
                    }
                    #endregion
                    #region WordProcessor Type
                    else if (common.myStr(hdnFieldType.Value).Trim().Equals("W")) // For WordProcessor
                    {
                        cCtlType = "W";
                        DropDownList ddl = (DropDownList)e.Row.Cells[(byte)enumNonT.Values].FindControl("ddlTemplateFieldFormats");
                        ddl.Visible = true;
                        ddl.DataSource = BindFieldFormats(common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text).ToString());
                        ddl.DataTextField = "FormatName";
                        ddl.DataValueField = "FormatId";
                        ddl.DataBind();
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = true;

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtW.Content = common.myStr(objDt.Rows[0]["FieldValue"]);
                            }
                        }
                    }
                    #endregion

                    #region CheckBox Type
                    else if (common.myStr(hdnFieldType.Value).Equals("C"))
                    {
                        cCtlType = "C";
                        DataList list = (DataList)e.Row.FindControl("C");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                       
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        list.Visible = true;

                        list.DataSource = objDv;
                        list.DataBind();

                        if (hdnColumnNosToDisplay.Value != null)
                        {
                            list.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                            list.RepeatDirection = RepeatDirection.Horizontal;

                        }

                        HiddenField hdnCV = (HiddenField)list.FindControl("hdnCV");
                        foreach (DataListItem item in list.Items)
                        {
                            HtmlTextArea CT = (HtmlTextArea)item.FindControl("CT");
                            CT.Attributes.Add("onkeypress", "javascript:return AutoChange('" + CT.ClientID + "');");
                            CT.Attributes.Add("onkeydown", "javascript:return AutoChange('" + CT.ClientID + "');");
                            HiddenField hdn = (HiddenField)item.FindControl("hdnCV");
                            CheckBox chk = (CheckBox)item.FindControl("C");
                            chk.Checked = false;
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    foreach (DataRow drow in objDt.Rows)
                                    {
                                        if (common.myStr(drow["FieldValue"]).Trim().Equals(common.myStr(hdn.Value).Trim()))
                                        {
                                            chk.Checked = true;
                                        }
                                    }
                                    
                                }

                            }
                        }
                    }
                    #endregion
                    #region Boolean Type
                    else if (common.myStr(hdnFieldType.Value).Equals("B"))
                    {
                        cCtlType = "B";
                        RadioButtonList B = (RadioButtonList)e.Row.FindControl("B");

                        B.Attributes.Add("onclick", "radioMe(event,'" + B.ClientID + "');");


                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        B.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                objDvValue = objDt.DefaultView;
                                if (objDvValue.Table.Columns["FieldId"] != null)
                                {
                                    objDvValue.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                                }

                                objDt = objDvValue.ToTable();

                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(1))//Yes
                                    {
                                        B.SelectedValue = "1";
                                    }
                                    else if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(0))//No
                                    {
                                        B.SelectedValue = "0";
                                    }
                                    else
                                    {
                                        B.SelectedValue = "-1";  //Select
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region DropDown Type
                    else if (common.myStr(hdnFieldType.Value).Equals("D"))
                    {
                        cCtlType = "D";
                        DropDownList ddl = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        ddl.DataSource = objDv;
                        ddl.DataTextField = "ValueName";
                        ddl.DataValueField = "ValueId";
                        ddl.DataBind();

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])) != null)
                                {
                                    ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])).Selected = true;
                                }
                            }
                        }

                    }
                    #endregion
                    #region DropDown Image Type
                    else if (common.myStr(hdnFieldType.Value).Equals("IM"))
                    {
                        cCtlType = "IM";
                        RadComboBox ddl = (RadComboBox)e.Row.Cells[(byte)enumNonT.Values].FindControl("IM");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        //ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;


                        foreach (DataRow drImage in objDv.ToTable().Rows)
                        {
                            RadComboBoxItem item = new RadComboBoxItem();
                            item.Text = (string)drImage["ValueName"];
                            item.Value = drImage["ValueId"].ToString();
                            item.ImageUrl = drImage["ImagePath"].ToString();
                            ddl.Items.Add(item);
                            item.DataBind();
                        }
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (common.myStr(objDt.Rows[0]["FieldValue"]) != null)
                                {
                                    ddl.SelectedValue = common.myStr(objDt.Rows[0]["FieldValue"]);
                                }
                            }
                        }

                    }
                    #endregion

                    #region RadioButton Type
                    else if (common.myStr(hdnFieldType.Value).Equals("R"))
                    {
                        cCtlType = "R";
                        RadioButtonList ddl = (RadioButtonList)e.Row.Cells[(byte)enumNonT.Values].FindControl("R");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                      //  HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");

                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        if (hdnColumnNosToDisplay.Value != null)
                        {
                            ddl.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                            ddl.RepeatDirection = RepeatDirection.Horizontal;

                        }
                        ddl.DataSource = objDv;
                        ddl.DataTextField = "ValueName";
                        ddl.DataValueField = "ValueId";
                        ddl.DataBind();

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])) != null)
                                {
                                    ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])).Selected = true;
                                }
                            }
                        }

                    }
                    #endregion
                    #region Date Type
                    else if (common.myStr(hdnFieldType.Value).Equals("S"))//For Date
                    {
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        AjaxControlToolkit.CalendarExtender cal = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender3");
                        txtW.Visible = false;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtDate.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim();
                            }
                        }
                        tblDate.Visible = true;
                    }
                    #endregion
                    #region Header Type
                    else if (common.myStr(hdnFieldType.Value).Equals("H"))//For Heading
                    {
                        cCtlType = "H";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        //btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;
                    }
                    #endregion
                    #region Static Template Type
                    else if (common.myStr(hdnFieldType.Value).Equals("L"))
                    {
                        cCtlType = "L";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");

                        LinkButton lnkStaticTemplate = (LinkButton)e.Row.FindControl("lnkStaticTemplate");

                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");

                        string sStaticTemplateId = common.myStr(lnkStaticTemplate.CommandArgument);
                        lnkStaticTemplate.Visible = true;


                        //BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
                        //DataSet dsStatic = master.GetAllTypeTemplates(common.myInt(Session["HospitalLocationId"]), "S");

                        WebClient client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetAllTypeTemplates";
                        APIRootClass.GetAllTypeTemplates objRoot = new global::APIRootClass.GetAllTypeTemplates();
                        objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                        objRoot.sType = "S";

                        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                        string sValue = client.UploadString(ServiceURL, inputJson);
                        sValue = JsonConvert.DeserializeObject<string>(sValue);
                        DataSet dsStatic = JsonConvert.DeserializeObject<DataSet>(sValue);

                        DataView dvStatic = new DataView(dsStatic.Tables[0]);
                        dvStatic.RowFilter = "DisplayInTemplate=1 AND SectionId=" + sStaticTemplateId;
                        DataTable dtStatic = dvStatic.ToTable();
                        if (dtStatic.Rows.Count > 0)
                        {
                            HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                            HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                            //if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            //{
                            //    lnkStaticTemplate.Attributes.Add("onclick", "openStaticTemplateWindow('" + common.myStr(dtStatic.Rows[0]["PageUrl"]) + "',' " + sStaticTemplateId + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim()) + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.SectionId].Text).Trim()) + "')");
                                lnkStaticTemplate.Enabled = true;
                            //}
                            //else
                            //{
                            //    lnkStaticTemplate.Enabled = false;
                            //}

                            lnkStaticTemplate.Text = common.myStr(dtStatic.Rows[0]["SectionName"]);
                        }
                        // btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        // btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;



                    }
                    #endregion
                    #region Patient Data Object
                    else if (common.myStr(hdnFieldType.Value).Equals("O"))//Patient Data Object
                    {
                        cCtlType = "O";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        // btnAdd.Visible = false;
                        txtT.Visible = false;
                        txtM.Visible = false;
                        // btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;

                        int DataObjectId = common.myInt(e.Row.Cells[(byte)enumNonT.DataObjectId].Text);

                        //clsIVF objivf = new clsIVF(sConString);
                        clsIVF objivf = new clsIVF(string.Empty);

                        string strOutput = objivf.getDataObjectValue(DataObjectId);

                        if (strOutput.Length > 50)
                        {
                            txtM.Visible = true;
                            txtM.Text = strOutput;
                            txtM.ToolTip = strOutput;
                            txtM.Enabled = false;
                        }
                        else
                        {
                            txtT.Visible = true;
                            txtT.Text = strOutput;
                            txtT.ToolTip = strOutput;
                            txtT.Enabled = false;
                            txtT.Columns = strOutput.Length + 2;

                        }

                    }
                    #endregion
                }
                else
                {
                    e.Row.Cells[(byte)enumNonT.Values].Text = "No Record Found!";
                }
                if (cCtlType.Equals("T") || cCtlType.Equals("M"))
                {
                    RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                    txtW.Visible = false;
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
            ddv.Dispose();
        }
    }
   
    protected void ddlTemplateFieldFormats_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.GetFormatText objRoot = new global::APIRootClass.GetFormatText();
        try
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            RadEditor txtW = (RadEditor)row.FindControl("txtW");
            
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetFormatText";
            
            objRoot.FormatId = common.myInt(ddl.SelectedValue);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtW.Content = common.myStr(ds.Tables[0].Rows[0]["FormatText"]);
            }
            else
            {
                txtW.Content = String.Empty;
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
            ds.Dispose();
            client = null;
            objRoot =null;
        }
    }
    public DataTable BindFieldFormats(String strFieldId)
    {
        DataTable dt = new DataTable();
        WebClient client = new WebClient();
        DataSet ds = new DataSet();
        APIRootClass.GetTemplateFieldFormats objRoot = new global::APIRootClass.GetTemplateFieldFormats();
        try
        {
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetTemplateFieldFormats";
            
            objRoot.FieldId = common.myInt(strFieldId);
            objRoot.SpecialisationId = common.myInt(ViewState["DoctorSpecialisationId"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr;
                dr = dt.NewRow();
                dr["FormatId"] = 0;
                dr["FormatName"] = "--Options--";
                dt.Rows.InsertAt(dr, 0);
            }
            client = null;
            ds.Dispose();
             objRoot = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dt;
    }
  


    protected void btnCopy_Click(object sender, EventArgs e)
    {
        StringBuilder strNonTabular = new StringBuilder();
        ArrayList collNonTabular = new ArrayList();

        StringBuilder strSpeciality = new StringBuilder();
        ArrayList collSpeciality = new ArrayList();

        StringBuilder strService = new StringBuilder();
        ArrayList collService = new ArrayList();

        StringBuilder strDrugClass = new StringBuilder();
        ArrayList collDrugClass = new ArrayList();


        StringBuilder strPrescription = new StringBuilder();
        ArrayList collPrescription = new ArrayList();


        StringBuilder strTemplate = new StringBuilder();
        ArrayList colltemplate = new ArrayList();

        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        WebClient client = new WebClient();
        try
        {
            if (common.myInt(hdnSelectedDayDetailId.Value) == 0)
            {
                Alert.ShowAjaxMsg("please select day for copy", Page);
                return;
            }
            if (common.myInt(hdnCopyDayDetailId.Value) == 0)
            {
               Alert.ShowAjaxMsg("please select day to copy", Page);
                return;
            }


            foreach (GridViewRow item in gvSpecialsation.Rows)
            {
                CheckBox chkSpecialisation = (CheckBox)item.FindControl("chkSpecialisation");
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                HiddenField hdnPlanId = (HiddenField)item.FindControl("hdnPlanId");
                HiddenField hdnDayId = (HiddenField)item.FindControl("hdnDayId");
                HiddenField hdnDayDetailId = (HiddenField)item.FindControl("hdnDayDetailId");
                HiddenField hdnSpecialisationId = (HiddenField)item.FindControl("hdnSpecialisationId");
                
                if (chkSpecialisation.Checked)
                {
                    collSpeciality.Add(common.myInt(hdnId.Value));
                    collSpeciality.Add(common.myInt(hdnPlanId.Value));
                    collSpeciality.Add(common.myInt(hdnDayId.Value));
                    collSpeciality.Add(common.myInt(hdnDayDetailId.Value));
                    collSpeciality.Add(common.myInt(hdnSpecialisationId.Value));
                    strSpeciality.Append(common.setXmlTable(ref collSpeciality));
                }
            }

            foreach (GridViewRow item in gvService.Rows)
            {
                CheckBox chkService = (CheckBox)item.FindControl("chkService");
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                HiddenField hdnPlanId = (HiddenField)item.FindControl("hdnPlanId");
                HiddenField hdnDayId = (HiddenField)item.FindControl("hdnDayId");
                HiddenField hdnDayDetailId = (HiddenField)item.FindControl("hdnDayDetailId");
                HiddenField hdnServiceId = (HiddenField)item.FindControl("hdnServiceId");
                if (chkService.Checked)
                {
                    collService.Add(common.myInt(hdnId.Value));
                    collService.Add(common.myInt(hdnPlanId.Value));
                    collService.Add(common.myInt(hdnDayId.Value));
                    collService.Add(common.myInt(hdnDayDetailId.Value));
                    collService.Add(common.myInt(hdnServiceId.Value));
                    strService.Append(common.setXmlTable(ref collService));
                }
            }
            foreach (GridViewRow item in gvDrugClass.Rows)
            {
                CheckBox chkDrugClass = (CheckBox)item.FindControl("chkDrugClass");
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                HiddenField hdnPlanId = (HiddenField)item.FindControl("hdnPlanId");
                HiddenField hdnDayId = (HiddenField)item.FindControl("hdnDayId");
                HiddenField hdnDayDetailId = (HiddenField)item.FindControl("hdnDayDetailId");
                HiddenField hdnDrugClassId = (HiddenField)item.FindControl("hdnDrugClassId");
                
                if (chkDrugClass.Checked)
                {
                    collDrugClass.Add(common.myInt(hdnId.Value));
                    collDrugClass.Add(common.myInt(hdnPlanId.Value));
                    collDrugClass.Add(common.myInt(hdnDayId.Value));
                    collDrugClass.Add(common.myInt(hdnDayDetailId.Value));
                    collDrugClass.Add(common.myInt(hdnDrugClassId.Value));
                    strDrugClass.Append(common.setXmlTable(ref collDrugClass));
                }
            }

            foreach (GridViewRow item in gvPrescription.Rows)
            {
                CheckBox chkPrescription = (CheckBox)item.FindControl("chkPrescription");
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                HiddenField hdnPlanId = (HiddenField)item.FindControl("hdnPlanId");
                HiddenField hdnDayId = (HiddenField)item.FindControl("hdnDayId");
                HiddenField hdnDayDetailId = (HiddenField)item.FindControl("hdnDayDetailId");
                HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId"); 
                if (chkPrescription.Checked)
                {
                    collPrescription.Add(common.myInt(hdnId.Value));
                    collPrescription.Add(common.myInt(hdnPlanId.Value));
                    collPrescription.Add(common.myInt(hdnDayId.Value));
                    collPrescription.Add(common.myInt(hdnDayDetailId.Value));
                    collPrescription.Add(common.myInt(hdnItemId.Value));
                    strPrescription.Append(common.setXmlTable(ref collPrescription));
                }
            }
            foreach (GridViewRow item in gvTemplateLis.Rows)
            {
                CheckBox chkTemplate = (CheckBox)item.FindControl("chkTemplate");
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");
                HiddenField hdnPlanId = (HiddenField)item.FindControl("hdnPlanId");
                HiddenField hdnDayId = (HiddenField)item.FindControl("hdnDayId");
                HiddenField hdnDayDetailId = (HiddenField)item.FindControl("hdnDayDetailId");
                HiddenField hdnTemplateId = (HiddenField)item.FindControl("hdnTemplateId"); 
                if (chkTemplate.Checked)
                {
                    colltemplate.Add(common.myInt(hdnId.Value));
                    colltemplate.Add(common.myInt(hdnPlanId.Value));
                    colltemplate.Add(common.myInt(hdnDayId.Value));
                    colltemplate.Add(common.myInt(hdnDayDetailId.Value));
                    colltemplate.Add(common.myInt(hdnTemplateId.Value));
                    strTemplate.Append(common.setXmlTable(ref colltemplate));
                }
            }

        
            foreach (GridViewRow item2 in gvSelectedServices.Rows)
            {
                if (item2.RowType.Equals(DataControlRowType.DataRow))
                {
                    HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
                    HiddenField hdnFieldId = (HiddenField)item2.FindControl("hdnFieldId");
                    if (common.myStr(hdnFieldType.Value).Equals("T"))
                    {
                        TextBox txtT = (TextBox)item2.FindControl("txtT");
                        collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                        
                        collNonTabular.Add(common.myInt(hdnFieldId.Value));
                        collNonTabular.Add(txtT.Text);
                        strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                    }
                    if (common.myStr(hdnFieldType.Value).Equals("I"))
                    {
                        TextBox txtT = (TextBox)item2.FindControl("txtT");
                        collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                        collNonTabular.Add(common.myInt(hdnFieldId.Value));
                        collNonTabular.Add(txtT.Text);
                        strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                    }
                    if (common.myStr(hdnFieldType.Value).Equals("IS"))
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                        collNonTabular.Add(common.myInt(hdnFieldId.Value));
                        collNonTabular.Add(txtM.Text);
                        strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("M"))
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                        collNonTabular.Add(common.myInt(hdnFieldId.Value));
                        collNonTabular.Add(txtM.Text);
                        strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("W")) // For Word Processor
                    {
                        RadEditor txtW = (RadEditor)item2.FindControl("txtW");
                        collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                        collNonTabular.Add(common.myInt(hdnFieldId.Value));
                        collNonTabular.Add(txtW.Content);
                        strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("D"))
                    {
                        DropDownList ddl = (DropDownList)item2.FindControl("D");

                        collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                        collNonTabular.Add(common.myInt(hdnFieldId.Value));
                        collNonTabular.Add(ddl.SelectedValue);
                        strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("IM"))
                    {
                        RadComboBox ddl = (RadComboBox)item2.FindControl("IM");

                        collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));
                        collNonTabular.Add(common.myInt(hdnFieldId.Value));
                        collNonTabular.Add(ddl.SelectedValue);
                        strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("R"))
                    {
                        RadioButtonList ddl = (RadioButtonList)item2.FindControl("R");
                        collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                        collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                        collNonTabular.Add(common.myInt(hdnFieldId.Value));
                        collNonTabular.Add(ddl.SelectedValue);
                        strNonTabular.Append(common.setXmlTable(ref collNonTabular));

                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("B"))
                    {
                        RadioButtonList ddlB = (RadioButtonList)item2.FindControl("B");
                        if (ddlB.SelectedItem != null)
                        {
                            if (!ddlB.SelectedItem.Text.Equals("Select"))
                            {
                                collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                                collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                                collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                                collNonTabular.Add(common.myInt(hdnFieldId.Value));
                                collNonTabular.Add((common.myInt(ddlB.SelectedValue).Equals(1)) ? 1 : 0);
                                strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                            }
                            else
                            {
                                collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                                collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                                collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                                collNonTabular.Add(common.myInt(hdnFieldId.Value));
                                collNonTabular.Add(null);
                                strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                            }
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("C"))
                    {
                        DataList rptC = (DataList)item2.FindControl("C");
                        string sCheckedValues = string.Empty;
                        foreach (DataListItem rptItem in rptC.Items)
                        {
                            CheckBox chk = (CheckBox)rptItem.FindControl("C");
                            HiddenField hdn = (HiddenField)rptItem.FindControl("hdnCV");
                            sCheckedValues = chk.Checked == true ? hdn.Value : "0";
                            collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                            collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                            collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                            collNonTabular.Add(common.myInt(hdnFieldId.Value));
                            collNonTabular.Add(sCheckedValues);
                            strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                        }
                        sCheckedValues = string.Empty;
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("S"))
                    {
                        RadDatePicker txtDate = (RadDatePicker)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");
                        if (txtDate.SelectedDate != null)
                        {

                            collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                            collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                            collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                            collNonTabular.Add(common.myInt(hdnFieldId.Value));
                            collNonTabular.Add(common.myDate(txtDate.SelectedDate).ToString("dd/MM/yyyyy"));// + " " + common.myStr(time));
                            strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("ST"))
                    {
                        RadDatePicker txtDate = (RadDatePicker)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");
                        if (tpTime.SelectedDate != null)
                        {
                            collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                            collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                            collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                            collNonTabular.Add(common.myInt(hdnFieldId.Value));
                            DateTime d = tpTime.SelectedDate.Value;
                            string time = d.ToString("HH:mm:ss");
                            collNonTabular.Add(common.myStr("01/01/1900").Trim() + " " + common.myStr(time));
                            strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("SB"))
                    {
                        RadDatePicker txtDate = (RadDatePicker)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");
                        if (txtDate.SelectedDate != null)
                        {
                            collNonTabular.Add(common.myInt(hdnSelectedPlanId.Value));
                            collNonTabular.Add(common.myInt(hdnSelectedDayId.Value));
                            collNonTabular.Add(common.myInt(hdnSelectedDayDetailId.Value));

                            collNonTabular.Add(common.myInt(hdnFieldId.Value));
                            DateTime d = tpTime.SelectedDate.Value;
                            string time = d.ToString("HH:mm:ss");
                            collNonTabular.Add(common.myDate(txtDate.SelectedDate).ToString("dd/MM/yyyy") + " " + common.myStr(time));
                            strNonTabular.Append(common.setXmlTable(ref collNonTabular));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("O"))
                    {
                        
                    }
                }
            }
          

            
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRCopyTreatmentPlanDetails";
            

            objRoot.PlanId = common.myInt(hdnSelectedPlanId.Value);
            objRoot.DayId = common.myInt(hdnCopyDayId.Value);
            objRoot.DayDetailId = common.myInt(hdnCopyDayDetailId.Value);

            objRoot.SelectedDayId = common.myInt(hdnSelectedDayId.Value);
            objRoot.SelectedDayDetailId= common.myInt(hdnSelectedDayDetailId.Value);

            objRoot.xmlSpecialtyDetails = strSpeciality.ToString();
            objRoot.xmlServiceDetails = strService.ToString();

            objRoot.xmlDrugClassDetails = strDrugClass.ToString();
            objRoot.xmlPrescriptionDetails = strPrescription.ToString();

            objRoot.xmlTemplateDetails = strTemplate.ToString();
            objRoot.xmlTemplateFieldDetails = strNonTabular.ToString();

            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);


        }
        catch(Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            strNonTabular = null;
            collNonTabular = null;
            strSpeciality = null;
            collSpeciality = null;
            strService = null;
            collService = null;
            strDrugClass = null;
            collDrugClass = null;
            strPrescription = null;
            collPrescription = null;
            strTemplate = null;
            colltemplate = null;
            objRoot = null;
            client = null;
        }
    }

    protected void gvSpecialsation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            CheckBox chkSpecialisation = (CheckBox)e.Row.FindControl("chkSpecialisation");
            if (common.myInt(hdnId.Value) > 0)
            {
                chkSpecialisation.Checked = true;
            }
        }
    }

    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            CheckBox chkService = (CheckBox)e.Row.FindControl("chkService");
            if (common.myInt(hdnId.Value) > 0)
            {
                chkService.Checked = true;
            }
        }
    }

    protected void gvDrugClass_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            CheckBox chkDrugClass = (CheckBox)e.Row.FindControl("chkDrugClass");
            if (common.myInt(hdnId.Value) > 0)
            {
                chkDrugClass.Checked = true;
            }
        }
    }

    protected void gvPrescription_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            CheckBox chkPrescription = (CheckBox)e.Row.FindControl("chkPrescription");
            if (common.myInt(hdnId.Value) > 0)
            {
                chkPrescription.Checked = true;
            }
        }
    }

    protected void gvTemplateLis_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            CheckBox chkTemplate = (CheckBox)e.Row.FindControl("chkTemplate");
            if (common.myInt(hdnId.Value) > 0)
            {
                chkTemplate.Checked = true;
            }
        }
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {
        hdnSelectedPlanId.Value = "";
        hdnSelectedDayDetailId.Value = "";
        hdnSelectedDayId.Value = "";

        hdnSelectedPlanId.Value = common.myStr(Request.QueryString["PlanId"]); 
        hdnSelectedDayDetailId.Value = common.myStr(Request.QueryString["SelectedDayDetailId"]);
        hdnSelectedDayId.Value = common.myStr(Request.QueryString["SelectedDayId"]);

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }
}