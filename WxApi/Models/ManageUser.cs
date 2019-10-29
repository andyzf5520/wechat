using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxApi.Models
{
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