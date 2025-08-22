using SchoolAgend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolAgend.Application.Contracts;

namespace SchoolAgend.Application.Contracts
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllAsync();
        Task<Session> GetByIdAsync(int id);
        Task AddAsync(Session session);
        Task UpdateAsync(Session session);
        Task DeleteAsync(Session session);
    }
}
