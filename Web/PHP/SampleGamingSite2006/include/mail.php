<?php
$name	= $_POST['name'];
$email  = $_POST['email'];
$msg	= $_POST['msg'];
$frmail = $_POST['frmail'];
$no 	= $_POST['no'];
$subject= "Arkada��n�zdan size bir �neri var. oyun.Sadecepaylas.com";
$mailtanim  = "MIME-Version: 1.0\r\n";									// bu k�s�m tan�mlama k�sm�
$mailtanim .= "Content-type: text/plain;  charset=iso-8859-9\r\n";		// mailin karakter seti
$mailtanim .= "From: $name <$email>\r\n";   							// Mail'i a��nca kimden geldi�i k�sm�nda yazacak olanlar
$mailtanim .= "Reply-To: $name <$email>\r\n";  							// Mail'i cevaplamak i�in cevab�n kime gidece�ini i�eren k�s�m
$sms  = "Name :: ".$ad."\n\rE-Mail :: ".$email."\n\rSubject :: ".$konu;  		// Mailin i�eri�inde, ba� taraf�na formdan gelen ad,email gibi bilgileri de ekler.
$sms .= "\n\rMessage ::".$msg."i�te adresi http://oyun.sadecepaylas.com/index.php?p=item&no=$no";   														// Ard�ndan da mesaj� ekler.


$submit = $_POST['submit'];   											// g�nder butonuna bas�l�p bas�lmad���n� ��renmek i�in de�i�ken al�n�r.

if (empty($submit) || !$email || !$name || !$subject || !$msg || !$frmail) 
{	
	echo "Bo�luklar� doldurun";
} 

else 
{
		//mysql_query("insert into mails (tomail,frmail) values('$email','$frmail')");
		mail($frmail, $Subject ,stripslashes($sms), $mailtanim);  // Mail g�nderme kodu. Ana kod sat�r�m�z budur.
		header("Location: ../index.php?p=item&no=$no");
}
?>