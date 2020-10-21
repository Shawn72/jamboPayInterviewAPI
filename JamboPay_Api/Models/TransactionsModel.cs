namespace JamboPay_Api.Models
{
    public class TransactionsModel
    {
        //for getting
        public string supporter_id { get; set; }
        public string ambassador_id { get; set; }
        public decimal transaction_cost { get; set; }
        public decimal ambassador_commission { get; set; }

        //for fetching
        public string AmbassadorEmail { get; set; }
    }
}