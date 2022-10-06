using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Drawing;
using System.Configuration;
using System.Collections;

public partial class LIS_Phlebotomy_previewResult : System.Web.UI.Page
{
    #region Global values
    //Global Class objects
    BaseC.clsLISPhlebotomy objval;

    private string cCtlType = "";
    //Constant Values For Retriving the DataTable from list
    private const int FIELDS = 0;
    private const int OPTIONS = 1;
    private const int VALUES = 2;
    // To set the template Horizontal view or vertical view
    //DataSet and Datatable user fro Temporariy transactions
    DataSet objDs = new DataSet();
    DataTable dt = new DataTable();
    //Globals Enum used in retereving Data from Global List
    private enum eServicesGrid
    {
        FieldID = 0,
        FieldName = 1,
        FieldType = 2,
        Values = 3,
        Remarks = 4,
        ServiceId = 5
    }
    private enum eRowGrid
    {
        FieldId = 0,
        RowId = 1,
        RowCaption = 2
    }
    private enum eFieldType : int
    {
        [StringValue("N")]
        Numeric = 1,
        [StringValue("T")]
        TextSingleLine = 2,
        [StringValue("M")]
        TextMultiLine = 3,
        [StringValue("W")]
        TextFormats = 4,
        [StringValue("D")]
        ListofChoices = 5,
        [StringValue("H")]
        Heading = 6,
        [StringValue("TA")]
        Tabular = 7,
        [StringValue("F")]
        Formula = 8,
        [StringValue("S")]
        Date = 9,
        [StringValue("C")]
        CheckBox = 10,
        [StringValue("B")]
        Boolean = 11,
        [StringValue("O")]
        Organism = 12,
        [StringValue("E")]
        Enzyme = 13,
        [StringValue("SN")]
        Sensitivity = 14,
        [StringValue("TM")]
        Time = 15,
        [StringValue("I")]
        Image = 16

    }
    #endregion
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindTemplateGrid();
            lblServiceName.Text = common.myStr(Request.QueryString["ServiceName"]);
        }
    }
    protected void btnSN_Click(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        int iFieldId = common.myInt(gvSelectedServices.Rows[index].Cells[0].Text);

        BaseC.clsMicrobiology objMicrobiology = new BaseC.clsMicrobiology(sConString);
        string source = common.myStr(Request.QueryString["SOURCE"]);
        int iLabNo = common.myInt(Request.QueryString["LabNo"]);
        int iResultId = common.myInt(ddlResultId.SelectedValue);
        int iDiagSampleId = common.myInt(Request.QueryString["DIAG_SAMPLEID"]);
        int iServiceId = common.myInt(Request.QueryString["SERVICEID"]);
        string sFieldType = "SN";
        string strOrganismIds = "";
        int iOrganismCount = 1;

        for (int i = 0; i < gvSelectedServices.Rows.Count; i++)
        {
            string fieldType = common.myStr(gvSelectedServices.Rows[i].Cells[(int)eServicesGrid.FieldType].Text);
            if (fieldType == common.getEnumStrVal(eFieldType.Organism))
            {
                RadComboBox ddl = (RadComboBox)gvSelectedServices.Rows[i].Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.Organism));
                if (common.myInt(ddl.SelectedIndex) != 0)
                {
                    if (strOrganismIds == "")
                    {
                        strOrganismIds = ddl.SelectedValue;
                    }
                    else
                    {
                        strOrganismIds = strOrganismIds + "," + ddl.SelectedValue;
                    }
                    iOrganismCount = iOrganismCount + 1;
                }
            }
        }
        if (common.myStr(ViewState["StatusCode"]) != "RF")
        {
            if (strOrganismIds == "")
            {
                //lblMessage.Text = "Please Select Organism(s)!";
                return;
            }
            bool IsSaved = objMicrobiology.CheckMicrobiologyResult(source, iDiagSampleId, sFieldType, strOrganismIds, iOrganismCount, iResultId);
            if (IsSaved == false)
            {
                //lblMessage.Text = "Please Save Before Entering Sensitivity Data!";
                return;
            }
        }
        string ServiceName = common.myStr(Request.QueryString["ServiceName"]);
        rwPrintLabReport.NavigateUrl = "/LIS/Phlebotomy/SensitivityResult.aspx?LabNo=" + iLabNo +
            "&DiagSampleId=" + iDiagSampleId + "&OrganismIds=" + strOrganismIds + "&ResultId=" + iResultId
            + "&Source=" + source + "&ServiceName=" + ServiceName + "&ServiceId=" + iServiceId + "&FieldId="
            + iFieldId + "&RStatus=RF&IsRelease=1";
        rwPrintLabReport.Height = 630;
        rwPrintLabReport.Width = 1000;
        rwPrintLabReport.Top = 10;
        rwPrintLabReport.Left = 10;
        rwPrintLabReport.Modal = true;
        rwPrintLabReport.VisibleOnPageLoad = true;
        rwPrintLabReport.VisibleStatusbar = false;
        rwPrintLabReport.Behaviors = WindowBehaviors.Move | WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Resize;
    }
    protected void RenderAttachedImage(object sender, EventArgs e)
    {

        DataSet ds = new DataSet();
        // objval = new clsLISPhlebotomy(sConString);
        BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
        string key = "Word";
        string sFileName = common.myStr(((ImageButton)sender).CommandName);

        string sSavePath = common.myStr(ConfigurationManager.AppSettings["LabResultPath"]);
        string path = sSavePath + sFileName;
        string URLPath = "AttachementRender.aspx?FTPFolder=@FTPFolder&FileName=@FileName";
        URLPath = URLPath.Replace("@FTPFolder", en.Encrypt(ConfigurationManager.AppSettings["LabResultPath"], key, true, string.Empty)).Replace("@FileName", en.Encrypt(sFileName, key, true, string.Empty));
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + URLPath.Replace("+", "%2B") + "','_blank')", true);
        ds.Dispose();
        objval = null;

    }
    public DataSet Getimageresult(int sampleID, int fieldID, string chvSource)
    {
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            HshIn.Add("@sampleID", sampleID);
            HshIn.Add("@fieldID", fieldID);
            HshIn.Add("@chvSource", chvSource);
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiaggetimageResult", HshIn);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objDl = null;
            HshIn = null;
        }
    }
    public DataSet GetDefaultFieldImage(int iFieldId)
    {
        DAL.DAL obj1 = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();

        try
        {
            HshIn.Add("@intFieldId", iFieldId);
            return obj1.FillDataSet(CommandType.Text, "SELECT * FROM DiagFieldsImage WHERE Active=1 AND FieldId = @intFieldId", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            obj1 = null;

        }
    }
    protected void gvSelectedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            //Hiding the Bound Fields -FieldID,FieldType,Remarks,ServiceId
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[(int)eServicesGrid.FieldID].Visible = false;
                e.Row.Cells[(int)eServicesGrid.FieldType].Visible = false;
                e.Row.Cells[(int)eServicesGrid.Remarks].Visible = false;
                e.Row.Cells[(int)eServicesGrid.ServiceId].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (common.myInt(Session["HospitalLocationId"]) > 0)
                {
                    DataView objDv = null;
                    DataView objDvValue;
                    DataTable objDt = null;
                    //Getting Whether The Result Varies Machine Wise Or nOt
                    ViewState["MachineWise"] = common.myBool(DataBinder.Eval(e.Row.DataItem, "LimitMachineWise"));
                    //Getting Whether the field is tagged with machine or not
                    ViewState["LinkedWithMachine"] = common.myBool(DataBinder.Eval(e.Row.DataItem, "LinkedWithMachine"));

                    DataView ddv = new DataView(dt);
                    //Filtering the Particular Row FieldId Data 
                    if (ddv.Count > 0)
                    {
                        ddv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'"; //0
                        objDt = ddv.ToTable();
                        if (objDt.Rows.Count > 0)
                        {
                            e.Row.Visible = true;
                        }
                    }
                    else
                    {
                        //getting  the Particular Row FieldId Data 
                        if (objDs.Tables.Count > 2)
                        {
                            if (objDs.Tables[2].Rows.Count > 0)
                            {
                                objDvValue = objDs.Tables[2].DefaultView;
                                if (objDvValue.Table.Columns["FieldId"] != null)
                                {
                                    objDvValue.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'" + " AND ResultId='" + ddlResultId.SelectedValue + "'"; //0
                                }

                                objDt = objDvValue.ToTable();

                                if (objDt.Rows.Count > 0)
                                {
                                    e.Row.Visible = true;
                                }
                            }
                        }
                    }
                    HtmlTextArea txtMulti = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                    if (e.Row.Cells[(int)eServicesGrid.FieldType].Text != null)
                    {
                        string fieldType = common.myStr(e.Row.Cells[(int)eServicesGrid.FieldType].Text);
                        //Block For Heading Type Field
                        if (fieldType == common.getEnumStrVal(eFieldType.Heading))
                        {
                            Label fieldName = (Label)e.Row.FindControl("lblFieldName");

                            fieldName.Font.Bold = true;
                            fieldName.Font.Underline = true;
                            e.Row.Cells[(int)eServicesGrid.FieldName].ColumnSpan = 3;

                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            txtW.Visible = false;
                        }
                        //Awadhesh

                        #region Image Type Field
                        if (fieldType == common.getEnumStrVal(eFieldType.Image))
                        {
                            cCtlType = fieldType;
                            Button ibtnUpload = (Button)e.Row.FindControl("ibtnUpload");
                            ImageButton imgRIS = (ImageButton)e.Row.FindControl("imgRIS");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            FileUpload iFileUploader = (FileUpload)e.Row.FindControl("iFileUploader");
                            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                            string FieldID = common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text);
                            DataView objDvValue1 = objDs.Tables[2].DefaultView;
                            if (objDvValue1.Table.Columns["FieldId"] != null)
                            {
                                objDvValue1.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'" + " AND ResultId='" + ddlResultId.SelectedValue + "'"; //0
                            }

                            DataTable objDt1 = objDvValue1.ToTable();
                            if (Session["OPIP"].ToString() == "I")
                            {
                                ViewState["Sourcee"] = "IPD";
                            }
                            else
                            {
                                ViewState["Sourcee"] = "OPD";
                            }
                            DataSet ds = Getimageresult(common.myInt(objDt1.Rows[0]["DiagSampleId"]), common.myInt(FieldID), common.myStr(ViewState["Sourcee"]));
                            if (common.myInt(ds.Tables[0].Rows.Count) > 0)
                            {

                                imgRIS.Visible = true;

                                imgRIS.CommandName = ds.Tables[0].Rows[0]["imageUrl"].ToString();
                                btnRemoveimage.CommandName = ds.Tables[0].Rows[0]["ID"].ToString();

                            }
                            ibtnUpload.Visible = false;
                            iFileUploader.Visible = false;
                            txtW.Visible = false;

                        }
                        #endregion

                        //
                        //Block For Single Line Textbox Type Field
                        if (fieldType == common.getEnumStrVal(eFieldType.TextSingleLine))
                        {
                            cCtlType = fieldType;
                            TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                            txtW.Visible = false;
                            txtT.Visible = true;

                            if (common.myInt(txtT.MaxLength) == 0)
                            {
                                txtT.MaxLength = 100;
                            }

                            if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                            {
                                txtT.Enabled = false;
                                txtT.CssClass = "Disabledtxt"; // Data non editable
                            }
                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                txtT.Enabled = false;
                                txtT.CssClass = "LinkedWithMachine"; // Data non editable
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    txtT.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    //Checking This is Machine Result then the user has edit Permission or not
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        txtT.CssClass = "MachineResultTxt";

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                txtT.Enabled = true;
                                            }
                                            else
                                            {
                                                txtT.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                txtT.Enabled = false;
                                            }
                                            else
                                            {
                                                txtT.Enabled = true;
                                            }
                                        }
                                    }
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) == 0)
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            txtT.Enabled = false;
                                            txtT.CssClass = "LinkedWithMachine"; // Data non editable
                                        }

                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtT.Enabled = false;
                                        txtT.CssClass = "Disabledtxt"; // Data non editable
                                    }



                                }
                            }

                        }
                        //Block For Numeric Textbox Type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Numeric)) //
                        {
                            cCtlType = fieldType;
                            TextBox txtN = (TextBox)e.Row.FindControl("txtN");
                            Label lblN = (Label)e.Row.FindControl("lblN");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            Label lblLocation = (Label)e.Row.FindControl("lblLocation");
                            RadComboBox ddlRange = (RadComboBox)e.Row.FindControl("ddlRange");
                            HiddenField hdnUnitName = (HiddenField)e.Row.FindControl("hdnUnitName");
                            HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                            HiddenField hdnMinValue = (HiddenField)e.Row.FindControl("hdnMinValue");
                            HiddenField hdnMaxValue = (HiddenField)e.Row.FindControl("hdnMaxValue");
                            HiddenField hdnSymbol = (HiddenField)e.Row.FindControl("hdnSymbol");
                            TextBox txtFinalizedDislpayMachine = (TextBox)e.Row.FindControl("txtFinalizedDislpayMachine");
                            txtW.Visible = false;
                            txtN.Visible = true;
                            lblN.Visible = true;
                            txtFinalizedDislpayMachine.Visible = false;
                            string Range = "", Symbol = "";
                            double minValue = 0, maxValue = 0;
                            if (common.myInt(txtN.MaxLength) == 0)
                            {
                                txtN.MaxLength = 10;
                            }
                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                ddlRange.Enabled = false;
                                txtN.Enabled = false;
                                txtN.CssClass = "LinkedWithMachine"; // Data non editable
                                ddlRange.BackColor = Color.FromName("Burlywood"); ;
                            }
                            if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                            {
                                txtN.BackColor = Color.Empty;
                                ddlRange.Enabled = false;
                                ddlRange.Skin = "Telerik";
                                txtN.Enabled = false;
                                txtN.CssClass = "Disabledtxt"; // Data non editable

                            }
                            //Checkin whether result Varies machine Wise or not
                            if (common.myBool(ViewState["MachineWise"]) == true)
                            {

                                lblLocation.Visible = true;
                                ddlRange.Visible = true;
                                txtN.Width = 180;
                                if (objDs.Tables[3].Rows.Count > 0)
                                {
                                    objDv = objDs.Tables[3].DefaultView;
                                    objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'"; //0
                                    if (objDv.Count > 0)
                                    {
                                        foreach (DataRowView dr in objDv)
                                        {

                                            RadComboBoxItem item = new RadComboBoxItem();
                                            item.Text = (string)dr["MachineName"];
                                            item.Value = dr["machineId"].ToString();
                                            item.Attributes.Add("FieldId", common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim());
                                            ddlRange.Items.Add(item);
                                            item.DataBind();

                                        }

                                        ViewState["Range"] = objDv.Table;
                                        objDv.RowFilter = "MachineId=" + ddlRange.SelectedValue + "And FieldId ='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'";
                                        if (objDv.Count > 0)
                                        {
                                            minValue = common.myDbl(objDv[0]["minValue"]);
                                            maxValue = common.myDbl(objDv[0]["maxValue"]);
                                            Symbol = common.myStr(objDv[0]["Symbol"]);
                                            Range = " (" + common.myDbl(minValue) + " " + common.myStr(Symbol) + " " + common.myDbl(maxValue) + ") ";
                                            txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                            txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                            lblN.Text = Range;
                                        }
                                    }
                                    else
                                    {
                                        lblN.Text += " (Range:&nbsp;Undefined)";
                                    }


                                }
                            }
                            else
                            {

                                lblN.Text = common.myStr(hdnUnitName.Value);
                                if (common.myStr(hdnMinValue.Value).Trim() == ""
                                    && common.myStr(hdnMaxValue.Value).Trim() == "")
                                {
                                    lblN.Text += " (Range:&nbsp;Undefined)";
                                }
                                else
                                {
                                    lblN.Text += " (" + common.myDbl(hdnMinValue.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(hdnMaxValue.Value) + ") ";
                                }
                                if (hdnMinValue.Value.Length > 0 && hdnMaxValue.Value.Length > 0)
                                {
                                    if (common.myStr(txtN.Text) != "")
                                    {
                                        if (common.myDbl(hdnMinValue.Value) <= common.myDbl(txtN.Text)
                                            && common.myDbl(hdnMaxValue.Value) >= common.myDbl(txtN.Text))
                                        {
                                            txtN.ForeColor = Color.Black;
                                        }
                                        else
                                        {
                                            txtN.ForeColor = Color.Red;
                                        }
                                    }
                                    txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                    txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                }
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        txtN.BackColor = Color.FromName("#EBDDE2");
                                        ddlRange.BackColor = Color.FromName("#EBDDE2");
                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                txtN.Enabled = true;
                                                ddlRange.Enabled = true;
                                            }
                                            else
                                            {
                                                txtN.Enabled = false;
                                                ddlRange.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                txtN.Enabled = false;
                                                ddlRange.Enabled = false;
                                            }
                                            else
                                            {
                                                txtN.Enabled = true;
                                                ddlRange.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            txtN.BackColor = Color.Empty;
                                            ddlRange.Enabled = false;
                                            txtN.Enabled = false;
                                            txtN.CssClass = "LinkedWithMachine"; // Data non editable
                                            ddlRange.BackColor = Color.FromName("Burlywood"); ;
                                            //ddlRange.Skin = "DisabledComboMachine";
                                        }
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtN.BackColor = Color.Empty;
                                        ddlRange.Enabled = false;
                                        ddlRange.Skin = "Telerik";
                                        txtN.Enabled = false;
                                        txtN.CssClass = "Disabledtxt"; // Data non editable
                                        txtFinalizedDislpayMachine.CssClass = "Disabledtxt";
                                        txtFinalizedDislpayMachine.Enabled = false;

                                    }

                                    if ((common.myStr(objDt.Rows[0]["RefRangeMachineId"]) != "") && (common.myStr(objDt.Rows[0]["RefRangeMachineId"]) != "0"))
                                    {
                                        if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                        {
                                            txtN.Width = 180;
                                            txtN.Enabled = false;
                                            txtN.CssClass = "Disabledtxt";
                                            txtFinalizedDislpayMachine.Visible = true;
                                            txtFinalizedDislpayMachine.CssClass = "Disabledtxt";
                                            txtFinalizedDislpayMachine.Enabled = false;
                                            lblLocation.Visible = true;
                                            ddlRange.Visible = false;
                                            txtFinalizedDislpayMachine.Text = common.myStr(objDt.Rows[0]["RefRangeMachineName"]);
                                            lblN.Text = common.myStr(hdnUnitName.Value);
                                            if (common.myStr(hdnMinValue.Value).Trim() == ""
                                                && common.myStr(hdnMaxValue.Value).Trim() == "")
                                            {
                                                lblN.Text += " (Range:&nbsp;Undefined)";
                                            }
                                            else
                                            {
                                                lblN.Text += " (" + common.myDbl(hdnMinValue.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(hdnMaxValue.Value) + ") ";
                                            }
                                        }
                                    }
                                    if (common.myBool(ViewState["MachineWise"]) == true)
                                    {

                                        ddlRange.SelectedValue = common.myStr(objDt.Rows[0]["RefRangeMachineId"]);
                                        txtN.Text = common.myStr(objDt.Rows[0]["FieldValue"]);//rafat
                                        if (ddlRange.Items.Count > 0)
                                        {
                                            objDv.RowFilter = "MachineId=" + ddlRange.SelectedValue + "And FieldId ='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'";
                                            if (objDv.Count > 0)
                                            {
                                                minValue = common.myDbl(objDv[0]["minValue"]);
                                                maxValue = common.myDbl(objDv[0]["maxValue"]);
                                                Symbol = common.myStr(objDv[0]["Symbol"]);
                                                Range = " (" + common.myDbl(minValue) + " " + common.myStr(Symbol) + " " + common.myDbl(maxValue) + ") ";
                                                txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                                txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(minValue) + ", " + common.myDbl(maxValue) + ");");
                                                lblN.Text = Range;
                                            }

                                            if (txtN.Text.Trim() != "")
                                            {
                                                if (minValue <= common.myDbl(txtN.Text) &&
                                                    maxValue >= common.myDbl(txtN.Text))
                                                {
                                                    txtN.ForeColor = Color.Black;
                                                }
                                                else
                                                {
                                                    txtN.ForeColor = Color.Red;
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {

                                        txtN.Text = common.myStr(objDt.Rows[0]["FieldValue"]);//rafat
                                        lblN.Text = common.myStr(hdnUnitName.Value);
                                        if (common.myStr(hdnMinValue.Value).Trim() == ""
                                            && common.myStr(hdnMaxValue.Value).Trim() == "")
                                        {
                                            lblN.Text += " (Range:&nbsp;Undefined)";
                                        }
                                        else
                                        {
                                            lblN.Text += " (" + common.myDbl(hdnMinValue.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(hdnMaxValue.Value) + ") ";
                                        }
                                        if (hdnMinValue.Value.Length > 0 && hdnMaxValue.Value.Length > 0)
                                        {
                                            if (common.myStr(txtN.Text) != "")
                                            {
                                                if (common.myDbl(hdnMinValue.Value) <= common.myDbl(txtN.Text)
                                                    && common.myDbl(hdnMaxValue.Value) >= common.myDbl(txtN.Text))
                                                {
                                                    txtN.ForeColor = Color.Black;
                                                }
                                                else
                                                {
                                                    txtN.ForeColor = Color.Red;
                                                }
                                            }
                                            txtN.Attributes.Add("onkeypress", "checkRange('" + txtN.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                            txtN.Attributes.Add("onkeyup", "checkRange('" + txtN.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                        }
                                    }


                                }
                            }


                        }
                        //Block for Formula type field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Formula))
                        {
                            Label lblFieldName = (Label)e.Row.FindControl("lblFieldName");
                            lblFieldName.Text = common.myStr(lblFieldName.Text) + "&nbsp;(Fx)";

                            cCtlType = fieldType;
                            TextBox txtF = (TextBox)e.Row.FindControl("txtF");
                            Button btnF = (Button)e.Row.FindControl("btnF");
                            Label lblF = (Label)e.Row.FindControl("lblF");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            TextBox TxtFRemarks = (TextBox)e.Row.FindControl("TxtFRemarks");
                            txtW.Visible = false;
                            txtF.Visible = true;
                            lblF.Visible = true;
                            btnF.Visible = true;
                            TxtFRemarks.Visible = true;
                            txtF.Attributes.Add("onfocus", "onFocusTxtF('" + btnF.ClientID + "');");
                            txtF.Style.Add("Width", "50px");

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    //Checking This is Machine Result then the user has edit Permission or not
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        txtF.BackColor = Color.FromName("#EBDDE2");

                                        TxtFRemarks.BackColor = Color.FromName("#EBDDE2");

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                txtF.Enabled = true;
                                                TxtFRemarks.Enabled = true;
                                            }
                                            else
                                            {
                                                txtF.Enabled = false;
                                                TxtFRemarks.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                txtF.Enabled = false;
                                                TxtFRemarks.Enabled = false;
                                            }
                                            else
                                            {
                                                txtF.Enabled = true;
                                                TxtFRemarks.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            btnF.Enabled = false;
                                            btnF.CssClass = "Disabledbtn";
                                            txtF.Enabled = false;
                                            txtF.CssClass = "LinkedWithMachine"; // Data non editable
                                            TxtFRemarks.Enabled = false;
                                            TxtFRemarks.CssClass = "LinkedWithMachine"; // Data non editable
                                        }


                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtF.Enabled = false;
                                        txtF.CssClass = "Disabledtxt"; // Data non editable
                                        btnF.Enabled = false;
                                        btnF.CssClass = "Disabledbtn";
                                        TxtFRemarks.Enabled = false;
                                        TxtFRemarks.CssClass = "Disabledtxt"; // Data non editable
                                    }
                                    txtF.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    TxtFRemarks.Text = common.myStr(objDt.Rows[0]["FindingRemarks"]);

                                }


                            }
                            HiddenField hdnUnitName = (HiddenField)e.Row.FindControl("hdnUnitName");
                            HiddenField hdnUnitId = (HiddenField)e.Row.FindControl("hdnUnitId");
                            HiddenField hdnMinValue = (HiddenField)e.Row.FindControl("hdnMinValue");
                            HiddenField hdnMaxValue = (HiddenField)e.Row.FindControl("hdnMaxValue");
                            HiddenField hdnSymbol = (HiddenField)e.Row.FindControl("hdnSymbol");
                            lblF.Text = common.myStr(hdnUnitName.Value);
                            if (common.myStr(hdnMinValue.Value).Trim() == ""
                                && common.myStr(hdnMaxValue.Value).Trim() == "")
                            {
                                lblF.Text += " (Range:&nbsp;Undefined)";
                            }
                            else
                            {
                                lblF.Text += " (" + common.myDbl(hdnMinValue.Value) + " " + common.myStr(hdnSymbol.Value) + " " + common.myDbl(hdnMaxValue.Value) + ") ";
                            }

                            if (hdnMinValue.Value.Length > 0 && hdnMaxValue.Value.Length > 0)
                            {
                                if (common.myStr(txtF.Text) != "")
                                {
                                    if (common.myDbl(hdnMinValue.Value) <= common.myDbl(txtF.Text)
                                        && common.myDbl(hdnMaxValue.Value) >= common.myDbl(txtF.Text))
                                    {
                                        txtF.ForeColor = Color.Black;
                                    }
                                    else
                                    {
                                        txtF.ForeColor = Color.Red;
                                    }
                                }
                                txtF.Attributes.Add("onkeypress", "checkRange('" + txtF.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                                txtF.Attributes.Add("onkeyup", "checkRange('" + txtF.ClientID + "', " + common.myDbl(hdnMinValue.Value) + ", " + common.myDbl(hdnMaxValue.Value) + ");");
                            }
                        }
                        //Block for Multiline Textbox
                        else if (fieldType == common.getEnumStrVal(eFieldType.TextMultiLine))
                        {
                            TextBox txtM = (TextBox)e.Row.FindControl("txtM");


                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl("ddlMultilineFormats"); //3
                            if (objDs.Tables[1] != null)
                            {
                                DataRow[] drCheck = objDs.Tables[1].Select("ValueId=0 and FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'");
                                if (drCheck.Length == 0)
                                {
                                    DataRow dr = objDs.Tables[1].NewRow();
                                    dr["ValueId"] = 0;
                                    dr["ValueName"] = "[Select Text Format]";
                                    dr["FieldId"] = common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text);
                                    dr["SequenceNo"] = 0;
                                    objDs.Tables[1].Rows.InsertAt(dr, 0);
                                }
                                objDv = objDs.Tables[1].DefaultView;
                                objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'"; //0
                                objDv.Sort = "SequenceNo Asc";
                                ddl.Visible = true;
                                ddl.DataSource = objDv; //0
                                ddl.DataValueField = "ValueId";
                                ddl.DataTextField = "ValueName";
                                ddl.DataBind();
                                objDv = objDs.Tables[1].DefaultView;

                            }



                            txtW.Visible = false;
                            txtM.Visible = true;
                            txtM.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txtM.ClientID + "');");

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {

                                    txtM.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    //Checking This is Machine Result then the user has edit Permission or not
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        txtM.BackColor = Color.FromName("#EBDDE2");
                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                txtM.Enabled = true;
                                            }
                                            else
                                            {
                                                txtM.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                txtM.Enabled = false;
                                            }
                                            else
                                            {
                                                txtM.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            txtM.CssClass = "LinkedWithMachine";
                                            txtM.Enabled = true;
                                        }
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtM.Enabled = false;
                                        txtM.CssClass = "Disabledtxt"; // Data non editable
                                        //Added on 26-08-2014 Start Naushad  Ali
                                        ddl.Visible = false;
                                        //Addded on 26-08-2014 End Naushad ali
                                    }

                                }

                                //Added on 26-08-2014 Start Naushad  Ali
                                ddl.Visible = false;
                                //Addded on 26-08-2014 End Naushad ali

                            }

                        }
                        //Block For Ttext Format Type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.TextFormats))
                        {
                            cCtlType = fieldType;
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl("ddlTemplateFieldFormats"); //3
                            //Added on Start  26-08-2014 Naushad
                            RadComboBox ddlMultilineFormats = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl("ddlMultilineFormats"); //3

                            //Added on End 26-08-2014 Ali
                            if (objDs.Tables[1] != null)
                            {

                                DataRow[] drCheck = objDs.Tables[1].Select("ValueId=0 and FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'");
                                if (drCheck.Length == 0)
                                {
                                    DataRow dr = objDs.Tables[1].NewRow();
                                    dr["ValueId"] = 0;
                                    dr["ValueName"] = "[Select Text Format]";
                                    dr["FieldId"] = common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text);
                                    dr["SequenceNo"] = 0;
                                    objDs.Tables[1].Rows.InsertAt(dr, 0);
                                }
                                objDv = objDs.Tables[1].DefaultView;
                                objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "'"; //0
                                objDv.Sort = "SequenceNo Asc";
                                ddl.Visible = true;
                                ddl.DataSource = objDv; //0
                                ddl.DataValueField = "ValueId";
                                ddl.DataTextField = "ValueName";
                                ddl.DataBind();
                            }
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            txtW.Visible = true;
                            if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                            {
                                txtW.Enabled = false; // Data non editable
                                ddl.Enabled = false;
                                ddl.Skin = "Telerik";
                                //Added on 26-08-2014 Start Naushad
                                //For disable  Drop down   
                                ddlMultilineFormats.Visible = false;
                                ddl.Visible = false;


                                //Added on 26-08-2014 End Naushad
                            }

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {

                                    // string Content = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    //Content = Content.Replace("font size=1", "<span style=\"font-size: 10px");
                                    //Content = Content.Replace("font size=2", "<span style=\"font-size: 13px");
                                    //Content = Content.Replace("font size=3", "<span style=\"font-size: 16px");
                                    //Content = Content.Replace("font size=4", "<span style=\"font-size: 18px");
                                    //Content = Content.Replace("font size=5", "<span style=\"font-size: 24px");
                                    //Content = Content.Replace("font size=6", "<span style=\"font-size: 32px");
                                    //Content = Content.Replace("font size=7", "<span style=\"font-size: 48px");
                                    //Content = Content.Replace("<u>", "<span style=\"text-decoration: underline;\">");
                                    //Content = Content.Replace("</u>", "</span>");
                                    //Content = Content.Replace("</font>", "</span>");
                                    //Content = Content.Replace("</font>", "</span>");


                                    txtW.Content = Server.HtmlDecode(objDt.Rows[0]["FieldValue"].ToString());
                                }
                            }
                        }
                        //Block For Checkbox type Fields
                        else if (fieldType == common.getEnumStrVal(eFieldType.CheckBox))
                        {
                            cCtlType = fieldType;
                            objDv = objDs.Tables[1].DefaultView;
                            objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "' AND ValueType='V'"; //0
                            objDv.Sort = "SequenceNo Desc";

                            Repeater rpt = (Repeater)e.Row.Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.CheckBox));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            txtW.Visible = false;
                            tbl1.Visible = false;
                            rpt.Visible = true;
                            rpt.DataSource = objDv;
                            rpt.DataBind();
                            HtmlTextArea txtRemarks = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                            txtRemarks.Visible = false;
                            foreach (RepeaterItem item in rpt.Items)
                            {
                                HtmlTextArea CT = (HtmlTextArea)item.FindControl("CT");
                                CT.Attributes.Add("onkeypress", "javascript:return AutoChange('" + CT.ClientID + "');");
                                CT.Attributes.Add("onkeydown", "javascript:return AutoChange('" + CT.ClientID + "');");
                                if (objDt != null)
                                {
                                    if (objDt.Rows.Count > 0)
                                    {
                                        HiddenField hdn = (HiddenField)item.FindControl("hdnCV");
                                        CheckBox chk = (CheckBox)item.FindControl(common.getEnumStrVal(eFieldType.CheckBox));
                                        foreach (DataRow drow in objDt.Rows)
                                        {
                                            if (common.myStr(drow["FieldValue"]).Trim() == common.myStr(hdn.Value).Trim())
                                            {
                                                chk.Checked = true; //rafat
                                                if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                                {
                                                    chk.Enabled = false;
                                                    //txtM.CssClass = "Disabledtxt"; // Data non editable
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Block for Boolean Type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Boolean))
                        {
                            cCtlType = fieldType;
                            RadComboBox ddlB = (RadComboBox)e.Row.FindControl(common.getEnumStrVal(eFieldType.Boolean));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            txtW.Visible = false;
                            tbl1.Visible = false;
                            ddlB.Visible = true;

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                    {
                                        ddlB.Enabled = false;
                                        ddlB.EnableEmbeddedSkins = false;
                                        ddlB.BackColor = Color.FromName("Burlywood");
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        ddlB.Enabled = false;
                                        ddlB.Skin = "Telerik"; // Data non editable
                                    }
                                    objDvValue = objDt.DefaultView;
                                    if (objDvValue.Table.Columns["FieldId"] != null)
                                    {
                                        objDvValue.RowFilter = "FieldId='" + e.Row.Cells[(int)eServicesGrid.FieldID].Text.Trim() + "'";
                                    }

                                    objDt = objDvValue.ToTable();
                                    if (objDt.Rows.Count > 0)
                                    {
                                        if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                        {
                                            ddlB.EnableEmbeddedSkins = false;
                                            ddlB.Skin = "DisabledCombo";

                                            if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                            {
                                                if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                                {
                                                    ddlB.Enabled = true;
                                                }
                                                else
                                                {
                                                    ddlB.Enabled = false;
                                                }
                                            }
                                            else
                                            {
                                                if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                                {
                                                    ddlB.Enabled = false;
                                                }
                                                else
                                                {
                                                    ddlB.Enabled = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                            {
                                                ddlB.Enabled = false;
                                                ddlB.EnableEmbeddedSkins = false;
                                                ddlB.BackColor = Color.FromName("Burlywood");
                                            }
                                        }
                                        if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                        {
                                            ddlB.Enabled = false;
                                            ddlB.Skin = "Telerik"; // Data non editable
                                        }
                                        if (common.myStr(objDt.Rows[0]["FieldValue"]) == "1")//rafat
                                        {
                                            ddlB.SelectedValue = "1";
                                        }
                                        else if (common.myStr(objDt.Rows[0]["FieldValue"]) == "0")
                                        {
                                            ddlB.SelectedValue = "0";
                                        }
                                        else
                                        {
                                            ddlB.SelectedValue = "-1";
                                        }


                                    }
                                }
                            }


                        }
                        //Block For Dropdown Type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.ListofChoices))
                        {
                            cCtlType = fieldType;
                            objDv = objDs.Tables[1].DefaultView;
                            objDv.RowFilter = "FieldId='" + common.myStr(e.Row.Cells[(int)eServicesGrid.FieldID].Text).Trim() + "' AND ValueType='V'"; //0
                            objDv.Sort = "SequenceNo Asc";

                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.ListofChoices));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            TextBox txtFinalizedDislpay = (TextBox)e.Row.FindControl("txtFinalizedDislpay");
                            Label lblD = (Label)e.Row.FindControl("lblD");
                            lblD.Visible = true;
                            txtW.Visible = false;
                            tbl1.Visible = false;
                            ddl.Visible = true;
                            txtFinalizedDislpay.Visible = false;
                            foreach (DataRowView dr in objDv)
                            {
                                RadComboBoxItem item = new RadComboBoxItem();
                                item.Text = (string)dr["ValueName"];
                                item.Value = dr["ValueId"].ToString();
                                item.Attributes.Add("FindingRemarks", dr["Remarks"].ToString());
                                ddl.Items.Add(item);
                                item.DataBind();
                            }

                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                ddl.Enabled = false;
                                ddl.EnableEmbeddedSkins = false;
                                ddl.BackColor = Color.FromName("Burlywood");

                                //ddl.Skin = "DisabledCombo";
                            }
                            if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                            {
                                txtFinalizedDislpay.Visible = true;
                                txtFinalizedDislpay.Enabled = false;
                                txtFinalizedDislpay.CssClass = "Disabledtxt";
                                ddl.Visible = false;
                                ddl.Enabled = false;
                                ddl.Skin = "Telerik"; // Data non editable
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindItemByValue(common.myStr(objDt.Rows[0]["FieldValue"])));//rafat
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        ddl.EnableEmbeddedSkins = false;
                                        ddl.BackColor = Color.FromName("#EBDDE2");

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                ddl.Enabled = true;
                                            }
                                            else
                                            {
                                                ddl.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                ddl.Enabled = false;
                                            }
                                            else
                                            {
                                                ddl.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            ddl.Enabled = false;
                                            ddl.EnableEmbeddedSkins = false;
                                            ddl.BackColor = Color.FromName("Burlywood");

                                        }
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Text = common.myStr(objDt.Rows[0]["TextValue"]);
                                        lblD.Text = common.myStr(objDt.Rows[0]["FindingRemarks"]);
                                        ddl.Skin = "Telerik";
                                        txtFinalizedDislpay.Enabled = false;
                                        txtFinalizedDislpay.CssClass = "Disabledtxt";
                                    }

                                }
                            }
                            if ((common.myStr(ViewState["StatusCode"]) != "RF") || (common.myStr(ViewState["StatusCode"]) != "RP"))//Result Finalized or Result Provisonal
                            {
                                string strRemarks = common.myStr(ddl.SelectedItem.Attributes["FindingRemarks"]);
                                if (strRemarks != "")
                                {
                                    lblD.Text = " (" + strRemarks + ")";
                                }
                                else
                                {
                                    lblD.Text = "";
                                }
                            }
                        }
                        //Block for Date type field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Date))
                        {
                            HtmlTable tblDate = (HtmlTable)e.Row.Cells[(int)eServicesGrid.Values].FindControl("tblDate");
                            TextBox txtDate = e.Row.Cells[(int)eServicesGrid.Values].FindControl("txtDate") as TextBox;
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            txtW.Visible = false;

                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    txtDate.Enabled = true;
                                    txtDate.CssClass = "clsTextbox";
                                    txtDate.Text = common.myStr(objDt.Rows[0]["FieldValue"]);//rafat
                                }
                            }

                            txtDate.Attributes.Add("onblur", "validateDate('" + txtDate.ClientID + "');");
                            tblDate.Visible = true;
                        }
                        // Organism Dropdown start
                        else if (fieldType == common.getEnumStrVal(eFieldType.Organism))
                        {
                            cCtlType = fieldType;
                            objDv = objDs.Tables[1].DefaultView;
                            objDv.RowFilter = "FieldId=0 And ValueType='O'"; //0
                            objDv.Sort = "SequenceNo,ValueName Asc";
                            TextBox txtFinalizedDislpay = (TextBox)e.Row.FindControl("txtFinalizedDislpay");
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.Organism));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            HtmlTable tblOrganism = (HtmlTable)e.Row.FindControl("tblOrganism");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            TextBox txtMOrg = (TextBox)e.Row.FindControl("txtMOrg");
                            ddl.DataSource = objDv;
                            ddl.DataTextField = "ValueName";
                            ddl.DataValueField = "ValueId";
                            ddl.DataBind();
                            txtW.Visible = false;
                            tbl1.Visible = false;
                            ddl.Visible = true;
                            txtFinalizedDislpay.Visible = false;
                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                ddl.Enabled = false;
                                ddl.EnableEmbeddedSkins = false;
                                ddl.BackColor = Color.FromName("Burlywood");
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    txtMOrg.Text = common.myStr(objDt.Rows[0]["FindingRemarks"]);
                                }
                            }


                            if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                            {
                                txtFinalizedDislpay.Visible = true;
                                txtFinalizedDislpay.Enabled = false;
                                txtFinalizedDislpay.CssClass = "Disabledtxt";
                                ddl.Visible = false;
                                ddl.Enabled = false;
                                ddl.Skin = "Telerik"; // Data non editable
                            }


                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindItemByValue(common.myStr(objDt.Rows[0]["FieldValue"])));//rafat
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        ddl.EnableEmbeddedSkins = false;
                                        ddl.BackColor = Color.FromName("#EBDDE2");

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                ddl.Enabled = true;
                                            }
                                            else
                                            {
                                                ddl.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                ddl.Enabled = false;
                                            }
                                            else
                                            {
                                                ddl.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            ddl.Enabled = false;
                                            ddl.EnableEmbeddedSkins = false;
                                            ddl.BackColor = Color.FromName("Burlywood");
                                        }
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Visible = true;
                                        txtFinalizedDislpay.Enabled = false;
                                        txtFinalizedDislpay.CssClass = "Disabledtxt";
                                        ddl.Visible = false;
                                        ddl.Enabled = false;
                                        ddl.Skin = "Telerik"; // Data non editable
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Text = common.myStr(objDt.Rows[0]["TextValue"]);

                                    }

                                }
                            }
                        }
                        //Organism Dropdown End
                        // Enzyme Dropdown start
                        else if (fieldType == common.getEnumStrVal(eFieldType.Enzyme))
                        {
                            cCtlType = fieldType;
                            objDv = objDs.Tables[1].DefaultView;
                            objDv.RowFilter = "FieldId=0 And ValueType='E'"; //0
                            objDv.Sort = "ValueName Asc";
                            RadComboBox ddl = (RadComboBox)e.Row.Cells[(int)eServicesGrid.Values].FindControl(common.getEnumStrVal(eFieldType.Enzyme));
                            HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            TextBox txtFinalizedDislpay = (TextBox)e.Row.FindControl("txtFinalizedDislpay");
                            txtW.Visible = false;
                            tbl1.Visible = false;
                            ddl.Visible = true;
                            ddl.DataSource = objDv;
                            ddl.DataTextField = "ValueName";
                            ddl.DataValueField = "ValueId";
                            ddl.DataBind();
                            if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                            {
                                txtFinalizedDislpay.Visible = true;
                                txtFinalizedDislpay.Enabled = false;
                                txtFinalizedDislpay.CssClass = "Disabledtxt";
                                ddl.Enabled = false;
                                ddl.Visible = false;
                                ddl.Skin = "Telerik"; // Data non editable
                            }
                            if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                            {
                                ddl.Enabled = false;
                                ddl.EnableEmbeddedSkins = false;

                                ddl.BackColor = Color.FromName("Burlywood");
                            }
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindItemByValue(common.myStr(objDt.Rows[0]["FieldValue"])));//rafat
                                    if (common.myInt(objDt.Rows[0]["MachineId"]) > 0)
                                    {
                                        ddl.EnableEmbeddedSkins = false;
                                        ddl.Skin = "DisabledCombo";

                                        if (common.myStr(ViewState["EditMachineWise"]) == "Y")
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && Convert.ToBoolean(objDt.Rows[0]["EditMachineResult"]) == true)
                                            {
                                                ddl.Enabled = true;
                                            }
                                            else
                                            {
                                                ddl.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (common.myInt(objDt.Rows[0]["MachineId"]) > 0 && common.myStr(ViewState["Permission"]) != "Y")
                                            {
                                                ddl.Enabled = false;
                                            }
                                            else
                                            {
                                                ddl.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (common.myBool(ViewState["LinkedWithMachine"]) == true) //Link with machine 
                                        {
                                            ddl.Enabled = false;
                                            ddl.EnableEmbeddedSkins = false;
                                            ddl.BackColor = Color.FromName("Burlywood");
                                        }
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal
                                    {
                                        txtFinalizedDislpay.Visible = true;
                                        txtFinalizedDislpay.Enabled = false;
                                        txtFinalizedDislpay.CssClass = "Disabledtxt";
                                        ddl.Enabled = false;
                                        ddl.Visible = false;
                                        ddl.Skin = "Telerik"; // Data non editable
                                    }
                                    if ((common.myStr(ViewState["StatusCode"]) == "RF") || (common.myStr(ViewState["StatusCode"]) == "RP"))//Result Finalized or Result Provisonal 
                                    {
                                        txtFinalizedDislpay.Text = common.myStr(objDt.Rows[0]["TextValue"]);
                                    }

                                }
                            }
                        }
                        //Enzyme Dropdown End
                        else if (fieldType == common.getEnumStrVal(eFieldType.Sensitivity))
                        {
                            cCtlType = fieldType;
                            Button btnSN = (Button)e.Row.FindControl("btnSN");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                            txtW.Visible = false;
                            btnSN.Visible = true;
                        }






                        //Block for Time type Field
                        else if (fieldType == common.getEnumStrVal(eFieldType.Time))
                        {
                            //
                            Label lblTimeString = (Label)e.Row.FindControl("lblTimeString");
                            HtmlTable tblDate = (HtmlTable)e.Row.FindControl("Tabletm");
                            RadComboBox ddlHr = (RadComboBox)e.Row.FindControl("ddlHr");
                            RadComboBox ddlMin = (RadComboBox)e.Row.FindControl("ddlMin");
                            RadComboBox ddlSec = (RadComboBox)e.Row.FindControl("ddlSec");
                            RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                            if (common.myStr(objDs.Tables[0].Rows[0]["FormulaDefinition"]) != "")
                            {
                                lblTimeString.Text = "(" + common.myStr(objDs.Tables[0].Rows[0]["FormulaDefinition"]).Trim() + ")";
                            }
                            txtW.Visible = false;
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count != 0)
                                {
                                    if (common.myStr(objDt.Rows[0]["FieldValue"]) != "")
                                    {

                                        string[] FieldValue = common.myStr(objDt.Rows[0]["FieldValue"]).Split(' ');
                                        if (FieldValue.Length == 7)
                                        {
                                            ddlHr.SelectedValue = FieldValue[0];
                                            ddlMin.SelectedValue = FieldValue[2];
                                            ddlSec.SelectedValue = FieldValue[4];
                                        }
                                        if (FieldValue.Length == 5)
                                        {
                                            if (FieldValue[1] == "hr")
                                            {
                                                ddlHr.SelectedValue = FieldValue[0];
                                            }
                                            if (FieldValue[1] == "min")
                                            {
                                                ddlMin.SelectedValue = FieldValue[0];
                                            }
                                            if (FieldValue[1] == "sec")
                                            {
                                                ddlSec.SelectedValue = FieldValue[0];
                                            }

                                            if (FieldValue[3] == "hr")
                                            {
                                                ddlHr.SelectedValue = FieldValue[2];
                                            }
                                            if (FieldValue[3] == "min")
                                            {
                                                ddlMin.SelectedValue = FieldValue[2];
                                            }
                                            if (FieldValue[3] == "sec")
                                            {
                                                ddlSec.SelectedValue = FieldValue[2];
                                            }
                                        }
                                        if (FieldValue.Length == 3)
                                        {
                                            if (FieldValue[1] == "hr")
                                            {
                                                ddlHr.SelectedValue = FieldValue[0];
                                            }
                                            if (FieldValue[1] == "min")
                                            {
                                                ddlMin.SelectedValue = FieldValue[0];
                                            }
                                            if (FieldValue[1] == "sec")
                                            {
                                                ddlSec.SelectedValue = FieldValue[0];
                                            }
                                        }

                                    }
                                }
                            }
                            tblDate.Visible = true;


                        }

                    }




                    else
                    {
                        e.Row.Cells[(int)eServicesGrid.Values].Text = "No Record Found!";
                    }

                    if (cCtlType == common.getEnumStrVal(eFieldType.TextSingleLine)
                        || cCtlType == common.getEnumStrVal(eFieldType.Numeric)
                        || cCtlType == common.getEnumStrVal(eFieldType.Formula)
                        || cCtlType == common.getEnumStrVal(eFieldType.TextMultiLine))
                    {
                        HtmlTextArea txtMulti1 = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        txtMulti1.Visible = false;
                    }

                    HtmlTextArea txt = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                    txt.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txt.ClientID + "');");
                    txt.Attributes.Add("onkeydown", "javascript:return AutoChange('" + txt.ClientID + "');");
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }

    protected void BindTemplateGrid()
    {
        try
        {
            string[] ageGender = common.myStr(Request.QueryString["AgeInDays"]).Split('/');
            string StatusCode = common.myStr(Request.QueryString["StatusCode"]);
            ViewState["StatusCode"] = StatusCode;
            string ServiceId = common.myStr(Request.QueryString["SERVICEID"]);
            string DIAG_SAMPLEID = common.myStr(Request.QueryString["DIAG_SAMPLEID"]);
            string SOURCE = common.myStr(Request.QueryString["SOURCE"]);

            DataTable dt = new DataTable();
            objval = new BaseC.clsLISPhlebotomy(sConString);
            //Checking Whether the Global List collLabTemplate Is Stored in ViewState or Not

            /***************** get data for fields *****************/

            int iIsCallFromLab = 1;
            if (common.myStr(Request.QueryString["StatusCode"]) != "RF"
                && common.myStr(Session["ModuleName"]) != "LIS")
            {
                iIsCallFromLab = 0;
            }
            int AgeInDays = objval.GetPatientAgeInDaysForRange(SOURCE, DIAG_SAMPLEID, ServiceId);

            //code here
            string result = IsDeptRadiologyAndPacs(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]), common.myInt(ServiceId));
            //if (result.Equals("1"))
            //{
            //    objDs = DiagGetLabNoInvFormatsForXR(common.myStr(SOURCE), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]), common.myInt(DIAG_SAMPLEID), common.myInt(ServiceId), common.myStr(StatusCode));
            //    if (objDs.Tables[0].Rows.Count > 0)
            //    {
            //        litResult.Text = objDs.Tables[0].Rows[0]["Resulttxt"].ToString();
            //    }
            //    else
            //    {
            //        litResult.Text = "No data found.";
            //    }
            //}
            //else
            //{


            objDs = objval.getLabNoInvFormats(common.myStr(SOURCE),
                            common.myInt(Session["FacilityID"]),
                            common.myInt(DIAG_SAMPLEID),
                            common.myInt(ServiceId), "", AgeInDays,
                            common.myStr(StatusCode),
                            common.myInt(Session["HospitalLocationId"]),
                            iIsCallFromLab);

            ddlResultId.Items.Clear();


            if (objDs.Tables[2].Rows.Count > 0)
            {
                dt = new DataTable();
                dt = objDs.Tables[2];
                int maxResultId = Convert.ToInt32(dt.Compute("max(ResultId)", string.Empty));
                //objLabTemplate.ResultEntered = maxResultId;
                for (int i = 1; i <= maxResultId + 1; i++)
                {
                    RadComboBoxItem Item = new RadComboBoxItem(common.myStr(i), common.myStr(i));
                    ddlResultId.Items.Add(Item);
                }

            }
            else
            {
                RadComboBoxItem Item = new RadComboBoxItem(common.myStr(1), common.myStr(1));
                ddlResultId.Items.Add(Item);

            }
            DataView dv = objDs.Tables[0].DefaultView;
            dv.Sort = "SequenceNo Asc";
            ddlResultId.SelectedValue = "1";
            gvSelectedServices.DataSource = dv;
            gvSelectedServices.DataBind();
            //Adding the List in global List
            // }
        }
        catch (Exception Ex)
        {

        }
    }
    private string IsDeptRadiologyAndPacs(int iHospID, int iLoginFacilityId, int iServiceID)
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        return objval.IsDeptRadiologyAndPacs(iHospID, iLoginFacilityId, iServiceID);
    }
    private DataSet DiagGetLabNoInvFormatsForXR(string SOURCE, int iHospID, int iLoginFacilityId, int DIAGSAMPLEID, int iServiceID, string StatusCode)
    {
        objval = new BaseC.clsLISPhlebotomy(sConString);
        return objval.GetLabNoInvFormatsForXR(SOURCE, iHospID, iLoginFacilityId, DIAGSAMPLEID, iServiceID, StatusCode);
    }


}
