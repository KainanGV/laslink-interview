using LastLink.Anticipation.Domain.Entities;
using LastLink.Anticipation.Domain.Enums;
using LastLink.Anticipation.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LastLink.Anticipation.Infra.Data.Repositories
{
    public class AnticipationRequestRepository : IAnticipationRequestRepository
    {
        private readonly AnticipationDbContext _context;

        public AnticipationRequestRepository(AnticipationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AnticipationRequest request)
        {
            await _context.AnticipationRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<AnticipationRequest?> GetPendingByCreatorIdAsync(Guid creatorId)
        {
            return await _context.AnticipationRequests
                .FirstOrDefaultAsync(r => r.CreatorId == creatorId && r.Status == RequestStatus.Pending);
        }

        public async Task<AnticipationRequest?> GetByIdAsync(Guid id)
        {
            return await _context.AnticipationRequests.FindAsync(id);
        }

        public async Task<IEnumerable<AnticipationRequest>> GetByCreatorIdAsync(Guid creatorId)
        {
            return await _context.AnticipationRequests
                .Where(r => r.CreatorId == creatorId)
                .ToListAsync();
        }

        public async Task UpdateAsync(AnticipationRequest request)
        {
            _context.AnticipationRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsPendingRequestAsync(Guid creatorId)
        {
            return await _context.AnticipationRequests
                .AnyAsync(ar => ar.CreatorId == creatorId && ar.Status == RequestStatus.Pending);

        }

    }
}
