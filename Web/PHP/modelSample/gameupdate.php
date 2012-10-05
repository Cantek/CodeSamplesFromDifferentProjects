<?php

class Model_Gameupdate extends ORM {
	protected $_table_name = 'game_updates';

	protected $_primary_key = 'id';
	/**
	 * Validation
	 */
	public function rules()
	{
		return array(
	        'id' => array(
		)
		);
	}

	public static function get($game) 
	{
		$key = "game_update_".$game->id;
		if(($update = Cache::instance()->get($key)) == null) {
			$update = ORM::factory('gameupdate')->where('game_id', '=', $game->id)->and_where('update_enabled','=',"1")->find();
			$key = "game_update_".$game->id;
			if($update != null){
				Cache::instance()->set($key, $update);
			}
		}
		else
		{
			if($update->loaded())
			{
				$key = "game_update_".$game->id;
				if(!$update->loaded())
				{
					$update = null;
				}
				if($update != null){
					Cache::instance()->set($key, $update);
				}
			}
		}
		return $update;
	}
}
?>