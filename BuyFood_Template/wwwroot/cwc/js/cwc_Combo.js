function cwc_showCombo(memberID) {
    $.ajax({
        url: "/Combo/getCombo",
        type: "GET",
        success: function (data) {
            cwc_ComboDetails = data;
            if (data.length == 0) {
                var txt = `<div style="width:100%;height:100%;position:relative;display:flex;align-items:center;text-align:center">
                                        <div style="width:100%">
                                            <h1>自訂專屬自己的套餐</h1>
                                            <button class="btn btn-success" style="font-size:50px;width:250px;height:100" onclick="cwc_EditCombo(0,'套餐${data.length + 1}',${memberID})">自訂套餐</button>
                                         </div>
                                  </div>`;
                $("#head_cwc").html("");
                $("#content_cwc").html(txt);
                return;
            }
            var txthead = `<span style="font-size:30px; line-height:50px;position:absolute;left:30px">我的套餐</span><span id="cwc_btn_addCombo" class="btn btn-success" style="position:absolute;right:30px" onclick="cwc_EditCombo(0,'套餐${data.length + 1}',${memberID})">新增套餐</span>`

            var txt = `<table class="table accordion" id="mycombo"><thead><tr><td>套餐名稱<td>餐點數量<td>套餐總額<td>點餐<td>修改<td>刪除<tbody>`;
            for (var i = 0; i < data.length; i++) {
                var issue = data[i].comboNotSalesCount > 0 ? `(部分餐點已停供)` : ``;
                txt += `<tr  class="accordion-toggle"
                                            data-toggle="collapse"
                                            data-target="#cwc_comboDetail_div_${data[i].cComboId}"
                                            id="cwc_combo_tr_${data[i].cComboId}">
                                        <td>${data[i].cComboName}${issue}
                                        <td>${data[i].comboDetails.length}
                                        <td>${data[i].comboSum}
                                        <td><div class="btn btn-success btn-sm" onclick="cwc_addCombotoCart(cwc_ComboDetails[${i}])">加入購物車</div>
                                        <td><div class="btn btn-success btn-sm" onclick="cwc_EditCombo(${data[i].cComboId},'${data[i].cComboName}',${memberID})">修改</div>
                                        <td><div class="btn btn-danger btn-sm" onclick="cwc_deleteCombo(${data[i].cComboId},${memberID})">刪除</div>
                                    <tr id="cwc_comboDetail_tr_${data[i].cComboId}">
                                        <td class="hiddenRow" colspan="5">
                                            <div class="accordian-body collapse" data-parent="#mycombo" id="cwc_comboDetail_div_${data[i].cComboId}">
                                                <table class="table table-success table-sm" style="text-align:center">
                                                    <thead><tr><td>餐點<td>數量<td>單價<td>點餐
                                                    <tbody>`;
                var comboDetail = new Array();
                for (var q = 0; q < data[i].comboDetails.length; q++) {

                    var sameProduct = false
                    if (comboDetail.length > 0) {
                        for (var z = 0; z < comboDetail.length; z++) {
                            if (data[i].comboDetails[q].cProductId == comboDetail[z].cProduct.cProductId) {
                                sameProduct = true;
                                comboDetail[z].Count += 1;
                                break;
                            }
                        }
                        if (!sameProduct) {
                            var newdata = {};
                            newdata.cProduct = data[i].comboDetails[q].cProduct;
                            newdata.Count = 1;
                            comboDetail.push(newdata);
                        }
                    }
                    else {
                        var newdata = {};
                        newdata.cProduct = data[i].comboDetails[q].cProduct;
                        newdata.Count = 1;
                        comboDetail.push(newdata);
                    }
                }

                for (var p = 0; p < comboDetail.length; p++) {
                    var issue = comboDetail[p].cProduct.cIsOnSaleId == 3 ? `(停止販售)` : ``
                    txt += `<tr><td>${comboDetail[p].cProduct.cProductName}${issue}
                                        <td>${comboDetail[p].Count}
                                        <td>${comboDetail[p].cProduct.cPrice}
                                        <td><div class="btn btn-success btn-sm" onclick="cwc_addProducttoCart(cwc_ComboDetails[${i}].comboDetails[${p}].cProduct)">加入購物車</div>
                                        `;
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

    var layoutTopSet = `<div style="width:50%; height:inherit; float:left">
                                                <div style="height:50px">
                                                    <input id="cwc_input_comboName" class="inline" type="text" value="${escapeHtml(comboName)}"/>
                                                    <div id="cwc_button_saveCombo" class="inline btn btn-success" onclick="cwc_saveCombo(${comboID},${memberID},'${escapeHtml(comboName)}')">儲存</div>
                                                </div>
                                            </div>
                                            <div style="width:50%; height:inherit; float:left">
                                                <div style="height:50px">
                                                    <span>餐點類型</span>
                                                    <select class="inline" id="cwc_select_Category"></select></br>
                                                    <label><input id="cwc_checkbox_b" class="inline" type="checkbox" />5:00~10:00
                                                    <label><input id="cwc_checkbox_l" class="inline" type="checkbox"  />10:00~17:00
                                                    <label><input id="cwc_checkbox_d" class="inline" type="checkbox" />17:00~5:00
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
    var comboCount = $("#mycombo").length - 1;
    $("#cwc_btn_addCombo").attr("onclick", `cwc_EditCombo(0,'套餐${comboCount + 1}',${memberID})`)
    $.ajax({
        url: `/Combo/deleteCombo?id=${comboID}`,
        success: function (data) {
            updateData(memberID);
            updateLayoutCombo();
            if (comboCount == 0)
                cwc_showCombo(memberID);
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

    if (db.length == 0) {
        if (comboID == 0)
            cwc_showCombo(memberID);
        else {
            $.ajax({
                url: `/Combo/deleteCombo?id=${comboID}`,
                success: function () {
                    cwc_showCombo(memberID);
                    updateData(memberID);
                    updateLayoutCombo();
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
                updateLayoutCombo();
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
