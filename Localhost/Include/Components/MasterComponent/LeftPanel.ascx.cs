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

public partial class Include_Components_MasterComponent_LeftPanel : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindModuleGrid();
            if (Session["ModuleId"] == null)
            {
                gvModules.SelectedIndex = 0;
                gvModules_SelectedIndexChanged(this, null);
            }
            else
            {
                gvModules.SelectedIndex = Convert.ToInt32(Session["ModuleId"]);
                gvModules_SelectedIndexChanged(this, null);
            }
            // ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "javascript:HideLeftPnl();", true);
        }
    }

    protected void BindModuleGrid()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("intUserId", Convert.ToInt32(Session["UserID"]));
            hshInput.Add("intGroupId", Convert.ToInt32(Session["GroupID"]));
            DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetUserModule", hshInput);
            gvModules.DataSource = objDs;
            gvModules.DataBind();
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindPages(Int16 iModuleId)
    {
        try
        {
            tvCategory.Nodes.Clear();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("inyModuleID", iModuleId);
            hshInput.Add("intUserId", Convert.ToInt32(Session["UserID"]));
            hshInput.Add("intGroupId", Convert.ToInt32(Session["GroupID"]));
            DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModulePages", hshInput);

            for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
            {
                AddNodes(tvCategory, Convert.ToInt32(objDs.Tables[0].Rows[i][0]), Convert.ToInt32(objDs.Tables[0].Rows[i][2]), Convert.ToString(objDs.Tables[0].Rows[i][1]), Convert.ToString(objDs.Tables[0].Rows[i][5]));
            }
            if (tvCategory.Nodes.Count > 0)
            {
                //tvCategory.Nodes[0].Selected = true;
                tvCategory.CollapseAll();
                DoSelectCurrentNode();
                tvCategory.PopulateNodesFromClient = true;
                tvCategory.ShowLines = true;
                tvCategory_SelectedNodeChanged(this, null);
            }
            else
            {
                //lbtnNewCategory_OnClick(this, null);
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void DoSelectCurrentNode()
    {
        try
        {
            if (Session["CurrentNode"] != null)
            {
                foreach (TreeNode node in tvCategory.Nodes)
                {
                    if (node.Value == Session["CurrentNode"].ToString())
                    {
                        node.Expand();
                        node.Select();
                        break;
                    }
                    foreach (TreeNode n in node.ChildNodes)
                    {
                        if (Session["CurrentNode"] != null)
                        {
                            if (n.Value == Session["CurrentNode"].ToString())
                            {
                                n.Expand();
                                n.Text = n.Text;
                                n.Select();
                                n.Parent.Expand();
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void tvCategory_SelectedNodeChanged(Object sender, EventArgs e)
    {

    }

    private void AddNodes(TreeView tvName, Int32 iNodeID, Int32 iParentID, String sNodeText, string sPageUrl)
    {
        try
        {
            txtLeftPnl.Text = "1";
            if (txtLeftPnl.Text == "1")
                ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "javascript:ShowLeftPnl();", true);
            else
                ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "javascript:HideLeftPnl();", true);

            if (iParentID == 0)
            {
                TreeNode masternode;
                if (sPageUrl != "")
                {
                    if (!sPageUrl.Contains("?"))
                    {
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", sPageUrl + "?Mpg=" + "P" + iNodeID.ToString(), "");

                        //if (masternode.Text == "Appointment")
                        //{
                        //    masternode.Text = "<a onclick=javascript:window.open('/Appointment/AppointmentFullScreen.aspx','','resizable=yes,scrollbars=yes,width=1040,Height=850,left=0,top=0'); return false; href='#' style='text-decoration:none; color:white;' >Appointment</a>";
                        //}

                    }
                    else
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "P" + iNodeID.ToString(), "");
                }
                else
                {
                    if (!sPageUrl.Contains("?"))
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", "");
                    else
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", "");
                }
                //DoSelectCurrentNode(masternode);
                tvName.Nodes.Add(masternode);
            }
            else
            {
                TreeNode masternode = new TreeNode();
                masternode = tvName.FindNode("P" + iParentID.ToString());
                if (masternode != null)
                {
                    TreeNode childNode;
                    if (!sPageUrl.Contains("?"))
                        childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "?Mpg=" + "C" + iNodeID.ToString(), "");
                    else
                        childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "C" + iNodeID.ToString(), "");
                    //DoSelectCurrentNode(childNode);
                    masternode.ChildNodes.Add(childNode);
                }
                else
                {
                    CallRecursive(tvName, iNodeID, "C" + iParentID, sNodeText, sPageUrl);
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void CallRecursive(TreeView tvName, Int32 iNodeID, String sParentID, String sNodeText, string sPageUrl)
    {
        TreeNodeCollection nodes = tvName.Nodes;
        foreach (TreeNode n in nodes)
        {
            ReCallRecursive(n, iNodeID, sParentID, sNodeText, sPageUrl);
        }
    }

    private void ReCallRecursive(TreeNode treeNode, Int32 iNodeID, String sParentID, String sNodeText, string sPageUrl)
    {
        foreach (TreeNode tn in treeNode.ChildNodes)
        {
            if (tn.Value == sParentID.ToString())
            {
                TreeNode childNode;
                if (!sPageUrl.Contains("?"))
                    childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "?Mpg=" + "C" + iNodeID.ToString(), "");
                else
                    childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "C" + iNodeID.ToString(), "");
                //DoSelectCurrentNode(childNode);
                tn.ChildNodes.Add(childNode);
            }
            ReCallRecursive(tn, iNodeID, sParentID, sNodeText, sPageUrl);
        }
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

    protected void gvModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // sModuleName.InnerHtml = gvModules.SelectedRow.Cells[1].Text;
            sModuleName.Text = gvModules.SelectedRow.Cells[1].Text;
            Session["ModuleId"] = gvModules.SelectedRow.RowIndex;
            BindPages(Convert.ToInt16(gvModules.SelectedRow.Cells[0].Text));
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
