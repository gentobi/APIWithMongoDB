namespace Entities.Entities
{
	public class Book : BaseEntity
	{
		public string Name { get; set; }
		public string Author { get; set; }
		public decimal Price { get; set; }
		public string Category { get; set; }
	}
}
