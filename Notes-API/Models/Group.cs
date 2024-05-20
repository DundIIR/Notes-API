namespace Notes_API.Models
{
    public class Group
    {
        public Group(string name)
        {
            Name = name;
            CreationDate = DateTime.UtcNow;
            Notes = new List<Note>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<Note> Notes { get; set; }
    }
}
