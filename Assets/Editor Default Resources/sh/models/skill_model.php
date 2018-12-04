<?php 
class Skill_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_skill($id){
		$select = 'id, skill_id, level';
		$table = $this->user_db->character_skill;
		$where = array();
		$where[] = "id={$id}";
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function get_master($skill_id, $level){
		$select = "`id`,`level`,`character_level`,`price`";
		$table = $this->master_db->skill;
		$where = array();
		$where[] = "id={$skill_id}";
		$where[] = "level={$level}";
		$result = $this->master_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function set_levelup_log($user_id, $child_id, $silver){
		$values = array();
		$values['user_id'] = $user_id;
		$values['type'] = "'skill_levelup'";
		$values['child_id'] = $child_id;
		$values['gold'] = 0;
		$values['silver'] = -$silver;
		$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
		$res = $this->user_db->insert($values, $this->user_db->bankbook);
		return $res;
	}
	function learn($user_id, $character_id, $item_id){
		$item_model = new Item_model();
		$item = $item_model->get_item_from_id($item_id);
		if(is_null($item) || $item["user_id"] != $user_id || $item["cnt"] < 1){
			$this->error("item cnt error");
			return false;			
		}
		$master_model = new Master_model();
		$items = $master_model->get_master_items("cn", $item["item_id"]);
		if(count($items) == 0){
			$this->error("item master error");
			return false;
		}
		$itemMaster = $items[0];
		if($itemMaster["item_type"] != "skillBook"){
			$this->error("item_type error");
			return false;
		}
		$character_skills = $master_model->get_character_skills($character_id);
		
		foreach ($character_skills as $child) {
			if($child["character_id"] == $character_id && $child["skill_id"] == $itemMaster["child_id"]){
				$this->error("skill exists error");
			}
		}
		//TODO::条件
		
		$this->user_db->trans_begin();
		$res_item = $item_model->remove_item($item, 1);
		if(!$res_item){
			$this->user_db->trans_rollback();
			$this->error("remove item error");
		}
		$character_skills = array();
		$character_skills['user_id'] = $user_id;
		$character_skills['character_id'] = $character_id;
		$character_skills['skill_id'] = $itemMaster["child_id"];
		$character_skills['level'] = 1;
		$character_skills['register_time'] = "'{$now}'";
		$character_model = new Character_model();
		$res_character = $character_model->character_skill_insert($character_skills);
		if(is_null($res_character)){
			$this->user_db->trans_rollback();
			$this->error("character skill insert error");
		}
		$this->user_db->trans_commit();
		return true;
	}
	function unlock($user_id, $character_id, $skill_id){
		$master_model = new Master_model();
		$character_skills = $master_model->get_character_skills($character_id);
		
		$skill = null;
		foreach ($character_skills as $child) {
			if($child["character_id"] == $character_id && $child["skill_id"] == $skill_id){
				$skill = $child;
				break;
			}
		}
		if(is_null($skill)){
			return false;
		}
		$character_model = new Character_model();
		$characters = $character_model->get_character_list($user_id, $character_id);
		$character = $characters[0];
		if($character["star"] < $skill["star"]){
			return false;
		}
		$item_model = new Item_model();
		$item = $item_model->get_item($user_id, Item_model::SKILL_POINT_ITEM_ID);
		if(is_null($item) || $item["cnt"] < $skill["skill_point"]){
			return false;			
		}
		$this->user_db->trans_begin();
		$res_item = $item_model->remove_item($item, $skill["skill_point"]);
		if(!$res_item){
			$this->user_db->trans_rollback();
			$this->error("remove skill point error");
		}
		$character_skills = array();
		$character_skills['user_id'] = $user_id;
		$character_skills['character_id'] = $character_id;
		$character_skills['skill_id'] = $skill_id;
		$character_skills['level'] = 1;
		$character_skills['register_time'] = "'{$now}'";
		$res_character = $character_model->character_skill_insert($character_skills);
		if(is_null($res_character)){
			$this->user_db->trans_rollback();
			$this->error("character skill insert error");
		}
		$this->user_db->trans_commit();
		return true;
	}
	function level_up($user_id, $id){
		$skill = $this->get_skill($id);
		if($skill == null){
			$this->error("get_skill error");
			return false;
		}
		$skillMaster = $this->get_master($skill["skill_id"], $skill["level"]);
		if($skillMaster == null){
			$this->error("get_master error");
			return false;
		}
		$user = $this->getSessionData("user");
		if($user["silver"] < $skillMaster["price"]){
			$this->error("Silver error");
			return false;
		}
		$this->user_db->trans_begin();
		$use_money = $this->set_levelup_log($user_id, 0, $skillMaster["price"]);
		if($use_money == null){
			$this->user_db->trans_rollback();
			$this->error("use money error");
		}
		$user_model = new User_model();
		$user_silver_update = $user_model->update_silver();
		if($user_silver_update == null){
			$this->user_db->trans_rollback();
			$this->error("silver update error");
		}
		$skill_up = $this->update($id,array("level"=>$skill["level"] + 1));
		$this->user_db->trans_commit();
		return true;
	}
	function update($id, $args){
		if(!$args || !is_array($args))return false;
		$values = array();
		foreach ($args as $key=>$value){
			if($key == "id")continue;
			$values[] = $key ."=". $value;
		}
		$where = array("id={$id}");
		$table = $this->user_db->character_skill;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
}
