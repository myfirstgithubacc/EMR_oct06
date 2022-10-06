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

public partial class EMR_Vitals_AddVitalValues : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        // strVitalID = Request.QueryString["VitalId"].ToString();
        if (!IsPostBack)
        {
            //strVitalID = Request.QueryString["VitalId"].ToString();
            BindVitalValue();
            //btnClose.Attributes.Add("onclick", "window.close()");
        }
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {

    }

    private void BindVitalValue()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strVitalValue = "select * from EMRVitalValues where VitalID=" + Request.QueryString["VitalId"].ToString() + " and active=1";
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, strVitalValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
            }
            else
            {
                gvDetails.DataSource = null;
                gvDetails.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDetails.EditIndex = e.NewEditIndex;
        BindVitalValue();
    }

    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string strVitalValue = "select ImageName from EMRVitalValues where VitalID=" + Request.QueryString["VitalId"].ToString() + " and active=1";
        DataSet ds = new DataSet();
        ds = dl.FillDataSet(CommandType.Text, strVitalValue);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["ImageName"].ToString() != "")
            {
                e.Row.Cells[2].Visible = true;
            }
            else
            {
                e.Row.Cells[2].Visible = false;
                fileUplaod.Visible = false;
                Label1.Visible = false;
            }
        }
    }

    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshTable = new Hashtable();
            string intVitalid = gvDetails.DataKeys[e.RowIndex].Value.ToString();
            //TextBox txtvitalName = (TextBox)gvVitalView.Rows[e.RowIndex].Cells[1].FindControl("txtVitalName");
            TextBox txtname = (TextBox)gvDetails.Rows[e.RowIndex].Cells[1].FindControl("txtName");
            string strUpdate = "update EMRVitalValues set name='" + txtname.Text.ToString() + "' where ValueID=" + intVitalid + "";
            int i = dl.ExecuteNonQuery(CommandType.Text, strUpdate);
            if (i == 0)
            {
                lblMsg.Text = "Update Record!";
            }
            gvDetails.EditIndex = -1;
            BindVitalValue();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDetails.EditIndex = -1;
        BindVitalValue();
    }

    protected void btnSaveData_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsinput = new Hashtable();
            String FilePath = "";

            FilePath = "/Images/VitalImages/" + fileUplaod.FileName;
            hsinput.Add("@chrName", txtValueName.Text);
            hsinput.Add("@intVitalID", Request.QueryString["VitalId"].ToString().Trim());
            hsinput.Add("@intEncodedBy", Convert.ToInt32(Session["UserID"]));
            if (fileUplaod.PostedFile.FileName != "")
            {
                hsinput.Add("@ImageName", fileUplaod.PostedFile.FileName);
                fileUplaod.SaveAs(Server.MapPath("/Images/VitalImages/" + fileUplaod.FileName));
            }
            else
            {
                hsinput.Add("@ImageName", DBNull.Value);
            }
            hsinput.Add("@ImagePath", FilePath);

            int i = dl.ExecuteNonQuery(CommandType.StoredProcedure, "EMRSaveVitalValues", hsinput);
            if (i == 0)
            {
                lblMsg.Text = "Save Record!";
            }
            else
            {
                lblMsg.Text = "No Save Record!";
            }
            txtValueName.Text = "";
            BindVitalValue();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string id = gvDetails.DataKeys[e.RowIndex].Value.ToString();
            string strDelete = "update EMRVitalValues set active=0 where valueID=" + id + "";
            int i = dl.ExecuteNonQuery(CommandType.Text, strDelete);
            if (i == 0)
            {
                lblMsg.Text = "Delete Record!";
            }
            BindVitalValue();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
