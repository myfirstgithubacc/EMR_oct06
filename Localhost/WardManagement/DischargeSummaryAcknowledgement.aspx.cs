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
using BaseC;
using System.IO;
using System.Xml;


public partial class WardManagement_DischargeSummaryAcknowledgement : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    BaseC.ATD objatd;
    BaseC.clsEMRBilling baseEBill;
    BaseC.clsLISMaster objLISMaster;
    BaseC.EMRBilling.clsOrderNBill bOrdernBill;
    BaseC.WardManagement objwd;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            txtAckDate.SelectedDate = common.myDate(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            if (common.myStr(Request.QueryString["RegNo"]) != "")
            {
                Session["StatusCngCheck"] = 0;
                BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]));
            }
            BindDischargeOutlierRemarks();


        }
    }


    void BindDischargeOutlierRemarks()
    {
        WardManagement objw = new WardManagement();
        DataSet dsStatus = objw.GetPatientDischargeSummaryAcknowledgementStatus(common.myInt(Session["EncounterId"]), "DischargeSummaryAcknowledge");
        if (dsStatus.Tables[0].Rows.Count > 0)
        {

            btnSave.Enabled = false;
            //ddldischargeOutlierstatus.SelectedIndex = ddldischargeOutlierstatus.Items.IndexOf(ddldischargeOutlierstatus.Items.FindItemByValue(common.myStr(dsStatus.Tables[0].Rows[0]["OutlierId"])));
            txtAckDate.SelectedDate = (common.myDate(dsStatus.Tables[0].Rows[0]["DischargeSummaryAcknowledgedDate"]));
            lblenteredby.Text = common.myStr(dsStatus.Tables[0].Rows[0]["EnteredBy"]);
            lblentered.Visible = true;
            lblenteredby.Visible = true;



        }
        else
        {
            //txtAckDate.SelectedDate = common.myDate(DateTime.Now.ToString("yyyy-MM-dd h:mm"));
            lblenteredby.Text = "";
            lblentered.Visible = false;
            lblenteredby.Visible = false;

        }


    }

    private void CheckUserRights()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        try
        {
            bool IsValidate = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "isChangeDischargeOutlier");
            if (IsValidate)
            {
                //ddldischargeOutlierstatus.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                //ddldischargeOutlierstatus.Enabled = false;
                btnSave.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { objSecurity = null; }
    }

    void BindPatientHiddenDetails(String RegistrationNo)
    {
        try
        {
            ViewState["StatusId"] = common.myStr(Request.QueryString["StatusId"]);
            if (Session["PatientDetailString"] != null)
            {
                //lblPatientDetail.Text = Session["PatientDetailString"].ToString();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WardManagement objw = new WardManagement();
            string Status = objw.SavePatientDischargeSummaryAcknowledgement(common.myInt(Session["EncounterId"]), common.myDate(txtAckDate.SelectedDate.Value), common.myInt(Session["UserId"]),"DischargeSummaryAcknowledge");

            BindDischargeOutlierRemarks();
           // CheckUserRights();
            lblMessage.Text = Status;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

  
}