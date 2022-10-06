using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class EMR_Masters_FacilityDepartmentLinking : System.Web.UI.Page
{
    //Global variables

    DataSet ds = new DataSet();//dataset objetct
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;//connectionstring
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsFacility objFacility;
    StringBuilder strXML;
    ArrayList coll;

    //Page load event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //btnSave.Visible = false;
            //btnSend4WO.Visible = false;
            //btnClear.Visible = false;
            hdnisSaveChk.Value = "0";
            ViewState["FacilityCompanyCode"] = "";
            bindTaggedMaster();
        }
    }
    protected void bindTaggedMaster()
    {
        try
        {
            objFacility = new BaseC.clsFacility(sConString);
            DataSet ds = objFacility.GetFacilityWiseTaggedMasterName();
            DataView dv = ds.Tables[0].DefaultView;
            // dv.RowFilter = "ModuleId = " + common.myInt(Session["ModuleId"]) + "";
            foreach (DataRow dr in ((DataTable)dv.ToTable()).Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["MasterName"];
                item.Value = dr["Id"].ToString();
                item.Attributes.Add("TableType", common.myStr(dr["TableType"]));
                item.Attributes.Add("MasterProcedure", common.myStr(dr["MasterProcedure"]));

                ddlTaggedFor.Items.Add(item);
                item.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlTaggedFor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblItemgrpStar.Visible = false;
            lblItemSubgrpStar.Visible = false;
            if ((common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]) == "IM") || (common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]) == "IMS"))
            {
                trItemSearch.Visible = true;
                ddlItemGroup.Text = "";
                ddlItemSubGroup.Text = "";
                fillGroupCombo();
                lblSearchCategory.Text = "Group";
                lblSearchSubCategory.Text = "Sub-Group";
            }
            else if (common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).ToString().Trim().ToUpper() == "I")
            {
                trItemSearch.Visible = true;
                ddlItemGroup.Text = "";
                ddlItemSubGroup.Text = "";
                fillGroupCombo();
                lblSearchCategory.Text = "Department";
                lblSearchSubCategory.Text = "Sub-Department";
                lblItemgrpStar.Visible = true;
                lblItemSubgrpStar.Visible = true;
            }
            else
                trItemSearch.Visible = false;

            gvDepartment.DataSource = null;
            gvDepartment.DataBind();
            grvFacilityWiseTag.DataSource = null;
            grvFacilityWiseTag.DataBind();
            ViewState["FacilityDept"] = null;
            lblServiceName.Text = "";
            lblServiceName.Text = common.myStr(ddlTaggedFor.SelectedItem.Text) + " " + "Name";


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void fillGroupCombo()
    {
        try
        {
            BaseC.clsPharmacy phr = new BaseC.clsPharmacy(sConString); DataSet ds = new DataSet();
            int isData = 0;
            ddlItemGroup.Text = "";
            ddlItemSubGroup.Text = "";
            ddlItemGroup.Items.Clear();
            ddlItemSubGroup.Items.Clear();
            ddlItemGroup.Items.Add(new RadComboBoxItem("", "0"));
            if (common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).ToString().Trim().ToUpper() == "IM")
            {
                ds = phr.getItemCategoryMaster(0, common.myInt(Session["HospitalLocationId"]), 1, common.myInt(Session["UserId"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlItemGroup.DataTextField = "ItemCategoryNameWithNo";
                        ddlItemGroup.DataValueField = "ItemCategoryId";
                        isData = 1;
                    }
                }
            }
            else if (common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).ToString().Trim().ToUpper() == "I")
            {
                ds = phr.getDepartment(common.myInt(Session["HospitalLocationId"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlItemGroup.DataTextField = "DepartmentName";
                        ddlItemGroup.DataValueField = "DepartmentID";
                        isData = 1;
                    }
                }
            }
            if (isData == 1)
            {
                ddlItemGroup.DataSource = ds;
                ddlItemGroup.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlItemGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlItemGroup.SelectedValue) > 0)
            {
                DataSet ds = new DataSet();
                int isData = 0;

                ddlItemSubGroup.Text = "";
                ddlItemSubGroup.Items.Clear();
                ddlItemSubGroup.Items.Add(new RadComboBoxItem("", "0"));
                if (common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).ToString().Trim().ToUpper() == "IM")
                {
                    BaseC.clsPharmacy phr = new BaseC.clsPharmacy(sConString);
                    ds = phr.getItemCategoryDetails(0, "", common.myInt(ddlItemGroup.SelectedValue), common.myInt(Session["HospitalLocationId"]), 1, common.myInt(Session["UserId"]));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlItemSubGroup.DataTextField = "ItemSubCategoryName";
                            ddlItemSubGroup.DataValueField = "ItemSubCategoryId";
                            isData = 1;
                        }
                    }
                }
                else if (common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).ToString().Trim().ToUpper() == "I")
                {
                    BaseC.RestFulAPI cm = new BaseC.RestFulAPI(sConString);
                    ds = cm.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlItemGroup.SelectedValue), "", 0);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlItemSubGroup.DataTextField = "SubName";
                            ddlItemSubGroup.DataValueField = "SubDeptId";
                            isData = 1;
                        }
                    }
                }
                if (isData == 1)
                {
                    ddlItemSubGroup.DataSource = ds;
                    ddlItemSubGroup.DataBind();
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
    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(ddlTaggedFor.SelectedValue) != "")
            {
                bindFacility();
                if (common.myStr(Application["IsSAPOrInterfaceEnabled"]).Equals("Y") && (common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).ToString().Trim().ToUpper().Equals("C"))) { btnSend4WO.Enabled = false; chkUnSelect.Enabled = false; }

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void bindFacility()
    {
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.getFacility(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));
            if (ds.Tables.Count > 0)
            {
                //DataView dv = ds.Tables[0].DefaultView;
                //if (common.myStr(Application["IsSAPOrInterfaceEnabled"]).Equals("Y"))
                //{
                //    dv.RowFilter = "FacilityId = " + common.myStr(Session["FacilityId"]);
                //    if (dv.Count > 0)
                //    {
                //        ViewState["FacilityCompanyCode"] = common.myStr(dv.ToTable().Rows[0]["SAPCompanyCode"]);
                //    }
                //    dv.RowFilter = "";
                //    dv.RowFilter = "SAPCompanyCode = '" + common.myStr(ViewState["FacilityCompanyCode"]) + "'";
                //}
                ddlFacility.Items.Clear();
                ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlFacility.SelectedIndex = 0;
                foreach (DataRowView drv in ds.Tables[0].DefaultView)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)drv["Name"];
                    item.Value = drv["FacilityID"].ToString();
                    item.Attributes.Add("SAPCompanyCode", common.myStr(drv["SAPCompanyCode"]));
                    ddlFacility.Items.Add(item);
                    item.DataBind();
                }

                populateFacilityData(0);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void populateFacilityData(int FaciLityId)
    {
        try
        {
            if (common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).ToString().Trim().ToUpper() == "I")
            {
                if (common.myInt(ddlItemGroup.SelectedValue) == 0)
                {
                    Alert.ShowAjaxMsg("Please! Select department first.", Page.Page);
                    ddlItemGroup.Focus();
                    return;
                }
                else if (common.myInt(ddlItemSubGroup.SelectedValue) == 0)
                {
                    Alert.ShowAjaxMsg("Please! Select sub department first.", Page.Page);
                    ddlItemSubGroup.Focus();
                    return;
                }
            }
            objFacility = new BaseC.clsFacility(sConString);
            DataSet ds = objFacility.GetFacilityWiseCommonMaster(common.myInt(Session["HospitalLocationID"]), common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).Trim(), 0, common.myInt(FaciLityId), common.myInt(Session["UserId"]), common.myStr(txtSeviceName.Text), common.myInt(ddlItemGroup.SelectedValue), common.myInt(ddlItemSubGroup.SelectedValue));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow DR = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(DR);
                }
                DataView dv = ds.Tables[0].DefaultView;
                if (common.myStr(Application["IsSAPOrInterfaceEnabled"]).Equals("Y") && common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).Equals("C"))
                {
                    dv.RowFilter = "InterfaceMappingCode <> '' And CompanyCode <> '' ";
                }

                chkUnSelect.Checked = false;
                gvDepartment.DataSource = dv.ToTable();
                gvDepartment.DataBind();
                lblMainFacilityCount.Text = "Total Records : " + (dv.ToTable()).Rows.Count.ToString();
            }
            lblMainFacilityTag.Text = ddlTaggedFor.SelectedItem.Text + " Name";
            lblTaggingFacility.Text = ddlTaggedFor.SelectedItem.Text + " Name";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected DataTable BindBlankTableInv()
    {
        DataTable dtInv = new DataTable();

        dtInv.Columns.Add("Id", typeof(int));
        dtInv.Columns.Add("ColumnId", typeof(int));
        dtInv.Columns.Add("ColumnName", typeof(string));
        dtInv.Columns.Add("FacilityID", typeof(int));
        dtInv.Columns.Add("FacilityName", typeof(string));
        return dtInv;
    }
    protected void ddlFacility_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState.Remove("SelectedFacilityId");
        if (common.myInt(ddlFacility.SelectedIndex) > 0)
        {
            ViewState["SelectedFacilityId"] = ddlFacility.SelectedValue;
            FillFacilityWiseData(common.myInt(ddlFacility.SelectedValue));
        }
        else
            btnClear_Click(null, null);

    }
    protected void FillFacilityWiseData(int FaciLityId)
    {
        try
        {
            objFacility = new BaseC.clsFacility(sConString);
            DataSet ds = objFacility.GetFacilityWiseCommonMaster(common.myInt(Session["HospitalLocationID"]), common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).Trim(), 0, common.myInt(FaciLityId), common.myInt(Session["UserId"]), "", common.myInt(ddlItemGroup.SelectedValue), common.myInt(ddlItemSubGroup.SelectedValue));
            if (ds.Tables.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;
                if (common.myStr(Application["IsSAPOrInterfaceEnabled"]).Equals("Y") && common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).Equals("C"))
                {
                    dv.RowFilter = "InterfaceMappingCode <> '' And CompanyCode <> '' ";
                }
                ViewState["FacilityDept"] = dv.ToTable();
                grvFacilityWiseTag.DataSource = dv.ToTable();
                grvFacilityWiseTag.DataBind();
                lblSelFacilityCount.Text = "Total Records : " + (dv.ToTable()).Rows.Count.ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void chkUnSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gv in gvDepartment.Rows)
        {
            CheckBox chk = (CheckBox)gv.FindControl("chkSelect");
            chk.Checked = false;
            if (chkUnSelect.Checked == true)
                chk.Checked = true;

        }
    }
    protected void btnSend4WO_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlFacility.SelectedIndex > 0)
            {
                DataTable dt;
                if (gvDepartment.Rows.Count > 0)
                {
                    grvFacilityWiseTag.DataSource = null;
                    grvFacilityWiseTag.DataBind();

                    if (ViewState["FacilityDept"] == null)
                        dt = BindBlankTableInv();
                    else
                        dt = (DataTable)ViewState["FacilityDept"];

                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "";
                    foreach (GridViewRow gv in gvDepartment.Rows)
                    {
                        CheckBox chk = (CheckBox)gv.FindControl("chkSelect");
                        if (chk.Checked == true)
                        {
                            dv.RowFilter = "ColumnId = " + common.myStr(((HiddenField)gv.FindControl("hdnColumnId")).Value) + "";
                            if (dv.ToTable().Rows.Count == 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr["Id"] = 0;
                                dr["ColumnId"] = common.myInt(((HiddenField)gv.FindControl("hdnColumnId")).Value);
                                dr["ColumnName"] = common.myStr(((Label)gv.FindControl("lblColumnName")).Text);
                                dr["FacilityID"] = common.myInt(ddlFacility.SelectedValue);
                                dr["FacilityName"] = common.myStr(ddlFacility.SelectedItem.Text);
                                dt.Rows.Add(dr);
                                dt.AcceptChanges();

                            }

                        }
                    }
                    hdnisSaveChk.Value = "1";
                    dv.RowFilter = "";
                    ViewState["FacilityDept"] = dt;
                    grvFacilityWiseTag.DataSource = dt;
                    grvFacilityWiseTag.DataBind();
                    lblSelFacilityCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                }
            }
            else { Alert.ShowAjaxMsg("Please!! Select Facility to tag..", Page.Page); }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
    }
    protected void lnkDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkBtn = (LinkButton)sender;
            int hdnSelColumnId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnSelColumnId")).Value);

            if (hdnSelColumnId > 0)
            {
                #region ---|| De - active Facility Tagging ||---
                int hdnID = common.myInt(((HiddenField)lnkBtn.FindControl("hdnID")).Value);
                if (hdnID > 0)
                {
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    Hashtable hstInput = new Hashtable();
                    string TableType = common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]);
                    string strqry = string.Empty;
                    hstInput.Add("@ColumnId", hdnSelColumnId);
                    hstInput.Add("@Id", hdnID);
                    if (TableType.Equals("C"))       //Company
                    {
                        strqry = "UPDATE FacilityWiseCompany SET Active = 0 WHERE CompanyId = @ColumnId and Id = @Id";
                    }
                    else if (TableType.Equals("D"))  //Department
                    {
                        strqry = "UPDATE FacilityWiseDepartment SET Active = 0 WHERE DepartmentId = @ColumnId and Id = @Id";
                    }
                    else if (TableType.Equals("S"))  // SubDepartment
                    {
                        strqry = "UPDATE FacilityWiseSubDepartment SET Active = 0 WHERE SubDeptId = @ColumnId and Id = @Id";
                    }
                    else if (TableType.Equals("I"))  // ItemOfServices
                    {
                        strqry = "UPDATE FacilityWiseItemOfService SET Active = 0 WHERE ServiceId = @ColumnId and Id = @Id";
                    }
                    else if (TableType.Equals("SP"))  // Suppliers
                    {
                        strqry = "UPDATE PhrSupplierFacilityTagging SET Active = 0 WHERE SupplierId = @ColumnId and Id = @Id";
                    }
                    else if (TableType.Equals("IM"))  // ItemMaster
                    {
                        strqry = "UPDATE FacilityWiseItemMaster SET Active = 0 WHERE ItemId = @ColumnId and Id = @Id";
                    }
                    else if (TableType.Equals("TM"))  // EMRTemplate
                    {
                        strqry = "UPDATE FacilityWiseEMRTemplate SET Active = 0 WHERE templateid = @ColumnId and Id = @Id";
                    }
                    else if (TableType.Equals("PM"))  // PaymentMode
                    {
                        strqry = "UPDATE FacilityWisePaymentMode SET Active = 0 WHERE PaymentModeId = @ColumnId and Id = @Id";
                    }
                    if (!string.IsNullOrEmpty(strqry) && hstInput.Count > 0)
                    {
                        objDl.ExecuteNonQuery(CommandType.Text, strqry, hstInput);
                        if (ViewState["SelectedFacilityId"] != null)
                        {
                            FillFacilityWiseData(common.myInt(ViewState["SelectedFacilityId"]));
                        }
                    }
                }
                else
                {
                    DataTable dt = (DataTable)ViewState["FacilityDept"];
                    foreach (DataRow dr in dt.Select("ColumnId = " + common.myStr(hdnSelColumnId) + ""))
                    {
                        if (common.myStr(dr["ColumnId"]) == common.myStr(hdnSelColumnId))
                            dr.Delete();
                    }
                    hdnisSaveChk.Value = "1";
                    dt.AcceptChanges();
                    grvFacilityWiseTag.DataSource = dt;
                    grvFacilityWiseTag.DataBind();
                    ViewState["FacilityDept"] = dt;
                    lblSelFacilityCount.Text = "Total Records : " + dt.Rows.Count.ToString();
                }
                lblMessage.Text = "Record Delete";
                //Alert.ShowAjaxMsg("Record Delete", Page.Page);
                #endregion
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            grvFacilityWiseTag.DataSource = null;
            grvFacilityWiseTag.DataBind();
            ViewState["FacilityDept"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(hdnisSaveChk.Value) == 0)
            {
                Alert.ShowAjaxMsg("No Data to Save!!!", Page.Page);
                return;
            }
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            objFacility = new BaseC.clsFacility(sConString);

            strXML = new StringBuilder();
            coll = new ArrayList();
            if (common.myInt(ddlFacility.SelectedValue) == 0)
            {
                lblMessage.Text = "Please Select Facility !";
                return;
            }
            foreach (GridViewRow gvRow in grvFacilityWiseTag.Rows)
            {

                HiddenField hdnSelColumnId = (HiddenField)gvRow.FindControl("hdnSelColumnId");

                if (common.myInt(hdnSelColumnId.Value) > 0)
                {
                    coll.Add(common.myInt(hdnSelColumnId.Value));
                    strXML.Append(common.setXmlTable(ref coll));

                }
            }
            if (strXML.ToString() == "")
            {
                lblMessage.Text = "Please Select " + ddlTaggedFor.SelectedItem.Text + " !";
                return;
            }
            lblMessage.Text = "";

            string strmsg = objFacility.SaveFacilityWiseCommonMaster(common.myInt(Session["HospitalLocationId"]), strXML.ToString(), common.myInt(ddlFacility.SelectedValue), common.myBool(1), common.myInt(Session["UserId"]), common.myStr(ddlTaggedFor.SelectedItem.Attributes["TableType"]).Trim());

            if ((strmsg.Contains(" Update") || strmsg.Contains(" Save")) && !strmsg.Contains("usp"))
            {
                lblMessage.Text = strmsg;
                FillFacilityWiseData(common.myInt(ddlFacility.SelectedValue));
                Alert.ShowAjaxMsg(strmsg, Page.Page);
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
