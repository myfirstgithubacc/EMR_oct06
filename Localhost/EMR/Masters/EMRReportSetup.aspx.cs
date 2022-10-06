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
using Telerik.Web.UI;
using System.Text;

public partial class EMR_Masters_EMRReportSetup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsIVF objIVF;
    StringBuilder strXML;
    ArrayList coll;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds;
    private bool RowSelStatus = false;
    DAL.DAL dl;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["PT"]) == "M")
        {
            MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       // string x = rdoTemplate.SelectedValue;
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            objIVF = new clsIVF(sConString);

            bindControl();

            hdnMainReportId.Value = "0";

            radgvFields.Visible = false;
            rdoTemplate.SelectedValue = "D";
            rdoTemplate_SelectedIndexChanged(this, null);
            bindSection(true);

            bindDetailData();
        }
    }

    private void bindControl()
    {
        try
        {
            DataSet ds;
            objIVF = new clsIVF(sConString);

            ddlReport.Items.Clear();

            ds = objIVF.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), "", 0, 0, "B", 0, "", common.myInt(Session["FacilityID"]), false);

            ds = objIVF.GetReportName();
///////////////////////////////////////////////////////////////////////////////
            RadComboBoxItem item;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                item = new RadComboBoxItem();

                item.Text = common.myStr(dr["ReportName"]);
                item.Value = common.myInt(dr["ReportId"]).ToString();
                item.Attributes.Add("IsCheckListRequired", common.myStr(dr["IsCheckListRequired"]));

                ddlReport.Items.Add(item);
                ddlReport.DataBind();
            }
            ddlReport.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlReport.SelectedIndex = 0;
