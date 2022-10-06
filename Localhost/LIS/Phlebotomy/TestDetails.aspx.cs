using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Configuration;

public partial class LIS_Phlebotomy_TestDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
   
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);

            if (Request.QueryString["RegNo"] != null && Request.QueryString["PName"] != null)
            {
                this.lblPatientDetails.Text = HttpContext.GetGlobalResourceObject("PRegistration", "regno") + ": " + common.myStr(Request.QueryString["RegNo"]) + " | Patient Name: " + common.myStr(Request.QueryString["PName"]).Trim();
            }

            ViewState["ServiceID"] = 0;
            if (Request.QueryString["ServiceID"] != null)
            {
                if (common.myInt(Request.QueryString["ServiceID"]) > 0)
                {
                    ViewState["ServiceID"] = common.myInt(Request.QueryString["ServiceID"]);
                }
            }
            bindTestDetailsData();
            ShowAcknowledgementGrid();
            BindRemarks();
        }
    }

    void BindRemarks()
    {
        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = new DataSet();
            ds = objval.getFinalRemarks(common.myStr(Request.QueryString["Source"]), common.myInt(Request.QueryString["DiagSampleId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtFinalRemarks.Text = common.myStr(ds.Tables[0].Rows[0]["FinalRemarks"]);
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

    void ShowAcknowledgementGrid()
    {
        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = new DataSet();
            ds = objval.getSampleAcknowledgedByList(common.myStr(Request.QueryString["Source"]), common.myInt(Session["FacilityID"]), common.myInt(Request.QueryString["DiagSampleId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.Sort = "SampleAcknowledgedDate Asc";
                    gvAcknowledgementList.DataSource = ds.Tables[0].DefaultView;
                    gvAcknowledgementList.DataBind();
                }
                else
                {
                    BlankGrid();
                }
            }
            else
            {
                BlankGrid();
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                txtSampleCollectedBy.Text = common.myStr(ds.Tables[1].Rows[0]["SampleCollectedBy"]);
                txtSampleCollectedDate.Text = common.myStr(ds.Tables[1].Rows[0]["SampleCollectedDate"]);
                txtSampleDispatchBy.Text = common.myStr(ds.Tables[1].Rows[0]["SampleDispatchedBy"]);
                txtSampleDispatchedDate.Text = common.myStr(ds.Tables[1].Rows[0]["SampleDispatchedDate"]);
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                txtProvResultBy.Text = common.myStr(ds.Tables[2].Rows[0]["ProvResultBy"]);
                txtProvResultDate.Text = common.myStr(ds.Tables[2].Rows[0]["ProvResultDate"]);
                txtFinalizedBy.Text = common.myStr(ds.Tables[2].Rows[0]["FinalizedBy"]);
                txtFinalizedDate.Text = common.myStr(ds.Tables[2].Rows[0]["FinalResultDate"]);
                txtResultEnterBY.Text = common.myStr(ds.Tables[2].Rows[0]["ResultEnteredBy"]);
                txtResultEnteredDate.Text = common.myStr(ds.Tables[2].Rows[0]["ResultEnteredDate"]);
                if (common.myStr(ds.Tables[2].Rows[0]["EncodedBy"]) != "")
                {
                    txtEncodedBy.Text = common.myStr(ds.Tables[2].Rows[0]["EncodedBy"]);
                    txtEncodedDate.Text = common.myStr(ds.Tables[2].Rows[0]["EncodedDate"]);
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

    private void bindTestDetailsData()
    {
        try
        {
            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();

            coll.Add(common.myInt(ViewState["ServiceID"]));
            strXML.Append(common.setXmlTable(ref coll));

            BaseC.clsOtherServiceDetails objServiceDetail = new BaseC.clsOtherServiceDetails(sConString);
            DataSet ds = new DataSet();
            //ds= objServiceDetail.GetOtherServiceDetails(strXML.ToString(), common.myInt(Session["FacilityId"]));
            if (common.myInt(ds.Tables.Count) > 0)
            {
                if (common.myInt(ds.Tables[0].Rows.Count) > 0)
                {
                    DataRow DR = ds.Tables[0].Rows[0];

                    txtServiceName.Text = common.myStr(DR["ServiceName"]);
                    txtSubName.Text = common.myStr(DR["SubName"]);
                    // txtSampleName.Text = common.myStr(DR["SampleName"]);
                    //txtQuantity.Text = common.myStr(DR["SampleQuantity"]) + " " + common.myStr(DR["UnitName"]);
                    txtVacutainerName.Text = common.myStr(DR["VacutainerName"]);
                    txtVialColor.BackColor = System.Drawing.Color.FromName(common.myStr(DR["VialColor"]));
                    txtInstructionForPhlebotomy.Text = common.myStr(DR["InstructionForPhlebotomy"]);
                    ds = null;
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

    private void BlankGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SampleAcknowledgedDate");
            dt.Columns.Add("EntrySiteName");
            dt.Columns.Add("SampleAcknowledgedBy");

            gvAcknowledgementList.DataSource = dt;
            gvAcknowledgementList.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnUpdateComment_Click(object sender, EventArgs e)
    {
        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = new DataSet();
            string strmsg = objval.SaveFinalRemarks(common.myStr(Request.QueryString["Source"]),
                common.myInt(Request.QueryString["DiagSampleId"]), txtFinalRemarks.Text, common.myInt(Session["Userid"]));
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            lblMessage.Text = strmsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void lbtnNotes_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (common.myStr(Request.QueryString["RegNo"]) == "0")
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }

            if (common.myInt(Request.QueryString["LabNo"]) == 0)
            {
                lblMessage.Text = "Please Select " + common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")) + " !";
                return;
            }
            string Source = common.myStr(Request.QueryString["Source"]);
            if ((Source == "PACKAGE") || (Source == "ER"))
            {
                Source = "OPD";
            }
            RadWindowForNew.NavigateUrl = "~/LIS/Format/LISNotes.aspx?&SOURCE=" + common.myStr(Source)
                + "&eno=" + common.myStr(Request.QueryString["EncNo"])
                + "&RegNo=" + common.myStr(Request.QueryString["RegNo"])
                + "&pn=" + common.myStr(Request.QueryString["PName"])
                + "&LABNO=" + common.myStr(Request.QueryString["LabNo"]);    

            RadWindowForNew.Height = 500;
            RadWindowForNew.Width =700;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            //RadWindow1.OnClientClose = "wndAddService_OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

}
