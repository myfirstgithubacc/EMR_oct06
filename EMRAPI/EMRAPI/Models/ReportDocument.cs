using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class ReportDocument
    {
        //PatientId	DocId	DiagSampleId	DocName	Description	Date
        public int PatientId { get; set; }
        public int DocId { get; set; }
        public int DiagSampleId { get; set; }
        public string DocName { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }

    }

    public class ReportInByteArray
    {

        public byte[] Report { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
}