////////////////////////////////////////////////////////////////////////////////

            //ddlReport.DataSource = ds;
            //ddlReport.DataTextField = "ReportName";
            //ddlReport.DataValueField = "ReportId";
            //ddlReport.DataBind();

            //ddlReport.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlReport.SelectedIndex = 0;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlTemplate_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        hdnMainReportId.Value = "0";

        bindSection(true);

    }

    protected void ddlSections_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        hdnMainReportId.Value = "0";

        bindFields(true);

    }

    private void bindSection(bool AllowDataBound)
    {
        try
        {

            objIVF = new clsIVF(sConString);

            DataSet ds = objIVF.getEMRReportSetupSection(Convert.ToInt16(common.myInt(Session["HospitalLocationId"])), common.myInt(ddlTemplatename.SelectedValue), common.myInt(hdnMainReportId.Value));

            if (rdoTemplate.SelectedValue == "D")
            {
                if (common.myInt(ddlTemplatename.SelectedItem.Value) == 0)
                {
                    RadgvPages.DataSource = ds.Tables[0].Clone();
                }
                else
                {
                    RadgvPages.DataSource = ds.Tables[0];
                }
                RadgvPages.DataBind();
            }
            else if (rdoTemplate.SelectedValue == "F")
            {
                ddlSections.Text = "";
                ddlSections.Items.Clear();
                ddlSections.Enabled = true;

                ddlSections.DataSource = ds.Tables[0];
                ddlSections.DataTextField = "SectionName";
                ddlSections.DataValueField = "SectionId";
                ddlSections.DataBind();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindFields(bool AllowDataBound)
    {
        try
        {

            objIVF = new clsIVF(sConString);

            DataSet ds = objIVF.getEMRReportSetupTemplateFields(common.myInt(ddlSections.SelectedValue));

            if (common.myInt(ddlTemplatename.SelectedItem.Value) == 0)
            {
                radgvFields.DataSource = ds.Tables[0].Clone();
            }
            else
            {
                radgvFields.DataSource = ds.Tables[0];
            }


            radgvFields.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindDetailData()
    {
        try
        {
            objIVF = new clsIVF(sConString);

            DataSet ds = objIVF.getEMRReportSetupSection(common.myInt(ddlReport.SelectedValue));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTagged.DataSource = ds.Tables[0];
                gvTagged.DataBind();
                ViewState["objDt"] = ds.Tables[0];
            }
            else
            {
                DataTable objDt = CreateTable();
                gvTagged.DataSource = objDt;
                gvTagged.DataBind();
                ViewState["objDt"] = objDt;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(ddlReport.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select report name !";
                return;
            }


            strXML = new StringBuilder();
            coll = new ArrayList();

            int sno = 0;
            foreach (GridDataItem dataItem in gvTagged.Items)
            {
                HiddenField hdnSectionId = (HiddenField)dataItem.FindControl("hdnSectionId");
                HiddenField hdnSeqNo = (HiddenField)dataItem.FindControl("hdnSeqNo");
                HiddenField HdnType = (HiddenField)dataItem.FindControl("HdnType");
                HiddenField hdnFieldId = (HiddenField)dataItem.FindControl("hdnFieldId");
                //Label lblHeadingName = (Label)dataItem.FindControl("lblHeadingName");
                LinkButton lblHeadingName = (LinkButton)dataItem.FindControl("lblHeadingName");
                Label lblDetailId = (Label)dataItem.FindControl("lblDetailId");

                HiddenField hdnShowTemplateNameInNote = (HiddenField)dataItem.FindControl("hdnShowTemplateNameInNote");
                HiddenField hdnShowSectionNameInNote = (HiddenField)dataItem.FindControl("hdnShowSectionNameInNote");
                HiddenField hdnShowFieldNameInNote = (HiddenField)dataItem.FindControl("hdnShowFieldNameInNote");

                HiddenField hdnAllSectionManditory = (HiddenField)dataItem.FindControl("hdnAllSectionManditory");

                coll.Add(HdnType.Value);//Type char(1),
                coll.Add(common.myInt(hdnSectionId.Value)); //SectionId int,
                coll.Add(++sno);//SequenceNo smallint,
                coll.Add(lblHeadingName.Text.ToString());//HeadingName varchar(50),
                coll.Add(common.myInt(hdnFieldId.Value));//FieldId int,
                coll.Add(common.myBool(hdnShowTemplateNameInNote.Value));//ShowTemplateNameInNote bit,
                coll.Add(common.myBool(hdnShowSectionNameInNote.Value));//ShowSectionNameInNote bit,
                coll.Add(common.myBool(hdnShowFieldNameInNote.Value));//ShowFieldNameInNote bit,
                coll.Add(common.myInt(lblDetailId.Text.ToString()));//DetailId int
                coll.Add(common.myBool(hdnAllSectionManditory.Value ));//   bit


                strXML.Append(common.setXmlTable(ref coll));

            }

            if (strXML.ToString() == "")
            {
                //lblMessage.Text = "Section not selected !";
                //return;
            }

            objIVF = new clsIVF(sConString);
            string strMsg = objIVF.SaveEMRTemplateReportSetup(common.myInt(ddlReport.SelectedValue), 1, strXML.ToString(),
                                            common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                hdnMainReportId.Value = "0";

                clearControl();

                bindSection(true);
                bindDetailData();

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }

            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Masters/ReportFormat.aspx";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1100;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        //  RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;

    }

    protected void RadgvPages_OnItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType != Telerik.Web.UI.GridItemType.Pager)
        {
            //e.Item.Cells[0].Visible = false;
            //e.Item.Cells[1].Visible = false;
            //e.Item.Cells[2].Visible = false;
        }
    }

    protected void RadgvPages_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddToList")
            {
                lblMessage.Text = "";
                RowSelStatus = true;
                RadgvPages.Items[e.Item.ItemIndex].Selected = true;
                int iSeqCount = 0;

                Label lblSectionName = (Label)e.Item.FindControl("lblSectionName");
                HiddenField hdnTemplateName = (HiddenField)e.Item.FindControl("hdnTemplateName");
                HiddenField hdnTemplateTypeName = (HiddenField)e.Item.FindControl("hdnTemplateTypeName");
                HiddenField hdnSectionId = (HiddenField)e.Item.FindControl("hdnSectionId");
                HiddenField HdnType = (HiddenField)e.Item.FindControl("HdnType");

                DataTable objDt = CreateTable();
                DataRow objDr;
                if (ViewState["objDt"] != null)
                {
                    objDt = (DataTable)ViewState["objDt"];
                }
                bool isAlredyExists = false;


                if (objDt.Rows.Count > 0)
                {
                    DataRow[] drexist = objDt.Select("SectionId='" + hdnSectionId.Value + "' AND Type='" + HdnType.Value + "'");
                    if (drexist.Length > 0)
                    {
                        isAlredyExists = true;
                    }
                    else
                    {
                        objDr = objDt.NewRow();

                        objDr["DetailId"] = 0;
                        objDr["SectionId"] = hdnSectionId.Value;

                        if (rdoTemplate.SelectedValue == "S")
                        {
                            objDr["SectionName"] = "";
                        }
                        else
                        {
                            objDr["SectionName"] = lblSectionName.Text;
                        }
                        objDr["FieldId"] = 0;
                        objDr["Type"] = HdnType.Value;
                        objDr["TemplateName"] = hdnTemplateName.Value;
                        objDr["TemplateTypeName"] = hdnTemplateTypeName.Value;
                        objDr["SequenceNo"] = common.myStr(iSeqCount + 1);

                        objDr["HeadingName"] = txtHeadingName.Text;
                        objDr["ShowTemplateNameInNote"] = common.myBool(chkShowTemplateName.Checked);
                        objDr["ShowSectionNameInNote"] = common.myBool(chkShowSectionName.Checked);
                        objDr["ShowFieldNameInNote"] = common.myBool(chkShowFieldName.Checked);                       
                        objDr["AllSectionManditory"] = common.myBool(chkAllSectionManditory.Checked);
                        objDt.Rows.Add(objDr);
                        isAlredyExists = false;
                    }
                }
                else
                {
                    objDr = objDt.NewRow();

                    objDr["DetailId"] = 0;
                    objDr["SectionId"] = hdnSectionId.Value;
                    if (rdoTemplate.SelectedValue == "S")
                    {
                        objDr["SectionName"] = "";
                    }
                    else
                    {
                        objDr["SectionName"] = lblSectionName.Text;
                    }
                    objDr["FieldId"] = 0;
                    objDr["Type"] = HdnType.Value;
                    objDr["TemplateName"] = hdnTemplateName.Value;
                    objDr["TemplateTypeName"] = hdnTemplateTypeName.Value;
                    objDr["SequenceNo"] = common.myStr(iSeqCount + 1);

                    objDr["HeadingName"] = txtHeadingName.Text;
                    objDr["ShowTemplateNameInNote"] = common.myBool(chkShowTemplateName.Checked);
                    objDr["ShowSectionNameInNote"] = common.myBool(chkShowSectionName.Checked);
                    objDr["ShowFieldNameInNote"] = common.myBool(chkShowFieldName.Checked);
                    objDr["AllSectionManditory"] = common.myBool(chkAllSectionManditory.Checked);

                    objDt.Rows.Add(objDr);
                    isAlredyExists = false;
                }

                if (!isAlredyExists)
                {
                    int i = 1;
                    foreach (DataRow dr in objDt.Rows)
                    {
                        dr["ID"] = i++;
                    }
                    gvTagged.DataSource = objDt;
                    gvTagged.DataBind();
                    ViewState["objDt"] = objDt;
                }

                if (isAlredyExists)
                {
                    lblMessage.Text = "Section Name '" + lblSectionName.Text + "' Already Exists !";
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void radgvFields_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddToList")
            {
                lblMessage.Text = "";
                RowSelStatus = true;
                radgvFields.Items[e.Item.ItemIndex].Selected = true;
                int iSeqCount = 0;

                Label lblFieldName = (Label)e.Item.FindControl("lblFieldName");
                HiddenField hdnTemplateName = (HiddenField)e.Item.FindControl("hdnTemplateName");
                HiddenField hdnTemplateTypeName = (HiddenField)e.Item.FindControl("hdnTemplateTypeName");
                HiddenField hdnSectionId = (HiddenField)e.Item.FindControl("hdnSectionId");
                HiddenField hdnSectionName = (HiddenField)e.Item.FindControl("hdnSectionName");
                HiddenField HdnType = (HiddenField)e.Item.FindControl("HdnType");
                HiddenField hdnFieldId = (HiddenField)e.Item.FindControl("hdnFieldId");

                DataTable objDt = CreateTable();
                DataRow objDr;
                if (ViewState["objDt"] != null)
                {
                    objDt = (DataTable)ViewState["objDt"];
                }
                bool isAlredyExists = false;


                if (objDt.Rows.Count > 0)
                {
                    DataRow[] drexist = objDt.Select("SectionId='" + hdnSectionId.Value + "' AND FieldId='" + hdnFieldId.Value + "' AND Type='" + HdnType.Value + "'");
                    if (drexist.Length > 0)
                    {
                        isAlredyExists = true;
                    }
                    else
                    {
                        objDr = objDt.NewRow();
                        objDr["DetailId"] = 0;
                        objDr["SectionId"] = hdnSectionId.Value;
                        objDr["SectionName"] = hdnSectionName.Value;
                        objDr["Type"] = HdnType.Value;
                        objDr["TemplateName"] = hdnTemplateName.Value;
                        objDr["TemplateTypeName"] = hdnTemplateTypeName.Value;
                        objDr["SequenceNo"] = common.myStr(iSeqCount + 1);

                        objDr["FieldId"] = hdnFieldId.Value;
                        objDr["FieldName"] = lblFieldName.Text;
                        objDr["HeadingName"] = txtHeadingName.Text;
                        objDr["ShowTemplateNameInNote"] = common.myBool(chkShowTemplateName.Checked);
                        objDr["ShowSectionNameInNote"] = common.myBool(chkShowSectionName.Checked);
                        objDr["ShowFieldNameInNote"] = common.myBool(chkShowFieldName.Checked);
                        objDr["AllSectionManditory"] = common.myBool(chkAllSectionManditory.Checked);

                        objDt.Rows.Add(objDr);
                        isAlredyExists = false;
                    }
                }
                else
                {
                    objDr = objDt.NewRow();
                    objDr["DetailId"] = 0;
                    objDr["SectionId"] = hdnSectionId.Value;
                    objDr["SectionName"] = hdnSectionName.Value;
                    objDr["Type"] = HdnType.Value;
                    objDr["TemplateName"] = hdnTemplateName.Value;
                    objDr["TemplateTypeName"] = hdnTemplateTypeName.Value;
                    objDr["SequenceNo"] = common.myStr(iSeqCount + 1);

                    objDr["FieldId"] = hdnFieldId.Value;
                    objDr["FieldName"] = lblFieldName.Text;
                    objDr["HeadingName"] = txtHeadingName.Text;

                    objDr["ShowTemplateNameInNote"] = common.myBool(chkShowTemplateName.Checked);
                    objDr["ShowSectionNameInNote"] = common.myBool(chkShowSectionName.Checked);
                    objDr["ShowFieldNameInNote"] = common.myBool(chkShowFieldName.Checked);
                    objDr["AllSectionManditory"] = common.myBool(chkAllSectionManditory.Checked);


                    objDt.Rows.Add(objDr);
                    isAlredyExists = false;
                }

                if (!isAlredyExists)
                {
                    int i = 1;
                    foreach (DataRow dr in objDt.Rows)
                    {
                        dr["ID"] = i++;
                    }
                    gvTagged.DataSource = objDt;
                    gvTagged.DataBind();
                    ViewState["objDt"] = objDt;
                }

                if (isAlredyExists)
                {
                    lblMessage.Text = "Field Name '" + lblFieldName.Text + "' Already Exists !";
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnEdit_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkId = sender as LinkButton;
            string sDetailId = lnkId.CommandArgument;
            //string sRptHeader = ((Label)lnkId.FindControl("lblHeadingName")).Text.ToString();
            string sRptHeader = ((LinkButton)lnkId.FindControl("lblHeadingName")).Text.ToString();
            ViewState["DeatilId"] = sDetailId;
            txtHeadingName.Text = sRptHeader.Trim();
            btnAddHeading.Text = "Update";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    protected void gvTagged_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            int selectedIdx = e.Item.ItemIndex;
            lblMessage.Text = "";

            if (e.CommandName == "Delete1")
            {
                DataTable Dt = CreateTable();

                for (int i = 0; i != gvTagged.Items.Count; i++)
                {
                    string ID = "";
                    string SectionId = "";
                    string SectionName = "";
                    int SequenceNo = 0;
                    int DetailId = 0;
                    string HeadingName = "";
                    string TemplateName = "";
                    string Type = "";
                    bool ShowTemplateNameInNote = false;
                    bool ShowSectionNameInNote = false;
                    bool ShowFieldNameInNote = false;
                    string FieldName = "";
                    int FieldId = 0;
                    string TemplateTypeName = "";
                    bool AllSectionManditory = false;


                    SectionName = ((Label)gvTagged.Items[i].FindControl("lblSectionName")).Text;
                    ID = ((Label)gvTagged.Items[i].FindControl("lblSno")).Text;
                    SectionId = ((HiddenField)gvTagged.Items[i].FindControl("hdnSectionId")).Value;
                    SequenceNo = common.myInt(((HiddenField)gvTagged.Items[i].FindControl("hdnSeqNo")).Value);

                    DetailId = common.myInt(((Label)gvTagged.Items[i].FindControl("lblDetailId")).Text);
                    //HeadingName = ((Label)gvTagged.Items[i].FindControl("lblHeadingName")).Text;
                    HeadingName = ((LinkButton)gvTagged.Items[i].FindControl("lblHeadingName")).Text;
                    TemplateName = ((Label)gvTagged.Items[i].FindControl("lblTemplateName")).Text;
                    Type = ((HiddenField)gvTagged.Items[i].FindControl("HdnType")).Value;
                    ShowTemplateNameInNote = common.myBool(((HiddenField)gvTagged.Items[i].FindControl("hdnShowTemplateNameInNote")).Value);
                    ShowSectionNameInNote = common.myBool(((HiddenField)gvTagged.Items[i].FindControl("hdnShowSectionNameInNote")).Value);
                    ShowFieldNameInNote = common.myBool(((HiddenField)gvTagged.Items[i].FindControl("hdnShowFieldNameInNote")).Value);
                    FieldName = ((Label)gvTagged.Items[i].FindControl("lblFieldName")).Text;
                    FieldId = common.myInt(((HiddenField)gvTagged.Items[i].FindControl("hdnFieldId")).Value);
                    TemplateTypeName = ((Label)gvTagged.Items[i].FindControl("lblTemplateTypeName")).Text;
                    AllSectionManditory = common.myBool(((HiddenField)gvTagged.Items[i].FindControl("hdnAllSectionManditory")).Value);





                    if (common.myStr(ID).Trim() != common.myStr(e.CommandArgument).Trim())
                    {
                        DataRow drDt = Dt.NewRow();
                        drDt["SectionId"] = common.myStr(SectionId).Trim();
                        drDt["ID"] = common.myStr(ID).Trim();
                        drDt["SectionName"] = common.myStr(SectionName).Trim();
                        drDt["SequenceNo"] = SequenceNo;

                        drDt["DetailId"] = common.myInt(DetailId);

                        drDt["HeadingName"] = common.myStr(HeadingName).Trim();
                        drDt["TemplateName"] = common.myStr(TemplateName).Trim();
                        drDt["Type"] = common.myStr(Type).Trim();
                        drDt["ShowTemplateNameInNote"] = ShowTemplateNameInNote;
                        drDt["ShowSectionNameInNote"] = ShowSectionNameInNote;
                        drDt["ShowFieldNameInNote"] = ShowFieldNameInNote;
                        drDt["FieldName"] = common.myStr(FieldName).Trim();
                        drDt["FieldId"] = common.myInt(FieldId);
                        drDt["TemplateTypeName"] = common.myStr(TemplateTypeName).Trim();
                        drDt["AllSectionManditory"] = AllSectionManditory;

                        Dt.Rows.Add(drDt);




                        //dr["ID"] = "0";
                        //dr["SectionId"] = "";
                        //dr["SectionName"] = "";
                        //dr["SequenceNo"] = 0;
                        //dr["DetailId"] = 0;

                        //dr["HeadingName"] = "";
                        //dr["TemplateName"] = "";
                        //dr["Type"] = "";
                        //dr["ShowTemplateNameInNote"] = 0;
                        //dr["ShowSectionNameInNote"] = 0;
                        //dr["ShowFieldNameInNote"] = 0;
                        //dr["FieldName"] = "";
                        //dr["FieldId"] = "";
                        //dr["TemplateTypeName"] = "";
                    }
                }


                if (Dt.Rows.Count > 0)
                {
                    gvTagged.DataSource = Dt;
                    gvTagged.DataBind();
                    ViewState["objDt"] = Dt;

                }
                else
                {
                    // BindBlankSelectedValue();
                    DataTable objDt = CreateTable();
                    gvTagged.DataSource = objDt;
                    gvTagged.DataBind();
                    ViewState["objDt"] = objDt;
                }
            }

            else if (e.CommandName == "MoveUP")
            {
                gvTagged.Items[selectedIdx].Selected = true;

                DataTable objDt = (DataTable)ViewState["objDt"];
                DataRow[] dr = null;
                DataRow[] dr1 = null;
                int SelectedRow = e.Item.ItemIndex;
                int prevRow = SelectedRow - 1;
                if (prevRow >= 0)
                {
                    string strSelectedType = ((HiddenField)gvTagged.Items[SelectedRow].FindControl("HdnType")).Value;
                    string strPrevType = ((HiddenField)gvTagged.Items[prevRow].FindControl("HdnType")).Value;

                    //Label lblSelectedHeadingName = (Label)gvTagged.Items[SelectedRow].FindControl("lblHeadingName");
                    //Label lblPrevHeadingName = (Label)gvTagged.Items[prevRow].FindControl("lblHeadingName");

                    LinkButton lblSelectedHeadingName = (LinkButton)gvTagged.Items[SelectedRow].FindControl("lblHeadingName");
                    LinkButton lblPrevHeadingName = (LinkButton)gvTagged.Items[prevRow].FindControl("lblHeadingName");

                    int SelectedRowValueId = common.myInt(((HiddenField)gvTagged.Items[SelectedRow].FindControl("hdnSectionId")).Value);
                    int PrevRowValueId = common.myInt(((HiddenField)gvTagged.Items[prevRow].FindControl("hdnSectionId")).Value);

                    int SelectedFieldValueId = common.myInt(((HiddenField)gvTagged.Items[SelectedRow].FindControl("hdnFieldId")).Value);
                    int PrevRowFieldValueId = common.myInt(((HiddenField)gvTagged.Items[prevRow].FindControl("hdnFieldId")).Value);

                    if (SelectedRowValueId == 0 && SelectedFieldValueId == 0)
                    {
                        dr = objDt.Select("Type = '" + common.myStr (strSelectedType) + "' AND HeadingName = '" + common.myStr(lblSelectedHeadingName.Text) + "'");
                        if (PrevRowValueId == 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr (strPrevType )+ "' AND HeadingName = '" + common.myStr ( lblPrevHeadingName.Text) + "'");

                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr ( lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt( PrevRowValueId));
                        }
                        if (PrevRowValueId == 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr ( lblPrevHeadingName.Text )+ "'  AND FieldId=" + common.myInt( PrevRowFieldValueId));
                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId) + " AND FieldId=" + common.myInt(PrevRowFieldValueId));

                        }

                    }
                    else if (SelectedRowValueId != 0 && SelectedFieldValueId == 0)
                    {
                        dr = objDt.Select("Type = '" + common.myStr(strSelectedType) + "' AND HeadingName = '" + common.myStr(lblSelectedHeadingName.Text) + "' AND SectionId=" + common.myInt(SelectedRowValueId));
                        if (PrevRowValueId == 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'");

                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId));
                        }
                        if (PrevRowValueId == 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'  AND FieldId=" + common.myInt(PrevRowFieldValueId));
                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId )+ " AND FieldId=" + common.myInt(PrevRowFieldValueId));

                        }
                    }
                    else if (SelectedRowValueId == 0 && SelectedFieldValueId != 0)
                    {
                        dr = objDt.Select("Type = '" + common.myStr(strSelectedType) + "' AND HeadingName = '" + common.myStr(lblSelectedHeadingName.Text) + " AND FieldId=" + common.myInt(SelectedFieldValueId));
                        if (PrevRowValueId == 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'");

                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId));
                        }
                        if (PrevRowValueId == 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'  AND FieldId=" + common.myInt(PrevRowFieldValueId));
                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId) + " AND FieldId=" + common.myInt(PrevRowFieldValueId));

                        }
                    }
                    else if (SelectedRowValueId != 0 && SelectedFieldValueId != 0)
                    {
                        dr = objDt.Select("Type = '" + common.myStr(strSelectedType) + "' AND HeadingName = '" + common.myStr(lblSelectedHeadingName.Text) + "' AND SectionId=" + common.myInt(SelectedRowValueId) + " AND FieldId=" + common.myInt(SelectedFieldValueId));
                        if (PrevRowValueId == 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'");

                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId));
                        }
                        if (PrevRowValueId == 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'  AND FieldId=" + common.myInt(PrevRowFieldValueId));
                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId) + " AND FieldId=" + common.myInt(PrevRowFieldValueId));

                        }
                    }

                    //DataRow[] dr = objDt.Select("Type = '" + strSelectedType + "' AND HeadingName = '" + lblSelectedHeadingName.Text + "' AND SectionId=" + SelectedRowValueId + " AND FieldId=" + SelectedFieldValueId);
                    //DataRow[] dr1 = objDt.Select("Type = '" + strPrevType + "' AND HeadingName = '" + lblPrevHeadingName.Text + "' AND SectionId=" + PrevRowValueId + " AND FieldId=" + PrevRowFieldValueId);
                    if ((dr1.Length > 0) && (dr.Length > 0))
                    {

                        int intDetailId = common.myInt(dr[0]["DetailId"]);
                        string strHeadingName = dr[0]["HeadingName"].ToString();
                        string strFieldId = dr[0]["FieldId"].ToString();
                        string strFieldName = dr[0]["FieldName"].ToString();
                        string strSectionId = dr[0]["SectionId"].ToString();
                        string strSectionName = dr[0]["SectionName"].ToString();
                        string strType = dr[0]["Type"].ToString();
                        string strTemplateName = dr[0]["TemplateName"].ToString();
                        string strTemplateTypeName = dr[0]["TemplateTypeName"].ToString();
                        bool ShowTemplateNameInNote = common.myBool(dr[0]["ShowTemplateNameInNote"]);
                        bool ShowSectionNameInNote = common.myBool(dr[0]["ShowSectionNameInNote"]);
                        bool ShowFieldNameInNote = common.myBool(dr[0]["ShowFieldNameInNote"]);

                        gvTagged.Items[selectedIdx - 1].Selected = true;

                        int intDetailId1 = common.myInt(dr1[0]["DetailId"]);
                        string strHeadingName1 = dr1[0]["HeadingName"].ToString();
                        string strFieldId1 = dr1[0]["FieldId"].ToString();
                        string strFieldName1 = dr1[0]["FieldName"].ToString();
                        string strSectionId1 = dr1[0]["SectionId"].ToString();
                        string strSectionName1 = dr1[0]["SectionName"].ToString();
                        string strType1 = dr1[0]["Type"].ToString();
                        string strTemplateName1 = dr1[0]["TemplateName"].ToString();
                        string strTemplateTypeName1 = dr1[0]["TemplateTypeName"].ToString();
                        bool ShowTemplateNameInNote1 = common.myBool(dr1[0]["ShowTemplateNameInNote"]);
                        bool ShowSectionNameInNote1 = common.myBool(dr1[0]["ShowSectionNameInNote"]);
                        bool ShowFieldNameInNote1 = common.myBool(dr1[0]["ShowFieldNameInNote"]);


                        //dr1[0]["DetailId"] = intDetailId;
                        //dr1[0]["HeadingName"] = strHeadingName;
                        //dr1[0]["FieldId"] = strFieldId;
                        //dr1[0]["FieldName"] = strFieldName;
                        //dr1[0]["SectionId"] = strSectionId;
                        //dr1[0]["SectionName"] = strSectionName;
                        //dr1[0]["Type"] = strType;
                        //dr1[0]["TemplateName"] = strTemplateName;
                        //dr1[0]["TemplateTypeName"] = strTemplateTypeName;
                        //dr1[0]["ShowTemplateNameInNote"] = ShowTemplateNameInNote;
                        //dr1[0]["ShowSectionNameInNote"] = ShowSectionNameInNote;
                        //dr1[0]["ShowFieldNameInNote"] = ShowFieldNameInNote;

                        //dr[0]["DetailId"] = intDetailId1;
                        //dr[0]["HeadingName"] = strHeadingName1;
                        //dr[0]["FieldId"] = strFieldId1;
                        //dr[0]["FieldName"] = strFieldName1;
                        //dr[0]["SectionId"] = strSectionId1;
                        //dr[0]["SectionName"] = strSectionName1;
                        //dr[0]["Type"] = strType1;
                        //dr[0]["TemplateName"] = strTemplateName1;
                        //dr[0]["TemplateTypeName"] = strTemplateTypeName1;
                        //dr[0]["ShowTemplateNameInNote"] = ShowTemplateNameInNote1;
                        //dr[0]["ShowSectionNameInNote"] = ShowSectionNameInNote1;
                        //dr[0]["ShowFieldNameInNote"] = ShowFieldNameInNote1;


                        dr1[0]["DetailId"] = intDetailId;
                        dr1[0]["HeadingName"] = strHeadingName;
                        if (strFieldId == "")
                        {
                            dr1[0]["FieldId"] = 0;
                        }
                        else
                        {
                            dr1[0]["FieldId"] = strFieldId;
                        }
                        dr1[0]["FieldName"] = strFieldName;
                        if (strSectionId == "")
                        {
                            dr1[0]["SectionId"] = 0;
                        }
                        else
                        {
                            dr1[0]["SectionId"] = strSectionId;
                        }
                        dr1[0]["SectionName"] = strSectionName;
                        dr1[0]["Type"] = strType;
                        dr1[0]["TemplateName"] = strTemplateName;
                        dr1[0]["TemplateTypeName"] = strTemplateTypeName;
                        dr1[0]["ShowTemplateNameInNote"] = ShowTemplateNameInNote;
                        dr1[0]["ShowSectionNameInNote"] = ShowSectionNameInNote;
                        dr1[0]["ShowFieldNameInNote"] = ShowFieldNameInNote;

                        if (intDetailId1 == null)
                        {
                            dr[0]["DetailId"] = 0;
                        }
                        else
                        {
                            dr[0]["DetailId"] = intDetailId1;
                        }

                        dr[0]["HeadingName"] = strHeadingName1;
                        if (strFieldId1 == "")
                        {
                            dr[0]["FieldId"] = 0;
                        }
                        else
                        {
                            dr[0]["FieldId"] = strFieldId1;
                        }

                        dr[0]["FieldName"] = strFieldName1;

                        if (strSectionId1 == "")
                        {
                            dr[0]["SectionId"] = 0;
                        }
                        else
                        {
                            dr[0]["SectionId"] = strSectionId1;
                        }
                        dr[0]["SectionName"] = strSectionName1;
                        dr[0]["Type"] = strType1;
                        dr[0]["TemplateName"] = strTemplateName1;
                        dr[0]["TemplateTypeName"] = strTemplateTypeName1;
                        dr[0]["ShowTemplateNameInNote"] = ShowTemplateNameInNote1;
                        dr[0]["ShowSectionNameInNote"] = ShowSectionNameInNote1;
                        dr[0]["ShowFieldNameInNote"] = ShowFieldNameInNote1;

                        gvTagged.DataSource = objDt;
                        gvTagged.DataBind();
                        gvTagged.Items[selectedIdx - 1].Selected = true;
                        ViewState["objDt"] = objDt;
                    }
                }

            }
            else if (e.CommandName == "MoveDown")
            {

                gvTagged.Items[selectedIdx].Selected = true;

                DataRow[] dr = null;
                DataRow[] dr1 = null;
                DataTable objDt = (DataTable)ViewState["objDt"];
                int SelectedRow = e.Item.ItemIndex;
                int prevRow = SelectedRow + 1;
                if (gvTagged.Items.Count > prevRow)
                {
                    string strSelectedType = ((HiddenField)gvTagged.Items[SelectedRow].FindControl("HdnType")).Value;
                    string strPrevType = ((HiddenField)gvTagged.Items[prevRow].FindControl("HdnType")).Value;

                    //Label lblSelectedHeadingName = (Label)gvTagged.Items[SelectedRow].FindControl("lblHeadingName");
                    //Label lblPrevHeadingName = (Label)gvTagged.Items[prevRow].FindControl("lblHeadingName");

                    LinkButton lblSelectedHeadingName = (LinkButton)gvTagged.Items[SelectedRow].FindControl("lblHeadingName");
                    LinkButton lblPrevHeadingName = (LinkButton)gvTagged.Items[prevRow].FindControl("lblHeadingName");

                    int SelectedRowValueId = common.myInt(((HiddenField)gvTagged.Items[SelectedRow].FindControl("hdnSectionId")).Value);
                    int PrevRowValueId = common.myInt(((HiddenField)gvTagged.Items[prevRow].FindControl("hdnSectionId")).Value);

                    int SelectedFieldValueId = common.myInt(((HiddenField)gvTagged.Items[SelectedRow].FindControl("hdnFieldId")).Value);
                    int PrevRowFieldValueId = common.myInt(((HiddenField)gvTagged.Items[prevRow].FindControl("hdnFieldId")).Value);


                    if (SelectedRowValueId == 0 && SelectedFieldValueId == 0)
                    {
                        dr = objDt.Select("Type = '" + common.myStr (strSelectedType) + "' AND HeadingName = '" + common.myStr ( lblSelectedHeadingName.Text )+ "'");
                        if (PrevRowValueId == 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'");

                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" +common.myInt( PrevRowValueId));
                        }
                        if (PrevRowValueId == 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'  AND FieldId=" + common.myInt(PrevRowFieldValueId));
                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId) + " AND FieldId=" + common.myInt(PrevRowFieldValueId));

                        }

                    }
                    else if (SelectedRowValueId != 0 && SelectedFieldValueId == 0)
                    {
                        dr = objDt.Select("Type = '" + common.myStr(strSelectedType) + "' AND HeadingName = '" + common.myStr(lblSelectedHeadingName.Text) + "' AND SectionId=" + common.myInt(SelectedRowValueId));
                        if (PrevRowValueId == 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'");

                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId));
                        }
                        if (PrevRowValueId == 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'  AND FieldId=" + common.myInt(PrevRowFieldValueId));
                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId) + " AND FieldId=" + common.myInt(PrevRowFieldValueId));

                        }
                    }
                    else if (SelectedRowValueId == 0 && SelectedFieldValueId != 0)
                    {
                        dr = objDt.Select("Type = '" + common.myStr(strSelectedType) + "' AND HeadingName = '" + common.myStr(lblSelectedHeadingName.Text) + " AND FieldId=" + common.myInt(SelectedFieldValueId));
                        if (PrevRowValueId == 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'");

                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId));
                        }
                        if (PrevRowValueId == 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'  AND FieldId=" + common.myInt(PrevRowFieldValueId));
                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId) + " AND FieldId=" + common.myInt(PrevRowFieldValueId));

                        }
                    }
                    else if (SelectedRowValueId != 0 && SelectedFieldValueId != 0)
                    {
                        dr = objDt.Select("Type = '" + common.myStr(strSelectedType) + "' AND HeadingName = '" + common.myStr(lblSelectedHeadingName.Text) + "' AND SectionId=" + common.myInt(SelectedRowValueId) + " AND FieldId=" + common.myInt(SelectedFieldValueId));
                        if (PrevRowValueId == 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'");

                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId == 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId));
                        }
                        if (PrevRowValueId == 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "'  AND FieldId=" + common.myInt(PrevRowFieldValueId));
                        }
                        if (PrevRowValueId != 0 && PrevRowFieldValueId != 0)
                        {
                            dr1 = objDt.Select("Type = '" + common.myStr(strPrevType) + "' AND HeadingName = '" + common.myStr(lblPrevHeadingName.Text) + "' AND SectionId=" + common.myInt(PrevRowValueId) + " AND FieldId=" + common.myInt(PrevRowFieldValueId));

                        }
                    }

                    if ((dr1.Length > 0) && (dr.Length > 0))
                    {
                        int intDetailId = common.myInt(dr[0]["DetailId"]);
                        string strHeadingName = dr[0]["HeadingName"].ToString();
                        string strFieldId = dr[0]["FieldId"].ToString();
                        string strFieldName = dr[0]["FieldName"].ToString();
                        string strSectionId = dr[0]["SectionId"].ToString();
                        string strSectionName = dr[0]["SectionName"].ToString();
                        string strType = dr[0]["Type"].ToString();
                        string strTemplateName = dr[0]["TemplateName"].ToString();
                        string strTemplateTypeName = dr[0]["TemplateTypeName"].ToString();
                        bool ShowTemplateNameInNote = common.myBool(dr[0]["ShowTemplateNameInNote"]);
                        bool ShowSectionNameInNote = common.myBool(dr[0]["ShowSectionNameInNote"]);
                        bool ShowFieldNameInNote = common.myBool(dr[0]["ShowFieldNameInNote"]);

                        gvTagged.Items[selectedIdx + 1].Selected = true;

                        int intDetailId1 = common.myInt(dr1[0]["DetailId"]);
                        string strHeadingName1 = dr1[0]["HeadingName"].ToString();
                        string strFieldId1 = dr1[0]["FieldId"].ToString();
                        string strFieldName1 = dr1[0]["FieldName"].ToString();
                        string strSectionId1 = dr1[0]["SectionId"].ToString();
                        string strSectionName1 = dr1[0]["SectionName"].ToString();
                        string strType1 = dr1[0]["Type"].ToString();
                        string strTemplateName1 = dr1[0]["TemplateName"].ToString();
                        string strTemplateTypeName1 = dr1[0]["TemplateTypeName"].ToString();
                        bool ShowTemplateNameInNote1 = common.myBool(dr1[0]["ShowTemplateNameInNote"]);
                        bool ShowSectionNameInNote1 = common.myBool(dr1[0]["ShowSectionNameInNote"]);
                        bool ShowFieldNameInNote1 = common.myBool(dr1[0]["ShowFieldNameInNote"]);


                        dr1[0]["DetailId"] = intDetailId;
                        dr1[0]["HeadingName"] = strHeadingName;
                        if (strFieldId == "")
                        {
                            dr1[0]["FieldId"] = 0;
                        }
                        else
                        {
                            dr1[0]["FieldId"] = strFieldId;
                        }
                        dr1[0]["FieldName"] = strFieldName;
                        if (strSectionId == "")
                        {
                            dr1[0]["SectionId"] = 0;
                        }
                        else
                        {
                            dr1[0]["SectionId"] = strSectionId;
                        }
                        dr1[0]["SectionName"] = strSectionName;
                        dr1[0]["Type"] = strType;
                        dr1[0]["TemplateName"] = strTemplateName;
                        dr1[0]["TemplateTypeName"] = strTemplateTypeName;
                        dr1[0]["ShowTemplateNameInNote"] = ShowTemplateNameInNote;
                        dr1[0]["ShowSectionNameInNote"] = ShowSectionNameInNote;
                        dr1[0]["ShowFieldNameInNote"] = ShowFieldNameInNote;

                        if (intDetailId1 == null)
                        {
                            dr[0]["DetailId"] = 0;
                        }
                        else
                        {
                            dr[0]["DetailId"] = intDetailId1;
                        }

                        dr[0]["HeadingName"] = strHeadingName1;
                        if (strFieldId1 == "")
                        {
                            dr[0]["FieldId"] = 0;
                        }
                        else
                        {
                            dr[0]["FieldId"] = strFieldId1;
                        }

                        dr[0]["FieldName"] = strFieldName1;

                        if (strSectionId1 == "")
                        {
                            dr[0]["SectionId"] = 0;
                        }
                        else
                        {
                            dr[0]["SectionId"] = strSectionId1;
                        }
                        dr[0]["SectionName"] = strSectionName1;
                        dr[0]["Type"] = strType1;
                        dr[0]["TemplateName"] = strTemplateName1;
                        dr[0]["TemplateTypeName"] = strTemplateTypeName1;
                        dr[0]["ShowTemplateNameInNote"] = ShowTemplateNameInNote1;
                        dr[0]["ShowSectionNameInNote"] = ShowSectionNameInNote1;
                        dr[0]["ShowFieldNameInNote"] = ShowFieldNameInNote1;

                        gvTagged.DataSource = objDt;
                        gvTagged.DataBind();
                        gvTagged.Items[selectedIdx + 1].Selected = true;
                        ViewState["objDt"] = objDt;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    private DataTable CreateTable()
    {
        DataTable objDt = new DataTable();
        objDt.Columns.Add("DetailId", typeof(int));
        objDt.Columns.Add("ID");
        objDt.Columns.Add("SectionId", typeof(int));
        objDt.Columns.Add("SectionName");
        objDt.Columns.Add("SequenceNo");
        objDt.Columns.Add("Type");
        objDt.Columns.Add("TemplateName");
        objDt.Columns.Add("TemplateTypeName");
        objDt.Columns.Add("FieldId", typeof(int));
        objDt.Columns.Add("FieldName");
        objDt.Columns.Add("HeadingName");

        objDt.Columns.Add("ShowTemplateNameInNote");
        objDt.Columns.Add("ShowSectionNameInNote");
        objDt.Columns.Add("ShowFieldNameInNote");
       // objDt.Columns.Add("ShowFieldNameInNote");
        objDt.Columns.Add("AllSectionManditory");

        objDt.Columns["ID"].AutoIncrement = true;
        return objDt;
    }
    protected void BindBlankSelectedValue()
    {
        try
        {
            DataTable Dt = new DataTable();

            Dt.Columns.Add("ID");
            Dt.Columns.Add("SectionId");
            Dt.Columns.Add("SectionName");
            Dt.Columns.Add("SequenceNo", typeof(int));
            Dt.Columns.Add("DetailId");
            Dt.Columns.Add("HeadingName");
            Dt.Columns.Add("TemplateName");
            Dt.Columns.Add("Type");
            Dt.Columns.Add("ShowTemplateNameInNote");
            Dt.Columns.Add("ShowSectionNameInNote");
            Dt.Columns.Add("ShowFieldNameInNote");
            Dt.Columns.Add("FieldName");
            Dt.Columns.Add("FieldId");
            Dt.Columns.Add("TemplateTypeName");
            Dt.Columns.Add("AllSectionManditory");




            DataRow dr = Dt.NewRow();

            dr["ID"] = "0";
            dr["SectionId"] = "";
            dr["SectionName"] = "";
            dr["SequenceNo"] = 0;
            dr["DetailId"] = 0;

            dr["HeadingName"] = "";
            dr["TemplateName"] = "";
            dr["Type"] = "";
            dr["ShowTemplateNameInNote"] = 0;
            dr["ShowSectionNameInNote"] = 0;
            dr["ShowFieldNameInNote"] = 0;
            dr["FieldName"] = "";
            dr["FieldId"] = "";
            dr["TemplateTypeName"] = "";
            dr["AllSectionManditory"] = false ;


            Dt.Rows.Add(dr);


            gvTagged.DataSource = Dt;
            gvTagged.DataBind();
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            bindDetailData();
            CheckIsCheckListRequired();
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
  private void   CheckIsCheckListRequired()
    {
        try
        {
            chkAllSectionManditory.Visible = false;

            if (ddlReport.SelectedIndex > 0)
            {
                if (common.myBool(ddlReport.SelectedItem.Attributes["IsCheckListRequired"]) && common.myStr(rdoTemplate.SelectedValue).Equals("D"))
                {
                    chkAllSectionManditory.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
           
        }

    }



    protected void btnAddHeading_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtHeadingName.Text.Trim() != "")
            {
                if (btnAddHeading.Text != "Update")
                {
                    lblMessage.Text = "";
                    int iSeqCount = 0;

                    DataTable objDt = CreateTable();
                    DataRow objDr;
                    if (ViewState["objDt"] != null)
                    {
                        objDt = (DataTable)ViewState["objDt"];
                    }
                    bool isAlredyExists = false;


                    if (objDt.Rows.Count > 0)
                    {
                        DataRow[] drexist = objDt.Select("HeadingName='" + txtHeadingName.Text.Trim() + "'");
                        if (drexist.Length > 0)
                        {
                            isAlredyExists = true;
                        }
                        else
                        {
                            objDr = objDt.NewRow();

                            objDr["SectionId"] = 0;
                            objDr["SectionName"] = "";
                            objDr["Type"] = "H";
                            objDr["TemplateName"] = "";
                            objDr["TemplateTypeName"] = "Heading";
                            objDr["SequenceNo"] = common.myStr(iSeqCount + 1);

                            objDr["HeadingName"] = txtHeadingName.Text;
                            objDr["ShowTemplateNameInNote"] = common.myBool(0);
                            objDr["ShowSectionNameInNote"] = common.myBool(0);
                            objDr["ShowFieldNameInNote"] = common.myBool(0);
                            objDr["AllSectionManditory"] = common.myBool(chkAllSectionManditory.Checked);

                            objDt.Rows.Add(objDr);
                            isAlredyExists = false;
                        }
                    }
                    else
                    {
                        objDr = objDt.NewRow();

                        objDr["SectionId"] = 0;
                        objDr["SectionName"] = "";
                        objDr["Type"] = "H";
                        objDr["TemplateName"] = "";
                        objDr["TemplateTypeName"] = "Heading";
                        objDr["SequenceNo"] = common.myStr(iSeqCount + 1);

                        objDr["HeadingName"] = txtHeadingName.Text;
                        objDr["ShowTemplateNameInNote"] = common.myBool(0);
                        objDr["ShowSectionNameInNote"] = common.myBool(0);
                        objDr["ShowFieldNameInNote"] = common.myBool(0);
                        objDr["AllSectionManditory"] = common.myBool(chkAllSectionManditory.Checked);

                        objDt.Rows.Add(objDr);
                        isAlredyExists = false;
                    }

                    if (!isAlredyExists)
                    {
                        int i = 1;
                        foreach (DataRow dr in objDt.Rows)
                        {
                            dr["ID"] = i++;
                        }
                        gvTagged.DataSource = objDt;
                        gvTagged.DataBind();
                        ViewState["objDt"] = objDt;
                    }

                    if (isAlredyExists)
                    {
                        lblMessage.Text = "Heading Name '" + txtHeadingName.Text.Trim() + "' Already Exists !";
                    }

                }
                else
                {

                    if (common.myInt(ViewState["DeatilId"]) != 0)
                    {
                        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        Hashtable hsinput = new Hashtable();
                        hsinput.Add("@intDetailId", common.myInt(ViewState["DeatilId"]));
                        hsinput.Add("@chvHeaderName", common.myStr(txtHeadingName.Text));
                        string strupdate = "";

                        strupdate = "UPDATE  EMRTemplateReportSetupDetails SET HeadingName =@chvHeaderName WHERE DetailId=@intDetailId";
                        int i = dl.ExecuteNonQuery(CommandType.Text, strupdate, hsinput);
                        if (i == 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            lblMessage.Text = "Update Header Name !";
                            txtHeadingName.Text = "";
                            ViewState["DeatilId"] = null;
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = i.ToString();
                        }
                        btnAddHeading.Text = "Add";
                        bindDetailData();
                    }

                    else
                    {

                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "select Header Name !";
                    }
                }
            }

        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }

    protected void rdoTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        DataSet ds = master.GetAllTypeTemplates(Convert.ToInt16(common.myInt(Session["HospitalLocationId"])), rdoTemplate.SelectedValue, 
                                                common.myInt(Session["FacilityId"]));

        if (rdoTemplate.SelectedValue == "S")
        {
            RadgvPages.Visible = true;
            radgvFields.Visible = false;

            ddlTemplatename.Text = "";
            ddlTemplatename.Items.Clear();

            ddlSections.Text = "";
            ddlSections.Items.Clear();

            ddlTemplatename.Enabled = false;
            ddlSections.Enabled = false;

            chkShowTemplateName.Checked = false;
            chkShowTemplateName.Enabled = false;

            chkShowSectionName.Checked = false;
            chkShowSectionName.Enabled = false;

            chkShowFieldName.Checked = false;
            chkShowFieldName.Enabled = false;

            if (ds.Tables[0].Rows.Count > 0)
            {
                RadgvPages.DataSource = ds.Tables[0];
                RadgvPages.DataBind();
            }
            else
            {
                RadgvPages.DataSource = ds.Tables[0].Clone();
            }

        }
        else
        {

            chkShowTemplateName.Checked = false;
            chkShowTemplateName.Enabled = true;

            chkShowSectionName.Checked = false;
            chkShowSectionName.Enabled = true;

            chkShowFieldName.Checked = false;
            chkShowFieldName.Enabled = true;

            ddlTemplatename.Items.Clear();
            if (rdoTemplate.SelectedValue == "D")
            {
                RadgvPages.Visible = true;
                radgvFields.Visible = false;

                ddlSections.Text = "";
                ddlSections.Items.Clear();
                ddlSections.Enabled = false;
            }
            else if (rdoTemplate.SelectedValue == "F")
            {
                RadgvPages.Visible = false;
                radgvFields.Visible = true;

                ddlSections.Text = "";
                ddlSections.Items.Clear();
                ddlSections.Enabled = true;
            }

            ddlTemplatename.Enabled = true;

            RadComboBoxItem item;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                item = new RadComboBoxItem();

                item.Text = common.myStr(dr["TemplateName"]);
                item.Value = common.myInt(dr["TemplateId"]).ToString();
                item.Attributes.Add("Type", common.myStr(dr["Type"]));

                ddlTemplatename.Items.Add(item);
                ddlTemplatename.DataBind();
            }
            ddlTemplatename.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlTemplatename.SelectedIndex = 0;

            if (rdoTemplate.SelectedValue == "D")
            {
                ddlTemplate_OnSelectedIndexChanged(this, e);
            }
            else
            {
                ddlSections_OnSelectedIndexChanged(this, e);
            }
        }
        CheckIsCheckListRequired();
    }

    protected void lblHeadingName_OnClick(Object sender, EventArgs e)
    {
        try
        {
            GridItem row = (GridItem)(((LinkButton)sender).NamingContainer);
            string DetailId = ((Label)row.FindControl("lblDetailId")).Text;
            if(common.myInt(DetailId).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please save then click on link";
                return ;
            }

            RadWindowForNew.NavigateUrl = "~/EMR/Masters/DefaultTextPopup.aspx?DetailId="+ DetailId;
            RadWindowForNew.Height = 400;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 20;
            RadWindowForNew.Left = 20;
          //  RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            //  RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
  
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}