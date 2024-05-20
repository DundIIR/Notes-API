namespace Notes_API.Models
{
    public class Note
    {
        public Note(string title, string description, Guid groupId)
        {
            Title = title;
            Description = description;
            CreationDate = DateTime.UtcNow;
            GroupId = groupId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
