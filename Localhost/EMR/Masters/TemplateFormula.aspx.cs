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

public partial class EMR_Masters_TemplateFormula : System.Web.UI.Page
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

            bindBlankSelectedFields();
            bindFields();
        }
    }

    private void bindFields()
    {
        DataSet ds = new DataSet();
        BaseC.EMROrders objE = new BaseC.EMROrders(sConString);
        try
        {
            ds = objE.getTemplateFields(common.myInt(Session["HospitalLocationId"]), common.myInt(ViewState["SectionId"]));

            DataView DV = ds.Tables[0].DefaultView;
            DV.RowFilter = "FieldType='T'";

            gvTemplateField.DataSource = DV.ToTable();
            gvTemplateField.DataBind();
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




    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    protected void gvSelectedFields_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnFieldId = (HiddenField)e.Row.FindControl("hdnFieldId");
            ImageButton imgBtn = (ImageButton)e.Row.FindControl("imgDelete");
            TextBox txtFormulaDefinition = (TextBox)e.Row.FindControl("txtFormulaDefinition");
            CheckBox chkTotalCalc = (CheckBox)e.Row.FindControl("chkTotalCalc");

            if (common.myInt(hdnFieldId.Value) == 0)
            {
                txtFormulaDefinition.Visible = false;
                chkTotalCalc.Visible = false;
                imgBtn.Visible = false;
            }
        }
    }

    protected void gvTemplateField_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //HiddenField hdnIsChk = (HiddenField)e.Row.FindControl("hdnIsChk");

            //System.Drawing.Color color = System.Drawing.Color.White;

            //if (common.myBool(hdnIsChk.Value))
            //{
            //    color = System.Drawing.Color.LightGreen;
            //}

            //e.Row.BackColor = color;
        }
    }

    protected void gvTemplateField_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddToList")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                lblMessage.Text = "";
                int iCount = 0;
                string sReference = "";

                Label lblFieldName = (Label)row.FindControl("lblFieldName");
                HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
                ViewState["FieldId"] = common.myInt(hdnFieldId.Value);

                int isCrosMaxLimit = 0;
                bool isAlredyExists = false;
                foreach (GridViewRow item in gvSelectedFields.Rows)
                {
                    HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
                    if (hdnFieldId.Value.Trim() == hdnFieldIdGrid.Value.Trim())
                    {
                        isAlredyExists = true;
                    }
                    if (hdnFieldIdGrid.Value.Trim() != "")
                    {
                        isCrosMaxLimit++;
                    }
                }

                string strMsg = "";
                if (isAlredyExists)
                {
                    strMsg += "Field Name '" + lblFieldName.Text + "' Already Exists !<br />";
                }

                //if (isCrosMaxLimit > 4)
                //{
                //    strMsg += "Maximum Field Name Limit are Five !";
                //    //return;
                //}

                if (strMsg != "")
                {
                    lblMessage.Text = strMsg;
                    return;
                }

                DataTable objDt = new DataTable();
                DataRow objDr;
                objDt = ((DataTable)ViewState["SelectedFieldsClone"]).Clone();
                if (objDt == null)
                {
                    return;
                }

                foreach (GridViewRow item in gvSelectedFields.Rows)
                {
                    Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
                    HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
                    Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
                    TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
                    CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

                    if (hdnFieldId.Value.Trim() != hdnFieldIdGrid.Value.Trim())
                    {
                        objDr = objDt.NewRow();
                        if (hdnFieldIdGrid.Value.Trim() != "")
                        {
                            sReference = lblReferenceName.Text;
                            objDr["FieldId"] = hdnFieldIdGrid.Value;
                            objDr["FieldName"] = lblFieldNameGrid.Text;
                            objDr["FormulaDefinition"] = txtFormulaDefinition.Text;
                            objDr["TotalCalc"] = chkTotalCalc.Checked ? 1 : 0;

                            if (iCount == 0)
                            {
                                objDr["ReferenceName"] = "A";
                            }
                            else
                            {
                                Label lblReferencePrevious = (Label)gvSelectedFields.Rows[iCount - 1].FindControl("lblReferenceName");
                                objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(lblReferencePrevious.Text)) + 1);
                            }
                            objDt.Rows.Add(objDr);

                            iCount++;
                        }
                        else
                        {
                            objDr["ReferenceName"] = hdnFieldIdGrid.Value;
                        }
                    }
                }

                objDr = objDt.NewRow();
                objDr["FieldId"] = hdnFieldId.Value;
                objDr["FieldName"] = lblFieldName.Text;
                Label lblReferenceNamePrevious = (Label)gvSelectedFields.Rows[gvSelectedFields.Rows.Count - 1].FindControl("lblReferenceName");

                if (objDt.Rows.Count > 0)
                {
                    string s = common.myStr(objDt.Rows[objDt.Rows.Count - 1]["ReferenceName"]);
                    if (s.Trim() != "")
                    {
                        objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(s)) + 1);
                        objDr["TotalCalc"] = 0;
                    }
                    else
                    {
                        objDr["ReferenceName"] = "A";
                        objDr["TotalCalc"] = 0;
                    }
                }
                else
                {
                    objDr["ReferenceName"] = "A";
                    objDr["TotalCalc"] = 0;
                }
                objDt.Rows.Add(objDr);


                gvSelectedFields.DataSource = objDt;
                gvSelectedFields.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvSelectedFields_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete1")
            {
                string sReference = "";
                int iCount = 0;
                string rowId = common.myStr(e.CommandArgument);

                DataTable objDt = new DataTable();
                DataRow objDr;
                objDt = ((DataTable)ViewState["SelectedFieldsClone"]).Clone();
                if (objDt == null)
                {
                    return;
                }

                foreach (GridViewRow item in gvSelectedFields.Rows)
                {
                    objDr = objDt.NewRow();
                    Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
                    HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
                    Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
                    TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
                    CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

                    if (hdnFieldIdGrid.Value.Trim() != "")
                    {
                        sReference = lblReferenceName.Text;
                        objDr["FieldID"] = hdnFieldIdGrid.Value;
                        objDr["FieldName"] = lblFieldNameGrid.Text;
                        objDr["FormulaDefinition"] = txtFormulaDefinition.Text;
                        objDr["TotalCalc"] = chkTotalCalc.Checked ? 1 : 0;

                        if (iCount == 0)
                        {
                            objDr["ReferenceName"] = "A";
                        }
                        else
                        {
                            Label lblReferencePrevious = (Label)gvSelectedFields.Rows[iCount - 1].FindControl("lblReferenceName");
                            objDr["ReferenceName"] = (char)(Convert.ToInt16(Convert.ToChar(lblReferencePrevious.Text)) + 1);
                        }
                        objDt.Rows.Add(objDr);
                        iCount++;
                    }
                    else
                        objDr["ReferenceName"] = hdnFieldIdGrid.Value;
                }

                objDt.Select("FieldID='" + e.CommandArgument + "'").FirstOrDefault().Delete();
                objDt.AcceptChanges();

                if (objDt.Rows.Count == 0)
                {
                    objDr = objDt.NewRow();
                    objDt.Rows.Add(objDr);
                }

                gvSelectedFields.DataSource = objDt;
                gvSelectedFields.DataBind();
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
        DataSet ds = new DataSet();
        BaseC.EMROrders objE = new BaseC.EMROrders(sConString);
        try
        {
            ds = objE.getEMRTemplateFieldsFormula(common.myInt(ViewState["SectionId"]));

            ViewState["SelectedFieldsClone"] = ds.Tables[0].Copy().Clone();

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvSelectedFields.DataSource = ds.Tables[0];
            gvSelectedFields.DataBind();
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

    private bool chkFormula(string strFormula, ArrayList ar)
    {
        bool isOk = true;
        string alphaCH = "";
        string numCH = "";
        int asciiVal = 0;
        int asciiValP = 0;

        int startPre = 0;
        int closePre = 0;

        string s = "";
        if (strFormula != "")
        {
            strFormula = strFormula.ToUpper();

            for (int icount = 0; icount < strFormula.Length; icount++)
            {
                s = strFormula.Substring(icount, 1);

                if (ar.Contains(s) == false)
                {
                    lblMessage.Text = "Enter valid formula !";
                    isOk = false;
                }

                asciiVal = (int)Convert.ToChar(s);

                if (asciiVal == 40)
                {
                    startPre++;
                }
                if (asciiVal == 41)
                {
                    if (startPre > closePre)
                    {
                        closePre++;
                    }
                    else
                    {
                        lblMessage.Text = "Prenthes are mismatch !";
                        isOk = false;
                    }
                }

                if (icount == 0)
                {
                    if (asciiVal >= 65 && asciiVal <= 90)
                    {
                        alphaCH = s;
                    }

                    if (asciiVal >= 65 && asciiVal <= 90)
                    {
                        alphaCH = s;
                    }
                    else if (asciiVal >= 48 && asciiVal <= 57)
                    {
                        numCH = s;
                    }
                }
                else if (icount > 0)
                {
                    string p = strFormula.Substring(icount - 1, 1);
                    asciiValP = (int)Convert.ToChar(p);

                    if ((asciiValP >= 65 && asciiValP <= 90)
                        && (asciiVal >= 65 && asciiVal <= 90))
                    {
                        lblMessage.Text = "Opreator should be between two operands !";
                        isOk = false;
                    }

                    if ((asciiValP >= 65 && asciiValP <= 90)
                        && (icount != strFormula.Length - 1))
                    {
                        if ((asciiVal < 40 || asciiVal > 47) || asciiVal == 46)
                        {
                            lblMessage.Text = "Formula should contain operators at appropriate positions !";
                            isOk = false;
                        }
                    }

                    if (asciiVal >= 65 && asciiVal <= 90)
                    {
                        if ((asciiValP < 40 || asciiValP > 47) || asciiVal == 46)
                        {
                            lblMessage.Text = "Formula should contain operators at appropriate positions !";
                            isOk = false;
                        }
                    }
                }
            }

            if (startPre != closePre)
            {
                lblMessage.Text = "Prenthes are mismatch !";
                isOk = false;
            }
            
            if (strFormula.EndsWith("+") || strFormula.EndsWith("-")
                || strFormula.EndsWith("*") || strFormula.EndsWith("/")
                || strFormula.EndsWith("."))
            {
                lblMessage.Text = "Operator should be between two operands !";
                isOk = false;
            }

            if (strFormula.StartsWith("+") || strFormula.StartsWith("-")
                       || strFormula.StartsWith("*") || strFormula.StartsWith("/")
                       || strFormula.StartsWith("."))
            {
                lblMessage.Text = "Operator should be between two operands !";
                isOk = false;
            }
        }

        return isOk;
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        ArrayList ar = new ArrayList();
        ar.Add("+");
        ar.Add("-");
        ar.Add("*");
        ar.Add("/");
        ar.Add("(");
        ar.Add(")");
        ar.Add(".");
        ar.Add("0");
        ar.Add("1");
        ar.Add("2");
        ar.Add("3");
        ar.Add("4");
        ar.Add("5");
        ar.Add("6");
        ar.Add("7");
        ar.Add("8");
        ar.Add("9");

        int i = 1;

        StringBuilder strXml = new StringBuilder();
        ArrayList coll = new ArrayList();

        foreach (GridViewRow item in gvSelectedFields.Rows)
        {
            Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
            HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
            Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
            TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");

            if (hdnFieldIdGrid.Value.Trim() != "")
            {
                coll.Add(hdnFieldIdGrid.Value);
                coll.Add(lblReferenceName.Text);
                coll.Add(i);
                strXml.Append(common.setXmlTable(ref coll));

                i++;

                if (lblReferenceName.Text.Trim() != "")
                {
                    ar.Add(lblReferenceName.Text);
                }

                if (common.myLen(txtFormulaDefinition.Text.Trim()) > 0)
                {
                    if (!chkFormula(common.myStr(txtFormulaDefinition.Text).Trim(), ar))
                    {
                        return false;
                    }
                }
            }
        }

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        int rIdx = 0;
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

            foreach (GridViewRow item in gvSelectedFields.Rows)
            {
                Label lblFieldNameGrid = (Label)item.FindControl("lblFieldName");
                HiddenField hdnFieldIdGrid = (HiddenField)item.FindControl("hdnFieldId");
                Label lblReferenceName = (Label)item.FindControl("lblReferenceName");
                TextBox txtFormulaDefinition = (TextBox)item.FindControl("txtFormulaDefinition");
                CheckBox chkTotalCalc = (CheckBox)item.FindControl("chkTotalCalc");

                if (hdnFieldIdGrid.Value.Trim() != "")
                {
                    coll.Add(hdnFieldIdGrid.Value);
                    coll.Add(lblReferenceName.Text.Trim().ToUpper());
                    coll.Add(txtFormulaDefinition.Text.Trim().ToUpper());
                    coll.Add(chkTotalCalc.Checked ? 1 : 0);
                    coll.Add(rIdx);

                    strXML.Append(common.setXmlTable(ref coll));

                    rIdx++;
                }
            }

            string strMsg = objE.saveEMRTemplateFormulaFields(common.myInt(ViewState["SectionId"]), strXML.ToString(), common.myInt(Session["UserID"]));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                clearControl();

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



    //protected void cmdSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);



    //            BaseC.clsTemplateFieldGroup obj = new BaseC.clsTemplateFieldGroup(sConString);
    //            string strMsg = obj.SaveData(common.myInt(Request.QueryString["FId"]), strXml.ToString(), "", txtFormula.Text, common.myInt(Session["UserID"]));

    //            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
    //            {
    //                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
    //            }

    //            lblMessage.Text = strMsg;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}











}