<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
<xsl:output method="html" cdata-section-elements="GDOSE"/>
	<xsl:template match="/Result/Content/Product/Document/MONOGRAPH">		
<STYLE>  
		   TD.SecondColumn{ background-Color: beige; } 
		   TD.FirstColumn{ background-color: bisque; }
		</STYLE>

		

		<Table>
			<tr>
				<td class="FirstColumn" width="120px">
					<strong>Field</strong>
				</td>
				<td class="SecondColumn">
					<strong>Data</strong>
				</td>

			</tr>
	 		<tr>
	 			<td class="FirstColumn">Generic Name</td>
				<td class="SecondColumn"><xsl:value-of select="GMNAME/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Class</td>
				<td class="SecondColumn"><xsl:value-of select="GCLS/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Category</td>
				<td class="SecondColumn"><xsl:value-of select="GCategory/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Pregnancy Category</td>
				<td class="SecondColumn"><xsl:value-of select="GPCAT/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Contra Indication</td>
				<td class="SecondColumn"><xsl:value-of select="GCI/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Special Precaution</td>
				<td class="SecondColumn"><xsl:value-of select="GSP/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Adverse Reaction</td>
				<td class="SecondColumn"><xsl:value-of select="GAR/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Drug Interaction</td>
				<td class="SecondColumn"><xsl:value-of select="GDI/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Food Interaction</td>
				<td class="SecondColumn"><xsl:value-of select="GFOOD/text()" disable-output-escaping="yes"/></td>
		      </tr>

			<tr>
	 			<td class="FirstColumn">Dosage</td>
			      <td class="SecondColumn"><xsl:value-of select="GDOSE/text()" disable-output-escaping="yes"/></td>
		      </tr>

		</Table>
 	
	</xsl:template>  
</xsl:stylesheet>

