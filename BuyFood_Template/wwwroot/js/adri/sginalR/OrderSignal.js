var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//開啟連線
connection.start().then(function () {
    console.log('layout開啟連線');
}).catch(function (err) {
    return console.error(err.toString());
});
    //開啟連線

//加入群組
function admAddToGroup(ChannelID) {
    console.log(ChannelID + '已加入群組');
    var groupNameOrderadm = ChannelID.toString();
    let userNameOrderadm = '管理員';
    connection.invoke("AddGroup", groupNameOrderadm, userNameOrderadm).catch(function (err) {
        return console.error(err.toString());
    })
}
    //加入群組


$(function () {
    console.log('後台成功');




})