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
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Summary description for clsVIDAL
/// </summary>
public class clsVIDAL
{
    string sConString = "";
    Hashtable hstInp;
    Hashtable hstOut;
    DataSet ds;
    DAL.DAL DlObj;

    private string sVidalConString = ConfigurationManager.ConnectionStrings["vidal"].ConnectionString;

    ////private static VSProductService.productServiceClient objProductService;
    ////private static VSDrugInteractionClassService.drugInteractionClassServiceClient objDrugInteractionClassService;
    //private static VSInteractionService.interactionServiceClient objInteractionService;
    ////private static VSDatabaseInfoService.databaseInfoServiceClient objDatabaseInfoService;
    //private static VSDocumentService.documentServiceClient objDocumentService;
    ////private static VSIndicationGroupService.indicationGroupServiceClient objIndicationGroupService;
    //private static VSAllergyService.allergyServiceClient objAllergyService;
    //private static VSSideEffectService.sideEffectServiceClient objSideEffectService;
    //private static VSWarningService.warningServiceClient objWarningService;
    //private static VSContraIndicationService.contraIndicationServiceClient objContraIndicationService;
    //private static VSCim10Service.cim10ServiceClient objCim10Service;
    //private static VSPrecautionService.precautionServiceClient objPrecautionService;

    public clsVIDAL(string Constring)
    {
        sConString = Constring;
        DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    }

    public DataTable getVIDALMonograph(int? commonNameGroupId)
    {
        DataTable tbl = new DataTable();
        //try
        //{
        //    DataColumn col = new DataColumn("URL");
        //    tbl.Columns.Add(col);

        //    col = new DataColumn("TYPE");
        //    tbl.Columns.Add(col);

        //    col = new DataColumn("NAME");
        //    tbl.Columns.Add(col);

        //    objDocumentService = new VSDocumentService.documentServiceClient("DocumentService" + "HttpPort", sVidalConString + "DocumentService");
        //    VSDocumentService.ArrayOfDocument objArrayOfDocument = objDocumentService.searchByCommonNameGroupId(commonNameGroupId);

        //    for (int idx = 0; idx < objArrayOfDocument.Count; idx++)
        //    {
        //        if (common.myStr(objArrayOfDocument[idx].type) == "VMPFR")
        //        {
        //            DataRow DR = tbl.NewRow();
        //            //objArrayOfDocument[idx].id//ProductId
        //            DR["URL"] = common.myStr(objArrayOfDocument[idx].urlAsString);
        //            DR["TYPE"] = common.myStr(objArrayOfDocument[idx].type);
        //            DR["NAME"] = common.myStr(objArrayOfDocument[idx].name);

        //            tbl.Rows.Add(DR);
        //        }
        //    }
        //}
        //catch
        //{
        //}

        return tbl;
    }

