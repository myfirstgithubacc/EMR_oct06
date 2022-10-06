using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BaseC;
using System.IO;
using System.Xml;
using System.Text;

public partial class WardManagement_InterHospitalTransferReq : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (common.myStr(Request.QueryString["RegNo"]) != "")
            {
                Session["StatusCngCheck"] = 0;
                hdnRegistrationNo.Value = common.myStr(Request.QueryString["RegNo"]);
                BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]));
            }
            if (common.myStr(Request.QueryString["encno"]) != "")
            {
                hdnEncounterNo.Value = common.myStr(Request.QueryString["encno"]);
            }
            if (common.myStr(Request.QueryString["cid"]) != "")
            {
                hdnCompanyCode.Value = Request.QueryString["cid"];
            }
            
            



            BindToHospital(common.myInt(Session["FacilityId"]),hdnRegistrationNo.Value, hdnEncounterNo.Value);

            

        }
    }

    void BindPatientHiddenDetails(String RegistrationNo)
    {
        try
        {
            ViewState["StatusId"] = common.myStr(Request.QueryString["StatusId"]);
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = Session["PatientDetailString"].ToString();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void BindToHospital(int FacilityId,string RegistrationNo,string EncounterNo)
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        BaseC.ATD objatd = new BaseC.ATD(sConString);
        BaseC.Patient patient = new BaseC.Patient(sConString);
        try
        {
            
            ds1 = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            common.myStr(RegistrationNo), common.myStr(EncounterNo), common.myInt(Session["UserId"]), 0);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ds = objatd.GetToHospital(FacilityId);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ddlToHospital.DataSource = ds;
                    ddlToHospital.DataTextField = "name";
                    ddlToHospital.DataValueField = "FacilityID";
                    ddlToHospital.DataBind();
                    ddlToHospital.Items.Insert(0, new RadComboBoxItem("Select", "0"));

                    ddlToHospital.SelectedIndex = ddlToHospital.Items.IndexOf(ddlToHospital.FindItemByValue(common.myStr(ds1.Tables[0].Rows[0]["HospitalLocationId"])));

                    ViewState["SpecialisationId"] = common.myInt(ds1.Tables[0].Rows[0]["SpecialisationId"]);
                    ViewState["DoctorId"] = common.myInt(ds1.Tables[0].Rows[0]["DoctorId"]);
                    ViewState["BedCategoryId"] = common.myInt(ds1.Tables[0].Rows[0]["BedCategoryId"]);

                    DataSet ds2 = new DataSet();
                    BaseC.WardManagement objwd = new BaseC.WardManagement();
                    ds2 = objwd.GetFacilityTransferDetails(common.myInt(Session["FacilityId"]),common.myInt(Session["HospitalLocationId"]),common.myInt(Request.QueryString["encId"]));

                    if (common.myInt(Request.QueryString["IsSecondaryFacility"]).Equals(1))
                    {
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            ddlToHospital.Text = common.myStr(ds2.Tables[0].Rows[0]["ToFacility"]);
                            ddlToHospital.SelectedValue = common.myStr(Request.QueryString["ToFacilityId"]);
                            ddlToHospital.Enabled = false;

                            ViewState["SpecialisationId"] = common.myInt(ds2.Tables[0].Rows[0]["SpecilizationId"]);
                            ViewState["DoctorId"] = common.myInt(ds2.Tables[0].Rows[0]["DoctorId"]);
                            ViewState["BedCategoryId"] = common.myInt(ds2.Tables[0].Rows[0]["BedCategoryId"]);

                            BindSpecilization(common.myInt(ds2.Tables[0].Rows[0]["SpecilizationId"]));
                            BindProvider(common.myInt(ds2.Tables[0].Rows[0]["DoctorId"]));
                            BindBedCategory(common.myInt(ds2.Tables[0].Rows[0]["BedCategoryId"]));

                           
                        }
                    }
                    else
                    {
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            ddlToHospital.Text = common.myStr(ds2.Tables[0].Rows[0]["ToFacility"]);
                            ddlToHospital.SelectedValue = common.myStr(Request.QueryString["ToFacilityId"]);
                            ddlToHospital.Enabled = false;

                            ViewState["SpecialisationId"] = common.myInt(ds2.Tables[0].Rows[0]["SpecilizationId"]);
                            ViewState["DoctorId"] = common.myInt(ds2.Tables[0].Rows[0]["DoctorId"]);
                            ViewState["BedCategoryId"] = common.myInt(ds2.Tables[0].Rows[0]["BedCategoryId"]);

                            BindSpecilization(common.myInt(ds2.Tables[0].Rows[0]["SpecilizationId"]));
                            BindProvider(common.myInt(ds2.Tables[0].Rows[0]["DoctorId"]));
                            BindBedCategory(common.myInt(ds2.Tables[0].Rows[0]["BedCategoryId"]));
                        }
                      
                        //ddlToHospital.Text = common.myStr(ds2.Tables[0].Rows[0]["Name"]);
                    }
                  

                   
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ds.Dispose();
            objatd = null;
        }
    }
    protected void ddlToHospital_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
       
        BindSpecilization(common.myInt(ViewState["SpecialisationId"]));
        BindProvider(common.myInt(ViewState["DoctorId"]));
        BindBedCategory(common.myInt(ViewState["BedCategoryId"]));
    }
    protected void ddlSpecilization_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
       
           
               BindProvider(0);
            //if (ddlSpecilization.SelectedValue != "0")
            //{
            //    RadComboBoxItem Provider = ddlProvider.Items.FindItemByValue("0");
            //    if (Provider != null)
            //    {
            //        ddlProvider.Items.Remove(0);
            //    }
            //}
        }

  

    private void BindSpecilization(int SpecilizationId)
    {
        BaseC.clsEMR objemr = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();

        try
        {
            ds = objemr.getFindPatientMasterList(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlToHospital.SelectedValue),
                                common.myInt(Session["UserId"]));

            #region Table-0, Specilisation

            ddlSpecilization.Text = string.Empty;
            ddlSpecilization.Items.Clear();
            ddlSpecilization.ClearSelection();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSpecilization.DataSource = ds.Tables[0];
                ddlSpecilization.DataTextField = "NAME";
                ddlSpecilization.DataValueField = "ID";
                ddlSpecilization.DataBind();
                ddlSpecilization.Items.Insert(0, new RadComboBoxItem("Select", "0"));
              
                if (SpecilizationId>0)
                {
                    ddlSpecilization.SelectedIndex = ddlSpecilization.Items.IndexOf(ddlSpecilization.FindItemByValue(common.myStr(SpecilizationId)));

                    //ddlSpecilization.SelectedValue = common.myStr(SpecilizationId);
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Specialization not available", Page);
            }

            #endregion

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            objemr = null;
        }
    }
    private void BindProvider(int DoctorId)
    {
        BaseC.clsEMR objemr = new BaseC.clsEMR(sConString);
        DataSet objDs = new DataSet();
        try
        {
            objDs = objemr.getEmployeeWithResource(common.myInt(Session["FacilityId"]), common.myInt(ddlSpecilization.SelectedValue),
                                common.myInt(Session["UserId"]));


            ddlProvider.ClearSelection();
            ddlProvider.DataSource = objDs.Tables[0];
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorID";
            ddlProvider.DataBind();

            ddlProvider.Items.Insert(0, new RadComboBoxItem("Select", "0"));

            if (DoctorId > 0)
            {
                ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.FindItemByValue(common.myStr(DoctorId)));

                //ddlProvider.SelectedValue = common.myStr(DoctorId);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            objDs.Dispose();
            objemr = null;
        }
    }
    private void BindBedCategory(int bedcategoryid)
    {
        BaseC.EMRBilling objBed = new BaseC.EMRBilling(sConString);
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataTable dtEntry = new DataTable();
        try
        {
            int HospitalLocation=0;
            if (common.myInt(ddlToHospital.SelectedValue) > 0)
            {
                HospitalLocation = Convert.ToInt16(ddlToHospital.SelectedValue);
            }
            ds = objBed.GetWardBedCategoryWiseBedNo(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(HospitalLocation), common.myInt(0), 0);
            dv = new DataView(ds.Tables[0]);
            dtEntry = dv.ToTable(true, "BedCategoryId", "BedCategoryName");

            ddlbedcategory.DataSource = null;
            ddlbedcategory.DataBind();

            //if (dtEntry.Rows.Count > 0)
            //{
            ddlbedcategory.DataSource = dtEntry;
            ddlbedcategory.DataTextField = "BedCategoryName";
            ddlbedcategory.DataValueField = "BedCategoryId";
            ddlbedcategory.DataBind();

            ddlbedcategory.Items.Insert(0, new RadComboBoxItem("Select", "0"));

            if (bedcategoryid > 0)
            {
                ddlbedcategory.SelectedIndex = ddlbedcategory.Items.IndexOf(ddlbedcategory.FindItemByValue(common.myStr(bedcategoryid)));

                //ddlbedcategory.SelectedValue = common.myStr(bedcategoryid);
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            dv.Dispose();
            dtEntry.Dispose();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (common.myInt(ddlToHospital.SelectedValue).Equals(0))
        {
            Alert.ShowAjaxMsg("Please Select To Hospital!", Page);
            return;
        }
        if (common.myInt(ddlSpecilization.SelectedValue).Equals(0))
        {
            Alert.ShowAjaxMsg("Please Select Specilization!", Page);
            return;
        }
        if (common.myInt(ddlProvider.SelectedValue).Equals(0))
        {
            Alert.ShowAjaxMsg("Please Select Doctor!", Page);
            return;
        }
        if (common.myInt(ddlbedcategory.SelectedValue).Equals(0))
        {
            Alert.ShowAjaxMsg("Please Select Bed Category!", Page);
            return;
        }
        

        BaseC.WardManagement objwd = new BaseC.WardManagement();
        string str = "";
        str = objwd.SaveInterHospitalTranfer(common.myInt(hdnTransferId.Value), common.myInt(Session["FacilityId"]), common.myInt(Session["Hospitallocationid"]),
            common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), common.myInt(Session["FacilityId"]),
            common.myInt(ddlToHospital.SelectedValue), common.myInt(ddlSpecilization.SelectedValue), common.myInt(ddlProvider.SelectedValue), common.myInt(ddlbedcategory.SelectedValue),
            common.myInt(Session["UserId"]));

        if (str.Contains("Inter Hospital Transfer Request"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = str;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = str;
        }
    }
        
}