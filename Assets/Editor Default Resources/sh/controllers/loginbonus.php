<?php 
class Loginbonus extends MY_Controller {
	function __construct() {
		parent::__construct();
		load_model(array('user_model', 'character_model', 'loginbonus_model', 'version_model'));
	}
	public function log_count()
	{
		$loginbonus_model = new Loginbonus_model();
		$user = $this->getSessionData("user");
		$cnt = $loginbonus_model->get_log_count($user["id"]);
		$this->out(array("count"=>$cnt));
	}
	public function get_bonus()
	{
		load_model(array('item_model','equipment_model'));
		$loginbonus_model = new Loginbonus_model();
		$user = $this->getSessionData("user");
		$result = $loginbonus_model->get_loginbonus($user["id"]);
		if($result){
			$user = $this->getSessionData("user");
			$user["loginbonus_cnt"] = $user["loginbonus_cnt"] + 1;
			$user["loginbonus_received"] = 1;
			$this->setSessionData("user", $user);
			$this->out(array("user"=>$user));
		}else{
			$this->error("Loginbonus->get_bonus error");
		}
	}
}
