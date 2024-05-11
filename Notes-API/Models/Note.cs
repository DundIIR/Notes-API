namespace Notes_API.Models
{
    public class Note
    {

        public Note(string title, string description)
        {
            Title = title;
            Description = description;
            CreationDate = DateTime.UtcNow;
        }


        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }  
    }
}
