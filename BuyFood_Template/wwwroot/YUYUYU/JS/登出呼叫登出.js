   function 登出() {
        $.ajax({
            url: "/HomePage/SLogout",
            type: "POST",
            success: function (data) {
                window.alert(data);
                window.location.assign("/HomePage/Home")
            }
        })
    }