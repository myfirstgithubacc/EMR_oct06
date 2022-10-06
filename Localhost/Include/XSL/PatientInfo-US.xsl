<?xml version="1.0" encoding="UTF-8"?>
<!-- Created by Robin  Simon -->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
    <xsl:template match="/pr">
        <table width="100%" cellpadding="0" cellspacing="0" style="color:black;  background:#fff;">
            <tr>
                <td style="border-bottom: solid; border-width: 0px; border-color:gray"  valign="top" width="100%">
                    <table width="100%" cellpadding="2" cellspacing="2">
                        <tr> 
                            <td  align="left" valign="top" colspan="2">
                                <span style="color:#990066;"><strong Style="text-transform:uppercase"><xsl:value-of select="PatientName" /></strong></span>
                                <xsl:text>, </xsl:text>
                                <xsl:value-of select="PatientAgeGender"/>
                                <xsl:text>, </xsl:text>

                                <strong><xsl:text>UHID</xsl:text></strong>
                                <xsl:text> </xsl:text>
                                <xsl:value-of select="RegistrationNo"/>                                
                                <xsl:text>, </xsl:text>

                                <strong><xsl:text>DOB</xsl:text></strong>
                                <xsl:text> </xsl:text>
                                <xsl:value-of select="DateOfBirth"/>
                                <xsl:text>,   </xsl:text>
                              
                                <strong><xsl:text>Home #</xsl:text></strong>
                                <xsl:text>  </xsl:text>
                                <xsl:value-of select="PhoneHome"/>
                                <xsl:text>, </xsl:text>
                              
                                <strong><xsl:text>Mobile #</xsl:text></strong>
                                <xsl:text>  </xsl:text>
                                <xsl:value-of select="MobileNo"/>
                            </td>
                        </tr>
                    </table>
                </td>
              
                <!--
                  <td style="border-right:solid 1px gray"  valign="top" width="50%">
                    <table width="100%" cellpadding="2" cellspacing="0">
						          <tr>
							          <td><strong><xsl:text>Home</xsl:text></strong></td>
							          <td><xsl:value-of select="PhoneHome"/></td>
						          </tr>
						          
                      <tr>
							          <td><strong><xsl:text>Mobile</xsl:text></strong></td>
                        <td><xsl:value-of select="MobileNo"/></td>
						          </tr>
					          </table>
				          </td>
                -->
              
            </tr>
        </table>
      
    </xsl:template>
</xsl:stylesheet>