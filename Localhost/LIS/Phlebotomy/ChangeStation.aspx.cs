using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Configuration;

public partial class LIS_Phlebotomy_ChangeStation : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //BaseC.clsLISPhlebotomy objval;
    string Flag = "LIS";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Flag = common.myStr(Request.QueryString["Module"]);

        string s = common.myStr(Session["ModuleId"]);

        if (Flag == "")
        {
            Flag = common.myStr(Request.QueryString["MD"]);
        }
        if (!IsPostBack)
        {
            ViewState["Flag"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                 "StationRequiredForPhlebotomy", sConString);
            bindControl();
        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        try
        {
            if (Flag.Contains(","))
            {
                Flag = Flag.Substring(0, Flag.IndexOf(','));
            }
            BaseC.clsLISSampleReceivingStation objMaster = new BaseC.clsLISSampleReceivingStation(sConString);
            ds = objMaster.getStation(common.myInt(Session["EmployeeId"]), common.myInt(Session["GroupID"]), Flag);

            //////DataView dv = ds.Tables[0].DefaultView;
            //////dv.Sort = "StationName Asc";

            ////ddlStation.DataSource = ds.Tables[0];
            ////ddlStation.DataValueField = "StationId";
            ////ddlStation.DataTextField = "StationName";
            ////ddlStation.Attributes.Add("multipledoctor", "1");
            ////ddlStation.DataBind();

            foreach (DataRow dsTitle in ds.Tables[0].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dsTitle["StationName"];
                item.Value = dsTitle["StationId"].ToString();
                item.Attributes.Add("multipledoctor", common.myStr(dsTitle["multipledoctor"]));
                ddlStation.Items.Add(item);
                item.DataBind();
            }


            if (ds.Tables[0].Rows.Count == 1)
            {
                // Session["StationId"] =common.myInt(ds.Tables[0].Rows[0]["StationId"].ToString());
                Session["StationName"] = common.myStr(ds.Tables[0].Rows[0]["StationName"].ToString());
                btnSave_OnClick(this, null);
            }
            else
            {
                Session["StationId"] = 0;
                Session["StationName"] = "";
                //btnSave_OnClick(this, null);
            }

            //ddlStation.DataSource = ds.Tables[0];
            //ddlStation.DataValueField = "StationId";
            //ddlStation.DataTextField = "StationName";
            //ddlStation.DataBind();

        }
        catch (Exception Ex)
        {
            lblMessage.Text = "Error: " + Ex.Message;
        }
        finally
        {
            ds.Dispose();
        }
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        if (ddlStation.SelectedIndex < 0)
        {
            if (common.myStr(Request.QueryString["PT"]) != "COLL" && common.myStr(ViewState["Flag"]) == "N")
            {
                strmsg += "Please Select Station !";
                isSave = false;
            }
        }

        if (strmsg != "")
        {
            lblMessage.Text = strmsg;
        }
        return isSave;
    }

    protected void btnSave_OnClick(Object sender, EventArgs e)
    {
        DataSet dsPermission = new DataSet();
        try
        {
            if (!isSaved())
            {
                return;
            }
            BaseC.clsLISSignatureSetup objPermission = new BaseC.clsLISSignatureSetup(sConString);
            if (common.myStr(Request.QueryString["PT"]) == "COLL" && common.myStr(Request.QueryString["Module"]) == "")
            {
                if (common.myStr(ViewState["Flag"]) == "Y")
                {
                    Session["StationId"] = common.myInt(ddlStation.SelectedValue);
                    Session["StationName"] = common.myStr(ddlStation.SelectedItem.Text);
                }
                else
                {
                    Session["StationId"] = 0;
                    Session["StationName"] = "";
                }
            }
            else
            {
                Session["StationId"] = common.myInt(ddlStation.SelectedValue);
                Session["StationName"] = common.myStr(ddlStation.SelectedItem.Text);
            }

            Session["multipledoctor"] = common.myStr(ddlStation.SelectedItem.Attributes["multipledoctor"]);

            Session.Remove("StxtFromDate");
            Session.Remove("StxtToDate");
            Session.Remove("source");
            Session.Remove("Subdepartment");

            if (common.myStr(Session["ModuleName"]).Trim() == "LIS")
                ViewState["PT"] = "STATUS";
            else if (common.myStr(Session["ModuleName"]).Trim() == "Phlebotomy")
                ViewState["PT"] = "COLL";
            else
                ViewState["PT"] = "TEMPLATEVALUEMASTER";

            if (Request.QueryString["PT"] != null)
            {
                ViewState["PT"] = common.myStr(Request.QueryString["PT"]);
            }

            Session["EditMachineResult"] = "False";
            Session["CancelFinalizedResult"] = "False";
            Session["ReportingStage"] = "";
            Session["EditRelayDetails"] = "False";
            Session["editManualRequest"] = "False";
            Session["editPackageDetails"] = "False";

            objPermission.StationId = common.myInt(Session["StationId"]);
            objPermission.userId = common.myInt(Session["EmployeeId"]);

            dsPermission = objPermission.getPermissions(objPermission);

            if (dsPermission.Tables.Count > 0)
            {
                if (dsPermission.Tables[0].Rows.Count > 0)
                {
                    Session["EditMachineResult"] = common.myStr(dsPermission.Tables[0].Rows[0]["EditMachineResult"]);
                    Session["CancelFinalizedResult"] = common.myStr(dsPermission.Tables[0].Rows[0]["CancelFinalizedResult"]);
                    Session["ReportingStage"] = common.myStr(dsPermission.Tables[0].Rows[0]["ReportingStage"]);
                    Session["EditRelayDetails"] = common.myStr(dsPermission.Tables[0].Rows[0]["EditRelayDetails"]);
                    Session["editManualRequest"] = common.myStr(dsPermission.Tables[0].Rows[0]["editManualRequest"]);
                    Session["editPackageDetails"] = common.myStr(dsPermission.Tables[0].Rows[0]["EditPackageDetails"]);

                }
            }
            switch (common.myStr(ViewState["PT"]))
            {
                case "ATTACH":
                    Response.Redirect("/LIS/Phlebotomy/InvestigationStatusOutside.aspx", false);
                    break;
                case "FINALIZED":
                    Response.Redirect("/LIS/Phlebotomy/ResultFinalization.aspx?MD=" + Flag, false);
                    break;
                case "RESULT":
                    Response.Redirect("/LIS/Phlebotomy/InvestigationResult.aspx?MD=" + Flag, false);
                    break;
                case "TAT":
                    Response.Redirect("/LIS/Phlebotomy/TurnAroundTime.aspx", false);
                    break;
                case "DASH":
                    Response.Redirect("/LIS/Phlebotomy/LabDashBoard.aspx", false);
                    break;
                case "LDR":
                    Response.Redirect("/LIS/Phlebotomy/LabDevicesResult.aspx", false);
                    break;
                case "SIGN":
                    Response.Redirect("/LIS/Phlebotomy/LabDevicesResult.aspx", false);
                    break;
                case "PRINT":
                    Response.Redirect("/LIS/Phlebotomy/PrintInvServices.aspx", false);
                    break;
                case "MACHINE":
                    Response.Redirect("/LIS/Format/TemplateLabMachine.aspx", false);
                    break;
                case "MACHINETEST":
                    Response.Redirect("/LIS/Format/TemplateInterfaceMachinesTest.aspx", false);
                    break;
                case "SIGNATURE":
                    Response.Redirect("/LIS/Format/SignatureSetup.aspx", false);
                    break;
                case "TEMPLATEFIELD":
                    Response.Redirect("/LIS/Format/TemplateFields.aspx", false);
                    break;
                case "TEMPLATEVALUE":
                    Response.Redirect("/LIS/Format/TemplateValuesGroup.aspx", false);
                    break;
                case "TEMPLATEVALUEMASTER":
                    Response.Redirect("/LIS/Format/TemplateValuesMaster.aspx", false);
                    break;
                case "TEMPLATEREMARKS":
                    Response.Redirect("/LIS/Format/TemplateServiceRemarks.aspx", false);
                    break;
                case "SERVICETAG":
                    Response.Redirect("/MPages/Servicelokup.aspx", false);
                    break;
                case "ACK":
                    Response.Redirect("/LIS/Phlebotomy/SampleAck.aspx?MD=" + Flag, false);
                    break;
                case "LABREQUEST":
                    Response.Redirect("/LIS/Phlebotomy/LabRequest.aspx", false);
                    break;
                case "LABREQUESTLIST":
                    Response.Redirect("/LIS/Phlebotomy/LabRequestList.aspx", false);
                    break;
                case "REPORT":
                    Response.Redirect("/LIS/Phlebotomy/PrintInvServicesDaily.aspx", false);
                    break;
                case "TATEC":
                    Response.Redirect("/LIS/Phlebotomy/TurnAroundTimeExternal.aspx", false);
                    break;
                case "REPORTTYPENAME":
                    Response.Redirect("/LIS/Format/ReportType.aspx", false);
                    break;
                case "WORKSHEET":
                    Response.Redirect("/LIS/Phlebotomy/WorkSheet.aspx", false);
                    break;
                case "RISReceving":
                    Response.Redirect("/EMRReports/DailyInvestigationReport.aspx", false);
                    break;
                default:
                    Response.Redirect("/LIS/Phlebotomy/Phlebotomy.aspx?PT=" + common.myStr(ViewState["PT"]) + "&MD=" + Flag, false);
                    break;
            }
        }
        catch
        {
        }
        finally
        {
            dsPermission.Dispose();
        }
    }

    protected void ddlStation_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        btnSave.Focus();
    }

    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";
    }

}