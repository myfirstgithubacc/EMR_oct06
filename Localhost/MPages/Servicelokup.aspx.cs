using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.IO;

public partial class MPages_Servicelokup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    string Flag = "";
    private enum GridViewServiceDetail : byte
    {
        col0=0,
        col1 = 1,
        col2 = 2,
        col3 = 3,
        col4 = 4,
        col5 = 5,
        col6 = 6,
        col7 = 7,
        col8 = 8,
        col9 = 9,
        col10 = 10,
        col11 = 11,
        col12 = 12,

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Flag = common.myStr(Request.QueryString["MD"]);
        if (!IsPostBack)
        {
            if (Flag != "ADMIN")
            {
                btnback.Visible = false;
                if (common.myInt(Session["StationId"]) == 0)
                {
                    ViewState["PT"] = "SERVICETAG";
                    Response.Redirect("/LIS/Phlebotomy/ChangeStation.aspx?PT=" + common.myStr(ViewState["PT"]) + "&Module=" + Flag, false);
                }
            }
            else
            {
                btnback.Visible = true;
            }
            popMainDept();

            Bind_ServiceGrid();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind_ServiceGrid();
    }

    void Bind_ServiceGrid()
    {
        DataSet ds = new DataSet();
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.ParseData bc = new BaseC.ParseData();
            int i=1;

            if (OptStandard.Checked)
                i = 1;
            else
                i = 0;

            Hashtable hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            hshInput.Add("@intDepartmentId", ddlMainDept.SelectedValue);
            hshInput.Add("@chvSubDepartmentIds", ddlsubDept.SelectedValue);
            hshInput.Add("@chvCPTCode", Convert.ToString(txtcptcode.Text.Trim()));
            hshInput.Add("@chvServiceName", "%" + Convert.ToString(txtservicename.Text.Trim()) + "%");
            hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hshInput.Add("@isStandard", i);
            if (common.myStr(Request.QueryString["MD"]) == "BOTH")
            {
                hshInput.Add("@chrType", " 'I','IS' ");
            }

            ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", hshInput);
            if (ds.Tables[0].Rows.Count == 0)
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            gvServiceDetail.DataSource = ds.Tables[0];
            gvServiceDetail.DataBind();
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
        finally
        {
            ds.Dispose();
        }
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        ddlMainDept.SelectedIndex = 0;
        ddlsubDept.SelectedIndex = 0;
        txtcptcode.Text = "";
        txtservicename.Text = "";
    }

    protected void ddlMainDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        popSubDept();
    }

    //protected void ddlsubDept_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //}

    protected void gverviceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Header))
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;

           
            if (common.myStr(Request.QueryString["MD"]) == "BOTH")
            {
                gvServiceDetail.Columns[10].Visible = false;
                gvServiceDetail.Columns[11].Visible = true;
            }
            else
            {
                gvServiceDetail.Columns[10].Visible = true;
                gvServiceDetail.Columns[11].Visible = false;
            }

            if (OptCustom.Checked == true)
            {
                gvServiceDetail.Columns[10].Visible = true;
            }
            if (OptStandard.Checked == true)
            {
                gvServiceDetail.Columns[10].Visible = false;
            }

            LinkButton LnkBtnServiceTag = (LinkButton)e.Row.FindControl("LnkBtnServiceTag");
            LinkButton LnkBtnTag = (LinkButton)e.Row.FindControl("LnkBtnTag");
            Label lblServiceTag = (Label)e.Row.FindControl("lblServiceTag");
            if (LnkBtnServiceTag != null)
            {
                if ((e.Row.Cells[8].Text.Trim().ToUpper() == "I") || (e.Row.Cells[8].Text.Trim().ToUpper() == "IS"))
                {
                    LnkBtnServiceTag.Visible = true;
                    LnkBtnTag.Visible = true;

                }
                else
                {
                    //lblServiceTag.Visible = true;
                }
            }
        }
    }

    //protected void btnclose_OnClick(object sender, EventArgs e)
    //{
    //    Bind_ServiceGrid();
    //}

    protected void gverviceDetail_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ServiceTag")
        {
            int ServiceID = common.myInt(e.CommandArgument);
            if (ServiceID == 0)
            {
                return;
            }
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            HiddenField hdnStationId = (HiddenField)row.FindControl("hdnStationId");

            RadWindowForNew.NavigateUrl = "~/LIS/Format/TemplateOtherServiceDetail.aspx?ServiceID=" + ServiceID + "&StationId=" + common.myInt(hdnStationId.Value);
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 1100;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            // RadWindowForNew.Title = "Time Slot";
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        if (e.CommandName == "Template")
        {
            int ServiceID = common.myInt(e.CommandArgument.ToString());
            if (ServiceID == 0)
                return;

            RadWindowForNew.NavigateUrl = "~/LIS/Format/TemplateServiceTag.aspx?ServiceID=" + ServiceID.ToString();
            RadWindowForNew.Height = 615;
            RadWindowForNew.Width = 1000;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            //RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;

        }
    }

    protected void gverviceDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Serviceid = "?ServiceId=" + gvServiceDetail.SelectedRow.Cells[0].Text;
        Session["deptId"] = gvServiceDetail.SelectedRow.Cells[5].Text;
        Session["subdept"] = gvServiceDetail.SelectedRow.Cells[4].Text;
        Response.Redirect("~/mpages/ItemOfService.aspx" + Serviceid, false);
    }

    private void popMainDept()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder strSQL = new StringBuilder();
            Hashtable hshIn = new Hashtable();
            string SubDeptType ="";
            strSQL.Remove(0, strSQL.Length);
            hshIn.Add("@Active", "1");
            hshIn.Add("@HospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            hshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

            //strSQL.Append(" select distinct DM.departmentname,DM.departmentid from departmentmain dm Left join FacilityWiseDepartment fwd on fwd.DepartmentId = dm.DepartmentID  inner join departmentsub ds on dm.departmentid=ds.departmentid");
            //strSQL.Append(" where fwd.FacilityId=case when dm.HospitalLocationID is null then @intFacilityId and dm.Active =@Active and dm.HospitalLocationID=@HospitalLocationID ");

            


            if (common.myStr(Request.QueryString["MD"]) == "BOTH")
            {
                //strSQL.Append(" AND ds.Type IN('I', 'IS') ");
                SubDeptType = "I,IS";
                
            }
            hshIn.Add("@subDeptType", SubDeptType);

           
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure,"PopulateDepartment", hshIn);
            if (dr.HasRows)
            {
                ddlMainDept.DataSource = dr;
                ddlMainDept.DataTextField = "departmentname";
                ddlMainDept.DataValueField = "departmentid";
                ddlMainDept.DataBind();
                dr.Close();

                ddlMainDept.Items.Insert(0, new RadComboBoxItem("Select All"));
                ddlMainDept.Items[0].Value = "0";
            }
            else
            {
                //ddlMainDept.Width = Unit.Pixel(295);
                //ddlsubDept.Width = Unit.Pixel(220);

            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void popSubDept()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder strSQL = new StringBuilder();
            Hashtable hshTable = new Hashtable();
            if (ddlMainDept.Items.Count > 0)
            {
                hshTable.Add("@departmentid", common.myInt(ddlMainDept.SelectedItem.Value));
                hshTable.Add("@Active", 1);
                hshTable.Add("@HospitalLocationID", common.myInt(Session["HospitalLocationID"]));
                strSQL.Remove(0, strSQL.Length);

                strSQL.Append(" select subdeptid,ds.subname from departmentsub ds");
                strSQL.Append(" where  departmentid=@departmentid and ds.Active =@Active ");

                if (common.myStr(Request.QueryString["MD"]) == "BOTH")
                {
                    strSQL.Append(" and ds.Type in ( 'I' ,'IS') ");
                }
                strSQL.Append("  order by ds.subname ASC ");

                SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshTable);
                if (dr.HasRows)
                {
                    ddlsubDept.DataSource = dr;
                    ddlsubDept.DataTextField = "subname";
                    ddlsubDept.DataValueField = "subdeptid";
                    ddlsubDept.DataBind();
                    dr.Close();
                    ddlsubDept.Items.Insert(0, new RadComboBoxItem(" Select All "));
                    ddlsubDept.Items[0].Value = "0";
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

    protected void gvServiceDetail_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        gvServiceDetail.PageIndex = e.NewPageIndex;
        Bind_ServiceGrid();
    }

    /* added on 13-11-2015 by kabir */
    protected void gvServiceDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
           
            e.Row.Cells[Convert.ToByte(GridViewServiceDetail.col12)].Visible = false;
           
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
            e.Row.Cells[Convert.ToByte(GridViewServiceDetail.col12)].Visible = false;
            
        }
    }


    protected void btnExportToExcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            BaseC.clsMISDashboard objval = new BaseC.clsMISDashboard(sConString);
            DataSet ds  =new DataSet();


            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.ParseData bc = new BaseC.ParseData();
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            hshInput.Add("@intDepartmentId", ddlMainDept.SelectedValue);
            hshInput.Add("@chvSubDepartmentIds", ddlsubDept.SelectedValue);
            hshInput.Add("@chvCPTCode", Convert.ToString(txtcptcode.Text.Trim()));
            hshInput.Add("@chvServiceName", "%" + Convert.ToString(txtservicename.Text.Trim()) + "%");
            hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hshInput.Add("@isStandard", common.myInt(OptStandard.Checked));
            if (common.myStr(Request.QueryString["MD"]) == "BOTH")
            {
                hshInput.Add("@chrType", " 'I','IS' ");
            }
            ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", hshInput);
            string[] selectedColumns = new[] { "CPTCode", "ServiceName", "DepartmentName", "SubName" };
            DataTable dt= new DataView(ds.Tables[0]).ToTable(false, selectedColumns);
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
                    response.AddHeader("Content-Disposition", "attachment;filename=ServiceList.xls");

                    // create a string writer
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {
                            // instantiate a datagrid
                            DataGrid dg = new DataGrid();
                            dg.DataSource = dt;
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
    }

    protected void RadioButtonAge_CheckedChanged(object sender, System.EventArgs e)
    {
        Bind_ServiceGrid();
        
    }
}
