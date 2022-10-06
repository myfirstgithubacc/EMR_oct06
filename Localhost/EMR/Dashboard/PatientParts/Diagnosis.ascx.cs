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
using BaseC;
using Telerik.Web.UI;
public partial class EMR_Dashboard_Parts_Assessment : WebPartBase
{
    
    DL_Funs fun = new DL_Funs();
    clsExceptionLog objException = new clsExceptionLog();

    public EMR_Dashboard_Parts_Assessment()
    {
        this.Title = "Diagnosis";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindGDDiagnosis();

        if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
        {
            lnkAddAssessment.Enabled = true;
        }
        else
        {

            lnkAddAssessment.Enabled = false;
        }
        if (common.myStr(HttpContext.Current.Session["ModuleIdValue"]) == "41")
        {
            lnkAddAssessment.Enabled = false;
        }
    }

    public void BindGDDiagnosis()
    {
        try
        {
            Dashboard dsh = new Dashboard();

            DataSet dsApp = new DataSet();
            dsApp = dsh.getDashBoardValue(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(Session["registrationid"]), 
                                        common.myStr(hdnFromDate.Text), common.myStr(hdnToDate.Text), common.myStr(hdnDateVale.Text), 
                                        hdnEncounterNumber.Text==""? common.myInt(Session["EncounterId"]).ToString(): common.myInt(hdnEncounterNumber.Text).ToString(), 
                                        common.myInt(Session["FacilityID"]), "UspEMRGetPatientDiagnosis");

            GDAssessment.DataSource = dsApp;
            GDAssessment.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GDAssessment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDAssessment.PageIndex = e.NewPageIndex;
        BindGDDiagnosis();
    }

    protected void GDAssessment_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;

            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strCondition = "";
            e.Row.Cells[0].ToolTip = DataBinder.Eval(e.Row.DataItem, "ICDDescription").ToString() + ", " + DataBinder.Eval(e.Row.DataItem, "ICDCode").ToString();
            if (e.Row.Cells[1].Text == "True")
                e.Row.Cells[1].Text = "Y";
            else
                e.Row.Cells[1].Text = "";
            e.Row.Cells[0].Text = e.Row.Cells[0].Text.PadRight(38).Substring(0, 35);
            Label lblCondition = e.Row.FindControl("lblCondition") as Label;
            if (Convert.ToString(e.Row.Cells[2].Text.Trim()) != "" && Convert.ToString(e.Row.Cells[2].Text.Trim()) != "&nbsp;")
            {
                if (((Convert.ToString(e.Row.Cells[3].Text.Trim()) != "") && (Convert.ToString(e.Row.Cells[3].Text.Trim()) != "&nbsp;")) || ((Convert.ToString(e.Row.Cells[4].Text.Trim()) != "") && (Convert.ToString(e.Row.Cells[4].Text.Trim()) != "&nbsp;")))
                {
                    strCondition = Convert.ToString(e.Row.Cells[2].Text.Trim()) + ", ";
                }
                else
                    strCondition = Convert.ToString(e.Row.Cells[2].Text.Trim());
            }
            if (Convert.ToString(e.Row.Cells[3].Text.Trim()) != "" && Convert.ToString(e.Row.Cells[3].Text.Trim()) != "&nbsp;")
            {
                if (Convert.ToString(e.Row.Cells[4].Text.Trim()) != "" && Convert.ToString(e.Row.Cells[4].Text.Trim()) != "&nbsp;")
                {
                    strCondition = strCondition + Convert.ToString(e.Row.Cells[3].Text.Trim()) + ", ";
                }
                else
                    strCondition = strCondition + Convert.ToString(e.Row.Cells[3].Text.Trim());
            }
            if (Convert.ToString(e.Row.Cells[4].Text.Trim()) != "" && Convert.ToString(e.Row.Cells[4].Text.Trim()) != "&nbsp;")
            {
                strCondition = strCondition + Convert.ToString(e.Row.Cells[4].Text.Trim());
            }
            lblCondition.Text = strCondition;

            if (e.Row.Cells[8].Text.Trim() == "False")
            {
                if (e.Row.Cells[7].Text.Trim() == "True")
                {
                    e.Row.Cells[1].Text = "Chronic";
                }
                if (e.Row.Cells[9].Text.Trim() == "True")
                {
                    e.Row.Cells[1].Text = "Primary";
                }

            }
            else
            {
                e.Row.Cells[1].Text = "Resolved";
            }


        }
    }
    public void setLink()
    {

        
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindGDDiagnosis();
    }
    protected void lnkDiagnosis_OnClick(object sender, EventArgs e)
    {
        setLink();
    }
}
