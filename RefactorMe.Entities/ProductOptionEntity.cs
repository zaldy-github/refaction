namespace RefactorMe.Entities
{
    public class ProductOptionEntity
    {
        public System.Guid Id { get; set; }
        public System.Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
