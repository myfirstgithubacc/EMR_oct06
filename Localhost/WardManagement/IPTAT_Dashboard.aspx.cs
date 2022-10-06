using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class WardManagement_Default : System.Web.UI.Page
{

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);

            if (!Page.IsPostBack)
            {
                int TimerInerval = common.myInt(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "AllowAutoRefereshDischargeDashboard", sConString));
                if (TimerInerval.Equals(0))
                {
                    TimerInerval = 30000;
                }

                Timer1.Interval = TimerInerval;
                ViewState["TimerInerval"] = TimerInerval;

                bindStation();
                bindControl();
                
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void bindControl()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objWD = new BaseC.WardManagement();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
          
            //ds = objadt.GetWardStation(common.myInt(Session["FacilityId"]));

            if (common.myStr(ViewState["IsShowWardComputerTaggingDetails"]).Equals("Y"))
            {
                ds = objWD.GetWardComputerTagging(common.myInt(Session["HospitalLocationID"]), true, common.myStr(Environment.MachineName), common.myInt(Session["FacilityId"]));

            }
            else
            {
                ds = objWD.GetWardTagging(common.myInt(Session["HospitalLocationID"]), true, common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));
                if (ds.Tables[0].Rows.Count.Equals(0))
                {
                    if (common.myStr(ViewState["isRequiredAllFacilityBed"]) == "Y")
                    {
                        ds = objadt.GetWard(common.myInt(0));
                    }
                    else
                    {
                        ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
                    }
                }
            }

            ddlWard.DataSource = ds.Tables[0];
            //ddlWard.DataTextField = "StationName";
            //ddlWard.DataValueField = "ID";
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            ds.Tables[0].DefaultView.RowFilter = "FavouriteWard = '1'";
            DataTable dt = (ds.Tables[0].DefaultView).ToTable();
            if (dt.Rows.Count > 0)
            {
                List<Int16> lstWARDID = dt.Rows.OfType<DataRow>().Select(dr => (Int16)dr["WardId"]).ToList();
                // List<string> lstWARDID = new List<string>();
                // lstWARDID = (List<string>)Session["WARDDETAILSFINDWARDID"];
                foreach (RadComboBoxItem currentItem in ddlWard.Items)
                {
                    foreach (int WardValueChecked in lstWARDID)
                    {
                        if (currentItem.Value.Equals(common.myStr(WardValueChecked)))
                        {
                            currentItem.Checked = true;
                        }
                    }
                }
            }
            else
            {
                if (common.myLen(Session["WARDDETAILSFINDWARDID"]) > 0)
                {
                    try
                    {
                        List<string> lstWARDID = new List<string>();
                        lstWARDID = (List<string>)Session["WARDDETAILSFINDWARDID"];
                        foreach (RadComboBoxItem currentItem in ddlWard.Items)
                        {
                            currentItem.Checked = true;
                            foreach (string WardValueChecked in lstWARDID)
                            {
                                if (currentItem.Value.Equals(WardValueChecked))
                                {
                                    currentItem.Checked = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        foreach (RadComboBoxItem currentItem in ddlWard.Items)
                        {
                            currentItem.Checked = true;
                        }
                    }
                }
                else
                {
                    foreach (RadComboBoxItem currentItem in ddlWard.Items)
                    {
                        currentItem.Checked = true;
                    }
                }
            }

            BindGrid();
            // ddlWard.Items.Insert(0, new RadComboBoxItem("All", "0"));
            //ddlWard.SelectedIndex = 0;


            //  BindGroupTaggingMenu();

        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objWD = null;
            objadt = null;
            ds.Dispose();
        }
    }
    private void bindStation()
    {
        DataSet ds = new DataSet();
        //BaseC.WardManagement objWD = new BaseC.WardManagement();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            
            ds = objadt.GetWardStation(common.myInt(Session["FacilityId"]), common.myInt(Session["EmployeeId"]));

            ddlStation.DataSource = ds.Tables[0];
            ddlStation.DataTextField = "StationName";
            ddlStation.DataValueField = "ID";
            ddlStation.DataBind();
            ddlStation.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlStation.SelectedIndex = 0;

        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            // objWD = null;
            objadt = null;
            ds.Dispose();
        }
    }
    void BindGrid()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement obj = new BaseC.WardManagement();
        try
        {
            //filter add
            int ddlStationid = 0;
            if (common.myInt(ddlStation.SelectedValue) > 0)
            {
                ddlStationid = common.myInt(ddlStation.SelectedValue);
            }

            // filter Add
            ds = obj.GetIPTATDashBoardDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), showCheckAllItems(ddlWard), ddlStationid, common.myStr(ddlfilter.SelectedValue), common.myInt(ddlDuration.SelectedValue));

            if (common.myInt(ds.Tables.Count) > 1)
            {
                if (common.myInt(ds.Tables[1].Rows.Count) > 0)
                {
                    ViewState["SBToPCDurationMin"] = ds.Tables[1].Rows[0]["SBToPCDurationMin"];
                    ViewState["PCToBPDurationMin"] = ds.Tables[1].Rows[0]["PCToBPDurationMin"];
                    ViewState["BPToDNNonTPADurationMin"] = ds.Tables[1].Rows[0]["BPToDNNonTPADurationMin"];
                    ViewState["BPToDNTPADurationMin"] = ds.Tables[1].Rows[0]["BPToDNTPADurationMin"];
                    ViewState["DNToBVDurationMin"] = ds.Tables[1].Rows[0]["DNToBVDurationMin"];
                    ViewState["DNToDADurationMin"] = ds.Tables[1].Rows[0]["DNToDADurationMin"];
                }
            }

            if (common.myInt(ds.Tables.Count) > 0)
            {
                if (common.myInt(ds.Tables[0].Rows.Count) > 0)
                {
                    gvEncounterStatusList.DataSource = ds.Tables[0];
                    gvEncounterStatusList.DataBind();
                }
                else
                {

                    gvEncounterStatusList.DataSource = ds.Tables[0];
                    gvEncounterStatusList.DataBind();
                }
            }




        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null; lblMessage.Text = Ex.ToString();
        }
        finally { ds.Dispose(); obj = null; }
    }
    private string showCheckAllItems(RadComboBox rcb)
    {
        string checkAll = String.Empty;
        var collectionChecked = rcb.CheckedItems;
        if (collectionChecked.Count > 0)
        {
            foreach (var item in collectionChecked)
            {
                if (checkAll.Equals(String.Empty))
                {
                    checkAll = item.Value;
                    item.Attributes.ToString();
                }
                else
                {
                    checkAll = checkAll + "," + item.Value;
                }
            }
        }
        return checkAll;
    }

    protected void gvEncounterStatusList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }
        if (e.Item is GridDataItem)
        {
            HiddenField hdnIsPharmacyClearance = (HiddenField)e.Item.FindControl("hdnIsPharmacyClearance");
            Image imgPCGreen = (Image)e.Item.FindControl("imgPCGreen");
            Image imgPCOrange = (Image)e.Item.FindControl("imgPCOrange");
            Image imgPCRed = (Image)e.Item.FindControl("imgPCRed");
            Label lblPharmacyClearanceDueMin = (Label)e.Item.FindControl("lblPharmacyClearanceDueMin");
            Label lblPCHourMin = (Label)e.Item.FindControl("lblPCHourMin");

            HiddenField hdnIsBillPrepared = (HiddenField)e.Item.FindControl("hdnIsBillPrepared");
            Image imgBPGreen = (Image)e.Item.FindControl("imgBPGreen");
            Image imgBPOrange = (Image)e.Item.FindControl("imgBPOrange");
            Image imgBPRed = (Image)e.Item.FindControl("imgBPRed");
            Label lblBillPreparedDueMin = (Label)e.Item.FindControl("lblBillPreparedDueMin");
            Label lblBPHourMin = (Label)e.Item.FindControl("lblBPHourMin");

            HiddenField hdnIsBillPaidFIC = (HiddenField)e.Item.FindControl("hdnIsBillPaidFIC");
            Image imgBPFGreen = (Image)e.Item.FindControl("imgBPFGreen");
            Image imgBPFOrange = (Image)e.Item.FindControl("imgBPFOrange");
            Image imgBPFRed = (Image)e.Item.FindControl("imgBPFRed");
            Label lblBillPaidFICDueMin = (Label)e.Item.FindControl("lblBillPaidFICDueMin");
            Label lblBPFHourMin = (Label)e.Item.FindControl("lblBPFHourMin");

            HiddenField hdnIsBedRelease = (HiddenField)e.Item.FindControl("hdnIsBedRelease");
            Image imgBVGreen = (Image)e.Item.FindControl("imgBVGreen");
            Image imgBVOrange = (Image)e.Item.FindControl("imgBVOrange");
            Image imgBVRed = (Image)e.Item.FindControl("imgBVRed");
            Label lblBedVacantDueMin = (Label)e.Item.FindControl("lblBedVacantDueMin");
            Label lblBRHourMin = (Label)e.Item.FindControl("lblBRHourMin");


            HiddenField hdnDischargeApproval = (HiddenField)e.Item.FindControl("hdnDischargeApproval");
            Image imgDAGreen = (Image)e.Item.FindControl("imgDAGreen");
            Image imgDAOrange = (Image)e.Item.FindControl("imgDAOrange");
            Image imgDARed = (Image)e.Item.FindControl("imgDARed");
            Label lblDADueMin = (Label)e.Item.FindControl("lblDADueMin");
            Label lblDAHourMin = (Label)e.Item.FindControl("lblDAHourMin");


            Label lblCompanytype = (Label)e.Item.FindControl("lblCompanytype");

            int SBToPCDurationMin = common.myInt(ViewState["SBToPCDurationMin"]);
            int PCToBPDurationMin = common.myInt(ViewState["PCToBPDurationMin"]);
            int BPToDNNonTPADurationMin = common.myInt(ViewState["BPToDNNonTPADurationMin"]);
            int BPToDNTPADurationMin = common.myInt(ViewState["BPToDNTPADurationMin"]);
            int DNToBVDurationMin = common.myInt(ViewState["DNToBVDurationMin"]);
            int DNToDADurationMin = common.myInt(ViewState["DNToDADurationMin"]);

            if (!common.myBool(hdnIsPharmacyClearance.Value))
            {
                //lblPharmacyClearanceDueMin.Visible = true;
                lblPCHourMin.Visible = true;
                if (common.myInt(lblPharmacyClearanceDueMin.Text) > SBToPCDurationMin)
                {
                    imgPCRed.Visible = true;
                }
                else
                {
                    imgPCOrange.Visible = true;
                }
            }
            else
            {
                imgPCGreen.Visible = true;
                #region BillPrepared
                if (!common.myBool(hdnIsBillPrepared.Value))
                {
                    // lblBillPreparedDueMin.Visible = true;
                    lblBPHourMin.Visible = true;
                    if (common.myInt(lblBillPreparedDueMin.Text) > PCToBPDurationMin)
                    {
                        imgBPRed.Visible = true;
                    }
                    else
                    {
                        imgBPOrange.Visible = true;
                    }
                }
                else
                {
                    imgBPGreen.Visible = true;
                    #region FIC
                    if (!common.myBool(hdnIsBillPaidFIC.Value))
                    {
                        //lblBillPaidFICDueMin.Visible = true;
                        lblBPFHourMin.Visible = true;

                        if (common.myStr(lblCompanytype.Text).ToUpper().Contains("TPA") || common.myStr(lblCompanytype.Text).ToUpper().Contains("INSURANCE"))
                        {
                            if (common.myInt(lblBillPaidFICDueMin.Text) > BPToDNTPADurationMin)
                            {
                                imgBPFRed.Visible = true;
                            }
                            else
                            {
                                imgBPFOrange.Visible = true;
                            }
                        }

                        else
                        {
                            if (common.myInt(lblBillPaidFICDueMin.Text) > BPToDNNonTPADurationMin)
                            {
                                imgBPFRed.Visible = true;
                            }
                            else
                            {
                                imgBPFOrange.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        imgBPFGreen.Visible = true;
                        
                        #region DischargeApprovel
                        if (!common.myBool(hdnDischargeApproval.Value))
                        {
                            //lblBedVacantDueMin.Visible = true;
                            lblDAHourMin.Visible = true;
                            if (common.myInt(lblDADueMin.Text) > DNToDADurationMin)
                            {
                                imgDARed.Visible = true;
                            }
                            else
                            {
                                imgDAOrange.Visible = true;
                            }
                        }
                        else
                        {

                            imgDAGreen.Visible = true;

                            #region BedRelease
                            if (!common.myBool(hdnIsBedRelease.Value))
                            {
                                //lblBedVacantDueMin.Visible = true;
                                lblBRHourMin.Visible = true;
                                if (common.myInt(lblBedVacantDueMin.Text) > DNToBVDurationMin)
                                {
                                    imgBVRed.Visible = true;
                                }
                                else
                                {
                                    imgBVOrange.Visible = true;
                                }
                            }
                            else
                            {
                                imgBVGreen.Visible = true;
                            }
                            #endregion
                        }
                        #endregion


                    }

                    #endregion
                }


                #endregion
            }


        }
    }

    protected void gvEncounterStatusList_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvEncounterStatusList.CurrentPageIndex = gvEncounterStatusList.CurrentPageIndex + 1;
        BindGrid();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddlWard_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindGrid();
    }

    protected void ddlStation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindGrid();
    }

    protected void AotoRefresh_OnClick(object Sender, EventArgs e)
    {

        int TimerInerval = common.myInt(ViewState["TimerInerval"]);

        if (TimerInerval > 0)
        {
            Timer1.Interval = common.myInt(TimerInerval);
                BindGrid();
            Timer1.Interval = common.myInt(TimerInerval);
        }

     
    }
}