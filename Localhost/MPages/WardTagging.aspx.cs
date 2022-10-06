using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Text;

public partial class MPages_WardTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl;
    BaseC.WardManagement objwm;
    DataSet ds;
    SqlDataReader dr;
    bool RowSelStauts = false;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblTag.Text = "Ward";
            GetEmplyeeType();
            ddlemployeetype_SelectedIndexChanged(this, null);

            Bidwardlist();
            bindTaggedFlag();

        }
    }

    protected void gvwardlist_PreRender(object sender, EventArgs e)
    {
        if (RowSelStauts)
        {
            Bidwardlist();
        }
    }

    void GetEmplyeeType()
    {
        try
        {
            objwm = new BaseC.WardManagement();
            ds = new DataSet();
            ds = objwm.GetEmployeeType();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlemployeetype.DataSource = ds;
                ddlemployeetype.DataTextField = "Description";
                ddlemployeetype.DataValueField = "ID";
                ddlemployeetype.DataBind();
            }
            ddlemployeetype.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    void GetEmmployee(int EmpTypeId)
    {
        try
        {
            ddlEmployee.Items.Clear();
            objwm = new BaseC.WardManagement();
            ds = new DataSet();
            ds = objwm.GetEmployee(common.myInt(Session["HospitallocationId"]), common.myInt(EmpTypeId), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlEmployee.DataSource = ds;
                ddlEmployee.DataTextField = "EmployeeName";
                ddlEmployee.DataValueField = "ID";
                ddlEmployee.DataBind();

                gvEmployee.DataSource = ds;
                gvEmployee.DataBind();

            }
            else
            {


                gvEmployee.DataSource = ds;
                gvEmployee.DataBind();
            }
            ddlEmployee.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            //ddlEmployee.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void Bidwardlist()
    {
        try
        {
            objwm = new BaseC.WardManagement();
            ds = objwm.GetWardTagging(common.myInt(Session["HospitalLocationId"]), false, common.myInt(ddlEmployee.SelectedValue), common.myInt(Session["FacilityId"]));
            gvwardlist.DataSource = ds;
            gvwardlist.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindTaggedFlag()
    {
        try
        {
            if (common.myStr(ddlTagging.SelectedItem) == "Ward")
            {
                objwm = new BaseC.WardManagement();
                ds = new DataSet();
                ds = objwm.GetWardTagging(common.myInt(Session["HospitalLocationId"]), true, common.myInt(ddlEmployee.SelectedValue), common.myInt(Session["FacilityId"]));
                gvTagged.DataSource = ds.Tables[0];
                gvTagged.DataBind();
            }
            else
            {

                objwm = new BaseC.WardManagement();
                ds = new DataSet();
                ds = objwm.GetOtTagging(true, common.myInt(ddlEmployee.SelectedValue), common.myInt(Session["FacilityId"]));
                gvTagged.DataSource = ds.Tables[0];
                gvTagged.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlemployeetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEmmployee(common.myInt(ddlemployeetype.SelectedValue));
        bindTaggedFlag();
    }

    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Bidwardlist();
        bindTaggedFlag();

    }

    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ddlemployeetype.SelectedIndex = 0;
        ddlemployeetype_SelectedIndexChanged(this, null);
        ddlEmployee.SelectedIndex = 0;
        Bidwardlist();
        bindTaggedFlag();
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";


        //if (common.myInt(ddlEmployee.SelectedIndex) == 0)
        //{
        //    strmsg += "Please select Employee !";
        //    isSave = false;
        //}
        //if (ddlTagging.SelectedIndex == 1 || ddlTagging.SelectedIndex == 0)
        //{
        //    if (hdnEmpId.Value =="" || hdnOTID.Value == "")
        //    {
        //        strmsg += "Please Select Employee  and OT !";
        //        isSave = false;
        //    }

        //    if (hdnEmpId.Value == "" || hdnWardID.Value == "")
        //    {
        //        strmsg += "please Select Ward and Employee !";
        //        isSave = false;
        //    }
        //}
        //else
        //{
        //    if (hdnEmpId.Value == "" || hdnWardID.Value == "")
        //    {
        //        strmsg += "please Select Ward and Employee !";
        //        isSave = false;
        //    }
        //}


        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        objwm = new BaseC.WardManagement();
        StringBuilder strXML = new StringBuilder();
        StringBuilder strEmpXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        try
        {
            if (common.myStr(ddlTagging.SelectedItem) == "OT")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //if (!isSaved())
                //{
                //    return;
                //}

                foreach (GridDataItem dataItem in gvOTList.Items)
                {
                    CheckBox chkOtId = (CheckBox)dataItem.FindControl("chkTheatreid");
                    if (chkOtId.Checked == true)
                    {
                        Label lblTheatreId = (Label)dataItem.FindControl("lblTheatreId");
                        hdnOTID.Value = lblTheatreId.Text;
                        if (common.myInt(lblTheatreId.Text) > 0)
                        {
                            coll.Add(common.myInt(lblTheatreId.Text));
                            strXML.Append(common.setXmlTable(ref coll));
                        }



                    }
                }
                foreach (GridDataItem dataItem in gvEmployee.Items)
                {
                    CheckBox chkEmpid = (CheckBox)dataItem.FindControl("chkEmpid");

                    if (chkEmpid.Checked == true)
                    {
                        Label lblEmpId = (Label)dataItem.FindControl("lblEmpId");

                        hdnEmpId.Value = lblEmpId.Text;
                        if (common.myInt(lblEmpId.Text) > 0)
                        {
                            coll.Add(common.myInt(lblEmpId.Text));
                            strEmpXML.Append(common.setXmlTable(ref coll));
                        }

                    }

                }

                if (strXML.ToString() == "")
                {
                    Alert.ShowAjaxMsg("Please Select Employee  and OT !", Page);
                    return;
                }
                if (strEmpXML.ToString() == "")
                {
                    Alert.ShowAjaxMsg("Please Select Employee  and OT !", Page);
                    return;
                }

                string strMsg = objwm.SaveOTTagging(strEmpXML.ToString(), strXML.ToString(), common.myInt(Session["FacilityId"]), common.myInt(Session["UserID"]));

                //if (!isSaved())
                //{
                //    return;
                //}

                if (strMsg.Contains("Record Saved..."))
                {

                    // Bidwardlist();
                    //bindTaggedFlag();
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }

                lblMessage.Text = strMsg;

            }
            else
            {

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                //if (!isSaved())
                //{
                //    return;
                //}
                foreach (GridDataItem dataItem in gvwardlist.Items)
                {
                    CheckBox chkwardid = (CheckBox)dataItem.FindControl("chkwardid");


                    if (chkwardid.Checked == true)
                    {
                        Label lblWardId = (Label)dataItem.FindControl("lblWardId");
                        hdnWardID.Value = lblWardId.Text;
                        if (common.myInt(lblWardId.Text) > 0)
                        {
                            coll.Add(common.myInt(lblWardId.Text));
                            strXML.Append(common.setXmlTable(ref coll));
                        }
                    }
                }


                foreach (GridDataItem dataItem in gvEmployee.Items)
                {
                    CheckBox chkEmpid = (CheckBox)dataItem.FindControl("chkEmpid");

                    if (chkEmpid.Checked == true)
                    {
                        Label lblEmpId = (Label)dataItem.FindControl("lblEmpId");
                        hdnEmpId.Value = lblEmpId.Text;
                        if (common.myInt(lblEmpId.Text) > 0)
                        {
                            coll.Add(common.myInt(lblEmpId.Text));
                            strEmpXML.Append(common.setXmlTable(ref coll));
                        }
                    }
                }

                if (strXML.ToString() == "")
                {
                    Alert.ShowAjaxMsg("Please Select Employee and ward !", Page);
                    return;
                }
                if (strEmpXML.ToString() == "")
                {
                    Alert.ShowAjaxMsg("Please Select Employee and  Ward !", Page);
                    return;
                }
                string strMsg = objwm.SaveWardlagTagging(strEmpXML.ToString(), strXML.ToString(), common.myInt(Session["HospitalLocationID"]),
                                                common.myInt(Session["FacilityId"]), common.myInt(Session["UserID"]));

                //if(!isSaved())
                //{
                //    return;
                //}
                if (strMsg.Contains("Record Saved..."))
                {


                    // Bidwardlist();
                    bindTaggedFlag();
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }

                lblMessage.Text = strMsg;
            }
            //GetEmmployee(common.myInt(hdnEmpId));
            bindTaggedFlag();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lnkEmpDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkBtn = (LinkButton)sender;
            // int WardId = common.myInt(((Label)lnkBtn.FindControl("lblWardId")).Text); 
            int TagId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnTagId")).Value);

            if (TagId > 0)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hstInput = new Hashtable();
                string strqry = string.Empty;

                hstInput.Add("@Id", TagId);

                if (common.myStr(ddlTagging.SelectedItem) == "Ward")
                {
                    strqry = "UPDATE WardTagging SET Active = 0 WHERE Id = @Id";

                    if (!string.IsNullOrEmpty(strqry) && hstInput.Count > 0)
                    {
                        objDl.ExecuteNonQuery(CommandType.Text, strqry, hstInput);

                    }
                }
                else
                {
                    strqry = "UPDATE EmployeeWiseOTTagging SET Active = 0 WHERE Id = @Id";

                    if (!string.IsNullOrEmpty(strqry) && hstInput.Count > 0)
                    {
                        objDl.ExecuteNonQuery(CommandType.Text, strqry, hstInput);

                    }
                }

            }

            lblMessage.Text = "Record Delete";
            bindTaggedFlag();

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
    }


    protected void chkUnchk_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvwardlist != null)
            {
                foreach (GridDataItem dataItem in gvwardlist.Items)
                {
                    CheckBox chkwardid = (CheckBox)dataItem.FindControl("chkwardid");

                    chkwardid.Checked = chkUnchk.Checked;
                    dataItem.Selected = chkUnchk.Checked;
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

    protected void chkAllEmp_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvEmployee != null)
            {
                foreach (GridDataItem dataItem in gvEmployee.Items)
                {
                    CheckBox chkEmpid = (CheckBox)dataItem.FindControl("chkEmpid");

                    chkEmpid.Checked = chkUnchk.Checked;
                    dataItem.Selected = chkUnchk.Checked;
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


    protected void ddlTagging_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseC.RestFulAPI objwcfOt = new BaseC.RestFulAPI(sConString);
        try
        {
            if (common.myStr(ddlTagging.SelectedItem) == "OT")
            {
                Panel1.Visible = false;
                panel4.Visible = true;
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet ds = new DataSet();
                //Hashtable HashIn = new Hashtable();
                //HashIn.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                //HashIn.Add("@intFacilityID", ddlFacility.SelectedValue);

                //ds= dl.FillDataSet(CommandType.StoredProcedure, "uspGetOTs", HashIn);
                lblTag.Text = "OT";
                Label2.Text = "OT Tagging";
                ds = objwcfOt.PopulateOTName(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);
                gvOTList.DataSource = ds;
                gvOTList.DataBind();
                bindTaggedFlag();
            }
            else
            {
                lblTag.Text = "Ward";
                Label2.Text = "Ward Tagging";
                panel4.Visible = false;
                Panel1.Visible = true;
                Bidwardlist();
                bindTaggedFlag();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }



    protected void gvTagged_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (ddlTagging.SelectedIndex == 0)
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem header = (GridHeaderItem)e.Item;
                header["wardName"].Text = "List of Wards";
            }
        }
        else
        {
            if (e.Item is GridHeaderItem)
            {
                GridHeaderItem header = (GridHeaderItem)e.Item;
                header["wardName"].Text = "List of OTs";
            }
        }
    }
}