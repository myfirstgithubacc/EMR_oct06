using System;
using System.Text;
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
using System.Data.SqlClient;

public partial class EMR_Templates_SentanceGallery : System.Web.UI.Page
{
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    hdControlId.Value = Request.QueryString["ID"].ToString().Trim();
                }
                if (Convert.ToString(Request.QueryString["ControlType"]) != null)
                {
                    hdControlType.Value = Request.QueryString["ControlType"].ToString().Trim();
                }

                if (Request.QueryString["LabId"] != null)
                {
                    ViewState["LabFieldId"] = Request.QueryString["LabId"].ToString().Trim();
                }


                ViewState["EditMode"] = "No";
                ViewState["Gallery"] = "Gallery";
                BindSentenceGalleryGridView();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindSentenceGalleryGridView()
    {
        BaseC.EMR emr = new BaseC.EMR(sConString);
        ds = new DataSet();
        try
        {
            if (common.myStr(Request["Lab"]) == "Lab")
            {
                ds = emr.BindSentenceGalleryGridView(common.myInt(Request.QueryString["TemplateFieldId"]),
                    common.myInt(Request.QueryString["SectionId"]), common.myInt(Session["HospitalLocationId"]),
                    common.myInt(Session["FacilityId"]), "L", common.myInt(ViewState["LabFieldId"]));
            }
            else
            {
                ds = emr.BindSentenceGalleryGridView(common.myInt(Request.QueryString["TemplateFieldId"]),
                   common.myInt(Request.QueryString["SectionId"]), common.myInt(Session["HospitalLocationId"]),
                   common.myInt(Session["FacilityId"]), "", 0);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSentenceGallery.DataSource = ds;
                gvSentenceGallery.DataBind();
            }
            else
            {
                gvSentenceGallery.DataSource = BindBlankGrid();
                gvSentenceGallery.DataBind();
            }
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
            emr = null;
        }
    }
    private DataTable BindBlankGrid()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("Id");
            dt.Columns.Add("SentenceId");

            dt.Columns.Add("Sentence");
            DataRow dr = dt.NewRow();
            dr["Id"] = 0;
            dr["SentenceId"] = 0;
            dr["Sentence"] = "";
            dt.Rows.Add(dt);
            return dt;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return dt;
        }
        finally
        {
            dt.Dispose();
        }

    }
    protected void gvSentenceGallery_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            ViewState["Gallery"] = "Gallery";
            gvSentenceGallery.PageIndex = e.NewPageIndex;
            BindSentenceGalleryGridView();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //ViewState["Gallery"] = "Gallery";
        BindSentenceGalleryGridView();
    }

    protected void btnAddSentence_Click(object sender, EventArgs e)
    {
        BaseC.EMR emr = new BaseC.EMR(sConString);
        Hashtable hshOut = new Hashtable();
        try
        {
            if (txtSentence.Text != "")
            {
                if (ViewState["Id"] == null)
                {
                    hshOut = emr.SaveDeleteSentenceGallery("S", 0, txtSentence.Text, common.myInt(Request.QueryString["SectionId"]), common.myInt(Request.QueryString["TemplateFieldId"]),
                            common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), common.myInt(ViewState["LabFieldId"]));
                }
                else
                {
                    hshOut = emr.SaveDeleteSentenceGallery("U", common.myInt(ViewState["Id"]), txtSentence.Text, common.myInt(Request.QueryString["SectionId"]), common.myInt(Request.QueryString["TemplateFieldId"]),
                            common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), common.myInt(ViewState["LabFieldId"]));
                }

                lblMessage.Text = hshOut["@chvErrorOutput"].ToString();
            }
            else
            {
                Alert.ShowAjaxMsg("Please enter Sentence.", Page);
            }
            BindSentenceGalleryGridView();
            txtSentence.Text = "";
            btnAddSentence.Text = "Add New Sentence";
            ViewState["Id"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            emr = null;
            hshOut = null;
        }
    }

    protected void lnkSentence_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkSentence = sender as LinkButton;
            GridViewRow row = lnkSentence.NamingContainer as GridViewRow;
            HiddenField hdnSentenceId = row.FindControl("hdnSentenceId") as HiddenField;
            string ctrlId = "";
            string jQuery = "";
            if (Request.QueryString["ctrlId"] != null)
                ctrlId = Request.QueryString["ctrlId"].ToString();
            if (Request.QueryString["typ"] != null)
            {
                if (Convert.ToInt16(Request.QueryString["typ"]) == 1)//TextBoxBox
                {
                    Int16 MaxTxt = Convert.ToInt16(Request.QueryString["Mx"]);
                    if (lnkSentence.Text.Length > MaxTxt)
                        jQuery = "window.opener.document.getElementById('" + ctrlId + "').value='" + lnkSentence.Text.Substring(0, MaxTxt) + "';";
                    else
                        jQuery = "window.opener.document.getElementById('" + ctrlId + "').value='" + lnkSentence.Text.Trim() + "';";
                }
                else if (Convert.ToInt16(Request.QueryString["typ"]) == 0) //MultiTextBox
                    jQuery = "window.opener.document.getElementById('" + ctrlId + "').value=window.opener.document.getElementById('" + ctrlId + "').value + ' ' + '" + lnkSentence.Text.Trim() + "';";
                else if (Convert.ToInt16(Request.QueryString["typ"]) == 2)
                { //RichTextBoxdocument.frames[RichTextFrame].document.body.innerText
                    jQuery = "alert(window.opener.document.frames['" + ctrlId + "'].document.body.innerHTML);";//=window.opener.document.getElementById('" + ctrlId + "').innerHTML + ' ' + '" + lnkSentence.Text.Trim() + "';";
                    //+ " window.opener.document.forms(0).submit();";
                }

            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "script1", jQuery, true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvSentenceGallery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    HiddenField hdnSentenceId = e.Row.FindControl("hdnSentenceId") as HiddenField;
        //    if (hdnSentenceId.Value == "0")
        //        e.Row.Visible = false;

        //    //ImageButton ibtnCopy = e.Row.FindControl("ibtnCopy") as ImageButton;
        //    //LinkButton lnkCopy = e.Row.FindControl("lnkCopy") as LinkButton;
        //    //TextBox lblSentence = e.Row.FindControl("lblSentence") as TextBox;
        //    //ibtnCopy.Attributes.Add("onclick", "return CopyToClipboard('" + lblSentence.ClientID + "', '" + lblSentence.Text + "');");
        //    //lnkCopy.Attributes.Add("onclick", "return CopyToClipboard('" + lblSentence.ClientID + "', '" + lblSentence.Text + "');");

        //    ImageButton img = (ImageButton)e.Row.FindControl("ibtnDelete");
        //    string jQuery = "var color=$get('" + e.Row.ClientID + "').style.backgroundColor;"
        //        + "$get('" + e.Row.ClientID + "').style.backgroundColor='Pink';"
        //        + "if(!confirm('Are you sure you want to De-Activate this Sentence?'))"
        //        + "{$get('" + e.Row.ClientID + "').style.backgroundColor=color; return false;}";
        //    img.Attributes.Add("onclick", jQuery);
        //    //e.Row.Attributes.Add("onclick", "clickButton('" + lnkSentence.ClientID + "')");
        //    //txtSentence.Text = txtSentence.Text + lblSentence.Text;

        //}

        //if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.Cells[4].Visible = false;
        //    if (ViewState["Gallery"].ToString() == "Favorites")
        //    {
        //        if (e.Row.RowType == DataControlRowType.Header)
        //        {
        //            e.Row.Cells[1].Text = "Favorite Sentence(s)";
        //        }
        //        e.Row.Cells[0].Visible = false;
        //        //btnAddToFavorites.Visible = false;
        //    }
        //    else
        //    {
        //        if (e.Row.RowType == DataControlRowType.Header)
        //        {
        //            e.Row.Cells[1].Text = "Sentence(s)";
        //        }
        //        //btnAddToFavorites.Visible = true;
        //    }
        //}

    }
    protected void btnStore_OnClick(object sender, EventArgs e)
    {
        Session["rtfText"] = txtSentence.Text.ToString();
    }

    protected void lnkEdit_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["EditMode"] = "Yes";
            GridViewRow Grow = ((LinkButton)sender).NamingContainer as GridViewRow;
            TextBox lblSentence = Grow.FindControl("lblSentence") as TextBox;
            HiddenField hdnSentenceId = Grow.FindControl("hdnSentenceId") as HiddenField;
            txtSentence.Text = lblSentence.Text;
            btnAddSentence.Text = "Update Sentence";
            btnAddSentence.ToolTip = "Update Sentence";
            ViewState["Id"] = hdnSentenceId.Value;
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script12", "$get('" + Grow.ClientID + "').style.backgroundColor='pink';", true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ibtnDelete_OnClick(object sender, EventArgs e)
    {
        ImageButton ibtnDelete = sender as ImageButton;
        BaseC.EMR emr = new BaseC.EMR(sConString);
        int iSentenceId = common.myInt(ibtnDelete.CommandArgument);
        Hashtable hshOut = new Hashtable();
        try
        {
            if (iSentenceId != 0)
            {
                hshOut = emr.SaveDeleteSentenceGallery("D", iSentenceId, "", common.myInt(Request.QueryString["SectionId"]), common.myInt(Request.QueryString["TemplateFieldId"]),
                        common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), common.myInt(ViewState["LabFieldId"]));
                lblMessage.Text = hshOut["@chvErrorOutput"].ToString();
            }
            else
            {
                Alert.ShowAjaxMsg("Please enter Sentence.", Page);
            }
            BindSentenceGalleryGridView();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            emr = null;
            hshOut = null;
            ibtnDelete.Dispose();
            iSentenceId = 0;
        }
    }

    protected void btnAddToFavorites_Click(object sender, EventArgs e)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Int16 count = 0;

        Hashtable ht = new Hashtable();
        HiddenField hdnSentenceId = new HiddenField();
        try
        {


            foreach (GridViewRow Grow in gvSentenceGallery.Rows)
            {
                if (((CheckBox)Grow.FindControl("chkInner")).Checked)
                {
                    count = 1;
                    hdnSentenceId = Grow.FindControl("hdnSentenceId") as HiddenField;
                    sb.Append("<Table1><c1>" + hdnSentenceId.Value + "</c1></Table1>");
                    //break;               
                }
            }
            if (count == 1)
            {

                //Hashtable hshOut = new Hashtable();
                //hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ht.Add("@xmlIds", sb.ToString());
                //ht.Add("@intSentenceId", hdnSentenceId.Value);
                ht.Add("@intDoctorId", ViewState["DocId"]);
                ht.Add("@intEncodedBy", Session["UserId"]);

                // int temp = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRSaveFavSentence", ht);
                lblMessage.Text = "Sentence(s) have been added to Favorites.";
            }
            else
                lblMessage.Text = "Please select Sentence(s) first to add to favorites.";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            sb = null;
            ht = null;
            hdnSentenceId = null;
        }
    }

    protected void lnkShowAll_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["Gallery"] = "Gallery";
            BindSentenceGalleryGridView();
            lblMessage.Text = "";
            // lnkShowFavorites.Visible = true;
            // lnkShowAll.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkShowFavorites_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["Gallery"] = "Favorites";
            BindSentenceGalleryGridView();
            lblMessage.Text = "";
            //lnkShowFavorites.Visible = false;
            // lnkShowAll.Visible = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected DataTable GetEmptyData()
    {
        DataTable dt = new DataTable();
        DataColumn dcId = new DataColumn("Id");
        DataColumn dcSentence = new DataColumn("Sentence");
        try
        {
            dt.Columns.Add(dcId);
            dt.Columns.Add(dcSentence);

            DataRow dr = dt.NewRow();
            dr["Id"] = 0;
            dr["Sentence"] = "";
            dt.Rows.Add(dr);
            return dt;
        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return dt;
        }
        finally
        {
            dt.Dispose();
            dcId.Dispose();
            dcSentence.Dispose();
        }

    }
    protected void gvSentenceGallery_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        BaseC.EMR emr = new BaseC.EMR(sConString);
        HiddenField hdSenId = gvSentenceGallery.Rows[e.RowIndex].Cells[1].FindControl("hdnSentenceId") as HiddenField;
        TextBox txtSen = gvSentenceGallery.Rows[e.RowIndex].Cells[1].FindControl("lblSentence") as TextBox;
        Hashtable hshOut = new Hashtable();
        try
        {
            if (hdSenId.Value != "0" && hdSenId.Value != "")
            {
                hshOut = emr.SaveDeleteSentenceGallery("U", common.myInt(hdSenId.Value), txtSen.Text, common.myInt(Request.QueryString["SectionId"]), common.myInt(Request.QueryString["TemplateFieldId"]),
                        common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), common.myInt(ViewState["LabFieldId"]));
                lblMessage.Text = hshOut["@chvErrorOutput"].ToString();
            }
            else
            {
                Alert.ShowAjaxMsg("Please enter Sentence.", Page);
            }
            BindSentenceGalleryGridView();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            gvSentenceGallery.EditIndex = -1;
            emr = null;
            hshOut = null;
            hdSenId = null;
            txtSen = null;
        }

    }
    protected void gvSentenceGallery_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Copy")
            {
                //int index = common.myInt(e.CommandArgument);
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnSentenceId = (HiddenField)row.FindControl("hdnSentenceId");
                TextBox lblCopySentence = (TextBox)row.FindControl("lblSentence");
                if (common.myStr(txtSentence.Text).Equals(string.Empty))
                {
                    txtSentence.Text = txtSentence.Text + " " + lblCopySentence.Text;
                }
                else
                {
                    txtSentence.Text = txtSentence.Text + " , " + lblCopySentence.Text;
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
}
