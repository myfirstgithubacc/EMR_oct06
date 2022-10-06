using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class MRD_Master_MRDReportAgeRange : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SetGrid();
        }
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    private void SetGrid()
    {
        BaseC.clsLISPhlebotomy oclsLISPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);
        gvAgeRange.DataSource = oclsLISPhlebotomy.GetAgeRange();
        gvAgeRange.DataBind();
    }
    protected void gvAgeRange_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvAgeRange.EditIndex = e.NewEditIndex;
        SetGrid();
    }
    protected void gvAgeRange_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox txtfrom = (TextBox)gvAgeRange.Rows[e.RowIndex].FindControl("txtagefrom");
        TextBox txtto = (TextBox)gvAgeRange.Rows[e.RowIndex].FindControl("txtageto");
        HiddenField hdnid = (HiddenField)gvAgeRange.Rows[e.RowIndex].FindControl("hdnId");
        CheckBox chkboxActive = (CheckBox)gvAgeRange.Rows[e.RowIndex].FindControl("chkboxActive");

        int AgeFrom = common.myInt(txtfrom.Text);
        int AgeTo = common.myInt(txtto.Text);

        if (AgeFrom > AgeTo)
        {
            lblMessage.Text = "[From Age] can not be greater than [To Age]";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            return;
        }

        BaseC.clsLISPhlebotomy oclsLISPhlebotomy = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
            lblMessage.Text = oclsLISPhlebotomy.UpdateMRDReportAgeRange(common.myInt(hdnid.Value), AgeFrom, AgeTo, chkboxActive.Checked);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            gvAgeRange.EditIndex = -1;
            SetGrid();
            
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }

    protected void gvAgeRange_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvAgeRange.EditIndex = -1;
        SetGrid();
    }
}
