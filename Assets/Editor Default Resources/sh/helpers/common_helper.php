<?php
if (!function_exists('load_model')) {
	function load_model($models) {
		foreach ($models as $model) {
			require_once(dirname(__FILE__)."/../models/{$model}.php");
		}
	}

}
?>