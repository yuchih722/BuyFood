


$(function () {
    var w = $("#mwt_slider_content").width();
    $("#mwt_slider_content").css("height", ($(window).height() - 20) + "px"); //將區塊自動撐滿畫面高度

    //$("#mwt_fb_tab").mouseover(function () { //滑鼠滑入時
    //    if ($("#mwt_mwt_slider_scroll").css('left') == '-' + w + 'px') {
    //        $("#mwt_mwt_slider_scroll").animate({ left: '-20px' }, 600, 'swing')
    //    }
    //});

    $("#mwt_fb_tab").click(function () { //滑鼠滑入時
        if ($("#mwt_mwt_slider_scroll").css('left') == '-' + w + 'px') {
            $("#mwt_mwt_slider_scroll").animate({ left: '-20px' }, 600, 'swing')
        }
    });

    $("#mwt_slider_content").mouseleave(function () {　//滑鼠離開後
        $("#mwt_mwt_slider_scroll").animate({ left: '-' + w + 'px' }, 600, 'swing');
    });

    //$("#mwt_fb_tab").click(function () {　//滑鼠離開後
    //    $("#mwt_mwt_slider_scroll").animate({ left: '-' + w + 'px' }, 600, 'swing');
    //});
});



/*篩選類別List*/

$.ajax({
    url: "/cProduct/getAllProductCategory",
    type: "Get",
    success: function (data) {
        txt = "";
        txt += `<button id="p_產品類別全部" class="btn btn-primary" value="All">全部產品</button></br>`
         //console.log(data);
        
        for (let i = 0; i< data.length; i++) {
           
            txt += `<button id="p_產品類別${i}" class="btn btn-primary" value="${data[i].cProductCategoryId}">${data[i].cCategoryName}</button></br>`
        }
        document.getElementById("mwt_slider_content").innerHTML = txt;

        for (let i = 0; i< data.length; i++) {
            $(`#p_產品類別${i}`).on("click", function () {

             /*   name = $(`#p_產品類別${i}`).attr("value");*/

                $.ajax({
                    url: "/cProduct/getCategoryJason",
                    type: "Get",
                    data: { "catNameId": $(`#p_產品類別${i}`).attr("value")},
                    success: function (data) {
                        console.log(data);
                        var txttable = "";
                       
                        txttable +=
                            `<div id="orderlist">
                            <table class="table table-hover table_striped table-bordered border" id="myTable">
                            <thead>
                            <tr>
                            <th width="50px" style="text-align:center">
                            <label class="ui-checkbox">
                            <input type="checkbox" id="chElt" onclick="checkOrCancelAll();" value="">
                            <span class="input-span"></span>
                            </label>
                            </th>
                             <th class="align">序</>
                             <th class="align"  >產品類別</th>
                             <th class="align"  >產品名稱</th>
                             <th class="align"  >產品價格</th>
                             <th class="align"  >庫存數量</th>
                             <th class="align" height="50" width="15%">產品描述</th>
                             <th class="align"  >販售時段早</th>
                             <th class="align"  >販售時段中</th>
                             <th class="align"  >販售時段晚</th>
                             <th class="align"  >販售狀態</th>
                             <th class="align" width="25%">產品圖片</th>
                              <th width="15%" class="align">操作</th>
                             </tr>
                             </thead>
                             <tbody id="yuActivitytbody">`
                        var count = 0;
                        for (let i = 0; i <= data.length - 1; i++) {
                            count++
                            if (data[i].cIsBreakFast == 1) {
                                Break = `<button class="btn btn-green" onclick="p產品販售早(${data[i].cProductId})" >販售</button>`
                            }
                            else {
                                Break = `<button  class="btn btn-gray" onclick="p產品販售早(${data[i].cProductId})" >不販售</button>`
                            }
                            if (data[i].cIsLunch == 1) {
                                Lunch = `<button  class=" btn btn-green"  onclick="p產品販售午(${data[i].cProductId})">販售 </button>`
                            }
                            else {
                                Lunch = `<button  class="btn btn-gray" onclick="p產品販售午(${data[i].cProductId})">不販售 </button>`
                            }
                            if (data[i].cIsDinner == 1) {
                                Dinner = `<button  class=" btn btn-green" onclick="p產品販售晚(${data[i].cProductId})">販售 </button>`
                            }
                            else {
                                Dinner = `<button  class="btn btn-gray"  onclick="p產品販售晚(${data[i].cProductId})">不販售 </button>`
                            }
                            if (data[i].cQuantity == 0) {
                                cQuantity = `<p class="btn btn-danger" style="vertical-align: middle" >請補充庫存</p>`
                            }
                            else {
                                cQuantity = data[i].cQuantity;
                            }
                            if (data[i].cIsOnSaleId == 3) {
                                cStatusName = `<button class="btn btn-gray" style="vertical-align: middle" onclick="p產品販售狀態(${data[i].cProductId}, ${data[i].cIsOnSaleId})">下架 </button>`
                            }
                            else if (data[i].cIsOnSaleId == 2) {
                                cStatusName = `<button class="btn btn-danger" style="vertical-align: middle" onclick="p產品販售狀態(${data[i].cProductId}, ${data[i].cIsOnSaleId})">售完 </button>`
                            }
                            else if (data[i].cIsOnSaleId == 1) {
                                cStatusName = `<button class=" btn btn-green" style="vertical-align: middle" onclick="p產品販售狀態(${data[i].cProductId}, ${data[i].cIsOnSaleId})">正常 </button>`
                            }


                            txttable += `<tr style="text-align:center"><td style="vertical-align: middle;"><label class="ui-checkbox"><input id="checkdelete" value="${data[i].cProductId}" type="checkbox" name="interest"><span class="input-span">
                                <td style="vertical-align: middle">${count}
                                <td style="vertical-align: middle">${data[i].cCategoryName}
                                <td style="vertical-align: middle">${data[i].cProductName}
                                <td style="vertical-align: middle">${data[i].cPrice}
                                <td style="vertical-align: middle">${cQuantity}
                                <td style="vertical-align: middle">${data[i].cDescription}
                                  <td style="vertical-align: middle" id="p販售早${data[i].cProductId}">${Break}
                                <td style="vertical-align: middle" id="p販售午${data[i].cProductId}">${Lunch}
                                <td style="vertical-align: middle" id="p販售晚${data[i].cProductId}">${Dinner}
                                <td style="vertical-align: middle" id="p產品狀態${data[i].cProductId}">${cStatusName}
                                <td style="vertical-align: middle"><img src="${data[i].cPicture.replace("~", "")}" alt="your image" width="180" height="150" />

                                <td style="vertical-align: middle">
                                <a  class="btn btn-Success btn-xs m-r-5" href="/cProduct/Edit?id=${data[i].cProductId}"style="background-color:##00ff21"><i class="fa fa-pencil font-18">編輯
                                <a  class="btn btn-Danger btn-xs" href="/cProduct/Delete?id=${data[i].cProductId}" style="background-color:#ff0000"><i class="fa fa-trash font-18">刪除`
                        }
                        txttable += ` </tbody>
                                </table>
                                </div>`

                        $("#yuActivityt").html(txttable);
                        $('#myTable').DataTable({
                            language: {
                                "emptyTable": "無資料...",
                                "processing": "處理中...",
                                "loadingRecords": "載入中...",
                                "lengthMenu": "每頁 _MENU_ 資料",
                                "zeroRecords": "無搜尋結果",
                                "info": "_START_ 至 _END_ / 共 _TOTAL_ 筆",
                                "infoEmpty": "尚無資料",
                                "infoFiltered": "(從 _MAX_ 筆資料過濾)",
                                "infoPostFix": "",
                                "search": "搜尋字串:",
                                "paginate": {
                                    "first": "首頁",
                                    "last": "末頁",
                                    "next": "下頁",
                                    "previous": "前頁"
                                },
                                "aria": {
                                    "sortAscending": ": 升冪",
                                    "sortDescending": ": 降冪"
                                }
                            }
                        });

                        $("#myTable_length").append(`<label id="btndelete" style = "display:none"><button type="button" class="btn button_btn btn-danger" onclick="deletemore();">刪除選中</button></label>`);




                        $("#myTable input[name = interest]").change(function Submit() {
                            var SelectFalse = false; //用于判断是否被选择条件
                            var CheckBox = $('input[name = interest]');//得到所的复选框
                            var ccbb = "#myTable input[type=checkbox]:checked";
                            for (var i = 0; i < CheckBox.length; i++) {
                                if (CheckBox[i].checked) {
                                    SelectFalse = true;
                                }
                            }

                            if (SelectFalse) {
                                $("#btndelete").show();
                            } else {
                                $("#btndelete").hide();
                            }
                        }
                        )

                    }
                });
            });
        }
        $(`#p_產品類別全部`).on("click", function () {

            name = $(`#p_產品類別全部`).attr("value");

            $.ajax({
                url: "/cProduct/getCategoryJason",
                type: "Get",
                data: { "catName": $(`#p_產品類別全部`).attr("value") },
                success: function (data) {
                    console.log(data);
                    var txttable = "";

                    txttable +=
                        `<div id="orderlist">
                            <table class="table table-hover table_striped table-bordered border" id="myTable">
                            <thead>
                            <tr>
                            <th width="50px" style="text-align:center">
                            <label class="ui-checkbox">
                            <input type="checkbox" id="chElt" onclick="checkOrCancelAll();" value="">
                            <span class="input-span"></span>
                            </label>
                            </th>
                             <th class="align">序</>
                             <th class="align"  >產品類別</th>
                             <th class="align"  >產品名稱</th>
                             <th class="align"  >產品價格</th>
                             <th class="align"  >庫存數量</th>
                             <th class="align" height="50" width="15%">產品描述</th>
                             <th class="align"  >販售時段早</th>
                             <th class="align"  >販售時段中</th>
                             <th class="align"  >販售時段晚</th>
                             <th class="align"  >販售狀態</th>
                             <th class="align" width="25%">產品圖片</th>
                              <th width="15%" class="align">操作</th>
                             </tr>
                             </thead>
                             <tbody id="yuActivitytbody">`
                    var count = 0;
                    for (let i = 0; i <= data.length - 1; i++) {
                        count++

                        if (data[i].cIsBreakFast == 1) {
                            Break = `<button class="btn btn-green" onclick="p產品販售早(${data[i].cProductId})" >販售</button>`
                        }
                        else {
                            Break = `<button  class="btn btn-gray" onclick="p產品販售早(${data[i].cProductId})" >不販售</button>`
                        }
                        if (data[i].cIsLunch == 1) {
                            Lunch = `<button  class=" btn btn-green"  onclick="p產品販售午(${data[i].cProductId})">販售 </button>`
                        }
                        else {
                            Lunch = `<button  class="btn btn-gray" onclick="p產品販售午(${data[i].cProductId})">不販售 </button>`
                        }
                        if (data[i].cIsDinner == 1) {
                            Dinner = `<button  class=" btn btn-green" onclick="p產品販售晚(${data[i].cProductId})">販售 </button>`
                        }
                        else {
                            Dinner = `<button  class="btn btn-gray"  onclick="p產品販售晚(${data[i].cProductId})">不販售 </button>`
                        }
                        if (data[i].cQuantity == 0) {
                            cQuantity = `<p class="btn btn-danger" style="vertical-align: middle" >請補充庫存</p>`
                        }
                        else {
                            cQuantity = data[i].cQuantity;
                        }
                        if (data[i].cIsOnSaleId == 3) {
                            cStatusName = `<button  class="btn btn-gray" style="vertical-align: middle" onclick="p產品販售狀態(${data[i].cProductId}, ${data[i].cIsOnSaleId})">下架 </button>`
                        }
                        else if (data[i].cIsOnSaleId == 2) {
                            cStatusName = `<button  class="btn btn-danger" style="vertical-align: middle" onclick="p產品販售狀態(${data[i].cProductId}, ${data[i].cIsOnSaleId})">售完 </button>`
                        }
                        else if (data[i].cIsOnSaleId == 1) {
                            cStatusName = `<button  class=" btn btn-green" style="vertical-align: middle" onclick="p產品販售狀態(${data[i].cProductId}, ${data[i].cIsOnSaleId})">正常 </button>`
                        }



                        txttable += `<tr style="text-align:center"><td style="vertical-align: middle;"><label class="ui-checkbox"><input id="checkdelete" value="${data[i].cProductId}" type="checkbox" name="interest"><span class="input-span">
                                <td style="vertical-align: middle">${count}
                                <td style="vertical-align: middle">${data[i].cCategoryName}
                                <td style="vertical-align: middle">${data[i].cProductName}
                                <td style="vertical-align: middle">${data[i].cPrice}
                                <td style="vertical-align: middle">${cQuantity}
                                <td style="vertical-align: middle">${data[i].cDescription}
                                <td style="vertical-align: middle" id="p販售早${data[i].cProductId}">${Break}
                                <td style="vertical-align: middle" id="p販售午${data[i].cProductId}">${Lunch}
                                <td style="vertical-align: middle" id="p販售晚${data[i].cProductId}">${Dinner}
                                <td style="vertical-align: middle" id="p產品狀態${data[i].cProductId}">${cStatusName}
                                <td style="vertical-align: middle;"><img src="${data[i].cPicture.replace("~", "")}" alt="your image" width="180" height="150" />

                                <td style="vertical-align: middle;">
                                <a  class="btn btn-Success btn-xs m-r-5" href="/cProduct/Edit?id=${data[i].cProductId}"style="background-color:##00ff21"><i class="fa fa-pencil font-18">編輯
                                <a  class="btn btn-Danger btn-xs" href="/cProduct/Delete?id=${data[i].cProductId}" style="background-color:#ff0000"><i class="fa fa-trash font-18">刪除`
                    }

                    txttable += ` </tbody>
                                </table>
                                </div>`

                    $("#yuActivityt").html(txttable);
                    $('#myTable').DataTable({
                        language: {
                            "emptyTable": "無資料...",
                            "processing": "處理中...",
                            "loadingRecords": "載入中...",
                            "lengthMenu": "每頁 _MENU_ 資料",
                            "zeroRecords": "無搜尋結果",
                            "info": "_START_ 至 _END_ / 共 _TOTAL_ 筆",
                            "infoEmpty": "尚無資料",
                            "infoFiltered": "(從 _MAX_ 筆資料過濾)",
                            "infoPostFix": "",
                            "search": "搜尋字串:",
                            "paginate": {
                                "first": "首頁",
                                "last": "末頁",
                                "next": "下頁",
                                "previous": "前頁"
                            },
                            "aria": {
                                "sortAscending": ": 升冪",
                                "sortDescending": ": 降冪"
                            }
                        }
                    });

                    $("#myTable_length").append(`<label id="btndelete" style = "display:none"><button type="button" class="btn button_btn btn-danger" onclick="deletemore();">刪除選中</button></label>`);




                    $("#myTable input[name = interest]").change(function Submit() {
                        var SelectFalse = false; //用于判断是否被选择条件
                        var CheckBox = $('input[name = interest]');//得到所的复选框
                        var ccbb = "#myTable input[type=checkbox]:checked";
                        for (var i = 0; i < CheckBox.length; i++) {
                            if (CheckBox[i].checked) {
                                SelectFalse = true;
                            }
                        }

                        if (SelectFalse) {
                            $("#btndelete").show();
                        } else {
                            $("#btndelete").hide();
                        }
                    }
                    )

                }
            });
        });
    }
});








