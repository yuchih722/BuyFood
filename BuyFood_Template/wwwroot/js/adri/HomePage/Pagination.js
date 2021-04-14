////let member_No = $("#user_member").val();
////let users_cart_No = "cart" + member_No;        //依登入的會員改變localstorage的Key值
var cart = [];
//判斷購物車內是否有商品
function isExist(id) {
    cart = JSON.parse(localStorage.getItem(users_cart_No));
    for (let i = 0; i < cart.length; i++) {
        if (cart[i].cProductId === id) {
            return i;
        }
    }
    return -1;
}

var pdtItem = null;
//將商品加入到購物車內
function addCart(obj_product) {
    //如果沒有登入則跳到登入畫面
    if (member_No == "") {
        window.alert("請先登入會員");
        window.location.assign("/ShoppingCart/CurrentCartItem");
        return;
    }
    if (obj_product.cQuantity <=0 || obj_product.cIsOnSaleId != 1) {
        window.alert("此商品目前已售完，請稍後");
        return;
    }
    var NowDate = new Date();  /*現在時間*/
    var h = NowDate.getHours();
    console.log(h)
    if (h >= 3 && h <= 10) {
        console.log("1")
        if (obj_product.cIsBreakFast == 0) {
            window.alert("此時段尚未販售，請耐心等待");
            return;
        }
    }
    if (h >= 10 && h <= 17) {
        console.log("2")
        if (obj_product.cIsLunch == 0) {
            window.alert("此時段尚未販售，請耐心等待");
            return;
        }
    }
    if (h >= 17 && h <= 23 || h >= 0 && h <= 3) {
        console.log("3")
        if (obj_product.cIsDinner == 0) {
            window.alert("此時段尚未販售，請耐心等待");
            return;
        }
    }
    console.log("4")
    if (localStorage.getItem(users_cart_No)) {
        let pdtindex = isExist(obj_product.cProductId);
        if (pdtindex != -1) {
            console.log("5")
            window.alert("此商品已在購物車中");
           
        } else {
            console.log("6")
            addItemInCart(obj_product);
            
        }
    } else {
console.log("7")
        addItemInCart(obj_product);
        
    }
}
//若購物車內無該商品則加入
function addItemInCart(obj_product) {
    let cartItem = {
        cProductId: obj_product.cProductId,
        cProductName: obj_product.cProductName,
        cPrice: obj_product.cPrice,
        cPicture: obj_product.cPicture,
        QuantityInCart: 1,
        ProductAmount: obj_product.cPrice * 1
    };
    cart.push(cartItem);
    window.alert("已加入購物車");
    localStorage.setItem(users_cart_No, JSON.stringify(cart));
}
//分頁function
function pagesForSearch(page) {

    
    $.ajax({
        url: "/HomePage/getSearchKey/",
        type: "POST",
        data: {
            "key": $("#SearchKeyInput").val(), "Category": $("#SearchOptionCategory option:selected").val(), "Min": $("#SearchKeyInputMinPrice").val(), "Max": $("#SearchKeyInputMaxPrice").val(), "BreakFast": $('#CheckboxForBreakFast:checked').val(), "Lunch": $('#CheckboxForLunch:checked').val(), "Dinner": $('#CheckboxForDinner:checked').val()
        },
        success: function (data) {
            pdtItem = data;
            console.log(data);

             
            var itemPerPage = 8;
            //var HowManyPages = (data.length / itemPerPage);   //40/8
            var textForLoop = "";
            var items = (page + 1) * itemPerPage;//1*8 2*8
            if (items > data.length) {
                items = data.length;
            }

            for (var i = (page * itemPerPage); i < items; i++) {
                var images = data[i].tProduct.cPicture;
                var imageSrc = images.replace("~", "");
                console.log(imageSrc);

                textForLoop += `<div class="col-lg-3 col-md-4 col-sm-6 mix oranges fresh-meat">
                                        <div class="featured__item">
                                        <div class="featured__item__pic set-bg" style="background-image: url(${data[i].tProduct.cPicture.replace("~", "")})">
                                        <ul class="featured__item__pic__hover">
                                         <li><a href = "#" ><i class="fa fa-heart"></i></a></li>
                                         <li><a href = "#" ><i class="fa fa-retweet"></i></a></li>
                                         <li><a href="javascript: void(0)" onclick="addCart(pdtItem[${i}].tProduct)"><i class="fa fa-shopping-cart"></i></a></li>
                                         </ul>
                                         <ul class="start_for_homepage">`
                let Average_message_adri = data[i].count <= 0 ? 0 : parseInt(data[i].sum / data[i].count)
                for (let x = 0; x < Average_message_adri; x++) {
                    textForLoop += `<li><span class="fa fa-star checked" style="color: orange;"></span></li>`
                }
                for (let g = 0; g < 5 - Average_message_adri; g++) {
                    textForLoop += `<li><span class="fa fa-star checked" style="color: #d5d3cf;"></span></li>`
                }
                textForLoop += ` </ul>
                                                <span class="product_time_yu">製作時間：${data[i].tProduct.cFinishedTime}分鐘</span> `

                textForLoop += `</div>
                                         <div class="featured__item__text">
                                         <h6><a href="/ProductDetail/ProductData?id=${data[i].tProduct.cProductId}">${data[i].tProduct.cProductName}</a></h6>
                                         <h5>$${data[i].tProduct.cPrice}</h5>
                                         <h6>庫存量 ${data[i].tProduct.cQuantity} </h6>
                                         </div>
                                         </div>
                                         </div>`;


            }
            $("#pagesForForLoop").html(textForLoop);
        }

    });


}
