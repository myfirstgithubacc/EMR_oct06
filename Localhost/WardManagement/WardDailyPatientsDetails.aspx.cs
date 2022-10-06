using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Configuration;

public partial class WardManagement_WardDailyPatientsDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            bindControl();
        }
    }
    private void bindControl()
    {
        DataSet ds = new DataSet();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            //ds = objadt.GetWardStation(common.myInt(Session["FacilityId"]));
            ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
            ddlWard.DataSource = ds.Tables[0];
            //ddlWard.DataTextField = "StationName";
            //ddlWard.DataValueField = "ID";
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlWard.SelectedIndex = 0;

            //  BindGroupTaggingMenu();
            //BindStatus();
        }

        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objadt = null;
            ds.Dispose();
        }
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            string Rurl = "";
            int iwardId = 0;
            string chk = "";
            if (common.myInt(ddldailypatientdetails.SelectedValue)>0)
            {
                
                iwardId = common.myInt(ddlWard.SelectedValue);
                if(chkexport.Checked==true)
                {
                    chk = "T";
                }
                else
                {
                    chk = "F";
                }

                if (common.myInt(ddldailypatientdetails.SelectedValue)==1)
                {
                    Rurl = "PrintWardDailyPatientsDetails.aspx?PT=W&WID=" + iwardId + "&CHK=" + chk + "";
                }
                else if(common.myInt(ddldailypatientdetails.SelectedValue) == 2)
                {
                    Rurl = "PrintWardDailyPatientsDetails.aspx?PT=E&WID=" + iwardId + "&CHK=" + chk + "";
                }
                if(Rurl!="")
                {
                    
                    RadWindowForReport.NavigateUrl = Rurl;
                    RadWindowForReport.Height = 610;
                    RadWindowForReport.Width = 1000;
                    RadWindowForReport.Top = 10;
                    RadWindowForReport.Left = 10;
                    RadWindowForReport.VisibleOnPageLoad = true;
                    RadWindowForReport.Modal = true;
                    RadWindowForReport.VisibleStatusbar = false;
                }
            }
            else
            {

            }
        }
        catch
        {

        }
    }
}