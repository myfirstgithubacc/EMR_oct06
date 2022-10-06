using BaseC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class WardManagement_EMROrderExceptionsMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            BindEmployee();
            BindDepartment();
            bindGetGroupName();
            BindGrid();



        }

    }

    private void Clear()
    {
        //ddlCategory.Items.Insert(0, new RadComboBoxItem("", "0"));
        //ddlsubCategory.Items.Insert(0, new RadComboBoxItem("", "0"));
        //ddlSubDeparment.Items.Insert(0, new RadComboBoxItem("", "0"));
        //ddlDepartment.Items.Insert(0, new RadComboBoxItem("", "0"));
        //ddlEmployee.Items.Insert(0, new RadComboBoxItem("", "0"));
        //ddlGroups.Items.Insert(0, new RadComboBoxItem("", "0"));
        ddlDepartment.ClearSelection();
        ddlEmployee.ClearSelection();
        ddlGroups.ClearSelection();
        ddlSubDeparment.ClearSelection();
        ddlsubCategory.ClearSelection();
        ddlCategory.ClearSelection();

    }

    private void BindCategory()
    {
        clsPharmacy objclsPharmacy = new clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objclsPharmacy.getItemCategoryMaster(0, common.myInt(Session["HospitalLocationID"]), 1, 0);
            ddlCategory.DataTextField = "ItemCategoryName";
            ddlCategory.DataValueField = "ItemCategoryId";
           
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
            ddlCategory.Text = "";
            ddlCategory.Items.Insert(0, new RadComboBoxItem("", "0"));

        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    private void BindGrid()

    {
        DataSet ds = new DataSet();
        BindNotes objbindnote = new BindNotes(sConString);

        try
        {
            ds = objbindnote.GetEMRServiceOrderExceptions(common.myInt(Session["FacilityId"]), common.myInt(ddlType.SelectedValue));
            if (ddlType.SelectedValue.Equals("0"))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvOrderApproval.DataSource = ds.Tables[0];
                    gvOrderApproval.DataBind();

                }
                else
                {
                    BindBlankGridOrderApproval();
                }
            }
            else
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvOrderFormularyby.DataSource = ds.Tables[0];
                    gvOrderFormularyby.DataBind();
                }
                else
                {
                    BindBlankGridFormulary();
                }
            }


        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }

    }

    void BindBlankGridOrderApproval()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("GroupName");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("SubName");
            dt.Columns.Add("id");
            dt.Columns.Add("ItemSubCategoryName");

            DataRow Dr = dt.NewRow();
            Dr["GroupName"] = "";
            Dr["EmployeeName"] = "";
            Dr["SubName"] = "";
            Dr["id"] = "";
            Dr["ItemSubCategoryName"] = "";
            dt.Rows.Add(Dr);
            gvOrderApproval.DataSource = dt;
            gvOrderApproval.DataBind();
            gvOrderFormularyby.DataSource = dt;
            gvOrderFormularyby.DataBind();
        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
    }
    void BindBlankGridFormulary()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("GroupName");
            dt.Columns.Add("EmployeeName");
            dt.Columns.Add("SubName");
            dt.Columns.Add("id");
            dt.Columns.Add("ItemSubCategoryName");
            DataRow Dr = dt.NewRow();
            Dr["GroupName"] = "";
            Dr["EmployeeName"] = "";
            Dr["SubName"] = "";
            Dr["id"] = "";
            Dr["ItemSubCategoryName"] = "";
            dt.Rows.Add(Dr);
            gvOrderFormularyby.DataSource = dt;
            gvOrderFormularyby.DataBind();
        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {


            BindNotes objBindNote = new BindNotes(sConString);
            String Message = "";
            if (ddlType.SelectedValue.Equals("0"))
            {
                Message = objBindNote.SaveEMRServiceOrderExceptions(common.myInt(Session["FacilityId"]), common.myInt(ddlGroups.SelectedValue),
                      common.myInt(ddlEmployee.SelectedValue), common.myInt(ddlSubDeparment.SelectedValue), common.myInt(Session["UserId"]), 0);
            }
            else if (ddlType.SelectedValue.Equals("1"))
            {
                Message = objBindNote.SaveEMRServiceOrderExceptions(common.myInt(Session["FacilityId"]), common.myInt(ddlGroups.SelectedValue),
                  common.myInt(ddlEmployee.SelectedValue), common.myInt(ddlsubCategory.SelectedValue), common.myInt(Session["UserId"]), 1);

            }

            if (Message.ToUpper().Contains("RECORD SAVE"))
            {
                lblMsg.Text = Message;
                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                BindGrid();
                Clear();
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemSubCategoryName");
                dt.Columns.Add("ItemSubCategoryId");
                ddlsubCategory.DataTextField = "ItemSubCategoryName";
                ddlsubCategory.DataValueField = "ItemSubCategoryId";
                ddlsubCategory.DataSource = dt;
                ddlsubCategory.DataBind();
            }
            else
            {
                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMsg.Text = Message;

            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    private void BindEmployee()
    {
        clsLISMaster objclsmaster = new clsLISMaster(sConString);
        DataSet ds = new DataSet();
        ds = objclsmaster.getEmployeeData(common.myInt(Session["HospitalLocationID"]), 0, 0, "", "", 0, 0, "", common.myInt(Session["FacilityId"]));
        ddlEmployee.DataTextField = "EmployeeName";
        ddlEmployee.DataValueField = "EmployeeId";
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlEmployee.DataSource = ds.Tables[0];
            ddlEmployee.DataBind();
            ddlEmployee.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
    }


    private void BindDepartment()
    {

        DataSet ds = new DataSet();
        DAL.DAL dl;
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();

            hshIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]));
            hshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hshIn.Add("@intDepartmentId", 0);
            hshIn.Add("@iEntrySiteId", 0);
            hshIn.Add("@iActive", 1);
            hshIn.Add("@chvDepartmentType", string.Empty);

            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalDepartments", hshIn);


            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentID";

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDepartment.DataSource = ds.Tables[0];
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new RadComboBoxItem("", "0"));

            }

        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataSet ds = new DataSet();
        BindNotes objbindnote = new BindNotes(sConString);
        try
        {
            int DepartmentId = common.myInt(ddlDepartment.SelectedValue);
            ddlSubDeparment.DataTextField = "SubName";
            ddlSubDeparment.DataValueField = "SubDeptId";
            ds = objbindnote.GetSubDepartment(DepartmentId, common.myInt(Session["HospitalLocationId"]));
            ddlSubDeparment.DataSource = ds.Tables[0];
            ddlSubDeparment.DataBind();
            ddlSubDeparment.Text = "";
            ddlSubDeparment.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }


    }

    private void bindGetGroupName()
    {
        DataSet ds = new DataSet();
        Security objSecurity = new Security(sConString);
        try
        {
            ds = objSecurity.GetGroupName(Convert.ToInt16(Session["HospitalLocationId"]));
            ddlGroups.DataTextField = "GroupName";
            ddlGroups.DataValueField = "GroupID";
            ddlGroups.DataSource = ds.Tables[0];
            ddlGroups.DataBind();
            ddlGroups.Items.Insert(0, new RadComboBoxItem("", "0"));
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }


    }

    protected void gvOrderApproval_PreRender(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void gvOrderApproval_ItemCommand(object sender, GridCommandEventArgs e)
    {
        BindNotes objBindNote = new BindNotes(sConString);
        DataSet ds = new DataSet();
        if (e.CommandName.ToUpper().Equals("ITEMDELETE"))
        {

            int id = common.myInt(e.CommandArgument);
            String Message = objBindNote.DeleteEMRServiceOrderExceptions(id, common.myInt(Session["Userid"]), 0);
            if (Message.ToUpper().Contains("RECORD DELETE"))
            {
                lblMsg.Text = Message;
                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                BindGrid();

            }
            else
            {
                if (!Message.ToUpper().Contains("SELECT GROUPS OR EMPLOYEE NAME"))
                {
                    lblMsg.Text = Message;
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                }
                else
                {

                }

            }
        }


    }



    protected void ddlType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Clear();
        DataTable dt = new DataTable();
        if (ddlType.SelectedValue.Equals("0"))
        {
            dt.Columns.Add("SubName");
            dt.Columns.Add("SubDeptId");
            ddlSubDeparment.DataTextField = "SubName";
            ddlSubDeparment.DataValueField = "SubDeptId";
            ddlSubDeparment.DataSource = dt;
            ddlSubDeparment.DataBind();
            BindGrid();
            gvOrderApproval.Visible = true;
            ddlDepartment.Visible = true;
            ddlSubDeparment.Visible = true;
            Literal3.Visible = true;
            Literal4.Visible = true;
            dprtmntlbl.Visible = true;
            dprtmnt.Visible = true;
            dprtsubdeparmentlbl.Visible = true;
            dprtsubdeparment.Visible = true;
            lblcategory.Visible = false;
            divCategory.Visible = false;
            lblSubCategory.Visible = false;
            duvsubcategory.Visible = false;
            gvOrderFormularyby.Visible = false;

        }
        else
        {
           
            dt.Columns.Add("ItemSubCategoryName");
            dt.Columns.Add("ItemSubCategoryId");
            ddlsubCategory.DataTextField = "ItemSubCategoryName";
            ddlsubCategory.DataValueField = "ItemSubCategoryId";
            ddlsubCategory.DataSource = dt;
            ddlsubCategory.DataBind();
            BindGrid();
            BindCategory();
            gvOrderApproval.Visible = false;
            ddlDepartment.Visible = false;
            ddlSubDeparment.Visible = false;
            Literal3.Visible = false;
            Literal4.Visible = false;
            dprtmntlbl.Visible = false;
            dprtmnt.Visible = false;
            dprtsubdeparmentlbl.Visible = false;
            dprtsubdeparment.Visible = false;
            lblcategory.Visible = true;
            divCategory.Visible = true;
            lblSubCategory.Visible = true;
            duvsubcategory.Visible = true;
            gvOrderFormularyby.Visible = true;
        }
    }

    protected void gvOrderFormularyby_PreRender(object sender, EventArgs e)
    {

    }

    protected void gvOrderFormularyby_ItemCommand(object sender, GridCommandEventArgs e)
    {

        BindNotes objBindNote = new BindNotes(sConString);
        DataSet ds = new DataSet();
        if (e.CommandName.ToUpper().Equals("ITEMDELETE"))
        {

            int id = common.myInt(e.CommandArgument);
            String Message = objBindNote.DeleteEMRServiceOrderExceptions(id, common.myInt(Session["Userid"]), 1);
            if (Message.ToUpper().Contains("RECORD DELETE"))
            {
                lblMsg.Text = Message;
                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                BindGrid();

            }
            else
            {
                if (!Message.ToUpper().Contains("SELECT GROUPS OR EMPLOYEE NAME"))
                {
                    lblMsg.Text = Message;
                    lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                }
                else
                {

                }

            }
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        clsPharmacy objclsPharmacy = new clsPharmacy(sConString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        try
        {

            if (common.myInt(ddlCategory.SelectedValue) > 0)
            {

                ds = objclsPharmacy.getItemCategoryDetails(0, "", common.myInt(ddlCategory.SelectedValue), common.myInt(Session["HospitalLocationID"]), 1, 0);
                ddlsubCategory.DataTextField = "ItemSubCategoryName";
                ddlsubCategory.DataValueField = "ItemSubCategoryId";
                ddlsubCategory.DataSource = ds.Tables[0];
                ddlsubCategory.DataBind();
                ddlsubCategory.Text = "";
                ddlsubCategory.Items.Insert(0, new RadComboBoxItem("", "0"));

            }
            else
            {

                dt.Columns.Add("ItemSubCategoryName");
                dt.Columns.Add("ItemSubCategoryId");
                ddlsubCategory.DataTextField = "ItemSubCategoryName";
                ddlsubCategory.DataValueField = "ItemSubCategoryId";
                ddlsubCategory.DataSource = dt;
                ddlsubCategory.DataBind();
                ddlsubCategory.Text = "";
                ddlsubCategory.Items.Insert(0, new RadComboBoxItem("", "0"));
            }



        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
}