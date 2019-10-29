using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxApi.Models
{
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
}