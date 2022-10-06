using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Drawing;
using System.Configuration;

public partial class LIS_Format_ServiceProfile : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    
    protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            bindBlankSelectedFields();
        }
    }

    private void GetServiceDetail()
    {
        try
        {
            int ServiceID = common.myInt(Request.QueryString["ServiceID"]);
            BaseC.clsServiceTaging objGetServiceTag = new BaseC.clsServiceTaging(sConString);
            DataSet objDs = objGetServiceTag.GetServiceDetail(ServiceID);
            if (objDs != null)
            {
                ServiceName.Text = common.myStr(objDs.Tables[0].Rows[0]["Servicename"]);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


    private void bindBlankSelectedFields()
    {
        try
        {
            if (common.myInt(Request.QueryString["ServiceID"]) > 0)
            {
                GetServiceDetail();

                int ServiceID = common.myInt(Request.QueryString["ServiceID"]);
                int iAge= common.myInt(Request.QueryString["Age"]);
                string sGender= Request.QueryString["Gender"].ToString();
                string sAgeType = Request.QueryString["AgeType"].ToString();
                BaseC.clsServiceTaging objGetServiceTag = new BaseC.clsServiceTaging(sConString);

                DataSet objDs = objGetServiceTag.GetServiceTagingWithRange(ServiceID, iAge, sGender, sAgeType,common.myInt(Session["FacilityId"]));

                if (objDs != null)
                {
                    objDs.Tables[0].Columns.Add("AssociatedFieldID");
                    gvSelectedFields.DataSource = objDs;
                    gvSelectedFields.DataBind();
                    ViewState["objDt"] = objDs.Tables[0];
                }
                else
                {
                    DataTable objDt = CreateTable();
                    DataRow objDr = objDt.NewRow();
                    objDr["FieldID"] = "";
                    objDr["FieldName"] = "&nbsp;";
                    objDr["SequenceNo"] = "1";
                    objDr["FieldType"] = "F";
                    objDr["HeaderStartSNo"] = 0;
                    objDr["HeaderEndSNo"] = 0;
                    objDt.Rows.Add(objDr);

                    gvSelectedFields.DataSource = objDt;
                    gvSelectedFields.DataBind();
                    ViewState["objDt"] = objDt;
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

    private DataTable CreateTable()
    {
        DataTable objDt = new DataTable();
        objDt.Columns.Add("ID");
        objDt.Columns.Add("FieldID", typeof(int));
        objDt.Columns.Add("AssociatedFieldID", typeof(int));
        objDt.Columns.Add("FieldName");
        objDt.Columns.Add("SequenceNo");
        objDt.Columns.Add("FieldType");
        objDt.Columns.Add("HeaderStartSNo");
        objDt.Columns.Add("HeaderEndSNo");
        objDt.Columns["ID"].AutoIncrement = true;

        objDt.Columns["ID"].AutoIncrementSeed = 1;
        objDt.Columns["ID"].AutoIncrementStep = 1;
        return objDt;
    }

    protected void cmdNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

   

    protected void gvSelectedFields_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            HiddenField hdnFieldID = (HiddenField)e.Item.FindControl("hdnFieldID");
            HiddenField hdnFieldType = (HiddenField)e.Item.FindControl("hdnFieldType");
            if (hdnFieldType.Value == "H")
            {
                e.Item.BackColor = Color.LightGoldenrodYellow;
            }
        }
    }

}
