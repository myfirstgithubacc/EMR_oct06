using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Text;
using Telerik.Web.UI;

public partial class MPages_DoctorTheatreTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GetTheatreData();
                fillDoctor();
                fillDoctorTheatreTagging();

                ddlWeekDay.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem { Text = "Sunday", Value = "1" });
                ddlWeekDay.Items.Insert(1, new Telerik.Web.UI.RadComboBoxItem { Text = "Monday", Value = "2" });
                ddlWeekDay.Items.Insert(2, new Telerik.Web.UI.RadComboBoxItem { Text = "Tuesday", Value = "3" });
                ddlWeekDay.Items.Insert(3, new Telerik.Web.UI.RadComboBoxItem { Text = "Wednesday", Value = "4" });
                ddlWeekDay.Items.Insert(4, new Telerik.Web.UI.RadComboBoxItem { Text = "Thursday", Value = "5" });
                ddlWeekDay.Items.Insert(5, new Telerik.Web.UI.RadComboBoxItem { Text = "Friday", Value = "6" });
                ddlWeekDay.Items.Insert(6, new Telerik.Web.UI.RadComboBoxItem { Text = "Saturday", Value = "7" });
           }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void GetTheatreData()
    {
        try
        {

            DataSet ds = new DataSet();
            BaseC.RestFulAPI objwcfOt = new BaseC.RestFulAPI(sConString);
            ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

            if (ds.Tables.Count > 0)
            {
                DataView dv = new DataView(ds.Tables[0]);
                //  dv.RowFilter = "Active=1";

                ddlTheatre.DataSource = dv.ToTable();
                ddlTheatre.DataTextField = "TheatreName";
                ddlTheatre.DataValueField = "TheatreID";
                ddlTheatre.DataBind();
                common.CheckAllItems(ddlTheatre);
            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void fillDoctor()
    {
        try
        {
            BaseC.Hospital bHos = new BaseC.Hospital(sConString);
            DataSet ds = bHos.GetDoctorDepartment(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDoctor.DataSource = ds.Tables[0];
                    ddlDoctor.DataTextField = "EmployeeName";
                    ddlDoctor.DataValueField = "EmployeeNo";
                    ddlDoctor.DataBind();
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
    protected void fillDoctorTheatreTagging()
    {
        try
        {
            BaseC.Hospital bHos = new BaseC.Hospital(sConString);
            DataSet ds = bHos.GetOTDoctorTagDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvDoctor.DataSource = ds.Tables[0];
                    gvDoctor.DataBind();
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
    protected void lnkSelect_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkSelect = (LinkButton)sender;
            ddlTheatre.Text = "";
            ddlDoctor.Text = "";
            ddlWeekDay.Text = "";
            ddlDoctor.SelectedIndex = ddlDoctor.Items.IndexOf(ddlDoctor.Items.FindItemByValue(((HiddenField)lnkSelect.FindControl("hdnDoctorId")).Value));
            ddlTheatre.SelectedIndex = ddlTheatre.Items.IndexOf(ddlTheatre.Items.FindItemByValue(((HiddenField)lnkSelect.FindControl("hdnTheatreId")).Value));
            ddlWeekDay.SelectedIndex = ddlWeekDay.Items.IndexOf(ddlWeekDay.Items.FindItemByValue(((HiddenField)lnkSelect.FindControl("hdnWeekDay")).Value));

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlDoctor.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Doctor Details.", Page.Page);
                return;
            }

            if (common.myInt(ddlTheatre.SelectedValue) == -1)
            {
                Alert.ShowAjaxMsg("Please! Select Theatre.", Page.Page);
                return;
            }

            if (common.myInt(ddlWeekDay.SelectedValue) == -1)
            {
                Alert.ShowAjaxMsg("Please! Select a WeekDay.", Page.Page);
                return;
            }
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            StringBuilder xmlWeekDayIds = new StringBuilder();
            string strWeekDayIds = common.GetCheckedItems(ddlWeekDay);
            string[] arWeekDayIds = strWeekDayIds.Split(',');

            ArrayList col1 = new ArrayList();
            foreach (string Id in arWeekDayIds)
            {
                if (Id != "")
                {
                    col1.Add(common.myInt(Id)); 
                    xmlWeekDayIds.Append(common.setXmlTable(ref col1));
                }
            }

            Hashtable HashIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HashIn.Add("@intHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            HashIn.Add("@intFacilityID", common.myInt(Session["FacilityId"]));
            HashIn.Add("@SEmployeeNo", common.myStr(ddlDoctor.SelectedValue));
            HashIn.Add("@xmlOTWeekDayIds", xmlWeekDayIds.ToString());
            HashIn.Add("@intOTTheaterId", ddlTheatre.SelectedValue);
            HashIn.Add("@intEncodedBy", common.myInt(Session["UserID"]));           
            HshOut.Add("@chvOutput", SqlDbType.VarChar);

            HshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDoctorWeekDayOTTagging", HashIn, HshOut);

            if (common.myStr(HshOut["@chvOutput"]).Contains(" saved") || common.myStr(HshOut["@chvOutput"]).Contains(" update"))
            {
                lblMessage.Text = HshOut["@chvOutput"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
            else
            {
                lblMessage.Text = HshOut["@chvOutput"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            }

            ddlDoctor.SelectedIndex = -1;
            ddlWeekDay.SelectedIndex = -1;
            ddlTheatre.SelectedIndex = -1;
            fillDoctorTheatreTagging();
            Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
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
}