function checkOrCancelAll() {
    //1.獲取checkbo的元素對象
    var chElt = document.getElementById("chElt");
    //2.獲取選中狀態
    var checkedElt = chElt.checked;
    console.log(checkedElt)
    //3.若checked=true,將所有的復選框選中,checked=false,將所有的復選框取消
    var allCheck = document.getElementsByName("interest");
    //4.循環遍歷取出每一個復選框中的元素
    //var mySpan=document.getElementById("mySpan");
    if (checkedElt) {
        //全選
        for (var i = 0; i < allCheck.length; i++) {
            //設置復選框的選中狀態
            allCheck[i].checked = true;
        }
        $("#btndelete").show();
        //mySpan.innerHTML="取消全選";
    } else {
        //取消全選
        for (var i = 0; i < allCheck.length; i++) {
            allCheck[i].checked = false;
        }
        $("#btndelete").hide();
        //mySpan.innerHTML="全選";
    }
};

//全選刪除
function deletemore() {
    var checked = $("#myTable input[type=checkbox]:checked");
    for (let i = 0; i < checked.length; i++) {
        console.log(checked[i].value);
        $.ajax({
            url: "/cProduct/Delete?id=" + checked[i].value,
            type: "GET",
            success: function () {   //請求成功後執行的操作
                window.location.reload();
            }
        });
    }
}



