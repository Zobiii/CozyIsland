using EngineCore.Math;
using IslandEngine.World;
using IslandEngine.Tiles;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;

namespace IslandEngine.Generation;

public sealed class IslandGenerator
{
    private readonly int _seed;
    private readonly RandomXor _rng;

    public IslandGenerator(int seed)
    {
        _seed = seed;
        _rng = new RandomXor(seed);
    }

    public Island Generate(int width, int height, float scale = 0.02f)
    {
        var island = new Island(width, height, _seed);

        float[,] heightMap = ValueNoise(width, height, scale);
        float[,] moistMap = ValueNoise(width, height, scale * 1.8f);

        ApplyRadialFalloff(heightMap);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                float h = Clamp01(heightMap[x, y]);
                float m = Clamp01(moistMap[x, y]);

                TileType type = Classify(h, m);
                island.SetTile(x, y, new Tile(type, h, m));
            }

        return island;
    }

    private static float[,] ValueNoise(int w, int h, float scale)
    {
        float[,] map = new float[w, h];
        int step = Math.Max(4, (int)(1f / MathF.Max(0.0001f, scale)));

        float[,] grid = new float[(w / step) + 3, (h / step) + 3];
        for (int gy = 0; gy < grid.GetLength(1); gy++)
            for (int gx = 0; gx < grid.GetLength(0); gx++)
                grid[gx, gy] = RandomHash(gx, gy);

        for (int y = 0; y < h; y++)
        {
            int gy0 = (y / step);
            float ty = (y % step) / (float)step;

            for (int x = 0; x < w; x++)
            {
                int gx0 = (x / step);
                float tx = (x % step) / (float)step;

                float a = grid[gx0, gy0];
                float b = grid[gx0 + 1, gy0];
                float c = grid[gx0, gy0 + 1];
                float d = grid[gx0 + 1, gy0 + 1];

                float ab = Lerp(a, b, Smooth(tx));
                float cd = Lerp(c, d, Smooth(tx));
                map[x, y] = Lerp(ab, cd, Smooth(ty));
            }
        }
        return map;
    }

    private static void ApplyRadialFalloff(float[,] map)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);
        float cx = (w - 1) / 2f;
        float cy = (h - 1) / 2f;
        float maxR = MathF.Sqrt(cx * cx + cy * cy);

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                float dx = (x - cx);
                float dy = (y - cy);
                float r = MathF.Sqrt(dx * dx + dy * dy) / maxR;
                float falloff = MathF.Pow(r, 2.2f);
                map[x, y] = map[x, y] * (1f - falloff);
            }
    }

    private static TileType Classify(float h, float m)
    {
        if (h < 0.35f) return h < 0.18f ? TileType.DeepWater : TileType.ShallowWater;
        if (h < 0.42f) return TileType.Beach;
        if (h < 0.70f) return m > 0.55f ? TileType.Forest : TileType.Grass;
        return TileType.Rock;
    }

    private static float Clamp01(float v) => MathF.Max(0f, MathF.Min(1f, v));
    private static float Lerp(float a, float b, float t) => a + (b - a) * t;
    private static float Smooth(float t) => t * t * (3f - 2f * t);

    private static float RandomHash(int x, int y)
    {
        uint h = (uint)(x * 374761393 + y * 668265263); // two big primes
        h = (h ^ (h >> 13)) * 1274126177u;
        return ((h ^ (h >> 16)) & 0x00FFFFFF) / 16777215f;
    }
}