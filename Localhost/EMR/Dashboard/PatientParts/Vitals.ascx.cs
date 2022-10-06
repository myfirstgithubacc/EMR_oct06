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

public partial class EMR_Dashboard_Parts_Vitals : System.Web.UI.UserControl
{
    DL_Funs fun = new DL_Funs();
    Hashtable hsTb = new Hashtable();
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        
            BindGDVitals();
            if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
            {
                lnkAddVitals.Enabled = true;
            }
            else
            {
                lnkAddVitals.Enabled = false;
            }
     
    }

    public void BindGDVitals()
    {
        try
        {
            Dashboard dsh = new Dashboard();
            DataSet dsApp = new DataSet();
            
            dsApp = dsh.getDashBoardValue(Convert.ToInt16(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]),
                                        hdnFromDate.Text, hdnToDate.Text, hdnDateVale.Text, hdnEncounterNumber.Text == "" ? Session["EncounterId"].ToString() : hdnEncounterNumber.Text, 
                                        common.myInt(Session["FacilityID"]), "UspEMRGetPatientPreviousVitals");

            ViewState["GridData"] = dsApp.Tables[0];

            GDVitals.DataSource = dsApp;
            GDVitals.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GDVitals_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GDVitals.PageIndex = e.NewPageIndex;
        BindGDVitals();
    }
    public void setLink()
    {
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        BindGDVitals();
    }
    protected void lnkAddVitals_OnClick(object sender, EventArgs e)
    {
        setLink();
    }



    protected void GDVitals_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        { 
            if (e.Row.RowType == DataControlRowType.Header
               || e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int idx = 0; idx < e.Row.Cells.Count; idx++)
                {
                    if (e.Row.Cells[idx].Text.Contains("_")
                        || common.myStr(e.Row.Cells[idx].Text) == "0"
                        || common.myStr(e.Row.Cells[idx].Text) == "1"
                        || common.myStr(e.Row.Cells[idx].Text) == "VitalEntryDate1")
                    {
                        e.Row.Cells[idx].Visible = false;

                        //if (e.Row.RowType == DataControlRowType.DataRow)
                        //{
                        //    if (common.myStr(e.Row.Cells[idx].Text) == "1")
                        //    {
                        //        e.Row.Cells[idx].ForeColor = System.Drawing.Color.Red;
                        //    }
                        //}
                    }
                    else
                    {
                        e.Row.Cells[idx].Visible = true;
                        //e.Row.Cells[idx].ForeColor = System.Drawing.Color.Blue;
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[1].Visible = false;

                        if (common.myStr(this.GDVitals.HeaderRow.Cells[idx].Text) != "Vital Date"
                            && common.myStr(this.GDVitals.HeaderRow.Cells[idx].Text) != "Entered BY"
                            && common.myStr(e.Row.Cells[idx].Text) != "&nbsp;"
                            && common.myStr(e.Row.Cells[idx].Text).Trim() != "")
                        {
                            string HeaderColumn = common.myStr(this.GDVitals.HeaderRow.Cells[idx].Text);

                            e.Row.Cells[idx].Attributes.Add("onclick", "setValue('" + e.Row.Cells[idx].Text + "','" + HeaderColumn + "');");

                            if (common.myStr(e.Row.Cells[idx].Text) == "1")
                            {
                                if (ViewState["GridData"] != null)
                                {
                                    DataTable tbl = (DataTable)ViewState["GridData"];

                                    string strCol = HeaderColumn.Substring(0, HeaderColumn.IndexOf("_"));

                                    e.Row.Cells[tbl.Columns[strCol].Ordinal].ForeColor = System.Drawing.Color.Red;
                                }
                            }
                        }

                    }

                }
            }
           


        }
        catch (Exception Ex)
        {           
            objException.HandleException(Ex);
        }
    }


    //if (dsApp.Tables[0].Columns.Contains("Vital Date"))
    //        {
    //            string dateFormat = common.myStr(Session["OutputDateformat"]) + " hh:MM:tt";

    //            for (int rIdx = 0; rIdx < dsApp.Tables[0].Rows.Count; rIdx++)
    //            {
    //                dsApp.Tables[0].Rows[rIdx]["Vital Date"] = common.myDate(common.myDate(dsApp.Tables[0].Rows[rIdx]["Vital Date"]).ToString(dateFormat));
    //            }
    //            dsApp.Tables[0].AcceptChanges();
    //        }

}
