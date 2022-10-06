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
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;

public partial class EMR_Vitals_VitalSignTemplate : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();

    //private enum GridSelectedVitals : byte
    //{
    //    VitalSignName = 0,
    //    SequenceNo = 1,
    //    Up = 2,
    //    Down = 3,
    //    InActive = 4,
    //    Active = 5
    //}

    private enum GridSelectedVitals : byte
    {
        TemplateID = 0,
        VitalID = 1,
        VitalName = 2,
        SequenceNo = 3,
        InActive = 4,
        Active = 5
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //Label sModuleName;
        ////UserControl UserControlTree = (UserControl)Master.FindControl("ucTree1");
        //sModuleName = (Label)Master.FindControl("sModuleName");
        //sModuleName.Text = "Clinical Masters";
        //DL_Funs fun = new DL_Funs();
        //TreeView Tree1 = (TreeView)Master.FindControl("tvCategory");
        //// fun.BindPagesFromOthers(5, Tree1);
        ////((Include_Master_EMRMaster)this.Master).BindPagesFromOthers(5);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ViewState["Count"] = null;
        if (!IsPostBack)
        {
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();

            FillTemplate();
            BindVitalsGrid();
            //getVitalTemplates();
            BindBlankSelectedVitalGrid();

            //ddlVitalTemplateSet_OnSelectedIndexChanged(sender, e);
        }
    }

    private void BindVitalsGrid()
    {
        try
        {
            BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
            DataSet ds = objEMRVitals.getAllVitals(Convert.ToInt16(Session["HospitalLocationID"]));

            gvVitals.DataSource = ds;
            gvVitals.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //private void getVitalTemplates()
    //{
    //    BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
    //    SqlDataReader dr = objEMRVitals.GetVitalTemplateName(Convert.ToInt16(Session["HospitalLocationID"]));
    //    if (dr.HasRows == true)
    //    {

    //    }
    //    else
    //    {
    //    }
    //}

    private void BindSelectedVitals()
    {
        try
        {
            hdnEditMode.Value = "";
            BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
            DataSet ds = objEMRVitals.GetVitalTemplateDetails(Convert.ToInt32(gvTemplate.SelectedRow.Cells[0].Text.Trim()), 0, 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["Count"] = "0";

                gvSelectedVitals.DataSource = ds.Tables[0];
                gvSelectedVitals.DataBind();
                ViewState["Count"] = null; ;

                hdnNew.Value = "False";
            }
            else
            {
                ViewState["Count"] = null;
                BindBlankSelectedVitalGrid();
                hdnNew.Value = "";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void FillTemplate()
    {
        try
        {
            // string sqlstr = "";
            if (Session["HospitalLocationId"] != null)
            {
                // sqlstr = "select Id,TemplateName,TemplateDescription,MeasurementSystem, Case MeasurementSystem when 'M' Then 'Metric' when 'E' Then 'English' end Measurement,DefaultTemplate,Case DefaultTemplate when 'True' Then 'Yes' when 'False' Then '' end DefaultValue from EMRVitalSignTemplate Where HospitalLocationId=" + Session["HospitalLocationId"];

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hs = new Hashtable();
                hs.Add("@inyHospitalLocationId", Session["HospitalLocationId"].ToString());
                DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "USPEMRGetVitalSiignTemplate", hs);
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    gvTemplate.DataSource = objDs;
                    gvTemplate.DataBind();
                }
                else
                {
                    BindBlankgvTemplate();
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

    protected void SaveVitalSignTemplate_OnClick(Object sender, EventArgs e)
    {
        try
        {
            StringBuilder strXML = new StringBuilder();
            if (txtVitalTemplateSet.Text.Trim() == "")
            {
                Alert.ShowAjaxMsg("Template name not found", Page);
                return;
            }

            foreach (GridViewRow gvr in gvSelectedVitals.Rows)
            {
                if (gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim() != "&nbsp;")
                {
                    String sVitalID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                    strXML.Append("<Table1><c1>");
                    strXML.Append(Convert.ToInt32(sVitalID.ToString().Trim()));
                    strXML.Append("</c1></Table1>");
                }
            }

            if (strXML.ToString().Trim().Length > 0)
            {
                //if (gvTemplate.SelectedIndex >-1)
                //{

                //int i = objEMRVitals.SaveVitalTemplate(, );
                string TemplateId = "0";
                if (gvTemplate.SelectedIndex > -1)
                {

                    TemplateId = gvTemplate.SelectedRow.Cells[0].Text.Trim();
                }

                BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
                String str = objEMRVitals.SaveVitalTemplateDetails(Convert.ToInt32(TemplateId), Convert.ToInt16(Session["UserID"]), strXML.ToString(), Convert.ToInt16(Session["HospitalLocationID"]), bc.ParseQ(txtVitalTemplateSet.Text.ToString().Trim()), bc.ParseQ(txtDescription.Text.ToString().Trim()), Convert.ToChar(ddlMeasurement.SelectedItem.Value), chkDefault.Checked);
                lblMessage.Text = str;

                //lblMessage.Text = "Record(s) Has Been Saved...";
                FillTemplate();
                //Alert.ShowAjaxMsg(str, this.Page);
                //ddlVitalTemplateSet_OnSelectedIndexChanged(sender, e);
                //}
                //else
                //{
                //    Alert.ShowAjaxMsg("Please Create A Vital Template First", Page);
                //    return;
                //}
            }
            else
            {
                Alert.ShowAjaxMsg("There Is No Selected Vital, Double Click To Add", Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("VitalSignTemplate.aspx", false);
    }

    protected void gvVitals_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
            e.Row.Cells[0].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.gvVitals, "Select$" + e.Row.RowIndex);
            // e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackClientHyperlink(this.gvVitals, "Select$" + e.Row.RowIndex) + ";javascript:GetVitalID('" + e.Row.Cells[0].Text.Replace("'", "''") + "');this.style.font='bold 10pt helvetica';this.style.color = '#890105';");
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackClientHyperlink(this.gvVitals, "Select$" + e.Row.RowIndex));
        }
    }

    protected void gvVitals_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            Boolean blnExits = false;
            int iSeqNo = 0;
            //if (hdnNew.Value == "")
            //{
            String sVitalID = "";
            String sVitalName = "";
            sVitalID = gvVitals.Rows[gvVitals.SelectedIndex].Cells[0].Text.ToString().Trim();
            sVitalName = gvVitals.Rows[gvVitals.SelectedIndex].Cells[1].Text.ToString().Trim();

            DataTable Dt = createTable();
            foreach (GridViewRow gvr in gvSelectedVitals.Rows)
            {
                //HiddenField hdnVitalID = (HiddenField)gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalSignName)].FindControl("hdnVitalID");
                //HiddenField hdnTemplateID = (HiddenField)gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalSignName)].FindControl("hdnTemplateId");
                String VitalID = "";
                String TemplateID = "";
                VitalID = gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.Trim();
                if (VitalID != "&nbsp;")
                {

                    TemplateID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                    TextBox txtVitalName = (TextBox)gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");

                    if (VitalID.ToString().Trim() == sVitalID.ToString().Trim())   // (HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalSignName)]) == sVitalID)
                    {
                        blnExits = true;
                    }
                    if (Convert.ToInt32(HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim())) >= iSeqNo)
                    {
                        iSeqNo = Convert.ToInt32(HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim()));
                    }
                    DataRow drDt = Dt.NewRow();
                    drDt["TemplateId"] = TemplateID.ToString().Trim();
                    drDt["VitalID"] = VitalID.ToString().Trim();
                    drDt["Vital"] = txtVitalName.Text.ToString().Trim();
                    drDt["SequenceNo"] = iSeqNo;
                    drDt["Active"] = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                    Dt.Rows.Add(drDt);
                }
            }
            if (blnExits == false)
            {
                DataRow drDt1 = Dt.NewRow();
                drDt1["TemplateId"] = "0";
                drDt1["VitalID"] = sVitalID;
                drDt1["Vital"] = sVitalName;
                drDt1["SequenceNo"] = iSeqNo + 1;
                drDt1["Active"] = 1;
                Dt.Rows.Add(drDt1);
            }

            ViewState["Count"] = "0";
            if (Dt.Rows.Count > 0)
            {
                gvSelectedVitals.DataSource = Dt;
                gvSelectedVitals.DataBind();
            }
            else
            {
                BindBlankSelectedVitalGrid();
            }
            ViewState["Count"] = null;

            //}
            //else if (hdnNew.Value != "")
            //{
            //    if (hdnEditMode.Value != "")
            //    {
            //        String sVitalID = "";
            //        String sVitalName = "";
            //        sVitalID = gvVitals.Rows[gvVitals.SelectedIndex].Cells[0].Text.ToString().Trim();
            //        sVitalName = gvVitals.Rows[gvVitals.SelectedIndex].Cells[1].Text.ToString().Trim();

            //        DataTable Dt = createTable();
            //        foreach (GridViewRow gvr in gvSelectedVitals.Rows)
            //        {
            //            if (HttpUtility.HtmlDecode(gvr.Cells[1].Text.ToString().Trim()) == sVitalID)
            //            {
            //                blnExits = true;
            //            }
            //            DataRow drDt = Dt.NewRow();
            //            drDt["TemplateId"] = HttpUtility.HtmlDecode(gvr.Cells[0].Text.ToString().Trim());
            //            drDt["VitalID"] = HttpUtility.HtmlDecode(gvr.Cells[1].Text.ToString().Trim());
            //            drDt["VitalSignName"] = HttpUtility.HtmlDecode(gvr.Cells[2].Text.ToString().Trim());
            //            Dt.Rows.Add(drDt);
            //        }
            //        if (blnExits == false)
            //        {
            //            DataRow drDt1 = Dt.NewRow();
            //            drDt1["TemplateId"] = "0";
            //            drDt1["VitalID"] = sVitalID;
            //            drDt1["VitalSignName"] = sVitalName;
            //            Dt.Rows.Add(drDt1);
            //        }
            //        gvSelectedVitals.DataSource = Dt;
            //        gvSelectedVitals.DataBind();
            //    }
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvVitals_PageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvVitals.PageIndex = e.NewPageIndex;
        BindVitalsGrid();
    }

    protected void gvSelectedVitals_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {

            e.Row.Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridSelectedVitals.Active)].Visible = false;
            e.Row.Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text != "&nbsp;")
            {
                ViewState["Count"] = Convert.ToInt16(ViewState["Count"].ToString()) + 1;
                TextBox txtVitalName = (TextBox)e.Row.FindControl("txtVitalNameGrid");
                txtVitalName.ReadOnly = true;
                if (ViewState["Count"] != null)
                {
                    if ((Convert.ToInt16(ViewState["Count"].ToString()) % 2) != 0)
                    {
                        txtVitalName.BackColor = System.Drawing.ColorTranslator.FromHtml("#EFF3FB");
                    }
                }

                ImageButton imgRemove = (ImageButton)e.Row.FindControl("ibtnRemoveFromGrid");
                ImageButton imgInActive = (ImageButton)e.Row.FindControl("ibtnInActive");
                ImageButton imgActive = (ImageButton)e.Row.FindControl("ibtnActivate");


                //imgUp.Attributes.Add("onclick", "javascript:MoveUp('" + hdnSerialNo.ClientID + "','" + hdnVitalID.ClientID + "','" + txtVitalName.ClientID + "','" + hdnTemplateId.ClientID + "','" + imgRemove.ClientID + "','" + imgInActive.ClientID + "','" + hdnActive.ClientID + "');");
                //imgDown.Attributes.Add("onclick", "javascript:MoveDown('" + hdnSerialNo.ClientID + "','" + hdnVitalID.ClientID + "','" + txtVitalName.ClientID + "','" + hdnTemplateId.ClientID + "','" + imgRemove.ClientID + "','" + imgInActive.ClientID + "','" + hdnActive.ClientID + "');");
                String sTemplateID = HttpUtility.HtmlDecode(e.Row.Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                if (sTemplateID.ToString().Trim() == "0")
                {
                    imgRemove.Visible = true;
                    imgInActive.Visible = false;
                    imgActive.Visible = false;
                }
                else
                {
                    if (HttpUtility.HtmlDecode(e.Row.Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim()) == "0")
                    {
                        imgRemove.Visible = false;
                        imgInActive.Visible = false;
                        imgActive.Visible = true;
                        //imgActive.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to Activate this record :  " + txtVitalName.Text + "')");

                    }
                    else
                    {
                        imgRemove.Visible = false;
                        imgActive.Visible = false;
                        imgInActive.Visible = true;
                        //imgInActive.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you want to De-Activate this record :  " + txtVitalName.Text + "')");

                    }
                }
            }
            else
            {
                ImageButton imgRemove = (ImageButton)e.Row.FindControl("ibtnRemoveFromGrid");
                ImageButton imgInActive = (ImageButton)e.Row.FindControl("ibtnInActive");
                ImageButton imgActive = (ImageButton)e.Row.FindControl("ibtnActivate");
                imgRemove.Visible = false;
                imgInActive.Visible = false;
                imgActive.Visible = false;
            }
        }
    }

    protected void gvSelectedVitals_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().Trim() == "Remove")
            {
                DataTable Dt = createTable();
                foreach (GridViewRow gvr in gvSelectedVitals.Rows)
                {
                    String sVitalID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                    if (sVitalID.ToString().Trim() != e.CommandArgument.ToString().Trim())
                    {
                        String sTemplateID = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                        TextBox txtVitalName = (TextBox)gvr.Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");

                        DataRow drDt = Dt.NewRow();
                        drDt["TemplateId"] = sTemplateID.ToString().Trim();
                        drDt["VitalID"] = sVitalID.ToString().Trim();
                        drDt["Vital"] = txtVitalName.Text.ToString().Trim();
                        drDt["SequenceNo"] = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim());
                        drDt["Active"] = HttpUtility.HtmlDecode(gvr.Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                        Dt.Rows.Add(drDt);

                    }
                }
                ViewState["Count"] = "0";
                if (Dt.Rows.Count > 0)
                {
                    gvSelectedVitals.DataSource = Dt;
                    gvSelectedVitals.DataBind();
                }
                else
                {
                    BindBlankSelectedVitalGrid();
                }
                ViewState["Count"] = null;
            }
            else if (e.CommandName.ToString().Trim() == "MoveUp")
            {
                DataTable Dt = createTable();
                for (int i = 1; i <= gvSelectedVitals.Rows.Count; i++)
                {

                    if (i + 1 == Convert.ToInt16(e.CommandArgument.ToString().Trim()))
                    {
                        String PreviousTemplateID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                        String PreviousVitalID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                        TextBox PrevioustxtVitalName = (TextBox)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");
                        ImageButton PreviousibtnInActive = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnInActive");
                        ImageButton PreviousibtnRemoveFromGrid = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnRemoveFromGrid");
                        String PreviousActive = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                        String PreviousSequenceNo = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim());

                        String ClickedTemplateID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                        String ClickedVitalID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                        TextBox ClickedtxtVitalName = (TextBox)gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");
                        ImageButton ClickedibtnInActive = (ImageButton)gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnInActive");
                        ImageButton ClickedibtnRemoveFromGrid = (ImageButton)gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnRemoveFromGrid");
                        String ClickedActive = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                        String ClickedSequenceNo = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim());

                        DataRow drDt = Dt.NewRow();
                        drDt["Vital"] = ClickedtxtVitalName.Text.ToString().Trim();
                        drDt["TemplateId"] = ClickedTemplateID.ToString().Trim();
                        drDt["VitalID"] = ClickedVitalID.ToString().Trim();
                        drDt["SequenceNo"] = i;
                        drDt["Active"] = ClickedActive;
                        Dt.Rows.Add(drDt);

                        DataRow drDt1 = Dt.NewRow();
                        drDt1["Vital"] = PrevioustxtVitalName.Text.ToString().Trim();
                        drDt1["TemplateId"] = PreviousTemplateID.ToString().Trim();
                        drDt1["VitalID"] = PreviousVitalID.ToString().Trim();
                        drDt1["SequenceNo"] = i + 1;
                        drDt1["Active"] = PreviousActive;
                        Dt.Rows.Add(drDt1);
                        i = i + 1;
                    }
                    else
                    {
                        String TemplateID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                        String VitalID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                        TextBox txtVitalName = (TextBox)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");
                        ImageButton btnInActive = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnInActive");
                        ImageButton ibtnRemoveFromGrid = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnRemoveFromGrid");
                        String Active = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                        String SequenceNo = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim());

                        DataRow drDt = Dt.NewRow();
                        drDt["Vital"] = txtVitalName.Text.ToString().Trim();
                        drDt["TemplateId"] = TemplateID.ToString().Trim();
                        drDt["VitalID"] = VitalID.ToString().Trim();
                        drDt["SequenceNo"] = i;
                        drDt["Active"] = Active;
                        Dt.Rows.Add(drDt);
                    }
                }

                ViewState["Count"] = "0";
                if (Dt.Rows.Count > 0)
                {
                    gvSelectedVitals.DataSource = Dt;
                    gvSelectedVitals.DataBind();
                }
                else
                {
                    BindBlankSelectedVitalGrid();
                }
                ViewState["Count"] = null;
            }
            else if (e.CommandName.ToString().Trim() == "MoveDown")
            {
                DataTable Dt = createTable();
                for (int i = 1; i <= gvSelectedVitals.Rows.Count; i++)
                {
                    if (gvSelectedVitals.Rows.Count != Convert.ToInt16(e.CommandArgument.ToString().Trim()))
                    {
                        if (i == Convert.ToInt16(e.CommandArgument.ToString().Trim()))
                        {
                            String ClickedTemplateID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                            String ClickedVitalID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                            TextBox ClickedtxtVitalName = (TextBox)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");
                            ImageButton ClickedibtnInActive = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnInActive");
                            ImageButton ClickedibtnRemoveFromGrid = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnRemoveFromGrid");
                            String ClickedActive = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                            String ClickedSequenceNo = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim());

                            String NextTemplateID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                            String NextVitalID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                            TextBox NexttxtVitalName = (TextBox)gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");
                            ImageButton NextibtnInActive = (ImageButton)gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnInActive");
                            ImageButton NextibtnRemoveFromGrid = (ImageButton)gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnRemoveFromGrid");
                            String NextActive = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                            String NextSequenceNo = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i].Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim());

                            DataRow drDt1 = Dt.NewRow();
                            drDt1["Vital"] = NexttxtVitalName.Text.ToString().Trim();
                            drDt1["TemplateId"] = NextTemplateID.ToString().Trim();
                            drDt1["VitalID"] = NextVitalID.ToString().Trim();
                            drDt1["SequenceNo"] = i;
                            drDt1["Active"] = NextActive;
                            Dt.Rows.Add(drDt1);

                            DataRow drDt = Dt.NewRow();
                            drDt["Vital"] = ClickedtxtVitalName.Text.ToString().Trim();
                            drDt["TemplateId"] = ClickedTemplateID.ToString().Trim();
                            drDt["VitalID"] = ClickedVitalID.ToString().Trim();
                            drDt["SequenceNo"] = i + 1;
                            drDt["Active"] = ClickedActive;
                            Dt.Rows.Add(drDt);

                            i = i + 1;
                        }
                        else
                        {
                            String TemplateID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                            String VitalID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                            TextBox txtVitalName = (TextBox)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");
                            ImageButton btnInActive = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnInActive");
                            ImageButton ibtnRemoveFromGrid = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnRemoveFromGrid");
                            String Active = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                            String SequenceNo = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim());

                            DataRow drDt = Dt.NewRow();
                            drDt["Vital"] = txtVitalName.Text.ToString().Trim();
                            drDt["TemplateId"] = TemplateID.ToString().Trim();
                            drDt["VitalID"] = VitalID.ToString().Trim();
                            drDt["SequenceNo"] = i;
                            drDt["Active"] = Active;
                            Dt.Rows.Add(drDt);
                        }
                    }
                    else
                    {
                        String TemplateID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.TemplateID)].Text.ToString().Trim());
                        String VitalID = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalID)].Text.ToString().Trim());
                        TextBox txtVitalName = (TextBox)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.VitalName)].FindControl("txtVitalNameGrid");
                        ImageButton btnInActive = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnInActive");
                        ImageButton ibtnRemoveFromGrid = (ImageButton)gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.InActive)].FindControl("ibtnRemoveFromGrid");
                        String Active = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.Active)].Text.ToString().Trim());
                        String SequenceNo = HttpUtility.HtmlDecode(gvSelectedVitals.Rows[i - 1].Cells[Convert.ToByte(GridSelectedVitals.SequenceNo)].Text.ToString().Trim());

                        DataRow drDt = Dt.NewRow();
                        drDt["Vital"] = txtVitalName.Text.ToString().Trim();
                        drDt["TemplateId"] = TemplateID.ToString().Trim();
                        drDt["VitalID"] = VitalID.ToString().Trim();
                        drDt["SequenceNo"] = i;
                        drDt["Active"] = Active;
                        Dt.Rows.Add(drDt);
                    }
                }

                ViewState["Count"] = "0";
                if (Dt.Rows.Count > 0)
                {
                    gvSelectedVitals.DataSource = Dt;
                    gvSelectedVitals.DataBind();
                }
                else
                {
                    BindBlankSelectedVitalGrid();
                }
                ViewState["Count"] = null;
            }
            else if (e.CommandName.ToString().Trim() == "InActive")
            {
                BaseC.EMRVitals objVitals = new BaseC.EMRVitals(sConString);
                Int32 i = objVitals.DeActivateVitalTemplateDetails(Convert.ToInt32(gvTemplate.SelectedRow.Cells[0].Text.Trim()), Convert.ToInt32(e.CommandArgument.ToString().Trim()));
                if (i == 0)
                {
                    lblMessage.Text = "Record De-Actived... ";
                    //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
                    //ddlVitalTemplateSet_OnSelectedIndexChanged(sender, e);
                    BindSelectedVitals();
                }
                else
                {
                    Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
                }
            }
            else if (e.CommandName.ToString().Trim() == "Activate")
            {
                BaseC.EMRVitals objVitals = new BaseC.EMRVitals(sConString);
                Int32 i = objVitals.ActivateVitalTemplateDetails(Convert.ToInt32(gvTemplate.SelectedRow.Cells[0].Text.Trim()), Convert.ToInt32(e.CommandArgument.ToString().Trim()));
                if (i == 0)
                {
                    lblMessage.Text = "Record Activated... ";
                    //Alert.ShowAjaxMsg("Record Activated... ", this.Page);
                    //ddlVitalTemplateSet_OnSelectedIndexChanged(sender, e);
                }
                else
                {
                    Alert.ShowAjaxMsg("Error In Activation... ", this.Page);
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

    protected void gvSelectedVitals_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
    }

    protected DataTable createTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("TemplateId");
        Dt.Columns.Add("VitalID");
        Dt.Columns.Add("Vital");
        Dt.Columns.Add("SequenceNo");
        Dt.Columns.Add("Active");
        Dt.Columns.Add("SerialNo");
        Dt.Columns["SerialNo"].AutoIncrement = true;
        Dt.Columns["SerialNo"].AutoIncrementSeed = 1;
        Dt.Columns["SerialNo"].AutoIncrementStep = 1;
        return Dt;
    }

    protected void gvTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindSelectedVitals();
            txtVitalTemplateSet.Text = gvTemplate.SelectedRow.Cells[1].Text.Trim(); ;
            txtDescription.Text = gvTemplate.SelectedRow.Cells[2].Text.Trim(); ;
            ddlMeasurement.SelectedValue = gvTemplate.SelectedRow.Cells[4].Text.Trim();
            chkDefault.Checked = Convert.ToBoolean(gvTemplate.SelectedRow.Cells[5].Text.Trim());
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;

        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;

            if (ViewState["BlankGrid"] != null)
            {
                e.Row.Cells[7].Visible = false;
            }
        }
    }

    #region Private method for Create DataTable dynamically for Gridview 'gvSelectedVitals'
    protected void BindBlankSelectedVitalGrid()
    {
        try
        {
            DataTable Dt = new DataTable();
            //  Dt.Columns.Add("SerialNo");
            Dt.Columns.Add("TemplateId");
            Dt.Columns.Add("VitalID");
            Dt.Columns.Add("Vital");
            Dt.Columns.Add("SequenceNo");
            Dt.Columns.Add("Active");
            //for (int i = 0; i < 4; i++)
            //{
            DataRow dr = Dt.NewRow();
            //  dr["SerialNo"] = "";
            dr["TemplateId"] = "";
            dr["VitalID"] = "";
            dr["Vital"] = "";
            dr["SequenceNo"] = "";
            dr["Active"] = "";
            Dt.Rows.Add(dr);
            //}

            gvSelectedVitals.DataSource = Dt;
            gvSelectedVitals.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    #endregion

    protected void BindBlankgvTemplate()
    {
        try
        {
            DataTable Dt = new DataTable();

            Dt.Columns.Add("Id");
            Dt.Columns.Add("TemplateName");
            Dt.Columns.Add("TemplateDescription");
            Dt.Columns.Add("Measurement");
            Dt.Columns.Add("MeasurementSystem");
            Dt.Columns.Add("DefaultTemplate");
            Dt.Columns.Add("DefaultValue");
            //for (int i = 0; i < 4; i++)
            //{
            DataRow dr = Dt.NewRow();
            //  dr["SerialNo"] = "";
            dr["Id"] = "";
            dr["TemplateName"] = "";
            dr["TemplateDescription"] = "";
            dr["Measurement"] = "";
            dr["MeasurementSystem"] = "";
            dr["DefaultTemplate"] = "";
            dr["DefaultValue"] = "";

            Dt.Rows.Add(dr);
            //}
            ViewState["BlankGrid"] = "True";
            gvTemplate.DataSource = Dt;
            gvTemplate.DataBind();
            ViewState["BlankGrid"] = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
