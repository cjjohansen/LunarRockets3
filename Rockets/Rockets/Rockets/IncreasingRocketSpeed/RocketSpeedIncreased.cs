using Core.Events;
using System;

namespace Lunar.Rockets.IncreasingSpeed;

public record RocketSpeedIncreased(
    Guid RocketId,
    long SpeedIncrement
): IEvent
{
    public static RocketSpeedIncreased Create(Guid rocketId, long speedIncrement)
    {
        if (rocketId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(rocketId));

        return new RocketSpeedIncreased(rocketId, speedIncrement);
    }
}
