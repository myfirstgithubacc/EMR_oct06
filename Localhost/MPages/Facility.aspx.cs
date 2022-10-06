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

public partial class EMR_Masters_Facility : System.Web.UI.Page
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
                BindFacilityGrid();
                populatecountry();
                dropLCountry.SelectedIndex = 1;

                GetState();
                bindPOS();
                bindTimeZone();
                GetMainFacility();
                //LocalCountry_SelectedIndexChanged(sender, e);
                btnUpdate.Visible = false;

                if (Session["HospitalLocationId"] != null)
                {
                    ClinicDefaults cd = new ClinicDefaults(Page);
                    dropLCountry.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByValue(cd.GetHospitalDefaults(DefaultCountry, Session["HospitalLocationId"].ToString())));
                    LocalCountry_SelectedIndexChanged(sender, e);
                    dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindByValue(cd.GetHospitalDefaults(DefaultState, Session["HospitalLocationId"].ToString())));
                    LocalState_SelectedIndexChanged(sender, e);

                    dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindByValue(cd.GetHospitalDefaults(DefaultCity, Session["HospitalLocationId"].ToString())));
                    LocalCity_OnSelectedIndexChanged(sender, e);

                    ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindByValue(cd.GetHospitalDefaults(DefaultZip, Session["HospitalLocationId"].ToString())));
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
    void bindPOS()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "Select * From GetBillingCodes('FacilityTypeCodes')");
            //ddlPOS.DataSource = dr;
            //ddlPOS.DataTextField = "Codedesc";
            //ddlPOS.DataValueField = "Code";
            //ddlPOS.DataBind();
            dr.Close();
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
    void populatecountry()
    {
        try
        {
            Cache.Remove("CountryTable");
            BaseC.Patient objBc = new BaseC.Patient(sConString);
            if (Cache["CountryTable"] == null)
            {
                DataSet objDs = objBc.GetPatientCountry();
                Cache.Insert("CountryTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                dropLCountry.DataSource = Cache["CountryTable"];
                dropLCountry.DataTextField = "CountryName";
                dropLCountry.DataValueField = "CountryId";
                dropLCountry.DataBind();
            }
            else
            {
                DataSet objDs = (DataSet)Cache["CountryTable"];
                dropLCountry.DataSource = objDs;
                dropLCountry.DataTextField = "CountryName";
                dropLCountry.DataValueField = "CountryId";
                dropLCountry.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    //public void GetCity()
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    DataSet objDs = dl.FillDataSet(CommandType.Text, "select distinct CityMaster.cityID, CityMaster.cityname from CityMaster ORDER BY CityMaster.cityname");
    //    ddlCity.DataSource = objDs;
    //    ddlCity.DataTextField = "CityName";
    //    ddlCity.DataValueField = "cityID";
    //    ddlCity.DataBind();
    //}
    public void GetState()
    {
        try
        {
            dropLState.Items.Clear();
            //populate Local State drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + dropLCountry.SelectedValue.ToString() + "' ORDER BY StateName");
            dropLState.DataSource = dr;
            dropLState.DataTextField = "StateName";
            dropLState.DataValueField = "StateID";
            dropLState.DataBind();
            dr.Close();
            //dropLState.Items.Insert(0, "[Select]");
            //dropLState.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    //public void GetCountry()
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    DataSet objDs = dl.FillDataSet(CommandType.Text, "SELECT distinct CountryMaster.CountryId, CountryMaster.CountryName FROM CountryMaster ORDER BY CountryName");
    //    ddlCountry.DataSource = objDs;
    //    ddlCountry.DataTextField = "CountryName";
    //    ddlCountry.DataValueField = "CountryID";
    //    ddlCountry.DataBind();
    //}
    private DataSet Facility(int iFacilityID)
    {
        DataSet ds = new DataSet();
        try
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //HshIn.Add("@inyUserId", Convert.ToInt16(Session["UserId"]));
            HshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationId"]));

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFacilityMaster", HshIn);
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
    protected void LocalCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dropLState.Items.Clear();
            //populate Local State drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + dropLCountry.SelectedValue.ToString() + "' ORDER BY StateName");
            dropLState.DataSource = dr;
            dropLState.DataTextField = "StateName";
            dropLState.DataValueField = "StateID";
            dropLState.DataBind();
            dr.Close();
            dropLState.Items.Insert(0, "[Select]");
            dropLState.Items[0].Value = "0";
            LocalState_SelectedIndexChanged(sender, e);
            dropLState.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void LocalState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dropLCity.Items.Clear();
            //populate Local City drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname from CityMaster where StateId='" + dropLState.SelectedValue.ToString() + "' ORDER BY CityMaster.cityname");
            dropLCity.DataSource = dr;
            dropLCity.DataTextField = "CityName";
            dropLCity.DataValueField = "CityID";
            dropLCity.DataBind();
            dr.Close();
            dropLCity.Items.Insert(0, "[Select]");
            dropLCity.Items[0].Value = "0";
            dropLCity.Focus();
            LocalCity_OnSelectedIndexChanged(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
        //    gvFacility.DataSource = dtAllergy;
        //    gvFacility.DataBind();
    }
    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/EMR/Masters/Facility.aspx", false);
    }
    protected void SaveFacility_OnClick(Object sender, EventArgs e)
    {
        SaveFacilityMaster();
        //Alert.ShowAjaxMsg(str, this.Page);
        GetMainFacility();
        BindFacilityGrid();

    }
    private void BindFacilityGrid()
    {
        try
        {
            DataSet ds = Facility(0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvFacility.DataSource = ds;
                gvFacility.DataBind();
            }
            else
            {
                ViewState["Blank"] = "True";
                BindBlankGrid();
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
    private void clearControl()
    {
        try
        {
            txtFacilityName.Text = "";
            txtAddress.Text = "";
            txtAddress2.Text = "";
            txtNPI.Text = "";
            txtTelephone.Text = "";
            txtFax.Text = "";
            dropLCountry.SelectedIndex = 0;
            dropLState.SelectedIndex = 0;
            dropLCity.SelectedIndex = 0;
            ddlTimeZone.SelectedIndex = 0;
            ddlZip.SelectedIndex = 0;
            // ddlPOS.SelectedIndex = 0;
            ddlReportto.SelectedIndex = 0;
            chkmain.Checked = false;
            txtLinkServer.Text = "";
            txtSmsSender.Text = "";
            txtCaseNoteFolder.Text = "";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public void SaveFacilityMaster()
    {
        try
        {
            if (Session["HospitalLocationID"] != null && Session["UserID"] != null)
            {
                if (ddlTimeZone.SelectedValue == "0")
                {
                    Alert.ShowAjaxMsg("Select Time Zone! ", Page);
                    return;
                }

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("inyHospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
                HshIn.Add("chvName", bc.ParseQ(txtFacilityName.Text.Trim()));
                HshIn.Add("chvNPI", bc.ParseQ(txtNPI.Text.Trim()));

                if (txtTelephone.Text != "___-___-____")
                    HshIn.Add("chvPhone", bc.ParseQ(txtTelephone.Text.Trim()));
                else
                    HshIn.Add("chvPhone", "");

                if (txtFax.Text != "___-___-____")
                    HshIn.Add("chvFax", bc.ParseQ(txtFax.Text.Trim()));
                else
                    HshIn.Add("chvFax", "");

                HshIn.Add("chvAddress1", bc.ParseQ(txtAddress.Text.Trim()));
                HshIn.Add("chvAddress2", bc.ParseQ(txtAddress2.Text.Trim()));
                if (dropLCity.SelectedValue != "0")
                {
                    HshIn.Add("inyCityId", bc.ParseQ(dropLCity.SelectedValue));
                }
                if (dropLState.SelectedValue != "0" && dropLState.SelectedValue != "")
                {
                    HshIn.Add("inyStateId", bc.ParseQ(dropLState.SelectedValue));
                }
                if (dropLCountry.SelectedValue != "0")
                {
                    HshIn.Add("inyCountryId", bc.ParseQ(dropLCountry.SelectedValue));
                }
                if (ddlZip.SelectedValue != "0")
                {
                    HshIn.Add("chvPinNo", bc.ParseQ(ddlZip.SelectedItem.Text.Trim()));
                }
                HshIn.Add("intEncodedBy", Convert.ToInt32(Session["UserID"]));
                //if (ddlPOS.SelectedValue != "0")
                //{
                //    HshIn.Add("chvPOSCode", ddlPOS.SelectedValue);
                //}
                HshIn.Add("chvTimeZoneId", ddlTimeZone.SelectedValue);
                HshIn.Add("intReportToFacility", ddlReportto.SelectedValue);
                HshIn.Add("btMainFacility", chkmain.Checked);
                BaseC.Patient objBc = new BaseC.Patient(sConString);
                BaseC.Patient.MyStruct objMyStruct = objBc.GetTimeZones(ddlTimeZone.SelectedValue)[0];
                HshIn.Add("intOffSetMinutes", (int)objMyStruct.TimeZoneMinutes);
                HshIn.Add("vSmsSender", common.myStr(txtSmsSender.Text));
                HshIn.Add("vSmsServer", common.myStr(txtLinkServer.Text));
                HshIn.Add("vCaseNoteAdd", common.myStr(txtCaseNoteFolder.Text));
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                // int i = (int)objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRSaveFacilityMaster", HshIn, HshOut);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveFacilityMaster", HshIn, HshOut);

                clearControl();
                lblMessage.Text = "Record(s) Has Been Saved...";
                //Alert.ShowAjaxMsg("Record(s) Has Been Saved...!", Page);
                //Response.Redirect("Facility.aspx");
            }
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
            dropLCountry.ClearSelection();
            dropLState.ClearSelection();
            dropLCity.ClearSelection();
            //ddlZip.ClearSelection();

            //HshIn = new Hashtable();
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //HshIn.Add("@intfacilityId", Convert.ToInt16(gvFacility.SelectedDataKey.Value));
            //DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFacilityMaster1", HshIn);

            //txtFacilityName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
            //txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
            //txtAddress2.Text = ds.Tables[0].Rows[0]["Address2"].ToString();
            //txtNPI.Text = ds.Tables[0].Rows[0]["NPI"].ToString();
            //txtTelephone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
            //txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();

            //dropLCountry.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByText (ds.Tables[0].Rows[0]["CountryId"].ToString()));
            //LocalCountry_SelectedIndexChanged(sender, e);

            //dropLState.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByText(ds.Tables[0].Rows[0]["StateId"].ToString()));

            //LocalState_SelectedIndexChanged(sender, e);
            //dropLCity.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByText(ds.Tables[0].Rows[0]["CityId"].ToString()));


            //LocalCity_OnSelectedIndexChanged(sender, e);

            //ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindByText(ds.Tables[0].Rows[0]["PinNo"].ToString())); //gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.PinNo)].Text;


            //ddlPOS.ClearSelection();
            //ddlPOS.SelectedIndex = ddlPOS.Items.IndexOf(ddlPOS.Items.FindByText(ds.Tables[0].Rows[0]["POSCode"].ToString()));
            //ddlEntityId.ClearSelection();
            //if (objDs.Tables[0].Rows[0]["Name"] != null)

            txtFacilityName.Text = gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.Name)].Text;  //objDs.Tables[0].Rows[0]["Name"].ToString();

            if (gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.Address)].Text == "&nbsp;")
            {
                txtAddress.Text = "";
            }
            else
            {
                txtAddress.Text = gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.Address)].Text; //objDs.Tables[0].Rows[0]["Address"].ToString();
            }
            if (gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.Address2)].Text == "&nbsp;")
            {
                txtAddress2.Text = "";
            }
            else
            {
                txtAddress2.Text = gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.Address2)].Text.Trim(); //objDs.Tables[0].Rows[0]["Address2"].ToString();
            }
            //if (objDs.Tables[0].Rows[0]["NPI"].ToString().Trim() != "")
            txtNPI.Text = HttpUtility.HtmlDecode(gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.NPI)].Text); //objDs.Tables[0].Rows[0]["NPI"].ToString();
            //if (objDs.Tables[0].Rows[0]["Phone"].ToString().Trim() != "")
            txtTelephone.Text = gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.Phone)].Text; //objDs.Tables[0].Rows[0]["Phone"].ToString();
            //if (objDs.Tables[0].Rows[0]["Fax"].ToString().Trim() != "")
            txtFax.Text = gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.Fax)].Text.Trim();//objDs.Tables[0].Rows[0]["Fax"].ToString();

            ddlTimeZone.SelectedIndex = ddlTimeZone.Items.IndexOf(ddlTimeZone.Items.FindByValue(gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.TimeZoneId)].Text));

            dropLCountry.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByValue(gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.CountryId)].Text));
            LocalCountry_SelectedIndexChanged(sender, e);

            dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindByValue(gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.StateId)].Text));
            LocalState_SelectedIndexChanged(sender, e);

            dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindByValue(gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.CityId)].Text));
            LocalCity_OnSelectedIndexChanged(sender, e);

            //objDs.Tables[0].Rows[0]["PinNo"].ToString();
            ddlZip.ClearSelection();
            ListItem lstddlZip = (ListItem)ddlZip.Items.FindByText(HttpUtility.HtmlDecode(gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.PinNo)].Text));
            if (lstddlZip != null)
            {
                lstddlZip.Selected = true;
            }

            ddlReportto.SelectedIndex = ddlReportto.Items.IndexOf(ddlReportto.Items.FindByValue(gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.ReferToFacility)].Text));
            if (gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.MainFacility)].Text == "False")
            {
                chkmain.Checked = false;
            }
            else
            {
                chkmain.Checked = true;
            }

            if (((HiddenField)gvFacility.SelectedRow.FindControl("hdnActive")).Value == "False" || ((HiddenField)gvFacility.SelectedRow.FindControl("hdnActive")).Value == "0")
            {
                chkActive.Checked = false;
            }
            else
            {
                chkActive.Checked = true;
            }

            txtSmsSender.Text = ((Label)gvFacility.SelectedRow.FindControl("lblSmsSender")).Text.Trim();
            txtLinkServer.Text = ((Label)gvFacility.SelectedRow.FindControl("lblSmsServer")).Text.Trim();
            txtCaseNoteFolder.Text = ((Label)gvFacility.SelectedRow.FindControl("lblCaseNote")).Text.Trim();

            //ddlPOS.ClearSelection();
            //ListItem lstddlPos = (ListItem)ddlPOS.Items.FindByValue(HttpUtility.HtmlDecode(gvFacility.SelectedRow.Cells[Convert.ToByte(Gridfacility.PosId)].Text));
            //     if (lstddlPos != null)
            //   {
            //       lstddlPos.Selected = true;
            //    }


            btnSave.Visible = false;
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
            ddlZip.Items.Clear();
            //populate Local City drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ZIPID, ZipCode from zipmaster where CityID='" + dropLCity.SelectedValue.ToString() + "' ORDER BY ZipCode");
            ddlZip.DataSource = dr;
            ddlZip.DataTextField = "ZipCode";
            ddlZip.DataValueField = "ZIPID";
            ddlZip.DataBind();
            dr.Close();
            ddlZip.Items.Insert(0, "[Select]");
            ddlZip.Items[0].Value = "0";
            ddlZip.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvFacility_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[Convert.ToByte(Gridfacility.FacilityID)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.CityId)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.StateId)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.CountryId)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.PosId)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.FacilityID)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.Address)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.NPI)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.CityName)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.CountryName)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.StateName)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.PinNo)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.Address2)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.TimeZoneId)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.TimeZoneOffSetMinutes)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.ReferToFacility)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gridfacility.MainFacility)].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='Pink';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';");
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvFacility, "Select$" + e.Row.RowIndex);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvFacility_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeActivate")
            {
                Int32 i = DeActivateFacilityName(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(e.CommandArgument), Convert.ToInt16(Session["UserID"]));
                if (i == 0)
                {
                    //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
                    lblMessage.Text = "Record De-Actived... ";
                    BindFacilityGrid();
                }
                else if (i == 1)
                {
                    lblMessage.Text = "Can Not De-Actived When Only One Facility Exist!";
                    BindFacilityGrid();
                }
                else
                {
                    //Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
                    lblMessage.Text = "Error In De-Activation...";
                    gvFacility.EditIndex = -1;
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
    private Int32 DeActivateFacilityName(Int16 iHospID, Int32 iFacilityID, Int32 iUserID)
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            HshIn.Add("@HospID", iHospID);
            HshIn.Add("@FacilityID", iFacilityID);
            ds = objDl.FillDataSet(CommandType.Text, "select HospitalLocationID from FacilityMaster where HospitalLocationID=@HospID and Active=1 GROUP BY HospitalLocationID  HAVING COUNT(HospitalLocationID )>1", HshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE FacilityMaster SET Active = '0' WHERE FacilityID=@FacilityID and  HospitalLocationID=@HospID", HshIn);
                return i;
            }
            else
            {
                Int32 i;
                i = 1;
                return i;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return 0;
        }
    }
    protected void gvFacility_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvFacility.PageIndex = e.NewPageIndex;
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
            gvFacility.EditIndex = e.NewEditIndex;
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
                if (gvFacility.SelectedRow != null)
                {
                    HshIn = new Hashtable();
                    HshOut = new Hashtable();

                    HshIn.Add("intFacilityID", gvFacility.SelectedRow.Cells[0].Text);
                    HshIn.Add("chvName", bc.ParseQ(txtFacilityName.Text.Trim()));
                    HshIn.Add("chvNPI", bc.ParseQ(txtNPI.Text.Trim()));

                    if (txtTelephone.Text != "___-___-____")
                        HshIn.Add("chvPhone", bc.ParseQ(txtTelephone.Text.Trim()));
                    else
                        HshIn.Add("chvPhone", "");

                    if (txtFax.Text != "___-___-____")
                        HshIn.Add("chvFax", bc.ParseQ(txtFax.Text.Trim()));
                    else
                        HshIn.Add("chvFax", "");
                    //HshIn.Add("chvPhone", bc.ParseQ(txtTelephone.Text.Trim()));
                    //HshIn.Add("chvFax", bc.ParseQ(txtFax.Text.Trim()));
                    HshIn.Add("chvAddress1", bc.ParseQ(txtAddress.Text.Trim()));
                    HshIn.Add("chvAddress2", bc.ParseQ(txtAddress2.Text.Trim()));
                    HshIn.Add("intEncodedBy", Session["UserID"].ToString());
                    if (dropLCity.SelectedValue != "0")
                        HshIn.Add("inyCityId", bc.ParseQ(dropLCity.SelectedValue));
                    if (dropLState.SelectedValue != "0")
                        HshIn.Add("inyStateId", bc.ParseQ(dropLState.SelectedValue));
                    if (dropLCountry.SelectedValue != "0")
                        HshIn.Add("inyCountryId", bc.ParseQ(dropLCountry.SelectedValue));
                    if (ddlZip.SelectedValue != "0")
                        HshIn.Add("chvPinNo", bc.ParseQ(ddlZip.SelectedItem.Text.Trim()));
                    //if (ddlPOS.SelectedValue != "0")
                    //    HshIn.Add("chvPOSCode", ddlPOS.SelectedValue);

                    HshIn.Add("chvTimeZoneId", ddlTimeZone.SelectedValue);
                    BaseC.Patient objBc = new BaseC.Patient(sConString);
                    BaseC.Patient.MyStruct objMyStruct = objBc.GetTimeZones(ddlTimeZone.SelectedValue)[0];
                    HshIn.Add("intOffSetMinutes", (int)objMyStruct.TimeZoneMinutes);
                    HshIn.Add("inyHospitalLocationId", Convert.ToInt32(Session["HospitalLocationID"]));

                    HshIn.Add("intReportToFacility", ddlReportto.SelectedValue);
                    HshIn.Add("btMainFacility", chkmain.Checked);
                    HshIn.Add("@intActive", chkActive.Checked);
                    HshIn.Add("vSmsSender", common.myStr(txtSmsSender.Text));
                    HshIn.Add("vSmsServer", common.myStr(txtLinkServer.Text));
                    HshIn.Add("vCaseNoteAdd", common.myStr(txtCaseNoteFolder.Text));
                    HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRUpdateFacilityMaster", HshIn, HshOut);

                    clearControl();

                    lblMessage.Text = "Record Updated!";
                    BindFacilityGrid();
                    btnSave.Visible = true;
                    btnUpdate.Visible = false;
                    //Response.Redirect("Facility.aspx");
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

    protected void GetMainFacility()
    {
        try
        {
            DAL.DAL objdl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            ds = objdl.FillDataSet(CommandType.Text, "SELECT FacilityID,Name  FROM FacilityMaster WHERE HospitalLocationID=" + common.myInt(Session["HospitalLocationId"]) + " AND MainFacility=1 AND Active =1 ");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlReportto.DataSource = ds;
                ddlReportto.DataTextField = "Name";
                ddlReportto.DataValueField = "FacilityId";
                ddlReportto.DataBind();
            }
            ddlReportto.Items.Insert(0, "[Select]");
            ddlReportto.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
