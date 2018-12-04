<?php 
class Loginbonus_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_log_count($user_id, $start = null, $end = null){
		if(is_null($start)){
			$start = date("Y-m-01 00:00:00");
		}
		if(is_null($end)){
			$end = date("Y-m-01 00:00:00", strtotime("+1 month"));
		}
		$select = "count(0) as `count`";
		$table = $this->user_db->login_bonus;
		$where = array("user_id={$user_id}","register_time>='{$start}'","register_time<'{$end}'");
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result["count"];
	}
	function get_max_loginbonus_date($valid_date){
		$select = "MAX(`valid_date`) as `max_valid_date`";
		$table = $this->master_db->login_bonus;
		$where = array("valid_date<={$valid_date}");
		$result = $this->master_db->select($select, $table, $where, null, null, Database_Result::TYPE_ROW);
		return $result["max_valid_date"];
	}
	function get_master_loginbonus($valid_date){
		$max_valid_date = $this->get_max_loginbonus_date($valid_date);
		$select = "`id`,`valid_date`,`contents`";
		$table = $this->master_db->login_bonus;
		$where = array("valid_date={$max_valid_date}");
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function received_loginbonus($user_id){
		$start = date("Y-m-d 00:00:00");
		$end = date("Y-m-d 00:00:00", strtotime("+1 day"));
		$count = $this->get_log_count($user_id, $start, $end);
		return $count > 0;
	}
	function get_loginbonus($user_id){
		$received = $this->received_loginbonus($user_id);
		if($received){
			return false;
		}
		$count = $this->get_log_count($user_id);
		$bonus_list = $this->get_master_loginbonus(date("Ym"));
		$bonus = $bonus_list[$count];
		$contents = json_decode($bonus["contents"],true);
		foreach($contents as $k=>$content){
			$content["get_type"] = "loginbonus";
			$contents[$k] = $content;
		}
		$this->user_db->trans_begin();
		$user_model = new User_model();
		$contents_res = $user_model->set_contents($user_id,$contents);
		if(!$contents_res){
			$this->user_db->trans_rollback();
			$this->error("set contents error ".$this->user_db->last_sql);
		}
		$values = array();
		$values['user_id'] = $user_id;
		$values['register_time'] = "'".date("Y-m-d H:i:s")."'";
		$table = $this->user_db->login_bonus;
		$result = $this->user_db->insert($values, $table);
		if(!$result){
			$this->user_db->trans_rollback();
			$this->error("insert log error ");
		}
		$this->user_db->trans_commit();
		return true;
	}
}
