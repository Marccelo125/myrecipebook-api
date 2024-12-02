namespace MyRecipeBook.Domain.Entities
{
    public class EntityBase
    {
        public long Id { get; set; }

        public DateTime CreationUtcDate { get; set; } = DateTime.UtcNow;
    }
}
