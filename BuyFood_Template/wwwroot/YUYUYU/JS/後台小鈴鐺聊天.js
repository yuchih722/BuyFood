function needtoReply() {

$.ajax({
    url:"/AdmHome/ger_chat_new",
    type: "get",
    success: function (data) {
        //console.log(data);
        $("[name='num_New_notice']").html(data.length);
        var chat_yu = "";
        for (let i = 0; i < data.length; i++) {
            let h_time = data[i].time.value.hours;
            let m_time = data[i].time.value.minutes;
            let show_time=""
            if (h_time != 0) {
                show_time += `${h_time} 小時 ${m_time} 分鐘前`
            } else {
                show_time += `${m_time} 分鐘前` 
            }

            chat_yu+=`<li class="list-group list-group-divider scroller" data-height="240px" data-color="#71808f">
                                <div>
                                    <a class="list-group-item" onclick="btn_to_chatroom(${data[i].id})">
                                        <div class="media">
                                            <div class="media-img">
                                                <img src="${data[i].img}" />
                                            </div>
                                            <div class="media-body">
                                                <div class="font-strong">${data[i].name} </div><small class="text-muted float-right">${show_time}</small>
                                                <div class="font-13">${data[i].content}</div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                </li>`
        }
        $("#chat_review").html(chat_yu)
    }
})
}
let time_chat_yu = window.setInterval("needtoReply()", 2000)
let time_chat_adri = window.setInterval("OrderMessageReplay()", 2000)
$("#goto_click_num_chat_yu").click(function() {
    window.clearInterval(time_chat_yu);
    $("#yu_forzerothemessage").html("0"),
        
    $.ajax({
        url: "/AdmHome/do_all_review_0",
        type: "get",
    })
})

function btn_to_chatroom(id) {
    $.ajax({
        url: "/AdmHome/click_num_chat?id=" + id,
        type: "get",
    })}

$("#goto_click_num_chat_adri").click(function () {
    window.clearInterval(time_chat_adri);
    $("#adri_forzerothemessage").html("0"),

        $.ajax({
            url: "/Order/clearAllnewOrderMessage/",
            type:"get",

        })
})

function OrderMessageReplay() {
    $.ajax({
        url: "/Order/newOrdersSignalR/",
        type: "POST",
        success: function (dataOrder) {

            $("[name='num_New_Order']").html(dataOrder.length);
            var txt_adri_newOderMes = "";
            for (let i = 0; i<dataOrder.length; i++) {

                let h_time_adri = dataOrder[i].time.value.hours;
                let m_time_adri = dataOrder[i].time.value.minutes;
                let show_time_adri = '';
                if (h_time_adri != 0) {
                    show_time_adri += `${h_time_adri} 小時 ${m_time_adri} 分鐘前`
                } else {
                    show_time_adri += `${m_time_adri} 分鐘前`
                }
                txt_adri_newOderMes +=`<li class="list-group list-group-divider scroller" data-height="240px" data-color="#71808f">
                <div>
                    <a class="list-group-item">
                        <div class="media">
                            <div class="media-img">
                                <span class="badge badge-default badge-big"><i class="fa fa-shopping-basket"></i></span>
                            </div>
                            <div class="media-body">
                                <div class="font-13">${dataOrder[i].cUserName} ${dataOrder[i].cMessageOrder}</div><small class="text-muted">${show_time_adri}</small>
                            </div>
                        </div>
                    </a>
                </div>
            </li>`;

            }
            $("#oder_message").html(txt_adri_newOderMes);
        }

    })

}