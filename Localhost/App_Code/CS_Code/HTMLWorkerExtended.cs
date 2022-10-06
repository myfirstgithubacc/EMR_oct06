using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf.draw;
using iTextSharp.text;
/// <summary>
/// Summary description for HTMLWorkerExtended
/// </summary>
public class HTMLWorkerExtended : HTMLWorker
{
    LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER,0);
    public HTMLWorkerExtended(IDocListener document) : base(document)
    {

    }
    public override void StartElement(string tag, IDictionary<string, string> str)
    {
        if (tag.Equals("hr"))
            document.Add(new Chunk(line));
        else
            base.StartElement(tag, str);
    }
}