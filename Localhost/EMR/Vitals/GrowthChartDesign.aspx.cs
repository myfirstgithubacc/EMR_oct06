using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.Globalization;

public partial class MPages_GrowthChartDesign : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    BaseC.ParseData bc = new BaseC.ParseData();
    private Hashtable hshTable = new Hashtable();


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        //this.MasterPageFile = "~/Include/Master/BlankMaster.master";

        Page.MasterPageFile = "/Include/Master/EMRMaster.master";



    }


  


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindGrowthGridandPercentiles();
           
            }

           
           
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void populateSpecialisation()
    {
        try
        {
           
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    public bool Validate()
    {
        bool result;

        if (txtAgeGroup.Text == "" || txtDisplayName.Text == "" || txtGrowthChartName.Text == ""
            || txtXAxisCapInterval.Text == "" || txtXAxisDisplayCap.Text == "" || txtXAxisInterval.Text == "" || txtXAxisMaxValue.Text == "" ||
            txtYAxisCapInterval.Text == "" || txtYAxisCapMinValue.Text == "" || txtYAxisCapMaxValue.Text == "" ||
                txtYAxisCapStartValue.Text == "" || txtYAxisDisplayCap.Text == "" || txtYAxisMaxValue.Text == "" || txtYAxisMinValue.Text == ""

            )
        {


            Alert.ShowAjaxMsg("All Fields are Mandatory", this.Page);
            result = false; 
            

        }
        else
        {
            result = true;

        }

        return result; 

    }

 

  



    protected void btnSaveDetails_Click(object sender, EventArgs e)
    {
        try
        {

            if (Validate()== true)
            {
                UpdateSaveEMRGrowthChartDetail();
                BindGrowthGridandPercentiles();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

  

    protected void clearPage()
    {
        lblMessage.Visible = false;
       
    }




    protected void GrdGrowthChart_RowEditing(object sender, GridViewEditEventArgs e)
    {
        
    }


    protected void GrdGrowthChart_OnRowCommand(object sender,GridViewCommandEventArgs  e) 
    {

        if (e.CommandName == "Edit")
        {


            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer); 
            HiddenField hdnGrowthChartId = (HiddenField)grdrow.FindControl("hdnGrowthChartId");
            DataSet ds = (DataSet)ViewState["objds"];
            ViewState["GrowthChartId"] = hdnGrowthChartId.Value;
            hdGrowthID.Value= hdnGrowthChartId.Value;
            DataTable dt = ds.Tables[0];


            DataView dv = new DataView(ds.Tables[0]);
            
            dv.RowFilter = "GrowthChartID='" + hdnGrowthChartId.Value + "'";
            dt = dv.ToTable();



            
            txtAgeGroup.Text = dt.Rows[0]["AgeGroup"].ToString();
            txtGrowthChartName.Text = dt.Rows[0]["GrowthChartName"].ToString();
            txtDisplayName.Text = dt.Rows[0]["DisplayName"].ToString();
            ddlVitalGrowthChart.SelectedValue = dt.Rows[0]["EMRVitalID"].ToString();
            txtXAxisCapInterval.Text = dt.Rows[0]["XAxisCapInterval"].ToString();
            txtXAxisDisplayCap.Text = dt.Rows[0]["XAxisDisplayCap"].ToString();
            txtXAxisMinValue.Text = dt.Rows[0]["XAxisMinValue"].ToString();
            txtXAxisMaxValue.Text = dt.Rows[0]["XAxisMaxValue"].ToString();
            txtXAxisInterval.Text = dt.Rows[0]["XAxisInterval"].ToString();
            txtYAxisDisplayCap.Text = dt.Rows[0]["YAxisDisplayCap"].ToString();
            txtYAxisCapMinValue.Text = dt.Rows[0]["YAxisCapMinValue"].ToString();
            txtYAxisCapMaxValue.Text = dt.Rows[0]["YAxisCapMaxValue"].ToString();
            txtYAxisCapInterval.Text = dt.Rows[0]["YAxisCapInterval"].ToString();
            txtYAxisCapStartValue.Text = dt.Rows[0]["YAxisCapStartValue"].ToString();
            txtYAxisMinValue.Text = dt.Rows[0]["YAxisMinValue"].ToString();
            txtYAxisMaxValue.Text = dt.Rows[0]["YAxisMaxValue"].ToString();
            txtYAxisInterval.Text = dt.Rows[0]["YAxisInterval"].ToString();
            //        AgeGroup,GrowthChartName,Active,DisplayName,EMRVitalID,
            //XAxisCapInterval,XAxisDisplayCap,
            //        XAxisMinValue,XAxisMaxValue,XAxisInterval
            //        ,YAxisDisplayCap,YAxisCapMinValue,YAxisCapMaxValue,YAxisCapInterval,YAxisCapStartValue
            //        ,YAxisMinValue,YAxisMaxValue,YAxisInterval  

            dv = new DataView(ds.Tables[1]);
            DataTable objdtPercen = dv.ToTable();
            dv.RowFilter = "GrowthChartID='" + hdnGrowthChartId.Value + "' and Active=1";
            objdtPercen = dv.ToTable(false, "PercetileCurveName");


            foreach (ListItem chk in chkPercentiles.Items)
            //for (int i = 0; i < objdtPercen.Rows.Count; i++)
            {

                for (int i = 0; i < objdtPercen.Rows.Count; i++)
               // foreach (ListItem chk in chkPercentiles.Items)
                {

                    if (common.myStr(objdtPercen.Rows[i]["PercetileCurveName"]) == chk.Text)
                    {
                        chk.Selected = true;
                        break; 
                    }
                    else
                    {
                        chk.Selected = false;
                    }

                }


            }


        }
 
  
    }

    public void UpdateSaveEMRGrowthChartDetail()
    {
        StringBuilder sbPercential = new StringBuilder();
        
        foreach (ListItem chk in chkPercentiles.Items)
        {
            sbPercential.Append("<Table1>");
            sbPercential.Append("<c1>" + chk.Text + "</c1>");
            if (chk.Selected  == true)
            {
                sbPercential.Append("<c2>1</c2>");
            }
            else
            {
                sbPercential.Append("<c2>0</c2>");
            }
            sbPercential.Append("</Table1>");

        }
      
        

        lblMessage.Text = updateEMRGrowthChartDetail(common.myInt(hdGrowthID.Value)   ,common.myInt(Session["FacilityId"]),txtAgeGroup.Text,txtGrowthChartName.Text,1,txtDisplayName.Text,common.myInt(ddlVitalGrowthChart.SelectedValue)
            ,txtXAxisCapInterval.Text,txtXAxisDisplayCap.Text,txtXAxisMinValue.Text,txtXAxisMaxValue.Text,txtXAxisInterval.Text,txtYAxisDisplayCap.Text,txtYAxisCapMinValue.Text ,txtYAxisCapMaxValue.Text
             ,txtYAxisCapInterval.Text,txtYAxisCapStartValue.Text ,txtYAxisMinValue.Text,txtYAxisMaxValue.Text,txtYAxisInterval.Text,sbPercential.ToString());




   




    }









    public string  updateEMRGrowthChartDetail(int intGrowthChartId, int Facilityid, string AgeGroup, string GrowthChartName, int Active, string  DisplayName,int EMRVitalID
        ,string XAxisCapInterval,string XAxisDisplayCap,string XAxisMinValue,string XAxisMaxValue,string XAxisInterval,
            string YAxisDisplayCap,string YAxisCapMinValue,string YAxisCapMaxValue,string YAxisCapInterval,string YAxisCapStartValue,
                string YAxisMinValue, string YAxisMaxValue, string YAxisInterval, string XmlPercentile)
    {

        string  result="";


        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hsinput = new Hashtable();
        Hashtable HshOut = new Hashtable(); 
        hsinput.Add("@intGrowthChartId", intGrowthChartId);
        hsinput.Add("@intFacilityId", Facilityid);
        hsinput.Add("@AgeGroup", AgeGroup);
        hsinput.Add("@GrowthChartName", GrowthChartName);
        hsinput.Add("@Active", Active);
        hsinput.Add("@DisplayName", DisplayName);
        hsinput.Add("@EMRVitalID", EMRVitalID);
        hsinput.Add("@XAxisCapInterval", XAxisCapInterval);
        hsinput.Add("@XAxisDisplayCap", XAxisDisplayCap);
        hsinput.Add("@XAxisMinValue", XAxisMinValue);
        hsinput.Add("@XAxisMaxValue", XAxisMaxValue);
        hsinput.Add("@XAxisInterval", XAxisInterval);
        hsinput.Add("@YAxisDisplayCap", YAxisDisplayCap);
        hsinput.Add("@YAxisCapMinValue", YAxisCapMinValue);
        hsinput.Add("@YAxisCapMaxValue", YAxisCapMaxValue);
        hsinput.Add("@YAxisCapInterval", YAxisCapInterval);
        hsinput.Add("@YAxisCapStartValue", YAxisCapStartValue);
        hsinput.Add("@YAxisMinValue", YAxisMinValue);
        hsinput.Add("@YAxisMaxValue", YAxisMaxValue);
        hsinput.Add("@YAxisInterval", YAxisInterval);
        hsinput.Add("@XmlPercentile", XmlPercentile);
        HshOut.Add("@chrmessage",SqlDbType.VarChar);  
        //
        //hsouput = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateEMRGrowthChartMaster", hsinput);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateEMRGrowthChartMaster", hsinput, HshOut);

        return HshOut["@chrmessage"].ToString();
       // result = hsouput["@chrmessage"].ToString(); 
        //result = "Save";
        //return result; 
        
    }





    public DataSet getEMRGrowthChartDetail(int FacilityID,int HospitalLocationid)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet(); 
        Hashtable hsinput = new Hashtable();
        
        hsinput.Add("@intFacilityId",FacilityID);
        hsinput.Add("@intHospitalLocationId", HospitalLocationid);
        //@intHospitalLocationId
       

        ds = dl.FillDataSet(CommandType.StoredProcedure, "uspgetEmrGrowthChartDetail", hsinput);

        return ds;



    }



    public void BindGrowthGridandPercentiles()

    {
        DataSet objds = new DataSet();
        objds = getEMRGrowthChartDetail(common.myInt(Session["FacilityID"]), common.myInt(Session["HospitalLocationId"]));
        GrdGrowthChart.DataSource = objds.Tables[0];
        GrdGrowthChart.DataBind();


        chkPercentiles.DataSource = objds.Tables[2];
        chkPercentiles.DataValueField = "COLUMN_NAME";
        chkPercentiles.DataTextField = "COLUMN_NAME";
        chkPercentiles.DataBind();


        ddlVitalGrowthChart.DataSource = objds.Tables[3];
        ddlVitalGrowthChart.DataValueField = "VitalId";
        ddlVitalGrowthChart.DataTextField = "VitalSignName";
        ddlVitalGrowthChart.DataBind();  

        ViewState["objds"] = objds; 


 

       
    }

 
 
  

}
