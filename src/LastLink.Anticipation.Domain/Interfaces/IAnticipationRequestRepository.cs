namespace LastLink.Anticipation.Domain.Interfaces
{
    using LastLink.Anticipation.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAnticipationRequestRepository
    {
        Task<IEnumerable<AnticipationRequest>> GetByCreatorIdAsync(Guid creatorId);
        Task<AnticipationRequest?> GetPendingByCreatorIdAsync(Guid creatorId);
        Task<AnticipationRequest?> GetByIdAsync(Guid id);
        Task AddAsync(AnticipationRequest request);
        Task UpdateAsync(AnticipationRequest request);
        Task<bool> ExistsPendingRequestAsync(Guid creatorId);

    }
}
