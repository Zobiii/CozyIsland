using Raylib_cs;
using System.Numerics;
using Game.Scenes;
using System.IO.Pipes;

namespace Game;

public sealed class GameApp : IDisposable
{
    private BaseScene _scene = new LoadingScene();
    private bool _isRunning = true;

    public void Run()
    {
        Raylib.InitWindow(1280, 720, "Cozy Island");
        Raylib.SetTargetFPS(60);

        _scene.onEnter();

        while (!Raylib.WindowShouldClose() && _isRunning)
        {
            float dt = Raylib.GetFrameTime();

            _scene.Update(dt);
            var next = _scene.NextScene();
            if (next is not null)
            {
                _scene.onExit();
                _scene = next;
                _scene.onEnter();
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);
            _scene.Draw();
            Raylib.EndDrawing();
        }

        _scene.onExit();
        Dispose();
    }

    public void Dispose()
    {
        Raylib.CloseWindow();
    }
}