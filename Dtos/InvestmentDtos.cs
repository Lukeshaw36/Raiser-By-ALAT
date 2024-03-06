namespace GROUP2.Dtos
{
    public class InvestmentDtos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PriceStock { get; set; }
        public string Description { get; set; }
        public string url { get; set; }
        public decimal MaximumCapital { get; set; }
        public long UserId { get; set; }
        // public List<UserDto> Users { get; set; }
    }
}
