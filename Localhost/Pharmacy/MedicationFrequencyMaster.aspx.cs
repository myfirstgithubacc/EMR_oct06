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
using Telerik.Web.UI;

public partial class Pharmacy_MedicationFrequencyMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    bool RowSelStauts = false;
    bool RowSelStautsDetails = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindFacility();
            BindMfrequency();
            Bindfrequency();
        }
    }
    private void bindFacility()
    {
        DataSet ds = new DataSet();
        BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
        try
        {
            ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));

            ddlFacility.DataSource = ds.Tables[0];
            ddlFacility.DataTextField = "Name";
            ddlFacility.DataValueField = "FacilityID";
            ddlFacility.DataBind();

            //ddlFacility.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlFacility.SelectedIndex = 0;
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
            objEMRFacility = null;
        }
    }

    private void BindMfrequency()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            ds = objEMR.getFrequencyMaster(0);

            gvmfrequency.DataSource = ds.Tables[0];
            gvmfrequency.DataBind();
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
            objEMR = null;
        }
    }
    private void Bindfrequency()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            ds = objEMR.getFrequencyDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue), common.myInt(lblFrequencyId.Text), false);

            RadGrid1.DataSource = ds.Tables[0];
            RadGrid1.DataBind();
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
            objEMR = null;
        }
    }

    protected void Btnsave1_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (common.myInt(ddlFacility.SelectedValue).Equals(0))
            {
                lblMessage.Text = "Please select facility !";
            }
            if (common.myInt(lblFrequencyId.Text).Equals(0))
            {
                lblMessage.Text = "Please select frequency !";
            }

            string strMsg = objEMR.SaveFrequencyDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue),
                                                        common.myInt(lblid.Text), common.myInt(lblFrequencyId.Text),
                                                        Convert.ToDateTime(RedFrequencyTime.SelectedDate), common.myBool(ddlStatus.SelectedValue));

            if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
            {
                lblid.Text = "0";
                //lblFrequencyId.Text = "0";
                Bindfrequency();
                RedFrequencyTime.SelectedDate = null;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                Btnsave1.Text = "Save";

                lblMessage.Text = strMsg;
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
        }
    }

    protected void gvmfrequency_ItemCommand1(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "view")
            {
                lblFrequencyId.Text = ((Label)e.Item.FindControl("lblId")).Text;
                Label lbldes = (Label)e.Item.FindControl("lbldesc");
                lblFrequencyName.Text = Convert.ToString(lbldes.Text);

                Bindfrequency();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                lblFrequencyId.Text = ((Label)e.Item.FindControl("lblDetailId")).Text; // DETATIL ID
                lblid.Text = ((Label)e.Item.FindControl("lblMasterId")).Text;// MASTER ID 
                ddlStatus.SelectedValue = common.myBool(((Label)e.Item.FindControl("lblActiv")).Text) ? "1" : "0";
                RedFrequencyTime.SelectedDate = Convert.ToDateTime(((Label)e.Item.FindControl("lblfTime")).Text);

                Btnsave1.Text = "Update";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void ddlFacility_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        lblFrequencyName.Text = "";
        Bindfrequency();
    }

}
