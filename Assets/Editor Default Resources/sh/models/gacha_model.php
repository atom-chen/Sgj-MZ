<?php 
class PriceType{
	const gold = "gold";
	const silver = "silver";
}
class Gacha_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_free_logs($gacha_id = null){
		$gachas = $this->get_gacha_masters(null, date("Y-m-d H:i:s",time()));
		$user = $this->getSessionData("user");
		$freelogs = array();
		foreach ($gachas as $key => $gacha) {
			$freelog = array();
			if(!is_null($gacha_id) && $gacha["id"] != $gacha_id){
				continue;
			}
			$log = $this->get_free_log($user["id"],$gacha["id"]);
			$free_cnt = 0;
			$last_log_time = TIME_INIT;
			if(!empty($log)){
				foreach ($log as $logchild) {
					if($logchild["register_time"] > DAY_START){
						$free_cnt++;
					}
				}
				$last_log_time = $log[0]["register_time"];
			}
			$freelog["GachaId"] = $gacha["id"];
			$freelog["LimitCount"] = $gacha["free_count"] - $free_cnt;
			$freelog["LastTime"] = $last_log_time;
			$freelogs[] = $freelog;
		}
		return $freelogs;
	}
	function get_free_log($user_id, $gacha_id){
		$select = "id, user_id, gacha_id, register_time";
		$table = $this->user_db->gacha_free_log;
		$where = array();
		$where[] = "`user_id`={$user_id}";
		$where[] = "`gacha_id`={$gacha_id}";
		$where[] = "`register_time`>'".date("Y-m-d H:i:s", strtotime("-2 day"))."'";
		$order_by = "id desc";
		$result = $this->user_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function set_gacha_free_log($user_id, $gacha_id){
		$values = array();
		$values["user_id"] = $user_id;
		$values["gacha_id"] = $gacha_id;
		$values["register_time"] = "'".date("Y-m-d H:i:s",time())."'";
		$table = $this->user_db->gacha_free_log;
		$res = $this->user_db->insert($values, $table);
		return $res;
	}
	function slot($args){
		$gachas = $this->get_gacha_masters($args["gacha_id"]);
		if(count($gachas) == 0){
			$this->error("can not gacha");
		}
		$gacha = $gachas[0];
		$gachaPrice = $this->get_gacha_price($args["gacha_id"], $args["price_id"]);
		$user = $this->getSessionData("user");
		$gold = 0;
		$silver = 0;
		$free_gacha = (isset($args["free_gacha"]) && $args["free_gacha"] == 1);
		if($free_gacha){
			$limit = $gacha["free_count"];
			$log = $this->get_free_log($user["id"], $gacha["id"]);
			if(!empty($log)){
				$free_cnt = 0;
				foreach ($log as $logchild) {
					if($logchild["register_time"] > DAY_START){
						$free_cnt++;
					}
				}
				if($free_cnt >= $limit){
					$this->error("can not free gacha");
				}
				$last_log = $log[0];
				if($last_log["register_time"] > date("Y-m-d H:i:s",strtotime("-".$gacha["free_time"]." minute"))){
					$this->error("can not free gacha");
				}
			}
		}else{
			$price_type = $gachaPrice["price_type"];
			if($price_type == PriceType::gold){
				$gold = $gachaPrice["price"];
			}else if($price_type == PriceType::silver){
				$silver = $gachaPrice["price"];
			}
			if($user["Gold"] < $gold || $user["Silver"] < $silver){
				$this->error("no money");
			}
		}
	
		$gacha_childs = $this->get_gacha_childs($gacha["id"]);
		$slot_list = array();
		$i = 0;
		while(count($slot_list) < $args["cnt"]){
			$probability_sum = 0;
			foreach($gacha_childs as $val){
				$probability_sum += $val["probability"];
			}
			$rand_index = rand(0,$probability_sum);
			$probability_sum = 0;
			foreach($gacha_childs as $val){
				$probability_sum += $val["probability"];
				if($rand_index <= $probability_sum){
					$slot_list[] = $val;
					break;
				}
			}
		}
		$this->user_db->trans_begin();
		//购买记录(消费)
		if($free_gacha){
			$setlog = $this->set_gacha_free_log($user["id"], $gacha["id"]);
		}else{
			$setlog = $this->set_gacha_buy_log($user["id"], $gacha["id"], $gold, $silver);
		}
		if(!$setlog){
		    $this->user_db->trans_rollback();
			$this->error("set log error");
		}
		$user_model = new User_model();
		$change = $user_model->update_money();
		if(!$change){
		    $this->user_db->trans_rollback();
			$this->error("update money error");
		}
		//奖品
		$equipment_model = null;
		$item_model = null;
		$character_model = null;
		$contents = array();
		for($i=0;$i<count($slot_list);$i++){
			$gacha_child = $slot_list[$i];
			switch($gacha_child["type"]){
				case "item":
					$items = $this->get_item_master($gacha_child["child_id"], $gacha_child["child_type"], $gacha_child["qualification"]);
					$item = $items[rand(0, count($items) - 1)];
					/*$item_model = $item_model == null ? new Item_model() : $item_model;
					$gacha_get = $item_model->set_item($user["id"], $item["id"]);*/
					$contents[] = array("type"=>$gacha_child["type"], "content_id"=>$item["id"]);
					break;
				case "horse":
				case "weapon":
				case "clothes":
					$equipments = $this->get_equipment_master($gacha_child["type"], $gacha_child["child_id"], $gacha_child["child_type"], $gacha_child["qualification"]);
					$equipment = $equipments[rand(0, count($equipments) - 1)];
					/*$equipment_model = $equipment_model == null ? new Equipment_model() : $equipment_model;
					$gacha_get = $equipment_model->set_equipment($user["id"], $equipment["id"], $gacha_child["type"]);*/
					$contents[] = array("type"=>$gacha_child["type"], "content_id"=>$equipment["id"]);
					break;
				case "character":
					$characters = $this->get_character_master($gacha_child["child_type"], $gacha_child["qualification"]);
					$character = $characters[rand(0, count($characters) - 1)];
					/*$character_model = $character_model == null ? new Character_model() : $character_model;
					$equipment_model = $equipment_model == null ? new Equipment_model() : $equipment_model;
					$character_values = array();
					$character_values['user_id'] = $user["id"];
					$character_values['character_id'] = $character["id"];
					$character_values['register_time'] = "'{$now}'";
					$gacha_get = $character_model->character_insert($character_values);*/
					$contents[] = array("type"=>$gacha_child["type"], "content_id"=>$character["id"]);
					break;
			}
			/*if(!$gacha_get){
			    $this->user_db->trans_rollback();
				$this->error("get present error ");
			}*/
		}
		$contents_res = $user_model->set_contents($user["id"],$contents);
		if(!$contents_res){
			$this->user_db->trans_rollback();
			$this->error("set contents error ".$this->user_db->last_sql);
		}
		$this->user_db->trans_commit();
		return $contents;
	}
	function get_gacha_character_ids($child_type){
		$select = "character_id, probability";
		$where = array("gacha_type='{$child_type}'");
		$result = $this->master_db->select($select, $this->master_db->gacha_characters, $where);
		return $result;
	}
	function get_character_master($child_type = null, $qualification = 0){
		$select = "*";
		$where = array();
		$where[] = "can_gacha=1";
		if(!is_null($child_type)){
			$id_array = $this->get_gacha_character_ids($child_type);
			$probability_sum = 0;
			foreach($id_array as $character){
				$probability_sum += $character["probability"];
			}
			$rand_value = rand(0, $probability_sum);
			$probability = 0;
			$character_ids = [];
			foreach($id_array as $character){
				$probability += $character["probability"];
				if($rand_value <= $probability){
					$character_id = split(",", $character["character_id"]);
					break;
				}
			}
			if(count($character_ids) > 0){
				$where[] = "id >= {$character_id[0]} ";
				$where[] = "id <= {$character_id[1]} ";
			}
		}
		$this->log("qualification={$qualification}\n");
		if($qualification > 0){
			$where[] = "qualification={$qualification}";
		}
		$result = $this->master_db->select($select, $this->master_db->base_character, $where);
		return $result;
	}
	function get_item_master($id = 0, $item_type = null, $qualification = null){
		$select = "*";
		$where = array();
		if($id > 0){
			$where[] = "id={$id}";
		}
		if(!is_null($item_type)){
			$where[] = "item_type='{$item_type}'";
		}
		if($qualification > 0){
			$where[] = "qualification={$qualification}";
		}
		$result = $this->master_db->select($select, $this->master_db->item, $where);
		return $result;
	}
	function get_equipment_master($table, $id = 0, $child_type = null, $qualification = null){
		$select = "*";
		$where = array();
		if($id > 0){
			$where[] = "id={$id}";
		}
		if(!is_null($child_type) && !empty($child_type)){
			$where[] = "child_type='{$child_type}'";
		}
		if($qualification > 0){
			$where[] = "qualification={$qualification}";
		}
		$result = $this->master_db->select($select, $this->master_db->$table, $where);
		return $result;
	}
	function get_gacha_masters($id = null){
		$select = "id, gold, silver, from_time, to_time, free_time,free_count";
		$table = $this->master_db->gacha;
		$where = array();
		$now = date("Y-m-d H:i:s",time());
		$where[] = "(`from_time`='0000-00-00 00:00:00' OR `from_time`<'{$now}')";
		$where[] = "(`to_time`='0000-00-00 00:00:00' OR `to_time`>'{$now}')";
		if(!is_null($id)){
			$where[] = "id={$id}";
		}
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
	function get_gacha_childs($gacha_id){
		$select = "id, gacha_id, type, child_id, child_type, probability,qualification";
		$table = $this->master_db->gacha_child;
		$where = array();
		$where[] = "gacha_id={$gacha_id}";
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
	function get_gacha_price($gacha_id, $price_id){
		$select = "id, child_id, price_type, price, cnt, free_time, free_count";
		$table = $this->master_db->gacha_price;
		$where = array("id = {$gacha_id}", "child_id = {$price_id}");
		$result = $this->master_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result;
	}
	function set_gacha_buy_log($user_id, $gacha_id, $gold, $silver){
		$values = array();
		$values['user_id'] = $user_id;
		$values['type'] = "'gacha'";
		$values['child_id'] = $gacha_id;
		$values['gold'] = -$gold;
		$values['silver'] = -$silver;
		$values['register_time'] = "'".date("Y-m-d H:i:s",time())."'";
		$res = $this->user_db->insert($values, $this->user_db->bankbook);
		return $res;
	}
}
