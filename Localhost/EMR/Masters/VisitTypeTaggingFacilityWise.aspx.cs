using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

public partial class EMR_Masters_VisitTypeTaggingFacilityWise : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();
    BaseC.EMRMasters.EMRVisit objEMRVisit;
    DataSet ds;
    StringBuilder strXML;
    ArrayList coll;

    private enum GridVisit : byte
    {
        VisitID = 0,
        Status = 1,
        Edit = 2,
        Active = 3
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindFacility();
            BindVisitType();
            BindGrid();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objEMRVisit = new BaseC.EMRMasters.EMRVisit(sConString);
            coll = new ArrayList();
            strXML = new StringBuilder();
            string strsave = "";

            foreach (RadComboBoxItem rd in ddlFacility.CheckedItems)
            {
                if (rd.Checked)
                {
                    coll.Add(common.myInt(rd.Value));
                    strXML.Append(common.setXmlTable(ref coll));
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please Select Facility !";
                }
            }
            if (strXML.ToString() != "")
            {
                strsave = objEMRVisit.SaveVisitTypeFacility(common.myInt(ddlVisitType.SelectedValue), strXML.ToString(), common.myInt(Session["UserId"]));
                if (strsave.Contains("Save"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strsave;

                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strsave;
                }


            }
            Clear();
            BindGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindGrid()
    {
        try
        {
            objEMRVisit = new BaseC.EMRMasters.EMRVisit(sConString);
            ds = new DataSet();
            if (common.myStr(ddlVisitType.SelectedValue) != "")
            {
                gvVisitType.Enabled = true;
                ds = objEMRVisit.GetVisitTypeFacilityWise(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlVisitType.SelectedValue));
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    ds.Tables[0].AcceptChanges();
                }
                gvVisitType.DataSource = ds;
                gvVisitType.DataBind();
            }
            else
            {
                BindBlankGrid(1);
                gvVisitType.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindFacility()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
            HashIn.Add("@intUserId", Session["UserID"]);
            HashIn.Add("@intGroupId", Session["GroupID"]);
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "uspGetFacilityList", HashIn);

            ddlFacility.DataSource = dr;
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();
            //ddlFacility.Items.Insert(0, new RadComboBoxItem("All Facilites", "0"));
            //ddlFacility.Items[0].Value = "0";

            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void BindVisitType()
    {
        try
        {
            objEMRVisit = new BaseC.EMRMasters.EMRVisit(sConString);
            ds = new DataSet();
            ds = objEMRVisit.GetVisitType(common.myInt(Session["HospitalLocationId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVisitType.DataSource = ds;
                ddlVisitType.DataTextField = "VisitTypeName";
                ddlVisitType.DataValueField = "VisitTypeID";
                ddlVisitType.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void gvVisitType_OnDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[3].Visible = false;

    }

    protected void gvVisitType_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        try
        {

            gvVisitType.EditIndex = -1;
            BindGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVisitType_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            //  if(gvVisitType.
            objEMRVisit = new BaseC.EMRMasters.EMRVisit(sConString);
            HiddenField hdnId = (HiddenField)gvVisitType.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.VisitID)].FindControl("hdnId");
            DropDownList strDrop = ((DropDownList)gvVisitType.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.Status)].FindControl("ddlGridStatus"));
            int i = objEMRVisit.UpdateVisitTypeFacility(common.myInt(hdnId.Value), common.myInt(strDrop.SelectedValue), common.myInt(Session["UserId"]), common.myDate(DateTime.Now));
            if (i == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Record Updated....";
            }
            gvVisitType.EditIndex = -1;
            BindGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVisitType_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        try
        {
            // Label EMTypeId = (Label)gvVisit.Rows[e.RowIndex].Cells[Convert.ToByte(GridVisit.EMType)].FindControl("lblemtype");
            gvVisitType.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlVisitType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            BindGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void Clear()
    {
        foreach (RadComboBoxItem rd in ddlFacility.CheckedItems)
        {
            rd.Checked = false;
        }
        
        ddlFacility.SelectedIndex = -1;
        ddlFacility.Text = "";
        ddlFacility.EmptyMessage = "/Select";




    }

    void BindBlankGrid(int row)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id");
        dt.Columns.Add("Name");
        dt.Columns.Add("Status");
        dt.Columns.Add("Active");

        DataRow dr = dt.NewRow();
        for (int i = 0; i < row; i++)
        {
            dr["Id"] = DBNull.Value;
            dr["Name"] = DBNull.Value;
            dr["Status"] = DBNull.Value;
            dr["Active"] = 0;
        }
        dt.Rows.Add(dr);
        gvVisitType.DataSource = dt;
        gvVisitType.DataBind();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        ddlFacility.SelectedIndex = -1;
        ddlFacility.Text = "";
        ddlFacility.EmptyMessage = "/Select";

        foreach (RadComboBoxItem rd in ddlFacility.CheckedItems)
        {
            rd.Checked = false;

        }

        ddlVisitType.SelectedIndex = -1;
        ddlVisitType.Text = "";
        ddlVisitType.EmptyMessage = "/Select";

        BindBlankGrid(1);

        gvVisitType.Enabled = false;
    }
}
