<?php 
class User_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	var $_select_clums = 'id, name, nickname, face, level, battling_id as battlingId, gold, silver, ap, map_id as mapId, last_ap_date as lastApDate';
	var $_select_other_clums = 'id, name, nickname, face, level, gold, silver, ap, map_id as mapId, last_ap_date as lastApDate';
	function register($account, $password, $name, $character_id){
		$character_model = new Character_model();
		$characters = $character_model->get_tutorial_characters($character_id);
		if(count($characters) == 0){
			return false;
		}
		$character = $characters[0];
		$this->user_db->trans_begin();
		$values = array();
		$values['account'] = "'{$account}'";
		$values['pass'] = "'{$password}'";
		$values['name'] = "'{$name}'";
		$now = date("Y-m-d H:i:s");
		$values['register_time'] = "'{$now}'";
		$res_player = $this->user_db->insert($values, $this->user_db->player);
		if(!$res_player){
			$this->user_db->trans_rollback();
			$this->error("register fail player");
		}
		$user = $this->login(array("account"=>$account, "pass"=>$password));
		if(is_null($user)){
			$this->user_db->trans_rollback();
			$this->error("register fail user");
		}
		$user_id = $user["id"];
		/*
		$mission_model = new Mission_model();
		$missions = $mission_model->get_master_missions();
		foreach ($missions as $mission) {
			if($mission["id"] == Mission_model::TUTORIAL_ID){
				$res = $mission_model->set_new_mission($user,$mission);
				if(!$res){
					$this->user_db->trans_rollback();
					$this->error("mission fail");
				}
				break;
			}
		}*/
		$this->user_db->trans_commit();
		return $user_id;
	}
	function login($args){
		$table = $this->user_db->player;
		$where = array();
		$where[] = "account='{$args["account"]}'";
		$where[] = "pass='{$args["pass"]}'";
		$result = $this->user_db->select($this->_select_clums, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function get_user_from_account($account){
		$table = $this->user_db->player;
		$where = array();
		$where[] = "account='{$account}'";
		$result = $this->user_db->select($this->_select_clums, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function update($args){
		if(!$args || !is_array($args))return false;
		$values = array();
		foreach ($args as $key=>$value){
			if($key == "id")continue;
			$values[] = $key ."=". $value;
		}
		$user = $this->getSessionData("user");
		$where = array("id={$user["id"]}");
		$table = $this->user_db->player;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
	function update_silver(){
		return $this->update_bankbook('sum(silver) as silver');
	}
	function update_gold(){
		return $this->update_bankbook('sum(gold) as gold');
	}
	function update_money(){
		return $this->update_bankbook('sum(silver) as silver, sum(gold) as gold');
	}
	function update_bankbook($select){
		$user = $this->getSessionData("user");
		$table = $this->user_db->bankbook;
		$where = array("user_id={$user["id"]}");
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		if(is_null($result)){
			return false;
		}
		return $this->update($result);
	}
	function get($id, $is_all = false, $is_self = false){
		$user = $this->getSessionData("user");
		if(!is_null($user) && !$is_self){
			$is_self = (isset($id) && $id == $user["id"]);
		}
		$select = $is_self ? $this->_select_clums : $this->_select_other_clums;
		$table = $this->user_db->player;
		$where = array("id={$id}");
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		if(is_null($result)){
			return null;
		}
		if($is_self && $is_all){
			$result["progress"] = $this->get_story_progress($id);
		}
		return $result;
	}
	function get_top_map($user_id){
		$select = "id, user_id, num, tile_id, x, y, level";
		$table = $this->user_db->top_map;
		$where = array("user_id={$user_id}");
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
	function has_progress($progress,$key,$v=1) {
	    foreach($progress as $child) {
	        if($child["k"] == $key){
	        	return $child["v"] == $v;
	        }
	    }
	    return false;
	}
	function get_story_progress($user_id, $k=null){
		$select = "k, v";
		$table = $this->user_db->story_progress;
		$where = array("user_id={$user_id}");
		if(!is_null($k)){
			$where[] = "k='".$k."'";
		}
		$result = $this->user_db->select($select, $table, $where);
		return $result;
	}
	function set_story_progress($user_id,$k,$v){
		$values = array();
		$this->user_db->trans_begin();
		$all_progress = $this->get_story_progress($user_id, $k);
		if(count($all_progress) == 0){
			$values["k"] = "'{$k}'";
			$values["v"] = $v;
			$values["user_id"] = $user_id;
			$result = $this->user_db->insert($values, $this->user_db->story_progress);
		}else{
			$values[] = "v=".$v;
			$table = $this->user_db->story_progress;
			$where = array("user_id={$user_id}","k='{$k}'");
			$result = $this->user_db->update($values, $table, $where);
		}
		if(!$result){
			$this->user_db->trans_rollback();
			return false;
		}
		if($v){
			$user = $this->getSessionData("user");
			$mission_change = false;
			$mission_model = new Mission_model();
			$mission_result = $mission_model->progress_mission_change($user, $k, $v, $mission_change);
			if(!$mission_result){
				$this->user_db->trans_rollback();
				return false;
			}
			if($mission_change){
				$user["missions"]=$mission_model->get_mission_list($user["id"]);
				$this->setSessionData("user",$user);
			}
		}
		$this->user_db->trans_commit();
		return $result;
	}
	function set_contents($user_id, $contents){
		$item_update = false;
		$equipment_update = false;
		$character_update = false;
		$user_update = false;
		foreach($contents as $content){  
			$content_res = $this->set_content($user_id,$content,$item_update,$equipment_update,$character_update,$user_update);
			if(!$content_res){
				return false;
			}
		}
		$user = $this->getSessionData("user");
		if($item_update){
			$item_model = new Item_model();
			$items = $item_model->get_item_list($user["id"]);
			$user["items"] = $items;
		}
		if($equipment_update){
			$equipment_model = new Equipment_model();
			$equipments = $equipment_model->get_list($user["id"]);
			$user["equipments"] = $equipments;
		}
		if($character_update){
			$character_model = new Character_model();
			$characters = $character_model->get_character_list($user_id);
			$user["characters"] = $characters;
			$mission_change = false;
			$mission_model = new Mission_model();
			$mission_result = $mission_model->character_mission_change($user, $mission_change);
			if(!$mission_result){
				return false;
			}
			if($mission_change){
				$user["missions"]=$mission_model->get_mission_list($user["id"]);
			}
		}
		if($user_update){
			$new_user = $this->get($user_id,false,true);
			$user["gold"] = $new_user["gold"];
			$user["silver"] = $new_user["silver"];
			$user["ap"] = $new_user["ap"];
		}
		$this->setSessionData("user",$user);
		return true;
	}
	function set_content($user_id, $content, &$item_update, &$equipment_update, &$character_update,&$user_update){
		$res_get = false;
		switch($content["type"]){
			case "item":
				$item_model = new Item_model();
				$res_get = $item_model->set_item($user_id, $content["content_id"]);
				$item_update = true;
				break;
			case "horse":
			case "weapon":
			case "clothes":
				$equipment_model = new Equipment_model();
				$res_get = $equipment_model->set_equipment($user_id, $content["content_id"], $content["type"]);
				$equipment_update = true;
				break;
			case "character":
				$now = date("Y-m-d H:i:s");
				$character_values = array();
				$character_values['user_id'] = $user_id;
				$character_values['character_id'] = $content["content_id"];
				$character_values['register_time'] = "'{$now}'";
				$character_model = new Character_model();
				$is_new = false;
				$res_get = $character_model->character_insert($character_values, $is_new);
				if(!$res_get){
					break;
				}
				if($is_new){
					$skills = $character_model->get_master_character_skills($content["content_id"]);
					foreach ($skills as $skill) {
						$character_skills = array();
						$character_skills['user_id'] = $user_id;
						$character_skills['character_id'] = $skill["character_id"];
						$character_skills['skill_id'] = $skill["skill_id"];
						$character_skills['level'] = 1;
						$character_skills['register_time'] = "'".NOW."'";
						$res_get = $character_model->character_skill_insert($character_skills);
						if(!$res_get){
							break;
						}
					}
				}
				$character_update = true;
				break;
			case "gold":
				$get_type = isset($content["get_type"]) ? $content["get_type"] : "";
				$content_id = isset($content["content_id"]) ? $content["content_id"] : 0;
				$res_get = $this->set_money_log($user_id,$get_type,$content_id,$content["value"],0);
				$this->update_money();
				$user_update = true;
				break;
			case "silver":
				$get_type = isset($content["get_type"]) ? $content["get_type"] : "";
				$content_id = isset($content["content_id"]) ? $content["content_id"] : 0;
				$res_get = $this->set_money_log($user_id,$get_type,$content_id,0,$content["value"]);
				$this->update_money();
				$user_update = true;
				break;
			case "ap":
				break;
		}
		return $res_get;
	}
	function set_money_log($user_id, $type,$child_id, $gold, $silver){
		$values = array();
		$values['user_id'] = $user_id;
		$values['type'] = "'{$type}'";
		$values['child_id'] = $child_id;
		$values['gold'] = $gold;
		$values['silver'] = $silver;
		$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
		$res = $this->user_db->insert($values, $this->user_db->bankbook);
		return $res;
	}
}
