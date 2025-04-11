namespace StealAllTheCats.DAL.Models
{
    public class Cat
    {
        public int Id { get; set; }
        public String CatId{ get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String Image {  get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public ICollection<CatTag> CatTags { get; set; }

    }
}
