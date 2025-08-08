using LastLink.Anticipation.Domain.Entities;

namespace LastLink.Anticipation.Application.Interfaces
{
    public interface ICreateAnticipationRequestUseCase
    {
        Task<AnticipationRequest> ExecuteAsync(Guid creatorId, decimal requestedAmount, DateTime requestedAt);
    }
}
