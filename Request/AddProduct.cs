namespace Auth.Request
{
    public class AddProduct
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Price { get; set; }
    }
}
