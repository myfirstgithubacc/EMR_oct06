using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.IO;
using System.Data.Common;
public class DL_Funs
{
    string ConString;
    DataSet ds;
    DataSet ds1;
    DataSet ds2;
    SqlDataReader dr;
    ArrayList ar = new ArrayList();
    Hashtable hstInput, hshInput;
    int _ICDCode;
    string _Description, _DiagnosisStatus;
    string _Remarks;
    string _ResolvedDate, _EncodedDate, _IsResolved;
    int _ICDID;
    int _Status;
    int _st;
    int _ID;
    int _PrimaryDiagnosis;
    public DL_Funs()
    {
        ConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    }
    public void BindPagesFromOthers(Int16 iModuleId, TreeView tvCategory)
    {
        tvCategory.Nodes.Clear();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        hshInput = new Hashtable();
        hshInput.Add("inyModuleID", iModuleId);

        hshInput.Add("@intEncounterId", Convert.ToInt32(System.Web.HttpContext.Current.Session["encounterid"]));
        hshInput.Add("intGroupId", Convert.ToInt32(System.Web.HttpContext.Current.Session["GroupID"]));
        DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModuleUserPages", hshInput);

        for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
        {
            AddNodes(tvCategory, Convert.ToInt32(objDs.Tables[0].Rows[i][0]), Convert.ToInt32(objDs.Tables[0].Rows[i][2]), Convert.ToString(objDs.Tables[0].Rows[i][1]), Convert.ToString(objDs.Tables[0].Rows[i][5]), Convert.ToString(objDs.Tables[0].Rows[i][8]));
        }
        if (tvCategory.Nodes.Count > 0)
        {
            //tvCategory.Nodes[0].Selected = true;
            //tvCategory.CollapseAll();
            tvCategory.ExpandAll();
            tvCategory.PopulateNodesFromClient = true;
            tvCategory.ShowLines = true;
            // tvCategory_SelectedNodeChanged(this, null);
        }
    }
    public void BindModuleGrid(Int32 intgroupID, Int32 intUserID, GridView gvModule)
    {
        DataSet objDs = new DataSet();
        if (HttpContext.Current.Cache["ModuleName" + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]] == null)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
            hshInput = new Hashtable();
            hshInput.Add("intUserId", Convert.ToInt32(intUserID));
            hshInput.Add("intGroupId", Convert.ToInt32(intgroupID));
            objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetUserModule", hshInput);
            if (objDs.Tables[0].Rows.Count > 0)
            {
                //sModuleName.Text = objDs.Tables[0].Rows[0]["ModuleName"].ToString();
                //hdnModuleId.Value = objDs.Tables[0].Rows[0]["ModuleID"].ToString();
                gvModule.DataSource = objDs;
                gvModule.DataBind();
                HttpContext.Current.Cache.Insert("ModuleName" + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"], objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }
        else
        {
            objDs = (DataSet)HttpContext.Current.Cache["ModuleName" + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
            gvModule.DataSource = objDs;
            gvModule.DataBind();
        }
    }
    public void BindPages(Int16 iModuleId, TreeView tvCategory)
    {
        //try
        //{
        System.Web.HttpContext.Current.Session["ModuleIdValue"] = iModuleId;
        DataSet objDs = new DataSet();
        DataSet dsPageCheck = new DataSet();
        DataSet dsModule = new DataSet();
        String strTickSign = "";
        tvCategory.Nodes.Clear();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        if (iModuleId == 3)
        {


            if (System.Web.HttpContext.Current.Session["NewFormID"] == null)
            {
                if (System.Web.HttpContext.Current.Session["EncounterID"] != null)
                {

                    HttpContext.Current.Cache.Remove("SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]);

                    hshInput = new Hashtable();
                    hshInput.Add("inyModuleID", iModuleId);
                    hshInput.Add("@intEncounterId", Convert.ToInt32(System.Web.HttpContext.Current.Session["encounterid"]));
                    hshInput.Add("intGroupId", Convert.ToInt32(System.Web.HttpContext.Current.Session["GroupID"]));
                    if (System.Web.HttpContext.Current.Session["formId"] != null)
                        hshInput.Add("intFormId", System.Web.HttpContext.Current.Session["formId"].ToString());

                    objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModuleUserPages", hshInput);

                    System.Web.HttpContext.Current.Session["NewFormID"] = System.Web.HttpContext.Current.Session["formId"];

                    HttpContext.Current.Cache.Insert("SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"], objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);

                    objDs = (DataSet)HttpContext.Current.Cache["SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];

                }
                else
                {
                    if (HttpContext.Current.Cache["SubOtherModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]] != null)
                    {
                        DataSet dsMenu = new DataSet();
                        dsMenu = (DataSet)HttpContext.Current.Cache["SubOtherModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
                        DataView dvModule = new DataView(dsMenu.Tables[0]);
                        dvModule.RowFilter = "ModuleId=" + iModuleId + " AND StaticPage=1";

                        objDs.Tables.Add(dvModule.ToTable());

                    }

                }
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["NewFormID"].ToString().Trim() == System.Web.HttpContext.Current.Session["FormID"].ToString().Trim())
                {
                    if (HttpContext.Current.Cache["SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]] != null)
                    {
                        objDs = (DataSet)HttpContext.Current.Cache["SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
                    }
                    else
                    {
                        HttpContext.Current.Cache.Remove("SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]);

                        System.Web.HttpContext.Current.Session["NewFormID"] = null;
                        hshInput = new Hashtable();
                        hshInput.Add("inyModuleID", iModuleId);
                        hshInput.Add("@intEncounterId", Convert.ToInt32(System.Web.HttpContext.Current.Session["encounterid"]));
                        hshInput.Add("intGroupId", Convert.ToInt32(System.Web.HttpContext.Current.Session["GroupID"]));
                        if (System.Web.HttpContext.Current.Session["formId"] != null)
                            hshInput.Add("intFormId", System.Web.HttpContext.Current.Session["formId"].ToString());

                        objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModuleUserPages", hshInput);

                        System.Web.HttpContext.Current.Session["NewFormID"] = System.Web.HttpContext.Current.Session["formId"];

                        HttpContext.Current.Cache.Insert("SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"], objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);

                        objDs = (DataSet)HttpContext.Current.Cache["SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
                    }
                }
                else
                {
                    HttpContext.Current.Cache.Remove("SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]);

                    System.Web.HttpContext.Current.Session["NewFormID"] = null;
                    hshInput = new Hashtable();
                    hshInput.Add("inyModuleID", iModuleId);
                    hshInput.Add("@intEncounterId", Convert.ToInt32(System.Web.HttpContext.Current.Session["encounterid"]));
                    hshInput.Add("intGroupId", Convert.ToInt32(System.Web.HttpContext.Current.Session["GroupID"]));
                    if (System.Web.HttpContext.Current.Session["formId"] != null)
                        hshInput.Add("intFormId", System.Web.HttpContext.Current.Session["formId"].ToString());

                    objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModuleUserPages", hshInput);

                    //HttpContext.Current.Cache.Insert("FormID", System.Web.HttpContext.Current.Session["FormID"], null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                    System.Web.HttpContext.Current.Session["NewFormID"] = System.Web.HttpContext.Current.Session["formId"];

                    HttpContext.Current.Cache.Insert("SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"], objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);

                    objDs = (DataSet)HttpContext.Current.Cache["SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
                }
            }
        }
        else
        {

            if (HttpContext.Current.Cache["SubOtherModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]] == null)
            {
                DataSet dsOtherModule = new DataSet();
                hshInput = new Hashtable();
                hshInput.Add("inyModuleID", 0);
                //hshInput.Add("@intEncounterId", Convert.ToInt32(System.Web.HttpContext.Current.Session["encounterid"]));
                hshInput.Add("intGroupId", Convert.ToInt32(System.Web.HttpContext.Current.Session["GroupID"]));
                //if (System.Web.HttpContext.Current.Session["formId"] != null)
                //hshInput.Add("intFormId", System.Web.HttpContext.Current.Session["formId"].ToString());

                dsOtherModule = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModuleUserPages", hshInput);

                if (dsOtherModule.Tables[0].Rows.Count > 0)
                {
                    HttpContext.Current.Cache.Insert("SubOtherModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"], dsOtherModule, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);

                }


                dsOtherModule = (DataSet)HttpContext.Current.Cache["SubOtherModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
                DataView dvModule = new DataView(dsOtherModule.Tables[0]);
                dvModule.RowFilter = "ModuleId=" + iModuleId + "";
                //DataTable dtModule = new DataTable();
                //dtModule = (DataTable)dvModule.ToTable();
                objDs.Tables.Add(dvModule.ToTable());
            }
            else
            {
                DataSet dsOtherModule = new DataSet();

                dsOtherModule = (DataSet)HttpContext.Current.Cache["SubOtherModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] + System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
                DataView dvModule = new DataView(dsOtherModule.Tables[0]);
                dvModule.RowFilter = "ModuleId=" + iModuleId + "";
                //DataTable dtModule = new DataTable();
                //dtModule = (DataTable)dvModule.ToTable();
                objDs.Tables.Add(dvModule.ToTable());
            }
        }

        if (iModuleId == 3 && Convert.ToString(System.Web.HttpContext.Current.Session["formId"]) != "")
        {
            hshInput = new Hashtable();
            hshInput.Add("@intRegistrationId", Convert.ToInt32(System.Web.HttpContext.Current.Session["RegistrationId"]));
            hshInput.Add("@intEncounterId", Convert.ToInt32(System.Web.HttpContext.Current.Session["encounterid"]));
            hshInput.Add("@FormId", System.Web.HttpContext.Current.Session["formId"].ToString());
            dsPageCheck = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRShowPageCheck", hshInput);
        }

        for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
        {
            strTickSign = "";
            if (iModuleId == 3 && Convert.ToString(System.Web.HttpContext.Current.Session["formId"]) != "")
            {
                DataTable dt = new DataTable();
                DataView dv = new DataView(dsPageCheck.Tables[0]);
                dv.RowFilter = "PageIdentification = '" + Convert.ToString(objDs.Tables[0].Rows[i][8]) + "'";
                dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["DataStatus"]) == "LOCK")
                    {
                        strTickSign = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
                    }
                    else
                        strTickSign = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
                }
                else
                    strTickSign = "";
            }

            AddNodes(tvCategory, Convert.ToInt32(objDs.Tables[0].Rows[i][0]), Convert.ToInt32(objDs.Tables[0].Rows[i][2]), Convert.ToString(objDs.Tables[0].Rows[i][1]), Convert.ToString(objDs.Tables[0].Rows[i][5]), strTickSign);
        }
        if (tvCategory.Nodes.Count > 0)
        {
            //tvCategory.Nodes[0].Selected = true;
            //tvCategory.CollapseAll();
            tvCategory.ExpandAll();
            tvCategory.PopulateNodesFromClient = true;
            tvCategory.ShowLines = true;
            //tvCategory_SelectedNodeChanged(this, null);
        }
        else
        {
            //lbtnNewCategory_OnClick(this, null);  
        }

    }

    public void DoSelectCurrentNode(TreeView tvCategory)
    {
        if (System.Web.HttpContext.Current.Session["CurrentNode"] != null)
        {
            foreach (TreeNode node in tvCategory.Nodes)
            {
                if (node.Value == System.Web.HttpContext.Current.Session["CurrentNode"].ToString())
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
                    if (System.Web.HttpContext.Current.Session["CurrentNode"] != null)
                    {
                        if (n.Value == System.Web.HttpContext.Current.Session["CurrentNode"].ToString())
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
    public void AddNodes(TreeView tvName, Int32 iNodeID, Int32 iParentID, String sNodeText, string sPageUrl, String sShowTickSign)
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
    //protected string ShowTickSign(string sNodeText, string NodeId, string sNodeIdentification)
    // {
    //     DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
    //     hshInput = new Hashtable();
    //     string nStr = "";
    // int RegId = Convert.ToInt32(System.Web.HttpContext.Current.Session["registrationId"]);

    //     int HospitalId = Convert.ToInt32(System.Web.HttpContext.Current.Session["HospitalLocationID"]);
    //     int EncounterId = Convert.ToInt32(System.Web.HttpContext.Current.Session["encounterid"]);

    //     //hshInput.Add("@intRegistrationId", RegId);
    //     //hshInput.Add("@intRegistrationNo", RegNo);
    //     hshInput.Add("@intEncounterId", EncounterId);
    //     string FormId = "0";
    //     if (System.Web.HttpContext.Current.Session["formId"] != null)
    //         FormId = System.Web.HttpContext.Current.Session["formId"].ToString();
    //     hshInput.Add("@formid", FormId);
    //     //hshInput.Add("@inyHospitalLocationId", HospitalId);
    //     DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRShowPageCheck", hshInput);

    //     if (objDs.Tables.Count > 1)
    //     {
    //         if (sNodeText == "Vitals")
    //         {
    //             int LockedNode = DoLockUnLock("sel",  EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //             if (LockedNode == 0)
    //             {
    //                 if (objDs.Tables[0].Rows.Count > 0)
    //                     nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //             }
    //             else
    //                 nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";

    //         }
    //         else if (sNodeText == "Chief Complaints")
    //         {
    //             int LockedNode = DoLockUnLock("sel", EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //             if (LockedNode == 0)
    //             {
    //                 if (objDs.Tables[1].Rows.Count > 0)
    //                     nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //             }
    //             else
    //                 nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         }
    //         else if (sNodeText == "Diagnosis")
    //         {
    //             int LockedNode = DoLockUnLock("sel",EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //             if (LockedNode == 0)
    //             {
    //                 if (objDs.Tables[2].Rows.Count > 0)
    //                     nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //             }
    //             else
    //                 nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         }
    //         else if (sNodeText == "Allergies")
    //         {
    //             int LockedNode = DoLockUnLock("sel", EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //             if (LockedNode == 0)
    //             {
    //                 if (objDs.Tables[3].Rows.Count > 0)
    //                     nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //             }
    //             else
    //                 nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         }
    //         else if (sNodeText == "New Prescription")
    //         {
    //             int LockedNode = DoLockUnLock("sel", EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //             if (LockedNode == 0)
    //             {
    //                 if (objDs.Tables[4].Rows.Count > 0)
    //                     nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //             }
    //             else
    //                 nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         }
    //         else if (sNodeText == "Orders & Procedures")
    //         {
    //             int LockedNode = DoLockUnLock("sel",EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //             if (LockedNode == 0)
    //             {
    //                 if (objDs.Tables[5].Rows.Count > 0)
    //                     nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //             }
    //             else
    //                 nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         }
    //         //else if (sNodeText == "ROS")
    //         //{
    //         //    int LockedNode = DoLockUnLock("sel",EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //         //    if (LockedNode == 0)
    //         //    {
    //         //        if (objDs.Tables[6].Rows.Count > 0)
    //         //            nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //         //    }
    //         //    else
    //         //        nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         //}
    //         //else if (sNodeText == "Past History")
    //         //{
    //         //    int LockedNode = DoLockUnLock("sel", EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //         //    if (LockedNode == 0)
    //         //    {
    //         //        if (objDs.Tables[7].Rows.Count > 0)
    //         //            nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //         //    }
    //         //    else
    //         //        nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         //}
    //         //else if (sNodeText == "Physical Examination")
    //         //{
    //         //    int LockedNode = DoLockUnLock("sel", EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //         //    if (LockedNode == 0)
    //         //    {
    //         //        if (objDs.Tables[8].Rows.Count > 0)
    //         //            nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //         //    }
    //         //    else
    //         //        nStr = "&nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         //}
    //         else if (sNodeText == "Attachments")
    //         {
    //             int LockedNode = DoLockUnLock("sel", EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //             if (LockedNode == 0)
    //             {
    //                 if (objDs.Tables[6].Rows.Count > 0)
    //                     nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //             }
    //             else
    //                 nStr = " &nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //         }
    //         else
    //         {
    //             string strPageType = sNodeIdentification.Substring(0,1);
    //             if (strPageType == "T")
    //             {
    //                 DataView dv = objDs.Tables[7].DefaultView;
    //                 dv.RowFilter = "id=" + NodeId.Trim();
    //                 if (dv.Count > 0)
    //                 {
    //                     int LockedNode = DoLockUnLock("sel", EncounterId.ToString(), HospitalId.ToString(), FormId, NodeId, "");
    //                     if (LockedNode == 0)
    //                     {
    //                         if (objDs.Tables[7].Rows.Count > 0)
    //                             nStr = "&nbsp;<img src='/Images/right-icon27.gif' height='10' width='10' />";
    //                     }
    //                     else
    //                         nStr = " &nbsp;<img src='/Images/Redtick7.gif' height='10' width='10' />";
    //                 }
    //             }
    //         }
    //     }

    //     return nStr;
    // }
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
                tn.ChildNodes.Add(childNode);
            }
            ReCallRecursive(tn, iNodeID, sParentID, sNodeText, sPageUrl);
        }
    }
    public int DoLockUnLock(string type, string EncounterId, string HospId, string formId, string TemplateId, string Lock)
    {

        bool isDynamic = false;
        string TemplateType = TemplateId.Substring(0, 1);
        if (TemplateType == "T")
        {
            TemplateId = TemplateId.Substring(1);
            isDynamic = true;
        }
        else if (HttpContext.Current.Request.Url.AbsolutePath.ToLower() == "/emr/templates/default.aspx")
        {
            isDynamic = true;
        }
        else
            isDynamic = false;

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        int ret = 0;
        string sqlQ = "";
        hstInput = new Hashtable();
        //hstInput.Add("@intRegistrationNo", RegNo);
        hstInput.Add("@intEncounterId", EncounterId);
        hstInput.Add("@inyHospitalLocationId", HospId);
        hstInput.Add("@intformId", formId);
        hstInput.Add("@intTemplateId", TemplateId);
        if (type == "Upd")
        {
            if (Lock == "Lock")
                hstInput.Add("@Lock", 1);
            else
                hstInput.Add("@Lock", 0);


            sqlQ = "Update EMRPatientFormDetails Set Lock = @Lock From EMRPatientFormDetails fd inner Join EMRPatientForms f"
               + " On fd.PatientFormId = f.PatientFormId where f.EncounterId = @intEncounterId";
            if (isDynamic)
                sqlQ += " And f.FormId = @intformId And fd.TemplateId = @intTemplateId";
            else
                sqlQ += " And f.FormId = @intformId And fd.PageId = @intTemplateId";
            ret = objDl.ExecuteNonQuery(CommandType.Text, sqlQ, hstInput);
        }
        else
        {
            sqlQ = "Select fd.Lock From EMRPatientFormDetails fd WITH (NOLOCK) inner Join EMRPatientForms f WITH (NOLOCK) "
                + " On fd.PatientFormId = f.PatientFormId where f.EncounterId = @intEncounterId";
            if (isDynamic)
                sqlQ += " And f.FormId = @intformId And fd.TemplateId = @intTemplateId";
            else
                sqlQ += " And f.FormId = @intformId And fd.PageId = @intTemplateId";
            //            Object Locked = objDl.ExecuteScalar(CommandType.Text, sqlQ, hstInput);
            DataSet dsLocked = new DataSet();
            dsLocked = objDl.FillDataSet(CommandType.Text, sqlQ, hstInput);
            if (dsLocked.Tables[0].Rows.Count > 0)
                if (dsLocked.Tables[0].Rows[0]["Lock"].ToString() != "")
                    if (((bool)dsLocked.Tables[0].Rows[0]["Lock"]) == true)
                        ret = 1;
            //if (Locked != null)
            //    if (((bool)Locked) == true)
            //        ret = 1;
            //ret = Convert.ToInt16(Locked);
        }
        return ret;
    }
    public string getDefaultFontSize(Page pg, string HospitalLocationId)
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", HospitalLocationId);
        if (FieldValue != "")
        {
            sFontSize = " font-size: " + FieldValue + ";";
        }
        return sFontSize;
    }
    public string getDefaultFontName(Page pg, string HospitalLocationId)
    {
        string sFontName = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontType", HospitalLocationId);
        if (FieldValue != "")
        {
            sFontName = fonts.GetFont("Name", FieldValue);
            if (sFontName != "")
                sFontName = " font-family: " + sFontName + ";";
        }
        return sFontName;
    }
    public StringBuilder BindProblemsHPI(int RegId, int HospitalId, int EncounterId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle)
    {
        string BeginList = "", EndList = "", sBegin = "", sEnd = "";
        StringBuilder sbTemp = new StringBuilder();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        Hashtable hsProblems = new Hashtable();
        DataTable dtChronic = new DataTable();
        DataTable dtNonChronic = new DataTable();
        string strSql = "";
        string strAge = "";
        Int16 intRowCount;
        hsProblems.Add("@inyHospitalLocationID", HospitalId);
        hsProblems.Add("@intRegistrationId", RegId);
        hsProblems.Add("@intEncounterId", EncounterId);

        strSql = "Select r.FirstName, r.LastName, dbo.AgeInYrsMonthDay (Convert(varchar(10),DateofBirth,111), Convert(Varchar(10), Getdate(),111)) as Age, dbo.GetGender(Gender) as Gender,";
        strSql = strSql + " c.Race from Registration r WITH (NOLOCK) Left Join RaceMaster c WITH (NOLOCK) On r.RaceId = c.RaceId";
        strSql = strSql + " where r.Id = @intRegistrationId ";
        strSql = strSql + " Select IsNull(IsPregnant,0) as IsPregnant, IsNull(IsBreastFeeding,0) as IsBreastFeeding from Encounter WITH (NOLOCK) where Id = @intEncounterId";

        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", hsProblems);
        //ds1 = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetProblemsHPI", hsProblems);
        ds2 = DlObj.FillDataSet(CommandType.Text, strSql, hsProblems);
        if (Convert.ToString(ds2.Tables[0].Rows[0]["Age"]).Trim() != "")
        {
            strAge = Convert.ToString(ds2.Tables[0].Rows[0]["Age"]).Trim();
            strAge = strAge.Remove(strAge.Length - 4, 4).ToLower();
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (drTemplateListStyle != null)
            {
                if (drTemplateListStyle["TemplateListStyle"].ToString() == "1")
                { BeginList = "<ul>"; EndList = "</ul>"; sBegin = "<li>"; sEnd = "</li>"; }
                else if (drTemplateListStyle["TemplateListStyle"].ToString() == "2")
                { BeginList = "<ol>"; EndList = "</ol>"; sBegin = "<li>"; sEnd = "</li>"; }
            }

            DataView dvChronic = ds.Tables[0].DefaultView;
            dvChronic.RowFilter = "IsChronic = 'True'";
            dtChronic = dvChronic.ToTable();
            DataView dvNonChronic = ds.Tables[0].DefaultView;
            dvNonChronic.RowFilter = "IsChronic = 'False'";
            dtNonChronic = dvNonChronic.ToTable();
            if (dtChronic.Rows.Count > 0)
            {
                sbTemp.Append("The patient, " + Convert.ToString(ds2.Tables[0].Rows[0]["FirstName"]).Trim() + " " + Convert.ToString(ds2.Tables[0].Rows[0]["LastName"]).Trim() + ", is a ");
                sbTemp.Append(strAge + " years old " + Convert.ToString(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + Convert.ToString(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " with a history of ");
                // Chronic problems starts here
                for (int i = 0; i < dtChronic.Rows.Count; i++)
                {
                    DataRow dr1 = dtChronic.Rows[i] as DataRow;
                    if (i == 0)
                    {
                        if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtChronic.Rows.Count - 1)
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                            }
                        }
                    }

                    if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem2"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtChronic.Rows.Count - 1)
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem2"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem3"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtChronic.Rows.Count - 1)
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem3"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem4"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtChronic.Rows.Count - 1)
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem4"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem5"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtChronic.Rows.Count - 1)
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().Trim() + ", ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem5"]) != "")
                    {
                        if (i == dtChronic.Rows.Count - 1)
                        {
                            sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                        }
                        else
                        {
                            sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                        }
                    }
                }// Chronic problems ends here
                // Non Chronic problems starts here
                for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                {
                    //intRowCount = Convert.ToInt16( dtNonChronic.Rows.Count - 1 );
                    DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                    if (i == 0)
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" who presents with ");
                        //sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        //
                        if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                            }
                        }
                    }
                    else
                    {

                        if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                        {
                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        }
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                        }
                    }

                    if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem2"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem2"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem3"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem3"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem4"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem4"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem5"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem5"]) != "")
                    {
                        if (i == dtNonChronic.Rows.Count - 1)
                        {
                            sbTemp.Append(" and " + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                        }
                        else
                        {
                            sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                        }
                    }
                }
                //Non Chronic problems ends here.
            }
            else
            {
                sbTemp.Append("The patient, " + Convert.ToString(ds2.Tables[0].Rows[0]["FirstName"]).Trim() + " " + Convert.ToString(ds2.Tables[0].Rows[0]["LastName"]).Trim() + ", is a ");
                sbTemp.Append(strAge + " years old " + Convert.ToString(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + Convert.ToString(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " who presents with ");
                for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                {
                    DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                    if (i == 0)
                    {
                        if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                        {
                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        }
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        }
                    }

                    if (Convert.ToString(dr1["AssociatedProblem1"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem2"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem2"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem3"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem3"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem4"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("and " + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem4"]) != "")
                    {
                        if (Convert.ToString(dr1["AssociatedProblem5"]) != "")
                            sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                        else
                        {
                            if (i == dtNonChronic.Rows.Count - 1)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append("and " + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                        }
                    }
                    if (Convert.ToString(dr1["AssociatedProblem5"]) != "")
                    {
                        if (i == dtNonChronic.Rows.Count - 1)
                        {
                            sbTemp.Append("and " + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                        }
                        else
                            sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                    }
                }
            }

            //sbTemp.Remove(sbTemp.Length - 2, 2);
            if (Convert.ToString(ds2.Tables[1].Rows[0]["IsPregnant"]) != "False")
                sbTemp.Append(" The patient is or may be pregnant. ");
            if (Convert.ToString(ds2.Tables[1].Rows[0]["IsBreastFeeding"]) != "False")
                sbTemp.Append(" The patient is breast feeding. ");


            ////////////
            for (int i = 0; i < dtChronic.Rows.Count; i++)
            {
                if (i == 0)
                {
                    sbTemp.Append(BeginList);
                }
                DataRow dr1 = dtChronic.Rows[i] as DataRow;
                if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Quality2"]) != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["OnsetDate"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
                {

                    sbTemp.Append(sBegin + " The patient describes the " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
                    if (Convert.ToString(dr1["Quality1"]).ToLower() != "")
                    {
                        if (Convert.ToString(dr1["Quality2"]) != "")
                            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + " ");
                        }
                    }
                    if (Convert.ToString(dr1["Quality2"]) != "")
                    {
                        if (Convert.ToString(dr1["Quality3"]) != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality3"]) != "")
                    {
                        if (Convert.ToString(dr1["Quality4"]) != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality4"]) != "")
                    {
                        if (Convert.ToString(dr1["Quality5"]) != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality5"]) != "")
                    {
                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                        {
                            sbTemp.Remove(sbTemp.Length - 2, 2);
                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + ". ");
                        }
                        else
                        {
                            sbTemp.Remove(sbTemp.Length - 2, 2);
                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + " ");
                        }
                    }

                    if (Convert.ToString(dr1["Location"]) != "")
                    {
                        if (Convert.ToString(dr1["Severity"]) != "")
                        {
                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " ");
                        }
                        else
                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + ". ");
                    }
                    if (Convert.ToString(dr1["Severity"]) != "")
                    {
                        if (Convert.ToString(dr1["OnsetDate"]) != "")
                            sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + " ");
                        else
                        {
                            sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                        }
                    }

                    if (Convert.ToString(dr1["OnsetDate"]) != "")
                    {
                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" that began on " + Convert.ToString(dr1["OnsetDate"]).Trim() + ". ");
                    }
                    //sbTemp.Append("that began on [OnSet Date] ");  
                    if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms occur " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms are " + Convert.ToString(dr1["Onset"]).ToLower() + " and " + Convert.ToString(dr1["Duration"]).ToLower() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) == "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                    }

                    if (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "" && Convert.ToString(dr1["NoOfOccurrence"]) != "0")
                    {
                        if (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != "")
                            sbTemp.Append(" problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
                        else
                            sbTemp.Append(" problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times. ");
                    }
                    if (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != "")
                        sbTemp.Append(" beginning on " + Convert.ToString(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");

                }

                //asdfg
                if (Convert.ToString(dr1["AggravatingFactors"]).Trim() != "")
                    sbTemp.Append(" Aggravating factors are: " + Convert.ToString(dr1["AggravatingFactors"]).ToLower().Trim() + ". ");
                if (Convert.ToString(dr1["RelievingFactors"]).Trim() != "")
                    sbTemp.Append(" Relieving factors are:  " + Convert.ToString(dr1["RelievingFactors"]).ToLower().Trim() + ". ");
                if ((Convert.ToString(dr1["DeniesSymptoms1"]) != "") || (Convert.ToString(dr1["DeniesSymptoms2"]) != "") || (Convert.ToString(dr1["DeniesSymptoms3"]) != "") || (Convert.ToString(dr1["DeniesSymptoms4"]) != "") || (Convert.ToString(dr1["DeniesSymptoms5"]) != ""))
                    sbTemp.Append(" Patient denies symptoms of ");
                if (Convert.ToString(dr1["DeniesSymptoms1"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms2"]) != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms2"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms3"]) != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms3"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms4"]) != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms4"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms5"]) != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms5"]) != "")
                {
                    sbTemp.Remove(sbTemp.Length - 2, 2);
                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms5"]).ToLower().Trim() + ". ");
                }

            }
            sbTemp.Append(sEnd);
            for (int i = 0; i < dtNonChronic.Rows.Count; i++)
            {
                if (i == 0)
                {
                    // sbTemp.Append("<br />");
                }
                DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                //sbTemp.Append("<br /><br />" + Convert.ToString(dr1["ProblemDescription"]) + " presents with ");

                if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Quality2"]) != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["OnsetDate"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
                {
                    sbTemp.Append(sBegin + " The patient describes the " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
                    if (Convert.ToString(dr1["Quality1"]).ToLower() != "")
                    {
                        if (Convert.ToString(dr1["Quality2"]) != "")
                            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + " ");
                        }
                    }
                    if (Convert.ToString(dr1["Quality2"]) != "")
                    {
                        if (Convert.ToString(dr1["Quality3"]) != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality3"]) != "")
                    {
                        if (Convert.ToString(dr1["Quality4"]) != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality4"]) != "")
                    {
                        if (Convert.ToString(dr1["Quality5"]) != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality5"]) != "")
                    {
                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                        {
                            sbTemp.Remove(sbTemp.Length - 2, 2);
                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + ". ");
                        }
                        else
                        {
                            sbTemp.Remove(sbTemp.Length - 2, 2);
                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + " ");
                        }
                    }

                    if (Convert.ToString(dr1["Location"]) != "")
                    {
                        if (Convert.ToString(dr1["Severity"]) != "")
                        {
                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " ");
                        }
                        else
                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + ". ");
                    }
                    if (Convert.ToString(dr1["Severity"]) != "")
                    {
                        if (Convert.ToString(dr1["OnsetDate"]) != "")
                            sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + " ");
                        else
                            sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                    }
                    //sbTemp.Append("that began on [OnSet Date] "); 

                    if (Convert.ToString(dr1["OnsetDate"]) != "")
                    {
                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" that began on " + Convert.ToString(dr1["OnsetDate"]).Trim() + ". ");
                    }

                    if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms occur " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) == "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) == "")
                    {
                        if (i == 0)
                            sbTemp.Append("Symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("Symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                    }

                    if (Convert.ToString(dr1["NoOfOccurrence"]) != "" && Convert.ToString(dr1["NoOfOccurrence"]) != "0")
                    {
                        if (Convert.ToString(dr1["PriorIllnessDate"]) != "")
                            sbTemp.Append(" problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
                        else
                            sbTemp.Append(" problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times.");
                    }
                    if (Convert.ToString(dr1["PriorIllnessDate"]) != "")
                        sbTemp.Append(" beginning on " + Convert.ToString(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");
                }
                if (Convert.ToString(dr1["AggravatingFactors"]).Trim() != "")
                    sbTemp.Append(" Aggravating factors are: " + Convert.ToString(dr1["AggravatingFactors"]).ToLower().Trim() + ". ");
                if (Convert.ToString(dr1["RelievingFactors"]).Trim() != "")
                    sbTemp.Append(" Relieving factors are:  " + Convert.ToString(dr1["RelievingFactors"]).ToLower().Trim() + ". ");
                if ((Convert.ToString(dr1["DeniesSymptoms1"]) != "") || (Convert.ToString(dr1["DeniesSymptoms2"]) != "") || (Convert.ToString(dr1["DeniesSymptoms3"]) != "") || (Convert.ToString(dr1["DeniesSymptoms4"]) != "") || (Convert.ToString(dr1["DeniesSymptoms5"]) != ""))
                    sbTemp.Append(" Patient denies symptoms of ");
                if (Convert.ToString(dr1["DeniesSymptoms1"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms2"]) != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms2"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms3"]) != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms3"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms4"]) != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms4"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms5"]) != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms5"]) != "")
                {
                    sbTemp.Remove(sbTemp.Length - 2, 2);
                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms5"]).ToLower().Trim() + ". ");
                }
                sbTemp.Append(sEnd);
            }
        }
        sbTemp.Append(EndList);
        if (sbTemp.ToString() != "")
        {
            sb.Append(sbTemplateStyle);
        }
        sb.Append("<br/>");
        sb.Append(sbTemp);
        //sb.Remove(sb.Length - 2, 2);
        //sb.Append(". ");
        return sb;
    }
    public StringBuilder BindProblemsROS(Int64 RegNo, int HospitalId, int EncounterId, StringBuilder sb)
    {
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        Hashtable hsProblems = new Hashtable();
        hsProblems.Add("@inyHospitalLocationID", HospitalId);
        hsProblems.Add("@intRegistrationNo", RegNo);
        hsProblems.Add("@intEncounterId", EncounterId);
        int count = 0;
        //Review Of Systems
        ds = new DataSet();
        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetROS", hsProblems);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append("<br /><u>Review of systems</u>");
            sb.Append("<br />Positive Symptoms:");
        }
        count = 0;
        //Positive Symptoms
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            DataRow dr1 = ds.Tables[0].Rows[i] as DataRow;
            DataRow dr2;
            DataRow dr0;
            if (i + 1 < ds.Tables[0].Rows.Count)
                dr2 = ds.Tables[0].Rows[i + 1] as DataRow;
            else
                dr2 = ds.Tables[0].Rows[i] as DataRow;
            if (i - 1 >= 0)
                dr0 = ds.Tables[0].Rows[i - 1] as DataRow;
            else
                dr0 = ds.Tables[0].Rows[0] as DataRow;
            if (i == 0)
                sb.Append("<br />" + dr1["SectionName"].ToString().ToUpper() + ": He has ");
            if (Convert.ToInt32(dr1["SectionId"]) == Convert.ToInt32(dr2["SectionId"]))
            {
                count++;
                if (Convert.ToInt32(dr1["SectionId"]) == Convert.ToInt32(dr0["SectionId"]))
                    sb.Append(dr1["FieldName"].ToString() + ", ");
                else
                    sb.Append("<br />" + dr1["SectionName"].ToString().ToUpper() + ": He has " + dr1["FieldName"].ToString() + ", ");
            }
            else
            {
                if (Convert.ToInt32(dr1["SectionId"]) == Convert.ToInt32(dr0["SectionId"]))
                {
                    if (count > 0)
                    {
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append(" and " + dr1["FieldName"].ToString() + ".");
                    }
                    else
                        sb.Append(dr1["FieldName"].ToString() + ".");
                    count = 0;
                }
                else
                    sb.Append("<br />" + dr1["SectionName"].ToString().ToUpper() + ": He has " + dr1["FieldName"].ToString() + ".");
            }
        }
        sb.Remove(sb.Length - 2, 2);
        sb.Append(".");
        count = 0;
        //Negative Symptoms
        if (ds.Tables[1].Rows.Count > 0)
            sb.Append("<br /><br />Negitive Symptoms:");
        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
        {
            DataRow dr1 = ds.Tables[1].Rows[i] as DataRow;
            DataRow dr2;
            DataRow dr0;
            if (i + 1 < ds.Tables[1].Rows.Count)
                dr2 = ds.Tables[1].Rows[i + 1] as DataRow;
            else
                dr2 = ds.Tables[1].Rows[i] as DataRow;
            if (i - 1 >= 0)
                dr0 = ds.Tables[1].Rows[i - 1] as DataRow;
            else
                dr0 = ds.Tables[1].Rows[0] as DataRow;
            if (i == 0)
                sb.Append("<br />" + dr1["SectionName"].ToString().ToUpper() + ": He does not have ");
            if (Convert.ToInt32(dr1["SectionId"]) == Convert.ToInt32(dr2["SectionId"]))
            {
                count++;
                if (Convert.ToInt32(dr1["SectionId"]) == Convert.ToInt32(dr0["SectionId"]))
                    sb.Append(dr1["FieldName"].ToString() + ", ");
                else
                    sb.Append("<br />" + dr1["SectionName"].ToString().ToUpper() + ": He does not have " + dr1["FieldName"].ToString() + ", ");
            }
            else
            {
                if (Convert.ToInt32(dr1["SectionId"]) == Convert.ToInt32(dr0["SectionId"]))
                {
                    if (count > 0)
                    {
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append(" and " + dr1["FieldName"].ToString() + ".");
                    }
                    else
                        sb.Append(dr1["FieldName"].ToString() + ".");
                    count = 0;
                }
                else
                    sb.Append("<br />" + dr1["SectionName"].ToString().ToUpper() + ": He does not have " + dr1["FieldName"].ToString() + ".");
            }
        }
        sb.Remove(sb.Length - 2, 2);
        sb.Append(".");
        //sb.Append("</font>");
        return sb;
    }
    public void BindDDLorLstBox(params object[] param)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        hstInput = new Hashtable();
        System.Collections.Specialized.ListDictionary Li = new System.Collections.Specialized.ListDictionary();
        for (int i = 2; i < param.Length; i++)
            hstInput.Add("@Arg" + (i - 1).ToString(), param[i].ToString());
        SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, param[1].ToString(), hstInput); //cmd.ExecuteReader();
        while (dr.Read())
            Li.Add(dr.GetValue(0).ToString(), dr.GetValue(1).ToString());
        if (param[0].GetType().Name == "DropDownList")
            DDL_Bind(param[0] as DropDownList, Li);
        else if (param[0].GetType().Name == "ListBox")
            LIST_Bind(param[0] as ListBox, Li);
        dr.Close();
    }
    public void BindDDLorLstBoxNEW(params object[] param)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        //hstInput = new Hashtable();
        System.Collections.Specialized.ListDictionary Li = new System.Collections.Specialized.ListDictionary();
        //for (int i = 2; i < param.Length; i++)
        //    hstInput.Add("@Arg" + (i - 1).ToString(), param[i].ToString());
        SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, param[1].ToString(), ((Hashtable)param[2])); //cmd.ExecuteReader();
        while (dr.Read())
            Li.Add(dr.GetValue(0).ToString(), dr.GetValue(1).ToString());
        if (param[0].GetType().Name == "DropDownList")
            DDL_Bind(param[0] as DropDownList, Li);
        else if (param[0].GetType().Name == "ListBox")
            LIST_Bind(param[0] as ListBox, Li);
        dr.Close();
    }
    public void BindDDLorLstBox_SqlStmt(params object[] param)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        hstInput = new Hashtable();
        System.Collections.Specialized.ListDictionary Li = new System.Collections.Specialized.ListDictionary();
        for (int i = 2; i < param.Length; i++)
            hstInput.Add("@Arg" + (i - 1).ToString(), param[i].ToString());
        SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, param[1].ToString(), hstInput); //cmd.ExecuteReader();
        while (dr.Read())
            Li.Add(dr.GetValue(0).ToString(), dr.GetValue(1).ToString());
        if (param[0].GetType().Name == "DropDownList")
            DDL_Bind(param[0] as DropDownList, Li);
        else if (param[0].GetType().Name == "ListBox")
            LIST_Bind(param[0] as ListBox, Li);
        dr.Close();
    }
    public void BindDDLorLstBox_SqlStmtNew(params object[] param)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        hstInput = new Hashtable();
        System.Collections.Specialized.ListDictionary Li = new System.Collections.Specialized.ListDictionary();
        //for (int i = 2; i < param.Length; i++)
        //    hstInput.Add("@Arg" + (i - 1).ToString(), param[i].ToString());
        SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, param[1].ToString(), ((Hashtable)param[2])); //cmd.ExecuteReader();
        while (dr.Read())
            Li.Add(dr.GetValue(0).ToString(), dr.GetValue(1).ToString());
        if (param[0].GetType().Name == "DropDownList")
            DDL_Bind(param[0] as DropDownList, Li);
        else if (param[0].GetType().Name == "ListBox")
            LIST_Bind(param[0] as ListBox, Li);
        dr.Close();
    }
    public void BindDataSetToGrid(params object[] param)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        hstInput = new Hashtable();
        for (int i = 2; i < param.Length; i++)
            hstInput.Add("@Arg" + (i - 1).ToString(), param[i].ToString());
        ds = new DataSet();
        ds = objDl.FillDataSet(CommandType.StoredProcedure, param[1].ToString(), hstInput);
        if (param[0].GetType().Name == "GridView")
            GridView_Bind(param[0] as GridView, ds);
        else if (param[0].GetType().Name == "DetailsView")
            DetailsView_Bind(param[0] as DetailsView, ds);
    }
    public void BindDataSetToGridControl(params object[] param)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        hstInput = new Hashtable();
        ds = new DataSet();
        hstInput = param[2] as Hashtable;
        ds = objDl.FillDataSet(CommandType.StoredProcedure, param[1].ToString(), hstInput);

        //for (int i = 2; i < param.Length; i++)
        //   hstInput.Add("@Arg" + (i - 1).ToString(), param[i].ToString());

        if (param[0].GetType().Name == "GridView")
            GridView_Bind(param[0] as GridView, ds);
        else if (param[0].GetType().Name == "DetailsView")
            DetailsView_Bind(param[0] as DetailsView, ds);
        else if (param[0].GetType().Name == "Repeater")
            Repeater_Bind(param[0] as Repeater, ds);
        else if (param[0].GetType().Name == "DataList")
            DataList_Bind(param[0] as DataList, ds);

    }
    public DataSet ExecuteSql(System.Text.StringBuilder str)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        ds = new DataSet();
        ds = objDl.FillDataSet(CommandType.Text, str.ToString());
        return ds;
    }
    public DataSet ReturnDataset(params object[] param)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        hstInput = new Hashtable();
        for (int i = 1; i < param.Length; i++)
            hstInput.Add("@Arg" + (i).ToString(), param[i].ToString());
        ds = new DataSet();
        ds = objDl.FillDataSet(CommandType.StoredProcedure, param[0].ToString(), hstInput);
        return ds;
    }
    public DataSet ReturnDatasetByHastTable(params object[] param)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, ConString);
        hstInput = new Hashtable();
        hstInput = param[1] as Hashtable;
        //for (int i = 1; i < param.Length; i++)
        //    hstInput.Add("@Arg" + (i).ToString(), param[i].ToString());
        ds = new DataSet();
        ds = objDl.FillDataSet(CommandType.StoredProcedure, param[0].ToString(), hstInput);
        return ds;
    }
    public void DDL_Bind(DropDownList ddl, System.Collections.Specialized.ListDictionary Li)
    {
        ddl.DataSource = Li;
        ddl.DataValueField = "key";
        ddl.DataTextField = "value";
        ddl.DataBind();
    }
    public void LIST_Bind(ListBox list, System.Collections.Specialized.ListDictionary Li)
    {
        list.DataSource = Li;
        list.DataValueField = "key";
        list.DataTextField = "value";
        list.DataBind();
    }
    public void GridView_Bind(GridView gv, DataSet ds)
    {
        gv.DataSource = ds;
        gv.DataBind();
    }
    public void DetailsView_Bind(DetailsView dv, DataSet ds)
    {
        dv.DataSource = ds;
        dv.DataBind();
    }
    public void Repeater_Bind(Repeater rp, DataSet ds)
    {
        rp.DataSource = ds;
        rp.DataBind();
    }
    public void DataList_Bind(DataList dl, DataSet ds)
    {
        dl.DataSource = ds;
        dl.DataBind();
    }
    public string USFConvertDateDDMMYYYY_To(string date, string Dateformat, string seperator)
    {
        if (Dateformat == "MMDDYYYY")
        {
            if (seperator == "/")
            {
                char[] sep = { '/' };
                string[] dt = date.Split(sep);
                date = dt[1].ToString() + "/" + dt[0].ToString() + "/" + dt[2].ToString();
            }
            else if (seperator == "-")
            {
                char[] sep = { '-' };
                string[] dt = date.Split(sep);
                date = dt[1].ToString() + "-" + dt[0].ToString() + "-" + dt[2].ToString();
            }
        }
        else if (Dateformat == "DDMMYYYY")
        {

        }
        else if (Dateformat == "YYYYMMDD")
        {
            if (seperator == "/")
            {
                char[] sep = { '/' };
                string[] dt = date.Split(sep);
                string[] tm = dt[2].Split(new char[] { ' ' });
                date = tm[0].ToString() + "/" + dt[1].ToString() + "/" + dt[0].ToString() + " " + tm[1].ToString();
            }
            else if (seperator == "-")
            {
                char[] sep = { '/' };
                string[] dt = date.Split(sep);
                string[] tm = dt[2].Split(new char[] { ' ' });
                date = tm[0].ToString() + "-" + dt[1].ToString() + "-" + dt[0].ToString() + " " + tm[1].ToString();
            }
        }
        return date;
    }
    public string ResolvedDate
    {
        set { _ResolvedDate = value; }
        get { return _ResolvedDate; }
    }
    public string EncodedDate
    {
        set { _EncodedDate = value; }
        get { return _EncodedDate; }
    }
    public int ICDCode
    {
        set { _ICDCode = value; }
        get { return _ICDCode; }
    }
    public string Description
    {
        set { _Description = value; }
        get { return _Description; }
    }
    public int ICDID
    {
        set { _ICDID = value; }
        get { return _ICDID; }
    }
    public int StatusId
    {
        set { _Status = value; }
        get { return _Status; }
    }
    public int Status
    {
        set { _st = value; }
        get { return _st; }
    }
    public int PrimaryDiagnosis
    {
        set { _PrimaryDiagnosis = value; }
        get { return _PrimaryDiagnosis; }
    }
    public string Remarks
    {
        set { _Remarks = value; }
        get { return _Remarks; }
    }
    public int ID
    {
        set { _ID = value; }
        get { return _ID; }
    }
    public string DiagnosisStatus
    {
        set { _DiagnosisStatus = value; }
        get { return _DiagnosisStatus; }
    }
    public string IsResolved
    {
        set { _IsResolved = value; }
        get { return _IsResolved; }
    }
    public DataView FillTopMenu()
    {

        DataSet ds = new DataSet();
        DataView dvModule = new DataView();
        ds = (DataSet)HttpContext.Current.Cache["SubOtherModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] +
            System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                System.Web.HttpContext.Current.Session["Check"] = 1;
                dvModule = new DataView(ds.Tables[0]);
                dvModule.RowFilter = "ModuleId=" + common.myStr(System.Web.HttpContext.Current.Session["ModuleIdValue"]) + " AND StaticPage=1";
            }
        }
        else
        {
            ds = (DataSet)HttpContext.Current.Cache["SubEHRModuleName" + System.Web.HttpContext.Current.Session["RegistrationID"] +
            System.Web.HttpContext.Current.Session["encounterid"] + System.Web.HttpContext.Current.Session["UserID"] + System.Web.HttpContext.Current.Session["HospitalLocationID"]];
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session["Check"] = 1;
                    dvModule = new DataView(ds.Tables[0]);
                    dvModule.RowFilter = "ModuleId=" + common.myStr(System.Web.HttpContext.Current.Session["ModuleIdValue"]) + "";
                }
            }

        }
        if (common.myInt(System.Web.HttpContext.Current.Session["Check"]) == 0)
        {
            TreeView tv1 = new TreeView(); ;
            BindPages(1, tv1);
            System.Web.HttpContext.Current.Session["Check"] = 0;
            FillTopMenu();

        }
        return dvModule;

    }

}
