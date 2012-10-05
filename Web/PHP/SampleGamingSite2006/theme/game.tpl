<table cellpadding=0 cellspacing=0 width=100% height=100%>
	<tr><td align = center width = 100% height = 100%>
		<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0" width="550" height="400">
                <param name="movie" value="{URL}">
                <param name="quality" value="high">
                <embed src="{URL}" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width = 550 height = 500></embed>
                </object>
	</td>
	</tr>
</table>
</td></tr><tr><td>

<table cellpadding=0 cellspacing=0 width=100%>
<tr><td width = 100%>
	<!-- BEGIN COMMENT -->
		<form><fieldset><legend><b>{COMMENT.USERNAME}</b></legend>
		<table cellpadding=0 cellspacing=0 width=100% height=100%>
			<tr><td align = right>
				<b>{COMMENT.COMMENT}</b>
			</td></tr>
			<tr><td>
				{COMMENT.DATE}
			</td></tr>
		</table>
		</fieldset></form>
	<!-- END COMMENT -->
</td></tr>

<tr><td width = 100%>
	<form action = "include/mail.php" method = "post"><fieldset><legend><b>Oyunu Arkadaþýna Öner</b></legend>
	<table cellpadding=0 cellspacing=0 width=100% height=100%>
		<tr><td width =20%>
			<b>Ýsim</b></td> <td width = 80% align = center> <input type = "text" name = "name"></td></tr>
			<tr><td width = 20%><b>E-Mail</b></td> <td width = 80% align = center> <input type = "text" name = "email"></td></tr>
			<tr><td width = 20%><b>Arkadaþýnýn E-Mail adresi</b></td> <td width = 80% align = center> <input type = "text" name = "frmail"></td></tr>
			<td width = 20%><b>Mesajýn</b></td> <td width = 80% align = center> <textarea name = "msg" rows=3 cols = 50> </textarea></td></tr><tr>
			<td width = 20%></td><td width = 100% align = center ><input type = "submit" name = "submit" value = "Mail gönder" cols = 30></td></tr>
			<tr><td><input type = "hidden" value="{ITEMNO}" name = "no"></td>
		</tr>
	</table>
	</fieldset></form>
</td></tr>
</table>

</td></tr><tr><td>
<table cellpadding=0 cellspacing=0 width=100%>
<tr><td width = 100%>
	<form action = "include/comment.php" method = "post"><fieldset><legend><b>Bu oyuna yorum yada diðer oyunculara yardýmcý olacak bilgiler yazabilirsiniz</b></legend>
	<table cellpadding=0 cellspacing=0 width=100% height=100%>
		<tr><td width =20%>
			<b>Ýsim</b></td> <td width = 80% align = center> <input type = "text" name = "username"></td></tr>
			<td width = 20%><b>Yorum</b></td> <td width = 80% align = center> <textarea name = "comment" rows=3 cols = 50> </textarea></td></tr><tr>
			<td width = 20%></td><td width = 100% align = center ><input type = "submit" name = "submit" value = "Yorumu Gönder" cols = 30></td></tr>
			<tr><td width = 20%></td><td width = 80%><input type = "hidden" value="{ITEMNO}" name = "itemno"></td>
		</tr>


		</td></tr>
	</table>
	</fieldset></form>
</td></tr>
</table>