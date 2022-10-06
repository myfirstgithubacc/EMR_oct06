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


public class PDFPageEventHandlerMHC : PdfPageEventHelper
{
    // This is the contentbyte object of the writer
    PdfContentByte cb;

    // we will put the final number of pages in a template
    PdfTemplate template;

    // this is the BaseFont we are going to use for the header / footer
    BaseFont bf = null;

    // This keeps track of the creation time
    DateTime PrintTime = DateTime.Now;

    #region Properties

    string _LowestFooter;

    bool _IsHidePaging;


    public string LowestFooter
    {
        get { return _LowestFooter; }
        set { _LowestFooter = value; }
    }
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
    string _HeaderHTML;
    public string HeaderHTML
    {
        get { return _HeaderHTML; }
        set { _HeaderHTML = value; }
    }



    string _FooterLeft;
    string _FooterMiddle;
    string _FooterRight;
    string _FirstPageFooterLeft;
    string _FirstPageFooterRight;
    string _FirstPageFooterMiddle;
    string _footerlefttext;
    string _footercentertext;
    string _footerrighttext;

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
    #endregion

    // we override the onOpenDocument method
    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        try
        {
            PrintTime = DateTime.Now;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
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

        try
        {
            if (writer.PageNumber == 1)
            {
                if (FirstPageHeaderHTML != "")
                {


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

                    //List<IElement> objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(reader, null);
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

                }
            }

        }
        catch (Exception Ex)
        {

        }
        finally
        {

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

                switch (FirstPageFooterLeft)
                {
                    case "N":
                        text = pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.SetTextMatrix(pagesize.GetLeft(document.LeftMargin), pagesize.GetBottom(5));
                        cb.ShowText(text);
                        cb.EndText();


                        break;
                    case "NofN":
                        text = pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.SetTextMatrix(pagesize.GetLeft(document.LeftMargin), pagesize.GetBottom(5));
                        cb.ShowText(text);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetLeft(document.LeftMargin) + len, pagesize.GetBottom(5));
                        break;
                    case "PageN":
                        text = "Page " + pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);

                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.SetTextMatrix(pagesize.GetLeft(document.LeftMargin), pagesize.GetBottom(5));
                        cb.ShowText(text);
                        cb.EndText();
                        break;
                    case "PageNofN":
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.SetTextMatrix(pagesize.GetLeft(document.LeftMargin), pagesize.GetBottom(5));
                        cb.ShowText(text);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetLeft(document.LeftMargin) + len, pagesize.GetBottom(5));
                        break;
                    default:
                        break;
                }
                switch (FirstPageFooterMiddle)
                {
                    case "N":
                        text = pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, (pagesize.Width / 2) - (len / 2), pagesize.GetBottom(5), 0);
                        cb.EndText();

                        break;
                    case "NofN":
                        text = pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, (pagesize.Width / 2) - (len / 2), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        cb.AddTemplate(template, (pagesize.Width / 2), pagesize.GetBottom(5));
                        break;
                    case "PageN":
                        text = "Page " + pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, (pagesize.Width / 2) - (len / 2), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        break;
                    case "PageNofN":
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, (pagesize.Width / 2) - (len / 2), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        cb.AddTemplate(template, (pagesize.Width / 2), pagesize.GetBottom(5));
                        break;
                    default:
                        break;
                }

                switch (FirstPageFooterRight)
                {
                    case "N":
                        text = pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5), 0);
                        cb.EndText();

                        break;
                    case "NofN":
                        text = pagen.ToString() + " of";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5));
                        break;
                    case "PageN":
                        text = "Page " + pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        break;
                    case "PageNofN":
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5), 0);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5));
                        break;
                    default:
                        break;
                }

            }
            else
            {
                switch (FooterLeft)
                {
                    case "N":
                        text = pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.SetTextMatrix(pagesize.GetLeft(document.LeftMargin), pagesize.GetBottom(5));
                        cb.ShowText(text);
                        cb.EndText();

                        break;
                    case "NofN":
                        text = pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.SetTextMatrix(pagesize.GetLeft(document.LeftMargin), pagesize.GetBottom(5));
                        cb.ShowText(text);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetLeft(document.LeftMargin) + len, pagesize.GetBottom(5));
                        break;
                    case "PageN":
                        text = "Page " + pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.SetTextMatrix(pagesize.GetLeft(document.LeftMargin), pagesize.GetBottom(5));
                        cb.ShowText(text);
                        cb.EndText();
                        break;
                    case "PageNofN":
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.SetTextMatrix(pagesize.GetLeft(document.LeftMargin), pagesize.GetBottom(5));
                        cb.ShowText(text);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetLeft(document.LeftMargin) + len, pagesize.GetBottom(5));
                        break;
                    default:
                        break;
                }
                switch (FooterMiddle)
                {
                    case "N":
                        text = pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, (pagesize.Width / 2) - (len / 2), pagesize.GetBottom(5), 0);
                        cb.EndText();

                        break;
                    case "NofN":
                        text = pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, (pagesize.Width / 2) - (len / 2), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        cb.AddTemplate(template, (pagesize.Width / 2), pagesize.GetBottom(5));
                        break;
                    case "PageN":
                        text = "Page " + pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, (pagesize.Width / 2) - (len / 2), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        break;
                    case "PageNofN":
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, (pagesize.Width / 2) - (len / 2), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        cb.AddTemplate(template, (pagesize.Width / 2), pagesize.GetBottom(5));
                        break;
                    default:
                        break;
                }
                switch (FooterRight)
                {
                    case "N":
                        text = pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5), 0);
                        cb.EndText();

                        break;
                    case "NofN":
                        text = pagen.ToString() + " of";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5));
                        break;
                    case "PageN":
                        text = "Page " + pagen.ToString();
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5), 0);

                        cb.EndText();
                        break;
                    case "PageNofN":
                        text = "Page " + pagen.ToString() + " of ";
                        len = bf.GetWidthPoint(text, 8);
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 8);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, text, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5), 0);
                        cb.EndText();
                        cb.AddTemplate(template, pagesize.GetRight(document.RightMargin), pagesize.GetBottom(5));
                        break;
                    default:
                        break;
                }
            }
        }
        Font font = new Font(bf, 8);
        Phrase phraseleft = new Phrase(FooterLeftText, font);//
        Phrase phrasemiddle = new Phrase(FooterCenterText, font);
        Phrase phraseright = new Phrase(FooterRightText, font);
        //Phrase phraseright = new Phrase(string.Format("Page {0}", writer.PageNumber), font);



        Rectangle page = document.PageSize;
        PdfPTable footer = new PdfPTable(3);
        footer.TotalWidth = page.Width;

        //left cell
        PdfPCell cL = new PdfPCell(phraseleft);
        cL.Border = Rectangle.NO_BORDER;
        // cL.Border = Rectangle.TOP_BORDER;
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
        //cR.Border = Rectangle.TOP_BORDER;
        cR.VerticalAlignment = Element.ALIGN_CENTER;
        cR.HorizontalAlignment = Element.ALIGN_RIGHT;
        footer.AddCell(cR);

        // row 2
        Phrase phraseleft1 = new Phrase(LowestFooter, font);
        PdfPCell cL1 = new PdfPCell(phraseleft1);
        cL1.Colspan = 3;
        //cL1.Border = Rectangle.TOP_BORDER;
        cL1.PaddingLeft = 10;
        cL1.Border = Rectangle.NO_BORDER;
        cL1.VerticalAlignment = Element.ALIGN_BOTTOM;
        cL1.HorizontalAlignment = Element.ALIGN_LEFT;
        footer.AddCell(cL1);

        footer.WriteSelectedRows(0, -1, 0, footer.TotalHeight + 5, writer.DirectContent);

        //Paragraph paraleft = new Paragraph();
        //Paragraph paramiddle = new Paragraph();
        //Paragraph pararight = new Paragraph();


        //if (pagen == 1)
        //{
        //    switch (FirstPageFooterLeft)
        //    {
        //        case "Page1of1":
        //            paraleft.Add("Page " + pagen.ToString() + " of " + writer.);                    
        //            break;
        //        case "Page1":
        //            paraleft.Add("Page " + pagen.ToString());
        //            break;
        //        case "1":
        //            paraleft.Add(pagen.ToString());
        //            break;
        //        default:
        //            paraleft.Add("");
        //            break;
        //    }

        //    paraleft.Add("Page " + pagen.ToString());
        //    paramiddle.Add("2");
        //    pararight.Add("3");
        //}
        //else
        //{

        //}
        //Phrase phraseleft = new Phrase(paraleft.Chunks[0]);
        //Phrase phrasemiddle = new Phrase(paramiddle.Chunks[0]);
        //Phrase phraseright = new Phrase(pararight.Chunks[0]);


        //Rectangle page = document.PageSize;
        //PdfPTable footer = new PdfPTable(3);
        //footer.TotalWidth = page.Width;
        ////left cell
        //PdfPCell cL = new PdfPCell(phraseleft);
        //cL.Border = Rectangle.NO_BORDER;
        //cL.VerticalAlignment = Element.ALIGN_BOTTOM;
        //cL.HorizontalAlignment = Element.ALIGN_LEFT;
        //footer.AddCell(cL);
        ////middle cell
        //PdfPCell cM = new PdfPCell(phrasemiddle );
        //cM.Border = Rectangle.NO_BORDER;
        //cM.VerticalAlignment = Element.ALIGN_BOTTOM;
        //cM.HorizontalAlignment = Element.ALIGN_CENTER;
        //footer.AddCell(cM);
        ////right cell
        //PdfPCell cR = new PdfPCell(phraseright );
        //cR.Border = Rectangle.NO_BORDER;
        //cR.VerticalAlignment = Element.ALIGN_BOTTOM;
        //cR.HorizontalAlignment = Element.ALIGN_RIGHT;
        //footer.AddCell(cR);

        //footer.WriteSelectedRows(0, -1, 0, footer.TotalHeight + 5, writer.DirectContent);      
    }

    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);

        template.BeginText();
        template.SetFontAndSize(bf, 8);
        template.SetTextMatrix(0, 0);
        template.ShowText(" " + (writer.PageNumber));
        template.EndText();
    }

}
/// <summary>
/// Summary description for clsParsePDFMHC
/// </summary>
public class clsParsePDFMHC
{
    public clsParsePDFMHC()
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
    private string _headerhtml;
    private string _html;
    private string _firstpagefooterleft;
    private string _firstpagefooterright;
    private string _firstpagefootermiddle;
    private string _footerleft;
    private string _footerright;
    private string _footermiddle;
    private string _lowestfooter;
    private string _footerlefttext;
    private string _footercentertext;
    private string _footerrighttext;

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

