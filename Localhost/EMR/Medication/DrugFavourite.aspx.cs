using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Configuration;
using System.Net;

public partial class EMR_Medication_DrugFavourite : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["GenericId"] != null || Request.QueryString["ItemId"] != null)
            {
                GetBrandFavouriteData("", 0, 0 , common.myInt(Request.QueryString["DoctorId"]));
                lblFavouriteName.Text = Request.QueryString["ItemName"] == null ? "" : Request.QueryString["ItemName"].ToString();
            }
        }
    }
    protected void btnAddtoFav_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["ItemId"] != null && Request.QueryString["ItemId"].ToString() != "")
            {
                BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                string strMsg = objEMR.SaveFavouriteDrugs(common.myInt(Request.QueryString["DoctorId"]), common.myInt(Request.QueryString["ItemId"]), 
                                                string.Empty, common.myInt(Session["UserID"]), 0, 0, 0, 0, 0, 0, 0, 
                                                string.Empty, 0, string.Empty, string.Empty, 0);
                
                lblMessage.Text = strMsg;
                lblFavouriteName.Text = "";
                GetBrandFavouriteData("", 0, 0, common.myInt(Request.QueryString["DoctorId"]));
            }
            else
            {
                Alert.ShowAjaxMsg("Please select drug to save favourite", Page);
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
    protected void lstFavourite_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ItemDelete")
        {
            int ItemId = common.myInt(e.CommandArgument);

            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            string sResult = objEMR.DeleteFavoriteDrugs(common.myInt(Request.QueryString["DoctorId"]),
                                 ItemId,"",  common.myInt(Session["UserId"]), 0);
            lblMessage.Text = "Favourite Deleted";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            GetBrandFavouriteData("", 0, 0, common.myInt(Request.QueryString["DoctorId"]));
        }
    }
    private void GetBrandFavouriteData(string text, int GenericId,int ItemId,int DoctorId)
    {
        DataSet dsSearch = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        dsSearch = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
                            common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), ItemId,
                            GenericId, DoctorId, "", text.Replace("'", "''"));
        if (dsSearch.Tables[0].Rows.Count > 0)
        {
            lstFavourite.DataSource = dsSearch.Tables[0];
            lstFavourite.DataBind();
        }
        else
        {
            BlankFavouriteGrid();
        }
    }
    protected void lstFavourite_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnItemId = (HiddenField)lstFavourite.SelectedRow.FindControl("hdnItemId");
            Label lblItemName = (Label)lstFavourite.SelectedRow.FindControl("lblItemName");

            hdnPageItemId.Value = hdnItemId.Value;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BlankFavouriteGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ItemId", typeof(int));
        dt.Columns.Add("ItemName", typeof(string));
        DataRow dr = dt.NewRow();
        dr["ItemId"] = 0;
        dr["ItemName"] = "";
        dt.Rows.Add(dr);
        dt.AcceptChanges();
        lstFavourite.DataSource = dt;
        lstFavourite.DataBind();
    }
    protected void btnDeleteFavourite_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
            if (lstFavourite.SelectedIndex != -1)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hstInput = new Hashtable();
                hstInput.Add("@intDoctorID", common.myInt(Request.QueryString["DoctorId"]));
                hstInput.Add("@ItemId", "");
                hstInput.Add("@intEncodedBy", Session["UserId"]);
                objDl.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "UspEMRDeleteFavDrugs", hstInput);

                lblMessage.Text = "Drug deleted from your favorite list";
            }

           // PopulateFavouriteDrug("");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
