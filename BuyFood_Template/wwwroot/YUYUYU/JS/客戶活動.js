
    $(function () {
        $.ajax({
            url: "/Activity/ReadRss",
            type: "GET",
            success: function (data) {

                console.log(data);
                var itmes = data.getElementsByTagName("item");

                var txt = "";
                for (let i = 0; i < 10; i++) {
                    txt += `<a href="${itmes[i].getElementsByTagName("link")[0].innerHTML}" class="blog__sidebar__recent__item">
                                     <div class="blog__sidebar__recent__item__pic">
                                     <img src="${itmes[i].getElementsByTagName("image")[0].innerHTML}" alt="" width="70px" height="70px">
                                     </div>
                                     <div class="blog__sidebar__recent__item__text">
                                     <h6 style="width:200px;text-align: justify">${itmes[i].getElementsByTagName("title")[0].innerHTML.slice(14, -7)}</h6>
                                     <span>${itmes[i].getElementsByTagName("pubDate")[0].innerHTML.slice(0, 16)}</span>
                                     </div>
                                     </a>`
                }
                $("#RSSNEWS").html(txt);

            }
        });

        var page_index = 0; page_index_last = 5;

        page();

        function page() {
        $.ajax({
            url: "/Activity/ActivityPageView",
            type: "GET",
            success: function (data) {
                var txt = "";
                if (page_index_last > data.length)
                    page_index_last = data.length - 1;
                for (let i = page_index; i <= page_index_last; i++) {

                    txt += `<div class="col-lg-6 col-md-6 col-sm-6" >
                               <div class="blog__item">
                               <div class="blog__item__pic">
                               <img src="${data[i].cPicture}" alt="">
                               </div>
                               <div class="blog__item__text">
                               <ul>
                               <li><i class="fa fa-calendar-o"></i> ${data[i].cTime}</li>
                               </ul>
                               <h5><a href="#">${data[i].cActivityName}</a></h5>
                               <p>${data[i].cDescription}</p>
                               <a href="${data[i].cLink}" class="blog__btn">前往活動<span class="arrow_right"></span></a>
                               </div>
                               </div>
                               </div>`
                }
                $("#ActivityPage").html(txt);

                var Pagenum = Math.ceil(data.length / 6);
                var btnbutton = "";
                btnbutton += `<div class="col-lg-12">
                    <div class="product__pagination blog__pagination">
`
                for (let i = 0; i < Pagenum; i++) {
                    btnbutton += `<button  style="margin:0 10px" class="Actiview btn btn-outline-dark">${i + 1}</button>`
                }
                btnbutton += `<button id="pageplus" class="btn btn-outline-dark"><i class="fa fa-long-arrow-right"></i></button>
                    </div>
                </div>`

                $("#ActivityPage").append(btnbutton);

                $(".Actiview").click(function () {
                    if ($(this).text() == 1) {
                        page_index = 0; page_index_last = 5;
                    } else {
                        page_index = ($(this).text() - 1) * 6 + 1;
                        page_index_last = page_index + 5
                        if (page_index_last > data.length) {
                            page_index_last = data.length - 1
                        }
                    }
                    page();
                })

                $("#pageplus").click(function () {
                    if (page_index == 0)
                        page_index += 1;
                    page_index += 6;
                    page_index_last = page_index + 5;
                    if (page_index_last > data.length) {
                        page_index = (Pagenum - 1) * 6 + 1
                        page_index_last = data.length - 1
                    }
                    page();
                })
            }
        })
    }
    })