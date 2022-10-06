using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Web.Caching;
using System.Security.Cryptography;


/// <summary>
/// Developed By Manmohan
/// </summary>

public class common
{
    public common()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //private enum eFieldType : int
    //{
    //    [StringValue("N")]
    //    Numeric = 1
    //}
    /// <summary>
    /// By PADAM To Get FileToByteArray.
    /// </summary>
    /// <param name="_FileName"></param>
    /// <returns></returns>

    public static byte[] FileToByteArray(string _FileName)
    {
        byte[] _Buffer = null;

        try
        {
            // Open file for reading
            System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            // attach filestream to binary reader
            System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);

            // get total byte length of the file
            long _TotalBytes = new System.IO.FileInfo(_FileName).Length;

            // read entire file into buffer
            _Buffer = _BinaryReader.ReadBytes((Int32)_TotalBytes);

            // close file reader
            _FileStream.Close();
            _FileStream.Dispose();
            _BinaryReader.Close();
        }
        catch (Exception _Exception)
        {
            Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            throw _Exception;
        }

        return _Buffer;
    }
    /// <summary>
    /// PADAM to Convert from Byte arry to Filestream
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ByteArrayToFile(string fileName, byte[] byteArray, out string filedata)
    {
        FileStream fs = new FileStream(fileName + ".xml", FileMode.Create, FileAccess.ReadWrite);
        BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
        bw.Write(byteArray);
        bw.Close();
        fs.Close();
        filedata = fileName;
        return filedata;
    }
    public static string getEnumStrVal(Enum value)
    {
        // Get the type
        Type type = value.GetType();

        // Get fieldinfo for this type
        FieldInfo fieldInfo = type.GetField(value.ToString());
        // Get the stringvalue attributes
        StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
        // Return the first if there was a match.

        return (attribs.Length > 0 ? myStr(attribs[0].StringValue) : string.Empty);
    }
    public static bool FillDropDowns(string Query, System.Web.UI.WebControls.DropDownList ddl, string Dm, string Vm, DataSet ds, int tableindx)
    {
        ddl.DataSource = null;
        ddl.DataSource = ds.Tables[tableindx];
        ddl.DataTextField = Dm;
        ddl.DataValueField = Vm;
        ddl.DataBind();
        return true;
    }
    public static int myLen(object strVal)
    {
        try
        {
            return myStr(strVal).Trim().Length;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return 0;
    }

    public static string myStr(object strVal)
    {
        string retVal = string.Empty;
        try
        {
            if (strVal != null && strVal != DBNull.Value)
            {
                retVal = Convert.ToString(strVal);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return retVal;
    }

    public static string myStr(object strVal, bool isEscapeChar)
    {
        string retVal = string.Empty;
        try
        {
            if (strVal != null && strVal != DBNull.Value)
            {
                retVal = Convert.ToString(strVal);
                if (isEscapeChar)
                {
                    retVal = escapeCharString(retVal, false);
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return retVal;
    }

    public static string escapeCharString(string val, bool isLike)
    {
        string strVal = val;
        try
        {
            strVal = strVal.Replace("'", "''");
            if (isLike)
            {
                strVal = strVal.Replace("_", "[_]");
                strVal = strVal.Replace("%", "[%]");
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return strVal;
    }

    public static int myInt(object num)
    {
        try
        {
            if (isNumeric(myStr(num).Trim()))
            {
                return int.Parse(myStr((int)myDbl(num)));
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return 0;
    }

    public static int CalculateTotalPages(int TotalRecord, int PageSize)
    {
        try
        {
            if (TotalRecord == 0)
                return 0;

            var result = (TotalRecord / PageSize) + ((TotalRecord % PageSize) > 0 ? 1 : 0);

            return result;
        }
        catch 
        {

            return 0;
        }
       
    }
    public static long myLong(object num)
    {
        try
        {
            if (isNumeric(myStr(num).Trim()))
            {
                return long.Parse(myStr((long)myDbl(num)));
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return 0;
    }

    public static double myDbl(object num)
    {
        try
        {
            if (isNumeric(myStr(num).Trim()))
            {
                return Convert.ToDouble(myStr(num).Trim());
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return 0;
    }

    public static decimal myDec(object num)
    {
        try
        {
            if (isNumeric(myStr(num).Trim()))
            {
                return Convert.ToDecimal(myStr(num).Trim());
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return 0;
    }

    public static DateTime myDate(object date)
    {
        System.Globalization.DateTimeFormatInfo myDTFI = new System.Globalization.CultureInfo("en-gb").DateTimeFormat;

        DateTime retDate = new DateTime();
        retDate = Convert.ToDateTime(DateTime.Now, myDTFI);
        try
        {
            if (date != null && date != DBNull.Value && myLen(date) > 0)
            {
                //bool isVal = DateTime.TryParse(myStr(date), System.Globalization.DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out retDate);
                retDate = Convert.ToDateTime(date, myDTFI);
            }
        }
        catch (Exception Ex)
        {
            // throw Ex;
        }
        return retDate;
    }

    public static string GetDateInYYYYMMDDFormate(string Date)
    {
        return common.myDate(Date).ToString("yyyy/MM/dd");
    }

    public static bool myBool(object val)
    {
        try
        {
            val = myStr(val).Trim().ToUpper();
            if (val.Equals("1") || val.Equals("TRUE"))
            {
                return true;
            }
            else if (val.Equals("0") || val.Equals("FALSE"))
            {
                return false;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return false;
    }

    public static bool isNumeric(object num)
    {
        try
        {
            double retNum;
            return Double.TryParse(myStr(num), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return false;
    }

    public static string uniqueSessionId()
    {
        string retVal = string.Empty;
        try
        {
            retVal = HttpContext.Current.Session.SessionID + myStr(HttpContext.Current.Session["UserId"]);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return retVal;
    }

    public static DateTime myDate_withFormat(object date, string dateFormat, string dateSeprator)
    {
        DateTime retDate = DateTime.Now;
        try
        {
            if (myLen(date) > 0)
            {
                System.Globalization.DateTimeFormatInfo DFI = new System.Globalization.DateTimeFormatInfo();

                if (myLen(dateFormat) > 0)
                {
                    DFI.ShortDatePattern = dateFormat;
                }
                else
                {
                    DFI.ShortDatePattern = "dd/MMM/yyyy HH:mm";
                }

                if (myLen(dateSeprator) > 0)
                {
                    DFI.DateSeparator = dateSeprator;
                }
                else
                {
                    DFI.DateSeparator = "/";
                }

                //DFI.ShortDatePattern = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
                //DFI.DateSeparator = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator;

                bool isDate = DateTime.TryParse(myStr(date), DFI, System.Globalization.DateTimeStyles.None, out retDate);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return retDate;
    }

    public static DataTable setSelectInCombo(DataTable dtbl, string strID, string strVal)
    {
        DataTable dt = dtbl;
        try
        {
            if (myLen(strID) != 0 && myLen(strVal) != 0)
            {
                DataRow dr = dt.NewRow();
                dr[strID] = 0;
                dr[strVal] = "[ Select ]";

                dt.Rows.InsertAt(dr, 0);
                return dt;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return dt;
    }

    public static void msgShow(Page page, string msg)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<script type = 'text/javascript'>");

            sb.Append("window.onload=function(){");

            sb.Append("alert('");

            sb.Append(msg);

            sb.Append("')};");

            sb.Append("</script>");

            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", sb.ToString());
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public static void msgShowWithAlert(Page page, string msg)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<script>");

            sb.Append("alert('" + msg + "');");

            sb.Append("</script>");

            page.Response.Write(sb);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public static DataSet addSnoColumn(DataSet DS)
    {
        try
        {
            if (!DS.Tables[0].Columns.Contains("SNO"))
            {
                DataColumn colSNO = new DataColumn("SNO", typeof(Int32));

                DS.Tables[0].Columns.Add(colSNO);
                colSNO.SetOrdinal(0);
                if (DS.Tables[0].Rows.Count > 0)
                {
                    for (int rowIdx = 0; rowIdx < DS.Tables[0].Rows.Count; rowIdx++)
                    {
                        DS.Tables[0].Rows[rowIdx]["SNO"] = rowIdx + 1;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return DS;
    }

    public static string setXmlTable(ref ArrayList coll)
    {
        StringBuilder strXml = new StringBuilder();
        try
        {
            if (coll.Count < 1)
            {
                return string.Empty;
            }

            strXml.Append("<Table1>");
            for (int idx = 0; idx < coll.Count; idx++)
            {
                strXml.Append("<c" + myStr(idx + 1) + ">");
                strXml.Append(parseXML(coll[idx]));
                strXml.Append("</c" + myStr(idx + 1) + ">");
            }
            strXml.Append("</Table1>");

            coll = new ArrayList();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return strXml.ToString();
    }

    // <![CDATA[ ]]>
    public static string parseXML(object strVal)
    {
        string retVal = string.Empty;
        try
        {
            retVal = myStr(strVal);

            if (retVal.Trim().Length > 0)
            {
                retVal = "<![CDATA[" + retVal + "]]>";
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return retVal;
    }

    public static string parseXMLManually(object strVal)
    {
        string retVal = string.Empty;

        try
        {
            retVal = myStr(strVal);

            if (retVal.Trim().Length > 0)
            {
                retVal = retVal.Replace("&", "&amp;");
                retVal = retVal.Replace("<", "&lt;");
                retVal = retVal.Replace(">", "&gt;");
                retVal = retVal.Replace("\"", "&quot;");
                retVal = retVal.Replace("'", "&apos;");
                //retVal = retVal.Replace("ˆ", "&circ;");
                //retVal = retVal.Replace("˜", "&tilde;");
                //retVal = retVal.Replace("–", "&ndash;");
                //retVal = retVal.Replace("—", "&mdash;");
                //retVal = retVal.Replace("‘", "&lsquo;");
                //retVal = retVal.Replace("’", "&rsquo;");
                //retVal = retVal.Replace("‚", "&sbquo;");
                //retVal = retVal.Replace("“", "&ldquo;");
                //retVal = retVal.Replace("”", "&rdquo;");
                //retVal = retVal.Replace("„", "&bdquo;");
                //retVal = retVal.Replace("{", "&lsaquo;");
                //retVal = retVal.Replace("}", "&rsaquo;");

                retVal = retVal.Trim();
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

        return retVal;
    }

    public static string parseQManually(object strVal)
    {
        string retVal = string.Empty;

        try
        {
            retVal = myStr(strVal);

            if (retVal.Trim().Length > 0)
            {
                retVal = retVal.Replace("&nbsp;", "");
                retVal = retVal.Replace("&apos;", "'");
                retVal = retVal.Replace("&#39;", "'");
                retVal = retVal.Replace("\r\n", " ");
                retVal = retVal.Replace(";", "");

                retVal = retVal.Trim();
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

        return retVal;
    }

    public static void setCulture(string datePattern)
    {
        try
        {
            if (datePattern.Equals(string.Empty))
            {
                datePattern = "MM/dd/yyyy";
            }
            CultureInfo cCulture = new CultureInfo(CultureInfo.CurrentCulture.IetfLanguageTag);
            cCulture.DateTimeFormat.ShortDatePattern = datePattern;
            Thread.CurrentThread.CurrentCulture = cCulture;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public static DataTable GetLabelSetting()
    {
        DataTable dtLabel = new DataTable();
        dtLabel.Columns.Add("SerialNo");
        dtLabel.Columns.Add("cFont");
        dtLabel.Columns.Add("cFontSize");
        dtLabel.Columns.Add("cBold");
        dtLabel.Columns.Add("cEColor");
        dtLabel.Columns.Add("cAColor");
        dtLabel.Columns["SerialNo"].AutoIncrement = true;
        dtLabel.Columns["SerialNo"].AutoIncrementSeed = 1;
        dtLabel.Columns["SerialNo"].AutoIncrementStep = 1;
        dtLabel.Columns["cBold"].DataType = Type.GetType("System.Boolean");
        DataRow dtLabelR = dtLabel.NewRow();

        dtLabelR["cFont"] = string.Empty;
        dtLabelR["cFontSize"] = "12";
        dtLabelR["cBold"] = "True";
        dtLabelR["cEColor"] = "Red";
        dtLabelR["cAColor"] = "Green";

        dtLabel.Rows.Add(dtLabelR);

        return dtLabel;
    }

    public static string GetFlagValueHospitalSetup(int iHospitalLocation, int iFacilityId, string strFlag, string sConString)
    {
        string sValue = string.Empty;

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshIn = new Hashtable();
        hshIn.Add("@iHospitalLocationId", common.myInt(iHospitalLocation));
        hshIn.Add("@iFacilityId", common.myInt(iFacilityId));
        DataSet dsHs = new DataSet();
        dsHs = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);
        System.Web.HttpContext.Current.Cache.Insert("HospitalSetup", dsHs.Tables[0], null, DateTime.Now.AddYears(10), System.Web.Caching.Cache.NoSlidingExpiration);
        DataView dv = dsHs.Tables[0].DefaultView;
        dv.RowFilter = "Flag = '" + strFlag + "' AND facilityId = " + iFacilityId;
        if (dv.ToTable().Rows.Count > 0)
            sValue = common.myStr(dv.ToTable().Rows[0]["value"]);
        dv.RowFilter = string.Empty;
        //  }
        return sValue;
    }
    public static string GetFlagValue(DataSet ds, string strFlag, int iFacilityId)
    {
        string sValue = string.Empty;
        DataView dv = ds.Tables[0].DefaultView;
        dv.RowFilter = "Flag = '" + strFlag + "' AND facilityId = " + iFacilityId;
        if (dv.ToTable().Rows.Count > 0)
            sValue = common.myStr(dv.ToTable().Rows[0]["value"]);
        dv.RowFilter = string.Empty;
        //  }
        return sValue;

    }

    public static DataSet GetFlagValueHospitalSetup(int iHospitalLocation, int iFacilityId, string sConString)
    {


        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshIn = new Hashtable();
        hshIn.Add("@iHospitalLocationId", common.myInt(iHospitalLocation));
        hshIn.Add("@iFacilityId", common.myInt(iFacilityId));
        DataSet dsHs = new DataSet();
        dsHs = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);

        return dsHs;
    }


    public static string FormatNumber(string myValue, int iHospitalLocationID, string sConString, int FacilityId)
    {
        string NumbErValue;
        string sFormat = "{0:0.";
        int iNoDec = 0;

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsPoint = dl.FillDataSet(CommandType.Text, "SELECT Value FROM HospitalSetup WHERE Flag='DecimalPlaces' AND HospitalLocationId =" + iHospitalLocationID + "AND FacilityId =" + FacilityId);
        if (dsPoint.Tables[0].Rows.Count > 0)
        {
            iNoDec = Convert.ToInt32(dsPoint.Tables[0].Rows[0][0]);
        }

        for (int i = 0; i != iNoDec; i++)
        {
            sFormat = sFormat + "0";
        }
        sFormat = sFormat + "}";
        if (!myValue.Equals(string.Empty))
        {
            NumbErValue = string.Format(sFormat, Convert.ToDecimal(myValue));
            return NumbErValue;
        }
        else
        {
            return string.Empty;
        }
    }


    public static string[] datecalculate()
    {
        string DayName = DateTime.Now.DayOfWeek.ToString();
        string fromdate = string.Empty;
        string todate = string.Empty;

        string[] str = new string[2];

        switch (DayName)
        {
            case "Monday":
                fromdate = DateTime.Now.Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(6).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Tuesday":
                fromdate = DateTime.Now.AddDays(-1).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(5).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Wednesday":
                fromdate = DateTime.Now.AddDays(-2).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(4).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Thursday":
                fromdate = DateTime.Now.AddDays(-3).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(3).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Friday":
                fromdate = DateTime.Now.AddDays(-4).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(2).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Saturday":
                fromdate = DateTime.Now.AddDays(-5).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd") + " 23:59";
                break;
            case "Sunday":
                fromdate = DateTime.Now.AddDays(-6).Date.ToString("yyyy/MM/dd") + " 00:00";
                todate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59";
                break;
        }

        str[0] = fromdate;
        str[1] = todate;
        return str;
    }

    public DataSet GetTemplates(int iHospitalLocationId, string sCode, string sConString)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hshInput = new Hashtable();
        hshInput.Add("@chvTemplateCode", sCode);
        hshInput.Add("@inyHospitalLocationId", iHospitalLocationId);
        string strTemplate = "SELECT tm.id AS TemplateID,TemplateName FROM EMRTemplate TM INNER JOIN EMRTemplateTypes TT ON TM.TemplateTypeID=TT.ID " +
                                " WHERE HospitalLocationID=@inyHospitalLocationId AND tt.Code=@chvTemplateCode AND TM.Active=1 ORDER BY TemplateName";
        ds = (DataSet)dl.FillDataSet(CommandType.Text, strTemplate, hshInput);
        return ds;
    }

    // To get controls  if they mention in OracleNonEditableColumn
    public static DataSet NonEditableColumns(int iHospitalLocationId, int iFacilityId, string strFormName, string sConString)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hshInput = new Hashtable();
        hshInput.Add("@iHospitalLocationId", common.myInt(iHospitalLocationId));
        hshInput.Add("@iFacilityId", common.myInt(iFacilityId));
        hshInput.Add("@sFormName", strFormName);
        ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "uspGetOracleNonEditableColumns", hshInput);
        return ds;
    }
    public static void WriteNotificationFile(string xml, int FacilityId, string filePath)
    {

        System.IO.FileInfo file = new System.IO.FileInfo(filePath);


        if (!file.Exists)
        {
            File.Create(filePath).Dispose();
            using (TextWriter tw = new StreamWriter(filePath))
            {
                tw.WriteLine(xml);
                tw.Close();
            }

        }

        else if (File.Exists(filePath))
        {
            using (TextWriter tw = new StreamWriter(filePath))
            {
                tw.WriteLine(xml);
                tw.Close();
            }
        }
    }

    public static string ParseString(string str)
    {

        if (str == null) return null;
        if (str.Length > 0)
        {
            str = str.Replace("&nbsp;", "");
            str = str.Replace("&apos;", "'");
            str = str.Replace("&#39;", "'");
            //str = str.Replace("'", "''");
            str = str.Replace("\r\n", " ");
            str = str.Replace(";", "");

            str = str.Trim();
        }
        return str;
    }
    public static string ReadNotificationFile(string filePath)
    {
        System.IO.FileInfo file = new System.IO.FileInfo(filePath);

        string text = string.Empty;
        if (File.Exists(filePath))
        {
            text = System.IO.File.ReadAllText(filePath);
        }
        return text;
    }
    public DataSet GetReasonMaster(int iHospitalLocationId, int iFacilityId, string ReasonType, string sConString)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hshin = new Hashtable();
        hshin.Add("@inyHospitalLocationId", iHospitalLocationId);
        hshin.Add("@intFacilityId", iFacilityId);
        hshin.Add("@chvReasonType", ReasonType);

        string str = @"SELECT Id,Reason,ReasonType FROM [dbo].[ReasonMaster] WHERE HospitalLocationId=@inyHospitalLocationId AND FacilityId=@intFacilityId AND ReasonType=@chvReasonType AND ACTIVE=1";
        ds = objDl.FillDataSet(CommandType.Text, str, hshin);
        return ds;
    }

    public static DataTable GenerateTransposedTable(DataTable inputTable)
    {
        DataTable outputTable = new DataTable();

        // Add columns by looping rows

        // Header row's first column is same as in inputTable
        outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

        // Header row's second column onwards, 'inputTable's first column taken
        foreach (DataRow inRow in inputTable.Rows)
        {
            string newColName = inRow[0].ToString();
            outputTable.Columns.Add(newColName);
        }

        // Add rows by looping columns        
        for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
        {
            DataRow newRow = outputTable.NewRow();

            // First column is inputTable's Header row's second column
            newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
            for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
            {
                string colValue = inputTable.Rows[cCount][rCount].ToString();
                newRow[cCount + 1] = colValue;
            }
            outputTable.Rows.Add(newRow);
        }

        return outputTable;
    }


}

public class StringValueAttribute : Attribute
{
    public string StringValue;

    public StringValueAttribute(string value)
    {
        this.StringValue = value;
    }
}

public class commonLabelSetting
{
    public static string cFont = string.Empty;
    public static double cFontSize = 12;
    public static bool cBold = true;
    public static string cErrorColor = "Red";
    public static string cSucceedColor = "Green";
}



//public class clsCacheSetting
//{
//    public void ClearApplicationCache()
//    {
//        List<string> keys = new List<string>();

//        // retrieve application Cache enumerator
//        IDictionaryEnumerator enumerator = Cache.GetEnumerator();

//        // copy all keys that currently exist in Cache
//        while (enumerator.MoveNext())
//        {
//            keys.Add(enumerator.Key.ToString());
//        }

//        // delete every key from cache
//        for (int i = 0; i < keys.Count; i++)
//        {
//            Cache.Remove(keys[i]);
//        }
//    }
//}
