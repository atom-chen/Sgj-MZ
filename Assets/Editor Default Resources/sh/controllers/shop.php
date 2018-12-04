<?php 
class Shop extends MY_Controller {
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		load_model(array('user_model', 'shop_model', 'master_model'));
	}
	public function buy()
	{
		$shop_type = $this->args["shop_type"];
		if($shop_type == "building"){
			$this->buy_building();
		}else if($shop_type == "item"){
			$this->buy_item();
		}
	}
	private function buy_item()
	{
		load_model(array('item_model','equipment_model'));
		$shop_id = $this->args["child_id"];
		$shop_model = new Shop_model();
		$result = $shop_model->buy_item($shop_id);
		if($result){
			$user = $this->getSessionData("user");
			$this->out(array("user"=>$user));
		}else{
			$this->error("Shop buy_item failed");
		}
	}
	private function buy_building()
	{
		$building_id = $this->args["child_id"];
		$user = $this->getSessionData("user");
		$user_id = $user["id"];
		$user_model = new User_model();
		$buildings = $user_model->get_top_map($user["id"]);
		$master_model = new Master_model();
		$master_building = $master_model->get_master_building($user["Level"], $building_id);
		if(is_null($master_building)){
			$this->error("Not Found building_id");
		}
		$building_count = 0;
		foreach ($buildings as $building) {
			if($building["tile_id"] == $building_id){
				$building_count++;
			}
		}
		if($building_count >= $master_building["sum"]){
			$this->error("Limit Error " . $building_count . ">=".$master_building["sum"]);
		}
		$price_type = $master_building["price_type"];
		$price = $master_building["price"];
		$gold = 0;
		$silver = 0;
		if($price_type == "gold"){
			$gold = $price;
		}else if($price_type == "silver"){
			$silver = $price;
		}
		$shop_model = new Shop_model();
		$result = $shop_model->buy_building($master_building, $this->args["x"], $this->args["y"]);
		if($result){
			//player session 更新
			$user = $user_model->get($user_id, true);
			$this->setSessionData("user", $user);
			$this->out(array("user"=>$user));
		}else{
			$this->error("Shop buy_building failed");
		}
	}
}
