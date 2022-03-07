using Core.Events;
using System;

namespace Lunar.Rockets.DecreasingSpeed;

public record RocketSpeedDecreased(
    Guid RocketId,
    long SpeedDecrement
): IEvent
{
    public static RocketSpeedDecreased Create(Guid rocketId, long speedDecrement)
    {
        if (rocketId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(rocketId));

        return new RocketSpeedDecreased(rocketId, speedDecrement);
    }
}
