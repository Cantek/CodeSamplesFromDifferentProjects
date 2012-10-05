<?php
include('include/db.php');
header("Content-type: text/xml\n\n");
function sem($text) {
    $text=str_replace(" ","+",trim($text));
	$text=str_replace("&","",trim($text));
    $text=preg_replace("@[^A-Za-z0-9\-_ĞÜŞİÖÇğüşıöç]+@i","",$text);
    $text=ereg_replace(" +"," ",trim($text));
    $text=ereg_replace("[-]+","_",$text);
    $text=ereg_replace("[_]+","_",$text);
    $text=strtolowerTR($text);
    if ((substr($text,-1)=='_')||(substr($text,-1)=='-')) $text=substr($text,0,-1);
    return $text;
}
function strtolowerTR($text) {
    $TRBul=array('Ğ','Ü','Ş','İ','Ö','Ç','ğ','ü','ş','ı','ö','ç');
    $TRDegistir=array('g','u','s','i','o','c','g','u','s','i','o','c');
    $text=str_replace($TRBul,$TRDegistir,$text);
    $text=strtolower($text);
    return $text;
}                            
///////////////veriler dökülüyor/////////////////////////////
echo '<?xml version="1.0" encoding="ISO-8859-9"?>
<?xml-stylesheet type="text/xsl" href="sitemap.xsl"?>
<urlset xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.sitemaps.org/schemas/sitemap/09/sitemap.xsd" xmlns=
"http://www.sitemaps.org/schemas/sitemap/0.9">';

$query = mysql_query("SELECT * FROM document ORDER BY id desc");
while($sp = mysql_fetch_assoc($query)) {
 $fname = sem($sp['name']);
 $catid = $sp['category'];
$sor2 = mysql_query("SELECT tbname FROM category WHERE id = '$catid'");
while($sGo = mysql_fetch_array($sor2))
{
$link = "http://www.sadecepaylas.com/item-".$sp[id]."-".$catid."-".$sGo[tbname]."-".$fname.".htm";
echo "
 <url>
  <loc>".$link."</loc>
  <lastmod>".$sp['eklenme']."T21:29:35+00:00</lastmod>
  <changefreq>yearly</changefreq>
  <priority>0.5</priority>
</url>";
}
}
echo "</urlset>";
?>