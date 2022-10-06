using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OTScheduler_OTCheckListMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.RestFulAPI objwcfot;//= new wcf_Service_OT.ServiceClient();
    DataSet ds;
    Hashtable hshIn;
    private bool RowSelStatus = false;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objwcfot = new BaseC.RestFulAPI(sConString);
            BindGrid();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objwcfot = new BaseC.RestFulAPI(sConString);
            if (common.myStr(btnSave.Text) == "Save")
            {
                string strsave = "";
                strsave = objwcfot.SaveOTChecklistMaster(0, common.myStr(txtDescription.Text), common.myStr(ddlType.SelectedValue), common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserId"]));
                if (strsave.Contains("Save Data"))
                {
                    lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMassage.Text = strsave;
                }
                else
                {
                    lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMassage.Text = strsave;
                }
            }
            else
            {
                string strupdate = "";
                strupdate = objwcfot.SaveOTChecklistMaster(common.myInt(ViewState["ID"]), common.myStr(txtDescription.Text), common.myStr(ddlType.SelectedValue), common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserId"]));
                if (strupdate.Contains("Update Data"))
                {
                    lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMassage.Text = strupdate;
                }
                else
                {
                    lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMassage.Text = strupdate;
                }
            }
            Cleardata();
            BindGrid();
            btnSave.Text = "Save";

        }
        catch (Exception Ex)
        {
            lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMassage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Cleardata();
        lblMassage.Text = "";
    }

    void Cleardata()
    {
        txtDescription.Text = "";
        ddlStatus.SelectedIndex = 0;
        ddlType.SelectedIndex = 0;
    }

    void BindGrid()
    {

        ds = new DataSet();
        objwcfot = new BaseC.RestFulAPI(sConString);
        ds = objwcfot.GetOTChecklistMaster();
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDetails.DataSource = ds;
            gvDetails.DataBind();
        }
        else
        {
            BindBlankgrid(1);
        }
    }

    void BindBlankgrid(int row)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id");
        dt.Columns.Add("Description");
        dt.Columns.Add("ValueType");
        dt.Columns.Add("Status");
        dt.Columns.Add("Active");

        DataRow dr = dt.NewRow();

        for (int i = 0; i < row; i++)
        {
            dr["Id"] = DBNull.Value;
            dr["Description"] = DBNull.Value;
            dr["ValueType"] = DBNull.Value;
            dr["Status"] = DBNull.Value;
            dr["Active"] = DBNull.Value;
        }
        dt.Rows.Add(dr);

        gvDetails.DataSource = dt;
        gvDetails.DataBind();
    }
    protected void gvDetails_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            RowSelStatus = true;
            int selectedIdx = e.Item.ItemIndex;
            if (e.CommandName == "Select")
            {
                gvDetails.Items[selectedIdx].Selected = true;
                ViewState["ID"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtId")).Text;
                txtDescription.Text = ((Label)gvDetails.SelectedItems[0].FindControl("txtDescription")).Text;
                ddlType.SelectedValue = ((Label)gvDetails.SelectedItems[0].FindControl("txtValueType")).Text;
                
                if (((Label)gvDetails.SelectedItems[0].FindControl("lblActive")).Text == "False")
                {
                    ddlStatus.SelectedValue = "0";
                }
                else
                {
                    ddlStatus.SelectedValue = "1";
                }
                btnSave.Text = "Update";
            }
        }
        catch (Exception Ex)
        {
            lblMassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMassage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvDetails_PreRender(object sender, EventArgs e)
    {
       if(RowSelStatus == true)
        BindGrid();
    }
}
