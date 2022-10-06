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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;

public partial class EMR_Immunization_ImmunizationMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();

    private enum GridImmunization : byte
    {
        ImmunizationId = 0,
        SerialNo = 1,
        ImmunizationName = 2,
        CPTCode = 3,
        //CPTDescription = 4,
        CVXCode = 4,
        Status = 5,
        Active = 6,
        DeActive = 7,
        Edit = 8
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            BindImmunizationMasterGrid();
        }
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    private void BindImmunizationMasterGrid()
    {
        try
        {
            BaseC.EMRImmunization objEMRImmunization = new BaseC.EMRImmunization(sConString);
            DataSet ds = objEMRImmunization.GetImmunizationMaster(Convert.ToInt16(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvImmunization.DataSource = ds;
                gvImmunization.DataBind();
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lnkImmunizationMaster_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("ImmunizationBaby.aspx", false);
    }
    private void BindBlankGrid()
    {
        try
        {
            DataTable dtImmunization = new DataTable();
            dtImmunization.Columns.Add("ImmunizationId");
            dtImmunization.Columns.Add("SerialNo");
            dtImmunization.Columns.Add("ImmunizationName");
            dtImmunization.Columns.Add("CPTCode");
            //dtImmunization.Columns.Add("CPTDescription");
            dtImmunization.Columns.Add("CVXCode");
            dtImmunization.Columns.Add("Status");
            dtImmunization.Columns.Add("Active");

            for (int i = 0; i < 5; i++)
            {
                DataRow Dr = dtImmunization.NewRow();
                Dr["ImmunizationId"] = "";
                Dr["SerialNo"] = "";
                Dr["ImmunizationName"] = "";
                Dr["CPTCode"] = "";
                //Dr["CPTDescription"] = "";
                Dr["CVXCode"] = "";
                Dr["Status"] = "";
                Dr["Active"] = "";
                dtImmunization.Rows.Add(Dr);
            }
            ViewState["BlankGrid"] = "True";
            gvImmunization.DataSource = dtImmunization;
            gvImmunization.DataBind();
            ViewState["BlankGrid"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void SaveImmunizationMaster_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text.ToString().Trim() == "")
            {
                Alert.ShowAjaxMsg("Name Cannot Be Blank...", Page);
                txtName.Focus();
                return;
            }
            BaseC.EMRImmunization EMRImmunization = new BaseC.EMRImmunization(sConString);
            String str = EMRImmunization.SaveImmunizationMaster(Convert.ToInt16(Session["HospitalLocationID"]), txtName.Text.ToString().Trim(), Convert.ToString(txtCPTCode.Text), txtCVXCode.Text.ToString().Trim(), Convert.ToInt32(Session["UserID"]));
            if (str == "Data Saved")
            {
                lblMessage.Text = "Record(s) Has Been Saved";
            }
            else
            {
                lblMessage.Text = "Error In Saving...";
            }
            BindImmunizationMasterGrid();
            txtName.Text = "";
            txtCVXCode.Text = "";
            txtCPTCode.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvImmunization_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[Convert.ToByte(GridImmunization.ImmunizationId)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridImmunization.Active)].Visible = false;
            //e.Row.Cells[Convert.ToByte(GridImmunization.CPTDescription)].Visible = false;

            if (ViewState["BlankGrid"] != null)
            {

                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
        }

        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        {
            Label lblName = (Label)e.Row.Cells[Convert.ToByte(GridImmunization.ImmunizationName)].FindControl("lblNameImmunizationGrid");
            ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
            img.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to De-Activate this record :  " + lblName.Text + "')");
            if (e.Row.Cells[Convert.ToByte(GridImmunization.Active)].Text.ToString().Trim() == "0")
            {
                img.Visible = false;
            }
        }

        if (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))
        {
            DropDownList ddlGridStatus = (DropDownList)e.Row.FindControl("ddlGridStatus");
            Label lblGridAct = (Label)e.Row.FindControl("lblGridActive2");
            String strActive = e.Row.Cells[Convert.ToByte(GridImmunization.Active)].Text.ToString().Trim();

            if (strActive == "0")
            {
                ddlGridStatus.Visible = true;
                lblGridAct.Visible = false;
            }
            else
            {
                ddlGridStatus.Visible = false;
                lblGridAct.Visible = true;

            }
        }
    }

    protected void gvImmunization_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvImmunization.PageIndex = e.NewPageIndex;
        BindImmunizationMasterGrid();
    }

    protected void gvImmunization_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeActivate")
            {
                BaseC.EMRImmunization objEMRImmunization = new BaseC.EMRImmunization(sConString);
                Int32 i = objEMRImmunization.DeActivateImmunizationMaster(Convert.ToInt32(e.CommandArgument), Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserID"]));
                if (i == 0)
                {
                    lblMessage.Text = "Record De-Actived...";
                    //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
                    gvImmunization.EditIndex = -1;
                    BindImmunizationMasterGrid();
                }
                else
                {
                    lblMessage.Text = "Error In De-Activation...";
                    //Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
                    gvImmunization.EditIndex = -1;
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

    protected void gvImmunization_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvImmunization.EditIndex = e.NewEditIndex;
        BindImmunizationMasterGrid();
    }

    protected void gvImmunization_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvImmunization.EditIndex = -1;
        BindImmunizationMasterGrid();
    }

    protected void gvImmunization_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string ID = gvImmunization.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.ImmunizationId)].Text;
            TextBox Name = (TextBox)gvImmunization.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.ImmunizationName)].FindControl("txtNameGrid");
            TextBox CVXCode = (TextBox)gvImmunization.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.CVXCode)].FindControl("txtCVXCOdeGrid");
            string strActive = gvImmunization.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.Active)].Text;

            //Label st= (Label)gvImmunization.Rows[e.RowIndex].FindControl("lblGridActive2");
            //strActive=st.Text;
            TextBox txtCPTCodeGrid = ((TextBox)gvImmunization.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.CPTCode)].FindControl("txtCPTCodeGrid"));
            String strStatus = "";
            if (strActive == "0")
            {
                DropDownList strDrop = ((DropDownList)gvImmunization.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.Status)].FindControl("ddlGridStatus"));
                strStatus = strDrop.SelectedItem.Value;
            }
            else if (strActive == "1")
            {
                strStatus = "1";
            }

            BaseC.EMRImmunization objEMRImmunization = new BaseC.EMRImmunization(sConString);
            Int32 i = objEMRImmunization.UpdateImmunizationMaster(Convert.ToInt32(ID), Convert.ToInt16(Session["HospitalLocationID"]), bc.ParseQ(Name.Text.ToString().Trim()), Convert.ToString(txtCPTCodeGrid.Text), bc.ParseQ(CVXCode.Text.ToString().Trim()), Convert.ToInt16(Session["UserID"]), Convert.ToByte(strStatus));
            if (i == 0)
            {
                lblMessage.Text = "Record(s) Updated... ";
                //Alert.ShowAjaxMsg("Record(s) Updated... ", this.Page);
                gvImmunization.EditIndex = -1;
                BindImmunizationMasterGrid();
            }
            else
            {
                lblMessage.Text = "Error In Updating... ";
                //Alert.ShowAjaxMsg("Error In Updating... ", this.Page);
                gvImmunization.EditIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
