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

public partial class EMR_OTEquipment : System.Web.UI.Page
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
            GetOTEquipments();
            
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
    private void GetOTEquipments()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HashIn = new Hashtable();
            HashIn.Add("@intID",0);
            HashIn.Add("@inyHospitalLocationId", Convert.ToInt32(Session["HospitalLocationID"]));
            HashIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            HashIn.Add("@Active", 0);
            DataSet dsOT = dl.FillDataSet(CommandType.StoredProcedure, "uspGetOTEquipment", HashIn);
            if (dsOT.Tables[0].Rows.Count > 0)
            {
                gvOTEquipments.DataSource = dsOT.Tables[0];
                gvOTEquipments.DataBind();
            }
            else
            {
                gvOTEquipments.DataSource = null;
                gvOTEquipments.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnOTEquipmentsSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (ddlFacility.SelectedIndex == 0)
            {
                lblMessage.Text = "Please Select Facility!";
                return; 
            }
            if (txtOTEquipmentsName.Text.Trim()=="")
            {
                lblMessage.Text = "Please Enter Equipments Name!";
                return;
            }

            if (txtTotalQty.Text.Trim() == "")
            {
                lblMessage.Text = "Please Enter Qty.!";
                return;
            }
            else
            {
                if (common.myInt(txtTotalQty.Text.Trim())==0)
                {
                    lblMessage.Text = "Zero Qty. is not allow !";
                    return;
                }

            }


            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HashIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            if (hdnEquipmentId.Value != "")
            {
                HashIn.Add("@intOTEquipmentID", hdnEquipmentId.Value);
            }
            HashIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            HashIn.Add("@intFacilityID", common.myInt(ddlFacility.SelectedValue));
            HashIn.Add("@chvOTEquipmentName", txtOTEquipmentsName.Text.Trim());
            HashIn.Add("@intEncodedBy", common.myInt(Session["UserID"]));
            HashIn.Add("@bitStatus",ddlStatus.SelectedValue);
            HashIn.Add("@intTotalQty",common.myInt(txtTotalQty.Text.Trim()));
            HshOut.Add("@chvOutput",SqlDbType.VarChar);
           
            HshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateOTEquipment", HashIn, HshOut);

            if (common.myStr(HshOut["@chvOutput"]).Contains(" save") || common.myStr(HshOut["@chvOutput"]).Contains(" update"))
            {
                lblMessage.Text = HshOut["@chvOutput"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            else
            {
                lblMessage.Text = HshOut["@chvOutput"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            }

            hdnEquipmentId.Value = "";
            txtOTEquipmentsName.Text = "";
            txtTotalQty.Text = "";  
            ddlFacility.SelectedValue = "0";
            GetOTEquipments();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvOTEquipments_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }

    protected void gvOTEquipments_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {

        }
    }

    
    protected void btnNewOT_Click(object sender, EventArgs e)
    {
        ddlFacility.SelectedIndex = 0;
        txtOTEquipmentsName.Text = "";
        txtTotalQty.Text = "";  
        ddlStatus.SelectedIndex = 0;
        lblMessage.Text = "";
    }

    protected void gvOTEquipments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "btnEdit")
        {
            try
            {
                lblMessage.Text = string.Empty;    
                hdnEquipmentId.Value =common.myStr(e.CommandArgument);
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshInput = new Hashtable();
                hshInput.Add("@intID", Convert.ToInt32(hdnEquipmentId.Value));
                hshInput.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"])); 
                hshInput.Add("@Active",0);

                SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "uspGetOTEquipment", hshInput);
                if (dr.Read())
                {
                    ddlFacility.SelectedValue = common.myStr(dr["FacilityID"]);
                    txtOTEquipmentsName.Text = common.myStr(dr["EquipmentName"]);
                    txtTotalQty.Text = common.myStr(dr["TotalQty"]);
                    if (dr["Active"].ToString().Trim() == "False")
                        ddlStatus.SelectedValue = "0";
                    else
                        ddlStatus.SelectedValue = "1";
                    hdnUserID.Value = dr["UserId"].ToString().Trim();
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

    protected void gvOTEquipments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOTEquipments.PageIndex  = e.NewPageIndex;
        GetOTEquipments();
    }
    
}
