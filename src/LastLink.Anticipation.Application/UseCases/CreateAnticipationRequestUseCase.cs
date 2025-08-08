using LastLink.Anticipation.Application.Exceptions;
using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Domain.Entities;
using LastLink.Anticipation.Domain.Enums;
using LastLink.Anticipation.Domain.Interfaces;

namespace LastLink.Anticipation.Application.UseCases
{
    public class CreateAnticipationRequestUseCase : ICreateAnticipationRequestUseCase
    {
        private readonly IAnticipationRequestRepository _repository;
        public const decimal MinimumAllowedAmount = 100m;

        public CreateAnticipationRequestUseCase(IAnticipationRequestRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<AnticipationRequest> ExecuteAsync(Guid creatorId, decimal requestedAmount, DateTime requestedAt)
        {
            if (creatorId == Guid.Empty)
                throw new AppValidationException("CreatorId must be a non-empty GUID.",
                    new Dictionary<string, string[]> { ["creatorId"] = ["invalid guid"] });

            if (requestedAmount < MinimumAllowedAmount)
                throw new AppValidationException($"Requested amount must be at least {MinimumAllowedAmount:C}.",
                    new Dictionary<string, string[]> { ["requestedAmount"] = [$"min {MinimumAllowedAmount}"] });

            var pendings = await _repository.GetByCreatorIdAsync(creatorId);
            if (pendings.Any(r => r.Status == RequestStatus.Pending))
                throw new ConflictException("Creator already has a pending anticipation request.");

            var entity = new AnticipationRequest(creatorId, requestedAmount, requestedAt);

            await _repository.AddAsync(entity);
            return entity;
        }
    }
}
