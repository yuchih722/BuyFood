
$(function () {

    $.ajax({
        url: "/HomePage/get_categorysname",
        type: "GET",
        success: function (data_category) {
            //console.log(data_category)
            
            var category_li =
                `<ul><li class="active"  id="FirstCate" data-filter=".categoryid_${data_category[0].cProductCategoryId}">${data_category[0].cCategoryName}</li>`;
            for (let i = 1; i < data_category.length; i++) {
                category_li += `<li data-filter=".categoryid_${data_category[i].cProductCategoryId}">${data_category[i].cCategoryName}</li>`
            };
            category_li += `</li>`
            $("#push_procategory").append(category_li);
            pullme = data_category;

            for (let i = 0; i < data_category.length; i++) {
                var products = ""; 
                for (let y = 0; y < data_category[i].tProducts.length; y++) {
                    products += `<div class="col-lg-3 col-md-4 col-sm-6 mix categoryid_${data_category[i].tProducts[y].tProducts.cCategoryId}">
                                            <div class="featured__item">
                                                <div class="featured__item__pic set-bg" style="background-image: url(${data_category[i].tProducts[y].tProducts.cPicture.replace("~", "")})">
                                                    <ul class="featured__item__pic__hover">
                                                        <li><a href="#"><i class="fa fa-heart"></i></a></li>
                                                        <li><a href="#"><i class="fa fa-retweet"></i></a></li>
                                                        <li><a href="javascript: void(0)" onclick="addCart(pullme[${i}].tProducts[${y}].tProducts)"><i class="fa fa-shopping-cart"></i></a></li>
                                                    </ul>
                                                    <ul class="start_for_homepage">`
                    let Average_message_yu = data_category[i].tProducts[y].coun <= 0 ? 0 :   parseInt(data_category[i].tProducts[y].sum / data_category[i].tProducts[y].coun)
                    for (let x = 0; x < Average_message_yu; x++) {
                        products +=`<li><span class="fa fa-star checked" style="color: orange;"></span></li>`
                    }
                    for (let g = 0; g < 5 - Average_message_yu; g++) {
                        products += `<li><span class="fa fa-star checked" style="color: #d5d3cf;"></span></li>`
                    }                              
                    products += ` </ul>
                                                <span class="product_time_yu">製作時間：${data_category[i].tProducts[y].tProducts.cFinishedTime}分鐘</span> `

                    products +=   ` </div>
                                                <div class="featured__item__text">
                                                    <h6><a href="/ProductDetail/ProductData?id=${data_category[i].tProducts[y].tProducts.cProductId}">${data_category[i].tProducts[y].tProducts.cProductName}</a></h6>
                                                    <h5>$${data_category[i].tProducts[y].tProducts.cPrice}</h5>
                                                    <h6>庫存量 ${data_category[i].tProducts[y].tProducts.cQuantity} </h3>
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

