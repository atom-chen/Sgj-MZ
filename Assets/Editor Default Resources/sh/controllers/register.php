
<?php 
class Register extends MY_Controller {
	function __construct() {
		parent::__construct();
		load_model(array('user_model', 'character_model', 'version_model'));
	}
	public function character_list()
	{
		$character_model = new Character_model();
		$characters = $character_model->get_tutorial_characters();
		if($characters !== null){
			$this->out(array("characters"=>$characters));
		}else{
			$this->error("character is not exists");
		}
	}
	public function insert()
	{
		load_model(array('equipment_model'));
		$this->checkParam(array("account", "password"));
		$account = $this->args["account"];
		$user_model = new User_model();
		$has_account = $user_model->get_user_from_account($account);
		if($has_account){
			$this->error("account is exist");
		}	
		$password = $this->args["password"];
		$password = md5($password);
		$name = $this->args["name"];
		$user = null;
		$id = $user_model->register($account, $password, $name);
		
		/*if($id){
			$user = $user_model->login(array("account"=>$account,"pass"=>$password));
		}
		$this->args["pass"] = md5($this->args["pass"]);
		$user = $user_model->login($this->args);
		if(!is_null($user)){
			$version_model = new Version_model();
			$versions = $version_model->get_master();
			$this->setSessionData("user", $user);
			$this->out(array("user"=>$user, "versions"=>$versions, "ssid"=>session_id()));
		}else{
			$this->error("register fail");
		}
		*/
		if($id){
				$user = $user_model->get($id, true, true);
				$version_model = new Version_model();
				$versions = $version_model->get_master();
				$this->setSessionData("user", $user);
				$this->out(array("user"=>$user, "versions"=>$versions, "ssid"=>session_id()));
		}else{
			$this->error("register fail");
		}
	}
}
