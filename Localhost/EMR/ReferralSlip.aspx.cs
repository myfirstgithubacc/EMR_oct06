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

public partial class EMR_ReferralSlip : System.Web.UI.Page
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
            System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

            collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    "IsEMRCrossConsultationReferral", sConString);

            if (collHospitalSetupValues.ContainsKey("IsEMRCrossConsultationReferral"))
                ViewState["IsEMRCrossConsultationReferral"] = collHospitalSetupValues["IsEMRCrossConsultationReferral"];

            rdoUrgent.SelectedIndex = rdoUrgent.Items.IndexOf(rdoUrgent.Items.FindByValue(common.myStr(Request.QueryString["RequestType"])));

            btnIsValidPasswordClose.Visible = false;
            string IsAutoInsertIPReferralVisitCharge = "IsAutoInsertIPReferralVisitCharge";
            populateRequestTypeStatus();
            gvDetails.Columns[10].Visible = false;
            gvDetails.Columns[25].Visible = false;

            ViewState["IsAutoInsertIPReferral"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), IsAutoInsertIPReferralVisitCharge, sConString);
            if (common.myStr(ViewState["IsAutoInsertIPReferral"]).Equals("Y"))
            {
                btnMutipleReply.Visible = false;
            }

            if (common.myStr(Request.QueryString["FlagRef"]) == "All")
            {
                asplHeaderUD.Visible = false;
            }


            if (common.myStr(Request.QueryString["OP_IP"]) != "I")
            {

                snpSpecialization.Visible = false;
                spnReferToDoctor.Visible = true;
            }
            else
            {
                snpSpecialization.Visible = true;
                spnReferToDoctor.Visible = false;
            }



            if (common.myStr(Request.QueryString["MASTER"]).ToUpper() == "NO")
            {
                btnClose.Visible = true;
            }
            else
            {
                btnClose.Visible = false;
            }

            //if (common.myStr(Request.QueryString["OP_IP"]) == "E" && Session["EncounterId"] != null)
            //{
            //    ViewState["RegistrationNo"] = Session["RegistrationNo"];
            //    ViewState["EncounterId"] = Session["EncounterId"];
            //}
            //else 
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

            dtpdate.SelectedDate = System.DateTime.Now;
            dtpdate.Enabled = false;

            tblConclusion.Visible = false;
            fillSpecialization();
            ddlspecilization_SelectedIndexChanged(sender, e);
            PatientDetails();
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
                rdoUrgent.Enabled = false;
                ddlRequestType.Enabled = false;
                ddlspecilization.Enabled = false;
                ddlRefertodoctor.Enabled = false;
                txtReason.Enabled = false;
                gvDetails.Enabled = false;
            }
            FillReferFrom();

            if (common.myInt(Request.QueryString["ReferredByUserId"]) > 0)
            {
                ddlReferFromDoctor.Enabled = false;
                ddlRefertodoctor.Enabled = false;
                txtReason.Enabled = false;
            }

            if (common.myStr(ViewState["IsEMRCrossConsultationReferral"]).Equals("Y"))
            {
                ddlRequestType.Visible = false;
                Label6.Visible = false;
            }
        }
    }

    public void FillReferFrom()
    {
        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);


        DataSet ds = new DataSet();
        ds = objEmr.GetReferFromEmployeeList(common.myInt(ViewState["EncounterId"]), common.myInt(hdnRegistrationId.Value), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Request.QueryString["RequestId"]));

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlReferFromDoctor.Items.Clear();
            ddlReferFromDoctor.DataSource = ds.Tables[0];
            ddlReferFromDoctor.DataTextField = "DoctorName";
            ddlReferFromDoctor.DataValueField = "EmployeeId";
            ddlReferFromDoctor.DataBind();
        }
        if (common.myStr(Session["FacilityName"]).ToUpper().Contains("PARAS") && ds != null && ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0 && !common.myInt(ds.Tables[3].Rows[0]["ReferFromDoctorId"]).Equals(0))
        {
            ddlReferFromDoctor.SelectedIndex = ddlReferFromDoctor.Items.IndexOf(ddlReferFromDoctor.Items.FindItemByValue(common.myInt(ds.Tables[3].Rows[0]["ReferFromDoctorId"]).ToString()));
        }
        else if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            ddlReferFromDoctor.SelectedIndex = ddlReferFromDoctor.Items.IndexOf(ddlReferFromDoctor.Items.FindItemByValue(common.myInt(ds.Tables[1].Rows[0]["DoctorId"]).ToString()));
        }
        if (ds != null && ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
        {
            if (!common.myInt(ds.Tables[3].Rows[0]["RequestSpecialisationId"]).Equals(0))
            {
                ddlspecilization.SelectedIndex = ddlspecilization.Items.IndexOf(ddlspecilization.Items.FindItemByValue(common.myInt(ds.Tables[3].Rows[0]["RequestSpecialisationId"]).ToString()));
                ddlspecilization_SelectedIndexChanged(null, null);
                ddlspecilization.Enabled = false;
            }
        }
    }
    private void PatientDetails()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                //   lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlspecilization_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillDoctor();
    }
    protected void ddlRefertodoctor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (common.myInt(ddlReferFromDoctor.SelectedValue) == common.myInt(ddlRefertodoctor.SelectedValue))
        {           
            Alert.ShowAjaxMsg("You can not choose same doctor !", Page);
            ddlRefertodoctor.ClearSelection();
            return;
        }
        ddlspecilization.Text = "";

        ddlspecilization.SelectedIndex = ddlspecilization.Items.IndexOf(ddlspecilization.Items.FindItemByValue(common.myStr(ddlRefertodoctor.SelectedItem.Attributes["SpecialisationId"])));


        lblReferToDoctorMobile.Text = string.Empty;
        if (common.myLen(ddlRefertodoctor.SelectedItem.Attributes["Mobile"]) > 0)
        {
            lblReferToDoctorMobile.Text = "Refer To Doctor Mobile No.: " + common.myStr(ddlRefertodoctor.SelectedItem.Attributes["Mobile"]);
        }
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
    /// <summary>
    /// Fill Doctor to Combo on basis of Specialization
    /// </summary>
    protected void fillDoctor()
    {
        BaseC.Hospital baseHc = new BaseC.Hospital(sConString);
        DataSet ds = new DataSet();

        DataTable dtTmp = new DataTable();
        DataColumn dC = new DataColumn();
        DataRow dR;
        try
        {
            lblReferToDoctorMobile.Text = string.Empty;

            ddlRefertodoctor.DataSource = null;

            ddlRefertodoctor.Items.Clear();
            ddlRefertodoctor.Text = "";

            dC = new DataColumn("EmployeeId", typeof(int));
            dtTmp.Columns.Add(dC);
            dC = new DataColumn("EmployeeName", typeof(string));
            dtTmp.Columns.Add(dC);

            if (ViewState["Doctor"] == null)
            {
                string SpecialRigthFlag = "IsAllowToReferPatient";
                ds = baseHc.GetSpecialRightsEmployeeList(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(ddlspecilization.SelectedValue), Convert.ToInt16(Session["FacilityId"]), SpecialRigthFlag);
            }
            else
            {
                ds = (DataSet)ViewState["Doctor"];
            }

            ViewState["Doctor"] = ds;
            if (common.myInt(ddlspecilization.SelectedValue) > 0)
            {
                foreach (DataRow drt in ds.Tables[0].Select("SpecialisationId=" + common.myInt(ddlspecilization.SelectedValue)))
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    //dR = dtTmp.NewRow();
                    //dR["DoctorId"] = drt["DoctorId"];
                    //dR["DoctorName"] = drt["DoctorName"];
                    //dtTmp.Rows.Add(dR);


                    item.Text = (string)drt["EmployeeName"];
                    item.Value = drt["EmployeeId"].ToString();
                    item.Attributes.Add("SpecialisationId", common.myStr(drt["SpecialisationId"]));
                    item.Attributes.Add("Mobile", common.myStr(drt["Mobile"]));

                    ddlRefertodoctor.Items.Add(item);
                    item.DataBind();
                }
            }
            else
            {
                foreach (DataRow drt in ds.Tables[0].Select())
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    //dR = dtTmp.NewRow();
                    //dR["DoctorId"] = drt["DoctorId"];
                    //dR["DoctorName"] = drt["DoctorName"];
                    //item.Attributes.Add("Gender", common.myStr(dsTitle["Gender"]));
                    //dtTmp.Rows.Add(dR);

                    item.Text = (string)drt["EmployeeName"];
                    item.Value = drt["EmployeeId"].ToString();
                    item.Attributes.Add("SpecialisationId", common.myStr(drt["SpecialisationId"]));
                    item.Attributes.Add("Mobile", common.myStr(drt["Mobile"]));

                    ddlRefertodoctor.Items.Add(item);
                    item.DataBind();
                }
            }

            //ddlRefertodoctor.DataSource = dtTmp;
            //ddlRefertodoctor.DataTextField = "DoctorName";
            //ddlRefertodoctor.DataValueField = "DoctorId";
            //ddlRefertodoctor.DataBind();

            ddlRefertodoctor.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlRefertodoctor.SelectedIndex = 0;

            ddlRefertodoctor.Enabled = true;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            baseHc = null;
            ds.Dispose();
            dtTmp.Dispose();
            dC.Dispose();
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
            if (btnSave.Text == "Save (Ctrl+F3)")
            {
                int DoctorId = objuser.getEmployeeId(common.myInt(Session["UserId"]));
                if (DoctorId == common.myInt(ddlRefertodoctor.SelectedValue))
                {
                    lblmsg.Text = "Please select another refer to doctor !";
                    return;
                }


                if (common.myInt(ViewState["EncounterId"]) == 0)
                {
                    lblmsg.Text = "Please select encounter !";
                    Alert.ShowAjaxMsg("Please select encounter !", Page);
                    txtConclusion.Focus();
                    return;
                }

                //if ( !common.myStr(Request.QueryString["OP_IP"]).Equals("I"))
                //{
                if ((!common.myStr(Request.QueryString["OP_IP"]).Equals("I") && Request.QueryString["OP_IP"] != null) || (!common.myStr(Session["OPIP"]).Equals("I") && common.myStr(Session["OPIP"]) != null))
                {
                    if (common.myInt(ddlRefertodoctor.SelectedValue) == 0)
                    {
                        lblmsg.Text = "Refer to doctor is mandatory !";
                        return;
                    }
                }
                else
                {
                    if (common.myInt(ddlspecilization.SelectedValue) == 0)
                    {
                        lblmsg.Text = "Specilization not mandatory !";
                        return;
                    }
                }

                if ((common.myStr(Request.QueryString["OP_IP"]).Equals("I") && Request.QueryString["OP_IP"] != null) || (common.myStr(Session["OPIP"]).Equals("I") && common.myStr(Session["OPIP"]) != null))
                {
                    if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsDoctorMandatoryIPReferral", sConString).Equals("Y"))
                    {
                        if (common.myInt(ddlRefertodoctor.SelectedValue).Equals(0))
                        {
                            lblmsg.Text = "Refer to doctor is mandatory !";
                            return;
                        }
                    }
                }

                if (common.myLen(txtReason.Text) == 0)
                {
                    lblmsg.Text = "Reason for referral is mandatory";
                    return;
                }
                /*From ward */

                /*From ward */
                /* From EMR */
                //if (common.myStr(Session["OPIP"]).Equals("I") && common.myStr(Session["OPIP"])!=null)
                //{
                //    if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsDoctorMandatoryIPReferral", sConString).Equals("Y"))
                //    {
                //        if (common.myInt(ddlRefertodoctor.SelectedValue).Equals(0))
                //        {
                //            lblmsg.Text = "Refer to doctor is mandatory !";
                //            return;
                //        }
                //    }
                //}
                /* From EMR */


                string strsave = objwd.SaveReferralSlip(0, 0, common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["EncounterId"]),
                                 common.myDate(dtpdate.SelectedDate), common.myInt(ddlRefertodoctor.SelectedValue), common.myStr(txtReason.Text),
                                 common.myInt(rdoUrgent.SelectedValue), common.myBool(0), common.myInt(Session["UserId"]), "",
                                 common.myInt(ddlActive.SelectedValue), "", common.myInt(ddlspecilization.SelectedValue),
                                 common.myInt(ddlRequestType.SelectedValue), common.myInt(Session["FacilityID"]),
                                 common.myInt(ddlReferFromDoctor.SelectedValue), common.myInt(Request.QueryString["RequestId"]));

                //if (strsave.Contains("Save Data"))
                if (strsave.Contains("saved"))
                {
                    #region change Referral request status to Complete
                    if (common.myBool(Request.QueryString["IsCompleteRequest"]))
                    {
                        if (common.myInt(Request.QueryString["RequestId"]) > 0)
                        {
                            BaseC.WardManagement objw = new BaseC.WardManagement();
                            string strmsg = objw.updateReferralRequest(common.myInt(Request.QueryString["RequestId"]), "RRC", common.myInt(Session["UserId"]));

                            if (strmsg.ToUpper().Contains("UPDATE"))
                            {
                                Alert.ShowAjaxMsg("Referral Request Completed", this.Page);
                            }
                        }
                    }
                    #endregion

                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblmsg.Text = strsave;
                    if (common.myStr(Request.QueryString["OP_IP"]) == "I")
                    {
                        obje.SMSReferral(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlRefertodoctor.SelectedValue), common.myInt(ViewState["EncounterId"]));
                        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblmsg.Text = strsave;
                    }
                    Cleartext();
                }
                else
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = strsave;
                }

                BindGrid();
                //   Cleartext();
            }
            else
            {
                if (common.myInt(ViewState["SelectedEncounterId"]) != 0)
                {

                    if ((txtConclusion.Text.Length == 0) && txtConclusion.Visible == true)
                    {
                        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblmsg.Text = "Reply to referral is mandatory !";
                        txtConclusion.Focus();
                        return;
                    }

                    if (common.myStr(ViewState["IsAutoInsertIPReferral"]).Equals("Y") && tblConclusion.Visible == true)
                    {
                        if (!chkFinalized.Checked.Equals(true))
                        {
                            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblmsg.Text = "Finalized is mandatory !";
                            return;

                        }

                    }
                    UpdateReferral();
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
            objuser = null;
            obje = null;
        }
    }

    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        // RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP&OtherUserId="+ddlRefertodoctor.SelectedValue+"&UserName="+ddlRefertodoctor.Text;
        RadWindowForNew.NavigateUrl = "~/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP&OtherUserId=" + common.myStr(Session["UserId"]) + "&UserName=" + ddlRefertodoctor.Text;
        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    private void UpdateReferral()
    {
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        try
        {
            string EncounterType = "";

            string strsave = objwd.SaveReferralSlip(common.myInt(ViewState["ReferralReplyId"]), common.myInt(ViewState["ReferralId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["SelectedEncounterId"]),
                               common.myDate(dtpdate.SelectedDate), common.myInt(ddlRefertodoctor.SelectedValue), txtReason.Text,
                               common.myInt(rdoUrgent.SelectedValue), common.myBool(chkFinalized.Checked), common.myInt(Session["UserId"]), EncounterType,
                               common.myInt(ddlActive.SelectedValue), common.myStr(txtConclusion.Text), common.myInt(ddlspecilization.SelectedValue), common.myInt(ddlRequestType.SelectedValue),
                               common.myInt(Session["FacilityID"]), common.myInt(ddlReferFromDoctor.SelectedValue), 0);

            if (strsave.ToUpper().Contains("UPDATE"))
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblmsg.Text = strsave;

                #region change Referral request status to Complete
                if (common.myBool(Request.QueryString["IsCompleteRequest"]))
                {
                    if (common.myInt(Request.QueryString["RequestId"]) > 0)
                    {
                        BaseC.WardManagement objw = new BaseC.WardManagement();
                        string strmsg = objw.updateReferralRequest(common.myInt(Request.QueryString["RequestId"]), "RRC", common.myInt(Session["UserId"]));

                        if (strmsg.ToUpper().Contains("UPDATE"))
                        {
                            Alert.ShowAjaxMsg("Referral Request Completed", this.Page);
                        }
                    }
                }
                #endregion
            }
            else
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblmsg.Text = strsave;
            }
            BindGrid();
            Cleartext();
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
        }
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
                UpdateReferral();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }
    //////public void BindGrid()
    //////{
    //////    BaseC.WardManagement objwd = new BaseC.WardManagement();
    //////    DataSet ds = new DataSet();

    //////    try
    //////    {
    //////        if (common.myStr(Request.QueryString["FlagRef"]) == "All")
    //////        {
    //////            ViewState["RegistrationNo"] = "";
    //////        }
    //////        if (common.myStr(ViewState["RegistrationNo"]) == "")
    //////        {
    //////            asplHeaderUD.Visible = false;
    //////            ds = objwd.GetReferralDetail(common.myInt(Session["UserId"]), 0, "B", common.myInt(Session["FacilityId"]), "", 0, 0);
    //////        }
    //////        else
    //////        {
    //////            if (common.myStr(Request.QueryString["allreferral"]) == "yes")
    //////            {
    //////                asplHeaderUD.Visible = false;
    //////                ds = objwd.GetReferralDetail(common.myInt(Session["UserId"]), 0, "B", common.myInt(Session["FacilityId"]), "", 0, 0);


    //////            }
    //////            else
    //////            {
    //////                ds = objwd.GetReferralDetail(common.myInt(Session["UserId"]), 0, "B", common.myInt(Session["FacilityId"]), common.myStr(ViewState["RegistrationNo"]), 0, 0);

    //////            }
    //////        }

    //////        if (ds.Tables[0].Rows.Count > 0)
    //////        {
    //////            gvDetails.DataSource = ds;
    //////            gvDetails.DataBind();
    //////        }
    //////        else
    //////        {
    //////            DataRow dr = ds.Tables[0].NewRow();
    //////            ds.Tables[0].Rows.Add(dr);

    //////            gvDetails.DataSource = ds;
    //////            gvDetails.DataBind();
    //////        }
    //////    }
    //////    catch (Exception Ex)
    //////    {
    //////        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //////        lblmsg.Text = Ex.Message;
    //////        objException.HandleException(Ex);
    //////    }
    //////    finally
    //////    {
    //////        objwd = null;
    //////        ds.Dispose();
    //////    }
    //////}
    public void BindGrid()
    {
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        string regno = common.myStr(Session["RegistrationNo"]);
        try
        {
            if (common.myStr(Request.QueryString["FlagRef"]) == "All")
            {
                ViewState["RegistrationNo"] = "";
            }
            if (common.myStr(ViewState["RegistrationNo"]) == "")
            {
                if (Request.QueryString["RefReplyId"] != null)
                {
                    ds = objwd.GetReferralDetail(common.myInt(Request.QueryString["RefReplyId"]), 0, "B", common.myInt(Session["FacilityId"]), common.myStr(ViewState["RegistrationNo"]), 0, 0, 0);
                }
                else
                {
                    ds = objwd.GetReferralDetail(common.myInt(Session["UserId"]), 0, "B", common.myInt(Session["FacilityId"]), "", 0, 0, 0);
                }
            }
            else
            {
                if (Request.QueryString["RefReplyId"] != null)
                {
                    ds = objwd.GetReferralDetail(common.myInt(Request.QueryString["RefReplyId"]), 0, "B", common.myInt(Session["FacilityId"]), common.myStr(ViewState["RegistrationNo"]), 0, 0, 0);
                }
                else
                {
                    ds = objwd.GetReferralDetail((common.myInt(Request.QueryString["ReferredByUserId"]) > 0 ? common.myInt(Request.QueryString["ReferredByUserId"]) : common.myInt(Session["UserId"])), 0, "B", common.myInt(Session["FacilityId"]), common.myStr(ViewState["RegistrationNo"]), 0, 0, common.myInt(Request.QueryString["RequestId"]));
                }
            }

            if (common.myStr(ViewState["RegistrationNo"]) == "" && common.myStr(Request.QueryString["FlagRef"]) != "All")
            {
                BindBlankItemGrid();

            }
            else
            {
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

    private void BindBlankItemGrid()
    {

        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();

        try
        {

            dr["ReferralReplyId"] = 0;
            dr["ReferralId"] = 0;


            dr["Source"] = string.Empty;
            //Change Acknowledge
            dr["Acknowledge"] = string.Empty;
            dr["SpecialisationId"] = 0;

            dr["PatientName"] = string.Empty;

            dr["RegistrationNo"] = string.Empty;
            dr["ReferralDate"] = string.Empty;


            dr["FromDoctorName"] = string.Empty;
            dr["DoctorName"] = string.Empty;
            dr["Note"] = string.Empty;
            dr["DoctorRemark"] = string.Empty;
            dr["ReplyBy"] = string.Empty;
            dr["BeforeFinalizedDate"] = string.Empty;
            dr["ReferralConclusionDate"] = string.Empty;


            dr["Urgent"] = string.Empty;
            dr["ConcludeReferral"] = string.Empty;


            dr["ReferToDoctorId"] = 0;

            dr["EncodedId"] = 0;

            dr["SpecialisationId"] = 0;
            dr["EncounterId"] = 0;


            dr["CompareId"] = 0;
            dr["RegistrationId"] = 0;
            dr["WardName"] = string.Empty;
            dr["BedNo"] = 0;
            dr["EncounterNo"] = 0;
            dr["AdmissionDate"] = string.Empty;
            dr["Status"] = string.Empty;
            dr["StatusId"] = 0;

            //dr["UnitName"] = string.Empty;
            //dr["FoodRelationshipId"] = 0;
            //dr["FoodRelationship"] = string.Empty;

            ////dr["Instructions"] = string.Empty;
            //dr["DoseTypeId"] = 0;
            //dr["DoseTypeName"] = string.Empty;

            //dr["TimeUnit"] = string.Empty;

            //dr["Instruction"] = string.Empty;
            //dr["EncodedBy"] = string.Empty;

            dt.Rows.Add(dr);
            dt.AcceptChanges();


            //DataRow dr_Brow = dt.NewRow();
            //dt.Rows.InsertAt(dr_Brow, 0);



            //ViewState["ItemDetail"] = null;
            gvDetails.DataSource = dt;
            gvDetails.DataBind();

        }

        catch (Exception ex)
        {


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



            dt.Columns.Add("ReferralReplyId", typeof(int));

            dt.Columns.Add("ReferralId", typeof(int));

            dt.Columns.Add("Source", typeof(string));

            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("RegistrationNo", typeof(string));


            dt.Columns.Add("ReferralDate", typeof(string));
            dt.Columns.Add("FromDoctorName", typeof(string));
            dt.Columns.Add("DoctorName", typeof(string));
            dt.Columns.Add("Note", typeof(string));
            dt.Columns.Add("DoctorRemark", typeof(string));
            dt.Columns.Add("ReplyBy", typeof(string));
            dt.Columns.Add("BeforeFinalizedDate", typeof(string));
            dt.Columns.Add("ReferralConclusionDate", typeof(string));

            dt.Columns.Add("Urgent", typeof(string));

            dt.Columns.Add("ConcludeReferral", typeof(string));

            dt.Columns.Add("ReferToDoctorId", typeof(int));

            dt.Columns.Add("EncodedId", typeof(int));
            dt.Columns.Add("SpecialisationId", typeof(int));



            dt.Columns.Add("EncounterId", typeof(int));
            dt.Columns.Add("CompareId", typeof(int));
            dt.Columns.Add("RegistrationId", typeof(int));
            dt.Columns.Add("WardName", typeof(string));
            dt.Columns.Add("BedNo", typeof(int));
            dt.Columns.Add("EncounterNo", typeof(int));
            dt.Columns.Add("AdmissionDate", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("StatusId", typeof(int));

            return dt;
        }
        catch (Exception ex)
        {

            return dt;
        }

    }

    protected void gvDetails_ItemCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                txtConclusion.Focus();
                //new added start
                SetGridColor();
                if (e.Item.BackColor == System.Drawing.Color.LightGreen)
                {

                    //   e.Item.BackColor = System.Drawing.Color.Yellow;//Dandelion;
                    e.Item.BackColor = System.Drawing.Color.FromName("Dandelion");

                }
                else
                {

                    SetGridColor();
                }

                //new added end

                Label txConcludeReferral = (Label)e.Item.FindControl("txConcludeReferral");
                HiddenField hdnReferralReplyId = (HiddenField)e.Item.FindControl("hdnReferralReplyId");
                HiddenField hdnReferralId = (HiddenField)e.Item.FindControl("hdnReferralId");
                //Change Acknowledge
                HiddenField hdnAcknowledge = (HiddenField)e.Item.FindControl("hdnAcknowledge");
                HiddenField hdnSpecialisationId = (HiddenField)e.Item.FindControl("hdnSpecialisationId");
                
                //Change Acknowledge

                Label txtReferralDate = (Label)e.Item.FindControl("txtReferralDate");
                Label txtDoctorName = (Label)e.Item.FindControl("txtDoctorName");
                Label txtNote = (Label)e.Item.FindControl("txtNote");
                Label txDoctorRemark = (Label)e.Item.FindControl("txDoctorRemark");
                Label txtUrgent = (Label)e.Item.FindControl("txtUrgent");
                Label txReferToDoctorId = (Label)e.Item.FindControl("txReferToDoctorId");
                Label txEncodedId = (Label)e.Item.FindControl("txEncodedId");
                Label txEncounterId = (Label)e.Item.FindControl("txEncounterId");
                Label lblSource = (Label)e.Item.FindControl("lblSource");

                Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblPatientName = (Label)e.Item.FindControl("lblPatientName");
                Label lblFromDoctorName = (Label)e.Item.FindControl("txtFromDoctorName");
                Label lblWard = (Label)e.Item.FindControl("lblWardName");
                Label lblBedNumber = (Label)e.Item.FindControl("lblBedNo");
                Label lblRegistrationId = (Label)e.Item.FindControl("lblRegistrationId");
                Label lblRequestTypeStatusId = (Label)e.Item.FindControl("lblRequestTypeStatusId");

                hdnRegistrationId.Value = lblRegistrationId.Text;
                hdnRefferToDoctorID.Value = txReferToDoctorId.Text;
                //change acknowledge
                btnAcknowledge.Enabled = true;
                btnAcknowledge.ToolTip = "";
                if (common.myStr(hdnAcknowledge.Value).Equals("1"))
                {
                    btnAcknowledge.ToolTip = "Already Acknowledge";
                    btnAcknowledge.Enabled = false;
                    
                }
                if (lblRegistrationNo.Text.Trim() != string.Empty)
                {
                    lblRegistrationNoCaption.Visible = true;
                    lblRegistrationNumber.Text = lblRegistrationNo.Text;
                }
                else
                {
                    lblRegistrationNoCaption.Visible = false;
                    lblRegistrationNumber.Text = string.Empty;
                }

                if (lblPatientName.Text.Trim() != string.Empty)
                {
                    lblPatNameCaption.Visible = true;
                    lblPatName.Text = lblPatientName.Text;
                }
                else
                {
                    lblPatNameCaption.Visible = false;
                    lblPatName.Text = "";
                }

                if (lblFromDoctorName.Text.Trim() != string.Empty)
                {
                    lblFromDoctorCaption.Visible = true;
                    lblFromDoctor.Text = lblFromDoctorName.Text;
                }
                else
                {
                    lblFromDoctorCaption.Visible = false;
                    lblFromDoctor.Text = string.Empty;
                }

                if (lblWard.Text.Trim() != string.Empty)
                {
                    lblWardNameCaption.Visible = true;
                    lblWardName.Text = lblWard.Text;
                }
                else
                {
                    lblWardNameCaption.Visible = false;
                    lblWardName.Text = string.Empty;
                }

                if (lblBedNumber.Text.Trim() != string.Empty)
                {
                    lblBedCaption.Visible = true;
                    lblBed.Text = lblBedNumber.Text;
                }
                else
                {
                    lblBedCaption.Visible = false;
                    lblBed.Text = string.Empty;
                }

                ViewState["ReferralId"] = hdnReferralId.Value;
                ViewState["ReferralReplyId"] = hdnReferralReplyId.Value;
                ViewState["Source"] = lblSource.Text;
                dtpdate.SelectedDate = common.myDate(txtReferralDate.Text);
                ddlRefertodoctor.Text = "";
                ddlRefertodoctor.SelectedValue = txReferToDoctorId.Text;
                ddlRefertodoctor.Enabled = false;
                ddlspecilization.Enabled = false;
                ddlspecilization.SelectedValue = hdnSpecialisationId.Value;
                txtReason.Text = txtNote.Text;
                txtConclusion.Text = txDoctorRemark.Text;
                btnSave.Visible = true;
                lblmsg.Text = string.Empty;
                ViewState["SelectedEncounterId"] = txEncounterId.Text;

                ddlRequestType.Text = "";
                ddlRequestType.SelectedValue = common.myStr(lblRequestTypeStatusId.Text);
                if (common.myStr(lblRequestTypeStatusId.Text).Equals(string.Empty))
                {
                    ddlRequestType.SelectedIndex = 0;
                    ddlRequestType.Enabled = false;
                }
                else
                {
                    ddlRequestType.Enabled = false;
                }


                if (common.myStr(txtUrgent.Text) == "Routine")
                    rdoUrgent.SelectedValue = "0";
                else if (common.myStr(txtUrgent.Text) == "Stat")
                    rdoUrgent.SelectedValue = "2";
                else
                    rdoUrgent.SelectedValue = "1";

                if (common.myInt(Session["UserId"]) == common.myInt(txEncodedId.Text.ToString()))
                {
                    tblConclusion.Visible = false;
                }
                else
                {
                    tblConclusion.Visible = true;
                }
                // change visit status
                if (txConcludeReferral.Text.ToUpper() == "OPEN")
                {
                    btnSave.Text = "Update";
                    txtReason.Enabled = false;
                    // btnSave.Visible = true;
                    txtConclusion.Enabled = true;
                    if (!common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsAllowEditReplyToReferral", sConString).Equals("Y") && !common.myStr(txtConclusion.Text).Equals(string.Empty))
                    {
                        txtConclusion.Enabled = false;
                    }
                    chkFinalized.Enabled = true;
                    chkFinalized.Checked = false;
                    rdoUrgent.Enabled = false;
                    if (common.myInt(Session["UserId"]) == common.myInt(txEncodedId.Text.ToString()) && txtConclusion.Text.Trim() == "")
                    {
                        txtReason.Enabled = true;
                        lblActive.Visible = true;
                        ddlActive.Visible = true;
                        rdoUrgent.Enabled = true;
                    }
                    else
                    {
                        lblActive.Visible = false;
                        ddlActive.Visible = false;
                        //btnSave.Visible = false;
                    }
                }
                else // Close
                {
                    btnSave.Text = "Save (Ctrl+F3)";
                    btnSave.Visible = false;
                    txtReason.Enabled = false;
                    txtConclusion.Enabled = false;
                    chkFinalized.Enabled = false;
                    chkFinalized.Checked = true;
                    lblActive.Visible = false;
                    ddlActive.Visible = false;
                    rdoUrgent.Enabled = false;
                }

                if (common.myInt(Request.QueryString["ReferredByUserId"]) > 0)
                {
                    chkFinalized.Checked = true;
                }
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
            Label txtUrgent = (Label)e.Item.FindControl("txtUrgent");
            if (txtUrgent.Text.ToUpper() == "STAT")
            {
                e.Item.BackColor = System.Drawing.Color.LightGreen;
            }
        }
    }
    void Cleartext()
    {
        dtpdate.SelectedDate = System.DateTime.Now;
        ddlspecilization.Text = "";
        ddlspecilization.SelectedIndex = 0;
        ddlRefertodoctor.Text = "";
        ddlRefertodoctor.SelectedIndex = 0;
        txtReason.Text = "";
        rdoUrgent.SelectedValue = "0";
        txtConclusion.Text = "";
        tblConclusion.Visible = false;
        txtReason.Enabled = true;
        txtConclusion.Enabled = true;
        if (!common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsAllowEditReplyToReferral", sConString).Equals("Y") && !common.myStr(txtConclusion.Text).Equals(string.Empty))
        {
            txtConclusion.Enabled = false;
        }
        btnSave.Visible = true;
        ddlRefertodoctor.Enabled = true;
        ddlspecilization.Enabled = true;
        btnSave.Text = "Save (Ctrl+F3)";
        ViewState["SelectedEncounterId"] = "";
        ddlActive.SelectedValue = "1";
        ddlActive.Visible = false;
        lblActive.Visible = false;
        fillDoctor();
        ViewState["Source"] = "";

        lblRegistrationNoCaption.Visible = false;
        lblRegistrationNumber.Text = "";

        lblPatNameCaption.Visible = false;
        lblPatName.Text = "";

        lblFromDoctorCaption.Visible = false;
        lblFromDoctor.Text = "";

        lblWardNameCaption.Visible = false;
        lblWardName.Text = "";

        lblBedCaption.Visible = false;
        lblBed.Text = "";
        ddlRequestType.Text = "";
        ddlRequestType.SelectedIndex = 0;
        ddlRequestType.Enabled = true;

        //ddlTaggedFor.SelectedIndex = ddlTaggedFor.Items.IndexOf(ddlTaggedFor.Items.FindItemByValue("8"));
    }

    protected void lbLabHistory_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(lblRegistrationNumber.Text.Trim()) != "")
        {
            RadWindowForNew.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=0" + "&RegNo=" + common.myStr(lblRegistrationNumber.Text.Trim()) + "";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 20;
            RadWindowForNew.Left = 20;
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Please select patient!";
        }
    }

    protected void lbVisitHistory_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(lblRegistrationNumber.Text.Trim()) != "")
        {
            RadWindowForNew.NavigateUrl = "/EMR/Masters/PatientHistory.aspx?MP=NO&RegId=" + common.myStr(hdnRegistrationId.Value.ToString()) + "&RegNo=" + common.myStr(lblRegistrationNumber.Text.ToString());
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 20;
            RadWindowForNew.Left = 20;
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Please select patient!";
        }
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
                    ddlRefertodoctor.Text = string.Empty;
                    ddlRefertodoctor.SelectedValue = common.myStr(dt.Rows[0]["ReferToDoctorId"]);
                    ddlRefertodoctor.Enabled = false;
                    ddlspecilization.Enabled = false;
                    ddlRequestType.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["StatusId"]);
                    if (common.myStr(ddlRequestType.SelectedValue).Equals(string.Empty))
                    {
                        ddlRequestType.SelectedIndex = 0;
                        ddlRequestType.Enabled = false;
                    }
                    else
                    {
                        ddlRequestType.Enabled = false;
                    }
                    txtReason.Text = common.myStr(dt.Rows[0]["Note"]);
                    txtConclusion.Text = common.myStr(dt.Rows[0]["DoctorRemark"]);
                    lblmsg.Text = string.Empty;
                    btnSave.Visible = true;
                    ViewState["SelectedEncounterId"] = common.myStr(dt.Rows[0]["EncounterId"]);
                    if (common.myStr(dt.Rows[0]["Urgent"]) == "Routine")
                        rdoUrgent.SelectedValue = "0";
                    else if (common.myStr(dt.Rows[0]["Urgent"]) == "Stat")
                        rdoUrgent.SelectedValue = "2";
                    else
                        rdoUrgent.SelectedValue = "1";

                    ViewState["Source"] = common.myStr(dt.Rows[0]["Source"]);
                    if (common.myInt(Session["UserId"]) == common.myInt(dt.Rows[0]["EncodedId"]))
                    {
                        tblConclusion.Visible = false;
                    }
                    else
                    {
                        tblConclusion.Visible = true;
                    }
                    if (common.myStr(dt.Rows[0]["ConcludeReferral"]).ToUpper() == "OPEN")
                    {
                        btnSave.Text = "Update";
                        txtReason.Enabled = false;
                        btnSave.Visible = false;
                        rdoUrgent.Enabled = false;

                        txtConclusion.Enabled = true;
                        if (!common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsAllowEditReplyToReferral", sConString).Equals("Y") && !common.myStr(txtConclusion.Text).Equals(string.Empty))
                        {
                            txtConclusion.Enabled = false;
                        }
                        chkFinalized.Enabled = true;
                        chkFinalized.Checked = false;
                        if (common.myInt(Session["UserId"]) == common.myInt(dt.Rows[0]["EncodedId"]) && txtConclusion.Text.Trim() == "")
                        {
                            txtReason.Enabled = true;
                            lblActive.Visible = true;
                            ddlActive.Visible = true;
                            rdoUrgent.Enabled = true;
                            btnSave.Visible = true;
                        }
                        else
                        {
                            lblActive.Visible = false;
                            ddlActive.Visible = false;
                            btnSave.Visible = true;
                        }
                    }
                    else // Close
                    {
                        btnSave.Text = "Save (Ctrl+F3)";
                        btnSave.Visible = false;
                        txtReason.Enabled = false;
                        txtConclusion.Enabled = false;
                        chkFinalized.Enabled = false;
                        chkFinalized.Checked = true;
                        lblActive.Visible = false;
                        ddlActive.Visible = false;
                        rdoUrgent.Enabled = false;

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
        txtConclusion.Text = "";
    }

    protected void lnkBtnDoctorReferralHistory_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMR/ReferralPatientHistory.aspx?OP_IP=B&MASTER=NO&RegId=" + common.myStr(hdnRegistrationId.Value) + "&RegNo=" + common.myStr(lblRegistrationNumber.Text);
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 20;
        RadWindowForNew.Left = 20;
        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }
    private void populateRequestTypeStatus()
    {
        try
        {
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            String SqlStr = "Select * From GetStatus(@HospitalLocationId,'ReferralRequest')";
            DataSet objDs = dl.FillDataSet(CommandType.Text, SqlStr, hshInput);
            ddlRequestType.DataSource = objDs;
            ddlRequestType.DataTextField = "Status";
            ddlRequestType.DataValueField = "StatusId";
            ddlRequestType.DataBind();

            ddlRequestType.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlRequestType.SelectedIndex = 0;

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void SetGridColor()
    {
        try
        {
            foreach (GridDataItem dataItem in gvDetails.MasterTableView.Items)
            {
                if (dataItem.ItemType == GridItemType.Item
                    || dataItem.ItemType == GridItemType.AlternatingItem
                    || dataItem.ItemType == GridItemType.SelectedItem)
                {
                    //if (dataItem.BackColor == System.Drawing.Color.LightGreen)
                    //{
                    //    dataItem.BackColor = System.Drawing.Color.Yellow;

                    //}
                    string s = dataItem["Urgent"].ToString();

                    TableCell cell = dataItem["Urgent"];
                    string itemValue = (cell.Controls[0] as Label).Text;


                    if (itemValue.ToString().ToUpper() == "STAT")
                    {
                        dataItem.BackColor = System.Drawing.Color.LightGreen;
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
    }


    protected void btnAcknowledge_Click(object sender, EventArgs e)
    {
        
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        try
        {
            if (common.myInt(ViewState["ReferralId"]).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select referral request !", Page);
                
                return;
            }

           
            DataSet ds = new DataSet();
            ds = objwd.GetEmployeDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]), common.myInt(Session["UserId"]));

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string EmployeId=ds.Tables[0].Rows[0]["EmployeeID"].ToString();
                if (!common.myInt(hdnRefferToDoctorID.Value).Equals(common.myInt(EmployeId)))
                {
                    Alert.ShowAjaxMsg("Please select request referred to you !", Page);
                    return;

                }
            }

            string strsave = objwd.SendReferralAck(common.myInt(ViewState["ReferralId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["EncounterId"]),
                                 common.myInt(ddlRefertodoctor.SelectedValue),
                                 common.myInt(Session["UserId"]),
                                common.myInt(Session["FacilityID"]),
                                common.myInt(ddlReferFromDoctor.SelectedValue), common.myInt(Request.QueryString["RequestId"]));

            if (strsave.Contains("Acknowledge"))
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblmsg.Text = strsave;
                btnAcknowledge.Enabled = false;

                BindGrid();
            }
            else
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblmsg.Text = strsave;
            }

            }
        catch (Exception ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = ex.Message.ToString();
        }
    }
}