    public StringBuilder getVIDALDrugToDrugInteraction(bool isOnlyInteractionChk, int?[] commonNameGroupIds, out Hashtable collVitalItemIdFound)
    {
        StringBuilder sb = new StringBuilder();
        collVitalItemIdFound = new Hashtable();

        //try
        //{
        //    objInteractionService = new VSInteractionService.interactionServiceClient("InteractionService" + "HttpPort", sVidalConString + "InteractionService");

        //    VSInteractionService.InteractionCommonNameGroupResult objInteractionCommonNameGroupResult
        //        = objInteractionService.searchInteractionCouplesForCommonNameGroupIds(commonNameGroupIds, VSInteractionService.InteractionSeverityType.TAKE_INTO_ACCOUNT);

        //    VSInteractionService.interactionCoupleCommonNameGroup[] objinteractionCoupleCommonNameGroup
        //        = objInteractionCommonNameGroupResult.interactionCommonNameGroupCoupleList;

        //    int InteractionCount = 0;
        //    for (int idx = 0; idx < objinteractionCoupleCommonNameGroup.Length; idx++)
        //    {
        //        VSInteractionService.commonNameGroup objCommonNameGroupA = objinteractionCoupleCommonNameGroup[idx].commonNameGroupA;
        //        VSInteractionService.commonNameGroup objCommonNameGroupB = objinteractionCoupleCommonNameGroup[idx].commonNameGroupB;

        //        //VSInteractionService.drugInteractionClass objdrugInteractionClassA = objinteractionCoupleCommonNameGroup[idx].drugInteractionClassA;
        //        //VSInteractionService.drugInteractionClass objdrugInteractionClassB = objinteractionCoupleCommonNameGroup[idx].drugInteractionClassB;

        //        if (!collVitalItemIdFound.ContainsValue(objCommonNameGroupA.id))
        //        {
        //            collVitalItemIdFound.Add(InteractionCount++, objCommonNameGroupA.id);
        //        }
        //        if (!collVitalItemIdFound.ContainsValue(objCommonNameGroupB.id))
        //        {
        //            collVitalItemIdFound.Add(InteractionCount++, objCommonNameGroupB.id);
        //        }

        //        if (isOnlyInteractionChk)
        //        {
        //            continue;
        //        }

        //        VSInteractionService.InteractionSeverityType? objInteractionSeverityType = objinteractionCoupleCommonNameGroup[idx].severity;

        //        sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");
        //        sb.Append("<tr>");
        //        sb.Append("<td valign=\"top\" style=\"width:10px;height:100%;background-color:" + getSeverityColorCode(common.myStr(objInteractionSeverityType.Value)) + "\">&nbsp;</td>");


        //        sb.Append("<td valign=\"top\">");

        //        sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin-left: 5px\">");

        //        sb.Append("<tr><td>");
        //        sb.Append(headingString("Severity : ") + titleCase(common.myStr(objInteractionSeverityType.Value)).Replace("_", " "));
        //        sb.Append("</td></tr>");

        //        sb.Append("<tr><td>");
        //        sb.Append(headingString("Interaction between") + " [ " + common.myStr(objCommonNameGroupA.name) + " ] and [ " + common.myStr(objCommonNameGroupB.name) + " ]");
        //        sb.Append("</td></tr>");

        //        sb.Append("<tr><td>");
        //        sb.Append(headingString("Description : ") + common.myStr(objinteractionCoupleCommonNameGroup[idx].precautionComment));
        //        sb.Append("</td></tr>");

        //        sb.Append("<tr><td>");
        //        sb.Append(headingString("Risk : ") + common.myStr(objinteractionCoupleCommonNameGroup[idx].riskComment));
        //        sb.Append("</td></tr>");

        //        sb.Append("</table>");
        //        sb.Append("</td>");
        //        sb.Append("</tr>");

        //        sb.Append("</table></br>");
        //    }
        //}
        //catch
        //{
        //}

        return sb;
    }


