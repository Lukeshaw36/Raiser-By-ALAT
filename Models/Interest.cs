namespace GROUP2.Models
{
    public class Interest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreatedBy { get; set; }
        public List<Investment> InvestmentInterests { get; set; }
    }
}
