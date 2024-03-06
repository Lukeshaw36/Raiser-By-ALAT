namespace GROUP2.Models
{
    public class Investment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PriceStock { get; set; }
        public string Description { get; set; }
        public string url { get; set; }
        public decimal MaximumCapital { get; set; }
        public long UserId { get; set; }
       // public List<User> Users { get; set; }
        public long InterestId { get; set; }

        // Navigation property to Interest, assuming a one-to-one relationship
       // public Interest Interest { get; set; }
    }
    }

