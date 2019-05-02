注册：
function regHGuser($name,$name2,$type='1'){
 	$firstname=substr($name2,0,3);
	$lastname=substr($name2,3);
	$poststr='<?xml version="1.0"?><request action="registration"><element><properties name="name">'.$name.'</properties><properties name="mode">'.$type.'</properties><properties name="firstname">'.$name.'</properties><properties name="lastname">'.$name.'</properties><properties name="currencyid">'.$currencyid.'</properties><properties name="agentid">'.$agentid.'</properties><properties name="affiliateid"></properties><properties name="testusr"></properties></element></request>';
	$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
	if(strpos($str,'<properties name="status">0</properties>')){
		$ticketid=cut($str,'<properties name="ticket">','</properties>');
		return $ticketid;
	}else{
		return false;
	}
}
获得hg金额：
function getHGmoney($name,$name2,$psw,$type='1'){
 
	$poststr='<?xml version="1.0"?><request action="accountbalance"><element><properties name="name">'.$name.'</properties><properties name="mode">'.$type.'</properties></element></request>';
	$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
	if(strpos($str,'<properties name="status">608</properties>')){
		regHGuser($name,$name2);
		$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
	}
	$HGmoney=0;
	if(strpos($str,'<properties name="status">0</properties>')){
		$HGmoney=cut($str,'<properties name="balance">','</properties>');
	}
	return $HGmoney;
}

转账：
function zhuanruHGmoeny($amount,$name,$name2,$psw,$type='1'){
 
	$refno='订单号';
	$poststr='<?xml version="1.0"?><request action="deposit"><element><properties name="name">'.$name.'</properties><properties name="mode">'.$type.'</properties><properties name="currencyid">'.$currencyid.'</properties><properties name="amount">'.$amount.'</properties>'.($refno ? '<properties name="refno">'.$refno.'</properties>' : '').($promoid ? '<properties name="promoid">'.$promoid.'</properties>' : '').($agentid ? '<properties name="agentid">'.$agentid.'</properties>' : '').'</element></request>';
	$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
	if(strpos($str,'<properties name="status">116</properties>')){
		regHGuser($name,$name2);
		$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
	}
	if(strpos($str,'<properties name="status">')){
		$status=cut($str,'<properties name="status">','</properties>');
		if($status=='0'){
			$paymentid=cut($str,'<properties name="paymentid">','</properties>');
			$poststr='<?xml version="1.0"?><request action="deposit-confirm"><element><properties name="status">'.$status.'</properties><properties name="paymentid">'.$paymentid.'</properties><properties name="errdesc"></properties></element></request>';
			$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
		
			if(strpos($str,'<properties name="status">')){
				$status=cut($str,'<properties name="status">','</properties>');
			}else{
			      return false
			}
		}
	}else{
		return false
	}
	return false
}
         
function zhuanchuHGmoeny($amount,$name,$name2,$psw,$type='1'){
 
	$refno='订单号';
	$poststr='<?xml version="1.0"?><request action="withdrawal"><element><properties name="name">'.$name.'</properties><properties name="mode">'.$type.'</properties><properties name="currencyid">'.$currencyid.'</properties><properties name="amount">'.$amount.'</properties>'</element></request>';
	$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
	if(strpos($str,'<properties name="status">116</properties>')){
		regHGuser($name,$name2);
		$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
	}
	if(strpos($str,'<properties name="status">')){
		$status=cut($str,'<properties name="status">','</properties>');
		if($status=='0'){
			$paymentid=cut($str,'<properties name="paymentid">','</properties>');
			$poststr='<?xml version="1.0"?><request action="withdrawal-confirm"><element><properties name="status">'.$status.'</properties><properties name="paymentid">'.$paymentid.'</properties><properties name="errdesc"></properties></element></request>';
			$str=post('https://longchenguat.hointeractive.com/cgibin/EGameIntegration';,$poststr);
			if(strpos($str,'<properties name="status">')){
				$status=cut($str,'<properties name="status">','</properties>');
			}else{
			   return false;
			}
		}
	}else{
	 return false;
	}
 return false;
}