<?php
	$link = mysql_connect('localhost','paylas_sharex','sananeamk');
	if (!$link)
	{
		die('Could not connect'.mysql_error());
	}
	$database = 'paylas_flashgames';
	@mysql_query("SET NAMES 'latin5'");
	mysql_select_db($database,$link) or die($database." database cannot found");
?>