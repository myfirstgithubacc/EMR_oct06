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

public partial class OTScheduler_ReportDoctorDeptSurgeryCases : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));

          
            dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));

            BindDepartment();
            fillDoctor();
            ddlDepartment.Enabled = false;
        }

    }

    protected void btnPrintreport_OnClick(object sender, EventArgs e)
    {
        string strDepartmentIDs = common.GetCheckedItems(ddlDepartment);
        string strDoctorIDs = common.GetCheckedItems(ddlDoctor);

        RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?FromDate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&RptName=OT&RptType=DoctorDeptSurgeryCases&ReportType=" + common.myStr(rblReportType.SelectedValue) + "&DeptIds=" + common.myStr(strDepartmentIDs) + "&DoctorIds=" + common.myStr(strDoctorIDs);
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 980;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;        
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void BindDepartment()
    {
        try
        {
            DataSet ds = new DataSet();
            ddlDepartment.Items.Clear();
            BaseC.EMRMasters objDept = new BaseC.EMRMasters(sConString);
            ds = objDept.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"]), "");
            ddlDepartment.DataSource = ds.Tables[0];
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentID";
            ddlDepartment.DataBind();

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ex.Message.ToString();
            objException.HandleException(ex);
        }
    }

    protected void fillDoctor()
    {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                Hashtable HashIn = new Hashtable();
                HashIn.Add("@HospitalLocationId", Session["HospitalLocationID"]);
                HashIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

                DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspgetdoctorlist", HashIn);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlDoctor.DataSource = ds.Tables[0];
                        ddlDoctor.DataTextField = "DoctorName";
                        ddlDoctor.DataValueField = "DoctorId";
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

    protected void rblReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDepartment();
        fillDoctor();

        if (rblReportType.SelectedValue.ToString()=="DO" || rblReportType.SelectedValue.ToString() == "DS")
        {
            ddlDepartment.Enabled = false;
            ddlDoctor.Enabled = true;
        }
        if (rblReportType.SelectedValue.ToString() == "DE")
        {
            ddlDepartment.Enabled = true;
            ddlDoctor.Enabled = false;
        }
    }
}
