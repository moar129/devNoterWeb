using devNoter.Models;
using devNoter.Pages.LanguageFolder;
using devNoter.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static NuGet.Packaging.PackagingConstants;

namespace devNoter.Pages.Notes
{
    [Authorize]
    public class DeleteNoteModel : PageModel
    {
        private NoteService _noteService;
        private readonly IWebHostEnvironment _environment;
        public DeleteNoteModel(NoteService noteService, IWebHostEnvironment environment)
        {
            _noteService = noteService;
            _environment = environment;
        }
        [BindProperty]
        public Models.Note Note { get; set; }

        [BindProperty]
        public int LanguageFolderId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            Note = await _noteService.GetNoteByIdAsync(id);
            if (Note == null)
            {
                return RedirectToPage("/NotFound"); // NotFound er ikke defineret endnu
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var noteToDelete = await _noteService.GetNoteByIdAsync(Note.Id);
            if (noteToDelete == null)
            {
                return RedirectToPage("/NotFound"); // NotFound er ikke defineret endnu
            }

            // Fjern billeder fra wwwroot/images
            if (noteToDelete.ImagePaths != null && noteToDelete.ImagePaths.Count > 0)
            {
                foreach (var imgPath in noteToDelete.ImagePaths)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, imgPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            await _noteService.DeleteNoteAsync(Note.Id);
            return RedirectToPage("./GetAllNotes", new { id = LanguageFolderId });
        }
    }
}