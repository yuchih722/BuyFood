//for Profile done
function cwc_showReviseProfile(memberID) {
    var memberName = $("#cwc_span_ProfileName").html();
    var txt = `<input id="cwc_input_ProfileName" type="text" value="${escapeHtml(memberName)}" onblur="cwc_saveReviseProfile(${memberID})" style="padding-bottom:0px"/>`
    $("#cwc_span_ProfileName").html(txt);
    $("#cwc_button_ReviseProfile").attr("onclick", "");
    $("#cwc_input_ProfileName").focus();
}

//done
function cwc_saveReviseProfile(memberID) {
    var newName = $("#cwc_input_ProfileName").val();

    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "/Member/saveProfile",
        data: JSON.stringify({ "CName": `${newName}`, "CMemberId": memberID }),
        dataType: "json",
        success: function (data) {
            $("#cwc_span_ProfileName").html(data.cName);
            $("#cwc_button_ReviseProfile").attr("onclick", `cwc_showReviseProfile(${memberID})`);
            $("#getLoginName").html(`<i class="fa fa-user"></i>${data.cName}`)
        }
    })
}
function cwc_changeProfilePhoto(memberID) {
    console.log("OK")


}

//跑圖表

