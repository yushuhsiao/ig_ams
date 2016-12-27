<?php
$url="https://kioskpublicapi.redhorse88.com/createPlayer/TEST011/PPLAYDEV/PPLAYDEV/password/abcde12";
$result=getdata($url);
print_r($result);

function getdata($url){
	$path = dirname(__FILE__);
	$entity_key = '27f6733f33367660f21910bc73c43ad2a3bdd29f6e09daefa15432bbf27e1b976fcc0b982d69d0c211eb2c69f51a73ddf8d07b5746aca6df234e8aa467e8583e';
	$header   = array();
	$header[] = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"; 
	$header[] = "Cache-Control: max-age=0"; 
	$header[] = "Connection: keep-alive"; 
	$header[] = "Keep-Alive:timeout=5, max=100"; 
	$header[] = "Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3"; 
	$header[] = "Accept-Language:es-ES,es;q=0.8"; 
	$header[] = "Pragma: "; 
	$header[] = "X_ENTITY_KEY: " . $entity_key; 
	
	$tuCurl = curl_init();
   
	curl_setopt($tuCurl, CURLOPT_URL, $url);
    curl_setopt($tuCurl, CURLOPT_PORT , 443);
    curl_setopt($tuCurl, CURLOPT_VERBOSE, 0);
    curl_setopt($tuCurl, CURLOPT_HTTPHEADER, $header);
    curl_setopt($tuCurl, CURLOPT_SSL_VERIFYPEER, 0);
    curl_setopt($tuCurl, CURLOPT_SSL_VERIFYHOST, 0);
    curl_setopt($tuCurl, CURLOPT_SSLCERT, $path . '/api/play.pem');
    curl_setopt($tuCurl, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($tuCurl, CURLOPT_SSLKEY, $path . '/api/play.key');	   

	echo $exec = curl_exec($tuCurl);
	
	curl_close($tuCurl);
	print_r($exec);

}
?>