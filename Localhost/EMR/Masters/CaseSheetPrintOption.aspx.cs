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
using System.Text;

public partial class EMR_Masters_CaseSheetPrintOption : System.Web.UI.Page
{
    string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    Hashtable hstInput = new Hashtable();
    DL_Funs fun = new DL_Funs();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindTemplate();
            btnUpdate.Visible = false;
            ddlPrintGroup.Visible = false;
            btnGetDetails.Visible = false;
        }
    }

    private void bindTemplate()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("inyModuleID", 3);

            hshInput.Add("intGroupId", Convert.ToInt32(Session["GroupID"]));

            hshInput.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));

            DataSet objDs = new DataSet();
            objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModuleUserPages", hshInput);
            foreach (DataRow item in objDs.Tables[0].Rows)
            {
                if (item["PageId"].ToString() == "6" || item["PageId"].ToString() == "130" || item["PageId"].ToString() == "131")
                {
                    item.Delete();
                }
            }
            objDs.Tables[0].AcceptChanges();

            lstMasterTemplate.DataSource = objDs;
            lstMasterTemplate.DataTextField = "PageName";
            lstMasterTemplate.DataValueField = "PageIdentification";
            lstMasterTemplate.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnShift_OnClick(object sender, EventArgs e)
    {
        try
        {
            int temp = 0;
            for (int i = 0; i < lstMasterTemplate.Items.Count; i++)
            {
                if (lstMasterTemplate.Items[i].Selected)
                {
                    temp = 1;
                    int exist = 0;
                    foreach (ListItem item in lstTemplate.Items)
                    {
                        if (lstMasterTemplate.Items[i].Value == item.Value)
                        {
                            exist = 1;
                            break;
                        }
                    }
                    if (exist == 0)
                        lstTemplate.Items.Add(new ListItem(lstMasterTemplate.Items[i].Text, lstMasterTemplate.Items[i].Value, true));
                }
            }
            if (temp == 0)
                Alert.ShowAjaxMsg("Please select atleast one from Template(s) List", Page);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkRemove_OnClick(object sender, EventArgs e)
    {
        try
        {
            int temp = 0;
            for (int i = 0; i < lstTemplate.Items.Count; i++)
            {
                if (lstTemplate.Items[i].Selected)
                {
                    temp = 1;
                    lstTemplate.Items.Remove(lstTemplate.Items[i]);
                    i--;
                }
            }
            if (temp == 0)
                Alert.ShowAjaxMsg("Please select atleast one from Assigned Template(s)", Page);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder objSb = new StringBuilder();
            BaseC.ParseData parse = new BaseC.ParseData();
            foreach (ListItem item in lstTemplate.Items)
            {
                objSb.Append("<Table1>");
                if (item.Value.Substring(0, 1) == "T")
                {
                    objSb.Append("<c1>");
                    objSb.Append(item.Value.Substring(1, item.Value.Length - 1));
                    objSb.Append("</c1>");
                    objSb.Append("<c2>");
                    objSb.Append("0");
                    objSb.Append("</c2>");
                }
                else if (item.Value.Substring(0, 1) == "P")
                {
                    objSb.Append("<c1>");
                    objSb.Append("0");
                    objSb.Append("</c1>");
                    objSb.Append("<c2>");
                    objSb.Append(item.Value.Substring(1, item.Value.Length - 1));
                    objSb.Append("</c2>");
                }
                objSb.Append("</Table1>");
            }
            if (objSb.ToString().Trim() != "")
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput = new Hashtable();
                hstInput.Add("inyHospitalLocationId", Session["HospitalLocationId"]);
                hstInput.Add("chvPrintGroup", parse.ParseQ(txtPrintGroup.Text));
                hstInput.Add("xmlGroupDetails", objSb.ToString());
                hstInput.Add("intCaseSheetOptionId", 0);
                Hashtable hstOutput = new Hashtable();
                hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSavePrintGroup", hstInput, hstOutput);
                Alert.ShowAjaxMsg("Record Saved!", Page);
            }
            else
                Alert.ShowAjaxMsg("Select atleast one template", Page);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder objSb = new StringBuilder();
            BaseC.ParseData parse = new BaseC.ParseData();
            foreach (ListItem item in lstTemplate.Items)
            {
                objSb.Append("<Table1>");
                if (item.Value.Substring(0, 1) == "T")
                {
                    objSb.Append("<c1>");
                    objSb.Append(item.Value.Substring(1, item.Value.Length - 1));
                    objSb.Append("</c1>");
                    objSb.Append("<c2>");
                    objSb.Append("0");
                    objSb.Append("</c2>");
                }
                else if (item.Value.Substring(0, 1) == "P")
                {
                    objSb.Append("<c1>");
                    objSb.Append("0");
                    objSb.Append("</c1>");
                    objSb.Append("<c2>");
                    objSb.Append(item.Value.Substring(1, item.Value.Length - 1));
                    objSb.Append("</c2>");
                }
                objSb.Append("</Table1>");
            }
            if (objSb.ToString().Trim() != "")
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput = new Hashtable();
                hstInput.Add("inyHospitalLocationId", Session["HospitalLocationId"]);
                hstInput.Add("chvPrintGroup", parse.ParseQ(ddlPrintGroup.SelectedItem.Text));
                hstInput.Add("xmlGroupDetails", objSb.ToString());
                hstInput.Add("intCaseSheetOptionId", ddlPrintGroup.SelectedValue);
                Hashtable hstOutput = new Hashtable();
                hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSavePrintGroup", hstInput, hstOutput);
                Alert.ShowAjaxMsg("Record Updated!", Page);
            }
            else
                Alert.ShowAjaxMsg("Select atleast one template", Page);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder objSb = new StringBuilder();
            hstInput = new Hashtable();
            hstInput.Add("Id", ddlPrintGroup.SelectedValue);
            objSb.Append("SELECT Case when TemplateId <> 0 then ('T' + Convert(varchar(12), cspd.TemplateId)) when (PageId <> 0) then ('P' + Convert(varchar(12), cspd.PageId)) end as PageId, ");
            objSb.Append(" Case when TemplateId <> 0 then (Select templateName from EMRTemplate t where t.Id = cspd.TemplateId) when (pageid <> 0) ");
            objSb.Append(" then (select PageName from SecModulePages p where p.PageId = cspd.PageID) end as PageName ");
            objSb.Append(" FROM EMRCaseSheetPrintOption csp INNER JOIN EMRCaseSheetPrintOptionDetails cspd ON csp.Id = cspd.CaseSheetOptionId Where csp.Id = @Id And Active = 1");
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet objDs = objDl.FillDataSet(CommandType.Text, objSb.ToString(), hstInput);
            lstTemplate.DataSource = objDs;
            lstTemplate.DataTextField = "PageName";
            lstTemplate.DataValueField = "PageId";
            lstTemplate.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        btnUpdate.Visible = true;
        btnEdit.Visible = false;
        btnSave.Visible = false;
        btnGetDetails.Visible = true;
        txtPrintGroup.Visible = false;
        ddlPrintGroup.Visible = true;
        bindGroupOption();
    }

    private void bindGroupOption()
    {
        try
        {
            StringBuilder objSb = new StringBuilder();
            objSb.Append("SELECT distinct Id, SetupName From EMRCaseSheetPrintOption ");

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet objDs = objDl.FillDataSet(CommandType.Text, objSb.ToString(), hstInput);
            ddlPrintGroup.DataSource = objDs;
            ddlPrintGroup.DataTextField = "SetupName";
            ddlPrintGroup.DataValueField = "Id";
            ddlPrintGroup.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("/EMR/Masters/CaseSheetPrintOption.aspx", false);
    }
}