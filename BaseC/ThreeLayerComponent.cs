using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace BaseC
{
    public class ThreeLayerComponent
    {
        public string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

        private DAL.DAL objDl;

        #region Singleton Design pattern 

        private ThreeLayerComponent()
        {

        }

        public static ThreeLayerComponent GetInstnace = null;

        public static ThreeLayerComponent MyObject
        {
            get
            {
                if (GetInstnace == null)
                {
                    GetInstnace = new ThreeLayerComponent();
                }
                return GetInstnace;
            }
        }
        #endregion

        

        #region DataTable
        /// Can execute plain query        
        public DataTable GetDataTable(string Query)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, Query).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }

        /// Can execute Parameterize Query  and  Store Procedure        
        public DataTable GetDataTable(CommandType Type, string Query, Hashtable InputParameter)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(Type, Query, InputParameter).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }

        /// Can execute Query and Store Procedure 
        public DataTable GetDataTable(CommandType Type, string Query)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(Type, Query).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
            }
        }

        #endregion

        #region DataSet
        public DataSet GetDataSet(CommandType Type, string Query)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(Type, Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
            }
        }

        public DataSet GetDataSet(CommandType Type, string Query, Hashtable InputParameter)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(Type, Query, InputParameter);
            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }

        public DataSet GetDataSet(CommandType Type, string Query, Hashtable InputParameter, Hashtable OutputParameter)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(Type, Query, InputParameter, OutputParameter);
            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }

        #endregion

        #region ExecuteScalar
        public object GetSingleValue(string Query)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.ExecuteScalar(CommandType.Text, Query);

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }

        }
        #endregion

        #region Hashtable
        public Hashtable GetOutputParametersValues(string StoreProcdureName, Hashtable HashInputPara)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.getOutputParametersValues(CommandType.StoredProcedure, StoreProcdureName, HashInputPara);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
            }
        }
        public Hashtable GetOutputParametersValues(string StoreProcdureName, Hashtable HashInputPara, Hashtable HashOutputPara)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.getOutputParametersValues(CommandType.StoredProcedure, StoreProcdureName, HashInputPara, HashOutputPara);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
            }
        }

        #endregion

        #region IDataReader
        public IDataReader GetExecuteReader(CommandType cmdType, string cmdStr, Hashtable hashPara)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.ExecuteReader(cmdType, cmdStr, hashPara);

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }
        #endregion

        #region Query
        // Insert / Update 
        public int InsertUpdateDataTable(string Query)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.ExecuteNonQuery(CommandType.Text, Query);

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }

        // Insert / Update Using Parameterize Query  
        public int InsertUpdateDataTable(string Query, Hashtable InputParameter)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.ExecuteNonQuery(CommandType.Text, Query, InputParameter);

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }
        public int InsertUpdateDataTable(CommandType Type, string Query, Hashtable InputParameter)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.ExecuteNonQuery(Type, Query, InputParameter);

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }

        #endregion

       

    }

    

}
