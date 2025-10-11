using devNoter.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;

namespace devNoter.Pages.Notes
{
    [Authorize]
    public class GetNoteModel : PageModel
    {
        private NoteService _noteService;
        public GetNoteModel(NoteService noteService)
        {
            _noteService = noteService;
        }
        [BindProperty]
        public Models.Note Note { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Note = await _noteService.GetNoteByIdAsync(id);
            if (Note == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
