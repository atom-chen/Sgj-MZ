<?php 
//die(dirname(__FILE__));
class Master extends MY_Controller {
	function __construct() {
		//$this->needAuth = true;
		parent::__construct();
		load_model(array("master_model"));
	}
	private function master_data($args)
	{
		$this->log("master_data");
		$result = array();
		$master_model = new Master_model();
		if($args["story_progress"]){
			$master_story_progress = $master_model->get_master_story_progress();
			$result["story_progress_keys"]=$master_story_progress;
		}
		if($args["constant"]){
			$master_constant = $master_model->get_master_constant();
			$result["constant"]=$master_constant;
		}
		if($args["scenario"]){
			list($master_scenarios,$master_scenario_ids) = $master_model->get_master_scenarios();
			//$master_scenarios = $master_model->get_master_scenarios();
			$result["scenarios"]=$master_scenarios;
			$result["scenario_ids"]=$master_scenario_ids;
		}
		if($args["mission"]){
			$master_mission = $master_model->get_master_mission();
			$result["missions"]=$master_mission;
		}
		if($args["exp"]){
			$master_exp = $master_model->get_master_exp();
			$result["exps"]=$master_exp;
		}
		if($args["character_star"]){
			$master_character_stars = $master_model->get_character_stars();
			$result["character_stars"]=$master_character_stars;
		}
		if($args["building"]){
			$master_building = $master_model->get_master_building();
			$result["buildings"]=$master_building;
		}
		if($args["tutorial"]){
			$master_tutorial = $master_model->get_master_tutorial();
			$result["tutorials"]=$master_tutorial;
		}
		if($args["tile"]){
			$master_tile = $master_model->get_master_tile();
			$result["tiles"]=$master_tile;
		}
		if($args["base_map"]){
			$master_base_map	 = $master_model->get_master_base_map();
			$result["base_maps"]=$master_base_map;
		}
		if($args["world"]){
			$master_world	 = $master_model->get_master_world();
			$result["worlds"]=$master_world;
		}
		if($args["area"]){
			$master_area	 = $master_model->get_master_area();
			$result["areas"]=$master_area;
		}
		if($args["character"]){
			$master_character = $master_model->get_master_character();
			$result["characters"]=$master_character;
		}
		if($args["horse"]){
			$master_horse = $master_model->get_master_horse();
			$result["horses"]=$master_horse;
		}
		if($args["weapon"]){
			$master_weapon = $master_model->get_master_weapon();
			$result["weapons"]=$master_weapon;
		}
		if($args["clothes"]){
			$master_clothes = $master_model->get_master_clothes();
			$result["clothes"]=$master_clothes;
		}
		if($args["skill"]){
			$master_skill = $master_model->get_master_skill();
			$result["skills"]=$master_skill;
		}
		if($args["item"]){
			$master_item = $master_model->get_master_items();
			$result["items"]=$master_item;
		}
		if($args["gacha"]){
			$master_gacha = $master_model->get_master_gachas();
			$result["gachas"]=$master_gacha;
		}
		if($args["word"]){
			$master_word = $master_model->get_master_words();
			$result["words"]=$master_word;
		}
		if($args["npc"]){
			$master_npc = $master_model->get_master_npcs();
			$result["npcs"]=$master_npc;
		}
		if($args["loginbonus"]){
			$master_loginbonus = $master_model->get_master_loginbonus();
			$result["loginbonus"]=$master_loginbonus;
		}
		if($args["npc_equipment"]){
			$master_npc_equipment = $master_model->get_master_npc_equipments();
			$result["npc_equipments"]=$master_npc_equipment;
		}
		if($args["strategy"]){
			$master_strategy = $master_model->get_master_strategys();
			$result["strategys"]=$master_strategy;
		}
		if($args["battlefield"]){
			$master_battlefield = $master_model->get_master_battlefields();
			$result["battlefields"]=$master_battlefield;
		}
		if($args["shop"]){
			$master_shop = $master_model->get_master_shops();
			$result["shop_items"]=$master_shop;
		}
		
		return $result;
	}
	public function alldata()
	{
		$master_data = $this->master_data($this->args);
		if(!is_null($master_data)){
			$this->out($master_data);
		}else{
			$this->error("Master->all error");
		}
	}
	public function version()
	{
		load_model(array("version_model"));
		$result = array();
		$version_model = new Version_model();
		$versions = $version_model->get_master();
		$result["versions"]=$versions;
		$this->out($result);
	}
}
