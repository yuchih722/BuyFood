﻿//<label class="btn btn-info">
    //<input id="upload_img" style="display:none;" type="file" accept="image/*"><i class="fa fa-photo"></i>
      //              上傳圖片
     //       </label>
    //<button id="crop_img" class="btn btn-info"><i class="fa fa-scissors"></i> 裁剪圖片</button>

    //<div id="oldImg" style="display:none;"></div>
    //<div id="newImg"></div>


    (function ($) {
        var width_crop = 300, // 圖片裁切寬度 px 值
            height_crop = 300, // 圖片裁切高度 px 值
            type_crop = "square", // 裁切形狀: square 為方形, circle 為圓形
            width_preview = 350, // 預覽區塊寬度 px 值
            height_preview = 350, // 預覽區塊高度 px 值
            compress_ratio = 0.85, // 圖片壓縮比例 0~1
            type_img = "jpeg", // 圖檔格式 jpeg png webp
            oldImg = new Image(),
            myCrop, file, oldImgDataUrl;

        // 裁切初始參數設定
        myCrop = $("#oldImg").croppie({
            viewport: { // 裁切區塊
                width: width_crop,
                height: height_crop,
                type: type_crop
            },
            boundary: { // 預覽區塊
                width: width_preview,
                height: height_preview
            }
        });

        function readFile(input) {
            if (input.files && input.files[0]) {
                file = input.files[0];
            } else {
                alert("瀏覽器不支援此功能！建議使用最新版本 Chrome");
                return;
            }

            if (file.type.indexOf("image") == 0) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    oldImgDataUrl = e.target.result;
                    oldImg.src = oldImgDataUrl; // 載入 oldImg 取得圖片資訊
                    myCrop.croppie("bind", {
                        url: oldImgDataUrl
                    });
                };

                reader.readAsDataURL(file);
            } else {
                alert("您上傳的不是圖檔！");
            }
        }

        function displayCropImg(src) {
            var html = "<img id='cwc_upphoto' src='" + src + "' />";
            $("#newImg").html(html);
        }





        //function displayNewImgInfo(src) {
        //    var html = "",
        //        filesize = src.length * 0.75;

        //    html += "<p>裁切圖片尺寸 " + width_crop + "x" + height_crop + "</p>";
        //    html += "<p>檔案大小約 " + Math.round(filesize / 1000) + "k</p>";
        //    $("#newImgInfo").html(html);
        //}

        $("#upload_img").on("change", function () {
            $("#oldImg").show();
            readFile(this);
        });

        //oldImg.onload = function () {
        //    var width = this.width,
        //        height = this.height,
        //        fileSize = Math.round(file.size / 1000),
        //        html = "";

        //    html += "<p>原始圖片尺寸 " + width + "x" + height + "</p>";
        //    html += "<p>檔案大小約 " + fileSize + "k</p>";
        //    $("#oldImg").before(html);
        //};

        $("#crop_img").on("click", function () {
            myCrop.croppie("result", {
                type: "canvas",
                format: type_img,
                quality: compress_ratio
            }).then(function (src) {
                displayCropImg(src);
                console.log($("#cwc_upphoto").attr("src"));
                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ "dataURL": `${$("#cwc_upphoto").attr("src")}` }),
                    url: `/Member/decodeBase64ToImage`,
                    success: function (data) {
                        console.log(data);
                    }
                })
                //    displayNewImgInfo(src);
            });
        });
    })(jQuery);
   