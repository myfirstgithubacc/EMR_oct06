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
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using Telerik.Web.UI;
using Telerik.Windows.Controls;
using Telerik.Web.Dialogs;

public partial class EMR_Assessment_PatientSuperbill : System.Web.UI.Page
{
    DL_Funs fun = new DL_Funs();
    BaseC.DiagnosisBL bl = new BaseC.DiagnosisBL();   // Business Layer Class having properties
    static string STus = string.Empty;
    DataTable dt = new DataTable();
    string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    
    DAL.DAL dl;

    private enum GridTodayDx : byte
    {
        ICDCode = 0,
        ICDDescription = 1,
        DoctorName = 2,
        Link = 3,
        Edit = 4
    }

    private enum GridCPT : byte
    {
        ID = 0,
        CPTCode = 1,
        Servicename = 2,
        ModifierCode = 3,
        ICDID = 4,
        Units = 5,
        UnitAmount = 6,
        From = 7,
        To = 8,
        IsBillable = 9,
        Edit = 10
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (Request.QueryString["Mpg"] != null)
            Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
        if (Session["registrationID"] == null || Session["encounterid"] == null)
        {
            Response.Redirect("/Default.aspx?RegNo=0", false);
        }
        if (gvTodayDiagnosis.DataSource == null)
        {
            gvTodayDiagnosis.Enabled = false;
        }
        if (!IsPostBack)
        {
            if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
            {
                RetrievePatientDiagnosis();
                BindCPTGrid();
                gvCPT.Enabled = true;
                gvTodayDiagnosis.Enabled = true;
                btnMedication.Enabled = true;
                btnRefresh.Enabled = true;
                btnSuperbillStatus.Enabled = true;
                btnCnm.Enabled = true;
                lnkAddTodayDiagnosis.Enabled = true;
                lnkCPTAdd.Enabled = true;
                FillddlOrderName();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hsInput = new Hashtable();
                hsInput.Add("@EncounterId", Session["EncounterId"].ToString());
                DataSet status = dl.FillDataSet(CommandType.Text, "select SuperBillFinalize from Encounter where id = @EncounterId", hsInput);
                if (status.Tables[0].Rows.Count > 0)
                {
                    if (status.Tables[0].Rows[0]["SuperBillFinalize"].ToString().Trim() != "")
                    {
                        if (Convert.ToInt32(status.Tables[0].Rows[0]["SuperBillFinalize"]) == 99)
                        {
                            txtSuperbillStatus.Text = "Not Entered";
                            btnSuperbillStatus.Text = "Finalize";
                        }
                        else
                        {
                            if (Convert.ToInt32(status.Tables[0].Rows[0]["SuperBillFinalize"]) == 98)
                            {
                                txtSuperbillStatus.Text = "Finalize";
                                btnSuperbillStatus.Text = "Open";
                            }
                            else
                            {
                                txtSuperbillStatus.Text = "Open";
                                btnSuperbillStatus.Text = "Finalize";
                            }
                        }
                    }
                }
            }
            else
            {
                gvCPT.Enabled = false;
                gvTodayDiagnosis.Enabled = false;
                btnMedication.Enabled = false;
                btnRefresh.Enabled = false;
                btnSuperbillStatus.Enabled = false;
                btnCnm.Enabled = false;
                lnkAddTodayDiagnosis.Enabled = false;
                lnkCPTAdd.Enabled = false;

                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "No Active Encounter Exist for this Appointment";
            }
        }
    }

