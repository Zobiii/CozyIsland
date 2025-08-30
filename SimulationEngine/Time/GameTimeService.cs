using System.Text.Json.Serialization.Metadata;

namespace SimulationEngine.Time;

public sealed class GameTimeService
{
    public int Day { get; private set; } = 1;
    public int Minutes { get; private set; } = 8 * 60;

    public void Advance(float dt)
    {
        Minutes += (int)(dt * 60f);
        while (Minutes >= 24 * 60) { Minutes -= 24 * 60; Day++; }
    }

    public int Hour => Minutes / 60;
    public int Minute => Minutes % 60;
}