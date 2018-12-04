<?php 

class Battle extends MY_Controller {
	const ERROR_PARAME = "003001";
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		load_model(array('battle_model'));
	}
	public function battle_list()
	{
		$user = $this->getSessionData("user");
		$battle_model = new Battle_model();
		$battle_list = $battle_model->get_list($user["id"]);
		if(!is_null($battle_list)){
			$result = array("battlelist"=>$battle_list);
			$this->out($result);
		}else{
			$this->error("Battle->battle_list error");
		}
	}
	public function battle_start()
	{
		$battlefield_id = $this->args["battlefield_id"];
		$user = $this->getSessionData("user");
		if(/*$user["BattlingId"] > 0 || */$battlefield_id == 0 || !is_numeric($battlefield_id)){
			$this->error("Battle->start error");
		}
		$battle_model = new Battle_model();
		$result_start = $battle_model->battle_start($battlefield_id);
		if($result_start){
			//$result = array();
			$this->out(array());
		}else{
			$this->error("Battle->start error");
		}
	}
	public function battle_end()
	{
		$this->checkParam(array("character_ids","die_ids", "star"));
		$character_ids = explode(",",$this->args["character_ids"]);
		$die_ids = explode(",",$this->args["die_ids"]);
		$star = $this->args["star"];
		$user = $this->getSessionData("user");
		if($user["BattlingId"] == 0){
			$this->error("Battle->battle_end error");
		}
		load_model(array('character_model', 'item_model', 'equipment_model'));
		$battle_model = new Battle_model();
		$BattlingId = $user["BattlingId"];
		$battle_rewards = $battle_model->battle_end($character_ids, $die_ids, $star);
		if(!is_null($battle_rewards)){
			$user = $this->getSessionData("user");
			$result = array("battle_rewards"=>$battle_rewards, "user"=>$user);
			$this->out($result);
		}else{
			$this->error("Battle->battle_list error");
		}
	}
	public function battle_reset()
	{
		$user = $this->getSessionData("user");
		if($user["BattlingId"] == 0){
			$this->out(array());
		}
		$user_model = new User_model();
		$res = $user_model->update(array("id"=>$user["id"], "battling_id"=>0));
		if($res){
			$user["BattlingId"] = 0;
			$this->setSessionData("user", $user);
			$result = array("user"=>$user);
			$this->out($result);
		}else{
			$this->error("Battle->battle_reset error");
		}
	}
}
