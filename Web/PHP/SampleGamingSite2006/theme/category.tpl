<table align = center width = 80% cellpadding = 0 cellspacing = 0 >
	<!-- BEGIN ROW -->
	<tr>
		<!-- BEGIN COL -->
		<td align = center width = 50>
			<a href ="?p=item&no={ROW.COL.TOPVIDEOURL}" class = "iceride">|:{ROW.COL.TOPVIDEONAME}:|</a><br>
			</td></tr><tr><td align = center width = 50>
			<a href = "?p=item&no={ROW.COL.TOPVIDEOURL}"><img src = "{ROW.COL.TOPVIDEOIMG}" border = 0 >
		</td>
		<!-- END COL -->
	</tr>
	<!-- END ROW -->
</table>
<center><table width =40%>
	<tr>
			{LEFT}
<!-- BEGIN NEXT -->
		<td align = center>
			<a href = "?p=category&no={NEXT.NO}&s={NEXT.START}">{NEXT.NAME}.</a>
		</td>
<!-- END NEXT -->
{RIGHT}
	</tr>
</table>
