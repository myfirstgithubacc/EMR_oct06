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

public partial class WardManagement_AckByWard : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.Page.Title = "Acknowledge by Ward Details";
          
            txtSearchRegNo.Visible = false;
            txtSearch.Visible = false;

            switch (common.myStr(Request.QueryString["SearchCriteria"]))
            {
                case "R":
                    txtSearchRegNo.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearchRegNo.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "ENC":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text= common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "N":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
            }
            fetchData();
        }

    }

    protected void gvAckByWardDetails_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAckByWardDetails.PageIndex = e.NewPageIndex;
        BindgvAckByWardDetails();
    }

    protected void ddlSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtSearchRegNo.Visible = false;
        txtSearch.Visible = false;

        switch (common.myStr(ddlSearchCriteria.SelectedValue))
        {
            case "R":
                txtSearchRegNo.Visible = true;
                break;
            case "ENC":
                txtSearch.Visible = true;
                break;
            case "N":
                txtSearch.Visible = true;
                break;
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            fetchData();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void fetchData()
    {
        try
        {
            BindgvAckByWardDetails();

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

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        ddlSearchCriteria.SelectedIndex = 0;
        txtSearchRegNo.Text = string.Empty;
        txtSearch.Text = string.Empty;
        fetchData();
    }

    private void BindgvAckByWardDetails()
    {
        BaseC.WardManagement objWM = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        int WardId = common.myInt(Request.QueryString["wardid"]);
        int RegistrationNo = 0;
        string EncounterNo = string.Empty;
        string PatientName = string.Empty;
        try
        {
            switch (common.myStr(ddlSearchCriteria.SelectedValue))
            {
                case "R":
                    RegistrationNo = common.myInt(txtSearchRegNo.Text.Trim());
                    break;
                case "ENC":
                    EncounterNo = txtSearch.Text.Trim();
                    break;
                case "N":
                    PatientName = txtSearch.Text.Trim();
                    break;
            }

            ds = objWM.getAckByWarkDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), EncounterNo, WardId, RegistrationNo, PatientName);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblNoOfPatient.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
                        gvAckByWardDetails.DataSource = ds.Tables[0];
                        gvAckByWardDetails.DataBind();
                    }
                    else
                    {
                        BindBlankAckByWardDetails();
                    }
                }
                else
                {
                    BindBlankAckByWardDetails();
                }
            }
            else
            {
                BindBlankAckByWardDetails();
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
            objWM = null;
        }
    }

    private void BindBlankAckByWardDetails()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        try
        {
            dr["EncodedDate"] = string.Empty;
            dr["RegistrationNo"] = string.Empty;
            dr["EncounterNo"] = string.Empty;
            dr["PatientName"] = string.Empty;
            dr["AgeGender"] = string.Empty;
            dr["WardName"] = string.Empty;
            dr["Fromward"] = string.Empty;
            dr["FromBedNo"] = string.Empty;
            dr["Toward"] = string.Empty;
            dr["ToBedNo"] = string.Empty;
            dr["RequestRemarks"] = string.Empty;
            dr["Id"] = 0;
            dr["EncounterId"] = 0;
           
            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvAckByWardDetails.DataSource = dt;
            gvAckByWardDetails.DataBind();

            //GridViewRow gv1 = gvAckByWardDetails.Rows[0];
           
            //ImageButton ibtnDelete = (ImageButton)gv1.FindControl("ibtnDelete");
            //ibtnDelete.Enabled = false;
            //ibtnDelete.Visible = false;
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
            dt.Columns.Add("EncodedDate", typeof(string));
            dt.Columns.Add("RegistrationNo", typeof(string));
            dt.Columns.Add("EncounterNo", typeof(string));
            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("AgeGender", typeof(string));
            dt.Columns.Add("WardName", typeof(string));
            dt.Columns.Add("Fromward", typeof(string));
            dt.Columns.Add("FromBedNo", typeof(string));
            dt.Columns.Add("Toward", typeof(string));
            dt.Columns.Add("ToBedNo", typeof(string));
            dt.Columns.Add("RequestRemarks", typeof(string));
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("EncounterId", typeof(int));
        
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

    protected void gvAckByWardDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Details")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //int EncounterId = common.myInt(((HiddenField)row.FindControl("hdnEncounterId")).Value.ToString());
                int Id = common.myInt(((HiddenField)row.FindControl("hdnId")).Value.ToString());
                //ViewState["EncounterId"] = EncounterId;
                ViewState["Id"] = Id;
                foreach (GridViewRow rownew in gvAckByWardDetails.Rows)
                {
                    rownew.BackColor = System.Drawing.Color.FromName("White");
                }
                row.BackColor = System.Drawing.Color.FromName("LightSkyBlue");

                GetRemarks(Id);
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void GetRemarks(int id)
    {
        BaseC.ATD oATD = new BaseC.ATD(sConString);
        DataSet oDATASET = new DataSet();
        oDATASET = oATD.getRemarksBedTransferRequest(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), id);
        if (oDATASET.Tables[0].Rows.Count > 0) { txtRemark.Text = common.myStr(oDATASET.Tables[0].Rows[0]["Remarks"]); }
    }

    protected void btnAck_OnClick(object sender, EventArgs e)
    {
        try
        {
            BaseC.ATD oATD = new BaseC.ATD(sConString);
            int iId = common.myInt(ViewState["Id"]);
            string sRemarks = common.myStr(txtRemark.Text);
            int iReturn = oATD.UpdateACKByWard(iId, sRemarks);
            if (iReturn == 0) { lblMessage.Visible = true; lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor); lblMessage.Text = "Remarks Updated !"; txtRemark.Text = ""; }
            oATD = null;
            fetchData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


}
