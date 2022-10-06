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
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class EMR_Dashboard_PDashboard : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ddlProviders.SelectedIndex != 0)
            {
                BindProvider();

                if (Convert.ToBoolean(Session["EmployeeTypePermission"]) == false)
                {
                    ddlProviders.Enabled = false;
                }
                TextBox tk = new TextBox();
                tk = Task.FindControl("txtTasks") as TextBox;
                tk.Text = ddlProviders.SelectedValue.ToString();

                //TextBox t1 = new TextBox();
                //t1 = Notes.FindControl("txtTemp") as TextBox;
                //t1.Text = ddlProviders.SelectedValue.ToString();

                TextBox tA = new TextBox();
                tA = Appointments1.FindControl("txtApp") as TextBox;
                tA.Text = ddlProviders.SelectedValue.ToString();

                TextBox TXT = new TextBox();
                TXT = LabDashboard1.FindControl("txtProviderId") as TextBox;
                TXT.Text = ddlProviders.SelectedValue.ToString();

                TextBox txtAdmission = new TextBox();
                txtAdmission = ucAdmission.FindControl("txtAdmission") as TextBox;
                txtAdmission.Text = ddlProviders.SelectedValue.ToString();
                //txtAdmission
            }
            else if (ddlProviders.SelectedIndex == 0)
            {
            }
            ddlProviders_SelectedIndexChanged(this, null);
        }
    }

    protected void BindProvider()
    {
        try
        {
            Hashtable hshInput = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("intFacilityId", common.myInt(Session["FacilityId"]));
            DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshInput);
            if (objDs.Tables[0].Rows.Count > 0)
            {
                ddlProviders.Items.Clear();
                ddlProviders.DataSource = objDs;
                ddlProviders.DataValueField = "DoctorId";
                ddlProviders.DataTextField = "DoctorName";
                ddlProviders.DataBind();

                CheckUserDoctorOrNot();
            }
            ddlProviders.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void CheckUserDoctorOrNot()
    {
        try
        {
            BaseC.EMR objEmr = new BaseC.EMR(sConString);
            if (Session["UserID"] != null)
            {
                SqlDataReader objDr = (SqlDataReader)objEmr.GetEmployeeId(Convert.ToInt32(Session["UserID"]), Convert.ToInt16(Session["HospitalLocationID"]));
                if (objDr.Read())
                {
                    if ((Convert.ToString(objDr[0]) != "") && (objDr[0] != null))
                    {
                        ddlProviders.Items[0].Selected = false;
                        ddlProviders.SelectedIndex = ddlProviders.Items.IndexOf(ddlProviders.FindItemByValue(Convert.ToString(objDr[0])));
                        ddlProviders.Enabled = false;
                    }
                    else
                    {
                        ddlProviders.Items[0].Selected = true;
                        ddlProviders.Enabled = true;
                    }
                }
                objDr.Close();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlProviders_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            TextBox tk = new TextBox();
            tk = Task.FindControl("txtTasks") as TextBox;
            tk.Text = ddlProviders.SelectedValue.ToString();
            Task.BindGVTask(Convert.ToInt32(ddlProviders.SelectedValue.ToString()));

            //TextBox t1 = new TextBox();
            //t1 = Notes.FindControl("txtTemp") as TextBox;
            //t1.Text = ddlProviders.SelectedValue.ToString();
            //Notes.BindGVNotes(Convert.ToInt32(ddlProviders.SelectedValue.ToString()));

            TextBox tA = new TextBox();
            tA = Appointments1.FindControl("txtApp") as TextBox;
            tA.Text = ddlProviders.SelectedValue.ToString();
            Appointments1.BindGVAppointment(Convert.ToInt32(ddlProviders.SelectedValue.ToString()));

            TextBox TXT = new TextBox();
            TXT = LabDashboard1.FindControl("txtProviderId") as TextBox;
            TXT.Text = ddlProviders.SelectedValue.ToString();
            LabDashboard1.bindTestData();

            TextBox txtAdmission = new TextBox();
            txtAdmission = ucAdmission.FindControl("txtAdmission") as TextBox;
            txtAdmission.Text = ddlProviders.SelectedValue.ToString();
            ucAdmission.FillData();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RadDockLayout1_SaveDockLayout(object sender, Telerik.Web.UI.DockLayoutEventArgs e)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string serializedPositions = serializer.Serialize(e.Positions);
        string serializedIndices = serializer.Serialize(e.Indices);

        HttpCookie positionsCookie = new HttpCookie("ProviderDashboard_" + Session["UserId"],
            serializer.Serialize(new string[] { serializedPositions, serializedIndices }));

        positionsCookie.Expires = DateTime.Now.AddYears(1);
        Response.Cookies.Add(positionsCookie);
    }

    protected void RadDockLayout1_LoadDockLayout(object sender, Telerik.Web.UI.DockLayoutEventArgs e)
    {
        HttpCookie positionsCookie = Request.Cookies["ProviderDashboard_" + Session["UserId"]];
        if (!Object.Equals(positionsCookie, null))
        {
            string serializedPositionsAndIndices = positionsCookie.Value;
            if (!string.IsNullOrEmpty(serializedPositionsAndIndices))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string[] positionsAndIndices = serializer.Deserialize<string[]>(serializedPositionsAndIndices);

                //e.Positions = serializer.Deserialize<Dictionary<string, string>>(positionsAndIndices[0]);
                //e.Indices = serializer.Deserialize<Dictionary<string, int>>(positionsAndIndices[1]);
            }
        }
    }

    protected void btnSaveLayout_OnClick(object sender, EventArgs e)
    {
        List<DockState> dockstates = RadDockLayout1.GetRegisteredDocksState();
        Dictionary<string, string> myPositions = new Dictionary<string, string>();
        Dictionary<string, int> myIndices = new Dictionary<string, int>();
        Telerik.Web.UI.DockLayoutEventArgs myEvent = new DockLayoutEventArgs(myPositions, myIndices);

        SaveDashboardLayout(sender, myEvent, dockstates);
    }

    protected void SaveDashboardLayout(object sender, Telerik.Web.UI.DockLayoutEventArgs e, List<DockState> dockStates)
    {
        try
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string serializedPositions = serializer.Serialize(e.Positions);
            string serializedIndices = serializer.Serialize(e.Indices);
            string serializedDockStates = serializer.Serialize(dockStates);
            HttpCookie positionsCookie = new HttpCookie("ProviderDashboard_" + Session["UserId"], serializer.Serialize(new string[] { serializedPositions, serializedIndices, serializedDockStates }));
            positionsCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(positionsCookie);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
