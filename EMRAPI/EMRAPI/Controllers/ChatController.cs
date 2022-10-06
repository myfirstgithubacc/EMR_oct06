
using EMRAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMRAPI.Controllers
{

    public class ChatController : ApiController
    {

        private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

        private Hashtable hshInput = new Hashtable();
        DAL.DAL objDl;
        [HttpPost]
        [Route("api/Chat/SaveMessageChat")]
        public IHttpActionResult SaveMessageChat(PatientMessageInput inputParam)
        {
            DataSet ds = new DataSet();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            Hashtable HshOut = new Hashtable();
            hshInput.Add("@Id", inputParam.Id);
            hshInput.Add("@RegistrationId", inputParam.RegistrationId);
            hshInput.Add("@RegistrationNo", inputParam.RegistrationNo);
            hshInput.Add("@EncounterId", inputParam.EncounterId);
            hshInput.Add("@DoctorId", inputParam.DoctorId);
            hshInput.Add("@PatientName", inputParam.PatientName);
            hshInput.Add("@GenderAge", inputParam.GenderAge);
            hshInput.Add("@VisitType", inputParam.VisitType);
            hshInput.Add("@MessageText", inputParam.MessageText);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveDoctorMessageChat", hshInput, HshOut);              
            return Ok(HshOut["@chvErrorStatus"].ToString());
        }

        [HttpPost]
        [Route("api/Chat/GetNotification")]
        public IHttpActionResult GetNotification(PatientMessageInput inputParam)
        {

            DataSet ds = new DataSet();
            hshInput = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@DoctorId", inputParam.DoctorId);
            hshInput.Add("@RegistrationNo", inputParam.RegistrationNo);
            hshInput.Add("@Encounterid", inputParam.EncounterId);
            hshInput.Add("@char", inputParam.mchar);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDoctorNotification", hshInput);

            List<PatientMessageInput> Messagemodel = new List<PatientMessageInput>();
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    PatientMessageInput model = new PatientMessageInput();
                    model.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                    model.RegistrationId = Convert.ToInt32(ds.Tables[0].Rows[i]["RegistrationId"]);
                    model.RegistrationNo = Convert.ToInt32(ds.Tables[0].Rows[i]["RegistrationNo"]);
                    model.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                    model.GenderAge = ds.Tables[0].Rows[i]["GenderAge"].ToString();
                    model.VisitType = ds.Tables[0].Rows[i]["VisitType"].ToString();
                    model.EncounterId = Convert.ToInt32(ds.Tables[0].Rows[i]["EncounterId"]);
                    model.MessageText = ds.Tables[0].Rows[i]["MessageText"].ToString();
                    model.total = Convert.ToInt16(ds.Tables[0].Rows[i]["total"]);
                    model.refrenceid = Convert.ToInt16(ds.Tables[0].Rows[i]["RefrenceId"]);
                    model.DateTime = ds.Tables[0].Rows[i]["EncodedDate"].ToString(); 
                    model.DoctorName = ds.Tables[0].Rows[i]["DoctorName"].ToString();
                    model.DoctorId = Convert.ToInt16(ds.Tables[0].Rows[i]["DoctorId"]);
                    model.unread = Convert.ToInt16(ds.Tables[0].Rows[i]["Unread"]);
                    model.EncounterDate = ds.Tables[0].Rows[i]["EncounterDate"].ToString();
                    Messagemodel.Add(model);
                }
            }
            return Ok(Messagemodel);
        }

        [HttpPost]
        [Route("api/Chat/GetTotalNotification")]
        public IHttpActionResult GetTotalNotification(PatientMessageInput inputParam)
        {

            DataSet ds = new DataSet();
            hshInput = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@RegistrationNo", inputParam.RegistrationNo);
            hshInput.Add("@char", inputParam.mchar);
            hshInput.Add("@Encounterid", inputParam.EncounterId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientTotalNotification", hshInput);
            List<PatientMessageInput> Messagemodel = new List<PatientMessageInput>();
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    PatientMessageInput model = new PatientMessageInput();
                    model.total = Convert.ToInt16(ds.Tables[0].Rows[i]["total"]);
                    Messagemodel.Add(model);
                }
            }
            return Ok(Messagemodel);
        }

    }
}
