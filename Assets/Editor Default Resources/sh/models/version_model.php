<?php 
class Version_model extends MY_Model
{
	
	function __construct(){
		parent::__construct();
	}
	function get_master(){
		$result = $this->master_db->select("`name`,`ver`", $this->master_db->version, null, null, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$result_array[$row["name"]] = $row["ver"];
		}
		return $result_array;
	}
}
