using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Telerik.Web.UI;

public partial class MPages_SpecialRightMaster : System.Web.UI.Page
{

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshInput;
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    BaseC.EMRBilling objBC;
    private bool RowSelStatus = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindDetailsData();
            txtFlag.Focus();
        }
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
      
        try{
            objBC = new BaseC.EMRBilling(sConString);
            if (txtFlag.Text == "")
            {
                Alert.ShowAjaxMsg("Please enter Flag",Page);
                return;
            }
            if (txtDescription.Text == "")
            {
                Alert.ShowAjaxMsg("Please enter Flag", Page);
                return;
            }
            if (btnSave.Text == "Save")
            {
                string output = objBC.SaveSpecialRightMaster(0, txtFlag.Text, common.myInt(Session["HospitalLocationId"]), txtDescription.Text, common.myBool(chkActive.Checked), common.myInt(Session["UserId"]));
                lblMessage.Text = output;
                bindDetailsData();
                txtFlag.Text = string.Empty;
                txtDescription.Text = string.Empty;
            }
            else
            {
                lblMessage.Text = objBC.SaveSpecialRightMaster(common.myInt(hdnFlagId.Value), txtFlag.Text, common.myInt(Session["HospitalLocationId"]), txtDescription.Text, common.myBool(chkActive.Checked), common.myInt(Session["UserId"]));
                btnSave.Text = "Save";
                bindDetailsData();
                txtFlag.Text = "";
                txtDescription.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindDetailsData()
    {
        objBC = new BaseC.EMRBilling(sConString);
        DataTable  dt = new DataTable();
        try
        {          
            dt = objBC.getSpecialRightMaster(common.myInt(Session["HospitalLocationId"]));
            gvData.DataSource = dt;
            gvData.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
    }

    protected void gvData_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        bindDetailsData();
    }
    protected void gvData_PreRender(object sender, EventArgs e)
    {
        if (!RowSelStatus)
        {
            bindDetailsData();
        }
    }
    protected void gvData_OnSelectedIndexChanged(object sender, EventArgs e)
    {
       
            RowSelStatus = true;     

            try
            {
                btnSave.Text = "Update";
                HiddenField hdnItemFlagId = (HiddenField)gvData.SelectedItems[0].FindControl("hdnID");
                Label lblFlag = (Label)gvData.SelectedItems[0].FindControl("lblFlag");
                Label lblDescription = (Label)gvData.SelectedItems[0].FindControl("lblDescription");
                Label active = (Label)gvData.SelectedItems[0].FindControl("lblActive");
                txtFlag.Text = lblFlag.Text;
                txtDescription.Text = lblDescription.Text;
                hdnFlagId.Value = hdnItemFlagId.Value;
                chkActive.Checked = common.myBool(active.Text);
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }

            //objPharmacy = new BaseC.clsPharmacy(sConString);
            //DataSet ds = objPharmacy.GetItemFlagMaster(common.myInt(hdnItemFlagId.Value), common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["UserID"]));
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    txtItemFlagName.Text = common.myStr(ds.Tables[0].Rows[0]["ItemFlagName"]);
            //    ddlstatus.SelectedIndex = ddlstatus.Items.IndexOf(ddlstatus.Items.FindItemByText(common.myStr(ds.Tables[0].Rows[0]["status"]).Trim()));

            //    ViewState["ItemFlagId"] = hdnItemFlagId.Value;
            //}
         
        
    }
    
}
