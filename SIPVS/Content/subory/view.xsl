<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:zzz="http://fiit.stu.sk/SIPVS/UniversityApplication">

<xsl:template match="zzz:Student">
	<html>
		<head>
			<style>
			table {
				width: 650px;
				border: 2px solid black;
				border-collapse: collapse;
				margin: 30px;
			}
			tr, td, th {
				border: 1px solid black;
				border-collapse: collapse;
				text-align: left;
				padding-left: 5px;
				font-size: 12px;
				height: 20px;
			}
			.no-borders {
				border-style: none;
			}
			.no-bottom-border {
				border-bottom-style: none;
			}
			.no-top-border {
				border-top-style: none;
			}
			.no-right-border {
				border-right-style: none;
			}
			.no-left-border {
				border-left-style: none;
			}
			.no-horizontal-border {
				border-right-style: none;
				border-left-style: none;
			}
			.bold-border {
				border: 2px solid black;
			}
			</style>
		</head>
		<body>
			<table>
				<caption style="text-align:right;font-size: 12px;padding-right:50px;">Evidenčné číslo:</caption>
				<!--1-->
				<tr>
					<td class="bold-border" colspan="7" bgcolor="#ffcc00" style="font-size:16px;width:420px;">
						<b>Prihláška na vysokoškolské štúdium</b><br/>bakalárske – prvý stupeň alebo spojené – prvý a druhý stupeň v jednom celku<sup>1)</sup>
					</td>
					<th class="bold-border" colspan="6" rowspan="3" valign="top">Pečiatka VŠ, fakulty:</th>
				</tr>
				<!--2-->
				<tr>
					<th class="no-right-border" style="width:120px;">Akademický rok:</th>
					<td class="no-horizontal-border"><xsl:value-of select="zzz:Rok_od"/></td>
					<td class="no-horizontal-border">/</td>
					<td class="no-left-border" colspan="4"><xsl:value-of select="zzz:Rok_do"/></td>
				</tr>
				<!--3-->
				<tr>
					<th class="no-right-border">Štátne občianstvo:</th>
					<td class="no-left-border" colspan="6"><xsl:value-of select="zzz:stat"/></td>
				</tr>
				<!--4-->
				<tr>
					<th class="no-right-border">Meno:</th>
					<td class="no-horizontal-border" colspan="2"><xsl:value-of select="zzz:Meno"/></td>
					<th class="no-horizontal-border" colspan="2">Priezvisko:</th>
					<td class="no-left-border"       colspan="2"><xsl:value-of select="zzz:priezvisko"/></td>
					<td class="no-right-border"      colspan="2">Dátum doručenia:</td>
					<td class="no-left-border"       colspan="4">1.10.2020</td>
				</tr>
				<!--5-->
				<tr>
					<th class="no-right-border">Rodné priezvisko:</th>
					<td class="no-left-border"  colspan="6"><xsl:value-of select="zzz:rodne_priezvisko"/></td>
					<th class="no-right-border" colspan="2">Akademický titul:</th>
					<td class="no-left-border"  colspan="4"><xsl:value-of select="zzz:titul"/></td>
				</tr>
				<!--6-->

				<tr>
					<th class="no-right-border">Dátum narodenia:</th>
					<td class="no-horizontal-border">deň</td>
					<td class="no-horizontal-border"><xsl:value-of select="substring(zzz:datum_narodenia, 9, 2)"/></td>
					<td class="no-horizontal-border">mesiac</td>
					<td class="no-horizontal-border"><xsl:value-of select="substring(zzz:datum_narodenia, 6, 2)"/></td>
					<td class="no-horizontal-border">rok</td>
					<td class="no-left-border"><xsl:value-of select="substring(zzz:datum_narodenia, 1, 4)"/></td>
					<th class="no-right-border">Pohlavie<sup>2)</sup>:</th>
					<td class="no-horizontal-border"></td>
					<td class="no-left-border">muž</td>
					<xsl:choose>
						<xsl:when test="@Pohlavie">
							<xsl:if test="@Pohlavie='Muž'">
								<td>x</td>
							</xsl:if>
							<xsl:if test="@Pohlavie!='Muž'">
								<td>-</td>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<td>-</td>
						</xsl:otherwise>
					</xsl:choose>
					<td>žena</td>
					<xsl:choose>
						<xsl:when test="@Pohlavie">
							<xsl:if test="@Pohlavie='Žena'">
								<td>x</td>
							</xsl:if>
							<xsl:if test="@Pohlavie!='Žena'">
								<td>-</td>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<td>-</td>
						</xsl:otherwise>
					</xsl:choose>
				</tr>
				<!--7-->
				<tr>
					<th class="no-right-border">Miesto narodenia:</th>
					<td class="no-left-border" colspan="5"><xsl:value-of select="zzz:miesto_narodenia"/></td>
					<th class="no-right-border">Štát:</th>
					<td class="no-left-border" colspan="6"><xsl:value-of select="zzz:stat"/></td>
				</tr>
				<!--8-->
				<tr>
					<th class="no-right-border">Rodné číslo<sup>3)</sup>:</th>
					<td class="no-left-border" colspan="5"><xsl:value-of select="zzz:rodne_cislo"/></td>
					<td colspan="7"></td>
				</tr>
				<!--9-->
				<tr>
					<th class="no-right-border">Tel. č.<sup>4)</sup>:</th>
					<td class="no-left-border"  colspan="5"><xsl:value-of select="zzz:telefon"/></td>
					<th class="no-right-border" colspan="2">E-mailová adresa<sup>4)</sup>:</th>
					<td class="no-left-border"  colspan="5"><xsl:value-of select="zzz:email"/></td>
				</tr>
			</table>

			<table>
				<tr>
					<td colspan="7" style="width:631px; height:50px; text-align:center;">
						<b>Prospech podľa koncoročného vysvedčenia v jednotlivých ročníkoch strednej školy<sup>13)</sup></b><br/>(neuvádzať klasifikáciu zo správania)
					</td>
				</tr>
				<tr>
					<td rowspan="2" style="width:350px;"><b>Predmet</b></td>
					<td colspan="6" style="text-align:center;"><b>Ročník</b></td>
				</tr>
				<tr>
					<td style="width:50px;text-align:center;"><b>I.</b></td>
					<td style="width:50px;text-align:center;"><b>II.</b></td>
					<td style="width:50px;text-align:center;"><b>III.</b></td>
					<td style="width:50px;text-align:center;"><b>IV.</b></td>
					<td style="width:50px;text-align:center;"><b>V.</b></td>
					<td style="width:50px;text-align:center;"><b>VI.</b></td>
				</tr>
				<xsl:for-each select="zzz:predmety/zzz:Predmet">
				<xsl:if test="zzz:Nazov">
				<tr>
					<td><xsl:value-of select="zzz:Nazov"/></td>
					<td style="text-align:center;"><xsl:value-of select="zzz:prvy_rocnik"/></td>
					<td style="text-align:center;"><xsl:value-of select="zzz:druhy_rocnik"/></td>
					<td style="text-align:center;"><xsl:value-of select="zzz:treti_rocnik"/></td>
					<td style="text-align:center;"><xsl:value-of select="zzz:stvrty_rocnik"/></td>
					<td style="text-align:center;"><xsl:value-of select="zzz:piaty_rocnik"/></td>
					<td style="text-align:center;"><xsl:value-of select="zzz:siesty_rocnik"/></td>
				</tr>
				</xsl:if>
				</xsl:for-each>
				<tr>
					<td><b>Priemerný prospech</b> (uviesť na dve desatinné miesta)</td>
					<td style="text-align:center;font-weight:bold;"><xsl:value-of select="zzz:priemer/zzz:prvy_rocnik"/></td>
					<td style="text-align:center;font-weight:bold;"><xsl:value-of select="zzz:priemer/zzz:druhy_rocnik"/></td>
					<td style="text-align:center;font-weight:bold;"><xsl:value-of select="zzz:priemer/zzz:treti_rocnik"/></td>
					<td style="text-align:center;font-weight:bold;"><xsl:value-of select="zzz:priemer/zzz:stvrty_rocnik"/></td>
					<td style="text-align:center;font-weight:bold;"><xsl:value-of select="zzz:priemer/zzz:piaty_rocnik"/></td>
					<td style="text-align:center;font-weight:bold;"><xsl:value-of select="zzz:priemer/zzz:siesty_rocnik"/></td>
				</tr>
			</table>

			<div style="margin:30px;font-size:12px;width:650px;">
				<p><b>Pokyny a vysvetlivky na vyplňovanie prihlášky:</b></p>
			</div>

			<table class="no-borders">
			<tr class="no-borders">
				<td class="no-borders" valign="top"><sup>1)</sup></td>
				<td class="no-borders"><div align="justify">Prihláška je určená pre uchádzačov o vysokoškolské štúdium prvého stupňa (bakalárske štúdium)
					alebo spojeného prvého a druhého stupňa. Nehodiace sa prečiarknite.</div></td>
			</tr>
			<tr class="no-borders">
				<td class="no-borders" valign="top"><sup>2)</sup></td>
				<td class="no-borders"><div align="justify">V položke Pohlavie a Absolvovaná stredná škola vyznačte krížikom</div></td>
			</tr>
			<tr class="no-borders">
				<td class="no-borders" valign="top"><sup>3)</sup></td>
				<td class="no-borders"><div align="justify">V položke Rodné číslo u cudzinca sa zaznamenáva, ak mu bolo pridelené Ministerstvom vnútra SR.
					U cudzinca sa zaznamenáva aj miesto pobytu v Slovenskej republike.</div></td>
			</tr>
			<tr class="no-borders">
				<td class="no-borders" valign="top"><sup>4)</sup></td>
				<td class="no-borders"><div align="justify">Tel. č. a e-mailová adresa sú nepovinnými údajmi.</div></td>
			</tr>
			<tr class="no-borders">
				<td class="no-borders" valign="top"><sup>13)</sup></td>
				<td class="no-borders"><div align="justify">Uchádzač uvedie prospech zo štúdia na strednej škole v rozsahu, ktorý požaduje vysoká škola alebo fakulta.
					Uchádzač maturujúci v bežnom školskom roku nevyplní prospech v poslednom ročníku a v časti Klasifikácia
					maturitnej skúšky vyplní len stĺpec Predmet maturitnej skúšky a stĺpec Úroveň. Uchádzač maturujúci pred rokom
					2004 vyplní v časti Klasifikácia maturitnej skúšky stĺpec Predmet maturitnej skúšky, stĺpec Ústna forma (známka)
					v rámci internej časti, Dátum maturitnej skúšky a Priemer maturitnej skúšky.</div></td>
			</tr>
			</table>
		</body>
		<footer style="margin:30px;font-size:11px;">Ministerstvo školstva, vedy, výskumu a športu SR, 02/2019</footer>
	</html>
</xsl:template>

</xsl:stylesheet>
