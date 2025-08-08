namespace LastLink.Anticipation.API.DTOs
{
    public class CreateAnticipationRequestDto
    {
        public Guid CreatorId { get; set; }
        public decimal RequestedAmount { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}
