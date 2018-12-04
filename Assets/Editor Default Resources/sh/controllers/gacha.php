<?php 
class Gacha extends MY_Controller {
	function __construct() {
	//	$this->needAuth = true;
		parent::__construct();
		load_model(array('gacha_model','user_model','item_model','equipment_model'));
	}
	public function freelog()
	{
		$gacha_model = new Gacha_model();
		$logs = $gacha_model->get_free_logs();
		if($logs){
			$this->out(array("gachas"=>$logs));
		}else{
			$this->error("Gacha->freelog error");
		}
	}
	public function slot()
	{
		load_model(array('character_model'));
		$user = $this->getSessionData("user");
		$user_id = $user["id"];
		$gacha_id = $this->args["gacha_id"];
		$cnt = $this->args["cnt"];
		$gacha_model = new Gacha_model();
		$contents = $gacha_model->slot($this->args);
		$logs = $gacha_model->get_free_logs($gacha_id);
		$gacha = $logs[0];
		$user = $this->getSessionData("user");
		$user_model = new User_model();
		$new_user = $user_model->get($user_id);
		foreach ($new_user as $key => $value) {
			if(isset($user[$key])){
				$user[$key] = $value;
			}
		}
		$this->setSessionData("user", $user);
		if($contents){
			$this->out(array("contents"=>$contents, "gacha"=>$gacha, "user"=>$user));
		}else{
			$this->error("Gacha->slot error");
		}
	}
}
