using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Telerik.Web.UI;
using BaseC;

public partial class Include_Master_HSEMRMaster : System.Web.UI.MasterPage
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private Hashtable hshInput;
    public void bind()
    {
        Alert.ShowAjaxMsg("hi", Page);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {                    
           // ltrlYear.Text = System.DateTime.Now.Year.ToString();
            Page.Header.Title = "";
        }

        //if (Session["registrationno"] != null)
        //{
        //    Label lbl = new Label();
        //    string strPatientInfo="";
        //    BaseC.Patient objPatient = new BaseC.Patient(sConString);
        //    SqlDataReader objDr = (SqlDataReader)objPatient.getPatientDetails(Convert.ToInt32(Session["registrationno"]), Convert.ToInt32(Session["HospitalLocationID"]));
        //    if (objDr.Read())

        //    lbl.Text = objDr["name"].ToString();
        //    strPatientInfo = lbl.Text;
        //    Radslidingpane4.Title = strPatientInfo;
        //}
    }
    //protected void btnCheck_Click(object sender, EventArgs e)
    //{
    //    EncryptDecrypt en = new EncryptDecrypt();
    //     string strUserId = Convert.ToString(Session["UserID"]);
    //     strUserId = en.Encrypt(strUserId, en.getKey(sConString), true);       
    //    if (Convert.ToString(Session["UserID"]) != null || Convert.ToString(Session["UserID"]) !="")
    //    {
    //        string strHLId = Convert.ToString(Session["HospitalLocationID"]);
    //        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        string str = "";
    //        str = "update users set IsMenuPaneDocked=1 where UserName ='" + strUserId + "' and HospitalLocationID='" + strHLId + "'";
    //        dl.ExecuteNonQuery(CommandType.Text, str);
    //    }
    //}
    //public void test2(object sender, System.EventArgs e)
    //{
    //    EncryptDecrypt en = new EncryptDecrypt();
    //    string strUserId = Convert.ToString(Session["UserID"]);
    //    strUserId = en.Encrypt(strUserId, en.getKey(sConString), true);
    //    //Menu Docked start   
    //    SlidingZone1.DockedPaneId = "RadSlidingPane1";
    //    //Menu Docked End
    //}
  
}
