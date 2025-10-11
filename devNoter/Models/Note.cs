using System.ComponentModel.DataAnnotations;

namespace devNoter.Models
{
    public class Note
    {
        /// <summary>
        /// Note model representing a note in the application.
        /// </summary>


        // Primary Key
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }
        public string? CodeContext { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> ImagePaths { get; set; } = new List<string>();


        // Navigation property to LanguageFolder
        public int LanguageFolderId { get; set; }

        // Navigation property
        public LanguageFolder? LanguageFolder { get; set; }


        /// <summary>
        /// Constructor to initialize timestamps
        /// </summary>
        public Note()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public Note(int id, string title, string codeContext, string content, DateTime createdAt, DateTime updatedAt, int languageFolderId, LanguageFolder? languageFolder)
        {
            Id = id;
            Title = title;
            CodeContext = codeContext;
            Content = content; 
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            LanguageFolderId = languageFolderId;
            LanguageFolder = languageFolder;
        }


        // Override ToString for better debugging
        public override string ToString()
        {
            return $"Note(Id={Id}, Title={Title}, ,Code={CodeContext}, CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt}, LanguageFolderId={LanguageFolderId})";
        }
    }
}
