
using System.Web;

namespace EMRAPI
{
    public sealed class SessionHelper
    {
        #region Private Constants
        //private static string userId;
        //private static string projectId;
        //private static string _RegistrationId { get; set; }
        //private static string _RegistrationNo { get; set; }
        //private static string _HospitalLocationId { get; set; }
        //private static string _FacilityId { get; set; }
        //private static string _EncounterId { get; set; }
        //private static string _EncounterDate { get; set; }
        //private static string _UserId { get; set; }
        #endregion

        #region Private Static Member Variables
        private static HttpContext thisContext;
        #endregion
        #region Public Static Methods
        /// <summary>
        /// Clears Session
        /// </summary>
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }
        /// <summary>
        /// Abandons Session
        /// </summary>
        public static void Abandon()
        {
            ClearSession();
            HttpContext.Current.Session.Abandon();
        }
        #endregion
        #region Public Static Properties


        /// <summary>
        /// Gets/Sets Session for RegistrationId
        /// </summary>
        public static string RegistrationId
        {
            get
            {
                if (HttpContext.Current.Session["_RegistrationId"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_RegistrationId"].ToString();
            }
            set { HttpContext.Current.Session["_RegistrationId"] = value; }
        }

        /// <summary>
        /// Gets/Sets Session for RegistrationNo
        /// </summary>
        public static string RegistrationNo
        {
            get
            {
                if (HttpContext.Current.Session["_RegistrationNo"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_RegistrationNo"].ToString();
            }
            set { HttpContext.Current.Session["_RegistrationNo"] = value; }
        }

        /// <summary>
        /// Gets/Sets Session for HospitalLocationId
        /// </summary>
        public static string HospitalLocationId
        {
            get
            {
                if (HttpContext.Current.Session["_HospitalLocationId"] == null)
                    return "1";
                else
                    return HttpContext.Current.Session["_HospitalLocationId"].ToString();
            }
            set { HttpContext.Current.Session["_HospitalLocationId"] = value; }
        }

        /// <summary>
        /// Gets/Sets Session for FacilityId
        /// </summary>
        public static string FacilityId
        {
            get
            {
                if (HttpContext.Current.Session["_FacilityId"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_FacilityId"].ToString();
            }
            set { HttpContext.Current.Session["_FacilityId"] = value; }
        }

        /// <summary>
        /// Gets/Sets Session for Encounter Id
        /// </summary>
        public static string EncounterId
        {
            get
            {
                if (HttpContext.Current.Session["_EncounterId"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_EncounterId"].ToString();
            }
            set { HttpContext.Current.Session["_EncounterId"] = value; }
        }

        /// <summary>
        /// Gets/Sets Session for Encounter Date
        /// </summary>
        public static string EncounterDate
        {
            get
            {
                if (HttpContext.Current.Session["_EncounterDate"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_EncounterDate"].ToString();
            }
            set { HttpContext.Current.Session["_EncounterDate"] = value; }
        }

        /// <summary>
        /// Gets/Sets Session for User Id
        /// </summary>
        public static string UserId
        {
            get
            {
                if (HttpContext.Current.Session["_UserId"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_UserId"].ToString();
            }
            set { HttpContext.Current.Session["_UserId"] = value; }
        }
        /// <summary>
        /// Gets/Sets Session for Title
        /// </summary>
        public static string Title
        {
            get
            {
                if (HttpContext.Current.Session["_Title"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_Title"].ToString();
            }
            set { HttpContext.Current.Session["_Title"] = value; }
        }

        /// <summary>
        /// Gets/Sets Session for User First Name
        /// </summary>
        public static string FirstName
        {
            get
            {
                if (HttpContext.Current.Session["_FirstName"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_FirstName"].ToString();
            }
            set { HttpContext.Current.Session["_FirstName"] = value; }
        }
        /// <summary>
        /// Gets/Sets Session for Middle Name
        /// </summary>
        public static string MiddleName
        {
            get
            {
                if (HttpContext.Current.Session["_MiddleName"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_MiddleName"].ToString();
            }
            set { HttpContext.Current.Session["_MiddleName"] = value; }
        }
        /// <summary>
        /// Gets/Sets Session for Last Name
        /// </summary>
        public static string LastName
        {
            get
            {
                if (HttpContext.Current.Session["_LastName"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_LastName"].ToString();
            }
            set { HttpContext.Current.Session["_LastName"] = value; }
        }
        /// <summary>
        /// Gets/Sets Session for User Id
        /// </summary>
        public static string FullName
        {

            get
            {
                if (HttpContext.Current.Session["_FullName"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_FullName"].ToString();
            }
            set { HttpContext.Current.Session["_FullName"] = value; }
        }
        public static string TitleId
        {

            get
            {
                if (HttpContext.Current.Session["_TitleId"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_TitleId"].ToString();
            }
            set { HttpContext.Current.Session["_TitleId"] = value; }
        }
        public static string Gender
        {

            get
            {
                if (HttpContext.Current.Session["_Gender"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["_Gender"].ToString();
            }
            set { HttpContext.Current.Session["_Gender"] = value; }
        }
        public static string Menu
        {

            get
            {
                if (HttpContext.Current.Session["_Menu"] == null)
                    return "[]";
                else
                    return HttpContext.Current.Session["_Menu"].ToString();
            }
            set { HttpContext.Current.Session["_Menu"] = value; }
        }


        #endregion
    }
}