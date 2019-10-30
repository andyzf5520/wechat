using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WxApi.Models;

namespace WxApi.Controllers
{
    public class GetUserController : Controller
    {
        public string appid = ConfigurationManager.AppSettings["AppId"].ToString();
        public string secret = ConfigurationManager.AppSettings["Secret"].ToString();
        public string Host = ConfigurationManager.AppSettings["Host"].ToString();
        public string DevHost = ConfigurationManager.AppSettings["DevHost"].ToString();
        public string access_token { get; set; }
        public string openid { get; set; }
        public string unionid { get; set; }
        public string expires_in { get; set; }
        

       
        //public string redirect_uri = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.Host+ "/GetUser.aspx");


        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 注册跳转页获取unionId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string code, string state)
        {
            try
            {
                ViewBag.visibility = "hidden";
               
                string host = HttpUtility.UrlDecode(HttpContext.Request.Url.Host);
                //如果用户同意授权，页面将跳转至此处地址 redirect_uri/? code = CODE & state = STATE

                ResultData result = new ResultData();
                UserInfo user = new UserInfo();
                if (string.IsNullOrEmpty(code))
                {
                    OpenAccess();//如果code没获取成功，重新拉取一遍
                }
                var resData = await GetWxOpenId(code);
                if (resData != null && !string.IsNullOrEmpty(resData.access_token))
                {
                    result = resData;
                    access_token = result.access_token;
                    openid = result.openid;
                    expires_in = result.expires_in;

                    //添加缓存
                    Response.Cookies.Add(new HttpCookie("openid", openid));
                    Response.Cookies.Add(new HttpCookie("access_token", access_token));
                    Response.Cookies.Add(new HttpCookie("expires_in", expires_in));
                    // 获取unionId及用户信息
                    user = await GetUserInfo(access_token, openid);

                    if (user != null && !string.IsNullOrEmpty(user.openid))
                    {
                        unionid = user.unionid;
                        var mauser = GetWxUserById(user.openid);
                        Session.Add("openid", openid);
                        if (mauser != null)
                        {
                            // 已注册
                            return RedirectToRoute(new { controller = "Home", action = "Index" });
                        }
                        else
                        {
                            // 未注册 跳转注册页面
                            Response.Cookies.Add(new HttpCookie("unionid", unionid));
                            return RedirectToRoute(new { controller = "GetUser", action = "Regist" });
                        }
                        //if (!string.IsNullOrEmpty(unionid)) {


                        //}
                        //else
                        //{
                        //    WriteLogs("unionId", "unionId为空");
                        //}
                        //在这里做一下跳转  不然会无限循环授权
                        return RedirectToRoute(new { controller = "Home", action = "Index" });


                    }
                    

                }
                else
                {
                    // token获取失败 不做跳转 展示下
                    ViewBag.visibility = "visible";
                    ViewBag.Error = "授权失败";
                    WriteLogs("token获取失败", resData.errmsg);
                    //return RedirectToRoute(new { controller = "Home", action = "Index" });

                }
                return View();


            }
            catch (Exception e)
            {
                ViewBag.visibility = "visible";
                ViewBag.Error = e.Message;
                WriteLogs("error", e.Message);
                throw e;
            }


        }

        /// <summary>
        ///  注册门店页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]  // 因为往别的数据库更新 不需要post视图
        public ActionResult Regist()
        {
            var uid = unionid;
            return View();
        }
        //[HttpPost]  // 因为往别的数据库更新 不需要post视图
        //public ActionResult Regist(ManageUser user)
        //{

        //    return View();
        //}


        public async Task<ResultData> GetWxOpenId(string code)
        {

            try
            {
                HttpClient client = new HttpClient();
                var Url = Request.Url;
                string urlToken = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";
                string jsonStr = await client.GetAsync(urlToken).Result.Content.ReadAsStringAsync();
                ResultData result = new ResultData();

                result = JsonConvert.DeserializeObject<ResultData>(jsonStr);
                WriteLogs("jsonStr", jsonStr);


                return result;
            }
            catch (Exception e)
            {
                WriteLogs("error", e.Message);
                throw;
            }
        }
        [HttpGet]
        public async Task<UserInfo> GetUserInfo(string access_token, string openid)
        {
            try
            {
                string urlUser = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
                HttpClient http = new HttpClient();
                var str = await http.GetAsync(urlUser).Result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserInfo>(str);
                WriteLogs("userInfo", str);
                return user;
            }
            catch (Exception e)
            {

                WriteLogs("error", e.Message);
                throw;
            }

        }
        public void OpenAccess()
        {
            string redirect_uri = HttpUtility.UrlDecode(Request.Url.Host + "/GetUser/Index");
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
                Response.Redirect(Request.Url.AbsolutePath);
            }
        }
        public ManageUser GetWxUserById(string unionid)
        {
            try
            {
                //string url = "https://circulation.bxsuyuan.com/api/ManageUserApp/GetUserInfoByUnionId?unionId=" + unionid;
                string url = DevHost + "/api/ManageUserApp/GetUserInfoByUnionId?unionId=" + unionid;
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

        private static string logFile
        {
            get
            {
                return System.Web.HttpContext.Current.Server.MapPath("~/log/" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt");
            }
        }

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
                if (!System.IO.File.Exists(path))
                {

                    FileStream fs = System.IO.File.Create(path);
                    fs.Close();

                }
                if (System.IO.File.Exists(path))
                {
                    using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "   " + info + "-->" + content);
                        //  sw.WriteLine("----------------------------------------");

                    }

                }
            }
        }



        #region JS服务器接口信息配置地址

        private string Token = "Token";
        [HttpGet]
        public string Validate()
        {
            string echoStr = Request.QueryString["echoStr"].ToString();

            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    return echoStr;
                }
               
            } return null;
           
        }
      

        /// <summary>    
        /// 微信请求的地址      
        /// </summary>      
        /// <param name="echostr">随机字符串，用于返回微信</param>      
        /// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。 </param>     
        /// <param name="timestamp">时间戳 </param>    
        /// <param name="nonce">随机数</param>      
        /// <returns></returns>   
        public bool CheckSignature()
        {

            string signature = Request.QueryString["signature"].ToString();
            string timestamp = Request.QueryString["timestamp"].ToString();
            string nonce = Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }


        } 
        #endregion

    }


}