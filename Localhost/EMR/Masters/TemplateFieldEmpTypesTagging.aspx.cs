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
using System.Text;

public partial class EMR_Masters_TemplateFieldEmpTypesTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != string.Empty)
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            ViewState["FieldId"] = "0";
            ViewState["SectionId"] = common.myInt(Request.QueryString["secid"]);

            bindControl();
        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet(); 
        BaseC.EMROrders objE = new BaseC.EMROrders(sConString);
        try
        {
            ds = objE.getTemplateFields(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["SectionId"]));

            gvTemplateField.DataSource = ds.Tables[0];
            gvTemplateField.DataBind();

            bindEmpType();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objE = null;
        }
    }

    private void bindEmpType()
    {
        DataSet ds = new DataSet();
        BaseC.EMROrders objE = new BaseC.EMROrders(sConString);
        try
        {
            ds = objE.getEMRTemplateFieldEmpTypesTagging(common.myInt(ViewState["FieldId"]));

            gvEmpType.DataSource = ds.Tables[0];
            gvEmpType.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objE = null;
        }
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(ViewState["FieldId"]) == 0)
        {
            strmsg += "Field not Selected !";
            isSave = false;
        }

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        BaseC.EMROrders objE = new BaseC.EMROrders(sConString);

        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            foreach (GridViewRow dataItem in gvEmpType.Rows)
            {
                CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");

                if (common.myBool(chkRow.Checked))
                {
                    HiddenField hdnEmployeeTypeId = (HiddenField)dataItem.FindControl("hdnEmployeeTypeId");

                    coll.Add(common.myInt(hdnEmployeeTypeId.Value));

                    strXML.Append(common.setXmlTable(ref coll));
                }
            }

            string strMsg = objE.SaveEMRTemplateFieldEmpTypesTagging(common.myInt(ViewState["FieldId"]), strXML.ToString(),
                             common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                clearControl();

                bindControl();

                ViewState["FieldId"] = "0";

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }

            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            objE = null;
        }
    }

    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    protected void gvEmpType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //if (e.Row.RowIndex == 0)
            //{
            //    e.Row.Style.Add("height", "20px");
            //}

            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAll('" +
                ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
        }
    }

    protected void gvTemplateField_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnIsChk = (HiddenField)e.Row.FindControl("hdnIsChk");

            System.Drawing.Color color = System.Drawing.Color.White;

            if (common.myBool(hdnIsChk.Value))
            {
                color = System.Drawing.Color.LightGreen;
            }

            e.Row.BackColor = color;
        }
    }

    protected void gvTemplateField_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "FieldSelect")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");

                ViewState["FieldId"] = common.myInt(hdnFieldId.Value);

                bindEmpType();

                foreach (GridViewRow dataItem in gvTemplateField.Rows)
                {
                    HiddenField hdnIsChk = (HiddenField)dataItem.FindControl("hdnIsChk");

                    if (common.myBool(hdnIsChk.Value))
                    {
                        dataItem.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        dataItem.BackColor = System.Drawing.Color.White;
                    }
                }

                row.BackColor = System.Drawing.Color.LightPink;
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