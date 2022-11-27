using Domain.Exceptions;
using Shared.Domain;

namespace Domain.ValueObjects
{
    public record BoardClock
    {
        public DateTime StartTime { get; set; }
        public DateTime LastMoveTime { get; set; }
        public TimeSpan WhitePlayerDuration { get; set; }
        public TimeSpan BlackPlayerDuration { get; set; }
        public TimeSpan EndTurnTimeIncrement { get; set; }
        public BoardClock()
        {
            StartTime = DateTime.UtcNow;
            EndTurnTimeIncrement = TimeSpan.Zero;
            WhitePlayerDuration = new TimeSpan(DomainConstants.DefaultGameDurationHours,
                DomainConstants.DefaultGameDurationMinutes,
                DomainConstants.DefaultGameDurationSeconds);
            BlackPlayerDuration = new TimeSpan(DomainConstants.DefaultGameDurationHours,
                DomainConstants.DefaultGameDurationMinutes,
                DomainConstants.DefaultGameDurationSeconds); 
        }
        public BoardClock(TimeSpan TurnDuration)
        {
            if (TurnDuration.TotalMinutes < DomainConstants.MinTurnDurationInMinutes || TurnDuration.TotalMinutes > DomainConstants.MaxTurnDurationInMinutes)
            {
                throw new InvalidGameDurationException(TurnDuration);
            }
            StartTime = DateTime.UtcNow;
            EndTurnTimeIncrement = TimeSpan.Zero;
            WhitePlayerDuration = TurnDuration;
            BlackPlayerDuration = TurnDuration;
        }
        public BoardClock(TimeSpan TurnDuration,TimeSpan endTurnIncrement)
        {
            if (TurnDuration.TotalMinutes < DomainConstants.MinTurnDurationInMinutes || TurnDuration.TotalMinutes > DomainConstants.MaxTurnDurationInMinutes)
            {
                throw new InvalidGameDurationException(TurnDuration);
            }
            if (endTurnIncrement.TotalSeconds < DomainConstants.MinIncrementAmounthInSeconds 
                || endTurnIncrement.TotalSeconds>DomainConstants.MaxIncrementAmounthPercentageFromGameDuration*TurnDuration.TotalSeconds)
            {
                throw new InvalidIncrementAmounthException();
            }
            StartTime = DateTime.UtcNow;
            EndTurnTimeIncrement = endTurnIncrement;
            WhitePlayerDuration = TurnDuration;
            BlackPlayerDuration = TurnDuration;
        }

        public bool DecrementTimer(PieceColor playerColor,DateTime moveTime)
        {
            var moveDuration = moveTime - LastMoveTime;
            if (playerColor==PieceColor.White)
            {
                WhitePlayerDuration -= moveDuration;
                if (WhitePlayerDuration<TimeSpan.Zero)
                {
                    return false;
                }
                WhitePlayerDuration += EndTurnTimeIncrement;
            }
            else
            {
                BlackPlayerDuration -= moveDuration;
                if (BlackPlayerDuration < TimeSpan.Zero)
                {
                    return false;
                }
                BlackPlayerDuration += EndTurnTimeIncrement;
            }
            LastMoveTime=moveTime;
            return true;
        }
    }
}
