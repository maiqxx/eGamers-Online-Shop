
$().ready(function () {
    $("#btnEdit").hide();
    $("#btnDelete").hide();
    $("#btnUpdate").hide();
    $("#itmdesc").attr("disabled", "disabled");
    $("#itmprice").attr("disabled", "disabled");
    $("#itmonhand").attr("disabled", "disabled");

    $("#btnSearch").click(function () {

        $("#btnEdit").attr("disabled", "disabled");
        $("#btnDelete").attr("disabled", "disabled");
        var itemcode = $("#itmcde").val();

        $.post("../Home/SearchItem", {
            itemcode: itemcode
        }, function (res) {
            if (res[0].mess == 0) {
                $("#btnEdit").removeAttr("disabled");
                $("#btnEdit").show();
                $("#btnDelete").removeAttr("disabled");
                $("#btnDelete").show();
                $("#itmdesc").val(res[0].desc);
                $("#itmprice").val(res[0].price);
                $("#itmonhand").val(res[0].qty);
            } else {
                alert('Invalid Item code');
            }
        });
    });


    //for edit button
    $("#btnEdit").click(function () {
        $("#btnEdit").hide();
        $("#btnDelete").hide();
        $("#btnUpdate").show();
        $("#itmcde").attr("disabled", true);
        $("#itmdesc").removeAttr("disabled");
        $("#itmprice").removeAttr("disabled");
        $("#itmonhand").removeAttr("disabled");
        //alert($("#itmdesc").val());
    });


    //for delete button
    $("#btnDelete").click(function () {
        var itemcode = $("#itmcde").val();

        $.post("../Home/DeleteItem", {
            itemcode: itemcode
        }, function (res) {
            $("#btnUpdate").hide();
            //if (res[0].mess == 0) {
            //    alert('Delete this item?');
            //    alert("Item successfully deleted!");

            //} else {
            //    alert("Oops! Something Wrong!");
            //}

        });

    });


    //for update button
    $("#btnUpdate").click(function () {
        alert('Updating...');
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
                $("#btnDelete").hide();
                $("#itmcde").removeAttr("disabled").val();
                $("#itmdesc").attr("disabled", true).val();
                $("#itmprice").attr("disabled", true).val();
                $("#itmonhand").attr("disabled", true).val();
                /*  $("#itmdesc").val("");
                  $("#itmprice").val("");
                  $("#itmcde").val("");*/

            } else
                alert("Oops! Something Wrong!");
        });
    });

});
