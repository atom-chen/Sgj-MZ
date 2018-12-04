<?php 
class Master_model extends MY_Model
{
	function __construct(){
		parent::__construct();
	}
	public function get_master_character(){
		$select = "`id`,`name`,`nickname`,`head`,`hat`,`hp`,`mp`,`weapon`,`clothes`,`horse`,`qualification`,
		`power`,`knowledge`,`trick`,`endurance`,`speed`,`moving_power`,`riding`,`walker`,`pike`,`sword`,
		`long_knife`,`knife`,`long_ax`,`ax`,`long_sticks`,`sticks`,`archery`,`hidden_weapons`,`dual_wield`,`magic`,
		`resistance_metal`,`resistance_wood`,`resistance_water`,`resistance_fire`,`resistance_earth`, `introduction` ";
		$table = $this->master_db->base_character;
		$order_by = "id";
		$result = $this->master_db->select($select, $table, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$row['skills'] = $this->get_character_skills($row["id"]);
			$row['resistances'] = array(0,$row['resistance_metal'],$row['resistance_wood'],$row['resistance_water'],$row['resistance_fire'],$row['resistance_earth']);
			$result_array[] = $row;
		}
		return $result_array;
	}
	public function get_character_skills($character_id){
		$where = array();
		$where[] = "character_id = {$character_id}";
		
		$result = $this->master_db->select("`character_id`,`skill_id`,`star`,`skill_point`", $this->master_db->character_skill, $where, "star asc");
		return $result;
	}
	public function get_master_mission($language="cn"){
		$select = "`id`,`mission_type`,`name_{$language}` as `name`, `start_time`, `end_time`, `parent_id`, `battle_id`, 
		`story_progress`, `level`, `character_count`, `battle_count`, `gold_count`, `message_{$language}` as `message`,`rewards`";
		$table =  $this->master_db->mission;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$row["rewards"] = json_decode($row["rewards"]);
			$result_array[] = $row;
		}
		return $result_array;
	}
	public function get_character_stars(){
		$result = $this->master_db->select("`id`,`star`,`cost`", $this->master_db->character_star, null, "star asc");
		return $result;
	}
	public function get_master_avatar(){
		$select = "`id`,`move_arms`,`arms`,`clothes`,`avatar_action`,`avatar_action_index`,`avatar_property`,`animation_index`,`position_x`,`position_y`,`sibling`,`scale_x`,`scale_y`";
		$order_by = "move_arms ASC, arms ASC, avatar_action ASC, avatar_action_index ASC";
		$result = $this->master_db->select($select, $this->master_db->avatar_setting, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			if(!$result_array[$row["move_arms"]]){
				$result_array[$row["move_arms"]] = array();
			}
			$this->get_master_avatar_move_arms($result_array[$row["move_arms"]], $row);
		}
		return $result_array;
	}
	public function get_master_avatar_move_arms(&$move_arms, $row){
		if(!$move_arms[$row["arms"]]){
			$move_arms[$row["arms"]] = array();
		}
		$this->get_master_avatar_arms($move_arms[$row["arms"]], $row);
	}
	public function get_master_avatar_arms(&$arms, $row){
		if(!$arms[$row["avatar_action"]]){
			$arms[$row["avatar_action"]] = array();
		}
		$this->get_master_avatar_avatar_action($arms[$row["avatar_action"]], $row);
	}
	public function get_master_avatar_avatar_action(&$avatar_action, $row){
		if(!$avatar_action[$row["avatar_action_index"]]){
			$avatar_action[$row["avatar_action_index"]] = array();
		}
		$this->get_master_avatar_avatar_action_index($avatar_action[$row["avatar_action_index"]], $row);
		$avatar_action[$row["avatar_action_index"]]["clothesType"] = $row["clothes"];
	}
	public function get_master_avatar_avatar_action_index(&$avatar_action_index, $row){
		$avatar_property = array();
		$avatar_property["index"] = $row["animation_index"];
		$avatar_property["position_value"] = $row["position_x"].",".$row["position_y"];
		$avatar_property["sibling"] = $row["sibling"];
		$avatar_property["scale_value"] = $row["scale_x"].",".$row["scale_y"];
		$avatar_action_index[$row["avatar_property"]] = $avatar_property;
	}
	public function get_master_constant(){
		$result = $this->master_db->select("`name`,`val`", $this->master_db->constant, null, null, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			if($row["name"] == "female_heads"){
				$result_array[$row["name"]] = explode(",", $row["val"]);
			}else if($row["name"] == "user_characters"){
				$result_array[$row["name"]] = json_decode($row["val"]);
			}else{
				$result_array[$row["name"]] = $row["val"];
			}
		}
		return $result_array;
	}
	public function get_master_building($level = null, $building_id = null){
		$select = "`id`,`from_level`,`to_level`,`tile_id`,`sum`,`price`,`price_type`";
		$table = $this->master_db->building;
		$where = null;
		if(!is_null($level)){
			$where = array();
			$where[] = "from_level <= {$level}";
			$where[] = "to_level >= {$level}";
			$where[] = "tile_id = {$building_id}";
		}
		$order_by = "id asc";
		$result_select = $this->master_db->select($select, $table, $where, $order_by,null, Database_Result::TYPE_DEFAULT);
		if(!is_null($level)){
			$result = mysql_fetch_assoc($result_select);
		}else{
			$result = array();
			while ($row = mysql_fetch_assoc($result_select)) {
				$result[] = $row;
			}
		}
		return $result;
	}
	function get_master_strategys($language="cn"){
		$select = "`id`,`aid_type`,`effect_type`,`hert`,`effect`";
		$table = $this->master_db->strategy;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_tile($language="cn"){
		$select = "`id`,`name_{$language}` as `name`,`road`,`hp`,`strategy`,`heal`,`boat`";
		$table = $this->master_db->tile;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_base_map(){
		$select = "`id`,`width`,`height`,`tile_ids`";
		$table = $this->master_db->base_map;
		$order_by = "id asc";
		$result_select = $this->master_db->select($select, $table, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result = array();
		while ($row = mysql_fetch_assoc($result_select)) {
			$row["tile_ids"] = explode(",", $row["tile_ids"]);
			$result[] = $row;
		}
		return $result;
	}
	function get_master_world($language = "cn"){
		$select = "`id`,`tile_id`,`x`,`y`,`level`,`map_id`,`build_name_{$language}` as `build_name`";
		$table = $this->master_db->world;
		$order_by = "id asc";
		$result_select = $this->master_db->select($select, $table, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result = array();
		while ($row = mysql_fetch_assoc($result_select)) {
			$row["stages"] = $this->get_master_area($row["map_id"], $language);
			$result[] = $row;
		}
		return $result;
	}
	function get_master_area($map_id, $language = "cn"){
		$select = "`id`,`tile_id`,`x`,`y`,`level`";
		$table = $this->master_db->area;
		$where = array("`map_id`={$map_id}");
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function get_master_horse($language = "cn"){
		$select = "`id`,`name_{$language}` as `name`,`child_type` as `move_type`,`moving_power`,`image_index`,`saddle`,`hp`,`speed`";
		$table = $this->master_db->horse;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_weapon($language = "cn"){
		$select = "`id`,`name_{$language}` as `name`,`child_type` as `weapon_type`,`physical_attack`,`magic_attack`,`power`";
		$table = $this->master_db->weapon;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_clothes($language = "cn"){
		$select = "`id`,`name_{$language}` as `name`,`child_type` as `clothes_type`,`physical_defense`,`magic_defense`,`hp`";
		$table = $this->master_db->clothes;
		$order_by = "id asc";
		$result_select = $this->master_db->select($select, $table, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result = array();
		while ($row = mysql_fetch_assoc($result_select)) {
			$equipmentValue = json_decode($row["physical_defense"]);
			$row["physical_defense"] = array("value"=>$equipmentValue[0], "plus"=>$equipmentValue[1]);
			$equipmentValue = json_decode($row["magic_defense"]);
			$row["magic_defense"] = array("value"=>$equipmentValue[0], "plus"=>$equipmentValue[1]);
			$equipmentValue = json_decode($row["hp"]);
			$row["hp"] = array("value"=>$equipmentValue[0], "plus"=>$equipmentValue[1]);
			$result[] = $row;
		}
		return $result;
	}
	function get_master_skill($language="cn"){
		$select = "`id`,`name_{$language}` as `name`,`types`,`level`,`character_level`,`price`,`weapon_types`,`distance`,`strength`,`radius_type`,`radius`,
		`explanation_{$language}` as `explanation`,`effect`,`animation`,`five_elements`,`hp`,`mp`,`power`,`knowledge`,`speed`,`trick`,`endurance`,`moving_power`,`riding`,`walker`,
		`pike`,`sword`,`long_knife`,`knife`,`long_ax`,`ax`,`long_sticks`,`sticks`,`archery`,`hidden_weapons`,`dual_wield`,`magic`,
		`resistance_metal`,`resistance_wood`,`resistance_water`,`resistance_fire`,`resistance_earth`";
		$table = $this->master_db->skill;
		$order_by = "id asc,level asc";
		$result_select = $this->master_db->select($select, $table, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result = array();
		while ($row = mysql_fetch_assoc($result_select)) {
			$row["types"] = json_decode($row["types"]);
			$row["weapon_types"] = json_decode($row["weapon_types"]);
			$row["distance"] = json_decode($row["distance"]);
			//echo $row["effect"];
			$row["effect"] = json_decode($row["effect"]);
			$result[] = $row;
		}
		return $result;
	}
	function get_master_items($language = "cn", $id = null){
		$select = "`id`,`name_{$language}` as `name`,`item_type`,`child_id`,`price`,`content_value`,`explanation`";
		$table = $this->master_db->item;
		$where = null;
		if(!is_null($id)){
			$where = array("id = {$id}");
		}
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function get_master_words($language = "cn"){
		$select = "`word_key` as `key`,`word_value_{$language}` as `value`";
		$table = $this->master_db->word;
		$result = $this->master_db->select($select, $table);
		return $result;
	}
	function get_master_battlefield_tiles($battlefield_id){
		$select = "`id`,`tile_id`, `x`,`y`, `level`";
		$table = $this->master_db->battlefield_tile;
		$where = array("battlefield_id = {$battlefield_id}");
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function get_master_battlefield_npcs($battlefield_id,$belong){
		$select = "`id`, `npc_id`,`boss`,`skills`, `x`,`y`, `level`,`star`,`weapon`,`horse`,`clothes`";
		$table = $this->master_db->battlefield_npc;
		$where = array("battlefield_id = {$battlefield_id}","belong='{$belong}'");
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function get_master_battlefield_owns($battlefield_id){
		$select = "`out_index` as `id`,`x`,`y`";
		$table = $this->master_db->battlefield_own;
		$where = array("battlefield_id = {$battlefield_id}");
		$order_by = "out_index asc";
		$result = $this->master_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function get_master_scenarios($language="cn"){
		$select = "`id`,`script_{$language}` as `script`";
		$table = $this->master_db->scenario;
		$order_by = "id asc";
		$result_array = $this->master_db->select($select, $table, null, $order_by);
		$result = array(array(), array());
		foreach($result_array as $val){
			$arr = explode(";",$val["script"]);
			foreach($arr as $k=>$v){
				$arr[$k] = trim($v);
			}
			$result[0][] = $arr;
			$result[1][] = $val["id"];
		}
		return $result;
	}
	function get_master_story_progress(){
		$select = "`k`";
		$table = $this->master_db->story_progress;
		$result = $this->master_db->select($select, $table,null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$result_array[] = $row["k"];
		}
		return $result_array;
	}
	function get_master_tutorial($language="cn"){
		$select = "`id`,`script_{$language}` as `script`";
		$table = $this->master_db->tutorial;
		$order_by = "id asc";
		$result_array = $this->master_db->select($select, $table, null, $order_by);
		$result = array();
		foreach($result_array as $val){
			//$val["script"] = explode(";",$val["script"]);
			$arr = explode(";",$val["script"]);
			foreach($arr as $k=>$v){
				$arr[$k] = trim($v);
			}
			$result[] = $arr;
		}
		return $result;
	}
	function get_master_battlefields($language="cn"){
		$select = "`id`,`name_{$language}` as `name`,`world_id`,`script_{$language}` as `script`,`max_bout`, `ap`";
		$table = $this->master_db->battlefield;
		$order_by = "id asc";
		$result_array = $this->master_db->select($select, $table, null, $order_by);
		$result = array();
		foreach($result_array as $val){
			$scripts = explode(";",$val["script"]);
			$val["script"] = array();
			foreach($scripts as $script){
				$script = Trim($script);
				if(strlen($script) == 0){
					continue;
				}
				$val["script"][] = $script;
			}
			$val["tiles"] = $this->get_master_battlefield_tiles($val["id"]);
			$val["enemys"] = $this->get_master_battlefield_npcs($val["id"], "enemy");
			$val["friends"] = $this->get_master_battlefield_npcs($val["id"], "friend");
			$val["owns"] = $this->get_master_battlefield_owns($val["id"]);
			$result[] = $val;
		}
		return $result;
	}
	function get_master_gachas(){
		$select = "id, name, gold, silver, from_time, to_time";
		$table = $this->master_db->gacha;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		foreach ($result as $key => $child) {
			$child["childs"] = $this->get_master_gacha_childs($child["id"]);
			$child["prices"] = $this->get_master_gacha_prices($child["id"]);
			$result[$key] = $child;
		}
		return $result;
	}
	function get_master_gacha_childs($gacha_id){
		$select = "id, gacha_id, type, child_id, probability";
		$table = $this->master_db->gacha_child;
		$where = array("gacha_id = {$gacha_id}");
		$result = $this->master_db->select($select, $table, $where);
		return $result;
	}
	function get_master_gacha_prices($gacha_id){
		$select = "id, child_id, price_type, price, cnt, free_time, free_count";
		$table = $this->master_db->gacha_price;
		$where = array("id = {$gacha_id}");
		$order_by = "child_id asc";
		$result = $this->master_db->select($select, $table, $where, $order_by);
		return $result;
	}
	function get_master_npcs(){
		$select = "`id`,`character_id`,`star`,`horse`,`clothes`,`weapon`";
		$table = $this->master_db->npc;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_npc_equipments(){
		$select = "`id`,`equipment_id`,`equipment_type`,`level`";
		$table = $this->master_db->npc_equipment;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_exp(){
		$select = "`id`,`level`,`value`";
		$table = $this->master_db->exp;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by);
		return $result;
	}
	function get_master_loginbonus(){
		$select = "`id`,`year`,`month`,`contents`";
		$table = $this->master_db->login_bonus;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$row["contents"] = json_decode($row["contents"]);
			$result_array[] = $row;
		}
		return $result_array;
	}
	function get_master_shops(){
		$select = "`id`,`type`,`shop_content`,`gold`,`silver`,`money`,`limit_time`";
		$table = $this->master_db->shop;
		$order_by = "id asc";
		$result = $this->master_db->select($select, $table, null, $order_by, null, Database_Result::TYPE_DEFAULT);
		$result_array = array();
		while ($row = mysql_fetch_assoc($result)) {
			$row["shop_content"] = json_decode($row["shop_content"]);
			$result_array[] = $row;
		}
		return $result_array;
	}
}
?>