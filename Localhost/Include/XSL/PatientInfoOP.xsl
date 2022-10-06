<?xml version="1.0" encoding="UTF-8"?>
<!-- Created by Madhukar Jha (aspl) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
  <xsl:template match="/pr">
    <table width="100%" cellpadding="2" cellspacing="0" style="color:black;  background:#e0ebfd;">
      <tr>
        <!--<td >
          <table width="100%" cellpadding="2" cellspacing="0">
            <tr>
              <td colspan="2">
                <span style="color:#990066">
                  <strong>
                    <xsl:value-of select="PatientName" />

                  </strong>
                </span>
                <xsl:text>, </xsl:text>
                <xsl:value-of select="PatientAgeGender" />
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
                <xsl:value-of select="c/SponsorName" />
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
                <xsl:value-of select="c/PayerName" />
              </td>
            </tr>

          </table>
        </td>-->
        <td style="border-right:solid 1px gray"  valign="top" width="50%">
          <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
              <td align="left" valign="top" colspan="2">
                <span style="color:#990066;">
                  <strong Style="text-transform:uppercase">
                    <xsl:value-of select="PatientName" />
                  </strong>
                </span>
                <xsl:text>, </xsl:text>
                <xsl:value-of select="PatientAgeGender"/>
              </td>
            </tr>

            <tr>
              <td>
                <strong>
                  <xsl:text>Address</xsl:text>
                </strong>
              </td>
              <td>
                <xsl:value-of select="LocalAddress"/>
              </td>
            </tr>
            <tr>
              <td>
                <xsl:text></xsl:text>
              </td>
              <td>
                <xsl:value-of select="CityName"/>
              </td>
            </tr>
            <tr>
              <td>
                <xsl:text></xsl:text>
              </td>
              <td>
                <xsl:value-of select="StateName"/>
              </td>
            </tr>
            <tr>
              <td>
                <xsl:text></xsl:text>
              </td>
              <td>
                <xsl:value-of select="CountryName"/>
              </td>
            </tr>

          </table>
        </td>
        <td style="border-right:solid 1px gray"  valign="top" width="50%">
          <table width="100%" cellpadding="2" cellspacing="0">
            <tr>
              <td>
                <strong>
                  <xsl:text>Home</xsl:text>
                </strong>
              </td>
              <td>
                <xsl:value-of select="PhoneHome"/>
              </td>
            </tr>
            <tr>
              <td>
                <strong>
                  <xsl:text>Mobile</xsl:text>
                </strong>
              </td>
              <td>
                <xsl:value-of select="MobileNo"/>
              </td>
            </tr>
            <tr>
              <td>
                <strong>
                  <xsl:text>Email</xsl:text>
                </strong>
              </td>
              <td>
                <xsl:value-of select="Email"/>
              </td>
            </tr>
            <tr>
              <td>
                <strong>
                  <xsl:text>Sponsor</xsl:text>
                </strong>
              </td>
              <td>
                <xsl:value-of select="c/SponsorName"/>
              </td>
            </tr>
            <tr>
              <td>
                <strong>
                  <xsl:text>Payer</xsl:text>
                </strong>
              </td>
              <td>
                <xsl:value-of select="c/PayerName"/>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>