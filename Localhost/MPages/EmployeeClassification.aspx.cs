using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;

public partial class MPages_EmployeeClassification : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                BindEmployeeList();
                BindEmployeeType();
                ShowGrid();
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = ex.Message;
            }
        }
    }

    private void BindEmployeeType()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        StringBuilder sbSQL = new StringBuilder();
        sbSQL.Append(" SELECT ID, Description, EmployeeType FROM EmployeeType WHERE Active = 1 AND EmployeeType In ('D','E','N')  ORDER BY Description");
        ds = dl.FillDataSet(CommandType.Text, sbSQL.ToString());
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dr["Description"];
                    item.Value = dr["Id"].ToString();
                    item.Attributes.Add("EmployeeType", dr["EmployeeType"].ToString());
                    ddlEmployeeType.Items.Add(item);
                    item.DataBind();
                }
                ddlEmployeeType.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
                ddlEmployeeType.SelectedIndex = 0;
            }
        }
    }

    private void BindEmployeeList()
    {
        BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
        StringBuilder strType = new StringBuilder();
        ArrayList coll = new ArrayList();
        DataSet ds = new DataSet();
        if (common.myInt(ddlEmployeeType.SelectedValue) == 0)
        {
            coll.Add("D");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("E");
            strType.Append(common.setXmlTable(ref coll));
            coll.Add("N");
            strType.Append(common.setXmlTable(ref coll));
        }
        else
        {
            coll.Add(ddlEmployeeType.SelectedItem.Attributes["EmployeeType"].ToString());
            strType.Append(common.setXmlTable(ref coll));
        }
     
        int iHospId = common.myInt(Session["HospitalLocationID"]);
        
        ds = objMaster.getEmployeeData(iHospId, 0, 0, strType.ToString(), "", 0, common.myInt(Session["UserId"]),"",common.myInt(Session["FacilityId"]));

        ddlEmployee.DataSource = ds.Tables[0].Copy();
        ddlEmployee.DataValueField = "EmployeeId";
        ddlEmployee.DataTextField = "EmployeeName";
        ddlEmployee.DataBind();

        ddlEmployee.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
        ddlEmployee.SelectedIndex = 0;

        if (common.myInt(Request.QueryString["EmpId"]) != 0)
        {
            ddlEmployee.SelectedIndex = ddlEmployee.Items.IndexOf(ddlEmployee.Items.FindItemByValue(Request.QueryString["EmpId"].ToString()));
        }
    }

    public DataTable BindDropClassification()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        StringBuilder sbSQL = new StringBuilder();
        Hashtable hsinput = new Hashtable();
        hsinput.Add("@intHospitalLocationId", Session["HospitalLocationID"].ToString());
        sbSQL.Append("SELECT ID, Name FROM SurgeryDoctorClassification ");
        sbSQL.Append("WHERE HospitalLocationID = @intHospitalLocationId AND Active=1  ORDER BY SequenceNo");
        ds = dl.FillDataSet(CommandType.Text, sbSQL.ToString(), hsinput);

        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        DataRow dr;
        dr = dt.NewRow();
        dr["ID"] = 0;
        dr["Name"] = "--Select--";
        dt.Rows.InsertAt(dr, 0);
        return dt;
    }

    public DataTable BindDropSpecialisation()
    {
        BaseC.clsEMR objEmr = new BaseC.clsEMR(sConString);
        DataSet ds = objEmr.getSpecialisationMaster(common.myInt(ViewState["SpecialisationId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);
        DataRow dr = ds.Tables[0].NewRow();
        dr["SpecialisationId"] = 0;
        dr["SpecialisationName"] = "--Select--";
        ds.Tables[0].Rows.InsertAt(dr,0);
        ds.Tables[0].AcceptChanges();
        return ds.Tables[0];
    }
            


    protected void ddlEmployeeType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindEmployeeList();
        ShowGrid();
    }
    protected void ddlEmployee_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ShowGrid();
    }

    private void ShowGrid()
    {
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        DataSet ds = new DataSet();
        ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlEmployeeType.SelectedValue), common.myInt(ddlEmployee.SelectedValue), common.myInt(Session["FacilityId"]), 0, 0);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds.Tables[0];
                gvDetails.DataBind();
            }
        }
    }

    protected void gvDetails_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            ShowGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
    }

    protected void gvDetails_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            try
            {                

                GridEditFormItem editedItem = (GridEditFormItem)e.Item;
                HiddenField hdnEmployeeId = (HiddenField)editedItem.FindControl("hdnEmployeeId");
                RadComboBox ddlClassification = (RadComboBox)editedItem.FindControl("ddlClassification");
                RadComboBox ddlSpecialisation = (RadComboBox)editedItem.FindControl("ddlSpecialisation");


                //if (common.myInt(ddlClassification.SelectedValue) == 0)
                //{
                //    lblMessage.Text = "Please Select Classification !";
                //    ShowGrid();
                //    return;
                //}
                
                BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
                int HospId = common.myInt(Session["HospitalLocationId"]);
                int EmployeeId = common.myInt(hdnEmployeeId.Value);
                int ClassificationId = common.myInt(ddlClassification.SelectedValue);
                int SpecialisationId = common.myInt(ddlSpecialisation.SelectedValue);
                int UserId = common.myInt(Session["UserId"]);
               Hashtable htOut = master.SaveUpdateEmployeeClassification(HospId, Convert.ToInt16(Session["FacilityId"]), EmployeeId, ClassificationId, UserId, SpecialisationId);
                if (htOut.Count > 0)
                {
                    if (common.myStr(htOut["chvErrorStatus"]).Contains("successfully"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMessage.Text = common.myStr(htOut["chvErrorStatus"]);                       
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = common.myStr(htOut["chvErrorStatus"]);
                    }
                }               
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;                
            }
        }
        ShowGrid();
    }

    protected void gvDetails_CancelCommand(object source, GridCommandEventArgs e)
    {
        ShowGrid();
    }

    protected void gvDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }

        if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        {
            
            RadComboBox ddlClassification = (RadComboBox)e.Item.FindControl("ddlClassification");
            HiddenField hdnClassificationId = (HiddenField)e.Item.FindControl("hdnClassificationId");
            if(common.myInt(hdnClassificationId.Value) !=0)
            {
                ddlClassification.SelectedIndex = ddlClassification.Items.IndexOf(ddlClassification.Items.FindItemByValue(hdnClassificationId.Value));
            }

            //GridEditFormItem editedItem = (GridEditFormItem)e.Item;
            //Label lblEFieldType = (Label)editedItem.FindControl("lblEFieldType");

            //if (lblEFieldType.Text == "N" || lblEFieldType.Text == "F")
            //{
            //    TextBox txtResult = (TextBox)e.Item.FindControl("txtResult");
            //    FilteredTextBoxExtender filter1 = (FilteredTextBoxExtender)e.Item.FindControl("filter1");
            //    filter1.Enabled = true;
            //}
        }
    }

    protected void gvDetails_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        ShowGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
    protected void lnkEmployeeLookup_OnClick(object sebder, EventArgs e)
    {
        Response.Redirect("~/mpages/EmployeeLockUp.aspx", false);
    }
    protected void lnkProviderProfile_OnClick(object sender, EventArgs e)
    {
        if (ViewState["emp"] != null)
        {
            Response.Redirect("/MPages/DoctorProfileMaster.aspx?EmpNo=" + ViewState["emp"].ToString().Trim(), false);
        }
        else
        {
            Response.Redirect("/MPages/DoctorProfileMaster.aspx", false);
        }
    }
    protected void lnkAppointmentTemplate_OnClick(object sender, EventArgs e)
    {
        if (ViewState["emp"] != null)
        {
            Response.Redirect("/mpages/providertimings.aspx?EmpNo=" + ViewState["emp"].ToString().Trim(), false);
        }
        else
        {
            Response.Redirect("/mpages/providertimings.aspx", false);
        }
    }

    protected void lnkProviderDetails_OnClick(object sender, EventArgs e)
    {
        if (ViewState["emp"] != null)
        {
            Response.Redirect("/MPages/ProviderDetails.aspx?EmpNo=" + ViewState["emp"].ToString().Trim(), false);
        }
        else
        {
            Response.Redirect("/MPages/ProviderDetails.aspx", false);
        }
    }
    protected void lnkEmployee_OnClick(object sender, EventArgs e)
    {
        if (ViewState["DoctorId"] != null)
            {
                Response.Redirect("/MPages/employee.aspx?EmpNo=" + ViewState["DoctorId"].ToString().Trim(), false);
            }
            else
            {
                Response.Redirect("/MPages/employee.aspx", false);
            }
       
     
    }
}
