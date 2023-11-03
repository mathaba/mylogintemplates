

<?php
   
$targetFilePath2 = "./uploads/Cards/" ;
$img =  $_POST['imgBase64'];  
$name =  $_POST['name']; 
//$image_parts = explode(";base64,", $img); 
  //  $image_type_aux = explode("image/", $image_parts[0]);
   // $image_type = $image_type_aux[1];
	//$image_base64 = base64_decode($image_parts[1]);
$img = str_replace('data:image/png;base64,', '', $img);
    $img = str_replace(' ', '+', $img);
    $data = base64_decode($img);
    //$sig = $name."_".$NA_ID. "_sig_" .uniqid() .'.'.$image_type;

    $file = $targetFilePath2.$name. '.jpg';
    
   $upload2 =  file_put_contents($file, $data);
   	unlink('./uploads/Cards/'.$name.'.pdf');
?>

