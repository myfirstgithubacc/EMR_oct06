using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsLabTemplate
/// </summary>

[Serializable]
public class clsLabTemplate
{
    public bool isResultEntered = false;
    public int ServiceId = 0;
    public int DiagSampleId = 0;
    public int RegistrationId = 0;
    public int ResultRemarksId = 0;
    public int ResultEntered = 0;
    public int ResultAlert = 0;
    public string StatusCode = "";
    public string ReviewRemark = string.Empty;
    public DataSet dsField = new DataSet();
    public DataSet dsOrganism = new DataSet();
    public int SampleTypeId = 0;
}
