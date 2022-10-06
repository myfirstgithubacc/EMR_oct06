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
    public partial class clsBb
    {
        // Dublicate methods exist in clsBb class 

        //public string SaveKitTypeMaster(int KitTypeId, int HospitalLocationId, int FacilityId,
        //                       string KitTypeName, int Active, int UserId)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    HshIn.Add("@intKitTypeId", KitTypeId);
        //    HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //    HshIn.Add("@intFacilityId", FacilityId);
        //    HshIn.Add("@chvKitName", KitTypeName );

        //    HshIn.Add("@bitActive", Active);
        //    HshIn.Add("@intEncodedBy", UserId);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveKitTypeMaster", HshIn, HshOut);

        //    return HshOut["@chvErrorStatus"].ToString();
        //}
        //public DataSet getKitTypeMaster(int KitTypeId, int HospitalLocationId, int FacilityId, int Active)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();

        //        HshIn.Add("@intKitTypeId", KitTypeId);
        //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@bitActive", Active);

        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitTypeMaster", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        //public String SaveKitReceivingEntry(int KitId, int HospitalLocationId, int FacilityId, int KitTypeId, string KitName, string LotNo, 
        //    DateTime  ManufactureDate, DateTime  ExpiryDate, int NoOfKit, int Active , int UserId)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    HshIn.Add("@intKitId", KitId);
        //    HshIn.Add("@intHospitalLocationId", HospitalLocationId);
        //    HshIn.Add("@intFacilityId", FacilityId);
        //    HshIn.Add("@intKitTypeId", KitTypeId);
        //    HshIn.Add("@chvKitName", KitName);
        //    HshIn.Add("@chvLotNo", LotNo );
        //    HshIn.Add("@dtManufactureDate", ManufactureDate);
        //    HshIn.Add("@dtExpiryDate", ExpiryDate);            
        //    HshIn.Add("@intNoOfKits", NoOfKit  );
        //    HshIn.Add("@bitActive", Active);
        //    HshIn.Add("@intEncodedBy", UserId);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveKitReceivingEntry", HshIn, HshOut);

        //    return HshOut["@chvErrorStatus"].ToString();

        //}
        //public DataSet GetKitReceivingEntry(int KitId, int HospitalLocationId, int FacilityId, int Active)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();

        //        HshIn.Add("@intKitId", KitId);
        //        HshIn.Add("@intHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@bitActive", Active);

        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitReceivingEntry", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        //public DataSet GetKitReceivingEntryByKitTypeId(int KitTypeId, int HospitalLocationId, int FacilityId, int Active)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();

        //        HshIn.Add("@intKitTypeId", KitTypeId);
        //        HshIn.Add("@intHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@bitActive", Active);

        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitReceivingEntryByKitTypeId", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        //public DataSet getBloodBankTransactionType()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //        string qry = " SELECT StatusId, [Status], StatusType as Type, Code   FROM StatusMaster " +
        //                     " WHERE StatusType IN ('BBDocumentType') " +
        //                     " AND Active = 1 ORDER BY Status, SequenceNo ";

        //        ds = (DataSet)objDl.FillDataSet(CommandType.Text, qry);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;

        //}
    }
}
