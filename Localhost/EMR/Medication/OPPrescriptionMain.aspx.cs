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
using System.Data.SqlClient;
using Telerik.Web.UI;

public partial class EMR_OPPrescription_OPPrescriptionMain : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsPharmacy objPharmacy;
    BaseC.clsEMR objclsEMr;
    DAL.DAL dl = new DAL.DAL();
    private enum enumColumns : byte
    {
        Sno = 0,
        Generic = 1,
        Brand = 2,
        Unit = 3,
        TotalQty = 4,
        MonographCIMS = 5,
        InteractionCIMS = 6,
        DHInteractionCIMS = 7,
        MonographVIDAL = 8,
        InteractionVIDAL = 9,
        DHInteractionVIDAL = 10,
        Edit = 11,
        Delete = 12
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (Convert.ToString(Request.QueryString["Mpg"]) == "P168")
        if (common.myStr(Request.QueryString["OPIP"]) == "O")
        {
            ViewState["OPIP"] = "O";
        }
        else
        {

            ViewState["OPIP"] = "I";
        }



        if (common.myInt(Session["EncounterId"]) == 0)
        {
            Response.Redirect("/default.aspx?RegNo=0", false);
        }

        if (!IsPostBack)
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);

            ViewState["GridData"] = null;

            bindControl();
            bindData();
        }
    }

    private void bindControl()
    {
        try
        {
            OpPrescription OpPre = new OpPrescription(sConString);

            ddlStopOrder.DataSource = OpPre.GetCancelRemarks();
            ddlStopOrder.DataTextField = "Description";
            ddlStopOrder.DataValueField = "Id";
            ddlStopOrder.DataBind();

            ddlStopOrder.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlStopOrder.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void bindData()
    {
        try
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet ds;
            DataView dvConsumable = new DataView();
            DataView dvConsumable1 = new DataView();

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                ds = objPharmacy.getPreviousMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                    common.myInt(Session["EncounterId"]), 0, 0, "A", string.Empty, string.Empty, string.Empty);

                if (common.myBool(Request.QueryString["Consumable"]))
                {
                    dvConsumable = new DataView(ds.Tables[0]);
                    //dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ItemCategoryShortName IN ('LAC','MCS','SUR','DEF')";
                    dvConsumable1 = new DataView(ds.Tables[1]);
                }
                else
                {
                    dvConsumable = new DataView(ds.Tables[0]);
                    dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1  AND ItemCategoryShortName IN ('MED')";
                    dvConsumable1 = new DataView(ds.Tables[1]);
                }

            }
            else
            {
                objclsEMr = new BaseC.clsEMR(sConString);
                ds = objEMR.getOPMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                   common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0, "A");
                if (common.myBool(Request.QueryString["Consumable"]))
                {
                    dvConsumable = new DataView(ds.Tables[0]);
                    dvConsumable.RowFilter = "ISNULL(NurseCategory,0)=1 AND ItemCategoryShortName IN ('LAC','MCS','SUR','DEF')";
                    dvConsumable1 = new DataView(ds.Tables[1]);
                }
                else
                {
                    dvConsumable = new DataView(ds.Tables[0]);
                    dvConsumable.RowFilter = "ISNULL(DoctorCategory,0)=1  AND ItemCategoryShortName IN ('MED')";
                    dvConsumable1 = new DataView(ds.Tables[1]);
                }
                //dvConsumable = new DataView(ds.Tables[0]);
                //dvConsumable1 = new DataView(ds.Tables[1]);
            }


            ViewState["GridData"] = dvConsumable.ToTable();
            ViewState["GridDataDetail"] = dvConsumable1.ToTable();
            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].AcceptChanges();
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvPrevious.DataSource = dvConsumable.ToTable();
            gvPrevious.DataBind();

            ds.Dispose();
            dvConsumable.Dispose();
            dvConsumable1.Dispose();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvPrevious_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {
            if (e.CommandName == "ItemCancel")
            {
                int ItemId = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
                HiddenField hdnDetailsId = (HiddenField)row.FindControl("hdnDetailsId");
                //int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
                if ((hdnIndentId.Value == string.Empty || hdnIndentId.Value == "0") && ItemId == 0)
                {
                    return;
                }

                ViewState["IndentDetailsId"] = common.myInt(hdnDetailsId.Value).ToString();
                ViewState["StopItemId"] = common.myInt(ItemId).ToString();
                ViewState["StopIndentId"] = common.myStr(hdnIndentId.Value);

                txtStopRemarks.Text = string.Empty;
                dvConfirmStop.Visible = true;

                //TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");

                //Hashtable hshOutput = new Hashtable();

                //hshOutput = emr.StopCancelPreviousMedication(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]),
                //              Convert.ToInt32(hdnIndentId.Value), ItemId, Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["RegistrationId"]),
                //              Convert.ToInt32(Session["EncounterId"]), 1, txtRemarks.Text, Session["OPIP"].ToString());
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
            emr = null;
        }
    }
    protected void btnStopMedication_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            if (common.myLen(txtStopRemarks.Text) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Remarks can't be blank !";
                Alert.ShowAjaxMsg("Remarks can't be blank !", this);
                return;
            }

            if (common.myInt(ViewState["StopItemId"]) > 0 || common.myInt(ViewState["IndentDetailsId"]) > 0)
            {
                Hashtable hshOutput = new Hashtable();

                hshOutput = objEMR.StopCancelPreviousMedication(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                              common.myInt(ViewState["StopIndentId"]), common.myInt(ViewState["StopItemId"]), common.myInt(Session["UserId"]),
                              common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 1, txtStopRemarks.Text,
                              common.myStr(Session["OPIP"]), common.myInt(ViewState["IndentDetailsId"]));

                bindData();

                ViewState["IndentDetailsId"] = "0";
                if (common.myStr(hshOutput["@chvOutPut"]).ToUpper().Equals("SUCCESSFULL"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }

                lblMessage.Text = common.myStr(hshOutput["@chvOutPut"]);
            }
            else
            {
                Alert.ShowAjaxMsg("Drug not selected !", this);

                return;
            }

            ViewState["StopItemId"] = "";
            ViewState["StopIndentId"] = "";

            dvConfirmStop.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
        }
    }
    protected void btnStopClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            dvConfirmStop.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    protected void gvPrevious_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label TotalQty = (Label)e.Row.FindControl("lblTotalQty");
                TotalQty.Text = common.myDbl(TotalQty.Text).ToString("F2");
                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
                HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
                HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");

                if (ViewState["GridDataDetail"] != null)
                {
                    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
                    DataTable dt = (DataTable)ViewState["GridDataDetail"];
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value) + " AND ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv.ToTable());
                    }
                    else
                    {
                        DataView dv1 = new DataView(dt);
                        dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnIndentId.Value);
                        if (dv1.ToTable().Rows.Count > 0)
                        {
                            lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv1.ToTable());
                        }
                    }
                    dt.Dispose();
                    dv.Dispose();
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
    private string getMonographXML(string CIMSType, string CIMSItemId)
    {
        string strXML = "";
        try
        {
            CIMSType = (CIMSType == "") ? "Product" : CIMSType;

            strXML = "<Request><Content><" + CIMSType + " reference=\"" + CIMSItemId + "\" /></Content></Request>";
        }
        catch
        { }

        return strXML;
    }
    protected void btnStopOrder_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "";

            if (common.myInt(ddlStopOrder.SelectedValue) == 0)
            {
                lblMessage.Text = "Reason for stop order not selected !";
                return;
            }

            string strIds = "";

            int rowNo = 0;
            foreach (GridViewRow dataItem in gvPrevious.Rows)
            {
                CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                if (chkRow.Checked)
                {
                    HiddenField hdnDetailsId = (HiddenField)dataItem.FindControl("hdnDetailsId");
                    if (common.myInt(hdnDetailsId.Value) == 0)
                    {
                        continue;
                    }

                    Label lblQty = (Label)dataItem.FindControl("lblQty");
                    Label lblIssueQty = (Label)dataItem.FindControl("lblIssueQty");

                    if (common.myDbl(lblQty.Text) == common.myDbl(lblIssueQty.Text))
                    {
                        lblMessage.Text = "Fully issued quantity not allow to stop at serial no " + ++rowNo;
                        return;
                    }

                    if (strIds != "")
                    {
                        strIds += ",";
                    }
                    strIds += common.myInt(hdnDetailsId.Value);
                }
            }

            if (strIds == "")
            {
                lblMessage.Text = "Record not selected !";
                return;
            }

            objPharmacy = new BaseC.clsPharmacy(sConString);
            string strMsg = objPharmacy.stopPrescription(strIds, common.myInt(ddlStopOrder.SelectedValue), common.myInt(Session["UserId"]));

            if (strMsg.Contains("Succeeded"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }

            bindData();

            lblMessage.Text = "Stop Order " + strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnReOrder_Onclick(object sender, EventArgs e)
    {
        string sIndentIds = "", sItemIds = "", sIndentId = "", sItemId = "";
        int countIndentId = 0, countItemId = 0;
        foreach (GridViewRow row in gvPrevious.Rows)
        {
            HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
            HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");
            CheckBox chkRow = (CheckBox)row.FindControl("chkRow");
            if (chkRow.Checked == true)
            {
                if (hdnIndentId.Value != sIndentId)
                {
                    sIndentId = hdnIndentId.Value;
                    sIndentIds = countIndentId == 0 ? sIndentId : sIndentIds + "," + hdnIndentId.Value;
                    countIndentId++;
                }
                if (hdnItemId.Value != sItemId)
                {
                    sItemId = hdnItemId.Value;
                    sItemIds = countItemId == 0 ? sItemId : sItemIds + "," + hdnItemId.Value;
                    countItemId++;
                }
            }
        }
        if (sIndentId == "" || sItemIds == "")
        {
            Alert.ShowAjaxMsg("Please select Item to Reorder", Page);
            return;
        }
        hdnPageIndentIds.Value = sIndentIds;
        hdnPageItemIds.Value = sItemIds;

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }

}
