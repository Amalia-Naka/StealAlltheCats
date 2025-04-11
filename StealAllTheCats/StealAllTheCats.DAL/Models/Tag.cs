namespace StealAllTheCats.DAL.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public ICollection<CatTag> CatTags { get; set; }
    }
}
