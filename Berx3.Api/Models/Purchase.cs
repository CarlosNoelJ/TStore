namespace Berx3.Api.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int TShirtId { get; set; }
        public TShirt? TShirt { get; set; }

        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    }
}
