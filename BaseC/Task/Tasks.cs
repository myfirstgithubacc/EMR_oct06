using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Web;
namespace BaseC.Task
{
  public   class Tasks
    {
        #region Private Date Mamber
        int _tasksId;
        int _hospitalLocationId;
        int _taskCatId;
        string _message;
        int _registrationId;
        int _encounterId;
        int _isContactPatinet;
        string _patinetPhone;
        int _priorityLevel;
        string _contactPName;
        string _contactPPhone;
        int _isContactPerson;
        int _isDueOnNow;
        string _dueDate;
        string _assignedToEmp;
        string _assignedToEmpGroup;
        string _userComment;
        string _userTempCmnt;
        int _statusid;
        string _TaskUrl = "";
        int _Active;

        public int Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        public string UserTempCmnt
        {
            get { return _userTempCmnt; }
            set { _userTempCmnt = value; }
        }

        
        int _encodedBy;
        #endregion

        #region Public Property
       
        public int TasksId
        {
            get { return _tasksId; }
            set { _tasksId = value; }
        }

        public string UserComment
        {
            get { return _userComment; }
            set { _userComment = value; }
        }
        public int HospitalLocationId
        {
            get { return _hospitalLocationId; }
            set { _hospitalLocationId = value; }
        }
        

        public int TaskCatId
        {
            get { return _taskCatId; }
            set { _taskCatId = value; }
        }
        

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        

        public int RegistrationId
        {
            get { return _registrationId; }
            set { _registrationId = value; }
        }
        

        public int EncounterId
        {
            get { return _encounterId; }
            set { _encounterId = value; }
        }
        

        public int IsContactPatinet
        {
            get { return _isContactPatinet; }
            set { _isContactPatinet = value; }
        }
        

        public string PatinetPhone
        {
            get { return _patinetPhone; }
            set { _patinetPhone = value; }
        }
        

        public int PriorityLevel
        {
            get { return _priorityLevel; }
            set { _priorityLevel = value; }
        }
        

        public string ContactPName
        {
            get { return _contactPName; }
            set { _contactPName = value; }
        }
        

        public string ContactPPhone
        {
            get { return _contactPPhone; }
            set { _contactPPhone = value; }
        }
        

        public int IsContactPerson
        {
            get { return _isContactPerson; }
            set { _isContactPerson = value; }
        }
        

        public int IsDueOnNow
        {
            get { return _isDueOnNow; }
            set { _isDueOnNow = value; }
        }


        public string DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }
        

        public string AssignedToEmp
        {
            get { return _assignedToEmp; }
            set { _assignedToEmp = value; }
        }
        

        public string AssignedToEmpGroup
        {
            get { return _assignedToEmpGroup; }
            set { _assignedToEmpGroup = value; }
        }
        

        public int EncodedBy
        {
            get { return _encodedBy; }
            set { _encodedBy = value; }
        }

        public int StatusId
        {
            get { return _statusid; }
            set { _statusid = value; }
        }

        public string TaskUrl
        {
            get { return _TaskUrl; }
            set { _TaskUrl = value; }
        }

        #endregion

        #region Public Method

         public int DeleteTask()
        {
            int i;
            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString);
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@intTaskId",TasksId);
            hashtable.Add("@intEncodedBy ",EncodedBy);
            
            try 
	        {
                i = dal.ExecuteNonQuery(CommandType.Text, "update  TaskDetails set Active=0, LastChangedDate=GetDate(), LastChangedBy=@intEncodedBy Where TaskId = @intTaskId", hashtable);
                    return i;
	        }
	        catch (Exception e)
	         {
		
	            	throw e;
	        }
            
        }
        public string SaveTasks()
        {
            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString);
            Hashtable hashouttable= new Hashtable();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@inyHospitalLocationId ", HospitalLocationId);
            hashtable.Add("@intTaskId", TasksId);
            hashtable.Add("@intTaskCategoryId ", TaskCatId);
            hashtable.Add("@bitActive", Active);
            hashtable.Add("@chvMessage", Message);
            if (RegistrationId > 0)
            {
                hashtable.Add("@intRegistrationId", RegistrationId);
            }
            if (EncounterId > 0)
            {
                hashtable.Add("@intEncounterId", EncounterId);
            }
            hashtable.Add("@chvPatientPhone", PatinetPhone);
            hashtable.Add("@inyPriorityLevel", PriorityLevel);
            hashtable.Add("@chvContactPersonName", ContactPName);
            hashtable.Add("@chvContactPersonPhone", ContactPPhone);
            hashtable.Add("@bitDueNow", IsDueOnNow);
            hashtable.Add("@chvDueDate", DueDate);
            hashtable.Add("@xmlEmployees", AssignedToEmp);
            hashtable.Add("@xmlEmployeeTypes", AssignedToEmpGroup);
            hashtable.Add("@intEncodedBy",  EncodedBy);
            hashtable.Add("@bitPatientContact", IsContactPatinet);
            hashtable.Add("@bitPersonContact", IsContactPerson);
            hashtable.Add("@intStatusId", StatusId);
            hashtable.Add("@chvPageUrl", TaskUrl);
            hashtable.Add("@chvTaskComments", new ParseData().ParseQ(UserTempCmnt));
            hashouttable.Add("@chvErrorStatus", SqlDbType.VarChar);

            dal.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveTask", hashtable, hashouttable);
            return hashouttable["@chvErrorStatus"].ToString();
        }
        #endregion

    }
}