    public StringBuilder getVIDALDrugHealthInteraction(int?[] commonNameGroupIds, DateTime dateOfBirth, int? weight,
                    int breastfeeding, int? weeksOfAmenorrhoea, int? creatinClearance, string gender,
                    List<string> ICDIds, out Hashtable collVitalItemIdFound)
    {
        StringBuilder sb = new StringBuilder();
        collVitalItemIdFound = new Hashtable();



        try
        {
            if (weight == 0)
            {
                weight = null;
            }

            //List<IndicationGroup> indicationGroupList;

            //// Cim10CodingHelper pour le terme Rhume
            //VidalAPI api = new VidalAPI();
            //VidalServices services = api.


            //IndicationGroupService indicationGroupService = services.getIndicationGroupService();
            //Cim10Service cim10Service = services.getCim10Service();
            ////1- retrouver les Groupes d'indications Vidal pour le terme recherché
            //indicationGroupList = indicationGroupService.searchByName("rhume");

            ////2- pour chacun des Groupes d'indications du terme récupérer les Cim10 associées:

            //for (IndicationGroup indicationGroup : indicationGroupList) {
            //   List<Cim10> pathologies = cim10Service.searchByIndicationGroup(indicationGroup);
            //   System.out.println("groupe d'indication : " + indicationGroup.getName());
            //   for (Cim10 cim10 : pathologies) {
            //      System.out.println(" => Cim10 " + "[" + cim10.getCode() + "]" + cim10.getName());
            //   }
            //   System.out.println("---------------------------------");
            //}

            //VSContraIndicationService.Breastfeeding breastfeedingCI = VSContraIndicationService.Breastfeeding.NONE;
            //VSPrecautionService.Breastfeeding breastfeedingP = VSPrecautionService.Breastfeeding.NONE;

            //if (breastfeeding != 0)
            //{
            //    breastfeedingCI = VSContraIndicationService.Breastfeeding.ALL;
            //    breastfeedingP = VSPrecautionService.Breastfeeding.ALL;
            //}

            //VSContraIndicationService.Gender genderCI = VSContraIndicationService.Gender.FEMALE;
            //VSPrecautionService.Gender genderP = VSPrecautionService.Gender.FEMALE;

            //switch (common.myStr(gender))
            //{
            //    case "F"://Female
            //        genderCI = VSContraIndicationService.Gender.FEMALE;
            //        genderP = VSPrecautionService.Gender.FEMALE;
            //        break;
            //    case "M"://Male
            //        genderCI = VSContraIndicationService.Gender.MALE;
            //        genderP = VSPrecautionService.Gender.MALE;
            //        break;
            //    case "O"://Other
            //        break;
            //    case "U"://Unknown
            //        break;
            //    default:
            //        break;
            //}

            //objContraIndicationService = new VSContraIndicationService.contraIndicationServiceClient("ContraIndicationService" + "HttpPort", sVidalConString + "ContraIndicationService");
            //objPrecautionService = new VSPrecautionService.precautionServiceClient("PrecautionService" + "HttpPort", sVidalConString + "PrecautionService");
            //objCim10Service = new VSCim10Service.cim10ServiceClient("Cim10Service" + "HttpPort", sVidalConString + "Cim10Service");
        }
        //ContraindicationService.searchByCommonNameGroupIdAndEpp
        //PrecautionService.searchByCommonNameGroupIdAndEpp
        //Cim10Service.searchByCode(String)
        //ContraIndicationService.searchByCommonNameGroupIdAndCim10IdsWithChildren

        //    VSContraIndicationService.CommonNameGroupContraIndicationEppList objCommonNameGroupContraIndicationEppList
        //        = objContraIndicationService.searchByCommonNameGroupIdsAndEpp(commonNameGroupIds, dateOfBirth, weight,
        //                                    breastfeedingCI, weeksOfAmenorrhoea, creatinClearance, genderCI);

        //    VSContraIndicationService.contraIndicationTypeEppTuple[] objcontraIndicationTypeEppTuple
        //     = objCommonNameGroupContraIndicationEppList.contraIndications;

        //    //  <contraIndications xmlns="urn:Vidal">
        //    //   <contraIndicationTypeEppTuple>
        //    //      <commonNameGroupId>28</commonNameGroupId>
        //    //      <contraIndication>
        //    //         <id>4001</id>
        //    //         <name>Women of childbearing potential</name>
        //    //      </contraIndication>
        //    //      <epp>GENDER</epp>
        //    //      <type>ABSOLUTE</type>
        //    //   </contraIndicationTypeEppTuple>
        //    //</contraIndications>

        //    string Severity = "Precaution use";

        //    if (objcontraIndicationTypeEppTuple.Length > 0)
        //    {
        //        sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

        //        sb.Append("<tr><td>");
        //        sb.Append("<b><u>" + headingString("Contraindication") + "</u></b>");
        //        sb.Append("</td></tr>");

        //        sb.Append("</table></br>");
        //    }

        //    int InteractionCount = 0;
        //    string severityName = "PRECAUTION_USE";
        //    for (int idx = 0; idx < objcontraIndicationTypeEppTuple.Length; idx++)
        //    {
        //        sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");
        //        sb.Append("<tr>");

        //        if (common.myStr(objcontraIndicationTypeEppTuple[idx].type).ToUpper() == "ABSOLUTE")
        //        {
        //            Severity = "Contraindication";
        //            severityName = "CONTRAINDICATIONS";
        //        }
        //        else if (common.myStr(objcontraIndicationTypeEppTuple[idx].type).ToUpper() == "RELATIVE")
        //        {
        //            Severity = "Combination not recommended";
        //            severityName = "DISADVISES_ASSOCIATION";
        //        }
        //        else
        //        {
        //            severityName = "PRECAUTION_USE";
        //        }

        //        sb.Append("<td valign=\"top\" style=\"width:10px;height:100%;background-color:" + getSeverityColorCode(common.myStr(severityName)) + "\">&nbsp;</td>");

        //        sb.Append("<td valign=\"top\">");

        //        sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin-left: 5px\">");

        //        if (!collVitalItemIdFound.ContainsValue(objcontraIndicationTypeEppTuple[idx].commonNameGroupId))
        //        {
        //            collVitalItemIdFound.Add(InteractionCount++, objcontraIndicationTypeEppTuple[idx].commonNameGroupId);
        //        }

        //        sb.Append("<tr><td>");
        //        sb.Append(headingString("Severity : ") + Severity);
        //        sb.Append("</td></tr>");

        //        sb.Append("<tr><td>");
        //        sb.Append(headingString("Contraindication : ") + common.myStr(objcontraIndicationTypeEppTuple[idx].contraIndication.name));
        //        sb.Append("</td></tr>");

        //        sb.Append("<tr><td>");
        //        sb.Append(headingString("Epp : ") + common.myStr(objcontraIndicationTypeEppTuple[idx].epp));
        //        sb.Append("</td></tr>");


        //        sb.Append("</table>");
        //        sb.Append("</td>");
        //        sb.Append("</tr>");

        //        sb.Append("</table></br>");
        //    }

        //    //VSContraIndicationService.Message[] objMessage
        //    //    = objCommonNameGroupContraIndicationEppList.messages;

        //    //for (int idx = 0; idx < objMessage.Length; idx++)
        //    //{
        //    //    sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

        //    //    sb.Append("<tr><td>");
        //    //    sb.Append(" <b>Message Type :</b> " + titleCase(common.myStr(objMessage[idx].messageType).Replace("_", " "));
        //    //    sb.Append("</td></tr>");

        //    //    sb.Append("</table></br>");
        //    //}

        //    //List<int?> lstIds = new List<int?>();

        //    //foreach (string icdId in ICDIds)
        //    //{
        //    //    VSCim10Service.ArrayOfCim10 objArrayOfCim10 = objCim10Service.searchByCode(icdId);
        //    //    for (int idx = 0; idx < objArrayOfCim10.Count; idx++)
        //    //    {
        //    //        lstIds.Add(objArrayOfCim10[idx].id);
        //    //    }
        //    //}

        //    //int?[] icm10Ids = lstIds.ToArray();


        //    //foreach (int? commonNameGroupId in commonNameGroupIds)
        //    //{
        //    //    VSContraIndicationService.CommonNameGroupContraIndicationCim10List objCommonNameGroupContraIndicationCim10List
        //    //        = objContraIndicationService.searchByCommonNameGroupIdAndCim10IdsWithChildren(commonNameGroupId, icm10Ids);

        //    //    VSContraIndicationService.contraIndicationTypeCim10Tuple[] objcontraIndicationTypeCim10Tuple
        //    //     = objCommonNameGroupContraIndicationCim10List.contraIndications;

        //    //    for (int idx = 0; idx < objcontraIndicationTypeCim10Tuple.Length; idx++)
        //    //    {
        //    //        sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

        //    //        sb.Append("<tr><td>");
        //    //        sb.Append(" <b>cim10 :</b> " + common.myStr(objcontraIndicationTypeCim10Tuple[idx].cim10));
        //    //        sb.Append("</td></tr>");

        //    //        sb.Append("<tr><td>");
        //    //        sb.Append(" <b>type :</b> " + common.myStr(objcontraIndicationTypeCim10Tuple[idx].type));
        //    //        sb.Append("</td></tr>");

        //    //        sb.Append("<tr><td>");
        //    //        sb.Append(" <b>Patient condition :</b> " + common.myStr(objcontraIndicationTypeCim10Tuple[idx].contraIndication.name));
        //    //        sb.Append("</td></tr>");

        //    //        sb.Append("</table></br>");
        //    //    }
        //    //}


        //    VSPrecautionService.CommonNameGroupPrecautionEppList objCommonNameGroupPrecautionEppList
        //        = objPrecautionService.searchByCommonNameGroupIdsAndEpp(commonNameGroupIds, dateOfBirth, weight,
        //                                    breastfeedingP, weeksOfAmenorrhoea, creatinClearance, genderP);

        //    if (objCommonNameGroupPrecautionEppList != null)
        //    {
        //        VSPrecautionService.precautionEppPair[] objprecautionEppPair
        //         = objCommonNameGroupPrecautionEppList.precautions;

        //        if (objprecautionEppPair.Length > 0)
        //        {
        //            sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

        //            sb.Append("<tr><td>");
        //            sb.Append("<b><u>" + headingString("Precaution") + "</u></b>");
        //            sb.Append("</td></tr>");

        //            sb.Append("</table></br>");
        //        }

        //        for (int idx = 0; idx < objprecautionEppPair.Length; idx++)
        //        {
        //            sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");
        //            sb.Append("<tr>");
        //            sb.Append("<td valign=\"top\" style=\"width:10px;height:100%;background-color:" + getSeverityColorCode(common.myStr("PRECAUTION_USE")) + "\">&nbsp;</td>");


        //            sb.Append("<td valign=\"top\">");

        //            sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin-left: 5px\">");

        //            sb.Append("<tr><td>");
        //            sb.Append(headingString("Severity : ") + " Precaution use");
        //            sb.Append("</td></tr>");

        //            sb.Append("<tr><td>");
        //            sb.Append(headingString("Precaution : ") + common.myStr(objprecautionEppPair[idx].precaution.name));
        //            sb.Append("</td></tr>");

        //            sb.Append("<tr><td>");
        //            sb.Append(headingString("Epp : ") + common.myStr(objprecautionEppPair[idx].epp));
        //            sb.Append("</td></tr>");

        //            sb.Append("</table>");
        //            sb.Append("</td>");
        //            sb.Append("</tr>");

        //            sb.Append("</table></br>");
        //        }
        //    }

        //    //foreach (int? commonNameGroupId in commonNameGroupIds)
        //    //{
        //    //    VSPrecautionService.CommonNameGroupPrecautionCim10List objCommonNameGroupPrecautionCim10List
        //    //        = objPrecautionService.searchByCommonNameGroupIdAndCim10IdsWithChildren(commonNameGroupId, icm10Ids);

        //    //    VSPrecautionService.precautionCim10Pair[] objprecautionCim10Pair
        //    //     = objCommonNameGroupPrecautionCim10List.precautions;

        //    //    for (int idx = 0; idx < objprecautionCim10Pair.Length; idx++)
        //    //    {
        //    //        sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

        //    //        sb.Append("<tr><td>");
        //    //        sb.Append(" <b>cim10 :</b> " + common.myStr(objprecautionCim10Pair[idx].cim10));
        //    //        sb.Append("</td></tr>");

        //    //        sb.Append("<tr><td>");
        //    //        sb.Append(" <b>precaution :</b> " + common.myStr(objprecautionCim10Pair[idx].precaution));
        //    //        sb.Append("</td></tr>");

        //    //        sb.Append("</table></br>");
        //    //    }
        //    //}
        //}
        catch
        {
        }

        return sb;
    }

