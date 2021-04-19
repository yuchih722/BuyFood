function cwc_showReport() {
    $("#head_cwc").html("");
    $("#content_cwc").html("");
    $.ajax({
        url: `/Member/checkOD`, //先確認有無點餐紀錄
        success: function (result) { //result為最近一年點餐的年份
            console.log(result);
            if (result != "0") {
                var txt = `<div style="display:inline-block;width:22%;text-align:left">
                    <label> 查詢 : 
                    <select id="cwc_sel_year">
                    </select> 年
                    </label><br>
                    <label>實際付款金額 : </label><span id="cwc_sp_totalPay"></span>
                </div>
                <div style="display:inline-block;width:22%;text-align:left">
                    <label>點餐總額 : </label><span id="cwc_sp_totalOrder"></span><br>
                    <label>優惠總額 : </label><span id="cwc_sp_totalSave"></span>
                </div>
                <div style="display:inline-block;width:46%;text-align:left">
                    <label>喜愛餐點排名 :</label></br>
                    <span id="cwc_sp_orderRank"></span>
                </div>`;
                $("#head_cwc").html(txt);
                $("#cwc_sel_year").on('change', function () {
                    var changeYear = this.options[this.options.selectedIndex].value;
                    $("#content_cwc").html("");
                    getRecordData(changeYear, 1)
                });
                //$("#cwc_select_Category").html(txt).attr("onchange", `cwc_Category_selected(this.options[this.options.selectedIndex].value)`);

                console.log(result);
                getRecordData(result, "0");
            }
            else {
                var txt = `<h1>無點餐紀錄</h1>`;
                $("#content_cwc").html(txt);
            }
        }
    })
}
function getRecordData(y, first) {
    $.ajax({
        url: `/Member/memberRecord?year=${y}`,
        success: function (data) {
            console.log(data);
            console.log(`first=${first}`)
            //first = 0代表第一次載入須設定option
            if (first == "0") {
                var txt_option = ""
                for (var i = 0; i < data.year_coupon.length; i++) {
                    txt_option += i == 0 ? `<option selected value="${data.year_coupon[i].key}">${data.year_coupon[i].key}</option>` :
                        `<option value="${data.year_coupon[i].key}">${data.year_coupon[i].key}</option>`;
                }
                $("#cwc_sel_year").html(txt_option);
            };
            $(`#cwc_sp_totalPay`).html(`${data.totalPay}`);
            $(`#cwc_sp_totalOrder`).html(`${data.totalCost}`);
            $(`#cwc_sp_totalSave`).html(`${data.totalSave}`);

            var txt_top3 = "";
            for (var i = 0; i < data.top3.length; i++) {
                txt_top3 += `${i + 1}. ${data.top3[i].key}(${data.top3[i].count})  `;
            };
            $(`#cwc_sp_orderRank`).html(txt_top3);
            dashboard('#content_cwc', data.mth_data);
        }
    });
}

