using Microsoft.AspNetCore.Mvc;
using SchoolAgend.Domain.Entities;
using SchoolAgendCRUD.DTOs;
using SchoolAgend.Infrastructure.Data.Repositories;
using SchoolAgendCRUD;
using SchoolAgend.Application.Services;
using SchoolAgend.Application.Contracts;
using SchoolAgendCRUD.DTOs;


namespace SchoolAgendCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksIController : ControllerBase
    {
        private readonly ITaskIRepository _taskRepository;

        public TasksIController(ITaskIRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskI task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title))
                return BadRequest("Invalid task data.");

            await _taskRepository.AddAsync(task);
            return Created($"/api/tasks/{task.Id}", task);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskI task)
        {
            if (task == null || task.Id <= 0) return BadRequest("Invalid task data.");

            var existingTask = await _taskRepository.GetByIdAsync(task.Id);
            if (existingTask == null) return NotFound($"Task with ID {task.Id} not found.");

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;

            await _taskRepository.UpdateAsync(existingTask);
            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return NotFound($"Task with ID {id} not found.");

            await _taskRepository.DeleteAsync(task);
            return NoContent();
        }
    }
}
