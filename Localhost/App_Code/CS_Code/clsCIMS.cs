using System;
using System.Data;
using System.Web;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

/// <summary>
/// Summary description for clsCIMS
/// </summary>
public class clsCIMS
{
    public clsCIMS()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string getFastTrack5Output(string CIMSXML)
    {
        string outputValues = string.Empty;
        try
        {
            string queryString = CIMSXML;

            string CIMSDatabasePath = common.myStr(HttpContext.Current.Session["CIMSDatabasePath"]);
            string CIMSDatabasePassword = common.myStr(HttpContext.Current.Session["CIMSDatabasePassword"]);
            string CIMSDatabaseName = common.myStr(HttpContext.Current.Session["CIMSDatabaseName"]);

            if (common.myLen(CIMSDatabaseName).Equals(0))
            {
                CIMSDatabaseName = "FastTrackData.mrc";
            }

            //Monograph
            //string queryString = "<Request><Content><Product reference=\"{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}\" /></Content></Request>";

            //Interaction
            //string queryString = "<Request><Interaction><Prescribing><Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" /></Prescribing><Prescribed><Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" /><Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" /></Prescribed><Allergies /><References/></Interaction></Request>";

            string retultInfo = string.Empty;
            string guid = string.Empty;

            //string dataPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) +
            //                    "CIMSDatabase\\FastTrackData.mrc";

            //string CIMSDatabasePassword = "GDDBDEMO";
            //string CIMSDatabasePassword = "5ksQ7b49AR";

            string initString = string.Empty;

            if (File.Exists(CIMSDatabasePath + CIMSDatabaseName))
            {
                initString = "<Initialize><DataFile password='" + CIMSDatabasePassword + "' path='" + CIMSDatabasePath + CIMSDatabaseName + "' /></Initialize>";
            }

            FastTrack5.FastTrack_Creator ftCreator = new FastTrack5.FastTrack_Creator();
            FastTrack5.IFastTrack_Server ftServer;

            ftServer = ftCreator.CreateServer(initString, out retultInfo, out guid);

            outputValues = ftServer.RequestXML(queryString, out retultInfo);
        }
        catch
        {
        }
        return outputValues;
    }

    public string getCIMSFinalOutupt(bool IsBrandDetails)
    {
        string strOutput = string.Empty;
        string queryString = common.myStr(HttpContext.Current.Session["CIMSXMLInputData"]);
        //"<Request><Detail><Product reference=\"{864EBD9F-9789-4451-908A-C6A354E9FA24}\"><Items /><Packages /><Images /><TherapeuticClasses /><ATCCodes /><Companies /><Identifiers /></Product></Detail></Request>";
        //"<Request><Detail><Product reference=\"{F2076849-F5C2-40C1-A3F9-361BA4BFA3C1}\"><Items /><Packages /><Images /><TherapeuticClasses /><ATCCodes /><Companies /><Identifiers /></Product></Detail></Request>";

        //"<Request><Interaction><Prescribing><Product reference=\"{937B03D2-E204-477A-B28B-27DF7E6936DF}\" /><Product reference=\"{3F8099B1-E1EA-44D4-84F5-1CE1CE41E0DF}\" /><Product reference=\"{264CC195-2EEA-44A1-B9DF-B074561393B3}\" /><Product reference=\"{9AEDF043-39EC-468F-A94B-0FCFC81DE1FF}\" /></Prescribing><Allergies><Molecule reference=\"{68EC41A6-71EE-427C-B261-03425BAA3DD3}\" /></Allergies><HealthIssueCodes><HealthIssueCode code=\"J45\" codeType=\"ICD10\" /></HealthIssueCodes><References/></Interaction></Request>";

        string outputValues = string.Empty;

        outputValues = getFastTrack5Output(queryString);

        if (IsBrandDetails)
        {
            //this.Title = "Brand Details";

            if (outputValues != null)
            {
                //divData.InnerHtml = getBrandDetailsOutput(outputValues);

                //strOutput = "<html><head><title>Powered by CIMS. Copyright MIMS Pte Ltd. All rights reserved.</title></head><body>" + getBrandDetailsOutput(outputValues) + "</body></html>";
                strOutput = getBrandDetailsOutput(outputValues);
            }
        }
        else
        {
            string PathXSL = string.Empty;
            if (queryString.Contains("<Interaction>") && queryString.Contains("</Interaction>"))
            {
                PathXSL = common.myStr(HttpContext.Current.Session["CIMSDatabasePath"]) + "Monograph-Interaction-CDSDefault.xsl"; //"ResultData.xsl";
                //this.Title = "Interaction";
            }
            else
            {
                PathXSL = common.myStr(HttpContext.Current.Session["CIMSDatabasePath"]) + "Monograph-Interaction-CDSDefault.xsl"; //"GM-Presentation.xsl";
            }

            //System.IO.StreamReader xmlFile = new System.IO.StreamReader(PathXSL);
            //string xmlString = xmlFile.ReadToEnd();
            //xmlFile.Close();

            if (outputValues != null)
            {
                //divData.InnerHtml = XsltTransform(outputValues, PathXSL);

                //strOutput = "<html><head><title>Powered by CIMS. Copyright MIMS Pte Ltd. All rights reserved.</title></head><body>" + XsltTransform(outputValues, PathXSL) + "</body></html>";
                strOutput = XsltTransform(outputValues, PathXSL);
            }
        }

        return strOutput;
    }

