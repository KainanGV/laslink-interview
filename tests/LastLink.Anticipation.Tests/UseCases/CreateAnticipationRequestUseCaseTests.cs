using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLink.Anticipation.Application.Exceptions;
using LastLink.Anticipation.Application.UseCases;
using LastLink.Anticipation.Domain.Entities;
using LastLink.Anticipation.Domain.Enums;
using LastLink.Anticipation.Domain.Interfaces;
using Moq;
using Xunit;

namespace LastLink.Anticipation.Tests.UseCases
{
    public class CreateAnticipationRequestUseCaseTests
    {
        [Fact]
        public async Task Should_Throw_When_Amount_Is_Less_Than_100()
        {
            var repo = new Mock<IAnticipationRequestRepository>();
            var sut = new CreateAnticipationRequestUseCase(repo.Object);

            await Assert.ThrowsAsync<AppValidationException>(() =>
                sut.ExecuteAsync(Guid.NewGuid(), 50m, DateTime.UtcNow));
        }

        [Fact]
        public async Task Should_Throw_When_Already_Has_Pending_Request()
        {
            var creatorId = Guid.NewGuid();
            var existing = new List<AnticipationRequest>
            {
                new AnticipationRequest(creatorId, 200m, DateTime.UtcNow) // status default: Pending
            };

            var repo = new Mock<IAnticipationRequestRepository>();
            repo.Setup(r => r.GetByCreatorIdAsync(creatorId))
                .ReturnsAsync(existing);

            var sut = new CreateAnticipationRequestUseCase(repo.Object);

            await Assert.ThrowsAsync<ConflictException>(() =>
                sut.ExecuteAsync(creatorId, 300m, DateTime.UtcNow));
        }

        [Fact]
        public async Task Should_Create_When_Valid()
        {
            var creatorId = Guid.NewGuid();
            var repo = new Mock<IAnticipationRequestRepository>();
            repo.Setup(r => r.GetByCreatorIdAsync(creatorId))
                .ReturnsAsync(Enumerable.Empty<AnticipationRequest>());

            var sut = new CreateAnticipationRequestUseCase(repo.Object);

            var result = await sut.ExecuteAsync(creatorId, 300m, DateTime.UtcNow);

            Assert.Equal(creatorId, result.CreatorId);
            Assert.Equal(RequestStatus.Pending, result.Status);
            repo.Verify(r => r.AddAsync(It.IsAny<AnticipationRequest>()), Times.Once);
        }
    }
}
