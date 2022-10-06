using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMR_PACEMR_PACDashboard : System.Web.UI.Page
{
    private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {

            ViewState["UserSpecialisationId"] = Session["LoginSpecialisationID"];
            dtpFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtpFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtpToDate.SelectedDate = DateTime.Now;
            BindGroupTaggingMenu();
            BindSpeciliazation();
            BindGrid();
        }
    }
    private void BindGroupTaggingMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet dsp = new DataSet();

        try
        {
            
            RadMenuItem menu;
            menuStatus.Items.Clear();

            menu = new RadMenuItem();
            menu.Value = "EMR";
            menu.Text = "EMR";
            menu.Attributes.Add("Code", "EMR");
            menu.Attributes.Add("TemplateId", "0");
            menuStatus.Items.Add(menu);
            menu = new RadMenuItem();
            menu.Value = "PCS";
            menu.Text = "Past Clinical History";
            menu.Attributes.Add("Code", "PCS");
            menu.Attributes.Add("TemplateId", "0");

            menuStatus.Items.Add(menu);
            menuStatus.DefaultGroupSettings.Height = Unit.Percentage(100);

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            user = null;
            dsp.Dispose();
        }
    }
    private void BindGrid()
    {
        BaseC.clsEMR obj = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            int HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int DoctorId = common.myInt(ddlProvider.SelectedItem.Value);
            int EncodedBy = common.myInt(Session["UserId"]);
            int RegistrationNo = 0;
            string MobileNo = "";
            string PatientName = "";
            if (ddlName.SelectedItem.Value.Equals("R"))
                RegistrationNo = common.myInt(txtSearchN.Text);
            else if (ddlName.SelectedItem.Value.Equals("MN"))
                MobileNo = txtSearch.Text;
            else if (ddlName.SelectedItem.Value.Equals("M"))
                PatientName = txtSearch.Text;
            string FromDate = common.myDate(dtpFromDate.SelectedDate).ToString("yyyy/MM/dd");
            string ToDate = common.myDate(dtpToDate.SelectedDate).ToString("yyyy/MM/dd");
            int PACStatus = common.myInt(rblPACStatus.SelectedItem.Value);

            ds = obj.GetPACDashboard(HospitalLocationId, FacilityId, DoctorId, EncodedBy, RegistrationNo, MobileNo, PatientName, PACStatus, FromDate, ToDate);
            gvEncounter.DataSource = ds;
            gvEncounter.DataBind();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            obj = null;
            ds.Dispose();            
        }
    }
    private void BindProvider()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet objDs = new DataSet();
        Hashtable hsIn = new Hashtable();
        try
        {
            hsIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsIn.Add("@iUserId", common.myInt(Session["UserId"]));
            hsIn.Add("@intSpecializationId", common.myInt(ddlSpecilization.SelectedValue));
            hsIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            objDs = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeWithResource", hsIn);
            
            ddlProvider.DataSource = objDs;
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorID";
            ddlProvider.DataBind();

            CheckUserDoctorOrNot();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objDl = null;
            hsIn = null;
            objDs.Dispose();
        }
    }
    private void CheckUserDoctorOrNot()
    {
        BaseC.EMR objEmr = new BaseC.EMR(sConString);
        try
        {
            if (Session["UserID"] != null)
            {
                if (common.myInt(Session["LoginIsAdminGroup"]) == 1)
                {
                    ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                    ddlProvider.Items[0].Selected = true;
                    ddlProvider.Enabled = true;
                    ViewState["IsDoctor"] = "0";
                }
                else
                {
                    DataSet ds = objEmr.CheckUserDoctorOrNotDS(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));
                    if (ds != null)
                    {
                        if ((ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && common.myStr(ds.Tables[0].Rows[0][0]) != "") && (ds.Tables[0].Rows[0][0] != null) && (common.myInt(Session["LoginIsAdminGroup"]) == 0))
                        {
                            if (ddlProvider.Items.Count > 0)
                            {
                                ddlProvider.Items[0].Selected = false;
                            }
                            ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.FindItemByValue(common.myStr(ds.Tables[0].Rows[0][0])));
                            ddlSpecilization.Enabled = false;
                            ViewState["IsDoctor"] = "1";
                        }
                        else
                        {
                            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", ""));
                            ddlProvider.Items[0].Selected = true;
                            ViewState["IsDoctor"] = "0";
                        }
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objEmr = null;
        }
    }
    private void BindSpeciliazation()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsh = new Hashtable();
        DataSet dsSpeciliazation = new DataSet();
        try
        {
            hsh.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsh.Add("@intFacilityId", common.myInt(Session["facilityId"]));

            dsSpeciliazation = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorTimeSpecialisation", hsh);
            if (dsSpeciliazation.Tables[0].Rows.Count > 0)
            {
                ddlSpecilization.Items.Clear();
                ddlSpecilization.DataSource = dsSpeciliazation.Tables[0];
                ddlSpecilization.DataTextField = "NAME";
                ddlSpecilization.DataValueField = "id";
                ddlSpecilization.DataBind();
                ddlSpecilization.Items.Insert(0, new RadComboBoxItem("All", "0"));
                if (ViewState["UserSpecialisationId"] != null)
                {
                    ddlSpecilization.SelectedValue = common.myStr(ViewState["UserSpecialisationId"]);
                }
                ddlSpecilization_SelectedIndexChanged(null, null);
            }
            else
            {
                ddlSpecilization.Text = "";
                ddlSpecilization.Items.Clear();
                Alert.ShowAjaxMsg("Specialization not available", Page);
                return;
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            dl = null;
            hsh = null;
            dsSpeciliazation.Dispose();
        }
    }
    protected void ddlSpecilization_SelectedIndexChanged(Object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindProvider();        
    }
    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        txtSearchN.Text = "";

        if (common.myStr(ddlName.SelectedValue) == "R")
        {
            txtSearch.Visible = false;
            txtSearchN.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            txtSearchN.Visible = false;
        }
    }
    
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void gvEncounter_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvEncounter.CurrentPageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void gvEncounter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            HiddenField hdnOTRequestID = (HiddenField)e.Item.FindControl("hdnOTRequestID");
            HiddenField hdnIsEmergency = (HiddenField)e.Item.FindControl("hdnIsEmergency");
            HiddenField hdnRegistrationId = (HiddenField)e.Item.FindControl("hdnRegistrationId");
            HiddenField hdnOTREncounterId = (HiddenField)e.Item.FindControl("hdnOTREncounterId");
            HiddenField hdnEncounterId = (HiddenField)e.Item.FindControl("hdnEncounterId");
            HiddenField hdnEncounterNo = (HiddenField)e.Item.FindControl("hdnEncounterNo");
            HiddenField hdnOTREncounterNo = (HiddenField)e.Item.FindControl("hdnOTREncounterNo");
            HiddenField hdnDoctorName = (HiddenField)e.Item.FindControl("hdnDoctorName");
            HiddenField hdnDoctorId = (HiddenField)e.Item.FindControl("hdnDoctorId");
            HiddenField hdnRefDoctorId = (HiddenField)e.Item.FindControl("hdnRefDoctorId");
            Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
            Label lblPatientName = (Label)e.Item.FindControl("lblPatientName");
            Label lblRefDoctorName = (Label)e.Item.FindControl("lblRefDoctorName");
            //for (int i = 0; i <= e.Item.Cells.Count - 7; i++)
            //{
            //    e.Item.Cells[i].Attributes.Add("OnClick", "showMenu(event,'" + menuStatus.ClientID + "','" + //menu
            //                                            common.myInt(hdnRegistrationId.Value) + "','" + // RegistrationId
            //                                            common.myInt(hdnEncounterId.Value) + "','" + // EncounterId
            //                                            common.myStr(lblRegistrationNo.Text) + "','" + // RegistrationNo
            //                                            common.myStr(common.myStr(hdnEncounterNo.Value)) + "','" + // EncounterNo
            //                                            common.myStr(lblPatientName.Text) + "','" + // PatientName
            //                                            common.myStr(hdnDoctorName.Value) + "','" + // DoctorName
            //                                            common.myInt(hdnDoctorId.Value) + "','" + // DoctorId                                     
            //                                            common.myInt(hdnOTRequestID.Value) + "','" + //OTRequestID
            //                                            common.myInt(hdnRefDoctorId.Value) + "','" + // RefDoctorId 
            //                                            common.myInt(hdnOTREncounterId.Value) + "','" + // RefEncounterId   
            //                                            common.myInt(e.Item.ItemIndex) + "')"); //RowIdx
            //}
        }
    }

    protected void gvEncounter_PreRender(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void menuStatus_ItemClick(object sender, RadMenuEventArgs e)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Patient bC = new BaseC.Patient(sConString);
        Hashtable hshInput = new Hashtable();
        DataSet objDs = new DataSet();
        DataSet dsPatientDetail = new DataSet();
        DataSet ds = new DataSet();

        string sRegNo = common.myStr(hdnMnuRegNo.Value);
        string sEncNo = common.myStr(hdnMnuEncNo.Value);
        string sRegId = common.myStr(hdnMnuRegId.Value);
        string sEncId = common.myStr(hdnMnuEncId.Value);
        string sPatName = common.myStr(hdnMnuPatName.Value);
        string sDocName = common.myStr(hdnMnuDocName.Value);
        string sDoctorId = common.myStr(hdnMnuDoctorId.Value);
        string sOTRId = common.myStr(hdnMnuOTRequestID.Value);
        string sRefDocId = common.myStr(hdnMnuRefDoctorId.Value);
        string sRefEId = common.myStr(hdnMnuRefEncounterId.Value);




        if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]) == "EMR")
        {
            hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hshInput.Add("@intEncounterId", common.myInt(sEncId));
            hshInput.Add("@intLoginFacilityId", common.myInt(Session["FacilityId"]));
            objDs = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDoctorPatientLists", hshInput);




            Session["IsMedicalAlert"] = null;
            Session["IsAllergiesAlert"] = null;
            Session["Gender"] = objDs.Tables[0].Rows[0]["Gender"];
            Session["Facility"] = objDs.Tables[0].Rows[0]["FacilityId"];
            Session["AppointmentID"] = objDs.Tables[0].Rows[0]["AppointmentID"];
            Session["RegistrationID"] = objDs.Tables[0].Rows[0]["RegistrationId"];
            Session["RegistrationNo"] = objDs.Tables[0].Rows[0]["RegistrationNo"];
            Session["encounterid"] = objDs.Tables[0].Rows[0]["encounterid"];
            Session["DoctorID"] = objDs.Tables[0].Rows[0]["DoctorID"];
            Session["FollowUpDoctorId"] = objDs.Tables[0].Rows[0]["DoctorID"];
            Session["EncounterStatus"] = objDs.Tables[0].Rows[0]["Status"];
            Session["BillingstatusId"] = objDs.Tables[0].Rows[0]["BillingstatusId"];
            Session["IVFId"] = objDs.Tables[0].Rows[0]["IVFId"];
            Session["IVFNo"] = objDs.Tables[0].Rows[0]["IVFNo"];
            Session["EncVisitPackageName"] = objDs.Tables[0].Rows[0]["PackageName"];
            Session["EncounterNo"] = objDs.Tables[0].Rows[0]["EncounterNo"];
            Session["OPIP"] = objDs.Tables[0].Rows[0]["OPIP"];
            Session["PatientEncStatusCode"] = objDs.Tables[0].Rows[0]["StatusCode"];
            Session["PatientName"] = objDs.Tables[0].Rows[0]["Name"];
            Session["EncounterDate"] = objDs.Tables[0].Rows[0]["EncounterDate"];
            Session["MedicalAlert"] = objDs.Tables[0].Rows[0]["MedicalAlert"];
            Session["AllergiesAlert"] = objDs.Tables[0].Rows[0]["AllergiesAlert"];
            Session["AgeGender"] = objDs.Tables[0].Rows[0]["AgeGender"];

            int iLastVisit = common.myInt(objDs.Tables[0].Rows[0]["LastVisit"]);
            string LastVisit = "";
            if (iLastVisit <= 1)
            {
                LastVisit = "First Visit";
            }
            else
            {
                LastVisit = "Last Visit " + iLastVisit.ToString() + " Day(s) ago visit ";
            }
            Session["LastVisit"] = LastVisit;

            #region PatientDetailStringOP
            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["PatientDetailString"] = null;
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(Session["RegistrationNo"]),
                common.myStr(Session["EncounterNo"]), common.myInt(Session["UserId"]));
            dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
            if (dsPatientDetail.Tables.Count > 0)
            {
                if (dsPatientDetail.Tables[0].Rows.Count > 0)
                {
                    string CardValidity = "";
                    if (dsPatientDetail.Tables[0].Rows[0]["CardValidFrom"] != null)
                    {
                        CardValidity = "&nbsp;Card Validity:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["CardValidFrom"]) + " - " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["ValidupTo"]) + "</span>";
                    }
                    string Lasvisitsession = "&nbsp;<span style = 'color:white;font-weight: bold;background-color:green;'> " + common.myStr(Session["LastVisit"]) + " </span> ";

                    string sRegNoTitle = Resources.PRegistration.regno;
                    string sDoctorTitle = Resources.PRegistration.Doctor;
                    string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                    Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>"
                        + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", "
                     + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"])
                     + ", DOB: " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DOB"]) + "</span>"
                     + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                     + "&nbsp;Enc #:" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
                     + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
                     + DateTitle + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"

                     + "&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                     + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"
                     + CardValidity
                     + Lasvisitsession
                     + "</b>";

                    Session["PrescriptionPatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>"
                        + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"])
                        + ", Gender: " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Gender"])
                        + ", DOB: " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DOB"]) + "</span>"
                  + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"

                  + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"

                  + "&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                  + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"

                  + "</b>";
                }
            }

            #endregion

            if (Session["ModuleData"] != null)
            {
                ds = (DataSet)Session["ModuleData"];
                if (ds != null)
                {
                    int i = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (common.myStr(dr["ModuleName"]) == "EMR")
                        {
                            Session["ModuleId"] = dr["ModuleId"];
                            Session["ModuleName"] = dr["ModuleName"];
                        }
                        else if (common.myStr(dr["ModuleName"]) == "Nurse Workbench")
                        {
                            if (common.myStr(Session["EmployeeType"]).Trim() == "N")
                            {
                                Session["ModuleId"] = dr["ModuleId"];
                                Session["ModuleName"] = dr["ModuleName"];
                            }
                        }
                        i++;
                    }
                }
            }
            Session["formId"] = "1";
            Response.Redirect("/EMR/Dashboard/PatientDashboardForDoctor.aspx", false);
        }

        else if (common.myStr(menuStatus.SelectedItem.Attributes["Code"]) == "EMR")
        {
            Session["SelectedCaseSheet"] = "PN";
            RadWindowForNew.NavigateUrl = "/WardManagement/VisitHistory.aspx?RNo=" + sRegNo
                                + "&Regid=" + sRegId + "&RegNo=" + sRegNo
                                + "&EncId=" + sEncId + "&EncNo=" + sEncNo
                                + "&FromWard=Y&OP_IP=I&Category=PopUp";
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = string.Empty;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
        }
    }

    protected void gvEncounter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            HiddenField hdnEncounterId = (HiddenField)e.Item.FindControl("hdnEncounterId");

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.Patient bC = new BaseC.Patient(sConString);
            Hashtable hshInput = new Hashtable();
            DataSet objDs = new DataSet();
            DataSet dsPatientDetail = new DataSet();
            DataSet ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hshInput.Add("@intEncounterId", common.myInt(hdnEncounterId.Value));
            hshInput.Add("@intLoginFacilityId", common.myInt(Session["FacilityId"]));
            objDs = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDoctorPatientLists", hshInput);




            Session["IsMedicalAlert"] = null;
            Session["IsAllergiesAlert"] = null;
            Session["Gender"] = objDs.Tables[0].Rows[0]["Gender"];
            Session["Facility"] = objDs.Tables[0].Rows[0]["FacilityId"];
            Session["AppointmentID"] = objDs.Tables[0].Rows[0]["AppointmentID"];
            Session["RegistrationID"] = objDs.Tables[0].Rows[0]["RegistrationId"];
            Session["RegistrationNo"] = objDs.Tables[0].Rows[0]["RegistrationNo"];
            Session["encounterid"] = objDs.Tables[0].Rows[0]["encounterid"];
            Session["DoctorID"] = objDs.Tables[0].Rows[0]["DoctorID"];
            Session["FollowUpDoctorId"] = objDs.Tables[0].Rows[0]["DoctorID"];
            Session["EncounterStatus"] = objDs.Tables[0].Rows[0]["Status"];
            Session["BillingstatusId"] = objDs.Tables[0].Rows[0]["BillingstatusId"];
            Session["IVFId"] = objDs.Tables[0].Rows[0]["IVFId"];
            Session["IVFNo"] = objDs.Tables[0].Rows[0]["IVFNo"];
            Session["EncVisitPackageName"] = objDs.Tables[0].Rows[0]["PackageName"];
            Session["EncounterNo"] = objDs.Tables[0].Rows[0]["EncounterNo"];
            Session["OPIP"] = objDs.Tables[0].Rows[0]["OPIP"];
            Session["PatientEncStatusCode"] = objDs.Tables[0].Rows[0]["StatusCode"];
            Session["PatientName"] = objDs.Tables[0].Rows[0]["Name"];
            Session["EncounterDate"] = objDs.Tables[0].Rows[0]["EncounterDate"];
            Session["MedicalAlert"] = objDs.Tables[0].Rows[0]["MedicalAlert"];
            Session["AllergiesAlert"] = objDs.Tables[0].Rows[0]["AllergiesAlert"];
            Session["AgeGender"] = objDs.Tables[0].Rows[0]["AgeGender"];

            int iLastVisit = common.myInt(objDs.Tables[0].Rows[0]["LastVisit"]);
            string LastVisit = "";
            if (iLastVisit <= 1)
            {
                LastVisit = "First Visit";
            }
            else
            {
                LastVisit = "Last Visit " + iLastVisit.ToString() + " Day(s) ago visit ";
            }
            Session["LastVisit"] = LastVisit;

            #region PatientDetailStringOP
            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["PatientDetailString"] = null;
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(Session["RegistrationNo"]),
                common.myStr(Session["EncounterNo"]), common.myInt(Session["UserId"]));
            dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
            if (dsPatientDetail.Tables.Count > 0)
            {
                if (dsPatientDetail.Tables[0].Rows.Count > 0)
                {
                    string CardValidity = "";
                    if (dsPatientDetail.Tables[0].Rows[0]["CardValidFrom"] != null)
                    {
                        CardValidity = "&nbsp;Card Validity:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["CardValidFrom"]) + " - " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["ValidupTo"]) + "</span>";
                    }
                    string Lasvisitsession = "&nbsp;<span style = 'color:white;font-weight: bold;background-color:green;'> " + common.myStr(Session["LastVisit"]) + " </span> ";

                    string sRegNoTitle = Resources.PRegistration.regno;
                    string sDoctorTitle = Resources.PRegistration.Doctor;
                    string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date :" : "Encounter Date :";
                    Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>"
                        + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", "
                     + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"])
                     + ", DOB: " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DOB"]) + "</span>"
                     + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                     + "&nbsp;Enc #:" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
                     + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
                     + DateTitle + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"

                     + "&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                     + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"
                     + CardValidity
                     + Lasvisitsession
                     + "</b>";

                    Session["PrescriptionPatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>"
                        + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"])
                        + ", Gender: " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Gender"])
                        + ", DOB: " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DOB"]) + "</span>"
                  + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"

                  + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"

                  + "&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                  + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["Payername"]) + "</span>"

                  + "</b>";
                }
            }

            #endregion

            if (Session["ModuleData"] != null)
            {
                ds = (DataSet)Session["ModuleData"];
                if (ds != null)
                {
                    int i = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (common.myStr(dr["ModuleName"]) == "EMR")
                        {
                            Session["ModuleId"] = dr["ModuleId"];
                            Session["ModuleName"] = dr["ModuleName"];
                        }
                        else if (common.myStr(dr["ModuleName"]) == "Nurse Workbench")
                        {
                            if (common.myStr(Session["EmployeeType"]).Trim() == "N")
                            {
                                Session["ModuleId"] = dr["ModuleId"];
                                Session["ModuleName"] = dr["ModuleName"];
                            }
                        }
                        i++;
                    }
                }
            }
            Session["formId"] = "1";
            Session["FindPatientExpanded"] = "1";
            Response.Redirect("/EMR/Dashboard/PatientDashboardForDoctor.aspx", false);
        }
    }

    protected void rblPACStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblPACStatus.SelectedValue == "0")
            dvDateRange.Visible = false;
        else
            dvDateRange.Visible = true;
    }
}