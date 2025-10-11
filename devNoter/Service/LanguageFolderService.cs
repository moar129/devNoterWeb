using devNoter.Models;

namespace devNoter.Service
{
    public class LanguageFolderService
    {
        /// <summary>
        /// Service to manage LanguageFolder entities using a generic dbService.
        /// </summary>
        private readonly dbService<LanguageFolder> _context;

        /// <summary>
        /// Constructor to initialize the LanguageFolderService with a dbService context.
        /// </summary>
        /// <param name="context"></param>
        public LanguageFolderService(dbService<LanguageFolder> context)
        {
            _context = context;
        }

        /// <summary>
        /// Method to get all LanguageFolder entities from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LanguageFolder>> GetLanguageFoldersAsync()
        {
            return await _context.GetObjectsAsync();
        }

        /// <summary>
        /// Method to get a single LanguageFolder by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LanguageFolder> GetLanguageFolderByIdAsync(int id)
        {
            return await _context.GetObjectByIdAsync(id);
        }

        /// <summary>
        /// Method to add a new LanguageFolder to the database.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task<LanguageFolder> AddLanguageFolderAsync(LanguageFolder folder)
        {
            return await _context.AddObjectAsync(folder);
        }

        /// <summary>
        /// Method to update an existing LanguageFolder in the database.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task UpdateLanguageFolderAsync(LanguageFolder folder)
        {
            await _context.UpdateObjectAsync(folder);
        }


        /// <summary>
        /// Method to delete a LanguageFolder by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteLanguageFolderAsync(int id)
        {
            await _context.DeleteObjectAsync(id);
        }

        /// <summary>
        /// Method to save a list of LanguageFolder entities to the database.
        /// </summary>
        /// <param name="folders"></param>
        /// <returns></returns>
        public async Task SaveLanguageFoldersAsync(List<LanguageFolder> folders)
        {
            await _context.SaveObjectsAsync(folders);
        }


        /// <summary>
        /// Method to get all top-level LanguageFolders for a specific user (where ParentFolderId is null).
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<LanguageFolder>> GetLanguageFoldersForUserAsync(string userId)
        {
            var allFolders = (await _context.GetObjectsAsync())
                                .Where(f => f.UserId == userId)
                                .ToList();

            // Return only top-level folders (ParentFolderId == null)
            return allFolders.Where(f => f.ParentFolderId == null).ToList();
        }
    }
}
