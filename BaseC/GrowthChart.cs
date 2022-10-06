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
    public class GrowthChart
    {
        private string sConString = "";
        DAL.DAL objDl;

        Hashtable HshIn;

        public GrowthChart(string Constring)
        {
            sConString = Constring;
        }

        public DataSet BindGrowthChartBirthTo36Months(int HospId, int RegId, string DisplayName, string Measurmentsystem)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@chvDisplayName", DisplayName);
            HshIn.Add("@chvMeasurementSystem", Measurmentsystem);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRBirthTo36MonthsGraph", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet BindGrowthChart2Yto20Y(int HospId, int RegId, string DisplayName, int FromAge, int ToAge, string DayType) // 2 to 20 Year Length for Age
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@chvDisplayName", DisplayName);
            HshIn.Add("@intFromAge", FromAge);
            HshIn.Add("@intToAge", ToAge);
            HshIn.Add("@DayType", DayType);
            try
            {


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMR2Yto20YGraph", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet BindGrowthChart2Yto20Y(int HospId, int RegId, string DisplayName, int FromAge, int ToAge, string DayType, int GrowthChartId) // 2 to 20 Year Length for Age
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@chvDisplayName", DisplayName);
            HshIn.Add("@intFromAge", FromAge);
            HshIn.Add("@intToAge", ToAge);
            HshIn.Add("@DayType", DayType);
            HshIn.Add("@intGrowthChartId", GrowthChartId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMR2Yto20YGraph", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet BindGrowthChartFor2Yto20YHTWT(int HospId, int RegId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intRegistrationId", RegId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMR2Yto20YHTWTGraph", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //For Birth to 23 To 42 Weeks (Height for Weight) growth charts
        public DataSet BindGrowthChartForBirth23To42weeksHTWT(int HospId, int RegId, string Measurmentsystem)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@chvMeasurementSystem", Measurmentsystem);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRBirthTo36MonthsHTWTGraph", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetAgeinDays(int HospId, int RegId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string strsql = "";

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intRegistrationId", RegId);
            strsql = "SELECT ISNULL(AgeYear * 365, 0) + ISNULL(AgeMonth * 30, 0) + ISNULL(AgeDays,0) AS Days FROM Registration WITH (NOLOCK) WHERE ID = @intRegistrationId and HospitalLocationId=@inyHospitalLocationId";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindGrowthChart(int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGrowthChart", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

    }
}
