using devNoter.Models;
using devNoter.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace devNoter.Pages.LanguageFolder
{
    [Authorize] // Only allow logged-in users
    public class CreateLanguageFolderModel : PageModel
    {
        private readonly LanguageFolderService _languageFolderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateLanguageFolderModel(LanguageFolderService languageFolderService, UserManager<ApplicationUser> userManager)
        {
            _languageFolderService = languageFolderService;
            _userManager = userManager;
        }

        [BindProperty]
        public Models.LanguageFolder LanguageFolder { get; set; }

        public IActionResult OnGet()
        {
            // Page will only be accessible to logged-in users due to [Authorize]
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge(); // Forces redirect to login page
            }

            // Ignore UserId validation from the form
            ModelState.Remove("LanguageFolder.UserId");

            if (!ModelState.IsValid)
            {
                // Collect all validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                // Or log them for debugging
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation error: {error}");
                }

                return Page();
            }
            // Assign the folder to the current user
            LanguageFolder.UserId = user.Id;
            
            

            await _languageFolderService.AddLanguageFolderAsync(LanguageFolder);

            return RedirectToPage("/Index");
        }
    }
}
