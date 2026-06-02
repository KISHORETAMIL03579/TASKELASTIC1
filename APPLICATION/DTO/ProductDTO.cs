namespace APPLICATION.DTO
{
    public class ProductDTO 
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Stock { get; set; }
    }

    public class ProductPatchDTO
    {
        public string? Name { get; set; }
        public double? Price { get; set; }
        public int? Stock { get; set; }
    }

}
