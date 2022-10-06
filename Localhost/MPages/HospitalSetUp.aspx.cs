using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Configuration;

public partial class MPages_HospitalSetUp : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData Parse = new BaseC.ParseData();

    private enum enumCol : byte
    {
        Description = 0,
        Value = 1
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Session["UserID"] != null)
        {
            this.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (!IsPostBack)
            {
                btnSave.Enabled = false;

                bindFacility();
                bindModule();

                if (Request.QueryString["Tr"] == null)
                {
                    if (Session["HospitalLocationID"] != null)
                    {
                        BindDetails();
                        btnSave.Enabled = true;
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

    void BindDetails()
    {
        BaseC.HospitalSetup objHS = new BaseC.HospitalSetup(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objHS.getHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue),
                                    string.Empty, common.myInt(ddlModule.SelectedValue));

            if (ds.Tables.Count > 1)
            {
                ViewState["HospitalSetupDetailsData"] = ds.Tables[1];
            }

            gvDetails.DataSource = ds.Tables[0];
            gvDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objHS = null;
            ds.Dispose();
        }
    }

    protected void Save_OnClick(Object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

        BaseC.HospitalSetup objHS = new BaseC.HospitalSetup(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        string strVal = string.Empty;

        try
        {
            if (common.myInt(ddlFacility.SelectedValue).Equals(0))
            {
                lblMessage.Text = "Facillity not selected !";
                return;
            }

            foreach (GridViewRow dataItem in gvDetails.Rows)
            {
                HiddenField hdnFlag = (HiddenField)dataItem.FindControl("hdnFlag");

                if (common.myLen(hdnFlag.Value) > 0)
                {
                    HiddenField hdnFlagType = (HiddenField)dataItem.FindControl("hdnFlagType");

                    Label lblDescription = (Label)dataItem.FindControl("lblDescription");

                    TextBox txtT = (TextBox)dataItem.FindControl("txtT");
                    TextBox txtN = (TextBox)dataItem.FindControl("txtN");
                    DropDownList DDL = (DropDownList)dataItem.FindControl("DDL");
                    RadioButtonList RDO = (RadioButtonList)dataItem.FindControl("RDO");

                    strVal = string.Empty;

                    switch (common.myStr(hdnFlagType.Value))
                    {
                        case "T":
                            strVal = common.myStr(txtT.Text).Trim();

                            break;
                        case "N":
                            strVal = common.myStr(txtN.Text).Trim();

                            break;
                        case "D":
                            strVal = common.myStr(DDL.SelectedValue);

                            break;
                        case "B":
                            strVal = common.myStr(RDO.SelectedValue);

                            break;
                    }

                    if (common.myLen(strVal).Equals(0))
                    {
                        lblMessage.Text = "Please define value of [" + common.myStr(lblDescription.Text) + "]";
                        return;
                    }

                    coll.Add(common.myStr(hdnFlag.Value));
                    coll.Add(strVal);

                    strXML.Append(common.setXmlTable(ref coll));
                }
            }

            objHS.HospitalLocationID = Convert.ToInt16(common.myInt(Session["HospitalLocationID"]));
            objHS.idFacility = Convert.ToInt16(common.myInt(ddlFacility.SelectedValue));
            objHS.XMLHospitalFlags = strXML.ToString();
            objHS.EncodedBy = common.myInt(Session["UserID"]);

            string ErrorStatus = objHS.SaveHospitalSetup();

            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (ErrorStatus == "Record Has Been Updated!")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }

            lblMessage.Text = ErrorStatus;

            BindDetails();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objHS = null;
            strXML = null;
            coll = null;
            strVal = string.Empty;
        }
    }

    protected void bindFacility()
    {
        BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objEMRFacility.getFacility(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

            ddlFacility.DataSource = ds;
            ddlFacility.DataTextField = "Name";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();

            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myInt(Session["FacilityId"]).ToString()));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMRFacility = null;
            ds.Dispose();
        }
    }

    protected void bindModule()
    {
        BaseC.HospitalSetup objHS = new BaseC.HospitalSetup(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objHS.getHostitalSetupModules();

            ddlModule.DataSource = ds.Tables[0];
            ddlModule.DataTextField = "ModuleName";
            ddlModule.DataValueField = "ModuleId";
            ddlModule.DataBind();

            ddlModule.SelectedIndex = 0;
            ddlModule.Enabled = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objHS = null;
            ds.Dispose();
        }
    }


    //if (lblFlag.Text == "EditMachineResultLevel")
    //ddlValue.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
    //ddlValue.Items.Insert(1, new RadComboBoxItem("MACHINE", "M"));
    //ddlValue.Items.Insert(2, new RadComboBoxItem("USER", "U"));

    //if (lblFlag.Text == "MachineResultManualEntry")
    //ddlValue.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
    //ddlValue.Items.Insert(1, new RadComboBoxItem("YES", "YES"));
    //ddlValue.Items.Insert(2, new RadComboBoxItem("NO", "NO"));

    //if (lblFlag.Text == "WeeklyOffDayName")
    //ddlValue.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
    //ddlValue.Items.Insert(1, new RadComboBoxItem("MONDAY", "MONDAY"));
    //ddlValue.Items.Insert(2, new RadComboBoxItem("TUESDAY", "TUESDAY"));
    //ddlValue.Items.Insert(3, new RadComboBoxItem("WEDNESDAY", "WEDNESDAY"));
    //ddlValue.Items.Insert(4, new RadComboBoxItem("THURSDAY", "THURSDAY"));
    //ddlValue.Items.Insert(5, new RadComboBoxItem("FRIDAY", "FRIDAY"));
    //ddlValue.Items.Insert(6, new RadComboBoxItem("SATURDAY", "SATURDAY"));
    //ddlValue.Items.Insert(7, new RadComboBoxItem("SUNDAY", "SUNDAY"));

    protected void gvDetails_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        DataTable dtHospitalSetupDetailsData = new DataTable();
        DataView DV = new DataView();
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnFlag = (HiddenField)e.Row.FindControl("hdnFlag");
                HiddenField hdnFlagType = (HiddenField)e.Row.FindControl("hdnFlagType");
                HiddenField hdnValue = (HiddenField)e.Row.FindControl("hdnValue");

                TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                TextBox txtN = (TextBox)e.Row.FindControl("txtN");
                DropDownList DDL = (DropDownList)e.Row.FindControl("DDL");
                RadioButtonList RDO = (RadioButtonList)e.Row.FindControl("RDO");

                txtT.Visible = false;
                txtN.Visible = false;
                DDL.Visible = false;
                RDO.Visible = false;

                switch (common.myStr(hdnFlagType.Value))
                {
                    case "T":
                        txtT.Visible = true;
                        txtT.Text = common.myStr(hdnValue.Value).Trim();

                        break;
                    case "N":
                        txtN.Visible = true;
                        txtN.Text = common.myStr(hdnValue.Value).Trim();

                        break;
                    case "D":
                        DDL.Visible = true;

                        if (ViewState["HospitalSetupDetailsData"] != null)
                        {
                            dtHospitalSetupDetailsData = (DataTable)ViewState["HospitalSetupDetailsData"];
                            if (dtHospitalSetupDetailsData != null)
                            {
                                if (dtHospitalSetupDetailsData.Rows.Count > 0)
                                {
                                    DV = dtHospitalSetupDetailsData.DefaultView;
                                    DV.RowFilter = "Flag='" + common.myStr(hdnFlag.Value) + "'";

                                    DDL.DataSource = DV.ToTable();
                                    DDL.DataValueField = "ValueId";
                                    DDL.DataTextField = "ValueName";
                                    DDL.DataBind();

                                    DDL.SelectedIndex = DDL.Items.IndexOf(DDL.Items.FindByValue(common.myStr(hdnValue.Value).ToString()));
                                }
                            }
                        }
                        break;
                    case "B":
                        RDO.Visible = true;

                        if (ViewState["HospitalSetupDetailsData"] != null)
                        {
                            dtHospitalSetupDetailsData = (DataTable)ViewState["HospitalSetupDetailsData"];
                            if (dtHospitalSetupDetailsData != null)
                            {
                                if (dtHospitalSetupDetailsData.Rows.Count > 0)
                                {
                                    DV = dtHospitalSetupDetailsData.DefaultView;
                                    DV.RowFilter = "Flag='" + common.myStr(hdnFlag.Value) + "'";

                                    RDO.DataSource = DV.ToTable();
                                    RDO.DataValueField = "ValueId";
                                    RDO.DataTextField = "ValueName";
                                    RDO.DataBind();

                                    RDO.SelectedIndex = RDO.Items.IndexOf(RDO.Items.FindByValue(common.myStr(hdnValue.Value).ToString()));
                                }
                            }
                        }
                        break;
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

    protected void ddlFacility_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDetails();
    }

    protected void ddlModule_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDetails();
    }

    //protected void DDL_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DropDownList ddl = sender as DropDownList;
    //        if (ddl == null)
    //        {
    //            return;
    //        }
    //        GridViewRow row = ddl.NamingContainer as GridViewRow;
    //        HiddenField hdnValue = (HiddenField)row.Cells[(byte)enumCol.Value].FindControl("hdnValue");

    //        hdnValue.Value = common.myStr(ddl.SelectedValue);
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    //protected void RDO_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadioButtonList rdo = sender as RadioButtonList;
    //        if (rdo == null)
    //        {
    //            return;
    //        }
    //        GridViewRow row = rdo.NamingContainer as GridViewRow;
    //        HiddenField hdnValue = (HiddenField)row.Cells[(byte)enumCol.Value].FindControl("hdnValue");

    //        hdnValue.Value = common.myStr(rdo.SelectedValue);
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}


}
