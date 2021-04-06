$(function(){
    var textForTops = "";
    var countForTops = 1;
    $.ajax({
        url: "/HomePage/getTopRatedProducts/",
        type: "POST",
        async: true,
        success: function (topProducts) {
            
            textForTops += `<div class="col-lg-4 col-md-6">
                                                    <div class="latest-product__text">
                                                        <h4>好評商品</h4>
                                                        <div class="latest-product__slider owl-carousel">`;
            for (let a = 0; a < 2; a++) {
                textForTops += `<div class="latest-prdouct__slider__item">`;

                for (let b = (3 * (countForTops - 1)); b < (3 * countForTops); b++) {
                    textForTops += `<a href="/ProductDetail/ProductData?id=${topProducts[b].cProductId}" class="latest-product__item">
                                                            <div class="latest-product__item__pic">
                                                                <img src="${topProducts[b].cPicture.replace("~", "")}" style="width:100px;height:105.86px;" alt="">
                                                                            </div>
                                                                <div class="latest-product__item__text">
                                                                    <h6>${topProducts[b].cProductName}</h6>
                                                                    <span>$${topProducts[b].cPrice}</span>
                                                                </div>
                                                                        </a>`;

                }
                textForTops += `</div>`;
                countForTops++
            }
            textForTops += `</div>
                        </div></div>`;

            $("#ProductSliderForAnime").append(textForTops);
            //console.log('topProducts');
        }
    });


})