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

public partial class MPages_SetDefaultMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
   
    private const string DefaultCountry = "DefaultCountry";
    private const string DefaultState = "DefaultState";
    private const string DefaultCity = "DefaultCity";
    private const string DefaultZip = "DefaultZip";

    private const string DefaultCompany = "DefaultCompany";
    private const string DefaultReligion = "DefaultReligion";
    private const string DefaultFontType = "DefaultFontType";
    private const string DefaultFontSize = "DefaultFontSize";
    private const string DefaultPageSize = "DefaultPageSize";
    private const string DefaultNationality = "DefaultNationality";
    private const string DefaultLanguage = "DefaultLanguage";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
          

            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            }

            if (!IsPostBack)
            {
                GetFonts();
                fillNationality();
                fillLanguage();
                //btnUpdate.Visible = false;
                //btnNew.Visible = false;
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
               
                SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT countryid,countryname FROM countrymaster Where Active = 1 order by countryname");
                ddlcountryname.DataSource = dr;
                ddlcountryname.DataTextField = "countryname";
                ddlcountryname.DataValueField = "countryid";
                ddlcountryname.DataBind();
                dr.Close();
              

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet objDsReligion = objDl.FillDataSet(CommandType.Text, "select Id, Name from ReligionMaster where Active = 1 Order By Name");
                ddlDefaultReligion.DataSource = objDsReligion;
                ddlDefaultReligion.DataTextField = "Name";
                ddlDefaultReligion.DataValueField = "Id";
                ddlDefaultReligion.DataBind();

                if (Session["HospitalLocationId"] != null)
                {
                    ClinicDefaults cd = new ClinicDefaults(Page);
                    DataTable objDs = (DataTable)cd.GetHospitalDefaults(Session["HospitalLocationId"].ToString());
                    gv.DataSource = objDs;
                    gv.DataBind();

                    ddlDefaultNationality.SelectedIndex = ddlDefaultNationality.Items.IndexOf(ddlDefaultNationality.Items.FindByValue(cd.GetHospitalDefaults(DefaultNationality, Session["HospitalLocationId"].ToString())));
                    ddlDefaultLanguage.SelectedIndex = ddlDefaultLanguage.Items.IndexOf(ddlDefaultLanguage.Items.FindByValue(cd.GetHospitalDefaults(DefaultLanguage, Session["HospitalLocationId"].ToString())));
                    ddlcountryname.SelectedIndex = ddlcountryname.Items.IndexOf(ddlcountryname.Items.FindByValue(cd.GetHospitalDefaults(DefaultCountry, Session["HospitalLocationId"].ToString())));
                    LocalCountry_SelectedIndexChanged(sender, e);
                    dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindByValue(cd.GetHospitalDefaults(DefaultState, Session["HospitalLocationId"].ToString())));
                    LocalState_SelectedIndexChanged(sender, e);

                    dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindByValue(cd.GetHospitalDefaults(DefaultCity, Session["HospitalLocationId"].ToString())));
                    LocalCity_OnSelectedIndexChanged(sender, e);

                    ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindByValue(cd.GetHospitalDefaults(DefaultZip, Session["HospitalLocationId"].ToString())));
                    //ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(cd.GetHospitalDefaults(DefaultCompany, Session["HospitalLocationId"].ToString())));
                    ddlDefaultReligion.SelectedIndex = ddlDefaultReligion.Items.IndexOf(ddlDefaultReligion.Items.FindByValue(cd.GetHospitalDefaults(DefaultReligion, Session["HospitalLocationId"].ToString())));

                    ddlfonttype.SelectedIndex = ddlfonttype.Items.IndexOf(ddlfonttype.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontType, Session["HospitalLocationId"].ToString())));
                    ddlfontsize.SelectedIndex = ddlfontsize.Items.IndexOf(ddlfontsize.Items.FindByValue(cd.GetHospitalDefaults(DefaultFontSize, Session["HospitalLocationId"].ToString())));
                    ddlpagesize.SelectedIndex = ddlpagesize.Items.IndexOf(ddlpagesize.Items.FindByValue(cd.GetHospitalDefaults(DefaultPageSize, Session["HospitalLocationId"].ToString())));
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

    private void GetFonts()
    {
        try
        {
            BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
            DataSet ds = fonts.GetFontDetails("Size");
            DataSet dsFontName = fonts.GetFontDetails("Name");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlfontsize.Items.Clear();
                ddlfontsize.DataSource = ds;
                ddlfontsize.DataTextField = "FontSize";
                ddlfontsize.DataValueField = "FontSize";
                ddlfontsize.DataBind();
            }
            ddlfontsize.Items.Insert(0, "Select");
            ddlfontsize.Items[0].Value = "0";
            if (dsFontName.Tables[0].Rows.Count > 0)
            {
                ddlfonttype.Items.Clear();
                ddlfonttype.DataSource = dsFontName;
                ddlfonttype.DataTextField = "FontName";
                ddlfonttype.DataValueField = "FontId";
                ddlfonttype.DataBind();
            }
            ddlfonttype.Items.Insert(0, "Select");
            ddlfonttype.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void LocalCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dropLState.Items.Clear();
            //populate Local State drop down control
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName from StateMaster where StateMaster.CountryID='" + ddlcountryname.SelectedValue.ToString() + "' and Active=1 ORDER BY StateName");
            dropLState.DataSource = dr;
            dropLState.DataTextField = "StateName";
            dropLState.DataValueField = "StateID";
            dropLState.DataBind();
            dr.Close();
            dropLState.Items.Insert(0, "Select");
            dropLState.Items[0].Value = "0";
            dropLState.Focus();

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
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname from CityMaster where StateId='" + dropLState.SelectedValue.ToString() + "' and Active=1 ORDER BY CityMaster.cityname");
            dropLCity.DataSource = dr;
            dropLCity.DataTextField = "CityName";
            dropLCity.DataValueField = "CityID";
            dropLCity.DataBind();
            dr.Close();
            dropLCity.Items.Insert(0, "Select");
            dropLCity.Items[0].Value = "0";

            LocalCity_OnSelectedIndexChanged(sender, e);
            dropLCity.Focus();
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
            ddlZip.Items.Insert(0, "Select");
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

    private void deleteIfBlank(string fieldKey, string HospitalLocationId)
    {
        try
        {
            if (HospitalLocationId != null)
            {
                ClinicDefaults cd = new ClinicDefaults(Page);
                cd.DeleteKey(fieldKey, HospitalLocationId);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void saveData(ClinicDefaults cd, DropDownList ddlCtrl, string keyField, string HospitalLocationId)
    {
        try
        {
            if (ddlCtrl.SelectedIndex > 0 && ddlCtrl.SelectedItem != null)
            {
                if (cd.GetHospitalDefaults(keyField, HospitalLocationId) == "")
                {
                    cd.StoreValue(HospitalLocationId, keyField, ddlCtrl.SelectedItem.Text, ddlCtrl.SelectedValue);
                }
                else
                {
                    cd.UpdateKey(keyField, HospitalLocationId, ddlCtrl.SelectedItem.Text, ddlCtrl.SelectedValue);
                }
            }
            else
            {
                deleteIfBlank(keyField, HospitalLocationId);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_OnClick(Object sender, EventArgs e)
    {
        ClinicDefaults cd = new ClinicDefaults(Page);
        try
        {
            string HospitalLocationId = Convert.ToString(Session["HospitalLocationId"]);

            saveData(cd, ddlcountryname, DefaultCountry, HospitalLocationId);
            saveData(cd, dropLState, DefaultState, HospitalLocationId);
            saveData(cd, dropLCity, DefaultCity, HospitalLocationId);
            saveData(cd, ddlZip, DefaultZip, HospitalLocationId);
            //saveData(cd, ddlcompany, DefaultCompany, HospitalLocationId);
            saveData(cd, ddlDefaultReligion, DefaultReligion, HospitalLocationId);
            saveData(cd, ddlfonttype, DefaultFontType, HospitalLocationId);
            saveData(cd, ddlfontsize, DefaultFontSize, HospitalLocationId);
            saveData(cd, ddlpagesize, DefaultPageSize, HospitalLocationId);
            saveData(cd, ddlDefaultNationality, DefaultNationality, HospitalLocationId);
            saveData(cd, ddlDefaultLanguage, DefaultLanguage, HospitalLocationId);
            Cache.Remove("ClinicDefaults");

            lblMessage.Text = " Record Saved!";

            DataTable objDs = (DataTable)cd.GetHospitalDefaults(Session["HospitalLocationId"].ToString());
            gv.DataSource = objDs;
            gv.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
        }
    }

    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete1")
            {
                if (Session["HospitalLocationId"] != null)
                {
                    ClinicDefaults cd = new ClinicDefaults(Page);
                    cd.DeleteKey(e.CommandArgument.ToString(), Convert.ToString(Session["HospitalLocationId"]));

                    DataSet objDs = (DataSet)cd.GetHospitalDefaults();
                    DataView dv = new DataView();
                    dv = objDs.Tables[0].DefaultView;

                    dv.RowFilter = "HospitalLocationId = '" + Convert.ToString(Session["HospitalLocationId"]) + "' ";

                    gv.DataSource = dv;
                    gv.DataBind();
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

    public void fillNationality()
    {
        //populate Nationality drop down control
        if (Cache["NationalityTable"] == null)
        {
            BaseC.Patient objBc = new BaseC.Patient(sConString);
            DataSet objDs = (DataSet)objBc.GetPatientNationality();
            Cache.Insert("NationalityTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            ddlDefaultNationality.DataSource = (DataSet)Cache["NationalityTable"];
            ddlDefaultNationality.DataTextField = "name";
            ddlDefaultNationality.DataValueField = "ID";
            ddlDefaultNationality.DataBind();
            //dropNationality.Items.Insert(0, "[Select]");
            //dropNationality.Items[0].Value = "";
        }
        else
        {
            DataSet objDs = (DataSet)Cache["NationalityTable"];
            ddlDefaultNationality.DataSource = objDs;
            ddlDefaultNationality.DataTextField = "name";
            ddlDefaultNationality.DataValueField = "ID";
            ddlDefaultNationality.DataBind();
            //dropNationality.Items.Insert(0, "[Select]");
            //dropNationality.Items[0].Value = "";
        }
    }

    public void fillLanguage()
    {
        BaseC.Patient objBc = new BaseC.Patient(sConString);
       DataSet ds = (DataSet)objBc.GetLanguage();
        ddlDefaultLanguage.DataSource = ds;
        ddlDefaultLanguage.DataTextField = "Language";
        ddlDefaultLanguage.DataValueField = "LanguageID";
        ddlDefaultLanguage.DataBind();
    }
}