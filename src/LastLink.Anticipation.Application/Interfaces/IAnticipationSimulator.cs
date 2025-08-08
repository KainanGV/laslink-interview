using LastLink.Anticipation.Application.Models;

namespace LastLink.Anticipation.Application.Interfaces
{
    public interface IAnticipationSimulator
    {
        AnticipationSimulationResult Simulate(decimal requestedAmount);
    }
}
