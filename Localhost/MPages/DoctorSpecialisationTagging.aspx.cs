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
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Text;

public partial class MPages_DoctorSpecialisationTagging : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl;
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
            GetDoctors();
            BindSpecialisationList();
        }
    }
    void GetDoctors()
    {
        BaseC.clsLISMaster objLIS = new BaseC.clsLISMaster(sConString);
        DataTable dt = new DataTable();
        try
        {
            ddlEmployee.Items.Clear();
            dt = objLIS.getDoctorList(0, "", common.myInt(Session["HospitallocationId"]), common.myInt(0), common.myInt(Session["FacilityId"]), 0);
            if (dt.Rows.Count > 0)
            {
                ddlEmployee.DataSource = dt;
                ddlEmployee.DataTextField = "DoctorName";
                ddlEmployee.DataValueField = "DoctorId";
                ddlEmployee.DataBind();
            }
            ddlEmployee.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objLIS = null;
            dt.Dispose();
        }
    }
    private void BindSpecialisationList()
    {
        DataTable dt = new DataTable();
        BaseC.EMR emr = new BaseC.EMR(sConString);
        try
        {
            dt = emr.GetDoctorSpecialisationTagging(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), common.myInt(ddlEmployee.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                gvSpecialisationList.DataSource = dt;
                gvSpecialisationList.DataBind();
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
            dt.Dispose();
            emr = null;
        }
    }
    private void bindTaggedSpecialisation(int DoctorId)
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        DataTable dt = new DataTable();
        DataView dv = new DataView();
        try
        {
            dt = objEMR.GetDoctorSpecialisationTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(DoctorId));
            dv = new DataView(dt);
            dv.RowFilter = "IsTaggedWithDoctor=1";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvTagged.DataSource = dv.ToTable();
                gvTagged.DataBind();
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
            objEMR = null;
            dt.Dispose();
        }
    }
    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEmployee.SelectedValue != "")
        {
            BindSpecialisationList();
            bindTaggedSpecialisation(common.myInt(ddlEmployee.SelectedValue));
        }
    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ddlEmployee.SelectedIndex = 0;
        bindTaggedSpecialisation(0);
    }
    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(ddlEmployee.SelectedIndex) == 0)
        {
            strmsg += "Please select Employee !";
            isSave = false;
        }

        lblMessage.Text = strmsg;
        return isSave;
    }
    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        Hashtable hshOut = new Hashtable();
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (!isSaved())
            {
                return;
            }
            foreach (GridViewRow dataItem in gvSpecialisationList.Rows)
            {
                CheckBox chkSpecialisation = (CheckBox)dataItem.FindControl("chkSpecialisation");

                if (chkSpecialisation.Checked == true)
                {
                    HiddenField hdnSpecialisationId = (HiddenField)dataItem.FindControl("hdnSpecialisationId");
                    if (common.myInt(hdnSpecialisationId.Value) > 0)
                    {
                        coll.Add(common.myInt(hdnSpecialisationId.Value));
                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
            }
            if (strXML.ToString() != "")
            {
                hshOut = objEMR.SaveUpdateDoctorSpecialisationTagging(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                    common.myInt(ddlEmployee.SelectedValue), strXML.ToString(), common.myInt(Session["UserID"]));
                if (common.myStr(hshOut["@chvOutPut"]).Contains("Record Saved"))
                {
                    bindTaggedSpecialisation(common.myInt(ddlEmployee.SelectedValue));
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }
                lblMessage.Text = common.myStr(hshOut["@chvOutPut"]);
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
            objEMR =null;
            strXML = null;
            coll = null;
            hshOut = null;
        }
    }

    protected void chkUnchk_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvSpecialisationList != null)
            {
                foreach (GridViewRow dataItem in gvSpecialisationList.Rows)
                {
                    CheckBox chkSpecialisation = (CheckBox)dataItem.FindControl("chkSpecialisation");
                    chkSpecialisation.Checked = chkUnchk.Checked;
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
}
