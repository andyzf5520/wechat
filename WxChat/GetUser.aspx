<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUser.aspx.cs" Inherits="WxChat.GetUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="title">
                用户信息获取：
            </div>
            <div>
                错误信息：
                <label runat="server" id="lerror"></label>
            </div>
            <hr style="border: 1px solid gray;" />
          <div class="content" style="width:100%;text-align:center;background-color:aliceblue;color:blueviolet">
                <div>
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
               
            </div>



          </div>



        </div>
    </form>
</body>
</html>
