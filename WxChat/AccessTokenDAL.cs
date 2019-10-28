using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace WxChat
{
    public class AccessTokenDAL
    {
        /// <summary>
        /// 根据当前日期 判断Access_Token 是否超期  如果超期返回新的Access_Token   否则返回之前的Access_Token
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string IsExistAccess_Token()
        {
            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            string filepath = HttpContext.Current.Server.MapPath("./") + "\\Access_token.xml";

            StreamReader str = new StreamReader(filepath, Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);

            //TimeSpan st1 = new TimeSpan(YouXRQ.Ticks); //最后刷新的时间
            //TimeSpan st2 = new TimeSpan(DateTime.Now.Ticks); //当前时间
            //TimeSpan st = st2 - st1; //两者相差时间
            if (DateTime.Now > YouXRQ)
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_token();
                if (mode == null) {
                    return null;
                }
                xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
                _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
                xml.Save(filepath);
                Token = mode.access_token;


            }
            GetUser.access_token = Token;
            return Token;
        }


        /// <summary>
        /// 获取Access_token
        /// </summary>
        /// <returns></returns>
        private static Access_token GetAccess_token()
        {
            try
            {
                string appid = ConfigurationManager.AppSettings["AppId"];//微信公众号appid
                string secret = ConfigurationManager.AppSettings["Secret"]; //微信公众号appsecret
                string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
                Access_token mode = new Access_token();

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);  //用GET形式请求指定的地址 
                req.Method = "GET";

                using (WebResponse wr = req.GetResponse())
                {
                    //HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();  
                    StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();

                    //在这里对Access_token 赋值  
                    Access_token token = new Access_token();
                    token = JsonConvert.DeserializeObject<Access_token>(content);
                    mode.access_token = token.access_token;
                    mode.expires_in = token.expires_in;
                }
                return mode;
            }
            catch (Exception e)
            {
                WriteLogs("获取Access_token", "GetAccess_token", e.Message);
                throw;
            }
        }
        /// <summary>
        /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
        public static string Sha1(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString();
        }
        /// <summary>
        /// 获取js票据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetTicket()
        {
            try
            {
                string token = IsExistAccess_Token();
                string jsapi_ticket = "";//唯一凭证
                string jsurl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + token + "&type=jsapi";
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(jsurl);  //用GET形式请求指定的地址 
                req.Method = "GET";

                using (WebResponse wr = req.GetResponse())
                {
                    //HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();  
                    StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();
                    //jsapi_ticket = content;
                    reader.Close();
                    reader.Dispose();

                    JObject outputObj = JObject.Parse(content);
                    jsapi_ticket = outputObj["ticket"].ToString();
                     
                }
                WriteLogs("piaoju", "ticket", jsapi_ticket);

                return jsapi_ticket;
            }
            catch (Exception e)
            {
                WriteLogs("piaoju", "GetTicket", e.Message);
                throw;
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