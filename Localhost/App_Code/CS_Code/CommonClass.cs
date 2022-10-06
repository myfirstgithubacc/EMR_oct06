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
/// Summary description for CommonClass
/// </summary>
public class CommonClass<T>
{
    T hold;
	public CommonClass()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public void FocusButton(T hold, Button myBtn)
    {
        if (hold is WebControl)
        {
            WebControl wc = hold as WebControl;
            wc.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + myBtn.ClientID + "').click();return false;}} else {return true}; ");
        }
    }
    public void RestrictCutCopyPasteDragDrop(T hold)
    {
        if (hold is TextBox)
        {
            TextBox txt = hold as TextBox;
            txt.Attributes.Add("onCut", "return false");
            txt.Attributes.Add("onCopy", "return false");
            txt.Attributes.Add("onPaste", "return false");
            txt.Attributes.Add("onDrag", "return false");
            txt.Attributes.Add("onDrop", "return false");
        }
    }
}
