using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class EMR_Orders_CommonInvestigations : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            //Cache.Remove("SelectedService");
            ViewState["SelectedService"] = null;
           
            BindDropDownServiceCategories();
            bindInvestigationSet();
            BindSelectedServicesGridView();
            BindServiceGrid(0);
        }
    }

    private void BindDropDownServiceCategories()
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = order.GetServiceCategories(Convert.ToInt32(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlInvSetName.DataSource = ds.Tables[0];
                ddlInvSetName.DataTextField = "DepartmentName";
                ddlInvSetName.DataValueField = "DepartmentID";
                ddlInvSetName.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }

    // no use method
    protected void gvBindSelectedServices()
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        gvSelectedServices.DataSource = GetInvestigationSetDetail(Convert.ToInt32(ddlInvestigationSet.SelectedValue));
        gvSelectedServices.DataBind();
    }

    protected void bindInvestigationSet()
    {
        try
        {
            BaseC.EMR objEMR = new BaseC.EMR(sConString);
            ddlInvestigationSet.DataSource = objEMR.GetFavouriteInvestigationSet(Convert.ToInt32(Session["HospitalLocationID"]), 0);
            ddlInvestigationSet.DataTextField = "SetName";
            ddlInvestigationSet.DataValueField = "SetId";
            ddlInvestigationSet.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlInvestigationSet_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSelectedServicesGridView();

    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        ddlInvestigationSet.Visible = false;
        gvSelectedServices.DataSource = null;
        gvSelectedServices.DataBind();
        BindBlankSelectedServiceGrid();
    }

    //main save

    private void BindSelectedServicesGridView()
    {
        try
        {
            pnlInvestigationOptions.Visible = true;
            if (ddlInvestigationSet.SelectedValue != "")
            {
                DataSet objDs = GetInvestigationSetDetail(Convert.ToInt32(ddlInvestigationSet.SelectedValue));
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    ViewState["SelectedService"] = objDs.Tables[0];
                    gvSelectedServices.DataSource = objDs.Tables[0];
                    gvSelectedServices.DataBind();
                //    Cache.Insert("SelectedService", objDs.Tables[0], null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);

                }
                else
                {
                    BindBlankSelectedServiceGrid();
                }
            }
            else
            {
                BindBlankSelectedServiceGrid();
            }
            ddlInvestigationSet.Visible = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (ddlInvestigationSet.Items.Count > 0)
        {
            BaseC.EMR objEMR = new BaseC.EMR(sConString);
            objEMR.DeleteInvestigationSet(Convert.ToInt32(ddlInvestigationSet.SelectedValue));
            bindInvestigationSet();
            ddlInvestigationSet_SelectedIndexChanged(this, null);
        }
    }

    protected void btnUndo_Click(object sender, EventArgs e)
    {
        ddlInvestigationSet.Visible = true;
    }

    #region working on Search Area

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindServiceGrid(1);
    }

    private void BindServiceGrid(int intSearch)
    {
        try
        {
            string ServiceName = "%%";
            if (txtSearch.Text.Trim() != "")
            {
                if (ddlSearchCriteria.SelectedValue == "1")//any where
                    ServiceName = "%" + txtSearch.Text.Trim() + "%";
                if (ddlSearchCriteria.SelectedValue == "2")//starts with
                    ServiceName = txtSearch.Text.Trim() + "%";
                if (ddlSearchCriteria.SelectedValue == "3")//ends with
                    ServiceName = "%" + txtSearch.Text.Trim();
            }
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            DataSet ds = order.GetHospitalServices(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(ddlInvSetName.SelectedValue), 
                0, "", ServiceName, common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvServices.DataSource = ds;
                gvServices.DataBind();
            }
            else
            {
                BindBlankServiceGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    #endregion

    #region working on Grid 'gvServices

    protected void gvServices_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvServices.PageIndex = e.NewPageIndex;
        BindServiceGrid(0);
    }

    protected void gvServices_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((Label)e.Row.Cells[2].FindControl("lblServiceName")).Text == string.Empty)
            {
                LinkButton lb = ((LinkButton)e.Row.Cells[3].Controls[0]);
                lb.Enabled = false;
            }

            string strId = e.Row.Cells[0].Text;
            if (strId == "0")
            {
                e.Row.Cells[3].Enabled = false;
            }
            //Label lbl = (Label)e.Row.Cells[1].FindControl("lbl");
            //lbl.ToolTip = lbl.Text.Trim();
            //e.Row.Cells[1].ToolTip = lbl.Text.Trim();
            //if (lbl.Text.Trim().Length > 55)
            //{
            //    lbl.Text = lbl.Text.Substring(0, 50) + " ...";
            //}
        }
    }

    protected void gvServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string strServiceId = gvServices.DataKeys[gvServices.SelectedIndex].Values[0].ToString();
            string strId = "0";
            Label lblServiceName = (Label)gvServices.SelectedRow.FindControl("lblServiceName");
            BindSelectedServicesGrid(Convert.ToInt32(strServiceId), lblServiceName.Text, Convert.ToInt32(strId));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvSelectedServices_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           // DataTable Dt = (DataTable)Cache["SelectedService"];
            DataTable Dt = (DataTable)ViewState["SelectedService"];
            gvSelectedServices.PageIndex = e.NewPageIndex;
            gvSelectedServices.DataSource = Dt;
            gvSelectedServices.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindBlankServiceGrid()
    {
        try
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("ServiceID");
            Dt.Columns.Add("SerialNo");
            Dt.Columns.Add("ServiceName");
            Dt.Columns.Add("SpecialPrecaution");
            Dt.Columns.Add("CPTCode");
            Dt.Columns["SerialNo"].AutoIncrement = true;
            Dt.Columns["SerialNo"].AutoIncrementSeed = 1;
            Dt.Columns["SerialNo"].AutoIncrementStep = 1;

            for (int i = 1; i <= 1; i++)
            {
                DataRow Dr = Dt.NewRow();

                Dr["ServiceID"] = "0";
                Dr["SerialNo"] = DBNull.Value;
                Dr["ServiceName"] = "No Service Found";
                Dr["SpecialPrecaution"] = "";

                Dt.Rows.Add(Dr);
            }

            gvServices.DataSource = Dt;
            gvServices.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    #endregion

    #region // working on Selected Service Grid
    void BindSelectedServicesGrid(Int32 iServiceId, string strServiceName, Int32 iID)
    {
        try
        {
            DataTable DT = (DataTable)ViewState["SelectedService"];
            if (DT == null)
            {
                DT = CreateTable();
            }

            DataRow[] datarow = DT.Select("ServiceId=" + iServiceId);
            if (datarow.Length > 0)
            {
                datarow[0].BeginEdit();
                datarow[0]["ID"] = iID;
                datarow[0]["ServiceId"] = iServiceId;
                datarow[0]["ServiceName"] = strServiceName;
                datarow[0].EndEdit();
            }
            else
            {
                DataRow dr;
                dr = DT.NewRow();
                dr["ID"] = iID;
                dr["ServiceId"] = iServiceId;
                dr["ServiceName"] = strServiceName;
                DT.Rows.Add(dr);
            }

           // Cache.Insert("SelectedService", DT, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            ViewState["SelectedService"] = DT;
            gvSelectedServices.DataSource = DT;
            gvSelectedServices.DataBind();
            //  tbProblem.ActiveTabIndex = 1;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindBlankSelectedServiceGrid()
    {
        try
        {
           // Cache.Remove("SelectedService");
            ViewState["SelectedService"] = null;
            DataTable dt = CreateTable();
            for (int i = 1; i <= 1; i++)
            {
                DataRow Dr = dt.NewRow();
                Dr["ID"] = "0";
                Dr["ServiceId"] = "0";
                Dr["ServiceName"] = "";

                dt.Rows.Add(Dr);
            }

            gvSelectedServices.DataSource = dt;
            gvSelectedServices.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("ID");
        Dt.Columns.Add("ServiceId");
        Dt.Columns.Add("ServiceName");
        return Dt;
    }

    protected void gvSelectedServices_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvSelectedServices_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Del")
            {
                BaseC.EMROrders order = new BaseC.EMROrders(sConString);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                if (row.Cells[0].Text != "0")
                {
                    lblMessage.Text = order.DeleteOrderSetService(Convert.ToInt32(row.Cells[1].Text));
                }
                if (row.Cells[1].Text != "0")
                {
                    DataTable dt = new DataTable();
                  //  dt = (DataTable)Cache["SelectedService"];
                    dt = (DataTable)ViewState["SelectedService"];
                    dt.Rows.RemoveAt(row.RowIndex);
                    dt.AcceptChanges();
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["SelectedService"] = dt;
                        gvSelectedServices.DataSource = dt;
                        gvSelectedServices.DataBind();
                       // Cache.Insert("SelectedService", dt, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        BindBlankSelectedServiceGrid();
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

    protected void gvSelectedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }
    }

    #endregion

    protected void ddlInvSetName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindServiceGrid(1);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnFormSave_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.EMROrders order =new BaseC.EMROrders(sConString);
            StringBuilder objXML = new StringBuilder();
            foreach (GridViewRow gvr in gvSelectedServices.Rows)
            {

                string strServiceId = gvr.Cells[1].Text;
                if (strServiceId == "0")
                {
                    Alert.ShowAjaxMsg("please select service list.", this.Page);
                    return;
                }
                if (strServiceId != "0")
                {
                    objXML.Append("<Table1><c1>");
                    objXML.Append(strServiceId);
                    objXML.Append("</c1></Table1>");
                }
            }
            if (ddlInvestigationSet.SelectedValue != "" && objXML.ToString()!="")
            {
                Hashtable hshOutPut = new Hashtable();
                hshOutPut=order.SaveOrderSetDetails(Convert.ToInt32(ddlInvestigationSet.SelectedValue), objXML.ToString());
                lblMessage.Text =hshOutPut["@chvErrorStatus"].ToString();
                BindSelectedServicesGridView();
            }
            else
            {
                Alert.ShowAjaxMsg("please select set name.", this.Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public DataSet GetInvestigationSetDetail(int iSetID)
    {
        DataSet objDs = new DataSet();
        try
        {
            BaseC.EMROrders order = new BaseC.EMROrders(sConString);
            objDs = order.GetInvestigationSetDetail(iSetID);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        } return objDs;
    }
    protected void ibtnAddOrderSet_Click(object sender, ImageClickEventArgs e)
    {
        RadWindow1.NavigateUrl = "~/MPages/AddOrderSet.aspx";
        RadWindow1.Height = 600;
        RadWindow1.Width = 800;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;

        RadWindow1.OnClientClose = "addDiagnosisSerchOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void btnAddOrderSetClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindInvestigationSet();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
}
