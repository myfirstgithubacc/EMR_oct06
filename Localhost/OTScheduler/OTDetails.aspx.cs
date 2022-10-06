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

public partial class OT_Scheduler_OTDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet ds;
    Hashtable hshIn;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvSurgeon.Visible = false;
            gvAsstSurgeon.Visible = false;
            gvAnesthetist.Visible = false;
            gvPerfusionist.Visible = false;
            gvScrubNurse.Visible = false;
            gvFloorNurse.Visible = false;
            gvOTTechnician.Visible = false;
            gvEquipment.Visible = false;
            BindOTBookingDetails();
        }
    }




    private void BindOTBookingDetails()
    {
        BaseC.clsOTBooking objOTBooking = new BaseC.clsOTBooking(sConString);
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataTable dt = new DataTable();
        DataRow dr;
        int HospId = common.myInt(Session["HospitalLocationId"]);
        int FacilityId = common.myInt(Session["FacilityId"]);
        int OTBookingId = common.myInt(Request.QueryString["OTBookingId"]);
        int ServiceId = common.myInt(Request.QueryString["ServiceId"]);
        string OTBookingNo = "";

        ds = objOTBooking.getOTBookingDetails(HospId, FacilityId, OTBookingId, OTBookingNo);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "ServiceId =" + ServiceId;
            dt = dv.ToTable();
            if (dv.ToTable().Rows.Count > 0)
            {
                dr = dt.Rows[0];

                lblPatientName.Text = common.myStr(dr["FirstName"]) + " " + common.myStr(dr["MiddleName"]) + " " + common.myStr(dr["LastName"]);
                lblRegistrationNo.Text = common.myStr(dr["RegistrationNo"]);
                lblDob.Text = common.myStr(dr["DOB"]);
                lblGender.Text = common.myStr(dr["Gender"]);
                lblTheaterName.Text = common.myStr(dr["TheatreName"]);
                lblOTBookingDate.Text = common.myStr(dr["OTBookingDateF"]);
                lblFromTome.Text = common.myStr(dr["FromTime"]);
                lblToTime.Text = common.myStr(dr["ToTime"]);

                lblDepartment.Text = common.myStr(dr["DepartmentName"]);
                lblSubDepartment.Text = common.myStr(dr["SubDepartmentName"]);
                lblServiceName.Text = common.myStr(dr["ServiceName"]);
            }

            dv = new DataView(ds.Tables[1]);
            dv.RowFilter = "ServiceId =" + ServiceId + " AND ResourceType = 'SR'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvSurgeon.DataSource = dv.ToTable();
                gvSurgeon.DataBind();
                gvSurgeon.Visible = true;
            }

            dv = new DataView(ds.Tables[1]);
            dv.RowFilter = "ServiceId =" + ServiceId + " AND ResourceType = 'AS'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvAsstSurgeon.DataSource = dv.ToTable();
                gvAsstSurgeon.DataBind();
                gvAsstSurgeon.Visible = true;
            }
            
            dv = new DataView(ds.Tables[1]);
            dv.RowFilter = "ServiceId =" + ServiceId + " AND ResourceType = 'AN'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvAnesthetist.DataSource = dv.ToTable();
                gvAnesthetist.DataBind();
                gvAnesthetist.Visible = true;
            }

            dv = new DataView(ds.Tables[1]);
            dv.RowFilter = "ServiceId =" + ServiceId + " AND ResourceType = 'AAN'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvAssttAnesthetist.DataSource = dv.ToTable();
                gvAssttAnesthetist.DataBind();
                gvAssttAnesthetist.Visible = true;
            }
            dv = new DataView(ds.Tables[1]);
            dv.RowFilter = "ServiceId =" + ServiceId + " AND ResourceType = 'PR'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvPerfusionist.DataSource = dv.ToTable();
                gvPerfusionist.DataBind();
                gvPerfusionist.Visible = true;
            }

            dv = new DataView(ds.Tables[1]);
            dv.RowFilter = "ServiceId =" + ServiceId + " AND ResourceType = 'SN'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvScrubNurse.DataSource = dv.ToTable();
                gvScrubNurse.DataBind();
                gvScrubNurse.Visible = true;
            }

            dv = new DataView(ds.Tables[1]);
            dv.RowFilter = "ServiceId =" + ServiceId + " AND ResourceType = 'FN'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvFloorNurse.DataSource = dv.ToTable();
                gvFloorNurse.DataBind();
                gvFloorNurse.Visible = true;
            }

            dv = new DataView(ds.Tables[1]);
            dv.RowFilter = "ServiceId =" + ServiceId + " AND ResourceType = 'TC'";
            if (dv.ToTable().Rows.Count > 0)
            {
                gvOTTechnician.DataSource = dv.ToTable();
                gvOTTechnician.DataBind();
                gvOTTechnician.Visible = true;
            }

            dv = new DataView(ds.Tables[2]);
            dv.RowFilter = "ServiceId =" + ServiceId;
            if (dv.ToTable().Rows.Count > 0)
            {
                gvEquipment.DataSource = dv.ToTable();
                gvEquipment.DataBind();
                gvEquipment.Visible = true;
            }

        }
    }


}
