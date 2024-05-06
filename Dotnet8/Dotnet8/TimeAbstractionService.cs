namespace Dotnet8
{
    public class TimeAbstractionService
    {
        private readonly TimeProvider _timeProvider;

        public TimeAbstractionService(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public string GetTimeOfDay()
        {
            var currentTime = _timeProvider.GetUtcNow();

            var timeOfDayMessage = currentTime.Hour switch
            {
                >= 6 and <= 12 => "Morning",
                > 12 and <= 18 => "Afternoon",
                > 18 and <= 24 => "Evening",
                _ => "Night"
            };

            return timeOfDayMessage;
        }
    }
}
