using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Problems_PriorityStatus : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //clsExceptionLog objException = new clsExceptionLog();
    private bool cStatus = false;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          
            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            }
            BindCommonTable();
        }
    }

    private void BindCommonTable()
    {
        try
        {
            if (Request.QueryString["prbls"] == "Severity")
            {
                lblTitle.Text = "Severity Name";
                BindSeverity();
            }
            else if (Request.QueryString["prbls"] == "Context")
            {
                lblTitle.Text = "Context Name";
                BindContext();
                btnSetOrder.Visible = false;
            }
            else if (Request.QueryString["prbls"] == "Quality")
            {
                lblTitle.Text = "Quality Name";
                BindQuality();
                btnSetOrder.Visible = false;
            }
            else if (Request.QueryString["prbls"] == "Duration")
            {
                lblTitle.Text = "Duration Name";
                BindDuration();
                btnSetOrder.Visible = false;
            }
            else if (Request.QueryString["prbls"] == "Location")
            {
                lblTitle.Text = "Location Name";
                BindLocation();
                btnSetOrder.Visible = false;
            }
            else if (Request.QueryString["prbls"] == "Condition")
            {
                lblTitle.Text = "Condition Name";
                BindCondition();
            }
            else if (Request.QueryString["prbls"] == "Onset")
            {
                lblTitle.Text = "Onset Name";
                BindOnset();
                btnSetOrder.Visible = false;
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

    private void BindCondition()
    {
        DataSet ds = new DataSet();
        try
        {
            gvDetails.DataSource = null;
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            //hshInput = new Hashtable();
            //hshInput.Add("@HospID", Convert.ToInt16(Session["HospitalLocationID"]));
            //ds = obj.FillDataSet(CommandType.Text, "SELECT Id,Description,SequenceNo  FROM EMRProblemsCondition where HospitalLocationId is  null or HospitalLocationId=@HospID and Active=1 order by SequenceNo asc", hshInput);
            string strSql = "SELECT Id,Description,SequenceNo  FROM EMRProblemsCondition where HospitalLocationId is  null or HospitalLocationId= " + common.myInt(Session["HospitalLocationID"]).ToString() + " and Active=1 order by SequenceNo asc";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
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
        finally
        {
            ds.Dispose();
        }
    }

    private void BindOnset()
    {
        DataSet ds = new DataSet();
        try
        {
            gvDetails.DataSource = null;
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshInput = new Hashtable();
            //hshInput = new Hashtable();
            //hshInput.Add("@HospID", Convert.ToInt16(Session["HospitalLocationID"]));
            //ds = obj.FillDataSet(CommandType.Text, "SELECT Id,Description,SequenceNo  FROM ProblemOnset where HospitalLocationId is  null or HospitalLocationId=@HospID and Active=1 order by Description", hshInput);
            string strSql = "SELECT Id,Description,SequenceNo  FROM ProblemOnset where HospitalLocationId is  null or HospitalLocationId= " + common.myInt(Session["HospitalLocationID"]).ToString() + "  and Active=1 order by Description";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
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
        finally
        {
            ds.Dispose();
        }
    }

    private void BindLocation()
    {
        DataSet ds = new DataSet();
        try
        {
            gvDetails.DataSource = null;
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshInput = new Hashtable();
            //hshInput = new Hashtable();
            //hshInput.Add("@HospID", Convert.ToInt16(Session["HospitalLocationID"]));
            //ds = obj.FillDataSet(CommandType.Text, "SELECT Id,Description,SequenceNo  FROM EMRProblemsLocation where HospitalLocationId is  null or HospitalLocationId=@HospID and Active=1 order by SequenceNo asc", hshInput);
            string strSql = "SELECT Id,Description,SequenceNo  FROM EMRProblemsLocation where HospitalLocationId is  null or HospitalLocationId= " + common.myInt(Session["HospitalLocationID"]).ToString() + "  and Active=1 order by SequenceNo asc";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
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
        finally
        {
            ds.Dispose();
        }
    }

    private void BindSeverity()
    {
        DataSet ds = new DataSet();
        try
        {
            gvDetails.DataSource = null;
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshInput = new Hashtable();
            //hshInput = new Hashtable();
            //hshInput.Add("@HospID", Convert.ToInt16(Session["HospitalLocationID"]));
            //ds = obj.FillDataSet(CommandType.Text, "SELECT Id,Description,SequenceNo  FROM EMRProblemsSeverity where HospitalLocationId is  null or HospitalLocationId=@HospID and Active=1 order by SequenceNo asc ", hshInput);
            string strSql = "SELECT Id,Description,SequenceNo  FROM EMRProblemsSeverity where HospitalLocationId is  null or HospitalLocationId= " + common.myInt(Session["HospitalLocationID"]).ToString() + "  and Active=1 order by SequenceNo asc";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
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
        finally
        {
            ds.Dispose();
        }
    }

    private void BindQuality()
    {
        DataSet ds = new DataSet();
        try
        {
            gvDetails.DataSource = null;
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshInput = new Hashtable();
            //hshInput = new Hashtable();
            //hshInput.Add("@HospID", Convert.ToInt16(Session["HospitalLocationID"]));
            //ds = obj.FillDataSet(CommandType.Text, "SELECT Id,Description,SequenceNo  FROM EMRProblemsQuality where HospitalLocationId is  null or HospitalLocationId=@HospID and Active=1 order by Description", hshInput);
            string strSql = "SELECT Id,Description,SequenceNo  FROM EMRProblemsQuality where HospitalLocationId is  null or HospitalLocationId= " + common.myInt(Session["HospitalLocationID"]).ToString() + "  and Active=1 order by Description asc";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
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
        finally
        {
            ds.Dispose();
        }
    }

    private void BindDuration()
    {
        DataSet ds = new DataSet();
        try
        {
            gvDetails.DataSource = null;
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshInput = new Hashtable();
            //hshInput = new Hashtable();
            //hshInput.Add("@HospID", Convert.ToInt16(Session["HospitalLocationID"]));
            //ds = obj.FillDataSet(CommandType.Text, "SELECT Id,Description,SequenceNo  FROM EMRProblemsDuration where HospitalLocationId is  null or HospitalLocationId=@HospID and Active=1 order by Description", hshInput);
            string strSql = "SELECT Id,Description,SequenceNo  FROM EMRProblemsDuration where HospitalLocationId is  null or HospitalLocationId= " + common.myInt(Session["HospitalLocationID"]).ToString() + "  and Active=1 order by Description asc";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
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
        finally
        {
            ds.Dispose();
        }
    }

    private void BindContext()
    {
        DataSet ds = new DataSet();
        try
        {
            gvDetails.DataSource = null;
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshInput = new Hashtable();
            //hshInput = new Hashtable();
            //hshInput.Add("@HospID", Convert.ToInt16(Session["HospitalLocationID"]));
            //ds = obj.FillDataSet(CommandType.Text, "SELECT Id,Description,SequenceNo  FROM EMRProblemsContext where HospitalLocationId is  null or HospitalLocationId=@HospID and Active=1 order by Description", hshInput);
            string strSql = "SELECT Id,Description,SequenceNo  FROM EMRProblemsContext where HospitalLocationId is  null or HospitalLocationId= " + common.myInt(Session["HospitalLocationID"]).ToString() + "  and Active=1 order by Description asc";
            //objDs = dl.FillDataSet(CommandType.Text, strSql);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRData";
            APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
            objRoot.Query = strSql;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDetails.DataSource = ds;
                gvDetails.DataBind();
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
        finally
        {
            ds.Dispose();
        }
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (txtValueName.Text.Trim() == "")
            {
                Alert.ShowAjaxMsg("Please enter Name", Page);
                return;
            }
            else
            {
                //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //Hashtable hshInput = new Hashtable();
                //Hashtable hshOutput = new Hashtable();
                //hshInput.Add("@inyHospitallocationId", Session["HospitalLocationID"].ToString());
                //hshInput.Add("@chvDescription", txtValueName.Text);
                //hshInput.Add("@intEncodedBy", Convert.ToInt32(Session["UserId"].ToString()));
                //hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProblemMasters";
                APIRootClass.SaveProblemMasters objRoot = new global::APIRootClass.SaveProblemMasters();
                objRoot.HospitallocationId = common.myInt(Session["HospitalLocationID"]);
                objRoot.Description = txtValueName.Text;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.Id = 0;

                if (Request.QueryString["prbls"] == "Severity")
                {
                    //hshInput.Add("@chvTableName", "EMRProblemsSeverity");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Saved!";
                    //BindSeverity();
                    objRoot.TableName = "EMRProblemsSeverity";
                }
                else if (Request.QueryString["prbls"] == "Context")
                {
                    //hshInput.Add("@chvTableName", "EMRProblemsContext");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Saved!";
                    //BindContext();
                    objRoot.TableName = "EMRProblemsContext";
                }
                else if (Request.QueryString["prbls"] == "Quality")
                {
                    //hshInput.Add("@chvTableName", "EMRProblemsQuality");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Saved!";
                    //BindQuality();
                    objRoot.TableName = "EMRProblemsQuality";
                }
                else if (Request.QueryString["prbls"] == "Duration")
                {
                    //hshInput.Add("@chvTableName", "EMRProblemsDuration");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Saved!";
                    //BindDuration();
                    objRoot.TableName = "EMRProblemsDuration";
                }
                else if (Request.QueryString["prbls"] == "Location")
                {
                    //hshInput.Add("@chvTableName", "EMRProblemsLocation");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Saved!";
                    //BindLocation();
                    objRoot.TableName = "EMRProblemsLocation";
                }
                else if (Request.QueryString["prbls"] == "Condition")
                {
                    //hshInput.Add("@chvTableName", "EMRProblemsCondition");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Saved!";
                    //BindCondition();
                    objRoot.TableName = "EMRProblemsCondition";
                }
                else if (Request.QueryString["prbls"] == "Onset")
                {
                    //hshInput.Add("@chvTableName", "ProblemOnset");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Saved!";
                    //BindOnset();
                    objRoot.TableName = "ProblemOnset";
                }
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dt = JsonConvert.DeserializeObject<DataTable>(sValue);
                lblMsg.Text = common.myStr(dt.Rows[0][0]);
                if (Request.QueryString["prbls"] == "Severity")
                {
                    BindSeverity();
                }
                else if (Request.QueryString["prbls"] == "Context")
                {
                    BindContext();
                }
                else if (Request.QueryString["prbls"] == "Quality")
                {
                    BindQuality();
                }
                else if (Request.QueryString["prbls"] == "Duration")
                {
                    BindDuration();
                }
                else if (Request.QueryString["prbls"] == "Location")
                {
                    BindLocation();
                }
                else if (Request.QueryString["prbls"] == "Condition")
                {
                    BindCondition();
                }
                else if (Request.QueryString["prbls"] == "Onset")
                {
                    BindOnset();
                }
                txtValueName.Text = "";
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
        finally
        {
            dt.Dispose();
        }
    }

    protected void gvDetails_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        GridDataItem editedItem = (GridDataItem)e.Item;
        TextBox LBL = (TextBox)editedItem.FindControl("txtID");
    }

    protected void gvDetails_ItemCommand(object source, GridCommandEventArgs e)
    {
        try
        {
            int selectedIdx = e.Item.ItemIndex;
            if (e.CommandName == "Delete1")
            {
                gvDetails.Items[selectedIdx].Selected = true;
                string id = ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text;
                lblMsg.Text = "";
                //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                string strdel = "";
                //obj.ExecuteNonQuery(CommandType.Text, strdel);

                
                if (Request.QueryString["prbls"] == "Severity")
                {
                    //String strdel = "UPDATE EMRProblemsSeverity SET Active=0 WHERE Id=" + id;
                    strdel = "UPDATE EMRProblemsSeverity SET Active=0 WHERE Id=" + id;
                    //obj.ExecuteNonQuery(CommandType.Text, strdel);
                    //lblMsg.Text = "You sucessfully delete Severity!";
                    //BindSeverity();

                }
                else if (Request.QueryString["prbls"] == "Context")
                {
                    //String strdel = "UPDATE EMRProblemsContext SET Active=0 WHERE Id=" + id;
                    strdel = "UPDATE EMRProblemsContext SET Active=0 WHERE Id=" + id;
                    //obj.ExecuteNonQuery(CommandType.Text, strdel);
                    //lblMsg.Text = "You sucessfully delete Severity!";
                    //BindContext();
                }
                else if (Request.QueryString["prbls"] == "Quality")
                {
                    //String strdel = "UPDATE EMRProblemsQuality SET Active=0 WHERE Id=" + id;
                    strdel = "UPDATE EMRProblemsQuality SET Active=0 WHERE Id=" + id;
                    //obj.ExecuteNonQuery(CommandType.Text, strdel);
                    //lblMsg.Text = "You sucessfully delete Severity!";
                    //BindQuality();
                }
                else if (Request.QueryString["prbls"] == "Duration")
                {
                    //String strdel = "UPDATE EMRProblemsDuration SET Active=0 WHERE Id=" + id;
                    strdel = "UPDATE EMRProblemsDuration SET Active=0 WHERE Id=" + id;
                    //obj.ExecuteNonQuery(CommandType.Text, strdel);
                    //lblMsg.Text = "You sucessfully delete Severity!";
                    //BindDuration();
                }
                else if (Request.QueryString["prbls"] == "Location")
                {
                    //String strdel = "UPDATE EMRProblemsLocation SET Active=0 WHERE Id=" + id;
                    strdel = "UPDATE EMRProblemsLocation SET Active=0 WHERE Id=" + id;
                    //obj.ExecuteNonQuery(CommandType.Text, strdel);
                    //lblMsg.Text = "You sucessfully delete Severity!";
                    //BindLocation();
                }
                else if (Request.QueryString["prbls"] == "Condition")
                {
                    //String strdel = "UPDATE EMRProblemsCondition SET Active=0 WHERE Id=" + id;
                    strdel = "UPDATE EMRProblemsCondition SET Active=0 WHERE Id=" + id;
                    //obj.ExecuteNonQuery(CommandType.Text, strdel);
                    //lblMsg.Text = "You sucessfully delete Severity!";
                    //BindCondition();
                }
                else if (Request.QueryString["prbls"] == "Onset")
                {
                    //String strdel = "UPDATE ProblemOnset SET Active=0 WHERE Id=" + id;
                    strdel = "UPDATE ProblemOnset SET Active=0 WHERE Id=" + id;
                    //obj.ExecuteNonQuery(CommandType.Text, strdel);
                    //lblMsg.Text = "You sucessfully delete Severity!"; ;
                    //BindOnset();
                }

                if (strdel != "")
                {
                    string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/ExecuteQuery";
                    APIRootClass.EMRModel objRoot = new global::APIRootClass.EMRModel();
                    objRoot.Query = strdel;
                    WebClient client = new WebClient();
                    client.Headers["Content-type"] = "application/json";
                    client.Encoding = Encoding.UTF8;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                    string sValue = client.UploadString(ServiceURL, inputJson);
                    sValue = JsonConvert.DeserializeObject<string>(sValue);
                    int intResult = common.myInt(sValue);
                }

                if (Request.QueryString["prbls"] == "Severity")
                {
                    lblMsg.Text = "You sucessfully delete Severity!";
                    BindSeverity();

                }
                else if (Request.QueryString["prbls"] == "Context")
                {
                    lblMsg.Text = "You sucessfully delete Severity!";
                    BindContext();
                }
                else if (Request.QueryString["prbls"] == "Quality")
                {
                    lblMsg.Text = "You sucessfully delete Severity!";
                    BindQuality();
                }
                else if (Request.QueryString["prbls"] == "Duration")
                {
                    lblMsg.Text = "You sucessfully delete Severity!";
                    BindDuration();
                }
                else if (Request.QueryString["prbls"] == "Location")
                {
                    lblMsg.Text = "You sucessfully delete Severity!";
                    BindLocation();
                }
                else if (Request.QueryString["prbls"] == "Condition")
                {
                    lblMsg.Text = "You sucessfully delete Severity!";
                    BindCondition();
                }
                else if (Request.QueryString["prbls"] == "Onset")
                {
                    lblMsg.Text = "You sucessfully delete Severity!";
                    BindOnset();
                }

            }
            else if (e.CommandName == "MoveUP")
            {
                if (selectedIdx > 0)
                {
                    gvDetails.Items[selectedIdx].Selected = true;

                    string strFieldID = ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text;
                    string strSeqNo = ((Label)gvDetails.SelectedItems[0].FindControl("hdnSeqNo")).Text;
                    string strFieldName = ((Label)gvDetails.SelectedItems[0].FindControl("lbltasktype")).Text;

                    gvDetails.Items[selectedIdx - 1].Selected = true;

                    string strFieldID1 = ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text;
                    string strSeqNo1 = ((Label)gvDetails.SelectedItems[0].FindControl("hdnSeqNo")).Text;
                    string strFieldName1 = ((Label)gvDetails.SelectedItems[0].FindControl("lbltasktype")).Text;

                    ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text = strFieldID;
                    ((Label)gvDetails.SelectedItems[0].FindControl("hdnSeqNo")).Text = strSeqNo;
                    ((Label)gvDetails.SelectedItems[0].FindControl("lbltasktype")).Text = strFieldName;

                    gvDetails.Items[selectedIdx].Selected = true;

                    ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text = strFieldID1;
                    ((Label)gvDetails.SelectedItems[0].FindControl("hdnSeqNo")).Text = strSeqNo1;
                    ((Label)gvDetails.SelectedItems[0].FindControl("lbltasktype")).Text = strFieldName1;

                    gvDetails.Items[selectedIdx - 1].Selected = true;
                    cStatus = true;
                }
                else
                    gvDetails.Items[selectedIdx].Selected = false;

            }
            else if (e.CommandName == "MoveDown")
            {
                if (selectedIdx < gvDetails.Items.Count - 1)
                {
                    gvDetails.Items[selectedIdx].Selected = true;

                    string strFieldID = ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text;
                    string strSeqNo = ((Label)gvDetails.SelectedItems[0].FindControl("hdnSeqNo")).Text;
                    string strFieldName = ((Label)gvDetails.SelectedItems[0].FindControl("lbltasktype")).Text;

                    gvDetails.Items[selectedIdx + 1].Selected = true;

                    string strFieldID1 = ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text;
                    string strSeqNo1 = ((Label)gvDetails.SelectedItems[0].FindControl("hdnSeqNo")).Text;
                    string strFieldName1 = ((Label)gvDetails.SelectedItems[0].FindControl("lbltasktype")).Text;

                    ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text = strFieldID;
                    ((Label)gvDetails.SelectedItems[0].FindControl("hdnSeqNo")).Text = strSeqNo;
                    ((Label)gvDetails.SelectedItems[0].FindControl("lbltasktype")).Text = strFieldName;

                    gvDetails.Items[selectedIdx].Selected = true;

                    ((Label)gvDetails.SelectedItems[0].FindControl("hdnFieldID")).Text = strFieldID1;
                    ((Label)gvDetails.SelectedItems[0].FindControl("hdnSeqNo")).Text = strSeqNo1;
                    ((Label)gvDetails.SelectedItems[0].FindControl("lbltasktype")).Text = strFieldName1;

                    gvDetails.Items[selectedIdx + 1].Selected = true;

                    cStatus = true;
                }
                else
                    gvDetails.Items[selectedIdx].Selected = false;
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
   
    protected void gvDetails_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editedItem = (GridEditFormItem)e.Item;
                String id = editedItem.GetDataKeyValue("ID").ToString();

                TextBox TXTName = (TextBox)editedItem.FindControl("txtstatusname1");
                //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //Hashtable hshInput = new Hashtable();
                //Hashtable hshOutput = new Hashtable();
                //hshInput.Add("@inyHospitallocationId", Session["HospitalLocationID"].ToString());
                //hshInput.Add("@chvDescription", TXTName.Text);
                //hshInput.Add("@intEncodedBy", Convert.ToInt32(Session["UserId"].ToString()));
                //hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProblemMasters";
                APIRootClass.SaveProblemMasters objRoot = new global::APIRootClass.SaveProblemMasters();
                objRoot.HospitallocationId = common.myInt(Session["HospitalLocationID"]);
                objRoot.Description = txtValueName.Text;
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.Id = common.myInt(id);
                if (Request.QueryString["prbls"] == "Severity")
                {
                    //hshInput.Add("@intId", id);
                    //hshInput.Add("@chvTableName", "EMRProblemsSeverity");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Update!";
                    //BindSeverity();
                    objRoot.TableName = "EMRProblemsSeverity";

                }
                else if (Request.QueryString["prbls"] == "Context")
                {
                    //hshInput.Add("@intId", id);
                    //hshInput.Add("@chvTableName", "EMRProblemsContext");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Update!";
                    //BindContext();
                    objRoot.TableName = "EMRProblemsContext";
                }
                else if (Request.QueryString["prbls"] == "Quality")
                {
                    //hshInput.Add("@intId", id);
                    //hshInput.Add("@chvTableName", "EMRProblemsQuality");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Update!";
                    //BindQuality();
                    objRoot.TableName = "EMRProblemsQuality";

                }
                else if (Request.QueryString["prbls"] == "Duration")
                {
                    //hshInput.Add("@intId", id);
                    //hshInput.Add("@chvTableName", "EMRProblemsDuration");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Update!";
                    //BindDuration();
                    objRoot.TableName = "EMRProblemsDuration";
                }
                else if (Request.QueryString["prbls"] == "Location")
                {
                    //hshInput.Add("@intId", id);
                    //hshInput.Add("@chvTableName", "EMRProblemsLocation");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Update!";
                    //BindLocation();
                    objRoot.TableName = "EMRProblemsLocation";
                }
                else if (Request.QueryString["prbls"] == "Condition")
                {
                    //hshInput.Add("@intId", id);
                    //hshInput.Add("@chvTableName", "EMRProblemsCondition");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Update!";
                    //BindCondition();
                    objRoot.TableName = "EMRProblemsCondition";
                }
                else if (Request.QueryString["prbls"] == "Onset")
                {
                    //hshInput.Add("@intId", id);
                    //hshInput.Add("@chvTableName", "ProblemOnset");
                    //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", hshInput, hshOutput);
                    //lblMsg.Text = "Record(s) Saved!";
                    //BindOnset();
                    objRoot.TableName = "ProblemOnset";
                }
                
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                dt = JsonConvert.DeserializeObject<DataTable>(sValue);
                lblMsg.Text = common.myStr(dt.Rows[0][0]);
                if (Request.QueryString["prbls"] == "Severity")
                {
                    BindSeverity();
                }
                else if (Request.QueryString["prbls"] == "Context")
                {
                    BindContext();
                }
                else if (Request.QueryString["prbls"] == "Quality")
                {
                    BindQuality();
                }
                else if (Request.QueryString["prbls"] == "Duration")
                {
                    BindDuration();
                }
                else if (Request.QueryString["prbls"] == "Location")
                {
                    BindLocation();
                }
                else if (Request.QueryString["prbls"] == "Condition")
                {
                    BindCondition();
                }
                else if (Request.QueryString["prbls"] == "Onset")
                {
                    BindOnset();
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
        finally
        {
            dt.Dispose();
        }
    }

    protected void gvDetails_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        BindCommonTable();
    }

    protected void gvDetails_CancelCommand(object source, GridCommandEventArgs e)
    {
        BindCommonTable();
    }

    protected void btnSetOrder_OnClick(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            lblMsg.Text = "";
            Label lblFieldNameGrid = null;
            Label hdnFieldIDGrid = null;
            Label hdnSeqNo = null;
            int i = 1;
            StringBuilder objStr = new StringBuilder();
            foreach (Telerik.Web.UI.GridItem item in gvDetails.Items)
            {
                lblFieldNameGrid = (Label)item.FindControl("lbltasktype");
                hdnFieldIDGrid = (Label)item.FindControl("hdnFieldID");
                hdnSeqNo = (Label)item.FindControl("hdnSeqNo");
                if (hdnFieldIDGrid.Text.Trim() != "")
                {
                    objStr.Append("<Table1>");
                    objStr.Append("<c1>");
                    objStr.Append(hdnFieldIDGrid.Text);
                    objStr.Append("</c1>");
                    objStr.Append("<c2>");
                    objStr.Append(i);
                    objStr.Append("</c2>");
                    objStr.Append("</Table1>");
                    i++;
                }
            }
            //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hshInput = new Hashtable();
            //Hashtable hshOutput = new Hashtable();
            //hshInput.Add("@xmlDetails", objStr.ToString());
            //hshInput.Add("@intEncodedBy", Convert.ToInt32(Session["UserId"].ToString()));
            //hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveProblemMastersSeq";
            APIRootClass.SaveProblemMastersSeq objRoot = new global::APIRootClass.SaveProblemMastersSeq();
            objRoot.xmlDetails = objStr.ToString();
            objRoot.UserId = common.myInt(Session["UserId"]);

            if (Request.QueryString["prbls"] == "Severity")
            {
                //hshInput.Add("@chvTableName", "EMRProblemsSeverity");
                //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMastersSeq", hshInput, hshOutput);
                //lblMsg.Text = "Set Order sucessfully!";
                //BindSeverity();
                objRoot.TableName = "EMRProblemsSeverity";
            }
            else if (Request.QueryString["prbls"] == "Context")
            {
                //hshInput.Add("@chvTableName", "EMRProblemsContext");
                //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMastersSeq", hshInput, hshOutput);
                //lblMsg.Text = "Set Order sucessfully!";
                //BindContext();
                objRoot.TableName = "EMRProblemsContext";
            }
            else if (Request.QueryString["prbls"] == "Quality")
            {
                //hshInput.Add("@chvTableName", "EMRProblemsQuality");
                //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMastersSeq", hshInput, hshOutput);
                //lblMsg.Text = "Set Order sucessfully!";
                //BindQuality();
                objRoot.TableName = "EMRProblemsQuality";
            }
            else if (Request.QueryString["prbls"] == "Duration")
            {
                //hshInput.Add("@chvTableName", "EMRProblemsDuration");
                //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMastersSeq", hshInput, hshOutput);
                //lblMsg.Text = "Set Order sucessfully!";
                //BindDuration();
                objRoot.TableName = "EMRProblemsDuration";
            }
            else if (Request.QueryString["prbls"] == "Location")
            {
                //hshInput.Add("@chvTableName", "EMRProblemsLocation");
                //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMastersSeq", hshInput, hshOutput);
                //lblMsg.Text = "Set Order sucessfully!";
                //BindLocation();
                objRoot.TableName = "EMRProblemsLocation";
            }
            else if (Request.QueryString["prbls"] == "Condition")
            {
                //hshInput.Add("@chvTableName", "EMRProblemsCondition");
                //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMastersSeq", hshInput, hshOutput);
                //lblMsg.Text = "Set Order sucessfully!";
                //BindCondition();
                objRoot.TableName = "EMRProblemsCondition";
            }
            else if (Request.QueryString["prbls"] == "Onset")
            {
                //hshInput.Add("@chvTableName", "EMRProblemsCondition");
                //hshOutput = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMastersSeq", hshInput, hshOutput);
                //lblMsg.Text = "Set Order sucessfully!";
                //BindOnset();
                objRoot.TableName = "ProblemOnset";
            }
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dt = JsonConvert.DeserializeObject<DataTable>(sValue);
            lblMsg.Text = common.myStr(dt.Rows[0][0]);
            if (Request.QueryString["prbls"] == "Severity")
            {
                BindSeverity();
            }
            else if (Request.QueryString["prbls"] == "Context")
            {
                BindContext();
            }
            else if (Request.QueryString["prbls"] == "Quality")
            {
                BindQuality();
            }
            else if (Request.QueryString["prbls"] == "Duration")
            {
                BindDuration();
            }
            else if (Request.QueryString["prbls"] == "Location")
            {
                BindLocation();
            }
            else if (Request.QueryString["prbls"] == "Condition")
            {
                BindCondition();
            }
            else if (Request.QueryString["prbls"] == "Onset")
            {
                BindOnset();
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
        finally
        {
            dt.Dispose();
        }
    }

    protected void gvDetails_PreRender(object sender, EventArgs e)
    {
        if (cStatus == false)
        {
            BindCommonTable();
        }
    }

    protected void gvDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            if (Request.QueryString["prbls"] == "Context")
            {
                dataItem.Cells[5].Visible = false;
                dataItem.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Quality")
            {
                dataItem.Cells[5].Visible = false;
                dataItem.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Duration")
            {
                dataItem.Cells[5].Visible = false;
                dataItem.Cells[6].Visible = false;
            }

            if (Request.QueryString["prbls"] == "Location")
            {
                dataItem.Cells[5].Visible = false;
                dataItem.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Onset")
            {
                dataItem.Cells[5].Visible = false;
                dataItem.Cells[6].Visible = false;
            }
        }
        if (e.Item is GridFilteringItem)
        {
            GridFilteringItem dataFiltering = (GridFilteringItem)e.Item;
            if (Request.QueryString["prbls"] == "Context")
            {
                dataFiltering.Cells[5].Visible = false;
                dataFiltering.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Quality")
            {
                dataFiltering.Cells[5].Visible = false;
                dataFiltering.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Duration")
            {
                dataFiltering.Cells[5].Visible = false;
                dataFiltering.Cells[6].Visible = false;
            }

            if (Request.QueryString["prbls"] == "Location")
            {
                dataFiltering.Cells[5].Visible = false;
                dataFiltering.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Onset")
            {
                dataFiltering.Cells[5].Visible = false;
                dataFiltering.Cells[6].Visible = false;
            }
        }
        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem header = (GridHeaderItem)e.Item;
            if (Request.QueryString["prbls"] == "Context")
            {
                header.Cells[5].Visible = false;
                header.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Quality")
            {
                header.Cells[5].Visible = false;
                header.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Duration")
            {
                header.Cells[5].Visible = false;
                header.Cells[6].Visible = false;
            }

            if (Request.QueryString["prbls"] == "Location")
            {
                header.Cells[5].Visible = false;
                header.Cells[6].Visible = false;
            }
            if (Request.QueryString["prbls"] == "Onset")
            {
                header.Cells[5].Visible = false;
                header.Cells[6].Visible = false;
            }
        }
    }
}