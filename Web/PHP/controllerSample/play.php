<?php

/**
 * Partnercontroller
 * @package Application
 */

class Controller_Play extends Controller_Website {


	private static $public_actions = array('index','games', 'status', 'login', 'register','contact','forgot_password','reset_password','code','get_mail','create_a_partner');
	public $assets_folder = "platform";
	public $forced_local = "en-us";

	/**
	 * Registration and login functions start here
	 */

	///partner/ default page definition
	public function before() {
		parent::before();

		# Check, whether the current partner is valid, if action is protected
		if (!in_array($this->request->action(), self::$public_actions)) {
			if (!$this->check_login()) {
				throw new HTTP_Exception_403();
			}
		}
	}


	public function action_index() {
		$this->auto_render = false;
		$template = View::factory('platform/_layout');
		$template->title = "Plinga Play";
		$template->main = View::factory('platform/main');
		$template->games = View::factory('platform/games');
		$this->response->body($template);
	}

	private $current_partner, $current_user;

	private function check_login()
	{
		$user = Auth::instance()->get_user();
		if ($user != null) {
			$session = $this->get_session();
			if ($user->loaded() && $partner->loaded()) {
				$this->current_user = $user;
				return true;
			}
		}
		return false;
	}

	public function action_status()
	{
		$is_auth = true;//$this->check_login();
		$response = array('authenticated' => $is_auth);
		if($is_auth)
		{
			$user = Auth::instance()->get_user();
			$response = $this->add_userdata($response, $user);
		}
		$this->send_json($response);
	}

	public function action_games()
	{
		//		# in addition add a published parameter
		//		# i added it already into database and changed the get function of game model with respect of environment
		//		# if we have unpublished games still we want to show them on there but with a title like "coming soon" !
//		if(!Auth::instance()->logged_in())
//		{
//			throw new HTTP_Exception_404();
//		}

		$query = ORM::factory("game")->where("url","!=","")->where('published','=','1')->order_by('platform_order','asc');
		if (($last_id = $this->request->query('last_id')) != null) {
			$query->offset($last_id);
		}
		$games = $query->find_all();

		$games_data = array();

		$partner = Model_Partner::get('9');
		$active = true;
		foreach ($games as $game)
		{
			$integration_iframe = URL::site('game/partner_iframe/' . $game->id . '/9', true);
			
				 
			$demo_code = Helper_Integration::code(9, array('game_id' => $game->id, 'target_element' => 'demo_game'));
			$game_data = array(
				'id' => $game->id,
				'title' => __('partner_'.$game->backend_name.'_title'),
				'description' => __('partner_'.$game->backend_name.'_description'),
				'picture' => $game->gameLogo(),//$game->thumbnail_url,
				'published' => $game->published,
				'height' => $game->framework_height(),
				'active' => $active,
				'user_count' => $game->user_count_text,
				'promoMedium' => 'assets/img/game/'.$game->id.'/platform/promo-medium.png',
				'promoSmall' => 'assets/img/game/'.$game->id.'/platform/promo-small.png',

				'hotbox' => 'assets/img/game/'.$game->id.'/platform/hotbox.jpg',
				'icon' => 'assets/img/game/'.$game->id.'/platform/icon.png',
				
				'integration_iframe' => $integration_iframe,
				'middlePromo' => in_array($game->id, array(1, 7))
			//'integration_code' => self::generate_code($partner->id, array('game_id' => $game->id))
			);
			if($active)
				$active = false;
			
			$game_data['languages'] = array();
			$languages = Model_Language::get_locales_for_languages($game->supported_languages);
			foreach ($languages as $language) {
				$game_data['languages'][] = $language->as_array();
			}

			$games_data[] = $game_data;

		}
		//print_r($allGames);
		$this->send_json(array('games' => $games_data));
	}

	/**
	 * pages whichs are Seenable By not loged in users start here
	 */

	public function action_contact()
	{
		$message = $this->request->post('text');
		$email = $this->request->post('email');
		//Check mail address if is valid or not and return result !

		if(empty($email) || !valid::email($email)) {
			$arr = array('result' => false,'result_message'=>__('partner_reg_invalid_email'),'message'=>$message);
		}
		else if(empty($message))
		{
			$arr = array('result' => false,'result_message'=>__('partner_empty_message'));
		}
		else {
			// Send Mail to Support and return the result ! Tested And confirmed it is working on my local :D CC
			// $to = support@plinga.com
			$subject = "Plinga Play Partner MT";
			if(Helper_Mail::send("cantek.cetin@plinga.com"/*$to*/,$subject,$message,$email)){
				$arr = array('result' => true,'result_message'=>__('partner_successfuly_sent'));
			}
			else{
				$arr = array('result' => false,'result_message'=>__('partner_not_successful'));
			}
		}

		$this->send_json($arr);
	}


	/**
	 * pages whichs are Seenable By all users start here
	 */
	//Plinga Play About Page
	public function action_about()
	{

	}

	/**
	 * pages whichs are Seenable by By all users end here
	 */




	private function display($view) {
		$this->content = View::factory('partner/template');
		$partner = false;
		if(Session::instance()->get('is_partner', false))
		$partner = Model_Partner::current();
		$this->content->partner = $partner;
		$this->content->content = $view;
	}

	//SESSION HANDLING PART STARTS
	public function set_session($partner_id)
	{
		Session::instance()->set('loggedin_partner_id',$partner_id);
	}

	public function delete_session()
	{
		Session::instance()->delete('loggedin_partner_id');
	}

	public function get_session()
	{
		return array("partner_id"=>Session::instance()->get('loggedin_partner_id'));
	}
	//SESSION HANDLING PART ENDS

	/**
	 * Logout
	 */
	private function logout() {
		$this->delete_session();
		Auth::instance()->logout();
	}

	/**
	 * Add user data to response
	 */
	private function add_userdata($response, $user = null) {
		$partner_info = Model_Partnerinfo::get_by_user($user);
		if(!empty($user) && $partner_info->loaded())
		{
			$platforms_of_partner = ORM::factory('partner')->where('company_id','=',$partner_info->id)->find_all();
			$platforms_data = array();
			foreach ($platforms_of_partner as $platform)
			{
				$platform_data = array(
				'id' => $platform->id,
				'name' => $platform->platform_name,
				'platform_secret' => $platform->secret,
				'backend_name' => $platform->name,
				'url' => $platform->url
				);
				$platforms_data[] = $platform_data;
			}
			$response['user'] = array(
			'email' => $user->email,
			'username' => $user->username,
			'company_name' => $partner_info->partner_name,
			'partner_id' => $partner_info->id,
			'platforms' => $platforms_data,
			'platform' => false
			//'api_key' => $partner->secret
			);
		}
		return $response;
	}

	/**
	 * general send json response handler
	 */
	private function send_json($response, $error = false)
	{
		$response['status'] = $error ? 'error' : 'ok';
		$this->auto_render = false;
		$this->response->send_json($response);
	}
}
