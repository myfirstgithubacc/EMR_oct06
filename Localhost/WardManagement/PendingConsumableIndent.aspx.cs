using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.IO;
using System.Net;
using System.Configuration;
using System.Data;
using System.Text;
using System.Collections;

public partial class WardManagement_PendingConsumableIndent : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private enum GridCol : byte
    {
        COL0 = 0, COL1 = 1, COL2 = 2, COL3 = 3, COL4 = 4, COL5 = 5, COL6 = 6, COL7 = 7, COL8 = 8, COL9 = 9, COL10 = 10, COL11 = 11
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsCallback)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("/Login.aspx?Logout=1", false);
            }
        }
        Session["PreviousPageUrl"] = Request.Url;
        //if (common.myInt(Session["StoreId"]) == 0)
        //{
        //    if (!Page.IsCallback)
        //    {
        //        Response.Redirect("/Pharmacy/ChangeStore.aspx?Mpg=P335&PT=" + common.myStr(Request.Url.PathAndQuery), false);
        //    }
        //}

        if (!Page.IsPostBack)
        {
            dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpfromDate.SelectedDate = System.DateTime.Now.AddMonths(-1);
            dtpToDate.SelectedDate = System.DateTime.Now;

            if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP"))
            {
                btnclose.Visible = true;
                divDateFilter.Visible = false;

                ViewState["Regno"] = common.myStr(Request.QueryString["Regno"]);
                ViewState["Encno"] = common.myStr(Request.QueryString["Encno"]);
                ViewState["RegId"] = common.myStr(Request.QueryString["RegId"]);
                ViewState["EncId"] = common.myStr(Request.QueryString["EncId"]);
                ViewState["DoctorId"] = common.myStr(Request.QueryString["DoctorId"]);
            }

            if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                                                    "isRequiredAllFacilityBed", sConString).Equals("Y"))
            {
                ViewState["isRequiredAllFacilityBed"] = "Y";
            }
            bindControl();
            bindIndent();
        }
    }

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        bindIndent();
    }

    protected void gvIndent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            lblMessage.Text = "";
            //strXMLItems = new StringBuilder();
            //coll = new ArrayList();

            if (e.CommandName == "MAKEORDER")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");

                if (common.myStr(Request.QueryString["POPUP"]).Equals("POPUP"))
                {
                    Session["PendingConsumableIndentId"] = common.myStr(hdnIndentId.Value);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    return;
                }
                else
                {
                    if (common.myInt(hdnIndentId.Value) > 0)
                    {
                        Label lblRegistrationNo = (Label)row.FindControl("lblRegistrationNo");
                        Label lblEncounterNo = (Label)row.FindControl("lblEncounterNo");
                        HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                        HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                        HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");

                        ds = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                common.myStr(lblRegistrationNo.Text), common.myStr(lblEncounterNo.Text), common.myInt(Session["UserId"]),
                                                common.myInt(hdnRegistrationId.Value));

                        Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                        Session["PatientDetailString"] = null;
                        Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

                        RadWindow1.NavigateUrl = "/EMR/Medication/ConsumableOrder.aspx?POPUP=POPUP&DRUGORDERCODE=CO&LOCATION=WARD&OPIP=I" +
                                                "&DoctorId" + common.myStr(hdnDoctorId.Value) +
                                                "&IndentId=" + common.myStr(hdnIndentId.Value) +
                                                "&Regno=" + common.myStr(lblRegistrationNo.Text) +
                                                "&Encno=" + common.myStr(lblEncounterNo.Text) +
                                                "&RegId=" + common.myStr(hdnRegistrationId.Value) +
                                                "&EncId=" + common.myStr(hdnEncounterId.Value);

                        RadWindow1.Height = 620;
                        RadWindow1.Width = 1200;
                        RadWindow1.Top = 40;
                        RadWindow1.Left = 100;
                        RadWindow1.OnClientClose = "SearchIndentOnClientClose";
                        RadWindow1.VisibleOnPageLoad = true;
                        RadWindow1.Modal = true;
                        RadWindow1.VisibleStatusbar = false;
                        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    }
                }
            }
            else if (e.CommandName.Equals("PRINT"))
            {
                int intPrescriptionId = common.myInt(e.CommandArgument);

                if (intPrescriptionId.Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Prescription not Selected !";
                    return;
                }

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(hdnEncounterId.Value) + "&PId=" + intPrescriptionId;

                RadWindow1.Height = 600;
                RadWindow1.Width = 900;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.Modal = true;
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

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
            patient = null;
            ds.Dispose();
        }
    }


    protected void gvIndent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
            //IsShowIndent = common.myStr(objBill.getHospitalSetupValue("IsShowIndentForSPS", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
            //if (IsShowIndent == "Y")
            //{
            //    //e.Row.Cells[3].Visible = false;
            //}
            //else
            //{
            //    //e.Row.Cells[3].Visible = true;
            //}

            //if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow || e.Row.RowState == DataControlRowState.Alternate)
            //{
            //    if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
            //    {
            //        //LinkButton lnkbtnMakeOrder = (LinkButton)e.Row.FindControl("lnkbtnMakeOrder");
            //        //lnkbtnMakeOrder.Visible = false;

            //        e.Row.Cells[(byte)GridCol.COL10].Visible = false;
            //    }
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkbtnMakeOrder = (LinkButton)e.Row.FindControl("lnkbtnMakeOrder");
                HiddenField hdnIsClosedByNurse = (HiddenField)e.Row.FindControl("hdnIsClosedByNurse");

                //lnkbtnMakeOrder.Visible = !common.myBool(hdnIsClosedByNurse.Value);

                if (common.myBool(hdnIsClosedByNurse.Value))
                {
                    e.Row.Cells[(byte)GridCol.COL7].BackColor = System.Drawing.Color.LightCoral;
                    e.Row.Cells[(byte)GridCol.COL8].BackColor = System.Drawing.Color.LightCoral;
                    e.Row.Cells[(byte)GridCol.COL9].BackColor = System.Drawing.Color.LightCoral;
                }

                //LinkButton lnkApproved = (LinkButton)e.Row.FindControl("lnkApproved");
                //LinkButton lnkCancel = (LinkButton)e.Row.FindControl("lnkCancel");
                //HiddenField hdnApprovalStatusColor = (HiddenField)e.Row.FindControl("hdnApprovalStatusColor");
                //if (common.myStr(hdnisapproved.Value) == "A")
                //{
                //    lnkApproved.Enabled = false;
                //    chkCheck.Checked = true;
                //    chkCheck.Enabled = false;
                //}
                //else if (common.myStr(hdnisapproved.Value) == "C")
                //{
                //    lnkCancel.Enabled = false;
                //    lnkApproved.Enabled = true;
                //}
                //e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(hdnApprovalStatusColor.Value);
            }
        }
        catch
        {
        }
    }

    void bindIndent()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            string fromDate = string.Empty;
            string toDate = string.Empty;

            if (!common.myStr(Request.QueryString["POPUP"]).Equals("POPUP"))
            {
                fromDate = Convert.ToDateTime(dtpfromDate.SelectedDate.Value).ToString("yyyy/MM/dd");
                toDate = Convert.ToDateTime(dtpToDate.SelectedDate.Value).ToString("yyyy/MM/dd");
            }

            ds = objEMR.getPendingConsumableIndent(common.myInt(Session["FacilityId"]), 0, common.myInt(ViewState["EncId"]), common.myInt(ViewState["RegId"]),
                                                common.myInt(ddlWard.SelectedValue), common.myInt(ddlClosedStatus.SelectedValue), fromDate, toDate);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }
            gvIndent.DataSource = ds.Tables[0];
            gvIndent.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;
            ds.Dispose();
        }
    }

    protected void gvIndent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIndent.PageIndex = e.NewPageIndex;
        bindIndent();
    }

    protected void ddlWard_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindIndent();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objWD = new BaseC.WardManagement();
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        try
        {
            ds = objWD.GetWardTagging(common.myInt(Session["HospitalLocationID"]), true, common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                if (common.myStr(ViewState["isRequiredAllFacilityBed"]) == "Y")
                {
                    ds = objadt.GetWard(common.myInt(0));
                }
                else
                {
                    ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
                }
            }

            ddlWard.DataSource = ds.Tables[0];
            ddlWard.DataTextField = "WardName";
            ddlWard.DataValueField = "WardId";
            ddlWard.DataBind();

            ddlWard.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlWard.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            objWD = null;
            objadt = null;
            ds.Dispose();
        }
    }


    protected void ddlClosedStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindIndent();
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
    }

}
