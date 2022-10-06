
<!-- Created by Madhukar Jha (aspl) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
    <xsl:template match="/pr">
        <table width="100%" cellpadding="2" cellspacing="0" style="color:#003366">
            <tr>
                <td >
                    <table width="100%" cellpadding="2" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <span style="color:#990066">
                                    <strong>
                                        <xsl:value-of select="PatientName" />
                                    </strong>
                                </span>
                               <xsl:if test="string-length(PatientAgeGender)!=0">
                                <xsl:text>, </xsl:text>
                                <xsl:value-of select="PatientAgeGender" />
                                </xsl:if>
                                <xsl:if test="string-length(MaritalStat)!=0">
                                    <xsl:text> (</xsl:text>
                                    <xsl:value-of select="MaritalStat" />
                                    <xsl:text>) </xsl:text>
                                </xsl:if>
                                <xsl:if test="string-length(RegistrationNo)!=0">
                                    <xsl:text> -</xsl:text>
                                    <xsl:value-of select="RegistrationNo" />
                                </xsl:if>
                                <br />
                                <xsl:if test="string-length(GuardianRelationName)!=0">
                                    <xsl:value-of select="GuardianRelationName" />
                                    <xsl:text> </xsl:text>
                                </xsl:if>
                                <xsl:if test="string-length(GuardianName)!=0">
                                    <xsl:value-of select="GuardianName" />
                                    
                                </xsl:if>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <xsl:if test="string-length(LocalAddress)!=0">
                                    <xsl:value-of select="LocalAddress" />
                                    <xsl:if test="string-length(cm/Cityname)!=0">
                                        <xsl:text> </xsl:text>
                                        <xsl:value-of select="cm/Cityname" />
                                    </xsl:if>
                                    <xsl:if test="string-length(cm/sm/statename)!=0">
                                        <xsl:text>, </xsl:text>
                                        <xsl:value-of select="cm/sm/statename" />
                                    </xsl:if>
                                    <xsl:if test="string-length(cm/sm/cntM/CountryName)!=0">
                                        <xsl:text>, </xsl:text>
                                        <xsl:value-of select="cm/sm/cntM/CountryName" />
                                    </xsl:if>
                                </xsl:if>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <xsl:if test="string-length(PhoneHome)!=0">
                                    <xsl:value-of select="PhoneHome" />
                                </xsl:if>
                                <xsl:if test="string-length(MobileNo)!=0">
                                    <xsl:if test="string-length(PhoneHome)!=0">
                                        <xsl:text>, </xsl:text>
                                    </xsl:if>
                                    <xsl:value-of select="MobileNo" />
                                </xsl:if>
                                <xsl:if test="string-length(Email)!=0">
                                    <xsl:if test="string-length(PhoneHome)!=0 and string-length(MobileNo)!=0">
                                        <xsl:text>, </xsl:text>
                                    </xsl:if>
                                    <xsl:value-of select="Email" />
                                </xsl:if>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <xsl:if test="string-length(cm/sm/cntM/C1/C2/rod/ContactPerson)!=0">
                                    <strong>Person to be notified in Emergency</strong>

                                    <br />
                                    <xsl:value-of select="cm/sm/cntM/C1/C2/rod/ContactPerson" />
                                    <xsl:if test="string-length(cm/sm/cntM/C1/C2/rod/rm/OM/NM/km/kinname)!=0">
                                        <xsl:text> (</xsl:text>
                                        <xsl:value-of select="cm/sm/cntM/C1/C2/rod/rm/OM/NM/km/kinname" />
                                        <xsl:text>) </xsl:text>
                                    </xsl:if>
                                    <br />
                                    <xsl:value-of select="cm/sm/cntM/C1/C2/rod/ContactPersonMobile" />
                                    <xsl:if test="string-length(cm/sm/cntM/C1/C2/rod/ContactPersonPhone)!=0">
                                        <xsl:text>, </xsl:text> <xsl:value-of select="cm/sm/cntM/C1/C2/rod/ContactPersonPhone" />
                                    </xsl:if>                  
                                </xsl:if>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <xsl:if test="string-length(cm/sm/cntM/C1/C2/rod/rm/OM/NM/km/SocialSecurityCaption)!=0">
                                   
                                    <strong>
                                        <xsl:value-of select="cm/sm/cntM/C1/C2/rod/rm/OM/NM/km/SocialSecurityCaption" />
                                        <xsl:text> No</xsl:text>
                                    </strong>
                                    <xsl:text>  </xsl:text>
                                    <xsl:value-of select="cm/sm/cntM/C1/C2/rod/SocialSecurityNo" />
                                </xsl:if>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <xsl:if test="cm/sm/cntM/C1/C2/rod/VIP !=0">
                                    <strong>
                                        <xsl:text>He is a VIP Patient. </xsl:text>
                                    </strong>
                                    <xsl:text> (</xsl:text>
                                    <xsl:if test="string-length(cm/sm/cntM/C1/C2/rod/VIPNarration) !=0">
                                        <xsl:value-of select="cm/sm/cntM/C1/C2/rod/VIPNarration" />
                                    </xsl:if>
                                    <xsl:text>) </xsl:text>
                                </xsl:if>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <xsl:if test="cm/sm/cntM/C1/C2/rod/NewBorn !=0">
                                    <strong>
                                        <xsl:text>He is a New born baby. </xsl:text>
                                    </strong>
                                   
                                </xsl:if>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>
                                    <xsl:text>Sponsor Name</xsl:text>
                                </strong>
                                <xsl:text> </xsl:text>
                            </td>
                            <td>
                                <xsl:value-of select="cm/sm/cntM/C1/Sponsor" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>
                                    <xsl:text>Payer Name</xsl:text>
                                </strong>
                                <xsl:text> </xsl:text>
                            </td>
                            <td>
                                <xsl:value-of select="cm/sm/cntM/C1/C2/Payer" />
                            </td>
                       </tr>
                    </table>
                </td>
            </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>