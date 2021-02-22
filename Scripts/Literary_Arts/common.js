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
        async:false,
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
        error: function (res) { }
        
    });
}
