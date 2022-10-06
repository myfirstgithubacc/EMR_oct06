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

public partial class EMR_Immunization_ImmunizationBaby : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();

    private enum GridImmunization : byte
    {
        ID = 0,
        ImmunizationId = 1,
        SerialNo = 2,
        ImmunizationName = 3,
        AgeFrom = 4,
        AgeTo = 5,
        Status = 6,
        Active = 7,
        DeActive = 8,
        Edit = 9
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Mpg"] != null)
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();

                populateImmunizationDropDown();
                BindImmunizationBabyGrid();
                ddlImmunizationName.Focus();
                FillDotor(); // Akshay Tirathram 21-Sep-2022
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void populateImmunizationDropDown()
    {
        BaseC.EMRImmunization EMRImmunization = new BaseC.EMRImmunization(sConString);
        try
        {

            SqlDataReader dr = EMRImmunization.getImmunizationName(Convert.ToInt16(Session["HospitalLocationID"]));
            if (dr.HasRows == true)
            {
                ddlImmunizationName.DataSource = dr;
                ddlImmunizationName.DataTextField = "ImmunizationName";
                ddlImmunizationName.DataValueField = "ImmunizationID";
                ddlImmunizationName.DataBind();

                //ddlImmunizationName.Items.Insert(0, " [ Select ] ");
            }
            dr.Close();
            dr.Dispose();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            EMRImmunization = null;
        }
    }

    private void BindImmunizationBabyGrid()
    {
        BaseC.EMRImmunization objEMRImmunization = new BaseC.EMRImmunization(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objEMRImmunization.GetImmunisationScheduleBaby(Convert.ToInt16(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvImmunizationBaby.DataSource = ds;
                gvImmunizationBaby.DataBind();
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
        finally
        {
            objEMRImmunization = null;
            ds.Dispose();

        }
    }

    private void BindBlankGrid()
    {
        DataTable dtImmunization = new DataTable();
        try
        {

            dtImmunization.Columns.Add("Id");
            dtImmunization.Columns.Add("ImmunizationId");
            dtImmunization.Columns.Add("SerialNo");
            dtImmunization.Columns.Add("ImmunizationName");
            dtImmunization.Columns.Add("CPTCode");
            dtImmunization.Columns.Add("CPTDescription");
            dtImmunization.Columns.Add("CVXCode");
            dtImmunization.Columns.Add("Status");
            dtImmunization.Columns.Add("Active");
            dtImmunization.Columns.Add("FromAge");
            dtImmunization.Columns.Add("ToAge");
            dtImmunization.Columns.Add("AgeFrom1");
            dtImmunization.Columns.Add("AgeFrom2");
            dtImmunization.Columns.Add("AgeFromType1");
            dtImmunization.Columns.Add("AgeFromType2");
            dtImmunization.Columns.Add("AgeTo1");
            dtImmunization.Columns.Add("AgeTo2");
            dtImmunization.Columns.Add("AgeToType1");
            dtImmunization.Columns.Add("AgeToType2");
            dtImmunization.Columns.Add("ScheduleID");


            for (int i = 0; i < 5; i++)
            {
                DataRow Dr = dtImmunization.NewRow();
                Dr["Id"] = "";
                Dr["ImmunizationId"] = "";
                Dr["SerialNo"] = "";
                Dr["ImmunizationName"] = "";
                Dr["CPTCode"] = "";
                Dr["CPTDescription"] = "";
                Dr["CVXCode"] = "";
                Dr["Status"] = "";
                Dr["Active"] = "";
                Dr["FromAge"] = "";
                Dr["ToAge"] = "";
                Dr["AgeFrom1"] = "";
                Dr["AgeFrom2"] = "";
                Dr["AgeFromType1"] = "";
                Dr["AgeFromType2"] = "";
                Dr["AgeTo1"] = "";
                Dr["AgeTo2"] = "";
                Dr["AgeToType1"] = "";
                Dr["AgeToType2"] = "";
                Dr["ScheduleID"] = "";
                dtImmunization.Rows.Add(Dr);
            }

            ViewState["BlankGrid"] = "True";
            gvImmunizationBaby.DataSource = dtImmunization;
            gvImmunizationBaby.DataBind();
            ViewState["BlankGrid"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dtImmunization.Dispose();
        }
    }

    protected void UpdateImmunizationBabySchedule_OnClick(Object sender, EventArgs e)
    {
        BaseC.EMRImmunization EMRImmunization = new BaseC.EMRImmunization(sConString);
        try
        {

            int i = EMRImmunization.UpdateImmunizationScheduleBaby(common.myInt(ddlImmunizationName.SelectedItem.Value), Convert.ToInt16(common.myInt(txtAgeFrom1.Text)),
                                                                    Convert.ToInt16(common.myInt(txtAgeFrom2.Text)), common.myStr(ddlFrom1.SelectedValue), common.myStr(ddlFrom2.SelectedValue),
                                                                    Convert.ToInt16(common.myInt(txtAgeTo1.Text)), Convert.ToInt16(common.myInt(txtAgeTo2.Text)), common.myStr(ddlAgeTo1.SelectedValue),
                                                                    common.myStr(ddlAgeTo2.SelectedValue), common.myInt(ViewState["ID"]), common.myInt(Session["UserID"]));


            if (i == 0)
            {
                lblMessage.Text = "Record(s) Updated... ";
                //Alert.ShowAjaxMsg("Record(s) Updated... ", this.Page);
                gvImmunizationBaby.EditIndex = -1;
                BindImmunizationBabyGrid();
            }
            else
            {
                lblMessage.Text = "Error In Updating... ";
                //Alert.ShowAjaxMsg("Error In Updating... ", this.Page);
                gvImmunizationBaby.EditIndex = -1;
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
            EMRImmunization = null;
        }
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {

        Response.Redirect("ImmunizationBaby.aspx", false);
    }
    protected void lnkImmunizationMaster_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("ImmunizationMaster.aspx", false);
    }
    protected void SaveImmunizationBabySchedule_OnClick(Object sender, EventArgs e)
    {
        BaseC.EMRImmunization EMRImmunization = new BaseC.EMRImmunization(sConString);
        String txtAgeFrm2 = "";
        String txtAgeT2 = "";
        String txtAgeT1 = "";
        String str = "";
        try
        {


            if (txtAgeTo1.Text == "")
            {
                txtAgeT1 = "0";

            }
            else
            {
                txtAgeT1 = txtAgeTo1.Text.ToString().Trim();
            }



            if (txtAgeFrom2.Text == "")
            {
                txtAgeFrm2 = "0";
            }
            else
            {
                txtAgeFrm2 = txtAgeFrom2.Text.ToString().Trim();
            }

            if (ddlFrom2.SelectedIndex == 0)
            {
                txtAgeFrm2 = "0";
            }

            if (txtAgeTo2.Text == "")
            {
                txtAgeT2 = "0";
            }
            else
            {
                txtAgeT2 = txtAgeTo2.Text.ToString().Trim();
            }


            if (ddlImmunizationName.SelectedIndex < 0)
            {
                lblMessage.Text = "Pleas select Name";
                return;
            }

            if (txtAgeFrom1.Text == "")
            {

                lblMessage.Text = "Pleas select From Age";
                return;
            }

            if (ddlFrom1.SelectedValue == "0")
            {
                lblMessage.Text = "Pleas select From Age";
                return;
            }


            //Added By Akshay Tirathram 21-Sep-2022
            if (ddlDoctor.SelectedIndex < 0)
            {
                lblMessage.Text = "Pleas select Doctor";
                return;
            }

            //if (ddlFrom2.SelectedValue == "0")
            //{
            //    lblMessage.Text = "Pleas select Age";
            //    return;
            //}

            //if (ddlAgeTo1.SelectedValue == "0")
            //{
            //    lblMessage.Text = "Pleas select Age";
            //    return;
            //}

            //if (ddlAgeTo2.SelectedValue == "0")
            //{
            //    lblMessage.Text = "Pleas select Age";
            //    return;
            //}




            if (ddlAgeTo2.SelectedIndex == 0)
            {
                txtAgeT2 = "0";
            }
            if (txtAgeTo2.Text == "")
            {
                ddlAgeTo2.SelectedIndex = 0;
            }


            str = EMRImmunization.SaveImmunizationScheduleBaby(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(ddlImmunizationName.SelectedValue),
                 Convert.ToInt16(bc.ParseQ(txtAgeFrom1.Text.ToString().Trim())), Convert.ToInt16(bc.ParseQ(txtAgeFrm2)),
                 ddlFrom1.SelectedValue, ddlFrom2.SelectedValue, Convert.ToInt16(txtAgeT1), Convert.ToInt16(bc.ParseQ(txtAgeT2)),
                 ddlAgeTo1.SelectedValue, ddlAgeTo2.SelectedValue, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(ddlDoctor.SelectedValue));  // Added Convert.ToInt32(ddlDoctor.SelectedValue)  By Akshay Tirathram 24-Aug-2022
            if (str == "Data Saved")
            {
                lblMessage.Text = "Record(s) Has Been Saved...";
            }
            else
            {
                lblMessage.Text = "Error In Saving...";
            }
            //Alert.ShowAjaxMsg(str, this.Page);
            BindImmunizationBabyGrid();
            //txtName.Text = "";
            //ddlCPTCOde.SelectedIndex = 0;
            //txtCVXCode.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {

            EMRImmunization = null;
            txtAgeFrm2 = null;
            txtAgeT2 = null;
            txtAgeT1 = null;
            str = null;
        }
    }

    protected void gvImmunizationBaby_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {

        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[Convert.ToByte(GridImmunization.ID)].Visible = false;
                e.Row.Cells[Convert.ToByte(GridImmunization.ImmunizationId)].Visible = false;
                e.Row.Cells[Convert.ToByte(GridImmunization.Active)].Visible = false;

                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false;

                if (ViewState["BlankGrid"] != null)
                {
                    // e.Row.Cells[2].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                }
            }

            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
            {
                Label lblName = (Label)e.Row.Cells[Convert.ToByte(GridImmunization.ImmunizationName)].FindControl("lblNameImmunizationGrid");
                ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
                img.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to De-Activate this record :  " + e.Row.Cells[Convert.ToByte(GridImmunization.ImmunizationName)].Text + "');");
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

                DropDownList ddlName = (DropDownList)e.Row.Cells[Convert.ToByte(GridImmunization.ImmunizationName)].FindControl("ddlImmunizationNameGrid");
                TextBox txtAgeFrom1Grid = (TextBox)e.Row.Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("txtAgeFrom1Grid");
                DropDownList ddlFrom1 = (DropDownList)e.Row.Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("ddlFrom1");
                TextBox txtAgeFrom2Grid = (TextBox)e.Row.Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("txtAgeFrom2Grid");
                DropDownList ddlFrom2 = (DropDownList)e.Row.Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("ddlFrom2");

                TextBox txtAgeTo1Grid = (TextBox)e.Row.Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("txtAgeTo1Grid");
                DropDownList ddlTo1 = (DropDownList)e.Row.Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("ddlTo1");
                TextBox txtAgeTo2Grid = (TextBox)e.Row.Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("txtAgeTo2Grid");
                DropDownList ddlTo2 = (DropDownList)e.Row.Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("ddlTo2");


                LinkButton lnkUpd = (LinkButton)e.Row.Cells[Convert.ToByte(GridImmunization.Edit)].FindControl("lnkUpdate");
                lnkUpd.Attributes.Add("onclick", "javascript:return Validation('" + ddlFrom1.ClientID + "','" + ddlFrom2.ClientID + "','" + ddlTo1.ClientID + "','" + ddlTo2.ClientID + "','" + txtAgeFrom1Grid.ClientID + "','" + txtAgeFrom2Grid.ClientID + "','" + txtAgeTo1Grid.ClientID + "','" + txtAgeTo2Grid.ClientID + "','" + ddlName.ClientID + "')");
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void gvImmunizationBaby_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvImmunizationBaby.PageIndex = e.NewPageIndex;
            BindImmunizationBabyGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvImmunizationBaby_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        BaseC.EMRImmunization objEMRImmunization = new BaseC.EMRImmunization(sConString);
        try
        {
            if (e.CommandName == "DeActivate")
            {

                Int32 i = objEMRImmunization.DeActivateImmunizationBaby(Convert.ToInt32(e.CommandArgument), Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserID"]));
                if (i == 0)
                {
                    lblMessage.Text = "Record De-Actived... ";
                    //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
                    gvImmunizationBaby.EditIndex = -1;
                    BindImmunizationBabyGrid();
                }
                else
                {
                    lblMessage.Text = "Error In De-Activation... ";
                    //    Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
                    gvImmunizationBaby.EditIndex = -1;
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
            objEMRImmunization = null;
        }

    }

    protected void gvImmunizationBaby_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvImmunizationBaby.EditIndex = e.NewEditIndex;
            BindImmunizationBabyGrid();
            LinkButton lnk = (LinkButton)gvImmunizationBaby.Rows[e.NewEditIndex].FindControl("lnkUpdate");
            lnk.ForeColor = System.Drawing.Color.White;
            lnk = (LinkButton)gvImmunizationBaby.Rows[e.NewEditIndex].FindControl("lnkCancel");
            lnk.ForeColor = System.Drawing.Color.White;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void gvImmunizationBaby_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvImmunizationBaby.EditIndex = -1;
            BindImmunizationBabyGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvImmunizationBaby_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        BaseC.EMRImmunization EMRImmunization = new BaseC.EMRImmunization(sConString);
        String strStatus = string.Empty;
        String txtAgeFrom2 = string.Empty;
        String txtAgeTo2 = string.Empty;
        try
        {
            String ID = gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.ID)].Text;
            DropDownList ddlName = (DropDownList)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.ImmunizationName)].FindControl("ddlImmunizationNameGrid");

            TextBox txtAgeFrom1Grid = (TextBox)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("txtAgeFrom1Grid");
            DropDownList ddlFrom1 = (DropDownList)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("ddlFrom1");
            TextBox txtAgeFrom2Grid = (TextBox)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("txtAgeFrom2Grid");
            DropDownList ddlFrom2 = (DropDownList)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("ddlFrom2");

            TextBox txtAgeTo1Grid = (TextBox)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("txtAgeTo1Grid");
            DropDownList ddlTo1 = (DropDownList)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("ddlTo1");
            TextBox txtAgeTo2Grid = (TextBox)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("txtAgeTo2Grid");
            DropDownList ddlTo2 = (DropDownList)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.AgeFrom)].FindControl("ddlTo2");

            string strActive = gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.Active)].Text;


            if (txtAgeFrom2Grid.Text == "")
            {
                txtAgeFrom2 = "0";
            }
            else
            {
                txtAgeFrom2 = txtAgeFrom2Grid.Text.ToString().Trim();
            }

            if (ddlFrom2.SelectedIndex == 0)
            {
                txtAgeFrom2 = "0";
            }

            if (txtAgeTo2Grid.Text == "")
            {
                txtAgeTo2 = "0";
            }
            else
            {
                txtAgeTo2 = txtAgeTo2Grid.Text.ToString().Trim();
            }
            if (ddlTo2.SelectedIndex == 0)
            {
                txtAgeTo2 = "";
            }


            if (strActive == "0")
            {
                DropDownList strDrop = ((DropDownList)gvImmunizationBaby.Rows[e.RowIndex].Cells[Convert.ToByte(GridImmunization.Status)].FindControl("ddlGridStatus"));
                strStatus = strDrop.SelectedItem.Value;
            }
            else if (strActive == "1")
            {
                strStatus = "1";
            }


            Int32 i = 0;// = EMRImmunization.UpdateImmunizationScheduleBaby(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(ddlName.SelectedItem.Value), Convert.ToInt16(bc.ParseQ(txtAgeFrom1Grid.Text.ToString().Trim())), Convert.ToInt16(bc.ParseQ(txtAgeFrom2)), ddlFrom1.SelectedItem.Value, ddlFrom2.SelectedItem.Value, Convert.ToInt16(bc.ParseQ(txtAgeTo1Grid.Text.ToString().Trim())), Convert.ToInt16(bc.ParseQ(txtAgeTo2)), ddlTo1.SelectedItem.Value, ddlTo2.SelectedItem.Value, Convert.ToInt32(ID), Convert.ToByte(strStatus));

            if (i == 0)
            {
                lblMessage.Text = "Record(s) Updated... ";
                //Alert.ShowAjaxMsg("Record(s) Updated... ", this.Page);
                gvImmunizationBaby.EditIndex = -1;
                BindImmunizationBabyGrid();
            }
            else
            {
                lblMessage.Text = "Error In Updating... ";
                //Alert.ShowAjaxMsg("Error In Updating... ", this.Page);
                gvImmunizationBaby.EditIndex = -1;
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
            EMRImmunization = null;
            strStatus = null;
            txtAgeFrom2 = null;
            txtAgeTo2 = null;
        }
    }

    protected void gvImmunizationBaby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlImmunizationName.SelectedValue = gvImmunizationBaby.SelectedRow.Cells[1].Text.Trim();
            txtAgeFrom1.Text = gvImmunizationBaby.SelectedRow.Cells[10].Text.Trim();
            txtAgeFrom2.Text = gvImmunizationBaby.SelectedRow.Cells[11].Text;
            ddlFrom1.SelectedValue = gvImmunizationBaby.SelectedRow.Cells[12].Text;
            ddlFrom2.SelectedValue = gvImmunizationBaby.SelectedRow.Cells[13].Text;
            txtAgeTo1.Text = gvImmunizationBaby.SelectedRow.Cells[14].Text;
            txtAgeTo2.Text = gvImmunizationBaby.SelectedRow.Cells[15].Text;
            ddlAgeTo1.SelectedValue = gvImmunizationBaby.SelectedRow.Cells[16].Text;
            ddlAgeTo2.SelectedValue = gvImmunizationBaby.SelectedRow.Cells[17].Text;
            ViewState["ID"] = gvImmunizationBaby.SelectedRow.Cells[0].Text;
            //ddlImmunizationName.Enabled = false;
            ibtnSaveImmunizationBabySchedule.Visible = false;
            btnNew.Visible = true;
            ibtnUpdateImmunizationBabySchedule.Visible = true;
            txtAgeFrom1.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    // Akshay Tirathram 21-Sep-2022
    protected void FillDotor()
    {
        BaseC.Hospital hospObj = new BaseC.Hospital(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = hospObj.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDoctor.DataSource = ds.Tables[0];
                ddlDoctor.DataTextField = "DoctorName";
                ddlDoctor.DataValueField = "DoctorID";
                ddlDoctor.DataBind();
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
