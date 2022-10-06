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

public partial class WardManagement_PatientTransferWardtoOT : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myStr(Request.QueryString["RegNo"]) != "")
            {
                Session["StatusCngCheck"] = 0;
                BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]));
                BindTheater();
            }
            bindData();
        }
    }

    private void BindPatientHiddenDetails(String RegistrationNo)
    {
        if (Session["PatientDetailString"] != null)
        {
            lblPatientDetail.Text = Session["PatientDetailString"].ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            saveData("A");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            saveData("B");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }

    protected void saveData(string saveEvent)
    {
        BaseC.clsOTBooking objCls = new BaseC.clsOTBooking(sConString);
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(ddlTheater.SelectedValue) == 0)
            {
                lblMessage.Text = "Please select theater. !";
                return;
            }
            if (common.myLen(txtWardRemarks.Text) == 0)
            {
                lblMessage.Text = "Remarks can't be blank !";
                return;
            }

            string strMsg = objCls.savePatientTransferWardtoOT(saveEvent, common.myInt(ViewState["TransferId"]), common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                common.myStr(txtWardRemarks.Text).Trim(), common.myInt(Session["UserID"]), common.myInt(ddlTheater.SelectedValue));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }

            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objCls = null;
        }
    }

    private void bindData()
    {
        DataSet ds = new DataSet();
        BaseC.clsOTBooking objCls = new BaseC.clsOTBooking(sConString);
        try
        {
            ds = objCls.getPatientTransferWardtoOTRemarks(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));

            ViewState["TransferId"] = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["TransferId"] = common.myInt(ds.Tables[0].Rows[0]["TransferId"]);
                txtWardRemarks.Text = common.myStr(ds.Tables[0].Rows[0]["WardRemarks"]);
                ddlTheater.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["TheatreID"]);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }

    private void BindTheater()
    {
        DataSet ds = new DataSet();
        BaseC.clsOTBooking objCls = new BaseC.clsOTBooking(sConString);
        try
        {
            ds = objCls.FillOTList(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
            ddlTheater.DataSource = ds.Tables[0];
            ddlTheater.DataTextField = "TheatreName";
            ddlTheater.DataValueField = "TheatreId";
            ddlTheater.DataBind();

            ddlTheater.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));

            ds = new DataSet();
            ds = objCls.getDefaultOTTheatreId(common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlTheater.SelectedIndex = ddlTheater.Items.IndexOf(ddlTheater.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["TheaterId"]).ToString()));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCls = null;
        }
    }


}
