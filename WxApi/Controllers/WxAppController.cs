using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WxApi.Models;

namespace WxApi.Controllers
{
    public class WxAppController : ApiController
    {

        public string appid = ConfigurationManager.AppSettings["AppId"].ToString();
        public string secret = ConfigurationManager.AppSettings["Secret"].ToString();
        public string access_token { get; set; }
        public string openid { get; set; }
        public string unionid { get; set; }
        //public string redirect_uri = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.Host+ "/GetUser.aspx");
        public string redirect_uri = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.Host + "/api/WxApp/GetUser");
        public string host = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.Host);
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        public async Task<IHttpActionResult> GetUser(string code, string state)
        {
            try
            {
                //如果用户同意授权，页面将跳转至此处地址 redirect_uri/? code = CODE & state = STATE
                //string code = Request.Content.Headers.GetValues("code").FirstOrDefault();       
                //string state = Request.Content.Headers.GetValues("state").FirstOrDefault();
                var list = Request.GetQueryNameValuePairs().ToList();
                var co = list.FirstOrDefault(e=>e.Key=="code");
                if (string.IsNullOrEmpty(code))
                {
                    //如果code没获取成功，重新拉取一遍
                    OpenAccess();

                }

                HttpClient client = new HttpClient();
                var Url = Request.RequestUri;
                string urlToken = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";
                string jsonStr = await client.GetAsync(urlToken).Result.Content.ReadAsStringAsync();
                ResultData result = new ResultData();
              
                result = JsonConvert.DeserializeObject<ResultData>(jsonStr);
                if (result.access_token == null)
                {

                    return  null;
                }
                access_token = result.access_token;
                openid = result.openid;

                //HttpContext.Current.Session.Add("access_token", access_token);
                HttpContext.Current.Session.Add("openid", openid);
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("openid", openid));
                UserInfo user = new UserInfo();
                user = await GetUserInfo(access_token, openid);
                result.userInfo = user;
                WriteLogs("UserInfos", "nickname", result.userInfo.nickname.ToString() + "***" + openid);
                var response = new HttpResponseMessage();
                string resulstr = JsonConvert.SerializeObject(result);

                //return this.Redirect("Home/Index");
                //var path = AppDomain.CurrentDomain.BaseDirectory+"/wxview/wxlogin.html";
                // 这样跳转会死循环
                //response.Content = new StringContent($"<script>alert('xxx');window.location.href=\"http://snc5uk.natappfree.cc/wxview/wxlogin.html\";  window.close();</script>", System.Text.Encoding.Default);
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                //return response;
                return Ok(result);


            }
            catch (Exception e)
            {

                throw  e;
            }


        }
        [HttpGet]
        public async Task<UserInfo> GetUserInfo(string access_token, string openid)
        {
            string urlUser = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
            HttpClient http = new HttpClient();
            var str = await http.GetAsync(urlUser).Result.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserInfo>(str);
            return user;

        }
        public void OpenAccess()
        {

            //判断session不存在
            if (HttpContext.Current.Session["openid"] == null)
            {
                //认证第一步：重定向跳转至认证网址
                string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&&response_type=code&scope=snsapi_userinfo&m=oauth2#wechat_redirect", appid, redirect_uri);
                HttpContext.Current.Response.Redirect(url);
            }
            //判断session存在
            else
            {
                //跳转到前端页面.aspx
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsolutePath);
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
                path = AppDomain.CurrentDomain.BaseDirectory + "/wxlogs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
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
            else
            {
                Directory.CreateDirectory(path);
            }
        }
        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

   
}
