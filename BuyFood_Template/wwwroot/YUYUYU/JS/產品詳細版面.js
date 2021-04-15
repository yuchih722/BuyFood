$(function () {
    var id = $("#getid").val();
    $.ajax({
        url: "/ProductDetail/startJson?id=" + id,
        type: "get",
        success: function (data) {
            
            var starforright = "";
            for (let i = 0; i < data.bavg; i++) {
                starforright += `<span class="fa fa-star checked" style="color: orange;"></span>`
            }
            for (let i = 0; i < 5 - data.bavg; i++) {
                starforright += ` <span class="fa fa-star checked" style="color:#d5d3cf"></span>`
            }
            starforright += `<span>( ${data.bcount} 筆留言)</span>`

            $("#productstert").html(starforright);

            var starforbown = `<div><span style="color: orange;font-size:30px;">${data.bavg} </span><span style="color:orange;font-size:30px;"> / 5</span></div>
                                            <div>`
            for (var i = 0; i < data.bavg; i++) {
                starforbown += `<span class="fa fa-star checked" style="color: orange;font-size: 30px;"></span>`
            }
            for (var i = 0; i < 5 - data.bavg; i++) {
                starforbown += `<span class="fa fa-star checked" style="color: #d5d3cf;font-size: 30px;"></span>`
            }
            starforbown += `</div >`

            $("#Bigstert").html(starforbown);
        }
    }),
        
        $.ajax({
            url: "/ProductDetail/productBoards?id=" + id,
            type: "get",
            success: function (data) {
                //console.log(data);
                var Boardlist = "";
                if (data.length == 0) {
                    Boardlist += ` <img style="display:block; margin:auto;" width="400" height="400" src="/imgs/維修圖.jpg"  />`
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
                    <br />
                    <p style="font-size:15px">${data[i].memberreview}</p>
                </div>
            </div>`
                }
                $("#proBoards").html(Boardlist);
            }

        }),


        $.ajax({
            url: "/ProductDetail/productstar?id=" + id,
            type: "get",
            success: function (data) {
                //console.log(data);
                var btnstar = `<div style = "float:right;width: 70%; height: 100%;" >
        <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
            <li class="nav-item" role="presentation">
                <button onclick="gorunBoards(99)" style="margin:10px;" class="nav-link active" id="pills-home-tab" data-bs-toggle="pill" data-bs-target="#pills-home" type="button" role="tab" aria-controls="pills-home" aria-selected="true">全部( ${data.all} )</button>
            </li>
            <li class="nav-item" role="presentation">
                <button onclick="gorunBoards(5)" style="margin:10px;" class="nav-link" id="pills-profile-tab" data-bs-toggle="pill" data-bs-target="#pills-profile" type="button" role="tab" aria-controls="pills-profile" aria-selected="false">5 星 ( ${data.five} )</button>
            </li>
            <li class="nav-item" role="presentation">
                <button onclick="gorunBoards(4)" style="margin:10px;" class="nav-link" id="pills-contact-tab" data-bs-toggle="pill" data-bs-target="#pills-contact" type="button" role="tab" aria-controls="pills-contact" aria-selected="false">4 星 ( ${data.four} )</button>
            </li>
            <li class="nav-item" role="presentation">
                <button onclick="gorunBoards(3)" style="margin:10px;" class="nav-link" id="pills-contact-tab" data-bs-toggle="pill" data-bs-target="#pills-contact" type="button" role="tab" aria-controls="pills-contact" aria-selected="false">3 星 ( ${data.three} )</button>
            </li>
            <li class="nav-item" role="presentation">
                <button onclick="gorunBoards(2)" style="margin:10px;" class="nav-link" id="pills-contact-tab" data-bs-toggle="pill" data-bs-target="#pills-contact" type="button" role="tab" aria-controls="pills-contact" aria-selected="false">2 星 ( ${data.two} )</button>
            </li>
            <li class="nav-item" role="presentation">
                <button onclick="gorunBoards(1)" style="margin:10px;" class="nav-link" id="pills-contact-tab" data-bs-toggle="pill" data-bs-target="#pills-contact" type="button" role="tab" aria-controls="pills-contact" aria-selected="false">1 星 ( ${data.one} )</button>
            </li>
            <li class="nav-item" role="presentation">
                <button onclick="gorunBoards(0)" style="margin:10px;" class="nav-link" id="pills-contact-tab" data-bs-toggle="pill" data-bs-target="#pills-contact" type="button" role="tab" aria-controls="pills-contact" aria-selected="false"> 附上評論  ( ${data.hasreview} )</button>
            </li>
        </ul>
           </div >`

                $("#btnstar").html(btnstar);
            }
        })

    $.ajax({
        url: "/ProductDetail/smelltit?id=" + id,
        type: "get",
        success: function (data) {
            var NowDate = new Date();  /*現在時間*/
            var h = NowDate.getHours();
            
            var productQuantity = data.table.cQuantity <= 0 ? `<span style="color:#ff0000">已售完</span>` : `<span>${data.table.cQuantity}</span>`
            var productdetail=""
            if (data.table.cQuantity <= 0) {
                productdetail += `<input type = "button" class="btn_enable_style"  value = "此商品已完售" disabled>`
                $("#btu_check_stock").hide();
            } else if (data.table.cIsOnSaleId == 3) {
                productdetail += `<input type = "button" class="btn_enable_style"  value = "此商品暫停販售" disabled>`
                $("#btu_check_stock").hide();
            } else
                productdetail += `<input type = "button" class="primary-btn" onclick = "addCart(${data.table.cProductId})"  value = "加入購物車" >`
            
             productdetail += ` <ul>
                        <li><b>庫存量</b> ${productQuantity}</li>
                        <li><b>類型</b> <span>${data.tablea} </span></li>
                        <li><b>製作時間</b> <span>${data.table.cFinishedTime} 分鐘</span></li>
                        <li>
                            <b>類型販售時段</b>
                            <div class="share" id="tagfood">
                            </div>
                        </li>
                    </ul >`
            $("#smelltitle").after(productdetail);

            if (data.table.cIsBreakFast == 1) {
                $("#tagfood").append(` <a href="javascript:void(0)" class="btn btn - outline - primary">早</a> `)
            }
            if (data.table.cIsLunch == 1) {
                $("#tagfood").append(` <a href="javascript:void(0)" class="btn btn - outline - primary">中</a> `)
            }
            if (data.table.cIsDinner == 1) {
                $("#tagfood").append(` <a href="javascript:void(0)" class="btn btn - outline - primary">晚</a> `)
            }

        }
    })


    $.ajax({
        url: "/ProductDetail/gethotproduct?id="+id,
        type: "get",
        success: function (data) {
            console.log(data);
            var hot = "";
            pdtItem = data;
            for (let i = 0; i < data.length; i++) {

                hot += `<div class="col-lg-3 col-md-4 col-sm-6">
                                            <div class="product__item">
                                                <div class="product__item__pic set-bg" style="background-image: url(${data[i].product.cPicture.replace("~", "")})">
                                                 <ul class="product__item__pic__hover" style="text-align: right;">
                                                        
                                                        <li><a href="javascript: void(0)"  onclick="addCartlist(pdtItem[${i}].product)"><i class="fa fa-shopping-cart"></i></a></li>
                                                    </ul>
                                                    <ul class="start_for_homepage">`
                let Average_message_yu = data[i].coun <= 0 ? 0 : parseInt(data[i].sum / data[i].coun)
                    for (let x = 0; x < Average_message_yu; x++) {
                        hot +=`<li> <span class="fa fa-star checked" style="color: orange;font-size:25px"></span></li > `
                    }
                    for (let g = 0; g < 5 - Average_message_yu; g++) {
                        hot += `<li> <span class="fa fa-star checked" style="color: #d5d3cf;font-size:25px"></span></li > `
                    }                              
                hot += ` </ul >
                    <span class="product_time_yu">製作時間：${data[i].product.cFinishedTime}分鐘</span> `

                hot +=   ` </div >
                                                <div class="product__item__text">
                                                    <h6><a href="/ProductDetail/ProductData?id=${data[i].product.cProductId}">${data[i].product.cProductName}</a></h6>
                                                    <h5>$ ${data[i].product.cPrice}</h5>
                                                    <h6>庫存量 ${ data[i].product.cQuantity}</h6>
                                                </div>
                                            </div>
                                        </div>`
            }
            $("#hot_product").html(hot);
        }
    })
    //$.ajax
//<li><a href="#"><i class="fa fa-heart"></i></a></li>
//<li><a href="#"><i class="fa fa-retweet"></i></a></li>
})