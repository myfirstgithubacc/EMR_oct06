using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class MPages_ItemOfService : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();
    BaseC.Patient pt;//= new BaseC.Patient();
    StringBuilder strSQL;
    DataSet ds;
    SqlDataReader dr;
    DAL.DAL dl = new DAL.DAL();
    Hashtable hshIn;
    //Added by rakesh start for tagging with template    
    private bool RowSelStatus = false;
    //Added by rakesh end


    #region Private

    [Serializable]
    private class MergedColumnsInfo
    {
        // indexes of merged columns
        public List<int> MergedColumns = new List<int>();
        // key-value pairs: key = first column index, value = number of merged columns
        public Hashtable StartColumns = new Hashtable();
        // key-value pairs: key = first column index, value = common title of merged columns 
        public Hashtable Titles = new Hashtable();

        //parameters: merged columns's indexes, common title of merged columns 
        public void AddMergedColumns(int[] columnsIndexes, string title)
        {
            MergedColumns.AddRange(columnsIndexes);
            StartColumns.Add(columnsIndexes[0], columnsIndexes.Length);
            Titles.Add(columnsIndexes[0], title);
        }
    }

    //property for storing of information about merged columns
    private MergedColumnsInfo info
    {
        get
        {
            if (ViewState["info"] == null)
                ViewState["info"] = new MergedColumnsInfo();
            return (MergedColumnsInfo)ViewState["info"];

        }
    }

    //method for rendering of columns's headers	
    private void RenderHeader(HtmlTextWriter output, Control container)
    {
        for (int i = 0; i < container.Controls.Count; i++)
        {
            TableCell cell = (TableCell)container.Controls[i];
            //stretch non merged columns for two rows
            if (!info.MergedColumns.Contains(i))
            {
                cell.Attributes["rowspan"] = "2";
                cell.RenderControl(output);
            }
            else //render merged columns's common title
                if (info.StartColumns.Contains(i))
            {
                output.Write(string.Format("<th align='center' colspan='{0}'>{1}</th>",
                         info.StartColumns[i], info.Titles[i]));
            }
        }

        //close first row	
        output.RenderEndTag();
        //set attributes for second row
        gvDefaultServiceCharges.HeaderStyle.AddAttributesToRender(output);
        //start second row
        output.RenderBeginTag("tr");

        //render second row (only merged columns)
        for (int i = 0; i < info.MergedColumns.Count; i++)
        {
            TableCell cell = (TableCell)container.Controls[info.MergedColumns[i]];
            cell.RenderControl(output);
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // lblDepartmentType.Text = "";
            if (!IsPostBack)
            {
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                bindSampleRequired();

                if (Request.QueryString["Mpg"] != null)
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                ltrlEncodedBy.Visible = false;
                ltrlUpdateBy.Visible = false;
                ltrlEncodedByDisplay.Text = "";
                ltrlUpdateByDisplay.Text = "";

                info.AddMergedColumns(new int[] { 4, 5 }, "Charge");
                info.AddMergedColumns(new int[] { 7, 8 }, "Discount %");

                btnsave.Visible = true;
                ibtnUpdate.Visible = false;
                //Commented by rakesh start
                //popMainDept();
                //ddlMainDept_OnSelectedIndexChanged(sender, e);
                //Commented by rakesh end
                populateControls();
                populateSubDeptReportTagging();
                populateConsultationType();
                BindMHCReportFormatId();
                if (Request.QueryString["ServiceId"] != null)
                {
                    btnsave.Visible = false;
                    ibtnUpdate.Visible = true;
                    EnableDisableControls(false);
                    rblStatus.Visible = true;
                    ltrlStatus.Visible = true;
                    ViewState["serviceid"] = Request.QueryString["ServiceId"].ToString();

                    if (Session["deptId"] != null)
                    {
                        ds = dl.FillDataSet(CommandType.Text, " select dm.DepartmentID, dm.Departmentname from departmentmain dm  inner join FacilityWiseDepartment fwd on fwd.DepartmentId = dm.DepartmentID where fwd.FacilityId= " + Session["FacilityId"] + " and   dm.DepartmentID = " + Session["deptId"]);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlMainDept.DataSource = ds;
                            ddlMainDept.DataTextField = "Departmentname";
                            ddlMainDept.DataValueField = "DepartmentID";
                            ddlMainDept.DataBind();
                            ddlMainDept.SelectedValue = ds.Tables[0].Rows[0]["DepartmentID"].ToString();
                        }
                        if (Session["subdept"] != null)
                        {
                            DataSet ds1 = new DataSet();
                            ds1 = dl.FillDataSet(CommandType.Text, "select subdeptid, subname from departmentsub where subdeptid = " + Session["subdept"].ToString());
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                ddlsubDept.DataSource = ds1;
                                ddlsubDept.DataTextField = "subname";
                                ddlsubDept.DataValueField = "subdeptid";
                                ddlsubDept.DataBind();
                                ddlsubDept.SelectedValue = ds1.Tables[0].Rows[0]["subdeptid"].ToString();
                            }
                        }
                    }
                    Bindsdata();

                    //Added by rakesh for tagging with template work start
                    hdnServiceid.Value = common.myStr(common.myInt(Request.QueryString["ServiceId"]));
                    //Added by rakesh for tagging with template work end
                }

                else
                {
                    rblStatus.Visible = false;
                    ltrlStatus.Visible = false;
                    //Added by rakesh for tagging with template work start
                    hdnServiceid.Value = "0";
                    //Added by rakesh for tagging with template work end
                }
                //added by rakesh start
                popMainDept();
                ddlMainDept_OnSelectedIndexChanged(sender, e);
                //added by rakesh end

                chkpnl5SampleRequired.Attributes.Add("onclick", "javascript:Visiblity();");
                // btnReportFormat.Visible = false;
                ViewState["PercentageVariation"] = null;
                popTableDefaultValue();

                divSACCode.Visible = false;
                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isGSTApplicable", sConString).Equals("Y"))
                {
                    divSACCode.Visible = true;
                } 

                txtRefServiceCode.ReadOnly = false;
                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "isServiceRefCodeAutoGenerated", sConString).Equals("Y"))
                {
                    txtRefServiceCode.ReadOnly = true;
                } 
                
                    
            }

            if (Application["IPIncluded"].ToString() == "N")
            {
                pnl6.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void populateSubDeptReportTagging()
    {
        try
        {
            BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
            DataSet ds = master.GetSubDeptReportTagging();
            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Issubdeptgroup=0";

            ddlReportTagging.DataSource = dv.ToTable();
            ddlReportTagging.DataTextField = "TaggingName";
            ddlReportTagging.DataValueField = "TaggingId";
            ddlReportTagging.DataBind();
            ddlReportTagging.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select"));
            ddlReportTagging.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void populateConsultationType()
    {
        try
        {
            BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
            DataSet ds = master.GetConsultationType();

            ddlConsultationType.Items.Clear();
            ddlConsultationType.DataSource = ds.Tables[0];
            ddlConsultationType.DataTextField = "ConsultationType";
            ddlConsultationType.DataValueField = "ConsultationTypeID";
            ddlConsultationType.DataBind();
            ddlConsultationType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select"));
            ddlConsultationType.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


    protected string FormatNumber(object myValue)
    {
        return (Convert.ToDecimal(myValue)).ToString("N2");
    }

    void bindSampleRequired()
    {
        try
        {
            if (Cache["SampleRequiredTable"] == null)
            {
                Cache.Remove("SampleRequiredTable");

                DataSet objDs = new DataSet();

                objDs = dl.FillDataSet(CommandType.Text, "select ' [ Select ] ' as Name , '' as SampleID union Select Name,SampleID from DiagSampleMaster where Active=1 order by name");

                Cache.Insert("SampleRequiredTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                ddlpnl5SampleRequired.DataSource = Cache["SampleRequiredTable"];
                ddlpnl5SampleRequired.DataTextField = "Name";
                ddlpnl5SampleRequired.DataValueField = "SampleID";
                ddlpnl5SampleRequired.DataBind();
            }
            else
            {
                DataSet objDs = (DataSet)Cache["SampleRequiredTable"];
                ddlpnl5SampleRequired.DataSource = objDs;
                ddlpnl5SampleRequired.DataTextField = "Name";
                ddlpnl5SampleRequired.DataValueField = "SampleID";
                ddlpnl5SampleRequired.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void New_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/MPages/ItemOfService.aspx", false);
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Edit_OnClick(Object sender, EventArgs e)
    {
        //Response.Redirect("/MPages/ItemOfService.aspx?Mode=Edit");
        //Page.SetFocus(ddlMainDept);
    }

    protected void btnsave_OnClick(object sender, EventArgs e)
    {

        try
        {

            if (txtTime.Text.Trim() == "")
            {
                txtTime.Text = "15";
            }

            if (spnSACCode.Visible == true && common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsRequiredSACCode", sConString).Equals("Y"))
            {
                if (common.myInt(txtSACCode.Text.Length) == 0)
                {
                    Alert.ShowAjaxMsg("SAC Code Can Not Be Blank..", this);
                    return;
                }
            }

            if (common.myInt(txtTime.Text) > 59)
            {
                Alert.ShowAjaxMsg("Minutes Valid upto 59 only.", this);
                return;
            }
            if (common.myStr(ViewState["DeptFlag"]) == "R" && chkIsBillingCategory.Checked == false && common.myInt(ddlBillingCategory.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please Select Billing Category.", this);
                return;
            }
            DAL.DAL dl;
            String strXMLInvestigation = "";
            String strXMLBedCharge = "";
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL = new StringBuilder();
            using (System.Transactions.TransactionScope Trans = default(System.Transactions.TransactionScope))
            {
                Hashtable hshIn = new Hashtable();
                Hashtable hshOut = new Hashtable();


                hshIn.Add("@Jcode", txtJCode.Text.Trim());
                hshIn.Add("@LONICCode", txtLONICCode.Text.Trim());
                hshIn.Add("@bitCompulsoryonBill", chkCompulsaryonBill.Checked ? 1 : 0);
                hshIn.Add("@intServiceId", common.myInt(ViewState["serviceid"]));
                hshIn.Add("@mEstimatedCost", common.myDbl(txtEstimatedPrice.Text));

                hshIn.Add("@HospitalLocationID", common.myInt(Session["HospitalLocationID"]));
                hshIn.Add("@subdeptid", ddlsubDept.SelectedValue);
                if (txtRefServiceCode.Text == "")
                {
                    hshIn.Add("@RefServiceCode", "");
                }
                else
                {
                    hshIn.Add("@RefServiceCode", bc.ParseQ(txtRefServiceCode.Text));
                }
                hshIn.Add("@ServiceName", bc.ParseQ(txtServiceName.Text));
                hshIn.Add("@Cptcode", txtcptcode.Text.Trim());
                hshIn.Add("@TimeBased", ddlTimeBased.SelectedValue);
                hshIn.Add("@FAAccountId", common.myInt(txtFAAccountId.Text));
                hshIn.Add("@mServiceTax", common.myDbl(txtServiceTax.Text));//commented by santosh
                hshIn.Add("@mVAT", common.myDbl(txtVat.Text));//commented by santosh
                // For Checking the Doctor Required or not 
                //if ((chkpnl1DoctorReq.Checked == true) || (chkPnl2DoctorReq.Checked == true))
                if (((chkpnl1DoctorReq.Checked == true) && (chkpnl1DoctorReq.Visible == true)) || ((chkPnl2DoctorReq.Checked == true) && (chkPnl2DoctorReq.Visible == true)))
                {
                    hshIn.Add("@DoctorReq", "1");
                }
                else
                {
                    hshIn.Add("@DoctorReq", "0");
                }

                // For Checking the Surgery field
                if (ddlPnl3Classification.Enabled == true)
                {
                    hshIn.Add("@intSurgeryClassificationID", common.myInt(ddlPnl3Classification.SelectedValue));
                }
                else
                {
                    hshIn.Add("@intSurgeryClassificationID", 0);
                }

                // Checking wheter price is editable or not
                //if ((chkpnl1PriceEditable.Checked == true) || (chkPnl2PriceEditable.Checked == true) || (chkPnl3PriceEditable.Checked == true))
                if ((chkpnl1PriceEditable.Checked == true) || (chkPnl2PriceEditable.Checked == true))
                {
                    hshIn.Add("@PriceEdit", "1");
                }
                else
                {
                    hshIn.Add("@PriceEdit", "0");
                }

                //// Checking whether price is differ by bedcategory or not
                if (optDifferbybedcategory.SelectedItem.Value == "0")
                {
                    hshIn.Add("@DifferByBed", "0");
                    hshIn.Add("@DifferByPercent", "0");
                }
                else if (optDifferbybedcategory.SelectedItem.Value == "1")
                {
                    hshIn.Add("@DifferByBed", "1");
                    hshIn.Add("@DifferByPercent", "0");
                }
                else if (optDifferbybedcategory.SelectedItem.Value == "2")
                {
                    hshIn.Add("@DifferByBed", "1");
                    hshIn.Add("@DifferByPercent", "1");
                }
                hshIn.Add("@EncodedBy", common.myInt(Session["UserID"]));
                hshIn.Add("@ServiceType", common.myStr(ViewState["DeptFlag"]));

                // For populating the flag related tables
                if (ViewState["DeptFlag"].ToString() == "I")
                {
                    if (pnl5.Visible == true)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ReportType", typeof(String));
                        dt.Columns.Add("Profile", typeof(Int16));
                        dt.Columns.Add("FormatCode", typeof(Int16));
                        dt.Columns.Add("SpecialPrecaution", typeof(String));
                        dt.Columns.Add("ReportDays", typeof(Int16));
                        dt.Columns.Add("ReportDaysWeek", typeof(String));
                        dt.Columns.Add("TestDuration", typeof(String));
                        dt.Columns.Add("TestDays", typeof(String));
                        dt.Columns.Add("SampleRequired", typeof(Int16));
                        dt.Columns.Add("SampleId", typeof(Int16));
                        dt.Columns.Add("PrintSequence", typeof(Int16));
                        dt.Columns.Add("ResourceScheduling", typeof(Int16));
                        DataRow drow = dt.NewRow();
                        drow["ReportType"] = ddlpnl5ReportType.SelectedItem.Value;
                        if (ddlpnl5ReportType.SelectedItem.Value == "P")
                        {
                            drow["Profile"] = "1";
                        }
                        else
                        {
                            drow["Profile"] = "0";
                        }

                        drow["FormatCode"] = DBNull.Value;
                        drow["SpecialPrecaution"] = bc.ParseQ(txtpnl5SpecialPrecaution.Text);
                        drow["ReportDays"] = DBNull.Value;
                        drow["ReportDaysWeek"] = DBNull.Value;
                        drow["TestDuration"] = DBNull.Value;
                        drow["TestDays"] = DBNull.Value;
                        //drow["SampleRequired"] = ddlpnl5SampleRequired.SelectedIndex == 0 ? "0" : "1";
                        drow["SampleRequired"] = chkpnl5SampleRequired.Checked == false ? "0" : "1";
                        drow["SampleId"] = Convert.ToInt32(ddlpnl5SampleRequired.SelectedItem.Value);
                        drow["PrintSequence"] = DBNull.Value;

                        drow["ResourceScheduling"] = common.myInt(txtTime.Text);
                        dt.Rows.Add(drow);
                        strXMLInvestigation = bc.MakeXMLString(dt);
                    }
                }
                else if (ViewState["DeptFlag"].ToString() == "R")
                {
                    if (pnl4.Visible == true)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("BedCategoryType", typeof(String));
                        dt.Columns.Add("ExcludeFromOccupancy", typeof(Int16));
                        dt.Columns.Add("PrintSequence", typeof(Int16));
                        //Done by Ujjwal 24March2015 to capture billing Caetgory
                        dt.Columns.Add("isBillingCategory", typeof(bool));
                        dt.Columns.Add("BillingCategory", typeof(Int32));
                        dt.Columns.Add("MinimumAdvanceAmount", typeof(decimal));

                        DataRow drow = dt.NewRow();
                        drow["BedCategoryType"] = ddlPnl4BedType.SelectedItem.Value;
                        if (chkPnl4ExcludeFromOccupancy.Checked == true)
                        {
                            drow["ExcludeFromOccupancy"] = "1";
                        }
                        else
                        {
                            drow["ExcludeFromOccupancy"] = "0";
                        }
                        drow["PrintSequence"] = DBNull.Value;
                        drow["isBillingCategory"] = chkIsBillingCategory.Checked;

                        drow["BillingCategory"] = common.myInt(ddlBillingCategory.SelectedValue);
                        drow["MinimumAdvanceAmount"] = common.myStr(txtMinAdvanceAmount.Text.Trim());
                        dt.Rows.Add(drow);
                        strXMLBedCharge = bc.MakeXMLString(dt);

                    }
                }

                hshIn.Add("@XMLInvestigationFlag", strXMLInvestigation);
                hshIn.Add("@XMLRoomRentflags", strXMLBedCharge);
                hshIn.Add("@intActivityType", ddlAccountType.SelectedValue);
                hshIn.Add("@ConsultationTypeStatusId", ddlConsultationType.SelectedValue);
                hshIn.Add("@bitAppointmentRequired", ChkAppointmentRequired.Checked ? 1 : 0);
                hshIn.Add("@bitExpressBillingExcluded", common.myBool(chkIsExpressBillingExcluded.Checked));
                hshIn.Add("@bitExcludedForOrdering", common.myBool(chkisExcludedForOrdering.Checked));
                hshIn.Add("@IsAutoInsertVisitCharges", common.myBool(chkIsAutoInsertVisitCharges.Checked));
                if (common.myStr(txtSACCode.Text).Trim().Length > 0)
                {
                    hshIn.Add("@SACCode", common.myStr(txtSACCode.Text.Trim()));
                }
                else
                {
                    hshIn.Add("@SACCode", DBNull.Value);
                }

                hshOut.Add("@StrStatus", SqlDbType.VarChar);
                if (ViewState["DeptFlag"] != null)
                {
                    if (ViewState["DeptFlag"].ToString() == "I" || ViewState["DeptFlag"].ToString() == "IS")
                    {
                        hshIn.Add("@isServiceRemarkMandatory", chkremarkmanadatory.Checked); //commented by santosh
                    }
                }

                if (ViewState["Update"] != null)
                {
                    if (ViewState["Update"].ToString() == "True")
                    {

                        hshIn.Add("@SaveUpdate", 1);
                        hshIn.Add("@Active", rblStatus.SelectedItem.Value);

                    }
                    else
                    {
                        hshIn.Add("@SaveUpdate", 0);
                        hshOut.Add("@ServiceId", SqlDbType.Int);
                        hshIn.Add("@Active", 1);
                    }
                }
                else
                {
                    hshIn.Add("@SaveUpdate", 0);
                    hshIn.Add("@Active", 1);
                    hshOut.Add("@ServiceId", SqlDbType.Int);
                }

                //Added by rakesh start For Service Tax Applicable
                if (chkIsServiceTaxApplicable.Checked)
                {
                    hshIn.Add("@intIsServiceTaxApplicable", 1);
                }
                else
                {
                    hshIn.Add("@intIsServiceTaxApplicable", 0);
                }

                if (chkIsLuxuryTaxApplicable.Enabled == true)
                {
                    if (chkIsLuxuryTaxApplicable.Checked)
                    {
                        hshIn.Add("@intIsLuxuryTaxApplicable", 1);
                    }
                    else
                    {
                        hshIn.Add("@intIsLuxuryTaxApplicable", 0);
                    }
                }
                else
                {
                    hshIn.Add("@intIsLuxuryTaxApplicable", 0);
                }

                hshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                hshIn.Add("@TextRemarks", common.myStr(txtTextRemarks.Text));
                hshIn.Add("@chvApplicableFor", common.myStr(ddlApplicableFor.SelectedValue));

                //if (chkIsAckRequiredForBill.Checked)
                //{
                //    hshIn.Add("@IsAckRequiredForBill", 1);
                //}
                //else
                //{
                //    hshIn.Add("@IsAckRequiredForBill", 0);
                //}

                //hshOut.Add("@StrStatus", 0);
                ///    hshIn.Add("@intMHCReportFormatId", common.myStr(ddlMHCReportFormatId.SelectedValue)); //commented by santosh

                hshIn.Add("@chvServiceInstructions", common.myStr(txtServiceInstructions.Text));
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveItemOfService", hshIn, hshOut);
                if (string.IsNullOrEmpty(common.myStr(hshOut["@StrStatus"])))
                {
                    if (ViewState["Update"] != null)
                    {
                        if (ViewState["Update"].ToString() != "True")
                        {
                            txtServiceCode.Value = common.myStr(hshOut["@ServiceId"]);
                            // This checking is done Coz for CL,VF we dont have to show the charge in the grid, the rates of these are defined from Employee Master Pagehttp://localhost:12/MPages/ItemOfService.aspx
                            //
                            if (string.IsNullOrEmpty(common.myStr(hshOut["@StrStatus"])))
                            {
                                if ((ViewState["DeptFlag"].ToString() == "CL") || (ViewState["DeptFlag"].ToString() == "VF") || (ViewState["dTypeShort"].ToString() == "CL") || (ViewState["dTypeShort"].ToString() == "VF"))
                                {

                                    lblmsg.Text = "Record(s) Saved.Enter The Charges From Employee Master Page ...";
                                    lblmsg.ForeColor = System.Drawing.Color.Green;
                                    ClearData();
                                }
                                else
                                {

                                    lblmsg.Text = "Record(s) Saved. Now Please Enter The Charges ...";
                                    lblmsg.ForeColor = System.Drawing.Color.Green;
                                }
                                ClearData();
                            }
                            else
                            {
                                lblmsg.Text = common.myStr(hshOut["@StrStatus"]);
                                lblmsg.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        else
                        {

                            if (string.IsNullOrEmpty(common.myStr(hshOut["@StrStatus"])))
                            {
                                // This checking is done Coz for CL,VF we dont have to show the charge in the grid, the rates of these are defined from Employee Master Pagehttp://localhost:12/MPages/ItemOfService.aspx
                                if ((ViewState["DeptFlag"].ToString() == "CL") || (ViewState["DeptFlag"].ToString() == "VF") || (ViewState["dTypeShort"].ToString() == "CL") || (ViewState["dTypeShort"].ToString() == "VF"))
                                {

                                    lblmsg.Text = "Record(s) Updated ...";
                                    lblmsg.ForeColor = System.Drawing.Color.Green;
                                    ClearData();
                                }
                                else
                                {
                                    lblmsg.Text = "Record(s) Updated ...";
                                    lblmsg.ForeColor = System.Drawing.Color.Green;
                                    ClearData();
                                }
                            }
                            else
                            {
                                lblmsg.Text = common.myStr(hshOut["@StrStatus"]);
                                lblmsg.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                    else
                    {
                        txtServiceCode.Value = hshOut["@ServiceId"].ToString();
                        if (string.IsNullOrEmpty(common.myStr(hshOut["@StrStatus"])))
                        {
                            // This checking is done Coz for CL,VF we dont have to show the charge in the grid, the rates of these are defined from Employee Master Pagehttp://localhost:12/MPages/ItemOfService.aspx
                            if ((ViewState["DeptFlag"].ToString() == "CL") || (ViewState["DeptFlag"].ToString() == "VF") || (ViewState["dTypeShort"].ToString() == "CL") || (ViewState["dTypeShort"].ToString() == "VF"))
                            {

                                lblmsg.Text = "Service Added. Please add a charge for this service.";
                                lblmsg.ForeColor = System.Drawing.Color.Green;

                            }
                            else
                            {
                                ClearData();

                                lblmsg.Text = "Record(s) Saved. Now Please Enter The Charges...";
                                lblmsg.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                        else
                        {
                            lblmsg.Text = common.myStr(hshOut["@StrStatus"]);
                            lblmsg.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                else
                {
                    lblmsg.Text = common.myStr(hshOut["@StrStatus"]);
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                }
                if (ddlReportTagging.SelectedValue != "" && ddlReportTagging.SelectedValue != "0")
                {
                    BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
                    Hashtable output = master.SaveUpdateReportTagging(common.myInt(ViewState["serviceid"]), common.myInt(ddlReportTagging.SelectedValue), false);//master.SaveUpdateReportTagging(common.myInt(ViewState["serviceid"]), common.myInt(ddlReportTagging.SelectedValue), false);

                    lblmsg.Text = output["@chvOutPut"].ToString();

                }
                if (txtServiceCode.Value.Length > 0)
                {
                    PopulategvDefaultServiceCharges();
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



    protected void UpdateItemOfService_OnClick(object sender, EventArgs e)
    {
        btnsave_OnClick(sender, e);
    }

    protected void ddlMainDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlsubDept.Text = string.Empty;
        popSubDept();
        ddlsubDept_OnSelectedIndexChanged(sender, e);
    }

    protected void ddlsubDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clearField();
            //ddlItemOfService.Items.Clear();
            //ddlItemOfService.Items.Insert(0, " [ Select ] ");
            hshIn = new Hashtable();
            strSQL = new StringBuilder();
            if (ddlsubDept.Items.Count > 0)
            {
                hshIn.Add("@HospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
                hshIn.Add("@subdeptID", ddlsubDept.SelectedValue);
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                // For finding the department type with respect to the sub department selected
                strSQL.Remove(0, strSQL.Length);
                strSQL.Append(" select type, FAAccountId from departmentsub where HospitalLocationID=@HospitalLocationID and subdeptID= @subdeptID");

                dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
                if (dr.Read() == true)
                {
                    ViewState["DeptFlag"] = dr["type"].ToString();
                    //Done By Ujjwal 24March2013 to show isBillingCategory option if type is [R] start
                    if (common.myStr(ViewState["DeptFlag"]) == "R")
                    {
                        trisBillingCategory.Visible = true;
                        chkIsBillingCategory_CheckedChanged(sender, e);
                        bindBillingCategory();
                    }
                    else
                    {
                        trisBillingCategory.Visible = false;
                        trBillingCategory.Visible = false;
                    }
                    if (common.myStr(ViewState["DeptFlag"]) == "I" || common.myStr(ViewState["DeptFlag"]) == "IS")
                    {
                        chkremarkmanadatory.Visible = true;
                    }
                    else
                    {
                        chkremarkmanadatory.Visible = false;
                    }

                    //Done By Ujjwal 24March2013 to show isBillingCategory option if type is [R] end
                    txtFAAccountId.Text = dr["FAAccountId"].ToString();

                }

                dr.Close();

            }
            else
            {
            }
            if (common.myStr(ViewState["DeptFlag"]) == "S")
            {
                chkIsAutoInsertVisitCharges.Visible = true;
            }
            else
            {
                chkIsAutoInsertVisitCharges.Visible = false;
            }
            if (ddlsubDept.Items.Count > 0)
            {
            }
            panelVisibility();
            if (Request.QueryString["ServiceId"] != null)
            {
                //panelVisibility();
                //ddlItemOfService.Enabled = false;
                //return;
            }
            else
            {
                EnableDisableControls(false);
                // ddlItemOfService.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlPnl2ChargeType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if ((ddlPnl2ChargeType.SelectedIndex == 0) || (ddlPnl2ChargeType.SelectedIndex == 1))
            //{
            //    ltrlPnl2Slab.Visible = false;
            //    ddlPnl2Slab.Visible = false;
            //}
            //else if (ddlPnl2ChargeType.SelectedIndex == 2)
            //{
            //    ltrlPnl2Slab.Visible = true;
            //    ddlPnl2Slab.Visible = true;
            //}
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void optPercentageFixed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        PopulategvDefaultServiceCharges();
    }

    protected void SaveCharges_OnClick(object sender, EventArgs e)
    {
        try
        {
            pt = new BaseC.Patient(sConString);
            strSQL = new StringBuilder();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL.Remove(0, strSQL.Length);
            StringBuilder strSQLBedPercent = new StringBuilder();
            foreach (GridViewRow gvr in gvDefaultServiceCharges.Rows)
            {
                TextBox txtBedTypePercent = (TextBox)gvr.FindControl("txtBedTypePercent");
                TextBox txtHospitalCharge = (TextBox)gvr.FindControl("txtHospitalCharges");
                TextBox txtDrCharge = (TextBox)gvr.FindControl("txtDoctorCharge");
                TextBox txtValidfrom = (TextBox)gvr.FindControl("txtValidFrom");
                TextBox txtOPDisc = (TextBox)gvr.FindControl("txtOPDDiscPercent");
                TextBox txtIPDisc = (TextBox)gvr.FindControl("txtIPDDiscPercent");
                HtmlGenericControl lblActiveInActive = (HtmlGenericControl)gvr.FindControl("ActiveInactive");
                CheckBox chkStatus = (CheckBox)gvr.FindControl("chkActiveInactive");

                if (((txtHospitalCharge.Text.Trim() != "0.00") || (txtDrCharge.Text.Trim() != "0.00")) && (txtValidfrom.Text != "__/__/____"))
                {
                    strSQL.Append("<Table1><c1>");
                    strSQL.Append(gvr.Cells[9].Text.ToString());
                    strSQL.Append("</c1><c2>");
                    strSQL.Append(gvr.Cells[1].Text.ToString());
                    strSQL.Append("</c2><c3>");
                    strSQL.Append(gvr.Cells[0].Text.ToString());
                    strSQL.Append("</c3><c4>");
                    strSQL.Append(bc.ParseQ(txtBedTypePercent.Text.Trim()));
                    strSQL.Append("</c4><c5>");
                    strSQL.Append(bc.ParseQ(txtHospitalCharge.Text.Trim()));
                    strSQL.Append("</c5><c6>");
                    strSQL.Append(bc.ParseQ(txtDrCharge.Text.Trim()));
                    strSQL.Append("</c6><c7>");
                    strSQL.Append(pt.FormatDateYearMonthDate(txtValidfrom.Text.ToString().Trim()));
                    strSQL.Append("</c7><c8>");
                    strSQL.Append(bc.ParseQ(txtOPDisc.Text.Trim()));
                    strSQL.Append("</c8><c9>");
                    strSQL.Append(bc.ParseQ(txtIPDisc.Text.Trim()));
                    strSQL.Append("</c9><c10>");
                    strSQL.Append(bc.ParseQ(chkStatus.Checked == true ? "1" : "0"));
                    strSQL.Append("</c10></Table1>");
                    if (Convert.ToDouble(txtBedTypePercent.Text.ToString().Trim()) > 0)
                    {
                        strSQLBedPercent.Append("<Table1><c1>");
                        strSQLBedPercent.Append(gvr.Cells[0].Text.ToString());
                        strSQLBedPercent.Append("</c1><c2>");
                        strSQLBedPercent.Append(txtBedTypePercent.Text.ToString().Trim());
                        strSQLBedPercent.Append("</c2><c3>");
                        strSQLBedPercent.Append(pt.FormatDateYearMonthDate(txtValidfrom.Text.ToString().Trim()));
                        strSQLBedPercent.Append("</c3></Table1>");
                    }
                }
            }

            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            hshIn.Add("@ServiceId", bc.ParseQ(txtServiceCode.Value.Trim()));
            hshIn.Add("@ServiceType", ViewState["dTypeShort"].ToString());
            hshIn.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            if (optDifferbybedcategory.SelectedItem.Value == "0")
            {
                hshIn.Add("@ServiceDifferbyBedType", "0");
                hshIn.Add("@ServiceDifferbyPercentage", "0");
            }
            else if (optDifferbybedcategory.SelectedItem.Value == "1")
            {
                hshIn.Add("@ServiceDifferbyBedType", "1");
                hshIn.Add("@ServiceDifferbyPercentage", "0");
            }
            else if (optDifferbybedcategory.SelectedItem.Value == "2")
            {
                hshIn.Add("@ServiceDifferbyBedType", "1");
                hshIn.Add("@ServiceDifferbyPercentage", "1");
            }
            //hshIn.Add("@ServiceDifferbyBedType", optDifferbybedcategory.SelectedItem.Value == "True" ? "1" : "0");
            //hshIn.Add("@ServiceDifferbyPercentage", optPercentageFixed.SelectedItem.Value == "True" ? "1" : "0");
            hshIn.Add("@ChargeDetail", strSQL.ToString());
            hshIn.Add("@UserId", Convert.ToInt16(Session["UserID"]));
            hshOut.Add("@ErrorStatus", SqlDbType.VarChar);
            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateItemOfServiceCharge", hshIn, hshOut);
            if (hshOut["@ErrorStatus"].ToString() == "")
            {
                hshIn = new Hashtable();
                hshOut = new Hashtable();
                if (ViewState["PercentageVariation"].ToString() == "Yes")
                {
                    hshIn.Add("@intCompanyId", common.myStr(Cache["DefaultHospitalCompanyId"]));
                    hshIn.Add("@intdepartmentTypeId", ViewState["DepartmentTypeID"].ToString());
                    hshIn.Add("@intSubDeptid", ddlsubDept.SelectedItem.Value);
                    hshIn.Add("@intServiceId", bc.ParseQ(txtServiceCode.Value.Trim()));
                    hshIn.Add("@xmlBedCategory", strSQLBedPercent.ToString());
                    hshIn.Add("@intEncodedBy", Convert.ToInt16(Session["UserID"]));
                    hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                    hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveUpdateBedCategoryPercentage", hshIn, hshOut);
                }
            }
            BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
            Hashtable output = master.SaveUpdateReportTagging(Convert.ToInt32(ViewState["serviceid"]),
               Convert.ToInt32(ddlReportTagging.SelectedValue), false);

            lblmsg.Text = "Charges Saved ...";
            lblmsg.ForeColor = System.Drawing.Color.Green;
            PopulategvDefaultServiceCharges();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDefaultServiceCharges_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[11].Visible = true;

            if (ViewState["dTypeShort"].ToString() == "R")
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
            else
            {
                if (optDifferbybedcategory.SelectedItem.Value == "2")
                {
                    e.Row.Cells[3].Visible = true;
                }
                else
                {
                    e.Row.Cells[3].Visible = false;
                }

                //if (optPercentageFixed.SelectedItem.Value == "False")
                //{
                //    e.Row.Cells[3].Visible = false;
                //}

                if (pnl1.Visible == true)
                {
                    if (chkpnl1DoctorReq.Checked == false)
                    {
                        e.Row.Cells[5].Enabled = false;
                    }
                }
                else if (pnl2.Visible == true)
                {
                    if (chkPnl2DoctorReq.Checked == false)
                    {
                        e.Row.Cells[5].Enabled = false;
                    }
                }

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlGenericControl lblActiveInActive = (HtmlGenericControl)e.Row.FindControl("ActiveInactive");
                CheckBox chkStatus = (CheckBox)e.Row.FindControl("chkActiveInactive");
                lblActiveInActive.Attributes.Add("onclick", "javascript:fnActiveInActive('" + lblActiveInActive.ClientID + "','" + chkStatus.ClientID + "');");
                lblActiveInActive.Attributes.Add("onmouseover", "javascript:fnOnMouseOver('" + lblActiveInActive.ClientID + "');");
                lblActiveInActive.Attributes.Add("onmouseout", "javascript:fnOnMouseOut('" + lblActiveInActive.ClientID + "');");

                TextBox txtBedPercent = (TextBox)e.Row.FindControl("txtBedTypePercent");
                TextBox txtHosp = (TextBox)e.Row.FindControl("txtHospitalCharges");
                TextBox DChar = (TextBox)e.Row.FindControl("txtDoctorCharge");
                TextBox txtOPPercent = (TextBox)e.Row.FindControl("txtOPDDiscPercent");
                TextBox txtIPPercent = (TextBox)e.Row.FindControl("txtIPDDiscPercent");
                chkStatus.Enabled = false;
            }
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    ImageButton ibtn = (ImageButton)e.Row.FindControl("btnsaveGridCharges");
            //    if (Request.QueryString["ServiceId"] != null && ddlItemOfService.SelectedIndex > 0)
            //    {
            //        ibtn.Visible = true;
            //    }
            //    else if (Request.QueryString["ServiceId"] == null && ddlItemOfService.SelectedIndex > 0)
            //    {
            //        ibtn.Visible = false;
            //    }
            //  }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDefaultServiceCharges_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.SetRenderMethodDelegate(RenderHeader);

                Int32 idx = 0;
                foreach (TableCell cell in e.Row.Cells)
                {
                    idx += 1;
                    if (idx == 7)
                    {
                        cell.Text = "Valid From  <span style=\"color:White; font-size:smaller;text-align:center\">[ dd/MM/yyyy ]</span>";//String.Format("{0}_{1}", cell.Text, (++idx));
                        cell.HorizontalAlign = HorizontalAlign.Left;
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

    private void popMainDept()
    {
        try
        {
            DAL.DAL dl;
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL = new StringBuilder();
            hshIn = new Hashtable();
            strSQL.Remove(0, strSQL.Length);
            hshIn.Add("@Active", "1");
            hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
            strSQL.Append(" select distinct DM.departmentname,DM.departmentid from departmentmain dm inner join departmentsub ds on dm.departmentid=ds.departmentid");
            strSQL.Append(" where dm.Active =@Active and dm.HospitalLocationID=@HospitalLocationID order by dm.departmentname ");
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
            if (dr.HasRows == true)
            {
                ddlMainDept.DataSource = dr;
                ddlMainDept.DataTextField = "departmentname";
                ddlMainDept.DataValueField = "departmentid";
                ddlMainDept.DataBind();
                dr.Close();
            }
            else
            {
                ddlMainDept.Width = Unit.Pixel(295);
                ddlsubDept.Width = Unit.Pixel(220);
                // ddlItemOfService.Width = Unit.Pixel(250);
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void popSubDept()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL = new StringBuilder();
            hshIn = new Hashtable();
            if (ddlMainDept.Items.Count > 0)
            {
                hshIn.Add("@departmentid", ddlMainDept.SelectedValue);
                hshIn.Add("@Active", "1");
                hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"].ToString()));
                strSQL.Remove(0, strSQL.Length);
                strSQL.Append(" select subdeptid,ds.subname,ds.FAAccountId from departmentsub ds");
                strSQL.Append(" where departmentid = @departmentid and ds.Active =@Active and ds.HospitalLocationID = @HospitalLocationID order by ds.subname ASC");
                dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
                if (dr.HasRows == true)
                {
                    ddlsubDept.DataSource = dr;
                    ddlsubDept.DataTextField = "subname";
                    ddlsubDept.DataValueField = "subdeptid";
                    ddlsubDept.DataBind();
                    dr.Close();
                }
                else
                {
                    ddlsubDept.Width = Unit.Pixel(220);
                    //      ddlItemOfService.Width = Unit.Pixel(250);
                }
                dr.Close();
            }
            else
            {
                ddlsubDept.Width = Unit.Pixel(220);
                //            ddlItemOfService.Width = Unit.Pixel(250);
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void clearField()
    {
        // txtRefServiceCode.Text = "";
        // txtServiceCode.Value = "";
        //    txtServiceName.Text = "";
        //  chkpnl1PriceEditable.Checked = false;
        //chkpnl1DoctorReq.Checked = false;
        //chkpnl1AppointmentReq.Checked = false;
        //  chkPnl2PriceEditable.Checked = false;
        //   chkPnl2DoctorReq.Checked = false;
        //ddlPnl2ChargeType.SelectedIndex = 0;
        //ddlPnl4BedType.SelectedIndex = 0;
        //chkPnl4ExcludeFromOccupancy.Checked = false;
        //ddlpnl5ReportType.SelectedIndex = 0;
        //if (ddlpnl5SampleRequired.Items.Count > 0)
        //{
        //    ddlpnl5SampleRequired.SelectedIndex = 0;
        //}
        //txtpnl5SpecialPrecaution.Text = "";
        //optDifferbybedcategory.SelectedIndex = 0;
        //optPercentageFixed.SelectedIndex = 0;
        //ltrlPercentFixed.Visible = false;
        //optPercentageFixed.Visible = false;
        //    gvDefaultServiceCharges.DataBind();
        // ddlItemOfService.SelectedIndex = -1;
        txtFAAccountId.Text = "";
    }

    private void panelVisibility()
    {
        try
        {
            if (ddlsubDept.Items.Count > 0)
            {
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                strSQL = new StringBuilder();
                hshIn = new Hashtable();
                hshIn.Add("@subdeptid", ddlsubDept.SelectedValue);
                String strSubType = "";
                strSQL.Append("select ds.type,dt.name,dt.ID as DepartmentTypeID from departmentsub ds inner join departmenttype dt on ds.type=dt.departmenttype where subdeptid=@subdeptid");
                dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
                if (dr.Read() == true)
                {
                    strSubType = dr["type"].ToString().Trim();
                    ViewState["dtype"] = dr["name"].ToString().Trim();
                    ViewState["dTypeShort"] = dr["type"].ToString().Trim();
                    ViewState["DepartmentTypeID"] = dr["DepartmentTypeID"].ToString().Trim();
                }
                dr.Close();
                populateControls();
                ltrlDifferbybedcategory.Visible = true;
                chkpnl1DoctorReq.Enabled = true;
                optDifferbybedcategory.Visible = true;
                {
                    chkPnl2PriceEditable.Enabled = true;
                    chkPnl2PriceEditable.Checked = false;
                    //Disable all the controls 
                    chkpnl1DoctorReq.Enabled = false;
                    txtTime.Enabled = false;
                    txtEstimatedPrice.Enabled = false;
                    ddlPnl3Classification.Enabled = false;
                    ddlTimeBased.Enabled = false;
                    chkIsLuxuryTaxApplicable.Enabled = false;
                    chkIsLuxuryTaxApplicable.Checked = false;
                    ddlpnl5ReportType.Enabled = false;
                    chkpnl5SampleRequired.Enabled = false;
                    chkpnl5SampleRequired.Checked = false;
                    ddlpnl5SampleRequired.Enabled = false;
                    ddlTimeBased.Enabled = false;
                }
                if (strSubType == "I" || strSubType == "IS" || strSubType == "P" || strSubType == "C")
                {
                    chkpnl1DoctorReq.Enabled = true;
                    txtTime.Enabled = true;
                    txtEstimatedPrice.Enabled = true;
                    ddlpnl5ReportType.Enabled = true;
                    chkpnl5SampleRequired.Enabled = true;
                    ddlpnl5SampleRequired.Enabled = true;
                    ddlTimeBased.Enabled = true;
                }
                else if (strSubType == "CL" || strSubType == "VF" || strSubType == "VS")
                {
                    chkpnl1DoctorReq.Enabled = true;
                    ddlTimeBased.Enabled = true;
                }
                else if (strSubType == "S")
                {
                    chkpnl1DoctorReq.Enabled = true;
                    ddlPnl3Classification.Enabled = true;
                    if (ddlPnl3Classification.SelectedIndex == 0)
                    {
                        ddlPnl3Classification.SelectedIndex = 1;
                    }
                }
                else if (strSubType == "O" || strSubType == "OS")
                {
                    chkpnl1DoctorReq.Enabled = true;
                    ddlTimeBased.Enabled = true;
                }
                else if (strSubType == "R")
                {
                    chkIsLuxuryTaxApplicable.Enabled = true;
                }
                //else
                //{
                //    chkPnl2DoctorReq.Enabled = true;
                //    chkCompulsaryonBill.Enabled = true;
                //    txtTime.Enabled = true;
                //    txtEstimatedPrice.Enabled = true;
                //    ddlPnl3Classification.Enabled = true;
                //    ddlTimeBased.Enabled = true;

                //}
                //commented by rakesh start
                //if ((strSubType == "I") || (strSubType == "IS") || (strSubType == "P"))
                //{
                //    pnl1.Visible = true;
                //    pnl2.Visible = false;
                //    pnl3.Visible = false;
                //    pnl4.Visible = false;
                //    pnl5.Visible = true;
                //}
                //else
                //{
                //    pnl1.Visible = false;
                //    pnl2.Visible = false;
                //    pnl3.Visible = false;
                //    pnl4.Visible = false;
                //    pnl5.Visible = false;
                //    if (strSubType == "S")
                //    {
                //        pnl3.Visible = true;
                //    }
                //    if (strSubType == "R")
                //    {
                //        pnl4.Visible = true;
                //        ltrlDifferbybedcategory.Visible = true;
                //        optDifferbybedcategory.Visible = true;
                //    }
                //    else
                //    {
                //        if (strSubType != "S")
                //        {
                //            pnl2.Visible = true;
                //        }
                //        if ((strSubType == "CL") || (strSubType == "OPP") || (strSubType == "IPP") || (strSubType == "VF") || (strSubType == "VS"))
                //        {
                //            //ltrlPnl2ChargeType.Visible = false;
                //            //ddlPnl2ChargeType.Visible = false;
                //            ltrlPnl2PriceEditable.Visible = false;
                //            chkPnl2PriceEditable.Visible = false;
                //        }
                //        else
                //        {
                //            // ltrlPnl2ChargeType.Visible = true;
                //            // ddlPnl2ChargeType.Visible = true;
                //        }
                //    }
                //}
                //commented by rakesh end

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulategvDefaultServiceCharges()
    {
        // This checking is done Coz for CL,VF we dont have to show the charge in the grid, the rates of these are defined from Employee Master Pagehttp://localhost:12/MPages/ItemOfService.aspx
        //if ((ViewState["DeptFlag"].ToString() == "CL") || (ViewState["DeptFlag"].ToString() == "VF") || (ViewState["dTypeShort"].ToString() == "CL") || (ViewState["dTypeShort"].ToString() == "VF"))
        //{

        //}
        //else
        //{
        //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    strSQL = new StringBuilder();
        //    try
        //    {
        //        hshIn = new Hashtable();
        //        Int32 intData;
        //        Boolean blnTemp = Int32.TryParse(txtServiceCode.Value, NumberStyles.Integer, CultureInfo.CurrentCulture.NumberFormat, out intData);
        //        hshIn.Add("@serviceid", intData);
        //        hshIn.Add("@servicetype", ViewState["dTypeShort"].ToString());
        //        hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));

        //        if (optDifferbybedcategory.SelectedItem.Value == "0")
        //        {
        //            hshIn.Add("@ServiceDifferbyBedType", "0");
        //            hshIn.Add("@ServiceDifferbyPercentage", "0");
        //        }
        //        else if (optDifferbybedcategory.SelectedItem.Value == "1")
        //        {
        //            hshIn.Add("@ServiceDifferbyBedType", "1");
        //            hshIn.Add("@ServiceDifferbyPercentage", "0");
        //        }
        //        else if (optDifferbybedcategory.SelectedItem.Value == "2")
        //        {
        //            hshIn.Add("@ServiceDifferbyBedType", "1");
        //            hshIn.Add("@ServiceDifferbyPercentage", "1");
        //        }


        //        ds = dl.FillDataSet(CommandType.StoredProcedure, "uspshowservicecharge", hshIn);
        //        gvDefaultServiceCharges.DataSource = ds;
        //        gvDefaultServiceCharges.DataBind();

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.ToString());
        //    }
        //}
    }

    private void RetrieveItemOfService()
    {
        ltrlEncodedBy.Visible = false;
        ltrlUpdateBy.Visible = false;
        ltrlEncodedByDisplay.Text = "";
        ltrlUpdateByDisplay.Text = "";
        try
        {
            ViewState["Edit"] = "TRUE";
            DAL.DAL dl;
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL = new StringBuilder();
            Hashtable hshIn = new Hashtable();
            //  hshIn.Add("@ServiceType", ViewState["dTypeShort"].ToString());
            //hshIn.Add("@ServiceID", gvIOS.Rows[gvIOS.SelectedIndex].Cells[0].Text.ToString());
            hshIn.Add("@intServiceID", Convert.ToInt32(ViewState["serviceid"].ToString()));
            //Added by rakesh start
            hshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            //Added by rakesh end
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "UspRetrieveItemOfService", hshIn);
            if (dr.Read())
            {
                ViewState["DocReq"] = dr["DoctorRequired"].ToString();// This is used to make the doc charge field in the default grid active or inactive
                txtServiceCode.Value = dr["serviceid"].ToString();
                txtRefServiceCode.Text = dr["RefServiceCode"].ToString();
                txtServiceName.Text = dr["ServiceName"].ToString();
                txtcptcode.Text = dr["CPTCode"].ToString();
                //rblStatus.SelectedValue = dr["active"].ToString();
                if (common.myStr(dr["active"]).ToUpper().Equals("FALSE") || common.myStr(dr["active"]).ToUpper().Equals("0"))
                {
                    rblStatus.SelectedValue = "0";
                }
                else
                {
                    rblStatus.SelectedValue = "1";
                }
                txtJCode.Text = dr["JCODE"].ToString();
                txtLONICCode.Text = dr["LONICCode"].ToString();
                ltrlEncodedBy.Visible = true;
                ltrlUpdateBy.Visible = true;
                ltrlEncodedByDisplay.Text = dr["EncodedBy"].ToString() + " - " + dr["Encodeddate"].ToString();// +"  " + dr["EncodedTime"].ToString();
                ltrlUpdateByDisplay.Text = dr["LastChangedBy"].ToString() + " - " + dr["LastCHangedDate"].ToString();// +"  " + dr["LastChangedTime"].ToString();
                ViewState["DeptFlag"] = dr["departmenttype"].ToString();
                if (ViewState["DeptFlag"].ToString() == "I" || ViewState["DeptFlag"].ToString() == "IS")
                {
                    chkremarkmanadatory.Checked = common.myBool(dr["isServiceRemarkMandatory"]);
                }
                else
                {
                    chkremarkmanadatory.Checked = false;
                }
                ViewState["dTypeShort"] = dr["type"].ToString().Trim();
                ddlTimeBased.SelectedValue = dr["TimeBased"].ToString();
                txtEstimatedPrice.Text = dr["EstimatedCost"].ToString();
                ddlAccountType.SelectedValue = common.myStr(dr["ActivityType"]);
                ddlConsultationType.SelectedValue = common.myStr(dr["ConsultationTypeStatusId"]);
                txtTextRemarks.Text = dr["TextRemarks"].ToString();
                ddlReportTagging.SelectedValue = dr["TaggingId"].ToString();
                txtFAAccountId.Text = dr["FAAccountId"].ToString();
                chkCompulsaryonBill.Checked = common.myBool(dr["CompulsoryonBill"]);
                ChkAppointmentRequired.Checked = common.myBool(dr["IsAppointmentRequired"]);
                txtServiceInstructions.Text = dr["ServiceInstructions"].ToString();
                ddlApplicableFor.SelectedIndex = ddlApplicableFor.Items.IndexOf(ddlApplicableFor.Items.FindByValue(common.myStr(dr["ApplicableFor"])));
                txtServiceTax.Text = common.myStr(dr["ServiceTax"]);
                txtVat.Text = common.myStr(dr["VAT"]);
                chkIsAutoInsertVisitCharges.Checked = common.myBool(dr["IsAutoInsertVisitCharge"]);
                txtMinAdvanceAmount.Text = common.myDec(dr["MinimumAdvanceAmount"]).ToString("F" + 2);
                //Added on  18 Mar Naushad Start
                txtremarks.Text = dr["TextRemarks"].ToString();
                //Addded on 18 Mar Naushad End
                txtSACCode.Text = dr["SACCode"].ToString();
                ddlMHCReportFormatId.SelectedIndex = ddlMHCReportFormatId.Items.IndexOf(ddlMHCReportFormatId.Items.FindByValue(common.myStr(dr["MHCReportFormatId"])));
                if (dr["ResourceScheduleTime"] != null)
                {
                    txtTime.Text = common.myStr(dr["ResourceScheduleTime"]);
                }
                else
                {
                    txtTime.Text = "15";
                }
                //lblDepartmentType.Text = ViewState["dtype"].ToString();
                if (pnl1.Visible == true)
                {
                    if (dr["PriceEditable"].ToString() == "False")
                    {
                        chkpnl1PriceEditable.Checked = false;
                    }
                    else
                    {
                        chkpnl1PriceEditable.Checked = true;
                    }
                    if (dr["DoctorRequired"].ToString() == "False")
                    {
                        chkpnl1DoctorReq.Checked = false;
                    }
                    else
                    {
                        chkpnl1DoctorReq.Checked = true;
                    }
                    //if (dr["AppointmentRequired"].ToString() == "False")
                    //{
                    //    chkpnl1AppointmentReq.Checked = false;
                    //}
                    //else
                    //{
                    //    chkpnl1AppointmentReq.Checked = true;
                    //}
                }
                if (pnl2.Visible == true)
                {
                    if (dr["PriceEditable"].ToString() == "False")
                    {
                        chkPnl2PriceEditable.Checked = false;
                    }
                    else
                    {
                        chkPnl2PriceEditable.Checked = true;
                    }

                    if (dr["DoctorRequired"].ToString() == "False")
                    {
                        chkPnl2DoctorReq.Checked = false;
                    }
                    else
                    {
                        chkPnl2DoctorReq.Checked = true;
                    }

                }

                if (dr["PriceEditable"].ToString() == "False")
                {
                    chkPnl2PriceEditable.Checked = false;
                }
                else
                {
                    chkPnl2PriceEditable.Checked = true;
                }

                if (pnl4.Visible == true)
                {
                    ddlPnl4BedType.SelectedValue = dr["BedCategoryType"].ToString();
                    if (dr["ExcludeFromOccupancy"].ToString() == "False")
                    {
                        chkPnl4ExcludeFromOccupancy.Checked = false;
                    }
                    else
                    {
                        chkPnl4ExcludeFromOccupancy.Checked = true;
                    }
                    if (Convert.ToInt16(dr["SurgeryClassificationID"].ToString()) > 0)
                    {
                        ddlPnl3Classification.SelectedValue = dr["SurgeryClassificationID"].ToString();
                    }
                }
                if (pnl5.Visible == true)
                {
                    ddlpnl5ReportType.SelectedValue = dr["ReportType"].ToString();

                    string sid = Convert.ToString(dr["SampleId"]);
                    ddlpnl5SampleRequired.SelectedIndex = ddlpnl5SampleRequired.Items.IndexOf(ddlpnl5SampleRequired.Items.FindByValue(sid));

                    txtpnl5SpecialPrecaution.Text = dr["SpecialPrecaution"].ToString();
                    if (dr["SampleRequired"].ToString() == "0")
                    {
                        chkpnl5SampleRequired.Checked = false;
                    }
                    else
                    {
                        chkpnl5SampleRequired.Checked = true;
                    }
                }

                if (dr["DifferByBedCategory"].ToString() == "False")  //True
                {
                    optDifferbybedcategory.SelectedValue = "0";
                }
                else
                {
                    if (dr["DifferbyPercentage"].ToString() == "False")
                    {
                        optDifferbybedcategory.SelectedValue = "1";
                    }
                    else
                    {
                        optDifferbybedcategory.SelectedValue = "2";
                        ViewState["PercentageVariation"] = "Yes";
                    }
                }
                if (dr["HospitalLocationId"].ToString() == "")
                {
                    ibtnUpdate.Enabled = false;
                    lblmsg.Text = " Not Editable !";
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                }
                //Added by rakesh start
                if (common.myBool(dr["ServiceTaxApplicable"]) == false)
                {
                    chkIsServiceTaxApplicable.Checked = false;
                }
                else
                {
                    chkIsServiceTaxApplicable.Checked = true;
                }
                if (common.myBool(dr["LuxuryTaxApplicable"]) == false)
                {
                    chkIsLuxuryTaxApplicable.Enabled = true;
                    chkIsLuxuryTaxApplicable.Checked = false;
                }
                else
                {
                    chkIsLuxuryTaxApplicable.Enabled = true;
                    chkIsLuxuryTaxApplicable.Checked = true;
                }

                //if (dr["AckRequiredForBill"].ToString() == "False")
                //{
                //    chkIsAckRequiredForBill.Checked = false;
                //}
                //else
                //{
                //    chkIsAckRequiredForBill.Checked = true;
                //}
                //Added by rakesh end

                //Done by Ujjwal 24March2015 to Capture Billing Category start 
                if (common.myStr(ViewState["DeptFlag"]) == "R")
                {
                    if (dr["IsBillingCategory"].ToString().ToUpper() == "FALSE")
                    {
                        chkIsBillingCategory.Checked = false;
                        bindBillingCategory();
                        ddlBillingCategory.SelectedValue = dr["BillingCategoryID"].ToString();
                        trBillingCategory.Visible = true;
                        chkIsBillingCategory_CheckedChanged(this, null);
                    }
                    else
                    {
                        chkIsBillingCategory.Checked = true;
                        trBillingCategory.Visible = false;
                    }
                }
                else
                {
                    trBillingCategory.Visible = false;
                    trisBillingCategory.Visible = false;
                }
                //Done by Ujjwal 24March2015 to Capture Billing Category end
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void EnableDisableControls(Boolean blnTF)
    {
        try
        {
            //  txtRefServiceCode.Enabled = blnTF;
            //txtServiceName.Enabled = blnTF;
            //  chkpnl1PriceEditable.Enabled = blnTF;
            // chkpnl1DoctorReq.Enabled = blnTF;
            //chkpnl1AppointmentReq.Enabled = blnTF;
            chkPnl2PriceEditable.Enabled = blnTF;
            chkPnl2DoctorReq.Enabled = blnTF;
            //ddlPnl2ChargeType.Enabled = blnTF;
            //ddlPnl2Slab.Enabled = blnTF;
            //chkPnl3PriceEditable.Enabled = blnTF;
            ////    ddlPnl3Classification.Enabled = blnTF;
            //ddlPnl3SurgeryType.Enabled = blnTF;
            ddlPnl4BedType.Enabled = blnTF;
            chkPnl4ExcludeFromOccupancy.Enabled = blnTF;
            // ddlpnl5ReportType.Enabled = blnTF;
            ddlpnl5SampleRequired.Enabled = blnTF;
            // txtpnl5SpecialPrecaution.Enabled = blnTF;
            // optDifferbybedcategory.Enabled = blnTF;
            //optPercentageFixed.Enabled = blnTF;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    private void populateControls()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL = new StringBuilder();
            Hashtable hshIn = new Hashtable();

            hshIn.Add("@Active", "1");
            hshIn.Add("@HospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
            strSQL.Remove(0, strSQL.Length);
            strSQL.Append(" select ID,Name from surgeryclassification where Active=@Active and HospitalLocationID=@HospitalLocationID");
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
            if (dr.HasRows == true)
            {
                ddlPnl3Classification.DataSource = dr;
                ddlPnl3Classification.DataValueField = "ID";
                ddlPnl3Classification.DataTextField = "Name";
                ddlPnl3Classification.DataBind();
                ddlPnl3Classification.Items.Insert(0, " ");

            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void popTableDefaultValue()
    {
        try
        {
            /////////  Finding the default Company
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ///dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select dbo.GetHospitalSetupValue(" + common.myInt(Session["HospitalLocationId"]) + ",'DefaultHospitalCompany'," + common.myInt(Session["FacilityId"]) + ")");
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "Select dbo.GetHospitalSetupValue(" + common.myInt(Session["HospitalLocationID"]) + "," + common.myInt(Session["FacilityId"]) + ",'DefaultHospitalCompany')");
            dr.Read();
            ViewState["DefaultCompany"] = dr[0].ToString();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkEmployeeLookup_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("~/mpages/Servicelokup.aspx?MD=ADMIN", false);
    }
    protected void lnkChargetype_OnClick(object seneder, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/MPages/Chargetype.aspx";
        RadWindow1.Height = 300;
        RadWindow1.Width = 600;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void Bindsdata()
    {
        try
        {
            btnsave.Enabled = false;
            RetrieveItemOfService();
            PopulategvDefaultServiceCharges();
            ViewState["Update"] = "True";

            if (Request.QueryString["ServiceId"] != null)
            {
                if (txtServiceCode.Value.ToString().Trim().Length > 0)
                {
                    EnableDisableControls(true);
                }
                else
                {
                    EnableDisableControls(false);
                }

                btnsave.Enabled = true;
                //  btnReportFormat.Visible = true;
            }
            else
            {
                btnsave.Enabled = false;
                EnableDisableControls(false);
                // btnReportFormat.Visible = false;
            }
            //  btnReportFormat.Attributes.Add("onclick", "window.open('/MPages/InvestigationFormats.aspx?MDID=" + ddlMainDept.SelectedItem.Value + "&SDID=" + ddlsubDept.SelectedItem.Value + "&SID=" + ddlItemOfService.SelectedItem.Value + "',\"mywindow\", \"menubar=0,resizable=0,width=800,height=450,status=0,toolbars=0\");");
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ClearData()
    {
        txtServiceName.Text = "";
        txtcptcode.Text = "";
        txtLONICCode.Text = "";
        txtRefServiceCode.Text = "";
        txtJCode.Text = "";
        ddlMainDept.SelectedIndex = 0;
        ddlsubDept.Items.Clear();
        ddlsubDept.Text = string.Empty;
        chkpnl1DoctorReq.Checked = false;
        chkpnl1PriceEditable.Checked = false;
        ChkAppointmentRequired.Checked = false;
        chkIsServiceTaxApplicable.Checked = false;
        chkIsExpressBillingExcluded.Checked = false;
        chkisExcludedForOrdering.Checked = false;

        txtMinAdvanceAmount.Text = "0.00";
    }
    //Added by rakesh for template tagging start


    //public DataSet getItemFlagTagging(int ItemId, bool OnlyTagged, int HospId)
    public DataSet GetEMRTemplateServiceTagging(int HospitalLocationId
        , int FacilityId, int ServiceId, int OnlyTagged, int Active)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@iServiceId", ServiceId);
        HshIn.Add("@onlyTagged", OnlyTagged);
        HshIn.Add("@bitActive", Active);
        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateServiceTagging", HshIn);
        return ds;

    }


    protected void LinkButton1_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "TemplateServiceTag.aspx?Master=I";
        RadWindowForNew.Height = 615;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        //Response.Redirect("~/mpages/ServiceTemplateTagging.aspx?MD=ADMIN", false);
    }
    protected void LinkButton2_OnClick(object sender, EventArgs e)
    {

        if (txtServiceCode.Value.Trim() == "")
        {
            Alert.ShowAjaxMsg("Please Select Service", this);
            return;
        }


        RadWindowForNew.NavigateUrl = "/MPages/SurgeryMapping.aspx?SId=" + bc.ParseQ(txtServiceCode.Value.Trim()) + "&ServiceName=" + common.myStr(txtServiceName.Text)
            + "&FId= " + Session["FacilityId"];

        RadWindowForNew.Height = 615;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void lnkFacilityServiceFlagTagging_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "FacilityServiceFlagTag.aspx?Master=I";
        RadWindowForNew.Height = 615;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        //Response.Redirect("~/mpages/ServiceTemplateTagging.aspx?MD=ADMIN", false);
    }
    //Added by rakesh for template tagging start

    //Done by ujjwal 23March2015 to show hide trbillingcategory start
    protected void chkIsBillingCategory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsBillingCategory.Checked == true)
        {
            trBillingCategory.Visible = false;
            bindBillingCategory();
            ddlBillingCategory.SelectedValue = "0";
        }
        else
        {
            trBillingCategory.Visible = true;
        }
    }
    protected void bindBillingCategory()
    {
        try
        {
            Hashtable hshIn = new Hashtable();
            int serviceid = 0;
            if (ViewState["serviceid"] != null)
            {
                serviceid = Convert.ToInt32(ViewState["serviceid"].ToString());
            }
            hshIn.Add("@intServiceID", serviceid);
            ddlBillingCategory.Items.Clear();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataTable dt = dl.FillDataSet(CommandType.StoredProcedure, "uspGetBillingCategory", hshIn).Tables[0];
            ddlBillingCategory.DataSource = dt;
            ddlBillingCategory.DataTextField = "servicename";
            ddlBillingCategory.DataValueField = "BedCategoryid";

            ddlBillingCategory.DataBind();
            ddlBillingCategory.Items.Insert(0, new ListItem("SELECT"));
            ddlBillingCategory.Items[0].Value = "0";
        }
        catch
        {
        }
    }
    //Done by ujjwal 23March2015 to show hide trbillingcategory end

    //Done By Ujjwal 13062016 
    protected void BindMHCReportFormatId()
    {
        try
        {
            BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
            DataSet ds = master.GetMHCReportFormatId();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlMHCReportFormatId.DataSource = ds.Tables[0];
                ddlMHCReportFormatId.DataTextField = "ReportName";
                ddlMHCReportFormatId.DataValueField = "ReportId";
                ddlMHCReportFormatId.DataBind();
            }
            ddlMHCReportFormatId.Items.Insert(0, new ListItem("", "0"));
            ddlMHCReportFormatId.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
}
