using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;
using System.Collections;
using System.Text;

public partial class OTScheduler_OTAntibioticEntry : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populateAntibiotic();
            BindGrid();
            GetAdmissionDateTime();
        }
    }


    public void BindGrid()
    {
        BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
        DataSet ds = new DataSet();
        ds = objOT.GetSurgeryOtherDetails(common.myInt(Request.QueryString["OTBookingId"]));

        if (ds.Tables.Count > 0)
        {
            DataView dv = new DataView(ds.Tables[0]);
            hdnServiceId.Value = dv[0]["serviceId"].ToString();
            Session["serviceId"] = hdnServiceId.Value;
            gvOTAntibioticDetail.DataSource = dv;
            gvOTAntibioticDetail.DataBind();
        }
        if (common.myStr(ds.Tables[0].Rows[0]["Antibiotics"]) == "Yes")
        {
            gvOTAntibioticDetail.Columns[8].Visible = true;
        }
    }

    public void populateAntibiotic()
    {
        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        ds = objEmr.GetDiagAntibiotics(common.myInt(Session["HospitalLocationID"]));
        if (ds.Tables.Count > 0)
        {
            rlbAntibiotic.DataTextField = "AntibioticName";
            rlbAntibiotic.DataValueField = "AntibioticId";
            rlbAntibiotic.DataSource = ds;
            rlbAntibiotic.DataBind();
        }
    }

    private void GetAdmissionDateTime()
    {

        if (!string.IsNullOrEmpty(Request.QueryString["OTBookingId"]))
        {

            Hashtable hshInCHK = new Hashtable();
            StringBuilder sbCHk = new StringBuilder();
            DataSet dsCHK = new DataSet();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            string strAdmissionDateTime = string.Empty;
            try
            {

                hshInCHK.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                hshInCHK.Add("@intOTBookingId", common.myInt(Request.QueryString["OTBookingId"]));
                sbCHk.Append("SELECT  dbo.GetDateFormatUTC(enc.EncounterDate, 'DT', fm.TimeZoneOffSetMinutes) as AdmissionDateTime ");
                sbCHk.Append(" FROM OTBooking ot");
                sbCHk.Append(" INNER JOIN Encounter enc ON ot.EncounterID = enc.Id AND enc.Active = 1");
                sbCHk.Append(" INNER JOIN FacilityMaster fm ON enc.FacilityId = fm.FacilityId AND fm.Active = 1");
                sbCHk.Append(" WHERE ot.OTBookingId = @intOTBookingId");
                sbCHk.Append(" AND ot.FacilityID = @intFacilityId");
                sbCHk.Append(" AND enc.FacilityID = @intFacilityId");

                dsCHK = dl.FillDataSet(CommandType.Text, sbCHk.ToString(), hshInCHK);
                if (dsCHK.Tables.Count > 0)
                {
                    if (dsCHK.Tables[0].Rows.Count > 0)
                    {
                        strAdmissionDateTime = common.myStr(dsCHK.Tables[0].Rows[0]["AdmissionDateTime"]);
                        txtAdmissionDate.Text = strAdmissionDateTime;

                    }
                }


            }
            catch (Exception Ex)
            {
            }
            finally
            {
                hshInCHK = null;
                sbCHk = null;
                dsCHK.Dispose();
            }

        }
    }
    protected void btnSaveData_Click(object sender, EventArgs e)
    {
        int TimeDiff = 0;
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        DateTime srr = DateTime.Now;
        DateTime bloomingDateTime = Convert.ToDateTime(txtIntiTime.SelectedDate);
        DateTime AdmissionDateTime = common.myDate(txtAdmissionDate.Text);
        TimeSpan TimeSpan = bloomingDateTime.Subtract(AdmissionDateTime);
        //int TimeDiff = (int)TimeSpan.Minutes;
        TimeDiff = TimeDiff + ((bloomingDateTime.Subtract(AdmissionDateTime).Days) * 24 * 60) + ((bloomingDateTime.Subtract(AdmissionDateTime).Hours) * 60) + (bloomingDateTime.Subtract(AdmissionDateTime).Minutes);
        try
        {
            lblMessage.Text = "";



            if (txtoralstartDate.SelectedDate.Equals(null))
            {
                Alert.ShowAjaxMsg("Please select Intubation Start Time", this.Page);
                return;
            }
            else if (txtScanInDate.SelectedDate.Equals(null))
            {
                Alert.ShowAjaxMsg("Please select Incision Time ", this.Page);
                return;
            }
            else
            {
                BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
                DataSet ds = new DataSet();

                BaseC.clsOTBooking ValobjOT = new BaseC.clsOTBooking(sConString);
                DataSet ds1 = ValobjOT.GetCheckITOT(common.myStr(common.myInt(Request.QueryString["OTBookingId"])), common.myInt(Session["FacilityId"]));
                if (ds1.Tables.Count > 0)
                {
                    if (ds1.Tables[0].Rows[0]["OTCheckintime"].ToString().Equals(string.Empty))
                    {
                        Alert.ShowAjaxMsg("Please update OT check in time !", this.Page);
                        return;
                    }

                    if (ds1.Tables[0].Rows[0]["OTCheckintime"].ToString() != "")
                    {
                        if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckintime"]) > Convert.ToDateTime(txtoralstartDate.SelectedDate) && !txtoralstartDate.SelectedDate.Equals(null))
                        {
                            Alert.ShowAjaxMsg("Intubation Start Time  must be greater than check in time!", this.Page);
                            return;
                        }
                        if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckintime"]) > Convert.ToDateTime(txtScanInDate.SelectedDate) && !txtScanInDate.SelectedDate.Equals(null))
                        {
                            Alert.ShowAjaxMsg("Incision Time  must be greater than check in time!", this.Page);
                            return;
                        }
                        if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckintime"]) > Convert.ToDateTime(txtScanOutDate.SelectedDate) && !txtScanOutDate.SelectedDate.Equals(null))
                        {
                            Alert.ShowAjaxMsg("Antibiotic Prophylactic Time must be greater than check in time!", this.Page);
                            return;
                        }
                        if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckintime"]) > Convert.ToDateTime(txtIntiTime.SelectedDate) && !txtIntiTime.SelectedDate.Equals(null))
                        {
                            Alert.ShowAjaxMsg("Ballooning Time must be greater than check in time!", this.Page);
                            return;
                        }
                    }

                    if (ds1.Tables[0].Rows[0]["OTCheckouttime"].ToString() != "")
                    {
                        if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckouttime"]) < Convert.ToDateTime(txtoralstartDate.SelectedDate) && !txtoralstartDate.SelectedDate.Equals(null))
                        {
                            Alert.ShowAjaxMsg("Intubation Start Time must be less than to check out time!", this.Page);
                            return;
                        }
                        if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckouttime"]) < Convert.ToDateTime(txtScanInDate.SelectedDate) && !txtScanInDate.SelectedDate.Equals(null))
                        {
                            Alert.ShowAjaxMsg("Incision Time must be less than to check out time!", this.Page);
                            return;
                        }
                        if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckouttime"]) < Convert.ToDateTime(txtScanOutDate.SelectedDate) && !txtScanOutDate.SelectedDate.Equals(null))
                        {
                            Alert.ShowAjaxMsg("IncisionAntibiotic Prophylactic Time must be less than to check out time!", this.Page);
                            return;
                        }
                        if (Convert.ToDateTime(ds1.Tables[0].Rows[0]["OTCheckouttime"]) < Convert.ToDateTime(txtIntiTime.SelectedDate) && !txtIntiTime.SelectedDate.Equals(null))
                        {
                            Alert.ShowAjaxMsg("Ballooning Time must be less than to check out time!", this.Page);
                            return;
                        }
                    }
                }
                if (TimeDiff > 90)
                {
                    Alert.ShowAjaxMsg("Please Enter Remarks!", this.Page);
                    return;
                }


                if (common.myStr(ViewState["chkRemarks"]) == "1")
                {
                    Alert.ShowAjaxMsg("Please Enter Remarks!", this.Page);
                    return;
                }
                ds = objEmr.GetDiagAntibiotics(common.myInt(Session["HospitalLocationID"]));
                var collection = rlbAntibiotic.CheckedItems;

                foreach (var item in collection)
                {
                    if (item.Checked)
                    {
                        coll.Add(common.myInt(item.Value));
                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
                String ScanOutDate = null;
                if (!txtScanOutDate.SelectedDate.Equals(null))
                {
                    ScanOutDate = Convert.ToString(txtScanOutDate.SelectedDate);
                }
                String IntiTime = null;
                if (!txtScanOutDate.SelectedDate.Equals(null))
                {
                    IntiTime = Convert.ToString(txtScanOutDate.SelectedDate);
                }

                BaseC.clsOTBooking objOT = new BaseC.clsOTBooking(sConString);
                Hashtable htOut = new Hashtable();


                htOut = objOT.SaveOTAntibioticDetail(common.myInt(Request.QueryString["OTBookingId"]), common.myInt(hdnServiceId.Value), common.myDate(txtoralstartDate.SelectedDate),
                                                     common.myDate(txtScanInDate.SelectedDate), ScanOutDate, IntiTime,
                                                     common.myStr(strXML), common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), common.myStr(txtRemarks.Text));


                String strMessage = common.myStr(htOut["@chvErrorStatus"].ToString());
                if (strMessage == "Record(s) Saved")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = "Record Saved";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strMessage;
                }
                BindGrid();
            }
            clear();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


    }

    protected void gvOTAntibioticDetail_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();

            if (e.CommandName == "Select")
            {
                //BindGrid();
                int ServiceId = common.myInt(e.CommandArgument.ToString());
                Session["ServiceId"] = common.myStr(ServiceId);

                btnSaveData.Text = "Update";

                if (((Label)e.Item.FindControl("lblIntubation")).Text != "")
                {

                    txtoralstartDate.SelectedDate = common.myDate_withFormat((((Label)e.Item.FindControl("lblIntubation")).Text), "dd/MM/yyyy HH:mm", "/");

                }
                if (((Label)e.Item.FindControl("lblIncisionTime")).Text != "")
                {

                    txtScanInDate.SelectedDate = common.myDate_withFormat((((Label)e.Item.FindControl("lblIncisionTime")).Text), "dd/MM/yyyy HH:mm", "/");
                }

                if (((Label)e.Item.FindControl("lblAnProphylactic")).Text != "")
                {

                    txtScanOutDate.SelectedDate = common.myDate_withFormat((((Label)e.Item.FindControl("lblAnProphylactic")).Text), "dd/MM/yyyy HH:mm", "/");
                }
                if (((Label)e.Item.FindControl("lblBallooningTime")).Text != "")
                {
                    txtIntiTime.SelectedDate = common.myDate_withFormat((((Label)e.Item.FindControl("lblBallooningTime")).Text), "dd/MM/yyyy HH:mm", "/");
                }

                if (((Label)e.Item.FindControl("lblRemarks")).Text != "")
                {
                    //divRemarks.Style.Add("display", "block");
                    txtRemarks.Text = (((Label)e.Item.FindControl("lblRemarks"))).Text;
                }

                TaggedCheckBox();

            }
            else if (e.CommandName == "Yes")
            {
                RadWindowForNew.NavigateUrl = "~/OTScheduler/TaggedAntibiotic.aspx?&OTBookingId=" + common.myInt(Request.QueryString["OTBookingId"]) + "&serviceId=" + common.myInt(Session["ServiceId"]);
                RadWindowForNew.Height = 300;
                RadWindowForNew.Width = 300;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowForNew.VisibleStatusbar = false;

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    private void TaggedCheckBox()
    {
        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        rlbAntibiotic.ClearChecked();
        ds = objEmr.getAntibiloticTaggedWithOTPatient(common.myInt(Request.QueryString["OTBookingId"]), common.myInt(Session["ServiceId"]));

        if (ds.Tables.Count > 0)
        {
            if (rlbAntibiotic.Items.Count > 0)
            {
                for (int i = 0; i < rlbAntibiotic.Items.Count; i++)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (rlbAntibiotic.Items[i].Value == common.myStr(dr["AntibioticId"]))
                        {
                            rlbAntibiotic.Items[i].Checked = true;
                        }
                    }
                }

            }
        }
    }


    protected void btnNew_Click(object sender, EventArgs e)
    {
        clear();

    }
    private void clear()
    {

        txtIntiTime.SelectedDate = null;
        txtoralstartDate.SelectedDate = null;
        txtScanInDate.SelectedDate = null;
        txtScanInDate.SelectedDate = null;
        txtScanOutDate.SelectedDate = null;
        rlbAntibiotic.ClearChecked();
        txtRemarks.Text = "";
        //divRemarks.Style.Add("dispaly", "none");
    }
}