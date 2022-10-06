using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class EMR_Masters_GroupTaggingEMRTemplate : System.Web.UI.Page
{
    //Global variables

    DataSet ds = new DataSet();//dataset objetct
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;//connectionstring
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.EMRMasters objEMRMasters;
    StringBuilder strXML;
    ArrayList coll;
    private bool RowSelStatus = false;
    //Page load event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //btnSave.Visible = false;
            //btnSend4WO.Visible = false;
            //btnClear.Visible = false;
            hdnisSaveChk.Value = "0";
            bindTaggedMaster();
            bindEMRTemplate();

        }
    }
    protected void bindTaggedMaster()
    {
        try
        {
            objEMRMasters = new BaseC.EMRMasters(sConString);
            DataSet ds = objEMRMasters.GetTemplateGroup(common.myInt(Session["FacilityId"]));

            foreach (DataRow dr in ((DataTable)ds.Tables[0]).Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["GroupName"];
                item.Value = dr["Groupid"].ToString();
                //item.Attributes.Add("TableType", common.myStr(dr["TableType"]));
                //item.Attributes.Add("MasterProcedure", common.myStr(dr["MasterProcedure"]));

                ddlTaggedFor.Items.Add(item);
                item.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void bindEMRTemplate()
    {
        try
        {
            objEMRMasters = new BaseC.EMRMasters(sConString);
            DataSet ds = objEMRMasters.GetEMRTemplate();

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTemplate.DataSource = ds.Tables[0];
                gvTemplate.DataBind(); 
            }
            else
            {
                gvTemplate.DataSource = null;
                gvTemplate.DataBind();             
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    protected void ddlTaggedFor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindEMRTemplateGroupWise(common.myInt(ddlTaggedFor.SelectedValue));
            
            
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void bindEMRTemplateGroupWise(int GroupId)
    {
        try
        {
            objEMRMasters = new BaseC.EMRMasters(sConString);
            DataSet ds = objEMRMasters.GetEMRTemplateGroupWise(GroupId);
            lblTaggingGroup.Text = common.myStr(ddlTaggedFor.SelectedItem.Text);
            ViewState["GroupWiseTag"] = ds.Tables[0];
            DataTable dt = (DataTable)ViewState["GroupWiseTag"];
            if (dt.Rows.Count > 0)
            {
                grvGroupWiseTag.DataSource = dt;
                grvGroupWiseTag.DataBind();
            }
            else
            {
                grvGroupWiseTag.DataSource = null;
                grvGroupWiseTag.DataBind();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
      
    
    protected DataTable BindBlankTableInv()
    {
        DataTable dtInv = new DataTable();

        dtInv.Columns.Add("Id", typeof(int));
        dtInv.Columns.Add("TemplateId", typeof(int));
        dtInv.Columns.Add("TemplateName", typeof(string));
        dtInv.Columns.Add("GroupId", typeof(int));
        dtInv.Columns.Add("GroupName", typeof(string));
        return dtInv;
    }
    

    protected void lnkDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkBtn = (LinkButton)sender;
            int hdnSelTemplateId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnSelTemplateId")).Value);
            if (hdnSelTemplateId > 0)
            {
                DataTable dt = (DataTable)ViewState["GroupWiseTag"];
                foreach (DataRow dr in dt.Select("TemplateId = " + common.myStr(hdnSelTemplateId) + ""))
                {
                    if (common.myStr(dr["TemplateId"]) == common.myStr(hdnSelTemplateId))
                        dr.Delete();
                }
                hdnisSaveChk.Value = "1";
                dt.AcceptChanges();
                grvGroupWiseTag.DataSource = dt;
                grvGroupWiseTag.DataBind();
                ViewState["GroupWiseTag"] = dt;
              
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(hdnisSaveChk.Value) == 0)
            {
                Alert.ShowAjaxMsg("No Data to Save!!!", Page.Page);
                return;
            }
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            objEMRMasters = new BaseC.EMRMasters(sConString);

            strXML = new StringBuilder();
            coll = new ArrayList();

            foreach (GridViewRow gvRow in grvGroupWiseTag.Rows)
            {
                HiddenField hdnSelTemplateId = (HiddenField)gvRow.FindControl("hdnSelTemplateId");

                if (common.myInt(hdnSelTemplateId.Value) > 0)
                {
                    coll.Add(common.myInt(hdnSelTemplateId.Value));
                    strXML.Append(common.setXmlTable(ref coll));

                }
            }

            if (ddlTaggedFor.SelectedIndex == -1)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Group !";
                return;

            }

            if (strXML.ToString() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select " +lblEMRTemplate.Text   + " !";
                return;
            }

            lblMessage.Text = "";

            string strmsg = objEMRMasters.SaveEMRTemplateGroupWise(common.myInt(Session["HospitalLocationId"]), strXML.ToString(), common.myInt(ddlTaggedFor.SelectedValue), common.myBool(1), common.myInt(Session["UserId"]));

            if ((strmsg.Contains(" Update") || strmsg.Contains(" Save")) && !strmsg.Contains("usp"))
            {
                lblMessage.Text = strmsg;
                bindEMRTemplateGroupWise(common.myInt(ddlTaggedFor.SelectedValue));
                //Alert.ShowAjaxMsg(strmsg, Page.Page);
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvTemplate_OnPageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        bindEMRTemplate();
    }
    protected void gvTemplate_PreRender(object sender, EventArgs e)
    {
        if (RowSelStatus == false)
        {
            bindEMRTemplate();
        }
    }
    protected void gvTemplate_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }
    protected void gvTemplate_OnItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {   RowSelStatus = true;
               try
                {
                    if (ddlTaggedFor.SelectedIndex == -1)
                    {
                        Alert.ShowAjaxMsg("Please!! Select Group to tag..", Page.Page);
                        return;
                    }
                    else
                    {
                        GridDataItem item = e.Item as GridDataItem;
                        Label lblColumnName = (Label)item.FindControl("lblColumnName");
                        

                        DataTable dt;
                        //if (gvTemplate.Rows.Count > 0)
                       // {
                            grvGroupWiseTag.DataSource = null;
                            grvGroupWiseTag.DataBind();

                            if (ViewState["GroupWiseTag"] == null)
                                dt = BindBlankTableInv();
                            else
                                dt = (DataTable)ViewState["GroupWiseTag"];

                            DataView dv = dt.DefaultView;
                            dv.RowFilter = "";
                            
                                    dv.RowFilter = "TemplateId = " + common.myStr(e.CommandArgument) + "";
                                    if (dv.ToTable().Rows.Count == 0)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr["Id"] = 0;
                                        dr["TemplateId"] = common.myInt(e.CommandArgument);
                                        dr["TemplateName"] = common.myStr(lblColumnName.Text);
                                        dr["GroupId"] = common.myStr(ddlTaggedFor.SelectedValue);
                                        dr["GroupName"] = common.myStr(ddlTaggedFor.SelectedItem.Text);
                                        dt.Rows.Add(dr);
                                        dt.AcceptChanges();

                                    }

                            hdnisSaveChk.Value = "1";
                            dv.RowFilter = "";
                            ViewState["GroupWiseTag"] = dt;
                            grvGroupWiseTag.DataSource = dt;
                            grvGroupWiseTag.DataBind();

                        //}

                    }

                }
                catch (Exception Ex)
                {
                    clsExceptionLog objException = new clsExceptionLog();
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = ("Error: " + Ex.Message);
                    objException.HandleException(Ex);
                }
        }

    }
}
