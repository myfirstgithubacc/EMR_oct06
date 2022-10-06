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

public partial class WardManagement_DischargeOutlier : System.Web.UI.Page
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
        DataSet ds = new DataSet();
        objatd = new BaseC.ATD(sConString);
        ds = objatd.GetDischargeOutlierRemarks(common.myInt(Session["HospitalLocationId"]),"Discharge Outlier");

        ddldischargeOutlierstatus.DataSource = ds;
        ddldischargeOutlierstatus.DataTextField = "Remarks";
        ddldischargeOutlierstatus.DataValueField = "id";
        ddldischargeOutlierstatus.DataBind();
        ddldischargeOutlierstatus.Items.Insert(0, new RadComboBoxItem("Select", "0"));

        WardManagement objw = new WardManagement();
       DataSet dsStatus = objw.GetDischargeOutlierStatus(common.myInt(Session["EncounterId"]));
      if(dsStatus.Tables[0].Rows.Count>0)
        {
            ddldischargeOutlierstatus.SelectedIndex = ddldischargeOutlierstatus.Items.IndexOf(ddldischargeOutlierstatus.Items.FindItemByValue(common.myStr(dsStatus.Tables[0].Rows[0]["OutlierId"])));
            btnSave.Text = "Cancel  (Ctrl+F2)";
            lblenteredby.Text = common.myStr(dsStatus.Tables[0].Rows[0]["EnteredBy"]);
            lblentered.Visible = true;
            lblenteredby.Visible = true;
            CheckUserRights();
        }
        else
        {
            btnSave.Text = "Save (Ctrl+F2)";
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
                ddldischargeOutlierstatus.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                ddldischargeOutlierstatus.Enabled = false;
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
           string Status= objw.SaveDischargeOutlierStatus(common.myInt(Session["EncounterId"]),common.myInt(ddldischargeOutlierstatus.SelectedValue),1, common.myInt(Session["UserId"]));

            if(Status.Contains("Saved"))
            {
                BindDischargeOutlierRemarks();
            }

            CheckUserRights();
            lblMessage.Text = Status;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void BindEncounterStatus()
    {
        try
        {

            EMRBilling objBill = new EMRBilling(sConString);
            DataSet ds = objBill.FillStatusMaster();
            DataView DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "Code IN ('MD','O','Ack')";
            //ddlFilter.DataSource = DV.ToTable();
            //ddlFilter.DataValueField = "StatusID";
            //ddlFilter.DataTextField = "Status";
            //ddlFilter.DataBind();
            ViewState["Acknowledge"] = "0";
            ViewState["MarkForDischarge"] = "0";
            ViewState["OPEN"] = "0";
            DV.RowFilter = "Code = 'MD'";
            if (DV.ToTable().Rows.Count > 0)
            {
                ViewState["StatusCode"] = common.myStr(DV.ToTable().Rows[0]["Code"]);
                ViewState["MarkForDischarge"] = common.myStr(DV.ToTable().Rows[0]["StatusId"]);

            }
            DV.RowFilter = "";
            DV.RowFilter = "Code = 'Ack'";
            if (DV.ToTable().Rows.Count > 0)
            {
                ViewState["Acknowledge"] = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            DV.RowFilter = "";
            DV.RowFilter = "Code = 'SB'";
            if (DV.ToTable().Rows.Count > 0)
            {
                ViewState["SentForBilling"] = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            DV.RowFilter = "";
            DV.RowFilter = "Code = 'O'";
            if (DV.ToTable().Rows.Count > 0)
            {
                ViewState["OPEN"] = common.myStr(DV.ToTable().Rows[0]["StatusId"]);
            }
            if ((common.myInt(ViewState["MarkForDischarge"]) == common.myInt(ViewState["StatusId"]))
                || (common.myInt(ViewState["OPEN"]) == common.myInt(ViewState["StatusId"])) || (common.myInt(ViewState["Acknowledge"]) == common.myInt(ViewState["StatusId"])))
            {

                objwd = new BaseC.WardManagement();
                DataSet tbl = objwd.getNextWardEncounterStatus(common.myInt(ViewState["StatusId"]));
                tbl.Tables[0].DefaultView.RowFilter = "StatusId >0";

                if (common.myInt(ViewState["StatusId"]) == 183)
                {
                    for (int i = 0; i < tbl.Tables[0].Rows.Count; i++)
                    {
                        if (common.myInt(tbl.Tables[0].Rows[i]["StatusId"]) == 197)
                        {
                            tbl.Tables[0].Rows[i]["StatusId"] = 184;
                            tbl.Tables[0].Rows[i]["EncounterStatus"] = "Sent For Billing";
                        }
                    }
                    tbl.Tables[0].AcceptChanges();
                }
                else if (common.myInt(ViewState["StatusId"]) == 184)
                {
                    for (int i = 0; i < tbl.Tables[0].Rows.Count; i++)
                    {
                        if (common.myInt(tbl.Tables[0].Rows[i]["StatusId"]) == 197)
                        {
                            tbl.Tables[0].Rows[i].Delete();
                            tbl.Tables[0].AcceptChanges();
                            break;
                        }
                    }

                }
                else if (common.myInt(ViewState["StatusId"]) == 197)
                {
                    for (int i = 0; i < tbl.Tables[0].Rows.Count; i++)
                    {
                        if (common.myInt(tbl.Tables[0].Rows[i]["StatusId"]) == 183)
                        {
                            tbl.Tables[0].Rows[i].Delete();
                            tbl.Tables[0].AcceptChanges();
                            break;
                        }
                    }

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
}
