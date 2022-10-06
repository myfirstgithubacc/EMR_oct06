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
using Telerik.Web.UI;
using System.Text;



public partial class MRD_Master_ICD9SubTaggingMaster : System.Web.UI.Page
{
private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsMRD objBb;
    private bool RowSelStatus = false;
    UserAuthorisations ua1 = new UserAuthorisations();


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        //Page.Theme = "DefaultControls";
        //this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        //if (common.myStr(Request.QueryString["MP"]) == "NO")
        //{
        //    this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objBb = new BaseC.clsMRD(sConString);

            ViewState["_ID"] = 0;
            bindGroup();
            bindDetailsData();
            SetPermission();
        }
    }

    private void bindGroup()
    {      
        DataSet ds = new DataSet();
        try
        {
            objBb =new BaseC.clsMRD(sConString);
            ds = objBb.getIcd9Group();

            ddlGroup.DataSource = ds.Tables[0].Copy();
            ddlGroup.DataValueField = "Id";
            ddlGroup.DataTextField = "Name";
            ddlGroup.DataBind();

            ddlGroup.Items.Insert(0, new RadComboBoxItem("", "0"));


            bindSubGroup();
         
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    private void bindSubGroup()
    {
        DataSet ds = new DataSet();
        try
        {
            if (ddlGroup.SelectedIndex  < 1)
            {
                return;
            }
            int grpid;
            ddlSubGroup.Text = "";
            grpid = Convert.ToInt32(ddlGroup.SelectedValue);
            ds = new DataSet();
            objBb = new BaseC.clsMRD(sConString);
            ds = objBb.getIcd9SubGroup(grpid);
            ddlSubGroup.DataSource = ds.Tables[0].Copy();
            ddlSubGroup.DataValueField = "Id";
            ddlSubGroup.DataTextField = "Name";
            ddlSubGroup.DataBind();

            //ddlSubGroup.Items.Insert(0, new RadComboBoxItem("", "0"));


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    private void bindDetailsData()
    {
        try
        {
            objBb = new BaseC.clsMRD(sConString);
            //DataSet ds = objBb.GetIcdCodeList(((!IsPostBack) ? -1 : 0), common.myInt(ddlGroup.SelectedValue), common.myInt(ddlSubGroup.SelectedValue));
            StringBuilder GroupIds = new StringBuilder();
            string GroupIdsStr = string.Empty;
            if (ddlSubGroup.CheckedItems.Count > 0)
            {
                for (int i = 0; i < ddlSubGroup.CheckedItems.Count; i++)
                {
                    GroupIds.Append(ddlSubGroup.CheckedItems[i].Value.ToString() + ",");
                }
                GroupIdsStr = GroupIds.ToString().Substring(0, GroupIds.ToString().Length - 1);
            }
            
            
            
            int pageindex = 0;
            if (gvData.Items.Count > 0)
            {
                pageindex = gvData.CurrentPageIndex + 1;
            }
            else
            {
                pageindex = 1;
            }
           
            DataSet ds = objBb.GetIcdCodeList(common.myInt(ddlGroup.SelectedValue), common.myStr(GroupIdsStr), pageindex, gvData.PageSize, "%" + common.myStr(txtICD9Desc.Text) + "%");

            //gvData.VirtualItemCount = Convert.ToInt32(ds.Tables[0].Rows[0]["totalrecord"]);    Commented by Akshay Sharma
            if (ds.Tables[0].Rows.Count != 0)   // Condition written by Akshay Sharma
            {
                gvData.VirtualItemCount = Convert.ToInt32(ds.Tables[0].Rows[0]["totalrecord"]);
            }
            gvData.DataSource = ds.Tables[0];
            gvData.DataBind();
            ViewState["gvData"] = ds;
            
            
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }



    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    private void clearControl()
    {

        ddlStatus.SelectedIndex = 0;

        //----------------------commented by prashant------------
        ddlGroup.SelectedIndex = 0;
        ddlSubGroup.SelectedIndex = 0;
        txtICD9Code.Text = string.Empty;
        txtICD9Desc.Text = string.Empty;
    }

    protected bool isSave()
    {
        try
        {
            string strmsg = "";
            bool isValid = true;

            if (common.myInt(ddlSubGroup.SelectedValue) == 0)
            {
                strmsg += "Sub Group not selected !";
                isValid = false;
            }
           
            else if (common.myInt(ddlGroup.SelectedValue) == 0)
            {
                strmsg += "Group not selected ! ";
                isValid = false;
            }

            else if (common.myStr(txtICD9Desc.Text) == "")
            {
                strmsg += "Enter ICD Description ! ";
                isValid = false;
            }
            else if (common.myStr(txtICD9Code.Text) == "")
            {
                strmsg += "Enter ICD Code ! ";
                isValid = false;
            }
            
            lblMessage.Text = strmsg;
            return isValid;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
            return false;
        }
    }

    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (!isSave())
            {
                return;
            }

            objBb = new BaseC.clsMRD(sConString);
            string strMsg = objBb.SaveICD9Disease(common.myInt(ViewState["_ID"]), common.myStr(txtICD9Code.Text), common.myStr(txtICD9Desc.Text.ToUpper()),
                                        common.myInt(ddlGroup.SelectedValue), common.myInt(ddlSubGroup.SelectedValue),  
                                        common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]));

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                clearControl();
                bindDetailsData();                
                ViewState["_ID"] = 0;
                SetPermission();
                ddlStatus.Enabled = false;
            }
            lblMessage.Text = strMsg;
            bindDetailsData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvData_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvData.CurrentPageIndex = e.NewPageIndex;
        bindDetailsData();
    }

    protected void gvData_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (e.CommandName == "Select")
            {
                RowSelStatus = true;

                ddlStatus.Enabled = true;

                if (ua1.CheckPermissions("E", Request.Url.AbsolutePath))
                {
                    ua1.DisableEnableControl(btnSaveData, true);
                }
                else
                {
                    ua1.DisableEnableControl(btnSaveData, false);
                }

                HiddenField hdnICDId = (HiddenField)e.Item.FindControl("hdnICDId");
                HiddenField hdnGroupid = (HiddenField)e.Item.FindControl("hdnGroupid");
                HiddenField hdnSubGroupId = (HiddenField)e.Item.FindControl("hdnSubGroupId");
                HiddenField hdnStatus = (HiddenField)e.Item.FindControl("hdnStatus");

                
                Label lbldescription = (Label)e.Item.FindControl("lblDescription");
                Label lblICDCode = (Label)e.Item.FindControl("lblICDCode");
               
                ViewState["_ID"] = common.myInt(hdnICDId.Value);


                ddlGroup.SelectedValue = common.myStr(hdnGroupid.Value);//ddlDonationType.Items.IndexOf(Convert.ToInt32(hdnDonorTypeId.Value));
                bindSubGroup();
                ddlSubGroup.SelectedValue = common.myStr(hdnSubGroupId.Value);
                txtICD9Code.Text = lblICDCode.Text; //common.myStr(ds.Tables[0].Rows[0]["DonationRepeatDays"]);
                txtICD9Desc.Text = lbldescription.Text;
                ddlStatus.SelectedValue = common.myBool(hdnStatus.Value) == true ? "1" : "0";

                //objBb = new BaseC.clsBb(sConString);
                //DataSet ds = objBb.GetDonationDonorTypeMaster(common.myInt(ViewState["_ID"]), common.myInt(ddlGroup.SelectedValue), common.myInt(ddlSubGroup.SelectedValue), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);               

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    //txtDonorType.Text = common.myStr(ds.Tables[0].Rows[0]["DonorType"]);
                //    ////ddlDonorType.SelectedIndex = ddlDonorType.Items.IndexOf(ddlDonorType.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["DonorTypeId"])));
                //    ////ddlDonationType.SelectedIndex = ddlDonationType.Items.IndexOf(ddlDonationType.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["DonationTypeId"])));
                //    ////txtDonationRepeatDays.Text = common.myStr(ds.Tables[0].Rows[0]["DonationRepeatDays"]);
                //    ////txtDonorTypeDesc.Text = common.myStr(ds.Tables[0].Rows[0]["DonorTypeDesc"]);
                //    ////ddlStatus.SelectedValue = common.myBool(ds.Tables[0].Rows[0]["Active"]) == true ? "1" : "0";

                    
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

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
                lbl.Visible = false;

                RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
                combo.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

   
       
    protected void ddlDonationType_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlGroup.SelectedIndex > -1)
        {
            bindSubGroup();
        }
    }
    protected void ddlDonorType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        //bindDetailsData();
    }

    private void SetPermission()
    {
        ua1.DisableEnableControl(btnSaveData, false);
        ua1.DisableEnableControl(btnNew, false);

        if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnSaveData, true);
            ua1.DisableEnableControl(btnNew, true);
        }
        if (ua1.CheckPermissions("E", Request.Url.AbsolutePath))
        {
            // ua1.DisableEnableControl(btnSaveData, true);
            //ua1.DisableEnableControl(btnNew, true);
            // ua1.DisableEnableControl(btnclose, true);
            //ua1.DisableEnableControl(btnPost, true);
        }
        if (ua1.CheckPermissions("C", Request.Url.AbsolutePath))
        {
            //ua1.DisableEnableControl(btnCancel, true);
        }
        if (ua1.CheckPermissions("P", Request.Url.AbsolutePath))
        {
            //ua1.DisableEnableControl(bt, true);
            //ua1.DisableEnableControl(btnPrintPreview, true);
            //ua1.DisableEnableControl(btnPrint, true);
        }
        ua1.Dispose();
    }

    protected void gvData_PreRender(object sender, EventArgs e)
    {
        gvData.DataSource = (DataSet)ViewState["gvData"];
        gvData.DataBind();
        //bindDetailsData();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        bindDetailsData();
    }

    protected void imgBtnAddGroup_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/MRD/ICDFlagMaster.aspx";
            RadWindowForNew.Height = 500;
            RadWindowForNew.Width = 600;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void imgBtnAddSubGroup_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "~/MRD/Master/ICD9SubGroupMaster.aspx";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 700;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        bindGroup();
        bindSubGroup();
    }
}
