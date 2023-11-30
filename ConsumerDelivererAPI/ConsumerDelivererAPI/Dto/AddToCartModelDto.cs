namespace ConsumerDelivererAPI.Dto
{
    public class AddToCartModelDto
    {
        public int ProductId { get; set; }
        public string email { get; set; }
        public int Quantity { get; set; }
    }
}
