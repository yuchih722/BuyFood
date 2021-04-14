var textForSideBarCate = "";
$.ajax({
    url: "/HomePage/getSideBar_CateNums",
    type: "POST",
    success: function (data) {

        for (var i = 0; i < data.length; i++) {
            var catName = data[i].cCategoryName.replace(/\s*/g, "");
            var catGroupBy = data[i].tProducts.length;
            textForSideBarCate += `<li><a href="#featureProductsForLocation" onclick="sideBarClick(${i})" style="font-size:17px;margin:10px">${catName}  (${catGroupBy})</a></li>`;
        }


        $("#sideBarCategory").append(textForSideBarCate);
    }
});

function sideBarClick(num) {

    $("#push_procategory ul li").eq(num).click();

}