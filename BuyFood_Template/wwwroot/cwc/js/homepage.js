$("#cwc_home_btn_memberCenter").attr("onclick","checkLogin(0,'/Member/MemberCenter')");
$("#cwc_home_btn_memberCombo").attr("onclick","checkLogin(2,'/Member/MemberCenter')");

function checkLogin(goto,location) {
    $.ajax({
        url: `/Member/checkLogin?id=${goto}`,
        success: function (result) {
            if (result == "1")
                window.location.href = location;

            else {
                //window.location.href = `/Member/MemberCenter`;
                window.alert("請先登入會員");
                window.location.href = `/HomePage/登入`;
            }

        }
    });

}

//$.ajax({
//    url:`/Combo/`,
//    success: function (homeCombodata) {

//        const forCombo = dataForHomePage.forCombo;
//        if (forCombo != 0 && forCombo.length > 0) {
//            $("#cwc_howManyCombo").html(forCombo.length);
//            var cwc_pageCombo = "";
//            for (var i = 0; i < forCombo.length; i++) {
//                cwc_pageCombo += `<div style="display:inline-block">${forCombo[i].cComboName}</div><button class="btn btn-success btn-sm">訂購</button>`
//            }
//            $("#cwc_div_homePageCombo").html(cwc_pageCombo);

//        }
//    }

//})