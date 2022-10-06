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

public partial class EMR_Masters_SMSSetup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();
    Hashtable HshIn;
    Hashtable HshOut;
    private const string DefaultCountry = "DefaultCountry";
    private const string DefaultState = "DefaultState";
    private const string DefaultCity = "DefaultCity";
    private const string DefaultZip = "DefaultZip";

    private enum Gridfacility : byte
    {
        FacilityID = 0,
        Name = 1,
        NPI = 2,
        PosId = 3,
        Entity = 4,
        Phone = 5,
        Fax = 6,
        Address = 7,
        CityId = 8,
        CityName = 9,
        StateId = 10,
        StateName = 11,
        CountryId = 12,
        CountryName = 13,
        PinNo = 14,
        Address2 = 15,
        TimeZoneId = 16,
        TimeZoneOffSetMinutes = 17,
        ReferToFacility = 18,
        MainFacility = 19,
        Active = 20,
        Status = 21,
        SMSSender = 22,
        smsServer = 23,
        CaseNoteAdd = 24
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            lblMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                bindDescriptionOfUse();
                BindFacilityGrid();


                //LocalCountry_SelectedIndexChanged(sender, e);
                btnUpdate.Visible = false;

                ////if (Session["HospitalLocationId"] != null)
                ////{
                ////    ClinicDefaults cd = new ClinicDefaults(Page);
                                  

                ////    //////ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindByValue(cd.GetHospitalDefaults(DefaultZip, Session["HospitalLocationId"].ToString())));
                ////}
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void bindDescriptionOfUse()
    {
        BaseC.SmsClass clssms = new BaseC.SmsClass(sConString);
        DataSet ds;
        try
        {
            ds = new DataSet();
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds=clssms.GetDescriptionOfUse();
         
            ddlDescriptionOfUse.DataSource = ds.Tables[0];
            ddlDescriptionOfUse.DataTextField = "DescriptionOfUse";
            ddlDescriptionOfUse.DataValueField = "EevntName";
            ddlDescriptionOfUse.DataBind();
            ddlDescriptionOfUse.Items.Insert(0, new ListItem("Select", "NA")); //updated code
            //dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
  
  
    private DataSet Facility(int iFacilityID)
    {
        DataSet ds = new DataSet();
        try
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //HshIn.Add("@inyUserId", Convert.ToInt16(Session["UserId"]));
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            HshIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSMSEventDetails", HshIn);
            return ds;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return ds;
        }
    }
   
   
    private void BindBlankGrid()
    {
        //    DataTable dtAllergy = new DataTable();
        //    dtAllergy.Columns.Add("FacilityID");
        //    dtAllergy.Columns.Add("Name");
        //    dtAllergy.Columns.Add("SerialNo");
        //    for (int i = 0; i < 10; i++)
        //    {
        //        DataRow Dr = dtAllergy.NewRow();
        //        Dr["FacilityID"] = "";
        //        Dr["Name"] = "";
        //        Dr["SerialNo"] = "";
        //        dtAllergy.Rows.Add(Dr);
        //    }
        //    gvSMSDetails.DataSource = dtAllergy;
        //    gvSMSDetails.DataBind();
    }
    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("Facility.aspx");
    }
    protected void btnSaveSMS_OnClick(Object sender, EventArgs e)
    {
         BaseC.SmsClass clsSMS=new BaseC.SmsClass(sConString); 
        //Alert.ShowAjaxMsg(str, this.Page);
         divtooltip.Visible = false;
         if (ddlDescriptionOfUse.SelectedValue == "NA")
         {
             Alert.ShowAjaxMsg("Select Event Name", this.Page);
             return ;
         }

         if (txtEventText.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
         {

             Alert.ShowAjaxMsg("Enter SMS Text", this.Page);
             return;
         }




         if (ddlDescriptionOfUse.SelectedValue != "0")
         {
             if (!txtEventText.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
             {
                 string StrSaveResponse = clsSMS.SaveSMSSetup(ddlDescriptionOfUse.SelectedValue, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), txtEventText.Text, common.myInt(Session["UserId"]));

                 lblMessage.Text = StrSaveResponse;
                 txtEventText.Text = string.Empty;
                 bindDescriptionOfUse();
             }
         }
        BindFacilityGrid();

    }
    private void BindFacilityGrid()
    {
        try
        {
            DataSet ds = (DataSet)Facility(0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSMSDetails.DataSource = ds;
                gvSMSDetails.DataBind();
            }
            else
            {

                DataRow dr = ds.Tables[0].NewRow();

                ds.Tables[0].Rows.InsertAt(dr, 0);
                gvSMSDetails.DataSource = ds.Tables[0];
                gvSMSDetails.DataBind();
                dr = null;

               
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void clearControl()
    {
        try
        {
          

          
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
   
    protected void gvFacility_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            btnUpdate.Visible = true;
         
          


            //btnSave.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void LocalCity_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            //ddlZip.Items.Clear();
            ////populate Local City drop down control
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ZIPID, ZipCode from zipmaster where CityID='" + dropLCity.SelectedValue.ToString() + "' ORDER BY ZipCode");
            //ddlZip.DataSource = dr;
            //ddlZip.DataTextField = "ZipCode";
            //ddlZip.DataValueField = "ZIPID";
            //ddlZip.DataBind();
            //dr.Close();
            //ddlZip.Items.Insert(0, "[Select]");
            //ddlZip.Items[0].Value = "0";
            //ddlZip.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvSMSDetails_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#e5ffff';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");

                HiddenField hdnCustomizedSMSId = (HiddenField)(e.Row.FindControl("hdnCustomizedSMSId"));

                if (common.myInt(hdnCustomizedSMSId.Value) > 0)
                {
                    ImageButton imgbtndel = (ImageButton)e.Row.FindControl("ibtnDelete");
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Pink;
                    imgbtndel.Visible = true;
                }
                else
                {
                    ImageButton imgbtndel = (ImageButton)e.Row.FindControl("ibtnDelete");
                    imgbtndel.Visible = false;         
                }
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvSMSDetails, "Select$" + e.Row.RowIndex);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void gvSMSDetails_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeActivate")
            {
                divtooltip.Visible = false;
                dvConfirmCancelOptions.Visible = true;
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                int RowIndex = gvr.RowIndex; 

                //int index = Convert.ToInt32(e.CommandArgument);
                //GridViewRow row = gvSMSDetails.Rows[index];

                HiddenField hdnEevntName = (HiddenField)gvr.FindControl("hdnEevntName");
                int CustomizedSMSId = common.myInt(e.CommandArgument);

                ViewState["EevntName"] = hdnEevntName.Value;
                ViewState["CustomizedSMSId"] = CustomizedSMSId;

               
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private string DeActivateSMS(int EventId, string EventName)
    {
        BaseC.SmsClass clsSMS=new BaseC.SmsClass(sConString); 
        try
        {

            string strresponse = clsSMS.DeleteSMSSetup(EventId, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), EventName, common.myInt(Session["UserId"]));
            return strresponse.ToString();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return "";
        }
    }
    protected void gvFacility_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSMSDetails.PageIndex = e.NewPageIndex;
            BindFacilityGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvFacility_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvSMSDetails.EditIndex = e.NewEditIndex;
            BindFacilityGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void UpdateLocation_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Session["HospitalLocationID"] != null && Session["UserID"] != null)
            {
                if (gvSMSDetails.SelectedRow != null)
                {
                    HshIn = new Hashtable();
                    HshOut = new Hashtable();

            

                  
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


    protected void gvSMSDetails_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnpasshelp_Click(object sender, EventArgs e)
    {
        try
        {
           BaseC.SmsClass sms=new BaseC.SmsClass(sConString);
            DataSet ds=new DataSet();

            ds = sms.GetSMSKeywords();

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblFacilityKeyWord.Text = common.myStr(ds.Tables[0].Rows[0]["Keyword"]);
                lblFacilityKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[0]["Description"]);
                lblFacilityNameMaxLength.Text = common.myStr(ds.Tables[0].Rows[0]["MaxLength"]);

                lblClinicPhoneKeyWord.Text = common.myStr(ds.Tables[0].Rows[1]["Keyword"]);
                lblClinicPhoneKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[1]["Description"]);
                lblClinicPhoneMaxLength.Text = common.myStr(ds.Tables[0].Rows[1]["MaxLength"]);

                lblDrNameKeyWord.Text = common.myStr(ds.Tables[0].Rows[2]["Keyword"]);
                lblDrNameKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[2]["Description"]);
                lblDrNameMaxLength.Text = common.myStr(ds.Tables[0].Rows[2]["MaxLength"]);

                lblDateKeyWord.Text = common.myStr(ds.Tables[0].Rows[3]["Keyword"]);
                lblDateKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[3]["Description"]);
                lblDateMaxLength.Text = common.myStr(ds.Tables[0].Rows[3]["MaxLength"]);

                lblTimeKeyWord.Text = common.myStr(ds.Tables[0].Rows[4]["Keyword"]);
                lblTimeKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[4]["Description"]);
                lblTimeMaxLength.Text = common.myStr(ds.Tables[0].Rows[4]["MaxLength"]);

                lblPatNameKeyWord.Text = common.myStr(ds.Tables[0].Rows[5]["Keyword"]);
                lblPatNameKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[5]["Description"]);
                lblPatNameMaxLength.Text = common.myStr(ds.Tables[0].Rows[5]["MaxLength"]);


                lblRegNoKeyWord.Text = common.myStr(ds.Tables[0].Rows[6]["Keyword"]);
                lblRegNoKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[6]["Description"]);
                lblRegNoMaxLength.Text = common.myStr(ds.Tables[0].Rows[6]["MaxLength"]);

                lblApmtCountKeyWord.Text = common.myStr(ds.Tables[0].Rows[7]["Keyword"]);
                lblApmtCountKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[7]["Description"]);
                lblApmtCountMaxLength.Text = common.myStr(ds.Tables[0].Rows[7]["MaxLength"]);

                lblAptListKeyWord.Text = common.myStr(ds.Tables[0].Rows[8]["Keyword"]);
                lblAptListKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[8]["Description"]);
                lblAptListMaxLength.Text = common.myStr(ds.Tables[0].Rows[8]["MaxLength"]);

                lblVaccinesKeyWord.Text = common.myStr(ds.Tables[0].Rows[9]["Keyword"]);
                lblVaccinesKeyWordDescription.Text = common.myStr(ds.Tables[0].Rows[9]["Description"]);
                lblVaccinesMaxLength.Text = common.myStr(ds.Tables[0].Rows[9]["MaxLength"]);


            }


            divtooltip.Visible = true;
            //ViewState["pwd"] = txtnewpassword.Text.Trim();
            //txtnewpassword.Attributes.Add("value", ViewState["pwd"].ToString());
            //txtConfirmPassword.Attributes.Add("value", ViewState["pwd"].ToString());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ButtonOk_OnClick(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR clemr = new BaseC.clsEMR(sConString);
        string StrDelMessage = string.Empty;
        string FormularyType = string.Empty;
        BaseC.EMRProblems objbc2 = new BaseC.EMRProblems(sConString);

        string strDeleteResponse = DeActivateSMS(common.myInt(ViewState["CustomizedSMSId"]), common.myStr(ViewState["EevntName"]));
        if (strDeleteResponse == "Record Deleted!")
        {
            //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
            lblMessage.Text = "Record Deleted!";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            BindFacilityGrid();
        }

        else
        {
            //Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
            lblMessage.Text = "Error In De-Activation...";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

        }

        StrDelMessage = clemr.DeleteFavoriteDrugs(common.myInt(Session["DoctorID"]), common.myInt(ViewState["ItemIdforDelete"]), 
                            FormularyType, common.myInt(Session["UserId"]), 0);

     
        dvConfirmCancelOptions.Visible = false;
       ViewState["EevntName"] ="";
       ViewState["CustomizedSMSId"] = 0;
    }
    protected void ButtonCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancelOptions.Visible = false;
    }

    protected void btntooltipshow_OnClick(object sender, EventArgs e)
    {
        divtooltip.Visible = false;
    }
}
