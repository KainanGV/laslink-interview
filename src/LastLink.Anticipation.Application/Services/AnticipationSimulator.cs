using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Application.Models;

namespace LastLink.Anticipation.Application.Services
{
    public sealed class AnticipationSimulator : IAnticipationSimulator
    {
        public const decimal DefaultFeePercentage = 0.05m;
        public AnticipationSimulationResult Simulate(decimal requestedAmount)
            => new(requestedAmount, DefaultFeePercentage);
    }
}
