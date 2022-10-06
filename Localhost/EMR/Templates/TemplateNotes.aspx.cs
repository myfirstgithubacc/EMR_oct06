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
public partial class EMR_Templates_TemplateNotes : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            ViewState["SingleScreenTemplateCode"] = common.myStr(Request.QueryString["SingleScreenTemplateCode"]);
            BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
            ViewState["DoctorId"] = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserID"]));

            BaseC.User objU = new BaseC.User(sConString);
            ViewState["EmployeeType"] = common.myStr(objU.getEmployeeType(common.myInt(ViewState["DoctorId"]))).Trim();

            clsIVF objivf = new clsIVF(sConString);
            DataSet dsEncSpec = objivf.getDoctorSpecialisation(common.myInt(ViewState["DoctorId"]));

            if (dsEncSpec.Tables[0].Rows.Count > 0)
            {
                ViewState["DoctorSpecialisationId"] = common.myInt(dsEncSpec.Tables[0].Rows[0]["SpecialisationId"]);
            }
            // rdbList.Items[0].Selected=true;
            bindAllTemplateList();
            txtAllTemplateSearch.Focus();
            hdnIsUnSavedData.Value = "0";
            BindPatientHiddenDetails();
        }
    }
    void BindPatientHiddenDetails()
    {
        try
        {
            if (common.myStr(Request.QueryString["RPD"]) != "")
            {
                lblPatientDetail.Text = common.myStr(Session["RelationPatientDetailString"]);
            }
            else if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void rdblist_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtAllTemplateSearch.Text = "";
        lblMessage.Text = "";
        if (rdbList.SelectedIndex == 2)
        {
            BindddlTempGroup();
        }
        bindAllTemplateList();
    }
    protected void ddlTempGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindAllTemplateList();
    }
    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }
    private void BindddlTempGroup()
    {
        DAL.DAL objFav = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
        DataSet ds = objFav.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateGroup", HshIn);
        ddlTempGroup.DataSource = ds;
        ddlTempGroup.DataTextField = "GroupName";
        ddlTempGroup.DataValueField = "GroupId";
        ddlTempGroup.DataBind();

        ds.Dispose();
    }
    private void bindAllTemplateList()
    {
        DataSet ds = new DataSet();
        DataTable tbl = new DataTable();
        clsIVF objivf = new clsIVF(sConString);
        try
        {
            if (rdbList.SelectedIndex == 0)//favourite
            {
                ddlTempGroup.Visible = false;
                Label1.Text = "Favourite Template(s)";
                DAL.DAL objFav = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable HshIn = new Hashtable();
                HshIn.Add("@intDoctorId", common.myInt(ViewState["DoctorId"]));
                //HshIn.Add("@chvSearchCriteria", txtAllTemplateSearch.Text);
                ds = objFav.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteTemplates", HshIn);
            }
            else if (rdbList.SelectedIndex == 1)
            {
                ddlTempGroup.Visible = false;
                Label1.Text = "Speciality Template(s)";
                ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), common.myStr(ViewState["EmployeeType"]),
                                     common.myInt(ViewState["DoctorSpecialisationId"]), 0, common.myStr(Session["OPIP"]), 0, "", common.myInt(Session["FacilityId"]));
            }
            else if (rdbList.SelectedIndex == 2)
            {

                //BindddlTempGroup();
                ddlTempGroup.Visible = true;
                Label1.Text = "Template Group";
                DAL.DAL objFav = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable HshIn = new Hashtable();
                HshIn.Add("@intGroupId", common.myInt(ddlTempGroup.SelectedValue));
                // HshIn.Add("@chvSearchCriteria", txtAllTemplateSearch.Text);
                ds = objFav.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateGroupDetails", HshIn);

            }
            else if (rdbList.SelectedIndex == 3)
            {
                ddlTempGroup.Visible = false;
                BindddlTempGroup();
                Label1.Text = "All Template(s)";
                ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), common.myStr(ViewState["EmployeeType"]),
                                          0, 0, common.myStr(Session["OPIP"]), 0, "", common.myInt(Session["FacilityId"]));
            }

            DataView DV = ds.Tables[0].DefaultView;
            if (common.myLen(txtAllTemplateSearch.Text) > 0)
            {
                DV.RowFilter = "TemplateName LIKE '%" + txtAllTemplateSearch.Text.Trim() + "%'";
            }

            tbl = DV.ToTable();
            DV.Dispose();
            DataView dvTemplateCode = new DataView(tbl);
            if (common.myStr(ViewState["SingleScreenTemplateCode"]) != "" && common.myStr(ViewState["SingleScreenTemplateCode"]) == "OTH")
            {
                dvTemplateCode.RowFilter = "TemplateTypeCode<>'EXM' AND TemplateTypeCode<>'HIS' AND TemplateTypeCode<>'POC'";
            }
            DataTable tdTemplate = dvTemplateCode.ToTable();
            if (tdTemplate.Rows.Count == 0)
            {
                DataRow DR = tdTemplate.NewRow();
                tdTemplate.Rows.Add(DR);
                tdTemplate.AcceptChanges();
            }

            gvAllTemplate.DataSource = tdTemplate;
            gvAllTemplate.DataBind();
            txtAllTemplateSearch.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            tbl.Dispose();
            objivf = null;
        }
    }
    protected void gvAllTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (e.CommandName == "TMP")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                        Response.Redirect("~/EMR/Templates/Default.aspx?DisplayMenu=1&TemplateId=" + common.myInt(e.CommandArgument) + "&IsEMRPopUp=1", false);
                    else
                        Response.Redirect("~/EMR/Templates/Default.aspx?DisplayMenu=1&TemplateId=" + common.myInt(e.CommandArgument) + "&From=" + common.myStr(Request.QueryString["From"]), false);
                }
            }
            if (e.CommandName == "Del")
            {
                try
                {
                    string FavID;
                    FavID = common.myStr(e.CommandArgument);
                    if (FavID != "")
                    {
                        Hashtable hstInput = new Hashtable();
                        hstInput.Add("@intDoctorId", common.myInt(ViewState["DoctorId"]));
                        hstInput.Add("@intTemplateID", FavID);
                        hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));
                        hstInput.Add("@chrTranType", "D");

                        DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                        if (dal.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRSaveFavouriteTemplates", hstInput) == 0)
                        {
                            lblMessage.Text = "Record deleted";
                        }

                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    }
                    bindAllTemplateList();
                }
                catch (Exception Ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Error: " + Ex.Message;
                    objException.HandleException(Ex);
                }
            }
            if (e.CommandName == "Add")
            {
                string TempId;
                TempId = common.myStr(e.CommandArgument);
                if (TempId != "")
                {
                    Hashtable hstInput = new Hashtable();
                    hstInput.Add("@intDoctorId", common.myInt(ViewState["DoctorId"]));
                    hstInput.Add("@intTemplateID", TempId);
                    hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));
                    hstInput.Add("@chrTranType", "S");

                    DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                    if (dal.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRSaveFavouriteTemplates", hstInput) == 0)
                    {
                        lblMessage.Text = "Record saved";
                    }
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }
                bindAllTemplateList();
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
            ds.Dispose();
        }
    }
    protected void gvAllTemplate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        clearControl();
        gvAllTemplate.PageIndex = e.NewPageIndex;
        bindAllTemplateList();
    }
    protected void gvAllTemplate_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            if (rdbList.SelectedIndex == 0)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = true;
            }
            else
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton lnkRemoveFavourite = (ImageButton)e.Row.FindControl("lnkRemoveFavourite");
            HiddenField hdnIsMandatory = (HiddenField)e.Row.FindControl("hdnIsMandatory");
            if (common.myBool(hdnIsMandatory.Value) == true || common.myStr(hdnIsMandatory.Value).Equals(string.Empty))
            {
                lnkRemoveFavourite.Visible = false;
            }
        }
    }
    protected void btnAllTemplateSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindAllTemplateList();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}

