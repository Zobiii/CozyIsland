namespace EngineCore.Math;

public sealed class RandomXor
{
    private ulong _state;

    public RandomXor(int seed) => _state = (ulong)seed * 0x9E3779B97F4A7C15ul + 0xBF58476D1CE4E5B9ul;

    private ulong NextU64()
    {
        _state ^= _state >> 12;
        _state ^= _state << 25;
        _state ^= _state >> 27;
        return _state * 0x2545F4914F6CDD1Dul;
    }

    public float NextFloat() => (NextU64() >> 40) / (float)(1ul << 24);
    public int NextInt(int min, int max) => min + (int)(NextFloat() * (max - min));
}