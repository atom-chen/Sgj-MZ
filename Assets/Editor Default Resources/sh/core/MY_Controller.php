<?php 
class MY_Controller {
	var $args;
	var $needAuth = false;
	function __construct() {
		($this->args = $_POST) || ($this->args = $_GET);
		if($this->args["ssid"]){
			session_id($this->args["ssid"]);
		}
		session_start();
		if(!$this->args["ssid"]){
			$_SESSION = array();
		}
		if($this->needAuth){
			$this->checkAuth();
		}
		//共通Model
		load_model(array('user_model', 'mission_model'));
	}
	protected function log($message){
		$fp = fopen('/var/www/d/test/test.txt', 'a');
		fwrite($fp, $message."\n");
		fclose($fp);
	}
	protected function error($message){
		die(json_encode(array("result"=>0,"message"=>$message)));
	}
	protected function out($arr){
		if(empty($arr)){
			$arr = array();
		}
		$arr["now"] = date("Y-m-d H:i:s");
		$arr["result"] = 1;
		//MY_Model::CloseDB();
		die(json_encode($arr));
	}
	protected function checkParam($list){
		foreach ($list as $value) {
			if(!isset($this->args[$value])){
				$this->error("Param error : " . $value);
			}
		}
	}
	protected function getSessionData($key){
		return $_SESSION[$key];
	}
	protected function setSessionData($key,$value){
		$_SESSION[$key] = $value;
	}
	protected function checkAuth(){
		$user = $this->getSessionData("user");
		if(empty($user)){
			$this->error("sessionError");
		}
	}
}
