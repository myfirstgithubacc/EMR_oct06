using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Telerik.Web.UI;
using BaseC;

public partial class WardManagement_OralGiven : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindControl();
            switch (common.myStr(Request.QueryString["SearchCriteria"]))
            {
                case "R":
                    txtSearchRegNo.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearchRegNo.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "ENC":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "N":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
            }

            if (common.myLen(Request.QueryString["WardId"]) > 0)
            {
                ddlWard.SelectedIndex = ddlWard.Items.IndexOf(ddlWard.Items.FindItemByValue(common.myInt(Request.QueryString["WardId"]).ToString()));
            }

            BindData();
        }
    }
    protected void ddlSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlWard.Visible = false;
        txtSearchRegNo.Visible = false;
        txtSearch.Visible = false;

        switch (common.myStr(ddlSearchCriteria.SelectedValue))
        {
            //case "W":
            //    ddlWard.Visible = true;
            //    break;
            case "R":
                txtSearchRegNo.Visible = true;
                break;
            case "ENC":
                txtSearch.Visible = true;
                break;
            case "P":
                txtSearch.Visible = true;
                break;
        }
    }
    private void BindControl()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        DataSet ds = new DataSet();
        try
        {

            #region Date Range
            //ddlrange.Items.Clear();

            //RadComboBoxItem ls = new RadComboBoxItem();
            //ls.Text = "Select All";
            //ls.Value = "";
            //ddlrange.Items.Add(ls);

            //RadComboBoxItem lst10 = new RadComboBoxItem();
            //lst10.Text = "This Week";
            //lst10.Value = "WW0";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW0")
            //{
            //    lst10.Selected = true;
            //}
            //ddlrange.Items.Add(lst10);

            //RadComboBoxItem lst = new RadComboBoxItem();
            //lst.Text = "Next Week";
            //lst.Value = "WW+1";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW+1")
            //{
            //    lst.Selected = true;
            //}
            //ddlrange.Items.Add(lst);

            //RadComboBoxItem lst1 = new RadComboBoxItem();
            //lst1.Text = "Next Two Week";
            //lst1.Value = "WW+2";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW+2")
            //{
            //    lst1.Selected = true;
            //}
            //ddlrange.Items.Add(lst1);

            //RadComboBoxItem lst2 = new RadComboBoxItem();
            //lst2.Text = "Next Year";
            //lst2.Value = "YY+1";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "YY+1")
            //{
            //    lst2.Selected = true;
            //}
            //ddlrange.Items.Add(lst2);

            //RadComboBoxItem lst3 = new RadComboBoxItem();
            //lst3.Text = "Today";
            //lst3.Value = "DD0";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "DD0")
            //{
            //    lst3.Selected = true;
            //}
            //else if (ViewState["SelectedDate"] == null)
            //{
            //    lst3.Selected = true;
            //}

            //ddlrange.Items.Add(lst3);

            //RadComboBoxItem lst4 = new RadComboBoxItem();
            //lst4.Text = "Last Week";
            //lst4.Value = "WW-1";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW-1")
            //{
            //    lst4.Selected = true;
            //}
            //ddlrange.Items.Add(lst4);

            //RadComboBoxItem lst5 = new RadComboBoxItem();
            //lst5.Text = "Last Two Week";
            //lst5.Value = "WW-2";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "WW-2")
            //{
            //    lst5.Selected = true;
            //}
            //ddlrange.Items.Add(lst5);

            //RadComboBoxItem lst6 = new RadComboBoxItem();
            //lst6.Text = "Last Year";
            //lst6.Value = "YY-1";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "YY-1")
            //{
            //    lst6.Selected = true;
            //}
            //ddlrange.Items.Add(lst6);

            //RadComboBoxItem lst7 = new RadComboBoxItem();
            //lst7.Text = "Date Range";
            //lst7.Value = "4";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "4")
            //{
            //    lst7.Selected = true;
            //}
            //ddlrange.Items.Add(lst7);

            //RadComboBoxItem lst8 = new RadComboBoxItem();
            //lst8.Text = "This Month";
            //lst8.Value = "MM0";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "MM0")
            //{
            //    lst8.Selected = true;
            //}
            //ddlrange.Items.Add(lst8);

            //RadComboBoxItem lst9 = new RadComboBoxItem();
            //lst9.Text = "Last One Month";
            //lst9.Value = "MM-1";
            //if (ViewState["SelectedDate"] != null && common.myStr(ViewState["SelectedDate"]) == "MM-1")
            //{
            //    lst9.Selected = true;
            //}
            //ddlrange.Items.Add(lst9);

            //if (ddlrange.SelectedValue == "4")
            //{
            //    tblDateRange.Visible = true;
            //}
            //else
            //{
            //    tblDateRange.Visible = false;
            //}

            #endregion
            #region Facility
            ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);

            ddlLocation.DataSource = ds.Tables[0];
            ddlLocation.DataTextField = "FacilityName";
            ddlLocation.DataValueField = "FacilityID";
            ddlLocation.DataBind();

            ddlLocation.SelectedIndex = 0;
            ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
            #endregion

            #region Ward
            ds = objadt.GetReportTypes(common.myInt(Session["HospitalLocationId"]), "Ward", common.myInt(Session["FacilityId"]));
            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "Name";
            ddlWard.DataValueField = "ID";
            ddlWard.DataBind();
            ddlWard.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlWard.SelectedIndex = 0;
            #endregion
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
            objMaster = null;
            objadt = null;
        }


    }

    //protected void bindSubDepartment()
    //{
    //    BaseC.clsLISMaster objLis = new BaseC.clsLISMaster(sConString);
    //    DataSet ds = new DataSet();
    //    int stationid = 0;

    //    try
    //    {
    //        if (common.myInt(Session["StationId"]) > 0 && common.myInt(Session["ModuleId"]).Equals(33))
    //        {
    //            stationid = common.myInt(Session["StationId"]);

    //            ds = objLis.GetSubDepartment(stationid, 0);
    //        }
    //        else
    //        {
    //            ds = objLis.getRISSubDepartment(common.myInt(Session["FacilityId"]));
    //        }
    //        DataView DV = ds.Tables[0].DefaultView;
    //        DV.Sort = "SubName ASC";
    //        ddlSubDepartment.DataSource = DV.ToTable();
    //        ddlSubDepartment.DataTextField = "SubName";
    //        ddlSubDepartment.DataValueField = "SubDeptId";
    //        ddlSubDepartment.DataBind();
    //        ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("All", "0"));
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlSubDepartment.SelectedIndex = 0;
    //        }

    //        PopulateResourceName();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        objLis = null;
    //        ds.Dispose();
    //    }
    //}


    //private void PopulateResourceName()
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    Hospital hs = new Hospital(sConString);
    //    DataSet ds = new DataSet();
    //    try
    //    {

    //        ds = hs.GetResourceMaster(Convert.ToInt16(common.myInt(Session["HospitalLocationId"])), Convert.ToInt16(common.myInt(ddlLocation.SelectedValue)),
    //                                        1, 0, common.myInt(ddlSubDepartment.SelectedValue));

    //        ddlResource.Items.Clear();
    //        ddlResource.DataSource = null;
    //        ddlResource.DataBind();

    //        foreach (DataRow dr in ds.Tables[0].Rows)
    //        {
    //            RadComboBoxItem item = new RadComboBoxItem();
    //            item.Text = (string)dr["ResourceName"];
    //            item.Value = dr["ResourceId"].ToString();
    //            item.Attributes.Add("SubDeptId", common.myStr(dr["SubDeptId"]));
    //            ddlResource.Items.Add(item);
    //            item.DataBind();
    //        }

    //        common.CheckAllItems(ddlResource);
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        dl = null;
    //        hs = null;
    //        ds.Dispose();
    //    }
    //}

    private void BindData()
    {
        BaseC.WardManagement objWard = new WardManagement();
        DataSet ds = new DataSet();
        try
        {
            if (Session["HospitalLocationID"] != null)
            {
                ViewState["Count"] = 0;
                ViewState["SelectedEncounterId"] = "";
                ViewState["SelectedEnc"] = "";

                string PatientName = string.Empty;
                int RegistrationNo = 0;
                string EncounterNo = string.Empty;
                

                switch (common.myStr(ddlSearchCriteria.SelectedValue))
                {
                    case "R":
                        RegistrationNo = common.myInt(ddlSearchCriteria.Text);
                        break;
                    case "N":
                        PatientName = common.myStr(txtSearch.Text).Trim();
                        break;

                    case "ENC":
                        EncounterNo = common.myStr(txtSearch.Text).Trim();
                        break;
                }
                ds = objWard.getOralGivenDetail(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ddlWard.SelectedValue),
                   RegistrationNo, EncounterNo, PatientName);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        DataRow DR = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(DR);
                    }
                    else
                    {
                        ViewState["Count"] = ds.Tables[0].Rows.Count;
                        DataRow DR = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(DR);
                    }
                    gvOralGivenList.DataSource = ds;
                    gvOralGivenList.DataBind();
                    ViewState["ResourceAppointment"] = null;
                    ViewState["ResourceAppointment"] = ds;
                }
                else
                {
                    gvOralGivenList.DataSource = null;
                    gvOralGivenList.DataBind();
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
            objWard = null;
            ds.Dispose();
        }
    }



    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    BindData();

    //}

    //protected void ddlrange_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    if (ddlrange.SelectedValue == "4")
    //    {
    //        tblDateRange.Visible = true;
    //    }
    //    else
    //    {
    //        tblDateRange.Visible = false;
    //    }
    //    ViewState["SelectedDate"] = ddlrange.SelectedValue;
    //}

    //protected void ddlName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    txtSearch.Text = "";
    //    txtSearchN.Text = "";

    //    if (common.myStr(ddlName.SelectedValue) == "R")
    //    {
    //        txtSearch.Visible = false;
    //        txtSearchN.Visible = true;
    //    }
    //    else
    //    {
    //        txtSearch.Visible = true;
    //        txtSearchN.Visible = false;
    //    }
    //}

    //protected void ddlSubDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //     try
    //    {
    //        PopulateResourceName();

    //        btnSearch_Click(null, null);
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    protected void btnResetFilter_Click(object sender, EventArgs e)
    {
        //txtSearch.Text = string.Empty;
        //txtSearchN.Text = string.Empty;
        ddlWard.SelectedIndex = 0;

        //ddlSubDepartment.SelectedIndex = 0;
        //ddlSource.SelectedIndex = 0;
        //ddlrange.SelectedIndex = ddlrange.Items.IndexOf(ddlrange.Items.FindItemByValue("DD0"));
        //ddlrange_SelectedIndexChanged(this, null);
        //ddlSubDepartment_SelectedIndexChanged(this, null);
    }

    //private string[] getToFromDate(string ddlTime)
    //{
    //    int timezone = BindUTCTime();
    //    string sFromDate = "", sToDate = "";
    //    string[] str = new string[2];

    //    if (ddlTime == "4")
    //    {
    //        //tdDateRange.Visible = true;
    //        if (dtpfromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
    //        {
    //            //sFromDate = Convert.ToDateTime(dtpfromDate.SelectedDate).ToString("yyyy-MM-dd") + " 00:00";
    //            //sToDate = Convert.ToDateTime(dtpToDate.SelectedDate).ToString("yyyy-MM-dd") + " 23:59";

    //            sFromDate = common.myStr(Convert.ToDateTime(dtpfromDate.SelectedDate.Value).ToString("yyyy-MM-dd"));
    //            sToDate = common.myStr(dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
    //        }
    //        str[0] = sFromDate;
    //        str[1] = sToDate;
    //    }
    //    else if (ddlTime == "YD")
    //    {
    //        sFromDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " 00:00";
    //        sToDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " 23:59";
    //        str[0] = sFromDate;
    //        str[1] = sToDate;
    //    }
    //    else if (ddlTime == "WW-1")
    //    {
    //        sFromDate = DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd") + " 00:00";
    //        sToDate = DateTime.Now.AddDays(0).ToString("yyyy/MM/dd") + " 23:59";
    //        str[0] = sFromDate;
    //        str[1] = sToDate;
    //    }
    //    else if (ddlTime == "WW0")
    //    {
    //        str = datecalculate();

    //    }
    //    else if (ddlTime == "MM0")
    //    {
    //        sFromDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01" + " 00:00";
    //        sToDate = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + " 23:59";

    //        str[0] = sFromDate;
    //        str[1] = sToDate;
    //    }
    //    else if (ddlTime == "MM-6")
    //    {
    //        sFromDate = DateTime.Now.AddMonths(-6).ToString("yyyy/MM/dd") + " 00:00";
    //        sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month)) + " 23:59";

    //        str[0] = sFromDate;
    //        str[1] = sToDate;
    //    }
    //    else if (ddlTime == "YY-1")
    //    {
    //        sFromDate = DateTime.Now.AddDays(-365).ToString("yyyy/MM/dd") + " 00:00";
    //        sToDate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 23:59";

    //        str[0] = sFromDate;
    //        str[1] = sToDate;
    //    }
    //    else if (ddlTime == "MM-1")
    //    {
    //        if ((DateTime.Now.Month - 1) != 0)
    //        {
    //            sFromDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/01" + " 00:00";
    //            sToDate = DateTime.Now.Year.ToString() + "/" + (DateTime.Now.Month - 1).ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, (DateTime.Now.Month - 1)) + " 23:59";
    //        }
    //        else
    //        {
    //            sFromDate = (DateTime.Now.Year - 1).ToString() + "/" + 12.ToString() + "/01" + " 00:00";
    //            sToDate = (DateTime.Now.Year - 1).ToString() + "/" + 12.ToString() + "/" + DateTime.DaysInMonth((DateTime.Now.Year - 1), 12) + " 23:59";

    //        }
    //        str[0] = sFromDate;
    //        str[1] = sToDate;
    //    }
    //    return str;
    //}


    //private string[] datecalculate()
    //{
    //    string DayName = DateTime.Now.DayOfWeek.ToString();
    //    string fromdate = "";
    //    string todate = "";

    //    string[] str = new string[2];

    //    switch (DayName)
    //    {
    //        case "Monday":
    //            fromdate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 00:00";
    //            todate = DateTime.Now.AddDays(6).ToString("yyyy/MM/dd") + " 23:59";
    //            break;
    //        case "Tuesday":
    //            fromdate = DateTime.Now.AddDays(-1).Date.ToString("yyyy/MM/dd") + " 00:00";
    //            todate = DateTime.Now.AddDays(5).ToString("yyyy/MM/dd") + " 23:59";
    //            break;
    //        case "Wednesday":
    //            fromdate = DateTime.Now.AddDays(-2).Date.ToString("yyyy/MM/dd") + " 00:00";
    //            todate = DateTime.Now.AddDays(4).ToString("yyyy/MM/dd") + " 23:59";
    //            break;
    //        case "Thursday":
    //            fromdate = DateTime.Now.AddDays(-3).Date.ToString("yyyy/MM/dd") + " 00:00";
    //            todate = DateTime.Now.AddDays(3).ToString("yyyy/MM/dd") + " 23:59";
    //            break;
    //        case "Friday":
    //            fromdate = DateTime.Now.AddDays(-4).Date.ToString("yyyy/MM/dd") + " 00:00";
    //            todate = DateTime.Now.AddDays(2).ToString("yyyy/MM/dd") + " 23:59";
    //            break;
    //        case "Saturday":
    //            fromdate = DateTime.Now.AddDays(-5).Date.ToString("yyyy/MM/dd") + " 00:00";
    //            todate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + " 23:59";
    //            break;
    //        case "Sunday":
    //            fromdate = DateTime.Now.AddDays(-6).Date.ToString("yyyy/MM/dd") + " 00:00";
    //            todate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59";
    //            break;
    //    }

    //    str[0] = fromdate;
    //    str[1] = todate;
    //    return str;
    //}

    //protected int BindUTCTime()
    //{
    //    int timezone = 0;
    //    try
    //    {
    //        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        Hashtable hsinput = new Hashtable();
    //        hsinput.Add("@intfacilityid", common.myInt(Session["FacilityID"]));
    //        string strtimezone = "Select TimeZoneOffSetMinutes  from FacilityMaster where FacilityID=@intfacilityid";
    //        DataSet ds = new DataSet();
    //        ds = dl.FillDataSet(CommandType.Text, strtimezone, hsinput);
    //        timezone = common.myInt(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"]);
    //    }
    //    catch (Exception Ex)
    //    {
    //        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    return timezone;
    //}

    protected void ddlWard_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindData();
    }
}