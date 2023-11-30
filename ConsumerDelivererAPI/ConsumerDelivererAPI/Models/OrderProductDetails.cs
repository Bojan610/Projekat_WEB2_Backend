namespace ConsumerDelivererAPI.Models
{
    public class OrderProductDetails
    {
        public int Id { get; set; }
        public string ConsumerId { get; set; }
        public string DelivererId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public int Quantity { get; set; }
    }
}
