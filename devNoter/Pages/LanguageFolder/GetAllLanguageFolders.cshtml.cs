using devNoter.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using devNoter.Models;

namespace devNoter.Pages.LanguageFolder
{
    public class GetAllLanguageFoldersModel : PageModel
    {
        private readonly LanguageFolderService _languageFolder;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllLanguageFoldersModel(LanguageFolderService languageFolder, UserManager<ApplicationUser> userManager)
        {
            _languageFolder = languageFolder;
            _userManager = userManager;
        }

        [BindProperty]
        public List<Models.LanguageFolder> Folders { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var allFolders = await _languageFolder.GetLanguageFoldersAsync();

            // Filter folders to only those owned by the current user
            Folders = allFolders.Where(f => f.UserId == user.Id).ToList();
        }
    }
}
