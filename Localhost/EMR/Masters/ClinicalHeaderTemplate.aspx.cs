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
using System.Text;
using System.IO;
using System.Data.SqlClient;

public partial class Include_Master_Default : System.Web.UI.Page
{
    DL_Funs fun = new DL_Funs();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Mpg"] != null)
            Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();

        if (!IsPostBack)
        {
            ViewState["Mode"] = "Mode";
            ViewState["Save"] = "Save";
            BindGridView(Convert.ToInt16(ddlRows.SelectedValue));
            BindHeaderGrid();

            lblMessage.Text = "&nbsp;";
        }
    }

    protected void BindHeaderGrid()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            DataSet ds = new DataSet();
            hstInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            ds = objDl.FillDataSet(CommandType.Text, "Select HeaderId, HeaderName, Active=Case when Active=1 then 'Active' else 'In-Active' end from EMRFormHeader where HospitalLocationId = @inyHospitalLocationId Order by HeaderName", hstInput);
            gvHeader.DataSource = ds;
            gvHeader.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindGridView(int rows)
    {
        try
        {
            DataTable dt2 = new DataTable(); //Create Temporary Table
            DataColumn dcId = new DataColumn("Id");
            dt2.Columns.Add(dcId);
            for (int i = 1; i <= rows; i++)
            {
                DataRow drow = dt2.NewRow();
                drow["id"] = 0;
                dt2.Rows.Add(drow);
            }
            gvHeaderTemplate.DataSource = dt2;
            gvHeaderTemplate.DataBind();
            foreach (GridViewRow row in gvHeaderTemplate.Rows)
            {
                for (int c = 0; c < row.Cells.Count; c++)
                {
                    gvHeaderTemplate.HeaderRow.Cells[c].Text = "Colomn " + (c + 1).ToString();
                    if (c >= Convert.ToInt16(ddlColomns.SelectedValue))
                    {
                        gvHeaderTemplate.HeaderRow.Cells[c].Visible = false;
                        row.Cells[c].Visible = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (ViewState["Save"].ToString() == "Save")
            Save_UpdateMethod("0");
        else
            Save_UpdateMethod(ViewState["HeaderId"].ToString());

    }

    protected void Save_UpdateMethod(string HeaderId)
    {
        try
        {
            if (IsValid)
            {
                System.Text.StringBuilder objStrTB = new System.Text.StringBuilder();
                ArrayList arr = new ArrayList();
                for (int i = 0; i < gvHeaderTemplate.Rows.Count; i++) // i is Row
                {
                    for (int c = 0; c < gvHeaderTemplate.Rows[i].Cells.Count; c++)
                    {
                        if (gvHeaderTemplate.Rows[i].Cells[c].Visible == true)
                        {
                            TextBox txtName = gvHeaderTemplate.Rows[i].FindControl("txtName" + c.ToString()) as TextBox;
                            DropDownList ddlHTemplate = gvHeaderTemplate.Rows[i].FindControl("ddlHTemplate" + c.ToString()) as DropDownList;
                            if (ddlHTemplate.SelectedValue != "0")
                            {
                                objStrTB.Append("<Table1>");
                                objStrTB.Append("<c1>");
                                objStrTB.Append(ddlHTemplate.SelectedValue);
                                objStrTB.Append("</c1>");

                                objStrTB.Append("<c2>");
                                objStrTB.Append(i + 1); //Row No.
                                objStrTB.Append("</c2>");

                                objStrTB.Append("<c3>");
                                objStrTB.Append(c + 1);//Col No.
                                objStrTB.Append("</c3>");

                                objStrTB.Append("<c4>");
                                objStrTB.Append(txtName.Text);
                                objStrTB.Append("</c4>");
                                objStrTB.Append("</Table1>");
                                arr.Add(ddlHTemplate.SelectedValue);
                            }
                        }
                    }
                }
                int ch = 0;
                for (int i = 0; i < arr.Count; i++)
                {
                    for (int j = i + 1; j < arr.Count; j++)
                    {
                        if (arr[i].ToString() == arr[j].ToString())
                        {
                            ch = 1;
                            break;
                        }
                    }
                }
                if (ch == 0)
                {
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    Hashtable hstInput;
                    hstInput = new Hashtable();
                    if (HeaderId != "0")
                    {
                        hstInput.Add("@intHeaderId", HeaderId);
                        hstInput.Add("@bitActive", ddlActive.SelectedValue);
                    }
                    hstInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
                    hstInput.Add("@chrHeaderName", txtHeaderName.Text.Trim());
                    hstInput.Add("@xmlHeaderDetails", objStrTB.ToString());
                    hstInput.Add("@charHeaderType", ddlType.SelectedValue);
                    if (chkBorder.Checked == true)
                    {
                        hstInput.Add("@bitShowBorder", chkBorder.Checked);
                    }

                    if (chkDate.Checked == true)
                    {
                        hstInput.Add("@bitShowDate", chkDate.Checked);
                    }
                    //hstInput.Add("@chrLogoUrl", imgLogo.ImageUrl.ToString());

                    //FileUploader.SaveAs(Server.MapPath("/Images/Logo/") + imgLogo.ImageUrl);
                    //hstInput.Add("@chrLogoAlign", rdlLogoAlignment.SelectedValue);
                    hstInput.Add("@intEncodedBy", Convert.ToInt16(Session["UserID"]));

                    Hashtable hstOutput = new Hashtable();
                    hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);

                    int ii = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRSaveFormHeader", hstInput, hstOutput);
                    if (ii == 0)
                    {
                        if (HeaderId == "0")
                            lblMessage.Text = "Header(s) have been Saved successfully.";
                        else
                            lblMessage.Text = "Header(s) have been Updated successfully.";
                        BindHeaderGrid();
                        BindGridView(Convert.ToInt16(ddlRows.SelectedValue));
                        txtHeaderName.Text = "";
                        chkDate.Checked = false;
                        chkBorder.Checked = false;
                    }
                }
                else
                    lblMessage.Text = "Sorry you have selected duplicate type. Kindly select unique type.";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvHeaderTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            DataSet ds = new DataSet();
            sql.Append("SELECT Id, ObjectName FROM EMRTemplateDataObjects WITH(NOLOCK) WHERE Active=1 ORDER BY ObjectName");
            ds = fun.ExecuteSql(sql);
            for (int i = 0; i < 3; i++)
            {
                DropDownList ddl = e.Row.FindControl("ddlHTemplate" + i.ToString()) as DropDownList;
                ddl.DataSource = ds;
                ddl.DataTextField = "ObjectName";
                ddl.DataValueField = "Id";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("[ Select ]", "0", true));
            }
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Mode"].ToString() == "Edit")
            {
                int rows = Convert.ToInt32(ddlRows.SelectedValue);
                int columns = Convert.ToInt32(ddlColomns.SelectedValue);
                DataTable dtTemp = new DataTable();
                for (int i = 1; i <= 3; i++) //columns
                {
                    dtTemp.Columns.Add(new DataColumn("Colomn" + i.ToString(), typeof(string)));
                }
                foreach (GridViewRow row in gvHeaderTemplate.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        DataRow dr1 = dtTemp.NewRow();
                        //gvHeaderTemplate.Columns.Count
                        for (int c = 1; c <= dtTemp.Columns.Count; c++)
                        {
                            TextBox txtName = row.Cells[c - 1].FindControl("txtName" + (c - 1).ToString()) as TextBox;
                            DropDownList ddlHTemplate = row.Cells[c - 1].FindControl("ddlHTemplate" + (c - 1).ToString()) as DropDownList;
                            string s1 = txtName.Text;
                            string s2 = ddlHTemplate.SelectedValue.ToString();
                            dr1["Colomn" + c.ToString()] = s1 + '*' + s2;
                        }
                        dtTemp.Rows.Add(dr1);
                    }
                }
                Cache["TempTable"] = dtTemp;

                BindGridView(Convert.ToInt16(ddlRows.SelectedValue));

                DataTable dtR = (DataTable)Cache["TempTable"];
                if (dtR.Rows.Count != 0)
                {
                    //dtR.Rows.Count
                    int tempRow = Convert.ToInt32(ddlRows.SelectedValue);
                    for (int c = 1; c <= rows; c++)//dtR.Rows.Count
                    {
                        //foreach (DataRow row in dtR.Rows )
                        //{

                        for (int i = 0; i < columns; i++) //dtR.Columns.Count
                        {
                            //TextBox txtName = gvHeaderTemplate.Rows[c - 1].FindControl("txtName" + (c - 1).ToString()) as TextBox;

                            TextBox txtName = gvHeaderTemplate.Rows[c - 1].FindControl("txtName" + i.ToString()) as TextBox;
                            DropDownList ddlHTemplate = gvHeaderTemplate.Rows[c - 1].FindControl("ddlHTemplate" + i.ToString()) as DropDownList;
                            string str = "";
                            if (c <= dtR.Rows.Count && i <= dtR.Columns.Count)
                            {
                                str = dtR.Rows[c - 1]["Colomn" + (i + 1).ToString()].ToString();
                            }
                            if (!String.IsNullOrEmpty(str))
                            {
                                string[] strArr = new string[2];
                                char[] splitchar = { '*' };
                                strArr = str.Split(splitchar);
                                txtName.Text = (String.IsNullOrEmpty(strArr[0])) ? "" : strArr[0];
                                string HTemplate = (String.IsNullOrEmpty(strArr[1])) ? "0" : strArr[1];
                                ListItem lstHTemplate = ddlHTemplate.Items.FindByValue(HTemplate);
                                if (lstHTemplate != null)
                                {
                                    lstHTemplate.Selected = true;
                                }
                            }
                            else
                            {
                                txtName.Text = String.Empty;
                                ddlHTemplate.SelectedValue = "0";

                            }
                        }
                    }
                }
            }
            else
            {
                BindGridView(Convert.ToInt16(ddlRows.SelectedValue));
                lblMessage.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvHeader_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[3].Visible = false;
        //}
    }

    #region // working on Image

    /// <summary>
    /// will call to remove the particular image
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>


    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void gvHeader_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            DataSet ds = new DataSet();
            string HeaderId = "0";
            HeaderId = gvHeader.DataKeys[gvHeader.SelectedIndex].Value.ToString();

            sql.Append(" Select ObjectId,FieldCaption,RowNo, ColNo from EMRFormHeaderDetails Where HeaderId=" + HeaderId + " And Active=1 Order by RowNo,ColNo");
            sql.Append(" Select HeaderId,HeaderName,ShowBorder,ShowDate,Active,HeaderType from EMRFormHeader Where HeaderId=" + HeaderId);
            sql.Append(" Select Max(RowNo)MaxRows, Max(ColNo) MaxCols from EMRFormHeaderDetails Where Active=1 And HeaderId=" + HeaderId);

            ds = fun.ExecuteSql(sql);
            if (ds.Tables.Count >= 3 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
            {
                ViewState["Save"] = "Update";
                ViewState["HeaderId"] = HeaderId;

                ddlRows.SelectedValue = ds.Tables[2].Rows[0]["MaxRows"].ToString();
                ddlColomns.SelectedValue = ds.Tables[2].Rows[0]["MaxCols"].ToString();

                BindGridView(common.myInt(ddlRows.SelectedValue));

                txtHeaderName.Text = ds.Tables[1].Rows[0]["HeaderName"].ToString();
                ddlType.SelectedValue = ds.Tables[1].Rows[0]["HeaderType"].ToString();
                if (ds.Tables[1].Rows[0]["ShowBorder"].ToString() == "True")
                    chkBorder.Checked = true;
                else
                    chkBorder.Checked = false;

                if (ds.Tables[1].Rows[0]["ShowDate"].ToString() == "True")
                    chkDate.Checked = true;
                else
                    chkDate.Checked = false;

                if (ds.Tables[1].Rows[0]["Active"].ToString() == "True")
                    ddlActive.SelectedValue = "1";
                else
                    ddlActive.SelectedValue = "0";

                int cc = 0;
                for (int r = 1; r <= 8; r++)
                {
                    DataView dvR = new DataView(ds.Tables[0]);
                    dvR.RowFilter = "RowNo=" + r.ToString();
                    DataTable dtR = dvR.ToTable();
                    if (dtR.Rows.Count != 0)
                    {
                        for (int c = 1; c <= dtR.Rows.Count; c++)
                        {
                            TextBox txtName = gvHeaderTemplate.Rows[r - 1].FindControl("txtName" + (c - 1).ToString()) as TextBox;
                            DropDownList ddlHTemplate = gvHeaderTemplate.Rows[r - 1].FindControl("ddlHTemplate" + (c - 1).ToString()) as DropDownList;
                            txtName.Text = ds.Tables[0].Rows[cc]["FieldCaption"].ToString();
                            ddlHTemplate.SelectedValue = ds.Tables[0].Rows[cc]["ObjectId"].ToString();
                            cc++;
                        }
                    }
                }

            }

            ViewState["Mode"] = "Edit";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnUpload_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// will call to Blank page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnNew_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["Mode"] = "Mode";
            ViewState["Save"] = "Save";
            txtHeaderName.Text = String.Empty;
            BindGridView(Convert.ToInt16(ddlRows.SelectedValue));
            BindHeaderGrid();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
