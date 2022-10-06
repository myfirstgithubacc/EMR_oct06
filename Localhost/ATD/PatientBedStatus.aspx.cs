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
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Globalization;
using Telerik.Web.UI;

public partial class ATD_PatientBedStatus : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //DAL.DAL dl;
    //BaseC.ATD Objstatus;
    //DataSet ds;
    private bool RowSelStauts = false;
    //clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rdlbedlist_SelectedIndexChanged(this, null);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvNameList.SelectedItems.Count == 0)
            {
                Alert.ShowAjaxMsg("Please Select a Patient !", Page);
                return;
            }
            else
            {
                if (rdlbedlist.SelectedValue == "I")
                {
                    BaseC.Security objSecurity = new BaseC.Security(sConString);  //

                    string FlagIsDischargeApprovalAuthorizationReqd = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsDischargeApprovalAuthorizationReqd", sConString);

                    if (FlagIsDischargeApprovalAuthorizationReqd.Equals("Y"))
                    {
                        if (!objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "isAuthorizedForATDDischargeApproval"))
                        {
                            Alert.ShowAjaxMsg("Not authorized to Discharge Approval !", this.Page);
                            return;
                        }
                    }
                    objSecurity = null;

                    RadWindowForNew.NavigateUrl = "/PRegistration/Checklist.aspx?From=D&RegistrationId=" + hdnRegid.Value + "&RegistrationNo=" + hdnregno.Value;
                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 700;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "CloseCheckList";
                    RadWindowForNew.Title = "Discharge Checklist";
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                }
                else
                {
                    BaseC.Security objSecurity = new BaseC.Security(sConString);  //
                    if (!objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "isAuthorizedForATDBedRelease"))
                    {
                        Alert.ShowAjaxMsg("Not authorized to Bed Release !", this.Page);
                        return;
                    }
                    objSecurity = null;
                    BaseC.ATD Objstatus = new BaseC.ATD(sConString);
                    string str = Objstatus.SaveUpdateBedstatus(common.myInt(Session["HospitallocationId"]),
                        common.myInt(hdnRegid.Value),
                        common.myInt(hdnEncid.Value),
                        common.myInt(hdnBedid.Value),
                        common.myStr(rdlbedlist.SelectedValue),
                        common.myInt(Session["UserId"]),
                        common.myInt(Session["FacilityId"]), hdnType.Value.Trim());
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblmsg.Text = str;
                }
                BindGrid(rdlbedlist.SelectedValue);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSaveAfterChecklist_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.ATD Objstatus = new BaseC.ATD(sConString);
            string str = Objstatus.SaveUpdateBedstatus(common.myInt(Session["HospitallocationId"]),
                         common.myInt(hdnRegid.Value),
                         common.myInt(hdnEncid.Value),
                         common.myInt(hdnBedid.Value),
                         common.myStr(rdlbedlist.SelectedValue),
                         common.myInt(Session["UserId"]),
                         common.myInt(Session["FacilityId"]), hdnType.Value.Trim());
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindGrid(string status)
    {
        try
        {
            BaseC.ATD Objstatus = new BaseC.ATD(sConString);
            DataSet ds = new DataSet();
            string strbstatus = status;
            gvNameList.DataSource = null;
            gvNameList.DataBind();
            ds = Objstatus.GetHospitalBedStatus(common.myInt(Session["HospitallocationId"]), strbstatus, common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvNameList.DataSource = ds;
                    gvNameList.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void rdlbedlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        RowSelStauts = true;
        BindGrid(rdlbedlist.SelectedValue);
    }
    protected void gvNameList_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (!RowSelStauts)
            {
                BindGrid(rdlbedlist.SelectedValue);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvNameList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                RowSelStauts = true;
                Label lblBedId = new Label();
                Label lblRegistrationId = new Label();
                Label lblEncounterId = new Label();
                Label lblRegistrationNo = new Label();
                Label lblType = new Label();
                lblBedId = (Label)e.Item.FindControl("lblBedId");
                lblRegistrationId = (Label)e.Item.FindControl("lblRegistrationId");
                lblEncounterId = (Label)e.Item.FindControl("lblEncounterId");
                lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
                lblType = (Label)e.Item.FindControl("lblType");

                hdnregno.Value = lblRegistrationNo.Text;
                hdnRegid.Value = lblRegistrationId.Text.Trim();
                hdnEncid.Value = lblEncounterId.Text.Trim();
                hdnBedid.Value = lblBedId.Text.Trim();
                hdnType.Value = lblType.Text.Trim();

                if (hdnEncid.Value.Trim() != "")
                {
                    Session["encounterid"] = hdnEncid.Value.Trim();
                    Session["RegistrationID"] = hdnRegid.Value.Trim();
                }
            }
            if (e.CommandName == "Print")
            {
                Label lblEncounterNo = new Label();
                lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");

                RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?IpNo= " + lblEncounterNo.Text.Trim() + "&rptType=DischargeNotification&RptName=DischargeNotification";
                RadWindowForNew.Height = 500;
                RadWindowForNew.Width = 750;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true;
                RadWindowForNew.Modal = true;
                RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
                RadWindowForNew.VisibleStatusbar = false;

            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvNameList_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
                lbl.Visible = false;

                RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
                combo.Visible = false;
            }


            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
            {
                if (common.myStr(rdlbedlist.SelectedValue).Equals("H"))
                {
                    gvNameList.Columns[5].Visible = true;
                }
                else
                {
                    gvNameList.Columns[5].Visible = false;
                }
            }
        }
        catch
        {
        }
    }

}
