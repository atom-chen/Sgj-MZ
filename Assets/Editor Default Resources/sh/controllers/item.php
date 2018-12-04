<?php 
class Item extends MY_Controller {
	function __construct() {
		$this->needAuth = true;
		parent::__construct();
		load_model(array('item_model', 'user_model'));
	}
	public function item_list()
	{
		$user = $this->getSessionData("user");
		$item_model = new Item_model();
		$items = $item_model->get_item_list($user["id"]);
		if($items){
			$result = array("items"=>$items);
			$this->out($result);
		}else{
			$this->error("Item->item_list error");
		}
	}
	public function sale()
	{
		$user = $this->getSessionData("user");
		$result = $this->item_model->sale($user["id"], $this->args["item_id"], $this->args["number"]);
		if(!$result){
			$this->error("Item->sale error");
			return;
		}
		$user = $this->user_model->get($user["id"]);
		$this->setSessionData("user", $user);
		$items = $this->item_model->get_item_list($user["id"]);
		if($items){
			$result = array("items"=>$items, "user"=>$user);
			$this->out($result);
		}else{
			$this->error("Item->sale error");
		}
	}
	public function use_item()
	{
		$this->load->model(array('character_model'));
		$user = $this->getSessionData("user");
		$result_get = $this->item_model->use_item($user["id"], $this->args["item_id"], $this->args["number"]);
		if(!$result_get){
			$this->error("Item->use_item error:1");
			return;
		}
		$items = $this->item_model->get_item_list($user["id"]);
		if($items){
			$result = array("items"=>$items, "get"=>$result_get);
			$this->out($result);
		}else{
			$this->error("Item->use_item error:2");
		}
	}
}
