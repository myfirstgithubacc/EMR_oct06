using System;
using System.Data;

namespace BaseC
{
    public class Dates
    {
        string sConString = "";
        DataSet ds;
        DAL.DAL objDl;
        public Dates(string Constring)
        {
            sConString = Constring;
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }
        public DataSet GetFyYear()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            // string str = "select distinct Fy_Year as value ,  Fy_Year as text  from Admissions_Monthly  order by Fy_Year desc ";
            string str = "select distinct Fy_Year as value ,  CONVERT(varchar, Fy_Year) +'-'+ convert(varchar, Fy_Year+1) as text  from Admissions_Monthly  order by Fy_Year desc ";


            try
            {

                return objDl.FillDataSet(CommandType.Text, str);
            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }

        }
    }
}
