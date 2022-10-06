using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PreAuthAPI
/// </summary>
public class PreAuthAPI
{
    public PreAuthAPI()
    {

    }
    public class Rootobject
    {
        public Patientinfo[] Patientinfo { get; set; }
    }
    public class Patientinfo
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Gender { get; set; }
        public string Date_of_Birth { get; set; }
        public string Age { get; set; }
        public string Cell_Phone { get; set; }
        public string Relationship { get; set; }
        public string Insurance_Company { get; set; }
        public string TPA_Name { get; set; }
        public string Policy { get; set; }
        public string TPA_Card { get; set; }
        public string Valid_From { get; set; }
        public string Valid_Upto { get; set; }
    }
}