    protected void FillddlOrderName()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsInput = new Hashtable();
            hsInput.Add("@inyHospitalLocationId", Session["HospitalLocationID"].ToString());
            string strOrderName = "SELECT OrderSetID,Name From Exam_OrderSet WHERE HospitalLocationID =@inyHospitalLocationId";
            SqlDataReader sqlOrderName = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strOrderName, hsInput);
            if (sqlOrderName.HasRows == true)
            {
                //sqlOrderName.Read();
                ddlOrderSet.DataSource = sqlOrderName;
                ddlOrderSet.DataValueField = "OrderSetID";
                ddlOrderSet.DataTextField = "Name";
                ddlOrderSet.DataBind();
                ddlOrderSet.Items.Insert(0, "Select");
                ddlOrderSet.Items[0].Value = "0";

            }
            sqlOrderName.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected DataTable createChronicTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("ICDCode");
        Dt.Columns.Add("ICDID");
        Dt.Columns.Add("ID");
        Dt.Columns.Add("ICDDescription");
        Dt.Columns.Add("Conditions");
        Dt.Columns.Add("DiagnosisStatus");
        Dt.Columns.Add("DoctorName");
        Dt.Columns.Add("Link");
        return Dt;
    }

    protected void ddlOrderSubset_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlOrderSet.SelectedValue != "0")
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshInput = new Hashtable();
                Hashtable hshOutput = new Hashtable();
                hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
                hshInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                hshInput.Add("@intDoctorID ", Convert.ToInt32(Session["DoctorID"]));
                hshInput.Add("@intFacilityID ", Convert.ToInt32(Session["FacilityID"]));
                hshInput.Add("@intEncounterId ", Convert.ToInt32(Session["EncounterID"]));
                hshInput.Add("@intEncodedBy ", Convert.ToInt32(Session["UserID"]));
                hshInput.Add("@intOrderName ", ddlOrderSet.SelectedValue);
                hshOutput.Add("@chvErrorStatus ", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveOrderSuperbill", hshInput, hshOutput);
                BindCPTGrid();
                RetrievePatientDiagnosis();
                ddlOrderSet.SelectedIndex = -1;
                ddlOrderSet.SelectedItem.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnOpenOrder_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/ExamManagement/Order.aspx?regForDetails=" + Session["RegistrationId"] + "&encForDetails=" + Session["EncounterID"] + "&appForDetails=" + Session["AppointmentID"];
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1000;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.ReloadOnShow = true;

    }

    protected void imgBCloseOrder_Click(object sender, EventArgs e)
    {
        //Image1.Visible = false;
        //divOrder.Visible = false;
    }

    protected void btnPullForward_OnClick(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
            hshInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
            hshInput.Add("@intDoctorID ", Convert.ToInt32(Session["DoctorID"]));
            hshInput.Add("@intFacilityID ", Convert.ToInt32(Session["FacilityID"]));
            hshInput.Add("@intEncounterId ", Convert.ToInt32(Session["EncounterID"]));
            hshInput.Add("@intEncodedBy ", Convert.ToInt32(Session["UserID"]));
            hshInput.Add("@intFormId ", Convert.ToInt32(Session["formId"]));
            hshOutput.Add("@chvErrorStatus ", SqlDbType.VarChar);
            hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveSuperbillPullForward", hshInput, hshOutput);
            BindCPTGrid();
            RetrievePatientDiagnosis();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSuperbillStatus_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Session["EncounterId"] != null)
            {
                String strSQL = "";
                Hashtable hshTable = new Hashtable();
                Hashtable hshOutput = new Hashtable();

                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshTable.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                hshTable.Add("@intEncounterID", Session["EncounterId"]);
                hshTable.Add("@intRegistrationID", Session["RegistrationId"]);
                hshOutput.Add("@chvValue", SqlDbType.VarChar);

                if (btnSuperbillStatus.Text == "Open")
                {
                    Hashtable hshInput = new Hashtable();
                    hshInput.Add("@intEncounterID", Session["EncounterId"]);
                    strSQL = "Update Encounter set SuperBillFinalize=97 where Id=@intEncounterId";
                    btnSuperbillStatus.Text = "Finalize";
                    txtSuperbillStatus.Text = "Open";
                    dl.ExecuteNonQuery(CommandType.Text, strSQL, hshInput);
                    lblMessage.Text = "Open successfull";
                }
                else
                {
                    hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRCheckSuperbillFinalize", hshTable, hshOutput);

                    if (hshOutput["@chvValue"].ToString() != "True")
                    {
                        dvWarning.Visible = true;
                        //Alert.ShowAjaxMsg("Please correct the diagnosis codes Or Please check the dates of service", Page);
                        //return;
                    }
                    else
                    {
                        lblMessage.Text = "Finalize successfull";
                        btnSuperbillStatus.Text = "Open";
                        txtSuperbillStatus.Text = "Finalize";
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
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
            hshInput.Add("@intRegistrationID", Session["RegistrationID"]);
            hshInput.Add("@intEncounterId", Session["EncounterID"]);

            string strUpdateStatus = "UPDATE Encounter SET SuperBillFinalize=98 WHERE Id=@intEncounterId " +
                                      "AND HospitalLocationId=@inyHospitalLocationID AND RegistrationId=@intRegistrationID";
            dl.ExecuteNonQuery(CommandType.Text, strUpdateStatus, hshInput);
            dvWarning.Visible = false;
            lblMessage.Text = "Finalize successfull";
            btnSuperbillStatus.Text = "Open";
            txtSuperbillStatus.Text = "Finalize";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        dvWarning.Visible = false;
    }

    protected void RetrievePatientDiagnosis()
    {
        try
        {
            if (Session["encounterid"] != null && Session["RegistrationID"] != null && Session["HospitalLocationId"] != null)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshInput = new Hashtable();

                hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
                hshInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                hshInput.Add("@intEncounterId ", Convert.ToInt32(Session["encounterid"]));
                DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hshInput);
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    ViewState["Record"] = 1;
                    DataView dvDiagnosisDetail = new DataView(objDs.Tables[0]);
                    dvDiagnosisDetail.RowFilter = "IsChronic=0";
                    DataTable dtDiagnosisDetail = dvDiagnosisDetail.ToTable();
                    if (dtDiagnosisDetail.Rows.Count > 0)
                    {
                        gvTodayDiagnosis.DataSource = dtDiagnosisDetail;
                        gvTodayDiagnosis.DataBind();
                        //Cache.Insert("DignosisDetails", dtDiagnosisDetail, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        BindBlankTodayDiagnosisGrid();
                    }
                }
                else
                {
                    ViewState["Record"] = 0;
                    BindBlankTodayDiagnosisGrid();
                }
            }
            else
            {
                ViewState["Record"] = 0;
                BindBlankTodayDiagnosisGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvTodayDiagnosis_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if ((e.Item.ItemType == GridItemType.Header) || (e.Item.ItemType == GridItemType.Item))
        {
            HiddenField hdnId = (HiddenField)e.Item.FindControl("hdnID");
            if (hdnId != null)
            {
                if (hdnId.Value.Trim() == "")
                {
                    e.Item.Cells[Convert.ToByte(GridTodayDx.Edit)].Enabled = false;
                }
            }
        }
    }

    protected void gvTodayDiagnosis_PageIndexChanging(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        //gvTodayDiagnosis.PageIndex = e.NewPageIndex;
        RetrievePatientDiagnosis();
    }

    private void BindBlankTodayDiagnosisGrid()
    {
        try
        {
            DataTable Dt = createChronicTable(); ;

            for (int i = 0; i < 5; i++)
            {
                DataRow Dr = Dt.NewRow();
                Dr["ICDCode"] = "";
                Dr["ICDID"] = "";
                Dr["ID"] = "";
                Dr["ICDDescription"] = "";
                Dr["Conditions"] = "";
                Dr["DoctorName"] = "";
                Dr["DiagnosisStatus"] = "";
                Dr["Link"] = "";

                Dt.Rows.Add(Dr);
            }
            gvTodayDiagnosis.DataSource = Dt;
            gvTodayDiagnosis.DataBind();
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkAddTodayDiagnosis_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("Diagnosis.aspx?Page=SuperBill", false);
    }

    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        BindCPTGrid();
        RetrievePatientDiagnosis();
    }

    // Coded related with CPT Grid
    protected DataTable createCPTTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("DepartmentName");
        Dt.Columns.Add("CPTCode");
        Dt.Columns.Add("Servicename");
        Dt.Columns.Add("ID");
        Dt.Columns.Add("ModifierCode");
        Dt.Columns.Add("ICDID");
        Dt.Columns.Add("Units");
        Dt.Columns.Add("ServiceAmount");
        Dt.Columns.Add("FromDate");
        Dt.Columns.Add("ToDate");
        Dt.Columns.Add("IsBillable");
        Dt.Columns.Add("SubDeptId");
        //   Dt.Columns.Add("OrderId");     
        Dt.Columns.Add("UnitAmount");
        Dt.Columns.Add("SubName");
        Dt.Columns.Add("yn");
        Dt.Columns.Add("NDCCode", typeof(string));
        Dt.Columns.Add("ServiceDiscountAmount");
        Dt.Columns.Add("DoctorAmount");
        Dt.Columns.Add("DoctorDiscountAmount");

        return Dt;
    }

    private void BindBlankCPTGrid()
    {
        try
        {
            DataTable Dt = createCPTTable();
            for (int i = 0; i < 5; i++)
            {
                DataRow Dr = Dt.NewRow();
                Dr["DepartmentName"] = "";
                Dr["CPTCode"] = "";
                Dr["Servicename"] = "";
                Dr["ID"] = "";
                Dr["ModifierCode"] = "";
                Dr["ICDID"] = "";
                Dr["Units"] = "";
                Dr["ServiceAmount"] = "0";
                Dr["FromDate"] = "";
                Dr["ToDate"] = "";
                Dr["IsBillable"] = false;
                Dr["UnitAmount"] = "";
                Dr["SubName"] = "";
                Dr["yn"] = "";
                Dr["SubDeptId"] = "";
                Dr["ServiceDiscountAmount"] = "0";
                Dr["DoctorAmount"] = "0";
                Dr["DoctorDiscountAmount"] = "0";
                Dt.Rows.Add(Dr);
            }

            gvCPT.DataSource = Dt;
            gvCPT.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindCPTGrid()
    {
        try
        {
            if (Session["EncounterID"] != null && Session["registrationId"] != null && Session["HospitalLocationId"] != null)
            {
                DAL.DAL dlSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();
                hshIn.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
                hshIn.Add("@intRegistrationId", Convert.ToInt32(Session["registrationId"].ToString()));
                hshIn.Add("@intEncounterId", Convert.ToInt32(Session["EncounterID"].ToString()));
                DataSet dsCPT = dlSave.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hshIn);

                //string sQ = "exec UspEMRGetPatientDrugHistory " + Session["HospitalLocationId"] + "," + Session["RegistrationId"] + "," + Session["EncounterId"] + ", 0, '''A'',''D''', 0, 0, 0, 0,0,0, 0, '', '%%', '', '', 1  ";
                string sQ = "exec UspEMRGetPatientDrugHistory " + Session["HospitalLocationId"] + "," + Session["RegistrationId"] + "," + Session["EncounterId"] + ",0 ,'', 0, 0, 0, 0, 0,0,0, '', '%%', '', '', '', 0";
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet objDs = objDl.FillDataSet(CommandType.Text, sQ);
                DataRow objDr;
                dsCPT.Tables[0].Columns.Add("NDCCode", typeof(string));
                foreach (DataRow dr in objDs.Tables[0].Rows)
                {
                    objDr = dsCPT.Tables[0].NewRow();
                    objDr["DepartmentName"] = "Medication";
                    objDr["CPTCode"] = dr["JCode"];
                    objDr["ServiceName"] = dr["DISPLAY_NAME"];
                    objDr["Id"] = dr["PrescriptionId"];
                    objDr["ModifierCode"] = DBNull.Value;
                    objDr["IcdId"] = dr["ICDCode"];
                    objDr["Units"] = dr["QtyAmount"];
                    objDr["ServiceAmount"] = dr["UNIT_CHARGE"];
                    objDr["FromDate"] = dr["StartDate"];
                    objDr["ToDate"] = dr["EndDate"];
                    objDr["IsBillable"] = false;
                    objDr["yn"] = dr["yn"];
                    objDr["SubDeptId"] = 0;
                    objDr["NDCCode"] = dr["NDCCode"];
                    dsCPT.Tables[0].Rows.Add(objDr);
                }

                if (dsCPT.Tables[0].Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    DataView dv = dsCPT.Tables[0].DefaultView;
                    //dv.RowFilter = "ServiceAmount not like '0*'";
                    if (dv.Count > 0)
                    {
                        gvCPT.DataSource = dv.ToTable();
                        gvCPT.DataBind();
                        string strStatus = "SELECT SuperBillFinalize FROM Encounter WHERE Id=@intEncounterId AND HospitalLocationId=@inyHospitalLocationID AND RegistrationId=@intRegistrationID";
                        ds = dlSave.FillDataSet(CommandType.Text, strStatus, hshIn);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["SuperBillFinalize"].ToString().Trim() != "98")
                            {
                                string strUpdateStatus = "UPDATE Encounter SET SuperBillFinalize=97 WHERE Id=@intEncounterId " +
                                                                              "AND HospitalLocationId=@inyHospitalLocationID AND RegistrationId=@intRegistrationID";
                                dlSave.ExecuteNonQuery(CommandType.Text, strUpdateStatus, hshIn);
                                btnSuperbillStatus.Text = "Finalize";
                                txtSuperbillStatus.Text = "Open";
                            }
                        }
                    }
                }
                else
                {
                    BindBlankCPTGrid();
                }
            }
            else
            {
                BindBlankCPTGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvCPT_RowDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item)
        {
            if (((HiddenField)e.Item.FindControl("lblID")).Value == "")
            {
                e.Item.Cells[9].Enabled = false;
            }
        }

        if (e.Item is GridDataItem)
        {
            // HiddenField lblServiceIDGrid = (HiddenField)e.Item.FindControl("hdnServiceId");
            // LinkButton lb = (LinkButton)e.Item.FindControl("lnkEdit");
            //ImageButton ibtnDelete = (ImageButton)e.Item.FindControl("ibtnDelete");
            // Label lblServiceNameGrid = (Label)e.Item.FindControl("lblServiceName");
            //if (lblServiceNameGrid != null)
            //{
            //    lblServiceNameGrid.ToolTip = lblServiceNameGrid.Text;
            //}

            //if (lblServiceIDGrid.Value == "0" || lblServiceIDGrid.Value == "")
            //{
            //    lb.Enabled = false;
            //    ibtnDelete.Enabled = false;
            //}



            Label lblServiceAmount = (Label)e.Item.FindControl("lblUnitAmount");
            HiddenField hdnServiceAmount = (HiddenField)e.Item.FindControl("hdnServiceAmount");
            HiddenField hdnServiceDiscountAmount = (HiddenField)e.Item.FindControl("hdnServiceDiscountAmount");
            HiddenField hdnDoctorAmount = (HiddenField)e.Item.FindControl("hdnDoctorAmount");
            HiddenField hdnDoctorDiscountAmount = (HiddenField)e.Item.FindControl("hdnDoctorDiscountAmount");

            if (lblServiceAmount.Text.Trim().ToString() == "0.00" || lblServiceAmount.Text.Trim().ToString() == "")
            {
                lblServiceAmount.Text = "$0.00";
            }
            else
            {
                lblServiceAmount.Text = "$" + ((Convert.ToDecimal(hdnServiceAmount.Value) + Convert.ToDecimal(hdnDoctorAmount.Value == "" ? "0" : hdnDoctorAmount.Value)) - (Convert.ToDecimal(hdnServiceDiscountAmount.Value == "" ? "0" : hdnServiceDiscountAmount.Value) + Convert.ToDecimal(hdnDoctorDiscountAmount.Value == "" ? "0" : hdnDoctorDiscountAmount.Value))).ToString();
            }
            //
        }
    }

    protected void gvCPT_PageIndexChanged(Object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        BindCPTGrid();
    }

    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMRReports/PatientSuperBill.aspx?RegistrationId=" + Session["registrationId"].ToString() + "&AppointmentId=" + Session["AppointmentID"].ToString();
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.ReloadOnShow = true;

    }

    protected void gvCPT_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName != "0")
        {
            try
            {
                LinkButton lnk = (LinkButton)e.CommandSource;
                if (lnk.ID == "lnkEdit")
                {
                    int[] ServiceId = { 1013638, 1013627, 1013688, 1013797, 1013661, 1013668, 1013683, 1013649, 1013652, 1013675, 1013700, 1013800, 1013804, 5113638, 1013797, 1019410 };
                    //Array.Sort<int>(ServiceId);
                    if (e.CommandArgument.ToString() == "0")
                        RadWindowForNew.NavigateUrl = "PatientMedicationCharges.aspx?Mpg=" + Request.QueryString["Mpg"] + "&ID=" + e.CommandName;
                    else
                    {

                        if (ServiceId.Contains<int>(Convert.ToInt32(e.CommandArgument)))
                        {
                            RadWindowForNew.NavigateUrl = "ENMCodes.aspx?Mpg=" + Request.QueryString["Mpg"] + "&ID=" + e.CommandName;
                        }
                        //if (Array.BinarySearch(ServiceId, Convert.ToInt32(e.CommandArgument.ToString().Trim())) > 0)

                        else
                            RadWindowForNew.NavigateUrl = "PatientDiagnosisCharges.aspx?Mpg=" + Request.QueryString["Mpg"] + "&ID=" + e.CommandName;
                    }
                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 850;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    //RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.OnClientClose = "OnClientClose";
                }
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }
        }
    }

}