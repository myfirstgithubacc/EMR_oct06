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
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class FileRequestList : System.Web.UI.Page
{
   // String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    DataSet ds;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        this.MasterPageFile = "~/Include/Master/BlankMaster.master";

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpfromDate.SelectedDate = DateTime.Now.AddDays(-15);
            dtpToDate.SelectedDate = DateTime.Now;
            btnSearch_OnClick(this, null);
            BindGrid();

        }
    }
  
    protected void btnNew_Click(object sender, EventArgs e)
    {
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        if (!ValidateDate())
        {
            return;
        }

        lblMessage.Text = "";


        string fromdate = "";
        string todate = "";
        int RegistrationNo = 0;
        string EncounterNo = "";
        string PatientName = "";
        string RequestedBy = "";
        char RequestStatus = 'A';
        try
        {
            if (dtpfromDate.SelectedDate.HasValue && dtpToDate.SelectedDate.HasValue)
            {
                fromdate = (Convert.ToDateTime(dtpfromDate.SelectedDate)).ToString("yyyy-MM-dd");
                todate = (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("yyyy-MM-dd");
            }
            if (ddlSearchCriteria.SelectedValue == "RN")
            {
                RegistrationNo = common.myInt(txtRegNo.Text);
            }
            else if (ddlSearchCriteria.SelectedValue == "EN")
            {
                EncounterNo = txtSearch.Text.Trim();
            }
            else if (ddlSearchCriteria.SelectedValue == "PN")
            {
                PatientName = txtSearch.Text.Trim();
            }
            RequestStatus = Convert.ToChar(ddlStatus.SelectedValue.ToString());
            bindGrid(0, 0, fromdate, todate, RegistrationNo, EncounterNo, PatientName, RequestedBy, RequestStatus);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);

        }
        finally
        {
            fromdate = string.Empty;
            todate = string.Empty;
            RegistrationNo = 0;
            EncounterNo = string.Empty;
            PatientName = string.Empty;
            RequestedBy = string.Empty;

        }
        //BindGrid();
    }
    private void bindGrid(int RegistrationId, int EncounterId, string fromdate, string todate, int RegistrationNo, string EncounterNo, string PatientName, string RequestedBy, char RequestStatus)
    {

       // BaseC.EMR objEMR = new BaseC.EMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRFileRequest";
            APIRootClass.GetEMRFileRequest objRoot = new global::APIRootClass.GetEMRFileRequest();
            objRoot.UserId = common.myInt(Session["UserID"]);
            objRoot.RegistrationId = RegistrationId;
            objRoot.iLoginFacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncounterId = EncounterId;
            objRoot.fromdate = fromdate;
            objRoot.todate = todate;
            objRoot.RegistrationNo = RegistrationNo;
            objRoot.EncounterNo = EncounterNo;
            objRoot.PatientName = PatientName;
            objRoot.RequestedBy = RequestedBy;
            objRoot.RequestStatus = RequestStatus;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

           // ds = objEMR.GetEMRFileRequest(common.myInt(Session["UserID"]), RegistrationId, common.myInt(Session["FacilityId"]), EncounterId, fromdate, todate, RegistrationNo, EncounterNo, PatientName, RequestedBy, RequestStatus);


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds.Tables[0];
                gvDetails.DataBind();
            }
            else
            {
                BindBlankgrid();
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
            //objEMR = null;
            ds.Dispose();
        }

    }
    public void BindGrid()
    {
        // BaseC.EMR objEMR =  new BaseC.EMR(sConString);
        DataSet ds = new DataSet();
        
        string dtfrom = (Convert.ToDateTime(dtpfromDate.SelectedDate)).ToString("yyyy-MM-dd");
        string dtto = (Convert.ToDateTime(dtpToDate.SelectedDate)).ToString("yyyy-MM-dd");
        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRFileRequest";
        APIRootClass.GetEMRFileRequest objRoot = new global::APIRootClass.GetEMRFileRequest();
        objRoot.UserId = common.myInt(Session["UserID"]);
        objRoot.RegistrationId = 0;
        objRoot.iLoginFacilityId = common.myInt(Session["FacilityId"]);
        objRoot.EncounterId = 0;
        objRoot.fromdate = dtfrom;
        objRoot.todate = dtto;       
        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);
       // ds = objEMR.GetEMRFileRequest(Convert.ToInt32(Session["UserId"]), 0, common.myInt(Session["FacilityID"]), 0, dtfrom,dtto);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDetails.DataSource = ds;
            gvDetails.DataBind();
        }
        else
        {
            BindBlankgrid();
        }
    }

    protected void BindBlankgrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("Remarks");
        dt.Columns.Add("RequestedBy");
        dt.Columns.Add("EncodedDate");
        dt.Columns.Add("PermissionStatus");
        dt.Columns.Add("RequestAcknowledged");
        dt.Columns.Add("Validupto");
        dt.Columns.Add("RequestID");

        DataRow dr = dt.NewRow();

        for (int i = 0; i < 1; i++)
        {
            dr["RegistrationNo"] = DBNull.Value;
            dr["EncounterNo"] = DBNull.Value;
            dr["PatientName"] = DBNull.Value;
            dr["Remarks"] = DBNull.Value;
            dr["RequestedBy"] = DBNull.Value;
            dr["EncodedDate"] = DBNull.Value;
            dr["PermissionStatus"] = DBNull.Value;
            dr["RequestAcknowledged"] = DBNull.Value;
            dr["Validupto"] = DBNull.Value;
            dr["RequestID"] = DBNull.Value;
        }
        dt.Rows.Add(dr);
        gvDetails.DataSource = dt;
        gvDetails.DataBind();
    }

  
   
    protected void gvDetails_OnItemDataBound(object source, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            HiddenField hfRequest = (HiddenField)item.FindControl("hfRequest");
            LinkButton linkDelete = (LinkButton)item.FindControl("linkDelete");

            if (common.myStr(hfRequest.Value) == "False")
            {
                linkDelete.Visible = true;
            }
            else
            {
                linkDelete.Visible = false;
            }
        }
    }
    protected void gvDetails_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        btnSearch_OnClick(this, null);
        //BindGrid();
    }

    protected void gvDetails_OnItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if(e.CommandName == "DeleteRec")
            {
                lblMessage.Text = "";
                GridDataItem item = e.Item as GridDataItem;
                HiddenField hfRequest = (HiddenField)item.FindControl("hfRequest");

                if (common.myStr(hfRequest.Value) == "False")
                 {
                     string strquery = "update EMRFileRequest set Active=0,LastChangedBy=" + common.myStr(Session["UserId"]) + ", LastChangedDate=getutcdate() where isnull(PermissionGranted,0)=0 and isnull(RequestAcknowledged,0)=0 and RequestID=" + common.myStr(e.CommandArgument);
                   
                    //  DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/ExecuteQuery";
                    APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
                    objRoot.Query = strquery;
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    int intResult = common.myInt(sValue);
                    

                   // int intResult = objDl.ExecuteNonQuery(CommandType.Text, strquery);
                     if (intResult == 0)
                     {
                         btnSearch_OnClick(this, null);
                         lblMessage.Text = "Request has been deleted.";
                         lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                         lblMessage.Visible = true;
                         //BindGrid();
                     }
                 }
                 else
                 {
                     lblMessage.Text = "This Request cannot be deleted.";
                     lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                       
                 }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
        
    }
    protected bool ValidateDate()
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        
        if (dtpfromDate.SelectedDate.HasValue && !dtpToDate.SelectedDate.HasValue)
        {
            lblMessage.Text = "Please Fill Date To.";
            return false;
        }
        if (!dtpfromDate.SelectedDate.HasValue && dtpToDate.SelectedDate.HasValue)
        {
            lblMessage.Text = "Please Fill Date From.";
            return false;
        }
        if (Convert.ToDateTime(dtpfromDate.SelectedDate) > Convert.ToDateTime(dtpToDate.SelectedDate))
        {
            lblMessage.Text = "From date must be smaller or equal to To date.";
            return false;
        }
        return true;
    }
    protected void ddlSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearchCriteria.SelectedValue == "RN")
        {
            txtSearch.Visible = false;
            txtRegNo.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            txtRegNo.Visible = false;
        }
    }
    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        bindGrid(0, 0, "", "", 0, "", "", "", 'A');
        ddlSearchCriteria.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        dtpfromDate.SelectedDate = null;
        txtSearch.Visible = false;
        txtRegNo.Visible = true;
        dtpToDate.SelectedDate = null;
        ddlStatus.SelectedIndex = 0;
        lblMessage.Text = "";
    }
}
