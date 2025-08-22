using SchoolAgend.Application.Contracts;
using SchoolAgend.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolAgendCRUD.DTOs;
using SchoolAgend.Application.Contracts;

namespace SchoolAgend.Application.Services
{
    public class SessionService : ISessionRepository
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<IEnumerable<SessionDto>> GetAllSessionsAsync()
        {
            var sessions = await _sessionRepository.GetAllAsync();
            return sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                CourseId = s.CourseId,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Classroom = s.Classroom
            });
        }

        public async Task<SessionDto?> GetSessionByIdAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null) return null;

            return new SessionDto
            {
                Id = session.Id,
                CourseId = session.CourseId,
                DayOfWeek = session.DayOfWeek,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Classroom = session.Classroom
            };
        }

        public async Task<SessionDto> CreateSessionAsync(CreateSessionDto dto)
        {
            var session = new Session
            {
                CourseId = dto.CourseId,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Classroom = dto.Classroom
            };

            await _sessionRepository.AddAsync(session);

            return new SessionDto
            {
                Id = session.Id,
                CourseId = session.CourseId,
                DayOfWeek = session.DayOfWeek,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Classroom = session.Classroom
            };
        }

        public async Task<bool> UpdateSessionAsync(int id, UpdateSessionDto dto)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null) return false;

            session.CourseId = dto.CourseId;
            session.DayOfWeek = dto.DayOfWeek;
            session.StartTime = dto.StartTime;
            session.EndTime = dto.EndTime;
            session.Classroom = dto.Classroom;

            await _sessionRepository.UpdateAsync(session);
            return true;
        }

        public async Task<bool> DeleteSessionAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null) return false;

            await _sessionRepository.DeleteAsync(session);
            return true;
        }

        public Task<IEnumerable<Session>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Session> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Session session)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Session session)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Session session)
        {
            throw new NotImplementedException();
        }
    }
}