//產品販售狀態
function p產品販售狀態(PrId, PrTimeId) {
    $.ajax({
        url: "/cProduct/changeSaleonoff",
        type: "Get",
        data: { "PrId": PrId, "PrTimeId": PrTimeId },
        success: function (data) {
            console.log(data);
            if (data == 1) {
                cStatusName = `<button class=" btn btn-green" style="vertical-align: middle" onclick="p產品販售狀態(${PrId},${PrTimeId + 1})">正常</button>`
            }
            else if (data == 2) {

                cStatusName = `<button  class="btn btn-danger" style="vertical-align: middle" onclick="p產品販售狀態(${PrId},${PrTimeId + 1})">售完</button>`

            }
            else if (data == 3) {
                cStatusName = `<button  class="btn btn-gray" style="vertical-align: middle" onclick="p產品販售狀態(${PrId},${PrTimeId = 0})">下架</button>`

            }
            $(`#p產品狀態${PrId}`).html(`<td style="vertical-align: middle" id="p產品狀態${PrId}">${cStatusName}`);
        }
    });


}

//產品販售狀態
function p產品販售狀態(PrId, PrTimeId) {
    $.ajax({
        url: "/cProduct/changeSaleonoff",
        type: "Get",
        data: { "PrId": PrId, "PrTimeId": PrTimeId },
        success: function (data) {
            console.log(data);
            if (data == 1) {
                cStatusName = `<button class=" btn btn-green" style="vertical-align: middle" onclick="p產品販售狀態(${PrId},${PrTimeId + 1})">正常</button>`
            }
            else if (data == 2) {

                cStatusName = `<button  class="btn btn-danger" style="vertical-align: middle" onclick="p產品販售狀態(${PrId},${PrTimeId + 1})">售完</button>`

            }
            else if (data == 3) {
                cStatusName = `<button  class="btn btn-gray" style="vertical-align: middle" onclick="p產品販售狀態(${PrId},${PrTimeId = 0})">下架</button>`

            }
            $(`#p產品狀態${PrId}`).html(`<td style="vertical-align: middle" id="p產品狀態${PrId}">${cStatusName}`);
        }
    });


}

