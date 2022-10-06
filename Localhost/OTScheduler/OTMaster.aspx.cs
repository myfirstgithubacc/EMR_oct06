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

public partial class EMR_Masters_OT : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    Hashtable HashIn;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateFacility();
            GetOTs();
            
        }
    }
    private void PopulateFacility()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HashIn = new Hashtable();
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intUserId", Session["UserID"]);
            HashIn.Add("@intGroupId", Session["GroupID"]);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "uspgetfacilitylist", HashIn);

            ddlFacility.DataSource = dr;
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void GetOTs()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HashIn = new Hashtable();
            HashIn.Add("@inyHospitalLocationId", Convert.ToInt32(Session["HospitalLocationID"]));
            HashIn.Add("intFacilityId", common.myInt(Session["FacilityId"]));
            DataSet dsOT = dl.FillDataSet(CommandType.StoredProcedure, "uspGetOTs", HashIn);
            if (dsOT.Tables[0].Rows.Count > 0)
            {
                gvOT.DataSource = dsOT.Tables[0];
                gvOT.DataBind();
            }
            else
            {
                gvOT.DataSource = null;
                gvOT.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnOTSave_Click(object sender, EventArgs e)
    {
        //try
        //{
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HashIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            if (hdnOTID.Value != "")
            {
                HashIn.Add("@intOTID", hdnOTID.Value);
            }
            HashIn.Add("@inyHospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
            HashIn.Add("@intFacilityID", Convert.ToInt32(ddlFacility.SelectedValue));
            HashIn.Add("@chvOTName", txtOTName.Text);
            HashIn.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
            HashIn.Add("@bitStatus",ddlStatus.SelectedValue);
            if(chkIsOTAdvance.Checked)
            {
                HashIn.Add("@bitOTAdvanceManadetory", 1);
            }
            else
           {
                HashIn.Add("@bitOTAdvanceManadetory", 0);
           }
        string FreezeTime = null ;
        if (RadFreezeTime.SelectedDate != null)
            FreezeTime = RadFreezeTime.SelectedDate.Value.ToString("hh:mm tt");

        HashIn.Add("@OTFreezeTime", FreezeTime );
        HshOut.Add("@chvOutput",SqlDbType.VarChar);

            HshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateOTName", HashIn, HshOut);
            //if (HshOut.Contains("OT save successfull"))
            //{
                lblMessage.Text = HshOut["@chvOutput"].ToString();
        //}
        //hdnOTID.Value = "";
        //txtOTName.Text = "";
        //ddlFacility.SelectedValue = "0";
        //chkIsOTAdvance.Checked = false;
        ClearMe();

        GetOTs();
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}
    }
    protected void gvOT_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hdnOTID.Value = gvOT.SelectedRow.Cells[0].Text;
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@intID", Convert.ToInt32(hdnOTID.Value));
            hshInput.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            hshInput.Add("intFacilityId", common.myInt(Session["FacilityId"]));
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "uspGetOTs", hshInput);
            if (dr.Read())
            {
                ddlFacility.SelectedValue = dr["FacilityID"].ToString();
                txtOTName.Text = dr["TheatreName"].ToString();
                if (dr["Active"].ToString().Trim() == "False")
                    ddlStatus.SelectedValue = "0";
                else
                    ddlStatus.SelectedValue = "1";
                hdnUserID.Value = dr["UserId"].ToString().Trim();
                if(dr["IsOTAdvanceMandatory"].ToString().Trim()=="True")
                {
                    chkIsOTAdvance.Checked = true;
                }
                else
                {
                    chkIsOTAdvance.Checked = false;
                }
                if (!string.IsNullOrEmpty(common.myStr(dr["OTFreezeTime"])))
                    RadFreezeTime.SelectedDate = common.myDate(common.myStr(dr["OTFreezeTime"]));
                else
                    RadFreezeTime.SelectedDate = null;
                
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvOT_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Width = Unit.Pixel(100);
        }
    }

    protected void gvOT_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string id = gvOT.DataKeys[e.RowIndex].Value.ToString();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intID", id);
            HshIn.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
            //HshIn.Add("@FacilityID", Convert.ToInt32(gvScheduler.SelectedRow.Cells[3].ToString()));
            int i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspDeleteOT", HshIn);
            if (i == 0)
            {
                Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
            }
            GetOTs();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnNewOT_Click(object sender, EventArgs e)
    {
        ClearMe();
        lblMessage.Text = "";
    }

    private void ClearMe()
    {
        gvOT.SelectedIndex = -1;
        ddlFacility.SelectedIndex = 1;
        txtOTName.Text = "";
        ddlStatus.SelectedIndex = 0;
        hdnOTID.Value = "";
        chkIsOTAdvance.Checked = false;
        RadFreezeTime.SelectedDate = null;
        txtOTName.Focus();
    }
}
