using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using BaseC;
using System.Web.UI;

public partial class LIS_Phlebotomy_AddendumProgressNote : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MP"]).ToUpper().Equals("NO"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //dvConfirmCancelOptions.Visible = false;
            hdnProgressNoteID.Value = common.myStr(Request.QueryString["PNID"]);
            if (common.myInt(hdnProgressNoteID.Value) ==0 || common.myInt(Session["EmployeeId"])==0) return;
            bindAddendumProgressNote(common.myInt(hdnProgressNoteID.Value));
            ResetPage();

        }
    }
    
    protected void btnSaveData_Click(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        { 
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (!isSave())
            {
                return;
            }
            SaveData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objException = null;
        }
    }
    public void SaveData()
    {
        clsEMR objval = new clsEMR(sConString);
        clsExceptionLog objException = new clsExceptionLog();
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        Hashtable hshOut = new Hashtable();
        try
        {
            
            string strMsg = string.Empty;
            hshOut = objval.SaveDoctorProgressNoteAddendum(0, common.myInt(hdnProgressNoteID.Value), common.myStr(txtWProgressNote.Content), common.myInt(Session["EmployeeId"]));// common.myInt(ViewState["AddendumId"])

            if (common.myStr(hshOut["@chvErrorOutput"]).Contains("Save") || common.myStr(hshOut["@chvErrorOutput"]).Contains("Update"))
            {
                txtWProgressNote.Content = string.Empty;
                hdnAddendumID.Value = "";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindAddendumProgressNote(common.myInt(hdnProgressNoteID.Value));
                lblMessage.Text = common.myStr(hshOut["@chvErrorOutput"]);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "clearProgressNote();", true);
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strMsg;
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
            objval = null;
            objException = null;
            strXML = null;
            coll = null;
            txtWProgressNote.Content = string.Empty;
        }
    }
    private bool isSave()
    {
        bool isSave = true;
        string msg = string.Empty;
        if (common.myStr(txtWProgressNote.Content).Length == 0)
        {
            msg += "Please enter the Addendum!";
            isSave = false;
        }
        lblMessage.Text = string.Empty;
        lblMessage.Text = msg;
        return isSave;
    }
    
    private void bindAddendumProgressNote( int ProgressNoteID)
    {
        DataSet ds = new DataSet();
        clsEMR  objval = new clsEMR(sConString);
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            
                ds=objval.GetDoctorProgressNoteAdendum(ProgressNoteID);
                gvAddendum.DataSource = ds;
                gvAddendum.DataBind();

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
            objval = null;
            objException = null;
        }
    }
       
    protected void ButtonOk_OnClick(object sender, EventArgs e)
    {
        clsEMR objval = new clsEMR(sConString);
        clsExceptionLog objException = new clsExceptionLog();
        Hashtable hshOut = new Hashtable();
        try
        {

            hshOut = objval.DeleteProgressNoteAddendum(common.myInt(hdnAddendumID.Value), common.myInt(Session["EmployeeId"]));
            if (common.myStr(hshOut["@chvErrorOutput"]).Contains("Delete"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = common.myStr(hshOut["@chvErrorOutput"]);
            }
            bindAddendumProgressNote(common.myInt(hdnProgressNoteID.Value));
            ResetPage();
            //dvConfirmCancelOptions.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objException = null;
            objval = null;
        }
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvAddendum, "Select$" + e.Row.RowIndex);
            e.Row.ToolTip = "Click to select this row.";
        }
    }
    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvAddendum.Rows)
        {
            //if (row.RowIndex == gvAddendum.SelectedIndex)
            //{
            //    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
            //    row.ToolTip = string.Empty;
            //}
            //else
            //{
            //    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            //    row.ToolTip = "Click to select this row.";
            //}
        }
    }

    protected void gvAddendum_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
           // LinkButton row = (LinkButton)(((LinkButton)e.CommandSource).NamingContainer);
            // GridViewRow gvr = (GridViewRow)((GridViewRow)e.CommandSource).NamingContainer;
            // int RowIndex = gvr.RowIndex;
            int index = common.myInt(e.CommandArgument);
            GridViewRow row = this.gvAddendum.Rows[index];
            HiddenField Addendum = (HiddenField)row.FindControl("lblAddendum");
            if (common.myStr(e.CommandName.ToUpper()).Equals("SELECT"))
            {
                //RTF1.Content = Addendum.Value;
                lblNoteView.Text = Addendum.Value;
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
            
            objException = null;
            
        }
    }
protected void gvAddendum_OnItemCommand(object source, GridCommandEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            HiddenField hdnAddID = (HiddenField)e.Item.FindControl("hdnAddendumId");
            HiddenField Addendum = (HiddenField)e.Item.FindControl("lblAddendum");
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
               // dvConfirmCancelOptions.Visible = true;
                hdnAddendumID.Value = hdnAddID.Value;
                bindAddendumProgressNote(common.myInt(hdnProgressNoteID.Value));

            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                //RTF1.Content = Addendum.Value;
                lblNoteView.Text= Addendum.Value;
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                btnSaveData.Visible = false;
                btnUpdateData.Visible = true;
                hdnAddendumID.Value = hdnAddID.Value;
                Label lblAddendum = (Label)e.Item.FindControl("lblAddendum");
                txtWProgressNote.Content = lblAddendum.Text;
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
            objException = null;
           
        }
    }
    private void ResetPage()
    {
        try
        {
            hdnAddendumID.Value = "";
            btnSaveData.Visible = true;
            btnUpdateData.Visible = false;
            ViewState["DiagSampleId"] = string.Empty;
            txtWProgressNote.Content = string.Empty;
            lblMessage.Text = "";
            lblNoteView.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        ResetPage();

    }
    
    
}