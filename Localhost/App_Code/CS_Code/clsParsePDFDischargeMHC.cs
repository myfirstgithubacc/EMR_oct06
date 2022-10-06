using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for clsParsePDFDischarge
/// Created by Rajeev 
/// Use for only Discharge summary print pdf file
/// </summary>
public class PDFPageEventHandlerSummaryMHC : PdfPageEventHelper
{
    //public clsParsePDFDischarge()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //}
    // This is the contentbyte object of the writer
    PdfContentByte cb;

    // we will put the final number of pages in a template
    PdfTemplate template, footerTemplate;

    // this is the BaseFont we are going to use for the header / footer
    BaseFont bf = null;

    // This keeps track of the creation time
    DateTime PrintTime = DateTime.Now;

    #region Properties


    bool _IsHidePaging;

    public bool IsHidePaging
    {
        get { return _IsHidePaging; }
        set { _IsHidePaging = value; }
    }

    string _FirstPageHeaderHTML;
    public string FirstPageHeaderHTML
    {
        get { return _FirstPageHeaderHTML; }
        set { _FirstPageHeaderHTML = value; }
    }

    string _FirstPageFooterHTML;
    public string FirstPageFooterHTML
    {
        get { return _FirstPageFooterHTML; }
        set { _FirstPageFooterHTML = value; }
    }

    string _HeaderHTML;
    public string HeaderHTML
    {
        get { return _HeaderHTML; }
        set { _HeaderHTML = value; }
    }


    string _FooterLeftText;
    string _FooterCenterText;

    string _FooterLeft;
    string _FooterMiddle;
    string _FooterRight;
    string _FirstPageFooterLeft;
    string _FirstPageFooterRight;
    string _FirstPageFooterMiddle;
    string _LowestFooter;
    string _footerrighttext;

    public string FooterLeft
    {
        get { return _FooterLeft; }
        set { _FooterLeft = value; }
    }
    public string FooterMiddle
    {
        get { return _FooterMiddle; }
        set { _FooterMiddle = value; }
    }
    public string FooterRight
    {
        get { return _FooterRight; }
        set { _FooterRight = value; }
    }


    public string FooterCenterText
    {
        get { return _FooterCenterText; }
        set { _FooterCenterText = value; }
    }

    public string FooterRightText
    {
        get { return _footerrighttext; }
        set { _footerrighttext = value; }
    }
    public string FooterLeftText
    {
        get { return _FooterLeftText; }
        set { _FooterLeftText = value; }
    }
    public string FirstPageFooterLeft
    {
        get { return _FirstPageFooterLeft; }
        set { _FirstPageFooterLeft = value; }
    }
    public string FirstPageFooterMiddle
    {
        get { return _FirstPageFooterMiddle; }
        set { _FirstPageFooterMiddle = value; }
    }
    public string FirstPageFooterRight
    {
        get { return _FirstPageFooterRight; }
        set { _FirstPageFooterRight = value; }
    }

    private Font _HeaderFont;
    public Font HeaderFont
    {
        get { return _HeaderFont; }
        set { _HeaderFont = value; }
    }

    private Font _FooterFont;
    public Font FooterFont
    {
        get { return _FooterFont; }
        set { _FooterFont = value; }
    }

    private int _margintop;
    private int _marginbottom;
    private int _marginleft;
    private int _marginright;
    public int MarginTop
    {
        get { return _margintop; }
        set { _margintop = value; }
    }
    public int MarginBottom
    {
        get { return _marginbottom; }
        set { _marginbottom = value; }
    }
    public int MarginLeft
    {
        get { return _marginleft; }
        set { _marginleft = value; }
    }
    public int MarginRight
    {
        get { return _marginright; }
        set { _marginright = value; }
    }
    public string LowestFooter
    {
        get { return _LowestFooter; }
        set { _LowestFooter = value; }
    }
    #endregion

