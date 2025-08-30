using Raylib_cs;
using IslandEngine.World;
using IslandEngine.Tiles;
using System.Numerics;

namespace Game.Scenes;

public class WorldScene : BaseScene
{
    private readonly Island _island;
    private Vector2 _camera = new(0, 0);
    private float _zoom = 2f; // Tilegröße * Zoom

    public WorldScene(Island island)
    {
        _island = island;
    }

    public override void onEnter() { }

    public override void Update(float dt)
    {
        // Simple Kamera-Steuerung (WASD + Mausrad)
        float speed = 300f / _zoom;
        if (Raylib.IsKeyDown(KeyboardKey.W)) _camera.Y -= speed * dt;
        if (Raylib.IsKeyDown(KeyboardKey.S)) _camera.Y += speed * dt;
        if (Raylib.IsKeyDown(KeyboardKey.A)) _camera.X -= speed * dt;
        if (Raylib.IsKeyDown(KeyboardKey.D)) _camera.X += speed * dt;

        float wheel = Raylib.GetMouseWheelMove();
        if (wheel != 0) _zoom = Math.Clamp(_zoom + wheel * 0.2f, 0.5f, 6f);
    }

    public override void Draw()
    {
        // Tile-Rendering (farbige Quads je Biome)
        int tileSize = (int)(6 * _zoom);
        for (int y = 0; y < _island.Height; y++)
        {
            for (int x = 0; x < _island.Width; x++)
            {
                var tile = _island.GetTile(x, y);
                var color = tile.Type switch
                {
                    TileType.DeepWater => new Color(24, 78, 119, 255),
                    TileType.ShallowWater => new Color(54, 129, 168, 255),
                    TileType.Beach => new Color(235, 214, 154, 255),
                    TileType.Grass => new Color(108, 162, 92, 255),
                    TileType.Forest => new Color(64, 112, 75, 255),
                    TileType.Rock => new Color(120, 120, 120, 255),
                    _ => Color.Magenta
                };

                int px = (int)(x * tileSize - _camera.X);
                int py = (int)(y * tileSize - _camera.Y);
                Raylib.DrawRectangle(px, py, tileSize, tileSize, color);
            }
        }

        Raylib.DrawRectangle(12, 12, 300, 60, new Color(0, 0, 0, 160));
        Raylib.DrawText($"Seed: {_island.Seed}", 20, 20, 20, Color.RayWhite);
        Raylib.DrawText($"Zoom: {_zoom:0.00}", 20, 44, 20, Color.RayWhite);
    }

    public override void onExit() { }
}
