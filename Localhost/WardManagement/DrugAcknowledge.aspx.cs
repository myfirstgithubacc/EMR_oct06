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

public partial class EMR_Medication_MedicationDispense : System.Web.UI.Page
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
        this.MasterPageFile = "~/Include/Master/BlankMaster.master";

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
            if (Request.QueryString["Regno"] != null)
            {
                BindPatientHiddenDetails();


            }

            ViewState["PatientOPIPType"] = common.myStr("O");
            ViewState["PT"] = common.myStr(Request.QueryString["PT"]);

            // clearControl();
            // bindControl();
            bindWardControl();
            //ddlWard.SelectedValue=GetCurrentWardNo(common.myInt(Request.QueryString["RegId"]), common.myInt(Request.QueryString["EncounterId"])).ToString();
            ddlWard.SelectedValue = "0";
            bindMainData();
            //  bindDetailsData("");



            //if (common.myStr(ViewState["PT"]) == "P")
            //{
            //    lblHeader.Text = "Prescription";
            //    tdDispenseBy.Visible = false;
            //    BtnAcknowledge.Visible = false;

            //    gvIssueDurgDetail.Enabled = false;
            //}
        }
    }

    private void clearControl()
    {
        //txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
        //txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        //txtToDate.SelectedDate = DateTime.Now;
        //txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        //ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
        ddlAcknowledge.SelectedValue = "ALL";
        ddlWard.SelectedValue = "0";
        bindMainData();


    }




    void bindMainData()
    {
        try
        {
            //setDate();
            string PrescNo = "";
            string RegNo = "";
            string EncNo = "";
            string PatientName = "";

            DataView dvData = new DataView();
            DataSet ds = new DataSet();
            objEMR = new BaseC.clsEMR(sConString);

            objWard = new BaseC.WardManagement();

            ds = objWard.getAcknowlegeDurgMain(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                        common.myInt(Request.QueryString["RegID"]), common.myInt(Request.QueryString["EncounterId"]), common.myInt(ddlWard.SelectedValue),
                        common.myStr(ddlAcknowledge.SelectedValue).ToUpper().Equals("ALL") ? string.Empty : common.myStr(ddlAcknowledge.SelectedValue),
                        common.myStr(txtIssueNo.Text));

            DataView DV = ds.Tables[0].DefaultView;

            if (ds.Tables[0].Rows.Count == 0)
            {
                CreateTable();
            }
            else
            {
                gvPatientDurgStatus.DataSource = ds.Tables[0];
                gvPatientDurgStatus.DataBind();
            }

            gvIssueDurgDetail.DataSource = null;
            gvIssueDurgDetail.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public string AcknowledgePhrissueDetail(int HospId, int FacilityID, int RegID, int EncounterID, string xmlDetails, int EncodedBy)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HshIn.Add("@intHospitalLocationID", HospId);
        HshIn.Add("@intFacilityId", FacilityID);
        HshIn.Add("@intRegistraionId", RegID);
        HshIn.Add("@intEncounterId", EncounterID);
        HshIn.Add("@intEncodedBy", EncodedBy);
        HshIn.Add("@xmlPhrissueDetail", xmlDetails);
        HshOut.Add("@Chrstatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspAcknowledgePhrissueDetail", HshIn, HshOut);

        return HshOut["@Chrstatus"].ToString();
    }









    void BindPatientHiddenDetails()
    {
        if (Session["PatientDetailString"] != null)
        {
            lblPatientDetail.Text = Session["PatientDetailString"].ToString();
        }

    }


    public DataSet getAcknowlegeDurgMain(int HospId, int FacilityId, int RegistraionID,
        int EncoutnerID, int WardId, string Status)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistraionid", RegistraionID);
            HshIn.Add("@intEncouterID", EncoutnerID);
            HshIn.Add("@intWardid", WardId);
            HshIn.Add("@chrAckStatus", Status);



            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAckPhrMain", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getAcknowlegeDurgDetail(int RegistraionID, int EncoutnerID, int IssueID)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intRegistraionid", RegistraionID);
            HshIn.Add("@intEncouterID", EncoutnerID);
            HshIn.Add("@intIssueId", IssueID);



            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAckPhrDetail", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    private void bindWardControl()
    {
        try
        {
            DataSet ds = new DataSet();
            DataSet dsWard = new DataSet();

            //BaseC.ATD objadt = new BaseC.ATD(sConString);
            //ds = objadt.getWardMaster(common.myInt(common.myInt(Session["HospitalLocationID"])));


            BaseC.EMRBilling objPatientTransfer = new BaseC.EMRBilling(sConString);
            dsWard = objPatientTransfer.getPatientTransefers(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["EncounterId"]), "uspGetPatientTransferDetails", 0);
            DataView dv = new DataView(dsWard.Tables[0]);
            dv.RowFilter = "ToWardId>0";
            //dv.RowFilter = "select DISTINCT ToWardId";
            DataTable dtWard = dv.ToTable(true, "ToWardId", "ToWardName");



            //ddlWard.DataSource = ds.Tables[0];
            //ddlWard.DataTextField = "WardName";
            //ddlWard.DataValueField = "WardId";
            //ddlWard.DataBind();


            ddlWard.DataSource = dtWard;
            ddlWard.DataTextField = "ToWardName";
            ddlWard.DataValueField = "ToWardId";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem("", "0"));

            //ddlWard.SelectedIndex = 0;
            // ddlWard.SelectedValue= "0";
            //ddlAcknowledge.SelectedValue = "2";  


        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    //Added on 20-08-2014 End Naushad Ali





    private void reBindData()
    {
        hdnSelectedIndentId.Value = "";
        hdnSelectedIndentNo.Value = "";
        hndSelectedIsInsuranceCompany.Value = false.ToString();

        bindMainData();
        gvIssueDurgDetail.DataSource = null;
        gvIssueDurgDetail.DataBind();


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

    protected void bindDetailsData(int RegistrationID, int EncounterId, int IssueID)
    {
        try
        {
            DataSet ds;


            objPharmacy = new BaseC.clsPharmacy(sConString);
            objWard = new BaseC.WardManagement();



            // ds = getAcknowlegeDurgDetail(RegistrationID, EncounterId, IssueID);
            ds = objWard.getAcknowlegeDurgDetail(RegistrationID, EncounterId, IssueID);


            //Added for check Acknowleged all or Not Start Naushad
            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "AcknowledgeID=0";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count == 0)
            {
                ViewState["AllAck"] = 1;
            }
            else
            {
                ViewState["AllAck"] = 0;
            }

            //Added for check Acknowleged all or Not End Naushad






            gvIssueDurgDetail.DataSource = ds.Tables[0];
            gvIssueDurgDetail.DataBind();
            ViewState["gvIssueDurgDetail"] = ds.Tables[0];



            gvIssueDurgDetail.Visible = true;
            gvIssueDurgDetail.Enabled = true;



            //Added on 20-08-2014 End Naushad

            foreach (GridViewRow gr in gvIssueDurgDetail.Rows)
            {

            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void gvPatientDurgStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {



        if (e.Row.RowType == DataControlRowType.DataRow)
        {









        }
    }


    void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("RegistrationId");
        dt.Columns.Add("EncounterId");
        dt.Columns.Add("IssueId");
        dt.Columns.Add("FacilityId");
        dt.Columns.Add("WardId");
        dt.Columns.Add("WardName");
        dt.Columns.Add("LoginStoreId");
        dt.Columns.Add("StoreID");
        dt.Columns.Add("IssueNo");
        dt.Columns.Add("IssuedByID");
        dt.Columns.Add("IssuedBy");
        dt.Columns.Add("IssuedDate");
        dt.Columns.Add("IndentId");
        dt.Columns.Add("OrderNo");
        dt.Columns.Add("OrderDate");
        dt.Columns.Add("AdvisingDoctorId");
        dt.Columns.Add("AdvisingDoctor");
        dt.Columns.Add("OrderByID");
        dt.Columns.Add("OrderBy");



        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);

        gvPatientDurgStatus.DataSource = dt;
        gvPatientDurgStatus.DataBind();
    }







    protected void gvIssueDurgDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAllD")).Attributes.Add("onclick", "javascript:SelectAllDetails('" +
                ((CheckBox)e.Row.FindControl("chkAllD")).ClientID + "')");

            CheckBox chkAllD = (CheckBox)e.Row.FindControl("chkAllD");
            chkAllD.Checked = true;
            if (common.myInt(ViewState["AllAck"]) == 0)
            {
                chkAllD.Visible = true;
                chkAllD.Checked = true;



            }
            else
            {

                chkAllD.Visible = false;

            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblAcknowledgeBy = (Label)e.Row.FindControl("lblAcknowledgeBy");
            HiddenField hdnIssueDetailsId = (HiddenField)e.Row.FindControl("hdnIssueDetailsId");
            TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");

            CheckBox chkAllD = (CheckBox)e.Row.FindControl("chkAllD");
            CheckBox chkRowD = (CheckBox)e.Row.FindControl("chkRowD");
            if (common.myStr(lblAcknowledgeBy.Text) == "")
            {
                chkRowD.Visible = true;
                chkRowD.Checked = true;
            }
            else
            {
                chkRowD.Visible = false;
            }



        }
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
            if (gvPatientDurgStatus != null)
            {
                if (gvPatientDurgStatus.SelectedIndex > -1)
                {
                    HiddenField hdnIndentId = (HiddenField)gvPatientDurgStatus.SelectedRow.Cells[Convert.ToByte(1)].FindControl("hdnIndentId");
                    HiddenField hdnRegistrationId = (HiddenField)gvPatientDurgStatus.SelectedRow.Cells[Convert.ToByte(1)].FindControl("hdnRegistrationId");
                    HiddenField hdnEncounterId = (HiddenField)gvPatientDurgStatus.SelectedRow.Cells[Convert.ToByte(1)].FindControl("hdnEncounterId");
                    HiddenField hdnIssueId = (HiddenField)gvPatientDurgStatus.SelectedRow.Cells[Convert.ToByte(1)].FindControl("hdnIssueId");
                    ViewState["RegistrationId"] = hdnRegistrationId.Value;
                    ViewState["EncounterId"] = hdnEncounterId.Value;
                    bindDetailsData(common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value), common.myInt(hdnIssueId.Value));






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

    protected void BtnAcknowledge_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            strXML = new StringBuilder();
            coll = new ArrayList();



            foreach (GridViewRow dataItem in gvIssueDurgDetail.Rows)
            {
                CheckBox chkRowD = (CheckBox)dataItem.FindControl("chkRowD");
                if (chkRowD.Checked && chkRowD.Visible)
                {
                    HiddenField hdnIssueDetailsId = (HiddenField)dataItem.FindControl("hdnIssueDetailsId");
                    HiddenField hdnIssueID = (HiddenField)dataItem.FindControl("hdnIssueID");
                    TextBox txtdispenseqty = (TextBox)dataItem.FindControl("txtdispenseqty");
                    TextBox txtPreAuthorization = (TextBox)dataItem.FindControl("txtPreAuthorization");
                    TextBox txtIssuedStrength = (TextBox)dataItem.FindControl("txtIssuedStrength");

                    coll.Add(common.myInt(hdnIssueDetailsId.Value));//DetailsId int, 
                    coll.Add(common.myInt(hdnIssueID.Value));//txtdispenseqty varchar(20)
                    //coll.Add(common.myStr(txtPreAuthorization.Text).Trim());//PreAuthorization varchar(20)
                    //coll.Add(common.myStr(txtIssuedStrength.Text).Trim());//txtIssuedStrength varchar(15)


                    strXML.Append(common.setXmlTable(ref coll));
                }
            }

            if (strXML.ToString() == "")
            {
                lblMessage.Text = "Durg not selected !";
                return;
            }
            else
            {
                //IsValidPassword();
                SaveData();
            }




        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


    }
    private void SaveData()
    {
        try
        {
            strXML = new StringBuilder();
            coll = new ArrayList();

            foreach (GridViewRow dataItem in gvIssueDurgDetail.Rows)
            {
                CheckBox chkRowD = (CheckBox)dataItem.FindControl("chkRowD");
                if (chkRowD.Checked && chkRowD.Visible)
                {
                    HiddenField hdnIssueDetailsId = (HiddenField)dataItem.FindControl("hdnIssueDetailsId");
                    HiddenField hdnIssueID = (HiddenField)dataItem.FindControl("hdnIssueID");
                    TextBox txtdispenseqty = (TextBox)dataItem.FindControl("txtdispenseqty");
                    TextBox txtPreAuthorization = (TextBox)dataItem.FindControl("txtPreAuthorization");
                    TextBox txtIssuedStrength = (TextBox)dataItem.FindControl("txtIssuedStrength");
                    TextBox txtRemarks = (TextBox)dataItem.FindControl("txtRemarks");


                    coll.Add(common.myInt(hdnIssueDetailsId.Value));//DetailsId int, 
                    strXML.Append("<Table1><c1>");
                    strXML.Append(common.myInt(hdnIssueDetailsId.Value));
                    strXML.Append("</c1><c2>");
                    strXML.Append(common.myInt(hdnIssueID.Value));
                    strXML.Append("</c2><c3>");
                    strXML.Append(common.myStr(txtRemarks.Text));
                    strXML.Append("</c3></Table1>");

                }
            }

            if (strXML.ToString() == "")
            {
                lblMessage.Text = "Durg not selected !";
                return;
            }

            objEMR = new BaseC.clsEMR(sConString);
            string strMsg = "";
            //strMsg = objEMR.MedicationDispense(common.myInt(Session["HospitalLocationId"]), common.myInt(hdnSelectedIndentId.Value), common.myInt(ddlDispenseBy.SelectedValue), strXML.ToString(), common.myInt(Session["UserId"]));
            objWard = new BaseC.WardManagement();
            //strMsg = AcknowledgePhrissueDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]),common.myInt(ViewState["EncounterId"]),strXML.ToString(),common.myInt(Session["UserId"]));
            strMsg = objWard.AcknowledgePhrissueDetail(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), strXML.ToString(), common.myInt(Session["UserId"]));

            if (strMsg.Contains("updated"))
            {
                lblMessage.Text = "Record updated !";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);


            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            }

            bindMainData();


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


    protected void gvPatientDurgStatus_OnRowCommand(object source, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "PatientDetails")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                LinkButton lnkPatientName = (LinkButton)row.FindControl("lnkPatientName");
                Label lblFacilityName = (Label)row.FindControl("lblFacilityName");
                Label lblRegistrationNo = (Label)row.FindControl("lblRegistrationNo");
                //  Label lblFacilityName = (Label)row.FindControl("lblFacilityName");
                if (lblRegistrationNo.Text != "")
                {
                    RadWindow1.NavigateUrl = "~/LIS/Phlebotomy/PatientDetails.aspx?RegNo=" + common.myStr(lblRegistrationNo.Text) + "&PName=" + lnkPatientName.Text + "";//&facility=" + common.myStr(lblFacilityName.Text);

                    RadWindow1.Height = 300;
                    RadWindow1.Width = 900;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;

                }
                else
                {
                    Alert.ShowAjaxMsg("Reg# No. Not Exist", Page);
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














    protected void ddlAcknowledge_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        if (ViewState["gvIssueDurgDetail"] != null)
        {
            DataSet ds = (DataSet)ViewState["gvIssueDurgDetail"];
            DataView dv = ds.Tables[0].DefaultView;
            DataTable dt = new DataTable();
            if (common.myInt(ddlAcknowledge.SelectedValue) == 1)
            {
                dv.RowFilter = "AcknowledgeID>0";
                dt = dv.ToTable();
            }
            else if (common.myInt(ddlAcknowledge.SelectedValue) == 2)
            {
                dv.RowFilter = "AcknowledgeID=0";
                dt = dv.ToTable();
            }
            else
            {
                dt = dv.ToTable();
            }
            gvIssueDurgDetail.DataSource = dt;
            gvIssueDurgDetail.DataBind();

            gvIssueDurgDetail.Visible = true;
            gvIssueDurgDetail.Enabled = true;
        }
    }
}
