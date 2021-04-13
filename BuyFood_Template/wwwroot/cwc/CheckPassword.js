document.getElementById("password").onblur = checkPwd;
document.getElementById("checkPassword").onblur = checkCheckPassword;
let flagCheckpwd = false;
let flagCheckCheckpwd = false;

function checkPwd() {
    let pwdObj = document.getElementById("password");
    let pwdObjVal = pwdObj.value;
    let spObj = document.getElementById("Wrongpassword");
    let pwdObjValLen = pwdObjVal.length;
    let flag1 = false, flag2 = false;

    if (pwdObjValLen <= 6) {
        spObj.innerHTML = "密碼必須大於6個字"
    }
    else if (pwdObjValLen > 6) {
        for (let i = 0; i < pwdObjValLen; i++) {
            let ch = pwdObjVal.charAt(i).toUpperCase();
            if (ch >= "A" && ch <= "Z")
                flag1 = true;
            else if (ch >= "0" && ch <= "9")
                flag2 = true;

            if (flag1 == true && flag2 == true)
                break;
        }

        if (flag1 == true && flag2 == true) {
            spObj.innerHTML = "密碼格式正確";
            flagCheckpwd = true;
        }
        else
            spObj.innerHTML = "密碼格式錯誤"
    }
}

function checkCheckPassword() {
    let pwdObjVal = document.getElementById("password").value;
    let checkPwdObjVal = document.getElementById("checkPassword").value;
    let spObj = document.getElementById("WrongCheckpassword");

    if (pwdObjVal == "") {
        spObj.innerHTML = "";
    }
    else if (pwdObjVal != checkPwdObjVal) {
        spObj.innerHTML = "密碼確認錯誤"
    }
    else if (pwdObjVal == checkPwdObjVal) {
        spObj.innerHTML = "密碼確認相同";
        flagCheckCheckpwd = true;
    }
}

var cwc_bool_showchangePassword = false;

function cwc_showChangPassword() {
    if (!cwc_bool_showchangePassword) {
        $("#aaa").css("display", "block");
        $("#cwc_i_div_bodyLeft").css("height", "800px");
        $("#content_cwc").css("height", "800px");
        $("#cwc_i_div_wholeDiv").css("height", "950px");
        cwc_bool_showchangePassword = true;
    }
    else {
        $("#aaa").css("display", "none");
        $("#cwc_i_div_bodyLeft").css("height", "650px");
        $("#content_cwc").css("height", "650px");
        $("#cwc_i_div_wholeDiv").css("height", "800px");
        $("#cwc_orP")[0].value = "";
        $("#password")[0].value = "";
        $("#checkPassword")[0].value = "";
        cwc_bool_showchangePassword = false;
    }
}

function cwc_savePassword(memberID, meth) {
    if (meth == 1) {
        if (!flagCheckpwd)
            return alert("新密碼輸入有誤");
        if (!flagCheckCheckpwd)
            return alert("新密碼確認錯誤");
        var oPassword = $("#cwc_orP").val();
        var nPassword = $("#password").val();

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/Member/savePassword",
            data: JSON.stringify({ "memberID": `${memberID}`, "oPassword": `${oPassword}`, "nPassword": `${nPassword}` }),
            success: function (data) {
                if (data == "1")
                    return alert("密碼錯誤，請重新輸入。");
                cwc_showChangPassword()
                alert("變更成功");
            }
        })
    }
    else {
        cwc_showChangPassword()
    }
}