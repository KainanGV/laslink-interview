using LastLink.Anticipation.Application.Models;

namespace LastLink.Anticipation.Application.Interfaces
{
    public interface ISimulateAnticipationRequestUseCase
    {
        Task<AnticipationSimulationResult> ExecuteAsync(decimal requestedAmount, CancellationToken ct = default);
    }
}
