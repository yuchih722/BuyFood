////var cart = [];
////function isExist(id) {
////    cart = JSON.parse(localStorage.getItem("cart"));
////    for (let i = 0; i < cart.length; i++) {
////        if (cart[i].cProductId === id) {
////            return i;
////        }
////    }
////    return -1;
////}

//////將商品加入到購物車內
////function addCartlist(obj_product) {
////    if (localStorage.getItem("cart")) {
////        let pdtindex = isExist(obj_product.cProductId);
////        if (pdtindex != -1) {
////            window.alert("此商品已在購物車中");
////        } else {
////            addItemInCartlist(obj_product);
////        }
////    } else {
////        addItemInCartlist(obj_product);
////    }
////}
//////若購物車內無該商品則加入
////function addItemInCartlist(obj_product) {
////    let cartItem = {
////        cProductId: obj_product.cProductId,
////        cProductName: obj_product.cProductName,
////        cPrice: obj_product.cPrice,
////        cPicture: obj_product.cPicture,
////        QuantityInCart: 1,
////        ProductAmount: obj_product.cPrice * 1
////    };
////    cart.push(cartItem);
////    window.alert("已加入購物車");
////    localStorage.setItem("cart", JSON.stringify(cart));
////}