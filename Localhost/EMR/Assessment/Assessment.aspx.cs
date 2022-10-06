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

public partial class EMR_Problems_Assessment : System.Web.UI.Page
{
    DL_Funs fun = new DL_Funs();
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.DiagnosisBL bl = new BaseC.DiagnosisBL();   // Business Layer Class having properties
    static DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["GridCheckedItems"] = null;
            Session["GridCheckedIds"] = null;
            dt = null;
            dt = new DataTable();
            CreateTable(dt);
            Session["ICDcode"] = "";
            LstCategory.Items.Add(new ListItem("All", "0", true));
            LstCategory.AppendDataBoundItems = true;
            fun.BindDDLorLstBox(LstCategory, "CategoryBind");
            LstCategory.SelectedIndex = 0;
            fun.BindDataSetToGrid(GVDisease, "DiseasesBind", LstCategory.SelectedValue);
            showStatus(LstCategory.SelectedItem.Text, GVDisease);
            hdnStatus.Value = "CurrentAssessment";
            
        }
    }
    public void SaveGridCheckedItems(GridView grv, string chkControlName, string hdnUniqueId)
    {
        ArrayList arr = new ArrayList();
        ArrayList arr1 = new ArrayList();
        int index = -1;
        if (Session["GridCheckedItems"] != null)
            arr = Session["GridCheckedItems"] as ArrayList;
        if (Session["GridCheckedIds"] != null)
            arr1 = Session["GridCheckedIds"] as ArrayList;

        foreach (GridViewRow gvrow in grv.Rows)
        {
            index = common.myInt(grv.DataKeys[gvrow.RowIndex].Value);
            bool result = ((CheckBox)gvrow.FindControl(chkControlName)).Checked;
            int hdnId = common.myInt(((HiddenField)gvrow.FindControl(hdnUniqueId)).Value);
            if (result)
            {
                if (!arr.Contains(index))
                {
                    arr.Add(index);
                }
                else
                        if (arr.Contains(index))
                    arr.Remove(index);
                if (!arr1.Contains(hdnId))
                {
                    arr1.Add(hdnId);
                }
                else
                        if (arr1.Contains(hdnId))
                    arr1.Remove(hdnId);
                if (Session["ICDcode"] != null)
                    Session["ICDcode"] += hdnId + ",";
            }
        }
        Session["GridCheckedItems"] = arr;
        Session["GridCheckedIds"] = arr1;
    }

    public void SetGridCheckedItems(GridView grv, string chkControlName)
    {
        if (Session["GridCheckedItems"] == null)
            return;
        ArrayList arr = new ArrayList();
        arr = Session["GridCheckedItems"] as ArrayList;
        foreach (GridViewRow gvrow in grv.Rows)
        {
            int index = common.myInt(grv.DataKeys[gvrow.RowIndex].Value);

            if (arr.Contains(index))
                (((CheckBox)gvrow.FindControl(chkControlName))).Checked = true;
            else
                (((CheckBox)gvrow.FindControl(chkControlName))).Checked = false;
        }

    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindGridView2();
    }

    public void BindGridView2()
    {
        try
        {
            int pageSize = GVDisease.PageSize;
            string codes = "";
            if (pageSize > GVDisease.Rows.Count)
                pageSize = GVDisease.Rows.Count;
            for (int i = 0; i < pageSize; i++)
            {
                CheckBox chk = GVDisease.Rows[i].FindControl("chkInner") as CheckBox;
                HiddenField hdn1 = GVDisease.Rows[i].FindControl("hiddenICDCode") as HiddenField;
                if (chk.Checked)
                {
                    codes += hdn1.Value + ",";
                }
            }

            if (codes != "")
            {
                codes = ReturnICDCodes(codes);
                if (codes != "")
                {
                    DataSet ds = fun.ReturnDataset("MCDDiseasesBind", codes.Substring(0, codes.Length - 1));
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["ICDCode"] = ds.Tables[0].Rows[i]["ICDCode"].ToString();
                            dr["Description"] = ds.Tables[0].Rows[i]["Description"].ToString();
                            dr["ICDID"] = ds.Tables[0].Rows[i]["ICDID"].ToString();
                            dt.Rows.Add(dr);
                            //codes += dt.Rows[i][2].ToString() + ",";
                        }
                    }
                }
                if (txtDiagnosis.Text.Trim() != "")
                {
                    if (ReturnDescription(txtDiagnosis.Text.Trim()))
                    {
                        DataRow dr = dr = dt.NewRow();
                        dr["ICDCode"] = "0";
                        dr["Description"] = txtDiagnosis.Text.Trim();
                        dr["ICDID"] = "0";
                        dt.Rows.Add(dr);
                    }
                }
                GridView2.DataSource = dt;
                GridView2.DataBind();
                btnOK.Visible = true;
                txtDiagnosis.Text = "";
            }
            else if (txtDiagnosis.Text.Trim() != "")
            {
                if (ReturnDescription(txtDiagnosis.Text.Trim()))
                {
                    DataRow dr = dt.NewRow();
                    dr["ICDCode"] = "0";
                    dr["Description"] = txtDiagnosis.Text.Trim();
                    dr["ICDID"] = "0";
                    dt.Rows.Add(dr);
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                    btnOK.Visible = true;
                    txtDiagnosis.Text = "";
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Please enter Diagnosis or select atleast one from list.", Page);
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public bool ReturnDescription(string Des)
    {
        if (dt != null)
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() == "0")
                {
                    if (Des == dr[1].ToString())
                    {
                        return false;
                    }
                }
            }

        if (dt == null)
            return true;
        return true;
    }

    public string ReturnICDCodes(string str)
    {
        string s = "";
        string[] cdesId = str.Substring(0, str.Length - 1).Split(new char[] { ',' });
        string temp = "";
        int i = 0;
        foreach (string sEx in cdesId)
        {
            temp = "";
            if (dt != null)
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[0].ToString() != "0")
                    {
                        if (sEx == dr[2].ToString())
                        {
                            temp = "";
                            break;
                        }
                        else
                            temp = sEx;
                    }
                    else
                        i++;
                }
            if (temp != "")
                s += temp + ",";
        }
        if (dt != null)
        {
            if (dt.Rows.Count == 0)
                s = str;
        }
        if (dt == null)
            s = str;
        if (dt != null)
        {
            if (i == dt.Rows.Count)
                s = str;
        }
        return s;
    }

    public void CreateTable(DataTable dt)
    {
        DataColumn dcIcdCode = new DataColumn("ICDCode");
        DataColumn dcDescription = new DataColumn("Description");
        DataColumn dcIcdId = new DataColumn("ICDID");
        dt.Columns.Add(dcIcdCode);
        dt.Columns.Add(dcDescription);
        dt.Columns.Add(dcIcdId);
    }

    protected void LstCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnStatus.Value = "CurrentAssessment";
        Session["ICDcode"] = "";
        fun.BindDataSetToGrid(GVDisease, "DiseasesBind", LstCategory.SelectedValue);
        showStatus(LstCategory.SelectedItem.Text, GVDisease);
    }

    protected void GVDisease_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveGridCheckedItems(GVDisease, "chkInner", "hiddenICDCode");
        GVDisease.PageIndex = e.NewPageIndex;
        BindSearchData();
        SetGridCheckedItems(GVDisease, "chkInner");
    }

    protected void SearchProblem(object sender, EventArgs e)
    {
        BindSearchData();
    }

    public void BindSearchData()
    {
        try
        {
            StringBuilder str = new StringBuilder();
            if (hdnStatus.Value == "CurrentAssessment")
            {
                if (ddlSortBy.SelectedValue == "2")
                {
                    if (LstCategory.SelectedValue != "0")
                    {
                        str.Append(" SELECT    ICDCode, Description,ICDID ");
                        str.Append(" FROM  MRDICDDisease");
                        str.Append(" WHERE  (CategoryId = " + LstCategory.SelectedValue.ToString() + ") AND (Active = 1) AND (Description LIKE '%" + txtKeywords.Text.Trim() + "%')");
                        str.Append(" ORDER BY ICDCode");
                        DataSet ds = fun.ExecuteSql(str);
                        MakeAddButtonVisibleFalseTrue(ds);
                        GVDisease.DataSource = ds;
                        GVDisease.DataBind();
                        showStatus(LstCategory.SelectedItem.Text, GVDisease);
                    }
                    else
                    {
                        str.Append(" SELECT    ICDCode, Description,ICDID ");
                        str.Append(" FROM  MRDICDDisease");
                        str.Append(" WHERE  (Active = 1) AND (Description LIKE '%" + txtKeywords.Text.Trim() + "%')");
                        str.Append(" ORDER BY ICDCode");
                        DataSet ds = fun.ExecuteSql(str);
                        MakeAddButtonVisibleFalseTrue(ds);
                        GVDisease.DataSource = ds;
                        GVDisease.DataBind();
                        showStatus(LstCategory.SelectedItem.Text, GVDisease);
                    }
                }
                else
                {
                    if (LstCategory.SelectedValue != "0")
                    {
                        str.Append(" SELECT    ICDCode, Description,ICDID ");
                        str.Append(" FROM  MRDICDDisease");
                        str.Append(" WHERE  (CategoryId = " + LstCategory.SelectedValue.ToString() + ") AND (Active = 1) AND (ICDCode LIKE '" + txtKeywords.Text.Trim() + "%')");
                        str.Append(" ORDER BY ICDCode");
                        DataSet ds = fun.ExecuteSql(str);
                        MakeAddButtonVisibleFalseTrue(ds);
                        GVDisease.DataSource = ds;
                        GVDisease.DataBind();
                        showStatus(LstCategory.SelectedItem.Text, GVDisease);
                    }
                    else
                    {
                        str.Append(" SELECT    ICDCode, Description,ICDID ");
                        str.Append(" FROM  MRDICDDisease");
                        str.Append(" WHERE  (Active = 1) AND (ICDCode LIKE '" + txtKeywords.Text.Trim() + "%')");
                        str.Append(" ORDER BY ICDCode");
                        DataSet ds = fun.ExecuteSql(str);
                        MakeAddButtonVisibleFalseTrue(ds);
                        GVDisease.DataSource = ds;
                        GVDisease.DataBind();
                        showStatus(LstCategory.SelectedItem.Text, GVDisease);
                    }
                }
            }
            else if (hdnStatus.Value == "My Favorites")
            {
                bl.EncodedBy = Convert.ToInt16(Session["UserID"]);
                if (ddlSortBy.SelectedValue == "2")
                {
                    str.Append("SELECT md.ICDCode, md.Description, md.ICDID ");
                    str.Append(" FROM MRDICDDisease AS md INNER JOIN ");
                    str.Append(" EMRFavouriteDiagnosis AS fv ON md.ICDID = fv.ICDID ");
                    str.Append(" WHERE (md.Active = 1) AND (fv.DoctorID = " + bl.EncodedBy + ") AND (fv.Active = 1) AND (md.Description LIKE '%" + txtKeywords.Text.Trim() + "%')");
                    str.Append(" ORDER BY ICDCode");
                    DataSet ds = fun.ExecuteSql(str);
                    MakeAddButtonVisibleFalseTrue(ds);
                    GVDisease.DataSource = ds;
                    GVDisease.DataBind();
                    showStatus("My Favorites", GVDisease);
                }
                else
                {
                    str.Append("SELECT md.ICDCode, md.Description, md.ICDID ");
                    str.Append(" FROM MRDICDDisease AS md INNER JOIN ");
                    str.Append(" EMRFavouriteDiagnosis AS fv ON md.ICDID = fv.ICDID ");
                    str.Append(" WHERE (md.Active = 1) AND (fv.DoctorID = " + bl.EncodedBy + ") AND (fv.Active = 1) AND (md.ICDCode LIKE '" + txtKeywords.Text.Trim() + "%')");
                    str.Append(" ORDER BY ICDCode");
                    DataSet ds = fun.ExecuteSql(str);
                    MakeAddButtonVisibleFalseTrue(ds);
                    GVDisease.DataSource = ds;
                    GVDisease.DataBind();
                    showStatus("My Favorites", GVDisease);
                }
            }
            else if (hdnStatus.Value == "PrevAssessment")
            {
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void MakeAddButtonVisibleFalseTrue(DataSet ds)
    {
        if (ds.Tables[0].Rows.Count == 0)
            btnSubmit.Visible = false;
        else
            btnSubmit.Visible = true;
    }

    protected void GVDisease_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chk = (CheckBox)e.Row.FindControl("chkInner");
            TableRow r = e.Row as TableRow;
            Label lblIcdCode = e.Row.FindControl("lblICDCode") as Label;
            lblIcdCode.Attributes.Add("onclick", "if($get('" + chk.ClientID + "').checked)$get('" + chk.ClientID + "').checked=false; else $get('" + chk.ClientID + "').checked=true;");
        }
    }

    protected void chkHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox hchk = sender as CheckBox;
        int pageSize = GVDisease.PageSize;
        if (pageSize > GVDisease.Rows.Count)
            pageSize = GVDisease.Rows.Count;
        for (int i = 0; i < pageSize; i++)
        {
            CheckBox chk = GVDisease.Rows[i].FindControl("chkInner") as CheckBox;
            chk.Checked = hchk.Checked;
            if (chk.Checked)
            {
                GridViewRow row = (GridViewRow)chk.NamingContainer;
                HiddenField hdn1 = row.FindControl("hiddenICDCode") as HiddenField;
                if (Session["ICDcode"] != null)
                    Session["ICDcode"] += hdn1.Value + ",";
            }
        }
    }

    protected void chkInner_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        if (chk.Checked)
        {
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            HiddenField hdn1 = row.FindControl("hiddenICDCode") as HiddenField;
            if (Session["ICDcode"] != null)
                Session["ICDcode"] += hdn1.Value + ",";
        }
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSno = e.Row.FindControl("lblSno") as Label;
            lblSno.Text = (e.Row.RowIndex + 1).ToString();
            Label lblICDCode = e.Row.FindControl("lblICDCode") as Label;
            if (lblICDCode.Text == "0")
                lblICDCode.Text = "&nbsp;";
            else
            {
                ImageButton ibtn = e.Row.FindControl("ibtnEdit") as ImageButton;
                ibtn.Visible = false;
            }
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton imgBtn = sender as ImageButton;
            GridViewRow row = imgBtn.NamingContainer as GridViewRow;
            Label lblSno = row.FindControl("lblSno") as Label;
            int index = int.Parse(lblSno.Text) - 1;
            dt.Rows.Remove(dt.Rows[index]);
            GridView2.DataSource = dt;
            GridView2.DataBind();
            if (dt.Rows.Count == 0)
                btnOK.Visible = false;
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkFavorities_Click(object sender, EventArgs e)
    {
        try
        {
            hdnStatus.Value = "My Favorites";
            bl.EncodedBy = Convert.ToInt16(Session["UserID"]);
            fun.BindDataSetToGrid(GVDisease, "UspEMRGetFavorities", bl.EncodedBy);
            if (GVDisease.Rows.Count == 0)
                btnSubmit.Visible = false;
            showStatus("My Favorites", GVDisease);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkPrevAssess_Click(object sender, EventArgs e)
    {
        hdnStatus.Value = "Previous Assessment";
        bl.EncodedBy = Convert.ToInt16(Session["UserID"]);
        showStatus("Previous Assessment", GVDisease);
    }

    public void showStatus(string text, GridView gd)
    {
        if (gd.Rows.Count != 0)
        {
            Label lblStat = gd.HeaderRow.FindControl("lblStat") as Label;
            lblStat.Text = "[" + text + "]&nbsp;";
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["frm"] != null)
                {
                    if (Convert.ToString(Request.QueryString["frm"]) == "Diag")
                    {
                        string ICDCds = string.Empty;
                        ArrayList Ar_DiagnosisList = new ArrayList();
                        foreach (GridViewRow row in GridView2.Rows)
                        {
                            HiddenField hdnICDID = row.FindControl("hdnICDID") as HiddenField;
                            if (hdnICDID.Value == "0")
                            {
                                Label lblDescription = row.FindControl("lblDescription") as Label;
                                if (lblDescription == null)
                                {
                                    Alert.ShowAjaxMsg("Record can not be submit as it is in edit mode.Click on cancel or update button first", Page);
                                    return;
                                }
                                Ar_DiagnosisList.Add(lblDescription.Text);
                            }
                            else
                            {
                                //ICDCds += hdnICDID.Value + ",";
                                Label lblIcdCodes = row.FindControl("lblICDCode") as Label;
                                ICDCds += lblIcdCodes.Text.ToString().Trim() + ",";
                                //ICDCds += "'" + lblIcdCodes.Text.ToString().Trim() + "',";
                            }
                        }
                        Session["DiangnosisLst"] = Ar_DiagnosisList;
                        Session["icdCodes"] = ICDCds;
                        if (Convert.ToString(Request.QueryString["frm"]).Trim().ToUpper() == "CHR")
                        {
                        }
                        else if (Convert.ToString(Request.QueryString["frm"]).Trim().ToUpper() == "TDY")
                        {
                        }
                        ScriptManager.RegisterStartupScript(Page, GetType(), "sc1", "__addKeyword();", true);
                    }
                }
                else
                {
                    StringBuilder scriptString = new StringBuilder();
                    StringBuilder sResult = new StringBuilder();
                    foreach (GridViewRow gvr in GridView2.Rows)
                    {
                        Label lbl = (Label)gvr.Cells[1].FindControl("lblICDCode");

                        //string sICDID = gvr.Cells[1].Text.ToString();

                        if (sResult.ToString() == "")
                        {
                            sResult.Append(lbl.Text.ToString().Trim());
                        }
                        else
                        {
                            sResult.Append(",");
                            sResult.Append(lbl.Text.ToString().Trim());
                        }

                    }
                    if (sResult.ToString() != "")
                    {
                        scriptString.Append("window.opener.document.getElementById('" + Request.QueryString[0] + "').value = '" + sResult.ToString() + "';window.opener.document.forms(0).submit();window.close();");
                        //scriptString.Append("window.opener.__doPostBack('" + Request.QueryString[0] + ".value', ''); self.close();");

                        ScriptManager.RegisterStartupScript(Page, GetType(), "scr1", scriptString.ToString(), true);
                    }
                }
            }
            else
            {
                string ICDCds = string.Empty;
                ArrayList Ar_DiagnosisList = new ArrayList();
                foreach (GridViewRow row in GridView2.Rows)
                {
                    HiddenField hdnICDID = row.FindControl("hdnICDID") as HiddenField;
                    if (hdnICDID.Value == "0")
                    {
                        Label lblDescription = row.FindControl("lblDescription") as Label;
                        if (lblDescription == null)
                        {
                            Alert.ShowAjaxMsg("Record can not be submit as it is in edit mode.Click on cancel or update button first", Page);
                            return;
                        }
                        Ar_DiagnosisList.Add(lblDescription.Text);
                    }
                    else
                        ICDCds += hdnICDID.Value + ",";
                }
                Session["DiangnosisLst"] = Ar_DiagnosisList;
                Session["icdCodes"] = ICDCds;
                ScriptManager.RegisterStartupScript(Page, GetType(), "sc1", "__addKeyword();", true);
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView2.EditIndex = e.NewEditIndex;
        GridView2.DataSource = dt;
        GridView2.DataBind();
    }

    protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView2.EditIndex = -1;
        GridView2.DataSource = dt;
        GridView2.DataBind();
    }

    protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int index = e.RowIndex;
            TextBox txtDescription = GridView2.Rows[index].FindControl("txtDescription") as TextBox;
            dt.Rows[index]["Description"] = txtDescription.Text;
            GridView2.EditIndex = -1;
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}

