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
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;

public partial class BloodBank_SetupMaster_ReleaseRequisitionList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsBb objBb;

 

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            bindData(Convert.ToInt32(Request.QueryString["Regid"]));
            
            BindLegend();
        }
    }


    private void clearControl()
    {
        txtRequisitionNo.Text = "";
        txtPatientName.Text = "";
        lblMessage.Text = "&nbsp;";

    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData(Convert.ToInt32(Request.QueryString["Regid"]));

    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        lblMessage.Text = "";
    }

    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                Label lblRequestType = (Label)e.Item.FindControl("lblRequestType");

                if (lblRequestType.Text == "Immediate")
                {
                    e.Item.BackColor = System.Drawing.Color.LightYellow;
                }
                else if (lblRequestType.Text == "Urgent")
                {
                    e.Item.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }
        catch
        {
        }
    }

    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvEncounter.CurrentPageIndex = e.NewPageIndex;

        bindData(Convert.ToInt32(Request.QueryString["Regid"]));
    }

    protected void gvEncounter_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                hdnRelease.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnReleaseID")).Value);
                hdnRequisitionId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRequisitionId")).Value);
                
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                return;
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindData(int Regid)
    {
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {

            ds = objBb.getReleaseRequisitionList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myStr(Regid), common.myStr(txtPatientName.Text));

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            gvEncounter.DataSource = ds.Tables[0];
            gvEncounter.DataBind();
            BindLegend();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindLegend()
    {
        //for urgent requests
        Label LBL1;

        TableRow tr1;
        TableCell td1;

        tr1 = new TableRow();

        td1 = new TableCell();
        LBL1 = new Label();
        LBL1.BorderWidth = Unit.Pixel(1);        
        LBL1.BackColor = System.Drawing.Color.LightPink;
        LBL1.SkinID = "label";
        LBL1.Width = Unit.Pixel(15);
        LBL1.Height = Unit.Pixel(14);

        td1.Controls.Add(LBL1);
        tr1.Cells.Add(td1);
        tblLegendUrgent.Rows.Add(tr1);


        //for Immediate requests
        Label LBL2;

        TableRow tr2;
        TableCell td2;

        tr2 = new TableRow();

        td2 = new TableCell();
        LBL2 = new Label();
        LBL2.BorderWidth = Unit.Pixel(1);
        LBL2.BackColor = System.Drawing.Color.LightYellow;
        LBL2.SkinID = "label";
        LBL2.Width = Unit.Pixel(15);
        LBL2.Height = Unit.Pixel(14);

        td2.Controls.Add(LBL2);
        tr2.Cells.Add(td2);
        tblLegendImmediate.Rows.Add(tr2);

    }
}
