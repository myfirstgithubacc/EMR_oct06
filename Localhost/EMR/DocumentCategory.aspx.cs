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

public partial class EMR_DocumentCategory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    Hashtable hstInput;
    String status = "";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["Master"]) != "Yes")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myStr(Request.QueryString["DcoType"]) != "")
            {
                GetDocumentType();
                lblCatName.Text = "Docment Refer Type";
                gvDocumentCategory.Columns[1].HeaderText = "Docment Type";
            }
            else
            {
                lblCatName.Text = "Category Name";
                GetCategory();
                gvDocumentCategory.Columns[1].HeaderText = "Category Name";
            }
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsCategory = new DataSet();
            DataTable dt = new DataTable();
            dsCategory = (DataSet)ViewState["CategoryDetails"];
            if (dsCategory.Tables.Count > 0)
            {
                dt = dsCategory.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        if (dt.Rows[i]["Description"].Equals(common.myStr(txtCatName.Text)))
                        {

                            if (common.myStr(txtCatName.Text).Equals(""))
                            {
                                lblMsg.Text = "Category Name Allready Exist!";
                            txtCatName.Text = "";
                            return;
                            }
                        }

                    }
                }
            }
            BaseC.ParseData objParse = new BaseC.ParseData();

            if (txtCatName.Text != "")
            {
                if (common.myStr(Request.QueryString["DcoType"]) != "")
                {
                    DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    Hashtable hstInput = new Hashtable();
                    hstInput.Add("@chvCasename", txtCatName.Text.ToString());
                    hstInput.Add("@HospitalLocationId", Session["HospitalLocationId"]);
                    SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select DocumentType  from DocumentType where DocumentType=@chvCasename and ( HospitalLocationId=@HospitalLocationId or HospitalLocationId is null) ", hstInput);
                    if (objDr.Read())
                    {
                        lblMsg.Text = "Document Type Name Allready Exist!";
                        txtCatName.Text = "";
                    }
                    else
                    {

                        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        hstInput = new Hashtable();
                        hstInput.Add("@chvDocumentType", objParse.ParseQ(txtCatName.Text));
                        hstInput.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                        hstInput.Add("@intEncodedBy", Session["UserId"]);
                        objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDocumentsType", hstInput);
                        GetDocumentType();
                        txtCatName.Text = "";
                        // Alert.ShowAjaxMsg("Document Type Created!", Page);
                        lblMsg.Text = "Document Type Created!";
                    }
                    objDr.Close();

                }
                else
                {
                    DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    Hashtable hstInput = new Hashtable();
                    hstInput.Add("@chvCasename", txtCatName.Text.ToString());
                    hstInput.Add("@HospitalLocationId", Session["HospitalLocationId"]);
                    SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select Description  from EMRDocumentCategory where Description=@chvCasename and ( HospitalLocationId=@HospitalLocationId or HospitalLocationId is null) ", hstInput);
                    if (objDr.Read())
                    {
                        lblMsg.Text = "Category Name Allready Exist!";
                        txtCatName.Text = "";
                    }
                    else
                    {

                        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        hstInput = new Hashtable();
                        hstInput.Add("chvDescription", objParse.ParseQ(txtCatName.Text));
                        hstInput.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                        hstInput.Add("@intEncodedBy", Session["UserId"]);
                        objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDocumentsCategory", hstInput);
                        GetCategory();
                        txtCatName.Text = "";
                        //Alert.ShowAjaxMsg("Category Created!", Page);
                        lblMsg.Text = "Category Created!";
                    }
                    objDr.Close();


                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void btnclose_Click(object sender, EventArgs e)
    {
        string telrikstr = " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ";
        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", telrikstr, true);
    }


    protected void GetCategory()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hs = new Hashtable();
        hs.Add("@intHospitalLocationId", Session["HospitalLocationId"].ToString());
        DataSet ds = new DataSet();
        ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetAttachmentCategory", hs);
        ViewState["CategoryDetails"] = ds;
        gvDocumentCategory.DataSource = ds;
        gvDocumentCategory.DataBind();
    }
    protected void GetDocumentType()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hs = new Hashtable();
        hs.Add("@intHospitalLocationId", Session["HospitalLocationId"].ToString());
        DataSet ds = new DataSet();
        ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDocumentType", hs);
        gvDocumentCategory.DataSource = ds;
        gvDocumentCategory.DataBind();
    }

    protected void gvDocumentCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType != DataControlRowType.Pager)
            e.Row.Cells[0].Visible = false;



    }



    protected void gvDocumentCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDocumentCategory.EditIndex = e.NewEditIndex;

        if (common.myStr(Request.QueryString["DcoType"]) != "")
        {
            GetDocumentType();
        }
        else
        {
            GetCategory();
        }
    }
    protected void gvDocumentCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDocumentCategory.EditIndex = -1;
        if (common.myStr(Request.QueryString["DcoType"]) != "")
        {
            GetDocumentType();
        }
        else
        {
            GetCategory();
        }
    }
    protected void gvDocumentCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string strStatus = "";
            Hashtable hshTable = new Hashtable();

            if (common.myStr(Request.QueryString["DcoType"]) != "")
            {
                TextBox txtDescName = (TextBox)gvDocumentCategory.Rows[e.RowIndex].Cells[1].FindControl("txteditdescription");

                hshTable.Add("@chvDocumentType", txtDescName.Text);
                DropDownList strDrop = (DropDownList)gvDocumentCategory.Rows[e.RowIndex].Cells[2].FindControl("ddlactive");
                strStatus = strDrop.SelectedItem.Value;

                hshTable.Add("@bitActive", strStatus); //Request.QueryString["RegNo"];

                hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshTable.Add("@intDocumentId", gvDocumentCategory.Rows[e.RowIndex].Cells[0].Text);
                hshTable.Add("@intEncodedBy", Session["UserId"]);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDocumentsCategory", hshTable);

                //txtnotenumber.Text = hshOutput["chvDocumentNo"].ToString();
                gvDocumentCategory.EditIndex = -1;
                GetDocumentType();
                txtCatName.Text = "";
                // Alert.Show("Document Type  Updated!");
                lblMsg.Text = "Document Type  Updated!";
            }
            else
            {

                TextBox txtDescName = (TextBox)gvDocumentCategory.Rows[e.RowIndex].Cells[1].FindControl("txteditdescription");

                hshTable.Add("@chvDescription", txtDescName.Text);

                DropDownList strDrop = (DropDownList)gvDocumentCategory.Rows[e.RowIndex].Cells[2].FindControl("ddlactive");

                strStatus = strDrop.SelectedItem.Value;


                hshTable.Add("@bitActive", strStatus); //Request.QueryString["RegNo"];

                hshTable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
                hshTable.Add("@intCategoryId", gvDocumentCategory.Rows[e.RowIndex].Cells[0].Text);
                hshTable.Add("@intEncodedBy", Session["UserId"]);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDocumentsCategory", hshTable);

                //txtnotenumber.Text = hshOutput["chvDocumentNo"].ToString();
                gvDocumentCategory.EditIndex = -1;
                GetCategory();
                txtCatName.Text = "";
                //  Alert.Show("Category Updated!");
                lblMsg.Text = "Category Updated!";
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
