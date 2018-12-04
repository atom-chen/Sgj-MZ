<?php 
class Mission extends MY_Controller {
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		load_model(array('character_model', 'item_model', 'equipment_model'));
	}
	public function mission_list()
	{
		$user = $this->getSessionData("user");
		$mission_model = new Mission_model();
		$missions = $mission_model->get_mission_list($user["id"]);
		if($missions){
			$result = array("missions"=>$missions);
			$this->out($result);
		}else{
			$this->error("Mission->mission_list error");
		}
	}
	public function complete()
	{
		$user = $this->getSessionData("user");
		$missionId = $this->args["mission_id"];
		$missions = $user["missions"];
		$mission = null;
		foreach ($missions as $key=>$child) {
			if($child["MissionId"] == $missionId){
				$mission = $child;
				break;
			}
		}
		if(is_null($mission) || $mission["Status"] != MissionStatus::clear){
			$this->error("Mission->exist error");
		}
		$mission_model = new Mission_model();
		$result = $mission_model->complete($user, $mission);
		if($result){
			$user = $this->getSessionData("user");
			$result = array("user"=>$user);
			$this->out($result);
		}else{
			$this->error("Mission->complete error");
		}
	}
}
