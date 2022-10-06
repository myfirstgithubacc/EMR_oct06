using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web;
using Telerik.Web.UI;
using System.IO;
using System.Data.SqlClient;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json;

public partial class Include_Master_EMRMasterWithTopDetails : System.Web.UI.MasterPage
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //private string UtdLink = ConfigurationManager.ConnectionStrings["UTDLink"].ConnectionString;
    private Hashtable hshInput;
    //clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Init(object sender, System.EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (!Page.IsCallback)
            {
                if (common.myStr(Request.QueryString["irtrf"]) != "")
                {
                    Session["StrO"] = null;
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
                    Session["StrO"] = Request.QueryString["irtrf"];
                    Session["IsAdminGroup"] = Request.QueryString["OP"].Split('_')[0];
                    Session["LoginIsAdminGroup"] = Request.QueryString["OP"].Split('_')[1];
                    Session["HospitalLocationID"] = Request.QueryString["OP"].Split('_')[2];
                    Session["FacilityID"] = Request.QueryString["OP"].Split('_')[3];
                    Session["GroupID"] = Request.QueryString["OP"].Split('_')[4];
                    Session["FinancialYearId"] = Request.QueryString["OP"].Split('_')[5];
                    Session["EntrySite"] = Request.QueryString["OP"].Split('_')[6];
                    Session["UserID"] = Request.QueryString["OP"].Split('_')[7];
                    Session["UserName"] = Request.QueryString["OP"].Split('_')[8].Replace("%", " ");
                    Session["ModuleId"] = Request.QueryString["OP"].Split('_')[9];
                    Session["URLPId"] = Request.QueryString["OP"].Split('_')[10];
                    Session["FacilityName"] = Request.QueryString["OP"].Split('_')[11].Replace("%", " ");
                    Session["CanDownloadPatientDocument"] = Request.QueryString["OP"].Split('_')[12];
                    Session["FacilityStateId"] = Request.QueryString["OP"].Split('_')[13];
                }
                this.AddKeepAlive();
                if (Session["UserID"] == null || Session["StrO"] == null)
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Session Expired", false);
                    return;
                }
                else if (Session["StrO"] != null && common.myStr(Request.QueryString["irtrf"]) != "" && common.myStr(Session["StrO"]) != common.myStr(Request.QueryString["irtrf"]))
                {
                    Session["UserID"] = null;
                    Response.Redirect("/Login.aspx?Logout=1&Status=Invalid URL", false);
                    return;
                }
                else if (Session["StrO"] != null)
                {
                    string output = "";
                    //BaseC.User usr = new BaseC.User(sConString);
                    //output = usr.RedirectionHandler(common.myInt(Session["UserID"]), common.myStr(Request.ServerVariables["REMOTE_ADDR"]), "VALIDATED"
                    //    , common.myStr(Session["StrO"]));
                    //usr = null;
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string ServiceURL = WebAPIAddress.ToString() + "api/Login/RedirectionHandler";
                    APIRootClass.RedirectionHandler objRoot = new global::APIRootClass.RedirectionHandler();
                    objRoot.UserId = common.myInt(Session["UserID"]);
                    objRoot.IPaddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    objRoot.Redirection = "VALIDATED";
                    objRoot.RedirectionCode = common.myStr(Session["StrO"]);
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    ds = JsonConvert.DeserializeObject<DataSet>(sValue);
                    output = common.myStr(ds.Tables[8].Rows[0][0]);
                    if (output.Contains("Expired") || output.Contains("Invalid"))
                    {
                        Session["UserID"] = null;
                        Response.Redirect("/Login.aspx?Logout=1&Status=" + output, false);
                        return;
                    }
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
        finally
        {
            ds.Dispose();
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
                    Page.Header.Title = Application["HospitalName"].ToString();
                if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null) || (Session["mainMenu"] == null) || (Session["MenuData"] == null))
                {
                    ExtractUserMenuFromSession();
                }

                bindPatientDetails();

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
                            Radslidingpane4.Title = common.myStr(objDs.Tables[0].Rows[0]["Name"]).Trim().Substring(0, 30) + " , " + common.myStr(objDs.Tables[0].Rows[0]["Payor"]); 
                        }
                        else
                        {
                            Radslidingpane4.Title = common.myStr(objDs.Tables[0].Rows[0]["Name"]).Trim() + " , " + common.myStr(objDs.Tables[0].Rows[0]["Payor"]) ;
                        }
                    }

                }
                ShowMenuItems();
                if (Session["LastRedirecetd"] == null)
                {
                    string strurl = Request.Url.AbsoluteUri.ToString();
                    if (strurl.ToLower().Contains("default.aspx"))
                    {
                        if (CheckQueries() > 0)
                        {
                            if (strurl.ToLower().Contains("?"))
                                strurl = strurl + "&Qry=1";
                            else
                                strurl = strurl + "?Qry=1";
                        }
                    }
                    Response.Redirect(strurl, false);
                    Session["LastRedirecetd"] = DateTime.Now;
                    //if (CheckQueries() > 0)
                    //{
                    //    Response.Redirect("default.aspx?Qry=1", false);
                    //    Session["LastRedirecetd"] = DateTime.Now;
                    //}
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
        lblQty_Click(null, null);
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
    private void bindPatientDetails()
    {
        if (!common.myStr(Request.QueryString["RPD"]).Equals(string.Empty))
        {
            lblPatientDetail.Text = common.myStr(Session["RelationPatientDetailString"]);
        }
        else if (Session["PatientDetailString"] != null)
        {
            lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
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
        DataTable objDs1 = new DataTable();
        DataSet dsPageCheck = new DataSet();
        DataTable dt = new DataTable();
        DataSet objDs = new DataSet();
        DataView dv = null;
        try
        {

            String strTickSign = "";
            tvCategory.Nodes.Clear();

            if (common.myStr(Session["MenuData"]) != "")
            {
                //objDs1 = (DataTable)Session["MenuData"];
                objDs1 = ((DataTable)Session["MenuData"]).Copy();
                if (objDs1.Rows.Count > 0)
                {
                    dv = objDs1.DefaultView;
                    dv.RowFilter = "ModuleId =" + iModuleId;
                    dt = new DataTable();
                    dt = dv.ToTable();
                    objDs = new DataSet();
                    objDs.Tables.Add(dt);


                    for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                    {
                        AddNodes(tvCategory, common.myInt(objDs.Tables[0].Rows[i]["PageId"]), common.myInt(objDs.Tables[0].Rows[i]["ParentId"]), common.myStr(objDs.Tables[0].Rows[i]["PageName"]), common.myStr(objDs.Tables[0].Rows[i]["PageUrl"]), common.myStr(objDs.Tables[0].Rows[i]["ModuleLocation"]), strTickSign, common.myInt(objDs.Tables[0].Rows[i]["ModuleId"]));
                        // AddNodes(tvCategory, common.myInt(objDs.Tables[0].Rows[i][1]), common.myInt(objDs.Tables[0].Rows[i][3]), common.myStr(objDs.Tables[0].Rows[i][2]), common.myStr(objDs.Tables[0].Rows[i][6]), strTickSign);
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
            dt.Dispose();
            objDs.Dispose(); objDs1.Dispose();
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
    public void AddNodes(TreeView tvName, Int32 iNodeID, Int32 iParentID, String sNodeText, string sPageUrl, string sModuleLocation, String sShowTickSign, int iModuelId)
    {
        try
        {
            string str = "&irtrf=" + common.myStr(Session["StrO"])
                + "&OP=" + common.myStr(Session["IsAdminGroup"]) + "_"
                + common.myStr(Session["LoginIsAdminGroup"]) + "_"
                + common.myStr(Session["HospitalLocationID"]) + "_"
                + common.myStr(Session["FacilityID"]) + "_"
                + common.myStr(Session["GroupID"]) + "_"
                + common.myStr(Session["FinancialYearId"]) + "_"
                + common.myStr(Session["EntrySite"]) + "_"
                + common.myStr(Session["UserId"]) + "_"
                + common.myStr(Session["UserName"]).Replace(" ", "%") + "_"
                + common.myStr(iModuelId) + "_"
                + common.myStr(iNodeID) + "_"
                + common.myStr(Session["FacilityName"]).Replace(" ", "%") + "_"
                + common.myStr(Session["CanDownloadPatientDocument"]) + "_"
                + common.myInt(Session["FacilityStateId"]).ToString();

            if (iParentID == 0)
            {
                TreeNode masternode;
                if (sPageUrl != "")
                {
                    if (!sPageUrl.Contains("?"))
                    {
                        masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", "");
                        if (!sPageUrl.Contains("MPG=") && !sPageUrl.Contains("Mpg="))
                        {
                            masternode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + "?Mpg=" + "P" + iNodeID.ToString() + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                        else
                        {
                            masternode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                    }
                    else
                    {
                        if (!sPageUrl.Contains("Mpg=") && !sPageUrl.Contains("MPG="))
                        {
                            masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "P" + iNodeID.ToString(), "");
                            masternode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + "&Mpg=" + "P" + iNodeID.ToString() + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                        else
                        {
                            masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", sPageUrl, "");
                            masternode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
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
                    if (!sPageUrl.Contains("?"))
                    {
                        childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", "", "");
                        if (!sPageUrl.Contains("MPG=") && !sPageUrl.Contains("Mpg="))
                        {
                            childNode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + "?Mpg=" + "C" + iNodeID.ToString() + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                        else
                        {
                            childNode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                    }
                    else
                    {
                        if (!sPageUrl.Contains("&MPG=") && !sPageUrl.Contains("&Mpg="))
                        {
                            childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl + "&Mpg=" + "C" + iNodeID.ToString(), "");
                            childNode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + "&Mpg=" + "C" + iNodeID.ToString() + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
                        }
                        else
                        {
                            childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", sPageUrl, "");
                            childNode.Text = "<a target='_self' onclick=showDia('" + sModuleLocation + sPageUrl + str + "') href='#' style='text-decoration:none; color:white;' >" + sNodeText.ToString() + sShowTickSign + "</a>";
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
            objException.HandleExceptionWithMethod(Ex, "AddNodes");
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
                    string str = "&irtrf=" + common.myStr(Session["StrO"])
                        + "&OP=" + common.myStr(Session["IsAdminGroup"]) + "_"
                        + common.myStr(Session["LoginIsAdminGroup"]) + "_"
                        + common.myStr(Session["HospitalLocationID"]) + "_"
                        + common.myStr(Session["FacilityID"]) + "_"
                        + common.myStr(Session["GroupID"]) + "_"
                        + common.myStr(Session["FinancialYearId"]) + "_"
                        + common.myStr(Session["EntrySite"]) + "_"
                        + common.myStr(Session["UserId"]) + "_"
                        + common.myStr(Session["UserName"]).Replace(" ", "%") + "_"
                        + common.myStr(Session["ModuleId"]) + "_0_"
                        + common.myStr(Session["FacilityName"]).Replace(" ", "%") + "_"
                        + common.myStr(Session["CanDownloadPatientDocument"]) + "_"
                        + common.myInt(Session["FacilityStateId"]).ToString();

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
                                //Done By Ujjwal 16 June 2015 to add page id in the url when clicked from rad menu start
                                string CurrentPageURL = common.myStr(tbl.Rows[rIdx]["PageURL"]).Trim();
                                // sushil
                                //string ModuleLocation = "http://localhost:1709/";
                                string ModuleLocation = common.myStr(tbl.Rows[rIdx]["ModuleLocation"]).Trim();
                                if (common.myLen(CurrentPageURL) > 0)
                                {
                                    if (!common.myStr(tbl.Rows[rIdx]["PageURL"]).ToUpper().Contains("MPG"))
                                    {
                                        if (common.myStr(tbl.Rows[rIdx]["PageURL"]).Contains("?"))
                                        {
                                            if (!CurrentPageURL.ToUpper().Contains("MPG=P"))
                                            {
                                                tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + "&Mpg=P" + common.myInt(tbl.Rows[rIdx]["PageId"]) + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);
                                            }
                                            else
                                            {
                                                tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);
                                            }
                                        }
                                        else
                                        {
                                            if (!CurrentPageURL.ToUpper().Contains("MPG=P"))
                                            {
                                                tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + "?Mpg=P" + common.myInt(tbl.Rows[rIdx]["PageId"]) + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);
                                            }
                                            else
                                            {
                                                tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]);
                                            }
                                        }
                                    }
                                    else
                                    { tbl.Rows[rIdx]["PageURL"] = ModuleLocation + CurrentPageURL + str + "_" + common.myStr(tbl.Rows[rIdx]["PageId"]); }
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
        if (e.Item.NavigateUrl == Request.Url.PathAndQuery)
        {
            e.Item.ForeColor = System.Drawing.Color.White;
            e.Item.BackColor = System.Drawing.Color.Fuchsia;
        }
    }
    public void ExtractUserMenuFromSession()
    {
        //BaseC.Security objSecurity = new BaseC.Security(sConString);
        DataSet objDs = new DataSet();
        DataTable dtMenuDetail = new DataTable();
        DataTable dtGroupDetail = new DataTable();
        DataTable dtModuleDetail = new DataTable();
        DataTable dtDistinctModuleId = new DataTable();
        DataTable dtDistinctModuleDetail = new DataTable();
        DataTable dtDistinctPageId = new DataTable();
        DataTable dtDistinctPageDetail = new DataTable();
        try
        {
            string[] columnNames = new string[1];

            if ((Session["MenuDetail"] == null) || (Session["GroupDetail"] == null) || (Session["ModuleDetail"] == null))
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/Login/getSecurityUserPages";
                APIRootClass.SecurityUserPages objRoot = new global::APIRootClass.SecurityUserPages();
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.Url = common.myStr(Request.Url.Host);
                objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationID"]);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                //objDs = objSecurity.getSecurityUserPages(); //uspSecurityGetUserPages

                if (objDs != null && objDs.Tables.Count > 2)
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
                //objDs = objSecurity.getSecurityUserPages(Request.Url.Host, common.myInt(Session["HospitalLocationID"]),
                //    common.myInt(Session["FacilityId"])); //uspSecurityGetUserPages
                string ServiceURL = WebAPIAddress.ToString() + "api/Login/getSecurityUserPages";
                APIRootClass.SecurityUserPages objRoot = new global::APIRootClass.SecurityUserPages();
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.Url = common.myStr(Request.Url.Host);
                objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationID"]);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

                if (objDs != null && objDs.Tables.Count > 2)
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
                                        // sushil
                                        // drSelected["ModuleLocation"] = "http://akhil32:4444/";
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

    protected void lblQty_Click(object sender, EventArgs e)
    {
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //DataSet ds = dl.FillDataSet(CommandType.Text, "exec UspGetGfsQueryDoc @empID=" + common.myInt(Session["EmployeeId"]));
        DataSet ds = new DataSet();
        string ServiceURL = WebAPIAddress.ToString() + "api/Common/GetGfsQueryDoc";
        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        APIRootClass.GfsCountManagement objRoot = new global::APIRootClass.GfsCountManagement();
        objRoot.Employeeid = common.myInt(Session["EmployeeId"]);
        string OutResult = string.Empty;
        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);      
        Session["CountQueryData"] = ds.Tables[0].Rows.Count; 
        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
            
        //    RadWindowForNew.NavigateUrl = "/Approval/QueryResponse.aspx";
        //    RadWindowForNew.Height = 600;
        //    RadWindowForNew.Width = 750;
        //    RadWindowForNew.Top = 10;
        //    RadWindowForNew.Left = 10;
        //    RadWindowForNew.VisibleOnPageLoad = true;
        //    RadWindowForNew.Modal = true;
        //    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        //    RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        //    RadWindowForNew.VisibleStatusbar = false;

        //    lblQty.Text = ds.Tables[0].Rows.Count.ToString();
        //    lblQty.ToolTip = "You have " + lblQty.Text + " Unrespond Queries from Insurance.";

        //}
    }

    protected void lnkDiagnosis_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMR/Assessment/Diagnosis.aspx?Diag=Tx&MPG=P1392&From=POPUP";
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
}