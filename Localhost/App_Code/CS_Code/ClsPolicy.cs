using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Summary description for ClsPolicy
/// </summary>
public class ClsPolicy:ICertificatePolicy
{
    public ClsPolicy()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bool CheckValidationResult (ServicePoint srvPoint,X509Certificate cert,WebRequest req,int certificateproblem)
    {
        return true;
    }
}