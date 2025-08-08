using System;
using System.Threading.Tasks;
using LastLink.Anticipation.Application.Exceptions;
using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Application.Models;
using LastLink.Anticipation.Application.Services;
using LastLink.Anticipation.Application.UseCases;
using Moq;
using Xunit;

namespace LastLink.Anticipation.Tests.UseCases
{
    public class SimulateAnticipationRequestUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldReturnSimulation_FromSimulator()
        {
            const decimal amount = 1000m;
            var expected = new AnticipationSimulationResult(amount, AnticipationSimulator.DefaultFeePercentage);

            var simulator = new Mock<IAnticipationSimulator>();
            simulator.Setup(s => s.Simulate(amount)).Returns(expected);

            var sut = new SimulateAnticipationRequestUseCase(simulator.Object);

            var result = await sut.ExecuteAsync(amount);

            Assert.Equal(expected.NetAmount, result.NetAmount);
            Assert.Equal(expected.FeeAmount, result.FeeAmount);
            simulator.Verify(s => s.Simulate(amount), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(99.99)]
        public async Task ExecuteAsync_ShouldThrow_WhenBelowMinimum(decimal invalidAmount)
        {
            var simulator = new Mock<IAnticipationSimulator>();
            var sut = new SimulateAnticipationRequestUseCase(simulator.Object);

            await Assert.ThrowsAsync<AppValidationException>(() => sut.ExecuteAsync(invalidAmount));
            simulator.VerifyNoOtherCalls();
        }
    }
}
