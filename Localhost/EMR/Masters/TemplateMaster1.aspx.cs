using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
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

public partial class EMR_Masters_TemplateMaster1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();
    private Hashtable hshIn;
    private Hashtable hshOut;
    DAL.DAL dl;
    StringBuilder strSQL;
    BaseC.EncryptDecrypt objEncDc = new BaseC.EncryptDecrypt();
    ArrayList coll;

    private enum GridProperty : byte
    {
        Propertyid = 0,
        SerialNo = 1,
        PropertyName = 2,
        Type = 3,
        Gender = 4,
        Active = 5,
        encodedby = 6,
        encodeddate = 7,
        MaxLength = 8,
        DefaultValue = 9,
        Deactivate = 10,
        Status = 11,
        Detail = 12,
        Select = 13,
        DataObject = 14
    }

    private enum GridTemplate : byte
    {
        TemplateId = 0,
        SerialNo = 1,
        //Select = 2,
        TemplateName = 2,
        Type = 3,
        Code = 4,
        Active = 6,
        encodedby = 5,
        //encodeddate = 7,
        Deactivate = 7,
        Edit = 8,
        Status = 9,
        TemplateTypeID = 10,
        DisplayTitle = 11
    }
    private enum GridColgvValueDropDown : byte
    {
        col0 = 0,
        COL1 = 1,
        COL2 = 2,
        COL3 = 3,
        COL4 = 4
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MP"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblFieldMessage.Text = "";
        if (!IsPostBack)
        {

            //OnClientClick="if(pos!=null){pos.close();} pos=window.open('ReArrangeTree.aspx?Tmp=sec','ReArrangeTree','resizable=no,scrollbars=yes,width=490,height=320');pos.moveTo(346,110); return false;" 

            //OnClientClick="if(pos!=null){pos.close();} pos=window.open('ReArrangeTree.aspx?Tmp=fld','ReArrangeTree','resizable=no,scrollbars=yes,width=490,height=320');pos.moveTo(346,300); return false;"
            //            btnFields.Attributes.Add("onclick", " return openRadWindow('fld')");
            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            }

            ViewState["FieldId"] = "0";
            pnlNewEditCategorySubCategory.Visible = false;
            //ViewState["FullSectionID"] = null;
            ViewState["Category"] = "Category";

            ViewState["TemplateID"] = common.myInt(Request.QueryString["tempid"]).ToString();// Session["TmpId"].ToString();
            ViewState["TemplateTypeID"] = common.myInt(Request.QueryString["tempid"]).ToString();

            btnSort.Attributes.Add("onclick", " return openRadWindow('sec', '" + ViewState["TemplateID"].ToString() + "')");
            // lbtnServices.Attributes.Add("onclick", " return openRadWindowServices()");

            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshIn = new Hashtable();
            hshIn.Add("@Id", ViewState["TemplateTypeID"]);

            DataSet ds = dl.FillDataSet(CommandType.Text, "select TemplateName from emrtemplate where id=@id", hshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblTemplateName.Text = ds.Tables[0].Rows[0][0].ToString().Trim();
            }

            BindCategoryTree();
            ddlPropertyType_OnSelectedIndexChanged(sender, e);
            ibtnProtertySave.Visible = true;
            if (tvCategory.Nodes.Count > 0)
                lbtnEditCategory_OnClick(this, null);
            ibtnCategorySave.Visible = true;
            ibtnProtertySave.Visible = false;
            rbtnNoTabular.Checked = true;
            btnBack.Visible = true;
            trFieldStatus.Visible = false;
            //Session["ArrangeT"] = null;

        }
        if (lblSelectedCategoryID.Text != "")
            hdnMenuId.Value = lblSelectedCategoryID.Text;
        if (lblSelectedFieldID.Text != "")
            hdnMenuId2.Value = lblSelectedFieldID.Text;
        //if (common.myStr(ddlPropertyType.SelectedValue).Equals("M"))
        //{
        //    //txtPropertyDefaultMale.MaxLength = 4001;
        //    //txtPropertyDefaultFemale.MaxLength = 4001;
        //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "return MaxLenTxt('" + txtPropertyDefaultMale + "', 4000);", true);
        //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "return MaxLenTxt('" + txtPropertyDefaultFemale + "', 4000);", true);
        //}
        //else if (common.myStr(ddlPropertyType.SelectedValue).Equals("T"))
        //{
        //    //txtPropertyDefaultMale.MaxLength = 11;
        //    //txtPropertyDefaultFemale.MaxLength = 11;
        //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "return MaxLenTxt('" + txtPropertyDefaultMale + "', 10);", true);
        //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "return MaxLenTxt('" + txtPropertyDefaultFemale + "', 10);", true);
        //}
    }

    protected void btnpost_Click(object sender, EventArgs e)
    {
        Response.Redirect("TemplateMaster1.aspx?tempid=" + ViewState["TemplateTypeID"], false);
    }

    protected void chkDefaultNo_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkDefaultNo.Checked)
            chkDefaultYes.Checked = false;
        else
            chkDefaultNo.Checked = false;
    }

    protected void chkDefaultYes_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkDefaultYes.Checked)
            chkDefaultNo.Checked = false;
        else
            chkDefaultYes.Checked = false;
    }

    protected void rbocreatesection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbocreatesection.SelectedValue == "1")
        {
            lbtnNewCategory_OnClick(sender, e);
        }
        else if (rbocreatesection.SelectedValue == "2")
        {
            lbtnNewCategorySub_OnClick(sender, e);
        }
        else if (rbocreatesection.SelectedValue == "3")
        {
            tvCategory_OnSelectedNodeChanged(sender, e);
        }
    }

    protected void rboCreateFields_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["FieldValueData"] = null;
        if (rboCreateFields.SelectedValue == "1")
        {
            lbtnSubNewField_OnClick(sender, e);
        }
        else if (rboCreateFields.SelectedValue == "2")
        {
            lbtnNewSubField_OnClick(sender, e);
        }
        else if (rboCreateFields.SelectedValue == "3")
        {
            TreeProperties_OnNodeSelected(sender, e);
        }
    }

    protected void chkDropDownList_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = sender as CheckBox;
            GridViewRow row = chk.NamingContainer as GridViewRow;
            int index = common.myInt(((HiddenField)row.FindControl("hdnIndex")).Value);
            if (chk.Checked == false)
                chk.Checked = false;
            else
            {
                foreach (GridViewRow r in gvValueDropDown.Rows)
                {
                    CheckBox chkTemp = r.FindControl("chkDefault") as CheckBox;
                    int indexT = common.myInt(((HiddenField)r.FindControl("hdnIndex")).Value);
                    if (index != indexT)
                        chkTemp.Checked = false;
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void TreeProperties_OnNodeSelected(object sender, EventArgs e)
    {
        try
        {
            rbocreatesection.ClearSelection();
            rboCreateFields.SelectedIndex = rboCreateFields.Items.IndexOf(rboCreateFields.Items.FindByValue("3"));

            ViewState["Field"] = "Update";
            if (treeProperties.SelectedValue != "")
            {
                if (CheckFieldTabular() > 0)
                {
                    ddlPropertyType.Items.Remove(new ListItem("WordProcessor", "W"));
                    ddlPropertyType.Items.Remove(new ListItem("CheckBox", "C"));
                }
                else
                {
                    if (!ddlPropertyType.Items.Contains(new ListItem("WordProcessor", "W"))
                        && !ddlPropertyType.Items.Contains(new ListItem("CheckBox", "C")))
                    {
                        ddlPropertyType.Items.Insert(3, new ListItem("WordProcessor", "W"));
                        ddlPropertyType.Items.Insert(4, new ListItem("CheckBox", "C"));
                    }
                }
                Label lableControl = new Label();

                lblSelectedFieldID.Text = treeProperties.SelectedValue;
                hdnMenuId2.Value = treeProperties.SelectedValue;
                String FieldID = treeProperties.SelectedValue.Substring(1, treeProperties.SelectedValue.Length - 1);
                String lblFieldID = treeProperties.SelectedNode.Text;
                String lblFieldType = treeProperties.SelectedNode.Target;
                TreeNode tn = treeProperties.SelectedNode;
                ViewState["FType"] = lblFieldType;
                ViewState["FName"] = lblFieldID;
                strSQL = new StringBuilder();
                hshIn = new Hashtable();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshIn.Add("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
                hshIn.Add("intFieldID", common.myInt(common.myStr(FieldID).Trim()));
                DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateFieldDetails", hshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtPropertyName.Text = common.myStr(ds.Tables[0].Rows[0].ItemArray[2]);
                    txtPropertyCode.Text = common.myStr(ds.Tables[0].Rows[0].ItemArray[3]);

                    ddlCoumnsPerRow.SelectedIndex = ddlCoumnsPerRow.Items.IndexOf(ddlCoumnsPerRow.Items.FindByValue(common.myStr(common.myStr(ds.Tables[0].Rows[0]["ColumnNosToDisplay"]))));

                    if (common.myStr(lblFieldType).Trim().Equals("H"))
                    {
                        txtHeadingStart.Text = common.myInt(ds.Tables[0].Rows[0]["HeadingStart"]).ToString();
                        txtHeadingEnd.Text = common.myInt(ds.Tables[0].Rows[0]["HeadingEnd"]).ToString();
                    }
                    if (common.myStr(lblFieldType).Trim().Equals("IS") || common.myStr(lblFieldType).Trim().Equals("I"))
                    {
                        lbtnServices.Visible = true;
                    }
                    else
                    {
                        lbtnServices.Visible = false;
                    }

                    ddlPropertyType.SelectedIndex = ddlPropertyType.Items.IndexOf(ddlPropertyType.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0].ItemArray[4])));
                    ddlPropertyType_OnSelectedIndexChanged(sender, e);

                    ddlPropertyGender.SelectedIndex = ddlPropertyGender.Items.IndexOf(ddlPropertyGender.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0].ItemArray[6])));
                    ddlPropertyGender_OnSelectedIndexChanged(sender, e);

                    chkTitleField.Checked = common.myBool(ds.Tables[0].Rows[0].ItemArray[14]);
                    if (common.myInt(ds.Tables[0].Rows[0].ItemArray[8]) > 0)
                    {
                        chkDefaultYes.Checked = true;
                        chkDefaultNo.Checked = false;
                    }
                    else
                    {
                        chkDefaultNo.Checked = true;
                        chkDefaultYes.Checked = false;
                    }

                    txtPropertyDefaultMale.Text = common.myStr(ds.Tables[0].Rows[0].ItemArray[9]);
                    txtPropertyDefaultFemale.Text = common.myStr(ds.Tables[0].Rows[0].ItemArray[10]);
                    txtPropertyDefaultContentWordProcessor.Content = common.myStr(ds.Tables[0].Rows[0].ItemArray[11]);

                    txtMaxLength.Text = common.myStr(ds.Tables[0].Rows[0].ItemArray[5]);
                    if (common.myInt(ds.Tables[0].Rows[0].ItemArray[7]) > 0)
                    {
                        ddlPatientDataObject.SelectedIndex = ddlPatientDataObject.Items.IndexOf(ddlPatientDataObject.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0].ItemArray[7])));
                    }
                    ddlFieldStatus.SelectedIndex = ddlFieldStatus.Items.IndexOf(ddlFieldStatus.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0].ItemArray[12])));
                    trFieldStatus.Visible = true;
                    // ddlPropertyType_OnSelectedIndexChanged(this, e);
                    ddlStaticTemplate.SelectedIndex = ddlStaticTemplate.Items.IndexOf(ddlStaticTemplate.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["StaticTemplateId"])));
                    if (common.myInt(ds.Tables[0].Rows[0].ItemArray[15]) > 0)
                    {
                        BindTemplateFieldFormats();
                        ddlTemplateFieldFormats.SelectedIndex = ddlTemplateFieldFormats.Items.IndexOf(ddlTemplateFieldFormats.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0].ItemArray[15])));
                        ddlTemplateFieldFormats.Visible = true;

                        rboFormats.Visible = true;
                        txtFormatName.Visible = false;

                        rboFormats.SelectedIndex = 1;
                    }

                    if (common.myBool(ds.Tables[0].Rows[0]["IsMandatory"]))
                    {
                        ddlIsMandatory.SelectedValue = "1";
                        ddlMandatoryType.Enabled = true;
                        ddlMandatoryType.SelectedIndex = ddlMandatoryType.Items.IndexOf(ddlMandatoryType.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0]["MandatoryType"])));
                    }
                    else
                    {
                        ddlIsMandatory.SelectedValue = "0";
                        ddlMandatoryType.SelectedIndex = 0;
                        ddlMandatoryType.Enabled = false;
                    }

                    if (common.myBool(ds.Tables[0].Rows[0]["IsLinkRequire"]))
                    {
                        ddlLinkRequired.SelectedIndex = ddlLinkRequired.Items.IndexOf(ddlLinkRequired.Items.FindByValue("1"));
                        txtLinkUrl.Text = common.myStr(ds.Tables[0].Rows[0]["LinkUrl"]);
                    }
                    else
                    {
                        ddlLinkRequired.SelectedIndex = ddlLinkRequired.Items.IndexOf(ddlLinkRequired.Items.FindByValue("0"));
                        txtLinkUrl.Text = string.Empty;
                    }
                    rdoDisplayInNote.SelectedIndex = rdoDisplayInNote.Items.IndexOf(rdoDisplayInNote.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0]["DisplayInNote"])));
                    chkSortingOnSequenceNo.Checked = common.myBool(ds.Tables[0].Rows[0]["IsSortingOnSequenceNo"]);
                    chkPrintValueLineWise.Checked = common.myBool(ds.Tables[0].Rows[0]["PrintValueLineWise"]);
                }
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        if (common.myStr(ddlPropertyType.SelectedItem.Value).Equals("C")
                            || common.myStr(ddlPropertyType.SelectedItem.Value).Equals("D")
                            || common.myStr(ddlPropertyType.SelectedItem.Value).Equals("R")
                            || common.myStr(ddlPropertyType.SelectedItem.Value).Equals("IM"))
                        {
                            gvValueDropDown.DataSource = ds.Tables[1];
                            gvValueDropDown.DataBind();

                            btnSetValueOrder.Attributes.Add("onclick", " return openRadWindow1('vid', '" + ViewState["SectionID"].ToString() + "','" + FieldID.ToString() + "')");


                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                if (common.myInt(ds.Tables[1].Rows[i]["ValueId"]).Equals(common.myInt(ds.Tables[0].Rows[0]["defaultValue"])))
                                {
                                    CheckBox chk = (CheckBox)gvValueDropDown.Rows[i].FindControl("chkDefault");
                                    chk.Checked = true;
                                }
                            }
                        }
                        else if (common.myStr(ddlPropertyType.SelectedItem.Value).Equals("B"))
                        {
                            if (common.myInt(ds.Tables[1].Rows[0].ItemArray[0]) > 0)
                            {
                                txtBooleanYesCode.Text = common.myStr(ds.Tables[1].Rows[0].ItemArray[2]);
                                txtBooleanYesMale.Text = common.myStr(ds.Tables[1].Rows[0].ItemArray[4]);
                                txtBooleanYesFemale.Text = common.myStr(ds.Tables[1].Rows[0].ItemArray[5]);
                                txtBooleanNoCode.Text = common.myStr(ds.Tables[1].Rows[1].ItemArray[2]);
                                txtBooleanNoMale.Text = common.myStr(ds.Tables[1].Rows[1].ItemArray[4]);
                                txtBooleanNoFemale.Text = common.myStr(ds.Tables[1].Rows[1].ItemArray[5]);
                                hdnYes.Value = common.myStr(ds.Tables[1].Rows[1]["ValueId"]);
                                hdnNo.Value = common.myStr(ds.Tables[1].Rows[0]["ValueId"]);
                            }
                            else
                            {
                                txtBooleanYesCode.Text = common.myStr(ds.Tables[1].Rows[1].ItemArray[2]);
                                txtBooleanYesMale.Text = common.myStr(ds.Tables[1].Rows[1].ItemArray[4]);
                                txtBooleanYesFemale.Text = common.myStr(ds.Tables[1].Rows[1].ItemArray[5]);
                                txtBooleanNoCode.Text = common.myStr(ds.Tables[1].Rows[0].ItemArray[2]);
                                txtBooleanNoMale.Text = common.myStr(ds.Tables[1].Rows[0].ItemArray[4]);
                                txtBooleanNoFemale.Text = common.myStr(ds.Tables[1].Rows[0].ItemArray[5]);
                                hdnYes.Value = common.myStr(ds.Tables[1].Rows[1]["ValueId"]);
                                hdnNo.Value = common.myStr(ds.Tables[1].Rows[0]["ValueId"]);
                            }
                        }
                    }
                    ViewState["FieldValueData"] = ds.Tables[1];
                }
                else
                {
                    hdnYes.Value = "";
                    hdnNo.Value = "";
                }
                ViewState["FieldId"] = FieldID.ToString().Trim();
                ibtnCategorySave.Visible = false;
                ibtnProtertySave.Visible = true;
                hdnMenuId2.Value = lblSelectedFieldID.Text;
            }
            trHeader1.Visible = false;
            trOptions.Visible = false;
            pnlNewEditCategorySubCategory.Visible = false;
            pnlNewEditField.Visible = true;

            //if (common.myStr(ddlPropertyType.SelectedValue).Equals("M"))
            //{
            //    //txtPropertyDefaultMale.MaxLength = 4001;
            //    //txtPropertyDefaultFemale.MaxLength = 4001;
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "return MaxLenTxt('"+ txtPropertyDefaultMale+"', 4000);", true);
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "return MaxLenTxt('" + txtPropertyDefaultFemale + "', 4000);", true);

            //}
            //else if (common.myStr(ddlPropertyType.SelectedValue).Equals("T"))
            //{
            //    //txtPropertyDefaultMale.MaxLength = 11;
            //    //txtPropertyDefaultFemale.MaxLength = 11;
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "return MaxLenTxt('" + txtPropertyDefaultMale + "', 10);", true);
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "return MaxLenTxt('" + txtPropertyDefaultFemale + "', 10);", true);
            //}
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //protected void gvProperties_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Label lableControl = new Label();
    //    String FieldID = HttpUtility.HtmlDecode(gvProperties.SelectedRow.Cells[Convert.ToByte(GridProperty.Propertyid)].Text.ToString());
    //    String SerialNo = HttpUtility.HtmlDecode(gvProperties.SelectedRow.Cells[Convert.ToByte(GridProperty.SerialNo)].Text.ToString());
    //    Label lblFieldID = (Label)gvProperties.SelectedRow.FindControl("LName");
    //    Label lblFieldType = (Label)gvProperties.SelectedRow.FindControl("lblPropertyType");

    //    strSQL = new StringBuilder();
    //    hshIn = new Hashtable();
    //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hshIn.Add("inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
    //    hshIn.Add("intFieldID", common.myInt(FieldID.ToString().Trim()));
    //    DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateFieldDetails", hshIn);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        txtPropertyName.Text = ds.Tables[0].Rows[0].ItemArray[2].ToString();
    //        txtPropertyCode.Text = ds.Tables[0].Rows[0].ItemArray[3].ToString();
    //        ddlPropertyType.SelectedValue = ds.Tables[0].Rows[0].ItemArray[4].ToString();
    //        ddlPropertyType_OnSelectedIndexChanged(sender, e);
    //        ddlPropertyGender.SelectedValue = ds.Tables[0].Rows[0].ItemArray[6].ToString();
    //        ddlPropertyGender_OnSelectedIndexChanged(sender, e);
    //        //ddlDefault.SelectedValue = ds.Tables[0].Rows[0].ItemArray[8].ToString();
    //        if (common.myInt(ds.Tables[0].Rows[0].ItemArray[8]) == 1)
    //            chkDefaultYes.Checked = true;
    //        else
    //            chkDefaultNo.Checked = false;
    //        txtPropertyDefaultMale.Text = ds.Tables[0].Rows[0].ItemArray[9].ToString();
    //        txtPropertyDefaultFemale.Text = ds.Tables[0].Rows[0].ItemArray[10].ToString();
    //        txtMaxLength.Text = ds.Tables[0].Rows[0].ItemArray[5].ToString();
    //        ddlPatientDataObject.SelectedValue = ds.Tables[0].Rows[0].ItemArray[7].ToString();
    //        ddlFieldStatus.SelectedValue = ds.Tables[0].Rows[0].ItemArray[11].ToString();
    //        trFieldStatus.Visible = true;
    //    }
    //    if (ds.Tables.Count > 1)
    //    {
    //        if (ds.Tables[1].Rows.Count > 0)
    //        {
    //            if ((ddlPropertyType.SelectedItem.Value == "C") || (ddlPropertyType.SelectedItem.Value == "D"))
    //            {
    //                gvValueDropDown.DataSource = ds.Tables[1];
    //                gvValueDropDown.DataBind();
    //            }
    //            else if (ddlPropertyType.SelectedItem.Value == "B")
    //            {
    //                if (ds.Tables[1].Rows[0].ItemArray[0].ToString() == "1")
    //                {
    //                    txtBooleanYesCode.Text = ds.Tables[1].Rows[0].ItemArray[2].ToString();
    //                    txtBooleanYesMale.Text = ds.Tables[1].Rows[0].ItemArray[4].ToString();
    //                    txtBooleanYesFemale.Text = ds.Tables[1].Rows[0].ItemArray[5].ToString();
    //                    txtBooleanNoCode.Text = ds.Tables[1].Rows[1].ItemArray[2].ToString();
    //                    txtBooleanNoMale.Text = ds.Tables[1].Rows[1].ItemArray[4].ToString();
    //                    txtBooleanNoFemale.Text = ds.Tables[1].Rows[1].ItemArray[5].ToString();
    //                }
    //                else
    //                {
    //                    txtBooleanYesCode.Text = ds.Tables[1].Rows[1].ItemArray[2].ToString();
    //                    txtBooleanYesMale.Text = ds.Tables[1].Rows[1].ItemArray[4].ToString();
    //                    txtBooleanYesFemale.Text = ds.Tables[1].Rows[1].ItemArray[5].ToString();
    //                    txtBooleanNoCode.Text = ds.Tables[1].Rows[0].ItemArray[2].ToString();
    //                    txtBooleanNoMale.Text = ds.Tables[1].Rows[0].ItemArray[4].ToString();
    //                    txtBooleanNoFemale.Text = ds.Tables[1].Rows[0].ItemArray[5].ToString();
    //                }
    //            }
    //        }
    //    }
    //    ViewState["FieldId"] = FieldID.ToString().Trim();
    //    ibtnTemplateSave.Visible = false;
    //    ibtnCategorySave.Visible = false;
    //    ibtnProtertySave.Visible = true;
    //}

    //protected void btnBack_OnClick(object sender, EventArgs e)
    //{
    //    Response.Redirect("ClinicalTemplate.aspx", false);
    //}

    //private void populateTemplateView()
    //{
    //    mltVW.SetActiveView(vwTemplates);
    //    populateTemplateControls();
    //    BindTemplateGrid();
    //}

    protected void NewTemplate_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/EMR/Default.aspx?Page=" + objEncDc.Encrypt("8", objEncDc.getKey(sConString), true, null).Trim() + "&tb=Tem", false);
    }

    //protected void SaveTemplate_OnClick(Object sender, EventArgs e)
    //{
    //    SaveUpdateTemplate();
    //}

    protected void SaveCategory_OnClick(Object sender, EventArgs e)
    {
        SaveUpdateCategory();
    }

    protected void SaveProperty_OnClick(Object sender, EventArgs e)
    {
        SaveUpdateFields();
    }

    //private void SaveUpdateTemplate()
    //{
    //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    if (txtTemplateName.Text.ToString().Trim().Length == 0)
    //    {
    //        Alert.ShowAjaxMsg("Name Cannot Be Blank...", this.Page);
    //        return;
    //    }
    //    if ((ddlTemplateType.SelectedItem.Value == "0") || (ddlTemplateType.SelectedIndex == 0))
    //    {
    //        Alert.ShowAjaxMsg("Please Select Any Type ...", this.Page);
    //        return;
    //    }
    //    lblStatusT.Visible = false;
    //    ddlStatus.Visible = false;
    //    hshIn = new Hashtable();
    //    hshOut = new Hashtable();
    //    hshIn.Add("@SectionsBold", chkBoldSections.Checked ? "1" : null);
    //    hshIn.Add("@SectionsItalic", chkItalicSections.Checked ? "1" : null);
    //    hshIn.Add("@SectionsUnderline", chkUnderlineSections.Checked ? "1" : null);
    //    hshIn.Add("@SectionsForecolor", txtColorSections.Text.Trim() == "" ? null : txtColorSections.Text.Trim());
    //    hshIn.Add("@SectionsListStyle", ddlListStyleSections.SelectedValue == "0" ? null : ddlListStyleSections.SelectedValue);
    //    hshIn.Add("@SectionsFontStyle", ddlFontTypeSections.SelectedValue == "0" ? null : ddlFontTypeSections.SelectedValue);
    //    hshIn.Add("@SectionsFontSize", ddlFontSizeSections.SelectedValue == "0" ? null : ddlFontSizeSections.SelectedValue);

    //    hshIn.Add("@FieldsBold", chkBoldFields.Checked ? "1" : null);
    //    hshIn.Add("@FieldsItalic", chkItalicFields.Checked ? "1" : null);
    //    hshIn.Add("@FieldsUnderline", chkUnderlineFields.Checked ? "1" : null);
    //    hshIn.Add("@FieldsForecolor", txtColorFields.Text.Trim() == "" ? null : txtColorFields.Text.Trim());
    //    hshIn.Add("@FieldsListStyle", ddlListStyleFields.SelectedValue == "0" ? null : ddlListStyleFields.SelectedValue);
    //    hshIn.Add("@FieldsFontStyle", ddlFontTypeFields.SelectedValue == "0" ? null : ddlFontTypeFields.SelectedValue);
    //    hshIn.Add("@FieldsFontSize", ddlFontSizeFields.SelectedValue == "0" ? null : ddlFontSizeFields.SelectedValue);
    //    hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
    //    if (ViewState["TemplateEdit"] != null)
    //    {
    //        if (ViewState["TemplateEdit"].ToString() == "TemplateEdit")
    //        {
    //            hshIn.Add("@ID", common.myInt(ViewState["TemplateId"]));
    //            hshIn.Add("@chvTemplateName", bc.ParseQ(txtTemplateName.Text));
    //            hshIn.Add("@chvAssignCode", bc.ParseQ(txtAssignCode.Text));
    //            hshIn.Add("@inyTemplateTypeID", bc.ParseQ(ddlTemplateType.SelectedValue));
    //            hshIn.Add("@inySpecialisationId", bc.ParseQ(ddlSpecialisation.SelectedValue));
    //            hshIn.Add("@Active", bc.ParseQ(ddlStatus.SelectedValue));
    //            hshIn.Add("@bitDisplayTitle", chkTitleNote.Checked == true ? 1 : 0);
    //            hshIn.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));

    //            //string sqlQuery = "UPDATE EMRTemplate Set Templatename=@Name, TemplateTypeId=@TemplateTypeId, "
    //            //+ " Active=@Active, Code=@Code, DisplayTitle=@DisplayTitle, SpecialisationId=@SpecialisationId, "
    //            //+ "SectionsBold = @SectionsBold, "
    //            // + "SectionsItalic = @SectionsItalic , "
    //            // + "SectionsUnderline = @SectionsUnderline , "
    //            // + "SectionsForecolor =  @SectionsForecolor, "
    //            // + "SectionsListStyle = @SectionsListStyle , "
    //            // + "SectionsFontStyle  = @SectionsFontStyle, "
    //            // + "SectionsFontSize  = @SectionsFontSize, "
    //            // + "FieldsBold  = @FieldsBold, "
    //            // + "FieldsItalic  = @FieldsItalic, "
    //            // + "FieldsUnderline = @FieldsUnderline , "
    //            // + "FieldsForecolor  = @FieldsForecolor, "
    //            // + "FieldsListStyle  = @FieldsListStyle, "
    //            // + "FieldsFontStyle  = @FieldsFontStyle, "
    //            // + "FieldsFontSize  = @FieldsFontSize "
    //            //+ " WHERE ID = @ID and Hospitallocationid=@HospitalLocationID";
    //            int i = dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRUpdateTemplate", hshIn, hshOut);
    //            if (i == 0)
    //            {
    //                gvTemplates.EditIndex = -1;
    //                MakeEmptyTemplateInputFields("Record(s) has been Updated successfully.");
    //            }
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        hshIn.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
    //        hshIn.Add("@chvTemplateName", bc.ParseQ(txtTemplateName.Text.ToString()));
    //        hshIn.Add("@inyTemplateTypeID", ddlTemplateType.SelectedItem.Value);
    //        hshIn.Add("@intEncodedBy", common.myInt(Session["UserID"]));
    //        hshIn.Add("@chvAssignCode", bc.ParseQ(txtAssignCode.Text.ToString().Trim()));
    //        hshIn.Add("@bitDisplayTitle", chkTitleNote.Checked ? 1 : 0);
    //        hshIn.Add("@inySpecialisationId", ddlSpecialisation.SelectedValue);


    //        hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveTemplate", hshIn, hshOut);
    //        MakeEmptyTemplateInputFields("Record(s) has been Saved successfully.");
    //    }
    //}
    //protected void btnTemplateNew_OnClick(object sender, EventArgs e)
    //{
    //    ViewState["TemplateEdit"] = "TemplateNew";
    //    lblStatusT.Visible = false;
    //    ddlStatus.Visible = false;
    //    MakeEmptyTemplateInputFields("");
    //}
    //public void MakeEmptyTemplateInputFields(string msg)
    //{
    //    lblTemplateMessage.Text = msg;
    //    ddlTemplateType.SelectedIndex = 0;
    //    ddlStatus.SelectedIndex = 0;
    //    ddlSpecialisation.SelectedIndex = 0;
    //    txtTemplateName.Text = "";
    //    txtAssignCode.Text = "";
    //    txtTemplateName.Text = "";
    //    chkTitleNote.Checked = false;
    //    BindTemplateGrid();
    //}
    //private void BindTemplateGrid()
    //{
    //    strSQL = new StringBuilder();
    //    hshIn = new Hashtable();
    //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
    //    DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetTemplate", hshIn);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        gvTemplates.DataSource = ds;
    //        gvTemplates.DataBind();
    //    }
    //}

    protected void tvCategory_OnSelectedNodeChanged(Object sender, EventArgs e)
    {
        try
        {
            rbocreatesection.SelectedValue = "3";
            rboCreateFields.ClearSelection();
            //trHeaderCategory.Visible = false;
            //ClearCategoryFields();
            //ClearPropertiesFields();
            //ViewState["FieldId"] = null;
            //hdnMenuId.Value = tvCategory.SelectedValue;
            lblSelectedCategoryID.Text = tvCategory.SelectedValue;
            //lblFieldName.Text = tvCategory.SelectedNode.Text;
            //txtCategoryName.Text = "";
            //ltrlCategoryStatus.Visible = false;
            //ddlCategoryStatus.Visible = false;
            //pnlNewEditCategorySubCategory.Visible = false;
            //pnlNewEditField.Visible = true;

            ViewState["SectionID"] = null;
            int ParentCategoryID = 0;
            int ChildCategoryID = 0;
            //ViewState["FullSectionID"] = tvCategory.SelectedNode.Value;
            if (tvCategory.SelectedNode.Value.StartsWith("P"))
            {
                ParentCategoryID = common.myInt(tvCategory.SelectedNode.Value.Substring(1, tvCategory.SelectedNode.Value.Length - 1));
                ViewState["SectionID"] = ParentCategoryID;
                ViewState["SectionName"] = tvCategory.SelectedNode.Text.ToString().Trim();
            }
            else if (tvCategory.SelectedNode.Value.StartsWith("C"))
            {
                ChildCategoryID = common.myInt(tvCategory.SelectedNode.Value.Substring(1, tvCategory.SelectedNode.Value.Length - 1));
                ViewState["SectionID"] = ChildCategoryID;
                ViewState["SectionName"] = tvCategory.SelectedNode.Text.ToString().Trim();
            }
            BindFieldTree();
            lbtnEditCategory_OnClick(this, null);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lbtnSubNewField_OnClick(object sender, EventArgs e)//New Field
    {
        try
        {
            if (CheckFieldTabular() > 0)
            {
                ddlPropertyType.Items.Remove(new ListItem("WordProcessor", "W"));
                ddlPropertyType.Items.Remove(new ListItem("CheckBox", "C"));
            }
            else
            {
                if (ddlPropertyType.Items.Contains(new ListItem("WordProcessor", "W")) == false && ddlPropertyType.Items.Contains(new ListItem("CheckBox", "C")) == false)
                {
                    ddlPropertyType.Items.Insert(3, new ListItem("WordProcessor", "W"));
                    ddlPropertyType.Items.Insert(4, new ListItem("CheckBox", "C"));
                }
            }
            lbtnServices.Visible = false;
            trHeader1.Visible = false;
            trOptions.Visible = false;
            pnlNewEditField.Visible = true;
            pnlNewEditValues.Visible = false;
            txtPropertyName.Text = "";
            txtPropertyCode.Text = "";
            ddlPropertyType.SelectedIndex = 0;
            ddlPropertyGender.SelectedIndex = 0;
            txtPropertyDefaultMale.Text = "";
            txtPropertyDefaultFemale.Text = "";
            txtMaxLength.Text = "";
            ddlFieldStatus.SelectedIndex = 0;
            ddlIsMandatory.SelectedValue = "0";
            ddlMandatoryType.SelectedIndex = 0;
            ddlMandatoryType.Enabled = false;
            txtPropertyDefaultContentWordProcessor.Content = null;
            //pnlNewEditValues.Visible = false;
            //---------------------------------------------
            ibtnCategorySave.Text = "Save";
            ibtnCategorySave.ToolTip = "Save Field";
            ViewState["Field"] = "NewMainField";
            ViewState["Category"] = null;
            //pnlNewEditCategorySubCategory.Visible = true;
            //pnlNewEditField.Visible = false;
            hdnMenuId2.Value = lblSelectedFieldID.Text;
            ltrlCategoryStatus.Visible = false;
            ddlCategoryStatus.Visible = false;
            //lblSelectedCategory.Text = "New Section";
            lblFieldName.Text = "New Field";
            txtCategoryName.Text = "";

            ibtnCategorySave.Visible = false;
            ibtnProtertySave.Visible = true;
            pnlNewEditCategorySubCategory.Visible = false;
            //  pnlNewEditField.Visible = true;//by rk
            ClearPropertiesFields();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lbtnEditField_OnClick(object sender, EventArgs e)//Edit
    {
        try
        {
            trHeader1.Visible = true;
            TreeProperties_OnNodeSelected(this, null);
            pnlNewEditField.Visible = true;
            pnlNewEditValues.Visible = true;
            ibtnCategorySave.Text = "Save";
            ibtnCategorySave.ToolTip = "Update Field";
            ViewState["Field"] = "EditMainField";
            ViewState["Category"] = null;
            //pnlNewEditCategorySubCategory.Visible = true;
            //pnlNewEditField.Visible = false;
            hdnMenuId2.Value = lblSelectedFieldID.Text;

            //PopulateSelectedCategoryData();
            PopulateSelectedFieldData();
            ltrlCategoryStatus.Visible = true;
            ddlCategoryStatus.Visible = true;
            if (treeProperties.SelectedNode != null)
            {
                lblFieldName.Text = treeProperties.SelectedNode.Text;
            }

            ibtnCategorySave.Visible = false;
            ibtnProtertySave.Visible = true;
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lbtnNewSubField_OnClick(object sender, EventArgs e)//Sub-Field
    {
        try
        {
            int chkval = CheckSubFieldTabular();
            if (treeProperties.SelectedNode != null)
            {
                if (chkval > 0)
                {
                    pnlNewEditField.Visible = false;
                    pnlNewEditValues.Visible = false;
                    lblFieldMessage.Text = "Now you can't create sub field because section is already created in tabular form.";
                    lblFieldMessage.ForeColor = Color.Red;
                    //// return;
                }
                else
                {
                    pnlNewEditField.Visible = true;
                }
                ltrlParentNode.Text = treeProperties.SelectedNode.Text;
                Label lableControl = new Label();
                lblSelectedFieldID.Text = treeProperties.SelectedValue;
                hdnMenuId2.Value = treeProperties.SelectedValue;
                String FieldID = treeProperties.SelectedValue.Substring(1, treeProperties.SelectedValue.Length - 1);
                String lblFieldID = treeProperties.SelectedNode.Text;
                String lblFieldType = treeProperties.SelectedNode.Target;
                TreeNode tn = treeProperties.SelectedNode;
                ltrlParentNode.Text = treeProperties.SelectedNode.Text;
                strSQL = new StringBuilder();
                hshIn = new Hashtable();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshIn.Add("inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
                hshIn.Add("intFieldID", common.myInt(FieldID.ToString().Trim()));
                DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateFieldDetails", hshIn);
                hdnParentFieldType.Value = ds.Tables[0].Rows[0]["FieldType"].ToString();
                string FieldType = hdnParentFieldType.Value;
                if (!(FieldType == "D" || FieldType == "B" || FieldType == "R") && chkval == 0)
                {
                    Alert.ShowAjaxMsg("If Field Type Only DropdownList and Boolean Type Of Field Then can be Sub-Field.", Page);
                    rbocreatesection.ClearSelection();
                    rboCreateFields.ClearSelection();
                    return;
                }
                trHeader1.Visible = true;
                trOptions.Visible = true;
                Hashtable hs = new Hashtable();
                hs.Add("@FieldId", FieldID);
                DataSet dds = new DataSet();
                if (ds.Tables[0].Rows[0]["FieldType"].ToString() == "B")
                {
                    ddlOptions.Items.Clear();
                    ddlOptions.Items.Add(new ListItem("No", "0", true));
                    ddlOptions.Items.Add(new ListItem("Yes", "1", true));
                    if (ds.Tables[0].Rows[0]["DefaultValue"].ToString() == "1")
                        ddlOptions.SelectedIndex = 1;//Yes
                    else
                        ddlOptions.SelectedIndex = 0;//No
                }
                else
                {
                    dds = dl.FillDataSet(CommandType.Text, "Select ValueId Id,ValueName Name from EMRTemplateValues Where FieldId=@FieldId And Active=1 Order by ValueId desc", hs);
                    if (dds.Tables[0].Rows.Count > 0)
                    {

                        //if (ds.Tables[0].Rows[0]["FieldType"].ToString() == "B")
                        //{
                        //    ddlOptions.Items.Clear();
                        //    ddlOptions.Items.Add(new ListItem("No", "0", true));
                        //    ddlOptions.Items.Add(new ListItem("Yes", "1", true));
                        //    //if (dds.Tables[0].Rows.Count > 0)
                        //    //{
                        //    //    if (ds.Tables[0].Rows[0]["DefaultValue"].ToString() == "1")//Yes
                        //    //        ddlOptions.Items[ddlOptions.Items.IndexOf(ddlOptions.Items.FindByText("Yes"))].Selected = true;
                        //    //    else
                        //    //        ddlOptions.Items[ddlOptions.Items.IndexOf(ddlOptions.Items.FindByText("No"))].Selected = true;
                        //    //}
                        //    //else
                        //    //{
                        //    if (ds.Tables[0].Rows[0]["DefaultValue"].ToString() == "1")
                        //        ddlOptions.SelectedIndex = 1;//Yes
                        //    else
                        //        ddlOptions.SelectedIndex = 0;//No
                        //    //}
                        //}
                        //else
                        //{
                        ddlOptions.DataSource = dds;
                        ddlOptions.DataValueField = "Id";
                        ddlOptions.DataTextField = "Name";
                        ddlOptions.DataBind();
                        if (common.myInt(ds.Tables[0].Rows[0]["DefaultValue"]) != 0)
                            ddlOptions.Items[ddlOptions.Items.IndexOf(ddlOptions.Items.FindByValue(ds.Tables[0].Rows[0]["DefaultValue"].ToString()))].Selected = true;
                        // ddlOptions.Items[ddlOptions.Items.IndexOf(ddlOptions.Items.FindByText(ds.Tables[0].Rows[0]["DefaultValue"].ToString()))].Selected = true; 
                    }
                }
                //  pnlNewEditField.Visible = true;  //by rk
                txtPropertyName.Text = "";
                txtPropertyCode.Text = "";
                ddlPropertyType.SelectedIndex = 0;
                ddlPropertyGender.SelectedIndex = 0;
                txtPropertyDefaultMale.Text = "";
                txtPropertyDefaultFemale.Text = "";
                txtPropertyDefaultContentWordProcessor.Content = null;
                txtMaxLength.Text = "";
                ddlFieldStatus.SelectedIndex = 0;
                ddlIsMandatory.SelectedValue = "0";
                ddlMandatoryType.SelectedIndex = 0;
                ddlMandatoryType.Enabled = false;
                pnlNewEditValues.Visible = false;
                ///////////////////////////////////////////////////////////////
                ibtnCategorySave.Text = "Save";
                ibtnCategorySave.ToolTip = "Save Sub-Field";
                ViewState["Field"] = "NewMainSubField";
                ViewState["Category"] = null;
                //pnlNewEditCategorySubCategory.Visible = true;
                //pnlNewEditField.Visible = false;
                pnlNewEditValues.Visible = false;
                hdnMenuId2.Value = lblSelectedFieldID.Text;
                ltrlCategoryStatus.Visible = false;
                ddlCategoryStatus.Visible = false;
                if (treeProperties.SelectedNode != null)
                {
                    //lblSelectedCategory.Text = tvCategory.SelectedNode.Text;
                    //lblFieldName.Text = treeProperties.SelectedNode.Text;
                    lblFieldName.Text = "New Sub-Field";
                }
                ibtnCategorySave.Visible = false;
                ibtnProtertySave.Visible = true;

            }
            else
            {
                //  Alert.ShowAjaxMsg("Please click on desired Field first to create Sub-Field then use right click.", Page);
                if (chkval > 0)
                {
                    lblFieldMessage.Text = "Now you can't create sub field because section is already created in tabular form.";
                    lblFieldMessage.ForeColor = Color.Red;
                }
                else
                {
                    Alert.ShowAjaxMsg("Please fistly select any Field name then after to create Sub-Field .", Page);
                }
            }
            rbocreatesection.ClearSelection();
            rboCreateFields.ClearSelection();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void ClearCategoryFields()
    {
        txtCategoryName.Text = "";
        ltrlCategoryStatus.Visible = false;
        ddlCategoryStatus.Visible = false;
    }

    private void SaveUpdateCategory()
    {
        try
        {
            strSQL = new StringBuilder();
            hshIn = new Hashtable();
            hshOut = new Hashtable();

            hshIn.Add("@intEncodedBy", common.myInt(Session["UserID"]));
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            if (ViewState["Category"] != null)
            {
                hshIn.Add("@inyGender", common.myStr(ddlCategoryGender.SelectedItem.Value));
                hshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hshIn.Add("@intTemplateID", common.myInt(ViewState["TemplateID"]));
                hshIn.Add("@chvSectionName", bc.ParseQ(txtCategoryName.Text));
                hshIn.Add("@chvCode", common.myStr(txtSectionCode.Text, true).Trim());
                hshIn.Add("@Tabular", common.myBool(rbtnYesTabular.Checked) ? 1 : 0);
                hshIn.Add("@DisplayTitle", common.myBool(chkSectionTitle.Checked) ? 1 : 0);
                hshIn.Add("@intTRows", common.myInt(txtRows.Text));
                hshIn.Add("@inyNoOfBlankRows", common.myInt(txtBlankRows.Text));
                hshIn.Add("@bitIsFreezeFirstColumn", common.myInt(rdoIsFreezeFirstColumn.SelectedValue));
                hshIn.Add("@bitIsAddendum", common.myBool(rdoAddendum.SelectedValue));

                if (common.myStr(ViewState["Category"]) == "NewMainCategory")
                {
                    hshIn.Add("@intParentId", DBNull.Value);
                    hshIn.Add("@bitActive", 1);
                    hshIn.Add("@intParameterId", 0);
                    hshIn.Add("@MainSub", 0);  // 0-> Main, 1 -> Sub
                }
                else if (common.myStr(ViewState["Category"]) == "EditMainCategory")
                {
                    if (CheckSectionDataTabular() > 0)
                    {
                        return;
                    }
                    hshIn.Add("@intParentId", DBNull.Value);
                    hshIn.Add("@bitActive", common.myInt(ddlCategoryStatus.SelectedItem.Value));
                    hshIn.Add("@intParameterId", common.myInt(lblSelectedCategoryID.Text.Substring(1)));
                    hshIn.Add("@MainSub", 0);  // 0-> Main, 1 -> Sub   
                }
                else if (common.myStr(ViewState["Category"]) == "NewMainCategorySub")
                {
                    hshIn.Add("@intParentId", common.myInt(lblSelectedCategoryID.Text.Substring(1)));
                    hshIn.Add("@bitActive", 1);
                    hshIn.Add("@intParameterId", 0);
                    hshIn.Add("@MainSub", 1);  // 0-> Main, 1 -> Sub   
                }
                else if (common.myStr(ViewState["Category"]) == "NewSubCategory")
                {
                    hshIn.Add("@intParentId", common.myInt(lblSelectedCategoryID.Text.Substring(1)));
                    hshIn.Add("@bitActive", 1);
                    hshIn.Add("@intParameterId", 0);
                    hshIn.Add("@MainSub", 1);  // 0-> Main, 1 -> Sub   
                }
                else if (common.myStr(ViewState["Category"]) == "EditSubCategory")
                {
                    if (CheckSectionDataTabular() > 0)
                    {
                        return;
                    }
                    hshIn.Add("@intParentId", DBNull.Value);
                    hshIn.Add("@bitActive", common.myInt(ddlCategoryStatus.SelectedItem.Value));
                    hshIn.Add("@intParameterId", common.myInt(lblSelectedCategoryID.Text.Substring(1)));
                    hshIn.Add("@MainSub", 1);  // 0-> Main, 1 -> Sub   
                }
                else if (common.myStr(ViewState["Category"]) == "NewSubNewCategory")
                {
                    hshIn.Add("@intParentId", common.myInt(lblSelectedCategoryID.Text.Substring(1)));
                    hshIn.Add("@bitActive", 1);
                    hshIn.Add("@intParameterId", 0);
                    hshIn.Add("@MainSub", 1);  // 0-> Main, 1 -> Sub   
                }

                #region rows defination
                int sno = 0;
                int ActiveSno = 0;
                strSQL = new StringBuilder();

                foreach (GridViewRow gvr in gvValueDropDown.Rows)
                {
                    TextBox txtOption = (TextBox)gvr.FindControl("txtOptionGrid");

                    if (common.myStr(txtOption.Text).Trim().Length > 0)
                    {
                        TextBox txtDefaultText = (TextBox)gvr.FindControl("txtDefaultTextGrid");
                        TextBox txtCode = (TextBox)gvr.FindControl("txtCodeGrid");
                        TextBox txtDefaultMale = (TextBox)gvr.FindControl("txtDefaultMaleGrid");
                        TextBox txtDefaultFemale = (TextBox)gvr.FindControl("txtDefaultFemaleGrid");
                        CheckBox chkDefault = (CheckBox)gvr.FindControl("chkDefault");
                        HiddenField hdnValueId = gvr.FindControl("hdnValueId") as HiddenField;
                        DropDownList ddlStatus = gvr.FindControl("ddlStatus") as DropDownList;

                        if (chkDefault.Checked)
                        {
                            //sDefaultValue = hdnValueId.Value;
                        }

                        coll = new ArrayList();
                        coll.Add(common.myInt(hdnValueId.Value));//ValueId int,
                        coll.Add(common.myStr(txtCode.Text, true).Trim());//Code Varchar(20),
                        coll.Add(common.myStr(txtOption.Text, true).Trim());//ValueName Varchar(50),
                        coll.Add(common.myStr(txtDefaultMale.Text, true).Trim());//DefaultTextMale Varchar(50),
                        coll.Add(common.myStr(txtDefaultFemale.Text, true).Trim());//DefaultTextFemale Varchar(50),
                        coll.Add(++sno);//SequenceNo tinyint,
                        coll.Add(common.myInt(ddlStatus.SelectedValue));//Active bit,
                        coll.Add(common.myBool(chkDefault.Checked) ? 1 : 0);//DefaultValue int


                        strSQL.Append(common.setXmlTable(ref coll));

                        if (common.myInt(ddlStatus.SelectedValue) == 1)
                        {
                            ++ActiveSno;
                        }
                    }
                }
                hshIn.Add("@xmlValues", strSQL.ToString());
                if (common.myInt(txtRows.Text) != ActiveSno)
                {
                    lblFieldMessage.ForeColor = Color.Red;
                    lblFieldMessage.Text = "No. of rows and defination of active rows must be same !";

                    return;
                }

                #endregion



                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateTemplateSection", hshIn, hshOut);
            }

            BindCategoryTree();
            BindFieldTree();
            lblFieldMessage.Text = "Record(s) has been updated successfully...";
            lblFieldMessage.ForeColor = Color.Green;
            //ibtnCategorySave.Visible = false;
            //ibtnProtertySave.Visible = true;
            txtCategoryName.Focus();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private int CheckSectionDataTabular()// for edit
    {
        int retval = 0;
        try
        {
            string tabularcheckvalue = rbtnYesTabular.Checked == true ? "True" : "False";
            if (hdntabularcheck.Value != tabularcheckvalue)
            {

                if (common.myInt(lblSelectedCategoryID.Text == "" ? "0" : lblSelectedCategoryID.Text.Substring(1)) > 0)
                {
                    Hashtable hasTablecheck = new Hashtable();
                    hasTablecheck.Add("@intParameterId", lblSelectedCategoryID.Text.Substring(1));
                    hasTablecheck.Add("@TemplateId", ViewState["TemplateID"].ToString());
                    object tabularcheck = dl.ExecuteScalar(CommandType.StoredProcedure, "UspCheckTemplate", hasTablecheck);
                    if (common.myInt(tabularcheck) > 0 || tvCategory.SelectedNode.ChildNodes.Count > 0 || CheckCountSubfield() > 0)
                    {
                        lblFieldMessage.Text = "Now you can't change tabular field.";
                        lblFieldMessage.ForeColor = Color.Red;
                        retval = 1;
                        if (rbtnYesTabular.Checked == true)
                        {
                            rbtnYesTabular.Checked = false;
                            rbtnNoTabular.Checked = true;
                        }
                    }
                    else
                    {
                        retval = 0;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return retval;
    }

    int CheckCountSubfield()
    {
        int ret = 0;
        for (int i = 0; i < treeProperties.Nodes.Count; i++)
        {
            if (treeProperties.Nodes[i].ChildNodes.Count > 0)
            {
                ret = 1;
            }
        }
        return ret;
    }

    private int CheckSubSectionDataTabular()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        int retval = 0;
        try
        {
            if (common.myInt(lblSelectedCategoryID.Text == "" ? "0" : lblSelectedCategoryID.Text.Substring(1)) > 0)
            {
                Hashtable hasTableSectioncheck = new Hashtable();
                hasTableSectioncheck.Add("@intParameterId", lblSelectedCategoryID.Text.Substring(1));
                // hasTableSectioncheck.Add("@TemplateId", ViewState["TemplateID"].ToString());
                object subsectioncheck = new object();
                subsectioncheck = dl.ExecuteScalar(CommandType.StoredProcedure, "UspCheckTemplateSubSection", hasTableSectioncheck);
                if (common.myInt(subsectioncheck) > 0)
                    retval = 1;
                else
                    retval = 0;
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return retval;
    }

    private int CheckFieldTabular()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        int retval = 0;
        try
        {
            string tabularcheckvalue = rbtnYesTabular.Checked == true ? "True" : "False";
            if (hdntabularcheck.Value != tabularcheckvalue)
            {
                if (common.myInt(lblSelectedCategoryID.Text == "" ? "0" : lblSelectedCategoryID.Text.Substring(1)) > 0)
                {
                    Hashtable hasTableSectioncheck = new Hashtable();
                    hasTableSectioncheck.Add("@intParameterId", lblSelectedCategoryID.Text.Substring(1));
                    object subsectioncheck = new object();
                    subsectioncheck = dl.ExecuteScalar(CommandType.StoredProcedure, "UspCheckTemplateSubSection", hasTableSectioncheck);
                    if (common.myInt(subsectioncheck) > 0)
                    {
                        retval = 1;
                    }
                    else
                    {
                        retval = 0;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return retval;
    }

    private int CheckSubFieldTabular()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        int retval = 0;
        try
        {
            string tabularcheckvalue = rbtnYesTabular.Checked == true ? "True" : "False";
            if (hdntabularcheck.Value != tabularcheckvalue)
            {
                if (common.myInt(lblSelectedCategoryID.Text == "" ? "0" : lblSelectedCategoryID.Text.Substring(1)) > 0)
                {
                    Hashtable hasTableSectioncheck = new Hashtable();
                    hasTableSectioncheck.Add("@intParameterId", lblSelectedCategoryID.Text.Substring(1));
                    object subsectioncheck = new object();
                    subsectioncheck = dl.ExecuteScalar(CommandType.StoredProcedure, "UspCheckTemplateSubSection", hasTableSectioncheck);
                    if (common.myInt(subsectioncheck) > 0)
                    {
                        retval = 1;
                    }
                    else
                    {
                        retval = 0;
                    }
                }
            }
            else
            {
                if (rbtnYesTabular.Checked == true)
                    retval = 1;
                else
                    retval = 0;
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return retval;
    }
    protected void checkDirectory(string iSectionId)
    {
        DirectoryInfo objHospitalDir = new DirectoryInfo(Server.MapPath("/Images/TemplateImage/" + iSectionId));
        if (objHospitalDir.Exists == false)
        {
            objHospitalDir.Create();
        }
        DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/Images/TemplateImage/" + iSectionId));
        if (objDir.Exists == false)
        {
            objDir.Create();
        }
    }
    private void SaveUpdateFields()
    {
        string sMaxLength = "0";
        string sDefaultValue = "0";
        string sDefaultFormat = "0";
        string sDefaultMaleText = string.Empty;
        string sDefaultFemaleText = string.Empty;
        string sContentWordProcessor = string.Empty;
        string sFormatName = string.Empty;
        string sFormatId = string.Empty;

        StringBuilder sXMLValues = new StringBuilder();
        StringBuilder sbForUpdate = new StringBuilder();
        StringBuilder sbTableBorderStyle = new StringBuilder();
        

        string datobject = null;

        hshIn = new Hashtable();
        hshOut = new Hashtable();

            coll = new ArrayList();

        int i = 0;

        try
        {
            if (ViewState["SectionID"] == null)
            {
                Alert.ShowAjaxMsg("Please Select Any Category ...", this.Page);
                return;
            }
            if (ddlPropertyType.SelectedItem.Value == "T")
            {
                //sDefaultValue = txtDefaultMale.Text.ToString().Trim();
                if (common.myInt(txtMaxLength.Text) == 0)
                    sMaxLength = "50";
                else
                    sMaxLength = bc.ParseQ(txtMaxLength.Text.ToString().Trim());
            }
            else if (ddlPropertyType.SelectedItem.Value == "W")
            {
                if (chkFieldFormatDefault.Checked)
                    sDefaultFormat = "1";
                else
                    sDefaultFormat = "0";

                if (txtPropertyDefaultContentWordProcessor.Content != null)
                {
                    sbTableBorderStyle.Append(txtPropertyDefaultContentWordProcessor.Content);
                    sbTableBorderStyle.Replace("<table>", "<table border='1' style='border:1px solid black;border-collapse:collapse;'>");
                    sbTableBorderStyle.Replace("<td>", "<td border='1' style='border:1px solid black;border-collapse:collapse;'>");

                    sContentWordProcessor =Convert.ToString(sbTableBorderStyle);
					
                    //sContentWordProcessor = txtPropertyDefaultContentWordProcessor.Content;
                    if (rboFormats.SelectedValue == "1") // for Add New Word Processor Format 
                    {
                        if (common.myStr(txtFormatName.Text).Trim() == "")
                        {
                            txtFormatName.BorderColor = Color.Tomato;
                            Alert.ShowAjaxMsg("Please Enter Option Name", this.Page);
                            return;
                        }
                        else
                        {
                            sFormatId = "0";
                            sFormatName = common.myStr(txtFormatName.Text).Trim();
                        }
                    }
                    else // For Existing Word Processor Format Id.
                    {
                        if (common.myStr(ddlTemplateFieldFormats.SelectedIndex) != "0")
                        {
                            sFormatId = common.myStr(ddlTemplateFieldFormats.SelectedItem.Value);
                            sFormatName = String.Empty;
                        }
                        else
                        {
                            ddlTemplateFieldFormats.BorderColor = Color.Tomato;
                            Alert.ShowAjaxMsg("Please Select an Option!", this.Page);
                            return;
                        }
                    }
                }
                else
                    sContentWordProcessor = "";
            }
            else if ((ddlPropertyType.SelectedItem.Value == "D") || (ddlPropertyType.SelectedItem.Value == "C" || ddlPropertyType.SelectedItem.Value == "R"))
            {
                i = 1;
                sXMLValues.Remove(0, sXMLValues.Length);
                coll = new ArrayList();
                foreach (GridViewRow gvr in gvValueDropDown.Rows)
                {
                    TextBox txtOption = (TextBox)gvr.FindControl("txtOptionGrid");
                    if (bc.ParseQ(txtOption.Text.ToString().Trim()).Length > 0)
                    {
                        TextBox txtDefaultText = (TextBox)gvr.FindControl("txtDefaultTextGrid");
                        TextBox txtCode = (TextBox)gvr.FindControl("txtCodeGrid");
                        TextBox txtDefaultMale = (TextBox)gvr.FindControl("txtDefaultMaleGrid");
                        TextBox txtDefaultFemale = (TextBox)gvr.FindControl("txtDefaultFemaleGrid");
                        CheckBox chkDefault = (CheckBox)gvr.FindControl("chkDefault");
                        HiddenField hdnValueId = gvr.FindControl("hdnValueId") as HiddenField;
                        DropDownList ddlStatus = gvr.FindControl("ddlStatus") as DropDownList;
                        TextBox txtScoreValue = (TextBox)gvr.FindControl("txtScoreValue");

                        if (chkDefault.Checked)
                        {
                            sDefaultValue = hdnValueId.Value;
                        }

                        coll.Add(common.myInt(hdnValueId.Value));
                        coll.Add(common.myStr(txtCode.Text, true).Trim());
                        coll.Add(common.myStr(txtOption.Text, true).Trim());
                        coll.Add(common.myStr(txtDefaultMale.Text, true).Trim());
                        coll.Add(common.myStr(txtDefaultFemale.Text, true).Trim());
                        coll.Add(i);
                        coll.Add(common.myInt(ddlStatus.SelectedValue));
                        coll.Add(common.myBool(chkDefault.Checked) ? 1 : 0);
                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        coll.Add((common.myLen(txtScoreValue.Text) > 0) ? common.myDec(txtScoreValue.Text).ToString() : string.Empty);//ScoreValue
                        sXMLValues.Append(common.setXmlTable(ref coll));
                        i++;
                    }
                }
            }
            else if (ddlPropertyType.SelectedItem.Value == "IM")
            {
                i = 1;
                sXMLValues.Remove(0, sXMLValues.Length);
                coll = new ArrayList();
                foreach (GridViewRow gvr in gvValueDropDown.Rows)
                {

                    TextBox txtOption = (TextBox)gvr.FindControl("txtOptionGrid");
                    if (bc.ParseQ(txtOption.Text.ToString().Trim()).Length > 0)
                    {
                        TextBox txtDefaultText = (TextBox)gvr.FindControl("txtDefaultTextGrid");
                        TextBox txtCode = (TextBox)gvr.FindControl("txtCodeGrid");
                        TextBox txtDefaultMale = (TextBox)gvr.FindControl("txtDefaultMaleGrid");
                        TextBox txtDefaultFemale = (TextBox)gvr.FindControl("txtDefaultFemaleGrid");
                        CheckBox chkDefault = (CheckBox)gvr.FindControl("chkDefault");
                        HiddenField hdnValueId = gvr.FindControl("hdnValueId") as HiddenField;
                        DropDownList ddlStatus = gvr.FindControl("ddlStatus") as DropDownList;

                        FileUpload _FileUpload = (FileUpload)gvr.FindControl("_FileUpload");

                        Label lblImageName = (Label)gvr.FindControl("lblImageName");
                        if (lblImageName.Text == "")
                        {
                            lblImageName.Text = _FileUpload.FileName;
                        }

                        string ext = _FileUpload.FileName == "" ? Path.GetExtension(lblImageName.Text) : Path.GetExtension(_FileUpload.FileName);
                        if (ext != ".jpg")
                        {
                            Alert.ShowAjaxMsg("Please Upload JPG Image Only", Page); return;
                        }

                        string sImagePath = "";
                        if (chkDefault.Checked)
                        {
                            sDefaultValue = hdnValueId.Value;
                        }
                        checkDirectory(common.myStr(ViewState["SectionID"]));

                        try
                        {
                            System.Drawing.Bitmap varBmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(Server.MapPath("/Images/TemplateImage/") + lblImageName.Text);
                            System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(varBmp);
                            varBmp.Dispose();

                            newBitmap.Save(Server.MapPath("/Images/TemplateImage/") + common.myInt(ViewState["SectionID"]) + "/" + lblImageName.Text);

                            sImagePath = "/Images/TemplateImage/" + common.myInt(ViewState["SectionID"]) + "/" + lblImageName.Text;

                            //string strFileFullPath = Server.MapPath("~/Images/TemplateImage/") + common.myInt(ViewState["SectionID"]) + @"\" + lblImageName.Text;
                            //_FileUpload.SaveAs(strFileFullPath);
                            //System.Drawing.Bitmap varBmp = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strFileFullPath);
                            //System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(varBmp);
                            //varBmp.Dispose();
                            //newBitmap.Save(strFileFullPath);
                            //sImagePath = "/Images/TemplateImage/" + common.myInt(ViewState["SectionID"]) + "/" + lblImageName.Text;
                            //newBitmap.Dispose();

                            coll.Add(common.myInt(hdnValueId.Value));
                            coll.Add(common.myStr(txtCode.Text, true).Trim());
                            coll.Add(common.myStr(txtOption.Text, true).Trim());
                            coll.Add(common.myStr(txtDefaultMale.Text, true).Trim());
                            coll.Add(common.myStr(txtDefaultFemale.Text, true).Trim());
                            coll.Add(i);
                            coll.Add(common.myInt(ddlStatus.SelectedValue));
                            coll.Add(common.myBool(chkDefault.Checked) ? 1 : 0);
                            coll.Add(common.myStr(sImagePath));
                            coll.Add(common.myStr(lblImageName.Text));
                            coll.Add(DBNull.Value);

                            sXMLValues.Append(common.setXmlTable(ref coll));
                        }
                        catch (Exception Ex)
                        {
                        }
                        finally
                        {
                            _FileUpload.Dispose();
                        }
                        i++;
                    }
                }
            }
            else if (ddlPropertyType.SelectedItem.Value == "B")
            {
                if (chkDefaultYes.Checked)
                    sDefaultValue = "1";
                else
                    sDefaultValue = "0";
                if (ViewState["Field"].ToString() == "NewMainField" || ViewState["Field"].ToString() == "NewMainSubField" || ViewState["Field"].ToString() == "0" || ViewState["Field"] == null)
                {
                    if (txtBooleanYesCode.Text.Trim() != "" || txtBooleanYesMale.Text.Trim() != "" || txtBooleanYesFemale.Text.Trim() != "" || txtBooleanNoCode.Text.Trim() != "" || txtBooleanNoMale.Text.Trim() != "" || txtBooleanNoFemale.Text.Trim() != "")
                    {
                        sXMLValues.Remove(0, sXMLValues.Length);
                        coll = new ArrayList();

                        coll.Add(0);
                        coll.Add(common.myStr(txtBooleanYesCode.Text, true).Trim());
                        coll.Add("Yes");
                        coll.Add(common.myStr(txtBooleanYesMale.Text, true).Trim());
                        coll.Add(common.myStr(txtBooleanYesFemale.Text, true).Trim());
                        coll.Add(1);
                        coll.Add(1);
                        coll.Add(common.myBool(chkDefaultYes.Checked) ? 1 : 0);
                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        sXMLValues.Append(common.setXmlTable(ref coll));



                        coll.Add(0);
                        coll.Add(common.myStr(txtBooleanNoCode.Text, true).Trim());
                        coll.Add("No");
                        coll.Add(common.myStr(txtBooleanNoMale.Text, true).Trim());
                        coll.Add(common.myStr(txtBooleanNoFemale.Text, true).Trim());
                        coll.Add(2);
                        coll.Add(1);
                        coll.Add(common.myBool(chkDefaultNo.Checked) ? 1 : 0);
                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        coll.Add(DBNull.Value);
                        sXMLValues.Append(common.setXmlTable(ref coll));
                    }
                }
                else//for update existing records // for monday see 
                {
                    sXMLValues.Remove(0, sXMLValues.Length);
                    coll = new ArrayList();

                    coll.Add(hdnYes.Value);
                    coll.Add(common.myStr(txtBooleanYesCode.Text, true).Trim());
                    coll.Add("Yes");
                    coll.Add(common.myStr(txtBooleanYesMale.Text, true).Trim());
                    coll.Add(common.myStr(txtBooleanYesFemale.Text, true).Trim());
                    coll.Add(1);
                    coll.Add(1);
                    coll.Add(common.myBool(chkDefaultYes.Checked) ? 1 : 0);
                    coll.Add(DBNull.Value);
                    coll.Add(DBNull.Value);
                    coll.Add(DBNull.Value);
                    sXMLValues.Append(common.setXmlTable(ref coll));


                    coll.Add(hdnNo.Value);
                    coll.Add(common.myStr(txtBooleanNoCode.Text, true).Trim());
                    coll.Add("No");
                    coll.Add(common.myStr(txtBooleanNoMale.Text, true).Trim());
                    coll.Add(common.myStr(txtBooleanNoFemale.Text, true).Trim());
                    coll.Add(2);
                    coll.Add(1);
                    coll.Add(common.myBool(chkDefaultNo.Checked) ? 1 : 0);
                    coll.Add(DBNull.Value);
                    coll.Add(DBNull.Value);
                    coll.Add(DBNull.Value);
                    sXMLValues.Append(common.setXmlTable(ref coll));

                }
            }
            else if (ddlPropertyType.SelectedItem.Value == "O")
            {
                datobject = ddlPatientDataObject.SelectedValue;
            }
            if (ddlPropertyType.SelectedItem.Value == "H")
            {
                if (common.myInt(txtHeadingStart.Text) > common.myInt(txtHeadingEnd.Text))
                {
                    lblFieldMessage.Text = "Heading start can not be greater then heading end !";
                    return;
                }
            }
            else
            {
                sDefaultValue = "";
                //sMaxLength = "";
            }

            if (ddlPropertyGender.SelectedItem.Value == "B")
            {
                sDefaultFemaleText = common.myStr(txtPropertyDefaultFemale.Text, true).Trim();
                sDefaultMaleText = common.myStr(txtPropertyDefaultMale.Text, true).Trim();
            }
            else if (ddlPropertyGender.SelectedItem.Value == "M")
            {
                sDefaultFemaleText = "";
                sDefaultMaleText = common.myStr(txtPropertyDefaultMale.Text, true).Trim();
            }
            else if (ddlPropertyGender.SelectedItem.Value == "F")
            {
                sDefaultFemaleText = common.myStr(txtPropertyDefaultFemale.Text, true).Trim();
                sDefaultMaleText = "";
            }

            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (ViewState["Field"] != null)
            {
                if (common.myStr(ViewState["Field"]) == "NewMainField")//New
                {
                    hshIn.Add("@intParentId", DBNull.Value);
                    hshIn.Add("@MainSub", 0);  // 0-> Main,  New
                }
                else if (common.myStr(ViewState["Field"]) == "NewMainSubField")//Sub-Field
                {
                    if (treeProperties.SelectedValue != "")
                    {
                        ViewState["treevalue"] = treeProperties.SelectedNode.Value.Substring(1);
                        hshIn.Add("@intParentId", common.myInt(treeProperties.SelectedNode.Value.Substring(1)));
                    }
                    else
                    {
                        hshIn.Add("@intParentId", ViewState["treevalue"]);
                    }
                    if (hdnParentFieldType.Value == "B")
                    {
                        hshIn.Add("@intParentValue", (ddlOptions.SelectedItem.Text == "Yes") ? 1 : 0);
                    }
                    else
                    {
                        hshIn.Add("@intParentValue", ddlOptions.SelectedValue);
                    }

                    hshIn.Add("@MainSub", 1);  // 0-> Main, Sub
                }
            }

            hshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            hshIn.Add("@chvCode", common.myStr(txtPropertyCode.Text, true).Trim());
            hshIn.Add("@chvFieldName", bc.ParseQ(txtPropertyName.Text));
            hshIn.Add("@chrFieldType", common.myStr(ddlPropertyType.SelectedItem.Value));
            hshIn.Add("@chrGender", common.myStr(ddlPropertyGender.SelectedItem.Value));
            hshIn.Add("@intDefaultValue", common.myInt(sDefaultValue));
            hshIn.Add("@bitDefaultFormat", common.myInt(sDefaultFormat));
            hshIn.Add("@chvDefaultTextMale", common.myStr(sDefaultMaleText).Trim());
            hshIn.Add("@chvDefaultTextFemale", common.myStr(sDefaultFemaleText).Trim());
            hshIn.Add("@txtContentWordProcessor", common.myStr(sContentWordProcessor).Trim());
            hshIn.Add("@inyMaxLength", common.myInt(sMaxLength));
            hshIn.Add("@intDataObjectId", datobject);
            hshIn.Add("@xmlValues", sXMLValues.ToString());
            hshIn.Add("@intEncodedBy", common.myInt(Session["UserID"]));
            hshIn.Add("@DisplayTitle", common.myBool(chkTitleField.Checked) ? 1 : 0);
            hshIn.Add("@chvFormatName", common.myStr(sFormatName));
            hshIn.Add("@inyHeadingStart", common.myInt(txtHeadingStart.Text));
            hshIn.Add("@inyHeadingEnd", common.myInt(txtHeadingEnd.Text));
            hshIn.Add("@bitIsMandatory", common.myInt(ddlIsMandatory.SelectedValue));
            hshIn.Add("@chrMandatoryType", common.myStr(ddlMandatoryType.SelectedValue));
            //New Code Added-14-2-2014
            hshIn.Add("@IsLinkRequire", common.myInt(ddlLinkRequired.SelectedValue));
            hshIn.Add("@LinkUrl", common.myStr(txtLinkUrl.Text));
            //Code End-14-2-2014
            hshIn.Add("@intStaticTemplateId", ddlStaticTemplate.SelectedValue);
            hshIn.Add("@bitDisplayInNote", rdoDisplayInNote.SelectedValue);
            hshIn.Add("@bitIsSortingOnSequenceNo", common.myBool(chkSortingOnSequenceNo.Checked));
            hshIn.Add("@bitPrintValueLineWise", common.myBool(chkPrintValueLineWise.Checked));
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshIn.Add("@ColumnNosToDisplay", common.myInt(ddlCoumnsPerRow.SelectedValue));

            if (common.myStr(ViewState["Field"]) == "NewMainField"
                || common.myStr(ViewState["Field"]) == "NewMainSubField"
                || common.myStr(ViewState["Field"]) == "0"
                || ViewState["Field"] == null)
            {
                hshIn.Add("@intSectionId", common.myInt(tvCategory.SelectedNode.Value.Substring(1)));
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveTemplateFields", hshIn, hshOut);
            }
            else
            {
                hshIn.Add("@intFormatId", sFormatId);
                hshIn.Add("@intFieldId", common.myInt(ViewState["FieldId"]));
                hshIn.Add("@bitActive", ddlFieldStatus.SelectedValue);
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRUpdateTemplateFields", hshIn, hshOut);
                if (ddlPropertyType.SelectedItem.Value == "W")
                {
                    SqlConnection cn = new SqlConnection(sConString);
                    cn.Open();

                    string sql = "UPDATE EMRTemplateFieldFormats SET FormatText=N'" + sContentWordProcessor + "' WHERE FormatId = " + sFormatId + "";
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cn.Close();
                    sql = string.Empty;
                }
            }
            lblFieldMessage.Text = "Record(s) has been Updated successfully...";

            BindFieldTree();
            ClearPropertiesFields();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            sMaxLength = string.Empty;
            sDefaultValue = string.Empty;
            sDefaultFormat = string.Empty;
            sDefaultMaleText = string.Empty;
            sDefaultFemaleText = string.Empty;
            sContentWordProcessor = string.Empty;
            sFormatName = string.Empty;
            sFormatId = string.Empty;

            sXMLValues = null;
            sbForUpdate = null;
            datobject = null;

            hshIn = null;
            hshOut = null;

            coll = new ArrayList();
        }
    }
    private void ClearPropertiesFields()
    {
        hdnParentFieldType.Value = "";
        hdnYes.Value = "";
        hdnNo.Value = "";
        txtPropertyName.Text = "";
        ddlPropertyType.SelectedIndex = 0;
        ddlPropertyType_OnSelectedIndexChanged(this, null);
        ddlPropertyGender.SelectedIndex = 0;
        txtPropertyCode.Text = "";
        txtPropertyDefaultFemale.Text = "";
        txtPropertyDefaultMale.Text = "";
        txtPropertyDefaultContentWordProcessor.Content = null;
        txtBooleanYesCode.Text = "";
        txtBooleanYesMale.Text = "";
        txtBooleanYesFemale.Text = "";
        txtBooleanNoCode.Text = "";
        txtBooleanNoMale.Text = "";
        txtBooleanNoFemale.Text = "";

        ddlLinkRequired.SelectedValue = "0";
        txtLinkUrl.Text = "";
        trFieldStatus.Visible = false;
        ViewState["FieldID"] = null;
        chkPrintValueLineWise.Checked = false;
    }

    //private void populateTemplateControls()
    //{
    //    // populating the  ddlPropertyTemplateType control
    //    ddlTemplateType.Items.Clear();
    //    strSQL = new StringBuilder();
    //    hshIn = new Hashtable();
    //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
    //    strSQL.Append("select ID, TypeName  from EMRTemplateTypes where HospitalLocationID=@HospitalLocationID order by TypeName");
    //    SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
    //    if (dr.HasRows == true)
    //    {
    //        ddlTemplateType.DataSource = dr;
    //        ddlTemplateType.DataBind();

    //        ddlTemplateType.Items.Insert(0, " [ Select ] ");
    //        ddlTemplateType.Items[0].Value = "0";

    //    }
    //    else
    //    {
    //        ddlTemplateType.Items.Insert(0, " [ Select ] ");
    //        ddlTemplateType.Items[0].Value = "0";
    //    }
    //    dr.Close();
    //}
    //private void populateFieldGrid()
    //{
    //    if (ViewState["SectionID"] != null)
    //    {
    //        strSQL = new StringBuilder();
    //        hshIn = new Hashtable();
    //        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        hshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
    //        hshIn.Add("@intCategoryID", ViewState["SectionID"].ToString());
    //        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateFieldNames", hshIn);
    //if (ds.Tables[0].Rows.Count > 0)
    //{
    //    gvProperties.DataSource = ds;
    //    gvProperties.DataBind();
    //}
    //else
    //{
    //    gvProperties.DataSource = null;
    //    gvProperties.DataBind();
    //}
    //}
    //else
    //{
    //    gvProperties.DataSource = null;
    //    gvProperties.DataBind();
    //}
    //}
    //protected void gvTemplates_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType != DataControlRowType.Pager)
    //    {
    //        e.Row.Cells[Convert.ToByte(GridTemplate.TemplateId)].Visible = false;
    //        e.Row.Cells[Convert.ToByte(GridTemplate.Status)].Visible = false;
    //        e.Row.Cells[Convert.ToByte(GridTemplate.TemplateTypeID)].Visible = false;
    //        e.Row.Cells[Convert.ToByte(GridTemplate.DisplayTitle)].Visible = false;
    //    }
    //    if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
    //    {
    //        LinkButton lblName = (LinkButton)e.Row.FindControl("lnkTname");
    //        Label lblEncBy = (Label)e.Row.FindControl("lblSpecialisation");

    //        //if (lblEncBy.Text.Length > 10)
    //        //    lblEncBy.Text = lblEncBy.Text.PadRight(35).Substring(0, 35) + "...";
    //        //else
    //        //    lblEncBy.Text = lblEncBy.Text.PadRight(35).Substring(0, 35);

    //        ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
    //        string jQuery = "var color=$get('" + e.Row.ClientID + "').style.backgroundColor;"
    //            + "$get('" + e.Row.ClientID + "').style.backgroundColor='Pink';"
    //            + "if(!confirm('Are you sure you want to De-Activate \\'" + lblName.Text + "\\' Template?'))"
    //            + "{$get('" + e.Row.ClientID + "').style.backgroundColor=color; return false;}";
    //        img.Attributes.Add("onclick", jQuery);
    //        if (e.Row.Cells[Convert.ToByte(GridTemplate.Status)].Text.ToString().Trim() == "0")
    //        {
    //            img.Enabled = false;
    //        }
    //    }
    //    if (e.Row.RowState == DataControlRowState.Edit)
    //    {
    //        DropDownList ddlTStatus = (DropDownList)e.Row.FindControl("ddlTStatus");
    //        String strStatus = e.Row.Cells[Convert.ToByte(GridTemplate.Status)].Text.ToString().Trim();

    //        if (strStatus == "0")
    //        {
    //            ddlTStatus.Enabled = true;
    //        }
    //        else
    //        {
    //            ddlTStatus.Enabled = false;
    //        }
    //    }

    //    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
    //    {

    //        DropDownList ddlTStatus = (DropDownList)e.Row.FindControl("ddlTStatus");
    //        Label lblName2 = (Label)e.Row.FindControl("LName2");
    //        String strStatus = e.Row.Cells[Convert.ToByte(GridTemplate.Status)].Text.ToString().Trim();

    //        if (strStatus == "0")
    //        {
    //            ddlTStatus.Visible = true;
    //            lblName2.Visible = false;
    //        }
    //        else
    //        {
    //            ddlTStatus.Visible = false;
    //            lblName2.Visible = true;

    //        }
    //    }
    //}
    //protected void gvTemplates_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvTemplates.PageIndex = e.NewPageIndex;
    //    BindTemplateGrid();
    //}
    //protected void gvTemplates_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    gvTemplates.EditIndex = e.NewEditIndex;
    //    BindTemplateGrid();
    //    lblTemplateMessage.Text = "";
    //}
    //protected void gvTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    ViewState["TemplateTypeID"] = null;
    //    if (e.CommandName == "EDT")
    //    {
    //        foreach (GridViewRow gRow in gvTemplates.Rows)
    //        {
    //            LinkButton lnkTemplateId = gRow.FindControl("lnkEdit") as LinkButton;
    //            if (common.myInt(lnkTemplateId.CommandArgument) == common.myInt(e.CommandArgument.ToString()))
    //            {
    //                ViewState["TemplateId"] = e.CommandArgument;
    //                LinkButton lnkTemplateName = gRow.FindControl("lnkTname") as LinkButton;
    //                String TemplateType = gRow.Cells[Convert.ToByte(GridTemplate.Type)].Text;
    //                String TemplateCode = gRow.Cells[Convert.ToByte(GridTemplate.Code)].Text;
    //                Label lblSpecialisation = gRow.FindControl("lblSpecialisation") as Label;
    //                Label lblStatus = gRow.FindControl("lblStatus") as Label;
    //                txtTemplateName.Text = Server.HtmlDecode(lnkTemplateName.Text).Trim();
    //                ddlSpecialisation.ClearSelection();
    //                ddlTemplateType.ClearSelection();
    //                ddlStatus.ClearSelection();
    //                if (lblSpecialisation.Text.Trim() != "")
    //                    ddlSpecialisation.Items[ddlSpecialisation.Items.IndexOf(ddlSpecialisation.Items.FindByText(lblSpecialisation.Text.Trim()))].Selected = true;
    //                else
    //                    ddlSpecialisation.SelectedIndex = 0;
    //                ddlTemplateType.Items[ddlTemplateType.Items.IndexOf(ddlTemplateType.Items.FindByText(TemplateType))].Selected = true;
    //                ddlStatus.Items[ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(lblStatus.Text))].Selected = true;
    //                txtAssignCode.Text = Server.HtmlDecode(TemplateCode).Trim();
    //                string TitleNode = Server.HtmlDecode(gRow.Cells[Convert.ToByte(GridTemplate.DisplayTitle)].Text).Trim();
    //                chkTitleNote.Checked = common.myBool((TitleNode == "" || TitleNode == "False") ? 0 : 1);


    //                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //                hshIn = new Hashtable();
    //                hshIn.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
    //                hshIn.Add("@intTemplateId", e.CommandArgument.ToString());
    //                DataSet ds = dl.FillDataSet(CommandType.Text, "Select * from EMRTemplate where Id=@intTemplateId And HospitalLocationId=@inyHospitalLocationId", hshIn);
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    DataRow dr = ds.Tables[0].Rows[0] as DataRow;
    //                    if (dr["SectionsBold"].ToString() == "True") { chkBoldSections.Checked = true; } else { chkBoldSections.Checked = false; };
    //                    if (dr["SectionsItalic"].ToString() == "True") { chkItalicSections.Checked = true; } else { chkItalicSections.Checked = false; };
    //                    if (dr["SectionsUnderline"].ToString() == "True") { chkUnderlineSections.Checked = true; } else { chkUnderlineSections.Checked = false; };
    //                    if (dr["SectionsForecolor"].ToString() == "")
    //                    {
    //                        txtColorSections.BackColor = System.Drawing.Color.White;
    //                    }
    //                    else
    //                    {
    //                        txtColorSections.BackColor = System.Drawing.Color.FromName("#" + dr["SectionsForecolor"].ToString()); 
    //                        //txtColorSections.BackColor = System.Drawing.Color.Red;
    //                        ajax1.SelectedColor = "#" + dr["SectionsForecolor"].ToString();
    //                        btnForeColorSections.BackColor = System.Drawing.Color.FromName("#" + dr["SectionsForecolor"].ToString());
    //                        ajax1.TargetControlID = "txtColorSections";
    //                        ajax1.PopupButtonID = "btn1";
    //                        ajax1.SampleControlID = "btnForeColorSections";
    //                        //ajax1.
    //                        //lblColor.BackColor = System.Drawing.Color.FromName("#" + dr["SectionsForecolor"].ToString()); 
    //                        //ScriptManager.RegisterStartupScript(Page, GetType(), "scrip12", "$get('" + txtColorSections.ClientID + "').style.backgroundColor ='#" + dr["SectionsForecolor"].ToString() + "';", true);
    //                    }
    //                    if (dr["SectionsListStyle"].ToString() == "") { ddlListStyleSections.SelectedIndex = 0; } else { ddlListStyleSections.SelectedValue = dr["SectionsListStyle"].ToString(); }
    //                    if (dr["SectionsFontStyle"].ToString() == "") { ddlFontTypeSections.SelectedIndex = 0; } else { ddlFontTypeSections.SelectedValue = dr["SectionsFontStyle"].ToString(); }
    //                    if (dr["SectionsFontSize"].ToString() == "") { ddlFontSizeSections.SelectedIndex = 0; } else { ddlFontSizeSections.SelectedValue = dr["SectionsFontSize"].ToString(); }

    //                    if (dr["FieldsBold"].ToString() == "True") { chkBoldFields.Checked = true; } else { chkBoldFields.Checked = false; };
    //                    if (dr["FieldsItalic"].ToString() == "True") { chkItalicFields.Checked = true; } else { chkItalicFields.Checked = false; };
    //                    if (dr["FieldsUnderline"].ToString() == "True") { chkUnderlineFields.Checked = true; } else { chkUnderlineFields.Checked = false; };
    //                    if (dr["FieldsForecolor"].ToString() == "") { txtColorFields.BackColor = System.Drawing.Color.White; } else {
    //                        txtColorFields.BackColor = System.Drawing.Color.FromName("#" + dr["FieldsForecolor"].ToString());
    //                        ColorPickerExtender1.SelectedColor = "#" + dr["FieldsForecolor"].ToString();
    //                    }
    //                    if (dr["FieldsListStyle"].ToString() == "") { ddlListStyleFields.SelectedIndex = 0; } else { ddlListStyleFields.SelectedValue = dr["FieldsListStyle"].ToString(); }
    //                    if (dr["FieldsFontStyle"].ToString() == "") { ddlFontTypeFields.SelectedIndex = 0; } else { ddlFontTypeFields.SelectedValue = dr["FieldsFontStyle"].ToString(); }
    //                    if (dr["FieldsFontSize"].ToString() == "") { ddlFontSizeFields.SelectedIndex = 0; } else { ddlFontSizeFields.SelectedValue = dr["FieldsFontSize"].ToString(); }
    //                }
    //                ScriptManager.RegisterStartupScript(Page, GetType(), "sc1", "$get('" + gRow.ClientID + "').style.backgroundColor='pink';", true);
    //                ViewState["TemplateEdit"] = "TemplateEdit";
    //                lblStatusT.Visible = true;
    //                ddlStatus.Visible = true;
    //                //break;
    //            }
    //            LinkButton lblName = (LinkButton)gRow.FindControl("lnkTname");
    //            ImageButton img = (ImageButton)gRow.FindControl("ibtnDelete");
    //            string jQuery = "var color=$get('" + gRow.ClientID + "').style.backgroundColor;"
    //                + "$get('" + gRow.ClientID + "').style.backgroundColor='Pink';"
    //                + "if(!confirm('Are you sure you want to De-Activate \\'" + lblName.Text + "\\' Template?'))"
    //                + "{$get('" + gRow.ClientID + "').style.backgroundColor=color; return false;}";
    //            img.Attributes.Add("onclick", jQuery);
    //        }
    //    }
    //    else if (e.CommandName == "Del")
    //    {
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        hshIn = new Hashtable();
    //        hshIn.Add("TID", e.CommandArgument);
    //        int i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE EMRTemplate set Active = 0 WHERE ID = @TID", hshIn);
    //        if (i == 0)
    //        {
    //            lblTemplateMessage.Text = "Template has been DeActivated successfully.";
    //            //Alert.ShowAjaxMsg("Template DeActivated!", this.Page);
    //            gvTemplates.EditIndex = -1;
    //            BindTemplateGrid();
    //        }
    //    }
    //    else if (e.CommandName == "Select")
    //    {
    //        btnBack.Visible = true;
    //        btnNew.Visible = true;
    //        btnTemplateNew.Visible = false;
    //        foreach (GridViewRow gvr in gvTemplates.Rows)
    //        {
    //            if (e.CommandArgument.ToString().Trim() == gvr.Cells[Convert.ToByte(GridTemplate.SerialNo)].Text.ToString().Trim())
    //            {
    //                String TemplateID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridTemplate.TemplateId)].Text.ToString().Trim());
    //                LinkButton TemplateName = (LinkButton)gvr.FindControl("lnkTname");
    //                String TemplateTypeID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridTemplate.TemplateTypeID)].Text.ToString().Trim());
    //                lblTemplateName.Text = TemplateName.Text.ToString().Trim();
    //                //lblTemplateName.Width = TemplateName.Text.Length + 100;
    //                //lblTemplateName
    //                ViewState["Category"] = "Category";
    //                ViewState["TemplateID"] = TemplateID.ToString().Trim();
    //                ViewState["TemplateTypeID"] = TemplateTypeID.ToString().Trim();
    //                mltVW.SetActiveView(vwCategory);
    //                BindCategoryTree();
    //                pnlNewEditCategorySubCategory.Visible = false;
    //                pnlNewEditField.Visible = true;
    //                ddlPropertyType_OnSelectedIndexChanged(sender, e);
    //            }
    //        }
    //        ibtnTemplateSave.Visible = false;
    //        ibtnCategorySave.Visible = false;
    //        ibtnProtertySave.Visible = true;
    //        lblStatusT.Visible = false;
    //        ddlStatus.Visible = false;
    //        trFieldStatus.Visible = false;
    //        lbtnEditCategory_OnClick(this, null);
    //    }
    //    //if ((e.CommandName != "Edit") && (e.CommandName != "Del"))
    //    //{
    //    //    lblTemplateName.Text = e.CommandName.ToString().Trim().ToUpper();
    //    //    e.
    //    //    ViewState["Category"] = "Category";
    //    //    mltVW.SetActiveView(vwCategory);
    //    //    BindCategoryTree();
    //    //    pnlNewEditCategorySubCategory.Visible = false;
    //    //    pnlNewEditField.Visible = true;
    //    //    ddlPropertyType_OnSelectedIndexChanged(sender, e);
    //    //}
    //}
    //protected void gvTemplates_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    gvTemplates.EditIndex = -1;
    //    BindTemplateGrid();
    //}
    //protected void gvTemplates_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hshIn = new Hashtable();
    //    GridViewRow row = gvTemplates.Rows[e.RowIndex];

    //    string ID = gvTemplates.Rows[e.RowIndex].Cells[Convert.ToByte(GridTemplate.TemplateId)].Text;
    //    string Name = ((TextBox)gvTemplates.Rows[e.RowIndex].Cells[Convert.ToByte(GridTemplate.TemplateName)].Controls[1]).Text;
    //    String Code = ((TextBox)gvTemplates.Rows[e.RowIndex].Cells[Convert.ToByte(GridTemplate.Code)].FindControl("txtCodeTemplateGrid")).Text.ToString().Trim();
    //    string strStatus = gvTemplates.Rows[e.RowIndex].Cells[Convert.ToByte(GridTemplate.Status)].Text;
    //    String strActive = "";
    //    if (strStatus == "0")
    //    {
    //        DropDownList strDrop = ((DropDownList)gvTemplates.Rows[e.RowIndex].Cells[Convert.ToByte(GridTemplate.Active)].FindControl("ddlTStatus"));
    //        strActive = strDrop.SelectedItem.Value;
    //    }
    //    else if (strStatus == "1")
    //    {
    //        strActive = "1";
    //    }
    //    hshIn.Add("@ID", ID);
    //    hshIn.Add("@Name", bc.ParseQ(Name));
    //    hshIn.Add("@Code", bc.ParseQ(Code));
    //    hshIn.Add("@Active", bc.ParseQ(strActive));
    //    hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));

    //    int i = dl.ExecuteNonQuery(CommandType.Text, "UPDATE EMRTemplate SET Templatename = @Name,Active=@Active,Code=@Code WHERE ID = @ID and Hospitallocationid=@HospitalLocationID", hshIn);
    //    if (i == 0)
    //    {
    //        lblTemplateMessage.Text = "Record has been updated successfully.";
    //        //Alert.ShowAjaxMsg("Template Updated!", this.Page);
    //        gvTemplates.EditIndex = -1;
    //        BindTemplateGrid();
    //    }
    //}

    protected void gvProperties_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[Convert.ToByte(GridProperty.Propertyid)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridProperty.Type)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridProperty.SerialNo)].Visible = false;

        }
        //if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Attributes["ondblclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvProperties, "Select$" + e.Row.RowIndex);
        //    Label lblName = (Label)e.Row.FindControl("LName");
        //}
    }

    //protected void gvProperties_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    //{
    //    gvProperties.PageIndex = e.NewPageIndex;
    //    populateFieldGrid();
    //}
    //protected void gvProperties_RowEditing(Object sender, GridViewEditEventArgs e)
    //{
    //    gvProperties.EditIndex = e.NewEditIndex;
    //    populateFieldGrid();
    //}
    //protected void gvProperties_RowCommand(Object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Del")
    //    {
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

    //        hshIn = new Hashtable();
    //        hshIn.Add("PID", e.CommandArgument);

    //        int i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE EMRTemplateProperties set Active = 0 WHERE Propertyid = @PID", hshIn);
    //        if (i == 0)
    //        {
    //            Alert.ShowAjaxMsg("Property De-Activated!", this.Page);
    //            gvProperties.EditIndex = -1;
    //            populateFieldGrid();
    //        }
    //    }
    //}
    //protected void gvProperties_RowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    //{
    //    gvProperties.EditIndex = -1;
    //    populateFieldGrid();
    //}
    //protected void gvProperties_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    //{
    //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

    //    hshIn = new Hashtable();
    //    GridViewRow row = gvProperties.Rows[e.RowIndex];

    //    string ID = gvProperties.Rows[e.RowIndex].Cells[Convert.ToByte(GridProperty.Propertyid)].Text;
    //    string Name = ((TextBox)gvProperties.Rows[e.RowIndex].Cells[Convert.ToByte(GridProperty.PropertyName)].Controls[1]).Text;
    //    string strStatus = gvProperties.Rows[e.RowIndex].Cells[Convert.ToByte(GridProperty.Status)].Text;

    //    DropDownList ddlGender = (DropDownList)gvProperties.Rows[e.RowIndex].Cells[Convert.ToByte(GridProperty.Gender)].FindControl("ddlPropertyGenderGrid");
    //    String strActive = "";
    //    if (strStatus == "0")
    //    {
    //        DropDownList strDrop = ((DropDownList)gvProperties.Rows[e.RowIndex].Cells[Convert.ToByte(GridProperty.Active)].FindControl("ddlTStatus"));
    //        strActive = strDrop.SelectedItem.Value;
    //    }
    //    else if (strStatus == "1")
    //    {
    //        strActive = "1";
    //    }
    //    DropDownList ddlPropType = ((DropDownList)gvProperties.Rows[e.RowIndex].Cells[Convert.ToByte(GridProperty.Type)].FindControl("ddlPropType"));

    //    // Code for newly added Def val and Max length
    //    String sMaxLength = "";
    //    String sDefValue = "";
    //    TextBox txtDefVal = (TextBox)gvProperties.Rows[e.RowIndex].FindControl("txtDefaultGrid");
    //    DropDownList ddlBoolGrid = (DropDownList)gvProperties.Rows[e.RowIndex].FindControl("ddlDefaultBooleanGrid");
    //    DropDownList ddlDropCheckGrid = (DropDownList)gvProperties.Rows[e.RowIndex].FindControl("ddlDefaultDropCheckGrid");
    //    Label lblPType = (Label)gvProperties.Rows[e.RowIndex].FindControl("lblPtypeEditGrid");
    //    TextBox txtMaxLengthGrid = (TextBox)gvProperties.Rows[e.RowIndex].FindControl("txtMaxLengthGrid");

    //    if ((lblPType.Text == "T") || (lblPType.Text == "M"))
    //    {
    //        if (txtMaxLengthGrid.Text.ToString().Trim().Length == 0)
    //        {
    //            if (lblPType.Text == "T")
    //            {
    //                sMaxLength = "40";
    //            }
    //            else if (lblPType.Text == "M")
    //            {
    //                sMaxLength = "";
    //            }
    //        }
    //        else
    //        {
    //            sMaxLength = bc.ParseQ(txtMaxLengthGrid.Text.ToString().Trim());
    //        }
    //        sDefValue = txtDefVal.Text.ToString().Trim();
    //    }
    //    else if (lblPType.Text == "B")
    //    {
    //        sMaxLength = "";
    //        sDefValue = ddlBoolGrid.SelectedItem.Value;
    //    }
    //    else
    //    {
    //        sMaxLength = "";
    //        sDefValue = ddlDropCheckGrid.SelectedItem.Value;
    //    }
    //    // End Code for newly added Def val and Max length

    //    hshIn.Add("@ID", ID);
    //    hshIn.Add("@Name", bc.ParseQ(Name));
    //    hshIn.Add("@Active", bc.ParseQ(strActive));
    //    hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
    //    //hshIn.Add("@PType", bc.ParseQ(ddlPropType.SelectedItem.Value));
    //    hshIn.Add("@Gender", bc.ParseQ(ddlGender.SelectedItem.Value));
    //    hshIn.Add("@MaxLength", bc.ParseQ(sMaxLength));
    //    hshIn.Add("@DefValue", bc.ParseQ(sDefValue));

    //    int i = dl.ExecuteNonQuery(CommandType.Text, "UPDATE EMRTemplateproperties SET propertyname = @Name,Active=@Active,Gender=@Gender,DefaultValue=@DefValue,MaxLength=@MaxLength WHERE Propertyid = @ID and Hospitallocationid=@HospitalLocationID", hshIn);
    //    if (i == 0)
    //    {
    //        Alert.ShowAjaxMsg("Property Updated!", this.Page);
    //        gvProperties.EditIndex = -1;
    //        populateFieldGrid();
    //    }
    //}

    protected void gvValue_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[8].Visible = false;
        }
        //e.Row.Cells[10].Visible = false;
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        {
            Label lblName = (Label)e.Row.FindControl("LName");
            //lblName.Attributes.Add("onmouseover", "this.style.cursor='hand'");
            //lblName.Attributes.Add("onclick", "javascript:window.showModalDialog('/EMR/Masters/TemplateCategory.aspx?TID=" + e.Row.Cells[0].Text.ToString().Trim() + "&TN=" + lblName.Text.ToString().Trim() + "','mywindow', 'menubar=0,resizable=0,width=490,height=250,status=0,toolbars=0');");

            ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
            img.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to De-Activate this record :  " + lblName.Text + "')");
            if (e.Row.Cells[8].Text.ToString().Trim() == "0")
            {
                img.Enabled = false;
            }
        }
        if (e.Row.RowState == DataControlRowState.Edit)
        {
            DropDownList ddlTStatus = (DropDownList)e.Row.FindControl("ddlTStatus");
            String strStatus = e.Row.Cells[8].Text.ToString().Trim();

            if (strStatus == "0")
            {
                ddlTStatus.Enabled = true;
            }
            else
            {
                ddlTStatus.Enabled = false;
            }
        }

        if ((e.Row.RowState & DataControlRowState.Edit) > 0)
        {

            DropDownList ddlTStatus = (DropDownList)e.Row.FindControl("ddlTStatus");
            Label lblName2 = (Label)e.Row.FindControl("LName2");
            String strStatus = e.Row.Cells[8].Text.ToString().Trim();

            if (strStatus == "0")
            {
                ddlTStatus.Visible = true;
                lblName2.Visible = false;
            }
            else
            {
                ddlTStatus.Visible = false;
                lblName2.Visible = true;
            }
        }
    }

    protected void BindFieldTree()
    {
        try
        {
            //pnlNewEditCategorySubCategory.Visible = false;
            treeProperties.Nodes.Clear();
            hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            hshIn.Add("@intCategoryID", ViewState["SectionID"].ToString());
            //Session["SectionId"] = ViewState["SectionID"];

            btnFields.Attributes.Add("onclick", " return openRadWindow('fld', '" + ViewState["TemplateID"].ToString() + "','" + ViewState["SectionID"].ToString() + "')");

            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateFieldNames", hshIn);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                AddNodes(treeProperties, common.myInt(ds.Tables[0].Rows[i]["FieldId"]), common.myInt(ds.Tables[0].Rows[i]["ParentId"]), common.myStr(ds.Tables[0].Rows[i]["FieldName"]), ds.Tables[0].Rows[i]["FieldType"].ToString());
            }
            if (treeProperties.Nodes.Count > 0)
            {
                //treeProperties.Nodes[0].Selected = true;
                treeProperties.CollapseAll();
                treeProperties.PopulateNodesFromClient = true;
                treeProperties.ShowLines = true;
                //TreeProperties_OnNodeSelected(this, null);
            }
            else
            {
                //lbtnNewCategory_OnClick(this, null);
            }

            ddlIsMandatory.SelectedValue = "0";
            ddlMandatoryType.SelectedIndex = 0;
            ddlMandatoryType.Enabled = false;
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindCategoryTree()
    {
        try
        {
            pnlNewEditCategorySubCategory.Visible = false;
            tvCategory.Nodes.Clear();
            hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            hshIn.Add("@intTemplateId", ViewState["TemplateID"]);

            //hshIn.Add("@inyTemplateTypeID", ViewState["TemplateTypeID"].ToString()); //  Comment By Chandan According To Store Procedure.
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTree", hshIn);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                AddNodes(tvCategory, common.myInt(ds.Tables[0].Rows[i]["SectionId"]), common.myInt(ds.Tables[0].Rows[i]["ParentId"]), common.myStr(ds.Tables[0].Rows[i]["SectionName"]), "aaa");
            }
            if (tvCategory.Nodes.Count > 0)
            {
                tvCategory.Nodes[0].Selected = true;
                tvCategory.CollapseAll();
                tvCategory.PopulateNodesFromClient = true;
                tvCategory.ShowLines = true;
                tvCategory_OnSelectedNodeChanged(this, null);
            }
            else
            {
                lbtnNewCategory_OnClick(this, null);
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void AddNodes(TreeView tvName, Int32 iNodeID, Int32 iParentID, String sNodeText, String DataType)
    {
        try
        {
            if (iParentID == 0)
            {
                TreeNode masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", DataType);
                tvName.Nodes.Add(masternode);
            }
            else
            {
                TreeNode masternode = new TreeNode();
                masternode = tvName.FindNode("P" + iParentID.ToString());
                if (masternode != null)
                {
                    TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", "", DataType);
                    masternode.ChildNodes.Add(childNode);
                }
                else
                {
                    CallRecursive(tvName, iNodeID, "C" + iParentID, sNodeText);
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void CallRecursive(TreeView tvName, Int32 iNodeID, String sParentID, String sNodeText)
    {
        TreeNodeCollection nodes = tvName.Nodes;
        foreach (TreeNode n in nodes)
        {
            ReCallRecursive(n, iNodeID, sParentID, sNodeText);
        }
    }

    private void ReCallRecursive(TreeNode treeNode, Int32 iNodeID, String sParentID, String sNodeText)
    {
        foreach (TreeNode tn in treeNode.ChildNodes)
        {
            if (tn.Value == sParentID.ToString())
            {
                TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString());
                tn.ChildNodes.Add(childNode);
            }
            ReCallRecursive(tn, iNodeID, sParentID, sNodeText);
        }
    }

    //protected void btnNew_OnClick(object sender, EventArgs e)
    //{
    //    ClearPropertiesFields();
    //    ViewState["FieldId"] = null;
    //}

    protected void lbtnNewCategory_OnClick(Object sender, EventArgs e)
    {
        trHeaderCategory.Visible = false;
        ibtnCategorySave.Text = "Save";
        ibtnCategorySave.ToolTip = "Save Section";
        ViewState["Category"] = "NewMainCategory";
        pnlNewEditCategorySubCategory.Visible = true;
        pnlNewEditField.Visible = false;
        hdnMenuId.Value = lblSelectedCategoryID.Text;
        ltrlCategoryStatus.Visible = false;
        ddlCategoryStatus.Visible = false;
        //lblSelectedCategory.Text = "New Section";
        lblFieldName.Text = "New Section";
        txtCategoryName.Text = "";

        ibtnCategorySave.Visible = true;
        ibtnProtertySave.Visible = false;
    }

    protected void lbtnNewCategorySub_OnClick(Object sender, EventArgs e)
    {
        if (CheckSubSectionDataTabular() > 0)
        {
            pnlNewEditCategorySubCategory.Visible = false;
            pnlNewEditValues.Visible = false;//by rk

            lblFieldMessage.Text = "Now you can't create sub section because section is already created in tabular form.";
            lblFieldMessage.ForeColor = Color.Red;
            // return;
        }
        else
        {
            pnlNewEditCategorySubCategory.Visible = true;
        }

        trHeaderCategory.Visible = true;
        lblParentNode.Text = tvCategory.SelectedNode.Text;
        ibtnCategorySave.Text = "Save";
        ibtnCategorySave.ToolTip = "Save Sub-Section";
        ViewState["Category"] = "NewMainCategorySub";
        // pnlNewEditCategorySubCategory.Visible = true;
        pnlNewEditField.Visible = false;

        hdnMenuId.Value = lblSelectedCategoryID.Text;
        txtCategoryName.Text = "";
        txtSectionCode.Text = "";
        rbtnNoTabular.Checked = true;
        rbtnYesTabular.Checked = false;
        ltrlCategoryStatus.Visible = false;
        ddlCategoryStatus.Visible = false;
        if (tvCategory.SelectedNode != null)
        {
            //lblSelectedCategory.Text = tvCategory.SelectedNode.Text;
            //lblFieldName.Text = tvCategory.SelectedNode.Text; 
            lblFieldName.Text = "New Sub-Section";
        }

        ibtnCategorySave.Visible = true;
        ibtnProtertySave.Visible = false;
    }

    protected void lbtnEditCategory_OnClick(Object sender, EventArgs e)
    {
        trHeaderCategory.Visible = false;
        ibtnCategorySave.Text = "Save";
        ibtnCategorySave.ToolTip = "Update Section";
        ViewState["Category"] = "EditMainCategory";
        pnlNewEditCategorySubCategory.Visible = true;
        pnlNewEditField.Visible = false;
        hdnMenuId.Value = lblSelectedCategoryID.Text;
        hdnMenuId2.Value = lblSelectedFieldID.Text;

        pnlValueDropDown.Visible = false;
        pnlNewEditValues.Visible = false;//by rk

        PopulateSelectedCategoryData();
        ltrlCategoryStatus.Visible = true;
        ddlCategoryStatus.Visible = true;

        if (tvCategory.SelectedNode != null)
        {
            lblFieldName.Text = tvCategory.SelectedNode.Text;
        }
        ibtnCategorySave.Visible = true;
        ibtnProtertySave.Visible = false;
    }

    protected void lbtnSubNewCategory_OnClick(Object sender, EventArgs e)
    {
        ibtnCategorySave.Text = "Save";
        ibtnCategorySave.ToolTip = "Save Section";
        ViewState["Category"] = "NewSubNewCategory";
        pnlNewEditCategorySubCategory.Visible = true;
        pnlNewEditField.Visible = false;
        hdnMenuId.Value = lblSelectedCategoryID.Text;
        hdnMenuId2.Value = lblSelectedFieldID.Text;
        ltrlCategoryStatus.Visible = false;
        ddlCategoryStatus.Visible = false;

        ibtnCategorySave.Visible = true;
        ibtnProtertySave.Visible = false;
    }

    protected void lbtnNewSubCategory_OnClick(Object sender, EventArgs e)
    {
        ibtnCategorySave.Text = "Save";
        ibtnCategorySave.ToolTip = "Save Sub-Section";
        ViewState["Category"] = "NewSubCategory";
        pnlNewEditCategorySubCategory.Visible = true;
        pnlNewEditField.Visible = false;
        hdnMenuId.Value = lblSelectedCategoryID.Text;
        hdnMenuId2.Value = lblSelectedFieldID.Text;
        ltrlCategoryStatus.Visible = false;
        ddlCategoryStatus.Visible = false;

        ibtnCategorySave.Visible = true;
        ibtnProtertySave.Visible = false;
    }

    protected void lbtnEditSubCategory_OnClick(Object sender, EventArgs e)
    {
        ibtnCategorySave.Text = "Save";
        ibtnCategorySave.ToolTip = "Update Sub-Section";
        ViewState["Category"] = "EditSubCategory";
        pnlNewEditCategorySubCategory.Visible = true;
        pnlNewEditField.Visible = false;
        hdnMenuId.Value = lblSelectedCategoryID.Text;
        hdnMenuId2.Value = lblSelectedFieldID.Text;
        PopulateSelectedCategoryData();
        ltrlCategoryStatus.Visible = true;
        ddlCategoryStatus.Visible = true;

        ibtnCategorySave.Visible = true;
        ibtnProtertySave.Visible = false;
    }

    private void PopulateSelectedFieldData()
    {
        try
        {
            // populating the  controls with respect to selected category
            if (lblSelectedFieldID.Text != "")
            {
                String strSelectedID = lblSelectedFieldID.Text.Substring(1);

                strSQL = new StringBuilder();
                hshIn = new Hashtable();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshIn.Add("@FieldID", bc.ParseQ(strSelectedID));
                strSQL.Append("select FieldID, FieldName,Gender,Convert(int,Active) Active,ISNULL(Code,'') as Code from EMRTemplateFields where FieldID = @FieldID");
                SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
                if (dr.HasRows == true)
                {
                    if (dr.Read())
                    {
                        txtCategoryName.Text = dr["FieldName"].ToString();
                        ddlCategoryGender.SelectedValue = dr["Gender"].ToString();
                        ddlCategoryStatus.SelectedValue = dr["Active"].ToString();
                        txtSectionCode.Text = dr["Code"].ToString();
                    }
                }
                else
                {
                    ClearCategoryFields();
                }
                dr.Close();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void visibleIfTabular()
    {
        lblMaxLength.Visible = false;
        txtMaxLength.Visible = false;
        trMale.Visible = false;
        trFemale.Visible = false;
        trWordProcessor.Visible = false;
        ddlPatientDataObject.Visible = false;
        lblDataObject.Visible = false;
        pnlNewEditValues.Visible = true;
        pnlValueBoolean.Visible = false;
        pnlValueDropDown.Visible = true;
        trFieldFormats.Visible = false;
        lblValueDropDown.Text = "Row Type - Text";

        BindBlankValueDropDownGrid();
    }

    private void PopulateSelectedCategoryData()
    {
        try
        {
            // populating the  controls with respect to selected category
            if (lblSelectedCategoryID.Text != "")
            {
                String strSelectedID = lblSelectedCategoryID.Text.Substring(1);

                hshIn = new Hashtable();
                hshIn.Add("@HospitalLocationID", common.myInt(Session["HospitalLocationID"]));
                hshIn.Add("@CategoryID", common.myInt(strSelectedID));

                strSQL = new StringBuilder();
                strSQL.Append("SELECT SectionId, SectionName, Gender, CONVERT(INT, Active) AS Active, ISNULL(Code, '') AS Code, Tabular, DisplayTitle, TRows, NoOfBlankRows, IsFreezeFirstColumn,IsAddendum FROM EMRTemplateSections WHERE SectionId = @CategoryID");

                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);

                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        txtCategoryName.Text = common.myStr(dr["SectionName"]);
                        ddlCategoryGender.SelectedValue = common.myStr(dr["Gender"]);
                        ddlCategoryStatus.SelectedValue = common.myStr(dr["Active"]);
                        txtSectionCode.Text = common.myStr(dr["Code"]);
                        chkSectionTitle.Checked = common.myBool(dr["DisplayTitle"]);
                        hdntabularcheck.Value = common.myStr(dr["Tabular"]);
                        rdoAddendum.SelectedValue = common.myStr(dr["IsAddendum"]);
                        if (common.myBool(dr["Tabular"]))
                        {
                            rbtnYesTabular.Checked = true;
                            rbtnNoTabular.Checked = false;

                            trRows.Visible = true;
                            txtRows.Text = common.myStr(dr["TRows"]);

                            trBlankRows.Visible = true;
                            trIsFreezeFirstColumn.Visible = true;
                            btnSetFormula.Visible = true;
                            btnSetScoreFormula.Visible = false;

                            txtBlankRows.Text = common.myStr(dr["NoOfBlankRows"]);
                            rdoIsFreezeFirstColumn.SelectedIndex = rdoIsFreezeFirstColumn.Items.IndexOf(rdoIsFreezeFirstColumn.Items.FindByValue(common.myBool(dr["IsFreezeFirstColumn"]) ? "1" : "0"));

                            visibleIfTabular();

                            DataSet ds = new DataSet();
                            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                            string qry = "SELECT ValueId, SectionId AS FieldId, Code, ValueName, DefaultTextMale, DefaultTextFemale, " +
                                        " SequenceNo, Active, EncodedBy, EncodedDate," +
                                        " CASE Active  WHEN 1 Then 'Active' WHEN 0 Then 'In-Active' END AS 'ActiveText', DefaultValue,'' ImagePath,'' ImageFileName, NULL ScoreValue " +
                                        " FROM EMRTemplateRows" +
                                        " WHERE SectionId = @CategoryID " +
                                        " ORDER BY SequenceNo";

                            ds = objDl.FillDataSet(CommandType.Text, qry, hshIn);

                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());

                                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                            }

                            gvValueDropDown.DataSource = ds.Tables[0];
                            gvValueDropDown.DataBind();
                        }
                        else
                        {
                            rbtnYesTabular.Checked = false;
                            rbtnNoTabular.Checked = true;

                            trRows.Visible = false;
                            txtRows.Text = "";

                            trBlankRows.Visible = false;
                            trIsFreezeFirstColumn.Visible = false;
                            btnSetFormula.Visible = false;
                            btnSetScoreFormula.Visible = true;

                            txtBlankRows.Text = "";
                            rdoIsFreezeFirstColumn.SelectedValue = "0";
                        }
                    }
                }
                else
                {
                    ClearCategoryFields();
                }
                dr.Close();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlCategoryTemplateType_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        ViewState["CategoryTemplateType"] = "3";
        BindCategoryTree();
    }

    protected void ddlPropertyType_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        if (ddlPropertyType.SelectedValue.Equals("T") && (txtPropertyDefaultMale.Text.Length > 10))
        {
            txtPropertyDefaultMale.Text = string.Empty;
        }
        if (ddlPropertyType.SelectedValue.Equals("T") && (txtPropertyDefaultFemale.Text.Length > 10))
        {
            txtPropertyDefaultFemale.Text = string.Empty;
        }
        ddlPropertyGender.SelectedIndex = 0;
        GenderDropDownList();
        BindBlankValueDropDownGrid();
        FieldTypeDropDownList();
        if (ddlPropertyType.SelectedValue == "L")
        {
            trButtonType.Visible = true;
            BindTemplates();
        }

        else
        {
            trButtonType.Visible = false;
        }


        //if (ddlPropertyType.SelectedValue == "C" || ddlPropertyType.SelectedValue == "B" || ddlPropertyType.SelectedValue == "R")
        //{
        //    lblCoumnsPerRow.Visible = true;
        //    ddlCoumnsPerRow.Visible = true;
        //}
        //else
        //{
        //    lblCoumnsPerRow.Visible = false;
        //    ddlCoumnsPerRow.Visible = false;
        //}
    }
    private void BindTemplates()
    {
        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
        DataSet ds = master.GetAllTypeTemplates(common.myInt(Session["HospitalLocationId"]), "S");
        DataView dv = new DataView(ds.Tables[0]);
        dv.RowFilter = "DisplayInTemplate=1";
        DataTable dt = dv.ToTable();
        if (dt.Rows.Count > 0)
        {
            ddlStaticTemplate.DataSource = dt;
            ddlStaticTemplate.DataTextField = "SectionName";
            ddlStaticTemplate.DataValueField = "SectionId";
            ddlStaticTemplate.DataBind();
        }
    }
    public void FieldTypeDropDownList()
    {
        trHeadingStart.Visible = false;
        trHeadingEnd.Visible = false;
        //lbtnServices.Visible = false;
        chkPrintValueLineWise.Visible = false;
        chkPrintValueLineWise.Checked = false;

        if (ddlPropertyType.SelectedValue == "IM")
        {
            lblImageMessage.Visible = true;
        }
        else
        {
            lblImageMessage.Visible = false;
        }

        if (ddlPropertyType.SelectedItem.Value == "T")
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = true;
            txtMaxLength.Visible = true;
            trMale.Visible = true;
            trFemale.Visible = true;
            trWordProcessor.Visible = false;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = false;
            trFieldFormats.Visible = false;

            lblLinkUrl.Visible = true;
            lbl_IsLink.Visible = true;
            ddlLinkRequired.Visible = true;
            txtLinkUrl.Visible = true;
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;
        }

        //Added on 12-Feb-2014
        //Added By Naushad 
        //else   if (ddlPropertyType.SelectedItem.Value == "Y")
        //{
        //    lblMaxLength.Visible = true;
        //    lblDefaultMaleText.Text = "Default Url Link";
        //    txtMaxLength.Visible = true;
        //    trMale.Visible = true;
        //    trFemale.Visible = true;
        //    trWordProcessor.Visible = false;
        //    //trBoolean.Visible = false;
        //    ddlPatientDataObject.Visible = false;
        //    lblDataObject.Visible = false;
        //    pnlNewEditValues.Visible = false;
        //    trFieldFormats.Visible = false;
        //}            

        else if (ddlPropertyType.SelectedItem.Value == "M")
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = true;
            trFemale.Visible = true;
            trWordProcessor.Visible = false;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = false;
            trFieldFormats.Visible = false;

            lblLinkUrl.Visible = true;
            lbl_IsLink.Visible = true;
            ddlLinkRequired.Visible = true;
            txtLinkUrl.Visible = true;
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;

        }
        else if (ddlPropertyType.SelectedItem.Value == "W")
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = false;
            trFemale.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = false;
            trFieldFormats.Visible = true;
            rboFormats.Visible = true;
            rboFormats.SelectedValue = "1";
            txtFormatName.Visible = true;
            ddlTemplateFieldFormats.Visible = false;
            BindTemplateFieldFormats();
            trWordProcessor.Visible = true;

            lblLinkUrl.Visible = true;
            lbl_IsLink.Visible = true;
            ddlLinkRequired.Visible = true;
            txtLinkUrl.Visible = true;
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;
        }
        //---------------------------------------------------
        else if (ddlPropertyType.SelectedItem.Value == "C")
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = false;
            trFemale.Visible = false;
            trWordProcessor.Visible = false;
            //HideUnHideDefaultMaleFemale(false);
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;

            pnlNewEditValues.Visible = true;
            pnlValueBoolean.Visible = false;
            pnlValueDropDown.Visible = true;
            trFieldFormats.Visible = false;
            lblValueDropDown.Text = "Field Type - " + ddlPropertyType.SelectedItem.Text;

            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;
            chkSortingOnSequenceNo.Visible = true;
            chkPrintValueLineWise.Visible = true;

        }
        else if (ddlPropertyType.SelectedItem.Value == "D" || ddlPropertyType.SelectedItem.Value == "IM")
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            //HideUnHideDefaultMaleFemale(false);
            trMale.Visible = false;
            trFemale.Visible = false;
            trWordProcessor.Visible = false;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = true;
            pnlValueBoolean.Visible = false;
            pnlValueDropDown.Visible = true;
            trFieldFormats.Visible = false;
            lblValueDropDown.Text = "Field Type - " + ddlPropertyType.SelectedItem.Text;

            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;
            chkSortingOnSequenceNo.Visible = true;

        }
        else if (ddlPropertyType.SelectedItem.Value == "R")
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            //HideUnHideDefaultMaleFemale(false);
            trMale.Visible = false;
            trFemale.Visible = false;
            trWordProcessor.Visible = false;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = true;
            pnlValueBoolean.Visible = false;
            pnlValueDropDown.Visible = true;
            trFieldFormats.Visible = false;
            lblValueDropDown.Text = "Field Type - " + ddlPropertyType.SelectedItem.Text;

            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;
            chkSortingOnSequenceNo.Visible = true;
        }

        else if (ddlPropertyType.SelectedItem.Value == "O")
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = false;
            trFemale.Visible = false;
            //trBoolean.Visible = true;
            ddlPatientDataObject.Visible = true;
            lblDataObject.Visible = true;
            pnlNewEditValues.Visible = false;
            trWordProcessor.Visible = false;
            trFieldFormats.Visible = false;

            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;

            BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
            ddlPatientDataObject.DataSource= emr.GetEMRTemplateDataObjects();
            ddlPatientDataObject.DataTextField = "ObjectName";
            ddlPatientDataObject.DataValueField = "Id";
            ddlPatientDataObject.DataBind();
            emr = null;
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;

        }
        else if (ddlPropertyType.SelectedItem.Value == "S" || ddlPropertyType.SelectedItem.Value == "ST" || ddlPropertyType.SelectedItem.Value == "SB")
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = true;
            trFemale.Visible = true;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = false;
            trWordProcessor.Visible = false;
            trFieldFormats.Visible = false;

            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;
        }
        else if (ddlPropertyType.SelectedItem.Value == "H")
        {
            lbtnServices.Visible = false;
            trHeadingStart.Visible = true;
            trHeadingEnd.Visible = true;

            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = false;
            trFemale.Visible = false;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = false;
            trWordProcessor.Visible = false;
            trFieldFormats.Visible = false;
            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;
        }
        else if (ddlPropertyType.SelectedItem.Value == "I")//Investigation
        {

            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = false;
            trFemale.Visible = false;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = false;
            trWordProcessor.Visible = false;
            trFieldFormats.Visible = false;
            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;
            if (common.myStr(ViewState["FType"]) == "I")
            {
                lbtnServices.Visible = true;
            }
            else
            {
                lbtnServices.Visible = false;
            }
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;
        }
        else if (ddlPropertyType.SelectedItem.Value == "IS")//Investigation Special
        {
            lbtnServices.Visible = true;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = false;
            trFemale.Visible = false;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = false;
            trWordProcessor.Visible = false;
            trFieldFormats.Visible = false;
            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;
            if (common.myStr(ViewState["FType"]) == "SI")
            {
                lbtnServices.Visible = true;
            }
            else
            {
                lbtnServices.Visible = false;
            }
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;
        }
        else
        {
            lbtnServices.Visible = false;
            lblMaxLength.Visible = false;
            txtMaxLength.Visible = false;
            trMale.Visible = false;
            trFemale.Visible = false;
            //trBoolean.Visible = false;
            ddlPatientDataObject.Visible = false;
            lblDataObject.Visible = false;
            pnlNewEditValues.Visible = false;
            trWordProcessor.Visible = false;
            trFieldFormats.Visible = false;
            lblLinkUrl.Visible = false;
            lbl_IsLink.Visible = false;
            ddlLinkRequired.Visible = false;
            txtLinkUrl.Visible = false;
            // lbtnServices.Visible = false;
            chkSortingOnSequenceNo.Visible = false;
            chkSortingOnSequenceNo.Checked = false;
        }
    }

    protected void HideUnHideDefaultMaleFemale(bool b)
    {
        lblDefaultMaleText.Visible = b; txtPropertyDefaultMale.Visible = b;
        lblDefaultFemaleText.Visible = b; txtPropertyDefaultFemale.Visible = b;
    }

    private void BindBlankValueDropDownGrid()
    {
        try
        {
            DataTable dtValueDropDown = new DataTable();
            dtValueDropDown.Columns.Add("FieldID");
            dtValueDropDown.Columns.Add("ValueName");
            dtValueDropDown.Columns.Add("Code");
            dtValueDropDown.Columns.Add("DefaultTextMale");
            dtValueDropDown.Columns.Add("DefaultTextFemale");
            dtValueDropDown.Columns.Add("ValueId");
            dtValueDropDown.Columns.Add("ActiveText");
            dtValueDropDown.Columns.Add("ImagePath");
            dtValueDropDown.Columns.Add("ImageFileName");
            dtValueDropDown.Columns.Add("ScoreValue", typeof(float));

            for (int i = 0; i < 2; i++)
            {
                DataRow Dr = dtValueDropDown.NewRow();
                Dr["FieldID"] = "";
                Dr["ValueName"] = "";
                Dr["Code"] = "";
                Dr["DefaultTextMale"] = "";
                Dr["DefaultTextFemale"] = "";
                Dr["ValueId"] = "";
                Dr["ActiveText"] = "";
                Dr["ImagePath"] = "";
                Dr["ImageFileName"] = "";
                Dr["ScoreValue"] = DBNull.Value;
                dtValueDropDown.Rows.Add(Dr);
            }
            gvValueDropDown.DataSource = dtValueDropDown;
            gvValueDropDown.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnAddNewRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dtValueDropDown = new DataTable();
            dtValueDropDown.Columns.Add("FieldID");
            dtValueDropDown.Columns.Add("ValueName");
            dtValueDropDown.Columns.Add("Code");
            dtValueDropDown.Columns.Add("DefaultTextMale");
            dtValueDropDown.Columns.Add("DefaultTextFemale");
            dtValueDropDown.Columns.Add("ValueId");
            dtValueDropDown.Columns.Add("ActiveText");
            dtValueDropDown.Columns.Add("ForCheckbox");
            dtValueDropDown.Columns.Add("ImagePath");
            dtValueDropDown.Columns.Add("ImageFileName");
            dtValueDropDown.Columns.Add("ScoreValue");

            DataRow dR;

            int j = 0;
            foreach (GridViewRow item in gvValueDropDown.Rows)
            {
                dR = dtValueDropDown.NewRow();
                TextBox txtOption = (TextBox)item.FindControl("txtOptionGrid");
                TextBox txtCode = (TextBox)item.FindControl("txtCodeGrid");
                TextBox txtDefaultMale = (TextBox)item.FindControl("txtDefaultMaleGrid");
                TextBox txtDefaultFemale = (TextBox)item.FindControl("txtDefaultFemaleGrid");
                CheckBox chk = item.FindControl("chkDefault") as CheckBox;
                HiddenField hdnValueId = item.FindControl("hdnValueId") as HiddenField;
                HiddenField hdnFieldID = item.FindControl("hdnFieldID") as HiddenField;
                TextBox txtScoreValue = (TextBox)item.FindControl("txtScoreValue");

                FileUpload _FileUpload = (FileUpload)item.FindControl("_FileUpload");
                HiddenField hdnImagePath = (HiddenField)item.FindControl("hdnImagePath");
                Label lblImageName = (Label)item.FindControl("lblImageName");
                hdnImagePath.Value = _FileUpload.FileName;
                if (chk.Checked)
                    dR["ForCheckbox"] = "1";
                else
                    dR["ForCheckbox"] = "0";
                dR["FieldID"] = common.myStr(hdnFieldID.Value);
                dR["ValueName"] = txtOption.Text.ToString().Trim();
                dR["Code"] = txtCode.Text.ToString().Trim();
                dR["DefaultTextMale"] = txtDefaultMale.Text.ToString().Trim();
                dR["DefaultTextFemale"] = txtDefaultFemale.Text.ToString().Trim();
                dR["ValueId"] = hdnValueId.Value;
                dR["ImagePath"] = hdnImagePath.Value;
                dR["ScoreValue"] = txtScoreValue.Text.ToString().Trim();
                if (ViewState["FieldValueData"] != null)
                {
                    DataTable dt = (DataTable)ViewState["FieldValueData"];
                    if (dt.Rows.Count - 1 > 0)
                    {
                        if (common.myStr(dt.Rows[j]["ImageFileName"]) != "")
                        {
                            dR["ImageFileName"] = common.myStr(dt.Rows[j]["ImageFileName"]);
                            j++;
                        }
                        else
                        {
                            if (_FileUpload.FileName == lblImageName.Text)
                            {
                                dR["ImageFileName"] = lblImageName.Text;
                            }
                            else
                            {
                                dR["ImageFileName"] = _FileUpload.FileName;
                            }
                        }
                    }
                    else
                    {
                        if (_FileUpload.FileName == lblImageName.Text)
                        {
                            dR["ImageFileName"] = lblImageName.Text;
                        }
                        else
                        {
                            dR["ImageFileName"] = _FileUpload.FileName;
                        }
                    }

                }
                else
                {
                    if (_FileUpload.FileName == lblImageName.Text)
                    {
                        dR["ImageFileName"] = lblImageName.Text;
                    }
                    else
                    {
                        dR["ImageFileName"] = _FileUpload.FileName;
                    }
                }
                if (lblImageName.Text == "")
                {
                    lblImageName.Text = _FileUpload.FileName;
                }

                dtValueDropDown.Rows.Add(dR);
            }
            dR = dtValueDropDown.NewRow();
            dR["ForCheckbox"] = "0";
            dR["FieldID"] = "";
            dR["ValueName"] = "";
            dR["Code"] = "";
            dR["DefaultTextMale"] = "";
            dR["DefaultTextFemale"] = "";
            dR["ValueId"] = "";
            dR["ActiveText"] = "";
            dR["ImagePath"] = "";
            dR["ImageFileName"] = "";
            dR["ScoreValue"] = DBNull.Value;
            dtValueDropDown.Rows.Add(dR);

            ViewState["FieldValueData"] = dtValueDropDown;

            gvValueDropDown.DataSource = dtValueDropDown;
            gvValueDropDown.DataBind();
            for (int i = 0; i < gvValueDropDown.Rows.Count; i++)
            {
                if (common.myInt(dtValueDropDown.Rows[i]["ForCheckbox"]) == 1)
                {
                    CheckBox chk = gvValueDropDown.Rows[i].FindControl("chkDefault") as CheckBox;
                    chk.Checked = true;
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvValueDropDown_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlStatus = e.Row.FindControl("ddlStatus") as DropDownList;
            if (ddlStatus.ToolTip != "")
            {
                ddlStatus.Items[ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(ddlStatus.ToolTip))].Selected = true;
            }
            if (ddlPropertyType.SelectedValue == "IM")
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[9].Visible = false;
            }
            else if (ddlPropertyType.SelectedValue == "R")
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[9].Visible = true;
            }
            else
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[9].Visible = false;
            }
            FileUpload _FileUpload = (FileUpload)e.Row.FindControl("_FileUpload");
        }
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (ddlPropertyGender.SelectedItem.Value == "M")
            {
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[4].Visible = false;
                //e.Row.Cells[6].Visible = false;
                e.Row.Cells[9].Visible = false;
            }
            else if (ddlPropertyGender.SelectedItem.Value == "F")
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = true;
                //e.Row.Cells[6].Visible = false;
                e.Row.Cells[9].Visible = false;
            }

            if (ddlPropertyType.SelectedValue == "IM")
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[9].Visible = false;
            }
            else if (ddlPropertyType.SelectedValue == "R")
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[9].Visible = true;
            }
            else
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[9].Visible = false;
            }
        }
    }

    protected void ddlPropertyGender_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        GenderDropDownList();
        BindBlankValueDropDownGrid();
    }

    public void GenderDropDownList()
    {
        try
        {
            if (ddlPropertyType.SelectedValue == "C" || ddlPropertyType.SelectedValue == "B" || ddlPropertyType.SelectedValue == "R")
            {
                lblCoumnsPerRow.Visible = true;
                ddlCoumnsPerRow.Visible = true;
            }
            else
            {
                lblCoumnsPerRow.Visible = false;
                ddlCoumnsPerRow.Visible = false;
            }

            if (ddlPropertyGender.SelectedItem.Value == "B")
            {
                if (ddlPropertyType.SelectedItem.Value == "B")
                {
                    trMale.Visible = false;
                    trFemale.Visible = false;

                    tdMale1.Visible = true;
                    tdMale2.Visible = true;
                    tdMale3.Visible = true;
                    tdMale4.Visible = true;
                    tdFemale1.Visible = true;
                    tdFemale2.Visible = true;
                    tdFemale3.Visible = true;
                    tdFemale4.Visible = true;
                }
                else if (ddlPropertyType.SelectedItem.Value == "T" || ddlPropertyType.SelectedItem.Value == "M" || ddlPropertyType.SelectedItem.Value == "S" || ddlPropertyType.SelectedItem.Value == "ST" || ddlPropertyType.SelectedItem.Value == "SB")
                {
                    trMale.Visible = true;
                    trFemale.Visible = true;
                }
            }
            else if (ddlPropertyGender.SelectedItem.Value == "M")
            {
                if (ddlPropertyType.SelectedItem.Value == "B")
                {
                    trMale.Visible = false;
                    trFemale.Visible = false;

                    tdMale1.Visible = true;
                    tdMale2.Visible = true;
                    tdMale3.Visible = true;
                    tdMale4.Visible = true;
                    tdFemale1.Visible = false;
                    tdFemale2.Visible = false;
                    tdFemale3.Visible = false;
                    tdFemale4.Visible = false;
                }
                else if (ddlPropertyType.SelectedItem.Value == "T" || ddlPropertyType.SelectedItem.Value == "M" || ddlPropertyType.SelectedItem.Value == "S" || ddlPropertyType.SelectedItem.Value == "ST" || ddlPropertyType.SelectedItem.Value == "SB")
                {
                    trMale.Visible = true;
                    trFemale.Visible = false;
                }
            }
            else if (ddlPropertyGender.SelectedItem.Value == "F")
            {
                if (ddlPropertyType.SelectedItem.Value == "B")
                {
                    trMale.Visible = false;
                    trFemale.Visible = false;

                    tdMale1.Visible = false;
                    tdMale2.Visible = false;
                    tdMale3.Visible = false;
                    tdMale4.Visible = false;
                    tdFemale1.Visible = true;
                    tdFemale2.Visible = true;
                    tdFemale3.Visible = true;
                    tdFemale4.Visible = true;
                }
                else if (ddlPropertyType.SelectedItem.Value == "T" || ddlPropertyType.SelectedItem.Value == "M" || ddlPropertyType.SelectedItem.Value == "S" || ddlPropertyType.SelectedItem.Value == "ST" || ddlPropertyType.SelectedItem.Value == "SB")
                {
                    trMale.Visible = false;
                    trFemale.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindTemplateFieldFormats()
    {
        try
        {
            //ddlTemplateFieldFormats.Items.Clear();
            ddlTemplateFieldFormats.DataSource = null;
            ddlTemplateFieldFormats.DataBind();
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            if (common.myStr(lblSelectedFieldID.Text) != "")
            {
                String strFieldID = lblSelectedFieldID.Text.Substring(1);
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hs.Add("@intFieldId", common.myInt(strFieldID));
                hs.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                ds = dl.FillDataSet(CommandType.Text, "Select FormatId, FormatName from EMRTemplateFieldFormats Where HospitalLocationId = @inyHospitalLocationId And FieldId=@intFieldId And Active=1 Order by FormatName", hs);
                ddlTemplateFieldFormats.DataSource = ds;
                ddlTemplateFieldFormats.DataTextField = "FormatName";
                ddlTemplateFieldFormats.DataValueField = "FormatId";
                ddlTemplateFieldFormats.DataBind();
            }
            else
            {
                ddlTemplateFieldFormats.DataSource = null;
                ddlTemplateFieldFormats.DataBind();
            }
            ddlTemplateFieldFormats.Items.Insert(0, "-Select-");
            ddlTemplateFieldFormats.Items[0].Value = "";
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void rboFormats_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (rboFormats.SelectedIndex == 0)  // For New
        {
            ddlTemplateFieldFormats.Visible = false;
            txtFormatName.Visible = true;
            ddlTemplateFieldFormats.SelectedIndex = 0;
        }
        else  // For Edit (Existing Format from Dropdown List)
        {
            ddlTemplateFieldFormats.Visible = true;
            txtFormatName.Visible = false;
            txtFormatName.Text = String.Empty;
        }
    }

    protected void ddlTemplateFieldFormats_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hs.Add("@intFormatId", common.myInt(ddlTemplateFieldFormats.SelectedItem.Value));
            ds = dl.FillDataSet(CommandType.Text, "Select FormatText from EMRTemplateFieldFormats Where FormatId = @intFormatId ", hs);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtPropertyDefaultContentWordProcessor.Content = common.myStr(ds.Tables[0].Rows[0]["FormatText"]).Trim();
            }
            else
            {
                txtPropertyDefaultContentWordProcessor.Content = String.Empty;
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnRowsVisible_OnClick(object sender, EventArgs e)
    {
        trRows.Visible = rbtnYesTabular.Checked;
        pnlValueDropDown.Visible = rbtnYesTabular.Checked;
        if (rbtnYesTabular.Checked)
        {
            visibleIfTabular();
        }

        trBlankRows.Visible = rbtnYesTabular.Checked;
        btnSetFormula.Visible = rbtnYesTabular.Checked;
        trIsFreezeFirstColumn.Visible = rbtnYesTabular.Checked;
    }

    protected void ddlIsMandatory_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlIsMandatory.SelectedValue) == 1)
            {
                ddlMandatoryType.Enabled = true;
            }
            else
            {
                ddlMandatoryType.SelectedIndex = 0;
                ddlMandatoryType.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            lblFieldMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblFieldMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lbtnServices_OnClick(object sender, EventArgs e)
    {
        // RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP&OtherUserId="+ddlRefertodoctor.SelectedValue+"&UserName="+ddlRefertodoctor.Text;
        RadWindowForNew.NavigateUrl = "/EMR/Masters/InvestigationLabServiceTag.aspx?FieldId=" + common.myStr(lblSelectedFieldID.Text) + "&FName=" + common.myStr(ViewState["FName"]) + "&FType=" + common.myStr(ViewState["FType"]);
        RadWindowForNew.Height = 500;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = string.Empty; //"OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnEmployeeTypeTagging_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Masters/TemplateFieldEmpTypesTagging.aspx?secid=" + common.myStr(ViewState["SectionID"]);
        RadWindowForNew.Height = 570;
        RadWindowForNew.Width = 650;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnSetFormula_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Masters/TemplateFormula.aspx?secid=" + common.myStr(ViewState["SectionID"]);
        RadWindowForNew.Height = 570;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }


    protected void btnSetScoreFormula_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Masters/SetScoreFormula.aspx?secid=" + common.myStr(ViewState["SectionID"]);
        RadWindowForNew.Height = 570;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void gvValueDropDown_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[(byte)GridColgvValueDropDown.COL2].Visible = false;
                e.Row.Cells[(byte)GridColgvValueDropDown.COL3].Visible = false;
                e.Row.Cells[(byte)GridColgvValueDropDown.COL4].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[(byte)GridColgvValueDropDown.COL2].Visible = false;
                e.Row.Cells[(byte)GridColgvValueDropDown.COL3].Visible = false;
                e.Row.Cells[(byte)GridColgvValueDropDown.COL4].Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblFieldMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblFieldMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }

    }

}