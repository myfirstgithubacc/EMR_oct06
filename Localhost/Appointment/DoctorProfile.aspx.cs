using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

public partial class Appointment_DoctorProfile : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds;
    BaseC.Appointment Obj;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (common.myInt(Request.QueryString["resounceId"]) != 0)
                {

                    Obj = new BaseC.Appointment(sConString);
                    ds = Obj.GetDoctorProfile(common.myInt(Request.QueryString["resounceId"]));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (common.myStr(dr["ElementName"]) == "Email")
                            {
                                lblEmail.Text = common.myStr(dr["Value"]);
                            }
                            if (common.myStr(dr["ElementName"]) == "Name")
                            {
                                lblDoctorname.Text = common.myStr(dr["Value"]);
                            }
                            //if (common.myStr(dr["ElementName"]) == "Gender")
                            //{
                            //    lblGender.Text = common.myStr(dr["Value"]);
                            //}
                            if (common.myStr(dr["ElementName"]) == "Mobile")
                            {
                                lblContactNo.Text = common.myStr(dr["Value"]);
                            }
                            if (common.myStr(dr["ElementName"]) == "Education")
                            {
                                lblQualification.Text = common.myStr(dr["Value"]);
                            }
                            if (common.myStr(dr["ElementName"]) == "Designation")
                            {
                                lblDesignation.Text = common.myStr(dr["Value"]);
                            }
                        }
                    }
                }
            }

            catch (Exception Ex)
            {

                objException.HandleException(Ex);
            }
            //added by sikandar for code optimize
            finally
            {
                Obj = null;
            }



        }
    }
}