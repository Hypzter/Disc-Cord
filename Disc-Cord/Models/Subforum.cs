namespace Disc_Cord.Models
{
    public class Subforum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Forum? Forum { get; set; }
        public int ForumId { get; set; }
        public DateTime Date { get; set; }
    }
}
