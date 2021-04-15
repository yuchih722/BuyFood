
$(function () {
    $.ajax({
        url: "/HomePage/get_categorysname",
        type: "GET",
        success: function (dataForHomePage) {
            console.log(dataForHomePage)


            //首頁商品
            var category_li =
                `<ul><li class="active"  id="FirstCate" data-filter=".categoryid_${dataForHomePage.forProduct[0].cProductCategoryId}">${dataForHomePage.forProduct[0].cCategoryName}</li>`;
            for (let i = 1; i < dataForHomePage.forProduct.length; i++) {
                category_li += `<li data-filter=".categoryid_${dataForHomePage.forProduct[i].cProductCategoryId}">${dataForHomePage.forProduct[i].cCategoryName}</li>`
            };
            category_li += `</li>`
            $("#push_procategory").append(category_li);
            pullme = dataForHomePage.forProduct;

            for (let i = 0; i < dataForHomePage.forProduct.length; i++) {
                var products = ""; 
                for (let y = 0; y < dataForHomePage.forProduct[i].tProducts.length; y++) {
                    products += `<div class="col-lg-3 col-md-4 col-sm-6 mix categoryid_${dataForHomePage.forProduct[i].tProducts[y].tProducts.cCategoryId}">
                                            <div class="featured__item">
                                                <div class="featured__item__pic set-bg" style="background-image: url(${dataForHomePage.forProduct[i].tProducts[y].tProducts.cPicture.replace("~", "")})">
                                                    <ul class="featured__item__pic__hover">

                                                        <li><a href="javascript: void(0)" onclick="addCart(pullme[${i}].tProducts[${y}].tProducts)"><i class="fa fa-shopping-cart"></i></a></li>
                                                    </ul>
                                                    <ul class="start_for_homepage">`
                    let Average_message_yu = dataForHomePage.forProduct[i].tProducts[y].coun <= 0 ? 0 :   parseInt(dataForHomePage.forProduct[i].tProducts[y].sum / dataForHomePage.forProduct[i].tProducts[y].coun)
                    for (let x = 0; x < Average_message_yu; x++) {
                        products +=`<li><span class="fa fa-star checked" style="color: orange;font-size:25px"></span></li>`
                    }
                    for (let g = 0; g < 5 - Average_message_yu; g++) {
                        products += `<li><span class="fa fa-star checked" style="color: #d5d3cf;font-size:25px"></span></li>`
                    }                              
                    products += ` </ul>
                                                <span class="product_time_yu">製作時間：${dataForHomePage.forProduct[i].tProducts[y].tProducts.cFinishedTime}分鐘</span> `

                    products +=   ` </div>
                                                <div class="featured__item__text">
                                                    <h6><a href="/ProductDetail/ProductData?id=${dataForHomePage.forProduct[i].tProducts[y].tProducts.cProductId}">${dataForHomePage.forProduct[i].tProducts[y].tProducts.cProductName}</a></h6>
                                                    <h5>$${dataForHomePage.forProduct[i].tProducts[y].tProducts.cPrice}</h5>
                                                    <h6>庫存量 ${dataForHomePage.forProduct[i].tProducts[y].tProducts.cQuantity} </h3>
                                                </div>
                                            </div>
                                        </div>`
                }
                $("#many_products").append(products);
            }

            $('.featured__controls li').on('click', function () {

                $('.featured__controls li').removeClass('active');
                $(this).addClass('active');
            });
            if ($('.featured__filter').length > 0) {
                var containerEl = document.querySelector('.featured__filter');
                var mixer = mixitup(containerEl);
            }
            $("#FirstCate").click();
        }
    })
})

    //< li > <a href="#"><i class="fa fa-heart"></i></a></li >
    //    <li><a href="#"><i class="fa fa-retweet"></i></a></li>