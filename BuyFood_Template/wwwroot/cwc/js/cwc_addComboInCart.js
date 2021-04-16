function cwc_addProducttoCart(product) {
    cwc_addCart(product);
    window.alert("已加入購物車");
}

function cwc_addCombotoCart(cwc_combo) {
    console.log(cwc_combo)
    var CurrentTime = new Date().getHours();
    var tcIndex = CurrentTime >= 3 && CurrentTime < 10 ? 0 :
        CurrentTime >= 10 && CurrentTime < 17 ? 1 : 2;

    var cb = cwc_combo.comboB,
        cl = cwc_combo.comboL,
        cd = cwc_combo.comboD;

    var timeCheck = [cb, cl, cd];

    if (timeCheck[tcIndex] != "0") {
        for (var i = 0; i < cwc_combo.comboDetails.length; i++) {
            cwc_addCart(cwc_combo.comboDetails[i].cProduct);
        }
        window.alert("已加入購物車");
    }
    else window.alert("套餐內容包含此時段不供應餐點，請耐心等待");

}

var cwc_cart = [];
//判斷購物車內是否有商品
function cwc_isExist(id) {
    cwc_cart = JSON.parse(localStorage.getItem(users_cart_No));
    for (let i = 0; i < cwc_cart.length; i++) {
        if (cwc_cart[i].cProductId === id) {
            cwc_cart[i].QuantityInCart += 1;
            localStorage.setItem(users_cart_No, JSON.stringify(cwc_cart));
            return i;
        }
    }
    return -1;
}

var cwc_pdtItem = null;
//將商品加入到購物車內
function cwc_addCart(obj_product) {
    //如果沒有登入則跳到登入畫面
    if (member_No == "") {
        window.alert("請先登入會員");
        window.location.assign("/ShoppingCart/CurrentCartItem");
        return;
    }
    if (obj_product.cQuantity <= 0 || obj_product.cIsOnSaleId != 1) {
        console.log(obj_product)
        window.alert("此商品目前已售完，請稍後");
        return;
    }
    var NowDate = new Date();  /*現在時間*/
    var h = NowDate.getHours();
    console.log(h)
    if (h >= 3 && h <= 10) {
        if (obj_product.cIsBreakFast == 0) {
            window.alert("此時段尚未販售，請耐心等待");
            return;
        }
    }
    if (h >= 10 && h <= 17) {
        if (obj_product.cIsLunch == 0) {
            window.alert("此時段尚未販售，請耐心等待");
            return;
        }
    }
    if (h >= 17 && h <= 23 || h >= 0 && h <= 3) {
        if (obj_product.cIsDinner == 0) {
            window.alert("此時段尚未販售，請耐心等待");
            return;
        }
    }
    if (localStorage.getItem(users_cart_No)) {
        let cwc_pdtItem = cwc_isExist(obj_product.cProductId);
        if (cwc_pdtItem == -1) {
            cwc_addItemInCart(obj_product);
        }
    } else {
        cwc_addItemInCart(obj_product);
    }
}
//若購物車內無該商品則加入
function cwc_addItemInCart(obj_product) {
    let cartItem = {
        cProductId: obj_product.cProductId,
        cProductName: obj_product.cProductName,
        cPrice: obj_product.cPrice,
        cPicture: obj_product.cPicture,
        QuantityInCart: 1,
        ProductAmount: obj_product.cPrice * 1
    };
    cwc_cart.push(cartItem);
    localStorage.setItem(users_cart_No, JSON.stringify(cwc_cart));
}