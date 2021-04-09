let member_No = $("#user_member").val();
let users_cart_No = "cart" + member_No;        //依登入的會員改變localstorage的Key值
let product_name = $("#pdtName").text();       //商品的名稱
let product_count = $("#productcount").val();  //會員輸入的商品數量
let product_price = parseFloat($("#pdtPrice").text().replace('$', ''));  //商品的價格
let product_image = $("#pdtImage").attr("src")   //商品的圖片路徑
let pdtcart_quantity = [];    //存放商品ID和庫存數量
//console.log(product_price);
//console.log(product_image);
//載入商品細表時抓取現有庫存量
$(function () {
    let pdt_ID = parseInt($("#getid").val());
    let pdtIDquantity = [];
    let pdtInCart = pdt_ID
    pdtIDquantity.push(pdtInCart);

    $.ajax({
        url: "/ShoppingCart/CurrentItemCount",
        data: JSON.stringify(pdtIDquantity),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            pdtcart_quantity = data;    //回傳的商品ID和總數量
        }
    });
})
//手動輸入數量
function ChangeQuantity() {
    let count = $("#productcount").val();
    if (count > pdtcart_quantity[0].quantityInCart) {   //檢查產品現有可製作的份量
        window.alert("已超過現有可製作的份量");
        count = pdtcart_quantity[0].quantityInCart;
    }
    $("#productcount").val(count);
    product_count = count;
}
//按下-按鈕數量-1
function decbtn() {
    var quantity = $("#productcount").val();
    if (quantity > 1)
        var count = parseFloat(quantity) - 1;
    else
        var count = 1;
    $("#productcount").val(count);
    product_count = count;
}
//按下+按鈕數量+1
function incbtn() {
    var quantity = $("#productcount").val();
    var count = parseFloat(quantity) + 1;
    if (count > pdtcart_quantity[0].quantityInCart) {
        window.alert("已超過現有可製作的份量");
        count -= 1;
    }
    $("#productcount").val(count);
    product_count = count;
}

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
//加入購物車方法
var pdtItem = null;
function addCart(id) {
    //如果沒有登入則跳到登入畫面
    if (member_No == "") {
        window.location.assign("/ShoppingCart/CurrentCartItem");
        return;
    }
    if (localStorage.getItem(users_cart_No)) {
        let pdtindex = isExist(id);
        if (pdtindex != -1) {
            window.alert("此商品已在購物車中");
        } else {
            addItemInCart(id);
        }
    } else {
        addItemInCart(id);
    }
}
//若購物車內無該商品則加入
function addItemInCart(id) {
    let cartItem = {
        cProductId: id,
        cProductName: product_name,
        cPrice: product_price,
        cPicture: product_image,
        QuantityInCart: product_count,
        ProductAmount: product_price * product_count
    };
    cart.push(cartItem);
    window.alert("已加入購物車");
    localStorage.setItem(users_cart_No, JSON.stringify(cart));
}