    public StringBuilder getVIDALDrugAllergyInteraction(int?[] commonNameGroupIds, int?[] allergyIds, int?[] moleculeIds)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            //objAllergyService = new VSAllergyService.allergyServiceClient("AllergyService" + "HttpPort", sVidalConString + "AllergyService");

            //VSAllergyService.commonNameGroupAllergyCouple[] objcommonNameGroupAllergyCouple
            //     = objAllergyService.getAlertsOnCommonNameGroupActivePrincipleAllergiesClass(commonNameGroupIds, allergyIds);

            ////VSAllergyService.commonNameGroupMoleculeCouple[] commonNameGroupMoleculeCouple
            ////= objAllergyService.getAlertsOnCommonNameGroupActivePrincipleAllergiesMolecules(commonNameGroupIds, moleculeIds);

            //for (int idx = 0; idx < objcommonNameGroupAllergyCouple.Length; idx++)
            //{
            //    sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");
            //    sb.Append("<tr>");
            //    sb.Append("<td valign=\"top\" style=\"width:10px;height:100%;background-color:" + getSeverityColorCode(common.myStr("CONTRAINDICATIONS")) + "\">&nbsp;</td>");


            //    sb.Append("<td valign=\"top\">");

            //    sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin-left: 5px\">");

            //    sb.Append("<tr><td>");
            //    sb.Append(headingString("Severity : ") + "Contraindication"); //titleCase(objcommonNameGroupAllergyCouple[idx].molecule.role).Replace("_", " "));
            //    sb.Append("</td></tr>");

