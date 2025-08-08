using LastLink.Anticipation.Application.Exceptions;
using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Domain.Interfaces;

namespace LastLink.Anticipation.Application.UseCases
{
    public class ApproveAnticipationRequestUseCase : IApproveAnticipationRequestUseCase
    {
        private readonly IAnticipationRequestRepository _repository;

        public ApproveAnticipationRequestUseCase(IAnticipationRequestRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task ExecuteAsync(Guid requestId, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(requestId);
            if (entity is null)
                throw new NotFoundException("Anticipation request not found.");

            entity.Approve();
            await _repository.UpdateAsync(entity);
        }
    }
}
