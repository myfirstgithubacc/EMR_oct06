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

public partial class MRD_ViewDocument : System.Web.UI.Page
{
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl;
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ClearAll();
            txtAccountNo.Focus();
        }
    }
    protected void ClearAll()
    {
        txtAccountNo.Text = "";
        hdnRegistrationId.Value = "";
        hdnRegistrationNo.Value = "";

    }

    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        ClearAll();
        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=2";
        RadWindow1.Height = 600;
        RadWindow1.Width = 900;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void lnkShowDetails_OnClick(object sender, EventArgs e)
    {
        try
        {

            LinkButton lnkBtn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkBtn.NamingContainer;

            int iEncId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnEncId")).Value);
            int iRegId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnRegId")).Value);
            int hdnDoctorId = common.myInt(((HiddenField)lnkBtn.FindControl("hdnDoctorId")).Value);
            string hdnPayer = common.myStr(((HiddenField)lnkBtn.FindControl("hdnPayer")).Value);
            string hdnSponsor = common.myStr(((HiddenField)lnkBtn.FindControl("hdnSponsor")).Value);
            string hdnDischargeDate = common.myStr(((HiddenField)lnkBtn.FindControl("hdnDischargeDate")).Value);
            string lblEncDt = common.myStr(((Label)lnkBtn.FindControl("lblEncDate")).Text);
            string hdnEncounterDate = common.myStr(((HiddenField)lnkBtn.FindControl("hdnEncounterDate")).Value);
            string hdnBedNo = common.myStr(((HiddenField)lnkBtn.FindControl("hdnBedNo")).Value);
            string hdnBedCategory = common.myStr(((HiddenField)lnkBtn.FindControl("hdnBedCategory")).Value);
            string lblOPIP = common.myStr(((Label)lnkBtn.FindControl("lblOPIP")).Text);

            lblPayer.Text = hdnPayer.ToString();
            lblSponsor.Text = hdnSponsor.ToString();
            lblDischargeDate.Text = hdnDischargeDate.ToString();
            lblEncDate.Text = hdnEncounterDate.ToString();
            lblEncounterNo.Text = lnkBtn.Text;
            lblBedCategory.Text = hdnBedNo.ToString() + "/" + hdnBedCategory.ToString();

            if (lblOPIP.ToString() == "O")
            {
                lblEnc.Text = "Enc#";
                lblEDt.Text = "Enc.Dt.";
            }
            else
            {
                lblEnc.Text = "IP#";
                lblEDt.Text = "Adm.Dt.";
            }
            Session["RegistrationID"] = iRegId;
            Session["encounterid"] = iEncId;
            ViewState["EncounterId"] = iEncId;
            Session["DoctorID"] = hdnDoctorId;
            bindImage();
           
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected string CreateWindowScript(object id)
    {
      
        if (txtAccountNo.Text == "")
        {
            return string.Format("var win = window.radopen('/EMR/DisplayImage.aspx?id={0}&regno={1}&hosid={2}','Details');win.add_close(OnClientClose);win.center();win.maximize();win.set_visibleStatusbar(false); ", Convert.ToString(id), Session["registrationId"].ToString(), Session["HospitalLocationId"].ToString());
        }
        else
        {
            return string.Format("var win = window.radopen('/EMR/DisplayImage.aspx?id={0}&regno={1}&hosid={2}','Details');win.add_close(OnClientClose);win.center();win.maximize();win.set_visibleStatusbar(false);", Convert.ToString(id), txtAccountNo.Text.ToString(), Session["HospitalLocationId"].ToString());
        }
    }

    protected Unit ImageWidth
    {
        get
        {
            object state = ViewState["ImageWidth"] ?? Unit.Pixel(200);
            return (Unit)state;
        }
        private set { ViewState["ImageWidth"] = value; }
    }

    protected Unit ImageHeight
    {
        get
        {
            object state = ViewState["ImageHeight"] ?? Unit.Pixel(200);
            return (Unit)state;
        }
        private set { ViewState["ImageHeight"] = value; }
    }

    private void bindImage()
    {
        try
        {
            string strSQL = "";
            if (txtAccountNo.Text != "")
            {
                DAL.DAL objdl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
               DataSet ds = new DataSet();
               Hashtable hstInput = new Hashtable();
                string smappath = Server.MapPath("");
                hstInput.Add("@RegistrationId", hdnRegistrationId.Value);
                hstInput.Add("@EncounterId",common.myInt(ViewState["EncounterId"]));
                strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type from EMRPatientDocuments pd";
                strSQL += " where RegistrationID = @RegistrationId AND EncounterId=@EncounterId AND DocumentTypeId=3  AND Active=1";
                ds = objdl.FillDataSet(CommandType.Text, strSQL, hstInput);


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {


                        RadListView1.DataSource = ds;
                        RadListView1.DataBind();
                    }
                    else
                    {
                        RadListView1.DataSource = null;
                        RadListView1.DataBind();
                    }
                }
                else
                {
                    RadListView1.DataSource = null;
                    RadListView1.DataBind();
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
    protected void RadListView1_PageIndexChanged(object sender, Telerik.Web.UI.RadListViewPageChangedEventArgs e)
    {
        bindImage();
     
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@intGroupId", Convert.ToInt32(Session["GroupID"]));
            ds = objDl.FillDataSet(CommandType.Text, "select GroupId from SecGroupMaster where GroupId= @intGroupId and Admin=1 and Active=1", hsinput);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Button btndel = new Button();
                btndel = (Button)sender;
                //Response.Write(btnup.CommandArgument.ToString());
                objDl.ExecuteNonQuery(CommandType.Text, "update EMRPatientDocuments set Active=0 where Id=" + btndel.CommandArgument.ToString());
                bindImage();
              //  bindDocumentCategory();
               // bindDocumentType();
               
             //   ltrlMessage.Text = "<span style='color:Red; font-weight:Bold'>Document delete successfull!</span>";
            }
            else
            {
                Alert.ShowAjaxMsg("You are Not Authorized to Delete Attachment!", this);
                return;
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        if (common.myStr(txtAccountNo.Text.Trim()) != "")
        {
            //Added by ujjwal 06 July 2015 to validate UHID start
            if (!txtAccountNo.Text.Trim().Length.Equals(0))
            {
                int UHID;
                int.TryParse(txtAccountNo.Text, out UHID);
                if ((UHID > 2147483647 || UHID.Equals(0)))
                {
                    return;
                }
            }
            //Added by ujjwal 06 July 2015 to validate UHID start
            int RegistrationNo = common.myInt(txtAccountNo.Text.Trim());
            ClearAll();
            if (RegistrationNo > 0)
            {
                BaseC.Patient bC = new BaseC.Patient(sConString);
                DataSet ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, 0, 0);
                if (ds.Tables.Count > 0)
                {

                    DataRow dr = ds.Tables[0].Rows[0];
                    hdnRegistrationNo.Value = common.myStr(dr["RegistrationNo"]);
                    txtAccountNo.Text = common.myStr(dr["RegistrationNo"]);
                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    lblPayer.Text = common.myStr(dr["PatientSponsorName"]);
                    lblSponsor.Text = common.myStr(dr["PatientSponsorName"]);
                    patientQV.ShowPatientDetails(Convert.ToInt32(hdnRegistrationId.Value));
                }
                ds.Dispose();
                ds.Clear();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshIn = new Hashtable();

                hshIn.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
                hshIn.Add("@FacilityID", common.myInt(Session["FacilityID"]));
                hshIn.Add("@RegistrationId", common.myInt(hdnRegistrationId.Value));
                ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterDetails", hshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                 
                    gvVisists.DataSource = ds;
                    gvVisists.DataBind();
                }
            }
        }
    }
}
