using Dotnet8;
using FluentAssertions;
using Moq;

namespace Dotnet8Tests
{
    [TestClass]
    public class TimeAbstractionServiceTests
    {
        private readonly TimeAbstractionService _demoService;
        private readonly Mock<TimeProvider> _timeProviderMock = new();

        public TimeAbstractionServiceTests()
        {
            _demoService = new TimeAbstractionService(_timeProviderMock.Object);
        }

        [TestMethod]
        [DynamicData(nameof(TimeOfDayTestCases))]
        public void GetTimeOfDay_ShouldReturnExpectedTimeOfDay(
            DateTimeOffset date, string expectedMessage)
        {
            // Arrange
            _timeProviderMock.Setup(c => c.GetUtcNow()).Returns(date);

            // Act
            var result = _demoService.GetTimeOfDay();

            // Assert
            result.Should().Be(expectedMessage);
        }

        public static IEnumerable<object[]> TimeOfDayTestCases
        {
            get
            {
                return
                [
                    [new DateTimeOffset(2024, 1, 1, 6, 0, 0, TimeSpan.Zero), "Morning"],
                    [new DateTimeOffset(2024, 1, 1, 13, 0, 0, TimeSpan.Zero), "Afternoon"],
                    [new DateTimeOffset(2024, 1, 1, 19, 0, 0, TimeSpan.Zero), "Evening"],
                    [new DateTimeOffset(2024, 1, 1, 00, 0, 0, TimeSpan.Zero), "Night"],
                ];
            }
        }
    }
}