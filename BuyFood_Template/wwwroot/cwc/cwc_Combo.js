function cwc_showCombo(memberID) {
    $.ajax({
        url: "/Combo/getCombo?id=" + memberID,
        type: "GET",
        success: function (data) {
            var txthead = `<h3 class="inline">我的套餐</h3><div class="btn btn-success inline" onclick="cwc_EditCombo(0,'套餐${data.length + 1}',${memberID})">新增套餐</div>`

            var txt = `<table class="table accordion" id="mycombo"><thead><tr><td>套餐名稱<td>餐點數量<td>套餐總額<td>修改<td>刪除<tbody>`;
            for (var i = 0; i < data.length; i++) {
                var issue = data[i].notSaleItem > 0 ? `(部分餐點已停供)` : ``;
                txt += `<tr  class="accordion-toggle"
                                            data-toggle="collapse"
                                            data-target="#cwc_comboDetail_div_${data[i].comboID}"
                                            id="cwc_combo_tr_${data[i].comboID}">
                                        <td>${data[i].comboName}${issue}
                                        <td>${data[i].totalItems}
                                        <td>${data[i].totalPrice}
                                        <td><div class="btn btn-success btn-sm" onclick="cwc_EditCombo(${data[i].comboID},'${data[i].comboName}',${memberID})">修改</div>
                                        <td><div class="btn btn-danger btn-sm" onclick="cwc_deleteCombo(${data[i].comboID},${memberID})">刪除</div>
                                    <tr id="cwc_comboDetail_tr_${data[i].comboID}">
                                        <td class="hiddenRow" colspan="5">
                                            <div class="accordian-body collapse" data-parent="#mycombo" id="cwc_comboDetail_div_${data[i].comboID}">
                                                <table class="table table-success table-sm" style="text-align:center">
                                                    <thead><tr><td>餐點<td>數量<td>單價
                                                    <tbody>`;
                for (var p = 0; p < data[i].comboDetail.length; p++) {
                    var issue = data[i].comboDetail[p].onSale == 3 ? `(停止販售)` : ``
                    txt += `<tr><td>${data[i].comboDetail[p].productName}${issue}<td>${data[i].comboDetail[p].qty}<td>${data[i].comboDetail[p].price}`;
                }
                txt += `</tbody></thead></table></div>`;
            }
            txt += `</tbody>`;
            $("#head_cwc").html(txthead);
            $("#content_cwc").html(txt);
            updateData(memberID);
        }
    });
}

