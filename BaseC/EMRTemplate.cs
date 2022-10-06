using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class EMRTemplate
    {
        private string sConString = "";
        Hashtable HshIn;

        public EMRTemplate(string Constring)
        {
            sConString = Constring;
        }
        //Not in use
        //public int abc ()
        //{
        //    List<int> names = new List<int>();
        //    names.Add(222);
        //    names.Add(2223);
        //    names.Add(4444);
        //    for (int i = 0; i < names.Count; i++)
        //    {
        //        int s = names[i];
        //        names[i] = s;
        //    }
        //    return 0;
        //}
        //Not in use
        //public DataSet GetCategory(Int16 iHospitalLocationId, Int16 iTemplateType)
        //{
        //    hstInput = new Hashtable();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    hstInput.Add("@inyHospitalLocationID", iHospitalLocationId);
        //    hstInput.Add("@inyTemplateTypeID", iTemplateType.ToString());
        //    DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetCategorySubCategory", hstInput);
        //    return ds;
        //}
        //Not in use
        //public SqlDataReader GetTemplateNames(Int16 iHospitalLocationId)
        //{
        //    hstInput = new Hashtable();
        //    hstInput.Add("inyHospitalLocationID", iHospitalLocationId);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "UspGetTemplate1", hstInput);
        //    return objDr;
        //}
        //Not in use
        //public SqlDataReader GetTemplateNames(Int16 iHospitalLocationId, int iTemplateType)
        //{
        //    hstInput = new Hashtable();
        //    hstInput.Add("inyHospitalLocationID", iHospitalLocationId);
        //    hstInput.Add("TemplateType", iTemplateType);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "UspEMRGetClinicalTemplates", hstInput);
        //    return objDr;
        //}
        //Not in use
        //public DataSet GetFormTemplate(Int16 iHospitalLocationId, int iFormId)
        //{
        //    hstInput = new Hashtable();
        //    hstInput.Add("inyHospitalLocationID", iHospitalLocationId);
        //    hstInput.Add("intformId", iFormId);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDr = objDl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormTemplate", hstInput);
        //    return objDr;
        //}

    }
}
