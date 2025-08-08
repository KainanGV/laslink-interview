namespace LastLink.Anticipation.Application.Models
{
    public sealed class AnticipationSimulationResult
    {
        public decimal RequestedAmount { get; }
        public decimal FeePercentage { get; }
        public decimal FeeAmount { get; }
        public decimal NetAmount { get; }

        public AnticipationSimulationResult(decimal requestedAmount, decimal feePercentage)
        {
            if (requestedAmount < 0) throw new ArgumentOutOfRangeException(nameof(requestedAmount));
            if (feePercentage < 0 || feePercentage > 1) throw new ArgumentOutOfRangeException(nameof(feePercentage));

            RequestedAmount = requestedAmount;
            FeePercentage = feePercentage;
            FeeAmount = decimal.Round(requestedAmount * feePercentage, 2, MidpointRounding.AwayFromZero);
            NetAmount = decimal.Round(requestedAmount - FeeAmount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
