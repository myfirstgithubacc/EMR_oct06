using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class clsOTBooking
    {
        private string sConString = "";
        public clsOTBooking(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        StringBuilder sbSQL;
        public int HospitalLocationId = 0;
        public int LoginFacilityId = 0;
        public int UserId = 0;
        public int RegId = 0;
        public int EncounterId = 0;
        public int BookingEncounterId = 0;
        public string RegistrationNo = "";
        public string FirstName = "";
        public string MiddleName = "";
        public string LastName = "";
        public string DOB = "";
        public int AgeYears = 0;
        public int AgeMonths = 0;
        public int AgeDays = 0;
        public int Gender = 0;
        public int TheaterId = 0;
        public string OTBookingDate = "";
        public string FromTime = "";
        public string ToTime = "";
        public string Remarks = "";
        public string chrMobileNo = "";
        public string ProvisionalDiagnosis = null;

        public int OTBookingId = 0;
        public int Active = 0;
        public int AnesthesiaId = 0;
        public int IsPackage = 0;
        public string DiagnsisSearchKeyId = null;
        public StringBuilder XMLServiceDetails;
        public StringBuilder XMLResourceDetails;
        public StringBuilder XMLEquipmentDetails;
        public int IsEmergency = 0;
        public decimal intPatientAdvance = 0;
        public int isUnplannedOT = 0;
        public string chvStatusType = null;
        public int IsICURequired = 0;
        public int BloodUnit = 0;
        public int isImplantRequired = 0;
        public string ImplantRequiredRemark = null;
        public int isBloodRequired = 0;
        public int OTRequestID = 0;
        public string LMPDatetime = "";
        public int IschkVentilatorRequired = 0;
        public string RescheduledRemark = null;
        public bool isRescheduled = false;
        public int isEquipmentRequired = 0;
        public string EquipmentRequiredRemark = null;
        public bool ReExploration = false;
        public int BloodGroupId;// Akshay

        public Hashtable SaveOTBooking(clsOTBooking objOTBooking)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", objOTBooking.HospitalLocationId);
            HshIn.Add("intFacilityId", objOTBooking.LoginFacilityId);
            HshIn.Add("intOTBookingId", objOTBooking.OTBookingId);
            HshIn.Add("intRegistrationId", objOTBooking.RegId);
            HshIn.Add("chvRegistrationNo", objOTBooking.RegistrationNo);
            HshIn.Add("intEncounterId", objOTBooking.EncounterId);
            HshIn.Add("chvFirstName", objOTBooking.FirstName);
            HshIn.Add("chvMiddleName", objOTBooking.MiddleName);
            HshIn.Add("chvLastName", objOTBooking.LastName);
            HshIn.Add("dtDOB", Convert.ToDateTime(objOTBooking.DOB));
            HshIn.Add("inyAgeYears", objOTBooking.AgeYears);
            HshIn.Add("inyAgeMonths", objOTBooking.AgeMonths);
            HshIn.Add("inyAgeDays", objOTBooking.AgeDays);
            HshIn.Add("inyGender", objOTBooking.Gender);
            HshIn.Add("intTheaterId", objOTBooking.TheaterId);
            HshIn.Add("dtOTBookingDate", Convert.ToDateTime(objOTBooking.OTBookingDate));
            HshIn.Add("dtFromTime", objOTBooking.FromTime);
            HshIn.Add("dtToTime", objOTBooking.ToTime);
            HshIn.Add("chvRemarks", objOTBooking.Remarks);
            HshIn.Add("bitActive", objOTBooking.Active);
            HshIn.Add("intEncodedBy", objOTBooking.UserId);
            HshIn.Add("intAnesthesiaId", objOTBooking.AnesthesiaId);
            HshIn.Add("intBookingEncounterId", objOTBooking.BookingEncounterId);
            if (objOTBooking.XMLServiceDetails != null)
                HshIn.Add("xmlServiceDetails", objOTBooking.XMLServiceDetails.ToString());
            if (objOTBooking.XMLResourceDetails != null)
                HshIn.Add("xmlResourceDetails", objOTBooking.XMLResourceDetails.ToString());
            if (objOTBooking.XMLEquipmentDetails != null)
                HshIn.Add("xmlEquipmentDetails", objOTBooking.XMLEquipmentDetails.ToString());
            HshIn.Add("bitIsPackage", IsPackage);
            HshOut.Add("chvOTBookingNo", SqlDbType.VarChar);
            HshOut.Add("intId", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("chrMobileNo", chrMobileNo);
            HshIn.Add("chvProvisionalDiagnosis", objOTBooking.ProvisionalDiagnosis);
            HshIn.Add("intDiagnosisSearchId", objOTBooking.DiagnsisSearchKeyId);
            HshIn.Add("IsEmergency", objOTBooking.IsEmergency);
            HshIn.Add("isImplantRequired", objOTBooking.isImplantRequired);
            HshIn.Add("ImplantRequiredRemark", objOTBooking.ImplantRequiredRemark);
            HshIn.Add("isBloodRequired", objOTBooking.isBloodRequired);
            HshIn.Add("mnPatientAdvance", objOTBooking.intPatientAdvance);
            HshIn.Add("@isUnplannedOT", objOTBooking.isUnplannedOT);
            HshIn.Add("@chvStatusType", objOTBooking.chvStatusType);
            HshIn.Add("@bloodUnit", objOTBooking.BloodUnit);
            HshIn.Add("@isICURequired", objOTBooking.IsICURequired);
            HshIn.Add("@OTRequestID", objOTBooking.OTRequestID);
            HshIn.Add("@OTLMPDatetime", objOTBooking.LMPDatetime);
            HshIn.Add("@IschkVentilatorRequired", objOTBooking.IschkVentilatorRequired);
            HshIn.Add("@RescheduledRemark", objOTBooking.RescheduledRemark);
            HshIn.Add("@isRescheduled", objOTBooking.isRescheduled);
            HshIn.Add("@isEquipmentRequired", objOTBooking.isEquipmentRequired);
            HshIn.Add("@EquipmentRequiredRemark", objOTBooking.EquipmentRequiredRemark);
            HshIn.Add("@ReExploration", objOTBooking.ReExploration);
            HshIn.Add("@BloodGroupId", objOTBooking.BloodGroupId);//Akshay

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOTBooking", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable UpdateOTBookingTheater(int iOTBookingID, int iOTTheaterId, int iChangedBy, string sFromTime, string OTBookingDate, int intFacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("iOTBookingID", iOTBookingID);
            HshIn.Add("iOTTheaterId", iOTTheaterId);
            HshIn.Add("iChangedBy", iChangedBy);
            HshIn.Add("chrAppFromTime", sFromTime);
            if (OTBookingDate != string.Empty)
            {
                HshIn.Add("dtOTBookingDate", OTBookingDate);
            }
            HshIn.Add("@intFacilityId", intFacilityId);
            HshOut.Add("cErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateOTBookingTheater", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet SearchOTrelatedPatientByName(Int16 HospId, Int32 FacilityId, String sKeyword, DateTime dtfordate)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("chvKeyword", sKeyword);
                HshIn.Add("dtForDate", dtfordate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspSearchOTrelatedPatientByName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getOTBookingDetails(int HospId, int FacilityId, int OTBookingId, string sOTBookingNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intOTBookingId", OTBookingId);
                HshIn.Add("chvOTBookingNo", sOTBookingNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTBookingDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetOTScheduler(int HospId, int FacilityId, string xmlTheaterId, DateTime Fordate, string CurrentView,
                                    int statuscolor, string filteron, string filtervalue, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                if (FacilityId == 0)
                {
                    HshIn.Add("@intFacilityId", "0");

                }
                else
                {
                    HshIn.Add("@intFacilityId", FacilityId);
                }
                HshIn.Add("@xmlOTTheaterIds", xmlTheaterId);
                HshIn.Add("@chrForDate", Fordate.ToString("yyyy/MM/dd"));
                HshIn.Add("@chrDateOption", CurrentView == null ? "D" : CurrentView);
                HshIn.Add("@intStatusId", statuscolor);
                //string filteron, string filtervalue
                HshIn.Add("@filteron", filteron);
                HshIn.Add("@filtervalue", filtervalue);
                HshIn.Add("@intRegistrationId", RegistrationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTSchedule", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable CopyOTBooking(clsOTBooking objOTBooking)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", objOTBooking.HospitalLocationId);
            HshIn.Add("intFacilityId", objOTBooking.LoginFacilityId);
            HshIn.Add("intCopyOTBookingID", objOTBooking.OTBookingId);
            HshIn.Add("dtOTBookingDate", Convert.ToDateTime(objOTBooking.OTBookingDate));
            HshIn.Add("chrAppFromTime", objOTBooking.FromTime);
            HshIn.Add("intTheaterId", objOTBooking.TheaterId);
            HshIn.Add("intEncodedBy", objOTBooking.UserId);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspCopyOTTheaterBooking", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetOtDetails(int HospId, int FacilityId, int TheaterId, int DoctorId, DateTime fromDate, DateTime toDate)
        {
            return GetOtDetails(HospId, FacilityId, TheaterId, DoctorId, fromDate, toDate, "", "", "", false, true);
        }

        public DataSet GetOtDetails(int HospId, int FacilityId, int TheaterId, int DoctorId, DateTime fromDate, DateTime toDate, string RegistrationNo, string EncounterNo, string PatientName, bool VentilatorReq, bool chkReExploration)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intTheaterId", TheaterId);
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@dtDate", fromDate.ToString("yyyy-MM-dd"));
            HshIn.Add("@dtToDate", toDate.ToString("yyyy-MM-dd"));
            HshIn.Add("@chvRegistrationno", RegistrationNo);
            HshIn.Add("@chvEncounterno", EncounterNo);
            HshIn.Add("@chvPatientName", PatientName);
            if (VentilatorReq)
                HshIn.Add("@chvVentilatorReq", VentilatorReq);
            if (chkReExploration)
                HshIn.Add("@chkReExploration", chkReExploration);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetOTDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int CheckOTBookingExist(int iHospId, int iFacilityId, int iTheaterId, DateTime OTDate,
            string sFromTime, string sToTime, int iRegistrastionId, int iStatusId, int iServiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intTheaterId", iTheaterId);
                HshIn.Add("@chrOTBookingDate", OTDate.ToString("yyyy/MM/dd"));
                HshIn.Add("@intRegistrationId", iRegistrastionId);
                HshIn.Add("@chrFromTime", sFromTime);
                HshIn.Add("@chrToTime", sToTime);
                HshIn.Add("@intStatusId", iStatusId);
                HshIn.Add("@intServiceId", iServiceId);
                HshOut.Add("@chvOutput", DbType.Boolean);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCheckOTBookingExist", HshIn, HshOut);
                return Convert.ToInt16(HshOut["@chvOutput"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet FillOTList(int FacId, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            // Change by kuldeep kumar 22/01/2021 
            try
            {

                hstInput.Add("@intFacilityID", FacId);
                return objDl.FillDataSet(CommandType.Text, "SELECT TheatreId, TheatreName FROM OTTheatreMaster WITH(NOLOCK) WHERE Active = 1 AND FacilityID = @intFacilityID ", hstInput);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet BindOTCategory(int FacId, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID, Name from SurgeryClassification WITH (NOLOCK) where Active =1");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCheckITOT(string Bookingid, int iFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intFacilityID", iFacilityId);

                int timezone = 0;
                ds = objDl.FillDataSet(CommandType.Text, "select TimeZoneOffSetMinutes from FacilityMaster WITH (NOLOCK) where FacilityID = @intFacilityID ", HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    timezone = Convert.ToInt32(ds.Tables[0].Rows[0]["TimeZoneOffSetMinutes"]);
                }

                HshIn = new Hashtable();
                HshIn.Add("Bookingid", Bookingid);
                HshIn.Add("timezone", timezone);
                String qry = "select (case OTCheckintime when null then OTCheckintime else DATEADD(mi,@timezone, OTCheckintime) end) as OTCheckintime, ";
                qry += " (case OTCheckouttime when null then OTCheckouttime else DATEADD(mi, @timezone, OTCheckouttime) end) as OTCheckouttime, ";
                qry += " PACClearanceRemarks,SM.Code StatusCode, Isnull(IsUnplannedReturnToOT,0) as IsUnplannedReturnToOT,  ";
                qry += " case OTCheckintime when null then '' else dbo.getdateformatutc(OTCheckintime,'DT',@timezone) end as OTChkInTimeString, ";
                qry += " case OTCheckouttime when null then '' else dbo.getdateformatutc(OTCheckouttime,'DT',@timezone) end as OTChkOutTimeString ";
                qry += " from OTBooking WITH (NOLOCK)inner join StatusMaster SM WITH(NOLOCK) on OTBooking.StatusId = SM.StatusId where OTBookingID = @Bookingid ";
                //ds = objDl.FillDataSet(CommandType.Text, "select (case OTCheckintime when null then OTCheckintime else DATEADD(mi,@timezone, OTCheckintime) end) as OTCheckintime, (case OTCheckouttime when null then OTCheckouttime else DATEADD(mi,@timezone, OTCheckouttime) end) as OTCheckouttime from OTBooking WITH (NOLOCK) where OTBookingID=@Bookingid", hstInput);
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public DataSet GetServiceChargeSurgery(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
                      int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, int iAnesthesiaServiceId, StringBuilder xmlSurgeryServices,
                      DateTime dtOrderDate, DateTime OTCheckinTime, DateTime OTCheckoutTime, int IsMultiIncision)
        {
            return GetServiceChargeSurgery(iHospId, iFacilityId, iRegId, iEncId, iCompId, iInsId,
                            iCardId, iBedCatId, sOPIP, iOTServiceId, iAnesthesiaServiceId, xmlSurgeryServices,
                            dtOrderDate, OTCheckinTime, OTCheckoutTime, IsMultiIncision, false, DateTime.Now);

        }
        public DataSet GetServiceChargeSurgery(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
                        int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, int iAnesthesiaServiceId, StringBuilder xmlSurgeryServices,
                        DateTime dtOrderDate, DateTime OTCheckinTime, DateTime OTCheckoutTime, int IsMultiIncision, bool IsEmergency, DateTime OrderDate)
        {
            HshIn = new Hashtable();


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegId);
                HshIn.Add("iEncounterId", iEncId);
                HshIn.Add("intCompanyid", iCompId);
                HshIn.Add("iInsuranceId", iInsId);
                HshIn.Add("iCardId", iCardId);
                HshIn.Add("intBedCategoryId", iBedCatId);
                HshIn.Add("cPatientOPIP", sOPIP);
                HshIn.Add("intOTServiceId", iOTServiceId);
                HshIn.Add("intAnesthesiaServiceId", iAnesthesiaServiceId);
                HshIn.Add("xmlSurgeryServices", xmlSurgeryServices.ToString());
                HshIn.Add("dtsOrderDate", dtOrderDate);
                HshIn.Add("OTCheckinTime", OTCheckinTime);
                HshIn.Add("OTCheckoutTime", OTCheckoutTime);
                HshIn.Add("isMultiIncision", IsMultiIncision);

                HshIn.Add("iEMergencyCharge", IsEmergency);
                HshIn.Add("OrderDate", OrderDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceChargeSurgery", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }



        public DataSet GetServiceChargeSurgery(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
                      int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, int iAnesthesiaServiceId, StringBuilder xmlSurgeryServices,
                      DateTime dtOrderDate, DateTime OTCheckinTime, DateTime OTCheckoutTime, int IsMultiIncision, string xmlDoctorShare, string xmlExtraSurgeryComponent, bool IsEmergency)
        {
            Hashtable hstInput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            hstInput = new Hashtable();
            hstInput.Add("inyHospitalLocationID", iHospId);
            hstInput.Add("intFacilityId", iFacilityId);
            hstInput.Add("intRegistrationId", iRegId);
            hstInput.Add("iEncounterId", iEncId);
            hstInput.Add("intCompanyid", iCompId);
            hstInput.Add("iInsuranceId", iInsId);
            hstInput.Add("iCardId", iCardId);
            hstInput.Add("intBedCategoryId", iBedCatId);
            hstInput.Add("cPatientOPIP", sOPIP);
            hstInput.Add("intOTServiceId", iOTServiceId);
            hstInput.Add("intAnesthesiaServiceId", iAnesthesiaServiceId);
            hstInput.Add("xmlSurgeryServices", xmlSurgeryServices.ToString());
            hstInput.Add("dtsOrderDate", dtOrderDate);
            hstInput.Add("OTCheckinTime", OTCheckinTime);
            hstInput.Add("OTCheckoutTime", OTCheckoutTime);
            hstInput.Add("isMultiIncision", IsMultiIncision);
            hstInput.Add("xmlDoctorShare", xmlDoctorShare);
            hstInput.Add("xmlExtraSurgeryComponent", xmlExtraSurgeryComponent);
            hstInput.Add("iEMergencyCharge", IsEmergency);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceChargeSurgery", hstInput);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet GetServiceChargeSurgeryWithMultipleANServiceId(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
        //                     int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, String iAnesthesiaServiceId, StringBuilder xmlSurgeryServices,
        //                     DateTime dtOrderDate, DateTime OTCheckinTime, DateTime OTCheckoutTime, int IsMultiIncision, string xmlDoctorShare, string xmlExtraSurgeryComponent, bool IsEmergency, bool IsHighRiskSurgery,
        //                     bool IsHighRiskStat = false)
        //{
        //    Hashtable hstInput = new Hashtable();

        //    DataSet ds = new DataSet();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //    hstInput = new Hashtable();
        //    hstInput.Add("inyHospitalLocationID", iHospId);
        //    hstInput.Add("intFacilityId", iFacilityId);
        //    hstInput.Add("intRegistrationId", iRegId);
        //    hstInput.Add("iEncounterId", iEncId);
        //    hstInput.Add("intCompanyid", iCompId);
        //    hstInput.Add("iInsuranceId", iInsId);
        //    hstInput.Add("iCardId", iCardId);
        //    hstInput.Add("intBedCategoryId", iBedCatId);
        //    hstInput.Add("cPatientOPIP", sOPIP);
        //    hstInput.Add("intOTServiceId", iOTServiceId);
        //    hstInput.Add("intAnesthesiaServiceId", iAnesthesiaServiceId);
        //    hstInput.Add("xmlSurgeryServices", xmlSurgeryServices.ToString());
        //    hstInput.Add("dtsOrderDate", dtOrderDate);
        //    hstInput.Add("OTCheckinTime", OTCheckinTime);
        //    hstInput.Add("OTCheckoutTime", OTCheckoutTime);
        //    hstInput.Add("isMultiIncision", IsMultiIncision);
        //    hstInput.Add("xmlDoctorShare", xmlDoctorShare);
        //    hstInput.Add("xmlExtraSurgeryComponent", xmlExtraSurgeryComponent);
        //    hstInput.Add("iEMergencyCharge", IsEmergency);
        //    hstInput.Add("IsHighRiskSurgery", IsHighRiskSurgery);
        //    hstInput.Add("IsHighRiskStat", IsHighRiskStat);
        //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceChargeSurgery", hstInput);
        //    return ds;
        //}

        //public DataSet GetServiceChargeSurgeryWithMultipleANServiceId(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
        //                  int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, String iAnesthesiaServiceId, StringBuilder xmlSurgeryServices,
        //                  DateTime dtOrderDate, DateTime OTCheckinTime, DateTime OTCheckoutTime, int IsMultiIncision, string xmlDoctorShare, string xmlExtraSurgeryComponent, bool IsEmergency, bool IsHighRiskSurgery,
        //                  int OTBookingId, bool IsGradBased, bool IsHighRiskStat = false)
        //{
        //    Hashtable hstInput = new Hashtable();

        //    DataSet ds = new DataSet();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //    hstInput = new Hashtable();
        //    hstInput.Add("inyHospitalLocationID", iHospId);
        //    hstInput.Add("intFacilityId", iFacilityId);
        //    hstInput.Add("intRegistrationId", iRegId);
        //    hstInput.Add("iEncounterId", iEncId);
        //    hstInput.Add("intCompanyid", iCompId);
        //    hstInput.Add("iInsuranceId", iInsId);
        //    hstInput.Add("iCardId", iCardId);
        //    hstInput.Add("intBedCategoryId", iBedCatId);
        //    hstInput.Add("cPatientOPIP", sOPIP);
        //    hstInput.Add("intOTServiceId", iOTServiceId);
        //    hstInput.Add("intAnesthesiaServiceId", iAnesthesiaServiceId);
        //    hstInput.Add("xmlSurgeryServices", xmlSurgeryServices.ToString());
        //    hstInput.Add("dtsOrderDate", dtOrderDate);
        //    hstInput.Add("OTCheckinTime", OTCheckinTime);
        //    hstInput.Add("OTCheckoutTime", OTCheckoutTime);
        //    hstInput.Add("isMultiIncision", IsMultiIncision);
        //    hstInput.Add("xmlDoctorShare", xmlDoctorShare);
        //    hstInput.Add("xmlExtraSurgeryComponent", xmlExtraSurgeryComponent);
        //    hstInput.Add("iEMergencyCharge", IsEmergency);
        //    hstInput.Add("IsHighRiskSurgery", IsHighRiskSurgery);
        //    hstInput.Add("IsHighRiskStat", IsHighRiskStat);
        //    hstInput.Add("IsGradBased", IsGradBased);

        //    if (OTBookingId > 0)
        //    {
        //        hstInput.Add("intOTBookingId", OTBookingId);
        //    }

        //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceChargeSurgery", hstInput);
        //    return ds;
        //}

        //palendra
        public string ShowAndHideOTDisplay(string OTBookingID, int EncodedBy, int ISShowInOTDisplay)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intEncodedBy", EncodedBy);
                qry.Append("UPDATE OTBooking SET OTDashBoardStatus = '" + ISShowInOTDisplay + "',");
                qry.Append(" LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() ");
                qry.Append(" WHERE OTBookingID in (" + OTBookingID + ")");

                objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }

            return "Succeeded !";
        }
        //Palendra
        public DataSet GetServiceChargeSurgeryWithMultipleANServiceId(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId, int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, String iAnesthesiaServiceId, StringBuilder xmlSurgeryServices, DateTime dtOrderDate, DateTime OTCheckinTime, DateTime OTCheckoutTime, int IsMultiIncision, string xmlDoctorShare, string xmlExtraSurgeryComponent, bool IsEmergency, bool IsHighRiskSurgery, int OTBookingId, bool IsHighRiskStat = false)
        {
            return GetServiceChargeSurgeryWithMultipleANServiceId(iHospId, iFacilityId, iRegId, iEncId, iCompId, iInsId, iCardId, iBedCatId, sOPIP, iOTServiceId, iAnesthesiaServiceId, xmlSurgeryServices, dtOrderDate, OTCheckinTime, OTCheckoutTime, IsMultiIncision, xmlDoctorShare, xmlExtraSurgeryComponent, IsEmergency, IsHighRiskSurgery, OTBookingId, IsHighRiskStat, 0);
        }
        public DataSet GetServiceChargeSurgeryWithMultipleANServiceId(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
                             int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, String iAnesthesiaServiceId, StringBuilder xmlSurgeryServices,
                             DateTime dtOrderDate, DateTime OTCheckinTime, DateTime OTCheckoutTime, int IsMultiIncision, string xmlDoctorShare, string xmlExtraSurgeryComponent, bool IsEmergency, bool IsHighRiskSurgery, int OTBookingId, bool IsHighRiskStat = false, int iOTEquiID = 0)
        {
            HshIn = new Hashtable();


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegId);
                HshIn.Add("iEncounterId", iEncId);
                HshIn.Add("intCompanyid", iCompId);
                HshIn.Add("iInsuranceId", iInsId);
                HshIn.Add("iCardId", iCardId);
                HshIn.Add("intBedCategoryId", iBedCatId);
                HshIn.Add("cPatientOPIP", sOPIP);
                HshIn.Add("intOTServiceId", iOTServiceId);
                HshIn.Add("intAnesthesiaServiceId", iAnesthesiaServiceId);
                HshIn.Add("xmlSurgeryServices", xmlSurgeryServices.ToString());
                HshIn.Add("dtsOrderDate", dtOrderDate);
                HshIn.Add("OTCheckinTime", OTCheckinTime);
                HshIn.Add("OTCheckoutTime", OTCheckoutTime);
                HshIn.Add("isMultiIncision", IsMultiIncision);
                HshIn.Add("xmlDoctorShare", xmlDoctorShare);
                HshIn.Add("xmlExtraSurgeryComponent", xmlExtraSurgeryComponent);
                HshIn.Add("iEMergencyCharge", IsEmergency);
                HshIn.Add("IsHighRiskSurgery", IsHighRiskSurgery);
                HshIn.Add("IsHighRiskStat", IsHighRiskStat);
                HshIn.Add("iOTEquiID", iOTEquiID);

                if (OTBookingId > 0)
                {
                    HshIn.Add("intOTBookingId", OTBookingId);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceChargeSurgery", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string savePatientTransferWardtoOT(string SaveEvent, int TransferId, int HospId, int FacilityId, int RegistrationId,
                                            int EncounterId, string Remarks, int EncodedBy, int TheatreID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrSaveEvent", SaveEvent); // A-Transfer Ward to OT, B-Transfer Ward to OT Cancel, C-Ack Transfer in OT, D-Cancel Transfer in OT, E-Transfer to Ward, F-Ward Ack
            HshIn.Add("@intTransferId", TransferId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intTheatreID", TheatreID);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePatientTransferWardtoOT", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPatientTransferWardtoOTRemarks(int RegistrationId, int EncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);

                string str = "SELECT ID AS TransferId, WardRemarks , Isnull(TheatreID,0) TheatreID FROM PatientTransferWardtoOT WITH (NOLOCK) WHERE EncounterId = @intEncounterId AND RegistrationId = @intRegistrationId AND Active = 1 ORDER BY ID DESC";

                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getPatientTransferWardtoOT(int FacilityId, Int64 RegistrationNo, string EncounterNo, string PatientName, int OTTransferStatus)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@intOTTransferStatus", OTTransferStatus); //0 Patient From Ward, 1 Pre-Operative Patient List, 2 Patient In OT, 3 Post-Operative Patient List

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientTransferWardtoOT", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getDefaultOTTheatreId(int FacilityId, int RegistrationId, int EncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);

                string strQry = "SELECT TOP 1 ob.TheaterId " +
                                " FROM OTBooking ob WITH (NOLOCK) " +
                                " INNER JOIN StatusMaster st WITH (NOLOCK) on ob.StatusId=st.StatusId AND ob.Active = 1 AND st.Active = 1 AND st.Code <> 'OT-POST' " +
                                " WHERE EncounterID = @intEncounterId " +
                                " AND RegistrationID = @intRegistrationId " +
                                " AND ob.FacilityID = @intFacilityId " +
                                " ORDER BY ob.OTBookingID DESC ";


                return objDl.FillDataSet(CommandType.Text, strQry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveOTBreaks(int BreakId, int TheaterId, int HospId, int FacilityId, string BreakName, string BreakDate,
                                   string StartTime, string EndTime, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intBreakId", BreakId);
            HshIn.Add("@intTheaterId", TheaterId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBreakName", BreakName);
            HshIn.Add("@chrBreakDate", BreakDate);
            if (StartTime != string.Empty)
            {
                HshIn.Add("@chrStartTime", StartTime);
            }
            if (StartTime != string.Empty)
            {
                HshIn.Add("@chrEndTime", EndTime);
            }
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOTBreaks", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public int ExistOTBreak(int FacilityId, int TheaterId, string sBreakDate, string sStartTime, string sEndTime)
        {
            int BreakId = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@intTheaterId", TheaterId);
                HshIn.Add("@chrBreakDate", sBreakDate);
                HshIn.Add("@chrStartTime", sStartTime);
                HshIn.Add("@chrEndTime", sEndTime);

                string sQuery = "SELECT ID as BreakId " +
                                " FROM OTBreakList WITH (NOLOCK) " +
                                " WHERE TheaterId=@intTheaterId AND CONVERT(VARCHAR(10), BreakDate,111)=@chrBreakDate AND Active=1 " +
                                " AND SUBSTRING(@chrStartTime,0,6) BETWEEN CONVERT(VARCHAR(5), StartTime,108) AND CONVERT(VARCHAR(5), EndTime,108) " +
                                " AND SUBSTRING(@chrEndTime,0,6) BETWEEN CONVERT(VARCHAR(5), StartTime,108) AND CONVERT(VARCHAR(5), EndTime,108) " +
                                " AND FacilityID=@intFacilityID ";

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    BreakId = Convert.ToInt32(dr["BreakId"]);
                }
                dr.Close();
                return BreakId;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int ExistOTBooking(int FacilityId, int TheaterId, string OTDate, string sStartTime, string sEndTime)
        {
            int OTBookingId = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@intTheaterId", TheaterId);
                HshIn.Add("@chrOTDate", OTDate);
                HshIn.Add("@chrStartTime", sStartTime);
                HshIn.Add("@chrEndTime", sEndTime);

                string sQuery = "SELECT OTBookingId FROM OTBooking WITH (NOLOCK) " +
                                " WHERE TheaterId=@intTheaterId AND CONVERT(VARCHAR(10), OTBookingDate,111)=@chrOTDate AND Active=1 " +
                                " AND SUBSTRING(@chrStartTime,0,6) BETWEEN CONVERT(VARCHAR(5), FromTime,108) AND CONVERT(VARCHAR(5), ToTime,108) " +
                                " AND SUBSTRING(@chrEndTime,0,6) BETWEEN CONVERT(VARCHAR(5), FromTime,108) AND CONVERT(VARCHAR(5), ToTime,108) " +
                                " AND FacilityID=@intFacilityID ";

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    OTBookingId = Convert.ToInt32(dr["OTBookingId"]);
                }
                dr.Close();
                return OTBookingId;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getOTBreakDetails(int BreakId, int TheaterId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intBreakId", BreakId);
                HshIn.Add("@intTheaterId", TheaterId);
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTBreakDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public void cancelOTBreak(int BreakId, string CancelReason, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@intBreakId", BreakId);
            HshIn.Add("@chvCancelRemarks", CancelReason);
            HshIn.Add("@intLastChangedBy", UserId);
            try
            {
                objDl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update OTBreakList Set CancelRemarks = @chvCancelRemarks, Active=0, LastChangedBy = @intLastChangedBy, LastChangedDate = GETUTCDATE() where ID = @intBreakId", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public int GetEncodedBy(int hospitalLocationId, int FacilityId, int OTBookingId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsnIN = new Hashtable();
            hsnIN.Add("@intHospitalLocationId", hospitalLocationId);
            hsnIN.Add("@intFacilityId", FacilityId);
            hsnIN.Add("@OTBookingId", OTBookingId);
            try
            {
                string strQuery = "select Encodedby from OTBooking where OTBookingId=@OTBookingId and FacilityId=@intFacilityId and HospitalLocationId=@intHospitalLocationId";
                int x = (int)objDl.ExecuteScalar(CommandType.Text, strQuery, hsnIN);
                return x;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetOTAlertForAdmission(int hospitalLocationId, int FacilityId, int TheatreId, int WardId, Int64 RegNo, string EncounterNo, bool x)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsnIN = new Hashtable();
            hsnIN.Add("@intHospitalLocationId", hospitalLocationId);
            hsnIN.Add("@intFacilityId", FacilityId);
            hsnIN.Add("@intTheatreId", TheatreId);
            hsnIN.Add("@intWardId", WardId);
            hsnIN.Add("@intRegistrationNo", RegNo);
            hsnIN.Add("@chvIPNo", EncounterNo);
            hsnIN.Add("@bitTodayAdmission", x);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspOTGetAdmissionAlert", hsnIN);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }



        public DataSet GetSurgeryOtherDetails(int otbooking)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intOTBookingId", otbooking);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspOTGetSurgeryOtherDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public Hashtable SaveOTAntibioticDetail(int OTBookingId, int surgeryId, DateTime t1, DateTime t2, string t3, string t4, string xmlAntibioticId, int Encodedby, int FacilityId, String Remarks)
        {
            Hashtable hsIN = new Hashtable();
            Hashtable hsOUT = new Hashtable();

            hsIN.Add("@intOTBookingId", OTBookingId);
            hsIN.Add("@intSurgeryId", surgeryId);
            hsIN.Add("@dtIntubationStartTime", t1);
            hsIN.Add("@dtIncisionTime", t2);
            hsIN.Add("@dtAntibioticProphylacticTime", t3);
            hsIN.Add("@dtBallooningTime", t4);
            hsIN.Add("@xmlAntibioticId", xmlAntibioticId);
            hsIN.Add("@intEncodedBy", Encodedby);
            hsOUT.Add("@chvErrorStatus", SqlDbType.VarChar);
            hsIN.Add("@intFacilityId", FacilityId);
            hsIN.Add("@Remarks", Remarks);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsOUT = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspOTSaveSurgeryOtherDetails", hsIN, hsOUT);
            return hsOUT;

        }

        //public DataSet GetServiceChargeSurgeryWithMultipleANServiceId(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
        //                    int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, String iAnesthesiaServiceId, StringBuilder xmlSurgeryServices,
        //                    DateTime dtOrderDate, DateTime OTCheckinTime, DateTime OTCheckoutTime, int IsMultiIncision, string xmlDoctorShare, string xmlExtraSurgeryComponent, bool IsEmergency, bool IsHighRiskSurgery,
        //                    int OTBookingId, bool IsHighRiskStat = false)
        //{
        //    Hashtable hstInput = new Hashtable();

        //    DataSet ds = new DataSet();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //    hstInput = new Hashtable();
        //    hstInput.Add("inyHospitalLocationID", iHospId);
        //    hstInput.Add("intFacilityId", iFacilityId);
        //    hstInput.Add("intRegistrationId", iRegId);
        //    hstInput.Add("iEncounterId", iEncId);
        //    hstInput.Add("intCompanyid", iCompId);
        //    hstInput.Add("iInsuranceId", iInsId);
        //    hstInput.Add("iCardId", iCardId);
        //    hstInput.Add("intBedCategoryId", iBedCatId);
        //    hstInput.Add("cPatientOPIP", sOPIP);
        //    hstInput.Add("intOTServiceId", iOTServiceId);
        //    hstInput.Add("intAnesthesiaServiceId", iAnesthesiaServiceId);
        //    hstInput.Add("xmlSurgeryServices", xmlSurgeryServices.ToString());
        //    hstInput.Add("dtsOrderDate", dtOrderDate);
        //    hstInput.Add("OTCheckinTime", OTCheckinTime);
        //    hstInput.Add("OTCheckoutTime", OTCheckoutTime);
        //    hstInput.Add("isMultiIncision", IsMultiIncision);
        //    hstInput.Add("xmlDoctorShare", xmlDoctorShare);
        //    hstInput.Add("xmlExtraSurgeryComponent", xmlExtraSurgeryComponent);
        //    hstInput.Add("iEMergencyCharge", IsEmergency);
        //    hstInput.Add("IsHighRiskSurgery", IsHighRiskSurgery);
        //    hstInput.Add("IsHighRiskStat", IsHighRiskStat);

        //    if (OTBookingId > 0)
        //    {
        //        hstInput.Add("intOTBookingId", OTBookingId);
        //    }

        //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceChargeSurgery", hstInput);
        //    return ds;
        //}
        public string SaveOTBookingFollowupRemarks(int iOTBookingId, int iFacilityId, string FollowupRemarks, int EncodedBy, int iHospitalLocationId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intOTBookingId", iOTBookingId);
            HshIn.Add("@chvFollowupRemarks", FollowupRemarks);
            HshIn.Add("@intEncodedBy", EncodedBy);


            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOTBookingFollowupRemarks", HshIn, HshOut);
            return Convert.ToString(HshOut["@chvErrorStatus"]);
        }

        public string GetOTBookingFollowupRemarks(int OTBookingId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intOTBookingId", OTBookingId);

            return (objDl.ExecuteScalar(CommandType.StoredProcedure, "uspGetOTBookingFollowupRemarks", HshIn)).ToString();
        }

        public DataSet GetPackageSurgery(int iHospitalLocationID, int iFacilityId, int iOTBookingID, int iCompanyID, int iBedCategoryID)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@OTBookingID", iOTBookingID);
            HshIn.Add("@CompanyID", iCompanyID);
            HshIn.Add("@BedCategoryID", iBedCategoryID);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageSurgery", HshIn);
            return ds;
        }

        public Hashtable CheckOTBookingSlot(int NewAppTime, int TheatreId, string OTDate, int SlotDurationMinute, int FacilityId, int intHospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@NewAppTime", NewAppTime);
            HshIn.Add("@intTheatreId", TheatreId);
            HshIn.Add("@chrOTDate", OTDate);
            HshIn.Add("@SlotDurationMinute", SlotDurationMinute);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspCheckOTBookingSlot", HshIn, HshOut);
            return HshOut;
        }

        public DataTable getSurgeryEquipmentTagging(int FacilityId, int ServiceId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intServiceId", ServiceId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspOTGetSurgeryEquipmentTagging", HshIn);

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
            return ds.Tables[0];
        }

        public string saveSurgeryEquipmentTagging(int TaggingId, int ServiceId, int EquipmentId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();


            HshIn.Add("@intTaggingId", TaggingId);
            HshIn.Add("@intServiceId", ServiceId);
            HshIn.Add("@intEquipmentId", EquipmentId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspOTSaveSurgeryEquipmentTagging", HshIn, HshOut);
            return Convert.ToString(HshOut["@chvErrorStatus"]);
        }

        public DataTable GetHospitalHolidays(int iHospitalLocationID, DateTime BookingDate)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@dtBookingDate", BookingDate);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspOTGetHospitalHolidays", HshIn);
            return ds.Tables[0];
        }

        public DataTable GetSurgeryDetails(int HospitalLocationId, int FacilityId, int EncounterId, int OTBookingID)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intOTBookingID", OTBookingID);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspOTGetSurgeryDetails", HshIn);

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
            return ds.Tables[0];
        }
        public DataSet CheckOTRequestForPatient(int EncounterId)
        {
            HshIn = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckOTRequestForPatientV3", HshIn);

        }
    }
}
