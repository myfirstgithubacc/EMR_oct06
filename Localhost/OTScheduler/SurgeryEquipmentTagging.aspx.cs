using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using Telerik.Web.UI;

public partial class OTScheduler_SurgeryEquipmentTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            ddlService.Text = string.Empty;

            BindEquipment();

            BindDepartment();
            BindSudepartment();

            BindEquipmentTagged();
        }
    }

   
    private void BindEquipment()
    {
        BaseC.Hospital objot = new BaseC.Hospital(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objot.GetOTEquipmentmaster(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0);

            ViewState["Equipmentmaster"] = ds.Tables[0];

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEquipment.DataSource = ds.Tables[0];
                gvEquipment.DataBind();
            }
            else
            {
                BindBlankEquipment();
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
            objot = null;
            if (ds != null)
            {
                ds.Dispose();
            }
        }
    }

    private void BindEquipmentTagged()
    {
        BaseC.clsOTBooking objot = new BaseC.clsOTBooking(sConString);
        DataTable dt = new DataTable();
        try
        {
            if (common.myInt(ddlService.SelectedValue) > 0)
            {
                dt = objot.getSurgeryEquipmentTagging(common.myInt(Session["FacilityId"]), common.myInt(ddlService.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    gvEquipmentTagged.DataSource = dt;
                    gvEquipmentTagged.DataBind();
                }
                else
                {
                    BindBlankEquipmentTagged();
                }
            }
            else
            {
                BindBlankEquipmentTagged();
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
            objot = null;
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }

    private void BindBlankEquipment()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        try
        {
            dr["TaggingId"] = 0;
            dr["EquipmentName"] = string.Empty;
            dr["EquipmentId"] = 0;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvEquipment.DataSource = dt;
            gvEquipment.DataBind();
        }

        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
        finally
        {
            dt.Dispose();
        }
    }

    private void BindBlankEquipmentTagged()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        try
        {
            dr["EquipmentName"] = string.Empty;
            dr["EquipmentId"] = 0;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvEquipmentTagged.DataSource = dt;
            gvEquipmentTagged.DataBind();
        }

        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
        finally
        {
            dt.Dispose();
        }
    }
    protected DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("TaggingId", typeof(int));
            dt.Columns.Add("EquipmentName", typeof(string));
            dt.Columns.Add("EquipmentId", typeof(int));
            return dt;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
            return dt;
        }
    }

    protected void ddlService_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        try
        {
            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            //if (e.Text != "")
            //{
            DataTable data = GetData(e.Text);

            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(data.Rows[i]["ServiceName"]); // + " " + common.myStr(data.Rows[i]["RefServiceCode"]) + " " + common.myStr(data.Rows[i]["CPTCode"]);
                item.Value = data.Rows[i]["ServiceId"].ToString();
                //ddl.Items.Add(new RadComboBoxItem(data.Rows[i]["ServiceName"].ToString(), data.Rows[i]["ServiceId"].ToString()));
                item.Attributes.Add("ServiceName", data.Rows[i]["ServiceName"].ToString());
                item.Attributes.Add("DepartmentId", data.Rows[i]["DepartmentId"].ToString());
                item.Attributes.Add("SubDeptId", data.Rows[i]["SubDeptId"].ToString());
                item.Attributes.Add("RefServiceCode", data.Rows[i]["RefServiceCode"].ToString());
                item.Attributes.Add("CPTCode", data.Rows[i]["CPTCode"].ToString());
                this.ddlService.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
        }
    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }
    private DataTable GetData(string text)
    {
        BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);
        DataSet ds = new DataSet();
        DataTable data = new DataTable();

        string ServiceName = text + "%";
        string strDepartmentType = "'S'";

        try
        {
            ds = objCommon.GetHospitalServices(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue),
                         common.myInt(ddlSubDepartment.SelectedValue), strDepartmentType, ServiceName, common.myInt(Session["FacilityId"]), 0, 0);

            if (ds.Tables[0].Rows.Count > 0)
            {
                data = ds.Tables[0];
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            objCommon = null;
            ds.Dispose();
        }
        return data;
    }

    protected void btnGetInfoService_OnClick(object sender, EventArgs e)
    {
        DataView dv = new DataView();
        DataTable dt = new DataTable();
        try
        {
            BindEquipmentTagged();

            if (ViewState["Equipmentmaster"] != null)
            {
                dt = (DataTable)ViewState["Equipmentmaster"];

                dv = dt.Copy().DefaultView;
                dv.RowFilter = "EquipmentName like '%" + common.myStr(txtEquipmentSearch.Text, true) + "%'";

                if (dv.ToTable().Rows.Count > 0)
                {
                    gvEquipment.DataSource = dv.ToTable().Copy();
                    gvEquipment.DataBind();

                    gvEquipment.SelectedIndex = -1;
                }
                else
                {
                    BindBlankEquipment();
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
            dv.Dispose();
            dt.Dispose();
        }
    }
    protected void BindDepartment()
    {
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objwcfcm = new BaseC.RestFulAPI(sConString);
        try
        {
            ddlDepartment.Items.Clear();

            string strDepartmentType = "'S'";

            ds = objwcfcm.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"]), strDepartmentType);
            ddlDepartment.DataSource = ds.Tables[0];
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentID";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new RadComboBoxItem("All", ""));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objwcfcm = null;
        }
    }

    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindSudepartment();
            ddlService.Items.Clear();
            ddlService.Text = string.Empty;
            ddlService.SelectedIndex = -1;
            BindEquipmentTagged();
        }
        catch
        {
        }
        
    }

    protected void ddlSubDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlService.Items.Clear();
            ddlService.Text = string.Empty;
            ddlService.SelectedIndex = -1;
            BindEquipmentTagged();
        }
        catch
        {
        }

    }

    private void BindSudepartment()
    {
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objwcfcm = new BaseC.RestFulAPI(sConString);
        try
        {
            ddlSubDepartment.Items.Clear();
            ddlService.Items.Clear();

            string strDepartmentType = "'S'";
            int iSubDeptId = 0;

            ds = objwcfcm.GetHospitalSubDepartment(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue), strDepartmentType, iSubDeptId);
            ddlSubDepartment.DataSource = ds.Tables[0];
            ddlSubDepartment.DataTextField = "SubName";
            ddlSubDepartment.DataValueField = "SubDeptId";
            ddlSubDepartment.DataBind();

            ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("All", ""));
            ddlSubDepartment.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void gvEquipment_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("SELECT"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnEquipmentId = (HiddenField)row.FindControl("hdnEquipmentId");

                if (common.myInt(hdnEquipmentId.Value) > 0)
                {
                    SaveData(common.myInt(hdnEquipmentId.Value));
                }
            }
        }
        catch
        {
        }
    }


    private bool isSaved()
    {
        bool isSave = true;

        if (common.myInt(ddlService.SelectedValue).Equals(0))
        {
            lblMessage.Text = "Please select surgery !";
            isSave = false;
            return isSave;
        }

        return isSave;
    }

    protected void SaveData(int EquipmentId)
    {
        BaseC.clsOTBooking objot = new BaseC.clsOTBooking(sConString);
        //StringBuilder strXML = new StringBuilder();
        //ArrayList coll = new ArrayList();

        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            //foreach (GridViewRow item in gvEquipment.Rows)
            //{
            //    CheckBox chkRowEquipment = (CheckBox)item.FindControl("chkRowEquipment");
            //    HiddenField hdnEquipmentId = (HiddenField)item.FindControl("hdnEquipmentId");

            //    if (chkRowEquipment.Checked && common.myInt(hdnEquipmentId.Value) > 0)
            //    {
            //        coll.Add(common.myInt(hdnEquipmentId.Value));
            //        strXML.Append(common.setXmlTable(ref coll));
            //    }
            //}

            string strMsg = objot.saveSurgeryEquipmentTagging(0, common.myInt(ddlService.SelectedValue), EquipmentId, common.myInt(Session["UserID"]));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                BindEquipmentTagged();
            }
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void gvEquipmentTagged_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.clsOTBooking objot = new BaseC.clsOTBooking(sConString);
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("DEL"))
            {
                BaseC.clsEMR clsEMR = new BaseC.clsEMR(sConString);
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnTaggingId = (HiddenField)row.FindControl("hdnTaggingId");

                if (common.myInt(hdnTaggingId.Value) > 0)
                {
                    string strMsg = objot.saveSurgeryEquipmentTagging(common.myInt(hdnTaggingId.Value), common.myInt(ddlService.SelectedValue), 0, common.myInt(Session["UserID"]));

                    BindEquipmentTagged();
                    lblMessage.Text = strMsg;
                }
            }
        }
        catch
        {
        }
    }

}
