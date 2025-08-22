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
    public class SessionsController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepository;

        // Inyección de la interfaz
        public SessionsController(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _sessionRepository.GetAllAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] Session session)
        {
            
            await _sessionRepository.AddAsync(session);
            return Created($"/api/sessions/{session.Id}", session);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSession([FromBody] Session session)
        {
            if (session == null || session.Id <= 0) return BadRequest("Invalid session data.");

            var existingSession = await _sessionRepository.GetByIdAsync(session.Id);
            if (existingSession == null) return NotFound($"Session with ID {session.Id} not found.");

            existingSession.Classroom = session.Classroom;
            existingSession.DayOfWeek = session.DayOfWeek;

            await _sessionRepository.UpdateAsync(existingSession);
            return Ok(existingSession);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null) return NotFound($"Session with ID {id} not found.");

            await _sessionRepository.DeleteAsync(session);
            return NoContent();
        }
    }
}
