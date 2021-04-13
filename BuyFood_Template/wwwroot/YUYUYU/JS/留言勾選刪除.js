$("#Board99").click(function () { pagename = 99; (Board(pagename));})
$("#Board5").click(function () { pagename = 5; (Board(pagename)); })
$("#Board4").click(function () { pagename = 4; (Board(pagename)); })
$("#Board3").click(function () { pagename = 3; (Board(pagename)); })
$("#Board2").click(function () { pagename = 2; (Board(pagename)); })
$("#Board1").click(function () { pagename = 1; (Board(pagename)); })

$("#deletemore").dialog({
    autoOpen: false,
    modal: true,
        buttons: {
            "Ok": function () {
                var checked = $("#myTable input[type=checkbox]:checked");
                var $Array = new Array;
                for (let i = 0; i < checked.length; i++) {
                    var data = {};
                    data.strmember = checked[i].value
                    $Array.push(data);
                }
                $(this).dialog("close");
                //全選刪除
                $.ajax({
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    url: "/yuBoardsALL/DetailBoardDelete",
                    data: JSON.stringify($Array),
                    success: function (data) {

                            $(Board(pagename));

                        $("#btndelete").hide();
                        updateTOP()
                    }
                });
            },
            "Cancel": function () { $(this).dialog("close"); }
        },
        hide: "explode",
        show: "blind",
    })
        
