using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class EMR_Masters_HospitalSetupSettingsPopUp : System.Web.UI.Page
{

    //<%--------------------------------------Yogesh------------1/04/2022--------------------%>

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataTable dt = new DataTable();
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindDdlName();
            bindDdlType();
        }
        else
        {

            BaseC.Hospital objH = new BaseC.Hospital(sConString);
            ds = objH.getImageHW();
            ds1 = objH.getType();
            ViewState["DT"] = ds.Tables[0];

        }
    }


    private void bindDdlName()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {

            string disString = "SELECT ReportName,ReportId from EMRTemplateReportSetup where active = 1";
            ds = objDl.FillDataSet(CommandType.Text, disString);
            ddlName.DataSource = ds.Tables[0];
            ddlName.DataTextField = "ReportName";
            ddlName.DataValueField = "ReportId";
            ddlName.DataBind();
            ddlName.Items.Insert(0, new ListItem("Select", ""));

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }

    private void bindDdlType()
    {
        BaseC.Hospital objH = new BaseC.Hospital(sConString);
        try
        {

            ds1 = objH.getType();
            ddlType.DataSource = ds1;
            ddlType.DataTextField = "ImageName";
            ddlType.DataValueField = "id";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem("Select", ""));

            // dt = ds.Tables[0];
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseC.Hospital objH = new BaseC.Hospital(sConString);
        if (txtHeight.Text.Equals(""))
        {
            lblmsg.Visible = true;
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = "Please Enter Height";
            return;
        }
        else
        {
            lblmsg.Visible = false;
        }
        if (txtWidth.Text.Equals(""))
        {
            lblmsg.Visible = true;
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = "Please Enter Width";
            return;
        }
        else
        {
            lblmsg.Visible = false;
        }

        if (ddlName.SelectedItem.Text.Equals("Select"))
        {
            lblmsg.Visible = true;
            lblmsg.ForeColor = System.Drawing.Color.Red;
            lblmsg.Text = "Please Select Report Name";
            return;
        }
        else
        {
            lblmsg.Visible = false;
        }

        lblmsg.Visible = false;
        Session["LogoHeight"] = txtHeight.Text.Trim();
        Session["LogoWidth"] = txtWidth.Text.Trim();

        // Akshay
        string respose;
        if (ddlName.SelectedItem.Text.Equals("Prescription") || ddlName.SelectedItem.Text.Equals("DISCHARGE  SUMMARY") || ddlName.SelectedItem.Text.Equals("NORMAL SUMMARY"))
        {
            respose = objH.saveHeightWidthImage(Convert.ToInt32(ddlName.SelectedValue), Convert.ToInt32(txtHeight.Text), Convert.ToInt32(txtWidth.Text), common.myInt(ddlType.SelectedValue), common.myBool(chkHospitalLogo.Checked), common.myBool(chkAddress.Checked), common.myBool(chkNBAHLogo.Checked));
        }
        else
        {
            respose = objH.saveHeightWidthImage(Convert.ToInt32(ddlName.SelectedValue), Convert.ToInt32(txtHeight.Text), Convert.ToInt32(txtWidth.Text), common.myInt(ddlType.SelectedValue), true, true, false);
        }
        //
        if (respose.Length > 0)
        {
            txtHeight.Text = "";
            txtWidth.Text = "";

            //yogesh 6/05/2022
            ddlType.ClearSelection();
            ddlName.ClearSelection();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "<Scripts>", "<script>alert('" + respose + "')</script>");
            bindDdlName();
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "<Scripts>", "<script>alert('Height & Width Didn't saved Or Update!')</script>");
        }

    }
    //protected void ddlName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlName.SelectedItem.Text.Equals("Select"))
    //        return;
    //    int id= Convert.ToInt32(ddlName.SelectedValue);
    //    txtHeight.Text = dt.Rows[id - 1]["Height"].ToString();
    //    txtWidth.Text = dt.Rows[id - 1]["Width"].ToString();
    //    Session["id"] = id;
    //}



    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //yogesh 6/05/2022

        if (ddlName.SelectedIndex == 0)
        {
            ddlType.SelectedIndex = 0;
            Alert.ShowAjaxMsg("please select Report.", Page);

            return;
        }
        if (ddlType.SelectedItem.Text.Equals("Select"))
            return;
        DataView dv = new DataView((DataTable)ViewState["DT"]);
        dv.RowFilter = "ReportId='" + ddlName.SelectedValue + "' AND typeId='" + ddlType.SelectedValue + "'";

        if (dv.ToTable().Rows.Count > 0)
        {
            txtHeight.Text = dv.ToTable().Rows[0]["height"].ToString();
            txtWidth.Text = dv.ToTable().Rows[0]["width"].ToString();
        }
        else
        {
            txtHeight.Text = "";
            txtWidth.Text = "";
        }

    }

    //<%------------------------------------Yogesh------------1/04/2022----------------%>
}

