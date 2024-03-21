using System;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class CurvePoint
    {
        // TODO: Map columns in data table CURVEPOINT with corresponding fields
        public int Id { get; set; }

        public byte? CurveId { get; set; }

        [DataType(DataType.Date, ErrorMessage = "CreationDate must be a valid date")]
        public DateTime? AsOfDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The Term field must be a positive number")]
        public double? Term { get; set; }

        [Range(double.MinValue, double.MaxValue, ErrorMessage = "The CurvePointValue field must be a valid number")]
        public double? CurvePointValue { get; set; }

        [DataType(DataType.Date, ErrorMessage = "CreationDate must be a valid date")]
        public DateTime? CreationDate { get; set; }
    }
}