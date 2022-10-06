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

public partial class EMR_Masters_DoctorDuration : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();

    private enum GridDoctorVisitDuration : byte
    {
        ID = 0,
        SerialNo = 1,
        VisitType = 2,
        Duration = 3,
        IsPackageVisitStatus = 4,
        NoPAForInsurancePatientStatus = 5,
        NoBillForPrivatePatientStatus = 6,
        Status = 7,
        Deactivate = 8,
        Edit = 9,
        Active =10

       
    }

    private enum GridVisit : byte
    {
        VisitID = 0,
        SerialNo = 1,
        Name = 2,
        EMType = 3,
        IsPackageVisitStatus = 4,
        NoPAForInsurancePatientStatus = 5,
        NoBillForPrivatePatientStatus = 6,
        Status = 7,
        Edit = 8,
        Active = 9
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            BindEMType();
            BindVisitGrid();
        }
    }

    private void BindEMType()
    {
        try
        {
            ddlEM.Items.Clear();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, "select EMTypeId,EMType from EMRVisitTypeEM");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlEM.DataSource = ds;
                ddlEM.DataTextField = "EMType";
                ddlEM.DataValueField = "EMTypeId";
                ddlEM.DataBind();
            }
            ddlEM.Items.Insert(0, "Select");
            ddlEM.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDoctorVisitDuration_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.ID)].Visible = false;
                e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Active)].Visible = false;
                if (ViewState["BlankGrid"] != null)
                {
                    e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Deactivate)].Visible = false;
                    e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Edit)].Visible = false;
                }
            }

            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
            {
                Literal ltrl = (Literal)e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.VisitType)].FindControl("ltrlGridVisitType");
                ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
                img.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to De-Activate this record :  " + ltrl.Text + "')");
                if (e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Active)].Text.ToString().Trim() == "0")
                {
                    img.Visible = false;
                }
            }

            if (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))
            {
                DropDownList ddlGridStatus = (DropDownList)e.Row.FindControl("ddlGridStatus");
                Label lblGridAct = (Label)e.Row.FindControl("lblGridActive2");
                String strStatus = e.Row.Cells[Convert.ToByte(GridDoctorVisitDuration.Active)].Text.ToString().Trim();

                if (strStatus == "0")
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
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void SaveVisit_OnClick(Object sender, EventArgs e)
    {
        try
        {
            int iIsPackageVisit = 0, iNoPAForInsurancePatient = 0, iNoBillForPrivatePatient = 0;

            //if (chkIsPackageVisit.Checked == true)
            //    iIsPackageVisit = 1;
            //if (chkNoPAForInsurancePatient.Checked == true)
            //    iNoPAForInsurancePatient = 1;
            //if (chkNoBillForPrivatePatient.Checked == true)
            //    iNoBillForPrivatePatient = 1;

            BaseC.EMRMasters.EMRVisit objEMRVisit = new BaseC.EMRMasters.EMRVisit(sConString);
            String str = objEMRVisit.SaveVisit(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(ddlEM.SelectedValue.ToString()), Convert.ToString(bc.ParseQ(txtVisitName.Text.ToString().Trim())), common.myInt(iIsPackageVisit), Convert.ToInt32(Session["UserID"].ToString()),common.myInt(iNoBillForPrivatePatient),common.myInt(iNoPAForInsurancePatient));
            BindVisitGrid();
            txtVisitName.Text = "";
            //chkIsPackageVisit.Checked = false;
            //chkNoBillForPrivatePatient.Checked = false;
            //chkNoPAForInsurancePatient.Checked = false;
            lblMessage.Text = str;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindVisitGrid()
    {
        try
        {
            BaseC.EMRMasters.EMRVisit objEMRVisit = new BaseC.EMRMasters.EMRVisit(sConString);
            DataSet ds = objEMRVisit.GetVisit(Convert.ToInt16(Session["HospitalLocationID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvVisit.DataSource = ds;
                gvVisit.DataBind();
            }
            else
            {
                ViewState["Blank"] = "True";
                BindVisitBlankGrid();
                ViewState["Blank"] = null;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindVisitBlankGrid()
    {
        try
        {
            DataTable dtVisit = new DataTable();
            dtVisit.Columns.Add("VisitTypeID");
            dtVisit.Columns.Add("Type");
            dtVisit.Columns.Add("EMType");
            dtVisit.Columns.Add("Active");
            dtVisit.Columns.Add("Status");
            dtVisit.Columns.Add("IsPackageVisit");
            dtVisit.Columns.Add("IsPackageVisitStatus");
            dtVisit.Columns.Add("SerialNo");
            dtVisit.Columns.Add("NoPAForInsurancePatientStatus");
            dtVisit.Columns.Add("NoBillForPrivatePatientStatus");
            for (int i = 0; i < 10; i++)
            {
                DataRow Dr = dtVisit.NewRow();
                Dr["VisitTypeID"] = "";
                Dr["Type"] = "";
                Dr["EMType"] = "";
                Dr["Active"] = "";
                Dr["Status"] = "";
                Dr["IsPackageVisit"] = "";
                Dr["IsPackageVisitStatus"] = "";
                Dr["SerialNo"] = "";

                Dr["NoPAForInsurancePatientStatus"] = "";
                Dr["NoBillForPrivatePatientStatus"] = "";
                dtVisit.Rows.Add(Dr);
            }
            gvVisit.DataSource = dtVisit;
            gvVisit.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private DataSet getdataset()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = dl.FillDataSet(CommandType.Text, "select EMTypeId,EMType from EMRVisitTypeEM");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return ds;
    }

    private void binddropdown()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, "select EMTypeId,EMType from EMRVisitTypeEM");
            foreach (GridViewRow row in gvVisit.Rows)
            {
                DropDownList drdList = ((DropDownList)row.FindControl("ddlEMtype"));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    drdList.DataSource = ds;
                    drdList.DataValueField = "EMTypeId";
                    drdList.DataTextField = "EMType";
                    drdList.DataBind();
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

    int i = 1;

    protected void gvVisit_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSerial = (Label)e.Row.FindControl("lblSerial");
                lblSerial.Text = i.ToString();
                i++;
            }

            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[Convert.ToByte(GridVisit.VisitID)].Visible = false;
                e.Row.Cells[Convert.ToByte(GridVisit.Active)].Visible = false;
                if (ViewState["Blank"] != null)
                {

                    e.Row.Cells[Convert.ToByte(GridVisit.Edit)].Visible = false;
                }
            }

            if (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))
            {
                //DropDownList ddlGridStatus = (DropDownList)e.Row.FindControl("ddlGridStatus");
                //Label lblGridAct = (Label)e.Row.FindControl("lblGridActive2");
                //String strStatus = e.Row.Cells[Convert.ToByte(GridVisit.Active)].Text.ToString().Trim();

                //if (strStatus == "0")
                //{
                //    ddlGridStatus.Visible = true;
                //    lblGridAct.Visible = false;
                //}
                //else
                //{
                //    ddlGridStatus.Visible = false;
                //    lblGridAct.Visible = true;

                //}

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVisit_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeActivate")
            {
                BaseC.EMRMasters.EMRVisit objEMRVisit = new BaseC.EMRMasters.EMRVisit(sConString);
                Int32 i = objEMRVisit.DeActivateVisitName(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(e.CommandArgument), Convert.ToInt16(Session["UserID"]));
                if (i == 0)
                {
                    lblMessage.Text = "Record De-Actived... ";
                    gvVisit.EditIndex = -1;
                    BindVisitGrid();
                }
                else
                {
                    lblMessage.Text = "Error In De-Activation... ";
                    gvVisit.EditIndex = -1;
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

    protected void gvVisit_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvVisit.EditIndex = -1;
            BindVisitGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVisit_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string VisitID = gvVisit.Rows[e.RowIndex].Cells[common.myInt(GridVisit.VisitID)].Text;

            TextBox Name = (TextBox)gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.Name)].FindControl("txtGridVisitName");
            DropDownList EMTypeId = (DropDownList)gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.EMType)].FindControl("ddlEMtype");
            //string strActive = gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.Active)].Text;
            String strStatus = "";
            String strPackage = "";

            String strNoPAForInsurancePatient = "";
            String strNoBillForPrivatePatient = "";

            //if (strActive == "0")
            //{
            DropDownList strDrop = ((DropDownList)gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.Status)].FindControl("ddlGridStatus"));
            DropDownList strDropPackage = ((DropDownList)gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.IsPackageVisitStatus)].FindControl("ddlGridPackage"));
            DropDownList strDropNoPAForInsurancePatient = ((DropDownList)gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.NoPAForInsurancePatientStatus)].FindControl("ddlGridNoPAForInsurancePatient"));
            DropDownList strDropNoBillForPrivatePatientStatus = ((DropDownList)gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.NoBillForPrivatePatientStatus)].FindControl("ddlGridNoBillForPrivatePatient"));

            strStatus = strDrop.SelectedItem.Value;
            strPackage = strDropPackage.SelectedItem.Value;
            strNoPAForInsurancePatient = strDropNoPAForInsurancePatient.SelectedItem.Value;
            strNoBillForPrivatePatient = strDropNoBillForPrivatePatientStatus.SelectedItem.Value;
            //}
            //else if (strActive == "1")
            //{
            //    strStatus = "1";
            //}

            BaseC.EMRMasters.EMRVisit objEMRVisit = new BaseC.EMRMasters.EMRVisit(sConString);
            String str = objEMRVisit.UpdateVisit(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(VisitID), Convert.ToInt32(EMTypeId.SelectedValue), bc.ParseQ(Name.Text.ToString().Trim()), Convert.ToByte(strPackage), Convert.ToInt16(Session["UserID"]), Convert.ToByte(strStatus), Convert.ToByte(strNoBillForPrivatePatient), Convert.ToByte(strNoPAForInsurancePatient));
            lblMessage.Text = str;
            gvVisit.EditIndex = -1;
            BindVisitGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVisit_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvVisit.PageIndex = e.NewPageIndex;
            BindVisitGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVisit_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            // Label EMTypeId = (Label)gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.EMType)].FindControl("lblemtype");
            gvVisit.EditIndex = e.NewEditIndex;
            BindVisitGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lnkvisittypetaging_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindow1.NavigateUrl = "~/EMR/Masters/VisitTypeTaggingFacilityWise.aspx";
            RadWindow1.Height = 490;
            RadWindow1.Width = 660;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            // RadWindow1.OnClientClose = "OnClientClosePrescription";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}



