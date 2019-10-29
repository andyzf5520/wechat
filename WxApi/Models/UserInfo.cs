using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxApi.Models
{
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
}