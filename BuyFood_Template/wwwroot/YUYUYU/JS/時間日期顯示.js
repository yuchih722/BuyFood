    //設置時間
        function ShowTime() {
        function p(s) {                 /*把個位數的時間變成兩位數 好看*/
            return s < 10 ? '0' + s : s;
        };
            var NowDate = new Date();  /*現在時間*/
            var h = p(NowDate.getHours());   /*小時輸出兩位數*/
            var m = p(NowDate.getMinutes());  /*分鐘輸出兩位數*/
            var s = p(NowDate.getSeconds());  /*秒數輸出兩位數*/
            var yyyy = NowDate.getFullYear();    /*當前年份*/
            var mm = p(NowDate.getMonth() + 1);  /*月份輸出兩位數*/
            var dd = p(NowDate.getDate());  /*日期輸出兩位數*/
            var day_list = ['日', '一', '二', '三', '四', '五', '六']; var day = NowDate.getDay();  /*小時輸出兩位數*/
            /*if (h < 10) h = "0" + h; if (m < 10) m = "0" + m; if (s < 10) s = "0" + s; if (dd < 10) dd = "0" + dd; if (mm < 10) mm = "0" + mm;*/
            $("#timehtml").html('<div class="time_display clearfix"><span class="h com-time ">' + h + '</span><b>：</b><span class="m com-time ">' + m + '</span><b>：</b><span class="s com-time ">' + s + '</span></div><div class="date_display clearfix"><span class="years">' + yyyy + '年</span><span class="mot">' + mm + '月</span><span class="day">' + dd + '</span><span>星期' + day_list[day] + '</span></div>');
            setTimeout('ShowTime()', 1000);
        }
        setInterval("ShowTime()", 1000);
