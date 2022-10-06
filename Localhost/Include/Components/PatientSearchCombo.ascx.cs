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
using Telerik.Web.UI;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;

public partial class Include_Components_PatientSearchCombo : System.Web.UI.UserControl
{
    
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    clsExceptionLog objException = new clsExceptionLog();
    private string strText = "";
    private static string pStatus = "A";

    private string strSearchText
    {
        get
        {
            return strText;
        }
        set
        {
            strText = value;
        }
    }

    private const int ItemsPerRequest = 50;

    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    public void RadComboBoxProduct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = GetScrolledData(e.Text);

        int itemOffset = e.NumberOfItems;
        if (itemOffset == 0)
        {
            this.RadCmbPatientSearch.Items.Clear();
        }
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
        e.EndOfItems = endOffset == data.Rows.Count;

        for (int i = itemOffset; i < endOffset; i++)
        {
            //RadCmbPatientSearch.Items.Add(new RadComboBoxItem(data.Rows[i]["CompanyName"].ToString(), data.Rows[i]["CompanyName"].ToString()));
            RadComboBoxItem item = new RadComboBoxItem();
            item.Text = (string)data.Rows[i]["PatientName"];
            item.Value = data.Rows[i]["RegistrationId"].ToString();
            item.Attributes.Add("Account", data.Rows[i]["RegistrationNo"].ToString());
            item.Attributes.Add("DOB", data.Rows[i]["DOB"].ToString());
            item.Attributes.Add("Gender", data.Rows[i]["Gender"].ToString());
            item.Attributes.Add("PhoneHome", data.Rows[i]["PhoneHome"].ToString());
            this.RadCmbPatientSearch.Items.Add(item);
            item.DataBind();
        }
        e.Message = GetStatusMessage(endOffset, data.Rows.Count);
    }

    public void GetData(string text)
    {
        try
        {
            this.RadCmbPatientSearch.Items.Clear();
            string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
            //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Hashtable hashtable = new Hashtable();
            //hashtable.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
            //hashtable.Add("@strName", text);

            //if (Convert.ToBoolean(HttpContext.Current.Session["A"]) == true)
            //{
            //    hashtable.Add("@strStatus", 'A');
            //}
            //else if (Convert.ToBoolean(HttpContext.Current.Session["D"]) == true)
            //{
            //    hashtable.Add("@strStatus", 'D');
            //}
            //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "SearchPatientByName", hashtable);

            string ServiceURL = WebAPIAddress.ToString() + "api/Common/SearchPatientByName";

            APIRootClass.SearchPatientByName objRoot = new global::APIRootClass.SearchPatientByName();

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.PatientName = text;
            
            if (Convert.ToBoolean(HttpContext.Current.Session["A"]) == true)
            {
                objRoot.Status = "A";
            }
            else if (Convert.ToBoolean(HttpContext.Current.Session["D"]) == true)
            {
                objRoot.Status = "D";
            }

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet dataset = JsonConvert.DeserializeObject<DataSet>(sValue);

            DataTable dataTable = dataset.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dataRow["PatientName"];
                item.Value = dataRow["RegistrationId"].ToString();
                item.Attributes.Add("Account", dataRow["RegistrationNo"].ToString());
                item.Attributes.Add("DOB", dataRow["DOB"].ToString());
                item.Attributes.Add("Gender", dataRow["Gender"].ToString());
                item.Attributes.Add("PhoneHome", dataRow["PhoneHome"].ToString());
                this.RadCmbPatientSearch.Items.Add(item);
                item.DataBind();
            }
        }
        catch (Exception Ex)
        {
            
            objException.HandleException(Ex);
        }
    }

    public void ClearRadGrid()
    {
        this.RadCmbPatientSearch.Text = "";
        this.RadCmbPatientSearch.SelectedValue = "";
    }

    public void SelectRadGrid(string Id)
    {
        this.RadCmbPatientSearch.SelectedValue = Id;
    }

    public string GetSelectedItemText()
    {
        string strname = "";
        if (RadCmbPatientSearch.SelectedIndex != -1)
        {
            strname = this.RadCmbPatientSearch.SelectedItem.Text;
        }
        return strname;
    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";
        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }

    private static DataTable GetScrolledData(string text)
    {
        //string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        //DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hashtable = new Hashtable();
        //hashtable.Add("@inyHospitalLocationId", Convert.ToInt32(HttpContext.Current.Session["HospitalLocationId"]));
        //if (text.ToString().Trim() != "")
        //{
        //    hashtable.Add("@strName", text.ToString().Trim());
        //}
        //if (Convert.ToBoolean(HttpContext.Current.Session["A"]) == true)
        //{
        //    hashtable.Add("@strStatus", 'A');
        //}
        //else if (Convert.ToBoolean(HttpContext.Current.Session["D"]) == true)
        //{
        //    hashtable.Add("@strStatus", 'D');
        //}
        //DataSet dataset = objSave.FillDataSet(CommandType.StoredProcedure, "SearchPatientByName", hashtable);

        string ServiceURL = WebAPIAddress.ToString() + "api/Common/SearchPatientByName";

        APIRootClass.SearchPatientByName objRoot = new global::APIRootClass.SearchPatientByName();

        objRoot.HospitalLocationId = common.myInt(HttpContext.Current.Session["HospitalLocationId"]);
        objRoot.PatientName = text;

        if (Convert.ToBoolean(HttpContext.Current.Session["A"]) == true)
        {
            objRoot.Status = "A";
        }
        else if (Convert.ToBoolean(HttpContext.Current.Session["D"]) == true)
        {
            objRoot.Status = "D";
        }

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        DataSet dataset = JsonConvert.DeserializeObject<DataSet>(sValue);

        DataTable dataTable = dataset.Tables[0];
        return dataTable;
    }

    protected void RadCmbPatientSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadCmbPatientSearch.SelectedValue != "")
        {
            HiddenField hdnddlid = (HiddenField)Page.FindControl("hdnddlid");
            if (hdnddlid != null)
            {
                hdnddlid.Value = RadCmbPatientSearch.SelectedValue;
            }
        }
    }
}