    public System.IO.MemoryStream ParsePDF()
    {
        return ParsePDF(false);
    }

    public System.IO.MemoryStream ParsePDF(bool IsHidePaging)
    {
        Document doc = new Document(Size);
        //Document doc1=new Document(Font)

        doc.SetMargins(MarginLeft, MarginRight, MarginTop, MarginBottom);



        //iTextSharp.text.html.simpleparser.HTMLWorker parser = new iTextSharp.text.html.simpleparser.HTMLWorker(doc);
        System.IO.MemoryStream m = new System.IO.MemoryStream();
        PdfWriter pdfw = PdfWriter.GetInstance(doc, m);

        pdfw.ViewerPreferences = PdfWriter.PageModeUseOutlines;

        PDFPageEventHandlerMHC PageEventHandler = new PDFPageEventHandlerMHC();
        pdfw.PageEvent = PageEventHandler;
        if (IsHidePaging)
        {
            PageEventHandler.IsHidePaging = IsHidePaging;
        }

        PageEventHandler.FirstPageHeaderHTML = FirstPageHeaderHtml;
        PageEventHandler.FirstPageFooterLeft = FirstPageFooterLeft;
        PageEventHandler.FirstPageFooterMiddle = FirstPageFooterMiddle;
        PageEventHandler.FirstPageFooterRight = FirstPageFooterRight;
        PageEventHandler.HeaderHTML = HeaderHtml;
        PageEventHandler.FooterLeft = FooterLeft;
        PageEventHandler.FooterMiddle = FooterMiddle;
        PageEventHandler.FooterRight = FooterRight;
        PageEventHandler.LowestFooter = LowestFooter;
        PageEventHandler.FooterLeftText = FooterLeftText;
        PageEventHandler.FooterCenterText = FooterCenterText;
        PageEventHandler.FooterRightText = FooterRightText;


        String HtmCode = Html.ToString();
        HtmCode = HtmCode.Replace("..", "&#46;&#46;");
        HtmCode = HtmCode.Replace("'/Images/TemplateImage", "'../Images/TemplateImage");
        HtmCode = HtmCode.Replace("..", System.Web.HttpContext.Current.Server.MapPath("~"));
        Html = HtmCode.ToString();

        if (common.myLen(System.Web.HttpContext.Current.Session["EMRAllowLanguageTranslation"]).Equals(0))
        {
            string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
            System.Web.HttpContext.Current.Session["EMRAllowLanguageTranslation"] = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
                                              common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "InEMRAllowLanguageTranslation", sConString);
        }

