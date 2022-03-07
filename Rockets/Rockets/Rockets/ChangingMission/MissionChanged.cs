using Core.Events;
using System;

namespace Lunar.Rockets.ChangingMission;

public record MissionChanged(
    Guid RocketId,
    string PreviousMission,
    string NewMission
): IEvent
{
    public static MissionChanged Create(Guid rocketId,string oldMission, string newMission)
    {
        if (rocketId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(rocketId));

        return new MissionChanged(rocketId,oldMission, newMission);
    }
}