            //    sb.Append("<tr><td>");
            //    sb.Append(headingString("Drug : ") + common.myStr(objcommonNameGroupAllergyCouple[idx].commonNameGroup.name));
            //    sb.Append("</td></tr>");

            //    sb.Append("<tr><td>");
            //    sb.Append(headingString("Patient condition : ") + common.myStr(objcommonNameGroupAllergyCouple[idx].allergy.name));
            //    sb.Append("</td></tr>");

            //    sb.Append("</table>");
            //    sb.Append("</td>");
            //    sb.Append("</tr>");

            //    sb.Append("</table></br>");
            //}

            ////VSAllergyService.pagedResultAllergy objpagedResultAllergy = objAllergyService.getAllAllergies(1, 300);
            ////VSAllergyService.allergy[] result = objpagedResultAllergy.result;

            ////sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");
            ////for (int idx = 0; idx < result.Length; idx++)
            ////{
            ////    sb.Append("<tr><td>");
            ////    sb.Append(common.myStr(result[idx].id) + "  -  " + common.myStr(result[idx].name));
            ////    sb.Append("</td></tr>");
            ////}

            ////sb.Append("</table></br>");




            ////for (int idx = 0; idx < commonNameGroupMoleculeCouple.Length; idx++)
            ////{
            ////    sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

