using LastLink.Anticipation.API.DTOs;
using LastLink.Anticipation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LastLink.Anticipation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnticipationRequestsController : ControllerBase
    {
        private readonly ICreateAnticipationRequestUseCase _createUseCase;
        private readonly IListAnticipationRequestsByCreatorUseCase _listUseCase;
        private readonly IApproveAnticipationRequestUseCase _approveUseCase;
        private readonly IRejectAnticipationRequestUseCase _rejectUseCase;
        private readonly ISimulateAnticipationRequestUseCase _simulateUseCase;

        public AnticipationRequestsController(
            ICreateAnticipationRequestUseCase createUseCase,
            IListAnticipationRequestsByCreatorUseCase listUseCase,
            IApproveAnticipationRequestUseCase approveUseCase,
            IRejectAnticipationRequestUseCase rejectUseCase,
            ISimulateAnticipationRequestUseCase simulateUseCase)
        {
            _createUseCase = createUseCase;
            _listUseCase = listUseCase;
            _approveUseCase = approveUseCase;
            _rejectUseCase = rejectUseCase;
            _simulateUseCase = simulateUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAnticipationRequestDto dto)
        {
            var request = await _createUseCase.ExecuteAsync(dto.CreatorId, dto.RequestedAmount, dto.RequestedAt);
            return CreatedAtAction(nameof(GetByCreator), new { creatorId = dto.CreatorId }, request);
        }

        [HttpGet("{creatorId}")]
        public async Task<IActionResult> GetByCreator(Guid creatorId)
        {
            var list = await _listUseCase.ExecuteAsync(creatorId);
            return Ok(list);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id, CancellationToken ct)
        {
            await _approveUseCase.ExecuteAsync(id, ct);
            return NoContent();
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id, CancellationToken ct)
        {
            await _rejectUseCase.ExecuteAsync(id, ct);
            return NoContent();
        }

        [HttpGet("simulate")]
        public async Task<IActionResult> Simulate([FromQuery] decimal requestedAmount, CancellationToken ct)
        {
            var result = await _simulateUseCase.ExecuteAsync(requestedAmount, ct);
            return Ok(result);
        }
    }
}
