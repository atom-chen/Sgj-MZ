<?php 
class Character_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_character_list($user_id, $chara_id = null){
		$select = "`id`,`user_id`, `character_id`,`fragment`, 
		`star`, `level`, `exp`, `horse`, `clothes`, `weapon`";
		$table = $this->user_db->characters;
		$where = array();
		$where[] = "user_id = {$user_id}";
		if($chara_id != null){
			if(is_array($chara_id)){
				$where[] = "character_id in (".implode(", ", $chara_id).") ";
			}else{
				$where[] = "character_id = {$chara_id}";
			}
		}
		
		$result_select = $this->master_db->select($select, $table, $where, null, null, Database_Result::TYPE_DEFAULT);
		$result = array();
		while ($row = mysql_fetch_assoc($result_select)) {
			$row["skills"] = $this->get_character_skills($user_id, $row["character_id"]);
			$result[] = $row;
		}
		return $result;
	}
	function get_character_skills($user_id, $character_id){
		$select = "id, character_id, skill_id, level";
		$table = $this->user_db->character_skill;
		$where = array();
		$where[] = "user_id = {$user_id}";
		$where[] = "character_id = {$character_id}";
		$order_by = "id asc";
		$result = $this->user_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function character_insert($values, &$is_new){
		$characters = $this->get_character_list($values["user_id"], $values["character_id"]);
		if(count($characters) > 0){
			$res = $this->update_character($values["user_id"], $values["character_id"], array("fragment"=>$characters[0]["Fragment"] + 1));
			$is_new = false;
		}else{
			$res = $this->user_db->insert($values, $this->user_db->characters);
			$is_new = true;
		}
		return $res;
	}
	function character_skill_insert($values){
		$res = $this->user_db->insert($values, $this->user_db->character_skill);
		return $res;
	}
	function update_character($user_id, $character_id, $args){
		if(!$args || !is_array($args))return false;
		$values = array();
		foreach ($args as $key=>$value){
			$values[] = $key ."=". $value;
		}
		$where = array("user_id={$user_id}", "character_id={$character_id}");
		$table = $this->user_db->characters;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
	function get_master_character_skills($character_id){
		$select = "character_id, skill_id, star, skill_point";
		$where = array("character_id = {$character_id}");
		$result = $this->master_db->select($select, $this->master_db->character_skill, $where);
		return $result;
	}

	function get_tutorial_characters($character_id=0){
		$user_characters = $this->get_user_characters_constant();
		$select = "`id`,1 as level,`horse`, `clothes`, `weapon`";
		$table = $this->master_db->base_character;
		$order_by = "id";
		$where = array("id>={$user_characters[0]}","id<={$user_characters[1]}");
		if($character_id > 0){
			$where[] = "id={$character_id}";
		}
		$result_select = $this->master_db->select($select, $table, $where, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result = array();
		while ($row = mysql_fetch_assoc($result_select)) {
			$skills = $this->get_master_character_skills($row["character_id"]);
			$skill = $skills[0];
			$row["skills"] = array(array("skill_id"=>$skill["skill_id"],"level"=>1));
			$result[] = $row;
		}
		return $result;
	}
	public function get_user_characters_constant(){
		$result = $this->master_db->select("`val`", $this->master_db->constant, array("`name`='user_characters'"), null, null, Database_Result::TYPE_ROW);
		return json_decode($result["val"], true);
	}
}