function cwc_EditCombo(comboID, comboName, memberID) {
    console.log(1);
    var layoutTopSet = `<div style="width:50%; height:inherit; float:left">
                                                <div style="height:50px">
                                                    <input id="cwc_input_comboName" class="inline" type="text" value="${escapeHtml(comboName)}"/>
                                                    <div id="cwc_button_saveCombo" class="inline btn btn-success" onclick="cwc_saveCombo(${comboID},${memberID},'${escapeHtml(comboName)}')">儲存</div>
                                                </div>
                                            </div>
                                            <div style="width:50%; height:inherit; float:left">
                                                <div style="height:50px">
                                                    <span>餐點類型</span>
                                                    <select class="inline" id="cwc_select_Category"></select>
                                                    <label><input id="cwc_checkbox_b" class="inline" type="checkbox" />7:00~10:30
                                                    <label><input id="cwc_checkbox_l" class="inline" type="checkbox"  />10:30~14:30
                                                    <label><input id="cwc_checkbox_d" class="inline" type="checkbox" />14:30~21:00
                                                </div>

                                             </div>`;
    var layoutBottomSet = `<div id="cwc_combo_leftContent" style="width:50%; height:inherit; float:left; overflow:scroll"></div>
                                                        <div id="cwc_combo_rightContent" style="width:50%; height:inherit; float:left; overflow:scroll"></div>`
    $("#head_cwc").html(layoutTopSet);
    $("#content_cwc").html(layoutBottomSet);

    $.ajax({
        url: "/Combo/getComboDetail?id=" + comboID,
        type: "GET",
        success: function (data) {
            var txt = `<table class="table table-success table-striped" style="text-align:center"><thead><tr><td>餐點<td>數量<td>單價<td>刪除
                                        <tbody id="cwc_combo_tbody">`;
            for (var i = 0; i < data.length; i++) {
                checkboxChecked(data[i].br, data[i].lu, data[i].di, 0);
                txt += cwc_new_tr_ProductInCombo(data[i].productID, data[i].categoryID, data[i].productName, data[i].unitPrice, data[i].quantity, data[i].productOn, data[i].br, data[i].lu, data[i].di);
            }
            txt += `</tbody>`;
            $("#cwc_combo_leftContent").html(txt);
            $.ajax({
                url: "/ProductCategory/getAllCategory",
                success: function (allCategory) {
                    var txt = "";
                    var selectedCategoryID = 1;
                    for (var i = 0; i < allCategory.length; i++) {
                        if (i != 0)
                            txt += `<option value="${allCategory[i].cProductCategoryId}">
                                                            ${allCategory[i].cCategoryName}`;
                        else {

                            selectedCategoryID = allCategory[i].cProductCategoryId;
                            txt += `<option selected value="${allCategory[i].cProductCategoryId}">
                                                            ${allCategory[i].cCategoryName}`;
                        }
                    };
                    $("#cwc_select_Category").html(txt).attr("onchange", `cwc_Category_selected(this.options[this.options.selectedIndex].value)`);
                    $("#cwc_checkbox_b").attr("onclick", "cwc_checkoffer()");
                    $("#cwc_checkbox_l").attr("onclick", "cwc_checkoffer()");
                    $("#cwc_checkbox_d").attr("onclick", "cwc_checkoffer()");
                    cwc_Category_selected(selectedCategoryID);
                }
            });
        }
    });
}

function cwc_deleteCombo(comboID, memberID) {

    $(`#cwc_combo_tr_${comboID}`).remove();
    $(`#cwc_comboDetail_tr_${comboID}`).remove();
    $.ajax({
        url: `Combo/deleteCombo?id=${comboID}`,
        success: function (data) {
            updateData(memberID);
        }
    });
}

function cwc_new_tr_ProductInCombo(productID, categoryID, productName, unitPrice, qty, onsale, br, lu, di) {
    txt = `<tr class="cwc_combo_trs" id="cwc_combo_tr_${productID}">
                                        <input class="cwc_input_tr_productID" type="hidden" value="${productID}"/input>
                                        <input class="cwc_input_tr_br" type="hidden" value="${br}"/input>
                                        <input class="cwc_input_tr_lu" type="hidden" value="${lu}"/input>
                                        <input class="cwc_input_tr_di" type="hidden" value="${di}"/input>
                                        <td>${productName}
                                        <td>
                                                <button onclick="cwc_dash('combo_qty_${productID}')" class="btn btn-success btn-sm">-</button>
                                                <span id="combo_qty_${productID}">${qty}</span>
                                                <button onclick="cwc_plus('combo_qty_${productID}')" class="btn btn-success btn-sm">+</button>
                                        <td>${unitPrice}
                                                                                <td><div class="btn btn-danger btn-sm" onclick="cwc_DeleteProductInCombo(
                                                                                                        ${productID},
                                                                                                        ${categoryID},
                                                                                                        '${productName}',
                                                                                                        ${unitPrice},
                                                                                                        ${onsale},
                                                                                                        ${br},
                                                                                                        ${lu},
                                                                                                        ${di})">刪除</div>`;
    return txt;
}

