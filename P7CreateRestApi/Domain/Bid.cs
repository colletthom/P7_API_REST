namespace Dot.Net.WebApi.Domain
{
    public class Bid
    {
        // TODO: Map columns in data table BIDLIST with corresponding fields
        public int BidId { get; set; }
        public string Account { get; set; }
        public string BidType { get; set; }
        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double? Bid2 { get; set; }
        public double? Ask { get; set; }
        public string Benchmark { get; set; }
        public DateTime? BidListDate { get; set; }
        public string Commentary { get; set; }
        public string BidSecurity { get; set; }
        public string BidStatus { get; set; }
        public string Trader { get; set; }
        public string Book { get; set; }
        public string CreationName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string SourceListId { get; set; }
        public string Side { get; set; }
    }
}