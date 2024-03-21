using System;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class Trade
    {
        public int TradeId { get; set; }

        [Required(ErrorMessage = "Missing Account")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Missing AccountType")]
        public string AccountType { get; set; }

        public double? BuyQuantity { get; set; }

        public double? SellQuantity { get; set; }

        public double? BuyPrice { get; set; }

        public double? SellPrice { get; set; }

        public DateTime? TradeDate { get; set; }

        [Required(ErrorMessage = "Missing TradeSecurity")]
        public string TradeSecurity { get; set; }

        [Required(ErrorMessage = "Missing TradeStatus")]
        public string TradeStatus { get; set; }

        [Required(ErrorMessage = "Missing Trader")]
        public string Trader { get; set; }

        [Required(ErrorMessage = "Missing Benchmark")]
        public string Benchmark { get; set; }

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