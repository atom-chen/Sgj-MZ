<?php 
class User_DB extends Base_Database {
	var $player = "ch_user.player";
	var $top_map = "ch_user.top_map";
	var $item = "ch_user.item";
	var $characters = "ch_user.characters";
	var $character_skill = "ch_user.character_skill";
	var $bankbook = "ch_user.bankbook";
	var $world = "ch_user.world";
	var $area = "ch_user.area";
	var $stage = "ch_user.stage";
	var $equipment = "ch_user.equipment";
	var $gacha_free_log = "ch_user.gacha_free_log";
	var $battle_list = "ch_user.battle_list";
	var $story_progress = "ch_user.story_progress";
	var $login_bonus = "ch_user.login_bonus";
	var $present= "ch_user.present";
	var $mission= "ch_user.mission";
	function __construct() {
		parent::__construct();
		$db = mysql_select_db('ch_user', $this->connect);
		if (!$db){
			throw new Exception('' + mysql_error());
			die('');
		}
	}
}
