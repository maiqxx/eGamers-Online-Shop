$().ready(function () {
    $('#btnRegister').click(function () {

        $("select.userRole").change(function () {
            var selectedRole = $(this).children("option:selected").val();

            $.post("../Home/getSelectedRole", selectedRole {
                role: selectedRole
            }, function (res) {

                if (res[0].mess == 0) {
                    alert("Signed Up as a " + selectedRole);
                } else {
                    alert("Something went wrong... Please try again.");
                }
            });
        });
        
        
    });
});