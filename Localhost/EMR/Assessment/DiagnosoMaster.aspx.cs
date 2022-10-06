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

public partial class EMR_Assessment_DiagnosoMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    BaseC.ParseData Parse = new BaseC.ParseData();
    BaseC.DiagnosisDA objDiag;
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDrpCategory();
            BindSubCategory(hdngroupid.Value);
            BindDisease("HD");
            BindGrid();
        }

    }

    // Populating Category Dropdown
    private void BindDrpCategory()
    {
        try
        {

            objDiag = new BaseC.DiagnosisDA(sConString);
            DataTable dt = new DataTable();
            ds = new DataSet();
            ds = objDiag.BindCategory();
            DataView dv = new DataView(ds.Tables[0]);

            dv.RowFilter = "GroupId=18";

            if (dv.Count > 0)
            {
                dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    hdngroupid.Value = dt.Rows[0]["GroupId"].ToString();
                    txtGroup.Text = dt.Rows[0]["GroupName"].ToString();
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

    private void BindSubCategory(string GroupId)
    {
        try
        {

            if (GroupId != "")
            {
                objDiag = new BaseC.DiagnosisDA(sConString);
                DataTable dt = new DataTable();
                ds = new DataSet();
                ds = objDiag.BindSubCategory(common.myStr(GroupId));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnsubgroupid.Value = ds.Tables[0].Rows[0]["SubGroupId"].ToString().Trim();
                    txtSubgroup.Text = ds.Tables[0].Rows[0]["SubGroupName"].ToString().Trim();

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
    private void BindDisease(string Code)
    {
        try
        {

            if (Code != "")
            {
                objDiag = new BaseC.DiagnosisDA(sConString);
                DataTable dt = new DataTable();
                ds = new DataSet();
                ds = objDiag.BindDisease(common.myStr(Code));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdndiseaseid.Value = ds.Tables[0].Rows[0]["DiseaseId"].ToString().Trim();
                    txtDisease.Text = ds.Tables[0].Rows[0]["DiseaseName"].ToString().Trim();

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
    private void BindGrid()
    {
        try
        {
            objDiag = new BaseC.DiagnosisDA(sConString);
            DataTable dt = new DataTable();
            ds = new DataSet();
            ds = objDiag.BindHospitalDisease("HD");
            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                ds.Tables[0].AcceptChanges();
            }
            gvDetails.DataSource = ds;
            gvDetails.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

   
    protected void gvDetails_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {

            Label lblICDCode = (Label)e.Item.FindControl("lblICDCode");
            HiddenField hdnICDId = (HiddenField)e.Item.FindControl("hdnICDId");
            HiddenField hdnGroupId = (HiddenField)e.Item.FindControl("hdnGroupId");
            HiddenField hdnSubGroupId = (HiddenField)e.Item.FindControl("hdnSubGroupId");
            HiddenField hdnDiseaseId = (HiddenField)e.Item.FindControl("hdnDiseaseId");
            Label lblDescription = (Label)e.Item.FindControl("lblDescription");
            HiddenField hdnActive = (HiddenField)e.Item.FindControl("hdnActive");


            ViewState["IcdId"] = hdnICDId.Value.Trim();
            txtIcdCode.Text = lblICDCode.Text.Trim();
            txtSubDisease.Text = lblDescription.Text.Trim();
            if (common.myStr(hdnActive.Value) == "False")
            {
                // ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue(common.myStr(hdnActive.Value)));
                ddlStatus.SelectedIndex = 1;
            }
            else
            {
                ddlStatus.SelectedIndex = 0;
            }
            btnSave.Text = "Update";
        }
    }
    protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (gvDetails != null)
            //{
            //    if (gvDetails.SelectedIndex > -1)
            //    {
            //        Label lblICDCode = (Label)gvDetails.SelectedRow.Cells[Convert.ToByte(0)].FindControl("lblICDCode");
            //        HiddenField hdnICDId = (HiddenField)gvDetails.SelectedRow.Cells[Convert.ToByte(0)].FindControl("hdnICDId");
            //        HiddenField hdnGroupId = (HiddenField)gvDetails.SelectedRow.Cells[Convert.ToByte(0)].FindControl("hdnGroupId");
            //        HiddenField hdnSubGroupId = (HiddenField)gvDetails.SelectedRow.Cells[Convert.ToByte(0)].FindControl("hdnSubGroupId");
            //        HiddenField hdnDiseaseId = (HiddenField)gvDetails.SelectedRow.Cells[Convert.ToByte(0)].FindControl("hdnDiseaseId");
            //        Label lblDescription = (Label)gvDetails.SelectedRow.Cells[Convert.ToByte(1)].FindControl("lblDescription");
            //        HiddenField hdnActive = (HiddenField)gvDetails.SelectedRow.Cells[Convert.ToByte(2)].FindControl("hdnActive");
                    

            //        ViewState["IcdId"] = hdnICDId.Value.Trim(); 
            //        txtIcdCode.Text = lblICDCode.Text.Trim();
            //        txtSubDisease.Text = lblDescription.Text.Trim();
            //        if (common.myStr(hdnActive.Value) == "False")
            //        {
            //            // ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindItemByValue(common.myStr(hdnActive.Value)));
            //            ddlStatus.SelectedIndex = 1;
            //        }
            //        else
            //        {
            //            ddlStatus.SelectedIndex = 0;
            //        }
            //        btnSave.Text = "Update";
            //    }
           // }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        btnSave.Text = "Save";
        ClearData();
        BindGrid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objDiag = new BaseC.DiagnosisDA(sConString);
            dl=new DAL.DAL(DAL.DAL.DBType.SqlServer,sConString);
            ds = new DataSet();
            string strsave = "";
            if (btnSave.Text == "Save")
            {
                string strsql = "select ICDCode ,Description  from ICD9SubDisease WHERE ICDCode='" +  common.myStr(txtIcdCode.Text) + "'";
                ds = dl.FillDataSet(CommandType.Text, strsql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "This ICD Code - " + ds.Tables[0].Rows[0]["ICDCode"].ToString() + " and Disease - " + ds.Tables[0].Rows[0]["Description"].ToString() + " already exists!";
                    return;
                }
                               
                strsave = objDiag.SaveUpdateDiagnosismaster(0, common.myInt(hdngroupid.Value), common.myInt(hdnsubgroupid.Value), common.myInt(hdndiseaseid.Value), common.myStr(txtIcdCode.Text.ToUpper()), common.myStr(txtSubDisease.Text), 1);
                if (strsave.Contains("Save"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strsave;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strsave;
                }
                ClearData();
                BindGrid();
            }
            else
            {
                strsave = objDiag.SaveUpdateDiagnosismaster(common.myInt(ViewState["IcdId"]), common.myInt(hdngroupid.Value), common.myInt(hdnsubgroupid.Value), common.myInt(hdndiseaseid.Value), common.myStr(txtIcdCode.Text), common.myStr(txtSubDisease.Text), common.myInt(ddlStatus.SelectedValue));
                if (strsave.Contains("Update"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strsave;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strsave;
                }
                ClearData();
                BindGrid();
                btnSave.Text = "Save";
            }
        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void ClearData()
    {
        txtIcdCode.Text = "";
        txtSubDisease.Text = "";
        ddlStatus.SelectedIndex = 0;
    }
    protected void gvDetails_PreRender(object sender, EventArgs e)
    {
        BindGrid();
    }
}
