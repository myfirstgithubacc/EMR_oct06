using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class PateintProblemModel : SavePatientProblems
    {
        public PateintProblemModel()
        {
            PatientProblem = new List<ChiefComplaint>();
        }

        public List<ChiefComplaint> PatientProblem { get; set; }
        
    }
    public class DoctorProblemModel
    {
        public DoctorProblemModel()
        {
            DoctorProblem = new List<ChiefComplaint>();
        }
        public List<ChiefComplaint> DoctorProblem { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
    }

    public class ChiefComplaint
    {
        public int problemId { get; set; }
        public string problemName { get; set; }
        public string duration { get; set; }
        public string intduration { get; set; }
        public string remarks { get; set; }
        
    }
}