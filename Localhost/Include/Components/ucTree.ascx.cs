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
using System.Data.SqlClient;

public partial class Include_Components_ucTree : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshInput;
    DL_Funs fun;

    protected void Page_Load(object sender, EventArgs e)
    {
        fun = new DL_Funs();
        if (!IsPostBack)
        {
            BaseC.EMR objEmr = new BaseC.EMR(sConString);
           
            BindModuleGrid();
            //ltrlYear.Text = System.DateTime.Now.Year.ToString();
        }
        if (Session["ModuleId"] == null)
            gvModules.SelectedIndex = 0;
        else
            gvModules.SelectedIndex = Convert.ToInt32(Session["ModuleId"]);
        if (gvModules.Rows.Count > 0)
        {
            BindTree();
            fun.DoSelectCurrentNode(tvCategory);
        }
    }

    protected void BindModuleGrid()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("intUserId", Convert.ToInt32(Session["UserID"]));
            hshInput.Add("intGroupId", Convert.ToInt32(Session["GroupID"]));
            DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetUserModule", hshInput);
            if (objDs.Tables[0].Rows.Count > 0)
            {
                //sModuleName.Text = objDs.Tables[0].Rows[0]["ModuleName"].ToString();
                //hdnModuleId.Value = objDs.Tables[0].Rows[0]["ModuleID"].ToString();
                gvModules.DataSource = objDs;
                gvModules.DataBind();

            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    protected void DoSelectNodeViaOnClickMasterPage()
    {
        try
        {
            foreach (TreeNode node in tvCategory.Nodes)
            {
                node.Expand();
                if (node.ChildNodes.Count > 0)
                {
                    node.ChildNodes[0].Select();
                    node.ChildNodes[0].Text = "<b>" + node.ChildNodes[0].Text + "</b>";
                }
                else
                {
                    node.Select();
                    node.Text = "<b>" + node.Text + "</b>";
                }
                string[] stringSeparators_ShowDia = new string[] { "showDia('" };
                string[] stringSeparators_aspx = new string[] { "') href" };
                string[] result1 = node.Text.Split(stringSeparators_ShowDia, StringSplitOptions.None);
                string[] result2 = { "" };
                if (result1.Length > 1)
                    result2 = result1[1].Split(stringSeparators_aspx, StringSplitOptions.None);
                Session["CurrentNode"] = node.Value;
                if (result1.Length > 1)
                    Response.Redirect(result2[0], false);
                break;
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindTree();
        DoSelectNodeViaOnClickMasterPage();
    }

  
    protected void BindTree()
    {
        if (Session["DefaultID"] != null)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strModuleName = "Select smp.ModuleID,smp.PageTitle From Users ur Inner Join SecModulePages smp ON ur.DefaultPageId = smp.PageID And smp.Active = 1 Where ISNULL(smp.IsPopupPage,0) <> 1 AND ur.Id = " + Session["UserID"] + "";
            DataSet dsMod = new DataSet();
            dsMod = objDl.FillDataSet(CommandType.Text, strModuleName);
            if (dsMod.Tables[0].Rows.Count > 0)
            {
                sModuleName.Text = dsMod.Tables[0].Rows[0]["PageTitle"].ToString();
                fun.BindPages(Convert.ToInt16(dsMod.Tables[0].Rows[0]["ModuleID"].ToString()), tvCategory);
            }
            gvModules.SelectedIndex = Convert.ToInt16(dsMod.Tables[0].Rows[0]["ModuleID"].ToString());
            Session["DefaultID"] = null;
            return;
        }
        //if (common.myInt(Session["ModuleId"]) == 14)
        //{
        //    sModuleName.Text = common.myStr(Session["ModuleName"]);
        //    //Session["ModuleId"] = gvModules.SelectedRow.RowIndex;
        //    fun.BindPages(Convert.ToInt16(Session["ModuleId"]), tvCategory);
        //    //Session["ModuleName"]=null;
        //    Session["ModuleId"] = null;
        //}

        //else
        //{
        if (Session["ModuleName"] != null)
        {
            if (Session["ModuleName"].ToString().Trim() == "Registration")
            {
                sModuleName.Text = Session["ModuleName"].ToString();
                //Session["ModuleId"] = gvModules.SelectedIndex;
                fun.BindPages(Convert.ToInt16(Session["ModuleId"]), tvCategory);
                Session["ModuleName"] = null;
            }
            //else if (Session["ModuleName"].ToString().Trim() == "ATD")
            //{
            //    sModuleName.Text = Session["ModuleName"].ToString();
            //    //Session["ModuleId"] = gvModules.SelectedIndex;
            //    fun.BindPages(Convert.ToInt16(Session["ModuleId"]), tvCategory);
            //    Session["ModuleName"] = null;
            //}
            else
            {

                sModuleName.Text = gvModules.SelectedRow.Cells[1].Text;

                Session["ModuleId"] = gvModules.SelectedRow.RowIndex;
                fun.BindPages(Convert.ToInt16(gvModules.SelectedRow.Cells[0].Text), tvCategory);

            }
        }
        else
        {
            sModuleName.Text = gvModules.SelectedRow.Cells[1].Text;
            Session["ModuleId"] = gvModules.SelectedRow.RowIndex;
            //Session["ModuleName"] = common.myStr(gvModules.SelectedRow.Cells[1].Text);
            fun.BindPages(Convert.ToInt16(gvModules.SelectedRow.Cells[0].Text), tvCategory);

        }
        // }
    }

    protected void tvCategory_SelectedNodeChanged(Object sender, EventArgs e)
    {

    }



    protected void gvModules_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvModules, "Select$" + e.Row.RowIndex);
        }
    }

    protected void gvModulePages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].Visible = false;
        }
    }

    protected void gvModulePages_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

}
