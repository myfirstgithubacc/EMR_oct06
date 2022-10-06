using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for Photo
/// </summary>
public class Photo
{
    private static readonly object _key = new object();
    private static int _counter;

    public Photo()
    {
        Id = GetId();
    }

    public string Name { get; set; }
    public byte[] Data { get; set; }
    public int Id { get; private set; }

    protected static int GetId()
    {
        lock (_key)
        {
            _counter++;
        }
        return _counter;
    }
}
