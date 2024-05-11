using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes_API.Contracts;
using Notes_API.DB;
using Notes_API.Models;
using System.Linq.Expressions;

namespace Notes_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NotesDbContext _dbContext;

        public NotesController(NotesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult>Create([FromBody] NoteRequest request, CancellationToken ct)
        {

            if (string.IsNullOrEmpty(request.title))
            {
                return BadRequest("Title is required.");
            }

            var note = new Note(request.title, request.description);

            await _dbContext.Notes.AddAsync(note, ct);
            await _dbContext.SaveChangesAsync(ct);

            return Ok();
        }

        private Expression<Func<Note, object>> GetSelectorKey(string? sortItem)
        {
            switch(sortItem?.ToLower()) 
            {
                case "date":
                    return note => note.CreationDate;
                case "title":
                    return note => note.Title;     
                default:
                    return note => note.Id;
            }
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetNotesRequest request)
        {
            var notesQuery = _dbContext.Notes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                if (request.SearchAll)
                {
                    notesQuery = notesQuery.Where(note =>
                        note.Title.ToLower().Contains(request.Search.ToLower()) ||
                        note.Description.ToLower().Contains(request.Search.ToLower()));
                }
                else
                {
                    notesQuery = notesQuery.Where(note =>
                        note.Title.ToLower().Contains(request.Search.ToLower()));
                }
            }

            notesQuery = request.SortOrder == "desc"
                ? notesQuery.OrderByDescending(GetSelectorKey(request.SortItem))
                : notesQuery.OrderBy(GetSelectorKey(request.SortItem));


            var notes = await notesQuery.Select(note => new NoteDto(note.Id, note.Title, note.Description, note.CreationDate)).ToListAsync();

            return Ok(new GetNotesResponse(notes));

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NoteRequest request)
        {
            var existingNote = await _dbContext.Notes.FindAsync(id);

            if (existingNote == null)
            {
                return NotFound();
            }

            existingNote.Title = request.title;
            existingNote.Description = request.description;

            _dbContext.Notes.Update(existingNote);
            await _dbContext.SaveChangesAsync();

            return Ok(existingNote);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var noteToDelete = await _dbContext.Notes.FindAsync(id);

            if (noteToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Notes.Remove(noteToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
