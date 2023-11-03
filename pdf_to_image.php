

<script src="pdf.js"></script>
<script src="pdf.worker.js"></script>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.4/jquery.min.js"></script>
<form method = "post">
<div  >
		<canvas id="pdf" name="pdf" ></canvas>
</div>
</form>
<script>
	var canvas = document.getElementById('pdf');
    url = './hulul/uploads/contracts/hello_world_in_Arabic.pdf';   
	PDFJS.getDocument(url).then( doc => {
		
		doc.getPage(1).then(page =>{
			
			
			var ctx = canvas.getContext('2d');
			var viewport = page.getViewport(3);
            canvas.height = viewport.height;
            canvas.width = viewport.width;

            var renderContext = {
                canvasContext: ctx,
			viewport: viewport}
           
			page.render(renderContext).then(function() {
			var b64Image = canvas.toDataURL();
			 console.log(b64Image);
		$.ajax({
    type: "POST",
    url: "save_image_b64.php",
    data: { 
        imgBase64: b64Image
    }
}).done(function(o) {
    console.log('saved'); 
	 });
});
			
			
		});
		
	});
	

</script>	