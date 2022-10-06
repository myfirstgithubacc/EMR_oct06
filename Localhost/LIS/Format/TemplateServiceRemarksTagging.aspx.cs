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
using Telerik.Web.UI;
[Serializable]
public partial class LIS_Format_TemplateServiceRemarksTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshTable;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private Hashtable hshInput;
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    StringBuilder strSQL;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string EncodedRemarksName = common.myStr(Request.QueryString["RemarksName"]).Replace("||", "&");
            lblRemarksName.Text = EncodedRemarksName;
            popMainDept();
            Bind_ServiceGrid();
            BindSaveTaggedServiceRemarks();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind_ServiceGrid();
    }
    void Bind_ServiceGrid()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.clsLISMaster objclsLISMaster = new BaseC.clsLISMaster(sConString);
        DataSet ds = new DataSet();
        try
        {
            
            
            ds = objclsLISMaster.GetHospitalServices(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(ddlMainDept.SelectedValue)),
                Convert.ToInt16(common.myInt(ddlsubDept.SelectedValue)), common.myInt(Session["FacilityId"]), common.myStr(txtservicename.Text), common.myStr(txtcptcode.Text));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvServiceDetail.DataSource = ds.Tables[0];
                gvServiceDetail.DataBind();
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
            dl = null; objclsLISMaster = null;
            ds.Dispose();
        }
    }
    private void BindSaveTaggedServiceRemarks()
    {
        BaseC.InvestigationFormat objCls = new BaseC.InvestigationFormat(sConString);
        DataSet ds = objCls.getServiceTaggedRemarks(common.myStr(Request.QueryString["RemarkId"]));
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvSelectedService.DataSource = ds;
                    gvSelectedService.DataBind();
                    ViewState["Service"] = ds.Tables[0];
                }
                else
                {
                    DataTable dtNew = CreateTableDefaultRow();
                    gvSelectedService.DataSource = dtNew;
                    gvSelectedService.DataBind();

                }
            }
            else
            {
                DataTable dtNew = CreateTableDefaultRow();
                gvSelectedService.DataSource = dtNew;
                gvSelectedService.DataBind();

            }
        }
        catch(Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objCls = null;
            ds.Dispose();
        }

    }
    private DataTable CreateTable()
    {
        DataTable objDt = new DataTable();
        objDt.Columns.Add("ServiceId", typeof(int));
        objDt.Columns.Add("ServiceName");
        return objDt;
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        ddlMainDept.SelectedIndex = 0;
        ddlsubDept.SelectedIndex = 0;
        //txtcptcode.Text = "";
        //txtservicename.Text = "";
        txtcptcode.Text = string.Empty;
        txtservicename.Text = string.Empty;
    }

    protected void ddlMainDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        popSubDept();
    }

    protected void ddlsubDept_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void popMainDept()
    {

        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        string strDepartmentType = "";
        try
        {
            //DAL.DAL dl;
            //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //strSQL = new StringBuilder();
            //Hashtable hshIn = new Hashtable();
            //strSQL.Remove(0, strSQL.Length);
            //hshIn.Add("@Active", "1");
            //hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
            //strSQL.Append(" select distinct DM.departmentname,DM.departmentid from departmentmain dm inner join departmentsub ds on dm.departmentid=ds.departmentid");
            //strSQL.Append(" where dm.Active =@Active and dm.HospitalLocationID=@HospitalLocationID order by dm.departmentname ");
            //dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
            //if (dr.HasRows == true)
            //{
            //    ddlMainDept.DataSource = dr;
            //    ddlMainDept.DataTextField = "departmentname";
            //    ddlMainDept.DataValueField = "departmentid";
            //    ddlMainDept.DataBind();
            //    dr.Close();

            //    ddlMainDept.Items.Insert(0, new RadComboBoxItem("Select All"));
            //    ddlMainDept.Items[0].Value = "0";
            //}
            //else
            //{
            //    //ddlMainDept.Width = Unit.Pixel(295);
            //    //ddlsubDept.Width = Unit.Pixel(220);

            //}
            //dr.Close();
            ddlMainDept.Items.Clear();
            strDepartmentType = "'I','IS'";
            DataSet ds = bMstr.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"].ToString()), strDepartmentType);
            ddlMainDept.DataSource = ds.Tables[0];
            ddlMainDept.DataTextField = "DepartmentName";
            ddlMainDept.DataValueField = "DepartmentID";
            ddlMainDept.DataBind();
            ddlMainDept.Items.Insert(0, new RadComboBoxItem("", ""));
            ddlMainDept.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            bMstr = null;
            strDepartmentType = string.Empty;
        }
    }

    private void popSubDept()
    {

        BaseC.clsLabRequest objLabRequest = new BaseC.clsLabRequest(sConString);
        DataSet ds = new DataSet();
        try
        {
            //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //strSQL = new StringBuilder();
            //hshTable = new Hashtable();
            //if (ddlMainDept.Items.Count > 0)
            //{
            //    hshTable.Add("@departmentid", ddlMainDept.SelectedItem.Value);
            //    hshTable.Add("@Active", "1");
            //    hshTable.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"].ToString()));
            //    strSQL.Remove(0, strSQL.Length);
            //    strSQL.Append(" select subdeptid,ds.subname from departmentsub ds");
            //    strSQL.Append(" where departmentid=@departmentid and ds.Active =@Active and ds.HospitalLocationID=@HospitalLocationID order by ds.subname ASC");
            //    dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshTable);
            //    if (dr.HasRows == true)
            //    {
            //        ddlsubDept.DataSource = dr;
            //        ddlsubDept.DataTextField = "subname";
            //        ddlsubDept.DataValueField = "subdeptid";
            //        ddlsubDept.DataBind();
            //        dr.Close();
            //        ddlsubDept.Items.Insert(0, new RadComboBoxItem(" Select All "));
            //        ddlsubDept.Items[0].Value = "0";
            //    }

            //}







            ddlsubDept.Items.Clear();
            ddlsubDept.Text = "";
            ds = objLabRequest.GetDepartmentSubMaster(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlMainDept.SelectedItem.Value));
            ddlsubDept.DataSource = ds.Tables[0];
            ddlsubDept.DataTextField = "SubName";
            ddlsubDept.DataValueField = "SubDeptId";
            ddlsubDept.DataBind();
            ddlsubDept.Items.Insert(0, new RadComboBoxItem(" Select All ", "0"));



        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objLabRequest = null;
            ds.Dispose();
        }
    }
    protected void gvServiceDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvServiceDetail.PageIndex = e.NewPageIndex;
        Bind_ServiceGrid();
    }
    protected void LnkBtnServiceTag_click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)lnk.NamingContainer;
        LinkButton LnkBtnServiceTag = (LinkButton)gvr.FindControl("LnkBtnServiceTag");
        string ServiceName = gvr.Cells[0].Text.ToString();
        string ServiceId = LnkBtnServiceTag.CommandArgument;
        DataTable dt = new DataTable();
        try
        {
            if (common.myStr(ViewState["Service"]) != "")
            {
                dt = (DataTable)ViewState["Service"];
            }
            else
            {
                dt = CreateTable();
            }
            dt.DefaultView.RowFilter = "ServiceId=" + ServiceId;
            if (dt.DefaultView.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["ServiceId"] = ServiceId;
                dr["ServiceName"] = ServiceName;
                dt.Rows.Add(dr);
                dt.DefaultView.RowFilter = "";
                gvSelectedService.DataSource = dt;
                gvSelectedService.DataBind();
                ViewState["Service"] = dt;
            }
        }
        catch(Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            lnk = null;
            gvr = null;
            LnkBtnServiceTag = null;
            ServiceName = string.Empty; ;
            ServiceId = string.Empty;
            dt.Dispose();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        StringBuilder strXml = new StringBuilder();
        ArrayList coll = new ArrayList();
        BaseC.InvestigationFormat objCls = new BaseC.InvestigationFormat(sConString);
        try
        {
            foreach (GridViewRow gvr in gvSelectedService.Rows)
            {

                ImageButton imgDelete = (ImageButton)gvr.FindControl("imgDelete");
                if (common.myInt(imgDelete.CommandArgument) != 0)
                {
                    coll.Add(common.myInt(imgDelete.CommandArgument));
                    strXml.Append(common.setXmlTable(ref coll));
                }
            }
            if (strXml.Length > 0)
            {

                
                string Message = objCls.SaveTaggedRemarksService(common.myStr(Request.QueryString["RemarkId"]), strXml.ToString(), common.myInt(Session["UserId"]));
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = Message;
            }
            else
            {
                string Message = objCls.RemoveAllTaggedRemarksService(common.myInt(Request.QueryString["RemarkId"]), common.myInt(Session["UserId"]));
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = Message;
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
            strXml = null;
            coll = null;
        }
    }

    private DataTable CreateTableDefaultRow()
    {
        DataTable objDt = new DataTable();
        objDt.Columns.Add("ServiceId", typeof(int));
        objDt.Columns.Add("ServiceName");

        DataRow dr = objDt.NewRow();
        dr["ServiceId"] = "0";
        dr["ServiceName"] = "";
        objDt.Rows.Add(dr);
        return objDt;
    }
    protected void imgDelete_OnClick(object sender, EventArgs e)
    {
        ImageButton lnk = (ImageButton)sender;
        GridViewRow gvr = (GridViewRow)lnk.NamingContainer;


        string ServiceId = lnk.CommandArgument;
        DataTable dt = new DataTable();
        try
        {
            if (common.myStr(ViewState["Service"]) != "")
            {
                dt = (DataTable)ViewState["Service"];
            }
            else
            {
                dt = CreateTable();
            }
            if (ServiceId != "0")
            {
                dt.Select("ServiceId='" + ServiceId + "'").FirstOrDefault().Delete();
                dt.AcceptChanges();
                gvSelectedService.DataSource = dt;
                gvSelectedService.DataBind();
                ViewState["Service"] = dt;

                if (dt.Rows.Count == 0)
                {
                    DataTable dtNew = CreateTableDefaultRow();
                    gvSelectedService.DataSource = dtNew;
                    gvSelectedService.DataBind();

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
            lnk = null;
            gvr = null;


            ServiceId = string.Empty; ;
            dt.Dispose();
        }
    }

}
