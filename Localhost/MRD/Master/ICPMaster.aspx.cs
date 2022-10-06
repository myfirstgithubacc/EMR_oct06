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
using Telerik.Web.UI;

public partial class MRD_ICPMaster : System.Web.UI.Page
{
    private static  string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds;


  //  wcf_Service_Common.CommonMasterClient objCM = new wcf_Service_Common.CommonMasterClient();
    //wcf_Service_MRD.MRDServiceClient objMRD = new wcf_Service_MRD.MRDServiceClient();

    BaseC.RestFulAPI objMRD = new BaseC.RestFulAPI(sConString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindProcedurGrid();
         
        }
    }
    protected void ibtnNew_Click(object sender, EventArgs e)
    {
        Clear();
        lblMassage.Text = "";
       // BindProcedurGrid();
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string strsave = "";

        strsave=objMRD.SaveMrdProcedure(common.myInt(hdndId.Value),common.myStr(txtCode.Text),
                        common.myStr(txtName.Text),common.myBool(ddlStatus.SelectedValue),sConString);

        if (strsave.Contains("Save Data..") || strsave.Contains("Update Data.."))
        {
            lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMassage.Text = strsave.ToString();
        }
        else
        {
            lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMassage.Text = strsave.ToString();
        }
        Clear();
        BindProcedurGrid();
    }

    void BindBlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Code");
        dt.Columns.Add("Name");
        dt.Columns.Add("Status");
     


        for (int i = 0; i < 1; i++)
        {
            DataRow dr = dt.NewRow();
          
            dr["Code"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["Status"] = DBNull.Value;
           
            dt.Rows.Add(dr);

        }


        gvProcedure.DataSource = dt;
        gvProcedure.DataBind();


    }

    void BindProcedurGrid()
    {
        ds = new DataSet();
        ds = objMRD.GetICPDisease(sConString);
        if (ds.Tables.Count > 0)
        {
            gvProcedure.DataSource = ds;
            gvProcedure.DataBind();
        }
        else
        {
            BindBlankGrid();
        }
    }

    protected void gvProcedure_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                ddlStatus.Enabled = true;

                Label lblId = (Label)e.Item.FindControl("lblId");
                Label lblCode = (Label)e.Item.FindControl("lblCode");
                Label lblName = (Label)e.Item.FindControl("lblName");
                Label lblStatus = (Label)e.Item.FindControl("lblStatus");
              
                if (lblId.Text != "")
                {
                    hdndId.Value = lblId.Text;
                }
                if (lblCode.Text != "")
                {
                    txtCode.Text = lblCode.Text.Trim();
                    txtName.Text = lblName.Text.Trim();
                    ddlStatus.SelectedValue = lblStatus.Text.Trim();
                  
                }
                btnsave.Text = "Update";
            }
        }
        catch (Exception Ex)
        {
            lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMassage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvProcedure_PreRender(object sender, EventArgs e)
    {
        try
        {
            BindProcedurGrid();
           
        }
        catch (Exception Ex)
        {
            lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMassage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void Clear()
    {
        hdndId.Value = "";
        txtCode.Text = "";
        txtName.Text = "";
        ddlStatus.SelectedIndex =0;
    }
}
