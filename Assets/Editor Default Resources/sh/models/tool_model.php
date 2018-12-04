<?php 
class Tool_model extends MY_Model
{
	var $base_map = "sh109_master.base_map";
	var $world = "sh109_master.world";
	var $area = "sh109_master.area";
	function __construct(){
		parent::__construct();
	}
	function set_stage($world_id, $stages){
		$this->master_db->trans_begin();
		$where = array("world_id='{$world_id}'");
		$res = $this->master_db->delete($this->area, $where);
		if(!$res){
			$this->master_db->trans_rollback();
			return false;
		}
		foreach ($stages as $world) {
			$values = array();
			$values["x"] = $world["x"];
			$values["y"] = $world["y"];
			$values["tile_id"] = $world["tile_id"];
			$values["world_id"] = $world_id;
			$res = $this->master_db->insert($values, $this->area);
			if(!$res){
				$this->master_db->trans_rollback();
				return false;
			}
		}
		$this->master_db->trans_commit();
		return true;
	}
	function set_world($worlds){
		$this->master_db->trans_begin();
		$sql = "TRUNCATE ".$this->world;
		$res = $this->master_db->selectSQL($sql);
		if(!$res){
			$this->master_db->trans_rollback();
			return false;
		}
		foreach ($worlds as $world) {
			$values = array();
			$values["id"] = $world["id"];
			$values["x"] = $world["x"];
			$values["y"] = $world["y"];
			$values["tile_id"] = $world["tile_id"];
			$values["map_id"] = $world["map_id"];
			$values["build_name_cn"] = "'".$world["build_name"]."'";
			$res = $this->master_db->insert($values, $this->world);
			if(!$res){
				$this->master_db->trans_rollback();
				return false;
			}
		}
		$this->master_db->trans_commit();
		return true;
	}
	function set_basemap($id, $width, $height, $tile_ids){
		$basemap = $this->get_basemap($id);
		if($basemap == null){
			$values = array();
			$values["id"] = $id;
			$values["width"] = $width;
			$values["height"] = $height;
			$values["tile_ids"] = "'".$tile_ids."'";
			return $this->master_db->insert($values, $this->base_map);
		}else{
			$values = array();
			$values[] = "width={$width}";
			$values[] = "height={$height}";
			$values[] = "tile_ids='{$tile_ids}'";
			$where = array();
			$where[] = "id={$id}";
			return $this->master_db->update($values, $this->base_map, $where);
		}
	}
	function get_basemap($id){
		$select = "id,width,height,tile_ids";
		$where = array();
		$where[] = "id={$id}";
		$result = $this->master_db->select($select, $this->base_map, $where);
		return $result;
	}
	
	function delete_bankbook($user_id){
		return $this->user_db->delete($this->user_db->bankbook, array("user_id={$user_id}"));
	}
	function delete_battle_list($user_id){
		return $this->user_db->delete($this->user_db->battle_list, array("user_id={$user_id}"));
	}
	function delete_characters($user_id){
		return $this->user_db->delete($this->user_db->characters, array("user_id={$user_id}"));
	}
	function delete_character_skill($user_id){
		return $this->user_db->delete($this->user_db->character_skill, array("user_id={$user_id}"));
	}
	function delete_gacha_free_log($user_id){
		return $this->user_db->delete($this->user_db->gacha_free_log, array("user_id={$user_id}"));
	}
	function delete_item($user_id){
		return $this->user_db->delete($this->user_db->item, array("user_id={$user_id}"));
	}
	function delete_login_bonus($user_id){
		return $this->user_db->delete($this->user_db->login_bonus, array("user_id={$user_id}"));
	}
	function delete_mission($user_id){
		return $this->user_db->delete($this->user_db->mission, array("user_id={$user_id}"));
	}
	function delete_present($user_id){
		return $this->user_db->delete($this->user_db->present, array("user_id={$user_id}"));
	}
	function delete_story_progress($user_id){
		return $this->user_db->delete($this->user_db->story_progress, array("user_id={$user_id}"));
	}
	function delete_top_map($user_id){
		return $this->user_db->delete($this->user_db->top_map, array("user_id={$user_id}"));
	}
	function delete_equipment($user_id){
		return $this->user_db->delete($this->user_db->equipment, array("user_id={$user_id}"));
	}
	function delete_player($user_id){
		return $this->user_db->delete($this->user_db->player, array("id={$user_id}"));
	}
}
