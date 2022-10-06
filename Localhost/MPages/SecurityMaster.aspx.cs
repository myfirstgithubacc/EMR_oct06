using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Text;
using Telerik.Web.UI;
using System.Collections.Generic;

public partial class MPages_SecurityMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            if (!IsPostBack)
            {
                lblmsg.Text = "";
                pnlUpdate.Visible = false;
                btnUpdate.Visible = false;
                BindGroups();
                BindPages();
                BindModuleNames();
                txtGroupName.Attributes.Add("onkeypress", "Unselect();");
                ddlGroupName.Attributes.Add("onchange", "ClearText();");
                DataSet objDs = CreateTable();
                DataRow objDr;
                for (int i = 0; i <= 12; i++)
                {
                    objDr = objDs.Tables[0].NewRow();
                    objDr[0] = 0;
                    objDr[1] = 0;
                    objDr[2] = "";
                    objDr[3] = 0;
                    objDr[4] = "";
                    objDr[5] = 0;
                    objDr[6] = 0;
                    objDr[7] = 0;
                    objDr[8] = 0;
                    objDr[9] = 0;
                    //objDr[10] = 0;

                    objDs.Tables[0].Rows.Add(objDr);
                }
                gvSecurity.DataSource = objDs;
                gvSecurity.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public void ClearApplicationCache()
    {
        List<string> keys = new List<string>();
        // retrieve application Cache enumerator
        IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
        // copy all keys that currently exist in Cache
        while (enumerator.MoveNext())
        {
            keys.Add(enumerator.Key.ToString());
        }
        // delete every key from cache
        for (int i = 0; i < keys.Count; i++)
        {
            HttpRuntime.Cache.Remove(keys[i]);
        }
    }
    private void BindPages()
    {
        try
        {
            BaseC.Security objSec = new BaseC.Security(sConString);
            RadgvPages.DataSource = objSec.GetPageNames(Convert.ToInt16(ddlModuleName.SelectedValue));
            RadgvPages.DataBind();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindGroups()
    {
        try
        {
            if (Session["HospitalLocationID"] != null)
            {
                ddlGroupName.Items.Clear();
                ListItem lst = new ListItem();
                lst.Value = "0";
                lst.Text = "Select Group";
                lst.Selected = true;
                ddlGroupName.Items.Add(lst);
                BaseC.Security objSec = new BaseC.Security(sConString);
                ddlGroupName.DataSource = objSec.GetGroupName(Convert.ToInt16(Session["HospitalLocationID"]));
                ddlGroupName.DataTextField = "GroupName";
                ddlGroupName.DataValueField = "GroupID";
                ddlGroupName.DataBind();
                //foreach (ListItem item in ddlGroupName.Items)
                //{
                //    if (item.Value != "0")
                //    {
                //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
                //        {
                //            item.Attributes.Add("Style", "background-color:Pink;");
                //        }
                //    }
                //}
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvPages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RadgvPages_OnItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType != Telerik.Web.UI.GridItemType.Pager)
        {
            //e.Item.Cells[0].Visible = false;
            //e.Item.Cells[1].Visible = false;
            //e.Item.Cells[2].Visible = false;
        }
    }

    protected void gvPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            AddPages("0", gvPages.SelectedRow.Cells[0].Text, gvPages.SelectedRow.Cells[3].Text, gvPages.SelectedRow.Cells[1].Text, gvPages.SelectedRow.Cells[2].Text);

            BaseC.Security objSec = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RadgvPages_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnPageID = ((HiddenField)(RadgvPages.SelectedItems[0].FindControl("hdnPageID")));
            HiddenField hdnModuleID = ((HiddenField)(RadgvPages.SelectedItems[0].FindControl("hdnModuleID")));
            HiddenField hdnModuleName = ((HiddenField)(RadgvPages.SelectedItems[0].FindControl("hdnModuleName")));
            Label lbl_PageName = ((Label)(RadgvPages.SelectedItems[0].FindControl("lbl_PageName")));

            AddPages("0", hdnPageID.Value, lbl_PageName.Text, hdnModuleID.Value, hdnModuleName.Value);

            BaseC.Security objSec = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void AddPages(string Id, string PageId, string PageName, string ModuleId, string ModuleName)
    {
        try
        {
            DataSet objDs = new DataSet();
            DataTable objDt = new DataTable();
            int i = 0;
            objDs.Tables.Add(objDt);

            DataRow objDr;
            objDs.Tables[0].Columns.Add("Id", typeof(Int32));
            objDs.Tables[0].Columns.Add("PageId", typeof(Int32));
            objDs.Tables[0].Columns.Add("PageName", typeof(string));
            objDs.Tables[0].Columns.Add("ModuleId", typeof(Int16));
            objDs.Tables[0].Columns.Add("ModuleName", typeof(string));
            objDs.Tables[0].Columns.Add("InsertData", typeof(bool));
            objDs.Tables[0].Columns.Add("EditData", typeof(bool));
            objDs.Tables[0].Columns.Add("CancelData", typeof(bool));
            objDs.Tables[0].Columns.Add("DisplayData", typeof(bool));
            objDs.Tables[0].Columns.Add("PrintData", typeof(bool));

            foreach (GridDataItem item in gvSecurity.Items)
            {
                if (item.Cells[3].Text.Trim() != "")
                {
                    Label lblPageId = (Label)item.FindControl("lblpageid");
                    Label lblPageName = (Label)item.FindControl("lblPageName");
                    HiddenField hdnModuleId = (HiddenField)item.FindControl("hdnModuleId");
                    HiddenField hdnModuleName = (HiddenField)item.FindControl("hdnModuleName");
                    if (HttpUtility.HtmlDecode(lblPageName.Text) != "")
                        if (PageId != lblPageId.Text)
                        {
                            objDr = objDs.Tables[0].NewRow();
                            objDr["Id"] = HttpUtility.HtmlDecode(item.Cells[3].Text);


                            objDr["PageId"] = HttpUtility.HtmlDecode(lblPageId.Text);
                            objDr["PageName"] = lblPageName.Text;
                            objDr["ModuleId"] = HttpUtility.HtmlDecode(hdnModuleId.Value);
                            objDr["ModuleName"] = hdnModuleName.Value;

                            CheckBox chkInsertData = (CheckBox)item.FindControl("chkInsertData");
                            CheckBox chkEditData = (CheckBox)item.FindControl("chkEditData");
                            CheckBox chkCancelData = (CheckBox)item.FindControl("chkCancelData");
                            CheckBox chkDisplayData = (CheckBox)item.FindControl("chkDisplayData");
                            CheckBox chkPrintData = (CheckBox)item.FindControl("chkPrintData");
                            objDr["InsertData"] = chkInsertData.Checked;
                            objDr["EditData"] = chkEditData.Checked;
                            objDr["CancelData"] = chkCancelData.Checked;
                            objDr["DisplayData"] = chkDisplayData.Checked;
                            objDr["PrintData"] = chkPrintData.Checked;
                            objDs.Tables[0].Rows.Add(objDr);
                        }
                        else
                            i = Convert.ToInt32(PageId);
                }
            }
            if (i != Convert.ToInt32(PageId))
            {
                objDr = objDs.Tables[0].NewRow();
                objDr["Id"] = Id;
                objDr["PageId"] = PageId;
                objDr["PageName"] = PageName;
                objDr["ModuleId"] = ModuleId;
                objDr["ModuleName"] = ModuleName;
                objDr["InsertData"] = 1;
                objDr["EditData"] = 1;
                objDr["CancelData"] = 1;
                objDr["DisplayData"] = 1;
                objDr["PrintData"] = 1;
                objDs.Tables[0].Rows.Add(objDr);
                Cache.Insert("UserPermissions" + Session["UserID"].ToString(), objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
               
                gvSecurity.DataSource = objDs;
                gvSecurity.DataBind();
                ViewState["gvSecurity"] = objDs;
                //    }
            }
            for (int k = 0; k < gvSecurity.Items.Count; k++)
            {
                CheckBox chkInsertData = (CheckBox)gvSecurity.Items[k].FindControl("chkInsertData");
                CheckBox chkEditData = (CheckBox)gvSecurity.Items[k].FindControl("chkEditData");
                CheckBox chkCancelData = (CheckBox)gvSecurity.Items[k].FindControl("chkCancelData");
                CheckBox chkDisplayData = (CheckBox)gvSecurity.Items[k].FindControl("chkDisplayData");
                CheckBox chkPrintData = (CheckBox)gvSecurity.Items[k].FindControl("chkPrintData");

                chkInsertData.Enabled = true;
                chkEditData.Enabled = true;
                chkCancelData.Enabled = true;
                chkDisplayData.Enabled = true;
                chkPrintData.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindRightsGrid()
    {
        try
        {
            if (ddlGroupName.SelectedValue != "0")
            {
                if (Session["UserID"] != null)
                {
                    BaseC.Security objSec = new BaseC.Security(sConString);
                    DataSet objDs = objSec.GetPageRights(Convert.ToInt32(ddlGroupName.SelectedValue));
                    Cache.Insert("UserPermissions" + Session["UserID"].ToString(), objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                    if (objDs.Tables[0].Rows.Count > 0)
                    {
                        if (ViewState["gvSecurity"] == null)
                        {
                            gvSecurity.DataSource = objDs;
                            gvSecurity.DataBind();
                            lblmsg.Text = "";
                        }
                        else
                        {
                            if (ViewState["gvSecurity"] != null)
                            {

                                gvSecurity.DataSource = ViewState["gvSecurity"];
                                gvSecurity.DataBind();
                            }
                            
                        }
                    }
                    else
                    {
                        if (ViewState["gvSecurity"] != null)
                        {

                            gvSecurity.DataSource = ViewState["gvSecurity"];
                            gvSecurity.DataBind();
                        }
                        else
                        {
                            gvSecurity.DataSource = null;
                            gvSecurity.DataBind();
                            lblmsg.Text = "Data Not Found!";
                        }
                    }

                }
                else
                {
                    Alert.ShowAjaxMsg("Login Again.", Page);
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindRightsGrid();
            BaseC.Security objSec = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
            for (int i = 0; i < gvSecurity.Items.Count; i++)
            {
                Label lblpid = (Label)gvSecurity.Items[i].FindControl("lblpageid");
                CheckBox chkInsertData = (CheckBox)gvSecurity.Items[i].FindControl("chkInsertData");
                CheckBox chkEditData = (CheckBox)gvSecurity.Items[i].FindControl("chkEditData");
                CheckBox chkCancelData = (CheckBox)gvSecurity.Items[i].FindControl("chkCancelData");
                CheckBox chkDisplayData = (CheckBox)gvSecurity.Items[i].FindControl("chkDisplayData");
                CheckBox chkPrintData = (CheckBox)gvSecurity.Items[i].FindControl("chkPrintData");

                chkInsertData.Enabled = true;
                chkEditData.Enabled = true;
                chkCancelData.Enabled = true;
                chkDisplayData.Enabled = true;
                chkPrintData.Enabled = true;
            }
            //if (objSec.IsAdmin(Convert.ToInt16(ddlGroupName.SelectedValue)))
            //{
            //    lblmsg.Text = " (Administrator Group)";
            //}
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindModuleNames()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = dl.FillDataSet(CommandType.Text, "SELECT ModuleId, ModuleName FROM SecModuleMaster WHERE Active = 1  ORDER BY Sequence ");
            ddlModuleName.DataSource = ds.Tables[0];
            ddlModuleName.DataTextField = "ModuleName";
            ddlModuleName.DataValueField = "ModuleId";
            ddlModuleName.DataBind();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlModuleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindPages();
            BaseC.Security objSec = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCreateGroup_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            if (Session["HospitalLocationID"] != null && Session["UserID"] != null)
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                BaseC.ParseData objParse = new BaseC.ParseData();
                string GroupName = objParse.ParseQ(txtGroupName.Text);

                if (GroupName != "")
                {
                    DataSet objDs = objSec.GetGroupName(Convert.ToInt16(Session["HospitalLocationID"]), GroupName);
                    if (objDs.Tables[0].Rows.Count == 0)
                    {
                        msg = objSec.CreateUserGroup(Convert.ToInt16(Session["HospitalLocationID"]), GroupName, chkIsAdmin.Checked, Convert.ToInt32(Session["UserID"]));
                        lblMessage.Text = "Group Created.";
                        txtGroupName.Text = string.Empty;
                        BindGroups();
                    }
                    else
                        Alert.ShowAjaxMsg("Group Already Assigned.", Page);
                }
                //foreach (ListItem item in ddlGroupName.Items)
                //{
                //    if (item.Value != "0")
                //    {
                //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
                //        {
                //            item.Attributes.Add("Style", "background-color:Pink;");
                //        }
                //    }
                //}
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["HospitalLocationId"] != null && Session["UserID"] != null)
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                StringBuilder objStr = new StringBuilder();
                BaseC.ParseData objParse = new BaseC.ParseData();
                string str = "";
                string GroupName = objParse.ParseQ(txtGroupName.Text);
                foreach (GridDataItem item in gvSecurity.Items)
                {
                    Label lblpid = (Label)item.FindControl("lblpageid");
                    string strPageId = lblpid.Text.Trim();
                    CheckBox chkInsertData = (CheckBox)item.FindControl("chkInsertData");
                    CheckBox chkEditData = (CheckBox)item.FindControl("chkEditData");
                    CheckBox chkCancelData = (CheckBox)item.FindControl("chkCancelData");
                    CheckBox chkDisplayData = (CheckBox)item.FindControl("chkDisplayData");
                    CheckBox chkPrintData = (CheckBox)item.FindControl("chkPrintData");
                    if (chkInsertData.Checked.ToString() != "False" || chkEditData.Checked.ToString() != "False" || chkCancelData.Checked.ToString() != "False" || chkDisplayData.Checked.ToString() != "False" || chkPrintData.Checked.ToString() != "False")
                    {
                        objStr.Append("<Table1>");
                        objStr.Append("<c1>");
                        objStr.Append(strPageId);
                        objStr.Append("</c1>");
                        objStr.Append("<c2>");
                        objStr.Append(chkInsertData.Checked);
                        objStr.Append("</c2>");
                        objStr.Append("<c3>");
                        objStr.Append(chkEditData.Checked);
                        objStr.Append("</c3>");
                        objStr.Append("<c4>");
                        objStr.Append(chkCancelData.Checked);
                        objStr.Append("</c4>");
                        objStr.Append("<c5>");
                        objStr.Append(chkDisplayData.Checked);
                        objStr.Append("</c5>");
                        objStr.Append("<c6>");
                        objStr.Append(chkPrintData.Checked);
                        objStr.Append("</c6>");
                        objStr.Append("</Table1>");
                    }
                }
                if (objStr.ToString() != "")
                {
                    if (ddlGroupName.SelectedValue != "0" && GroupName == "")
                    {
                        str = (string)objSec.SaveAccessRights(Convert.ToInt16(ddlGroupName.SelectedValue), objStr.ToString(), Convert.ToInt16(Session["UserID"]), Convert.ToInt32(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                        Session["AddedRows"] = null;
                    }
                    else if (GroupName != "" && ddlGroupName.SelectedValue == "0")
                    {
                        str = (string)objSec.SaveAccessRights(0, objStr.ToString(), Convert.ToInt16(Session["UserID"]), Convert.ToInt32(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
                        Session["AddedRows"] = null;
                        BindGroups();
                    }
                    else
                        Alert.ShowAjaxMsg("Select GroupName or Enter a new one!", Page);
                    lblMessage.Text = "Record(s) Has Been Saved...";
                }
            }
            BaseC.Security objSec1 = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec1.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
            ClearApplicationCache();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspSecurityGetUserPages");
            if (objDs.Tables.Count == 3)
            {
                Cache["ModuleDetail"] = objDs.Tables[0].Copy();
                Cache["GroupDetail"] = objDs.Tables[1].Copy();
                Cache["MenuDetail"] = objDs.Tables[2].Copy();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
   
    private DataSet CreateAddedTable(int PageId)
    {
        DataSet objDs = CreateTable();
        try
        {
            DataRow objDr;
            foreach (GridViewRow item in gvSecurity.Items)
            {
                if (PageId.ToString() != item.Cells[0].Text)
                {
                    objDr = objDs.Tables[0].NewRow();
                    objDr["Id"] = item.Cells[0].Text;
                    objDr["PageId"] = item.Cells[1].Text;
                    objDr["PageName"] = item.Cells[2].Text;
                    objDr["ModuleId"] = item.Cells[3].Text;
                    objDr["ModuleName"] = item.Cells[4].Text;
                    CheckBox chkInsertData = (CheckBox)item.FindControl("chkInsertData");
                    CheckBox chkEditData = (CheckBox)item.FindControl("chkEditData");
                    CheckBox chkCancelData = (CheckBox)item.FindControl("chkCancelData");
                    CheckBox chkDisplayData = (CheckBox)item.FindControl("chkDisplayData");
                    CheckBox chkPrintData = (CheckBox)item.FindControl("chkPrintData");
                    objDr["InsertData"] = chkInsertData.Checked;
                    objDr["EditData"] = chkEditData.Checked;
                    objDr["CancelData"] = chkCancelData.Checked;
                    objDr["DisplayData"] = chkDisplayData.Checked;
                    objDr["PrintData"] = chkPrintData.Checked;
                    objDs.Tables[0].Rows.Add(objDr);
                }
            }
            return objDs;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return objDs;
        }
    }

    private DataSet CreateTable()
    {
        DataSet objDs = new DataSet();
        try
        {
            DataTable objDt = new DataTable();
            objDs.Tables.Add(objDt);

            objDs.Tables[0].Columns.Add("Id", typeof(Int32));
            objDs.Tables[0].Columns.Add("PageId", typeof(Int32));
            objDs.Tables[0].Columns.Add("PageName", typeof(string));
            objDs.Tables[0].Columns.Add("ModuleId", typeof(Int16));
            objDs.Tables[0].Columns.Add("ModuleName", typeof(string));
            objDs.Tables[0].Columns.Add("InsertData", typeof(bool));
            objDs.Tables[0].Columns.Add("EditData", typeof(bool));
            objDs.Tables[0].Columns.Add("CancelData", typeof(bool));
            objDs.Tables[0].Columns.Add("DisplayData", typeof(bool));
            objDs.Tables[0].Columns.Add("PrintData", typeof(bool));
            return objDs;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            return objDs;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlGroupName.SelectedValue != "0")
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                chkIsAdmin.Checked = objSec.IsAdmin(Convert.ToInt16(ddlGroupName.SelectedValue));
                txtGroupName.Text = ddlGroupName.SelectedItem.Text;
                hdnGroupId.Value = ddlGroupName.SelectedValue;
                pnlUpdate.Visible = true;
                btnUpdate.Visible = true;
                btnEdit.Visible = false;
                btnCreateGroup.Visible = false;
            }
            BaseC.Security objSec1 = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec1.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlGroupName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["gvSecurity"] = null;
            BindRightsGrid();
            BaseC.Security objSec = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
            for (int i = 0; i < gvSecurity.Items.Count; i++)
            {
                Label lblpid = (Label)gvSecurity.Items[i].FindControl("lblpageid");
                CheckBox chkInsertData = (CheckBox)gvSecurity.Items[i].FindControl("chkInsertData");
                CheckBox chkEditData = (CheckBox)gvSecurity.Items[i].FindControl("chkEditData");
                CheckBox chkCancelData = (CheckBox)gvSecurity.Items[i].FindControl("chkCancelData");
                CheckBox chkDisplayData = (CheckBox)gvSecurity.Items[i].FindControl("chkDisplayData");
                CheckBox chkPrintData = (CheckBox)gvSecurity.Items[i].FindControl("chkPrintData");

                chkInsertData.Enabled = true;
                chkEditData.Enabled = true;
                chkCancelData.Enabled = true;
                chkDisplayData.Enabled = true;
                chkPrintData.Enabled = true;
            }
            //if (objSec.IsAdmin(Convert.ToInt16(ddlGroupName.SelectedValue)))
            //{
            //    lblmsg.Text = " (Administrator Group)";
            //}
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            pnlUpdate.Visible = false;
            btnUpdate.Visible = false;
            btnEdit.Visible = true;
            txtGroupName.Text = "";
            chkIsAdmin.Checked = false;
            btnCreateGroup.Visible = true;

            BaseC.Security objSec = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.ParseData objParse = new BaseC.ParseData();
            string GroupName = objParse.ParseQ(txtGroupName.Text);
            if (Session["HospitalLocationID"] != null && GroupName != "")
            {
                if (hdnGroupId.Value != "")
                {
                    BaseC.Security objSec = new BaseC.Security(sConString);
                    string strErrorStatus = objSec.UpdateGroupDetails(Convert.ToInt16(hdnGroupId.Value), chkIsAdmin.Checked, GroupName, Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["UserID"]));
                    if (strErrorStatus == "Updated")
                    {
                        lblMessage.Text = "Group detail updated";
                        pnlUpdate.Visible = false;
                        btnUpdate.Visible = false;
                        btnEdit.Visible = true;
                        txtGroupName.Text = string.Empty;
                        BindGroups();
                    }
                    else
                    {
                        btnCreateGroup.Visible = true;
                        lblMessage.Text = "Sorry! Duplicate Group Name.";
                    }
                }
                BaseC.Security objSec1 = new BaseC.Security(sConString);
                //foreach (ListItem item in ddlGroupName.Items)
                //{
                //    if (item.Value != "0")
                //    {
                //        if (objSec1.IsAdmin(Convert.ToInt16(item.Value)))
                //        {
                //            item.Attributes.Add("Style", "background-color:Pink;");
                //        }
                //    }
                //}
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnAll_Click(object sender, EventArgs e)
    {
        try
        {
            //gvSecurity.DataSource = null;
            //gvSecurity.DataBind();
            for (int i = 0; i < RadgvPages.Items.Count; i++)
            {
                HiddenField hdnPageID = ((HiddenField)(RadgvPages.Items[i].FindControl("hdnPageID")));
                HiddenField hdnModuleID = ((HiddenField)(RadgvPages.Items[i].FindControl("hdnModuleID")));
                HiddenField hdnModuleName = ((HiddenField)(RadgvPages.Items[i].FindControl("hdnModuleName")));
                Label lbl_PageName = ((Label)(RadgvPages.Items[i].FindControl("lbl_PageName")));

                AddPages("0", hdnPageID.Value, lbl_PageName.Text, hdnModuleID.Value, hdnModuleName.Value);
                //AddPages("0", gvPages.Rows[i].Cells[0].Text, gvPages.Rows[i].Cells[3].Text, gvPages.Rows[i].Cells[1].Text, gvPages.Rows[i].Cells[2].Text);
            }
            BaseC.Security objSec = new BaseC.Security(sConString);
            //foreach (ListItem item in ddlGroupName.Items)
            //{
            //    if (item.Value != "0")
            //    {
            //        if (objSec.IsAdmin(Convert.ToInt16(item.Value)))
            //        {
            //            item.Attributes.Add("Style", "background-color:Pink;");
            //        }
            //    }
            //}
            gvSecurity.Rebind();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvSecurity_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == GridItemType.Pager)
            {
                CheckBox chkInsertData = (CheckBox)e.Item.FindControl("chkInsertData");
                CheckBox chkEditData = (CheckBox)e.Item.FindControl("chkEditData");
                CheckBox chkCancelData = (CheckBox)e.Item.FindControl("chkCancelData");
                CheckBox chkDisplayData = (CheckBox)e.Item.FindControl("chkDisplayData");
                CheckBox chkPrintData = (CheckBox)e.Item.FindControl("chkPrintData");
                LinkButton btnEdit = (LinkButton)e.Item.FindControl("btnEdit");

                if (Session["AddedRows"] != null)
                {
                    char[] ch = { ',' };
                    string str = Session["AddedRows"].ToString();
                    string[] strArr = str.Split(ch);
                    if (strArr.Length > 0)
                    {
                        for (int i = 0; i < strArr.Length; i++)
                        {
                            ImageButton lnk1 = (ImageButton)e.Item.FindControl("btnDelete");
                            ImageButton lnk = (ImageButton)e.Item.FindControl("btnDeActivate");

                            if (e.Item.Cells[1].Text.Trim() == strArr[i])
                            {
                                lnk1.Visible = true;
                            }
                            else
                            {
                                lnk.Visible = true;
                                chkInsertData.Enabled = false;
                                chkEditData.Enabled = false;
                                chkPrintData.Enabled = false;
                                chkCancelData.Enabled = false;
                                chkDisplayData.Enabled = false;
                            }
                            if (lnk1.Visible == true)
                            {
                                lnk.Visible = false;
                                chkInsertData.Enabled = true;
                                chkEditData.Enabled = true;
                                chkPrintData.Enabled = true;
                                chkCancelData.Enabled = true;
                                chkDisplayData.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        ImageButton lnk1 = (ImageButton)e.Item.FindControl("btnDeActivate");
                        lnk1.Visible = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvSecurity_ItemCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            CheckBox chkInsert = (CheckBox)e.Item.FindControl("chkInsertData");
            CheckBox chkEditData = (CheckBox)e.Item.FindControl("chkEditData");
            CheckBox chkCancelData = (CheckBox)e.Item.FindControl("chkCancelData");
            CheckBox chkDisplayData = (CheckBox)e.Item.FindControl("chkDisplayData");
            CheckBox chkPrintData = (CheckBox)e.Item.FindControl("chkPrintData");

            if (e.CommandName == "Delete")
            {
                if (Cache["UserPermissions" + Session["UserID"].ToString()] != null)
                {
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                    Label lblpid = (Label)e.Item.FindControl("lblpageid");
                    int Id = Convert.ToInt32(lblpid.Text);

                    long grpID = 0;
                    if (ddlGroupName.SelectedValue != null)
                    {
                        grpID = Convert.ToInt32(ddlGroupName.SelectedValue);
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Please select group name !", Page);
                    }
                    if (ViewState["gvSecurity"] != null)
                    {
                        DataTable dt = new DataTable();
                        DataSet ds = (DataSet)ViewState["gvSecurity"];
                        dt = ds.Tables[0];
                        DataRow drCurrentRow = null;
                        if (e.Item is GridDataItem)
                        {
                            GridDataItem item = (GridDataItem)e.Item;
                            var itemIndex = item.ItemIndex;
                            if (dt.Rows.Count > 1)
                            {
                                DataView DV = dt.DefaultView;
                                DV.RowFilter = "Pageid <>" + Id;
                                dt = DV.ToTable();
                                //dt.Rows.Remove(dt.Rows[itemIndex]);
                                //drCurrentRow = dt.NewRow();
                               
                                DataSet dsnew = new DataSet();
                                dsnew.Tables.Add(dt);
                                ViewState["gvSecurity"] = dsnew;
                                ViewState["CurrentTable"] = dt;
                                gvSecurity.DataSource = dt;
                                gvSecurity.DataBind();
                            }
                        }
                    }


                  
                        dl.ExecuteNonQuery(CommandType.Text, "update SecGroupPages set Active = 0 where PageID = " + Id + " and GroupID = " + grpID);
                    
                }
            }
            else if (e.CommandName == "CmdDeActivate")
            {
                if (ddlGroupName.SelectedValue != "0")
                {
                    DataSet objDs = CreateAddedTable(Convert.ToInt32(gvSecurity.Items[Convert.ToInt32(e.CommandArgument)].Cells[1].Text));
                    BaseC.Security objSec = new BaseC.Security(sConString);
                    objSec.DeActivePage(Convert.ToInt32(gvSecurity.Items[Convert.ToInt32(e.CommandArgument)].Cells[1].Text), Convert.ToInt32(ddlGroupName.SelectedValue));
                    gvSecurity.DataSource = objDs;
                    gvSecurity.DataBind();
                    lblMessage.Text = "Page DeActivated!";
                }
            }
            else if (e.CommandName == "Edit")
            {

                chkInsert.Enabled = true;
                chkEditData.Enabled = true;
                chkCancelData.Enabled = true;
                chkDisplayData.Enabled = true;
                chkPrintData.Enabled = true;
            }
            else if (e.CommandName == "Cancel")
            {
                chkInsert.Enabled = false;
                chkEditData.Enabled = false;
                chkCancelData.Enabled = false;
                chkDisplayData.Enabled = false;
                chkPrintData.Enabled = false;
            }
            else if (e.CommandName == "cmdUpdate")
            {
                BaseC.Security objSec = new BaseC.Security(sConString);
                int Id = Convert.ToInt32(gvSecurity.Items[Convert.ToInt32(e.CommandArgument)].Cells[0].Text);
                int i = objSec.updatePageRights(Id, chkInsert.Checked, chkEditData.Checked, chkCancelData.Checked, chkDisplayData.Checked, chkPrintData.Checked);
                chkInsert.Enabled = false;
                chkEditData.Enabled = false;
                chkCancelData.Enabled = false;
                chkDisplayData.Enabled = false;
                chkPrintData.Enabled = false;
            }
           
                BindRightsGrid();
           
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnGroupDepartment_Click(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            if (common.myInt(ddlGroupName.SelectedValue) < 1)
            {
                lblMessage.Text = "Please Select Group !";
                return;
            }

            RadWindowForNew.NavigateUrl = "/MPages/GroupDepartmentsTagging.aspx?GroupId=" + common.myInt(ddlGroupName.SelectedValue) + "&GroupName=" + common.myStr(ddlGroupName.SelectedItem.Text);

            RadWindowForNew.Height = 500;
            RadWindowForNew.Width = 800;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = ""; //"OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnDashboardPermission_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

        if (common.myInt(ddlGroupName.SelectedValue) < 1)
        {
            lblMessage.Text = "Please Select Group !";
            return;
        }

        RadWindowForNew.NavigateUrl = "/MPages/DashboardPermission.aspx?GroupId=" + common.myInt(ddlGroupName.SelectedValue) + "&GroupName=" + common.myStr(ddlGroupName.SelectedItem.Text);

        RadWindowForNew.Height = 500;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = ""; //"OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    //Done By Ujjwal 13April2015 to Add control over menu options on the basis of user groups start
    protected void btnGroupOptionEdit_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";

            if (common.myInt(ddlGroupName.SelectedValue) < 1)
            {
                lblMessage.Text = "Please Select Group !";
                return;
            }

            RadWindowForNew.NavigateUrl = "/MPages/GroupMenuTagging.aspx?GroupId=" + common.myInt(ddlGroupName.SelectedValue) + "&GroupName=" + common.myStr(ddlGroupName.SelectedItem.Text);

            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = ""; //"OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    //Done By Ujjwal 13April2015 to Add control over menu options on the basis of user groups end
}