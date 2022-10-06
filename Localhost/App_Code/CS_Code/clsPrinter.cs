using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;

/// <summary>
/// Summary description for clsPrinter
/// </summary>
public class clsPrinter
{
    string sConString = "";

    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];

    private string _PageType;
    private double _PageHeight;
    private double _PageWidth;
    private double _MarginTop;
    private double _MarginBottom;
    private double _MarginLeft;
    private double _MarginRight;
    private string _PageMargin;
    private string _PrinterName;
    private string _PrinterDescription;
    private string _PrinterLocation;
    private string _TrayName;
    private string _ReportName;

    private string _reportServer;
    private string _reportFolder;
    private string _SysUser;
    private string _SysPassword;
    private string _SysDomain;

    DAL.DAL Dlobj;
    public clsPrinter()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public clsPrinter(string Constring)
    {
        sConString = Constring;                
    }  
    

    public clsPrinter(string sConString, string ReportName,string hostName)
    {
        DataSet ds = new DataSet();
        //clsPrinter _printer = new clsPrinter(sConString);
        try
        {
            //string hostName = System.Windows.Forms.SystemInformation.ComputerName;            
            //System.Environment.MachineName;
           
            Hashtable HshIn = new Hashtable();
            //HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            //HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@ReportName", ReportName);
            HshIn.Add("@HostName", hostName);

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "uspGetPrintSettingDetails", HshIn);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this.ReportName = common.myStr(dr["ReportName"]);
                    this.PageType = common.myStr(dr["PageType"]);
                    this.PageHeight = common.myDbl(dr["Height"]);
                    this.PageWidth = common.myDbl(dr["Width"]);
                    this.MarginTop = common.myDbl(dr["m_Top"]);
                    this.MarginBottom = common.myDbl(dr["m_Bottom"]);
                    this.MarginRight = common.myDbl(dr["m_Right"]);
                    this.MarginLeft = common.myDbl(dr["m_Left"]);
                    this.PageMargin = common.myStr(dr["PageMargin"]);
                    this.PrinterName = common.myStr(dr["PrinterName"]);
                    this.PrinterDescription = common.myStr(dr["PrinterDescription"]);
                    this.PrinterLocation = common.myStr(dr["PrinterLocation"]);
                    this.TrayName = common.myStr(dr["TrayName"]);
                    this.reportServer = common.myStr(reportServer);
                    this.reportFolder = common.myStr(reportFolder);
                    this.SysUser1 = common.myStr(SysUser);
                    this.SysPassword1 = common.myStr(SysPassword);
                    this.SysDomain1 = common.myStr(SysDomain);
                }                
            }
        }
        catch (Exception ex)
        {

        }       
    }
    public clsPrinter(string sConString, string ReportName,int StoreId, string hostName)
    {
        DataSet ds = new DataSet();
        //clsPrinter _printer = new clsPrinter(sConString);
        try
        {
            //string domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            //string hostName = Dns.GetHostName();
            //string hostName = System.Environment.MachineName;
            //string hostName1 = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
            //string hostName2 = System.Net.Dns.GetHostEntry("localhost").HostName;
            //string fqdn = "";
            //if (!hostName.Contains(domainName))
            //    fqdn = hostName + "." + domainName;
            //else
            //    fqdn = hostName;

            Hashtable HshIn = new Hashtable();
            //HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            //HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@ReportName", ReportName);
            HshIn.Add("@StoreId", StoreId);
            HshIn.Add("@HostName", hostName);

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "uspGetPrintSettingDetails", HshIn);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this.ReportName = common.myStr(dr["ReportName"]);
                    this.PageType = common.myStr(dr["PageType"]);
                    this.PageHeight = common.myDbl(dr["Height"]);
                    this.PageWidth = common.myDbl(dr["Width"]);
                    this.MarginTop = common.myDbl(dr["m_Top"]);
                    this.MarginBottom = common.myDbl(dr["m_Bottom"]);
                    this.MarginRight = common.myDbl(dr["m_Right"]);
                    this.MarginLeft = common.myDbl(dr["m_Left"]);
                    this.PageMargin = common.myStr(dr["PageMargin"]);
                    this.PrinterName = common.myStr(dr["PrinterName"]);
                    this.PrinterDescription = common.myStr(dr["PrinterDescription"]);
                    this.PrinterLocation = common.myStr(dr["PrinterLocation"]);
                    this.TrayName = common.myStr(dr["TrayName"]);

                    this.reportServer = common.myStr(reportServer);
                    this.reportFolder = common.myStr(reportFolder);
                    this.SysUser1 = common.myStr(SysUser);
                    this.SysPassword1 = common.myStr(SysPassword);
                    this.SysDomain1 = common.myStr(SysDomain);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public clsPrinter(string _PageType, double _PageHeight, double _PageWidth, double _MarginTop, double _MarginBottom, double _MarginLeft, double _MarginRight, string _PageMargin, string _PrinterName, string _PrinterDescription, string _PrinterLocation, string _TrayName, string _ReportName)
    {
        this.PageType = _PageType;
        this.PageHeight = _PageHeight;
        this.PageWidth = _PageWidth;
        this.MarginTop = _MarginTop;
        this.MarginBottom = _MarginBottom;
        this.MarginLeft = _MarginLeft;
        this.MarginRight = _MarginRight;
        this.PageMargin = _PageMargin;
        this.PrinterName = _PrinterName;
        this.PrinterDescription = _PrinterDescription;
        this.PrinterLocation = _PrinterLocation;
        this.TrayName = _TrayName;
        this.ReportName = _ReportName;
    }

    public DataSet SetPrinterSetting(string ReportName)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            //HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            //HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@ReportName", ReportName);   
                   
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = (DataSet)dl.FillDataSet(CommandType.StoredProcedure, "uspGetPrintSettingDetails", HshIn);            
        }
        catch (Exception ex)
        {

        }
        return ds;
    }   

    public string PageType
    {
        get
        {
            return _PageType;
        }

        set
        {
            _PageType = value;
        }
    }

    public double PageHeight
    {
        get
        {
            return _PageHeight;
        }

        set
        {
            _PageHeight = value;
        }
    }

    public double PageWidth
    {
        get
        {
            return _PageWidth;
        }

        set
        {
            _PageWidth = value;
        }
    }

    public double MarginTop
    {
        get
        {
            return _MarginTop;
        }

        set
        {
            _MarginTop = value;
        }
    }

    public double MarginBottom
    {
        get
        {
            return _MarginBottom;
        }

        set
        {
            _MarginBottom = value;
        }
    }

    public double MarginLeft
    {
        get
        {
            return _MarginLeft;
        }

        set
        {
            _MarginLeft = value;
        }
    }

    public double MarginRight
    {
        get
        {
            return _MarginRight;
        }

        set
        {
            _MarginRight = value;
        }
    }

    public string PrinterName
    {
        get
        {
            return _PrinterName;
        }

        set
        {
            _PrinterName = value;
        }
    }

    public string PrinterDescription
    {
        get
        {
            return _PrinterDescription;
        }

        set
        {
            _PrinterDescription = value;
        }
    }

    public string PrinterLocation
    {
        get
        {
            return _PrinterLocation;
        }

        set
        {
            _PrinterLocation = value;
        }
    }

    public string TrayName
    {
        get
        {
            return _TrayName;
        }

        set
        {
            _TrayName = value;
        }
    }

    public string PageMargin
    {
        get
        {
            return _PageMargin;
        }

        set
        {
            _PageMargin = value;
        }
    }

    public string ReportName
    {
        get
        {
            return _ReportName;
        }

        set
        {
            _ReportName = value;
        }
    }

    public string ReportServer
    {
        get
        {
            return _reportServer;
        }

        set
        {
            _reportServer = value;
        }
    }

    public string ReportFolder
    {
        get
        {
            return _reportFolder;
        }

        set
        {
            _reportFolder = value;
        }
    }

    public string SysUser1
    {
        get
        {
            return _SysUser;
        }

        set
        {
            _SysUser = value;
        }
    }

    public string SysPassword1
    {
        get
        {
            return _SysPassword;
        }

        set
        {
            _SysPassword = value;
        }
    }

    public string SysDomain1
    {
        get
        {
            return _SysDomain;
        }

        set
        {
            _SysDomain = value;
        }
    }
}

public class Printer
{
    private string sConString;

    public Printer(string Constring)
    {
        sConString = Constring;
    }
    
}