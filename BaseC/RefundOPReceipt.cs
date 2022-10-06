//----------------------------------------------------------------------------
//   Purpose           :To Get Data in OP Receipt Refund Page
//   Author            :Robin Simon
//  --------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace BaseC
{
    public class RefundOPReceipt
    {
        Int32 intReceiptNo;
        Int32 intRefundNo;
        Int16 intYearID;
        Int16 intHospitalLocationID;
        Int32 intRegistrationNo;
        Int32 intBillID;
        Int32 intEncodedBy;
        Int32 intBillNo;
        Int32 intCurrenctID;

        Double dblAmount;
        Double dblPreviousRefundedAmt;
        Double dblNetRefundableAmt;
        Double dblRefundAmt;

        String strConString;
        String strRemarks;
        String strRefundServicesXML;
        String strPaymentModeXML;
        String strOPIP;

        public RefundOPReceipt(String constring)
        {
            strConString = constring;
        }

        public Int32 ReceiptNo
        {
            get
            {
                return intReceiptNo;
            }
            set
            {
                intReceiptNo = value;
            }
        }

        public Int32 RefundNo
        {
            get
            {
                return intRefundNo;
            }
            set
            {
                intRefundNo = value;
            }
        }

        public Int16 YearID
        {
            get
            {
                return intYearID;
            }
            set
            {
                intYearID = value;
            }
        }

        public Int16 HospitalLocationId
        {
            get
            {
                return intHospitalLocationID;
            }
            set
            {
                intHospitalLocationID = value;
            }
        }

        public Int32 RegistrationNo
        {
            get
            {
                return intRegistrationNo;
            }
            set
            {
                intRegistrationNo = value;
            }
        }

        public Int32 BillID
        {
            get
            {
                return intBillID;
            }
            set
            {
                intBillID = value;
            }
        }

        public Int32 BillNo
        {
            get
            {
                return intBillNo;
            }
            set
            {
                intBillNo = value;
            }
        }

        public Int32 EncodedBy
        {
            get
            {
                return intEncodedBy;
            }
            set
            {
                intEncodedBy = value;
            }
        }

        public Int32 CurrencyID
        {
            get
            {
                return intCurrenctID;
            }
            set
            {
                intCurrenctID = value;
            }
        }

        public Double Amount
        {
            get
            {
                return dblAmount;
            }
            set
            {
                dblAmount = value;
            }
        }

        public Double PreviousRefundedAmt
        {
            get
            {
                return dblPreviousRefundedAmt;
            }
            set
            {
                dblPreviousRefundedAmt = value;
            }
        }

        public Double RefundableAmt
        {
            get
            {
                return dblNetRefundableAmt;
            }
            set
            {
                dblNetRefundableAmt = value;
            }
        }

        public Double RefundAmt
        {
            get
            {
                return dblRefundAmt;
            }
            set
            {
                dblRefundAmt = value;
            }
        }

        public String Remarks
        {
            get
            {
                return strRemarks;
            }
            set
            {
                strRemarks = value;
            }
        }

        public String XMLRefundServices
        {
            get
            {
                return strRefundServicesXML;
            }
            set
            {
                strRefundServicesXML = value;
            }
        }

        public String XMLPaymentMode
        {
            get
            {
                return strPaymentModeXML;
            }
            set
            {
                strPaymentModeXML = value;
            }
        }

        public String OPIP
        {
            get
            {
                return strOPIP;
            }
            set
            {
                strOPIP = value;
            }
        }
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           : To retrieve Receipt Details
        //   Author            : Robin Simon
        //  --------------------------------------------------------------------------
        //public SqlDataReader GetReceiptDetails()
        //{
        //    //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        //    //SqlDataReader objDr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "SELECT Id, Name FROM OccupationMaster WHERE (Active = '1') ORDER BY Name");
        //    //return objDr;


        //    Hashtable hshIn = new Hashtable();
        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        //    BaseC.ParseData bc = new BaseC.ParseData();
        //    hshIn.Add("@intBillNo",bc.ParseQ(BillNo.ToString()));
        //    hshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //    hshIn.Add("@inyYearId", bc.ParseQ(YearID.ToString()));
        //    SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.StoredProcedure, "USPGetBillRefundDetails", hshIn);
        //    return dr;
        //}

        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           : To retrieve Refund Details
        //   Author            : Robin Simon
        //  --------------------------------------------------------------------------
        //public DataSet GetRefundDetails()
        //{
        //    Hashtable hshIn = new Hashtable();
        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        //    BaseC.ParseData bc = new BaseC.ParseData();
        //    hshIn.Add("@intRefundNo", bc.ParseQ(RefundNo.ToString()));
        //    hshIn.Add("@inyHospitalLocationID",HospitalLocationId);
        //    hshIn.Add("@inyYearID", bc.ParseQ(YearID.ToString()));
        //    DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "USPGetRefundDetails", hshIn);
        //    return ds;
           
        //}
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           : To Populate the Exchange Rates
        //   Author            : Robin Simon
        //  --------------------------------------------------------------------------
        //public SqlDataReader CurrencySelectedIndexChanged()
        //{
        //    StringBuilder strSQL = new StringBuilder();
        //    Hashtable hshIn = new Hashtable();
        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        //    BaseC.ParseData bc = new BaseC.ParseData();
        //    hshIn.Add("@Active", "1");
        //    hshIn.Add("@CurrencyID", CurrencyID.ToString());
        //    strSQL.Remove(0, strSQL.Length);
        //    strSQL.Append(" select ExchangeRate from currencyExchangedetail where Active=@Active And  currencyid=@CurrencyID");
        //    SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, strSQL.ToString(), hshIn);
        //    return dr;
        //}
        //Not in Use
        ////----------------------------------------------------------------------------
        ////   Purpose           : To populate the dropdown Currency
        ////   Author            : Robin Simon
        ////  --------------------------------------------------------------------------
        //public SqlDataReader popCurrency()
        //{
        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        //    SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "Select Id, Name from CurrencyMaster");
        //    return dr;
        //}
        //Not in Use
        ////----------------------------------------------------------------------------
        ////   Purpose           : To populate the dropdown Year
        ////   Author            : Robin Simon
        ////  --------------------------------------------------------------------------
        //public SqlDataReader popYear()
        //{
        //    Hashtable hshIn = new Hashtable();
        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        //    hshIn.Add("@HospitalLocationId", HospitalLocationId);
        //    SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select yearid,Prefix from FyDocumentnorefund where hospitallocationid=@HospitalLocationId order by yearid desc", hshIn);
        //    return dr;
        //}
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           : To Save Receipt Refund Details
        //   Author            : Robin Simon
        //  --------------------------------------------------------------------------
        //public String SaveRefundOPReceipt()
        //{
        //    Hashtable hshIn = new Hashtable();
        //    Hashtable hshOut = new Hashtable();
        //    BaseC.ParseData bc = new BaseC.ParseData();
        //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        //    hshIn.Add("@inyYearID", intYearID);
        //    hshIn.Add("@inyHospitalLocationID", HospitalLocationId);
        //    hshIn.Add("@intRegistrationNo", bc.ParseQ(RegistrationNo.ToString()));
        //    hshIn.Add("@intBillID", BillID);
        //    hshIn.Add("@Remarks", bc.ParseQ(Remarks));
        //    hshIn.Add("@OPIP", OPIP);
        //    hshIn.Add("@intEncodedBy", EncodedBy);
        //    hshIn.Add("@xmlRefundedServices", XMLRefundServices);
        //    hshIn.Add("@xmlRefundModes", XMLPaymentMode);
        //    hshOut.Add("@RefundNo", SqlDbType.Int);
        //    hshOut.Add("@StrStatus", SqlDbType.VarChar);
        //    hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveRefundReceipt", hshIn, hshOut);
        //    return hshOut["@RefundNo"].ToString();
        //}
    }
}
