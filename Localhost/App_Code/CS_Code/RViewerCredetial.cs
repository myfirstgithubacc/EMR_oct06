using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;
using System.Runtime.InteropServices;


namespace RveiwerCredentail
{

    public class ReportViewerCredentials : IReportServerCredentials
    {

        [DllImport("advapi32.dll", SetLastError = true)]

        public extern static bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]

        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,

        int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        public ReportViewerCredentials()
        {

        }

        public ReportViewerCredentials(string username)
        {

            this.Username = username;

        }



        public ReportViewerCredentials(string username, string password)
        {

            this.Username = username;

            this.Password = password;

        }



        public ReportViewerCredentials(string username, string password, string domain)
        {

            this.Username = username;

            this.Password = password;

            this.Domain = domain;

        }



        public string Username
        {

            get
            {

                return this.username;

            }

            set
            {

                string username = value;

                if (username.Contains("\\"))
                {

                    this.domain = username.Substring(0, username.IndexOf("\\"));

                    this.username = username.Substring(username.IndexOf("\\") + 1);

                }

                else
                {

                    this.username = username;

                }

            }

        }

        private string username;





        public string Password
        {

            get
            {

                return this.password;

            }

            set
            {

                this.password = value;

            }

        }

        private string password;



        public string Domain
        {

            get
            {

                return this.domain;

            }

            set
            {

                this.domain = value;

            }

        }

        private string domain;







        #region IReportServerCredentials Members

        public bool GetBasicCredentials(out string basicUser, out string basicPassword, out string basicDomain)
        {

            basicUser = username;

            basicPassword = password;

            basicDomain = domain;

            return username != null && password != null && domain != null;

        }

        public bool GetFormsCredentials(out string formsUser, out string formsPassword, out string formsAuthority)
        {

            formsUser = username;

            formsPassword = password;

            formsAuthority = domain;

            return username != null && password != null && domain != null;

        }

        public bool GetFormsCredentials(out Cookie authCookie,

        out string user, out string password, out string authority)
        {

            authCookie = null;

            user = password = authority = null;

            return false; // Not implemented

        }



        public WindowsIdentity ImpersonationUser
        {

            get
            {

                string[] args = new string[3] { this.Domain.ToString(), this.Username.ToString(), this.Password.ToString() };

                IntPtr tokenHandle = new IntPtr(0);

                IntPtr dupeTokenHandle = new IntPtr(0);

                const int LOGON32_PROVIDER_DEFAULT = 0;

                //This parameter causes LogonUser to create a primary token.

                const int LOGON32_LOGON_INTERACTIVE = 2;

                const int SecurityImpersonation = 2;

                tokenHandle = IntPtr.Zero;

                dupeTokenHandle = IntPtr.Zero;

                try
                {

                    // Call LogonUser to obtain an handle to an access token.

                    bool returnValue = LogonUser(args[1], args[0], args[2],

                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,

                    ref tokenHandle);

                    if (false == returnValue)
                    {

                        Console.WriteLine("LogonUser failed with error code : {0}",

                        Marshal.GetLastWin32Error());

                        return null;

                    }

                    // Check the identity.

                    System.Diagnostics.Trace.WriteLine("Before impersonation: "

                    + WindowsIdentity.GetCurrent().Name);



                    bool retVal = DuplicateToken(tokenHandle, SecurityImpersonation, ref dupeTokenHandle);

                    if (false == retVal)
                    {

                        CloseHandle(tokenHandle);

                        Console.WriteLine("Exception in token duplication.");

                        return null;

                    }



                    // The token that is passed to the following constructor must 

                    // be a primary token to impersonate.

                    WindowsIdentity newId = new WindowsIdentity(dupeTokenHandle);

                    WindowsImpersonationContext impersonatedUser = newId.Impersonate();



                    // Free the tokens.

                    if (tokenHandle != IntPtr.Zero)

                        CloseHandle(tokenHandle);

                    if (dupeTokenHandle != IntPtr.Zero)

                        CloseHandle(dupeTokenHandle);

                    // Check the identity.

                    System.Diagnostics.Trace.WriteLine("After impersonation: "

                    + WindowsIdentity.GetCurrent().Name);

                    return newId;

                }

                catch (Exception ex)
                {

                    Console.WriteLine("Exception occurred. " + ex.Message);

                }

                return null;

            }

        }

        public ICredentials NetworkCredentials
        {

            get
            {

                return null; // Not using NetworkCredentials to authenticate.

            }

        }



        #endregion

    }

}

