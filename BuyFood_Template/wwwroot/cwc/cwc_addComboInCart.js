var cart = [];
//判斷購物車內是否有商品
function isExist(id) {
    cart = JSON.parse(localStorage.getItem(users_cart_No));
    for (let i = 0; i < cart.length; i++) {
        if (cart[i].cProductId === id) {
            cart[i].QuantityInCart += 1;
            localStorage.setItem(users_cart_No, JSON.stringify(cart));
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
    if (obj_product.cQuantity <= 0 || obj_product.cIsOnSaleId != 1) {
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
        if (pdtindex == -1) {
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
    localStorage.setItem(users_cart_No, JSON.stringify(cart));
}