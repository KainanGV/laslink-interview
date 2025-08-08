using LastLink.Anticipation.Domain.Entities;

namespace LastLink.Anticipation.Application.Interfaces
{
    public interface IListAnticipationRequestsByCreatorUseCase
    {
        Task<IEnumerable<AnticipationRequest>> ExecuteAsync(Guid creatorId);
    }
}
