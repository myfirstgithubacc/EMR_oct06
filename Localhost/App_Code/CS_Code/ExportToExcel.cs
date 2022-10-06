using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

/// <summary>
/// Summary description for ExportToExcel
/// </summary>
public class ExportToExcel
{
    string sConString = "";
    Hashtable hstInput;
    Hashtable houtPara;
    DAL.DAL DlObj;
    DL_Funs ff = new DL_Funs();
   // BaseC.Patient bc;

    public ExportToExcel(string Constring)
    {
        sConString = Constring;
        DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
       // bc = new BaseC.Patient(sConString);
	}

    public DataSet ExportToExcelPayer(int CompanyId, int FacilityId)
    {

        DataSet ds = new DataSet();
        Hashtable hst = new Hashtable();
        hst.Add("@CompanyId", CompanyId);
        hst.Add("@FacilityId", FacilityId);


        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspCompanyDetailList", hst);
        ds.Dispose();
        hst = null;
        return ds;
    }

    public DataSet ExportToExcelEmployeeList(int EmployeeType, int DepartmentId,int FacilityId)
    {
  


        DataSet ds = new DataSet();
        Hashtable hst = new Hashtable();
        hst.Add("@EmployeeType", EmployeeType);
        hst.Add("@DepartmentId", DepartmentId);
         hst.Add("@FacilityId", FacilityId);


         ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEmployeeList", hst);
        ds.Dispose();
        hst = null;
        return ds;
    }
    public DataSet ExportToExcelItemOfServiceList(int DepartmentId, int SubdeptId ,int FacilityId)
    {

        DataSet ds = new DataSet();
        Hashtable hst = new Hashtable();
        
        hst.Add("@DepartmentId", DepartmentId);        
        hst.Add("@SubdeptId", SubdeptId);
        hst.Add("@FacilityId", FacilityId);


        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspgetItemofServiceMaster", hst);
        ds.Dispose();
        hst = null;
        return ds;
    }

     public DataSet ExportToExcelPackagedetailList(int Packageid ,int FacilityId)
    {

        DataSet ds = new DataSet();
        Hashtable hst = new Hashtable();        
        hst.Add("@Packageid", Packageid);      
      
        hst.Add("@FacilityId", FacilityId);


        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspPackagedetailList", hst);
        ds.Dispose();
        hst = null;
        return ds;
    }

 
}
