document.getElementById("password").onblur = checkPwd
document.getElementById("checkPassword").onblur = checkCheckPassword
let flagCheckpwd = false, flagCheckCheckpwd = false, flagCheckEmail = false;


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