using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EMRAPI.Classes
{
    public class clsExceptionLog:IDisposable
    {
        string sConString;
        Hashtable HshIn, HshOut;
        DAL.DAL Dl;
        string strQueryString = "";
        public string QueryString
        {
            set { strQueryString = value; }
            get { return strQueryString; }
        }
        public clsExceptionLog(string sConString)
        {
            sConString = Util.GetConnectionString();
        }

        public void HandleException(Exception ex)
        {
            HttpContext ctxObject = HttpContext.Current;
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", common.myInt(SessionHelper.HospitalLocationId));
            HshIn.Add("chvSource", "APPLICATION");
            HshIn.Add("chvMessage", common.myStr(ex.Message));
            HshIn.Add("chvQueryString", common.myStr(strQueryString));
            HshIn.Add("chvTargetSite", common.myStr(ex.TargetSite));
            HshIn.Add("chvStackTrace", common.myStr(ex.StackTrace));
            HshIn.Add("chvServerName", common.myStr(ctxObject.Request.ServerVariables["SERVER_NAME"]));
            HshIn.Add("chvRequestURL", common.myStr(ctxObject.Request.Url));
            HshIn.Add("chvUserAgent", common.myStr(ctxObject.Request.UserAgent));
            HshIn.Add("chvUserIP", common.myStr(ctxObject.Request.UserHostAddress));
            HshIn.Add("intUserId", common.myInt(ctxObject.Session["UserId"]));
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspSaveExceptionLog", HshIn);
        }
        public void HandleExceptionWithMethod(Exception ex, String MethodName)
        {
            HttpContext ctxObject = HttpContext.Current;
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", common.myInt(SessionHelper.HospitalLocationId));
            HshIn.Add("chvSource", "APPLICATION");
            HshIn.Add("chvMessage", "(" + MethodName + ")" + common.myStr(ex.Message));
            HshIn.Add("chvQueryString", common.myStr(strQueryString));
            HshIn.Add("chvTargetSite", common.myStr(ex.TargetSite));
            HshIn.Add("chvStackTrace", common.myStr(ex.StackTrace));
            HshIn.Add("chvServerName", common.myStr(ctxObject.Request.ServerVariables["SERVER_NAME"]));
            HshIn.Add("chvRequestURL", common.myStr(ctxObject.Request.Url));
            HshIn.Add("chvUserAgent", common.myStr(ctxObject.Request.UserAgent));
            HshIn.Add("chvUserIP", common.myStr(ctxObject.Request.UserHostAddress));
            HshIn.Add("intUserId", common.myInt(ctxObject.Session["UserId"]));
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspSaveExceptionLog", HshIn);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~clsExceptionLog() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}