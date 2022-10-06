using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

public partial class EMR_Immunization_ImmunizationMain : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindImmunizationMain();
    }

    private void BindImmunizationMain()
    {
        try
        {
            Hashtable hshInputs = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInputs.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInputs.Add("@ImmunizationId", Request.QueryString["ImmId"]);
            hshInputs.Add("@Type", 0);// data comming for EMRImmunizationStockReceivingMain table
            hshInputs.Add("@Status", ddlstatus.SelectedValue);

            DataSet objDs1 = dl.FillDataSet(CommandType.StoredProcedure, "USPEMRGETIMMUNIZATIONSTOCK", hshInputs);

            gvImmunizationStock.DataSource = objDs1;
            gvImmunizationStock.DataBind();

            if (ddlstatus.SelectedValue == "P")
            {
                gvImmunizationStock.Columns[10].Visible = false;
                gvImmunizationStock.Columns[9].Visible = false;
                gvImmunizationStock.Columns[8].Visible = false;
            }
            else if (ddlstatus.SelectedValue == "O")
            {
                gvImmunizationStock.Columns[10].Visible = true;
                gvImmunizationStock.Columns[9].Visible = true;
                gvImmunizationStock.Columns[8].Visible = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ImageButton lbtn = sender as ImageButton;
            GridViewRow dr = lbtn.NamingContainer as GridViewRow;

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@DocumentNo", ((Label)dr.FindControl("lblDocumentNo")).Text);
            HshIn.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            HshIn.Add("@EncodedBy", Session["UserId"]);
            Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE EMRImmunizationStockReceivingDetails set Active=0 where MainId in (select Id from EMRImmunizationStockReceivingMain where Active=1 and DocumentNo=@DocumentNo and HospitalLocationId=@HospitalLocationId)  update EMRImmunizationStockReceivingMain set Active=0 where DocumentNo=@DocumentNo and HospitalLocationId=@HospitalLocationId", HshIn);
            if (i == 0)
            {
                lblMessage.Text = "Record Deleted...";
                //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);

                BindImmunizationMain();
            }
            else
            {
                lblMessage.Text = "Error In Deleted...";
                //Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvImmunizationStock_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvImmunizationStock.EditIndex = -1;
        BindImmunizationMain();
    }

    protected void gvImmunizationStock_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvImmunizationStock.PageIndex = e.NewPageIndex;
        BindImmunizationMain();
    }

    protected void gvImmunizationStock_SelectedIndexChanged(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)gvImmunizationStock.Rows[gvImmunizationStock.SelectedIndex].FindControl("btnview");
        Session["mainid"] = ((Label)gvImmunizationStock.Rows[gvImmunizationStock.SelectedIndex].FindControl("lblId")).Text;
        //// this.Dispose();
        lb.Attributes.Add("onclick", "javascript:window.close();");
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "window.close();", true);   
    }

    protected void btnStockPost_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            LinkButton lbtn = sender as LinkButton;
            GridViewRow dr = lbtn.NamingContainer as GridViewRow;

            Hashtable hshInput = new Hashtable();
            Hashtable hsOutput = new Hashtable();
            try
            {
                string status = ((Label)dr.FindControl("lblStatus")).Text == "Open" ? "O" : "P";
                //hshInput.Add("@StockId", ViewState["StockId"] == null ? "0" : ViewState["StockId"]);
                // hshInput.Add("@Id",);
                hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
                hshInput.Add("@DocumentNo", ((Label)dr.FindControl("lblDocumentNo")).Text);
                hshInput.Add("@Status", status);     // O=open ,P= post           
                hshInput.Add("@EncodedBy", Session["UserId"]);
                hsOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hsOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRSaveUpdateIMMUNIZATIONSTOCK", hshInput, hsOutput);
                lblMessage.Text = "Record is saved.";
                BindImmunizationMain();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;// hsOutput["@chvErrorStatus"].ToString();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvImmunizationStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))//&& (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        {
            ////e.Row.Cells[6].Attributes.Add("onclick", "javascript:window.close();");
            ////Session["mainid"] = e.Row.Cells[0].Text;// ((Label)gvImmunizationStock.Rows[gvImmunizationStock.SelectedIndex].FindControl("lblId")).Text;
            //////gvImmunizationStock.Attributes.Add("onclick", "javascript:window.close();");
            //if (ddlstatus.SelectedValue== "P")
            //{
            //    e.Row.Cells[8].Visible = false;
            //}
            //else if (ddlstatus.SelectedValue == "O")
            //{
            //    e.Row.Cells[8].Visible = true;
            //}
        }

        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
        {
            Label lblName = (Label)e.Row.Cells[8].FindControl("lblDocumentNo");
            ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
            img.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to Delete this Document No :  " + lblName.Text + "')");
            //if (e.Row.Cells[8].Text.ToString().Trim() == "0")
            //{
            //    img.Visible = false;
            //}
        }
    }

    //protected void btnview_Click(object sender, EventArgs e)
    //{
    //    //LinkButton btnEdit = sender as LinkButton;
    //    //GridViewRow row = btnEdit.NamingContainer as GridViewRow;
    //    //Session["mainid"] = ((Label)row.FindControl("lblId")).Text;        
    //    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "window.close();", true);     


    //}

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindImmunizationMain();
        lblMessage.Text = "";
    }

}
