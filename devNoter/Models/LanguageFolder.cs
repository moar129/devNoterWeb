using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace devNoter.Models
{
    public class LanguageFolder
    {
        /// <summary>
        /// LanguageFolder model representing a folder containing notes for a specific programming language.
        /// </summary>


        // Primary Key
        [Key]
        public int Id { get; set; }
        // Folder name
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Foreign key to the ApplicationUser who owns this LanguageFolder.
        /// </summary>
        [Required]
        public string UserId { get; set; }


        public ApplicationUser? User { get; set; }

        // Self-referencing relationship (for nested folders)
        public int? ParentFolderId { get; set; }

        [ForeignKey("ParentFolderId")]
        public LanguageFolder? ParentFolder { get; set; }

        public List<LanguageFolder> ChildFolders { get; set; } = new();

        // One-to-many relationship with Notes
        public List<Note> Notes { get; set; } = new();

        /// <summary>
        /// Constructor to initialize the Folders in EF
        /// </summary>
        public LanguageFolder()
        {
        }
        /// <summary>
        /// Constructor to initialize the Folders with parameters 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <param name="parentFolderId"></param>
        public LanguageFolder(int id, string name, string userId, ApplicationUser? user, int? parentFolderId = null)
        {
            Id = id;
            Name = name;
            UserId = userId;
            User = user;
            ParentFolderId = parentFolderId;
        }

        // Override ToString for better debugging
        public override string ToString()
        {
            return $"LanguageFolder(Id={Id}, Name={Name}, ParentFolderId={ParentFolderId})";
        }

    }
}
