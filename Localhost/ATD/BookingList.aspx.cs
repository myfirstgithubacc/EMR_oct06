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

public partial class ATD_BookingList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                dtpFromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtpFromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
                dtpToDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtpToDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
                dtpFromdate.SelectedDate = DateTime.Now;
                dtpToDate.SelectedDate = DateTime.Now;
                BindControl();
                ShowBookingDetails();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
            btnPrint.Visible = false;
        }
    }

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        ShowBookingDetails();
    }
    public void BindControl()
    {
        DataSet ds = new DataSet();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            ds = objadt.GetAdmissionBookingType(0, "", "");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBookingType.DataSource = ds.Tables[0];
                ddlBookingType.DataTextField = "BookingTypeName";
                ddlBookingType.DataValueField = "BookingTypeValue";
                ddlBookingType.DataBind();
            }
            ddlBookingType.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlBookingType.SelectedIndex = 0;
        }
        catch(Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
        }
        finally
        {
            objadt = null;
            ds.Dispose();
        }
    }
    protected void gvBooking_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvBooking.CurrentPageIndex = e.NewPageIndex;
        //int FilterType = 1; // For OT Order Grid (Top)
        //int OrderId = 0;
        ShowBookingDetails();

    }
    protected void gvBooking_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Print")
            {

                string BookingNo = " ";
                string RegNo = " ";
                string DocName = " ";
                string BookingType = " ";
                string BookingId = " ";

                lblMsg.Text = "";
                Label lblBookingNo = (Label)e.Item.FindControl("lblBookingNo");
                Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblDoctorName = (Label)e.Item.FindControl("lblDoctorName");
                Label lblBookingId = (Label)e.Item.FindControl("lblBookingId");
                HiddenField hdnBookingtype = (HiddenField)e.Item.FindControl("hdnBookingtype");

                BookingId = common.myStr(lblBookingId.Text);

                //if (ddlSearchBy.SelectedValue == "BN")
                //{
                BookingNo = common.myStr(lblBookingNo.Text);
                //}
                //else if (ddlSearchBy.SelectedValue == "RN")
                //{
                RegNo = common.myStr(lblRegistrationNo.Text).Trim();
                //}
                //else if (ddlSearchBy.SelectedValue == "BD")
                //{
                DocName = common.myStr("%" + lblDoctorName.Text + "%").Trim();
                //}
                //if (ddlBookingType.SelectedValue != "")
                //{
                BookingType = common.myStr(hdnBookingtype.Value).Trim();
                //}
                string FromDate = dtpFromdate.SelectedDate.Value.ToString("yyyy/MM/dd");
                string ToDate = dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd");


                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + FromDate
                    + "&Todate=" + ToDate
                    + "&BI=" + BookingId + "&BN=" + BookingNo
                    + "&RN=" + RegNo
                     //+ "&DN=" + DocName
                     + "&DN=" + "%%"
                    + "&ReportType=" + ddlReportType.SelectedValue
                    + "&BT="
                    + BookingType + "&ReportName=BookingList"
                    + "&RT=" + ddlBookingType.SelectedValue;
                RadWindowForNew.Height = 480;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                //  RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowForNew.VisibleStatusbar = false;


            }
            if (e.CommandName == "Select")
            {
                #region Print

                lblMsg.Text = "";
                Label lblBookingNo = (Label)e.Item.FindControl("lblBookingNo");
                Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");
                Label lblDoctorName = (Label)e.Item.FindControl("lblDoctorName");
                Label lblBookingId = (Label)e.Item.FindControl("lblBookingId");
                HiddenField hdnBookingtype = (HiddenField)e.Item.FindControl("hdnBookingtype");

             //   Session["AdmissionRequestBookingId"] = common.myStr(lblBookingId.Text);
                //Session["AdmissionRequestBookingNo"] = common.myStr(lblBookingNo.Text);
                //Session["AdmissionRequestRegNo"] = common.myStr(lblRegistrationNo.Text).Trim();
                //Session["AdmissionRequestDocName"] = common.myStr(" % " + lblDoctorName.Text + "%").Trim();
                //Session["AdmissionRequestBookingType"] = common.myStr(hdnBookingtype.Value).Trim();
                //Session["AdmissionRequestFromDate"] = dtpFromdate.SelectedDate.Value.ToString("yyyy /MM/dd");
                //Session["AdmissionRequestToDate"] = dtpToDate.SelectedDate.Value.ToString("yyyy /MM/dd");

                //Session["AdmissionRequestRT"] = common.myStr(ddlBookingType.SelectedValue);
                //Session["AdmissionRequestReportType"] = common.myStr(ddlReportType.SelectedValue);
                #endregion



                //ViewState["Selected"] = "1";
                lblMsg.Text = "";
                // Label lblBookingNo = (Label)e.Item.FindControl("lblBookingNo");
                //  Label lblRegistrationNo = (Label)e.Item.FindControl("lblRegistrationNo");

                hdnBookingNo.Value = lblBookingNo.Text;
                hdnRegNo.Value = lblRegistrationNo.Text;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
            }
            if (e.CommandName == "BookingDelete")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    BaseC.ATD objATD = new BaseC.ATD(sConString);

                    int vv = objATD.inActivePatientBooking(common.myInt(e.CommandArgument), common.myInt(Session["UserId"]));

                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMsg.Text = "Deleted Successfully ...";

                    ShowBookingDetails();

                  
                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
        }
    }

    protected void gvBooking_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");

            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }
    }

    private void ShowBookingDetails()
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        try
        {
            BaseC.ATD objBed = new BaseC.ATD(sConString);
            int HospId = common.myInt(Session["HospitalLocationID"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int BookingId = 0;
            string BookingNo = "";
            string RegNo = "";
            string DocName = "";
            string BookingType = "";
            if (ddlSearchBy.SelectedValue == "BN")
            {
                BookingNo = common.myStr(txtKeyword.Text);
            }
            else if (ddlSearchBy.SelectedValue == "RN")
            {
                RegNo = common.myStr(txtKeyword.Text);
            }
            else if (ddlSearchBy.SelectedValue == "BD")
            {
                DocName = common.myStr("%" + txtKeyword.Text + "%");
            }
            if (ddlBookingType.SelectedValue != "")
            {
                BookingType = ddlBookingType.SelectedValue;
            }
            string FromDate = dtpFromdate.SelectedDate.Value.ToString("yyyy/MM/dd");
            string ToDate = dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd");

            ds = objBed.GetPatientBedBookingDetails(HospId, FacilityId, FromDate, ToDate, BookingId,
                BookingNo, RegNo, DocName, BookingType, ddlReportType.SelectedValue, common.myInt(Session["Encounterid"]));
            dv = new DataView(ds.Tables[0]);
            //   dv.RowFilter = "Bookingstatus<>'AD'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvBooking.DataSource = dv.ToTable();
                gvBooking.DataBind();
            }
            else
            {
                gvBooking.DataSource = null;
                gvBooking.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
        finally
        {
            ds.Dispose();
            dv.Dispose();
        }
    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        try//           
        {
            string BookingNo = " ";
            string RegNo = " ";
            string DocName = " ";
            string BookingType = " ";
            if (ddlSearchBy.SelectedValue == "BN")
            {
                BookingNo = common.myStr(txtKeyword.Text).Trim();
            }
            else if (ddlSearchBy.SelectedValue == "RN")
            {
                RegNo = common.myStr(txtKeyword.Text).Trim();
            }
            else if (ddlSearchBy.SelectedValue == "BD")
            {
                DocName = common.myStr("%" + txtKeyword.Text + "%").Trim();
            }
            if (ddlBookingType.SelectedValue != "")
            {
                BookingType = ddlBookingType.SelectedValue.ToString().Trim();
            }
            string FromDate = dtpFromdate.SelectedDate.Value.ToString("yyyy/MM/dd");
            string ToDate = dtpToDate.SelectedDate.Value.ToString("yyyy/MM/dd");


            RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + FromDate
                + "&Todate=" + ToDate
                + "&BI=0&BN=" + BookingNo
                + "&RN=" + RegNo
                + "&DN=" + DocName
                + "&ReportType=" + ddlReportType.SelectedValue
                + "&BT="
                + BookingType + "&ReportName=BookingList"
                + "&RT=" + ddlBookingType.SelectedValue;
            RadWindowForNew.Height = 480;
            RadWindowForNew.Width = 850;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            //  RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }
}
