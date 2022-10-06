using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EMRBILLING_Popup_RejectedDrug : System.Web.UI.Page
{

    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    BaseC.WardManagement objwd;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }

    }

    private void BindGrid()
    {
        try
        {
            objwd = new BaseC.WardManagement();
            ds = new DataSet();
            BaseC.Patient pt = new BaseC.Patient(sConString);

            ds = objwd.GetRejectedDrugs(common.myInt(common.myInt(Request.QueryString["encId"])), common.myInt(Session["FacilityId"]), common.myBool(chkRejectedDrug.Checked));

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvRejectedDrug.DataSource = ds;
                        gvRejectedDrug.DataBind();
                    }
                    else
                    {
                        BindBlankItemGrid();
                    }
                }
                else
                {
                    BindBlankItemGrid();
                }
            }
            else
            {
                BindBlankItemGrid();
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

    protected void chkRejectedDrug_OnCheckedChanged(Object sender, EventArgs args)
    {
        BindGrid();
    }
    private void BindBlankItemGrid()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        try
        {

            dr["ItemName"] = string.Empty;
            dr["Remarks"] = string.Empty;
            dr["RejectedBy"] = string.Empty;
            dr["RejectionDate"] = string.Empty;
            dr["AdvisingDoctor"] = string.Empty;
            dr["EncodedBy"] = string.Empty;
            dr["EncodedDate"] = string.Empty;
            dr["Store"] = string.Empty;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvRejectedDrug.DataSource = dt;
            gvRejectedDrug.DataBind();

        }

        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            dt.Dispose();
        }
    }

    protected DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("RejectedBy", typeof(string));
            dt.Columns.Add("RejectionDate", typeof(string));
            dt.Columns.Add("AdvisingDoctor", typeof(string));
            dt.Columns.Add("EncodedBy", typeof(string));
            dt.Columns.Add("EncodedDate", typeof(string));
            dt.Columns.Add("Store", typeof(string));

            return dt;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
            return dt;
        }

    }

}