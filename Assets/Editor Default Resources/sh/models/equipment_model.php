<?php 
class Equipment_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function set_equipment($user_id, $equipment_id, $equipment_type, $character_id = 0){
		$values = array();
		$values["user_id"] = "{$user_id}";
		$values["equipment_id"] = "{$equipment_id}";
		$values["equipment_type"] = "'{$equipment_type}'";
		$values["register_time"] = "'".NOW."'";
		if($character_id > 0){
			$values["character_id"] = "{$character_id}";
		}
		return $this->user_db->insert($values, $this->user_db->equipment);
	}
	
	function get_list($user_id, $chara_id = null){
		$select = "id,character_id,equipment_id,equipment_type";
		$table = $this->user_db->equipment;
		$where = array();
		$where[] = "user_id={$user_id}";
		if($chara_id != null){
			$where[] = "character_id={$chara_id}";
		}
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
	function update($id, $args){
		if(!$args || !is_array($args))return false;
		$values = array();
		foreach ($args as $key=>$value){
			if($key == "id")continue;
			$values[] = $key ."=". $value;
		}
		$where = array("id={$id}");
		$table = $this->user_db->equipment;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
	function equip_remove($user_id, $character_id, $equipment_type){
		$equiped_equipment = $this->get_equiped_equipment($user_id, $character_id, $equipment_type);
		if(!is_null($equiped_equipment)){
			return $this->update($equiped_equipment["Id"], array("character_id"=>0));
		}
		return true;
	}
	function equip($user_id, $character_id, $equipment_id){
		$equipment = $this->get_equipment_by_id($equipment_id);
		$this->user_db->trans_begin();
		$delete_old = $this->equip_remove($user_id, $character_id, $equipment["equipment_type"]);
		if(!$delete_old){
			$this->user_db->trans_rollback();
			$this->error("delete equip error".$this->user_db->last_sql);
		}
		$equip_new = $this->update($equipment["Id"], array("character_id"=>$character_id));
		if(!$equip_new){
			$this->user_db->trans_rollback();
			$this->error("equip new error");
		}
		$character_model = new Character_model();
		$character_args = array();
		$character_args[$equipment["equipment_type"]] = $equipment["equipment_id"];
		$character_equip = $character_model->update_character($user_id, $character_id, $character_args);
		if(!$character_equip){
			$this->user_db->trans_rollback();
			$this->error("character equip error ". $this->user_db->last_sql);
		}
		$this->user_db->trans_commit();
		return true;
	}
	function get_equipment_by_id($id){
		$select = "id,character_id,equipment_id,equipment_type";
		$table = $this->user_db->equipment;
		$where = array();
		$where[] = "id={$id}";
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function get_equiped_equipment($user_id, $character_id, $equipment_type){
		$select = "id,character_id,equipment_id,equipment_type";
		$table = $this->user_db->equipment;
		$where = array();
		$where[] = "user_id={$user_id}";
		$where[] = "character_id={$character_id}";
		$where[] = "equipment_type='{$equipment_type}'";
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
}
