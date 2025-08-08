using System;
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
    public class ApproveAnticipationRequestUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldApproveAndPersist()
        {
            var id = Guid.NewGuid();
            var entity = new AnticipationRequest(Guid.NewGuid(), 300m, DateTime.UtcNow);

            var repo = new Mock<IAnticipationRequestRepository>();
            repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

            var sut = new ApproveAnticipationRequestUseCase(repo.Object);

            await sut.ExecuteAsync(id);

            Assert.Equal(RequestStatus.Approved, entity.Status);
            repo.Verify(r => r.UpdateAsync(entity), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrow_WhenNotFound()
        {
            var repo = new Mock<IAnticipationRequestRepository>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((AnticipationRequest?)null);

            var sut = new ApproveAnticipationRequestUseCase(repo.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => sut.ExecuteAsync(Guid.NewGuid()));
            repo.Verify(r => r.UpdateAsync(It.IsAny<AnticipationRequest>()), Times.Never);
        }
    }
}
