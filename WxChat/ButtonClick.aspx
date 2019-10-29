<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ButtonClick.aspx.cs" Inherits="WxChat.ButtonClick" %>

 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
      <meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=0" />
    <title></title>
    
    <link rel="stylesheet" type="text/css" href="https://cdn.bootcss.com/weui/1.1.3/style/weui.min.css" />
    <link rel="stylesheet" href="https://cdn.bootcss.com/jquery-weui/1.2.1/css/jquery-weui.min.css" />
</head>
<style>
    #btnCode {
        margin-bottom:50px;
    }
</style>
<body>
    <form id="form1" runat="server">
        <div>
            
            <hr style="border: 1px solid gray;" />
            <div class="content" style="width: 100%; text-align: center; background-color: aliceblue; color: blueviolet">
                <a href="javascript:" id="btnCode" class="weui-btn weui-btn_primary">获取授权</a>
             
            </div>
             


        </div>
    </form>
    <script src="https://cdn.bootcss.com/jquery/2.2.4/jquery.js"></script>

    <!--引入微信的两个js 微信公众号 JS SDK 接口调用必须使用-->
    <script type="text/javascript " src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js "></script>
    <!--jqueryWEUI原生和微信Weui作用相同-->
    <script src="https://cdn.bootcss.com/jquery-weui/1.2.1/js/jquery-weui.min.js"></script>
    <script>
        $(function () {
            const APPID = 'wx725745973888374b';
            const REDIRECT_URI = 'http://zy4rr4.natappfree.cc/GetUser.aspx';
            const SCOPE = 'snsapi_userinfo';


            $("#btnCode").click(function () {
                var urls = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=' + APPID + '&redirect_uri=' + encodeURIComponent(REDIRECT_URI) + '&response_type=code&scope=' + SCOPE + '&state=STATE#wechat_redirect'
                 window.location.href = urls; //"index.html"
                //delayURL(); //三秒后跳转授权页面
                //function delayURL() {
                //    var urls = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=' + APPID + '&redirect_uri=' + encodeURIComponent(REDIRECT_URI) + '&response_type=code&scope=' + SCOPE + '&state=STATE#wechat_redirect'
                //    var delay = $("#time").html();
                //    var t = setTimeout("delayURL()", 1000);
                //    if (delay > 0) {
                //        delay--;
                //        $("#time").html(delay);
                //    } else {
                //        clearTimeout(t);
                //        window.location.href = urls; //"index.html"
                //    }
                //}
            })

        })

    </script>
</body>
</html>