    // we override the onOpenDocument method
    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        try
        {
            PrintTime = DateTime.Now;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            //template = cb.CreateTemplate(50, 50);
            template = cb.CreateTemplate(100, 100);
            footerTemplate = cb.CreateTemplate(50, 50);
        }
        catch (DocumentException de)
        {
        }
        catch (System.IO.IOException ioe)
        {
        }
    }

    public override void OnStartPage(PdfWriter writer, Document document)
    {
        base.OnStartPage(writer, document);
        Rectangle pageSize = document.PageSize;

        if (writer.PageNumber == 1)
        {
            if (FirstPageHeaderHTML != "")
            {
                System.IO.StringReader reader = new System.IO.StringReader(FirstPageHeaderHTML);

                StyleSheet ST = new StyleSheet();
                ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Arial Unicode MS");
                //Set the default encoding to support Unicode characters
                ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);

                using (TextReader htmlViewReader = new StringReader(FirstPageHeaderHTML))
                {
                    using (var htmlWorker = new HTMLWorkerExtended(document))
                    {
                        htmlWorker.Open();
                        htmlWorker.Parse(htmlViewReader);
                    }
                }
                // List<IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(reader, null);
                //Paragraph para = new Paragraph();
                //para.InsertRange(0, objects);
                //document.Add(para);
            }
        }
        else
        {
            if (HeaderHTML != "")
            {
                System.IO.StringReader reader = new System.IO.StringReader(HeaderHTML);

                StyleSheet ST = new StyleSheet();
                ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Arial Unicode MS");
                //Set the default encoding to support Unicode characters
                ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);

                using (TextReader htmlViewReader = new StringReader(HeaderHTML))
                {
                    using (var htmlWorker = new HTMLWorkerExtended(document))
                    {
                        htmlWorker.Open();
                        htmlWorker.Parse(htmlViewReader);
                    }
                }
                //List<IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(reader, null);
                //Paragraph para = new Paragraph();
                //para.InsertRange(0, objects);
                //document.Add(para);
            }
        }
    }
    public override void OnEndPage(PdfWriter writer, Document document)
    {
        base.OnEndPage(writer, document);
        int pagen = writer.PageNumber;
        Rectangle pagesize = document.PageSize;
        string text = "";
        float len;
        if (!IsHidePaging)
        {
            if (pagen == 1)
            {
                #region


                switch (FirstPageFooterRight)
                {
                    case "N":
                        text = pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);
                        cb.EndText();

                        break;
                    case "NofN":
                        text = pagen.ToString() + " of";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);

                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10));
                        break;
                    case "PageN":
                        text = "Page " + pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);

                        cb.EndText();
                        break;
                    case "PageNofN":
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10));
                        break;
                    default:
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10));
                        break;
                }

            }
            else
            {
                switch (FooterRight)
                {
                    case "N":
                        text = pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);
                        cb.EndText();

                        break;
                    case "NofN":
                        text = pagen.ToString() + " of";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);

                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10));
                        break;
                    case "PageN":
                        text = "Page " + pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);

                        cb.EndText();
                        break;
                    case "PageNofN":
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10));
                        break;
                    default:
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10), 0);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(10));
                        break;
                }
            }
            #endregion      
        }

        //-------------
        Font font = new Font(bf, 8);
        Phrase phraseleft = new Phrase(FooterLeftText, font);//
        Phrase phrasemiddle = new Phrase(FooterCenterText, font);
        Phrase phraseright = new Phrase(FooterRightText, font);
        //Phrase phraseright = new Phrase();
        //Phrase phraseright = new Phrase(string.Format("Page {0}", writer.PageNumber), font);



        Rectangle page = document.PageSize;
        PdfPTable footer = new PdfPTable(3);
        footer.TotalWidth = page.Width;

        //left cell
        PdfPCell cL = new PdfPCell(phraseleft);
        cL.Border = Rectangle.NO_BORDER;
        //cL.Border = Rectangle.TOP_BORDER;
        cL.PaddingLeft = 10;
        cL.VerticalAlignment = Element.ALIGN_CENTER;
        cL.HorizontalAlignment = Element.ALIGN_LEFT;
        footer.AddCell(cL);

        //middle cell
        PdfPCell cM = new PdfPCell(phrasemiddle);
        cM.Border = Rectangle.NO_BORDER;
        //cM.Border = Rectangle.TOP_BORDER;
        cM.VerticalAlignment = Element.ALIGN_CENTER;
        cM.HorizontalAlignment = Element.ALIGN_LEFT;
        footer.AddCell(cM);

        //right cell
        PdfPCell cR = new PdfPCell(phraseright);
        cR.PaddingRight = 10;
        cR.Border = Rectangle.NO_BORDER;
        // cR.Border = Rectangle.TOP_BORDER;
        cR.VerticalAlignment = Element.ALIGN_CENTER;
        cR.HorizontalAlignment = Element.ALIGN_RIGHT;
        footer.AddCell(cR);

        // row 2
        if (LowestFooter != string.Empty)
        {
            Phrase phraseleft1 = new Phrase(LowestFooter, font);
            PdfPCell cL1 = new PdfPCell(phraseleft1);
            cL1.Colspan = 3;
            //cL1.Border = Rectangle.TOP_BORDER;
            cL1.PaddingLeft = 10;
            cL1.Border = Rectangle.NO_BORDER;
            cL1.VerticalAlignment = Element.ALIGN_BOTTOM;
            cL1.HorizontalAlignment = Element.ALIGN_LEFT;
            footer.AddCell(cL1);
        }

        footer.WriteSelectedRows(0, -1, 0, footer.TotalHeight + 5, writer.DirectContent);
        //-------------
    }
    public void AddOutline(PdfWriter writer, string Title, float Position)
    {
        PdfDestination destination = new PdfDestination(PdfDestination.FITH, Position);
        PdfOutline outline = new PdfOutline(writer.DirectContent.RootOutline, destination, Title);
        writer.DirectContent.AddOutline(outline, "Name = " + Title);
    }

    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);
        template.BeginText();
        template.SetFontAndSize(bf, 8);
        template.SetTextMatrix(0, 0);
        template.ShowText(" " + (writer.PageNumber).ToString());
        template.EndText();


        footerTemplate.BeginText();
        footerTemplate.SetFontAndSize(bf, 8);
        footerTemplate.SetTextMatrix(0, 0);
        footerTemplate.ShowText((writer.PageNumber).ToString());
        footerTemplate.EndText();
    }

}
/// <summary>
/// Summary description for PDFPageEventHandlerSummaryMHC
/// </summary>
public class clsParsePDFDischargeMHC
{
    public clsParsePDFDischargeMHC()
    {
        //
        // TODO: Add constructor logic here
        //

    }
    private int _margintop;
    private int _marginbottom;
    private int _marginleft;
    private int _marginright;
    private string _firstpageheaderhtml;
    private string _firstpagefooterhtml;
    private string _headerhtml;
    private string _html;
    private string _firstpagefooterleft;
    private string _firstpagefooterright;
    private string _firstpagefootermiddle;
    private string _footerleft;
    private string _footerright;
    private string _footermiddle;
    private string _footerlefttext;
    private string _footercentertext;
    private string _lowestfooter;
    string _footerrighttext;

