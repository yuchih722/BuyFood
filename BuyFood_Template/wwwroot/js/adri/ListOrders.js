var count = 1;
var text = "";
$.ajax({
    url: "/Order/ListOrder",
    type: "POST",
    data: { MemberName: document.getElementById("getLoginName").childNodes[1].nodeValue },
    success: function (data) {
        console.log(data);
        $.ajax({
            url: "/Order/OrderStatusDate",
            type: "POST",
            data: { MemberName: document.getElementById("getLoginName").childNodes[1].nodeValue },
            success: function (response) {
                console.log(response);
                for (let i = 0; i < response.length; i++) {
                    console.log(data[i].cOrderID + data[i].cTotalPrice + response[i].cOrderStatus + response[i].cOrderDate)
                    text += ` <tr>
                                                   <th scope="row">${count}</th>
                                                   <td>${data[i].cOrderID}</td>
                                                    <td>$${data[i].cTotalPrice}</td>
                                                    <td>${response[i].cOrderStatus}</td>
                                                    <td><a href="/OrderDetail/ShowOrderDetail/${data[i].cOrderID}"><button type="button" class="btn btn-outline-success">訂單詳細</button></a></td>
                                                    <td>${response[i].cOrderDate}</td>
                                                    </tr>`;
                    count++;
                }
                $("#OrderForList").html(text);

            }
        });

    }

});

$("#pills-profile-tab").on("click", function () {
    var textOnGoing = "";
    var countOnGoing = 1;
    $.ajax({
        url: "/Order/ListOrderOnGoing",
        type: "POST",
        data: { MemberName: document.getElementById("getLoginName").childNodes[1].nodeValue },
        success: function (data) {
            $.ajax({
                url: "/Order/OrderStatusDateOnGoing",
                type: "POST",
                data: { MemberName: document.getElementById("getLoginName").childNodes[1].nodeValue },
                success: function (response) {
                    for (let i = 0; i < response.length; i++) {
                        textOnGoing += ` <tr>
                                                   <th scope="row">${countOnGoing}</th>
                                                   <td>${data[i].cOrderID}</td>
                                                    <td>$${data[i].cTotalPrice}</td>
                                                    <td>${response[i].cOrderStatus}</td>
                                                    <td><a href="/OrderDetail/ShowOrderDetail/${data[i].cOrderID}"><button type="button" class="btn btn-outline-success">訂單詳細</button></a></td>
                                                    <td>${response[i].cOrderDate}</td>
                                                    </tr>`;
                        countOnGoing++;
                    }
                    $("#OrderForList").html(textOnGoing);
                }
            });
        }
    });
});

//按鈕觸發全部訂單

$("#pills-home-tab").on("click", function () {

    var countAll = 1;
    var textAll = "";
    $.ajax({
        url: "/Order/ListOrder",
        type: "POST",
        data: { MemberName: document.getElementById("getLoginName").childNodes[1].nodeValue },
        success: function (data) {
            $.ajax({
                url: "/Order/OrderStatusDate",
                type: "POST",
                data: { MemberName: document.getElementById("getLoginName").childNodes[1].nodeValue },
                success: function (response) {
                    console.log(response);
                    for (let i = 0; i < response.length; i++) {
                        console.log(data[i].cOrderID + data[i].cTotalPrice + response[i].cOrderStatus + response[i].cOrderDate)
                        textAll += ` <tr>
                                                   <th scope="row">${countAll}</th>
                                                   <td>${data[i].cOrderID}</td>
                                                    <td>$${data[i].cTotalPrice}</td>
                                                    <td>${response[i].cOrderStatus}</td>
                                                    <td><a href="/OrderDetail/ShowOrderDetail/${data[i].cOrderID}"><button type="button" class="btn btn-outline-success">訂單詳細</button></a></td>
                                                    <td>${response[i].cOrderDate}</td>
                                                    </tr>`;
                        countAll++;
                    }
                    $("#OrderForList").html(textAll);
                }
            });
        }
    });
});

//按鈕觸發已完成訂單

$("#pills-contact-tab").on("click", function () {
    var textFinished = "";
    var countFinished= 1;
    $.ajax({
        url: "/Order/ListOrderFinished",
        type: "POST",
        data: { MemberName: document.getElementById("getLoginName").childNodes[1].nodeValue },
        success: function (data) {
            $.ajax({
                url: "/Order/OrderStatusDateFinished",
                type: "POST",
                data: { MemberName: document.getElementById("getLoginName").childNodes[1].nodeValue },
                success: function (response) {
                    for (let i = 0; i < response.length; i++) {
                        textFinished += ` <tr>
                                                   <th scope="row">${countFinished}</th>
                                                   <td>${data[i].cOrderID}</td>
                                                    <td>$${data[i].cTotalPrice}</td>
                                                    <td>${response[i].cOrderStatus}</td>
                                                    <td><a href="/OrderDetail/ShowOrderDetail/${data[i].cOrderID}"><button type="button" class="btn btn-outline-success">訂單詳細</button></a></td>
                                                    <td>${response[i].cOrderDate}</td>
                                                    </tr>`;
                        countFinished++;
                    }
                    $("#OrderForList").html(textFinished);
                }
            });
        }
    });
});
