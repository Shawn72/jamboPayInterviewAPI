using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamboPay_Api.Models
{
    public class ServiceModel
    {
        //for posting
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public decimal ServiceCommisionPercent { get; set; }

        //for getting
        public string service_name { get; set; }
        public string service_code { get; set; }
        public decimal service_commission_percent { get; set; }

    }
}