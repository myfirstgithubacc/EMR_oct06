using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class PatientMessageModel
    {

    }
    public class PatientMessageInput
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public int RegistrationNo { get; set; }
        public int EncounterId { get; set; }
        public int DoctorId { get; set; }
        public string PatientName { get; set; }
        public string GenderAge { get; set; }
        public string VisitType { get; set; }
        public string MessageText { get; set; }
        public int total { get; set; }
        public int refrenceid { get; set; }

        public string mchar { get; set; }

        public string  DateTime { get; set; }

        public string DoctorName { get; set; }

        public int unread { get; set; }

        public string EncounterDate { get; set; }
    }
}