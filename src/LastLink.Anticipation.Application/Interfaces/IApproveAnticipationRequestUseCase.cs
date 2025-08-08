namespace LastLink.Anticipation.Application.Interfaces
{
    public interface IApproveAnticipationRequestUseCase
    {
        Task ExecuteAsync(Guid requestId, CancellationToken ct = default);
    }
}
