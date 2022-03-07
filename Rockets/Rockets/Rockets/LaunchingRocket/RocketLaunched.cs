using Core.Events;
using System;

namespace Lunar.Rockets.Launching;

public record RocketLaunched(
    Guid RocketId,
    long MessageNumber,
    DateTime MessageTime,
    String Type,    
    String Mission,
    long Speed,
    RocketStatus Status
) : IEvent
{
    public static RocketLaunched Create(Guid channelId,long messageNumber, DateTime messageTime, String type,String mission, long speed, RocketStatus rocketStatus)
    {
        if (channelId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(channelId));
        if (String.IsNullOrEmpty(type))
            throw new ArgumentOutOfRangeException(nameof(type));
        if (String.IsNullOrEmpty(mission))
            throw new ArgumentOutOfRangeException(nameof(mission));

        //TODO: Check all parameters

        return new RocketLaunched(channelId, messageNumber, messageTime, type, mission, speed, rocketStatus);
    }
}
