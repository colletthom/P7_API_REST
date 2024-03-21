using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class Bid
    {
        // TODO: Map columns in data table BIDLIST with corresponding fields
        public int BidId { get; set; }

        [Required(ErrorMessage = "Missing Account")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Missing BidType")]
        public string BidType { get; set; }

        public double? BidQuantity { get; set; }

        public double? AskQuantity { get; set; }

        public double? Bid2 { get; set; }

        public double? Ask { get; set; }

        [Required(ErrorMessage = "Missing Benchmark")]
        public string Benchmark { get; set; }

        public DateTime? BidListDate { get; set; }

        [Required(ErrorMessage = "Missing Commentary")]
        public string Commentary { get; set; }

        [Required(ErrorMessage = "Missing BidSecurity")]
        public string BidSecurity { get; set; }

        [Required(ErrorMessage = "Missing BidStatus")]
        public string BidStatus { get; set; }

        [Required(ErrorMessage = "Missing Trader")]
        public string Trader { get; set; }

        [Required(ErrorMessage = "Missing Book")]
        public string Book { get; set; }

        [Required(ErrorMessage = "Missing CreationName")]
        public string CreationName { get; set; }

        public DateTime? CreationDate { get; set; }

        [Required(ErrorMessage = "Missing RevisionName")]
        public string RevisionName { get; set; }

        public DateTime? RevisionDate { get; set; }

        [Required(ErrorMessage = "Missing DealName")]
        public string DealName { get; set; }

        [Required(ErrorMessage = "Missing DealType")]
        public string DealType { get; set; }

        [Required(ErrorMessage = "Missing SourceListId")]
        public string SourceListId { get; set; }

        [Required(ErrorMessage = "Missing Side")]
        public string Side { get; set; }
    }
}