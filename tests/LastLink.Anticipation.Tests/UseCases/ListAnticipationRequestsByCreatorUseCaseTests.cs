using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLink.Anticipation.Application.Exceptions;
using LastLink.Anticipation.Application.UseCases;
using LastLink.Anticipation.Domain.Entities;
using LastLink.Anticipation.Domain.Interfaces;
using Moq;
using Xunit;

namespace LastLink.Anticipation.Tests.UseCases
{
    public class ListAnticipationRequestsByCreatorUseCaseTests
    {
        [Fact]
        public async Task Should_Return_Requests_For_Creator()
        {
            var creatorId = Guid.NewGuid();
            var expected = new List<AnticipationRequest>
            {
                new AnticipationRequest(creatorId, 200m, DateTime.UtcNow),
                new AnticipationRequest(creatorId, 300m, DateTime.UtcNow)
            };

            var repo = new Mock<IAnticipationRequestRepository>();
            repo.Setup(r => r.GetByCreatorIdAsync(creatorId)).ReturnsAsync(expected);

            var sut = new ListAnticipationRequestsByCreatorUseCase(repo.Object);

            var result = await sut.ExecuteAsync(creatorId);

            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Equal(creatorId, r.CreatorId));
        }

        [Fact]
        public async Task Should_Throw_When_CreatorId_Is_Empty()
        {
            var repo = new Mock<IAnticipationRequestRepository>();
            var sut = new ListAnticipationRequestsByCreatorUseCase(repo.Object);

            await Assert.ThrowsAsync<AppValidationException>(() => sut.ExecuteAsync(Guid.Empty));
            repo.Verify(r => r.GetByCreatorIdAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
