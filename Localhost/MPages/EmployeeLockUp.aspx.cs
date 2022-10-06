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
using System.Text;
using System.Drawing;
using System.IO;

public partial class MPages_EmployeeLockUp : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private Hashtable hshInput;
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.SetFocus(txtFullName);
        if (!IsPostBack)
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //populate Kin Relation drop down control
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT ID,Description FROM EmployeeType");
            ddlEmployeeType.DataSource = dr;
            ddlEmployeeType.DataTextField = "Description";
            ddlEmployeeType.DataValueField = "ID";
            ddlEmployeeType.DataBind();
            ddlEmployeeType.Items.Insert(0, "All");
            ddlEmployeeType.Items[0].Value = "0";
            dr.Close();
            //if (Request.QueryString["Mpg"] != null)
            //    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            //if (Request.QueryString.Count > 1)
            //    //btnok.Visible = true;

            // rdlType.SelectedIndex = 0;

            if (Request.QueryString.Count > 1)
            {
                txtFullName.Text = Request.QueryString["TextBoxValue"].ToString();
                //ddlSearchOption.SelectedValue = Request.QueryString["ddlMode"].ToString();
            }

            Bind_EmployeeGrid();

        }

    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Request.QueryString.Count > 0)
        //if (Request.QueryString["View"] != null)
        {
            //if (Request.QueryString["View"].ToString() == "_Blank")
            //{
            Page.MasterPageFile = "/include/master/BlankMaster.master";

            //}
        }
        else
        {
            Page.MasterPageFile = "/include/master/emrMaster.master";
        }

    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        try
        {
            string scriptString = "";

            if (gvEmployeeDetail.SelectedIndex >= 0)
            {
                scriptString = "<script language='JavaScript'>window.opener.document.getElementById('" + Request.QueryString["TextBoxId"].ToString() + "').value='" + gvEmployeeDetail.SelectedRow.Cells[0].Text + "';window.opener.document.getElementById('ctl00_ContentPlaceHolder1_TabContainer1_TabVisitMaster_btnfind').click();window.close();</script>";
                //  scriptString = "<script language='JavaScript'>window.opener.document.getElementById('" + Request.QueryString["TextBoxId"].ToString() + "').value='" + gvAdmissionResult.SelectedRow.Cells[0].Text + "';window.opener.document.getElementById('" + Request.QueryString["hdnTextBoxId"].ToString() + "').value='A';window.opener.document.forms(0).submit();window.close();</script>";
                //string scriptString = "<script language='JavaScript'>window.opener.document.forms(0).submit(); </script>";
                if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptString))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", scriptString);
                }
            }
            else
            {
                Alert.Show("Please Selct Patient");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind_EmployeeGrid();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        ddlEmployeeType.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        txtFullName.Text = "";
        txtMobileNo.Text = "";
        Bind_EmployeeGrid();
    }

    void Bind_EmployeeGrid()
    {
        try
        {
            DAL.DAL dl;
            BaseC.ParseData bc = new BaseC.ParseData();
            DataSet ds = new DataSet();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            hshInput.Add("@intEmployeeTypeId", ddlEmployeeType.SelectedValue);
            hshInput.Add("@chrEmployeeName", Convert.ToString(txtFullName.Text.Trim()));
            hshInput.Add("@intMobileNo", Convert.ToString(txtMobileNo.Text.Trim()));
           // if (ddlStatus.SelectedValue.ToString() != "")
           // {
                hshInput.Add("@bitStatus", ddlStatus.SelectedValue);
           // }
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", hshInput);
            gvEmployeeDetail.DataSource = ds.Tables[0];
            gvEmployeeDetail.DataBind();
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    lblnoofrows.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) Found.";
            //}
            //else
            //{
            //    lblnoofrows.Text = "";
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvEmployeeDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strquery = "?EmpNo=" + gvEmployeeDetail.SelectedRow.Cells[0].Text;

        Response.Redirect("~/mpages/Employee.aspx" + strquery, false);
        //string scriptString = "";
        //scriptString = "<script language='JavaScript'>window.opener.document.getElementById('" + Request.QueryString["TextBoxId"].ToString() + "').value='" + gvEmployeeDetail.SelectedRow.Cells[0].Text + "';window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnfind').click();window.close();</script>";
        ////  scriptString = "<script language='JavaScript'>window.opener.document.getElementById('" + Request.QueryString["TextBoxId"].ToString() + "').value='" + gvAdmissionResult.SelectedRow.Cells[0].Text + "';window.opener.document.getElementById('" + Request.QueryString["hdnTextBoxId"].ToString() + "').value='A';window.opener.document.forms(0).submit();window.close();</script>";
        ////string scriptString = "<script language='JavaScript'>window.opener.document.forms(0).submit(); </script>";
        //if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptString))
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", scriptString);
        //}
    }

    protected void gvEmployeeDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Header))
        {
            e.Row.Cells[0].Visible = false;
        }

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
        try
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkClassification_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("/MPages/EmployeeClassification.aspx?EmpId=" + common.myInt(ViewState["emp"]), false);
    }
    protected void btnExportToExcel_OnClick(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {

            ExportToExcel expExcel = new ExportToExcel(sConString);

            ds = expExcel.ExportToExcelEmployeeList(common.myInt(ddlEmployeeType.SelectedValue), 0, common.myInt(Session["FacilityId"]));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    //ds.Tables[0].DefaultView.RowFilter = "OPH=" + ddlOPIP.SelectedValue;

                    HttpResponse response = HttpContext.Current.Response;

                    // first let's clean up the response.object
                    response.Clear();
                    response.Charset = "";

                    // set the response mime type for excel
                    response.ContentType = "application/vnd.ms-excel";
                    response.AddHeader("Content-Disposition", "attachment;filename=EmployeeList.xls");

                    // create a string writer
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {
                            // instantiate a datagrid
                            DataGrid dg = new DataGrid();
                            dg.DataSource = ds.Tables[0].DefaultView;
                            dg.DataBind();
                            dg.RenderControl(htw);
                            response.Write(sw.ToString());
                            response.End();
                        }
                    }
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
            ds.Dispose();
        }
    }
}
