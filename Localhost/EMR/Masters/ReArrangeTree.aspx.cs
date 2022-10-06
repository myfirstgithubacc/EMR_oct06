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

public partial class EMR_Masters_ReArrangeTree : System.Web.UI.Page
{
    Hashtable hshIn;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    DL_Funs fun = new DL_Funs();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["tmp"] != null)
            {
                if (Request.QueryString["tmp"] == "sec")
                {
                    lblTemplate.Text = "Sections";
                    if (Request.QueryString["tempid"] != null)
                    {
                        ViewState["TemplateID"] = Request.QueryString["tempid"];  //Session["TmpId"];
                        BindCategoryTree();
                    }

                }
                else if(Request.QueryString["tmp"] == "vid")
                {
                    lblTemplate.Text = "Value";
                    if (Request.QueryString["flid"] != null)
                    {
                        ViewState["FieldId"] = Request.QueryString["flid"];  //Session["TmpId"];
                        ViewState["SectionId"]= Request.QueryString["secid"];
                        BindFieldTree();
                    }
                }
                else
                {
                    lblTemplate.Text = "Fields";
                    if (Request.QueryString["secid"] != null)
                    {
                        ViewState["SectionId"] = Request.QueryString["secid"]; // Session["SectionId"];
                        BindFieldTree();
                    }
                }
            }
        }
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void tvTree_OnSelectedNodeChanged(Object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["tmp"] == "sec")
            {
                hshIn = new Hashtable();

                ////Select SectionId,SectionName from EMRTemplateSections  Where TemplateId = 48 
                ////and 
                ////isnull(ParentId,0)=( Select isnull(ParentId,0) from 
                ////EMRTemplateSections where SectionId = 125) Order By ParentId,Hierarchy,SequenceNo

                String strQuery = "Select SectionId,SectionName from EMRTemplateSections "
                            + " Where TemplateId = @TemplateId and isnull(ParentId,0)=( "
                            + "Select isnull(ParentId,0) from EMRTemplateSections where SectionId = @SectionId) Order By ParentId,Hierarchy,SequenceNo";
                hshIn.Add("@TemplateId", ViewState["TemplateID"]);
                hshIn.Add("@SectionId", tvTree.SelectedNode.Value.Substring(1, tvTree.SelectedNode.Value.Length - 1));
                fun.BindDDLorLstBox_SqlStmtNew(lstTemplate, strQuery, hshIn);

                lstTemplate.SelectedIndex = 0;

            }
            else if (Request.QueryString["tmp"] == "vid")
            {
                hshIn = new Hashtable();
                String strQuery = "Select ValueId,ValueName from EMRTemplateValues "
                    + " Where FieldId = @FieldId Order By SequenceNo";
                hshIn.Add("@valueId", tvTree.SelectedNode.Value.Substring(1, tvTree.SelectedNode.Value.Length - 1));
                hshIn.Add("@FieldId", ViewState["FieldId"]);
                fun.BindDDLorLstBox_SqlStmtNew(lstTemplate, strQuery, hshIn);
                if (lstTemplate.Items.Count > 0)
                {
                    lstTemplate.SelectedIndex = 0;
                }
            }
            else
            {
                hshIn = new Hashtable();
                String strQuery = "Select FieldId,FieldName from EMRTemplateFields "
                    + " Where SectionId = @SectionId and isnull(ParentId,0)=( "
                    + "Select isnull(ParentId,0) from EMRTemplateFields where FieldId = @FieldId) Order By ParentId,Hierarchy,SequenceNo";
                hshIn.Add("@SectionId", ViewState["SectionId"]);
                hshIn.Add("@FieldId", tvTree.SelectedNode.Value.Substring(1, tvTree.SelectedNode.Value.Length - 1));
                fun.BindDDLorLstBox_SqlStmtNew(lstTemplate, strQuery, hshIn);
                if (lstTemplate.Items.Count > 0)
                {
                    lstTemplate.SelectedIndex = 0;
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

    protected void btnUp_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (lstTemplate.SelectedIndex != -1)
            {
                if (lstTemplate.SelectedIndex > 0)
                {
                    Move("Move Up");
                    int temp = 0;
                    string tempValue = "0";
                    string tempText = "";
                    for (int i = 0; i < lstTemplate.Items.Count; i++)
                    {
                        if (lstTemplate.Items[i].Selected)
                        {
                            tempValue = lstTemplate.Items[i].Value;
                            tempText = lstTemplate.Items[i].Text;
                            temp = i;
                            lstTemplate.Items.Remove(lstTemplate.Items[i]);
                            i--;
                        }
                    }
                    ListItem templist = new ListItem(tempText, tempValue);
                    lstTemplate.Items.Insert(temp - 1, templist);
                    lstTemplate.Items[temp - 1].Selected = true;
                }
            }
            else
                Alert.ShowAjaxMsg("Please select atleast one Template Name to move.", Page);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnDown_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (lstTemplate.SelectedIndex != -1)
            {
                if (lstTemplate.SelectedIndex < lstTemplate.Items.Count - 1)
                {
                    Move("Move Down");
                    int temp = 0;
                    string tempValue = "0";
                    string tempText = "";
                    for (int i = 0; i < lstTemplate.Items.Count; i++)
                    {
                        if (lstTemplate.Items[i].Selected)
                        {
                            tempValue = lstTemplate.Items[i].Value;
                            tempText = lstTemplate.Items[i].Text;
                            temp = i;
                            lstTemplate.Items.Remove(lstTemplate.Items[i]);
                            i--;
                        }
                    }
                    ListItem templist = new ListItem(tempText, tempValue);

                    lstTemplate.Items.Insert(temp + 1, templist);
                    lstTemplate.Items[temp + 1].Selected = true;
                }
            }
            else
                Alert.ShowAjaxMsg("Please select atleast one Template Name to move.", Page);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void Move(string s)
    {
        try
        {
            //Session["ArrangeT"] = "ArrangeTree";
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script1", "$get('hdn1').value='ArrangeTree';", true);
            hdn1.Value = "ArrangeTree";
            hshIn = new Hashtable();
            int index = lstTemplate.SelectedIndex;
            hshIn.Add("@CurrSectionId", lstTemplate.SelectedValue);
            if (s == "Move Down")
                hshIn.Add("@PrevSectionId", lstTemplate.Items[index + 1].Value); //Move Down
            else
                hshIn.Add("@PrevSectionId", lstTemplate.Items[index - 1].Value); //Move Up
            string sqlQuery = "";
            if (Request.QueryString["tmp"] == "sec")
            {
                sqlQuery = "Declare @TempSequenceNo int "
                    + "Select @TempSequenceNo=SequenceNo from EMRTemplateSections where SectionId = @CurrSectionId "
                    + "Update EMRTemplateSections Set SequenceNo= "
                    + "(Select SequenceNo from EMRTemplateSections where SectionId = @PrevSectionId) where SectionId = @CurrSectionId "
                    + "Update EMRTemplateSections Set SequenceNo=@TempSequenceNo where SectionId = @PrevSectionId";
            }
            else if (Request.QueryString["tmp"] == "vid")
            {
                sqlQuery = "Declare @TempSequenceNo int "
             + "Select @TempSequenceNo=SequenceNo from EMRTemplateValues where valueId = @CurrSectionId "
             + "Update EMRTemplateValues Set SequenceNo= "
             + "(Select SequenceNo from EMRTemplateValues where valueId = @PrevSectionId) where valueId = @CurrSectionId "
             + "Update EMRTemplateValues Set SequenceNo=@TempSequenceNo where valueId = @PrevSectionId";
            }
            else
            {
                sqlQuery = "Declare @TempSequenceNo int "
              + "Select @TempSequenceNo=SequenceNo from EMRTemplateFields where FieldId = @CurrSectionId "
              + "Update EMRTemplateFields Set SequenceNo= "
              + "(Select SequenceNo from EMRTemplateFields where FieldId = @PrevSectionId) where FieldId = @CurrSectionId "
              + "Update EMRTemplateFields Set SequenceNo=@TempSequenceNo where FieldId = @PrevSectionId";
            }
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int itemp = dl.ExecuteNonQuery(CommandType.Text, sqlQuery, hshIn);
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindCategoryTree()
    {
        try
        {
            tvTree.Nodes.Clear();
            hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
            hshIn.Add("@intTemplateId", ViewState["TemplateID"]);
            //hshIn.Add("@inyTemplateTypeID", ViewState["TemplateTypeID"].ToString()); //  Comment By Chandan According To Store Procedure.
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTree", hshIn);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                AddNodes(tvTree, Convert.ToInt32(ds.Tables[0].Rows[i]["SectionId"]), Convert.ToInt32(ds.Tables[0].Rows[i]["ParentId"]), Convert.ToString(ds.Tables[0].Rows[i]["SectionName"]), "aaa");
            }
            if (tvTree.Nodes.Count > 0)
            {
                tvTree.Nodes[0].Selected = true;
                tvTree.CollapseAll();
                tvTree.PopulateNodesFromClient = true;
                tvTree.ShowLines = true;
                tvTree_OnSelectedNodeChanged(this, null);
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindFieldTree()
    {
        try
        {
            //pnlNewEditCategorySubCategory.Visible = false;
            tvTree.Nodes.Clear();
            hshIn = new Hashtable();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
            hshIn.Add("@intCategoryID", common.myInt(ViewState["SectionId"]));
            hshIn.Add("@intFieldId", common.myInt(ViewState["FieldId"]));
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateFieldNames", hshIn);
            if (common.myStr(Request.QueryString["tmp"]) != "vid")
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AddNodes(tvTree, Convert.ToInt32(ds.Tables[0].Rows[i]["FieldId"]), Convert.ToInt32(ds.Tables[0].Rows[i]["ParentId"]), Convert.ToString(ds.Tables[0].Rows[i]["FieldName"]), ds.Tables[0].Rows[i]["FieldType"].ToString());
                }
            }
            else
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    AddNodes(tvTree, Convert.ToInt32(ds.Tables[1].Rows[i]["valueId"]), Convert.ToInt32(ds.Tables[1].Rows[i]["ParentId"]), Convert.ToString(ds.Tables[1].Rows[i]["ValueName"]), ds.Tables[1].Rows[i]["FieldType"].ToString());
                }
            }
            if (tvTree.Nodes.Count > 0)
            {
                tvTree.Nodes[0].Selected = true;
                tvTree.CollapseAll();
                tvTree.PopulateNodesFromClient = true;
                tvTree.ShowLines = true;
                tvTree_OnSelectedNodeChanged(this, null);
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void AddNodes(TreeView tvName, Int32 iNodeID, Int32 iParentID, String sNodeText, String DataType)
    {
        if (iParentID == 0)
        {
            TreeNode masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString(), "", "", DataType);
            tvName.Nodes.Add(masternode);
        }
        else
        {
            TreeNode masternode = new TreeNode();
            masternode = tvName.FindNode("P" + iParentID.ToString());
            if (masternode != null)
            {
                TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString(), "", "", DataType);
                masternode.ChildNodes.Add(childNode);
            }
            else
            {
                CallRecursive(tvName, iNodeID, "C" + iParentID, sNodeText);
            }
        }
    }

    private void CallRecursive(TreeView tvName, Int32 iNodeID, String sParentID, String sNodeText)
    {
        TreeNodeCollection nodes = tvName.Nodes;
        foreach (TreeNode n in nodes)
        {
            ReCallRecursive(n, iNodeID, sParentID, sNodeText);
        }
    }

    private void ReCallRecursive(TreeNode treeNode, Int32 iNodeID, String sParentID, String sNodeText)
    {
        foreach (TreeNode tn in treeNode.ChildNodes)
        {
            if (tn.Value == sParentID.ToString())
            {
                TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString());
                tn.ChildNodes.Add(childNode);
            }
            ReCallRecursive(tn, iNodeID, sParentID, sNodeText);
        }
    }

}
