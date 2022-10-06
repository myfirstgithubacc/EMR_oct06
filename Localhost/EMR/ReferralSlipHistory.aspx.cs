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

public partial class EMR_ReferralSlipHistory : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    String sqlstr = "";
    BaseC.ParseData bc = new BaseC.ParseData();
    BaseC.Hospital baseHc;
    BaseC.ATD objbc;
    BaseC.WardManagement objwd;
    DataSet ds;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MASTER"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        baseHc = new BaseC.Hospital(sConString);
        objbc = new BaseC.ATD(sConString);

        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();

            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            if (Request.QueryString["RegNo"] != null)
            {
                hdnRegistrationNo.Value = common.myStr(Request.QueryString["RegNo"]);
                hdnEncounterId.Value = common.myInt(Request.QueryString["EncId"]).ToString();
                hdnRegistrationId.Value = common.myInt(Request.QueryString["RegId"]).ToString();
                //btnReferral.Visible = false;
            }
            else
            {
                hdnRegistrationNo.Value = common.myStr(Session["RegistrationNo"]);
                hdnEncounterId.Value = common.myInt(Session["EncounterId"]).ToString();
                hdnRegistrationId.Value = common.myInt(Session["RegistrationId"]).ToString();
            }

            BindGrid();
            BindPatientHiddenDetails();
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
            {
                gvDetails.Enabled = false;
            }
        }
    }

    void BindPatientHiddenDetails()
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                //lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
    }

    public void BindGrid()
    {
        objwd = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        ds = objwd.GetReferralDetail(0, 0, "B", common.myInt(Session["FacilityId"]), common.myStr(hdnRegistrationNo.Value), 2, 0, 0);
        if (common.myStr(hdnRegistrationNo.Value) != "")
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);

                gvDetails.DataSource = ds;
                gvDetails.DataBind();
            }
        }
        else
        {
            BindBlankItemGrid();
            //DataSet ds_blank = new DataSet();
            //DataRow dr = ds_blank.Tables[0].NewRow();
            //ds.Tables[0].Rows.Add(dr);

            //gvDetails.DataSource = ds;
            //gvDetails.DataBind();
        }
    }

    private void BindBlankItemGrid()
    {

        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();

        try
        {

            dr["Source"] = string.Empty;
            dr["ReferralId"] = 0;


            dr["ReferralDate"] = string.Empty;

            dr["FromDoctorName"] = string.Empty;

            dr["DoctorName"] = string.Empty;
            dr["ReferralConclusionDate"] = string.Empty;


            dr["Note"] = string.Empty;
            dr["DoctorRemark"] = string.Empty;
            dr["ReplyBy"] = string.Empty;
            dr["Urgent"] = string.Empty;
            dr["ConcludeReferral"] = string.Empty;
            dr["EncodedId"] = 0;
            dr["CompareId"] = 0;
            //dr["UnitName"] = string.Empty;
            //dr["FoodRelationshipId"] = 0;
            //dr["FoodRelationship"] = string.Empty;

            ////dr["Instructions"] = string.Empty;
            //dr["DoseTypeId"] = 0;
            //dr["DoseTypeName"] = string.Empty;

            //dr["TimeUnit"] = string.Empty;

            //dr["Instruction"] = string.Empty;
            //dr["EncodedBy"] = string.Empty;

            dt.Rows.Add(dr);
            dt.AcceptChanges();


            //DataRow dr_Brow = dt.NewRow();
            //dt.Rows.InsertAt(dr_Brow, 0);



            //ViewState["ItemDetail"] = null;
            gvDetails.DataSource = dt;
            gvDetails.DataBind();

        }

        catch (Exception ex)
        {


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



            dt.Columns.Add("Source", typeof(string));

            dt.Columns.Add("ReferralId", typeof(int));

            dt.Columns.Add("ReferralDate", typeof(string));

            dt.Columns.Add("FromDoctorName", typeof(string));
            dt.Columns.Add("DoctorName", typeof(string));


            dt.Columns.Add("ReferralConclusionDate", typeof(string));
            dt.Columns.Add("Note", typeof(string));
            dt.Columns.Add("DoctorRemark", typeof(string));
            dt.Columns.Add("ReplyBy", typeof(string));
            dt.Columns.Add("Urgent", typeof(string));
            dt.Columns.Add("ConcludeReferral", typeof(string));
            dt.Columns.Add("EncodedId", typeof(int));
            dt.Columns.Add("CompareId", typeof(int));

            dt.Columns.Add("ReferralReplyId", typeof(int));
            dt.Columns.Add("ReferToDoctorId", typeof(int));
            dt.Columns.Add("SpecialisationId", typeof(int));
            dt.Columns.Add("EncounterId", typeof(int));
            return dt;
        }
        catch (Exception ex)
        {

            return dt;
        }

    }



    protected void gvDetails_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            Label lblSource = (Label)e.Item.FindControl("lblSource");
            Label txtId = (Label)e.Item.FindControl("txtId");
            HiddenField hdnReferralReplyId = (HiddenField)e.Item.FindControl("hdnReferralReplyId");

            RadWindow3.NavigateUrl = "/EMR/ReferralSlip.aspx?OP_IP=" + ((common.myStr(lblSource.Text) == "IPD") ? "I" : "O") +
                                        "&MASTER=NO&RegNo=" + common.myStr(hdnRegistrationNo.Value) +
                                        "&EId=" + common.myInt(hdnEncounterId.Value) +
                                        "&RefHis=Yes&refId=" + common.myInt(txtId.Text) +
                                        "&RefReplyId=" + common.myInt(hdnReferralReplyId.Value);

            //if (common.myStr(lblSource.Text) == "OPD")
            //{
            //    RadWindow3.NavigateUrl = "/EMR/ReferralSlip.aspx?OP_IP=O&MASTER=NO&RegNo=" + common.myStr(hdnRegistrationNo.Value) + "&EId=" + common.myInt(hdnEncounterId.Value) + "&RefHis=Yes&refId=" + txtId.Text;
            //}
            //else if (common.myStr(lblSource.Text) == "IPD")
            //{
            //    RadWindow3.NavigateUrl = "/EMR/ReferralSlip.aspx?OP_IP=I&MASTER=NO&RegNo=" + common.myStr(hdnRegistrationNo.Value) + "&EId=" + common.myInt(hdnEncounterId.Value) + "&RefHis=Yes&refId=" + txtId.Text;
            //}
            //else
            //{
            //    return;
            //}
            RadWindow3.Height = 600;
            RadWindow3.Width = 800;
            RadWindow3.Top = 20;
            RadWindow3.Left = 20;
            RadWindow3.OnClientClose = "OnClientClose";
            RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow3.Modal = true;
            RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
            RadWindow3.VisibleStatusbar = false;

        }
    }

    //protected void btnReferral_OnClick(object sender, EventArgs e)
    //{
    //    if (common.myStr(Session["OPIP"]) == "O" || common.myStr(Session["OPIP"]) == "E")
    //    {
    //        RadWindow3.NavigateUrl = "/EMR/ReferralSlip.aspx?OP_IP=O&MASTER=NO&RegNo=" + common.myStr(hdnRegistrationNo.Value) + "&EId=" + common.myInt(hdnEncounterId.Value);
    //    }
    //    else if (common.myStr(Session["OPIP"]) == "I")
    //    {
    //        RadWindow3.NavigateUrl = "/EMR/ReferralSlip.aspx?OP_IP=I&MASTER=NO&RegNo=" + common.myStr(hdnRegistrationNo.Value) + "&EId=" + common.myInt(hdnEncounterId.Value);
    //    }
    //    else
    //    {
    //        return;
    //    }
    //    RadWindow3.Height = 600;
    //    RadWindow3.Width = 800;
    //    RadWindow3.Top = 20;
    //    RadWindow3.Left = 20;
    //    RadWindow3.OnClientClose = "OnClientClose";
    //    RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //    RadWindow3.Modal = true;
    //    RadWindow3.InitialBehavior = WindowBehaviors.Maximize;
    //    RadWindow3.VisibleStatusbar = false;
    //}
    protected void btnClientClose_OnClick(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void gvDetails_OnItemDataBound(object source, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.SelectedItem)
        {
            Label txtUrgent = (Label)e.Item.FindControl("txtUrgent");
            Label txEncodedId = (Label)e.Item.FindControl("txEncodedId");
            Label lblCompareId = (Label)e.Item.FindControl("lblCompareId");
            LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");

            Label txConcludeReferral = (Label)e.Item.FindControl("txConcludeReferral");

            if (txtUrgent.Text.ToUpper() == "STAT")
            {
                e.Item.BackColor = System.Drawing.Color.LightGreen;
            }
            if (txConcludeReferral.Text.ToUpper() == "OPEN")
            {
                if (common.myInt(Session["UserId"]) == common.myInt(txEncodedId.Text) || common.myInt(Session["UserId"]) == common.myInt(lblCompareId.Text))
                {
                    lnkEdit.Visible = true;
                }
                else
                {
                    lnkEdit.Visible = false;
                }
            }
            else
            {
                lnkEdit.Visible = false;
            }

        }
    }
    protected void gvDetails_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        BindGrid();
    }

}
