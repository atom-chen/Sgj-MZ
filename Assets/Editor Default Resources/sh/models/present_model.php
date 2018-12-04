<?php 
class Present_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	function get_list($user_id, $present_id = 0){
		$now = date("Y-m-d H:i:s");
		$select = "`id` as `Id`, `content` as `Content`, `limit_time` as `LimitTime`";
		$table = $this->user_db->present;
		$where = array("user_id={$user_id}","receive=0","limit_time>'{$now}'");
		if($present_id > 0){
			$where[] = "id={$present_id}";
		}
		$result = $this->user_db->select($select, $table, $where, null, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$row['Content'] = json_decode($row["Content"], true);
			$result_array[] = $row;
		}
		return $result_array;
	}
	function receive($user_id, $present_id){
		$presents = $this->get_list($user_id, $present_id);
		if(count($presents) == 0){
			return false;
		}
		$present = $presents[0];
		$content = $present["Content"];
		$content["get_type"] = "present";
		$this->user_db->trans_begin();
		$user_model = new User_model();
		$contents_res = $user_model->set_contents($user_id,array($content));
		if(!$contents_res){
			$this->user_db->trans_rollback();
			$this->error("set contents error ");
		}
		$res_present = $this->update(array("id"=>$present_id, "receive"=>1));
		if(!$res_present){
			$this->user_db->trans_rollback();
			$this->error("set contents error ");
		}
		$this->user_db->trans_commit();
		return true;
	}
	function update($args){
		if(!$args || !is_array($args))return false;
		$values = array();
		foreach ($args as $key=>$value){
			if($key == "id")continue;
			$values[] = $key ."=". $value;
		}
		$where = array("id={$args['id']}");
		$table = $this->user_db->present;
		$result = $this->user_db->update($values, $table, $where);
		return $result;
	}
}
