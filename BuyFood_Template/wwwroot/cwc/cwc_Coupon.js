function cwc_showCoupon(memberID) {
    var txt =
        `<ul class="nav nav-pills mb-3 justify-content-center"
                        id="pills-tab"
                        role="tablist">
                        <li class="nav-item"
                                role="presentation">
                                <button class="nav-link active"
                                                id="pills-unuse-tab"
                                                data-bs-toggle="pill"
                                                data-bs-target="#pills-unuse"
                                                type="button"
                                                role="tab"
                                                aria-controls="pills-unuse"
                                                aria-selected="true"
                                                onclick="cwc_showCoupons(${memberID},0)">
                                未使用
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link"
                                                id="pills-used-tab"
                                               data-bs-toggle="pill"
                                                data-bs-target="#pills-used"
                                                type="button"
                                                role="tab"
                                                aria-controls="pills-used"
                                                aria-selected="false"
                                                onclick="cwc_showCoupons(${memberID},1)">
                                已使用
                                </button>
                            </li>
                    </ul>`;
    var txtBottom = `<div class="tab-content" id="pills-tabContent">
            <div class="tab-pane fade show active" id="pills-unuse" role="tabpanel" aria-labelledby="pills-unuse-tab"></div>
            <div class="tab-pane fade" id="pills-used" role="tabpanel" aria-labelledby="pills-used-tab"></div>
        </div>`;
    $("#head_cwc").html(txt);
    $("#content_cwc").html(txtBottom);
    cwc_showCoupons(memberID, 0);
}
function cwc_showCoupons(memberID, used) {
    $.ajax({
        url: `/Coupon/getCoupons?id=${memberID}&used=${used}`,
        type: "GET",
        success: function (data) {
            var start =
                `<table class="table"><thead>
                            <tr><td>優惠券名稱</td>
                                    <td>領取日</td>
                                    <td>到期日</td></tr></thead><tbody>`;
            var end =
                `</tbody></table>`;
            var content = "";
            for (var i = 0; i < data.length; i++) {
                content += `<tr><td>${data[i].categoryName} <td>${data[i].rdate}<td>${data[i].edate}`;
            };
            var txt = start + content + end;

            if (used == 0)
                $("#pills-unuse").html(txt);
            else $("#pills-used").html(txt);
        }
    })

}
