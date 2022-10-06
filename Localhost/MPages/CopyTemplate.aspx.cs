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
using System.Data.SqlClient;

public partial class MPages_CopyTemplate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.ParseData bc = new BaseC.ParseData();
    StringBuilder strSQL;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshIn;
    private Hashtable hshOut;
    DAL.DAL dl;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                Session["GridCheckedItems"] = null;
                Session["GridCheckedIds"] = null;
                //gvTemplates.Columns[5].Visible = false;
                if (Request.QueryString["Mpg"] != null)
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                BindTemplateGrid();
                populateTemplateControls();

                /* Binding Specilisation */
                DL_Funs fun = new DL_Funs();
                StringBuilder sb = new StringBuilder();
                sb.Append("Select Id, Name from SpecialisationMaster Where Active = 1 Order By Name");
                DataSet dsSpecialisation = new DataSet();
                if (Cache["Specialisation"] != null)
                    dsSpecialisation = Cache["Specialisation"] as DataSet;
                else
                {
                    dsSpecialisation = fun.ExecuteSql(sb);
                    Cache.Insert("Specialisation", dsSpecialisation, null, System.DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                ddlSpecialisation.DataSource = dsSpecialisation;
                ddlSpecialisation.DataBind();
                /* Binding Specilisation */

                //   BindHospitalLocationGrid();
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = ex.Message;
            }
        }
        gvTemplates.Columns[0].Visible = false;
        gvTemplates.Columns[4].Visible = false;
        gvTemplates.Columns[8].Visible = false;
    }

    private void populateTemplateControls()
    {
        try
        {
            ddlTemplateType.Items.Clear();
            strSQL = new StringBuilder();
            //hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //hshIn.Add("@HospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
            strSQL.Append("select ID, TypeName  from EMRTemplateTypes order by TypeName");
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString());
            if (dr.HasRows == true)
            {
                ddlTemplateType.DataSource = dr;
                ddlTemplateType.DataBind();
                ddlTemplateType.Items.Insert(0, " [ Select ] ");
                ddlTemplateType.Items[0].Value = "0";
            }
            else
            {
                ddlTemplateType.Items.Insert(0, " [ Select ] ");
                ddlTemplateType.Items[0].Value = "0";
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindTemplateGrid()
    {
        try
        {
            DataTable dtSpecializationTemplate = new DataTable();
            hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshIn.Add("@inyHospitalLocationID", 0);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetTemplate", hshIn);
            dtSpecializationTemplate = ds.Tables[1];
            DataView dv = new DataView(ds.Tables[0]);
           
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                ViewState["gvTemplatesDetails"] = dt;
                lblTotalCount.Text = "Total Templates : " + common.myInt(dt.Rows.Count);
                gvTemplates.DataSource = dt;
                gvTemplates.DataBind();
                ViewState["gvTemplatesDetailsPageIndeexChange"] = dt;
            }
            if(dtSpecializationTemplate.Rows.Count >0)
            {
                gvSpecializationTemplate.DataSource = dtSpecializationTemplate;
                gvSpecializationTemplate.DataBind();
            }

            

        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    //private void BindHospitalLocationGrid()
    //{
    //    string strSQL = "";
    //    hshIn = new Hashtable();
    //    dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
    //    strSQL = "select Id, Name, ISNULL(NPI,'') NPI, ISNULL(EIN,'') EIN, ISNULL(BillingAddress, '') BillingAddress from HospitalLocation where Id != @inyHospitalLocationID and Active=1";
    //    DataSet ds = dl.FillDataSet(CommandType.Text, strSQL, hshIn);
    //    gvHospitalLocation.DataSource = ds;
    //    gvHospitalLocation.DataBind();        
    //}

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable ht = new Hashtable();
            Hashtable ho = new Hashtable();
            String strTemplateXML = "";
            //  String strHospitalLocationXML = "";
            int count = 0;
            if (Session["GridCheckedIds"] == null)
            {
                for (int i = 0; i < gvTemplates.Rows.Count; i++)
                {
                    CheckBox chkT = (CheckBox)gvTemplates.Rows[i].FindControl("chkT");
                    if (chkT.Checked == true)
                    {
                        count = 1;
                        strTemplateXML = strTemplateXML + "<Table1><c1>" + gvTemplates.Rows[i].Cells[1].Text + "</c1></Table1>";
                    }
                }
            }
            else
            {
                ArrayList arr = Session["GridCheckedIds"] as ArrayList;
                for (int i = 0; i <= arr.Count-1; i++)
                {
                    CheckBox chkT = (CheckBox)gvTemplates.Rows[i].FindControl("chkT");
                    if (chkT.Checked == true)
                    {
                        count = 1;
                        strTemplateXML = strTemplateXML + "<Table1><c1>" + arr[i] + "</c1></Table1>";
                    }
                }
            }
            if (count == 0)
            {
                Alert.ShowAjaxMsg("Please Select Atleast one Template to Copy", Page);

                return;
            }
            // count = 0;
            //for (int i = 0; i < gvHospitalLocation.Rows.Count; i++)
            //{                
            //   CheckBox chkH = (CheckBox)gvHospitalLocation.Rows[i].FindControl("chkH");
            //  if (chkH.Checked == true)
            // {
            //    count = 1;
            //   strHospitalLocationXML = strHospitalLocationXML + "<Table1><c1>" + gvHospitalLocation.Rows[i].Cells[1].Text + "</c1></Table1>";
            //}
            //}
            //if (count == 0)
            //{
            //  Alert.ShowAjaxMsg("Please Select Atleast one Hospital Location to Copy", Page);
            // return;
            //}
            ht.Add("@intHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
            ht.Add("@intEncodedBy", Session["UserID"].ToString());
            ht.Add("@xmlTemplateIds", strTemplateXML);
            // ht.Add("@xmlHospitalLocationIDs", strHospitalLocationXML);
            ho.Add("chvErrorStatus", SqlDbType.VarChar);
            ho = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspCopyEMRTemplates", ht, ho);
            lbl_Msg.Text = "Template(s) copied successfully.";
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvTemplates_OnRowDataBound(object o, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAllItems")).Attributes.Add("onclick", "javascript:SelectAll('" +
                    ((CheckBox)e.Row.FindControl("chkAllItems")).ClientID + "','" + gvTemplates.ClientID + "')");

        }
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[7].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if (common.myStr(Session["RegistrationNo"]) == string.Empty)
            //{
            //    Session["RegistrationNo"] = 140001;

            //  }

            //LinkButton lbtndetails = (LinkButton)e.Row.FindControl("lbtndetails");
            //  lbtndetails.Attributes.Add("onclick", "javascript:openRadWindow('" + e.Row.Cells[1].Text + "', '" + e.Row.Cells[7].Text + "'); return false;");


            //  lbtndetails.Attributes.Add("onclick", "javascript:openRadWindow('" + "/EMR/Templates/TemplateLibraryView.aspx?DisplayMenu=1&CopyTemplate=1&IsEMRPopUp=1&TemplateId=" + e.Row.Cells[1].Text +"" + "'); return false;");
            //   lbtndetails.Attributes.Add("onclick", "javascript:openRadWindow('" + "/EMR/Templates/TemplateLibraryView.aspx" + "'); return false;");


            // Response.Redirect("/EMR/Templates/TemplateLibraryView.aspx?DisplayMenu=1&CopyTemplate=1&IsEMRPopUp=1&TemplateId=" + e.Row.Cells[1].Text);


        }
    }
    protected void lbtndetails_OnClik(object sender, EventArgs e)  //For Details
    {

        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        LinkButton lbtndetails = (LinkButton)row.FindControl("lbtndetails");
        Label lblTemplateName = (Label)row.FindControl("lblTemplateName");
        string TemplateId = row.Cells[1].Text;
        //HiddenField hdnTemplateName = (HiddenField)row.FindControl("hdnTemplateName");

        if (!common.myStr(TemplateId).Equals(string.Empty))
        {
            try
            {
                //EMR / Templates / Default.aspx ?IsEMRPopUp=1&DisplayMenu=1&TemplateId=5896&SingleScreenTemplateCode = HIS

                RadWindowForNew.NavigateUrl = "~/EMR/Templates/TemplateLibraryView.aspx?IsEMRPopUp=1&DisplayMenu=1&SingleScreenTemplateCode = HIS&TemplateId=" + row.Cells[1].Text;
                //  RadWindowForNew.NavigateUrl = "~/EMR/Templates/TemplateLibraryView.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + row.Cells[1].Text;

                RadWindowForNew.Height = 600;
                RadWindowForNew.Width = 1350;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                //RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
                RadWindowForNew.VisibleOnPageLoad = true;
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                Session["TemplateLibraryName"] = common.myStr(lblTemplateName.Text);

            }
            catch (Exception Ex)
            {
                lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lbl_Msg.Text = "Error: " + Ex.Message;

                objException.HandleException(Ex);
            }
        }
    }
    protected void gvTemplates_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            SaveGridCheckedItems(gvTemplates, "chkT", "hdnId");
            gvTemplates.PageIndex = e.NewPageIndex;
            // BindTemplateGrid();
            BindTemplateGridPageIndexChanged();
            SetGridCheckedItems(gvTemplates, "chkT");
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }
   private void BindTemplateGridPageIndexChanged()
    {
        DataTable dt = new DataTable();
        try
        {
           
           
            if (ViewState["gvTemplatesDetailsPageIndeexChange"] != null)
            {
                dt = (DataTable)ViewState["gvTemplatesDetailsPageIndeexChange"];
                if (dt.Rows.Count >0)
                {
                    gvTemplates.DataSource = dt;
                    gvTemplates.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();

        }

        //ViewState["gvTemplatesDetailsPageIndeexChange"] = dv.ToTable();

    }
    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable dtFinal = new DataTable();
        DataView dv = new DataView();
        try
        {
            dt = (DataTable)ViewState["gvTemplatesDetails"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dv = dt.DefaultView;

                    string s = common.myStr(ddlSpecialisation.SelectedValue); //"0" blank value
                    string TType = common.myStr(ddlTemplateType.SelectedValue); //"0" blank value
                    string TName = common.myStr(txtTemplate.Text); //"" blank value

                    if (s == "0" && TType == "0" && TName == string.Empty)
                    {
                        dtFinal = dt;
                    }
                    else if (s == "0" && TType == "0" && TName != string.Empty)
                    {
                        dv.RowFilter = "TemplateName LIKE '%" + TName + "%'";
                        dtFinal = dv.ToTable();
                    }
                    else if (s == "0" && TType != "0" && TName == string.Empty)
                    {
                        dv.RowFilter = "TemplateTypeID=" + TType;
                        dtFinal = dv.ToTable();
                    }
                    else if (s == "0" && TType != "0" && TName != string.Empty)
                    {
                        dv.RowFilter = "TemplateTypeID=" + TType + "AND TemplateName LIKE '%" + TName + "%'";
                        dtFinal = dv.ToTable();
                    }
                    else if (s != "0" && TType == "0" && TName == string.Empty)
                    {
                        dv.RowFilter = "SpecialisationId=" + s;
                        dtFinal = dv.ToTable();
                    }
                    else if (s != "0" && TType == "0" && TName != string.Empty)
                    {
                       // dv.RowFilter = "TemplateTypeID=" + TType;
                        dv.RowFilter = "SpecialisationId=" + s +  "AND TemplateName LIKE '%" + TName + "%'";
                        dtFinal = dv.ToTable();
                    }
                    else if (s != "0" && TType != "0" && TName == string.Empty)
                    {
                        dv.RowFilter = "SpecialisationId=" + s + "AND TemplateTypeID=" + TType;
                        dtFinal = dv.ToTable();
                    }
                    else if (s != "0" && TType != "0" && TName != string.Empty)
                    {
                        dv.RowFilter = "SpecialisationId=" + s + "AND TemplateTypeID=" + TType + "AND TemplateName LIKE '%" + TName + "%'";
                        dtFinal = dv.ToTable();
                    }



                    // dv.RowFilter = "SpecialisationId="+s+"AND TemplateTypeID="+TType+"AND TemplateName LIKE '%"+TName+"%'";



                    if (dtFinal.Rows.Count > 0)
                    {
                        gvTemplates.DataSource = dtFinal;
                        gvTemplates.DataBind();
                        ViewState["gvTemplatesDetailsPageIndeexChange"] = dtFinal;
                    }
                    else
                    {
                        BindBlankItemGrid();

                    }

                    lblTotalCount.Text = "Total Templates : " + common.myInt(dv.ToTable().Rows.Count);
                }
            }
        }
        catch (Exception Ex)
        {
            lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lbl_Msg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }


    }

    private void BindBlankItemGrid()
    {

        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();

        try
        {

            dr["TemplateID"] =0;
            dr["TemplateName"] = string.Empty;


            dr["Specialisation"] = string.Empty;

            dr["Code"] = string.Empty;

            dr["Type"] = string.Empty;
            dr["View"] = string.Empty;


            dr["TemplateTypeId"] = 0;
            dr["SpecialisationId"] =0;
          
          

            dt.Rows.Add(dr);
            dt.AcceptChanges();





            //ViewState["ItemDetail"] = null;
            gvTemplates.DataSource = dt;
            gvTemplates.DataBind();
            ViewState["gvTemplatesDetailsPageIndeexChange"] = dt;

        }

        catch (Exception ex)
        {


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



            dt.Columns.Add("TemplateID", typeof(int));

            dt.Columns.Add("TemplateName", typeof(string));

            dt.Columns.Add("Specialisation", typeof(string));

            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Type", typeof(string));


            dt.Columns.Add("View", typeof(string));
            dt.Columns.Add("TemplateTypeId", typeof(int));
            dt.Columns.Add("SpecialisationId", typeof(int));
            //dt.Columns.Add("ReplyBy", typeof(string));
            //dt.Columns.Add("Urgent", typeof(string));
            //dt.Columns.Add("ConcludeReferral", typeof(string));
            //dt.Columns.Add("EncodedId", typeof(int));
            //dt.Columns.Add("CompareId", typeof(int));

            return dt;
        }
        catch (Exception ex)
        {

            return dt;
        }

    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        ddlSpecialisation.SelectedIndex = 0;
        ddlTemplateType.SelectedIndex = 0;
        txtTemplate.Text = string.Empty;

    }

    protected void lnkTemplateCount_OnClik(object sender, EventArgs e)  //For View Other Notes
    {
      
        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
        DataTable dt = new DataTable();
       

        Label  lblSpecilization = (Label)row.FindControl("lblSpecilization");

        if (!common.myStr(lblSpecilization.Text).Equals(string.Empty))
        {
            try
            {
                gvTemplates.Visible = true;
                dt = (DataTable)ViewState["gvTemplatesDetails"];
                DataView dv = new DataView(dt);

                //dv.RowFilter = "PublicTemplate='True'";
             //   dv.RowFilter = "Specialisation like '%"+common.myStr(lblSpecilization.Text) +"%'";//         ='True'";
                dv.RowFilter = "Specialisation ='" + common.myStr(lblSpecilization.Text) + "'";//         ='True'";
                gvTemplates.DataSource =    dv.ToTable();
                gvTemplates.DataBind();
                lblTotalCount.Text = "Total Templates : " + common.myInt(dv.ToTable().Rows.Count);
                ViewState["gvTemplatesDetailsPageIndeexChange"] = dv.ToTable();

                //ifrmpage.Attributes["src"] = path + "Editor/WordProcessorTemplateBased.aspx?IsEMRPopUp=1&DisplayMenu=1&TemplateId=" + common.myInt(TemplateId) + "&CloseButtonShow=No";

                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('View Plan Of Care Details');", true);
                //imgbtnAddPOFTemplate.Focus();



            }
            catch (Exception Ex)
            {
                lbl_Msg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lbl_Msg.Text = "Error: " + Ex.Message;

                objException.HandleException(Ex);
            }
        }
    }

    


    //protected void gvHospitalLocation_OnRowDataBound(object o, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        //Find the checkbox control in header and add an attribute
    //        ((CheckBox)e.Row.FindControl("chkAllItems")).Attributes.Add("onclick", "javascript:SelectAll('" +
    //                ((CheckBox)e.Row.FindControl("chkAllItems")).ClientID + "','" + gvHospitalLocation.ClientID + "')");
    //    }
    //    if (e.Row.RowType != DataControlRowType.Pager)
    //    {
    //        e.Row.Cells[1].Visible = false;
    //    }
    //}

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
            int hdnId = common.myInt(gvTemplates.Rows[index].Cells[1].Text);
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
}
