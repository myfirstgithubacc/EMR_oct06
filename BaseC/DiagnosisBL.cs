using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace BaseC
{
    public class DiagnosisBL
    {
        public DiagnosisBL() { }
        Int16 intHospitalLocationID;
        Int32 intRegistrationNo;
        Int32 intEncodedBy;
        Int32 intEncounterId;
        Int32 intDoctorId;
        String xml;
        String xmlCPT;
        Char cSide;

        public Int16 HospitalLocationId
        {
            set { intHospitalLocationID = value; }
            get { return intHospitalLocationID; }
        }
        public Int32 RegistrationNo
        {
            set { intDoctorId = value; }
            get { return intDoctorId; }
        }
        public Int32 DoctorId
        {
            set { intRegistrationNo = value; }
            get { return intRegistrationNo; }
        }
        public String XmlData
        {
            set { xml = value; }
            get { return xml; }
        }
        public String XmlCPTData
        {
            set { xmlCPT = value; }
            get { return xmlCPT; }
        }
        public Int32 EncodedBy
        {
            set { intEncodedBy = value; }
            get { return intEncodedBy; }
        }
        public Int32 EncounterId
        {
            set { intEncounterId = value; }
            get { return intEncounterId; }
        }
        public char Side
        {
            set { cSide = value; }
            get { return cSide; }
        }
    }
}