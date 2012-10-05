<?php
$name	= $_POST['name'];
$email  = $_POST['email'];
$msg	= $_POST['msg'];
$frmail = $_POST['frmail'];
$no 	= $_POST['no'];
$subject= "Arkadaþýnýzdan size bir öneri var. oyun.Sadecepaylas.com";
$mailtanim  = "MIME-Version: 1.0\r\n";									// bu kýsým tanýmlama kýsmý
$mailtanim .= "Content-type: text/plain;  charset=iso-8859-9\r\n";		// mailin karakter seti
$mailtanim .= "From: $name <$email>\r\n";   							// Mail'i açýnca kimden geldiði kýsmýnda yazacak olanlar
$mailtanim .= "Reply-To: $name <$email>\r\n";  							// Mail'i cevaplamak için cevabýn kime gideceðini içeren kýsým
$sms  = "Name :: ".$ad."\n\rE-Mail :: ".$email."\n\rSubject :: ".$konu;  		// Mailin içeriðinde, baþ tarafýna formdan gelen ad,email gibi bilgileri de ekler.
$sms .= "\n\rMessage ::".$msg."iþte adresi http://oyun.sadecepaylas.com/index.php?p=item&no=$no";   														// Ardýndan da mesajý ekler.


$submit = $_POST['submit'];   											// gönder butonuna basýlýp basýlmadýðýný öðrenmek için deðiþken alýnýr.

if (empty($submit) || !$email || !$name || !$subject || !$msg || !$frmail) 
{	
	echo "Boþluklarý doldurun";
} 

else 
{
		//mysql_query("insert into mails (tomail,frmail) values('$email','$frmail')");
		mail($frmail, $Subject ,stripslashes($sms), $mailtanim);  // Mail gönderme kodu. Ana kod satýrýmýz budur.
		header("Location: ../index.php?p=item&no=$no");
}
?>