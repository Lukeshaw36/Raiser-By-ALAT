namespace GROUP2.Dtos
{
    public class InterestDtos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreatedBy { get; set; }
        public List<InvestmentDtos> InvestmentInterests { get; set; }
    }
}
