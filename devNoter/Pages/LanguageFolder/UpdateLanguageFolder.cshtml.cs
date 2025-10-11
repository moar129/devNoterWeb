using devNoter.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using devNoter.Models;

namespace devNoter.Pages.LanguageFolder
{
    public class UpdateLanguageFolderModel : PageModel
    {
        private readonly LanguageFolderService _languageFolderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateLanguageFolderModel(LanguageFolderService languageFolderService, UserManager<ApplicationUser> userManager)
        {
            _languageFolderService = languageFolderService;
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
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            // Fetch the folder again to verify ownership
            var folderFromDb = await _languageFolderService.GetLanguageFolderByIdAsync(Folder.Id);
            if (folderFromDb == null || folderFromDb.UserId != user.Id)
            {
                return RedirectToPage("/NotFound"); // Prevent editing others' folders
            }

            folderFromDb.Name = Folder.Name; // Update only allowed fields
            await _languageFolderService.UpdateLanguageFolderAsync(folderFromDb);

            return RedirectToPage("/Index");
        }
    }
}