            ////    sb.Append("<tr><td>");
            ////    sb.Append(" <b>Drug :</b> " + common.myStr(commonNameGroupMoleculeCouple[idx].commonNameGroup.name));
            ////    sb.Append("</td></tr>");

            ////    sb.Append("<tr><td>");
            ////    sb.Append(" <b>Molecule :</b> " + common.myStr(commonNameGroupMoleculeCouple[idx].molecule.name));
            ////    sb.Append("</td></tr>");

            ////    sb.Append("<tr><td>");
            ////    sb.Append(" <b>Allergy :</b> " + common.myStr(commonNameGroupMoleculeCouple[idx].allergy.name));
            ////    sb.Append("</td></tr>");

            ////    sb.Append("<tr><td>");
            ////    sb.Append(" <b>Type :</b> " + titleCase(common.myStr(commonNameGroupMoleculeCouple[idx].type).Replace("_", " "));
            ////    sb.Append("</td></tr>");

            ////    sb.Append("</table></br>");
            //}
        }
        catch
        {
        }

        return sb;
    }

    public StringBuilder getVIDALDrugWarning(int? commonNameGroupId)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            //objWarningService = new VSWarningService.warningServiceClient("WarningService" + "HttpPort", sVidalConString + "WarningService");

            //VSWarningService.CommonNameGroupWarningList objCommonNameGroupWarningList
            //    = objWarningService.searchByCommonNameGroupId(commonNameGroupId);

            //VSWarningService.ArrayOfWarningCommentCouple objArrayOfWarningCommentCouple
            //    = objCommonNameGroupWarningList.warningCommentCouples;

            //for (int idx = 0; idx < objArrayOfWarningCommentCouple.Count; idx++)
            //{
            //    sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

            //    sb.Append("<tr><td>");
            //    sb.Append(headingString("Warning : ") + common.myStr(objArrayOfWarningCommentCouple[idx].warning.name));
            //    sb.Append("</td></tr>");

            //    sb.Append("<tr><td>");
            //    sb.Append(headingString("Comment : ") + common.myStr(objArrayOfWarningCommentCouple[idx].comment));
            //    sb.Append("</td></tr>");

            //    sb.Append("</table></br>");
            //}
        }
        catch
        {
        }

        return sb;
    }

    public StringBuilder getVIDALDrugSideEffect(int?[] commonNameGroupIds)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            //objSideEffectService = new VSSideEffectService.sideEffectServiceClient("SideEffectService" + "HttpPort", sVidalConString + "SideEffectService");

            //VSSideEffectService.commonNameGroupSideEffect[] objcommonNameGroupSideEffect
            //    = objSideEffectService.searchByCommonNameGroupIds(commonNameGroupIds);

            //DataTable tbl = new DataTable();

            //DataColumn col = new DataColumn("Frequency");
            //tbl.Columns.Add(col);

            //col = new DataColumn("SideEffect");
            //tbl.Columns.Add(col);

            //string Frequency = string.Empty;
            //string SideEffect = string.Empty;

            //for (int idx = 0; idx < objcommonNameGroupSideEffect.Length; idx++)
            //{
            //    Frequency = titleCase(common.myStr(objcommonNameGroupSideEffect[idx].frequency)).Replace("_", " ");
            //    SideEffect = common.myStr(objcommonNameGroupSideEffect[idx].sideEffect.name);

            //    tbl.DefaultView.RowFilter = "Frequency='" + Frequency + "'";

            //    if (tbl.DefaultView.Count == 0)
            //    {
            //        DataRow DR = tbl.NewRow();
            //        DR["Frequency"] = Frequency;
            //        DR["SideEffect"] = SideEffect;
            //        tbl.Rows.Add(DR);
            //    }
            //    else
            //    {
            //        tbl.DefaultView[0]["SideEffect"] = common.myStr(tbl.DefaultView[0]["SideEffect"]) + ", " + SideEffect;
            //    }
            //    tbl.DefaultView.RowFilter = "";
            //}

            //sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

            //foreach (DataRow DR in tbl.Rows)
            //{
            //    //sb.Append(" <b>Drug :</b> " + common.myStr(objcommonNameGroupSideEffect[idx].commonNameGroup.name));

            //    sb.Append("<tr><td>");
            //    sb.Append(headingString("Frequency : ") + common.myStr(DR["Frequency"]));
            //    sb.Append("</td></tr>");

            //    sb.Append("<tr><td>");
            //    sb.Append(headingString("Side Effect : ") + common.myStr(DR["SideEffect"]));
            //    sb.Append("</td></tr>");

            //    sb.Append("<tr><td>");
            //    sb.Append("&nbsp;");
            //    sb.Append("</td></tr>");
            //}

            //sb.Append("</table>");

        }
        catch
        {
        }

        return sb;
    }

    public string titleCase(string strVal)
    {
        if (common.myStr(strVal).Length == 0)
        {
            return string.Empty;
        }
        return char.ToUpper(strVal[0]) + strVal.Substring(1).ToLower();

        //return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(common.myStr(strVal).ToLower());
    }

    public string headingString(string strVal)
    {
        return "<font color=Gray><I>" + strVal + "</I></font>";
    }

    public string getSeverityColorCode(string Severity)
    {
        string colorCode = "#CF113C";
        switch (Severity)
        {
            case "CONTRAINDICATIONS":
                colorCode = "#CF113C";
                break;
            case "DISADVISES_ASSOCIATION":
                colorCode = "#FF4326";
                break;
            case "PRECAUTION_USE":
                colorCode = "#F97D00";
                break;
            case "TAKE_INTO_ACCOUNT":
                colorCode = "#FFDC2C";
                break;
        }

        return colorCode;
    }

    public DataTable getVIDALBrandDetails(int? commonNameGroupId)
    {
        DataTable tbl = new DataTable();
        try
        {
            DataColumn col = new DataColumn("URL");
            tbl.Columns.Add(col);

            col = new DataColumn("TYPE");
            tbl.Columns.Add(col);

            col = new DataColumn("NAME");
            tbl.Columns.Add(col);

            //objDocumentService = new VSDocumentService.documentServiceClient("DocumentService" + "HttpPort", sVidalConString + "DocumentService");
            //VSDocumentService.ArrayOfDocument objArrayOfDocument = objDocumentService.searchByCommonNameGroupId(commonNameGroupId);

            //for (int idx = 0; idx < objArrayOfDocument.Count; idx++)
            //{
            //    if (common.myStr(objArrayOfDocument[idx].type).ToUpper().Equals("VMPCLASS"))
            //    {
            //        DataRow DR = tbl.NewRow();
            //        //objArrayOfDocument[idx].id//ProductId
            //        DR["URL"] = common.myStr(objArrayOfDocument[idx].urlAsString);
            //        DR["TYPE"] = common.myStr(objArrayOfDocument[idx].type);
            //        DR["NAME"] = common.myStr(objArrayOfDocument[idx].name);

            //        tbl.Rows.Add(DR);
            //    }
            //}
        }
        catch (Exception ex)
        {
        }

        return tbl;
    }
}
