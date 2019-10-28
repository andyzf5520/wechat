using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WxChat
{
    public partial class GetUser : System.Web.UI.Page
    {
        public string openid = "";

        //公众号信息部分
        public string appid = ConfigurationManager.AppSettings["AppId"];
 
        public string appsecret { get; protected set; }
        public string redirect_uri = HttpUtility.UrlEncode(HttpContext.Current.Request.Url.Host+ "/GetUser.aspx");
        public string scope = "snsapi_base";//以snsapi_userinfo为scope发起的网页授权，是用来获取用户的基本信息的。但这种授权需要用户手动同意，并且由于用户同意过，所以无须关注，就可在授权后获取该用户的基本信息。
        public string accesstoken = "";
        public int expires_in = 0;
        public string refresh_token = "";


        public static string access_token { get; set; }//票据所需的token

        public string nickname;
        public string sex;
        public string headimgurl;
        public string province;
        public string country;
        public string language;
        public string city;
        public string unionid;


        protected void Page_Load(object sender, EventArgs e)
        {
            //如果用户同意授权，页面将跳转至此处地址 redirect_uri/? code = CODE & state = STATE
            string code = Request.QueryString["code"];

            string state = Request.QueryString["state"];
            if (string.IsNullOrEmpty(code))
            {
                //如果code没获取成功，重新拉取一遍
                OpenAccess();

            }
            

            //GetWXConfig();
            WriteLogs("access_token", "access_token====", access_token);
            GetAuthriseAccess_Token(code);
 
        }

        /// <summary>
        /// 获取网页授权用户信息
        /// </summary>
        public void GetAuthriseAccess_Token(string code)
        {
            string UserInfo = "";
            appsecret = ConfigurationManager.AppSettings["Secret"];
            string html = string.Empty;
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + appsecret + "&code=" + code + "&grant_type=authorization_code";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            ioStream.Close();
            response.Close();
            JObject outputObj = JObject.Parse(html);
            
            var openidas= outputObj["openid"];
           



            if (openidas == null) {
               lerror.InnerText= outputObj.First.ToString();
                return;
            }
            openid = outputObj["openid"].ToString();
            accesstoken = outputObj["access_token"].ToString();
            expires_in = Convert.ToInt32(outputObj["expires_in"]);
            refresh_token = outputObj["refresh_token"].ToString();
            var timeout = DateTime.Now.AddSeconds(expires_in);
            // accesstoken = AccessTokenDAL.IsExistAccess_Token();
            WriteLogs("getInfo", "Useropen", html);
            WriteLogs("openid", "signature--openid", code.ToString() + "***" + openid);
            Session["openid"] = openid;
            Session["refresh_token"] = refresh_token;
            UserInfo = GetUserInfos();

            JObject outputObj1 = JObject.Parse(UserInfo);
            nickname = outputObj1["nickname"].ToString(); //昵称
            sex = outputObj1["sex"].ToString(); //性别什么的
            headimgurl = outputObj1["headimgurl"].ToString(); //头像url
            province = outputObj1["province"].ToString();
            country = outputObj1["country"].ToString(); 
            language = outputObj1["language"].ToString(); 
            city = outputObj1["city"].ToString();
            //unionid = outputObj1["unionid"].ToString();
            lnickname.InnerText = nickname;
            lsex.InnerText = sex;
            lheadimgurl.InnerText = headimgurl;
            lprovince.InnerText = province;
            lcountry.InnerText = country;
            llanguage.InnerText = language;
            lcity.InnerText = city;
            lopenid.InnerText = openid;
            //GetWXConfig();
            WriteLogs("UserInfos", "nickname", nickname.ToString() + "***" + openid);


        }

        //获取微信客户端需要的配置文件参数
        public string signature = "";
        public string timestamp = "";
        public string nonce = "";
        public string jsapi_ticket = "";

        public void GetWXConfig()
        {
            string url = Request.Url.ToString();
            jsapi_ticket = AccessTokenDAL.GetTicket();
            timestamp = GetTimeStamp();
            nonce = GenerateCheckCodeNum(16);
            string[] ArrTmp = { timestamp, nonce };
            //    WriteLogs("urllog", "url====", url);
            Array.Sort(ArrTmp);     //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            string wxconfigStr = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + nonce + "&timestamp=" + timestamp + "&url=" + url + "";
            tmpStr = AccessTokenDAL.Sha1(wxconfigStr);
            WriteLogs("wxconfigStr", "wxconfigStr====", wxconfigStr);
            signature = tmpStr;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }


        /// 
        /// 生成随机数字字符串
        /// 
        /// 待生成的位数
        /// 生成的数字字符串
        /// 
        private int rep = 0;
        private string GenerateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }

        public string GetUserInfos()
        {
            string UserURL = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accesstoken, openid);
            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(UserURL);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
            html = sr.ReadToEnd();
            WriteLogs("userlog", "userlog====", html);
            sr.Close();
            ioStream.Close();
            response.Close();
            return html;
        }



        public void OpenAccess()
        {
            //判断session不存在
            if (Session["openid"] == null)
            {
                //认证第一步：重定向跳转至认证网址
                string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&&response_type=code&scope=snsapi_userinfo&m=oauth2#wechat_redirect", appid, redirect_uri);
                Response.Redirect(url);
            }
            //判断session存在
            else
            {
                //跳转到前端页面.aspx
                Response.Redirect(Request.Url.ToString());
            }
        }


        /// <summary>
        /// 日志部分
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public static void WriteLogs(string fileName, string type, string content)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + fileName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                if (File.Exists(path))
                {
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + type + "-->" + content);
                    //  sw.WriteLine("----------------------------------------");
                    sw.Close();
                }
            }
        }
    }
}
 

         
 