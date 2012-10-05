<?php
	include('config.php');
	include('include/db.php');
	include('include/template.php');
	error_reporting(E_ALL ^ (E_NOTICE ^ E_WARNING));
	$tpl = new Template($config['theme']);
	$p = $_GET['p'];
	if(empty($p))
		$p='index';
function Ortak()
{
	global $tpl;
				$queryf = mysql_query("select * from category order by id");
				for($k = 0 ; $k < 9 ; $k++)
					{
								$tpl->assign_block_vars('BLOCKMENUF', array('NAME'=>mysql_result($queryf,$k,'name'),'URL'=>mysql_result($queryf,$k,'id')));
					}
}
	switch($p)
	{
		case 'index':
		{
				$query1 = mysql_query('select * from item order by lasthitspd');
				for($j = 0 ; $j < 5 ; $j++)
					{
							$tpl->assign_block_vars('BLOCKFLASH', array('TOPFLASHNAMES'=>mysql_result($query1,$j,'name'),'TOPFLASHIMG'=>mysql_result($query1,$j,'imageurl'),'TOPFLASHURL'=>mysql_result($query1,$j,'id')));
					}
				$query2 = mysql_query('select * from item order by id desc');
				for($j = 0 ; $j < 5 ; $j++)
					{
							$tpl->assign_block_vars('RANDOMF', array('NAME'=>mysql_result($query2,$j,'name'),'IMG'=>mysql_result($query2,$j,'imageurl'),'URL'=>mysql_result($query2,$j,'id')));
					}
					
				Ortak();
				$tpl->set_filenames(array('index'=>'site.tpl','index1'=>'index.tpl'));
				$tpl->assign_vars(array('TITLE'=>'Sadece Eðlenmek Ýçin - SADECE PAYLAÞ OYUN'));
				$tpl->assign_var_from_handle('BODY','index1');
				break;
		}
		case 'item':
		{
			$no = $_GET['no'];
			$query = mysql_query("select * from item where id = '$no'");
			$name = mysql_result($query,0,'name');
			$cat = mysql_result($query,0,'catid');
			$catname = mysql_query("select * from category where id = '$cat'");
				$querycom = mysql_query("select * from comment where itemno  = '$no' order by date desc");
				$com = 0;
				$rows = mysql_num_rows($querycom);
				if($rows>3)
				{
					for($com = 0 ; $com < 3; $com++)
					{
						$tpl->assign_block_vars('COMMENT',array('USERNAME'=>mysql_result($querycom,$com,'username'),'COMMENT'=>mysql_result($querycom,$com,'comment'),'DATE'=>mysql_result($querycom,$com,'date')));
					}
				}
				elseif(($rows<3)&&($rows>0))
				{
					for($com = 0 ; $com <$rows ; $com++)
					{
						$tpl->assign_block_vars('COMMENT',array('USERNAME'=>mysql_result($querycom,$com,'username'),'COMMENT'=>mysql_result($querycom,$com,'comment'),'DATE'=>mysql_result($querycom,$com,'date')));
					}
					
				}
				$hits =0;
				$hits = mysql_result($query,0,'hitspd');
				$hits++;
				$totalhit =0;
				$totalhit = mysql_result($query,0,'hits');
				$totalhit++;
				$querytothits = mysql_query("update item set hits = '$totalhit' where id = '$no'");
				$queryhits = mysql_query("update item set hitspd = '$hits' where id = '$no'");
				$url = mysql_result($query,0,'url');
				Ortak();
				$tpl->set_filenames(array('index'=>'site.tpl','game'=>'game.tpl'));
				$tpl->assign_vars(array('TITLE'=>$name,'URL'=>$url,'ITEMNO'=>$no,'HIT'=>$totalhit.' kere görüntülendi'));
				$tpl->assign_var_from_handle('BODY','game');
			break;
		}
		case 'category':
		{
			$no = $_GET['no'];
				if($_GET['s']!=0 && $_GET['s'] !="")
				{
					$start = $_GET['s'];
				}
				else
				{
					$start =0;
				}
			$cat = mysql_query("select * from category where id = '$no'");
			$hits = mysql_result($cat,0,'hitspd');
			$hits++;
			$queryhits = mysql_query("update category set hitspd = '$hits' where id = '$no'");
			$catname = mysql_result($cat,0,'name');
			$query = mysql_query("select * from item where catid = '$no' limit $start, 20");
			$query1 = mysql_query("select count(*) as toplam from item where catid = '$no'");
			$pagenum = mysql_result($query1,0,'toplam');
			$pagenum = ceil($pagenum)/25;
			$rows = mysql_num_rows($query);
			$maxim = mysql_result($query1,0,'toplam');
			$i=0;
			if($pagenum > 10 && $start < $maxim)
			{
				$loop = $start;
				$ileri = $start+20;
				$right = "<td><a href='../?p=category&no=$no&s=$ileri'>&raquo;</a></td>";
				$geri = $start-20;
				$left = "<td><a href='../?p=category&no=$no&s=$geri'>&laquo;</a></td>";
				$tpl->assign_vars(array('LEFT'=>$left,'RIGHT'=>$right));
				for ($page = ($loop/20)+1 ; $page <= ($loop/20)+11 ; $page++)
				{
					if($page >$pagenum)
						break;
					$tpl->assign_block_vars('NEXT', array('NAME'=>$page,'START'=>$start,'NO'=>$no));
					$start+=20;
				}
			}
			else if($pagenum <= 10)
			{
				for ($page = 1 ; $page <= $pagenum ; $page++)
				{
						$tpl->assign_block_vars('NEXT', array('NAME'=>$page,'START'=>$start,'NO'=>$no));
						$start+=20;
				}
			}
				for($r = 0; $r < 5 ; $r++)
				{
					$tpl->assign_block_vars('ROW',array());
					for($c = 0; $c < 5 ; $c++)
					{
						$imageurl = mysql_result($query,$i,'imageurl');
						$tpl->assign_block_vars('ROW.COL', array('TOPVIDEONAME'=>mysql_result($query,$i,'name'),'TOPVIDEOIMG'=>$imageurl,'TOPVIDEOURL'=>mysql_result($query,$i,'id')));
						$i++;
						$currentpage = mysql_result($query,$i,'no');
						if($i>=$rows)
						{
							$r=5;
							break;
						}
					}
				}
				Ortak();
				$tpl->set_filenames(array('index'=>'site.tpl','category'=>'category.tpl'));
				$tpl->assign_vars(array('TITLE'=>$catname,'HIT'=>$hits.' kere bakýldý'));
				$tpl->assign_var_from_handle('BODY','category');
			
			break;
		}
	}
	$tpl->pparse("index");
?>