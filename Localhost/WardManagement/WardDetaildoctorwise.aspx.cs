using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Configuration;

public partial class WardManagement_WardDetaildoctorwise : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    Image img = new Image();
    BaseC.ATD Objstatus;
    BaseC.WardManagement objwm;
    DataSet ds;
    clsExceptionLog objException = new clsExceptionLog();
    private const int ItemsPerRequest = 50;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myBool(Session["IsLoginDoctor"]) != true)
            {
                tdWard.Visible = true;
            }

            bindControl();
            setStatus();
            BindStatus();
            BindGroupTaggingMenu();
            ViewState["IndentStoreId"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                        common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString);

            fillData(common.myInt(ddlColumns.SelectedValue));
            fillSampleCollectedStatus();
        }
    }




    private void bindControl()
    {
        try
        {
            ds = new DataSet();

            BaseC.ATD objadt = new BaseC.ATD(sConString);
            ds = objadt.GetWardStation(common.myInt(Session["FacilityId"]), common.myInt(Session["EmployeeId"]));

            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "StationName";
            ddlWard.DataValueField = "ID";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlWard.SelectedIndex = 0;

            BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
            ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "Encounter", "");

            DataTable tbl = ds.Tables[0];

            RadComboBoxItem lst;
            for (int idx = 0; idx < tbl.Rows.Count; idx++)
            {
                lst = new RadComboBoxItem();
                lst.Attributes.Add("style", "background-color:" + common.myStr(tbl.Rows[idx]["StatusColor"]) + ";");

                lst.Value = common.myStr(tbl.Rows[idx]["StatusId"]);
                lst.Text = common.myStr(tbl.Rows[idx]["Status"]);

                ddlEncounterStatus.Items.Add(lst);
            }
            BaseC.clsEMRBilling objval1 = new BaseC.clsEMRBilling(sConString);
            DataSet DSS = objval1.getEntrySite(common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]));
            ddlEntrySitesActual.DataSource = DSS;
            ddlEntrySitesActual.DataValueField = "ESId";
            ddlEntrySitesActual.DataTextField = "ESName";
            ddlEntrySitesActual.DataBind();
            //ddlEntrySitesDiag.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlEntrySitesDiag.SelectedIndex = 0;
            ddlEntrySitesActual.SelectedIndex = ddlEntrySitesActual.Items.IndexOf(ddlEntrySitesActual.Items.FindItemByValue(common.myStr("0")));
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindGroupTaggingMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                common.myInt(Session["ModuleId"]), "WARDDETAILS");
            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            RadMenuItem menu;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                menu = new RadMenuItem();
                menu.Value = common.myStr(ds.Tables[0].Rows[i]["OptionCode"]);
                menu.Text = common.myStr(ds.Tables[0].Rows[i]["OptionName"]);
                menu.Attributes.Add("Code", common.myStr(ds.Tables[0].Rows[i]["OptionCode"]));
                menu.Attributes.Add("OptionId", common.myStr(ds.Tables[0].Rows[i]["OptionId"]));
                RadContextMenu2.Items.Add(menu);
            }
            if (ds.Tables[0].Rows.Count <= 20)
            {
                RadContextMenu2.DefaultGroupSettings.Height = Unit.Percentage(100);
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            user = null;
            ds.Dispose();
        }
    }

    private void setStatus()
    {
        try
        {
            /* set StatusId of Open, MarkedForDischarged */
            hdnOpenEncStatusId.Value = "0";
            hdnMarkedForDischargedEncStatusId.Value = "0";

            BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
            ds = objPharmacy.getStatus(common.myInt(Session["HospitalLocationID"]), "Encounter", "", 0);

            DataView DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "Code = 'O' AND Active = 1";

            if (DV.ToTable().Rows.Count > 0)
            {
                hdnOpenEncStatusId.Value = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }

            DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "Code = 'MD' AND Active = 1";

            if (DV.ToTable().Rows.Count > 0)
            {
                hdnMarkedForDischargedEncStatusId.Value = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            //hdnAcknoledgeEncStatusId
            //Done By Ujjwal 20March2015 to assign value for new status [Acknowledge] before open status start
            DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "Code = 'Ack' AND Active = 1";

            if (DV.ToTable().Rows.Count > 0)
            {
                hdnAcknoledgeEncStatusId.Value = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            //Done By Ujjwal 20March2015 to assign value for new status [Acknowledge] before open status end
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void BindStatus()
    {
        try
        {
            ds = new DataSet();
            BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
            ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "BedStatus", "");
            DataTable tbl = ds.Tables[0];
            RadComboBoxItem lst;
            for (int idx = 0; idx < tbl.Rows.Count; idx++)
            {
                lst = new RadComboBoxItem();
                lst.Attributes.Add("style", "background-color:" + common.myStr(tbl.Rows[idx]["StatusColor"]) + ";");

                lst.Value = common.myStr(tbl.Rows[idx]["Code"]);
                lst.Text = common.myStr(tbl.Rows[idx]["Status"]);

                lst.Font.Bold = true;
                lst.ForeColor = System.Drawing.Color.Black;

                ddlBedStatus.Items.Add(lst);
            }
        }
        catch (Exception Ex)
        {
            //lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblmsg.Text = "Error: " + Ex.Message;
            //objException.HandleException(Ex);
        }

    }
    public void fillData(int Columns)
    {
        try
        {
            BaseC.WardManagement Objstatus = new BaseC.WardManagement();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataTable dt = new DataTable();

            DataSet objds = new DataSet();
            int i = 0, noOfColumn = Columns;
            DataRow dr1 = null;
            string wardname = "";
            Hashtable hsinput = new Hashtable();
            string BedStatusId = "";
            if (common.myStr(ddlBedStatus.SelectedValue) != "")
            {
                BedStatusId = ddlBedStatus.SelectedValue;
            }


            int doctorId = 0;
            if (common.myBool(Session["IsLoginDoctor"]))
            {
                doctorId = common.myInt(Session["LoginDoctorId"]);
            }

            int wardId = common.myInt(ddlWard.SelectedValue);
            if (common.myInt(hdnregID.Value) > 0 && ddlPatient.SelectedValue != "")
            {
                wardId = 0;
                ddlWard.SelectedIndex = 0;
                ddlBedStatus.SelectedValue = "";
                BedStatusId = "";
            }

            if (common.myStr(ViewState["dt"]) == "" || ViewState["dt"] == null)
            {
                objds = Objstatus.GetWardDoctorwise(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]),
                                 common.myInt(0), wardId, doctorId, common.myInt(Session["FacilityId"]));

                if (objds.Tables.Count > 0)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["dt"] = objds.Tables[0];

                    }
                }
            }
            int EntrySite = 0;
            if (common.myInt(ddlEntrySitesActual.SelectedValue) > 0)
            {
                EntrySite = common.myInt(ddlEntrySitesActual.SelectedValue);
            }

            DataTable dtExist = ViewState["dt"] as DataTable;

            DataTable dt1 = new DataTable();

            if (dtExist != null)
            {
                if (ddlEncounterStatus.SelectedIndex > 0)
                {
                    dtExist.DefaultView.RowFilter = "EncounterStatusId=" + common.myInt(ddlEncounterStatus.SelectedValue);
                }

                if (EntrySite != 0)
                {
                    dtExist.DefaultView.RowFilter = "EntrySite=" + common.myInt(EntrySite);
                    dt1 = dtExist.DefaultView.ToTable();
                }

                if (wardId != 0)
                {
                    dtExist.DefaultView.RowFilter = "wardNo=" + common.myInt(wardId);
                    dt1 = dtExist.DefaultView.ToTable();
                }
                if (BedStatusId != "")
                {
                    if (wardId != 0)
                    {
                        dt1.DefaultView.RowFilter = "BedStatus='" + BedStatusId + "'";
                        dtExist = dt1.DefaultView.ToTable().Copy();
                    }
                    else
                    {
                        dtExist.DefaultView.RowFilter = "BedStatus='" + BedStatusId + "'";
                    }
                }
                if (common.myInt(hdnregID.Value) != 0 && ddlPatient.SelectedValue != "")
                {
                    dtExist.DefaultView.RowFilter = "RegistrationId=" + common.myInt(hdnregID.Value);
                }
            }
            else
            {
                objds = Objstatus.GetWardDoctorwise(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]),
                                  common.myInt(0), wardId, doctorId, common.myInt(Session["FacilityId"]));

                if (objds.Tables.Count > 0)
                {
                    if (objds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["dt"] = objds.Tables[0];
                    }
                }
            }
            if (dtExist != null)
            {
                if (dtExist.Rows.Count > 0)
                {
                    objds = new DataSet();
                    objds.Tables.Add(dtExist.DefaultView.ToTable());
                }
            }

            //   Added on 11-09-2014 Start Naushad
            if (objds.Tables.Count > 0)
            {
                if (objds.Tables[0].Rows.Count > 0)
                {
                    //if (common.myInt(objds.Tables[0].Rows[0]["BloodBankRequests"]) > 0)
                    //{
                    lblNoOfRequest.ForeColor = System.Drawing.Color.DarkRed;
                    lblNoOfRequest.Text = "(" + common.myStr(objds.Tables[0].Rows[0]["BloodBankRequests"]) + ")";
                    lnkbloodRe.Visible = true;
                    lblnondrugordercount.ForeColor = System.Drawing.Color.DarkRed;
                    lblunperformedSer.ForeColor = System.Drawing.Color.DarkRed;
                    lbldrugordercount.ForeColor = System.Drawing.Color.DarkRed;
                    lblNoOfRequest.Visible = true;
                    lblunperformedSer.Visible = true;
                    lbldrugordercount.Visible = true;
                    lblnondrugordercount.Visible = true;
                    lnkunperformedSer.Visible = true;
                    lnkdrugordercount.Visible = true;
                    lnknondrugordercount.Visible = true;
                    lblunperformedSer.Text = "(" + common.myStr(objds.Tables[0].Rows[0]["CountUnperformedService"]) + ")";
                    lbldrugordercount.Text = "(" + common.myStr(objds.Tables[0].Rows[0]["CountDrugOrder"]) + ")";
                    lblnondrugordercount.Text = "(" + common.myStr(objds.Tables[0].Rows[0]["CountNonDrugOrder"]) + ")";
                    if (common.myStr(objds.Tables[0].Rows[0]["ICUClearanceRequiredForBedTransfer"]) == "Y")
                    {
                        lnkBedClearance.Visible = true;
                        lblBedClearanceCount.Visible = true;
                        lblBedClearanceCount.Text = "(" + common.myStr(objds.Tables[0].Rows[0]["CountICUClearanceRequest"]) + ")";
                    }
                    else
                    {
                        lnkBedClearance.Visible = false;
                        lblBedClearanceCount.Visible = false;
                    }
                    //}
                    //else
                    //{
                    //lnkbloodRe.Visible = false;
                    //lblNoOfRequest.Visible = false;
                    //lblunperformedSer.Visible = false;
                    //lbldrugordercount.Visible = false;
                    //lblnondrugordercount.Visible = false;
                    //lnkunperformedSer.Visible = false;
                    //lnkdrugordercount.Visible = false;
                    //lnknondrugordercount.Visible = false;
                    //}
                }
            }
            else
            {
                lnkbloodRe.Visible = false;
                lblNoOfRequest.Visible = false;
            }
            //  Aded on 11-09-2014 End  Naushad

            if (objds.Tables.Count > 0)
            {
                foreach (DataRow dr in objds.Tables[0].Rows)
                {
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = Convert.ToString(i);
                    dt.Columns.Add(dc);
                    i++;
                    if (i == noOfColumn)
                    {
                        break;
                    }
                }

                int l = 0;
                for (int k = 0; objds.Tables[0].Rows.Count != k; k++)
                {
                    for (int j = 0; j != noOfColumn; j++)
                    {
                        if (objds.Tables[0].Rows.Count == k)
                        {
                            break;
                        }
                        else
                        {
                            if (k == 0)
                            {
                                dr1 = dt.NewRow();
                                wardname = common.myStr(objds.Tables[0].Rows[k]["WardName"]);
                                dr1[0] = wardname;
                                dt.Rows.Add(dr1);
                                l = 0;
                                dr1 = dt.NewRow();
                                dr1[l] = common.myStr(objds.Tables[0].Rows[k]["BedNo"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["BedStatus"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["RegistrationNo"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterNo"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["wardNo"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["BedCatg"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["CompanyType"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["Patient_Name"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterStatusColor"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterStatusID"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["WardName"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["BedCategoryName"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["CriticalLabResult"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["UnPerformedServices"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["OTBillClearancePending"]) +//Added on 02-09-2014 Naushad Ali hdnIsCritical1 
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["AllergiesAlert"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["MedicalAlert"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["MLC"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["IsReferralCase"]);

                                l++;
                                if (objds.Tables[0].Rows.Count == k + 1)
                                {
                                    dt.Rows.Add(dr1);
                                }
                                break;
                            }
                            else if (wardname == common.myStr(objds.Tables[0].Rows[k]["WardName"]))
                            {
                                if (l == noOfColumn)
                                {
                                    k--;
                                    dt.Rows.Add(dr1);
                                    dr1 = dt.NewRow();
                                    l = 0;
                                    break;
                                }


                                dr1[l] = Convert.ToString(objds.Tables[0].Rows[k]["BedNo"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["BedStatus"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["RegistrationNo"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterNo"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["wardNo"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["BedCatg"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["CompanyType"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["Patient_Name"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterStatusColor"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterStatusID"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["WardName"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["BedCategoryName"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["CriticalLabResult"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["UnPerformedServices"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["OTBillClearancePending"]) + //Added on 02-09-2014 Naushad Ali hdnIsCritical1 
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["AllergiesAlert"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["MedicalAlert"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["MLC"]) +
                                        "@" + common.myStr(objds.Tables[0].Rows[k]["IsReferralCase"]);

                                if (objds.Tables[0].Rows.Count == k + 1)
                                {
                                    dt.Rows.Add(dr1);
                                }
                                l++;
                                k++;

                            }
                            else
                            {
                                dt.Rows.Add(dr1);
                                dr1 = dt.NewRow();
                                wardname = Convert.ToString(objds.Tables[0].Rows[k]["WardName"]);
                                dr1[0] = wardname;
                                dt.Rows.Add(dr1);
                                l = 0;
                                dr1 = dt.NewRow();
                                dr1[l] = common.myStr(objds.Tables[0].Rows[k]["BedNo"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["BedStatus"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["RegistrationNo"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterNo"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["wardNo"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["BedCatg"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["CompanyType"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["Patient_Name"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterStatusColor"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["EncounterStatusID"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["WardName"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["BedCategoryName"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["CriticalLabResult"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["UnPerformedServices"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["OTBillClearancePending"]) + //Added on 02-09-2014 Naushad Ali hdnIsCritical1 
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["AllergiesAlert"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["MedicalAlert"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["MLC"]) +
                                    "@" + common.myStr(objds.Tables[0].Rows[k]["IsReferralCase"]);

                                l++;
                                if (objds.Tables[0].Rows.Count == k + 1)
                                {
                                    dt.Rows.Add(dr1);
                                }
                                break;
                            }
                        }
                        if (l == noOfColumn)
                        {
                            if (objds.Tables[0].Rows.Count != k)
                            {
                                k--;
                                dt.Rows.Add(dr1);
                                dr1 = dt.NewRow();
                                l = 0;
                                break;
                            }
                        }
                    }

                    if (objds.Tables[0].Rows.Count == k)
                    {
                        break;
                    }
                }
                noOfColumn = dt.Columns.Count;
                ViewState["noOfColumn"] = noOfColumn;
                grvBedStatus.DataSource = dt;
                grvBedStatus.DataBind();

            }
        }
        catch (Exception Ex)
        {
            // lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            Alert.ShowAjaxMsg("Error: " + Ex.Message, Page);
            objException.HandleException(Ex);
        }
    }



    //Added on 30-04-2014 Start Naushad

    protected void lnkbloodRe_OnClick(object sender, EventArgs e)
    {

        RadWindow1.NavigateUrl = "/BloodBank/SetupMaster/ComponentRequisitionList.aspx?MP=NO&Regid=" + common.myInt(0)
                                  + "&RegNo=" + common.myStr("")
                                   + "&EncId=" + common.myInt(0)
                                   + "&EncNo=" + common.myStr("")
                                   + "&AckStatus=Ack"
                                   + "&Ptype=I";
        RadWindow1.Height = 550;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        // RadWindow1.OnClientClose = "OnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    }

    //Added on 30-04-2014 End  Naushad 

    protected void grvBedStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int noOfColumn = common.myInt(ViewState["noOfColumn"]);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label label1 = (Label)e.Row.FindControl("label1");
                Label label2 = (Label)e.Row.FindControl("label2");
                Label label3 = (Label)e.Row.FindControl("label3");
                Label label4 = (Label)e.Row.FindControl("label4");
                Label label5 = (Label)e.Row.FindControl("label5");
                Label label6 = (Label)e.Row.FindControl("label6");
                Label label7 = (Label)e.Row.FindControl("label7");
                Label label8 = (Label)e.Row.FindControl("label8");
                Label label9 = (Label)e.Row.FindControl("label9");
                Label label10 = (Label)e.Row.FindControl("label10");

                Label lblPatientName = (Label)e.Row.FindControl("lblPatientName");
                Label lblPatientName2 = (Label)e.Row.FindControl("lblPatientName2");
                Label lblPatientName3 = (Label)e.Row.FindControl("lblPatientName3");
                Label lblPatientName4 = (Label)e.Row.FindControl("lblPatientName4");
                Label lblPatientName5 = (Label)e.Row.FindControl("lblPatientName5");
                Label lblPatientName6 = (Label)e.Row.FindControl("lblPatientName6");
                Label lblPatientName7 = (Label)e.Row.FindControl("lblPatientName7");
                Label lblPatientName8 = (Label)e.Row.FindControl("lblPatientName8");
                Label lblPatientName9 = (Label)e.Row.FindControl("lblPatientName9");
                Label lblPatientName10 = (Label)e.Row.FindControl("lblPatientName10");

                Label lblCompanytype = (Label)e.Row.FindControl("lblCompanytype");
                Label lblCompanytype2 = (Label)e.Row.FindControl("lblCompanytype2");
                Label lblCompanytype3 = (Label)e.Row.FindControl("lblCompanytype3");
                Label lblCompanytype4 = (Label)e.Row.FindControl("lblCompanytype4");
                Label lblCompanytype5 = (Label)e.Row.FindControl("lblCompanytype5");
                Label lblCompanytype6 = (Label)e.Row.FindControl("lblCompanytype6");
                Label lblCompanytype7 = (Label)e.Row.FindControl("lblCompanytype7");
                Label lblCompanytype8 = (Label)e.Row.FindControl("lblCompanytype8");
                Label lblCompanytype9 = (Label)e.Row.FindControl("lblCompanytype9");
                Label lblCompanytype10 = (Label)e.Row.FindControl("lblCompanytype10");


                Label lblMLC1 = (Label)e.Row.FindControl("lblMLC1");
                Label lblMLC2 = (Label)e.Row.FindControl("lblMLC2");
                Label lblMLC3 = (Label)e.Row.FindControl("lblMLC3");
                Label lblMLC4 = (Label)e.Row.FindControl("lblMLC4");
                Label lblMLC5 = (Label)e.Row.FindControl("lblMLC5");
                Label lblMLC6 = (Label)e.Row.FindControl("lblMLC6");
                Label lblMLC7 = (Label)e.Row.FindControl("lblMLC7");
                Label lblMLC8 = (Label)e.Row.FindControl("lblMLC8");
                Label lblMLC9 = (Label)e.Row.FindControl("lblMLC9");
                Label lblMLC10 = (Label)e.Row.FindControl("lblMLC10");

                lblMLC1.Visible = false;
                lblMLC2.Visible = false;
                lblMLC3.Visible = false;
                lblMLC4.Visible = false;
                lblMLC5.Visible = false;
                lblMLC6.Visible = false;
                lblMLC7.Visible = false;
                lblMLC8.Visible = false;
                lblMLC9.Visible = false;
                lblMLC10.Visible = false;

                Label lblEnc1 = (Label)e.Row.FindControl("lblEnc1");
                Label lblEnc2 = (Label)e.Row.FindControl("lblEnc2");
                Label lblEnc3 = (Label)e.Row.FindControl("lblEnc3");
                Label lblEnc4 = (Label)e.Row.FindControl("lblEnc4");
                Label lblEnc5 = (Label)e.Row.FindControl("lblEnc5");
                Label lblEnc6 = (Label)e.Row.FindControl("lblEnc6");
                Label lblEnc7 = (Label)e.Row.FindControl("lblEnc7");
                Label lblEnc8 = (Label)e.Row.FindControl("lblEnc8");
                Label lblEnc9 = (Label)e.Row.FindControl("lblEnc9");
                Label lblEnc10 = (Label)e.Row.FindControl("lblEnc10");



                ImageButton imgbutton1 = (ImageButton)e.Row.FindControl("Imagebutton1");
                ImageButton imgbutton2 = (ImageButton)e.Row.FindControl("Imagebutton2");
                ImageButton imgbutton3 = (ImageButton)e.Row.FindControl("Imagebutton3");
                ImageButton imgbutton4 = (ImageButton)e.Row.FindControl("Imagebutton4");
                ImageButton imgbutton5 = (ImageButton)e.Row.FindControl("Imagebutton5");
                ImageButton imgbutton6 = (ImageButton)e.Row.FindControl("Imagebutton6");
                ImageButton imgbutton7 = (ImageButton)e.Row.FindControl("Imagebutton7");
                ImageButton imgbutton8 = (ImageButton)e.Row.FindControl("Imagebutton8");
                ImageButton imgbutton9 = (ImageButton)e.Row.FindControl("Imagebutton9");
                ImageButton imgbutton10 = (ImageButton)e.Row.FindControl("Imagebutton10");


                Image imgbill = (Image)e.Row.FindControl("imgbill");
                Image imgbill2 = (Image)e.Row.FindControl("imgbill2");
                Image imgbill3 = (Image)e.Row.FindControl("imgbill3");
                Image imgbill4 = (Image)e.Row.FindControl("imgbill4");
                Image imgbill5 = (Image)e.Row.FindControl("imgbill5");
                Image imgbill6 = (Image)e.Row.FindControl("imgbill6");
                Image imgbill7 = (Image)e.Row.FindControl("imgbill7");
                Image imgbill8 = (Image)e.Row.FindControl("imgbill8");
                Image imgbill9 = (Image)e.Row.FindControl("imgbill9");
                Image imgbill10 = (Image)e.Row.FindControl("imgbill10");

                imgbill.Visible = false;
                imgbill2.Visible = false;
                imgbill3.Visible = false;
                imgbill4.Visible = false;
                imgbill5.Visible = false;
                imgbill6.Visible = false;
                imgbill7.Visible = false;
                imgbill8.Visible = false;
                imgbill9.Visible = false;
                imgbill10.Visible = false;

                Image imgAllergyAlert1 = (Image)e.Row.FindControl("imgAllergyAlert1");
                Image imgAllergyAlert2 = (Image)e.Row.FindControl("imgAllergyAlert2");
                Image imgAllergyAlert3 = (Image)e.Row.FindControl("imgAllergyAlert3");
                Image imgAllergyAlert4 = (Image)e.Row.FindControl("imgAllergyAlert4");
                Image imgAllergyAlert5 = (Image)e.Row.FindControl("imgAllergyAlert5");
                Image imgAllergyAlert6 = (Image)e.Row.FindControl("imgAllergyAlert6");
                Image imgAllergyAlert7 = (Image)e.Row.FindControl("imgAllergyAlert7");
                Image imgAllergyAlert8 = (Image)e.Row.FindControl("imgAllergyAlert8");
                Image imgAllergyAlert9 = (Image)e.Row.FindControl("imgAllergyAlert9");
                Image imgAllergyAlert10 = (Image)e.Row.FindControl("imgAllergyAlert10");

                imgAllergyAlert1.Visible = false;
                imgAllergyAlert2.Visible = false;
                imgAllergyAlert3.Visible = false;
                imgAllergyAlert4.Visible = false;
                imgAllergyAlert5.Visible = false;
                imgAllergyAlert6.Visible = false;
                imgAllergyAlert7.Visible = false;
                imgAllergyAlert8.Visible = false;
                imgAllergyAlert9.Visible = false;
                imgAllergyAlert10.Visible = false;

                Image imgMedicalAlert1 = (Image)e.Row.FindControl("imgMedicalAlert1");
                Image imgMedicalAlert2 = (Image)e.Row.FindControl("imgMedicalAlert2");
                Image imgMedicalAlert3 = (Image)e.Row.FindControl("imgMedicalAlert3");
                Image imgMedicalAlert4 = (Image)e.Row.FindControl("imgMedicalAlert4");
                Image imgMedicalAlert5 = (Image)e.Row.FindControl("imgMedicalAlert5");
                Image imgMedicalAlert6 = (Image)e.Row.FindControl("imgMedicalAlert6");
                Image imgMedicalAlert7 = (Image)e.Row.FindControl("imgMedicalAlert7");
                Image imgMedicalAlert8 = (Image)e.Row.FindControl("imgMedicalAlert8");
                Image imgMedicalAlert9 = (Image)e.Row.FindControl("imgMedicalAlert9");
                Image imgMedicalAlert10 = (Image)e.Row.FindControl("imgMedicalAlert10");

                imgMedicalAlert1.Visible = false;
                imgMedicalAlert2.Visible = false;
                imgMedicalAlert3.Visible = false;
                imgMedicalAlert4.Visible = false;
                imgMedicalAlert5.Visible = false;
                imgMedicalAlert6.Visible = false;
                imgMedicalAlert7.Visible = false;
                imgMedicalAlert8.Visible = false;
                imgMedicalAlert9.Visible = false;
                imgMedicalAlert10.Visible = false;

                Image imgReffHistory1 = (Image)e.Row.FindControl("imgReffHistory1");
                Image imgReffHistory2 = (Image)e.Row.FindControl("imgReffHistory2");
                Image imgReffHistory3 = (Image)e.Row.FindControl("imgReffHistory3");
                Image imgReffHistory4 = (Image)e.Row.FindControl("imgReffHistory4");
                Image imgReffHistory5 = (Image)e.Row.FindControl("imgReffHistory5");
                Image imgReffHistory6 = (Image)e.Row.FindControl("imgReffHistory6");
                Image imgReffHistory7 = (Image)e.Row.FindControl("imgReffHistory7");
                Image imgReffHistory8 = (Image)e.Row.FindControl("imgReffHistory8");
                Image imgReffHistory9 = (Image)e.Row.FindControl("imgReffHistory9");
                Image imgReffHistory10 = (Image)e.Row.FindControl("imgReffHistory10");

                imgReffHistory1.Visible = false;
                imgReffHistory2.Visible = false;
                imgReffHistory3.Visible = false;
                imgReffHistory4.Visible = false;
                imgReffHistory5.Visible = false;
                imgReffHistory6.Visible = false;
                imgReffHistory7.Visible = false;
                imgReffHistory8.Visible = false;
                imgReffHistory9.Visible = false;
                imgReffHistory10.Visible = false;

                //hdnIsCritical1
                //Added on 02-09-2014 Start Naushad Ali

                HiddenField hdnregno1 = (HiddenField)e.Row.FindControl("hdnregno1");
                HiddenField hdnregno2 = (HiddenField)e.Row.FindControl("hdnregno2");
                HiddenField hdnregno3 = (HiddenField)e.Row.FindControl("hdnregno3");
                HiddenField hdnregno4 = (HiddenField)e.Row.FindControl("hdnregno4");
                HiddenField hdnregno5 = (HiddenField)e.Row.FindControl("hdnregno5");
                HiddenField hdnregno6 = (HiddenField)e.Row.FindControl("hdnregno6");
                HiddenField hdnregno7 = (HiddenField)e.Row.FindControl("hdnregno7");
                HiddenField hdnregno8 = (HiddenField)e.Row.FindControl("hdnregno8");
                HiddenField hdnregno9 = (HiddenField)e.Row.FindControl("hdnregno9");
                HiddenField hdnregno10 = (HiddenField)e.Row.FindControl("hdnregno10");

                HiddenField hdnencounterno1 = (HiddenField)e.Row.FindControl("hdnencounterno1");
                HiddenField hdnencounterno2 = (HiddenField)e.Row.FindControl("hdnencounterno2");
                HiddenField hdnencounterno3 = (HiddenField)e.Row.FindControl("hdnencounterno3");
                HiddenField hdnencounterno4 = (HiddenField)e.Row.FindControl("hdnencounterno4");
                HiddenField hdnencounterno5 = (HiddenField)e.Row.FindControl("hdnencounterno5");
                HiddenField hdnencounterno6 = (HiddenField)e.Row.FindControl("hdnencounterno6");
                HiddenField hdnencounterno7 = (HiddenField)e.Row.FindControl("hdnencounterno7");
                HiddenField hdnencounterno8 = (HiddenField)e.Row.FindControl("hdnencounterno8");
                HiddenField hdnencounterno9 = (HiddenField)e.Row.FindControl("hdnencounterno9");
                HiddenField hdnencounterno10 = (HiddenField)e.Row.FindControl("hdnencounterno10");
                //OTBillClearancePending

                if (noOfColumn == 1)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');

                    if (str.Length > 1)
                    {
                        label1.Text = str[0];
                        hdnregno1.Value = str[2];
                        hdnencounterno1.Value = str[3];

                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            lblencno.Text = str[3];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";

                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];

                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");

                                #region Critical result and Unperformed Services

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");

                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }
                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");

                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }


                                #endregion

                                #region forOTBillCleareancecou1image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Attributes.Add("style", "display:none");
                                    imgbill.Visible = false;
                                    //imgbill.Width = 1;
                                    //imgbill.Height = 1;
                                    //imgbill.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion

                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }

                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;
                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    imgbutton2.Visible = false;
                    HtmlControl div2 = (HtmlControl)e.Row.FindControl("div2");
                    div2.Visible = false;
                    imgbutton3.Visible = false;
                    HtmlControl div3 = (HtmlControl)e.Row.FindControl("div3");
                    div3.Visible = false;
                    imgbutton4.Visible = false;
                    HtmlControl div4 = (HtmlControl)e.Row.FindControl("div4");
                    div4.Visible = false;
                    imgbutton5.Visible = false;
                    HtmlControl div5 = (HtmlControl)e.Row.FindControl("div5");
                    div5.Visible = false;
                    imgbutton6.Visible = false;
                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div6");
                    div6.Visible = false;

                    imgbutton7.Visible = false;
                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div7");
                    div7.Visible = false;

                    imgbutton8.Visible = false;
                    HtmlControl div8 = (HtmlControl)e.Row.FindControl("div8");
                    div8.Visible = false;

                    imgbutton9.Visible = false;
                    HtmlControl div9 = (HtmlControl)e.Row.FindControl("div9");
                    div9.Visible = false;

                    imgbutton10.Visible = false;
                    HtmlControl div10 = (HtmlControl)e.Row.FindControl("div10");
                    div10.Visible = false;

                    //Added on 3-09-2014 Start 

                    //imgbill2.Visible = false;
                    //imgbill3.Visible = false;
                    //imgbill4.Visible = false;
                    //imgbill5.Visible = false;
                    //imgbill6.Visible = false;
                    //imgbill7.Visible = false;
                    //imgbill8.Visible = false;
                    //imgbill9.Visible = false;
                    //imgbill10.Visible = false;

                    //Added on 0-09-2014 END

                }
                if (noOfColumn == 2)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    label1.Text = str[0];
                    label2.Text = str1[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];

                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";

                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                                #region Critical result and Unperformed Services

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");

                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }
                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");

                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }
                                #endregion


                                #region forOTBillCleareance1coul2image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Attributes.Add("style", "display:none");
                                    imgbill.Visible = false;


                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion

                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";

                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);

                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }


                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");
                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }

                                #region forOTBillCleareancecol2imag2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill2.ImageUrl = "~/Images/PendingBill";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill2.ImageUrl = "~/Images/PendingBill";
                                    imgbill2.Attributes.Add("style", "display:none");
                                    imgbill2.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion

                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }

                    imgbutton3.Visible = false;
                    HtmlControl div3 = (HtmlControl)e.Row.FindControl("div3");
                    div3.Visible = false;
                    imgbutton4.Visible = false;
                    HtmlControl div4 = (HtmlControl)e.Row.FindControl("div4");
                    div4.Visible = false;
                    imgbutton5.Visible = false;
                    HtmlControl div5 = (HtmlControl)e.Row.FindControl("div5");
                    div5.Visible = false;
                    imgbutton6.Visible = false;
                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div6");
                    div6.Visible = false;

                    imgbutton7.Visible = false;
                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div7");
                    div7.Visible = false;

                    imgbutton8.Visible = false;
                    HtmlControl div8 = (HtmlControl)e.Row.FindControl("div8");
                    div8.Visible = false;

                    imgbutton9.Visible = false;
                    HtmlControl div9 = (HtmlControl)e.Row.FindControl("div9");
                    div9.Visible = false;

                    imgbutton10.Visible = false;
                    HtmlControl div10 = (HtmlControl)e.Row.FindControl("div10");
                    div10.Visible = false;

                }
                if (noOfColumn == 3)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {

                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";

                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");
                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }


                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");
                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }
                                #region forOTBillCleareanceCou3Image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Attributes.Add("style", "display:none");
                                    imgbill.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion


                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);

                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";

                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);

                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }


                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");
                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }

                                #region forOTBillCleareanceCou3Image2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill2.Attributes.Add("style", "display:none");
                                    imgbill2.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion

                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }
                    if (str2.Length > 1)
                    {
                        lblEnc3.Text = str2[3];
                        lblPatientName3.Text = str2[7];
                        lblCompanytype3.Text = str2[6];
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";

                            }
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);

                                HiddenField hdnIsCritical3 = (HiddenField)e.Row.FindControl("hdnIsCritical3");
                                Label lblCriticalIndication3 = (Label)e.Row.FindControl("lblCriticalIndication3");
                                if (str2[12].Contains("1"))
                                {
                                    lblCriticalIndication3.Visible = true;
                                    lblCriticalIndication3.ToolTip = "Test results with panic values.";
                                    hdnIsCritical3.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication3.Visible = false;
                                    lblCriticalIndication3.ToolTip = string.Empty;
                                    hdnIsCritical3.Value = "0";
                                }

                                HiddenField hdnUnperformedServices3 = (HiddenField)e.Row.FindControl("hdnUnperformedServices3");
                                Label lblUnperformedServices3 = (Label)e.Row.FindControl("lblUnperformedServices3");
                                if (str2[13].Contains("1"))
                                {
                                    lblUnperformedServices3.Visible = true;
                                    lblUnperformedServices3.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices3.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices3.Visible = false;
                                    lblUnperformedServices3.ToolTip = string.Empty;
                                    hdnUnperformedServices3.Value = "0";
                                }

                                #region forOTBillCleareanceCou3Image3
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str2[14].Contains("1"))
                                {
                                    imgbill3.Visible = true;
                                    imgbill3.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill3.Attributes.Add("style", "display:none");

                                    imgbill3.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert3.Visible = common.myBool(str2[15]);
                                imgMedicalAlert3.Visible = common.myBool(str2[16]);
                                #endregion

                                #region MLC Patient
                                if (str2[17].Contains("True"))
                                {
                                    lblMLC3.Visible = true;
                                    lblMLC3.Text = "MLC";
                                    lblMLC3.Font.Bold = true;
                                    lblMLC3.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory3.Visible = common.myBool(str2[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "','" + str2[9] + "','" + hdnIsCritical3.ClientID + "','" + hdnUnperformedServices3.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;

                    }

                    //imgbutton3.Visible = false;
                    //HtmlControl div3 = (HtmlControl)e.Row.FindControl("div3");
                    //div3.Visible = false;
                    imgbutton4.Visible = false;
                    HtmlControl div4 = (HtmlControl)e.Row.FindControl("div4");
                    div4.Visible = false;
                    imgbutton5.Visible = false;
                    HtmlControl div5 = (HtmlControl)e.Row.FindControl("div5");
                    div5.Visible = false;
                    imgbutton6.Visible = false;
                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div6");
                    div6.Visible = false;

                    imgbutton7.Visible = false;
                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div7");
                    div7.Visible = false;

                    imgbutton8.Visible = false;
                    HtmlControl div8 = (HtmlControl)e.Row.FindControl("div8");
                    div8.Visible = false;

                    imgbutton9.Visible = false;
                    HtmlControl div9 = (HtmlControl)e.Row.FindControl("div9");
                    div9.Visible = false;

                    imgbutton10.Visible = false;
                    HtmlControl div10 = (HtmlControl)e.Row.FindControl("div10");
                    div10.Visible = false;

                }
                if (noOfColumn == 4)
                {

                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);


                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");
                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }

                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");

                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }

                                #region forOTBillCleareanceCou4Image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Attributes.Add("style", "display:none");
                                    imgbill.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion

                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);

                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }

                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");

                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }
                                #region forOTBillCleareanceCou4Image2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill2.Attributes.Add("style", "display:none");
                                    imgbill2.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion

                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }
                    if (str2.Length > 1)
                    {
                        lblEnc3.Text = str2[3];
                        lblEnc3.Text = str2[3];
                        lblPatientName3.Text = str2[7];
                        lblCompanytype3.Text = str2[6];
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                            }
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);

                                HiddenField hdnIsCritical3 = (HiddenField)e.Row.FindControl("hdnIsCritical3");
                                Label lblCriticalIndication3 = (Label)e.Row.FindControl("lblCriticalIndication3");
                                if (str2[12].Contains("1"))
                                {
                                    lblCriticalIndication3.Visible = true;
                                    lblCriticalIndication3.ToolTip = "Test results with panic values.";
                                    hdnIsCritical3.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication3.Visible = false;
                                    lblCriticalIndication3.ToolTip = string.Empty;
                                    hdnIsCritical3.Value = "0";
                                }

                                HiddenField hdnUnperformedServices3 = (HiddenField)e.Row.FindControl("hdnUnperformedServices3");
                                Label lblUnperformedServices3 = (Label)e.Row.FindControl("lblUnperformedServices3");
                                if (str2[13].Contains("1"))
                                {
                                    lblUnperformedServices3.Visible = true;
                                    lblUnperformedServices3.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices3.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices3.Visible = false;
                                    lblUnperformedServices3.ToolTip = string.Empty;
                                    hdnUnperformedServices3.Value = "0";
                                }

                                #region forOTBillCleareanceCou4Image3
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str2[14].Contains("1"))
                                {
                                    imgbill3.Visible = true;
                                    imgbill3.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill3.Attributes.Add("style", "display:none");
                                    imgbill3.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert3.Visible = common.myBool(str2[15]);
                                imgMedicalAlert3.Visible = common.myBool(str2[16]);
                                #endregion

                                #region MLC Patient
                                if (str2[17].Contains("True"))
                                {
                                    lblMLC3.Visible = true;
                                    lblMLC3.Text = "MLC";
                                    lblMLC3.Font.Bold = true;
                                    lblMLC3.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory3.Visible = common.myBool(str2[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "','" + str2[9] + "','" + hdnIsCritical3.ClientID + "','" + hdnUnperformedServices3.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;

                    }
                    if (str3.Length > 1)
                    {
                        lblEnc4.Text = str3[3];
                        lblPatientName4.Text = str3[7];
                        lblCompanytype4.Text = str3[6];
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);

                                HiddenField hdnIsCritical4 = (HiddenField)e.Row.FindControl("hdnIsCritical4");
                                Label lblCriticalIndication4 = (Label)e.Row.FindControl("lblCriticalIndication4");
                                if (str3[12].Contains("1"))
                                {
                                    lblCriticalIndication4.Visible = true;
                                    lblCriticalIndication4.ToolTip = "Test results with panic values.";
                                    hdnIsCritical4.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication4.Visible = false;
                                    lblCriticalIndication4.ToolTip = string.Empty;
                                    hdnIsCritical4.Value = "0";
                                }


                                HiddenField hdnUnperformedServices4 = (HiddenField)e.Row.FindControl("hdnUnperformedServices4");
                                Label lblUnperformedServices4 = (Label)e.Row.FindControl("lblUnperformedServices4");
                                if (str3[13].Contains("1"))
                                {
                                    lblUnperformedServices4.Visible = true;
                                    lblUnperformedServices4.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices4.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices4.Visible = false;
                                    lblUnperformedServices4.ToolTip = string.Empty;
                                    hdnUnperformedServices4.Value = "0";
                                }

                                #region forOTBillCleareanceCou4Image4
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str3[14].Contains("1"))
                                {
                                    imgbill4.Visible = true;
                                    imgbill4.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill4.Attributes.Add("style", "display:none");
                                    imgbill4.Visible = false;
                                    imgbill4.Width = 1;
                                    imgbill4.Height = 1;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert4.Visible = common.myBool(str3[15]);
                                imgMedicalAlert4.Visible = common.myBool(str3[16]);
                                #endregion

                                #region MLC Patient
                                if (str3[17].Contains("True"))
                                {
                                    lblMLC4.Visible = true;
                                    lblMLC4.Text = "MLC";
                                    lblMLC4.Font.Bold = true;
                                    lblMLC4.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory4.Visible = common.myBool(str3[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "','" + str3[9] + "','" + hdnIsCritical4.ClientID + "','" + hdnUnperformedServices4.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;

                    }

                    HtmlControl div4 = (HtmlControl)e.Row.FindControl("div5");
                    div4.Visible = false;

                    HtmlControl div5 = (HtmlControl)e.Row.FindControl("div6");
                    div5.Visible = false;

                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div7");
                    div6.Visible = false;

                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div8");
                    div7.Visible = false;

                    HtmlControl div8 = (HtmlControl)e.Row.FindControl("div9");
                    div8.Visible = false;

                    HtmlControl div9 = (HtmlControl)e.Row.FindControl("div10");
                    div9.Visible = false;
                }
                if (noOfColumn == 5)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");
                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }


                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");
                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }

                                #region forOTBillCleareanceCou5Image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.Visible = false;
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Attributes.Add("style", "display:none");
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion

                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //  imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);

                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }

                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");
                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }

                                #region forOTBillCleareancecou5Image2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill2.Visible = false;
                                    imgbill2.Attributes.Add("style", "display:none");
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion

                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");

                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }
                    if (str2.Length > 1)
                    {
                        lblEnc3.Text = str2[3];
                        lblPatientName3.Text = str2[7];
                        lblCompanytype3.Text = str2[6];
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);


                                HiddenField hdnIsCritical3 = (HiddenField)e.Row.FindControl("hdnIsCritical3");
                                Label lblCriticalIndication3 = (Label)e.Row.FindControl("lblCriticalIndication3");
                                if (str2[12].Contains("1"))
                                {
                                    lblCriticalIndication3.Visible = true;
                                    lblCriticalIndication3.ToolTip = "Test results with panic values.";
                                    hdnIsCritical3.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication3.Visible = false;
                                    lblCriticalIndication3.ToolTip = string.Empty;
                                    hdnIsCritical3.Value = "0";
                                }

                                HiddenField hdnUnperformedServices3 = (HiddenField)e.Row.FindControl("hdnUnperformedServices3");
                                Label lblUnperformedServices3 = (Label)e.Row.FindControl("lblUnperformedServices3");
                                if (str2[13].Contains("1"))
                                {
                                    lblUnperformedServices3.Visible = true;
                                    lblUnperformedServices3.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices3.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices3.Visible = false;
                                    lblUnperformedServices3.ToolTip = string.Empty;
                                    hdnUnperformedServices3.Value = "0";
                                }

                                #region forOTBillCleareancecou5Image3
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str2[14].Contains("1"))
                                {
                                    imgbill3.Visible = true;
                                    imgbill3.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill3.Visible = false;
                                    imgbill3.Attributes.Add("style", "display:none");
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert3.Visible = common.myBool(str2[15]);
                                imgMedicalAlert3.Visible = common.myBool(str2[16]);
                                #endregion


                                #region MLC Patient
                                if (str2[17].Contains("True"))
                                {
                                    lblMLC3.Visible = true;
                                    lblMLC3.Text = "MLC";
                                    lblMLC3.Font.Bold = true;
                                    lblMLC3.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory3.Visible = common.myBool(str2[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "','" + str2[9] + "','" + hdnIsCritical3.ClientID + "','" + hdnUnperformedServices3.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;

                    }
                    if (str3.Length > 1)
                    {
                        lblEnc4.Text = str3[3];
                        lblPatientName4.Text = str3[7];
                        lblCompanytype4.Text = str3[6];
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);

                                HiddenField hdnIsCritical4 = (HiddenField)e.Row.FindControl("hdnIsCritical4");
                                Label lblCriticalIndication4 = (Label)e.Row.FindControl("lblCriticalIndication4");
                                if (str3[12].Contains("1"))
                                {
                                    lblCriticalIndication4.Visible = true;
                                    lblCriticalIndication4.ToolTip = "Test results with panic values.";
                                    hdnIsCritical4.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication4.Visible = false;
                                    lblCriticalIndication4.ToolTip = string.Empty;
                                    hdnIsCritical4.Value = "0";
                                }

                                HiddenField hdnUnperformedServices4 = (HiddenField)e.Row.FindControl("hdnUnperformedServices4");
                                Label lblUnperformedServices4 = (Label)e.Row.FindControl("lblUnperformedServices4");
                                if (str3[13].Contains("1"))
                                {
                                    lblUnperformedServices4.Visible = true;
                                    lblUnperformedServices4.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices4.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices4.Visible = false;
                                    lblUnperformedServices4.ToolTip = string.Empty;
                                    hdnUnperformedServices4.Value = "0";
                                }


                                #region forOTBillCleareancecou5Image4
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str3[14].Contains("1"))
                                {
                                    imgbill4.Visible = true;
                                    imgbill4.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill4.Visible = false;
                                    imgbill4.Attributes.Add("style", "display:none");
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert4.Visible = common.myBool(str3[15]);
                                imgMedicalAlert4.Visible = common.myBool(str3[16]);
                                #endregion


                                #region MLC Patient
                                if (str3[17].Contains("True"))
                                {
                                    lblMLC4.Visible = true;
                                    lblMLC4.Text = "MLC";
                                    lblMLC4.Font.Bold = true;
                                    lblMLC4.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory4.Visible = common.myBool(str3[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "','" + str3[9] + "','" + hdnIsCritical4.ClientID + "','" + hdnUnperformedServices4.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;

                    }
                    if (str4.Length > 1)
                    {

                        lblEnc5.Text = str4[3];
                        lblPatientName5.Text = str4[7];
                        lblCompanytype5.Text = str4[6];
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");

                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);

                                HiddenField hdnIsCritical5 = (HiddenField)e.Row.FindControl("hdnIsCritical5");
                                Label lblCriticalIndication5 = (Label)e.Row.FindControl("lblCriticalIndication5");
                                if (str4[12].Contains("1"))
                                {
                                    lblCriticalIndication5.Visible = true;
                                    lblCriticalIndication5.ToolTip = "Test results with panic values.";
                                    hdnIsCritical5.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication5.Visible = false;
                                    lblCriticalIndication5.ToolTip = string.Empty;
                                    hdnIsCritical5.Value = "0";
                                }

                                HiddenField hdnUnperformedServices5 = (HiddenField)e.Row.FindControl("hdnUnperformedServices5");
                                Label lblUnperformedServices5 = (Label)e.Row.FindControl("lblUnperformedServices5");
                                if (str4[13].Contains("1"))
                                {
                                    lblUnperformedServices5.Visible = true;
                                    lblUnperformedServices5.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices5.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices5.Visible = false;
                                    lblUnperformedServices5.ToolTip = string.Empty;
                                    hdnUnperformedServices5.Value = "0";
                                }


                                #region forOTBillCleareancecou5Image5
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str4[14].Contains("1"))
                                {
                                    imgbill5.Visible = true;
                                    imgbill5.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill5.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill5.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill5.Visible = false;
                                    imgbill5.Attributes.Add("style", "display:none");
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert5.Visible = common.myBool(str4[15]);
                                imgMedicalAlert5.Visible = common.myBool(str4[16]);
                                #endregion


                                #region MLC Patient
                                if (str4[17].Contains("True"))
                                {
                                    lblMLC5.Visible = true;
                                    lblMLC5.Text = "MLC";
                                    lblMLC5.Font.Bold = true;
                                    lblMLC5.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory5.Visible = common.myBool(str4[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "','" + str4[9] + "','" + hdnIsCritical5.ClientID + "','" + hdnUnperformedServices5.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;

                    }
                    HtmlControl div5 = (HtmlControl)e.Row.FindControl("div6");
                    div5.Visible = false;

                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div7");
                    div6.Visible = false;

                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div8");
                    div7.Visible = false;

                    HtmlControl div8 = (HtmlControl)e.Row.FindControl("div9");
                    div8.Visible = false;

                    HtmlControl div9 = (HtmlControl)e.Row.FindControl("div10");
                    div9.Visible = false;
                }
                if (noOfColumn == 6)
                {

                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');
                    string[] str5 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "5")).Split('@');



                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];
                    label6.Text = str5[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];

                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");
                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }


                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");
                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }


                                #region forOTBillCleareancecou6Image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //imgbill.Attributes.Add("style", "display:none");
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    //imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    //imgbill.Visible = false;
                                    imgbill.Attributes.Add("style", "display:none");
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion


                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);

                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }


                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");
                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }


                                #region forOTBillCleareancecou6Image2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill2.ImageUrl = "~/Images/PendingBill.PNG";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    //imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    //imgbill2.Attributes.Add("style", "display:none");

                                    imgbill2.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion
                                imgbill2.Attributes.Add("style", "display:none");


                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion


                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }
                    if (str2.Length > 1)
                    {
                        lblEnc3.Text = str2[3];
                        lblPatientName3.Text = str2[7];
                        lblCompanytype3.Text = str2[6];
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);

                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);

                                HiddenField hdnIsCritical3 = (HiddenField)e.Row.FindControl("hdnIsCritical3");
                                Label lblCriticalIndication3 = (Label)e.Row.FindControl("lblCriticalIndication3");
                                if (str2[12].Contains("1"))
                                {
                                    lblCriticalIndication3.Visible = true;
                                    lblCriticalIndication3.ToolTip = "Test results with panic values.";
                                    hdnIsCritical3.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication3.Visible = false;
                                    lblCriticalIndication3.ToolTip = string.Empty;
                                    hdnIsCritical3.Value = "0";
                                }


                                HiddenField hdnUnperformedServices3 = (HiddenField)e.Row.FindControl("hdnUnperformedServices3");
                                Label lblUnperformedServices3 = (Label)e.Row.FindControl("lblUnperformedServices3");
                                if (str2[13].Contains("1"))
                                {
                                    lblUnperformedServices3.Visible = true;
                                    lblUnperformedServices3.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices3.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices3.Visible = false;
                                    lblUnperformedServices3.ToolTip = string.Empty;
                                    hdnUnperformedServices3.Value = "0";
                                }

                                #region forOTBillCleareancecou6Image3
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str2[14].Contains("1"))
                                {
                                    imgbill3.Visible = true;
                                    imgbill3.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill3.ImageUrl = "~/Images/PendingBill.PNG";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    //imgbill3.Attributes.Add("style", "display:none");
                                    //imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill3.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert3.Visible = common.myBool(str2[15]);
                                imgMedicalAlert3.Visible = common.myBool(str2[16]);
                                #endregion

                                #region MLC Patient
                                if (str2[17].Contains("True"))
                                {
                                    lblMLC3.Visible = true;
                                    lblMLC3.Text = "MLC";
                                    lblMLC3.Font.Bold = true;
                                    lblMLC3.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory3.Visible = common.myBool(str2[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "','" + str2[9] + "','" + hdnIsCritical3.ClientID + "','" + hdnUnperformedServices3.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;

                    }
                    if (str3.Length > 1)
                    {
                        lblEnc4.Text = str3[3];
                        lblPatientName4.Text = str3[7];
                        lblCompanytype4.Text = str3[6];
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);

                                HiddenField hdnIsCritical4 = (HiddenField)e.Row.FindControl("hdnIsCritical4");
                                Label lblCriticalIndication4 = (Label)e.Row.FindControl("lblCriticalIndication4");
                                if (str3[12].Contains("1"))
                                {
                                    lblCriticalIndication4.Visible = true;
                                    lblCriticalIndication4.ToolTip = "Test results with panic values.";
                                    hdnIsCritical4.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication4.Visible = false;
                                    lblCriticalIndication4.ToolTip = string.Empty;
                                    hdnIsCritical4.Value = "0";
                                }


                                HiddenField hdnUnperformedServices4 = (HiddenField)e.Row.FindControl("hdnUnperformedServices4");
                                Label lblUnperformedServices4 = (Label)e.Row.FindControl("lblUnperformedServices4");
                                if (str3[13].Contains("1"))
                                {
                                    lblUnperformedServices4.Visible = true;
                                    lblUnperformedServices4.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices4.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices4.Visible = false;
                                    lblUnperformedServices4.ToolTip = string.Empty;
                                    hdnUnperformedServices4.Value = "0";
                                }



                                #region forOTBillCleareancecou6Image4
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str3[14].Contains("1"))
                                {
                                    imgbill4.Visible = true;
                                    imgbill4.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill4.Visible = false;
                                    //imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    //imgbill4.Attributes.Add("style", "display:none");
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert4.Visible = common.myBool(str3[15]);
                                imgMedicalAlert4.Visible = common.myBool(str3[16]);
                                #endregion


                                #region MLC Patient
                                if (str3[17].Contains("True"))
                                {
                                    lblMLC4.Visible = true;
                                    lblMLC4.Text = "MLC";
                                    lblMLC4.Font.Bold = true;
                                    lblMLC4.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory4.Visible = common.myBool(str3[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "','" + str3[9] + "','" + hdnIsCritical4.ClientID + "','" + hdnUnperformedServices4.ClientID + "');");

                            }
                        }

                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;

                    }
                    if (str4.Length > 1)
                    {
                        lblEnc5.Text = str4[3];
                        lblPatientName5.Text = str4[7];
                        lblCompanytype5.Text = str4[6];
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);

                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);

                                HiddenField hdnIsCritical5 = (HiddenField)e.Row.FindControl("hdnIsCritical5");
                                Label lblCriticalIndication5 = (Label)e.Row.FindControl("lblCriticalIndication5");
                                if (str4[12].Contains("1"))
                                {
                                    lblCriticalIndication5.Visible = true;
                                    lblCriticalIndication5.ToolTip = "Test results with panic values.";
                                    hdnIsCritical5.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication5.Visible = false;
                                    lblCriticalIndication5.ToolTip = string.Empty;
                                    hdnIsCritical5.Value = "0";
                                }


                                HiddenField hdnUnperformedServices5 = (HiddenField)e.Row.FindControl("hdnUnperformedServices5");
                                Label lblUnperformedServices5 = (Label)e.Row.FindControl("lblUnperformedServices5");
                                if (str4[13].Contains("1"))
                                {
                                    lblUnperformedServices5.Visible = true;
                                    lblUnperformedServices5.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices5.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices5.Visible = false;
                                    lblUnperformedServices5.ToolTip = string.Empty;
                                    hdnUnperformedServices5.Value = "0";
                                }

                                #region forOTBillCleareancecou6Image5
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str4[14].Contains("1"))
                                {
                                    imgbill5.Visible = true;
                                    imgbill5.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill5.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill5.Visible = false;
                                    //imgbill5.ImageUrl = "~/Images/PendingBill.png";
                                    //imgbill5.Attributes.Add("style", "display:none");

                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert5.Visible = common.myBool(str4[15]);
                                imgMedicalAlert5.Visible = common.myBool(str4[16]);
                                #endregion


                                #region MLC Patient
                                if (str4[17].Contains("True"))
                                {
                                    lblMLC5.Visible = true;
                                    lblMLC5.Text = "MLC";
                                    lblMLC5.Font.Bold = true;
                                    lblMLC5.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory5.Visible = common.myBool(str4[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "','" + str4[9] + "','" + hdnIsCritical5.ClientID + "','" + hdnUnperformedServices5.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;

                    }
                    if (str5.Length > 1)
                    {
                        lblEnc6.Text = str5[3];
                        lblPatientName6.Text = str5[7];
                        lblCompanytype6.Text = str5[6];
                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);

                                HiddenField hdnIsCritical6 = (HiddenField)e.Row.FindControl("hdnIsCritical6");
                                Label lblCriticalIndication6 = (Label)e.Row.FindControl("lblCriticalIndication6");
                                if (str5[12].Contains("1"))
                                {
                                    lblCriticalIndication6.Visible = true;
                                    lblCriticalIndication6.ToolTip = "Test results with panic values.";
                                    hdnIsCritical6.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication6.Visible = false;
                                    lblCriticalIndication6.ToolTip = string.Empty;
                                    hdnIsCritical6.Value = "0";
                                }


                                HiddenField hdnUnperformedServices6 = (HiddenField)e.Row.FindControl("hdnUnperformedServices6");
                                Label lblUnperformedServices6 = (Label)e.Row.FindControl("lblUnperformedServices6");


                                if (str5[13].Contains("1"))
                                {
                                    lblUnperformedServices6.Visible = true;
                                    lblUnperformedServices6.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices6.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices6.Visible = false;
                                    lblUnperformedServices6.ToolTip = string.Empty;
                                    hdnUnperformedServices6.Value = "0";
                                }

                                #region forOTBillCleareancecou6Image6
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str5[14].Contains("1"))
                                {
                                    imgbill6.Visible = true;
                                    imgbill6.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    imgbill6.ImageUrl = "~/Images/PendingBill.png";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    //imgbill6.ImageUrl = "~/Images/PendingBill.png";
                                    //imgbill6.Attributes.Add("style", "display:none");
                                    imgbill6.Visible = false;
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert6.Visible = common.myBool(str5[15]);
                                imgMedicalAlert6.Visible = common.myBool(str5[16]);
                                #endregion


                                #region MLC Patient
                                if (str5[17].Contains("True"))
                                {
                                    lblMLC6.Visible = true;
                                    lblMLC6.Text = "MLC";
                                    lblMLC6.Font.Bold = true;
                                    lblMLC6.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory6.Visible = common.myBool(str5[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "','" + str5[9] + "','" + hdnIsCritical6.ClientID + "','" + hdnUnperformedServices6.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton6.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                        div.Visible = false;

                    }


                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div7");
                    div6.Visible = false;

                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div8");
                    div7.Visible = false;

                    HtmlControl div8 = (HtmlControl)e.Row.FindControl("div9");
                    div8.Visible = false;

                    HtmlControl div9 = (HtmlControl)e.Row.FindControl("div10");
                    div9.Visible = false;


                }
                if (noOfColumn == 7)
                {

                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');
                    string[] str5 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "5")).Split('@');
                    string[] str6 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "6")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];
                    label6.Text = str5[0];
                    label7.Text = str6[0];
                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");
                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }

                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");


                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }

                                #region forOTBillCleareancecou7Image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.Visible = false;
                                    imgbill.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion


                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);

                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }

                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");

                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }


                                #region forOTBillCleareancecou7Image2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill2.Visible = false;
                                    imgbill2.Dispose();

                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion

                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }
                    if (str2.Length > 1)
                    {
                        lblEnc3.Text = str2[3];
                        lblPatientName3.Text = str2[7];
                        lblCompanytype3.Text = str2[6];
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);


                                HiddenField hdnIsCritical3 = (HiddenField)e.Row.FindControl("hdnIsCritical3");
                                Label lblCriticalIndication3 = (Label)e.Row.FindControl("lblCriticalIndication3");
                                if (str2[12].Contains("1"))
                                {
                                    lblCriticalIndication3.Visible = true;
                                    lblCriticalIndication3.ToolTip = "Test results with panic values.";
                                    hdnIsCritical3.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication3.Visible = false;
                                    lblCriticalIndication3.ToolTip = string.Empty;
                                    hdnIsCritical3.Value = "0";
                                }

                                HiddenField hdnUnperformedServices3 = (HiddenField)e.Row.FindControl("hdnUnperformedServices3");
                                Label lblUnperformedServices3 = (Label)e.Row.FindControl("lblUnperformedServices3");

                                if (str2[13].Contains("1"))
                                {
                                    lblUnperformedServices3.Visible = true;
                                    lblUnperformedServices3.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices3.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices3.Visible = false;
                                    lblUnperformedServices3.ToolTip = string.Empty;
                                    hdnUnperformedServices3.Value = "0";
                                }

                                #region forOTBillCleareancecou7Image3
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str2[14].Contains("1"))
                                {
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill3.Visible = true;
                                    imgbill3.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill3.Visible = false;
                                    imgbill3.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert3.Visible = common.myBool(str2[15]);
                                imgMedicalAlert3.Visible = common.myBool(str2[16]);
                                #endregion


                                #region MLC Patient
                                if (str2[17].Contains("True"))
                                {
                                    lblMLC3.Visible = true;
                                    lblMLC3.Text = "MLC";
                                    lblMLC3.Font.Bold = true;
                                    lblMLC3.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory3.Visible = common.myBool(str2[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "','" + str2[9] + "','" + hdnIsCritical3.ClientID + "','" + hdnUnperformedServices3.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;

                    }
                    if (str3.Length > 1)
                    {
                        lblEnc4.Text = str3[3];
                        lblPatientName4.Text = str3[7];
                        lblCompanytype4.Text = str3[6];
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);


                                HiddenField hdnIsCritical4 = (HiddenField)e.Row.FindControl("hdnIsCritical4");
                                Label lblCriticalIndication4 = (Label)e.Row.FindControl("lblCriticalIndication4");
                                if (str3[12].Contains("1"))
                                {
                                    lblCriticalIndication4.Visible = true;
                                    lblCriticalIndication4.ToolTip = "Test results with panic values.";
                                    hdnIsCritical4.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication4.Visible = false;
                                    lblCriticalIndication4.ToolTip = string.Empty;
                                    hdnIsCritical4.Value = "0";
                                }

                                HiddenField hdnUnperformedServices4 = (HiddenField)e.Row.FindControl("hdnUnperformedServices4");
                                Label lblUnperformedServices4 = (Label)e.Row.FindControl("lblUnperformedServices4");

                                if (str3[13].Contains("1"))
                                {
                                    lblUnperformedServices4.Visible = true;
                                    lblUnperformedServices4.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices4.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices4.Visible = false;
                                    lblUnperformedServices4.ToolTip = string.Empty;
                                    hdnUnperformedServices4.Value = "0";
                                }

                                #region forOTBillCleareancecou7Image4
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str3[14].Contains("1"))
                                {
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill4.Visible = true;
                                    imgbill4.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill4.Visible = false;
                                    imgbill4.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert4.Visible = common.myBool(str3[15]);
                                imgMedicalAlert4.Visible = common.myBool(str3[16]);
                                #endregion


                                #region MLC Patient
                                if (str3[17].Contains("True"))
                                {
                                    lblMLC4.Visible = true;
                                    lblMLC4.Text = "MLC";
                                    lblMLC4.Font.Bold = true;
                                    lblMLC4.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory4.Visible = common.myBool(str3[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "','" + str3[9] + "','" + hdnIsCritical4.ClientID + "','" + hdnUnperformedServices4.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;

                    }
                    if (str4.Length > 1)
                    {
                        lblEnc5.Text = str4[3];
                        lblPatientName5.Text = str4[7];
                        lblCompanytype5.Text = str4[6];
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //  imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);


                                HiddenField hdnIsCritical5 = (HiddenField)e.Row.FindControl("hdnIsCritical5");
                                Label lblCriticalIndication5 = (Label)e.Row.FindControl("lblCriticalIndication5");
                                if (str4[12].Contains("1"))
                                {
                                    lblCriticalIndication5.Visible = true;
                                    lblCriticalIndication5.ToolTip = "Test results with panic values.";
                                    hdnIsCritical5.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication5.Visible = false;
                                    lblCriticalIndication5.ToolTip = string.Empty;
                                    hdnIsCritical5.Value = "0";
                                }


                                HiddenField hdnUnperformedServices5 = (HiddenField)e.Row.FindControl("hdnUnperformedServices5");
                                Label lblUnperformedServices5 = (Label)e.Row.FindControl("lblUnperformedServices5");

                                if (str4[13].Contains("1"))
                                {
                                    lblUnperformedServices5.Visible = true;
                                    lblUnperformedServices5.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices5.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices5.Visible = false;
                                    lblUnperformedServices5.ToolTip = string.Empty;
                                    hdnUnperformedServices5.Value = "0";
                                }


                                #region forOTBillCleareancecou7Image5
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str4[14].Contains("1"))
                                {
                                    imgbill5.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill5.Visible = true;
                                    imgbill5.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill5.Visible = false;
                                    imgbill5.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert5.Visible = common.myBool(str4[15]);
                                imgMedicalAlert5.Visible = common.myBool(str4[16]);
                                #endregion


                                #region MLC Patient
                                if (str4[17].Contains("True"))
                                {
                                    lblMLC5.Visible = true;
                                    lblMLC5.Text = "MLC";
                                    lblMLC5.Font.Bold = true;
                                    lblMLC5.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory5.Visible = common.myBool(str4[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "','" + str4[9] + "','" + hdnIsCritical5.ClientID + "','" + hdnUnperformedServices5.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;

                    }
                    if (str5.Length > 1)
                    {
                        lblEnc6.Text = str5[3];
                        lblPatientName6.Text = str5[7];
                        lblCompanytype6.Text = str5[6];
                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);

                                HiddenField hdnIsCritical6 = (HiddenField)e.Row.FindControl("hdnIsCritical6");
                                Label lblCriticalIndication6 = (Label)e.Row.FindControl("lblCriticalIndication6");
                                if (str5[12].Contains("1"))
                                {
                                    lblCriticalIndication6.Visible = true;
                                    lblCriticalIndication6.ToolTip = "Test results with panic values.";
                                    hdnIsCritical6.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication6.Visible = false;
                                    lblCriticalIndication6.ToolTip = string.Empty;
                                    hdnIsCritical6.Value = "0";
                                }


                                HiddenField hdnUnperformedServices6 = (HiddenField)e.Row.FindControl("hdnUnperformedServices6");
                                Label lblUnperformedServices6 = (Label)e.Row.FindControl("lblUnperformedServices6");

                                if (str5[13].Contains("1"))
                                {
                                    lblUnperformedServices6.Visible = true;
                                    lblUnperformedServices6.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices6.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices6.Visible = false;
                                    lblUnperformedServices6.ToolTip = string.Empty;
                                    hdnUnperformedServices6.Value = "0";
                                }

                                #region forOTBillCleareancecou7Image6
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str5[14].Contains("1"))
                                {
                                    imgbill6.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill6.Visible = true;
                                    imgbill6.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill6.Visible = false;
                                    imgbill6.Dispose();

                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert6.Visible = common.myBool(str5[15]);
                                imgMedicalAlert6.Visible = common.myBool(str5[16]);
                                #endregion

                                #region MLC Patient
                                if (str5[17].Contains("True"))
                                {
                                    lblMLC6.Visible = true;
                                    lblMLC6.Text = "MLC";
                                    lblMLC6.Font.Bold = true;
                                    lblMLC6.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory6.Visible = common.myBool(str5[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "','" + str5[9] + "','" + hdnIsCritical6.ClientID + "','" + hdnUnperformedServices6.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton6.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                        div.Visible = false;

                    }

                    if (str6.Length > 1)
                    {
                        lblEnc7.Text = str6[3];
                        lblPatientName7.Text = str6[7];
                        lblCompanytype7.Text = str6[6];
                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");

                            }
                            else if (str6[1] == "R")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "H")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "I")
                            {
                                //imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton7.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                            }
                            else if (str6[1] == "B")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str6[1] == "U")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno7.Value = str6[2];
                                hdnencounterno7.Value = str6[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                                div.Attributes.Add("style", "background-color:" + str6[8] + ";");
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);


                                HiddenField hdnIsCritical7 = (HiddenField)e.Row.FindControl("hdnIsCritical7");
                                Label lblCriticalIndication7 = (Label)e.Row.FindControl("lblCriticalIndication7");
                                if (str6[12].Contains("1"))
                                {
                                    lblCriticalIndication7.Visible = true;
                                    lblCriticalIndication7.ToolTip = "Test results with panic values.";
                                    hdnIsCritical7.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication7.Visible = false;
                                    lblCriticalIndication7.ToolTip = string.Empty;
                                    hdnIsCritical7.Value = "0";
                                }


                                HiddenField hdnUnperformedServices7 = (HiddenField)e.Row.FindControl("hdnUnperformedServices7");
                                Label lblUnperformedServices7 = (Label)e.Row.FindControl("lblUnperformedServices7");

                                if (str6[13].Contains("1"))
                                {
                                    lblUnperformedServices7.Visible = true;
                                    lblUnperformedServices7.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices7.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices7.Visible = false;
                                    lblUnperformedServices7.ToolTip = string.Empty;
                                    hdnUnperformedServices7.Value = "0";
                                }

                                #region forOTBillCleareancecou7Image7
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str6[14].Contains("1"))
                                {
                                    imgbill7.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill7.Visible = true;
                                    imgbill7.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill7.Visible = false;
                                    imgbill7.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert7.Visible = common.myBool(str6[15]);
                                imgMedicalAlert7.Visible = common.myBool(str6[16]);
                                #endregion


                                #region MLC Patient
                                if (str6[17].Contains("True"))
                                {
                                    lblMLC7.Visible = true;
                                    lblMLC7.Text = "MLC";
                                    lblMLC7.Font.Bold = true;
                                    lblMLC7.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory7.Visible = common.myBool(str6[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[2] + "','" + str6[3] + "','" + str6[9] + "','" + hdnIsCritical7.ClientID + "','" + hdnUnperformedServices7.ClientID + "');");

                            }
                        }

                    }
                    else
                    {
                        imgbutton7.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                        div.Visible = false;

                    }
                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div8");
                    div7.Visible = false;

                    HtmlControl div8 = (HtmlControl)e.Row.FindControl("div9");
                    div8.Visible = false;

                    HtmlControl div9 = (HtmlControl)e.Row.FindControl("div10");
                    div9.Visible = false;
                }
                if (noOfColumn == 8)
                {


                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');
                    string[] str5 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "5")).Split('@');
                    string[] str6 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "6")).Split('@');
                    string[] str7 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "7")).Split('@');


                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];
                    label6.Text = str5[0];
                    label7.Text = str6[0];
                    label8.Text = str7[0];
                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");
                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }


                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");


                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }

                                #region forOTBillCleareancecou8Image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.Visible = false;
                                    imgbill.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion


                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);
                        lblCompanytype.Text = "";
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                        div.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);


                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }

                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");


                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }

                                #region forOTBillCleareancecou8Image2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill2.Visible = false;
                                    imgbill2.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion

                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }
                    if (str2.Length > 1)
                    {
                        lblEnc3.Text = str2[3];
                        lblPatientName3.Text = str2[7];
                        lblCompanytype3.Text = str2[6];
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);


                                HiddenField hdnIsCritical3 = (HiddenField)e.Row.FindControl("hdnIsCritical3");
                                Label lblCriticalIndication3 = (Label)e.Row.FindControl("lblCriticalIndication3");
                                if (str2[12].Contains("1"))
                                {
                                    lblCriticalIndication3.Visible = true;
                                    lblCriticalIndication3.ToolTip = "Test results with panic values.";
                                    hdnIsCritical3.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication3.Visible = false;
                                    lblCriticalIndication3.ToolTip = string.Empty;
                                    hdnIsCritical3.Value = "0";
                                }

                                HiddenField hdnUnperformedServices3 = (HiddenField)e.Row.FindControl("hdnUnperformedServices3");
                                Label lblUnperformedServices3 = (Label)e.Row.FindControl("lblUnperformedServices3");


                                if (str2[13].Contains("1"))
                                {
                                    lblUnperformedServices3.Visible = true;
                                    lblUnperformedServices3.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices3.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices3.Visible = false;
                                    lblUnperformedServices3.ToolTip = string.Empty;
                                    hdnUnperformedServices3.Value = "0";
                                }

                                #region forOTBillCleareancecou8Image3
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str2[14].Contains("1"))
                                {
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill3.Visible = true;
                                    imgbill3.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill3.Visible = false;
                                    imgbill3.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert3.Visible = common.myBool(str2[15]);
                                imgMedicalAlert3.Visible = common.myBool(str2[16]);
                                #endregion

                                #region MLC Patient
                                if (str2[17].Contains("True"))
                                {
                                    lblMLC3.Visible = true;
                                    lblMLC3.Text = "MLC";
                                    lblMLC3.Font.Bold = true;
                                    lblMLC3.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory3.Visible = common.myBool(str2[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "','" + str2[9] + "','" + hdnIsCritical3.ClientID + "','" + hdnUnperformedServices3.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;

                    }
                    if (str3.Length > 1)
                    {
                        lblEnc4.Text = str3[3];
                        lblPatientName4.Text = str3[7];
                        lblCompanytype4.Text = str3[6];
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);


                                HiddenField hdnIsCritical4 = (HiddenField)e.Row.FindControl("hdnIsCritical4");
                                Label lblCriticalIndication4 = (Label)e.Row.FindControl("lblCriticalIndication4");
                                if (str3[12].Contains("1"))
                                {
                                    lblCriticalIndication4.Visible = true;
                                    lblCriticalIndication4.ToolTip = "Test results with panic values.";
                                    hdnIsCritical4.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication4.Visible = false;
                                    lblCriticalIndication4.ToolTip = string.Empty;
                                    hdnIsCritical4.Value = "0";
                                }


                                HiddenField hdnUnperformedServices4 = (HiddenField)e.Row.FindControl("hdnUnperformedServices4");
                                Label lblUnperformedServices4 = (Label)e.Row.FindControl("lblUnperformedServices4");


                                if (str3[13].Contains("1"))
                                {
                                    lblUnperformedServices4.Visible = true;
                                    lblUnperformedServices4.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices4.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices4.Visible = false;
                                    lblUnperformedServices4.ToolTip = string.Empty;
                                    hdnUnperformedServices4.Value = "0";
                                }


                                #region forOTBillCleareancecou8Image4
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str3[14].Contains("1"))
                                {
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill4.Visible = true;
                                    imgbill4.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill4.Visible = false;
                                    imgbill4.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert4.Visible = common.myBool(str3[15]);
                                imgMedicalAlert4.Visible = common.myBool(str3[16]);
                                #endregion


                                #region MLC Patient
                                if (str3[17].Contains("True"))
                                {
                                    lblMLC4.Visible = true;
                                    lblMLC4.Text = "MLC";
                                    lblMLC4.Font.Bold = true;
                                    lblMLC4.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory4.Visible = common.myBool(str3[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "','" + str3[9] + "','" + hdnIsCritical4.ClientID + "','" + hdnUnperformedServices4.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;

                    }
                    if (str4.Length > 1)
                    {
                        lblEnc5.Text = str4[3];
                        lblPatientName5.Text = str4[7];
                        lblCompanytype5.Text = str4[6];
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);


                                HiddenField hdnIsCritical5 = (HiddenField)e.Row.FindControl("hdnIsCritical5");
                                Label lblCriticalIndication5 = (Label)e.Row.FindControl("lblCriticalIndication5");
                                if (str4[12].Contains("1"))
                                {
                                    lblCriticalIndication5.Visible = true;
                                    lblCriticalIndication5.ToolTip = "Test results with panic values.";
                                    hdnIsCritical5.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication5.Visible = false;
                                    lblCriticalIndication5.ToolTip = string.Empty;
                                    hdnIsCritical5.Value = "0";
                                }


                                HiddenField hdnUnperformedServices5 = (HiddenField)e.Row.FindControl("hdnUnperformedServices5");
                                Label lblUnperformedServices5 = (Label)e.Row.FindControl("lblUnperformedServices5");

                                if (str4[13].Contains("1"))
                                {
                                    lblUnperformedServices5.Visible = true;
                                    lblUnperformedServices5.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices5.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices5.Visible = false;
                                    lblUnperformedServices5.ToolTip = string.Empty;
                                    hdnUnperformedServices5.Value = "0";
                                }

                                #region forOTBillCleareancecou8Image5
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str4[14].Contains("1"))
                                {
                                    imgbill5.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill5.Visible = true;
                                    imgbill5.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill5.Visible = false;
                                    imgbill5.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert5.Visible = common.myBool(str4[15]);
                                imgMedicalAlert5.Visible = common.myBool(str4[16]);
                                #endregion

                                #region MLC Patient
                                if (str4[17].Contains("True"))
                                {
                                    lblMLC5.Visible = true;
                                    lblMLC5.Text = "MLC";
                                    lblMLC5.Font.Bold = true;
                                    lblMLC5.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory5.Visible = common.myBool(str4[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "','" + str4[9] + "','" + hdnIsCritical5.ClientID + "','" + hdnUnperformedServices5.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;

                    }
                    if (str5.Length > 1)
                    {
                        lblEnc6.Text = str5[3];
                        lblPatientName6.Text = str5[7];
                        lblCompanytype6.Text = str5[6];
                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);

                                HiddenField hdnIsCritical6 = (HiddenField)e.Row.FindControl("hdnIsCritical6");
                                Label lblCriticalIndication6 = (Label)e.Row.FindControl("lblCriticalIndication6");
                                if (str5[12].Contains("1"))
                                {
                                    lblCriticalIndication6.Visible = true;
                                    lblCriticalIndication6.ToolTip = "Test results with panic values.";
                                    hdnIsCritical6.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication6.Visible = false;
                                    lblCriticalIndication6.ToolTip = string.Empty;
                                    hdnIsCritical6.Value = "0";
                                }


                                HiddenField hdnUnperformedServices6 = (HiddenField)e.Row.FindControl("hdnUnperformedServices6");
                                Label lblUnperformedServices6 = (Label)e.Row.FindControl("lblUnperformedServices6");

                                if (str5[13].Contains("1"))
                                {
                                    lblUnperformedServices6.Visible = true;
                                    lblUnperformedServices6.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices6.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices6.Visible = false;
                                    lblUnperformedServices6.ToolTip = string.Empty;
                                    hdnUnperformedServices6.Value = "0";
                                }


                                #region forOTBillCleareancecou8Image6
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str5[14].Contains("1"))
                                {
                                    imgbill6.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill6.Visible = true;
                                    imgbill6.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill6.Visible = false;
                                    imgbill6.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert6.Visible = common.myBool(str5[15]);
                                imgMedicalAlert6.Visible = common.myBool(str5[16]);
                                #endregion

                                #region MLC Patient
                                if (str5[17].Contains("True"))
                                {
                                    lblMLC6.Visible = true;
                                    lblMLC6.Text = "MLC";
                                    lblMLC6.Font.Bold = true;
                                    lblMLC6.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory6.Visible = common.myBool(str5[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "','" + str5[9] + "','" + hdnIsCritical6.ClientID + "','" + hdnUnperformedServices6.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton6.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                        div.Visible = false;

                    }
                    if (str6.Length > 1)
                    {
                        lblEnc7.Text = str6[3];
                        lblPatientName7.Text = str6[7];
                        lblCompanytype7.Text = str6[6];
                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str1[1] + "');");
                            }
                            else if (str6[1] == "R")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "H")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "I")
                            {
                                //imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton7.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                                div.Attributes.Add("style", "background-color:" + str6[8] + ";");
                            }
                            else if (str6[1] == "B")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str6[1] == "U")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno7.Value = str6[2];
                                hdnencounterno7.Value = str6[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                                div.Attributes.Add("style", "background-color:" + str6[8] + ";");
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);


                                HiddenField hdnIsCritical7 = (HiddenField)e.Row.FindControl("hdnIsCritical7");
                                Label lblCriticalIndication7 = (Label)e.Row.FindControl("lblCriticalIndication7");
                                if (str6[12].Contains("1"))
                                {
                                    lblCriticalIndication7.Visible = true;
                                    lblCriticalIndication7.ToolTip = "Test results with panic values.";
                                    hdnIsCritical7.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication7.Visible = false;
                                    lblCriticalIndication7.ToolTip = string.Empty;
                                    hdnIsCritical7.Value = "0";
                                }


                                HiddenField hdnUnperformedServices7 = (HiddenField)e.Row.FindControl("hdnUnperformedServices7");
                                Label lblUnperformedServices7 = (Label)e.Row.FindControl("lblUnperformedServices7");

                                if (str6[13].Contains("1"))
                                {
                                    lblUnperformedServices7.Visible = true;
                                    lblUnperformedServices7.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices7.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices7.Visible = false;
                                    lblUnperformedServices7.ToolTip = string.Empty;
                                    hdnUnperformedServices7.Value = "0";
                                }



                                #region forOTBillCleareancecou8Image7
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str6[14].Contains("1"))
                                {
                                    imgbill7.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill7.Visible = true;
                                    imgbill7.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill7.Visible = false;
                                    imgbill7.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert7.Visible = common.myBool(str6[15]);
                                imgMedicalAlert7.Visible = common.myBool(str6[16]);
                                #endregion

                                #region MLC Patient
                                if (str6[17].Contains("True"))
                                {
                                    lblMLC7.Visible = true;
                                    lblMLC7.Text = "MLC";
                                    lblMLC7.Font.Bold = true;
                                    lblMLC7.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory7.Visible = common.myBool(str6[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[2] + "','" + str6[3] + "','" + str6[9] + "','" + hdnIsCritical7.ClientID + "','" + hdnUnperformedServices7.ClientID + "');");

                            }
                        }

                    }
                    else
                    {
                        imgbutton7.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                        div.Visible = false;

                    }
                    if (str7.Length > 1)
                    {
                        lblEnc8.Text = str7[3];
                        lblPatientName8.Text = str7[7];
                        lblCompanytype8.Text = str7[6];
                        if (label8.Text != "")
                        {
                            if (str7[1] == "V")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");

                            }
                            else if (str7[1] == "R")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "H")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "I")
                            {
                                //imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton8.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div8");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str7[1] == "B")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str7[1] == "U")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno8.Value = str7[2];
                                hdnencounterno8.Value = str7[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div8");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);


                                HiddenField hdnIsCritical8 = (HiddenField)e.Row.FindControl("hdnIsCritical8");
                                Label lblCriticalIndication8 = (Label)e.Row.FindControl("lblCriticalIndication8");
                                if (str7[12].Contains("1"))
                                {
                                    lblCriticalIndication8.Visible = true;
                                    lblCriticalIndication8.ToolTip = "Test results with panic values.";
                                    hdnIsCritical8.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication8.Visible = false;
                                    lblCriticalIndication8.ToolTip = string.Empty;
                                    hdnIsCritical8.Value = "0";
                                }


                                HiddenField hdnUnperformedServices8 = (HiddenField)e.Row.FindControl("hdnUnperformedServices8");
                                Label lblUnperformedServices8 = (Label)e.Row.FindControl("lblUnperformedServices8");

                                if (str7[13].Contains("1"))
                                {
                                    lblUnperformedServices8.Visible = true;
                                    lblUnperformedServices8.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices8.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices8.Visible = false;
                                    lblUnperformedServices8.ToolTip = string.Empty;
                                    hdnUnperformedServices8.Value = "0";
                                }

                                #region forOTBillCleareancecou8Image8
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str7[14].Contains("1"))
                                {
                                    imgbill8.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill8.Visible = true;
                                    imgbill8.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill8.Visible = false;
                                    imgbill8.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert8.Visible = common.myBool(str7[15]);
                                imgMedicalAlert8.Visible = common.myBool(str7[16]);
                                #endregion

                                #region MLC Patient
                                if (str7[17].Contains("True"))
                                {
                                    lblMLC8.Visible = true;
                                    lblMLC8.Text = "MLC";
                                    lblMLC8.Font.Bold = true;
                                    lblMLC8.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory8.Visible = common.myBool(str7[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[2] + "','" + str7[3] + "','" + str7[9] + "','" + hdnIsCritical8.ClientID + "','" + hdnUnperformedServices8.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton8.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div8");
                        div.Visible = false;

                    }
                    HtmlControl div8 = (HtmlControl)e.Row.FindControl("div9");
                    div8.Visible = false;

                    HtmlControl div9 = (HtmlControl)e.Row.FindControl("div10");
                    div9.Visible = false;
                }

                if (noOfColumn == 9)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');
                    string[] str5 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "5")).Split('@');
                    string[] str6 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "6")).Split('@');
                    string[] str7 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "7")).Split('@');
                    string[] str8 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "8")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];
                    label6.Text = str5[0];
                    label7.Text = str6[0];
                    label8.Text = str7[0];
                    label9.Text = str8[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");
                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }


                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");

                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }

                                #region forOTBillCleareancecou9Image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.Visible = false;
                                    imgbill.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion


                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);


                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }


                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");

                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }

                                #region forOTBillCleareancecou9Image2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill2.Visible = false;
                                    imgbill2.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion


                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }
                    if (str2.Length > 1)
                    {
                        lblEnc3.Text = str2[3];
                        lblPatientName3.Text = str2[7];
                        lblCompanytype3.Text = str2[6];
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);


                                HiddenField hdnIsCritical3 = (HiddenField)e.Row.FindControl("hdnIsCritical3");
                                Label lblCriticalIndication3 = (Label)e.Row.FindControl("lblCriticalIndication3");
                                if (str2[12].Contains("1"))
                                {
                                    lblCriticalIndication3.Visible = true;
                                    lblCriticalIndication3.ToolTip = "Test results with panic values.";
                                    hdnIsCritical3.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication3.Visible = false;
                                    lblCriticalIndication3.ToolTip = string.Empty;
                                    hdnIsCritical3.Value = "0";
                                }

                                HiddenField hdnUnperformedServices3 = (HiddenField)e.Row.FindControl("hdnUnperformedServices3");
                                Label lblUnperformedServices3 = (Label)e.Row.FindControl("lblUnperformedServices3");

                                if (str2[13].Contains("1"))
                                {
                                    lblUnperformedServices3.Visible = true;
                                    lblUnperformedServices3.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices3.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices3.Visible = false;
                                    lblUnperformedServices3.ToolTip = string.Empty;
                                    hdnUnperformedServices3.Value = "0";
                                }

                                #region forOTBillCleareancecou9Image3
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str2[14].Contains("1"))
                                {
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill3.Visible = true;
                                    imgbill3.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill3.Visible = false;
                                    imgbill3.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert3.Visible = common.myBool(str2[15]);
                                imgMedicalAlert3.Visible = common.myBool(str2[16]);
                                #endregion


                                #region MLC Patient
                                if (str2[17].Contains("True"))
                                {
                                    lblMLC3.Visible = true;
                                    lblMLC3.Text = "MLC";
                                    lblMLC3.Font.Bold = true;
                                    lblMLC3.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory3.Visible = common.myBool(str2[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "','" + str2[9] + "','" + hdnIsCritical3.ClientID + "','" + hdnUnperformedServices3.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;

                    }
                    if (str3.Length > 1)
                    {
                        lblEnc4.Text = str3[3];
                        lblPatientName4.Text = str3[7];
                        lblCompanytype4.Text = str3[6];
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);


                                HiddenField hdnIsCritical4 = (HiddenField)e.Row.FindControl("hdnIsCritical4");
                                Label lblCriticalIndication4 = (Label)e.Row.FindControl("lblCriticalIndication4");
                                if (str3[12].Contains("1"))
                                {
                                    lblCriticalIndication4.Visible = true;
                                    lblCriticalIndication4.ToolTip = "Test results with panic values.";
                                    hdnIsCritical4.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication4.Visible = false;
                                    lblCriticalIndication4.ToolTip = string.Empty;
                                    hdnIsCritical4.Value = "0";
                                }

                                HiddenField hdnUnperformedServices4 = (HiddenField)e.Row.FindControl("hdnUnperformedServices4");
                                Label lblUnperformedServices4 = (Label)e.Row.FindControl("lblUnperformedServices4");

                                if (str3[13].Contains("1"))
                                {
                                    lblUnperformedServices4.Visible = true;
                                    lblUnperformedServices4.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices4.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices4.Visible = false;
                                    lblUnperformedServices4.ToolTip = string.Empty;
                                    hdnUnperformedServices4.Value = "0";
                                }


                                #region forOTBillCleareancecou9Image4
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str3[14].Contains("1"))
                                {
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill4.Visible = true;
                                    imgbill4.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill4.Visible = false;
                                    imgbill4.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert4.Visible = common.myBool(str3[15]);
                                imgMedicalAlert4.Visible = common.myBool(str3[16]);
                                #endregion


                                #region MLC Patient
                                if (str3[17].Contains("True"))
                                {
                                    lblMLC4.Visible = true;
                                    lblMLC4.Text = "MLC";
                                    lblMLC4.Font.Bold = true;
                                    lblMLC4.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory4.Visible = common.myBool(str3[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "','" + str3[9] + "','" + hdnIsCritical4.ClientID + "','" + hdnUnperformedServices4.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;

                    }
                    if (str4.Length > 1)
                    {
                        lblEnc5.Text = str4[3];
                        lblPatientName5.Text = str4[7];
                        lblCompanytype5.Text = str4[6];
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];

                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);


                                HiddenField hdnIsCritical5 = (HiddenField)e.Row.FindControl("hdnIsCritical5");
                                Label lblCriticalIndication5 = (Label)e.Row.FindControl("lblCriticalIndication5");
                                if (str4[12].Contains("1"))
                                {
                                    lblCriticalIndication5.Visible = true;
                                    lblCriticalIndication5.ToolTip = "Test results with panic values.";
                                    hdnIsCritical5.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication5.Visible = false;
                                    lblCriticalIndication5.ToolTip = string.Empty;
                                    hdnIsCritical5.Value = "0";
                                }

                                HiddenField hdnUnperformedServices5 = (HiddenField)e.Row.FindControl("hdnUnperformedServices5");
                                Label lblUnperformedServices5 = (Label)e.Row.FindControl("lblUnperformedServices5");

                                if (str4[13].Contains("1"))
                                {
                                    lblUnperformedServices5.Visible = true;
                                    lblUnperformedServices5.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices5.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices5.Visible = false;
                                    lblUnperformedServices5.ToolTip = string.Empty;
                                    hdnUnperformedServices5.Value = "0";
                                }

                                #region forOTBillCleareancecou9Image5
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str4[14].Contains("1"))
                                {
                                    imgbill5.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill5.Visible = true;
                                    imgbill5.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill5.Visible = false;
                                    imgbill5.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert5.Visible = common.myBool(str4[15]);
                                imgMedicalAlert5.Visible = common.myBool(str4[16]);
                                #endregion

                                #region MLC Patient
                                if (str4[17].Contains("True"))
                                {
                                    lblMLC5.Visible = true;
                                    lblMLC5.Text = "MLC";
                                    lblMLC5.Font.Bold = true;
                                    lblMLC5.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory5.Visible = common.myBool(str4[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "','" + str4[9] + "','" + hdnIsCritical5.ClientID + "','" + hdnUnperformedServices5.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;

                    }
                    if (str5.Length > 1)
                    {
                        lblEnc6.Text = str5[3];
                        lblPatientName6.Text = str5[7];
                        lblCompanytype6.Text = str5[6];
                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);

                                HiddenField hdnIsCritical6 = (HiddenField)e.Row.FindControl("hdnIsCritical6");
                                Label lblCriticalIndication6 = (Label)e.Row.FindControl("lblCriticalIndication6");
                                if (str5[12].Contains("1"))
                                {
                                    lblCriticalIndication6.Visible = true;
                                    lblCriticalIndication6.ToolTip = "Test results with panic values.";
                                    hdnIsCritical6.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication6.Visible = false;
                                    lblCriticalIndication6.ToolTip = string.Empty;
                                    hdnIsCritical6.Value = "0";
                                }

                                HiddenField hdnUnperformedServices6 = (HiddenField)e.Row.FindControl("hdnUnperformedServices6");
                                Label lblUnperformedServices6 = (Label)e.Row.FindControl("lblUnperformedServices6");

                                if (str5[13].Contains("1"))
                                {
                                    lblUnperformedServices6.Visible = true;
                                    lblUnperformedServices6.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices6.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices6.Visible = false;
                                    lblUnperformedServices6.ToolTip = string.Empty;
                                    hdnUnperformedServices6.Value = "0";
                                }

                                #region forOTBillCleareancecou9Image6
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str5[14].Contains("1"))
                                {
                                    imgbill6.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill6.Visible = true;
                                    imgbill6.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill6.Visible = false;
                                    imgbill6.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert6.Visible = common.myBool(str5[15]);
                                imgMedicalAlert6.Visible = common.myBool(str5[16]);
                                #endregion


                                #region MLC Patient
                                if (str5[17].Contains("True"))
                                {
                                    lblMLC6.Visible = true;
                                    lblMLC6.Text = "MLC";
                                    lblMLC6.Font.Bold = true;
                                    lblMLC6.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory6.Visible = common.myBool(str5[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "','" + str5[9] + "','" + hdnIsCritical6.ClientID + "','" + hdnUnperformedServices6.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton6.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                        div.Visible = false;

                    }
                    if (str6.Length > 1)
                    {
                        lblEnc7.Text = str6[3];
                        lblPatientName7.Text = str6[7];
                        lblCompanytype7.Text = str6[6];
                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "R")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "H")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "I")
                            {
                                //imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton7.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                                div.Attributes.Add("style", "background-color:" + str6[8] + ";");
                            }
                            else if (str6[1] == "B")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str6[1] == "U")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno7.Value = str6[2];
                                hdnencounterno7.Value = str6[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                                div.Attributes.Add("style", "background-color:" + str6[8] + ";");
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);


                                HiddenField hdnIsCritical7 = (HiddenField)e.Row.FindControl("hdnIsCritical7");
                                Label lblCriticalIndication7 = (Label)e.Row.FindControl("lblCriticalIndication7");
                                if (str6[12].Contains("1"))
                                {
                                    lblCriticalIndication7.Visible = true;
                                    lblCriticalIndication7.ToolTip = "Test results with panic values.";
                                    hdnIsCritical7.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication7.Visible = false;
                                    lblCriticalIndication7.ToolTip = string.Empty;
                                    hdnIsCritical7.Value = "0";
                                }

                                HiddenField hdnUnperformedServices7 = (HiddenField)e.Row.FindControl("hdnUnperformedServices7");
                                Label lblUnperformedServices7 = (Label)e.Row.FindControl("lblUnperformedServices7");

                                if (str6[13].Contains("1"))
                                {
                                    lblUnperformedServices7.Visible = true;
                                    lblUnperformedServices7.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices7.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices7.Visible = false;
                                    lblUnperformedServices7.ToolTip = string.Empty;
                                    hdnUnperformedServices7.Value = "0";
                                }


                                #region forOTBillCleareancecou9Image7
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str6[14].Contains("1"))
                                {
                                    imgbill7.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill7.Visible = true;
                                    imgbill7.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill7.Visible = false;
                                    imgbill7.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert7.Visible = common.myBool(str6[15]);
                                imgMedicalAlert7.Visible = common.myBool(str6[16]);
                                #endregion

                                #region MLC Patient
                                if (str6[17].Contains("True"))
                                {
                                    lblMLC7.Visible = true;
                                    lblMLC7.Text = "MLC";
                                    lblMLC7.Font.Bold = true;
                                    lblMLC7.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory7.Visible = common.myBool(str6[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[2] + "','" + str6[3] + "','" + str6[9] + "','" + hdnIsCritical7.ClientID + "','" + hdnUnperformedServices7.ClientID + "');");

                            }
                        }

                    }
                    else
                    {
                        imgbutton7.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                        div.Visible = false;

                    }
                    if (str7.Length > 1)
                    {
                        lblEnc8.Text = str7[3];
                        lblPatientName8.Text = str7[7];
                        lblCompanytype8.Text = str7[6];
                        if (label8.Text != "")
                        {
                            if (str7[1] == "V")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "R")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "H")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "I")
                            {
                                //imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton8.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div9");
                                div.Attributes.Add("style", "background-color:" + str7[8] + ";");
                            }
                            else if (str7[1] == "B")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str7[1] == "U")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno8.Value = str7[2];
                                hdnencounterno8.Value = str7[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div8");
                                div.Attributes.Add("style", "background-color:" + str7[8] + ";");
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);


                                HiddenField hdnIsCritical8 = (HiddenField)e.Row.FindControl("hdnIsCritical8");
                                Label lblCriticalIndication8 = (Label)e.Row.FindControl("lblCriticalIndication8");
                                if (str7[12].Contains("1"))
                                {
                                    lblCriticalIndication8.Visible = true;
                                    lblCriticalIndication8.ToolTip = "Test results with panic values.";
                                    hdnIsCritical8.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication8.Visible = false;
                                    lblCriticalIndication8.ToolTip = string.Empty;
                                    hdnIsCritical8.Value = "0";
                                }

                                HiddenField hdnUnperformedServices8 = (HiddenField)e.Row.FindControl("hdnUnperformedServices8");
                                Label lblUnperformedServices8 = (Label)e.Row.FindControl("lblUnperformedServices8");


                                if (str7[13].Contains("1"))
                                {
                                    lblUnperformedServices8.Visible = true;
                                    lblUnperformedServices8.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices8.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices8.Visible = false;
                                    lblUnperformedServices8.ToolTip = string.Empty;
                                    hdnUnperformedServices8.Value = "0";
                                }


                                #region forOTBillCleareancecou9Image8
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str7[14].Contains("1"))
                                {
                                    imgbill8.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill8.Visible = true;
                                    imgbill8.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill8.Visible = false;
                                    imgbill8.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert8.Visible = common.myBool(str7[15]);
                                imgMedicalAlert8.Visible = common.myBool(str7[16]);
                                #endregion


                                #region MLC Patient
                                if (str7[17].Contains("True"))
                                {
                                    lblMLC8.Visible = true;
                                    lblMLC8.Text = "MLC";
                                    lblMLC8.Font.Bold = true;
                                    lblMLC8.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory8.Visible = common.myBool(str7[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[2] + "','" + str7[3] + "','" + str7[9] + "','" + hdnIsCritical8.ClientID + "','" + hdnUnperformedServices8.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton8.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div8");
                        div.Visible = false;

                    }
                    if (str8.Length > 1)
                    {
                        lblEnc9.Text = str8[3];
                        lblPatientName9.Text = str8[7];
                        lblCompanytype9.Text = str8[6];
                        if (label9.Text != "")
                        {
                            if (str8[1] == "V")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "R")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "H")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "I")
                            {
                                //imgbutton9.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton9.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div9");
                                div.Attributes.Add("style", "background-color:" + str8[8] + ";");
                            }
                            else if (str8[1] == "B")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str8[1] == "U")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno9.Value = str8[2];
                                hdnencounterno9.Value = str8[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div9");
                                div.Attributes.Add("style", "background-color:" + str8[8] + ";");
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);


                                HiddenField hdnIsCritical9 = (HiddenField)e.Row.FindControl("hdnIsCritical9");
                                Label lblCriticalIndication9 = (Label)e.Row.FindControl("lblCriticalIndication9");
                                if (str8[12].Contains("1"))
                                {
                                    lblCriticalIndication9.Visible = true;
                                    lblCriticalIndication9.ToolTip = "Test results with panic values.";
                                    hdnIsCritical9.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication9.Visible = false;
                                    lblCriticalIndication9.ToolTip = string.Empty;
                                    hdnIsCritical9.Value = "0";
                                }

                                HiddenField hdnUnperformedServices9 = (HiddenField)e.Row.FindControl("hdnUnperformedServices9");
                                Label lblUnperformedServices9 = (Label)e.Row.FindControl("lblUnperformedServices9");


                                if (str8[13].Contains("1"))
                                {
                                    lblUnperformedServices9.Visible = true;
                                    lblUnperformedServices9.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices9.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices9.Visible = false;
                                    lblUnperformedServices9.ToolTip = string.Empty;
                                    hdnUnperformedServices9.Value = "0";
                                }


                                #region forOTBillCleareancecou9Image9
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str8[14].Contains("1"))
                                {
                                    imgbill9.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill9.Visible = true;
                                    imgbill9.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill9.Visible = false;
                                    imgbill9.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert9.Visible = common.myBool(str8[15]);
                                imgMedicalAlert9.Visible = common.myBool(str8[16]);
                                #endregion

                                #region MLC Patient
                                if (str8[17].Contains("True"))
                                {
                                    lblMLC9.Visible = true;
                                    lblMLC9.Text = "MLC";
                                    lblMLC9.Font.Bold = true;
                                    lblMLC9.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory9.Visible = common.myBool(str8[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[2] + "','" + str8[3] + "','" + str8[9] + "','" + hdnIsCritical9.ClientID + "','" + hdnUnperformedServices9.ClientID + "');");

                            }
                        }
                    }
                    else
                    {
                        imgbutton9.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div9");
                        div.Visible = false;
                    }

                    HtmlControl div10 = (HtmlControl)e.Row.FindControl("div10");
                    div10.Visible = false;
                }
                if (noOfColumn == 10)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');
                    string[] str5 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "5")).Split('@');
                    string[] str6 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "6")).Split('@');
                    string[] str7 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "7")).Split('@');
                    string[] str8 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "8")).Split('@');
                    string[] str9 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "9")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];
                    label6.Text = str5[0];
                    label7.Text = str6[0];
                    label8.Text = str7[0];
                    label9.Text = str8[0];
                    label10.Text = str9[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            lblEnc1.Text = str[3];
                            lblPatientName.Text = str[7];
                            lblCompanytype.Text = str[6];
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str9[8] + ";");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                                div.Attributes.Add("style", "background-color:" + str[8] + ";");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                                HiddenField hdnIsCritical1 = (HiddenField)e.Row.FindControl("hdnIsCritical1");
                                Label lblCriticalIndication1 = (Label)e.Row.FindControl("lblCriticalIndication1");
                                if (str[12].Contains("1"))
                                {
                                    lblCriticalIndication1.Visible = true;
                                    lblCriticalIndication1.ToolTip = "Test results with panic values.";
                                    hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication1.Visible = false;
                                    lblCriticalIndication1.ToolTip = string.Empty;
                                    hdnIsCritical1.Value = "0";
                                }

                                HiddenField hdnUnperformedServices1 = (HiddenField)e.Row.FindControl("hdnUnperformedServices1");
                                Label lblUnperformedServices1 = (Label)e.Row.FindControl("lblUnperformedServices1");

                                if (str[13].Contains("1"))
                                {
                                    lblUnperformedServices1.Visible = true;
                                    lblUnperformedServices1.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices1.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices1.Visible = false;
                                    lblUnperformedServices1.ToolTip = string.Empty;
                                    hdnUnperformedServices1.Value = "0";
                                }

                                #region forOTBillCleareancecou10Image1
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str[14].Contains("1"))
                                {
                                    imgbill.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill.Visible = true;
                                    imgbill.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill.Visible = false;
                                    imgbill.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert1.Visible = common.myBool(str[15]);
                                imgMedicalAlert1.Visible = common.myBool(str[16]);
                                #endregion


                                #region MLC Patient
                                if (str[17].Contains("True"))
                                {
                                    lblMLC1.Visible = true;
                                    lblMLC1.Text = "MLC";
                                    lblMLC1.Font.Bold = true;
                                    lblMLC1.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory1.Visible = common.myBool(str[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "','" + str[9] + "','" + hdnIsCritical1.ClientID + "','" + hdnUnperformedServices1.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;
                        //e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");

                        e.Row.Cells[0].ColumnSpan = 9;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div1 = (HtmlControl)e.Row.FindControl("div1");
                        div1.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            lblEnc2.Text = str1[3];
                            lblPatientName2.Text = str1[7];
                            lblCompanytype2.Text = str1[6];
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                                div.Attributes.Add("style", "background-color:" + str1[8] + ";");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);


                                HiddenField hdnIsCritical2 = (HiddenField)e.Row.FindControl("hdnIsCritical2");
                                Label lblCriticalIndication2 = (Label)e.Row.FindControl("lblCriticalIndication2");
                                if (str1[12].Contains("1"))
                                {
                                    lblCriticalIndication2.Visible = true;
                                    lblCriticalIndication2.ToolTip = "Test results with panic values.";
                                    hdnIsCritical2.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication2.Visible = false;
                                    lblCriticalIndication2.ToolTip = string.Empty;
                                    hdnIsCritical2.Value = "0";
                                }


                                HiddenField hdnUnperformedServices2 = (HiddenField)e.Row.FindControl("hdnUnperformedServices2");
                                Label lblUnperformedServices2 = (Label)e.Row.FindControl("lblUnperformedServices2");

                                if (str1[13].Contains("1"))
                                {
                                    lblUnperformedServices2.Visible = true;
                                    lblUnperformedServices2.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices2.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices2.Visible = false;
                                    lblUnperformedServices2.ToolTip = string.Empty;
                                    hdnUnperformedServices2.Value = "0";
                                }

                                #region forOTBillCleareancecou10Image2
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str1[14].Contains("1"))
                                {
                                    imgbill2.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill2.Visible = true;
                                    imgbill2.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill2.Visible = false;
                                    imgbill2.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert2.Visible = common.myBool(str1[15]);
                                imgMedicalAlert2.Visible = common.myBool(str1[16]);
                                #endregion

                                #region MLC Patient
                                if (str1[17].Contains("True"))
                                {
                                    lblMLC2.Visible = true;
                                    lblMLC2.Text = "MLC";
                                    lblMLC2.Font.Bold = true;
                                    lblMLC2.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory2.Visible = common.myBool(str1[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "','" + str1[9] + "','" + hdnIsCritical2.ClientID + "','" + hdnUnperformedServices2.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;

                    }
                    if (str2.Length > 1)
                    {
                        lblEnc3.Text = str2[3];
                        lblPatientName3.Text = str2[7];
                        lblCompanytype3.Text = str2[6];
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                                div.Attributes.Add("style", "background-color:" + str2[8] + ";");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);


                                HiddenField hdnIsCritical3 = (HiddenField)e.Row.FindControl("hdnIsCritical3");
                                Label lblCriticalIndication3 = (Label)e.Row.FindControl("lblCriticalIndication3");
                                if (str2[12].Contains("1"))
                                {
                                    lblCriticalIndication3.Visible = true;
                                    lblCriticalIndication3.ToolTip = "Test results with panic values.";
                                    hdnIsCritical3.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication3.Visible = false;
                                    lblCriticalIndication3.ToolTip = string.Empty;
                                    hdnIsCritical3.Value = "0";
                                }


                                HiddenField hdnUnperformedServices3 = (HiddenField)e.Row.FindControl("hdnUnperformedServices3");
                                Label lblUnperformedServices3 = (Label)e.Row.FindControl("lblUnperformedServices3");

                                if (str2[13].Contains("1"))
                                {
                                    lblUnperformedServices3.Visible = true;
                                    lblUnperformedServices3.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices3.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices3.Visible = false;
                                    lblUnperformedServices3.ToolTip = string.Empty;
                                    hdnUnperformedServices3.Value = "0";
                                }

                                #region forOTBillCleareancecou10Image3
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str2[14].Contains("1"))
                                {
                                    imgbill3.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill3.Visible = true;
                                    imgbill3.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill3.Visible = false;
                                    imgbill3.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert3.Visible = common.myBool(str2[15]);
                                imgMedicalAlert3.Visible = common.myBool(str2[16]);
                                #endregion


                                #region MLC Patient
                                if (str2[17].Contains("True"))
                                {
                                    lblMLC3.Visible = true;
                                    lblMLC3.Text = "MLC";
                                    lblMLC3.Font.Bold = true;
                                    lblMLC3.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory3.Visible = common.myBool(str2[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "','" + str2[9] + "','" + hdnIsCritical3.ClientID + "','" + hdnUnperformedServices3.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;

                    }
                    if (str3.Length > 1)
                    {
                        lblEnc4.Text = str3[3];
                        lblPatientName4.Text = str3[7];
                        lblCompanytype4.Text = str3[6];
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");

                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                                div.Attributes.Add("style", "background-color:" + str3[8] + ";");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);


                                HiddenField hdnIsCritical4 = (HiddenField)e.Row.FindControl("hdnIsCritical4");
                                Label lblCriticalIndication4 = (Label)e.Row.FindControl("lblCriticalIndication4");
                                if (str3[12].Contains("1"))
                                {
                                    lblCriticalIndication4.Visible = true;
                                    lblCriticalIndication4.ToolTip = "Test results with panic values.";
                                    hdnIsCritical4.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication4.Visible = false;
                                    lblCriticalIndication4.ToolTip = string.Empty;
                                    hdnIsCritical4.Value = "0";
                                }


                                HiddenField hdnUnperformedServices4 = (HiddenField)e.Row.FindControl("hdnUnperformedServices4");
                                Label lblUnperformedServices4 = (Label)e.Row.FindControl("lblUnperformedServices4");

                                if (str3[13].Contains("1"))
                                {
                                    lblUnperformedServices4.Visible = true;
                                    lblUnperformedServices4.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices4.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices4.Visible = false;
                                    lblUnperformedServices4.ToolTip = string.Empty;
                                    hdnUnperformedServices4.Value = "0";
                                }


                                #region forOTBillCleareancecou10Image4
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str3[14].Contains("1"))
                                {
                                    imgbill4.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill4.Visible = true;
                                    imgbill4.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill4.Visible = false;
                                    imgbill4.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert4.Visible = common.myBool(str3[15]);
                                imgMedicalAlert4.Visible = common.myBool(str3[16]);
                                #endregion

                                #region MLC Patient
                                if (str3[17].Contains("True"))
                                {
                                    lblMLC4.Visible = true;
                                    lblMLC4.Text = "MLC";
                                    lblMLC4.Font.Bold = true;
                                    lblMLC4.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory4.Visible = common.myBool(str3[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "','" + str3[9] + "','" + hdnIsCritical4.ClientID + "','" + hdnUnperformedServices4.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;

                    }
                    if (str4.Length > 1)
                    {
                        lblEnc5.Text = str4[3];
                        lblPatientName5.Text = str4[7];
                        lblCompanytype5.Text = str4[6];
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                                div.Attributes.Add("style", "background-color:" + str4[8] + ";");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);


                                HiddenField hdnIsCritical5 = (HiddenField)e.Row.FindControl("hdnIsCritical5");
                                Label lblCriticalIndication5 = (Label)e.Row.FindControl("lblCriticalIndication5");
                                if (str4[12].Contains("1"))
                                {
                                    lblCriticalIndication5.Visible = true;
                                    lblCriticalIndication5.ToolTip = "Test results with panic values.";
                                    hdnIsCritical5.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication5.Visible = false;
                                    lblCriticalIndication5.ToolTip = string.Empty;
                                    hdnIsCritical5.Value = "0";
                                }


                                HiddenField hdnUnperformedServices5 = (HiddenField)e.Row.FindControl("hdnUnperformedServices5");
                                Label lblUnperformedServices5 = (Label)e.Row.FindControl("lblUnperformedServices5");

                                if (str4[13].Contains("1"))
                                {
                                    lblUnperformedServices5.Visible = true;
                                    lblUnperformedServices5.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices5.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices5.Visible = false;
                                    lblUnperformedServices5.ToolTip = string.Empty;
                                    hdnUnperformedServices5.Value = "0";
                                }


                                #region forOTBillCleareancecou10Image5
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str4[14].Contains("1"))
                                {
                                    imgbill5.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill5.Visible = true;
                                    imgbill5.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill5.Visible = false;
                                    imgbill5.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert5.Visible = common.myBool(str4[15]);
                                imgMedicalAlert5.Visible = common.myBool(str4[16]);
                                #endregion


                                #region MLC Patient
                                if (str4[17].Contains("True"))
                                {
                                    lblMLC5.Visible = true;
                                    lblMLC5.Text = "MLC";
                                    lblMLC5.Font.Bold = true;
                                    lblMLC5.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory5.Visible = common.myBool(str4[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "','" + str4[9] + "','" + hdnIsCritical5.ClientID + "','" + hdnUnperformedServices5.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;

                    }
                    if (str5.Length > 1)
                    {
                        lblEnc6.Text = str5[3];
                        lblPatientName6.Text = str5[7];
                        lblCompanytype6.Text = str5[6];
                        if (label6.Text != "")
                        {
                            lblPatientName6.Text = str5[7];
                            lblCompanytype6.Text = str5[6];
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                                div.Attributes.Add("style", "background-color:" + str5[8] + ";");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);

                                HiddenField hdnIsCritical6 = (HiddenField)e.Row.FindControl("hdnIsCritical6");
                                Label lblCriticalIndication6 = (Label)e.Row.FindControl("lblCriticalIndication6");
                                if (str5[12].Contains("1"))
                                {
                                    lblCriticalIndication6.Visible = true;
                                    lblCriticalIndication6.ToolTip = "Test results with panic values.";
                                    hdnIsCritical6.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication6.Visible = false;
                                    lblCriticalIndication6.ToolTip = string.Empty;
                                    hdnIsCritical6.Value = "0";
                                }

                                HiddenField hdnUnperformedServices6 = (HiddenField)e.Row.FindControl("hdnUnperformedServices6");
                                Label lblUnperformedServices6 = (Label)e.Row.FindControl("lblUnperformedServices6");

                                if (str5[13].Contains("1"))
                                {
                                    lblUnperformedServices6.Visible = true;
                                    lblUnperformedServices6.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices6.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices6.Visible = false;
                                    lblUnperformedServices6.ToolTip = string.Empty;
                                    hdnUnperformedServices6.Value = "0";
                                }

                                #region forOTBillCleareancecou10Image6
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str5[14].Contains("1"))
                                {
                                    imgbill6.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill6.Visible = true;
                                    imgbill6.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill6.Visible = false;
                                    imgbill6.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert6.Visible = common.myBool(str5[15]);
                                imgMedicalAlert6.Visible = common.myBool(str5[16]);
                                #endregion


                                #region MLC Patient
                                if (str5[17].Contains("True"))
                                {
                                    lblMLC6.Visible = true;
                                    lblMLC6.Text = "MLC";
                                    lblMLC6.Font.Bold = true;
                                    lblMLC6.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory6.Visible = common.myBool(str5[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "','" + str5[9] + "','" + hdnIsCritical6.ClientID + "','" + hdnUnperformedServices6.ClientID + "');");
                            }
                        }


                    }
                    else
                    {
                        imgbutton6.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                        div.Visible = false;

                    }
                    if (str6.Length > 1)
                    {
                        lblEnc7.Text = str6[3];
                        lblPatientName7.Text = str6[7];
                        lblCompanytype7.Text = str6[6];
                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "R")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "H")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                            }
                            else if (str6[1] == "I")
                            {
                                //imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton7.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                                div.Attributes.Add("style", "background-color:" + str6[8] + ";");
                            }
                            else if (str6[1] == "B")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str6[1] == "U")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno7.Value = str6[2];
                                hdnencounterno7.Value = str6[3];

                                HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                                div.Attributes.Add("style", "background-color:" + str6[8] + ";");
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);


                                HiddenField hdnIsCritical7 = (HiddenField)e.Row.FindControl("hdnIsCritical7");
                                Label lblCriticalIndication7 = (Label)e.Row.FindControl("lblCriticalIndication7");
                                if (str6[12].Contains("1"))
                                {
                                    lblCriticalIndication7.Visible = true;
                                    lblCriticalIndication7.ToolTip = "Test results with panic values.";
                                    hdnIsCritical7.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication7.Visible = false;
                                    lblCriticalIndication7.ToolTip = string.Empty;
                                    hdnIsCritical7.Value = "0";
                                }

                                HiddenField hdnUnperformedServices7 = (HiddenField)e.Row.FindControl("hdnUnperformedServices7");
                                Label lblUnperformedServices7 = (Label)e.Row.FindControl("lblUnperformedServices7");
                                if (str6[13].Contains("1"))
                                {
                                    lblUnperformedServices7.Visible = true;
                                    lblUnperformedServices7.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices7.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices7.Visible = false;
                                    lblUnperformedServices7.ToolTip = string.Empty;
                                    hdnUnperformedServices7.Value = "0";
                                }


                                #region forOTBillCleareancecou10Image7
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str6[14].Contains("1"))
                                {
                                    imgbill7.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill7.Visible = true;
                                    imgbill7.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill7.Visible = false;
                                    imgbill7.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert7.Visible = common.myBool(str6[15]);
                                imgMedicalAlert7.Visible = common.myBool(str6[16]);
                                #endregion

                                #region MLC Patient
                                if (str6[17].Contains("True"))
                                {
                                    lblMLC7.Visible = true;
                                    lblMLC7.Text = "MLC";
                                    lblMLC7.Font.Bold = true;
                                    lblMLC7.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory7.Visible = common.myBool(str6[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[2] + "','" + str6[3] + "','" + str6[9] + "','" + hdnIsCritical7.ClientID + "','" + hdnUnperformedServices7.ClientID + "');");

                            }
                        }

                    }
                    else
                    {
                        imgbutton7.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                        div.Visible = false;

                    }
                    if (str7.Length > 1)
                    {
                        lblEnc8.Text = str7[3];
                        lblPatientName8.Text = str7[7];
                        lblCompanytype8.Text = str7[6];
                        if (label8.Text != "")
                        {
                            if (str7[1] == "V")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //  imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "R")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "H")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //  imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "I")
                            {
                                //imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton8.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div8");
                                div.Attributes.Add("style", "background-color:" + str7[8] + ";");
                            }
                            else if (str7[1] == "B")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str7[1] == "U")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno8.Value = str7[2];
                                hdnencounterno8.Value = str7[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div8");
                                div.Attributes.Add("style", "background-color:" + str7[8] + ";");
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);


                                HiddenField hdnIsCritical8 = (HiddenField)e.Row.FindControl("hdnIsCritical8");
                                Label lblCriticalIndication8 = (Label)e.Row.FindControl("lblCriticalIndication8");
                                if (str7[12].Contains("1"))
                                {
                                    lblCriticalIndication8.Visible = true;
                                    lblCriticalIndication8.ToolTip = "Test results with panic values.";
                                    hdnIsCritical8.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication8.Visible = false;
                                    lblCriticalIndication8.ToolTip = string.Empty;
                                    hdnIsCritical8.Value = "0";
                                }

                                HiddenField hdnUnperformedServices8 = (HiddenField)e.Row.FindControl("hdnUnperformedServices8");
                                Label lblUnperformedServices8 = (Label)e.Row.FindControl("lblUnperformedServices8");
                                if (str7[13].Contains("1"))
                                {
                                    lblUnperformedServices8.Visible = true;
                                    lblUnperformedServices8.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices8.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices8.Visible = false;
                                    lblUnperformedServices8.ToolTip = string.Empty;
                                    hdnUnperformedServices8.Value = "0";
                                }

                                #region forOTBillCleareancecou10Image8
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str7[14].Contains("1"))
                                {
                                    imgbill8.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill8.Visible = true;
                                    imgbill8.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill8.Visible = false;
                                    imgbill8.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert8.Visible = common.myBool(str7[15]);
                                imgMedicalAlert8.Visible = common.myBool(str7[16]);
                                #endregion


                                #region MLC Patient
                                if (str7[17].Contains("True"))
                                {
                                    lblMLC8.Visible = true;
                                    lblMLC8.Text = "MLC";
                                    lblMLC8.Font.Bold = true;
                                    lblMLC8.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory8.Visible = common.myBool(str7[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[2] + "','" + str7[3] + "','" + str7[9] + "','" + hdnIsCritical8.ClientID + "','" + hdnUnperformedServices8.ClientID + "');");
                            }
                        }

                    }
                    else
                    {
                        imgbutton8.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div8");
                        div.Visible = false;

                    }
                    if (str8.Length > 1)
                    {
                        lblEnc9.Text = str8[3];
                        lblPatientName9.Text = str8[7];
                        lblCompanytype9.Text = str8[6];
                        if (label9.Text != "")
                        {
                            if (str8[1] == "V")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                //imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "R")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                // imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "H")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                // imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "I")
                            {
                                //imgbutton9.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton9.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div9");
                                div.Attributes.Add("style", "background-color:" + str8[8] + ";");
                            }
                            else if (str8[1] == "B")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str8[1] == "U")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno9.Value = str8[2];
                                hdnencounterno9.Value = str8[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div9");
                                div.Attributes.Add("style", "background-color:" + str8[8] + ";");
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);


                                HiddenField hdnIsCritical9 = (HiddenField)e.Row.FindControl("hdnIsCritical9");
                                Label lblCriticalIndication9 = (Label)e.Row.FindControl("lblCriticalIndication9");
                                if (str8[12].Contains("1"))
                                {
                                    lblCriticalIndication9.Visible = true;
                                    lblCriticalIndication9.ToolTip = "Test results with panic values.";
                                    hdnIsCritical9.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication9.Visible = false;
                                    lblCriticalIndication9.ToolTip = string.Empty;
                                    hdnIsCritical9.Value = "0";
                                }

                                HiddenField hdnUnperformedServices9 = (HiddenField)e.Row.FindControl("hdnUnperformedServices9");
                                Label lblUnperformedServices9 = (Label)e.Row.FindControl("lblUnperformedServices9");
                                if (str8[13].Contains("1"))
                                {
                                    lblUnperformedServices9.Visible = true;
                                    lblUnperformedServices9.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices9.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices9.Visible = false;
                                    lblUnperformedServices9.ToolTip = string.Empty;
                                    hdnUnperformedServices9.Value = "0";
                                }

                                #region forOTBillCleareancecou10Image9
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str8[14].Contains("1"))
                                {
                                    imgbill9.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill9.Visible = true;
                                    imgbill9.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill9.Visible = false;
                                    imgbill9.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert9.Visible = common.myBool(str8[15]);
                                imgMedicalAlert9.Visible = common.myBool(str8[16]);
                                #endregion


                                #region MLC Patient
                                if (str8[17].Contains("True"))
                                {
                                    lblMLC9.Visible = true;
                                    lblMLC9.Text = "MLC";
                                    lblMLC9.Font.Bold = true;
                                    lblMLC9.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory9.Visible = common.myBool(str8[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[2] + "','" + str8[3] + "','" + str8[9] + "','" + hdnIsCritical9.ClientID + "','" + hdnUnperformedServices9.ClientID + "');");

                            }
                        }
                    }
                    else
                    {
                        imgbutton9.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div9");
                        div.Visible = false;
                    }
                    if (str9.Length > 1)
                    {
                        lblEnc10.Text = str9[3];
                        lblPatientName10.Text = str9[7];
                        lblCompanytype10.Text = str9[6];
                        if (label10.Text != "")
                        {
                            if (str9[1] == "V")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                // imgbutton10.Attributes.Add("OnClick", "showMenu(event,'" + str9[4] + "','" + str9[5] + "','" + str9[0] + "','" + str9[1] + "','" + str9[2] + "','" + str9[3] + "');");
                            }
                            else if (str9[1] == "R")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                //imgbutton10.Attributes.Add("OnClick", "showMenu(event,'" + str9[4] + "','" + str9[5] + "','" + str9[0] + "','" + str9[1] + "','" + str9[2] + "','" + str9[3] + "');");
                            }
                            else if (str9[1] == "H")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                //imgbutton10.Attributes.Add("OnClick", "showMenu(event,'" + str9[4] + "','" + str9[5] + "','" + str9[0] + "','" + str9[1] + "','" + str9[2] + "','" + str9[3] + "');");
                            }
                            else if (str9[1] == "I")
                            {
                                //imgbutton10.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton10.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton10.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str9[2] + " , Name " + str9[7] + ", Ward Name : " + str9[10] + ", BedCategory Name : " + str9[11]);
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div10");
                                div.Attributes.Add("style", "background-color:" + str9[8] + ";");
                            }
                            else if (str9[1] == "B")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/bedonly.JPG";
                            }
                            else if (str9[1] == "U")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            else
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/bed3.jpg";

                                hdnregno10.Value = str9[2];
                                hdnencounterno10.Value = str9[3];
                                HtmlControl div = (HtmlControl)e.Row.FindControl("div10");
                                div.Attributes.Add("style", "background-color:" + str9[8] + ";");
                                imgbutton10.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str9[2] + " , Name " + str9[7] + ", Ward Name : " + str9[10] + ", BedCategory Name : " + str9[11]);


                                HiddenField hdnIsCritical10 = (HiddenField)e.Row.FindControl("hdnIsCritical10");
                                Label lblCriticalIndication10 = (Label)e.Row.FindControl("lblCriticalIndication10");
                                if (str9[12].Contains("1"))
                                {
                                    lblCriticalIndication10.Visible = true;
                                    lblCriticalIndication10.ToolTip = "Test results with panic values.";
                                    hdnIsCritical10.Value = "1";
                                }
                                else
                                {
                                    lblCriticalIndication10.Visible = false;
                                    lblCriticalIndication10.ToolTip = string.Empty;
                                    hdnIsCritical10.Value = "0";
                                }


                                HiddenField hdnUnperformedServices10 = (HiddenField)e.Row.FindControl("hdnUnperformedServices10");
                                Label lblUnperformedServices10 = (Label)e.Row.FindControl("lblUnperformedServices10");
                                if (str9[13].Contains("1"))
                                {
                                    lblUnperformedServices10.Visible = true;
                                    lblUnperformedServices10.ToolTip = "Orders are pending.";
                                    hdnUnperformedServices10.Value = "1";
                                }
                                else
                                {
                                    lblUnperformedServices10.Visible = false;
                                    lblUnperformedServices10.ToolTip = string.Empty;
                                    hdnUnperformedServices10.Value = "0";
                                }

                                #region forOTBillCleareancecou10Image10
                                //Added on 02-09-2014 Start Naushad Ali
                                if (str9[14].Contains("1"))
                                {
                                    imgbill10.ImageUrl = "~/Images/PendingBill.png";
                                    imgbill10.Visible = true;
                                    imgbill10.ToolTip = "Bill Clearance for Surgery is Pending.";
                                    //hdnIsCritical1.Value = "1";
                                }
                                else
                                {
                                    imgbill10.Visible = false;
                                    imgbill10.Dispose();
                                }
                                //Added on 02-09-2014 End Naushad Ali 
                                #endregion

                                #region Allergy and Medical Alert
                                imgAllergyAlert10.Visible = common.myBool(str9[15]);
                                imgMedicalAlert10.Visible = common.myBool(str9[16]);
                                #endregion

                                #region MLC Patient
                                if (str9[17].Contains("True"))
                                {
                                    lblMLC10.Visible = true;
                                    lblMLC10.Font.Bold = true;
                                    lblMLC10.Text = "MLC";
                                    lblMLC10.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                #endregion

                                imgReffHistory10.Visible = common.myBool(str9[18]);

                                div.Attributes.Add("OnClick", "showMenu2(event,'" + str9[4] + "','" + str9[5] + "','" + str9[0] + "','" + str9[2] + "','" + str9[3] + "','" + str9[9] + "','" + hdnIsCritical10.ClientID + "','" + hdnUnperformedServices10.ClientID + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton10.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div10");
                        div.Visible = false;
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            //lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void grvBedStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        HiddenField hf = (HiddenField)grvBedStatus.SelectedRow.Cells[grvBedStatus.SelectedIndex].FindControl("hdnregno2");
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["Otherdet"]) == "adm")
        {
            Response.Redirect("AdmissionDetails.aspx?Regno=" + lblregno.Text.Trim() + "&Ipno=" + lblencno.Text.Trim(), false);
        }
        else
        {
            ViewState["dt"] = null;
            fillData(common.myInt(ddlColumns.SelectedValue));
        }
    }
    protected void RadContextMenu2_ItemClick(object sender, RadMenuEventArgs e)
    {
        DataSet dsStatus = new DataSet();
        BaseC.clsPharmacy status = new BaseC.clsPharmacy(sConString);
        BaseC.Patient patient = new BaseC.Patient(sConString);
        ds = new DataSet();
        try
        {

            ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                common.myStr(lblregno.Text), common.myStr(lblencno.Text), common.myInt(Session["UserId"]), 0);

            #region Patient Detail
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["OPIP"] = "I";
                    Session["EncounterId"] = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
                    Session["RegistrationID"] = common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]);
                    Session["EncounterDate"] = common.myStr(ds.Tables[0].Rows[0]["EncounterDate"]);
                    Session["DoctorID"] = common.myInt(ds.Tables[0].Rows[0]["DoctorId"].ToString());
                    Session["RegistrationNo"] = common.myInt(ds.Tables[0].Rows[0]["RegistrationNo"]);
                    Session["Regno"] = common.myStr(lblregno.Text);
                    Session["Encno"] = common.myStr(lblencno.Text).Trim();

                    string sRegNoTitle = common.myStr(Resources.PRegistration.regno);
                    string sDoctorTitle = common.myStr(Resources.PRegistration.Doctor);
                    string DateTitle = common.myStr(ds.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date : " : "Encounter Date : ";

                    Session["PatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(ds.Tables[0].Rows[0]["GenderAge"]) + "</span>"
                     + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
                     + "&nbsp;Enc #.:&nbsp;" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
                     + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
                     + DateTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(Session["EncounterDate"]) + "</span>"
                     + "&nbsp;Bed No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["BedNo"]) + "</span>"
                     + "&nbsp;Ward:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["WardName"]) + "</span>"
                     + "&nbsp;Mobile No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["MobileNo"]) + "</span>"
                     + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["Payername"]) + "</span>"
                     + "</b>";
                }
            }
            #endregion
            #region Allergy
            if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("AL"))
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    //Objstatus = new BaseC.ATD(sConString);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Response.Redirect("/EMR/Allergy/Allergy.aspx?Regno=" + common.myStr(lblregno.Text).Trim() + "&Encno=" + common.myStr(lblencno.Text).Trim(), false);
                        }
                    }
                }
            }
            #endregion
            #region Bed Transfer
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("BT"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "/ATD/BedTransfer1.aspx?Regno=" + common.myStr(lblregno.Text).Trim()
                        + "&encno=" + common.myStr(lblencno.Text).Trim() + "&bedno=" + common.myStr(lblbeno.Text).Trim() + "&BT=WM";
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "OnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                }

            }
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("BTW"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "/ATD/BedTransfer1.aspx?Regno=" + common.myStr(lblregno.Text).Trim() + "&encno=" + common.myStr(lblencno.Text).Trim() + "&bedno=" + common.myStr(lblbeno.Text).Trim() + "";
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "OnClientClose";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                }

            }
            #endregion
            #region Drug Order
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("DG"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedicationNew.aspx?POPUP=POPUP&Regid="
                                + common.myInt(ds.Tables[0].Rows[0]["RegistrationId"])
                                + "&RegNo=" + common.myStr(lblregno.Text)
                                + "&EncId=" + common.myInt(ds.Tables[0].Rows[0]["EncounterId"])
                                + "&EncNo=" + common.myStr(lblencno.Text)
                                + "&DoctorId=" + common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);


                            RadWindow1.Height = 620;
                            RadWindow1.Width = 1000;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region File Request
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("FR"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "~/MRD/MRDFileRequest.aspx?Regno=" + lblregno.Text.Trim() + "&Encno=" + lblencno.Text.Trim()
                                + "&RegId=" + common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]).Trim()
                                + "&EncId=" + common.myStr(ds.Tables[0].Rows[0]["EncounterId"]).Trim() + "&POPUP=POPUP"
                                + "&DoctorId=" + common.myStr(ds.Tables[0].Rows[0]["DoctorId"]) + "&RequestFrom=WARD";
                            RadWindow1.Height = 620;
                            RadWindow1.Width = 1000;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Non Drug order
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("NDO"))// Non Drug order
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (common.myStr(Session["EmployeeType"]).Equals("N"))
                            {
                                RadWindow1.NavigateUrl = "/ICM/AckNonDrugOrder.aspx?RegId=" + common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]).Trim()
                                    + "&POPUP=POPUP&Ward=Y";
                                RadWindow1.Height = 620;
                                RadWindow1.Width = 1000;
                                RadWindow1.Top = 10;
                                RadWindow1.Left = 10;

                                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                RadWindow1.Modal = true;
                                RadWindow1.VisibleStatusbar = false;
                                RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                            }
                            else
                            {
                                RadWindow1.NavigateUrl = "/ICM/ICMNONDrugOrder.aspx?RegId=" + common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]).Trim()
                                    + "&POPUP=POPUP&Ward=Y";
                                RadWindow1.Height = 620;
                                RadWindow1.Width = 1000;
                                RadWindow1.Top = 10;
                                RadWindow1.Left = 10;
                                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                RadWindow1.Modal = true;
                                RadWindow1.VisibleStatusbar = false;
                                RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                            }
                        }
                    }
                }
            }
            #endregion
            #region Order Consumable Items
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("CO"))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dsStatus = status.getStatus(common.myInt(Session["HospitalLocationId"]), "DrugOrderType", "CO", 0);
                        RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedication.aspx?Regno=" + lblregno.Text.Trim()
                                                + "&Encno=" + common.myStr(lblencno.Text.Trim())
                                                + "&RegId=" + common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]).Trim()
                                                + "&EncId=" + common.myStr(ds.Tables[0].Rows[0]["EncounterId"]).Trim()
                                                + "&POPUP=POPUP&DRUGORDERTYPE=" + common.myStr(dsStatus.Tables[0].Rows[0]["StatusID"])
                                                + "&DRUGORDERCODE=" + common.myStr(dsStatus.Tables[0].Rows[0]["Code"])
                                                + "&DoctorId=" + common.myStr(ds.Tables[0].Rows[0]["DoctorId"])
                                                + "&LOCATION=WARD";
                        RadWindow1.Top = 10;
                        RadWindow1.Left = 10;
                        RadWindow1.Height = 600;
                        RadWindow1.Width = 1000;
                        RadWindow1.Top = 40;
                        RadWindow1.Left = 100;
                        RadWindow1.OnClientClose = "";
                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.VisibleStatusbar = false;
                        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    }
                }
            }
            #endregion
            #region Service Order
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("SV"))
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServices.aspx?From=WARD&Regid=" + common.myInt(Session["RegistrationID"])
                                + "&RegNo=" + common.myInt(lblregno.Text) + "&EncId=" + common.myInt(Session["EncounterId"])
                                + "&EncNo=" + common.myInt(lblencno.Text) + "&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=&BType=";
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                            RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                        }
                    }
                }
            }
            #endregion
            #region Patient Dashboard
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("PD"))
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/emr/Dashboard/PatientDashboard.aspx?Regno=" + lblregno.Text.Trim() + "&Encno="
                                + lblencno.Text.Trim() + "&From=POPUP&Source=IPD";
                            RadWindow1.Height = 600;
                            RadWindow1.Width = 1000;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Add Services
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("IV"))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServices.aspx?Regid=" + common.myInt(Session["RegistrationID"])
                            + "&RegNo=" + lblregno.Text.Trim() + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + lblencno.Text
                            + "&OP_IP=I&CompanyId=" + common.myInt(ds.Tables[0].Rows[0]["SponsorId"].ToString())
                            + "&InsuranceId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"].ToString())
                            + "&CardId=" + common.myInt(ds.Tables[0].Rows[0]["PayorId"].ToString())
                            + "&PayerType=" + common.myInt(ds.Tables[0].Rows[0]["PayerType"].ToString())
                            + "&BType=" + common.myInt(ds.Tables[0].Rows[0]["CurrentBillCategory"].ToString());
                        RadWindow1.Top = 10;
                        RadWindow1.Left = 10;
                        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                        RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.VisibleStatusbar = false;
                    }
                }
            }
            #endregion
            #region Change Encounter Status
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("VC"))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadWindow1.NavigateUrl = "/WardManagement/ChangeEncounterPatient.aspx?RegNo=" + lblregno.Text.Trim() + "&StatusId=" + common.myStr(ds.Tables[0].Rows[0]["StatusId"]);
                        RadWindow1.Height = 300;
                        RadWindow1.Width = 500;
                        RadWindow1.Top = 10;
                        RadWindow1.Left = 10;
                        RadWindow1.OnClientClose = "OnClearClientClose";
                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.VisibleStatusbar = false;
                        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                    }
                }
            }
            #endregion
            #region Drug Return
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("DR"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/IPDItemReturn.aspx?MASTER=No&Regno=" + common.myStr(lblregno.Text) + "&Encno=" + common.myStr(lblencno.Text);
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 940;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Doctor Visit
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("DV"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/EMRBILLING/Popup/VisitPopup.aspx?Regid=" + common.myInt(Session["RegistrationId"])
                                + "&RegNo=" + common.myStr(lblregno.Text) + "&EncId=" + common.myInt(Session["EncounterId"])
                                + "&EncNo=" + common.myStr(lblencno.Text) + "&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0";
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 940;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Result Finalization
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("RV"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string admDate = DateTime.Now.ToString("dd/MM/yyyy");

                            if (common.myLen(ds.Tables[0].Rows[0]["EncounterDate"]) > 9)
                            {
                                admDate = common.myDate(common.myStr(ds.Tables[0].Rows[0]["EncounterDate"]).Substring(0, 10)).ToString("MM/dd/yyyy");
                            }

                            RadWindow1.NavigateUrl = "/LIS/Phlebotomy/ResultFinalization.aspx?RegNo=" + common.myStr(lblregno.Text)
                                + "&Admisiondate=" + admDate + "&OP_IP=I&Master=WARD";
                            RadWindow1.Height = 600;
                            RadWindow1.Width = 990;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                        RadWindow1.VisibleStatusbar = false;
                    }
                }
            }
            #endregion
            #region Referral Slip
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("RS"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "~/EMR/ReferralSlip.aspx?Regid=" + common.myInt(Session["RegistrationId"])
                                + "&RegNo=" + common.myStr(lblregno.Text) + "&EId=" + common.myInt(Session["EncounterId"])
                                + "&EncNo=" + common.myStr(lblencno.Text) + "&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=NO";
                            RadWindow1.Height = 550;
                            RadWindow1.Width = 940;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = "OnClientClose"; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region UnacknowledgedServices
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("UPS"))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadWindow1.NavigateUrl = "/EMRBILLING/Popup/UnacknowledgedServicesV1.aspx?encId=" + common.myInt(Session["EncounterId"]) + "";
                        RadWindow1.Height = 600;
                        RadWindow1.Width = 700;
                        RadWindow1.Top = 40;
                        RadWindow1.Left = 100;
                        RadWindow1.OnClientClose = "OnClientClose";
                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.VisibleStatusbar = false;
                        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                    }
                }
            }
            #endregion
            #region Attach Document
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("VD"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //RadWindow1.NavigateUrl = "/EMR/AttachDocument.aspx?RNo=" + common.myStr(lblregno.Text)
                            //    + "&Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(lblregno.Text)
                            //    + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(lblencno.Text)
                            //    + "&FromWard=Y&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=No";

                            RadWindow1.NavigateUrl = "/EMR/AttachDocumentFTP.aspx?RNo=" + common.myStr(lblregno.Text)
                                + "&Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(lblregno.Text)
                                + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(lblencno.Text)
                                + "&FromWard=Y&OP_IP=I&CompanyId=0&InsuranceId=0&CardId=0&PayerType=0&BType=0&MASTER=No";

                            //+ "&Category=PopUp";
                            RadWindow1.Height = 630;
                            RadWindow1.Width = 1200;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            //RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";                           
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region VisitHistory
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("VH"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["SelectedCaseSheet"] = "PN";
                            RadWindow1.NavigateUrl = "/WardManagement/VisitHistory.aspx?RNo=" + common.myStr(lblregno.Text)
                                + "&Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(lblregno.Text)
                                + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(lblencno.Text)
                                + "&FromWard=Y&OP_IP=I&Category=PopUp";
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 980;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region DischargeSummary
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("DS"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["FollowUpDoctorId"] = common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);
                            Session["FollowUpRegistrationId"] = common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]).Trim();
                            RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + common.myInt(Session["RegistrationId"])
                                + "&RegNo=" + common.myStr(lblregno.Text)
                                + "&EncId=" + common.myInt(Session["EncounterId"])
                                + "&EncNo=" + common.myStr(lblencno.Text)
                                + "&AdmissionDate=" + common.myDate(ds.Tables[0].Rows[0]["EncounterDate"]).ToString("MM/dd/yyyy")
                                + "&AdmDId=" + common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 980;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Diet Requisition
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("DIR"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "~/Diet/EMRPatientDietRequisition.aspx?Regid=" + common.myInt(Session["RegistrationID"])
                             + "&RegNo=" + common.myStr(lblregno.Text)
                             + "&EncId=" + common.myInt(Session["Encounterid"])
                             + "&EncNo=" + common.myStr(lblencno.Text)
                             + "&Ward=Ward";
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 880;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Clinical Template
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("NN"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?From=POPUP&EREncounterId=" + common.myInt(ds.Tables[0].Rows[0]["EREncounterId"])
                                + "&AdmissionDate=" + common.myDate(ds.Tables[0].Rows[0]["EncounterDate"]).ToString("MM/dd/yyyy");
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 880;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Consolidated Amount Report
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("CA"))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadWindow1.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(Session["EncounterId"])
                      + "&RptName=IPBill&RptType=D"
                      + "&BillId=" + common.myInt(0)
                      + "&AdmDt=" + common.myDate(ds.Tables[0].Rows[0]["EncounterDate"])
                      + "&Adv=Y"
                      + "&Disc=Y"
                      + "&RegId=" + common.myInt(Session["RegistrationId"])
                      + "&FromDate=" + common.myDate(DateTime.Now).ToString("dd/MM/yyyy HH:mm")
                      + "&ToDate=" + common.myDate(DateTime.Now).ToString("dd/MM/yyyy HH:mm")
                      + "&IsFilterByDate=0"
                      + "&ReportType=D";
                        RadWindow1.Height = 600;
                        RadWindow1.Width = 1000;
                        RadWindow1.Top = 10;
                        RadWindow1.Left = 10;
                        RadWindow1.VisibleOnPageLoad = true;
                        RadWindow1.Modal = true;
                        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        RadWindow1.VisibleStatusbar = false;
                    }
                }
            }
            #endregion
            #region DrugAdministered
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("DA"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "~/ICM/DrugAdministered.aspx?Master=NO&Regid=" + common.myInt(Session["RegistrationId"])
                                + "&RegNo=" + common.myStr(lblregno.Text)
                                + "&EncId=" + common.myInt(Session["EncounterId"])
                                + "&EncNo=" + common.myStr(lblencno.Text) + "&AdmissionDate=" + common.myDate(ds.Tables[0].Rows[0]["EncounterDate"]).ToString("MM/dd/yyyy");
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 980;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            // RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Case Sheet
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("CS"))   //Case Sheet         
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&EREncounterId=" + common.myInt(ds.Tables[0].Rows[0]["EREncounterId"])
                                + "&EncounterDate=" + common.myDate(ds.Tables[0].Rows[0]["EncounterDate"]).ToString("yyyy/MM/dd") + "&OPIP=I";
                            RadWindow1.Height = 600;
                            RadWindow1.Width = 990;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region DoctorProgressNote
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("PN"))//Progress Notes
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "~/EMR/Templates/DoctorProgressNote.aspx?Mpg=P1013&RegId=" + common.myStr(Session["RegistrationID"])
                                + "&OPIP=I&MP=NO";
                            RadWindow1.Height = 600;
                            RadWindow1.Width = 990;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Blood Bank
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("BR"))//start blood bank menu list
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/ComponentRequisition.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationID"])
                       + "&RegNo=" + common.myStr(lblregno.Text)
                        + "&EncId=" + common.myInt(Session["EncounterId"])
                        + "&EncNo=" + common.myStr(lblencno.Text);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region RequisitionAcknowledge
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("BRA"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/RequisitionAcknowledge.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationId"])
                       + "&RegNo=" + common.myStr(lblregno.Text)
                        + "&EncId=" + common.myInt(Session["EncounterId"])
                        + "&EncNo=" + common.myStr(lblencno.Text);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region RequisitionReleaseAcknowledge
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("BRR"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/RequisitionReleaseAcknowledge.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationId"])
                       + "&RegNo=" + common.myStr(lblregno.Text)
                        + "&EncId=" + common.myInt(Session["EncounterId"])
                        + "&EncNo=" + common.myStr(lblencno.Text);
                    RadWindow1.Height = 510;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region ReleaseAcknowledge
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("RA"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/ReleaseAcknowledge.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationId"])
                       + "&RegNo=" + common.myStr(lblregno.Text)
                        + "&EncId=" + common.myInt(Session["EncounterId"])
                        + "&EncNo=" + common.myStr(lblencno.Text);
                    RadWindow1.Height = 510;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region BloodAcknowledgeFromWard
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("BA"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/BloodAcknowledgeFromWard.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationId"])
                       + "&RegNo=" + common.myStr(lblregno.Text)
                        + "&EncId=" + common.myInt(Session["EncounterId"])
                        + "&EncounterNo=" + common.myStr(lblencno.Text);
                    RadWindow1.Height = 510;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region BloodTransfusionDetails
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("BTD"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/BloodTransfusionDetails.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationId"])
                       + "&RegNo=" + common.myStr(lblregno.Text)
                        + "&EncId=" + common.myInt(Session["EncounterId"])
                        + "&EncNo=" + common.myStr(lblencno.Text);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Blood Component Return
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("BCR"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/ComponentReturn.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationId"])
                       + "&RegNo=" + common.myStr(lblregno.Text)
                        + "&EncId=" + common.myInt(Session["EncounterId"])
                        + "&EncNo=" + common.myStr(lblencno.Text);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Adverse Transfusion Reaction Workup Report
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("ATRWR"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/BloodBank/SetupMaster/AdverseTransfusionReaction.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationId"])
                       + "&RegNo=" + common.myStr(lblregno.Text)
                        + "&EncId=" + common.myInt(Session["EncounterId"])
                        + "&EncNo=" + common.myStr(lblencno.Text);
                    RadWindow1.Height = 610;
                    RadWindow1.Width = 960;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Referral History
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("RH"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "/EMR/ReferralSlipHistory.aspx?MASTER=NO&Regno=" + common.myStr(lblregno.Text) + "&RegId="
                                + common.myStr(Session["RegistrationId"]) + "&EncId=" + common.myStr(Session["EncounterId"]);
                            RadWindow1.Height = 610;
                            RadWindow1.Width = 940;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.VisibleStatusbar = false;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region View Critical Tests
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("VCT"))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["EncounterId"] = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
                        Session["RegistrationId"] = common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]);
                        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&&EncId=" + common.myStr(Session["EncounterId"])
                            + "&RegNo=" + common.myStr(lblregno.Text) + "&PageSource=Ward";
                        RadWindow1.Height = 600;
                        RadWindow1.Width = 900;
                        RadWindow1.Top = 20;
                        RadWindow1.Left = 20;
                        RadWindow1.OnClientClose = "OnClientClose";
                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        RadWindow1.VisibleStatusbar = false;
                    }
                }
            }
            #endregion
            #region View Unperformed Service
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("VUS"))
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadWindow1.NavigateUrl = "UnacknowledgedServices.aspx?CF=&Master=Blank&&EncId=" + common.myStr(Session["EncounterId"])
                            + "&RegNo=" + common.myStr(lblregno.Text) + "&EncounterNo=" + lblencno.Text.Trim() + "&PageSource=Ward";
                        RadWindow1.Height = 600;
                        RadWindow1.Width = 900;
                        RadWindow1.Top = 20;
                        RadWindow1.Left = 20;
                        RadWindow1.OnClientClose = "OnClientClose";
                        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindow1.Modal = true;
                        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        RadWindow1.VisibleStatusbar = false;
                    }
                }
            }
            #endregion
            #region Drug Acknowledge
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("DAK"))   //Drug Acknowledge)
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RadWindow1.NavigateUrl = "~/WardManagement/DrugAcknowledge.aspx?Regno=" + lblregno.Text.Trim() + "&RegID="
                                + common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]) + "&From=POPUP&EncounterId="
                                + common.myInt(ds.Tables[0].Rows[0]["EncounterId"])
                                + "&AdmissionDate=" + common.myDate(ds.Tables[0].Rows[0]["EncounterDate"]).ToString("MM/dd/yyyy");
                            RadWindow1.Height = 600;
                            RadWindow1.Width = 990;
                            RadWindow1.Top = 10;
                            RadWindow1.Left = 10;
                            RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                            RadWindow1.Modal = true;
                            RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                            RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                        }
                    }
                }
            }
            #endregion
            #region Send to OT
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("STOT"))
            {
                if (common.myStr(lblregno.Text).Trim() != "" && common.myStr(lblencno.Text).Trim() != "")
                {
                    RadWindow1.NavigateUrl = "~/WardManagement/PatientTransferWardtoOT.aspx?RegNo=" + common.myStr(lblregno.Text);
                    RadWindow1.Height = 400;
                    RadWindow1.Width = 700;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    RadWindow1.Title = RadContextMenu2.SelectedItem.Text;
                    RadWindow1.VisibleStatusbar = false;
                }
            }
            #endregion
            #region Patient Allergy/Alert
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("VAA"))
            {
                RadWindow1.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&AlertType=ALL&CF=PTA&Ward=Y";
                RadWindow1.Height = 400;
                RadWindow1.Width = 700;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
            #endregion


            #region Provisional Bill Details
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("PBD"))
            {
                RadWindow1.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(Session["EncounterId"])
                    + "&RptName=IPBill&RptType=D"
                    + "&BillId=0"
                    + "&AdmDt=Y"
                    + "&Adv=Y"
                    + "&Disc=Y"
                    + "&RegId=" + common.myInt(Session["RegistrationID"])
                    + "&FromDate=" + common.myDate(common.myStr(ds.Tables[0].Rows[0]["EncounterDate"]).Substring(0, 10)).ToString("dd/MM/yyyy HH:mm")
                    + "&ToDate=" + Convert.ToDateTime(System.DateTime.Now).ToString("dd/MM/yyyy HH:mm")
                    + "&IsFilterByDate=0"
                    + "&ReportType=D"
                    + "&preview=N";
                RadWindow1.Height = 600;
                RadWindow1.Width = 1000;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.Modal = true;
                RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;

            }
            #endregion
            #region Service Activity Detail
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]) == "SAD")
            {
                RadWindow1.NavigateUrl = "~/WardManagement/ServiceActivityDetails.aspx?Regno=" + lblregno.Text.Trim() + "&RegID="
                                + common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]) + "&From=POPUP&EncounterId="
                                + common.myInt(ds.Tables[0].Rows[0]["EncounterId"])
                                + "&AdmissionDate=" + common.myDate(ds.Tables[0].Rows[0]["EncounterDate"]).ToString("MM/dd/yyyy");
                RadWindow1.Height = 400;
                RadWindow1.Width = 700;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
                RadWindow1.VisibleStatusbar = false;
            }
            #endregion

            #region Unperformed Service and Items
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("USAI"))
            {

                RadWindow1.NavigateUrl = "/EMRReports/Doctorwiseprocedurerpt.aspx?Fromdate=" + "1900/01/01" + "&Todate=" + "2079/01/01" + "&HospitalLocationId=" + common.myInt(Session["HospitalLocationID"]) + "&FacilityId=" + common.myInt(Session["FacilityID"]) + "&EncounterId=" + common.myInt(Session["encounterid"]) + "&ReportName=USAI";
                RadWindow1.Height = 600;
                RadWindow1.Width = 950;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
                RadWindow1.VisibleStatusbar = false;

            }
            #endregion
            #region Early Warning Score
            else if (common.myStr(RadContextMenu2.SelectedItem.Attributes["Code"]).Equals("EWS"))
            {
                RadWindow1.NavigateUrl = "/EMR/Templates/EWSTemplate.aspx?HospitalLocationId=" + common.myInt(Session["HospitalLocationID"]) + "&FacilityId=" + common.myInt(Session["FacilityID"]) + "&EncounterId=" + common.myInt(Session["encounterid"]) + "&From=POPUP";
                RadWindow1.Height = 600;
                RadWindow1.Width = 950;
                RadWindow1.Top = 40;
                RadWindow1.Left = 100;
                RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindow1.Modal = true;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
                RadWindow1.VisibleStatusbar = false;
            }
            
            #endregion
        }
        catch (Exception Ex)
        {
            Alert.ShowAjaxMsg("Error: " + Ex.Message, Page);
            objException.HandleException(Ex);
        }
        finally
        {
            dsStatus.Dispose();
            status = null;
            patient = null;
            ds.Dispose();
        }
    }
    protected void ddlPatient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = sender as RadComboBox;
        GridViewRow row = ddl.NamingContainer as GridViewRow;
        //if (e.Text != "")
        //{
        DataTable data = GetData(e.Text + "%");

        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            //ddl.Items.Add(new RadComboBoxItem(data.Rows[i]["ServiceName"].ToString(), data.Rows[i]["ServiceId"].ToString()));
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["PatientName"];
            item.Value = data.Rows[i]["RegistrationId"].ToString();
            item.Attributes.Add("Account", data.Rows[i]["RegistrationNo"].ToString());
            item.Attributes.Add("EncounterNo", data.Rows[i]["EncounterNo"].ToString());
            item.Attributes.Add("AgeGender", data.Rows[i]["AgeGender"].ToString());
            item.Attributes.Add("Ward/Bed", data.Rows[i]["Ward/Bed"].ToString());
            item.Attributes.Add("FatherName", data.Rows[i]["FatherName"].ToString());
            this.ddlPatient.Items.Add(item);
            item.DataBind();
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        //}
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches found";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetData(string text)
    {
        String sKeyword = text;

        BaseC.RestFulAPI objBilling = new BaseC.RestFulAPI(sConString);
        DataSet ds = objBilling.SearchAdmitedPatientByName(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), sKeyword);

        DataTable data = new DataTable();
        data = ds.Tables[0];
        return data;
    }

    protected void btnRegNoFind_OnClick(Object sender, EventArgs e)
    {
        fillData(common.myInt(ddlColumns.SelectedValue));
    }

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        hdnregID.Value = "0";
        ddlPatient.Text = "";
        //  ddlWard.SelectedIndex = 0;
        ddlPatient.Items.Clear();
        ddlPatient.Text = "";
        DataTable dt1 = new DataTable();
        ViewState["dt"] = null;
        fillData(common.myInt(ddlColumns.SelectedValue));
    }
    protected void ddlbedstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillData(common.myInt(ddlColumns.SelectedValue));
    }


    protected void ddlWard_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        fillData(common.myInt(ddlColumns.SelectedValue));
    }
    protected void ddlColumns_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        fillData(common.myInt(ddlColumns.SelectedValue));
    }

    protected void lnkUnperformedService_OnClick(object sender, EventArgs e)
    {

        //RadWindow1.NavigateUrl = "UnacknowledgedServices.aspx?encId=" + hdnEncounterId.Value + "&RegId=" + hdnRegistrationId.Value + "";
        //RadWindow1.Height = 600;
        //RadWindow1.Width = 850;
        //RadWindow1.Top = 40;
        //RadWindow1.Left = 100;
        //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        //RadWindow1.Modal = true;
        //RadWindow1.VisibleStatusbar = false;
    }
    protected void lnkunperformedSer_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=Unperformed";
        RadWindow1.Height = 650;
        RadWindow1.Width = 1125;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void lnkdrugordercount_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=DrugOrder";
        RadWindow1.Height = 650;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void lnknondrugordercount_Click(object sender, EventArgs e)
    {

        RadWindow1.NavigateUrl = "/WardManagement/UnperformedServiceListDetails.aspx?Type=NonDrugOrder";
        RadWindow1.Height = 650;
        RadWindow1.Width = 1100;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void ddlEntrySitesActual_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        fillData(common.myInt(ddlColumns.SelectedValue));
    }

    protected void ddlEncounterStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        fillData(common.myInt(ddlColumns.SelectedValue));
    }

    private void fillSampleCollectedStatus()
    {
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        int stationID = common.myInt(Session["StationId"]);
        DataSet ds = new DataSet();
        try
        {
            ds = objval.GetSampleCollectedStatus(common.myInt(Session["FacilityId"]), common.myDate(DateTime.Today), common.myDate(DateTime.Today));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblRejectedSampleStart.Text = "&nbsp;(&nbsp;Rejected&nbsp;Sample&nbsp;";
                    lblRejectedSampleEnd.Text = "&nbsp;)";
                    lnkBtnRejectedSampleIPCount.Text = "&nbsp;&nbsp;&nbsp;IP&nbsp;-&nbsp;" + common.myInt(ds.Tables[0].Rows[0]["IPRS"]);
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void lnkBtnRejectedSampleOPCount_OnClick(object sender, EventArgs e)
    {
        try
        {
            showRejectedSample("OPD");
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lnkBtnRejectedSampleIPCount_OnClick(object sender, EventArgs e)
    {
        try
        {
            showRejectedSample("IPD");
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void showRejectedSample(string Source)
    {
        try
        {
            //lblMessage.Text = "";

            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/RejectedSampleDetails.aspx?SOURCE=" + common.myStr(Source) +
                                        "&FDate=" + common.myDate(DateTime.Today).ToString("yyyy-MM-dd") +
                                        "&TDate=" + common.myDate(DateTime.Today).ToString("yyyy-MM-dd") + "&CallFrom=1";

            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 1100;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void lnkBedClearance_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/ATD/BedClearance.aspx";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1100;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
    }

}
