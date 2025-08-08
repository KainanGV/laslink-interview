using LastLink.Anticipation.Application.Exceptions;
using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Domain.Entities;
using LastLink.Anticipation.Domain.Interfaces;

namespace LastLink.Anticipation.Application.UseCases
{
    public class ListAnticipationRequestsByCreatorUseCase : IListAnticipationRequestsByCreatorUseCase
    {
        private readonly IAnticipationRequestRepository _repository;

        public ListAnticipationRequestsByCreatorUseCase(IAnticipationRequestRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<IEnumerable<AnticipationRequest>> ExecuteAsync(Guid creatorId)
        {
            if (creatorId == Guid.Empty)
                throw new AppValidationException("CreatorId must be a non-empty GUID.",
                    new Dictionary<string, string[]> { ["creatorId"] = ["invalid guid"] });

            return _repository.GetByCreatorIdAsync(creatorId);
        }
    }
}
