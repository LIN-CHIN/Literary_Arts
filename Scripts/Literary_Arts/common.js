//點擊愛心按鈕 
//包括：文章、推薦...文章留言 推薦留言...
//參數 ： num : 文章/文章留言編號
//        type : "01" = 文章  , "02" = 推薦
//        isReply : true = 留言 , false = 非留言
function clickLike(num, type, isReply)
{
    var url = ""
    if (type == "01") {
        url = "/Article/"
    } else if (type == "02")
    {
        url = "/Recommend/"
    }                 

    $.ajax({
        url: url + "ClickLike",                        
        type: "post",
        dataType: "JSON",
        data: {
            "num": num,
            "isReply": isReply,
        },      
        success: function (res) {
            //沒登入重導
            if (res.success != undefined && res.message == "") {
                document.location.href = "/Member/Login";
                return false;
            }
            //新增/刪除愛心出錯
            else if (res.success != undefined) {
                alert(res.message);
                return false ;
            }

            //如果有按過愛心
            if (res.isClick) {
                $("#userLike_" + num).css({ "color": "" });
            } else {
                $("#userLike_" + num).css({ "color": "red" });
            }

            //重新計算愛心數量
            $("#likeCount_" + num).text(res.likeCount);
    
        },
        error: function () {
            alert("愛心新增/刪除系統異常，請洽系統管理員");
        }
        
    });
}


//點擊收藏按鈕 
//包括：文章、推薦...
//參數 ： num : 文章編號
//        type : "01" = 文章  , "02" = 推薦
function clickCollection(num, type) {
    var url = ""
    if (type == "01") {
        url = "/Article/"
    } else if (type == "02") {
        url = "/Recommend/"
    }

    $.ajax({
        url: url + "ClickCollection",
        type: "post",
        dataType: "JSON",
        data: {
            "num": num
        },
        success: function (res) {
            //沒登入重導
            if (res.success != undefined && res.message == "") {
                document.location.href = "/Member/Login";
                return false;
            }
            //新增/刪除收藏出錯
            else if (res.success != undefined) {
                alert(res.message);
                return false;
            }
            //如果有按過收藏
            if (res) {
                $("#i_userColl_" + num).removeClass("fas");
                $("#i_userColl_" + num).addClass("far");
            } else {
                $("#i_userColl_" + num).removeClass('far');
                $("#i_userColl_" + num).addClass('fas');
            }

        },
        error: function (res) {
            alert("收藏新增/刪除系統異常，請洽系統管理員");
        }

    });
}

//取代換行符號
function replaceBr(id)
{
    $("#" + id).text($("#" + id ).text().replace(new RegExp("<br>", "g"), "\n"));
}

//判斷是否有登入
function isLogin() {
    var result = false;
    if (loginUser != "") {
        result = true
    }
    return result;
}