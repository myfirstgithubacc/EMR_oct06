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
   public  class TaskTypes
    {
        int _taskTypeId;
        string _description;
        int _hospitalLoctionId;
        int _encodeBy;
        int _lastChangedBy;
        int _active;

        public int LastChangedBy
        {
            get { return _lastChangedBy; }
            set { _lastChangedBy = value; }
        }
       

        public int Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public int EncodeBy
        {
            get { return _encodeBy; }
            set { _encodeBy = value; }
        }
        public int TaskTypeId
        {
            get { return _taskTypeId; }
            set { _taskTypeId = value; }
        }
       

        public int HospitalLoctionId
        {
            get { return _hospitalLoctionId; }
            set { _hospitalLoctionId = value; }
        }
      

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool DeleteTaskeType()
        {
            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString);
           
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@intTaskCategoryId ", TaskTypeId);
            hashtable.Add("@intEncodedBy", EncodeBy);
            if (dal.ExecuteNonQuery(CommandType.Text, "Update TaskCategoriesMaster Set Active = 0 ,LastChangedBy = @intEncodedBy, LastChangedDate = GetDate() Where TaskCategoryId = @intTaskCategoryId", hashtable) == 0)
            {
             return true   ;
            }
            return false;
            
        }
        public string  SaveTaskType()
        {
           
            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer,System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString);
            Hashtable hashtable = new Hashtable();
            Hashtable hashouttable= new Hashtable();
            hashtable.Add("@intTaskCategoryId ", TaskTypeId);
            hashtable.Add("@inyHospitalLocationId", HospitalLoctionId);
            hashtable.Add("@chvDescription", Description);
            hashtable.Add("@bitActive", Active);
            hashtable.Add("@intEncodedBy", EncodeBy);
            hashouttable.Add("@chvErrorStatus", SqlDbType.VarChar);

            dal.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveTaskCategories", hashtable, hashouttable);
            return hashouttable["@chvErrorStatus"].ToString();
            //if (dal.ExecuteNonQuery(System.Data.CommandType.StoredProcedure,  "UspSaveTaskCategories", hashtable) == 0)
            //{
            //    return true;
            //}
            //return false;
        }
        public ArrayList GetTaskType()
        {
            
            ArrayList arraylist;
            try {
                using (IDataReader dataReader = new DAL.DAL(DAL.DAL.DBType.SqlServer, System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString).ExecuteReader(System.Data.CommandType.Text, "select TaskCategoryId,Description from TaskCategoriesMaster where Active=1"))
                {
                    TaskTypes tasktype;
                    arraylist = new ArrayList();
                    while(dataReader.Read())
                    {
                        tasktype = new TaskTypes();
                        tasktype._taskTypeId = Convert.ToInt32(dataReader[0]);
                        tasktype.Description = dataReader[1].ToString();
                        arraylist.Add(tasktype);
                    }
                    return arraylist;
                }

            
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                arraylist = null;
            }
            
        }
    }
}
