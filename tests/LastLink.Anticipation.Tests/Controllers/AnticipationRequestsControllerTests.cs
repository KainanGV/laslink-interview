using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LastLink.Anticipation.API.Controllers;
using LastLink.Anticipation.API.DTOs;
using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Application.Models;
using LastLink.Anticipation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LastLink.Anticipation.Tests.Controllers
{
    public class AnticipationRequestsControllerTests
    {
        private static AnticipationRequestsController BuildController(
            Mock<ICreateAnticipationRequestUseCase>? create = null,
            Mock<IListAnticipationRequestsByCreatorUseCase>? list = null,
            Mock<IApproveAnticipationRequestUseCase>? approve = null,
            Mock<IRejectAnticipationRequestUseCase>? reject = null,
            Mock<ISimulateAnticipationRequestUseCase>? simulate = null)
        {
            return new AnticipationRequestsController(
                (create ?? new Mock<ICreateAnticipationRequestUseCase>()).Object,
                (list ?? new Mock<IListAnticipationRequestsByCreatorUseCase>()).Object,
                (approve ?? new Mock<IApproveAnticipationRequestUseCase>()).Object,
                (reject ?? new Mock<IRejectAnticipationRequestUseCase>()).Object,
                (simulate ?? new Mock<ISimulateAnticipationRequestUseCase>()).Object
            );
        }

        [Fact]
        public async Task Should_Create_Request()
        {
            // Arrange
            var dto = new CreateAnticipationRequestDto
            {
                CreatorId = Guid.NewGuid(),
                RequestedAmount = 200m,
                RequestedAt = DateTime.UtcNow
            };

            var create = new Mock<ICreateAnticipationRequestUseCase>();
            create.Setup(u => u.ExecuteAsync(dto.CreatorId, dto.RequestedAmount, dto.RequestedAt))
                  .ReturnsAsync(new AnticipationRequest(dto.CreatorId, dto.RequestedAmount, dto.RequestedAt));

            var controller = BuildController(create: create);

            // Act
            var result = await controller.Create(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.GetByCreator), createdResult.ActionName);
            create.Verify(u => u.ExecuteAsync(dto.CreatorId, dto.RequestedAmount, dto.RequestedAt), Times.Once);
        }

        [Fact]
        public async Task Should_List_Requests_By_Creator()
        {
            // Arrange
            var creatorId = Guid.NewGuid();
            var list = new Mock<IListAnticipationRequestsByCreatorUseCase>();
            list.Setup(u => u.ExecuteAsync(creatorId))
                .ReturnsAsync(new List<AnticipationRequest>());

            var controller = BuildController(list: list);

            // Act
            var result = await controller.GetByCreator(creatorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<AnticipationRequest>>(okResult.Value);
            list.Verify(u => u.ExecuteAsync(creatorId), Times.Once);
        }

        [Fact]
        public async Task Should_Approve_Request()
        {
            // Arrange
            var id = Guid.NewGuid();
            var approve = new Mock<IApproveAnticipationRequestUseCase>();
            approve.Setup(u => u.ExecuteAsync(id, It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);

            var controller = BuildController(approve: approve);

            // Act
            var result = await controller.Approve(id, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
            approve.Verify(u => u.ExecuteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Reject_Request()
        {
            // Arrange
            var id = Guid.NewGuid();
            var reject = new Mock<IRejectAnticipationRequestUseCase>();
            reject.Setup(u => u.ExecuteAsync(id, It.IsAny<CancellationToken>()))
                  .Returns(Task.CompletedTask);

            var controller = BuildController(reject: reject);

            // Act
            var result = await controller.Reject(id, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
            reject.Verify(u => u.ExecuteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Simulate_Request()
        {
            // Arrange
            const decimal amount = 1000m;
            var expected = new AnticipationSimulationResult(amount, 0.05m);

            var simulate = new Mock<ISimulateAnticipationRequestUseCase>();
            simulate.Setup(u => u.ExecuteAsync(amount, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expected);

            var controller = BuildController(simulate: simulate);

            // Act
            var result = await controller.Simulate(amount, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var payload = Assert.IsType<AnticipationSimulationResult>(ok.Value);
            Assert.Equal(expected.NetAmount, payload.NetAmount);
            Assert.Equal(expected.FeeAmount, payload.FeeAmount);
            simulate.Verify(u => u.ExecuteAsync(amount, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
