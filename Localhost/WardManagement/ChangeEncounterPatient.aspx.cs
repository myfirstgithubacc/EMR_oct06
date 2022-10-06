using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BaseC;
using System.IO;
using System.Xml;
using System.Text;

public partial class WardManagement_ChangeEncounterPatient : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            trEdod.Visible = false;
            trStatus.Visible = false;

            if (common.myStr(Request.QueryString["RegNo"]) != "")
            {
                Session["StatusCngCheck"] = 0;
                hdnRegistrationNo.Value = common.myStr(Request.QueryString["RegNo"]);
                BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]));
            }
            if (common.myStr(Request.QueryString["encno"]) != "")
            {
                hdnEncounterNo.Value = common.myStr(Request.QueryString["encno"]);
            }
            if (common.myStr(Request.QueryString["cid"]) != "")
            {
                hdnCompanyCode.Value = Request.QueryString["cid"];
            }
            dvTemplate.Visible = false;
            BindEncounterStatus();
            BidStatus();
            BindExpiredReason();

            if (common.myStr(common.GetFlagValueHospitalSetup(Convert.ToInt16(Session["HospitalLocationID"].ToString()), Convert.ToInt16(Session["FacilityId"].ToString()), "IsShowAdvisingDoctorInMarkForDischarge", sConString)).Equals("Y"))
            {
                trAdmittingDoctor.Visible = true;
                hdnadmitingdoctor.Value = "Y";
                BindDoctor();
            }
            dtpEod.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpEod.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpEod.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            dtpEod.MinDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));

            if (common.myInt(ViewState["StatusId"]) == 183)    //Added on 29-04-2020
            {
                btnSave.Attributes.Add("onclick", "Confirm()");
            }

            dtpdeathdatetime.MinDate = common.myDate(common.myDate(Session["EncounterDate"]).ToString(common.myStr(Application["OutputDateFormat"])));

            System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

            collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsEMRReasonMandatory,IsEMRSentForBillingChecklistMandatory", sConString);

            if (collHospitalSetupValues.ContainsKey("IsEMRReasonMandatory"))
                ViewState["IsEMRReasonMandatory"] = common.myStr(collHospitalSetupValues["IsEMRReasonMandatory"]);
            if (collHospitalSetupValues.ContainsKey("IsEMRSentForBillingChecklistMandatory"))
                ViewState["IsEMRSentForBillingChecklistMandatory"] = common.myStr(collHospitalSetupValues["IsEMRSentForBillingChecklistMandatory"]);
        }
    }


    void BidStatus()
    {
        DataSet ds = new DataSet();
        BaseC.ATD objatd = new BaseC.ATD(sConString);
        try
        {
            ds = objatd.GetDischargeStatus();

            ddldischargestatus.DataSource = ds;
            ddldischargestatus.DataTextField = "Name";
            ddldischargestatus.DataValueField = "id";
            ddldischargestatus.DataBind();
            ddldischargestatus.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        }
        catch (Exception)
        {
        }
        finally
        {
            ds.Dispose();
            objatd = null;
        }
    }

    void BindExpiredReason()
    {
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        DataSet ds = new DataSet();

        try
        {
            ds = objadt.GetExpiredReason();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlExpiredReason.Items.Insert(0, "Select");
                foreach (DataRowView drReferal in ds.Tables[0].DefaultView)
                {
                    ListItem item = new ListItem();
                    item.Text = (string)drReferal["Reason"];
                    item.Value = drReferal["Id"].ToString();
                    item.Attributes.Add("ShowTxtBox", common.myStr(drReferal["ShowTxtBox"]));
                    ddlExpiredReason.Items.Add(item);
                }

                ddlExpiredReason.SelectedIndex = -1;
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ds.Dispose();
            objadt = null;
        }
    }

    void BindPatientHiddenDetails(String RegistrationNo)
    {
        try
        {
            ViewState["StatusId"] = common.myStr(Request.QueryString["StatusId"]);
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = Session["PatientDetailString"].ToString();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void Alert(string p)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertmsg", "alert('" + p + "');", true);
    }
    private bool SavePatientDeathDetail()
    {
        bool IsAllow = true;
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshtableout = new Hashtable();
        Hashtable hshtablein = new Hashtable();
        StringBuilder strapend = new StringBuilder();
        BaseC.ParseData bc = new BaseC.ParseData();
        try
        {
            if (ddldischargestatus.SelectedItem.Text.ToUpper() == "EXPIRED")
            {
                if (ddlExpiredReason.SelectedIndex == 0)
                {
                    Alert("Please! Select Expired Reason");
                    IsAllow = false;
                    return IsAllow;
                }
                if (dtpdeathdatetime.SelectedDate == null)
                {
                    Alert("Please! Fill Expired DateTime");
                    IsAllow = false;
                    return IsAllow;
                }

                hshtablein.Add("RegistrationId", common.myInt(Session["RegistrationID"]));
                hshtablein.Add("EncounterId", common.myInt(Session["EncounterId"]));
                hshtablein.Add("DeathDate", Convert.ToString(bc.ParseQ(dtpdeathdatetime.SelectedDate.ToString())));
                hshtablein.Add("BodyDeposition", Convert.ToString(bc.ParseQ(ddldepositionofbody.SelectedItem.Text.Trim())));
                hshtablein.Add("BodyReceivedBy", Convert.ToString(bc.ParseQ(txtbodyreceviedby.Text.Trim())));
                hshtablein.Add("Modeoftransfer", txtmodeoftransfer.Text.ToString().Trim());
                hshtablein.Add("Authorisedburialpermission", txtauthorised.Text.ToString().Trim());

                foreach (GridDataItem item in grddeathcause.MasterTableView.Items)
                {

                    if (item is GridDataItem)
                    {
                        DropDownList ddldoctor = (DropDownList)item.FindControl("ddldoctor");
                        TextBox txtDesc = (TextBox)item.FindControl("txtDescription");

                        if (item.RowIndex == 2)//Convert.ToInt32(GridView1.SelectedIndex)
                                               //if (Convert.ToInt32(grddeathcause.MasterTableView.SelectedIndex) == 1)
                        {
                            if (ddldoctor.SelectedIndex == 0 || txtDesc.Text == "" || txtDesc.Text == string.Empty)
                            {

                                txtDesc.Focus();
                                Alert("fill first row");
                                IsAllow = false;
                                return IsAllow;

                            }
                        }

                        if (ddldoctor.SelectedIndex != 0)
                        {
                            strapend.Append("<Table1><c1>");
                            strapend.Append(ddldoctor.SelectedValue);
                            strapend.Append("</c1><c2>");
                            strapend.Append(txtDesc.Text.Trim());
                            strapend.Append("</c2></Table1>");
                        }
                    }
                }
                hshtablein.Add("XMLDeathCause", strapend.ToString().Trim());
                hshtablein.Add("ExpiredReasonId", common.myStr(ddlExpiredReason.SelectedValue));
                hshtablein.Add("ExpiredReasonOther", txtOtherExpiredRemarks.Text.ToString().Trim());

            }
            hshtablein.Add("EncodedBy", common.myInt(Session["UserId"]));
            hshtableout.Add("@ErrorStatus", SqlDbType.VarChar);

            hshtableout = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveExpiredPatientDetail", hshtablein, hshtableout);
            lblMessage.Text = common.myStr(hshtableout["@ErrorStatus"]);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDl = null;
            bc = null;
        }
        return IsAllow;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool IsAllow = false;
        DataSet ds = new DataSet();
        BaseC.WardManagement objwd = new WardManagement();
        try
        {
            lblMessage.Text = string.Empty;

            IsAllow = SavePatientDeathDetail();
            if (!IsAllow)
            {
                return;
            }

            if (common.myInt(ddlStatus.SelectedValue) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Status not selected !";
                return;
            }
            if (common.myStr(ViewState["IsEMRReasonMandatory"]).ToUpper().Equals("Y"))
            {
                if (common.myInt(ddlStatus.SelectedValue).Equals(183) && common.myInt(ddlReason.SelectedValue).Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Reason not selected !";
                    return;
                }
            }
            if (common.myStr(ViewState["StatusCode"]) == "MD" && common.myInt(ddldischargestatus.SelectedValue) == 0
                && common.myInt(ddlStatus.SelectedValue) == 183 && !(common.myInt(ViewState["StatusId"]) == 184))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Discharge Status not selected !";
                return;
            }


            #region ---|| Special Rights wise Cancel Mark For Discharge ||---            
            if (ddlStatus.SelectedItem.Text.Equals("Cancel Marked For Discharged"))
            {
                clsEMRBilling objclsEMRBilling = new clsEMRBilling(sConString);
                if (common.myBool(objclsEMRBilling.getHospitalSetupValue("IsApplicableSpecialRightCancelMarkForDischarge",
                                  common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]))))
                {
                    BaseC.Security objSecurity = new BaseC.Security(sConString);
                    if (!common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]),
                        common.myInt(Session["FacilityId"]), "IsAuthorizedCancelMarkForDischarge")))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "You are not authorized to cancel mark for discharge!!!.";
                        return;
                    }
                }
                objclsEMRBilling = null;
            }
            #endregion

            if (common.myStr(ViewState["StatusCode"]) == "MD")
            {
                DataTable dtTemplate = objwd.GetTemplateForMandatoryForMarkForDischarge(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]));

                if (dtTemplate.Rows.Count > 0)
                {
                    dvTemplate.Visible = true;
                    lblMessage.Text = "";
                    objwd = null;
                    gvTemplate.DataSource = dtTemplate;
                    gvTemplate.DataBind();

                    dtTemplate.Dispose();

                    return;
                }
            }

            if (common.myStr(ViewState["StatusCode"]) == "MD" && common.myInt(ddladmitingdoctor.SelectedValue) == 0 && hdnadmitingdoctor.Value.Equals("Y") && common.myInt(ddlStatus.SelectedValue) == 183 && !(common.myInt(ViewState["StatusId"]) == 184))
            {
                ddladmitingdoctor.Focus();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Advising Doctor can not be left blank.";
                return;
            }
            //if (common.myStr(ViewState["SentForBilling"]) == common.myStr(ddlStatus.SelectedValue))
            //{

            //    objwd = new WardManagement();
            //    string status = objwd.IsSendToBilling(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), common.myInt(Session["UserId"]));

            //    if (common.myStr(status).ToUpper() != "Y")
            //    {
            //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //        lblMessage.Text = status;
            //        return;
            //    }

            //}

            if (common.myStr(ViewState["SentForBilling"]) == common.myStr(ddlStatus.SelectedValue))
            {
                //ds = getAcknowlegeDurgMain(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["RegID"]), common.myInt(Request.QueryString["EncounterId"]), common.myInt(ddlWard.SelectedValue),"");

                string confirmValue = Request.Form["confirm_value"];   //Added on 29-04-2020
                if (confirmValue == "No")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please return Medicines.";
                    return;
                }

                string IsDurgAck = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "DurgAcknowlegedRequire", sConString);
                if (IsDurgAck == "Y")
                {
                    ds = objwd.getAcknowlegeDurgMain(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                                    common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), 0, "U", string.Empty);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Drugs are UnAcknowleged!";
                        return;
                    }
                }

                //Added By Manoj Puri
                if (common.myStr(ViewState["IsEMRSentForBillingChecklistMandatory"]).ToUpper().Equals("Y"))
                {
                    DataSet dss = objwd.CheckDischargeCheckList(common.myInt(Session["RegistrationID"]), common.myInt(Session["EncounterId"]));

                    if (dss.Tables[0].Rows.Count <= 0)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "CheckList Not Saved !";
                        return;
                    }
                    dss.Dispose();
                }
            }

            DateTime? EOD = null;
            int dischaegetatus = 0;

            if (common.myInt(ddlStatus.SelectedValue) == 183)
            {
                EOD = common.myDate(dtpEod.SelectedDate);
                dischaegetatus = common.myInt(ddldischargestatus.SelectedValue);
            }
            if (ddldischargestatus.SelectedItem.Text.ToUpper().Equals("DAMA")
                && spnCommonRekarksMandatory.Visible && common.myLen(txtCommonRekarks.Text).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please fill remarks!";
                return;
            }

            if (common.myInt(Session["StatusCngCheck"]) == 0)
            {
                objwd = new BaseC.WardManagement();
                string str = "";
                str = objwd.UpdateEncouterStatus(common.myInt(Session["EncounterId"]), common.myInt(Session["Hospitallocationid"]),
                    common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationID"]),
                    common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserId"]), EOD, dischaegetatus,
                    common.myStr(txtCommonRekarks.Text),common.myInt(ddlReason.SelectedValue));

                if (str.Contains("Changed"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    Session["StatusCngCheck"] = 1;

                    if (common.myInt(ViewState["SentForBilling"]) == common.myInt(ddlStatus.SelectedValue))
                    {
                        string fileLoc = Server.MapPath("~/PatientDocuments/Notofication_SB" + common.myInt(Session["FacilityId"]) + ".txt");
                        string read = common.ReadNotificationFile(fileLoc);
                        ds = new DataSet("table1");
                        string ShowNotification = "N";
                        string Count = "0";
                        string StatusType = "";

                        if (read != "")
                        {
                            string xmlSchema = common.myStr(read);
                            StringReader sr = new StringReader(xmlSchema);
                            ds.ReadXml(sr);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int count = common.myInt(ds.Tables[0].Rows[0]["SBCount"]);
                                ds.Tables[0].Rows[0]["ShowNotification"] = "Y";
                                ds.Tables[0].Rows[0]["SBCount"] = count + 1;
                                ds.Tables[0].Rows[0]["PCCount"] = 0;
                                ds.Tables[0].Rows[0]["StatusType"] = "SB";
                                ds.Tables[0].AcceptChanges();
                            }
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("ShowNotification");
                            dt.Columns.Add("SBCount");
                            dt.Columns.Add("PCCount");
                            dt.Columns.Add("ICCount");
                            dt.Columns.Add("StatusType");

                            DataRow dr = dt.NewRow();
                            dr["ShowNotification"] = "Y";
                            dr["SBCount"] = 1;
                            dr["PCCount"] = 0;
                            dr["ICCount"] = 0;

                            dr["StatusType"] = "SB";
                            dt.Rows.Add(dr);
                            ds.Tables.Add(dt);
                        }
                        System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
                        System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                        ds.Tables[0].WriteXml(writer);
                        //put schema in string
                        string xmlSchemaWrite = writer.ToString();
                        XmlDocument xdoc = new XmlDocument();
                        common.WriteNotificationFile(xmlSchemaWrite, common.myInt(Session["FacilityId"]), fileLoc);
                    }
                    BindEncounterStatus();
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                }
                lblMessage.Text = str;
            }
            else if (common.myInt(Session["StatusCngCheck"]) == 1)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Already Updated";
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
            objwd = null;
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        dvTemplate.Visible = false;
    }

    void BindEncounterStatus()
    {
        BaseC.WardManagement objwd = new BaseC.WardManagement();
        EMRBilling objBill = new EMRBilling(sConString);
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        try
        {
            ds = objBill.FillStatusMaster();
            DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "Code IN ('MD','O','Ack')";
            //ddlFilter.DataSource = DV.ToTable();
            //ddlFilter.DataValueField = "StatusID";
            //ddlFilter.DataTextField = "Status";
            //ddlFilter.DataBind();
            ViewState["Acknowledge"] = "0";
            ViewState["MarkForDischarge"] = "0";
            ViewState["OPEN"] = "0";
            DV.RowFilter = "Code = 'MD'";
            if (DV.ToTable().Rows.Count > 0)
            {
                ViewState["StatusCode"] = common.myStr(DV.ToTable().Rows[0]["Code"]);
                ViewState["MarkForDischarge"] = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            DV.RowFilter = "";
            DV.RowFilter = "Code = 'Ack'";
            if (DV.ToTable().Rows.Count > 0)
            {
                ViewState["Acknowledge"] = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            DV.RowFilter = "";
            DV.RowFilter = "Code = 'SB'";
            if (DV.ToTable().Rows.Count > 0)
            {
                ViewState["SentForBilling"] = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            DV.RowFilter = "";
            DV.RowFilter = "Code = 'O'";
            if (DV.ToTable().Rows.Count > 0)
            {
                ViewState["OPEN"] = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            if ((common.myInt(ViewState["MarkForDischarge"]) == common.myInt(ViewState["StatusId"]))
                || (common.myInt(ViewState["OPEN"]) == common.myInt(ViewState["StatusId"])) || (common.myInt(ViewState["Acknowledge"]) == common.myInt(ViewState["StatusId"])))
            {
                DataSet tbl = objwd.getNextWardEncounterStatus(common.myInt(ViewState["StatusId"]));
                tbl.Tables[0].DefaultView.RowFilter = "StatusId >0";
                //palendra
                if (common.myStr(common.GetFlagValueHospitalSetup(Convert.ToInt16(Session["HospitalLocationID"].ToString()), Convert.ToInt16(Session["FacilityId"].ToString()), "IsDischargeMedicationRequiredForCompany", sConString)).Equals("Y"))
                {
                    DataSet ds1 = new DataSet();
                    Hashtable HshIn = new Hashtable();
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                    HshIn.Add("@cid", Request.QueryString["cid"]);
                    HshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));

                    string sQuery = "SELECT c.CompanyId,c.IsDischargeMedication FROM Company c WITH (NOLOCK) where  c.active = 1 and c.CompanyId=@cid AND c.HospitalLocationId = @inyHospitalLocationId ";
                    ds1 = objDl.FillDataSet(CommandType.Text, sQuery, HshIn);

                    for (int i = 0; i < tbl.Tables[0].Rows.Count; i++)
                    {
                    //Request.QueryString["cid"]
                        string statusid =common.myStr(tbl.Tables[0].Rows[i]["StatusId"]);
                        if (common.myInt(statusid) == 183 && common.myBool(ds1.Tables[0].Rows[0]["IsDischargeMedication"]).Equals(true))
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertmsg", "alert('Discharge Medication Mandatory');", true);
                        }
                            
                    }
                }
                    if (common.myInt(ViewState["StatusId"]) == 183)
                {
                    for (int i = 0; i < tbl.Tables[0].Rows.Count; i++)
                    {
                        if (common.myInt(tbl.Tables[0].Rows[i]["StatusId"]) == 197)
                        {
                            tbl.Tables[0].Rows[i]["StatusId"] = 184;
                            tbl.Tables[0].Rows[i]["EncounterStatus"] = "Sent For Billing";
                        }
                    }
                    tbl.Tables[0].AcceptChanges();
                }
                else if (common.myInt(ViewState["StatusId"]) == 184)
                {
                    for (int i = 0; i < tbl.Tables[0].Rows.Count; i++)
                    {
                        if (common.myInt(tbl.Tables[0].Rows[i]["StatusId"]) == 197)
                        {
                            tbl.Tables[0].Rows[i].Delete();
                            tbl.Tables[0].AcceptChanges();
                            break;
                        }
                    }
                }
                else if (common.myInt(ViewState["StatusId"]) == 197)
                {
                    for (int i = 0; i < tbl.Tables[0].Rows.Count; i++)
                    {
                        if (common.myInt(tbl.Tables[0].Rows[i]["StatusId"]) == 183)
                        {
                            tbl.Tables[0].Rows[i].Delete();
                            tbl.Tables[0].AcceptChanges();
                            break;
                        }
                    }
                }
                else if (common.myInt(ViewState["StatusId"]) == 7)
                {
                    if (common.myInt(tbl.Tables[0].Rows[1]["StatusId"]) == 183)
                    {
                        trEdod.Visible = true;
                        trStatus.Visible = true;
                    }
                }
                else
                {
                    tbl.Tables[0].Rows[0].Delete();
                    tbl.Tables[0].Rows[1].Delete();
                    tbl.Tables[0].AcceptChanges();
                }
                ddlStatus.DataSource = tbl.Tables[0].DefaultView;
                ddlStatus.DataTextField = "EncounterStatus";
                ddlStatus.DataValueField = "StatusId";
                ddlStatus.DataBind();

                ddlStatus.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlStatus.SelectedIndex = 0;
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
            objwd = null;
            objBill = null;
            ds.Dispose();
            DV.Dispose();
        }
    }
    protected void ddldischargestatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        showdischargepanel();
    }
    protected void grddeathcause_ItemCreated(object sender, GridItemEventArgs e)
    {

    }
    protected void grddeathcause_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DropDownList ddldoctor = (DropDownList)e.Item.FindControl("ddldoctor");
                ddldoctor.SelectedIndex = 0;

                TextBox txtDescription = (TextBox)e.Item.FindControl("txtDescription");
            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void showdischargepanel()
    {
        try
        {
            if (ddldischargestatus.SelectedItem.Text.ToUpper().Equals("EXPIRED"))
            {
                pnldeathdetails.Visible = true;
                dtpdeathdatetime.SelectedDate = null;
                pnlPaymentDetail.Visible = true;
                ddlExpiredReason.Visible = true;
                lnkMultiplePayMode();
                dvCommonRekarks.Visible = false;
            }
            else
            {
                pnldeathdetails.Visible = false;
                pnlPaymentDetail.Visible = false;
                grddeathcause.Visible = false;
                ddlExpiredReason.Visible = false;
                dvCommonRekarks.Visible = true;

                spnCommonRekarksMandatory.Visible = (ddldischargestatus.SelectedItem.Text.ToUpper().Equals("DAMA"));

            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void lnkMultiplePayMode()
    {
        try
        {
            grddeathcause.Visible = true;
            //Bind Payment mode gird

            DataTable defT;
            DataColumn defC;
            DataRow defR;
            defT = new DataTable("Doctor");
            defC = new DataColumn("DoctorId", typeof(Int32));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            defC = new DataColumn("Description", typeof(String));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            for (int i = 0; i < 4; i++)
            {
                defR = defT.NewRow();
                defR["DoctorId"] = 1;

                defR["Description"] = "";
                defT.Rows.Add(defR);
            }
            Cache["DataTable"] = defT;
            grddeathcause.DataSource = defT;
            grddeathcause.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlExpiredReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpiredReason.SelectedIndex > 0)
        {
            if (common.myStr(ddlExpiredReason.SelectedItem.Attributes["ShowTxtBox"]).Equals("T"))
                txtOtherExpiredRemarks.Visible = true;
            else
                txtOtherExpiredRemarks.Visible = false;
        }
    }

    public void BindDoctor()
    {
        Hospital baseHosp = new Hospital(sConString);
        DataSet dsDoctor = new DataSet();
        try
        {
            ddladmitingdoctor.Items.Clear();
            ddladmitingdoctor.Text = "";

            if (common.myStr(common.GetFlagValueHospitalSetup(Convert.ToInt16(Session["HospitalLocationID"].ToString()), Convert.ToInt16(Session["FacilityId"].ToString()), "ShowAdmittingDoctorAsPerCompanyType", sConString)).Equals("N"))
            {
                dsDoctor = baseHosp.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationID"].ToString()), 0, Convert.ToInt16(Session["FacilityId"].ToString()));
                if (dsDoctor.Tables.Count > 0)
                {
                    dsDoctor.Tables[0].DefaultView.RowFilter = "AdmissionAuthorised=True OR AdmissionAuthorised=1";
                    if (dsDoctor.Tables[0].Rows.Count > 0)
                    {
                        if (dsDoctor.Tables[0].DefaultView.Count > 0)
                        {
                            foreach (DataRow dr in (dsDoctor.Tables[0].DefaultView).ToTable().Rows)
                            {
                                RadComboBoxItem itemsecdoctor = new RadComboBoxItem();
                                itemsecdoctor.Text = (string)dr["ShowDoctorNameWithSpeciality"];
                                itemsecdoctor.Value = Convert.ToString(dr["DoctorID"]);
                                itemsecdoctor.Attributes.Add("isRequiredSecondoryDoctorOnATD", common.myStr(dr["isRequiredSecondoryDoctorOnATD"]));
                                this.ddladmitingdoctor.Items.Add(itemsecdoctor);
                                itemsecdoctor.DataBind();
                            }
                            ddladmitingdoctor.Items.Insert(0, new RadComboBoxItem("", "0"));
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
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {  
        WardManagement objwd = new BaseC.WardManagement();
        DataTable dt = new DataTable();
        try
        {
            dvReason.Visible = false;
            if (common.myInt(ddlStatus.SelectedValue).Equals(183) && common.myStr(ViewState["IsEMRReasonMandatory"]).ToUpper().Equals("Y"))
            {
                dvReason.Visible = true;
                dt = objwd.GetReasonMasterList(1, common.myInt(Session["FacilityId"]), "MD");
                if (common.myInt(dt.Rows.Count) > 0)
                {
                    ddlReason.DataSource = dt;
                    ddlReason.DataTextField = "ReasonName";
                    ddlReason.DataValueField = "ReasonId";
                    ddlReason.DataBind();
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
            objwd = null;
            dt.Dispose();
        }       
    }

    //Added By Manoj Puri
    protected void btnCheckListSFB_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["SentForBilling"]) == common.myStr(ddlStatus.SelectedValue))
        {
            RadWindow1.NavigateUrl = "/PRegistration/Checklist.aspx?From=D&RegistrationId=" + common.myInt(Session["RegistrationID"]) + "&RegistrationNo=" + hdnRegistrationNo.Value + "&EncounterId= " + common.myInt(Session["EncounterId"]);
            RadWindow1.Height = 550;
            RadWindow1.Width = 700;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.Title = "Discharge Checklist";
            RadWindow1.Modal = true;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindow1.VisibleStatusbar = false;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Select Status as Sent For Billing !";
            return;
        }
    }
}