    public int MarginTop
    {
        get { return _margintop; }
        set { _margintop = value; }
    }
    public int MarginBottom
    {
        get { return _marginbottom; }
        set { _marginbottom = value; }
    }
    public int MarginLeft
    {
        get { return _marginleft; }
        set { _marginleft = value; }
    }
    public int MarginRight
    {
        get { return _marginright; }
        set { _marginright = value; }
    }
    public string FirstPageHeaderHtml
    {
        get { return _firstpageheaderhtml; }
        set { _firstpageheaderhtml = value; }
    }
    public string FirstPageFooterHtml
    {
        get { return _firstpagefooterhtml; }
        set { _firstpagefooterhtml = value; }
    }
    public string HeaderHtml
    {
        get { return _headerhtml; }
        set { _headerhtml = value; }
    }
    public string Html
    {
        get { return _html; }
        set { _html = value; }
    }
    public string FirstPageFooterLeft
    {
        get { return _firstpagefooterleft; }
        set { _firstpagefooterleft = value; }
    }
    public string FirstPageFooterRight
    {
        get { return _firstpagefooterright; }
        set { _firstpagefooterright = value; }
    }
    public string FirstPageFooterMiddle
    {
        get { return _firstpagefootermiddle; }
        set { _firstpagefootermiddle = value; }
    }
    public string FooterLeft
    {
        get { return _footerleft; }
        set { _footerleft = value; }
    }
    public string FooterRight
    {
        get { return _footerright; }
        set { _footerright = value; }
    }
    public string FooterMiddle
    {
        get { return _footermiddle; }
        set { _footermiddle = value; }
    }
    Rectangle _Size;
    public Rectangle Size
    {
        get { return _Size; }
        set { _Size = value; }
    }
    string _font;
    public string Font
    {
        get { return _font; }
        set { _font = value; }
    }
    public string FooterLeftText
    {
        get { return _footerlefttext; }
        set { _footerlefttext = value; }
    }//
    public string FooterCenterText
    {
        get { return _footercentertext; }
        set { _footercentertext = value; }
    }

    public string FooterRightText
    {
        get { return _footerrighttext; }
        set { _footerrighttext = value; }
    }
    public string LowestFooter
    {
        get { return _lowestfooter; }
        set { _lowestfooter = value; }
    }

