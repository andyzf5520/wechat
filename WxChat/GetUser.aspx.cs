using Newtonsoft.Json;
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
        public string DevHost = ConfigurationManager.AppSettings["DevHost"];
        public string Host = ConfigurationManager.AppSettings["Host"];

        public string appsecret { get; protected set; }
        public string redirect_uri = HttpUtility.UrlEncode(HttpContext.Current.Request.Url.Host + "/GetUser.aspx");
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
       
            GetAuthriseAccess_Token(code);

        }

        /// <summary>
        /// 获取网页授权用户信息
        /// </summary>
        public void GetAuthriseAccess_Token(string code)
        {

            try
            {
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
                ResultData result = new ResultData();
                result = JsonConvert.DeserializeObject<ResultData>(html);
                //JObject outputObj = JObject.Parse(html);
                if (result != null && !string.IsNullOrEmpty(result.openid) && !string.IsNullOrEmpty(result.access_token))
                {

                    var openid = result.openid;

                }
                else
                {
                    //lerror.InnerText= outputObj.First.ToString();
                    WriteLogs("error", result.errmsg);
                    return;
                }



                
                openid = result.openid;
                accesstoken = result.access_token;
                expires_in = Convert.ToInt32(result.expires_in);
                refresh_token = result.refresh_token;
                var timeout = DateTime.Now.AddSeconds(expires_in);
                // accesstoken = AccessTokenDAL.IsExistAccess_Token();
                WriteLogs("tokeninfo", html);
                WriteLogs("openid", openid);
                Session["openid"] = openid;
                Session["refresh_token"] = refresh_token;
                Response.Cookies.Add(new HttpCookie("opneid", openid));
                Response.Cookies.Add(new HttpCookie("access_token", accesstoken));
                Response.Cookies.Add(new HttpCookie("expires_in", expires_in.ToString()));
                string UserInfoStr = GetUserInfos();

                var user = JsonConvert.DeserializeObject<UserInfo>(UserInfoStr);

                //JObject result = JObject.Parse(UserInfo);
                if (user != null)
                {
                    nickname = user.nickname; //昵称
                    headimgurl = user.headimgurl; //头像url
                    unionid = user.unionid;
                }
                else
                {

                }
                //查询用户是否首次授权
                if (user.unionid != null) {

                }
                var wxUser = GetWxUserById(user.unionid);
                if (wxUser != null && !string.IsNullOrEmpty(wxUser.unionId))
                {
                    //不是首次授权  校验头像是否失效 
                    //if (wxUser.headimgurl != UserInfo.headimgurl || wxUser.nickname != UserInfo.nickname)
                    //{
                    //    //更新头像和名称 业务逻辑代码 之后跳转小程序或者
                    //}

                    Response.Redirect("Button.aspx");
                }
                else
                {
                    //GetWXConfig();
                    WriteLogs("nickname", nickname.ToString());
                    WriteLogs("openid", openid);
                    //首次授权 保存用户基本信息 业务逻辑代码 此处重定向返回注册页面
                    Response.Redirect("Home.aspx");


                }

            }
            catch (Exception e)
            {
                WriteLogs("error", e.Message);
                throw;
            }

            


        }

        public ManageUser GetWxUserById(string unionid)
        {
            try
            {
                //string url = "https://circulation.bxsuyuan.com/api/ManageUserApp/GetUserInfoByUnionId?unionId=" + unionid;
                string url = Host+"/api/ManageUserApp/GetUserInfoByUnionId?unionId=" + unionid;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream ioStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
                string html = sr.ReadToEnd();
                sr.Close();
                ioStream.Close();
                response.Close();

                var result = JsonConvert.DeserializeObject<ManageUser>(html);
                return result;
            }
            catch (Exception e)
            {
                WriteLogs("error", e.Message);
                throw;
            }

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
            WriteLogs("wxconfigStr", wxconfigStr);
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
            try
            {
                string UserURL = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accesstoken, openid);
                string html = string.Empty;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(UserURL);
                request.Method = "GET";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream ioStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
                html = sr.ReadToEnd();
                WriteLogs("userinfos", html);
                sr.Close();
                ioStream.Close();
                response.Close();
                return html;
            }
            catch (Exception e)
            {

                throw;
            }
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
        public static void WriteLogs(string info, string content)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "/wxlogs";
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
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") +"   " +info + "-->" + content);
                    //  sw.WriteLine("----------------------------------------");
                    sw.Close();
                }
            }
        }
    }
    public class ResultData
    {

        public string access_token { get; set; }
        public string openid { get; set; }
        //[JsonProperty(".expires_in")]
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public UserInfo userInfo { get; set; }
    }
    public class UserInfo
    {

        public string nickname { get; set; }
        public string sex { get; set; }
        public string headimgurl { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string unionid { get; set; }
        public string openid { get; set; }

    }
    public class ManageUser
    {
        public string manageUserId { get; set; }
        public string manageUserName { get; set; }
        public string manageNodeId { get; set; }
        public string idno { get; set; }
        public string tel { get; set; }
        public string icNo { get; set; }
        public string userPhotoUrl { get; set; }
        public string businessNo { get; set; }
        public string businessScope { get; set; }
        public int userState { get; set; }
        public int isDelete { get; set; }
        public string operatorId { get; set; }
        public string lastOperatorId { get; set; }
        public Nullable<System.DateTime> operatorDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public string remark { get; set; }
        public string managePerson { get; set; }
        public string manageAddress { get; set; }
        public string manageBusinessName { get; set; }
        public string ownerMarket { get; set; }
        public string manageBusinessType { get; set; }
        public string unionId { get; set; }
    }
}



