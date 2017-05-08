var readURL = function (input, destination) { 
    if (input.files && input.files[0]) {
        var reader = new FileReader();       
        reader.onload = function (e) {           
            $(destination).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);      
    }
};