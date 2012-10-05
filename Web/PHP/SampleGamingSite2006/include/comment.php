<?php
	include('db.php');
	$username = $_POST['username'];
	$comment = $_POST['comment'];
	$itemno = $_POST['itemno'];
	if(strlen($comment) > 1024)
	{
		$length = (strlen($comment));
		header("Location: ../index.php?p=index");
	}
	if(!empty($username) && !empty($comment) && !empty($itemno))
	{
		mysql_query("insert into comment(username, itemno, comment, date) values ('$username','$itemno', '$comment',now())");
		header("Location: ../index.php?p=item&no=$itemno");
	}
?>