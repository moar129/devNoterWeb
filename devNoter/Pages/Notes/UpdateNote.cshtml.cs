using devNoter.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace devNoter.Pages.Notes
{
    // Razor Page Model for updating an existing note
    public class UpdateNoteModel : PageModel
    {
        private readonly NoteService _noteService; // Service for note CRUD operations
        private readonly IWebHostEnvironment _environment; // Provides web root path for file storage

        // Constructor: inject dependencies
        public UpdateNoteModel(NoteService noteService, IWebHostEnvironment environment)
        {
            _noteService = noteService;
            _environment = environment;
        }

        // ------------------------
        // Properties bound to form
        // ------------------------
        [BindProperty]
        public Models.Note Note { get; set; } // Holds the note being edited

        [BindProperty]
        public string? Code { get; set; } // Optional code snippet

        [BindProperty]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>(); // New images uploaded

        [BindProperty]
        public List<string> RemoveImages { get; set; } = new List<string>(); // Images marked for removal

        // ------------------------
        // GET handler
        // ------------------------
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch the note by ID from the database
            Note = await _noteService.GetNoteByIdAsync(id);

            // Redirect to NotFound page if the note does not exist
            if (Note == null)
                return RedirectToPage("/NotFound");

            return Page(); // Render the page with note data
        }

        // ------------------------
        // POST handler
        // ------------------------
        public async Task<IActionResult> OnPostAsync()
        {
            // ------------------------
            // Update code snippet if provided
            // ------------------------
            if (!string.IsNullOrEmpty(Code))
                Note.CodeContext = Code;

            // ------------------------
            // Validate form data
            // ------------------------
            if (!ModelState.IsValid)
            {
                // Print all validation errors to console
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState error: {error.ErrorMessage}");
                }
                return Page(); // Return page if invalid
            }

            // ------------------------
            // Fetch the existing note from database
            // ------------------------
            var noteToUpdate = await _noteService.GetNoteByIdAsync(Note.Id);
            if (noteToUpdate == null)
                return RedirectToPage("/NotFound");

            // ------------------------
            // Update main note fields
            // ------------------------
            noteToUpdate.Title = Note.Title;
            noteToUpdate.Content = Note.Content;
            noteToUpdate.CodeContext = Note.CodeContext;
            noteToUpdate.UpdatedAt = DateTime.Now; // Update timestamp
            noteToUpdate.CreatedAt = Note.CreatedAt; // Preserve original creation date

            // ------------------------
            // Ensure ImagePaths is initialized and contains no nulls
            // ------------------------
            if (noteToUpdate.ImagePaths == null)
                noteToUpdate.ImagePaths = new List<string>();
            noteToUpdate.ImagePaths = noteToUpdate.ImagePaths.Where(x => !string.IsNullOrEmpty(x)).ToList();

            // ------------------------
            // Handle removal of selected images
            // ------------------------
            if (RemoveImages != null && RemoveImages.Count > 0)
            {
                foreach (var imgPath in RemoveImages.ToList())
                {
                    if (string.IsNullOrEmpty(imgPath))
                        continue; // Skip null or empty entries

                    // Extract file name from path
                    var fileName = Path.GetFileName(imgPath);

                    // Find matching image by filename
                    var match = noteToUpdate.ImagePaths.FirstOrDefault(x => x.EndsWith(fileName));

                    if (match != null)
                    {
                        noteToUpdate.ImagePaths.Remove(match); // Remove from note

                        // Construct full file path on server
                        var filePath = Path.Combine(_environment.WebRootPath, "images", Path.GetFileName(match));

                        // Attempt to delete file safely
                        try
                        {
                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Could not delete file {filePath}: {ex.Message}");
                            // Do not crash the application if deletion fails
                        }
                    }
                }
            }

            // ------------------------
            // Handle new image uploads
            // ------------------------
            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" }; // Only allow these types
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

                // Ensure the uploads folder exists
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                foreach (var file in ImageFiles)
                {
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                    // Skip file if extension is not permitted
                    if (!permittedExtensions.Contains(ext))
                        continue;

                    // Generate unique filename to avoid collisions
                    var uniqueFileName = Guid.NewGuid() + ext;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file to server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Add path to note
                    noteToUpdate.ImagePaths.Add("/images/" + uniqueFileName);
                }
            }

            // ------------------------
            // Update note in database
            // ------------------------
            await _noteService.UpdateNoteAsync(noteToUpdate);

            // Redirect to page showing all notes in the same language folder
            return RedirectToPage("./GetAllNotes", new { id = noteToUpdate.LanguageFolderId });
        }
    }
}
