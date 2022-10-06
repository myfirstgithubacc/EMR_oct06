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
using BaseC;

public partial class MRD_MrdSurgeryCptCodeMaster : System.Web.UI.Page
{
    //Global variables
    DAL.DAL dl = new DAL.DAL();
    StringBuilder strSQL;
    DataSet ds = new DataSet();//dataset objetc
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;//connectionstring
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();

    //page load event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            fillCPTCodeMasterList();
        }
    }


    public void fillCPTCodeMasterList()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetSurgeryCptCodeMaster", hshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["selectDisplay"] = "true";
                grvCPTCode.DataSource = ds;
                grvCPTCode.DataBind();
                ViewState["CPTCodeMaster"] = ds;
            }
            else
            {
                BindBlankGrid();
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //button click event to save or update Data
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ////Converting viewstate to dataset
            if (common.myStr(txtCptCode.Text.Trim()) == "" || common.myStr(txtDescription.Text.Trim()) == "")
            {
                lblMsg.Text = "Please Enter The CPTCode and Description";
                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Adding parameters to hastable
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();

            hshIn.Add("@CPTCode", common.myStr(txtCptCode.Text.Trim()));
            hshIn.Add("@Description", bc.ParseQ1(txtDescription.Text));
            hshIn.Add("@status", ddlstatus.SelectedValue);
            hshIn.Add("@EncodedBy", Session["UserID"]);
            hshIn.Add("@Id", common.myInt(ViewState["Id"]));

            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspInsertUpdateSurgeryCptCodeMaster", hshIn, hshOut);
            if (hshOut["@chvErrorStatus"].ToString().ToUpper().Contains("SAVE"))
            {
                if (btnSave.Text == "Save")
                    lblMsg.Text = "Record Save Successfully";
                else
                    lblMsg.Text = "Record Updated Successfully";
            }
            else
            {
                lblMsg.Text = hshOut["@chvErrorStatus"].ToString();
                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }


            lblMsg.ForeColor = System.Drawing.Color.Green;

            fillCPTCodeMasterList();
            btnClear_Click(null, null);

        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //gridview row Datbound Event
    protected void grvCPTCode_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      //  e.Row.Cells[0].Visible = false;
    }
    protected void grvCPTCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvCPTCode.PageIndex = e.NewPageIndex;
        fillCPTCodeMasterList();
    }

    //gridview select link button click
    protected void lbSelect_Click(object sender, EventArgs e)
    {
        try
        {
            //finding link button control
            LinkButton lbSelect = (LinkButton)sender;
            //finding selected gridview row 
            GridViewRow grv = (GridViewRow)lbSelect.NamingContainer;

            //putting from date in textbox
            txtCptCode.Text = Convert.ToString(grv.Cells[1].Text);//Description
            txtDescription.Text = Convert.ToString(grv.Cells[2].Text);//Description
            ViewState["Id"] = Convert.ToString(grv.Cells[0].Text);//Id
            //checking Condition for status
            if (Convert.ToString(grv.Cells[3].Text) == "IN-ACTIVE")
            {
                ddlstatus.SelectedValue = "0";//status
            }
            else
            {
                ddlstatus.SelectedValue = "1";//status
            }

            //Changing the button save text
            btnSave.Text = "Update";

            //clearing the label text
            lblMsg.Text = "";
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //button clear click event
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //setting control to Default Value

        txtDescription.Text = "";
        txtCptCode.Text = "";
        ddlstatus.SelectedIndex = -1;
        ViewState["id"] = "";
        btnSave.Text = "Save";
    }

    //fill blank data Gridview
    private void BindBlankGrid()
    {
        try
        {
            DataTable dtRoomNo = new DataTable();
            //Data column
            dtRoomNo.Columns.Add("Id");
            dtRoomNo.Columns.Add("CPTCode");
            dtRoomNo.Columns.Add("Description");
            dtRoomNo.Columns.Add("Active");
            for (int i = 0; i < 5; i++)
            {
                DataRow Dr = dtRoomNo.NewRow();
                Dr["Id"] = "";
                Dr["CPTCode"] = "";
                Dr["Description"] = "";
                Dr["Active"] = "";
                dtRoomNo.Rows.Add(Dr);
            }
            ViewState["selectDisplay"] = "false";
            grvCPTCode.DataSource = dtRoomNo;
            grvCPTCode.DataBind();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
