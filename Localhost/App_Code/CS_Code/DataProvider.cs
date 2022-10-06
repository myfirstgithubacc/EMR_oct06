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
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
/// <summary>
/// Summary description for DataProvider
/// </summary>
public class DataProvider
{

    [ThreadStatic]
    private static List<Photo> _photos;

    public static IList<Photo> GetData()
    {
        //if (_photos != null)
        //{
        //    return _photos;
        //}

        _photos = new List<Photo>();
        foreach (var file in Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("/PatientDocuments/PatientImages/Images")))
        {
            var photo = new Photo { Name = Path.GetFileName(file) };

            System.Drawing.Image image = System.Drawing.Image.FromFile(file);
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Jpeg);
                photo.Data = memoryStream.ToArray();
            }
            _photos.Add(photo);
        }
        return _photos;
    }

    public static void Update(int photoId, string name)
    {
        Photo first = _photos.FirstOrDefault(p => p.Id == photoId);

        if (first != null)
            first.Name = name;
    }
}

