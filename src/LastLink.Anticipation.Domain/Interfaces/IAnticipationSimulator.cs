namespace LastLink.Anticipation.Domain.Interfaces
{
    public interface IAnticipationSimulator
    {
        AnticipationSimulationResult Simulate(decimal requestedAmount);
    }

    public class AnticipationSimulationResult
    {
        public decimal RequestedAmount { get; set; }
        public decimal Fee { get; set; }
        public decimal NetAmount { get; set; }
    }
}
