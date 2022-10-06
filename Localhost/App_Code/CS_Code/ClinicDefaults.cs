using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Web.UI;
using System.Web;

/// <summary>
/// Summary description for ClinicDefaults
/// </summary>
public class ClinicDefaults
{
    DataSet objDs;
    string sPath = "";

    public ClinicDefaults(Page page)
    {
        sPath = page.Server.MapPath("/DefaultData/ClinicDefaults.Data");
    }

    private DataSet CreateTable()
    {
        DataTable objDt = new DataTable();
        objDt.Columns.Add("Id", typeof(int));
        objDt.Columns["Id"].AutoIncrement = true;
        objDt.Columns["Id"].AutoIncrementSeed = 1;
        objDt.Columns["Id"].AutoIncrementStep = 1;
        objDt.Columns.Add("HospitalLocationId", typeof(Int16));
        objDt.Columns.Add("FieldKey", typeof(string));
        objDt.Columns.Add("FieldDescription", typeof(string));
        objDt.Columns.Add("FieldValue", typeof(string));
        objDs = new DataSet();
        objDs.Tables.Add(objDt);
        return objDs;
    }

    public Int16 StoreValue(string HospitalLocationId, string FieldKey, string FieldDescription, string FieldValue)
    {
        if (File.Exists(sPath) == false)
        {
            using (DataSet objDs = CreateTable())
            {
                DataRow objDr;

                objDr = objDs.Tables[0].NewRow();
                objDr["HospitalLocationId"] = HospitalLocationId;
                objDr["FieldKey"] = FieldKey;
                objDr["FieldDescription"] = FieldDescription;
                objDr["FieldValue"] = FieldValue;
                objDs.Tables[0].Rows.Add(objDr);

                try
                {
                    Int16 i = StoreFile(objDs, sPath);
                    return i;
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }
        }
        else
        {
            try
            {
                objDs = GetFileData();
                DataRow objDr;
                DataView objDv = objDs.Tables[0].DefaultView;
                objDv.RowFilter = "FieldKey ='" + FieldKey + "' And HospitalLocationId = '" + HospitalLocationId + "'";
                if (objDv.Count == 0)
                {
                    objDr = objDs.Tables[0].NewRow();
                    objDr["HospitalLocationId"] = HospitalLocationId;
                    objDr["FieldKey"] = FieldKey;
                    objDr["FieldDescription"] = FieldDescription;
                    objDr["FieldValue"] = FieldValue;
                    objDs.Tables[0].Rows.Add(objDr);
                    Int16 i = StoreFile(objDs, sPath);
                    return i;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }

    public DataSet GetHospitalDefaults()
    {
        try
        {
            objDs = GetFileData();
            return objDs;
        }
        catch (Exception)
        {
            throw new Exception();
        }

    }

    public DataTable GetHospitalDefaults(string HospitalLocationId)
    {
        try
        {
            objDs = GetFileData();
            DataView objDv = objDs.Tables[0].DefaultView;
            objDv.RowFilter = "HospitalLocationId = '" + HospitalLocationId + "'";
            return objDv.ToTable();
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    public string GetHospitalDefaults(string FieldKey, string HospitalLocationId)
    {
        try
        {
            objDs = GetFileData();
            DataView objDv = objDs.Tables[0].DefaultView;
            objDv.RowFilter = "FieldKey ='" + FieldKey + "' And HospitalLocationId = '" + HospitalLocationId + "'";
            if (objDv.Count > 0)
            {
                DataTable objDt = objDv.ToTable();
                return objDt.Rows[0]["FieldValue"].ToString();
            }
            else
                return "";
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    public Int16 UpdateKey(string FieldKey, string HospitalLocationId, string FieldDescription, string FieldValue)
    {
        try
        {
            objDs = GetFileData();
            DataRow[] objDr = objDs.Tables[0].Select("FieldKey ='" + FieldKey + "' And HospitalLocationId = '" + HospitalLocationId + "'");
            if (objDr.Count() != 0)
            {
                objDr[0]["FieldDescription"] = FieldDescription;
                objDr[0]["FieldValue"] = FieldValue;

                Int16 i = StoreFile(objDs, sPath);
                return i;
            }
            else
                return 0;
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
    }

    public Int16 DeleteKey(string FieldKey, string HospitalLocationId)
    {
        try
        {
            objDs = GetFileData();
            DataRow[] objDr = objDs.Tables[0].Select("FieldKey ='" + FieldKey + "' And HospitalLocationId = '" + HospitalLocationId + "'");
            if (objDr.Count() != 0)
            {
                objDs.Tables[0].Rows.Remove(objDr[0]);

                Int16 i = StoreFile(objDs, sPath);
                return i;
            }
            else
                return 0;
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
    }

    private Int16 StoreFile(DataSet objDs, string Path)
    {
        using (FileStream objFs1 = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            try
            {
                BinaryFormatter objBf1 = new BinaryFormatter();
                objBf1.Serialize(objFs1, objDs);
                objFs1.Close();
                return 1;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }

    private DataSet GetFileData()
    {
        if (File.Exists(sPath))
        {
            using (FileStream objFs = new FileStream(sPath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    BinaryFormatter objBf = new BinaryFormatter();
                    objDs = (DataSet)objBf.Deserialize(objFs);
                    objFs.Close();
                    return objDs;
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
        else
            return null;
    }

    public void SetControlDefaults(ref System.Web.UI.WebControls.DropDownList dropdownlist, string FieldKey)
    {
        if (HttpContext.Current.Cache["ClinicDefaults"] == null)
        {
            ClinicDefaults cd = new ClinicDefaults((System.Web.UI.Page)HttpContext.Current.Handler);
            string defaultReligion = cd.GetHospitalDefaults(FieldKey, System.Web.HttpContext.Current.Session["HospitalLocationID"].ToString());
            if (defaultReligion != "")
                dropdownlist.SelectedIndex = dropdownlist.Items.IndexOf(dropdownlist.Items.FindByValue(defaultReligion));

            System.Data.DataTable objDt = cd.GetHospitalDefaults(System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
            if (objDt.Rows.Count > 0)
            {
                HttpContext.Current.Cache.Insert("ClinicDefaults", objDt, null, DateTime.Now.AddMinutes(99999), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }
        else
        {
            DataTable objDt = (DataTable)HttpContext.Current.Cache["ClinicDefaults"];
            DataView objDv = objDt.DefaultView;
            objDv.RowFilter = "Fieldkey = '" + FieldKey + "'";
            if (objDv.Count > 0)
                dropdownlist.SelectedIndex = dropdownlist.Items.IndexOf(dropdownlist.Items.FindByValue(objDv.ToTable().Rows[0]["FieldValue"].ToString()));
        }
    }
}