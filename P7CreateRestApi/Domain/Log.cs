namespace Dot.Net.WebApi.Domain
{
    public class Log
    {
        int Id { get; set; }
        public DateTime? LogDateTime { get; set; }
        public string LogDescription { get; set; }
        public int UserID { get; set; }
        public int CRUD {  get; set; }
        public int Entity {  get; set; }

    }
}
