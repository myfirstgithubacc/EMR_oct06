using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Configuration;
using System.Net;
using System.Drawing;

public partial class EMR_Medication_ConsumableOrder : System.Web.UI.Page
{
	private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
	private string reportServer = System.Configuration.ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
	private string reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSFolder"];
	private string SysUser = System.Configuration.ConfigurationManager.AppSettings["SysUser"];
	private string SysPassword = System.Configuration.ConfigurationManager.AppSettings["SysPassword"];
	private string SysDomain = System.Configuration.ConfigurationManager.AppSettings["SysDomain"];

	clsExceptionLog objException = new clsExceptionLog();
	BaseC.clsEMR objEMR;
	BaseC.clsPharmacy objPharmacy;

	private const int ItemsPerRequest = 50;
	StringBuilder strXML;
	ArrayList coll;
	int count = 0;

	private Hashtable hstInput;

	StringBuilder sb = new StringBuilder();
	string Fonts = "";

	string FlagShowPreviousStoreInDrugIndent = string.Empty;
	private enum enumColumns : byte
	{
		Sno = 0,
		Brand = 1,
		IndentType = 2,
		TotalQty = 3,
		Unit = 4,
		PrescriptionDetail = 5,
		MonographCIMS = 6,
		InteractionCIMS = 7,
		DHInteractionCIMS = 8,
		MonographVIDAL = 9,
		InteractionVIDAL = 10,
		DHInteractionVIDAL = 11,
		Edit = 12,
		Delete = 13
	}
	protected void Page_PreInit(object sender, System.EventArgs e)
	{
		Page.Theme = "DefaultControls";
		if (common.myStr(Request.QueryString["PT"]) == "IPEMR")
		{
			this.MasterPageFile = "~/Include/Master/EMRMaster.master";
		}
		else if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
		{
			this.MasterPageFile = "~/Include/Master/BlankMaster.master";
		}
		if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
		{
			Page.MasterPageFile = "/Include/Master/BlankMaster.master";
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			Session["PendingConsumableIndentId"] = null;

			ViewState["OPIP"] = Session["OPIP"];
			ViewState["DoctorId"] = Session["DoctorId"];
			Session["DrugOrderDuplicateCheck"] = "0";

			if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
			{
				ViewState["Regno"] = common.myStr(Request.QueryString["Regno"]);
				ViewState["Encno"] = common.myStr(Request.QueryString["Encno"]);
				ViewState["RegId"] = common.myStr(Request.QueryString["RegId"]);
				ViewState["EncId"] = common.myStr(Request.QueryString["EncId"]);

				if (common.myLen(Request.QueryString["DoctorId"]) > 0)
				{
					ViewState["DoctorId"] = common.myInt(Request.QueryString["DoctorId"]);
				}

				if (common.myLen(Request.QueryString["OPIP"]) > 0)
				{
					ViewState["OPIP"] = common.myStr(Request.QueryString["OPIP"]);
				}

				ViewState["IndentId"] = common.myStr(Request.QueryString["IndentId"]);
			}
			else
			{
				ViewState["Regno"] = common.myStr(Session["RegistrationNo"]);
				ViewState["Encno"] = common.myStr(Session["EncounterNo"]);
				ViewState["RegId"] = common.myStr(Session["RegistrationId"]);
				ViewState["EncId"] = common.myStr(Session["EncounterId"]);
			}

			if (common.myStr(Request.QueryString["PT"]) == "IPEMR")
			{
				ViewState["Regno"] = common.myStr(Session["RegistrationNo"]);
				ViewState["Encno"] = common.myStr(Session["EncounterNo"]);
				ViewState["RegId"] = common.myStr(Session["RegistrationId"]);
				ViewState["EncId"] = common.myStr(Session["EncounterId"]);

				btnclose.Visible = false;
			}

			if (common.myInt(ViewState["EncId"]) == 0)
			{
				Response.Redirect("/default.aspx?RegNo=0", false);
			}

			if (common.myInt(ViewState["IndentId"]) > 0)
			{
				lnkBtnPendingConsumableIndent.Visible = false;
				divClosedByNurse.Visible = true;
			}

			hdnStoreId.Value = "0";
			hdnGenericId.Value = "0";
			hdnItemId.Value = "0";

			lblMessage.Text = "&nbsp;";
			ViewState["GridDataItem"] = null;

			btnPrint.Visible = (ViewState["OPIP"].ToString() == "O");

			lblTitle.Text = "Drug/Consumable Order";

			if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO")
			{
				btnPreviousMedications.Text = "Previous Order";
				lblDrugDetail.Text = "Consumable Indent";
				lblPrescription.Text = "Selected Indent";
				lblTitle.Text = "Consumable Order";
				//ddlStore.Enabled = false;
			}

			bindControl();

			ViewState["DrugHealthInteractionColorSet"] = System.Drawing.Color.FromName("#82AB76");
			ViewState["DrugAllergyColorSet"] = System.Drawing.Color.FromName("#82CAFA");


			BindBlankItemGrid();

			if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
			{
				btnSave.Visible = false;
			}
			if (common.myStr(Request.QueryString["POPUP"]) == "POPUP")
			{
				btnclose.Visible = true;
			}
			ddlBrand.Focus();

			bindPreviousIndent();

			ViewState["IsSelectAdvisingDoctorInConsumableOrder"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsSelectAdvisingDoctorInConsumableOrder", sConString);


			if (ViewState["IsSelectAdvisingDoctorInConsumableOrder"].Equals("Y"))
			{
				dvAdvisingDoctor.Visible = true;
				BindAdviserDoctor();
			}
			System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

			collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
					"CostlyItemSubCategoryId", sConString);
			if (collHospitalSetupValues.ContainsKey("CostlyItemSubCategoryId"))
				hdnHospitalSetupCostlyItemSubCategoryId.Value = collHospitalSetupValues["CostlyItemSubCategoryId"];

			if (common.myInt(hdnHospitalSetupCostlyItemSubCategoryId.Value).Equals(0))
			{
				tblLegend_ItemList.Visible = false;
			}

			if (common.myBool(Request.QueryString["IsConsignment"]))
			{
				lnkBtnPendingConsumableIndent.Visible = false;
				tblLegend_ItemList.Visible = false;
				lblTitle.Text = "Consignment Order";
				lblInfoBrand.Text = "Consignment";
				btnSurgicalKit.Visible = false;
				ddlAdvisingDoctor.SelectedValue = common.myStr(Request.QueryString["DoctorId"]);
				chkCustomMedication.Visible = true;


				DataTable dtSurgeryDetails = new DataTable();
				BaseC.clsOTBooking objOTBooking = new BaseC.clsOTBooking(sConString);
				dtSurgeryDetails = objOTBooking.GetSurgeryDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["EncounterId"]), common.myInt(Request.QueryString["OTBookingId"]));

				if (common.myInt(dtSurgeryDetails.Rows.Count) > 0)
				{
					lblSurgeonName.Text = common.myStr(dtSurgeryDetails.Rows[0]["SurgeonName"]);
					lblSurgeryname.Text = common.myStr(dtSurgeryDetails.Rows[0]["Surgeryname"]);
				}
				dtSurgeryDetails.Dispose();
				objOTBooking = null;


			}
			if (common.myBool(Request.QueryString["IsConsignment"]))
			{
				Chktoprinter.Visible = true;
				Chktoprinter.Checked = true;
			}
		}
	}


	void bindPatientProfileItemDetails()
	{
		try
		{
			DataSet dsItem = new DataSet();
			BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
			dsItem = objPharmacy.getPatientProfileItemDetails(common.myInt(ddlProfileItem.SelectedValue), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]));

			addItemFromKit(dsItem.Tables[0]);
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			clsExceptionLog objException = new clsExceptionLog();
			objException.HandleException(Ex);
		}
	}

	private void bindProfileItem()
	{
		try
		{
			BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
			DataSet ds = objPharmacy.getProfileItems(common.myInt(Session["HospitalLocationID"]));

			ddlProfileItem.DataSource = ds.Tables[0];
			ddlProfileItem.DataTextField = "ItemName";
			ddlProfileItem.DataValueField = "ItemId";
			ddlProfileItem.DataBind();
			ddlProfileItem.Items.Insert(0, new RadComboBoxItem("", "0"));
			ddlProfileItem.SelectedIndex = 0;
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			clsExceptionLog objException = new clsExceptionLog();
			objException.HandleException(Ex);
		}
	}

	protected void btnSurgicalKit_OnClick(object sender, EventArgs e)
	{
		try
		{
			//txtNetAmount.Text = Request[txtNetAmount.UniqueID];
			//txtLAmt.Text = Request[txtLAmt.UniqueID];
			//txtRounding.Text = Request[txtRounding.UniqueID];
			//txtReceived.Text = Request[txtReceived.UniqueID];

			dvConfirmProfileItem.Visible = false;
			//if (common.myStr(txtPatientName.Text).Trim() == "")
			//{
			//    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			//    lblMessage.Text = "Patient not selected ! ";
			//    return;
			//}
			bindProfileItem();
			dvConfirmProfileItem.Visible = true;
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			clsExceptionLog objException = new clsExceptionLog();
			objException.HandleException(Ex);
		}

	}

	protected void btnOkProfileItem_OnClick(object sender, EventArgs e)
	{
		bindPatientProfileItemDetails();
		dvConfirmProfileItem.Visible = false;
	}

	protected void btnCancelProfileItem_OnClick(object sender, EventArgs e)
	{
		dvConfirmProfileItem.Visible = false;
	}

	private void bindControl()
	{
		DataSet dsRoute = new DataSet();
		DataSet ds = new DataSet();
		DataTable tbl = new DataTable();
		DataView dv = new DataView();
		try
		{
			objPharmacy = new BaseC.clsPharmacy(sConString);
			BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

			/*************** Store ***************/

			bindDdlstore();
			/*************** Advising Doctor ***************/


			ds = new DataSet();
			ds = objEMR.GetIndentType(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
			if (common.myBool(Request.QueryString["IsConsignment"]))
			{

				DataView dvConsignment = new DataView(ds.Tables[0]);
				dvConsignment.RowFilter = "IndentCode='CONOD' ";
				ddlIndentType.DataSource = dvConsignment.ToTable();
				ddlIndentType.DataValueField = "Id";
				ddlIndentType.DataTextField = "IndentType";
				ddlIndentType.DataBind();
				ddlIndentType.Enabled = false;
			}
			else
			{
				ddlIndentType.DataSource = ds.Tables[0];
				ddlIndentType.DataValueField = "Id";
				ddlIndentType.DataTextField = "IndentType";
				ddlIndentType.DataBind();
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
			ds.Dispose();
			dv.Dispose();
			dsRoute.Dispose();
			tbl.Dispose();
		}
	}
	protected void btnSave_Onclick(object sender, EventArgs e)
	{
		try
		{


			if (common.myInt(ViewState["IndentId"]) > 0)
			{
				if (rdoClosedByNurse.SelectedIndex < 0)
				{
					lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

					lblMessage.Text = "Close indent not selected!";
					return;
				}

				SavePendingDrugIndent();
				return;
			}

			DataSet dsXml;
			DataTable dt;
			strXML = new StringBuilder();
			StringBuilder strXML1 = new StringBuilder();
			BaseC.Patient patient = new BaseC.Patient(sConString);
			coll = new ArrayList();
			ArrayList coll1 = new ArrayList();
			BaseC.WardManagement ward = new BaseC.WardManagement();
			DataSet ds = new DataSet();

			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

			if (common.myInt(ViewState["EncId"]) == 0)
			{
				lblMessage.Text = "Patient has no appointment !";
				return;
			}

			double sQuantity = 0;
			if (ViewState["OPIP"] != null && (ViewState["OPIP"].ToString() == "O" || ViewState["OPIP"].ToString() == "E"))
			{
				#region OP Drug
				foreach (GridViewRow dataItem in gvItem.Rows)
				{
					HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
					HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
					TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

					HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
					Label lblItemName = (Label)dataItem.FindControl("lblItemName");
					HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");

					if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
					{
						hdnIndentId.Value = "0";
					}

					if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
						&& common.myDbl(txtTotalQty.Text).Equals(0.00))
					{
						Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
						return;
					}

					if (hdnIndentId.Value == "0" || hdnIndentId.Value == "")
					{
						HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
						HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
						HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
						HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");

						Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

						coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int
						if (common.myInt(hdnItemId.Value) != 0)
						{
							coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
							coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
						}
						else
						{
							coll1.Add(DBNull.Value);//ItemId INT,
							coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
						}
						coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
						coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
						coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT

						dsXml = new DataSet();
						string xmlSchema = "";
						dt = new DataTable();
						if (hdnXMLData.Value == "")
						{
							Alert.ShowAjaxMsg("Please add drug to save", Page);
							return;
						}
						xmlSchema = common.myStr(hdnXMLData.Value);
						StringReader sr = new StringReader(xmlSchema);

						dsXml.ReadXml(sr);
						DataView dv = new DataView(dsXml.Tables[0]);
						dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
						dt = dv.ToTable();
						if (dt.Rows.Count > 0)
						{
							RadDatePicker dtStartTime = new RadDatePicker();
							RadDatePicker dtEndTime = new RadDatePicker();
							string sStartDate = "", sEndDate = "";
							for (int i = 0; i < dt.Rows.Count; i++)
							{
								int iFrequencyId = 0;
								string sDose = string.Empty;
								string sDuration = string.Empty;
								string sType = string.Empty;
								if (!dt.Columns.Contains("UnitId"))
								{
									dt.Columns.Add("UnitId", typeof(int));

								}
								int iUnit = 0;

								sStartDate = dt.Rows[i]["StartDate"].ToString();

								sEndDate = dt.Rows[i]["EndDate"].ToString();
								//    
								string sInstructions = string.Empty;
								int iReferanceItemId = 0;
								int iFoodRelationshipID = 0;
								int iDoseTypeId = 0;
								coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
								coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
								coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
								coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
								coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
								coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
								coll.Add(common.myInt(iUnit));//UNITID INT,
								coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
								coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
								coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,
								strXML.Append(common.setXmlTable(ref coll));
							}
							coll1.Add(sStartDate != "" ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
							coll1.Add(sEndDate != "" ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,



							coll1.Add(common.myDec(txtTotalQty.Text));//TotalQty INT
							coll1.Add(DBNull.Value);//ICD CODE VARCHAR
							coll1.Add(common.myInt(0));//Refill INT
							coll1.Add(common.myBool(0));//Is Override BIT
							coll1.Add(DBNull.Value);//OverrideComments VARCHAR
							coll1.Add(DBNull.Value);//DrugAllergyScreeningResult VARCHAR
							coll1.Add(common.myInt(424));//PrescriptionModeId INT
							coll1.Add(hdnNotToPharmcy.Value);//NotToPharmcy BIT
							coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR

							strXML1.Append(common.setXmlTable(ref coll1));
						}
					}
				}
				#endregion
			}
			else
			{
				#region IP Drug
				foreach (GridViewRow dataItem in gvItem.Rows)
				{
					HiddenField hdnGenericId = (HiddenField)dataItem.FindControl("hdnGenericId");
					HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
					TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

					HiddenField hdnIndentId = (HiddenField)dataItem.FindControl("hdnIndentId");
					Label lblItemName = (Label)dataItem.FindControl("lblItemName");
					HiddenField hdnNotToPharmcy = (HiddenField)dataItem.FindControl("hdnNotToPharmcy");

					if (ViewState["Stop"] != null && Convert.ToBoolean(ViewState["Stop"]) == true)
					{
						hdnIndentId.Value = "0";
					}

					if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
						&& common.myDbl(txtTotalQty.Text).Equals(0.00))
					{
						Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
						return;
					}

					if (hdnIndentId.Value == "0" || hdnIndentId.Value == "")
					{
						HiddenField hdnFormulationId = (HiddenField)dataItem.FindControl("hdnFormulationId");
						HiddenField hdnRouteId = (HiddenField)dataItem.FindControl("hdnRouteId");
						HiddenField hdnStrengthId = (HiddenField)dataItem.FindControl("hdnStrengthId");
						HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
						Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

						#region TotalQty Roundoff Calucation

						sQuantity = Convert.ToDouble(txtTotalQty.Text);

						#endregion

						coll1.Add(common.myInt(hdnIndentId.Value));//IndentId INT,

						if (common.myInt(hdnItemId.Value) != 0)
						{
							coll1.Add(common.myInt(hdnItemId.Value));//ItemId INT,
							coll1.Add(DBNull.Value);//CustomMedication varchar(1000)
						}
						else
						{
							coll1.Add(DBNull.Value);//ItemId INT,
							coll1.Add(lblItemName.Text);//CustomMedication varchar(1000)
						}

						coll1.Add(common.myInt(hdnGenericId.Value));//GenericId int
						coll1.Add(common.myInt(hdnFormulationId.Value));//FormulationId int
						coll1.Add(common.myInt(hdnRouteId.Value));//RouteId INT
						coll1.Add(common.myInt(hdnStrengthId.Value));//StrengthId INT                                                                     
						coll1.Add(common.myDec(sQuantity));//TotalQty INT

						dsXml = new DataSet();
						string xmlSchema = "";
						dt = new DataTable();
						if (hdnXMLData.Value == "")
						{
							Alert.ShowAjaxMsg("Please add drug to save", Page);
							return;
						}
						xmlSchema = common.myStr(hdnXMLData.Value);
						StringReader sr = new StringReader(xmlSchema);

						dsXml.ReadXml(sr);
						DataView dv = new DataView(dsXml.Tables[0]);
						dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);
						dt = dv.ToTable();
						if (dt.Rows.Count > 0)
						{
							RadDatePicker dtStartTime = new RadDatePicker();
							RadDatePicker dtEndTime = new RadDatePicker();
							string sStartDate = "", sEndDate = "";
							for (int i = 0; i < dt.Rows.Count; i++)
							{
								int iFrequencyId = 0;
								string sDose = string.Empty;
								// string sFrequency = dt.Rows[i]["Frequency"].ToString();
								string sDuration = string.Empty;
								string sType = string.Empty;
								if (!dt.Columns.Contains("UnitId"))
								{
									dt.Columns.Add("UnitId", typeof(int));
								}

								int iUnit = 0;

								sStartDate = dt.Rows[i]["StartDate"].ToString();

								sEndDate = dt.Rows[i]["EndDate"].ToString();

								string sInstructions = string.Empty;
								int iReferanceItemId = 0;
								int iFoodRelationshipID = 0;
								int iDoseTypeId = 0;
								coll.Add(common.myInt(hdnItemId.Value));//FrequencyId TINYINT, 
								coll.Add(common.myInt(iFrequencyId));//FrequencyId TINYINT,
								coll.Add(common.myDec(sDose));//Dose DECIMAL(10,3),
								coll.Add(common.myStr(sDuration));//Duration VARCHAR(20)
								coll.Add(common.myStr(sType));//DURATION TYPE CHAR(1),
								coll.Add(sInstructions);//INSTRUCTIONID VARCHAR(1000),
								coll.Add(common.myInt(iUnit));//UNITID INT,
								coll.Add(common.myInt(iReferanceItemId));//ReferanceItem INT,
								coll.Add(common.myInt(iFoodRelationshipID));//FoodRelationship INT,
								coll.Add(common.myInt(iDoseTypeId));//iDoseTypeId INT,
								strXML.Append(common.setXmlTable(ref coll));
							}

							coll1.Add(sStartDate != "" ? common.myDate(sStartDate).ToString("yyyy-MM-dd") : null);//StartDate SMALLDATETIME,
							coll1.Add(sEndDate != "" ? common.myDate(sEndDate).ToString("yyyy-MM-dd") : null);//EndDate SMALLDATETIME,
							coll1.Add(0);//NotToPharmcy BIT
							coll1.Add(string.Empty);//OverrideComments
							coll1.Add(string.Empty);//OverrideCommentsDrugToDrug
							coll1.Add(string.Empty);//OverrideCommentsDrugHealth
							coll1.Add(lblPrescriptionDetail.Text);//Prescription detail VARCHAR(1000)
							coll1.Add(string.Empty);//StrengthValue
							coll1.Add(common.myDec(txtTotalQty.Text));//Prescription detail VARCHAR(1000)


							strXML1.Append(common.setXmlTable(ref coll1));
						}
						else
						{
							return;
						}

					}
				}
				#endregion
			}

			if (strXML.ToString() == string.Empty)
			{
				lblMessage.Text = "Please add medicine before saving !";
				return;
			}
			bool isConsumable = common.myStr(Request.QueryString["DRUGORDERCODE"]) == "CO" ? true : false;
			Hashtable hshOutput = new Hashtable();
			if (common.myStr(Session["DrugOrderDuplicateCheck"]) == "0")
			{
				int AdvisingDoctorDoctorId = common.myInt(ViewState["DoctorId"]);
				if (common.myStr(ViewState["IsSelectAdvisingDoctorInConsumableOrder"]).Equals("Y"))
				{
					AdvisingDoctorDoctorId = common.myInt(ddlAdvisingDoctor.SelectedValue);
				}

				if (common.myStr(ViewState["OPIP"]).Equals("I"))
				{
					BaseC.WardManagement objwd = new BaseC.WardManagement();
					bool ApprovalRequired = false;

					//hshOutput = objwd.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
					//                common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue),
					//                common.myInt(ViewState["DoctorId"]), strXML1.ToString(), strXML.ToString(), "",
					//                Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0, common.myInt(Session["UserId"]),
					//                isConsumable, string.Empty, string.Empty, 0, common.myStr(Request.QueryString["LOCATION"]).Equals("WARD"), ApprovalRequired,
					//                false, "", false, string.Empty, string.Empty, 0, string.Empty, 0);


					hshOutput = objwd.SaveEMRMedicine(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
									common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]), common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue),
									AdvisingDoctorDoctorId, strXML1.ToString(), strXML.ToString(), "",
									Request.QueryString["DRUGORDERTYPE"] != null ? common.myInt(Request.QueryString["DRUGORDERTYPE"]) : 0, common.myInt(Session["UserId"]),
									isConsumable, string.Empty, string.Empty, 0, common.myStr(Request.QueryString["LOCATION"]).Equals("WARD"), ApprovalRequired,
									false, "", false, string.Empty, string.Empty, 0, string.Empty, 0, 0, common.myInt(Request.QueryString["OTBookingId"]));
				}
				else
				{
					objEMR = new BaseC.clsEMR(sConString);
					//hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
					//        common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
					//        common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue), common.myInt(ViewState["DoctorId"]),
					//        0, 0, strXML1.ToString(), strXML.ToString(), "", common.myInt(Session["UserId"]), "", isConsumable, string.Empty);

					hshOutput = objEMR.SaveEMRMedicineOP(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
								common.myInt(ViewState["RegId"]), common.myInt(ViewState["EncId"]),
								common.myInt(ddlIndentType.SelectedValue), common.myInt(ddlStore.SelectedValue), AdvisingDoctorDoctorId,
								0, 0, strXML1.ToString(), strXML.ToString(), "", common.myInt(Session["UserId"]), "", isConsumable, string.Empty);
				}

				if ((hshOutput["@chvErrorStatus"].ToString().Contains("UPDATE") || hshOutput["@chvErrorStatus"].ToString().Contains("Saved"))
					&& !hshOutput["@chvErrorStatus"].ToString().Contains("USP"))
				{
					lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

					Session["DrugOrderDuplicateCheck"] = "1";
					hdnGenericId.Value = "0";
					hdnGenericName.Value = "";
					hdnItemId.Value = "0";
					hdnItemName.Value = "";
					ddlIndentType.SelectedValue = "0";

					hdnCIMSItemId.Value = "";
					hdnCIMSType.Value = "";
					hdnVIDALItemId.Value = "";
					txtCustomMedication.Text = string.Empty;

					if (common.myInt(hshOutput["@intPrescriptionId"]) > 0)
					{
						if (Chktoprinter.Checked && common.myBool(Request.QueryString["IsConsignment"]))
						{

							string DirectPRintingURL = string.Empty;

							System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

							collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
									"DirectPRintingURL", sConString);

							if (collHospitalSetupValues.ContainsKey("DirectPRintingURL"))
								//ViewState["DirectPRintingURL"] = collHospitalSetupValues["DirectPRintingURL"];
								DirectPRintingURL = collHospitalSetupValues["DirectPRintingURL"];

							System.Text.StringBuilder str = new System.Text.StringBuilder();

							BaseC.User valUser = new BaseC.User(sConString);

							clsPrinter _printer = null;
							string sUserName = string.Empty;
							try
							{
								if (common.myStr(DirectPRintingURL) != "")
								{
									string[] hostName = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
									_printer = new clsPrinter(sConString, "ConsignmentIndent", 0, common.myStr(hostName[0].ToUpper().Trim()));

									sUserName = common.myStr(Session["EmployeeName"]).Trim();

									str.Clear();
									str.Append("UserName/");
									str.Append(sUserName);
									str.Append("!");

									str.Append("inyHospitalLocationId/");
									str.Append(common.myInt(Session["HospitalLocationID"]).ToString());
									str.Append("!");

									str.Append("intFacilityId/");
									str.Append(common.myInt(Session["FacilityID"]).ToString());
									str.Append("!");

									str.Append("intEncounterId/");
									str.Append(common.myInt(ViewState["EncId"]));
									str.Append("!");

									str.Append("intIndentId/");
									str.Append(common.myInt(hshOutput["@intPrescriptionId"]).ToString());
									str.Append("!");

									str.Append("UHID/");
									str.Append((string)HttpContext.GetGlobalResourceObject("PRegistration", "UHID"));
									str.Append("!");

									str.Append("intOTBookingID/");
									str.Append(common.myInt(Request.QueryString["OTBookingId"]));

									//string Str = DirectPRintingURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/" + _printer.ReportName + "$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + _printer.PageMargin;
									string Str = DirectPRintingURL + "http://" + reportServer + "/ReportServer$/" + reportFolder + "/ConsignmentIndent$" + SysUser + "$" + SysPassword + "$" + SysDomain + "$" + str.ToString() + "$" + _printer.PageMargin;
									ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", "window.location.href='" + Str + "';", true);

									return;
								}
								return;
							}
							catch (Exception ex)
							{
								clsExceptionLog objException = new clsExceptionLog();
								objException.HandleException(ex);
								objException = null;
							}
							finally
							{
								valUser = null; ;
								_printer = null;
							}
						}
						else if (!Chktoprinter.Checked && common.myBool(Request.QueryString["IsConsignment"]))
						{
							RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=CONOD&EncId=" + common.myInt(ViewState["EncId"]) + "&PId=" + common.myInt(hshOutput["@intPrescriptionId"]).ToString() + "&OTBookingId=" + common.myInt(Request.QueryString["OTBookingId"]);
							RadWindow1.Top = 10;
							RadWindow1.Left = 10;
							RadWindow1.Height = 600;
							RadWindow1.Width = 1000;
							RadWindow1.Top = 40;
							RadWindow1.Left = 100;
							RadWindow1.OnClientClose = string.Empty;
							RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
							RadWindow1.Modal = true;
							RadWindow1.VisibleStatusbar = false;
							RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
						}
					}
				}

				lblMessage.Text = common.myStr(hshOutput["@chvErrorStatus"]);

				if (common.myStr(lblMessage.Text).Contains("Saved"))
				{
                    BindBlankItemGrid();
                    ViewState["Item"] = null;
                    ViewState["Stop"] = null;
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
			Session["DrugOrderDuplicateCheck"] = "0";
		}
	}


	protected void SavePendingDrugIndent()
	{
		try
		{
			BaseC.WardManagement ward = new BaseC.WardManagement();
			StringBuilder strXML1 = new StringBuilder();
			ArrayList coll1 = new ArrayList();
			int intCount = 1;

			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

			if (common.myInt(ViewState["EncId"]) == 0)
			{
				lblMessage.Text = "Patient has no appointment !";
				return;
			}

			double sQuantity = 0;
			if (common.myStr(ViewState["OPIP"]).Equals("I"))
			{
				#region IP Drug
				foreach (GridViewRow dataItem in gvItem.Rows)
				{
					HiddenField hdnItemId = (HiddenField)dataItem.FindControl("hdnItemId");
					if (common.myInt(hdnItemId.Value) > 0)
					{
						TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");

						if (Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"] == "CO"
							&& common.myDbl(txtTotalQty.Text).Equals(0.00))
						{
							Alert.ShowAjaxMsg("Quantity should not be Zero!", Page);
							return;
						}

						Label lblPrescriptionDetail = (Label)dataItem.FindControl("lblPrescriptionDetail");

						sQuantity = Convert.ToDouble(txtTotalQty.Text);

						coll1.Add(common.myInt(hdnItemId.Value)); //ItemId INT,
						coll1.Add(common.myDec(sQuantity)); //TotalQty DECIMAL(10, 3),
						coll1.Add(common.myStr(lblPrescriptionDetail.Text)); //Instructions VARCHAR(2000),
						coll1.Add(intCount); //SequenceNo tinyint

						strXML1.Append(common.setXmlTable(ref coll1));
						intCount++;
					}
				}
				#endregion
			}

			//if (strXML1.ToString() == string.Empty)
			//{
			//    lblMessage.Text = "Please add medicine before saving !";
			//    return;
			//}

			string strMsg = string.Empty;
			if (common.myStr(Session["DrugOrderDuplicateCheck"]) == "0")
			{
				if (common.myStr(ViewState["OPIP"]).Equals("I"))
				{
					strMsg = ward.updateIPIndent(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["EncId"]),
									common.myInt(ViewState["IndentId"]), strXML1.ToString(), common.myBool(rdoClosedByNurse.SelectedValue), common.myInt(Session["UserId"]));
				}


				if ((strMsg.ToUpper().Contains("UPDATE") || strMsg.ToUpper().Contains("Saved"))
					&& !strMsg.ToUpper().Contains("USP"))
				{
					lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

					Session["DrugOrderDuplicateCheck"] = "1";
					hdnGenericId.Value = "0";
					hdnGenericName.Value = "";
					hdnItemId.Value = "0";
					hdnItemName.Value = "";
					ddlIndentType.SelectedValue = "0";

					hdnCIMSItemId.Value = "";
					hdnCIMSType.Value = "";
					hdnVIDALItemId.Value = "";

					bindPreviousIndent();
				}

				lblMessage.Text = common.myStr(strMsg);

				if (!common.myStr(lblMessage.Text).Contains("ALREADY PENDING"))
				{
					BindBlankItemGrid();
					ViewState["Item"] = null;
					ViewState["Stop"] = null;
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
			Session["DrugOrderDuplicateCheck"] = "0";
		}
	}

	void BindPatientHiddenDetails()
	{
		try
		{
			//if (Session["PatientDetailString"] != null)
			//{
			//    lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
			//}
		}
		catch (Exception ex)
		{
		}
	}

	private static string GetStatusMessage(int offset, int total)
	{
		if (total <= 0)
			return "No matches found...";

		return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
	}

	protected void ddlBrand_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
	{
		RadComboBox ddl = sender as RadComboBox;
		DataTable data = new DataTable();
		if (common.myInt(hdnStoreId.Value) == 0)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Store not selected !";
			return;
		}
		data = GetBrandData(e.Text, 0);
		int itemOffset = e.NumberOfItems;
		int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
		e.EndOfItems = endOffset == data.Rows.Count;

		for (int i = itemOffset; i < endOffset; i++)
		{
			RadComboBoxItem item = new RadComboBoxItem();
			item.Text = (string)data.Rows[i]["ItemSubCategoryShortName"] + " - " + (string)data.Rows[i]["ItemName"];
			item.Value = common.myStr(data.Rows[i]["ItemId"]);

			item.Attributes.Add("ClosingBalance", common.myStr(data.Rows[i]["ClosingBalance"]));
			item.Attributes.Add("CIMSItemId", common.myStr(data.Rows[i]["CIMSItemId"]));
			item.Attributes.Add("CIMSType", common.myStr(data.Rows[i]["CIMSType"]));
			item.Attributes.Add("VIDALItemId", common.myStr(data.Rows[i]["VIDALItemId"]));
			item.Attributes.Add("ItemSubCategoryId", common.myStr(data.Rows[i]["ItemSubCategoryId"]));

			this.ddlBrand.Items.Add(item);
			item.DataBind();
		}

		ViewState["ShowStockQtyInDrugConsumableorder"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "ShowStockQtyInDrugConsumableorder", sConString);

		if (common.myStr(ViewState["ShowStockQtyInDrugConsumableorder"]).Equals("Y"))
		{
			for (int j = itemOffset; j < endOffset; j++)
			{
				Label lblClosingBalanceItem = (Label)ddlBrand.Items[j].FindControl("lblClosingBalanceItem");
				lblClosingBalanceItem.Visible = true;
			}
		}
		else
		{
			for (int j = itemOffset; j < endOffset; j++)
			{
				Label lblClosingBalanceItem = (Label)ddlBrand.Items[j].FindControl("lblClosingBalanceItem");
				lblClosingBalanceItem.Visible = false;

			}
		}

		e.Message = GetStatusMessage(endOffset, data.Rows.Count);
		ddl = null;
		data.Dispose();
	}

	private DataTable GetBrandData(string text, int GenericId)
	{
		DataSet dsSearch = new DataSet();
		DataTable dt = new DataTable();

		objPharmacy = new BaseC.clsPharmacy(sConString);
		objEMR = new BaseC.clsEMR(sConString);

		int StoreId = common.myInt(hdnStoreId.Value); //common.myInt(Session["StoreId"]);
		int ItemId = 0;

		int itemBrandId = 0;
		int WithStockOnly = 0;

		int iOT = 3;
		int CompanyId = 0;
		string ItemSearchFor = "I";
		if (Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"].ToString() == "OT"
			&& Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"].ToString() == "CO")
		{
			iOT = 2;
		}
		else if (Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"].ToString() == "WARD"
			&& Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"].ToString() == "CO")
		{
			iOT = 2;
		}
		else
		{
			iOT = 1;
		}


		if (common.myDbl(ViewState["QtyBal"]) > 0
			   && common.myInt(Request.QueryString["ItemId"]) > 0)
		{
			ItemId = common.myInt(ViewState["ItemId"]);
		}


		if (dsSearch.Tables.Count > 0)
		{
			if (dsSearch.Tables[0].Rows.Count > 0)
			{
				dt = dsSearch.Tables[0];
			}
		}

		string IsPackagePatient = common.myStr(Session["IsPackagePatient"]);
		string IsPanelPatient = common.myStr(Session["IsPanelPatient"]);

		if (common.myStr(ViewState["OPIP"]).Equals("I"))
		{
			if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowZeroStockItemInIPPrescription", sConString).Equals("Y"))
			{
				WithStockOnly = 0;
			}
			else
			{
				WithStockOnly = 1;
			}
		}
		else
		{
			if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "AllowZeroStockItemInOPPrescription", sConString).Equals("Y"))
			{
				WithStockOnly = 0;
			}
			else
			{
				WithStockOnly = 1;
			}
		}

		if (!common.myInt(Request.QueryString["CompanyId"]).Equals(0))
		{
			CompanyId = common.myInt(Request.QueryString["CompanyId"]);
		}
		else
		{
			CompanyId = common.myInt(Session["SponsorIdPayorId"]);
		}

		if (common.myBool(Request.QueryString["IsConsignment"]))
		{
			ItemSearchFor = "C";
		}

		if (common.myStr(ViewState["OPIP"]).Equals("I"))
		{
			dsSearch = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId,
										  ItemId == 0 && ViewState["ItemId"] != null ? Convert.ToInt32(ViewState["ItemId"]) : ItemId,
										  itemBrandId, GenericId, common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]),
										  0, text.Replace("'", "''"), WithStockOnly, "", iOT, "IP",
										  common.myBool(IsPackagePatient), common.myBool(IsPanelPatient), common.myBool(Session["AllowPanelExcludedItems"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]), ItemSearchFor, CompanyId, 0);
		}
		else
		{
			dsSearch = objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId,
										  ItemId == 0 && ViewState["ItemId"] != null ? Convert.ToInt32(ViewState["ItemId"]) : ItemId,
										  itemBrandId, GenericId, common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]),
										  0, text.Replace("'", "''"), WithStockOnly, "", iOT, string.Empty,
										  common.myBool(IsPackagePatient), common.myBool(IsPanelPatient), common.myBool(Session["AllowPanelExcludedItems"]), common.myInt(Session["GroupID"]), common.myInt(Session["EmployeeId"]), ItemSearchFor, CompanyId, 0);
		}

		if (dsSearch.Tables.Count > 0)
		{
			if (dsSearch.Tables[0].Rows.Count > 0)
			{
				dt = dsSearch.Tables[0];
			}
		}
		return dt;
	}
	protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
	{
		hdnStoreId.Value = common.myInt(ddlStore.SelectedValue).ToString();
		ViewState["Item"] = null; //Added on 30122019
		BindBlankItemGrid();   //Added on 27122019
	}
	protected void gvItem_OnRowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			if (e.Row.RowType == DataControlRowType.Header)
			{
				if (Request.QueryString["LOCATION"] != null && Request.QueryString["DRUGORDERCODE"] != null
				   && Request.QueryString["DRUGORDERCODE"].ToString() == "CO" &&
				   (Request.QueryString["LOCATION"].ToString() == "OT" || Request.QueryString["LOCATION"].ToString() == "WARD"))
				{
					e.Row.Cells[3].Visible = true;
				}

				if (common.myBool(Request.QueryString["IsConsignment"]))
				{
					e.Row.Cells[1].Text = "Consignment Item";
				}
			}
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				int rowID = 0;
				rowID = common.myInt(e.Row.RowIndex) + 1;

				TextBox txtTotalQty = (TextBox)e.Row.FindControl("txtTotalQty");

				Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");
				HiddenField hdnItemId = (HiddenField)e.Row.FindControl("hdnItemId");
				HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");
				Label lblItemName = (Label)e.Row.FindControl("lblItemName");

				HiddenField hdnXMLData = (HiddenField)e.Row.FindControl("hdnXMLData");

				HiddenField hdnCustomMedication = (HiddenField)e.Row.FindControl("hdnCustomMedication");

				#region Set Costly Item Color
				HiddenField hndItemSubCategoryId = (HiddenField)e.Row.FindControl("hndItemSubCategoryId");
				if (!common.myBool(Request.QueryString["IsConsignment"]))
				{
					if (!common.myInt(hdnHospitalSetupCostlyItemSubCategoryId.Value).Equals(0))
					{
						if (common.myInt(hndItemSubCategoryId.Value) > 0)
						{
							e.Row.Cells[1].BackColor = Color.Yellow;
						}
					}
				}
				#endregion
				if (!common.myBool(Request.QueryString["IsConsignment"]))
				{
					txtTotalQty.Attributes.Add("onkeyup", "javascript:ChangeRowColor('" + txtTotalQty.ClientID + "','" + hdnClosingBalance.ClientID + "','" + rowID + "')");
				}
				if (ViewState["ItemDetail"] != null)
				{
					DataTable dt = (DataTable)ViewState["ItemDetail"];
					DataView dv = new DataView(dt);

					dv.RowFilter = "ISNULL(ItemId,0)=" + common.myInt(hdnItemId.Value);

					if (dv.ToTable().Rows.Count > 0)
					{
						BaseC.clsEMR emr = new BaseC.clsEMR(sConString);

						if (dv.ToTable().Rows.Count > 1)
						{
							DataTable tblPres = dt.Copy().Clone();
							if (e.Row.RowIndex <= dt.Rows.Count)
							{
								tblPres.Rows.Add(dt.Copy().Rows[e.Row.RowIndex].ItemArray);
							}
							//lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(tblPres);
						}
						else
						{
							// lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv.ToTable());
						}

						// dt.TableName = "ItemDetail";
						StringBuilder builder = new StringBuilder();// Create the StringWriter object with the StringBuilder object.

						StringWriter writer = new StringWriter(builder);// Write the schema into the StringWriter.
						dv.ToTable().WriteXml(writer, XmlWriteMode.IgnoreSchema);

						string xmlSchema = writer.ToString();
						hdnXMLData.Value = xmlSchema;
					}
					else
					{
						DataView dv1 = new DataView(dt);
						dv1.RowFilter = "ISNULL(IndentId,0)=" + common.myInt(hdnItemId.Value);
						BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
						if (dv1.ToTable().Rows.Count > 0)
						{
							lblPrescriptionDetail.Text = emr.GetPrescriptionDetailString(dv1.ToTable());

							dt.TableName = "ItemDetail";
							System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

							System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
							dt.WriteXml(writer, XmlWriteMode.IgnoreSchema);

							string xmlSchema = writer.ToString();
							hdnXMLData.Value = xmlSchema;
						}
						else
						{
							lblPrescriptionDetail.Text = "";
						}
					}

				}
				if (Request.QueryString["LOCATION"] != null && Request.QueryString["DRUGORDERCODE"] != null
					&& Request.QueryString["DRUGORDERCODE"].ToString() == "CO" &&
					(Request.QueryString["LOCATION"].ToString() == "OT" || Request.QueryString["LOCATION"].ToString() == "WARD"))
				{
					e.Row.Cells[3].Visible = true;
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

	protected void gvItem_OnRowCommand(object Sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName == "ItemDelete")
			{
				int ItemId = common.myInt(e.CommandArgument);

				GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
				int GenericId = common.myInt(((HiddenField)row.FindControl("hdnGenericId")).Value);
				int IndentId = common.myInt(((HiddenField)row.FindControl("hdnIndentId")).Value);
				HiddenField hdnCustomMedication = (HiddenField)row.FindControl("hdnCustomMedication");

				if (ViewState["ItemDetail"] != null && (ItemId > 0 || GenericId > 0) || common.myBool(hdnCustomMedication.Value))
				{
					DataTable tbl = (DataTable)ViewState["ItemDetail"];
					DataTable tbl1 = (DataTable)ViewState["Item"];
					DataView DV = tbl.Copy().DefaultView;
					DataView DV1 = tbl1.Copy().DefaultView;

					if (common.myBool(hdnCustomMedication.Value))
					{
						Label lblItemName = (Label)row.FindControl("lblItemName");
						DV.RowFilter = "CustomMedication=0 AND ISNULL(ItemName,0) <> '" + common.myStr(lblItemName.Text) + "'";
						DV1.RowFilter = "CustomMedication=0 AND ISNULL(ItemName,0) <> '" + common.myStr(lblItemName.Text) + "'";
						//DV.RowFilter = "UnAppPrescriptionId <> " + common.myInt(hdnUnAppPrescriptionId.Value);
						//DV1.RowFilter = "UnAppPrescriptionId <> " + common.myInt(hdnUnAppPrescriptionId.Value);
					}
					else
					{

						if (ItemId > 0)
						{
							DV.RowFilter = "ISNULL(ItemId,0) <> " + ItemId;// +" AND IndentId<>" + IndentId;
							DV1.RowFilter = "ISNULL(ItemId,0) <> " + ItemId;// +" AND IndentId<>" + IndentId;
						}
						else
						{
							DV.RowFilter = "ISNULL(GenericId, 0) <> " + GenericId;
							DV1.RowFilter = "ISNULL(GenericId, 0) <> " + GenericId;
						}
					}

					tbl = DV.ToTable();

					ViewState["GridDataItem"] = DV1.ToTable();
					ViewState["ItemDetail"] = tbl;
					ViewState["Item"] = DV1.ToTable();

					if (tbl.Rows.Count == 0)
					{
						DataRow DR = tbl.NewRow();
						tbl.Rows.Add(DR);
						tbl.AcceptChanges();
					}

					gvItem.DataSource = DV1.ToTable();
					gvItem.DataBind();

					if (DV1.ToTable().Rows.Count == 0)
					{
						BindBlankItemGrid();
					}

					ViewState["StopItemDetail"] = null;
					ViewState["Edit"] = null;
					ViewState["ItemId"] = 0;
					//setVisiblilityInteraction();
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

	protected void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{

			lblMessage.Text = "";
			RadWindow1.NavigateUrl = "/EMR/Medication/PrintPriscription.aspx?PT=P&EncId=" + common.myInt(ViewState["EncId"]);
			RadWindow1.Height = 650;
			RadWindow1.Width = 900;
			RadWindow1.Top = 10;
			RadWindow1.Left = 10;
			RadWindow1.Modal = true;
			RadWindow1.VisibleOnPageLoad = true;
			RadWindow1.VisibleStatusbar = false;
			RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			objException.HandleException(Ex);
		}
	}


	protected void btnPreviousMedications_Click(Object sender, EventArgs e)
	{
		try
		{
			//if (common.myBool(Request.QueryString["IsConsignment"]))
			//{
			//    RadWindow1.NavigateUrl = "/EMR/Medication/ConsumableOrderHistory.aspx?POPUP=POPUP&IsConsignment=true&OTBookingId=" + common.myInt(Request.QueryString["OTBookingId"]);
			//}
			//else
			//{
			//    RadWindow1.NavigateUrl = "/EMR/Medication/ConsumableOrderHistory.aspx?POPUP=POPUP";
			//}

			if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
			{
				RadWindow1.NavigateUrl = "/EMR/Medication/OPPrescriptionMainNew.aspx?Consumable=" + common.myBool(ViewState["Consumable"]);
			}
			else
			{
				RadWindow1.NavigateUrl = "/EMR/Medication/OPPrescriptionMainNew.aspx?Consumable=";
			}
			RadWindow1.Height = 570;
			RadWindow1.Width = 850;
			RadWindow1.Top = 10;
			RadWindow1.Left = 10;
			RadWindow1.OnClientClose = "OnClientClose";
			RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
			RadWindow1.Modal = true;
			RadWindow1.VisibleStatusbar = false;
			RadWindow1.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Maximize;
		}
		catch
		{
		}
	}

	private DataTable CreateItemTable()
	{
		DataTable dt = new DataTable();
		//dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("IndentNo", typeof(int));
		dt.Columns.Add("IndentDate", typeof(string));

		dt.Columns.Add("IndentTypeId", typeof(int));
		dt.Columns.Add("IndentType", typeof(string));
		dt.Columns.Add("GenericId", typeof(int));
		dt.Columns.Add("GenericName", typeof(string));
		dt.Columns.Add("ItemId", typeof(int));
		dt.Columns.Add("IndentId", typeof(int));
		dt.Columns.Add("ItemName", typeof(string));
		dt.Columns.Add("FormulationId", typeof(int));
		dt.Columns.Add("RouteId", typeof(int));
		dt.Columns.Add("StrengthId", typeof(int));
		dt.Columns.Add("CIMSItemId", typeof(int));
		dt.Columns.Add("CIMSType", typeof(string));
		dt.Columns.Add("VIDALItemId", typeof(int));
		dt.Columns.Add("Qty", typeof(decimal));
		dt.Columns.Add("PrescriptionDetail", typeof(string));
		dt.Columns.Add("ReferanceItemId", typeof(int));
		dt.Columns.Add("StartDate", typeof(string));
		dt.Columns.Add("EndDate", typeof(string));
		dt.Columns.Add("CustomMedication", typeof(bool));
		dt.Columns.Add("NotToPharmacy", typeof(bool));
		dt.Columns.Add("XMLData", typeof(String));
		dt.Columns.Add("IsInfusion", typeof(bool));
		dt.Columns.Add("UnitName", typeof(string));
		dt.Columns.Add("ItemSubCategoryId", typeof(int));
		return dt;

	}
	private DataTable CreateItemDetailTable()
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("IndentTypeId", typeof(int));
		dt.Columns.Add("IndentType", typeof(string));
		dt.Columns.Add("GenericId", typeof(int));
		dt.Columns.Add("GenericName", typeof(string));
		dt.Columns.Add("ItemId", typeof(int));
		dt.Columns.Add("IndentId", typeof(int));
		dt.Columns.Add("ItemName", typeof(string));
		dt.Columns.Add("FormulationId", typeof(int));
		dt.Columns.Add("RouteId", typeof(int));
		dt.Columns.Add("RouteName", typeof(string));
		dt.Columns.Add("StrengthId", typeof(int));
		dt.Columns.Add("CIMSItemId", typeof(int));
		dt.Columns.Add("CIMSType", typeof(string));
		dt.Columns.Add("VIDALItemId", typeof(int));
		dt.Columns.Add("Frequency", typeof(decimal));
		dt.Columns.Add("FrequencyName", typeof(string));
		dt.Columns.Add("FrequencyId", typeof(int));
		dt.Columns.Add("Duration", typeof(string));
		dt.Columns.Add("Type", typeof(string));
		dt.Columns.Add("DurationText", typeof(string));
		dt.Columns.Add("Dose", typeof(string));
		dt.Columns.Add("UnitId", typeof(int));
		dt.Columns.Add("UnitName", typeof(string));
		dt.Columns.Add("FoodRelationshipId", typeof(int));
		dt.Columns.Add("FoodRelationship", typeof(string));
		dt.Columns.Add("StartDate", typeof(string));
		dt.Columns.Add("EndDate", typeof(string));
		dt.Columns.Add("Qty", typeof(decimal));
		dt.Columns.Add("PrescriptionDetail", typeof(string));
		dt.Columns.Add("ReferanceItemId", typeof(int));
		dt.Columns.Add("ReferanceItemName", typeof(string));
		dt.Columns.Add("Instructions", typeof(string));
		dt.Columns.Add("DoseTypeId", typeof(int));
		dt.Columns.Add("DoseTypeName", typeof(string));
		dt.Columns.Add("CustomMedication", typeof(bool));
		dt.Columns.Add("NotToPharmacy", typeof(bool));
		dt.Columns.Add("XMLData", typeof(String));
		dt.Columns.Add("IsInfusion", typeof(bool));
		return dt;

	}
	private void BindBlankItemGrid()
	{
		DataTable dt = CreateItemTable();
		DataRow dr = dt.NewRow();
		dr["IndentTypeId"] = 0;
		dr["IndentType"] = "";
		dr["IndentNo"] = 0;
		dr["IndentDate"] = "";
		dr["GenericId"] = 0;
		dr["ItemId"] = 0;
		dr["IndentId"] = 0;
		dr["GenericName"] = "";
		dr["ItemName"] = "";
		dr["FormulationId"] = 0;
		dr["RouteId"] = 0;
		dr["StrengthId"] = 0;
		dr["CIMSItemId"] = 0;
		dr["CIMSType"] = "";
		dr["VIDALItemId"] = 0;
		dr["Qty"] = 0.00;
		dr["PrescriptionDetail"] = "";
		dr["ReferanceItemId"] = 0;

		dr["StartDate"] = "";
		dr["EndDate"] = "";
		dr["CustomMedication"] = 0;
		dr["XMLData"] = "";
		dr["NotToPharmacy"] = 0;
		dr["IsInfusion"] = false;
		dr["UnitName"] = string.Empty;
		dr["ItemSubCategoryId"] = 0;
		dt.Rows.Add(dr);
		dt.AcceptChanges();
		ViewState["ItemDetail"] = null;
		gvItem.DataSource = dt;
		gvItem.DataBind();
		ViewState["DataTableItem"] = dt;
	}
	private void addItemFromKit(DataTable objKitdt)
	{
		#region comment
		//BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
		//DataTable dtold = new DataTable();
		//DataTable dt1old = new DataTable();
		//DataTable dt = new DataTable();
		//DataTable dt1 = new DataTable();
		//DataTable dtNew = new DataTable();
		//DataSet ds = new DataSet();
		//try
		//{
		//    DataRow DR;
		//    DataRow DR1;
		//    decimal dQty = 0;
		//    int countRow = 0;
		//    if (ViewState["Item"] == null && ViewState["Edit"] == null)
		//    {
		//        dt = CreateItemTable();
		//        dt1 = CreateItemDetailTable();
		//    }
		//    else
		//    {
		//        dtold = (DataTable)ViewState["Item"];
		//        if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["Item"] != null)
		//        {
		//            DataView dv = new DataView(dtold);
		//            dv.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
		//            dt = dv.ToTable();
		//        }
		//        else
		//        {
		//            dt = (DataTable)ViewState["Item"];
		//        }
		//        if (dt.Rows.Count > 0)
		//        {
		//            if (ViewState["Edit"] == null)
		//            {
		//                foreach (GridViewRow dataItem in gvItem.Rows)
		//                {
		//                    TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
		//                    dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString("F2");
		//                    countRow++;
		//                    dt.AcceptChanges();
		//                }
		//            }
		//        }
		//        if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
		//        {
		//            dt1 = CreateItemDetailTable();
		//        }
		//        else
		//        {
		//            dt1old = (DataTable)ViewState["ItemDetail"];
		//            if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["ItemDetail"] != null)
		//            {
		//                DataView dv1 = new DataView(dt1old);
		//                dv1.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
		//                dt1 = dv1.ToTable();
		//            }
		//            else
		//            {
		//                dt1 = (DataTable)ViewState["ItemDetail"];
		//            }
		//        }
		//    }
		//    foreach (DataRow dr in objKitdt.Rows)
		//    {
		//        DataRow[] FindRow = dt.Select("itemid=" + common.myInt(dr["itemid"]) + "");

		//        if (FindRow.Length > 0)
		//        {
		//            //return;
		//            continue;
		//        }
		//        else
		//        {
		//            //foreach (GridViewRow row in gvItem.Rows)
		//            //{
		//            //    HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
		//            //    if (hdnItemId.Value == common.myStr(dr["itemid"]))
		//            //    {
		//            //        Alert.ShowAjaxMsg("Item already added", Page);
		//            //        return;
		//            //        //break; 
		//            //    }
		//            //    else
		//            //    {
		//            DR = dt.NewRow();
		//            DR["IndentTypeId"] = Convert.ToInt16(ddlIndentType.SelectedValue);
		//            DR["IndentType"] = ddlIndentType.SelectedValue == "" ? "" : ddlIndentType.Text;
		//            DR["IndentId"] = 0;
		//            DR["GenericId"] = 0;
		//            DR["ItemId"] = common.myInt(dr["itemid"]);
		//            DR["GenericName"] = "";
		//            DR["ItemName"] = common.myStr(dr["itemname"]);
		//            DR["RouteId"] = 0;
		//            DR["CustomMedication"] = 0;
		//            DR["NotToPharmacy"] = chkNotToPharmacy.Checked;
		//            DR["IsInfusion"] = hdnInfusion.Value == "1" ? true : false;
		//            DR["FormulationId"] = 0;
		//            ///Item Detail
		//            TextBox txtInstructions = new TextBox();
		//            RadComboBox ddlFoodRelation = new RadComboBox();
		//            //---------------------
		//            foreach (GridViewRow dataItem in gvItemDetails.Rows)
		//            {
		//                DR1 = dt1.NewRow();
		//                string sFrequency = "0";
		//                RadComboBox ddlFrequencyId = (RadComboBox)dataItem.FindControl("ddlFrequencyId");
		//                TextBox txtDose = (TextBox)dataItem.FindControl("txtDose");
		//                if (ddlFrequencyId.SelectedValue != "0")
		//                {
		//                    sFrequency = ddlFrequencyId.SelectedItem.Attributes["Frequency"].ToString();
		//                }

		//                TextBox txtDuration = (TextBox)dataItem.FindControl("txtDuration");

		//                RadComboBox ddlPeriodType = (RadComboBox)dataItem.FindControl("ddlPeriodType");

		//                RadComboBox ddlDoseType = (RadComboBox)dataItem.FindControl("ddlDoseType");

		//                RadComboBox ddlReferanceItem = (RadComboBox)dataItem.FindControl("ddlReferanceItem");

		//                if (Request.QueryString["DRUGORDERCODE"] == null)
		//                {
		//                    if (ddlDoseType.SelectedValue == "" || ddlDoseType.SelectedValue == "0")
		//                    {
		//                        if (ddlFrequencyId.SelectedValue == "0" || ddlFrequencyId.SelectedValue == "")
		//                        {
		//                            Alert.ShowAjaxMsg("Please select Frequency", Page);
		//                            return;
		//                        }

		//                        if (ddlReferanceItem.SelectedValue == "0" || ddlReferanceItem.SelectedValue == "")
		//                        {
		//                            if (common.myDbl(txtDose.Text) == 0.0)
		//                            {
		//                                txtDose.Text = "1";
		//                                //Alert.ShowAjaxMsg("Please type Dose", Page);
		//                                //return;
		//                            }
		//                            if (txtDuration.Text == "" || txtDuration.Text == "0"
		//                                || txtDuration.Text == "0." || txtDuration.Text == ".0"
		//                                || txtDuration.Text == ".")
		//                            {
		//                                Alert.ShowAjaxMsg("Please type Duration", Page);
		//                                return;
		//                            }
		//                        }
		//                    }
		//                }

		//                //Label lblTotalQty = (Label)dataItem.FindControl("lblTotalQty");

		//                string Type = "";
		//                decimal dDuration = 0;

		//                switch (common.myStr(ddlPeriodType.SelectedValue))
		//                {
		//                    case "N":
		//                        Type = txtDuration.Text + " Minute(s)";
		//                        dDuration = 1;
		//                        break;
		//                    case "H":
		//                        Type = txtDuration.Text + " Hour(s)";
		//                        dDuration = 1;
		//                        break;
		//                    case "D":
		//                        Type = txtDuration.Text + " Day(s)";
		//                        dDuration = 1;
		//                        break;
		//                    case "W":
		//                        Type = txtDuration.Text + " Week(s)";
		//                        dDuration = 7;
		//                        break;
		//                    case "M":
		//                        Type = txtDuration.Text + " Month(s)";
		//                        dDuration = 30;
		//                        break;
		//                    case "Y":
		//                        Type = txtDuration.Text + " Year(s)";
		//                        dDuration = 365;
		//                        break;
		//                }
		//                RadComboBox ddlUnit = (RadComboBox)dataItem.FindControl("ddlUnit");
		//                HiddenField StartDate = (HiddenField)dataItem.FindControl("hdnStartDate");
		//                HiddenField EndDate = (HiddenField)dataItem.FindControl("hdnEndDate");
		//                HiddenField Remarks = (HiddenField)dataItem.FindControl("hdnRemarks");
		//                ddlFoodRelation = (RadComboBox)dataItem.FindControl("ddlFoodRelation");
		//                RadDatePicker txtStartDate = (RadDatePicker)dataItem.FindControl("txtStartDate");
		//                RadDatePicker txtEndDate = (RadDatePicker)dataItem.FindControl("txtEndDate");
		//                HiddenField hdnCIMSItemId = (HiddenField)dataItem.FindControl("hdnCIMSItemId");
		//                HiddenField hdnCIMSType = (HiddenField)dataItem.FindControl("hdnCIMSType");
		//                HiddenField hdnVIDALItemId = (HiddenField)dataItem.FindControl("hdnVIDALItemId");
		//                HiddenField hdnQty = (HiddenField)dataItem.FindControl("hdnQty");

		//                txtInstructions = (TextBox)dataItem.FindControl("txtInstructions");

		//                HiddenField hdnXMLData = (HiddenField)dataItem.FindControl("hdnXMLData");
		//                //if (Request.QueryString["DRUGORDERCODE"] == null)
		//                //{
		//                if (common.myBool(ViewState["ISCalculationRequired"]))
		//                {
		//                    dQty = dQty + common.myDec(sFrequency) * common.myDec(txtDose.Text) * common.myDec(txtDuration.Text) * dDuration;
		//                }
		//                else
		//                {
		//                    dQty = 1;
		//                }
		//                DR1["RouteId"] = ddlRoute.SelectedValue;
		//                DR1["RouteName"] = ddlRoute.SelectedItem.Text;
		//                //}
		//                //else
		//                //{
		//                //    dQty = 0;
		//                //}

		//                DR1["GenericId"] = ddlGeneric.SelectedValue == "" ? 0 : common.myInt(ddlGeneric.SelectedValue);
		//                DR1["ItemId"] = common.myInt(dr["itemid"]);
		//                DR1["GenericName"] = ddlGeneric.SelectedValue == "" ? "" : ddlGeneric.Text;
		//                DR1["ItemName"] = common.myStr(dr["itemname"]);


		//                DR1["Dose"] = txtDose.Text;
		//                DR1["Frequency"] = common.myInt(ddlFrequencyId.SelectedValue) == 0 ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
		//                DR1["Duration"] = txtDuration.Text;
		//                DR1["DurationText"] = Type;
		//                DR1["Type"] = ddlPeriodType.SelectedValue;

		//                DR1["StartDate"] = common.myDate(txtStartDate.SelectedDate).ToString("dd/MM/yyyy");
		//                DR1["EndDate"] = common.myDate(txtEndDate.SelectedDate).ToString("dd/MM/yyyy");

		//                DR1["FoodRelationshipId"] = ddlFoodRelation.SelectedValue == "0" ? 0 : common.myInt(ddlFoodRelation.SelectedValue);
		//                DR1["FoodRelationship"] = ddlFoodRelation.SelectedValue == "0" ? "" : common.myStr(ddlFoodRelation.Text);
		//                DR1["UnitId"] = ddlUnit.SelectedValue == "" || ddlUnit.SelectedValue == "0" ? 0 : common.myInt(ddlUnit.SelectedValue);
		//                DR1["UnitName"] = ddlUnit.SelectedValue == "" || ddlUnit.SelectedValue == "0" ? "" : ddlUnit.Text;
		//                DR1["CIMSItemId"] = common.myInt(hdnCIMSItemId.Value);
		//                DR1["CIMSType"] = hdnCIMSType.Value;
		//                DR1["VIDALItemId"] = common.myInt(hdnVIDALItemId.Value);
		//                DR1["PrescriptionDetail"] = "";
		//                DR1["Instructions"] = txtInstructions.Text;
		//                DR1["ReferanceItemId"] = ddlReferanceItem.SelectedValue == "" || ddlReferanceItem.SelectedValue == "0" ? 0 : Convert.ToInt32(ddlReferanceItem.SelectedValue);
		//                DR1["ReferanceItemName"] = ddlReferanceItem.SelectedValue == "" || ddlReferanceItem.SelectedValue == "0" ? "" : ddlReferanceItem.Text;
		//                DR1["DoseTypeId"] = ddlDoseType.SelectedValue;
		//                DR1["DoseTypeName"] = ddlDoseType.SelectedValue == "" || ddlDoseType.SelectedValue == "0" ? "" : ddlDoseType.Text;
		//                if (hdnInfusion.Value == "1" || (ddlReferanceItem.SelectedItem != null && common.myBool(ddlReferanceItem.SelectedItem.Attributes["IsInfusion"]) == true))
		//                {
		//                    DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
		//                    DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
		//                    DR1["IsInfusion"] = true;
		//                    DR["IsInfusion"] = true;
		//                }
		//                else if (ddlDoseType.SelectedValue != "0")
		//                {
		//                    DR1["IsInfusion"] = false;
		//                    DR1["FrequencyId"] = 0;
		//                    DR1["FrequencyName"] = "";
		//                }
		//                else
		//                {
		//                    DR1["FrequencyId"] = common.myInt(ddlFrequencyId.SelectedValue);
		//                    DR1["FrequencyName"] = ddlFrequencyId.SelectedItem.Text;
		//                    DR1["IsInfusion"] = false;
		//                }
		//                dt1.Rows.Add(DR1);
		//                dt1.AcceptChanges();
		//                ddlReferanceItem.SelectedValue = "0";
		//                txtInstructions.Text = "";
		//            }
		//            dt1.TableName = "ItemDetail";
		//            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

		//            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
		//            dt1.WriteXml(writer);

		//            string xmlSchema = writer.ToString();
		//            DR["XMLData"] = xmlSchema;
		//            DR["PrescriptionDetail"] = emr.GetPrescriptionDetailString(dt1);
		//            //DR["Qty"] = dQty.ToString("F2");
		//            //---------------------

		//            DR["Qty"] = dr["Qty"];

		//            dt.Rows.Add(DR);
		//            dt.AcceptChanges();
		//        }
		//    }
		//    //}

		//    //-----------------------------
		//    // dt1.TableName = "ItemDetail";
		//    // System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
		//    //System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
		//    //dt1.WriteXml(writer);
		//    //string xmlSchema = writer.ToString();
		//    ViewState["DataTableDetail"] = null;
		//    ViewState["ItemDetail"] = dt1;

		//    ViewState["Item"] = dt;
		//    gvItem.DataSource = dt;
		//    gvItem.DataBind();

		//    BindBlankItemDetailGrid();
		//    setVisiblilityInteraction();
		//    ddlFormulation.Enabled = true;
		//    ddlRoute.Enabled = true;
		//    ddlStrength.Enabled = true;
		//    ddlBrand.Focus();
		//    ddlBrand.Items.Clear();
		//    ddlBrand.Text = "";
		//    ddlGeneric.Items.Clear();
		//    ddlGeneric.Text = "";
		//    ddlBrand.Enabled = true;
		//    ddlBrand.SelectedValue = "0";


		//    ViewState["Edit"] = null;
		//    ViewState["ItemId"] = null;
		//    txtCustomMedication.Text = "";

		//}
		//catch (Exception Ex)
		//{
		//    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
		//    lblMessage.Text = "Error: " + Ex.Message;
		//    objException.HandleException(Ex);
		//}
		//finally
		//{
		//    dtold.Dispose();
		//    dt1old.Dispose();
		//    dt.Dispose();
		//    dt1.Dispose();
		//    dtNew.Dispose();
		//    ds.Dispose();
		//    emr = null;
		//}
		#endregion
	}

	private void addItem()
	{
		BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
		DataTable dtold = new DataTable();
		DataTable dt1old = new DataTable();

		DataTable dt = new DataTable();
		DataTable dt1 = new DataTable();
		DataTable dtNew = new DataTable();
		DataSet ds = new DataSet();
		try
		{
			DataRow DR;
			DataRow DR1;
			decimal dQty = 0;
			int countRow = 0;

			if (((ddlBrand.SelectedValue == "" || ddlBrand.SelectedValue == "0")) && (common.myLen(txtCustomMedication.Text).Equals(0)))
			{
				Alert.ShowAjaxMsg("Please select drug", Page);
				return;
			}

			if (ViewState["Item"] == null && ViewState["Edit"] == null)
			{
				dt = CreateItemTable();
				dt1 = CreateItemDetailTable();
			}
			else
			{
				dtold = (DataTable)ViewState["Item"];
				DataView dv = new DataView(dtold);

				if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["Item"] != null)
				{
					if (chkCustomMedication.Checked)
					{
						dv.RowFilter = "ISNULL(ItemName,'') <> '" + common.myStr(txtCustomMedication.Text, true).Trim() + "'";
					}
					else
					{

						dv.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
						dt = dv.ToTable();
					}
				}
				else
				{
					dt = (DataTable)ViewState["Item"];
				}
				if (dt.Rows.Count > 0)
				{
					if (ViewState["Edit"] == null)
					{
						foreach (GridViewRow dataItem in gvItem.Rows)
						{
							TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
							dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString();
							countRow++;
							dt.AcceptChanges();
						}
					}
				}
				if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
				{
					dt1 = CreateItemDetailTable();
				}
				else
				{
					dt1old = (DataTable)ViewState["ItemDetail"];
					if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["ItemDetail"] != null)
					{
						DataView dv1 = new DataView(dt1old);
						if (chkCustomMedication.Checked)
						{
							dv1.RowFilter = "ISNULL(ItemName,'') <> '" + common.myStr(txtCustomMedication.Text, true).Trim() + "'";
						}
						else
						{
							dv1.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
						}
						dt1 = dv1.ToTable();
					}
					else
					{
						dt1 = (DataTable)ViewState["ItemDetail"];
					}
				}
			}
			if (!chkCustomMedication.Checked)
			{
				foreach (GridViewRow row in gvItem.Rows)
				{
					HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
					if (hdnItemId.Value == ddlBrand.SelectedValue && ViewState["Edit"] == null)
					{
						Alert.ShowAjaxMsg("Item already added", Page);
						return;
					}
				}
			}

			//Item
			DR = dt.NewRow();

			if (ddlIndentType.SelectedValue == "")
			{
				Alert.ShowAjaxMsg("Please select Indent Type", Page);
				return;
			}
			DR["IndentTypeId"] = Convert.ToInt16(ddlIndentType.SelectedValue);
			DR["IndentType"] = ddlIndentType.SelectedValue == "" ? "" : ddlIndentType.Text;
			DR["IndentId"] = DBNull.Value;
			DR["GenericId"] = DBNull.Value;

			if (common.myInt(ddlBrand.SelectedValue).Equals(0) && chkCustomMedication.Checked)
			{
				DR["ItemId"] = DBNull.Value;
			}
			else if (common.myInt(ddlBrand.SelectedValue) > 0 && !chkCustomMedication.Checked)
			{
				DR["ItemId"] = common.myInt(ddlBrand.SelectedValue);
			}

			DR["GenericName"] = DBNull.Value;
			//DR["ItemName"] = common.myStr(ddlBrand.Text);
			DR["ItemName"] = chkCustomMedication.Checked ? common.myStr(txtCustomMedication.Text).Trim() : common.myStr(ddlBrand.Text);
			DR["RouteId"] = DBNull.Value;
			//DR["CustomMedication"] = false;
			DR["CustomMedication"] = chkCustomMedication.Checked;
			DR["NotToPharmacy"] = false;
			DR["IsInfusion"] = false;

			DR["FormulationId"] = DBNull.Value;
			DR["UnitName"] = DBNull.Value;
			///Item Detail
			DR1 = dt1.NewRow();
			dQty = common.myInt(txtQty.Text); //common.myInt(dQty.ToString());
			DR1["RouteName"] = DBNull.Value;
			DR1["GenericId"] = DBNull.Value;
			DR1["ItemId"] = common.myInt(ddlBrand.SelectedValue);
			DR1["GenericName"] = DBNull.Value;
			//DR1["ItemName"] = common.myStr(ddlBrand.SelectedValue) == "" ? "" : ddlBrand.Text;
			DR1["ItemName"] = chkCustomMedication.Checked ? common.myStr(txtCustomMedication.Text).Trim() : common.myStr(ddlBrand.Text);
			DR1["Dose"] = DBNull.Value;
			DR1["Frequency"] = DBNull.Value;
			DR1["Duration"] = DBNull.Value;
			DR1["DurationText"] = DBNull.Value;
			DR1["Type"] = DBNull.Value;

			DR1["StartDate"] = common.myDate(DateTime.Now).ToString("dd/MM/yyyy");


			DR1["EndDate"] = common.myDate(DateTime.Now).ToString("dd/MM/yyyy");

			DR1["FoodRelationshipId"] = DBNull.Value;
			DR1["FoodRelationship"] = DBNull.Value;



			DR1["UnitId"] = DBNull.Value;
			DR1["UnitName"] = DBNull.Value;
			DR1["CIMSItemId"] = DBNull.Value;
			DR1["CIMSType"] = DBNull.Value;
			DR1["VIDALItemId"] = DBNull.Value;
			DR1["PrescriptionDetail"] = common.myStr(txtRemarks.Text);
			DR1["Instructions"] = DBNull.Value;
			DR1["ReferanceItemId"] = DBNull.Value;
			DR1["ReferanceItemName"] = DBNull.Value;
			DR1["DoseTypeId"] = DBNull.Value;
			DR1["DoseTypeName"] = DBNull.Value;

			DR1["IsInfusion"] = DBNull.Value;
			DR1["FrequencyId"] = DBNull.Value;
			DR1["FrequencyName"] = DBNull.Value;

			dt1.Rows.Add(DR1);
			dt1.AcceptChanges();


			dt1.TableName = "ItemDetail";
			System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

			System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
			dt1.WriteXml(writer);

			string xmlSchema = writer.ToString();
			DR["XMLData"] = xmlSchema;
			DR["PrescriptionDetail"] = common.myStr(txtRemarks.Text);
			DR["Qty"] = dQty.ToString();
			//   DR["ItemSubCategoryId"] = common.myStr(ddlBrand.SelectedItem.Attributes["ItemSubCategoryId"]);
			DR["ItemSubCategoryId"] = common.myInt(ddlBrand.SelectedValue).Equals(0) ? 0 : common.myInt(hdnItemSubCategoryId.Value);

			dt.Rows.Add(DR);
			dt.AcceptChanges();

			ViewState["DataTableDetail"] = null;
			ViewState["ItemDetail"] = dt1;
			ViewState["Item"] = dt;
			gvItem.DataSource = dt;
			gvItem.DataBind();


			ddlBrand.Focus();
			ddlBrand.Items.Clear();
			ddlBrand.Text = "";
			txtQty.Text = "";
			ddlBrand.Enabled = true;
			ddlBrand.SelectedValue = "0";
			ViewState["Edit"] = null;
			ViewState["ItemId"] = null;
			txtRemarks.Text = "";
			txtCustomMedication.Text = string.Empty;
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			objException.HandleException(Ex);
		}
		finally
		{
			dtold.Dispose();
			dt1old.Dispose();
			dt.Dispose();
			dt1.Dispose();
			dtNew.Dispose();
			ds.Dispose();
			emr = null;
		}
	}


	protected void btnYes_OnClick(object sender, EventArgs e)
	{
		try
		{
			//if (common.myBool(ViewState["HighValueItem"]))
			//{
			//    string strMsg = "This is high value item do you want to proceed";
			//    //Alert.ShowAjaxMsg(strMsg, this.Page);
			//    //clearItemDetails();
			//}

			dvConfirm.Visible = false;
			//txtDose.Focus();
		}

		catch (Exception ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + ex.Message;
			objException.HandleException(ex);

		}
		finally
		{

			dvConfirm.Visible = false;
			lblConfirmHighValue.Text = "This is high value item do you want to proceed ";


		}


	}
	protected void btnCancel_OnClick(object sender, EventArgs e)
	{
		try
		{

			ddlBrand.Items.Clear();
			ddlBrand.Text = string.Empty;
			ddlBrand.Enabled = true;
			ddlBrand.SelectedValue = null;

			dvConfirm.Visible = false;
		}
		catch (Exception ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + ex.Message;
			objException.HandleException(ex);

		}
		finally
		{

			dvConfirm.Visible = false;
			lblConfirmHighValue.Text = "This is high value item do you want to proceed ";

		}


	}


	private void BindItemWithItemDetail(DataTable dtItem, DataTable dtItemDetail)
	{
		BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
		DataTable dtold = new DataTable();
		DataTable dt1old = new DataTable();

		DataTable dt = new DataTable();
		dt = (DataTable)ViewState["Item"];
		DataTable dt1 = new DataTable();
		dt1 = (DataTable)ViewState["ItemDetail"];
		DataTable dtNew = new DataTable();
		DataSet ds = new DataSet();

		DataRow DR;
		DataRow DR1;
		decimal dQty = 0;
		int countRow = 0;

		try
		{
			for (int counter = 0; counter < dtItem.Rows.Count; counter++)
			{
				DataTable dtTemp = new DataTable();
				dtTemp = (DataTable)ViewState["Item"];
				int exists = 0;
				for (int counter1 = 0; counter1 < dtTemp.Rows.Count; counter1++)
				{
					if (common.myInt(dtItem.Rows[counter]["ItemId"].ToString()) == common.myInt(dtTemp.Rows[counter1]["ItemId"].ToString()))
					{
						exists = 1;
						break;
						//counter1 = dtTemp.Rows.Count;
					}
				}
				if (exists == 0)
				{
					DR = dt.NewRow();

					if (ddlIndentType.SelectedValue == "")
					{
						Alert.ShowAjaxMsg("Please select Indent Type", Page);
						return;
					}
					DR["IndentTypeId"] = dtItem.Rows[counter]["IndentTypeId"];
					DR["IndentType"] = dtItem.Rows[counter]["IndentType"];
					DR["IndentId"] = 0;
					DR["GenericId"] = dtItem.Rows[counter]["GenericId"];
					DR["ItemId"] = dtItem.Rows[counter]["ItemId"];
					DR["GenericName"] = dtItem.Rows[counter]["GenericName"];
					DR["ItemName"] = dtItem.Rows[counter]["ItemName"];
					DR["RouteId"] = dtItem.Rows[counter]["RouteId"];
					DR["CustomMedication"] = dtItem.Rows[counter]["CustomMedication"];
					DR["NotToPharmacy"] = dtItem.Rows[counter]["NotToPharmacy"];
					DR["IsInfusion"] = dtItem.Rows[counter]["IsInfusion"];
					DR["FormulationId"] = dtItem.Rows[counter]["FormulationId"];
					///Item Detail
					//TextBox txtInstructions = new TextBox();
					//RadComboBox ddlFoodRelation = new RadComboBox();

					//Item Details start

					DR1 = dt1.NewRow();

					DR1["RouteId"] = dtItemDetail.Rows[counter]["RouteId"];
					DR1["RouteName"] = dtItemDetail.Rows[counter]["RouteName"];
					DR1["GenericId"] = dtItemDetail.Rows[counter]["GenericId"];
					DR1["ItemId"] = dtItemDetail.Rows[counter]["ItemId"];
					DR1["GenericName"] = dtItemDetail.Rows[counter]["GenericName"];
					DR1["ItemName"] = dtItemDetail.Rows[counter]["ItemName"];


					DR1["Dose"] = dtItemDetail.Rows[counter]["Dose"];
					DR1["Frequency"] = dtItemDetail.Rows[counter]["Frequency"];
					DR1["Duration"] = dtItemDetail.Rows[counter]["Duration"];
					DR1["DurationText"] = dtItemDetail.Rows[counter]["DurationText"];
					DR1["Type"] = dtItemDetail.Rows[counter]["Type"];

					DR1["StartDate"] = dtItemDetail.Rows[counter]["StartDate"];
					DR1["EndDate"] = dtItemDetail.Rows[counter]["EndDate"];

					DR1["FoodRelationshipId"] = dtItemDetail.Rows[counter]["FoodRelationshipId"];
					DR1["FoodRelationship"] = dtItemDetail.Rows[counter]["FoodRelationship"];
					DR1["UnitId"] = 0;// dtItemDetail.Rows[counter]["UnitId"];
					DR1["UnitName"] = dtItemDetail.Rows[counter]["UnitName"];
					DR1["CIMSItemId"] = dtItemDetail.Rows[counter]["CIMSItemId"];
					DR1["CIMSType"] = dtItemDetail.Rows[counter]["CIMSType"];
					DR1["VIDALItemId"] = dtItemDetail.Rows[counter]["VIDALItemId"];
					DR1["PrescriptionDetail"] = dtItemDetail.Rows[counter]["PrescriptionDetail"];
					DR1["Instructions"] = dtItemDetail.Rows[counter]["Instructions"];
					DR1["ReferanceItemId"] = dtItemDetail.Rows[counter]["ReferanceItemId"];
					DR1["ReferanceItemName"] = dtItemDetail.Rows[counter]["ReferanceItemName"];
					DR1["DoseTypeId"] = dtItemDetail.Rows[counter]["DoseTypeId"];
					DR1["DoseTypeName"] = dtItemDetail.Rows[counter]["DoseTypeName"];

					DR1["FrequencyId"] = dtItemDetail.Rows[counter]["FrequencyId"];
					DR1["FrequencyName"] = dtItemDetail.Rows[counter]["FrequencyName"];
					DR1["IsInfusion"] = true;
					DR["IsInfusion"] = true;

					dt1.Rows.Add(DR1);
					dt1.AcceptChanges();

					dt1.TableName = "ItemDetail";
					System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

					System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
					dt1.WriteXml(writer);

					string xmlSchema = writer.ToString();
					DR["XMLData"] = xmlSchema;
					DR["PrescriptionDetail"] = emr.GetPrescriptionDetailString(dt1);
					//Item Details end

					dt.Rows.Add(DR);
					dt.AcceptChanges();
				}

			}
			ViewState["Item"] = dt;
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			objException.HandleException(Ex);
		}
		finally
		{
			dtold.Dispose();
			dt1old.Dispose();
			dt.Dispose();
			dt1.Dispose();
			dtNew.Dispose();
			ds.Dispose();
			emr = null;
		}

	}

	protected void btnAddItem_OnClick(object sender, EventArgs e)
	{
		DataTable dt = new DataTable();
		BaseC.EMR objEMR2 = new BaseC.EMR(sConString);
		DataView dvDrugAllergy = new DataView();
		DataTable tblDrugAllergy = new DataTable();
		string Instructions = string.Empty;
		try
		{
			if (common.myStr(ViewState["IsSelectAdvisingDoctorInConsumableOrder"]).Equals("Y"))
			{
				if (ddlAdvisingDoctor.SelectedIndex.Equals(0))
				{
					lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
					lblMessage.Text = "Advising Doctor not selected";
					return;
				}
			}
			addItem();
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			objException.HandleException(Ex);
		}
	}



	protected void btnAlredyExistProceed_OnClick(object sender, EventArgs e)
	{
		try
		{
			addItem();
			dvConfirmAlreadyExistOptions.Visible = false;
		}
		catch
		{
		}
	}
	protected void btnAlredyExistCancel_OnClick(object sender, EventArgs e)
	{
		dvConfirmAlreadyExistOptions.Visible = false;
	}
	private void bindDdlstore()
	{
		DataSet dsStoreMasterList = new DataSet();
		BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
		//RadComboBoxItem item;
		//ddlStore.Items.Clear();
		//ddlStore.Text = string.Empty;

		try
		{
			string sEncId = ViewState["EncId"].ToString();

			dsStoreMasterList = objEMR.getStoreMasterList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
														 common.myInt(Session["UserID"]), common.myInt(sEncId));


			//if (dsStoreMasterList.Tables.Count > 0)
			//{
			//    foreach (DataRow dr in dsStoreMasterList.Tables[0].Rows)
			//    {
			//        item = new RadComboBoxItem();
			//        item.Text = common.myStr(dr["DepartmentName"]);
			//        item.Value = common.myInt(dr["StoreId"]).ToString();
			//        ddlStore.Items.Add
			//            (item);

			//    }
			//}
			if (dsStoreMasterList.Tables.Count > 0)
			{
				ddlStore.DataSource = dsStoreMasterList.Tables[0];
				ddlStore.DataTextField = "DepartmentName";
				ddlStore.DataValueField = "StoreId";
				ddlStore.DataBind();
			}

			if (dsStoreMasterList.Tables[0].Rows.Count > 0)
			{
				if (ViewState["OPIP"] != null && ViewState["OPIP"].ToString() == "I"
					 && Request.QueryString["DRUGORDERCODE"] == "DO" && Request.QueryString["LOCATION"] == "WARD")
				{
					ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
																														   common.myInt(Session["FacilityId"]), "DefaultWardIndentStoreId", sConString))));

					ViewState["Consumable"] = false;
				}
				if (ViewState["OPIP"] != null && ViewState["OPIP"].ToString() == "I"
					&& Request.QueryString["DRUGORDERCODE"] == "CO" && Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"] == "WARD")
				{
					ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
																														   common.myInt(Session["FacilityId"]), "DefaultWardConsumableStoreId", sConString))));

					ViewState["Consumable"] = true;
				}
				else if (ViewState["OPIP"] != null && ViewState["OPIP"].ToString() == "O" && Request.QueryString["DRUGORDERCODE"] == null)
				{
					ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
																														   common.myInt(Session["FacilityId"]), "DefaultOPIndentStoreId", sConString))));

					ViewState["Consumable"] = false;
				}
				else if (ViewState["OPIP"] != null && ViewState["OPIP"].ToString() == "I"
					&& Request.QueryString["DRUGORDERCODE"] == "CO" && Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"] == "OT")
				{
					ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
																														   common.myInt(Session["FacilityId"]), "DefaultOTIndentStoreId", sConString))));

					ViewState["Consumable"] = true;
				}
				else if (ViewState["OPIP"] != null && ViewState["OPIP"].ToString() == "E")
				{
					ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindItemByValue(common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
																									   common.myInt(Session["FacilityId"]), "DefaultERIndentStoreId", sConString))));

					ViewState["Consumable"] = false;
				}
				hdnStoreId.Value = ddlStore.SelectedValue;
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
			dsStoreMasterList.Dispose();
			objEMR = null;
		}
	}
	private void ChangeRowColor()
	{
		foreach (GridViewRow row in gvItem.Rows)
		{
			Label lblItemName = (Label)row.FindControl("lblItemName");
			if (common.myStr(((HiddenField)row.FindControl("hdnChangeRowColor")).Value).Equals("1"))
			{
				row.Cells[2].BackColor = System.Drawing.Color.LightGreen;
			}
			else
			{
				row.Cells[2].BackColor = System.Drawing.Color.White;
			}
		}
	}

	void bindPreviousIndent()
	{
		rdoClosedByNurse.SelectedValue = "0";

		if (common.myInt(ViewState["IndentId"]).Equals(0))
		{
			tblLegend.Visible = false;
			return;
		}

		BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
		DataSet ds = new DataSet();
		bool IsClosed = false;
		try
		{
			ds = objPharmacy.getPreviousMedicines(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
										common.myInt(ViewState["EncId"]), common.myInt(ViewState["IndentId"]), 0, "A", string.Empty, string.Empty, string.Empty);

			if (ds.Tables[0].Rows.Count > 0)
			{
				if (common.myBool(ds.Tables[0].Rows[0]["IsClosedByNurse"]))
				{
					rdoClosedByNurse.SelectedValue = "1";
					btnSave.Visible = false;
				}
			}
			else
			{
				DataRow DR = ds.Tables[0].NewRow();
				ds.Tables[0].Rows.Add(DR);
			}

			gvIndent.DataSource = ds.Tables[0];
			gvIndent.DataBind();
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			objException.HandleException(Ex);
		}
		finally
		{
			objPharmacy = null;
			ds.Dispose();
		}
	}

	protected void gvIndent_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		try
		{
			//BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
			//IsShowIndent = common.myStr(objBill.getHospitalSetupValue("IsShowIndentForSPS", common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"])));
			//if (IsShowIndent == "Y")
			//{
			//    //e.Row.Cells[3].Visible = false;
			//}
			//else
			//{
			//    //e.Row.Cells[3].Visible = true;
			//}

			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				HiddenField hdnIsConsumable = (HiddenField)e.Row.FindControl("hdnIsConsumable");

				if (common.myBool(hdnIsConsumable.Value))
				{
					e.Row.Cells[1].BackColor = System.Drawing.Color.Cyan;
				}
			}
		}
		catch
		{
		}
	}

	protected void gvIndent_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			lblMessage.Text = "";
			//strXMLItems = new StringBuilder();
			//coll = new ArrayList();

			//if (e.CommandName == "MAKEORDER")
			//{
			//    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
			//    HiddenField hdnIndentId = (HiddenField)row.FindControl("hdnIndentId");

			//    if (common.myInt(hdnIndentId.Value) > 0)
			//    {
			//        Label lblRegistrationNo = (Label)row.FindControl("lblRegistrationNo");
			//        Label lblEncounterNo = (Label)row.FindControl("lblEncounterNo");
			//        HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
			//        HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

			//        RadWindow1.NavigateUrl = "/EMR/Medication/ConsumableOrder.aspx?POPUP=POPUP&DRUGORDERCODE=CO&LOCATION=WARD" +
			//                                "&Regno=" + common.myStr(lblRegistrationNo.Text) +
			//                                "&Encno=" + common.myStr(lblEncounterNo.Text) +
			//                                "&RegId=" + common.myStr(hdnRegistrationId.Value) +
			//                                "&EncId=" + common.myStr(hdnEncounterId.Value);

			//        RadWindow1.Height = 620;
			//        RadWindow1.Width = 1200;
			//        RadWindow1.Top = 40;
			//        RadWindow1.Left = 100;
			//        RadWindow1.OnClientClose = "SearchIndentOnClientClose";
			//        RadWindow1.VisibleOnPageLoad = true;
			//        RadWindow1.Modal = true;
			//        RadWindow1.VisibleStatusbar = false;
			//        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
			//    }
			//}
		}
		catch (Exception Ex)
		{
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "Error: " + Ex.Message;
			objException.HandleException(Ex);
		}
	}

	protected void lnkBtnPendingConsumableIndent_OnClick(object sender, EventArgs e)
	{
		Session["PendingConsumableIndentId"] = null;
		lblMessage.Text = "";
		RadWindow1.NavigateUrl = "/WardManagement/PendingConsumableIndent.aspx?POPUP=POPUP" +
									"&Regno=" + common.myStr(ViewState["Regno"]) +
									"&Encno=" + common.myStr(ViewState["Encno"]) +
									"&RegId=" + common.myStr(ViewState["RegId"]) +
									"&EncId=" + common.myStr(ViewState["EncId"]) +
									"&DoctorId=" + common.myStr(ViewState["DoctorId"]);

		RadWindow1.Height = 500;
		RadWindow1.Width = 1100;
		RadWindow1.Top = 10;
		RadWindow1.Left = 10;
		RadWindow1.Modal = true;
		RadWindow1.OnClientClose = "OnClientPendingConsumableIndentClose";
		RadWindow1.VisibleOnPageLoad = true;
		RadWindow1.VisibleStatusbar = false;
		RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
		RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
	}

	protected void btnPendingConsumableIndent_OnClick(object sender, EventArgs e)
	{
		ViewState["IndentId"] = common.myStr(Session["PendingConsumableIndentId"]);
		if (common.myInt(ViewState["IndentId"]) > 0)
		{
			divClosedByNurse.Visible = true;
		}
		bindPreviousIndent();
	}

	private void BindAdviserDoctor()
	{
		BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
		DataTable tbl = new DataTable();
		try
		{
			tbl = objlis.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]), 0);
			if (tbl != null && tbl.Rows.Count > 0)
			{
				ddlAdvisingDoctor.Items.Clear();
				ddlAdvisingDoctor.DataSource = tbl;
				ddlAdvisingDoctor.DataTextField = "DoctorName";
				ddlAdvisingDoctor.DataValueField = "DoctorId";
				ddlAdvisingDoctor.DataBind();
				ddlAdvisingDoctor.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
				ddlAdvisingDoctor.SelectedIndex = 0;
			}
		}
		catch (Exception Ex)
		{
			clsExceptionLog objException = new clsExceptionLog();
			objException.HandleException(Ex);
		}
		finally { objlis = null; tbl = null; }
	}

	protected void chkCustomMedication_OnCheckedChanged(object sender, EventArgs e)
	{
		BaseC.Security objSecurity = new BaseC.Security(sConString);  //
		if (common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]),
			common.myInt(Session["FacilityId"]), "IsAuthorizedToCustomMedicationForEMR")) == true)
		{
			setCustomMedicationChange();

		}
		else
		{
			chkCustomMedication.Checked = false;
			lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
			lblMessage.Text = "You are not authorized to fill custom medicines.";

		}
		objSecurity = null;


	}

	private void setCustomMedicationChange()
	{
		try
		{
			if (chkCustomMedication.Checked)
			{
				txtCustomMedication.Text = string.Empty;
			}
			else
			{
				hdnGenericId.Value = "0";
				hdnGenericName.Value = string.Empty;
				hdnItemId.Value = "0";
				ddlBrand.Text = string.Empty;
				ddlBrand.SelectedValue = null;

				hdnCIMSItemId.Value = string.Empty;
				hdnCIMSType.Value = string.Empty;
				hdnVIDALItemId.Value = string.Empty;

			}
			trGeneric.Visible = !chkCustomMedication.Checked;
			trCustomMedication.Visible = chkCustomMedication.Checked;
		}
		catch
		{
		}
	}
	protected void lnkStopMedication_OnClick(object sender, EventArgs e)
	{
		string sRegId = common.myInt(Request.QueryString["RegId"]).Equals(0) ? common.myInt(Session["RegistrationId"]).ToString() : common.myInt(Request.QueryString["RegId"]).ToString();
		string sEncId = common.myInt(Request.QueryString["EncId"]).Equals(0) ? common.myInt(Session["EncounterId"]).ToString() : common.myInt(Request.QueryString["EncId"]).ToString();

		ViewState["Mode"] = "S";
		RadWindow1.NavigateUrl = "/EMR/Medication/StopMedication.aspx?RegId=" + sRegId + "&EncId=" + sEncId;
		RadWindow1.Height = 600;
		RadWindow1.Width = 900;
		RadWindow1.Top = 10;
		RadWindow1.Left = 10;
		//RadWindow1w1.OnClientClose = "OnClientClose";
		RadWindow1.VisibleOnPageLoad = true;
		RadWindow1.Modal = true;
		RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
		RadWindow1.VisibleStatusbar = false;
	}
	#region Omprakash SHARMA 05-05-2021
	protected void btnRefresh_Click(object sender, EventArgs e)
	{
		addpreviousItem();
	}
	private void addpreviousItem()
	{
		BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
		DataTable dtold = new DataTable();
		DataTable dt1old = new DataTable();

		DataTable dt = new DataTable();
		DataTable dt1 = new DataTable();
		DataTable dtNew = new DataTable();
		DataSet ds = new DataSet();
		try
		{
			DataTable dtpreviousItem = (DataTable)Session["previousitemtbl"];

			foreach (DataRow row1 in dtpreviousItem.Rows)
			{
				ddlBrand.SelectedValue = row1["ItemId"].ToString();
				ddlBrand.Text = row1["ItemName"].ToString();
				DataRow DR;
				DataRow DR1;
				decimal dQty = common.myDec(row1["Qty"]);
				int countRow = 0;

				if (((ddlBrand.SelectedValue == "" || ddlBrand.SelectedValue == "0")) && (common.myLen(txtCustomMedication.Text).Equals(0)))
				{
					Alert.ShowAjaxMsg("Please select drug", Page);
					return;
				}

				if (ViewState["Item"] == null && ViewState["Edit"] == null)
				{
					dt = CreateItemTable();
					dt1 = CreateItemDetailTable();
				}
				else
				{
					dtold = (DataTable)ViewState["Item"];
					DataView dv = new DataView(dtold);

					if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["Item"] != null)
					{
						if (chkCustomMedication.Checked)
						{
							dv.RowFilter = "ISNULL(ItemName,'') <> '" + common.myStr(txtCustomMedication.Text, true).Trim() + "'";
						}
						else
						{

							dv.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
							dt = dv.ToTable();
						}
					}
					else
					{
						dt = (DataTable)ViewState["Item"];
					}
					if (dt.Rows.Count > 0)
					{
						if (ViewState["Edit"] == null)
						{
							foreach (GridViewRow dataItem in gvItem.Rows)
							{
								TextBox txtTotalQty = (TextBox)dataItem.FindControl("txtTotalQty");
								dt.Rows[countRow]["Qty"] = common.myDbl(txtTotalQty.Text).ToString();
								countRow++;
								dt.AcceptChanges();
							}
						}
					}
					if (ViewState["ItemDetail"] == null && ViewState["Edit"] == null)
					{
						dt1 = CreateItemDetailTable();
					}
					else
					{
						dt1old = (DataTable)ViewState["ItemDetail"];
						if (ViewState["Edit"] != null && Convert.ToBoolean(ViewState["Edit"]) == true && ViewState["ItemDetail"] != null)
						{
							DataView dv1 = new DataView(dt1old);
							if (chkCustomMedication.Checked)
							{
								dv1.RowFilter = "ISNULL(ItemName,'') <> '" + common.myStr(txtCustomMedication.Text, true).Trim() + "'";
							}
							else
							{
								dv1.RowFilter = "ItemId<>" + ViewState["ItemId"].ToString();
							}
							dt1 = dv1.ToTable();
						}
						else
						{
							dt1 = (DataTable)ViewState["ItemDetail"];
						}
					}
				}
				if (!chkCustomMedication.Checked)
				{
					foreach (GridViewRow row in gvItem.Rows)
					{
						HiddenField hdnItemId = (HiddenField)row.FindControl("hdnItemId");
						if (hdnItemId.Value == ddlBrand.SelectedValue && ViewState["Edit"] == null)
						{
							Alert.ShowAjaxMsg("Item already added", Page);
							return;
						}
					}
				}

				//Item
				DR = dt.NewRow();

				if (ddlIndentType.SelectedValue == "")
				{
					Alert.ShowAjaxMsg("Please select Indent Type", Page);
					return;
				}
				DR["IndentTypeId"] = Convert.ToInt16(ddlIndentType.SelectedValue);
				DR["IndentType"] = ddlIndentType.SelectedValue == "" ? "" : ddlIndentType.Text;
				DR["IndentId"] = DBNull.Value;
				DR["GenericId"] = DBNull.Value;

				if (common.myInt(ddlBrand.SelectedValue).Equals(0) && chkCustomMedication.Checked)
				{
					DR["ItemId"] = DBNull.Value;
				}
				else if (common.myInt(ddlBrand.SelectedValue) > 0 && !chkCustomMedication.Checked)
				{
					DR["ItemId"] = common.myInt(ddlBrand.SelectedValue);
				}

				DR["GenericName"] = DBNull.Value;
				//DR["ItemName"] = common.myStr(ddlBrand.Text);
				DR["ItemName"] = chkCustomMedication.Checked ? common.myStr(txtCustomMedication.Text).Trim() : common.myStr(ddlBrand.Text);
				DR["RouteId"] = DBNull.Value;
				//DR["CustomMedication"] = false;
				DR["CustomMedication"] = chkCustomMedication.Checked;
				DR["NotToPharmacy"] = false;
				DR["IsInfusion"] = false;

				DR["FormulationId"] = DBNull.Value;
				DR["UnitName"] = DBNull.Value;
				///Item Detail
				DR1 = dt1.NewRow();
				//dQty = common.myInt(txtQty.Text); //common.myInt(dQty.ToString());
				DR1["RouteName"] = DBNull.Value;
				DR1["GenericId"] = DBNull.Value;
				DR1["ItemId"] = common.myInt(ddlBrand.SelectedValue);
				DR1["GenericName"] = DBNull.Value;
				//DR1["ItemName"] = common.myStr(ddlBrand.SelectedValue) == "" ? "" : ddlBrand.Text;
				DR1["ItemName"] = chkCustomMedication.Checked ? common.myStr(txtCustomMedication.Text).Trim() : common.myStr(ddlBrand.Text);
				DR1["Dose"] = DBNull.Value;
				DR1["Frequency"] = DBNull.Value;
				DR1["Duration"] = DBNull.Value;
				DR1["DurationText"] = DBNull.Value;
				DR1["Type"] = DBNull.Value;

				DR1["StartDate"] = common.myDate(DateTime.Now).ToString("dd/MM/yyyy");


				DR1["EndDate"] = common.myDate(DateTime.Now).ToString("dd/MM/yyyy");

				DR1["FoodRelationshipId"] = DBNull.Value;
				DR1["FoodRelationship"] = DBNull.Value;



				DR1["UnitId"] = DBNull.Value;
				DR1["UnitName"] = DBNull.Value;
				DR1["CIMSItemId"] = DBNull.Value;
				DR1["CIMSType"] = DBNull.Value;
				DR1["VIDALItemId"] = DBNull.Value;
				DR1["PrescriptionDetail"] = common.myStr(txtRemarks.Text);
				DR1["Instructions"] = DBNull.Value;
				DR1["ReferanceItemId"] = DBNull.Value;
				DR1["ReferanceItemName"] = DBNull.Value;
				DR1["DoseTypeId"] = DBNull.Value;
				DR1["DoseTypeName"] = DBNull.Value;

				DR1["IsInfusion"] = DBNull.Value;
				DR1["FrequencyId"] = DBNull.Value;
				DR1["FrequencyName"] = DBNull.Value;

				dt1.Rows.Add(DR1);
				dt1.AcceptChanges();


				dt1.TableName = "ItemDetail";
				System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.

				System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
				dt1.WriteXml(writer);

				string xmlSchema = writer.ToString();
				DR["XMLData"] = xmlSchema;
				DR["PrescriptionDetail"] = common.myStr(txtRemarks.Text);
				DR["Qty"] = dQty.ToString();
				//   DR["ItemSubCategoryId"] = common.myStr(ddlBrand.SelectedItem.Attributes["ItemSubCategoryId"]);
				DR["ItemSubCategoryId"] = common.myInt(ddlBrand.SelectedValue).Equals(0) ? 0 : common.myInt(hdnItemSubCategoryId.Value);

				dt.Rows.Add(DR);
				dt.AcceptChanges();

				ViewState["DataTableDetail"] = null;
				ViewState["ItemDetail"] = dt1;
				ViewState["Item"] = dt;
				gvItem.DataSource = dt;
				gvItem.DataBind();


				ddlBrand.Focus();
				ddlBrand.Items.Clear();
				ddlBrand.Text = "";
				txtQty.Text = "";
				ddlBrand.Enabled = true;
				ddlBrand.SelectedValue = "0";
				ViewState["Edit"] = null;
				ViewState["ItemId"] = null;
				txtRemarks.Text = "";
				txtCustomMedication.Text = string.Empty;
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
			dtold.Dispose();
			dt1old.Dispose();
			dt.Dispose();
			dt1.Dispose();
			dtNew.Dispose();
			ds.Dispose();
			emr = null;
		}
	}
	#endregion
}