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
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;

public partial class EMR_Masters_LegendMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    Hashtable hstIn;
    DAL.DAL objDl;
    BaseC.ParseData bc = new BaseC.ParseData();
    BaseC.EncryptDecrypt objEncDc = new BaseC.EncryptDecrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        //trAddCustomColor.Visible = false;
        //trAddStatus.Visible = false;
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            // PopulateLegends();
            PopulateType();
            ddlType_SelectedIndexChanged(this, null);
            //BindSelectedGrid();
            //populatColorList();
           
        }
        //populatColorList();
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    private void PopulateLegends()
    {
        try
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstIn = new Hashtable();
            hstIn.Add("@inyHopitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            DataSet objDs = objDl.FillDataSet(CommandType.Text, "Select * From AppointmentStatus(@inyHopitalLocationId)", hstIn);
            ddlLegendType.DataSource = objDs.Tables[0];
            ddlLegendType.DataTextField = "Status";
            ddlLegendType.DataValueField = "StatusId";
            ddlLegendType.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void PopulateType()
    {
        try
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstIn = new Hashtable();
            hstIn.Add("@inyHopitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            DataSet objDs = objDl.FillDataSet(CommandType.Text, "select distinct StatusType from statusMaster where ColorName IS NOT NULL", hstIn);
            ddlType.DataSource = objDs.Tables[0];
            ddlType.DataTextField = "StatusType";
            ddlType.DataValueField = "StatusType";
            ddlType.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindSelectedGrid()
    {
        try
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstIn = new Hashtable();
            hstIn.Add("@inyHopitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            hstIn.Add("@Type", ddlType.SelectedItem.Text);

            string strQuery = "Select * From GetStatus(@inyHopitalLocationId,@Type)";
            DataSet objDs = objDl.FillDataSet(CommandType.Text, strQuery, hstIn);
            gvSelectedLegends.DataSource = objDs.Tables[0];
            gvSelectedLegends.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void populatColorList()
    {
        try
        {
            ddlColorList.Items.Clear();
            ddlStatusColor.Items.Clear();
            ListItem lst;
            ListItem lstStatusColor;
            //string[] colorNames = System.Enum.GetNames(typeof(KnownColor));

            DataSet ds = new DataSet();
            Hashtable HashIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet Colordt = new DataSet();

            Colordt = dl.FillDataSet(CommandType.Text, "select ColorCode,ColorName from ColorMaster");
            for (int i = 0; i < Colordt.Tables[0].Rows.Count; i++)
            {
                //System.Drawing.Color objClr = (System.Drawing.Color)System.Drawing.Color.FromName(item);
                //KnownColor selectedColor = (KnownColor)System.Enum.Parse(typeof(KnownColor), item);

                //Byte A;
                //Byte R;
                //Byte G;
                //Byte B;

                //A = System.Drawing.Color.FromKnownColor(selectedColor).A;
                //R = System.Drawing.Color.FromKnownColor(selectedColor).R;
                //G = System.Drawing.Color.FromKnownColor(selectedColor).G;
                //B = System.Drawing.Color.FromKnownColor(selectedColor).B;

                //string htmlHexColorValue = ColorTranslator.ToHtml(Color.FromArgb(A, R, G, B));

                lst = new ListItem();
                lstStatusColor = new ListItem();
                //if (item == "Black")

                lst.Attributes.Add("style", "background-color:" + Colordt.Tables[0].Rows[i].ItemArray[0].ToString() + ";");
                lstStatusColor.Attributes.Add("style", "background-color:" + Colordt.Tables[0].Rows[i].ItemArray[0].ToString() + ";");
                //else
                //    lst.Attributes.Add("style", "background-color:" + System.Drawing.ColorTranslator.ToHtml(objClr) + "; color: Black;");

                lst.Text = Colordt.Tables[0].Rows[i].ItemArray[1].ToString();
                lstStatusColor.Text = Colordt.Tables[0].Rows[i].ItemArray[1].ToString();
                lst.Value = Colordt.Tables[0].Rows[i].ItemArray[0].ToString();
                lstStatusColor.Value = Colordt.Tables[0].Rows[i].ItemArray[0].ToString();
                ddlColorList.Items.Add(lst);
                ddlStatusColor.Items.Add(lstStatusColor);
            }
            ddlColorList.Items.Insert(0, "Select");
            ddlStatusColor.Items.Insert(0, "Select");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvSelectedLegends_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Visible = false;
            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Alternate))
            {
                String strHexVal = e.Row.Cells[2].Text.ToString().Trim();
                if (strHexVal != "&nbsp;")
                {
                    e.Row.Cells[2].BackColor = System.Drawing.ColorTranslator.FromHtml(strHexVal);
                    e.Row.Cells[2].Text = string.Empty;
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

    protected void ibtnLegendColorSave_OnClick(Object sender, EventArgs e)
    {
        try
        {
            hstIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstIn.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            hstIn.Add("@StatusId", Convert.ToInt32(ddlLegendType.SelectedValue));
            hstIn.Add("@StatusColor", Convert.ToString(ddlColorList.SelectedItem.Value));
            hstIn.Add("@ColorName", Convert.ToString(ddlColorList.SelectedItem.Text));
            hstIn.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));

            Hashtable hshout = new Hashtable();
            hshout.Add("@chvErrorStatus", SqlDbType.VarChar);

            int i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveColorLegends", hstIn, hshout);
            if (i == 0)
            {
                lblMessage.Text = "Record(s) Has Been Saved...";
            }
            populatColorList();
            BindSelectedGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstIn = new Hashtable();
            hstIn.Add("@Type", ddlType.SelectedItem.Text);
            string strQuery = "Select StatusId,Status From StatusMaster Where StatusType = @Type And Active = 1 Order By SequenceNo";
            DataSet dsStatus = dl.FillDataSet(CommandType.Text, strQuery, hstIn);
            ddlLegendType.DataSource = dsStatus;
            ddlLegendType.DataTextField = "Status";
            ddlLegendType.DataValueField = "StatusId";
            ddlLegendType.DataBind();
            BindSelectedGrid();
            populatColorList();

            if (ddlType.SelectedItem.Value == "0")
            {
                trCustomStatus.Visible = true;
            }
            else
            {
                trCustomStatus.Visible = false;
                trAddStatus.Visible = false;
                trAddStatus.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkbAddCustomColor_OnClick(object sender, EventArgs e)
    {
        trAddCustomColor.Visible = true;
        txtColorCode.Text = "";
        txtColorName.Text = "";
        trAddStatus.Visible = false;
    }

    protected void btnSaveColor_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstIn = new Hashtable();
            hstIn.Add("@Type", ddlType.SelectedItem.Text);
            string strQuery = "Select * From ColorMaster Where ColorName = '" + txtColorName.Text.Trim() + "' Or ColorCode = '" + txtColorCode.Text.Trim() + "' ";
            DataSet dsColor = dl.FillDataSet(CommandType.Text, strQuery, hstIn);
            if (dsColor.Tables[0].Rows.Count > 0)
            {
                lblMessage.Text = "Color Name and Code Already Exists.";
                return;
            }

            strQuery = "insert into ColorMaster  (ColorName , ColorCode )    values('" + txtColorName.Text.Trim() + "' ,  '" + txtColorCode.Text.Trim() + "') ";
            dl.ExecuteNonQuery(CommandType.Text, strQuery, hstIn);

            lblMessage.Text = "Color Saved...";
            trAddCustomColor.Visible = false;

            populatColorList();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        trAddCustomColor.Visible = false;
    }

    protected void lnkbAddStatus_OnClick(object sender, EventArgs e)
    {
        trAddStatus.Visible = true;
        trAddCustomColor.Visible = false;
        txtstatus.Text = "";
        txtStatusCode.Text = "";
        ddlStatusColor.SelectedIndex = 0;
    }

    protected void btnCancelStatus_OnClick(object sender, EventArgs e)
    {
        trAddStatus.Visible = false;
    }

    protected void btnSaveStatus_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstIn = new Hashtable();

            string strQuery = "Select * From StatusMaster Where ltrim(rtrim(StatusType)) = 'CustomAppointment' ";
            DataSet dsStatus = dl.FillDataSet(CommandType.Text, strQuery, hstIn);

            DataView dvStatus = new DataView();
            dvStatus = dsStatus.Tables[0].DefaultView;
            dvStatus.RowFilter = " Status= '" + txtstatus.Text.Trim() + "' ";
            if (dvStatus.Count > 0)
            {
                lblMessage.Text = "Status Allready Exists";
                return;
            }

            DataView dvColor = new DataView();
            dvColor = dsStatus.Tables[0].DefaultView;
            dvColor.RowFilter = " StatusColor = '" + ddlStatusColor.SelectedItem.Value + "' ";
            if (dvColor.Count > 0)
            {
                lblMessage.Text = "Status Color Allready Exists";
                return;
            }

            DataView dvCode = new DataView();
            dvCode = dsStatus.Tables[0].DefaultView;
            dvCode.RowFilter = " Code = '" + txtStatusCode.Text + "' ";
            if (dvCode.Count > 0)
            {
                lblMessage.Text = "Status Code Allready Exists";
                return;
            }

            dsStatus = null;
            strQuery = "Select max(SequenceNo)+1 SequenceNo From StatusMaster Where ltrim(rtrim(StatusType)) = 'CustomAppointment' "
                    + " Select max(StatusID)+1 StatusID  From StatusMaster  ";
            dsStatus = dl.FillDataSet(CommandType.Text, strQuery, hstIn);

            int iSeqNo = 0;
            if (dsStatus.Tables[0].Rows.Count > 0)
                if (dsStatus.Tables[0].Rows[0][0] != null)
                    iSeqNo = Convert.ToInt32(dsStatus.Tables[0].Rows[0][0].ToString());


            int iStatusID = 0;
            if (dsStatus.Tables[1].Rows.Count > 0)
                if (dsStatus.Tables[1].Rows[0][0] != null)
                    iStatusID = Convert.ToInt32(dsStatus.Tables[1].Rows[0][0].ToString());

            strQuery = "insert into StatusMaster (StatusID, StatusType, Status, StatusColor, ColorName, Code, SequenceNo, Active )   "
                    + "  values(" + iStatusID + ",'CustomAppointment', '" + txtstatus.Text + "', '" + ddlStatusColor.SelectedItem.Value.Trim() + "' ,  '" + ddlStatusColor.SelectedItem.Text.Trim() + "', '" + txtStatusCode.Text + "'  ,  " + iSeqNo + " , " + ddlStatusActive.SelectedItem.Value + " ) ";

            dl.ExecuteNonQuery(CommandType.Text, strQuery, hstIn);

            lblMessage.Text = "Status Saved...";
            trAddCustomColor.Visible = false;

            ddlType_SelectedIndexChanged(this, null);
            BindSelectedGrid();
            trAddStatus.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
