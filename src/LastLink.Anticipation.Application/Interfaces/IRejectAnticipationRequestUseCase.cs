namespace LastLink.Anticipation.Application.Interfaces
{
    public interface IRejectAnticipationRequestUseCase
    {
        Task ExecuteAsync(Guid requestId, CancellationToken ct = default);
    }
}
