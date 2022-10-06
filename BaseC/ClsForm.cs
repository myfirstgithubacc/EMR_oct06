using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace BaseC
{

    /// <summary>
    /// writen by Awadesh 
    /// </summary>
    public class ClsForm
    {
        private string sConString = string.Empty;
        public ClsForm(string conString)
        {
            sConString = conString;
        }
        public DataSet getFormTagging()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "Usp_DescriptionFormtagging");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

                objDl = null;
            }


        }
        public DataSet DuplicateFormTagging(int form,int route)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@Form", form);
                HshIn.Add("@Route", route);
                return objDl.FillDataSet(CommandType.StoredProcedure, "Usp_DescriptionFormtagging_Duplicate", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

                objDl = null;
            }


        }
        public DataSet Select_FormTagging()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet ds = new DataSet();
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "Usp_DescriptionFormtagging_Select");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

                objDl = null;
            }


        }
        public DataSet Edit_FormTagging(int id)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@id", id);
                return objDl.FillDataSet(CommandType.StoredProcedure, "Usp_DescriptionFormtagging_Edit", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

                objDl = null;
            }

        }
        public string InsertFormTaggingData(int Form, int Route, int Unit, decimal Dose, int Default, string Incodecby)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@Form", Form);
                HshIn.Add("@Route", Route);
                HshIn.Add("@Unit", Unit);
                HshIn.Add("@Dose", Dose);
                HshIn.Add("@Default", Default);
                HshIn.Add("@Encodedby", Incodecby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "Usp_DescriptionFormtagging_Insert", HshIn, HshOut);
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return HshOut["@chvErrorStatus"].ToString();

        }
        public string UpdateFormTaggingData(int id, int Form, int Route, int Unit, decimal Dose, int Default, string Incodecby)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@Form", Form);
                HshIn.Add("@Route", Route);
                HshIn.Add("@Unit", Unit);
                HshIn.Add("@Dose", Dose);
                HshIn.Add("@Default", Default);
                HshIn.Add("@Encodedby", Incodecby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "Usp_DescriptionFormtagging_Update", HshIn, HshOut);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return HshOut["@chvErrorStatus"].ToString();
        }
        public string InactiveFormTaggingData(int id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "Usp_DescriptionFormtagging_InActive", HshIn, HshOut);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return HshOut["@chvErrorStatus"].ToString();
        }
    }

   

}