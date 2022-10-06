using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Telerik.Web.UI;
using System.Configuration;

public partial class ATD_BedStatus : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    String bedStatus = "";
    Image img = new Image();
    BaseC.ATD Objstatus;
    DataSet ds;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.EMRBilling objBed;
    private const int ItemsPerRequest = 50;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["CF"]) == "ER")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bindbedcategory();
            bindWard();
            BindStatus();
            fillData();
            BindContextMenu1();
            BindContextMenu2();
            grvBedStatus.Enabled = false;
        }
    }
    private void bindWard()
    {
        DataSet ds = new DataSet();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
            ddlWard.DataSource = ds.Tables[0];

            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlWard.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objadt = null;
            ds.Dispose();
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

                ddlBedStatus.Items.Add(lst);
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void rdobeddetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (rdobeddetails.SelectedValue == "1")
        //{
        //    bed.Visible = true;
        //    ward.Visible = false;
        //    Bindbedcategory();
        //    // Showbedstatus();
        //}
        //else if (rdobeddetails.SelectedValue == "2")
        //{
        //    ///lblWard.Text = "Ward / Floor";
        //    bed.Visible = false;
        //    ward.Visible = true;
        //    Bindfloorward();
        //    //Showbedstatus();
        //}
        //else if (rdobeddetails.SelectedValue == "3")
        //{
        //    bed.Visible = true;
        //    ward.Visible = false;
        //    Bindbedcategory();
        //    // Show_Beds_all();
        //}
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
        if (common.myInt(rbtnlstVIP.SelectedValue) > 0)
        {
            DataView dvv = ds.Tables[0].DefaultView;
            dvv.RowFilter = "PatientName like '%*VIP%'";
            data = dvv.ToTable();
            dvv.RowFilter = "";
            dvv.Dispose();
        }
        else
        {
            data = ds.Tables[0];
        }
        return data;
    }
    protected void lbtnShowAll_OnClick(object sender, EventArgs e)
    {
        rbtnlstVIP.SelectedIndex = 0;
        rbtnlstVIP.SelectedValue = "0";
        hdnRegId.Value = "0";
        ddlBedStatus.SelectedValue = "0";
        ddlbedcategory.SelectedValue = "0";
        ddlPatient.Items.Clear();
        ddlPatient.Text = "";
        fillData();
    }
    protected void Bindbedcategory()
    {
        try
        {
            ddlbedcategory.Items.Clear();
            BaseC.EMRBilling objBed = new BaseC.EMRBilling(sConString);
            DataSet ds = new DataSet();
            ds = objBed.GetBedCategory(common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbedcategory.DataSource = ds;
                ddlbedcategory.DataTextField = "BedCategoryName";
                ddlbedcategory.DataValueField = "BedCategoryId";
                ddlbedcategory.DataBind();
            }
            ddlbedcategory.Items.Insert(0, new RadComboBoxItem("All", ""));
            ddlbedcategory.SelectedIndex = 0;
            //ddlbedcategory.Items.Insert(0, "ALL");
            //ddlbedcategory.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlbedcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillData();

        //Showbedstatus();
    }
    protected void ddlwardfloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillData();
        //Showbedstatus();
    }
    protected void Show_Beds_all()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataTable dt = new DataTable();

            DataSet objds = new DataSet();
            int i = 0, noOfColumn = 10;
            DataRow dr1;
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@bedCat", ddlbedcategory.SelectedValue);
            hsinput.Add("@wardno", "0");
            hsinput.Add("@inyHospitalLocationId", common.myInt(Session["Hospitallocationid"]));
            hsinput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            objds = dl.FillDataSet(CommandType.StoredProcedure, "IpAtdBedStatusWardwise", hsinput);
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

            for (int k = 0; objds.Tables[0].Rows.Count != k; k++)
            {
                dr1 = dt.NewRow();
                int l = 0;
                for (int j = 0; j != noOfColumn; j++)
                {

                    if (objds.Tables[0].Rows.Count == k)
                    {
                        break;
                    }
                    else
                    {
                        dr1[j] = Convert.ToString(objds.Tables[0].Rows[k]["BedNo"]) + "/" + Convert.ToString(objds.Tables[0].Rows[k]["BedStatus"]) + "/" + Convert.ToString(objds.Tables[0].Rows[k]["RegistrationId"]) + "/" + Convert.ToString(objds.Tables[0].Rows[k]["EncounterNo"]);
                        l++;
                        k++;
                    }

                    if (l == noOfColumn)
                    {
                        k--;
                        break;

                    }

                }
                dt.Rows.Add(dr1);
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
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlbedstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillData();
    }
    public void bindcountbed(DataTable dt)
    {
        try
        {
            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            dt.DefaultView.RowFilter = "BedStatus = 'V'";
            lblVaccant.Text = "Vacant (" + dt.DefaultView.Count + ")";
            dt.DefaultView.RowFilter = "";
            dt.DefaultView.RowFilter = "BedStatus = 'O'";
            lblOccupied.Text = "Occupied (" + dt.DefaultView.Count + ")";
            dt.DefaultView.RowFilter = "";
            dt.DefaultView.RowFilter = "BedStatus = 'H'";
            lblHouseKeeping.Text = "House Keeping (" + dt.DefaultView.Count + ")";
            dt.DefaultView.RowFilter = "";
            dt.DefaultView.RowFilter = "BedStatus = 'R'";
            lblRetain.Text = "Retain (" + dt.DefaultView.Count + ")";
            dt.DefaultView.RowFilter = "";
            dt.DefaultView.RowFilter = "BedStatus = 'U'";
            lblUnderRepair.Text = "Under Repair (" + dt.DefaultView.Count + ")";
            dt.DefaultView.RowFilter = "";
            dt.DefaultView.RowFilter = "BedStatus = 'B'";
            lblBlocked.Text = "Blocked (" + dt.DefaultView.Count + ")";
            dt.DefaultView.RowFilter = "BedStatus = 'I'";
            lblDischargedApproved.Text = "Discharge Approval (" + dt.DefaultView.Count + ")";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public void fillData()
    {
        try
        {
            BaseC.ATD Objstatus = new BaseC.ATD(sConString);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataTable dt = new DataTable();

            DataSet objds = new DataSet();
            int i = 0, noOfColumn = 10;
            DataRow dr1 = null;
            string wardname = "";
            DataSet ds = new DataSet();
            Hashtable hsinput = new Hashtable();
            int RegId = common.myInt(hdnRegId.Value);

            ds = Objstatus.FillBedStatus(common.myInt(ddlbedcategory.SelectedValue), common.myStr(ddlBedStatus.SelectedValue), common.myInt(ddlWard.SelectedValue),
                                        string.Empty, 0, common.myInt(Session["FacilityId"]), common.myInt(Session["HospitallocationId"]));

            DataView dv = new DataView(ds.Tables[0]);

            if (RegId != 0)
            {
                dv.RowFilter = "RegistrationId=" + RegId;
            }
            if (common.myInt(rbtnlstVIP.SelectedValue) > 0)
            {
                dv.RowFilter = "Patient_Name like '%*VIP%'";
            }
            objds.Tables.Add(dv.ToTable());

            bindcountbed(objds.Tables[0]);
            lblNoOfRequest.Text = "(" + common.myStr(ds.Tables[1].Rows[0][0]) + ")";
            ds.Dispose();
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
                            wardname = Convert.ToString(objds.Tables[0].Rows[k]["WardName"]);
                            dr1[0] = wardname;
                            dt.Rows.Add(dr1);
                            l = 0;
                            dr1 = dt.NewRow();
                            dr1[l] = Convert.ToString(objds.Tables[0].Rows[k]["BedNo"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["BedStatus"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["RegistrationNo"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["EncounterNo"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["wardNo"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["BedCatg"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["CompanyType"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["Patient_Name"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["EncounterStatusColor"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["EncounterStatusID"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["WardName"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedCategoryName"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BlockedRemark"])
                                   ; l++;
                            if (objds.Tables[0].Rows.Count == k + 1)
                            {
                                dt.Rows.Add(dr1);
                            }
                            break;

                        }
                        else if (wardname == Convert.ToString(objds.Tables[0].Rows[k]["WardName"]))
                        {

                            if (l == noOfColumn)
                            {
                                k--;
                                dt.Rows.Add(dr1);
                                dr1 = dt.NewRow();
                                l = 0;
                                break;
                            }


                            dr1[l] = Convert.ToString(objds.Tables[0].Rows[k]["BedNo"]) + "@"
                                       + Convert.ToString(objds.Tables[0].Rows[k]["BedStatus"]) + "@"
                                       + Convert.ToString(objds.Tables[0].Rows[k]["RegistrationNo"]) + "@"
                                       + Convert.ToString(objds.Tables[0].Rows[k]["EncounterNo"]) + "@"
                                       + Convert.ToString(objds.Tables[0].Rows[k]["wardNo"]) + "@"
                                       + Convert.ToString(objds.Tables[0].Rows[k]["BedCatg"]) + "@"
                                       + Convert.ToString(objds.Tables[0].Rows[k]["CompanyType"])
                                       + "@" + Convert.ToString(objds.Tables[0].Rows[k]["Patient_Name"])
                                       + "@" + Convert.ToString(objds.Tables[0].Rows[k]["EncounterStatusColor"])
                                       + "@" + Convert.ToString(objds.Tables[0].Rows[k]["EncounterStatusID"])
                                       + "@" + Convert.ToString(objds.Tables[0].Rows[k]["WardName"])
                                       + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedCategoryName"])
                                       + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BlockedRemark"])
                                       ;
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
                            dr1[l] = Convert.ToString(objds.Tables[0].Rows[k]["BedNo"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["BedStatus"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["RegistrationNo"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["EncounterNo"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["wardNo"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["BedCatg"]) + "@"
                                   + Convert.ToString(objds.Tables[0].Rows[k]["CompanyType"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["Patient_Name"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["EncounterStatusColor"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["EncounterStatusID"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["WardName"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedCategoryName"])
                                   + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BlockedRemark"])
                                   ;
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
            lblmsg.Text = "";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void grvBedStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int noOfColumn = Convert.ToInt32(ViewState["noOfColumn"]);
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

                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);

                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair


                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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


                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];

                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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

                            if (str[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair


                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");
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


                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }

                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair




                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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

                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair


                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");
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

                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton3.ToolTip = str2[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair


                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                imgbutton3.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "');");
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

                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }

                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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

                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }

                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair


                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");
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

                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton3.ToolTip = str2[12];
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                imgbutton3.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "');");
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

                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton4.ToolTip = str3[12];
                            }

                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair

                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                imgbutton4.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "');");
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

                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair

                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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

                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }

                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");

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

                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton3.ToolTip = str2[12];
                            }

                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair

                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                imgbutton3.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "');");
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

                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton4.ToolTip = str3[12];
                            }

                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair

                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                imgbutton4.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "');");
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


                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");

                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton5.ToolTip = str4[12];
                            }

                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair

                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                imgbutton5.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "');");
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

                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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

                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair


                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");
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

                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton3.ToolTip = str2[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                imgbutton3.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "');");
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

                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
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
                                imgbutton4.ToolTip = str3[12];
                            }

                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                imgbutton4.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "');");
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

                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton5.ToolTip = str4[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                imgbutton5.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "');");
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

                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton6.ToolTip = str5[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                imgbutton6.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "');");
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

                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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

                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");
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

                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton3.ToolTip = str2[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                imgbutton3.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "');");
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

                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton4.ToolTip = str3[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                imgbutton4.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "');");
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

                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton5.ToolTip = str4[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                imgbutton5.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "');");
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

                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton6.ToolTip = str5[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                imgbutton6.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "');");
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

                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");

                            }
                            else if (str6[1] == "R")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                            }
                            else if (str6[1] == "H")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
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
                                imgbutton7.ToolTip = str6[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str6[1] == "U")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno7.Value = str6[2];
                                hdnencounterno7.Value = str6[3];
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                                imgbutton7.Attributes.Add("OnClick", "showMenu2(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[2] + "','" + str6[3] + "');");

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

                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                        div.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {

                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");
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

                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton3.ToolTip = str2[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                imgbutton3.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "');");
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

                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton4.ToolTip = str3[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                imgbutton4.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "');");
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

                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton5.ToolTip = str4[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                imgbutton5.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "');");
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

                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton6.ToolTip = str5[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                imgbutton6.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "');");
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

                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "');");
                            }
                            else if (str6[1] == "R")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                            }
                            else if (str6[1] == "H")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
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
                                imgbutton7.ToolTip = str6[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str6[1] == "U")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno7.Value = str6[2];
                                hdnencounterno7.Value = str6[3];
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                                imgbutton7.Attributes.Add("OnClick", "showMenu2(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[2] + "','" + str6[3] + "');");

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

                        if (label8.Text != "")
                        {
                            if (str7[1] == "V")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");

                            }
                            else if (str7[1] == "R")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                            }
                            else if (str7[1] == "H")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "I")
                            {
                                //imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton8.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                            }
                            else if (str7[1] == "B")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton8.ToolTip = str7[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str7[1] == "U")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno8.Value = str7[2];
                                hdnencounterno8.Value = str7[3];
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                                imgbutton8.Attributes.Add("OnClick", "showMenu2(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[2] + "','" + str7[3] + "');");
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

                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                        div.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {

                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                //imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");
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

                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton3.ToolTip = str2[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                imgbutton3.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "');");
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

                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton4.ToolTip = str3[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                imgbutton4.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "');");
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

                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton5.ToolTip = str4[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                imgbutton5.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "');");
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

                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton6.ToolTip = str5[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                imgbutton6.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "');");
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

                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "');");
                            }
                            else if (str6[1] == "R")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                            }
                            else if (str6[1] == "H")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
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
                                imgbutton7.ToolTip = str6[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str6[1] == "U")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno7.Value = str6[2];
                                hdnencounterno7.Value = str6[3];
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                                imgbutton7.Attributes.Add("OnClick", "showMenu2(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[2] + "','" + str6[3] + "');");

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

                        if (label8.Text != "")
                        {
                            if (str7[1] == "V")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");

                            }
                            else if (str7[1] == "R")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                            }
                            else if (str7[1] == "H")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "I")
                            {
                                //imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton8.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                            }
                            else if (str7[1] == "B")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton8.ToolTip = str7[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str7[1] == "U")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno8.Value = str7[2];
                                hdnencounterno8.Value = str7[3];
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                                imgbutton8.Attributes.Add("OnClick", "showMenu2(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[2] + "','" + str7[3] + "');");
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

                        if (label9.Text != "")
                        {
                            if (str8[1] == "V")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "R")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);
                            }
                            else if (str8[1] == "H")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "I")
                            {
                                //imgbutton9.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton9.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);
                            }
                            else if (str8[1] == "B")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton9.ToolTip = str8[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str8[1] == "U")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno9.Value = str8[2];
                                hdnencounterno9.Value = str8[3];
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);
                                imgbutton9.Attributes.Add("OnClick", "showMenu2(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[2] + "','" + str8[3] + "');");

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

                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "R")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "H")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[1] + "','" + str[2] + "','" + str[3] + "');");
                            }
                            else if (str[1] == "I")
                            {
                                //imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                            }
                            else if (str[1] == "B")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton1.ToolTip = str[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str[1] == "U")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno1.Value = str[2];
                                hdnencounterno1.Value = str[3];
                                imgbutton1.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str[2] + " , Name " + str[7] + ", Ward Name : " + str[10] + ", BedCategory Name : " + str[11]);
                                imgbutton1.Attributes.Add("OnClick", "showMenu2(event,'" + str[4] + "','" + str[5] + "','" + str[0] + "','" + str[2] + "','" + str[3] + "');");
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
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div1");
                        div.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {

                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "R")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "H")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[1] + "','" + str1[2] + "','" + str1[3] + "');");
                            }
                            else if (str1[1] == "I")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/patient-bed.jpg";//"~/Images/Bed/bed3.jpg";
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                            }
                            else if (str1[1] == "B")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton2.ToolTip = str1[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str1[1] == "U")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno2.Value = str1[2];
                                hdnencounterno2.Value = str1[3];
                                imgbutton2.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str1[2] + " , Name " + str1[7] + ", Ward Name : " + str1[10] + ", BedCategory Name : " + str1[11]);
                                imgbutton2.Attributes.Add("OnClick", "showMenu2(event,'" + str1[4] + "','" + str1[5] + "','" + str1[0] + "','" + str1[2] + "','" + str1[3] + "');");
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

                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");

                            }
                            else if (str2[1] == "R")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "H")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[1] + "','" + str2[2] + "','" + str2[3] + "');");
                            }
                            else if (str2[1] == "I")
                            {
                                //imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                            }
                            else if (str2[1] == "B")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton3.ToolTip = str2[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str2[1] == "U")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno3.Value = str2[2];
                                hdnencounterno3.Value = str2[3];
                                imgbutton3.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str2[2] + " , Name " + str2[7] + ", Ward Name : " + str2[10] + ", BedCategory Name : " + str2[11]);
                                imgbutton3.Attributes.Add("OnClick", "showMenu2(event,'" + str2[4] + "','" + str2[5] + "','" + str2[0] + "','" + str2[2] + "','" + str2[3] + "');");
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

                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "R")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "H")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[1] + "','" + str3[2] + "','" + str3[3] + "');");
                            }
                            else if (str3[1] == "I")
                            {
                                //imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                            }
                            else if (str3[1] == "B")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton4.ToolTip = str3[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str3[1] == "U")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair

                            else
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno4.Value = str3[2];
                                hdnencounterno4.Value = str3[3];
                                imgbutton4.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str3[2] + " , Name " + str3[7] + ", Ward Name : " + str3[10] + ", BedCategory Name : " + str3[11]);
                                imgbutton4.Attributes.Add("OnClick", "showMenu2(event,'" + str3[4] + "','" + str3[5] + "','" + str3[0] + "','" + str3[2] + "','" + str3[3] + "');");
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

                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "R")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "H")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[1] + "','" + str4[2] + "','" + str4[3] + "');");
                            }
                            else if (str4[1] == "I")
                            {
                                //imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                            }
                            else if (str4[1] == "B")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton5.ToolTip = str4[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str4[1] == "U")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno5.Value = str4[2];
                                hdnencounterno5.Value = str4[3];
                                imgbutton5.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str4[2] + " , Name " + str4[7] + ", Ward Name : " + str4[10] + ", BedCategory Name : " + str4[11]);
                                imgbutton5.Attributes.Add("OnClick", "showMenu2(event,'" + str4[4] + "','" + str4[5] + "','" + str4[0] + "','" + str4[2] + "','" + str4[3] + "');");
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

                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "R")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "H")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[1] + "','" + str5[2] + "','" + str5[3] + "');");
                            }
                            else if (str5[1] == "I")
                            {
                                //imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                            }
                            else if (str5[1] == "B")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton6.ToolTip = str5[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str5[1] == "U")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno6.Value = str5[2];
                                hdnencounterno6.Value = str5[3];
                                imgbutton6.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str5[2] + " , Name " + str5[7] + ", Ward Name : " + str5[10] + ", BedCategory Name : " + str5[11]);
                                imgbutton6.Attributes.Add("OnClick", "showMenu2(event,'" + str5[4] + "','" + str5[5] + "','" + str5[0] + "','" + str5[2] + "','" + str5[3] + "');");
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

                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "');");
                            }
                            else if (str6[1] == "R")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                            }
                            else if (str6[1] == "H")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[1] + "','" + str6[2] + "','" + str6[3] + "');");
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
                                imgbutton7.ToolTip = str6[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str6[1] == "U")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno7.Value = str6[2];
                                hdnencounterno7.Value = str6[3];
                                imgbutton7.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str6[2] + " , Name " + str6[7] + ", Ward Name : " + str6[10] + ", BedCategory Name : " + str6[11]);
                                imgbutton7.Attributes.Add("OnClick", "showMenu2(event,'" + str6[4] + "','" + str6[5] + "','" + str6[0] + "','" + str6[2] + "','" + str6[3] + "');");

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

                        if (label8.Text != "")
                        {
                            if (str7[1] == "V")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");

                            }
                            else if (str7[1] == "R")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                            }
                            else if (str7[1] == "H")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton8.Attributes.Add("OnClick", "showMenu(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[1] + "','" + str7[2] + "','" + str7[3] + "');");
                            }
                            else if (str7[1] == "I")
                            {
                                //imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton8.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                            }
                            else if (str7[1] == "B")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton8.ToolTip = str7[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str7[1] == "U")
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton8.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno8.Value = str7[2];
                                hdnencounterno8.Value = str7[3];
                                imgbutton8.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str7[2] + " , Name " + str7[7] + ", Ward Name : " + str7[10] + ", BedCategory Name : " + str7[11]);
                                imgbutton8.Attributes.Add("OnClick", "showMenu2(event,'" + str7[4] + "','" + str7[5] + "','" + str7[0] + "','" + str7[2] + "','" + str7[3] + "');");
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

                        if (label9.Text != "")
                        {
                            if (str8[1] == "V")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "R")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);
                            }
                            else if (str8[1] == "H")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton9.Attributes.Add("OnClick", "showMenu(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[1] + "','" + str8[2] + "','" + str8[3] + "');");
                            }
                            else if (str8[1] == "I")
                            {
                                //imgbutton9.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton9.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);
                            }
                            else if (str8[1] == "B")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton9.ToolTip = str8[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str8[1] == "U")
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton9.ImageUrl = "~/Images/Bed/bed3.jpg";
                                hdnregno9.Value = str8[2];
                                hdnencounterno9.Value = str8[3];
                                imgbutton9.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str8[2] + " , Name " + str8[7] + ", Ward Name : " + str8[10] + ", BedCategory Name : " + str8[11]);
                                imgbutton9.Attributes.Add("OnClick", "showMenu2(event,'" + str8[4] + "','" + str8[5] + "','" + str8[0] + "','" + str8[2] + "','" + str8[3] + "');");

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

                        if (label10.Text != "")
                        {
                            if (str9[1] == "V")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton10.Attributes.Add("OnClick", "showMenu(event,'" + str9[4] + "','" + str9[5] + "','" + str9[0] + "','" + str9[1] + "','" + str9[2] + "','" + str9[3] + "');");
                            }
                            else if (str9[1] == "R")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/Retain-bed.JPG";
                                imgbutton10.Attributes.Add("OnClick", "showMenu(event,'" + str9[4] + "','" + str9[5] + "','" + str9[0] + "','" + str9[1] + "','" + str9[2] + "','" + str9[3] + "');");
                                imgbutton10.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str9[2] + " , Name " + str9[7] + ", Ward Name : " + str9[10] + ", BedCategory Name : " + str9[11]);
                            }
                            else if (str9[1] == "H")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/Housekeeping.JPG";
                                imgbutton10.Attributes.Add("OnClick", "showMenu(event,'" + str9[4] + "','" + str9[5] + "','" + str9[0] + "','" + str9[1] + "','" + str9[2] + "','" + str9[3] + "');");
                            }
                            else if (str9[1] == "I")
                            {
                                //imgbutton10.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton10.ImageUrl = "~/Images/Bed/patient-bed.jpg";
                                imgbutton10.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str9[2] + " , Name " + str9[7] + ", Ward Name : " + str9[10] + ", BedCategory Name : " + str9[11]);
                            }
                            else if (str9[1] == "B")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/bedonly.JPG";
                                imgbutton10.ToolTip = str9[12];
                            }
                            //Added on 26-08-2014 Start for Under Repaiir
                            else if (str9[1] == "U")
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/bedUnderRepair.JPG";

                            }
                            //Added on 26-08-2014 End  for Under Repair
                            else
                            {
                                imgbutton10.ImageUrl = "~/Images/Bed/bed3.jpg";

                                hdnregno10.Value = str9[2];
                                hdnencounterno10.Value = str9[3];
                                imgbutton10.ToolTip = Convert.ToString(lblRegResource.Text + " : " + str9[2] + " , Name " + str9[7] + ", Ward Name : " + str9[10] + ", BedCategory Name : " + str9[11]);
                                imgbutton10.Attributes.Add("OnClick", "showMenu2(event,'" + str9[4] + "','" + str9[5] + "','" + str9[0] + "','" + str9[2] + "','" + str9[3] + "');");
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
    protected void RadContextMenu1_ItemClick(object sender, RadMenuEventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (RadContextMenu1.SelectedValue == "AD")
            {
                if (txtBedstatus.Text != "R" && txtBedstatus.Text != "H")
                {
                    if (common.myStr(Request.QueryString["CF"]) == "ER")
                        RadWindowForNew.NavigateUrl = "/ATD/Admission.aspx?bedno=" + lblbeno.Text.Trim() + "&wardno=" + lblwardno.Text.Trim() + "&bedcat=" + lblbedcetogry.Text.Trim() + "&NewAd=N&Regno=" + common.myStr(Request.QueryString["RNo"]) + "&RegId=" + common.myStr(Request.QueryString["RId"]) + "&CF=" + common.myStr(Request.QueryString["CF"]) + "&EREncounterId=" + common.myStr(Request.QueryString["EId"]);
                    else
                        RadWindowForNew.NavigateUrl = "/ATD/Admission.aspx?bedno=" + lblbeno.Text.Trim() + "&wardno=" + lblwardno.Text.Trim() + "&bedcat=" + lblbedcetogry.Text.Trim() + "&NewAd=N";

                    RadWindowForNew.Height = 660;
                    RadWindowForNew.Width = 1060;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                }
                else
                {
                    Alert.ShowAjaxMsg("Bed Already allocated to another Patient!", Page);
                    return;
                }
            }
            else if (RadContextMenu1.SelectedValue == "BR")
            {

                RadWindowForNew.NavigateUrl = "/ATD/BedRetainRelease.aspx?bedno=" + lblbeno.Text.Trim() + "&wardno=" + lblwardno.Text.Trim() + "&bedcat=" + lblbedcetogry.Text.Trim() + "";
                RadWindowForNew.Height = 525;
                RadWindowForNew.Width = 950;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
            else if (RadContextMenu1.SelectedValue == "BRL")
            {
                RadWindowForNew.NavigateUrl = "/ATD/BedRelease.aspx?bedno=" + lblbeno.Text.Trim() + "&wardno=" + lblwardno.Text.Trim() + "&bedcat=" + lblbedcetogry.Text.Trim() + "&Regno=" + lblregno.Text.Trim() + "&encno=" + lblencno.Text.Trim() + "";
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 950;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
            else if (RadContextMenu1.SelectedValue == "DSL")
            {
                RadWindowForNew.NavigateUrl = "/ATD/DischargeList.aspx";
                RadWindowForNew.Height = 600;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
            else if (RadContextMenu1.SelectedValue == "PD")
            {
                BaseC.ATD Objstatus = new BaseC.ATD(sConString);


                ds = ds = Objstatus.GetIPPatientDetails(common.myInt(lblregno.Text), common.myStr(lblencno.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblwardname.Text = ds.Tables[0].Rows[0]["WardName"].ToString().Trim();
                    lblbedcategryname.Text = ds.Tables[0].Rows[0]["BedCategoryName"].ToString().Trim();
                    lblbednumber.Text = lblbeno.Text.Trim();
                    lblregistrationno.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString().Trim();
                    lblipno.Text = ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();
                    lblpatientname.Text = ds.Tables[0].Rows[0]["PName"].ToString().Trim();
                    lblagegender.Text = ds.Tables[0].Rows[0]["AgeGender"].ToString().Trim();
                    lbladmissiondate.Text = ds.Tables[0].Rows[0]["AdmissionDate"].ToString().Trim();
                    lbldoctorname.Text = ds.Tables[0].Rows[0]["DName"].ToString().Trim();
                    lblfacilityname.Text = ds.Tables[0].Rows[0]["FName"].ToString().Trim();
                    lblBedRetain.Text = ds.Tables[0].Rows[0]["BedNo"].ToString().Trim();
                }

                ModalPopupExtender.Show();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void RadContextMenu2_ItemClick(object sender, RadMenuEventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            #region Patient Detail
            ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                common.myStr(lblregno.Text), common.myStr(lblencno.Text), common.myInt(Session["UserId"]), 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["OPIP"] = "I";
                Session["EncounterId"] = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
                Session["RegistrationID"] = common.myInt(ds.Tables[0].Rows[0]["RegistrationId"]);
                Session["EncounterDate"] = common.myStr(ds.Tables[0].Rows[0]["EncounterDate"]);
                Session["DoctorID"] = common.myInt(ds.Tables[0].Rows[0]["DoctorId"].ToString());
                Session["RegistrationNo"] = common.myLong(ds.Tables[0].Rows[0]["RegistrationNo"]);
                Session["EncounterNo"] = lblencno.Text.Trim();

                string sRegNoTitle = Resources.PRegistration.regno;
                string sDoctorTitle = Resources.PRegistration.Doctor;
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
            #endregion

            if (RadContextMenu2.SelectedValue == "BT")
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    RadWindowForNew.NavigateUrl = "/ATD/BedTransfer1.aspx?Regno=" + lblregno.Text.Trim() + "&encno=" + lblencno.Text.Trim() + "&bedno=" + lblbeno.Text.Trim() + "";
                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                }
            }
            else if (RadContextMenu2.SelectedValue == "DT")
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    RadWindowForNew.NavigateUrl = "/ATD/doctortransfer.aspx?Regno=" + lblregno.Text.Trim() + "&encno=" + lblencno.Text.Trim() + "";
                    RadWindowForNew.Height = 525;
                    RadWindowForNew.Width = 850;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                }
            }

            else if (RadContextMenu2.SelectedValue == "DS")
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    RadWindowForNew.NavigateUrl = "/ATD/discharge.aspx?Regno=" + lblregno.Text.Trim() + "&encno=" + lblencno.Text.Trim() + "&MASTER=No";
                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 950;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                }
            }
            else if (RadContextMenu2.SelectedValue == "BR")
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    BaseC.ATD Objstatus = new BaseC.ATD(sConString);
                    if (Objstatus.isBedRetain(common.myStr(lblregno.Text.Trim()), common.myStr(lblencno.Text.Trim())) == true)
                    {
                        RadWindowForNew.NavigateUrl = "/ATD/BedRetainRelease.aspx?bedno=" + lblbeno.Text.Trim() + "&wardno=" + lblwardno.Text.Trim() + "&bedcat=" + lblbedcetogry.Text.Trim() + "&Regno=" + lblregno.Text.Trim() + "&encno=" + lblencno.Text.Trim() + "";
                        RadWindowForNew.Height = 525;
                        RadWindowForNew.Width = 950;
                        RadWindowForNew.Top = 40;
                        RadWindowForNew.Left = 100;
                        RadWindowForNew.OnClientClose = "OnClientClose";
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Patient has already retain a bed", Page.Page);
                        return;
                    }
                }
            }
            else if (RadContextMenu2.SelectedValue == "BRL")
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    RadWindowForNew.NavigateUrl = "/ATD/BedRelease.aspx?bedno=" + lblbeno.Text.Trim() + "&wardno=" + lblwardno.Text.Trim() + "&bedcat=" + lblbedcetogry.Text.Trim() + "&Regno=" + lblregno.Text.Trim() + "&encno=" + lblencno.Text.Trim() + "";
                    RadWindowForNew.Height = 525;
                    RadWindowForNew.Width = 950;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                }
            }
            else if (RadContextMenu2.SelectedValue == "ADU")
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    RadWindowForNew.NavigateUrl = "/ATD/Admission.aspx?Regno=" + lblregno.Text.Trim() + "&encno=" + lblencno.Text.Trim() + "";
                    RadWindowForNew.Height = 625;
                    RadWindowForNew.Width = 1000;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                }
            }

            else if (RadContextMenu2.SelectedValue == "ADC")
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    RadWindowForNew.NavigateUrl = "/ATD/Admission.aspx?Regno=" + lblregno.Text.Trim() + "&encno=" + lblencno.Text.Trim() + "&CF=" + common.myStr(Request.QueryString["CF"]) + "";
                    RadWindowForNew.Height = 625;
                    RadWindowForNew.Width = 950;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                }
            }
            else if (RadContextMenu2.SelectedValue == "PY")
            {
                if (lblregno.Text.Trim() != "" && lblencno.Text.Trim() != "")
                {
                    int i = 0;
                    DataSet ds1 = (DataSet)Session["ModuleData"];
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        if (common.myStr(dr["ModuleName"]) == "Billing")
                        {
                            Session["ModuleId"] = i;
                        }
                        i++;
                    }
                    Response.Redirect("~/EMRBILLING/IPBill.aspx?RegNo=" + lblregno.Text.Trim() + "&EncId=" + common.myInt(Session["EncounterId"]), false);
                }
            }
            else if (RadContextMenu2.SelectedValue == "PD")
            {
                BaseC.ATD Objstatus = new BaseC.ATD(sConString);


                ds = ds = Objstatus.GetIPPatientDetails(common.myInt(lblregno.Text), common.myStr(lblencno.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblwardname.Text = ds.Tables[0].Rows[0]["WardName"].ToString().Trim();
                    lblbedcategryname.Text = ds.Tables[0].Rows[0]["BedCategoryName"].ToString().Trim();
                    lblbednumber.Text = lblbeno.Text.Trim();
                    lblregistrationno.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString().Trim();
                    lblipno.Text = ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();
                    lblpatientname.Text = ds.Tables[0].Rows[0]["PName"].ToString().Trim();
                    lblagegender.Text = ds.Tables[0].Rows[0]["AgeGender"].ToString().Trim();
                    lbladmissiondate.Text = ds.Tables[0].Rows[0]["AdmissionDate"].ToString().Trim();
                    lbldoctorname.Text = ds.Tables[0].Rows[0]["DName"].ToString().Trim();
                    lblfacilityname.Text = ds.Tables[0].Rows[0]["FName"].ToString().Trim();
                    lblBedRetain.Text = ds.Tables[0].Rows[0]["BedNo"].ToString().Trim();
                }

                ModalPopupExtender.Show();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void grvBedStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenField hf = (HiddenField)grvBedStatus.SelectedRow.Cells[grvBedStatus.SelectedIndex].FindControl("hdnregno2");
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void Imagebutton1_OnClick(object sender, EventArgs e)
    {

    }
    protected void grvBedStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string san = "santosh";
    }
    public void data(string e, string f)
    {

    }
    protected void btnfncall_Click(object sender, EventArgs e)
    {
        //data(string e,string f);
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(Session["Otherdet"]) == "adm")
            {
                Response.Redirect("AdmissionDetails.aspx?Regno=" + lblregno.Text.Trim() + "&Ipno=" + lblencno.Text.Trim(), false);
            }
            else
            {
                fillData();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnfindad_Click(object sender, EventArgs e)
    {

    }
    //done by rakesh for user authorisation start
    private void BindContextMenu1()
    {
        CreateDataTableForRadContextMenu1();
        RadContextMenu1.DataSource = (DataTable)ViewState["dtable1"];
        RadContextMenu1.DataTextField = "Text";
        RadContextMenu1.DataValueField = "Value";
        RadContextMenu1.DataBind();
    }
    protected void CreateDataTableForRadContextMenu1()
    {
        UserAuthorisations ua = new UserAuthorisations();
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Text", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));

            DataRow dr;
            if (ua.CheckPermissions("N", "/ATD/Admission.aspx", true))
            {

                dr = dt.NewRow();
                dr["Text"] = "Admission";
                dr["Value"] = "AD";
                dt.Rows.Add(dr);
                //ViewState["dtable"] = dt;
            }
            if (ua.CheckPermissions("N", "/ATD/BedRetainRelease.aspx", true))
            {

                dr = dt.NewRow();
                dr["Text"] = "Bed Retain";
                dr["Value"] = "BR";
                dt.Rows.Add(dr);
                //ViewState["dtable"] = dt;
            }
            if (ua.CheckPermissions("N", "/ATD/BedRelease.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Bed Release";
                dr["Value"] = "BRL";
                dt.Rows.Add(dr);
                //ViewState["dtable"] = dt;
            }
            dr = dt.NewRow();
            dr["Text"] = "Patient Details";
            dr["Value"] = "PD";
            dt.Rows.Add(dr);
            ViewState["dtable1"] = dt;

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ua.Dispose();
        }
    }
    private void BindContextMenu2()
    {
        CreateDataTableForRadContextMenu2();
        RadContextMenu2.DataSource = (DataTable)ViewState["dtable"];
        RadContextMenu2.DataTextField = "Text";
        RadContextMenu2.DataValueField = "Value";
        RadContextMenu2.DataBind();
    }

    protected void CreateDataTableForRadContextMenu2()
    {
        DataTable dt = new DataTable();
        UserAuthorisations ua = new UserAuthorisations();
        DataRow dr;
        try
        {

            dt.Columns.Add(new DataColumn("Text", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));

            if (ua.CheckPermissions("N", "ATD/BedTransfer1.aspx", true))
            {

                dr = dt.NewRow();
                dr["Text"] = "Bed Transfer";
                dr["Value"] = "BT";
                dt.Rows.Add(dr);
                //ViewState["dtable"] = dt;
            }
            if (ua.CheckPermissions("N", "ATD/doctortransfer.aspx", true))
            {
                dr = dt.NewRow();
                dr["Text"] = "Doctor Transfer";
                dr["Value"] = "DT";
                dt.Rows.Add(dr);
                //ViewState["dtable"] = dt;
            }

            if (ua.CheckPermissions("N", "/ATD/BedRetainRelease.aspx", true))
            {

                dr = dt.NewRow();
                dr["Text"] = "Bed Retain";
                dr["Value"] = "BR";
                dt.Rows.Add(dr);
                //ViewState["dtable"] = dt;
            }

            if (ua.CheckPermissions("E", "ATD/Admission.aspx", true))
            {

                dr = dt.NewRow();
                dr["Text"] = "Admission Update";
                dr["Value"] = "ADU";
                dt.Rows.Add(dr);
                //ViewState["dtable"] = dt;
            }
            dr = dt.NewRow();
            dr["Text"] = "Patient Details";
            dr["Value"] = "PD";
            dt.Rows.Add(dr);
            ViewState["dtable"] = dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ua.Dispose();
        }
    }
    //done by rakesh for user authorisation end
    protected void lnkbedTransferReq_OnClick(object sender, EventArgs e)
    {

        RadWindow1.NavigateUrl = "/ATD/BedTransferRequest.aspx?MP=NO";
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

    protected void rbtnlstVIP_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillData();
    }

    protected void ddlWard_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fillData();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

}