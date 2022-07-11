
$().ready(function () {
    $("#btnEdit").hide();
    $("#btnUpdate").hide();
    $("#itmdesc").attr("disabled", "disabled");
    $("#itmprice").attr("disabled", "disabled");
    $("#itmonhand").attr("disabled", "disabled");

    $("#btnSearch").click(function () {

        $("#btnEdit").attr("disabled", "disabled");
        var itemcode = $("#itmcde").val();

        $.post("../Home/SearchItem", {
            itemcode: itemcode
        }, function (res) {
            if (res[0].mess == 0) {
                $("#btnEdit").removeAttr("disabled");
                $("#btnEdit").show();
                $("#itmdesc").val(res[0].desc);
                $("#itmprice").val(res[0].price);
            } else {
                alert('Invalid Item code');
            }
        });
    });

    $("#btnEdit").click(function () {
        $("#btnEdit").hide();
        $("#btnUpdate").show();
        $("#itmcde").attr("disabled", true);
        $("#itmdesc").removeAttr("disabled");
        $("#itmprice").removeAttr("disabled");
        //alert($("#itmdesc").val());
    });

    $("#btnUpdate").click(function () {
        alert('test');
        var itemdesc = $("#itmdesc").val();
        var itemprice = $("#itmprice").val();
        var itemcode = $("#itmcde").val();
        var itmonhand = $("#itmonhand").val();

        $.post("../Home/UpdateItem", {
            itemdesc: itemdesc,
            itemprice: itemprice,
            itemcode: itemcode,
            itmonhand: itmonhand
        }, function (res) {
            if (res[0].mess == 0) {
                alert("Successfully Updated!");
                $("#btnUpdate").hide();
                $("#itmcde").removeAttr("disabled").val();
                $("#itmdesc").attr("disabled", true).val();
                $("#itmprice").attr("disabled", true).val();
                /*  $("#itmdesc").val("");
                  $("#itmprice").val("");
                  $("#itmcde").val("");*/

            } else
                alert("Ooooppppssss.. Something Wrong");
        });
    });

});
