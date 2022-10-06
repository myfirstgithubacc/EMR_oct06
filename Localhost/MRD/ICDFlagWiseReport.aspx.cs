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

public partial class MRD_ICDFlagWiseReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;

    BaseC.clsMRD objMRD;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfromdate.SelectedDate = DateTime.Now;
            dtpTodate.SelectedDate = DateTime.Now;
            BindICDFlagMaster();
            bindGroup();

            if ((common.myStr(Request.QueryString["Diag"]).ToString() == "DD") || common.myStr(Request.QueryString["Diag"]).ToString() == "DDM" ||
                common.myStr(Request.QueryString["Diag"]).ToString() == "FORMP" || common.myStr(Request.QueryString["Diag"]).ToString() == "MLC")
            {
                ddlICDFlag.Visible = false;
                lblUnderPackage.Visible = false;
            }
            if (common.myStr(Request.QueryString["Diag"]).ToString() == "FORMP")
            {
                pnlGroup.Visible = true;
            }
            else
            {
                pnlGroup.Visible = false;
            }
            if (common.myStr(Request.QueryString["Diag"]).ToString() == "MLC")
            {
                ddlOPIP.Visible = false;
                lblSource.Visible = false;
                lblHeader.Text = "MLC Detail Report";
            }
            else
            {
                ddlOPIP.Visible = true;
                lblSource.Visible = true;
            }
            //if (common.myStr(Request.QueryString["Diag"]).ToString().Length >0)
            //{
            //    ddlICDFlag.Visible = false;
            //    lblUnderPackage.Visible = false;
            //}
        }
    }
    private void BindICDFlagMaster()
    {
        try
        {
            objMRD = new BaseC.clsMRD(sConString);
            DataSet ds = objMRD.GetICDFlagMaster();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlICDFlag.DataSource = ds.Tables[0];
                ddlICDFlag.DataTextField = "ICDFlagName";
                ddlICDFlag.DataValueField = "ICDFlagId";
                ddlICDFlag.DataBind();

            }

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void btnPrintreport_Click(object sender, EventArgs e)
    {
        ShowReport();
    }
    protected void ShowReport()
    {
        StringBuilder SubGroupIds = new StringBuilder();
        string SubGroupIdsStr = string.Empty;
        if (ddlSubGroup.CheckedItems.Count > 0)
        {
            for (int i = 0; i < ddlSubGroup.CheckedItems.Count; i++)
            {
                SubGroupIds.Append(ddlSubGroup.CheckedItems[i].Value.ToString() + ",");
            }
            SubGroupIdsStr = SubGroupIds.ToString().Substring(0, SubGroupIds.ToString().Length - 1);
        }
        if (ddlICDFlag.Visible == true)
        {
            if (ddlICDFlag.SelectedValue == "2")
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&ICDFlagId=" + ddlICDFlag.SelectedValue + "&Export=" + chkExport.Checked + "&OutPut=A&ReportName=MRDNEO";
            }
            else if (ddlICDFlag.SelectedValue == "1")
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&ICDFlagId=" + ddlICDFlag.SelectedValue + "&Export=" + chkExport.Checked + "&OutPut=A&ReportName=MRDNOT";
            }
        }
        else
        {
            if (common.myStr(Request.QueryString["Diag"]).ToString() == "DD")
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&OutPut=A&ReportName=DiseaseSt";
            }
            if (common.myStr(Request.QueryString["Diag"]).ToString() == "DDM")
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&Export=" + chkExport.Checked + "&OutPut=A&ReportName=DiseaseStM";
            }
            if (common.myStr(Request.QueryString["Diag"]).ToString() == "FORMP")
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&SourceType=" + ddlOPIP.SelectedValue + "&GroupId=" + common.myInt(ddlGroup.SelectedValue) + "&SubGroupId=" + common.myStr(SubGroupIdsStr) + "&Export=" + chkExport.Checked + "&OutPut=A&ReportName=FORMP";
            }
            if (common.myStr(Request.QueryString["Diag"]).ToString() == "MLC")
            {
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + dtpfromdate.SelectedDate.Value.ToString("yyyy/MM/dd") + "&Todate=" + dtpTodate.SelectedDate.Value.ToString("yyyy/MM/dd") + "&Export=" + chkExport.Checked + "&OutPut=A&ReportName=MLCDetail";
            }
        }

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1020;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;

        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;
        ViewState["chkAllDep"] = "";
    }
    protected void ddlGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        bindSubGroup();
    }
    private void bindSubGroup()
    {
        DataSet ds = new DataSet();
        try
        {
            if (ddlGroup.SelectedIndex < 1)
            {
                return;
            }
            int grpid;
            ddlSubGroup.Text = "";
            grpid = Convert.ToInt32(ddlGroup.SelectedValue);
            ds = new DataSet();
            objMRD = new BaseC.clsMRD(sConString);
            ds = objMRD.getIcd9SubGroup(grpid);
            ddlSubGroup.DataSource = ds.Tables[0].Copy();
            ddlSubGroup.DataValueField = "Id";
            ddlSubGroup.DataTextField = "Name";
            ddlSubGroup.DataBind();

            //ddlSubGroup.Items.Insert(0, new RadComboBoxItem("", "0"));


        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    private void bindGroup()
    {
        DataSet ds = new DataSet();
        try
        {
            objMRD = new BaseC.clsMRD(sConString);
            ds = objMRD.getIcd9Group();

            ddlGroup.DataSource = ds.Tables[0].Copy();
            ddlGroup.DataValueField = "Id";
            ddlGroup.DataTextField = "Name";
            ddlGroup.DataBind();

            ddlGroup.Items.Insert(0, new RadComboBoxItem("", "0"));


            bindSubGroup();

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
      
   
    
}
