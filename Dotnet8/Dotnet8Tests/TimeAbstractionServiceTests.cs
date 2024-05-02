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

        [DataTestMethod]
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

        public static IEnumerable<object[]> TimeOfDayTestCases()
        {
            yield return new object[] {
           new DateTimeOffset(2024, 1, 1, 6, 0, 0, TimeSpan.Zero), "Morning" };
            yield return new object[] {
           new DateTimeOffset(2024, 1, 1, 12, 59, 59, TimeSpan.Zero), "Morning" };
            yield return new object[] {
           new DateTimeOffset(2024, 1, 1, 13, 0, 0, TimeSpan.Zero), "Afternoon" };
            yield return new object[] {
           new DateTimeOffset(2024, 1, 1, 18, 59, 59, TimeSpan.Zero), "Afternoon" };
            yield return new object[] {
           new DateTimeOffset(2024, 1, 1, 19, 0, 0, TimeSpan.Zero), "Evening" };
            yield return new object[] {
           new DateTimeOffset(2024, 1, 1, 23, 59, 59, TimeSpan.Zero), "Evening" };
            yield return new object[] {
           new DateTimeOffset(2024, 1, 1, 00, 0, 0, TimeSpan.Zero), "Night" };
            yield return new object[] {
           new DateTimeOffset(2024, 1, 1, 05, 59, 59, TimeSpan.Zero), "Night" };
        }
    }
}