function cwc_new_tr_ProductInOption(productID, categoryID, productName, unitPrice, onsale, br, lu, di) {
    var availableTime = "";
    availableTime += br == 1 ? "早" : "";
    availableTime += lu == 1 ? (availableTime == "" ? "午" : "、午") : "";
    availableTime += di == 1 ? (availableTime == "" ? "晚" : "、晚") : "";

    var txt = `<tr class="cwc_product_trs" id="cwc_product_tr_${productID}">
                                        <input type="hidden" value="${productID}"/input>
                                        <td>${productName}
                                        <td>${unitPrice}
                                        <td>${availableTime}
                                        <td><div class="btn btn-success btn-sm"
                                                            onclick="cwc_AddProducttoCombo(
                                                                            ${productID},
                                                                            '${productName}',
                                                                            ${unitPrice},
                                                                            ${categoryID},
                                                                            ${onsale},
                                                                            ${br},
                                                                            ${lu},
                                                                            ${di})">加入</div>`;
    return txt;
}

function cwc_checkoffer() {
    resetProductList();
}

function cwc_dash(targetID) {
    var qty = parseInt($(`#${targetID}`).html());
    if (qty > 1) qty--;
    else qty = 1;
    $(`#${targetID}`).html(`${qty}`);
}

function cwc_plus(targetID) {

    var qty = parseInt($(`#${targetID}`).html());
    qty++;
    $(`#${targetID}`).html(`${qty}`);

}

function cwc_AddProducttoCombo(productID, productName, unitPrice, categoryID, onsale, br, lu, di) {
    var txt = cwc_new_tr_ProductInCombo(productID, categoryID, productName, unitPrice, 1, onsale, br, lu, di);
    $("#cwc_combo_tbody").append(txt);
    $(`#cwc_product_tr_${productID}`).remove();
    checkboxChecked(br, lu, di, 1);
}

function cwc_DeleteProductInCombo(productID, categoryID, productName, unitPrice, onsale, br, lu, di) {

    $(`#cwc_combo_tr_${productID}`).remove();
    if (onsale != 3) {
        var doublebr = br == 0 ? true : false;
        var doublelu = lu == 0 ? true : false;
        var doubledi = di == 0 ? true : false;
        for (var i = 0; i < $(`.cwc_combo_trs`).length; i++) {
            var brcheck = $(`.cwc_combo_trs`).children(".cwc_input_tr_br")[i].attributes["value"].nodeValue;
            var lucheck = $(`.cwc_combo_trs`).children(".cwc_input_tr_lu")[i].attributes["value"].nodeValue;
            var dicheck = $(`.cwc_combo_trs`).children(".cwc_input_tr_di")[i].attributes["value"].nodeValue;
            doublebr = (brcheck == 0) ? false : doublebr;
            doublelu = (lucheck == 0) ? false : doublelu;
            doubledi = (dicheck == 0) ? false : doubledi;
        }
        if (doublebr || doublelu || doubledi) {
            if (doublebr) $("#cwc_checkbox_b").prop("disabled", false);
            if (doublelu) $("#cwc_checkbox_l").prop("disabled", false);
            if (doubledi) $("#cwc_checkbox_d").prop("disabled", false);
            resetProductList();

        }
        else {
            if (categoryID ==
                $("#cwc_select_Category")[0].options[$("#cwc_select_Category")[0].options.selectedIndex].attributes["value"].nodeValue) {
                var txt = cwc_new_tr_ProductInOption(productID, categoryID, productName, unitPrice, onsale, br, lu, di);
                $("#cwc_product_tbody").append(txt);
            }
        }
    }
}

