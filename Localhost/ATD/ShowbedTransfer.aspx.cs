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


public partial class ATD_ShowbedTransfer : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    String sqlstr = "";
    BaseC.ParseData bc = new BaseC.ParseData();
    BaseC.ATD objbc;
    DataSet ds;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        objbc = new BaseC.ATD(sConString);
        ds = new DataSet();
        if (!IsPostBack)
        {
            dtpfromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpfromDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpfromDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));

            dtpToDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpToDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));



            if (Request.QueryString["Regno"] != "")
            {
                hdnregno.Value = Request.QueryString["Regno"].ToString().Trim();
                hdnencno.Value = Request.QueryString["encno"].ToString().Trim();
                txtregno.Text = hdnregno.Value;
                ds = objbc.GetRegistrationId(common.myInt(hdnregno.Value));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnregId.Value = ds.Tables[0].Rows[0]["id"].ToString().Trim();
                }
                btnfilter_Click(null, null);
            }
        }
    }

    protected void BidBedTransfer()
    {
        try
        {
            objbc = new BaseC.ATD(sConString);
            ds = new DataSet();


            if (txtregno.Text != "")
            {
                ds = objbc.GetRegistrationId(common.myInt(txtregno.Text.Trim()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnregId.Value = ds.Tables[0].Rows[0]["id"].ToString().Trim();
                }
                else
                {
                    if (common.myStr(txtregno.Text) != "")
                        lblmsg.Text = "Invalid UHID !";
                }

            }
            DateTime dtfromdate = common.myDate(dtpfromDate.SelectedDate);
            DateTime dttodate = common.myDate(dtpToDate.SelectedDate);
            ds = objbc.GetBedTransfer(common.myInt(Session["HospitallocationId"]),
                common.myInt(hdnregId.Value), common.myStr(hdnencno.Value),
                common.myStr(dtfromdate.ToString("yyyy/MM/dd")),
                    common.myStr(dttodate.ToString("yyyy/MM/dd")), common.myInt(Session["FacilityId"]),common.myStr(rdoBedReq.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblmsg.Text = "";
                ds.Tables[0].DefaultView.Sort = "TransferDate ASC";
                gvBedtorTransfer.DataSource = ds.Tables[0].DefaultView.ToTable();
                gvBedtorTransfer.DataBind();
            }
            else
            {
                lblmsg.Text = "";
                gvBedtorTransfer.DataSource = ds.Clone();
                gvBedtorTransfer.DataBind();



            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);

        }
    }
    protected void btnfilter_Click(object sender, EventArgs e)
    {
        try
        {
            hdnencno.Value = "";
            hdnregId.Value = "";
            ds = objbc.GetRegistrationId(common.myInt(txtregno.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                hdnregId.Value = ds.Tables[0].Rows[0]["id"].ToString().Trim();
            }
            BidBedTransfer();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);

        }
    }
    protected void btnclearfilter_Click(object sender, EventArgs e)
    {
        try
        {
            txtregno.Text = "";
            hdnencno.Value = "";
            hdnregId.Value = "";
           
            dtpfromDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            dtpToDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            BidBedTransfer();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);

        }
    }
}