    public System.IO.MemoryStream ParsePDF()
    {
        return ParsePDF(false);
    }
    public System.IO.MemoryStream ParsePDF(bool IsHidePaging)
    {
        Document doc = new Document(Size);
        doc.SetMargins(MarginLeft, MarginRight, MarginTop, MarginBottom);
        //iTextSharp.text.html.simpleparser.HTMLWorker parser = new iTextSharp.text.html.simpleparser.HTMLWorker(doc);
        System.IO.MemoryStream m = new System.IO.MemoryStream();
        PdfWriter pdfw = PdfWriter.GetInstance(doc, m);
        pdfw.ViewerPreferences = PdfWriter.PageModeUseOutlines;
        PDFPageEventHandlerSummaryMHC PageEventHandler = new PDFPageEventHandlerSummaryMHC();
        pdfw.PageEvent = PageEventHandler;
        if (IsHidePaging)
        {
            PageEventHandler.IsHidePaging = IsHidePaging;
        }
        PageEventHandler.FirstPageHeaderHTML = FirstPageHeaderHtml;
        PageEventHandler.FirstPageFooterLeft = FirstPageFooterLeft;
        PageEventHandler.FirstPageFooterHTML = FirstPageFooterHtml;
        PageEventHandler.FirstPageFooterMiddle = FirstPageFooterMiddle;
        PageEventHandler.FirstPageFooterRight = FirstPageFooterRight;
        PageEventHandler.HeaderHTML = HeaderHtml;
        //PageEventHandler.FooterLeft = FooterLeft;
        //PageEventHandler.FooterMiddle = FooterMiddle;
        PageEventHandler.FooterRight = FooterRight;
        PageEventHandler.LowestFooter = LowestFooter;

        PageEventHandler.FooterLeftText = FooterLeftText;
        PageEventHandler.FooterCenterText = FooterCenterText;
        PageEventHandler.FooterRightText = FooterRightText;

        String HtmCode = Html.ToString();
        HtmCode = HtmCode.Replace("..", System.Web.HttpContext.Current.Server.MapPath("~"));

        HtmCode = HtmCode.Replace("&#46;&#46;.", System.Web.HttpContext.Current.Server.MapPath("~"));
        HtmCode = HtmCode.Replace("<table style=\"border: 1px solid black; width: 60%;\">", "<table border=\"1\">");
        Html = HtmCode.ToString();

        System.IO.StringReader reader = new System.IO.StringReader(Html);
        List<iTextSharp.text.IElement> objects;
        doc.Open();




        //objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(reader, null);
        iTextSharp.text.html.simpleparser.StyleSheet s = new iTextSharp.text.html.simpleparser.StyleSheet();
        s.LoadTagStyle("Body", "Align", "left");

        s.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Arial Unicode MS");

        s.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
        objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(reader, s);
        doc.Open();


        RegisterFontToItext();

        //  objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(reader, null);
        for (int k = 0; k < objects.Count; k++)
        {
            if (common.myStr(objects[k]) == "iTextSharp.text.pdf.PdfPTable")
            {
                ((iTextSharp.text.pdf.PdfPTable)(objects[k])).HorizontalAlignment = 0;
                ((iTextSharp.text.pdf.PdfPTable)(objects[k])).KeepTogether = false;
                ((iTextSharp.text.pdf.PdfPTable)(objects[k])).SplitLate = false;
            }
            else if (common.myStr(objects[k]) == "iTextSharp.text.List")
            {
                foreach (iTextSharp.text.ListItem lst in ((iTextSharp.text.List)(objects[k])).Items)
                {
                    lst.IndentationLeft = 12;
                }
            }
            doc.Add((IElement)objects[k]);
        }
        doc.Close();
        return m;
    }

    public void RegisterFontToItext()
    {
        string[] strAllFont;
        string strFontpathALL = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts";
        if (Directory.Exists(strFontpathALL))
        {
            strAllFont = Directory.GetFiles(strFontpathALL);

            foreach (string font in strAllFont)
            {
                if (!FontFactory.IsRegistered(font))
                {
                    iTextSharp.text.FontFactory.Register(font);
                }
            }
        }




    }
    public static iTextSharp.text.Font GetTahoma()
    {
        var fontName = "Tahoma";
        if (!FontFactory.IsRegistered(fontName))
        {
            var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf";
            FontFactory.Register(fontPath);
        }
        return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    }

    public Document GetFontName()
    {
        Document doc = new Document();
        //font.FamilyFontName=
        int totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
        StringBuilder sb = new StringBuilder();
        foreach (string fontname in FontFactory.RegisteredFonts)
        {
            sb.Append(fontname + "\n");
        }
        doc.Open();
        doc.Add(new Paragraph(sb.ToString()));
        doc.Close();
        return doc;
    }

}