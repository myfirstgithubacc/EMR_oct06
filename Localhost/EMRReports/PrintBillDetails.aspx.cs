using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using BaseC;

public partial class EMRReports_PrintBillDetails :  System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.ParseData bc = new BaseC.ParseData();
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ATD objadt;
    DAL.DAL dl;
    DataSet ds;
    private bool Stauts = false;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (!common.myStr(Request.QueryString["RW"]).Equals("IPNOEnable"))
        {

            this.MasterPageFile = "~/Include/Master/BlankMaster.master";

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //    else if (common.myStr(Request.QueryString["RN"]) == "BillPrint")
            //    {
            txtIpno.Text = common.myStr(Request.QueryString["EncNo"]);

            //        txtInvoice.Text = common.myStr(Session["InvoiceId"]);
            lblHeader.Text = "IP Bill Detail";
            hdnEncId.Value = common.myStr(Request.QueryString["EncId"]);
            //palendr change
            if (common.myStr(Request.QueryString["RW"]) == "IPNOEnable")
            {

                txtIpno.ReadOnly = false;
            }


            //        rdolabelform.Visible = false;
            //        rdolabelformipbilldetail.Visible = true;
            //        tradmfrm.Visible = true;
            //        trdate.Visible = false;
            //        trddl.Visible = false;
            //        trgrid.Visible = false;
            //        trloction.Visible = false;
            //        rdoIpbillType.Visible = true;

            //    }

        }

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {

        if (common.myStr(txtIpno.Text) != "")
        {
            Security objSecurity = new Security(sConString);
            bool orginalshow = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "isAuthorizedToPrintOriginalBill");

            Session["IPNO"] = txtIpno.Text.Trim();
            string sEncDate = common.myStr(Request.QueryString["AdmDt"]);
            int iRegId = common.myInt(Request.QueryString["sRegId"]);
            if (common.myStr(Request.QueryString["RN"]) == "BillPrint")
            {
                string strShowAdvance = "Y", strShowDiscount = "Y";
                RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(hdnEncId.Value)
               + "&RptName=IPBill&RptType=" + rdoIpbillType.SelectedValue
               + "&BillId=" + common.myInt(0)
               + "&AdmDt=" + common.myDate(sEncDate)
               + "&Adv=" + strShowAdvance
               + "&Disc=" + strShowDiscount
               + "&RegId=" + common.myInt(iRegId)
                  //+ "&FromDate=" + Convert.ToDateTime("1900-01-01").ToString("dd/MM/yyyy HH:mm")
                  //+ "&ToDate=" + Convert.ToDateTime("2100-01-01").ToString("dd/MM/yyyy HH:mm")
                  + "&FromDate=NULL"
               + "&ToDate=NULL"
               + "&IsFilterByDate=" + common.myInt(0)
                + "&ShowOrginal=" + common.myBool(orginalshow)
                + "&ReportType=" + rdoIpbillType.SelectedValue
                + "&ShowPharmacy=N";
            }
           else if (common.myStr(Request.QueryString["RW"]) == "IPNOEnable")
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet ds = dl.FillDataSet(CommandType.Text, "select Id,RegistrationId from Encounter where EncounterNo ='" + txtIpno.Text + "' and FacilityId=" + common.myInt(Session["FacilityId"]));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    string strShowAdvance = "Y", strShowDiscount = "Y";
                    RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(ds.Tables[0].Rows[0]["Id"])
                   + "&RptName=IPBill&RptType=" + rdoIpbillType.SelectedValue
                   + "&BillId=" + common.myInt(0)
                   + "&AdmDt=" + common.myDate(sEncDate)
                   + "&Adv=" + strShowAdvance
                   + "&Disc=" + strShowDiscount
                   + "&RegId=" + common.myInt(ds.Tables[0].Rows[0]["RegistrationId"])
                      //+ "&FromDate=" + Convert.ToDateTime("1900-01-01").ToString("dd/MM/yyyy HH:mm")
                      //+ "&ToDate=" + Convert.ToDateTime("2100-01-01").ToString("dd/MM/yyyy HH:mm")
                      + "&FromDate=NULL"
                   + "&ToDate=NULL"
                   + "&IsFilterByDate=" + common.myInt(0)
                    + "&ShowOrginal=" + common.myBool(orginalshow)
                    + "&ReportType=" + rdoIpbillType.SelectedValue
                    + "&ShowPharmacy=N";
                }
            }
            else
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet ds = dl.FillDataSet(CommandType.Text, "select Id,RegistrationId from Encounter where EncounterNo ='" + txtIpno.Text + "' and FacilityId=" + common.myInt(Session["FacilityId"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //RadWindowForNew.NavigateUrl = "/EMRReports/DischargeSummary.aspx?IpNo=" + common.myStr(txtIpno.Text) + "";
                    RadWindowForNew.NavigateUrl = "/EMRReports/PrintPdf1.aspx?page=Report&EncId=" + common.myInt(hdnEncId.Value) +
                        "&RegId=" + common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]) + "&For=DISSUM&ShowOrginal= " + common.myBool(orginalshow);
                }
            }

            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 950;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
            //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }

    }
}