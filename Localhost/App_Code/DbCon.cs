using System;
using System.Web.Services;
using System.Data.SqlClient;
using DBconnector;
using System.Data;

/// <summary>
/// Summary description for DbCon
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class DbCon : System.Web.Services.WebService {

    public DbCon () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    SymmCrypto cr = new SymmCrypto(SymmCrypto.SymmProvEnum.DES);
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public string DMLNonQuery(string Query, string ConnectionString, string Key, out string result)
        {
            string returns = "";
            try
            {
                SqlConnection con = new SqlConnection(cr.Decrypting(ConnectionString, Key));
                SqlCommand cmd = new SqlCommand(cr.Decrypting(Query, Key), con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                returns = "Command Successfull..";

            }
            catch (Exception ex)
            {
                returns = ex.Message;
            }
            result = returns;
            return result;
        }
        [WebMethod]
        public DataSet DMLDset(string Query, string ConnectionString, string Key, out string result)
        {
            string returns = "";
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(cr.Decrypting(ConnectionString, Key));
                SqlCommand cmd = new SqlCommand(cr.Decrypting(Query, Key), con);
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                con.Open();
                adpt.Fill(ds, "Recoreds");
                con.Close();
                returns = "Command Successfull..";

            }
            catch (Exception ex)
            {
                returns = ex.Message;
            }
            result = returns;
            return ds;
        }
        public object DMLScaler(string Query, string ConnectionString, string Key, out string result)
        {
            object obj = null;
            string returns = "";
            try
            {
                SqlConnection con = new SqlConnection(cr.Decrypting(ConnectionString, Key));
                SqlCommand cmd = new SqlCommand(cr.Decrypting(Query, Key), con);
                con.Open();
                obj = cmd.ExecuteScalar();
                con.Close();
                returns = "Command Successfull..";

            }
            catch (Exception ex)
            {
                returns = ex.Message;
            }
            result = returns;
            return obj;
        }
    }
    


