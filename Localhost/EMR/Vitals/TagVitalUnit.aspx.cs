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
            // DataSet ds = dl.FillDataSet(CommandType.Text, "select UnitID, UnitName from EMRVitalUnitMaster Where Active=1");

            Hashtable hsinput = new Hashtable();
            DataSet ds1 = new DataSet();

            hsinput.Add("@inyHospitalLocationId", Session["HospitalLocationID"].ToString());
            ds1 = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalUnits", hsinput);
            //DataView dv = new DataView(ds1.Tables[0]);
            //dv.RowFilter = "HospitalLocationID IS NOT NULL";
            chkunit.DataSource = ds1.Tables[0];
            chkunit.DataValueField = "UnitId";
            chkunit.DataTextField = "UnitName";
            chkunit.DataBind();

            // ds = dl.FillDataSet(CommandType.Text, "select  VitalId, VitalSignName from EMRVitalSignTypes Where Active=1 and HospitalLocationId=" + Session["HospitalLocationID"].ToString());
            Hashtable hsinput1 = new Hashtable();
            DataSet ds = new DataSet();

            hsinput1.Add("@inyHospitalLocationId", 0);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalList", hsinput);
            DataView dv1 = new DataView(ds.Tables[0]);
            //dv1.RowFilter = "HospitalLocationID IS NOT NULL";

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
            hsinput.Add("@intVitalId", ddlvital.SelectedValue.ToString());
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "UspEMRGetVitalTagUnits", hsinput);

            gvUnitView.DataSource = dr;

            gvUnitView.DataBind();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvUnitView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
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
            //@intVitalId smallint,  
            //@xmlUnits XML,
            //@bitActive bit,   
            //@intEncodedBy int,    
            //@chrTransactionType char(1),  
            //@chvErrorStatus varchar(max) output,
            //@intUnitId smallint = 0

            Hashtable hshTable = new Hashtable();

            DropDownList ddlStatusUnit = (DropDownList)gvUnitView.Rows[e.RowIndex].FindControl("ddlUnitStatusEdit");

            hshTable.Add("@bitActive", ddlStatusUnit.SelectedValue);
            hshTable.Add("@intVitalId", ddlvital.SelectedValue);
            hshTable.Add("@xmlUnits", "");
            hshTable.Add("@intUnitId", gvUnitView.Rows[e.RowIndex].Cells[6].Text);

            hshTable.Add("@chrTransactionType", "U");
            hshTable.Add("@intEncodedBy", Session["UserId"]);

            Hashtable hshOutput = new Hashtable();
            hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateVitalUnits", hshTable, hshOutput);

            gvUnitView.EditIndex = -1;
            bindUnitGrid();

            // Alert.Show("Data Updated.");
            txtmsg.ForeColor = System.Drawing.Color.Green;
            txtmsg.Font.Bold.ToString();
            txtmsg.Text = "Data Updated.";
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
            int Flag = 0;
            String xmlstring = "";
            Hashtable hshTable = new Hashtable();

            //@intVitalId smallint,  
            //@xmlUnits XML,
            //@bitActive bit,   
            //@intEncodedBy int,    
            //@chrTransactionType char(1),  
            //@chvErrorStatus varchar(max) output,
            //@intUnitId smallint = 0

            for (int i = 0; i < chkunit.Items.Count; i++)
            {

                if (chkunit.Items[i].Selected == true)
                {
                    Flag = 1;
                    xmlstring = xmlstring + "<Table><c1>" + chkunit.Items[i].Value + "</c1></Table>";
                    //xmlstring = xmlstring + "<c2>" + gvvitalsCancel.Rows[i].Cells[8].Text + "</c2></Table1>";
                    //xmlstring = xmlstring + "<c3>" + gvvitals.SelectedValue + "</c3>";
                    //xmlstring = xmlstring + "<c4>" + gvvitals.Rows[i].Cells[4].Text + "</c4></Table1>";

                }
            }

            if (Flag == 0)
            {
                Alert.ShowAjaxMsg("Select at least one Unit.", this.Page);
                return;
            }

            else
            {
                hshTable.Add("@intVitalId", ddlvital.SelectedValue);
                hshTable.Add("@xmlUnits", xmlstring);

                hshTable.Add("@bitActive", 1);
                hshTable.Add("@chrTransactionType", "N");


                //hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshTable.Add("@intEncodedBy", Session["UserId"]);
                //hshTable.Add("xmlVitalDetails", xmlstring);

                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                //hshOutput.Add("chvDocumentNo", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateVitalUnits", hshTable, hshOutput);

                //txtnotenumber.Text = hshOutput["chvDocumentNo"].ToString();

                //  Alert.Show("Data Saved.");
                txtmsg.ForeColor = System.Drawing.Color.Green;
                txtmsg.Font.Bold.ToString();
                txtmsg.Text = "Data Saved.";

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

    protected void ddlvital_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindUnitGrid();
    }

}
