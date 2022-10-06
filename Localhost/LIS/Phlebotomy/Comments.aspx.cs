using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Telerik.Web.UI;

public partial class LIS_Phlebotomy_Comments : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Request.QueryString["LabNo"] != null && Request.QueryString["RegNo"] != null && Request.QueryString["PName"] != null)
                {
                    this.lblPatientDetails.Text = HttpContext.GetGlobalResourceObject("PRegistration", "LABNO") + ": " + common.myStr(Request.QueryString["LabNo"]) + " | " + HttpContext.GetGlobalResourceObject("PRegistration", "regno") + ": " + common.myStr(Request.QueryString["RegNo"]) + " | Patient Name: " + common.myStr(Request.QueryString["PName"]).Trim();
                }
                if (common.myStr(Request.QueryString["CF"]) == "FNOTE")
                {
                    ddlSubDepartment.Visible = false;
                    lbl1.Visible = false;
                    ShowRemarks();
                }
                else
                {
                    ddlSubDepartment.Visible = true;
                    lbl1.Visible = true;
                    BindSubDepartment();
                }
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = ex.Message;
            }
        }
    }

    void BindSubDepartment()
    {
        try
        {
            BaseC.clsLISMaster objLisMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds = objLisMaster.getSubDepartmentByStationAndLabNo(common.myStr(Request.QueryString["Source"]), common.myInt(Session["StationId"]), common.myInt(Request.QueryString["LabNo"]));
            ddlSubDepartment.DataSource = ds.Tables[0];
            ddlSubDepartment.DataTextField = "SubName";
            ddlSubDepartment.DataValueField = "SubDeptId";
            ddlSubDepartment.DataBind();
            ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlSubDepartment.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlSubDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ShowRemarks();
    }

    void ShowRemarks()
    {
        try
        {
            //if (common.myInt(ddlSubDepartment.SelectedValue) > 0)
            //{
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = new DataSet();
            ds = objval.getDigInvResultRemarks(common.myStr(Request.QueryString["Source"]), common.myInt(Request.QueryString["LabNo"]), common.myInt(Session["StationId"]), common.myInt(ddlSubDepartment.SelectedValue), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtRemarks.Content = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]);
            }
            else
                txtRemarks.Content = string.Empty;
            //}
            //else
            //    txtRemarks.Content = string.Empty;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    bool SaveRemarks()
    {
        if (!String.IsNullOrEmpty(txtRemarks.Text.Trim()) && (common.myInt(ddlSubDepartment.SelectedValue) != 0 || common.myStr(Request.QueryString["CF"]) == "FNOTE"))
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            string Source = common.myStr(Request.QueryString["Source"]);
            int iLabNo = common.myInt(Request.QueryString["LabNo"]);
            int iSubDepartmentId = common.myInt(ddlSubDepartment.SelectedValue);
            string sRemarks = Convert.ToString(txtRemarks.Content);
            string strMsg = objval.SaveUpdateDiagInvResultRemarks(Source, iLabNo, iSubDepartmentId, sRemarks, common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]));
            if ((strMsg.Contains("Update") || strMsg.Contains("Save")) && !strMsg.Contains("usp"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;
                Session["CommentRefresh"] = "Y";
                return true;
            }
            else
                return false;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Sub Department And Enter Some Comments to Save!";
            return false;
        }
    }

    bool CancelRemarks()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        string Source = common.myStr(Request.QueryString["Source"]);
        int iLabNo = common.myInt(Request.QueryString["LabNo"]);
        string sRemarks = Convert.ToString(txtRemarks.Content);
        int iSubDepartmentId = common.myInt(ddlSubDepartment.SelectedValue);
        string strMsg = objval.cancelDigInvResultRemarks(Source, iLabNo, iSubDepartmentId, common.myInt(Session["UserId"]),common.myInt(Session["FacilityId"]));
        if (strMsg == "0")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = "Record cancelled successfully....";
            txtRemarks.Content = null;
            Session["CommentRefresh"] = "";
            return true;
        }
        else
            return false;
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (SaveRemarks() == true)
            {

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (CancelRemarks() == true)
            {

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnClose_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
    }
}
