<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUser.aspx.cs" Inherits="WxChat.GetUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
      <meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=0" />
    <title></title>
    <link rel="stylesheet" href="https://cdn.bootcss.com/jquery-weui/1.2.1/css/jquery-weui.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--<div class="title">
                用户信息获取：
            </div>--%>
            <%--  <div>
                错误信息：
                <label runat="server" id="lerror"></label>
            </div>--%>
            <hr style="border: 1px solid gray;" />
            <div class="content" style="width: 100%; text-align: center; background-color: aliceblue; color: blueviolet">
                 <%--<button   id="time" class="weui-btn weui-btn_primary">3</button>秒后跳转注册页面--%> 
             <%--   <div>
                    nickname :
                <label runat="server" id="lnickname"></label>

                </div>
                <div>
                    sex :
                <label runat="server" id="lsex"></label>

                </div>
                <div>
                    headimgurl :
                <label runat="server" id="lheadimgurl"></label>

                </div>
                <div>
                    province :
                <label runat="server" id="lprovince"></label>

                </div>
                <div>
                    language :
                <label runat="server" id="lcountry"></label>

                </div>
                <div>
                    accesstoken :
                <label runat="server" id="llanguage"></label>

                </div>
                <div>
                    city :
                <label runat="server" id="lcity"></label>

                </div>
                <div>
                    lopenid :
                <label runat="server" id="lopenid"></label>

                </div>

                <div>
                    unionid :
                <label runat="server" id="lunionid"></label>

                </div>--%>
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
            const REDIRECT_URI = 'http://zy4rr4.natappfree.cc/api/WxApp/GetUser';
            const SCOPE = 'snsapi_userinfo';


            //$("#btnCode").click(function () {
            //    var urls = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=' + APPID + '&redirect_uri=' + encodeURIComponent(REDIRECT_URI) + '&response_type=code&scope=' + SCOPE + '&state=STATE#wechat_redirect'
            //     window.location.href = urls; //"index.html"
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
            //})

        })

    </script>
</body>
</html>
