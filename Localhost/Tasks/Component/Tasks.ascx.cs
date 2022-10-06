using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using BaseC.Task;
using System.Configuration;
using System.Collections;
using System.Text;
using Telerik.Web.UI;
using System.Web.Services;
using System.Drawing;
using System.Data.SqlClient;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class Tasks_Component_Tasks : System.Web.UI.UserControl
{
    #region Private Function
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    class MyEncounter
    {
        int _encounterid;

        public int Encounterid
        {
            get { return _encounterid; }
            set { _encounterid = value; }
        }
        string _encounterDetails;

        public string EncounterDetails
        {
            get { return _encounterDetails; }
            set { _encounterDetails = value; }
        }
    }

    private enum Gvtask : byte
    {
        TaskType = 0,
        CreatedDate = 1,
        DueTime = 2,
        TaskStatus = 3,
        PatientName = 4,
        TaskPriority = 5,
        assignedby = 6,
        TaskId = 7,
        pageurl = 8,
        StatusColor = 10


    }
    private enum Gvposted : byte
    {
        TaskType = 0,
        CreatedDate = 1,
        DueTime = 2,
        TaskStatus = 3,
        PatientName = 4,
        TaskPriority = 5,
        pageurl = 6,
        StatusColor = 10

    }
    private void Blankgridtask()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TaskType");
        dt.Columns.Add("CreatedDate");
        dt.Columns.Add("DueTime");
        dt.Columns.Add("TaskStatus");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("TaskPriority");
        dt.Columns.Add("assignedby");
        dt.Columns.Add("TaskId");
        dt.Columns.Add("pageurl");
        dt.Columns.Add("StatusColor");
        for (int i = 0; i < 4; i++)
        {
            DataRow dr = dt.NewRow();
            dr["TaskType"] = "";
            dr["CreatedDate"] = "";
            dr["DueTime"] = "";
            dr["TaskStatus"] = "";
            dr["PatientName"] = "";
            dr["TaskPriority"] = "";
            dr["assignedby"] = "";
            dr["TaskId"] = "";
            dr["pageurl"] = "";
            dr["StatusColor"] = "Blank";
            dt.Rows.Add(dr);
        }

        ViewState["BlankGrid"] = "True";
        gvTasks.DataSource = dt;
        gvTasks.DataBind();
        ViewState["BlankGrid"] = null;


    }
    private void Blankgridpost()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TaskType");
        dt.Columns.Add("CreatedDate");
        dt.Columns.Add("DueTime");
        dt.Columns.Add("TaskStatus");
        dt.Columns.Add("PatientName");
        dt.Columns.Add("TaskPriority");
        dt.Columns.Add("pageurl");
        dt.Columns.Add("Active");
        dt.Columns.Add("StatusColor");

        dt.Columns.Add("TaskId");
        for (int i = 0; i < 4; i++)
        {
            DataRow dr = dt.NewRow();
            dr["TaskType"] = "";
            dr["CreatedDate"] = "";
            dr["DueTime"] = "";
            dr["TaskStatus"] = "";
            dr["PatientName"] = "";
            dr["TaskPriority"] = "";
            dr["pageurl"] = "";
            dr["Active"] = "";
            dr["TaskId"] = "";
            dr["StatusColor"] = "Blank";
            dt.Rows.Add(dr);
        }


        ViewState["BlankGrid"] = "True";
        grvPosted.DataSource = dt;
        grvPosted.DataBind();
        ViewState["BlankGrid"] = null;


    }
    //protected void RadComboBoxProduct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    //{

    //    ComboPatientSearch.Items.Clear();
    //    DataTable dataTable = GetData(e.Text);


    //    foreach (DataRow dataRow in dataTable.Rows)
    //    {
    //        RadComboBoxItem item = new RadComboBoxItem();

    //        item.Text = (string)dataRow["PatientName"];
    //        item.Value = dataRow["RegistrationId"].ToString();

    //        // string PatientName = dataRow["Account"].ToString();
    //        //string DOB = dataRow["DOB"].ToString();

    //        item.Attributes.Add("RegistrationNo", dataRow["RegistrationNo"].ToString());
    //        item.Attributes.Add("DOB", dataRow["DOB"].ToString());
    //        item.Attributes.Add("Gender", dataRow["Gender"].ToString());
    //        item.Attributes.Add("PhoneHome", dataRow["PhoneHome"].ToString());
    //        //item.Value += ":" + unitPrice;

    //        RadComboBox2.Items.Add(item);

    //        item.DataBind();
    //    }
    //}
    //private DataTable GetData(string text)
    //{
    //    string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //    DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    Hashtable hashtable = new Hashtable();
    //    hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
    //    hashtable.Add("@strName", text);
    //    DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "SearchPatientByName", hashtable);
    //    return dataset.Tables[0];
    //}
    private void ClearForm()
    {
        txtCPerson.Text = "";
        txtCPPhone.Text = "";
        txtNote.Text = "";
        txtpatientName.Text = "";
        txtpNumber.Text = "";
        TextBox1.Text = "";
        chlstEmployee.SelectedIndex = -1;
        chklstempgrp.SelectedIndex = -1;
        txtAccountNumber.Text = "";
        ddlEncounter.SelectedIndex = 0;
        ddlPriority.SelectedIndex = 0;
        ddlTaskType.SelectedIndex = 0;
        hdnregno1.Value = "";
        rblstCpatient.SelectedIndex = -1;
        rblstcPerson.SelectedIndex = -1;
        RadDateTimePicker1.Clear();

        //RadDateTimePicker1.= RadDateTimePicker1.Custom;


    }


    private string getEmpID()
    {
        //BaseC.EMRMasters.EMRColorLegends EMRColorLegends = new BaseC.EMRMasters.EMRColorLegends(sConString);
        //SqlDataReader dr = EMRColorLegends.getEmployeeId(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserID"]));
        //string EmpId = "";
        //if (dr.HasRows == true)
        //{
        //    dr.Read();
        //    String strSelID = dr[0].ToString();
        //    if (strSelID != "")
        //    {
        //        char[] chArray = { ',' };
        //        string[] serviceIdXml = strSelID.Split(chArray);
        //        EmpId = serviceIdXml[0].ToString();
        //        ViewState["EmpId"] = EmpId;
        //    }
        //}
        //return EmpId;
        return common.myStr(Session["EmployeeId"]);
    }


    private void BindTaks()
    {
        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //hashtable.Add("@intEmpAssignedTo", getEmpID());
        //hashtable.Add("@intStatusId", Convert.ToInt32(ddlStatus1.SelectedValue.ToString()));

        //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);
        //Cache.Insert("Task_" + Session["UserID"].ToString(), dataset, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);

        //ViewState["Count"] = dataset.Tables[0].Rows.Count;
        //gvTasks.DataSource = dataset.Tables[0];
        //gvTasks.DataBind();

        //if (dataset.Tables[0].Rows.Count > 0)
        //{
        //    ViewState["Count"] = 0;


        //}
        //else
        //{
        //    Blankgridtask();
        //}


    }
    private void BindPost()
    {
        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //hashtable.Add("@intEmpAssignedBy", Session["UserId"].ToString());
        //hashtable.Add("@intStatusId", Convert.ToInt32(ddlstatus.SelectedValue.ToString()));


        //DataSet dataset1 = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);
        DataSet dataset1 = new DataSet();
        ViewState["Count1"] = dataset1.Tables[0].Rows.Count;
        grvPosted.DataSource = dataset1.Tables[0];
        grvPosted.DataBind();

        if (dataset1.Tables[0].Rows.Count > 0)
        {

            ViewState["Count1"] = 0;
        }
        else
        {
            Blankgridpost();
        }

    }
    private void BindDropDownList()
    {
        ////DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        ////DataSet dsobj = dl.FillDataSet(CommandType.Text, " Select StatusId, Status From GetStatus(1,'TaskPriority')");

        string ServiceURL = WebAPIAddress.ToString() + "api/Common/GetStatusMaster";

        APIRootClass.GetStatusMaster objRoot = new global::APIRootClass.GetStatusMaster();

        objRoot.StatusType = "TaskPriority";

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        DataSet dsobj = JsonConvert.DeserializeObject<DataSet>(sValue);

        ddlPriority.DataSource = dsobj;
        ddlPriority.DataTextField = "Status";
        ddlPriority.DataValueField = "StatusId";
        ddlPriority.DataBind();
        String str1 = dsobj.Tables[0].Rows[0]["Status"].ToString();
        if (str1 == "Normal")
        {
            lblstatus.Text = "";
        }
        else if (str1 == "Checkout Priority")
        {
            lblstatus.ForeColor = System.Drawing.Color.Green;
            lblstatus.Text = "Open";

        }
        else
        {
            lblstatus.ForeColor = System.Drawing.Color.Red;
            lblstatus.Text = "Open";

        }
        //dsobj = dl.FillDataSet(CommandType.Text, "Select Description,TaskCategoryId from TaskCategoriesMaster  where  HospitalLocationId=" + Session["HospitalLocationId"].ToString() + " and Active=1");
        //ddlTaskType.DataSource = dsobj;
        //ddlTaskType.DataTextField = "Description";
        //ddlTaskType.DataValueField = "TaskCategoryId";
        //ddlTaskType.DataBind();
        //dsobj = dl.FillDataSet(CommandType.Text, "Select Description,Id from EmployeeType where Active=1");
        //chklstempgrp.DataSource = dsobj;
        //chklstempgrp.DataTextField = "Description";
        //chklstempgrp.DataValueField = "Id";
        //chklstempgrp.DataBind();
        //Hashtable hashtable = new Hashtable();
        //hashtable.Add("@HospitalLocationId", Convert.ToInt32(Session["HospitalLocationId"]));
        //dsobj = dl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", hashtable);
        //chlstEmployee.DataSource = dsobj;
        //chlstEmployee.DataTextField = "EmployeeName";
        //chlstEmployee.DataValueField = "EmployeeId";
        //chlstEmployee.DataBind();
        //for (int i = 0; i < 60; i++)
        //{

        //    if (i.ToString().Length == 1)
        //    {
        //        RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
        //    }
        //    else
        //    {
        //        RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
        //    }
        //}

    }

    private void BindListBox()
    {
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //DataSet ds = new DataSet();
        //ds = dl.FillDataSet(CommandType.Text, " Select Description,TaskCategoryId  as TaskTypeId,Status =Case when Active=1 then 'Active' else 'In-Active' end, CONVERT(int,Active) as Active  from TaskCategoriesMaster where  HospitalLocationId=" + Session["HospitalLocationId"].ToString() + "");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    lstTypeList.DataSource = ds.Tables[0];

        //    lstTypeList.DataBind();
        //}
    }
    private void BindComment()
    {
        DataTable datatable = CreateCommentTable();
        DataRow datarow = datatable.NewRow();
        datatable.Rows.Add(datarow);
        gvComments.DataSource = datatable;
        gvComments.DataBind();
    }
    private DataTable CreateCommentTable()
    {
        DataTable datatable = new DataTable();
        datatable.Columns.Add("ID");
        datatable.Columns.Add("Comment");
        datatable.Columns.Add("UserName");
        datatable.Columns.Add("Status");
        return datatable;

    }


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (Session["UserID"] == null && Session["HospitalLocationID"] == null)
        //{
        //    Response.Redirect("~/login.aspx?Logout=1", false);
        //}
        //ddlActive.SelectedIndex = 0;
        if (!IsPostBack)
        {
            //if (Session["HospitalLocationID"] != null && Session["UserId"] != null)
            //{
            //    EmployeeId = GetEmployeeId();
            //}
            ////UserControl uc = (UserControl)Page.FindControl("ComboPatientSearch_Task");
            ////RadComboBox cmbPSearch = (RadComboBox)ComboPatientSearch_Task.FindControl("RadCmbPatientSearch");
            ////cmbPSearch.OnClientSelectedIndexChanged = "OnClientSelectedIndexChangedEventHandler_Task";

            //status();
            //status1();
            //statusset();
            //ddlstatusset.Visible = false;
            //BindListBox();
            //BindDropDownList();
            ////BindTaks();
            ////BindPost();
            //RadDateTimePicker1.Enabled = false;
            //RadDateTimePicker1.DateInput.DateFormat = Session["OutputDateFormat"].ToString() + " hh:mm tt";

            //string MyProrityCookieValue = "";
            //if (Request.Cookies["MyProrityCookie"] != null)
            //    MyProrityCookieValue = Request.Cookies["MyProrityCookie"].Value;
            //if (MyProrityCookieValue.ToString() != "")
            //{
            //    string[] strArr = new string[2];
            //    //char[] splitchar = { '~' };
            //    strArr = MyProrityCookieValue.ToString().Split('~');

            //    if (Convert.ToInt32(strArr[0]) != 0)
            //    {
            //        lblPriorityMessageHighPriority.Text = Server.HtmlDecode(" Open High Priority Task(s) : <font color='red'>" + strArr[0].ToString() + "</font>");
            //    }
            //    if (Convert.ToInt32(strArr[1]) != 0)
            //    {
            //        lblPriorityMessageCheckOutPriority.Text = Server.HtmlDecode(" Open Checkout Priority Task(s) :  <font color='red'>" + strArr[1].ToString() + "</font>");
            //    }
            //    Response.Cookies["MyProrityCookie"].Expires = DateTime.Now.AddDays(-1);
            //    pnlAddType.Visible = false;
            //    pnlAssigned.Visible = false;
            //    pnlNewTask.Visible = false;
            //    pnlPosted.Visible = false;
            //    pnlPriorityMessage.Visible = true;
            //}
            //if (Request.QueryString["TaskID"] != null)
            //{
            //    pnlAddType.Visible = false;
            //    pnlAssigned.Visible = false;
            //    pnlNewTask.Visible = true;
            //    pnlPosted.Visible = false;
            //    pnlPriorityMessage.Visible = false;
            //    BindPatientTask();
            //}


        }
        //if (ViewState["taskstatusA"] != null)
        //{
        //    lblstatus.Text = ViewState["taskstatusA"].ToString();
        //    ddlstatusset.SelectedItem.Text = ViewState["taskstatusA"].ToString();
        //}

    }

    private Int32 GetEmployeeId()
    {
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //DataSet ds = new DataSet();
        //string sQuery = "Select  dbo.GetUserEmployeeId  (@inyHospitalLocationId,@UserId) as EmpId";
        //Hashtable hshIn = new Hashtable();
        //hshIn.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hshIn.Add("@UserId", Session["UserId"]);
        //ds = dl.FillDataSet(CommandType.Text, sQuery, hshIn);
        //string[] strArr = new string[2];
        //char[] splitchar = { ',' };
        //strArr = ds.Tables[0].Rows[0]["EmpId"].ToString().Split(splitchar);

        //return Convert.ToInt32(strArr[0]);

        return common.myInt(Session["EmployeeId"]);

    }

    private Int32 EmployeeId
    {

        get
        {
            return (ViewState["EmpId"] == null ? 0 : Convert.ToInt32(ViewState["EmpId"]));
        }

        set
        {
            ViewState["EmpId"] = value;
        }
    }

    #region TaskTypeGrid
    protected void lstTypeList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lstTypeList.EditIndex = -1;
        BindListBox();
    }
    protected void lstTypeList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lstTypeList.EditIndex = e.NewEditIndex;
        BindListBox();
    }

    protected void lstTypeList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //string strStatus = "";
        //TaskTypes tasktype = new TaskTypes();
        //tasktype.TaskTypeId = Convert.ToInt32(lstTypeList.DataKeys[e.RowIndex].Value);
        //tasktype.EncodeBy = Convert.ToInt32(Session["UserId"]);
        //tasktype.Description = ((TextBox)lstTypeList.Rows[e.RowIndex].Cells[0].FindControl("txttasktupe")).Text.Trim();
        //tasktype.HospitalLoctionId = Convert.ToInt32(Session["HospitalLocationId"]);
        //DropDownList strDrop = ((DropDownList)lstTypeList.Rows[e.RowIndex].Cells[2].FindControl("ddlGridStatus"));
        //strStatus = strDrop.SelectedItem.Value;
        //tasktype.Active = Convert.ToInt16(strStatus);

        //try
        //{

        //    txtNewType.Text = "";
        //    string stmsg = tasktype.SaveTaskType();
        //    if (stmsg == "VarChar")
        //    {
        //        lblMsg.Text = "Updated";

        //    }
        //    else
        //    {
        //        lblMsg.Text = stmsg;
        //    }
        //    btnAdd.Visible = true;
        //    //  btnUpdate.Visible = false;

        //}
        //catch (Exception ex)
        //{
        //    lblMsg.Text = ex.Message;
        //}
        //finally
        //{
        //    tasktype = null;
        //    lstTypeList.EditIndex = -1;
        //    BindListBox();

        //}
    }
    protected void lstTypeList_RowDataBound(object sender, GridViewRowEventArgs e)
    {



    }

    #endregion

    #region Assigned to Grid Event
    protected void gvTasks_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTasks.PageIndex = e.NewPageIndex;

        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //hashtable.Add("@intEmpAssignedTo", EmployeeId);
        //hashtable.Add("@intStatusId", Convert.ToInt32(ddlStatus1.SelectedValue.ToString()));
        //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);

        //gvTasks.DataSource = dataset.Tables[0];
        gvTasks.DataBind();

    }
    protected void gvTasks_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ClearForm();
        //ddlEncounter.Items.Clear();
        //txtpNumber.Items.Clear();
        //lbltaskMsg.Text = string.Empty;
        ////DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        ////Hashtable hashtable = new Hashtable();
        ////hashtable.Add("@intTaskId", gvTasks.DataKeys[gvTasks.SelectedIndex].Value);
        ////hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        ////hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        ////DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);
        //DataSet dataset = new DataSet();
        //DataRow datarow = dataset.Tables[0].Rows[0];
        //hdnTaskId.Value = gvTasks.DataKeys[gvTasks.SelectedIndex].Value.ToString();
        //ddlTaskType.SelectedIndex = ddlTaskType.FindItemIndexByText(datarow["TaskType"].ToString());

        //// ddlTaskType.Items.FindItemIndexByText(datarow["TaskType"].ToString());
        //ddlPriority.SelectedIndex = ddlPriority.Items.FindItemIndexByText(datarow["TaskPriority"].ToString());
        //txtNote.Text = datarow["TaskMessage"].ToString();
        //if (datarow["PatientName"] != DBNull.Value)
        //{
        //    txtpatientName.Text = datarow["PatientName"].ToString();
        //}
        //if (datarow["RegistrationNo"] != DBNull.Value)
        //{
        //    txtAccountNumber.Text = datarow["RegistrationNo"].ToString();
        //}
        //if (Convert.ToBoolean(datarow["PatientContactAllowed"].ToString()) == false)
        //{
        //    rblstCpatient.SelectedIndex = 1;
        //}
        //else
        //{
        //    rblstCpatient.SelectedIndex = 0;
        //}
        //if (Convert.ToBoolean(datarow["PersonContactAllowed"].ToString()) == false)
        //{
        //    rblstcPerson.SelectedIndex = 1;
        //}
        //else
        //{
        //    rblstcPerson.SelectedIndex = 0;
        //}
        ////// if (Convert.ToBoolean(datarow["DueNow"].ToString()) == true)
        ////// {
        ////     rbkduenow.Checked = false;
        ////     rbduedate.Checked = false;
        ////     RadDateTimePicker1.Enabled = false;
        ////     RadComboBox1.Enabled = false;
        //// //}

        //rbduedate.Checked = true;
        //rbkduenow.Checked = false;
        //RadDateTimePicker1.Enabled = true;
        //RadComboBox1.Enabled = true;
        //RadDateTimePicker1.SelectedDate = Convert.ToDateTime(datarow["DueDate"].ToString());


        //hdnregno1.Value = datarow["RegistrationId"].ToString();
        //txtCPerson.Text = datarow["ContactPersonName"].ToString();
        //txtCPPhone.Text = datarow["ContactPersonPhone"].ToString();

        //lblstatus.Text = datarow["TaskStatus"].ToString();
        //if (datarow["StatusId"] != null)
        //{
        //    ddlstatusset.SelectedValue = datarow["StatusId"].ToString();
        //}

        //DataTable datatableemp = dataset.Tables[1];
        //foreach (DataRow dr in datatableemp.Rows)
        //{
        //    chlstEmployee.Items[chlstEmployee.Items.IndexOf(chlstEmployee.Items.FindByValue(dr[0].ToString()))].Selected = true;
        //}
        //DataTable datatableempgrp = dataset.Tables[2];
        //foreach (DataRow dr in datatableempgrp.Rows)
        //{
        //    chklstempgrp.Items[chklstempgrp.Items.IndexOf(chklstempgrp.Items.FindByValue(dr[0].ToString()))].Selected = true;
        //}
        //txtpNumber.Items.Add(new RadComboBoxItem("Select", "0"));
        //if (dataset.Tables[3].Rows.Count > 0)
        //{
        //    txtpNumber.Items.Add(new RadComboBoxItem("C: " + dataset.Tables[3].Rows[0]["Mobile"].ToString()));
        //    txtpNumber.Items.Add(new RadComboBoxItem("P: " + dataset.Tables[3].Rows[0]["Phone"].ToString()));
        //    txtpNumber.Items.Add(new RadComboBoxItem("W:" + dataset.Tables[3].Rows[0]["WorkNumber"].ToString()));
        //}
        //txtpNumber.SelectedIndex = txtpNumber.FindItemIndexByText(datarow["PatientContactNo"].ToString());
        //Hashtable hshin1 = new Hashtable();
        //hshin1.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
        //hshin1.Add("@intRegistrationId", hdnregno1.Value);
        //DataSet dsobj = objSave.FillDataSet(CommandType.StoredProcedure, "UspSearchEncounter", hshin1);
        //ArrayList arr = new ArrayList();
        //MyEncounter myencounter;
        //foreach (DataRow dr in dsobj.Tables[0].Rows)
        //{
        //    myencounter = new MyEncounter();
        //    myencounter.Encounterid = Convert.ToInt32(dr["EncounterId"].ToString());
        //    myencounter.EncounterDetails = dr["EncounterNo"].ToString() + ", " + dr["DoctorName"].ToString() + ", " + dr["Contact"].ToString();
        //    arr.Add(myencounter);
        //}



        //ddlEncounter.DataSource = arr;
        //ddlEncounter.DataValueField = "Encounterid";
        //ddlEncounter.DataTextField = "EncounterDetails";
        //ddlEncounter.DataBind();
        //ddlEncounter.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        //ddlEncounter.SelectedIndex = ddlEncounter.FindItemIndexByValue(datarow["EncounterId"].ToString());
        //Save.Text = "Update";
        //pnlNewTask.Visible = true;
        //pnlAssigned.Visible = false;
        //gvComments.DataSource = dataset.Tables[4];
        //gvComments.DataBind();
        //DataSet ds1 = new DataSet();
        //ds1 = objSave.FillDataSet(CommandType.Text, "select (FirstName + ' ' + MiddleName + ' ' + LastName) as assignedby   from  Employee where ID=" + Session["UserID"].ToString());
        //txtassignedby.Text = dataset.Tables[0].Rows[0]["assignedby"].ToString();
        //ddlstatusset.Visible = true;
        //ltrlLink.Text = Convert.ToString(datarow["pageurl"]);
        ////btndelete.Visible = true;
        ////btnworking.Visible = true;
        ////btncompleted.Visible = true;
    }
    #endregion

    #region Posted Grid Event
    protected void grvPosted_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Byte A;
        Byte R;
        Byte G;
        Byte B;
        String htmlHexColorValue = "";
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (ViewState["BlankGrid"] != null)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;


            }
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[Convert.ToByte(Gvtask.StatusColor)].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[9].Text.Trim() == "&nbsp;")
            {
                e.Row.Cells[8].Visible = false;
            }
            if (ViewState["Blank"] == null)
            {
                String PriorityColor = e.Row.Cells[Convert.ToByte(Gvposted.StatusColor)].Text.ToString().Trim();
                Label lbl_PriorityColor = new Label();

                if (PriorityColor.ToString().Trim() != "Blank" && PriorityColor.ToString().Trim() != "&nbsp;" && PriorityColor.ToString().Trim() != "" && PriorityColor.ToString().Trim() != "StatusColor")
                {
                    lbl_PriorityColor.BackColor = System.Drawing.ColorTranslator.FromHtml(PriorityColor.Trim());

                    A = lbl_PriorityColor.BackColor.A;
                    B = lbl_PriorityColor.BackColor.B;
                    R = lbl_PriorityColor.BackColor.R;
                    G = lbl_PriorityColor.BackColor.G;
                    htmlHexColorValue = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(A, R, G, B));
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlHexColorValue.ToString());
                    }
                }
            }
            if (e.Row.Cells[9].Text == " ")
            {
                LinkButton lbl = (LinkButton)e.Row.FindControl("lnkOpenTask");

                lbl.Visible = false;


            }
            else
            {
                LinkButton lbl = (LinkButton)e.Row.FindControl("lnkOpenTask");

                lbl.Visible = true;

            }
        }
    }
    protected void grvPosted_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //Tasks task = new Tasks();
        //task.TasksId = Convert.ToInt32(grvPosted.DataKeys[e.RowIndex].Value);
        //int result = task.DeleteTask();
        //if (result == 0)
        //{
        //    lblPostedMsg.Text = "Deleted";
        //    DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    Hashtable hashtable = new Hashtable();
        //    hashtable = new Hashtable();
        //    hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //    hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //    hashtable.Add("@intEmpAssignedBy", Session["UserId"].ToString());
        //    hashtable.Add("@intStatusId", Convert.ToInt32(ddlstatus.SelectedValue.ToString()));
        //    DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);
        //    ViewState["Count1"] = dataset.Tables[0].Rows.Count;
        //    grvPosted.DataSource = dataset.Tables[0];
        //    grvPosted.DataBind();
        //    if (dataset.Tables[0].Rows.Count > 0)
        //    {
        //        ViewState["Count1"] = 0;
        //    }

        //}

    }
    protected void grvPosted_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grvPosted.PageIndex = e.NewPageIndex;
        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //hashtable.Add("@intEmpAssignedBy", Session["UserId"].ToString());
        //hashtable.Add("@intStatusId", Convert.ToInt32(ddlstatus.SelectedValue.ToString()));
        //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);
        //ViewState["Count1"] = dataset.Tables[0].Rows.Count;
        //grvPosted.DataSource = dataset.Tables[0];
        //grvPosted.DataBind();

        //if (dataset.Tables[0].Rows.Count > 0)
        //{
        //    ViewState["Count1"] = 0;
        //}

    }
    protected void grvPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlstatusset.Visible = true;
        ////btndelete.Visible =true;
        ////btnworking.Visible =true;
        ////btncompleted.Visible = true;
        //ClearForm();
        //ddlEncounter.Items.Clear();
        //txtpNumber.Items.Clear();
        //lbltaskMsg.Text = string.Empty;
        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //hashtable.Add("@intTaskId", grvPosted.DataKeys[grvPosted.SelectedIndex].Value);
        //hashtable.Add("@intStatusId", Convert.ToInt32(ddlstatus.SelectedValue.ToString()));
        //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);
        //DataRow datarow = dataset.Tables[0].Rows[0];
        //hdnTaskId.Value = grvPosted.DataKeys[grvPosted.SelectedIndex].Value.ToString();

        //ddlTaskType.SelectedIndex = ddlTaskType.FindItemIndexByText(datarow["TaskType"].ToString());

        //// ddlTaskType.Items.FindItemIndexByText(datarow["TaskType"].ToString());
        //ddlPriority.SelectedIndex = ddlPriority.Items.FindItemIndexByText(datarow["TaskPriority"].ToString());

        ////if (datarow["Status"] != "")
        ////{
        ////    ddlActive.SelectedValue= datarow["Status"].ToString();
        ////}
        //txtNote.Text = datarow["TaskMessage"].ToString();
        //if (datarow["AssignedBy"] != null && datarow["CreatedDate"] != null)
        //{

        //    txtassignedby.Text = datarow["AssignedBy"].ToString() + " " + datarow["CreatedDate"].ToString();

        //}
        //if (datarow["PatientName"] != DBNull.Value)
        //{
        //    txtpatientName.Text = datarow["PatientName"].ToString();
        //}
        //if (datarow["RegistrationNo"] != DBNull.Value)
        //{
        //    txtAccountNumber.Text = datarow["RegistrationNo"].ToString();
        //}
        //if (Convert.ToBoolean(datarow["PatientContactAllowed"].ToString()) == false)
        //{
        //    rblstCpatient.SelectedIndex = 1;
        //}
        //else
        //{
        //    rblstCpatient.SelectedIndex = 0;
        //}
        //if (Convert.ToBoolean(datarow["PersonContactAllowed"].ToString()) == false)
        //{
        //    rblstcPerson.SelectedIndex = 1;
        //}
        //else
        //{
        //    rblstcPerson.SelectedIndex = 0;
        //}
        //if (Convert.ToBoolean(datarow["DueNow"].ToString()) == true)
        //{
        //    rbkduenow.Checked = true;
        //    rbduedate.Checked = false;
        //    RadDateTimePicker1.Enabled = false;
        //    RadComboBox1.Enabled = false;
        //}
        //else
        //{
        //    rbduedate.Checked = true;
        //    rbkduenow.Checked = false;
        //    RadDateTimePicker1.Enabled = true;
        //    RadComboBox1.Enabled = true;
        //    RadDateTimePicker1.SelectedDate = Convert.ToDateTime(datarow["DueDate"].ToString());
        //}

        //hdnregno1.Value = datarow["RegistrationId"].ToString();
        //txtCPerson.Text = datarow["ContactPersonName"].ToString();
        //txtCPPhone.Text = datarow["ContactPersonPhone"].ToString();


        //lblstatus.Text = datarow["TaskStatus"].ToString();
        //if (datarow["StatusId"] != null)
        //{
        //    ddlstatusset.SelectedValue = datarow["StatusId"].ToString();
        //}

        //DataTable datatableemp = dataset.Tables[1];
        //foreach (DataRow dr in datatableemp.Rows)
        //{
        //    chlstEmployee.Items[chlstEmployee.Items.IndexOf(chlstEmployee.Items.FindByValue(dr[0].ToString()))].Selected = true;
        //}
        //DataTable datatableempgrp = dataset.Tables[2];
        //foreach (DataRow dr in datatableempgrp.Rows)
        //{
        //    chklstempgrp.Items[chklstempgrp.Items.IndexOf(chklstempgrp.Items.FindByValue(dr[0].ToString()))].Selected = true;
        //}
        //txtpNumber.Items.Add(new RadComboBoxItem("Select", "0"));
        //if (dataset.Tables[3].Rows.Count > 0)
        //{
        //    txtpNumber.Items.Add(new RadComboBoxItem("C: " + dataset.Tables[3].Rows[0]["Mobile"].ToString()));
        //    txtpNumber.Items.Add(new RadComboBoxItem("P: " + dataset.Tables[3].Rows[0]["Phone"].ToString()));
        //    txtpNumber.Items.Add(new RadComboBoxItem("W:" + dataset.Tables[3].Rows[0]["WorkNumber"].ToString()));
        //}
        //txtpNumber.SelectedIndex = txtpNumber.FindItemIndexByText(datarow["PatientContactNo"].ToString());
        //Hashtable hshin1 = new Hashtable();
        //hshin1.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
        //hshin1.Add("@intRegistrationId", hdnregno1.Value);
        //DataSet dsobj = objSave.FillDataSet(CommandType.StoredProcedure, "UspSearchEncounter", hshin1);
        //ArrayList arr = new ArrayList();
        //MyEncounter myencounter;
        //foreach (DataRow dr in dsobj.Tables[0].Rows)
        //{
        //    myencounter = new MyEncounter();
        //    myencounter.Encounterid = Convert.ToInt32(dr["EncounterId"].ToString());
        //    myencounter.EncounterDetails = dr["EncounterNo"].ToString() + ", " + dr["DoctorName"].ToString() + ", " + dr["Contact"].ToString();
        //    arr.Add(myencounter);
        //}



        //ddlEncounter.DataSource = arr;
        //ddlEncounter.DataValueField = "Encounterid";
        //ddlEncounter.DataTextField = "EncounterDetails";
        //ddlEncounter.DataBind();
        //ddlEncounter.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        //ddlEncounter.SelectedIndex = ddlEncounter.FindItemIndexByValue(datarow["EncounterId"].ToString());
        //Save.Text = "Update";
        //pnlNewTask.Visible = true;
        //pnlPosted.Visible = false;
        //gvComments.DataSource = dataset.Tables[4];
        //gvComments.DataBind();
    }
    #endregion
    private void BindPatientTask()
    {
        ddlstatusset.Visible = true;
        //btndelete.Visible =true;
        //btnworking.Visible =true;
        //btncompleted.Visible = true;
        ClearForm();
        ddlEncounter.Items.Clear();
        txtpNumber.Items.Clear();
        lbltaskMsg.Text = string.Empty;
        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //hashtable.Add("@intTaskId", Convert.ToInt32(Request.QueryString["TaskID"]));
        //hashtable.Add("@intStatusId", Convert.ToInt32(ddlstatus.SelectedValue.ToString()));
        //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);
        DataSet dataset = new DataSet();
        DataRow datarow = dataset.Tables[0].Rows[0];
        hdnTaskId.Value = Request.QueryString["TaskID"];

        ddlTaskType.SelectedIndex = ddlTaskType.FindItemIndexByText(datarow["TaskType"].ToString());

        // ddlTaskType.Items.FindItemIndexByText(datarow["TaskType"].ToString());
        ddlPriority.SelectedIndex = ddlPriority.Items.FindItemIndexByText(datarow["TaskPriority"].ToString());

        //if (datarow["Status"] != "")
        //{
        //    ddlActive.SelectedValue= datarow["Status"].ToString();
        //}
        txtNote.Text = datarow["TaskMessage"].ToString();
        if (datarow["AssignedBy"] != null && datarow["CreatedDate"] != null)
        {

            txtassignedby.Text = datarow["AssignedBy"].ToString() + " " + datarow["CreatedDate"].ToString();

        }
        if (datarow["PatientName"] != DBNull.Value)
        {
            txtpatientName.Text = datarow["PatientName"].ToString();
        }
        if (datarow["RegistrationNo"] != DBNull.Value)
        {
            txtAccountNumber.Text = datarow["RegistrationNo"].ToString();
        }
        if (Convert.ToBoolean(datarow["PatientContactAllowed"].ToString()) == false)
        {
            rblstCpatient.SelectedIndex = 1;
        }
        else
        {
            rblstCpatient.SelectedIndex = 0;
        }
        if (Convert.ToBoolean(datarow["PersonContactAllowed"].ToString()) == false)
        {
            rblstcPerson.SelectedIndex = 1;
        }
        else
        {
            rblstcPerson.SelectedIndex = 0;
        }
        if (Convert.ToBoolean(datarow["DueNow"].ToString()) == true)
        {
            rbkduenow.Checked = true;
            rbduedate.Checked = false;
            RadDateTimePicker1.Enabled = false;
            RadComboBox1.Enabled = false;
        }
        else
        {
            rbduedate.Checked = true;
            rbkduenow.Checked = false;
            RadDateTimePicker1.Enabled = true;
            RadComboBox1.Enabled = true;
            RadDateTimePicker1.SelectedDate = Convert.ToDateTime(datarow["DueDate"].ToString());
        }

        hdnregno1.Value = datarow["RegistrationId"].ToString();
        txtCPerson.Text = datarow["ContactPersonName"].ToString();
        txtCPPhone.Text = datarow["ContactPersonPhone"].ToString();


        lblstatus.Text = datarow["TaskStatus"].ToString();
        if (datarow["StatusId"] != null)
        {
            ddlstatusset.SelectedValue = datarow["StatusId"].ToString();
        }

        DataTable datatableemp = dataset.Tables[1];
        foreach (DataRow dr in datatableemp.Rows)
        {
            chlstEmployee.Items[chlstEmployee.Items.IndexOf(chlstEmployee.Items.FindByValue(dr[0].ToString()))].Selected = true;
        }
        DataTable datatableempgrp = dataset.Tables[2];
        foreach (DataRow dr in datatableempgrp.Rows)
        {
            chklstempgrp.Items[chklstempgrp.Items.IndexOf(chklstempgrp.Items.FindByValue(dr[0].ToString()))].Selected = true;
        }
        txtpNumber.Items.Add(new RadComboBoxItem("Select", "0"));
        if (dataset.Tables[3].Rows.Count > 0)
        {
            txtpNumber.Items.Add(new RadComboBoxItem("C: " + dataset.Tables[3].Rows[0]["Mobile"].ToString()));
            txtpNumber.Items.Add(new RadComboBoxItem("P: " + dataset.Tables[3].Rows[0]["Phone"].ToString()));
            txtpNumber.Items.Add(new RadComboBoxItem("W:" + dataset.Tables[3].Rows[0]["WorkNumber"].ToString()));
        }
        txtpNumber.SelectedIndex = txtpNumber.FindItemIndexByText(datarow["PatientContactNo"].ToString());
        //Hashtable hshin1 = new Hashtable();
        //hshin1.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
        //hshin1.Add("@intRegistrationId", hdnregno1.Value);
        //DataSet dsobj = objSave.FillDataSet(CommandType.StoredProcedure, "UspSearchEncounter", hshin1);
        DataSet dsobj = new DataSet();
        ArrayList arr = new ArrayList();
        MyEncounter myencounter;
        foreach (DataRow dr in dsobj.Tables[0].Rows)
        {
            myencounter = new MyEncounter();
            myencounter.Encounterid = Convert.ToInt32(dr["EncounterId"].ToString());
            myencounter.EncounterDetails = dr["EncounterNo"].ToString() + ", " + dr["DoctorName"].ToString() + ", " + dr["Contact"].ToString();
            arr.Add(myencounter);
        }



        ddlEncounter.DataSource = arr;
        ddlEncounter.DataValueField = "Encounterid";
        ddlEncounter.DataTextField = "EncounterDetails";
        ddlEncounter.DataBind();
        ddlEncounter.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        ddlEncounter.SelectedIndex = ddlEncounter.FindItemIndexByValue(datarow["EncounterId"].ToString());
        Save.Text = "Update";
        pnlNewTask.Visible = true;
        pnlPosted.Visible = false;
        gvComments.DataSource = dataset.Tables[4];
        gvComments.DataBind();
    }
    #region Button Event
    protected void btnfind_Click(object sender, EventArgs e)
    {


    }
    protected void btnAssigned_Click(object sender, EventArgs e)
    {
        ////pnlAddType.Visible = false;

        ////pnlNewTask.Visible = false;
        //Hashtable hshIn = new Hashtable();
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //BaseC.ParseData parse = new BaseC.ParseData();
        //hshIn.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
        //hshIn.Add("@intRegistrationId", txttest.Text);
        //DataSet ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "UspSearchRegistration", hshIn);
        //ViewState["NewRegNo"] = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
        //txtpatientName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
        //txtpNumber.Items.Clear();
        //txtpNumber.Items.Add(new RadComboBoxItem("Select"));
        //txtpNumber.Items.Add(new RadComboBoxItem("C: " + ds.Tables[0].Rows[0]["Mobile"].ToString()));
        //txtpNumber.Items.Add(new RadComboBoxItem("P: " + ds.Tables[0].Rows[0]["PhoneHome"].ToString()));
        //txtpNumber.Items.Add(new RadComboBoxItem("W:" + ds.Tables[0].Rows[0]["WorkNumber"].ToString()));

        //hdnregno1.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
        //txtAccountNumber.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
        //Hashtable hshin1 = new Hashtable();
        //hshin1.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
        //hshin1.Add("@intRegistrationId ", ds.Tables[0].Rows[0]["RegistrationId"].ToString());
        //DataSet dsobj = dl.FillDataSet(CommandType.StoredProcedure, "UspSearchEncounter", hshin1);
        //ArrayList arr = new ArrayList();
        //MyEncounter myencounter;
        //if (dsobj.Tables.Count > 0)
        //{
        //    foreach (DataRow dr in dsobj.Tables[0].Rows)
        //    {
        //        myencounter = new MyEncounter();
        //        myencounter.Encounterid = Convert.ToInt32(dr["EncounterId"].ToString());
        //        myencounter.EncounterDetails = dr["EncounterNo"].ToString() + ", " + dr["DoctorName"].ToString() + ", " + dr["Contact"].ToString();
        //        arr.Add(myencounter);
        //    }
        //}


        //ddlEncounter.DataSource = arr;
        //ddlEncounter.DataValueField = "Encounterid";
        //ddlEncounter.DataTextField = "EncounterDetails";
        //ddlEncounter.DataBind();
        //ddlEncounter.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        ////dlEncounter.Items.Insert(0, new ListItem("Select", "0"));



    }
    protected void btnSent_Click(object sender, EventArgs e)
    {

        //pnlAddType.Visible = false;
        //pnlAssigned.Visible = true;
        //pnlNewTask.Visible = false;
        //pnlPosted.Visible = false;
        //pnlPriorityMessage.Visible = false;
        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //hashtable.Add("@intEmpAssignedTo", EmployeeId);
        //hashtable.Add("@intStatusId", Convert.ToInt32(ddlstatus.SelectedValue.ToString()));
        //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);
        //Cache.Insert("Task_" + Session["UserID"].ToString(), dataset, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
        //ViewState["Count"] = dataset.Tables[0].Rows.Count;
        //gvTasks.DataSource = dataset.Tables[0];
        //gvTasks.DataBind();

        //if (dataset.Tables[0].Rows.Count > 0)
        //{

        //    ViewState["Count"] = 0;

        //}
        //else
        //{
        //    Blankgridtask();
        //}

    }
    protected void btnNewTask_Click(object sender, EventArgs e)
    {
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //DataSet ds1 = new DataSet();
        //ds1 = dl.FillDataSet(CommandType.Text, "select (FirstName + ' ' + MiddleName + ' ' + LastName) as assignedby   from  Employee where ID=" + Session["UserID"].ToString());
        ////txtassignedby.Text = ds1.Tables[0].Rows[0]["assignedby"].ToString()+" "+DateTime .Now.ToString("MM/dd/yyyy hh:mm tt");
        //BaseC.EMR objEmr = new BaseC.EMR(sConString);
        //if (Session["UserID"] != null && Session["HospitalLocationID"] != null)
        //{
        //    SqlDataReader objDr = (SqlDataReader)objEmr.GetEmployeeId(Convert.ToInt32(Session["UserID"]), Convert.ToInt16(Session["HospitalLocationID"]));
        //    if (objDr.Read())
        //        txtassignedby.Text = objDr[1].ToString() + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        //}
        //ClearForm();

        //DataSet dsobj = dl.FillDataSet(CommandType.Text, "Select Description,TaskCategoryId from TaskCategoriesMaster  where  HospitalLocationId=" + Session["HospitalLocationID"] + " and Active=1");
        //ddlTaskType.DataSource = dsobj;
        //ddlTaskType.DataTextField = "Description";
        //ddlTaskType.DataValueField = "TaskCategoryId";
        //ddlTaskType.DataBind();
        //pnlAddType.Visible = false;
        //pnlAssigned.Visible = false;
        //pnlNewTask.Visible = true;
        //pnlPosted.Visible = false;
        //pnlPriorityMessage.Visible = false;
        //hdnTaskId.Value = "0";
        //ddlEncounter.Items.Clear();
        //txtpNumber.Items.Clear();
        //lbltaskMsg.Text = string.Empty;
        //gvComments.DataSource = null;
        //gvComments.DataBind();
        //Save.Text = "Save & Leave Open";
        //lblstatus.Text = "";
        //ddlStatus1.SelectedIndex = 0;
    }

    protected void btnAddType_Click(object sender, EventArgs e)
    {
        pnlAddType.Visible = true;
        pnlAssigned.Visible = false;
        pnlNewTask.Visible = false;
        pnlPosted.Visible = false;
        pnlPriorityMessage.Visible = false;
        BindListBox();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        //TaskTypes tasktype = new TaskTypes();
        //tasktype.TaskTypeId = 0;
        //tasktype.Description = new BaseC.ParseData().ParseQ(txtNewType.Text);
        //tasktype.EncodeBy = Convert.ToInt32(Session["UserId"]);
        //tasktype.HospitalLoctionId = Convert.ToInt32(Session["HospitalLocationId"]);


        //try
        //{

        //    txtNewType.Text = "";

        //    string stmsg = tasktype.SaveTaskType();
        //    if (stmsg == "VarChar")
        //    {
        //        lblMsg.Text = "Saved";
        //        BindListBox();
        //    }
        //    else
        //    {
        //        lblMsg.Text = stmsg;
        //    }

        //}
        //catch (Exception ex)
        //{
        //    lblMsg.Text = ex.Message;
        //}
        //finally
        //{
        //    tasktype = null;
        //}



    }

    protected void btnPosted_Click(object sender, EventArgs e)
    {
        //pnlAddType.Visible = false;
        //pnlAssigned.Visible = false;
        //pnlNewTask.Visible = false;
        //pnlPosted.Visible = true;
        //pnlPriorityMessage.Visible = false;
        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationID"]);
        //hashtable.Add("@intLoginFacilityId", Convert.ToInt32(Session["FacilityID"]));
        //hashtable.Add("@intEmpAssignedBy", Session["UserId"].ToString());
        //hashtable.Add("@intStatusId", Convert.ToInt32(ddlstatus.SelectedValue.ToString()));
        //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", hashtable);

        //ViewState["Count1"] = dataset.Tables[0].Rows.Count;
        //grvPosted.DataSource = dataset.Tables[0];
        //grvPosted.DataBind();
        //if (dataset.Tables[0].Rows.Count > 0)
        //{
        //    ViewState["Count1"] = 0;

        //}
        //else
        //{
        //    Blankgridpost();
        //}

    }

    protected void Save_Click(object sender, EventArgs e)
    {
        //if (ddlTaskType.SelectedIndex < 0)
        //{
        //    lbltaskMsg.ForeColor = System.Drawing.Color.Red;
        //    lbltaskMsg.Text = "Select a Task Type";
        //    return;
        //}
        //if (ddlPriority.SelectedIndex < 0)
        //{
        //    lbltaskMsg.ForeColor = System.Drawing.Color.Red;
        //    lbltaskMsg.Text = "Select a Priority of Task";
        //    return;
        //}
        //if (chlstEmployee.SelectedIndex < 0 && chklstempgrp.SelectedIndex < 0)
        //{
        //    lbltaskMsg.ForeColor = System.Drawing.Color.Red;
        //    lbltaskMsg.Text = "Select Employee or Employee Group";
        //    return;
        //}
        //if (rbduedate.Checked == true && RadDateTimePicker1.IsEmpty)
        //{
        //    lbltaskMsg.ForeColor = System.Drawing.Color.Red;
        //    lbltaskMsg.Text = "Select a Valid Due Date";
        //    return;
        //}
        //BaseC.Task.Tasks taske = new BaseC.Task.Tasks();
        //BaseC.ParseData parse = new BaseC.ParseData();
        //taske.TasksId = Convert.ToInt32(hdnTaskId.Value);
        //taske.StatusId = Convert.ToInt32(ddlstatusset.SelectedValue.ToString());
        //taske.HospitalLocationId = Convert.ToInt32(Session["HospitalLocationId"].ToString());
        //taske.TaskCatId = Convert.ToInt32(ddlTaskType.SelectedValue);
        //taske.Message = parse.ParseQ(txtNote.Text);

        //if (hdnregno1.Value != string.Empty)
        //{
        //    taske.RegistrationId = Convert.ToInt32(hdnregno1.Value);

        //}
        //else
        //{
        //    taske.RegistrationId = 0;
        //}
        //if (ddlEncounter.Items.Count > 0)
        //{
        //    taske.EncounterId = Convert.ToInt32(ddlEncounter.SelectedValue);
        //}
        //else
        //{
        //    taske.RegistrationId = 0;
        //}
        //if (txtpNumber.Items.Count > 0)
        //{
        //    taske.PatinetPhone = txtpNumber.SelectedItem.Text.Substring(txtpNumber.SelectedItem.Text.LastIndexOf(":") + 1);
        //}
        //else
        //{
        //    taske.PatinetPhone = "  ";
        //}
        //taske.IsContactPatinet = rblstCpatient.SelectedIndex == 0 ? 1 : 0;
        //taske.PriorityLevel = Convert.ToInt32(ddlPriority.SelectedValue);
        //taske.ContactPName = parse.ParseQ(txtCPerson.Text);
        //taske.ContactPPhone = parse.ParseQ(txtCPPhone.Text);
        //taske.IsContactPerson = rblstcPerson.SelectedIndex == 0 ? 1 : 0;
        //taske.IsDueOnNow = rbkduenow.Checked == true ? 1 : 0;
        //if (rbkduenow.Checked == true)
        //{
        //    taske.DueDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
        //}
        //else
        //{
        //    taske.DueDate = RadDateTimePicker1.SelectedDate.Value.ToString("dd/MM/yyyy hh:mm");

        //}
        //StringBuilder objXMLEmp = new StringBuilder();

        //foreach (ListItem listitem in chlstEmployee.Items)
        //{
        //    if (listitem.Selected == true)
        //    {
        //        objXMLEmp.Append("<Table1><c1>");
        //        objXMLEmp.Append(listitem.Value);
        //        objXMLEmp.Append("</c1></Table1>");
        //    }
        //}
        //StringBuilder objXMLEmpgrp = new StringBuilder();
        //foreach (ListItem listitem in chklstempgrp.Items)
        //{
        //    if (listitem.Selected == true)
        //    {
        //        objXMLEmpgrp.Append("<Table1><c1>");
        //        objXMLEmpgrp.Append(listitem.Value);
        //        objXMLEmpgrp.Append("</c1></Table1>");
        //    }
        //}
        //taske.AssignedToEmp = objXMLEmp.ToString();
        //taske.AssignedToEmpGroup = objXMLEmpgrp.ToString();
        //taske.EncodedBy = Convert.ToInt32(Session["UserId"]);
        //taske.UserTempCmnt = TextBox1.Text.Trim();
        //taske.TaskUrl = ltrlLink.Text;
        //taske.Active = Convert.ToInt32(ddlActive.SelectedValue);

        //string stmsg = taske.SaveTasks();

        //if (stmsg == "VarChar")
        //{
        //    lbltaskMsg.ForeColor = System.Drawing.Color.Green;
        //    lbltaskMsg.Text = "Saved";
        //    BindListBox();

        //}
        //else
        //{
        //    lbltaskMsg.Text = stmsg;
        //}
        //ClearForm();
        //ddlEncounter.Items.Clear();
        //txtpNumber.Items.Clear();
        //hdnTaskId.Value = "0";
        //Save.Text = "    Save   ";
    }

    #endregion

    #region RadCombo and Radiobutton Evnet

    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (!RadDateTimePicker1.IsEmpty)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append(RadDateTimePicker1.SelectedDate.Value.ToString());
            sb.Remove(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
            sb.Insert(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
            RadDateTimePicker1.SelectedDate = Convert.ToDateTime(sb.ToString());
        }
    }
    
    //protected void RadComboBox2_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    //string strValue = RadComboBox2.SelectedItem.Text;
    //    //int acountNo = Convert.ToInt32(strValue.Substring(strValue.LastIndexOf("-") + 1));
    //    //txtAccountNumber.Text = acountNo.ToString();
    //}

    protected void rbduedate_CheckedChanged(object sender, EventArgs e)
    {
        if (rbduedate.Checked == true)
        {
            RadDateTimePicker1.Enabled = true;
            RadComboBox1.Enabled = true;
            lbl_hhmm.Enabled = true;
        }
        else
        {
            RadDateTimePicker1.Enabled = false;
            RadComboBox1.Enabled = false;
            lbl_hhmm.Enabled = false;
        }
    }

    protected void rbkduenow_CheckedChanged(object sender, EventArgs e)
    {
        if (rbkduenow.Checked == true)
        {
            RadDateTimePicker1.Enabled = false;
            RadComboBox1.Enabled = false;
            lbl_hhmm.Enabled = false;
        }
        else
        {
            RadDateTimePicker1.Enabled = true;
            RadComboBox1.Enabled = true;
            lbl_hhmm.Enabled = true;
        }
    }

    protected void RadDateTimePicker1_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(RadDateTimePicker1.SelectedDate.Value.ToString());
        sb.Remove(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
        RadDateTimePicker1.SelectedDate = Convert.ToDateTime(sb.ToString());
    }

    #endregion

    #region Comment Grid

    protected void gvComments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (e.Row.Cells[1].Controls.Count > 0)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (dr["EncodedId"].ToString() != Session["UserID"].ToString())
                {
                    ImageButton l = (ImageButton)e.Row.Cells[1].Controls[0];
                    l.Visible = false;
                    LinkButton l1 = (LinkButton)e.Row.Cells[2].Controls[0];
                    l1.Visible = false;
                }

            }
        }
    }
    
    protected void gvComments_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //dal.ExecuteNonQuery(CommandType.Text, "Delete from TaskUserComments where id=" + gvComments.DataKeys[e.RowIndex].Value);
        BindComments();
    }
    
    protected void gvComments_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvComments.EditIndex = e.NewEditIndex;
        BindComments();
    }
    
    private void BindComments()
    {

        //DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //gvComments.DataSource = dal.FillDataSet(CommandType.Text, "Select id, Remarks, EncodedBy As EncodedId, dbo.GetDoctorName(EncodedBy) As EncodedBy, dbo.GetDateFormat(EncodedDate,'DT') EncodedDate 	From TaskUserComments Where TaskId = " + hdnTaskId.Value + " And Active = 1 Order By Id Desc").Tables[0];
        gvComments.DataBind();

    }
    
    protected void gvComments_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvComments.EditIndex = -1;
        BindComments();
    }
    
    protected void gvComments_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //int commentID = Convert.ToInt32(gvComments.DataKeys[e.RowIndex].Value);
        //DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hastable = new Hashtable();
        //hastable.Add("@remarks", ((TextBox)gvComments.Rows[e.RowIndex].Cells[0].FindControl("txtremarks")).Text.Trim());
        //hastable.Add("@LastChangedBy", Session["UserID"].ToString());
        //hastable.Add("@id", commentID);
        //dal.ExecuteNonQuery(CommandType.Text, "update TaskUserComments  set Remarks=@remarks,  LastChangedDate=getDate(), LastChangedBy=@LastChangedBy where id=@id ", hastable);
        gvComments.EditIndex = -1;
        BindComments();
    }
    
    #endregion

    protected void ddlStatus_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        BindPost();
    }
    
    protected void ddlStatus1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        BindTaks();
    }
    
    protected void ddlstatusset_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {


        //  lblstatus.Text = ddlstatusset.SelectedItem.Text;


    }
    
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }
    
    public void status()
    {
        //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //int hosid = common.myInt(Session["HospitalLocationID"]);
        //String sqlstr = "Select StatusId, Status From dbo.GetStatus(" + hosid + ",'TaskStatus')";
        //DataSet ds = new DataSet();
        //ds = obj.FillDataSet(CommandType.Text, sqlstr);
        //ddlstatus.DataSource = ds.Tables[0];
        //ddlstatus.DataTextField = "Status";
        //ddlstatus.DataValueField = "StatusId";
        //ddlstatus.DataBind();
        //ddlstatus.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("All"));
        //ddlstatus.Items[0].Value = "0";
    }
    
    public void status1()
    {
        //string sqlstr = "Select StatusId, Status From dbo.GetStatus(" + common.myInt(Session["HospitalLocationID"]) + ",'TaskStatus')";

        //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //DataSet ds = new DataSet();
        //ds = obj.FillDataSet(CommandType.Text, sqlstr);

        //ddlStatus1.DataSource = ds;
        //ddlStatus1.DataValueField = "StatusId";
        //ddlStatus1.DataTextField = "Status";
        //ddlStatus1.DataBind();

        //ddlStatus1.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("All", "0"));
        //ddlStatus1.SelectedIndex = 0;
    }
    
    public void statusset()
    {
        //DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //int hosid = common.myInt(Session["HospitalLocationID"]);
        //String sqlstr = "Select StatusId, Status From dbo.GetStatus(" + hosid + ",'TaskStatus')";
        //DataSet ds = new DataSet();
        //ds = obj.FillDataSet(CommandType.Text, sqlstr);
        //ddlstatusset.DataSource = ds.Tables[0];
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlstatusset.DataTextField = "Status";
        //    ddlstatusset.DataValueField = "StatusId";
        //    ddlstatusset.DataBind();
        //}
    }
    
    protected void gvTasks_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            int iTotalRecords = Convert.ToInt16(ViewState["Count"]);
            int iPageSize = Convert.ToInt16(gvTasks.PageSize);
            int iRowCount = Convert.ToInt16(gvTasks.Rows.Count);
            if (iRowCount == iPageSize)
            {
                lblGridStatus.Text = "Showing " + ((gvTasks.PageIndex *
                gvTasks.PageSize) + 1) + " - " + ((gvTasks.PageIndex + 1) *
                gvTasks.Rows.Count) + " of " + iTotalRecords + " Record(s)";
            }
            else
            {
                lblGridStatus.Text = "Showing " + ((gvTasks.PageIndex * iPageSize) + 1)
                    + " - " + iTotalRecords + " of " + iTotalRecords + " Record(s)";
            }
        }
    }
    
    protected void grvPosted_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            int iTotalRecords = Convert.ToInt16(ViewState["Count1"]);
            int iPageSize = Convert.ToInt16(grvPosted.PageSize);
            int iRowCount = Convert.ToInt16(grvPosted.Rows.Count);
            if (iRowCount == iPageSize)
            {
                lblGridStatus.Text = "Showing " + ((grvPosted.PageIndex *
                grvPosted.PageSize) + 1) + " - " + ((grvPosted.PageIndex + 1) *
                grvPosted.Rows.Count) + " of " + iTotalRecords + " Record(s)";
            }
            else
            {
                lblGridStatus1.Text = "Showing " + ((grvPosted.PageIndex * iPageSize) + 1)
                    + " - " + iTotalRecords + " of " + iTotalRecords + " Record(s)";
            }
        }

    }

    protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Byte A;
        Byte R;
        Byte G;
        Byte B;
        String htmlHexColorValue = "";
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (ViewState["BlankGrid"] != null)
            {

                e.Row.Cells[Convert.ToByte(Gvtask.TaskPriority)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gvtask.TaskId)].Visible = false;
                e.Row.Cells[Convert.ToByte(Gvtask.pageurl)].Visible = false;


            }

            e.Row.Cells[9].Visible = false;
            e.Row.Cells[Convert.ToByte(Gvtask.StatusColor)].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[9].Text.Trim() == "&nbsp;")
            {
                e.Row.Cells[8].Visible = false;
            }
            if (ViewState["Blank"] == null)
            {
                String PriorityColor = e.Row.Cells[Convert.ToByte(Gvtask.StatusColor)].Text.ToString().Trim();
                Label lbl_PriorityColor = new Label();

                if (PriorityColor.ToString().Trim() != "Blank" && PriorityColor.ToString().Trim() != "&nbsp;" && PriorityColor.ToString().Trim() != "" && PriorityColor.ToString().Trim() != "StatusColor")
                {
                    lbl_PriorityColor.BackColor = System.Drawing.ColorTranslator.FromHtml(PriorityColor.Trim());

                    A = lbl_PriorityColor.BackColor.A;
                    B = lbl_PriorityColor.BackColor.B;
                    R = lbl_PriorityColor.BackColor.R;
                    G = lbl_PriorityColor.BackColor.G;
                    htmlHexColorValue = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(A, R, G, B));
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml(htmlHexColorValue.ToString());
                    }
                }
            }
            if (e.Row.Cells[9].Text == " ")
            {
                LinkButton lbl = (LinkButton)e.Row.FindControl("lnkOpenTask");

                lbl.Visible = false;


            }
            else
            {
                LinkButton lbl = (LinkButton)e.Row.FindControl("lnkOpenTask");

                lbl.Visible = true;
                gvTasks.Columns[1].SortExpression = "Tasktype";
            }
        }
    }
    
    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        gvComments.DataSource = null;
        gvComments.DataBind();

        lstTypeList.DataSource = null;
        lstTypeList.DataBind();

        gvTasks.DataSource = null;
        gvTasks.DataBind();

        grvPosted.DataSource = null;
        grvPosted.DataBind();

        //BindPost();
    }
    
    protected void btnfiltertask_Click(object sender, EventArgs e)
    {
        ///BindTaks();

    }
    
    protected void btnRemovePageLink_Click(object sender, EventArgs e)
    {
        ltrlLink.Text = "";
    }

    protected void btnSelectTask_Click(object sender, EventArgs e)
    {
        string url = "";
        if (Session["RegistrationId"] != null)
            url = "?RegId=" + Session["RegistrationId"].ToString();

        if (Session["encounterid"] != null)
        {
            if (url == "")
                url += "?EncId=" + Session["encounterid"].ToString();
            else
                url += "&EncId=" + Session["encounterid"].ToString();
        }

        if (Session["formId"] != null)
        {
            if (url == "")
                url += "?frmId=" + Session["formId"].ToString();
            else
                url += "&frmId=" + Session["formId"].ToString();
        }

        if (Session["AppointmentID"] != null)
        {
            if (url == "")
                url += "?admId=" + Session["AppointmentID"].ToString();
            else
                url += "&admId=" + Session["AppointmentID"].ToString();
        }

        ltrlLink.Text = Request.Url.AbsolutePath + url;
    }

    protected void gvPosted_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "task")
        {
            Response.Redirect(e.CommandArgument.ToString(), false);
        }
    }

    protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "task")
        {
            Response.Redirect(e.CommandArgument.ToString(), false);
        }
    }
 
    protected void gvTasks_Sorting(object sender, GridViewSortEventArgs e)
    {
        Cache["SortExpression"] = e.SortExpression;
        DataSet objDs = new DataSet();
        DataView dv;
        if (Session["UserID"] != null)
        {
            if (Cache["Task_" + Session["UserID"].ToString()] != null)
            {
                if (Cache["Task_" + Session["UserID"].ToString()].GetType().ToString() == "System.Data.DataSet")
                {
                    objDs = (DataSet)Cache["Task_" + Session["UserID"].ToString()];
                    dv = new DataView(objDs.Tables[0]);
                    dv.Sort = e.SortExpression;
                    gvTasks.DataSource = dv;
                }
                else
                {
                    dv = (DataView)Cache["Task_" + Session["UserID"].ToString()];
                    if (GridViewSortDirection == SortDirection.Ascending)
                    {
                        GridViewSortDirection = SortDirection.Descending;
                        dv.Sort = e.SortExpression + " desc";
                    }
                    else
                    {
                        GridViewSortDirection = SortDirection.Ascending;
                        dv.Sort = e.SortExpression + " asc";
                    }

                    gvTasks.DataSource = dv;
                }
                Cache["Task_" + Session["UserID"].ToString()] = gvTasks.DataSource;
                gvTasks.DataBind();
            }
        }


    }

}