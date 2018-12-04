<?php 

class Skill extends MY_Controller {
	const ERROR_PARAME = "003001";
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		load_model(array('skill_model', 'user_model', 'character_model'));
	}
	public function level_up()
	{
		$user = $this->getSessionData("user");
		$skill_id = $this->args["skill_id"];
		$skill_model = new Skill_model();
		$skill_result = $skill_model->level_up($user["id"], $skill_id);
		if($skill_result){
			$skill = $skill_model->get_skill($skill_id);
			$result = array("skill"=>$skill);
			$this->out($result);
		}else{
			$this->error("Skill->level_up error");
		}
	}
	public function unlock()
	{
		$user = $this->getSessionData("user");
		$user_id = $user["id"];
		$character_id = $this->args["character_id"];
		$skill_id = $this->args["skill_id"];
		load_model(array("master_model","item_model"));
		$skill_model = new Skill_model();
		$skill_result = $skill_model->unlock($user_id, $character_id, $skill_id);
		if($skill_result){
			$item_model = new Item_model();
			$items = $item_model->get_item_list($user_id);
			$character_model = new Character_model();
			$characters = $character_model->get_character_list($user_id, $character_id);
			$character = $characters[0];
			$result = array("skills"=>$character["Skills"], "item"=>$items);
			$this->out($result);
		}else{
			$this->error("Skill->unlock error");
		}
	}
	public function learn()
	{
		$user = $this->getSessionData("user");
		$user_id = $user["id"];
		$character_id = $this->args["character_id"];
		$item_id = $this->args["item_id"];
		load_model(array("master_model","item_model"));
		$skill_model = new Skill_model();
		$skill_result = $skill_model->learn($user_id, $character_id, $item_id);
		if($skill_result){
			$item_model = new Item_model();
			$items = $item_model->get_item_list($user_id);
			$character_model = new Character_model();
			$characters = $character_model->get_character_list($user_id, $character_id);
			$character = $characters[0];
			$result = array("skills"=>$character["Skills"], "item"=>$items);
			$this->out($result);
		}else{
			$this->error("Skill->learn error");
		}
	}
}
