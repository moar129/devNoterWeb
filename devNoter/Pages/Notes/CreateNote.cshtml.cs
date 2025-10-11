using Azure.Core.Cryptography;
using devNoter.Models;
using devNoter.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace devNoter.Pages.Notes
{
    public class CreateNoteModel : PageModel
    {
        private NoteService _noteService;
        private readonly IWebHostEnvironment _environment;

        public CreateNoteModel(NoteService noteService, IWebHostEnvironment environment)
        {
            _noteService = noteService;
            _environment = environment;
        }
        [BindProperty]
        public Models.Note Note { get; set; }
        [BindProperty]
        public int LanguageFolderId { get; set; }

        /// <summary>
        /// Property to hold the code context for the note.
        /// </summary>
        [BindProperty]
        public string? Code { get; set; }

        /// <summary>
        /// Property to hold the uploaded image file.
        /// </summary>
        [BindProperty]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
        // Property to hold any messages for the user.
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            LanguageFolderId = id;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Note.CodeContext = Code;
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState error: {error.ErrorMessage}");
                }
                return Page();
            }

            Note.LanguageFolderId = LanguageFolderId;
            Note.LanguageFolder = null; // don’t attach a stub object

            // Check for valid file and save it
            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                Note.ImagePaths = new List<string>(); // Gem stier til alle billeder

                foreach (var file in ImageFiles)
                {
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!permittedExtensions.Contains(ext))
                        continue; // spring fil over hvis den ikke er tilladt

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    Note.ImagePaths.Add("/images/" + uniqueFileName);
                    //Console.WriteLine("Billede gemt i: " + filePath);
                }
            }


            await _noteService.AddNoteAsync(Note);
            return RedirectToPage("./GetAllNotes", new { id = LanguageFolderId });

        }
    }
}
