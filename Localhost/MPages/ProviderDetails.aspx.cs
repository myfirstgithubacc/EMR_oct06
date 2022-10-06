using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

public partial class MPages_ProviderDetails : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    BaseC.ParseData bc = new BaseC.ParseData();
    private Hashtable hshTable = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PopulateDoctor();
                populateSpecialisation();
                PopolateTaxonomyCode();
                PopulateSurgeryDoctorClassification();
            }

            if (Request.QueryString["EmpNo"] != null)
            {
                //findoption(txtemployeeno.Text, 0);
                if (ddlDoctorName.SelectedValue == "" || ddlDoctorName.SelectedValue == "0")
                {
                    ddlDoctorName.SelectedValue = Request.QueryString["EmpNo"].ToString();
                    ddlDoctorName_SelectedIndexChanged(this, null);
                    ViewState["DoctorId"] = Request.QueryString["EmpNo"];
                    //   ShowImage();
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

    private void populateSpecialisation()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select 0 Id,'[Select]' As 'Name' union all select Id,Name As 'Name' from SpecialisationMaster where active = 1 order by Name");

            ddlSpecialisation.DataSource = dr;
            ddlSpecialisation.DataValueField = "ID";
            ddlSpecialisation.DataTextField = "Name";
            ddlSpecialisation.DataBind();
            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopolateTaxonomyCode()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select '[ Select ]' as SpecialtyName, '0' as SpecialtyCode union all select SpecialtyName + ' ('+SpecialtyCode+')' as SpecialtyName, SpecialtyCode  from TaxonomyCodes order by SpecialtyName");

            ddlTaxonomyCode.DataSource = dr;
            ddlTaxonomyCode.DataValueField = "SpecialtyCode";
            ddlTaxonomyCode.DataTextField = "SpecialtyName";
            ddlTaxonomyCode.DataBind();

            dr.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void PopulateDoctor()
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet objDs = new DataSet();
            Hashtable hsinput = new Hashtable();
            ddlDoctorName.Items.Clear();

            string str = "";
            BaseC.HospitalSetup objHospitalSetup = new BaseC.HospitalSetup(sConString);
            str = objHospitalSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "isShowAllEmployeeInDoctorDetail");
            if (str == "Y")
            {
                hsinput.Add("@HospitalLocationId", Session["HospitalLocationID"].ToString());
                hsinput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", hsinput);

                ddlDoctorName.DataSource = objDs;
                ddlDoctorName.DataValueField = "EmployeeId";
                ddlDoctorName.DataTextField = "EmployeeName";
            }
            else
            {
                hsinput.Add("@HospitalLocationId", Session["HospitalLocationID"].ToString());
                hsinput.Add("@intIsMedicalProvider", Convert.ToBoolean(0));
                hsinput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hsinput);

                ddlDoctorName.DataSource = objDs;
                ddlDoctorName.DataValueField = "DoctorId";
                ddlDoctorName.DataTextField = "DoctorName";
            }
            objHospitalSetup = null;
            ddlDoctorName.DataBind();
            RadComboBoxItem rci = new RadComboBoxItem("Select");
            ddlDoctorName.Items.Insert(0, rci);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void PopulateSurgeryDoctorClassification()
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        StringBuilder sbSQL = new StringBuilder();
        Hashtable hsinput = new Hashtable();
        hsinput.Add("@intHospitalLocationId", Session["HospitalLocationID"].ToString());
        sbSQL.Append("SELECT ID, Name FROM SurgeryDoctorClassification ");
        sbSQL.Append("WHERE HospitalLocationID = @intHospitalLocationId AND Active=1  ORDER BY SequenceNo");
        ds = dl.FillDataSet(CommandType.Text, sbSQL.ToString(), hsinput);

        ddlSurgeryDoctorTypeId.DataSource = ds.Tables[0];
        ddlSurgeryDoctorTypeId.DataValueField = "ID";
        ddlSurgeryDoctorTypeId.DataTextField = "Name";
        ddlSurgeryDoctorTypeId.DataBind();
        ddlSurgeryDoctorTypeId.Items.Insert(0, " [ Select ]");
        ddlSurgeryDoctorTypeId.Items[0].Value = "0";
    }


    protected void btnSaveDetails_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValid)
            {

                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                if (ddlDoctorName.SelectedValue == "")
                {
                    Alert.ShowAjaxMsg("Please select Doctor", Page);
                    return;
                }

                Hashtable hshtableout = new Hashtable();
                Hashtable hshtablein = new Hashtable();

                hshtablein.Add("intDoctorId", ddlDoctorName.SelectedValue);
                hshtablein.Add("intFacilityId", common.myInt(Session["FacilityId"]));
                hshtablein.Add("intSpecialisationId", ddlSpecialisation.SelectedValue);
                hshtablein.Add("chvNPI", bc.ParseQ(txtNPI.Text));
                hshtablein.Add("chvUPIN", bc.ParseQ(txtUPIN.Text));
                hshtablein.Add("chvGroupMedicare", bc.ParseQ(txtGroupMedicare.Text));
                hshtablein.Add("chvDEA", bc.ParseQ(txtDEA.Text));
                if (ddlTaxonomyCode.SelectedIndex != -1)
                    hshtablein.Add("chvTaxonomyCode", bc.ParseQ(ddlTaxonomyCode.SelectedItem.Value));
                else
                    hshtablein.Add("chvTaxonomyCode", DBNull.Value);

                hshtablein.Add("chvStateMedicaid", bc.ParseQ(txtStateMedicaid.Text));
                hshtablein.Add("chvChampus", bc.ParseQ(txtChampus.Text));
                hshtablein.Add("chvBCBS", bc.ParseQ(txtBCBS.Text));
                hshtablein.Add("chvOtherProvider1", bc.ParseQ(txtotherProvider1.Text));
                hshtablein.Add("chvOtherProvider2", bc.ParseQ(txtotherProvider2.Text));
                hshtablein.Add("chvOtherProvider3", bc.ParseQ(txtotherProvider3.Text));
                hshtablein.Add("chvMediPass", bc.ParseQ(txtMediPass.Text));
                hshtablein.Add("chvChampVA", bc.ParseQ(txtChampVA.Text));
                hshtablein.Add("chvFECA", bc.ParseQ(txtFECA.Text));
                hshtablein.Add("chvStateLic", bc.ParseQ(txtStateLic.Text));
                hshtablein.Add("chvSpecialityLic", bc.ParseQ(txtSpecialityLic.Text));
                hshtablein.Add("chvStateContrLic", bc.ParseQ(txtStateContrLic.Text));
                hshtablein.Add("chvBusinessName", bc.ParseQ(txtBusinessName.Text));
                hshtablein.Add("chvProviderFederalTaxId", bc.ParseQ(txtProviderTaxId.Text));
                hshtablein.Add("chvBusinessFederalTaxId", bc.ParseQ(txtBusinessTaxId.Text));
                hshtablein.Add("chvSurescriptID", bc.ParseQ(txtSureScrptId.Text));
                hshtablein.Add("chvBusinessFederalTaxType", bc.ParseQ(rblBusinessTaxIdType.SelectedValue));
                hshtablein.Add("chvProviderFederalTaxType", bc.ParseQ(rblProviderTaxIdType.SelectedValue));
                hshtablein.Add("Notes", bc.ParseQ(txtDoctorNotes.Text));
                hshtablein.Add("chvSignatureLine1", bc.ParseQ(txtSignatureLine1.Text));
                hshtablein.Add("chvSignatureLine2", bc.ParseQ(txtSignatureLine2.Text));
                hshtablein.Add("chvSignatureLine3", bc.ParseQ(txtSignatureLine3.Text));
                hshtablein.Add("chvSignatureLine4", bc.ParseQ(txtSignatureLine4.Text));
                if (chkSignatureOnFile.Checked == true)
                    hshtablein.Add("inyPhysicianSignature", 1);
                else
                    hshtablein.Add("inyPhysicianSignature", 0);

                if (chkSignatureWithDateTime.Checked == true)
                    hshtablein.Add("bitSignatureWithDateTime", 1);
                else
                    hshtablein.Add("bitSignatureWithDateTime", 0);

                if (chkSecondaryDoctor.Checked == true)
                {
                    hshtablein.Add("bitSecondaryDoctorNeeded", 1);
                }
                else
                {
                    hshtablein.Add("bitSecondaryDoctorNeeded", 0);
                }


                hshtablein.Add("intEncodedBy", common.myInt(Session["UserID"]));

                if (common.myInt(ddlSurgeryDoctorTypeId.SelectedValue) > 0)
                    hshtablein.Add("intSurgeryDoctorTypeId", common.myInt(ddlSurgeryDoctorTypeId.SelectedValue));

                hshtablein.Add("inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hshtableout.Add("chvErrorStatus", SqlDbType.VarChar);

                hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveDoctorDetails", hshtablein, hshtableout);

                if (lblBoolSignature.Text == "True")
                {
                    DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/DoctorImages"));
                    if (objDir.Exists == true)
                    {
                        FileInfo[] fi_array = objDir.GetFiles();
                        foreach (FileInfo files in fi_array)
                        {
                            if (files.Exists)
                            {
                                SaveImage();
                            }
                        }
                    }
                }
                lblMessage.Text = hshtableout["chvErrorStatus"].ToString();
                lblMessage.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlDoctorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clearPage();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            System.Text.StringBuilder str = new System.Text.StringBuilder(2000);
            str.Remove(0, str.Length);

            //str.Append("Select SpecialisationId,NPI,UPIN,GroupMedicare,DEA,TaxonomyCode,StateMedicaid,");
            //str.Append(" Champus,BCBS,OtherProvider1,OtherProvider2,OtherProvider3,MediPass,ChampVA,");
            //str.Append(" FECA,StateLic,SpecialityLic,StateContrLic,BusinessName,ProviderFederalTaxId,");
            //str.Append(" ProviderFederalTaxIdType,BusinessFederalTaxId,BusinessFederalTaxIdType,Isnull(PhysicianSignature,0) PhysicianSignature,Isnull(SignatureWithDateTime,0) SignatureWithDateTime,SurescriptID");
            //str.Append(" From DoctorDetails Where DoctorId = " + Convert.ToString(ddlDoctorName.SelectedValue));
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@intProviderId", ddlDoctorName.SelectedValue);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetProviderDetails", hsinput);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSpecialisation.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["SpecialisationId"]);
                txtNPI.Text = Convert.ToString(ds.Tables[0].Rows[0]["NPI"]);
                txtUPIN.Text = Convert.ToString(ds.Tables[0].Rows[0]["UPIN"]);
                txtGroupMedicare.Text = Convert.ToString(ds.Tables[0].Rows[0]["GroupMedicare"]);
                txtDEA.Text = Convert.ToString(ds.Tables[0].Rows[0]["DEA"]);
                if (Convert.ToString(ds.Tables[0].Rows[0]["TaxonomyCode"]) != "0")
                {
                    ddlTaxonomyCode.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["TaxonomyCode"]);
                }
                txtStateMedicaid.Text = Convert.ToString(ds.Tables[0].Rows[0]["StateMedicaid"]);
                txtChampus.Text = Convert.ToString(ds.Tables[0].Rows[0]["Champus"]);
                txtBCBS.Text = Convert.ToString(ds.Tables[0].Rows[0]["BCBS"]);
                txtotherProvider1.Text = Convert.ToString(ds.Tables[0].Rows[0]["OtherProvider1"]);
                txtotherProvider2.Text = Convert.ToString(ds.Tables[0].Rows[0]["OtherProvider2"]);
                txtotherProvider3.Text = Convert.ToString(ds.Tables[0].Rows[0]["OtherProvider3"]);
                txtMediPass.Text = Convert.ToString(ds.Tables[0].Rows[0]["MediPass"]);
                txtChampVA.Text = Convert.ToString(ds.Tables[0].Rows[0]["ChampVA"]);
                txtFECA.Text = Convert.ToString(ds.Tables[0].Rows[0]["FECA"]);
                txtStateLic.Text = Convert.ToString(ds.Tables[0].Rows[0]["StateLic"]);
                txtSpecialityLic.Text = Convert.ToString(ds.Tables[0].Rows[0]["SpecialityLic"]);
                txtStateContrLic.Text = Convert.ToString(ds.Tables[0].Rows[0]["StateContrLic"]);
                txtBusinessName.Text = Convert.ToString(ds.Tables[0].Rows[0]["Qualification"]);
                txtProviderTaxId.Text = Convert.ToString(ds.Tables[0].Rows[0]["ProviderFederalTaxId"]);
                txtBusinessTaxId.Text = Convert.ToString(ds.Tables[0].Rows[0]["BusinessFederalTaxId"]);
                rblProviderTaxIdType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ProviderFederalTaxIdType"]);
                rblBusinessTaxIdType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["BusinessFederalTaxIdType"]);
                txtSureScrptId.Text = Convert.ToString(ds.Tables[0].Rows[0]["SurescriptID"]);
                chkSignatureOnFile.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PhysicianSignature"]);
                chkSignatureWithDateTime.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["SignatureWithDateTime"]);
                txtDoctorNotes.Text = Convert.ToString(ds.Tables[0].Rows[0]["Notes"]);
                chkSecondaryDoctor.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["isRequiredSecondoryDoctorOnATD"]);
                txtSignatureLine1.Text = common.myStr(ds.Tables[0].Rows[0]["SignatureLine1"]);
                txtSignatureLine2.Text = common.myStr(ds.Tables[0].Rows[0]["SignatureLine2"]);
                txtSignatureLine3.Text = common.myStr(ds.Tables[0].Rows[0]["SignatureLine3"]);
                txtSignatureLine4.Text = common.myStr(ds.Tables[0].Rows[0]["SignatureLine4"]);
                if (common.myInt(ds.Tables[0].Rows[0]["SurgeryDoctorTypeId"]) > 0)
                    ddlSurgeryDoctorTypeId.SelectedIndex = ddlSurgeryDoctorTypeId.Items.IndexOf(ddlSurgeryDoctorTypeId.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0]["SurgeryDoctorTypeId"])));
                else
                    ddlSurgeryDoctorTypeId.SelectedValue = "0";
                //DeleteFiles();
            }
            ShowImage();
            ViewState["DoctorId"] = ddlDoctorName.SelectedValue;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void clearPage()
    {
        lblMessage.Visible = false;
        ddlSpecialisation.SelectedIndex = 0;
        txtNPI.Text = "";
        txtUPIN.Text = "";
        txtGroupMedicare.Text = "";
        txtDEA.Text = "";
        ddlTaxonomyCode.SelectedIndex = 0;
        txtStateMedicaid.Text = "";
        txtChampus.Text = "";
        txtBCBS.Text = "";
        txtotherProvider1.Text = "";
        txtotherProvider2.Text = "";
        txtotherProvider3.Text = "";
        txtMediPass.Text = "";
        txtChampVA.Text = "";
        txtFECA.Text = "";
        txtStateLic.Text = "";
        txtSpecialityLic.Text = "";
        txtStateContrLic.Text = "";
        txtBusinessName.Text = "";
        txtProviderTaxId.Text = "";
        txtBusinessTaxId.Text = "";
        txtSureScrptId.Text = "";
        txtDoctorNotes.Text = "";
        txtSignatureLine1.Text = "";
        txtSignatureLine2.Text = "";
        txtSignatureLine3.Text = "";
        txtSignatureLine4.Text = "";
        chkSignatureOnFile.Checked = false;
        chkSignatureWithDateTime.Checked = false;

        for (int i = 0; i < rblProviderTaxIdType.Items.Count; i++)
        {
            rblProviderTaxIdType.Items[i].Selected = false;
        }

        for (int i = 0; i < rblBusinessTaxIdType.Items.Count; i++)
        {
            rblBusinessTaxIdType.Items[i].Selected = false;
        }
    }

    protected void lnkEmployeeDetails_OnClick(object sender, EventArgs e)
    {
    }

    protected void Upload_OnClick(Object sender, EventArgs e)
    {
        try
        {
            string sFileName = "";
            StringBuilder objStr = new StringBuilder();

            if (FileUploader.FileName != "")
            {
                //  DeleteFiles();
                sFileName = FileUploader.FileName.ToString();
                ViewState["FileName"] = sFileName;
                FileUploader.SaveAs(Server.MapPath("/PatientDocuments/DoctorImages/") + FileUploader.FileName);
                PatientImage.ImageUrl = "/PatientDocuments/DoctorImages/" + FileUploader.FileName;
                lblBoolSignature.Text = "True";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void ShowImage()
    {
        try
        {
            StringBuilder strSQL = new StringBuilder();
            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmdTemp;
            SqlParameter prm1;

            strSQL.Append("select top 1 SignatureImage,ImageType from EmployeeSignatureImage where EmployeeId=@EmployeeId and Active=1 order by   EncodedDate desc ");
            cmdTemp = new SqlCommand(strSQL.ToString(), con);
            cmdTemp.CommandType = CommandType.Text;

            prm1 = new SqlParameter();
            prm1.ParameterName = "@EmployeeId";
            prm1.Value = bc.ParseQ(ddlDoctorName.SelectedValue.ToString());
            prm1.SqlDbType = SqlDbType.Int;
            prm1.Direction = ParameterDirection.Input;

            cmdTemp.Parameters.Add(prm1);

            con.Open();
            SqlDataReader dr = cmdTemp.ExecuteReader();

            if (dr.HasRows == true)
            {
                dr.Read();
                Stream strm;
                String FileName = dr["ImageType"].ToString();
                if (dr["SignatureImage"].ToString() != "")
                {
                    Object img = dr["SignatureImage"];
                    strm = new MemoryStream((byte[])img);
                    byte[] buffer = new byte[strm.Length];
                    int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                    FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);
                    fs.Write(buffer, 0, byteSeq);
                    fs.Dispose();
                }
                dr.Close();
                PatientImage.ImageUrl = "/PatientDocuments/DoctorImages/" + FileName;
            }
            else
            {
                PatientImage.ImageUrl = "/Images/Signature.jpg";
            }
            dr.Close();
            con.Close();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void DeleteFiles()
    {
        try
        {
            DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/DoctorImages"));
            if (objDir.Exists == true)
            {
                FileInfo[] fi_array = objDir.GetFiles();
                foreach (FileInfo files in fi_array)
                {
                    if (files.Exists)
                    {
                        File.Delete(Server.MapPath("/PatientDocuments/DoctorImages/") + files);
                    }
                }
                PatientImage.ImageUrl = "/Images/Signature.jpg";
            }
            else
            {
                objDir.Create();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RemoveImage_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if ((ddlDoctorName.SelectedIndex == 0))
            {
                // DeleteFiles();
                PatientImage.ImageUrl = "/Images/patient.jpg";
            }
            else
            {
                hshTable = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshTable.Add("@EmployeeId", bc.ParseQ(ddlDoctorName.SelectedValue.ToString().Trim()));
                String strSQL = "Update EmployeeSignatureImage set Active=0 where EmployeeId = @EmployeeId and Active = 1";
                objDl.ExecuteNonQuery(CommandType.Text, strSQL, hshTable);
                PatientImage.ImageUrl = "/Images/Signature.jpg";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void SaveImage()
    {
        try
        {
            StringBuilder strSQL = new StringBuilder();
            byte[] byteImageData;
            String FileName = "";
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("/PatientDocuments/DoctorImages/"));
            FileInfo[] fi_array = dir.GetFiles();
            //FileName = fi_array[0].ToString();
            FileName = ViewState["FileName"].ToString();
            String FilePath = Server.MapPath("/PatientDocuments/DoctorImages/") + FileName.ToString();
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] image = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();

            byteImageData = image;
            // DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            //Hashtable hshtableout = new Hashtable();
            //Hashtable hshtablein = new Hashtable();

            //hshtablein.Add("@EmployeeId", bc.ParseQ(ddlDoctorName.SelectedValue.ToString().Trim()));
            //hshtablein.Add("@Image", DBNull.Value);
            //hshtablein.Add("@ImageType", FileName);
            //hshtablein.Add("@EncodedBy", common.myInt(Session["UserID"]));
            //hshtablein.Add("@inyHospitalLocationId", Convert.ToInt32(Session["HospitalLocationID"]));
            //hshtableout.Add("@chvErrorStatus", SqlDbType.VarChar);

            //hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveProviderSignature", hshtablein, hshtableout);


            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmdTemp;
            SqlParameter prm1, prm2, prm3, prm4, prm5, prm6;

            strSQL.Append("Exec UspEMRSaveProviderSignature @EmployeeId,@Image,@ImageType,@EncodedBy,@inyHospitalLocationId,@chvErrorStatus");
            cmdTemp = new SqlCommand(strSQL.ToString(), con);
            cmdTemp.CommandType = CommandType.Text;

            prm1 = new SqlParameter();
            prm1.ParameterName = "@EmployeeId";
            prm1.Value = bc.ParseQ(ddlDoctorName.SelectedValue.ToString().Trim());
            prm1.SqlDbType = SqlDbType.Int;
            prm1.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm1);

            prm2 = new SqlParameter();
            prm2.ParameterName = "@Image";
            prm2.Value = byteImageData;
            prm2.SqlDbType = SqlDbType.Image;
            prm2.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm2);

            prm3 = new SqlParameter();
            prm3.ParameterName = "@ImageType";
            prm3.Value = FileName;
            prm3.SqlDbType = SqlDbType.VarChar;
            prm3.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm3);

            prm4 = new SqlParameter();
            prm4.ParameterName = "@EncodedBy";
            prm4.Value = Session["UserID"];
            prm4.SqlDbType = SqlDbType.Int;
            prm4.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm4);

            prm5 = new SqlParameter();
            prm5.ParameterName = "@inyHospitalLocationId";
            prm5.Value = Convert.ToInt32(Session["HospitalLocationID"]);
            prm5.SqlDbType = SqlDbType.Int;
            prm5.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm5);

            prm6 = new SqlParameter();
            prm6.ParameterName = "@chvErrorStatus";
            prm6.Value = SqlDbType.VarChar;
            prm6.SqlDbType = SqlDbType.VarChar;
            prm6.Direction = ParameterDirection.Output;
            cmdTemp.Parameters.Add(prm6);

            con.Open();
            cmdTemp.ExecuteNonQuery();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkEmployee_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DoctorId"] != null)
            {
                Response.Redirect("/MPages/employee.aspx?EmpNo=" + ViewState["DoctorId"].ToString().Trim(), false);
            }
            else
            {
                Response.Redirect("/MPages/employee.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkAppointmentTemplate_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DoctorId"] != null)
            {
                Response.Redirect("/mpages/providertimings.aspx?EmpNo=" + ViewState["DoctorId"].ToString().Trim(), false);
            }
            else
            {
                Response.Redirect("/mpages/providertimings.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkProviderProfile_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DoctorId"] != null)
            {
                Response.Redirect("/MPages/DoctorProfileMaster.aspx?EmpNo=" + ViewState["DoctorId"].ToString().Trim(), false);
            }
            else
            {
                Response.Redirect("/MPages/DoctorProfileMaster.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ibtnSpecilalization_onClick(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/PRegistration/Status.aspx?CtrlDesp=Specialization";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 550;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "BindCombo";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnFillCombo_Click(object sender, EventArgs e)
    {
        try
        {
            populateSpecialisation();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkEmployeeLookup_OnClick(object sebder, EventArgs e)
    {
        Response.Redirect("~/mpages/EmployeeLockUp.aspx", false);
    }
    protected void lnkClassification_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("/MPages/EmployeeClassification.aspx?EmpId=" + common.myInt(ViewState["emp"]), false);
    } 

    protected void ibtnpreviousnotes_Click(object sender, EventArgs e)
    {
        if (common.myStr(ddlDoctorName.SelectedValue) != "")
        {
            RadWindowForNew.NavigateUrl = "~/MPages/DoctorNotesDetail.aspx?&Doctorid=" + common.myStr(ddlDoctorName.SelectedValue) + "&CF=DoctorNotes";
            RadWindowForNew.Height = 450;
            RadWindowForNew.Width = 550;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "";
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {

        }
    }
}
