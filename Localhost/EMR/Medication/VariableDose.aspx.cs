using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Text;
using System.IO;

public partial class EMR_Medication_VariableDose : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblItemName.Text = "Item Name : " + common.myStr(Request.QueryString["IName"]);

            if (common.myDbl(Request.QueryString["Dose"]) > 0)
            {
                lblItemName.Text = lblItemName.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Dose Qty : " + common.myInt(Request.QueryString["Dose"]).ToString();
            }
            if (common.myStr(Request.QueryString["From"]) == "Ward" || common.myStr(Request.QueryString["From"]) == "OPPres")
            {
                gvData.Visible = false;
                btnSave.Visible = false;
                DisplayVariableDose();
            }
            else
            {
                gvDisplayVariableDose.Visible = false;
                CreateTable();
            }
            if (common.myStr(Request.QueryString["DisplayMsg"]) == "1")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                // lblMessage.Text = "You Changed into duration or frequency dose so need to fill value for dose. ";
                lblMessage.Text = "Please recheck variable dose. ";
            }
        }
    }
    void CreateTable()
    {
        int totaldays = common.myInt(Request.QueryString["Day"]);
        DataSet ds = new DataSet();
        DataSet dsXml = new DataSet();
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            HshIn.Add("@intFrequencyId", common.myInt(Request.QueryString["FrequencyId"]));
            HshIn.Add("@intDays", totaldays);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetVariableDose", HshIn);

            int NoOfDose = 1;
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    NoOfDose = common.myInt(ds.Tables[1].Rows[0]["NoOfDose"]);
                }
            }
            if (NoOfDose.Equals(0))
            {
                NoOfDose = 1;
            }

            ViewState["NoOfDose"] = common.myInt(NoOfDose).ToString();


            if (!ds.Tables[0].Columns.Contains("Dose13"))
            {
                ds.Tables[0].Columns.Add("Dose13", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose14"))
            {
                ds.Tables[0].Columns.Add("Dose14", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose15"))
            {
                ds.Tables[0].Columns.Add("Dose15", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose16"))
            {
                ds.Tables[0].Columns.Add("Dose16", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose17"))
            {
                ds.Tables[0].Columns.Add("Dose17", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose18"))
            {
                ds.Tables[0].Columns.Add("Dose18", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose19"))
            {
                ds.Tables[0].Columns.Add("Dose19", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose20"))
            {
                ds.Tables[0].Columns.Add("Dose20", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose21"))
            {
                ds.Tables[0].Columns.Add("Dose21", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose22"))
            {
                ds.Tables[0].Columns.Add("Dose22", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose23"))
            {
                ds.Tables[0].Columns.Add("Dose23", typeof(String));
            }
            if (!ds.Tables[0].Columns.Contains("Dose24"))
            {
                ds.Tables[0].Columns.Add("Dose24", typeof(String));
            }


            if (Cache["VariableDose" + common.myStr(Session["UserId"])] != null)
            {

                hdnXmlString.Value = Cache["VariableDose" + common.myStr(Session["UserId"])].ToString();
                Cache.Remove("VariableDose" + common.myStr(Session["UserId"]));
                StringReader sr = new StringReader(hdnXmlString.Value);
                dsXml = ds.Clone();
                dsXml.ReadXml(sr);


                if (ds.Tables[0].Rows.Count > 0)// Hide or add rows for days wise
                {
                    int Savedrow = dsXml.Tables[0].Rows.Count;
                    int NewRow = ds.Tables[0].Rows.Count;
                    if (NewRow <= Savedrow)
                    {
                        for (int row = 0; row < (Savedrow - NewRow); row++)
                        {
                            dsXml.Tables[0].Rows[NewRow].Delete();
                        }
                    }
                    else
                    {
                        for (int row = 0; row < (NewRow - Savedrow); row++)
                        {
                            DataRow nr = dsXml.Tables[0].NewRow();
                            dsXml.Tables[0].Rows.Add(nr);
                            dsXml.Tables[0].Rows[Savedrow + row]["Date"] = ds.Tables[0].Rows[Savedrow + row]["Date"];
                        }
                    }
                }
                gvData.DataSource = dsXml;
                gvData.DataBind();

            }
            else
            {
                dsXml = ds;
                gvData.DataSource = dsXml;
                gvData.DataBind();
                if (common.myInt(Request.QueryString["Dose"]) > 0)// fill the value
                {
                    foreach (GridViewRow gvRow in gvData.Rows)
                    {
                        for (int Col = 1; Col <= NoOfDose; Col++)
                        {
                            string colname = "Dose" + common.myStr(Col);
                            ((TextBox)gvRow.FindControl("txt" + colname)).Text = common.myStr(Request.QueryString["Dose"]);
                        }
                    }
                }
            }
            if (NoOfDose > 0)//for hide extra colume frequenc wise
            {
                for (int col = 1; col <= 24 - NoOfDose; col++)
                {
                    gvData.Columns[NoOfDose + col].Visible = false;
                }
            }

            ViewState["VariableDose"] = dsXml;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dsXml.Dispose();
            ds.Dispose();
        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        hdnXmlString.Value = "";

        DataTable dtgrd = new DataTable();
        dtgrd = ((DataSet)ViewState["VariableDose"]).Tables[0].Clone();
        foreach (GridViewRow gvRow in gvData.Rows)
        {
            if (common.myInt(ViewState["NoOfDose"]) > 0)
            {
                DataRow dRow = dtgrd.NewRow();
                dRow["Date"] = common.myStr(((Label)gvRow.FindControl("lblDate")).Text.Trim());
                for (int Col = 1; Col <= common.myInt(ViewState["NoOfDose"]); Col++)
                {
                    string colname = "Dose" + common.myStr(Col);
                    dRow[colname] = common.myStr(((TextBox)gvRow.FindControl("txt" + colname)).Text.Trim());
                }
                dtgrd.Rows.Add(dRow);
            }
        }

        DataSet ds = new DataSet();
        ds.Tables.Add(dtgrd.Copy());
        DataSet dsResponse = new DataSet();
        System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
        System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
        ds.WriteXml(writer);
        string xmlSchema = writer.ToString();
        hdnXmlString.Value = xmlSchema;
        hdnvariableDoseDuration.Value = common.myStr(Request.QueryString["Day"]);
        hdnvariableDoseFrequency.Value = common.myStr(Request.QueryString["FrequencyId"]);
        hdnVariabledose.Value = common.myStr(Request.QueryString["Dose"]);


        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        // return;
    }

    void DisplayVariableDose()
    {

        DataSet ds = new DataSet();
        DataTable dtdose = new DataTable();
        DataView dv = new DataView();
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            HshIn.Add("@intFrequencyId", common.myInt(Request.QueryString["FrequencyId"]));
            HshIn.Add("@intDays", common.myInt(Request.QueryString["Day"]));
            HshIn.Add("@intIndentId", common.myStr(Request.QueryString["IndentId"]));
            HshIn.Add("@intItemId", common.myStr(Request.QueryString["ItemId"]));
            HshIn.Add("@chrUseFor", common.myStr(Request.QueryString["From"]));

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetVariableDose", HshIn);

            int NoOfDose = common.myInt(Request.QueryString["Frequency"]);

            dtdose = ds.Tables[1];

            for (int i = 1; i <= NoOfDose; i++)
            {
                dtdose.Columns.Add("Dose" + common.myStr(i));
            }
            for (int row = 0; row < dtdose.Rows.Count; row++)
            {
                dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "VariableDoseDate='" + common.myStr(dtdose.Rows[row]["VariableDoseDate"]) + "'";
                for (int drow = 0; drow < dv.ToTable().Rows.Count; drow++)
                {
                    dtdose.Rows[row]["Dose" + common.myStr(drow + 1)] = common.myStr(dv.ToTable().Rows[drow]["Dose"]);
                }
            }
            dtdose.AcceptChanges();
            gvDisplayVariableDose.DataSource = dtdose;
            gvDisplayVariableDose.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            dtdose.Dispose();
            ds.Dispose();
            dv.Dispose();

        }
    }
}
