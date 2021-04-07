  $(function () {
        $.ajax({
            url: "/HomePage/輪播牆",
            type: "GET",
            success: function (data) {
                var txt = "";
                var addsome = "";
                for (let i = 0; i < data.length; i++) {
                    txt += `<a href="${data[i].cLink}"><div class="hero__item set-bg"  id="slider_homepage" title="${data.cDescription}" style="background-image: url('${data[i].cPicture}');"></div>`
                }
                addsome += `<ul  class="dot yuUsingUl">`
                for (let i = 0; i < data.length; i++) {
                    addsome += `<li class="yuUsingLi" id="${i + 1}"></li>`
                }
                addsome += `</ul >`;
                            //<div id="prevSlide" class="slide_btn">
                            //         <i class="fa fa-caret-left"></i>
                            //</div>
                            //<div id="nextSlide" class="slide_btn">
                            //          <i class="fa fa-caret-right"></i>
                            //</div>;
                $("#runpage").html(txt);
                $("#addsome").append(addsome);

                //設定輪播牆移動
                var img_count = data.length; var img_width = 825;
                var img_index = 0; var lastimg_index = img_count - 1;
                var time = 3000;

                width_runpage = img_count * img_width; //總寬度

                $("#runpage").css("width", width_runpage) //輪播總寬度

                $(".yuUsingLi").eq(0).css("background-color", "#000000");//設定進入畫面給樣式                         

                //執行後判斷位置
                function auto() {
                    img_index++;
                    if (img_index >= img_count) {
                        img_index = 0;
                    }
                    autochang();
                }

                //此方法自動替換圖片位置及DOT位置
                function autochang() {
                    postion = -img_index * img_width;
                    $("#runpage").animate({ "left": +postion + "px" }, 400);
                    $(".yuUsingLi").eq(img_index).css("background-color", "#000000").siblings().css("background-color", "transparent")
                }

                //移動到照片後停止動作 離開後繼續動作
                $("#runpage div").hover(stopshow, goshow)
                clock = setInterval(auto, time);//設定時間開始

                //移入DOT 抓出index 修改樣式設定停止 移出繼續
                $(".yuUsingLi").hover(function () {
                    img_index = $(this).index();
                    autochang();
                    stopshow();
                }, goshow)

                $(".yuUsingUl").mousemove(stopshow)

                ////按下左
                //$("#prevSlide").click(function () {
                //    img_index--;
                //    if (img_index < 0) img_index = lastimg_index;
                //    autochang();
                //}).mousemove(stopshow).mouseout(goshow)

                ////按下右
                //$("#nextSlide").click(function () {
                //    img_index++;
                //    if (img_index > lastimg_index) img_index = 0;
                //    autochang();
                //}).mousemove(stopshow).mouseout(goshow)

                function goshow() {
                    clock = setInterval(auto, time)
                }
                function stopshow() {
                    window.clearTimeout(clock);
                }
            }
        })
    })
