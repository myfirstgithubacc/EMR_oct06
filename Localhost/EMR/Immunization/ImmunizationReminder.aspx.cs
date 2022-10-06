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
using System.Net;

public partial class EMR_Immunization_ImmunizationReminder : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindImmunizatiionReminder();
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
            tblDateRange.Visible = false;
        }
    }
    private enum GridColOrder :byte 
    {
        COL0 = 0,
        COL1 = 1,
        COL2 = 2,
        COL3 = 3,
        COL4 = 4,
        COL5 = 5,
        COL6 = 6,
        COL7 = 7,

    }
    private void BindImmunizatiionReminder()
    {
        ds = new DataSet();
        

        BaseC.EMRImmunization immu = new BaseC.EMRImmunization(sConString);
        ds = immu.GetImmunizationReminder(Convert.ToInt32(Session["HospitalLocationId"]), Convert.ToInt16(ddlOption.SelectedValue), 
            ddlOption.SelectedValue=="4" ? Convert.ToDateTime(dtpFromDate.SelectedDate.Value.Date).ToString("yyyy/MM/dd"): null,
            ddlOption.SelectedValue=="4" ? Convert.ToDateTime(dtpToDate.SelectedDate.Value.Date).ToString("yyyy/MM/dd"): null,common.myInt(Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvReminder.DataSource = ds.Tables[0];
            gvReminder.DataBind();
            MergeGrid(gvReminder);
        }
        else
        {
            BindBlankGrid();
        }
       
    }


    private void BindBlankGrid()
    {

        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();

        try
        {

            dr["RegistrationNo"] = string.Empty;
            dr["PatientName"] = string.Empty;
            dr["ImmunizationId"] = 0;
            dr["RegistrationId"] = 0;
            dr["ImmunizationName"] = string.Empty;
            dr["ImmunizationDueDate"] = string.Empty;
            dr["MobileNo"] = string.Empty;
            dr["Email"] = string.Empty;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvReminder.DataSource = dt;
            gvReminder.DataBind();

           
        }

        catch (Exception ex)
        {
           
            objException.HandleException(ex);

        }
        finally
        {
            dt.Dispose();
        }

    }

    protected DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        try
        {

            dt.Columns.Add("RegistrationNo", typeof(string));
            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("ImmunizationId", typeof(int));
            dt.Columns.Add("RegistrationId", typeof(int));
            dt.Columns.Add("ImmunizationName", typeof(string));
            dt.Columns.Add("ImmunizationDueDate", typeof(string));
            dt.Columns.Add("MobileNo", typeof(string));
            dt.Columns.Add("Email", typeof(string));
           
            return dt;
        }
        catch (Exception ex)
        {
           
            objException.HandleException(ex);
            return dt;
        }

    }
    private void MergeGrid(GridView gvDue)
    {
        int count = 2;
        for (int rowIndex = gvDue.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow gvRow = gvDue.Rows[rowIndex];
            GridViewRow gvPreviousRow = gvDue.Rows[rowIndex + 1];
            Label lblCRegistrationNo = (Label)gvRow.Cells[0].FindControl("lblRegistrationNo");
            Label lblPRegistrationNo = (Label)gvPreviousRow.Cells[0].FindControl("lblRegistrationNo");

            Label lblCDueDate = (Label)gvRow.Cells[0].FindControl("lblDueDate");
            Label lblPDueDate = (Label)gvPreviousRow.Cells[0].FindControl("lblDueDate");

            Label lblCImmunizationName = (Label)gvRow.Cells[0].FindControl("lblImmunizationName");
            Label lblPImmunizationName = (Label)gvPreviousRow.Cells[0].FindControl("lblImmunizationName");


            if (lblCRegistrationNo.Text == lblPRegistrationNo.Text)
            {
                gvRow.Cells[0].RowSpan = gvDue.Rows[rowIndex + 1].Cells[0].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[0].RowSpan + 1;
                gvDue.Rows[rowIndex + 1].Cells[0].Visible = false;

                gvRow.Cells[1].RowSpan = gvDue.Rows[rowIndex + 1].Cells[1].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[1].RowSpan + 1;
                gvDue.Rows[rowIndex + 1].Cells[1].Visible = false;

                gvRow.Cells[4].RowSpan = gvDue.Rows[rowIndex + 1].Cells[4].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[4].RowSpan + 1;
                gvDue.Rows[rowIndex + 1].Cells[4].Visible = false;

                gvRow.Cells[5].RowSpan = gvDue.Rows[rowIndex + 1].Cells[5].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[5].RowSpan + 1;
                gvDue.Rows[rowIndex + 1].Cells[5].Visible = false;
            }
            if (lblCDueDate.Text == lblPDueDate.Text && lblCRegistrationNo.Text == lblPRegistrationNo.Text)
            {
                gvRow.Cells[3].RowSpan = gvDue.Rows[rowIndex + 1].Cells[3].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[3].RowSpan + 1;
                gvDue.Rows[rowIndex + 1].Cells[3].Visible = false;

                gvRow.Cells[6].RowSpan = gvDue.Rows[rowIndex + 1].Cells[6].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[6].RowSpan + 1;
                gvDue.Rows[rowIndex + 1].Cells[6].Visible = false;

                gvRow.Cells[2].RowSpan = gvDue.Rows[rowIndex + 1].Cells[2].RowSpan < 2 ? 2 : gvDue.Rows[rowIndex + 1].Cells[2].RowSpan + 1;
                gvDue.Rows[rowIndex + 1].Cells[2].Visible = false;

                lblCImmunizationName.Text = lblCImmunizationName.Text + ",&nbsp;&nbsp;&nbsp; " + lblPImmunizationName.Text;
            }
            else
            {
                lblTotalCount.Text = "Total Records: " + count++;
            }
        }
        if (gvDue.Rows.Count == 1)
        {
            lblTotalCount.Text = "Total Records: " + 1;
        }
    }
    protected void gvReminder_OnDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.BackColor = System.Drawing.Color.White;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSend =(CheckBox)e.Row.FindControl("chkSend");
            Label lblMobileNo = (Label)e.Row.FindControl("lblMobileNo");
            if (lblMobileNo.Text == "")
            {
                chkSend.Visible = false;
            }
            else
            {
                chkSend.Visible = true;
            }
        }
    }
    protected void ddlOption_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOption.SelectedValue == "4")
        {
            tblDateRange.Visible = true;
        }
        else
        {
            tblDateRange.Visible = false;
            //dtpFromDate.SelectedDate = null;
            //dtpToDate.SelectedDate = null;
        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        if (ddlOption.SelectedValue == "4" && dtpToDate.SelectedDate < dtpFromDate.SelectedDate)
        {
            lblmsg.Text= "To date can't be before from date";
            return;
        }
        BindImmunizatiionReminder();
    }
    protected void btnSend_OnClick(object sender, EventArgs e)
    {
        try
        {
            //if (txtmono.Text != String.Empty && txtmsg.Text != String.Empty)
            //{
                SendMessage();
                //txtmono.Text = String.Empty;
                //txtmsg.Text = String.Empty;
                //lblmsg.Visible = true;
                //lblmsg.Text = "Message Sented";
            //}
            //else
            //{
            //    lblmsg.Visible = true;
            //    lblmsg.Text = "Not Sented";
            //}
        }
        catch (Exception EX)
        {
            
            throw;
        }
    }
    private void SendMessage()
    {
        string _msg = "Test";
        //we creating the necessary URL string:
        string ozSURL = "http://127.0.0.1"; //where Ozeki NG SMS Gateway is running
        string ozSPort = "9501"; //port number where Ozeki NG SMS Gateway is listening
        string ozUser = HttpUtility.UrlEncode("admin"); //username for successful login
        string ozPassw = HttpUtility.UrlEncode("123456"); //user's password
        string ozMessageType = "SMS:TEXT"; //type of message
        string ozRecipients = HttpUtility.UrlEncode("+91" + "9015318884"); //who will get the message
        string ozMessageData = HttpUtility.UrlEncode(_msg); //body of message

        string createdURL = ozSURL + ":" + ozSPort + "/httpapi" +
            "?action=sendMessage" +
            "&username=" + ozUser +
            "&password=" + ozPassw +
            "&messageType=" + ozMessageType +
            "&recipient=" + ozRecipients +
            "&messageData=" + ozMessageData;
        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(createdURL);

        //Get response from Ozeki NG SMS Gateway Server and read the answer
        HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
        System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
        string responseString = respStreamReader.ReadToEnd();
        respStreamReader.Close();
        myResp.Close();
    }
    protected void gvReminder_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[(byte)GridColOrder.COL5].Visible = false;
            e.Row.Cells[(byte)GridColOrder.COL6].Visible = false;
            
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[(byte)GridColOrder.COL5].Visible = false;
            e.Row.Cells[(byte)GridColOrder.COL6].Visible = false;
          

        }
    }
}
