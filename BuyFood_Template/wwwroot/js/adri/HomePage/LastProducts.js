$(function () {
    var txtLastProducts = "";
    var countForLasts = 1;
    $.ajax({
        url: "/HomePage/getLastProducts/",
        type: "POST",
        async: true,
        success: function (lastProducts) {
            
            txtLastProducts += `  <div class="col-lg-4 col-md-6">
                                                                                        <div class="latest-product__text">
                                                                                            <h4>最新商品</h4>
                                                                                            <div class="latest-product__slider owl-carousel" >`;
            for (let a = 0; a < 2; a++) {

                txtLastProducts += `<div class="latest-prdouct__slider__item">`;

                for (let b = (3 * (countForLasts - 1)); b < (3 * countForLasts); b++) {

                    txtLastProducts += ` <a href="/ProductDetail/ProductData?id=${lastProducts[b].cProductId}" class="latest-product__item">
                                                                                            <div class="latest-product__item__pic">
                                                                                                <img src="${lastProducts[b].cPicture.replace("~", "")}" style="width:100px;height:105.86px;" alt="">
                                                                                            </div>
                                                                                            <div class="latest-product__item__text">
                                                                                                <h6>${lastProducts[b].cProductName}</h6>
                                                                                                <span>$${lastProducts[b].cPrice}</span>
                                                                                            </div>
                                                                                        </a>`;

                }
                countForLasts++;

                txtLastProducts += `</div>`;
            }
            txtLastProducts += ` </div>
                                                        </div>
                                                    </div>`;

            $("#ProductSliderForAnime").append(txtLastProducts);
            //console.log('lastProducts');


        }
    })
})