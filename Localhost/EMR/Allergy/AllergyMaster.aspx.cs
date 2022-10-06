using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Allergy_AllergyMaster : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //clsExceptionLog objException = new clsExceptionLog();
    //BaseC.ParseData bc = new BaseC.ParseData();
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    private enum GridAllergy : byte
    {
        AllergyID = 0,
        SerialNo = 1,
        Description = 2,
        Status = 3,
        Edit = 4
    }

    private enum GridAllergyType : byte
    {
        TypeID = 0,
        SerialNo = 1,
        Description = 2,
        Status = 3,
        Edit = 4
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["userid"]) == "")
        {
            Response.Redirect("/default.aspx?RegNo=0", false);
        }
        
        if (!IsPostBack)
        {
            lblMsg.Text = "";

            if (common.myStr(Request.QueryString["From"]) == "POPUP")
            {
            }
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            PopulateAllergyType();
            BindAllergyTypeGrid();
            ddlAllergyType_OnSelectedIndexChanged(sender, e);
        }
    }

    private void PopulateAllergyType()
    {
        DataSet ds = new DataSet();
        try
        {
            ddlAllergyType.Items.Clear();
            //BaseC.EMRAllergy objEMRAllergy = new BaseC.EMRAllergy(sConString);
            if (Session["HospitalLocationID"] != null)
            {
                //SqlDataReader dr = objEMRAllergy.populateAllergyType(Convert.ToInt16(Session["HospitalLocationID"]));
                //ddlAllergyType.DataSource = dr;

                //WebClient client = new WebClient();
                //client.Headers["Content-type"] = "application/json";
                //client.Encoding = Encoding.UTF8;
                //string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/populateAllergyType";
                //APIRootClass.populateAllergyType objRoot = new global::APIRootClass.populateAllergyType();
                //objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);

                //string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                //string sValue = client.UploadString(ServiceURL, inputJson);
                //sValue = JsonConvert.DeserializeObject<string>(sValue);
                //ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/populateAllergyType";
                APIRootClass.populateAllergyType objRoot = new global::APIRootClass.populateAllergyType();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);



                ddlAllergyType.DataSource = ds;
                ddlAllergyType.DataValueField = "TypeID";
                ddlAllergyType.DataTextField = "TypeName";
                ddlAllergyType.DataBind();
                //dr.Close();
            }

            //ddlAllergyType.Items.Insert(0, " [Select] ");
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
        }
    }



    private void BindAllergyGrid()
    {
        try
        {
            string strTypeId = "";
            strTypeId = ddlAllergyType.SelectedValue.ToString();
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder str = new StringBuilder();
            str.Append("select am.AllergyID,  am.DESCRIPTION As AllergyName, at.TypeName as AllergyType, ");
            str.Append(" case am.Active when 0 then 'In-Active' else 'Active'  end as Status,CONVERT(int,am.Active) AS Active, am.TypeId ");
            str.Append(" from AllergyMaster am inner join AllergyType at on ");
            str.Append(" am.TypeID = at.TypeID ");
            str.Append(" where am.HospitalLocationID=" + Convert.ToInt16(Session["HospitalLocationID"]) + " order by Description");
            //DataSet objDs = (DataSet)objDl.FillDataSet(CommandType.Text, str.ToString());
            
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = str.ToString();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (objDs.Tables[0].Rows.Count > 0)
            {
                DataView dv = new DataView(objDs.Tables[0]);
                dv.RowFilter = "TypeId='" + strTypeId + "'";
                DataTable dt = dv.ToTable();
                gvAllergy.DataSource = dt;
                gvAllergy.DataBind();
            }
            else
            {
                ViewState["Blank"] = "True";
                BindBlankGrid();
                ViewState["Blank"] = null;
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    private void BindAllergyTypeGrid()
    {
        try
        {
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder str = new StringBuilder();
            str.Append("select TypeId, TypeName,CONVERT(int,Active) AS Active,Case Active when 0 then 'In-Active' else 'Active' end as Status  from allergytype");
            str.Append(" where HospitalLocationId =" + Convert.ToInt16(Session["HospitalLocationID"]) + " order by TypeName");
            //DataSet objDs = (DataSet)objDl.FillDataSet(CommandType.Text, str.ToString());

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = str.ToString();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (objDs.Tables[0].Rows.Count > 0)
            {
                gvAllergyType.DataSource = objDs.Tables[0];
                gvAllergyType.DataBind();
            }
            else
            {
                ViewState["Blank"] = "True";
                BindBlankAllergyTypeGrid();
                ViewState["Blank"] = null;
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    private void BindBlankGrid()
    {
        try
        {
            DataTable dtAllergy = new DataTable();
            dtAllergy.Columns.Add("AllergyID");
            dtAllergy.Columns.Add("AllergyName");
            dtAllergy.Columns.Add("Active");
            dtAllergy.Columns.Add("Status");
            dtAllergy.Columns.Add("SerialNo");
            for (int i = 0; i < 10; i++)
            {
                DataRow Dr = dtAllergy.NewRow();
                Dr["AllergyID"] = "";
                Dr["AllergyName"] = "";
                Dr["Active"] = "";
                Dr["Status"] = "";
                Dr["SerialNo"] = "";
                dtAllergy.Rows.Add(Dr);
            }
            gvAllergy.DataSource = dtAllergy;
            gvAllergy.DataBind();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    private void BindBlankAllergyTypeGrid()
    {
        try
        {
            DataTable dtAllergyType = new DataTable();
            dtAllergyType.Columns.Add("TypeID");
            dtAllergyType.Columns.Add("TypeName");
            dtAllergyType.Columns.Add("Active");
            dtAllergyType.Columns.Add("Status");
            dtAllergyType.Columns.Add("SerialNo");
            for (int i = 0; i < 10; i++)
            {
                DataRow Dr = dtAllergyType.NewRow();
                Dr["TypeID"] = "";
                Dr["TypeName"] = "";
                Dr["Active"] = "";
                Dr["Status"] = "";
                Dr["SerialNo"] = "";
                dtAllergyType.Rows.Add(Dr);
            }
            gvAllergyType.DataSource = dtAllergyType;
            gvAllergyType.DataBind();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void ddlAllergyType_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        if (ddlAllergyType.SelectedValue != "")
        {
            hdnAllergyType.Value = ddlAllergyType.SelectedItem.Value;
            BindAllergyGrid();
        }
    }

    protected void SaveAllergyMaster_OnClick(Object sender, EventArgs e)
    {
        try
        {
            clsIVF bc = new clsIVF(string.Empty);
            //BaseC.EMRAllergy objEMRAllergy = new BaseC.EMRAllergy(sConString);
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            //Hashtable hs = new Hashtable();
            //hs.Add("intHospitallocationId", Convert.ToInt16(Session["HospitalLocationID"]));
            //hs.Add("@chvVitalName", txtAllergyName.Text.ToString());
            //hs.Add("@intTypeId", common.myInt(ddlAllergyType.SelectedValue));
            //SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select Description  from AllergyMaster where  TypeID=@intTypeId and Description=@chvVitalName and HospitalLocationId=@intHospitallocationId", hs);

            string str = "select TOP 1 Description  from AllergyMaster where  TypeID=" + common.myInt(ddlAllergyType.SelectedValue).ToString() + " and Description='" + txtAllergyName.Text + "' and HospitalLocationId=" + Convert.ToInt16(Session["HospitalLocationID"]).ToString() + "";
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = str.ToString();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                lblMsg.Text = "Allergy Name Allready Exist!";
                txtAllergyName.Text = "";
                Alert.ShowAjaxMsg(lblMsg.Text, this.Page);
            }
            else
            {
                //String strret = objEMRAllergy.SaveAllergyMaster(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(ddlAllergyType.SelectedItem.Value), bc.ParseQ(txtAllergyName.Text.Trim()), Convert.ToInt32(Session["UserID"]));

                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveAllergyMaster";
                APIRootClass.SaveAllergyMaster objRoot1 = new global::APIRootClass.SaveAllergyMaster();
                objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
                objRoot1.TypeId = common.myInt(ddlAllergyType.SelectedItem.Value);
                objRoot1.Description = bc.ParseQ(txtAllergyName.Text.Trim());
                objRoot1.UserId = common.myInt(Session["UserID"]);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                string strret = JsonConvert.DeserializeObject<string>(sValue);

                if (strret.Length == 5)
                {
                    lblMsg.Text = "Record(s) Has Been Saved.";
                }
                else
                {
                    lblMsg.Text = "Error In Saving...";
                }
            }
            //Alert.ShowAjaxMsg(str, this.Page);
            //objDr.Close();
            BindAllergyGrid();
            txtAllergyName.Text = "";
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void gvAllergy_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[Convert.ToByte(GridAllergy.AllergyID)].Visible = false;

            if (ViewState["Blank"] != null)
            {
                e.Row.Cells[Convert.ToByte(GridAllergy.Edit)].Visible = false;
            }
        }
    }

    protected void gvAllergy_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeActivate")
            {
                //BaseC.EMRAllergy objAllergy = new BaseC.EMRAllergy(sConString);
                //int i = objAllergy.DeActivateAllergyMaster(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(e.CommandArgument), Convert.ToInt16(Session["UserID"]));

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeActivateAllergyMaster";
                APIRootClass.DeActivateAllergyMaster objRoot = new global::APIRootClass.DeActivateAllergyMaster();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
                objRoot.AllergyId = common.myInt(e.CommandArgument);
                objRoot.UserId = common.myInt(Session["UserID"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                int i = JsonConvert.DeserializeObject<int>(sValue);

                if (i == 0)
                {
                    lblMsg.Text = "Record De-Actived... ";
                    //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
                    gvAllergy.EditIndex = -1;
                    BindAllergyGrid();
                }
                else
                {
                    lblMsg.Text = "Error In De-Activation...";
                    //Alert.ShowAjaxMsg("Error In De-Activation... ", this.Page);
                    gvAllergy.EditIndex = -1;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void gvAllergy_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvAllergy.EditIndex = -1;
        BindAllergyGrid();
    }

    protected void gvAllergy_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string AllergyID = gvAllergy.Rows[e.RowIndex].Cells[Convert.ToByte(GridAllergy.AllergyID)].Text;
            TextBox Desc = (TextBox)gvAllergy.Rows[e.RowIndex].Cells[Convert.ToByte(GridAllergy.Description)].FindControl("txtGridAllergyName");
            String strStatus = "";
            clsIVF bc = new clsIVF(string.Empty);
            DropDownList strDrop = ((DropDownList)gvAllergy.Rows[e.RowIndex].Cells[1].FindControl("ddlGridStatus"));
            strStatus = strDrop.SelectedItem.Value;

            //BaseC.EMRAllergy objAllergy = new BaseC.EMRAllergy(sConString);
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet ds = new DataSet();
            //Hashtable hs = new Hashtable();
            //hs.Add("@chvVitalName", Convert.ToString(Desc.Text));
            //hs.Add("@AllergyID", Convert.ToInt32(AllergyID));
            //hs.Add("@intTypeId", common.myInt(ddlAllergyType.SelectedValue));

            //SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select Description  from AllergyMaster where TypeID=@intTypeId and Description=@chvVitalName and AllergyID!=@AllergyID", hs);

            string str = "select TOP 1 Description  from AllergyMaster where  TypeID=" + common.myInt(ddlAllergyType.SelectedValue).ToString() + " and Description='" + txtAllergyName.Text + "' and AllergyID!=" + Convert.ToInt32(AllergyID).ToString() + "";
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = str.ToString();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                lblMsg.Text = "Allergy Name Allready Exist!";
            }
            else
            {
                //Int32 i = objAllergy.UpdateAllergyMaster(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(AllergyID), Convert.ToInt16(hdnAllergyType.Value), bc.ParseQ(Desc.Text.ToString().Trim()), Convert.ToInt16(Session["UserID"]), Convert.ToByte(strStatus));
                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateAllergyMaster";
                APIRootClass.UpdateAllergyMaster objRoot1 = new global::APIRootClass.UpdateAllergyMaster();
                objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
                objRoot1.TypeId = common.myInt(hdnAllergyType.Value);
                objRoot1.AllergyId = common.myInt(AllergyID);
                objRoot1.Description = bc.ParseQ(Desc.Text);
                objRoot1.UserId = common.myInt(Session["UserID"]);
                objRoot1.IsActive = common.myBool(strStatus);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                int i = JsonConvert.DeserializeObject<int>(sValue);

                if (i == 0)
                {
                    lblMsg.Text = "Record(s) Updated... ";
                    //Alert.ShowAjaxMsg("Record(s) Updated... ", this.Page);
                    gvAllergy.EditIndex = -1;
                    BindAllergyGrid();
                }
                else
                {
                    lblMsg.Text = "Error In Updating...";
                    //Alert.ShowAjaxMsg("Error In Updating... ", this.Page);
                    gvAllergy.EditIndex = -1;
                }
            }
            //objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void gvAllergy_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvAllergy.PageIndex = e.NewPageIndex;
        BindAllergyGrid();
    }

    protected void gvAllergy_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvAllergy.EditIndex = e.NewEditIndex;
        BindAllergyGrid();
    }

    //for Allergy Type Master ///////////-------------------------------------------
    protected void gvAllergyType_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[Convert.ToByte(GridAllergyType.TypeID)].Visible = false;

            if (ViewState["Blank"] != null)
            {
                e.Row.Cells[Convert.ToByte(GridAllergyType.Edit)].Visible = false;
            }
        }
    }

    protected void gvAllergyType_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeActivate")
            {
                //BaseC.EMRAllergy objAllergy = new BaseC.EMRAllergy(sConString);
                //Int32 i = objAllergy.DeActivateAllergyTypeMaster(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(e.CommandArgument), Convert.ToInt16(Session["UserID"]));
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeActivateAllergyTypeMaster";
                APIRootClass.DeActivateAllergyTypeMaster objRoot = new global::APIRootClass.DeActivateAllergyTypeMaster();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
                objRoot.TypeId = common.myInt(e.CommandArgument);
                objRoot.UserId = common.myInt(Session["UserID"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                int i = JsonConvert.DeserializeObject<int>(sValue);
                if (i == 0)
                {
                    lblMsg.Text = "Record De-Actived... ";
                    //Alert.ShowAjaxMsg("Record De-Actived... ", this.Page);
                    gvAllergyType.EditIndex = -1;
                    BindAllergyTypeGrid();
                    PopulateAllergyType();
                }
                else
                {
                    lblMsg.Text = "Error In De-Activation...";

                    gvAllergyType.EditIndex = -1;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void gvAllergyType_OnRowCancelingEdit(Object sender, GridViewCancelEditEventArgs e)
    {
        gvAllergyType.EditIndex = -1;
        BindAllergyTypeGrid();
    }

    protected void gvAllergyType_OnRowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string TypeID = gvAllergyType.Rows[e.RowIndex].Cells[Convert.ToByte(GridAllergyType.TypeID)].Text;
            TextBox Desc = (TextBox)gvAllergyType.Rows[e.RowIndex].Cells[Convert.ToByte(GridAllergyType.Description)].FindControl("txtGridAllergyName");
            String strStatus = "";

            DropDownList strDrop = ((DropDownList)gvAllergyType.Rows[e.RowIndex].Cells[Convert.ToByte(GridAllergyType.Status)].FindControl("ddlGridStatus"));
            strStatus = strDrop.SelectedValue;
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            //Hashtable hs = new Hashtable();
            //hs.Add("@chvVitalName", Convert.ToString(Desc.Text));
            //hs.Add("@TypeID", Convert.ToInt32(TypeID));
            //SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select TypeName from AllergyType where  TypeName =@chvVitalName and TypeID!=@TypeID", hs);

            string str = "select TOP 1 TypeName from AllergyType where  TypeName='" + Desc.Text + "' and TypeID!=" + Convert.ToInt32(TypeID).ToString() + "";
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = str.ToString();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                lblMsg.Text = "Allergy Type Name Allready Exist!";
                txtAddNewAllergyType.Text = "";
                Alert.ShowAjaxMsg(lblMsg.Text, this.Page);
            }
            else
            {
                clsIVF bc = new clsIVF(string.Empty);
                //BaseC.EMRAllergy objAllergy = new BaseC.EMRAllergy(sConString);
                //Int32 i = objAllergy.UpdateAllergyTypeMaster(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(TypeID), bc.ParseQ(Desc.Text.ToString().Trim()), Convert.ToInt16(Session["UserID"]), Convert.ToByte(strStatus));
                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/UpdateAllergyTypeMaster";
                APIRootClass.UpdateAllergyTypeMaster objRoot1 = new global::APIRootClass.UpdateAllergyTypeMaster();
                objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
                objRoot1.TypeId = common.myInt(TypeID); //common.myInt(hdnAllergyType.Value);
                objRoot1.Description = bc.ParseQ(Desc.Text);
                objRoot1.UserId = common.myInt(Session["UserID"]);
                objRoot1.IsActive = common.myBool(strStatus);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                int i = JsonConvert.DeserializeObject<int>(sValue);
                if (i == 0)
                {
                    lblMsg.Text = "Record(s) Updated... ";
                    //Alert.ShowAjaxMsg("Record(s) Updated... ", this.Page);
                    gvAllergyType.EditIndex = -1;
                    BindAllergyTypeGrid();
                    PopulateAllergyType();
                }
                else
                {
                    lblMsg.Text = "Error In Updating...";
                    //Alert.ShowAjaxMsg("Error In Updating... ", this.Page);
                    gvAllergyType.EditIndex = -1;
                }
            }
            //objDr.Close();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }

    protected void gvAllergyType_OnPageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvAllergyType.PageIndex = e.NewPageIndex;
        BindAllergyTypeGrid();
    }

    protected void gvAllergyType_OnRowEditing(Object sender, GridViewEditEventArgs e)
    {
        gvAllergyType.EditIndex = e.NewEditIndex;
        BindAllergyTypeGrid();
    }

    protected void SaveAllergyTypeMaster_OnClick(Object sender, EventArgs e)
    {
        try
        {
            //BaseC.EMRAllergy objEMRAllergy = new BaseC.EMRAllergy(sConString);
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            //Hashtable hs = new Hashtable();
            //hs.Add("intHospitallocationId", common.myInt(Session["HospitalLocationID"]));
            //hs.Add("@chvVitalName", txtAddNewAllergyType.Text.ToString());
            //SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "select TypeName from AllergyType where TypeName =@chvVitalName and HospitalLocationId=@intHospitallocationId", hs);

            string str = "select TOP 1 TypeName from AllergyType where  TypeName='" + txtAddNewAllergyType.Text + "' and HospitalLocationId =" + common.myInt(Session["HospitalLocationID"]).ToString() + "";
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = str.ToString();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);


            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
            {
                lblMsg.Text = "Allergy Type Name Allready Exist!";
                txtAddNewAllergyType.Text = "";
                Alert.ShowAjaxMsg(lblMsg.Text, this.Page);
            }
            else
            {
                clsIVF bc = new clsIVF(string.Empty);
                //String strret = objEMRAllergy.SaveAllergyTypeMaster(Convert.ToInt16(Session["HospitalLocationID"]), bc.ParseQ(txtAddNewAllergyType.Text.Trim()), Convert.ToInt32(Session["UserID"]));

                ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveAllergyTypeMaster";
                APIRootClass.SaveAllergyTypeMaster objRoot1 = new global::APIRootClass.SaveAllergyTypeMaster();
                objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
                objRoot1.Description = bc.ParseQ(txtAddNewAllergyType.Text);
                objRoot1.UserId = common.myInt(Session["UserID"]);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                string strret = JsonConvert.DeserializeObject<string>(sValue);


                if (strret.Length == 5)
                {
                    lblMsg.Text = "Record(s) Has Been Saved.";
                }
                else
                {
                    lblMsg.Text = "Error In Saving...";
                }
            }
            //objDr.Close();
            //Alert.ShowAjaxMsg(str, this.Page);
            BindAllergyTypeGrid();
            PopulateAllergyType();
            txtAddNewAllergyType.Text = "";
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
    }
    //-----------------------------------------------------------------------------------------------------

}