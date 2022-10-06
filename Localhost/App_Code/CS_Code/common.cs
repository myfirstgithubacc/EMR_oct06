using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Text;
using System.IO;
using System.Reflection;
using Telerik.Web.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Developed By Manmohan
/// </summary>

public class common
{
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
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

    public enum enumCIMSorVIDALInterfaceFor
    {
        None = 0,
        InterfaceForEMRDrugOrder = 1,
        InterfaceForWordDrugRequisition = 2,
        InterfaceForOPSale = 3,
        InterfaceForIPIssue = 4,
        InterfaceForDrugAdministered = 5,
        InterfaceForMedicationDispense = 6
    }

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
        FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
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
        catch { }
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
        catch { }
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
        catch { }
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
        catch { }
        return strVal;
    }
    public static string removeUnusedHTML(string val)
    {
        string strVal = val;
        try
        {
            if (strVal.ToUpper().Contains("<UL>") && strVal.ToUpper().Contains("<OL>"))
            {
                strVal = strVal.Replace("<ul>", "<p>");
                strVal = strVal.Replace("</ul>", "</p>");
                strVal = strVal.Replace("<ol>", "<p>");
                strVal = strVal.Replace("</ol>", "</p>");
                strVal = strVal.Replace("<li>", "<p>");
                strVal = strVal.Replace("</li>", "</p>");
            }
        }
        catch { }
        return strVal;
    }

    public static int myInt(object num)
    {
        string strNum = common.myStr(num).Trim();
        try
        {
            if (isNumeric(strNum))
            {
                return int.Parse(Convert.ToString((int)myDbl(strNum)));
            }
        }
        catch { }
        return 0;
    }

    public static long myLong(object num)
    {
        string strNum = common.myStr(num).Trim();
        try
        {
            if (isNumeric(strNum))
            {
                return long.Parse(Convert.ToString((long)myDbl(strNum)));
            }
        }
        catch { }
        return 0;
    }

    public static double myDbl(object num)
    {
        string strNum = common.myStr(num).Trim();
        try
        {
            if (isNumeric(strNum))
            {
                return double.Parse(strNum);
            }
        }
        catch { }
        return 0;
    }

    public static decimal myDec(object num)
    {
        string strNum = common.myStr(num).Trim();
        try
        {
            if (isNumeric(strNum))
            {
                return decimal.Parse(strNum);
            }
        }
        catch { }
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
        catch
        {
        }
        return retDate;
    }

    public static bool myBool(object val)
    {
        string strVal = myStr(val).Trim().ToUpper();
        try
        {
            return (strVal.Equals("1") || strVal.Equals("TRUE")) ? true : false;
        }
        catch { }
        return false;
    }

    public static string myNumericCustomFormat(object num, int intDecimalPlaces)
    {
        double numberToSplit = 0;
        double decimalresult = 0;
        string strResult = "0";

        try
        {
            numberToSplit = common.myDbl(num);
            decimalresult = (int)numberToSplit - numberToSplit;

            if (intDecimalPlaces.Equals(0))
            {
                intDecimalPlaces = 2;
            }

            if (decimalresult.Equals(0))
            {
                strResult = ((int)numberToSplit).ToString();
            }
            else
            {
                strResult = numberToSplit.ToString("F" + intDecimalPlaces.ToString());
            }
        }
        catch
        {
        }
        return strResult;
    }

    public static bool isNumeric(object num)
    {
        try
        {
            double retNum;
            return Double.TryParse(myStr(num), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }
        catch { }
        return false;
    }

    public static string uniqueSessionId()
    {
        string retVal = string.Empty;
        try
        {
            retVal = HttpContext.Current.Session.SessionID + myStr(HttpContext.Current.Session["UserId"]);
        }
        catch { }
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
        catch { }
        return retDate;
    }

    public static DataTable setSelectInCombo(DataTable dtbl, string strID, string strVal)
    {
        DataTable dt = dtbl;
        try
        {
            if (myLen(strID) > 0 && myLen(strVal) > 0)
            {
                DataRow dr = dt.NewRow();
                dr[strID] = 0;
                dr[strVal] = "[ Select ]";

                dt.Rows.InsertAt(dr, 0);
                return dt;
            }
        }
        catch { }
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
        catch { }
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
        catch { }
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
        catch { }
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
        catch { }
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
        catch { }
        return retVal;
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
        catch { }

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
                retVal = retVal.Replace("&nbsp;", string.Empty);
                retVal = retVal.Replace("&apos;", "'");
                retVal = retVal.Replace("&#39;", "'");
                retVal = retVal.Replace("\r\n", " ");
                retVal = retVal.Replace(";", string.Empty);

                retVal = retVal.Trim();
            }
        }
        catch { }

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
        catch { }
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

    public static string FormatNumber(string myValue, int iHospitalLocationID, string sConString, int FacilityId)
    {
        string NumbErValue;
        string sFormat = "{0:0.";
        int iNoDec = 0;

        Hashtable hsh = new Hashtable();
        hsh.Add("@intHospId", iHospitalLocationID);
        hsh.Add("@intFacilityId", FacilityId);

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsPoint = dl.FillDataSet(CommandType.Text, "SELECT Value FROM HospitalSetup WITH (NOLOCK) WHERE Flag='DecimalPlaces' AND FacilityId =@intFacilityId AND HospitalLocationId =@intHospId ", hsh);
        if (dsPoint.Tables[0].Rows.Count > 0)
        {
            iNoDec = myInt(dsPoint.Tables[0].Rows[0][0]);
        }

        for (int i = 0; i < iNoDec; i++)
        {
            sFormat = sFormat + "0";
        }
        sFormat = sFormat + "}";
        if (!myValue.Equals(string.Empty))
        {
            NumbErValue = string.Format(sFormat, myDec(myValue));
            return NumbErValue;
        }
        else
        {
            return string.Empty;
        }
    }


    public static string Getprovidingservice(int EmployeeID,int FacilityId, string sConString)
    {
        string providingservice = string.Empty;
        Hashtable hsh = new Hashtable();
        try
        {

            hsh.Add("@EmployeeID", EmployeeID);
            hsh.Add("@intFacilityId", FacilityId);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            providingservice = (string)dl.ExecuteScalar(CommandType.Text, "Select providingservice from Employee WHERE ID =@EmployeeID AND FacilityId =@intFacilityId  ", hsh);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return providingservice;

    }

    public static string GetCheckedItems(RadComboBox comboBox)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string strSB = string.Empty;
        var collection = comboBox.CheckedItems;

        foreach (var item in collection)
            sb.Append(item.Value + ",");

        if (sb.Length > 0)
        {
            strSB = sb.ToString().Remove(sb.ToString().Length - 1, 1);
        }
        return strSB;
    }
    public static void CheckAllItems(RadComboBox comboBox)
    {
        foreach (RadComboBoxItem item in comboBox.Items)
        {
            item.Checked = true;
        }
    }

    public static void UnCheckAllCheckedItems(RadComboBox comboBox)
    {
        var collection = comboBox.CheckedItems;

        foreach (var item in collection)
        {
            if (item.Checked)
            {
                item.Checked = false;
            }
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

        string strTemplate = "SELECT tm.id AS TemplateID,TemplateName FROM EMRTemplate TM WITH (NOLOCK) INNER JOIN EMRTemplateTypes TT WITH (NOLOCK) ON TM.TemplateTypeID=TT.ID " +
                             " WHERE tt.Code=@chvTemplateCode AND TM.Active=1 AND HospitalLocationID=@inyHospitalLocationId ORDER BY TemplateName";

        ds = dl.FillDataSet(CommandType.Text, strTemplate, hshInput);
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
        ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetOracleNonEditableColumns", hshInput);
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

        string str = @"SELECT Id,Reason,ReasonType FROM [dbo].[ReasonMaster] WITH (NOLOCK) WHERE ReasonType=@chvReasonType AND ACTIVE=1 AND FacilityId=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ";
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

    public static string PatientInfo(int encounterID, System.Web.UI.WebControls.Xml xmlPatientInfo, int HospitalLocationID, string OPIP)
    {
        //try
        //{
        string error = string.Empty;
        if (encounterID > 0)
        {
            DataSet ds = new DataSet();
            string sXSL = string.Empty;
            String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
            BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
            ds = master.GetPatientRecordInfoXML(encounterID, 1, HospitalLocationID);
            if (OPIP.Equals("O"))
                sXSL = "/include/xsl/PatientInfo-US.xsl";
            else
                sXSL = "/include/xsl/PatientInfo-US_IP.xsl";

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!myStr(ds.Tables[0].Rows[0][0]).Equals("Patient Is Expired, No Transaction Allowed!"))
                    {
                        xmlPatientInfo.DocumentContent = ds.Tables[0].Rows[0][0].ToString();
                        xmlPatientInfo.TransformSource = sXSL;
                        xmlPatientInfo.DataBind();
                    }
                    else
                    {
                        error = ds.Tables[0].Rows[0][0].ToString();
                        //lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        //lblmsg.Text = ds.Tables[0].Rows[0][0].ToString();
                        xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                        xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                        xmlPatientInfo.DataBind();
                    }
                }
                else
                {
                    error = "No patient record found ";
                    //Alert.ShowAjaxMsg("No patient record found ", Page);
                    xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                    xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                    xmlPatientInfo.DataBind();
                }
            }
            else
            {
                error = "No patient record found ";
                //Alert.ShowAjaxMsg("No patient record found ", Page);
                xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                xmlPatientInfo.DataBind();
            }
        }
        else
        {
            xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
            xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
            xmlPatientInfo.DataBind();
        }
        return error;
        //}
        //catch (Exception Ex)
        //{
        //    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblmsg.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}
    }

    public static string clearHTMLTags(object strVal)
    {
        string result = string.Empty;

        try
        {
            result = myStr(strVal);

            if (result.Trim().Length > 0)
            {
                result = result.Replace("<br/>\n", Environment.NewLine);
                result = result.Replace("<br />\n", Environment.NewLine);
                result = result.Replace("<br/>", Environment.NewLine);
                result = result.Replace("<br />", Environment.NewLine);

                result = System.Web.HttpUtility.HtmlDecode(result);

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\r", string.Empty);
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", Environment.NewLine);
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*head([^>])*>", "<head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(<head>).*(</head>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*script([^>])*>", "<script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*style([^>])*>", "<style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(<style>).*(</style>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*td([^>])*>", "\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*br( )*>", "\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*li( )*>", "\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                //result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]*>", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result, @" ", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result, @"&bull;", " * ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&lsaquo;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&rsaquo;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&trade;", "(tm)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&frasl;", "/", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&lt;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&gt;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&copy;", "(c)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&reg;", "(r)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = System.Text.RegularExpressions.Regex.Replace(result, @"&(.{2,6});", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                //result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)( )+(\r)", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\t)( )+(\t)", "\t\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\t)( )+(\r)", "\t\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)( )+(\t)", "\r\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)(\t)+", "\r\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }
            }
        }
        catch { }

        return result.Trim();
    }

    public static bool ContainsUnicodeCharacter(string input)
    {
        bool IsUnicode = false;
        try
        {
            byte[] abCharacters = System.Text.Encoding.ASCII.GetBytes(common.myStr(input));
            string sConverted = System.Text.Encoding.ASCII.GetString(abCharacters);

            if (input != sConverted)
            {
                IsUnicode = true;
            }
        }
        catch
        {
        }
        return IsUnicode;
    }

    public static string getConnection()
    {
        return common.myStr(ConfigurationManager.ConnectionStrings["akl"].ConnectionString);
    }

    public static string getConnectionV3()
    {

        return common.myStr(ConfigurationManager.AppSettings["WebAPIAddress"].ToString());
    }

    public static string GetPatientDocumentId(System.Web.HttpContext context)
    {
        try
        {
            return common.myStr(common.myStr(context.Session["FacilityId"])
                            + "|" + common.myStr(context.Session["RegistrationId"])
                            + "|" + common.myStr(context.Session["EncounterId"])
                            + "|" + common.myStr(context.Session["UserId"]));
            //+ "DPN.txt";
        }
        catch
        {
            return "|||";
        }


    }
    public static string Base64Encode(string plainText)
    {
        try
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        catch
        {
            return "";
        }

    }
    public static string Base64Decode(string base64EncodedData)
    {
        try
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        catch
        {
            return "";
        }
    }
    public static JArray GetJArray(string JsonData)
    {
        try
        {
            return JArray.Parse("[" + JsonData + "]");
        }
        catch
        {
            return new JArray();
        }

    }
    public static string GetKeyValue(string Key)
    {
        try
        {
            return ConfigurationManager.AppSettings[Key].ToString();
        }
        catch
        {
            return string.Empty;
        }

    }

    public static string GetFlagValueHospitalSetupDesc(int iHospitalLocation, int iFacilityId, string strFlag, string sConString)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshIn = new Hashtable();
        DataSet dsHs = new DataSet();
        DataView DV = new DataView();
        string sValue = string.Empty;
        try
        {
            hshIn.Add("@iHospitalLocationId", common.myInt(iHospitalLocation));
            hshIn.Add("@iFacilityId", common.myInt(iFacilityId));

            dsHs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);
            System.Web.HttpContext.Current.Cache.Insert("HospitalSetup", dsHs.Tables[0], null, DateTime.Now.AddYears(10), System.Web.Caching.Cache.NoSlidingExpiration);
            DV = dsHs.Tables[0].DefaultView;
            DV.RowFilter = "Flag='" + strFlag + "' AND FacilityId=" + iFacilityId;

            if (DV.ToTable().Rows.Count > 0)
            {
                sValue = myStr(DV.ToTable().Rows[0]["Description"]);
            }
            DV.RowFilter = string.Empty;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            dl = null;
            hshIn = null;
            dsHs.Dispose();
            DV.Dispose();
        }
        return sValue;
    }

    public static string EscapeLikeValue(string valueWithoutWildcards)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < valueWithoutWildcards.Length; i++)
        {
            char c = valueWithoutWildcards[i];
            if (c == '*' || c == '%' || c == '[' || c == ']')
                sb.Append("[").Append(c).Append("]");
            else if (c == '\'')
                sb.Append("''");
            else
                sb.Append(c);
        }
        return sb.ToString();
    }

    public static string GetFlagValueHospitalSetup(int HospitalLocationId, int FacilityId, string sFlags, string sConString)
    {
        string sValue = string.Empty;

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshIn = new Hashtable();
        DataSet ds = new DataSet();
        try
        {
            hshIn.Add("@iHospitalLocationId", HospitalLocationId);
            hshIn.Add("@iFacilityId", FacilityId);
            hshIn.Add("@chvFlags", sFlags);

            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                sValue = common.myStr(ds.Tables[0].Rows[0]["Value"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dl = null;
            hshIn = null;
            ds.Dispose();
        }

        return sValue;
    }


    public static string GetFlagValueHospitalSetupV3(int iHospitalLocation, int iFacilityId, string strFlag, string sConString)
    {
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hshIn = new Hashtable();
        DataSet dsHs = new DataSet();
        DataView DV = new DataView();
        APIRootClass.getHospitalSetupValueMultiple objRoot = new global::APIRootClass.getHospitalSetupValueMultiple();
        objRoot.HospitalLocationId = iHospitalLocation;
        objRoot.FacilityId = iFacilityId;
        objRoot.Flag = "'" + strFlag + "'";

        string ServiceURL = WebAPIAddress.ToString() + "api/Common/getHospitalSetupValueMultiple";
        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        dsHs = JsonConvert.DeserializeObject<DataSet>(sValue);
        DV = dsHs.Tables[0].DefaultView;
        // DV.RowFilter = "Flag='" + strFlag + "' AND FacilityId=" + iFacilityId;

        if (DV.ToTable().Rows.Count > 0)
        {
            sValue = myStr(DV.ToTable().Rows[0]["value"]);
        }
        DV.RowFilter = string.Empty;

        //try
        //{
        //    hshIn.Add("@iHospitalLocationId", common.myInt(iHospitalLocation));
        //    hshIn.Add("@iFacilityId", common.myInt(iFacilityId));

        //    dsHs = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);

        //    DV = dsHs.Tables[0].DefaultView;
        //    DV.RowFilter = "Flag='" + strFlag + "' AND FacilityId=" + iFacilityId;

        //    if (DV.ToTable().Rows.Count > 0)
        //    {
        //        sValue = myStr(DV.ToTable().Rows[0]["value"]);
        //    }
        //    DV.RowFilter = string.Empty;
        //}
        //catch (Exception Ex)
        //{
        //    throw Ex;
        //}
        //finally
        //{
        //    dl = null;
        //    hshIn = null;
        //    dsHs.Dispose();
        //    DV.Dispose();
        //}
        return sValue;
    }

    public static string setPatientDetails(int RegId, int RegNo, string EncNo, string sConString)
    {
        StringBuilder sbPatientDetils = new StringBuilder();
        //BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getEMRPatientDetails";
            APIRootClass.getEMRPatientDetails objRoot = new global::APIRootClass.getEMRPatientDetails();

            objRoot.HospitalLocationId = common.myInt(HttpContext.Current.Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(HttpContext.Current.Session["FacilityId"]);
            objRoot.RegistrationNo = common.myStr(RegNo);
            objRoot.EncounterNo = EncNo;
            objRoot.UserId = common.myInt(HttpContext.Current.Session["UserId"]);
            objRoot.RegistrationId = RegId;

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            //ds = bC.getEMRPatientDetails(common.myInt(HttpContext.Current.Session["HospitalLocationId"]), common.myInt(HttpContext.Current.Session["FacilityId"]),
            //          common.myInt(RegNo).ToString(), common.myStr(EncNo).Trim(), common.myInt(HttpContext.Current.Session["UserId"]), common.myInt(RegId));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow DR = ds.Tables[0].Rows[0];

                string sRegNoTitle = Resources.PRegistration.regno;
                string sDoctorTitle = Resources.PRegistration.Doctor;
                string DateTitle = common.myStr(DR["OPIP"]).StartsWith("I") ? "Admission Date :" : "Encounter Date :";

                sbPatientDetils.Append("<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["PatientName"]).Trim() + ", " + common.myStr(DR["GenderAge"]) + "</span>");
                sbPatientDetils.Append("&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["RegistrationNo"]) + "</span>");
                sbPatientDetils.Append("&nbsp;Enc #:<span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["EncounterNo"]) + "</span>");
                sbPatientDetils.Append("&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["DoctorName"]) + "</span>");
                sbPatientDetils.Append("&nbsp;" + DateTitle + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["EncounterDate"]) + "</span>");
                //sbPatientDetils.Append("&nbsp;Bed:<span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["BedNo"]) + "</span>");
                //sbPatientDetils.Append("&nbsp;Ward:<span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["WardName"]) + "</span>");
                sbPatientDetils.Append("&nbsp;Mobile:<span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["MobileNo"]) + "</span>");
                sbPatientDetils.Append("&nbsp;Company:<span style='color: #990066;font-weight: bold;'>" + common.myStr(DR["Payername"]) + "</span></b>");
            }
        }
        catch
        {
        }
        finally
        {
            // bC = null;
            ds.Dispose();
        }
        return sbPatientDetils.ToString();
    }

    public static System.Collections.Generic.Dictionary<string, string> GetFlagValueHospitalSetupMultiple(int HospitalLocationId, int FacilityId, string sFlags, string sConString)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshIn = new Hashtable();
        DataSet ds = new DataSet();
        System.Collections.Generic.Dictionary<string, string> coll = new System.Collections.Generic.Dictionary<string, string>();
        try
        {
            hshIn.Add("@iHospitalLocationId", HospitalLocationId);
            hshIn.Add("@iFacilityId", FacilityId);
            hshIn.Add("@chvFlags", sFlags);

            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);

            if (ds.Tables.Count > 0)
            {
                for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                {
                    coll.Add(common.myStr(ds.Tables[0].Rows[idx]["Flag"]), common.myStr(ds.Tables[0].Rows[idx]["Value"]));
                }
            }
        }
        catch
        { }
        finally
        {
            dl = null;
            hshIn = null;
            ds.Dispose();
        }
        return coll;
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
