using IslandEngine.Tiles;

namespace IslandEngine.World;

public readonly struct Tile
{
    public TileType Type { get; }
    public float Height { get; }
    public float Moisture { get; }


    public Tile(TileType type, float height, float moisture)
    {
        Type = type;
        Height = height;
        Moisture = moisture;
    }
}