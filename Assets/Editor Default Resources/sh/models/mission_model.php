<?php 
class MissionStatus{
	const init = "init";
	const clear = "clear";
	const complete = "complete";
}
class Mission_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	const TUTORIAL_ID = 1;
	function get_master_missions($parent_id=0){
		$select = "`id`,`mission_type`,`start_time`, `end_time`, `parent_id`, `battle_id`, 
		`story_progress`, `level`, `character_count`, `battle_count`, `gold_count` ,`rewards`";
		$where = null;
		if($parent_id > 0){
			$where = array("parent_id={$parent_id}");
		}
		$table =  $this->master_db->mission;
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
	function get_mission_list($user_id, $is_all = false){
		$select = "`id` as Id, `user_id` as UserId, `mission_id` as MissionId, `status` as Status, `counts` as Counts, `update_time`";
		$table = $this->user_db->mission;
		$where = array("`user_id`={$user_id}");
		if(!$is_all){
			$where[] = "`status`<>'complete'";
		}
		$order_by = "id asc";
		$result = $this->user_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function get_master_mission($masters, $mission_id){
		foreach ($masters as $mission) {
			if($mission["id"] == $mission_id){
				return $mission;
			}
		}
		return null;
	}
	function complete($user, $mission){
		$mission_masters = $this->getSessionData("mission_masters");
		$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
		$contents = array();
		$rewards = json_decode($mission_master["rewards"],true);
		foreach ($rewards as $content) {
			$content["get_type"] = "mission";
			$contents[] = $content;
		}
		$this->user_db->trans_begin();
		$user_model = new User_model();
		$contents_res = $user_model->set_contents($user["id"],$contents);
		if(!$contents_res){
			$this->user_db->trans_rollback();
			$this->error("set contents error ".$this->user_db->last_sql);
		}
		$update_res = $this->update($mission["Id"], array("status"=>"'".MissionStatus::complete."'"));
		if(!$update_res){
			$this->user_db->trans_rollback();
			$this->error("mission complete error ");
		}
		if($mission["MissionId"] == 1){
			$new_missions = $this->get_master_missions($mission["MissionId"]);
			foreach($new_missions as $new_mission){
				$new_res = $this->set_new_mission($user, $new_mission);
				if(!$new_res){
					$this->user_db->trans_rollback();
					$this->error("set new mission error ");
				}
			}
			$mission_change = false;
			$this->mission_init($user,$mission_change);
		}
		$user = $this->getSessionData("user");
		$user["missions"]=$this->get_mission_list($user["id"]);
		$this->setSessionData("user",$user);
		$this->user_db->trans_commit();
		return true;
	}
	function mission_plus($user, $missionId){
		if($missionId == 1){
			return true;
		}
		$new_missions = $this->get_master_missions($missionId);
		foreach($new_missions as $new_mission){
			$new_res = $this->set_new_mission($user, $new_mission);
			if(!$new_res){
				return false;
			}
		}
		return true;
	}
	function mission_init($user, &$mission_change){
		$mission_masters = $this->getSessionData("mission_masters");
		$mission_change = false;
		foreach ($mission_masters as $mission_master) {
			if($mission_master["start_time"] > NOW || $mission_master["end_time"] < NOW){
				continue;
			}
			if($mission_master["mission_type"] == "normal" || $mission_master["mission_type"] == "events"){
				$user_mission = $this->get_user_mission($user["missions"], $mission_master["id"]);
				if(!is_null($user_mission)){
					continue;
				}
				if($mission_master["parent_id"] > 0){
					$parent_mission = $this->get_user_mission($user["missions"], $mission_master["parent_id"]);
					if(is_null($parent_mission) || $parent_mission["Status"] == MissionStatus::init){
						continue;
					}
				}
				$mission_change = true;
				$res = $this->set_new_mission($user, $mission_master);
				if(!$res){
					return false;
				}
			}else if($mission_master["mission_type"] == "daily"){
				if($mission_master["parent_id"] > 0){
					continue;
				}
				$user_mission = $this->get_user_mission($user["missions"], $mission_master["id"]);
				if(is_null($user_mission)){
					$this->set_new_mission($user, $mission_master);
					continue;
				}
				if($user_mission["update_time"] < DAY_START){
					$res = $this->update($user_mission["Id"], array("status"=>MissionStatus::init,"counts"=>0/*, "update_time"=>NOW*/));
					$mission_change = true;
					if(!$res){
						return false;
					}
				}
			}
		}
		return true;
	}
	function get_user_mission($user_missions, $mission_id){
		foreach ($user_missions as $user_mission) {
			if($user_mission["MissionId"] == $mission_id){
				return $user_mission;
			}
		}
		return null;
	}
	function set_new_mission($user, $mission_master){
		$values = array();
		$values["user_id"] = "{$user["id"]}";
		$values["mission_id"] = "{$mission_master["id"]}";
		$values["status"] = "'".MissionStatus::init."'";
		$values["register_time"] = "'".NOW."'";
		return $this->user_db->insert($values, $this->user_db->mission);
	}
	function update($id, $args){
		return $this->update_common($this->user_db->mission,$id,$args);
	}
	function battle_mission_change($user, $BattlingId, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $this->getSessionData("mission_masters");
		$mission_change = false;
		foreach ($missions as $mission) {
			if($mission["Status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if($mission_master["battle_id"] == $BattlingId){
				$change = $this->update($mission["Id"], array("status"=>"'".MissionStatus::clear."'"));
				if(!$change){
					return false;
				}
				$add = $this->mission_plus($user, $mission["MissionId"]);
				if(!$add){
					return false;
				}
				$mission_change  = true;
			}else if($mission_master["battle_count"] > 0){
				$args = array("counts"=>$mission["Counts"] + 1);
				if($mission["Counts"] + 1 >= $mission_master["battle_count"]){
					$args["status"] = "'".MissionStatus::clear."'";
				}
				$change = $this->update($mission["Id"], $args);
				if(!$change){
					return false;
				}
				$add = $this->mission_plus($user, $mission["MissionId"]);
				if(!$add){
					return false;
				}
				$mission_change  = true;
			}
		}
		return true;
	}
	function character_mission_change($user, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $this->getSessionData("mission_masters");
		$character_count = count($user["characters"]);
		$mission_change = false;
		foreach ($missions as $mission) {
			if($mission["Status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if($mission_master["character_count"] > 0 && $mission["Counts"] != $character_count){
				$args = array("counts"=>$character_count);
				if($character_count >= $mission_master["character_count"]){
					$args["status"] = "'".MissionStatus::clear."'";
				}
				$change = $this->update($mission["Id"], $args);
				if(!$change){
					return false;
				}
				$add = $this->mission_plus($user, $mission["MissionId"]);
				if(!$add){
					return false;
				}
				$mission_change  = true;
			}
		}
		return true;
	}
	function level_mission_change($user, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $this->getSessionData("mission_masters");
		$character_count = count($user["characters"]);
		$mission_change = false;
		foreach ($missions as $mission) {
			if($mission["Status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if($mission_master["level"] > 0 && $user["Level"] >= $mission_master["level"]){
				$change = $this->update($mission["Id"], array("status"=>"'".MissionStatus::clear."'"));
				if(!$change){
					return false;
				}
				$add = $this->mission_plus($user, $mission["MissionId"]);
				if(!$add){
					return false;
				}
				$mission_change  = true;
			}
		}
		return true;
	}
	function progress_mission_change($user, $key, $value, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $this->getSessionData("mission_masters");
		$character_count = count($user["characters"]);
		$mission_change = false;
		foreach ($missions as $mission) {
			if($mission["Status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if(!empty($mission_master["story_progress"])){
				$story_progress = explode(":", $mission_master["story_progress"]);
				if($story_progress[0] == $key && $story_progress[1] == $value){
					$change = $this->update($mission["Id"], array("status"=>"'".MissionStatus::clear."'"));
					if(!$change){
						return false;
					}
					$add = $this->mission_plus($user, $mission["MissionId"]);
					if(!$add){
						return false;
					}
					$mission_change  = true;
				}
			}
		}
		return true;
	}
	function gold_count_mission_change($user, &$mission_change){
		$missions = $user["missions"];
		$mission_masters = $this->getSessionData("mission_masters");
		$mission_change = false;
		foreach ($missions as $mission) {
			if($mission["Status"] != MissionStatus::init){
				continue;
			}
			$mission_master = $this->get_master_mission($mission_masters, $mission["MissionId"]);
			if($mission_master["gold_count"] > 0){
				$args = array("counts"=>$mission["Counts"] + 1);
				if($mission["Counts"] + 1 >= $mission_master["gold_count"]){
					$args["status"] = "'".MissionStatus::clear."'";
				}
				$change = $this->update($mission["Id"], $args);
				if(!$change){
					return false;
				}
				$add = $this->mission_plus($user, $mission["MissionId"]);
				if(!$add){
					return false;
				}
				$mission_change  = true;
			}
		}
		return true;
	}
}
