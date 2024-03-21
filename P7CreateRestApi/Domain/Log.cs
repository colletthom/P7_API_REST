namespace Dot.Net.WebApi.Domain
{
    public class Log
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public DateTime? LogDateTime { get; set; }

        //1 for Create, 2 Read All, 3 for Read by Id, 4 for Update by ID, 5 for Delete by ID
        public int CRUD {  get; set; }

        //1 for Bid, 2 for CurvePoint, 3 for Rating, 4 for RuleName, 5 for Trade, 6 for User
        public int Entity {  get; set; }
        public string? LogDescription { get; set; }
    }
}
