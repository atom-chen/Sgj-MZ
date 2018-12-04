<?php 
class Present extends MY_Controller {
	function __construct() {
		parent::__construct();
		load_model(array('user_model', 'character_model', 'present_model'));
	}
	public function present_list()
	{
		$present_model = new Present_model();
		$user = $this->getSessionData("user");
		$list = $present_model->get_list($user["id"]);
		$this->out(array("presents"=>$list));
	}
	public function receive()
	{
		load_model(array('item_model','equipment_model'));
		$present_model = new Present_model();
		$user = $this->getSessionData("user");
		$result = $present_model->receive($user["id"], $this->args["present_id"]);
		if($result){
			$user = $this->getSessionData("user");
			//$list = $present_model->get_list($user["id"]);
			$this->out(array("user"=>$user));
		}else{
			$this->error("Present->receive error");
		}
	}
}
