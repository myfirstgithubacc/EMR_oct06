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
using System.Xml;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using System.Net;

public partial class WardManagement_ReferralRequest : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MASTER"]).ToUpper() == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnIsValidPasswordClose.Visible = false;

            if (common.myStr(Request.QueryString["MASTER"]).ToUpper() == "NO")
            {
                btnClose.Visible = true;
            }
            else
            {
                btnClose.Visible = false;
            }

            if (Session["OPIP"] != null && Session["OPIP"].ToString() == "E")
            {
                ViewState["RegistrationNo"] = Session["RegistrationNo"];
                ViewState["EncounterId"] = Session["EncounterId"];
            }
            else
            {
                ViewState["RegistrationNo"] = common.myStr(Request.QueryString["RegNo"]);
                ViewState["EncounterId"] = common.myStr(Request.QueryString["EId"]);
            }

            if (common.myLen(ViewState["RegistrationNo"]) == 0)
            {
                ViewState["RegistrationNo"] = Session["RegistrationNo"];
            }
            if (common.myLen(ViewState["EncounterId"]) == 0)
            {
                ViewState["EncounterId"] = Session["EncounterId"];
            }

            fillDoctor();
            BindGrid();
            if (common.myStr(Request.QueryString["RefHis"]) == "Yes")//this condition apply for referral  history page
            {
                ViewState["ReferralReplyId"] = common.myInt(Request.QueryString["RefReplyId"]).ToString();
                DisplayForHistoryData();
            }
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
            {
                btnSave.Visible = false;
                btnNew.Visible = false;

                ddlRequestToDoctor.Enabled = false;
                txtRemarks.Enabled = false;
                gvDetails.Enabled = false;
            }
            fillSpecialization();
            FillReferFrom();

        }
    }

    protected void fillDoctor()
    {
        BaseC.WardManagement baseHc = new BaseC.WardManagement();

        DataSet ds = new DataSet();
        try
        {
            ddlRequestToDoctor.DataSource = null;

            ddlRequestToDoctor.ClearSelection();
            ddlRequestToDoctor.Items.Clear();
            ddlRequestToDoctor.Text = "";


            ds = baseHc.getDoctorBasedEmployment(common.myInt(Session["FacilityId"]), "NE");

            //string SpecialRigthFlag = "IsAllowToReferPatient";
            //ds = baseHc.GetSpecialRightsEmployeeList(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(0), Convert.ToInt16(Session["FacilityId"]), SpecialRigthFlag);

            foreach (DataRow drt in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();

                item.Text = (string)drt["DoctorName"];
                item.Value = drt["DoctorId"].ToString();
                //item.Attributes.Add("SpecialisationId", common.myStr(drt["SpecialisationId"]));
                //item.Attributes.Add("Mobile", common.myStr(drt["Mobile"]));

                ddlRequestToDoctor.Items.Add(item);
                item.DataBind();
            }



            ddlRequestToDoctor.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlRequestToDoctor.SelectedIndex = 0;

            ddlRequestToDoctor.Enabled = true;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Cleartext();
        lblmsg.Text = "";
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        BaseC.User objuser = new BaseC.User(sConString);
        Email obje = new Email(sConString);

        try
        {


            if (common.myInt(ViewState["EncounterId"]) == 0)
            {
                lblmsg.Text = "Please select encounter !";
                Alert.ShowAjaxMsg("Please select encounter !", Page);
                return;
            }

            if (common.myInt(ddlRequestToDoctor.SelectedValue) == 0)
            {
                lblmsg.Text = "Please select doctor !";
                return;
            }

            if (common.myLen(txtRemarks.Text) == 0)
            {
                lblmsg.Text = "Please fill remarks !";
                return;
            }

            string strsave = objwd.saveIPReferralRequest(common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RequestId"]),
                            common.myInt(rdoRequestType.SelectedValue), common.myInt(ddlRequestToDoctor.SelectedValue),
                            common.myStr(txtRemarks.Text, true), common.myInt(ddlActive.SelectedValue), common.myInt(Session["UserId"]),common.myInt(ddlspecilization.SelectedValue),common.myInt(ddlReferFromDoctor.SelectedValue));

            if (strsave.ToUpper().Contains("SAVED") || strsave.ToUpper().Contains("UPDATED"))
            {
                ViewState["RequestId"] = "0";

                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                //obje.SMSReferral(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlRequestToDoctor.SelectedValue), common.myInt(ViewState["EncounterId"]));
                //lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //lblmsg.Text = strsave;
                ddlRequestToDoctor.Enabled = true;
                Cleartext();

                BindGrid();
            }
            else
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }

            lblmsg.Text = strsave;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objwd = null;
            objuser = null;
            obje = null;
        }
    }

    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        // RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP&OtherUserId="+ddlRequestToDoctor.SelectedValue+"&UserName="+ddlRequestToDoctor.Text;
        RadWindowForNew.NavigateUrl = "~/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP&OtherUserId=" + common.myStr(Session["UserId"]) + "&UserName=" + ddlRequestToDoctor.Text;
        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }


    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(hdnIsValidPassword.Value) == 0)
            {
                lblmsg.Text = "Invalid Password !";
                return;
            }
            if (common.myInt(hdnIsValidPassword.Value) == 1)
            {
                lblmsg.Text = "";
                btnSave_Click(null, null);
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public void BindGrid()
    {
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        try
        {
            ds = objwd.getIPReferralRequest(common.myInt(Session["FacilityId"]), common.myInt(ViewState["EncounterId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);

                gvDetails.DataSource = ds;
                gvDetails.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objwd = null;
            ds.Dispose();
        }
    }

    protected void gvDetails_ItemCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                HiddenField hdnRequestId = (HiddenField)e.Item.FindControl("hdnRequestId");
                HiddenField hdnRequestType = (HiddenField)e.Item.FindControl("hdnRequestType");
                HiddenField hdnRequestToDoctorId = (HiddenField)e.Item.FindControl("hdnRequestToDoctorId");
                HiddenField hdnActive = (HiddenField)e.Item.FindControl("hdnActive");

                Label lblRequestRemarks = (Label)e.Item.FindControl("lblRequestRemarks");

                ViewState["RequestId"] = common.myInt(hdnRequestId.Value).ToString();


                rdoRequestType.SelectedIndex = rdoRequestType.Items.IndexOf(rdoRequestType.Items.FindByValue(common.myStr(hdnRequestType.Value)));
                ddlRequestToDoctor.ClearSelection();
                ddlRequestToDoctor.SelectedIndex = ddlRequestToDoctor.Items.IndexOf(ddlRequestToDoctor.Items.FindItemByValue(common.myStr(hdnRequestToDoctorId.Value)));

                ddlRequestToDoctor.Enabled = false;

                txtRemarks.Text = common.myStr(lblRequestRemarks.Text);

                ddlActive.SelectedIndex = ddlActive.Items.IndexOf(ddlActive.Items.FindItemByValue(common.myStr(hdnActive.Value)));

                btnSave.Visible = true;
                lblmsg.Text = string.Empty;

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvDetails_OnItemDataBound(object source, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
        {
            //NRR - Requested, RRA - Acknowledged, RRC - Completed

            HiddenField hdnRequestStatusCode = (HiddenField)e.Item.FindControl("hdnRequestStatusCode");
            LinkButton lblEdit = (LinkButton)e.Item.FindControl("lblEdit");

            if (common.myStr(hdnRequestStatusCode.Value).Equals("NRR"))
            {
                lblEdit.Visible = true;
            }
            else
            {
                lblEdit.Visible = false;
            }
        }
    }
    void Cleartext()
    {
        ddlRequestToDoctor.Text = "";
        ddlRequestToDoctor.SelectedIndex = 0;
        txtRemarks.Text = "";


        txtRemarks.Enabled = true;


        btnSave.Visible = true;
        ddlRequestToDoctor.Enabled = true;

        btnSave.Text = "Save (Ctrl+F3)";
        ViewState["SelectedEncounterId"] = "";

        fillDoctor();
        ViewState["Source"] = "";


        //ddlTaggedFor.SelectedIndex = ddlTaggedFor.Items.IndexOf(ddlTaggedFor.Items.FindItemByValue("8"));
    }




    protected void gvDetails_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        BindGrid();
    }
    void DisplayForHistoryData()
    {
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataView DV = new DataView();
        try
        {
            ds = objwd.GetReferralDetail(common.myInt(Session["UserId"]), common.myInt(ViewState["EncounterId"]), "B",
                                    common.myInt(Session["FacilityId"]), common.myStr(ViewState["RegistrationNo"]), 2,
                                    common.myInt(Request.QueryString["refId"]), 0);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DV = ds.Tables[0].Copy().DefaultView;
                if (common.myStr(Request.QueryString["RefHis"]).ToUpper().Equals("YES"))
                {
                    if (common.myInt(Request.QueryString["RefReplyId"]) > 0)
                    {
                        DV.RowFilter = "ReferralReplyId=" + common.myInt(Request.QueryString["RefReplyId"]);
                    }
                }

                dt = DV.ToTable();

                if (dt.Rows.Count > 0)
                {
                    ViewState["ReferralId"] = common.myInt(Request.QueryString["refId"]);
                    ddlRequestToDoctor.Text = string.Empty;
                    ddlRequestToDoctor.SelectedValue = common.myStr(dt.Rows[0]["ReferToDoctorId"]);
                    ddlRequestToDoctor.Enabled = false;

                    txtRemarks.Text = common.myStr(dt.Rows[0]["Note"]);

                    lblmsg.Text = string.Empty;
                    btnSave.Visible = true;
                    ViewState["SelectedEncounterId"] = common.myStr(dt.Rows[0]["EncounterId"]);

                    ViewState["Source"] = common.myStr(dt.Rows[0]["Source"]);

                    if (common.myStr(dt.Rows[0]["ConcludeReferral"]).ToUpper() == "OPEN")
                    {
                        btnSave.Text = "Update";
                        txtRemarks.Enabled = false;
                        btnSave.Visible = false;

                        if (common.myInt(Session["UserId"]) == common.myInt(dt.Rows[0]["EncodedId"]))
                        {
                            txtRemarks.Enabled = true;
                            btnSave.Visible = true;
                        }
                        else
                        {

                            btnSave.Visible = true;
                        }
                    }
                    else // Close
                    {
                        btnSave.Text = "Save (Ctrl+F3)";
                        btnSave.Visible = false;
                        txtRemarks.Enabled = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objwd = null;
            ds.Dispose();
        }
    }

    protected void btnClose_OnClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        return;
    }
    protected void btnMutipleReply_OnClick(object sender, EventArgs e)
    {
        ViewState["ReferralReplyId"] = null;
    }

    protected void lnkBtnDoctorReferralHistory_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMR/ReferralPatientHistory.aspx?OP_IP=B&MASTER=NO&RegId=" + common.myStr(hdnRegistrationId.Value) + "&RegNo=";// + common.myStr(lblRegistrationNumber.Text);
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 20;
        RadWindowForNew.Left = 20;
        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }


    protected void fillSpecialization()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            ddlspecilization.DataSource = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorSpecialisation");
            ddlspecilization.DataTextField = "Specialisation";
            ddlspecilization.DataValueField = "SpecialisationId";
            ddlspecilization.DataBind();

            ddlspecilization.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlspecilization.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dl = null;
        }
    }

    public void FillReferFrom()
    {
        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);


        DataSet ds = new DataSet();
        ds = objEmr.GetReferFromEmployeeList(common.myInt(ViewState["EncounterId"]), common.myInt(Request.QueryString["Regid"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]), 0);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlReferFromDoctor.Items.Clear();
            ddlReferFromDoctor.DataSource = ds.Tables[0];
            ddlReferFromDoctor.DataTextField = "DoctorName";
            ddlReferFromDoctor.DataValueField = "EmployeeId";
            ddlReferFromDoctor.DataBind();
        }
        if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            ddlReferFromDoctor.SelectedIndex = ddlReferFromDoctor.Items.IndexOf(ddlReferFromDoctor.Items.FindItemByValue(common.myInt(ds.Tables[1].Rows[0]["DoctorId"]).ToString()));
        }
    }

}
