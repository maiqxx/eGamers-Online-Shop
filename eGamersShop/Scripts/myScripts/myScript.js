
function getImagePreview() {
    var txt = (event.target.files[0].name).split(".").pop().toLowerCase();
    if (txt == "jpg" || txt == "png" || txt == "gif" || txt == "bmp") {
        var image = URL.createObjectURL(event.target.files[0]);
        var imagediv = document.getElementById("preview");
        var newimg = document.createElement('img');
        newimg.src = image;
        newimg.width = "200";
        imagediv.appendChild(newimg);
    } else {
        alert("File uploaded is invalid");
        $("#preview").html("");
        $("#uploadImg").val("");
    }
}

$(function () {
    var numCode;
    
    $.post("../home/getItemCode", {
        
    }, function (res) {
        var num = parseInt(res[0].itmcode) + 1;
        numCode = $("#txtCode").val("I000000000" + num);
        $("#txtCode").prop('disabled', true);
    });
    
    $("#txtCode").val(numCode);
    $("#productRecord").submit(function () {
        $("#txtCode").prop('disabled', false);
    });


});