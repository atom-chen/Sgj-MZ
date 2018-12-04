<?php 

class Equipment extends MY_Controller {
	const ERROR_PARAME = "003001";
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		load_model(array('equipment_model', 'user_model', 'character_model'));
	}
	public function equipment_list()
	{
		$user = $this->getSessionData("user");
		$equipment_model = new Equipment_model();
		$equipments = $equipment_model->get_list($user["id"]);
		if(!is_null($equipments)){
			$result = array("equipments"=>$equipments);
			$this->out($result);
		}else{
			$this->error("Equipment->equipment_list error");
		}
	}
	public function equip()
	{
		$user = $this->getSessionData("user");
		$character_id = $this->args["character_id"];
		$equipment_model = new Equipment_model();
		$equip_result = $equipment_model->equip($user["id"], $character_id, $this->args["equipment_id"]);
		if($equip_result){
			$equipments = $equipment_model->get_list($user["id"]);
			$result = array("equipments"=>$equipments);
			$this->out($result);
		}else{
			$this->error("Equipment->equip error");
		}
	}
}
