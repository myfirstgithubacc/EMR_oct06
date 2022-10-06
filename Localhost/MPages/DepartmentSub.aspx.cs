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
using System.Threading;
using System.Globalization;
using System.Text;
using Telerik.Web.UI;

public partial class MPages_DepartmentSub : BasePage
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    StringBuilder strSQL;
    DataSet ds;
    SqlDataReader dr;
    BaseC.ParseData bc = new BaseC.ParseData();
    DAL.DAL dl;
    Hashtable hshIn;

    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //BaseC.ParseData bc = new BaseC.ParseData();
    //StringBuilder strSQL;
    //DataSet ds;
    //SqlDataReader dr;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();

            ViewState["Mode"] = "New";
            ViewState["DepartmentMode"] = "New";

            populateMainDept();
            populateControls();

            bindFacility();
        }

        if (common.myInt(hdnSubdeptid.Value) > 0)
        {
            string Dept_Id = ddlMainDept.SelectedValue;
            string Subdept_Id = hdnSubdeptid.Value;
            btnDistagging.Attributes.Add("onclick", "return openRadWindow1('" + Dept_Id + "')");
            // btnDistagging.Attributes.Add("onclick", "return openRadWindow1('" + Dept_Id + "," + Subdept_Id + "')");
            //RadWindow3.NavigateUrl = "/MPages/Company/DiscountTagging.aspx?deptId=" + ddlMainDept.SelectedItem.Value + "&SubDeptid=" + Convert.ToInt32(hdnSubdeptid.Value) + "";
            //RadWindow3.Height = 280;
            //RadWindow3.Width = 750;
            //RadWindow3.Top = 40;
            //RadWindow3.Left = 100;
            //RadWindow3.OnClientClose = "OnClientClose";
            //RadWindow3.Modal = true;
            //RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            //RadWindow3.VisibleStatusbar = false;
        }
    }

    void bindFacility()
    {
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.getFacility(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

            ddlFacility.DataSource = ds.Tables[0];
            ddlFacility.DataTextField = "Name";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();

            ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlFacility.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void SaveDepartmentSub_OnClick(object sender, EventArgs e)
    {
        try
        {
            DateTime emergencyfrom;
            DateTime emergencyto;
            if (rademergencyfrom.SelectedDate == null)
            {
                emergencyfrom = DateTime.Now;
            }
            else
            {
                emergencyfrom = Convert.ToDateTime(rademergencyfrom.SelectedDate);
            }
            if (rademergencyto.SelectedDate == null)
            {
                emergencyto = DateTime.Now;
            }
            else
            {
                emergencyto = Convert.ToDateTime(rademergencyto.SelectedDate);
            }

            if (ViewState["Mode"].ToString() == "Edit")
            {
                if (ddlDeptType.SelectedIndex == 0)
                {
                    Alert.ShowAjaxMsg("Please Select Department Type...", this.Page);
                    return;
                }

                if (txtDeptSubFAAccountId.Text == "" || common.myStr(txtDeptSubFAAccountId.Text) == "0")
                {
                    Alert.ShowAjaxMsg("FA AccountID Cannnot be Blank or zero...", this.Page);
                    return;
                }

                if (gvDepartmentSub.SelectedIndex >= 0)
                {
                    DAL.DAL dl;
                    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    strSQL = new StringBuilder();
                    strSQL.Remove(0, strSQL.Length);
                    Hashtable hshIn = new Hashtable();
                    if (rblStatus.SelectedItem.Value == "0")
                    {
                        hshIn = new Hashtable();
                        hshIn.Add("@SubDeptId", gvDepartmentSub.SelectedRow.Cells[0].Text);
                        strSQL.Append("select active from itemofservice where SubDeptId=@SubDeptId and active=1");
                        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
                        if (dr.HasRows == true)
                        {
                            dr.Close();
                            Alert.ShowAjaxMsg("Active Services  Are There For The Selected Subdepartment, So Cannot Make It Inactive...", this.Page);
                            return;
                        }
                        dr.Close();
                    }

                    hshIn = new Hashtable();
                    hshIn.Add("@SubDeptId", gvDepartmentSub.SelectedRow.Cells[0].Text);
                    hshIn.Add("@SubName", bc.ParseQ(txtSubDeptName.Text.ToString()));
                    hshIn.Add("@Type", ddlDeptType.SelectedItem.Value);
                    hshIn.Add("@LastChangedBy", Convert.ToInt32(Session["UserID"]));
                    hshIn.Add("@LastChangedDate", DateTime.Now);
                    hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
                    hshIn.Add("@Active", rblStatus.SelectedItem.Value);
                    hshIn.Add("@FAAccountId", bc.ParseQ(txtDeptSubFAAccountId.Text.ToString()));

                    hshIn.Add("@EmergencyChargesForOPD", Chkopemergency.Checked);
                    hshIn.Add("@EmergencyChargesForIPD", Chkipemergency.Checked);
                    hshIn.Add("@EmergencyChargesFromTime", emergencyfrom);
                    hshIn.Add("@EmergencyChargesToTime", emergencyto);


                    strSQL.Append(" update departmentsub set SubName=@SubName,Type=@Type,");
                    strSQL.Append(" LastChangedBy=@LastChangedBy,LastChangedDate=@LastChangedDate, FAAccountId = @FAAccountId, ");
                    strSQL.Append(" Active=@Active,EmergencyChargesForOPD=@EmergencyChargesForOPD ,EmergencyChargesForIPD=@EmergencyChargesForIPD,EmergencyChargesFromTime=@EmergencyChargesFromTime,EmergencyChargesToTime=@EmergencyChargesToTime where SubDeptId=@SubDeptId and HospitalLocationID=@HospitalLocationID");
                    dl.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), hshIn);
                    strSQL = new StringBuilder();
                    hshIn = new Hashtable();
                    hshIn.Add("@SubDeptId", gvDepartmentSub.SelectedRow.Cells[0].Text);
                    hshIn.Add("@FAAccountId", bc.ParseQ(txtDeptSubFAAccountId.Text.ToString()));
                    strSQL.Append(" Update ItemOfService Set FAAccountId = @FAAccountId Where SubDeptId = @SubDeptId and Active = 1 ");
                    dl.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), hshIn);

                    BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
                    Hashtable output = master.SaveUpdateReportTagging(Convert.ToInt32(gvDepartmentSub.SelectedRow.Cells[0].Text),
                       Convert.ToInt32(ddlReportTagging.SelectedValue), true);

                    populateSubDeptGrid();

                    Alert.ShowAjaxMsg("Updated  Successfully...", Page);
                    lblMessage.Text = "Updated  Successfully...";
                    txtSubDeptName.Text = "";
                    ddlDeptType.SelectedIndex = 0;
                    //Response.Redirect("/MPages/DepartmentSub.aspx?DID=" + ddlMainDept.SelectedItem.Value);
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select The Sub Department Before Updating...", this.Page);
                    return;
                }

            }
            else if (ViewState["Mode"].ToString() == "New")
            {
                if (txtSubDeptName.Text.ToString().Trim().Length == 0)
                {
                    Alert.ShowAjaxMsg("Sub Department Name Cannot Be Blank...", this.Page);
                    return;
                }

                if (ddlDeptType.SelectedIndex == 0)
                {
                    Alert.ShowAjaxMsg("Please Select Department Type...", this.Page);
                    ddlDeptType.Focus();
                    return;
                }
                DAL.DAL dl;
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                strSQL = new StringBuilder();
                strSQL.Remove(0, strSQL.Length);
                Hashtable hshIn = new Hashtable();
                string itypeId;


                itypeId = (string)dl.ExecuteScalar(CommandType.Text, "SELECT DISTINCT convert(varchar,ID) id FROM DepartmentType WHERE DepartmentType = '" + ddlDeptType.SelectedValue + "'");
                hshIn.Add("@SubName", bc.ParseQ(txtSubDeptName.Text.ToString()));
                hshIn.Add("@Type", ddlDeptType.SelectedItem.Value);
                hshIn.Add("@TypeID", itypeId);
                hshIn.Add("@DepartmentID", ddlMainDept.SelectedItem.Value);
                hshIn.Add("@EncodedBy", Convert.ToInt32(Session["UserID"]));
                hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
                hshIn.Add("@FAAccountId", bc.ParseQ(txtDeptSubFAAccountId.Text.ToString()));
                hshIn.Add("@EmergencyChargesForOPD", Chkopemergency.Checked);
                hshIn.Add("@EmergencyChargesForIPD", Chkipemergency.Checked);
                hshIn.Add("@EmergencyChargesFromTime", emergencyfrom);
                hshIn.Add("@EmergencyChargesToTime", emergencyto);



                strSQL.Append(" Insert into departmentsub (SubName,DepartmentTypeId,Type,DepartmentID,EncodedBy,HospitalLocationID, FAAccountId,EmergencyChargesForOPD,EmergencyChargesForIPD,EmergencyChargesFromTime,EmergencyChargesToTime) values (");
                strSQL.Append(" @SubName,@TypeID,@Type,@DepartmentID,@EncodedBy,@HospitalLocationID,@FAAccountId,@EmergencyChargesForOPD,@EmergencyChargesForIPD,@EmergencyChargesFromTime,@EmergencyChargesToTime)");
                dl.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), hshIn);
                Alert.ShowAjaxMsg("Saved Successfully...", Page);

                txtSubDeptName.Text = "";
                ddlDeptType.SelectedIndex = 0;
                ddlMainDept_OnSelectedIndexChanged(sender, e);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void SubNew_OnClick(Object sender, EventArgs e)
    {
        ViewState["DepartmentMode"] = "New";
        Response.Redirect("/MPages/DepartmentSub.aspx", false);
    }

    private void populateMainDept()
    {
        DAL.DAL dl;
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        strSQL = new StringBuilder();
        Hashtable hshIn = new Hashtable();
        strSQL.Remove(0, strSQL.Length);
        try
        {
            hshIn.Add("@Active", "1");
            hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"].ToString()));

            strSQL.Append(" select departmentname,departmentid from departmentmain ");
            strSQL.Append(" where Active =@Active and HospitalLocationID=@HospitalLocationID order by departmentname asc ");
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
            ddlMainDept.DataSource = dr;
            ddlMainDept.DataTextField = "departmentname";
            ddlMainDept.DataValueField = "departmentid";
            ddlMainDept.DataBind();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlMainDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        populateSubDeptGrid();
        txtSubDeptName.Text = "";
        populateSubDeptReportTagging();

        //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ASPL", "javascript:UpperTab(1)", true);
    }

    private void populateSubDeptReportTagging()
    {
        try
        {
            BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
            DataSet ds = master.GetSubDeptReportTagging();

            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "Issubdeptgroup=1";

            ddlReportTagging.DataSource = dv.ToTable();
            ddlReportTagging.DataTextField = "TaggingName";
            ddlReportTagging.DataValueField = "TaggingId";
            ddlReportTagging.DataBind();
            ddlReportTagging.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Select"));
            ddlReportTagging.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void populateSubDeptGrid()
    {
        DAL.DAL dl;
        ds = new DataSet();
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        strSQL = new StringBuilder();
        Hashtable hshIn = new Hashtable();
        try
        {
            hshIn.Add("@departmentid", ddlMainDept.SelectedItem.Value);
            hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"].ToString()));
            strSQL.Remove(0, strSQL.Length);
            //strSQL.Append(" select subdeptid,ds.subname, ds.Type,case ds.Active when 'True' then '1' when 'False' then '0' End as Active, FAAccountId from departmentsub ds");
            //strSQL.Append(" where departmentid=@departmentid and ds.HospitalLocationID=@HospitalLocationID order by ds.subname ASC");
            strSQL.Append(" SELECT subdeptid,ds.subname, ds.Type,case ds.Active WHEN 'True' THEN '1' WHEN 'False' THEN '0' END AS Active, FAAccountId ,rtd.TaggingId,isnull(EmergencyChargesForOPD,0) EmergencyChargesForOPD,isnull(EmergencyChargesForIPD,0) EmergencyChargesForIPD,isnull(EmergencyChargesFromTime,getdate()) EmergencyChargesFromTime,isnull(EmergencyChargesToTime,getdate()) EmergencyChargesToTime FROM departmentsub ds ");
            strSQL.Append(" LEFT JOIN ReportTaggingDetail rtd ON rtd.ServiceId=ds.SubDeptId AND rtd.IsSubDept=1 AND rtd.Active=1 WHERE departmentid=@departmentid AND ds.HospitalLocationID=@HospitalLocationID And ds.Active = 1 ORDER BY ds.subname ASC");
            ds = dl.FillDataSet(CommandType.Text, strSQL.ToString(), hshIn);
            gvDepartmentSub.DataSource = ds;
            gvDepartmentSub.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        for (int i = 0; i < this.gvDepartmentSub.Rows.Count; i++)
        {
            Page.ClientScript.RegisterForEventValidation(this.gvDepartmentSub.UniqueID, "Select$" + i);
        }

        for (int i = 0; i < this.gvDepartmentMain.Rows.Count; i++)
        {
            Page.ClientScript.RegisterForEventValidation(this.gvDepartmentMain.UniqueID, "Select$" + i);
        }
        // base.Render(writer);

        base.Render(writer);
    }

    protected void gvDepartmentSub_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDepartmentSub, "Select$" + e.Row.RowIndex);
        }

        //if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Edit) &&
        //(e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
        //{
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        //gvDepartmentSub.Columns[0].Visible = false;
        //}
    }

    protected void grd_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Mode"] = "Edit";


        txtSubDeptName.Text = gvDepartmentSub.SelectedRow.Cells[1].Text.ToString();
        ddlDeptType.SelectedValue = gvDepartmentSub.SelectedRow.Cells[2].Text.Trim().ToString();
        ddlDeptType.Enabled = false;
        rblStatus.SelectedValue = gvDepartmentSub.SelectedRow.Cells[3].Text.ToString();
        txtDeptSubFAAccountId.Text = gvDepartmentSub.SelectedRow.Cells[4].Text.ToString().Replace("&nbsp;", "");
        Chkopemergency.Checked = Convert.ToBoolean(gvDepartmentSub.SelectedRow.Cells[5].Text.ToString());
        Chkipemergency.Checked = Convert.ToBoolean(gvDepartmentSub.SelectedRow.Cells[6].Text.ToString());

        rademergencyfrom.SelectedDate = Convert.ToDateTime(gvDepartmentSub.SelectedRow.Cells[7].Text.ToString());
        rademergencyto.SelectedDate = Convert.ToDateTime(gvDepartmentSub.SelectedRow.Cells[8].Text.ToString());

        HiddenField hdnTaggingId = (HiddenField)gvDepartmentSub.SelectedRow.FindControl("hdnTaggingId");// add by Prashant 24/06/2014
        ddlReportTagging.SelectedValue = hdnTaggingId.Value;
        hdnSubdeptid.Value= gvDepartmentSub.SelectedRow.Cells[0].Text.ToString();

    }

    //////////////For DepartMent Main Related Code

    protected void New_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/MPages/departmentsub.aspx", false);
    }

    protected void SaveDepartmentMain_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if (txtDeptFAConsumptionAccountId.Text == "" || common.myStr(txtDeptFAConsumptionAccountId.Text) == "0")
            {
                Alert.ShowAjaxMsg("Consumption AccountID Cannot Be Blank or zero...", this.Page);
                return;
            }
            else if (txtDeptFARevenueAccountId.Text == "" || common.myStr(txtDeptFARevenueAccountId.Text) == "0")
            {

                Alert.ShowAjaxMsg("Revenue AccountID Cannot Be Blank or zero ...", this.Page);
                return;
            }
            else if (txtInventoryCode.Text == "" || common.myStr(txtInventoryCode.Text) == "0")
            {
                Alert.ShowAjaxMsg("Inventory Code Cannot Be Blank or zero  ...", this.Page);
                return;
            }

            if (ViewState["DepartmentMode"].ToString() == "Edit")
            {
                if (ViewState["DeptID"] != null)
                {
                    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    strSQL = new StringBuilder();
                    strSQL.Remove(0, strSQL.Length);
                    Hashtable hshIn = new Hashtable();

                    if (rblStatusDepartment.SelectedItem.Value == "0")
                    {
                        hshIn = new Hashtable();
                        hshIn.Add("@DepartmentID", ViewState["DeptID"].ToString());
                        strSQL.Append("select active from departmentsub where departmentid=@DepartmentID and active=1");
                        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
                        if (dr.HasRows == true)
                        {
                            dr.Close();
                            Alert.ShowAjaxMsg("Active Sub Departments Are There For The Selected Department, So Cannot Make It Inactive...", this.Page);
                            return;
                        }
                        dr.Close();
                    }

                    hshIn = new Hashtable();
                    hshIn.Add("@DepartmentID", ViewState["DeptID"].ToString());
                    hshIn.Add("@departmentname", bc.ParseQ(txtDeptName.Text.ToString()));
                    hshIn.Add("@shortname", bc.ParseQ(txtShrtName.Text.ToString()));
                    hshIn.Add("@PrintHeadName", bc.ParseQ(txtHeadName.Text.ToString()));
                    hshIn.Add("@FacilityId", common.myInt(ddlFacility.SelectedValue));
                    hshIn.Add("@LastChangedBy", Convert.ToInt32(Session["UserID"]));
                    hshIn.Add("@LastChangedDate", DateTime.Now);
                    hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
                    hshIn.Add("@Active", rblStatusDepartment.SelectedItem.Value);
                    hshIn.Add("@FAConsumptionAccountId", bc.ParseQ(txtDeptFAConsumptionAccountId.Text.ToString()));
                    hshIn.Add("@FARevenueAccountId", bc.ParseQ(txtDeptFARevenueAccountId.Text.ToString()));
                    hshIn.Add("@SubInventoryCode", bc.ParseQ(txtInventoryCode.Text.ToString()));
                    hshIn.Add("@DepartmentSpecification", common.myStr(txtCenterExecellence.Text));//New added on 23-08-2014 
                    hshIn.Add("@DepartmentContactNo", common.myStr(txtContactno.Text));//New added on 23-08-2014 
                    hshIn.Add("@DepartmentEmailId", common.myStr(txtEmailID.Text));//New added on 23-08-2014 
                    hshIn.Add("@bitIsEncounterWithoutConsultation", common.myInt(rdoIsEncounterWithoutConsultation.SelectedValue));
                    hshIn.Add("@chkIsIntimationrequired", common.myBool(chkIsIntimationrequired.Checked));
                    //hshIn.Add("@bitIsAvailableForPrint", common.myInt(rdoIsAvailableForPrint.SelectedValue));//Done by Ujjwal to add print option for invoice 23 nov 2015

                    strSQL.Append(" update departmentmain set departmentname=@departmentname,shortname=@shortname,IsIntimationRequired=@chkIsIntimationrequired,");
                    strSQL.Append(" PrintHeadName=@PrintHeadName, FacilityId = @FacilityId, LastChangedBy=@LastChangedBy,LastChangedDate=@LastChangedDate, FAConsumptionAccountId = @FAConsumptionAccountId, FARevenueAccountId = @FARevenueAccountId, OracleSubInventoryName = @SubInventoryCode, ");
                    strSQL.Append(" DepartmentSpecification=@DepartmentSpecification,DepartmentContactNo=@DepartmentContactNo,DepartmentEmailId=@DepartmentEmailId, ");
                    //strSQL.Append(" IsAvailableForPrint = @bitIsAvailableForPrint, ");//Done by Ujjwal to add print option for invoice 23 nov 2015
                    strSQL.Append(" IsEncounterWithoutConsultation = @bitIsEncounterWithoutConsultation, Active=@Active where DepartmentID=@DepartmentID and HospitalLocationID=@HospitalLocationID");

                    dl.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), hshIn);
                    Alert.ShowAjaxMsg("Updated  Successfully...", this.Page);
                    ClearDepartmentFields();

                    populateDepartment();
                    //ddlDeptType.Items.FindByText(txtDeptName.Text).Selected = true;


                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Any Department Before Updating", this.Page);
                    return;
                }
            }
            else if (ViewState["DepartmentMode"].ToString() == "New")
            {
                if (txtDeptName.Text.ToString().Trim().Length == 0)
                {
                    Alert.ShowAjaxMsg("Department Name Cannot Be Blank...", this.Page);
                    return;
                }
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                strSQL = new StringBuilder();

                strSQL.Remove(0, strSQL.Length);
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@departmentname", bc.ParseQ(txtDeptName.Text.ToString()));
                hshIn.Add("@shortname", bc.ParseQ(txtShrtName.Text.ToString()));
                hshIn.Add("@PrintHeadName", bc.ParseQ(txtHeadName.Text.ToString()));
                if (common.myInt(ddlFacility.SelectedValue) > 0)
                    hshIn.Add("@FacilityId", common.myInt(ddlFacility.SelectedValue));
                else
                    hshIn.Add("@FacilityId", common.myInt(Session["FacilityId"]));
                hshIn.Add("@EncodedBy", Convert.ToInt32(Session["UserID"]));
                hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
                hshIn.Add("@FAConsumptionAccountId", bc.ParseQ(txtDeptFAConsumptionAccountId.Text.ToString()));
                hshIn.Add("@FARevenueAccountId", bc.ParseQ(txtDeptFARevenueAccountId.Text.ToString()));
                hshIn.Add("@SubInventoryCode", bc.ParseQ(txtInventoryCode.Text.ToString()));
                hshIn.Add("@DepartmentSpecification", common.myStr(txtCenterExecellence.Text));//New added on 23-08-2014 
                hshIn.Add("@DepartmentContactNo", common.myStr(txtContactno.Text));//New added on 23-08-2014 
                hshIn.Add("@DepartmentEmailId", common.myStr(txtEmailID.Text));//New added on 23-08-2014 
                hshIn.Add("@bitIsEncounterWithoutConsultation", common.myInt(rdoIsEncounterWithoutConsultation.SelectedValue));
                //hshIn.Add("@bitIsAvailableForPrint", common.myInt(rdoIsAvailableForPrint.SelectedValue));//Done by Ujjwal to add print option for invoice 23 nov 2015
                hshIn.Add("@chkIsIntimationrequired", common.myBool(chkIsIntimationrequired.Checked));
                strSQL.Append(" Insert into departmentmain (IsIntimationRequired,departmentname,shortname,PrintHeadName, FacilityId, EncodedBy,HospitalLocationID, FAConsumptionAccountId,FARevenueAccountId, OracleSubInventoryName,DepartmentSpecification,DepartmentContactNo,DepartmentEmailId, IsEncounterWithoutConsultation) values (");
                strSQL.Append(" @chkIsIntimationrequired,@departmentname,@shortname,@PrintHeadName, @FacilityId, @EncodedBy,@HospitalLocationID, @FAConsumptionAccountId, @FARevenueAccountId, @SubInventoryCode,@DepartmentSpecification,@DepartmentContactNo,@DepartmentEmailId, @bitIsEncounterWithoutConsultation)");

                dl.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), hshIn);
                Alert.ShowAjaxMsg("Saved Successfully...", this.Page);
                populateDepartment();
                //ddlDeptType.Items.FindByText(txtDeptName.Text).Selected = true;
                ClearDepartmentFields();
            }

            populateMainDept();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnBillingGroup_OnClick(Object sender, EventArgs e)
    {

        RadWindow3.NavigateUrl = "TaggingBillingCategory.aspx";
        RadWindow3.Height = 650;
        RadWindow3.Width = 1020;
        RadWindow3.Top = 40;
        RadWindow3.Left = 100;
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow3.Modal = true;
        RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow3.VisibleStatusbar = false;
    }
    protected void gvDepartmentMain_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        //e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
        e.Row.Cells[10].Visible = false;
        e.Row.Cells[11].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDepartmentMain, "Select$" + e.Row.RowIndex.ToString(),true);
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDepartmentMain, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvDepartmentMain_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ltrlEncodedByDisplay.Text = "";
            ltrlUpdateByDisplay.Text = "";
            gvDepartmentMain.SelectedRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#BEDBFF");
            ViewState["DepartmentMode"] = "Edit";
            ViewState["DeptID"] = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[0].Text.ToString();

            //txtDeptName.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[1].Text.ToString();
            //rblStatusDepartment.SelectedValue = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[2].Text.ToString();

            //ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(common.myInt(gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[3].Text))));
            //txtDeptFAConsumptionAccountId.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[4].Text.ToString().Replace("&nbsp;", "");
            //txtDeptFARevenueAccountId.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[5].Text.ToString().Replace("&nbsp;", "");
            //txtInventoryCode.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[5].Text.ToString().Replace("&nbsp;", "");
            //txtCenterExecellence.Text    = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[6].Text.ToString().Replace("&nbsp;", "");
            //txtContactno.Text   = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[7].Text.ToString().Replace("&nbsp;", "");
            // txtEmailID.Text   = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[8].Text.ToString().Replace("&nbsp;", "");

            txtDeptName.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[1].Text.ToString();
            rblStatusDepartment.SelectedValue = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[2].Text.ToString();

            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(common.myInt(gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[3].Text))));
            txtDeptFAConsumptionAccountId.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[4].Text.ToString().Replace("&nbsp;", "");
            txtDeptFARevenueAccountId.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[5].Text.ToString().Replace("&nbsp;", "");
            txtInventoryCode.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[6].Text.ToString().Replace("&nbsp;", "");
            txtCenterExecellence.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[7].Text.ToString().Replace("&nbsp;", "");
            txtContactno.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[8].Text.ToString().Replace("&nbsp;", "");
            txtEmailID.Text = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[9].Text.ToString().Replace("&nbsp;", "");

            if (common.myBool(gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[10].Text))
            {
                rdoIsEncounterWithoutConsultation.SelectedValue = "1";
            }
            else
            {
                rdoIsEncounterWithoutConsultation.SelectedValue = "0";
            }
            chkIsIntimationrequired.Checked = common.myBool(gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[11].Text);
            //if (common.myBool(gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[11].Text))
            //{
            //    rdoIsAvailableForPrint.SelectedValue = "1";
            //}
            //else
            //{
            //    rdoIsAvailableForPrint.SelectedValue = "0";
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void populateControls()
    {
        populateDepartment();
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        populateDepartment();
    }

    private void populateDepartment()
    {
        DataSet ds = new DataSet();
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();

            hshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hshIn.Add("@chvDepartmentName", common.myStr(txtdepartment.Text).Trim());
            hshIn.Add("@btStatus", common.myInt(rblStatusDepartment.SelectedValue));

            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetDepartment", hshIn);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }
            else
            {
                gvDepartmentMain.DataSource = ds;
            }

            gvDepartmentMain.DataSource = ds;
            gvDepartmentMain.DataBind();
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
        }
    }

    private void ClearDepartmentFields()
    {
        txtDeptName.Text = string.Empty;
        txtShrtName.Text = string.Empty;
        txtHeadName.Text = string.Empty;
        txtCenterExecellence.Text = string.Empty;
        txtContactno.Text = string.Empty;
        txtEmailID.Text = string.Empty;
        txtDeptFAConsumptionAccountId.Text = string.Empty;
        txtDeptFARevenueAccountId.Text = string.Empty;
        txtInventoryCode.Text = string.Empty;

        ddlFacility.SelectedIndex = 0;
        rdoIsEncounterWithoutConsultation.SelectedValue = "0";
        //rdoIsAvailableForPrint.SelectedValue = "1";
    }

    ////////////End Department Main Code

    protected void btnDistagging_Click(object sender, EventArgs e)
    {
        if (common.myInt(hdnSubdeptid.Value) > 0)
        {
            string Dept_Id = ddlMainDept.SelectedValue;
            string Subdept_Id = hdnSubdeptid.Value;            
            RadWindow3.NavigateUrl = "/MPages/Company/DiscountTagging.aspx?deptId=" + ddlMainDept.SelectedItem.Value + "&SubDeptid=" + Convert.ToInt32(hdnSubdeptid.Value) + "";
            RadWindow3.Height = 600;
            RadWindow3.Width = 900;
            RadWindow3.Top = 40;
            RadWindow3.Left = 100;
            //RadWindow3.OnClientClose = "OnClientClose";
            RadWindow3.Modal = true;
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindow3.VisibleStatusbar = false;
        }
        else
        {
            Alert.ShowAjaxMsg("Please Select Sub Department First", this.Page);
            return;
        }
    }
}
