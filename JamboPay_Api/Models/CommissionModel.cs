namespace JamboPay_Api.Models
{
    public class CommissionModel
    {
        public string SupporterEmail { get; set; }
        public string AmbassadorEmail { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceTypeCommision { get; set; }
        public decimal ServiceFee { get; set; }
    }
}