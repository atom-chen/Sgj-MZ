<?php 
class Battle_model extends MY_Model
{
	var $select_battlefield_master = "`id`,`world_id`,`conditions`, `max_num`, `ap`, `exp`";
	var $select_battlefield_list = "id as Id,battlefield_id as BattlefieldId,level as Level,star as Star";
	
	function __construct(){
		parent::__construct();
	}
	var $exp_master = null;
	function get_list($user_id, $battlefield_id = 0){
		$select = $this->select_battlefield_list;
		$table = $this->user_db->battle_list;
		$where = array();
		$where[] = "user_id={$user_id}";
		if($battlefield_id > 0){
			$where[] = "battlefield_id={$battlefield_id}";
		}
		$result = $this->user_db->select($select, $table, $where);
		if(is_null($result)){
			return array();
		}
		return $result;
	}
	function set_battlefield($user_id, $battlefield_id, $boss_id, $level, $star, $isFirstBattle){
		$values = array();
		$table = $this->user_db->battle_list;
		if($isFirstBattle){
			$values['user_id'] = $user_id;
			$values['battlefield_id'] = $battlefield_id;
			$values['boss_id'] = $boss_id;
			$values['level'] = $level;
			$values['star'] = $star;
			$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
			$res = $this->user_db->insert($values, $table);
		}else{
			$values[] = "`star`={$star}";
			$where = array("user_id={$user_id}", "battlefield_id={$battlefield_id}");
			$res = $this->user_db->update($values, $table, $where);
		}
		return $res;
	}
	function battle_start($battlefield_id){
		$battlefield_master = $this->get_master($battlefield_id);
		$user = $this->getSessionData("user");
		$user_id = $user["id"];
		$recover_ap_time = $this->get_recover_ap_time_constant();
		$seconds = time() - $user["LastApDate"];
		$recover_ap = floor($seconds / $recover_ap_time);
		$currentAp = $user["Ap"] + $recover_ap;
		if($currentAp < $battlefield_master["ap"]){
			return false;
		}
		$user_model = new User_model();
		if(!empty($battlefield_master["conditions"])){
			$conditions = json_decode($battlefield_master["conditions"], true);
			$progress = $user_model->get_story_progress($user_id);
			foreach ($conditions as $key => $value) {
				$has = $user_model->has_progress($progress, $key, $value);
				if(!$has){
					return false;
				}
			}
		}
		$this->user_db->trans_begin();
		$ap = $currentAp - $battlefield_master["ap"];
		$res_start = $user_model->update(array("id"=>$user_id, "battling_id"=>$battlefield_id, "ap"=>$ap, "last_ap_date"=>"'".date("Y-m-d H:i:s")."'"));
		if(!$res_start){
			$this->user_db->trans_rollback();
			$this->error("user update error");
		}
		$user["BattlingId"] = $battlefield_id;
		$user["Ap"] = $ap;
		$this->setSessionData("user", $user);
		$this->user_db->trans_commit();
		return true;
	}
	function battle_end($characterIds, $die_ids, $star){
		$user = $this->getSessionData("user");
		$battlefield_id = $user["BattlingId"];
		$battlefield_master = $this->get_master($battlefield_id);
		$user_id = $user["id"];
		$user_model = new User_model();
		$character_model = new Character_model();
		
		$characters = array();
		foreach ($characterIds as $characterId) {
			$character_list = $character_model->get_character_list($user_id, $characterId);
			if(count($character_list) == 0){
				return null;
			}
			$characters[] = $character_list[0];
			/*$has_character = false;
			foreach($characters as $character){
				if($character["CharacterId"] == $characterId){
					$has_character = true;
					break;
				}
			}
			if(!$has_character){
				return null;
			}*/
		}
		$battleList = $this->get_list($user_id, $battlefield_id);
		$isFirstBattle = (count($battleList) == 0);
		if(!$isFirstBattle){
			$old_star = $battleList[0]["Star"];
			$star = $star > $old_star ? $star : $old_star;
		}
		$battlefield_rewards = $this->get_rewards_master($battlefield_id);
		$contents = array();
		foreach ($battlefield_rewards as $battlefield_reward) {
			$content = json_decode($battlefield_reward["content"],true);
			if($battlefield_reward["is_first"] == 1){
				if($isFirstBattle){
					$content["get_type"] = "battle";
					$contents[] = $content;
				}
				continue;
			}
			$probability = $battlefield_reward["probability"];
			if(rand(0,100) <= $probability){
				$content["get_type"] = "battle";
				$contents[] = $content;
			}
		}
		$npcs = $this->get_master_battlefield_npcs($battlefield_id);
		$boss_id = $npcs[0]["npc_id"];
		$sum_level = 0;
		foreach ($npcs as $npc) {
			$sum_level += $npc["level"];
			if($npc["boss"]){
				$boss_id = $npc["npc_id"];
			}
		}
		$level = floor($sum_level / count($npcs));
		$user_characters = $this->get_user_characters_constant();
		$this->user_db->trans_begin();
		$contents_res = $user_model->set_contents($user_id,$contents);
		if(!$contents_res){
			$this->user_db->trans_rollback();
			$this->error("set contents error ".$this->user_db->last_sql);
		}
		$res_set = $this->set_battlefield($user_id, $battlefield_id, $boss_id, $level, $star, $isFirstBattle);
		if(!$res_set){
			$this->user_db->trans_rollback();
			$this->error("set_battlefield error");
		}
		$user_level = 0;
		foreach ($characterIds as $characterId) {
			$character = null;
			foreach($characters as $child){
				if($child["CharacterId"] == $characterId){
					$character = $child;
					break;
				}
			}
			$exp = $this->get_character_exp($character["Level"], $level, $battlefield_master["exp"]);
			if(array_search($characterId, $die_ids) !== false){
				$exp = floor($exp * 0.5);
			}
			$character_level = $this->get_character_level($character["Exp"] + $exp);
			$character_args = array("exp"=>$exp + $character["Exp"], "level"=>$character_level);
			$res_update_character = $character_model->update_character($user_id, $characterId, $character_args);
			if(!$res_update_character){
				$this->user_db->trans_rollback();
				$this->error("character update error");
			}
			if($character["Id"] >= $user_characters[0] && $character["Id"] <= $user_characters[1]){
				$user_level = $character_level;
			}
		}
		$res_update = $user_model->update(array("id"=>$user_id, "battling_id"=>0, "level"=>$user_level));
		if(!$res_update){
			$this->user_db->trans_rollback();
			$this->error("user update error");
		}
		$battle_mission_change = false;
		$level_mission_change = false;
		$mission_model = new Mission_model();
		$mission_result = $mission_model->battle_mission_change($user, $battlefield_id, $battle_mission_change);
		if(!$mission_result){
			$this->user_db->trans_rollback();
			$this->error("mission update error");
		}
		$user["Level"] = $user_level;
		$mission_result = $mission_model->level_mission_change($user, $level_mission_change);
		if(!$mission_result){
			$this->user_db->trans_rollback();
			$this->error("mission update error");
		}
		if($battle_mission_change || $level_mission_change){
			$user["missions"]=$mission_model->get_mission_list($user["id"]);
		}

		$user["characters"]=$character_model->get_character_list($user["id"]);
		$user["BattlingId"] = 0;
		$this->setSessionData("user", $user);
		$this->master_db->trans_commit();
		return $contents;
	}
	function get_character_exp($character_level, $battlefield_level, $battlefield_exp){
		$exp = $battlefield_exp;
		$num = 0;
		if($character_level > $battlefield_level){
			$num = $character_level - $battlefield_level;
		}else if($character_level < $battlefield_level - 4){
			$num = $battlefield_level - 4 - $character_level;
		}
		if($num > 4){
			$num = 4;
		}
		$exp = floor($battlefield_exp * (1 - $num * 0.2));
		return $exp;
	}
	function get_master($battlefield_id){
		$select = $this->select_battlefield_master;
		$table = $this->master_db->battlefield;
		$where = array();
		$where[] = "id={$battlefield_id}";
		$order_by = "id asc";
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function get_character_level($sum_exp){
		if($this->exp_master == null){
			$select = "`exp_type`,`level`,`value`";
			$table = $this->master_db->exp;
			$order_by = "level desc";
			$this->exp_master = $this->master_db->select($select, $table, null, $order_by);
		}
		foreach ($this->exp_master as $child) {
			if($child["value"] <= $sum_exp){
				return $child["level"];
			}
		}
		return 1;
	}
	public function get_user_characters_constant(){
		$result = $this->master_db->select("`val`", $this->master_db->constant, array("`name`='user_characters'"), null, null, Database_Result::TYPE_ROW);
		return json_decode($result["val"], true);
	}
	public function get_recover_ap_time_constant(){
		$result = $this->master_db->select("`val`", $this->master_db->constant, array("`name`='recover_ap_time'"), null, null, Database_Result::TYPE_ROW);
		return json_decode($result["val"], true);
	}
	function get_master_battlefield_npcs($battlefield_id){
		$select = "`id`, `npc_id`,`boss`,`skills`, `x`,`y`, `level`,`star`,`weapon`,`horse`,`clothes`";
		$table = $this->master_db->battlefield_npc;
		$where = array("battlefield_id = {$battlefield_id}","belong='enemy'");
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
	function get_rewards_master($battlefield_id){
		$select = "`id`,`battlefield_id`,`content`, `probability`, `is_first`";
		$table = $this->master_db->battlefield_reward;
		$where = array();
		$where[] = "battlefield_id={$battlefield_id}";
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
}
