$(function () {
    var textForReviewsP = "";
    textForReviewsP += `    <div class="col-lg-4 col-md-6">
        <div class="latest-product__text">
            <h4>熱評商品</h4>
            <div class="latest-product__slider owl-carousel">`;

    $.ajax({
        url: "/HomePage/getReviewProducts/",
        type: "POST",
        async: true,
        success: function (ReviewProducts) {
            
            var ReviewTimes = 1;
            for (let a = 0; a < 2; a++) {
                textForReviewsP += `<div class="latest-prdouct__slider__item">`;
                for (let b = (3 * (ReviewTimes - 1)); b < (3 * ReviewTimes); b++) {
                    textForReviewsP += ` <a href="/ProductDetail/ProductData?id=${ReviewProducts[b].cProductId}" class="latest-product__item">
                                                                    <div class="latest-product__item__pic">
                                                                        <img src="${ReviewProducts[b].cPicture.replace("~", "")}" style="width:100px;height:105.86px;" alt="">
                                                                    </div>
                                                                    <div class="latest-product__item__text">
                                                                        <h6>${ReviewProducts[b].cProductName}</h6>
                                                                        <span>$${ReviewProducts[b].cPrice}</span>
                                                                    </div>
                                                                </a>`;
                }
                ReviewTimes++;
                textForReviewsP += `</div>`;
            }
            textForReviewsP += `            
                                </div>
                                        </div>
                                    </div>`;

            $("#ProductSliderForAnime").append(textForReviewsP);
            //console.log('ReviewProducts');
            /*--------------------------
            Latest Product Slider
            ----------------------------*/
            $(".latest-product__slider").owlCarousel({
                loop: true,
                margin: 0,
                items: 1,
                dots: false,
                nav: true,
                navText: ["<span class='fa fa-angle-left'><span/>", "<span class='fa fa-angle-right'><span/>"],
                smartSpeed: 1200,
                autoHeight: false,
                autoplay: true
            });
            //console.log('Carousel');
        }
    });






})


$(window).load(function () {




})