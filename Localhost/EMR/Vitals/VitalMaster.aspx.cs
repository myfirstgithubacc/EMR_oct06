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
using System.Collections.Generic;
using AjaxControlToolkit;

public partial class EMR_Vitals_VitalMaster : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
    static DataTable VitalDt = new DataTable();
    BaseC.ParseData bcText = new BaseC.ParseData();
    Boolean bitEdit = false;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        DAL.DAL dl = new DAL.DAL();

        if (!IsPostBack)
        {

            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();

            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.Text, "select UnitID, UnitName from EMRVitalUnitMaster WITH (NOLOCK) where Active=1");

            ddlConvertedId.DataSource = ds.Tables[0];
            ddlConvertedId.DataValueField = "UnitId";
            ddlConvertedId.DataTextField = "UnitName";
            ddlConvertedId.DataBind();
            //Cache["UnitName"] = ds.Tables[0];
            ddlConvertedId.Items.Insert(0, "[Select]");
            ddlConvertedId.Items[0].Value = "0";
            bindVitalGrid();
            bindUnitGrid();
            if (TabMaster.TabIndex == 0)
            {
                ibtnSaveVital.Visible = true;
                ibtnSaveUnit.Visible = false;
            }
            //lnkVitalType_Click(sender,e);
        }
    }

    void currentVitalValue()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.Security AuditCA = new BaseC.Security(sConString);
            Hashtable hshTableIn = new Hashtable();

            DataSet ds = new DataSet();
            // hshTableIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            //hshTableIn.Add("@TemplateId", ddlTemplate.SelectedValue);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "USPEMRGetVitalTemplateDetails", hshTableIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                VitalDt = ds.Tables[0];
                gvVitalView.DataSource = VitalDt;
                gvVitalView.DataBind();
                for (int i = 0; i < gvVitalView.Rows.Count; i++)
                {
                    if (gvVitalView.Rows[i].Cells[10].Text.Trim() == "DropDown")
                    {
                        DropDownList ddlFacePain = (DropDownList)gvVitalView.Rows[i].FindControl("ddlFaceDrop");
                        TextBox txtvalue1 = (TextBox)gvVitalView.Rows[i].FindControl("txtfindings1");
                        txtvalue1.Visible = false;
                        ddlFacePain.Visible = true;
                    }
                }
                if (!IsPostBack)
                    AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"]), Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void bindVitalGrid()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hsinput = new Hashtable();
            // ds = dl.FillDataSet(CommandType.Text, "select   VitalId, VitalSignName,DisplayName,MaxLength, Case When ResultType='A' then 'Age' when ResultType='S' then 'Gender' when ResultType='AS' then 'Age Gender' when  ResultType='G' then 'General' end as ResultType , Case when Active=1 then 'Active' else 'Inactive' End as Status ,ResultType as ResultTypeId,Active from EMRVitalSignTypes");
            hsinput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalList", hsinput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //DataView dv = new DataView(ds.Tables[0]);
                //dv.RowFilter = "HospitalLocationID IS NOT NULL";
                gvVitalView.DataSource = ds;
                gvVitalView.DataBind();
                for (int i = 0; i < gvVitalView.Rows.Count; i++)
                {
                    string strVitalType = gvVitalView.Rows[i].Cells[6].Text;
                    if (strVitalType.ToString().Trim() == "DropDown")
                    {
                        ImageButton imgVital = (ImageButton)gvVitalView.Rows[i].FindControl("imgVital");
                        imgVital.Visible = true;
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

    protected void gvVitalView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVitalView.PageIndex = e.NewPageIndex;
        bindVitalGrid();
    }

    //protected void lnkVitalType_Click(object sender, EventArgs e)
    //{
    //    LinkButton lnkPatient1 = sender as LinkButton;
    //    if (lnkPatient1.Text.Trim() == "DropDown")
    //    {
    //        string strvalue = lnkPatient1.CommandArgument;

    //        RadWindowForNew.NavigateUrl = "AddVitalValues.aspx?VitalId=" + strvalue + "";
    //        RadWindowForNew.Height = 500;
    //        RadWindowForNew.Width = 550;
    //        RadWindowForNew.Top = 40;
    //        RadWindowForNew.Left = 100;
    //        RadWindowForNew.Title = "Time Slot";
    //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.VisibleStatusbar = false;
    //        //lnkPatient1.Attributes.Add("onclick", "javascript:openRadWindowVital('" + strvalue + "', this )");
    //    }
    //    else
    //    {
    //        //lnkPatient1.Attributes.Remove("onclick");
    //    }
    //}

    void bindUnitGrid()
    {
        try
        {
            //  sqlstr = "select  t1.UnitId,row_number() over (order by T1.UnitId) as Sno,t1.UnitName,t1.Symbol,t1.MeasurementSystem,t1.ConversionFormula,Case When t1.MeasurementSystem='E' Then 'English' else 'Metric' end as MS,t1.ConvertedUnitId,t2.UnitName as ConvertedName,Case when t1.Active=1 then 'Active' else 'Inactive' End as Status ,t1.Active from EMRVitalUnitMaster t1 left join EMRVitalUnitMaster t2 on t1.ConvertedUnitId=t2.UnitId order by t1.UnitId";
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            //SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, sqlstr);
            // ds = dl.FillDataSet(CommandType.Text, sqlstr);
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@inyHospitalLocationId", Session["HospitalLocationID"].ToString());
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalUnits", hsinput);
            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "HospitalLocationID IS NOT NULL";
            gvUnitView.DataSource = dv;

            gvUnitView.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVitalView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        bitEdit = true;
        gvVitalView.EditIndex = e.NewEditIndex;
        bindVitalGrid();
    }

    protected void gvVitalView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVitalView.EditIndex = -1;
        bindVitalGrid();
    }

    protected void gvVitalView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Hashtable hshTable = new Hashtable();
            hshTable.Add("@intVitalId", gvVitalView.Rows[e.RowIndex].Cells[1].Text);
            TextBox txtvitalName = (TextBox)gvVitalView.Rows[e.RowIndex].Cells[1].FindControl("txtVitalName");

            hshTable.Add("@VitalName", txtvitalName.Text);

            TextBox txtShort = (TextBox)gvVitalView.Rows[e.RowIndex].Cells[2].FindControl("txtShortNameEdit");
            hshTable.Add("@DisplayName", txtShort.Text); //Request.QueryString["RegNo"];

            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            hs.Add("@chvVitalName", txtvitalName.Text.ToString());
            // hs.Add("@chvDisplayName",txtShort.Text.ToString());
            hs.Add("@VitalId", gvVitalView.Rows[e.RowIndex].Cells[1].Text);

            SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select VitalSignName, DisplayName FROM EMRVitalSignTypes WITH (NOLOCK) where VitalSignName=@chvVitalName AND VitalId <> @VitalId ", hs);
            if (objDr.Read())
            {
                lblMessage.Text = "Vital Name Allready Exist!";
            }
            else
            {
                DropDownList ddlResultType = (DropDownList)gvVitalView.Rows[e.RowIndex].Cells[4].FindControl("ddlResultTypeEdit");

                hshTable.Add("@ResultType", ddlResultType.SelectedValue);

                DropDownList ddlVitalType = (DropDownList)gvVitalView.Rows[e.RowIndex].Cells[4].FindControl("ddlVitalType");

                hshTable.Add("@VitalType", ddlVitalType.SelectedValue);

                if (ddlVitalType.SelectedValue == "T")
                {
                    TextBox txtMax = (TextBox)gvVitalView.Rows[e.RowIndex].Cells[3].FindControl("txtMaxLengthEdit");
                    hshTable.Add("@MaxLength", txtMax.Text); //Request.QueryString["IpNo"];
                }
                else
                {
                    TextBox txtMax = (TextBox)gvVitalView.Rows[e.RowIndex].Cells[3].FindControl("txtMaxLengthEdit");
                    hshTable.Add("@MaxLength", txtMax.Text);
                }

                DropDownList ddlStatus = (DropDownList)gvVitalView.Rows[e.RowIndex].Cells[5].FindControl("ddlStatusEdit");

                DropDownList ddlDisplayInGraph = (DropDownList)gvVitalView.Rows[e.RowIndex].Cells[4].FindControl("ddlDisplayInGraph");
                DropDownList ddlIsMandatory = (DropDownList)gvVitalView.Rows[e.RowIndex].Cells[4].FindControl("ddlIsMandatory");

                hshTable.Add("@bitActive", ddlStatus.SelectedValue); //Request.QueryString["RegNo"];
                hshTable.Add("@chrTransactionType", "U"); //Request.QueryString["IpNo"];


                hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshTable.Add("@intEncodedBy", Session["UserId"]);
                hshTable.Add("@bitDisplayInGraph", Convert.ToBoolean(ddlDisplayInGraph.SelectedValue));
                hshTable.Add("@bitIsMandatory", common.myBool(ddlIsMandatory.SelectedValue));

                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                //hshOutput.Add("chvDocumentNo", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateVitalSigns", hshTable, hshOutput);

                //txtnotenumber.Text = hshOutput["chvDocumentNo"].ToString();
                gvVitalView.EditIndex = -1;
                bindVitalGrid();
                //bindUnitGrid();

                lblMessage.Text = "Record Has Been Updated...";
                //Alert.Show("Data Updated.");
            }
            objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvUnitView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvUnitView.EditIndex = e.NewEditIndex;
        bindUnitGrid();
    }

    protected void gvUnitView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvUnitView.EditIndex = -1;
        bindUnitGrid();
    }

    protected void gvUnitView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Hashtable hshTable = new Hashtable();
            hshTable.Add("@intUnitId", gvUnitView.Rows[e.RowIndex].Cells[0].Text);

            TextBox txtunit = (TextBox)gvUnitView.Rows[e.RowIndex].FindControl("txtUnitNameEdit");

            hshTable.Add("@UnitName", txtunit.Text);

            TextBox txtSymbol = (TextBox)gvUnitView.Rows[e.RowIndex].FindControl("txtSymbolEdit");
            hshTable.Add("@Symbol", txtSymbol.Text); //Request.QueryString["RegNo"];


            TextBox txtConversion = (TextBox)gvUnitView.Rows[e.RowIndex].FindControl("txtConversionFormulaEdit");
            hshTable.Add("@ConversionFormula", txtConversion.Text); //Request.QueryString["IpNo"];

            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            hs.Add("@chvUnitName", Convert.ToString(txtunit.Text));
            hs.Add("@UnitId", gvUnitView.Rows[e.RowIndex].Cells[0].Text);
            SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select UnitName FROM EMRVitalUnitMaster WITH (NOLOCK) WHERE UnitName = @chvUnitName and UnitId <> @UnitId", hs);
            if (objDr.Read())
            {
                lblMessage.Text = "Unit Name Allready Exist!";
                txtVitalName.Text = "";
                txtShortName.Text = "";
            }
            else
            {
                DropDownList ddlConversion = (DropDownList)gvUnitView.Rows[e.RowIndex].FindControl("ddlConvertedUnitEdit");
                if (ddlConversion.SelectedValue != "")
                {
                    hshTable.Add("@ConvertedUnitId", ddlConversion.SelectedValue);
                }
                DropDownList ddlStatusUnit = (DropDownList)gvUnitView.Rows[e.RowIndex].FindControl("ddlUnitStatusEdit");

                hshTable.Add("@bitActive", ddlStatusUnit.SelectedValue); //Request.QueryString["RegNo"];
                hshTable.Add("@chrTransactionType", "U"); //Request.QueryString["IpNo"];


                DropDownList ddlMeasurement = (DropDownList)gvUnitView.Rows[e.RowIndex].FindControl("ddlMeasurementSystemEdit");

                hshTable.Add("@MeasurementSystem", ddlMeasurement.SelectedValue);

                hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshTable.Add("@intEncodedBy", Session["UserId"]);
                //hshTable.Add("xmlVitalDetails", xmlstring);

                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                //hshOutput.Add("chvDocumentNo", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateUnits", hshTable, hshOutput);

                //txtnotenumber.Text = hshOutput["chvDocumentNo"].ToString();
                gvUnitView.EditIndex = -1;
                bindUnitGrid();
                //bindUnitGrid();

                lblMessage.Text = "Record Has Been Updated...";
                //Alert.Show("Data Updated.");
            }
            objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlDropType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDropType.SelectedValue == "T")
        {
            txtMaxLength.Visible = true;
            label1.Visible = true;
        }
        else if (ddlDropType.SelectedValue == "D")
        {
            txtMaxLength.Visible = false;
            label1.Visible = false;
        }
    }

    protected void TabMaster_OnActiveTabChanged(object sender, EventArgs e)
    {
        if ((((TabContainer)sender).ActiveTab.TabIndex = (short)((TabContainer)sender).ActiveTabIndex) != 0)
        {
            ibtnSaveVital.Visible = false;
            ibtnSaveUnit.Visible = true;
        }
        else
        {
            ibtnSaveVital.Visible = true;
            ibtnSaveUnit.Visible = false;
        }
    }

    protected void ibtnSaveVital_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            hs.Add("@chvVitalName", txtVitalName.Text.ToString());
            // hs.Add("@chvDisplayName", txtShortName.Text);

            SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select VitalSignName from EMRVitalSignTypes WITH (NOLOCK) where VitalSignName=@chvVitalName", hs);
            if (objDr.Read())
            {
                lblMessage.Text = "Vital Name Allready Exist!";
                txtVitalName.Text = "";
                txtShortName.Text = "";
                txtMaxLength.Text = "";
            }
            else
            {
                Hashtable hshTable = new Hashtable();

                hshTable.Add("@VitalName", txtVitalName.Text);
                hshTable.Add("@DisplayName", txtShortName.Text);

                if (ddlDropType.SelectedValue == "1")
                {
                    hshTable.Add("@MaxLength", txtMaxLength.Text); //Request.QueryString["IpNo"];
                }

                hshTable.Add("@ResultType", ddlResultType.SelectedValue);
                if (ddlDropType.SelectedValue != "0")
                {
                    hshTable.Add("@VitalType", ddlDropType.SelectedValue);
                }
                else
                {
                    Alert.ShowAjaxMsg("Please select Vital Type", Page);
                    return;
                }
                hshTable.Add("@bitActive", 1); //Request.QueryString["RegNo"];
                hshTable.Add("@chrTransactionType", "N"); //Request.QueryString["IpNo"];

                hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshTable.Add("@intEncodedBy", Session["UserId"]);
                hshTable.Add("@bitDisplayInGraph", Convert.ToBoolean(ddlDisplayInGraph.SelectedValue));
                hshTable.Add("@bitIsMandatory", common.myBool(ddlIsMandatory.SelectedValue));

                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                //hshOutput.Add("chvDocumentNo", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateVitalSigns", hshTable, hshOutput);

                //txtnotenumber.Text = hshOutput["chvDocumentNo"].ToString();

                bindVitalGrid();
                bindUnitGrid();
                lblMessage.Text = "Record(s) Has Been Saved...";
                //Alert.Show("Data Saved.");
                txtVitalName.Text = "";
                txtShortName.Text = "";
                txtMaxLength.Text = "";
                ddlDropType.SelectedIndex = 0;
                ddlResultType.SelectedIndex = 0;
                txtunitname.Text = "";
                txtSymbol.Text = "";
                txtConversionFormula.Text = "";
                ddlMeasurementSystem.SelectedIndex = 0;
                ddlConvertedId.SelectedIndex = 0;
            }
            objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnSaveUnit_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            hs.Add("@chvUnitName", txtunitname.Text.ToString());

            SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select UnitName  from EMRVitalUnitMaster WITH (NOLOCK) WHERE UnitName = @chvUnitName", hs);
            if (objDr.Read())
            {
                lblMessage.Text = "Unit Name Allready Exist!";
                txtVitalName.Text = "";
                txtShortName.Text = "";
            }
            else
            {
                Hashtable hshTable = new Hashtable();

                hshTable.Add("@UnitName", txtunitname.Text);
                hshTable.Add("@Symbol", txtSymbol.Text); //Request.QueryString["RegNo"];
                hshTable.Add("@ConversionFormula", txtConversionFormula.Text); //Request.QueryString["IpNo"];
                if (ddlConvertedId.SelectedValue != "0")
                {
                    hshTable.Add("@ConvertedUnitId", ddlConvertedId.SelectedValue);
                }

                hshTable.Add("@bitActive", 1); //Request.QueryString["RegNo"];
                hshTable.Add("@chrTransactionType", "N"); //Request.QueryString["IpNo"];
                hshTable.Add("@MeasurementSystem", ddlMeasurementSystem.SelectedValue);

                hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshTable.Add("@intEncodedBy", Session["UserId"]);


                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                //hshOutput.Add("chvDocumentNo", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateUnits", hshTable, hshOutput);

                //txtnotenumber.Text = hshOutput["chvDocumentNo"].ToString();

                //bindVitalGrid();
                bindUnitGrid();
                lblMessage.Text = "Record(s) Has Been Saved...";
                //Alert.Show("Data Saved.");
                txtVitalName.Text = "";
                txtShortName.Text = "";
                txtMaxLength.Text = "";
                ddlDropType.SelectedIndex = 0;
                ddlResultType.SelectedIndex = 0;
                txtunitname.Text = "";
                txtSymbol.Text = "";
                txtConversionFormula.Text = "";
                ddlMeasurementSystem.SelectedIndex = 0;
                ddlConvertedId.SelectedIndex = 0;
            }
            objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvUnitView_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;

        //DropDownList ddlConvert = new DropDownList();
        //ddlConvert = (DropDownList)e.Row.Cells[5].Controls[0];// ("ddlConvertedUnitEdit");
        //DataTable dt = new DataTable();
        //dt = (DataTable)Cache["UnitName"];
        //ddlConvert.DataSource = dt;
        //ddlConvert.DataValueField = "UnitId";
        //ddlConvert.DataTextField = "UnitName";
        //ddlConvert.DataBind();
    }

    protected void gvVitalView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[1].Visible = false;
        }
        //imgTemplateHelp.Attributes.Add("onclick", "pos=window.open('/EMR/Vitals/VitalsTemplate.aspx?ControlId=" + litTemplate.ClientID + "',\"mywindow\", \"menubar=0,resizable=0,width=100,height=150,status=0,toolbars=0\"); pos.moveTo(0,110);");
        HtmlImage imgPop = new HtmlImage();
        if (bitEdit != true)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string VitalType = ((Label)e.Row.FindControl("lblVitalType")).Text;
                if (VitalType == "DropDown")
                {
                    ((ImageButton)e.Row.FindControl("imgVital")).Visible = true;

                }
                else
                {
                    ((ImageButton)e.Row.FindControl("imgVital")).Visible = false;
                }
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    imgPop = (HtmlImage)e.Row.Cells[7].FindControl("imgTagUnit");
        //    imgPop.Attributes.Add("onclick", "pos=window.open('/EMR/Vitals/TagVitalUnit.aspx?VitalId=" + e.Row.Cells[0].Text + "',\"mywindow\", \"menubar=0,resizable=0,width=600,height=550,status=0,toolbars=0\"); pos.moveTo(175,110);");
        //}
    }

}
