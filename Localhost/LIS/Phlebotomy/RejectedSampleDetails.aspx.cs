using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class LIS_Phlebotomy_RejectedSampleDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    private enum GridColgvData : byte
    {
        col0 = 0,
        COL1 = 1,
        COL2 = 2,
        COL3 = 3,
        COL4 = 4,
        COL5 = 5,
        COL6 = 6,
        COL7 = 7,
        COL8 = 8,
        COL9 = 9
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            ViewState["CallFrom"] = common.myInt(Request.QueryString["CallFrom"]);
            //bindWard();
            dtpDatefrom.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpDateto.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpDatefrom.SelectedDate = DateTime.Now;
            // dtpDatefrom.SelectedDate = common.myStr(Request.QueryString["FromDate"]) != "" ? common.myDate(Request.QueryString["FromDate"]) : DateTime.Now.AddMonths(-1);
            dtpDateto.SelectedDate = DateTime.Now;
            //ddlWard.Visible = false;
            txtSearchRegNo.Visible = false;
            txtSearch.Visible = false;

            switch (common.myStr(Request.QueryString["SearchCriteria"]))
            {
                case "R":
                case "N":
                    txtSearchRegNo.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr("R");
                    txtSearchRegNo.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "ENC":
                case "P":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
            }

            bindDetails(common.myInt(ViewState["CallFrom"]));

            if (common.myStr(Request.QueryString["SOURCE"]).Equals("OPD"))
            {
                gvDetails.Columns[(byte)GridColgvData.COL8].Visible = false;
                gvDetails.Columns[(byte)GridColgvData.COL9].Visible = false;
                gvDetails.Columns[(byte)GridColgvData.COL1].Visible = true;
                lblHeader.Text = common.myStr(lblHeader.Text) + " ( OPD ) ";

            }
            else if (common.myStr(Request.QueryString["SOURCE"]).Equals("IPD"))
            {
                gvDetails.Columns[(byte)GridColgvData.COL8].Visible = true;
                gvDetails.Columns[(byte)GridColgvData.COL9].Visible = true;
                gvDetails.Columns[(byte)GridColgvData.COL1].Visible = false;
                lblHeader.Text = common.myStr(lblHeader.Text) + " ( IPD ) ";
            }

        }
    }


    protected void ddlSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlWard.Visible = false;
        txtSearchRegNo.Visible = false;
        txtSearch.Visible = false;

        switch (common.myStr(ddlSearchCriteria.SelectedValue))
        {
            //case "W":
            //    ddlWard.Visible = true;
            //    break;
            case "R":
                txtSearchRegNo.Visible = true;
                break;
            case "ENC":
                txtSearch.Visible = true;
                break;
            case "P":
                txtSearch.Visible = true;
                break;
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        bindDetails(common.myInt(ViewState["CallFrom"]));
    }
    private void bindDetails(int CallFrom)
    {
        DataSet ds = new DataSet();
        BaseC.clsLISPhlebotomy objLIS = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
            //int WardId = 0;
            int RegistrationNo = 0;
            string EncounterNo = string.Empty;
            string PatientName = string.Empty;
            switch (common.myStr(ddlSearchCriteria.SelectedValue))
            {
                case "R":
                    RegistrationNo = common.myInt(txtSearchRegNo.Text.Trim());
                    break;
                case "ENC":
                    EncounterNo = txtSearch.Text.Trim();
                    break;
                case "P":
                    PatientName = txtSearch.Text.Trim();
                    break;
            }
            ds = objLIS.getInvRejectedSamples(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myStr(Request.QueryString["SOURCE"]),
                common.myDate(dtpDatefrom.SelectedDate), common.myDate(dtpDateto.SelectedDate), CallFrom, RegistrationNo, EncounterNo, PatientName);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (common.myLen(Request.QueryString["WardName"]) > 0)
                        {
                            DataView DV = ds.Tables[0].Copy().DefaultView;
                            DV.RowFilter = "WardName like '%" + common.myStr(Request.QueryString["WardName"]) + "%'";

                            ds = new DataSet();
                            ds.Tables.Add(DV.ToTable().Copy());
                            DV.Dispose();
                        }

                        gvDetails.DataSource = ds.Tables[0];
                        gvDetails.DataBind();
                    }
                    else
                    {
                        BindBlankItemGrid();
                    }
                }
                else
                {
                    BindBlankItemGrid();
                }
            }
            else
            {
                BindBlankItemGrid();
            }

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            //objException.HandleException(Ex);
        }
        finally
        {
            //ds.Dispose();
            objLIS = null;
        }
    }

    private void BindBlankItemGrid()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        try
        {

            dr["FacilityName"] = string.Empty;
            dr["Source"] = string.Empty;
            dr["LabNo"] = string.Empty;
            dr["RegistrationId"] = string.Empty;
            dr["EncounterId"] = string.Empty;
            dr["EncounterNo"] = string.Empty;
            dr["RegistrationNo"] = string.Empty;
            dr["RegistrationNo"] = string.Empty;
            dr["OrderDate"] = string.Empty;
            dr["ServiceName"] = string.Empty;
            dr["PatientName"] = string.Empty;
            dr["AgeGender"] = string.Empty;
            dr["BedNo"] = string.Empty;
            dr["WardName"] = string.Empty;
            dr["ReferredBy"] = string.Empty;
            dr["TowhomInformed"] = string.Empty;
            dr["CancelRemarks"] = string.Empty;
            dr["IsACK"] = false;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvDetails.DataSource = dt;
            gvDetails.DataBind();

        }

        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
        finally
        {
            dt.Dispose();
        }
    }

    protected DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("FacilityName", typeof(string));
            dt.Columns.Add("Source", typeof(string));
            dt.Columns.Add("LabNo", typeof(string));
            dt.Columns.Add("RegistrationId", typeof(string));
            dt.Columns.Add("EncounterId", typeof(string));
            dt.Columns.Add("EncounterNo", typeof(string));
            dt.Columns.Add("RegistrationNo", typeof(string));
            dt.Columns.Add("OrderDate", typeof(string));
            dt.Columns.Add("ServiceName", typeof(string));
            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("AgeGender", typeof(string));
            dt.Columns.Add("BedNo", typeof(string));
            dt.Columns.Add("WardName", typeof(string));
            dt.Columns.Add("ReferredBy", typeof(string));
            dt.Columns.Add("TowhomInformed", typeof(string));
            dt.Columns.Add("CancelRemarks", typeof(string));
            dt.Columns.Add("DiagSampleId", typeof(int));
            dt.Columns.Add("ACKByName", typeof(string));
            dt.Columns.Add("IsACK", typeof(bool));

            return dt;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
            return dt;
        }

    }

    protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDetails.PageIndex = e.NewPageIndex;
        bindDetails(common.myInt(ViewState["CallFrom"]));
    }

    protected void btnACK_Click(object sender, EventArgs e)
    {

        try
        {
            IsValidPassword();
        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordCheckerV1.aspx?UseFor=OPIP";
        RadWindow1.Height = 120;
        RadWindow1.Width = 340;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnIsValidPassword_OnClick(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        if (!common.myBool(hdnIsValidPassword.Value))
        {
            lblMessage.Text = "Invalid Password !";
            return;
        }
        if (common.myInt(hdnIsValidPassword.Value).Equals(1))
        {
            lblMessage.Text = "";
            SaveRecords();
            bindDetails(common.myInt(ViewState["CallFrom"]));
        }
    }

    public void SaveRecords()
    {

        BaseC.clsLISPhlebotomy objLIS = new BaseC.clsLISPhlebotomy(sConString);
        StringBuilder strXML = new StringBuilder();
        try
        {


            foreach (GridViewRow grow in gvDetails.Rows)
            {
                CheckBox chkIsACK = (CheckBox)grow.FindControl("chkIsACK");
                HiddenField hdnDiagSampleId = (HiddenField)grow.FindControl("hdnDiagSampleId");

                ArrayList coll = new ArrayList();

                if (chkIsACK.Visible && chkIsACK.Checked)
                {
                    coll.Add(common.myInt(hdnDiagSampleId.Value));
                    strXML.Append(common.setXmlTable(ref coll));
                }
            }

            string SuceessStatus = objLIS.ACKRejectedSample(
                              common.myInt(Session["HospitalLocationID"]),
                              common.myInt(Session["FacilityId"]), common.myStr(strXML), common.myInt(Session["EmployeeId"]));

            if (SuceessStatus.ToUpper().Contains("ACK"))
            {
                lblMessage.Text = SuceessStatus;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            else
            {
                lblMessage.Text = SuceessStatus;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
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

            objLIS = null;
        }
    }

    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkIsACK = (CheckBox)e.Row.FindControl("chkIsACK");

				chkIsACK.Visible = !chkIsACK.Checked;

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
