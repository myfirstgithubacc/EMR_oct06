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
using System.Text;
using Telerik.Web.UI;
using System.Data.SqlClient;

public partial class MPages_EmployeeSequenceOrder : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDepartment();
            BindEmployeeType();
            bindDepartmentCase();
            BindEmployee();
        }
    }

    void BindEmployeeType()
    {
        BaseC.User objuser = new BaseC.User(sConString);

        try
        {
            DataTable dt = objuser.getEmployeeType();
            ddlEmployeeType.DataSource = dt;
            ddlEmployeeType.DataTextField = "Description";
            ddlEmployeeType.DataValueField = "ID";
            ddlEmployeeType.DataBind();
            ddlEmployeeType.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindDepartment()
    {

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder strSQL = new StringBuilder();
        Hashtable hshIn = new Hashtable();
        strSQL.Remove(0, strSQL.Length);
        try
        {
            hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"].ToString()));
            strSQL.Append(" select departmentname,departmentid from departmentmain ");
            strSQL.Append(" where Active =1 and HospitalLocationID=@HospitalLocationID order by departmentname asc ");
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
            ddlMainDept.DataSource = dr;
            ddlMainDept.DataTextField = "departmentname";
            ddlMainDept.DataValueField = "departmentid";
            ddlMainDept.DataBind();
            ddlMainDept.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    void BindEmployee()
    {
        BaseC.Patient objbc = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();

        try
        {
            ds = objbc.GetEmployeeSequenceNo(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), 
                                            common.myInt(ddlMainDept.SelectedValue), common.myInt(ddlEmployeeType.SelectedValue), 
                                            common.myStr(rdoDepartmentCaseType.SelectedValue), common.myInt(ddlDepartmentCase.SelectedValue));
            
            if (ds.Tables.Count > 0)
            {
                gvEmployeeSequcence.DataSource = ds;
                gvEmployeeSequcence.DataBind();
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
            objbc = null;
            ds.Dispose();
        }
    }
    protected void ddlMainDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "&nbsp;";
        BindEmployee();
    }
   
    
    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "&nbsp;";
        BindEmployee();
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            ArrayList coll = new ArrayList();
            StringBuilder objXML = new StringBuilder();
            int count = 0;
            foreach (GridDataItem row in gvEmployeeSequcence.Items)
            {
                TextBox txtColumnSequenceNo = (TextBox)row.FindControl("txtColumnSequenceNo");

                if (common.myInt(txtColumnSequenceNo.Text) == 0)
                {
                    lblMessage.Text = "Please enter valid column no. !";
                    return;
                }

                count = count + 1;
                coll.Add(common.myInt(((HiddenField)row.FindControl("hdnEmpId")).Value));
                //coll.Add(common.myInt(((HiddenField)row.FindControl("hdnSequenceNo")).Value));
                coll.Add(common.myInt(count));
                coll.Add(common.myInt(txtColumnSequenceNo.Text));

                objXML.Append(common.setXmlTable(ref coll));
            }

            if (objXML.ToString() == "")
            {
                lblMessage.Text = "Employee not exists !";
                return;
            }

            BaseC.Patient objpat = new BaseC.Patient(sConString);
            string strMsg = objpat.SaveEmployeeSequence(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), objXML.ToString(), 
                                                       common.myStr(rdoDepartmentCaseType.SelectedValue), common.myInt(ddlDepartmentCase.SelectedValue));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;
                BindEmployee();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected DataTable createTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("ID");
        Dt.Columns.Add("EmployeeName");
        Dt.Columns.Add("ColumnSequenceNo");
        Dt.Columns.Add("SequenceNo");
        Dt.Columns.Add("SerialNo");
        Dt.Columns["SerialNo"].AutoIncrement = true;
        Dt.Columns["SerialNo"].AutoIncrementSeed = 1;
        Dt.Columns["SerialNo"].AutoIncrementStep = 1;
        return Dt;
    }

    protected void gvEmployeeSequcence_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
            {
                TextBox txtColumnSequenceNo = (TextBox)e.Item.FindControl("txtColumnSequenceNo");
                txtColumnSequenceNo.Attributes.Add("autocomplete", "off");
            }
        }
        catch
        {
        }
    }

    protected void gvSelectedValues_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            if (common.myStr(e.CommandName) == "Down")
            {
                DataTable dt = createTable();
                for (int i = 0; i < gvEmployeeSequcence.Items.Count; i++)
                {
                    int EmpID;

                    string EmployeeName = "";
                    int ColumnSequenceNo = 1;
                    int SequenceNo = 0;
                    EmployeeName = ((Label)gvEmployeeSequcence.Items[i].FindControl("lblEmployeeName")).Text;
                    EmpID = common.myInt(((HiddenField)gvEmployeeSequcence.Items[i].FindControl("hdnEmpId")).Value);
                    ColumnSequenceNo = common.myInt(((TextBox)gvEmployeeSequcence.Items[i].FindControl("txtColumnSequenceNo")).Text);
                    SequenceNo = common.myInt(((HiddenField)gvEmployeeSequcence.Items[i].FindControl("hdnSequenceNo")).Value);
                    DataRow drDt = dt.NewRow();
                    drDt["ID"] = EmpID;
                    drDt["EmployeeName"] = common.myStr(EmployeeName).Trim();
                    drDt["ColumnSequenceNo"] = ColumnSequenceNo;
                    drDt["SequenceNo"] = SequenceNo;
                    dt.Rows.Add(drDt);
                }
                dt.AcceptChanges();
                int SelectedRow = e.Item.ItemIndex;
                int NextRow = SelectedRow + 1;
                int SelectedRowValueId = common.myInt(((HiddenField)gvEmployeeSequcence.Items[SelectedRow].FindControl("hdnSequenceNo")).Value);
                int NextRowValueId = common.myInt(((HiddenField)gvEmployeeSequcence.Items[NextRow].FindControl("hdnSequenceNo")).Value);
                DataRow[] dr = dt.Select("SequenceNo='" + SelectedRowValueId + "'");
                DataRow[] dr1 = dt.Select("SequenceNo='" + NextRowValueId + "'");
                if ((dr1.Length > 0) && (dr.Length > 0))
                {

                    string SelEmpID = common.myStr(dr[0]["ID"]);
                    string SelEmployeeName = common.myStr(dr[0]["EmployeeName"]);
                    int SelColumnSequenceNo = common.myInt(dr[0]["ColumnSequenceNo"]);
                    int SelSequenceNo = common.myInt(dr[0]["SequenceNo"]);

                    string NextEmpID = common.myStr(dr1[0]["ID"]);
                    string NextEmployeeName = common.myStr(dr1[0]["EmployeeName"]);
                    int NextColumnSequenceNo = common.myInt(dr1[0]["ColumnSequenceNo"]);
                    int NextSequenceNo = common.myInt(dr1[0]["SequenceNo"]);

                    dr[0]["ID"] = NextEmpID;
                    dr[0]["EmployeeName"] = NextEmployeeName;
                    dr[0]["ColumnSequenceNo"] = NextColumnSequenceNo;
                    dr[0]["SequenceNo"] = NextSequenceNo;

                    dr1[0]["ID"] = SelEmpID;
                    dr1[0]["EmployeeName"] = SelEmployeeName;
                    dr1[0]["ColumnSequenceNo"] = SelColumnSequenceNo;
                    dr1[0]["SequenceNo"] = SelSequenceNo;

                    gvEmployeeSequcence.DataSource = dt;
                    gvEmployeeSequcence.DataBind();
                    gvEmployeeSequcence.Items[NextRow].Selected = true;
                }
            }
            if (common.myStr(e.CommandName) == "Up")
            {
                DataTable dt = createTable();
                Boolean Normal = false;
                for (int i = 0; i != gvEmployeeSequcence.Items.Count; i++)
                {
                    int EmpID;
                    string EmployeeName = "";
                    int ColumnSequenceNo = 1;
                    int SequenceNo = 0;
                    EmployeeName = ((Label)gvEmployeeSequcence.Items[i].FindControl("lblEmployeeName")).Text;
                    EmpID = common.myInt(((HiddenField)gvEmployeeSequcence.Items[i].FindControl("hdnEmpId")).Value);
                    ColumnSequenceNo = common.myInt(((TextBox)gvEmployeeSequcence.Items[i].FindControl("txtColumnSequenceNo")).Text);
                    SequenceNo = common.myInt(((HiddenField)gvEmployeeSequcence.Items[i].FindControl("hdnSequenceNo")).Value);

                    DataRow drDt = dt.NewRow();
                    drDt["ID"] = EmpID;
                    drDt["EmployeeName"] = common.myStr(EmployeeName).Trim();
                    drDt["ColumnSequenceNo"] = common.myInt(ColumnSequenceNo);
                    drDt["SequenceNo"] = HttpUtility.HtmlDecode(common.myStr(SequenceNo).Trim());

                    dt.Rows.Add(drDt);
                }
                int SelectedRow = e.Item.ItemIndex;
                int NextRow = SelectedRow - 1;
                int SelectedRowValueId = common.myInt(((HiddenField)gvEmployeeSequcence.Items[SelectedRow].FindControl("hdnSequenceNo")).Value);
                int PrevRowValueId = common.myInt(((HiddenField)gvEmployeeSequcence.Items[NextRow].FindControl("hdnSequenceNo")).Value);
                DataRow[] dr = dt.Select("SequenceNo='" + SelectedRowValueId + "'");
                DataRow[] dr1 = dt.Select("SequenceNo='" + PrevRowValueId + "'");
                if ((dr1.Length > 0) && (dr.Length > 0))
                {

                    string SelValueID = common.myStr(dr[0]["ID"]);
                    string SelValueName = common.myStr(dr[0]["EmployeeName"]);
                    int SelColumnSequenceNo = common.myInt(dr[0]["ColumnSequenceNo"]);
                    int SelSequenceNo = common.myInt(dr[0]["SequenceNo"]);

                    string PrevValueID = common.myStr(dr1[0]["ID"]);
                    string PrevValueName = common.myStr(dr1[0]["EmployeeName"]);
                    int PrevColumnSequenceNo = common.myInt(dr1[0]["ColumnSequenceNo"]);
                    int PrevSequenceNo = common.myInt(dr1[0]["SequenceNo"]);

                    dr[0]["ID"] = PrevValueID;
                    dr[0]["EmployeeName"] = PrevValueName;
                    dr[0]["ColumnSequenceNo"] = PrevColumnSequenceNo;
                    dr[0]["SequenceNo"] = PrevSequenceNo;

                    dr1[0]["ID"] = SelValueID;
                    dr1[0]["EmployeeName"] = SelValueName;
                    dr1[0]["ColumnSequenceNo"] = SelColumnSequenceNo;
                    dr1[0]["SequenceNo"] = SelSequenceNo;

                    gvEmployeeSequcence.DataSource = dt;
                    gvEmployeeSequcence.DataBind();
                    gvEmployeeSequcence.Items[NextRow].Selected = true;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


    protected void lnkEmployee_OnClick(object sender, EventArgs e)
    {
        try
        {
            //if (ViewState["DoctorId"] != null)
            //{
            //    Response.Redirect("/MPages/employee.aspx?EmpNo=" + ViewState["DoctorId"].ToString().Trim(), false);
            //}
            //else
            //{
            Response.Redirect("/MPages/employee.aspx", false);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void rdoDepartmentCaseType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        tblSingleDepartmentCase.Visible = false;
        tblMultiDepartmentCase.Visible = false;
        if (common.myStr(rdoDepartmentCaseType.SelectedValue).Equals("M"))
        {
            tblMultiDepartmentCase.Visible = true;
        }
        else
        {
            tblSingleDepartmentCase.Visible = true;
        }

        ddlEmployeeType.SelectedIndex = 0;
        ddlMainDept.SelectedIndex = 0;
        ddlDepartmentCase.SelectedIndex = 0;

        BindEmployee();
    }

    private void bindDepartmentCase()
    {
        BaseC.ICM objICM = new BaseC.ICM(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objICM.getEMRDSMultiDepartmentCase();

            ddlDepartmentCase.DataSource = ds;
            ddlDepartmentCase.DataTextField = "CaseName";
            ddlDepartmentCase.DataValueField = "CaseId";
            ddlDepartmentCase.DataBind();

            ddlDepartmentCase.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlDepartmentCase.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlDepartmentCase_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "&nbsp;";
        BindEmployee();
    }

}
