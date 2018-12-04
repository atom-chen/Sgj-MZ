<?php 
class Master_DB extends Base_Database {
	var $base_character = "ch_master.base_character";
	var $constant = "ch_master.constant";
	var $building = "ch_master.building";
	var $tile = "ch_master.tile";
	var $base_map = "ch_master.base_map";
	var $world = "ch_master.world";
	var $area = "ch_master.area";
	var $horse = "ch_master.horse";
	var $weapon = "ch_master.weapon";
	var $clothes = "ch_master.clothes";
	var $gacha = "ch_master.gacha";
	var $gacha_characters = "ch_master.gacha_characters";
	var $gacha_child = "ch_master.gacha_child";
	var $gacha_price = "ch_master.gacha_price";
	var $skill = "ch_master.skill";
	var $item = "ch_master.item";
	var $version = "ch_master.version";
	var $word = "ch_master.word";
	var $battlefield = "ch_master.battlefield";
	var $battlefield_tile = "ch_master.battlefield_tile";
	var $battlefield_npc = "ch_master.battlefield_npc";
	var $battlefield_own = "ch_master.battlefield_own";
	var $npc = "ch_master.npc";
	var $npc_equipment = "ch_master.npc_equipment";
	var $character_skill = "ch_master.character_skill";
	var $character_star = "ch_master.character_star";
	var $tutorial = "ch_master.tutorial";
	var $avatar_setting = "ch_master.avatar_setting";
	var $login_bonus = "ch_master.login_bonus";
	var $shop = "ch_master.shop";
	var $battlefield_reward = "ch_master.battlefield_reward";
	var $exp = "ch_master.exp";
	var $strategy = "ch_master.strategy";
	var $mission = "ch_master.mission";
	var $scenario = "ch_master.scenario";
	var $story_progress = "ch_master.story_progress";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('ch_master', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
