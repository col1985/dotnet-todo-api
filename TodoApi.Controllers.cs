using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;
using Microsoft.Extensions.Logging; // Add this line

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoController> _logger; // Declare the logger

        public TodoController(ITodoRepository repository, ILogger<TodoController> logger)
        {
            _repository = repository;
            _logger = logger; // Inject the logger instance
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get()
        {
            _logger.LogInformation("GET request received for all Todo items."); // Log the request
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(Guid id)
        {
            _logger.LogInformation("GET request received for Todo item with Id: {Id}", id); // Log with a template
            var item = _repository.GetById(id);
            if (item == null)
            {
                _logger.LogWarning("Todo item with Id: {Id} not found.", id); // Log a warning for a not-found case
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public ActionResult Post([FromBody] TodoItem item)
        {
            _logger.LogInformation("POST request received to create a new Todo item with title: {Title}", item.Title);
            _repository.Add(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public ActionResult Put(Guid id, [FromBody] TodoItem item)
        {
            _logger.LogInformation("PUT request received to update Todo item with Id: {Id}", id);
            if (id != item.Id)
            {
                _logger.LogWarning("Mismatched Ids in PUT request. Route Id: {RouteId}, Body Id: {BodyId}", id, item.Id);
                return BadRequest();
            }

            var existingItem = _repository.GetById(id);
            if (existingItem == null)
            {
                _logger.LogWarning("Attempted to update a non-existent Todo item with Id: {Id}", id);
                return NotFound();
            }

            _repository.Update(item);
            return NoContent();
        }
    }
}