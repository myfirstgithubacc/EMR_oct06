using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Globalization;
using Telerik.Web.UI;
using System.Text.RegularExpressions;

public partial class MPages_ReferDoctorMaster : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    BaseC.EMRMasters objbc;
    clsExceptionLog objException = new clsExceptionLog();
    private const string DefaultCountry = "DefaultCountry";
    private const string DefaultState = "DefaultState";
    private const string DefaultCity = "DefaultCity";
    private const string DefaultZip = "DefaultZip";
    protected void Page_Load(object sender, EventArgs e)
    {
        lblmsg.Text = string.Empty;
        if (!IsPostBack)
        {
            BindRefType();
            txtName.Focus();
            if (common.myStr(Request.QueryString["ReferedId"]) == "H")
            {
                Page.Title = "Refered By Hospital";
            }
            else
            {
                Page.Title = "Refered By Doctor";
            }
            BindCountory();
            BindRegion();
            fillgrid();
            ViewState["cmd"] = "1";

        }
    }


    public void fillgrid()
    {

        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        string strquery = "";

        if (ddlReferType.SelectedValue == "")
        {
            if (common.myStr(ViewState["ReferType"]) != "")
            {
                ddlReferType.SelectedValue =common.myStr(ViewState["ReferType"]);
            
            }
        
        }

      //  ViewState["ReferType"]

        if (txtSearchName.Text.Trim() == "")
        {
            strquery = "select id,name from ReferDoctorMaster where ReferralTypeId='" + ddlReferType.SelectedValue + "' And FacilityId='" + common.myInt(Session["FacilityId"]) + "' order by name";
        }
        else
        {
            strquery = "select id,name from ReferDoctorMaster where ReferralTypeId='" + ddlReferType.SelectedValue + "'  And FacilityId='" + common.myInt(Session["FacilityId"]) + "'  and name like '%" + txtSearchName.Text.Trim() + "%' order by name";

        }


        ds = dl.FillDataSet(CommandType.Text,strquery);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvReferDoctor.DataSource = ds.Tables[0];
            gvReferDoctor.DataBind();

        }
        else
        {
            gvReferDoctor.DataSource = null;
            gvReferDoctor.DataBind();
            gvReferDoctor.EmptyDataText = "No Data Found"; 
            
        }

    }

    protected void ddlcountry_TextChanged(object sender, EventArgs e)
    {

        fillstate();

    }

    void fillstate()
    {
        try
        {

            if (ddlcountry.SelectedValue != "")
            {
                ddlstate.Items.Clear();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //populate Bed Category drop down control
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@countryid", Convert.ToInt32(ddlcountry.SelectedValue));
                // dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT stateid,statename FROM statemaster where countryid=@countryid And Active=1 order by StateName", hshIn);
                DataSet ds = new DataSet();
                ds = dl.FillDataSet(CommandType.Text, "SELECT stateid,statename FROM statemaster where countryid=@countryid And Active=1 order by StateName", hshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlstate.DataSource = ds;
                    ddlstate.DataTextField = "statename";
                    ddlstate.DataValueField = "stateid";
                    ddlstate.DataBind();
                }

                ddlstate.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlstate.Items[0].Value = "0";
              
            }
        }
        catch (Exception Ex)
        {

            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlstate_TextChanged(object sender, EventArgs e)
    {

        fillcity();
    }
    void fillcity()
    {
        try
        {
            if (ddlstate.SelectedValue != "")
            {
                ddlcity.Items.Clear();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //populate Bed Category drop down control
                Hashtable hshIn = new Hashtable();
                if (ddlstate.SelectedValue != "")
                {
                    hshIn.Add("@stateid", Convert.ToInt32(ddlstate.SelectedValue));
                    dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT cityid,cityname FROM citymaster where stateid=@stateid and active=1", hshIn);
                    if (dr.HasRows)
                    {
                        ddlcity.DataSource = dr;

                        ddlcity.DataTextField = "cityname";
                        ddlcity.DataValueField = "cityid";

                        ddlcity.DataBind();
                    }
                    dr.Close();

                    //ddlcity.Items.Insert(0, "Select");
                    //ddlcity.Items[0].Value = "0";
                    ddlcity.Items.Insert(0, new RadComboBoxItem("", "0"));
                    ddlcity.SelectedIndex = 0;
                }



            }
        }
        catch (Exception Ex)
        {

            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }


    protected void ddlcity_TextChanged(object sender, EventArgs e)
    {
        try
        {
            /*if (ddlcity.SelectedValue != "")
            {
                ddlZip.Items.Clear();
                ddlZip.SelectedValue = null;

                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ZIPID, ZipCode from zipmaster where CityID='" + ddlcity.SelectedValue.ToString() + "' ORDER BY ZipCode");
                ddlZip.DataSource = dr;
                ddlZip.DataTextField = "ZipCode";
                ddlZip.DataValueField = "ZIPID";
                ddlZip.DataBind();
                dr.Close();
                //ListItem L = new ListItem("Select", "0");
                //ddlZip.Items.Insert(0, L);
                ddlZip.Items.Insert(0, new RadComboBoxItem("", "0"));
                ddlZip.Text = 0;


                ddlZip.Focus();
            }*/
        }
        catch (Exception Ex)
        {

            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    private bool check_validation()
    {

        if (txtName.Text.Trim() == "")
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Please enter name !!";
            return false ;
        }

        if (txtphonehome.Text.Trim() == "")
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Please enter phone no. !!";
            return false;
        }

        //if (chkrelease_unsettled.Checked == false)
        //{
        //    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblmsg.Text = "Please select the check box !!";
        //    return false;
        //}

        if (txtEmail.Text.Trim() != "")
        {
            bool isEmail = Regex.IsMatch(txtEmail.Text.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

            if (!isEmail)
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblmsg.Text = "Please provide valid email id !!";
                return false;

            }

        }
        
        return true; 
    }

    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!check_validation())
            {
                return; 
            }

            int chkvalue,cmdvalue,idvalue,intactive;

            if (chkrelease_unsettled.Checked == true)
                chkvalue = 1;
            else
                chkvalue = 0;


            if (ddlActive.SelectedIndex == 0)
                intactive = 1;
            else
                intactive = 0;




            //if (ViewState["cmd"].ToString() == "2")
            //{
            //    cmdvalue = 2;
            //    idvalue = Convert.ToInt32(ViewState["id"].ToString());
            //}
            //else
            //{
            //    cmdvalue = 1;
            //    idvalue = 0;
            //}


            if (btnSave.Text == "Update")
            {
                cmdvalue = 2;
                idvalue = Convert.ToInt32(ViewState["id"].ToString());

                
               idvalue=common.myInt(hdnId.Value);
            }
            else
            {
                cmdvalue = 1;
                idvalue = 0;
            }
            ViewState["ReferType"] = ddlReferType.SelectedValue;
            objbc = new BaseC.EMRMasters(sConString);
            string str = objbc.SaveReferedbyDoctor(
                common.myInt(Session["HospitalLocationID"]),
                common.myStr(ddlReferType.SelectedValue),
                common.myStr(txtName.Text),
                common.myStr(txtAddress.Text),
                common.myInt(ddlcity.SelectedValue),
                common.myInt(ddlstate.SelectedValue),
                common.myInt(ddlcountry.SelectedValue),
                common.myStr(txtZip.Text),
                common.myStr(txtphonehome.Text),
                common.myStr(txtmobile.Text),
                common.myStr(txtfax.Text),
                common.myStr(txtEmail.Text),
                common.myStr(txtlicenceno.Text),
                common.myInt(ddlRegion.SelectedValue),
                chkvalue,
                common.myInt(Session["UserID"]),
                common.myInt(Session["FacilityID"]), idvalue, cmdvalue, intactive);

            if (str == "Data Saved..")
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblmsg.Text = "Data Saved !";

                btnSave.Text = "Save";

                ClearData();
                BindRefType();
                fillgrid();
                

            }
            else if (str == "Data Updated..")
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblmsg.Text = str;

                btnSave.Text = "Save";

                ClearData();
                BindRefType();
                fillgrid();
            }
            else
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblmsg.Text = str;
                ClearData();
                BindRefType();
                fillgrid();
            
            }

        }
        catch(Exception Ex)
          {
              lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
              lblmsg.Text = "Error: " + Ex.Message;
              objException.HandleException(Ex);

       }
    }

    protected void BindCountory()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        ds = dl.FillDataSet(CommandType.Text, "SELECT countryid,countryname FROM countrymaster where active=1 order by countryname ");
        ddlcountry.Items.Clear();  
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcountry.DataSource = ds;
            ddlcountry.DataTextField = "countryname";
            ddlcountry.DataValueField = "countryid";
            ddlcountry.DataBind();
            ddlcountry.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlcountry.Items[0].Value = "0";
        }
    }

    protected void BindRegion()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        ds = dl.FillDataSet(CommandType.Text, "SELECT id,RegionName FROM DAReferRegionMaster where active=1 order by RegionName ");
        ddlRegion.Items.Clear(); 
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlRegion.DataSource = ds;
            ddlRegion.DataTextField = "RegionName";
            ddlRegion.DataValueField = "id";
            ddlRegion.DataBind();
            ddlRegion.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlRegion.Items[0].Value = "0";
        }
    }


    void ClearData()
    {
      //  ddlReferType.SelectedIndex = 0;  
        txtName.Text = "";
        txtAddress.Text = "";
        ddlcity.SelectedIndex = -1;
        ddlstate.SelectedIndex = -1;
        ddlcountry.SelectedIndex = -1;
        txtZip.Text = "";
        txtphonehome.Text = "";
        txtmobile.Text = "";
        txtfax.Text = "";
        txtEmail.Text = "";
        ddlReferType.SelectedIndex =-1;
        txtlicenceno.Text = "";
        chkrelease_unsettled.Checked = false;
        ddlActive.SelectedIndex = 0;
   
    }

    protected void gvReferDoctor_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand';this.style.backgroundColor='#e5eeff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white'");
        }
    }
    protected void gvReferDoctor_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gvReferDoctor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "view")
        {

            //string currentCommand = e.CommandName;
            ////Get the Selected RowIndex
            //int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            ////Get the GridViewRow from Current Row Index
            //GridViewRow row = gvReferDoctor.Rows[currentRowIndex];
            ////Assign the Back Color to Yellow
            ////Change this color as per your need
            //row.BackColor = System.Drawing.Color.Yellow;






            lblmsg.Text = "";  
            ViewState["cmd"] = "2";
            ViewState["id"] = e.CommandArgument;

            btnSave.Text = "Update";
            hdnId.Value =common.myStr(e.CommandArgument);

           
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, "select id,ReferralTypeId,Name,Address,City,State,Country,Pin,Phone,Mobile,Fax,Email,LicenseId,RefRegionId,ReleaseUnsettledBillFee,Active from ReferDoctorMaster where id=" + ViewState["id"].ToString());

            ddlReferType.SelectedValue = ds.Tables[0].Rows[0]["ReferralTypeId"].ToString();
            txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
            txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
            BindCountory(); 
            ddlcountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
            fillstate(); 
            ddlstate.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
            fillcity(); 
            ddlcity.SelectedValue = ds.Tables[0].Rows[0]["City"].ToString();
            txtZip.Text = ds.Tables[0].Rows[0]["Pin"].ToString();
            txtphonehome.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
            txtmobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
            txtfax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
            txtlicenceno.Text = ds.Tables[0].Rows[0]["LicenseId"].ToString();
            BindRegion();
            ddlRegion.SelectedValue = ds.Tables[0].Rows[0]["RefRegionId"].ToString();

            if (ds.Tables[0].Rows[0]["ReleaseUnsettledBillFee"].ToString() == "True")
            {
                chkrelease_unsettled.Checked  = true;
            }
            else
            {
                chkrelease_unsettled.Checked = false;
                
            }        

            if (ds.Tables[0].Rows[0]["Active"].ToString() == "True")
            {
                ddlActive.SelectedValue = "1";

            }
            else
            {
                ddlActive.SelectedValue = "0";

            }


        }
    }
    protected void gvReferDoctor_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReferDoctor.PageIndex = e.NewPageIndex;
        fillgrid();
    }
    protected void ddlReferType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        fillgrid();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), false);
    }

    protected void ibtnRegion_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/PRegistration/Status.aspx?CtrlDesp=Region";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 550;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        //RadWindowForNew.OnClientClose = "BindCombo";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        fillgrid();
    }


    private void BindRefType()
    {
        try
        {
            ddlReferType.Items.Clear();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet dsReferal = dl.FillDataSet(CommandType.StoredProcedure, "uspGetReferby");



            foreach (DataRowView drReferal in dsReferal.Tables[0].DefaultView)
            {
                string strrefreralname = (string)drReferal["ReferralType"];
                if (strrefreralname == "C" || strrefreralname == "D" || strrefreralname == "H")
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)drReferal["ReferralTypeName"];
                    item.Value = drReferal["ReferralType"].ToString();
                    item.Attributes.Add("Id", common.myStr(drReferal["Id"]));
                    ddlReferType.Items.Add(item);
                    item.DataBind();
                }

            }
            RadComboBoxItem rci = new RadComboBoxItem("Select");
            ddlReferType.Items.Insert(0, rci);
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
