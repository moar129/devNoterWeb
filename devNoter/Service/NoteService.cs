using devNoter.Models;

namespace devNoter.Service
{
    public class NoteService
    {
        /// <summary>
        /// Service to manage Note entities using a generic dbService.
        /// </summary>
        private readonly dbService<Note> _context;

        /// <summary>
        /// Constructor to initialize the NoteService with a dbService context.
        /// </summary>
        /// <param name="context"></param>
        public NoteService(dbService<Note> context)
        {
            _context = context;
        }

        /// <summary>
        /// Method to get all Note entities from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Note>> GetNotesAsync()
        {
            return await _context.GetObjectsAsync();
        }

        /// <summary>
        /// Method to get a single Note by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Note> GetNoteByIdAsync(int id)
        {
            return await _context.GetObjectByIdAsync(id);
        }

        /// <summary>
        /// Method to add a new Note to the database.
        /// </summary>
        /// <param name="languageFolderId"></param>
        /// <returns></returns>
        public async Task<Note> AddNoteAsync(Note note)
        {
            return await _context.AddObjectAsync(note);
        }

        /// <summary>
        /// Method to update an existing Note in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdateNoteAsync(Note note)
        {
            await _context.UpdateObjectAsync(note);
        }

        /// <summary>
        /// Method to delete a Note from the database by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteNoteAsync(int id)
        {
            await _context.DeleteObjectAsync(id);
        }
    }
}
