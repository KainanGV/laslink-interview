using LastLink.Anticipation.Application.Exceptions;
using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Application.Models;

namespace LastLink.Anticipation.Application.UseCases
{
    public sealed class SimulateAnticipationRequestUseCase : ISimulateAnticipationRequestUseCase
    {
        public const decimal MinimumAllowedAmount = 100m;
        private readonly IAnticipationSimulator _simulator;

        public SimulateAnticipationRequestUseCase(IAnticipationSimulator simulator)
        {
            _simulator = simulator ?? throw new ArgumentNullException(nameof(simulator));
        }

        public Task<AnticipationSimulationResult> ExecuteAsync(decimal requestedAmount, CancellationToken ct = default)
        {
            if (requestedAmount < MinimumAllowedAmount)
                throw new AppValidationException($"Requested amount must be at least {MinimumAllowedAmount:C}.",
                    new Dictionary<string, string[]> { ["requestedAmount"] = [$"min {MinimumAllowedAmount}"] });

            var result = _simulator.Simulate(requestedAmount);
            return Task.FromResult(result);
        }
    }
}
