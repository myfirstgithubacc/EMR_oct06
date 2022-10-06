using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Data.SqlClient;

public partial class EMR_AttachDocument : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string sFileServerAdd = ConfigurationManager.AppSettings["FileServerAdd"];
    clsExceptionLog objException = new clsExceptionLog();
    Hashtable hstInput;
    BaseC.ATD objatd;
    BaseC.Patient ObjPat;
    BaseC.Security objSecurity;
    DataSet ds;
    string[] strdocType = { ".pdf", ".doc", ".xls", ".docx", ".xlsx", ".txt", ".zip" };

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Request.QueryString["ATD"] != "ATD")
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            }
        }
        else
        {
            if (common.myStr(Request.QueryString["MASTER"]) == "No" || common.myStr(Session["AttachDocMaster"]) == "No")
            {
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
                Session["AttachDocMaster"] = common.myStr(Session["AttachDocMaster"]) == null ? common.myStr(Request.QueryString["MASTER"]) : common.myStr(Session["AttachDocMaster"]);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            if (Request.QueryString["Category"] != null)
            {
                if (Request.QueryString["Category"].ToString() == "PopUp")
                {
                    btnNew.Visible = false;
                    txtAccountNo.Visible = false;
                    lbtnSearchPatient.Visible = false;
                }
                else
                {
                    btnNew.Visible = true;
                    txtAccountNo.Visible = true;
                    lbtnSearchPatient.Visible = true;
                }
            }
            else
            {
                btnNew.Visible = true;
                txtAccountNo.Visible = true;
                lbtnSearchPatient.Visible = true;
            }


            if (common.myLen(txtAccountNo.Text) > 0)
            {
                txtAccountNo.ReadOnly = true;
                txtAccountNo.Enabled = false;
            }

            if (Request.QueryString["Usefor"] == "MRD")
            {
                ViewState["UseFor"] = "MRD";
            }


            objSecurity = new BaseC.Security(sConString);
            bool CheckPermissions = false;
            string isDeletePermission = "N";

            CheckPermissions = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "AllowDocumentDeletePermission");

            if (CheckPermissions)
            {
                isDeletePermission = "Y";
            }

            ViewState["isDeletePermission"] = isDeletePermission;

            if (Request.QueryString["RegId"] != null && Request.QueryString["RegNo"] != null && Request.QueryString["EncId"] != null && Request.QueryString["DocType"] != null)
            {
                txtRegNo.Text = common.myStr(Request.QueryString["RegNo"]);
                PatientInfo();
                BindRegistrationId();

                iFrame1.Attributes.Add("src", common.myStr(sFileServerAdd) + "?RegNo=" + common.myStr(txtRegNo.Text) + "&EncId=" + common.myStr(Request.QueryString["EncId"]) + "&DocType=" + common.myStr(Request.QueryString["DocType"]) + "&FID=" + common.myStr(Session["FacilityID"]) + "&ISDP=" + common.myStr(ViewState["isDeletePermission"]));
            }
            else if (Request.QueryString["RegId"] != null && Request.QueryString["EncId"] != null)
            {
                ObjPat = new BaseC.Patient(sConString);
                txtRegNo.Text = ObjPat.GetPatientRegistrationNo(common.myInt(Request.QueryString["RegId"]));

                PatientInfo();
                BindRegistrationId();

                iFrame1.Attributes.Add("src", common.myStr(sFileServerAdd) + "?RegNo=" + common.myStr(txtRegNo.Text) + "&EncId=" + common.myStr(Request.QueryString["EncId"]) + "&FID=" + common.myStr(Session["FacilityID"]) + "&ISDP=" + common.myStr(ViewState["isDeletePermission"]));
            }
            else if (Request.QueryString["RNo"] != null)
            {
                txtRegNo.Text = Request.QueryString["RNo"].ToString();
                txtAccountNo.Text = Request.QueryString["RNo"].ToString();

                PatientInfo();
                BindRegistrationId();

                iFrame1.Attributes.Add("src", common.myStr(sFileServerAdd) + "?RegNo=" + common.myStr(txtRegNo.Text) + "&FID=" + common.myStr(Session["FacilityID"]) + "&ISDP=" + common.myStr(ViewState["isDeletePermission"]));
            }
            if(Request.QueryString["CloseButtonShow"] !=null && !common.myStr(Request.QueryString["CloseButtonShow"]).Equals(string.Empty))
            {
                if(common.myStr(Request.QueryString["CloseButtonShow"]).Equals("Yes"))
                {
                    btnClose.Visible = true;
                }

            }

        }

        if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
        {
            btnUpload.Visible = false;
        }

    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        //Response.Redirect("/emr/AttachDocument.aspx?Usefor=MRD");
        Response.Redirect("/emr/AttachDocumentFTP.aspx?Usefor=MRD");
    }
    protected void BindRegistrationId()
    {

        if (common.myStr(txtRegNo.Text) != "")
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            hstInput = new Hashtable();
            hstInput.Add("@chvRegistrationNo", common.myLong(txtRegNo.Text.Trim()));

            string strsql = "select Id, firstname from Registration WITH (NOLOCK) Where RegistrationNo=@chvRegistrationNo";
            ds = dl.FillDataSet(CommandType.Text, strsql, hstInput);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                hdnRegistrationId.Value = ds.Tables[0].Rows[0]["Id"].ToString().Trim();
                hdnRegistrationName.Value = ds.Tables[0].Rows[0]["firstname"].ToString().Trim();
            }
            else
            {
                lblMessage.Text = "Invalid UHID No";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
        }
    }

    private void PatientInfo()
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.ParseData bParse = new BaseC.ParseData();
        try
        {
            string sXSL = "/include/xsl/PatientInfo-US.xsl";

            if (common.myStr(txtRegNo.Text) != "")
            {
                SqlDataReader dr;

                if (common.myLen(txtAccountNo.Text) > 0 && Convert.ToInt64(common.myStr(txtRegNo.Text)) > 2147483647)
                {
                    return;
                }
                dr = (SqlDataReader)bC.getPatientDetails(Convert.ToInt32(bParse.ParseQ(common.myStr(txtRegNo.Text))), 1, Convert.ToInt32(Session["HospitalLocationID"]));


                //dr.NextResult();
                if (dr.Read())
                {
                    xmlPatientInfo.DocumentContent = dr[0].ToString();
                    xmlPatientInfo.TransformSource = sXSL;
                    xmlPatientInfo.DataBind();

                }
                else
                {
                    xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                    xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                    xmlPatientInfo.DataBind();

                }
                dr.Close();

            }
            else
            {
                xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                xmlPatientInfo.DataBind();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //added bys sikandar for code optimize
        finally
        {
            bC = null;
            bParse = null;
        }
    }



    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        lblMessage.Text = String.Empty;

        if (common.myLen(txtAccountNo.Text) > 0 && Convert.ToInt64(txtAccountNo.Text) > 9223372036854775807)
        {
            Alert.ShowAjaxMsg("Value should not be more than 9223372036854775807.", this.Page);
            txtAccountNo.Text = txtAccountNo.Text.Substring(0, 12);
        }

        if (!common.myLong(txtAccountNo.Text).Equals("0") || !common.myLong(hdnRegistrationNo.Value).Equals("0"))
        {
            if (common.myLong(hdnRegistrationNo.Value) != 0)
            {
                txtAccountNo.Text = hdnRegistrationNo.Value;
                hdnRegistrationNo.Value = "0";
            }
            else
            {
                txtRegNo.Text = txtAccountNo.Text;
            }

            PatientInfo();
            BindRegistrationId();

            iFrame1.Attributes.Add("src", common.myStr(sFileServerAdd) + "?RegNo=" + common.myStr(txtRegNo.Text) + "&FID=" + common.myStr(Session["FacilityID"]) + "&ISDP=" + common.myStr(ViewState["isDeletePermission"]));


        }


    }

    protected void btnNo_OnClick(Object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true); // Call client method in radwindow page 
    }

    protected void lnkScan_Onclick(object sender, EventArgs e)
    {
        if (common.myInt(txtRegNo.Text.Trim()) == 0)
        {
            Alert.ShowAjaxMsg("Please Enter Patient ID", this);
            txtRegNo.Focus();
            return;
        }
        string navstring = "";
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            navstring = dl.ExecuteScalar(CommandType.Text, "select Isnull(LocalFileScanFolder,'') from FileServersetup where FacilityID=" + Session["FacilityID"].ToString()).ToString();
            Session["LocFile"] = navstring;
        }
        catch (Exception ex)
        {

        }
        string Str = "DoScan:ScanNow?ID=" + navstring + "\\RegNo=" + txtRegNo.Text + "&Filename=ASPLScan_" + DateTime.Now.ToString("ddMMyyyyHHmm");
        ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);
    }

    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetailsV1.aspx?OPIP=O&RegEnc=0";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }


}