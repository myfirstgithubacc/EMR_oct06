using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using Telerik.Web.UI;
using System.IO;
using BaseC;
using System.Web;
using System.Linq;
using System.Net;


public partial class LIS_Phlebotomy_ViewOutsourcePatientHistory : System.Web.UI.Page
{
    Hashtable HshIn;
    Hashtable HshOut;

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;
    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];

    string Flag = "";
    string flg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                int RegId = common.myInt(Request.QueryString["RegID"]);
                ViewState["Regid"] = RegId;
                string EncounterId = common.myStr(Request.QueryString["EncounterId"]);
                ViewState["EncounterId"] = EncounterId;
                bindMainData(RegId,EncounterId);
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
            //SetGridColor();
        }
    }

    
    
    private void bindMainData(int RegID,string EncounterId)
    {
        try
        {
            DataSet ds = GetOutsideResults(common.myInt(Session["HospitalLocationID"]),common.myInt(Session["FacilityID"]),common.myInt(RegID),common.myStr(EncounterId));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;
                gvResultFinal.Columns.FindByUniqueName("FieldName").Visible = true;
                gvResultFinal.DataSource = dv; //dv.ToTable().Copy();
                gvResultFinal.DataBind();
            }
            //SetGridColor();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
        }
    }


    protected void gvResultFinal_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
             if (e.CommandName == "Result")
            {
                string Source = "OPD";
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                HiddenField lblServiceId = (HiddenField)e.Item.FindControl("hdnServiceid");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                Label lnkServiceName = (Label)e.Item.FindControl("lblservicename");
                HiddenField hdnStatuCode= (HiddenField)e.Item.FindControl("hdnStatuCode");

                RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/previewResult.aspx?SOURCE=" + Source
                                            + "&DIAG_SAMPLEID=" + common.myInt(lblDiagSampleID.Text)
                                            + "&SERVICEID=" + common.myInt(lblServiceId.Value)
                                            + "&ServiceName=" + common.myStr(lnkServiceName.Text)
                                            + "&LabNo=" + common.myStr(lblLabNo.Text)
                                            + "&StatusCode=" + common.myStr(hdnStatuCode.Value);

                //RadWindowPopup.NavigateUrl = "/LIS/Phlebotomy/previewResult.aspx?SOURCE=" + Source
                //                            + "&DIAG_SAMPLEID=" + common.myInt(lblDiagSampleID.Text)
                //                            + "&SERVICEID=" + common.myInt(lblServiceId.Text)
                //                            + "&AgeInDays=" + common.myStr(lblAgeGender.Text)
                //                            + "&StatusCode=" + common.myStr(lblStatusCode.Text)
                //                            + "&ServiceName=" + common.myStr(lnkServiceName.Text)
                //                            + "&LabNo=" + common.myStr(lblLabNo.Text);

                RadWindowPopup.Height = 550;
                RadWindowPopup.Width = 850;
                RadWindowPopup.Top = 10;
                RadWindowPopup.Left = 10;
                RadWindowPopup.VisibleOnPageLoad = true;
                RadWindowPopup.Modal = true;
                RadWindowPopup.VisibleStatusbar = false;
                RadWindowPopup.InitialBehaviors = WindowBehaviors.Maximize;
            }
            if (e.CommandName == "Delete")
            {
               
                HiddenField FieldId = (HiddenField)e.Item.FindControl("hdnFieldId");
                HiddenField DiagInvResultOPId = (HiddenField)e.Item.FindControl("DiagInvResultOPId");
                Label lblDiagSampleID = (Label)e.Item.FindControl("lblDiagSampleID");
                Label lblLabNo = (Label)e.Item.FindControl("lblLabNo");
                int Result = InActiveOutSideLabResultField(common.myInt(FieldId.Value),common.myInt(lblDiagSampleID.Text),common.myInt(lblLabNo.Text), common.myInt(DiagInvResultOPId.Value));
                 bindMainData(common.myInt(ViewState["Regid"].ToString()), common.myStr(ViewState["EncounterId"].ToString()));

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    public int InActiveOutSideLabResultField(int FieldId,int DiagSampleid, int LabNo,int DiagInvResultOPId)
    {
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        int i = 0;
        try
        {
            HshIn.Add("@intFieldId", FieldId);
            HshIn.Add("@intDiagSampleid", DiagSampleid);
            HshIn.Add("@intLabNo", LabNo);
            HshIn.Add("@intDiagInvResultOPId", DiagInvResultOPId);
            i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE DiagInvResultOP set Active=0 Where FieldId=@intFieldId and DiagSampleId=@intDiagSampleid and LabNo=@intLabNo and Id=@intDiagInvResultOPId", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return i;

    }
    private void SetGridColor()
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        try
        {
            DataSet ds = new DataSet();
            foreach (GridDataItem dataItem in gvResultFinal.MasterTableView.Items)
            {
                if (dataItem.ItemType == GridItemType.Item
                    || dataItem.ItemType == GridItemType.AlternatingItem
                    || dataItem.ItemType == GridItemType.SelectedItem)
                {
                    Label lblStatusColor = (Label)dataItem.FindControl("lblStatusColor");
                    HiddenField hdnIsMultiStageResultFinalized = (HiddenField)dataItem.FindControl("hdnIsMultiStageResultFinalized");
                    dataItem.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));
                    if (((Label)dataItem.FindControl("lblStat")).Text.ToUpper().Equals("TRUE"))
                    {
                        ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "Stat");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                dataItem.Cells[4].BackColor = System.Drawing.Color.FromName(common.myStr(ds.Tables[0].Rows[0]["StatusColor"]));
                            }
                        }
                    }
                    if (common.myBool(hdnIsMultiStageResultFinalized.Value))
                    {
                        ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), "LabOthers", "MulStageRF");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                dataItem.Cells[3].BackColor = System.Drawing.Color.FromName(common.myStr(ds.Tables[0].Rows[0]["StatusColor"]));
                            }
                        }
                    }
                }
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
            // Legend1.loadLegend("LabOthers", "'Stat'");
        }
    }
    private void BindMainBlankGrid()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("LabNo");
            dt.Columns.Add("Investigation");
            dt.Columns.Add("FieldName");
            dt.Columns.Add("Result");
            dt.Columns.Add("Ref. Range");

            gvResultFinal.VirtualItemCount = 0;
            gvResultFinal.DataSource = dt;
            gvResultFinal.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }
    }

    public DataSet GetOutsideResults(int inyHospitalLocationId, int intFacilityId, int intRegistrationId, string intEncounterId)
    {
        HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
        HshIn.Add("@intFacilityId", intFacilityId);
        HshIn.Add("@intRegistrationId", intRegistrationId);
        HshIn.Add("@intEncounterId", intEncounterId);
        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetOutsideResults", HshIn);
        return ds;
    }

}