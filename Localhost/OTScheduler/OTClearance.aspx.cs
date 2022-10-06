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
using System.Drawing;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Text;

public partial class OTScheduler_OTClearance : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsPharmacy objPharmacy;
    BaseC.WardManagement objWard;

    DAL.DAL dl = new DAL.DAL();
    BaseC.clsEMR objEMR;
    BaseC.clsLISMaster objCommon;
    StringBuilder strXML;
    ArrayList coll;
    DataTable detaildata;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        //this.MasterPageFile = "~/Include/Master/emr.master";

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        objEMR = new BaseC.clsEMR(sConString);

        //if (common.myInt(Session["EncounterId"]) == 0)
        //{
        //    Response.Redirect("/default.aspx?RegNo=0");
        //}

        if (!IsPostBack)
        {
            objPharmacy = new BaseC.clsPharmacy(sConString);

            hdnSelectedIndentId.Value = "";
            hdnSelectedIndentNo.Value = "";
            hndSelectedIsInsuranceCompany.Value = false.ToString();

            dtpOTDateFrom.SelectedDate = DateTime.Now.AddDays(-7);
            dtpOTDateTo.SelectedDate = DateTime.Now;
            ddlAcknowledge_SelectedIndexChanged(null, null);
            bindMainData();

        }
    }

    private void clearControl()
    {
        //txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
        //txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        //txtToDate.SelectedDate = DateTime.Now;
        //txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        //ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
        ddlAcknowledge.SelectedValue = "U";
        ddlSearchOn.SelectedValue = "I";
        dtpOTDateFrom.SelectedDate = DateTime.Now.AddDays(-7);
        dtpOTDateTo.SelectedDate = DateTime.Now;
        //hdnMEncounterId.Value = "";
        //hdnMRegistrationId.Value = "";
        bindMainData();


    }

    void bindMainData()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet ds = new DataSet();

            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

            if (txtSearchOn.Text.Trim().Length > 0)
            {
                if (ddlSearchOn.SelectedValue == "R")
                {
                    HshIn.Add("@intRegistrationNo", common.myInt(txtSearchOn.Text.Trim()));
                }
                else if (ddlSearchOn.SelectedValue == "I")
                {
                    HshIn.Add("@chvEncounterNo", common.myStr(txtSearchOn.Text.Trim()));
                }
                else if (ddlSearchOn.SelectedValue == "N")
                {
                    HshIn.Add("@chvPatientName", common.myStr(txtSearchOn.Text.Trim()));
                }
            }

            if (ddlAcknowledge.SelectedValue == "A")
            {
                HshIn.Add("@bitIsOTClea", 1);
            }
            else if (ddlAcknowledge.SelectedValue == "U")
            {
                HshIn.Add("@bitIsOTClea", 0);
            }

            HshIn.Add("@BookingDateFrom", Convert.ToDateTime(dtpOTDateFrom.SelectedDate).ToString("yyyy/MM/dd"));
            HshIn.Add("@BookingDateTo", Convert.ToDateTime(dtpOTDateTo.SelectedDate).ToString("yyyy/MM/dd"));

            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientOTClearance", HshIn);

            if (ds.Tables[0].Rows.Count == 0)
            {
                CreateTable();
            }
            else
            {
                gvPatientDurgStatus.DataSource = ds.Tables[0];
                gvPatientDurgStatus.DataBind();
            }

            //gvIssueDurgDetail.DataSource = null;
            //gvIssueDurgDetail.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //public string AcknowledgePhrissueDetail(int HospId,int FacilityID, int RegID,int EncounterID, string xmlDetails, int EncodedBy)
    //{
    //   //Hashtable   HshIn = new Hashtable();
    //   //Hashtable HshOut = new Hashtable();

    //   //HshIn.Add("@intHospitalLocationID", HospId);
    //   //HshIn.Add("@intFacilityId", FacilityID);
    //   //HshIn.Add("@intRegistraionId", RegID);
    //   //HshIn.Add("@intEncounterId",EncounterID);
    //   //HshIn.Add("@intEncodedBy", EncodedBy);
    //   //HshIn.Add("@xmlPhrissueDetail", xmlDetails );
    //   //HshOut.Add("@Chrstatus", SqlDbType.VarChar);

    //   // DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //   // HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspAcknowledgePhrissueDetail", HshIn, HshOut);

    //   // return HshOut["@Chrstatus"].ToString();
    //}


    //public DataSet  getAcknowlegeDurgMain(int HospId, int FacilityId, int RegistraionID,
    //    int EncoutnerID ,int WardId,string Status)
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        Hashtable HshIn = new Hashtable();
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

    //        HshIn.Add("@intHospitalLocationId", HospId);
    //        HshIn.Add("@intFacilityId", FacilityId);
    //        HshIn.Add("@intRegistraionid", RegistraionID);
    //        HshIn.Add("@intEncouterID", EncoutnerID);
    //        HshIn.Add("@intWardid", WardId);
    //        HshIn.Add("@chrAckStatus", Status);  



    //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAckPhrMain", HshIn);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    return ds;
    //}

    //public DataSet getAcknowlegeDurgDetail(int RegistraionID,int EncoutnerID,int IssueID  )
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        Hashtable HshIn = new Hashtable();
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

    //        HshIn.Add("@intRegistraionid", RegistraionID);
    //        HshIn.Add("@intEncouterID", EncoutnerID);
    //        HshIn.Add("@intIssueId", IssueID);



    //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAckPhrDetail", HshIn);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    return ds;
    //}

    private void reBindData()
    {
        //hdnRegistrationId.Value = "";
        //hdnSelectedIndentNo.Value = "";
        //hndSelectedIsInsuranceCompany.Value = false.ToString();

        bindMainData();
        //gvIssueDurgDetail.DataSource = null;
        //gvIssueDurgDetail.DataBind(); 


    }

    public int GetCurrentWardNo(int RegistrionId, int EncounterId)
    {
        DataSet ds = new DataSet();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string strupdate = "";

        try
        {

            strupdate = "select  CurrentWardID,wM.WardNo,wM.WardName  from  Admission ad INNER  JOIN WardMaster wM on " +
                         "WM.WardId=ad.CurrentWardID  where RegistrationId=" + common.myInt(RegistrionId) + " and  EncounterId=" + EncounterId;

            ds = objDl.FillDataSet(CommandType.Text, strupdate);


            return common.myInt(ds.Tables[0].Rows[0]["CurrentWardID"]);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return 0;
        }
        finally
        {
            ds.Dispose();
            objDl = null;
        }

    }

    //gvIssueDurgDetail

    //protected void bindDetailsData(int RegistrationID,int EncounterId,int IssueID)
    //{
    //    try
    //    {
    //        DataSet ds;


    //            objPharmacy = new BaseC.clsPharmacy(sConString);
    //            objWard = new BaseC.WardManagement(); 



    //       // ds = getAcknowlegeDurgDetail(RegistrationID, EncounterId, IssueID);
    //            ds=objWard.getAcknowlegeDurgDetail(RegistrationID, EncounterId, IssueID);


    //           //Added for check Acknowleged all or Not Start Naushad
    //            DataView dv = new DataView(ds.Tables[0]); 
    //            dv.RowFilter = "AcknowledgeID=0";
    //             DataTable  dt = dv.ToTable();
    //             if (dt.Rows.Count == 0)
    //             {
    //                 ViewState["AllAck"] = 1;
    //             }
    //             else
    //             {
    //                 ViewState["AllAck"] = 0;
    //             }

    //               //Added for check Acknowleged all or Not End Naushad






    //            gvIssueDurgDetail.DataSource = ds.Tables[0];
    //            gvIssueDurgDetail.DataBind();
    //            ViewState["gvIssueDurgDetail"] = ds.Tables[0];  



    //            gvIssueDurgDetail.Visible = true;  
    //           gvIssueDurgDetail.Enabled = true; 



    //        //Added on 20-08-2014 End Naushad
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    protected void gvPatientDurgStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            gvPatientDurgStatus.Columns[1].HeaderText = common.myStr(Session["RegistrationLabelName"]);
            if (common.myStr(ddlAcknowledge.SelectedValue).Equals("U"))
            {
                gvPatientDurgStatus.Columns[5].Visible = false;
                gvPatientDurgStatus.Columns[6].Visible = false;
            }
            else if (common.myStr(ddlAcknowledge.SelectedValue).Equals("A"))
            {
                gvPatientDurgStatus.Columns[5].Visible = true;
                gvPatientDurgStatus.Columns[6].Visible = true;
            }
        }
    }

    void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("RegistrationId");
        dt.Columns.Add("EncounterId");
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("TheatreName");//Pra
        dt.Columns.Add("AdmissionDate");
        dt.Columns.Add("OTBookingAckBy");
        dt.Columns.Add("OTBookingAckDate");

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);

        gvPatientDurgStatus.DataSource = dt;
        gvPatientDurgStatus.DataBind();
    }

    protected void gvIssueDurgDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    //Find the checkbox control in header and add an attribute
        //    ((CheckBox)e.Row.FindControl("chkAllD")).Attributes.Add("onclick", "javascript:SelectAllDetails('" +
        //        ((CheckBox)e.Row.FindControl("chkAllD")).ClientID + "')");

        //    CheckBox chkAllD = (CheckBox)e.Row.FindControl("chkAllD");
        //    chkAllD.Checked = true;  
        //    if (common.myInt(ViewState["AllAck"]) == 0)
        //    {
        //        chkAllD.Visible = true;
        //        chkAllD.Checked = true;  



        //    }
        //    else
        //    {

        //        chkAllD.Visible = false;

        //    }
        //}
        //else if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    Label lblAcknowledgeBy = (Label)e.Row.FindControl("lblAcknowledgeBy");

        //    CheckBox chkAllD = (CheckBox)e.Row.FindControl("chkAllD");
        //    CheckBox chkRowD = (CheckBox)e.Row.FindControl("chkRowD");
        //    if (common.myStr(lblAcknowledgeBy.Text) == "")
        //    {
        //        chkRowD.Visible = true;
        //        chkRowD.Checked = true;   
        //    }
        //    else
        //    {
        //        chkRowD.Visible = false;
        //    }











        //}
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        reBindData();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        clearControl();
        lblMessage.Text = "";
        hdnSelectedIndentId.Value = "";
        hdnSelectedIndentNo.Value = "";
        hndSelectedIsInsuranceCompany.Value = false.ToString();
        //ddlAcknowledge.SelectedIndex = 1;
        ddlAcknowledge.SelectedIndex = 1;
        txtSearchOn.Text = "";
        // txtSearch.Text = "";
        bindMainData();
        // bindDetailsData("");
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        //setDate();
    }

    protected void gvPatientDurgStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BtnAcknowledge_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (hdnMEncounterId.Value == "" || hdnMRegistrationId.Value == "")
            {
                //raghuvir
                Alert.ShowAjaxMsg("Please Select Patient !", Page);
                return;
            }

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DataSet ds = new DataSet();

            HshIn.Add("@chrSaveEvent", "C");
            HshIn.Add("@inyHospitalLocationId", common.myInt(Session["hospitallocationId"]));
            HshIn.Add("@intFacilityId", common.myInt(Session["facilityId"]));
            HshIn.Add("@intRegistrationId", common.myInt(hdnMRegistrationId.Value));
            HshIn.Add("@intEncounterId", common.myInt(hdnMEncounterId.Value));
            HshIn.Add("@intEncodedBy", common.myInt(Session["userId"]));

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOtClearance", HshIn, HshOut);

            String strMessage = common.myStr(HshOut["@chvErrorStatus"].ToString());
            if (strMessage == "Data Saved...")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMessage;
                bindMainData();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strMessage;

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


    }

    protected void gvPatientDurgStatus_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPatientDurgStatus.PageIndex = e.NewPageIndex;

        reBindData();
    }

    protected void lnkSelect_OnClick(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row1 in gvPatientDurgStatus.Rows)
            {
                row1.BackColor = System.Drawing.Color.White;
            }

            LinkButton lnkSelect = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkSelect.NamingContainer;
            row.BackColor = System.Drawing.Color.Bisque;
            hdnMRegistrationId.Value = ((HiddenField)lnkSelect.FindControl("hdnRegistrationId")).Value;
            hdnMEncounterId.Value = ((HiddenField)lnkSelect.FindControl("hdnEncounterId")).Value;


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void ddlAcknowledge_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        //if (ViewState["gvIssueDurgDetail"] != null)
        //{
        //    DataSet ds = ViewState["gvIssueDurgDetail"];
        //    DataView dv = ds.Tables[0].DefaultView;
        //    DataTable dt = new DataTable();
        //    if (common.myInt(ddlAcknowledge.SelectedValue) == 1)
        //    {
        //        dv.RowFilter = "AcknowledgeID>0";
        //        dt = dv.ToTable();
        //    }
        //    else if (common.myInt(ddlAcknowledge.SelectedValue) == 2)
        //    {
        //        dv.RowFilter = "AcknowledgeID=0";
        //        dt = dv.ToTable();
        //    }
        //    else
        //    {
        //        dt = dv.ToTable();
        //    }
        //    gvIssueDurgDetail.DataSource = dt;
        //    gvIssueDurgDetail.DataBind();

        //    gvIssueDurgDetail.Visible = true;
        //    gvIssueDurgDetail.Enabled = true;
        //}
    }

    protected void ddlAcknowledge_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlAcknowledge.SelectedValue == "U")
        {
            BtnAcknowledge.Visible = true;
            BtnUnAcknowledge.Visible = false;
        }
        else if (ddlAcknowledge.SelectedValue == "A")
        {
            BtnAcknowledge.Visible = false;
            BtnUnAcknowledge.Visible = true;
        }
        bindMainData();
    }

    protected void BtnUnAcknowledge_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (hdnMEncounterId.Value == "" || hdnMRegistrationId.Value == "")
            {
                //raghuvir
                Alert.ShowAjaxMsg("Please Select Patient !", Page);
                return;
            }

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DataSet ds = new DataSet();

            HshIn.Add("@chrSaveEvent", "D");
            HshIn.Add("@inyHospitalLocationId", common.myInt(Session["hospitallocationId"]));
            HshIn.Add("@intFacilityId", common.myInt(Session["facilityId"]));
            HshIn.Add("@intRegistrationId", common.myInt(hdnMRegistrationId.Value));
            HshIn.Add("@intEncounterId", common.myInt(hdnMEncounterId.Value));
            HshIn.Add("@intEncodedBy", common.myInt(Session["userId"]));

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOtClearance", HshIn, HshOut);

            String strMessage = common.myStr(HshOut["@chvErrorStatus"].ToString());

            if (strMessage == "Data Saved...")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMessage;
                bindMainData();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strMessage;

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
