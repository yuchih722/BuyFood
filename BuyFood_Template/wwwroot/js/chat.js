function ChatRoomStart(MemberNameForChatStart) {


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
var 自己有沒有連續留言 = 0;
var 對方有沒有連續留言 = 0;




connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var when = new Date().toLocaleTimeString();
    var encodedMsg = /*user + " says " +*/ '<p>'+ msg+'<span class="chat_message_time">' + when +'</span></p>';
    var li = document.createElement("li");
    li.innerHTML = encodedMsg;

    $.ajax({
        url: "/adChatRoom/getMessage",
        type: "POST",
        data: { MemberName: user, Content: message, MessageTime: when },

    });



    var Picture ="";
    if (user == '管理員') {
        Picture = 'src = "img/adrChatRoom/adm2.jpg" class="md-user-image" >';
        
    }
    else {
        Picture = 'src = "img/adrChatRoom/Customer_Icon.jpg" class="md-user-image">';
    }
    if (msg == "") { return; }
    if (user == MemberNameForChatStart)
    {
    /*自己都放右邊  */
        if (自己有沒有連續留言 == 0)
        {

            $("#chatStartPoint").append('<div class="chat_message_wrapper chat_message_right">' + '<div class= "chat_user_avatar">' + '<a target="_blank">' + '<img alt="' + user + '" title="' + user + '"' + Picture + '</a>' + '</div>' + '<ul class="chat_message messagesListForClient" >');
            /*            document.getElementById("messagesListForClient").appendChild(li);*/
            $('ul.messagesListForClient:eq(-1)').append(li);

            自己有沒有連續留言 = 1;
        }
        else
        {
            $('ul.messagesListForClient:eq(-1)').append(li);
/*            document.getElementById("messagesListForClient").appendChild(li);*/
        }
        對方有沒有連續留言 = 0;
        document.getElementById('messageInput').value = '';
    }

    else {
        if (對方有沒有連續留言 == 0) {
            $("#chatStartPoint").append('<div class="chat_message_wrapper">' + '<div class="chat_user_avatar">' + '<a target="_blank">' + '<img alt="' + user + '" title="' + user + '"' + Picture + '</a>' + '</div>' + '<ul class="chat_message messagesList">');
        /*document.getElementById("messagesList").appendChild(li);*/
            $('ul.messagesList:eq(-1)').append(li);

            對方有沒有連續留言 = 1;
        }
        else
        {
            $('ul.messagesList:eq(-1)').append(li);
/*            document.getElementById("messagesList").appendChild(li);*/
        }
        自己有沒有連續留言 = 0;
        document.getElementById('messageInput').value = '';
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = MemberNameForChatStart;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

}