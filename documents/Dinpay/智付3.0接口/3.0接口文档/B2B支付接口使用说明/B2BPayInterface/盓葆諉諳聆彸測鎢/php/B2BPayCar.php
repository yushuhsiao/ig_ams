<?php

$flag = false;
$sb = "";

$bank_code = $_POST['bank_code'];
if($bank_code!=null && $bank_code!="")
{
	$sb = $sb."bank_code=".$bank_code;
	$flag = true;
}

$client_ip = $_POST['client_ip'];
if($client_ip!=null && $client_ip!="")
{
        if($flag){
            $sb = $sb."&client_ip=".$client_ip;
        }else{
            $sb = $sb."client_ip=".$client_ip;
        }
	$flag = true;
}

$extend_param = $_POST['extend_param'];
if($extend_param!=null && $extend_param!="")
{
        if($flag){
            $sb = $sb."&extend_param=".$extend_param;
        }else{
            $sb = $sb."extend_param=".$extend_param;
        }
	$flag = true;
}

$extra_return_param = $_POST['extra_return_param'];
if($extra_return_param!=null && $extra_return_param!="")
{
        if($flag){
            $sb = $sb."&extra_return_param=".$extra_return_param;
        }else{
            $sb = $sb."extra_return_param=".$extra_return_param;
        }
	$flag = true;
}

$input_charset = $_POST['input_charset'];
if($input_charset!=null && $input_charset!="")
{
        if($flag){
            $sb = $sb."&input_charset=".$input_charset;
        }else{
            $sb = $sb."input_charset=".$input_charset;
        }
	$flag = true;
}

$interface_version = $_POST['interface_version'];
if($interface_version!=null && $interface_version!="")
{
        $sb = $sb."&interface_version=".$interface_version;
	$flag = true;
}

$merchant_code = $_POST['merchant_code'];
if($merchant_code!=null && $merchant_code!="")
{
        $sb = $sb."&merchant_code=".$merchant_code;
	$flag = true;
}

$notify_url = $_POST['notify_url'];
if($notify_url!=null && $notify_url!="")
{
        $sb = $sb."&"."notify_url=".$notify_url;
	$flag = true;
}

$order_amount = $_POST['order_amount'];
if($order_amount!=null && $order_amount!="")
{
        $sb = $sb."&order_amount=".$order_amount;
	$flag = true;
}

$order_no = $_POST['order_no'];
if($order_no!=null && $order_no!="")
{
        $sb = $sb."&order_no=".$order_no;
	$flag = true;
}

$order_time = $_POST['order_time'];
if($order_time!=null && $order_time!="")
{
        $sb = $sb."&order_time=".$order_time;
	$flag = true;
}

$product_code = $_POST['product_code'];
if($product_code!=null && $product_code!="")
{
        $sb = $sb."&product_code=".$product_code;
	$flag = true;
}

$product_desc = $_POST['product_desc'];
if($product_desc!=null && $product_desc!="")
{
        $sb = $sb."&product_desc=".$product_desc;
	$flag = true;
}

$product_name = $_POST['product_name'];
if($product_name!=null && $product_name!="")
{
        $sb = $sb."&product_name=".$product_name;
	$flag = true;
}

$product_num = $_POST['product_num'];
if($product_num!=null && $product_num!="")
{
        $sb = $sb."&product_num=".$product_num;
	$flag = true;
}

$return_url = $_POST['return_url'];
if($return_url!=null && $return_url!="")
{
        $sb = $sb."&return_url=".$return_url;
	$flag = true;
}

$service_type = $_POST['service_type'];
if($service_type!=null && $service_type!="")
{
        $sb = $sb."&service_type=".$service_type;
	$flag = true;
}

$show_url = $_POST['show_url'];
if($show_url!=null && $show_url!="")
{
        $sb = $sb."&show_url=".$show_url;
	$flag = true;
}

$sign_type = $_POST['sign_type'];

$key="123456789a123456789_";
$sb = $sb."&key={".$key."}";
$sign = strtoupper(md5($sb));
//echo $sb."<br>";
//echo $sign;
?>
<html>
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>模拟商户提交</title>
  </head>
<body  onload="document.form1.submit();">
    <form action="https://payment.dinpay.com/B2BReceiveMerchantAction.do" name="form1" method="POST">

            <input type="hidden" name="service_type" size="40" value="<?echo $service_type ?>" />

            <input type="hidden" name="merchant_code" size="40" value="<?echo $merchant_code ?>" />

            <input type="hidden" name="input_charset" size="40" value="<?echo $input_charset ?>" />

            <input type="hidden" name="notify_url" size="40" value="<?echo $notify_url ?>" />

            <input type="hidden" name="return_url" size="40" value="<?echo $return_url ?>" />

            <input type="hidden" name="client_ip" size="40" value="<?echo $client_ip ?>" />

            <input type="hidden" name="interface_version" size="40" value="<?echo $interface_version ?>" />

            <input type="hidden" name="sign_type" size="40" value="<?echo $sign_type ?>" />

            <input type="hidden" name="order_no" size="40" value="<?echo $order_no ?>" />

            <input type="hidden" name="order_time" size="40" value="<?echo $order_time ?>" />

            <input type="hidden" name="order_amount" size="40" value="<?echo $order_amount ?>" />

            <input type="hidden" name="product_name" value="<?echo $product_name ?>" size="40" />

            <input type="hidden" name="show_url" size="40" value="<?echo $show_url ?>" />

            <input type="hidden" name="product_code" size="40" value="<?echo $product_code ?>" />

            <input type="hidden" name="product_num" size="40" value="<?echo $product_num ?>" />

            <input type="hidden" name="product_desc" size="40" value="<?echo $product_desc ?>" />

            <input type="hidden" name="bank_code" size="40" value="<?echo $bank_code ?>" />

            <input type="hidden" name="extra_return_param" size="40"value="<?echo $extra_return_param ?>" />

            <input type="hidden" name="extend_param" size="40" value="<?echo $extend_param ?>" />

            <input type="hidden" name="sign" size="" value="<?echo $sign ?>" />
    </form>

  </body>
</html>
