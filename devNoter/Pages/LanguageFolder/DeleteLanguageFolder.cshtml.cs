using devNoter.Models;
using devNoter.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace devNoter.Pages.LanguageFolder
{
    public class DeleteLanguageFolderModel : PageModel
    {
        private LanguageFolderService _languageFolderService;
        private NoteService _noteService;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteLanguageFolderModel(LanguageFolderService languageFolderService, NoteService noteService, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _languageFolderService = languageFolderService;
            _noteService = noteService;
            _environment = environment;
            _userManager = userManager;
        }

        [BindProperty]
        public Models.LanguageFolder Folder { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            Folder = await _languageFolderService.GetLanguageFolderByIdAsync(id);

            if (Folder == null || Folder.UserId != user.Id)
            {
                return RedirectToPage("/NotFound"); // NotFound er ikke defineret endnu
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var folderToDelete = await _languageFolderService.GetLanguageFolderByIdAsync(Folder.Id);
            var notes = await _noteService.GetNotesAsync();

            if (folderToDelete == null || folderToDelete.UserId != user.Id)
            {
                // Prevent deleting others' folders
                return RedirectToPage("/NotFound"); // not defined yet
            }

            // first remove all notes in the folder
            foreach (var note in notes)
            {
                if (note.LanguageFolderId == folderToDelete.Id)
                {
                    // delete images on disk
                    if (note.ImagePaths != null)
                    {
                        foreach (var imgPath in note.ImagePaths)
                        {
                            var filePath = Path.Combine(_environment.WebRootPath, imgPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);
                        }
                    }

                    await _noteService.DeleteNoteAsync(note.Id);
                }
            }

            // Delete folder after all notes are deleted

            await _languageFolderService.DeleteLanguageFolderAsync(folderToDelete.Id);
            return RedirectToPage("/Index");
        }
    }
}
