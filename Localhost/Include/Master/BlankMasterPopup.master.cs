using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web;
using Telerik.Web.UI;
using System.IO;
using System.Drawing;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class Include_Master_BlankMasterPopup : System.Web.UI.MasterPage
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //private string UtdLink = ConfigurationManager.ConnectionStrings["UTDLink"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    private Hashtable hshInput;
    //clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
            if (!Page.IsCallback)
            {
                this.AddKeepAlive();
                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                }
            }
            if (common.myLen(Request.QueryString["Mpg"]) > 0)
            {
                Session["CurrentNode"] = common.myStr(Request.QueryString["Mpg"]);
            }
            if (common.myInt(Request.QueryString["EncId"]) > 0)
            {
                Session["EncounterId"] = common.myInt(Request.QueryString["EncId"]);
            }
            if (common.myInt(Request.QueryString["mdlId"]) > 0)
            {
                Session["ModuleId"] = common.myInt(Request.QueryString["mdlId"]);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "PageInit");
            objException = null;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null || common.myInt(Session["UserId"]) == 0)
        {
            Response.Redirect("~/Login.aspx?Logout=1", false);
        }

        string notificationCheck = "Y";

        if (common.myStr(Session["EmployeeType"]) == "D")
        { notificationCheck = "N"; }
        else if (common.myStr(Session["EmployeeType"]) == "N")
        { notificationCheck = "N"; }
        else if (common.myStr(Session["EmployeeType"]) == "LD")
        { notificationCheck = "N"; }
        else if (common.myStr(Session["EmployeeType"]) == "LDIR")
        {
            notificationCheck = "N";
        }
        if (notificationCheck == "Y")
        { NotificationCheck(); }

        DataSet objDs = new DataSet();
        try
        {
            if (!IsPostBack)
            {
                dvNotification.Visible = false;
                if (common.myStr(Session["EmployeeType"]) == "D")
                {
                    if (!common.myBool(Session["FindPatientExpanded"]))
                    {
                        Radslidingzone2.ExpandedPaneId = "rdpAppList";
                        Session["FindPatientExpanded"] = true;
                    }
                    else
                    {
                        Radslidingzone2.ExpandedPaneId = "";
                    }
                }
                if (Application["HospitalName"] != null)
                   // Page.Header.Title = Application["HospitalName"].ToString();
                if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null) || (Session["mainMenu"] == null) || (Session["MenuData"] == null))
                {
                    ExtractUserMenuFromSession();
                }

                BindModuleGrid();
                if (Session["ModuleId"] != null && common.myInt(Session["ModuleId"]) > 0)
                {
                    int mid = 0;
                    int ModIndex = 0;

                    for (ModIndex = 0; ModIndex < gvModules.Rows.Count; ModIndex++)
                    {

                        if (gvModules.Rows[ModIndex].Cells[0].Text == common.myStr(Session["ModuleId"]))
                        {
                            mid = ModIndex;
                            break;
                        }
                    }
                    if (mid == 0)
                    {
                        gvModules.SelectedIndex = 0;
                        Session["ModuleId"] = gvModules.Rows[0].Cells[0].Text;
                    }
                    else
                    {
                        gvModules.SelectedIndex = mid;
                    }
                }
                else
                {
                    gvModules.SelectedIndex = 0;
                    Session["ModuleId"] = gvModules.Rows[0].Cells[0].Text;
                }
                if (gvModules.Rows.Count > 0)
                {
                    Session["ModuleName"] = gvModules.SelectedRow.Cells[1].Text;
                    Session["ModuleFlag"] = gvModules.SelectedRow.Cells[3].Text;
                    BindTree();
                    DoSelectCurrentNode(tvCategory);
                }
                if ((Session["RegistrationId"] != null && Session["RegistrationId"].ToString() != ""
                    && common.myStr(Session["EncounterID"]) != null) &&
                    (common.myStr(Session["ModuleName"]) == "Nurse Workbench") || (common.myStr(Session["ModuleName"]) == "EMR"))
                {
                    //BaseC.Patient objPatient = new BaseC.Patient(sConString);
                    //if (common.myStr(Session["RegistrationID"]) != "&nbsp;")
                    //{
                    //    SqlDataReader objDr = (SqlDataReader)objPatient.getPatientShortDetail(common.myInt(Session["RegistrationID"]), common.myInt(Session["EncounterId"]));
                    //    try
                    //    {
                    //        if (objDr.Read())
                    //        {
                    //            Radslidingpane4.Visible = true;
                    //            if (common.myLen(objDr["Name"]) > 30)
                    //            {
                    //                Radslidingpane4.Title = "<b><font name='Tahoma'>" + common.myStr(objDr["Name"]).Trim().Substring(0, 30) + "</font></b>";
                    //            }
                    //            else
                    //            {
                    //                Radslidingpane4.Title = "<b><font name='Tahoma'>" + common.myStr(objDr["Name"]).Trim() + "</font></b>";
                    //            }
                    //        }
                    //    }
                    //    catch
                    //    {
                    //    }
                    //    finally
                    //    {
                    //        objDr.Close();
                    //    }
                    //}

                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getPatientShortDetail";

                    APIRootClass.getPatientShortDetail objRoot = new global::APIRootClass.getPatientShortDetail();

                    objRoot.RegistrationId = common.myInt(Session["RegistrationID"]);
                    objRoot.EncounterId = common.myInt(Session["EncounterId"]);

                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;

                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
                    if (objDs != null && objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                    {
                        Radslidingpane4.Visible = true;
                        if (common.myLen(objDs.Tables[0].Rows[0]["Name"]) > 30)
                        {
                            Radslidingpane4.Title = "<b><font name='Tahoma'>" + common.myStr(objDs.Tables[0].Rows[0]["Name"]).Trim().Substring(0, 30) + "</font></b>";
                        }
                        else
                        {
                            Radslidingpane4.Title = "<b><font name='Tahoma'>" + common.myStr(objDs.Tables[0].Rows[0]["Name"]).Trim() + "</font></b>";
                        }
                    }
                }
                ShowMenuItems();
                if(Session["LastRedirecetd"]==null)
                {  
                if (CheckQueries() > 0)
                {     
                     
                        Response.Redirect("default.aspx?Qry=1", false);
                        Session["LastRedirecetd"] = DateTime.Now;
                    
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objDs.Dispose();
        }
    }
    int CheckQueries()
    {
        int i = 0;
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //i = common.myInt(dl.ExecuteScalar(CommandType.Text, "select count(*) from InsuranceQMS with(nolock) where isnull(QueryClosed,0)=0 and ToUser =" + Session["EmployeeID"]));
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/Login/CheckQueries";
            APIRootClass.CheckQueries objRoot = new global::APIRootClass.CheckQueries();
            objRoot.EmployeeId = common.myInt(Session["EmployeeID"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            i = JsonConvert.DeserializeObject<int>(sValue);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "CheckQueries");
            objException = null;
        }
        return i;
    }
    protected void BindModuleGrid()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataSet dsModule = new DataSet();

        try
        {
            if (common.myStr(Session["mainMenu"]) != "")
            {
                dt = (DataTable)Session["mainMenu"];
                gvModules.DataSource = dt;
                gvModules.DataBind();
                DataRow[] foundisEMR = dt.Select("ModuleId in (3,44)");
                if (foundisEMR.Length > 0)
                {
                    Session["isEMR"] = "1";
                }
                else
                {
                    Session["isEMR"] = "0";
                }

                if (Session["ModuleData"] == null)
                {
                    ds.Tables.Add(dt);
                    Session["ModuleData"] = ds;
                }
                else
                {
                    dsModule = (DataSet)Session["ModuleData"];
                    if (dsModule == null || dsModule.Tables.Count == 0)
                    {
                        Session["ModuleData"] = ds;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "BindModuleGrid");
            objException = null;
        }
        finally
        {
            ds.Dispose();
            dsModule.Dispose();
            dt.Dispose();
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
                {
                    Response.Redirect(result2[0], false);
                    break;
                }

            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "DoSelectNodeViaOnClickMasterPage");
            objException = null;
        }
    }
    protected void gvModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvModules.SelectedRow != null)
            {
                Session["StationId"] = 0;
                Session["ModuleId"] = gvModules.SelectedRow.Cells[0].Text;
                Session["ModuleName"] = gvModules.SelectedRow.Cells[1].Text;
                Session["ModuleFlag"] = gvModules.SelectedRow.Cells[3].Text;
                BindTree();
                DoSelectNodeViaOnClickMasterPage();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "gvModules_SelectedIndexChanged");
            objException = null;
        }

    }
    protected void BindTree()
    {
        try
        {
            sModuleName.Text = common.myStr(Session["ModuleName"]);
            BindPages(Convert.ToInt16(gvModules.SelectedRow.Cells[0].Text), tvCategory);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "BindTree");
            objException = null;
        }
    }
    protected void tvCategory_SelectedNodeChanged(Object sender, EventArgs e)
    {

    }
    protected void gvModules_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvModules, "Select$" + e.Row.RowIndex);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "gvModules_RowDataBound");
            objException = null;
        }
    }
    public void BindPages(Int16 iModuleId, TreeView tvCategory)
    {
        DataTable objDt = new DataTable();
        DataSet objDs = new DataSet();
        DataSet dsPageCheck = new DataSet();
        String strTickSign = "";
        tvCategory.Nodes.Clear();
        DataTable dtm = new DataTable();
        DataView dv = new DataView();
        try
        {
            if (common.myStr(Session["MenuData"]) != "")
            {
                objDt = ((DataTable)Session["MenuData"]).Copy();
                if (objDt.Rows.Count > 0)
                {
                    dv = objDt.DefaultView;
                    dv.RowFilter = "ModuleId = " + iModuleId.ToString();

                    dtm = dv.ToTable();

                    objDs.Tables.Add(dtm);


                    for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                    {
                        AddNodes(tvCategory, common.myInt(objDs.Tables[0].Rows[i][1]), common.myInt(objDs.Tables[0].Rows[i][3]), common.myStr(objDs.Tables[0].Rows[i][2]), common.myStr(objDs.Tables[0].Rows[i][6]), strTickSign);
                    }
                    if (tvCategory.Nodes.Count > 0)
                    {
                        tvCategory.ExpandAll();
                        tvCategory.PopulateNodesFromClient = true;
                        tvCategory.ShowLines = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "BindPages");
            objException = null;
        }
        finally
        {
            objDt.Dispose();
            objDs.Dispose();
            dsPageCheck.Dispose();
            dv.Dispose();
        }
    }
    public void DoSelectCurrentNode(TreeView tvCategory)
    {
        try
        {
            if (HttpContext.Current.Session["CurrentNode"] != null)
            {
                foreach (TreeNode node in tvCategory.Nodes)
                {
                    if (node.Value == HttpContext.Current.Session["CurrentNode"].ToString())
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
                        break;
                    }
                    foreach (TreeNode n in node.ChildNodes)
                    {
                        if (HttpContext.Current.Session["CurrentNode"] != null)
                        {
                            if (n.Value == HttpContext.Current.Session["CurrentNode"].ToString())
                            {
                                n.Expand();
                                n.Text = n.Text;
                                n.Select();
                                n.Parent.Expand();
                                n.Text = "<b>" + n.Text + "</b>";
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "DoSelectCurrentNode");
            objException = null;
        }
    }
    public void AddNodes(TreeView tvName, Int32 iNodeID, Int32 iParentID, String sNodeText, string sPageUrl, String sShowTickSign)
    {
        try
        {
            if (iParentID == 0)
            {
                TreeNode masternode;
                if (sPageUrl != "")
                {
                    if (!common.myStr(sPageUrl).ToUpper().Contains("MPG"))
                    {
                        if (!sPageUrl.Contains("?"))
                        {
                            masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", "");
                            masternode.Text = "<a target='_self' onclick=showDia('" + sPageUrl + "?Mpg=" + "P" + iNodeID.ToString() + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                        else
                        {
                            masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "P" + iNodeID.ToString(), "");
                            masternode.Text = "<a target='_self' onclick=showDia('" + sPageUrl + "&Mpg=" + "P" + iNodeID.ToString() + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                    }
                    else
                    {
                        if (!sPageUrl.Contains("?"))
                        {
                            masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", "");
                            masternode.Text = "<a target='_self' onclick=showDia('" + sPageUrl + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                        else
                        {
                            masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", sPageUrl, "");
                            masternode.Text = "<a target='_self' onclick=showDia('" + sPageUrl + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                    }
                }
                else
                {
                    if (!sPageUrl.Contains("?"))
                        masternode = new TreeNode(sNodeText.ToString() + sShowTickSign, "P" + iNodeID.ToString(), "", "", "");
                    else
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", "");
                }
                tvName.Nodes.Add(masternode);
            }
            else
            {
                TreeNode masternode = new TreeNode();
                masternode = tvName.FindNode("P" + iParentID.ToString());
                if (masternode != null)
                {
                    TreeNode childNode;
                    if (sPageUrl.ToUpper().Contains("MPG"))
                    {
                        if (!sPageUrl.Contains("?"))
                        {
                            childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", "", "");
                            childNode.Text = "<a target='_self' onclick=showDia('" + sPageUrl + "?Mpg=" + "C" + iNodeID.ToString() + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                        else
                        {
                            childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "C" + iNodeID.ToString(), "");
                            childNode.Text = "<a target='_self' onclick=showDia('" + sPageUrl + "&Mpg=" + "C" + iNodeID.ToString() + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                    }
                    else
                    {
                        if (!sPageUrl.Contains("?"))
                        {
                            childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", "", "");
                            childNode.Text = "<a target='_self' onclick=showDia('" + sPageUrl + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                        else
                        {
                            childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "C" + iNodeID.ToString(), "");
                            childNode.Text = "<a target='_self' onclick=showDia('" + sPageUrl + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                    }
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
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "DoSelectCurrentNode");
            objException = null;
        }
    }
    private void CallRecursive(TreeView tvName, Int32 iNodeID, String sParentID, String sNodeText, string sPageUrl)
    {
        try
        {
            TreeNodeCollection nodes = tvName.Nodes;
            foreach (TreeNode n in nodes)
            {
                ReCallRecursive(n, iNodeID, sParentID, sNodeText, sPageUrl);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "CallRecursive");
            objException = null;
        }
    }
    private void ReCallRecursive(TreeNode treeNode, Int32 iNodeID, String sParentID, String sNodeText, string sPageUrl)
    {
        try
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
                    tn.ChildNodes.Add(childNode);
                }
                ReCallRecursive(tn, iNodeID, sParentID, sNodeText, sPageUrl);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "ReCallRecursive");
            objException = null;
        }
    }
    public void ShowMenuItems()
    {
        DataTable dt = new DataTable();
        DataTable tbl = new DataTable();
        DataView DV = new DataView();
        try
        {
            if (common.myStr(Session["MenuData"]) != "" && Session["MenuData"] != null)
            {
                dt = ((DataTable)Session["MenuData"]).Copy();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        DV = dt.DefaultView;

                        DV.RowFilter = "";
                        DV.Sort = "";

                        DV.RowFilter = "ModuleId=" + common.myInt(Session["ModuleId"]);
                        DV.Sort = "Hierarchy,Sequence ASC";

                        tbl = DV.ToTable().Copy();

                        if (tbl.Rows.Count > 0)
                        {
                            tbl.AcceptChanges();

                            for (int rIdx = 0; rIdx < tbl.Rows.Count; rIdx++)
                            {
                                if (common.myInt(tbl.Rows[rIdx]["ParentId"]) < 1)
                                {
                                    tbl.Rows[rIdx]["ParentId"] = DBNull.Value;
                                    tbl.AcceptChanges();
                                }
                                string CurrentPageURL = common.myStr(tbl.Rows[rIdx]["PageURL"]).Trim();
                                if (common.myLen(CurrentPageURL) > 0)
                                {
                                    if (!common.myStr(tbl.Rows[rIdx]["PageURL"]).ToUpper().Contains("MPG"))
                                    {
                                        if (common.myStr(tbl.Rows[rIdx]["PageURL"]).Contains("?"))
                                        {
                                            tbl.Rows[rIdx]["PageURL"] = CurrentPageURL + "&Mpg=P" + common.myInt(tbl.Rows[rIdx]["PageId"]);
                                        }
                                        else
                                        {
                                            tbl.Rows[rIdx]["PageURL"] = CurrentPageURL + "?Mpg=P" + common.myInt(tbl.Rows[rIdx]["PageId"]);
                                        }
                                    }
                                }
                            }

                            RadMenu1.DataSource = tbl;
                            //Establish hierarchy: 
                            RadMenu1.DataFieldID = "PageId";
                            RadMenu1.DataFieldParentID = "ParentId";

                            //Set Text, Value, and NavigateUrl:  
                            RadMenu1.DataTextField = "PageName";
                            RadMenu1.DataValueField = "PageId";
                            RadMenu1.DataNavigateUrlField = "PageUrl";
                            RadMenu1.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "ShowMenuItems");
            objException = null;
        }
        finally
        {
            dt.Dispose();
            tbl.Dispose();
            DV.Dispose();
        }
    }
    protected void RadMenu1_ItemDataBound(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
        if (e.Item.Selected == true)
        {
            //Session["ModuleId"] = "";
        }
        //e.Item.ToolTip = "Read more about " +
        //(string)DataBinder.Eval(e.Item.DataItem, "Text");
    }
    public void ExtractUserMenuFromSession()
    {
        //BaseC.Security objSecurity = new BaseC.Security(sConString);
        DataTable dtMenuDetail = new DataTable();
        DataTable dtGroupDetail = new DataTable();
        DataTable dtModuleDetail = new DataTable();
        DataTable dtDistinctModuleId = new DataTable();
        DataTable dtDistinctModuleDetail = new DataTable();
        DataTable dtDistinctPageId = new DataTable();
        DataTable dtDistinctPageDetail = new DataTable();
        DataSet objDs = new DataSet();
        try
        {
            string[] columnNames = new string[1];

            if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null))
            {
                objDs = new DataSet();
                //objDs = objSecurity.getSecurityUserPages(); //uspSecurityGetUserPages

                string ServiceURL = WebAPIAddress.ToString() + "api/Login/getSecurityUserPages";

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(null);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                if (objDs.Tables.Count > 2)
                {
                    Session["ModuleDetail"] = objDs.Tables[0].Copy();
                    Session["GroupDetail"] = objDs.Tables[1].Copy();
                    Session["MenuDetail"] = objDs.Tables[2].Copy();
                }
            }
            
            dtMenuDetail = (DataTable)Session["MenuDetail"];
            dtGroupDetail = (DataTable)Session["GroupDetail"];
            dtModuleDetail = (DataTable)Session["ModuleDetail"];
            if ((dtMenuDetail.Rows.Count == 0) || (dtGroupDetail.Rows.Count == 0) || (dtModuleDetail.Rows.Count == 0))
            {
                 objDs = new DataSet();
                //objDs = objSecurity.getSecurityUserPages(); //uspSecurityGetUserPages

                string ServiceURL = WebAPIAddress.ToString() + "api/Login/getSecurityUserPages";

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(null);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                if (objDs.Tables.Count > 2)
                {
                    Session["ModuleDetail"] = objDs.Tables[0].Copy();
                    Session["GroupDetail"] = objDs.Tables[1].Copy();
                    Session["MenuDetail"] = objDs.Tables[2].Copy();
                }
                dtMenuDetail = (DataTable)Session["MenuDetail"];
                dtGroupDetail = (DataTable)Session["GroupDetail"];
                dtModuleDetail = (DataTable)Session["ModuleDetail"];
            }
            if (common.myBool(Session["IsAdminGroup"]))
            {
                try
                {
                    dtGroupDetail.DefaultView.RowFilter = "";
                    dtGroupDetail.DefaultView.RowFilter = "GroupId=" + common.myInt(Session["GroupId"]);
                    Session["PrintAuthentication"] = dtGroupDetail.DefaultView.ToTable();
                    dtGroupDetail.DefaultView.RowFilter = "";
                }
                catch
                {
                }
                columnNames = new string[1];
                columnNames[0] = "ModuleId";
                dtGroupDetail.DefaultView.RowFilter = "";
                dtDistinctModuleId = dtGroupDetail.DefaultView.ToTable(true, columnNames);
                if (dtDistinctModuleId.Rows.Count > 0)
                {
                    dtDistinctModuleDetail = dtModuleDetail.Clone();
                    foreach (DataRow dr in dtDistinctModuleId.Rows)
                    {
                        DataView dvModuleDetail = dtModuleDetail.DefaultView;
                        dvModuleDetail.RowFilter = "";
                        dvModuleDetail.RowFilter = "ModuleId=" + common.myInt(dr["ModuleId"]);
                        if (dvModuleDetail.Count > 0)
                        {
                            DataRow drSelected = dtDistinctModuleDetail.NewRow();
                            drSelected["ModuleId"] = common.myInt(dvModuleDetail[0]["ModuleId"]);
                            drSelected["ModuleName"] = common.myStr(dvModuleDetail[0]["ModuleName"]);
                            drSelected["Sequence"] = common.myInt(dvModuleDetail[0]["Sequence"]);
                            drSelected["ModuleFlag"] = common.myStr(dvModuleDetail[0]["ModuleFlag"]);
                            drSelected["ImageUrl"] = common.myStr(dvModuleDetail[0]["ImageUrl"]);
                            dtDistinctModuleDetail.Rows.Add(drSelected);
                        }
                    }
                }
                dtDistinctPageDetail = dtMenuDetail.Copy();

                if (dtDistinctModuleDetail.Rows.Count > 0)
                {
                    dtDistinctModuleDetail.DefaultView.Sort = "";
                    dtDistinctModuleDetail.DefaultView.Sort = "Sequence ASC";
                    Session["mainMenu"] = dtDistinctModuleDetail;
                }
                if (dtMenuDetail.Rows.Count > 0)
                {
                    dtMenuDetail.DefaultView.RowFilter = "IsPopupPage=False";
                    dtMenuDetail.DefaultView.Sort = "";
                    dtMenuDetail.DefaultView.Sort = "Hierarchy,Sequence ASC";
                    Session["MenuData"] = dtMenuDetail.DefaultView.ToTable().Copy();
                    Session["PharmacyIntimationRights"] = "Y";
                    Session["BillingIntimationRights"] = "Y";
                    Session["ICIntimationRights"] = "Y";
                    Session["BedTransferRequest"] = "Y";
                }
            }
            else
            {
                dtGroupDetail.DefaultView.RowFilter = "";
                dtGroupDetail.DefaultView.RowFilter = "GroupId=" + common.myInt(Session["GroupId"]);
                Session["PrintAuthentication"] = dtGroupDetail.DefaultView.ToTable();
                if (dtGroupDetail.Rows.Count > 0)
                {
                    columnNames = new string[1];
                    columnNames[0] = "ModuleId";
                    dtDistinctModuleId.DefaultView.RowFilter = "";
                    dtDistinctModuleId = dtGroupDetail.DefaultView.ToTable(true, columnNames);
                    if (dtDistinctModuleId.Rows.Count > 0)
                    {
                        dtDistinctModuleDetail = dtModuleDetail.Clone();
                        foreach (DataRow dr in dtDistinctModuleId.Rows)
                        {
                            DataView dvModuleDetail = dtModuleDetail.DefaultView;
                            dtModuleDetail.DefaultView.RowFilter = "";
                            dvModuleDetail.RowFilter = "ModuleId=" + common.myInt(dr["ModuleId"]);
                            if (dvModuleDetail.ToTable().Rows.Count > 0)
                            {
                                DataRow drSelected = dtDistinctModuleDetail.NewRow();
                                drSelected["ModuleId"] = common.myInt(dvModuleDetail.ToTable().Rows[0]["ModuleId"]);
                                drSelected["ModuleName"] = common.myStr(dvModuleDetail.ToTable().Rows[0]["ModuleName"]);
                                drSelected["Sequence"] = common.myInt(dvModuleDetail.ToTable().Rows[0]["Sequence"]);
                                drSelected["ModuleFlag"] = common.myStr(dvModuleDetail.ToTable().Rows[0]["ModuleFlag"]);
                                drSelected["ImageUrl"] = common.myStr(dvModuleDetail.ToTable().Rows[0]["ImageUrl"]);
                                dtDistinctModuleDetail.Rows.Add(drSelected);
                            }
                        }
                        dtGroupDetail.DefaultView.RowFilter = "";
                        dtGroupDetail.DefaultView.RowFilter = "GroupId=" + common.myInt(Session["GroupId"]);

                        if (dtGroupDetail.Rows.Count > 0)
                        {
                            dtDistinctPageDetail = dtMenuDetail.Clone();
                            foreach (DataRowView drv in dtGroupDetail.DefaultView)
                            {
                                DataView dvPageDetails = dtMenuDetail.DefaultView;
                                dvPageDetails.RowFilter = "";
                                dvPageDetails.RowFilter = "PageId=" + common.myInt(drv["PageId"]);
                                if (dvPageDetails.Count > 0)
                                {
                                    DataRow drSelected = dtDistinctPageDetail.NewRow();
                                    drSelected["ModuleId"] = common.myInt(dvPageDetails[0]["ModuleId"]);
                                    drSelected["PageId"] = common.myInt(dvPageDetails[0]["PageId"]);
                                    drSelected["PageName"] = common.myStr(dvPageDetails[0]["PageName"]);
                                    drSelected["ParentId"] = common.myInt(dvPageDetails[0]["ParentId"]);
                                    drSelected["Hierarchy"] = common.myStr(dvPageDetails[0]["Hierarchy"]);
                                    drSelected["Sequence"] = common.myInt(dvPageDetails[0]["Sequence"]);
                                    drSelected["PageUrl"] = common.myStr(dvPageDetails[0]["PageUrl"]);
                                    drSelected["IsPopupPage"] = common.myStr(dvPageDetails[0]["IsPopupPage"]);
                                    dtDistinctPageDetail.Rows.Add(drSelected);
                                }
                            }
                        }
                    }
                    if (dtDistinctModuleDetail.Rows.Count > 0)
                    {
                        dtDistinctModuleDetail.DefaultView.Sort = "Sequence Asc";
                        Session["mainMenu"] = dtDistinctModuleDetail;
                    }
                    if (dtDistinctPageDetail.Rows.Count > 0)
                    {
                        dtDistinctPageDetail.DefaultView.RowFilter = "IsPopupPage=False";
                        dtDistinctPageDetail.DefaultView.Sort = "";
                        dtDistinctPageDetail.DefaultView.Sort = "Hierarchy,Sequence ASC";
                        Session["MenuData"] = dtDistinctPageDetail.DefaultView.ToTable().Copy();

                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=PHR%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            Session["PharmacyIntimationRights"] = "Y";
                        }
                        else
                        {
                            Session["PharmacyIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=BILL%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            Session["BillingIntimationRights"] = "Y";
                        }
                        else
                        {
                            Session["BillingIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=IC%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            Session["ICIntimationRights"] = "Y";
                        }
                        else
                        {
                            Session["ICIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%/ATD/BedTransferRequest.aspx%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            Session["BedTransferRequest"] = "Y";
                        }
                        else
                        {
                            Session["BedTransferRequest"] = "N";
                        }

                        dtDistinctPageDetail.DefaultView.RowFilter = "";
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            //string lineNumber = Ex.StackTrace.Substring(Ex.StackTrace.Length - 7, 7);
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "ExtractUserMenuFromSession");
            objException = null;
        }
        finally
        {
            dtMenuDetail.Dispose();
            dtGroupDetail.Dispose();
            dtModuleDetail.Dispose();
            dtDistinctModuleId.Dispose();
            dtDistinctModuleDetail.Dispose();
            dtDistinctPageId.Dispose();
            dtDistinctPageDetail.Dispose();
            objDs.Dispose();
        }
    }

    public void btnReset_OnClick(object sender, EventArgs e)
    {
    }

    DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 1;
        dt.Columns["SNo"].AutoIncrementStep = 1;
        dt.Columns.Add("Notification");
        dt.Columns.Add("Count");
        dt.Columns.Add("LinkPage");
        dt.Columns.Add("Type");
        return dt;
    }
    protected void NotificationCheck()
    {
        DataTable dtGrid = new DataTable();
        DataSet ds = new DataSet("table1");
        try
        {
            string fileLoc = Server.MapPath("~/PatientDocuments/Notofication_SB" + common.myInt(Session["FacilityId"]) + ".txt");

            string read = common.ReadNotificationFile(fileLoc);

            string ShowNotification = "N";
            string PharmacyCount = "0";
            string BillingCount = "0", BPCount = "0", BTRCount = "0", OpenVisits = "0";
            string InsuranceCount = "0";
            string StatusType = "";


            if (read != "")
            {
                string xmlSchema = common.myStr(read);
                StringReader sr = new StringReader(xmlSchema);
                ds.ReadXml(sr);
                dtGrid = CreateTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataColumnCollection columns = ds.Tables[0].Columns;

                    if (columns.Contains("ShowNotification"))
                        ShowNotification = common.myStr(ds.Tables[0].Rows[0]["ShowNotification"]);
                    if (columns.Contains("SBCount"))
                        PharmacyCount = common.myStr(ds.Tables[0].Rows[0]["SBCount"]);
                    if (columns.Contains("PCCount"))
                        BillingCount = common.myStr(ds.Tables[0].Rows[0]["PCCount"]);
                    if (columns.Contains("ICCount"))
                        InsuranceCount = common.myStr(ds.Tables[0].Rows[0]["ICCount"]);
                    if (columns.Contains("BPCount"))
                        BPCount = common.myStr(ds.Tables[0].Rows[0]["BPCount"]);
                    if (columns.Contains("BTRCount"))
                        BTRCount = common.myStr(ds.Tables[0].Rows[0]["BTRCount"]);
                    if (columns.Contains("StatusType"))
                        StatusType = common.myStr(ds.Tables[0].Rows[0]["StatusType"]);
                    if (columns.Contains("OV" + common.myStr(Session["UserId"])))
                        OpenVisits = common.myStr(ds.Tables[0].Rows[0]["OV" + common.myStr(Session["UserId"])]);
                    int Count = 0;

                    if ((common.myStr(Session["PharmacyIntimationRights"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(PharmacyCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " New Pharmacy Intimations Pending!";
                        dr["Count"] = common.myStr(PharmacyCount);
                        dr["LinkPage"] = "/EMRBILLING/Popup/PatientIntimationStatus.aspx?PT=PHR&From=POPUP";
                        dr["Type"] = "PC";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(PharmacyCount);
                    }
                    if ((common.myStr(Session["BillingIntimationRights"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(BillingCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = "  File Recevied Intimations Pending!";
                        dr["Count"] = common.myStr(BillingCount);
                        dr["LinkPage"] = "/EMRBILLING/Popup/PatientIntimationStatus.aspx?PT=BILL&From=POPUP";
                        dr["Type"] = "FR";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(BillingCount);
                    }

                    if ((common.myStr(Session["ICIntimationRights"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(InsuranceCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " Insurance Clearance Intimations Pending!";
                        dr["Count"] = common.myStr(InsuranceCount);
                        dr["LinkPage"] = "/EMRBILLING/Popup/PatientIntimationStatus.aspx?PT=IC&From=POPUP";
                        dr["Type"] = "IC";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(InsuranceCount);
                    }
                    if ((common.myStr(Session["BillingIntimationRights"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(BPCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " Bill Prepration Intimations Pending!";
                        dr["Count"] = common.myStr(BPCount);
                        dr["LinkPage"] = "";
                        dr["Type"] = "BP";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(BPCount);
                    }

                    if ((common.myStr(Session["BedTransferRequest"]) == "Y") && (common.myStr(ShowNotification) == "Y") && (common.myInt(BTRCount) > 0))
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " Bed Transfer request Pending!";
                        dr["Count"] = common.myStr(BTRCount);
                        dr["LinkPage"] = "/ATD/BedTransferRequest.aspx?From=POPUP";
                        dr["Type"] = "BT";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(BTRCount);
                    }
                    if (common.myInt(OpenVisits) > 0)
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Notification"] = " Pending Open Visits!";
                        dr["Count"] = common.myStr(OpenVisits);
                        dr["LinkPage"] = "/EMRReports/Doctorwiseprocedurerpt.aspx?Fromdate=" + Convert.ToString(Convert.ToDateTime(DateTime.Now.Date.AddDays(-7)).ToString("yyyy/MM/dd")) + "&Todate=" + Convert.ToString(Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy/MM/dd")) + "&ReportName=EPS&statusId=7&Export=0&CF=MPage&UserId=" + Convert.ToString(Session["UserId"]) + "";
                        dr["Type"] = "OV";
                        dtGrid.Rows.Add(dr);
                        Count = Count + common.myInt(OpenVisits);
                    }

                    ibtnNotification.Visible = false;
                    lblNotification.Visible = false;
                    lblNotification.Text = "";
                    if (common.myInt(Count) > 0)
                    {
                        ibtnNotification.Visible = true;
                        lblNotification.Visible = true;
                        lblNotification.Text = Count.ToString();
                        grdViewNotification.DataSource = dtGrid;
                        grdViewNotification.DataBind();
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleExceptionWithMethod(Ex, "NotificationCheck");
            objException = null;
        }
        finally
        {
            dtGrid.Dispose();
            ds.Dispose();
        }
    }

    private void AddKeepAlive()
    {
        return;
        if (Session["KeepAlive"] != null)
        {
            return;
        }
        else
        {
            Session["KeepAlive"] = "Y";
        }
        int int_MilliSecondsTimeOut = (this.Session.Timeout * 60000) - 30000;
        string str_Script = @"
            <script type='text/javascript'>
            //Number of Reconnects
            var count=0;
            //Maximum reconnects setting
            var max = 5;
            function Reconnect(){

            count++;
            if (count < max)
            {
            window.status = 'Link to Server Refreshed ' + count.toString()+' time(s)' ;

            var img = new Image(1,1);

            img.src = '/RefreshSessionState.aspx';

            }
            }

            window.setInterval('Reconnect()'," + int_MilliSecondsTimeOut.ToString() + @"); //Set to length required

            </script>

            ";

        this.Page.RegisterClientScriptBlock("Reconnect", str_Script);

    }

    protected void ibtnNotification_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(hdnNotification.Value) == 0)
        {
            hdnNotification.Value = "1";
            dvNotification.Visible = true;
        }
        else if (common.myInt(hdnNotification.Value) == 1)
        {
            hdnNotification.Value = "0";
            dvNotification.Visible = false;
        }
    }
    protected void btnCloseDiv_OnClick(object sender, EventArgs e)
    {
        dvNotification.Visible = false;
        hdnNotification.Value = "0";
    }
    protected void grdViewNotification_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandArgument.ToString() == "OpenLink")
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            HiddenField hdnLink = (HiddenField)row.FindControl("hdnLinkPage");
            HiddenField hdnType = (HiddenField)row.FindControl("hdnType");

            if (common.myStr(hdnLink.Value) != "")
            {
                if (common.myStr(hdnType.Value) == "BT")
                {
                    hdnNotification.Value = "0";
                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(hdnType.Value) == "BP")
                {
                    hdnNotification.Value = "0";
                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(hdnType.Value) == "FR")
                {
                    hdnNotification.Value = "0";
                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(hdnType.Value) == "PC")
                {

                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                else if (common.myStr(hdnType.Value) == "OV")
                {
                    RadWindow1.NavigateUrl = common.myStr(hdnLink.Value);
                    RadWindow1.Height = 550;
                    RadWindow1.Width = 1000;
                    RadWindow1.Top = 40;
                    RadWindow1.Left = 100;
                    RadWindow1.OnClientClose = "";
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                }
                //dvNotification.Visible = false;
            }
        }
    }

    protected void imgQms_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("/QMS");
    }
}



