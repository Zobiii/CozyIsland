using EngineCore.Utils;
using IslandEngine.Tiles;

namespace IslandEngine.World;

public sealed class Island
{
    private readonly Array2D<Tile> _tiles;

    public int Width => _tiles.Width;
    public int Height => _tiles.Height;
    public int Seed { get; }

    public Island(int width, int height, int seed)
    {
        _tiles = new Array2D<Tile>(width, height);
        Seed = seed;
    }

    public void SetTile(int x, int y, Tile tile) => _tiles[x, y] = tile;
    public Tile GetTile(int x, int y) => _tiles[x, y];
}