function dashboard(id, fData) {
    d3.select(id).append("div").attr("id", "top");
    d3.select(id).append("div").attr("id", "bottom").style("margin-top", "15px");

    var barColor = 'steelblue';
    function segColor(c) {
        return {
            麵食: "#AE0000",
            飯食: "#FF79BC",
            傳統小吃: "	#02F78E",
            湯品: "#00E3E3",
            炸物: "#FFE153",
            甜點: "#FF8040",
            飲品: "#4F9D9D",
            點心: "#5A5AAD"
        }[c];
    }

    // compute total for each state.
    fData.forEach(function (d) {
        d.total = d.ctys_sum.麵食 +
            d.ctys_sum.飯食 +
            d.ctys_sum.傳統小吃 +
            d.ctys_sum.湯品 +
            d.ctys_sum.炸物 +
            d.ctys_sum.甜點 +
            d.ctys_sum.飲品 +
            d.ctys_sum.點心;
    });

    // function to handle histogram.
    function histoGram(fD) {
        var hG = {}, hGDim = { t: 60, r: 0, b: 30, l: 0 };
        hGDim.w = 500 - hGDim.l - hGDim.r,
            hGDim.h = 300 - hGDim.t - hGDim.b;

        //create svg for histogram.
        var hGsvg = d3.select("#top").append("svg")
            .attr("width", hGDim.w + hGDim.l + hGDim.r)
            .attr("height", hGDim.h + hGDim.t + hGDim.b).append("g")
            .attr("transform", "translate(" + hGDim.l + "," + hGDim.t + ")");

        // create function for x-axis mapping.
        var x = d3.scale.ordinal().rangeRoundBands([0, hGDim.w], 0.1)
            .domain(fD.map(function (d) { return d[0]; }));

        // Add x-axis to the histogram svg.
        hGsvg.append("g").attr("class", "x axis")
            .attr("transform", "translate(0," + hGDim.h + ")")
            .call(d3.svg.axis().scale(x).orient("bottom"));

        // Create function for y-axis map.
        var y = d3.scale.linear().range([hGDim.h, 0])
            .domain([0, d3.max(fD, function (d) { return d[1]; })]);

        // Create bars for histogram to contain rectangles and freq labels.
        var bars = hGsvg.selectAll(".bar").data(fD).enter()
            .append("g").attr("class", "bar");

        //create the rectangles.
        bars.append("rect")
            .attr("x", function (d) { return x(d[0]); })
            .attr("y", function (d) { return y(d[1]); })
            .attr("width", x.rangeBand())
            .attr("height", function (d) { return hGDim.h - y(d[1]); })
            .attr('fill', barColor)
            .on("mouseover", mouseover)// mouseover is defined below.
            .on("mouseout", mouseout);// mouseout is defined below.

        //Create the frequency labels above the rectangles.
        bars.append("text").text(function (d) { return d3.format(",")(d[1]) })
            .attr("x", function (d) { return x(d[0]) + x.rangeBand() / 2; })
            .attr("y", function (d) { return y(d[1]) - 5; })
            .attr("text-anchor", "middle");

        function mouseover(d) {  // utility function to be called on mouseover.
            // filter for selected state.
            var st = fData.filter(function (s) { return s.month == d[0]; })[0],
                nD = d3.keys(st.ctys_sum).map(function (s) { return { type: s, ctys_sum: st.ctys_sum[s] }; });

            // call update functions of pie-chart and legend.
            pC.update(nD);
            leg.update(nD);
        }

        function mouseout(d) {    // utility function to be called on mouseout.
            // reset the pie-chart and legend.
            pC.update(tF);
            leg.update(tF);
        }

        // create function to update the bars. This will be used by pie-chart.
        hG.update = function (nD, color) {
            // update the domain of the y-axis map to reflect change in frequencies.
            y.domain([0, d3.max(nD, function (d) { return d[1]; })]);

            // Attach the new data to the bars.
            var bars = hGsvg.selectAll(".bar").data(nD);

            // transition the height and color of rectangles.
            bars.select("rect").transition().duration(500)
                .attr("y", function (d) { return y(d[1]); })
                .attr("height", function (d) { return hGDim.h - y(d[1]); })
                .attr("fill", color);

            // transition the frequency labels location and change value.
            bars.select("text").transition().duration(500)
                .text(function (d) { return d3.format(",")(d[1]) })
                .attr("y", function (d) { return y(d[1]) - 5; });
        }
        return hG;
    }

    // function to handle pieChart.
    function pieChart(pD) {
        var pC = {}, pieDim = { w: 300, h: 300 };
        pieDim.r = Math.min(pieDim.w, pieDim.h) / 2;

        // create svg for pie chart.
        var piesvg = d3.select("#bottom").append("svg")
            .style("vertical-align", "top").style("margin-right", "25px")
            .attr("width", pieDim.w).attr("height", pieDim.h).append("g")
            .attr("transform", "translate(" + pieDim.w / 2 + "," + pieDim.h / 2 + ")");

        // create function to draw the arcs of the pie slices.
        var arc = d3.svg.arc().outerRadius(pieDim.r - 10).innerRadius(0);

        // create a function to compute the pie slice angles.
        var pie = d3.layout.pie().sort(null).value(function (d) { return d.ctys_sum; });

        // Draw the pie slices.
        piesvg.selectAll("path").data(pie(pD)).enter().append("path").attr("d", arc)
            .each(function (d) { this._current = d; })
            .style("fill", function (d) { return segColor(d.data.type); })
            .on("mouseover", mouseover).on("mouseout", mouseout);

        // create function to update pie-chart. This will be used by histogram.
        pC.update = function (nD) {
            piesvg.selectAll("path").data(pie(nD)).transition().duration(500)
                .attrTween("d", arcTween);
        }
        // Utility function to be called on mouseover a pie slice.
        function mouseover(d) {
            // call the update function of histogram with new data.
            hG.update(fData.map(function (v) {
                return [v.month, v.ctys_sum[d.data.type]];
            }), segColor(d.data.type));
        }
        //Utility function to be called on mouseout a pie slice.
        function mouseout(d) {
            // call the update function of histogram with all data.
            hG.update(fData.map(function (v) {
                return [v.month, v.total];
            }), barColor);
        }
        // Animating the pie-slice requiring a custom function which specifies
        // how the intermediate paths should be drawn.
        function arcTween(a) {
            var i = d3.interpolate(this._current, a);
            this._current = i(0);
            return function (t) { return arc(i(t)); };
        }
        return pC;
    }

    // function to handle legend.
    function legend(lD) {
        var leg = {};

        // create table for legend.
        var legend = d3.select("#bottom").append("table").attr('class', 'legend');

        // create one row per segment.
        var tr = legend.append("tbody").selectAll("tr").data(lD).enter().append("tr");

        // create the first column for each segment.
        tr.append("td").append("svg").attr("width", '16').attr("height", '16').append("rect")
            .attr("width", '16').attr("height", '16')
            .attr("fill", function (d) { return segColor(d.type); });

        // create the second column for each segment.
        tr.append("td").text(function (d) { return d.type; });

        // create the third column for each segment.
        tr.append("td").attr("class", 'legendFreq')
            .text(function (d) { return d3.format(",")(d.ctys_sum); });

        // create the fourth column for each segment.
        tr.append("td").attr("class", 'legendPerc')
            .text(function (d) { return getLegend(d, lD); });

        // Utility function to be used to update the legend.
        leg.update = function (nD) {
            // update the data attached to the row elements.
            var l = legend.select("tbody").selectAll("tr").data(nD);

            // update the frequencies.
            l.select(".legendFreq").text(function (d) { return d3.format(",")(d.ctys_sum); });

            // update the percentage column.
            l.select(".legendPerc").text(function (d) { return getLegend(d, nD); });
        }

        function getLegend(d, aD) { // Utility function to compute percentage.
            return d3.format("%")(d.ctys_sum / d3.sum(aD.map(function (v) { return v.ctys_sum; })));
        }

        return leg;
    }

    // calculate total frequency by segment for all state.
    var tF = ['麵食', '傳統小吃', '湯品', '炸物', '甜點', '飯食', '飲品', '點心'].map(function (d) {
        return { type: d, ctys_sum: d3.sum(fData.map(function (t) { return t.ctys_sum[d]; })) };
    });

    // calculate total frequency by state for all segment.
    var sF = fData.map(function (d) { return [d.month, d.total]; });

    var hG = histoGram(sF), // create the histogram.
        pC = pieChart(tF), // create the pie-chart.
        leg = legend(tF);  // create the legend

}