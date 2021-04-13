function gorunBoards(star) {
    var id = $("#getid").val();
    $.ajax({
        url: "/ProductDetail/productBoards",
        type: "get",
        data: { "id": id, "star": star },
        success: function (data) {
            console.log(data);
            var Boardlist = "";
            if (data.length == 0) {
                Boardlist += ` <img style="display:block; margin:auto;" width="400" height="400" src="/imgs/維修圖.jpg" />`
            }
            for (var i = 0; i < data.length; i++) {
                Boardlist += `<div style="width:100%;height:150px;float:none;left:30%;border-bottom:1px solid #808080;margin:10px;">
                <div style="width:10%;height:100%;float:left;text-align:right">`

                Boardlist += `<img style="border-radius:50%;"  width="40" height="40" src="${data[i].membersphoto}"/>
                </div>
                <div style="width: 90%; float: left;margin-bottom: 5px;left:10px">`
                var name = data[i].membername
                var rename = name.substr(0, 1) + "***" + name.substr(name.length - 1, 1)
                Boardlist += `<div style="font-size:12px">${rename}</div>`
                for (let j = 0; j < data[i].memberstar; j++) {
                    Boardlist += `<span class="fa fa-star checked" style="color: orange;"></span>`
                }
                for (let j = 0; j < 5 - data[i].memberstar; j++) {
                    Boardlist += `<span class="fa fa-star checked" style="color:#d5d3cf"></span>`
                }
                Boardlist += `<div style="font-size:12px;color:#808080">${data[i].memberproduct}</div>
                    <br />`
                if (data[i].memberreview != null) {
                    Boardlist += `<p style="font-size:15px">${data[i].memberreview}</p>`
                    }
                Boardlist += `</div>
            </div>`
            }
            $("#proBoards").html(Boardlist);
        }

    })
}