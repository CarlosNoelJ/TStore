namespace Berx3.Api.Models
{
    public class TShirt
    {
        public int Id { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
