using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Data;
/// <summary>
/// Summary description for ProblemAutoSuggestion
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class ProblemAutoSuggestion : System.Web.Services.WebService {

    public ProblemAutoSuggestion () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    [WebMethod]
    //[Script.Services.ScriptMethod()]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCompletionList(string prefixText, int count)
    {
        string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        List<string> names = new List<string>();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshInput = new Hashtable();
        hshInput.Add("@chvSearchCriteria", "%" + prefixText.ToString() + "%");
        hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
        DataSet objDs = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetProblemsList", hshInput);
        foreach (DataRow datarow in objDs.Tables[0].Rows)
        {
            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(datarow["ProblemDescription"].ToString(), datarow["ProblemId"].ToString());
            names.Add(item);
        }
        return names.ToArray();
    }
    
}