        if (common.myStr(System.Web.HttpContext.Current.Session["EMRAllowLanguageTranslation"]).ToUpper().Equals("Y"))
        {
            if (common.ContainsUnicodeCharacter(Html))
            {
                Html = Html.Replace("Arial", "Arial Unicode MS");
                Html = Html.Replace("arial", "Arial Unicode MS");

                Html = Html.Replace("Candara", "Arial Unicode MS");
                Html = Html.Replace("candara", "Arial Unicode MS");

                Html = Html.Replace("Courier New", "Arial Unicode MS");
                Html = Html.Replace("courier new", "Arial Unicode MS");

                Html = Html.Replace("Garamond", "Arial Unicode MS");
                Html = Html.Replace("garamond", "Arial Unicode MS");

                Html = Html.Replace("Georgia", "Arial Unicode MS");
                Html = Html.Replace("georgia", "Arial Unicode MS");

                Html = Html.Replace("MS Sans Serif", "Arial Unicode MS");
                Html = Html.Replace("ms sans serif", "Arial Unicode MS");

                Html = Html.Replace("Segoe UI", "Arial Unicode MS");
                Html = Html.Replace("segoe ui", "Arial Unicode MS");

                Html = Html.Replace("Tahoma", "Arial Unicode MS");
                Html = Html.Replace("tahoma", "Arial Unicode MS");

                Html = Html.Replace("Times New Roman", "Arial Unicode MS");
                Html = Html.Replace("times new roman", "Arial Unicode MS");

                Html = Html.Replace("Trebuchet MS", "Arial Unicode MS");
                Html = Html.Replace("trebuchet ms", "Arial Unicode MS");

                Html = Html.Replace("Verdana", "Arial Unicode MS");
                Html = Html.Replace("verdana", "Arial Unicode MS");
            }
        }

        System.IO.StringReader reader = new System.IO.StringReader(Html);
        List<iTextSharp.text.IElement> objects;
        //Mahendra check it
        iTextSharp.text.html.simpleparser.StyleSheet s = new iTextSharp.text.html.simpleparser.StyleSheet();
        s.LoadTagStyle("Body", "Align", "left");

        s.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Arial Unicode MS");

        s.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
        objects = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(reader, s);

        doc.Open();

        for (int k = 0; k < objects.Count; k++)
        {
            if (common.myStr(objects[k]) == "iTextSharp.text.pdf.PdfPTable")
            {
                ((iTextSharp.text.pdf.PdfPTable)(objects[k])).HorizontalAlignment = 0;
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
