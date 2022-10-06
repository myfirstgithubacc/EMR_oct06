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

/// <summary>
/// Summary description for UserAuthorisations
/// </summary>
public class UserAuthorisations
{
    private string _ConString = string.Empty;

    public UserAuthorisations()
    {
    }

    public UserAuthorisations(string Constring)
    {
        _ConString = Constring;
    }

    public void disableControl(TextBox txt, Button myBtn)
    {
    }

    public bool CheckPermissions(string action, string pagePath)
    {
        string sPath = pagePath;
        string[] strarry = sPath.Split('/');
        int lengh = strarry.Length;
        string pageName = strarry[lengh - 1];

        //It will remove start
        //DataTable dt1 = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
        //dt1.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pageName + "%'");
        //It will remove end
        int IsAuthorized = 0;
        if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) != "True")
        {
            if (System.Web.HttpContext.Current.Session["PrintAuthentication"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
                dt.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pageName + "%'");
                if (dt.DefaultView.Count > 0)
                {
                    switch (action)
                    {
                        case "N":
                            if (common.myStr(dt.DefaultView[0]["InsertData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "E":
                            if (common.myStr(dt.DefaultView[0]["EditData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "P":
                            if (common.myStr(dt.DefaultView[0]["PrintData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "C":
                            if (common.myStr(dt.DefaultView[0]["CancelData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                    }
                }

            }
        }
        else if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) == "True")
        {
            IsAuthorized = 1;
        }

        if (IsAuthorized == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckPermissions(string action, string pagePath, bool splitPath)
    {
        //string sPath = pagePath;
        //string[] strarry = sPath.Split('/');
        //int lengh = strarry.Length;
        //string pageName = strarry[lengh - 1];

        //It will remove start
        //DataTable dt1 = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
        //dt1.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pageName + "%'");
        //It will remove end
        int IsAuthorized = 0;
        if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) != "True")
        {
            if (System.Web.HttpContext.Current.Session["PrintAuthentication"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
                dt.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pagePath + "%'");
                if (dt.DefaultView.Count > 0)
                {
                    switch (action)
                    {
                        case "N":
                            if (common.myStr(dt.DefaultView[0]["InsertData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "E":
                            if (common.myStr(dt.DefaultView[0]["EditData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "P":
                            if (common.myStr(dt.DefaultView[0]["PrintData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "C":
                            if (common.myStr(dt.DefaultView[0]["CancelData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                    }
                }
            }
        }
        else if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) == "True")
        {
            IsAuthorized = 1;
        }

        if (IsAuthorized == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisableEnableControl(Button myBtn, bool action)
    {
        myBtn.Visible = action;
    }

    public void DisableEnableControl(ImageButton myBtn, bool action)
    {
        myBtn.Visible = action;
    }

    public bool CheckPermission(string action, string pagePath, ref string message)
    {
        string sPath = pagePath;
        string[] strarry = sPath.Split('/');
        int lengh = strarry.Length;
        string pageName = strarry[lengh - 1];

        message = "You are not authorised.";
        //It will remove start
        //DataTable dt1 = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
        //dt1.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pageName + "%'");
        //It will remove end
        int IsAuthorized = 0;
        if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) != "True")
        {
            if (System.Web.HttpContext.Current.Session["PrintAuthentication"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
                dt.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pageName + "%'");
                if (dt.DefaultView.Count > 0)
                {
                    switch (action)
                    {
                        case "N":
                            if (common.myStr(dt.DefaultView[0]["InsertData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "E":
                            if (common.myStr(dt.DefaultView[0]["EditData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "P":
                            if (common.myStr(dt.DefaultView[0]["PrintData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                        case "C":
                            if (common.myStr(dt.DefaultView[0]["CancelData"]) == "True")
                            {
                                IsAuthorized = 1;
                            }
                            break;
                    }
                }
            }
        }
        else if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) == "True")
        {
            IsAuthorized = 1;
        }

        if (IsAuthorized == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string[] CheckPermission1(string pagePath)
    {
        string sPath = pagePath;
        string[] strarry = sPath.Split('/');
        int lengh = strarry.Length;
        string pageName = strarry[lengh - 1];

        string[] auth = new string[5];
        auth[0] = "False";
        auth[1] = "False";
        auth[2] = "False";
        auth[3] = "False";
        auth[4] = "False";

        //message = "You are not authorised.";
        //It will remove start
        //DataTable dt1 = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
        //dt1.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pageName + "%'");
        //It will remove end
        //int IsAuthorized = 0;
        if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) != "True")
        {
            if (System.Web.HttpContext.Current.Session["PrintAuthentication"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
                dt.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pageName + "%'");
                if (dt.DefaultView.Count > 0)
                {

                    auth[0] = "True";

                    if (common.myStr(dt.DefaultView[0]["InsertData"]) == "True")
                    {
                        auth[1] = "True";
                    }
                    if (common.myStr(dt.DefaultView[0]["EditData"]) == "True")
                    {
                        auth[2] = "True";
                    }
                    if (common.myStr(dt.DefaultView[0]["PrintData"]) == "True")
                    {
                        auth[3] = "True";
                    }
                    if (common.myStr(dt.DefaultView[0]["CancelData"]) == "True")
                    {
                        auth[4] = "True";
                    }
                }
            }
        }
        else if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) == "True")
        {
            auth[0] = "True";
            auth[1] = "True";
            auth[2] = "True";
            auth[3] = "True";
            auth[4] = "True";
        }
        return auth;
    }
    //For ATD and Appointement show the context menu item if the page is not added with the logged in user
    public bool CheckPermission(string pagePath)
    {
        //string sPath = pagePath;
        //string[] strarry = sPath.Split('/');
        //int lengh = strarry.Length;
        //string pageName = strarry[lengh - 1];

        //It will remove start
        //DataTable dt1 = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
        //dt1.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pageName + "%'");
        //It will remove end
        int IsAuthorized = 0;
        if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) != "True")
        {
            if (System.Web.HttpContext.Current.Session["PrintAuthentication"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
                dt.DefaultView.RowFilter = string.Concat("PageUrl LIKE '%" + pagePath + "%'");
                if (dt.DefaultView.Count > 0)
                {
                    IsAuthorized = 1;
                }
            }
        }
        else if (common.myStr(System.Web.HttpContext.Current.Session["IsAdminGroup"]) == "True")
        {
            IsAuthorized = 1;
        }

        if (IsAuthorized == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckPermissionsForEMRModule(string action, string pagePath, int EncounterId)
    {
        return CheckPermissionsForEMRModule(action, pagePath, EncounterId, 0);
    }

    public bool CheckPermissionsForEMRModule(string action, string pagePath, int EncounterId, int PageId)
    {
        string sPath = pagePath;
        string[] strarry = sPath.Split('/');
        int lengh = strarry.Length;
        string pageName = strarry[lengh - 1];

        bool IsAuthorized = false;

        Hashtable hsh = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataView DV = new DataView();
        try
        {
            if (common.myBool(System.Web.HttpContext.Current.Session["isEMRSuperUser"])) //IsAdminGroup
            {
                IsAuthorized = true;
            }
            else
            {
                //bool IsFound = false;

                //hsh.Add("@intEncounterId", EncounterId);
                //hsh.Add("@intLoginEmpId", common.myInt(System.Web.HttpContext.Current.Session["EmployeeId"]));

                //string strQry = "SELECT DoctorId FROM ( " +
                //                " SELECT (CASE WHEN enc.OPIP = 'I' THEN adm.ConsultingDoctorId ELSE enc.DoctorId END) AS DoctorId " +
                //                " FROM Encounter enc WITH (NOLOCK) " +
                //                " LEFT OUTER JOIN Admission adm WITH (NOLOCK) ON enc.Id = adm.EncounterId " +
                //                " WHERE enc.Id = @intEncounterId " +
                //                " AND enc.Active = 1 " +
                //                " )xx " +
                //                " WHERE xx.DoctorId = @intLoginEmpId ";

                //ds = objDl.FillDataSet(System.Data.CommandType.Text, strQry, hsh);


                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    if (common.myInt(ds.Tables[0].Rows[0]["DoctorId"]) > 0)
                //    {
                //        IsFound = true;
                //    }
                //}

                //if (IsFound)
                //{
                //    IsAuthorized = true;
                //}
                //else
                //{
                if (System.Web.HttpContext.Current.Session["PrintAuthentication"] != null)
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["PrintAuthentication"];
                    DV = new DataView();
                    DV = dt.Copy().DefaultView;
                    if (PageId > 0)
                    {
                        DV.RowFilter = "ModuleId=" + common.myInt(System.Web.HttpContext.Current.Session["ModuleId"]) + " AND PageId=" + PageId;
                    }
                    else
                    {
                        DV.RowFilter = "ModuleId=" + common.myInt(System.Web.HttpContext.Current.Session["ModuleId"]) + " AND " + string.Concat("PageUrl LIKE '%" + pageName + "%'");
                    }

                    if (DV.Count > 0)
                    {
                        switch (action)
                        {
                            case "N":
                                if (common.myBool(DV[0]["InsertData"]))
                                {
                                    IsAuthorized = true;
                                }
                                break;
                            case "E":
                                if (common.myBool(DV[0]["EditData"]))
                                {
                                    IsAuthorized = true;
                                }
                                break;
                            case "P":
                                if (common.myBool(DV[0]["PrintData"]))
                                {
                                    IsAuthorized = true;
                                }
                                break;
                            case "C":
                                if (common.myBool(DV[0]["CancelData"]))
                                {
                                    IsAuthorized = true;
                                }
                                break;
                        }
                    }
                }
                //}
            }
        }
        catch
        {
        }
        finally
        {
            hsh = null;
            objDl = null;
            ds.Dispose();
            dt.Dispose();
            DV.Dispose();
        }
        return IsAuthorized;
    }

    private bool disposed = false;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
            }
            disposed = true;
        }
    }
    ~UserAuthorisations()
    {
        Dispose(false);
    }
}

