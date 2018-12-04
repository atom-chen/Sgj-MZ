
<?php 
class Character extends MY_Controller {
	function __construct() {
		parent::__construct();
		load_model(array('user_model', 'character_model', 'item_model', 'version_model', 'equipment_model'));
	}
	public function character_list()
	{
		$user_id = $this->args["user_id"];
		if(is_null($user_id)){
			$user = $this->getSessionData("user");
			$user_id = $user["id"];
		}
		$character_model = new Character_model();
		$characters = $character_model->get_character_list($user_id);
		if($characters !== null){
			$this->out(array("characters"=>$characters));
		}else{
			$this->error("character is not exists");
		}
	}
}
