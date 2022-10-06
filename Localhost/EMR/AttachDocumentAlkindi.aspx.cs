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
using System.IO;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using Ionic.Zip;

public partial class EMR_AttachDocument : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    Hashtable hstInput;
    BaseC.ATD objatd;
    BaseC.Patient ObjPat;
    DataSet ds;
    string[] strdocType = { ".pdf", ".doc", ".xls", ".docx", ".xlsx", ".txt", ".zip" };

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";


        //if (Request.QueryString["Reg"] != null)
        //{
        //    if (common.myStr(Request.QueryString["Reg"]) != string.Empty)
        //    {
        //        if (common.myStr(Request.QueryString["Reg"]) == "1")
        //        {
        //            Page.MasterPageFile = "/Include/Master/RegistrationAttachDocumentMaster.master";
        //        }


        //    }

        //}
        //else
        //{ 


        if (Request.QueryString["ATD"] != "ATD")
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
            //  btnClose.Visible = false;
        }
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            }
        }
        else
        {
            if (common.myStr(Request.QueryString["MASTER"]) == "No")
            {
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            }
        }




    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool x = btnUpload.Visible;

        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                tdName.Visible = false;
                tdSer.Visible = false;
                btnNo.Visible = true;
                reg.Visible = false;
            }
        }
        if (Request.QueryString["Mpg"] != null)
        {
            Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
        }

        if (!IsPostBack)
        {

            if (common.myStr(Session["ModuleName"]).Equals("EMR")
               || common.myStr(Session["ModuleName"]).Equals("WARD MANAGEMENT")
               || common.myStr(Session["ModuleName"]).Equals("NURSE WORKBENCH"))
            {
                lblShowInEMRCaseSheet.Visible = true;
                chkShowInEMRCaseSheet.Visible = true;
            }
            else
            {
                lblShowInEMRCaseSheet.Visible = false;
                chkShowInEMRCaseSheet.Visible = false;
            }


            ViewState["FileName"] = string.Empty;
            hstInput = new Hashtable();

            SetPermission();

            if (Request.QueryString["FromEMR"] != null)
            {
                if (common.myStr(Request.QueryString["FromEMR"]) != string.Empty)
                {
                    if (common.myStr(Request.QueryString["FromEMR"]).Equals("1"))
                    {
                        asplHeaderUDEMR.Visible = true;
                        asplHeaderUD.Visible = false;
                        asplHeaderUDNurse.Visible = false;
                        lbtnSearchPatient.Visible = false;
                    }
                    else
                    {
                        asplHeaderUDNurse.Visible = false;
                        asplHeaderUDEMR.Visible = false;
                        asplHeaderUD.Visible = true;
                        lbtnSearchPatient.Visible = true;
                    }
                }
                else
                {
                    asplHeaderUDNurse.Visible = false;
                    asplHeaderUDEMR.Visible = false;
                    asplHeaderUD.Visible = true;
                    lbtnSearchPatient.Visible = true;
                }
            }

            else if (Request.QueryString["Nurse"] != null)
            {
                if (common.myStr(Request.QueryString["Nurse"]) != string.Empty)
                {
                    if (common.myStr(Request.QueryString["Nurse"]).Equals("1"))
                    {
                        asplHeaderUDEMR.Visible = false;
                        asplHeaderUD.Visible = false;
                        asplHeaderUDNurse.Visible = true;
                        lbtnSearchPatient.Visible = false;
                    }
                    else
                    {
                        asplHeaderUDNurse.Visible = false;
                        asplHeaderUDEMR.Visible = false;
                        asplHeaderUD.Visible = true;
                        lbtnSearchPatient.Visible = true;
                    }
                }
                else
                {
                    asplHeaderUDNurse.Visible = false;
                    asplHeaderUDEMR.Visible = false;
                    asplHeaderUD.Visible = true;
                    lbtnSearchPatient.Visible = true;
                }
            }
            else
            {
                asplHeaderUDNurse.Visible = false;
                asplHeaderUDEMR.Visible = false;
                asplHeaderUD.Visible = true;
                lbtnSearchPatient.Visible = true;
            }

            if (common.myStr(txtRegNo.Text) == "")
            {
                ObjPat = new BaseC.Patient(sConString);
                string strregno = "";
                strregno = ObjPat.GetPatientRegistrationNo(common.myInt(Session["RegistrationID"]));
                if (common.myInt(Session["RegistrationID"]) > 0)
                    hdnRegistrationId.Value = common.myStr(Session["RegistrationID"]);
                else
                    hdnRegistrationId.Value = "0";
                ViewState["RegNo"] = strregno.ToString();
                txtRegNo.Text = strregno.ToString();
                txtAccountNo.Text = strregno.ToString();
                hdnEMRPatientDetails.Value = strregno.ToString();
                Session["RNo"] = strregno.ToString();
            }

            Session["RNo"] = null;
            if (Request.QueryString["RNo"] != null)
            {
                txtRegNo.Text = Request.QueryString["RNo"].ToString();
                Session["RNo"] = txtRegNo.Text.Trim();
            }
            else
            {
                Session["RNo"] = txtRegNo.Text == "" ? Session["RNo"] : txtRegNo.Text;
            }
            txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            if (Request.QueryString["RegId"] != null && Request.QueryString["EncId"] != null)
            {
                ObjPat = new BaseC.Patient(sConString);
                txtRegNo.Text = ObjPat.GetPatientRegistrationNo(common.myInt(Request.QueryString["RegId"]));
                txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
                BindTree();
                bindImage("0");
                bindDocumentCategory();
                bindDocumentType();
            }

            if (Request.QueryString["RNo"] == null && Session["RNo"] == null && txtRegNo.Text == string.Empty)
            {
                tddetails.Visible = false;
                bindDocumentCategory();
                bindDocumentType();
            }
            else
            {
                tddetails.Visible = true;
                txtRegNo.Text = Convert.ToString(Request.QueryString["RNo"]) == null ? Convert.ToString(Session["RNo"]) : Convert.ToString(Request.QueryString["RNo"]);



                bindImage("0");
                bindDocumentCategory();
                bindDocumentType();
                txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
                BindTree();


            }
            if (Session["CurrentNode"] != null)
            {
                if (Session["CurrentNode"].ToString() == "P64")
                {
                    if (Convert.ToString(Session["encounterid"]) == "")
                    {
                        Response.Redirect("/default.aspx?RegNo=0", false);
                    }

                    tddetails.Visible = false;
                    txtRegNo.Text = common.myStr(ViewState["RegNo"]);
                    txtAccountNo.Text = common.myStr(txtRegNo.Text);


                    // string strName = ComboPatientSearch.GetSelectedItemText();
                    // txt_hdn_PName.Text = strName;
                    pdetails.Visible = false;

                    txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
                    BindTree();
                    //bindImage("0");
                    bindDocumentCategory();
                    bindDocumentType();

                    if (Session["EncounterId"] != null && Session["EncounterId"].ToString() != "0")
                    {
                        //done by rakesh for user authorisation start
                        //btnUpload.Enabled = true;
                        SetPermission(btnUpload, "N", true);
                        //done by rakesh for user authorisation end
                        TreeView1.Enabled = true;
                        _FileUpload.Enabled = true;
                        ddlCategory.Enabled = true;
                        chkAll.Enabled = true;
                        txtDate.Enabled = true;
                        txtDescription.Enabled = true;
                        txtRemarks.Enabled = true;
                        btnDocumentCategory.Enabled = true;
                        RadWindowManager1.Enabled = true;
                        RadListView1.Visible = true;
                    }
                    else
                    {
                        btnUpload.Enabled = false;
                        TreeView1.Enabled = false;
                        _FileUpload.Enabled = false;
                        ddlCategory.Enabled = false;
                        btnDocumentCategory.Enabled = false;
                        chkAll.Enabled = false;
                        txtDate.Enabled = false;
                        txtDescription.Enabled = false;
                        txtRemarks.Enabled = false;
                        pnlImages.Enabled = false;
                        RadWindowManager1.Enabled = false;
                        RadListView1.Visible = false;

                        ltrlMessage.Text = "<span style='color:Red; font-weight:Bold'> No Active Encounter Exist for this Appointment</span>";
                    }
                }
                if (Request.QueryString["ATD"] == "ATD")
                {
                    //objatd = new BaseC.ATD(sConString);
                    //ds = new DataSet();
                    //ds = objatd.GetRegistrationId(common.myInt(Request.QueryString["Regno"]));
                    //txtRegNo.Text = ds.Tables[0].Rows[0]["Id"].ToString().Trim();
                    reg.Visible = false;
                    tddetails.Visible = false;
                    pdetails.Visible = false;
                    txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
                    BindTree();

                    bindDocumentCategory();
                    bindDocumentType();
                }
            }

            PatientInfo();
            BindRegistrationId();
            BindVitistNo();
            if (common.myLen(txtAccountNo.Text) > 0)
            {
                txtAccountNo.ReadOnly = true;
                txtAccountNo.Enabled = false;
            }
        }
        if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
        {
            btnUpload.Visible = false;


        }
        if (common.myStr(Request.QueryString["CloseButtonShow"]) == "No")
        {
            ibtnClose.Visible = false;
        }
        if (Request.QueryString["Reg"] != null)
        {
            if (common.myStr(Request.QueryString["Reg"]) != string.Empty)
            {
                if (common.myStr(Request.QueryString["Reg"]) == "1")
                {
                    ibtnClose.Visible = false;
                }
            }
        }

        //if (Request.QueryString["Reg"] != null)
        //{
        //    if (common.myStr(Request.QueryString["Reg"]) != string.Empty)
        //    {
        //        if (common.myStr(Request.QueryString["Reg"]) == "1")
        //        {
        //            BaseC.Patient bC = new BaseC.Patient(sConString);
        //            if (common.myLen(txtAccountNo.Text) > 0)
        //            {

        //           if(Session["EncounterNoReg"]!=null )
        //                { 
        //       //         Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(txtAccountNo.Text),
        //       //common.myStr (Session["EncounterNoReg"]), common.myInt(Session["UserId"]));
        //                }
        //            }
        //        }


        //    }

        //}

        if (Request.QueryString["Reg"] != null && Request.QueryString["Nurse"] == null)
        {
            if (common.myStr(Request.QueryString["Reg"]) != string.Empty)
            {
                if (common.myStr(Request.QueryString["Reg"]) == "1")
                {
                    btnNew.Visible = true;
                }


            }

        }

    }

    private void PatientInfo()
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.ParseData bParse = new BaseC.ParseData();
        try
        {
            //if (Convert.ToString(Request.QueryString["RNo"]) != "")
            if (common.myStr(Session["RNo"]) != "")
            {
                if (common.myStr(Session["RNo"]) != "")
                {
                    SqlDataReader dr;

                    string sXSL = "";
                    //txtAccountNo.Text = bC.GetPatientRegistrationNo(Convert.ToInt32(Session["RNo"]));
                    txtAccountNo.Text = common.myStr(Session["RNo"]);
                    if (common.myStr(Session["RNo"]) != string.Empty)
                    {
                        Session["RegistrationNoReg"] = common.myStr(Session["RNo"]);
                    }
                    if (common.myLen(txtAccountNo.Text) > 0 && Convert.ToInt64(txtAccountNo.Text) > 2147483647)
                    {
                        return;
                    }
                    //dr = (SqlDataReader)bC.getPatientDetails(Convert.ToInt32(bParse.ParseQ(Convert.ToString(Session["RNo"]))), 1, Convert.ToInt32(Session["FacilityId"]));
                    sXSL = "/include/xsl/PatientInfo-US.xsl";

                    //dr.NextResult();
                    //if (dr.Read())
                    //{
                    //    xmlPatientInfo.DocumentContent = dr[0].ToString();
                    //    xmlPatientInfo.TransformSource = sXSL;
                    //    xmlPatientInfo.DataBind();
                    //}
                    //else
                    //{
                    //    xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                    //    xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                    //    xmlPatientInfo.DataBind();

                    //}
                    //dr.Close();
                }
            }
            //else
            //{
            //    xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
            //    xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
            //    xmlPatientInfo.DataBind();
            //}

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added bys sikandar for code optimize
        finally
        {
            bC = null;
            bParse = null;
        }
    }

    protected void BindTree()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.ParseData objParse = new BaseC.ParseData();
        try
        {
            if (Session["HospitalLocationID"] != null)
            {
                if (common.myLen(txtAccountNo.Text) > 0 && Convert.ToInt64(txtAccountNo.Text) > 2147483647)
                {
                    return;
                }
                DataSet objDs = new DataSet();
                TreeView1.Nodes.Clear();

                hstInput = new Hashtable();

                if (txtRegNo.Text == "")
                {
                    hstInput.Add("@intRegistrationId", txtRegNo.Text.Trim());
                    if (chkAll.Checked == false)
                    {
                        hstInput.Add("@intEncounterId", -1);
                    }

                    //Retrieving two resultsets - one for department and other sub-department
                    objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspRetrievePatientDocuments", hstInput);
                    if (objDs != null)
                    {
                        if (objDs.Tables.Count > 0)
                        {
                            ViewState["TreeParentName"] = objDs.Tables[0];
                            ViewState["TreeChildName"] = objDs.Tables[1];
                            int iCount = 0;
                            for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                            {
                                iCount++;
                                AddNodes(TreeView1, objDs.Tables[0].Rows[i][0].ToString(), "", Convert.ToString(objDs.Tables[0].Rows[i][1]));
                            }
                        }
                    }
                }
                else
                {
                    hstInput.Add("@intRegistrationId", txtRegNo.Text);
                    //if (Convert.ToString(Session["EncounterId"]) != null && Convert.ToString(Session["EncounterId"]) != "")
                    hstInput.Add("@intEncounterId", -1);


                    objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspRetrievePatientDocuments", hstInput);
                    ViewState["TreeParentName"] = objDs.Tables[0];
                    ViewState["TreeChildName"] = objDs.Tables[1];

                    objDs.Relations.Add("Child", objDs.Tables[0].Columns["ImageCategoryId"], objDs.Tables[1].Columns["ImageCategoryId"]);
                    if (objDs != null)
                    {
                        if (objDs.Tables.Count > 0)
                        {
                            //binding it to tree control

                            string strTmpDid = "";

                            //present in the table parent

                            foreach (DataRow masterrow in objDs.Tables[0].Rows)
                            {
                                TreeNode masternode = new TreeNode(masterrow["Description"].ToString(), "P" + masterrow["ImageCategoryId"].ToString());
                                //binding master row data to the treeview master node

                                TreeView1.Nodes.Add(masternode);
                                masternode.SelectAction = TreeNodeSelectAction.SelectExpand;

                                strTmpDid = masterrow["ImageCategoryId"].ToString();
                                //loop for binding the child row to treeview

                                //that make a relation to the Link_id field

                                foreach (var childrow in masterrow.GetChildRows("Child"))
                                {
                                    //binding the child row to the treeview

                                    //with value as the URL of the field
                                    TreeNode childnode = new TreeNode(childrow["Description"].ToString(), "C" + childrow["ID"].ToString());

                                    //childnode.SelectAction = TreeNodeSelectAction.None;
                                    masternode.ChildNodes.Add(childnode);
                                }
                            }
                        }
                    }
                }
                if (objDs != null)
                {
                    if (objDs.Tables.Count > 0)
                    {
                        if (objDs.Tables[0].Rows.Count == 0)
                        {
                            RadListView1.DataSource = null;
                            RadListView1.DataBind();
                        }
                    }
                }

                if (TreeView1.Nodes.Count > 0)
                {
                    TreeView1.Nodes[0].Select();
                    //TreeView1.Nodes[0].Selected = true;
                    TreeView1.ExpandAll();
                    TreeView1.PopulateNodesFromClient = true;
                    TreeView1.ShowLines = true;
                    // TreeView1_SelectedNodeChanged(this, null);
                    if (TreeView1.SelectedNode.Value != null)
                    {
                        ltrlMessage.Text = "";
                        bindImage("00");

                        TreeView1.SelectedNode.Selected = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            objDl = null;
            objParse = null;
        }
    }

    private void AddNodes(TreeView tvName, string iNodeID, string iParentID, String sNodeText)
    {
        try
        {
            if (iParentID == "0")
            {
                TreeNode masternode;
                //masternode = new TreeNode(sNodeText.ToString(), iNodeID.ToString(), "", "", "");
                masternode = new TreeNode("Encounter: " + iNodeID.Substring(2, iNodeID.Length - 2) + "(" + sNodeText.ToString() + ")", iNodeID.ToString(), "", "", "");
                // "Encounter: "+ masterrow["EncounterId"].ToString() +"("+ masterrow["EncodedDate"].ToString() +")"
                tvName.Nodes.Add(masternode);
            }
            else
            {
                TreeNode masternode = new TreeNode();
                masternode = tvName.FindNode(iParentID.ToString());
                if (masternode != null)
                {
                    TreeNode childNode;
                    childNode = new TreeNode(sNodeText.ToString(), iNodeID.ToString(), "", "", "");
                    childNode.Text = sNodeText.ToString();
                    masternode.ChildNodes.Add(childNode);
                }
                else
                {
                    CallRecursive(tvName, iNodeID, iParentID, sNodeText);
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

    private void CallRecursive(TreeView tvName, string iNodeID, String sParentID, String sNodeText)
    {
        try
        {
            TreeNodeCollection nodes = tvName.Nodes;
            foreach (TreeNode n in nodes)
            {
                ReCallRecursive(n, iNodeID, sParentID, sNodeText);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void ReCallRecursive(TreeNode treeNode, string iNodeID, String sParentID, String sNodeText)
    {
        foreach (TreeNode tn in treeNode.ChildNodes)
        {
            if (tn.Value == sParentID.ToString())
            {
                TreeNode childNode;
                childNode = new TreeNode(sNodeText.ToString(), iNodeID.ToString(), "", "", "");
                tn.ChildNodes.Add(childNode);
            }
            ReCallRecursive(tn, iNodeID, sParentID, sNodeText);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string SelectedNodeType = string.Empty;
        if (TreeView1.SelectedNode.Value != null)
        {
            SelectedNodeType = common.myStr(TreeView1.SelectedNode.Value).Substring(0, 1);
            ltrlMessage.Text = "";

            if (SelectedNodeType.Equals("C"))
            {
                bindImage("0");
            }
            else if (SelectedNodeType.Equals("P"))
            {
                bindImage("00");
            }

            TreeView1.SelectedNode.Selected = false;
        }
    }

    protected string getFileName(string sId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string sFileName = "";
        try
        {
            hstInput = new Hashtable();
            hstInput.Add("Id", sId);

            SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select ImagePath, ImageName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks from EMRPatientDocuments WITH (NOLOCK) where id = @Id", hstInput);
            if (objDr.Read())
            {
                sFileName = objDr["ImagePath"].ToString();
                //dvImageInformation.InnerHtml = objDr["ImageName"].ToString() + "<br />" + objDr["Description"].ToString();
            }
            objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        //added by sikandar for code optimize
        finally
        {
            objDl = null;

        }

        return sFileName;
    }

    private String sReadText(String sPath)
    {
        String sTxt = "";
        try
        {
            StreamReader SR = File.OpenText(Server.MapPath(sPath));
            string input = null;
            while ((input = SR.ReadLine()) != null)
            {
                sTxt = sTxt + input;
            }
            SR.Close();
            return sTxt;
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            return sTxt = "File Read Failed Due To The Following Reason" + Ex.ToString();
        }
    }

    protected void lnkDemographics_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/Demographics.aspx?mode=E&RNo=" + txtRegNo.Text.ToString().Trim() + "&Category=PopUp", false);
            }
            else
            {
                Response.Redirect("/PRegistration/Demographics.aspx?mode=E&RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
            }
        }
        else
        {
            Response.Redirect("/PRegistration/Demographics.aspx?mode=E&RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
        }
    }

    protected void lnkPatientRelation_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/PatientContacts.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "&Category=PopUp", false);
            }
            else
            {
                Response.Redirect("/PRegistration/PatientContacts.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
            }
        }
        else
        {
            Response.Redirect("/PRegistration/PatientContacts.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
        }
    }

    protected void lnkOtherDetails_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/PatientOtherDetails.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "&Category=PopUp", false);
            }
            else
            {
                Response.Redirect("/PRegistration/PatientOtherDetails.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
            }
        }
        else
        {
            Response.Redirect("/PRegistration/PatientOtherDetails.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
        }
    }

    protected void lnkPayment_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/PaymentDetails.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "&Category=PopUp", false);
            }
            else
            {
                Response.Redirect("/PRegistration/PaymentDetails.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
            }
        }
        else
        {
            Response.Redirect("/PRegistration/PaymentDetails.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
        }
    }

    protected void lnkResponsibleParty_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/ResponsibleParty.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "&Category=PopUp", false);
            }
            else
            {
                Response.Redirect("/PRegistration/ResponsibleParty.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
            }
        }
        else
        {
            Response.Redirect("/PRegistration/ResponsibleParty.aspx?RNo=" + txtRegNo.Text.ToString().Trim() + "", false);
        }
    }

    protected void btnrefreshCategory_Click(object sender, EventArgs e)
    {
        bindDocumentCategory();
        bindDocumentType();
        BindTree();
        bindImage("0");
        ltrlMessage.Text = "";
    }

    protected void btnDocumentCategory_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/DocumentCategory.aspx";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 550;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnDocmenttype_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/DocumentCategory.aspx?DcoType=DcoType";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 550;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.Title = "Time Slot";
        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        BaseC.ParseData objParse = new BaseC.ParseData();
        BaseC.Patient objPatient = new BaseC.Patient(sConString);

        try
        {



            DataTable dtTreeChildName = new DataTable();


            dtTreeChildName = (DataTable)ViewState["TreeChildName"];

            DataView dv = new DataView();
            dv = dtTreeChildName.DefaultView;
            dv.RowFilter = "DocumentCategory='" + common.myStr(ddlCategory.SelectedItem.Text) + "' AND Description='" + common.myStr(txtDescription.Text) + "'";

            if (dv.ToTable().Rows.Count > 0)
            {
                Alert.ShowAjaxMsg("File name already exists for this Category!", this);
                return;
            }


            if (Session["HospitalLocationId"] != null)
            {
                StringBuilder objStr = new StringBuilder();
                string sFileName = "";
                string sFileSize;

                if (_FileUpload.FileName != "")
                {
                    if (txtDescription.Text.Trim().Length == 0)
                    {
                        ltrlMessage.Text = "<span style='color:Red; font-weight:Bold'>Please Enter The Description...</span>";
                        txtDescription.Focus();
                        return;
                    }
                    if (txtDate.Text.Trim().Length == 0)
                    {
                        ltrlMessage.Text = "<span style='color:Red; font-weight:Bold'>Please Enter The Date...</span>";
                        txtDescription.Focus();
                        return;
                    }
                    //"/PatientDocuments/" + HospitalLocationId + "/" + RegistrationId

                    //if (Request.QueryString["RNo"] == null)
                    if (txtRegNo.Text == "")
                    {
                        Alert.ShowAjaxMsg("Please Select The Patient", Page);
                        return;
                    }

                    if (common.myInt(ddlCategory.SelectedValue).Equals(0))
                    {
                        Alert.ShowAjaxMsg("Please Select Category", Page);
                        return;
                    }

                    if (txtRegNo.Text == "")
                    {
                        checkDirectory(txtRegNo.Text.ToString().Trim(), Session["HospitalLocationID"].ToString());
                    }
                    else
                    {
                        checkDirectory(txtRegNo.Text, Session["HospitalLocationID"].ToString());
                    }

                    sFileName = CreateFileName(ddlCategory.SelectedValue, _FileUpload.FileName);
                    sFileSize = _FileUpload.PostedFile.ContentLength.ToString() + " KB";
                    //if (_FileUpload.PostedFile.ContentLength > 10485760)
                    if (_FileUpload.PostedFile.ContentLength > 20971520)
                    {
                        Alert.ShowAjaxMsg("The file you uplaod is too large.", Page);
                        return;
                    }
                    else
                    {
                        string zipFileName = "";

                        if (Convert.ToString(Path.GetExtension(_FileUpload.FileName)).ToLower().Trim() == ".zip")
                        {


                            int extIndex = sFileName.IndexOf(".");
                            string sExt = sFileName.Substring(extIndex, sFileName.Length - extIndex);
                            zipFileName = sFileName.Substring(0, extIndex);
                            zipFileName = zipFileName + ".zip";
                            //var saveToFilename = Server.MapPath("~/Uploads/" + Path.GetFileNameWithoutExtension(fupUploadAndZip.FileName) + ".zip");
                            if (txtRegNo.Text == "")
                            {
                                using (var zip = new ZipFile())
                                {

                                    zip.Password = Session["HospitalLocationID"].ToString() + "@" + txtRegNo.Text.ToString().Trim();
                                    zip.AddEntry(sFileName, _FileUpload.FileContent);
                                    zip.Save(Server.MapPath("/PatientDocuments/") + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text + "/" + zipFileName);
                                }
                            }
                            else
                            {
                                using (var zip = new ZipFile())
                                {
                                    zip.Password = Session["HospitalLocationID"].ToString() + "@" + txtRegNo.Text.ToString().Trim();
                                    zip.AddEntry(sFileName, _FileUpload.FileContent);

                                    zip.Save(Server.MapPath("/PatientDocuments/") + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text + "/" + zipFileName);
                                }

                            }
                        }
                        else
                        {
                            if (txtDescription.Text.Trim().Length == 0)
                            {
                                ltrlMessage.Text = "<span style='color:Red; font-weight:Bold'>Please Enter The Description...</span>";
                                txtDescription.Focus();
                                return;
                            }

                            if (txtDescription.Text.Trim().Length == 0)
                            {
                                ltrlMessage.Text = "<span style='color:Red; font-weight:Bold'>Please Enter The Description...</span>";
                                txtDescription.Focus();
                                return;
                            }


                            if (txtRegNo.Text == "")
                            {
                                checkDirectory(txtRegNo.Text.ToString().Trim(), Session["HospitalLocationID"].ToString());
                            }
                            else
                            {
                                checkDirectory(txtRegNo.Text, Session["HospitalLocationID"].ToString());
                            }

                            sFileName = CreateFileName(ddlCategory.SelectedValue, _FileUpload.FileName);

                            if (txtRegNo.Text == "")
                            {
                                _FileUpload.SaveAs(Server.MapPath("/PatientDocuments/") + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text.ToString().Trim() + "/" + sFileName);
                            }
                            else
                            {
                                _FileUpload.SaveAs(Server.MapPath("/PatientDocuments/") + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text.ToString().Trim() + "/" + sFileName);
                            }
                        }
                        objStr.Append("<Table1>");
                        objStr.Append("<c1>");
                        if (txtRegNo.Text == "")
                        {
                            objStr.Append("/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text.ToString().Trim() + "/" + sFileName);
                        }
                        else
                        {
                            objStr.Append("/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text + "/" + sFileName);
                        }
                        objStr.Append("</c1>");
                        objStr.Append("<c2>");
                        objStr.Append(objParse.ParseQ(txtDescription.Text));
                        objStr.Append("</c2>");
                        objStr.Append("<c3>");
                        objStr.Append(ddlCategory.SelectedValue);
                        objStr.Append("</c3>");
                        objStr.Append("<c4>");
                        objStr.Append(sFileName);
                        objStr.Append("</c4>");
                        objStr.Append("<c5>");
                        objStr.Append(objParse.ParseQ(Convert.ToString(sFileSize)));
                        objStr.Append("</c5>");
                        objStr.Append("<c6>");
                        if (Convert.ToString(Path.GetExtension(_FileUpload.FileName)).ToLower().Trim() == ".zip")
                        {
                            objStr.Append(objParse.ParseQ(zipFileName));
                        }
                        else
                        {
                            objStr.Append(objParse.ParseQ(sFileName));
                        }
                        objStr.Append("</c6>");
                        objStr.Append("<c7>");
                        objStr.Append(objParse.ParseQ(txtRemarks.Text));
                        objStr.Append("</c7>");
                        objStr.Append("<c8>");
                        objStr.Append(objPatient.FormatDateDateMonthYear(txtDate.Text));
                        objStr.Append("</c8>");
                        objStr.Append("</Table1>");
                        if (objStr.ToString() != "")
                        {
                            StringBuilder strSQL = new StringBuilder();
                            //byte[] byteImageData;
                            // String FileName = "";

                            String FilePath = "";
                            if (txtRegNo.Text == "")
                            {
                                FilePath = Server.MapPath("/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text.ToString().Trim() + "/" + sFileName);
                            }
                            else
                            {
                                FilePath = Server.MapPath("/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text.ToString() + "/" + sFileName);
                            }

                            using (SqlConnection objCon = new SqlConnection(sConString))
                            {
                                using (SqlCommand objCmd = new SqlCommand("UspSavePatientDocuments", objCon))
                                {
                                    objCmd.CommandType = CommandType.StoredProcedure;

                                    objCmd.Parameters.AddWithValue("@bitIsShowInEMRCaseSheet", chkShowInEMRCaseSheet.Checked ? "1" : "0");

                                    if (txtRegNo.Text != "")
                                    {
                                        objCmd.Parameters.AddWithValue("intRegistrationId", txtRegNo.Text.ToString());
                                    }
                                    else
                                    {
                                        objCmd.Parameters.AddWithValue("intRegistrationId", txtRegNo.Text.ToString().Trim());
                                    }
                                    //if (Convert.ToString(Session["EncounterId"]) != null && Convert.ToString(Session["EncounterId"]) != "")
                                    if (common.myInt(ddlvisit.SelectedValue) != 0)
                                    {
                                        objCmd.Parameters.AddWithValue("intEncounterId", common.myInt(ddlvisit.SelectedValue));
                                    }
                                    string ext = Path.GetExtension(sFileName);
                                    if (ext == ".txt" || ext == ".rtf")
                                    {
                                        ext = ".doc";
                                    }
                                    if (strdocType.Contains<string>(ext))
                                    {
                                        if (ext.ToLower() == ".doc" || ext.ToLower() == ".docx")
                                            FilePath = "/Images/WordThumbnail.png";
                                        else if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
                                            FilePath = "/Images/Excel.png";
                                        else if (ext.ToLower() == ".pdf")
                                            FilePath = "/Images/pdf.jpg";
                                        else if (ext.ToLower() == ".zip")
                                            FilePath = "/Images/winzip-icon.jpg";


                                        objCmd.Parameters.AddWithValue("chvThumbnail", FilePath);
                                    }
                                    else
                                        objCmd.Parameters.AddWithValue("chvThumbnail", "/Images/nothumbnail.jpg");


                                    objCmd.Parameters.AddWithValue("xmlDocumentDetails", objStr.ToString());
                                    objCmd.Parameters.AddWithValue("intEncodedBy", Convert.ToInt16(Session["UserID"]));

                                    objCmd.Parameters.AddWithValue("chvDocumentType", ext);
                                    if (common.myStr(Request.QueryString["Usefor"]) == "MRD")
                                    {
                                        objCmd.Parameters.AddWithValue("intDocumentTypeId", 3);
                                    }
                                    //@ objCmd.Parameters.AddWithValue("intDocumentId", ddlDcomenttype.SelectedValue);
                                    objCmd.Parameters.AddWithValue("chvDocType", common.myStr(ddlCategory.SelectedItem.Text));
                                    objCon.Open();
                                    int i = objCmd.ExecuteNonQuery();
                                    objCon.Close();
                                    BindTree();
                                    ltrlMessage.Text = "<span style='color:green; font-weight:Bold'>Document Uploaded!</span>";
                                    txtRemarks.Text = "";
                                    
                                    txtDescription.Text = "";
                                    bindImage("0");
                                    chkShowInEMRCaseSheet.Checked = false;
                                }

                            }
                        }
                    }
                }
            }



        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {
            objParse = null;
            ObjPat = null;


        }
    }

    protected Unit ImageWidth
    {
        get
        {
            object state = ViewState["ImageWidth"] ?? Unit.Pixel(200);
            return (Unit)state;
        }
        private set { ViewState["ImageWidth"] = value; }
    }

    protected Unit ImageHeight
    {
        get
        {
            object state = ViewState["ImageHeight"] ?? Unit.Pixel(200);
            return (Unit)state;
        }
        private set { ViewState["ImageHeight"] = value; }
    }

    private void bindImage(string sortCriteria)
    {
        string fileName = string.Empty;
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet objDr = new DataSet();
        try
        {
            string strSQL = "";
            if (txtRegNo.Text != "")
            {
                if (TreeView1.Nodes.Count > 0)
                {
                    if (TreeView1.SelectedNode != null)
                    {
                        //string sFileName = "";
                        hstInput = new Hashtable();

                        string smappath = Server.MapPath("");
                        hstInput.Add("ImageCategoryId", TreeView1.SelectedNode.Value.Substring(1, TreeView1.SelectedNode.Value.Length - 1));

                        if (TreeView1.SelectedNode.Value.ToLower().StartsWith("c") == false)
                        {
                            hstInput.Add("RegistrationId", (common.myInt(hdnRegistrationId.Value) > 0) ? common.myInt(hdnRegistrationId.Value) : common.myInt(Session["RegistrationID"])); //txtRegNo.Text.ToString().Trim());
                            //hstInput.Add("HospitalLocationId", Session["HospitalLocationId"].ToString());
                            if (chkAll.Checked)
                            {
                                strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                                strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                                strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                                strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type ";
                                strSQL += " FROM EMRPatientDocuments pd WITH (NOLOCK) where RegistrationID = @RegistrationId  AND Active=1";

                                objDr = objDl.FillDataSet(CommandType.Text, strSQL, hstInput);
                            }
                            else
                            {
                                strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                                strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                                strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                                strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type ";
                                strSQL += " FROM EMRPatientDocuments pd WITH (NOLOCK) where RegistrationID = @RegistrationId And ImageCategoryId = @ImageCategoryId AND Active=1 ";

                                objDr = objDl.FillDataSet(CommandType.Text, strSQL, hstInput);
                            }
                        }
                        else
                        {
                            strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                            strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                            strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type ";
                            strSQL += " FROM EMRPatientDocuments pd WITH (NOLOCK) where pd.Id = @ImageCategoryId  AND Active=1";

                            objDr = objDl.FillDataSet(CommandType.Text, strSQL, hstInput);
                        }
                    }
                }
            }
            else
            {
                if (TreeView1.SelectedNode != null)
                {
                    string sFileName = "";
                    hstInput = new Hashtable();

                    sFileName = TreeView1.SelectedNode.Value.ToLower().Substring(0, 1);
                    if (chkAll.Checked == true)
                    {
                        hstInput.Add("RegistrationId", (common.myInt(hdnRegistrationId.Value) > 0) ? common.myInt(hdnRegistrationId.Value) : common.myInt(Session["RegistrationID"])); //txtRegNo.Text.ToString().Trim());
                        strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                        strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                        strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                        strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type ";
                        strSQL += " FROM EMRPatientDocuments pd WITH (NOLOCK) where RegistrationID = @RegistrationId  AND Active=1";

                        objDr = objDl.FillDataSet(CommandType.Text, strSQL, hstInput);
                    }
                    else
                    {
                        if (sFileName == "p")
                        {
                            hstInput.Add("RegistrationId", (common.myInt(hdnRegistrationId.Value) > 0) ? common.myInt(hdnRegistrationId.Value) : common.myInt(Session["RegistrationID"]));
                            hstInput.Add("ImageCategoryId", TreeView1.SelectedNode.Value.Substring(1, TreeView1.SelectedNode.Value.Length - 1));
                            strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                            strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                            strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type ";
                            strSQL += " FROM EMRPatientDocuments pd WITH (NOLOCK) where RegistrationID = @RegistrationId And ImageCategoryId = @ImageCategoryId  AND Active=1 order by slno ";

                            objDr = objDl.FillDataSet(CommandType.Text, strSQL, hstInput);
                        }

                        else if (sFileName == "c")
                        {
                            hstInput.Add("ImageCategoryId", TreeView1.SelectedNode.Value.Substring(1, TreeView1.SelectedNode.Value.Length - 1));
                            strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                            strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                            strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type ";
                            strSQL += " FROM EMRPatientDocuments pd WITH (NOLOCK) where pd.Id = @ImageCategoryId  AND Active=1 order by slno ";

                            objDr = objDl.FillDataSet(CommandType.Text, strSQL, hstInput);
                        }
                        else
                        {
                            objDr = new DataSet();
                        }
                    }

                }
            }

            RadListView1.DataSource = null;
            RadListView1.DataBind();

            if (objDr.Tables.Count > 0)
            {
                if (objDr.Tables[0].Rows.Count > 0)
                {
                    RadListView1.DataSource = objDr;
                    RadListView1.DataBind();
                }
            }

            foreach (RadListViewDataItem item in RadListView1.Items)
            {
                Button btnDelete = (Button)item.FindControl("btnDelete");
                UserAuthorisations ua1 = new UserAuthorisations();
                ua1.DisableEnableControl(btnDelete, false);

                if (ua1.CheckPermissions("C", Request.Url.AbsolutePath))
                {
                    ua1.DisableEnableControl(btnDelete, true);
                }
                else
                {
                    ua1.DisableEnableControl(btnDelete, false);

                }
                ua1.Dispose();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDl = null;

            if (objDr.Tables.Count > 0)
            {
                if (objDr.Tables[0].Rows.Count > 0)
                {
                    fileName = common.myStr(objDr.Tables[0].Rows[0]["ImageName"]);

                    ViewState["FileName"] = fileName;

                    if (common.myLen(fileName) > 0)
                    {
                        if (sortCriteria != "00")
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "downLoadFile();", true);
                        }
                    }
                }
            }

        }
    }

    protected void RadListView1_PageIndexChanged(object sender, RadListViewPageChangedEventArgs e)
    {
        bindImage("0");
        ltrlMessage.Text = "";
    }

    protected void checkDirectory(string RegistrationId, string HospitalLocationId)
    {
        DirectoryInfo objHospitalDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/" + HospitalLocationId));
        if (objHospitalDir.Exists == false)
        {
            objHospitalDir.Create();
        }
        DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/" + HospitalLocationId + "/" + RegistrationId));
        if (objDir.Exists == false)
        {
            objDir.Create();
        }
    }

    protected string CreateFileName(string sCategoryId, string sFileName)
    {
        string FileName = "";
        try
        {
            int extIndex = sFileName.IndexOf(".");
            string sExt = sFileName.Substring(extIndex, sFileName.Length - extIndex);
            BaseC.Patient objPat = new BaseC.Patient(sConString);
            string[] sTime = null;
            char[] chr = { ' ' };
            sTime = DateTime.Now.ToString().Split(chr);
            FileName = objPat.FormatDateDateMonthYear(sTime[0]) + sTime[1] + sTime[2];
            FileName = FileName.Replace("/", "");
            FileName = FileName.Replace(":", "");
            //Response.Write(objPat.FormatDateDateMonthYear(DateTime.Today.ToShortDateString()) + " " + DateTime.Now.Hour.ToString() + "_" + DateTime.Today.Now.ToString() + "_" + DateTime.Now.Second.ToString());
            if (txtRegNo.Text == "")
            {
                FileName = txtRegNo.Text.ToString().Trim() + "_" + FileName + "_" + sCategoryId + sExt;
            }
            else
            {
                FileName = txtRegNo.Text.ToString() + "_" + FileName + "_" + sCategoryId + sExt;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return FileName;
    }

    private void bindDocumentCategory()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            hstInput = new Hashtable();
            hstInput.Add("@intHospId", common.myInt(Session["HospitalLocationId"]));

            ddlCategory.Items.Clear();

            ddlCategory.DataSource = objDl.ExecuteReader(CommandType.Text, "SELECT id, Description FROM EMRDocumentCategory WITH (NOLOCK) where Active = 1 and (HospitalLocationId=@intHospId or HospitalLocationId is null) order by Description", hstInput);
            ddlCategory.DataTextField = "Description";
            ddlCategory.DataValueField = "id";
            ddlCategory.DataBind();

            if (ddlCategory.SelectedItem != null)
            {
                Session["ddlcate"] = ddlCategory.SelectedItem.Text;
            }

            ddlCategory.Items.Insert(0, new ListItem(string.Empty, "0"));
            ddlCategory.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {

            objDl = null;
        }
    }

    private void bindDocumentType()
    {
        try
        {
            //ddlDcomenttype.Items.Clear();
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //ddlDcomenttype.DataSource = objDl.ExecuteReader(CommandType.Text, "SELECT DocumentId, DocumentType FROM DocumentType where  (HospitalLocationId=" + Session["HospitalLocationId"] + " or HospitalLocationId is null) and Active = 1 order by DocumentType");
            //ddlDcomenttype.DataTextField = "DocumentType";
            //ddlDcomenttype.DataValueField = "DocumentId";
            //ddlDcomenttype.DataBind();
            //ddlDcomenttype.Items.Insert(0, "Select");
            //ddlDcomenttype.Items[0].Value = "0";

            //if (ddlDcomenttype.SelectedItem != null)
            //{
            //    Session["ddlDocType"] = ddlDcomenttype.SelectedItem.Text;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {

            DataSet ds = new DataSet();
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@intGroupId", Convert.ToInt32(Session["GroupID"]));
            ds = objDl.FillDataSet(CommandType.Text, "select GroupId from SecGroupMaster WITH (NOLOCK) where GroupId= @intGroupId  and Active=1", hsinput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Button btndel = new Button();
                btndel = (Button)sender;
                //Response.Write(btnup.CommandArgument.ToString());
                objDl.ExecuteNonQuery(CommandType.Text, "update EMRPatientDocuments set Active=0 where Id=" + btndel.CommandArgument.ToString());
                bindImage("0");
                bindDocumentCategory();
                bindDocumentType();
                BindTree();
                ltrlMessage.Text = "<span style='color:Red; font-weight:Bold'>Document delete successfull!</span>";
            }
            else
            {
                Alert.ShowAjaxMsg("You are Not Authorized to Delete Attachment!", this);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {

            objDl = null;
        }
    }

    protected string CreateWindowScript(object id)
    {
        string ext = Path.GetExtension(TreeView1.SelectedNode.Text);
        if (txtRegNo.Text == "")
        {
            return string.Format("var win = window.radopen('DisplayImage.aspx?id={0}&regno={1}&hosid={2}','Details');win.add_close(OnClientClose);win.center();win.maximize();win.set_visibleStatusbar(false); ", Convert.ToString(id), Session["registrationId"].ToString(), Session["HospitalLocationId"].ToString());
        }
        else
        {
            return string.Format("var win = window.radopen('DisplayImage.aspx?id={0}&regno={1}&hosid={2}','Details');win.add_close(OnClientClose);win.center();win.maximize();win.set_visibleStatusbar(false);", Convert.ToString(id), txtRegNo.Text.ToString(), Session["HospitalLocationId"].ToString());
        }
    }

    protected void lnkViewImages_Click(object sender, EventArgs e)
    {
        pnlImages.Visible = true;
    }

    private void DisplayDocument(string Id)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {

            hstInput = new Hashtable();
            hstInput.Add("ImageCategoryId", Id);
            SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select pd.Id, ImagePath, ImageName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks , dd.Data, dd.Type from EMRPatientDocuments pd WITH (NOLOCK) INNER JOIN EMRPatientDocumentData dd WITH (NOLOCK) on pd.Id = dd.DocumentId where pd.id =@ImageCategoryId AND Type in ('" + string.Join("' ,'", strdocType) + "')", hstInput);
            if (objDr.Read())
            {
                FileStream fs = new FileStream(Server.MapPath(objDr["ImagePath"].ToString()), FileMode.Create, FileAccess.ReadWrite);
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    bw.Write((byte[])objDr["Data"]);
                    fs.Close();
                    bw.Close();
                }
                catch (Exception Ex)
                {
                    Response.Write(Ex);
                }
            }
            objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {

            objDl = null;
        }
    }

    private void DisplayDoc()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            string strRegistrationId = "";

            if (common.myStr(hdnRegistrationId.Value) != "")
            {
                strRegistrationId = hdnRegistrationId.Value;
            }
            else
            {
                strRegistrationId = common.myStr(Session["RegistrationId"]);
            }

            hstInput = new Hashtable();
            hstInput.Add("@intRegistrationId", common.myInt(strRegistrationId));

            SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select pd.Id, ImagePath, ImageName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks , dd.Data, dd.Type from EMRPatientDocuments pd WITH (NOLOCK) INNER JOIN EMRPatientDocumentData dd WITH (NOLOCK) on pd.Id = dd.DocumentId where pd.RegistrationId=@intRegistrationId and Active=1", hstInput);
            if (objDr.Read())
            {
                FileStream fs = new FileStream(Server.MapPath(objDr["ImagePath"].ToString()), FileMode.Create, FileAccess.ReadWrite);
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write((byte[])objDr["Data"]);
                    fs.Close();
                    bw.Close();
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }
            }
            objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added by sikandar for code optimize
        finally
        {

            objDl = null;
        }
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        BindTree();

        ltrlMessage.Text = "";
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        if (common.myLen(txtAccountNo.Text) > 0 && Convert.ToInt64(txtAccountNo.Text) > 9223372036854775807)
        {
            Alert.ShowAjaxMsg("Value should not be more than 9223372036854775807.", this.Page);
            txtAccountNo.Text = txtAccountNo.Text.Substring(0, 12);
        }

        if (!common.myLong(txtAccountNo.Text).Equals("0") || !common.myLong(hdnRegistrationNo.Value).Equals("0"))
        {
            if (common.myLong(hdnRegistrationNo.Value) != 0)
            {
                txtAccountNo.Text = hdnRegistrationNo.Value;
                Session["RegistrationNoReg"] = common.myStr(hdnRegistrationNo.Value);
                hdnRegistrationNo.Value = "0";

            }

            else
            {
                txtRegNo.Text = txtAccountNo.Text;
            }

            BindRegistrationId();
            BindVitistNo();
        }

        //   Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);

        if (Request.QueryString["Nurse"] != null)
        {
            if (common.myStr(Request.QueryString["Nurse"]) != string.Empty)
            {
                if (common.myStr(Request.QueryString["Nurse"]) == "1")
                {
                    Response.Redirect("AttachDocumentAlkindi.aspx?Reg=1&Nurse=1&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
                }
                else
                {
                    Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
                }
            }
            else
            {
                Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
            }
        }
        else if (Request.QueryString["Reg"] != null)
        {
            if (common.myStr(Request.QueryString["Reg"]) != string.Empty)
            {
                if (common.myStr(Request.QueryString["Reg"]) == "1")
                {


                    Response.Redirect("AttachDocumentAlkindi.aspx?Reg=1&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
                }
                else
                {
                    Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
                }

            }
            else
            {
                Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
            }
        }

        else
        {
            Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
        }
    }

    protected void lbtnDownload_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton btndownload = new LinkButton();
            btndownload = (LinkButton)sender;
            string filename = common.myStr(btndownload.CommandArgument);

            downloadFile(filename, true);
        }
        catch
        {
        }
    }

    //protected void lbtnDownloadTest_Click(object sender, EventArgs e)
    //{
    //     int id = common.myInt(RadListView1.DataKeyValues );
    //    //int id = common.myInt (DataBinder.Eval(RadListView1.DataKeyValues, "id"));
    //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hstInput = new Hashtable();
    //    //string str1 = (sender as LinkButton).CommandArgument;
    //    hstInput.Add("Id", 1844);
    //    DataSet objDr = objDl.FillDataSet(CommandType.Text, "select ImageName, ImagePath from EMRPatientDocuments where id = @Id ", hstInput);

       
    //    if (objDr.Tables[0].Rows.Count > 0)
    //    {
    //        string filename = objDr.Tables[0].Rows[0]["ImagePath"].ToString();
    //        string path = Server.MapPath(filename);
    //        System.IO.FileInfo file = new System.IO.FileInfo(path);
    //        if (file.Exists)
    //        {
    //            //Response.Clear();
    //            //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
    //            //Response.AddHeader("Content-Length", file.Length.ToString());
    //            //Response.ContentType = "application/octet-stream";
    //            //Response.WriteFile(file.FullName);
    //            //Response.Close();
    //            ////Response.End();

    //            Response.Clear();
    //            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
    //            Response.AddHeader("Content-Length", file.Length.ToString());
    //            Response.ContentType = "Application/pdf";
    //            Response.WriteFile(file.FullName);
    //            Response.Close();
    //            //Response.End();

    //            // System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
    //            // response.ClearContent();
    //            // response.Clear();
    //            // response.ContentType = "application/octet-stream";
    //            // response.AddHeader("Content-Disposition",
    //            //                    "attachment; filename=" + file.Name);
    //            //// response.TransmitFile(Server.MapPath("FileDownload.csv"));
    //            // response.Flush();
    //            // response.End();
    //        }
    //    }
    //}


    protected void btnNo_OnClick(Object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true); // Call client method in radwindow page 
    }

    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        //RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=O&RegEnc=0";
        // RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0";


        if (Request.QueryString["Reg"] != null)
        {
            if (common.myStr(Request.QueryString["Reg"]) != string.Empty)
            {
                if (common.myStr(Request.QueryString["Reg"]) == "1")
                {
                    RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0&Reg=1";
                }
                else
                {
                    RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0";
                }

            }
            else
            {
                RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0";
            }
        }
        else
        {
            RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0";
        }

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void lnkAllergies_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("/EMR/Allergy/Allergy.aspx?RegId=" + common.myInt(txtRegNo.Text), false);
    }

    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnUpload, false);

        if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnUpload, true);
        }
        ua1.Dispose();
    }

    private void SetPermission(Button btnID, string mode, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, false);

        if (ua1.CheckPermissions(mode, Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnID, action);
        }
        else
        {
            ua1.DisableEnableControl(btnID, !action);
        }
        ua1.Dispose();
    }

    protected void BindVitistNo()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hshIn = new Hashtable();

        hshIn.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
        hshIn.Add("@FacilityID", common.myInt(Session["FacilityID"]));
        hshIn.Add("@RegistrationId", common.myInt(hdnRegistrationId.Value));
        ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterDetails", hshIn);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlvisit.Items.Clear();
            ddlvisit.DataSource = ds;
            ddlvisit.DataTextField = "EncounterNo";
            ddlvisit.DataValueField = "Id";
            ddlvisit.DataBind();
            ddlvisit.Items.Insert(0, "Select");
            ddlvisit.Items[0].Value = "0";
            ddlvisit.SelectedIndex = ddlvisit.Items.IndexOf(ddlvisit.Items.FindByValue(common.myStr(Session["EncounterId"])));
        }
    }

    protected void BindRegistrationId()
    {

        if (common.myInt(hdnRegistrationId.Value) == 0 && common.myStr(txtRegNo.Text) != "")
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            hstInput = new Hashtable();
            hstInput.Add("@chvRegistrationNo", common.myLong(txtRegNo.Text.Trim()));

            string strsql = "select Id from Registration WITH (NOLOCK) Where RegistrationNo=@chvRegistrationNo";
            ds = dl.FillDataSet(CommandType.Text, strsql, hstInput);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                hdnRegistrationId.Value = ds.Tables[0].Rows[0]["Id"].ToString().Trim();
            }
            else
            {
                lblMessage.Text = "Invalid UHID No";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
        }
    }

    protected void downloadFile(string filename, bool IsShowDiaglog)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            string strRegistrationId = "";
            if (filename != "")
            {
                if (common.myStr(hdnRegistrationId.Value) != "")
                {
                    strRegistrationId = hdnRegistrationId.Value;
                }
                else
                {
                    strRegistrationId = common.myStr(Session["RegistrationId"]);
                }
                Hashtable hshInput = new Hashtable();
                hshInput.Add("@intRegistrationId", strRegistrationId);

                string disString = "select Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'" +
                                    " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'" +
                                    " else ImagePath end as ImagePath, ImagePath as FilePath,Type,ImageName " +
                                    " from EMRPatientDocuments WITH (NOLOCK) " +
                                    " where RegistrationID = @intRegistrationId AND Active=1";

                DataSet ds = objDl.FillDataSet(CommandType.Text, disString, hshInput);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (filename == Convert.ToString(ds.Tables[0].Rows[i]["ImageName"].ToString()))
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["Type"].ToString()) == ".zip")
                            {
                                string zFileName = Server.MapPath("/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + txtRegNo.Text.ToString().Trim() + "/" + filename);
                                System.IO.FileInfo file = new System.IO.FileInfo(zFileName);
                                if (file.Exists)
                                {
                                    Response.Clear();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                                    Response.AddHeader("Content-Length", file.Length.ToString());
                                    Response.ContentType = "application/octet-stream";
                                    Response.WriteFile(file.FullName);
                                    Response.Close();

                                    //Response.End();
                                }
                            }
                            else
                            {
                                //string sFileName = Server.MapPath("/PatientDocuments/" + common.myStr(Session["HospitalLocationID"]) + "/" + common.myStr(txtRegNo.Text).Trim() + "/" + filename);
                                //System.IO.FileInfo file = new System.IO.FileInfo(sFileName);
                                //if (file.Exists)
                                //{
                                //    Response.Clear();
                                //    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                                //    Response.AddHeader("Content-Length", file.Length.ToString());
                                //    Response.ContentType = "application/octet-stream";
                                //    Response.WriteFile(file.FullName);

                                //    Response.Close();
                                //    //Response.End();
                                //}

                                string sFileName = Server.MapPath("/PatientDocuments/" + common.myStr(Session["HospitalLocationID"]) + "/" + common.myStr(txtRegNo.Text).Trim() + "/" + filename);

                                System.IO.FileInfo file = new System.IO.FileInfo(sFileName);
                                if (file.Exists)
                                {
                                    if (IsShowDiaglog)
                                    {
                                        Response.Clear();
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                                        Response.AddHeader("Content-Length", file.Length.ToString());
                                        Response.ContentType = "application/octet-stream";
                                        Response.WriteFile(file.FullName);
                                      Response.Close();
                                      //  Response.End();
                                    }
                                    else
                                    {
                                        if (file.Extension.ToLower().Equals(".docx")
                                            || file.Extension.ToLower().Equals(".docx")
                                            || file.Extension.ToLower().Equals(".doc")
                                            || file.Extension.ToLower().Equals(".rar")
                                            || file.Extension.ToLower().Equals(".zip")
                                            || file.Extension.ToLower().Equals(".xls")
                                            || file.Extension.ToLower().Equals(".csv"))
                                        {
                                            // Get the physical Path of the file
                                            string filepath = sFileName;

                                            // Clear the content of the response
                                            Response.ClearContent();

                                            // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                                            Response.AddHeader("Content-Disposition", "inline; filename=" + file.Name);

                                            // Add the file size into the response header
                                            Response.AddHeader("Content-Length", file.Length.ToString());

                                            // Set the ContentType
                                            Response.ContentType = ReturnExtension(file.Extension.ToLower());

                                            // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                                            Response.TransmitFile(file.FullName);

                                            // End the response
                                            //Response.End();
                                            Response.Close();
                                        }
                                        else
                                        {

                                            Session["AttachmentPathFileName"] = sFileName;

                                            RadWindowForNew.NavigateUrl = "/EMR/AttachmentOpen.aspx";
                                            RadWindowForNew.Height = 450;
                                            RadWindowForNew.Width = 550;
                                            RadWindowForNew.Top = 10;
                                            RadWindowForNew.Left = 10;
                                            RadWindowForNew.Title = "Document Open";
                                            RadWindowForNew.OnClientClose = string.Empty;
                                            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                            RadWindowForNew.Modal = true;
                                            RadWindowForNew.VisibleStatusbar = false;
                                            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDl = null;
        }
    }

    private string ReturnExtension(string fileExtension)
    {
        switch (fileExtension)
        {
            case ".htm":
            case ".html":
            case ".log":
                return "text/HTML";
            case ".txt":
                return "text/plain";
            case ".docx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".doc":
                return "application/msword";
            case ".tiff":
            case ".tif":
                return "image/tiff";
            case ".asf":
                return "video/x-ms-asf";
            case ".avi":
                return "video/avi";
            case ".rar":
                return "application/rar";
            case ".zip":
                return "application/zip";
            case ".xls":
            case ".csv":
                return "application/vnd.ms-excel";
            case ".gif":
                return "image/gif";
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".png":
                return "image/png";
            case ".bmp":
                return "image/bmp";
            case ".wav":
                return "audio/wav";
            case ".mp3":
                return "audio/mpeg3";
            case ".mpg":
            case ".mpeg":
                return "video/mpeg";
            case ".rtf":
                return "application/rtf";
            case ".asp":
                return "text/asp";
            case ".pdf":
                return "application/pdf";
            case ".fdf":
                return "application/vnd.fdf";
            case ".ppt":
                return "application/mspowerpoint";
            case ".dwg":
                return "image/vnd.dwg";
            case ".msg":
                return "application/msoutlook";
            case ".xml":
            case ".sdxl":
                return "application/xml";
            case ".xdp":
                return "application/vnd.adobe.xdp+xml";
            default:
                return "application/octet-stream";
        }

    }


    protected void btnDownLoadFile_OnClick(object sender, EventArgs e)
    {
        if (common.myLen(ViewState["FileName"]) > 0)
        {
            downloadFile(common.myStr(ViewState["FileName"]), false);
        }
    }


    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtAccountNo.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        hdnRegistrationId.Value = string.Empty;
        hdnRegistrationNo.Value = string.Empty;
        Session.Remove("RNo");
        // Response.Redirect("AttachDocumentAlkindi.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
        if (Request.QueryString["Nurse"] != null)
        {
            if (common.myStr(Request.QueryString["Nurse"]) != string.Empty)
            {
                if (common.myStr(Request.QueryString["Nurse"]) == "1")
                {


                    Response.Redirect("AttachDocumentAlkindi.aspx?Reg=1&Nurse=1&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
                }
                else
                {
                    Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
                }

            }
            else
            {
                Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
            }
        }

        else if (Request.QueryString["Reg"] != null)
        {
            if (common.myStr(Request.QueryString["Reg"]) != string.Empty)
            {
                if (common.myStr(Request.QueryString["Reg"]) == "1")
                {
                    Response.Redirect("AttachDocumentAlkindi.aspx?Reg=1&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
                }
                else
                {
                    Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
                }

            }
            else
            {
                Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
            }
        }
        else
        {
            Response.Redirect("AttachDocumentAlkindi.aspx?MASTER=No&RNo=" + txtAccountNo.Text.ToString().Trim() + "", false);
        }
        Session["RegistrationNoReg"] = string.Empty;
    }
}
