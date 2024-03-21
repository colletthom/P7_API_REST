using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class RuleName
    {
        // TODO: Map columns in data table RULENAME with corresponding fields
        public int Id { get; set; }

        [Required(ErrorMessage = "Missing Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Missing Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Missing Json")]
        public string Json { get; set; }

        [Required(ErrorMessage = "Missing Template")]
        public string Template { get; set; }

        [Required(ErrorMessage = "Missing SqlStr")]
        public string SqlStr { get; set; }

        [Required(ErrorMessage = "Missing SqlPart")]
        public string SqlPart { get; set; }
    }
}