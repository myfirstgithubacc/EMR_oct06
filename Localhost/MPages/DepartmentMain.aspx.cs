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
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;

public partial class MPages_DepartmentMain : BasePage
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.ParseData bc = new BaseC.ParseData();
    clsExceptionLog objException = new clsExceptionLog();
    StringBuilder strSQL;
    DataSet ds;
    SqlDataReader dr;
    DAL.DAL dl;
    Hashtable hshIn;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            ibtnSave.Visible = true;
            ibtnUpdate.Visible = false;
            populateControls();

            if (Request.QueryString["Mode"] == "Edit")
            {
                ibtnSave.Visible = false;
                ibtnUpdate.Visible = true;
                rblStatus.Visible = true;
                ltrlStatus.Visible = true;
            }
            else
            {
                ibtnSave.Visible = true;
                ibtnUpdate.Visible = false;
                rblStatus.Visible = false;
                ltrlStatus.Visible = false;
                //gvDepartmentMain.Enabled = false;
            }

            ltrlEncodedByDisplay.Text = "";
            ltrlUpdateByDisplay.Text = "";
            ltrlEncodedBy.Visible = false;
            ltrlUpdateBy.Visible = false;

        }
    }

    protected void New_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/MPages/DepartmentMain.aspx", false);
        ibtnSave.Visible = true;
        ibtnUpdate.Visible = false;
    }

    protected void Edit_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/MPages/DepartmentMain.aspx?Mode=Edit", false);
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected override void Render(HtmlTextWriter writer)
    {
        for (int i = 0; i < this.gvDepartmentMain.Rows.Count; i++)
        {
            Page.ClientScript.RegisterForEventValidation(this.gvDepartmentMain.UniqueID, "Select$" + i);
        }
        base.Render(writer);
    }

    protected void SaveDepartmentMain_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if (txtDeptName.Text.ToString().Trim().Length == 0)
            {
                Alert.ShowAjaxMsg("Department Name Cannot Be Blank...", this.Page);
                return;
            }
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL = new StringBuilder();
            using (System.Transactions.TransactionScope Trans = default(System.Transactions.TransactionScope))
            {
                strSQL.Remove(0, strSQL.Length);
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@departmentname", bc.ParseQ(txtDeptName.Text.ToString()));
                hshIn.Add("@shortname", bc.ParseQ(txtShrtName.Text.ToString()));
                hshIn.Add("@PrintHeadName", bc.ParseQ(txtHeadName.Text.ToString()));
                hshIn.Add("@EncodedBy", Convert.ToInt32(Session["UserID"]));
                hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));


                strSQL.Append(" Insert into departmentmain (departmentname,shortname,PrintHeadName,EncodedBy,HospitalLocationID) values (");
                strSQL.Append(" @departmentname,@shortname,@PrintHeadName,@EncodedBy,@HospitalLocationID)");
                dl.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), hshIn);
                Alert.ShowAjaxMsg("Saved Successfully...", this.Page);
                ClearDepartmentFields();
                populateDepartment();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void UpdateDepartmentMain_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Mode"] == "Edit")
            {
                if (ViewState["DeptID"] != null)
                {
                    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    strSQL = new StringBuilder();
                    strSQL.Remove(0, strSQL.Length);
                    Hashtable hshIn = new Hashtable();

                    if (rblStatus.SelectedItem.Value == "0")
                    {
                        hshIn = new Hashtable();
                        hshIn.Add("@DepartmentID", ViewState["DeptID"].ToString());
                        strSQL.Append("select active from departmentsub where departmentid=@DepartmentID and active=1");
                        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
                        if (dr.HasRows == true)
                        {
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
                    hshIn.Add("@LastChangedBy", Convert.ToInt32(Session["UserID"]));
                    hshIn.Add("@LastChangedDate", DateTime.Now);
                    hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
                    hshIn.Add("@Active", rblStatus.SelectedItem.Value);

                    strSQL.Append(" update departmentmain set departmentname=@departmentname,shortname=@shortname,");
                    strSQL.Append(" PrintHeadName=@PrintHeadName,LastChangedBy=@LastChangedBy,LastChangedDate=@LastChangedDate,");
                    strSQL.Append(" Active=@Active where DepartmentID=@DepartmentID and HospitalLocationID=@HospitalLocationID");
                    dl.ExecuteNonQuery(CommandType.Text, strSQL.ToString(), hshIn);
                    Alert.ShowAjaxMsg("Updated  Successfully...", this.Page);
                    ClearDepartmentFields();
                    populateDepartment();
                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Any Department Before Updating", this.Page);
                    return;
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please Click Edit Button And Then Continue Updating...", this.Page);
                ClearDepartmentFields();
            }
            ViewState["DeptID"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDepartmentMain_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDepartmentMain, "Select$" + e.Row.RowIndex.ToString(),true);
            e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvDepartmentMain, "Select$" + e.Row.RowIndex);

        }

        //if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Edit) &&
        //(e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
        //{
        //    e.Row.Cells[0].Visible = false;
        //}
    }

    protected void gvDepartmentMain_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ltrlEncodedByDisplay.Text = "";
            ltrlUpdateByDisplay.Text = "";

            if (Request.QueryString["Mode"] != "Edit")
            {
                ltrlEncodedBy.Visible = false;
                ltrlUpdateBy.Visible = false;
                //gvDepartmentMain.SelectedRowStyle.BackColor = System.Drawing.Color.White;
                return;
            }
            else
            {
                gvDepartmentMain.SelectedRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#BEDBFF");
            }

            ltrlEncodedBy.Visible = true;
            ltrlUpdateBy.Visible = true;

            //ViewState["Edit"] = "TRUE";
            ViewState["DeptID"] = gvDepartmentMain.Rows[gvDepartmentMain.SelectedIndex].Cells[0].Text.ToString();

            DAL.DAL dl;
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL = new StringBuilder();
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
            hshIn.Add("@departmentID", ViewState["DeptID"].ToString());

            strSQL.Append(" Select dm.Shortname,dm.departmentname,dm.printheadname,convert(int,dm.active) as active,");
            strSQL.Append(" emp1.firstname+space(2)+emp1.Middlename+space(2)+ emp1.LastName as EncodedBy ,");
            strSQL.Append(" Convert(varchar,dm.encodeddate,103) as Encodeddate, substring( convert(varchar,dm.encodeddate,108),1,5) as EncodedTime,");
            strSQL.Append(" ISNULL(emp2.firstname,'')+space(2)+ISNULL(emp2.Middlename,'')+space(2)+ ISNULL(emp2.LastName,'') as LastChangedBy ,");
            strSQL.Append(" ISNULL(Convert(varchar,dm.lastchangeddate,103),'') as LastCHangedDate,substring( convert(varchar,dm.lastchangeddate,108),1,5) as LastChangedTime");
            strSQL.Append(" from departmentmain dm");
            strSQL.Append(" inner join employee emp1 on dm.encodedby=emp1.ID");
            strSQL.Append(" left join employee emp2 on dm.lastchangedby=emp2.ID");
            strSQL.Append(" where dm.HospitalLocationID=@HospitalLocationID and dm.departmentID=@departmentID");


            //strSQL.Append(" Select Shortname,departmentname,printheadname,convert(int,active) as active from departmentmain");
            //strSQL.Append(" where HospitalLocationID=@HospitalLocationID and departmentID=@departmentID");
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
            if (dr.Read())
            {
                txtDeptName.Text = dr["departmentname"].ToString();
                txtShrtName.Text = dr["Shortname"].ToString();
                txtHeadName.Text = dr["printheadname"].ToString();
                rblStatus.SelectedValue = dr["active"].ToString();

                ltrlEncodedByDisplay.Text = dr["EncodedBy"].ToString() + " - " + dr["Encodeddate"].ToString() + "  " + dr["EncodedTime"].ToString();
                ltrlUpdateByDisplay.Text = dr["LastChangedBy"].ToString() + " - " + dr["LastCHangedDate"].ToString() + "  " + dr["LastChangedTime"].ToString();

            }
            dr.Close();

            ibtnSave.Visible = false;
            ibtnUpdate.Visible = true;
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

    private void populateDepartment()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL = new StringBuilder();
            Hashtable hshIn = new Hashtable();

            hshIn.Add("@HospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
            strSQL.Remove(0, strSQL.Length);
            strSQL.Append(" select departmentid,departmentname from departmentmain where HospitalLocationID=@hospitallocationid order by departmentname asc");
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
            //if (dr.HasRows == true)
            //{
            gvDepartmentMain.DataSource = dr;
            gvDepartmentMain.DataBind();
            dr.Close();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void ClearDepartmentFields()
    {
        txtDeptName.Text = "";
        txtShrtName.Text = "";
        txtHeadName.Text = "";
    }

}