function cwc_saveCombo(comboID, memberID, comboName) {
    console.log("in");
    $("#cwc_button_saveCombo").attr("onclick", "");
    var newcomboName = $("#cwc_input_comboName").val() == "" ? escapeHtml(comboName) : escapeHtml($("#cwc_input_comboName").val());
    var db = new Array();
    for (var i = 0; i < $(`.cwc_combo_trs`).length; i++) {
        var productID = $(`.cwc_combo_trs`).children(".cwc_input_tr_productID")[i].attributes["value"].nodeValue;
        var qty = $(`#combo_qty_${productID}`).html();
        var data = {};
        data.cID = comboID;
        data.pID = parseInt(productID);
        data.q = parseInt(qty);
        data.mID = memberID;
        data.cName = `${newcomboName}`;
        db.push(data);
    }
    console.log(123)
    if (db.length == 0) {
        if (comboID == 0)
            cwc_showCombo(memberID);
        else {
            $.ajax({
                url: `Combo/deleteCombo?id=${comboID}`,
                success: function () {
                    cwc_showCombo(memberID);
                    updateData(memberID);
                }
            });
        }
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Combo/saveCombo",
            data: JSON.stringify(db),
            success: function (data) {
                cwc_showCombo(memberID);
                updateData(memberID);
            }
        });
    }


}

function cwc_Category_selected(CategoryID) {
    var br = $("#cwc_checkbox_b").prop("disabled")
    var lu = $("#cwc_checkbox_l").prop("disabled")
    var di = $("#cwc_checkbox_d").prop("disabled")

    var selectb = $("#cwc_checkbox_b").prop("checked") ? 1 : 0;
    var selectl = $("#cwc_checkbox_l").prop("checked") ? 1 : 0;
    var selectd = $("#cwc_checkbox_d").prop("checked") ? 1 : 0;

    $.ajax({
        url: `/ProductDetail/getProductsByCategory?id=${CategoryID}`,
        success: function (ProductList) {
            console.log(ProductList)
            var txt = `<table class="table table-success table-striped" style="text-align:center"><thead><tr><td>餐點<td>單價<td>供餐時段<td>新增
                                        <tbody id="cwc_product_tbody">`;

            for (var i = 0; i < ProductList.length; i++) {

                var b = selectb == 1 ? ProductList[i].cIsBreakFast == 1 ? true : false : true;
                var l = selectl == 1 ? ProductList[i].cIsLunch == 1 ? true : false : true;
                var d = selectd == 1 ? ProductList[i].cIsDinner == 1 ? true : false : true;

                if (b && l && d) {
                    var a = cwc_new_tr_ProductInOption(ProductList[i].cProductId, CategoryID, ProductList[i].cProductName,
                        ProductList[i].cPrice, ProductList[i].cIsOnSaleId, ProductList[i].cIsBreakFast, ProductList[i].cIsLunch, ProductList[i].cIsDinner)

                    var doubleP = false;
                    for (var x = 0; x < $(`.cwc_combo_trs`).length; x++) {
                        if (ProductList[i].cProductId == parseInt($(`.cwc_combo_trs`).children(".cwc_input_tr_productID")[x].attributes["value"].nodeValue)) {
                            doubleP = !doubleP;
                        }
                    };

                    if (!doubleP) {
                        txt += ((br == true && ProductList[i].cIsBreakFast == 1) || (lu == true && ProductList[i].cIsLunch == 1) || (di == true && ProductList[i].cIsDinner == 1)) ?
                            ((br == false && ProductList[i].cIsBreakFast == 1) || (lu == false && ProductList[i].cIsLunch == 1) || (di == false && ProductList[i].cIsDinner == 1)) ? a : ""
                            : a;
                    }
                }
            };

            txt += `</tbody>`;
            $("#cwc_combo_rightContent").html(txt);
        }
    });
}

function checkboxChecked(br, lu, di, state) {
    checkBoxDisabled($("#cwc_checkbox_b"), br, state);
    checkBoxDisabled($("#cwc_checkbox_l"), lu, state);
    checkBoxDisabled($("#cwc_checkbox_d"), di, state);
}

function checkBoxDisabled(target, targetAvailable, state) {
    var current = target.prop("disabled");
    var after = targetAvailable == 1 ? current : true;
    if (!current == after) {
        target.prop("disabled", true);
        if (state == 1) {
            resetProductList()
        }
    }
}
function resetProductList() {
    var CategoryID = $("#cwc_select_Category")[0].options[$("#cwc_select_Category")[0].options.selectedIndex].value;
    cwc_Category_selected(CategoryID);
}