//產品販售時段早
function p產品販售早(id) {
    $.ajax({
        url: "/cProduct/changeProductBreakFast?id=" + id,
        type: "Get",
        success: function (data) {
            console.log(data);
            if (data == true) {

                $("#p販售早" + id).html(`<button class=" btn btn-green" style="vertical-align: middle" onclick="p產品販售早(${id})" style="vertical-align: middle">販售</button>`);
            }
            else {
                $("#p販售早" + id).html(`<button class="btn btn-gray" style="vertical-align: middle" onclick="p產品販售早(${id})" style="vertical-align: middle")">不販售</button>`);
            }

        }
    });

}
//產品販售時段午
function p產品販售午(id) {
    $.ajax({
        url: "/cProduct/changeProductLunch?id=" + id,
        type: "Get",
        success: function (data) {
            if (data == true) {
                $("#p販售午" + id).html(`<button class=" btn btn-green" style="vertical-align: middle" onclick="p產品販售午(${id})" style="vertical-align: middle">販售</button>`);
            }
            else {
                $("#p販售午" + id).html(`<button class="btn btn-gray" style="vertical-align: middle" onclick="p產品販售午(${id})" style="vertical-align: middle")">不販售</button>`);
            }

        }
    });

}
//產品販售時段晚
function p產品販售晚(id) {
    $.ajax({
        url: "/cProduct/changeProductDinner?id=" + id,
        type: "Get",
        success: function (data) {
            if (data == true) {
                $("#p販售晚" + id).html(`<button class=" btn btn-green" style="vertical-align: middle" onclick="p產品販售晚(${id})" style="vertical-align: middle">販售</button>`);
            }
            else {
                $("#p販售晚" + id).html(`<button class="btn btn-gray" style="vertical-align: middle" onclick="p產品販售晚(${id})" style="vertical-align: middle")">不販售</button>`);
            }

        }
    });

}





//進入此頁面執行
$(Activity());