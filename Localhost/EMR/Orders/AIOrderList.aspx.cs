using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

using System.Data;
using System.Text;
using System.Collections;

public partial class EMR_Orders_AIOrderList : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //clsReport objReport;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                GetTopOrderListBasedOnICDCodes("Both");
            }
        }
        catch (Exception Ex)
        {
            //clsExceptionLog objException = new clsExceptionLog();
        }
    }


   

    private void GetTopOrderListBasedOnICDCodes(string GridtoBind)
    {
        int EncounterId = common.myInt(Request.QueryString["EncID"]);
        DataSet ds = new DataSet();
        clsReport objReport = new clsReport();
        ds = objReport.GetTopOrderListBasedOnICDCodes(EncounterId);

        if (GridtoBind == "P")
        {
            gvDrug.DataSource = ds.Tables[0];
            gvDrug.DataBind();
        }
        else if (GridtoBind == "S")
        {
            gvDrugSeconday.DataSource = ds.Tables[1];
            gvDrugSeconday.DataBind();
        }
        else
        {
            gvDrug.DataSource = ds.Tables[0];
            gvDrug.DataBind();

            gvDrugSeconday.DataSource = ds.Tables[1];
            gvDrugSeconday.DataBind();
        }
    }

    protected void gvDrug_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
           

        }
    }

    //protected void gvDrug_PreRender(object sender, EventArgs e)
    //{
    //    GetTopPrescriptionListBasedOnICDCodes("P");
    //}

    protected void gvDrugSeconday_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
        }
    }

   

    protected void btnAdtoList_Click(object sender, EventArgs e)
    {
        //DataTable dt = CreateItemTable();
        int Sno = 0;
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        foreach (GridDataItem item in gvDrug.Items)
        {
            TableCell cell = item["chkCollection"];
            CheckBox CHK = (CheckBox)cell.Controls[0];
            if (CHK.Checked == true && CHK.Visible == true)
            {
                Sno++;
                int ServiceId = common.myInt(item["ServiceId"].Text);
                //string ServiceName = common.myStr(item["ServiceName"].Text);
               // TextBox txtQty = (TextBox)item.FindControl("txtQty");
                //if (common.myInt(txtQty.Text) == 0)
                //    txtQty.Text = "1";

                coll.Add(common.myInt(Sno));
                coll.Add(ServiceId);             
            }           
        }

        foreach (GridDataItem item in gvDrugSeconday.Items)
        {
            TableCell cell = item["chkCollection"];
            CheckBox CHK = (CheckBox)cell.Controls[0];
            if (CHK.Checked == true && CHK.Visible == true)
            {
                Sno++;
                int ServiceId = common.myInt(item["ServiceId"].Text);
               // string ServiceName = common.myStr(item["ServiceName"].Text);
                //TextBox txtQty = (TextBox)item.FindControl("txtQty");
                //if (common.myInt( txtQty.Text) ==0)
                //    txtQty.Text = "1";
                coll.Add(common.myInt(Sno));
                coll.Add(ServiceId);
            }
        }
        strXML.Append(common.setXmlTable(ref coll));

        //DataSet ds = new DataSet();
        //ds.Tables.Add(dt);
        Session["AIOrderList"] = strXML.ToString();
       
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }

   

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Session["AIOrderList"] = null;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }

    private DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("ServiceId", typeof(int)); //6    
        dt.Columns.Add("ServiceName", typeof(string)); //7      
        dt.Columns.Add("Qty", typeof(decimal)); //16           
        dt.Columns.Add("Remarks", typeof(string));

        return dt;
    }
}