    public string XsltTransform(string srcXml, string xslPath)
    {
        string transformedXml = null;
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(srcXml));
        XPathDocument xmlDoc = new XPathDocument(stream);
        XslCompiledTransform xslt = new XslCompiledTransform();

        XsltSettings settings = new XsltSettings(true, true);

        XmlTextReader xslDoc = new XmlTextReader(xslPath);
        xslt.Load(xslDoc, settings, new XmlUrlResolver());

        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);

        xslt.Transform(xmlDoc, null, sw);
        transformedXml = sb.ToString();

        xslDoc.Close();
        return transformedXml;
    }

    public string getBrandDetailsOutput(string srcXml)
    {
        string strTableStart = "<table border='0' cellpadding='2' cellspacing='0'>";
        string strTableEnd = "</table>";
        string strTrStart = "<tr>";
        string strTrEnd = "</tr>";
        string strTdStart = "<td valign='top'>";
        string strTdEnd = "</td>";

        StringBuilder sbFinal = new StringBuilder();

        StringBuilder sbProduct = new StringBuilder();
        StringBuilder sbForm = new StringBuilder();
        StringBuilder sbMolecule = new StringBuilder();
        StringBuilder sbTherapeuticClass = new StringBuilder();
        StringBuilder sbATCCode = new StringBuilder();
        StringBuilder sbCompany = new StringBuilder();
        StringBuilder sbCompanyType = new StringBuilder();
        StringBuilder sbPackage = new StringBuilder();
        StringBuilder sbPrice = new StringBuilder();

        StringBuilder sbBatch = new StringBuilder();
        DataSet dsBatch = new DataSet();

        try
        {
            if (common.myLen(srcXml) > 0)
            {
                sbBatch.Append(common.myStr(srcXml));
                StringReader srBatch = new StringReader(sbBatch.ToString());
                dsBatch.ReadXml(srBatch);

                //Product->name
                //Form->name
                //Molecule-> name|strength|strengthUnit
                //TherapeuticClass->name
                //ATCCode->name|code
                //Company->name| CompanyType->name
                //Package->Packing|Price->Price_Text

                foreach (DataTable tbl in dsBatch.Tables)
                {
                    switch (tbl.TableName)
                    {
                        case "Product":
                            if (tbl.Columns.Contains("name"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbProduct.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%' style='background: #7C9CA6; color:White; font-weight:bold; border-style: solid; border-width: 1px; border-color: #ADD8E6;'>");
                                    sbProduct.Append(strTrStart);
                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbProduct.Append("<td align='center'>");
                                        sbProduct.Append(common.myStr(DR["name"]).Trim());
                                        sbProduct.Append(strTdEnd);

                                        break;
                                    }
                                    sbProduct.Append(strTrEnd);
                                    sbProduct.Append(strTableEnd);
                                }
                            }
                            break;
                        case "Form":
                            if (tbl.Columns.Contains("name"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbForm.Append(strTableStart);
                                    sbForm.Append(strTrStart);
                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbForm.Append(strTdStart);
                                        sbForm.Append(common.myStr(DR["name"]).Trim());
                                        sbForm.Append(strTdEnd);

                                        break;
                                    }
                                    sbForm.Append(strTrEnd);
                                    sbForm.Append(strTableEnd);
                                }
                            }
                            break;
                        case "Molecule":
                            if (tbl.Columns.Contains("name") && tbl.Columns.Contains("strength") && tbl.Columns.Contains("strengthUnit"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbMolecule.Append("<table border='0' cellpadding='2' cellspacing='0' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                    sbMolecule.Append(strTrStart);

                                    sbMolecule.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbMolecule.Append("Name");
                                    sbMolecule.Append(strTdEnd);

                                    sbMolecule.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbMolecule.Append("Strength");
                                    sbMolecule.Append(strTdEnd);

                                    sbMolecule.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbMolecule.Append("Unit");
                                    sbMolecule.Append(strTdEnd);

                                    sbMolecule.Append(strTrEnd);

                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbMolecule.Append(strTrStart);

                                        sbMolecule.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbMolecule.Append(common.myStr(DR["name"]).Trim());
                                        sbMolecule.Append(strTdEnd);

                                        sbMolecule.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbMolecule.Append(common.myStr(DR["strength"]).Trim());
                                        sbMolecule.Append(strTdEnd);

                                        sbMolecule.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbMolecule.Append(common.myStr(DR["strengthUnit"]).Trim());
                                        sbMolecule.Append(strTdEnd);

                                        sbMolecule.Append(strTrEnd);
                                    }

                                    sbMolecule.Append(strTableEnd);
                                }
                            }
                            break;
                        case "TherapeuticClass":
                            if (tbl.Columns.Contains("name"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbTherapeuticClass.Append("<ul>");

                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbTherapeuticClass.Append("<li>");
                                        sbTherapeuticClass.Append(common.myStr(DR["name"]).Trim());
                                        sbTherapeuticClass.Append("</li>");
                                    }

                                    sbTherapeuticClass.Append("</ul>");
                                }
                            }
                            break;
                        case "ATCCode":
                            if (tbl.Columns.Contains("name") && tbl.Columns.Contains("code"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbATCCode.Append("<table border='0' cellpadding='2' cellspacing='0' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                    sbATCCode.Append(strTrStart);

                                    sbATCCode.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbATCCode.Append("Name");
                                    sbATCCode.Append(strTdEnd);

                                    sbATCCode.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbATCCode.Append("Code");
                                    sbATCCode.Append(strTdEnd);

                                    sbATCCode.Append(strTrEnd);

                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbATCCode.Append(strTrStart);

                                        sbATCCode.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbATCCode.Append(common.myStr(DR["name"]).Trim());
                                        sbATCCode.Append(strTdEnd);

                                        sbATCCode.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbATCCode.Append(common.myStr(DR["code"]).Trim());
                                        sbATCCode.Append(strTdEnd);

                                        sbATCCode.Append(strTrEnd);
                                    }

                                    sbATCCode.Append(strTableEnd);
                                }
                            }
                            break;
                        case "Company":
                            if (tbl.Columns.Contains("name"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbCompany.Append("<table border='0' cellpadding='2' cellspacing='0' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                    sbCompany.Append(strTrStart);

                                    sbCompany.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbCompany.Append("Name");
                                    sbCompany.Append(strTdEnd);

                                    sbCompany.Append(strTrEnd);

                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbCompany.Append(strTrStart);

                                        sbCompany.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbCompany.Append(common.myStr(DR["name"]).Trim());
                                        sbCompany.Append(strTdEnd);

                                        sbCompany.Append(strTrEnd);
                                    }

                                    sbCompany.Append(strTableEnd);
                                }
                            }
                            break;
                        case "CompanyType":
                            if (tbl.Columns.Contains("name"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbCompanyType.Append("<table border='0' cellpadding='2' cellspacing='0' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                    sbCompanyType.Append(strTrStart);

                                    sbCompanyType.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbCompanyType.Append("Type");
                                    sbCompanyType.Append(strTdEnd);

                                    sbCompanyType.Append(strTrEnd);

                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbCompanyType.Append(strTrStart);

                                        sbCompanyType.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbCompanyType.Append(common.myStr(DR["name"]).Trim());
                                        sbCompanyType.Append(strTdEnd);

                                        sbCompanyType.Append(strTrEnd);
                                    }

                                    sbCompanyType.Append(strTableEnd);
                                }
                            }
                            break;
                        case "Package":
                            if (tbl.Columns.Contains("Packing"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbPackage.Append("<table border='0' cellpadding='2' cellspacing='0' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                    sbPackage.Append(strTrStart);

                                    sbPackage.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbPackage.Append("Packing");
                                    sbPackage.Append(strTdEnd);

                                    sbPackage.Append(strTrEnd);

                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbPackage.Append(strTrStart);

                                        sbPackage.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbPackage.Append(common.myStr(DR["Packing"]).Trim());
                                        sbPackage.Append(strTdEnd);

                                        sbPackage.Append(strTrEnd);
                                    }

                                    sbPackage.Append(strTableEnd);
                                }
                            }
                            break;
                        case "Price":
                            if (tbl.Columns.Contains("Price_Text"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    sbPrice.Append("<table border='0' cellpadding='2' cellspacing='0' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                    sbPrice.Append(strTrStart);

                                    sbPrice.Append("<td valign='top' style='background: #D7ECF3;'>");
                                    sbPrice.Append("Price (INR)");
                                    sbPrice.Append(strTdEnd);

                                    sbPrice.Append(strTrEnd);

                                    foreach (DataRow DR in tbl.Rows)
                                    {
                                        sbPrice.Append(strTrStart);

                                        sbPrice.Append("<td valign='top' style='border-style: solid; border-width: 1px; border-color: #C2E0FF;'>");
                                        sbPrice.Append(common.myStr(DR["Price_Text"]).Trim());
                                        sbPrice.Append(strTdEnd);

                                        sbPrice.Append(strTrEnd);
                                    }

                                    sbPrice.Append(strTableEnd);
                                }
                            }
                            break;
                    }
                }

                //                            Brand Name: ACENAC-P TA
                //Dosage Form: TAB
                //Molecule Composition:   Name|Strength|Unit

                //Therapeutic Class:

                //ATC Code: Name| Code

                //Company: Name | Type

                //Package: Packing|Price



                sbFinal.Append("<table border='0' cellpadding='0' cellspacing='2' width='100%' style='background: #EFF7FF; border-style: solid; border-width: 1px; border-color: #ADD8E6;'>");

                //Product
                sbFinal.Append(strTrStart);
                sbFinal.Append("<td valign='top' colspan='2' align='center'>");
                sbFinal.Append(sbProduct.ToString());
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTrEnd);

                //Form
                sbFinal.Append(strTrStart);

                sbFinal.Append("<td valign='top' style='background: #ADD8E6; width: 170px;'>");
                sbFinal.Append("&nbsp;Dosage Form");
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTdStart);
                sbFinal.Append(sbForm.ToString());
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTrEnd);

                //Molecule
                sbFinal.Append(strTrStart);
                sbFinal.Append("<td valign='top' style='background: #ADD8E6;'>");
                sbFinal.Append("&nbsp;Molecule Composition");
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTdStart);
                sbFinal.Append(sbMolecule.ToString());
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTrEnd);

                //TherapeuticClass
                sbFinal.Append(strTrStart);
                sbFinal.Append("<td valign='top' style='background: #ADD8E6;'>");
                sbFinal.Append("&nbsp;Therapeutic Class");
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTdStart);
                sbFinal.Append(sbTherapeuticClass.ToString());
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTrEnd);

                //ATCCode
                sbFinal.Append(strTrStart);
                sbFinal.Append("<td valign='top' style='background: #ADD8E6;'>");
                sbFinal.Append("&nbsp;ATC Code");
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTdStart);
                sbFinal.Append(sbATCCode.ToString());
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTrEnd);

                //Company
                sbFinal.Append(strTrStart);

                sbFinal.Append("<td valign='top' style='background: #ADD8E6;'>");
                sbFinal.Append("&nbsp;Company");
                sbFinal.Append(strTdEnd);

                sbFinal.Append(strTdStart);
                sbFinal.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                sbFinal.Append(strTrStart);
                sbFinal.Append(strTdStart);
                sbFinal.Append(sbCompany);
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTdStart);
                sbFinal.Append(sbCompanyType);
                sbFinal.Append(strTdEnd);

                sbFinal.Append(strTrEnd);
                sbFinal.Append(strTableEnd);

                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTrEnd);

                //Package
                sbFinal.Append(strTrStart);

                sbFinal.Append("<td valign='top' style='background: #ADD8E6;'>");
                sbFinal.Append("&nbsp;Package");
                sbFinal.Append(strTdEnd);

                sbFinal.Append(strTdStart);
                sbFinal.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                sbFinal.Append(strTrStart);
                sbFinal.Append(strTdStart);
                sbFinal.Append(sbPackage);
                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTdStart);
                sbFinal.Append(sbPrice);
                sbFinal.Append(strTdEnd);

                sbFinal.Append(strTrEnd);
                sbFinal.Append(strTableEnd);

                sbFinal.Append(strTdEnd);
                sbFinal.Append(strTrEnd);

                sbFinal.Append(strTableEnd);
            }
        }
        catch
        {
        }
        finally
        {
            sbProduct = null;
            sbForm = null;
            sbMolecule = null;
            sbTherapeuticClass = null;
            sbATCCode = null;
            sbCompany = null;
            sbCompanyType = null;
            sbPackage = null;
            sbPrice = null;

            sbBatch = null;
            dsBatch.Dispose();
        }

        return sbFinal.ToString();
    }
    public double IsDuplicateInteractionFound(string strCIMSItemId, string outputValues, bool IsSevereOnly)
    {
        double dblOutput = 0;

        StringBuilder sbReferences = new StringBuilder();

        StringBuilder sbBatch = new StringBuilder();
        DataSet dsBatch = new DataSet();

        try
        {
            if (common.myLen(strCIMSItemId) > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(outputValues);

                var nav = doc.CreateNavigator();


                string query = string.Empty;
                if (IsSevereOnly)
                {
                    //count (//Interaction/*[@reference='{35051B69-F816-49D2-8A1A-1DA953855A0E}' and not(@Mirror)]/Route/*/Route/ClassInteraction[Severity="Severe"]|//Interaction/*/Route/*[@reference='{35051B69-F816-49D2-8A1A-1DA953855A0E}' and not(@Mirror)]/Route/ClassInteraction[Severity="Severe"])
                    query = "count(//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/*/Route/ClassInteraction[Severity=\"Severe\"]|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/ClassInteraction[Severity=\"Severe\"])";
                }
                else
                {
                    //count(//Interaction/*[@reference='{864EBD9F-9789-4451-908A-C6A354E9FA24}' and not(@Mirror)]/Route/*/Route/ClassInteraction|//Interaction/*/Route/*[@reference='{864EBD9F-9789-4451-908A-C6A354E9FA24}' and not(@Mirror)]/Route/ClassInteraction)
                    query = "count(//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/*/Route/ClassInteraction|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/ClassInteraction)";
                }

                var expr = nav.Compile(query);

                switch (expr.ReturnType)
                {
                    case XPathResultType.Number:
                        dblOutput = common.myDbl(nav.Evaluate(query));
                        break;

                        //case XPathResultType.String:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.Boolean:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.NodeSet:
                        //    var nodes = nav.Select(expr);
                        //    while (nodes.MoveNext())
                        //    {
                        //        if (nodes.Current != null && !nodes.Current.HasChildren) continue;
                        //        if (nodes.Current == null) continue;
                        //        var childIter = nodes.Current.SelectChildren(XPathNodeType.Element);
                        //        while (childIter.MoveNext())
                        //        {
                        //            if (childIter.Current != null) Console.WriteLine(childIter.Current.Value);
                        //        }
                        //    }
                        //    break;

                        //case XPathResultType.Any:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //default:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;
                }
            }
        }
        catch
        {
        }
        return dblOutput;
    }

    public double IsDrugToAllInteractionFound(string strCIMSItemId, string outputValues, bool IsSevereOnly)
    {
        double dblOutput = 0;

        StringBuilder sbReferences = new StringBuilder();

        StringBuilder sbBatch = new StringBuilder();
        DataSet dsBatch = new DataSet();

        try
        {
            if (common.myLen(strCIMSItemId) > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(outputValues);

                var nav = doc.CreateNavigator();

                //count(//Interaction/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}' and not(@Mirror)]/Route/*/Route/ClassInteraction | //Interaction/*/Route/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}' and not(@Mirror)]/Route/ClassInteraction |
                //Interaction/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}'and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction | //Interaction/*/Route/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}' and not (@Mirror)]/Route/HealthIssueCode/ClassInteraction | 
                //Interaction/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}'and not(@Mirror)]/Allergy |
                //Interaction/DuplicateTherapy/Warning/Duplicate/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}'] | 
                //Interaction/DuplicateIngredient/Warning/Molecule/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}'] | 
                //Interaction/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}'and not(@Mirror)]/Route/Pregnancy| //Interaction/*/Route/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}' and not(@Mirror)]/Route/Pregnancy | 
                //Interaction/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}'and not(@Mirror)]/Route/Lactation|//Interaction/*/Route/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}' and not(@Mirror)]/Route/Lactation | 
                //Interaction/DoseCheck/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}'] | 
                //Interaction/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}' and not(@Mirror)]/Route/WOCBA|//Interaction/*/Route/*[@reference='{B3E81726-C768-05D4-E034-080020E1DD8C}' and not(@Mirror)]/Route/WOCBA
                //)

                StringBuilder query = new StringBuilder();
                query.Append("count(");

                if (IsSevereOnly)
                {
                    //DrugToDrugInteraction
                    query.Append("//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/*/Route/ClassInteraction/Severity[@name='Severe' or @name='Moderate']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/ClassInteraction/Severity[@name='Severe' or @name='Moderate']");

                    //DrugToHealthInteraction
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction/Severity[@name='Contraindicated' or @name='Extreme Caution']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction/Severity[@name='Contraindicated' or @name='Extreme Caution']");

                    //Allergy
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Allergy");

                    //Duplicate
                    query.Append("|//Interaction/DuplicateTherapy/Warning[@Level='1']/Duplicate/*[@reference='" + strCIMSItemId + "']|//Interaction/DuplicateIngredient/Warning[@Level='1' or @Level='2' or @Level='3']/Molecule/*[@reference='" + strCIMSItemId + "']");

                    //Pregnancy
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/Pregnancy/Category[@name='A' or @name='B' or @name='C' or @name='D' or @name='X' or @name='+']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/Pregnancy/Category[@name='A' or @name='B' or @name='C' or @name='D' or @name='X' or @name='+']");

                    //Lactation
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/Lactation/Severity[@name='Contraindicated' or @name='Avoid if possible' or @name='Caution']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/Lactation/Severity[@name='Contraindicated' or @name='Avoid if possible' or @name='Caution']");

                    //DoseCheck
                    //query.Append("|//Interaction/DoseCheck/*[@reference='" + strCIMSItemId + "']");

                    //WOCBA
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/WOCBA/Category[@name='A' or @name='B' or @name='C' or @name='D' or @name='X' or @name='+']|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/WOCBA/Category[@name='A' or @name='B' or @name='C' or @name='D' or @name='X' or @name='+']");

                }
                else
                {
                    //DrugToDrugInteraction
                    query.Append("//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/*/Route/ClassInteraction|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/ClassInteraction");

                    //DrugToHealthInteraction
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction");

                    //Allergy
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Allergy");

                    //Duplicate
                    query.Append("|//Interaction/DuplicateTherapy/Warning/Duplicate/*[@reference='" + strCIMSItemId + "']|//Interaction/DuplicateIngredient/Warning/Molecule/*[@reference='" + strCIMSItemId + "']");

                    //Pregnancy
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/Pregnancy|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/Pregnancy");

                    //Lactation
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/Lactation|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/Lactation");

                    //DoseCheck
                    //query.Append("|//Interaction/DoseCheck/*[@reference='" + strCIMSItemId + "']");

                    //WOCBA
                    query.Append("|//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/WOCBA|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/WOCBA");
                }

                query.Append(")");

                var expr = nav.Compile(query.ToString());

                switch (expr.ReturnType)
                {
                    case XPathResultType.Number:
                        dblOutput = common.myDbl(nav.Evaluate(query.ToString()));
                        break;

                        //case XPathResultType.String:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.Boolean:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.NodeSet:
                        //    var nodes = nav.Select(expr);
                        //    while (nodes.MoveNext())
                        //    {
                        //        if (nodes.Current != null && !nodes.Current.HasChildren) continue;
                        //        if (nodes.Current == null) continue;
                        //        var childIter = nodes.Current.SelectChildren(XPathNodeType.Element);
                        //        while (childIter.MoveNext())
                        //        {
                        //            if (childIter.Current != null) Console.WriteLine(childIter.Current.Value);
                        //        }
                        //    }
                        //    break;

                        //case XPathResultType.Any:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //default:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;
                }
            }
        }
        catch
        {
        }
        return dblOutput;
    }

    public double IsDrugToDrugInteractionFound(string strCIMSItemId, string outputValues, bool IsSevereOnly)
    {
        double dblOutput = 0;

        StringBuilder sbReferences = new StringBuilder();

        StringBuilder sbBatch = new StringBuilder();
        DataSet dsBatch = new DataSet();

        try
        {
            if (common.myLen(strCIMSItemId) > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(outputValues);

                var nav = doc.CreateNavigator();


                string query = string.Empty;
                if (IsSevereOnly)
                {
                    //count (//Interaction/*[@reference='{35051B69-F816-49D2-8A1A-1DA953855A0E}' and not(@Mirror)]/Route/*/Route/ClassInteraction[Severity="Severe"]|//Interaction/*/Route/*[@reference='{35051B69-F816-49D2-8A1A-1DA953855A0E}' and not(@Mirror)]/Route/ClassInteraction[Severity="Severe"])
                    query = "count(//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/*/Route/ClassInteraction[Severity=\"Severe\"]|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/ClassInteraction[Severity=\"Severe\"])";
                }
                else
                {
                    //count(//Interaction/*[@reference='{864EBD9F-9789-4451-908A-C6A354E9FA24}' and not(@Mirror)]/Route/*/Route/ClassInteraction|//Interaction/*/Route/*[@reference='{864EBD9F-9789-4451-908A-C6A354E9FA24}' and not(@Mirror)]/Route/ClassInteraction)
                    query = "count(//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/*/Route/ClassInteraction|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/ClassInteraction)";
                }

                var expr = nav.Compile(query);

                switch (expr.ReturnType)
                {
                    case XPathResultType.Number:
                        dblOutput = common.myDbl(nav.Evaluate(query));
                        break;

                        //case XPathResultType.String:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.Boolean:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.NodeSet:
                        //    var nodes = nav.Select(expr);
                        //    while (nodes.MoveNext())
                        //    {
                        //        if (nodes.Current != null && !nodes.Current.HasChildren) continue;
                        //        if (nodes.Current == null) continue;
                        //        var childIter = nodes.Current.SelectChildren(XPathNodeType.Element);
                        //        while (childIter.MoveNext())
                        //        {
                        //            if (childIter.Current != null) Console.WriteLine(childIter.Current.Value);
                        //        }
                        //    }
                        //    break;

                        //case XPathResultType.Any:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //default:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;
                }
            }
        }
        catch
        {
        }
        return dblOutput;
    }

    public double IsDrugToHealthInteractionFound(string strCIMSItemId, string outputValues)
    {
        double dblOutput = 0;

        StringBuilder sbReferences = new StringBuilder();

        StringBuilder sbBatch = new StringBuilder();
        DataSet dsBatch = new DataSet();

        try
        {
            if (common.myLen(strCIMSItemId) > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(outputValues);

                var nav = doc.CreateNavigator();

                //count(//Interaction/*[@reference='{35051B69-F816-49D2-8A1A-1DA953855A0E}' and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction|//Interaction/*/Route/*[@reference='{35051B69-F816-49D2-8A1A-1DA953855A0E}' and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction)

                string query = "count(//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction|//Interaction/*/Route/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Route/HealthIssueCode/ClassInteraction)";

                var expr = nav.Compile(query);

                switch (expr.ReturnType)
                {
                    case XPathResultType.Number:
                        dblOutput = common.myDbl(nav.Evaluate(query));
                        break;

                        //case XPathResultType.String:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.Boolean:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.NodeSet:
                        //    var nodes = nav.Select(expr);
                        //    while (nodes.MoveNext())
                        //    {
                        //        if (nodes.Current != null && !nodes.Current.HasChildren) continue;
                        //        if (nodes.Current == null) continue;
                        //        var childIter = nodes.Current.SelectChildren(XPathNodeType.Element);
                        //        while (childIter.MoveNext())
                        //        {
                        //            if (childIter.Current != null) Console.WriteLine(childIter.Current.Value);
                        //        }
                        //    }
                        //    break;

                        //case XPathResultType.Any:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //default:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;
                }
            }
        }
        catch
        {
        }
        return dblOutput;
    }

    public double IsDrugToAllergyInteractionFound(string strCIMSItemId, string outputValues)
    {
        double dblOutput = 0;

        StringBuilder sbReferences = new StringBuilder();

        StringBuilder sbBatch = new StringBuilder();
        DataSet dsBatch = new DataSet();

        try
        {
            if (common.myLen(strCIMSItemId) > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(outputValues);

                var nav = doc.CreateNavigator();

                //count(//Interaction/*[@reference='{673FE2DD-4B75-4EFA-85DA-7957A90AF7A2}' and not(@Mirror)]/Allergy)
                string query = "count(//Interaction/*[@reference='" + strCIMSItemId + "' and not(@Mirror)]/Allergy)";

                var expr = nav.Compile(query);

                switch (expr.ReturnType)
                {
                    case XPathResultType.Number:
                        dblOutput = common.myDbl(nav.Evaluate(query));
                        break;

                        //case XPathResultType.String:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.Boolean:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //case XPathResultType.NodeSet:
                        //    var nodes = nav.Select(expr);
                        //    while (nodes.MoveNext())
                        //    {
                        //        if (nodes.Current != null && !nodes.Current.HasChildren) continue;
                        //        if (nodes.Current == null) continue;
                        //        var childIter = nodes.Current.SelectChildren(XPathNodeType.Element);
                        //        while (childIter.MoveNext())
                        //        {
                        //            if (childIter.Current != null) Console.WriteLine(childIter.Current.Value);
                        //        }
                        //    }
                        //    break;

                        //case XPathResultType.Any:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;

                        //default:
                        //    Console.WriteLine(nav.Evaluate(query));
                        //    break;
                }
            }
        }
        catch
        {
        }
        return dblOutput;
    }

    public DataTable getCIMSDetails(string useFor, string searchName)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        StringBuilder sbBatch = new StringBuilder();
        DataRow dr;
        try
        {
            dt.Columns.Add("CIMSItemId", typeof(string));
            dt.Columns.Add("CIMSTYPE", typeof(string));
            dt.Columns.Add("CIMSItemDesc", typeof(string));

            if (common.myLen(searchName) > 0)
            {
                string queryString = string.Empty;
                if (useFor.Equals("I"))
                {
                    queryString = "<Request><List><Product><ByName>%" + searchName + "%</ByName></Product></List></Request>";
                }
                else if (useFor.Equals("G"))
                {
                    queryString = "<Request><List><GenericItem><ByMoleculeName>%" + searchName + "%</ByMoleculeName></GenericItem></List></Request>";
                }

                string outputValues = getFastTrack5Output(queryString);

                sbBatch.Append(common.myStr(outputValues));
                StringReader srBatch = new StringReader(sbBatch.ToString());
                ds.ReadXml(srBatch);

                foreach (DataTable tbl in ds.Tables)
                {
                    switch (common.myStr(tbl.TableName))
                    {
                        case "Product":
                        case "GenericItem":
                        case "GGPI":

                            if (tbl.Columns.Contains("name"))
                            {
                                if (tbl.Rows.Count > 0)
                                {
                                    foreach (DataRow data in tbl.Rows)
                                    {
                                        dr = dt.NewRow();

                                        dr["CIMSItemId"] = common.myStr(data["reference"]);
                                        dr["CIMSItemDesc"] = common.myStr(data["name"]);
                                        dr["CIMSTYPE"] = common.myStr(tbl.TableName);

                                        dt.Rows.Add(dr);
                                        dt.AcceptChanges();
                                    }
                                }
                            }
                            break;

                    }
                }
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }

        return dt;
    }

}
