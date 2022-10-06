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
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;

public partial class EMR_DoctorProfile_DoctorProfileMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();

    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;
            RTF1.EditModes = RTF1.EditModes ^ Telerik.Web.UI.EditModes.Html;
            if (!IsPostBack)
            {
                if (Request.QueryString["Mpg"] != null)
                {
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                    string pid = Session["CurrentNode"].ToString();
                    int len = pid.Length;
                    ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
                }
                else
                {

                    ViewState["PageId"] = "81";
                }

                PopulateDoctor();

                if (Request.QueryString["EmpNo"] != null)
                {
                    ViewState["empid"] = Request.QueryString["EmpNo"];
                    ddlDoctorName.SelectedIndex = ddlDoctorName.Items.IndexOf(ddlDoctorName.Items.FindByValue(Request.QueryString["EmpNo"]));
                    ddlDoctorName_OnSelectedIndexChanged(sender, e);

                }
                ////BaseC.Security Auditca= new BaseC.Security(sConString);
                ////Auditca.AuditCommonAccess(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityID"].ToString()), Convert.ToInt32(ViewState["PageId"].ToString()));
                RTF1.RealFontSizes.Clear();
                RTF1.RealFontSizes.Add("9pt");
                RTF1.RealFontSizes.Add("11pt");
                RTF1.RealFontSizes.Add("12pt");
                RTF1.RealFontSizes.Add("14pt");
                RTF1.RealFontSizes.Add("18pt");
                RTF1.RealFontSizes.Add("20pt");
                RTF1.RealFontSizes.Add("24pt");
                RTF1.RealFontSizes.Add("26pt");
                RTF1.RealFontSizes.Add("36pt");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateDoctor()
    {
        try
        {
            ddlDoctorName.Enabled = true;
            ddlDoctorName.Enabled = true;
            //BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
            //DataSet ds = objEMRVitals.GetDoctors(Convert.ToInt16(Session["HospitalLocationID"]));
            Hashtable hstInput;
            hstInput = new Hashtable();
            hstInput.Add("@HospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", hstInput);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDoctorName.DataSource = ds;
                ddlDoctorName.DataBind();
            }
            ddlDoctorName.Items.Insert(0, new ListItem("[Select]", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlDoctorName_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BaseC.EMRMasters.EMREmployeeProfile DocProf = new BaseC.EMRMasters.EMREmployeeProfile(sConString);
            BaseC.Security AuditCA = new BaseC.Security(sConString);

            SqlDataReader dr = DocProf.getDoctorProfile(Convert.ToInt32(ddlDoctorName.SelectedItem.Value));
            if (dr.HasRows == true)
            {
                dr.Read();
                RTF1.Content = dr[0].ToString();
                AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), 0, 0, Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(ViewState["empid"]), "ACCESSED", Convert.ToString(Session["IPAddress"]));
            }
            else
            {
                RTF1.Content = "";
            }
            dr.Close();
            ViewState["DoctorId"] = ddlDoctorName.SelectedItem.Value;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void SaveDoctorProfile_OnClick(Object sender, EventArgs e)
    {
        try
        {
            String strRichText = RTF1.Content.ToString();
            BaseC.EMRMasters.EMREmployeeProfile DocProf = new BaseC.EMRMasters.EMREmployeeProfile(sConString);
            String str = DocProf.SaveEmployeeProfile(Convert.ToInt32(ddlDoctorName.SelectedItem.Value), bc.ParseQ(Convert.ToString(strRichText.Trim())), Convert.ToInt32(Session["UserID"].ToString()), Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityID"].ToString()), Convert.ToInt32(ViewState["PageId"].ToString()));
            lblMessage.Text = str;
            ddlDoctorName_OnSelectedIndexChanged(sender, e);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkEmployee_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DoctorId"] != null)
            {
                Response.Redirect("/MPages/employee.aspx?EmpNo=" + ViewState["DoctorId"].ToString().Trim(), false);
            }
            else
            {
                Response.Redirect("/MPages/employee.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lnkEmployeeLookup_OnClick(object sebder, EventArgs e)
    {
        Response.Redirect("~/mpages/EmployeeLockUp.aspx", false);
    }

    protected void lnkClassification_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("/MPages/EmployeeClassification.aspx?EmpId=" + common.myInt(ViewState["emp"]), false);
    }

    protected void lnkAppointmentTemplate_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DoctorId"] != null)
            {
                Response.Redirect("/mpages/providertimings.aspx?EmpNo=" + ViewState["DoctorId"].ToString().Trim(), false);
            }
            else
            {
                Response.Redirect("/mpages/providertimings.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkProviderDetails_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DoctorId"] != null)
            {
                Response.Redirect("/MPages/ProviderDetails.aspx?EmpNo=" + ViewState["DoctorId"].ToString().Trim(), false);
            }
            else
            {
                Response.Redirect("/MPages/ProviderDetails.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}