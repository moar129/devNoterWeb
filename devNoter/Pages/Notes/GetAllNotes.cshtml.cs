using devNoter.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace devNoter.Pages.Notes
{
    [Authorize]
    public class GetAllNotesModel : PageModel
    {
        private NoteService _noteService;
        private LanguageFolderService _languageFolderService;

        public GetAllNotesModel(NoteService noteService, LanguageFolderService languageFolderService)
        {
            _noteService = noteService;
            _languageFolderService = languageFolderService;
        }
        [BindProperty]
        public List<Models.Note> Notes { get; set; }
        [BindProperty]
        public Models.LanguageFolder Folder { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {

            Notes = new List<Models.Note>();
            Folder = await _languageFolderService.GetLanguageFolderByIdAsync(id);
            foreach (var note in (List<Models.Note>)await _noteService.GetNotesAsync())
            {
                if (note.LanguageFolderId == id)
                {
                    Notes.Add(note);
                }
            }
            return Page();
        }
    }
}
