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

public partial class WardManagement_PatientBulkTransfer : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    clsExceptionLog objException = new clsExceptionLog();


   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindBlankGrid();
            BindAssinedStaff();

        }
    }
    private void BindAssinedStaff()
    {
        ICM objICM = new ICM(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objICM.GetICMNurse(common.myInt(Session["FacilityId"]));
            ddlFromStaff.DataSource = ds.Tables[0];
            ddlFromStaff.DataTextField = "NurseName";
            ddlFromStaff.DataValueField = "Id"; ddlFromStaff.DataBind();
            ddlFromStaff.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlFromStaff.SelectedIndex = 0;

            ddlToStaff.DataSource = ds.Tables[0];
            ddlToStaff.DataTextField = "NurseName";
            ddlToStaff.DataValueField = "Id";
            ddlToStaff.DataBind();
            ddlToStaff.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ddlToStaff.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objICM = null;
            ds.Dispose();
        }



    }
    private void BindAssignedNurseDetails(int NurseId)
    {
        WardManagement objWard = new WardManagement();
        DataTable dt = new DataTable();
        try
        {
            dt = objWard.GetEMRPatientsByNurseId(NurseId);
            gvPatients.DataSource = dt;
            gvPatients.DataBind();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objWard = null;
            dt.Dispose();
        }
    }
    private void clearControls()
    {
        txtRemarks.Text = string.Empty;
        ddlFromStaff.SelectedIndex = 0;
        ddlToStaff.SelectedIndex = 0;
    }
    private bool ValidateForm()
    {
        lblMessage.ForeColor = System.Drawing.Color.Red;
        var IsPatientSelected = false;

        if (ddlFromStaff.SelectedValue == "0")
        {
            ddlFromStaff.Focus();
            lblMessage.Text = "Please select from staff";

            return false;
        }
        else if (ddlToStaff.SelectedValue == "0")
        {
            ddlToStaff.Focus();
            lblMessage.Text = "Please select To staff";
            return false;
        }

        else if (ddlFromStaff.SelectedValue.Equals(ddlToStaff.SelectedValue))
        {
            lblMessage.Text = "From Staff and To staff can't be same";
            return false;
        }

        foreach (GridDataItem item in gvPatients.MasterTableView.Items)
        {
            CheckBox childChkbox = (CheckBox)item.FindControl("chkItem");
            if (childChkbox.Checked)
            {
                IsPatientSelected = true;
                break;
            }

        }
        if (!IsPatientSelected)
        {
            lblMessage.Text = "Please check at leaset one patient";
            return false;
        }

        return true;

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        lblMessage.ForeColor = System.Drawing.Color.Green;
        WardManagement objWard = new WardManagement();
        try
        {
            if (ValidateForm())
            {
                System.Text.StringBuilder sbXML = new System.Text.StringBuilder();
                foreach (GridDataItem item in gvPatients.MasterTableView.Items)
                {
                    CheckBox chkItem = (CheckBox)item.FindControl("chkItem");
                    if (chkItem.Checked)
                    {
                        var arrayList = new ArrayList();
                        HiddenField hdnEncounterId = (HiddenField)item.FindControl("hdnEncounterId");
                        arrayList.Add(hdnEncounterId.Value);
                        sbXML.Append(common.setXmlTable(ref arrayList));

                    }
                }

                var fromNurseId = common.myInt(ddlFromStaff.SelectedValue);
                var toNurseId = common.myInt(ddlToStaff.SelectedValue);

                var result = objWard.SavePatientNurseTransfer(fromNurseId, toNurseId, sbXML.ToString(), txtRemarks.Text, common.myInt(Session["UserId"]), common.myInt(Session["HospitalLocationID"]));
                if (result.Contains("Patients transferred."))
                {
                    ddlFromStaff.SelectedIndex = 0;
                    ddlToStaff.SelectedIndex = 0;
                    txtRemarks.Text = string.Empty;
                    lblMessage.Text = result;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    BindBlankGrid();

                }
                else
                {
                    lblMessage.Text = result;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objWard = null;
        }


    }

    protected void chkHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkHeader = (CheckBox)sender;

        foreach (GridDataItem item in gvPatients.MasterTableView.Items)
        {
            CheckBox childChkbox = (CheckBox)item.FindControl("chkItem");
            childChkbox.Checked = chkHeader.Checked;
        }
    }
    private DataTable CreateBlankGrid()
    {
        DataTable dt = CreateBlankTable();
        DataRow dr = dt.NewRow();
        dr["RegistrationNo"] = string.Empty;
        dr["EncounterId"] = 0;
        dr["EncounterNo"] = string.Empty;
        dr["PatientName"] = string.Empty;
        dr["Remarks"] = string.Empty;

        dt.Rows.Add(dr);
        return dt;
    }
    private DataTable CreateBlankTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("RegistrationNo", typeof(string));
        dt.Columns.Add("EncounterId", typeof(int));
        dt.Columns.Add("EncounterNo", typeof(string));
        dt.Columns.Add("PatientName", typeof(string));
        dt.Columns.Add("Remarks", typeof(string));


        return dt;
    }
    private void BindBlankGrid()
    {
        gvPatients.DataSource = CreateBlankTable();
        gvPatients.DataBind();
    }




    protected void ddlFromStaff_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindAssignedNurseDetails(common.myInt(ddlFromStaff.SelectedValue));
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindow1.NavigateUrl = "/WardManagement/NursesAllocationReport.aspx";
            RadWindow1.Height = 550;
            RadWindow1.Width = 850;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
}