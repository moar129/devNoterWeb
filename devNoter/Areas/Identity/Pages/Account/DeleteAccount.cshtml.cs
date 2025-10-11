using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using devNoter.Models;
using devNoter.EFDbContext;
using devNoter.Service;
using static Azure.Core.HttpHeader;

namespace devNoter.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Razor Page Model for deleting a user account.
    /// </summary>
    public class DeleteAccountModel : PageModel
    {
        /// <summary>
        /// Dependencies for user management, database context, environment, and services.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly DevNoterDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly NoteService _noteService;
        private readonly LanguageFolderService _languageFolderService;

        /// <summary>
        /// Constructor to initialize dependencies via dependency injection.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="dbContext"></param>
        /// <param name="environment"></param>
        /// <param name="noteService"></param>
        /// <param name="languageFolderService"></param>
        public DeleteAccountModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            DevNoterDbContext dbContext,
            IWebHostEnvironment environment,
            NoteService noteService,
            LanguageFolderService languageFolderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _environment = environment;
            _noteService = noteService;
            _languageFolderService = languageFolderService;
        }

        public void OnGet()
        {
        }

        /// <summary>
        /// Handle POST request to delete the current user's account.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            // If user is null, redirect to home page
            if (user == null)
            {
                return RedirectToPage("/Index");
            }

            /// <summary>
            /// Delete all user data associated with this account.
            /// </summary>
            ///<returns></returns>
            

            // Delete all folders + notes for this user
            var folders = _dbContext.LanguageFolders.Where(f => f.UserId == user.Id).ToList();
            // For each folder, get its notes and delete them
            foreach (var folder in folders)
            {
                // Get notes for this folder
                var notes = _dbContext.Notes.Where(n => n.LanguageFolderId == folder.Id).ToList();
                // For each note, delete associated images from disk and then delete the note
                foreach (var note in notes)
                {
                    // delete images
                    if (note.ImagePaths != null)
                    {
                        // Delete each image file from disk
                        foreach (var imgPath in note.ImagePaths)
                        {
                            var filePath = Path.Combine(_environment.WebRootPath,imgPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
                            );

                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);
                        }
                    }

                    // delete note
                    await _noteService.DeleteNoteAsync(note.Id);
                }
                // Then delete the folder
                await _languageFolderService.DeleteLanguageFolderAsync(folder.Id);
            }

            // Set result to delete the user
            var result = await _userManager.DeleteAsync(user);
            // If deletion failed, add error to model state and return to page
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred deleting account.");
                return Page();
            }
            // Sign out the user and redirect to home page
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");

        }
    }
}
