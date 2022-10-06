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
using System.IO;
using System.Data.SqlClient;
using System.Text;

public partial class Include_Components_PatientQView : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    clsExceptionLog objException = new clsExceptionLog();

    //private int iRegistrationId = 0;
    //public int RegistrationId
    //{
    //    get
    //    {
    //        return iRegistrationId;
    //    }
    //    set
    //    {
    //        iRegistrationId = value;
    //        //ShowPatientDetails(iRegistrationId);
    //    }
    //}  

    public void Page_Load(object sender, EventArgs e)
    {
        //ShowPatientDetails(iRegistrationId);
    }

    public void ShowPatientDetails(int RegId)
    {
        try
        {
            DataSet ds = new DataSet();
            Hashtable HashIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder strSQL = new StringBuilder();
            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmdTemp;
            SqlParameter prm2;
            strSQL.Append("select PatientImage,ImageType from RegistrationImage where RegistrationId=@RegistrationNo and Active=1");
            cmdTemp = new SqlCommand(strSQL.ToString(), con);
            cmdTemp.CommandType = CommandType.Text;
            prm2 = new SqlParameter();
            prm2.ParameterName = "@RegistrationNo";
            prm2.Value = RegId;
            prm2.SqlDbType = SqlDbType.Int;
            prm2.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm2);
            con.Open();
            SqlDataReader dr = cmdTemp.ExecuteReader();
            if (dr.HasRows == true)
            {
                dr.Read();
                Stream strm;
                Object img = dr["PatientImage"];
                String FileName = dr["ImageType"].ToString();
                if (FileName != "")
                {
                    strm = new MemoryStream((byte[])img);
                    byte[] buffer = new byte[strm.Length];
                    int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                    FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                    fs.Write(buffer, 0, byteSeq);
                    fs.Dispose();
                    imgFindPatient.ImageUrl = "/PatientDocuments/PatientImages/" + FileName;
                }
            }
            else
            {
                imgFindPatient.ImageUrl = "~/Images/no_photo.jpg";
            }
            dr.Close();
            HashIn.Add("@intRegistrationId", RegId);
            HashIn.Add("@inyHospitalLocationId", Session["HospitalLocationId"]);
            HashIn.Add("@intFacilityId", Session["FacilityId"]);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDetails", HashIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.lbl_PatientName.Text = ds.Tables[0].Rows[0]["PatientName"].ToString();
                this.lbl_AgeGender.Text = ds.Tables[0].Rows[0]["GenderAge"].ToString();
              //  this.lbl_Dob.Text = "DOB: " + ds.Tables[0].Rows[0]["DOB"].ToString();
                if (ds.Tables[0].Rows[0]["PhoneHome"].ToString() != "___-___-____")
                {
                    this.lbl_PhoneHome.Text = ds.Tables[0].Rows[0]["PhoneHome"].ToString();
                }
                else
                {
                    this.lbl_PhoneHome.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                }
                StringBuilder Address = new StringBuilder();
               // Address.Append(ds.Tables[0].Rows[0]["LocalAddress"].ToString());
               // Address.Append("<br />" + ds.Tables[0].Rows[0]["CityName"].ToString() + " " + ds.Tables[0].Rows[0]["StateName"].ToString() + " " + ds.Tables[0].Rows[0]["LocalPin"].ToString());
                Address.Append(ds.Tables[0].Rows[0]["CityName"].ToString() + " " + ds.Tables[0].Rows[0]["StateName"].ToString() + "");
                this.lbl_Address.Text = Address.ToString();
                //this.lbl_Prim_Insurance.Text = "Pyr: " + ds.Tables[0].Rows[0]["PatientPayername"].ToString();
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            Response.Write("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
    }
}
