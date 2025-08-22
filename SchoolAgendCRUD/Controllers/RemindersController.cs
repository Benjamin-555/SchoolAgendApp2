using Microsoft.AspNetCore.Mvc;
using SchoolAgend.Domain.Entities;
using SchoolAgendCRUD.DTOs;
using SchoolAgend.Infrastructure.Data.Repositories;
using SchoolAgend.Application.Contracts;
using SchoolAgendCRUD.DTOs;

namespace SchoolAgendCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderRepository _reminderRepository;

        public RemindersController(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetReminders()
        {
            var reminders = await _reminderRepository.GetAllAsync();
            return Ok(reminders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReminderById(int id)
        {
            var reminder = await _reminderRepository.GetByIdAsync(id);
            if (reminder == null) return NotFound();
            return Ok(reminder);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReminder([FromBody] Reminder reminder)
        {
            if (reminder == null || string.IsNullOrWhiteSpace(reminder.Message))
                return BadRequest("Invalid reminder data.");

            await _reminderRepository.AddAsync(reminder);
            return Created($"/api/reminders/{reminder.Id}", reminder);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateReminder([FromBody] Reminder reminder)
        {
            if (reminder == null || reminder.Id <= 0) return BadRequest("Invalid reminder data.");

            var existingReminder = await _reminderRepository.GetByIdAsync(reminder.Id);
            if (existingReminder == null) return NotFound($"Reminder with ID {reminder.Id} not found.");

            existingReminder.Message = reminder.Message;

            await _reminderRepository.UpdateAsync(existingReminder);
            return Ok(existingReminder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            var reminder = await _reminderRepository.GetByIdAsync(id);
            if (reminder == null) return NotFound($"Reminder with ID {id} not found.");

            await _reminderRepository.DeleteAsync(reminder);
            return NoContent();
        }
    }
}
