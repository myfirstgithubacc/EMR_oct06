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
using BaseC;
using Telerik.Web.UI;

public partial class EMR_Dashboard_Parts_Orders : WebPartBase
{
    Hashtable hsTb;
    DL_Funs fun = new DL_Funs();
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    public EMR_Dashboard_Parts_Orders()
    {
        this.Title = "Orders";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        BindGDOrders();
    }

    public void BindGDOrders()
    {
        try
        {
            Dashboard dsh = new Dashboard();

            DataSet dsApp = new DataSet();
            dsApp = dsh.getDashBoardValue(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["registrationid"]), hdnFromDate.Text, hdnToDate.Text, hdnDateVale.Text, hdnEncounterNumber.Text == "" ? Session["EncounterId"].ToString() : hdnEncounterNumber.Text, Convert.ToInt32(Session["FacilityID"]), "UspEMRGetPatientServices");
            if (dsApp.Tables[0].Rows.Count > 0)
            {
                dsApp.Tables[0].DefaultView.RowFilter = "ServiceType IN('I','IS')";
                GDOrders.DataSource = dsApp.Tables[0].DefaultView;
                GDOrders.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GDOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDOrders.PageIndex = e.NewPageIndex;
        BindGDOrders();
    }
    public void setLink()
    {
        //if (common.myStr(Session["FromPage"]) == "POPUP")
        //{

        //    //RadWindow1.NavigateUrl = "/EMR/Orders/ICMOrders.aspx?Regno=" + lblregno.Text.Trim() + "&Encno=" + lblencno.Text.Trim() + "";
        //    RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServices.aspx?Regid=" + common.myInt(Session["RegistrationID"])
        //        + "&RegNo=" + common.myInt(Session["Regno"])
        //        + "&EncId=" + common.myInt(Session["encounterid"])
        //        + "&EncNo=" + common.myInt(Session["Encno"])
        //        + "&OP_IP=I&FromWard=Y&CompanyId=0&InsuranceId=0&CardId=0&PayerType=&BType=";
        //    RadWindow1.Height = 570;
        //    RadWindow1.Width = 850;
        //    RadWindow1.Top = 10;
        //    RadWindow1.Left = 10;
        //    RadWindow1.OnClientClose = "SearchPatientOnClientClose";//
        //    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //    RadWindow1.Modal = true;
        //    RadWindow1.VisibleStatusbar = false;
        //    RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        //}
        //else
        //{
        //    Response.Redirect("~/EMR/Orders/Orders.aspx", false);

        //}
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindGDOrders();
    }
    protected void lnkOrder_OnClick(object sender, EventArgs e)
    {
        setLink();
    }
}