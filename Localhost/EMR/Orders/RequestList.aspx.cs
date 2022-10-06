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

public partial class EMR_Orders_RequestList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            PatientInfo(common.myStr(Request.QueryString["RegNo"]));
            lblAcknowledged.BackColor = System.Drawing.Color.Bisque;
            BindData();
        }
    }
    private void PatientInfo(string RegNo)
    {
        try
        {
            if (RegNo != "")
            {
                DataSet ds = new DataSet();
                string sXSL = "";
                BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
                ds = objCM.getPatientDetails(Convert.ToInt64(RegNo), 1, Convert.ToInt32(Session["HospitalLocationID"]));
                sXSL = "/include/xsl/PatientInfo-US.xsl";
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() != "Patient Is Expired, No Transaction Allowed!")
                        {
                            xmlPatientInfo.DocumentContent = ds.Tables[0].Rows[0][0].ToString();
                            xmlPatientInfo.TransformSource = sXSL;
                            xmlPatientInfo.DataBind();
                        }
                        else
                        {
                            xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                            xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                            xmlPatientInfo.DataBind();
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("No patient record found ", Page);
                        xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                        xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                        xmlPatientInfo.DataBind();
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("No patient record found ", Page);
                    xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                    xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                    xmlPatientInfo.DataBind();
                }
            }
            else
            {
                xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                xmlPatientInfo.DataBind();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
    }
    protected void BindData()
    {
        try
        {
            BaseC.EMROrders objB = new BaseC.EMROrders(sConString);
            DataSet ds = objB.GetRequestService(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
               "", 0, 0, "", "", "", "", "", common.myStr(Request.QueryString["RegNo"]));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["GridData"] = ds.Tables[0];
                    gvData.DataSource = ds.Tables[0];
                    gvData.DataBind();
                }
                else
                {
                    BindBlankGrid();
                }
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindBlankGrid()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Source");
        dt.Columns.Add("OrderID");
        dt.Columns.Add("RegistrationId");     
        dt.Columns.Add("RegistrationNo");       
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("EncounterId");
        dt.Columns.Add("OrderDate");
        dt.Columns.Add("DepartmentName");
        dt.Columns.Add("SubDepName");
        dt.Columns.Add("SubDeptId");
        dt.Columns.Add("DepartmentId");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("DoctorId");
        dt.Columns.Add("EncodedBy");
        dt.Columns.Add("StatusCode");
        dt.Columns.Add("IsAcknowledged");
        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        gvData.DataSource = dt;
        gvData.DataBind();
    }
   
    protected void gvData_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.PageIndex = e.NewPageIndex;
        if (ViewState["GridData"] != null)
        {
            gvData.DataSource = (DataTable)ViewState["GridData"];
            gvData.DataBind();
        }
        gvData.SelectedIndex = -1;      
    }
    protected void lnkViewDetails_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkOpen = (LinkButton)sender;
        string sServiceId = ((HiddenField)lnkOpen.FindControl("hdnSubDepId")).Value.ToString().Trim();
        string RequestId = ((Label)lnkOpen.FindControl("lblOrderID")).Text;
        RadWindow3.NavigateUrl = "~/EMR/Templates/Default.aspx?Source=ProcedureOrder&TemplateRequiredServices=&RegNo=" + common.myStr(Request.QueryString["RegNo"]) + "&TagType=D&RequestId=" + RequestId + "&RList=RL&SubDepId=" + sServiceId;
        RadWindow3.Height = 625;
        RadWindow3.Width = 950;
        RadWindow3.Top = 40;
        RadWindow3.Left = 100;
       // RadWindow3.OnClientClose = "OnClientClose";
        RadWindow3.Modal = true;
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow3.VisibleStatusbar = false;
    }
    protected void gvData_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {           
            hdnSubDepId.Value =((HiddenField)gvData.Rows[e.RowIndex].FindControl("hdnSubDepId")).Value;
            hdnRegistrationId.Value = ((HiddenField)gvData.Rows[e.RowIndex].FindControl("hdnRegistrationId")).Value;
            hdnEncounterId.Value = ((HiddenField)gvData.Rows[e.RowIndex].FindControl("hdnEncounterId")).Value;
            hdnRequestId.Value = ((Label)gvData.Rows[e.RowIndex].FindControl("lblOrderID")).Text;
            if (hdnSubDepId.Value != "&nbsp;")
            {
                divDelete.Visible = true;
            }
            else
            {
                Alert.ShowAjaxMsg("Please select Service", Page);
                return;
            }            
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnNo_OnClick(object sender, EventArgs e)
    {
        divDelete.Visible = false;
    }
    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
        if (hdnSubDepId.Value != "" || hdnSubDepId.Value != "0")
        {
            lblMessage.Text = order.DeletePatientServices(Convert.ToInt32(hdnSubDepId.Value),
               Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(hdnRegistrationId.Value), Convert.ToInt32(hdnEncounterId.Value),
               Convert.ToInt16(Session["FacilityID"]), false, 0, Convert.ToInt32(Session["UserId"]),
               common.myInt(hdnRequestId.Value), true);
            BindData();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }
        divDelete.Visible = false;
        //if (Request.QueryString["For"] == null)
        //{
        //    ShowHideAddInvestigationSpecification();
        //}
    }
    protected void lnkViewService_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkAck = (LinkButton)sender;
        string servicename = ((LinkButton)lnkAck.FindControl("lblSunbDepartmentName")).Text;

        RadWindow3.NavigateUrl = "~/EMR/Orders/SubDepartmentServices.aspx?RegNo=" + ((Label)lnkAck.FindControl("lblRegistrationNo")).Text.ToString().Trim()
                        + "&RequestId=" + common.myStr(((Label)lnkAck.FindControl("lblOrderID")).Text);
        RadWindow3.Height = 400;
        RadWindow3.Width = 800;
        RadWindow3.Top = 10;
        RadWindow3.Left = 10;
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow3.Modal = true;
        //RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow3.VisibleStatusbar = false;
    }
    protected void gvData_OnDataBinding(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnIsAcknowledge = (HiddenField)e.Row.FindControl("hdnIsAcknowledge");
            ImageButton ibtndaDelete = (ImageButton)e.Row.FindControl("ibtndaDelete");
            if (hdnIsAcknowledge.Value == "True")
            {
                e.Row.BackColor = System.Drawing.Color.Bisque;
                ibtndaDelete.Visible = false;

            }
        }
    }

}
