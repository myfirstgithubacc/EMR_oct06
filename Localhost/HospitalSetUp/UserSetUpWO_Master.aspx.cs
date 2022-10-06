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
using System.Text;
using System.Security.Cryptography;

public partial class HospitalSetUp_UserSetUpWO_Master : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData Parse = new BaseC.ParseData();


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //string Name = "Niraj Kumar Mishra";


            //    Response.Redirect("/HospitalSetUp/WelcomeNote.aspx?pname=" + Name, false);
            //    return;



            lblMessage.Text = "";
            if (!IsPostBack)
            {
                //btnSave.Enabled = false;
                chkAccept.Enabled = true;
                chkAccept.Checked = false;
                PopulateControl();
                PopulateBillingControl();
                bindTimeZone();
                //ddlCountry.SelectedIndex = 1;
                ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue("1"));
                //ddlCountry.SelectedItem.Text = "USA";
                ddlbillingcountery.SelectedIndex = ddlbillingcountery.Items.IndexOf(ddlbillingcountery.Items.FindByValue("1"));
                BindState();
                BindBillingState();
                bindPOS();
                BindTitle();
                BindEmployeeType();
                if (Request.QueryString["Tr"] == null)
                {
                    if (Session["HospitalLocationID"] != null)
                    {
                        //////getHospitalDetails();
                        //////btnSave.Enabled = true;
                        //////trnewuser.Visible = false;

                        //////tableUserDetails.Visible = false;
                        //////trposcode.Visible = false;

                        //////tbltimezonehd.Visible = false;
                        //////tbltimezone.Visible = false;
                        //////lnkResetPassword.Visible = false;
                    }
                }
                BindddlEmploymentstatus();
                ddlEmploymentstatus.SelectedIndex = ddlEmploymentstatus.Items.IndexOf(ddlEmploymentstatus.Items.FindByValue("12"));
                ddlmonth.SelectedIndex = ddlmonth.Items.IndexOf(ddlmonth.Items.FindByValue("12"));
                ddlmonth_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void bindPOS()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "Select * From GetBillingCodes('FacilityTypeCodes')");
            ddlPOS.DataSource = dr;
            ddlPOS.DataTextField = "Codedesc";
            ddlPOS.DataValueField = "Code";
            ddlPOS.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindTimeZone()
    {
        try
        {
            BaseC.Patient objBc = new BaseC.Patient(sConString);

            ddlTimeZone.DataSource = objBc.GetTimeZones("");
            ddlTimeZone.DataValueField = "ID";
            ddlTimeZone.DataTextField = "Name";
            ddlTimeZone.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void BindddlEmploymentstatus()
    {
        try
        {
            DAL.DAL dL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = dL.FillDataSet(CommandType.Text, "SELECT StatusId, Status FROM StatusMaster WHERE StatusType = 'Employment' AND Code IN ('EFT','EPT') AND Active = 1 ORDER BY SequenceNo");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlEmploymentstatus.DataSource = ds;
                ddlEmploymentstatus.DataValueField = "StatusId";
                ddlEmploymentstatus.DataTextField = "Status";
                ddlEmploymentstatus.DataBind();

            }
            ddlEmploymentstatus.Items.Insert(0, "Select");
            ddlEmploymentstatus.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindEmployeeType()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //populate Kin Relation drop down control
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT ID,Description FROM EmployeeType");
            ddlemployeetype.DataSource = dr;
            ddlemployeetype.DataTextField = "Description";
            ddlemployeetype.DataValueField = "ID";
            ddlemployeetype.DataBind();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindTitle()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "Select 0 As TitleId,'[Select]' As Name Union All Select TitleId,Name From titlemaster Where Active = 1 Order By Name");
            ddltitle.DataSource = dr;
            ddltitle.DataTextField = "Name";
            ddltitle.DataValueField = "TitleId";
            ddltitle.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateControl()
    {
        try
        {
            BaseC.Patient objBc = new BaseC.Patient(sConString);
            if (Cache["CountryTable"] == null)
            {
                DataSet objDs = (DataSet)objBc.GetPatientCountry();
                Cache.Insert("CountryTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                ddlCountry.DataSource = (DataSet)Cache["CountryTable"];
                ddlCountry.DataTextField = "CountryName";
                ddlCountry.DataValueField = "CountryId";
                ddlCountry.DataBind();
            }
            else
            {
                DataSet objDs = (DataSet)Cache["CountryTable"];
                ddlCountry.DataSource = objDs;
                ddlCountry.DataTextField = "CountryName";
                ddlCountry.DataValueField = "CountryId";
                ddlCountry.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlCountry_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            //populate State drop down control
            ddlState.Items.Clear();

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + ddlCountry.SelectedValue.ToString() + "' and Active = 1 ORDER BY StateName");
            ddlState.DataSource = dr;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateID";
            ddlState.DataBind();
            dr.Close();
            ddlState.Items.Insert(0, "[Select]");
            ddlState.Items[0].Value = "0";
            ddlState_OnSelectedIndexChanged(sender, e);
            ddlState.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindState()
    {
        try
        {
            ddlState.Items.Clear();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + ddlCountry.SelectedValue.ToString() + "' and Active = 1 ORDER BY StateName");
            ddlState.DataSource = dr;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateID";
            ddlState.DataBind();
            dr.Close();
            ddlState.Items.Insert(0, "[Select]");
            ddlState.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlState_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            //populate City drop down control
            ddlCity.Items.Clear();

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname from CityMaster where StateId='" + ddlState.SelectedValue.ToString() + "' And Active = 1 ORDER BY CityMaster.cityname");
            ddlCity.DataSource = dr;
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityID";
            ddlCity.DataBind();
            dr.Close();
            ddlCity.Items.Insert(0, "[Select]");
            ddlCity.Items[0].Value = "0";
            //  ddlCity_OnSelectedIndexChanged(sender, e);
            ddlCity.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlCity_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ddlZip.Items.Clear();
            //populate Local City drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@CityId", ddlCity.SelectedValue);
            DataSet dsZip = dl.FillDataSet(CommandType.Text, "select ZIPID, ZipCode from zipmaster where CityID=@CityId ORDER BY ZipCode", hshIn);
            if (dsZip.Tables[0].Rows.Count > 0)
            {
                ddlZip.DataSource = dsZip;
                ddlZip.DataTextField = "ZipCode";
                ddlZip.DataValueField = "ZIPID";
                ddlZip.DataBind();
            }
            else
            {
                ddlZip.Items.Insert(0, new ListItem("[Select]", "0", true));
            }
            // ddlZip.Items.Insert(0,new ListItem("[Select]", "0",true));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void chkAccept_OnCheckedChanged(Object sender, EventArgs e)
    {
        btnSave.Enabled = chkAccept.Checked;
    }

    private void ClearDate()
    {
        try
        {
            txtAccountName.Text = "";
            txtNPI.Text = "";
            txtEIN.Text = "";
            txtMainNo.Text = "";
            txtFaxNo.Text = "";
            txtAddress.Text = "";
            txtAddress2.Text = "";
            ddlCountry.Items.Clear();
            //ddlCountry.Items.Insert(0, "[Select]");
            //ddlCountry.Items[0].Value = "0";
            ddlState.Items.Clear();
            ddlState.Items.Insert(0, "[Select]");
            ddlState.Items[0].Value = "0";
            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, "[Select]");
            ddlCity.Items[0].Value = "0";
            ddlZip.Items.Clear();
            ddlZip.Items.Insert(0, "[Select]");
            ddlZip.Items[0].Value = "0";

            ddlbillingcountery.Items.Clear();
            //ddlCountry.Items.Insert(0, "[Select]");
            //ddlCountry.Items[0].Value = "0";
            ddlbillingstate.Items.Clear();
            ddlbillingstate.Items.Insert(0, "[Select]");
            ddlbillingstate.Items[0].Value = "0";
            ddlbillingcity.Items.Clear();
            ddlbillingcity.Items.Insert(0, "[Select]");
            ddlCity.Items[0].Value = "0";
            ddlbillingzip.Items.Clear();
            ddlbillingzip.Items.Insert(0, "[Select]");
            ddlbillingzip.Items[0].Value = "0";
            ddlPOS.SelectedIndex = 0;

            chkAccept.Checked = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void Save_OnClick(Object sender, EventArgs e)
    {

        if (checkAvailability() != null)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "";
            lblMessage.Text = "User Name Not Available!";
        }
        else
        {
            SaveData();
        }


    }

    private void SaveData()
    {
        try
        {
            int month = Convert.ToInt16(ddlmonth.SelectedValue);
            int date = Convert.ToInt16(ddldate.SelectedItem.Text);
            DateTime endofyear = Convert.ToDateTime(month + "/" + date + "/" + 1900);
            BaseC.HospitalSetup HS = new BaseC.HospitalSetup(sConString);
            BaseC.EncryptDecrypt eN = new BaseC.EncryptDecrypt();
            Int16 iTrial = 0;
            Int16 iTrialDays = 0;

            if (Request.QueryString["Tr"] != null)
            {
                iTrial = 1;
                iTrialDays = 30;
            }
            else
            {
                iTrial = 0;
                iTrialDays = 0;
            }
            HS.AccountName = Parse.ParseQ(txtAccountName.Text.ToString().Trim());
            HS.NPI = Parse.ParseQ(txtNPI.Text.ToString().Trim());
            HS.EIN = Parse.ParseQ(txtEIN.Text.ToString().Trim());

            String strMainNo = "";
            if (txtMainNo.Text.ToString().Trim() == "___-___-____")
            {
                strMainNo = "";
            }
            else
            {
                strMainNo = txtMainNo.Text.ToString().Trim();
            }

            String strFaxNo = "";
            if (txtFaxNo.Text.ToString().Trim() == "___-___-____")
            {
                strFaxNo = "";
            }
            else
            {
                strFaxNo = txtFaxNo.Text.ToString().Trim();
            }

            HS.MainNo = Parse.ParseQ(strMainNo.Trim());
            HS.FaxNo = Parse.ParseQ(strFaxNo.Trim());

            HS.Address = Parse.ParseQ(txtAddress.Text.ToString().Trim());
            HS.Address2 = Parse.ParseQ(txtAddress2.Text.ToString().Trim());
            HS.Country = Convert.ToInt32(ddlCountry.SelectedValue);
            HS.State = Convert.ToInt32(ddlState.SelectedValue);
            HS.City = Convert.ToInt32(ddlCity.SelectedValue);
            HS.Zip = ddlZip.SelectedItem.Text;

            HS.BillingAddress = txtBillingaddress.Text.Trim();
            HS.BillingAddress2 = txtBillingaddress2.Text.Trim();
            if (ddlbillingcountery.SelectedValue != "0")
                HS.BillingCountry = Convert.ToInt32(ddlbillingcountery.SelectedValue);
            if (ddlbillingstate.SelectedValue != "0")
                HS.BillingState = Convert.ToInt32(ddlbillingstate.SelectedValue);
            if (ddlbillingcity.SelectedValue != "" && ddlbillingcity.SelectedValue != "0")
                HS.Billingcity = Convert.ToInt32(ddlbillingcity.SelectedValue);
            if (ddlbillingzip.SelectedValue != "0")
                HS.Billingzip = ddlbillingzip.SelectedValue;
            HS.BillingPhone = txtBillingphone.Text;
            HS.Endofyear = endofyear;
            HS.TrialVersion = iTrial;
            HS.TrialDays = iTrialDays;
            HS.EmploymentStatus = Convert.ToInt32(ddlEmploymentstatus.SelectedValue);
            HS.EncodedBy = Convert.ToInt16(Session["UserID"]);
            //////if (Session["HospitalLocationID"] == null)
            //if (Session["HospitalLocationID"] != null)
            //{
                HS.EmpNo = "";
                if (ddltitle.SelectedValue != "0")
                {
                    HS.TitleId = Convert.ToInt16(ddltitle.SelectedValue);
                }
                HS.EmpType = Convert.ToInt16(ddlemployeetype.SelectedValue);
                HS.FirstName = Parse.ParseQ(txtfirstname.Text.Trim());
                HS.MiddleName = Parse.ParseQ(txtmiddlename.Text.Trim());
                HS.LastName = Parse.ParseQ(txtlastname.Text.Trim());
                HS.POS = Convert.ToInt32(ddlPOS.SelectedValue);
                HS.TimeZone = ddlTimeZone.SelectedValue;

                BaseC.Patient objBc = new BaseC.Patient(sConString);
                BaseC.Patient.MyStruct objMyStruct = objBc.GetTimeZones(ddlTimeZone.SelectedValue)[0];
                HS.OffSetMinutes = (int)objMyStruct.TimeZoneMinutes;

                //DAL.DAL dL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //Hashtable hshInput = new Hashtable();
                //hshInput.Add("UserName", eN.Encrypt(txtusername.Text.Trim(), eN.getKey(sConString), true, null));
                //SqlDataReader objDr = (SqlDataReader)dL.ExecuteReader(CommandType.Text, "SELECT UserName, PrivateKey FROM Users u INNER JOIN UserPrivateKey up ON u.ID = up.UserId WHERE UserName = @UserName", hshInput);
                //string sUserName = "", sPrivateKey = "";

                //if (objDr.Read())
                //{
                //    sUserName = objDr["UserName"].ToString();
                //    sPrivateKey = objDr["PrivateKey"].ToString();
                //    bPrivateKey = UTF8Encoding.UTF8.GetBytes(sPrivateKey);
                //}
                //else
                //{

                //}

                string sPrivateKey = System.Guid.NewGuid().ToString();
                HS.PrivateKey = sPrivateKey;
                HS.Username = eN.Encrypt(Parse.ParseQ(txtusername.Text.Trim()), eN.getKey(sConString), true, "");
                HS.Password = eN.Encrypt(Parse.ParseQ(txtpassword.Text.Trim()), eN.getKey(sConString), true, sPrivateKey);


                String strWorkNo = "";
                if (txtworkPhone.Text.ToString().Trim() == "___-___-____")
                {
                    strWorkNo = "";
                }
                else
                {
                    strWorkNo = txtworkPhone.Text.ToString().Trim();
                }

                String strMobileNo = "";
                if (txtmobileno.Text.ToString().Trim() == "___-___-____")
                {
                    strMobileNo = "";
                }
                else
                {
                    strMobileNo = txtmobileno.Text.ToString().Trim();
                }

                HS.Work = Parse.ParseQ(strWorkNo.Trim());
                HS.Mobile = Parse.ParseQ(strMobileNo.Trim());
                HS.Mail = Parse.ParseQ(txtemailid.Text.Trim());
                //saving....
                String strMessage = HS.saveHospitalSetUp();
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = strMessage;

                String Name = "";
                if (ddltitle.SelectedIndex > 0)
                {
                    Name = ddltitle.Text;
                }
                if (txtfirstname.Text.Trim().Length > 0)
                {
                    Name = Name + txtfirstname.Text;
                }
                if (txtmiddlename.Text.Trim().Length > 0)
                {
                    Name = Name + ' ' + txtmiddlename.Text;
                }
                if (txtlastname.Text.Trim().Length > 0)
                {
                    Name = Name + ' ' + txtlastname.Text;
                }

                //Response.Redirect("/HospitalSetUp/WelcomeNote.aspx?pname=" + Name, false);

                //  Response.Redirect("/Login.aspx");
            //}
            //else
            //{
            //    HS.HospitalLocationID = Convert.ToInt16(Session["HospitalLocationID"]);
            //    String strMessage = HS.UpdateHospitalSetUp();
            //    lblMessage.ForeColor = System.Drawing.Color.Green;
            //    lblMessage.Text = strMessage;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void getHospitalDetails()
    {
        try
        {
            BaseC.HospitalSetup HS = new BaseC.HospitalSetup(sConString);
            HS.HospitalLocationID = Convert.ToInt16(Session["HospitalLocationID"]);
            HS.idFacility = Convert.ToInt16(Session["FacilityId"]);
            DataSet ds = (DataSet)HS.getHospitalSetUp();
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAccountName.Text = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                txtEIN.Text = Convert.ToString(ds.Tables[0].Rows[0]["EIN"]);
                txtNPI.Text = Convert.ToString(ds.Tables[0].Rows[0]["NPI"]);
                txtMainNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["Phone"]);
                txtFaxNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["Fax"]);
                txtAddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["Address"]);
                txtAddress2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Address2"]);
                ddlCountry.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                ddlCountry_OnSelectedIndexChanged(this, null);
                ddlState.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["StateId"]);
                ddlState_OnSelectedIndexChanged(this, null);
                ddlCity.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["CityId"]);
                ddlCity_OnSelectedIndexChanged(this, null);
                if (Convert.ToString(ddlZip.SelectedItem.Text) != null)
                {
                    ddlZip.SelectedItem.Text = Convert.ToString(ds.Tables[0].Rows[0]["PinNo"]);
                }
                //ddlPOS.SelectedIndex=ddlPOS.Items.IndexOf(ddlPOS.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["PinNo"])));
                txtBillingaddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["BillingAddress1"]);
                txtBillingaddress2.Text = Convert.ToString(ds.Tables[0].Rows[0]["BillingAddress2"]);
                ddlbillingcountery.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["BillingCountryId"]);
                ddlbillingcountery_OnSelectedIndexChanged(this, null);
                ddlbillingstate.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["BillingStateId"]);
                ddlbillingstate_OnSelectedIndexChanged(this, null);
                ddlbillingcity.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["BillingCityId"]);
                ddlbillingcity_OnSelectedIndexChanged(this, null);
                if (ds.Tables[0].Rows[0]["BillingPinNo"].ToString() != "" && ds.Tables[0].Rows[0]["BillingPinNo"].ToString() != " " && ds.Tables[0].Rows[0]["BillingPinNo"].ToString() != null)
                {
                    ddlbillingzip.SelectedItem.Text = Convert.ToString(ds.Tables[0].Rows[0]["BillingPinNo"]);
                }
                txtBillingphone.Text = Convert.ToString(ds.Tables[0].Rows[0]["BillingPhone"]);
                if (ds.Tables[0].Rows[0]["Endofyeardate"].ToString() != null || ds.Tables[0].Rows[0]["Endofyeardate"].ToString() != "")
                {
                    string strendofyeardt = ds.Tables[0].Rows[0]["Endofyeardate"].ToString();
                    if (strendofyeardt != "")
                    {
                        ddlmonth.SelectedValue = strendofyeardt.Substring(0, 2).ToString();
                        ddldate.SelectedItem.Text = strendofyeardt.Substring(3, 2);
                    }
                }
                chkAccept.Checked = true;
                chkAccept.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected string checkAvailability()
    {
        try
        {
            BaseC.ParseData objParse = new BaseC.ParseData();
            BaseC.User objUser = new BaseC.User(sConString);
            string sUserName = objParse.ParseQ(txtusername.Text);
            string i = objUser.GetUserID(sUserName);
            return i;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return "";
        }
    }

    protected void btnAvailability_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["pwd"] = txtpassword.Text.Trim();
            if (checkAvailability() != null)
            {
                lblAvailabilityMessage.Text = "User Name Not Available!";
                lblAvailabilityMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblAvailabilityMessage.Text = "User Name Available!";
                lblAvailabilityMessage.ForeColor = System.Drawing.Color.Green;
            }
            txtpassword.Attributes.Add("value", ViewState["pwd"].ToString());
            txtConfirmPassword.Attributes.Add("value", ViewState["pwd"].ToString());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlmonth.SelectedValue == "01" || ddlmonth.SelectedValue == "03" || ddlmonth.SelectedValue == "05" || ddlmonth.SelectedValue == "07" || ddlmonth.SelectedValue == "08" || ddlmonth.SelectedValue == "10" || ddlmonth.SelectedValue == "12")
            {
                ddldate.Items.Clear();
                //for (int i = 1; i <= 31; i++)
                //{
                int i = 31;
                ddldate.Items.Add(i.ToString());
                ddldate.DataBind();
                //}
            }
            else if (ddlmonth.SelectedValue == "04" || ddlmonth.SelectedValue == "06" || ddlmonth.SelectedValue == "09" || ddlmonth.SelectedValue == "11")
            {
                ddldate.Items.Clear();
                //for (int i = 1; i <= 30; i++)
                //{
                int i = 30;
                ddldate.Items.Add(i.ToString());
                ddldate.DataBind();
                //}
            }
            else
            {
                ddldate.Items.Clear();
                //for (int i = 1; i <= 29; i++)
                //{
                int i = 29;
                ddldate.Items.Add(i.ToString());
                ddldate.DataBind();
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

    protected void btnpasshelp_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["pwd"] = txtpassword.Text.Trim();
            txtpassword.Attributes.Add("value", ViewState["pwd"].ToString());
            txtConfirmPassword.Attributes.Add("value", ViewState["pwd"].ToString());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateBillingControl()
    {
        try
        {
            BaseC.Patient objBc = new BaseC.Patient(sConString);
            if (Cache["CountryTable"] == null)
            {
                DataSet objDs = (DataSet)objBc.GetPatientCountry();
                Cache.Insert("CountryTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                ddlbillingcountery.DataSource = (DataSet)Cache["CountryTable"];
                ddlbillingcountery.DataTextField = "CountryName";
                ddlbillingcountery.DataValueField = "CountryId";
                ddlbillingcountery.DataBind();
            }
            else
            {
                DataSet objDs = (DataSet)Cache["CountryTable"];
                ddlbillingcountery.DataSource = objDs;
                ddlbillingcountery.DataTextField = "CountryName";
                ddlbillingcountery.DataValueField = "CountryId";
                ddlbillingcountery.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlbillingcountery_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlbillingstate.Items.Clear();

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + ddlbillingcountery.SelectedValue.ToString() + "' and Active = 1 ORDER BY StateName");
            ddlbillingstate.DataSource = dr;
            ddlbillingstate.DataTextField = "StateName";
            ddlbillingstate.DataValueField = "StateID";
            ddlbillingstate.DataBind();
            dr.Close();
            ddlbillingstate.Items.Insert(0, "[Select]");
            ddlbillingstate.Items[0].Value = "0";
            ddlbillingstate_OnSelectedIndexChanged(sender, e);
            ddlbillingstate.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindBillingState()
    {
        try
        {
            ddlbillingstate.Items.Clear();

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + ddlbillingcountery.SelectedValue.ToString() + "' and Active = 1 ORDER BY StateName");
            ddlbillingstate.DataSource = dr;
            ddlbillingstate.DataTextField = "StateName";
            ddlbillingstate.DataValueField = "StateID";
            ddlbillingstate.DataBind();
            dr.Close();
            ddlbillingstate.Items.Insert(0, "[Select]");
            ddlbillingstate.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlbillingstate_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlbillingcity.Items.Clear();

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dscity = dl.FillDataSet(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname from CityMaster where StateId='" + ddlbillingstate.SelectedValue.ToString() + "' And Active = 1 ORDER BY CityMaster.cityname");

            if (dscity.Tables[0].Rows.Count > 0)
            {
                ddlbillingcity.DataSource = dscity;
                ddlbillingcity.DataTextField = "CityName";
                ddlbillingcity.DataValueField = "CityID";
                ddlbillingcity.DataBind();
            }
            ddlbillingcity.Items.Insert(0, "[Select]");
            ddlbillingcity.Items[0].Value = "0";
            //  ddlCity_OnSelectedIndexChanged(sender, e);
            ddlbillingcity.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlbillingcity_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlbillingzip.Items.Clear();
            //populate Local City drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@CityId", ddlbillingcity.SelectedValue);
            DataSet dsZip = dl.FillDataSet(CommandType.Text, "select ZIPID, ZipCode from zipmaster where CityID=@CityId ORDER BY ZipCode", hshIn);
            if (dsZip.Tables[0].Rows.Count > 0)
            {
                ddlbillingzip.DataSource = dsZip;
                ddlbillingzip.DataTextField = "ZipCode";
                ddlbillingzip.DataValueField = "ZIPID";
                ddlbillingzip.DataBind();
            }
            else
            {
                ddlbillingzip.Items.Insert(0, new ListItem("[Select]", "0", true));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkResetPassword_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("ResetPassword.aspx", false);
    }
}
