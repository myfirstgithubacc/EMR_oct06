using EMRAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMRAPI
{
  public  class CommonReportInputParams:CommonBaseClass
    {
        public string InvoiceId { get; set; }
        public string YearId { get; set; }
        public string UserId { get; set; }
        public string QueryString { get; set; }
        //public string ReportName { get; set; }
    }
}
