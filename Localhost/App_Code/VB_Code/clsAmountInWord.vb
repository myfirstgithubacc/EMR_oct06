Imports Microsoft.VisualBasic
Imports System.Windows.Forms

Public Class clsAmountInWord
    Public Function PrnWords(ByRef nAmount As Object) As String
        Dim aWrd1, aUnit, aFact, aWrd2 As Object
        Dim nAm1, nCount, nAm2 As Object
        Dim nAm3 As Short
        Dim nLen As Short
        Dim nAmnt As Integer
        Dim nDecAmnt As Short
        Dim cRetStrg As String
        Try
            aUnit = New Object() {" Crore", " Lakh", " Thousand", " Hundred", " "}
            aFact = New Object() {10000000, 100000, 1000, 100, 1}
            aWrd1 = New Object() {"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty"}
            aWrd2 = New Object() {"Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"}

            nLen = UBound(aUnit)
            nAmnt = CInt(System.Math.Abs(Val(nAmount)))
            nDecAmnt = (nAmount - nAmnt) * 100

            Dim aRsStrg As Object
            aRsStrg = New Object() {"", "", "", "", ""}

            For nCount = 0 To nLen
                nAm1 = Int(nAmnt / aFact(nCount))
                If nAm1 <> 0 Then
                    If nAm1 < 21 Then
                        aRsStrg(nCount) = aWrd1(nAm1 - 1) & aUnit(nCount) & IIf(nAm1 > 0 And nCount <= 2, "", "")
                    Else
                        nAm2 = Int(nAm1 / 10)
                        nAm3 = nAm1 Mod 10
                        aRsStrg(nCount) = aWrd2(nAm2 - 2)
                        If nAm3 > 0 Then
                            aRsStrg(nCount) = aRsStrg(nCount) & " " & aWrd1(nAm3 - 1) & aUnit(nCount) & IIf(nAm1 > 0 And nCount <= 2, "", "")
                        Else
                            aRsStrg(nCount) = aRsStrg(nCount) & aUnit(nCount) & IIf(nAm1 > 0 And nCount <= 2, "", "")
                        End If
                    End If
                    nAmnt = nAmnt - (nAm1 * aFact(nCount))
                End If
            Next nCount
            nLen = UBound(aRsStrg)
            For nCount = 0 To nLen
                If aRsStrg(nCount) <> "" Then
                    cRetStrg = cRetStrg & aRsStrg(nCount) & " "
                End If
            Next nCount
            cRetStrg = "Rupees " & Trim(cRetStrg)
            ' for paise
            If nDecAmnt <= 0 Then
            ElseIf nDecAmnt < 21 Then
                cRetStrg = cRetStrg & " & Paise " & aWrd1(nDecAmnt - 1)
            Else
                nAm2 = Int(nDecAmnt / 10)
                nAm3 = nDecAmnt Mod 10
                cRetStrg = cRetStrg & " and Paise " & aWrd2(nAm2 - 2) & " "
                If nAm3 > 0 Then
                    cRetStrg = cRetStrg & aWrd1(nAm3 - 1)
                End If
            End If
            PrnWords = Trim(cRetStrg) & " only"
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
    End Function

End Class
