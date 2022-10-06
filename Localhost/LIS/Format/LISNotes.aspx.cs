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
using System.Collections.Generic;
public partial class LIS_Format_LISNotes : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if(common.myInt(Request.QueryString["Servicedetails"])>0)
                {
                    lblInfoMessage.Visible = false;
                    txtNotes.Visible = false;
                    btnSaveMessage.Visible = false;
                }
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);                
                string SOURCE = common.myStr(Request.QueryString["SOURCE"]);
                BindPatientDetails(SOURCE);
                BindNotes();
                RadTabStrip1.SelectedIndex = 0;
                RadMultiPage1.SelectedIndex = 0;              
            }
            catch (Exception ex)
            {
                Alert.ShowAjaxMsg(ex.Message, this.Page);
                return;
            }
        }
    }
    private void BindPatientDetails(string SOURCE)
    {
        lblPatientName.Text = common.myStr(Request.QueryString["pn"]);
    
        lblRegNo.Text = common.myStr(Request.QueryString["RegNo"]);
        lblLabNo.Text = common.myStr(Request.QueryString["LABNO"]);
        if (SOURCE == "IPD")
        {
            lblEncounterNo.Text = common.myStr(Request.QueryString["eno"]);        
            lblInfoEncounterNo.Visible = true;         
            lblEncounterNo.Visible = true;
          
        }
        else
        {
            lblInfoEncounterNo.Visible = false;           
            lblEncounterNo.Visible = false;         
        }

    }

    private void BindNotes()
    {
        DataSet ds = new DataSet();
        try
        {
            BaseC.clsLISPhlebotomy objNote = new BaseC.clsLISPhlebotomy(sConString);
            string SOURCE = common.myStr(Request.QueryString["SOURCE"]);

            if (common.myInt(Request.QueryString["Servicedetails"]) > 0)
            {
                ds = objNote.GetLISNotes("uspDiagGetSampleNotes", SOURCE, common.myInt(Request.QueryString["LABNO"]), common.myStr(txtNotes.Text),
                             common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["OrderId"]));
            }
            else
            {

                ds = objNote.GetLISNotes("uspDiagGetSampleNotes", SOURCE, common.myInt(Request.QueryString["LABNO"]), common.myStr(txtNotes.Text),
                             common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0);
            }

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dvMsg = new DataView(ds.Tables[0]);
                    dvMsg.RowFilter = "NoteType=1";
                    gvMessages.DataSource = dvMsg.ToTable();
                    gvMessages.DataBind();
                    RadTabStrip1.Tabs[0].Text = "Messages (" + common.myStr(dvMsg.ToTable().Rows.Count) + ")";

                    DataView dvRef = new DataView(ds.Tables[0]);
                    dvRef.RowFilter = "NoteType=2";
                    gvPref.DataSource = dvRef.ToTable();
                    gvPref.DataBind();
                    RadTabStrip1.Tabs[1].Text = "Cancel Remarks (" + common.myStr(dvRef.ToTable().Rows.Count) + ")";


                }
                else
                {
                    gvMessages.DataSource = null;
                    gvMessages.DataBind();
                    RadTabStrip1.Tabs[0].Text = "Messages (0)";

                    gvPref.DataSource = null;
                    gvPref.DataBind();
                    RadTabStrip1.Tabs[1].Text = "Cancel Remarks (0)";
                }

            }
            else
            {
                gvMessages.DataSource = null;
                gvMessages.DataBind();
                RadTabStrip1.Tabs[0].Text = "Messages (0)";

                gvPref.DataSource = null;
                gvPref.DataBind();
                RadTabStrip1.Tabs[1].Text = "Preferences (0)";
            }
        }
        catch (Exception ex)
        {
            Alert.ShowAjaxMsg(ex.Message, this.Page);
            return;
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
    }

    //protected void gvMessages_OnItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Item is GridFooterItem)
    //        {
    //            GridFooterItem footer = ((GridFooterItem)e.Item);
    //        }
    //        if (e.Item is GridGroupFooterItem)
    //        {
    //            GridGroupFooterItem groupfooter = ((GridGroupFooterItem)e.Item);
    //            //groupfooter["ServiceName"].Text = "Total: ";
    //            //groupfooter["ServiceName"].HorizontalAlign = HorizontalAlign.Right;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Alert.ShowAjaxMsg(ex.Message, this.Page);
    //        return;
    //    }
    //}
    protected void btnSaveMessage_OnClick(object sender, EventArgs e)
    {
        string msg = "Please enter message";
        Int16 NoteType = 1;
        if (RadTabStrip1.SelectedIndex == 1)
        {
            msg = "Please enter preference";
            NoteType = 2;
        }
        if (common.myStr(txtNotes.Text) == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = msg;
            txtNotes.Focus();
            return;
        }
        SaveUpdateNotes(NoteType);
    }

    void SaveUpdateNotes(Int16 NoteType)
    {
        try
        {
            
            BaseC.clsLISPhlebotomy objNote = new BaseC.clsLISPhlebotomy(sConString);
            //DataSet ds = new DataSet();
            string SOURCE = common.myStr(Request.QueryString["SOURCE"]);           
            int LabNo = common.myInt(Request.QueryString["LABNO"]);

            //Int16 HospId = Convert.ToInt16(Session["HospitalLocationId"]);
            //int FacilityId = common.myInt(Session["FacilityId"]);
            //int EncounterId = common.myInt(Request.QueryString["EncId"]);
            //int RegistrationId = common.myInt(Request.QueryString["RegId"]);
            //Int32 iNoteId = common.myInt(hdnNoteId.Value);

            string SaveMessage = objNote.SaveLISNotes("uspDiagSaveSampleNotes", SOURCE, LabNo, common.myStr(txtNotes.Text), common.myInt(Session["UserId"]));
            if (SaveMessage != null)
            {
                //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //lblMessage.Text = SaveMessage;
                BindNotes();
                btnSaveMessage.Text = "Save";
                hdnNoteId.Value = "0";
                txtNotes.Text = "";
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ex.Message;
            objException.HandleException(ex);
        }
    }

    protected void gvMessages_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                gvMessages.Items[e.Item.ItemIndex].Selected = true;
                int iNoteId = common.myInt(gvMessages.SelectedItems[0].Cells[2].Text);
                Literal ltrNotes = (Literal)gvMessages.SelectedItems[0].FindControl("ltrNotes");
                string Note = common.myStr(ltrNotes.Text);
                hdnNoteId.Value = iNoteId.ToString();
                txtNotes.Text = Note;
                btnSaveMessage.Text = "Update";
                txtNotes.Focus();
                lblMessage.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvPref_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                gvPref.Items[e.Item.ItemIndex].Selected = true;
                int iNoteId = common.myInt(gvPref.SelectedItems[0].Cells[2].Text);
                Literal ltrNotes = (Literal)gvPref.SelectedItems[0].FindControl("ltrNotes");
                string Note = common.myStr(ltrNotes.Text);
                hdnNoteId.Value = iNoteId.ToString();
                txtNotes.Text = Note;
                btnSaveMessage.Text = "Update";
                txtNotes.Focus();
                lblMessage.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
   
    protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
    {
        if (e.Tab.Index == 1)
            pnlNewMessage.Visible = false;
        else if(e.Tab.Index == 0)
            pnlNewMessage.Visible = true;

    }
}
