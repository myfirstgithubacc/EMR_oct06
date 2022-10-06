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

public partial class Include_Master_EMRMaster : System.Web.UI.MasterPage
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Init(object sender, System.EventArgs e)
    {
        try
        {
            if (!Page.IsCallback)
            {
                if (Session["StrO"] == null)
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Session Expired", false);
                    return;
                }
                else if (Session["StrO"] != null && common.myStr(Request.QueryString["irtrf"]) != "" && common.myStr(Session["StrO"]) != common.myStr(Request.QueryString["irtrf"]))
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Invalid URL", false);
                }
                else if (Session["StrO"] != null)
                {
                    string output = "";
                    BaseC.User usr = new BaseC.User(sConString);
                    usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATED", common.myStr(Session["StrO"]), out output);
                    if (output.Contains("Expired") || output.Contains("Invalid"))
                    {
                        Session["UserID"] = null;
                        Response.Redirect("/Login.aspx?Logout=1&Status=" + output, false);
                        return;
                    }
                    usr = null;
                }
                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                    return;
                }
            }

            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            }
            if (Request.QueryString["EncId"] != null)
            {
                Session["EncounterId"] = Request.QueryString["EncId"];
            }
            if (Request.QueryString["mdlId"] != null)
            {
                Session["ModuleId"] = Request.QueryString["mdlId"];
            }
            if (common.myStr(Request.QueryString["irtrf"]) != ""
                && common.myStr(Request.QueryString["OP"]).Split('_').Length > 10)
            {
                Session["irtrf"] = null;
                Session["IsAdminGroup"] = null;
                Session["LoginIsAdminGroup"] = null;
                Session["HospitalLocationID"] = null;
                Session["FacilityID"] = null;
                Session["GroupID"] = null;
                Session["FinancialYearId"] = null;
                Session["EntrySite"] = null;
                Session["UserID"] = null;
                Session["UserName"] = null;
                Session["ModuleId"] = null;
                Session["URLPId"] = null;
                Session["irtrf"] = Request.QueryString["irtrf"];

                Session["IsAdminGroup"] = common.myStr(Request.QueryString["OP"].Split('_')[0]);
                Session["LoginIsAdminGroup"] = common.myStr(Request.QueryString["OP"].Split('_')[1]);
                Session["HospitalLocationID"] = common.myStr(Request.QueryString["OP"].Split('_')[2]);
                Session["FacilityID"] = common.myStr(Request.QueryString["OP"].Split('_')[3]);
                Session["GroupID"] = common.myStr(Request.QueryString["OP"].Split('_')[4]);
                Session["FinancialYearId"] = common.myStr(Request.QueryString["OP"].Split('_')[5]);
                Session["EntrySite"] = common.myStr(Request.QueryString["OP"].Split('_')[6]);
                Session["UserID"] = common.myStr(Request.QueryString["OP"].Split('_')[7]);
                Session["UserName"] = common.myStr(Request.QueryString["OP"].Split('_')[8]).Replace("%", " ");
                Session["ModuleId"] = common.myStr(Request.QueryString["OP"].Split('_')[9]);
                Session["URLPId"] = common.myStr(Request.QueryString["OP"].Split('_')[10]);

                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 11)
                    Session["FacilityName"] = common.myStr(Request.QueryString["OP"]).Split('_')[11].Replace("%", " ");
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 12)
                    Session["CanDownloadPatientDocument"] = common.myStr(Request.QueryString["OP"]).Split('_')[12];
                if (common.myStr(Request.QueryString["OP"]).Split('_').Length > 13)
                    Session["FacilityStateId"] = common.myStr(Request.QueryString["OP"]).Split('_')[13];


                string output = "";
                BaseC.User usr = new BaseC.User(sConString);
                usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATED", common.myStr(Session["StrO"]), out output);
                if (output.Contains("Expired") || output.Contains("Invalid"))
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=" + output, false);
                }
                usr = null;


                if (Session["UserID"] == null)
                {
                    Response.Redirect("/Login.aspx?Logout=1", false);
                }
            }

        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "PageInit");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
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

                string facilityTitleName = common.myStr(Session["FacilityName"]);
                if (common.myStr(Session["FacilityName"]).Equals(string.Empty))
                {
                    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);

                    facilityTitleName = emr.BindApplicationName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]));
                    emr = null;
                }

                Page.Header.Title = facilityTitleName;


                if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null) || (Session["mainMenu"] == null) || (Session["MenuData"] == null) || common.myInt(Request.QueryString["irtrf"]).Equals(1))
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
                if (Session["RegistrationId"] != null && Session["RegistrationId"].ToString() != "" && common.myStr(Session["EncounterID"]) != null && Session["FormID"] != null)
                {
                    BaseC.Patient objPatient = new BaseC.Patient(sConString);
                    if (common.myStr(Session["RegistrationID"]) != "&nbsp;")
                    {
                        SqlDataReader objDr = (SqlDataReader)objPatient.getPatientShortDetail(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["encounterid"]));
                        if (objDr.Read())
                        {
                            BaseC.clsLISPhlebotomy Objstatus = new BaseC.clsLISPhlebotomy(sConString);
                            string result = string.Empty;

                            if (Session["OPIP"].ToString().Equals("O"))
                            {
                                result = Objstatus.GetPatientHasCriticalParameterForOP(Convert.ToString(Session["encounterno"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), 0, 0);
                            }
                            else if (Session["OPIP"].ToString().Equals("I"))
                            {
                                result = Objstatus.GetPatientHasCriticalParameter(Convert.ToString(Session["encounterno"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), 0, 0);
                            }
                            if (result.Equals("1"))
                            {
                                Radslidingpane4.Visible = true;
                                Radslidingpane4.Title = "<b><font size='2' name='Tahoma'>" + objDr["Name"].ToString() + "</font><span style= 'color: #FF0000;font-weight: bold;font-size:20px;valign:top'>*</span></b>";// +objDr["AgeAndFormName"].ToString();
                                lnkShowCriticalResults.Visible = true;
                            }
                            else if (result.Equals("0"))
                            {
                                Radslidingpane4.Visible = true;
                                Radslidingpane4.Title = "<b><font size='2' name='Tahoma'>" + objDr["Name"].ToString() + "</font></b>";//, " + objDr["AgeAndFormName"].ToString();
                                lnkShowCriticalResults.Visible = false;
                            }
                        }
                        objDr.Close();
                    }
                }
                ShowMenuItems();
            }
            catch (Exception Ex)
            {
                objException.HandleExceptionWithMethod(Ex, "PageLoad");
            }
        }
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
            objException.HandleExceptionWithMethod(Ex, "BindModuleGrid");
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
            objException.HandleExceptionWithMethod(Ex, "DoSelectNodeViaOnClickMasterPage");
        }
    }
    protected void gvModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["StationId"] = 0;
            Session["ModuleId"] = gvModules.SelectedRow.Cells[0].Text;
            Session["ModuleName"] = gvModules.SelectedRow.Cells[1].Text;
            Session["ModuleFlag"] = gvModules.SelectedRow.Cells[3].Text;
            BindTree();
            DoSelectNodeViaOnClickMasterPage();
        }
        catch (Exception Ex)
        {
            objException.HandleExceptionWithMethod(Ex, "gvModules_SelectedIndexChanged");
        }

    }
    protected void BindTree()
    {
        try
        {
            sModuleName.Text = common.myStr(Session["ModuleName"]);
            BindPages(Convert.ToInt16(gvModules.SelectedRow.Cells[0].Text), tvCategory);
        }
        catch (Exception ex)
        {
            objException.HandleExceptionWithMethod(ex, "BindTree");
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
            objException.HandleExceptionWithMethod(Ex, "gvModules_RowDataBound");
        }
    }
    public void BindPages(Int16 iModuleId, TreeView tvCategory)
    {

        try
        {
            DataTable objDs1 = new DataTable();
            DataSet dsPageCheck = new DataSet();
            String strTickSign = "";
            tvCategory.Nodes.Clear();
            DataView dv = null;

            if (common.myStr(Session["MenuData"]) != "")
            {
                objDs1 = (DataTable)Session["MenuData"];
                if (objDs1.Rows.Count > 0)
                {
                    DataSet objDs = new DataSet();
                    dv = objDs1.DefaultView;
                    dv.RowFilter = "ModuleId =" + iModuleId;
                    DataTable dt = new DataTable();
                    dt = dv.ToTable().Copy();

                    objDs.Tables.Add(dt);

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
            objException.HandleExceptionWithMethod(Ex, "BindPages");
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
            objException.HandleExceptionWithMethod(Ex, "DoSelectCurrentNode");
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
            objException.HandleExceptionWithMethod(Ex, "DoSelectCurrentNode");
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
            objException.HandleExceptionWithMethod(Ex, "CallRecursive");
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
            objException.HandleExceptionWithMethod(Ex, "ReCallRecursive");
        }
    }
    public void ShowMenuItems()
    {
        try
        {
            if (common.myStr(Session["MenuData"]) != "" && Session["MenuData"] != null)
            {
                DataTable dt = (DataTable)Session["MenuData"];
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataView DV = dt.DefaultView;

                        DV.RowFilter = "";
                        DV.Sort = "";

                        DV.RowFilter = "ModuleId=" + common.myInt(Session["ModuleId"]);
                        DV.Sort = "Hierarchy,Sequence ASC";

                        DataTable tbl = DV.ToTable().Copy();

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
                                //Done By Ujjwal 16 June 2015 to add page id in the url when clicked from rad menu start
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
                                //Done By Ujjwal 16 June 2015 to add page id in the url when clicked from rad menu end

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
            objException.HandleExceptionWithMethod(Ex, "ShowMenuItems");
        }
        finally
        {

        }
    }

    protected void RadMenu1_ItemDataBound(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
        if (e.Item.NavigateUrl == Request.Url.PathAndQuery)
        {
            e.Item.ForeColor = System.Drawing.Color.White;
            e.Item.BackColor = System.Drawing.Color.Fuchsia;
        }
    }
    public void ExtractUserMenuFromSession()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        try
        {
            string[] columnNames = new string[1];

            if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null))
            {
                DataSet objDs = new DataSet();
                objDs = objSecurity.getSecurityUserPages(common.myInt(Session["GroupID"]), Request.Url.Host, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

                if (objDs.Tables.Count > 2)
                {
                    Session["ModuleDetail"] = objDs.Tables[0].Copy();
                    Session["GroupDetail"] = objDs.Tables[1].Copy();
                    Session["MenuDetail"] = objDs.Tables[2].Copy();
                }
            }

            DataTable dtMenuDetail = new DataTable();
            DataTable dtGroupDetail = new DataTable();
            DataTable dtModuleDetail = new DataTable();
            DataTable dtDistinctModuleId = new DataTable();
            DataTable dtDistinctModuleDetail = new DataTable();
            DataTable dtDistinctPageId = new DataTable();
            DataTable dtDistinctPageDetail = new DataTable();
            dtMenuDetail = (DataTable)Session["MenuDetail"];
            dtGroupDetail = (DataTable)Session["GroupDetail"];
            dtModuleDetail = (DataTable)Session["ModuleDetail"];
            if ((dtMenuDetail.Rows.Count == 0) || (dtGroupDetail.Rows.Count == 0) || (dtModuleDetail.Rows.Count == 0))
            {
                DataSet objDs = new DataSet();
                objDs = objSecurity.getSecurityUserPages(common.myInt(Session["GroupID"]), Request.Url.Host, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));

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
            if (common.myStr(Session["IsAdminGroup"]) == "True")
            {
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
                            if (dvModuleDetail.ToTable().Rows.Count > 0)
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
                    dtMenuDetail.DefaultView.Sort = "ModuleId,Hierarchy,Sequence ASC";
                    Session["MenuData"] = dtMenuDetail.DefaultView.ToTable().Copy();
                    ViewState["PharmacyIntimationRights"] = "Y";
                    ViewState["BillingIntimationRights"] = "Y";
                    ViewState["ICIntimationRights"] = "Y";
                    ViewState["BedTransferRequest"] = "Y";
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
                            dtModuleDetail.DefaultView.RowFilter = "ModuleId=" + common.myInt(dr["ModuleId"]);
                            if (dtModuleDetail.Rows.Count > 0)
                            {
                                if (dvModuleDetail.ToTable().Rows.Count > 0)
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
                                    if (dvPageDetails.ToTable().Rows.Count > 0)
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
                                        drSelected["ModuleLocation"] = common.myStr(dvPageDetails[0]["ModuleLocation"]);

                                        dtDistinctPageDetail.Rows.Add(drSelected);
                                    }
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
                        dtDistinctPageDetail.DefaultView.Sort = "ModuleId,Hierarchy,Sequence Asc";
                        Session["MenuData"] = dtDistinctPageDetail.DefaultView.ToTable().Copy();

                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=PHR%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            ViewState["PharmacyIntimationRights"] = "Y";
                        }
                        else
                        {
                            ViewState["PharmacyIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=BILL%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            ViewState["BillingIntimationRights"] = "Y";
                        }
                        else
                        {
                            ViewState["BillingIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%PatientIntimationStatus.aspx?PT=IC%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            ViewState["ICIntimationRights"] = "Y";
                        }
                        else
                        {
                            ViewState["ICIntimationRights"] = "N";
                        }
                        dtDistinctPageDetail.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%/ATD/BedTransferRequest.aspx%'");
                        if (dtDistinctPageDetail.DefaultView.Count > 0)
                        {
                            ViewState["BedTransferRequest"] = "Y";
                        }
                        else
                        {
                            ViewState["BedTransferRequest"] = "N";
                        }

                        dtDistinctPageDetail.DefaultView.RowFilter = "";
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            string lineNumber = Ex.StackTrace.Substring(Ex.StackTrace.Length - 7, 7);
            objException.HandleExceptionWithMethod(Ex, "ExtractUserMenuFromCache" + lineNumber);
        }

    }
    public void btnReset_OnClick(object sender, EventArgs e)
    {

    }
    protected void lnkShowCriticalResults_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/LIS/Phlebotomy/PatientHistory.aspx?CF=&Master=Blank&EncId=" + common.myStr(Session["EncounterID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&PageSource=Ward";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 20;
        RadWindow1.Left = 20;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }

}