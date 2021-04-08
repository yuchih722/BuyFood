$("#cwc_home_btn_memberCenter").click(function () {
    $.ajax({
        url: "/Member/checkLogin",
        success: function (result) {
            if (result == "1")
                window.location.href = `/Member/MemberCenter`;

            else {
                //window.location.href = `/Member/MemberCenter`;
                window.alert("請先登入會員");
                window.location.href = `/HomePage/登入`;
            }

        }
    });
});