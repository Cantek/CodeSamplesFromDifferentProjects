<table cellpadding=0 cellspacing=0 width=100% height=100%>
	<tr><td align = center>
		<embed SRC="{URL}" WIDTH=550 HEIGHT=400 AUTOPLAY=true CONTROLLER=true LOOP=false>
	</td></tr>
</table>
</td></tr><tr><td>

<table cellpadding=0 cellspacing=0 width=100%>
<tr><td width = 100%>
	<form action = "include/mail.php" method = "post"><fieldset><legend><b>Recommend This video to your Friend</b></legend>
	<table cellpadding=0 cellspacing=0 width=100% height=100%>
		<tr><td width =20%>
			<b>Name</b></td> <td width = 80% align = center> <input type = "text" name = "name"></td></tr>
			<tr><td width = 20%><b>E-Mail</b></td> <td width = 80% align = center> <input type = "text" name = "mail"></td></tr>
			<tr><td width = 20%><b>Friend's E-Mail</b></td> <td width = 80% align = center> <input type = "text" name = "frmail"></td></tr>
			<td width = 20%><b>Message</b></td> <td width = 80% align = center> <textarea name = "msg" rows=3 cols = 50> </textarea></td></tr><tr>
			<td width = 20%></td><td width = 100% align = center ><input type = "submit" name = "submit" value = "Send My Mail" cols = 30></td></tr>
			<tr><td><input type = "hidden" value="{ITEMNO}" name = "itemno"></td>
		</tr>
	</table>
	</fieldset></form>
</td></tr>
</table>
</td></tr><tr><td>

<table cellpadding=0 cellspacing=0 width=100%>
<tr><td width = 100%>
	<form action = "include/comment.php" method = "post"><fieldset><legend><b>Add Your comments about this Video</b></legend>
	<table cellpadding=0 cellspacing=0 width=100% height=100%>
		<tr><td width =20%>
			<b>Name</b></td> <td width = 80% align = center> <input type = "text" name = "username"></td></tr>
			<td width = 20%><b>Comment</b></td> <td width = 80% align = center> <textarea name = "comment" rows=3 cols = 50> </textarea></td></tr><tr>
			<td width = 20%></td><td width = 100% align = center ><input type = "submit" name = "submit" value = "Send My comment" cols = 30></td></tr>
			<tr><td><input type = "hidden" value="{ITEMNO}" name = "itemno"></td>
		</tr>
	</table>
	</fieldset></form>
</td></tr>
<tr><td>
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
</table>