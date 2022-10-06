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
public partial class EMR_Templates_TemplateNotesPrint : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Convert.ToInt64(Request.QueryString["RegistrationNo"]) > 0 && Convert.ToInt64(Request.QueryString["OT"]) == 2)
            {
                Session["RegistrationNo"] = Convert.ToInt64(Request.QueryString["RegistrationNo"]);
                btnGetInfo_Click(null, null);

            }

            ddlTempGroup.Visible = false;
            txtAllTemplateSearch.Text = common.myStr(Request.QueryString["DefaultTemplate"]);
            if (!common.myInt(Request.QueryString["RegistrationNo"]).Equals(0))
            {
                DataSet dsPatientDetail = new DataSet();
                BaseC.Patient bC = new BaseC.Patient(sConString);
                Session["RegistrationNo"] = common.myInt(Request.QueryString["RegistrationNo"]);
                Session["RegistrationId"] = common.myInt(Request.QueryString["RegistrationId"]);

                if (common.myInt(Session["EncounterId"]).Equals(0))
                {
                    Session["EncounterId"] = common.myInt(null);
                }

                dsPatientDetail = bC.GetPatientRecord(common.myInt(Session["RegistrationNo"]), common.myStr(null));
                string Lasvisitsession = "&nbsp;<span style = 'color:white;font-weight: bold;background-color:green;'> " + common.myStr(Session["LastVisit"]) + " </span> ";

                string sRegNoTitle = Resources.PRegistration.regno;
                string sDoctorTitle = Resources.PRegistration.Doctor;

                Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" +
                            common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientAgeGender"]) + "</span>"
                         + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                         + "&nbsp;Mobile:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                         + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["CompanyName"]) + "</span>"
                         + Lasvisitsession
                         + "</b>";

                dsPatientDetail.Dispose();

                //#endregion
            }

            BindPatientHiddenDetailsEncounterNo(common.myInt(Session["RegistrationNo"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            hdnIsUnSavedData.Value = "0";
            //  BindPatientHiddenDetails();
            //BindddlTempGroup();
            bindAllTemplateList();
            txtAllTemplateSearch.Focus();
            ShowTemplateData();
            BindPatientHiddenDetails();
        }

    }



    void BindPatientHiddenDetails()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    private void ShowTemplateData()
    {
        try
        {
            DAL.DAL dl;
            DataSet ds = new DataSet();
            Hashtable hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            //hshIn.Add("intRegistrationId", common.myInt(Session["RegistrationId"]).ToString());
            hshIn.Add("intRegistrationNo", common.myInt(Session["RegistrationNo"]).ToString());
            //hshIn.Add("intEncounterId", common.myInt(Session["EncounterId"]).ToString());
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientConsentForms", hshIn);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                ds.Tables[0].AcceptChanges();
            }

            gvContentForm.DataSource = ds.Tables[0];
            gvContentForm.DataBind();




        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void gvContentForm_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("EDITTEMP"))
            {
                if (common.myInt(Session["RegistrationId"]).Equals(0))
                {
                    lblMessage.Text = "Patient not selected!!";
                    return;
                }

                int TemplateId = common.myInt(e.CommandArgument);
                if (TemplateId > 0)
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblTemplateName = (Label)row.FindControl("lblTemplateNameConsent");
                    string hdnId = common.myStr(((HiddenField)row.FindControl("hdnId")).Value);

                    hdnpage.Value = "/EMR/Templates/EditConsentTemplate.aspx?FromId=" + hdnId + "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "captsig", "pagepopup();", true);

                    //RadWindow2.NavigateUrl = "/EMR/Templates/EditConsentTemplate.aspx?FromId=" + hdnId + "";
                    //RadWindow2.Height = 600;
                    //RadWindow2.Width = 1330;
                    //RadWindow2.Top = 0;
                    //RadWindow2.Left = 0;

                    //RadWindow2.OnClientClose = "OnClientClose";
                    //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow2.Modal = true;
                    //RadWindow2.VisibleStatusbar = false;

                }
            }
            else if (common.myStr(e.CommandName).Equals("Del"))
            {

                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lblTemplateName = (Label)row.FindControl("lblTemplateNameConsent");
                string hdnId = common.myStr(((HiddenField)row.FindControl("hdnId")).Value);
                string hdnFinalize = common.myStr(((HiddenField)row.FindControl("hdnFinalize")).Value);

                //if (hdnFinalize.Equals("True"))
                //{
                //    BaseC.Security oSec = new BaseC.Security(sConString);
                //    if (!oSec.CheckUserRights(common.myInt(Session["HospitalLocaationId"]), common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedToDeleteConsentForm"))
                //    {
                //        //GridViewRow row1 = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                //        //ImageButton ImageButton = (ImageButton)row1.FindControl("lnkDelete");
                //        //ImageButton.Visible = false;
                //        ////(e.Row.FindControl("lnkDelete") as LinkButton).Visible = false;
                //        lblMessage.Text = "You are not authorized to Delete record. Contact your administrator.";
                //        return;
                //    }
                //    oSec = null;
                //}


                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hstInput = new Hashtable();
                hstInput.Add("intId", common.myInt(hdnId));
                hstInput.Add("EncodedBy", common.myInt(Session["UserId"]));
                objDl.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "UspEMRDeletePatientConsentForms", hstInput);

                ShowTemplateData();
            }
            else if (common.myStr(e.CommandName).ToUpper().Equals("VIEW"))
            {
                if (common.myInt(Session["RegistrationId"]).Equals(0))
                {
                    lblMessage.Text = "Patient not selected!!";
                    return;
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblTemplateName = (Label)row.FindControl("lblTemplateNameConsent");
                string hdnId = common.myStr(((HiddenField)row.FindControl("hdnId")).Value);


                hdnpage.Value = "/EMR/Templates/EditConsentTemplate.aspx?FromId=" + hdnId + "&VIEW=DISABLED";
                ScriptManager.RegisterStartupScript(this, GetType(), "captsig", "pagepopup();", true);

                //RadWindow2.NavigateUrl = "/EMR/Templates/EditConsentTemplate.aspx?FromId=" + hdnId + "&VIEW=DISABLED";
                //RadWindow2.Height = 600;
                //RadWindow2.Width = 1330;
                //RadWindow2.Top = 0;
                //RadWindow2.Left = 0;
                //RadWindow2.OnClientClose = "OnClientClose";
                //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                //RadWindow2.Modal = true;
                //RadWindow2.VisibleStatusbar = false;

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvContentForm_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            gvContentForm.PageIndex = e.NewPageIndex;
            ShowTemplateData();
        }
        catch
        {

        }
    }

    protected void gvContentForm_RowDataBound(object sender, GridViewRowEventArgs e)
    {



        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lnkBtnDelete");
                //ScriptManager.GetCurrent(this).RegisterPostBackControl(lbtnEdit);

                string hdnFinalize = common.myStr(((HiddenField)e.Row.FindControl("hdnFinalize")).Value);
                if (hdnFinalize.Equals("True"))
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Plum;
                    (e.Row.FindControl("lnkBtnEditTemp") as LinkButton).Visible = false;

                }

            }
        }
    }


    protected void ddlTemplateTypeCode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtAllTemplateSearch.Text = "";
            lblMessage.Text = "&nbsp;";
            bindAllTemplateList();
        }
        catch
        {
        }
    }
    protected void ddlTempGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindAllTemplateList();
    }

    private void BindddlTempGroup()
    {
        DAL.DAL objFav = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        DataSet ds = new DataSet();
        try
        {
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            ds = objFav.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateGroup", HshIn);
            ddlTempGroup.DataSource = ds;
            ddlTempGroup.DataTextField = "GroupName";
            ddlTempGroup.DataValueField = "GroupId";
            ddlTempGroup.DataBind();
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
        }
    }
    private void bindAllTemplateList()
    {
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        BaseC.clsIVF objivf = new BaseC.clsIVF(sConString);
        try
        {
            Label1.Text = common.myStr(ddlTemplateTypeCode.SelectedItem.Text);

            ds = objivf.getEMRTemplateTypeWiseConsent(common.myInt(Session["HospitalLocationId"]), common.myStr(ddlTemplateTypeCode.SelectedValue), common.myStr(txtAllTemplateSearch.Text.Trim()), common.myInt(Session["EncounterId"]));

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                ds.Tables[0].AcceptChanges();
            }

            gvAllTemplate.DataSource = ds.Tables[0];
            gvAllTemplate.DataBind();
            txtAllTemplateSearch.Focus();
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
            objivf = null;
        }
    }
    protected void gvAllTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {

            if (common.myStr(e.CommandName).ToUpper().Equals("SELECT"))
            {
                if (common.myInt(Session["RegistrationNo"]).Equals(0))
                {
                    Alert.ShowAjaxMsg("Please Select Registration No", Page);
                    return;
                }
                int TemplateId = common.myInt(e.CommandArgument);

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblTemplateName = (Label)row.FindControl("lblTemplateName");

                if (lblTemplateName.Text.Contains("ARABIC"))
                {

                    hdnpage.Value = "/EMR/Templates/ArabicTemplate.aspx?From=" + common.myStr(Request.QueryString["From"]).ToUpper() + "&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    ScriptManager.RegisterStartupScript(this, GetType(), "captsig", "pagepopup();", true);

                    //RadWindow2.NavigateUrl = "/EMR/Templates/ArabicTemplate.aspx?From=POPUP&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    //RadWindow2.Height = 600;
                    //RadWindow2.Width = 1230;
                    //RadWindow2.Top = 0;
                    //RadWindow2.Left = 0;
                    //RadWindow2.OnClientClose = "OnClientClose";
                    //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow2.Modal = true;
                    //RadWindow2.VisibleStatusbar = false;
                }
                else if (lblTemplateName.Text.Contains("GLOBAL"))
                {
                    if (common.myInt(Session["EncounterId"]).Equals(0))
                    {
                        Alert.ShowAjaxMsg("Please Select Encounter No", Page);
                        return;
                    }

                    hdnpage.Value = "../../EMRReports/GlobalClaimForm.aspx?From=" + common.myStr(Request.QueryString["From"]).ToUpper() + "&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    ScriptManager.RegisterStartupScript(this, GetType(), "captsig", "pagepopup();", true);

                    //RadWindow2.NavigateUrl = "~/EMRReports/GlobalClaimForm.aspx?From=POPUP&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    //RadWindow2.Height = 600;
                    //RadWindow2.Width = 1230;
                    //RadWindow2.Top = 0;
                    //RadWindow2.Left = 0;

                    //RadWindow2.OnClientClose = "OnClientClose";
                    //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow2.Modal = true;
                    //RadWindow2.VisibleStatusbar = false;
                }
                else
                {

                    hdnpage.Value = "/EMR/Templates/ConsentTemplate.aspx?From=" + common.myStr(Request.QueryString["From"]).ToUpper() + "&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    ScriptManager.RegisterStartupScript(this, GetType(), "captsig", "pagepopup();", true);

                    //RadWindow2.NavigateUrl = "/EMR/Templates/ConsentTemplate.aspx?From=POPUP&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    //RadWindow2.Height = 600;
                    //RadWindow2.Width = 1230;
                    //RadWindow2.Top = 0;
                    //RadWindow2.Left = 0;
                    //RadWindow2.OnClientClose = "OnClientClose";
                    //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow2.Modal = true;
                    //RadWindow2.VisibleStatusbar = false;

                }

            }
            else if (common.myStr(e.CommandName).ToUpper().Equals("TABLET"))
            {
                if (common.myInt(Session["RegistrationNo"]).Equals(0))
                {
                    Alert.ShowAjaxMsg("Please Select Registration No", Page);
                    return;
                }
                int TemplateId = common.myInt(e.CommandArgument);

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblTemplateName = (Label)row.FindControl("lblTemplateName");
                if (lblTemplateName.Text.Contains("test"))
                {

                    hdnpage.Value = "/EMR/Templates/T_ArabicTemplate.aspx?From=" + common.myStr(Request.QueryString["From"]).ToUpper() + "&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    ScriptManager.RegisterStartupScript(this, GetType(), "captsig", "pagepopup();", true);

                    //RadWindow2.NavigateUrl = "/EMR/Templates/ArabicTemplate.aspx?From=POPUP&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    //RadWindow2.Height = 600;
                    //RadWindow2.Width = 1230;
                    //RadWindow2.Top = 0;
                    //RadWindow2.Left = 0;
                    //RadWindow2.OnClientClose = "OnClientClose";
                    //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow2.Modal = true;
                    //RadWindow2.VisibleStatusbar = false;
                }
                else if (lblTemplateName.Text.Contains("GLOBAL"))
                {
                    if (common.myInt(Session["EncounterId"]).Equals(0))
                    {
                        Alert.ShowAjaxMsg("Please Select Encounter No", Page);
                        return;
                    }

                    hdnpage.Value = "../../EMRReports/T_GlobalClaimForm.aspx?From=" + common.myStr(Request.QueryString["From"]).ToUpper() + "&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    ScriptManager.RegisterStartupScript(this, GetType(), "captsig", "pagepopup();", true);

                    //RadWindow2.NavigateUrl = "~/EMRReports/GlobalClaimForm.aspx?From=POPUP&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    //RadWindow2.Height = 600;
                    //RadWindow2.Width = 1230;
                    //RadWindow2.Top = 0;
                    //RadWindow2.Left = 0;

                    //RadWindow2.OnClientClose = "OnClientClose";
                    //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow2.Modal = true;
                    //RadWindow2.VisibleStatusbar = false;
                }
                else
                {

                    hdnpage.Value = "/EMR/Templates/T_ConsentTemplate.aspx?From=" + common.myStr(Request.QueryString["From"]).ToUpper() + "&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    ScriptManager.RegisterStartupScript(this, GetType(), "captsig", "pagepopup();", true);

                    //RadWindow2.NavigateUrl = "/EMR/Templates/ConsentTemplate.aspx?From=POPUP&TemplateId=" + TemplateId + "&TemplateName=" + lblTemplateName.Text;
                    //RadWindow2.Height = 600;
                    //RadWindow2.Width = 1230;
                    //RadWindow2.Top = 0;
                    //RadWindow2.Left = 0;
                    //RadWindow2.OnClientClose = "OnClientClose";
                    //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow2.Modal = true;
                    //RadWindow2.VisibleStatusbar = false;

                }

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
        }
    }
    protected void gvAllTemplate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            gvAllTemplate.PageIndex = e.NewPageIndex;
            bindAllTemplateList();
        }
        catch
        {
        }
    }
    protected void gvAllTemplate_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string hdnFieldStatus = common.myStr(((HiddenField)e.Row.FindControl("hdnFieldStatus")).Value.Trim());
                string hdnCode = common.myStr(((HiddenField)e.Row.FindControl("hdnCode")).Value.Trim());
                string hdnSignatureType = common.myStr(((HiddenField)e.Row.FindControl("hdnSignatureType")).Value.Trim());

                LinkButton lnkBtnSelect = (LinkButton)e.Row.FindControl("lnkBtnSelect");
                LinkButton lnkBtnTablet = (LinkButton)e.Row.FindControl("lnkBtnTablet");
                lnkBtnSelect.Visible = false;
                lnkBtnTablet.Visible = true;
                if (hdnSignatureType != "")
                {
                    if (hdnSignatureType.ToUpper() == "W")
                    {
                        lnkBtnSelect.Visible = true;
                    }
                    if (hdnSignatureType.ToUpper() == "T")
                    {
                        lnkBtnTablet.Visible = true;
                    }
                    if (hdnSignatureType.ToUpper() == "B")
                    {
                        lnkBtnSelect.Visible = true;
                        lnkBtnTablet.Visible = true;
                    }




                }

            }
        }
    }
    protected void btnAllTemplateSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindAllTemplateList();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnEnableControl_Click(object sender, EventArgs e)
    {
        ShowTemplateData();
    }


    protected void btnTemplatetablebind_Click(object sender, EventArgs e)
    {
        bindAllTemplateList();
    }

    void BindPatientHiddenDetailsEncounterNo(int RegistrationNo, int Regid, int EncounterId)
    {
        DataSet ds = new DataSet();
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);

        DataSet dsEMRBilling = new DataSet();
        try
        {
            if (RegistrationNo > 0)
            {
                ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), Regid, RegistrationNo, EncounterId, common.myInt(Session["UserId"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRowView drEncounter in ds.Tables[1].DefaultView)
                            {
                                ListItem item = new ListItem();
                                item.Text = (string)drEncounter["EncounterNo"];
                                item.Value = drEncounter["Id"].ToString();
                                //item.Attributes.Add("DoctorId", common.myStr(drEncounter["DoctorId"]));
                                ddlEncounter.Items.Add(item);
                            }
                            //ddlEncounter.SelectedIndex = ddlEncounter.Items.IndexOf(ddlEncounter.Items.FindByValue(common.myStr(hdnEncounterId.Value)));
                        }
                    }

                }

            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            bParse = null;
            bC = null;
            dsEMRBilling.Dispose();
        }
    }

    protected void ddlEncounter_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["EncounterId"] = common.myStr(ddlEncounter.SelectedValue.ToString());
        bindAllTemplateList();
        ShowTemplateData();
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        DataSet dsPatientDetail = new DataSet();
        DataSet dsPatientDetail1 = new DataSet();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        if (!common.myInt(txtAccountNo.Text).Equals(0))
        {

            Session["RegistrationNo"] = common.myInt(txtAccountNo.Text);

            Session["RegistrationId"] = bC.getRegistrationIDFromRegistrationNo(common.myStr(Session["RegistrationNo"]), common.myInt(Session["HospitalLocationID"]));

            //Session["RegistrationNo"] = common.myInt(hdnRegistrationNo.Value);
            //Session["RegistrationId"] = common.myInt(hdnRegistrationId.Value);

            if (common.myInt(Session["EncounterId"]).Equals(0))
            {
                Session["EncounterId"] = common.myInt(null);
            }

            dsPatientDetail = bC.GetPatientRecord(common.myInt(Session["RegistrationNo"]), common.myStr(null));
            string Lasvisitsession = "&nbsp;<span style = 'color:white;font-weight: bold;background-color:green;'> " + common.myStr(Session["LastVisit"]) + " </span> ";

            string sRegNoTitle = Resources.PRegistration.regno;
            string sDoctorTitle = Resources.PRegistration.Doctor;

            Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" +
                        common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientAgeGender"]) + "</span>"
                     + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                     + "&nbsp;Mobile:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                     + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["CompanyName"]) + "</span>"
                     + Lasvisitsession
                     + "</b>";

            dsPatientDetail.Dispose();
            BindPatientHiddenDetails();
            ShowTemplateData();
        }
    }

    protected void lnk_PendingQue_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/Templates/PendingQueDetails.aspx?OPIP=O&RegEnc=0";
        // RadWindowForNew.NavigateUrl = "/Pharmacy//Saleissue/PatientDetails.aspx?OPIP=O&RegEnc=0";
        RadWindow1.Height = 600;
        RadWindow1.Width = 980;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnUpload_Click(object sender, EventArgs e)//yogesh
    {
        try
        {
            RadWindow1.NavigateUrl = "~/EMR/AttachDocumentFTP.aspx?MASTER=No&IsEMRPopUp=1&RNo=" + common.myStr(Session["RegistrationNo"]) + "";
            RadWindow1.OnClientClose = "";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.Title = "Attachment";
            RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Default;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
}

