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
using Telerik.Web.UI;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class EMR_Vitals_TagVitalUnit : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
    BaseC.ParseData bcText = new BaseC.ParseData();

    protected void Page_Load(object sender, EventArgs e)
    {
        String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        DAL.DAL dl = new DAL.DAL();

        if (!IsPostBack)
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsinput1 = new Hashtable();
            DataSet ds = new DataSet();

            hsinput1.Add("@inyHospitalLocationId", 0);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalList", hsinput1);
            DataView dv1 = new DataView(ds.Tables[0]);

            ddlvital.DataSource = dv1;
            ddlvital.DataValueField = "VitalId";
            ddlvital.DataTextField = "VitalSignName";
            ddlvital.DataBind();

            ddlvital.SelectedValue = Request.QueryString["VitalId"].ToString();
            bindUnitGrid();
        }
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    void bindUnitGrid()
    {
        try
        {
            //  sqlstr = "select Row_number() over(order by VitalId) as Slno,  VSU.VitalId, VSU.UnitID ,UM.UnitName, UM.Symbol, Case When UM.MeasurementSystem='E' Then 'English' else 'Metric' end as MeasurementSystem, VSU.Active,Case when VSU.Active=1 then 'Active' else 'In Active' End as Status from EMRVitalSignUnits VSU  inner join EMRVitalUnitMaster UM on UM.UnitID=VSU.UnitId and VSU.VitalId=" + ddlvital.SelectedValue + " and UM.HospitalLocationId=" + Session["HospitalLocationID"];
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hsinput = new Hashtable();
            DataSet ds1 = new DataSet();

            hsinput.Add("@intVitalId", ddlvital.SelectedValue.ToString());
            ds1 = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalTagUnits", hsinput);
            ddUnit.Text = String.Empty;

            ddUnit.DataSource = ds1.Tables[0];
            ddUnit.DataValueField = "UnitId";
            ddUnit.DataTextField = "UnitName";
            ddUnit.DataBind();
           

            Hashtable hsinput1 = new Hashtable();
            DataSet ds = new DataSet();

            hsinput1.Add("@inyHospitalLocationId", 0);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalList", hsinput1);
            
            DataRow[] drVitalType = ds.Tables[0].Select("VitalId=" + ddlvital.SelectedValue + "");

            if (drVitalType.Length > 0)
            {
                lblVitalType.Text = drVitalType[0]["ResultType"].ToString();
            }

            Hashtable hsinput2 = new Hashtable();
            hsinput2.Add("@intVitalId", ddlvital.SelectedValue.ToString());
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "UspEMRGetVitalTagRanges", hsinput2);

            gvDetail.DataSource = dr;

            gvDetail.DataBind();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ibtnSaveVital_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable hshTable = new Hashtable();

            if (ddUnit.SelectedValue == "0")
            {
                Alert.ShowAjaxMsg("Select at least one Unit.", this.Page);
                return;
            }
                 
            else
            {
               
                hshTable.Add("@intUnitId", ddUnit.SelectedValue);
                hshTable.Add("@chrSex", ddGender.SelectedValue);
                hshTable.Add("@intAgeFrom", txtAgeFrom.Text);

                hshTable.Add("@intAgeTo", txtAgeTo.Text);
                hshTable.Add("@chrAgeFromType", ddAgeTypeFrom.SelectedValue);
                hshTable.Add("@chrAgeToType", ddAgeTypeTo.SelectedValue);

                hshTable.Add("@numMinValue", txtMinValue.Text);
                hshTable.Add("@numMaxValue", txtMaxValue.Text);

                hshTable.Add("@numMinPanicValue", txtMinPanicValue.Text);

                hshTable.Add("@numMaxPanicValue", txtMaxPanicValue.Text);

                if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true)
                {
                    hshTable.Add("@chrTransactionType", "E");
                    hshTable.Add("@intId", hdnVitalRangeId.Value);
                    hshTable.Add("@intVitalId", hdnVitalId.Value);
                }
                else
                {
                    hshTable.Add("@chrTransactionType", "N");
                    hshTable.Add("@bitActive", 1);
                    hshTable.Add("@intVitalId", common.myInt(ddlvital.SelectedValue));
                }
                hshTable.Add("@intEncodedBy", Session["UserId"]);
                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateVitalRanges", hshTable, hshOutput);
                txtmsg.ForeColor = System.Drawing.Color.Green;
                txtmsg.Text = hshOutput["@chvErrorStatus"].ToString();
                ViewState["Edit"] = false;
                bindUnitGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

  
    protected void btnclose_Click(object sender, EventArgs e)
    {

    }
    protected void ddlvital_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        bindUnitGrid();
    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[11].Visible = false;
        e.Row.Cells[12].Visible = false;
        e.Row.Cells[13].Visible = false;
        e.Row.Cells[14].Visible = false;
        e.Row.Cells[15].Visible = false;
    }

    protected void gvDetail_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow row = this.gvDetail.SelectedRow;
        hdnVitalId.Value = row.Cells[0].Text;
        
        txtAgeFrom.Text = row.Cells[3].Text;
        txtAgeTo.Text = row.Cells[4].Text;
        txtMinValue.Text = row.Cells[5].Text;

        txtMaxValue.Text = row.Cells[6].Text;
        txtMinPanicValue.Text = row.Cells[7].Text;
        txtMaxPanicValue.Text = row.Cells[8].Text;
        ddUnit.SelectedValue = row.Cells[11].Text;
        ddAgeTypeFrom.SelectedValue = row.Cells[12].Text;
        ddAgeTypeTo.SelectedValue = row.Cells[13].Text;
        hdnVitalRangeId.Value = row.Cells[14].Text;
        ddGender.SelectedValue = row.Cells[15].Text;
        ViewState["Edit"] = true;
    }
}
