namespace LastLink.Anticipation.Domain.Entities
{
    using System;
    using LastLink.Anticipation.Domain.Enums;

    public class AnticipationRequest
    {
        public Guid Id { get; private set; }
        public Guid CreatorId { get; private set; }
        public decimal RequestedAmount { get; private set; }
        public decimal NetAmount { get; private set; }
        public DateTime RequestedAt { get; private set; }
        public RequestStatus Status { get; private set; }

        private const decimal FeePercentage = 0.05m;

        public AnticipationRequest(Guid creatorId, decimal requestedAmount, DateTime requestedAt)
        {
            if (requestedAmount < 100)
                throw new ArgumentException("Valor solicitado deve ser maior que R$100,00");

            Id = Guid.NewGuid();
            CreatorId = creatorId;
            RequestedAmount = requestedAmount;
            NetAmount = CalculateNetAmount(requestedAmount);
            RequestedAt = requestedAt;
            Status = RequestStatus.Pending;
        }

        private decimal CalculateNetAmount(decimal amount) => amount * (1 - FeePercentage);

        public void Approve() => Status = RequestStatus.Approved;
        public void Reject() => Status = RequestStatus.Rejected;
    }
}
