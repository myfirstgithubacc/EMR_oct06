using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;

public partial class OTScheduler_OTChecklist : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.RestFulAPI objwcfOt;//= new wcf_Service_OT.ServiceClient();
    BaseC.RestFulAPI objwcfcm;//= new wcf_Service_Common.CommonMasterClient();
    DataSet ds;
    StringBuilder objXML1;
    ArrayList coll;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objwcfcm = new BaseC.RestFulAPI(sConString);
            objwcfOt = new BaseC.RestFulAPI(sConString);
            BindPatientHiddenDetails();
            //  lblIpno.Text = Request.QueryString["IpNo"].ToString().Trim();
            //lblPatientname.Text = Request.QueryString["Pname"].ToString().Trim();
            hdnBookinId.Value = common.myStr(Request.QueryString["BookingId"]);
            if (Request.QueryString["Surgery"] != null)
            {
                lblSurgey.Text = Request.QueryString["Surgery"].ToString().Trim().Replace("@", "&");
                // if(Request.QueryString["ward"]
                lblWard.Text = Request.QueryString["ward"].ToString().Trim();
                hdnBedId.Value = Request.QueryString["BedId"].ToString().Trim();
                
                hdnSurgeryId.Value = Request.QueryString["SurgeryId"].ToString().Trim();
            }
            BindProviderList();
            BindGrid();
            //  BindPatientChecklistdata();
        }
    }
    private void BindProviderList()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            ds = objwcfcm.GetDoctorList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlcheckedby.DataSource = ds.Tables[0];
                ddlcheckedby.DataTextField = "DoctorName";
                ddlcheckedby.DataValueField = "DoctorId";
                ddlcheckedby.DataBind();
                ddlcheckedby.Items.Insert(0, new RadComboBoxItem("All", "0"));
                ddlcheckedby.Items[0].Value = "0";
                //  ddlcheckedby.SelectedIndex = ddlcheckedby.Items.IndexOf(ddlcheckedby.Items.FindItemByValue(common.myStr(Session["UserId"])));
                ddlcheckedby.SelectedIndex = 0;


            }

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
    }
    protected void gvDetails_PreRender(object sender, EventArgs e)
    {

    }
    protected void gvDetails_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            int strIndex = gvDetails.MasterTableView.CurrentPageIndex;

            Label lbl = (Label)e.Item.FindControl("lblSn");
            lbl.Text = Convert.ToString((strIndex * gvDetails.PageCount) + e.Item.ItemIndex + 1);

            Label txtValueType = (Label)e.Item.FindControl("txtValueType");
            TextBox txtRemarks = (TextBox)e.Item.FindControl("txtRemarks");
            TextBox txtRemark = (TextBox)e.Item.FindControl("txtRemark");
            CheckBox chkChecked = (CheckBox)e.Item.FindControl("chkChecked");
            RadDatePicker dtpdate = (RadDatePicker)e.Item.FindControl("dtpdate");
            Label txtValue = (Label)e.Item.FindControl("txtValue");
            //bool chk = Convert.ToBoolean(DataBinder.Eval(e.Item, "CheckListId"));
            if (common.myStr(txtValueType.Text) == "Text")
            {
                chkChecked.Visible = false;
                dtpdate.Visible = false;
                txtRemarks.Visible = true;
                txtRemarks.Text = txtValue.Text;
            }
            else if (common.myStr(txtValueType.Text) == "Date-Time")
            {
                chkChecked.Visible = false;
                dtpdate.Visible = true;
                txtRemarks.Visible = false;
                //dtpdate.SelectedDate = System.DateTime.Now;
                dtpdate.SelectedDate = common.myDate(txtValue.Text);
            }
            else if (common.myStr(txtValueType.Text) == "Y/N")
            {
                chkChecked.Visible = true;
                dtpdate.Visible = false;
                txtRemarks.Visible = false;
                if (common.myStr(txtValue.Text) == "1")
                    chkChecked.Checked = true;
                else
                    chkChecked.Checked = false;
            }

        }

    }

    void BindGrid()
    {
        ds = new DataSet();
        //ds = objwcfOt.GetOTChecklistMaster();
        ds = objwcfOt.GetChecklistOTTransaction(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["encounterid"]), common.myInt(hdnBookinId.Value), common.myInt(Session["UserId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcheckedby.SelectedIndex = ddlcheckedby.Items.IndexOf(ddlcheckedby.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["CheckedBy"].ToString())));
        }


        if (ds.Tables[1].Rows.Count > 0)
        {
            gvDetails.DataSource = ds.Tables[1];
            gvDetails.DataBind();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        objXML1 = new StringBuilder();
        coll = new ArrayList();

        if (common.myInt(ddlcheckedby.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Checked By! ";
            ddlcheckedby.Focus();
            return;
        }

        foreach (GridDataItem item in gvDetails.Items)
        {

            CheckBox chk = (CheckBox)item.FindControl("chkChecked");
            TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");
            TextBox txtRemark = (TextBox)item.FindControl("txtRemark");
            RadDatePicker dtpdate = (RadDatePicker)item.FindControl("dtpdate");
            if (chk.Checked == true || common.myStr(txtRemarks.Text) != "" || common.myStr(dtpdate.SelectedDate) != "")
            {
                lblMessage.Text = "";
                Label txtId = (Label)item.FindControl("txtId");
                coll.Add(common.myStr(txtId.Text));
                coll.Add(common.myStr(chk.Checked));
                coll.Add(common.myStr(txtRemarks.Text));
                coll.Add(common.myStr(dtpdate.SelectedDate));
                coll.Add(common.myStr(txtRemark.Text));
                objXML1.Append(common.setXmlTable(ref coll));
            }
        }

        if (objXML1.ToString() == "" || objXML1.ToString() == "" || objXML1.ToString() == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Checklist Description Details !";
            return;
        }

        int? SurgeryId = null;
        if (common.myInt(hdnSurgeryId.Value) != 0)
        {
            SurgeryId = common.myInt(hdnSurgeryId.Value);
        }
        string strsave = "";
        if (common.myInt(Session["encounterid"]) != 0)
        {
            objwcfOt = new BaseC.RestFulAPI(sConString);
            strsave = objwcfOt.SaveOTChecklistTransaction(common.myInt(Session["HospitalLocationId"]), 
                common.myInt(Session["FacilityId"]), common.myInt(hdnBookinId.Value), common.myInt(Session["encounterid"]),
            SurgeryId, common.myInt(hdnBedId.Value), common.myInt(ddlcheckedby.SelectedValue), common.myInt(Session["UserId"]), objXML1.ToString());

            if (strsave.Contains("Saved") || strsave.Contains("Updated"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strsave;
            }
            else
            {

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strsave;

            }
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Admitted Patient! ";
        }
    }

    void BindPatientChecklistdata()
    {
        ds = new DataSet();
        ds = objwcfOt.GetChecklistOTTransaction(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["encounterid"]), common.myInt(hdnBookinId.Value), common.myInt(Session["UserId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {

            ddlcheckedby.SelectedIndex = ddlcheckedby.Items.IndexOf(ddlcheckedby.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["CheckedBy"].ToString())));

            //foreach (GridDataItem item in gvDetails.Items)
            //{
            //    Label txtId = (Label)item.FindControl("txtId");
            //    CheckBox chk = (CheckBox)item.FindControl("chkChecked");
            //    TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");
            //    RadDatePicker dtpdate = (RadDatePicker)item.FindControl("dtpdate");

            //    if (common.myInt(txtId.Text) == common.myInt(ds.Tables[0].Rows[0]["CheckListId"].ToString()))
            //    {
            //            chk.Checked = true;

            //    }


            //}

        }
    }
    void BindPatientHiddenDetails()
    {
        try
        {
            BaseC.ParseData bParse = new BaseC.ParseData();
            BaseC.Patient bC = new BaseC.Patient(sConString);

            if (common.myInt(Session["RegistrationId"]) != 0)
            {
                DataSet ds = new DataSet();
                ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), 0, common.myInt(Session["EncounterId"]), common.myInt(Session["UserId"]));

                if (ds.Tables.Count > 0)
                {
                    DataView dvIP = new DataView(ds.Tables[0]);
                    // dvIP.RowFilter = "OPIP = 'I'";
                    DataTable dt = new DataTable();
                    dt = dvIP.ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        lblRegistrationNo.Text = common.myStr(dr["RegistrationNo"]);
                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        lblDob.Text = common.myStr(dr["DOB"]);
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);

                    }

                }
                else
                {
                    //    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //lblMsg.Text = "Patient not found !";
                    return;
                }
            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMRReports/OTChecklistReport.aspx?BookingId=" + common.myInt(hdnBookinId.Value) + "&PatientName=" + common.myStr(lblPatientName.Text) ;
        RadWindow1.Height = 600;
        RadWindow1.Width = 750;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        //  RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
}
