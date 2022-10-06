using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class LoginModel
    {

    }
    public class RedirectionHandler
    {
        public int UserId { get; set; }
        public string IPaddress { get; set; }
        public string Redirection { get; set; }
        public string RedirectionCode { get; set; }
        public int Timeout { get; set; }
    }
    public class UpdateQMSDoctorLogin
    {
        public string IPaddress { get; set; }
    }
    //public class DeleteFiles
    //{
    //    public string DirectoryPath { get; set; }
    //}
    
    public class CheckQueries
    {
        public int EmployeeId { get; set; }
    }
    
    public class getEmployeeTypePermission
    {
        public int EmployeeId { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int GroupId { get; set; }

    }
    public class ValidateUserName
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
    public class ValidateUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class getMainFacilityId
    {
        public int FacilityId { get; set; }
    }
    public class LockUser
    {
        public string UserId { get; set; }
    }
    public class getFacilityInterfaceDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class getHospitalLogo
    {
        public int HospitalLocationId { get; set; }
    }
    public class saveQMSlogin
    {
        public int UserID { get; set; }
        public int DoctorId { get; set; }
        public string IPadderss { get; set; }
        public int FacilityId { get; set; }
    }

    public class SecurityUserPages
    {
        public string Url { get; set; }
        public int UserId { get; set; }
        public int HospitalLocationID { get; set; }
        public int FacilityId { get; set; }
    }
}